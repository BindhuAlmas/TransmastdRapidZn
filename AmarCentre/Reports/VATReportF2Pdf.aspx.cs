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
    public partial class VATReportF2Pdf : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {

            int? EmirateId = null;
            try
            {
                EmirateId = Convert.ToInt32(Request.QueryString["EmirateId"]);
            }
            catch (Exception xx)
            {
                EmirateId = null;
            }

            DateTime? FromDate = null;
            try
            {
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }
            catch (Exception xx)
            {
                FromDate = null;
            }
            DateTime? ToDate = null;
            try
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }
            catch (Exception xx)
            {
                ToDate = null;
            }
            DataSet ds = obj_report.VATReportF2Excel(FromDate, ToDate, EmirateId);
            DataTable dt1 = ds.Tables[0];
            DataTable dt = ds.Tables[1];
            DataTable dtsum = ds.Tables[2];

            Document document = new Document(PageSize.A4, 20f, 20f, 10f, 10f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=VatReport.pdf");
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
            document.Open();
            if (Application["PrintHeader"] != "")
            {

                string imageURL = Server.MapPath("../UploadedImage/" + Application["PrintHeader"]);
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(new Uri(imageURL));
                jpg.ScaleToFit(550f, 450f);
                jpg.SpacingAfter = 5f;
                jpg.Alignment = Element.ALIGN_CENTER;

                document.Add(jpg);
            }

            #region header

            PdfPTable table1 = new PdfPTable(1);
            table1.DefaultCell.Padding = 4;
            table1.WidthPercentage = 100;
            PdfPCell cell1 = new PdfPCell(new Phrase("List of Invoice", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            cell1.Border = 0;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            table1.AddCell(cell1);

            //PdfPCell cell45 = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace('-', '/'), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            //cell45.Border = 0;
            //cell45.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            //table1.AddCell(cell45);

            PdfPCell cell45 = new PdfPCell(new Phrase("Start Date : " + dt1.Rows[0]["FromDates"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            cell45.Border = 0;
            cell45.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            table1.AddCell(cell45);

            cell45 = new PdfPCell(new Phrase("End Date : " + dt1.Rows[0]["ToDates"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            cell45.Border = 0;
            cell45.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            table1.AddCell(cell45);

            if (EmirateId != null && dt.Rows.Count > 0)
            {
                cell45 = new PdfPCell(new Phrase("City : " + dt.Rows[0]["City"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                cell45.Border = 0;
                cell45.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                table1.AddCell(cell45);
            }

            document.Add(table1);

            #endregion

            #region data

            if (dt.Rows.Count > 0)
            {
                PdfPTable emp_details = new PdfPTable(10);
                emp_details.DefaultCell.Padding = 4;
                emp_details.WidthPercentage = 100;
                emp_details.SpacingBefore = 5f;
                float[] widths = new float[] { 6f, 10f, 10f, 25f, 10f, 14f, 10f, 10f, 10f, 10f };
                emp_details.SetWidths(widths);
                PdfPCell SN = new PdfPCell(new Phrase("Sl", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                SN.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(SN);

                PdfPCell tp = new PdfPCell(new Phrase("Date", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tp.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(tp);
                PdfPCell tp2 = new PdfPCell(new Phrase("Invoice", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tp2.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(tp2);
                PdfPCell tp3 = new PdfPCell(new Phrase("Customer", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tp3.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(tp3);
                PdfPCell app = new PdfPCell(new Phrase("TRN", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                app.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(app);

                app = new PdfPCell(new Phrase("City", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                app.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(app);

                app = new PdfPCell(new Phrase("Non Taxable Amount", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                app.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(app);

                app = new PdfPCell(new Phrase("Taxable Amount", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                app.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(app);

                app = new PdfPCell(new Phrase("VAT", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                app.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(app);

                app = new PdfPCell(new Phrase("Total Amount", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                app.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(app);


                int i = 0;
                foreach (DataRow rows in dt.Rows)
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
                        PdfPCell TP = new PdfPCell(new Phrase(rows["Date"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell TP = new PdfPCell(new Phrase(rows["RefNo"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }

                    try
                    {
                        PdfPCell TP = new PdfPCell(new Phrase(rows["Customer"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }

                    try
                    {
                        PdfPCell TP = new PdfPCell(new Phrase(rows["TRN"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        TP.PaddingLeft = 10f;
                        TP.MinimumHeight = 10f;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }

                    try
                    {
                        PdfPCell TP = new PdfPCell(new Phrase(rows["City"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        TP.PaddingLeft = 10f;
                        TP.MinimumHeight = 10f;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell TP = new PdfPCell(new Phrase(rows["NonTaxableAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        TP.PaddingRight =5f;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell TP = new PdfPCell(new Phrase(rows["TaxableAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        TP.PaddingRight = 5f;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell TP = new PdfPCell(new Phrase(rows["TaxAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        TP.PaddingRight = 5f;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }

                    try
                    {
                        PdfPCell TP = new PdfPCell(new Phrase(rows["TotalAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        TP.PaddingRight = 5f;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                }

                PdfPCell total = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                total.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                total.Colspan = 6;
                total.PaddingRight = 10f;
                emp_details.AddCell(total);

                PdfPCell total1 = new PdfPCell(new Phrase(dtsum.Rows[0]["NonTaxableAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                total1.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                emp_details.AddCell(total1);

                PdfPCell total2 = new PdfPCell(new Phrase(dtsum.Rows[0]["TaxableAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                total2.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                emp_details.AddCell(total2);

                PdfPCell total3 = new PdfPCell(new Phrase(dtsum.Rows[0]["TaxAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                total3.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                emp_details.AddCell(total3);

                total3 = new PdfPCell(new Phrase(dtsum.Rows[0]["TotalAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                total3.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                emp_details.AddCell(total3);


                document.Add(emp_details);
            }

            else
            {
                PdfPTable bill_details4 = new PdfPTable(1);
                bill_details4.DefaultCell.Padding = 5;
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