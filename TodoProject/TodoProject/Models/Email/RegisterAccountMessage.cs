namespace TodoProject.Models.Email
{
    public class RegisterAccountMessage : EmailMessage
    {
        public RegisterAccountMessage(string recipient)
            : base(recipient)
        {
            Subject = "Account Created";
            Body = "Welcome to To Do!. " +
                "Your account has been created and we look forward to seeing you online.";
        }
    }
}
