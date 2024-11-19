using Project4.Controllers;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace Project4.Models
{
	public class PasswordHasher
	{
		private readonly PasswordHasher<Agent> hasher = new PasswordHasher<Agent>();
		//private AgentList allAgents = new AgentList();
		private string salt;


		public string GenerateSalt()
		{
			var randomNumber = RandomNumberGenerator.Create();
			var saltBytes = new byte[16];
			randomNumber.GetBytes(saltBytes);
			salt = Convert.ToBase64String(saltBytes);
			return Convert.ToBase64String(saltBytes);
		}

		public string HashPasswordWithSalt(string password, string salt)
		{
			string saltedPassword = password + salt;
			return hasher.HashPassword(null, saltedPassword);
		}

		public string GetSalt()
		{
			return salt;
		}

		public bool VerifyPassword(string username, string enteredPassword)
		{
			//Agent currentAgent = allAgents.GetAgentByUsername(username);
			/*
			if (currentAgent == null)
			{
				return false;
			}
			else
			{
				string saltedPassword = enteredPassword + currentAgent.AgentSalt;
				var result = hasher.VerifyHashedPassword(null, currentAgent.AgentPassword, saltedPassword);
				return result == PasswordVerificationResult.Success;
			}
			*/
			return true;
		}
	}
}
