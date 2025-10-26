using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EpiasMarketAnalyzer.Models
{
    public class TransactionHistoryRequest
    {
        [JsonPropertyName("startDate")]
        public string StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public string EndDate { get; set; }

        public static TransactionHistoryRequest ForDate(DateTime date)
        {
            string startDate = $"{date.Date.AddDays(-1):yyyy-MM-dd}T00:00:00+03:00";
            string endDate = $"{date.Date:yyyy-MM-dd}T00:00:00+03:00";

            return new TransactionHistoryRequest
            {
                StartDate = startDate,
                EndDate = endDate
            };
        }
    }
}