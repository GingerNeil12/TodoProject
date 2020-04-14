using System.ComponentModel.DataAnnotations;

namespace TodoProject.ViewModels.Account
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(maximumLength: 25, ErrorMessage = "Cannot exceed {0} characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        [StringLength(maximumLength: 25, ErrorMessage = "Cannot exceed {0} characters.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
