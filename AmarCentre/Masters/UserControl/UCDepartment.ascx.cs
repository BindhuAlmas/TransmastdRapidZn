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
    public partial class UCDepartment : System.Web.UI.UserControl
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
            int res = obj_master.Insert_Update_Department(0, txt_name.Text, txt_desc.Text,
                 Convert.ToInt32(hdn_user_id.Value), txtArabicName.Text);

            RadComboBox drpDepartment = (RadComboBox)this.Parent.FindControl("drpDepartment");
            drpDepartment.ClearSelection();
            drpDepartment.Text = "";
            if (res > 0)
            {
                drpDepartment.Items.Clear();
                DataTable dt = obj_master.Drp_Department();
                drpDepartment.DataSource = dt;
                drpDepartment.DataTextField = "text";
                drpDepartment.DataValueField = "value";
                drpDepartment.DataBind();

                RadComboBoxItem CodeItem = new RadComboBoxItem();
                CodeItem.Text = "New Entry";
                CodeItem.Value = "0";
                drpDepartment.Items.Insert(0, CodeItem);

                drpDepartment.SelectedValue = res.ToString();

                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                drpDepartment.ClearSelection();
                drpDepartment.Text = "";
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }

            Panel pnlDepartment = (Panel)this.Parent.FindControl("pnlDepartment");
            UpdatePanel UpdDepartmentPanel = (UpdatePanel)this.Parent.FindControl("UpdDepartmentPanel");
            UpdatePanel UpdDepartmentDrop_Panel = (UpdatePanel)this.Parent.FindControl("UpdDepartmentDrop_Panel");
            UpdDepartmentPanel.Update();
            pnlDepartment.Visible = false;
            UpdDepartmentDrop_Panel.Update();
        }

        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
        }
        protected void btn_close_OnClick(object sender, EventArgs e)
        {
            RadComboBox drpDepartment = (RadComboBox)this.Parent.FindControl("drpDepartment");
            drpDepartment.ClearSelection();
            drpDepartment.Text = "";

            Panel pnlDepartment = (Panel)this.Parent.FindControl("pnlDepartment");
            UpdatePanel UpdDepartmentPanel = (UpdatePanel)this.Parent.FindControl("UpdDepartmentPanel");
            UpdatePanel UpdDepartmentDrop_Panel = (UpdatePanel)this.Parent.FindControl("UpdDepartmentDrop_Panel");
            UpdDepartmentPanel.Update();
            pnlDepartment.Visible = false;
            UpdDepartmentDrop_Panel.Update();
        }

        public void Clear()
        {
            txt_name.Text = "";
            txt_desc.Text = "";
            hdn_id.Value = "0";
            txtArabicName.Text = "";

            btn_save.Visible = hdn_add.Value == "0" ? false : true;

            Upd_Add_PanelInner.Update();
        }

        public void previlage_action_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    DataTable dt = obj_common.Action_Previlage_Validation(32, Convert.ToInt32(hdn_user_id.Value));
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