namespace TodoProject.Models.Email
{
    public class ChangePasswordMessage : EmailMessage
    {
        public ChangePasswordMessage(string recipient)
            : base(recipient)
        {
            Subject = "Password Changed";
            Body = "Your password has been recently changed. " +
                "If this was not you please reset your password and report this to our Administrators.";
        }
    }
}
