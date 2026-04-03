using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using AmarCentre.BAL;
using System.Globalization;
using Telerik.Web.UI;
using iTextSharp.text.pdf;
using System.Drawing.Printing;
using System.Drawing.Text;

namespace AmarCentre.Transactions
{
    public partial class Receipt : System.Web.UI.Page
    {
        Transaction_Bal obj_trans = new Transaction_Bal();
        Master_Bal obj_mas = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
        Report_Bal obj_report = new Report_Bal();
        Voucher BalVoucher = new Voucher();
        public int ReceiptIdpub ;

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
                fill_Customer();
            }
        }

        public void fill_Customer()
        {
            drp_customer.Items.Clear();
            DataTable dt = obj_trans.Drp_Customer();
            drp_customer.DataSource = dt;
            drp_customer.DataTextField = "Text";
            drp_customer.DataValueField = "Value";
            drp_customer.DataBind();
        }

        protected void drp_customer_OnSelectedIndexChanged(Object sender, EventArgs e)
        {
            DataTable dt = obj_trans.DrpPendingInvoice(drp_customer.SelectedValue==""?0:Convert.ToInt32(drp_customer.SelectedValue));
            drpInvoice.DataSource = dt;
            drpInvoice.DataTextField = "Code";
            drpInvoice.DataValueField = "Id";
            drpInvoice.DataBind();
            drpInvoice.ClearSelection();
            drpInvoice.Text = "";

            hdnAdvance.Value = "0"; lbladvance.Text = "";

            DataSet ds = obj_mas.Edit_Customer(drp_customer.SelectedValue == "" ? 0 : Convert.ToInt32(drp_customer.SelectedValue));
            if(ds.Tables[0].Rows.Count>0)
            {
               hdnAdvance.Value= ds.Tables[0].Rows[0]["TotalPayable"].ToString();
                lbladvance.Text ="Advance : "+ ds.Tables[0].Rows[0]["TotalPayable"].ToString();
            }
            updadvance.Update();
            updinvoiceDrp.Update();
        }

        protected void drpInvoiceOnSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (drpInvoice.SelectedValue != "")
            {
                txt_invCode.Text = drpInvoice.SelectedItem.Text;
                updinvoice.Update();
                txt_invCode_OnTextChanged(null,null);
            }
            else
                Clear();
        }

        //Get List 
        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_trans.List_Receipt(page_number, page_size, filter, column, order, Convert.ToInt32(hdn_user_id.Value),
                 drprecStatus.SelectedValue == "" ? 0 :Convert.ToInt32(drprecStatus.SelectedValue));
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
            DataTable dt = obj_trans.Get_List_Receipt_Excel(Convert.ToInt32(hdn_user_id.Value));
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
            hdnIsDisableRoundOff.Value = dt.Rows[0]["IsDisableRoundOff"].ToString();
        }

        /*rpt_list OnItemCommand*/
        protected void rpt_list_OnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdn_id");
            if (e.CommandName == "Edit")
            {
                Clear();
                pnl_add.Visible = true;
                
                DataSet ds = obj_trans.Edit_Receipt(Convert.ToInt32(hdn_rpt_id.Value),Convert.ToInt32(hdn_user_id.Value));
                DataTable dt1 = ds.Tables[0];/*invoic*/
                DataTable dt_ser = ds.Tables[1];/* Detail*/

                hdn_id.Value = dt1.Rows[0]["Id"].ToString();
                lbl_Code.Text = dt1.Rows[0]["Code"].ToString();
                txt_invCode.Text = dt1.Rows[0]["InvoiceCode"].ToString();
                hdn_invId.Value = dt1.Rows[0]["InvoiceId"].ToString();
                job_date.DbSelectedDate = dt1.Rows[0]["Dated"].ToString();
                txt_customerName.Text = dt1.Rows[0]["CustomerName"].ToString();
                hdn_customerId.Value = dt1.Rows[0]["Customer_Id"].ToString();
                hdnAdvance.Value = dt1.Rows[0]["CPayable"].ToString();
                lbladvance.Text = "Advance : " + ds.Tables[0].Rows[0]["CPayable"].ToString();
                txt_quotCode.Text = dt1.Rows[0]["QuotationCode"].ToString();
                hdn_quotId.Value = dt1.Rows[0]["Quotation_id"].ToString();
                hdnInvoiceType.Value = dt1.Rows[0]["InvoiceType"].ToString();

                drp_customer.SelectedValue = dt1.Rows[0]["Customer_Id"].ToString();
                RadComboBoxItem CodeItem = new RadComboBoxItem();
                CodeItem.Text = dt1.Rows[0]["InvoiceCode"].ToString();
                CodeItem.Value = dt1.Rows[0]["InvoiceId"].ToString();
                drpInvoice.Items.Insert(0, CodeItem);
                drpInvoice.SelectedValue = dt1.Rows[0]["InvoiceId"].ToString();

                drpInvoice.Enabled = drp_customer.Enabled= false;
                txt_invCode.ReadOnly = true;

                txt_totDiscount.Text = dt1.Rows[0]["Total_Discount"].ToString();
                txt_grand.Text = dt1.Rows[0]["AfterDiscount_GrandTotal"].ToString();
                txt_remark.Text = dt1.Rows[0]["Remarks"].ToString();
                hdn_receivedAmt.Value = dt1.Rows[0]["Received"].ToString();
                txt_pendingAmt.Text = dt1.Rows[0]["PendingAmount"].ToString();
                txtspotCommission.Text = dt1.Rows[0]["SpotCommission"].ToString();

                txt_amtPayNow.Text = dt1.Rows[0]["Amount"].ToString();
                txt_ReceivedAmt.Text = dt1.Rows[0]["ReceivedAmount"].ToString();
                txt_Balance.Text = dt1.Rows[0]["Balance"].ToString();
                drp_payMode.SelectedValue = dt1.Rows[0]["PaymentModeId"].ToString();
                drp_payMode_OnSelectedIndexChanged(null, null);
                drpPettyCash.SelectedValue = dt1.Rows[0]["PettyCashId"].ToString();
                if (drp_payMode.SelectedValue == "2" || drp_payMode.SelectedValue == "6" || drp_payMode.SelectedValue == "10")
                    fillBankAccountEdit(Convert.ToInt32(dt1.Rows[0]["AccountId"].ToString()));
                drpBankAccount.SelectedValue = dt1.Rows[0]["AccountId"].ToString();
                drpLoan.SelectedValue = dt1.Rows[0]["LoanId"].ToString();

                txtRecChargedAmt.Text = dt1.Rows[0]["ChargedAmountReceipt"].ToString();
                onchangedrp_bank(null, null);
                cheque_date.DbSelectedDate = dt1.Rows[0]["ChequeDate"].ToString();
                txt_chqNumber.Text = dt1.Rows[0]["ChequeNumber"].ToString();
                txt_commsn.Text = dt1.Rows[0]["BankCommission"].ToString();
                txtCommissionVat.Text = dt1.Rows[0]["CommissionVat"].ToString();

                if (dt1.Rows[0]["PaymentType"].ToString() == "2")
                {
                    drp_payMode.Enabled = false;
                    trChargedAmount.Visible = true;
                    txtChargedAmount.Text = dt1.Rows[0]["ChargedAmount"].ToString();
                    hdnpaymenttype.Value = "2";
                    txt_amtPayNow.Text = (Convert.ToDecimal(dt1.Rows[0]["Amount"].ToString()) + Convert.ToDecimal(dt1.Rows[0]["ChargedAmount"].ToString())).ToString();
                    txt_amtPayNow.ReadOnly = true;
                }

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
                    //btnOpenDelete.Visible = hdn_delete.Value == "0" ? false : true;
                    btn_save.Visible = hdnupdate.Value == "0" ? false : true;
                    btn_save_print.Visible = hdnupdateNPrint.Value == "0" ? false : true;
                }

                if (dt1.Rows[0]["IsAllowEdit"].ToString() == "0")
                    btn_save.Visible = btn_save_print.Visible =  false;
                if (dt1.Rows[0]["IsEnable"].ToString() == "0")
                {
                    lblenablemsg.Text = "Update & Cancel not Allowed. Check invoice history for details. ";
                    btnOpenCancel.Visible = btn_save.Visible = btn_save_print.Visible = false;
                }

                Upd_Add_Panel.Update();
            }
            else if (e.CommandName == "Print")
            {
                ReceiptPrint(Convert.ToInt32(hdn_rpt_id.Value));
            }
            else if (e.CommandName == "Sendmail")
            {
                EmailUC.UCPageLoad(3, Convert.ToInt32(hdn_rpt_id.Value));
                pnlMail.Visible = true;
                UpdMailPanel.Update();
            }
        }

        protected void rpt_list_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Button btnPrint = (Button)e.Item.FindControl("btnPrint");
            btnPrint.Visible = hdn_print.Value == "0" ? false : true;
            Button btnSendmail = (Button)e.Item.FindControl("btnSendmail");
            btnSendmail.Visible = hdnsendmail.Value == "0" ? false : true;
        }

        protected void txt_invCode_OnTextChanged(object sender, EventArgs e)
        {
            if (txt_invCode.Text != "")
            {
                DataSet ds = obj_trans.Get_Invoice(txt_invCode.Text, Convert.ToInt32(hdn_user_id.Value));
                DataTable dt1 = ds.Tables[0];/*invoic*/
                DataTable dt_ser = ds.Tables[1];/* Detail*/

                drp_payMode.SelectedValue = "1";
                drp_payMode.Enabled = true;
                txtChargedAmount.Text = hdnpaymenttype.Value = "";
                trChargedAmount.Visible = false;
                txt_amtPayNow.ReadOnly = false;

                if (dt1.Rows.Count > 0)
                {
                    hdn_invId.Value = dt1.Rows[0]["InvoiceId"].ToString();
                    txt_customerName.Text = dt1.Rows[0]["CustomerName"].ToString();
                    hdn_customerId.Value = dt1.Rows[0]["Customer_Id"].ToString();
                    hdnAdvance.Value = dt1.Rows[0]["CPayable"].ToString();
                    lbladvance.Text = "Advance : " + ds.Tables[0].Rows[0]["CPayable"].ToString();

                    txt_quotCode.Text = dt1.Rows[0]["QuotationCode"].ToString();
                    hdn_quotId.Value = dt1.Rows[0]["Quotation_id"].ToString();
                    hdnInvoiceType.Value = dt1.Rows[0]["InvoiceType"].ToString();

                    txt_totDiscount.Text = dt1.Rows[0]["Total_Discount"].ToString();
                    txt_grand.Text = dt1.Rows[0]["AfterDiscount_GrandTotal"].ToString();
                    hdn_receivedAmt.Value = dt1.Rows[0]["Received"].ToString();
                    txt_pendingAmt.Text = dt1.Rows[0]["PendingAmount"].ToString();
                    txt_amtPayNow.Text = dt1.Rows[0]["PendingAmount"].ToString();
                    txt_Balance.Text = "";
                    txt_ReceivedAmt.Text = "";

                    rpt_Item_list.DataSource = dt_ser;
                    rpt_Item_list.DataBind();

                    if (dt1.Rows[0]["PaymentType"].ToString() == "2")
                    {
                        drp_payMode.SelectedValue = "2";
                        drp_payMode.Enabled = false;
                        trChargedAmount.Visible = true;
                        txtChargedAmount.Text= dt1.Rows[0]["ChargedAmount"].ToString();
                        hdnpaymenttype.Value = "2";
                        txt_amtPayNow.Text =(Convert.ToDecimal(dt1.Rows[0]["PendingAmount"].ToString())+ Convert.ToDecimal(dt1.Rows[0]["ChargedAmount"].ToString())).ToString() ;
                        txt_amtPayNow.ReadOnly = true;
                    }

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
                    hdn_customerId.Value = hdnAdvance.Value = "0";
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
                lblerrormsg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
            }
            Upd_Add_PanelInner.Update();
        }

        public int SaveReceipt()
        {
            DataTable dt_deatils = fill_Detail();

            int res = 0;
            if (dt_deatils.Rows.Count > 0)
            {
                if (Convert.ToDecimal(hdnAdvance.Value) < Convert.ToDecimal(txt_amtPayNow.Text) && drp_payMode.SelectedValue == "4")
                {
                    string msg = "Payment amount " + txt_amtPayNow.Text + " cannot be greater than advance balance " + hdnAdvance.Value;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('"+ msg + "');", true);

                }
               else if (drp_payMode.SelectedValue == "1" && drpPettyCash.SelectedValue == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Select cash account.!');", true);
                }
                else if ((drp_payMode.SelectedValue == "2" || drp_payMode.SelectedValue == "6" || drp_payMode.SelectedValue == "10") 
                    && drpBankAccount.SelectedValue == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Select bank account.!');", true);
                }
                else if (drp_payMode.SelectedValue == "5" && drpLoan.SelectedValue == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Select Loan account.!');", true);
                }
                else
                {
                    decimal paynow = Convert.ToDecimal(txt_amtPayNow.Text);
                    decimal SpotCommission = txtspotCommission.Text == "" ? 0 : Convert.ToDecimal(txtspotCommission.Text);

                    if (txtChargedAmount.Text != "" && Convert.ToDecimal(txtChargedAmount.Text)>0 && hdnpaymenttype.Value == "2")
                    {
                        paynow = Convert.ToDecimal(txt_pendingAmt.Text) - SpotCommission;
                    }

                    res = obj_trans.Insert_Update_Receipt(Convert.ToInt32(hdn_id.Value), DateTime.ParseExact(CalDate(job_date), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Convert.ToInt32(hdn_invId.Value), txt_remark.Text, txt_totDiscount.Text == "" ? (decimal?)null : Convert.ToDecimal(txt_totDiscount.Text),
                        Convert.ToDecimal(txt_grand.Text), paynow,
                        //Convert.ToDecimal(txt_amtPayNow.Text),
                        Convert.ToInt32(drp_payMode.SelectedValue), drpBankAccount.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpBankAccount.SelectedValue),
                        drpPettyCash.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpPettyCash.SelectedValue),
                        drp_payMode.SelectedValue == "3" ? cheque_date.SelectedDate : (DateTime?)null,
                        drp_payMode.SelectedValue == "3" ? txt_chqNumber.Text : "", Convert.ToDecimal(txt_pendingAmt.Text), paynow,
                        //Convert.ToDecimal(txt_amtPayNow.Text),
                       //Convert.ToDecimal(txt_ReceivedAmt.Text), Convert.ToDecimal(txt_Balance.Text)
                       0, dt_deatils, Convert.ToInt32(hdn_user_id.Value), txt_commsn.Text == "" ? (decimal?)null : Convert.ToDecimal(txt_commsn.Text),
                       (txtChargedAmount.Text == "" ? 0 : Convert.ToDecimal(txtChargedAmount.Text)), 
                       (txtRecChargedAmt.Text==""?(decimal?)null:Convert.ToDecimal(txtRecChargedAmt.Text) ),
                       drpLoan.SelectedValue==""?(int?)null:Convert.ToInt32(drpLoan.SelectedValue), SpotCommission,
                        txtCommissionVat.Text == "" ?0 : Convert.ToDecimal(txtCommissionVat.Text)
                       );
                }
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
                ReceiptPrint(res);
            }
            else
            {
                lblerrormsg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
            }
            Upd_Add_PanelInner.Update();
        }

        void ReceiptPrint(int RId)
        {
            DataTable dt = obj_mas.Edit_GeneralSettings();
            string url = "";

            if (dt.Rows[0]["ReceiptFormat"].ToString() == "1")
                url = "../Reports/CashReceiptFormat1.aspx?id=" + RId;
            else if (dt.Rows[0]["ReceiptFormat"].ToString() == "2")
                url = "../Reports/CashReceiptPOS.aspx?id=" + RId;
            else if (dt.Rows[0]["ReceiptFormat"].ToString() == "3")
                url = "../Reports/CashReceiptFormat2.aspx?id=" + RId;
            else if (dt.Rows[0]["ReceiptFormat"].ToString() == "4")
                url = "../Reports/CashReceiptFormat3.aspx?id=" + RId;
            else if (dt.Rows[0]["ReceiptFormat"].ToString() == "5")
                url = "../Reports/CashReceiptFormat5.aspx?id=" + RId;
            else if (dt.Rows[0]["ReceiptFormat"].ToString() == "6")
                url = "../Reports/CashReceiptFormat6.aspx?id=" + RId;

            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);

        }

        /*Print*/
        protected void btn_print_OnClick(object sender, EventArgs e)
        {
            ReceiptPrint(Convert.ToInt32(hdn_id.Value));
        }

        protected void drp_payMode_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            pnl_PayMode_Panel.Visible = pnl_Cheque_Panel.Visible = pnlRecChargedAmt.Visible=
                pnlCommissionVat.Visible = pnlchqno.Visible= false;
            cheque_date.DbSelectedDate = "";
            txt_chqNumber.Text = txtCommissionVat.Text= "";

            hdn_bankcommsn.Value = "0";
            txt_commsn.Text = "";
            txtRecChargedAmt.Text = "";

            drpBankAccount.Items.Clear();
            drpBankAccount.Visible = false;
            drpBankAccount.ClearSelection();
            drpBankAccount.Text = "";

            drpPettyCash.ClearSelection();
            drpPettyCash.Text = "";
            drpPettyCash.Items.Clear();
            drpPettyCash.Visible = false;

            drpLoan.ClearSelection();
            drpLoan.Text = "";
            drpLoan.Items.Clear();
            drpLoan.Visible = false;

            if (drp_payMode.SelectedValue == "1")/*PettyCash*/
            {
                drpPettyCash.DataSource = BalVoucher.GetPettyCashAccountList(Convert.ToInt32(hdn_user_id.Value));
                drpPettyCash.DataValueField = "Value";
                drpPettyCash.DataTextField = "Text";
                drpPettyCash.DataBind();
                drpPettyCash.Visible = true;
                if (drpPettyCash.Items.Count == 1)
                    drpPettyCash.SelectedValue = drpPettyCash.Items[0].Value;

                lblToLabel.Text = "Petty Cash Name / اسم المصروفات النثرية";
                lblToLabel.Visible = true;
                rqTo.ValidationGroup = "save";
                rqTo.ControlToValidate = "drpPettyCash";

                pnl_PayMode_Panel.Visible = true;
            }
            else if (drp_payMode.SelectedValue == "2" || drp_payMode.SelectedValue == "6")/*Bank Transfer   6- Card swipe*/
            {
                drpBankAccount.DataSource = BalVoucher.GetBankAccountList(Convert.ToInt32(hdn_user_id.Value));
                drpBankAccount.DataValueField = "Value";
                drpBankAccount.DataTextField = "Text";
                drpBankAccount.DataBind();
                drpBankAccount.Visible = true;

                lblToLabel.Text = "Bank Name / اسم البنك";
                lblToLabel.Visible = true;
                rqTo.ValidationGroup = "save";
                rqTo.ControlToValidate = "drpBankAccount";

                if (hdnpaymenttype.Value != "2" && drp_payMode.SelectedValue == "2")
                    pnlRecChargedAmt.Visible = true;
                if (drp_payMode.SelectedValue == "6")
                    pnlCommissionVat.Visible = true;

                pnl_PayMode_Panel.Visible = true;
            }
            else if (drp_payMode.SelectedValue == "10")/*nomad accont*/
            {
                drpBankAccount.DataSource = BalVoucher.GetNomadBankAccountList(Convert.ToInt32(hdn_user_id.Value));
                drpBankAccount.DataValueField = "Value";
                drpBankAccount.DataTextField = "Text";
                drpBankAccount.DataBind();
                drpBankAccount.Visible = true;

                lblToLabel.Text = "Bank Name / اسم البنك";
                lblToLabel.Visible = true;
                rqTo.ValidationGroup = "save";
                rqTo.ControlToValidate = "drpBankAccount";

                pnl_PayMode_Panel.Visible = true;
            }
            else if (drp_payMode.SelectedValue == "3")/*Cheque*/
            {
                lblToLabel.Text = "Bank Name / اسم البنك";
                lblToLabel.Visible = false;
                rqTo.ValidationGroup = "no";
                rqTo.ControlToValidate = "drpBankAccount";

                pnl_Cheque_Panel.Visible = pnlchqno.Visible= true;
            }
            else if (drp_payMode.SelectedValue == "4")/*Advance*/
            {
                lblToLabel.Text = "Bank Name / اسم البنك";
                lblToLabel.Visible = false;
                rqTo.ValidationGroup = "no";
                rqTo.ControlToValidate = "drpBankAccount";

            }
            else if (drp_payMode.SelectedValue == "5")/*Loan*/
            {
                drpLoan.DataSource = BalVoucher.GetLoan();
                drpLoan.DataValueField = "Value";
                drpLoan.DataTextField = "Text";
                drpLoan.DataBind();
                drpLoan.Visible = true;

                lblToLabel.Text = "Loan";
                lblToLabel.Visible = true;
                rqTo.ValidationGroup = "save";
                rqTo.ControlToValidate = "drpLoan";

                pnl_PayMode_Panel.Visible = true;
            }

            Upd_PayMode_Panel.Update();
            upd_commsn.Update();
            Upd_Cheque_Panel.Update();
        }

        protected void onchangedrp_bank(object sender, EventArgs e)
        {
            hdn_bankcommsn.Value = hdnisCommissionVat.Value= "0";

            if (drpBankAccount.SelectedValue != "" && drp_payMode.SelectedValue=="6")// only for card swipe
            {
                DataTable dt = obj_mas.Edit_Bank_Account(Convert.ToInt32(drpBankAccount.SelectedValue));
                hdnisCommissionVat.Value = dt.Rows[0]["IsVatApplicable"].ToString();
                if (dt.Rows[0]["IsCommssionApp"].ToString() == "1" & dt.Rows[0]["CommissionPer"].ToString()!="")
                    hdn_bankcommsn.Value = dt.Rows[0]["CommissionPer"].ToString();
            }
            Upd_PayMode_Panel.Update();
            CalCommission();
        }

        public void CalCommission()
        {
            txt_commsn.Text =txtCommissionVat.Text= "";
            decimal commsn = 0, vat = 0;
            if (txt_amtPayNow.Text != "" & hdn_bankcommsn.Value !="0")
            {
                commsn= (Convert.ToDecimal(txt_amtPayNow.Text) * (Convert.ToDecimal(hdn_bankcommsn.Value) / 100)) ;
                txt_commsn.Text = commsn.ToString("0.00");
            }
            vat = (commsn * Convert.ToDecimal(0.05));
            if (hdnisCommissionVat.Value =="1")
                txtCommissionVat.Text = vat.ToString("0.00");

            upd_commsn.Update();
            Upd_Cheque_Panel.Update();
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

                    dt_ser.Rows.Add(Convert.ToInt32(hdn_D_id.Value), hdn_catgory_id.Value==""?(int?)null: Convert.ToInt32(hdn_catgory_id.Value),Convert.ToInt32(hdn_service_id.Value),
                        Convert.ToDecimal(txt_price.Text), Convert.ToDecimal(hdn_expn.Value),
                            Convert.ToDecimal(hdn_sc.Value), txt_discount.Text==""?(decimal?)null:Convert.ToDecimal(txt_discount.Text), Convert.ToDecimal(txt_Qty.Text), Convert.ToDecimal(txt_taxamt.Text),
                            Convert.ToDecimal(txt_PriceWitTax.Text), Convert.ToDecimal(txt_totPrice.Text));
                }
            }
            return dt_ser;
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rpt_cancelList.Items)
            {
                CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    HiddenField hdndetId = (HiddenField)item.FindControl("hdndetId");
                    HiddenField hdn_type = (HiddenField)item.FindControl("hdn_type");

                    if (hdn_type.Value == "1")
                    {
                        int ress = obj_trans.DeleteServiceCompletion(Convert.ToInt32(hdndetId.Value), Convert.ToInt32(hdn_user_id.Value));
                    }
                }
            }

            CancelDeleteReceipt(2);
        }

        protected void btnDelete_OnClick(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rpt_cancelList.Items)
            {
                CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    HiddenField hdndetId = (HiddenField)item.FindControl("hdndetId");
                    HiddenField hdn_type = (HiddenField)item.FindControl("hdn_type");

                    if (hdn_type.Value == "1")
                    {
                        int ress = obj_trans.DeleteServiceCompletion(Convert.ToInt32(hdndetId.Value), Convert.ToInt32(hdn_user_id.Value));
                    }
                }
            }

            CancelDeleteReceipt(3);
        }

        public void CancelDeleteReceipt(int Status)
        {
            int res = obj_trans.CancelDeleteReceipt(Convert.ToInt32(hdn_id.Value), Status, txtCancelRemark.Text, Convert.ToInt32(hdn_user_id.Value));
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

            DataTable dt = obj_trans.GetReceiptCancelDetail(Convert.ToInt32(hdn_id.Value));
            rpt_cancelList.DataSource = dt;
            rpt_cancelList.DataBind();
            div_candet.Visible = dt.Rows.Count > 0 ? true : false;

            updCancel.Update();
        }

        protected void btnOpenCancel_OnClick(object sender, EventArgs e)
        {
            lblCancel.Text = "Cancel Receipt";
            txtCancelRemark.Text = "";
            btnCancel.Visible = true;
            btnDelete.Visible = false;
            pnlCancel.Visible = true;

            DataTable dt = obj_trans.GetReceiptCancelDetail(Convert.ToInt32(hdn_id.Value));
            rpt_cancelList.DataSource = dt;
            rpt_cancelList.DataBind();
            div_candet.Visible = dt.Rows.Count > 0 ? true : false;

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
            drp_customer.ClearSelection();
            drpInvoice.Items.Clear();
            drpInvoice.Text = "";
            drpInvoice.Enabled = drp_customer.Enabled = true;
            txt_invCode.ReadOnly = false;
             lbladvance.Text = lblenablemsg.Text= "";

            job_date.DbSelectedDate = DateTime.Now;
            hdn_invId.Value = "0";
            txt_customerName.Text =txtspotCommission.Text= "";
            hdn_customerId.Value = hdnAdvance.Value = "0";
            txt_quotCode.Text = "";
            hdn_quotId.Value = "";
            hdnInvoiceType.Value = "";
            hdn_bankcommsn.Value =hdnisCommissionVat.Value= "0";
            txt_commsn.Text = txtRecChargedAmt.Text= "";
            drp_payMode.Enabled = true;
            txtChargedAmount.Text = hdnpaymenttype.Value= "";
            trChargedAmount.Visible = pnlRecChargedAmt.Visible= false;
            txt_amtPayNow.ReadOnly = false;

            txt_totDiscount.Text =txtCommissionVat.Text= "";
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

            //DataTable dt_serD = new DataTable();
            //dt_serD.Columns.Add("D_id", typeof(int));
            //dt_serD.Columns.Add("CategoryId", typeof(int));
            //dt_serD.Columns.Add("Service_Id", typeof(int));
            //dt_serD.Columns.Add("ServiceName", typeof(string));
            //dt_serD.Columns.Add("Particulars", typeof(string));
            //dt_serD.Columns.Add("Price", typeof(decimal));
            //dt_serD.Columns.Add("Expense", typeof(decimal));
            //dt_serD.Columns.Add("ServiceCharge", typeof(decimal));
            //dt_serD.Columns.Add("Discount", typeof(decimal));
            //dt_serD.Columns.Add("Quantity", typeof(decimal));
            //dt_serD.Columns.Add("TaxAmount", typeof(decimal));
            //dt_serD.Columns.Add("Tax", typeof(decimal));
            //dt_serD.Columns.Add("PriceWitTax", typeof(decimal));
            //dt_serD.Columns.Add("Total", typeof(decimal));
            //dt_serD.Columns.Add("Fine", typeof(decimal));
            //dt_serD.Columns.Add("AdditionalServiceCharge", typeof(decimal));

            //dt_serD.Rows.Add(0, 0, 0, null, null, null, null, null, null, null, null, null, null,null,null,null);
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
            DataTable dt = obj_common.Get_Code(18);
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
                    DataTable dt = obj_common.Action_Previlage_Validation(18, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdnupdate.Value = dt.Rows[1][1].ToString();
                        hdn_delete.Value = dt.Rows[2][1].ToString();
                        hdn_print.Value = dt.Rows[3][1].ToString();
                        hdn_add_N_print.Value = dt.Rows[4][1].ToString();
                        hdnupdateNPrint.Value = dt.Rows[5][1].ToString();
                        hdn_cancel.Value = dt.Rows[6][1].ToString();
                        hdnsendmail.Value = dt.Rows[7][1].ToString();
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

                    int val = obj_common.Form_Previlage_Validation(18, Convert.ToInt32(hdn_user_id.Value));
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

        #region print

        public void InchPrint(int ReceiptId)
        {
            ReceiptIdpub = ReceiptId;
            PrinterSettings settings = new PrinterSettings();
            string printname = settings.PrinterName;

            DataSet ds = obj_report.CashReceiptPrint(ReceiptIdpub);
            DataTable dt = ds.Tables[0];
            DataTable dt_invD = ds.Tables[1];

            int servicelen = dt_invD.Rows.Count * 15;
            if (Application["PrintHeader"] != "")
            {
                servicelen = servicelen + 65;
            }
            try
            {
                PrintDocument doc = new PrintDocument();
                doc.PrinterSettings.PrinterName = printname;
                doc.DefaultPageSettings.PaperSize = new PaperSize("PaperA4", 300, 300 + servicelen);
                doc.DocumentName = Server.MapPath("~") + "CashReceiptPrint.pdf";
                doc.PrintPage += new PrintPageEventHandler(PrintHandler);
                doc.Print();
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('"+ex.Message+"');", true);

            }
        }
        private void PrintHandler(object sender, PrintPageEventArgs ppeArgs)
        {
            DataSet ds = obj_report.CashReceiptPrint(ReceiptIdpub);
            DataTable dt = ds.Tables[0];
            DataTable dt_invD = ds.Tables[1];

            int servicelen = dt_invD.Rows.Count * 15;
            float currentY = 10;
            int initlX = 8;

            if (Application["PrintHeader"] != "")
            {
                servicelen = servicelen + 65;
                string imageURL = Server.MapPath("../UploadedImage/" + Application["PrintHeader"]);
                System.Drawing.Bitmap image1 = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(imageURL, true);
                System.Drawing.Bitmap resized = new System.Drawing.Bitmap(image1, new System.Drawing.Size(280, 60));
                System.Drawing.TextureBrush texture = new System.Drawing.TextureBrush(resized);
                System.Drawing.Graphics formGraphics = ppeArgs.Graphics;
                formGraphics.FillRectangle(texture, new System.Drawing.RectangleF(10, 10, 270, 50));
                currentY = currentY + 60;
            }

            var foo = new PrivateFontCollection();
            foo.AddFontFile(HttpContext.Current.Server.MapPath("~/Font/arabtype.ttf"));

            System.Drawing.Font arbfnt = new System.Drawing.Font((System.Drawing.FontFamily)foo.Families[0], 8f);
            System.Drawing.Font arbsmallbold = new System.Drawing.Font((System.Drawing.FontFamily)foo.Families[0], 6f);
            System.Drawing.Font arbfntbld = new System.Drawing.Font((System.Drawing.FontFamily)foo.Families[0], 10f);

            System.Drawing.Font Fontboldhead = new System.Drawing.Font("Times New Roman", 9, System.Drawing.FontStyle.Bold);
            System.Drawing.Font FontNormal = new System.Drawing.Font("Times New Roman", 8, System.Drawing.FontStyle.Regular);
            System.Drawing.Font FontNormalBold = new System.Drawing.Font("Times New Roman", 8, System.Drawing.FontStyle.Bold);

            System.Drawing.Graphics g = ppeArgs.Graphics;

            g.DrawString("CASH RECEIPT", Fontboldhead, System.Drawing.Brushes.Black, 100, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 15;
            g.DrawString("الايصال", arbfntbld, System.Drawing.Brushes.Black, 125, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 25;

            System.IO.Stream mem = new MemoryStream();
            Barcode128 barImg = new Barcode128();
            barImg.Code = dt.Rows[0]["InvoiceCode"].ToString();
            barImg.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White).Save(mem, System.Drawing.Imaging.ImageFormat.Png);
            System.Drawing.Bitmap image1s = (System.Drawing.Bitmap)System.Drawing.Image.FromStream(mem, true);
            mem.Flush();
            mem.Close();
            System.Drawing.Bitmap resizeds = new System.Drawing.Bitmap(image1s, new System.Drawing.Size(100, 35));
            System.Drawing.TextureBrush textures = new System.Drawing.TextureBrush(resizeds);
            System.Drawing.Graphics formGraphicss = ppeArgs.Graphics;
            System.Drawing.PointF Loc = new System.Drawing.PointF(100, currentY);
            System.Drawing.SizeF SizeFc = new System.Drawing.SizeF(100, 35);
            formGraphicss.FillRectangle(textures, new System.Drawing.RectangleF(Loc, SizeFc));
            currentY = currentY + 45;

            g.DrawString(dt.Rows[0]["CustomerName"].ToString(), FontNormalBold, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
            g.DrawString(dt.Rows[0]["Date"].ToString(), FontNormalBold, System.Drawing.Brushes.Black, 240, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 15;
            g.DrawString("Invoice No / ", FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
            g.DrawString("رقم الفاتورة", arbfnt, System.Drawing.Brushes.Black, 70, currentY, new System.Drawing.StringFormat());
            g.DrawString(" : " + dt.Rows[0]["InvoiceCode"].ToString(), FontNormal, System.Drawing.Brushes.Black, 125, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 15;
            g.DrawString("Receipt No / ", FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
            g.DrawString("رقم الايصال", arbfnt, System.Drawing.Brushes.Black, 70, currentY, new System.Drawing.StringFormat());
            g.DrawString(" : " + dt.Rows[0]["Code"].ToString(), FontNormal, System.Drawing.Brushes.Black, 125, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 20;
            g.DrawString("Service / ", FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
            g.DrawString("الخدمات", arbfnt, System.Drawing.Brushes.Black, 55, currentY, new System.Drawing.StringFormat());
            //currentY = currentY + 15;
            //System.Drawing.Pen p1 = new System.Drawing.Pen(System.Drawing.Color.Black, 0.5f);
            //System.Drawing.Point point1 = new System.Drawing.Point(10, Convert.ToInt32(currentY));
            //System.Drawing.Point point2 = new System.Drawing.Point(290, Convert.ToInt32(currentY));
            //g.DrawLine(p1, point1, point2);

            foreach (DataRow r in dt_invD.Rows)
            {
                currentY = currentY + 15;

                g.DrawString(r["Name"].ToString(), FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
                if (r["NameInArabic"].ToString() != "")
                {
                    currentY = currentY + 10;
                    g.DrawString(r["NameInArabic"].ToString(), arbfnt, System.Drawing.Brushes.Black, 100, currentY, new System.Drawing.StringFormat());
                }
            }
            //currentY = currentY + 15;
            //point1 = new System.Drawing.Point(10, Convert.ToInt32(currentY));
            //point2 = new System.Drawing.Point(290, Convert.ToInt32(currentY));
            //g.DrawLine(p1, point1, point2);
            currentY = currentY + 15;
            g.DrawString("Net Amount / ", FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
            g.DrawString("المبلغ الصافي", arbfnt, System.Drawing.Brushes.Black, 75, currentY, new System.Drawing.StringFormat());
            g.DrawString(" : " + dt.Rows[0]["PendingAmount"].ToString(), FontNormal, System.Drawing.Brushes.Black, 135, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 15;
            g.DrawString("Paid Amount / ", FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
            g.DrawString("المبلغ المدفوع", arbfnt, System.Drawing.Brushes.Black, 75, currentY, new System.Drawing.StringFormat());
            g.DrawString(" : " + dt.Rows[0]["Amount"].ToString(), FontNormal, System.Drawing.Brushes.Black, 135, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 15;
            g.DrawString("Balance / ", FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
            g.DrawString("الرصيد", arbfnt, System.Drawing.Brushes.Black, 55, currentY, new System.Drawing.StringFormat());
            g.DrawString(" : " + dt.Rows[0]["Receivable"].ToString(), FontNormal, System.Drawing.Brushes.Black, 135, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 25;
            g.DrawString("* * *", FontNormal, System.Drawing.Brushes.Black, 140, currentY, new System.Drawing.StringFormat());

        }
        public static string ConvertNumbertoWords(Decimal Number_Value)
        {
            int number = Convert.ToInt32(Math.Floor(Number_Value));
            if (number == 0)
                return "Zero";
            if (number < 0)
                return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = "";
            if ((number / 1000000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000000) + " Million ";
                number %= 1000000;
            }
            if ((number / 100000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000) + " Lakhs ";
                number %= 100000;
            }
            if ((number / 1000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000) + " Thousand ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += ConvertNumbertoWords(number / 100) + " Hundred ";
                number %= 100;
            }
            if (number > 0)
            {
                if (words != "")
                    words += " ";
                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }

            number = (int)((Number_Value - (int)Number_Value) * 100);
            if (number > 0)
            {
                if (words != "")
                    words += " and ";
                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
                if (number < 20)
                {
                    words += unitsMap[number];
                    words += " Fills";
                }
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                    {
                        words += " " + unitsMap[number % 10];
                        words += " Fills";
                    }
                    else
                    {
                        words += " Fills";
                    }
                }
            }
            return words;
        }

        #endregion
    }
}