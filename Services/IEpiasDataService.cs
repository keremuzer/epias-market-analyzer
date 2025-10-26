using EpiasMarketAnalyzer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpiasMarketAnalyzer.Services
{
    public interface IEpiasDataService
    {
        Task<List<TransactionHistoryItem>> GetTransactionHistoryAsync(DateTime date);
    }
}