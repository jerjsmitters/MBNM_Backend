using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Base
{
    public abstract record EntityBaseDto
    {
        public int? Id { get; init; }
        public DateTime? CreatedOn { get; init; }
        public DateTime? UpdatedOn { get; init; }
        public bool IsDeleted { get; set; }

    }
}
