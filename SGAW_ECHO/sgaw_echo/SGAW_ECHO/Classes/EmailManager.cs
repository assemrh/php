using System;
using System.Collections.Generic;
using System.Net.Mail;
using SmtpClient = System.Net.Mail.SmtpClient;

namespace SGAW_ECHO.Classes
{
    public class EmailManager
    {
        public static bool SendEmail(string Title, string MailBody, string ToEMail, out string errMessage, List<Attachment> lstAttFiles = null, string CCMail = "", string BCCMail = "") {
            string MailServer = "mail.tornado-soft.com";
            string MailFrom = "university@stuhere.com";
            string MailUser = "university@stuhere.com";
            string MailPassword = "&77Ahqn0";
            int MailPort = 587;
            //string MailServer = "smtp.gmail.com";
            //string MailFrom = "fyasser728@gmail.com";
            //string MailUser = "fyasser728@gmail.com";
            //string MailPassword = "12345657wdfd";
            //int MailPort = 587;
            string Sender_Email_Title = Title;
            
            errMessage = "";
            try {
                SmtpClient SmtpServer = new SmtpClient();
                MailMessage mail_message = new MailMessage();

                SmtpServer.Credentials = new System.Net.NetworkCredential(MailUser, MailPassword);
                SmtpServer.Port = MailPort;
                SmtpServer.Host = MailServer;
                SmtpServer.EnableSsl = false;

                mail_message.From = new MailAddress(MailFrom, Sender_Email_Title);
                mail_message.IsBodyHtml = true;

                //if (lstAttFiles != null && lstAttFiles.Count > 0) {
                //    for (int aCounter = 0; aCounter < lstAttFiles.Count; aCounter++) { 
                //            mail_message.Attachments.Add(lstAttFiles[aCounter]);
                //    }
                //}

                string[] arrToMail = ToEMail.Replace(",", ";").Split(new char[]{';'});
                for (int iCounter = 0; iCounter < arrToMail.Length; iCounter++) {
                    if (arrToMail[iCounter].Trim() != string.Empty) {
                        try {
                            MailAddress m = new MailAddress(arrToMail[iCounter]);
                            mail_message.To.Add(m);
                        } catch (Exception ex) {
                            errMessage += "Not valid : *** " + arrToMail[iCounter] + " ***" + Environment.NewLine;
                        }
                    }
                }
                //if (CCMail != null && CCMail.Trim() != string.Empty) {
                //    arrToMail = CCMail.Replace(",", ";").Split(new char[] { ';' });
                //    for (int iCounter = 0; iCounter < arrToMail.Length; iCounter++) {
                //        if (arrToMail[iCounter].Trim() != string.Empty) {
                //            try {
                //                MailAddress m = new MailAddress(arrToMail[iCounter]);
                //                mail_message.CC.Add(m);
                //            }
                //            catch (Exception ex) {
                //                errMessage += "Not valid : *** " + arrToMail[iCounter] + " ***" + Environment.NewLine;
                //            }
                //        }
                //    }
                //}
                //if (BCCMail != null && CCMail.Trim() != string.Empty)
                //{
                //    arrToMail = BCCMail.Replace(",", ";").Split(new char[] { ';' });
                //    for (int iCounter = 0; iCounter < arrToMail.Length; iCounter++)
                //    {
                //        if (arrToMail[iCounter].Trim() != string.Empty)
                //        {
                //            try
                //            {
                //                MailAddress m = new MailAddress(arrToMail[iCounter]);
                //                mail_message.Bcc.Add(m);
                //            }
                //            catch (Exception ex)
                //            {
                //                errMessage += "Not valid : *** " + arrToMail[iCounter] + " ***" + Environment.NewLine;
                //            }
                //        }
                //    }
                //}

                if (mail_message.To.Count == 0) {
                    errMessage = "Failed to send, No mail accounts added ...";
                    return false;
                }
                mail_message.Subject = Title;
                mail_message.Body = MailBody;
                
                SmtpServer.Send(mail_message);

                //if (mail_message.Attachments != null && mail_message.Attachments.Count > 0) {
                //    for (int aCounter = 0; aCounter < mail_message.Attachments.Count; aCounter++) {
                //        mail_message.Attachments[aCounter].Dispose();
                //    }
                //}
                mail_message.Dispose();

                return true;
            }
            catch (Exception ex) {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}