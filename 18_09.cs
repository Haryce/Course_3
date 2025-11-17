using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
class Program
{
    static void Main()
    {
        string gmailAddress = ""; //почта
        string appPassword = "";//пароль
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Your Name", gmailAddress));
        message.To.Add(new MailboxAddress("Recipient Name", "recipient@example.com"));
        message.Subject = "Тестовое письмо через MailKit с Gmail";
        message.Body = new TextPart("plain")
        {
            Text = "XD)) HAUNTED"
        };
        using (var client = new SmtpClient())
        {
            client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            client.Authenticate(gmailAddress, appPassword);

            client.Send(message);
            client.Disconnect(true);
        }
    }
}
