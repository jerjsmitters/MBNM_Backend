using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Api
{
    public record GenericEnum
    {
        public string Name { get; init; }
        public int Value { get; init; }
    }
}
