using System;
using EpiasMarketAnalyzer.Models;

namespace EpiasMarketAnalyzer.Services
{
    public static class ContractNameParser
    {
        public static ContractInfo Parse(string contractName)
        {
            if (string.IsNullOrWhiteSpace(contractName))
            {
                throw new ArgumentException("Contract name cannot be null or empty");
            }

            if (contractName.Length < 10)
            {
                throw new ArgumentException($"Invalid contract name format: '{contractName}'. Expected at least 10 characters.");
            }

            try
            {
                int year = int.Parse(contractName.Substring(2, 2));
                int month = int.Parse(contractName.Substring(4, 2));
                int day = int.Parse(contractName.Substring(6, 2));
                int hour = int.Parse(contractName.Substring(8, 2));

                int fullYear = 2000 + year;

                DateTime contractDate = new DateTime(fullYear, month, day, hour, 0, 0);

                return new ContractInfo
                {
                    ContractDate = contractDate
                };
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Failed to parse contract name '{contractName}': {ex.Message}", ex);
            }
        }
    }
}