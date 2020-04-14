namespace TodoProject.Models.Email
{
    public class ResetPasswordMessage : EmailMessage
    {
        public ResetPasswordMessage(string recipient)
            : base(recipient)
        {
            Subject = "Password Reset";
            Body = "Your password has been reset. If this was not you please reset again and contact our Administrators.";
        }
    }
}
