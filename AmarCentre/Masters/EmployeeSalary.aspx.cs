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

namespace AmarCentre.Masters
{
    public partial class EmployeeSalary : System.Web.UI.Page
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
                Clear();
                grid_fill(1, 10, "", "", "");
            }
        }

        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_master.Get_List_Employee_Salary(page_number, page_size, filter, column, order);
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
            DataTable dt = obj_master.Get_List_Employee_Salary_Excel();
            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "SalaryType");

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
            DataSet ds = obj_master.Edit_Employee_Salary(Convert.ToInt32(hdn_rpt_id.Value));
            DataTable dt1 = ds.Tables[0];/*Quot*/
            DataTable dt2 = ds.Tables[1];/* Detail*/

            hdn_id.Value = dt1.Rows[0]["Id"].ToString();
            lbl_Code.Text = dt1.Rows[0]["Code"].ToString();
            fill_Employee_Edit(Convert.ToInt32(dt1.Rows[0]["EmployeeId"].ToString()));
            drp_empl.SelectedValue = dt1.Rows[0]["EmployeeId"].ToString();
            txt_tot_amt.Text = dt1.Rows[0]["Total_Salary"].ToString();

            rpt_Item_list.DataSource = dt2;
            rpt_Item_list.DataBind();

            btn_save.Visible = hdn_update.Value == "0" ? false : true;
            btn_delete.Visible = hdn_delete.Value == "0" ? false : true;
            pnl_add.Visible = true;
            Upd_Add_Panel.Update();
        }

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            DataTable dt_deatils = fill_Detail();

            int res = 0;
            if (dt_deatils.Rows.Count > 0)
            {
                res = obj_master.Insert_Update_Employee_Salary(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(drp_empl.SelectedValue),
                    Convert.ToDecimal(txt_tot_amt.Text), dt_deatils, Convert.ToInt32(hdn_user_id.Value));
            }
            if (res > 0)
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
            int res = obj_master.Delete_Employee_Salary(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                lbl_msg.Text = "Deleted Successfully !..";
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

        /*Data To Save*/
        public DataTable fill_Detail()
        {
            DataTable dt_job = new DataTable();
            dt_job.Columns.Add("D_id", typeof(int));
            dt_job.Columns.Add("SalaryId", typeof(int));
            dt_job.Columns.Add("Amount", typeof(decimal));

            if (rpt_Item_list.Items.Count > 0)
            {
                foreach (RepeaterItem Item in rpt_Item_list.Items)
                {
                    HiddenField hdn_D_Id = (HiddenField)Item.FindControl("hdn_D_Id");
                    HiddenField hdn_salary_id = (HiddenField)Item.FindControl("hdn_salary_id");
                    TextBox txt_Amount = (TextBox)Item.FindControl("txt_Amount");

                    dt_job.Rows.Add(Convert.ToInt32(hdn_D_Id.Value), Convert.ToInt32(hdn_salary_id.Value),
                            Convert.ToDecimal(txt_Amount.Text));
                }
            }
            return dt_job;
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

        public void fill_Salary()
        {
            DataTable dt2 = obj_master.List_Salary();
            rpt_Item_list.DataSource = dt2;
            rpt_Item_list.DataBind();
        }

        /*Employee*/
        public void fill_Employee()
        {
            drp_empl.Items.Clear();
            DataTable dt = obj_master.Drp_Employee_sal();
            drp_empl.DataSource = dt;
            drp_empl.DataTextField = "Text";
            drp_empl.DataValueField = "Value";
            drp_empl.DataBind();
        }
        /*Employee*/
        public void fill_Employee_Edit(int CurrentEmpId)
        {
            drp_empl.Items.Clear();
            DataTable dt = obj_master.Drp_EmployeeSalaryEdit(CurrentEmpId);
            drp_empl.DataSource = dt;
            drp_empl.DataTextField = "Text";
            drp_empl.DataValueField = "Value";
            drp_empl.DataBind();
        }

        public void Clear()
        {
            hdn_id.Value = "0";
            fill_Employee();
            drp_empl.ClearSelection();
            drp_empl.Text = "";
            txt_tot_amt.Text = "0.00";
            fill_Salary();

            btn_save.Visible = hdn_add.Value == "0" ? false : true;
            btn_delete.Visible = false;
            Get_Code();

            Upd_Add_PanelInner.Update();
        }

        /*Code for Display*/
        public void Get_Code()
        {
            DataTable dt = obj_common.Get_Code(12);
            if (dt.Rows.Count > 0)
                lbl_Code.Text = dt.Rows[0][0].ToString();
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

                    int val = obj_common.Form_Previlage_Validation(12, Convert.ToInt32(hdn_user_id.Value));
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

                    DataTable dt = obj_common.Action_Previlage_Validation(12, Convert.ToInt32(hdn_user_id.Value));
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