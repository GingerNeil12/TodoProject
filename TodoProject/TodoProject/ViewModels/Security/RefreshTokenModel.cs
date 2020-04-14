using System.ComponentModel.DataAnnotations;

namespace TodoProject.ViewModels.Security
{
    public class RefreshTokenModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Refresh Token is required.")]
        public string RefreshToken { get; set; }

        [Required(ErrorMessage = "Bearer Token is required.")]
        public string BearerToken { get; set; }
    }
}
