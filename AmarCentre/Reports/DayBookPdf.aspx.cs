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
    public partial class DayBookPdf : System.Web.UI.Page
    {
        Report_Bal rep1 = new Report_Bal();
        System_Utilities obj_common = new System_Utilities();

        protected void Page_Load(object sender, EventArgs e)
        {
            
            ReportGen();
        }

        public void ReportGen()
        {
            DateTime ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            DateTime FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            int? userid = null;
            try
            {
                userid = Convert.ToInt32(Request.QueryString["userid"]);
            }
            catch (Exception ex) { userid = null; }

            DataSet ds = rep1.DayBookdetail(FromDate,ToDate, userid);
            DataTable dt = ds.Tables[0];
            DataTable dt2_sum = ds.Tables[1];

            Document document = new Document(PageSize.A4, 20f, 20f, 0f, 20f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=DayBook.pdf");
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
            PdfPCell cell1 = new PdfPCell(new Phrase("Day Book", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
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
                PdfPTable income_details = new PdfPTable(9);
                income_details.DefaultCell.Padding = 4;
                income_details.SpacingBefore = 20;
                income_details.WidthPercentage = 100;
                float[] widths = new float[] { 5f, 10f, 10f,  10f, 10f,20f,10f,10f,10f};
                income_details.SetWidths(widths);

                PdfPCell Sert = new PdfPCell(new Phrase("Sl", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Sert);
                PdfPCell Ser = new PdfPCell(new Phrase("Date & Time", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Ser.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Ser);
                Ser = new PdfPCell(new Phrase("Ref No", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Ser.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Ser);
                Ser = new PdfPCell(new Phrase("Account Type", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Ser.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Ser);
                 Ser = new PdfPCell(new Phrase("Account Name", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Ser.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Ser);
                 Ser = new PdfPCell(new Phrase("Remark", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Ser.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Ser);
                Ser = new PdfPCell(new Phrase("Done By", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Ser.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Ser);
                Ser = new PdfPCell(new Phrase("Debit", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Ser.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Ser);
                Ser = new PdfPCell(new Phrase("Credit", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Ser.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Ser);
              

                foreach (DataRow rows in dt.Rows)
                {
                    try
                    {
                        PdfPCell typee = new PdfPCell(new Phrase(rows["Sl"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        typee.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        income_details.AddCell(typee);

                    }
                    catch (Exception eee)
                    {
                        income_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell typee = new PdfPCell(new Phrase(rows["DateTime"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        typee.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        income_details.AddCell(typee);

                    }
                    catch (Exception eee)
                    {
                        income_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell typee = new PdfPCell(new Phrase(rows["Code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        typee.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        income_details.AddCell(typee);

                    }
                    catch (Exception eee)
                    {
                        income_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell typee = new PdfPCell(new Phrase(rows["AccountType"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        typee.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        income_details.AddCell(typee);

                    }
                    catch (Exception eee)
                    {
                        income_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell typee = new PdfPCell(new Phrase(rows["AccountName"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        typee.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        income_details.AddCell(typee);

                    }
                    catch (Exception eee)
                    {
                        income_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell typee = new PdfPCell(new Phrase(rows["Remark"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        typee.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        income_details.AddCell(typee);

                    }
                    catch (Exception eee)
                    {
                        income_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell typee = new PdfPCell(new Phrase(rows["Doneby"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        typee.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        income_details.AddCell(typee);

                    }
                    catch (Exception eee)
                    {
                        income_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell typee = new PdfPCell(new Phrase(rows["Debit"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        typee.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        income_details.AddCell(typee);

                    }
                    catch (Exception eee)
                    {
                        income_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell typee = new PdfPCell(new Phrase(rows["Credit"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
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
                Serttotdd.Colspan = 7;
                income_details.AddCell(Serttotdd);

                PdfPCell Serttot = new PdfPCell(new Phrase(dt2_sum.Rows[0]["Debit"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Serttot.HorizontalAlignment = Element.ALIGN_RIGHT;
                income_details.AddCell(Serttot);
                PdfPCell Sert1 = new PdfPCell(new Phrase(dt2_sum.Rows[0]["Credit"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
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