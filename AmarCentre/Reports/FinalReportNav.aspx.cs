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
    public partial class FinalReportNav : System.Web.UI.Page
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
                filldrpdwn();
                previlage_check();
            }
        }

        public void filldrpdwn()
        {
            DataTable dtmonth = new DataTable();
            dtmonth.Columns.Add("Text");
            dtmonth.Columns.Add("Value");
            dtmonth.Rows.Add("January", "1");
            dtmonth.Rows.Add("February", "2");
            dtmonth.Rows.Add("March", "3");
            dtmonth.Rows.Add("April", "4");
            dtmonth.Rows.Add("May", "5");
            dtmonth.Rows.Add("June", "6");
            dtmonth.Rows.Add("July", "7");
            dtmonth.Rows.Add("August", "8");
            dtmonth.Rows.Add("September", "9");
            dtmonth.Rows.Add("October", "10");
            dtmonth.Rows.Add("November", "11");
            dtmonth.Rows.Add("December", "12");

            drpFromMnth.DataSource = dtmonth;
            drpFromMnth.DataTextField = "Text";
            drpFromMnth.DataValueField = "Value";
            drpFromMnth.DataBind();

            drpToMnth.DataSource = dtmonth;
            drpToMnth.DataTextField = "text";
            drpToMnth.DataValueField = "value";
            drpToMnth.DataBind();

            RadComboBoxItem CodeItemyr;
            for (int date = 2019; date <= DateTime.Now.Year + 1; date++)
            {
                CodeItemyr = new RadComboBoxItem();
                CodeItemyr.Text = date.ToString();
                CodeItemyr.Value = date.ToString();
                drpYear.Items.Add(CodeItemyr);
            }

        }

        protected void btnPdfOnClick(object sender, EventArgs e)
        {
            string url = "";
            url = "../Reports/FinalReport.aspx?year=" + drpYear.SelectedValue
                + "&FromMnth=" + drpFromMnth.SelectedValue + "&ToMnth=" + drpToMnth.SelectedValue;

            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
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
        //Check Privilege
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    int val = obj_common.Form_Previlage_Validation(89, Convert.ToInt32(hdn_user_id.Value));
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