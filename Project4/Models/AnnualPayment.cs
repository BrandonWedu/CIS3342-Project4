using Newtonsoft.Json;

namespace Project4.Models
{
    [Serializable]
    public class AnnualPayment
    {
        // This class will represent the third party Mortgage API annual payment
        [JsonProperty("total")]
        public decimal Total { get; set; }

        [JsonProperty("mortgage")]
        public decimal Mortgage { get; set; }

        [JsonProperty("property_tax")]
        public decimal PropertyTax { get; set; }

        [JsonProperty("hoa")]
        public decimal HOA { get; set; }

        [JsonProperty("home_insurance")]
        public decimal HomeInsurance { get; set; }
    }
}
