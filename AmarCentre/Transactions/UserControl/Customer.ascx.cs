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

namespace AmarCentre.Transactions.UserControl
{
    public partial class Customer : System.Web.UI.UserControl
    {
        Master_Bal obj_master = new Master_Bal();
        Transaction_Bal obj_trans = new Transaction_Bal();
        System_Utilities obj_common = new System_Utilities();

        public void PageLoad(int agentid)
        {
            hdn_user_id.Value = Session["User_Id"].ToString();
            hdn_agentid.Value = agentid.ToString();
            previlage_check();
            previlage_action_check();
            fill_agent();
            Clear();
        }

        public void fill_agent()
        {
            DataSet ds = obj_master.drpforcustomer();

            drpagent.Items.Clear();
            drpagent.DataSource = ds.Tables[0];
            drpagent.DataTextField = "Text";
            drpagent.DataValueField = "Value";
            drpagent.DataBind();
            drpagent.Text = "";

            drpSponser.Items.Clear();
            drpSponser.DataSource = ds.Tables[1];
            drpSponser.DataTextField = "Text";
            drpSponser.DataValueField = "Value";
            drpSponser.DataBind();
            drpSponser.Text = "";

            drpEmirate.Items.Clear();
            drpEmirate.DataSource = ds.Tables[2];
            drpEmirate.DataTextField = "Text";
            drpEmirate.DataValueField = "Value";
            drpEmirate.DataBind();
            drpEmirate.Text = "";

            drpCategory.Items.Clear();
            drpCategory.DataSource = ds.Tables[3];
            drpCategory.DataTextField = "Text";
            drpCategory.DataValueField = "Value";
            drpCategory.DataBind();
            drpCategory.Text = "";

            RadComboBoxItem CodeItem = new RadComboBoxItem();
            CodeItem.Text = "New Entry";
            CodeItem.Value = "0";
            drpCategory.Items.Insert(0, CodeItem);

            if (hdn_agentid.Value != "0")
            {
                RadComboBoxItem item = (RadComboBoxItem)(drpagent.FindItemByValue(hdn_agentid.Value));
                item.Checked = true;
                item.Selected = true;
                drpagent.Enabled = false;
            }

            DataTable dt = ds.Tables[4];
            hdnDefaultEmirate.Value = dt.Rows[0]["DefaultEmirate"].ToString();
            hdnIsprofessionversion.Value =  dt.Rows[0]["IsProfessionVersion"].ToString();

            drpcompanygrp.Items.Clear();
            drpcompanygrp.DataSource = ds.Tables[5];
            drpcompanygrp.DataTextField = "Name";
            drpcompanygrp.DataValueField = "Id";
            drpcompanygrp.DataBind();
            drpcompanygrp.Text = "";
        }

        protected void drpCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (drpCategory.SelectedValue == "0")
            {
                pnlCategory.Visible = true;
                UCCategory.PageLoad();
                updCategoryPanel.Update();
            }
        }

        protected void chk_IsCredit_OnCheckedChanged(object sender, EventArgs e)
        {
            txt_CreditAmount.Text = "";
            pnl_CreditAmount.Visible = chk_IsCredit.Checked;
            Upd_CreditAmount_Panel.Update();
        }

        protected void btn_saveCustomer_OnClick(object sender, EventArgs e)
        {
            lbl_msg.Text = "";

            DataTable dt_agnt = new DataTable();
            dt_agnt.Columns.Add("Id", typeof(int));

            foreach (RadComboBoxItem item in drpagent.Items)
            {
                if (item.Checked)
                {
                    DataRow dr = dt_agnt.NewRow();
                    dt_agnt.Rows.Add(Convert.ToInt32(item.Value));
                }
            }

            int res = obj_master.Insert_Update_Customer(Convert.ToInt32(hdn_id.Value), txt_name.Text, txt_address.Text,
                txt_mob.Text, txt_phn.Text, txt_email.Text, txt_remark.Text, Convert.ToInt32(hdn_user_id.Value), txt_trn.Text
                , Convert.ToInt32(chk_IsCredit.Checked), txt_CreditAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txt_CreditAmount.Text), 0,
                txtArabicName.Text, dt_agnt, txtCperson.Text, txtmohre.Text, txtlicense.Text,
                drpSponser.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpSponser.SelectedValue),
                Convert.ToInt32(chkIsTyping.Checked),
                drpEmirate.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpEmirate.SelectedValue),
                drpCategory.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpCategory.SelectedValue), txtccmail.Text,
                txtWhatsappNo.Text,null,null, drpcompanygrp.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpcompanygrp.SelectedValue),
                Convert.ToInt32(chkcompanygrp.Checked));
           
            if (res == -1)
            {
                lbl_msg.Text = "Mobile Number Already exist.!";
            }

            else if (res > 0)
            {
                Panel pnl_Customer = (Panel)this.Parent.FindControl("pnl_Customer");
                UpdatePanel Upd_Customer_Panel = (UpdatePanel)this.Parent.FindControl("Upd_Customer_Panel");
                UpdatePanel Upd_CustomerDrop_Panel = (UpdatePanel)this.Parent.FindControl("Upd_CustomerDrop_Panel");

                RadComboBox drp_customer = (RadComboBox)this.Parent.FindControl("drp_customer");
                drp_customer.ClearSelection();
                drp_customer.Text = "";

                if (hdn_agentid.Value != "0")
                {
                    drp_customer.Items.Clear();
                    DataTable dt = obj_trans.Drp_Customer_FAgent(Convert.ToInt32(hdn_agentid.Value));
                    drp_customer.DataSource = dt;
                    drp_customer.DataTextField = "Text";
                    drp_customer.DataValueField = "Value";
                    drp_customer.DataBind();
                    drp_customer.Text = "";

                    RadComboBoxItem CodeItem = new RadComboBoxItem();
                    CodeItem.Text = "New Entry";
                    CodeItem.Value = "0";
                    drp_customer.Items.Insert(0, CodeItem);
                }
                else
                {
                    drp_customer.Items.Clear();
                    DataTable dt = obj_trans.Drp_CustomerWithMobileNo();
                    drp_customer.DataSource = dt;
                    drp_customer.DataTextField = "text";
                    drp_customer.DataValueField = "value";
                    drp_customer.DataBind();

                    RadComboBoxItem CodeItem = new RadComboBoxItem();
                    CodeItem.Text = "New Entry";
                    CodeItem.Value = "0";
                    drp_customer.Items.Insert(0, CodeItem);
                }

                drp_customer.SelectedValue = res.ToString();
                HiddenField hdn_PageName = (HiddenField)this.Parent.FindControl("hdn_PageName");
                if (hdn_PageName.Value == "Invoice")
                    drp_customer_OnSelectedIndexChanged();
                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                Upd_CustomerDrop_Panel.Update();
                pnl_Customer.Visible = false;
                Upd_Customer_Panel.Update();
            }
            else
            {
                Panel pnl_Customer = (Panel)this.Parent.FindControl("pnl_Customer");
                UpdatePanel Upd_Customer_Panel = (UpdatePanel)this.Parent.FindControl("Upd_Customer_Panel");
                UpdatePanel Upd_CustomerDrop_Panel = (UpdatePanel)this.Parent.FindControl("Upd_CustomerDrop_Panel");

                RadComboBox drp_customer = (RadComboBox)this.Parent.FindControl("drp_customer");
                drp_customer.ClearSelection();
                drp_customer.Text = "";

                drp_customer.ClearSelection();
                drp_customer.Text = "";
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                Upd_CustomerDrop_Panel.Update();
                pnl_Customer.Visible = false;
                Upd_Customer_Panel.Update();
            }
            Upd_CreditAmount_Panel.Update();
        }

        public void drp_customer_OnSelectedIndexChanged()
        {
            HiddenField hdn_IsCredit = (HiddenField)this.Parent.FindControl("hdn_IsCredit");
            Label lblCreditLimit = (Label)this.Parent.FindControl("lblCreditLimit");
            Label lblCurrentCreditAmt = (Label)this.Parent.FindControl("lblCurrentCreditAmt");
            Panel pnl_CreditDetail = (Panel)this.Parent.FindControl("pnl_CreditDetail");
            UpdatePanel Upd_CreditDetail_Panel = (UpdatePanel)this.Parent.FindControl("Upd_CreditDetail_Panel");
            HiddenField hdn_TaxInvoicePrint = (HiddenField)this.Parent.FindControl("hdn_TaxInvoicePrint");
            Button btn_TaxInvoicePrint = (Button)this.Parent.FindControl("btn_TaxInvoicePrint");
            UpdatePanel Upd_btnTaxInvoicePrint = (UpdatePanel)this.Parent.FindControl("Upd_btnTaxInvoicePrint");
            UpdatePanel updBankCharge = (UpdatePanel)this.Parent.FindControl("updBankCharge");
            Panel pnlbankcharge = (Panel)this.Parent.FindControl("pnlbankcharge");

            hdn_IsCredit.Value = Convert.ToInt32(chk_IsCredit.Checked).ToString();
            lblCreditLimit.Text = txt_CreditAmount.Text;
            lblCurrentCreditAmt.Text ="0.00";
            pnl_CreditDetail.Visible = hdn_IsCredit.Value == "1" ? true : false;
            if (hdn_IsCredit.Value == "1")
                btn_TaxInvoicePrint.Visible = hdn_TaxInvoicePrint.Value == "0" ? false : true;
            else
                pnlbankcharge.Visible = true;
            Upd_btnTaxInvoicePrint.Update();
            Upd_CreditDetail_Panel.Update();
            updBankCharge.Update();
        }

        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
        }
        protected void btn_close_OnClick(object sender, EventArgs e)
        {
            RadComboBox drp_customer = (RadComboBox)this.Parent.FindControl("drp_customer");
            drp_customer.ClearSelection();
            drp_customer.Text = "";

            Panel pnl_Customer = (Panel)this.Parent.FindControl("pnl_Customer");
            UpdatePanel Upd_Customer_Panel = (UpdatePanel)this.Parent.FindControl("Upd_Customer_Panel");
            UpdatePanel Upd_CustomerDrop_Panel = (UpdatePanel)this.Parent.FindControl("Upd_CustomerDrop_Panel");
            Upd_CustomerDrop_Panel.Update();
            pnl_Customer.Visible = false;
            Upd_Customer_Panel.Update();
        }
        
        public void Clear()
        {
            txt_name.Text = lbl_msg.Text= "";
            txtArabicName.Text = txtWhatsappNo.Text= "";
            txt_address.Text = "";
            txt_mob.Text = "";
            txt_phn.Text = "";
            txt_remark.Text = "";
            txt_email.Text = "";
            txt_trn.Text = "";
            chk_IsCredit.Checked =chkIsTyping.Checked= false;
            txt_CreditAmount.Text = "";
            pnl_CreditAmount.Visible = chk_IsCredit.Checked;
            hdn_id.Value = "0";
            txtCperson.Text = txtmohre.Text = txtlicense.Text = "";
            drpSponser.ClearSelection();
            drpSponser.Text = "";
            drpEmirate.ClearSelection();
            drpEmirate.Text = "";
            drpCategory.ClearSelection();
            drpCategory.Text =txtccmail.Text= "";
            drpEmirate.SelectedValue = hdnDefaultEmirate.Value;
            chkcompanygrp.Checked = false;
            drpcompanygrp.ClearSelection();
            drpcompanygrp.Text = "";
            pnlchkcompanygrp.Visible = pnlcompanygrp.Visible = (hdnIsprofessionversion.Value == "1" ? true : false);

            //drpagent.Text = string.Empty;
            //drpagent.ClearCheckedItems();

            btn_saveCustomer.Visible = hdn_add.Value == "0" ? false : true;
            

            Upd_Add_PanelInner.Update();
        }
       
        //Calculate Date
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

        //Check Privilege
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.Form_Previlage_Validation(8, Convert.ToInt32(hdn_user_id.Value));
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

        //Check Privilege
        public void previlage_action_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    DataTable dt = obj_common.Action_Previlage_Validation(8, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                    }
                    btn_saveCustomer.Visible = hdn_add.Value == "0" ? false : true;
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