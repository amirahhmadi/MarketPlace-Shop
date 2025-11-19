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
    public static class EmailTemplateHelper
    {
        public static string GetTemplate(string fileName, Dictionary<string, string>? values = null)
        {
            // مسیر پوشه قالب‌ها (داخل wwwroot/Templates/Emails)
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "Emails");
            var filePath = Path.Combine(basePath, fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"فایل قالب ایمیل یافت نشد: {filePath}");

            var content = File.ReadAllText(filePath);

            // جایگزینی مقادیر داخل قالب
            if (values != null)
            {
                foreach (var kvp in values)
                {
                    content = content.Replace($"{{{kvp.Key}}}", kvp.Value);
                }
            }

            return content;
        }
    }
}
