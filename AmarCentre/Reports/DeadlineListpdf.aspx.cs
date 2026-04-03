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
    public partial class DeadlineListpdf : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {

            int? CustomerId = null;
            try
            {
                CustomerId = Convert.ToInt32(Request.QueryString["CustomerId"]);
            }
            catch (Exception xx)
            {
                CustomerId = null;
            }
            int? AgentId = null;
            try
            {
                AgentId = Convert.ToInt32(Request.QueryString["AgentId"]);
            }
            catch (Exception xx)
            {
                AgentId = null;
            }
            int? ServiceId = null;
            try
            {
                ServiceId = Convert.ToInt32(Request.QueryString["ServiceId"]);
            }
            catch (Exception xx)
            {
                ServiceId = null;
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


            DataSet ds = obj_report.DeadlineExcel(FromDate, ToDate, CustomerId, AgentId, ServiceId);
            DataTable dtDetails = ds.Tables[0];
            

            Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=DeadlineList.pdf");
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
            PdfPCell cell1 = new PdfPCell(new Phrase("DeadlineList", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            cell1.Border = 0;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            table1.AddCell(cell1);

            PdfPCell cell45 = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace('-', '/'), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            cell45.Border = 0;
            cell45.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            table1.AddCell(cell45);

            document.Add(table1);

            #endregion

            #region data

            if (dtDetails.Rows.Count > 0)
            {
                PdfPTable emp_details = new PdfPTable(8);
                emp_details.DefaultCell.Padding = 4;
                emp_details.WidthPercentage = 100;
                emp_details.SpacingBefore = 10f;
                float[] widths = new float[] { 5f, 30f, 25f, 15f, 17f, 25f, 25f, 25f };
                emp_details.SetWidths(widths);

                PdfPCell SN = new PdfPCell(new Phrase("Sl", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                SN.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(SN);

                PdfPCell tp = new PdfPCell(new Phrase("Customer", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tp.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(tp);
                tp = new PdfPCell(new Phrase("Agent", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tp.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(tp);
                PdfPCell tp2 = new PdfPCell(new Phrase("Invoice", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tp2.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(tp2);
                PdfPCell app = new PdfPCell(new Phrase("Invoice Date", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                app.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(app);

                app = new PdfPCell(new Phrase("Service", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                app.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(app);

                app = new PdfPCell(new Phrase("Particular", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                app.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(app);

                app = new PdfPCell(new Phrase("Deadline date", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                app.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(app);


                int i = 0;
                foreach (DataRow rows in dtDetails.Rows)
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
                        PdfPCell TP = new PdfPCell(new Phrase(rows["Agent"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell TP = new PdfPCell(new Phrase(rows["Code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
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
                        PdfPCell TP = new PdfPCell(new Phrase(rows["InvoiceDate"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
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
                        PdfPCell TP = new PdfPCell(new Phrase(rows["Service"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        TP.PaddingLeft = 10f;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }

                    try
                    {
                        PdfPCell TP = new PdfPCell(new Phrase(rows["Particulars"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }

                    try
                    {
                        PdfPCell TP = new PdfPCell(new Phrase(rows["deadline"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }

                }

                document.Add(emp_details);
            }

            else
            {
                PdfPTable bill_details4 = new PdfPTable(1);
                bill_details4.DefaultCell.Padding = 5;
                bill_details4.SpacingBefore = 8;

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