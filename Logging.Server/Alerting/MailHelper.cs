using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Server.Alerting
{
    class MailHelper
    {

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="toEmail">接受邮件地址</param>
        /// <param name="body">内容</param>
        /// <param name="isBodyHtml">是否Html</param>
        public static int SendMail(string subject, string toEmail, string body, bool isBodyHtml)
        {

            // < item key = "feedback" from = "noreply@plu.cn" password = "PLUn0reply123!" host = "smtp.exmail.qq.com" port = "25" />

            string from = "noreply@plu.cn";
            string password = "PLUn0reply123!";
            string host = "smtp.exmail.qq.com";
            int port = 25;

            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress(from);
            mailMsg.To.Add(toEmail);
            mailMsg.Subject = subject;
            mailMsg.Body = body;
            mailMsg.BodyEncoding = Encoding.UTF8;
            mailMsg.IsBodyHtml = isBodyHtml;
            mailMsg.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new NetworkCredential(from, password);
            smtp.Port = port;
            smtp.Host = host; 
            smtp.EnableSsl = false; // 如果使用GMail，则需要设置为true 
            smtp.SendCompleted += new SendCompletedEventHandler(SendMailCompleted);
            try
            {
                smtp.Send(mailMsg);
                return 1;
            }
            catch (SmtpException ex)
            {
                return 0;
            }
        }





        static void SendMailCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MailMessage mailMsg = (MailMessage)e.UserState;
            string subject = mailMsg.Subject;
            if (e.Cancelled) // 邮件被取消 
            {
                //_logger.Error(subject + " 被取消。");
            }
            if (e.Error != null)
            {
                //_logger.Error("错误：" + e.Error.ToString());
            }
        }

    }
}
