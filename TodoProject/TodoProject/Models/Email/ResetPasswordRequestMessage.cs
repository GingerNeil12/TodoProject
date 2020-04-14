namespace TodoProject.Models.Email
{
    public class ResetPasswordRequestMessage : EmailMessage
    {
        public string ResetPasswordURL { get; }

        public ResetPasswordRequestMessage(string recipient, string resetPasswordURL)
            : base(recipient)
        {
            Subject = "Reset Password Request";
            Body = "Please click the link to reset your password. " +
                "If you did not request this please ignore this email.";
            ResetPasswordURL = resetPasswordURL;
        }
    }
}
