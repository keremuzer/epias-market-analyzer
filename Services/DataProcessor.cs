using System.Collections.Generic;
using System.Linq;
using EpiasMarketAnalyzer.Models;

namespace EpiasMarketAnalyzer.Services
{
    public class DataProcessor : IDataProcessor
    {
        public List<ContractInfo> GroupAndCalculate(List<TransactionHistoryItem> transactions)
        {
            var grouped = transactions
                .GroupBy(t => t.ContractName)
                .Select(group =>
                {
                    var parsedContract = ContractNameParser.Parse(group.Key);

                    var contractInfo = new ContractInfo
                    {
                        ContractDate = parsedContract.ContractDate
                    };

                    foreach (var transaction in group)
                    {
                        contractInfo.TotalTransactionAmount += (transaction.Price * transaction.Quantity) / 10m;
                        contractInfo.TotalTransactionQuantity += transaction.Quantity / 10m;
                    }

                    return contractInfo;
                })
                .OrderBy(c => c.ContractDate)
                .ToList();

            return grouped;
        }
    }
}
