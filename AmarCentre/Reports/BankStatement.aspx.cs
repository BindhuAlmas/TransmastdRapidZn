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

namespace AmarCentre.Reports
{
    public partial class BankStatement : System.Web.UI.Page
    {
        Master_Bal obj_master = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
        Report_Bal obj_report = new Report_Bal();

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
                DataSet ds = obj_report.GetBankBalancebyUser(Convert.ToInt32(hdn_user_id.Value));

                rptbank.DataSource = ds.Tables[0];
                rptbank.DataBind();
                fill_Drp_down();
                txt_reg_Frm_date.SelectedDate = DateTime.Now;
                txt_reg_to_date.SelectedDate = DateTime.Now;
            }
        }

        public void fill_Drp_down()
        {
            drpBank.Items.Clear();
            drpBank.DataSource = obj_report.Drp_Bank(Convert.ToInt32(hdn_user_id.Value));
            drpBank.DataTextField = "text";
            drpBank.DataValueField = "value";
            drpBank.DataBind();

            RadComboBoxItem CodeItem = new RadComboBoxItem();
            CodeItem.Text = "All";
            CodeItem.Value = "0";
            drpBank.Items.Insert(0, CodeItem);
        }

        public void grid_fill(int page_number, int page_size)
        {

            DataSet ds = obj_report.BankStatementVersion2(txt_reg_Frm_date.SelectedDate, txt_reg_to_date.SelectedDate,
                Convert.ToInt32(drpBank.SelectedValue),page_number, page_size,Convert.ToInt32(hdn_user_id.Value));
            DataTable dt = ds.Tables[0];
            rpt_list.DataSource = dt;
            rpt_list.DataBind();
            if (ds.Tables[1].Rows.Count > 0)
            {
                lblFromDate.Text = ds.Tables[1].Rows[0]["FromDate"].ToString();
                lblToDate.Text = ds.Tables[1].Rows[0]["ToDate"].ToString();
                lblAccountName.Text = ds.Tables[1].Rows[0]["AccountName"].ToString();
                lblOpeningBalance.Text = ds.Tables[1].Rows[0]["OpeningBalance"].ToString();
                lblDebit.Text = ds.Tables[1].Rows[0]["Debit"].ToString();
                lblCredit.Text = ds.Tables[1].Rows[0]["Credit"].ToString();
                lblClosingBalance.Text = ds.Tables[1].Rows[0]["ClosingBalance"].ToString();
            }
            else
            {
                lblFromDate.Text = "";
                lblToDate.Text = "";
                lblAccountName.Text = "";
                lblOpeningBalance.Text = "";
                lblDebit.Text = "";
                lblCredit.Text = "";
                lblClosingBalance.Text = "";
            }
            if (dt.Rows.Count > 0)
            {
                lbl_page_info.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["RowNum"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
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
            Upd_Nav_Panel.Update();
            Upd_List_Panel.Update();
        }

        protected void btn_excel_OnClick(object sender, EventArgs e)
        {
            DataSet ds = obj_report.BankStatementExcelVersion2(txt_reg_Frm_date.SelectedDate, txt_reg_to_date.SelectedDate,
                Convert.ToInt32(drpBank.SelectedValue), Convert.ToInt32(hdn_user_id.Value));
            DataTable dtEmp = ds.Tables[0];
            DataTable dt = ds.Tables[1];
            dt.Columns["RowNum"].ColumnName = "Sl No.";
            dt.Columns["TransactionNumber"].ColumnName = "Transaction No.";
            if (dt.Rows.Count > 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=BankStatement.xls");
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                if (dtEmp.Rows.Count > 0)
                {
                    GridView g3 = new GridView();
                    g3.AllowPaging = false;
                    g3.DataSource = dtEmp;
                    g3.DataBind();
                    g3.HeaderRow.Style.Add("background-color", "#ccc");
                    for (int i = 0; i < g3.Rows.Count; i++)
                    {
                        //Apply text style to each Row
                        g3.Rows[i].Attributes.Add("class", "textmode");

                    }
                    g3.RenderControl(hw);

                }

                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                GridView1.RenderControl(hw);

                string style = @"<style> .textmode { mso-number-format:\@; word-wrap: break-word; } </style>";
                HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        protected void btnPdfOnClick(object sender, EventArgs e)
        {
            string url = "../Reports/BankStatementPdf.aspx?FromDate=" + txt_reg_Frm_date.SelectedDate
        + "&ToDate=" + txt_reg_to_date.SelectedDate + "&BankId=" + Convert.ToInt32(drpBank.SelectedValue)+
        "&UserId=" + Convert.ToInt32(hdn_user_id.Value);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
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
        //Check Privilege
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    int val = obj_common.Form_Previlage_Validation(38, Convert.ToInt32(hdn_user_id.Value));
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
    }
}