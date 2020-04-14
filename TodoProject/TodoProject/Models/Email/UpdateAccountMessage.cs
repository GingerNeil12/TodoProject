namespace TodoProject.Models.Email
{
    public class UpdateAccountMessage : EmailMessage
    {
        public UpdateAccountMessage(string recipient)
            : base(recipient)
        {
            Subject = "Account Update";
            Body = "Your personal details have been updated. " +
                "If this was not you please login and change your password.";
        }
    }
}
