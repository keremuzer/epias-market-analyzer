using System.Collections.Generic;
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