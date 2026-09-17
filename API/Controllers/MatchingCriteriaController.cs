using Common.Dto.Api;
using Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchingCriteriaController
    {
        [HttpGet("modes")]
        public ActionResult<List<GenericEnum>> GetMatchingCriteriaModes()
        {
            return Enum
                .GetValues<MatchingCriteriaMode>()
                .Select(x => new GenericEnum
                {
                    Name = x.ToString(),
                    Value = (int)x
                })
                .ToList();
        }
    }
}
