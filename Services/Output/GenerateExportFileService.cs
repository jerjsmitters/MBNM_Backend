using Common.Dto.Filters;
using Common.Dto.Transactions;
using Services.Common;
using Services.Interfaces.Output;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Output
{
    public class GenerateExportFileService : IGenerateExportFileService
    {
        public async Task<FileInfo> GenerateExportFile(TransactionEntityFilter filtersUsedToGetTransactions, List<TransactionDto> transactions)
        {
            var fileName = GenerateFileName(filtersUsedToGetTransactions);
            var fileText = GenerateExportFileContents(transactions);
            var exportFile = await CreateExportFile(fileText, fileText);

            return exportFile;
        }

        
        //      Generating actual file
        private string GenerateExportFileContents(List<TransactionDto> transactions)
        {
            var stringBuilder = new StringBuilder();
            foreach (var transaction in transactions) 
            {
                var line = GenerateTransactionLine(transaction);
                stringBuilder.AppendLine(line);
            }

            return stringBuilder.ToString();
        }

        private async Task<FileInfo> CreateExportFile(string fileName, string fileContents)
        {
            var filePath = Path.Combine(Constants.FileRootLocation, fileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);
            using StreamWriter writer = new(fileStream);

            writer.Write(fileContents);

            return new FileInfo(filePath);
        }


        //      string ops
        private string GenerateFileName(TransactionEntityFilter filtersUsedToGetTransactions) =>
            string.Format(
                Constants.OutputFileNameTemplate,
                filtersUsedToGetTransactions.StartDate,
                filtersUsedToGetTransactions.EndDate,
                Guid.NewGuid().ToString()
            );

        private string GenerateTransactionLine(TransactionDto transaction)
        {
            var categoriesArray = transaction.Categories.Select(x => x.Description).ToArray();
            var categories = string.Join("/", categoriesArray);

            var propertiesArray = transaction.Categories.Select(x => x.Description).ToArray();
            var properties = string.Join("/", propertiesArray);

            return string.Format(
                Constants.OutputLineFormat,
                transaction.Date.ToString(Constants.OutputDateFormat),
                transaction.Sequence.ToString(),
                "£" + transaction.Balance.ToString(),
                properties,
                categories
            );
        }

    }
}
