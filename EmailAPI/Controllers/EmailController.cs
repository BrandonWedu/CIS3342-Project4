using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;

namespace EmailAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : Controller
    {
        //[HttpPost("SendToTempleEmail/{recipient}/{sender}/{subject}/{message}")]
        //public string Post(String recipient, String sender, String subject, String message)
        //{
        //    Email email = new Email();
        //    return $"email sending bool value: {email.SendMail(recipient, sender, subject, message)}";
        //}

        [HttpPost("SendToTempleEmail")]
        //{
        //"recipient": "string",
        //"sender": "string",
        //"subject": "string",
        //"message": "string"
        //}
        public string Post([FromBody] EmailInfo info)
        {
            Email email = new Email();
            string pattern = @"^[a-zA-Z0-9._%+-]+@temple\.edu$";
            if (Regex.IsMatch(info.Recipient, pattern) == false || Regex.IsMatch(info.Sender, pattern) == false)
            {
                return $"Both emails must be from \"@temple.edu\" \nTo: {info.Recipient} \nFrom: {info.Sender} \n";
            }

            return $"email sent: {email.SendMail(info.Recipient, info.Sender, info.Subject, info.Message)} \n" +
                $"To: {info.Recipient}\n" +
                $"From: {info.Sender} \n" +
                $"Subject: {info.Subject} \n" +
                $"Message: {info.Message} \n";
        }
    }
}
