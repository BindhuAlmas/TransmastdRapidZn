using AmarCentre.BAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AmarCentre.Masters
{
    public partial class SoftwareConfiguration : System.Web.UI.Page
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
            DataTable dtExpense = obj_master.Drp_Expense();

            drpprofitExp.Items.Clear();
            drpprofitExp.DataSource = dtExpense;
            drpprofitExp.DataValueField = "Value";
            drpprofitExp.DataTextField = "Text";
            drpprofitExp.DataBind();

            drpCDType.DataSource = obj_master.Drp_Income();
            drpCDType.DataValueField = "Value";
            drpCDType.DataTextField = "Text";
            drpCDType.DataBind();

            drpEmirate.Items.Clear();
            drpEmirate.DataSource = obj_master.fillEmirate();
            drpEmirate.DataTextField = "Text";
            drpEmirate.DataValueField = "Value";
            drpEmirate.DataBind();
        }

        public void fill_Data()
        {
            DataTable dt = obj_master.Edit_GeneralSettings();
            if (dt.Rows.Count > 0)
            {
                hdn_id.Value = dt.Rows[0]["Id"].ToString();

                chk_SerComWOPayment.Checked = Convert.ToBoolean(dt.Rows[0]["SerComWOPayment"]);
                chk_SerPriceWTax.Checked = Convert.ToBoolean(dt.Rows[0]["ServicePriceWithTax"]);
                drpInvoiceFormat.SelectedValue = dt.Rows[0]["InvoiceFormat"].ToString();
                chkPrintTerms.Checked = Convert.ToBoolean(dt.Rows[0]["PrintTerms"]);
                chkDepartmentRequired.Checked = Convert.ToBoolean(dt.Rows[0]["DepartmentRequiredInService"]);
                chkCategoryRequired.Checked = Convert.ToBoolean(dt.Rows[0]["CategoryRequiredInService"]);
                chkSubCategoryRequired.Checked = Convert.ToBoolean(dt.Rows[0]["SubCategoryRequiredInService"]);
                drp_paymode.SelectedValue = dt.Rows[0]["DefaultPayModeInQuickReceipt"].ToString();
                chkDisplayDiscount.Checked = Convert.ToBoolean(dt.Rows[0]["DisplayDiscountInInvoice"]);
                chkremark.Checked = Convert.ToBoolean(dt.Rows[0]["IsAddRemark"]);
                chkCustinv.Checked = Convert.ToBoolean(dt.Rows[0]["EnableCustomerInvoice"]);
                drpReceiptFormat.SelectedValue = dt.Rows[0]["ReceiptFormat"].ToString();
                drpQuotationPrint.SelectedValue = dt.Rows[0]["QuotationFormat"].ToString();
                drpInvoiceFormatCI.SelectedValue = dt.Rows[0]["CIInvoiceFormat"].ToString();
                drpDebtorsReport.SelectedValue = dt.Rows[0]["DebitorsReportFormat"].ToString();
                drpRVPrint.SelectedValue = dt.Rows[0]["ReceiptVoucherFormat"].ToString();
                drpCustomerSOAPdfFormat.SelectedValue = dt.Rows[0]["CustomerSOAPdfFormat"].ToString();
                drpCDType.SelectedValue = dt.Rows[0]["CustomerDiscounttype"].ToString();
                drpCDType.Enabled= dt.Rows[0]["CustomerDiscounttype"].ToString()==""?true:false;

                chkIsSoftareNameAdd.Checked = Convert.ToBoolean(dt.Rows[0]["IsSoftareNameAdd"]);
                drpVenStmt.SelectedValue = dt.Rows[0]["VendorStmtFormat"].ToString();
                drpSalesorder.SelectedValue = dt.Rows[0]["SalesOrderPrint"].ToString();
                chktemplate.Checked = Convert.ToBoolean(dt.Rows[0]["IsTemplateView"]);
                txtscpredate.Text = dt.Rows[0]["SCPredateDays"].ToString();
                chkQutnEdit.Checked = Convert.ToBoolean(dt.Rows[0]["IsQuotaionEditable"]);
                txtQremark.Text = dt.Rows[0]["DefaultQutotnRemark"].ToString();
                txtDinvoiceremark.Text = dt.Rows[0]["DefaultInvoiceRemark"].ToString();
                Application["Company"] = dt.Rows[0]["CompanyName"].ToString();
                chkdepartmentInInvoiceVisible.Checked = Convert.ToBoolean(dt.Rows[0]["DepartmentInInvoiceVisible"]);
                chkIsDisplaySCStatus.Checked = Convert.ToBoolean(dt.Rows[0]["IsDisplaySCStatus"]);
                txtTransEditdaylimit.Text = dt.Rows[0]["TransEditdaylimit"].ToString();
                chkIsDisableRoundOff.Checked = Convert.ToBoolean(dt.Rows[0]["IsDisableRoundOff"]);
                drpscview.SelectedValue = dt.Rows[0]["SCView"].ToString();
                chkIsHideServiceAmtInSC.Checked = Convert.ToBoolean(dt.Rows[0]["IsHideServiceAmtInSC"]);
                drpAgentCommission.SelectedValue = dt.Rows[0]["AgentCommission"].ToString();
               drpAgentCommission.Enabled = string.IsNullOrEmpty(dt.Rows[0]["AgentCommission"].ToString());
                drpprofitExp.SelectedValue = dt.Rows[0]["ProfitExpenseType"].ToString();
                chkIsEditInvoiceCreator.Checked = Convert.ToBoolean(dt.Rows[0]["IsEditInvoiceCreator"]);
                drpEmirate.SelectedValue = dt.Rows[0]["DefaultEmirate"].ToString();
                chkIncentivePercentage.Checked = Convert.ToBoolean(dt.Rows[0]["IncentivePercentage"]);
                chkIsAddCreatedByInInvoicePrint.Checked = Convert.ToBoolean(dt.Rows[0]["IsAddCreatedByInInvoicePrint"]);
                chkIsSCViewDepartmentBase.Checked = Convert.ToBoolean(dt.Rows[0]["IsSCViewDepartmentBase"]);

            }
        }
        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.insert_SoftwareConfiguration(Convert.ToInt32(hdn_id.Value),
                Convert.ToInt32(chk_SerComWOPayment.Checked), 
                Convert.ToInt32(chk_SerPriceWTax.Checked),
                drpInvoiceFormat.SelectedValue == "" ? 1 : Convert.ToInt32(drpInvoiceFormat.SelectedValue),
                Convert.ToInt32(chkPrintTerms.Checked), Convert.ToInt32(chkDepartmentRequired.Checked),
                Convert.ToInt32(chkCategoryRequired.Checked), Convert.ToInt32(chkSubCategoryRequired.Checked),
                Convert.ToInt32(hdn_user_id.Value), drp_paymode.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_paymode.SelectedValue),
                 Convert.ToInt32(chkDisplayDiscount.Checked), 
                drpCustomerSOAPdfFormat.SelectedValue == "" ? 1 : Convert.ToInt32(drpCustomerSOAPdfFormat.SelectedValue),
                 Convert.ToInt32(chkremark.Checked), Convert.ToInt32(chkCustinv.Checked),
                drpReceiptFormat.SelectedValue == "" ? 1 : Convert.ToInt32(drpReceiptFormat.SelectedValue),
                 drpQuotationPrint.SelectedValue == "" ? 1 : Convert.ToInt32(drpQuotationPrint.SelectedValue),
                  drpInvoiceFormatCI.SelectedValue == "" ? 2 : Convert.ToInt32(drpInvoiceFormatCI.SelectedValue),
                drpDebtorsReport.SelectedValue == "" ? 1 : Convert.ToInt32(drpDebtorsReport.SelectedValue),
                 drpRVPrint.SelectedValue == "" ? 1 : Convert.ToInt32(drpRVPrint.SelectedValue),
                 drpVenStmt.SelectedValue == "" ? 1 : Convert.ToInt32(drpVenStmt.SelectedValue),
                 drpSalesorder.SelectedValue == "" ? 1 : Convert.ToInt32(drpSalesorder.SelectedValue), 
                 Convert.ToInt32(chktemplate.Checked), txtscpredate.Text == "" ? 365 : Convert.ToInt32(txtscpredate.Text),
                 Convert.ToInt32(chkQutnEdit.Checked), txtQremark.Text, txtDinvoiceremark.Text,  Convert.ToInt32(chkdepartmentInInvoiceVisible.Checked),
                  Convert.ToInt32(chkIsDisplaySCStatus.Checked),
                   txtTransEditdaylimit.Text == "" ? (int?)null : Convert.ToInt32(txtTransEditdaylimit.Text), 
                Convert.ToInt32(chkIsDisableRoundOff.Checked), drpscview.SelectedValue == "" ? 1 : Convert.ToInt32(drpscview.SelectedValue),
                Convert.ToInt32(chkIsHideServiceAmtInSC.Checked), drpAgentCommission.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpAgentCommission.SelectedValue)
                 ,  drpprofitExp.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpprofitExp.SelectedValue),
                 Convert.ToInt32(chkIsEditInvoiceCreator.Checked),
                  drpCDType.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpCDType.SelectedValue),
                   drpEmirate.SelectedValue==""?(int?)null:Convert.ToInt32(drpEmirate.SelectedValue) ,
                   Convert.ToInt32(chkIncentivePercentage.Checked),Convert.ToInt32(chkIsAddCreatedByInInvoicePrint.Checked),
                   Convert.ToInt32(chkIsSCViewDepartmentBase.Checked) );
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

                    int val = obj_common.Form_Previlage_Validation(129, Convert.ToInt32(hdn_user_id.Value));
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

                    DataTable dt = obj_common.Action_Previlage_Validation(129, Convert.ToInt32(hdn_user_id.Value));
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