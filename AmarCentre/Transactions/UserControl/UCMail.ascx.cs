using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using AmarCentre.BAL;
using System.Web.UI.HtmlControls;
using System.Globalization;
using Telerik.Web.UI;
using System.Threading;
using System.Net.Mail;
using System.Web.Configuration;
using System.Net;
using System.Net.Mime;
using System.Security.Cryptography;

namespace AmarCentre.Transactions.UserControl
{
    public partial class UCMail : System.Web.UI.UserControl
    {
        Master_Bal obj_master = new Master_Bal();
        Transaction_Bal obj_trans = new Transaction_Bal();
        System_Utilities obj_common = new System_Utilities();
        Voucher BalVoucher = new Voucher();
        MailPdf MailPdfobj = new MailPdf();

        public static string DocType = "";

        public static DataTable dtfilepub = new DataTable();

        public void PageLoad()
        {

        }

        public void UCPageLoad(int PageId, int Id, string CustomerMail = "")
        {
            hdn_user_id.Value = Session["User_Id"].ToString();
            hdnPageId.Value = PageId.ToString();  //1-Quotation  2-invoice , 3- receipt, 4-receiptvoucher , 5- Doc Expiry,
            //6 - customersoa, 7- customerloginmail
            hdn_id.Value = Id.ToString();

            DataSet ds = obj_trans.getCustomerMail(Id, PageId);
            DataTable dt = ds.Tables[0];
            hdncustomerId.Value = ds.Tables[2].Rows[0][0].ToString();
            updcustomerid.Update();

            DataTable dtmail = new DataTable();
            dtmail.Columns.Add("MailId", typeof(string));

           foreach(DataRow r in dt.Rows)
                dtmail.Rows.Add(r["Email"].ToString());

            rptmaildetail.DataSource = dtmail;
            rptmaildetail.DataBind();

            Upd_Add_PanelInner.Update();
        }

        public void UCDocPageLoad( int Id, string CustomerMail,string DocTypeIn,string CustomerId)
        {
            hdn_user_id.Value = Session["User_Id"].ToString();
            hdnPageId.Value = "5"; // Doc Expiry
            hdn_id.Value = Id.ToString();
            DocType = DocTypeIn;

            hdncustomerId.Value = CustomerId;

            DataTable dtmail = new DataTable();
            dtmail.Columns.Add("MailId", typeof(string));

            if (hdncustomerId.Value =="" && CustomerMail !="")
                dtmail.Rows.Add(CustomerMail);
            else
            {
                DataSet ds = obj_trans.getCustomerMail(Convert.ToInt32(CustomerId), 6);   
                DataTable dt = ds.Tables[0];
                foreach (DataRow r in dt.Rows)
                    dtmail.Rows.Add(r["Email"].ToString());
            }

            rptmaildetail.DataSource = dtmail;
            rptmaildetail.DataBind();
            updcustomerid.Update();
            Upd_Add_PanelInner.Update();
        }

        public void UCSOAPageLoad(int PageId, DateTime? Fromdate, DateTime? Todate, int CustomerId, int PaymentStatus, int CompletionStatus, string CustomerMail = "")
        {
            hdn_user_id.Value = Session["User_Id"].ToString();
            hdnPageId.Value = PageId.ToString();  //6
            hdn_id.Value = CustomerId.ToString();
            hdnfromdate.Value = Fromdate.ToString(); ;
            hdntodate.Value = Todate.ToString(); ;
            hdnPaymentStatus.Value = PaymentStatus.ToString();
            hdnCompletionStatus.Value = CompletionStatus.ToString();

            DataSet ds = obj_trans.getCustomerMail(CustomerId, PageId);
            DataTable dt = ds.Tables[0];
            hdncustomerId.Value = CustomerId.ToString();
            updcustomerid.Update();

            DataTable dtmail = new DataTable();
            dtmail.Columns.Add("MailId", typeof(string));

            foreach (DataRow r in dt.Rows)
                dtmail.Rows.Add(r["Email"].ToString());

            rptmaildetail.DataSource = dtmail;
            rptmaildetail.DataBind();

            Upd_Add_PanelInner.Update();
        }

        protected void btn_serDetail_newEntry_Click(object sender, EventArgs e)
        {
            DataTable dtmail = new DataTable();
            dtmail.Columns.Add("MailId", typeof(string));
            foreach (RepeaterItem itm in rptmaildetail.Items)
            {
                TextBox txtmail = (TextBox)itm.FindControl("txtmail");
                if (txtmail.Text != "")
                    dtmail.Rows.Add(txtmail.Text);
            }
            dtmail.Rows.Add("");

            rptmaildetail.DataSource = dtmail;
            rptmaildetail.DataBind();

            UpdMailList.Update();
        }

        protected void rptmaildetail_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            DataTable dtmail = new DataTable();
            dtmail.Columns.Add("MailId", typeof(string));
            foreach (RepeaterItem itm in rptmaildetail.Items)
            {
                TextBox txtmail = (TextBox)itm.FindControl("txtmail");
                if (txtmail.Text != "")
                    dtmail.Rows.Add(txtmail.Text);
            }
            if (e.CommandName == "Add")
                dtmail.Rows.Add("");
            else if (e.CommandName == "Delete")
                dtmail.Rows.RemoveAt(e.Item.ItemIndex);

            rptmaildetail.DataSource = dtmail;
            rptmaildetail.DataBind();

            UpdMailList.Update();
        }

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            DataTable dtmail = new DataTable();
            dtmail.Columns.Add("MailId", typeof(string));
            foreach (RepeaterItem itm in rptmaildetail.Items)
            {
                TextBox txtmail = (TextBox)itm.FindControl("txtmail");
                if (txtmail.Text != "")
                    dtmail.Rows.Add(txtmail.Text);
            }

            if (dtmail.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Add Mail Id ');", true);
            }
            else
            {
                DataTable dtgen = obj_master.Edit_GeneralSettings();
                int Format = 1;
                if (hdnPageId.Value != "5" && hdnPageId.Value != "7")
                {
                    if (hdnPageId.Value == "1")
                    {
                        Format = dtgen.Rows[0]["QuotationFormat"].ToString() == "" ? 1 : Convert.ToInt32(dtgen.Rows[0]["QuotationFormat"].ToString());
                        if (Format == 3)
                            MailPdfobj.QuotationFormat3(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 2)
                            MailPdfobj.QuotationFormat2(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 4)
                            MailPdfobj.QuotationFormat4(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 5)
                            MailPdfobj.QuotationFormat5(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 6)
                            MailPdfobj.QuotationFormat6(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 7)
                            MailPdfobj.QuotationFormat7(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 8)
                            MailPdfobj.QuotationFormat8(Convert.ToInt32(hdn_id.Value));
                       
                        else
                            MailPdfobj.QuotationFormat1(Convert.ToInt32(hdn_id.Value));
                    }
                    else if (hdnPageId.Value == "2")
                    {
                        //Format = dtgen.Rows[0]["InvoiceFormat"].ToString() == "" ? 1 : Convert.ToInt32(dtgen.Rows[0]["InvoiceFormat"].ToString());
                        Format = Convert.ToInt32(obj_trans.Edit_Invoice(Convert.ToInt32(hdn_id.Value), 1, 1).Tables[0].Rows[0]["InvoiceFormat"].ToString());
                        if (Format == 1)
                            MailPdfobj.InvoiceFormat1(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 2)
                            MailPdfobj.InvoiceFormat2(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 3)
                            MailPdfobj.InvoiceFormat3(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 5)
                            MailPdfobj.InvoiceFormat5(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 6)
                            MailPdfobj.InvoiceFormat6(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 7)
                            MailPdfobj.InvoiceFormat7(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 8)
                            MailPdfobj.InvoiceFormat8(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 9)
                            MailPdfobj.InvoiceFormat9(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 10)
                            MailPdfobj.InvoiceFormat10(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 11)
                            MailPdfobj.InvoiceFormat11(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 4)
                            MailPdfobj.InvoiceFormat4(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 12)
                            MailPdfobj.InvoiceFormat12(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 13)
                            MailPdfobj.InvoiceFormat13(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 14)
                            MailPdfobj.InvoiceFormat14(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 15)
                            MailPdfobj.InvoiceFormat15(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 16)
                            MailPdfobj.InvoiceFormat16(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 17)
                            MailPdfobj.InvoiceFormat17(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 18)
                            MailPdfobj.InvoiceFormat18(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 19)
                            MailPdfobj.InvoiceFormat19(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 20)
                            MailPdfobj.InvoiceFormat20(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 21)
                            MailPdfobj.InvoiceFormat21(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 22)
                            MailPdfobj.InvoiceFormat22(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 23)
                            MailPdfobj.InvoiceFormat23(Convert.ToInt32(hdn_id.Value));
                    }
                    else if (hdnPageId.Value == "3")
                    {
                        Format = dtgen.Rows[0]["ReceiptFormat"].ToString() == "" ? 1 : Convert.ToInt32(dtgen.Rows[0]["ReceiptFormat"].ToString());
                        if (Format == 3)
                            MailPdfobj.ReceiptFormat3(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 4)
                            MailPdfobj.ReceiptFormat4(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 5)
                            MailPdfobj.ReceiptFormat5(Convert.ToInt32(hdn_id.Value));
                        else if (Format == 6)
                            MailPdfobj.ReceiptFormat6(Convert.ToInt32(hdn_id.Value));
                        else
                            MailPdfobj.ReceiptFormat1(Convert.ToInt32(hdn_id.Value));
                    }
                    else if (hdnPageId.Value == "4")
                    {
                        MailPdfobj.RVPrint(Convert.ToInt32(hdn_id.Value));
                    }
                   
                    else if (hdnPageId.Value == "6")
                    {
                        Format = dtgen.Rows[0]["CustomerSOAPdfFormat"].ToString() == "" ? 1 : Convert.ToInt32(dtgen.Rows[0]["CustomerSOAPdfFormat"].ToString());

                        if (Format == 1)
                            MailPdfobj.CustomerSOAPdfFormat1(DateTime.Parse(hdnfromdate.Value), DateTime.Parse(hdntodate.Value),
                          Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdnPaymentStatus.Value),
                            Convert.ToInt32(hdnCompletionStatus.Value));

                        else if (Format == 2)
                            MailPdfobj.CustomerSOAPdfFormat2(DateTime.Parse(hdnfromdate.Value), DateTime.Parse(hdntodate.Value),
                          Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdnPaymentStatus.Value),
                            Convert.ToInt32(hdnCompletionStatus.Value));

                        else if (Format == 3)
                            MailPdfobj.CustomerSOAPdfFormat3(DateTime.Parse(hdnfromdate.Value), DateTime.Parse(hdntodate.Value),
                          Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdnPaymentStatus.Value),
                            Convert.ToInt32(hdnCompletionStatus.Value));

                        else if (Format == 4)
                            MailPdfobj.CustomerSOAPdfFormat4(DateTime.Parse(hdnfromdate.Value), DateTime.Parse(hdntodate.Value),
                          Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdnPaymentStatus.Value),
                            Convert.ToInt32(hdnCompletionStatus.Value));

                        else if (Format == 5)
                            MailPdfobj.CustomerSOAPdfFormat5(DateTime.Parse(hdnfromdate.Value), DateTime.Parse(hdntodate.Value),
                          Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdnPaymentStatus.Value),
                            Convert.ToInt32(hdnCompletionStatus.Value));

                        else if (Format == 6)
                            MailPdfobj.CustomerSOAPdfFormat6(DateTime.Parse(hdnfromdate.Value), DateTime.Parse(hdntodate.Value),
                          Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdnPaymentStatus.Value),
                            Convert.ToInt32(hdnCompletionStatus.Value));

                        else if (Format == 7)
                            MailPdfobj.CustomerSOAPdfFormat7(DateTime.Parse(hdnfromdate.Value), DateTime.Parse(hdntodate.Value),
                          Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdnPaymentStatus.Value),
                            Convert.ToInt32(hdnCompletionStatus.Value));

                        else if (Format == 8)
                            MailPdfobj.CustomerSOAPdfFormat8(DateTime.Parse(hdnfromdate.Value), DateTime.Parse(hdntodate.Value),
                          Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdnPaymentStatus.Value),
                            Convert.ToInt32(hdnCompletionStatus.Value));

                        else if (Format == 9)
                            MailPdfobj.CustomerSOAPdfFormat9(DateTime.Parse(hdnfromdate.Value), DateTime.Parse(hdntodate.Value),
                          Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdnPaymentStatus.Value),
                            Convert.ToInt32(hdnCompletionStatus.Value));

                        else if (Format == 10)
                            MailPdfobj.CustomerSOAPdfFormat10(DateTime.Parse(hdnfromdate.Value), DateTime.Parse(hdntodate.Value),
                          Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdnPaymentStatus.Value),
                            Convert.ToInt32(hdnCompletionStatus.Value));
                         
                    }
                    if (hdnPageId.Value == "6")
                    {
                        DataSet dsprint = obj_trans.getDetailForMail(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdnPageId.Value));
                        DataTable dt1print = dsprint.Tables[0];

                        DateTime fromdarte = DateTime.Parse(hdnfromdate.Value);
                        DateTime todate = DateTime.Parse(hdntodate.Value);

                        string formattedFromDate = fromdarte.ToString("yyyy-MM-dd"); // Format as desired
                        string formattedToDate = todate.ToString("yyyy-MM-dd");

                        // Construct the PDF file name with dates
                        string customerName = dt1print.Rows[0]["Attachname"].ToString(); // Assuming this contains the customer name
                        string pdfFileName = $"{customerName}_{formattedFromDate}_{formattedToDate}.pdf";
                        DataTable dtccmail = obj_trans.getCustomerCCMail(Convert.ToInt32(hdncustomerId.Value));

                        ThreadStart sms_thread = new ThreadStart(() => PrintSendMail(dt1print.Rows[0]["MailBody"].ToString(), dt1print.Rows[0]["Subject"].ToString(),
                       Server.MapPath("~/PdfSave/" + pdfFileName),
                            dt1print.Rows[0]["CompanyMail"].ToString(), dt1print.Rows[0]["CompanyEmailPwd"].ToString(), dtccmail));
                        Thread t1 = new Thread(sms_thread);
                        t1.Start();
                    }
                    else
                    {
                        DataTable dtccmail = obj_trans.getCustomerCCMail(Convert.ToInt32(hdncustomerId.Value));
                        DataSet dsprint = obj_trans.getDetailForMail(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdnPageId.Value));
                        DataTable dt1print = dsprint.Tables[0];
                        ThreadStart sms_thread = new ThreadStart(() => PrintSendMail(dt1print.Rows[0]["MailBody"].ToString(), dt1print.Rows[0]["Subject"].ToString(),
                       Server.MapPath("~/PdfSave/" + dt1print.Rows[0]["Attachname"].ToString()),
                            dt1print.Rows[0]["CompanyMail"].ToString(), dt1print.Rows[0]["CompanyEmailPwd"].ToString(), dtccmail));
                        Thread t1 = new Thread(sms_thread);
                        t1.Start();
                    }
                }
                else if (hdnPageId.Value == "5")
                {
                    string cc_companymail = dtgen.Rows[0]["CompanyMail"].ToString();
                    string signpath = "";
                    if (dtgen.Rows[0]["MailSignature"].ToString() != "")
                        signpath = Server.MapPath("~/UploadedImage/" + dtgen.Rows[0]["MailSignature"].ToString());

                    DataTable dtres = obj_master.DocExpiryMail(Convert.ToInt32(hdn_id.Value),Convert.ToInt32(DocType), 
                        Convert.ToInt32(hdn_user_id.Value));
                    DataTable dtccmail = obj_trans.getCustomerCCMail(hdncustomerId.Value==""?0:Convert.ToInt32(hdncustomerId.Value));

                    
                        ThreadStart sms_thread = new ThreadStart(() => obj_common.SendMail(dtres.Rows[0]["MailBody"].ToString(),
                             dtres.Rows[0]["Subject"].ToString(), cc_companymail, signpath, dtccmail, dtmail));
                        Thread t1 = new Thread(sms_thread);
                        t1.Start();
                }
                else if (hdnPageId.Value == "7")
                {
                    string cc_companymail = dtgen.Rows[0]["CompanyMail"].ToString();
                    string signpath = "";

                    DataTable dt = obj_master.CustomerMail(Convert.ToInt32(hdn_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        DataTable dtccmail = obj_trans.getCustomerCCMail(hdn_id.Value == "" ? 0 : Convert.ToInt32(hdn_id.Value));

                        lbl_msg.Text = "Mail has been send successfully !..";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);

                        ThreadStart sms_thread = new ThreadStart(() => obj_common.SendMail(dt.Rows[0]["MailBody"].ToString(),
                             dt.Rows[0]["MailSubject"].ToString(), cc_companymail, signpath, dtccmail, dtmail));
                        Thread t1 = new Thread(sms_thread);
                        t1.Start();
                    }

                }
                Panel pnlMail = (Panel)this.Parent.FindControl("pnlMail");
                UpdatePanel UpdMailPanel = (UpdatePanel)this.Parent.FindControl("UpdMailPanel");
                pnlMail.Visible = false;
                UpdMailPanel.Update();
            }
        }

        protected void btn_close_OnClick(object sender, EventArgs e)
        {
            Panel pnlMail = (Panel)this.Parent.FindControl("pnlMail");
            UpdatePanel UpdMailPanel = (UpdatePanel)this.Parent.FindControl("UpdMailPanel");
            pnlMail.Visible = false;
            UpdMailPanel.Update();
        }

        public void fu_File_OnFileUploaded(object sender, FileUploadedEventArgs e)
        {
            fu_file.TargetFolder = "~/PdfSave";

            string files_name = e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();
            e.File.SaveAs(Path.Combine(Server.MapPath(fu_file.TargetFolder), files_name));
            hdn_file.Value = e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();

            Upd_fufile.Update();
        }

        public void PrintSendMail(string mailBody, string mailSubject, string Filename, string CompanyMail, string CompanyEmailPwd,
            DataTable dtccmail)
        {
            string fromMail = CompanyMail;
            string fromPassword = CompanyEmailPwd;
            var fromAddress = new MailAddress(fromMail, "");  //Transmas
            string subject = mailSubject;
            DataTable dt = obj_master.Edit_GeneralSettings();
            string CCMailCommon = dt.Rows[0]["CCMail"].ToString();

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

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(fromAddress.Address, ""); //From Email Id   Transmas
            mailMessage.Subject = subject; //Subject of Email
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = mailBody; //body or message of Email
            if (CCMailCommon != "")
                mailMessage.CC.Add(CCMailCommon);
            foreach (DataRow r in dtccmail.Rows)
                mailMessage.CC.Add(r["Email"].ToString());

            if (Filename != "")
                mailMessage.Attachments.Add(new Attachment(Filename));

            foreach (RepeaterItem itm in rptmaildetail.Items)
            {
                TextBox txtmail = (TextBox)itm.FindControl("txtmail");
                if (txtmail.Text != "")
                    mailMessage.To.Add(txtmail.Text);
            }

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
    }
}