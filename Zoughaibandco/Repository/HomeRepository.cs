using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using Zoughaibandco.Models;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.Repository
{
    public class HomeRepository
    {
        Zoughaibandco_DBEntities _DBContext;
        private IConfiguration _iconfiguration;

        public HomeRepository()
        {
            _DBContext = new Zoughaibandco_DBEntities();
        }

        public int SendOrderEmailToUser(List<OrderItems_VM> orderItems, string subject, string UserName, string email)
        {
            int nRet = 0;
            string orderDetails = "";
            try
            {
                string templatename = System.Web.HttpContext.Current.Server.MapPath("/Templates/Order.html");
                string html = System.IO.File.ReadAllText(templatename);
                html = html.Replace("{{userName}}", UserName);
                html = html.Replace("{{baseurl}}", Convert.ToString(ConfigurationManager.AppSettings["DomainURL"]));

                foreach (var item in orderItems)
                {
                    orderDetails += "<td style='width:100px'>";
                    orderDetails += "<table>";
                    orderDetails += "<tbody>";
                    orderDetails += "<tr>";
                    orderDetails += "<td align='left' style='font - size:15px; padding: 15px 15px 15px 15px; word -break:break-word; '>";
                    orderDetails += "<p style='text - align: center; '>" + item.ProductName + "</p>";
                    orderDetails += "</td>";
                    orderDetails += "</tr>";
                    orderDetails += "<tr>";
                    orderDetails += "<td style='width: 84px; '>";
                    orderDetails += "<img height='auto' src=" + Convert.ToString(ConfigurationManager.AppSettings["DomainURL"]) + "Uploads/"+ item.ProductImg+ " style='border: 0; display: block; outline: none; text - decoration:none; height: auto; width: 100 %; font - size:13px; ' width='84'>";
                    orderDetails += "</td>";
                    orderDetails += "</tr>";
                    orderDetails += "<tr>";
                    orderDetails += "<td align='left' style='font - size:15px; padding: 15px 15px 15px 15px; word -break:break-word; '>";
                    orderDetails += "<p style='text - align: center; '>" + item.Price + "$</p>";
                    orderDetails += "</td>";
                    orderDetails += "</tr>";
                    orderDetails += "</tbody>";
                    orderDetails += "</table>";
                    orderDetails += "</td>";

                }
                html = html.Replace("{{orderDetails}}", orderDetails);
                int sent = SendEmailAuto(email, subject, html, true);
            }
            catch (SmtpException mailex)
            {
                nRet = -1;
            }

            return nRet;
        }

        public int SendEmailAuto(string email, string subject, string message, bool Ishtml = false)
        {
            int nRet = 0;

            try
            {

                var SmtpClient = Convert.ToString(ConfigurationManager.AppSettings["SmtpClient"]);
                var Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                var UserName = Convert.ToString(ConfigurationManager.AppSettings["UserName"]);
                var Password = Convert.ToString(ConfigurationManager.AppSettings["Password"]);
                var EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["SSL"]);
                var FromAddress = Convert.ToString(ConfigurationManager.AppSettings["From"]);

                SmtpClient client = new SmtpClient(SmtpClient, Port);
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(UserName, Password);
                if (EnableSSL)
                {
                    client.EnableSsl = true;
                }

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(FromAddress);
                mailMessage.To.Add(email);
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = Ishtml;
                mailMessage.Subject = subject;
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                client.Send(mailMessage);
                nRet = 1;

            }
            catch (SmtpException mailex)
            {
                nRet = -1;
            }
            catch(Exception ex)
            {
                nRet = 0;
            }

            return nRet;
        }

        public int SendVoucherEmailToUser(string email, string subject, decimal amount, string userName, bool Ishtml = false)
        {
            int nRet = 0;

            try
            {
                var SmtpClient = Convert.ToString(ConfigurationManager.AppSettings["SmtpClient"]);
                var Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                var UserName = Convert.ToString(ConfigurationManager.AppSettings["UserName"]);
                var Password = Convert.ToString(ConfigurationManager.AppSettings["Password"]);
                var EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["SSL"]);
                var FromAddress = Convert.ToString(ConfigurationManager.AppSettings["From"]);

                SmtpClient client = new SmtpClient(SmtpClient, Port);
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(UserName, Password);
                if (EnableSSL)
                {
                    client.EnableSsl = true;
                }

                string templatename = System.Web.HttpContext.Current.Server.MapPath("/Templates/Voucher.html");
                string html = System.IO.File.ReadAllText(templatename);
               
                html = html.Replace("{{userName}}", userName);
                html = html.Replace("{{baseurl}}", Convert.ToString(ConfigurationManager.AppSettings["DomainURL"]));
                html = html.Replace("{{amount}}", amount.ToString());

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(FromAddress);
                mailMessage.To.Add(email);
                mailMessage.Body = html;
                mailMessage.IsBodyHtml = Ishtml;
                mailMessage.Subject = subject;
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                client.Send(mailMessage);
                nRet = 1;
            }
            catch (SmtpException mailex)
            {
                nRet = -1;
            }
            return nRet;
        }

        public int SendOrderEmail(string EmailBody, string Subject)
        {
            int nRet = 0;
            try
            {
                try
                {
                    var SmtpClient = Convert.ToString(ConfigurationManager.AppSettings["SmtpClient"]);
                    var Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                    var UserName = Convert.ToString(ConfigurationManager.AppSettings["UserName"]);
                    var Password = Convert.ToString(ConfigurationManager.AppSettings["Password"]);
                    var EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["SSL"]);
                    var FromAddress = Convert.ToString(ConfigurationManager.AppSettings["From"]);
                    var GiftVoucherNotificationTo = Convert.ToString(ConfigurationManager.AppSettings["GiftVoucherNotificationTo"]);

                    SmtpClient client = new SmtpClient(SmtpClient, Port);
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(UserName, Password);
                    if (EnableSSL)
                    {
                        client.EnableSsl = true;
                    }
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(FromAddress);
                    mailMessage.To.Add(GiftVoucherNotificationTo);
                    mailMessage.Body = EmailBody;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Subject = Subject;
                    mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                    client.Send(mailMessage);
                    nRet = 1;
                }
                catch (SmtpException mailex)
                {
                    nRet = -1;
                }
            }
            catch (Exception ex)
            {
                nRet = 0;
            }
            return nRet;
        }
    }
}