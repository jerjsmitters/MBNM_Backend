using Common.Dto.Input;
using Services.Interfaces.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Input
{
    public class SantanderCsvParsingService : ISantanderCsvParsingService
    {
        private const string EMPTY_DATA_ERROR_MSG = "No data provided";
        private const string PARSING_ERROR_MSG = "Unable to parse transactions from data";
        private const int REFERENCE_CSV_INDEX = 1;
        private const int AMOUNT_CSV_INDEX = 2;
        private const int DATE_CSV_INDEX = 3;
        private const string DATETIME_FORMAT = "YYY-MM-dd";

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
                .Split('\n')
                .Select(csv => csv.Split("'"))
                .ToList();
        }

        private InputTransactionsDto MapCsvLinesToInputTransactionsDto(List<string[]> transactionLines)
        {
            InputTransactionsDto mappedLines = new();
            InputTransactionDto mappedLine;

            foreach (var transactionLine in transactionLines)
            {
                mappedLine = new()
                {
                    Amount = decimal.Parse(transactionLine[AMOUNT_CSV_INDEX]),
                    Reference = transactionLine[REFERENCE_CSV_INDEX],
                    Date = DateOnly.ParseExact(transactionLine[DATE_CSV_INDEX], DATETIME_FORMAT),
                    Sequence = DetermineSequence(transactionLine, transactionLines)
                };

                mappedLines.Transactions.Add(mappedLine);
            }

            return mappedLines;

        }


        private int DetermineSequence(string[] transactionLine, List<string[]> transactionLines)
        {
            var index = transactionLines.IndexOf(transactionLine);

            if (index == -1)
            {
                throw new ArgumentException("Cannot determine sequence");
            }

            return index; 
        }
    }
}
