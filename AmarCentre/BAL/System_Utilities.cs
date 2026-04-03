using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web.Configuration;
using AmarCentre.BAL;
using System.Net.Mime;
using System.Reflection.Emit;
using System.Threading.Tasks;
using AmarCentre.WAImplementation;

namespace AmarCentre.BAL
{
    public class System_Utilities : System.Web.UI.Page
    {
        Master_Bal obj_master = new Master_Bal();
        Transaction_Bal obj_trans = new Transaction_Bal();
        WABAL wabal = new WABAL();

        #region MenuHandling

        /*Get Main Menu*/
        public DataSet Get_Main_Menu(int user_id,int langg)
        {
            Database_Operations db_obj = new Database_Operations("Get_Main_Menu", true);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddParameter("@langg", langg);
            return db_obj.GetDataSet();
        }

        /*Get Sub Menu*/
        public DataTable Get_Sub_Menu(int menu_id, int user_id, int langg)
        {
            Database_Operations obj_db = new Database_Operations("Get_Sub_Menu", true);
            obj_db.AddParameter("@menu_id", menu_id);
            obj_db.AddParameter("@user_id", user_id);
            obj_db.AddParameter("@langg", langg);
            return (obj_db.GetDataTable());
        }

        #endregion

        #region Login

        public DataSet SupportValidity()
        {
            Database_Operations obj_db = new Database_Operations("SupportValidity", true);
            return (obj_db.GetDataSet());
        }

        /*Validation Days*/
        public DataTable validation_check()
        {
            Database_Operations obj_db = new Database_Operations("HiddenManaging", true);
            return (obj_db.GetDataTable());
        }

        /*Check Valid User*/
        public DataSet User_Verification(string name, string pass)
        {
            Database_Operations obj_db = new Database_Operations("User_Verification", true);
            obj_db.AddParameter("@user_name", name);
            obj_db.AddParameter("@passwords", pass);
            return (obj_db.GetDataSet());
        }

        public DataSet DocumentExpiredDetails()
        {
            Database_Operations obj_db = new Database_Operations("DocumentExpiredDetails", true);
            return (obj_db.GetDataSet());
        }

        /*Form Previlage for User*/
        public int Form_Previlage_Validation(int menu_id, int user_id)
        {
            Database_Operations obj_db = new Database_Operations("Form_Previlage", true);
            obj_db.AddParameter("@form_id", menu_id);
            obj_db.AddParameter("@user_id", user_id);
            obj_db.AddOutputParameter("@result");
            obj_db.ExecuteQuery();
            return Convert.ToInt32(obj_db.SqlCmd.Parameters["@result"].Value.ToString());
        }

        /*Action Previlage for User*/
        public DataTable Action_Previlage_Validation(int menu_id, int user_id)
        {
            Database_Operations obj_db = new Database_Operations("Get_Action_Previliege", true);
            obj_db.AddParameter("@Sub_Menu", menu_id);
            obj_db.AddParameter("@User_Id", user_id);
            return obj_db.GetDataTable();
        }

        public DataSet FormAction_Previlage_Validation(int menu_id, int user_id)
        {
            Database_Operations obj_db = new Database_Operations("FormAction_Previlage_Validation", true);
            obj_db.AddParameter("@Sub_Menu", menu_id);
            obj_db.AddParameter("@User_Id", user_id);
            return obj_db.GetDataSet();
        }

        public int CustomerForm_Previlage_Validation(int menu_id, int user_id)
        {
            Database_Operations obj_db = new Database_Operations("CustomerForm_Previlage", true);
            obj_db.AddParameter("@form_id", menu_id);
            obj_db.AddParameter("@user_id", user_id);
            obj_db.AddOutputParameter("@result");
            obj_db.ExecuteQuery();
            return Convert.ToInt32(obj_db.SqlCmd.Parameters["@result"].Value.ToString());
        }

        #endregion

        #region Get Code

        public DataTable Get_CustomerPage(string PageName)
        {
            Database_Operations db_obj = new Database_Operations("Get_CustomerPage", true);
            db_obj.AddParameter("@Page", PageName);
            return db_obj.GetDataTable();
        }

        public DataTable Get_Code(int PageId)
        {
            Database_Operations db_obj = new Database_Operations("Get_Code", true);
            db_obj.AddParameter("@PageId", PageId);
            return db_obj.GetDataTable();
        }

        //Get File Code
        public DataTable Get_File_Code(string PageName)
        {
            Database_Operations db_obj = new Database_Operations("Get_File_Code", true);
            db_obj.AddParameter("@Page", PageName);
            return db_obj.GetDataTable();
        }

        #endregion

        #region other
        public StringWriter ExportToExcel(DataTable dt, string name)
        {
            GridView g1 = new GridView();
            g1.AllowPaging = false;
            g1.DataSource = dt;
            g1.DataBind();

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + name + ".xls");
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            g1.HeaderRow.Style.Add("background-color", "#ccc");
            g1.RenderControl(hw);
            return sw;
        }

        public void Back_up()
        {
            Database_Operations db_obj = new Database_Operations("back_up_db", true);
            db_obj.ExecuteQuery_backup();
        }

        public void send_SMS(string numbers, string message)
        {
            try
            {
                //string foundation = "http://sms.trivatechs.com/pushsms.php?username=trivatrchs&api_password=208feiwn18cd61ox3&sender=TTSIND&to=" + numbers + "&message=" + message + "&priority=11";
                //string foundation = "http://sms.trivatechs.com/pushsms.php?username=trivatrchs&api_password=208feiwn18cd61ox3&sender=DINORA&to=" + numbers + "&message=" + message + "&priority=11";
                string foundation = "http://sms.akkuassociates.com/pushsms.php?username=trivatrchs&api_password=208feiwn18cd61ox3&sender=DINORA&to=" + numbers + "&message=" + message + "&priority=11";
                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(foundation);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception e)
            {

            }
        }

        #endregion

        #region mail
        public void SendAgreementExpiredMail(DataTable dtDocDetails, string cc_companymail,string signpath )
        {
            if (dtDocDetails.Rows.Count > 0)
            {
                MailAddressCollection toAddressList = new MailAddressCollection();
                foreach (DataRow dr in dtDocDetails.Rows)
                {
                    DataTable dtccmail = obj_trans.getCustomerCCMail(Convert.ToInt32(dr["CustomerId"]));
                    DataTable dtmail = new DataTable();
                    dtmail.Columns.Add("MailId", typeof(string));

                    DataSet ds = obj_trans.getCustomerMail(Convert.ToInt32(dr["CustomerId"]), 6);
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow r in dt.Rows)
                        dtmail.Rows.Add(r["Email"].ToString());

                    SendMail(DocumentExpiredMailBody(dr),  dr["Subject"].ToString(), cc_companymail, signpath, dtccmail,
                        dtmail);
                }
            }
        }

       

        public void SendMail(string mailBody, string mailSubject, string bcc_companymail, string signpath,
            DataTable dtccmail,DataTable dtmail)
        {
            DataTable dt = obj_master.Edit_GeneralSettings();

            string fromMail = dt.Rows[0]["CompanyMail"].ToString();
            string fromPassword = dt.Rows[0]["CompanyEmailPwd"].ToString();
            string CCMailCommon = dt.Rows[0]["CCMail"].ToString();

            var fromAddress = new MailAddress(fromMail, "");
            string subject = mailSubject;

            string OnlineSoln = WebConfigurationManager.AppSettings["OnlineSoln"].ToString();
            string Hostsmtp = WebConfigurationManager.AppSettings["Hostsmtp"].ToString();
            string Portsmtp = WebConfigurationManager.AppSettings["Portsmtp"].ToString();

            //            Gmail         smtp.gmail.com  587
            //2   Outlook               smtp.live.com   587
            //3   Yahoo Mail          smtp.mail.yahoo.com 465
            //4   Yahoo Mail Plus    plus.smtp.mail.yahoo.com    465
            //5   Hotmail            smtp.live.com   465
            //6   Office365.com      smtp.office365.com  587
            //7   zoho Mail            smtp.zoho.com   465          "mail.almasit.ae"

            var smtp = new SmtpClient
            {
                //Host = "smtp.gmail.com",  // for local
                //Port = 587,
                //Host = "mail.almasit.ae",
                //Port = 26,
                Host = Hostsmtp,
                Port = Convert.ToInt32(Portsmtp),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            var smtponline = new SmtpClient
            {
                Host = "localhost",   // for online
                Port = 25,
                EnableSsl = false,

                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(fromAddress.Address, ""); //From Email Id    Transmas
                mailMessage.Subject = subject; //Subject of Email
                mailMessage.Body = mailBody; //body or message of Email
                mailMessage.Bcc.Add(bcc_companymail);
                if (CCMailCommon != "")
                    mailMessage.CC.Add(CCMailCommon);
                foreach (DataRow r in dtccmail.Rows)
                    mailMessage.CC.Add(r["Email"].ToString());

                mailMessage.IsBodyHtml = true;

                if (signpath != "")
                {
                    LinkedResource LinkedImage = new LinkedResource(signpath);
                    LinkedImage.ContentId = "Msign";
                    LinkedImage.ContentType = new ContentType(MediaTypeNames.Image.Jpeg);
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mailBody, null, "text/html");
                    htmlView.LinkedResources.Add(LinkedImage);
                    mailMessage.AlternateViews.Add(htmlView);
                }
                else
                {
                    mailBody = mailBody.Replace("<br /> <img src=cid:Msign>", "");
                    mailMessage.Body = mailBody; //body or message of Email
                }
                foreach (DataRow r in dtmail.Rows)
                    mailMessage.To.Add(r["MailId"].ToString());
                {
                    try
                    {
                        if (OnlineSoln == "1")
                            smtponline.Send(mailMessage);
                        else
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            smtp.Send(mailMessage);
                        }
                    }
                    catch (Exception ee)
                    {
                        //Response.Write(ee.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                //Response.Write(ee.Message);
            }
        }

        public string DocumentExpiredMailBody(DataRow drAgreementExpiredDetails)
        {
            string mailBody = "";
            mailBody = mailBody + @"
<html lang=""en"">
    <head>    
        <meta content=""text/html; charset=utf-8"" http-equiv=""Content-Type"">
        <title>
            Expiry
        </title>
    <style type=""text/css"">
        HTML{background-color: white;}
        .table_list{
        width:90%;
        border-collapse: collapse;
        }

            .table_list thead th,.table_list tr th,.table_list tr td
        {
        border: 1px solid black;
        border-collapse: collapse;

        }
        .table_list tr th
        {
        background-color: #dedede;
        color: maroon;
        text-align:center;
        }

    </style>
    </head><body>";
            mailBody = mailBody + drAgreementExpiredDetails["MailBody"].ToString() + "</body></html>";
            return mailBody;
        }

        public void SendMailPromotion(string mailBody, DataTable dtGetEmailListP, string mailSubject,
          string fromAddress, string fromPassword, string Filepath)
        {
            string OnlineSoln = WebConfigurationManager.AppSettings["OnlineSoln"].ToString();
            var fromAddressdet = new MailAddress(fromAddress, "");
            string subject = mailSubject;
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",  // for local
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddressdet.Address, fromPassword)
            };

            var smtponline = new SmtpClient
            {
                Host = "localhost",   // for online
                Port = 25,
                EnableSsl = false,

                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddressdet.Address, fromPassword)
            };

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(fromAddressdet.Address, "");
            mailMessage.Subject = subject;
            mailMessage.Body = mailBody;
            mailMessage.IsBodyHtml = true;

            if (Filepath != "")
            {
                Attachment myAttachment = new Attachment(Filepath);
                mailMessage.Attachments.Add(myAttachment);
            }
            foreach (DataRow r in dtGetEmailListP.Rows)
            {
                mailMessage.To.Add(r["EmailId"].ToString());
                {

                    try
                    {
                        if (OnlineSoln == "1")
                            smtponline.Send(mailMessage);
                        else
                            smtp.Send(mailMessage);
                    }
                    catch (Exception ee)
                    {
                        //Response.Write(ee.Message);
                    }
                }
            }
        }

        #endregion

        #region whatsapp

        public async Task<bool> SendWADemoRequest(
            string companyName,
            string emailAddress,
            string phoneNumber, string posturl

            )
        {


            using (var client = new System.Net.Http.HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "CompanyName", companyName },
                    { "PhoneNumber", phoneNumber },
                    { "EmailAddress", emailAddress },
                    { "Context", "Transmas" }
                };

                var content = new System.Net.Http.FormUrlEncodedContent(values);

                var response = await client.PostAsync(posturl, content);

                if (response.IsSuccessStatusCode)
                {
                    return (true);
                }
                else
                {
                    return (false);
                }

            }
        }
        public async void SendWAExpiredAlert(DataTable dtExpiredWAP, string CompanyName, string CompanyPhone,
           string CompanyContactPerson)
        {
            if (dtExpiredWAP.Rows.Count > 0)
            {
                string WABaseUrl = WebConfigurationManager.AppSettings["WABaseUrl"].ToString();
                string WATemplateCode = WebConfigurationManager.AppSettings["WATemplateCode"].ToString();
                string WAKey = WebConfigurationManager.AppSettings["WAKey"].ToString();

                foreach (DataRow dr in dtExpiredWAP.Rows)
                {
                    Mascom.Api.Client.MessagePayload message = new Mascom.Api.Client.MessagePayload
                    {
                        RecipientPhoneNumber = dr["WhatsappNo"].ToString(),
                        TemplateCode = WATemplateCode,
                        BodyParameters = new string[] { dr["Name"].ToString(), dr["DocumentType"].ToString(), dr["ExpiryDate"].ToString(),
                     CompanyPhone,CompanyContactPerson,  CompanyName },
                        MediaTypeName = "text",
                        Priority = 1
                    };

                    var response = await Mascom.Api.Client.Messages.SendMessageAsync(WABaseUrl, WAKey, message);

                    string bodyparameters = string.Join(", ", message.BodyParameters);

                    if (response.Status == true)
                    {
                        if (response.Data != null)
                        {
                            string messageid = response.Data.ToString();
                            int res = wabal.SaveWAMessageLog(messageid, dr["Name"].ToString(), DateTime.Now, dr["WhatsappNo"].ToString(), dr["DocumentType"].ToString(),Convert.ToDateTime(dr["ExpiryDate"].ToString()), dr["DocNumber"].ToString());
                            int res2 = wabal.SaveWAHistoryLog(messageid, dr["WhatsappNo"].ToString(), message.TemplateCode, bodyparameters, message.MediaTypeName, message.Priority.ToString());
                        }
                        //return true; // ✅ success
                    }
                    else
                    {
                        wabal.SaveWAFailedMessageLog(message.RecipientPhoneNumber, message.TemplateCode, bodyparameters, message.MediaTypeName, message.Priority.ToString(),
                             dr["Name"].ToString(), dr["DocumentType"].ToString(), dr["DocNumber"].ToString(), Convert.ToDateTime(dr["ExpiryDate"].ToString()));
                        //return false; // ❌ failure
                    }
                }
            }
        }

        public DataTable GetWhatsappMessageDelivered(string messageId)
        {
            Database_Operations obj_db = new Database_Operations("getmessagedelivered", true);
            obj_db.AddParameter("@MessageId", messageId);
            return (obj_db.GetDataTable());
        }
        public DataTable GetWhatsappMessageFailed(string messageId)
        {
            Database_Operations obj_db = new Database_Operations("GetMessageFailed", true);
            obj_db.AddParameter("@MessageId", messageId);
            return (obj_db.GetDataTable());
        }
        public async Task<bool> SendWAMessage(
            string RecipientPhoneNumber,
            string TemplateCode,
            string[] BodyParameters,
            string MediaTypeName,
            int Priority, string doctype, string docno, DateTime? docexpirydate)
        {
            string WABaseUrl = WebConfigurationManager.AppSettings["WABaseUrl"].ToString();
            string WAKey = WebConfigurationManager.AppSettings["WAKey"].ToString();

            var message = new Mascom.Api.Client.MessagePayload
            {
                RecipientPhoneNumber = RecipientPhoneNumber,
                TemplateCode = TemplateCode,
                BodyParameters = BodyParameters,
                MediaTypeName = MediaTypeName,
                Priority = Priority
            };

            var response = await Mascom.Api.Client.Messages.SendMessageAsync(WABaseUrl, WAKey, message);
           
            string bodyparameters = string.Join(", ", message.BodyParameters);

            if (response.Status == true)
            {
                if (response.Data != null)
                {
                    string messageid = response.Data.ToString();
                    int res = wabal.SaveWAMessageLog(messageid, BodyParameters[0], DateTime.Now, RecipientPhoneNumber, doctype, docexpirydate, docno);
                    int res2 = wabal.SaveWAHistoryLog(messageid, RecipientPhoneNumber, message.TemplateCode, bodyparameters, message.MediaTypeName, message.Priority.ToString());
                }
                return true; // ✅ success
            }
            else
            {
                wabal.SaveWAFailedMessageLog(message.RecipientPhoneNumber, message.TemplateCode, bodyparameters, message.MediaTypeName, message.Priority.ToString(), BodyParameters[0], doctype, docno, docexpirydate);
                return false; // ❌ failure
            }
        }

        #endregion
    }
}