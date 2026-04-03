using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using AmarCentre.BAL;
using System.Web.UI.DataVisualization.Charting;
using AmarCentre.Layout;
using System.Drawing;

namespace AmarCentre
{
    public partial class Dashboard : System.Web.UI.Page
    {
        Master_Bal obj_master = new Master_Bal();
        Transaction_Bal obj_trans = new Transaction_Bal();
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
                previlagefillChart();
                previlage_check();
            }
        }

        public void fill_Chart_SC()
        {
            DataTable dt = obj_master.GetSCChart();
            rptChart.DataSource = dt;
            rptChart.DataBind();

            rptProgress.DataSource = dt;
            rptProgress.DataBind();

            divchart.Visible = dt.Rows.Count > 0 ? true : false;
        }

        public void fill_Chart_Summary()
        {
           divsummarytop.Visible = true;
            DataSet ds = obj_master.GetSummary();

            if (ds.Tables[0].Rows.Count > 0)
            {
                divaccountSummary.Visible = true;
                rptAccount.DataSource = ds.Tables[0];
                rptAccount.DataBind();
            }

            Credit.Text = ds.Tables[1].Rows[0]["Amount"].ToString();
            Receivable.Text = ds.Tables[1].Rows[1]["Amount"].ToString();
            CustomerAdvance.Text = ds.Tables[1].Rows[2]["Amount"].ToString();
            VendorOustanding.Text = ds.Tables[1].Rows[3]["Amount"].ToString();
            lblSCReceivable.Text= ds.Tables[1].Rows[4]["Amount"].ToString();

            lblpftamt.Text = ds.Tables[2].Rows[0][0].ToString();
            lblpftdate.Text = ds.Tables[2].Rows[0]["date"].ToString();

        }

        protected void lnkaccname_Click(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;
            HiddenField hdnaccountType = (HiddenField)itemrp.FindControl("hdnaccountType");
            HiddenField hdnAccntId = (HiddenField)itemrp.FindControl("hdnAccntId");

            string url = "";
            if (hdnaccountType.Value == "1")
                url = "../Reports/PettyCashStatementPdf.aspx?FromDate=" + DateTime.Now + "&ToDate=" + DateTime.Now +
       "&CashId=" + Convert.ToInt32(hdnAccntId.Value) + "&UserId=" + Convert.ToInt32(hdn_user_id.Value);
            else
                url = "../Reports/BankStatementPdf.aspx?FromDate=" + DateTime.Now + "&ToDate=" + DateTime.Now +
          "&BankId=" + Convert.ToInt32(hdnAccntId.Value) + "&UserId=" + Convert.ToInt32(hdn_user_id.Value);

            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        protected void lnkloanaccname_Click(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;
            HiddenField hdnLoanAccntId = (HiddenField)itemrp.FindControl("hdnLoanAccntId");

            string url = "../Reports/LoanStatementPdf.aspx?FromDate=" + DateTime.Now+ "&ToDate=" + DateTime.Now + "&LoanId=" + Convert.ToInt32(hdnLoanAccntId.Value);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        public void fill_Chart_Profit()
        {
                DataSet ds = obj_master.GetMonthlyProfitChart();
                divchartbar.Visible = ds.Tables[0].Rows.Count > 0 ? true : false;

                DataTable dt = ds.Tables[1];
                // Convert to JSON
                string labels = "";
                string lastYear = "";
                string currentYear = "";

                foreach (DataRow row in dt.Rows)
                {
                    labels += "'" + row["Month"].ToString() + "',";
                    lastYear += Convert.ToDecimal(row["ProfitLY"]) + ",";
                    currentYear += Convert.ToDecimal(row["ProfitCY"]) + ",";
                }

                // Remove last comma
                labels = labels.TrimEnd(',');
                lastYear = lastYear.TrimEnd(',');
                currentYear = currentYear.TrimEnd(',');

            // Pass to JS

            ScriptManager.RegisterStartupScript(this, this.GetType(), "chartScript",
 "Sys.Application.add_load(function() { loadChart([" + labels + "], [" + lastYear + "], [" + currentYear + "]); });",
 true);

            LTotal.Text = "Total (" + (DateTime.Now.Year - 1).ToString() + ") : " + ds.Tables[0].Rows[0]["LYProfit"].ToString();
            CTotal.Text = "Total (" + (DateTime.Now.Year).ToString() + ") : " + ds.Tables[0].Rows[0]["CYProfit"].ToString();


            //if (ds.Tables[0].Rows.Count > 0)
            //{

            //    DataTable dt = ds.Tables[1];
            //    if (dt.Rows.Count > 0)
            //    {

            //        string[] x = (from p in dt.AsEnumerable()
            //                      select p.Field<string>("Month")).ToArray();

            //        decimal[] y = (from p in dt.AsEnumerable()
            //                       select p.Field<decimal>("ProfitLY")).ToArray();

            //        //Add Series to the Chart.
            //        Chart2.Series.Add(new Series((DateTime.Now.Year - 1).ToString()));
            //        Chart2.Series[(DateTime.Now.Year - 1).ToString()].IsValueShownAsLabel = true;
            //        Chart2.Series[(DateTime.Now.Year - 1).ToString()].ChartType = SeriesChartType.Column;
            //        Chart2.Series[(DateTime.Now.Year - 1).ToString()].Points.DataBindXY(x, y);
            //        Chart2.Series[(DateTime.Now.Year - 1).ToString()].BorderWidth = 2;
            //        Chart2.Series[(DateTime.Now.Year - 1).ToString()].BorderColor = Color.Green;

            //        Chart2.Series[(DateTime.Now.Year - 1).ToString()].Color = System.Drawing.ColorTranslator.FromHtml("#800abd8f"); //Color.Green;
            //        Chart2.Series[(DateTime.Now.Year - 1).ToString()].BackSecondaryColor = System.Drawing.ColorTranslator.FromHtml("#800abd8f"); //Color.WhiteSmoke;
            //        Chart2.Series[(DateTime.Now.Year - 1).ToString()].BackGradientStyle = GradientStyle.TopBottom;

            //        string[] x2 = (from p in dt.AsEnumerable()
            //                       select p.Field<string>("Month")).ToArray();

            //        decimal[] y2 = (from p in dt.AsEnumerable()
            //                        select p.Field<decimal>("ProfitCY")).ToArray();

            //        //Add Series to the Chart.
            //        Chart2.Series.Add(new Series((DateTime.Now.Year).ToString()));
            //        Chart2.Series[(DateTime.Now.Year).ToString()].IsValueShownAsLabel = true;
            //        Chart2.Series[(DateTime.Now.Year).ToString()].ChartType = SeriesChartType.Column;
            //        Chart2.Series[(DateTime.Now.Year).ToString()].Points.DataBindXY(x2, y2);
            //        Chart2.Series[(DateTime.Now.Year).ToString()].BorderWidth = 2;
            //        Chart2.Series[(DateTime.Now.Year).ToString()].BorderColor = Color.PaleVioletRed;

            //        Chart2.Series[(DateTime.Now.Year).ToString()].Color = System.Drawing.ColorTranslator.FromHtml("#46bd0a8f"); // Color.Red;
            //        Chart2.Series[(DateTime.Now.Year).ToString()].BackSecondaryColor = System.Drawing.ColorTranslator.FromHtml("#46bd0a8f");
            //        Chart2.Series[(DateTime.Now.Year).ToString()].BackGradientStyle = GradientStyle.TopBottom;

            //    }

            //    //LYear.Text = (DateTime.Now.Year - 1).ToString();
            //    //CYear.Text = (DateTime.Now.Year).ToString();
            //    LTotal.Text = "Total (" + (DateTime.Now.Year - 1).ToString() + ") : " + ds.Tables[0].Rows[0]["LYProfit"].ToString();
            //    CTotal.Text = "Total (" + (DateTime.Now.Year).ToString() + ") : " + ds.Tables[0].Rows[0]["CYProfit"].ToString();

            //    //CTotal.Text = ds.Tables[0].Rows[0]["CYProfit"].ToString();
            //}
        }

        public void fillLoan_Summary()
        {
            divLoan.Visible = true;
            DataSet ds = obj_master.GetLoanSummaryhome();
            if (ds.Tables[0].Rows.Count == 0)
            { divLoan.Visible = false; }
            else
            {
                //if (!ds.Tables[0].Columns.Contains("DueDate"))
                //{
                //    ds.Tables[0].Columns.Add("DueDate", typeof(DateTime));
                //}
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int dueDay = row["DueDay"] != DBNull.Value ? Convert.ToInt32(row["DueDay"]) : 0;
                    if (dueDay > 0)
                    {
                        DateTime dueDate = DateTime.Now;

                        int daysInMonth = DateTime.DaysInMonth(dueDate.Year, dueDate.Month);

                        // Adjust due date if needed
                        if (dueDay > daysInMonth)
                        {
                            DateTime nextMonthFirstDay = new DateTime(dueDate.Year, dueDate.Month, 1).AddMonths(1);
                            dueDate = nextMonthFirstDay.AddDays(dueDay - daysInMonth - 1);
                        }
                        else
                        {
                            dueDate = new DateTime(dueDate.Year, dueDate.Month, dueDay);
                            if (dueDate < DateTime.Now)
                            {
                                dueDate = dueDate.AddMonths(1);
                            }
                        }

                        // Set the adjusted due date back to the row
                        //row["DueDay"] = dueDate.ToString("yyyy-MM-dd");
                        row["DueDate"] = dueDate;
                    }
                    else
                    {
                        // If dueDay is 0, clear the due date
                        //row["DueDay"] = DBNull.Value;
                        row["DueDate"] = DBNull.Value;
                    }
                    
                }


            }
            rptLoan.DataSource = ds.Tables[0];
            rptLoan.DataBind();
        }

        public void fillTopup_Balance()
        {
            divtopup.Visible = true;
            DataSet ds = obj_master.GetTopUp_Balance();
            if (ds.Tables[0].Rows.Count == 0)
            { divtopup.Visible = false; }
            else
            {
                rptTopup.DataSource = ds.Tables[0];
                rptTopup.DataBind();
            }
        }

        public void fillTopEmployee()
        {
            DataTable dt = obj_master.TopEmpolyeeService().Tables[0];

            divTopEmployee.Visible = dt.Rows.Count > 0 ? true : false;

            rptEmployee.DataSource = dt;
            rptEmployee.DataBind();

        }

        public void fillTopService()
        {
            DataTable dt = obj_master.TopEmpolyeeService().Tables[1];

            divtopservice.Visible = dt.Rows.Count > 0 ? true : false;

            if (dt.Rows.Count > 0)
            {

                List<string> departments = (from p in dt.AsEnumerable()
                                            select p.Field<string>("Name")).Distinct().ToList();

                foreach (string depart in departments)
                {

                    string[] x = (from p in dt.AsEnumerable()
                                  where p.Field<string>("Name") == depart
                                  orderby p.Field<int>("EmplId") ascending
                                  select p.Field<string>("Labels")).ToArray();

                    int[] y = (from p in dt.AsEnumerable()
                               where p.Field<string>("Name") == depart
                               orderby p.Field<int>("EmplId") ascending
                               select p.Field<int>("Counts")).ToArray();

                    //Add Series to the Chart.
                    ChartService.Series.Add(new Series(depart));

                    ChartService.Series[depart].IsValueShownAsLabel = true;
                    ChartService.Series[depart].ChartType = SeriesChartType.Column;
                    ChartService.Series[depart].Points.DataBindXY(x, y);
                    ChartService.Series[depart]["PointWidth"] = (1).ToString();
                    ChartService.Series[depart].BorderWidth = 2;
                    ChartService.Series[depart].BorderColor = System.Drawing.Color.Transparent;

                    ChartService.Series[depart].SmartLabelStyle.Enabled = false;
                    ChartService.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;

                    ChartService.Series[depart].BackSecondaryColor = Color.WhiteSmoke;
                    ChartService.Series[depart].BackGradientStyle = GradientStyle.DiagonalLeft;
                }
            }
        }

        static IEnumerable<Color> GetSystemColors()
        {
            Type type = typeof(Color);
            return type.GetProperties().Where(info => info.PropertyType == type).Select(info => (Color)info.GetValue(null, null));
        }

        protected void lnkinvoice_Click(object sender, EventArgs e)
        {
            pnlInvoiceadd.Visible = true;
            UCInvoice.UCPageLoad(2, 0, "");
            UpdInvoiceadd.Update();
        }

        protected void lnkRV_Click(object sender, EventArgs e)
        {
            pnlRVadd.Visible = true;
            UCRV.UCPageLoad(2, 0, "");
            UpdRVadd.Update();
        }

        protected void lnkPV_Click(object sender, EventArgs e)
        {
            pnlPVadd.Visible = true;
            UCPV.UCPageLoad(2, 0, "");
            UpdPVadd.Update();
        }

        protected void callSAveCompletion(object sender, EventArgs e)
        {
            UCInvoice.callSAveCompletion(null, null);
        }

        protected void lnkbtndeferedincome_Click(object sender, EventArgs e)
        {
            string url = "../Reports/CreditorReportPDf.aspx";
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        protected void lnkbtnreceivable_Click(object sender, EventArgs e)
        {
            string url = "../Reports/ReceivablePdf.aspx?Rtype=1";
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        protected void lnkCustAdvance_Click(object sender, EventArgs e)
        {
            string url = "../Reports/CustomerAdvancePdf.aspx";
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        protected void lnkbtnVendrBalance_Click(object sender, EventArgs e)
        {
            string url = "../Reports/VendorBalancePdf.aspx";
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        protected void lnkscreceivable_Click(object sender, EventArgs e)
        {
            string url = "../Reports/ReceivablePdf.aspx?Rtype=2";
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        protected void lnktodayserviceprft_Click(object sender, EventArgs e)
        {
            DataTable dtEmply = new DataTable();
            dtEmply.Columns.Add("Id", typeof(string));

            Session["dt_Emply"] = dtEmply;
            Session["dt_Service"] = dtEmply;
            Session["dt_Customer"] = dtEmply;
            Session["dt_Department"] = dtEmply;
            Session["dt_Invoice"] = dtEmply;

            string url = "../Reports/ServiceProfitStatementpdf.aspx?FromDate=" + DateTime.Now + "&ToDate=" + DateTime.Now +
               "&vendorId=null&agentId=null";
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        public void previlage_check()
        {
            if (hdn_user_id.Value != null)
            {
                int val = 0;
                val = obj_common.Form_Previlage_Validation(16, Convert.ToInt32(hdn_user_id.Value));
                divlnkinvoice.Visible = val != 0 ? true : false;
                val = obj_common.Form_Previlage_Validation(19, Convert.ToInt32(hdn_user_id.Value));
                divlnkSC.Visible = val != 0 ? true : false;
                val = obj_common.Form_Previlage_Validation(23, Convert.ToInt32(hdn_user_id.Value));
                divlnkRV.Visible = val != 0 ? true : false;
                val = obj_common.Form_Previlage_Validation(24, Convert.ToInt32(hdn_user_id.Value));
                divlnkPV.Visible = val != 0 ? true : false;
            }
        }
        public void previlagefillChart()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    DataTable dt = obj_common.Action_Previlage_Validation(67, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][1].ToString() == "1")
                            fill_Chart_SC();
                        else
                            divchart.Visible = false;

                        if (dt.Rows[1][1].ToString() == "1")
                            fill_Chart_Profit();
                        else
                            divchartbar.Visible = false;

                        if (dt.Rows[2][1].ToString() == "1")
                            fillTopup_Balance();
                        else
                            divtopup.Visible = false;

                        if (dt.Rows[3][1].ToString() == "1")
                            fill_Chart_Summary();
                        else
                            divaccountSummary.Visible = false;

                        if (dt.Rows[6][1].ToString() == "1")
                            fillLoan_Summary();
                        else
                            divLoan.Visible = false;

                        //if (dt.Rows[6][1].ToString() == "1")
                        //    fillTopup_Balance();
                        //else
                        //    divtopup.Visible = false;

                        //if (dt.Rows[9][1].ToString() == "1")
                        //    fillTopEmployee();
                        //else
                            divTopEmployee.Visible = false;

                        //if (dt.Rows[10][1].ToString() == "1")
                        //    fillTopService();
                        //else
                            divtopservice.Visible = false;


                    }

                    if (divchart.Visible == true || divchartbar.Visible == true) { lblcharthead.Text = "CHARTS"; }
                }
            }
            catch (Exception e) { }
        }

    }
}