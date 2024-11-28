using Microsoft.AspNetCore.Mvc;

namespace Project4.Controllers
{
    public class AgentDashboardController : Controller
    {
        public IActionResult AgentDashboard()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DeleteCookie()
        {
            CookieOptions agentCookieOptions = new CookieOptions();
            //Add a new cookie with the same name and set it to expire yesterday
            agentCookieOptions.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Response.Cookies.Append("LoggedInAgent", "", agentCookieOptions);

            return View("AgentDashboard");
        }

    }
}
