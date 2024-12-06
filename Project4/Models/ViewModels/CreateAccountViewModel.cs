using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Project4.Models.ViewModels
{
    public class CreateAccountViewModel
    {
        [ValidateNever]
        public Agent agent { get; set; }

        [ValidateNever]
        public AgentContact contact { get; set; }

        [ValidateNever]
        public AgentPersonalInformation personalInformation { get; set; }

        [ValidateNever]
        public Company agentCompany { get; set; }

        [ValidateNever]
        public Address address { get; set; }

        [ValidateNever]
        public PasswordHasher passwordHasher { get; set; }


        public SecurityQuestionList securityQuestionList = new SecurityQuestionList();


        [ValidateNever]
        public AgentSecurity agentQuestionOne { get; set; }

        [ValidateNever]
        public AgentSecurity agentQuestionTwo { get; set; }

        [ValidateNever]
        public AgentSecurity agentQuestionThree { get; set; }

        // Account Info
        [Required(ErrorMessage = "Username is required!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Your password must be atleast 6 characters long!")]
        public string Password { get; set; }

        // Agent Personal Info
        [Required(ErrorMessage = "First Name is required!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Personal Street Address is required!")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Personal City is required!")]
        public string City { get; set; }

        [Required(ErrorMessage = "Personal State is required!")]
        public string State { get; set; }

        [Required(ErrorMessage = "Personal Zip Code is required!")]
        [RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "Zip code must be 5 or 9 digits!")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "Personal Phone Number is required!")]
        [RegularExpression(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}", ErrorMessage = "Phone Number must be a proper phone number!")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Personal Email is required!")]
        public string Email { get; set; }


        // Agent Work Contact Info
        [Required(ErrorMessage = "Work Street Address is required!")]
        public string WorkStreet { get; set; }

        [Required(ErrorMessage = "Work City is required!")]
        public string WorkCity { get; set; }

        [Required(ErrorMessage = "Work State is required!")]
        public string WorkState { get; set; }

        [Required(ErrorMessage = "Work Zip Code is required!")]
        [RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "Zip code must be 5 or 9 digits!")]
        public string WorkZip { get; set; }

        [Required(ErrorMessage = "Work Phone Number is required!")]
        [RegularExpression(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}", ErrorMessage = "Work Phone Number must be a proper phone number!")]
        public string WorkPhone { get; set; }

        [Required(ErrorMessage = "Work Email is required!")]
        public string WorkEmail { get; set; }


        // Company Info
        [Required(ErrorMessage = "Selecting a Company is required!")]
        public int Company { get; set; }


        // Security Question Info

        public string QuestionOne { get; set; }
        public string QuestionTwo { get; set; }
        public string QuestionThree { get; set; }

        [Required(ErrorMessage = "Security Question One Answer is required!")]
        public string AnswerOne { get; set; }

        [Required(ErrorMessage = "Security Question Two Answer is required!")]
        public string AnswerTwo { get; set; }

        [Required(ErrorMessage = "Security Question Three Answer is required!")]
        public string AnswerThree { get; set; }
    }
}
