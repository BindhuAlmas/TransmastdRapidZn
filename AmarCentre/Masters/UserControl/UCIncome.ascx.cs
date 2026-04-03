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
    public partial class UCIncome : System.Web.UI.UserControl
    {
        Master_Bal obj_master = new Master_Bal();
        Transaction_Bal obj_trans = new Transaction_Bal();
        System_Utilities obj_common = new System_Utilities();

        public void PageLoad()
        {
            hdn_user_id.Value = Session["User_Id"].ToString();
            previlage_action_check();
            Clear();
        }

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Insert_Update_Income(0, txt_name.Text, txt_desc.Text,
                 Convert.ToInt32(hdn_user_id.Value));

            RadComboBox drpIncomeType = (RadComboBox)this.Parent.FindControl("drpIncomeType");
            drpIncomeType.ClearSelection();
            drpIncomeType.Text = "";
            if (res > 0)
            {
                drpIncomeType.Items.Clear();
                DataTable dt = obj_master.Drp_Income();
                drpIncomeType.DataSource = dt;
                drpIncomeType.DataTextField = "text";
                drpIncomeType.DataValueField = "value";
                drpIncomeType.DataBind();

                RadComboBoxItem CodeItem = new RadComboBoxItem();
                CodeItem.Text = "New Entry";
                CodeItem.Value = "0";
                drpIncomeType.Items.Insert(0, CodeItem);

                drpIncomeType.SelectedValue = res.ToString();

                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                drpIncomeType.ClearSelection();
                drpIncomeType.Text = "";
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            Panel pnlIncome = (Panel)this.Parent.FindControl("pnlIncome");
            UpdatePanel UpdIncomePanel = (UpdatePanel)this.Parent.FindControl("UpdIncomePanel");
            UpdatePanel UpdIncomeDrop_Panel = (UpdatePanel)this.Parent.FindControl("UpdIncomeDrop_Panel");
            UpdIncomePanel.Update();
            pnlIncome.Visible = false;
            UpdIncomeDrop_Panel.Update();
        }

        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
        }
        protected void btn_close_OnClick(object sender, EventArgs e)
        {
            RadComboBox drpIncomeType = (RadComboBox)this.Parent.FindControl("drpIncomeType");
            drpIncomeType.ClearSelection();
            drpIncomeType.Text = "";

            Panel pnlIncome = (Panel)this.Parent.FindControl("pnlIncome");
            UpdatePanel UpdIncomePanel = (UpdatePanel)this.Parent.FindControl("UpdIncomePanel");
            UpdatePanel UpdIncomeDrop_Panel = (UpdatePanel)this.Parent.FindControl("UpdIncomeDrop_Panel");
            UpdIncomePanel.Update();
            pnlIncome.Visible = false;
            UpdIncomeDrop_Panel.Update();
        }

        public void Clear()
        {
            txt_name.Text = "";
            txt_desc.Text = "";
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
                    DataTable dt = obj_common.Action_Previlage_Validation(21, Convert.ToInt32(hdn_user_id.Value));
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