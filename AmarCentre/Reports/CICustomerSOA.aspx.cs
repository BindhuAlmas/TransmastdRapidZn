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
    public partial class CICustomerSOA : System.Web.UI.Page
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
                fill_Drp_down();
                txtFromDate.SelectedDate = DateTime.Now;
                txtToDate.SelectedDate = DateTime.Now;
            }
        }

        public void fill_Drp_down()
        {
            drpCustomer.Items.Clear();
            drpCustomer.DataSource = obj_report.Drp_Customer();
            drpCustomer.DataTextField = "text";
            drpCustomer.DataValueField = "value";
            drpCustomer.DataBind();
        }

        public void grid_fill(int page_number, int page_size)
        {

            DataSet ds = obj_report.CICustomerSOAlist(DateTime.ParseExact(CalDate(txtFromDate), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                DateTime.ParseExact(CalDate(txtToDate), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Convert.ToInt32(drpCustomer.SelectedValue),
                drpPaymentStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpPaymentStatus.SelectedValue), page_number, page_size,
                drpCompletionStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpCompletionStatus.SelectedValue));
            DataTable dt = ds.Tables[0];
            rpt_list.DataSource = dt;
            rpt_list.DataBind();
            if (dt.Rows.Count > 0)
            {
                lblFromDate.Text = ds.Tables[2].Rows[0]["FromDate"].ToString();
                lblToDate.Text = ds.Tables[2].Rows[0]["ToDate"].ToString();
                lblCustomerName.Text = ds.Tables[2].Rows[0]["CustomerName"].ToString();
               
                lbl_page_info.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["Sl_No"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
                hdn_last_page.Value = dt.Rows[0]["last_page"].ToString();
                lbl_page_number.Text = dt.Rows[0]["page_number"].ToString();
                hdn_total.Value = dt.Rows[0]["current_count"].ToString();

                lblTotalInvoiceAmount.Text = ds.Tables[1].Rows[0]["TotalDebit"].ToString();
                lblTotalReceivedAmount.Text = ds.Tables[1].Rows[0]["TotalCredit"].ToString();
                lblTotalOutstandingAmount.Text = ds.Tables[1].Rows[0]["TotalBalance"].ToString();
            }
            else
            {
                lblFromDate.Text = "";
                lblToDate.Text = "";
                lblCustomerName.Text = "";
               
                lbl_page_info.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
                hdn_last_page.Value = "0";
                lbl_page_number.Text = "1";
                hdn_total.Value = "0";

                lblTotalInvoiceAmount.Text = "";
                lblTotalReceivedAmount.Text = "";
                lblTotalOutstandingAmount.Text = "";
            }
            Upd_Nav_Panel.Update();
            Upd_List_Panel.Update();
        }

        protected void btn_excel_OnClick(object sender, EventArgs e)
        {
            DataSet ds = obj_report.CICustomerSOAPrintFormat2(DateTime.ParseExact(CalDate(txtFromDate), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                DateTime.ParseExact(CalDate(txtToDate), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Convert.ToInt32(drpCustomer.SelectedValue), drpPaymentStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpPaymentStatus.SelectedValue),
                 drpCompletionStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpCompletionStatus.SelectedValue));
            DataTable dtCustomer = ds.Tables[0];
            DataTable dtDetails = ds.Tables[1];
            DataTable dtSum = ds.Tables[2];
            if (dtDetails.Rows.Count > 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=CustomerSOA.xls");
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                if (dtDetails.Rows.Count > 0)
                {
                    GridView g3 = new GridView();
                    g3.AllowPaging = false;
                    g3.DataSource = dtCustomer;
                    g3.DataBind();
                    g3.HeaderRow.Style.Add("background-color", "#ccc");
                    g3.RenderControl(hw);

                }

                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dtDetails;
                GridView1.DataBind();

                GridView1.RenderControl(hw);

                if (dtSum.Rows.Count > 0)
                {
                    GridView g3 = new GridView();
                    g3.AllowPaging = false;
                    g3.DataSource = dtSum;
                    g3.DataBind();
                    g3.HeaderRow.Style.Add("background-color", "#ccc");
                    g3.RenderControl(hw);
                }
                string style = @"<style> .textmode { mso-number-format:\@; word-wrap: break-word; } </style>";
                HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        protected void btnPdfOnClick(object sender, EventArgs e)
        {
            string url = "";

            url = "../Reports/CICustomerSOAPdf.aspx?FromDate=" + DateTime.ParseExact(CalDate(txtFromDate), "dd/MM/yyyy", CultureInfo.InvariantCulture)
            + "&ToDate=" + DateTime.ParseExact(CalDate(txtToDate), "dd/MM/yyyy", CultureInfo.InvariantCulture) + "&Cus=" + Convert.ToInt32(drpCustomer.SelectedValue)
            + "&PaymentStatus=" + (drpPaymentStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpPaymentStatus.SelectedValue))
        + "&CompletionStatus=" + (drpCompletionStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpCompletionStatus.SelectedValue));
           
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
                    int val = obj_common.Form_Previlage_Validation(92, Convert.ToInt32(hdn_user_id.Value));
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