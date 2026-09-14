using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Base
{
    public abstract class EntityBase
    {
        public int? Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
