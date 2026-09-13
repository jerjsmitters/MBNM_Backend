using Common.Dto.ChangeRequest;
using Common.Dto.ChangeRequest.Line;
using Common.Dto.Transactions;
using DataAccess.Models.ChangeRequestEntities;
using DataAccess.Models.ChangeRequestEntities.Line;
using DataAccess.Models.Transactions;
using Services.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
    public class MappingService : IMappingService
    {
        public TransactionDto MapTransactionToDto(Transaction transaction)
        {
            return new TransactionDto
            {
                Id = transaction.Id,
                CreatedOn = transaction.CreatedOn,
                Date = transaction.Date,
                Amount = transaction.Amount
            };
        }



        #region Change Request
        public ChangeRequest MapChangeRequestDtoToEntity(ChangeRequestDto changeRequest)
        {
            var createLines = changeRequest.CreateLines.Select(x => MapToCreateLineEntity(x)).ToList();
            var updateLines = changeRequest.UpdateLines.Select(x => MapToUpdateLineEntity(x)).ToList();
            var deleteLines = changeRequest.DeleteLines.Select(x => MapToDeleteLineEntity(x)).ToList();
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
            var createLines = changeRequest.CreateLines.Select(x => MapToCreateLineDto(x)).ToList();
            var updateLines = changeRequest.UpdateLines.Select(x => MapToUpdateLineDto(x)).ToList();
            var deleteLines = changeRequest.DeleteLines.Select(x => MapToDeleteLineDto(x)).ToList();
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

        private ChangeRequestCreateLineDto MapToCreateLineDto(ChangeRequestCreateLine createLine)
        {
            return null;

        }

        private ChangeRequestUpdateLineDto MapToUpdateLineDto(ChangeRequestUpdateLine updateLine)
        {
            return null;

        }
        private ChangeRequestDeleteLineDto MapToDeleteLineDto(ChangeRequestDeleteLine updateLine)
        {
            return null;

        }

        private ChangeRequestDeleteLine MapToDeleteLineEntity(ChangeRequestDeleteLine deleteLine)
        {
            return null;

        }

        private ChangeRequestCreateLine MapToCreateLineEntity(ChangeRequestCreateLineDto createLine)
        {
            return null;

        }

        private ChangeRequestUpdateLine MapToUpdateLineEntity(ChangeRequestUpdateLineDto updateLine)
        {
            return null;

        }

        private ChangeRequestDeleteLine MapToDeleteLineEntity(ChangeRequestDeleteLineDto deleteLine)
        {
            return null;

        }

        #endregion


        public TransactionCategoryDto MapCategoryToDto(TransactionCategory category)
        {
            return new()
            {
                CreatedOn = category.CreatedOn,
                Id = category.Id,
                UpdatedOn = category.UpdatedOn,
                Description = category.Description,

            };
        }



        public TransactionPropertyDto MapPropertyToDto(TransactionProperty property)
        {
            return new()
            {
                CreatedOn = property.CreatedOn,
                Id = property.Id,
                UpdatedOn = property.UpdatedOn,
                Description = property.Description
            };
        }
    }
}
