using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoProject.Models.Email
{
    public abstract class EmailMessage
    {
        public string Sender { get; set; }
        public string Recipient { get; }
        public string Subject { get; protected set; }
        public string Body { get; protected set; }

        protected EmailMessage(string recipient)
        {
            Recipient = recipient;
        }
    }
}
