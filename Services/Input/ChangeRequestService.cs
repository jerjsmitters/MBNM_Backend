using Common.Dto.Api;
using Common.Dto.Base;
using Common.Dto.ChangeRequest;
using Common.Dto.ChangeRequest.Line;
using Common.Dto.Criteria;
using Common.Dto.Input;
using Common.Dto.Transactions;
using Common.Enums;
using Common.Extensions;
using DataAccess.EF.DbContextImpl;
using DataAccess.Models.Base;
using DataAccess.Models.ChangeRequestEntities;
using DataAccess.Models.ChangeRequestEntities.Line;
using DataAccess.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Common;
using Services.Interfaces.Input;
using Services.Interfaces.Transactions;
using System.Collections.Immutable;


namespace Services.Input
{
    public class ChangeRequestService(
        EfDbContext dbContext, 
        IGenerateChangeRequestService generateChangeRequestService,
        IChangeRequestMapper changeRequestMapper,
        ITransactionMapper transactionMapper,
        ITransactionService transactionService) : IChangeRequestService
    {

        public async Task<ChangeRequestDto?> GetForApproval(int id)
        {
            var changeRequest = await GetInternal(id);

            if (changeRequest == null)
            {
                return null;
            }

            return changeRequestMapper.MapEntityToDto(changeRequest);
        }

        public async Task<List<ChangeRequestDto>> GetAll()
        {

            var changeRequest = await dbContext
                .ChangeRequests
                .Where(x => !x.IsDeleted)
                .ToListAsync();

            if (!changeRequest.Any())
            {
                return [];
            }

            return changeRequest
                .Select(changeRequestMapper.MapEntityToDto)
                .ToList();
        }

        public async Task<ChangeRequestDto> GenerateChangeRequestFromInputTransactions(InputTransactionsDto dataInput)
        {
            if (dataInput == null || (dataInput.Transactions?.Count ?? 0) == 0)
            {
                return new ChangeRequestDto();
            }

            var dateRange = GetDateRangeOfInputTransactions(dataInput.Transactions);

            List<TransactionDto> persistedTransactionsForDateRange = GetTransactionsForDateRange(dateRange.Item1, dateRange.Item2);

            var proposedTransactionChangeLog = generateChangeRequestService.GenerateChangeRequest(dataInput.Transactions, persistedTransactionsForDateRange);

            if (proposedTransactionChangeLog.NothingToDo)
            {
                return proposedTransactionChangeLog;
            }

            proposedTransactionChangeLog = await SaveChangeRequest(proposedTransactionChangeLog);

            return proposedTransactionChangeLog;
        }

        //approval process
        public async Task<List<ChangeRequestCommitResultDto>> ApplyChangeRequest(CommitChangeRequestDto applyChangeRequestInfo)
        {
            await using var efDbTransaction = await dbContext.Database.BeginTransactionAsync();

            var results = new List<ChangeRequestCommitResultDto>();
            ChangeRequestCommitResultDto result;


            var changeRequest = await GetInternal(applyChangeRequestInfo.ChangeRequestId)
                ?? throw new InvalidOperationException($"Change Request with Id = {applyChangeRequestInfo.ChangeRequestId} doesn't exist.");

            var changeRequestLines = GetChangeRequestLines(changeRequest);
            if (NothingToProcess(changeRequestLines))
            {
                return [];
            }

            //add requests
            foreach (ChangeRequestCreateLine addRequest in changeRequest.CreateLines ?? [])
            {
                if (NeedToIgnoreLine(applyChangeRequestInfo.CreateLines, addRequest))
                {
                    addRequest.RequestState = ChangeRequestState.DENIED;
                    continue;
                }

                var transactionToAdd = ProjectChangeRequestLineToTransaction(addRequest);
                var addResult = dbContext.Transactions.Add(transactionToAdd);

                addRequest.RequestState = ChangeRequestState.COMMITED;

                await dbContext.SaveChangesAsync(); //ideally this should be called once at the end of loop, but I need its ID + this is a small private application that doesnt require such an elegant solution

                result = new ChangeRequestCommitResultDto
                (
                    null,
                    transactionMapper.MapEntityToDto(addResult.Entity),
                    ChangeRequestCommitOperation.ADD
                );

                results.Add(result);
            }

            //delete requests 
            foreach (var deleteRequest in changeRequest.DeleteLines ?? [])
            {
                if (NeedToIgnoreLine(applyChangeRequestInfo.DeleteLines, deleteRequest))
                {
                    deleteRequest.RequestState = ChangeRequestState.DENIED;
                    continue;
                }

                deleteRequest.Transaction.IsDeleted = true;
                deleteRequest.RequestState = ChangeRequestState.COMMITED;

                result = new ChangeRequestCommitResultDto (
                    transactionMapper.MapEntityToDto(deleteRequest.Transaction),
                    null,
                    ChangeRequestCommitOperation.DELETE
                );

                results.Add(result);
            }

            await dbContext.SaveChangesAsync();

            //only recalc if everything wasn't ignored
            if (results.Any())
            {
                var allTransactionBeforeIds = results.Select(x => x.TransactionBefore?.Id).Where(id => id != null);
                var allTransactionAfterIds = results.Select(x => x.TransactionAfter?.Id).Where(id => id != null);
                var allModifiedTransactionIds = allTransactionBeforeIds.Union(allTransactionAfterIds).ToList().ConvertAll(x => (int)x).Distinct();

                await transactionService.RecalculateBalances(allModifiedTransactionIds.ToList());
            }

            await efDbTransaction.CommitAsync();

            return results;
        }






        // ========================== PRIVATE FNS ===========================//

        ///FOR ONE DAY ONLY
        

        private async Task<ChangeRequestDto> SaveChangeRequest(ChangeRequestDto changeRequest)
        {
            if (changeRequest == null || GetChangeRequestLineDtos(changeRequest).Count == 0)
            {
                throw new ArgumentException("No change request lines" ,nameof(changeRequest));
            }

            var changeRequestEntity = changeRequestMapper.MapDtoToEntity(changeRequest);

            //attach the untracked many-to-many properties
            changeRequestEntity = AttachManyToNavigationProperties(changeRequestEntity);

            var savedRequest = await dbContext.AddAsync(changeRequestEntity);
            await dbContext.SaveChangesAsync();

            return changeRequestMapper.MapEntityToDto(savedRequest.Entity);
        }

        // =========== DATA ACCESS  ===============//
        private Task<ChangeRequest?> GetInternal(int id)
        {
            return dbContext
                .ChangeRequests
                .FirstOrDefaultAsync(cr => cr.Id == id);

        }

        private List<TransactionDto> GetTransactionsForDateRange(DateOnly startDate, DateOnly endDate)
        {
            return dbContext
                .Transactions
                .Where(t => t.Date <= startDate && t.Date >= endDate)
                .Where(t => !t.IsDeleted)
                .OrderBy(t => t.Date)
                .ThenBy(t => t.Sequence)
                .AsNoTracking()
                .Select(t => transactionMapper.MapEntityToDto(t))
                .ToList();
        }

        private ChangeRequest AttachManyToNavigationProperties(ChangeRequest changeRequest)
        {
            //properties
            var propertyIds = changeRequest
                .CreateLines
                .SelectMany(x => x.TransactionProperties)
                .Select(x => x.Id)
                .Distinct();

            var properties = dbContext.TransactionProperties.Where(x => propertyIds.Contains(x.Id)).ToList();


            //categories
            var categoryIds = changeRequest
                .CreateLines
                .SelectMany(x => x.TransactionCategories)
                .Select(x => x.Id);

            var categories = dbContext.TransactionCategories.Where(x => categoryIds.Distinct().Contains(x.Id)).ToList();

            foreach (var createLine in changeRequest.CreateLines)
            {
                createLine.TransactionProperties = properties.Where(ap => createLine.TransactionProperties.Any(cltp => ap.Id == cltp.Id)).ToList();
                createLine.TransactionCategories = categories.Where(ac => createLine.TransactionCategories.Any(cltc => ac.Id == cltc.Id)).ToList();
            }

            var transactionIds = changeRequest.DeleteLines.Select(x => x.Transaction.Id).Distinct();
            var transactions = dbContext.Transactions.Where(x => transactionIds.Contains(x.Id));

            foreach (var deleteLine in changeRequest.DeleteLines)
            {
                deleteLine.Transaction = transactions.FirstOrDefault(ac => deleteLine.Transaction.Id == ac.Id) ?? throw new InvalidOperationException("cant find transaction to attach to delete line");
            }

            return changeRequest;
        }


        // ============ HELPER FNS ================//
        private Transaction ProjectChangeRequestLineToTransaction(TransactionDataChangeRequestLine changeRequestLine) => changeRequestMapper.ProjectTransactionChangeRequestLineToEntity(changeRequestLine, new Transaction());

        private static bool NothingToProcess(List<ChangeRequestLine> requestLines)
        {
            if (requestLines.Count == 0)
            {
                return true;
            }

            return !requestLines.Any(crl => crl.RequestState == ChangeRequestState.PENDING);
        }

        private static List<ChangeRequestLine> GetChangeRequestLines(ChangeRequest changeRequest)
        {
            return [
                .. changeRequest?.CreateLines ?? [],
                .. changeRequest?.DeleteLines ?? []
            ];
        }

        private static List<ChangeRequestLineDto> GetChangeRequestLineDtos(ChangeRequestDto changeRequest)
        {
            return [
                .. changeRequest?.CreateLines ?? [],
                .. changeRequest?.DeleteLines ?? []
            ];
        }

        private static bool NeedToIgnoreLine(List<ApplyChangeRequestLineDto> applyChangeRequestLines, ChangeRequestLine changeRequestLine)
        {
            var matchingLine = applyChangeRequestLines.FirstOrDefault(x => x.LineId == changeRequestLine.Id) ?? throw new Exception("Cannot find line");
            return matchingLine.DesiredState == ChangeRequestState.DENIED;

        }


        private static Tuple<DateOnly, DateOnly> GetDateRangeOfInputTransactions(List<InputTransactionDto> inputs)
        {
            var startDate = inputs.Max(i => i.Date);
            var endDate = inputs.Min(i => i.Date);

            return Tuple.Create(startDate, endDate);
        }


       

    }
}
