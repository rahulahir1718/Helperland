using System.Net;
using System.Net.Mail;

namespace HelperlandProject.Models
{
    public class EmailManager
    {
         public static void SendEmail(string To,string Subject,string Body)
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.IsBodyHtml = true;
            mail.To.Add(To);
            mail.From = new MailAddress("my_email");
            mail.Subject = Subject;
            mail.Body = Body;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("my_email","my_password");
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}
