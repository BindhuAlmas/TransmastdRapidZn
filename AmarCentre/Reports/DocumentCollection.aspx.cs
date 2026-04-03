using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AmarCentre.BAL;
using System.Data;
using System.Globalization;
using System.IO;
using Telerik.Web.UI;

namespace AmarCentre.Reports
{
    public partial class DocumentCollection : System.Web.UI.Page
    {
        Report_Bal rep1 = new Report_Bal();
        System_Utilities obj_common = new System_Utilities();
        Transaction_Bal obj_trans = new Transaction_Bal();

        public int? CustId, StatusId, Agnt_id;
        public DateTime? from_date;
        public DateTime? To_date;

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
                fill_Customer();
                fill_Agent();
            }
        }
       
        protected void btn_excel_OnClick(object sender, EventArgs e)
        {
            try
            {
                CustId = Convert.ToInt32(drp_cus.SelectedValue);
            }
            catch
            {
                CustId = null;
            }
            StatusId = null;
            try
            {
                Agnt_id = Convert.ToInt32(drp_agent.SelectedValue);
            }
            catch
            {
                Agnt_id = null;
            }

            if (drp_agent.SelectedValue == "")
            {
                DataSet ds = rep1.Document_collectionExcel(CustId, StatusId);
                DataTable dt = ds.Tables[1];
                dt.Columns.Remove("Customer_Id");

                if (dt.Rows.Count > 0)
                {
                    StringWriter sw = obj_common.ExportToExcel(dt, "DocumentCollection");
                    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    HttpContext.Current.Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                }
            }
            else
            {
                DataSet ds = rep1.Document_collectionAgentExcel(CustId, Agnt_id);
                DataTable dt = ds.Tables[1];
                dt.Columns.Remove("Customer_Id");

                if (dt.Rows.Count > 0)
                {
                    StringWriter sw = obj_common.ExportToExcel(dt, "DocumentCollection");
                    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    HttpContext.Current.Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                }
            }
        }

        public void grid_fill(int page_number, int page_size)
        {
            DataSet ds = new DataSet();
            CustId = drp_cus.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_cus.SelectedValue);
            Agnt_id = drp_agent.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_agent.SelectedValue);

            if (drp_agent.SelectedValue == "")
            {
                ds = rep1.Document_collection_List(CustId, StatusId, page_number, page_size);
                DataTable dt = ds.Tables[0];
                rpt_list.DataSource = dt;
                rpt_list.DataBind();
                if (dt.Rows.Count > 0)
                {
                    lbl_page_info.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["Sl_No"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
                    hdn_last_page.Value = dt.Rows[0]["last_page"].ToString();
                    lbl_page_number.Text = dt.Rows[0]["page_number"].ToString();
                    hdn_total.Value = dt.Rows[0]["current_count"].ToString();
                }
                else
                {
                    lbl_page_info.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
                    hdn_last_page.Value = "0";
                    lbl_page_number.Text = "1";
                    hdn_total.Value = "0";
                }
            }
            else
            {
                ds = rep1.Document_collectionAgent_List(CustId, StatusId, page_number, page_size);
                DataTable dt = ds.Tables[0];
                rpt_list.DataSource = dt;
                rpt_list.DataBind();
                if (dt.Rows.Count > 0)
                {
                    lbl_page_info.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["Sl_No"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
                    hdn_last_page.Value = dt.Rows[0]["last_page"].ToString();
                    lbl_page_number.Text = dt.Rows[0]["page_number"].ToString();
                    hdn_total.Value = dt.Rows[0]["current_count"].ToString();
                }
                else
                {
                    lbl_page_info.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
                    hdn_last_page.Value = "0";
                    lbl_page_number.Text = "1";
                    hdn_total.Value = "0";
                }
            }
            Upd_Nav_Panel.Update();
            Upd_List_Panel.Update();
        }

        public void fill_Customer()
        {
            drp_cus.Items.Clear();
            DataTable dt = obj_trans.Drp_Customer();
            drp_cus.DataSource = dt;
            drp_cus.DataTextField = "text";
            drp_cus.DataValueField = "value";
            drp_cus.DataBind();
        }

        public void fill_Agent()
        {
            drp_agent.Items.Clear();
            DataTable dt = obj_trans.Drp_DocAgent();
            drp_agent.DataSource = dt;
            drp_agent.DataTextField = "text";
            drp_agent.DataValueField = "value";
            drp_agent.DataBind();
        }

        protected void btn_filter_OnClick(object sender, EventArgs e)
        {
            if (pnl_filter.Visible == true)
            {
                pnl_filter.Visible = false;
            }
            else
            {
                pnl_filter.Visible = true;
            }
            upd_nav_filter.Update();
        }

        protected void btn_search_OnClick(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue));
            pnl_filter.Visible = false;
            upd_nav_filter.Update();
        }

        #region Navigation

        //First Page
        protected void btn_first_OnClick(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue));
            Upd_List_Panel.Update();
        }

        //Previous Page
        protected void btn_prev_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number.Text) > 1)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text) - 1, Convert.ToInt32(drp_count.SelectedValue));
                Upd_List_Panel.Update();
            }
        }

        //Next Page
        protected void btn_next_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number.Text) < Convert.ToInt32(hdn_last_page.Value))
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text) + 1, Convert.ToInt32(drp_count.SelectedValue));
                Upd_List_Panel.Update();
            }
        }

        //Last Page
        protected void btn_last_OnClick(object sender, EventArgs e)
        {

            grid_fill(Convert.ToInt32(hdn_last_page.Value), Convert.ToInt32(drp_count.SelectedValue));
            Upd_List_Panel.Update();
        }

        //Page Data Count
        protected void drp_count_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue));
            Upd_List_Panel.Update();
        }

        #endregion

        public string CalDate(Telerik.Web.UI.RadDatePicker Dates)
        {
            string month = Dates.SelectedDate.Value.Month.ToString();
            if (month != "10" && month != "11" && month != "12")
                month = "0" + month;
            string day = Dates.SelectedDate.Value.Day.ToString();
            for (int i = 0; i < 10; i++)
            {
                if (Convert.ToInt32(day) == i)
                    day = "0" + day;
            }
            string year = Dates.SelectedDate.Value.Year.ToString();
            return day + '/' + month + '/' + year;
        }
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != "")
                {

                    int val = obj_common.Form_Previlage_Validation(88, Convert.ToInt32(hdn_user_id.Value));
                    if (val == 0)
                    {
                        Response.Redirect("../Login.aspx");
                    }

                }
                else
                {
                    Response.Redirect("../Login.aspx");
                }
            }
            catch
            {
                Response.Redirect("../Login.aspx");
            }
        }
    }
}