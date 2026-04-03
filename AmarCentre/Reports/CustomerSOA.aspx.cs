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
    public partial class CustomerSOA : System.Web.UI.Page
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

                DataTable dt = obj_master.Edit_GeneralSettings();
                if (dt.Rows[0]["IsProfessionVersion"].ToString() == "0")
                    btnSendmail.Visible = false;
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
            DataSet ds = obj_report.CustomerSOAVersion2(txtFromDate.SelectedDate, txtToDate.SelectedDate,
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
                //lblCustomerType.Text = ds.Tables[2].Rows[0]["CustomerType"].ToString();
                //lblOpeningBalance.Text = ds.Tables[2].Rows[0]["OpeningBalance"].ToString();
                //lblClosingBalance.Text = ds.Tables[2].Rows[0]["ClosingBalance"].ToString();

                lbl_page_info.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["Sl_No"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
                hdn_last_page.Value = dt.Rows[0]["last_page"].ToString();
                lbl_page_number.Text = dt.Rows[0]["page_number"].ToString();
                hdn_total.Value = dt.Rows[0]["current_count"].ToString();

                lblTotalGovtFee.Text = ds.Tables[1].Rows[0]["TotalGovtFee"].ToString();
                lblTotalTypingCharge.Text = ds.Tables[1].Rows[0]["TotalTypingCharge"].ToString();
                lblTotalTax.Text = ds.Tables[1].Rows[0]["TotalTax"].ToString();
                lblTotalFine.Text = ds.Tables[1].Rows[0]["TotalFine"].ToString();
                lblTotalDiscount.Text = ds.Tables[1].Rows[0]["TotalDiscount"].ToString();
                lblTotalInvoiceAmount.Text = ds.Tables[1].Rows[0]["TotalInvoiceAmount"].ToString();
                lblTotalReceivedAmount.Text = ds.Tables[1].Rows[0]["TotalReceivedAmount"].ToString();
                lblTotalOutstandingAmount.Text = ds.Tables[1].Rows[0]["TotalOutstandingAmount"].ToString();
            }
            else
            {
                lblFromDate.Text = "";
                lblToDate.Text = "";
                lblCustomerName.Text = "";
                //lblCustomerType.Text = "";
                //lblOpeningBalance.Text = "";
                //lblClosingBalance.Text = "";

                lbl_page_info.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
                hdn_last_page.Value = "0";
                lbl_page_number.Text = "1";
                hdn_total.Value = "0";

                lblTotalGovtFee.Text = "";
                lblTotalTypingCharge.Text = "";
                lblTotalTax.Text = "";
                lblTotalFine.Text = "";
                lblTotalDiscount.Text = "";
                lblTotalInvoiceAmount.Text = "";
                lblTotalReceivedAmount.Text = "";
                lblTotalOutstandingAmount.Text = "";
            }
            Upd_Nav_Panel.Update();
            Upd_List_Panel.Update();
        }

        protected void btn_excel_OnClick(object sender, EventArgs e)
        {
            DataTable dtgen = obj_master.Edit_GeneralSettings();
            if (dtgen.Rows[0]["CustomerSOAPdfFormat"].ToString() == "8")
            {
                DataSet ds = obj_report.CustomerSOAPrintFormat8(txtFromDate.SelectedDate, txtToDate.SelectedDate,
                    Convert.ToInt32(drpCustomer.SelectedValue), drpPaymentStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpPaymentStatus.SelectedValue),
                     drpCompletionStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpCompletionStatus.SelectedValue));
                DataTable dtCustomer = ds.Tables[0];
                DataTable dtDetails = ds.Tables[1];
                DataTable dtSubDetails = ds.Tables[2];
                DataTable dtInvoiceSum = ds.Tables[3];
                DataTable dtreceipt = ds.Tables[4];
                DataTable dtReceiptTot = ds.Tables[5];
                DataTable dtOb = ds.Tables[6];

                decimal totob = Convert.ToDecimal(dtOb.Rows[0][0]);
                decimal totinvsum = 0;
                decimal totrecsum = 0;
                totinvsum = (dtInvoiceSum.Rows[0][0].ToString() == "" ? 0 : Convert.ToDecimal(dtInvoiceSum.Rows[0][0])) + (totob);
                totrecsum = dtReceiptTot.Rows[0]["ReceivedAmount"].ToString() == "" ? 0 : (Convert.ToDecimal(dtReceiptTot.Rows[0]["ReceivedAmount"]));

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=CustomerSOA.xls");
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                DataTable dt1 = new DataTable();
                dt1.Columns.Add("h1");
                dt1.Rows.Add("Customer SOA");
                if ((drpCompletionStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpCompletionStatus.SelectedValue)) == 1)
                    dt1.Rows.Add("(Completed Transactions)");
                else if ((drpCompletionStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpCompletionStatus.SelectedValue)) == 2)
                    dt1.Rows.Add("(Pending Transactions)");

                DateTime? FromDate = txtFromDate.SelectedDate, ToDate = txtToDate.SelectedDate;
                if (FromDate != null && ToDate != null)
                    dt1.Rows.Add("From: " + Convert.ToDateTime(FromDate).ToString("dd/MM/yyyy") + " To: " + Convert.ToDateTime(ToDate).ToString("dd/MM/yyyy"));
                else if (FromDate != null && ToDate == null)
                    dt1.Rows.Add("From: " + Convert.ToDateTime(FromDate).ToString("dd/MM/yyyy") );
                else if (FromDate == null && ToDate != null)
                    dt1.Rows.Add(" To: " + Convert.ToDateTime(ToDate).ToString("dd/MM/yyyy"));

                dt1.Rows.Add("Name : " + dtCustomer.Rows[0]["Name"].ToString());

                GridView g1 = new GridView();
                g1.AllowPaging = false;
                g1.ShowHeader = false;
                g1.RowStyle.Font.Bold = true;
                g1.DataSource = dt1;
                g1.DataBind();
                g1.Rows[0].Cells[0].ColumnSpan = 4;
                if (dt1.Rows.Count>1)  g1.Rows[1].Cells[0].ColumnSpan = 4;
                if (dt1.Rows.Count > 2) g1.Rows[2].Cells[0].ColumnSpan = 4;
                if (dt1.Rows.Count > 3) g1.Rows[3].Cells[0].ColumnSpan = 4;

                g1.RenderControl(hw);

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("Mobile");
                dt2.Columns.Add("TRN");
                dt2.Columns.Add("Address");
                dt2.Rows.Add( dtCustomer.Rows[0]["Mobile_num"].ToString(), dtCustomer.Rows[0]["TRN"].ToString(), dtCustomer.Rows[0]["Address"].ToString());
                GridView g2 = new GridView();
                g2.AllowPaging = false;
                g2.DataSource = dt2;
                g2.DataBind();
                g2.HeaderRow.Style.Add("background-color", "#ccc");
                g2.RenderControl(hw);

                DataTable dt3 = new DataTable();
                dt3.Columns.Add("Previous Balance");
                dt3.Columns.Add("Invoiced Amount");
                dt3.Columns.Add("Amount Received");
                dt3.Columns.Add("Balance");
                dt3.Rows.Add(dtOb.Rows[0][0].ToString(), (totinvsum - totob).ToString(), (totrecsum).ToString(), (totinvsum - totrecsum).ToString());
                g2 = new GridView();
                g2.AllowPaging = false;
                g2.DataSource = dt3;
                g2.DataBind();
                g2.HeaderRow.Style.Add("background-color", "#ccc");
                g2.RenderControl(hw);

                GridView GridView1 = new GridView();

                DataTable dtInvoiceEx = new DataTable();
                dtInvoiceEx.Columns.Add("SlNo", typeof(int));
                dtInvoiceEx.Columns.Add("Date", typeof(string));
                dtInvoiceEx.Columns.Add("Invoice", typeof(string));
                dtInvoiceEx.Columns.Add("Service", typeof(string));
                dtInvoiceEx.Columns.Add("Quantity", typeof(decimal));
                dtInvoiceEx.Columns.Add("Rate", typeof(decimal));
                dtInvoiceEx.Columns.Add("Fine", typeof(decimal));
                dtInvoiceEx.Columns.Add("Amount", typeof(decimal));
                dtInvoiceEx.Columns.Add("Total", typeof(decimal));

                DataTable dttotalEX = new DataTable();

                int isl = 0;
                foreach (DataRow r in dtDetails.Rows)
                {
                    dtInvoiceEx.Rows.Add(++isl, r["Dated"], r["Code"], null, null, null, null, null,r["Amount"]);
                    DataTable dh = new DataTable();
                    dh = dtSubDetails.Clone();

                    string query = "Code LIKE '%" + r["Code"].ToString() + "%'";

                    DataRow[] dr = dtSubDetails.Select(query);
                    if (dr.Length > 0)
                        dh = dr.CopyToDataTable();

                    foreach (DataRow rin in dh.Rows)
                    {
                        dtInvoiceEx.Rows.Add(null, null, null, rin["Name"], rin["Quantity"], rin["Rate"], rin["Fine"], rin["AmountNoFine"], null);
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
                if (dtInvoiceSum.Rows.Count > 0 && dtInvoiceEx.Rows.Count > 0)
                {
                    dttotalEX = new DataTable();
                    dttotalEX.Columns.Add("h1", typeof(string));
                    dttotalEX.Columns.Add("h2", typeof(decimal));

                    dttotalEX.Rows.Add("Total", (totinvsum - totob).ToString());

                    GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.ShowHeader = false;
                    GridView1.RowStyle.Font.Bold = true;
                    GridView1.DataSource = dttotalEX;
                    GridView1.DataBind();
                    GridView1.Rows[0].Cells[0].ColumnSpan = 8;
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

                if (dtReceiptTot.Rows.Count > 0 && dtreceipt.Rows.Count > 0)
                {
                    dttotalEX = new DataTable();
                    dttotalEX.Columns.Add("h1", typeof(string));
                    dttotalEX.Columns.Add("h2", typeof(decimal));
                    dttotalEX.Columns.Add("h3", typeof(string));

                    dttotalEX.Rows.Add("Total", (totrecsum).ToString(),"");

                    GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.ShowHeader = false;
                    GridView1.RowStyle.Font.Bold = true;
                    GridView1.DataSource = dttotalEX;
                    GridView1.DataBind();
                    GridView1.Rows[0].Cells[0].ColumnSpan = 2;
                    GridView1.RenderControl(hw);
                }

                if (dtreceipt.Rows.Count > 0 || dtInvoiceEx.Rows.Count > 0)
                {
                    dttotalEX = new DataTable();
                    dttotalEX.Columns.Add("h1", typeof(string));
                    dttotalEX.Columns.Add("h2", typeof(decimal));
                    dttotalEX.Columns.Add("h3", typeof(string));

                    dttotalEX.Rows.Add("Balance", (totinvsum - totrecsum).ToString(),"");

                    GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.ShowHeader = false;
                    GridView1.DataSource = dttotalEX;
                    GridView1.DataBind();
                    GridView1.Rows[0].Cells[0].ColumnSpan = 2;
                    GridView1.RenderControl(hw);
                }

                string style = @"<style> .textmode { mso-number-format:\@; word-wrap: break-word; } </style>";
                HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
            else
            {
                    DataSet ds = obj_report.CustomerSOAExcelVersion2(txtFromDate.SelectedDate, txtToDate.SelectedDate,
                        Convert.ToInt32(drpCustomer.SelectedValue), drpPaymentStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpPaymentStatus.SelectedValue),
                         drpCompletionStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpCompletionStatus.SelectedValue));
                    DataTable dtEmp = ds.Tables[0];
                    DataTable dt = ds.Tables[1];
                    DataTable dtSum = ds.Tables[2];
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

                        for (int i = 0; i < GridView1.Rows.Count; i++)
                        {
                            //Apply text style to each Row
                            GridView1.Rows[i].Attributes.Add("class", "textmode");
                        }
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
                + "&ToDate=" + DateTime.ParseExact(CalDate(txtToDate), "dd/MM/yyyy", CultureInfo.InvariantCulture) + "&Cus=" + Convert.ToInt32(drpCustomer.SelectedValue)
                + "&PaymentStatus=" + (drpPaymentStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpPaymentStatus.SelectedValue))
            + "&CompletionStatus=" + (drpCompletionStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpCompletionStatus.SelectedValue));
            else if (dt.Rows[0]["CustomerSOAPdfFormat"].ToString() == "2")
                url = "../Reports/CustomerSOAPdfFormat2.aspx?FromDate=" + DateTime.ParseExact(CalDate(txtFromDate), "dd/MM/yyyy", CultureInfo.InvariantCulture)
            + "&ToDate=" + DateTime.ParseExact(CalDate(txtToDate), "dd/MM/yyyy", CultureInfo.InvariantCulture) + "&Cus=" + Convert.ToInt32(drpCustomer.SelectedValue)
            + "&PaymentStatus=" + (drpPaymentStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpPaymentStatus.SelectedValue))
        + "&CompletionStatus=" + (drpCompletionStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpCompletionStatus.SelectedValue));
            else if (dt.Rows[0]["CustomerSOAPdfFormat"].ToString() == "3")
                url = "../Reports/CustomerSOAPdfFormat3.aspx?FromDate=" + DateTime.ParseExact(CalDate(txtFromDate), "dd/MM/yyyy", CultureInfo.InvariantCulture)
            + "&ToDate=" + DateTime.ParseExact(CalDate(txtToDate), "dd/MM/yyyy", CultureInfo.InvariantCulture) + "&Cus=" + Convert.ToInt32(drpCustomer.SelectedValue)
            + "&PaymentStatus=" + (drpPaymentStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpPaymentStatus.SelectedValue))
        + "&CompletionStatus=" + (drpCompletionStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpCompletionStatus.SelectedValue));
            else if (dt.Rows[0]["CustomerSOAPdfFormat"].ToString() == "4")
                url = "../Reports/CustomerSOAPdfFormat4.aspx?FromDate=" + DateTime.ParseExact(CalDate(txtFromDate), "dd/MM/yyyy", CultureInfo.InvariantCulture)
            + "&ToDate=" + DateTime.ParseExact(CalDate(txtToDate), "dd/MM/yyyy", CultureInfo.InvariantCulture) + "&Cus=" + Convert.ToInt32(drpCustomer.SelectedValue)
            + "&PaymentStatus=" + (drpPaymentStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpPaymentStatus.SelectedValue))
        + "&CompletionStatus=" + (drpCompletionStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpCompletionStatus.SelectedValue));
            else if (dt.Rows[0]["CustomerSOAPdfFormat"].ToString() == "5")
                url = "../Reports/CustomerSOAPdfFormat5.aspx?FromDate=" + DateTime.ParseExact(CalDate(txtFromDate), "dd/MM/yyyy", CultureInfo.InvariantCulture)
            + "&ToDate=" + DateTime.ParseExact(CalDate(txtToDate), "dd/MM/yyyy", CultureInfo.InvariantCulture) + "&Cus=" + Convert.ToInt32(drpCustomer.SelectedValue)
            + "&PaymentStatus=" + (drpPaymentStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpPaymentStatus.SelectedValue))
        + "&CompletionStatus=" + (drpCompletionStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpCompletionStatus.SelectedValue));

            else if (dt.Rows[0]["CustomerSOAPdfFormat"].ToString() == "6")
                url = "../Reports/CustomerSOAPdfFormat6.aspx?FromDate=" + txtFromDate.SelectedDate
            + "&ToDate=" + txtToDate.SelectedDate + "&Cus=" + Convert.ToInt32(drpCustomer.SelectedValue)
             + "&PaymentStatus=" + (drpPaymentStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpPaymentStatus.SelectedValue))
        + "&CompletionStatus=" + (drpCompletionStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpCompletionStatus.SelectedValue));

            else if (dt.Rows[0]["CustomerSOAPdfFormat"].ToString() == "7")
                url = "../Reports/CustomerSOAPdfFormat7.aspx?FromDate=" + txtFromDate.SelectedDate
            + "&ToDate=" + txtToDate.SelectedDate + "&Cus=" + Convert.ToInt32(drpCustomer.SelectedValue)
             + "&PaymentStatus=" + (drpPaymentStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpPaymentStatus.SelectedValue))
        + "&CompletionStatus=" + (drpCompletionStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpCompletionStatus.SelectedValue));

            else if (dt.Rows[0]["CustomerSOAPdfFormat"].ToString() == "8")
                url = "../Reports/CustomerSOAPdfFormat8.aspx?FromDate=" + txtFromDate.SelectedDate
            + "&ToDate=" + txtToDate.SelectedDate + "&Cus=" + Convert.ToInt32(drpCustomer.SelectedValue)
             + "&PaymentStatus=" + (drpPaymentStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpPaymentStatus.SelectedValue))
        + "&CompletionStatus=" + (drpCompletionStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpCompletionStatus.SelectedValue));

            else if (dt.Rows[0]["CustomerSOAPdfFormat"].ToString() == "9")
                url = "../Reports/CustomerSOAPdfFormat9.aspx?FromDate=" + txtFromDate.SelectedDate
            + "&ToDate=" + txtToDate.SelectedDate + "&Cus=" + Convert.ToInt32(drpCustomer.SelectedValue)
             + "&PaymentStatus=" + (drpPaymentStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpPaymentStatus.SelectedValue))
        + "&CompletionStatus=" + (drpCompletionStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpCompletionStatus.SelectedValue));
            else if (dt.Rows[0]["CustomerSOAPdfFormat"].ToString() == "10")

                url = "../Reports/CustomerSOAPdfFormat10.aspx?FromDate=" + txtFromDate.SelectedDate
            + "&ToDate=" + txtToDate.SelectedDate + "&Cus=" + Convert.ToInt32(drpCustomer.SelectedValue)
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

        protected void btSendmailOnClick(object sender, EventArgs e)
        {
            EmailUC.UCSOAPageLoad(6, DateTime.ParseExact(CalDate(txtFromDate), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                DateTime.ParseExact(CalDate(txtToDate), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Convert.ToInt32(drpCustomer.SelectedValue), drpPaymentStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpPaymentStatus.SelectedValue),
                 drpCompletionStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpCompletionStatus.SelectedValue));
            pnlMail.Visible = true;
            UpdMailPanel.Update();
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
                    int val = obj_common.Form_Previlage_Validation(40, Convert.ToInt32(hdn_user_id.Value));
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