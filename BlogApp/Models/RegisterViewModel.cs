using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Models
{
    public class RegisterViewModel
    {
        
        [Required]
        [Display(Name = "Username")]
        public string? UserName { get; set; }
        
        [Required]
        [Display(Name = "Fullname")]
        public string? FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [StringLength(maximumLength:12, ErrorMessage = "Password field cannot be less than {2} characters", MinimumLength = 6)]
        [DataType(DataType.Password, ErrorMessage = "Passwords must match!")]
        [Display(Name ="Password")]
        public string? Password { get; set; }


        [Required]
        [StringLength(maximumLength:12, ErrorMessage = "Password field cannot be less than {2} characters", MinimumLength = 6)]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        [Display(Name ="Confirm password")]
        public string? ConfirmPassword { get; set; }
    }
}