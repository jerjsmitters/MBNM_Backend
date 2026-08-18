using Common.Dto.Input;
using Services.Common.Interfaces;

namespace Services.Common
{
    public class TransactionService : ITransactionService
    {
        public void AddTransactions(InputTransactions dataInput)
        {
            //get start and end date of the input transactions
            
            //get all persisted transactions for date range (given start and end transaction, also include other transactions before start and after end that have same matching criteria)

            //GetProposedTransactionChangeLog(dataInput, persistedTransactions) (implementation added in below)

            //ProposedTransactionChangeLog.SaveProposedChanges()

            //Return ProposedChanges via DTO

        }
    }
}
