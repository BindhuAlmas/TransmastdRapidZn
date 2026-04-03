using AmarCentre.BAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace AmarCentre.Masters
{
    public partial class Administration : System.Web.UI.Page
    {
        System_Utilities obj_common = new System_Utilities();
        Master_Bal obj_master = new Master_Bal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
            }
            if (!IsPostBack)
            {
                hdn_user_id.Value = Session["User_Id"].ToString();
                previlage_check();
                previlage_action_check();
                FilldrpDown();
                fill_Data();
            }

        }

        public void FilldrpDown()
        {

            drpExpense.Items.Clear();
            DataTable dtExpense = obj_master.Drp_Expense();
            drpExpense.DataSource = dtExpense;
            drpExpense.DataValueField = "Value";
            drpExpense.DataTextField = "Text";
            drpExpense.DataBind();

            drprefundexpense.Items.Clear();
            drprefundexpense.DataSource = dtExpense;
            drprefundexpense.DataValueField = "Value";
            drprefundexpense.DataTextField = "Text";
            drprefundexpense.DataBind();

            drpAdminDesign.DataSource = obj_master.Drp_Design();
            drpAdminDesign.DataTextField = "text";
            drpAdminDesign.DataValueField = "value";
            drpAdminDesign.DataBind();

            DataTable dttte = obj_master.DrpTemplate();
            drpTemplate.DataSource = dttte;
            drpTemplate.DataValueField = "Value";
            drpTemplate.DataTextField = "Text";
            drpTemplate.DataBind();
            drpTemplate.Text = "";
        }

        public void fill_Data()
        {
            DataTable dt = obj_master.Edit_GeneralSettings();
            if (dt.Rows.Count > 0)
            {
                hdn_id.Value = dt.Rows[0]["Id"].ToString();
                hdn_printHeader.Value = dt.Rows[0]["PrintHeader"].ToString();
                hdn_printfootr.Value = dt.Rows[0]["PrintFooter"].ToString();
                hdnfu_MailFile.Value = dt.Rows[0]["MailSignature"].ToString();

                drpExpense.SelectedValue = dt.Rows[0]["FineExpenseType"].ToString();
                drp_paymode.SelectedValue = dt.Rows[0]["DefaultPayModeInQuickReceipt"].ToString();
                chkTaxAppliedWithDiscount.Checked = Convert.ToBoolean(dt.Rows[0]["TaxAppliedWithDiscount"]);
                chkscinvoice.Checked = Convert.ToBoolean(dt.Rows[0]["SCInInvoice"]);
                txtSendAgreementExpiredMailBefore.Text = dt.Rows[0]["SendAgreementExpiredMailBefore"].ToString();
                txtDocExpireSecondaryMailDays.Text = dt.Rows[0]["DocExpireSecondaryMailDays"].ToString();
                chkEmpSC.Checked = Convert.ToBoolean(dt.Rows[0]["IsEmployeeBasedSCList"]);
                chkdeltdSC.Checked = Convert.ToBoolean(dt.Rows[0]["ShowDeletedSC"]);
                txtDefaultBankCharge.Text = dt.Rows[0]["DefaultBankCharge"].ToString();
                drprefundexpense.SelectedValue = dt.Rows[0]["RefundableExpenseId"].ToString();
                Application["Company"] = dt.Rows[0]["CompanyName"].ToString();
                chkIsMobileDupAllow.Checked = Convert.ToBoolean(dt.Rows[0]["IsMobileDupAllow"]);
                chkIstaxprintforall.Checked = Convert.ToBoolean(dt.Rows[0]["IsTaxPrintForAll"]);
                chkIsAllowSCAmountExceed.Checked = Convert.ToBoolean(dt.Rows[0]["IsAllowSCAmountExceed"]);
                chkIsCommissionEditableInInvoice.Checked = Convert.ToBoolean(dt.Rows[0]["IsCommissionEditableInInvoice"]);
                drpAdminDesign.SelectedValue = dt.Rows[0]["AdminDesginId"].ToString();
                drpTemplate.SelectedValue = dt.Rows[0]["TemplateId"].ToString();
                txtmail.Text = dt.Rows[0]["CompanyMail"].ToString();
                txtCompanyEmailPwd.Text = dt.Rows[0]["CompanyEmailPwd"].ToString();
                txtCompanyEmailPwd.Attributes["value"] = dt.Rows[0]["CompanyEmailPwd"].ToString();
                txtccmail.Text = dt.Rows[0]["CCMail"].ToString();
                drpDefaultInvoiceType.SelectedValue = dt.Rows[0]["InvoiceType"].ToString();
                drpDefaultInvoiceType.Enabled = dt.Rows[0]["TRN"].ToString() != "" ? false : true;
                txtTRN.Text = dt.Rows[0]["TRN"].ToString();
                txtCompanyname.Text = dt.Rows[0]["CompanyName"].ToString();
                txtCompanyPhone.Text = dt.Rows[0]["CompanyPhone"].ToString();
                txtCompanyContactPerson.Text = dt.Rows[0]["CompanyContactPerson"].ToString();
                txtVATOB.Text = dt.Rows[0]["VATOB"].ToString();
                radVATOBDate.DbSelectedDate = dt.Rows[0]["VATOBDate"].ToString();

                if (hdn_printHeader.Value != "")
                {
                    img_upld_fu_printHeader.ImageUrl = "~/UploadedImage/" + hdn_printHeader.Value;
                    div_filUp_fu_printHeader.Visible = true;
                    Application["PrintHeader"] = hdn_printHeader.Value;
                }
                else
                {
                    div_filUp_fu_printHeader.Visible = false;
                    Application["PrintHeader"] = "";
                }

                if (hdn_printfootr.Value != "")
                {
                    img_upld_fu_printfootr.ImageUrl = "~/UploadedImage/" + hdn_printfootr.Value;
                    div_filUp_fu_printfootr.Visible = true;
                    Application["PrintFooter"] = hdn_printfootr.Value;
                }
                else
                {
                    div_filUp_fu_printfootr.Visible = false;
                    Application["PrintFooter"] = "";
                }
                if (hdnfu_MailFile.Value != "")
                {
                    imgfu_MailFile.ImageUrl = "~/UploadedImage/" + hdnfu_MailFile.Value;
                    divfu_MailFile.Visible = true;
                }
                else
                {
                    divfu_MailFile.Visible = false;
                }
            }
        }

        public void Clear_PrintHeader()
        {
            hdn_printHeader.Value = "";
            if (hdn_printHeader.Value != "")
            {
                div_filUp_fu_printHeader.Visible = true;
            }
            else
            {
                div_filUp_fu_printHeader.Visible = false;
            }
            Upd_fu_printHeader.Update();
        }

        public void fu_printHeader_OnFileUploaded(object sender, FileUploadedEventArgs e)
        {
            Clear_PrintHeader();
            fu_printHeader.TargetFolder = "~/UploadedImage";

            string files_name = e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();
            e.File.SaveAs(Path.Combine(Server.MapPath(fu_printHeader.TargetFolder), files_name));

            try
            {
                //in backup folder also
                DataTable dtgen = obj_master.Edit_GeneralSettings();
                File.Copy((Path.Combine(Server.MapPath(fu_printHeader.TargetFolder), files_name)),
                    (Path.Combine(dtgen.Rows[0]["BackupDrivePath"].ToString() + "UploadedImage", files_name)), false);
            }
            catch (Exception cc) { }

            hdn_printHeader.Value = e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();

            if (hdn_printHeader.Value != "")
            {
                img_upld_fu_printHeader.ImageUrl = "~/UploadedImage/" + hdn_printHeader.Value;
                div_filUp_fu_printHeader.Visible = true;
            }
            else
            {
                div_filUp_fu_printHeader.Visible = false;
            }

            Upd_fu_printHeader.Update();
        }

        protected void btnbtnclosePHOnClick(object sender, EventArgs e)
        {
            hdn_printHeader.Value = "";
            div_filUp_fu_printHeader.Visible = false;
            Application["PrintHeader"] = "";
            Upd_fu_printHeader.Update();
        }

        public void Clear_PrintFooter()
        {
            hdn_printfootr.Value = "";
            if (hdn_printfootr.Value != "")
            {
                div_filUp_fu_printfootr.Visible = true;
            }
            else
            {
                div_filUp_fu_printfootr.Visible = false;
            }
            Upd_fu_printfootr.Update();
        }

        public void fu_printfooter_OnFileUploaded(object sender, FileUploadedEventArgs e)
        {
            Clear_PrintFooter();
            fu_printfooter.TargetFolder = "~/UploadedImage";

            string files_name = e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();
            e.File.SaveAs(Path.Combine(Server.MapPath(fu_printfooter.TargetFolder), files_name));

            try
            {
                //in backup folder also
                DataTable dtgen = obj_master.Edit_GeneralSettings();
                File.Copy((Path.Combine(Server.MapPath(fu_printfooter.TargetFolder), files_name)),
                    (Path.Combine(dtgen.Rows[0]["BackupDrivePath"].ToString() + "UploadedImage", files_name)), false);
            }
            catch (Exception cc) { }

            hdn_printfootr.Value = e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();

            if (hdn_printfootr.Value != "")
            {
                img_upld_fu_printfootr.ImageUrl = "~/UploadedImage/" + hdn_printfootr.Value;
                div_filUp_fu_printfootr.Visible = true;
            }
            else
            {
                div_filUp_fu_printfootr.Visible = false;
            }

            Upd_fu_printfootr.Update();
        }

        protected void btnbtnclosePFOnClick(object sender, EventArgs e)
        {
            hdn_printfootr.Value = "";
            div_filUp_fu_printfootr.Visible = false;
            Application["PrintFooter"] = "";
            Upd_fu_printfootr.Update();
        }

        public void Clear_fu_MailFile()
        {
            hdnfu_MailFile.Value = "";
            if (hdnfu_MailFile.Value != "")
            {
                divfu_MailFile.Visible = true;
            }
            else
            {
                divfu_MailFile.Visible = false;
            }
            updfu_MailFile.Update();
        }

        public void fu_mailsign_OnFileUploaded(object sender, FileUploadedEventArgs e)
        {
            Clear_fu_MailFile();
            fu_MailFile.TargetFolder = "~/UploadedImage";

            string files_name = e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();
            e.File.SaveAs(Path.Combine(Server.MapPath(fu_MailFile.TargetFolder), files_name));

            try
            {
                //in backup folder also
                DataTable dtgen = obj_master.Edit_GeneralSettings();
                File.Copy((Path.Combine(Server.MapPath(fu_MailFile.TargetFolder), files_name)),
                    (Path.Combine(dtgen.Rows[0]["BackupDrivePath"].ToString() + "UploadedImage", files_name)), false);
            }
            catch (Exception cc) { }

            hdnfu_MailFile.Value = e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();

            if (hdnfu_MailFile.Value != "")
            {
                imgfu_MailFile.ImageUrl = "~/UploadedImage/" + hdnfu_MailFile.Value;
                divfu_MailFile.Visible = true;
            }
            else
            {
                divfu_MailFile.Visible = false;
            }

            updfu_MailFile.Update();
        }

        protected void btnfu_MailFileOnClick(object sender, EventArgs e)
        {
            hdnfu_MailFile.Value = "";
            divfu_MailFile.Visible = false;
            updfu_MailFile.Update();
        }

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.insert_Administration(Convert.ToInt32(hdn_id.Value), hdn_printHeader.Value,
               drpExpense.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpExpense.SelectedValue), Convert.ToInt32(hdn_user_id.Value),
               drp_paymode.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_paymode.SelectedValue),
                 Convert.ToInt32(chkTaxAppliedWithDiscount.Checked),
               txtSendAgreementExpiredMailBefore.Text == "" ? (int?)null : Convert.ToInt32(txtSendAgreementExpiredMailBefore.Text),
               Convert.ToInt32(chkscinvoice.Checked), hdn_printfootr.Value,
                txtDocExpireSecondaryMailDays.Text == "" ? (int?)null : Convert.ToInt32(txtDocExpireSecondaryMailDays.Text),
                Convert.ToInt32(chkEmpSC.Checked), Convert.ToInt32(chkdeltdSC.Checked),
                txtDefaultBankCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtDefaultBankCharge.Text),
                drprefundexpense.SelectedValue == "" ? (int?)null : Convert.ToInt32(drprefundexpense.SelectedValue),
                 Convert.ToInt32(chkIsMobileDupAllow.Checked), Convert.ToInt32(chkIstaxprintforall.Checked),
                  Convert.ToInt32(chkIsAllowSCAmountExceed.Checked), hdnfu_MailFile.Value,
               Convert.ToInt32(chkIsCommissionEditableInInvoice.Checked),
               drpAdminDesign.SelectedValue == "" ? 1 : Convert.ToInt32(drpAdminDesign.SelectedValue),
                drpTemplate.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpTemplate.SelectedValue),
                 txtmail.Text, txtCompanyEmailPwd.Text, txtccmail.Text, txtTRN.Text,
                 txtTRN.Text != "" ? 1 : Convert.ToInt32(drpDefaultInvoiceType.SelectedValue), txtCompanyname.Text,
                 txtCompanyPhone.Text, txtCompanyContactPerson.Text, txtVATOB.Text == "" ? (decimal?)null :
                 Convert.ToDecimal(txtVATOB.Text), radVATOBDate.SelectedDate
                  );
            if (res == 1)
            {
                fill_Data();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            Upd_Add_Panel.Update();
        }

        //Check Privilege
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.Form_Previlage_Validation(20, Convert.ToInt32(hdn_user_id.Value));
                    if (val == 0)
                    {
                        Response.Redirect("~/Landing.aspx");
                    }
                }
                else
                {
                    Response.Redirect("~/Landing.aspx");
                }
            }
            catch
            {
                Response.Redirect("~/Landing.aspx");
            }
        }

        public void previlage_action_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    DataTable dt = obj_common.Action_Previlage_Validation(20, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                    }
                    btn_save.Visible = hdn_add.Value == "0" ? false : true;
                }
                else
                {
                    Response.Redirect("../Login.aspx");
                }
            }
            catch
            {
                Response.Redirect("../Login.aspx");
            }
        }
    }
}