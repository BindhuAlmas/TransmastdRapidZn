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
    public partial class CustomerSOAIQ : System.Web.UI.Page
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

        protected void btnPdfOnClick(object sender, EventArgs e)
        {
            string url = "../Reports/CustomerSOAPdfFormat8.aspx?FromDate=" + txtFromDate.SelectedDate
        + "&ToDate=" + txtToDate.SelectedDate + "&Cus=" + Convert.ToInt32(drpCustomer.SelectedValue)
        + "&PaymentStatus=0&CompletionStatus=0";

            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        protected void btn_excel_OnClick(object sender, EventArgs e)
        {
            DataTable dtCustomerEx = new DataTable();
            dtCustomerEx.Columns.Add("Name", typeof(string));
            dtCustomerEx.Columns.Add("Contactno", typeof(string));
            dtCustomerEx.Columns.Add("TRN", typeof(string));
            dtCustomerEx.Columns.Add("Address", typeof(string));
            dtCustomerEx.Columns.Add("Date", typeof(string));
            dtCustomerEx.Columns.Add("OpeningBalance", typeof(decimal));

            DataTable dtInvoiceEx = new DataTable();
            dtInvoiceEx.Columns.Add("SlNo", typeof(int));
            dtInvoiceEx.Columns.Add("Date", typeof(string));
            dtInvoiceEx.Columns.Add("Invoice", typeof(string));
            dtInvoiceEx.Columns.Add("Service", typeof(string));
            dtInvoiceEx.Columns.Add("Quantity", typeof(decimal));
            dtInvoiceEx.Columns.Add("Amount", typeof(decimal));

            DataTable dtBalanceEX = new DataTable();
            dtBalanceEX.Columns.Add("Balance", typeof(decimal));

            DataSet ds = obj_report.CustomerSOAPrintFormat8(txtFromDate.SelectedDate, txtToDate.SelectedDate,
                Convert.ToInt32(drpCustomer.SelectedValue),0,0);
            DataTable dtCustomer = ds.Tables[0];
            DataTable dtDetails = ds.Tables[1];
            DataTable dtSubDetails = ds.Tables[2];
            DataTable dtInvoiceSum = ds.Tables[3];
            DataTable dtreceipt = ds.Tables[4];
            DataTable dtReceiptTot = ds.Tables[5];
            DataTable dtOb = ds.Tables[6];
            if (dtCustomer.Rows.Count > 0)
            {
                dtCustomerEx.Rows.Add(dtCustomer.Rows[0]["Name"], dtCustomer.Rows[0]["Mobile_num"],
                    dtCustomer.Rows[0]["CustomerTRN"], dtCustomer.Rows[0]["Address"],
                    dtCustomer.Rows[0]["FromTodate"], dtOb.Rows[0][0]);
            
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=customerSOA.xls");
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dtCustomerEx;
                GridView1.DataBind();

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    GridView1.Rows[i].Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);

                int isl = 0;
                foreach (DataRow r in dtDetails.Rows)
                {
                    dtInvoiceEx.Rows.Add(++isl, r["Dated"], r["Code"], null, null, null);
                    DataTable dh = new DataTable();
                    dh = dtSubDetails.Clone();

                    string query = "Code LIKE '%" + r["Code"].ToString() + "%'";
                     
                    DataRow[] dr = dtSubDetails.Select(query);
                    if (dr.Length > 0)
                        dh = dr.CopyToDataTable();

                    foreach (DataRow rin in dh.Rows)
                    {
                        dtInvoiceEx.Rows.Add(null, null, null, rin["Name"], rin["Quantity"], rin["Amount"]);
                    }
                }

                if (dtInvoiceEx.Rows.Count > 0)
                {
                    GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dtInvoiceEx;
                    GridView1.DataBind();

                    GridView1.RenderControl(hw);
                }
                if (dtInvoiceSum.Rows.Count > 0)
                {
                    GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dtInvoiceSum;
                    GridView1.DataBind();

                    GridView1.RenderControl(hw);
                }

                if (dtreceipt.Rows.Count > 0)
                {
                    dtreceipt.Columns["Dated"].ColumnName = "Date";
                    dtreceipt.Columns.Remove("Dateds");
                    dtreceipt.Columns.Remove("Priority");

                    GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dtreceipt;
                    GridView1.DataBind();

                    GridView1.RenderControl(hw);
                }

                if (dtReceiptTot.Rows.Count > 0)
                {
                    GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dtReceiptTot;
                    GridView1.DataBind();

                    GridView1.RenderControl(hw);
                }

                decimal totob = Convert.ToDecimal(dtOb.Rows[0][0]);
                decimal totinvsum = 0;
                decimal totrecsum = 0;
                totinvsum = (dtInvoiceSum.Rows[0][0].ToString() == "" ? 0 : Convert.ToDecimal(dtInvoiceSum.Rows[0][0])) + (totob);
                totrecsum = dtReceiptTot.Rows[0]["ReceivedAmount"].ToString() == "" ? 0 : (Convert.ToDecimal(dtReceiptTot.Rows[0]["ReceivedAmount"]));

                dtBalanceEX.Rows.Add(totinvsum - totrecsum);

                if (dtBalanceEX.Rows.Count > 0)
                {
                    GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dtBalanceEX;
                    GridView1.DataBind();

                    GridView1.RenderControl(hw);
                }

                string style = @"<style> .textmode { mso-number-format:\@; word-wrap: break-word; } </style>";
                HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
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
                    int val = obj_common.Form_Previlage_Validation(99, Convert.ToInt32(hdn_user_id.Value));
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