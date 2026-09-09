using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Input
{
    public interface IInputService
    {
        Task AddTransactions(string inputData);
    }
}
