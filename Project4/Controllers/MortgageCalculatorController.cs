using Microsoft.AspNetCore.Mvc;

namespace Project4.Controllers
{
    public class MortgageCalculatorController : Controller
    {
        public IActionResult MortgageCalculator()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CalculateMortgage(double homeValue, double downPayment, double interestRate, int durationYears)
        {
            //Validation
            //everything must be TryParsed as a double
            //homeValue > downPayment
            //interestRate < 10000
            //durationYears > 0 %% durationYears < 10000
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
            }
            //---------------------------------
            return View("MortgageCalculator");
        }
    }
}
