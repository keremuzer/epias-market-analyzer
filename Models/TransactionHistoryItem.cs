using System;
using System.Text.Json.Serialization;

namespace EpiasMarketAnalyzer.Models
{
    public class TransactionHistoryItem
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("hour")]
        public string Hour { get; set; }

        [JsonPropertyName("contractName")]
        public string ContractName { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }
    }
}