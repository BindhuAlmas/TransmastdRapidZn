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
    public partial class PendingCard : System.Web.UI.Page
    {
        Master_Bal obj_master = new Master_Bal();
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
                fillbank();
                Clear();
            }
        }

        public void fillbank()
        {
            drpBankAccountfilter.DataSource = BalVoucher.GetBankAccountList(Convert.ToInt32(hdn_user_id.Value));
            drpBankAccountfilter.DataValueField = "Value";
            drpBankAccountfilter.DataTextField = "Text";
            drpBankAccountfilter.DataBind();
            drpBankAccountfilter.Text = "";
        }

        public void FillList()
        {
            btnProceed.Enabled = drpStatus.SelectedValue == "0" ? true : false;
            DataTable dt = BalVoucher.GetPendingCardList(txt_search.Text, drpBankAccountfilter.SelectedValue == "" ? (int?)null :
                Convert.ToInt32(drpBankAccountfilter.SelectedValue),Convert.ToInt32(drpStatus.SelectedValue));
            rpt_list.DataSource = dt;
            rpt_list.DataBind();

            chkselectall.Checked = false;

            Upd_List_Panel.Update();
        }

        protected void drpfilterOnSelectedIndexChanged(object sender, EventArgs e)
        {
            FillList();
        }

        protected void chkselectall_CheckedChanged(object sender, EventArgs e)
        {
            foreach (RepeaterItem itm in rpt_list.Items)
            {
                CheckBox chkselectIn = (CheckBox)itm.FindControl("chkselectIn");
                chkselectIn.Checked = chkselectall.Checked;
            }
            Upd_List_Panel.Update();
            CalculateAmt();
        }

        protected void chkselectIn_CheckedChanged(object sender, EventArgs e)
        {
            CalculateAmt();
        }
        protected void txtBankCommission_TextChanged(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            UpdatePanel updCreditAmountIn = (UpdatePanel)itemrp.FindControl("updCreditAmountIn");
            HiddenField hdnAmount = (HiddenField)itemrp.FindControl("hdnAmount");
            TextBox txtBankCommission = (TextBox)itemrp.FindControl("txtBankCommission");
            Label lblCreditAmount = (Label)itemrp.FindControl("lblCreditAmount");
            TextBox txtCommissionVat = (TextBox)itemrp.FindControl("txtCommissionVat");

            lblCreditAmount.Text = ( (hdnAmount.Value == "" ? 0 : Convert.ToDecimal(hdnAmount.Value)) -
                (txtBankCommission.Text == "" ? 0 : Convert.ToDecimal(txtBankCommission.Text))-
                (txtCommissionVat.Text == "" ? 0 : Convert.ToDecimal(txtCommissionVat.Text)) ).ToString();
            updCreditAmountIn.Update();
            CalculateAmt();
        }
        protected void rpt_list_ItemCommand(object source, RepeaterCommandEventArgs e) //not using now
        {
            if (e.CommandName == "Collect")
            {
                HiddenField hdn_id = (HiddenField)e.Item.FindControl("hdn_id");
                HiddenField hdnTypeId = (HiddenField)e.Item.FindControl("hdnTypeId");
                RadComboBox drpBankAccount = (RadComboBox)e.Item.FindControl("drpBankAccount");
                RadDatePicker Carddate = (RadDatePicker)e.Item.FindControl("Carddate");
                TextBox txtBankCommission = (TextBox)e.Item.FindControl("txtBankCommission");
                TextBox txtCommissionVat = (TextBox)e.Item.FindControl("txtCommissionVat");

                int res = 0;
                if (drpBankAccount.SelectedValue == "")
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Select Bank Account !');", true);
                else if (Carddate.SelectedDate == null)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Select Date !');", true);
                else
                    res = BalVoucher.CollectCardRCRV(Convert.ToInt32(hdn_id.Value), Carddate.SelectedDate, Convert.ToInt32(drpBankAccount.SelectedValue),
                       Convert.ToInt32(hdnTypeId.Value), Convert.ToInt32(hdn_user_id.Value),
                       (txtBankCommission.Text == "" ? 0 : Convert.ToDecimal(txtBankCommission.Text)),
                       (txtCommissionVat.Text == "" ? 0 : Convert.ToDecimal(txtCommissionVat.Text)));

                if (res > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Saved Successfully !..');", true);
                    FillList();
                }
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Sorry Failed to Process Your Request !.');", true);

            }
        }

        protected void btnProceed_Click(object sender, EventArgs e)
        {
            int Cnt = 0;
            foreach (RepeaterItem itm in rpt_list.Items)
            {
                HiddenField hdn_id = (HiddenField)itm.FindControl("hdn_id");
                HiddenField hdnTypeId = (HiddenField)itm.FindControl("hdnTypeId");
                RadComboBox drpBankAccount = (RadComboBox)itm.FindControl("drpBankAccount");
                RadDatePicker Carddate = (RadDatePicker)itm.FindControl("Carddate");
                CheckBox chkselectIn = (CheckBox)itm.FindControl("chkselectIn");
                TextBox txtBankCommission = (TextBox)itm.FindControl("txtBankCommission");
                TextBox txtCommissionVat = (TextBox)itm.FindControl("txtCommissionVat");

                if (chkselectIn.Checked)
                {
                    Cnt++;
                    int res = 0;
                    if (drpBankAccount.SelectedValue == "")
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Select Bank Account !');", true);
                    else if (Carddate.SelectedDate == null)
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Select Date !');", true);
                    else
                        res = BalVoucher.CollectCardRCRV(Convert.ToInt32(hdn_id.Value), Carddate.SelectedDate, Convert.ToInt32(drpBankAccount.SelectedValue),
                           Convert.ToInt32(hdnTypeId.Value), Convert.ToInt32(hdn_user_id.Value),
                             (txtBankCommission.Text == "" ? 0 : Convert.ToDecimal(txtBankCommission.Text)),
                             (txtCommissionVat.Text == "" ? 0 : Convert.ToDecimal(txtCommissionVat.Text)));
                }
            }
            if (Cnt == 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Select an entry to proceed !');", true);
            else
                Clear();
        }

        protected void rpt_list_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Button btnCollect = (Button)e.Item.FindControl("btnCollect");
            //btnCollect.Visible = hdn_add.Value == "0" ? false : true;
            RadComboBox drpBankAccount = (RadComboBox)e.Item.FindControl("drpBankAccount");
            HiddenField hdnAccountId = (HiddenField)e.Item.FindControl("hdnAccountId");
            HiddenField hdnstatus = (HiddenField)e.Item.FindControl("hdnstatus");
            RadDatePicker Carddate = (RadDatePicker)e.Item.FindControl("Carddate");
            Label lblcarddate = (Label)e.Item.FindControl("lbldate");

            drpBankAccount.DataSource = BalVoucher.GetBankAccountList(Convert.ToInt32(hdn_user_id.Value));
            drpBankAccount.DataValueField = "Value";
            drpBankAccount.DataTextField = "Text";
            drpBankAccount.DataBind();

            drpBankAccount.SelectedValue = hdnAccountId.Value;
            Carddate.SelectedDate = DateTime.Now;
            CheckBox chkselectIn = (CheckBox)e.Item.FindControl("chkselectIn");
            CheckBox chkselectall = (CheckBox)rpt_list.Parent.FindControl("chkselectall");
            if (hdnstatus.Value == "1") { chkselectIn.Enabled = false; chkselectall.Enabled = false; Carddate.Visible = false;lblcarddate.Visible = true; }
            else
            {
                chkselectIn.Enabled = true; chkselectall.Enabled = true;
                Carddate.Visible = true;
                lblcarddate.Visible = false;
            }
        }

        public void CalculateAmt()
        {
            decimal Tot = 0;
            foreach(RepeaterItem itm in rpt_list.Items)
            {
                //HiddenField hdnCreditAmount = (HiddenField)itm.FindControl("hdnCreditAmount");
                Label lblCreditAmount = (Label)itm.FindControl("lblCreditAmount");
                CheckBox chkselectIn = (CheckBox)itm.FindControl("chkselectIn");
                if (chkselectIn.Checked)
                    Tot = Tot + Convert.ToDecimal(lblCreditAmount.Text);
            }
            txtTotal.Text = Tot.ToString("0.00");
            updtotal.Update();
        }

        public void Clear()
        {
            txtTotal.Text = "0.00";
            chkselectall.Checked = false;
            btnProceed.Visible = hdn_add.Value == "1" ? true : false;

            FillList();
            Upd_List_Panel.Update();
        }
      
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.Form_Previlage_Validation(117, Convert.ToInt32(hdn_user_id.Value));
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

        public void previlage_action_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    DataTable dt = obj_common.Action_Previlage_Validation(117, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                        hdn_add.Value = dt.Rows[0][1].ToString();
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

        protected void drpStatus_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            FillList();
        }
    }
}