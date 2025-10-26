using System.Collections.Generic;
using EpiasMarketAnalyzer.Models;

namespace EpiasMarketAnalyzer.Services
{
    public interface IDataProcessor
    {
        List<ContractInfo> GroupAndCalculate(List<TransactionHistoryItem> transactions);
    }
}