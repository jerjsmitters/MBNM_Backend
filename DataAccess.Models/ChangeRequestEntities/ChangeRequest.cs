using Common.Enums;
using DataAccess.Models.Base;
using DataAccess.Models.ChangeRequestEntities.Line;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.ChangeRequestEntities
{
    public class ChangeRequest : EntityBase
    {
        public List<ChangeRequestCreateLine> CreateLines { get; set; }
        public List<ChangeRequestUpdateLine> UpdateLines { get; set; }
        public List<ChangeRequestDeleteLine> DeleteLines { get; set; }
    }
}
