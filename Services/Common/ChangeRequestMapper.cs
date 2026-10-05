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
        public ChangeRequest MapDtoToEntity(ChangeRequestDto changeRequest)
        {
            var createLines = changeRequest.CreateLines.Select(MapToCreateLineEntity).ToList();
            var deleteLines = changeRequest.DeleteLines.Select(MapToDeleteLineEntity).ToList();
            return new()
            {
                Id = changeRequest.Id,
                CreateLines = createLines,
                DeleteLines = deleteLines,

                CreatedOn = changeRequest.CreatedOn,
                UpdatedOn = changeRequest.UpdatedOn,
                IsDeleted = changeRequest.IsDeleted
            };
        }

        public ChangeRequestDto MapEntityToDto(ChangeRequest changeRequest)
        {
            var createLines = changeRequest.CreateLines.Select(MapToCreateLineDto).ToList();
            var deleteLines = changeRequest.DeleteLines.Select(MapToDeleteLineDto).ToList();
            return new()
            {
                Id = changeRequest.Id,
                CreateLines = createLines,
                DeleteLines = deleteLines,

                CreatedOn = changeRequest.CreatedOn,
                UpdatedOn = changeRequest.UpdatedOn,
                IsDeleted = changeRequest.IsDeleted
            };
        }

        public Transaction ProjectTransactionChangeRequestLineToEntity(TransactionDataChangeRequestLine changeRequestLine, Transaction transaction)
        {
            transaction.Sequence = changeRequestLine.Sequence;
            transaction.Reference = changeRequestLine.Reference;
            transaction.Amount = changeRequestLine.Amount;
            transaction.AssignedCategories = changeRequestLine.TransactionCategories;
            transaction.AssignedProperties = changeRequestLine.TransactionProperties;
            transaction.Date = changeRequestLine.Date;

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
            ProposedCategories = createLine.TransactionCategories?.Select(transactionCategoryMapper.MapEntityToDto).ToList() ?? new(),
            ProposedProperties = createLine.TransactionProperties?.Select(transactionPropertyMapper.MapEntityToDto).ToList() ?? new(),

            CreatedOn = createLine.CreatedOn,
            UpdatedOn = createLine.UpdatedOn,
            IsDeleted = createLine.IsDeleted
        };

        private ChangeRequestDeleteLineDto MapToDeleteLineDto(ChangeRequestDeleteLine deleteLine) => new()
        {
            Id = deleteLine.Id,
            TransactionToDelete = transactionMapper.MapEntityToDto(deleteLine.Transaction),
            RequestState = deleteLine.RequestState,

            CreatedOn = deleteLine.CreatedOn,
            UpdatedOn = deleteLine.UpdatedOn,
            IsDeleted = deleteLine.IsDeleted
        };

        private ChangeRequestCreateLine MapToCreateLineEntity(ChangeRequestCreateLineDto createLine) => new()
        {
            Id = createLine.Id,
            RequestState = createLine.RequestState,
            Amount = createLine.ProposedAmount,
            Date = createLine.ProposedDate,
            Sequence = createLine.ProposedSequence,
            Reference = createLine.ProposedReference,

            TransactionCategories = createLine.ProposedCategories?.Select(transactionCategoryMapper.MapDtoToEntity).ToList() ?? [], //UNTRACKED
            TransactionProperties = createLine.ProposedProperties?.Select(transactionPropertyMapper.MapDtoToEntity).ToList() ?? [],  //UNTRACKED

            CreatedOn = createLine.CreatedOn,
            UpdatedOn = createLine.UpdatedOn,
            IsDeleted = createLine.IsDeleted
        };

        private ChangeRequestDeleteLine MapToDeleteLineEntity(ChangeRequestDeleteLineDto deleteLine) => new()
        {
            Id = deleteLine.Id,
            RequestState = deleteLine.RequestState,
            Transaction = transactionMapper.MapDtoToEntity(deleteLine.TransactionToDelete),

            CreatedOn = deleteLine.CreatedOn,
            UpdatedOn = deleteLine.UpdatedOn,
            IsDeleted = deleteLine.IsDeleted
        };
    }
}
