using DataAccess.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Output
{
    public class ExportFile : EntityBase
    {
        public string FilePath { get; set; }
    }
}
