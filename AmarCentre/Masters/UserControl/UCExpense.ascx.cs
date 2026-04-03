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

namespace AmarCentre.Masters.UserControl
{
    public partial class UCExpense : System.Web.UI.UserControl
    {
        Master_Bal obj_master = new Master_Bal();
        Transaction_Bal obj_trans = new Transaction_Bal();
        System_Utilities obj_common = new System_Utilities();
        Voucher BalVoucher = new Voucher();

        public void PageLoad()
        {
           
        }

        public void UCPageLoad(int PageId)
        {
            hdn_user_id.Value = Session["User_Id"].ToString();
            hdnPageId.Value = PageId.ToString();  //1-PV,   2-service
            previlage_action_check();
            fillexpType();
            Clear();
        }

        public void fillexpType()
        {
            DataTable dt = obj_master.fill_exp_Type();
            drp_Type.DataSource = dt;
            drp_Type.DataTextField = "text";
            drp_Type.DataValueField = "value";
            drp_Type.DataBind();
        }

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Insert_Update_Expense(0, txt_name.Text, txt_desc.Text, Convert.ToInt32(drp_Type.SelectedValue),
                 txt_tax.Text == "" ? 0 : Convert.ToDecimal(txt_tax.Text), Convert.ToInt32(chkTaxAppliForCommision.Checked), Convert.ToInt32(hdn_user_id.Value));

            if (hdnPageId.Value == "1")
            {
                RadComboBox drpExpenseType = (RadComboBox)this.Parent.FindControl("drpExpenseType");
                drpExpenseType.ClearSelection();
                drpExpenseType.Text = "";
                if (res > 0)
                {
                    drpExpenseType.Items.Clear();
                    DataTable dt = BalVoucher.GetExpenseList();
                    drpExpenseType.DataSource = dt;
                    drpExpenseType.DataTextField = "text";
                    drpExpenseType.DataValueField = "value";
                    drpExpenseType.DataBind();

                    RadComboBoxItem CodeItem = new RadComboBoxItem();
                    CodeItem.Text = "New Entry";
                    CodeItem.Value = "0";
                    drpExpenseType.Items.Insert(0, CodeItem);

                    drpExpenseType.SelectedValue = res.ToString();

                    Clear();
                    lbl_msg.Text = "Saved Successfully !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                }
                else
                {
                    drpExpenseType.ClearSelection();
                    drpExpenseType.Text = "";
                    lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                }
                Panel pnlExpense = (Panel)this.Parent.FindControl("pnlExpense");
                UpdatePanel UpdExpensePanel = (UpdatePanel)this.Parent.FindControl("UpdExpensePanel");
                UpdatePanel UpdExpenseDrop_Panel = (UpdatePanel)this.Parent.FindControl("UpdExpenseDrop_Panel");
                pnlExpense.Visible = false;
                UpdExpensePanel.Update();
                UpdExpenseDrop_Panel.Update();
            }
            else if (hdnPageId.Value == "2")
            {
                Repeater rpt_serdetail = (Repeater)this.Parent.FindControl("rpt_serdetail");
                UpdatePanel Upd_ItemList = (UpdatePanel)this.Parent.FindControl("Upd_ItemList");

                DataTable dt_serDetail = new DataTable();
                dt_serDetail.Columns.Add("SerDetailId", typeof(int));
                dt_serDetail.Columns.Add("ExpenseId", typeof(int));
                dt_serDetail.Columns.Add("Amount", typeof(decimal));
                dt_serDetail.Columns.Add("VAT", typeof(decimal));
                dt_serDetail.Columns.Add("VendorId", typeof(int));
                dt_serDetail.Columns.Add("PayModeId", typeof(int));
                dt_serDetail.Columns.Add("AccountId", typeof(int));
                dt_serDetail.Columns.Add("TaxExempt", typeof(int));
                dt_serDetail.Columns.Add("vendorCommission", typeof(decimal));

                foreach (RepeaterItem itm in rpt_serdetail.Items)
                {
                    HiddenField hdn_serDetailId = (HiddenField)itm.FindControl("hdn_serDetailId");
                    RadComboBox drp_expense = (RadComboBox)itm.FindControl("drp_expense");
                    TextBox txt_amt = (TextBox)itm.FindControl("txt_amt");
                    TextBox txt_vat = (TextBox)itm.FindControl("txt_vat");
                    RadComboBox drp_vendor = (RadComboBox)itm.FindControl("drp_vendor");
                    RadComboBox drp_payMode = (RadComboBox)itm.FindControl("drp_payMode");
                    RadComboBox drp_account = (RadComboBox)itm.FindControl("drp_account");
                    CheckBox chk_taxExempt = (CheckBox)itm.FindControl("chk_taxExempt");
                    TextBox txtvendorCommission = (TextBox)itm.FindControl("txtvendorCommission");

                    if (drp_expense.SelectedValue != "")
                    {
                        dt_serDetail.Rows.Add(Convert.ToInt32(hdn_serDetailId.Value), Convert.ToInt32(drp_expense.SelectedValue),
                            Convert.ToDecimal(txt_amt.Text), Convert.ToDecimal(txt_vat.Text),
                            drp_vendor.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_vendor.SelectedValue),
                            drp_payMode.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_payMode.SelectedValue),
                            drp_account.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_account.SelectedValue),
                            Convert.ToInt32(chk_taxExempt.Checked),
                            txtvendorCommission.Text == "" ? 0 : Convert.ToDecimal(txtvendorCommission.Text));
                    }
                }
                if (dt_serDetail.Rows.Count == 0)
                    dt_serDetail.Rows.Add(-1, null, 0.00, 0.00, null, null, null, 0);
                rpt_serdetail.DataSource = dt_serDetail;
                rpt_serdetail.DataBind();

                Upd_ItemList.Update();

                Panel pnlExpense = (Panel)this.Parent.FindControl("pnlExpense");
                UpdatePanel UpdExpensePanel = (UpdatePanel)this.Parent.FindControl("UpdExpensePanel");
                pnlExpense.Visible = false;
                UpdExpensePanel.Update();

            }
        }

        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
        }
        protected void btn_close_OnClick(object sender, EventArgs e)
        {
            if (hdnPageId.Value == "1")
            {
                RadComboBox drpExpenseType = (RadComboBox)this.Parent.FindControl("drpExpenseType");
                drpExpenseType.ClearSelection();
                drpExpenseType.Text = "";

                Panel pnlExpense = (Panel)this.Parent.FindControl("pnlExpense");
                UpdatePanel UpdExpensePanel = (UpdatePanel)this.Parent.FindControl("UpdExpensePanel");
                UpdatePanel UpdExpenseDrop_Panel = (UpdatePanel)this.Parent.FindControl("UpdExpenseDrop_Panel");
                UpdExpensePanel.Update();
                pnlExpense.Visible = false;
                UpdExpenseDrop_Panel.Update();
            }
            else if (hdnPageId.Value == "2")
            {                
                Panel pnlExpense = (Panel)this.Parent.FindControl("pnlExpense");
                UpdatePanel UpdExpensePanel = (UpdatePanel)this.Parent.FindControl("UpdExpensePanel");
                pnlExpense.Visible = false;
                UpdExpensePanel.Update();
            }
        }

        public void Clear()
        {
            txt_name.Text = "";
            txt_desc.Text = "";
            drp_Type.ClearSelection();
            drp_Type.Text = "";
            txt_tax.Text = "";
            hdn_id.Value = "0";

            btn_save.Visible = hdn_add.Value == "0" ? false : true;

            Upd_Add_PanelInner.Update();
        }

        public void previlage_action_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    DataTable dt = obj_common.Action_Previlage_Validation(7, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
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
    }
}