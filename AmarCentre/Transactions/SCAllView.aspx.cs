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
    public partial class SCAllView : System.Web.UI.Page
    {
        Transaction_Bal obj_trans = new Transaction_Bal();
        Master_Bal obj_master = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
        Report_Bal obj_report = new Report_Bal();

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
                OnPageLoad();
                Clear();
                //drpStatus.SelectedValue = "1";
                grid_fill(1, 10, "", "", "");
            }
        }

        public void OnPageLoad()
        {
            DataTable dtgen = obj_master.Edit_GeneralSettings();
            hdnSCPredateDays.Value = dtgen.Rows[0]["SCPredateDays"].ToString();
            hdnIsDisplaySCStatus.Value = dtgen.Rows[0]["IsDisplaySCStatus"].ToString();
            hdnallowSCExceed.Value = dtgen.Rows[0]["IsAllowSCAmountExceed"].ToString();
            SerComDate.MinDate = DateTime.Now.AddDays(Convert.ToInt32(dtgen.Rows[0]["SCPredateDays"]) * -1);
            SerComDateAdd.MinDate = DateTime.Now.AddDays(Convert.ToInt32(dtgen.Rows[0]["SCPredateDays"]) * -1);
            hdnIsHideServiceAmtInSC.Value = dtgen.Rows[0]["IsHideServiceAmtInSC"].ToString();

            if (hdnIsDisplaySCStatus.Value == "0")
            {
                thSCStatus.Attributes.Add("style", "display:none");
                drpserviceStatusfilter.Visible = false;
            }

            if (hdnIsHideServiceAmtInSC.Value == "1")
                thServAmt.Attributes.Add("style", "display:none");

            DataSet dsSC = obj_trans.DrpForSC();

            drpNewExpense.Items.Clear();
            DataTable dtExpense = dsSC.Tables[0];
            drpNewExpense.DataSource = dtExpense;
            drpNewExpense.DataValueField = "Value";
            drpNewExpense.DataTextField = "Text";
            drpNewExpense.DataBind();

            drpAddExpense.Items.Clear();
            drpAddExpense.DataSource = dtExpense;
            drpAddExpense.DataValueField = "Value";
            drpAddExpense.DataTextField = "Text";
            drpAddExpense.DataBind();

            drpNewVendor.Items.Clear();
            DataTable dtVendor = dsSC.Tables[1];
            drpNewVendor.DataSource = dtVendor;
            drpNewVendor.DataValueField = "Value";
            drpNewVendor.DataTextField = "Text";
            drpNewVendor.DataBind();

            drpAddVendor.Items.Clear();
            drpAddVendor.DataSource = dtVendor;
            drpAddVendor.DataValueField = "Value";
            drpAddVendor.DataTextField = "Text";
            drpAddVendor.DataBind();

            drpNewPayMode.Items.Clear();
            DataTable dtPayMode = dsSC.Tables[2];
            drpNewPayMode.DataSource = dtPayMode;
            drpNewPayMode.DataValueField = "Value";
            drpNewPayMode.DataTextField = "Text";
            drpNewPayMode.DataBind();
            drpNewPayMode.Items.Remove(drpNewPayMode.Items.FindItemByValue("2"));/*Remove Cheque*/

            drpAddPayMode.Items.Clear();
            drpAddPayMode.DataSource = dtPayMode;
            drpAddPayMode.DataValueField = "Value";
            drpAddPayMode.DataTextField = "Text";
            drpAddPayMode.DataBind();
            drpAddPayMode.Items.Remove(drpAddPayMode.Items.FindItemByValue("2"));/*Remove Cheque*/

            drpInvoiceCreator.Items.Clear();
            drpInvoiceCreator.DataSource = dsSC.Tables[3];
            drpInvoiceCreator.DataValueField = "Id";
            drpInvoiceCreator.DataTextField = "Name";
            drpInvoiceCreator.DataBind();

            drpchangestatus.Items.Clear();
            drpchangestatus.DataSource = dsSC.Tables[4];
            drpchangestatus.DataValueField = "Id";
            drpchangestatus.DataTextField = "Name";
            drpchangestatus.DataBind();

            drpserviceStatusfilter.Items.Clear();
            drpserviceStatusfilter.DataSource = dsSC.Tables[4];
            drpserviceStatusfilter.DataValueField = "Id";
            drpserviceStatusfilter.DataTextField = "Name";
            drpserviceStatusfilter.DataBind();
        }

        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_trans.GetAllServiceSC(page_number, page_size, filter,
                 drpStatus.SelectedValue == "" ? 2 : Convert.ToInt32(drpStatus.SelectedValue), Convert.ToInt32(hdn_user_id.Value),
                  drpInvoiceCreator.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpInvoiceCreator.SelectedValue),
                   drpserviceStatusfilter.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpserviceStatusfilter.SelectedValue));
            rpt_Item_list.DataSource = dt;
            rpt_Item_list.DataBind();

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
            DataTable dt = obj_trans.Get_List_Invoice_ForServiceCompletion_Excel(drpStatus.SelectedValue == "" ? 1 : Convert.ToInt32(drpStatus.SelectedValue));
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

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            txtNewVat.Text = txtNewVat.Text != "" ? txtNewVat.Text : "0.00";

            if (rpt_expense_list.Items.Count > 0 || (drpNewExpense.SelectedValue != "" && txtNewAmt.Text != "" && txtNewVat.Text != "" && drpNewVendor.SelectedValue != ""
                && drpNewPayMode.SelectedValue != "" && drpNewAccount.SelectedValue != "" && txtNewPaidAmount.Text != ""))
            {
                if (hdn_id.Value == "0")
                {
                    int NoOfRows = Convert.ToInt32(Convert.ToDecimal(txt_Qty.Text));
                    DataTable dt_trans = new DataTable();
                    dt_trans.Columns.Add("TransactionNumber", typeof(string));

                    for (int i = 0; i < NoOfRows; i++)
                    {
                        dt_trans.Rows.Add("");
                    }
                    rpt_TransacDetail.DataSource = dt_trans;
                    rpt_TransacDetail.DataBind();

                    rptfileupl.DataSource = null;
                    rptfileupl.DataBind();
                }
                pnl_transaDetail.Visible = true;
                Upd_TransaDetail_Panel.Update();
            }
            else
            {
                DataSet dt_deatils = fill_Detail();
                SaveServiceCompletion(dt_deatils);

                pnl_transaDetail.Visible = false;
                Upd_TransaDetail_Panel.Update();
                Upd_Add_PanelInner.Update();
            }
        }

        protected void callSAveCompletion(object sender, EventArgs e)
        {
            DataSet dt_deatils = fill_Detail();
            SaveServiceCompletion(dt_deatils);
            pnl_transaDetail.Visible = false;
            Upd_TransaDetail_Panel.Update();
            Upd_Add_PanelInner.Update();
        }

        public void SaveServiceCompletion(DataSet dt_deatils)
        {
            txtNewVat.Text = txtNewVat.Text != "" ? txtNewVat.Text : "0.00";

            DataTable dtSCfile = new DataTable();
            dtSCfile.Columns.Add("FileNames");
            dtSCfile.Columns.Add("FileSaveNames");

            foreach (RepeaterItem itm in rptfileupl.Items)
            {
                Label lblfileupl = (Label)itm.FindControl("lblfileupl");
                Label lblfilesaveupl = (Label)itm.FindControl("lblfilesaveupl");
                dtSCfile.Rows.Add(lblfileupl.Text, lblfilesaveupl.Text);
            }

            if ((txtNewAmt.Text != "" || txtNewVat.Text != "") && (drpNewExpense.SelectedValue == "" || drpNewVendor.SelectedValue == ""))
            {
                InlineCalculation();
                Upd_Expense_Panel.Update();
            }

            decimal ServAmount = Convert.ToDecimal(hdnSingleamount.Value) * Convert.ToDecimal(txt_Qty.Text);

            if (Convert.ToDecimal(txt_totAmt.Text) > ServAmount && Convert.ToInt32(hdnallowSCExceed.Value) == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('SC Amount cannot be greater than Service amount.!');", true);
            }
            else
            {
                int res = obj_trans.Insert_Update_ServiceCompletion(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_invId.Value),
                        Convert.ToInt32(hdn_InvDetailId.Value), Convert.ToDecimal(txt_Qty.Text),
                        Convert.ToDecimal(txt_amtSQty.Text), Convert.ToDecimal(txt_totAmt.Text), dt_deatils, SerComDate.SelectedDate,
                        Convert.ToInt32(hdn_user_id.Value), txtscremark.Text, dtSCfile);
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
            }
        }

        protected void btn_FinalSave_OnClick(object sender, EventArgs e)
        {
            DataSet dt_deatils = fill_Detail();
            int chk = 0;

            if (dt_deatils.Tables[0].Rows.Count > 0)
            {
                decimal expenseamt = Convert.ToDecimal(txt_totAmt.Text == "" ? "0" : txt_totAmt.Text);

                DataSet ds = obj_trans.Get_SerExpenseDetail_ServiceCompletion(Convert.ToInt32(hdn_InvDetailId.Value));
                DataTable dtinvd = ds.Tables[1];/*invoic*/

                decimal qty = Convert.ToDecimal(txt_Qty.Text);
                decimal serviceamt = (Convert.ToDecimal(dtinvd.Rows[0]["AfterDiscount_Total"]) / Convert.ToDecimal(dtinvd.Rows[0]["TQty"])) * qty;

                if (serviceamt < expenseamt)
                {
                    chk = 1;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ConfirmE()", true);
                }

                else
               if (dt_deatils.Tables[0].Rows[0]["PayModeId"].ToString() == "3")
                {
                    int bankId = Convert.ToInt32(dt_deatils.Tables[0].Rows[0]["AccountId"]);
                    DataTable dt = obj_master.Edit_Bank_Account(bankId);
                    if (Convert.ToDecimal(dt.Rows[0]["Balance"]) < expenseamt)
                    {
                        chk = 1;
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Confirm()", true);
                    }
                }
                if (chk == 0)
                {
                    SaveServiceCompletion(dt_deatils);
                    pnl_transaDetail.Visible = false;
                    Upd_TransaDetail_Panel.Update();
                    Upd_Add_PanelInner.Update();
                }
            }
            else
            {
                lbl_msg.Text = "Add Quantity to Continue !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);

                pnl_transaDetail.Visible = false;
                Upd_TransaDetail_Panel.Update();
                Upd_Add_PanelInner.Update();
            }
        }

        public DataSet fill_Detail()
        {
            txtNewVat.Text = txtNewVat.Text != "" ? txtNewVat.Text : "0.00";

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
            dt_exp.Columns.Add("VendorCommission", typeof(decimal));

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
                    TextBox txtVendorCommissionIn = (TextBox)expItem.FindControl("txtVendorCommissionIn");

                    dt_exp.Rows.Add(Convert.ToInt32(hdn_InvDetailId.Value), Convert.ToInt32(hdnSerComDetailId.Value), Convert.ToInt32(hdn_expenseId.Value),
                Convert.ToDecimal(txt_amt.Text), txt_vat.Text == "" ? 0 : Convert.ToDecimal(txt_vat.Text),
                Convert.ToInt32(drp_vendor.SelectedValue), Convert.ToInt32(drp_payMode.SelectedValue),
               drp_account.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_account.SelectedValue), Convert.ToDecimal(txt_payableAmount.Text),
                Convert.ToDecimal(txt_paidAmount.Text), txtVendorCommissionIn.Text == "" ? 0 : Convert.ToDecimal(txtVendorCommissionIn.Text));
                }
            }
            if (drpNewExpense.SelectedValue != "" && txtNewAmt.Text != "" &&  drpNewVendor.SelectedValue != ""
                && drpNewPayMode.SelectedValue != ""  )
            {
                dt_exp.Rows.Add(Convert.ToInt32(hdn_InvDetailId.Value), 0, Convert.ToInt32(drpNewExpense.SelectedValue),
                Convert.ToDecimal(txtNewAmt.Text), txtNewVat.Text == "" ? 0 : Convert.ToDecimal(txtNewVat.Text),
                Convert.ToInt32(drpNewVendor.SelectedValue), Convert.ToInt32(drpNewPayMode.SelectedValue),
              drpNewAccount.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpNewAccount.SelectedValue), 
              Convert.ToDecimal(txtNewPayableAmount.Text), txtNewPaidAmount.Text == "" ? 0 :
                Convert.ToDecimal(txtNewPaidAmount.Text), txtVendorCommissionOut.Text == "" ? 0 : Convert.ToDecimal(txtVendorCommissionOut.Text));
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

        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
        }

        protected void btn_expDetail_line_OnClick(object sender, EventArgs e)
        {
            txt_Qty.Enabled = true;
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;
            HiddenField hdn_D_id = (HiddenField)itemrp.FindControl("hdn_D_id");
            TextBox txt_InComQty = (TextBox)itemrp.FindControl("txt_InComQty");
            HiddenField hdnSingleamountIn = (HiddenField)itemrp.FindControl("hdnSingleamountIn");
            HiddenField hdnInvoiceId = (HiddenField)itemrp.FindControl("hdnInvoiceId");

            hdn_InComQty.Value = txt_InComQty.Text;
            hdn_InvDetailId.Value = hdn_D_id.Value;
            hdnSingleamount.Value = hdnSingleamountIn.Value;
            hdn_invId.Value = hdnInvoiceId.Value;

            txt_Qty.Text = "1";

            txtscremark.Text = "";
            SerComDate.DbSelectedDate = DateTime.Now;
            DataSet ds = obj_trans.Get_SerExpenseDetail_ServiceCompletion(Convert.ToInt32(hdn_D_id.Value));
            DataTable dt1 = ds.Tables[0];/*invoic*/
            rpt_expense_list.DataSource = dt1;
            rpt_expense_list.DataBind();

            txt_totAmt.Text = txt_amtSQty.Text = ds.Tables[2].Rows[0]["PayableAmount"].ToString();

            hdn_id.Value = "0";
            if (hdn_invStatus.Value != "2")
                btn_save.Visible = hdn_add.Value == "0" ? false : true;
            else
                btn_save.Visible = false;

            ClearExpenseDetail();
            pnl_Expense_Panel.Visible = true;
            Upd_Expense_Panel.Update();
        }

        protected void rpt_Item_listOnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RequiredFieldValidator RqtxtQty = (RequiredFieldValidator)e.Item.FindControl("RqtxtQty");
                int index = e.Item.ItemIndex;
                RqtxtQty.ValidationGroup = index.ToString();
                HiddenField hdn_invStatusIn = (HiddenField)e.Item.FindControl("hdn_invStatusIn");

                Button btnInlineSave = (Button)e.Item.FindControl("btnInlineSave");
                btnInlineSave.ValidationGroup = index.ToString();
                TextBox txt_InComQty = (TextBox)e.Item.FindControl("txt_InComQty");
                TextBox txtInlineQty = (TextBox)e.Item.FindControl("txtInlineQty");
                Button btn_expDetail_line = (Button)e.Item.FindControl("btn_expDetail_line");
                Button btnsetDescpy = (Button)e.Item.FindControl("btnsetDescpy");
                HiddenField hdnDescrepncy = (HiddenField)e.Item.FindControl("hdnDescrepncy");

                if (hdn_invStatusIn.Value != "2")
                    btnInlineSave.Visible = hdn_complete.Value == "0" ? false : true;
                else
                    btnsetDescpy.Visible = btn_expDetail_line.Visible = btnInlineSave.Visible = false;
               

                Label lblcomplete = (Label)e.Item.FindControl("lblcomplete");
                lblcomplete.Visible = false;
                if (txt_InComQty.Text == "0" || txt_InComQty.Text == "0.00")
                {
                    lblcomplete.Visible = true;
                    btnInlineSave.Visible = btn_expDetail_line.Visible = txtInlineQty.Visible = btnsetDescpy.Visible = false;
                }

                RadDatePicker InlineSerComDate = (RadDatePicker)e.Item.FindControl("InlineSerComDate");
                InlineSerComDate.DbSelectedDate = DateTime.Now;
                InlineSerComDate.MinDate = DateTime.Now.AddDays(Convert.ToInt32(hdnSCPredateDays.Value) * -1);

                HtmlTableRow trService = (HtmlTableRow)e.Item.FindControl("trService");
                trService.BgColor = hdnDescrepncy.Value == "0" ? "White" : "Yellow";

                Button btnserviceStatus = (Button)e.Item.FindControl("btnserviceStatus");
                HtmlTableCell tdSCStatus = (HtmlTableCell)e.Item.FindControl("tdSCStatus");
                HtmlTableCell tdServAmt = (HtmlTableCell)e.Item.FindControl("tdServAmt");

                if (hdnIsDisplaySCStatus.Value == "0")
                {
                    btnserviceStatus.Visible = false;
                    tdSCStatus.Attributes.Add("style", "display:none");
                }

                if (hdnIsHideServiceAmtInSC.Value == "1")
                    tdServAmt.Attributes.Add("style", "display:none");
            }
        }

        protected void btnInlineSave_OnClick(object sender, EventArgs e)
        {
            int DisplayMessage = 0;
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;
            HiddenField hdn_D_id = (HiddenField)itemrp.FindControl("hdn_D_id");
            TextBox txt_InComQty = (TextBox)itemrp.FindControl("txt_InComQty");
            TextBox txtInlineQty = (TextBox)itemrp.FindControl("txtInlineQty");
            TextBox txtInlineAmtSQty = (TextBox)itemrp.FindControl("txtInlineAmtSQty");
            TextBox txtInlineTotAmt = (TextBox)itemrp.FindControl("txtInlineTotAmt");
            RadDatePicker InlineSerComDate = (RadDatePicker)itemrp.FindControl("InlineSerComDate");
            HiddenField hdnSingleamountIn = (HiddenField)itemrp.FindControl("hdnSingleamountIn");
            HiddenField hdnInvoiceId = (HiddenField)itemrp.FindControl("hdnInvoiceId");

            hdn_InComQty.Value = txt_InComQty.Text;
            hdn_InvDetailId.Value = hdn_D_id.Value;
            hdnSingleamount.Value = hdnSingleamountIn.Value;
            hdn_invId.Value = hdnInvoiceId.Value;

            txt_Qty.Text = txtInlineQty.Text;
            txt_amtSQty.Text = txtInlineAmtSQty.Text;
            txt_totAmt.Text = txtInlineTotAmt.Text;
            SerComDate.DbSelectedDate = InlineSerComDate.SelectedDate;
            DataSet ds = obj_trans.Get_SerExpenseDetail_ServiceCompletion(Convert.ToInt32(hdn_D_id.Value));
            DataTable dt1 = ds.Tables[0];/*invoic*/
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
            dt_exp.Columns.Add("VendorCommission", typeof(decimal));

            if (dt1.Rows.Count > 0)
            {
                foreach (DataRow r in dt1.Rows)
                {
                    decimal PayableAmount = 0;
                    if (r["VendorId"].ToString() != "" && r["PayModeId"].ToString() != "" &&
                      (
                      (r["PayModeId"].ToString() == "7") || (r["PayModeId"].ToString() == "8") ||
                      (r["PayModeId"].ToString() == "9") ||
                      (r["PayModeId"].ToString() != "7" && r["PayModeId"].ToString() != "8" && r["PayModeId"].ToString() != "9" && r["AccountId"].ToString() != "")
                      )
                      )
                    {
                        PayableAmount = (Convert.ToDecimal(r["PayableAmount"].ToString())) * Convert.ToDecimal(txt_Qty.Text);

                        dt_exp.Rows.Add(0, Convert.ToInt32(r["ExpenseId"].ToString()), r["ExpenseName"].ToString(),
                            Convert.ToDecimal(r["Amount"].ToString()), Convert.ToDecimal(r["VAT"].ToString()),
                            Convert.ToInt32(r["VendorId"].ToString()), Convert.ToInt32(r["PayModeId"].ToString()),
                           r["AccountId"].ToString() == "" ? (int?)null : Convert.ToInt32(r["AccountId"].ToString()), PayableAmount,
                         (r["PayModeId"].ToString() == "7" || r["PayModeId"].ToString() == "9") ? 0 : (r["IsSetZeroPaidAmt"].ToString() == "1" ? 0 : PayableAmount),
                         Convert.ToDecimal(r["VendorCommission"].ToString()));
                    }
                    else
                    {
                        DisplayMessage = 1;
                        break;
                    }
                }

                //rpt_expense_list.DataSource = dt1;
                //rpt_expense_list.DataBind();

                //foreach (RepeaterItem expItem in rpt_expense_list.Items)
                //{
                //    HiddenField hdn_expenseId = (HiddenField)expItem.FindControl("hdn_expenseId");
                //    Label lbl_Expense = (Label)expItem.FindControl("lbl_Expense");
                //    TextBox txt_amt = (TextBox)expItem.FindControl("txt_amt");
                //    TextBox txt_vat = (TextBox)expItem.FindControl("txt_vat");
                //    RadComboBox drp_vendor = (RadComboBox)expItem.FindControl("drp_vendor");
                //    RadComboBox drp_payMode = (RadComboBox)expItem.FindControl("drp_payMode");
                //    RadComboBox drp_account = (RadComboBox)expItem.FindControl("drp_account");
                //    TextBox txt_payableAmount = (TextBox)expItem.FindControl("txt_payableAmount");
                //    TextBox txt_paidAmount = (TextBox)expItem.FindControl("txt_paidAmount");
                //    TextBox txtVendorCommissionIn = (TextBox)expItem.FindControl("txtVendorCommissionIn");

                //    decimal PayableAmount = 0;
                //    if (drp_vendor.SelectedValue != "" && drp_payMode.SelectedValue != "" && drp_account.SelectedValue != "")
                //    {
                //        PayableAmount = (Convert.ToDecimal(txt_amt.Text) + Convert.ToDecimal(txt_vat.Text)) * Convert.ToDecimal(txt_Qty.Text);
                //        dt_exp.Rows.Add(0, Convert.ToInt32(hdn_expenseId.Value), lbl_Expense.Text,
                //    Convert.ToDecimal(txt_amt.Text), Convert.ToDecimal(txt_vat.Text),
                //    Convert.ToInt32(drp_vendor.SelectedValue), Convert.ToInt32(drp_payMode.SelectedValue),
                //    Convert.ToInt32(drp_account.SelectedValue), PayableAmount, PayableAmount,
                //    txtVendorCommissionIn.Text == "" ? 0 : Convert.ToDecimal(txtVendorCommissionIn.Text));
                //    }
                //    else
                //    {
                //        DisplayMessage = 1;
                //        break;
                //    }
                //}
                if (DisplayMessage == 1)
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please Add Service Expense Detail');", true);
                else
                {
                    rpt_expense_list.DataSource = dt_exp;
                    rpt_expense_list.DataBind();
                    btn_save_OnClick(null, null);
                }

            }
            else
            {
                rpt_expense_list.DataSource = dt_exp;
                rpt_expense_list.DataBind();
                btn_save_OnClick(null, null);
            }
            Upd_Expense_Panel.Update();
        }

        protected void rpt_expense_list_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdn_vendorId = (HiddenField)e.Item.FindControl("hdn_vendorId");
                RadComboBox drp_vendor = (RadComboBox)e.Item.FindControl("drp_vendor");
                drp_vendor.Items.Clear();
                DataTable dtVendor = obj_master.Drp_Vendor();
                drp_vendor.DataSource = dtVendor;
                drp_vendor.DataValueField = "Value";
                drp_vendor.DataTextField = "Text";
                drp_vendor.DataBind();
                drp_vendor.SelectedValue = hdn_vendorId.Value;

                HiddenField hdn_payModeId = (HiddenField)e.Item.FindControl("hdn_payModeId");
                RadComboBox drp_payMode = (RadComboBox)e.Item.FindControl("drp_payMode");
                drp_payMode.Items.Clear();
                DataTable dtPayMode = obj_master.Drp_PaymentMode_WithoutCredit();
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

                RequiredFieldValidator rqdaccountIn = (RequiredFieldValidator)e.Item.FindControl("rqdaccountIn");
                TextBox txt_paidAmount = (TextBox)e.Item.FindControl("txt_paidAmount");

                if (drp_payMode.SelectedValue == "7" || drp_payMode.SelectedValue == "9")
                {
                    rqdaccountIn.Enabled = false;
                    txt_paidAmount.ReadOnly = true;
                    txt_paidAmount.Text = "0";
                }
                else if (drp_payMode.SelectedValue == "8")
                {
                    rqdaccountIn.Enabled = false;
                }

                HiddenField hdnSerComDetailId = (HiddenField)e.Item.FindControl("hdnSerComDetailId");
                Button btnInlineEdit = (Button)e.Item.FindControl("btnInlineEdit");
                Button btnInlineDelete = (Button)e.Item.FindControl("btnInlineDelete");
                btnInlineDelete.Visible = btnInlineEdit.Visible = Convert.ToInt32(hdnSerComDetailId.Value) >= 0 ? false : true;
            }

        }

        protected void drp_payMode_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            RadComboBox drp = (RadComboBox)sender;
            RepeaterItem itm = (RepeaterItem)drp.Parent;
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            RequiredFieldValidator rqdaccountIn = (RequiredFieldValidator)itm.FindControl("rqdaccountIn");
            HiddenField hdn_accountId = (HiddenField)itm.FindControl("hdn_accountId");
            RadComboBox drp_account = (RadComboBox)itm.FindControl("drp_account");
            UpdatePanel Upd_Account_Panel = (UpdatePanel)itm.FindControl("Upd_Account_Panel");
            TextBox txt_paidAmount = (TextBox)itm.FindControl("txt_paidAmount");
            UpdatePanel updpaidAmountIn = (UpdatePanel)itm.FindControl("updpaidAmountIn");

            rqdaccountIn.Enabled = true;
            txt_paidAmount.ReadOnly = false;
            drp_account.Items.Clear();
            if (drp.SelectedValue != "")
            {
                DataTable dtAccount = obj_trans.ListAccountInServCompletion(Convert.ToInt32(drp.SelectedValue), Convert.ToInt32(hdn_user_id.Value), 0);
                drp_account.DataSource = dtAccount;
                drp_account.DataValueField = "Value";
                drp_account.DataTextField = "Text";
                drp_account.DataBind();
            }
            if (drp.SelectedValue == "7" || drp.SelectedValue == "9")
            {
                rqdaccountIn.Enabled = false;
                txt_paidAmount.ReadOnly = true;
                txt_paidAmount.Text = "0";
            }
            else if (drp.SelectedValue == "8")
            {
                rqdaccountIn.Enabled = false;
            }
            updpaidAmountIn.Update();

            hdn_accountId.Value = "";
            drp_account.ClearSelection();
            drp_account.Text = "";
            Upd_Account_Panel.Update();
        }

        protected void drpNewPayMode_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            drpNewAccount.Items.Clear();
            if (drpNewPayMode.SelectedValue != "")
            {
                DataTable dtAccount = obj_trans.ListAccountInServCompletion(Convert.ToInt32(drpNewPayMode.SelectedValue), Convert.ToInt32(hdn_user_id.Value), 0);
                drpNewAccount.DataSource = dtAccount;
                drpNewAccount.DataValueField = "Value";
                drpNewAccount.DataTextField = "Text";
                drpNewAccount.DataBind();
            }
            drpNewAccount.ClearSelection();
            drpNewAccount.Text = "";

            if (drpNewPayMode.SelectedValue == "7" || drpNewPayMode.SelectedValue == "9")
            {
                rqdaccOut.Enabled = false;
                txtNewPaidAmount.ReadOnly = true;
                txtNewPaidAmount.Text = "0";
            }
            else if (drpNewPayMode.SelectedValue == "8")
            {
                rqdaccOut.Enabled = false;
            }
            updpaidAmountOut.Update();
            UpdNewAccountPanel.Update();
        }

        protected void btnInlineEdit_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            HiddenField hdnSerComDetailIdP = (HiddenField)itemrp.FindControl("hdnSerComDetailId");
            HiddenField hdn_expenseIdP = (HiddenField)itemrp.FindControl("hdn_expenseId");
            Label lbl_ExpenseP = (Label)itemrp.FindControl("lbl_Expense");
            TextBox txt_amtP = (TextBox)itemrp.FindControl("txt_amt");
            TextBox txt_vatP = (TextBox)itemrp.FindControl("txt_vat");
            RadComboBox drp_vendorP = (RadComboBox)itemrp.FindControl("drp_vendor");
            RadComboBox drp_payModeP = (RadComboBox)itemrp.FindControl("drp_payMode");
            RadComboBox drp_accountP = (RadComboBox)itemrp.FindControl("drp_account");
            TextBox txt_payableAmountP = (TextBox)itemrp.FindControl("txt_payableAmount");
            TextBox txt_paidAmountP = (TextBox)itemrp.FindControl("txt_paidAmount");
            TextBox txtVendorCommissionInP = (TextBox)itemrp.FindControl("txtVendorCommissionIn");

            ClearExpenseDetail();

            hdnNewIndexId.Value = hdnSerComDetailIdP.Value;
            drpNewExpense.SelectedValue = hdn_expenseIdP.Value;
            txtNewAmt.Text = txt_amtP.Text;
            txtNewVat.Text = txt_vatP.Text;
            drpNewVendor.SelectedValue = drp_vendorP.SelectedValue;
            drpNewPayMode.SelectedValue = drp_payModeP.SelectedValue;
            drpNewPayMode_OnSelectedIndexChanged(null, null);
            drpNewAccount.SelectedValue = drp_accountP.SelectedValue;
            txtNewPayableAmount.Text = txt_payableAmountP.Text;
            txtNewPaidAmount.Text = txt_paidAmountP.Text;
            txtVendorCommissionOut.Text = txtVendorCommissionInP.Text;

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
            dt_exp.Columns.Add("VendorCommission", typeof(decimal));

            decimal? decimalnull = null;
            int? intnull = null;
            if (rpt_expense_list.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_expense_list.Items)
                {
                    HiddenField hdnSerComDetailId = (HiddenField)itm.FindControl("hdnSerComDetailId");
                    HiddenField hdn_expenseId = (HiddenField)itm.FindControl("hdn_expenseId");
                    Label lbl_Expense = (Label)itm.FindControl("lbl_Expense");
                    TextBox txt_amt = (TextBox)itm.FindControl("txt_amt");
                    TextBox txt_vat = (TextBox)itm.FindControl("txt_vat");
                    RadComboBox drp_vendor = (RadComboBox)itm.FindControl("drp_vendor");
                    RadComboBox drp_payMode = (RadComboBox)itm.FindControl("drp_payMode");
                    RadComboBox drp_account = (RadComboBox)itm.FindControl("drp_account");
                    TextBox txt_payableAmount = (TextBox)itm.FindControl("txt_payableAmount");
                    TextBox txt_paidAmount = (TextBox)itm.FindControl("txt_paidAmount");
                    TextBox txtVendorCommissionIn = (TextBox)itm.FindControl("txtVendorCommissionIn");

                    dt_exp.Rows.Add(Convert.ToInt32(hdnSerComDetailId.Value), Convert.ToInt32(hdn_expenseId.Value),
                    lbl_Expense.Text, txt_amt.Text == "" ? decimalnull : Convert.ToDecimal(txt_amt.Text), txt_vat.Text == "" ? decimalnull : Convert.ToDecimal(txt_vat.Text),
                    drp_vendor.SelectedValue == "" ? intnull : Convert.ToInt32(drp_vendor.SelectedValue), drp_payMode.SelectedValue == "" ? intnull : Convert.ToInt32(drp_payMode.SelectedValue),
                    drp_account.SelectedValue == "" ? intnull : Convert.ToInt32(drp_account.SelectedValue), txt_payableAmount.Text == "" ? decimalnull : Convert.ToDecimal(txt_payableAmount.Text),
                    txt_paidAmount.Text == "" ? decimalnull : Convert.ToDecimal(txt_paidAmount.Text),
                    txtVendorCommissionIn.Text == "" ? 0 : Convert.ToDecimal(txtVendorCommissionIn.Text));

                }
            }

            dt_exp.Rows.RemoveAt(itemrp.ItemIndex);
            rpt_expense_list.DataSource = dt_exp;
            rpt_expense_list.DataBind();

            Upd_Expense_Panel.Update();
        }

        protected void btnInlineDelete_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

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
            dt_exp.Columns.Add("VendorCommission", typeof(decimal));

            decimal? decimalnull = null;
            int? intnull = null;
            if (rpt_expense_list.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_expense_list.Items)
                {
                    HiddenField hdnSerComDetailId = (HiddenField)itm.FindControl("hdnSerComDetailId");
                    HiddenField hdn_expenseId = (HiddenField)itm.FindControl("hdn_expenseId");
                    Label lbl_Expense = (Label)itm.FindControl("lbl_Expense");
                    TextBox txt_amt = (TextBox)itm.FindControl("txt_amt");
                    TextBox txt_vat = (TextBox)itm.FindControl("txt_vat");
                    RadComboBox drp_vendor = (RadComboBox)itm.FindControl("drp_vendor");
                    RadComboBox drp_payMode = (RadComboBox)itm.FindControl("drp_payMode");
                    RadComboBox drp_account = (RadComboBox)itm.FindControl("drp_account");
                    TextBox txt_payableAmount = (TextBox)itm.FindControl("txt_payableAmount");
                    TextBox txt_paidAmount = (TextBox)itm.FindControl("txt_paidAmount");
                    TextBox txtVendorCommissionIn = (TextBox)itm.FindControl("txtVendorCommissionIn");

                    dt_exp.Rows.Add(Convert.ToInt32(hdnSerComDetailId.Value), Convert.ToInt32(hdn_expenseId.Value),
                    lbl_Expense.Text, txt_amt.Text == "" ? decimalnull : Convert.ToDecimal(txt_amt.Text), txt_vat.Text == "" ? decimalnull : Convert.ToDecimal(txt_vat.Text),
                    drp_vendor.SelectedValue == "" ? intnull : Convert.ToInt32(drp_vendor.SelectedValue), drp_payMode.SelectedValue == "" ? intnull : Convert.ToInt32(drp_payMode.SelectedValue),
                    drp_account.SelectedValue == "" ? intnull : Convert.ToInt32(drp_account.SelectedValue), txt_payableAmount.Text == "" ? decimalnull : Convert.ToDecimal(txt_payableAmount.Text),
                    txt_paidAmount.Text == "" ? decimalnull : Convert.ToDecimal(txt_paidAmount.Text),
                    txtVendorCommissionIn.Text == "" ? 0 : Convert.ToDecimal(txtVendorCommissionIn.Text));
                }
            }

            dt_exp.Rows.RemoveAt(itemrp.ItemIndex);
            rpt_expense_list.DataSource = dt_exp;
            rpt_expense_list.DataBind();

            InlineCalculation();
            Upd_Expense_Panel.Update();
        }

        protected void btnInlineNew_OnClick(object sender, EventArgs e)
        {
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
            dt_exp.Columns.Add("VendorCommission", typeof(decimal));

            decimal? decimalnull = null;
            int? intnull = null;
            if (rpt_expense_list.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_expense_list.Items)
                {
                    HiddenField hdnSerComDetailId = (HiddenField)itm.FindControl("hdnSerComDetailId");
                    HiddenField hdn_expenseId = (HiddenField)itm.FindControl("hdn_expenseId");
                    Label lbl_Expense = (Label)itm.FindControl("lbl_Expense");
                    TextBox txt_amt = (TextBox)itm.FindControl("txt_amt");
                    TextBox txt_vat = (TextBox)itm.FindControl("txt_vat");
                    RadComboBox drp_vendor = (RadComboBox)itm.FindControl("drp_vendor");
                    RadComboBox drp_payMode = (RadComboBox)itm.FindControl("drp_payMode");
                    RadComboBox drp_account = (RadComboBox)itm.FindControl("drp_account");
                    TextBox txt_payableAmount = (TextBox)itm.FindControl("txt_payableAmount");
                    TextBox txt_paidAmount = (TextBox)itm.FindControl("txt_paidAmount");
                    TextBox txtVendorCommissionIn = (TextBox)itm.FindControl("txtVendorCommissionIn");

                    dt_exp.Rows.Add(Convert.ToInt32(hdnSerComDetailId.Value), Convert.ToInt32(hdn_expenseId.Value),
                    lbl_Expense.Text, txt_amt.Text == "" ? decimalnull : Convert.ToDecimal(txt_amt.Text), txt_vat.Text == "" ? decimalnull : Convert.ToDecimal(txt_vat.Text),
                    drp_vendor.SelectedValue == "" ? intnull : Convert.ToInt32(drp_vendor.SelectedValue), drp_payMode.SelectedValue == "" ? intnull : Convert.ToInt32(drp_payMode.SelectedValue),
                    drp_account.SelectedValue == "" ? intnull : Convert.ToInt32(drp_account.SelectedValue), txt_payableAmount.Text == "" ? decimalnull : Convert.ToDecimal(txt_payableAmount.Text),
                    txt_paidAmount.Text == "" ? decimalnull : Convert.ToDecimal(txt_paidAmount.Text),
                     txtVendorCommissionIn.Text == "" ? 0 : Convert.ToDecimal(txtVendorCommissionIn.Text));
                }
            }
            dt_exp.Rows.Add(Convert.ToInt32(hdnNewIndexId.Value), Convert.ToInt32(drpNewExpense.SelectedValue), drpNewExpense.Text,
                Convert.ToDecimal(txtNewAmt.Text), txtNewVat.Text == "" ? 0 : Convert.ToDecimal(txtNewVat.Text),
                Convert.ToInt32(drpNewVendor.SelectedValue), Convert.ToInt32(drpNewPayMode.SelectedValue),
              drpNewAccount.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpNewAccount.SelectedValue), Convert.ToDecimal(txtNewPayableAmount.Text),
                Convert.ToDecimal(txtNewPaidAmount.Text),
                 txtVendorCommissionOut.Text == "" ? 0 : Convert.ToDecimal(txtVendorCommissionOut.Text));


            rpt_expense_list.DataSource = dt_exp;
            rpt_expense_list.DataBind();
            ClearExpenseDetail();
            drpNewExpense.Focus();
            Upd_Expense_Panel.Update();
        }

        public void InlineCalculation()
        {
            decimal AmountForSingleQty = 0;
            decimal TotalAmount = 0;
            foreach (RepeaterItem itm in rpt_expense_list.Items)
            {
                TextBox txt_amt = (TextBox)itm.FindControl("txt_amt");
                TextBox txt_vat = (TextBox)itm.FindControl("txt_vat");
                AmountForSingleQty += (txt_amt.Text == "" ? 0 : Convert.ToDecimal(txt_amt.Text)) +
                                       (txt_vat.Text == "" ? 0 : Convert.ToDecimal(txt_vat.Text));
            }
            if (drpNewExpense.SelectedValue != "" && drpNewVendor.SelectedValue != "" && drpNewPayMode.SelectedValue != ""
                && drpNewAccount.SelectedValue != "")
                AmountForSingleQty += (txtNewAmt.Text == "" ? 0 : Convert.ToDecimal(txtNewAmt.Text)) +
                                       (txtNewVat.Text == "" ? 0 : Convert.ToDecimal(txtNewVat.Text));
            txt_amtSQty.Text = AmountForSingleQty.ToString("0.00");
            TotalAmount = (txt_Qty.Text == "" ? 1 : Convert.ToDecimal(txt_Qty.Text)) * AmountForSingleQty;
            txt_totAmt.Text = TotalAmount.ToString("0.00");
        }

        public void ClearExpenseDetail()
        {
            hdnNewIndexId.Value = "-" + (rpt_expense_list.Items.Count + 1).ToString();
            drpNewExpense.ClearSelection();
            drpNewExpense.Text = "";
            txtNewAmt.Text = "";
            txtNewVat.Text = "";
            drpNewVendor.ClearSelection();
            drpNewVendor.Text = "";
            drpNewPayMode.ClearSelection();
            drpNewPayMode.Text = "";
            drpNewPayMode_OnSelectedIndexChanged(null, null);
            txtNewPayableAmount.Text = "";
            txtNewPaidAmount.Text = txtVendorCommissionOut.Text = "";
        }

        protected void btnServiceCompletionView_OnClick(object sender, EventArgs e)
        {
            Clear();
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;
            HiddenField hdn_D_id = (HiddenField)itemrp.FindControl("hdn_D_id");
            DataSet ds = obj_trans.GetServiceCompletionView(Convert.ToInt32(hdn_D_id.Value));

            rptServiceCompletionEdit.DataSource = ds.Tables[0];
            rptServiceCompletionEdit.DataBind();

            rptfiledown.DataSource = ds.Tables[1];
            rptfiledown.DataBind();
            pnlfiledwn.Visible = ds.Tables[1].Rows.Count > 0 ? true : false;

            rptDescHis.DataSource = ds.Tables[2];
            rptDescHis.DataBind();
            pnlDescHistory.Visible = ds.Tables[2].Rows.Count > 0 ? true : false;

            pnlServiceCompletionView.Visible = true;
            UpdServiceCompletionView.Update();
        }

        protected void rptfileOnItemCommand(object s, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Download")
            {
                HiddenField hdnfilesave = (HiddenField)e.Item.FindControl("hdnfilesave");
                Label lblfile = (Label)e.Item.FindControl("lblfile");

                try
                {
                    if (hdnfilesave.Value != "")
                    {
                        string fil_name = hdnfilesave.Value;
                        string full_name = Server.MapPath("~/UploadedFiles/" + fil_name);
                        Response.ContentType = "APPLICATION/OCTET-STREAM";
                        String Header = "Attachment; Filename=\"" + lblfile.Text + "\"";
                        Response.AppendHeader("Content-Disposition", Header);
                        System.IO.FileInfo Dfile = new System.IO.FileInfo(Server.MapPath("~/UploadedFiles/" + fil_name));
                        Response.WriteFile(Dfile.FullName);
                        //Don't forget to add the following line
                        Response.End();
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        protected void btnViewClose_OnClick(object sender, EventArgs e)
        {
            pnlServiceCompletionView.Visible = false;
            UpdServiceCompletionView.Update();
        }

        protected void btnServiceCompletionEdit_OnClick(object sender, EventArgs e)
        {
            Clear();
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;
            HiddenField hdnSerCompletionId = (HiddenField)itemrp.FindControl("hdnSerCompletionId");
            DataSet ds = obj_trans.EditServiceCompletion(Convert.ToInt32(hdnSerCompletionId.Value));
            DataTable dtSerCom = ds.Tables[0];
            DataTable dtSerComDetail = ds.Tables[1];
            DataTable dtSerComTransDetail = ds.Tables[2];
            hdn_id.Value = dtSerCom.Rows[0]["Id"].ToString();
            hdn_InvDetailId.Value = dtSerCom.Rows[0]["InvDetailId"].ToString();
            hdnSingleamount.Value = dtSerCom.Rows[0]["AfterDiscount_PriceWitTax"].ToString();
            hdn_invId.Value = dtSerCom.Rows[0]["InvId"].ToString();

            hdn_InComQty.Value = "0";/*We are not gonna use this bcz Qty blur wont work*/
            txt_Qty.Text = dtSerCom.Rows[0]["Quantity"].ToString();
            txt_Qty.Enabled = false;
            txt_Qty.CssClass = txt_Qty.CssClass + " Qty";
            txt_amtSQty.Text = dtSerCom.Rows[0]["AmtForSingleQty"].ToString();
            txt_totAmt.Text = dtSerCom.Rows[0]["TotalAmount"].ToString();
            SerComDate.DbSelectedDate = dtSerCom.Rows[0]["Created_Date"].ToString();
            txtscremark.Text = dtSerCom.Rows[0]["SCRemark"].ToString();

            rpt_expense_list.DataSource = dtSerComDetail;
            rpt_expense_list.DataBind();
            ClearExpenseDetail();

            rpt_TransacDetail.DataSource = dtSerComTransDetail;
            rpt_TransacDetail.DataBind();

            if (hdn_invStatus.Value != "2" & dtSerCom.Rows[0]["IsAllowEdit"].ToString() == "1")
                btn_save.Visible = hdn_update.Value == "0" ? false : true;
            else
                btn_save.Visible = false;

            pnl_Expense_Panel.Visible = true;
            Upd_Add_PanelInner.Update();

            pnlServiceCompletionView.Visible = false;
            UpdServiceCompletionView.Update();
        }

        protected void rptServiceCompletionEdit_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Button btnServiceCompletionDelete = (Button)e.Item.FindControl("btnServiceCompletionDelete");
                btnServiceCompletionDelete.Visible = hdn_delete.Value == "0" ? false : true;
            }

        }

        protected void btnServiceCompletionDelete_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            HiddenField hdnSerCompletionId = (HiddenField)itemrp.FindControl("hdnSerCompletionId");

            int res = obj_trans.DeleteServiceCompletion(Convert.ToInt32(hdnSerCompletionId.Value), Convert.ToInt32(hdn_user_id.Value));
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

            pnlServiceCompletionView.Visible = false;
            UpdServiceCompletionView.Update();
        }

        public void fu_SCFilesOnFileUploaded(object sender, FileUploadedEventArgs e)
        {
            fu_SCFiles.TargetFolder = "~/UploadedFiles";

            foreach (UploadedFile upfile in fu_SCFiles.UploadedFiles)
            {
                DataTable dtprefix = obj_common.Get_File_Code("SCFile");
                string files_namesave = dtprefix.Rows[0][0].ToString() + upfile.FileName;
                upfile.SaveAs(Path.Combine(Server.MapPath(fu_SCFiles.TargetFolder), files_namesave));

                try
                {
                    //in backup folder also
                    DataTable dtgen = obj_master.Edit_GeneralSettings();
                    File.Copy((Path.Combine(Server.MapPath(fu_SCFiles.TargetFolder), files_namesave)),
                        (Path.Combine(dtgen.Rows[0]["BackupDrivePath"].ToString() + "UploadedFiles", files_namesave)), false);
                }
                catch (Exception cc) { }

                hdnfilename.Value = upfile.FileName;
                hdnfilenamesave.Value = files_namesave;
            }

            Updfu_SCFiles.Update();
        }

        protected void btnadddEdit_Click(object sender, EventArgs e)
        {
            DataTable dtSCfile = new DataTable();
            dtSCfile.Columns.Add("FileNames");
            dtSCfile.Columns.Add("FileSaveNames");

            foreach (RepeaterItem itm in rptfileupl.Items)
            {
                Label lblfileupl = (Label)itm.FindControl("lblfileupl");
                Label lblfilesaveupl = (Label)itm.FindControl("lblfilesaveupl");
                dtSCfile.Rows.Add(lblfileupl.Text, lblfilesaveupl.Text);
            }

            if (hdnfilename.Value != "" && hdnfilenamesave.Value != "")
                dtSCfile.Rows.Add(hdnfilename.Value, hdnfilenamesave.Value);

            rptfileupl.DataSource = dtSCfile;
            rptfileupl.DataBind();

            hdnfilename.Value = hdnfilenamesave.Value = "";

            Updfu_SCFiles.Update();
        }


        #region Changestatus

        protected void btnChangestatusOnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            HiddenField hdn_InvD_id = (HiddenField)itemrp.FindControl("hdn_D_id");

            //TextBox txtInlineQty = (TextBox)itemrp.FindControl("txtInlineQty");
            //if (txtInlineQty.Text.Trim() != "" && Convert.ToDecimal(txtInlineQty.Text) != 0)
            //{
            hdnInvoiceDetId.Value = hdn_InvD_id.Value;
            drpchangestatus.ClearSelection();
            drpchangestatus.Text = "";
            btnChangestatussave.Visible = hdnServiceStatus.Value == "0" ? false : true;

            pnlServiceStatus.Visible = true;
            updServiceStatus.Update();
            UpdInvoiceDetId.Update();
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Enter Qty to proceed.!');", true);
            //}
        }

        protected void btnChangestatussaveOnClick(object sender, EventArgs e)
        {
            int res = obj_trans.UpdateServiceStatus(Convert.ToInt32(hdnInvoiceDetId.Value), Convert.ToInt32(drpchangestatus.SelectedValue), Convert.ToInt32(hdn_user_id.Value));
            if (res > 0)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);

                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lblerrormsg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
            }
            updmsg.Update();
            pnlServiceStatus.Visible = false;
            updServiceStatus.Update();
        }

        protected void btnChangestatuscloseOnClick(object sender, EventArgs e)
        {
            pnlServiceStatus.Visible = false;
            updServiceStatus.Update();
        }

        #endregion

        #region SetDiscrepancy

        protected void btnsetDescpyOnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            HiddenField hdn_InvD_id = (HiddenField)itemrp.FindControl("hdn_D_id");

            TextBox txtInlineQty = (TextBox)itemrp.FindControl("txtInlineQty");
            if (txtInlineQty.Text.Trim() != "" && Convert.ToDecimal(txtInlineQty.Text) != 0)
            {
                hdnInvoiceDetId.Value = hdn_InvD_id.Value;
                txtDiscrepancyremark.Text = "";
                lbldes.Text = "Set Descrepancy";
                btnclrDiscrepancysave.Visible = false;
                btnDiscrepancysave.Visible = hdnsetdescrepancy.Value == "0" ? false : true;

                pnlDiscrepancy.Visible = true;
                UpdDiscrepancy.Update();
                UpdInvoiceDetId.Update();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Enter Qty to proceed.!');", true);
            }
        }

        protected void btnDiscrepancysaveOnClick(object sender, EventArgs e)
        {
            int res = obj_trans.UpdateDescrepancy(Convert.ToInt32(hdnInvoiceDetId.Value), 1, txtDiscrepancyremark.Text, Convert.ToInt32(hdn_user_id.Value));
            if (res > 0)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);

                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lblerrormsg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
            }
            updmsg.Update();
            pnlDiscrepancy.Visible = false;
            UpdDiscrepancy.Update();
        }

        protected void btnDiscrepancycloseOnClick(object sender, EventArgs e)
        {
            pnlDiscrepancy.Visible = false;
            UpdDiscrepancy.Update();
        }

        #endregion

        #region ClearDiscrepancy

        protected void btnClearDescpyOnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            HiddenField hdn_InvD_id = (HiddenField)itemrp.FindControl("hdn_D_id");

            TextBox txtInlineQty = (TextBox)itemrp.FindControl("txtInlineQty");
            if (txtInlineQty.Text.Trim() != "" && Convert.ToDecimal(txtInlineQty.Text) != 0)
            {
                hdnInvoiceDetId.Value = hdn_InvD_id.Value;
                txtDiscrepancyremark.Text = "";
                lbldes.Text = "Clear Descrepancy";
                btnclrDiscrepancysave.Visible = hdncleardescrepancy.Value == "0" ? false : true;
                btnDiscrepancysave.Visible = false;

                pnlDiscrepancy.Visible = true;
                UpdDiscrepancy.Update();
                UpdInvoiceDetId.Update();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Enter Qty to proceed.!');", true);
            }
        }

        protected void btnClearDiscrepancysaveOnClick(object sender, EventArgs e)
        {
            int res = obj_trans.UpdateDescrepancy(Convert.ToInt32(hdnInvoiceDetId.Value), 0, txtDiscrepancyremark.Text, Convert.ToInt32(hdn_user_id.Value));
            if (res > 0)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);

                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lblerrormsg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
            }
            updmsg.Update();
            pnlDiscrepancy.Visible = false;
            UpdDiscrepancy.Update();
        }

        #endregion


        public void ClearExpenseDetailAddtnl()
        {
            hdnAddIndexId.Value = "-" + (rptAddtnlExpense.Items.Count + 1).ToString();
            drpAddExpense.ClearSelection();
            drpAddExpense.Text = "";
            txtAddAmt.Text = "";
            txtAddVat.Text = "";
            drpAddVendor.ClearSelection();
            drpAddVendor.Text = "";
            drpAddPayMode.ClearSelection();
            drpAddPayMode.Text = "";
            drpAddPayMode_OnSelectedIndexChanged(null, null);
            txtAddPayableAmount.Text = "";
            txtAddPaidAmount.Text = "";
            txtAddTotal.Text = "";
            rptAddtnlExpense.DataSource = null;
            rptAddtnlExpense.DataBind();
            SerComDateAdd.DbSelectedDate = null;
        }

        protected void drp_payModeAE_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            RadComboBox drp = (RadComboBox)sender;
            RepeaterItem itm = (RepeaterItem)drp.Parent;
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            RequiredFieldValidator rqdaccountIn = (RequiredFieldValidator)itm.FindControl("rqdaccountInAE");
            HiddenField hdn_accountId = (HiddenField)itm.FindControl("hdn_accountId_AE");
            RadComboBox drp_account = (RadComboBox)itm.FindControl("drp_account_AE");
            UpdatePanel Upd_Account_Panel = (UpdatePanel)itm.FindControl("Upd_Account_PanelAE");
            TextBox txt_paidAmount = (TextBox)itm.FindControl("txt_paidAmount_AE");
            UpdatePanel updpaidAmountIn = (UpdatePanel)itm.FindControl("updpaidAmountInAE");

            rqdaccountIn.Enabled = true;
            txt_paidAmount.ReadOnly = false;
            drp_account.Items.Clear();
            if (drp.SelectedValue != "")
            {
                DataTable dtAccount = obj_trans.ListAccountInServCompletion(Convert.ToInt32(drp.SelectedValue), Convert.ToInt32(hdn_user_id.Value), 0);
                drp_account.DataSource = dtAccount;
                drp_account.DataValueField = "Value";
                drp_account.DataTextField = "Text";
                drp_account.DataBind();
            }
            if (drp.SelectedValue == "7" || drp.SelectedValue == "9")
            {
                rqdaccountIn.Enabled = false;
                txt_paidAmount.ReadOnly = true;
                txt_paidAmount.Text = "0";
            }
            else if (drp.SelectedValue == "8")
            {
                rqdaccountIn.Enabled = false;
            }
            updpaidAmountIn.Update();

            hdn_accountId.Value = "";
            drp_account.ClearSelection();
            drp_account.Text = "";
            Upd_Account_Panel.Update();
        }

        protected void drpAddPayMode_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            drpAddAccount.Items.Clear();
            if (drpAddPayMode.SelectedValue != "")
            {
                DataTable dtAccount = obj_trans.ListAccountInServCompletion(Convert.ToInt32(drpAddPayMode.SelectedValue), Convert.ToInt32(hdn_user_id.Value), 0);
                drpAddAccount.DataSource = dtAccount;
                drpAddAccount.DataValueField = "Value";
                drpAddAccount.DataTextField = "Text";
                drpAddAccount.DataBind();
            }
            if (drpAddPayMode.SelectedValue == "7" || drpAddPayMode.SelectedValue == "9")
            {
                rqdaddacc.Enabled = false;
                txtAddPaidAmount.ReadOnly = true;
                txtAddPaidAmount.Text = "0";
            }
            else if (drpAddPayMode.SelectedValue == "8")
            {
                rqdaddacc.Enabled = false;
            }
            updAddPaidAmount.Update();

            drpAddAccount.ClearSelection();
            drpAddAccount.Text = "";
            UpdAddAccountPanel.Update();
        }

        public void InlineCalculationAddtional()
        {
            decimal AmountForSingleQty = 0;
            foreach (RepeaterItem itm in rptAddtnlExpense.Items)
            {
                TextBox txt_amt = (TextBox)itm.FindControl("txt_amt_AE");
                TextBox txt_vat = (TextBox)itm.FindControl("txt_vat_AE");
                AmountForSingleQty += (txt_amt.Text == "" ? 0 : Convert.ToDecimal(txt_amt.Text)) +
                                       (txt_vat.Text == "" ? 0 : Convert.ToDecimal(txt_vat.Text));
            }
            if (drpAddExpense.SelectedValue != "" && drpAddVendor.SelectedValue != "" && drpAddPayMode.SelectedValue != ""
                && drpAddAccount.SelectedValue != "")
                AmountForSingleQty += (txtAddAmt.Text == "" ? 0 : Convert.ToDecimal(txtAddAmt.Text)) +
                                       (txtAddVat.Text == "" ? 0 : Convert.ToDecimal(txtAddVat.Text));
            txtAddTotal.Text = AmountForSingleQty.ToString("0.00");
        }

        protected void btn_AddtnlExpenseinline_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;
            HiddenField hdn_D_id = (HiddenField)itemrp.FindControl("hdn_D_id");
            HiddenField hdninvoiceId = (HiddenField)itemrp.FindControl("hdninvoiceId");

            hdn_InvDetailIdAddtnl.Value = hdn_D_id.Value;
            hdn_invId.Value = hdninvoiceId.Value;

            ClearExpenseDetailAddtnl();
            pnlAddtnlExpense.Visible = true;
            updAddtnlExpense.Update();
        }

        protected void rptAddtnlExpenseOnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdn_vendorId = (HiddenField)e.Item.FindControl("hdn_vendorId_AE");
                RadComboBox drp_vendor = (RadComboBox)e.Item.FindControl("drp_vendor_AE");
                drp_vendor.Items.Clear();
                DataTable dtVendor = obj_master.Drp_Vendor();
                drp_vendor.DataSource = dtVendor;
                drp_vendor.DataValueField = "Value";
                drp_vendor.DataTextField = "Text";
                drp_vendor.DataBind();
                drp_vendor.SelectedValue = hdn_vendorId.Value;

                HiddenField hdn_payModeId = (HiddenField)e.Item.FindControl("hdn_payModeId_AE");
                RadComboBox drp_payMode = (RadComboBox)e.Item.FindControl("drp_payMode_AE");
                drp_payMode.Items.Clear();
                DataTable dtPayMode = obj_master.Drp_PaymentMode_WithoutCredit();
                drp_payMode.DataSource = dtPayMode;
                drp_payMode.DataValueField = "Value";
                drp_payMode.DataTextField = "Text";
                drp_payMode.DataBind();
                drp_payMode.SelectedValue = hdn_payModeId.Value;
                drp_payMode.Items.Remove(drp_payMode.Items.FindItemByValue("2"));/*Remove Cheque*/

                HiddenField hdn_accountId = (HiddenField)e.Item.FindControl("hdn_accountId_AE");
                RadComboBox drp_account = (RadComboBox)e.Item.FindControl("drp_account_AE");
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

                HiddenField hdnSerComDetailId = (HiddenField)e.Item.FindControl("hdnSerComDetailId_AE");
                Button btnInlineEdit = (Button)e.Item.FindControl("btnInlineEdit");
                Button btnInlineDelete = (Button)e.Item.FindControl("btnInlineDelete");
                btnInlineDelete.Visible = btnInlineEdit.Visible = Convert.ToInt32(hdnSerComDetailId.Value) >= 0 ? false : true;
            }

        }

        protected void btnInlineEditAddtnl_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            HiddenField hdnSerComDetailIdP = (HiddenField)itemrp.FindControl("hdnSerComDetailId_AE");
            HiddenField hdn_expenseIdP = (HiddenField)itemrp.FindControl("hdn_expenseId_AE");
            Label lbl_ExpenseP = (Label)itemrp.FindControl("lbl_Expense_AE");
            TextBox txt_amtP = (TextBox)itemrp.FindControl("txt_amt_AE");
            TextBox txt_vatP = (TextBox)itemrp.FindControl("txt_vat_AE");
            RadComboBox drp_vendorP = (RadComboBox)itemrp.FindControl("drp_vendor_AE");
            RadComboBox drp_payModeP = (RadComboBox)itemrp.FindControl("drp_payMode_AE");
            RadComboBox drp_accountP = (RadComboBox)itemrp.FindControl("drp_account_AE");
            TextBox txt_payableAmountP = (TextBox)itemrp.FindControl("txt_payableAmount_AE");
            TextBox txt_paidAmountP = (TextBox)itemrp.FindControl("txt_paidAmount_AE");
            ClearExpenseDetailAddtnl();

            hdnAddIndexId.Value = hdnSerComDetailIdP.Value;
            drpAddExpense.SelectedValue = hdn_expenseIdP.Value;
            txtAddAmt.Text = txt_amtP.Text;
            txtAddVat.Text = txt_vatP.Text;
            drpAddVendor.SelectedValue = drp_vendorP.SelectedValue;
            drpAddPayMode.SelectedValue = drp_payModeP.SelectedValue;
            drpAddPayMode_OnSelectedIndexChanged(null, null);
            drpAddAccount.SelectedValue = drp_accountP.SelectedValue;
            txtAddPayableAmount.Text = txt_payableAmountP.Text;
            txtAddPaidAmount.Text = txt_paidAmountP.Text;

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

            decimal? decimalnull = null;
            int? intnull = null;
            if (rptAddtnlExpense.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rptAddtnlExpense.Items)
                {
                    HiddenField hdnSerComDetailId = (HiddenField)itm.FindControl("hdnSerComDetailId_AE");
                    HiddenField hdn_expenseId = (HiddenField)itm.FindControl("hdn_expenseId_AE");
                    Label lbl_Expense = (Label)itm.FindControl("lbl_Expense_AE");
                    TextBox txt_amt = (TextBox)itm.FindControl("txt_amt_AE");
                    TextBox txt_vat = (TextBox)itm.FindControl("txt_vat_AE");
                    RadComboBox drp_vendor = (RadComboBox)itm.FindControl("drp_vendor_AE");
                    RadComboBox drp_payMode = (RadComboBox)itm.FindControl("drp_payMode_AE");
                    RadComboBox drp_account = (RadComboBox)itm.FindControl("drp_account_AE");
                    TextBox txt_payableAmount = (TextBox)itm.FindControl("txt_payableAmount_AE");
                    TextBox txt_paidAmount = (TextBox)itm.FindControl("txt_paidAmount_AE");

                    dt_exp.Rows.Add(Convert.ToInt32(hdnSerComDetailId.Value), Convert.ToInt32(hdn_expenseId.Value),
                    lbl_Expense.Text, txt_amt.Text == "" ? decimalnull : Convert.ToDecimal(txt_amt.Text), txt_vat.Text == "" ? 0 : Convert.ToDecimal(txt_vat.Text),
                    drp_vendor.SelectedValue == "" ? intnull : Convert.ToInt32(drp_vendor.SelectedValue), drp_payMode.SelectedValue == "" ? intnull : Convert.ToInt32(drp_payMode.SelectedValue),
                    drp_account.SelectedValue == "" ? intnull : Convert.ToInt32(drp_account.SelectedValue), txt_payableAmount.Text == "" ? decimalnull : Convert.ToDecimal(txt_payableAmount.Text),
                    txt_paidAmount.Text == "" ? decimalnull : Convert.ToDecimal(txt_paidAmount.Text));

                }
            }

            dt_exp.Rows.RemoveAt(itemrp.ItemIndex);
            rptAddtnlExpense.DataSource = dt_exp;
            rptAddtnlExpense.DataBind();

            updAddtnlExpense.Update();
        }

        protected void btnInlineDeleteAddtnl_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

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

            decimal? decimalnull = null;
            int? intnull = null;
            if (rptAddtnlExpense.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rptAddtnlExpense.Items)
                {
                    HiddenField hdnSerComDetailId = (HiddenField)itm.FindControl("hdnSerComDetailId_AE");
                    HiddenField hdn_expenseId = (HiddenField)itm.FindControl("hdn_expenseId_AE");
                    Label lbl_Expense = (Label)itm.FindControl("lbl_Expense_AE");
                    TextBox txt_amt = (TextBox)itm.FindControl("txt_amt_AE");
                    TextBox txt_vat = (TextBox)itm.FindControl("txt_vat_AE");
                    RadComboBox drp_vendor = (RadComboBox)itm.FindControl("drp_vendor_AE");
                    RadComboBox drp_payMode = (RadComboBox)itm.FindControl("drp_payMode_AE");
                    RadComboBox drp_account = (RadComboBox)itm.FindControl("drp_account_AE");
                    TextBox txt_payableAmount = (TextBox)itm.FindControl("txt_payableAmount_AE");
                    TextBox txt_paidAmount = (TextBox)itm.FindControl("txt_paidAmount_AE");

                    dt_exp.Rows.Add(Convert.ToInt32(hdnSerComDetailId.Value), Convert.ToInt32(hdn_expenseId.Value),
                    lbl_Expense.Text, txt_amt.Text == "" ? decimalnull : Convert.ToDecimal(txt_amt.Text), txt_vat.Text == "" ? 0 : Convert.ToDecimal(txt_vat.Text),
                    drp_vendor.SelectedValue == "" ? intnull : Convert.ToInt32(drp_vendor.SelectedValue), drp_payMode.SelectedValue == "" ? intnull : Convert.ToInt32(drp_payMode.SelectedValue),
                    drp_account.SelectedValue == "" ? intnull : Convert.ToInt32(drp_account.SelectedValue), txt_payableAmount.Text == "" ? decimalnull : Convert.ToDecimal(txt_payableAmount.Text),
                    txt_paidAmount.Text == "" ? decimalnull : Convert.ToDecimal(txt_paidAmount.Text));
                }
            }

            dt_exp.Rows.RemoveAt(itemrp.ItemIndex);
            rptAddtnlExpense.DataSource = dt_exp;
            rptAddtnlExpense.DataBind();

            InlineCalculationAddtional();
            updAddtnlExpense.Update();
        }

        protected void btnInlineNewAddtnl_OnClick(object sender, EventArgs e)
        {
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

            decimal? decimalnull = null;
            int? intnull = null;
            if (rptAddtnlExpense.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rptAddtnlExpense.Items)
                {
                    HiddenField hdnSerComDetailId = (HiddenField)itm.FindControl("hdnSerComDetailId_AE");
                    HiddenField hdn_expenseId = (HiddenField)itm.FindControl("hdn_expenseId_AE");
                    Label lbl_Expense = (Label)itm.FindControl("lbl_Expense_AE");
                    TextBox txt_amt = (TextBox)itm.FindControl("txt_amt_AE");
                    TextBox txt_vat = (TextBox)itm.FindControl("txt_vat_AE");
                    RadComboBox drp_vendor = (RadComboBox)itm.FindControl("drp_vendor_AE");
                    RadComboBox drp_payMode = (RadComboBox)itm.FindControl("drp_payMode_AE");
                    RadComboBox drp_account = (RadComboBox)itm.FindControl("drp_account_AE");
                    TextBox txt_payableAmount = (TextBox)itm.FindControl("txt_payableAmount_AE");
                    TextBox txt_paidAmount = (TextBox)itm.FindControl("txt_paidAmount_AE");

                    dt_exp.Rows.Add(Convert.ToInt32(hdnSerComDetailId.Value), Convert.ToInt32(hdn_expenseId.Value),
                    lbl_Expense.Text, txt_amt.Text == "" ? decimalnull : Convert.ToDecimal(txt_amt.Text), txt_vat.Text == "" ? 0 : Convert.ToDecimal(txt_vat.Text),
                    drp_vendor.SelectedValue == "" ? intnull : Convert.ToInt32(drp_vendor.SelectedValue), drp_payMode.SelectedValue == "" ? intnull : Convert.ToInt32(drp_payMode.SelectedValue),
                    drp_account.SelectedValue == "" ? intnull : Convert.ToInt32(drp_account.SelectedValue), txt_payableAmount.Text == "" ? decimalnull : Convert.ToDecimal(txt_payableAmount.Text),
                    txt_paidAmount.Text == "" ? decimalnull : Convert.ToDecimal(txt_paidAmount.Text));
                }
            }
            dt_exp.Rows.Add(Convert.ToInt32(hdnAddIndexId.Value), Convert.ToInt32(drpAddExpense.SelectedValue), drpAddExpense.Text,
                Convert.ToDecimal(txtAddAmt.Text), txtAddVat.Text==""?0: Convert.ToDecimal(txtAddVat.Text),
                Convert.ToInt32(drpAddVendor.SelectedValue), Convert.ToInt32(drpAddPayMode.SelectedValue),
              drpAddAccount.SelectedValue==""?(int?)null:  Convert.ToInt32(drpAddAccount.SelectedValue), Convert.ToDecimal(txtAddPayableAmount.Text),
                Convert.ToDecimal(txtAddPaidAmount.Text));

            ClearExpenseDetailAddtnl();
            rptAddtnlExpense.DataSource = dt_exp;
            rptAddtnlExpense.DataBind();
            drpAddExpense.Focus();
            updAddtnlExpense.Update();
        }

        protected void btn_saveAddtnl_OnClick(object sender, EventArgs e)
        {
            txtAddVat.Text = txtAddVat.Text != "" ? txtAddVat.Text : "0.00";

            if (rptAddtnlExpense.Items.Count > 0 || (drpAddExpense.SelectedValue != "" && txtAddAmt.Text != "" &&  drpAddVendor.SelectedValue != ""
                && drpAddPayMode.SelectedValue != ""  && txtAddPaidAmount.Text != ""))
            {
                DataTable dt_deatils = fill_DetailAddtnl();
                SaveAddtionalServiceCompletion(dt_deatils);

                pnlAddtnlExpense.Visible = false;
                updAddtnlExpense.Update();
                Upd_Add_PanelInner.Update();
            }
        }

        public DataTable fill_DetailAddtnl()
        {
            txtAddVat.Text = txtAddVat.Text != "" ? txtAddVat.Text : "0.00";

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
            dt_exp.Columns.Add("VendorCommission", typeof(decimal));

            if (rptAddtnlExpense.Items.Count > 0)
            {
                foreach (RepeaterItem expItem in rptAddtnlExpense.Items)
                {
                    HiddenField hdnSerComDetailId = (HiddenField)expItem.FindControl("hdnSerComDetailId_AE");
                    HiddenField hdn_expenseId = (HiddenField)expItem.FindControl("hdn_expenseId_AE");
                    TextBox txt_amt = (TextBox)expItem.FindControl("txt_amt_AE");
                    TextBox txt_vat = (TextBox)expItem.FindControl("txt_vat_AE");
                    RadComboBox drp_vendor = (RadComboBox)expItem.FindControl("drp_vendor_AE");
                    RadComboBox drp_payMode = (RadComboBox)expItem.FindControl("drp_payMode_AE");
                    RadComboBox drp_account = (RadComboBox)expItem.FindControl("drp_account_AE");
                    TextBox txt_payableAmount = (TextBox)expItem.FindControl("txt_payableAmount_AE");
                    TextBox txt_paidAmount = (TextBox)expItem.FindControl("txt_paidAmount_AE");

                    dt_exp.Rows.Add(Convert.ToInt32(hdn_InvDetailIdAddtnl.Value), Convert.ToInt32(hdnSerComDetailId.Value), Convert.ToInt32(hdn_expenseId.Value),
                Convert.ToDecimal(txt_amt.Text), Convert.ToDecimal(txt_vat.Text),
                Convert.ToInt32(drp_vendor.SelectedValue), Convert.ToInt32(drp_payMode.SelectedValue),
               drp_account.SelectedValue==""?(int?)null: Convert.ToInt32(drp_account.SelectedValue), Convert.ToDecimal(txt_payableAmount.Text),
                Convert.ToDecimal(txt_paidAmount.Text), 0);
                }
            }
            if (drpAddExpense.SelectedValue != "" && txtAddAmt.Text != "" && txtAddVat.Text != "" && drpAddVendor.SelectedValue != ""
                && drpAddPayMode.SelectedValue != "" && drpAddAccount.SelectedValue != "" && txtAddPaidAmount.Text != "")
            {
                dt_exp.Rows.Add(Convert.ToInt32(hdn_InvDetailIdAddtnl.Value), 0, Convert.ToInt32(drpAddExpense.SelectedValue),
                Convert.ToDecimal(txtAddAmt.Text), Convert.ToDecimal(txtAddVat.Text),
                Convert.ToInt32(drpAddVendor.SelectedValue), Convert.ToInt32(drpAddPayMode.SelectedValue),
                Convert.ToInt32(drpAddAccount.SelectedValue), Convert.ToDecimal(txtAddPayableAmount.Text),
                Convert.ToDecimal(txtAddPaidAmount.Text), 0);
            }

            return dt_exp;
        }

        public void SaveAddtionalServiceCompletion(DataTable dt_deatils)
        {
            txtAddVat.Text = txtAddVat.Text != "" ? txtAddVat.Text : "0.00";

            if ((txtAddAmt.Text != "" || txtAddVat.Text != "") && (drpAddExpense.SelectedValue == "" || drpAddVendor.SelectedValue == ""))
            {
                InlineCalculationAddtional();
                updAddtnlExpense.Update();
            }

            int res = obj_trans.Insert_Update_ServiceCompletionAddtional(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_invId.Value),
                    Convert.ToInt32(hdn_InvDetailIdAddtnl.Value), 0,
                    Convert.ToDecimal(txtAddTotal.Text), Convert.ToDecimal(txtAddTotal.Text), dt_deatils,
                    DateTime.ParseExact(CalDate(SerComDateAdd), "dd/MM/yyyy", CultureInfo.InvariantCulture), Convert.ToInt32(hdn_user_id.Value),
                    txtRemarkSC.Text);
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
        }


        #region Navigation

        /*drpStatus OnSelectedIndexChanged*/
        protected void drpStatusOnSelectedIndexChanged(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), txt_search.Text, Common_order_column.Value, Common_asc_desc.Value);
        }

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


        protected void btn_TransDetail_Close_OnClick(object sender, EventArgs e)
        {
            pnl_transaDetail.Visible = false;
            Upd_TransaDetail_Panel.Update();
        }

        public void Clear()
        {
            hdn_id.Value = "0";
            hdn_InvDetailId.Value =hdn_invId.Value= "0";
            hdn_InComQty.Value = hdnSingleamount.Value = "0";

            rpt_expense_list.DataSource = null;
            rpt_expense_list.DataBind();
            ClearExpenseDetail();
            ClearExpenseDetailAddtnl();
            txt_Qty.Text = "";
            txt_Qty.Enabled = true;
            txt_amtSQty.Text = "";
            txt_totAmt.Text = txtscremark.Text = "";
            SerComDate.DbSelectedDate = DateTime.Now;

            rptfileupl.DataSource = null;
            rptfileupl.DataBind();

            rpt_TransacDetail.DataSource = null;
            rpt_TransacDetail.DataBind();
            btn_save.Visible = hdn_add.Value == "0" ? false : true;
            pnl_Expense_Panel.Visible = false;
            Upd_Add_PanelInner.Update();
        }

        /*Check Action Privilege*/
        public void previlage_action_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    DataTable dt = obj_common.Action_Previlage_Validation(19, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdn_update.Value = dt.Rows[1][1].ToString();
                        hdn_delete.Value = dt.Rows[2][1].ToString();
                        hdn_complete.Value = dt.Rows[3][1].ToString();
                        hdnsetdescrepancy.Value = dt.Rows[4][1].ToString();
                        hdncleardescrepancy.Value = dt.Rows[5][1].ToString();
                        hdnServiceStatus.Value = dt.Rows[6][1].ToString();
                    }
                    btn_save.Visible = hdn_add.Value == "0" ? false : true;
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

                    int val = obj_common.Form_Previlage_Validation(19, Convert.ToInt32(hdn_user_id.Value));
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