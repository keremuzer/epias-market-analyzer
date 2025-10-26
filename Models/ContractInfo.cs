using System;

namespace EpiasMarketAnalyzer.Models
{
    public class ContractInfo
    {
        public DateTime ContractDate { get; set; }
        public decimal TotalTransactionAmount { get; set; }
        public decimal TotalTransactionQuantity { get; set; }
        public decimal WeightedAveragePrice
        {
            get
            {
                if (TotalTransactionQuantity == 0)
                    return 0;

                return TotalTransactionAmount / TotalTransactionQuantity;
            }
        }
    }
}