using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Social_Network.Core.Application.ViewModels.Users
{
    public class RegisterRequestVM
    {

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        [Required(ErrorMessage = "Phone number is required.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Photo is required.")]
        [DataType(DataType.Upload)]
        public IFormFile? PhotoPath { get; set; }
    }
}
