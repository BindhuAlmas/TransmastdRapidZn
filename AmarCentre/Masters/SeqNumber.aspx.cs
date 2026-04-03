using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using AmarCentre.BAL;

namespace AmarCentre.Masters
{
    public partial class SeqNumber : System.Web.UI.Page
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
                fill_submenu();
                Clear();
                grid_fill(1, 10, "", "", "");
            }
        }

        //Get List 
        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_master.Get_List_Sequence(page_number, page_size, filter, column, order);
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
            Upd_Nav_Panel.Update();
            Upd_List_Panel.Update();
        }

        //exel export
        public void btnexcel_export_OnClick(object sender, EventArgs e)
        {
            DataTable dt = obj_master.Get_List_Sequence_Excel();
            if (dt.Rows.Count > 0)
            {
                dt.Columns["RowNum"].ColumnName = "Sl No";
                dt.Columns["Lengths"].ColumnName = "Mini. Digit";
                dt.Columns.Remove("Id");

                dt.Columns.Remove("FormId");
                dt.Columns.Remove("Prefix");
                dt.Columns.Remove("Seperator");
                dt.Columns.Remove("Current_no");

                dt.Columns.Remove("Created_by");
                dt.Columns.Remove("Created_date");
                dt.Columns.Remove("Updated_By");
                dt.Columns.Remove("Updated_Date");
                StringWriter sw = obj_common.ExportToExcel(dt, "Sequence");

                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                HttpContext.Current.Response.Write(style);
                Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        //rpt Command
        protected void rpt_list_OnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            Clear();
            HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdn_id");
            DataTable dt = obj_master.Edit_Sequence(Convert.ToInt32(hdn_rpt_id.Value));
            txt_category.Text = dt.Rows[0]["Category"].ToString();
            txt_prefix.Text = dt.Rows[0]["Prefix"].ToString();
            txt_seperator.Text = dt.Rows[0]["Seperator"].ToString();
            txt_current_no.Text = dt.Rows[0]["Current_No"].ToString();
            txt_increment.Text = dt.Rows[0]["Incrementer"].ToString();
            drp_menus.SelectedValue = dt.Rows[0]["FormId"].ToString();
            txt_moni_dig.Text = dt.Rows[0]["Lengths"].ToString();
            hdn_id.Value = dt.Rows[0][0].ToString();

            btn_save.Visible = hdn_update.Value == "0" ? false : true;
            btn_delete.Visible = hdn_delete.Value == "0" ? false : true;
            pnl_add.Visible = true;
            Upd_Add_Panel.Update();
        }

        //Save Button
        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            int? menu_id;
            try
            {
                menu_id = Convert.ToInt32(drp_menus.SelectedValue);
            }
            catch
            {
                menu_id = null;
            }
            int res = obj_master.Insert_Update_Sequence(Convert.ToInt32(hdn_id.Value), txt_category.Text, txt_prefix.Text,
                txt_seperator.Text, Convert.ToInt32(txt_current_no.Text), Convert.ToInt32(txt_increment.Text), menu_id,
                txt_moni_dig.Text == "" ? 4: Convert.ToInt32(txt_moni_dig.Text),
                Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
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

        protected void btn_delete_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Delete_Sequence(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
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
        //Reset Button
        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
        }

        public void fill_submenu()
        {
            drp_menus.Items.Clear();
            DataTable dt = obj_master.Get_Sub_Menu();
            drp_menus.DataSource = dt;
            drp_menus.DataTextField = "Text";
            drp_menus.DataValueField = "Value";
            drp_menus.DataBind();
        }
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
        //Clear all Data
        public void Clear()
        {
            txt_category.Text = "";
            txt_prefix.Text = "";
            hdn_id.Value = "0";
            txt_seperator.Text = "";
            txt_current_no.Text = "";
            txt_increment.Text = "";
            drp_menus.Text = "";
            drp_menus.ClearSelection();
            txt_moni_dig.Text = "";

            btn_save.Visible = hdn_add.Value == "0" ? false : true;
            btn_delete.Visible = false;
            Upd_Add_PanelInner.Update();
        }

        //Search
        protected void txt_search_OnTextChanged(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), txt_search.Text, Common_order_column.Value, Common_asc_desc.Value);
        }

        #region Navigation

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

        //Check Form Privilege
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.Form_Previlage_Validation(2, Convert.ToInt32(hdn_user_id.Value));
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

                    DataTable dt = obj_common.Action_Previlage_Validation(2, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdn_update.Value = dt.Rows[1][1].ToString();
                        hdn_delete.Value = dt.Rows[2][1].ToString();
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