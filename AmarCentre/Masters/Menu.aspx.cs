using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AmarCentre.BAL;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;

namespace AmarCentre.Masters
{
    public partial class Menu : System.Web.UI.Page
    {
        Master_Bal obj_master = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
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
                Get_Main_Menu();
                drp_mainmenu();
            }
        }

        //Main Menu List
        public void Get_Main_Menu()
        {
            DataTable dt = obj_master.Get_Main_Menu();
            rpt_main_menu.DataSource = dt;
            rpt_main_menu.DataBind();
        }

        public void drp_mainmenu()
        {
            drp_mainmenuu.Items.Clear();
            DataTable dt = obj_master.drp_Get_Main_Menu();
            drp_mainmenuu.DataSource = dt;
            drp_mainmenuu.DataTextField = "text";
            drp_mainmenuu.DataValueField = "value";
            drp_mainmenuu.DataBind();
        }

        //rpt DataBound
        protected void rpt_main_menu_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField hdn_main = (HiddenField)e.Item.FindControl("hdn_main");
            DataTable dt = obj_master.Get_Sub_Menu(Convert.ToInt32(hdn_main.Value));
            Repeater rpt_sub_menu = (Repeater)e.Item.FindControl("rpt_sub_menu");
            rpt_sub_menu.DataSource = dt;
            rpt_sub_menu.DataBind();
        }

        //Expand Main Menu
        protected void btn_expand_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            Button btn_expand = (Button)itemrp.FindControl("btn_expand");
            Button btn_collapse = (Button)itemrp.FindControl("btn_collapse");
            HtmlGenericControl div_sub_menu = (HtmlGenericControl)itemrp.FindControl("div_sub_menu");

            btn_expand.Visible = false;
            btn_collapse.Visible = true;
            div_sub_menu.Visible = true;

            Upd_List_Panel.Update();
        }

        //Collapse Main Menu
        protected void btn_collapse_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            Button btn_expand = (Button)itemrp.FindControl("btn_expand");
            Button btn_collapse = (Button)itemrp.FindControl("btn_collapse");
            HtmlGenericControl div_sub_menu = (HtmlGenericControl)itemrp.FindControl("div_sub_menu");

            btn_expand.Visible = true;
            btn_collapse.Visible = false;
            div_sub_menu.Visible = false;

            Upd_List_Panel.Update();
        }

        //Display Add Main Menu
        protected void btn_new_line_OnClick(object sender, EventArgs e)
        {
            div_main_edit.Visible = true;
            div_sub_edit.Visible = false;
            btn_delete_M.Visible = false;
            txt_main_menu.Text = "";
            txt_main_display_order.Text = "";
            hdn_add_M.Value = "0";

            btn_save_M.Visible = hdn_add_MM.Value == "0" ? false : true;
            pnl_add.Visible = true;

            Upd_Add_Panel.Update();
        }

        //Display Edit Main Menu
        protected void btn_menu_edit_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            Label lbl_main = (Label)itemrp.FindControl("lbl_main");
            HiddenField hdn_main = (HiddenField)itemrp.FindControl("hdn_main");
            HiddenField hdn_main_DO = (HiddenField)itemrp.FindControl("hdn_main_DO");

            div_main_edit.Visible = true;
            div_sub_edit.Visible = false;

            txt_main_menu.Text = lbl_main.Text;
            txt_main_display_order.Text = hdn_main_DO.Value;
            hdn_add_M.Value = hdn_main.Value;

            btn_save_M.Visible = hdn_update_MM.Value == "0" ? false : true;
            btn_delete_M.Visible = hdn_delete_MM.Value == "0" ? false : true;
            pnl_add.Visible = true;

            Upd_Add_Panel.Update();
        }

        //Close Add Main Menu
        protected void btn_close_main_menu_OnClick(object sender, EventArgs e)
        {
            div_main_edit.Visible = false;
            btn_delete_M.Visible = false;
            txt_main_menu.Text = "";
            txt_main_display_order.Text = "";
            hdn_add_M.Value = "0";
            pnl_add.Visible = false;

            Upd_Add_Panel.Update();
        }

        //Save Main Menu Button
        protected void btn_save_M_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Insert_Update_Main_Menu(Convert.ToInt32(hdn_add_M.Value), txt_main_menu.Text, Convert.ToInt32(txt_main_display_order.Text),
                Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                Get_Main_Menu();
                div_main_edit.Visible = false;
                Upd_List_Panel.Update();
                Clear();
                drp_mainmenu();
                UpdMainMenuPanel.Update();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            pnl_add.Visible = false;

            Upd_Add_Panel.Update();
        }

        //Delete Main Menu Button
        protected void btn_delete_M_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Delete_Main_Menu(Convert.ToInt32(hdn_add_M.Value), Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                Get_Main_Menu();
                div_main_edit.Visible = false;                
                Upd_List_Panel.Update();
                Clear();
                drp_mainmenu();
                UpdMainMenuPanel.Update();
                lbl_msg.Text = "Deleted Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            pnl_add.Visible = false;

            Upd_Add_Panel.Update();
        }

        //Display Add Sub Menu
        protected void btn_add_sub_menu_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;
            HiddenField hdn_main = (HiddenField)itemrp.FindControl("hdn_main");
            drp_mainmenuu.SelectedValue = hdn_main.Value;
            div_sub_edit.Visible = true;
            div_main_edit.Visible = false;
            btn_delete_S.Visible = false;
            txt_sub_menu.Text = "";
            txt_sub_dest.Text = "";
            txt_display_order.Text = "";
            hdn_add_S.Value = "0";
            hdn_add_S_Main.Value = hdn_main.Value;

            btn_save_S.Visible = hdn_add_SM.Value == "0" ? false : true;
            pnl_add.Visible = true;

            Upd_Add_Panel.Update();
        }

        //Display Edit Sub Menu
        protected void btn_sub_menu_edit_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            HiddenField hdn_sub = (HiddenField)itemrp.FindControl("hdn_sub");

            DataTable dt = obj_master.Edit_Sub_Menu(Convert.ToInt32(hdn_sub.Value));
            div_sub_edit.Visible = true;
            div_main_edit.Visible = false;
            txt_sub_menu.Text = dt.Rows[0]["MenuName"].ToString();
            txt_sub_dest.Text = dt.Rows[0]["Destination"].ToString();
            txt_display_order.Text = dt.Rows[0]["OrderBy"].ToString();
            hdn_add_S.Value = dt.Rows[0]["Id"].ToString();
            hdn_add_S_Main.Value = dt.Rows[0]["MenuId"].ToString();
            drp_mainmenuu.SelectedValue = hdn_add_S_Main.Value;
            btn_save_S.Visible = hdn_update_SM.Value == "0" ? false : true;
            btn_delete_S.Visible = hdn_delete_SM.Value == "0" ? false : true;
            pnl_add.Visible = true;

            Upd_Add_Panel.Update();
        }

        //Close Add Main Menu
        protected void btn_close_sub_menu_OnClick(object sender, EventArgs e)
        {
            div_sub_edit.Visible = false;
            btn_delete_S.Visible = false;
            txt_sub_menu.Text = "";
            txt_sub_dest.Text = "";
            txt_display_order.Text = "";
            hdn_add_S.Value = "0";
            hdn_add_S_Main.Value = "0";
            pnl_add.Visible = false;

            Upd_Add_Panel.Update();
        }

        //Save Sub Menu Button
        protected void btn_save_S_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Insert_Update_Sub_Menu(Convert.ToInt32(hdn_add_S.Value), Convert.ToInt32(drp_mainmenuu.SelectedValue),
                       txt_sub_menu.Text, txt_sub_dest.Text, Convert.ToInt32(txt_display_order.Text), Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                Get_Main_Menu();
                div_sub_edit.Visible = false;
                Upd_List_Panel.Update();
                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            pnl_add.Visible = false;
            Upd_Add_Panel.Update();
        }

        //Delete Sub Menu Button
        protected void btn_delete_S_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Delete_Sub_Menu(Convert.ToInt32(hdn_add_S.Value), Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                Get_Main_Menu();
                div_sub_edit.Visible = false;               
                Upd_List_Panel.Update();
                Clear();
                lbl_msg.Text = "Deleted Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            pnl_add.Visible = false;

            Upd_Add_Panel.Update();
        }

        //Clear Data
        public void Clear()
        {
            txt_main_menu.Text = "";
            txt_main_display_order.Text = "";
            drp_mainmenuu.ClearSelection();
            hdn_add_M.Value = "0";
            txt_sub_menu.Text = "";
            txt_sub_dest.Text = "";
            txt_display_order.Text = "";
            hdn_add_S.Value = "0";
            hdn_add_S_Main.Value = "0";
        }

        //Check Form Privilege
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.Form_Previlage_Validation(1, Convert.ToInt32(hdn_user_id.Value));
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

        //Check Action Privilege
        public void previlage_action_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    DataTable dt = obj_common.Action_Previlage_Validation(1, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add_MM.Value = dt.Rows[0][1].ToString();
                        hdn_update_MM.Value = dt.Rows[1][1].ToString();
                        hdn_delete_MM.Value = dt.Rows[2][1].ToString();
                        hdn_add_SM.Value = dt.Rows[3][1].ToString();
                        hdn_update_SM.Value = dt.Rows[4][1].ToString();
                        hdn_delete_SM.Value = dt.Rows[5][1].ToString();
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