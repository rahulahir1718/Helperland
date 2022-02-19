using System.Net;
using System.Net.Mail;

namespace HelperlandProject.Models
{
    public class EmailManager
    {
         public static void SendEmail(List<string> toList,string Subject,string Body)
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            foreach (var mailTo in toList)
            {
                mail.To.Add(mailTo);
            }
            mail.IsBodyHtml = true;
            mail.From = new MailAddress("rahulkumbharvadiya28@gmail.com");
            mail.Subject = Subject;
            mail.Body = Body;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("rahulkumbharvadiya28@gmail.com","Rahul@028");
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}
