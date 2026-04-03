using AmarCentre.BAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;

namespace AmarCentre.Masters
{
    public partial class GeneralSettings : System.Web.UI.Page
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
            drpTemplate.Items.Clear();
            DataTable dtExpense = obj_master.DrpTemplate();
            drpTemplate.DataSource = dtExpense;
            drpTemplate.DataValueField = "Value";
            drpTemplate.DataTextField = "Text";
            drpTemplate.DataBind();
            drpTemplate.Text = "";

            drpExpense.Items.Clear();
            dtExpense = obj_master.Drp_Expense();
            drpExpense.DataSource = dtExpense;
            drpExpense.DataValueField = "Value";
            drpExpense.DataTextField = "Text";
            drpExpense.DataBind();

            drpprofitExp.Items.Clear();
            drpprofitExp.DataSource = dtExpense;
            drpprofitExp.DataValueField = "Value";
            drpprofitExp.DataTextField = "Text";
            drpprofitExp.DataBind();

            drprefundexpense.Items.Clear();
            drprefundexpense.DataSource = dtExpense;
            drprefundexpense.DataValueField = "Value";
            drprefundexpense.DataTextField = "Text";
            drprefundexpense.DataBind();

            drpCDType.DataSource = obj_master.Drp_Income();
            drpCDType.DataValueField = "Value";
            drpCDType.DataTextField = "Text";
            drpCDType.DataBind();

            drpAdminDesign.DataSource = obj_master.Drp_Design();
            drpAdminDesign.DataTextField = "text";
            drpAdminDesign.DataValueField = "value";
            drpAdminDesign.DataBind();

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

                chk_SerComWOPayment.Checked= Convert.ToBoolean(dt.Rows[0]["SerComWOPayment"]);
                drpExpense.SelectedValue = dt.Rows[0]["FineExpenseType"].ToString();
                chk_SerPriceWTax.Checked = Convert.ToBoolean(dt.Rows[0]["ServicePriceWithTax"]);
                drpInvoiceFormat.SelectedValue = dt.Rows[0]["InvoiceFormat"].ToString();
                chkPrintTerms.Checked = Convert.ToBoolean(dt.Rows[0]["PrintTerms"]);
                chkDepartmentRequired.Checked = Convert.ToBoolean(dt.Rows[0]["DepartmentRequiredInService"]);
                chkCategoryRequired.Checked = Convert.ToBoolean(dt.Rows[0]["CategoryRequiredInService"]);
                chkSubCategoryRequired.Checked = Convert.ToBoolean(dt.Rows[0]["SubCategoryRequiredInService"]);
                drp_paymode.SelectedValue = dt.Rows[0]["DefaultPayModeInQuickReceipt"].ToString();
                chkDisplayDiscount.Checked = Convert.ToBoolean(dt.Rows[0]["DisplayDiscountInInvoice"]);
                chkTaxAppliedWithDiscount.Checked = Convert.ToBoolean(dt.Rows[0]["TaxAppliedWithDiscount"]);
                chkscinvoice.Checked = Convert.ToBoolean(dt.Rows[0]["SCInInvoice"]);
                chkremark.Checked = Convert.ToBoolean(dt.Rows[0]["IsAddRemark"]);
                chkCustinv.Checked = Convert.ToBoolean(dt.Rows[0]["EnableCustomerInvoice"]);
                drpReceiptFormat.SelectedValue = dt.Rows[0]["ReceiptFormat"].ToString();
                drpQuotationPrint.SelectedValue = dt.Rows[0]["QuotationFormat"].ToString();
                drpInvoiceFormatCI.SelectedValue = dt.Rows[0]["CIInvoiceFormat"].ToString();
                drpDebtorsReport.SelectedValue = dt.Rows[0]["DebitorsReportFormat"].ToString();
                drpRVPrint.SelectedValue = dt.Rows[0]["ReceiptVoucherFormat"].ToString();
                drpDefaultInvoiceType.SelectedValue = dt.Rows[0]["InvoiceType"].ToString();
                drpCustomerSOAPdfFormat.SelectedValue = dt.Rows[0]["CustomerSOAPdfFormat"].ToString();
                drpCDType.SelectedValue = dt.Rows[0]["CustomerDiscounttype"].ToString();
                drpprofitExp.SelectedValue = dt.Rows[0]["ProfitExpenseType"].ToString();
                drpDefaultInvoiceType.Enabled = dt.Rows[0]["TRN"].ToString() != "" ? false : true;
                txtTRN.Text = dt.Rows[0]["TRN"].ToString();
                txtSendAgreementExpiredMailBefore.Text = dt.Rows[0]["SendAgreementExpiredMailBefore"].ToString();
                txtmail.Text = dt.Rows[0]["CompanyMail"].ToString();
                txtCompanyEmailPwd.Text = dt.Rows[0]["CompanyEmailPwd"].ToString();
                txtCompanyEmailPwd.Attributes["value"] = dt.Rows[0]["CompanyEmailPwd"].ToString();
                drpTemplate.SelectedValue = dt.Rows[0]["TemplateId"].ToString();
                chkIsSoftareNameAdd.Checked = Convert.ToBoolean(dt.Rows[0]["IsSoftareNameAdd"]);
                txtDocExpireSecondaryMailDays.Text = dt.Rows[0]["DocExpireSecondaryMailDays"].ToString();
                txtCompanyname.Text = dt.Rows[0]["CompanyName"].ToString();
                drpVenStmt.SelectedValue = dt.Rows[0]["VendorStmtFormat"].ToString();
                drpSalesorder.SelectedValue = dt.Rows[0]["SalesOrderPrint"].ToString();
                chkEmpSC.Checked = Convert.ToBoolean(dt.Rows[0]["IsEmployeeBasedSCList"]);
                chkdeltdSC.Checked = Convert.ToBoolean(dt.Rows[0]["ShowDeletedSC"]);
                txtDefaultBankCharge.Text = dt.Rows[0]["DefaultBankCharge"].ToString();
                chktemplate.Checked = Convert.ToBoolean(dt.Rows[0]["IsTemplateView"]);
                txtscpredate.Text = dt.Rows[0]["SCPredateDays"].ToString();
                chkQutnEdit.Checked = Convert.ToBoolean(dt.Rows[0]["IsQuotaionEditable"]);
                txtQremark.Text = dt.Rows[0]["DefaultQutotnRemark"].ToString();
                txtDinvoiceremark.Text = dt.Rows[0]["DefaultInvoiceRemark"].ToString();
                drprefundexpense.SelectedValue= dt.Rows[0]["RefundableExpenseId"].ToString();
                Application["Company"] = dt.Rows[0]["CompanyName"].ToString();
                chkdepartmentInInvoiceVisible.Checked = Convert.ToBoolean(dt.Rows[0]["DepartmentInInvoiceVisible"]);
                chkIsMobileDupAllow.Checked = Convert.ToBoolean(dt.Rows[0]["IsMobileDupAllow"]);
                chkIstaxprintforall.Checked = Convert.ToBoolean(dt.Rows[0]["IsTaxPrintForAll"]);
                chkIsDisplaySCStatus.Checked = Convert.ToBoolean(dt.Rows[0]["IsDisplaySCStatus"]);
                txtTransEditdaylimit.Text = dt.Rows[0]["TransEditdaylimit"].ToString();
                chkIsAllowSCAmountExceed.Checked =Convert.ToBoolean(dt.Rows[0]["IsAllowSCAmountExceed"]);
                chkIsDisableRoundOff.Checked = Convert.ToBoolean(dt.Rows[0]["IsDisableRoundOff"]);
                drpscview.SelectedValue = dt.Rows[0]["SCView"].ToString();
                chkIsCommissionEditableInInvoice.Checked = Convert.ToBoolean(dt.Rows[0]["IsCommissionEditableInInvoice"]);
                drpAdminDesign.SelectedValue = dt.Rows[0]["AdminDesginId"].ToString();
                chkIsHideServiceAmtInSC.Checked = Convert.ToBoolean(dt.Rows[0]["IsHideServiceAmtInSC"]);

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
            int res = obj_master.Insert_GeneralSettings(Convert.ToInt32(hdn_id.Value), hdn_printHeader.Value,
                Convert.ToInt32(chk_SerComWOPayment.Checked), drpExpense.SelectedValue==""?(int?)null: Convert.ToInt32(drpExpense.SelectedValue),
                Convert.ToInt32(chk_SerPriceWTax.Checked),
                drpInvoiceFormat.SelectedValue == "" ? 1 : Convert.ToInt32(drpInvoiceFormat.SelectedValue),
                Convert.ToInt32(chkPrintTerms.Checked), Convert.ToInt32(chkDepartmentRequired.Checked),
                Convert.ToInt32(chkCategoryRequired.Checked), Convert.ToInt32(chkSubCategoryRequired.Checked),
                txtTRN.Text, Convert.ToInt32(hdn_user_id.Value), drp_paymode.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_paymode.SelectedValue),
                 Convert.ToInt32(chkDisplayDiscount.Checked), Convert.ToInt32(chkTaxAppliedWithDiscount.Checked),
                txtTRN.Text != "" ? 1 : Convert.ToInt32(drpDefaultInvoiceType.SelectedValue),
                drpCustomerSOAPdfFormat.SelectedValue == "" ? 1 : Convert.ToInt32(drpCustomerSOAPdfFormat.SelectedValue),
                txtSendAgreementExpiredMailBefore.Text == "" ? (int?)null : Convert.ToInt32(txtSendAgreementExpiredMailBefore.Text),
                drpTemplate.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpTemplate.SelectedValue),
                Convert.ToInt32(chkscinvoice.Checked), Convert.ToInt32(chkremark.Checked), Convert.ToInt32(chkCustinv.Checked),
                txtmail.Text, drpReceiptFormat.SelectedValue == "" ? 1 : Convert.ToInt32(drpReceiptFormat.SelectedValue),
                 drpQuotationPrint.SelectedValue == "" ? 1 : Convert.ToInt32(drpQuotationPrint.SelectedValue),
                 hdn_printfootr.Value, drpInvoiceFormatCI.SelectedValue == "" ? 2 : Convert.ToInt32(drpInvoiceFormatCI.SelectedValue),
                 txtCompanyEmailPwd.Text, drpDebtorsReport.SelectedValue == "" ? 1 : Convert.ToInt32(drpDebtorsReport.SelectedValue),
                 drpRVPrint.SelectedValue == "" ? 1 : Convert.ToInt32(drpRVPrint.SelectedValue),
                 drpCDType.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpCDType.SelectedValue),
                 txtDocExpireSecondaryMailDays.Text == "" ? (int?)null : Convert.ToInt32(txtDocExpireSecondaryMailDays.Text),
                 drpprofitExp.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpprofitExp.SelectedValue), Convert.ToInt32(chkIsSoftareNameAdd.Checked),
                 txtCompanyname.Text, drpVenStmt.SelectedValue == "" ? 1 : Convert.ToInt32(drpVenStmt.SelectedValue),
                 drpSalesorder.SelectedValue == "" ? 1 : Convert.ToInt32(drpSalesorder.SelectedValue), Convert.ToInt32(chkEmpSC.Checked),
                 Convert.ToInt32(chkdeltdSC.Checked), txtDefaultBankCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtDefaultBankCharge.Text),
                 Convert.ToInt32(chktemplate.Checked), txtscpredate.Text == "" ? 365 : Convert.ToInt32(txtscpredate.Text),
                 Convert.ToInt32(chkQutnEdit.Checked), txtQremark.Text, txtDinvoiceremark.Text,drprefundexpense.SelectedValue==""?
                 (int?)null:Convert.ToInt32(drprefundexpense.SelectedValue), Convert.ToInt32(chkdepartmentInInvoiceVisible.Checked),
                  Convert.ToInt32(chkIsMobileDupAllow.Checked),Convert.ToInt32(chkIstaxprintforall.Checked), Convert.ToInt32(chkIsDisplaySCStatus.Checked),
                   txtTransEditdaylimit.Text==""?(int?)null:Convert.ToInt32(txtTransEditdaylimit.Text),Convert.ToInt32(chkIsAllowSCAmountExceed.Checked),
                Convert.ToInt32(chkIsDisableRoundOff.Checked),drpscview.SelectedValue==""?1:Convert.ToInt32(drpscview.SelectedValue),
                hdnfu_MailFile.Value, Convert.ToInt32(chkIsCommissionEditableInInvoice.Checked),
                drpAdminDesign.SelectedValue ==""?1:Convert.ToInt32(drpAdminDesign.SelectedValue),
                Convert.ToInt32(chkIsHideServiceAmtInSC.Checked)  
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