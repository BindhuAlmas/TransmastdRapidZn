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

namespace AmarCentre.Masters
{
    public partial class Templates : System.Web.UI.Page
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
                FillDropdowns();
                Clear();
                grid_fill(1, 10, "", "", "");
            }
        }

        public void FillDropdowns()
        {
            drpServices.Items.Clear();
            DataTable dt = obj_master.GetService();
            drpServices.DataSource = dt;
            drpServices.DataTextField = "Text";
            drpServices.DataValueField = "Value";
            drpServices.DataBind();

            drpDepartment.Items.Clear();
            drpDepartment.DataSource = obj_master.Drp_Department();
            drpDepartment.DataTextField = "Text";
            drpDepartment.DataValueField = "Value";
            drpDepartment.DataBind();
        }

        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_master.Get_List_Templates(page_number, page_size, filter, column, order);
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

        public void btnexcel_export_OnClick(object sender, EventArgs e)
        {
            DataTable dt = obj_master.Get_List_Templates_Excel();
            dt.Columns["Sl_No"].ColumnName = "Sl No.";
            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "Templates");

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
            Clear();
            HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdn_id");
            DataSet ds = obj_master.Edit_Templates(Convert.ToInt32(hdn_rpt_id.Value));
            DataTable dt = ds.Tables[0];
            DataTable dtServices = ds.Tables[1];
            txt_name.Text = dt.Rows[0]["Name"].ToString();
            txt_desc.Text = dt.Rows[0]["Description"].ToString();

            hdn_id.Value = dt.Rows[0][0].ToString();

            foreach (DataRow dr in dtServices.Rows)
            {
                RadComboBoxItem item = (RadComboBoxItem)(drpServices.FindItemByValue(dr["ServiceId"].ToString()));
                item.Checked = true;
                item.Selected = true;
            }

            drpServicesSelect.DataSource = dtServices;
            drpServicesSelect.DataBind();

            foreach (RadComboBoxItem item in drpServicesSelect.Items)
            {
                HiddenField hdnSId = (HiddenField)item.FindControl("hdnSId");
                foreach (DataRow dr in dtServices.Rows)
                {
                    if (dr["ServiceId"].ToString() == hdnSId.Value)
                    {
                        item.Checked = true;
                        item.Selected = true;
                    }
                }
            }

            btn_delete.Visible = hdn_delete.Value == "0" ? false : true;
            btn_save.Visible = hdn_update.Value == "0" ? false : true;
            pnl_add.Visible = true;
            Upd_Add_Panel.Update();
        }

        protected void drpChangeOnSelectedIndexChanged(object sender, EventArgs e)
        {
            drpServices.Text = string.Empty;
            drpServices.ClearCheckedItems();

            DataTable dt = obj_master.DrpServicebyDepartment(drpDepartment.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpDepartment.SelectedValue));
            drpServices.DataSource = dt;
            drpServices.DataTextField = "Name";
            drpServices.DataValueField = "Id";
            drpServices.DataBind();

            updService.Update();
        }

        protected void drpServiceOnSelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtService = new DataTable();
            dtService.Columns.Add("ServiceId", typeof(int));
            dtService.Columns.Add("Text", typeof(string));
            dtService.Columns.Add("Orderby", typeof(string));

            foreach (RadComboBoxItem item in drpServicesSelect.Items)
            {
                if (item.Checked)
                {
                    TextBox txtSOrderby = (TextBox)item.FindControl("txtSOrderby");
                    HiddenField hdnSname = (HiddenField)item.FindControl("hdnSname");
                    HiddenField hdnSId = (HiddenField)item.FindControl("hdnSId");
                    dtService.Rows.Add(Convert.ToInt32(hdnSId.Value), hdnSname.Value, txtSOrderby.Text);
                }
            }

            foreach (RadComboBoxItem item in drpServices.Items)
            {
                if (item.Checked)
                {
                    int x = 0;
                    foreach (DataRow r in dtService.Rows)
                    {
                        if (r["ServiceId"].ToString() == item.Value.ToString())
                        {
                            x = 1;
                            break;
                        }
                    }
                    if (x == 0)
                        dtService.Rows.Add(Convert.ToInt32(item.Value), item.Text,"");
                }
            }

            drpServicesSelect.DataSource = dtService;
            drpServicesSelect.DataBind();
           
            foreach (RadComboBoxItem item in drpServicesSelect.Items)
            {
                HiddenField hdnSId = (HiddenField)item.FindControl("hdnSId");
                foreach (DataRow dr in dtService.Rows)
                {
                    if (dr["ServiceId"].ToString() == hdnSId.Value)
                    {
                        item.Checked = true;
                        item.Selected = true;
                    }
                }
            }
            updServiceSelect.Update();
        }

        protected void RadComboBox1_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            DataRowView dataItem = (DataRowView)e.Item.DataItem;
            e.Item.Attributes["ServiceId"] = dataItem["ServiceId"].ToString();
            e.Item.Attributes["Orderby"] = dataItem["Orderby"].ToString();
            e.Item.Attributes["Text"] = dataItem["Text"].ToString();
        }

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            DataTable dtService = new DataTable();
            dtService.Columns.Add("ServiceId", typeof(int));
            dtService.Columns.Add("Orderby", typeof(int));

            foreach (RadComboBoxItem item in drpServicesSelect.Items)
            {
                if (item.Checked)
                {
                    TextBox txtSOrderby = (TextBox)item.FindControl("txtSOrderby");
                    HiddenField hdnSId = (HiddenField)item.FindControl("hdnSId");
                    dtService.Rows.Add(Convert.ToInt32(hdnSId.Value), txtSOrderby.Text == "" ? (int?)null : Convert.ToInt32(txtSOrderby.Text));
                }
            }

            int res = obj_master.Insert_Update_Templates(Convert.ToInt32(hdn_id.Value), txt_name.Text, dtService, txt_desc.Text,
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
                lblerrormsg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
            }
            pnl_add.Visible = false;
            Upd_Add_Panel.Update();
        }

        protected void btn_delete_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Delete_Templates(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                lbl_msg.Text = "Deleted Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lblerrormsg.Text = "Unable to delete. Entry may be used in transaction Page !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
            }
            pnl_add.Visible = false;
            Upd_Add_Panel.Update();
        }

        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
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
        public void Clear()
        {
            txt_name.Text = "";
            txt_desc.Text = "";
            hdn_id.Value = "0";
            drpServices.Text = string.Empty;
            drpServices.ClearCheckedItems();
            btn_delete.Visible = false;
            drpServicesSelect.Text = string.Empty;
            drpServicesSelect.ClearCheckedItems();
            drpServicesSelect.Items.Clear();
            drpDepartment.ClearSelection();
            drpDepartment.Text = "";
            btn_save.Visible = hdn_add.Value == "0" ? false : true;

            Upd_Add_PanelInner.Update();
        }
        //Search
        protected void txt_search_OnTextChanged(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), txt_search.Text, Common_order_column.Value, Common_asc_desc.Value);
            Upd_List_Panel.Update();
        }

        #region Navigation

        //First Page
        protected void btn_first_OnClick(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
            Upd_List_Panel.Update();
        }

        //Previous Page
        protected void btn_prev_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number.Text) > 1)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text) - 1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Upd_List_Panel.Update();
            }
        }

        //Next Page
        protected void btn_next_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number.Text) < Convert.ToInt32(hdn_last_page.Value))
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text) + 1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Upd_List_Panel.Update();
            }
        }

        //Last Page
        protected void btn_last_OnClick(object sender, EventArgs e)
        {

            grid_fill(Convert.ToInt32(hdn_last_page.Value), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
            Upd_List_Panel.Update();
        }

        //Page Data Count
        protected void drp_count_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
            Upd_List_Panel.Update();
        }

        #endregion

        //Check Privilege
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.Form_Previlage_Validation(52, Convert.ToInt32(hdn_user_id.Value));
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

                    DataTable dt = obj_common.Action_Previlage_Validation(52, Convert.ToInt32(hdn_user_id.Value));
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