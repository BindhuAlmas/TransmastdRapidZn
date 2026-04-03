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
    public partial class SOA : System.Web.UI.Page
    {
        Master_Bal obj_master = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
        Report_Bal obj_report = new Report_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
                hdn_user_id.Value = Session["User_Id"].ToString();
            }
            if (!IsPostBack)
            {
                hdn_user_id.Value = Session["User_Id"].ToString();
                lbl_User_name.Text = Session["User_Name"].ToString();
                hdn_user_id.Value = Session["User_Id"].ToString();

                txtFromDate.SelectedDate = DateTime.Now;
                txtToDate.SelectedDate = DateTime.Now;
                previlage_check();
            }
        }

        //public void grid_fill(int page_number, int page_size)
        //{

        //    DataSet ds = obj_report.CustomerSOAVersion2(txtFromDate.SelectedDate, txtToDate.SelectedDate,
        //        Convert.ToInt32(hdn_user_id.Value), 0, page_number, page_size, 0);
        //    DataTable dt = ds.Tables[0];
        //    rpt_list.DataSource = dt;
        //    rpt_list.DataBind();
        //    if (dt.Rows.Count > 0)
        //    {
        //        lblFromDate.Text = ds.Tables[2].Rows[0]["FromDate"].ToString();
        //        lblToDate.Text = ds.Tables[2].Rows[0]["ToDate"].ToString();

        //        lbl_page_info.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["Sl_No"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
        //        hdn_last_page.Value = dt.Rows[0]["last_page"].ToString();
        //        lbl_page_number.Text = dt.Rows[0]["page_number"].ToString();
        //        hdn_total.Value = dt.Rows[0]["current_count"].ToString();

        //        //lblTotalGovtFee.Text = ds.Tables[1].Rows[0]["TotalGovtFee"].ToString();
        //        //lblTotalTypingCharge.Text = ds.Tables[1].Rows[0]["TotalTypingCharge"].ToString();
        //        //lblTotalTax.Text = ds.Tables[1].Rows[0]["TotalTax"].ToString();
        //        //lblTotalFine.Text = ds.Tables[1].Rows[0]["TotalFine"].ToString();
        //        //lblTotalDiscount.Text = ds.Tables[1].Rows[0]["TotalDiscount"].ToString();
        //        lblTotalInvoiceAmount.Text = ds.Tables[1].Rows[0]["TotalInvoiceAmount"].ToString();
        //        lblTotalReceivedAmount.Text = ds.Tables[1].Rows[0]["TotalReceivedAmount"].ToString();
        //        lblTotalOutstandingAmount.Text = ds.Tables[1].Rows[0]["TotalOutstandingAmount"].ToString();
        //    }
        //    else
        //    {
        //        lblFromDate.Text = "";
        //        lblToDate.Text = "";

        //        lbl_page_info.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
        //        hdn_last_page.Value = "0";
        //        lbl_page_number.Text = "1";
        //        hdn_total.Value = "0";

        //        //lblTotalGovtFee.Text = "";
        //        //lblTotalTypingCharge.Text = "";
        //        //lblTotalTax.Text = "";
        //        //lblTotalFine.Text = "";
        //        //lblTotalDiscount.Text = "";
        //        lblTotalInvoiceAmount.Text = "";
        //        lblTotalReceivedAmount.Text = "";
        //        lblTotalOutstandingAmount.Text = "";
        //    }
        //    Upd_Nav_Panel.Update();
        //    Upd_List_Panel.Update();
        //}

        protected void btn_excel_OnClick(object sender, EventArgs e)
        {
            {
                DataSet ds = obj_report.CustomerSOAExcelVersion2(txtFromDate.SelectedDate, txtToDate.SelectedDate,
                Convert.ToInt32(hdn_user_id.Value), 0, 0);
                DataTable dtEmp = ds.Tables[0];
                DataTable dt = ds.Tables[1];
                DataTable dtSum = ds.Tables[2];

                dt.Columns.Remove("TransactionNumber");
                dt.Columns.Remove("Govt Fee");
                dt.Columns.Remove("Typing Charge");
                dt.Columns.Remove("Fine");
                dt.Columns.Remove("Discount");
                dt.Columns.Remove("Tax");

                if (dt.Rows.Count > 0)
                {
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=CustomerSOA.xls");
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter hw = new HtmlTextWriter(sw);

                    if (dtEmp.Rows.Count > 0)
                    {
                        GridView g3 = new GridView();
                        g3.AllowPaging = false;
                        g3.DataSource = dtEmp;
                        g3.DataBind();
                        g3.HeaderRow.Style.Add("background-color", "#ccc");
                        g3.RenderControl(hw);
                    }

                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dt;
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
        }
        protected void btnPdfOnClick(object sender, EventArgs e)
        {
            string url = "";
            DataTable dt = obj_master.Edit_GeneralSettings();
            if (dt.Rows[0]["CustomerSOAPdfFormat"].ToString() == "1")
                url = "../Reports/CustomerSOAPdfFormat1.aspx?FromDate=" + DateTime.ParseExact(CalDate(txtFromDate), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                + "&ToDate=" + DateTime.ParseExact(CalDate(txtToDate), "dd/MM/yyyy", CultureInfo.InvariantCulture) + "&Cus=" + Convert.ToInt32(hdn_user_id.Value)
                + "&PaymentStatus=0&CompletionStatus=0" ;
            else if (dt.Rows[0]["CustomerSOAPdfFormat"].ToString() == "2")
                url = "../Reports/CustomerSOAPdfFormat2.aspx?FromDate=" + DateTime.ParseExact(CalDate(txtFromDate), "dd/MM/yyyy", CultureInfo.InvariantCulture)
            + "&ToDate=" + DateTime.ParseExact(CalDate(txtToDate), "dd/MM/yyyy", CultureInfo.InvariantCulture) + "&Cus=" + Convert.ToInt32(hdn_user_id.Value)
             + "&PaymentStatus=0&CompletionStatus=0";
            else if (dt.Rows[0]["CustomerSOAPdfFormat"].ToString() == "3")
                url = "../Reports/CustomerSOAPdfFormat3.aspx?FromDate=" + DateTime.ParseExact(CalDate(txtFromDate), "dd/MM/yyyy", CultureInfo.InvariantCulture)
            + "&ToDate=" + DateTime.ParseExact(CalDate(txtToDate), "dd/MM/yyyy", CultureInfo.InvariantCulture) + "&Cus=" + Convert.ToInt32(hdn_user_id.Value)
             + "&PaymentStatus=0&CompletionStatus=0";
            else if (dt.Rows[0]["CustomerSOAPdfFormat"].ToString() == "4")
                url = "../Reports/CustomerSOAPdfFormat4.aspx?FromDate=" + DateTime.ParseExact(CalDate(txtFromDate), "dd/MM/yyyy", CultureInfo.InvariantCulture)
            + "&ToDate=" + DateTime.ParseExact(CalDate(txtToDate), "dd/MM/yyyy", CultureInfo.InvariantCulture) + "&Cus=" + Convert.ToInt32(hdn_user_id.Value)
             + "&PaymentStatus=0&CompletionStatus=0";
            else if (dt.Rows[0]["CustomerSOAPdfFormat"].ToString() == "5")
                url = "../Reports/CustomerSOAPdfFormat5.aspx?FromDate=" + DateTime.ParseExact(CalDate(txtFromDate), "dd/MM/yyyy", CultureInfo.InvariantCulture)
            + "&ToDate=" + DateTime.ParseExact(CalDate(txtToDate), "dd/MM/yyyy", CultureInfo.InvariantCulture) + "&Cus=" + Convert.ToInt32(hdn_user_id.Value)
            + "&PaymentStatus=0&CompletionStatus=0";

            else if (dt.Rows[0]["CustomerSOAPdfFormat"].ToString() == "6")
                url = "../Reports/CustomerSOAPdfFormat6.aspx?FromDate=" + txtFromDate.SelectedDate
            + "&ToDate=" + txtToDate.SelectedDate + "&Cus=" + Convert.ToInt32(hdn_user_id.Value)
            + "&PaymentStatus=0&CompletionStatus=0";

            else if (dt.Rows[0]["CustomerSOAPdfFormat"].ToString() == "7")
                url = "../Reports/CustomerSOAPdfFormat7.aspx?FromDate=" + txtFromDate.SelectedDate
            + "&ToDate=" + txtToDate.SelectedDate + "&Cus=" + Convert.ToInt32(hdn_user_id.Value) 
            + "&PaymentStatus=0&CompletionStatus=0";

            else if (dt.Rows[0]["CustomerSOAPdfFormat"].ToString() == "8")
                url = "../Reports/CustomerSOAPdfFormat8.aspx?FromDate=" + txtFromDate.SelectedDate
            + "&ToDate=" + txtToDate.SelectedDate + "&Cus=" + Convert.ToInt32(hdn_user_id.Value)
            + "&PaymentStatus=0&CompletionStatus=0";

            else if (dt.Rows[0]["CustomerSOAPdfFormat"].ToString() == "9")
                url = "../Reports/CustomerSOAPdfFormat9.aspx?FromDate=" + txtFromDate.SelectedDate
            + "&ToDate=" + txtToDate.SelectedDate + "&Cus=" + Convert.ToInt32(hdn_user_id.Value)
            + "&PaymentStatus=0&CompletionStatus=0";

            else if (dt.Rows[0]["CustomerSOAPdfFormat"].ToString() == "10")
                url = "../Reports/CustomerSOAPdfFormat10.aspx?FromDate=" + txtFromDate.SelectedDate
            + "&ToDate=" + txtToDate.SelectedDate + "&Cus=" + Convert.ToInt32(hdn_user_id.Value)
            + "&PaymentStatus=0&CompletionStatus=0";

            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        //protected void btn_search_OnClick(object sender, EventArgs e)
        //{
        //    grid_fill(1, Convert.ToInt32(drp_count.SelectedValue));
        //    pnl_filter.Visible = false;
        //    upd_nav_filter.Update();
        //}

        //#region Navigation

        ////First Page
        //protected void btn_first_OnClick(object sender, EventArgs e)
        //{
        //    grid_fill(1, Convert.ToInt32(drp_count.SelectedValue));
        //    Upd_List_Panel.Update();
        //}

        ////Previous Page
        //protected void btn_prev_OnClick(object sender, EventArgs e)
        //{
        //    if (Convert.ToInt32(lbl_page_number.Text) > 1)
        //    {
        //        grid_fill(Convert.ToInt32(lbl_page_number.Text) - 1, Convert.ToInt32(drp_count.SelectedValue));
        //        Upd_List_Panel.Update();
        //    }
        //}

        ////Next Page
        //protected void btn_next_OnClick(object sender, EventArgs e)
        //{
        //    if (Convert.ToInt32(lbl_page_number.Text) < Convert.ToInt32(hdn_last_page.Value))
        //    {
        //        grid_fill(Convert.ToInt32(lbl_page_number.Text) + 1, Convert.ToInt32(drp_count.SelectedValue));
        //        Upd_List_Panel.Update();
        //    }
        //}

        ////Last Page
        //protected void btn_last_OnClick(object sender, EventArgs e)
        //{

        //    grid_fill(Convert.ToInt32(hdn_last_page.Value), Convert.ToInt32(drp_count.SelectedValue));
        //    Upd_List_Panel.Update();
        //}

        ////Page Data Count
        //protected void drp_count_OnSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    grid_fill(1, Convert.ToInt32(drp_count.SelectedValue));
        //    Upd_List_Panel.Update();
        //}

        //#endregion
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
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.CustomerForm_Previlage_Validation(4, Convert.ToInt32(hdn_user_id.Value));
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