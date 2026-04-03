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
    public partial class CustomerInvoice : System.Web.UI.Page
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
                fill_Drp_down();
                Clear();
                grid_fill(1, 10, "", "", "");
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

            drp_Cust.Items.Clear();
            drp_Cust.DataSource = dt;
            drp_Cust.DataTextField = "text";
            drp_Cust.DataValueField = "value";
            drp_Cust.DataBind();
            drp_Cust.Text = "";

            drpagent.Items.Clear();
            drpagent.DataSource = obj_report.fill_drp_Agent();
            drpagent.DataTextField = "text";
            drpagent.DataValueField = "value";
            drpagent.DataBind();
            drpagent.Text = "";

        }

        public void fill_Drp_down()
        {
            fill_Customer();

            drpService.Items.Clear();
            drpService.DataSource = obj_report.Drp_Service(0);
            drpService.DataTextField = "text";
            drpService.DataValueField = "value";
            drpService.DataBind();
        }

        protected void btn_filter_OnClick(object sender, EventArgs e)
        {
            if (pnl_filter.Visible == true)
            {
                pnl_filter.Visible = false;
            }
            else
            {
                pnl_filter.Visible = true;
            }
            upd_nav_filter.Update();
        }


        protected void btn_search_Click(object sender, EventArgs e)
        {
            txt_search_OnTextChanged(null, null);
            pnl_filter.Visible = false;
            upd_nav_filter.Update();
        }

        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_trans.Get_List_CustomerInvoice(page_number, page_size, filter, column, order, Convert.ToInt32(hdn_user_id.Value),
                radfilterdate.SelectedDate, drp_Cust.SelectedValue==""?(int?)null:Convert.ToInt32(drp_Cust.SelectedValue), drpagent.SelectedValue==""?(int?)null:
                Convert.ToInt32(drpagent.SelectedValue),radtodate.SelectedDate );
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
            DataTable dt = obj_trans.Get_List_Customerinvoice_Excel(Convert.ToInt32(hdn_user_id.Value),
                 radfilterdate.SelectedDate, drp_Cust.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_Cust.SelectedValue), drpagent.SelectedValue == "" ? (int?)null :
                Convert.ToInt32(drpagent.SelectedValue), radtodate.SelectedDate);
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
            if (e.CommandName == "Edit")
            {
                Clear();
                pnl_add.Visible = true;

                DataSet ds = obj_trans.Edit_CustomerInvoice(Convert.ToInt32(hdn_rpt_id.Value));
                DataTable dt1 = ds.Tables[0];/*invoic*/
                DataTable dt_ser = ds.Tables[1];/* Detail*/

                hdn_id.Value = dt1.Rows[0]["Id"].ToString();
                lbl_Code.Text = dt1.Rows[0]["Code"].ToString();
                job_date.DbSelectedDate = dt1.Rows[0]["Dated"].ToString();
                drp_customer.SelectedValue = dt1.Rows[0]["Customer_Id"].ToString();
                hdn_CurrentInvoiceReceivable.Value = dt1.Rows[0]["Receivable"].ToString();
                drp_customer_OnSelectedIndexChanged(null, null);
                drp_customer.Enabled = false;
                Editfill_invoice();

                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    RadComboBoxItem item = (RadComboBoxItem)(drp_Invoice.FindItemByValue(dr["InvoiceId"].ToString()));
                    item.Checked = true;
                    item.Selected = true;
                }

                txt_grand.Text = dt1.Rows[0]["AfterDiscount_GrandTotal"].ToString();
                txt_totDiscount.Text = dt1.Rows[0]["Total_Discount"].ToString();

                txt_remark.Text = dt1.Rows[0]["Remarks"].ToString();
                hdnInvoiceStatus.Value = dt1.Rows[0]["Status"].ToString();
                rpt_Item_list.DataSource = dt_ser;
                rpt_Item_list.DataBind();

                btn_save.Visible = hdn_update.Value == "0" ? false : true;
                btn_save_print.Visible = hdn_update_N_print.Value == "0" ? false : true;
                btn_print.Visible = hdn_print.Value == "0" ? false : true;

                btn_cancel.Visible = hdn_cancel.Value == "0" ? false : true;
                btn_history.Visible = hdn_histry.Value == "0" ? false : true;

                if (dt1.Rows[0]["Status"].ToString() == "2" || dt1.Rows[0]["Status"].ToString() == "3") // 2-cancel 3-delete
                {
                    btn_cancel.Visible = btn_save.Visible = btn_save_print.Visible = false;
                }

                Upd_Add_Panel.Update();
            }
            else if (e.CommandName == "Print")
            {
                int Format = Convert.ToInt32(obj_mas.Edit_GeneralSettings().Rows[0]["CIInvoiceFormat"].ToString());
                string url = "";
                if (Format == 1)
                    url = "../Reports/CustomerInvoiceFormat1.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 2)
                    url = "../Reports/CustomerInvoiceFormat2.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 3)
                    url = "../Reports/CustomerInvoiceFormat3.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
            }
        }

        protected void drp_customer_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            /*Change in here should be checked in Customer.ascx also*/
           
            if (drp_customer.SelectedValue != "")
            {
                fill_invoice();
            }
            else
            {
                drp_Invoice.Items.Clear();
                drp_Invoice.DataSource = null;
                drp_Invoice.DataBind();
                drp_Invoice.Text = "";

                UpdInvoicePanel.Update();
            }
        }

        public int SaveInvoice()
        {
            int res = 0;
            DataTable dt_deatils = fill_Detail();
            if (dt_deatils.Rows.Count > 0)
            {
                    InlineCalculation();
                    res = obj_trans.Insert_Update_CustomerInvoice(Convert.ToInt32(hdn_id.Value), 
                        DateTime.ParseExact(CalDate(job_date), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Convert.ToInt32(drp_customer.SelectedValue), txt_remark.Text, Convert.ToInt32(hdn_user_id.Value), 
                Convert.ToDecimal(txt_totDiscount.Text), Convert.ToDecimal(txt_grand.Text),dt_deatils);
            }
            else
            {
                lbl_msgin.Text = "Add Service to Continue !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            return res;
        }

        /*Data To Save*/
        public DataTable fill_Detail()
        {
            DataTable dt_ser = new DataTable();
            dt_ser.Columns.Add("D_id", typeof(int));
            dt_ser.Columns.Add("InvoiceDetailId", typeof(int));
            dt_ser.Columns.Add("InvoiceId", typeof(int));

            if (rpt_Item_list.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_Item_list.Items)
                {
                    CheckBox chkSel = (CheckBox)itm.FindControl("chkSel");

                    HiddenField hdnInvDId = (HiddenField)itm.FindControl("hdnInvDId");
                    HiddenField hdnInvoiceDetailId = (HiddenField)itm.FindControl("hdnInvoiceDetailId");
                    HiddenField hdnInvoiceId = (HiddenField)itm.FindControl("hdnInvoiceId");
                    if (chkSel.Checked)
                        dt_ser.Rows.Add(Convert.ToInt32(hdnInvDId.Value), Convert.ToInt32(hdnInvoiceDetailId.Value), Convert.ToInt32(hdnInvoiceId.Value));
                }
            }
           
            return dt_ser;
        }

        /*Save*/
        protected void btn_save_OnClick(object sender, EventArgs e)
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

            Upd_Add_PanelInner.Update();
        }

        /*Save & Print*/
        protected void btn_save_print_OnClick(object sender, EventArgs e)
        {
            int res = SaveInvoice();

            if (res > 0)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                lbl_msgin.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                int Format = Convert.ToInt32(obj_mas.Edit_GeneralSettings().Rows[0]["CIInvoiceFormat"].ToString());
                string url = "";
                if (Format == 1)
                    url = "../Reports/CustomerInvoiceFormat1.aspx?id=" + res;
                else if (Format == 2)
                    url = "../Reports/CustomerInvoiceFormat2.aspx?id=" + res;
                else if (Format == 3)
                    url = "../Reports/CustomerInvoiceFormat3.aspx?id=" + res;
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
            }
            else
            {
                lbl_msgin.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }

            Upd_Add_PanelInner.Update();
        }

        /*Print*/
        protected void btn_print_OnClick(object sender, EventArgs e)
        {
            int Format = Convert.ToInt32(obj_mas.Edit_GeneralSettings().Rows[0]["CIInvoiceFormat"].ToString());
            string url = "";
            if (Format == 1)
                url = "../Reports/CustomerInvoiceFormat1.aspx?id=" + Convert.ToInt32(hdn_id.Value);
            else if (Format == 2)
                url = "../Reports/CustomerInvoiceFormat2.aspx?id=" + Convert.ToInt32(hdn_id.Value);
            else if (Format == 3)
                url = "../Reports/CustomerInvoiceFormat3.aspx?id=" + Convert.ToInt32(hdn_id.Value);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        #region cancel

        protected void btn_Cancelmain_OnClick(object sender, EventArgs e)
        {
            pnl_cancl.Visible = true;
            txt_cancelremark.Text = "";

            DataTable dt = obj_trans.getCustomerInvoiceCancelDetail(Convert.ToInt32(hdn_id.Value));
            rpt_cancelList.DataSource = dt;
            rpt_cancelList.DataBind();
            div_candet.Visible = dt.Rows.Count > 0 ? true : false;

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
            foreach (RepeaterItem item in rpt_cancelList.Items)
            {
                CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    HiddenField hdndetId = (HiddenField)item.FindControl("hdndetId");
                    HiddenField hdn_type = (HiddenField)item.FindControl("hdn_type");

                     if (hdn_type.Value == "2")
                    {
                        int ress = obj_trans.CustomerCancelDeleteReceipt(Convert.ToInt32(hdndetId.Value), 2, "Invoice Cancelled", Convert.ToInt32(hdn_user_id.Value));
                    }
                    else
                    {
                        DataTable dt_rv = obj_trans.get_receiptvoucherdet(Convert.ToInt32(hdndetId.Value));
                        if (dt_rv.Rows.Count > 0)
                        {
                            if (dt_rv.Rows[0][0].ToString() != "0")
                            {
                                int resf = BalVoucher.CancelDeleteReceiptVoucher(Convert.ToInt32(dt_rv.Rows[0][0].ToString()), 2, "Invoice Cancelled", Convert.ToInt32(hdn_user_id.Value));
                            }
                            else
                            {
                                int resf = obj_trans.CustomerCancelsingleReceiptVoucherentry(Convert.ToInt32(hdndetId.Value), Convert.ToInt32(hdn_user_id.Value));
                            }
                        }
                    }
                }
            }

            int res = obj_trans.Cancel_CustomerInvoice(Convert.ToInt32(hdn_id.Value), txt_cancelremark.Text, Convert.ToInt32(hdn_user_id.Value));
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

        public void fill_invoice()
        {
            drp_Invoice.Items.Clear();
            DataTable dt = obj_trans.drp_PendingInvoice(drp_customer.SelectedValue==""?0:Convert.ToInt32(drp_customer.SelectedValue), 
                0, frmdate.SelectedDate, todate.SelectedDate);
            drp_Invoice.DataSource = dt;
            drp_Invoice.DataTextField = "Text";
            drp_Invoice.DataValueField = "Value";
            drp_Invoice.DataBind();
            drp_Invoice.Text = "";

            UpdInvoicePanel.Update();

        }

        public void Editfill_invoice()
        {
            drp_Invoice.Items.Clear();
            DataTable dt = obj_trans.drp_PendingInvoice(drp_customer.SelectedValue == "" ? 0 : Convert.ToInt32(drp_customer.SelectedValue),
                Convert.ToInt32(hdn_id.Value), frmdate.SelectedDate, todate.SelectedDate);
            drp_Invoice.DataSource = dt;
            drp_Invoice.DataTextField = "Text";
            drp_Invoice.DataValueField = "Value";
            drp_Invoice.DataBind();
            drp_Invoice.Text = "";

            UpdInvoicePanel.Update();

        }

        protected void btn_SearchInvoice_OnClick(object sender, EventArgs e)
        {
            DataTable dtinvoice = new DataTable();
            dtinvoice.Columns.Add("InvoiceId", typeof(int));
            foreach (RadComboBoxItem item in drp_Invoice.Items)
            {
                if (item.Checked)
                    dtinvoice.Rows.Add(Convert.ToInt32(item.Value));
            }

            DataTable dt = new DataTable();
            dt = obj_trans.GetInvoiceDetails(dtinvoice,Convert.ToInt32(hdn_id.Value),frmdate.SelectedDate, 
                todate.SelectedDate,drpService.SelectedValue==""?(int?)null:Convert.ToInt32(drpService.SelectedValue),
                Convert.ToInt32(drp_customer.SelectedValue));
            rpt_Item_list.DataSource = dt;
            rpt_Item_list.DataBind();
            chkSelall.Checked = true;
            InlineCalculation();
            Upd_Item_Panel.Update();
        }

        protected void chkboxall_checked(object sender, EventArgs e)
        {
            foreach (RepeaterItem itm in rpt_Item_list.Items)
            {
                CheckBox chkSel = (CheckBox)itm.FindControl("chkSel");
                chkSel.Checked = chkSelall.Checked;
            }
            InlineCalculation();
            Upd_Item_Panel.Update();
        }

        /*Inline Calculation*/
        public void InlineCalculation()
        {
            decimal Total_Amt = 0, TotDiscount = 0, totQty = 0;

            decimal tot = 0, totdis = 0;

            foreach (RepeaterItem itm in rpt_Item_list.Items)
            {
                TextBox txtInvDTotal = (TextBox)itm.FindControl("txtInvDTotal");
                TextBox txtInvDdiscount = (TextBox)itm.FindControl("txtInvDdiscount");
                TextBox txtInvDQty = (TextBox)itm.FindControl("txtInvDQty");
                CheckBox chkSel = (CheckBox)itm.FindControl("chkSel");

                if (chkSel.Checked)
                {
                    tot = txtInvDTotal.Text == "" ? 0 : Convert.ToDecimal(txtInvDTotal.Text);
                    totdis = txtInvDdiscount.Text == "" ? 0 : Convert.ToDecimal(txtInvDdiscount.Text);
                    totQty = txtInvDQty.Text == "" ? 0 : Convert.ToDecimal(txtInvDQty.Text);

                    Total_Amt += tot;
                    TotDiscount = TotDiscount + Convert.ToDecimal(totQty * totdis);
                }
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

            txt_grand.Text = (Convert.ToDecimal(Final)).ToString("0.00");

            txt_totDiscount.Text = (Convert.ToDecimal(TotDiscount)).ToString("0.00");
            Updtxt_totDiscount.Update();
            Upd_Total_Panel.Update();
        }

        /*Reset*/
        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
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
            DataSet ds = obj_trans.list_CustomerInvHistry(date_from.SelectedDate, date_to.SelectedDate, Convert.ToInt32(hdn_id.Value),
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
            DataSet ds = obj_trans.list_CustomerInvHistry(date_from.SelectedDate, date_to.SelectedDate, Convert.ToInt32(hdn_id.Value),
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

        /*Clear All the Data*/
        public void Clear()
        {
            hdn_id.Value = "0";
            drp_customer.ClearSelection();
            drp_customer.Text = "";
            drp_customer.Enabled = true;
            drp_Invoice.Items.Clear();
            drp_Invoice.Text = "";
            chkSelall.Checked = false;
            frmdate.SelectedDate = null;
            todate.SelectedDate = null;
            drpService.ClearSelection();
            drpService.Text = "";
           
            hdn_CurrentInvoiceReceivable.Value = "0";
            drp_Invoice.Enabled = true;
            txt_remark.Text = "";
            txt_grand.Text = "";
            txt_totDiscount.Text = "";
            hdnInvoiceStatus.Value = "0";

            job_date.DbSelectedDate = DateTime.Now;

            rpt_Item_list.DataSource = null;
            rpt_Item_list.DataBind();
            btn_save.Visible = hdn_add.Value == "0" ? false : true;
            btn_save_print.Visible = hdn_add_N_print.Value == "0" ? false : true;
            btn_print.Visible = false;
            btn_cancel.Visible = false;
            btn_history.Visible = false;
            Get_Code();

            Upd_Add_PanelInner.Update();
        }

        /*Code for Display*/
        public void Get_Code()
        {
            DataTable dt = obj_common.Get_Code(55);
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
                    DataTable dt = obj_common.Action_Previlage_Validation(55, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdn_update.Value = dt.Rows[1][1].ToString();
                        hdn_print.Value = dt.Rows[2][1].ToString();
                        hdn_add_N_print.Value = dt.Rows[3][1].ToString();
                        hdn_update_N_print.Value = dt.Rows[4][1].ToString();
                        hdn_histry.Value = dt.Rows[5][1].ToString();
                        hdn_cancel.Value = dt.Rows[6][1].ToString();
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

                    int val = obj_common.Form_Previlage_Validation(55, Convert.ToInt32(hdn_user_id.Value));
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