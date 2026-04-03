using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AmarCentre.BAL;
using System.IO;
using System.Globalization;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace AmarCentre.Reports
{
    public partial class OBReportPdf : System.Web.UI.Page
    {
        Report_Bal rep1 = new Report_Bal();
        System_Utilities obj_common = new System_Utilities();

        protected void Page_Load(object sender, EventArgs e)
        {

            DataSet ds = rep1.OBExcelPdf();
            DataTable dt = ds.Tables[0];
            DataTable dt2_sum = ds.Tables[1];

            Document document = new Document(PageSize.A4, 20f, 20f, 0f, 20f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=OBPdf.pdf");
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
            document.Open();

            if (Application["PrintHeader"] != "")
            {

                PdfPTable ftrtbl = new PdfPTable(1);
                ftrtbl.DefaultCell.PaddingLeft = 10;
                ftrtbl.DefaultCell.FixedHeight = 130f;
                ftrtbl.DefaultCell.Border = 0;
                ftrtbl.SpacingAfter = 5f;
                ftrtbl.WidthPercentage = 100f;

                string imageURL = Server.MapPath("../UploadedImage/" + Application["PrintHeader"]);
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(new Uri(imageURL));
                //Resize image depend upon your need
                jpg.ScaleToFit(550f, 450f);
                //Give space before image
                //Give some space after the image
                jpg.Alignment = Element.ALIGN_CENTER;

                ftrtbl.AddCell(jpg);

                document.Add(ftrtbl);
            }
            PdfPTable incomzdvgsdzbg = new PdfPTable(1);
            incomzdvgsdzbg.DefaultCell.Padding = 4;
            incomzdvgsdzbg.WidthPercentage = 100f;
            PdfPCell cell1 = new PdfPCell(new Phrase("Opening Balance Statement", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            cell1.Border = 0;
            cell1.HorizontalAlignment = 1;
            incomzdvgsdzbg.AddCell(cell1);

            //PdfPCell cell2 = new PdfPCell(new Phrase(Convert.ToDateTime(ToDate).ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
            //cell2.Border = 0;
            //cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            //incomzdvgsdzbg.AddCell(cell2);


            PdfPCell docDate = new PdfPCell(new Phrase("Printed on : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace('-', '/'), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            docDate.Border = 0;
            docDate.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            incomzdvgsdzbg.AddCell(docDate);

            document.Add(incomzdvgsdzbg);

            if (dt.Rows.Count > 0)
            {
                PdfPTable income_details = new PdfPTable(6);
                income_details.DefaultCell.Padding = 4;
                income_details.SpacingBefore = 20;
                income_details.WidthPercentage = 100;
                float[] widths = new float[] { 5f, 10f, 20f, 10f, 10f, 10f };
                income_details.SetWidths(widths);

                PdfPCell Ser = new PdfPCell(new Phrase("Sl", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Ser.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Ser);
                Ser = new PdfPCell(new Phrase("Type", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Ser.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Ser);
                Ser = new PdfPCell(new Phrase("Name", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Ser.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Ser);
                Ser = new PdfPCell(new Phrase("Date", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Ser.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Ser);
                Ser = new PdfPCell(new Phrase("Receivable", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Ser.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Ser);
                Ser = new PdfPCell(new Phrase("Payable", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Ser.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Ser);

                int i = 0;

                foreach (DataRow rows in dt.Rows)
                {
                    try
                    {
                        PdfPCell typee = new PdfPCell(new Phrase((++i).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        typee.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        income_details.AddCell(typee);

                    }
                    catch (Exception eee)
                    {
                        income_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell typee = new PdfPCell(new Phrase(rows["Type"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        typee.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        income_details.AddCell(typee);

                    }
                    catch (Exception eee)
                    {
                        income_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell typee = new PdfPCell(new Phrase(rows["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        typee.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        income_details.AddCell(typee);

                    }
                    catch (Exception eee)
                    {
                        income_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell typee = new PdfPCell(new Phrase(rows["Date"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        typee.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        income_details.AddCell(typee);

                    }
                    catch (Exception eee)
                    {
                        income_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell typee = new PdfPCell(new Phrase(rows["Receivable"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        typee.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        income_details.AddCell(typee);

                    }
                    catch (Exception eee)
                    {
                        income_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell typee = new PdfPCell(new Phrase(rows["Payable"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        typee.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        income_details.AddCell(typee);

                    }
                    catch (Exception eee)
                    {
                        income_details.AddCell("");
                    }

                }

                PdfPCell Serttotdd = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Serttotdd.HorizontalAlignment = Element.ALIGN_RIGHT;
                Serttotdd.Colspan = 4;
                income_details.AddCell(Serttotdd);

                PdfPCell Serttot = new PdfPCell(new Phrase(dt2_sum.Rows[0]["Receivable"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Serttot.HorizontalAlignment = Element.ALIGN_RIGHT;
                income_details.AddCell(Serttot);
                PdfPCell Sert1 = new PdfPCell(new Phrase(dt2_sum.Rows[0]["Payable"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert1.HorizontalAlignment = Element.ALIGN_RIGHT;
                income_details.AddCell(Sert1);

                decimal net = dt2_sum.Rows[0]["NetAmount"].ToString()==""? 0:Convert.ToDecimal(dt2_sum.Rows[0]["NetAmount"].ToString());

                Serttotdd = new PdfPCell(new Phrase((net>=0)?"Net Receivable":"Net Payable", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Serttotdd.HorizontalAlignment = Element.ALIGN_RIGHT;
                Serttotdd.Colspan = 5;
                income_details.AddCell(Serttotdd);

                 Sert1 = new PdfPCell(new Phrase((net >= 0) ? (dt2_sum.Rows[0]["NetAmount"].ToString()):(net*-1).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert1.HorizontalAlignment = Element.ALIGN_RIGHT;
                income_details.AddCell(Sert1);

                document.Add(income_details);
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

            document.Close();
            HttpContext.Current.Response.Write(document);
            HttpContext.Current.Response.End();
        }

    }
}