using ProjetoAspNetMVC01.Messages.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAspNetMVC01.Messages.Services
{
    public class EmailService
    {
        private string Conta = "cotiaulasnoreply@gmail.com";
        private string Senha = "@AdminCOTI123";
        private string Smtp = "smtp.gmail.com";
        private int Porta = 587;

        public void EnviarMensagem(EmailModel model)
        {
            //montando o email
            var mail = new MailMessage(Conta, model.EmailDestinatario);
            mail.Subject = model.Assunto;
            mail.Body = model.Mensagem;
            mail.IsBodyHtml = true;

            //enviando o email
            var smtp = new SmtpClient(Smtp, Porta);
            smtp.Credentials = new NetworkCredential(Conta, Senha);
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}





