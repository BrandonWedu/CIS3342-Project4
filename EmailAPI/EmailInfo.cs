namespace EmailAPI
{
    public class EmailInfo
    {
        private String recipient;
        private String sender; 
        private String subject; 
        private String message;

        public EmailInfo(string recipient, string sender, string subject, string message)
        {
            this.recipient = recipient;
            this.sender = sender;
            this.subject = subject;
            this.message = message;
        }
        public String Recipient { get { return recipient; } }
        public String Sender { get { return sender; } }
        public String Subject { get { return subject; } }
        public String Message { get { return message; } }
    }
}
