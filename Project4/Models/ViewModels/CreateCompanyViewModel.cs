using System.ComponentModel.DataAnnotations;

namespace Project4.Models.ViewModels
{
    public class CreateCompanyViewModel
    {
		[Required(ErrorMessage = "Company Name is required!")]
		public string CompanyName { get; set; }

		[Required(ErrorMessage = "Company Street Address is required!")]
		public string CompanyStreet { get; set; }

		[Required(ErrorMessage = "Company City is required!")]
		public string CompanyCity { get; set; }

		[Required(ErrorMessage = "Company State is required!")]
		public string CompanyState { get; set; }

		[Required(ErrorMessage = "Company Zip Code is required!")]
		[RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "Zip code must be 5 or 9 digits!")]
		public string CompanyZip { get; set; }

		[Required(ErrorMessage = "Company Phone Number is required!")]
		public string CompanyPhone { get; set; }

		[Required(ErrorMessage = "Company Email is required!")]
		public string CompanyEmail { get; set; }


	}
}
