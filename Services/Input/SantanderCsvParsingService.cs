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
        private const int DateCsvIndex = 1;
        private const int ReferenceCsvIndex = 3;
        private const int AmountInCsvIndex = 5;
        private const int AmountOutCsvIndex = 6;
        private const int BalanceCsvIndex = 7;
        private const string DATETIME_FORMAT = "dd/MM/yyyy";
        private const string CsvDelimiter = ",";
        private const string UnescapedCommaPartSymbol = "\"";
        private const int TransactionStartingIndex = 5;
        private const int ExpectedCsvPartsPerLine = 10;


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

            transactionLines.Reverse(); //input is date descending, code below is written for ascending

            var mappedCsvLines = MapCsvLinesToInputTransactionsDto(transactionLines);

            return mappedCsvLines;
        }

        private List<List<string>> ParseCsvIntoTransactionLines(string csvContent)
        {
            List<List<string>> parsedCsvLines = [];
            var unparsedCsvLines = csvContent.Split("\r\n");

            var transactionsCsv = unparsedCsvLines
                .Skip(TransactionStartingIndex) //remove bits before transactions begin
                .Where(x => !string.IsNullOrWhiteSpace(x)); //remove trailing whitespace after the transactions

            foreach (var entry in transactionsCsv)
            {
                var csvLine = ParseCsvLine(entry);
                if (csvLine.Count != ExpectedCsvPartsPerLine)
                {
                    throw new InvalidOperationException("Cannot parse CSV line");
                }

                parsedCsvLines.Add(csvLine);
            }

            return parsedCsvLines;
        }

        private List<string> ParseCsvLine(string unparsedCsvLine) 
        {
            List<string> parsedCsvLine = [];

            var csvLineSplitByComma = unparsedCsvLine.Split(CsvDelimiter);

            string unescapedCommaLineContents = "";
            var unescapedCommaLineParsingBegun = false;

            foreach (var splitByCommaPart in csvLineSplitByComma)
            {
                //unescaped comma part starts with UnescapedCommaPartSymbol and ends in UnescapedCommaPartSymbol
                var partContainsUnescapedCommaLineDelimeter = splitByCommaPart.Contains(UnescapedCommaPartSymbol);

                //1)Begin unescaped part parsing (if applicable)
                if (!unescapedCommaLineParsingBegun && partContainsUnescapedCommaLineDelimeter)
                {
                    unescapedCommaLineContents = splitByCommaPart.Replace(UnescapedCommaPartSymbol, string.Empty);
                    unescapedCommaLineParsingBegun = true;
                    continue;
                }

                //2) Continue unescaped part parsing (if applicable)
                if (unescapedCommaLineParsingBegun && !partContainsUnescapedCommaLineDelimeter)
                {
                    unescapedCommaLineContents += splitByCommaPart;
                    continue;
                }

                //3) Terminate unescaped part parsing (if applicable)
                if (unescapedCommaLineParsingBegun && partContainsUnescapedCommaLineDelimeter)
                {
                    unescapedCommaLineContents += splitByCommaPart.Replace(UnescapedCommaPartSymbol, string.Empty);
                    parsedCsvLine.Add(unescapedCommaLineContents);
                    unescapedCommaLineParsingBegun = false;
                    unescapedCommaLineContents = "";
                    continue;
                }

                //4) Regular CSV part, already parsed
                parsedCsvLine.Add(splitByCommaPart);
            }

            return parsedCsvLine;
        }

        private InputTransactionsDto MapCsvLinesToInputTransactionsDto(List<List<string>> transactionLines)
        {
            InputTransactionsDto mappedLines = new(DateTime.UtcNow, []);
            InputTransactionDto mappedLine;

            var inputSequence = 0;

            DateOnly? dateForSequence = null;
            foreach (var transactionLine in transactionLines)
            {
                var isAmountOut = string.IsNullOrWhiteSpace(transactionLine[AmountInCsvIndex]);
                var amountMultiplier = isAmountOut ? -1 : 1;
                var amountNumberIndex = isAmountOut ? AmountOutCsvIndex : AmountInCsvIndex;

                var transactionLineDate = DateOnly.ParseExact(transactionLine[DateCsvIndex], DATETIME_FORMAT);

                mappedLine = new(
                     transactionLineDate,
                     inputSequence,
                     ParseAmount(transactionLine[amountNumberIndex]) * amountMultiplier,
                     transactionLine[ReferenceCsvIndex],
                     ParseAmount(transactionLine[BalanceCsvIndex])
                );

                mappedLines.Transactions.Add(mappedLine);
                inputSequence++;
            }

            return mappedLines;

        }

        private decimal ParseAmount(string amount) => 
            decimal.Parse(
                amount
                .Replace("£", string.Empty)
                .Replace(" ", string.Empty)
            );
    }
}
