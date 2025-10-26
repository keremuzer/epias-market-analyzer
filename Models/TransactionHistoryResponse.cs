using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace EpiasMarketAnalyzer.Models
{
    public class TransactionHistoryResponse
    {
        [JsonPropertyName("items")]
        public List<TransactionHistoryItem> Items { get; set; }

        public TransactionHistoryResponse()
        {
            Items = new List<TransactionHistoryItem>();
        }
    }
}