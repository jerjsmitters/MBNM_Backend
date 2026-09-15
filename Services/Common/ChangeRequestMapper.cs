using Common.Dto.ChangeRequest;
using Common.Dto.ChangeRequest.Line;
using DataAccess.Models.Base;
using DataAccess.Models.ChangeRequestEntities;
using DataAccess.Models.ChangeRequestEntities.Line;
using DataAccess.Models.Transactions;
using Services.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
    public class ChangeRequestMapper(ITransactionMapper transactionMapper, ITransactionPropertyMapper transactionPropertyMapper, ITransactionCategoryMapper transactionCategoryMapper) : IChangeRequestMapper
    {
        public ChangeRequest MapDtoToChangeRequest(ChangeRequestDto changeRequest)
        {
            var createLines = changeRequest.CreateLines.Select(MapToCreateLineEntity).ToList();
            var updateLines = changeRequest.UpdateLines.Select(MapToUpdateLineEntity).ToList();
            var deleteLines = changeRequest.DeleteLines.Select(MapToDeleteLineEntity).ToList();
            return new()
            {
                Id = changeRequest.Id,
                CreatedOn = changeRequest.CreatedOn,
                UpdatedOn = changeRequest.UpdatedOn,
                CreateLines = createLines,
                UpdateLines = updateLines,
                DeleteLines = deleteLines
            };
        }

        public ChangeRequestDto MapChangeRequestToDto(ChangeRequest changeRequest)
        {
            var createLines = changeRequest.CreateLines.Select(MapToCreateLineDto).ToList();
            var updateLines = changeRequest.UpdateLines.Select(MapToUpdateLineDto).ToList();
            var deleteLines = changeRequest.DeleteLines.Select(MapToDeleteLineDto).ToList();
            return new()
            {
                Id = changeRequest.Id,
                CreatedOn = changeRequest.CreatedOn,
                UpdatedOn = changeRequest.UpdatedOn,
                CreateLines = createLines,
                UpdateLines = updateLines,
                DeleteLines = deleteLines
            };
        }

        public Transaction ProjectChangeRequestLineToTransaction(TransactionDataChangeRequestLine changeRequestLine, Transaction transaction)
        {
            transaction.Sequence = changeRequestLine.Sequence;
            transaction.Reference = changeRequestLine.Reference;
            transaction.Amount = changeRequestLine.Amount;
            transaction.AssignnedCategories = changeRequestLine.TransactionCategories;
            transaction.AssignedProperties = changeRequestLine.TransactionProperties;

            return transaction;
        }




        private ChangeRequestCreateLineDto MapToCreateLineDto(ChangeRequestCreateLine createLine) => new()
        {
            Id = createLine.Id,
            RequestState = createLine.RequestState,
            ProposedAmount = createLine.Amount,
            ProposedDate = createLine.Date,
            ProposedSequence = createLine.Sequence,
            ProposedReference = createLine.Reference,
            CreatedOn = createLine.CreatedOn,
            UpdatedOn = createLine.UpdatedOn
        };

        private ChangeRequestUpdateLineDto MapToUpdateLineDto(ChangeRequestUpdateLine updateLine) => new()
        {
            Id = updateLine.Id,
            RequestState = updateLine.RequestState,
            ProposedAmount = updateLine.Amount,
            ProposedDate = updateLine.Date,
            ProposedSequence = updateLine.Sequence,
            ProposedReference = updateLine.Reference,
            CreatedOn = updateLine.CreatedOn,
            UpdatedOn = updateLine.UpdatedOn,
            TransactionToUpdate = transactionMapper.MapTransactionToDto(updateLine.Transaction)
        };

        private ChangeRequestDeleteLineDto MapToDeleteLineDto(ChangeRequestDeleteLine deleteLine) => new()
        {
            Id = deleteLine.Id,
            TransactionToDelete = transactionMapper.MapTransactionToDto(deleteLine.Transaction),
            RequestState = deleteLine.RequestState,
            CreatedOn = deleteLine.CreatedOn,
            UpdatedOn = deleteLine.UpdatedOn
        };

        private ChangeRequestCreateLine MapToCreateLineEntity(ChangeRequestCreateLineDto createLine) => new()
        {
            Id = createLine.Id,
            RequestState = createLine.RequestState,
            Amount = createLine.ProposedAmount,
            Date = createLine.ProposedDate,
            Sequence = createLine.ProposedSequence,
            Reference = createLine.ProposedReference,
            CreatedOn = createLine.CreatedOn,
            UpdatedOn = createLine.UpdatedOn,
            TransactionCategories = createLine.ProposedCategories?.Select(transactionCategoryMapper.MapDtoToCategory).ToList() ?? [],
            TransactionProperties = createLine.ProposedProperties?.Select(transactionPropertyMapper.MapDtoToProperty).ToList() ?? []
        };

        private ChangeRequestUpdateLine MapToUpdateLineEntity(ChangeRequestUpdateLineDto updateLine) => new()
        {
            Id = updateLine.Id,
            RequestState = updateLine.RequestState,
            Amount = updateLine.ProposedAmount,
            Date = updateLine.ProposedDate,
            Sequence = updateLine.ProposedSequence,
            Reference = updateLine.ProposedReference,
            CreatedOn = updateLine.CreatedOn,
            UpdatedOn = updateLine.UpdatedOn,
            Transaction = transactionMapper.MapDtoToTransaction(updateLine.TransactionToUpdate),
            TransactionCategories = updateLine.ProposedCategories?.Select(transactionCategoryMapper.MapDtoToCategory).ToList() ?? [],
            TransactionProperties = updateLine.ProposedProperties?.Select(transactionPropertyMapper.MapDtoToProperty).ToList() ?? []
        };

        private ChangeRequestDeleteLine MapToDeleteLineEntity(ChangeRequestDeleteLineDto deleteLine) => new()
        {
            Id = deleteLine.Id,
            RequestState = deleteLine.RequestState,
            CreatedOn = deleteLine.CreatedOn,
            UpdatedOn = deleteLine.UpdatedOn,
            Transaction = transactionMapper.MapDtoToTransaction(deleteLine.TransactionToDelete)
        };
    }
}
