using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace ItAcademy.Helper
{
    public class Sms
    {
        public static async Task SendMailAsync(string messageSubject, string messageBody, string mailTo)
        {
            SmtpClient client = new SmtpClient("smtp.yandex.com", 587);
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("bytewise@itbrains.edu.az", "nybtfalynrtfrmwc");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            MailMessage message = new MailMessage("bytewise@itbrains.edu.az", mailTo);
            message.Subject = messageSubject;
            message.Body = messageBody;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            await client.SendMailAsync(message);

        }
    }
}
