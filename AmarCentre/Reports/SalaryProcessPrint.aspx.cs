using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using AmarCentre.BAL;
using System.IO;

namespace AmarCentre.Reports
{
    public partial class SalaryProcessPrint : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();
        Master_Bal obj_master = new Master_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"]);
            DataSet ds = obj_report.Salary_processPrint(id);
            DataTable dt1 = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];

            Document document = new Document(PageSize.A4, 0f, 0f, 10f, 10f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=SalaryProcess.pdf");
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
            document.Open();
            if (Application["PrintHeader"] != "")
            {

                string imageURL = Server.MapPath("../UploadedImage/" + Application["PrintHeader"]);
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(new Uri(imageURL));
                //Resize image depend upon your need
                jpg.ScaleToFit(550f, 450f);
                //Give space before image
                //jpg.SpacingBefore = 1f;
                //Give some space after the image
                jpg.SpacingAfter = 5f;
                jpg.Alignment = Element.ALIGN_CENTER;

                document.Add(jpg);
            }

            #region header

            PdfPTable table1 = new PdfPTable(1);
            table1.DefaultCell.Padding = 4;
            PdfPCell cell1 = new PdfPCell(new Phrase("Salary Process", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            cell1.Border = 0;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            table1.AddCell(cell1);

            document.Add(table1);

            PdfPTable Subhead = new PdfPTable(2);
            Subhead.DefaultCell.Padding = 4;
            Subhead.SpacingBefore = 20f;
            float[] widths = new float[] { 15f, 80f };
            Subhead.SetWidths(widths);
            Subhead.WidthPercentage = 90f;

            // empty cell

            PdfPCell Empty = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            Empty.Border = 0;
            Empty.HorizontalAlignment = Element.ALIGN_LEFT;

            //end

            PdfPCell sub1s = new PdfPCell(new Phrase("Code :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub1s.Border = 0;
            sub1s.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub1s);

            PdfPCell sub1ss = new PdfPCell(new Phrase(dt1.Rows[0]["Code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub1ss.Border = 0;
            sub1ss.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub1ss);

            //PdfPCell sub1 = new PdfPCell(new Phrase("Date :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            //sub1.Border = 0;
            //sub1.HorizontalAlignment = Element.ALIGN_LEFT;
            //Subhead.AddCell(sub1);

            //PdfPCell sub12 = new PdfPCell(new Phrase(dt1.Rows[0]["Dates"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            //sub12.Border = 0;
            //sub12.HorizontalAlignment = Element.ALIGN_LEFT;
            //Subhead.AddCell(sub12);

            //sub1s = new PdfPCell(new Phrase("Total Salary :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            //sub1s.Border = 0;
            //sub1s.HorizontalAlignment = Element.ALIGN_LEFT;
            //Subhead.AddCell(sub1s);

            //sub1ss = new PdfPCell(new Phrase(dt_tot.Rows[0]["Total_Salary"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            //sub1ss.Border = 0;
            //sub1ss.HorizontalAlignment = Element.ALIGN_LEFT;
            //Subhead.AddCell(sub1ss);

            PdfPCell sub13 = new PdfPCell(new Phrase("Month :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub13.Border = 0;
            sub13.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub13);

            PdfPCell sub14 = new PdfPCell(new Phrase(dt1.Rows[0]["MonthNames"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub14.Border = 0;
            sub14.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub14);

            //sub1 = new PdfPCell(new Phrase("Total Addition :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            //sub1.Border = 0;
            //sub1.HorizontalAlignment = Element.ALIGN_LEFT;
            //Subhead.AddCell(sub1);

            //sub12 = new PdfPCell(new Phrase(dt_tot.Rows[0]["Total_Addition"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            //sub12.Border = 0;
            //sub12.HorizontalAlignment = Element.ALIGN_LEFT;
            //Subhead.AddCell(sub12);

            PdfPCell sub21 = new PdfPCell(new Phrase("Year :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub21.Border = 0;
            sub21.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub21);

            PdfPCell sub122 = new PdfPCell(new Phrase(dt1.Rows[0]["Year"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub122.Border = 0;
            sub122.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub122);

            sub21 = new PdfPCell(new Phrase("Net Payable :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub21.Border = 0;
            sub21.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub21);

            sub122 = new PdfPCell(new Phrase(dt1.Rows[0]["Amount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub122.Border = 0;
            sub122.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub122);

            //sub13 = new PdfPCell(new Phrase("Total Deduction :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            //sub13.Border = 0;
            //sub13.HorizontalAlignment = Element.ALIGN_LEFT;
            //Subhead.AddCell(sub13);

            //sub14 = new PdfPCell(new Phrase(dt_tot.Rows[0]["Total_Deduction"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            //sub14.Border = 0;
            //sub14.HorizontalAlignment = Element.ALIGN_LEFT;
            //Subhead.AddCell(sub14);

           

            sub21 = new PdfPCell(new Phrase("Date & Time :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub21.Border = 0;
            sub21.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub21);

            sub122 = new PdfPCell(new Phrase(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace('-', '/'), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub122.Border = 0;
            sub122.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub122);

            sub21 = new PdfPCell(new Phrase("Prepared By :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub21.Border = 0;
            sub21.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub21);

            sub122 = new PdfPCell(new Phrase(dt1.Rows[0]["Preparedby"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub122.Border = 0;
            sub122.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub122);


            document.Add(Subhead);

            #endregion

            #region data

            if (dt2.Rows.Count > 0)
            {
                //PdfPTable emp_details = new PdfPTable(8);
                PdfPTable emp_details = new PdfPTable(7);
                emp_details.DefaultCell.Padding = 4;
                //float[] widthsdet = new float[] { 5f, 15f, 8f, 8f, 8f, 8f, 8f, 9f };
                float[] widthsdet = new float[] { 5f, 15f, 8f, 8f, 8f, 8f, 9f };
                emp_details.SetWidths(widthsdet);
                emp_details.SpacingBefore = 20f;
                emp_details.WidthPercentage = 90f;
                PdfPCell SN = new PdfPCell(new Phrase("S.No", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                SN.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(SN);
                PdfPCell ty = new PdfPCell(new Phrase("Employee", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                ty.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(ty);
               
                PdfPCell nam = new PdfPCell(new Phrase("Salary", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                nam.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(nam);
                ty = new PdfPCell(new Phrase("No.of Days", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                ty.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(ty);
                PdfPCell da = new PdfPCell(new Phrase("Addition", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                da.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(da);
                PdfPCell inc = new PdfPCell(new Phrase("Deduction ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                inc.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(inc);
                //PdfPCell exp = new PdfPCell(new Phrase("Incentive", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                //exp.HorizontalAlignment = Element.ALIGN_CENTER;
                //emp_details.AddCell(exp);
                PdfPCell exptt = new PdfPCell(new Phrase("Total ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                exptt.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(exptt);

                int i = 0;

                foreach (DataRow rows in dt2.Rows)
                {
                    try
                    {
                        PdfPCell sn = new PdfPCell(new Phrase((++i).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        sn.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        emp_details.AddCell(sn);
                    }
                    catch (Exception ee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell TP = new PdfPCell(new Phrase(rows["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                   
                    try
                    {
                        PdfPCell REM = new PdfPCell(new Phrase(rows["Salary"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        REM.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        emp_details.AddCell(REM);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell TP = new PdfPCell(new Phrase(rows["EmployeeWorkedDays"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell DT = new PdfPCell(new Phrase(rows["Addition"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        emp_details.AddCell(DT);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell DT = new PdfPCell(new Phrase(rows["Deduction"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        emp_details.AddCell(DT);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    //try
                    //{
                    //    PdfPCell DT = new PdfPCell(new Phrase(rows["IncentiveAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    //    DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    //    emp_details.AddCell(DT);
                    //}
                    //catch (Exception eee)
                    //{
                    //    emp_details.AddCell("");
                    //}
                    try
                    {
                        PdfPCell DT = new PdfPCell(new Phrase(rows["TotalSalary"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        emp_details.AddCell(DT);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    
                }

                PdfPCell remarks = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
                remarks.Border = 0;
                remarks.Colspan = 8;
                remarks.MinimumHeight = 30f;
                remarks.HorizontalAlignment = Element.ALIGN_LEFT;
                emp_details.AddCell(remarks);

                 remarks = new PdfPCell(new Phrase("Remark : " + dt1.Rows[0]["Remarks"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
                remarks.Border = 0;
                remarks.Colspan = 8;
                remarks.MinimumHeight = 30f;
                remarks.HorizontalAlignment = Element.ALIGN_LEFT;
                emp_details.AddCell(remarks);

                document.Add(emp_details);

            }
            else
            {
                PdfPTable bill_details4 = new PdfPTable(1);
                bill_details4.DefaultCell.Padding = 4;
                bill_details4.SpacingBefore = 10;

                PdfPCell remarks = new PdfPCell(new Phrase("No Record", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
                remarks.Border = 0;
                remarks.Colspan = 1;
                remarks.HorizontalAlignment = Element.ALIGN_LEFT;
                bill_details4.AddCell(remarks);

                document.Add(bill_details4);
            }

            #endregion

            document.Close();
            HttpContext.Current.Response.Write(document);
            HttpContext.Current.Response.End();
        }

    }
}