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
    public partial class DebitNote : System.Web.UI.Page
    {
        Transaction_Bal obj_trans = new Transaction_Bal();
        System_Utilities obj_common = new System_Utilities();
        Master_Bal obj_master = new Master_Bal();
        public int ReceiptIdpub;

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
            drp_customer.ClearSelection();
            drp_customer.Text = "";
        }

        protected void drp_customer_OnSelectedIndexChanged(Object sender, EventArgs e)
        {
            DataTable dt = obj_trans.Drp_InvoicebyCustomer(drp_customer.SelectedValue == "" ? 0 : Convert.ToInt32(drp_customer.SelectedValue));
            drpInvoice.DataSource = dt;
            drpInvoice.DataTextField = "Code";
            drpInvoice.DataValueField = "Id";
            drpInvoice.DataBind();
            drpInvoice.ClearSelection();
            drpInvoice.Text = "";

            updinvoiceDrp.Update();
        }

        protected void drpInvoiceOnSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (drpInvoice.SelectedValue != "")
            {
                DataTable dtsc = obj_trans.Drp_DebitnoteSCbyInvoice(Convert.ToInt32(drpInvoice.SelectedValue));
               
                drpSC.DataSource = dtsc;
                drpSC.DataTextField = "Code";
                drpSC.DataValueField = "Id";
                drpSC.DataBind();
                drpSC.ClearSelection();
                drpSC.Text = "";
                updsc.Update();
            }
            else
                Clear();
        }

        //Get List 
        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        
        {
            DataTable dt = obj_trans.Get_ListDebitnote(page_number, page_size, filter, column, order, Convert.ToInt32(hdn_user_id.Value));
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
            DataTable dt = obj_trans.Get_ListDebitnoteExcel(Convert.ToInt32(hdn_user_id.Value));
            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "DebitNote");
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

                DataSet ds = obj_trans.EditDebitnote(Convert.ToInt32(hdn_rpt_id.Value), Convert.ToInt32(hdn_user_id.Value));
                DataTable dt1 = ds.Tables[0];/*invoic*/
                DataTable dt_ser = ds.Tables[1];/*det*/

                if (dt1.Rows.Count > 0)
                {
                    hdn_id.Value = dt1.Rows[0]["Id"].ToString();
                    lbl_Code.Text = dt1.Rows[0]["Code"].ToString();
                    job_date.DbSelectedDate = dt1.Rows[0]["Dated"].ToString();
                   
                    DataTable dtsc = new DataTable();
                    dtsc.Columns.Add("Id", typeof(int));
                    dtsc.Columns.Add("Code",typeof(string));
                    DataRow drow = dtsc.NewRow();
                    drow["Id"] = Convert.ToInt32(dt1.Rows[0]["scid"].ToString());
                    drow["Code"] = dt1.Rows[0]["scno"].ToString();
                    dtsc.Rows.Add(drow);
                    drpSC.DataSource = dtsc;
                    drpSC.DataTextField = "Code";
                    drpSC.DataValueField = "Id";
                    drpSC.DataBind();
                    drpSC.SelectedValue = dt1.Rows[0]["scid"].ToString();

                    drp_customer.SelectedValue = dt1.Rows[0]["CustomerId"].ToString();
                    drp_customer_OnSelectedIndexChanged(new object(), new EventArgs());
                    drpInvoice.SelectedValue = dt1.Rows[0]["InvoiceId"].ToString();
                   
                    txtdnqty.Text = dt1.Rows[0]["Quantity"].ToString();
                    txtDNServiceName.Text = dt1.Rows[0]["service"].ToString();
                    drpInvoice.Enabled = drp_customer.Enabled =drpSC.Enabled= false;
                    btnDebitNoteSave.Visible = false;
                    txt_grand.Text = dt1.Rows[0]["TotalAmount"].ToString();

                    rptdnexpense.DataSource = dt_ser;
                    rptdnexpense.DataBind();
                    if (dt1.Rows[0]["IsAllowEdit"].ToString() == "0")
                        btnDebitNoteSave.Visible =  false;

                    //if (dt1.Rows[0]["Statusid"].ToString() == "1")
                    //    btn_cancel.Visible = hdncancel.Value == "0" ? false : true;
                }
                Upd_Add_Panel.Update();
            }
            //else if (e.CommandName == "Print")
            //{
            //    string url = "../Reports/CreditNotePrint.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
            //    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
            //}
        }

        protected void rpt_list_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Button btnPrint = (Button)e.Item.FindControl("btnPrint");
            //btnPrint.Visible = hdn_print.Value == "0" ? false : true;
        }

        protected void drpSC_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (drpSC.SelectedValue != "")
            {
                FillExpenseDetails(Convert.ToInt32(drpSC.SelectedValue));
            }
        }
        private void FillExpenseDetails(int id)
        {
            DataSet ds = obj_trans.DebitNoteServiceCompletion(id);
            DataTable dtSerCom = ds.Tables[0];
            DataTable dtSerComDetail = ds.Tables[1];
            DataTable dtSerComTransDetail = ds.Tables[2];
            if (dtSerCom.Rows.Count > 0)
            {
                txtDNServiceName.Text = dtSerCom.Rows[0]["servicename"].ToString();
                txtdnqty.Text = "1";
            }
            rptdnexpense.DataSource = dtSerComDetail;
            rptdnexpense.DataBind();
            Upd_Add_PanelInner.Update();
        }
        protected void rptdnexpense_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdn_payModeId = (HiddenField)e.Item.FindControl("hdn_dnpayModeId");
                RadComboBox drp_payMode = (RadComboBox)e.Item.FindControl("drp_dnpayMode"); //value added in design

                drp_payMode.SelectedValue = hdn_payModeId.Value;

                HiddenField hdn_accountId = (HiddenField)e.Item.FindControl("hdn_dnaccountId");
                RadComboBox drp_account = (RadComboBox)e.Item.FindControl("drp_dnaccount");
                drp_account.Items.Clear();

                RequiredFieldValidator rqddnaccountIn = (RequiredFieldValidator)e.Item.FindControl("rqddnaccountIn");
                UpdatePanel Upd_dnAccount_Panel = (UpdatePanel)e.Item.FindControl("Upd_dnAccount_Panel");
                TextBox txtdnreceivedamt = (TextBox)e.Item.FindControl("txtdnreceivedamt");
                UpdatePanel upddnreceivedAmountIn = (UpdatePanel)e.Item.FindControl("upddnreceivedAmountIn");

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
                if (hdn_payModeId.Value == "7" || hdn_payModeId.Value == "9") //topup and receiving later
                {
                    rqddnaccountIn.Enabled = false;
                    txtdnreceivedamt.ReadOnly = true;
                    txtdnreceivedamt.Text = "0";
                }

                upddnreceivedAmountIn.Update();
                Upd_dnAccount_Panel.Update();
            }
        }

        protected void drp_dnpayMode_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox drp = (RadComboBox)sender;
            RepeaterItem itm = (RepeaterItem)drp.Parent;
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            HiddenField hdn_accountId = (HiddenField)itm.FindControl("hdn_dnaccountId");
            RadComboBox drp_dnaccount = (RadComboBox)itm.FindControl("drp_dnaccount");
            RequiredFieldValidator rqddnaccountIn = (RequiredFieldValidator)itm.FindControl("rqddnaccountIn");
            UpdatePanel Upd_dnAccount_Panel = (UpdatePanel)itm.FindControl("Upd_dnAccount_Panel");
            TextBox txtdnreceivedamt = (TextBox)itm.FindControl("txtdnreceivedamt");
            UpdatePanel upddnreceivedAmountIn = (UpdatePanel)itm.FindControl("upddnreceivedAmountIn");

            rqddnaccountIn.Enabled = true;
            txtdnreceivedamt.ReadOnly = false;
            drp_dnaccount.Items.Clear();

            if (drp.SelectedValue != "")
            {
                DataTable dtAccount = obj_trans.ListAccountInServCompletion(Convert.ToInt32(drp.SelectedValue), Convert.ToInt32(hdn_user_id.Value), 0);
                drp_dnaccount.DataSource = dtAccount;
                drp_dnaccount.DataValueField = "Value";
                drp_dnaccount.DataTextField = "Text";
                drp_dnaccount.DataBind();
            }
            hdn_accountId.Value = "";
            drp_dnaccount.ClearSelection();
            drp_dnaccount.Text = "";

            if (drp.SelectedValue == "7" || drp.SelectedValue == "9")
            {
                rqddnaccountIn.Enabled = false;
                txtdnreceivedamt.ReadOnly = true;
                txtdnreceivedamt.Text = "0";
            }

            upddnreceivedAmountIn.Update();
            Upd_dnAccount_Panel.Update();
        }
        protected void btnDebitNoteSave_Click(object sender, EventArgs e)
        {
            int res = SaveDebitNote();
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
            pnl_add.Visible = false;
            Upd_Add_Panel.Update();
        }
        public int SaveDebitNote()
        {
            DataTable dt_deatils = fill_DebitNoteDetail();

            int res = 0;
            if (dt_deatils.Rows.Count > 0)
            {

                res = obj_trans.Insert_UpdateDebitnote(Convert.ToInt32(hdn_id.Value), job_date.SelectedDate,
                    Convert.ToInt32(drpInvoice.SelectedValue), Convert.ToInt32(drpSC.SelectedValue), Convert.ToDecimal(txtdnqty.Text), "",
                    Convert.ToDecimal(txt_grand.Text), dt_deatils, Convert.ToInt32(hdn_user_id.Value));
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Add Service to Continue.!');", true);
            }
            return res;
        }
        public DataTable fill_DebitNoteDetail()
        {
            DataTable dt_ser = new DataTable();
            dt_ser.Columns.Add("D_id", typeof(int));
            dt_ser.Columns.Add("ExpenseId", typeof(int));
            dt_ser.Columns.Add("Amount", typeof(decimal));
            dt_ser.Columns.Add("Vat", typeof(decimal));
            dt_ser.Columns.Add("VendorId", typeof(int));
            dt_ser.Columns.Add("PaymentMethodId", typeof(int));
            dt_ser.Columns.Add("AccountId", typeof(int));
            dt_ser.Columns.Add("TotalAmount", typeof(decimal));
            dt_ser.Columns.Add("DebitNoteAmount", typeof(decimal));
            dt_ser.Columns.Add("DebitNoteReceivedAmount", typeof(decimal));
            dt_ser.Columns.Add("SerComDetailId", typeof(int));

            foreach (RepeaterItem itm in rptdnexpense.Items)
            {
                HiddenField hdn_D_id = (HiddenField)itm.FindControl("hdn_id");

                TextBox txt_Qty = (TextBox)itm.FindControl("txt_Qty");
                HiddenField hdn_expid = (HiddenField)itm.FindControl("hdn_expenseId");
                Label txt_amount = (Label)itm.FindControl("txt_amt");
                Label txt_vat = (Label)itm.FindControl("txt_vat");
                HiddenField hdn_vendorid = (HiddenField)itm.FindControl("hdn_vendorId");
                RadComboBox drp_payMode = (RadComboBox)itm.FindControl("drp_dnpayMode");
                RadComboBox drp_accountId = (RadComboBox)itm.FindControl("drp_dnaccount");
                Label txt_totalamt = (Label)itm.FindControl("txt_payableAmount");
                TextBox txt_dnamt = (TextBox)itm.FindControl("txt_dnpaidAmount");
                TextBox txt_dnrecamt = (TextBox)itm.FindControl("txtdnreceivedamt");
                HiddenField hdndnSerComDetailId = (HiddenField)itm.FindControl("hdndnSerComDetailId");

                if (txt_dnamt.Text != "")
                    dt_ser.Rows.Add(0, Convert.ToInt32(hdn_expid.Value), Convert.ToDecimal(txt_amount.Text),
                            Convert.ToDecimal(txt_vat.Text), Convert.ToInt32(hdn_vendorid.Value), Convert.ToInt32(drp_payMode.SelectedValue),
                        drp_accountId.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_accountId.SelectedValue), Convert.ToDecimal(txt_totalamt.Text),
                            Convert.ToDecimal(txt_dnamt.Text), Convert.ToDecimal(txt_dnrecamt.Text),
                            Convert.ToInt32(hdndnSerComDetailId.Value));

            }
            return dt_ser;
        }
        protected void btndnclose_Click(object sender, EventArgs e)
        {
            pnl_add.Visible = false;
            Upd_Add_Panel.Update();
        }

        protected void btn_cancel_OnClick(object sender, EventArgs e)
        {
            //int res = obj_trans.CancelCreditnote(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_user_id.Value));
            //if (res > 0)
            //{
            //    grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
            //    Clear();
            //    lbl_msgin.Text = "Cancelled Successfully !..";
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            //}
            //else
            //{
            //    lblerrormsg.Text = "Sorry Failed to Process Your Request !..";
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
            //}
            //Upd_Add_PanelInner.Update();
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

        protected void btn_newentry_OnClick(object sender, EventArgs e)
        {
            Clear();
            fill_Customer();
            pnl_add.Visible = true;
            Upd_Add_Panel.Update();
        }

        /*Clear All the Data*/
        public void Clear()
        {
            hdn_id.Value = "0";
            drpInvoice.Items.Clear();
            drpInvoice.Text = "";
            drpInvoice.ClearSelection();
            drpSC.Items.Clear();
            drpSC.Text = "";
            drpSC.ClearSelection();
            drp_customer.Text = "";
            drp_customer.ClearSelection();
            drpInvoice.Enabled = drp_customer.Enabled = drpSC.Enabled = true;
            job_date.DbSelectedDate = DateTime.Now;
            txt_grand.Text = "";
            txtdnqty.Text =txtDNServiceName.Text= "";
            job_date.SelectedDate = DateTime.Now;

            rptdnexpense.DataSource = null;
            rptdnexpense.DataBind();

            btnDebitNoteSave.Visible = hdn_add.Value == "0" ? false : true;
            Get_Code();
            Upd_Add_PanelInner.Update();
        }

        /*Code for Display*/
        public void Get_Code()
        {
            DataTable dt = obj_common.Get_Code(145);
            if (dt.Rows.Count > 0)
                lbl_Code.Text = dt.Rows[0][0].ToString();
        }

        public void previlage_action_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    DataTable dt = obj_common.Action_Previlage_Validation(145, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        //hdncancel.Value = dt.Rows[1][1].ToString();
                    }
                    btnDebitNoteSave.Visible = hdn_add.Value == "0" ? false : true;
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

                    int val = obj_common.Form_Previlage_Validation(145, Convert.ToInt32(hdn_user_id.Value));
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