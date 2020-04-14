using System.ComponentModel.DataAnnotations;

namespace TodoProject.ViewModels.Account
{
    public class UpdateAccountModel
    {
        [Required(ErrorMessage = "User Id is required.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        public string Surname { get; set; }
    }
}
