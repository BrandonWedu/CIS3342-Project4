using Newtonsoft.Json;

namespace Project4.Models
{
    [Serializable]
    public class MortgageCalculatorAPI
    {
        // This class will represent the third party Mortgage API response
        [JsonProperty("monthly_payment")]
        public MonthlyPayment MonthlyPayment { get; set; }

        [JsonProperty("annual_payment")]
        public AnnualPayment AnnualPayment { get; set; }

        [JsonProperty("total_interest_paid")]
        public decimal TotalInterestPaid { get; set; }
    }
}
