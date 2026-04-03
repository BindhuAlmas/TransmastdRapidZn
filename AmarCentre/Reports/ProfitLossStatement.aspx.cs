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
    public partial class ProfitLossStatement : System.Web.UI.Page
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
                txtFromDate.SelectedDate = DateTime.Now;
                txtToDate.SelectedDate = DateTime.Now;
            }
        }

        protected void btn_excel_OnClick(object sender, EventArgs e)
        {
            DataSet ds = obj_report.ProfitLossStatementExcel(DateTime.ParseExact(CalDate(txtFromDate), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                DateTime.ParseExact(CalDate(txtToDate), "dd/MM/yyyy", CultureInfo.InvariantCulture));

            DataTable dtOpeningProfit = ds.Tables[0];
            DataTable dtClosingProfit = ds.Tables[1];
            DataTable dtServiceExpenseHeading = ds.Tables[2];
            DataTable dtServiceExpense = ds.Tables[3];
            DataTable dtSumServiceExpense = ds.Tables[4];
            DataTable dtIncomeHeading = ds.Tables[5];
            DataTable dtIncome= ds.Tables[6];
            DataTable dtSumIncome= ds.Tables[7];
            DataTable dtGrossProfit = ds.Tables[8];
            DataTable dtExpenseHeading = ds.Tables[9];
            DataTable dtExpense = ds.Tables[10];
            DataTable dtSumExpense = ds.Tables[11];
            DataTable dtNetProfit = ds.Tables[12];

            if (dtServiceExpense.Rows.Count > 0 || dtIncome.Rows.Count>0 || dtExpense.Rows.Count>0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=ProfitAndLossStatement.xls");
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                Table maintable = new Table();
                TableRow profitRow = new TableRow();
                TableRow firstHeading = new TableRow();
                TableRow firstDetail = new TableRow();
                TableRow firstTotal = new TableRow();
                TableRow grossProfit = new TableRow();
                TableRow secondHeading = new TableRow();
                TableRow secondDetail = new TableRow();
                TableRow secondTotal = new TableRow();
                TableRow netProfit = new TableRow();

                GridView gridViewProfit = new GridView();
                gridViewProfit.AllowPaging = false;
                gridViewProfit.ShowHeader = false;
                gridViewProfit.DataSource = dtOpeningProfit;
                gridViewProfit.DataBind();
                gridViewProfit.Rows[0].Cells[1].ColumnSpan = 2;
                TableCell profitCell = new TableCell();
                profitCell.Controls.Add(gridViewProfit);
                profitRow.Cells.Add(profitCell);

                GridView gridViewProfit2 = new GridView();
                gridViewProfit2.AllowPaging = false;
                gridViewProfit2.ShowHeader = false;
                gridViewProfit2.DataSource = dtClosingProfit;
                gridViewProfit2.DataBind();
                gridViewProfit2.Rows[0].Cells[1].ColumnSpan = 2;
                TableCell profit2Cell = new TableCell();
                profit2Cell.Controls.Add(gridViewProfit2);
                profitRow.Cells.Add(profit2Cell);

                GridView gridViewHeading = new GridView();
                gridViewHeading.AllowPaging = false;
                gridViewHeading.ShowHeader = false;
                gridViewHeading.DataSource = dtServiceExpenseHeading;
                gridViewHeading.DataBind();
                gridViewHeading.Rows[0].Cells[0].ColumnSpan = 3;
                TableCell headingCell = new TableCell();
                headingCell.Controls.Add(gridViewHeading);
                firstHeading.Cells.Add(headingCell);

                GridView gridViewHeading2 = new GridView();
                gridViewHeading2.AllowPaging = false;
                gridViewHeading2.ShowHeader = false;
                gridViewHeading2.DataSource = dtIncomeHeading;
                gridViewHeading2.DataBind();
                gridViewHeading2.Rows[0].Cells[0].ColumnSpan = 3;
                TableCell heading2Cell = new TableCell();
                heading2Cell.Controls.Add(gridViewHeading2);
                firstHeading.Cells.Add(heading2Cell);

                if (dtServiceExpense.Rows.Count > 0)
                {
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dtServiceExpense;
                    GridView1.DataBind();
                    GridView1.HeaderRow.Style.Add("background-color", "#ccc");


                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        //Apply text style to each Row
                        GridView1.Rows[i].Attributes.Add("class", "textmode");
                    }
                    TableCell detailCell = new TableCell();
                    detailCell.Controls.Add(GridView1);
                    firstDetail.Cells.Add(detailCell);
                }
                if (dtSumServiceExpense.Rows.Count > 0)
                {
                    GridView g2 = new GridView();
                    g2.AllowPaging = false;
                    g2.ShowHeader = false;
                    g2.DataSource = dtSumServiceExpense;
                    g2.DataBind();
                    g2.Rows[0].Cells[0].ColumnSpan = 2;
                    TableCell totalCell = new TableCell();
                    totalCell.Controls.Add(g2);
                    firstTotal.Cells.Add(totalCell);

                }
                if (dtIncome.Rows.Count > 0)
                {
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dtIncome;
                    GridView1.DataBind();
                    GridView1.HeaderRow.Style.Add("background-color", "#ccc");


                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        //Apply text style to each Row
                        GridView1.Rows[i].Attributes.Add("class", "textmode");
                    }
                    TableCell detailCell = new TableCell();
                    detailCell.Controls.Add(GridView1);
                    firstDetail.Cells.Add(detailCell);
                }
                if (dtSumIncome.Rows.Count > 0)
                {
                    GridView g2 = new GridView();
                    g2.AllowPaging = false;
                    g2.ShowHeader = false;
                    g2.DataSource = dtSumIncome;
                    g2.DataBind();
                    g2.Rows[0].Cells[0].ColumnSpan = 2;
                    TableCell totalCell = new TableCell();
                    totalCell.Controls.Add(g2);
                    firstTotal.Cells.Add(totalCell);

                }
                if (dtGrossProfit.Rows.Count > 0)
                {
                    GridView g2 = new GridView();
                    g2.AllowPaging = false;
                    g2.ShowHeader = false;
                    g2.DataSource = dtGrossProfit;
                    g2.DataBind();
                    g2.Rows[0].Cells[0].ColumnSpan = 2;
                    TableCell grossProfitCell = new TableCell();
                    grossProfitCell.Controls.Add(g2);
                    grossProfit.Cells.Add(grossProfitCell);

                }
                GridView gridViewHeading3 = new GridView();
                gridViewHeading3.AllowPaging = false;
                gridViewHeading3.ShowHeader = false;
                gridViewHeading3.DataSource = dtExpenseHeading;
                gridViewHeading3.DataBind();
                gridViewHeading3.Rows[0].Cells[0].ColumnSpan = 3;
                TableCell heading3Cell = new TableCell();
                heading3Cell.Controls.Add(gridViewHeading3);
                secondHeading.Cells.Add(heading3Cell);
                if (dtExpense.Rows.Count > 0)
                {
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dtExpense;
                    GridView1.DataBind();
                    GridView1.HeaderRow.Style.Add("background-color", "#ccc");


                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        //Apply text style to each Row
                        GridView1.Rows[i].Attributes.Add("class", "textmode");
                    }
                    TableCell detail2Cell = new TableCell();
                    detail2Cell.Controls.Add(GridView1);
                    secondDetail.Cells.Add(detail2Cell);
                }
                if (dtSumExpense.Rows.Count > 0)
                {
                    GridView g2 = new GridView();
                    g2.AllowPaging = false;
                    g2.ShowHeader = false;
                    g2.DataSource = dtSumExpense;
                    g2.DataBind();
                    g2.Rows[0].Cells[0].ColumnSpan = 2;
                    TableCell total2Cell = new TableCell();
                    total2Cell.Controls.Add(g2);
                    secondTotal.Cells.Add(total2Cell);

                }
                if (dtNetProfit.Rows.Count > 0)
                {
                    GridView g2 = new GridView();
                    g2.AllowPaging = false;
                    g2.ShowHeader = false;
                    g2.DataSource = dtNetProfit;
                    g2.DataBind();
                    g2.Rows[0].Cells[0].ColumnSpan = 2;
                    TableCell netProfitCell = new TableCell();
                    netProfitCell.Controls.Add(g2);
                    netProfit.Cells.Add(netProfitCell);

                }

                maintable.Rows.Add(profitRow);
                maintable.Rows.Add(firstHeading);
                maintable.Rows.Add(firstDetail);
                maintable.Rows.Add(firstTotal);
                maintable.Rows.Add(grossProfit);
                maintable.Rows.Add(secondHeading);
                maintable.Rows.Add(secondDetail);
                maintable.Rows.Add(secondTotal);
                maintable.Rows.Add(netProfit);
                maintable.RenderControl(hw);
                string style = @"<style> .textmode { mso-number-format:\@; word-wrap: break-word; } </style>";
                HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        protected void btnGeneratePdf_OnClick(object sender, EventArgs e)
        {
            string url = "../Reports/ProfitLossStatementPdf.aspx?FromDate=" + DateTime.ParseExact(CalDate(txtFromDate), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                + "&ToDate=" + DateTime.ParseExact(CalDate(txtToDate), "dd/MM/yyyy", CultureInfo.InvariantCulture) +
                "&StatusId=" + (drpStatus.SelectedValue =="" ? 0 : Convert.ToInt32(drpStatus.SelectedValue));
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        protected void btnGeneratePdfDEt_OnClick(object sender, EventArgs e)
        {
            string url = "../Reports/PLDetailedpdf.aspx?FromDate=" + DateTime.ParseExact(CalDate(txtFromDate), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                + "&ToDate=" + DateTime.ParseExact(CalDate(txtToDate), "dd/MM/yyyy", CultureInfo.InvariantCulture) +
                "&StatusId=" + (drpStatus.SelectedValue == "" ? 0 : Convert.ToInt32(drpStatus.SelectedValue));
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
                    int val = obj_common.Form_Previlage_Validation(54, Convert.ToInt32(hdn_user_id.Value));
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