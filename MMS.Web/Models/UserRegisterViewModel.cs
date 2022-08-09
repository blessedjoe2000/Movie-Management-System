using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MMS.Data.Models;

namespace MMS.Web.Models
{
    public class UserRegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Remote("VerifyEmailAddress", "User")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Try again! Confirm password do not match")]
        [Display(Name = "Confirm Password")]
        public string PasswordConfirm { get; set; }

        [Required]
        public Role Role { get; set; }

    }
    
}