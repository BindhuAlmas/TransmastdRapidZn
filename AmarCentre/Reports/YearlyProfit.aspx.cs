using System;
using System.Web.UI;
using System.Data;
using AmarCentre.BAL;
using Telerik.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using System.Web;

namespace AmarCentre.Reports
{
    public partial class YearlyProfit : System.Web.UI.Page
    {
        Master_Bal obj_master = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
        Report_Bal obj_report = new Report_Bal();
        dtClass dtc = new dtClass();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
            }
            if (!IsPostBack)
            {
                hdn_user_id.Value = Session["User_Id"].ToString();
                fillYear();
                previlage_check();
            }
        }

        public void fillYear()
        {
            RadComboBoxItem CodeItem;
            int lastyear = DateTime.Now.Year;
            for (int date = lastyear; date >= 2018; date--)
            {
                CodeItem = new RadComboBoxItem();
                CodeItem.Text = date.ToString();
                CodeItem.Value = date.ToString();
                drpYear.Items.Add(CodeItem);
            }
        }

        protected void btn_excel_OnClick(object sender, EventArgs e)
        {
            DataTable dtyear = new DataTable();
            dtyear.Columns.Add("YearId", typeof(string));

            foreach (RadComboBoxItem item in drpYear.CheckedItems)
            {
                dtyear.Rows.Add(Convert.ToInt32(item.Value));
            }

            DataSet ds = obj_report.GetYearlyProfit(dtyear);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                dt.Columns.Remove("MonthId");

                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=YearlyProfit.xls");
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                GridView1.RenderControl(hw);

                string style = @"<style> .textmode { mso-number-format:\@; word-wrap: break-word; } </style>";
                HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        protected void btnGeneratePdf_OnClick(object sender, EventArgs e)
        {
            DataTable dtyear = new DataTable();
            dtyear.Columns.Add("YearId", typeof(string));

            foreach (RadComboBoxItem item in drpYear.CheckedItems)
            {
                dtyear.Rows.Add(Convert.ToInt32(item.Value));
            }

            dtc.setdtmultiple(dtyear);

            string url = "../Reports/YearlyProfitPdf.aspx";
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

        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    int val = obj_common.Form_Previlage_Validation(78, Convert.ToInt32(hdn_user_id.Value));
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