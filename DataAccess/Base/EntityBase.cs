using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities.Base
{
    public abstract class EntityBase
    {
        public int? Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
