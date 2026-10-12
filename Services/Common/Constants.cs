using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
    internal static class Constants
    {
        public const string FileRootLocation = "C:\\FileStore\\MBNM\\Uploads";
        public const string OutputDateFormat = "DD-MM-yyyy";

        /// <summary>
        /// startdate, enddate, guid
        /// </summary>
        public const string OutputFileNameTemplate = "ExportFile_{0}_to_{1}_{2}.txt"; 

        /// <summary>
        /// date, sequence, amount, balance, properties, categories
        /// </summary>
        public const string OutputLineFormat = @"{0},{1},{2},{3},{4},{5}"; 
    }
}
