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
    public partial class Service : System.Web.UI.Page
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
                fill_Department();
                grid_fill(1, 10, "", "", "");
            }
        }

        public void fill_Department()
        {
            drpDerpartSearch.Items.Clear();
            DataTable dt = obj_master.Drp_Department();
            drpDerpartSearch.DataSource = dt;
            drpDerpartSearch.DataTextField = "Text";
            drpDerpartSearch.DataValueField = "Value";
            drpDerpartSearch.DataBind();
        }

        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_master.Get_List_Service(page_number, page_size, filter, column, order, drpDerpartSearch.SelectedValue == "" ? (int?)null :
                Convert.ToInt32(drpDerpartSearch.SelectedValue));
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
            DataTable dt = obj_master.Get_List_Service_Excel( drpDerpartSearch.SelectedValue == "" ? (int?)null :
                Convert.ToInt32(drpDerpartSearch.SelectedValue));
            dt.Columns["RowNum"].ColumnName = "Sl No.";
            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "ServiceList");

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
            DataSet ds = obj_master.Edit_Service(Convert.ToInt32(hdn_rpt_id.Value));

            UC_Service.UCPageLoad( 1, Convert.ToInt32(hdn_rpt_id.Value));

            pnl_add.Visible = true;
            Upd_Add_Panel.Update();
        }

        public void DisplayMsg(int res)
        {
            if (res >0)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
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

        protected void btn_newentry_OnClick(object sender, EventArgs e)
        {
            UC_Service.UCPageLoad(1,0);

            pnl_add.Visible = true;
            Upd_Add_Panel.Update();
        }

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

                    int val = obj_common.Form_Previlage_Validation(14, Convert.ToInt32(hdn_user_id.Value));
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