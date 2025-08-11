using System.Net.Mail;
using System.Net;

namespace GameOnline.Core.ExtenstionMethods;


public static class SendEmail
{
    public static bool Send(string to, string subject, string body)
    {
        string senderEmail = "amirrezaahmadi869@gmail.com";
        string Password = "rtqa cxdy btwf xzid";
        MailMessage message = new MailMessage(senderEmail, to);
        message.Body = body;
        message.Subject = subject;
        message.IsBodyHtml = true;
        NetworkCredential mailAuthentication = new NetworkCredential(senderEmail, Password);
        SmtpClient mailClient = new SmtpClient("smtp.gmail.com", 587);
        mailClient.EnableSsl = true;
        mailClient.UseDefaultCredentials = false;
        mailClient.Credentials = mailAuthentication;
        mailClient.Send(message);
        return true;
    }
}
