using Project4.Controllers;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Project4.Models
{
    public class PasswordHasher
    {
        //private readonly PasswordHasher<Agent> hasher = new PasswordHasher<Agent>();
        //private AgentList allAgents = new AgentList();
        private string salt;


        public string GenerateSalt()
        {
            var randomNumber = RandomNumberGenerator.Create();
            var saltBytes = new byte[16];
            randomNumber.GetBytes(saltBytes);
            salt = Convert.ToBase64String(saltBytes);
            Console.WriteLine(salt);
            return Convert.ToBase64String(saltBytes);
        }

        public string HashPasswordWithSalt(string password, string sentSalt)
        {
            var sha256 = SHA256.Create();
            string saltedPassword = password + sentSalt;

            Console.WriteLine("SentSalt:" + sentSalt);

            var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
            var hashBytes = sha256.ComputeHash(saltedPasswordBytes);
            Console.WriteLine(saltedPassword);
            return Convert.ToBase64String(hashBytes);
        }

        public string GetSalt()
        {
            return salt;
        }

        public bool VerifyPassword(string username, string enteredPassword)
        {
            Agent currentAgent = ReadAgents.GetAgentByUsername(username);

            if (currentAgent == null)
            {
                return false;
            }

            string hashedPW = HashPasswordWithSalt(enteredPassword, currentAgent.AgentPasswordSalt);

            return hashedPW == currentAgent.AgentPassword;
        }

    }
}
