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
using System.Web.UI.DataVisualization.Charting;
using System.Web.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace AmarCentre
{
    public partial class WhatsappDashboardEmpty : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();
        Master_Bal obj_master = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
            }
            if (!IsPostBack)
            {
                DataTable dt = obj_master.Edit_GeneralSettings();
                if(dt.Rows.Count>0)
                {
                    string WABaseUrl = WebConfigurationManager.AppSettings["WABaseUrl"].ToString();
                    hdnWABaseUrl.Value = WABaseUrl;
                    hdnCompanyName.Value = dt.Rows[0]["CompanyName"].ToString();
                    hdnEmailAddress.Value = dt.Rows[0]["CompanyMail"].ToString();
                    hdnPhoneNumber.Value = dt.Rows[0]["CompanyPhone"].ToString();
                }
                //hdn_user_id.Value = Session["User_Id"].ToString();
                //fill_Drp_down();
            }
        }

        protected void btn_addnew_Click(object sender, EventArgs e)
        {
            pnlConfirm.Visible = true;
            lblCompany.Text = hdnCompanyName.Value.ToString();
            lblEmail.Text = hdnEmailAddress.Value.ToString();
            lblPhone.Text = hdnPhoneNumber.Value.ToString();
            Upd_Add_Panel.Update();
        }
        protected  async void btnSubmit_Click(object sender, EventArgs e)
        {
            submitwebcall();
        }
        private   async void submitwebcall()
        {
            string companyName = lblCompany.Text;
            string emailAddress = lblEmail.Text;
            string phoneNumber = lblPhone.Text;
            string postUrl = hdnWABaseUrl.Value + "/Leads/PostRequest";

            bool success = await Task.Run(() =>
                obj_common.SendWADemoRequest(companyName, emailAddress, phoneNumber, postUrl)
            );

            if (success)
            {
                pnlConfirm.Visible = false;
                pnlSuccess.Visible = true;
                lblSCompany.Text = companyName;
                lblSPhone.Text = phoneNumber;
                lblSEmail.Text = emailAddress;
                Upd_Add_Panel.Update();
            }

        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlConfirm.Visible = false;
            Upd_Add_Panel.Update();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            pnlEdit.Visible = true;
            txtCompany.Text = hdnCompanyName.Value.ToString();
            txtEmail.Text = hdnEmailAddress.Value.ToString();
            txtPhone.Text = hdnPhoneNumber.Value.ToString();
            Upd_Add_Panel.Update();
        }

        protected void btnSaveEdit_Click(object sender, EventArgs e)
        {
            int updated = obj_master.UpdateCompany_GeneralSettings(txtCompany.Text, txtEmail.Text, txtPhone.Text);
            if(updated>0)
            {
                pnlEdit.Visible = false;
                pnlConfirm.Visible = true;
                hdnCompanyName.Value = txtCompany.Text;
                hdnEmailAddress.Value = txtEmail.Text;
                hdnPhoneNumber.Value = txtPhone.Text;
                lblCompany.Text = txtCompany.Text;
                lblEmail.Text = txtEmail.Text;
                lblPhone.Text = txtPhone.Text;
                Upd_Add_Panel.Update();
            }
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            pnlEdit.Visible = false;
            Upd_Add_Panel.Update();
        }

        //public void fill_Drp_down()
        //{
        //    drpCustomer.Items.Clear();
        //    drpCustomer.DataSource = obj_report.Drp_Customer();
        //    drpCustomer.DataTextField = "text";
        //    drpCustomer.DataValueField = "value";
        //    drpCustomer.DataBind();
        //}

        //protected void btn_search_Click(object sender, EventArgs e)
        //{
        //    DataSet ds = obj_master.GetCustomerDashboard(Convert.ToInt32(drpCustomer.SelectedValue) );

        //    { //1
        //        DataTable dt = ds.Tables[0];
        //        divAccountSumry.Visible = dt.Rows.Count > 0 ? true : false;

        //        if (dt.Rows.Count > 0)
        //        {
        //            Chart1.DataSource = dt;
        //            Chart1.Series["Series1"].XValueMember = "PercentageS";
        //            Chart1.Series["Series1"].YValueMembers = "Percentage";

        //            Chart1.DataBind();

        //            //string[] colors = new[] { "#0080ff", "#FFFF00", "#FF007f" };

        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                Chart1.Series["Series1"].Points[i].Color = System.Drawing.ColorTranslator.FromHtml(dt.Rows[i]["Color"].ToString());

        //                Chart1.Series["Series1"].Points[i].LegendText = dt.Rows[i]["Remark"].ToString();
        //                Chart1.Series["Series1"].Points[i].Font = new System.Drawing.Font("Times", 12f, System.Drawing.FontStyle.Bold);
        //            }

        //            Chart1.Legends["Legend1"].Docking = Docking.Bottom;
        //            Chart1.Legends["Legend1"].Alignment = System.Drawing.StringAlignment.Center;
        //        }

        //    }

        //    { //2
        //        DataTable dt = ds.Tables[1];
        //        divTransSumry.Visible = dt.Rows.Count > 0 ? true : false;

        //        if (dt.Rows.Count > 0)
        //        {
        //            Chart2.DataSource = dt;
        //            Chart2.Series["Series1"].XValueMember = "PercentageS";
        //            Chart2.Series["Series1"].YValueMembers = "Percentage";

        //            Chart2.DataBind();

        //            //string[] colors = new[] { "#0080ff", "#FFFF00", "#FF007f" };

        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                //Chart1.Series["Series1"].Points[i].Color = System.Drawing.ColorTranslator.FromHtml(colors[i]);
        //                Chart2.Series["Series1"].Points[i].Color = System.Drawing.ColorTranslator.FromHtml(dt.Rows[i]["Color"].ToString());

        //                Chart2.Series["Series1"].Points[i].LegendText = dt.Rows[i]["Remark"].ToString();
        //                Chart2.Series["Series1"].Points[i].Font = new System.Drawing.Font("Times", 12f, System.Drawing.FontStyle.Bold);
        //            }

        //            Chart2.Legends["Legend1"].Docking = Docking.Bottom;
        //            Chart2.Legends["Legend1"].Alignment = System.Drawing.StringAlignment.Center;
        //        }
        //    }

        //    { //3
        //        DataTable dt = ds.Tables[2];
        //        pnlSalesRevenue.Visible = dt.Rows.Count > 0 ? true : false;
        //        // Series for Total Sales
        //        Series salesSeries = new Series("Revenue")
        //        {
        //            ChartType = SeriesChartType.Column,
        //            Color = System.Drawing.Color.SteelBlue,
        //            BorderWidth = 0,
        //            IsValueShownAsLabel = true, // Show the value above the bar
        //            LabelForeColor = System.Drawing.Color.Black, // Label color
        //            ["PixelPointWidth"] = "70" // Adjusts the width of bars for better spacing
        //        };

        //        // Series for Total Revenue
        //        Series revenueSeries = new Series("Profit")
        //        {
        //            ChartType = SeriesChartType.Column,
        //            Color = System.Drawing.Color.Green,
        //            BorderWidth = 0,
        //            IsValueShownAsLabel = true, // Show the value above the bar
        //            LabelForeColor = System.Drawing.Color.Black, // Label color
        //            ["PixelPointWidth"] = "70" // Adjusts the width of bars for better spacing
        //        };

        //        foreach (DataRow row in dt.Rows)
        //        {
        //            //string year = row["Year"].ToString();
        //            //string month = row["Month"].ToString();
        //            string monthYear = "Revenue Vs Profit";// $"{month}/{year}";
        //            decimal totalSales = Convert.ToDecimal(row["Revenue"]);
        //            decimal totalRevenue = Convert.ToDecimal(row["Profit"]);

        //            salesSeries.Points.AddXY(monthYear, totalSales);
        //            revenueSeries.Points.AddXY(monthYear, totalRevenue);
        //        }

        //        SalesRevenueChart.Series.Add(salesSeries);
        //        SalesRevenueChart.Series.Add(revenueSeries);

        //        // Customize chart appearance
        //        SalesRevenueChart.ChartAreas[0].AxisX.Title = "";
        //        SalesRevenueChart.ChartAreas[0].AxisY.Title = "Amount";
        //        SalesRevenueChart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0; // Remove grid lines
        //        SalesRevenueChart.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0; // Remove grid lines
        //        SalesRevenueChart.ChartAreas[0].AxisX.Interval = 1;
        //        SalesRevenueChart.ChartAreas[0].AxisY.Minimum = 0; // Start from 0
        //        SalesRevenueChart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Gray; // Axis line color
        //        SalesRevenueChart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Gray; // Axis line color

        //        // Position legend on top
        //        SalesRevenueChart.Legends[0].Docking = Docking.Top;
        //    }

        //    { //4
        //        DataTable dt = ds.Tables[3];
        //        pnldocumentexpiry.Visible = dt.Rows.Count > 0 ? true : false;

        //        if (dt.Rows.Count > 0)
        //        {

        //            List<string> departments = (from p in dt.AsEnumerable()
        //                                        select p.Field<string>("Name")).Distinct().ToList();

        //            foreach (string depart in departments)
        //            {

        //                string[] x = (from p in dt.AsEnumerable()
        //                              where p.Field<string>("Name") == depart
        //                              orderby p.Field<int>("EmplId") ascending
        //                              select p.Field<string>("Labels")).ToArray();

        //                int[] y = (from p in dt.AsEnumerable()
        //                           where p.Field<string>("Name") == depart
        //                           orderby p.Field<int>("EmplId") ascending
        //                           select p.Field<int>("Counts")).ToArray();

        //                //Add Series to the Chart.
        //                ChartDocu.Series.Add(new Series(depart));

        //                ChartDocu.Series[depart].IsValueShownAsLabel = true;
        //                ChartDocu.Series[depart].ChartType = SeriesChartType.Column;
        //                ChartDocu.Series[depart].Points.DataBindXY(x, y);
        //                ChartDocu.Series[depart]["PointWidth"] = (1).ToString();
        //                ChartDocu.Series[depart].BorderWidth = 2;
        //                ChartDocu.Series[depart].BorderColor = System.Drawing.Color.Transparent;

        //                ChartDocu.Series[depart].SmartLabelStyle.Enabled = false;
        //                ChartDocu.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;

        //                //ChartDocu.Series[depart].BackSecondaryColor = Color.WhiteSmoke;
        //                //ChartDocu.Series[depart].BackGradientStyle = GradientStyle.DiagonalLeft;
        //            }
        //        }
        //    }

        //    { //5
        //        DataTable dt = ds.Tables[4];
        //        pnlDeadline.Visible = dt.Rows.Count > 0 ? true : false;

        //        if (dt.Rows.Count > 0)
        //        {

        //            List<string> departments = (from p in dt.AsEnumerable()
        //                                        select p.Field<string>("Name")).Distinct().ToList();

        //            foreach (string depart in departments)
        //            {

        //                string[] x = (from p in dt.AsEnumerable()
        //                              where p.Field<string>("Name") == depart
        //                              orderby p.Field<int>("EmplId") ascending
        //                              select p.Field<string>("Labels")).ToArray();

        //                int[] y = (from p in dt.AsEnumerable()
        //                           where p.Field<string>("Name") == depart
        //                           orderby p.Field<int>("EmplId") ascending
        //                           select p.Field<int>("Counts")).ToArray();

        //                //Add Series to the Chart.
        //                ChartDeadline.Series.Add(new Series(depart));

        //                ChartDeadline.Series[depart].IsValueShownAsLabel = true;
        //                ChartDeadline.Series[depart].ChartType = SeriesChartType.Column;
        //                ChartDeadline.Series[depart].Points.DataBindXY(x, y);
        //                ChartDeadline.Series[depart]["PointWidth"] = (1).ToString();
        //                ChartDeadline.Series[depart].BorderWidth = 2;
        //                ChartDeadline.Series[depart].BorderColor = System.Drawing.Color.Transparent;

        //                ChartDeadline.Series[depart].SmartLabelStyle.Enabled = false;
        //                ChartDeadline.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;

        //                //ChartDocu.Series[depart].BackSecondaryColor = Color.WhiteSmoke;
        //                //ChartDocu.Series[depart].BackGradientStyle = GradientStyle.DiagonalLeft;
        //            }
        //        }
        //    }

        //    updFilldetails.Update();

        //}
    }
}