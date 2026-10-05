using Common.Dto.Base;
using Common.Dto.ChangeRequest;
using Common.Dto.Criteria;
using DataAccess.Models.Base;
using DataAccess.Models.ChangeRequestEntities;
using DataAccess.Models.Criteria;
using Services.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
    public class MatchingCriteriaMapper() : IMatchingCriteriaMapper
    {
        public MatchingCriteria MapDtoToEntity(MatchingCriteriaDto matchingCriteria)
        {
            return new()
            {
                Id = matchingCriteria.Id,
                CreatedOn = matchingCriteria.CreatedOn,
                UpdatedOn = matchingCriteria.UpdatedOn,
                MatchingCriteriaMode = matchingCriteria.MatchingCriteriaMode,
                SearchText = matchingCriteria.SearchText,
                IsDeleted = matchingCriteria.IsDeleted 
                //no need to map categories and properties as they own the relationship
            };
        }


        public MatchingCriteriaDto MapEntityToDto(MatchingCriteria matchingCriteria)
        {
            return new()
            {
                Id = matchingCriteria.Id,
                MatchingCriteriaMode = matchingCriteria.MatchingCriteriaMode,
                SearchText = matchingCriteria.SearchText,

                CreatedOn = matchingCriteria.CreatedOn,
                UpdatedOn = matchingCriteria.UpdatedOn,
                IsDeleted = matchingCriteria.IsDeleted
                //no need to map categories and properties as they own the relationship

            };
        }

        public MatchingCriteria ProjectDtoToEntity(MatchingCriteriaDto incoming, MatchingCriteria entity)
        {
            entity.Id = incoming.Id;
            entity.MatchingCriteriaMode = incoming.MatchingCriteriaMode;
            entity.SearchText = incoming.SearchText;

            return entity;
        }

        public ENT ProjectDtoToEntityWithCriteria<ENT,DTO>(DTO incoming, ENT entity) 
            where ENT : EntityWithCriteria 
            where DTO : EntityWithCriteriaDto
        {
            var existingCriteria = entity.CriteriaToMatchOn.Where(entCrit => entCrit.Id.HasValue && incoming.CriteriaToMatchOn.Any(incCrit => incCrit.Id.HasValue && incCrit.Id.Value == entCrit.Id.Value));
            var existingCriteriaDtos = incoming.CriteriaToMatchOn.Where(incCrit => existingCriteria.Any(ex => incCrit.Id.Value == ex.Id.Value));
            foreach (var existingCriterium in existingCriteria)
            {
                var existingCriteriaDto = existingCriteriaDtos.FirstOrDefault(exCrit => exCrit.Id.HasValue && exCrit.Id == existingCriterium.Id.Value);
                ProjectDtoToEntity(existingCriteriaDto, existingCriterium);
            }


            //add new
            var newCriteriaDto = incoming.CriteriaToMatchOn.Where(incCrit => !incCrit.Id.HasValue);
            foreach (var newCriteriumDto in newCriteriaDto)
            {
                entity.CriteriaToMatchOn.Add(
                    MapDtoToEntity(newCriteriumDto)
                );
            }


            //delete 
            entity.CriteriaToMatchOn.RemoveAll(ent => ent.Id.HasValue && !incoming.CriteriaToMatchOn.Any(inc => inc.Id.HasValue && inc.Id.Value == ent.Id.Value));

            return entity;
        }
    }
}
