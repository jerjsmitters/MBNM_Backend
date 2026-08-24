using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Extensions
{
    public static class ListHelpers
    {
        public static bool IsNullOrEmpty<T>(List<T> list)
        {
            return list == null || (list?.Count ?? 0) == 0;
        }
    }
}
