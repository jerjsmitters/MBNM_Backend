using Common.Dto.Base;
using Common.Dto.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
    internal static class ValidationService
    {
        public static void ThrowIfInvalidForCreate<T>(T dtoToCreateWith) where T : EntityWithCriteriaDto
        {
            string errorMsg = null;
            if (dtoToCreateWith.Id.HasValue)
            {
                errorMsg = "Cannot add an entity that already has an ID";
            }
            else if (string.IsNullOrWhiteSpace(dtoToCreateWith.Description))
            {
                errorMsg = "Needs a description";

                throw new ArgumentException("");
            }
            else if (dtoToCreateWith.CriteriaToMatchOn == null || !dtoToCreateWith.CriteriaToMatchOn.Any())
            {
                errorMsg = "Needs Criteria";
            }

            if (errorMsg != null)
            {
                throw new ArgumentException(errorMsg);
            }
        }

        public static void ThrowIfInvalidForUpdate<T>(T dtoToUpdateWith) where T : EntityWithCriteriaDto
        {
            string errorMsg = null;

            if (!dtoToUpdateWith.Id.HasValue)
            {
                errorMsg = "Cannot update an entity without an ID";
            }
            else if (string.IsNullOrWhiteSpace(dtoToUpdateWith.Description))
            {
                errorMsg = "Needs a description";

                throw new ArgumentException("");
            }
            else if (dtoToUpdateWith.CriteriaToMatchOn == null || !dtoToUpdateWith.CriteriaToMatchOn.Any())
            {
                errorMsg = "Needs Criteria";
            }

            if (errorMsg != null)
            {
                throw new ArgumentException(errorMsg);
            }
        }
    }
}
