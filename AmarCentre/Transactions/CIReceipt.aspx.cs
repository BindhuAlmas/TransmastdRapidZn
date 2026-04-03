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
    public partial class CIReceipt : System.Web.UI.Page
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
                previlage_check();
                previlage_action_check();
                OnpageLoad();
                Clear();
                grid_fill(1, 10, "", "", "");
            }
        }

        //Get List 
        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_trans.Get_List_CustomerReceipt(page_number, page_size, filter, column, order, Convert.ToInt32(hdn_user_id.Value));
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
            DataTable dt = obj_trans.Get_List_CustomerReceipt_Excel(Convert.ToInt32(hdn_user_id.Value));
            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "Receipt");
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                HttpContext.Current.Response.Write(style);
                Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        public void OnpageLoad()
        {
            DataTable dt = obj_mas.Edit_GeneralSettings();
            hdnTaxAppliedWithDiscount.Value = dt.Rows[0]["TaxAppliedWithDiscount"].ToString();
        }

        /*rpt_list OnItemCommand*/
        protected void rpt_list_OnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdn_id");
            if (e.CommandName == "Edit")
            {
                Clear();
                pnl_add.Visible = true;

                DataSet ds = obj_trans.Edit_CustomerReceipt(Convert.ToInt32(hdn_rpt_id.Value), Convert.ToInt32(hdn_user_id.Value));
                DataTable dt1 = ds.Tables[0];/*invoic*/
                DataTable dt_ser = ds.Tables[1];/* Detail*/

                hdn_id.Value = dt1.Rows[0]["Id"].ToString();
                lbl_Code.Text = dt1.Rows[0]["Code"].ToString();
                txt_invCode.Text = dt1.Rows[0]["InvoiceCode"].ToString();
                hdn_invId.Value = dt1.Rows[0]["InvoiceId"].ToString();
                job_date.DbSelectedDate = dt1.Rows[0]["Dated"].ToString();
                txt_customerName.Text = dt1.Rows[0]["CustomerName"].ToString();
                hdn_customerId.Value = dt1.Rows[0]["Customer_Id"].ToString();

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
                drpPettyCash.SelectedValue = dt1.Rows[0]["PettyCashId"].ToString();
                if (drp_payMode.SelectedValue == "2")
                    fillBankAccountEdit(Convert.ToInt32(dt1.Rows[0]["AccountId"].ToString()));
                drpBankAccount.SelectedValue = dt1.Rows[0]["AccountId"].ToString();
                onchangedrp_bank(null, null);
                cheque_date.DbSelectedDate = dt1.Rows[0]["ChequeDate"].ToString();
                txt_chqNumber.Text = dt1.Rows[0]["ChequeNumber"].ToString();
                txt_commsn.Text = dt1.Rows[0]["BankCommission"].ToString();

                rpt_Item_list.DataSource = dt_ser;
                rpt_Item_list.DataBind();

                btn_save.Visible = false;
                btn_save_print.Visible = false;
                btn_print.Visible = hdn_print.Value == "0" ? false : true;
                btnOpenCancel.Visible = false;
                btnOpenDelete.Visible = false;
                if (dt1.Rows[0]["DataStatus"].ToString() == "1" && dt1.Rows[0]["InvoiceStatus"].ToString() == "0")
                {
                    btnOpenCancel.Visible = hdn_cancel.Value == "0" ? false : true;
                    btnOpenDelete.Visible = hdn_delete.Value == "0" ? false : true;
                }
                Upd_Add_Panel.Update();
            }
            else if (e.CommandName == "Print")
            {
                string url = "";
                url = "../Reports/CashCustomerReceiptFormat1.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
            }
        }

        protected void rpt_list_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Button btnPrint = (Button)e.Item.FindControl("btnPrint");
                btnPrint.Visible = hdn_print.Value == "0" ? false : true;
            }

        }

        protected void txt_invCode_OnTextChanged(object sender, EventArgs e)
        {
            if (txt_invCode.Text != "")
            {
                DataSet ds = obj_trans.Get_CustomerInvoice(txt_invCode.Text, Convert.ToInt32(hdn_user_id.Value));
                DataTable dt1 = ds.Tables[0];/*invoic*/
                DataTable dt_ser = ds.Tables[1];/* Detail*/
                if (dt1.Rows.Count > 0)
                {
                    hdn_invId.Value = dt1.Rows[0]["InvoiceId"].ToString();
                    txt_customerName.Text = dt1.Rows[0]["CustomerName"].ToString();
                    hdn_customerId.Value = dt1.Rows[0]["Customer_Id"].ToString();

                    txt_totDiscount.Text = dt1.Rows[0]["Total_Discount"].ToString();
                    txt_grand.Text = dt1.Rows[0]["AfterDiscount_GrandTotal"].ToString();
                    hdn_receivedAmt.Value = dt1.Rows[0]["Received"].ToString();
                    txt_pendingAmt.Text = dt1.Rows[0]["PendingAmount"].ToString();
                    txt_amtPayNow.Text = dt1.Rows[0]["PendingAmount"].ToString();
                    txt_Balance.Text = "";
                    txt_ReceivedAmt.Text = "";

                    rpt_Item_list.DataSource = dt_ser;
                    rpt_Item_list.DataBind();

                    if (Convert.ToDecimal(txt_pendingAmt.Text) == 0)
                        btn_save.Visible = btn_save_print.Visible = false;
                    else
                        btn_save.Visible = btn_save_print.Visible = true;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Invalid Invoice Code !');", true);

                    txt_invCode.Text = "";
                    hdn_invId.Value = "0";
                    txt_customerName.Text = "";
                    hdn_customerId.Value = "0";
                    txt_quotCode.Text = "";
                    hdn_quotId.Value = "";
                    hdnInvoiceType.Value = "";

                    btn_save.Visible = btn_save_print.Visible = true;

                    txt_totDiscount.Text = "";
                    txt_grand.Text = "";
                    hdn_receivedAmt.Value = "";
                    txt_pendingAmt.Text = "";
                    txt_ReceivedAmt.Text = "";
                    txt_Balance.Text = "";
                    txt_amtPayNow.Text = "";
                    rpt_Item_list.DataSource = null;
                    rpt_Item_list.DataBind();
                }
                drp_payMode.SelectedValue = "1";
                UpdDrpPaymentModePAnel.Update();
                drp_payMode_OnSelectedIndexChanged(null, null);
                Upd_Add_PanelInner.Update();
            }
            else
            {
                Clear();
            }
        }

        /*Save*/
        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            int res = SaveReceipt();
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
            //pnl_add.Visible = false;
            Upd_Add_PanelInner.Update();
        }

        public int SaveReceipt()
        {
            DataTable dt_deatils = fill_Detail();

            int res = 0;
            if (dt_deatils.Rows.Count > 0)
            {
                res = obj_trans.Insert_Update_CustomerReceipt(Convert.ToInt32(hdn_id.Value), DateTime.ParseExact(CalDate(job_date), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Convert.ToInt32(hdn_invId.Value), txt_remark.Text, txt_totDiscount.Text == "" ? (decimal?)null : Convert.ToDecimal(txt_totDiscount.Text),
                    Convert.ToDecimal(txt_grand.Text), Convert.ToDecimal(txt_amtPayNow.Text),
                    Convert.ToInt32(drp_payMode.SelectedValue), drpBankAccount.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpBankAccount.SelectedValue),
                    drpPettyCash.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpPettyCash.SelectedValue),
                    drp_payMode.SelectedValue == "3" ? DateTime.ParseExact(CalDate(job_date), "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null,
                    drp_payMode.SelectedValue == "3" ? txt_chqNumber.Text : "",
                    Convert.ToDecimal(txt_pendingAmt.Text), Convert.ToDecimal(txt_ReceivedAmt.Text), Convert.ToDecimal(txt_Balance.Text), dt_deatils,
                    Convert.ToInt32(hdn_user_id.Value), txt_commsn.Text == "" ? (decimal?)null : Convert.ToDecimal(txt_commsn.Text));
            }
            else
            {
                lbl_msgin.Text = "Add Service to Continue !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            return res;
        }

        /*Save & Print*/
        protected void btn_save_print_OnClick(object sender, EventArgs e)
        {
            int res = SaveReceipt();

            if (res > 0)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                lbl_msgin.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                string url = "";
                url = "../Reports/CashCustomerReceiptFormat1.aspx?id=" + res;
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
            }
            else
            {
                lbl_msgin.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            //pnl_add.Visible = false;
            Upd_Add_PanelInner.Update();
        }

        /*Print*/
        protected void btn_print_OnClick(object sender, EventArgs e)
        {
            string url = "";
            url = "../Reports/CashCustomerReceiptFormat1.aspx?id=" + Convert.ToInt32(hdn_id.Value);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);

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
                    drpPettyCash.DataSource = BalVoucher.GetPettyCashAccountList(Convert.ToInt32(hdn_user_id.Value));
                    drpPettyCash.DataValueField = "Value";
                    drpPettyCash.DataTextField = "Text";
                    drpPettyCash.DataBind();
                    drpPettyCash.Visible = true;
                    drpPettyCash.ClearSelection();
                    drpPettyCash.Text = "";
                    if (drpPettyCash.Items.Count == 1)
                        drpPettyCash.SelectedValue = drpPettyCash.Items[0].Value;

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
                    drpBankAccount.DataSource = BalVoucher.GetBankAccountList(Convert.ToInt32(hdn_user_id.Value));
                    drpBankAccount.DataValueField = "Value";
                    drpBankAccount.DataTextField = "Text";
                    drpBankAccount.DataBind();
                    drpBankAccount.Visible = true;
                    drpBankAccount.ClearSelection();
                    drpBankAccount.Text = "";

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
                if (dt.Rows[0]["IsCommssionApp"].ToString() == "1" & dt.Rows[0]["CommissionPer"].ToString() != "")
                    hdn_bankcommsn.Value = dt.Rows[0]["CommissionPer"].ToString();
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

        public void fillBankAccountEdit(int AccountId)
        {
            drpBankAccount.DataSource = BalVoucher.GetBankAccountListEdit(Convert.ToInt32(hdn_user_id.Value), AccountId);
            drpBankAccount.DataValueField = "Value";
            drpBankAccount.DataTextField = "Text";
            drpBankAccount.DataBind();
            drpBankAccount.Visible = true;
            drpBankAccount.ClearSelection();
            drpBankAccount.Text = "";
        }

        /*Data To Save*/
        public DataTable fill_Detail()
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
                    HiddenField hdn_D_id = (HiddenField)itm.FindControl("hdn_D_id");
                    HiddenField hdn_catgory_id = (HiddenField)itm.FindControl("hdn_catgory_id");
                    HiddenField hdn_service_id = (HiddenField)itm.FindControl("hdn_service_id");

                    TextBox txt_price = (TextBox)itm.FindControl("txt_price");
                    TextBox txt_discount = (TextBox)itm.FindControl("txt_discount");
                    TextBox txt_Qty = (TextBox)itm.FindControl("txt_Qty");
                    TextBox txt_taxamt = (TextBox)itm.FindControl("txt_tax");
                    TextBox txt_PriceWitTax = (TextBox)itm.FindControl("txt_priceWitTax");
                    TextBox txt_totPrice = (TextBox)itm.FindControl("txt_totPrice");

                    HiddenField hdn_expn = (HiddenField)itm.FindControl("hdn_expn");
                    HiddenField hdn_sc = (HiddenField)itm.FindControl("hdn_sc");

                    dt_ser.Rows.Add(Convert.ToInt32(hdn_D_id.Value), hdn_catgory_id.Value == "" ? (int?)null : Convert.ToInt32(hdn_catgory_id.Value), Convert.ToInt32(hdn_service_id.Value),
                        Convert.ToDecimal(txt_price.Text), Convert.ToDecimal(hdn_expn.Value),
                            Convert.ToDecimal(hdn_sc.Value), txt_discount.Text == "" ? (decimal?)null : Convert.ToDecimal(txt_discount.Text), Convert.ToDecimal(txt_Qty.Text), Convert.ToDecimal(txt_taxamt.Text),
                            Convert.ToDecimal(txt_PriceWitTax.Text), Convert.ToDecimal(txt_totPrice.Text));
                }
            }
            return dt_ser;
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            //foreach (RepeaterItem item in rpt_cancelList.Items)
            //{
            //    CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
            //    if (chkSelect.Checked == true)
            //    {
            //        HiddenField hdndetId = (HiddenField)item.FindControl("hdndetId");
            //        HiddenField hdn_type = (HiddenField)item.FindControl("hdn_type");

            //        if (hdn_type.Value == "1")
            //        {
            //            int ress = obj_trans.DeleteServiceCompletion(Convert.ToInt32(hdndetId.Value), Convert.ToInt32(hdn_user_id.Value));
            //        }
            //    }
            //}

            CancelDeleteReceipt(2);
        }

        protected void btnDelete_OnClick(object sender, EventArgs e)
        {
            //foreach (RepeaterItem item in rpt_cancelList.Items)
            //{
            //    CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
            //    if (chkSelect.Checked == true)
            //    {
            //        HiddenField hdndetId = (HiddenField)item.FindControl("hdndetId");
            //        HiddenField hdn_type = (HiddenField)item.FindControl("hdn_type");

            //        if (hdn_type.Value == "1")
            //        {
            //            int ress = obj_trans.DeleteServiceCompletion(Convert.ToInt32(hdndetId.Value), Convert.ToInt32(hdn_user_id.Value));
            //        }
            //    }
            //}

            CancelDeleteReceipt(3);
        }

        public void CancelDeleteReceipt(int Status)
        {
            int res = obj_trans.CustomerCancelDeleteReceipt(Convert.ToInt32(hdn_id.Value), Status, txtCancelRemark.Text, Convert.ToInt32(hdn_user_id.Value));
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
            Upd_Add_PanelInner.Update();
            txtCancelRemark.Text = "";
            pnlCancel.Visible = false;
            updCancel.Update();
        }

        protected void btnOpenDelete_OnClick(object sender, EventArgs e)
        {
            lblCancel.Text = "Delete Receipt";
            txtCancelRemark.Text = "";
            btnCancel.Visible = false;
            btnDelete.Visible = true;
            pnlCancel.Visible = true;

            //DataTable dt = obj_trans.GetReceiptCancelDetail(Convert.ToInt32(hdn_id.Value));
            //rpt_cancelList.DataSource = dt;
            //rpt_cancelList.DataBind();
            //div_candet.Visible = dt.Rows.Count > 0 ? true : false;

            updCancel.Update();
        }

        protected void btnOpenCancel_OnClick(object sender, EventArgs e)
        {
            lblCancel.Text = "Cancel Receipt";
            txtCancelRemark.Text = "";
            btnCancel.Visible = true;
            btnDelete.Visible = false;
            pnlCancel.Visible = true;

            //DataTable dt = obj_trans.GetReceiptCancelDetail(Convert.ToInt32(hdn_id.Value));
            //rpt_cancelList.DataSource = dt;
            //rpt_cancelList.DataBind();
            //div_candet.Visible = dt.Rows.Count > 0 ? true : false;

            updCancel.Update();
        }

        protected void btnCloseCancel_OnClick(object sender, EventArgs e)
        {
            txtCancelRemark.Text = "";
            pnlCancel.Visible = false;
            updCancel.Update();
        }

        /*Reset*/
        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
        }

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

        /*Clear All the Data*/
        public void Clear()
        {
            hdn_id.Value = "0";
            txt_invCode.Text = "";
            job_date.DbSelectedDate = DateTime.Now;
            hdn_invId.Value = "0";
            txt_customerName.Text = "";
            hdn_customerId.Value = "0";
            txt_quotCode.Text = "";
            hdn_quotId.Value = "";
            hdnInvoiceType.Value = "";
            hdn_bankcommsn.Value = "0";
            txt_commsn.Text = "";

            txt_totDiscount.Text = "";
            txt_grand.Text = "";
            hdn_receivedAmt.Value = "";
            txt_pendingAmt.Text = "";
            txt_remark.Text = "";
            txt_amtPayNow.Text = "";
            txt_ReceivedAmt.Text = "";
            txt_Balance.Text = "";
            drp_payMode.ClearSelection();
            drp_payMode.Text = "";
            drp_payMode.SelectedValue = "1";
            drp_payMode_OnSelectedIndexChanged(null, null);
            drpBankAccount.ClearSelection();
            drpBankAccount.Text = "";
            cheque_date.DbSelectedDate = "";
            txt_chqNumber.Text = "";

            rpt_Item_list.DataSource = null;
            rpt_Item_list.DataBind();


            btn_save.Visible = hdn_add.Value == "0" ? false : true;
            btn_save_print.Visible = hdn_add_N_print.Value == "0" ? false : true;
            btn_print.Visible = false;
            btnOpenCancel.Visible = false;
            btnOpenDelete.Visible = false;
            Get_Code();

            Upd_Add_PanelInner.Update();
        }

        /*Code for Display*/
        public void Get_Code()
        {
            DataTable dt = obj_common.Get_Code(69);
            if (dt.Rows.Count > 0)
                lbl_Code.Text = dt.Rows[0][0].ToString();
        }

        /*Check Action Privilege*/
        public void previlage_action_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    DataTable dt = obj_common.Action_Previlage_Validation(69, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdn_delete.Value = dt.Rows[1][1].ToString();
                        hdn_print.Value = dt.Rows[2][1].ToString();
                        hdn_add_N_print.Value = dt.Rows[3][1].ToString();
                        hdn_cancel.Value = dt.Rows[4][1].ToString();
                    }
                    btn_save.Visible = hdn_add.Value == "0" ? false : true;
                    btn_save_print.Visible = hdn_add_N_print.Value == "0" ? false : true;
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

                    int val = obj_common.Form_Previlage_Validation(69, Convert.ToInt32(hdn_user_id.Value));
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