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

namespace AmarCentre.Transactions
{
    public partial class QuickReceipt : System.Web.UI.Page
    {
        Transaction_Bal obj_trans = new Transaction_Bal();
        Master_Bal obj_mas = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
        Report_Bal obj_report = new Report_Bal();
        Voucher BalVoucher = new Voucher();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
            }
            if (!IsPostBack)
            {
                hdn_user_id.Value = Session["User_Id"].ToString();
                hdnLanguage.Value = GetLanguage(Convert.ToInt32(hdn_user_id.Value));
                hdnSerPriceWTax.Value = GetServicePriceWithTax();
                previlage_check();
                previlage_action_check();
                fill_Customer();
                fill_Templates();
                OnpageLoad();
                Clear();
                grid_fill(1, 10, "", "", "");
            }
        }

        public string GetLanguage(int UserId)
        {
            DataTable dt = obj_trans.GetEmployeeLanguage(UserId);
            return dt.Rows[0][0].ToString();
        }

        public string GetServicePriceWithTax()
        {
            DataTable dt = obj_trans.GetServicePriceWithTax();
            return dt.Rows[0][0].ToString();
        }

        public void OnpageLoad()
        {
            DataTable dt = obj_mas.Edit_GeneralSettings();
            hdnTaxAppliedWithDiscount.Value = dt.Rows[0]["TaxAppliedWithDiscount"].ToString();
            hdnDefaultInvoiceType.Value = dt.Rows[0]["InvoiceType"].ToString();
            hdnSCInInvoice.Value = dt.Rows[0]["SCInInvoice"].ToString();
        }

        //Get List 
        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_trans.Get_List_InvoiceREceipt(page_number, page_size, filter, column, order, Convert.ToInt32(hdn_user_id.Value));
            rpt_list.DataSource = dt;
            rpt_list.DataBind();
            if (dt.Rows.Count > 0)
            {
                lbl_page_info.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["RowNum"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
                hdn_filter.Value = dt.Rows[0]["filter"].ToString();
                Common_order_column.Value = dt.Rows[0]["column_name"].ToString();
                Common_asc_desc.Value = dt.Rows[0]["asc_desc"].ToString();
                hdn_last_page.Value = dt.Rows[0]["last_page"].ToString();
                lbl_page_number.Text = dt.Rows[0]["page_number"].ToString();
                hdn_total.Value = dt.Rows[0]["current_count"].ToString();
            }
            else
            {
                lbl_page_info.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
                hdn_filter.Value = txt_search.Text;
                hdn_last_page.Value = "0";
                lbl_page_number.Text = "1";
                hdn_total.Value = "0";
            }
            Upd_List_Panel.Update();
            Upd_Nav_Panel.Update();
        }

        /*Export To Excel*/
        public void btnexcel_export_OnClick(object sender, EventArgs e)
        {
            DataTable dt = obj_trans.Get_List_invoice_Excel(Convert.ToInt32(hdn_user_id.Value));
            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "Invoice");
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                HttpContext.Current.Response.Write(style);
                Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        /*rpt_list OnItemCommand*/
        protected void rpt_list_OnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdn_id");
            HiddenField hdn_rec_id = (HiddenField)e.Item.FindControl("hdn_rec_id");

            if (e.CommandName == "Edit")
            {
                Clear();
                pnl_add.Visible = true;

                DataSet ds = obj_trans.Edit_InvoiceREceipt(Convert.ToInt32(hdn_rpt_id.Value), Convert.ToInt32(hdnLanguage.Value), Convert.ToInt32(hdnSerPriceWTax.Value));
                DataTable dt1 = ds.Tables[0];/*invoic*/
                DataTable dt_ser = ds.Tables[1];/* Detail*/

                hdn_id.Value = dt1.Rows[0]["Id"].ToString();
                lbl_Code.Text = dt1.Rows[0]["Code"].ToString();
                job_date.DbSelectedDate = dt1.Rows[0]["Dated"].ToString();
                drp_customer.SelectedValue = dt1.Rows[0]["Customer_Id"].ToString();
                hdn_CurrentInvoiceReceivable.Value = dt1.Rows[0]["Receivable"].ToString();
                drp_customer_OnSelectedIndexChanged(null, null);
                drp_customer.Enabled = false;

                fill_Edit_Quotation(drp_customer.SelectedValue == "" ? 0 : Convert.ToInt32(drp_customer.SelectedValue), Convert.ToInt32(hdn_rpt_id.Value));
                drp_quot.SelectedValue = dt1.Rows[0]["Quotation_id"].ToString();
                drp_quot.Enabled = drp_quot.SelectedValue == "" ? true : false;
                rbTaxInvoice.Checked = true;
                rbNormalInvoice.Checked = false;
                if (dt1.Rows[0]["InvoiceType"].ToString() == "1")
                {
                    rbTaxInvoice.Checked = true;
                    rbNormalInvoice.Checked = false;
                }
                else if (dt1.Rows[0]["InvoiceType"].ToString() == "2")
                {
                    rbTaxInvoice.Checked = false;
                    rbNormalInvoice.Checked = true;
                }
                txt_grand.Text = dt1.Rows[0]["Grand_Total"].ToString();
                txt_remark.Text = dt1.Rows[0]["Remarks"].ToString();

                rpt_Item_list.DataSource = dt_ser;
                rpt_Item_list.DataBind();

                if (hdn_IsCredit.Value == "1")
                    btn_TaxInvoicePrint.Visible = hdn_TaxInvoicePrint.Value == "0" ? false : true;
                else
                {
                    if ((dt1.Rows[0]["Received"].ToString() == "" ? "0" : dt1.Rows[0]["Received"].ToString()) ==
                        (dt1.Rows[0]["AfterDiscount_GrandTotal"].ToString() == "" ? "0" : dt1.Rows[0]["AfterDiscount_GrandTotal"].ToString()))
                        btn_TaxInvoicePrint.Visible = hdn_TaxInvoicePrint.Value == "0" ? false : true;
                    else
                        btn_TaxInvoicePrint.Visible = false;

                }
                btn_cancel.Visible = hdn_cancel.Value == "0" ? false : true;
                btn_history.Visible = hdn_histry.Value == "0" ? false : true;

                if (dt1.Rows[0]["Status"].ToString() == "2" || dt1.Rows[0]["Status"].ToString() == "3") // 2-cancel 3-delete
                {
                    btn_cancel.Visible = btn_save.Visible = false;
                }
                hdnInvoiceStatus.Value = dt1.Rows[0]["Status"].ToString();

                ds = obj_trans.Edit_ReceiptInv(Convert.ToInt32(dt1.Rows[0]["ReceiptId"]), Convert.ToInt32(hdn_user_id.Value));
                dt1 = ds.Tables[0];/*invoic*/
                dt_ser = ds.Tables[1];/* Detail*/
                if (dt1.Rows.Count > 0)
                {
                    lbl_RecCode.Text = dt1.Rows[0]["Code"].ToString();
                    txt_totDiscount.Text = dt1.Rows[0]["Total_Discount"].ToString();
                    txt_grand.Text = dt1.Rows[0]["AfterDiscount_GrandTotal"].ToString();
                    txt_remark.Text = dt1.Rows[0]["Remarks"].ToString();
                    hdn_receivedAmt.Value = dt1.Rows[0]["Received"].ToString();
                    txt_pendingAmt.Text = dt1.Rows[0]["PendingAmount"].ToString();
                    txt_amtPayNow.Text = dt1.Rows[0]["Amount"].ToString();
                    txt_ReceivedAmt.Text = dt1.Rows[0]["ReceivedAmount"].ToString();
                    txt_Balance.Text = dt1.Rows[0]["Balance"].ToString();
                    drp_payMode.SelectedValue = dt1.Rows[0]["PaymentModeId"].ToString();
                    drp_payMode_OnSelectedIndexChanged(null, null);
                    drpCardType.SelectedValue = dt1.Rows[0]["CardType"].ToString();
                    drp_cardType_OnSelectedIndexChanged(null, null);
                    drp_CardAcc.SelectedValue = dt1.Rows[0]["CardAccount"].ToString();

                    drpPettyCash.SelectedValue = dt1.Rows[0]["PettyCashId"].ToString();
                    drpBankAccount.SelectedValue = dt1.Rows[0]["AccountId"].ToString();
                    onchangedrp_bank(null, null);
                    cheque_date.DbSelectedDate = dt1.Rows[0]["ChequeDate"].ToString();
                    txt_chqNumber.Text = dt1.Rows[0]["ChequeNumber"].ToString();
                    txt_commsn.Text = dt1.Rows[0]["BankCommission"].ToString();
                }

                lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
                hdn_InvDetailId.Value = "-" + (rpt_Item_list.Items.Count + 1).ToString();

                btn_save.Visible = false;
                btn_Salesprint.Visible = hdn_Salesprint.Value == "0" ? false : true;
                btn_ReceiptPrint.Visible = hdn_Receiptprint.Value == "0" ? false : true;

                Upd_Add_Panel.Update();
            }
            else if (e.CommandName == "TaxInvoicePrint")
            {
                int Format = obj_trans.GetInvoiceFormat();
                string url = "";
                if (Format == 1)
                    url = "../Reports/TaxInvoiceFormat1.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 2)
                    url = "../Reports/TaxInvoiceFormat2.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
            }
            else if (e.CommandName == "SalesOrderPrint")
            {
                int Format = obj_trans.GetInvoiceFormat();
                string url = "";
                if (Format == 1)
                    url = "../Reports/SalesOrderFormat1.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 2)
                    url = "../Reports/SalesOrderFormat2.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
            }
            else if (e.CommandName == "Print")
            {
                string url = "";
                url = "../Reports/CashReceiptFormat1.aspx?id=" + Convert.ToInt32(hdn_rec_id.Value);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
            }
        }

        protected void rpt_list_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Button btnTaxInvoicePrint = (Button)e.Item.FindControl("btnTaxInvoicePrint");
                Button btnSalesOrderPrint = (Button)e.Item.FindControl("btnSalesOrderPrint");
                Button btnReceiptPrint = (Button)e.Item.FindControl("btnReceiptPrint");

                HiddenField hdnIsCredit = (HiddenField)e.Item.FindControl("hdnIsCredit");
                HiddenField hdnReceived = (HiddenField)e.Item.FindControl("hdnReceived");
                HiddenField hdnAfterDiscountGrandTotal = (HiddenField)e.Item.FindControl("hdnAfterDiscountGrandTotal");
                btnSalesOrderPrint.Visible = hdn_Salesprint.Value == "0" ? false : true;
                btnReceiptPrint.Visible = hdn_Receiptprint.Value == "0" ? false : true;

                if (hdnIsCredit.Value == "1")
                    btnTaxInvoicePrint.Visible = hdn_TaxInvoicePrint.Value == "0" ? false : true;
                else
                {
                    if ((hdnReceived.Value == "" ? "0" : hdnReceived.Value) ==
                        (hdnAfterDiscountGrandTotal.Value == "" ? "0" : hdnAfterDiscountGrandTotal.Value))
                        btnTaxInvoicePrint.Visible = hdn_TaxInvoicePrint.Value == "0" ? false : true;
                    else
                        btnTaxInvoicePrint.Visible = false;

                }
            }

        }

        protected void drp_customer_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            /*Change in here should be checked in Customer.ascx also*/
            pnl_CreditDetail.Visible = false;
            hdn_IsCredit.Value = "0";
            lblCreditLimit.Text = "";
            lblCurrentCreditAmt.Text = "";
            btn_TaxInvoicePrint.Visible = false;
            if (drp_customer.SelectedValue != "")
            {
                if (drp_customer.SelectedValue == "0")
                {
                    pnl_Customer.Visible = true;
                    UC_Customer.PageLoad(0);
                    Upd_Customer_Panel.Update();
                }
                else
                {
                    DataTable dt = obj_trans.Get_CustomerCreditDetail(Convert.ToInt32(drp_customer.SelectedValue));
                    if (dt.Rows.Count > 0)
                    {
                        decimal CurrentInvoiceCredit = hdn_CurrentInvoiceReceivable.Value == "" ? 0 : Convert.ToDecimal(hdn_CurrentInvoiceReceivable.Value);
                        decimal CurrentCredit = Convert.ToDecimal(dt.Rows[0]["Receivable"].ToString()) - CurrentInvoiceCredit;
                        hdn_IsCredit.Value = dt.Rows[0]["IsCredit"].ToString();
                        lblCreditLimit.Text = dt.Rows[0]["CreditAmount"].ToString();
                        lblCurrentCreditAmt.Text = CurrentCredit.ToString();
                        pnl_CreditDetail.Visible = hdn_IsCredit.Value == "1" ? true : false;
                        if (hdn_IsCredit.Value == "1")
                            btn_TaxInvoicePrint.Visible = hdn_TaxInvoicePrint.Value == "0" ? false : true;
                    }
                    fill_Quotation(Convert.ToInt32(drp_customer.SelectedValue));
                }
            }
            else
            {
                fill_Quotation(0);
            }
            Upd_btnTaxInvoicePrint.Update();
            Upd_CreditDetail_Panel.Update();
        }

        public bool CheckCreditAmount()
        {
            bool ProceedSave;
            if (hdn_IsCredit.Value == "1")
            {
                decimal PendingAmt = Convert.ToDecimal(txt_pendingAmt.Text);
                decimal PaidAmt = Convert.ToDecimal(txt_amtPayNow.Text);

                decimal CurrentCredit = Convert.ToDecimal(lblCurrentCreditAmt.Text);
                decimal CreditLimit = Convert.ToDecimal(lblCreditLimit.Text);

                if (((PendingAmt - PaidAmt) + CurrentCredit) > CreditLimit)
                {
                    ProceedSave = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Amount cannot be greater than Credit Limit');", true);
                }
                else
                {
                    ProceedSave = true;
                }
            }
            else
            {
                ProceedSave = true;
            }
            return ProceedSave;
        }

        public int SaveInvoice()
        {
            int res = 0, recid = 0;
            DataSet ds=fill_Detail();
            DataTable dt_deatils = ds.Tables[0];
            DataTable dtexpense = ds.Tables[1];
            DataTable dtTrans = ds.Tables[2];

            if (dt_deatils.Rows.Count > 0)
            {
                if (drpService.SelectedValue == "" && (txt_displayPrice.Text != "" || txt_Qty.Text != ""))
                {
                    InlineCalculation();
                }
                if (hdnSCInInvoice.Value == "1")
                {
                    res = obj_trans.Insert_Update_Invoice_recSC(Convert.ToInt32(hdn_id.Value), DateTime.ParseExact(CalDate(job_date), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Convert.ToInt32(drp_customer.SelectedValue), txt_remark.Text, Convert.ToInt32(hdn_user_id.Value), Convert.ToDecimal(txt_grand.Text),
                    dt_deatils, drp_quot.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_quot.SelectedValue),
                    rbTaxInvoice.Checked == true ? 1 : 2, Convert.ToInt32(hdnTaxAppliedWithDiscount.Value), dtexpense, dtTrans);
                }
                else
                {
                    dt_deatils.Columns.Remove("SerComDate");
                    dt_deatils.Columns.Remove("ExpQty");
                    dt_deatils.Columns.Remove("ExpSinglAmt");
                    dt_deatils.Columns.Remove("ExpTotAmt");

                    res = obj_trans.Insert_Update_Invoice_rec(Convert.ToInt32(hdn_id.Value), DateTime.ParseExact(CalDate(job_date), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                       Convert.ToInt32(drp_customer.SelectedValue), txt_remark.Text, Convert.ToInt32(hdn_user_id.Value), Convert.ToDecimal(txt_grand.Text),
                       dt_deatils, drp_quot.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_quot.SelectedValue),
                       rbTaxInvoice.Checked == true ? 1 : 2, Convert.ToInt32(hdnTaxAppliedWithDiscount.Value));
                }
                if (res > 0 & hdn_id.Value == "0")
                {
                    recid = obj_trans.Insert_Update_Receipt_inv(Convert.ToInt32(hdn_id.Value), DateTime.ParseExact(CalDate(job_date), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    res, txt_remark.Text, txt_totDiscount.Text == "" ? (decimal?)null : Convert.ToDecimal(txt_totDiscount.Text),
                    Convert.ToDecimal(txt_grand.Text), Convert.ToDecimal(txt_amtPayNow.Text),
                    Convert.ToInt32(drp_payMode.SelectedValue), drpBankAccount.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpBankAccount.SelectedValue),
                    drpPettyCash.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpPettyCash.SelectedValue),
                    drp_payMode.SelectedValue == "3" ? DateTime.ParseExact(CalDate(job_date), "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null,
                    drp_payMode.SelectedValue == "3" ? txt_chqNumber.Text : "",
                    Convert.ToDecimal(txt_pendingAmt.Text), Convert.ToDecimal(txt_ReceivedAmt.Text), Convert.ToDecimal(txt_Balance.Text), Convert.ToInt32(hdn_user_id.Value)
                    , txt_commsn.Text == "" ? (decimal?)null : Convert.ToDecimal(txt_commsn.Text),
                    Convert.ToInt32(drpCardType.SelectedValue), drp_CardAcc.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_CardAcc.SelectedValue));
                }
            }
            else
            {
                lbl_msgin.Text = "Add Service to Continue !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            return res;
        }

        public int SaveReceipt()
        {
            int res = 0, recid = 0;
            DataSet dsF = fill_Detail();
            DataTable dt_deatils = dsF.Tables[0];
            DataTable dtexpense = dsF.Tables[1];
            DataTable dtTrans = dsF.Tables[2];
            if (dt_deatils.Rows.Count > 0)
            {
                if (drpService.SelectedValue == "" && (txt_displayPrice.Text != "" || txt_Qty.Text != ""))
                {
                    InlineCalculation();
                }
                if (hdnSCInInvoice.Value == "1")
                {
                    res = obj_trans.Insert_Update_Invoice_recSC(Convert.ToInt32(hdn_id.Value), DateTime.ParseExact(CalDate(job_date), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Convert.ToInt32(drp_customer.SelectedValue), txt_remark.Text, Convert.ToInt32(hdn_user_id.Value), Convert.ToDecimal(txt_grand.Text),
                    dt_deatils, drp_quot.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_quot.SelectedValue),
                    rbTaxInvoice.Checked == true ? 1 : 2, Convert.ToInt32(hdnTaxAppliedWithDiscount.Value), dtexpense, dtTrans);
                }
                else
                {
                    dt_deatils.Columns.Remove("SerComDate");
                    dt_deatils.Columns.Remove("ExpQty");
                    dt_deatils.Columns.Remove("ExpSinglAmt");
                    dt_deatils.Columns.Remove("ExpTotAmt");

                    res = obj_trans.Insert_Update_Invoice_rec(Convert.ToInt32(hdn_id.Value), DateTime.ParseExact(CalDate(job_date), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                       Convert.ToInt32(drp_customer.SelectedValue), txt_remark.Text, Convert.ToInt32(hdn_user_id.Value), Convert.ToDecimal(txt_grand.Text),
                       dt_deatils, drp_quot.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_quot.SelectedValue),
                       rbTaxInvoice.Checked == true ? 1 : 2, Convert.ToInt32(hdnTaxAppliedWithDiscount.Value));
                }
                if (res > 0 & hdn_id.Value == "0")
                {
                    recid = obj_trans.Insert_Update_Receipt_inv(Convert.ToInt32(hdn_id.Value), DateTime.ParseExact(CalDate(job_date), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    res, txt_remark.Text, txt_totDiscount.Text == "" ? (decimal?)null : Convert.ToDecimal(txt_totDiscount.Text),
                    Convert.ToDecimal(txt_grand.Text), Convert.ToDecimal(txt_amtPayNow.Text),
                    Convert.ToInt32(drp_payMode.SelectedValue), drpBankAccount.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpBankAccount.SelectedValue),
                    drpPettyCash.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpPettyCash.SelectedValue),
                    drp_payMode.SelectedValue == "3" ? DateTime.ParseExact(CalDate(job_date), "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null,
                    drp_payMode.SelectedValue == "3" ? txt_chqNumber.Text : "",
                    Convert.ToDecimal(txt_pendingAmt.Text), Convert.ToDecimal(txt_ReceivedAmt.Text), Convert.ToDecimal(txt_Balance.Text), Convert.ToInt32(hdn_user_id.Value)
                    , txt_commsn.Text == "" ? (decimal?)null : Convert.ToDecimal(txt_commsn.Text),
                    Convert.ToInt32(drpCardType.SelectedValue), drp_CardAcc.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_CardAcc.SelectedValue));
                }
                else if (res > 0 & hdn_id.Value != "0")
                {
                    DataSet ds = obj_trans.Edit_InvoiceREceipt(res, Convert.ToInt32(hdnLanguage.Value), Convert.ToInt32(hdnSerPriceWTax.Value));
                    DataTable dt1 = ds.Tables[0];/*invoic*/

                    recid = Convert.ToInt32(dt1.Rows[0]["ReceiptId"]);
                }
            }
            else
            {
                lbl_msgin.Text = "Add Service to Continue !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            return recid;
        }

        /*Data To Save*/
        public DataSet fill_Detail()
        {
            DataTable dt_ser = new DataTable();
            dt_ser.Columns.Add("D_id", typeof(int));
            dt_ser.Columns.Add("CategoryId", typeof(int));
            dt_ser.Columns.Add("Service_Id", typeof(int));
            dt_ser.Columns.Add("Particulars", typeof(string));
            dt_ser.Columns.Add("Price", typeof(decimal));
            dt_ser.Columns.Add("Expense", typeof(decimal));
            dt_ser.Columns.Add("ServiceCharge", typeof(decimal));
            dt_ser.Columns.Add("AdditionalServiceCharge", typeof(decimal));
            dt_ser.Columns.Add("Quantity", typeof(decimal));
            dt_ser.Columns.Add("TaxAmount", typeof(decimal));
            dt_ser.Columns.Add("PriceWitTax", typeof(decimal));
            dt_ser.Columns.Add("Fine", typeof(decimal));
            dt_ser.Columns.Add("Total", typeof(decimal));
            dt_ser.Columns.Add("Discount", typeof(decimal));
            dt_ser.Columns.Add("Tax", typeof(decimal));

            //sc
            dt_ser.Columns.Add("SerComDate", typeof(DateTime));
            dt_ser.Columns.Add("ExpQty", typeof(int));
            dt_ser.Columns.Add("ExpSinglAmt", typeof(decimal));
            dt_ser.Columns.Add("ExpTotAmt", typeof(decimal));

            DataTable dt_exp = new DataTable();
            dt_exp.Columns.Add("InvDId", typeof(int));
            dt_exp.Columns.Add("SerComDetailId", typeof(int));
            dt_exp.Columns.Add("ExpenseId", typeof(int));
            dt_exp.Columns.Add("Amount", typeof(decimal));
            dt_exp.Columns.Add("VAT", typeof(decimal));
            dt_exp.Columns.Add("VendorId", typeof(int));
            dt_exp.Columns.Add("PayModeId", typeof(int));
            dt_exp.Columns.Add("AccountId", typeof(int));
            dt_exp.Columns.Add("PayableAmount", typeof(decimal));
            dt_exp.Columns.Add("PaidAmount", typeof(decimal));

            DataTable dt_trans = new DataTable();
            dt_trans.Columns.Add("InvDId", typeof(int));
            dt_trans.Columns.Add("TransactionNumber", typeof(string));

            if (rpt_Item_list.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_Item_list.Items)
                {
                    HiddenField hdnInvDId = (HiddenField)itm.FindControl("hdnInvDId");
                    HiddenField hdnInvDCategoryId = (HiddenField)itm.FindControl("hdnInvDCategoryId");
                    HiddenField hdnInvDServiceId = (HiddenField)itm.FindControl("hdnInvDServiceId");
                    Label lblInvDdesc = (Label)itm.FindControl("lblInvDdesc");
                    TextBox txtInvDDisplayPrice = (TextBox)itm.FindControl("txtInvDDisplayPrice");
                    HiddenField hdnInvDExpense = (HiddenField)itm.FindControl("hdnInvDExpense");
                    HiddenField hdnInvDServiceCharge = (HiddenField)itm.FindControl("hdnInvDServiceCharge");
                    HiddenField hdnInvDPrice = (HiddenField)itm.FindControl("hdnInvDPrice");
                    TextBox txtInvDAddServiceCharge = (TextBox)itm.FindControl("txtInvDAddServiceCharge");
                    TextBox txtInvDQty = (TextBox)itm.FindControl("txtInvDQty");
                    TextBox txtInvDTaxAmount = (TextBox)itm.FindControl("txtInvDTaxAmount");
                    TextBox txtInvDPriceWitTax = (TextBox)itm.FindControl("txtInvDPriceWitTax");
                    TextBox txtInvDFine = (TextBox)itm.FindControl("txtInvDFine");
                    TextBox txtInvDTotal = (TextBox)itm.FindControl("txtInvDTotal");
                    TextBox txtInvDdiscount = (TextBox)itm.FindControl("txtInvDdiscount");
                    HiddenField hdnInvDTax = (HiddenField)itm.FindControl("hdnInvDTax");

                    //sc
                    TextBox txtExpenseQty = (TextBox)itm.FindControl("txtExpenseQty");
                    TextBox txtExpenseSinglAmt = (TextBox)itm.FindControl("txtExpenseSinglAmt");
                    TextBox txtExpenseTotalAmt = (TextBox)itm.FindControl("txtExpenseTotalAmt");
                    RadDatePicker ExpenseSerComDate = (RadDatePicker)itm.FindControl("ExpenseSerComDate");

                    Repeater rptTransCode = (Repeater)itm.FindControl("rptTransCode");
                    Repeater rptexpensein = (Repeater)itm.FindControl("rptexpensein");


                    dt_ser.Rows.Add(Convert.ToInt32(hdnInvDId.Value), hdnInvDCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDCategoryId.Value),
                    Convert.ToInt32(hdnInvDServiceId.Value), lblInvDdesc.Text, Convert.ToDecimal(hdnInvDPrice.Value), Convert.ToDecimal(hdnInvDExpense.Value),
                    Convert.ToDecimal(hdnInvDServiceCharge.Value), txtInvDAddServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDAddServiceCharge.Text),
                    Convert.ToDecimal(txtInvDQty.Text), Convert.ToDecimal(txtInvDTaxAmount.Text),
                    Convert.ToDecimal(txtInvDPriceWitTax.Text), txtInvDFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDFine.Text), Convert.ToDecimal(txtInvDTotal.Text),
                     txtInvDdiscount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDdiscount.Text),
                     Convert.ToDecimal(hdnInvDTax.Value),

                         ExpenseSerComDate.SelectedDate == null ? (DateTime?)null : DateTime.ParseExact(CalDate(ExpenseSerComDate), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                   txtExpenseQty.Text == "" ? (decimal?)null : Convert.ToDecimal(txtExpenseQty.Text),
                   txtExpenseSinglAmt.Text == "" ? (decimal?)null : Convert.ToDecimal(txtExpenseSinglAmt.Text),
                   txtExpenseTotalAmt.Text == "" ? (decimal?)null : Convert.ToDecimal(txtExpenseTotalAmt.Text));

                    foreach (RepeaterItem itms in rptTransCode.Items)
                    {
                        TextBox txtTransCode = (TextBox)itms.FindControl("txtTransCode");
                        dt_trans.Rows.Add(Convert.ToInt32(hdnInvDId.Value), txtTransCode.Text);
                    }

                    foreach (RepeaterItem itms in rptexpensein.Items)
                    {
                        TextBox txtSerComDetailId = (TextBox)itms.FindControl("txtSerComDetailId");
                        TextBox txtExpenseId = (TextBox)itms.FindControl("txtExpenseId");
                        TextBox txtAmount = (TextBox)itms.FindControl("txtAmount");
                        TextBox txtVAT = (TextBox)itms.FindControl("txtVAT");
                        TextBox txtVendorId = (TextBox)itms.FindControl("txtVendorId");
                        TextBox txtPayModeId = (TextBox)itms.FindControl("txtPayModeId");
                        TextBox txtAccountId = (TextBox)itms.FindControl("txtAccountId");
                        TextBox txtPayableAmount = (TextBox)itms.FindControl("txtPayableAmount");
                        TextBox txtPaidAmount = (TextBox)itms.FindControl("txtPaidAmount");

                        dt_exp.Rows.Add(Convert.ToInt32(hdnInvDId.Value),
                             txtSerComDetailId.Text == "" ? (int?)null : Convert.ToInt32(txtSerComDetailId.Text),
                             txtExpenseId.Text == "" ? (int?)null : Convert.ToInt32(txtExpenseId.Text),
                              txtAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtAmount.Text),
                              txtVAT.Text == "" ? (decimal?)null : Convert.ToDecimal(txtVAT.Text),
                              txtVendorId.Text == "" ? (int?)null : Convert.ToInt32(txtVendorId.Text),
                              txtPayModeId.Text == "" ? (int?)null : Convert.ToInt32(txtPayModeId.Text),
                              txtAccountId.Text == "" ? (int?)null : Convert.ToInt32(txtAccountId.Text),
                              txtPayableAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtPayableAmount.Text),
                              txtPaidAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtPaidAmount.Text));
                    }
                }
            }
            if (drpService.SelectedValue != "" && txt_displayPrice.Text != "" && txt_Qty.Text != "")
            {
                dt_ser.Rows.Add(Convert.ToInt32(hdn_InvDetailId.Value), hdnSerCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnSerCategoryId.Value),
                    Convert.ToInt32(drpService.SelectedValue), txt_desc.Text, Convert.ToDecimal(hdnPrice.Value), Convert.ToDecimal(hdn_expn.Value),
                       Convert.ToDecimal(hdn_sc.Value), txtServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtServiceCharge.Text),
                       Convert.ToDecimal(txt_Qty.Text), Convert.ToDecimal(txt_taxamt.Text),
                       Convert.ToDecimal(txt_PriceWitTax.Text), txtFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtFine.Text),
                       Convert.ToDecimal(txt_totPrice.Text), txt_discount.Text == "" ? (decimal?)null : Convert.ToDecimal(txt_discount.Text),
                       Convert.ToDecimal(hdn_tax.Value));
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(dt_ser);
            ds.Tables.Add(dt_exp);
            ds.Tables.Add(dt_trans);

            return ds;
        }


        public DataTable fill_Detail_Receipt()
        {
            DataTable dt_ser = new DataTable();
            dt_ser.Columns.Add("D_id", typeof(int));
            dt_ser.Columns.Add("CategoryId", typeof(int));
            dt_ser.Columns.Add("Service_Id", typeof(int));
            dt_ser.Columns.Add("Price", typeof(decimal));
            dt_ser.Columns.Add("Expense", typeof(decimal));
            dt_ser.Columns.Add("ServiceCharge", typeof(decimal));
            dt_ser.Columns.Add("Discount", typeof(decimal));
            dt_ser.Columns.Add("Quantity", typeof(decimal));
            dt_ser.Columns.Add("TaxAmount", typeof(decimal));
            dt_ser.Columns.Add("PriceWitTax", typeof(decimal));
            dt_ser.Columns.Add("Total", typeof(decimal));

            if (rpt_Item_list.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_Item_list.Items)
                {
                    HiddenField hdnInvDId = (HiddenField)itm.FindControl("hdnInvDId");
                    HiddenField hdn_catgory_id = (HiddenField)itm.FindControl("hdnInvDCategoryId");
                    HiddenField hdn_service_id = (HiddenField)itm.FindControl("hdnInvDServiceId");
                    HiddenField hdnInvDPrice = (HiddenField)itm.FindControl("hdnInvDPrice");

                    HiddenField hdnInvDExpense = (HiddenField)itm.FindControl("hdnInvDExpense");
                    HiddenField hdnInvDServiceCharge = (HiddenField)itm.FindControl("hdnInvDServiceCharge");
                    TextBox txtInvDQty = (TextBox)itm.FindControl("txtInvDQty");
                    TextBox txtInvDTaxAmount = (TextBox)itm.FindControl("txtInvDTaxAmount");
                    TextBox txtInvDPriceWitTax = (TextBox)itm.FindControl("txtInvDPriceWitTax");
                    TextBox txtInvDTotal = (TextBox)itm.FindControl("txtInvDTotal");
                    TextBox txtInvDdiscount = (TextBox)itm.FindControl("txtInvDdiscount");

                    dt_ser.Rows.Add(Convert.ToInt32(hdnInvDId.Value), hdn_catgory_id.Value == "" ? (int?)null : Convert.ToInt32(hdn_catgory_id.Value), Convert.ToInt32(hdn_service_id.Value),
                        Convert.ToDecimal(hdnInvDPrice.Value), Convert.ToDecimal(hdnInvDExpense.Value),
                            Convert.ToDecimal(hdnInvDServiceCharge.Value), txtInvDdiscount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDdiscount.Text),
                            Convert.ToDecimal(txtInvDQty.Text), Convert.ToDecimal(txtInvDTaxAmount.Text),
                            Convert.ToDecimal(txtInvDPriceWitTax.Text), Convert.ToDecimal(txtInvDTotal.Text));
                }
            }
            return dt_ser;
        }

        /*Save*/
        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            if (CheckCreditAmount())
            {
                int res = SaveInvoice();

                if (res > 0)
                {
                    grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                    Clear();
                    lbl_msgin.Text = "Saved Successfully !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                }
                else
                {
                    lbl_msgin.Text = "Sorry Failed to Process Your Request !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                }
            }

            Upd_Add_PanelInner.Update();
        }

        /*SalesPrint*/
        protected void btn_Salesprint_OnClick(object sender, EventArgs e)
        {
            int Format = obj_trans.GetInvoiceFormat();
            string url = "";
            if (Format == 1)
                url = "../Reports/SalesOrderFormat1.aspx?id=";
            else if (Format == 2)
                url = "../Reports/SalesOrderFormat2.aspx?id=";

            if (CheckCreditAmount())
            {
                int res = SaveInvoice();
                if (res > 0)
                {
                    grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                    Clear();
                    lbl_msgin.Text = "Saved Successfully !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                    url = url + res;
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
                }
                else
                {
                    lbl_msgin.Text = "Sorry Failed to Process Your Request !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                }
            }
            Upd_Add_PanelInner.Update();
        }

        protected void btn_TaxInvoicePrint_OnClick(object sender, EventArgs e)
        {
            int Format = obj_trans.GetInvoiceFormat();
            string url = "";
            if (Format == 1)
                url = "../Reports/TaxInvoiceFormat1.aspx?id=";
            else if (Format == 2)
                url = "../Reports/TaxInvoiceFormat2.aspx?id=";
            if (hdn_IsCredit.Value == "0")
            {
                url = url + Convert.ToInt32(hdn_id.Value);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
            }
            else
            {
                if (CheckCreditAmount())
                {
                    int res = SaveInvoice();
                    if (res > 0)
                    {
                        grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                        Clear();
                        lbl_msgin.Text = "Saved Successfully !..";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                        url = url + res;
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
                    }
                    else
                    {
                        lbl_msgin.Text = "Sorry Failed to Process Your Request !..";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                    }
                }
                Upd_Add_PanelInner.Update();
            }
        }

        protected void btn_ReceiptPrint_OnClick(object sender, EventArgs e)
        {
            string url = "../Reports/CashReceiptFormat1.aspx?id=";

            if (CheckCreditAmount())
            {
                int res = SaveReceipt();
                if (res > 0)
                {
                    grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                    Clear();
                    lbl_msgin.Text = "Saved Successfully !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                    url = url + res;
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
                }
                else
                {
                    lbl_msgin.Text = "Sorry Failed to Process Your Request !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                }
            }

            Upd_Add_PanelInner.Update();
        }

        #region cancel

        protected void btn_Cancelmain_OnClick(object sender, EventArgs e)
        {
            pnl_cancl.Visible = true;
            txt_cancelremark.Text = "";
            upd_cancl.Update();
        }

        protected void btn_cnclse_OnClick(object sender, EventArgs e)
        {
            pnl_cancl.Visible = false;
            txt_cancelremark.Text = "";
            upd_cancl.Update();
        }

        protected void btn_cancel_OnClick(object sender, EventArgs e)
        {
            int res = obj_trans.Cancel_InvoiceReceipt(Convert.ToInt32(hdn_id.Value), txt_cancelremark.Text, Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                Clear();
                lbl_msg.Text = "Cancelled !..";

            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
            }
            pnl_cancl.Visible = false;
            txt_cancelremark.Text = "";
            upd_cancl.Update();
            Upd_Add_Panel.Update();
        }

        #endregion

        public void fill_Quotation(int CusId)
        {
            drp_quot.Items.Clear();
            DataTable dt = obj_trans.Drp_Quotation(CusId);
            drp_quot.DataSource = dt;
            drp_quot.DataTextField = "Text";
            drp_quot.DataValueField = "Value";
            drp_quot.DataBind();
            drp_quot.Text = "";

            UpdQuotationPanel.Update();

            drp_quo_OnSelectedIndexChanged(null, null);
        }

        //Drop Down Quotation
        public void fill_Edit_Quotation(int CusId, int inv_id)
        {
            drp_quot.Items.Clear();
            DataTable dt = obj_trans.Drp_Quotation_Edit(CusId, inv_id);
            drp_quot.DataSource = dt;
            drp_quot.DataTextField = "Text";
            drp_quot.DataValueField = "Value";
            drp_quot.DataBind();
            drp_quot.Text = "";
        }

        
        protected void drp_quo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            int InvoiceType = rbTaxInvoice.Checked ? 1 : 2;
            DataTable dt = obj_trans.GetQuotationDetails_invrecpt(drp_quot.SelectedValue == "" ? 0 : Convert.ToInt32(drp_quot.SelectedValue), Convert.ToInt32(hdnLanguage.Value), Convert.ToInt32(hdnSerPriceWTax.Value), Convert.ToInt32(hdnTaxAppliedWithDiscount.Value), InvoiceType,0);
            rpt_Item_list.DataSource = dt;
            rpt_Item_list.DataBind();
            InlineCalculation();
            lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
            hdn_InvDetailId.Value = "-" + (rpt_Item_list.Items.Count + 1).ToString();
            Upd_Item_Panel.Update();
        }

        /*Customer */
        public void fill_Customer()
        {
            drp_customer.Items.Clear();
            DataTable dt = obj_trans.Drp_Customer();
            drp_customer.DataSource = dt;
            drp_customer.DataTextField = "Text";
            drp_customer.DataValueField = "Value";
            drp_customer.DataBind();

            RadComboBoxItem CodeItem = new RadComboBoxItem();
            CodeItem.Text = "New Entry";
            CodeItem.Value = "0";
            drp_customer.Items.Insert(0, CodeItem);
        }

        public void fill_Templates()
        {
            drpTemplates.Items.Clear();
            DataTable dt = obj_trans.GetTemplates();
            drpTemplates.DataSource = dt;
            drpTemplates.DataTextField = "Text";
            drpTemplates.DataValueField = "Value";
            drpTemplates.DataBind();
        }

        protected void drpTemplatesOnSelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtTemplates = new DataTable();
            dtTemplates.Columns.Add("TemplatesId", typeof(int));
            foreach (RadComboBoxItem item in drpTemplates.Items)
            {
                if (item.Checked)
                    dtTemplates.Rows.Add(Convert.ToInt32(item.Value));
            }
            int InvoiceType = rbTaxInvoice.Checked ? 1 : 2;
            DataTable dt = obj_trans.GetServiceDetailsTemplate_invrecpt(dtTemplates, Convert.ToInt32(hdnLanguage.Value),
                Convert.ToInt32(hdnSerPriceWTax.Value), drp_customer.SelectedValue == "" ? 0 : Convert.ToInt32(drp_customer.SelectedValue), Convert.ToInt32(hdnTaxAppliedWithDiscount.Value), InvoiceType,
                0);
            rpt_Item_list.DataSource = dt;
            rpt_Item_list.DataBind();
            InlineCalculation();
            lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
            hdn_InvDetailId.Value = "-" + (rpt_Item_list.Items.Count + 1).ToString();
            Upd_Item_Panel.Update();
        }

        public void fill_FilterDropDown(int filterby)
        {
            int Department = drpDepartment.SelectedValue == "" ? 0 : Convert.ToInt32(drpDepartment.SelectedValue);
            int SerCategory = drpSerCategory.SelectedValue == "" ? 0 : Convert.ToInt32(drpSerCategory.SelectedValue);
            int SerSubCategory = drpSerSubCategory.SelectedValue == "" ? 0 : Convert.ToInt32(drpSerSubCategory.SelectedValue);
            DataSet ds = obj_trans.GetServiceFilter(filterby, Department, SerCategory, SerSubCategory, Convert.ToInt32(hdnLanguage.Value));
            DataTable dtDepartment = ds.Tables[0];
            DataTable dtSerCategory = ds.Tables[1];
            DataTable dtSerSubCategory = ds.Tables[2];
            DataTable dtService = ds.Tables[3];

            if (drpDepartment.SelectedValue == "")
            {
                drpDepartment.ClearSelection();
                drpDepartment.Text = "";
                drpDepartment.Items.Clear();
                drpDepartment.DataSource = dtDepartment;
                drpDepartment.DataTextField = "Text";
                drpDepartment.DataValueField = "Value";
                drpDepartment.DataBind();
                UpdDepartmentDropdown.Update();
            }
            if (drpSerCategory.SelectedValue == "")
            {
                drpSerCategory.ClearSelection();
                drpSerCategory.Text = "";
                drpSerCategory.Items.Clear();
                drpSerCategory.DataSource = dtSerCategory;
                drpSerCategory.DataTextField = "Text";
                drpSerCategory.DataValueField = "Value";
                drpSerCategory.DataBind();
                UpdSerCategoryDropdown.Update();
            }
            if (drpSerSubCategory.SelectedValue == "")
            {
                drpSerSubCategory.ClearSelection();
                drpSerSubCategory.Text = "";
                drpSerSubCategory.Items.Clear();
                drpSerSubCategory.DataSource = dtSerSubCategory;
                drpSerSubCategory.DataTextField = "Text";
                drpSerSubCategory.DataValueField = "Value";
                drpSerSubCategory.DataBind();
                UpdSerSubCategoryDropdown.Update();
            }
            drpService.ClearSelection();
            drpService.Text = "";
            drpService.Items.Clear();
            drpService.DataSource = dtService;
            drpService.DataTextField = "Text";
            drpService.DataValueField = "Value";
            drpService.DataBind();
            UpdServiceDropdown.Update();
            //drpService_OnSelectedIndexChanged(null, null);


            hdnDepartment.Value = "";
            hdnDepartmentId.Value = "";
            hdnSerCategory.Value = "";
            hdnSerCategoryId.Value = "";
            hdnSerSubCategory.Value = "";
            hdnSerSubCategoryId.Value = "";
            txt_desc.Text = "";
            txt_displayPrice.Text = "";
            hdnPrice.Value = "";
            hdn_expn.Value = hdn_sc.Value = "0";
            txtServiceCharge.Text = "";
            txt_Qty.Text = "";
            txt_taxamt.Text = "";
            hdn_tax.Value = "0";
            txt_PriceWitTax.Text = "";
            hdnFineApplicable.Value = "0";
            txtFine.Text = "";
            txt_totPrice.Text = "";
            txt_discount.Text = "";

            UpdTxtDescription.Update();
            UpdTxtPrice.Update();
            UpdTxtServiceCharge.Update();
            UpdTxtQty.Update();
            UpdTxtTaxAmt.Update();
            UpdTxtPriceWithTax.Update();
            UpdTxtFine.Update();
            UpdTxtTotPrice.Update();
            InlineCalculation();

        }

        protected void drpFilter_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            String contrlName = sendercontrol.ID;
            if (contrlName == "drpDepartment")
            {
                drpSerCategory.Text = "";

                drpSerSubCategory.Text = "";
                drpService.Text = "";

                drpSerCategory.ClearSelection();
                drpSerSubCategory.ClearSelection();
                drpService.ClearSelection();
            }
            else
                if (contrlName == "drpSerCategory")
            {

                drpSerSubCategory.Text = "";
                drpService.Text = "";
                drpSerSubCategory.ClearSelection();
                drpService.ClearSelection();
            }
            else
                    if (contrlName == "drpSerSubCategory")
            {


                drpService.Text = "";

                drpService.ClearSelection();
            }
            fill_FilterDropDown(1);
        }

        /*drp_item OnSelectedIndexChanged*/
        protected void drpService_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hdnDepartment.Value = "";
            hdnDepartmentId.Value = "";
            hdnSerCategory.Value = "";
            hdnSerCategoryId.Value = "";
            hdnSerSubCategory.Value = "";
            hdnSerSubCategoryId.Value = "";
            txt_desc.Text = "";
            txt_displayPrice.Text = "";
            hdnPrice.Value = "";
            hdn_expn.Value = hdn_sc.Value = "0";
            txtServiceCharge.Text = "";
            txt_Qty.Text = "";
            txt_taxamt.Text = "";
            hdn_tax.Value = "0";
            txt_PriceWitTax.Text = "";
            hdnFineApplicable.Value = "0";
            txtFine.Text = "";
            txt_totPrice.Text = "";

            if (drpService.SelectedValue != "")
            {
                int InvoiceType = rbTaxInvoice.Checked ? 1 : 2;
                DataTable Amount = new DataTable();
                
                Amount = obj_trans.Get_Services_Amount_invrecpt(Convert.ToInt32(drpService.SelectedValue), 1, Convert.ToInt32(hdnLanguage.Value),
                    Convert.ToInt32(hdnSerPriceWTax.Value), drp_customer.SelectedValue == "" ? 0 : Convert.ToInt32(drp_customer.SelectedValue), Convert.ToInt32(hdnTaxAppliedWithDiscount.Value), InvoiceType,
                   0);

                if (Amount.Rows.Count > 0)
                {
                    txt_displayPrice.Text = Amount.Rows[0]["DisplayPrice"].ToString();
                    hdnPrice.Value = Amount.Rows[0]["Price"].ToString();
                    txt_Qty.Text = "1";
                    txt_taxamt.Text = Amount.Rows[0]["TaxAmount"].ToString();
                    txt_PriceWitTax.Text = Amount.Rows[0]["PriceWitTax"].ToString();
                    txt_totPrice.Text = Amount.Rows[0]["Total"].ToString();
                    hdnFineApplicable.Value = Amount.Rows[0]["FineApplicable"].ToString();
                    txt_discount.Text = Amount.Rows[0]["Discount"].ToString();

                    hdn_expn.Value = Amount.Rows[0]["Expense"].ToString();
                    hdn_tax.Value = Amount.Rows[0]["Tax"].ToString();
                    hdn_sc.Value = Amount.Rows[0]["ServiceCharge"].ToString();
                    hdnDepartment.Value = Amount.Rows[0]["DepartmentName"].ToString();
                    hdnDepartmentId.Value = Amount.Rows[0]["DepartmentId"].ToString();
                    hdnSerCategory.Value = Amount.Rows[0]["SerCategoryName"].ToString();
                    hdnSerCategoryId.Value = Amount.Rows[0]["ServiceCategoryId"].ToString();
                    hdnSerSubCategory.Value = Amount.Rows[0]["SerSubCategoryName"].ToString();
                    hdnSerSubCategoryId.Value = Amount.Rows[0]["ServiceSubCategoryId"].ToString();
                    drpDepartment.SelectedValue = Amount.Rows[0]["DepartmentId"].ToString();
                    drpSerCategory.SelectedValue = Amount.Rows[0]["ServiceCategoryId"].ToString();
                    drpSerSubCategory.SelectedValue = Amount.Rows[0]["ServiceSubCategoryId"].ToString();
                }
            }
            UpdDepartmentDropdown.Update();
            UpdSerCategoryDropdown.Update();
            UpdSerSubCategoryDropdown.Update();
            UpdTxtDescription.Update();
            UpdTxtPrice.Update();
            UpdTxtServiceCharge.Update();
            UpdTxtQty.Update();
            UpdTxtTaxAmt.Update();
            UpdTxtPriceWithTax.Update();
            UpdTxtFine.Update();
            UpdTxtTotPrice.Update();
            Updtxt_discount.Update();
            InlineCalculation();

        }

        /*Inline Calculation*/
        public void InlineCalculation()
        {
            decimal Total_Amt = 0, TotDiscount = 0, totQty = 0;

            decimal tot = 0, totdis = 0, qty = 0;

            foreach (RepeaterItem itm in rpt_Item_list.Items)
            {
                TextBox txtInvDTotal = (TextBox)itm.FindControl("txtInvDTotal");
                TextBox txtInvDdiscount = (TextBox)itm.FindControl("txtInvDdiscount");
                TextBox txtInvDQty = (TextBox)itm.FindControl("txtInvDQty");

                tot = txtInvDTotal.Text == "" ? 0 : Convert.ToDecimal(txtInvDTotal.Text);
                totdis = txtInvDdiscount.Text == "" ? 0 : Convert.ToDecimal(txtInvDdiscount.Text);
                totQty = txtInvDQty.Text == "" ? 0 : Convert.ToDecimal(txtInvDQty.Text);

                Total_Amt += tot;
                TotDiscount = TotDiscount + Convert.ToDecimal(totQty * totdis);
            }
            if (drpService.SelectedValue != "" && txt_displayPrice.Text != "" && txt_Qty.Text != "")
            {
                qty = Convert.ToDecimal(txt_Qty.Text);
                Total_Amt += txt_totPrice.Text == "" ? 0 : Convert.ToDecimal(txt_totPrice.Text);
                TotDiscount += (qty * (txt_discount.Text == "" ? 0 : Convert.ToDecimal(txt_discount.Text)));

            }
            string[] substr = Total_Amt.ToString().Split('.');
            decimal AmtAfterDecimal = Total_Amt - Convert.ToDecimal(substr[0]);
            decimal AmtBeforeDecimal = Total_Amt - AmtAfterDecimal;
            decimal AmtDecimal = 0;
            decimal Final = 0;
            if (AmtAfterDecimal <= 0.12M)
            {
                AmtDecimal = 0;
            }
            else if ((AmtAfterDecimal >= 0.13M) && (AmtAfterDecimal <= 0.37M))
            {
                AmtDecimal = 0.25M;
            }
            else if ((AmtAfterDecimal >= 0.38M) && (AmtAfterDecimal <= 0.62M))
            {
                AmtDecimal = 0.50M;
            }
            else if ((AmtAfterDecimal >= 0.63M) && (AmtAfterDecimal <= 0.87M))
            {
                AmtDecimal = 0.75M;
            }
            else if (AmtAfterDecimal >= 0.88M)
            {
                AmtDecimal = 1;
            }
            Final = AmtBeforeDecimal + AmtDecimal;

          txt_ReceivedAmt.Text=  txt_amtPayNow.Text = txt_pendingAmt.Text = txt_grand.Text = (Convert.ToDecimal(Final)).ToString("0.00");
            txt_totDiscount.Text = (Convert.ToDecimal(TotDiscount)).ToString("0.00");
            //txt_ReceivedAmt.Text = "";
            try
            {
                txt_Balance.Text =( Convert.ToDecimal(txt_ReceivedAmt.Text) - Convert.ToDecimal(txt_amtPayNow.Text)).ToString();
            }
            catch
            {
                txt_Balance.Text = "";
            }
            Updtxt_totDiscount.Update();
            Updtxt_grand.Update();
            Updtxt_pendingAmt.Update();
            Updtxt_ReceivedAmt.Update();
            Updtxt_amtPayNow.Update();
            Updtxt_Balance.Update();
            CalCommission();
        }

        protected void rptitemlistDatabound(object sender, RepeaterItemEventArgs e)
        {
            Button btnCompleSC = (Button)e.Item.FindControl("btnCompleSC");
            if (hdnInvoiceStatus.Value == "2" || hdnInvoiceStatus.Value == "3")
                btnCompleSC.Visible = false;
            else
                btnCompleSC.Visible = hdnSCInInvoice.Value == "1" ? true : false;
        }

        /*New Item*/
        protected void btn_new_line_OnClick(object sender, EventArgs e)
        {
            DataTable dt_ser = new DataTable();
            dt_ser.Columns.Add("D_id", typeof(int));
            dt_ser.Columns.Add("DepartmentId", typeof(int));
            dt_ser.Columns.Add("CategoryId", typeof(int));
            dt_ser.Columns.Add("ServiceSubCategoryId", typeof(int));
            dt_ser.Columns.Add("Service_Id", typeof(int));
            dt_ser.Columns.Add("DepartmentName", typeof(string));
            dt_ser.Columns.Add("SerCategoryName", typeof(string));
            dt_ser.Columns.Add("SerSubCategoryName", typeof(string));
            dt_ser.Columns.Add("ServiceFullName", typeof(string));
            dt_ser.Columns.Add("Particulars", typeof(string));
            dt_ser.Columns.Add("DisplayPrice", typeof(decimal));
            dt_ser.Columns.Add("Price", typeof(decimal));
            dt_ser.Columns.Add("Expense", typeof(decimal));
            dt_ser.Columns.Add("ServiceCharge", typeof(decimal));
            dt_ser.Columns.Add("AdditionalServiceCharge", typeof(decimal));
            dt_ser.Columns.Add("Quantity", typeof(decimal));
            dt_ser.Columns.Add("TaxAmount", typeof(decimal));
            dt_ser.Columns.Add("Tax", typeof(decimal));
            dt_ser.Columns.Add("PriceWitTax", typeof(decimal));
            dt_ser.Columns.Add("FineApplicable", typeof(int));
            dt_ser.Columns.Add("Fine", typeof(decimal));
            dt_ser.Columns.Add("Total", typeof(decimal));
            dt_ser.Columns.Add("Discount", typeof(decimal));

            dt_ser.Columns.Add("SerComDate", typeof(DateTime));
            dt_ser.Columns.Add("ExpQty", typeof(int));
            dt_ser.Columns.Add("ExpSinglAmt", typeof(decimal));
            dt_ser.Columns.Add("ExpTotAmt", typeof(decimal));

            DataTable dt_exp = new DataTable();
            dt_exp.Columns.Add("InvDId", typeof(int));
            dt_exp.Columns.Add("SerComDetailId", typeof(int));
            dt_exp.Columns.Add("ExpenseId", typeof(int));
            dt_exp.Columns.Add("Amount", typeof(decimal));
            dt_exp.Columns.Add("VAT", typeof(decimal));
            dt_exp.Columns.Add("VendorId", typeof(int));
            dt_exp.Columns.Add("PayModeId", typeof(int));
            dt_exp.Columns.Add("AccountId", typeof(int));
            dt_exp.Columns.Add("PayableAmount", typeof(decimal));
            dt_exp.Columns.Add("PaidAmount", typeof(decimal));

            DataTable dt_trans = new DataTable();
            dt_trans.Columns.Add("InvDId", typeof(int));
            dt_trans.Columns.Add("TransactionNumber", typeof(string));

            if (rpt_Item_list.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_Item_list.Items)
                {
                    HiddenField hdnInvDId = (HiddenField)itm.FindControl("hdnInvDId");
                    HiddenField hdnInvDDepartmentId = (HiddenField)itm.FindControl("hdnInvDDepartmentId");
                    HiddenField hdnInvDCategoryId = (HiddenField)itm.FindControl("hdnInvDCategoryId");
                    HiddenField hdnInvDSerSubCategoryId = (HiddenField)itm.FindControl("hdnInvDSerSubCategoryId");
                    HiddenField hdnInvDServiceId = (HiddenField)itm.FindControl("hdnInvDServiceId");
                    HiddenField hdnInvDDepartment = (HiddenField)itm.FindControl("hdnInvDDepartment");
                    HiddenField hdnInvDSerCategory = (HiddenField)itm.FindControl("hdnInvDSerCategory");
                    HiddenField hdnInvDSerSubCategory = (HiddenField)itm.FindControl("hdnInvDSerSubCategory");
                    Label lblServiceFullName = (Label)itm.FindControl("lblServiceFullName");
                    Label lblInvDdesc = (Label)itm.FindControl("lblInvDdesc");
                    TextBox txtInvDDisplayPrice = (TextBox)itm.FindControl("txtInvDDisplayPrice");
                    HiddenField hdnInvDPrice = (HiddenField)itm.FindControl("hdnInvDPrice");
                    HiddenField hdnInvDExpense = (HiddenField)itm.FindControl("hdnInvDExpense");
                    HiddenField hdnInvDServiceCharge = (HiddenField)itm.FindControl("hdnInvDServiceCharge");
                    TextBox txtInvDAddServiceCharge = (TextBox)itm.FindControl("txtInvDAddServiceCharge");
                    TextBox txtInvDQty = (TextBox)itm.FindControl("txtInvDQty");
                    TextBox txtInvDTaxAmount = (TextBox)itm.FindControl("txtInvDTaxAmount");
                    HiddenField hdnInvDTax = (HiddenField)itm.FindControl("hdnInvDTax");
                    TextBox txtInvDPriceWitTax = (TextBox)itm.FindControl("txtInvDPriceWitTax");
                    HiddenField hdnInvDFineApplicable = (HiddenField)itm.FindControl("hdnInvDFineApplicable");
                    TextBox txtInvDFine = (TextBox)itm.FindControl("txtInvDFine");
                    TextBox txtInvDTotal = (TextBox)itm.FindControl("txtInvDTotal");
                    TextBox txtInvDdiscount = (TextBox)itm.FindControl("txtInvDdiscount");

                    //sc
                    TextBox txtExpenseQty = (TextBox)itm.FindControl("txtExpenseQty");
                    TextBox txtExpenseSinglAmt = (TextBox)itm.FindControl("txtExpenseSinglAmt");
                    TextBox txtExpenseTotalAmt = (TextBox)itm.FindControl("txtExpenseTotalAmt");
                    RadDatePicker ExpenseSerComDate = (RadDatePicker)itm.FindControl("ExpenseSerComDate");

                    Repeater rptTransCode = (Repeater)itm.FindControl("rptTransCode");
                    Repeater rptexpensein = (Repeater)itm.FindControl("rptexpensein");

                    dt_ser.Rows.Add(Convert.ToInt32(hdnInvDId.Value), hdnInvDDepartmentId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDDepartmentId.Value),
                     hdnInvDCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDCategoryId.Value), hdnInvDSerSubCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDSerSubCategoryId.Value),
                    Convert.ToInt32(hdnInvDServiceId.Value), hdnInvDDepartment.Value, hdnInvDSerCategory.Value, hdnInvDSerSubCategory.Value,
                    lblServiceFullName.Text, lblInvDdesc.Text, Convert.ToDecimal(txtInvDDisplayPrice.Text), Convert.ToDecimal(hdnInvDPrice.Value), Convert.ToDecimal(hdnInvDExpense.Value),
                    Convert.ToDecimal(hdnInvDServiceCharge.Value), txtInvDAddServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDAddServiceCharge.Text),
                    Convert.ToDecimal(txtInvDQty.Text), Convert.ToDecimal(txtInvDTaxAmount.Text), Convert.ToDecimal(hdnInvDTax.Value),
                    Convert.ToDecimal(txtInvDPriceWitTax.Text), Convert.ToInt32(hdnInvDFineApplicable.Value), txtInvDFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDFine.Text), Convert.ToDecimal(txtInvDTotal.Text),
                    txtInvDdiscount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDdiscount.Text),

                     ExpenseSerComDate.SelectedDate == null ? (DateTime?)null : DateTime.ParseExact(CalDate(ExpenseSerComDate), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                   txtExpenseQty.Text == "" ? (decimal?)null : Convert.ToDecimal(txtExpenseQty.Text),
                   txtExpenseSinglAmt.Text == "" ? (decimal?)null : Convert.ToDecimal(txtExpenseSinglAmt.Text),
                   txtExpenseTotalAmt.Text == "" ? (decimal?)null : Convert.ToDecimal(txtExpenseTotalAmt.Text));

                    foreach (RepeaterItem itms in rptTransCode.Items)
                    {
                        TextBox txtTransCode = (TextBox)itms.FindControl("txtTransCode");
                        dt_trans.Rows.Add(Convert.ToInt32(hdnInvDId.Value), txtTransCode.Text);
                    }

                    foreach (RepeaterItem itms in rptexpensein.Items)
                    {
                        TextBox txtSerComDetailId = (TextBox)itms.FindControl("txtSerComDetailId");
                        TextBox txtExpenseId = (TextBox)itms.FindControl("txtExpenseId");
                        TextBox txtAmount = (TextBox)itms.FindControl("txtAmount");
                        TextBox txtVAT = (TextBox)itms.FindControl("txtVAT");
                        TextBox txtVendorId = (TextBox)itms.FindControl("txtVendorId");
                        TextBox txtPayModeId = (TextBox)itms.FindControl("txtPayModeId");
                        TextBox txtAccountId = (TextBox)itms.FindControl("txtAccountId");
                        TextBox txtPayableAmount = (TextBox)itms.FindControl("txtPayableAmount");
                        TextBox txtPaidAmount = (TextBox)itms.FindControl("txtPaidAmount");

                        dt_exp.Rows.Add(Convert.ToInt32(hdnInvDId.Value),
                             txtSerComDetailId.Text == "" ? (int?)null : Convert.ToInt32(txtSerComDetailId.Text),
                             txtExpenseId.Text == "" ? (int?)null : Convert.ToInt32(txtExpenseId.Text),
                              txtAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtAmount.Text),
                              txtVAT.Text == "" ? (decimal?)null : Convert.ToDecimal(txtVAT.Text),
                              txtVendorId.Text == "" ? (int?)null : Convert.ToInt32(txtVendorId.Text),
                              txtPayModeId.Text == "" ? (int?)null : Convert.ToInt32(txtPayModeId.Text),
                              txtAccountId.Text == "" ? (int?)null : Convert.ToInt32(txtAccountId.Text),
                              txtPayableAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtPayableAmount.Text),
                              txtPaidAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtPaidAmount.Text));
                    }

                }
            }

            if (drpService.SelectedValue != "" && txt_displayPrice.Text != "" && txt_Qty.Text != "")
            {
                dt_ser.Rows.Add(Convert.ToInt32(hdn_InvDetailId.Value), hdnDepartmentId.Value == "" ? (int?)null : Convert.ToInt32(hdnDepartmentId.Value),
                    hdnSerCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnSerCategoryId.Value), hdnSerSubCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnSerSubCategoryId.Value), Convert.ToInt32(drpService.SelectedValue),
                    hdnDepartment.Value, hdnSerCategory.Value, hdnSerSubCategory.Value,
                    (hdnDepartment.Value + '/' + hdnSerCategory.Value + '/' + hdnSerSubCategory.Value + '/' + drpService.Text), txt_desc.Text, Convert.ToDecimal(txt_displayPrice.Text), Convert.ToDecimal(hdnPrice.Value), Convert.ToDecimal(hdn_expn.Value),
                       Convert.ToDecimal(hdn_sc.Value), txtServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtServiceCharge.Text), Convert.ToDecimal(txt_Qty.Text), Convert.ToDecimal(txt_taxamt.Text), Convert.ToDecimal(hdn_tax.Value),
                       Convert.ToDecimal(txt_PriceWitTax.Text), Convert.ToInt32(hdnFineApplicable.Value), txtFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtFine.Text), Convert.ToDecimal(txt_totPrice.Text),
                        txt_discount.Text == "" ? (decimal?)null : Convert.ToDecimal(txt_discount.Text));
            }
            rpt_Item_list.DataSource = dt_ser;
            rpt_Item_list.DataBind();

            //sc
            foreach (RepeaterItem itm in rpt_Item_list.Items)
            {
                Repeater rptTransCode = (Repeater)itm.FindControl("rptTransCode");
                Repeater rptexpensein = (Repeater)itm.FindControl("rptexpensein");
                HiddenField hdnInvDId = (HiddenField)itm.FindControl("hdnInvDId");

                DataTable dt_trans_in = new DataTable();
                dt_trans_in.Columns.Add("TransactionNumber", typeof(string));

                foreach (DataRow r in dt_trans.Rows)
                {
                    if (r["InvDId"].ToString() == hdnInvDId.Value)
                    {
                        dt_trans_in.Rows.Add(r["TransactionNumber"].ToString());
                    }
                }

                rptTransCode.DataSource = dt_trans_in;
                rptTransCode.DataBind();

                DataTable dt_expin = new DataTable();
                dt_expin.Columns.Add("InvDId", typeof(int));
                dt_expin.Columns.Add("SerComDetailId", typeof(int));
                dt_expin.Columns.Add("ExpenseId", typeof(int));
                dt_expin.Columns.Add("Amount", typeof(decimal));
                dt_expin.Columns.Add("VAT", typeof(decimal));
                dt_expin.Columns.Add("VendorId", typeof(int));
                dt_expin.Columns.Add("PayModeId", typeof(int));
                dt_expin.Columns.Add("AccountId", typeof(int));
                dt_expin.Columns.Add("PayableAmount", typeof(decimal));
                dt_expin.Columns.Add("PaidAmount", typeof(decimal));

                foreach (DataRow r in dt_exp.Rows)
                {
                    if (r["InvDId"].ToString() == hdnInvDId.Value)
                    {
                        dt_expin.Rows.Add(Convert.ToInt32(r["InvDId"]), Convert.ToInt32(r["SerComDetailId"]), Convert.ToInt32(r["ExpenseId"]),
                            Convert.ToDecimal(r["Amount"]), Convert.ToDecimal(r["VAT"]), Convert.ToInt32(r["VendorId"]), Convert.ToInt32(r["PayModeId"])
                            , Convert.ToInt32(r["AccountId"]), Convert.ToDecimal(r["PayableAmount"]), Convert.ToDecimal(r["PaidAmount"]));
                    }
                }

                rptexpensein.DataSource = dt_expin;
                rptexpensein.DataBind();
            }

            ClearServiceDetail();
            drpDepartment.Focus();
            //Upd_InvoiceDetail_Panel.Update();
            Upd_Item_Panel.Update();
        }

        /*Remove Item*/
        protected void btn_remove_line_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            DataTable dt_ser = new DataTable();
            dt_ser.Columns.Add("D_id", typeof(int));
            dt_ser.Columns.Add("DepartmentId", typeof(int));
            dt_ser.Columns.Add("CategoryId", typeof(int));
            dt_ser.Columns.Add("ServiceSubCategoryId", typeof(int));
            dt_ser.Columns.Add("Service_Id", typeof(int));
            dt_ser.Columns.Add("DepartmentName", typeof(string));
            dt_ser.Columns.Add("SerCategoryName", typeof(string));
            dt_ser.Columns.Add("SerSubCategoryName", typeof(string));
            dt_ser.Columns.Add("ServiceFullName", typeof(string));
            dt_ser.Columns.Add("Particulars", typeof(string));
            dt_ser.Columns.Add("DisplayPrice", typeof(decimal));
            dt_ser.Columns.Add("Price", typeof(decimal));
            dt_ser.Columns.Add("Expense", typeof(decimal));
            dt_ser.Columns.Add("ServiceCharge", typeof(decimal));
            dt_ser.Columns.Add("AdditionalServiceCharge", typeof(decimal));
            dt_ser.Columns.Add("Quantity", typeof(decimal));
            dt_ser.Columns.Add("TaxAmount", typeof(decimal));
            dt_ser.Columns.Add("Tax", typeof(decimal));
            dt_ser.Columns.Add("PriceWitTax", typeof(decimal));
            dt_ser.Columns.Add("FineApplicable", typeof(int));
            dt_ser.Columns.Add("Fine", typeof(decimal));
            dt_ser.Columns.Add("Total", typeof(decimal));
            dt_ser.Columns.Add("Discount", typeof(decimal));

            //sc
            dt_ser.Columns.Add("ExpQty", typeof(int));
            dt_ser.Columns.Add("ExpSinglAmt", typeof(decimal));
            dt_ser.Columns.Add("ExpTotAmt", typeof(decimal));
            dt_ser.Columns.Add("SerComDate", typeof(DateTime));

            DataTable dt_exp = new DataTable();
            dt_exp.Columns.Add("InvDId", typeof(int));
            dt_exp.Columns.Add("SerComDetailId", typeof(int));
            dt_exp.Columns.Add("ExpenseId", typeof(int));
            dt_exp.Columns.Add("Amount", typeof(decimal));
            dt_exp.Columns.Add("VAT", typeof(decimal));
            dt_exp.Columns.Add("VendorId", typeof(int));
            dt_exp.Columns.Add("PayModeId", typeof(int));
            dt_exp.Columns.Add("AccountId", typeof(int));
            dt_exp.Columns.Add("PayableAmount", typeof(decimal));
            dt_exp.Columns.Add("PaidAmount", typeof(decimal));

            DataTable dt_trans = new DataTable();
            dt_trans.Columns.Add("InvDId", typeof(int));
            dt_trans.Columns.Add("TransactionNumber", typeof(string));

            if (rpt_Item_list.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_Item_list.Items)
                {
                    HiddenField hdnInvDId = (HiddenField)itm.FindControl("hdnInvDId");
                    HiddenField hdnInvDDepartmentId = (HiddenField)itm.FindControl("hdnInvDDepartmentId");
                    HiddenField hdnInvDCategoryId = (HiddenField)itm.FindControl("hdnInvDCategoryId");
                    HiddenField hdnInvDSerSubCategoryId = (HiddenField)itm.FindControl("hdnInvDSerSubCategoryId");
                    HiddenField hdnInvDServiceId = (HiddenField)itm.FindControl("hdnInvDServiceId");
                    HiddenField hdnInvDDepartment = (HiddenField)itm.FindControl("hdnInvDDepartment");
                    HiddenField hdnInvDSerCategory = (HiddenField)itm.FindControl("hdnInvDSerCategory");
                    HiddenField hdnInvDSerSubCategory = (HiddenField)itm.FindControl("hdnInvDSerSubCategory");
                    Label lblServiceFullName = (Label)itm.FindControl("lblServiceFullName");
                    Label lblInvDdesc = (Label)itm.FindControl("lblInvDdesc");
                    TextBox txtInvDDisplayPrice = (TextBox)itm.FindControl("txtInvDDisplayPrice");
                    HiddenField hdnInvDPrice = (HiddenField)itm.FindControl("hdnInvDPrice");
                    HiddenField hdnInvDExpense = (HiddenField)itm.FindControl("hdnInvDExpense");
                    HiddenField hdnInvDServiceCharge = (HiddenField)itm.FindControl("hdnInvDServiceCharge");
                    TextBox txtInvDAddServiceCharge = (TextBox)itm.FindControl("txtInvDAddServiceCharge");
                    TextBox txtInvDQty = (TextBox)itm.FindControl("txtInvDQty");
                    TextBox txtInvDTaxAmount = (TextBox)itm.FindControl("txtInvDTaxAmount");
                    HiddenField hdnInvDTax = (HiddenField)itm.FindControl("hdnInvDTax");
                    TextBox txtInvDPriceWitTax = (TextBox)itm.FindControl("txtInvDPriceWitTax");
                    HiddenField hdnInvDFineApplicable = (HiddenField)itm.FindControl("hdnInvDFineApplicable");
                    TextBox txtInvDFine = (TextBox)itm.FindControl("txtInvDFine");
                    TextBox txtInvDTotal = (TextBox)itm.FindControl("txtInvDTotal");
                    TextBox txtInvDdiscount = (TextBox)itm.FindControl("txtInvDdiscount");

                    //sc
                    TextBox txtExpenseQty = (TextBox)itm.FindControl("txtExpenseQty");
                    TextBox txtExpenseSinglAmt = (TextBox)itm.FindControl("txtExpenseSinglAmt");
                    TextBox txtExpenseTotalAmt = (TextBox)itm.FindControl("txtExpenseTotalAmt");
                    RadDatePicker ExpenseSerComDate = (RadDatePicker)itm.FindControl("ExpenseSerComDate");

                    Repeater rptTransCode = (Repeater)itm.FindControl("rptTransCode");
                    Repeater rptexpensein = (Repeater)itm.FindControl("rptexpensein");

                    dt_ser.Rows.Add(Convert.ToInt32(hdnInvDId.Value), hdnInvDDepartmentId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDDepartmentId.Value),
                     hdnInvDCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDCategoryId.Value), hdnInvDSerSubCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDSerSubCategoryId.Value),
                    Convert.ToInt32(hdnInvDServiceId.Value), hdnInvDDepartment.Value, hdnInvDSerCategory.Value, hdnInvDSerSubCategory.Value,
                    lblServiceFullName.Text, lblInvDdesc.Text, Convert.ToDecimal(txtInvDDisplayPrice.Text),
                    Convert.ToDecimal(hdnInvDPrice.Value), Convert.ToDecimal(hdnInvDExpense.Value),
                    Convert.ToDecimal(hdnInvDServiceCharge.Value), txtInvDAddServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDAddServiceCharge.Text),
                    Convert.ToDecimal(txtInvDQty.Text), Convert.ToDecimal(txtInvDTaxAmount.Text), Convert.ToDecimal(hdnInvDTax.Value),
                    Convert.ToDecimal(txtInvDPriceWitTax.Text), Convert.ToInt32(hdnInvDFineApplicable.Value), txtInvDFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDFine.Text), Convert.ToDecimal(txtInvDTotal.Text),
                    txtInvDdiscount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDdiscount.Text),

                      txtExpenseQty.Text == "" ? (decimal?)null : Convert.ToDecimal(txtExpenseQty.Text),
                   txtExpenseSinglAmt.Text == "" ? (decimal?)null : Convert.ToDecimal(txtExpenseSinglAmt.Text),
                   txtExpenseTotalAmt.Text == "" ? (decimal?)null : Convert.ToDecimal(txtExpenseTotalAmt.Text),
                   ExpenseSerComDate.SelectedDate == null ? (DateTime?)null : DateTime.ParseExact(CalDate(ExpenseSerComDate), "dd/MM/yyyy", CultureInfo.InvariantCulture));


                    foreach (RepeaterItem itms in rptTransCode.Items)
                    {
                        TextBox txtTransCode = (TextBox)itms.FindControl("txtTransCode");
                        dt_trans.Rows.Add(Convert.ToInt32(hdnInvDId.Value), txtTransCode.Text);
                    }

                    foreach (RepeaterItem itms in rptexpensein.Items)
                    {
                        TextBox txtSerComDetailId = (TextBox)itms.FindControl("txtSerComDetailId");
                        TextBox txtExpenseId = (TextBox)itms.FindControl("txtExpenseId");
                        TextBox txtAmount = (TextBox)itms.FindControl("txtAmount");
                        TextBox txtVAT = (TextBox)itms.FindControl("txtVAT");
                        TextBox txtVendorId = (TextBox)itms.FindControl("txtVendorId");
                        TextBox txtPayModeId = (TextBox)itms.FindControl("txtPayModeId");
                        TextBox txtAccountId = (TextBox)itms.FindControl("txtAccountId");
                        TextBox txtPayableAmount = (TextBox)itms.FindControl("txtPayableAmount");
                        TextBox txtPaidAmount = (TextBox)itms.FindControl("txtPaidAmount");

                        dt_exp.Rows.Add(Convert.ToInt32(hdnInvDId.Value),
                             txtSerComDetailId.Text == "" ? (int?)null : Convert.ToInt32(txtSerComDetailId.Text),
                             txtExpenseId.Text == "" ? (int?)null : Convert.ToInt32(txtExpenseId.Text),
                              txtAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtAmount.Text),
                              txtVAT.Text == "" ? (decimal?)null : Convert.ToDecimal(txtVAT.Text),
                              txtVendorId.Text == "" ? (int?)null : Convert.ToInt32(txtVendorId.Text),
                              txtPayModeId.Text == "" ? (int?)null : Convert.ToInt32(txtPayModeId.Text),
                              txtAccountId.Text == "" ? (int?)null : Convert.ToInt32(txtAccountId.Text),
                              txtPayableAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtPayableAmount.Text),
                              txtPaidAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtPaidAmount.Text));
                    }

                }
            }

            dt_ser.Rows.RemoveAt(itemrp.ItemIndex);
            rpt_Item_list.DataSource = dt_ser;
            rpt_Item_list.DataBind();

            //sc
            foreach (RepeaterItem itm in rpt_Item_list.Items)
            {
                Repeater rptTransCode = (Repeater)itm.FindControl("rptTransCode");
                Repeater rptexpensein = (Repeater)itm.FindControl("rptexpensein");
                HiddenField hdnInvDId = (HiddenField)itm.FindControl("hdnInvDId");

                DataTable dt_trans_in = new DataTable();
                dt_trans_in.Columns.Add("TransactionNumber", typeof(string));

                foreach (DataRow r in dt_trans.Rows)
                {
                    if (r["InvDId"].ToString() == hdnInvDId.Value)
                    {
                        dt_trans_in.Rows.Add(r["TransactionNumber"].ToString());
                    }
                }

                rptTransCode.DataSource = dt_trans_in;
                rptTransCode.DataBind();

                DataTable dt_expin = new DataTable();
                dt_expin.Columns.Add("InvDId", typeof(int));
                dt_expin.Columns.Add("SerComDetailId", typeof(int));
                dt_expin.Columns.Add("ExpenseId", typeof(int));
                dt_expin.Columns.Add("Amount", typeof(decimal));
                dt_expin.Columns.Add("VAT", typeof(decimal));
                dt_expin.Columns.Add("VendorId", typeof(int));
                dt_expin.Columns.Add("PayModeId", typeof(int));
                dt_expin.Columns.Add("AccountId", typeof(int));
                dt_expin.Columns.Add("PayableAmount", typeof(decimal));
                dt_expin.Columns.Add("PaidAmount", typeof(decimal));

                foreach (DataRow r in dt_exp.Rows)
                {
                    if (r["InvDId"].ToString() == hdnInvDId.Value)
                    {
                        dt_expin.Rows.Add(Convert.ToInt32(r["InvDId"]), Convert.ToInt32(r["SerComDetailId"]), Convert.ToInt32(r["ExpenseId"]),
                            Convert.ToDecimal(r["Amount"]), Convert.ToDecimal(r["VAT"]), Convert.ToInt32(r["VendorId"]), Convert.ToInt32(r["PayModeId"])
                            , Convert.ToInt32(r["AccountId"]), Convert.ToDecimal(r["PayableAmount"]), Convert.ToDecimal(r["PaidAmount"]));
                    }
                }

                rptexpensein.DataSource = dt_expin;
                rptexpensein.DataBind();
            }

            lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
            InlineCalculation();
            //Upd_InvoiceDetail_Panel.Update();
            Upd_Item_Panel.Update();
        }

        /*Edit Item*/
        protected void btn_edit_line_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            HiddenField hdnInvDIdP = (HiddenField)itemrp.FindControl("hdnInvDId");
            HiddenField hdnInvDDepartmentIdP = (HiddenField)itemrp.FindControl("hdnInvDDepartmentId");
            HiddenField hdnInvDCategoryIdP = (HiddenField)itemrp.FindControl("hdnInvDCategoryId");
            HiddenField hdnInvDSerSubCategoryIdP = (HiddenField)itemrp.FindControl("hdnInvDSerSubCategoryId");
            HiddenField hdnInvDServiceIdP = (HiddenField)itemrp.FindControl("hdnInvDServiceId");
            HiddenField hdnInvDDepartmentP = (HiddenField)itemrp.FindControl("hdnInvDDepartment");
            HiddenField hdnInvDSerCategoryP = (HiddenField)itemrp.FindControl("hdnInvDSerCategory");
            HiddenField hdnInvDSerSubCategoryP = (HiddenField)itemrp.FindControl("hdnInvDSerSubCategory");
            Label lblServiceFullNameP = (Label)itemrp.FindControl("lblServiceFullName");
            Label lblInvDdescP = (Label)itemrp.FindControl("lblInvDdesc");
            TextBox txtInvDDisplayPriceP = (TextBox)itemrp.FindControl("txtInvDDisplayPrice");
            HiddenField hdnInvDPriceP = (HiddenField)itemrp.FindControl("hdnInvDPrice");
            HiddenField hdnInvDExpenseP = (HiddenField)itemrp.FindControl("hdnInvDExpense");
            HiddenField hdnInvDServiceChargeP = (HiddenField)itemrp.FindControl("hdnInvDServiceCharge");
            TextBox txtInvDAddServiceChargeP = (TextBox)itemrp.FindControl("txtInvDAddServiceCharge");
            TextBox txtInvDQtyP = (TextBox)itemrp.FindControl("txtInvDQty");
            TextBox txtInvDTaxAmountP = (TextBox)itemrp.FindControl("txtInvDTaxAmount");
            HiddenField hdnInvDTaxP = (HiddenField)itemrp.FindControl("hdnInvDTax");
            TextBox txtInvDPriceWitTaxP = (TextBox)itemrp.FindControl("txtInvDPriceWitTax");
            HiddenField hdnInvDFineApplicableP = (HiddenField)itemrp.FindControl("hdnInvDFineApplicable");
            TextBox txtInvDFineP = (TextBox)itemrp.FindControl("txtInvDFine");
            TextBox txtInvDTotalP = (TextBox)itemrp.FindControl("txtInvDTotal");
            TextBox txtInvDdiscountP = (TextBox)itemrp.FindControl("txtInvDdiscount");

            ClearServiceDetail();

            hdn_InvDetailId.Value = hdnInvDIdP.Value;
            drpDepartment.SelectedValue = hdnInvDDepartmentIdP.Value;
            hdnDepartmentId.Value = hdnInvDDepartmentIdP.Value;
            drpSerCategory.SelectedValue = hdnInvDCategoryIdP.Value;
            hdnSerCategoryId.Value = hdnInvDCategoryIdP.Value;
            drpSerSubCategory.SelectedValue = hdnInvDSerSubCategoryIdP.Value;
            hdnSerSubCategoryId.Value = hdnInvDSerSubCategoryIdP.Value;
            drpService.SelectedValue = hdnInvDServiceIdP.Value;
            hdnDepartment.Value = hdnInvDDepartmentP.Value;
            hdnSerCategory.Value = hdnInvDSerCategoryP.Value;
            hdnSerSubCategory.Value = hdnInvDSerSubCategoryP.Value;
            txt_desc.Text = lblInvDdescP.Text;
            txt_displayPrice.Text = txtInvDDisplayPriceP.Text;
            hdnPrice.Value = hdnInvDPriceP.Value;
            hdn_expn.Value = hdnInvDExpenseP.Value;
            hdn_sc.Value = hdnInvDServiceChargeP.Value;
            txtServiceCharge.Text = txtInvDAddServiceChargeP.Text;
            txt_Qty.Text = txtInvDQtyP.Text;
            txt_taxamt.Text = txtInvDTaxAmountP.Text;
            hdn_tax.Value = hdnInvDTaxP.Value;
            txt_PriceWitTax.Text = txtInvDPriceWitTaxP.Text;
            hdnFineApplicable.Value = hdnInvDFineApplicableP.Value;
            txtFine.Text = txtInvDFineP.Text;
            txt_totPrice.Text = txtInvDTotalP.Text;
            txt_discount.Text = txtInvDdiscountP.Text;


            DataTable dt_ser = new DataTable();
            dt_ser.Columns.Add("D_id", typeof(int));
            dt_ser.Columns.Add("DepartmentId", typeof(int));
            dt_ser.Columns.Add("CategoryId", typeof(int));
            dt_ser.Columns.Add("ServiceSubCategoryId", typeof(int));
            dt_ser.Columns.Add("Service_Id", typeof(int));
            dt_ser.Columns.Add("DepartmentName", typeof(string));
            dt_ser.Columns.Add("SerCategoryName", typeof(string));
            dt_ser.Columns.Add("SerSubCategoryName", typeof(string));
            dt_ser.Columns.Add("ServiceFullName", typeof(string));
            dt_ser.Columns.Add("Particulars", typeof(string));
            dt_ser.Columns.Add("DisplayPrice", typeof(decimal));
            dt_ser.Columns.Add("Price", typeof(decimal));
            dt_ser.Columns.Add("Expense", typeof(decimal));
            dt_ser.Columns.Add("ServiceCharge", typeof(decimal));
            dt_ser.Columns.Add("AdditionalServiceCharge", typeof(decimal));
            dt_ser.Columns.Add("Quantity", typeof(decimal));
            dt_ser.Columns.Add("TaxAmount", typeof(decimal));
            dt_ser.Columns.Add("Tax", typeof(decimal));
            dt_ser.Columns.Add("PriceWitTax", typeof(decimal));
            dt_ser.Columns.Add("FineApplicable", typeof(int));
            dt_ser.Columns.Add("Fine", typeof(decimal));
            dt_ser.Columns.Add("Total", typeof(decimal));
            dt_ser.Columns.Add("Discount", typeof(decimal));

            //sc
            dt_ser.Columns.Add("ExpQty", typeof(int));
            dt_ser.Columns.Add("ExpSinglAmt", typeof(decimal));
            dt_ser.Columns.Add("ExpTotAmt", typeof(decimal));
            dt_ser.Columns.Add("SerComDate", typeof(DateTime));

            DataTable dt_exp = new DataTable();
            dt_exp.Columns.Add("InvDId", typeof(int));
            dt_exp.Columns.Add("SerComDetailId", typeof(int));
            dt_exp.Columns.Add("ExpenseId", typeof(int));
            dt_exp.Columns.Add("Amount", typeof(decimal));
            dt_exp.Columns.Add("VAT", typeof(decimal));
            dt_exp.Columns.Add("VendorId", typeof(int));
            dt_exp.Columns.Add("PayModeId", typeof(int));
            dt_exp.Columns.Add("AccountId", typeof(int));
            dt_exp.Columns.Add("PayableAmount", typeof(decimal));
            dt_exp.Columns.Add("PaidAmount", typeof(decimal));

            DataTable dt_trans = new DataTable();
            dt_trans.Columns.Add("InvDId", typeof(int));
            dt_trans.Columns.Add("TransactionNumber", typeof(string));

            if (rpt_Item_list.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_Item_list.Items)
                {
                    HiddenField hdnInvDId = (HiddenField)itm.FindControl("hdnInvDId");
                    HiddenField hdnInvDDepartmentId = (HiddenField)itm.FindControl("hdnInvDDepartmentId");
                    HiddenField hdnInvDCategoryId = (HiddenField)itm.FindControl("hdnInvDCategoryId");
                    HiddenField hdnInvDSerSubCategoryId = (HiddenField)itm.FindControl("hdnInvDSerSubCategoryId");
                    HiddenField hdnInvDServiceId = (HiddenField)itm.FindControl("hdnInvDServiceId");
                    HiddenField hdnInvDDepartment = (HiddenField)itm.FindControl("hdnInvDDepartment");
                    HiddenField hdnInvDSerCategory = (HiddenField)itm.FindControl("hdnInvDSerCategory");
                    HiddenField hdnInvDSerSubCategory = (HiddenField)itm.FindControl("hdnInvDSerSubCategory");
                    Label lblServiceFullName = (Label)itm.FindControl("lblServiceFullName");
                    Label lblInvDdesc = (Label)itm.FindControl("lblInvDdesc");
                    TextBox txtInvDDisplayPrice = (TextBox)itm.FindControl("txtInvDDisplayPrice");
                    HiddenField hdnInvDPrice = (HiddenField)itm.FindControl("hdnInvDPrice");
                    HiddenField hdnInvDExpense = (HiddenField)itm.FindControl("hdnInvDExpense");
                    HiddenField hdnInvDServiceCharge = (HiddenField)itm.FindControl("hdnInvDServiceCharge");
                    TextBox txtInvDAddServiceCharge = (TextBox)itm.FindControl("txtInvDAddServiceCharge");
                    TextBox txtInvDQty = (TextBox)itm.FindControl("txtInvDQty");
                    TextBox txtInvDTaxAmount = (TextBox)itm.FindControl("txtInvDTaxAmount");
                    HiddenField hdnInvDTax = (HiddenField)itm.FindControl("hdnInvDTax");
                    TextBox txtInvDPriceWitTax = (TextBox)itm.FindControl("txtInvDPriceWitTax");
                    HiddenField hdnInvDFineApplicable = (HiddenField)itm.FindControl("hdnInvDFineApplicable");
                    TextBox txtInvDFine = (TextBox)itm.FindControl("txtInvDFine");
                    TextBox txtInvDTotal = (TextBox)itm.FindControl("txtInvDTotal");
                    TextBox txtInvDdiscount = (TextBox)itm.FindControl("txtInvDdiscount");

                    //sc
                    TextBox txtExpenseQty = (TextBox)itm.FindControl("txtExpenseQty");
                    TextBox txtExpenseSinglAmt = (TextBox)itm.FindControl("txtExpenseSinglAmt");
                    TextBox txtExpenseTotalAmt = (TextBox)itm.FindControl("txtExpenseTotalAmt");
                    RadDatePicker ExpenseSerComDate = (RadDatePicker)itm.FindControl("ExpenseSerComDate");

                    Repeater rptTransCode = (Repeater)itm.FindControl("rptTransCode");
                    Repeater rptexpensein = (Repeater)itm.FindControl("rptexpensein");

                    dt_ser.Rows.Add(Convert.ToInt32(hdnInvDId.Value), hdnInvDDepartmentId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDDepartmentId.Value),
                     hdnInvDCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDCategoryId.Value), hdnInvDSerSubCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDSerSubCategoryId.Value),
                    Convert.ToInt32(hdnInvDServiceId.Value), hdnInvDDepartment.Value, hdnInvDSerCategory.Value, hdnInvDSerSubCategory.Value,
                    lblServiceFullName.Text, lblInvDdesc.Text, Convert.ToDecimal(txtInvDDisplayPrice.Text), Convert.ToDecimal(hdnInvDPrice.Value), Convert.ToDecimal(hdnInvDExpense.Value),
                    Convert.ToDecimal(hdnInvDServiceCharge.Value), txtInvDAddServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDAddServiceCharge.Text),
                    Convert.ToDecimal(txtInvDQty.Text), Convert.ToDecimal(txtInvDTaxAmount.Text), Convert.ToDecimal(hdnInvDTax.Value),
                    Convert.ToDecimal(txtInvDPriceWitTax.Text), Convert.ToInt32(hdnInvDFineApplicable.Value), txtInvDFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDFine.Text),
                    Convert.ToDecimal(txtInvDTotal.Text), txtInvDdiscount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDdiscount.Text),

                     txtExpenseQty.Text == "" ? (decimal?)null : Convert.ToDecimal(txtExpenseQty.Text),
                   txtExpenseSinglAmt.Text == "" ? (decimal?)null : Convert.ToDecimal(txtExpenseSinglAmt.Text),
                   txtExpenseTotalAmt.Text == "" ? (decimal?)null : Convert.ToDecimal(txtExpenseTotalAmt.Text),
                   ExpenseSerComDate.SelectedDate == null ? (DateTime?)null : DateTime.ParseExact(CalDate(ExpenseSerComDate), "dd/MM/yyyy", CultureInfo.InvariantCulture));

                    foreach (RepeaterItem itms in rptTransCode.Items)
                    {
                        TextBox txtTransCode = (TextBox)itms.FindControl("txtTransCode");
                        dt_trans.Rows.Add(Convert.ToInt32(hdnInvDId.Value), txtTransCode.Text);
                    }

                    foreach (RepeaterItem itms in rptexpensein.Items)
                    {
                        TextBox txtSerComDetailId = (TextBox)itms.FindControl("txtSerComDetailId");
                        TextBox txtExpenseId = (TextBox)itms.FindControl("txtExpenseId");
                        TextBox txtAmount = (TextBox)itms.FindControl("txtAmount");
                        TextBox txtVAT = (TextBox)itms.FindControl("txtVAT");
                        TextBox txtVendorId = (TextBox)itms.FindControl("txtVendorId");
                        TextBox txtPayModeId = (TextBox)itms.FindControl("txtPayModeId");
                        TextBox txtAccountId = (TextBox)itms.FindControl("txtAccountId");
                        TextBox txtPayableAmount = (TextBox)itms.FindControl("txtPayableAmount");
                        TextBox txtPaidAmount = (TextBox)itms.FindControl("txtPaidAmount");

                        dt_exp.Rows.Add(Convert.ToInt32(hdnInvDId.Value),
                             txtSerComDetailId.Text == "" ? (int?)null : Convert.ToInt32(txtSerComDetailId.Text),
                             txtExpenseId.Text == "" ? (int?)null : Convert.ToInt32(txtExpenseId.Text),
                              txtAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtAmount.Text),
                              txtVAT.Text == "" ? (decimal?)null : Convert.ToDecimal(txtVAT.Text),
                              txtVendorId.Text == "" ? (int?)null : Convert.ToInt32(txtVendorId.Text),
                              txtPayModeId.Text == "" ? (int?)null : Convert.ToInt32(txtPayModeId.Text),
                              txtAccountId.Text == "" ? (int?)null : Convert.ToInt32(txtAccountId.Text),
                              txtPayableAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtPayableAmount.Text),
                              txtPaidAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtPaidAmount.Text));
                    }

                }
            }

            dt_ser.Rows.RemoveAt(itemrp.ItemIndex);
            rpt_Item_list.DataSource = dt_ser;
            rpt_Item_list.DataBind();

            //sc
            foreach (RepeaterItem itm in rpt_Item_list.Items)
            {
                Repeater rptTransCode = (Repeater)itm.FindControl("rptTransCode");
                Repeater rptexpensein = (Repeater)itm.FindControl("rptexpensein");
                HiddenField hdnInvDId = (HiddenField)itm.FindControl("hdnInvDId");

                DataTable dt_trans_in = new DataTable();
                dt_trans_in.Columns.Add("TransactionNumber", typeof(string));

                foreach (DataRow r in dt_trans.Rows)
                {
                    if (r["InvDId"].ToString() == hdnInvDId.Value)
                    {
                        dt_trans_in.Rows.Add(r["TransactionNumber"].ToString());
                    }
                }

                rptTransCode.DataSource = dt_trans_in;
                rptTransCode.DataBind();

                DataTable dt_expin = new DataTable();
                dt_expin.Columns.Add("InvDId", typeof(int));
                dt_expin.Columns.Add("SerComDetailId", typeof(int));
                dt_expin.Columns.Add("ExpenseId", typeof(int));
                dt_expin.Columns.Add("Amount", typeof(decimal));
                dt_expin.Columns.Add("VAT", typeof(decimal));
                dt_expin.Columns.Add("VendorId", typeof(int));
                dt_expin.Columns.Add("PayModeId", typeof(int));
                dt_expin.Columns.Add("AccountId", typeof(int));
                dt_expin.Columns.Add("PayableAmount", typeof(decimal));
                dt_expin.Columns.Add("PaidAmount", typeof(decimal));

                foreach (DataRow r in dt_exp.Rows)
                {
                    if (r["InvDId"].ToString() == hdnInvDId.Value)
                    {
                        dt_expin.Rows.Add(Convert.ToInt32(r["InvDId"]), Convert.ToInt32(r["SerComDetailId"]), Convert.ToInt32(r["ExpenseId"]),
                            Convert.ToDecimal(r["Amount"]), Convert.ToDecimal(r["VAT"]), Convert.ToInt32(r["VendorId"]), Convert.ToInt32(r["PayModeId"])
                            , Convert.ToInt32(r["AccountId"]), Convert.ToDecimal(r["PayableAmount"]), Convert.ToDecimal(r["PaidAmount"]));
                    }
                }

                rptexpensein.DataSource = dt_expin;
                rptexpensein.DataBind();
            }

            lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
            //Upd_InvoiceDetail_Panel.Update();
            Upd_Item_Panel.Update();
        }

        #region SC

        protected void btnCompleSC_OnClick(object sender, EventArgs e)
        {
            ClearSC();
            hdn_ExpinvD_id.Value = "0";
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;
            HiddenField hdnInvDIdP = (HiddenField)itemrp.FindControl("hdnInvDId");
            HiddenField hdnInvDServiceIdP = (HiddenField)itemrp.FindControl("hdnInvDServiceId");
            Label lblServiceFullNameP = (Label)itemrp.FindControl("lblServiceFullName");
            TextBox txtInvDQtyP = (TextBox)itemrp.FindControl("txtInvDQty");
            TextBox txtInvDFineP = (TextBox)itemrp.FindControl("txtInvDFine");
            if (hdn_id.Value == "0")
            {
                DataTable dt = obj_trans.ServiceAmtForSingleQty(Convert.ToInt32(hdnInvDServiceIdP.Value));
                lbl_service.Text = lblServiceFullNameP.Text;
                hdn_service_id.Value = hdnInvDServiceIdP.Value;
                hdn_ExpinvD_id.Value = hdnInvDIdP.Value;
                txt_InvQty.Text = txtInvDQtyP.Text;
                txt_InComQty.Text = txtInvDQtyP.Text;
                txtInlineQty.Text = "1";
                if (dt.Rows.Count > 0)
                {
                    txtInlineAmtSQty.Text = (Convert.ToDecimal(dt.Rows[0]["AmtForSingleQty"].ToString()) + (txtInvDFineP.Text == "" ? 0 : Convert.ToDecimal(txtInvDFineP.Text))).ToString();
                    txtInlineTotAmt.Text = (Convert.ToDecimal(dt.Rows[0]["AmtForSingleQty"].ToString()) + (txtInvDFineP.Text == "" ? 0 : Convert.ToDecimal(txtInvDFineP.Text))).ToString();
                    InlineSerComDate.DbSelectedDate = dt.Rows[0]["SerComDate"].ToString();
                }
                else
                {
                    txtInlineAmtSQty.Text = "0";
                    txtInlineTotAmt.Text = "0";
                    InlineSerComDate.DbSelectedDate = DateTime.Now;
                }
            }
            else
            {
                lbl_service.Text = lblServiceFullNameP.Text;
                hdn_service_id.Value = hdnInvDServiceIdP.Value;
                hdn_ExpinvD_id.Value = hdnInvDIdP.Value;
                DataSet ds = obj_trans.Get_InvDetail_ServiceCompletionINVSC(Convert.ToInt32(hdn_ExpinvD_id.Value), Convert.ToInt32(hdn_user_id.Value));
                DataTable dt_ser = ds.Tables[0];/* Detail*/

                txt_InvQty.Text = dt_ser.Rows[0]["InvoiceQuantity"].ToString();
                txt_InComQty.Text = dt_ser.Rows[0]["InComQuantity"].ToString();
                txtInlineQty.Text = dt_ser.Rows[0]["Quantity"].ToString();
                txtInlineAmtSQty.Text = dt_ser.Rows[0]["AmtForSingleQty"].ToString();
                txtInlineTotAmt.Text = dt_ser.Rows[0]["TotalAmount"].ToString();
                InlineSerComDate.DbSelectedDate = dt_ser.Rows[0]["SerComDate"].ToString();
            }
            pnlSC.Visible = true;
            UpdSC.Update();
        }

        public void ClearSC()
        {
            hdn_InvDetailId.Value = "0";
            hdn_InComQty.Value = "0";
            DataTable dt_exp = new DataTable();
            dt_exp.Columns.Add("SerComDetailId", typeof(int));
            dt_exp.Columns.Add("ExpenseId", typeof(int));
            dt_exp.Columns.Add("ExpenseName", typeof(string));
            dt_exp.Columns.Add("Amount", typeof(decimal));
            dt_exp.Columns.Add("VAT", typeof(decimal));
            dt_exp.Columns.Add("VendorId", typeof(int));
            dt_exp.Columns.Add("PayModeId", typeof(int));
            dt_exp.Columns.Add("AccountId", typeof(int));
            dt_exp.Columns.Add("PayableAmount", typeof(decimal));
            dt_exp.Columns.Add("PaidAmount", typeof(decimal));

            rpt_expense_list.DataSource = dt_exp;
            rpt_expense_list.DataBind();
            txtscqty.Text = "";
            txtscqty.Enabled = true;
            txt_amtSQty.Text = "";
            txt_totAmt.Text = "";
            SerComDate.DbSelectedDate = DateTime.Now;
            pnlSC.Visible = false;
            pnl_Expense_Panel.Visible = false;
            UpdSC.Update();
        }

        protected void btn_expDetail_line_OnClick(object sender, EventArgs e)
        {
            txtscqty.Text = "";
            txt_amtSQty.Text = "";
            txt_totAmt.Text = "";
            //txtscqty.Text = txtInlineQty.Text;
            //txt_amtSQty.Text = txtInlineAmtSQty.Text;
            //txt_totAmt.Text = txtInlineTotAmt.Text;
            SerComDate.DbSelectedDate = InlineSerComDate.SelectedDate;
            txtscqty.Enabled = true;
            hdn_InComQty.Value = txt_InComQty.Text;
            pnl_Expense_Panel.Visible = true;

            if (hdn_id.Value == "0")
            {
                DataSet ds = obj_trans.Get_SerExpenseDetail_SC_byService(Convert.ToInt32(hdn_service_id.Value));
                DataTable dt1 = ds.Tables[0];/*invoic*/
                rpt_expense_list.DataSource = dt1;
                rpt_expense_list.DataBind();
            }
            else
            {
                DataSet ds = obj_trans.Get_SerExpenseDetail_ServiceCompletion(Convert.ToInt32(hdn_ExpinvD_id.Value));
                DataTable dt1 = ds.Tables[0];/*invoic*/
                rpt_expense_list.DataSource = dt1;
                rpt_expense_list.DataBind();
            }
            Upd_Expense_Panel.Update();
        }

        protected void btnInlineExpenseSave_OnClick(object sender, EventArgs e)
        {
            int DisplayMessage = 0;
            txtscqty.Text = txtInlineQty.Text;
            txt_amtSQty.Text = txtInlineAmtSQty.Text;
            txt_totAmt.Text = txtInlineTotAmt.Text;
            SerComDate.DbSelectedDate = InlineSerComDate.SelectedDate;
            txtscqty.Enabled = true;
            hdn_InComQty.Value = txt_InComQty.Text;
            pnl_Expense_Panel.Visible = false;

            if (hdn_id.Value == "0")
            {
                DataSet ds = obj_trans.Get_SerExpenseDetail_SC_byService(Convert.ToInt32(hdn_service_id.Value));
                DataTable dt1 = ds.Tables[0];/*invoic*/
                rpt_expense_list.DataSource = dt1;
                rpt_expense_list.DataBind();
            }
            else
            {
                //DataSet ds = obj_trans.Get_SerExpenseDetail_SC_byService(Convert.ToInt32(hdn_service_id.Value));
                //DataTable dt1 = ds.Tables[0];/*invoic*/
                //rpt_expense_list.DataSource = dt1;
                //rpt_expense_list.DataBind();
                DataSet ds = obj_trans.Get_SerExpenseDetail_ServiceCompletion(Convert.ToInt32(hdn_ExpinvD_id.Value));
                DataTable dt1 = ds.Tables[0];/*invoic*/
                rpt_expense_list.DataSource = dt1;
                rpt_expense_list.DataBind();
            }

            DataTable dt_exp = new DataTable();
            dt_exp.Columns.Add("SerComDetailId", typeof(int));
            dt_exp.Columns.Add("ExpenseId", typeof(int));
            dt_exp.Columns.Add("ExpenseName", typeof(string));
            dt_exp.Columns.Add("Amount", typeof(decimal));
            dt_exp.Columns.Add("VAT", typeof(decimal));
            dt_exp.Columns.Add("VendorId", typeof(int));
            dt_exp.Columns.Add("PayModeId", typeof(int));
            dt_exp.Columns.Add("AccountId", typeof(int));
            dt_exp.Columns.Add("PayableAmount", typeof(decimal));
            dt_exp.Columns.Add("PaidAmount", typeof(decimal));

            foreach (RepeaterItem expItem in rpt_expense_list.Items)
            {
                HiddenField hdn_expenseId = (HiddenField)expItem.FindControl("hdn_expenseId");
                Label lbl_Expense = (Label)expItem.FindControl("lbl_Expense");
                TextBox txt_amt = (TextBox)expItem.FindControl("txt_amt");
                TextBox txt_vat = (TextBox)expItem.FindControl("txt_vat");
                RadComboBox drp_vendor = (RadComboBox)expItem.FindControl("drp_vendor");
                RadComboBox drp_payMode = (RadComboBox)expItem.FindControl("drp_payMode");
                RadComboBox drp_account = (RadComboBox)expItem.FindControl("drp_account");
                TextBox txt_payableAmount = (TextBox)expItem.FindControl("txt_payableAmount");
                TextBox txt_paidAmount = (TextBox)expItem.FindControl("txt_paidAmount");
                decimal PayableAmount = 0;
                if (drp_vendor.SelectedValue != "" && drp_payMode.SelectedValue != "" && drp_account.SelectedValue != "")
                {
                    PayableAmount = (Convert.ToDecimal(txt_amt.Text) + Convert.ToDecimal(txt_vat.Text)) * Convert.ToDecimal(txtscqty.Text);
                    dt_exp.Rows.Add(0, Convert.ToInt32(hdn_expenseId.Value), lbl_Expense.Text,
                Convert.ToDecimal(txt_amt.Text), Convert.ToDecimal(txt_vat.Text),
                Convert.ToInt32(drp_vendor.SelectedValue), Convert.ToInt32(drp_payMode.SelectedValue),
                Convert.ToInt32(drp_account.SelectedValue), PayableAmount, PayableAmount);
                }
                else
                {
                    DisplayMessage = 1;
                    break;
                }
            }
            if (DisplayMessage == 1)
            {
                pnl_Expense_Panel.Visible = true;
                txtscqty.Text = "";
                txt_amtSQty.Text = "";
                txt_totAmt.Text = "";
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please Add Service Expense Detail');", true);
            }
            else
            {
                rpt_expense_list.DataSource = dt_exp;
                rpt_expense_list.DataBind();
                btn_saveSC_OnClick(null, null);
            }
            Upd_Expense_Panel.Update();
        }

        protected void btn_closeSC_OnClick(object sender, EventArgs e)
        {
            pnlSC.Visible = false;
            UpdSC.Update();
        }

        protected void btn_saveSC_OnClick(object sender, EventArgs e)
        {
            if (rpt_expense_list.Items.Count > 0)
            {
                int NoOfRows = Convert.ToInt32(txtscqty.Text);
                DataTable dt_trans = new DataTable();
                dt_trans.Columns.Add("TransactionNumber", typeof(string));

                for (int i = 0; i < NoOfRows; i++)
                {
                    dt_trans.Rows.Add("");
                }
                rpt_TransacDetail.DataSource = dt_trans;
                rpt_TransacDetail.DataBind();
                pnl_transaDetail.Visible = true;
                Upd_TransaDetail_Panel.Update();
            }
            else
            {
                DataSet dt_deatils = SCfill_Detail();
                SaveServiceCompletion(dt_deatils);
                pnl_transaDetail.Visible = false;
                Upd_Add_PanelInner.Update();
                pnlSC.Visible = false;
                UpdSC.Update();
            }
        }

        public void SaveServiceCompletion(DataSet ds_deatils)
        {
            foreach (RepeaterItem itm in rpt_Item_list.Items)
            {
                HiddenField hdnInvDId = (HiddenField)itm.FindControl("hdnInvDId");
                TextBox txtExpenseQty = (TextBox)itm.FindControl("txtExpenseQty");
                TextBox txtExpenseSinglAmt = (TextBox)itm.FindControl("txtExpenseSinglAmt");
                TextBox txtExpenseTotalAmt = (TextBox)itm.FindControl("txtExpenseTotalAmt");
                RadDatePicker ExpenseSerComDate = (RadDatePicker)itm.FindControl("ExpenseSerComDate");

                Repeater rptTransCode = (Repeater)itm.FindControl("rptTransCode");
                Repeater rptexpensein = (Repeater)itm.FindControl("rptexpensein");


                if (hdnInvDId.Value == hdn_ExpinvD_id.Value)
                {
                    txtExpenseQty.Text = txtscqty.Text;
                    txtExpenseSinglAmt.Text = txt_amtSQty.Text;
                    txtExpenseTotalAmt.Text = txt_totAmt.Text;
                    ExpenseSerComDate.DbSelectedDate = DateTime.ParseExact(CalDate(SerComDate), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    rptexpensein.DataSource = ds_deatils.Tables[0];
                    rptexpensein.DataBind();
                    rptTransCode.DataSource = ds_deatils.Tables[1];
                    rptTransCode.DataBind();

                    break;
                }
            }
            Upd_Item_Panel.Update();
        }

        public DataSet SCfill_Detail()
        {
            DataSet ds = new DataSet();
            DataTable dt_exp = new DataTable();
            dt_exp.Columns.Add("InvDId", typeof(int));
            dt_exp.Columns.Add("SerComDetailId", typeof(int));
            dt_exp.Columns.Add("ExpenseId", typeof(int));
            dt_exp.Columns.Add("Amount", typeof(decimal));
            dt_exp.Columns.Add("VAT", typeof(decimal));
            dt_exp.Columns.Add("VendorId", typeof(int));
            dt_exp.Columns.Add("PayModeId", typeof(int));
            dt_exp.Columns.Add("AccountId", typeof(int));
            dt_exp.Columns.Add("PayableAmount", typeof(decimal));
            dt_exp.Columns.Add("PaidAmount", typeof(decimal));

            DataTable dt_trans = new DataTable();
            dt_trans.Columns.Add("TransactionNumber", typeof(string));

            if (rpt_expense_list.Items.Count > 0)
            {
                foreach (RepeaterItem expItem in rpt_expense_list.Items)
                {
                    HiddenField hdnSerComDetailId = (HiddenField)expItem.FindControl("hdnSerComDetailId");
                    HiddenField hdn_expenseId = (HiddenField)expItem.FindControl("hdn_expenseId");
                    TextBox txt_amt = (TextBox)expItem.FindControl("txt_amt");
                    TextBox txt_vat = (TextBox)expItem.FindControl("txt_vat");
                    RadComboBox drp_vendor = (RadComboBox)expItem.FindControl("drp_vendor");
                    RadComboBox drp_payMode = (RadComboBox)expItem.FindControl("drp_payMode");
                    RadComboBox drp_account = (RadComboBox)expItem.FindControl("drp_account");
                    TextBox txt_payableAmount = (TextBox)expItem.FindControl("txt_payableAmount");
                    TextBox txt_paidAmount = (TextBox)expItem.FindControl("txt_paidAmount");

                    dt_exp.Rows.Add(Convert.ToInt32(hdn_InvDetailId.Value), Convert.ToInt32(hdnSerComDetailId.Value), Convert.ToInt32(hdn_expenseId.Value),
                Convert.ToDecimal(txt_amt.Text), Convert.ToDecimal(txt_vat.Text),
                Convert.ToInt32(drp_vendor.SelectedValue), Convert.ToInt32(drp_payMode.SelectedValue),
                Convert.ToInt32(drp_account.SelectedValue), Convert.ToDecimal(txt_payableAmount.Text),
                Convert.ToDecimal(txt_paidAmount.Text));
                }
            }
            if (rpt_TransacDetail.Items.Count > 0)
            {
                foreach (RepeaterItem Item in rpt_TransacDetail.Items)
                {
                    TextBox txt_transNumber = (TextBox)Item.FindControl("txt_transNumber");
                    dt_trans.Rows.Add(txt_transNumber.Text);
                }
            }
            ds.Tables.Add(dt_exp);
            ds.Tables.Add(dt_trans);
            return ds;
        }

        protected void btn_FinalSave_OnClick(object sender, EventArgs e)
        {
            DataSet dt_deatils = SCfill_Detail();

            if (dt_deatils.Tables[0].Rows.Count > 0)
            {
                SaveServiceCompletion(dt_deatils);
                pnlSC.Visible = false;
                UpdSC.Update();
            }
            else
            {
                lbl_msg.Text = "Add Quantity to Continue !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            pnl_transaDetail.Visible = false;
            Upd_TransaDetail_Panel.Update();
        }

        protected void btn_TransDetail_Close_OnClick(object sender, EventArgs e)
        {
            pnl_transaDetail.Visible = false;
            Upd_TransaDetail_Panel.Update();
        }

        protected void rpt_expense_list_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdn_vendorId = (HiddenField)e.Item.FindControl("hdn_vendorId");
                RadComboBox drp_vendor = (RadComboBox)e.Item.FindControl("drp_vendor");
                drp_vendor.Items.Clear();
                DataTable dtVendor = obj_mas.Drp_Vendor();
                drp_vendor.DataSource = dtVendor;
                drp_vendor.DataValueField = "Value";
                drp_vendor.DataTextField = "Text";
                drp_vendor.DataBind();
                drp_vendor.SelectedValue = hdn_vendorId.Value;

                HiddenField hdn_payModeId = (HiddenField)e.Item.FindControl("hdn_payModeId");
                RadComboBox drp_payMode = (RadComboBox)e.Item.FindControl("drp_payMode");
                drp_payMode.Items.Clear();
                DataTable dtPayMode = obj_mas.Drp_PaymentMode_WithoutCredit();
                drp_payMode.DataSource = dtPayMode;
                drp_payMode.DataValueField = "Value";
                drp_payMode.DataTextField = "Text";
                drp_payMode.DataBind();
                drp_payMode.SelectedValue = hdn_payModeId.Value;
                drp_payMode.Items.Remove(drp_payMode.Items.FindItemByValue("2"));/*Remove Cheque*/

                HiddenField hdn_accountId = (HiddenField)e.Item.FindControl("hdn_accountId");
                RadComboBox drp_account = (RadComboBox)e.Item.FindControl("drp_account");
                drp_account.Items.Clear();
                if (hdn_payModeId.Value != "")
                {
                    DataTable dtAccount = obj_trans.ListAccountInServCompletion(Convert.ToInt32(hdn_payModeId.Value), Convert.ToInt32(hdn_user_id.Value),
                        hdn_accountId.Value == "" ? 0 : Convert.ToInt32(hdn_accountId.Value));
                    drp_account.DataSource = dtAccount;
                    drp_account.DataValueField = "Value";
                    drp_account.DataTextField = "Text";
                    drp_account.DataBind();
                }
                drp_account.SelectedValue = hdn_accountId.Value;
            }

        }

        protected void drp_payMode_OnSelectedIndexChangedSC(object sender, EventArgs e)
        {
            RadComboBox drp = (RadComboBox)sender;
            RepeaterItem itm = (RepeaterItem)drp.Parent;
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            HiddenField hdn_accountId = (HiddenField)itm.FindControl("hdn_accountId");
            RadComboBox drp_account = (RadComboBox)itm.FindControl("drp_account");
            UpdatePanel Upd_Account_Panel = (UpdatePanel)itm.FindControl("Upd_Account_Panel");
            drp_account.Items.Clear();
            if (drp.SelectedValue != "")
            {
                DataTable dtAccount = obj_trans.ListAccountInServCompletion(Convert.ToInt32(drp.SelectedValue), Convert.ToInt32(hdn_user_id.Value), 0);
                drp_account.DataSource = dtAccount;
                drp_account.DataValueField = "Value";
                drp_account.DataTextField = "Text";
                drp_account.DataBind();
            }
            hdn_accountId.Value = "";
            drp_account.ClearSelection();
            drp_account.Text = "";
            Upd_Account_Panel.Update();
        }

        #endregion


        /*Reset*/
        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
        }

        protected void drp_cardType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            pnlCardAcc.Visible = false;
            drp_CardAcc.ClearSelection();
            drp_CardAcc.Text = "";
            ReqCardAcc.Enabled = false;

            if (drpCardType.SelectedValue == "2")/*Company*/
            {
                drp_CardAcc.DataSource = BalVoucher.GetEdhirhmBankAccountList_QR(Convert.ToInt32(hdn_user_id.Value));
                drp_CardAcc.DataValueField = "Value";
                drp_CardAcc.DataTextField = "Text";
                drp_CardAcc.DataBind();
                drp_CardAcc.Visible = true;
                drp_CardAcc.ClearSelection();
                drp_CardAcc.Text = "";

                ReqCardAcc.Enabled = true;
                pnlCardAcc.Visible = true;
            }

            Upd_CardAcc.Update();
        }

        protected void drp_payMode_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            pnl_PayMode_Panel.Visible = false;
            pnl_Cheque_Panel.Visible = false;
            cheque_date.DbSelectedDate = "";
            txt_chqNumber.Text = "";

            hdn_bankcommsn.Value = "0";
            txt_commsn.Text = "";
            if (drp_payMode.SelectedValue != "")
            {
                if (drp_payMode.SelectedValue == "1")/*PettyCash*/
                {
                    drpPettyCash.DataSource = BalVoucher.GetPettyCashAccountList_QR(Convert.ToInt32(hdn_user_id.Value));
                    drpPettyCash.DataValueField = "Value";
                    drpPettyCash.DataTextField = "Text";
                    drpPettyCash.DataBind();
                    drpPettyCash.Visible = true;
                    drpPettyCash.ClearSelection();
                    drpPettyCash.Text = "";
                    if (drpPettyCash.Items.Count == 1)
                        drpPettyCash.SelectedValue = drpPettyCash.Items[0].Value;

                    DataSet dsemp = obj_mas.Edit_Employee(Convert.ToInt32(hdn_user_id.Value));
                    DataTable dtemp = dsemp.Tables[0];
                    if (dtemp.Rows[0]["DefaultPaymentAccount"].ToString() != "")
                        drpPettyCash.SelectedValue = dtemp.Rows[0]["DefaultPaymentAccount"].ToString();

                    drpBankAccount.DataSource = "";
                    drpBankAccount.DataBind();
                    drpBankAccount.Visible = false;
                    drpBankAccount.ClearSelection();
                    drpBankAccount.Text = "";

                    lblToLabel.Text = "Petty Cash Name / اسم المصروفات النثرية";
                    lblToLabel.Visible = true;
                    rqTo.ValidationGroup = "save";
                    rqTo.ControlToValidate = "drpPettyCash";

                    pnl_PayMode_Panel.Visible = true;
                    pnl_Cheque_Panel.Visible = false;
                }
                else if (drp_payMode.SelectedValue == "2")/*Bank Transfer*/
                {
                    drpBankAccount.DataSource = BalVoucher.GetBankAccountList_QR(Convert.ToInt32(hdn_user_id.Value));
                    drpBankAccount.DataValueField = "Value";
                    drpBankAccount.DataTextField = "Text";
                    drpBankAccount.DataBind();
                    drpBankAccount.Visible = true;
                    drpBankAccount.ClearSelection();
                    drpBankAccount.Text = "";

                    DataSet dsemp = obj_mas.Edit_Employee(Convert.ToInt32(hdn_user_id.Value));
                    DataTable dtemp = dsemp.Tables[0];
                    if (dtemp.Rows[0]["DefaultPaymentAccount"].ToString() != "")
                    {
                        drpBankAccount.SelectedValue = dtemp.Rows[0]["DefaultPaymentAccount"].ToString();
                        onchangedrp_bank(null, null);
                    }

                    drpPettyCash.ClearSelection();
                    drpPettyCash.Text = "";
                    drpPettyCash.DataSource = "";
                    drpPettyCash.DataBind();
                    drpPettyCash.Visible = false;

                    lblToLabel.Text = "Bank Name / اسم البنك";
                    lblToLabel.Visible = true;
                    rqTo.ValidationGroup = "save";
                    rqTo.ControlToValidate = "drpBankAccount";

                    pnl_PayMode_Panel.Visible = true;
                    pnl_Cheque_Panel.Visible = false;
                }
                else if (drp_payMode.SelectedValue == "3")/*Cheque*/
                {
                    drpBankAccount.DataSource = "";
                    drpBankAccount.DataBind();
                    drpBankAccount.Visible = false;
                    drpBankAccount.ClearSelection();
                    drpBankAccount.Text = "";

                    drpPettyCash.ClearSelection();
                    drpPettyCash.Text = "";
                    drpPettyCash.DataSource = "";
                    drpPettyCash.DataBind();
                    drpPettyCash.Visible = false;


                    lblToLabel.Text = "Bank Name / اسم البنك";
                    lblToLabel.Visible = false;
                    rqTo.ValidationGroup = "no";
                    rqTo.ControlToValidate = "drpBankAccount";

                    pnl_PayMode_Panel.Visible = false;
                    pnl_Cheque_Panel.Visible = true;
                }
                else if (drp_payMode.SelectedValue == "4")/*Credit*/
                {
                    drpBankAccount.DataSource = "";
                    drpBankAccount.DataBind();
                    drpBankAccount.Visible = false;
                    drpBankAccount.ClearSelection();
                    drpBankAccount.Text = "";

                    drpPettyCash.DataSource = "";
                    drpPettyCash.DataBind();
                    drpPettyCash.Visible = false;
                    drpPettyCash.ClearSelection();
                    drpPettyCash.Text = "";

                    lblToLabel.Text = "Bank Name / اسم البنك";
                    lblToLabel.Visible = false;
                    rqTo.ValidationGroup = "no";
                    rqTo.ControlToValidate = "drpBankAccount";

                    pnl_PayMode_Panel.Visible = false;
                    pnl_Cheque_Panel.Visible = false;
                }

            }

            Upd_PayMode_Panel.Update();
            upd_commsn.Update();
            Upd_Cheque_Panel.Update();
        }

        protected void onchangedrp_bank(object sender, EventArgs e)
        {
            hdn_bankcommsn.Value = "0";

            if (drpBankAccount.SelectedValue != "")
            {
                DataTable dt = obj_mas.Edit_Bank_Account(Convert.ToInt32(drpBankAccount.SelectedValue));
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["IsCommssionApp"].ToString() == "1" & dt.Rows[0]["CommissionPer"].ToString() != "")
                        hdn_bankcommsn.Value = dt.Rows[0]["CommissionPer"].ToString();
                }
            }
            Upd_PayMode_Panel.Update();
            CalCommission();
        }

        public void CalCommission()
        {
            txt_commsn.Text = "";
            if (txt_amtPayNow.Text != "" & hdn_bankcommsn.Value != "0")
            {
                txt_commsn.Text = (Convert.ToDecimal(txt_amtPayNow.Text) * (Convert.ToDecimal(hdn_bankcommsn.Value) / 100)).ToString("0.00");
            }
            upd_commsn.Update();
        }

        #region History

        protected void btn_histry_OnClick(object sender, EventArgs e)
        {
            date_from.SelectedDate = null;
            date_to.SelectedDate = null;

            grid_fill_his(1, 10);

            div_main.Visible = false;
            div_trans_main.Visible = true;
            upd_main.Update();
        }

        protected void btn_his_seacrh_OnClick(object sender, EventArgs e)
        {
            grid_fill_his(1, 10);

            Upd_History.Update();
        }

        protected void btnexcel_exportHis_OnClick(object sender, EventArgs e)
        {
            DataSet ds = obj_trans.list_InvHistry(date_from.SelectedDate, date_to.SelectedDate, Convert.ToInt32(hdn_id.Value),
                Convert.ToInt32(lbl_page_number1.Text), Convert.ToInt32(drp_count1.SelectedValue));
            DataTable dt = ds.Tables[0];

            dt.Columns.Remove("current_count");
            dt.Columns.Remove("page_number");
            dt.Columns.Remove("Page_size");
            dt.Columns.Remove("start_num");
            dt.Columns.Remove("end_num");
            dt.Columns.Remove("last_page");

            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "Invoicehistory");
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                HttpContext.Current.Response.Write(style);
                Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        protected void btn_histry_Close_OnClick(object sender, EventArgs e)
        {
            div_main.Visible = true;
            div_trans_main.Visible = false;
            upd_main.Update();
        }

        public void grid_fill_his(int page_number, int page_size)
        {
            DataSet ds = obj_trans.list_InvHistry(date_from.SelectedDate, date_to.SelectedDate, Convert.ToInt32(hdn_id.Value),
                page_number, page_size);
            DataTable dt = ds.Tables[0];

            rpt_His.DataSource = dt;
            rpt_His.DataBind();

            if (dt.Rows.Count > 0)
            {
                lbl_page_info1.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["SLNo"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
                hdn_last_page1.Value = dt.Rows[0]["last_page"].ToString();
                lbl_page_number1.Text = dt.Rows[0]["page_number"].ToString();
                hdn_total1.Value = dt.Rows[0]["current_count"].ToString();
            }
            else
            {
                lbl_page_info1.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
                hdn_last_page1.Value = "0";
                lbl_page_number1.Text = "1";
                hdn_total1.Value = "0";
            }
            upd_his_nav.Update();
            Upd_History.Update();
        }

        #region his Navigation

        //First Page
        protected void btn_first1_OnClick(object sender, EventArgs e)
        {
            grid_fill_his(1, Convert.ToInt32(drp_count1.SelectedValue));
        }

        //Previous Page
        protected void btn_prev1_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number1.Text) > 1)
            {
                grid_fill_his(Convert.ToInt32(lbl_page_number1.Text) - 1, Convert.ToInt32(drp_count1.SelectedValue));
            }
        }

        //Next Page
        protected void btn_next1_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number1.Text) < Convert.ToInt32(hdn_last_page1.Value))
            {
                grid_fill_his(Convert.ToInt32(lbl_page_number1.Text) + 1, Convert.ToInt32(drp_count1.SelectedValue));
            }
        }

        //Last Page
        protected void btn_last1_OnClick(object sender, EventArgs e)
        {
            grid_fill_his(Convert.ToInt32(hdn_last_page1.Value), Convert.ToInt32(drp_count1.SelectedValue));
        }

        //Page Data Count
        protected void drp_count1_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            grid_fill_his(1, Convert.ToInt32(drp_count1.SelectedValue));
        }

        #endregion

        #endregion

        #region Navigation

        /*txt_search OnTextChanged*/
        protected void txt_search_OnTextChanged(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), txt_search.Text, Common_order_column.Value, Common_asc_desc.Value);
        }

        //First Page
        protected void btn_first_OnClick(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
        }

        //Previous Page
        protected void btn_prev_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number.Text) > 1)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text) - 1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
            }
        }

        //Next Page
        protected void btn_next_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number.Text) < Convert.ToInt32(hdn_last_page.Value))
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text) + 1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
            }
        }

        //Last Page
        protected void btn_last_OnClick(object sender, EventArgs e)
        {

            grid_fill(Convert.ToInt32(hdn_last_page.Value), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
        }

        //Page Data Count
        protected void drp_count_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
        }

        #endregion

        protected void btn_close_OnClick(object sender, EventArgs e)
        {
            pnl_add.Visible = false;
            Upd_Add_Panel.Update();
        }

        protected void btn_newentry_OnClick(object sender, EventArgs e)
        {
            Clear();
            pnl_add.Visible = true;
            Upd_Add_Panel.Update();
        }

        public void ClearServiceDetail()
        {
            lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
            hdn_InvDetailId.Value = "-" + (rpt_Item_list.Items.Count + 1).ToString();
            drpDepartment.ClearSelection();
            drpDepartment.Text = "";
            drpSerCategory.ClearSelection();
            drpSerCategory.Text = "";
            drpSerSubCategory.ClearSelection();
            drpSerSubCategory.Text = "";
            drpService.ClearSelection();
            drpService.Text = "";

            fill_FilterDropDown(0);
        }

        /*Clear All the Data*/
        public void Clear()
        {
            hdn_bankcommsn.Value = "0";
            hdnInvoiceStatus.Value = "0";
            txt_commsn.Text = "";
            hdn_PageName.Value = "Invoice";/*Used in Customer User Control*/
            hdn_id.Value = "0";
            drp_customer.ClearSelection();
            drp_customer.Text = "";
            drp_customer_OnSelectedIndexChanged(null, null);
            drp_customer.Enabled = true;
            drp_quot.Items.Clear();
            drp_quot.Text = "";
            hdn_CurrentInvoiceReceivable.Value = "0";
            drp_quot.Enabled = true;
            txt_remark.Text = "";
            txt_grand.Text = "";
            if (hdnDefaultInvoiceType.Value == "1")
            {
                rbTaxInvoice.Checked = true;
                rbNormalInvoice.Checked = false;
            }
            else if (hdnDefaultInvoiceType.Value == "2")
            {
                rbTaxInvoice.Checked = false;
                rbNormalInvoice.Checked = true;
            }
            job_date.DbSelectedDate = DateTime.Now;

            drpDepartment.ClearSelection();
            drpDepartment.Text = "";
            drpSerCategory.ClearSelection();
            drpSerCategory.Text = "";
            drpSerSubCategory.ClearSelection();
            drpSerSubCategory.Text = "";
            drpService.ClearSelection();
            drpService.Text = "";
            fill_FilterDropDown(0);
            DataTable dtgen = obj_mas.Edit_GeneralSettings();
            if (dtgen.Rows[0]["DefaultPayModeInQuickReceipt"].ToString() != "")
                drp_payMode.SelectedValue = dtgen.Rows[0]["DefaultPayModeInQuickReceipt"].ToString();
            else
                drp_payMode.SelectedValue = "1";
            drp_payMode_OnSelectedIndexChanged(null, null);

            drpCardType.SelectedValue = "1";
            drp_cardType_OnSelectedIndexChanged(null, null);
            
            cheque_date.DbSelectedDate = "";
            txt_chqNumber.Text = "";

            drpTemplates.Text = string.Empty;
            drpTemplates.ClearCheckedItems();

            DataTable dt_ser = new DataTable();
            dt_ser.Columns.Add("D_id", typeof(int));
            dt_ser.Columns.Add("CategoryId", typeof(int));
            dt_ser.Columns.Add("Service_Id", typeof(int));
            dt_ser.Columns.Add("DepartmentName", typeof(string));
            dt_ser.Columns.Add("SerCategoryName", typeof(string));
            dt_ser.Columns.Add("SerSubCategoryName", typeof(string));
            dt_ser.Columns.Add("ServiceFullName", typeof(string));
            dt_ser.Columns.Add("Particulars", typeof(string));
            dt_ser.Columns.Add("DisplayPrice", typeof(decimal));
            dt_ser.Columns.Add("Price", typeof(decimal));
            dt_ser.Columns.Add("Expense", typeof(decimal));
            dt_ser.Columns.Add("ServiceCharge", typeof(decimal));
            dt_ser.Columns.Add("AdditionalServiceCharge", typeof(decimal));
            dt_ser.Columns.Add("Quantity", typeof(decimal));
            dt_ser.Columns.Add("TaxAmount", typeof(decimal));
            dt_ser.Columns.Add("Tax", typeof(decimal));
            dt_ser.Columns.Add("PriceWitTax", typeof(decimal));
            dt_ser.Columns.Add("FineApplicable", typeof(int));
            dt_ser.Columns.Add("Fine", typeof(decimal));
            dt_ser.Columns.Add("Total", typeof(decimal));
            dt_ser.Columns.Add("Discount", typeof(decimal));

            rpt_Item_list.DataSource = dt_ser;
            rpt_Item_list.DataBind();
            ClearServiceDetail();
            btn_save.Visible = hdn_add.Value == "0" ? false : true;
            btn_Salesprint.Visible = hdn_Salesprint.Value == "0" ? false : true;
            btn_ReceiptPrint.Visible = hdn_Receiptprint.Value == "0" ? false : true;
            btn_TaxInvoicePrint.Visible = false;
            btn_cancel.Visible = false;
            btn_history.Visible = false;
            Get_Code();

            Upd_Add_PanelInner.Update();
        }

        /*Code for Display*/
        public void Get_Code()
        {
            DataTable dt = obj_common.Get_Code(16);
            if (dt.Rows.Count > 0)
                lbl_Code.Text = dt.Rows[0][0].ToString();

            dt = obj_common.Get_Code(18);
            if (dt.Rows.Count > 0)
                lbl_RecCode.Text = dt.Rows[0][0].ToString();
        }

        /*Check Action Privilege*/
        public void previlage_action_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    DataTable dt = obj_common.Action_Previlage_Validation(55, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdn_update.Value = dt.Rows[1][1].ToString();
                        hdn_Salesprint.Value = dt.Rows[2][1].ToString();
                        hdn_TaxInvoicePrint.Value = dt.Rows[3][1].ToString();
                        hdn_Receiptprint.Value = dt.Rows[4][1].ToString();
                        hdn_histry.Value = dt.Rows[5][1].ToString();
                        hdn_cancel.Value = dt.Rows[6][1].ToString();
                    }
                }
                else
                {
                    Response.Redirect("../Landing.aspx");
                }
            }
            catch
            {
                Response.Redirect("../Landing.aspx");
            }
        }

        /*Check Form Privilege*/
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.Form_Previlage_Validation(16, Convert.ToInt32(hdn_user_id.Value));
                    if (val == 0)
                    {
                        Response.Redirect("../Landing.aspx");
                    }

                }
                else
                {
                    Response.Redirect("../Landing.aspx");
                }
            }
            catch
            {
                Response.Redirect("../Landing.aspx");
            }
        }

        /*Calucate the Date*/
        public string CalDate(Telerik.Web.UI.RadDatePicker Dates)
        {
            string month = Dates.SelectedDate.Value.Month.ToString();
            if (month != "10" && month != "11" && month != "12")
                month = "0" + month;
            string day = Dates.SelectedDate.Value.Day.ToString();
            for (int i = 0; i < 10; i++)
            {
                if (Convert.ToInt32(day) == i)
                    day = "0" + day;
            }
            string year = Dates.SelectedDate.Value.Year.ToString();
            return day + '/' + month + '/' + year;
        }
    }
}