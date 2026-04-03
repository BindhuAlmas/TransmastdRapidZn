using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using AmarCentre.BAL;
using System.Globalization;

namespace AmarCentre.Transactions
{
    public partial class VendorBalanceMapping : System.Web.UI.Page
    {
        Transaction_Bal obj_trans = new Transaction_Bal();
        Master_Bal obj_mas = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
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
                Clear();
                grid_fill(1, 10, "", "", "");
                filldrop();
            }
        }

        public void filldrop()
        {
            drpVendor.Items.Clear();
            DataTable dt = obj_trans.DrpVendorCustomer();
            drpVendor.DataSource = dt;
            drpVendor.DataTextField = "Name";
            drpVendor.DataValueField = "Id";
            drpVendor.DataBind();
        }
        
        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_trans.listVendorBalMap(page_number, page_size, filter, column, order, Convert.ToInt32(hdn_user_id.Value));
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
            DataTable dt = obj_trans.listVendorBalMapExcel();
            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "VendorBalanceMapping");
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                HttpContext.Current.Response.Write(style);
                Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        protected void rpt_list_OnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdn_id");
            if (e.CommandName == "Edit")
            {
                Clear();
                pnl_add.Visible = true;

                DataSet ds = obj_trans.EditVendorBalMap(Convert.ToInt32(hdn_rpt_id.Value));
                DataTable dt1 = ds.Tables[0]; 
                DataTable dt_ser = ds.Tables[1];/* Detail*/

                hdn_id.Value = dt1.Rows[0]["Id"].ToString();
                lbl_Code.Text = dt1.Rows[0]["Code"].ToString();
                job_date.DbSelectedDate = dt1.Rows[0]["Dated"].ToString();
                drpVendor.SelectedValue = dt1.Rows[0]["VendorId"].ToString();
                drpVendor.Enabled = false;
                hdncustomerId.Value = dt1.Rows[0]["CustomerId"].ToString();
                txtTotal.Text = dt1.Rows[0]["TotalAmount"].ToString();

                DataTable dt = obj_mas.Edit_Vendor(Convert.ToInt32(drpVendor.SelectedValue)).Tables[0];
                txtvendorPayable.Text =(Convert.ToDecimal(dt.Rows[0]["Payable"])+Convert.ToDecimal(dt1.Rows[0]["TotalAmount"])).ToString();

                rpt_invoiceList.DataSource = dt_ser;
                rpt_invoiceList.DataBind();

                btn_cancel.Visible = false;
                btn_save.Visible = hdn_update.Value == "0" ? false : true;
                if (dt1.Rows[0]["Statusid"].ToString() == "1")
                    btn_cancel.Visible = hdncancel.Value == "0" ? false : true;

                Upd_Add_Panel.Update();
            }
             
        }

        protected void drpVendor_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rpt_invoiceList.DataSource = null;
            rpt_invoiceList.DataBind();
            txtvendorPayable.Text = txtTotal.Text = "";

            if (drpVendor.SelectedValue != "")
            {
                DataTable dt = obj_mas.Edit_Vendor(Convert.ToInt32(drpVendor.SelectedValue)).Tables[0];
                hdncustomerId.Value = dt.Rows[0]["CustomerId"].ToString();
                txtvendorPayable.Text = dt.Rows[0]["Payable"].ToString();
                DataSet ds = BalVoucher.GetCustOutStandingInvoiceList(Convert.ToInt32(hdncustomerId.Value));
                rpt_invoiceList.DataSource = ds.Tables[0];
                rpt_invoiceList.DataBind();
            }
            UpdinvoiceList.Update();
            updpayable.Update();
        }

        protected void btnAllocOnClick(object sender, EventArgs e)
        {
            if (txtvendorPayable.Text != "")
            {
                decimal balamt = 0;
                foreach (RepeaterItem item in rpt_invoiceList.Items)
                {
                    CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                    TextBox txt_balance_amount = (TextBox)item.FindControl("txtReceivableamt");
                    TextBox txt_pay_amount = (TextBox)item.FindControl("txtAmount");
                    balamt = balamt + Convert.ToDecimal(txt_balance_amount.Text);
                    chkSelect.Checked = false;
                    txtTotal.Text = "";
                }

                {
                    decimal Amount = Convert.ToDecimal(txtvendorPayable.Text);
                    decimal TotAmount = 0;
                    foreach (RepeaterItem item in rpt_invoiceList.Items)
                    {
                        CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                        TextBox txt_balance_amount = (TextBox)item.FindControl("txtReceivableamt");
                        TextBox txt_pay_amount = (TextBox)item.FindControl("txtAmount");
                        if (Amount > 0)
                        {
                            chkSelect.Checked = true;
                            if (Amount > Convert.ToDecimal(txt_balance_amount.Text))
                            {
                                txt_pay_amount.Text = txt_balance_amount.Text;
                                Amount = Amount - Convert.ToDecimal(txt_balance_amount.Text);
                                TotAmount = TotAmount + Convert.ToDecimal(txt_balance_amount.Text);
                            }
                            else
                            {
                                TotAmount = TotAmount + Amount;
                                txt_pay_amount.Text = Amount.ToString();
                                Amount = 0;
                            }
                        }
                    }
                    txtTotal.Text = TotAmount.ToString();
                }
            }
            UpdinvoiceList.Update();
        }

        protected void chkSelectOnCheckedChanged(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;
            UpdatePanel updAmount = (UpdatePanel)itemrp.FindControl("updAmount");
            TextBox txtAmount = (TextBox)itemrp.FindControl("txtAmount");

            TextBox txtReceivableamt = (TextBox)itemrp.FindControl("txtReceivableamt");
            CheckBox chkSelect = (CheckBox)itemrp.FindControl("chkSelect");
            if (chkSelect.Checked)
            {
                txtAmount.Text = txtReceivableamt.Text;
            }
            else
                txtAmount.Text = "";
            txtAmount.Enabled = chkSelect.Checked;
            updAmount.Update();
            AmountCalCulation();
        }

        public void AmountCalCulation()
        {
            decimal TotalAmount = 0;
            foreach (RepeaterItem item in rpt_invoiceList.Items)
            {
                CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                TextBox txtAmount = (TextBox)item.FindControl("txtAmount");
                if (chkSelect.Checked == true)
                    TotalAmount = TotalAmount + (txtAmount.Text == "" ? 0 : Convert.ToDecimal(txtAmount.Text));
            }
            txtTotal.Text = TotalAmount.ToString();
            updTotalInvoiceAmount.Update();
        }

        protected void txtAmount_TextChanged(object sender, EventArgs e)
        {
            AmountCalCulation();
        }

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            int res = SaveEntry();
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
            //pnl_add.Visible = false;
            Upd_Add_PanelInner.Update();
        }

        public int SaveEntry()
        {
            DataTable dt_deatils = fill_Detail();

            int res = 0;
            if (Convert.ToDecimal(txtTotal.Text) > Convert.ToDecimal(txtvendorPayable.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Amount cannot be greater than payable !');", true);
            }
            else
            {
                if (dt_deatils.Rows.Count > 0)
                {
                    res = obj_trans.InsertUpdateVendorBalMap(Convert.ToInt32(hdn_id.Value), job_date.SelectedDate,
                        Convert.ToInt32(drpVendor.SelectedValue), Convert.ToInt32(hdncustomerId.Value),
                        Convert.ToDecimal(txtTotal.Text), dt_deatils, Convert.ToInt32(hdn_user_id.Value));
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Add Invoice to Continue.!');", true);
                }
            }
            return res;
        }

        protected void btn_cancel_OnClick(object sender, EventArgs e)
        {
            int res = obj_trans.CancelVendorBalMap(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_user_id.Value));
            if (res > 0)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                lbl_msgin.Text = "Cancelled Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lblerrormsg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
            }
            Upd_Add_PanelInner.Update();
        }

        public DataTable fill_Detail()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("InvoiceId", typeof(int));
            dt.Columns.Add("ReceivableAmount", typeof(double));
            dt.Columns.Add("PaidAmount", typeof(double));

            foreach (RepeaterItem item in rpt_invoiceList.Items)
            {
                CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    HiddenField hdnInvoiceId = (HiddenField)item.FindControl("hdnInvoiceId");
                    TextBox txtReceivableamt = (TextBox)item.FindControl("txtReceivableamt");
                    TextBox txtAmount = (TextBox)item.FindControl("txtAmount");
                    dt.Rows.Add(hdnInvoiceId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvoiceId.Value), 
                          Convert.ToDouble(txtReceivableamt.Text), Convert.ToDouble(txtAmount.Text));
                }
            }
            return dt;
        }

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
            hdncustomerId.Value = "0";
            drpVendor.ClearSelection();
            drpVendor.Text = "";
            drpVendor.Enabled =  true;

            job_date.DbSelectedDate = DateTime.Now;
            txtTotal.Text = "";
            txtvendorPayable.Text = "";

            rpt_invoiceList.DataSource = null;
            rpt_invoiceList.DataBind();

            btn_save.Visible = hdn_add.Value == "0" ? false : true;
            btn_cancel.Visible =  false;
            Get_Code();

            Upd_Add_PanelInner.Update();
        }

        /*Code for Display*/
        public void Get_Code()
        {
            DataTable dt = obj_common.Get_Code(144);
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
                    DataTable dt = obj_common.Action_Previlage_Validation(144, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdncancel.Value = dt.Rows[1][1].ToString();
                        hdn_update.Value = dt.Rows[2][1].ToString();

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

                    int val = obj_common.Form_Previlage_Validation(144, Convert.ToInt32(hdn_user_id.Value));
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

    }
}