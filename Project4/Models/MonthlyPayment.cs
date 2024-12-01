using Newtonsoft.Json;

namespace Project4.Models
{
    [Serializable]
    public class MonthlyPayment
    {
        // This class will represent the third party Mortgage API monthly payment 

        [JsonProperty("total")]
        public decimal Total { get; set; }

        [JsonProperty("mortgage")]
        public decimal Mortgage { get; set; }

        [JsonProperty("property_tax")]
        public decimal PropertyTax { get; set; }

        [JsonProperty("hoa")]
        public decimal HOA { get; set; }

        [JsonProperty("annual_home_ins")]
        public decimal AnnualHomeIns { get; set; }
    }
}

