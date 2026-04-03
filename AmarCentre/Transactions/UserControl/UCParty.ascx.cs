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
    public partial class UCParty : System.Web.UI.UserControl
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
            hdnPageId.Value = PageId.ToString();  //1-RV, 
            previlage_action_check();
            Clear();
        }

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Insert_UpdateParty(0, txt_name.Text,txt_address.Text, txt_mob.Text, txt_email.Text, Convert.ToInt32(hdn_user_id.Value));

            if (hdnPageId.Value == "1")
            {
                RadComboBox drpParty = (RadComboBox)this.Parent.FindControl("drpParty");
                drpParty.ClearSelection();
                drpParty.Text = "";
                if (res > 0)
                {
                    drpParty.Items.Clear();
                    drpParty.DataSource = BalVoucher.fillParty();
                    drpParty.DataTextField = "text";
                    drpParty.DataValueField = "value";
                    drpParty.DataBind();

                    RadComboBoxItem CodeItem = new RadComboBoxItem();
                    CodeItem.Text = "New Entry";
                    CodeItem.Value = "0";
                    drpParty.Items.Insert(0, CodeItem);

                    drpParty.SelectedValue = res.ToString();

                    Clear();
                    lbl_msg.Text = "Saved Successfully !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                }
                else
                {
                    drpParty.ClearSelection();
                    drpParty.Text = "";
                    lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                }
                Panel PartyPanel = (Panel)this.Parent.FindControl("PartyPanel");
                UpdatePanel updPartyPanel = (UpdatePanel)this.Parent.FindControl("updPartyPanel");
                UpdatePanel UpdFrom = (UpdatePanel)this.Parent.FindControl("UpdFrom");
                PartyPanel.Visible = false;
                updPartyPanel.Update();
                UpdFrom.Update();
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
                RadComboBox drpParty = (RadComboBox)this.Parent.FindControl("drpParty");
                drpParty.ClearSelection();
                drpParty.Text = "";

                Panel PartyPanel = (Panel)this.Parent.FindControl("PartyPanel");
                UpdatePanel updPartyPanel = (UpdatePanel)this.Parent.FindControl("updPartyPanel");
                UpdatePanel UpdFrom = (UpdatePanel)this.Parent.FindControl("UpdFrom");
                updPartyPanel.Update();
                PartyPanel.Visible = false;
                UpdFrom.Update();
            }
        }

        public void Clear()
        {
            txt_name.Text = "";
            txt_address.Text = "";
            txt_mob.Text = "";
            txt_email.Text = "";
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
                    DataTable dt = obj_common.Action_Previlage_Validation(125, Convert.ToInt32(hdn_user_id.Value));
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