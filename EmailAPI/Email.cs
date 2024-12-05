using System.Net.Mail;

namespace EmailAPI
{
    //Email class that works solely for smtp.temple.edu
    public class Email
    {
        private MailMessage objMail = new MailMessage();
        private MailAddress toAddress;
        private MailAddress fromAddress;
        private String subject;
        private String messageBody;
        private Boolean isHTMLBody = true;
        private MailPriority priority = MailPriority.Normal;
        private String mailHost = "smtp.temple.edu";

        public bool SendMail(String recipient, String sender, String subject, String message)
        {
            try
            {
                //Data Provided for contents of the email
                this.Recipient = recipient;
                this.Sender = sender;
                this.Subject = subject;
                this.Message = message;

                objMail.To.Add(this.toAddress);
                objMail.From = this.fromAddress;
                objMail.Subject = this.subject;
                objMail.Body = this.messageBody;
                objMail.IsBodyHtml = this.isHTMLBody;
                objMail.Priority = this.priority;
                SmtpClient smtpMailClient = new SmtpClient(this.mailHost);
                smtpMailClient.Send(objMail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public Boolean SendMail()
        {
            try
            {
                objMail.To.Add(this.toAddress);
                objMail.From = this.fromAddress;
                objMail.Subject = this.subject;
                objMail.Body = this.messageBody;
                objMail.IsBodyHtml = this.isHTMLBody;
                objMail.Priority = this.priority;
                SmtpClient smtpMailClient = new SmtpClient(this.mailHost);
                smtpMailClient.Send(objMail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public String Recipient
        {
            get { return this.toAddress.ToString(); }
            set { this.toAddress = new MailAddress(value); }
        }
        public String Sender

        {
            get { return this.fromAddress.ToString(); }
            set { this.fromAddress = new MailAddress(value); }
        }
        public String Subject
        {
            get { return this.subject; }
            set { this.subject = value; }
        }
        public String Message
        {
            get { return this.messageBody; }
            set { this.messageBody = value; }
        }
        public Boolean HTMLBody
        {
            get { return this.isHTMLBody; }
            set { this.isHTMLBody = value; }
        }
        public MailPriority Priority
        {
            get { return this.priority; }
            set { this.priority = value; }
        }
        public String MailHost
        {
            get { return this.mailHost; }
            set { this.mailHost = value; }
        }
    }
}
