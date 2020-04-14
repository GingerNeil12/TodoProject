using System.ComponentModel.DataAnnotations;

namespace TodoProject.ViewModels.Security
{
    public class ResetPasswordRequestModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Reset Password URL is required.")]
        [DataType(DataType.Url)]
        public string ResetPasswordURL { get; set; }
    }
}
