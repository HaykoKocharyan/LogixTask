using System;
using System.ComponentModel.DataAnnotations;

namespace Logix.Models
{
    public class EditUserModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address {  get; set; }
        public string? Username { get; set; }
        public string? NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "The NewPassword and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }
    }
}
