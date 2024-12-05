using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project4.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Project4.Controllers
{
    public class MortgageCalculatorController : Controller
    {
        public IActionResult MortgageCalculator()
        {
            string seralizedHome = HttpContext.Session.GetString("CurrentHome");
            Home currentHome = JsonConvert.DeserializeObject<Home>(seralizedHome);
            ViewBag.Home = currentHome;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CalculateMortgage(string homeValue, string downPayment, string interestRate, string durationYears)
        {
            //Validation
            List<string> error = new List<string>();

            string seralizedHome = HttpContext.Session.GetString("CurrentHome");
            Home currentHome = JsonConvert.DeserializeObject<Home>(seralizedHome);
            ViewBag.Home = currentHome;

            //everything must be TryParsed as a double
            if (!double.TryParse(homeValue, out double parsedHomeValue) || double.TryParse(downPayment, out double parsedDownPayment) && parsedHomeValue < parsedDownPayment)
            {
                error.Add("Home Value and Down Payment MUST be numbers and Home value MUST be greater than down payment");
            }
            if (!double.TryParse(interestRate, out double parsedInterestRate) || parsedInterestRate < 0 || parsedInterestRate > 10000)
            {
                error.Add("Interest rate MUST be a valid number greater than 0 and less than 10000.");
            }

            if (!int.TryParse(durationYears, out int parsedDurationYears) || parsedDurationYears < 1 || parsedDurationYears > 10000)
            {
                error.Add("Duration in years MUST be a valid number greater than 1 and less than 10000.");
            }
            if (error.Count > 0)
            {
                TempData["Error"] = error;
                return View("MortgageCalculator");
            }

            //Credit: Code snippet from API website
            //--------------------------------
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://mortgage-calculator-by-api-ninjas.p.rapidapi.com/v1/mortgagecalculator?home_value={homeValue}&downpayment={downPayment}&interest_rate={interestRate}&duration_years={durationYears}"),
                Headers =
            {
                { "x-rapidapi-key", "f1f1321212msh46f5644e29dcfc1p187b28jsn8dff8b93a11b" },
                { "x-rapidapi-host", "mortgage-calculator-by-api-ninjas.p.rapidapi.com" },
            },
                    };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
                TempData["Result"] = body;

                MortgageCalculatorAPI mortgageResponse = JsonConvert.DeserializeObject<MortgageCalculatorAPI>(body);
                if (mortgageResponse == null || mortgageResponse.MonthlyPayment == null || mortgageResponse.AnnualPayment == null)
                {
                    Console.WriteLine("Deserialization failed or some properties are null.");
                }
                else
                {
                    Console.WriteLine("Serialized Passed");
                }
                TempData["MortgageResult"] = JsonConvert.SerializeObject(mortgageResponse);

            }

            //---------------------------------
            return View("MortgageCalculator");
        }
    }
}
