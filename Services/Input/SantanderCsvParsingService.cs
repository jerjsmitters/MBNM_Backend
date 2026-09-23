using Common.Dto.Input;
using Services.Interfaces.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Input
{
    public class SantanderCsvParsingService : IParsingService
    {
        private const string EMPTY_DATA_ERROR_MSG = "No data provided";
        private const string PARSING_ERROR_MSG = "Unable to parse transactions from data";
        private const int DATE_CSV_INDEX = 0;
        private const int REFERENCE_CSV_INDEX = 1;
        private const int AMOUNT_CSV_INDEX = 2;
        private const string DATETIME_FORMAT = "yyyy-MM-dd";

        public InputTransactionsDto ParseInputData(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentNullException(nameof(data), EMPTY_DATA_ERROR_MSG);
            }

            var transactionLines = ParseCsvIntoTransactionLines(data);

            if (transactionLines.Count == 0)
            {
                throw new ArgumentException(nameof(transactionLines), PARSING_ERROR_MSG);
            }

            var mappedCsvLines = MapCsvLinesToInputTransactionsDto(transactionLines);

            return mappedCsvLines;
        }

        private List<string[]> ParseCsvIntoTransactionLines(string csvContent)
        {    
            return csvContent
                .Replace("\"", string.Empty)
                .Split("\r\n")
                .Select(csv => csv.Split(","))
                .ToList();
        }

        private InputTransactionsDto MapCsvLinesToInputTransactionsDto(List<string[]> transactionLines)
        {
            InputTransactionsDto mappedLines = new(DateTime.UtcNow, []);
            InputTransactionDto mappedLine;

            var sequence = 0;
            foreach (var transactionLine in transactionLines)
            {
                mappedLine = new(
                     DateOnly.ParseExact(transactionLine[DATE_CSV_INDEX], DATETIME_FORMAT),
                     sequence,
                     decimal.Parse(transactionLine[AMOUNT_CSV_INDEX]),
                     transactionLine[REFERENCE_CSV_INDEX]
                );

                mappedLines.Transactions.Add(mappedLine);
                sequence++;
            }

            return mappedLines;

        }
    }
}
