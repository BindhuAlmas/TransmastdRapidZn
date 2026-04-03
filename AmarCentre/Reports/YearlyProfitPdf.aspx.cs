using System;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using AmarCentre.BAL;

namespace AmarCentre.Reports
{
    public partial class YearlyProfitPdf : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();
        dtClass dtc = new dtClass();

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dtyear = dtc.returndtmultiple();

            DataSet ds = obj_report.GetYearlyProfit(dtyear); //Its only service profit
            DataTable dtdata = ds.Tables[0];
            dtdata.Columns.Remove("MonthId");

            Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=YearlyProfitPdf.pdf");
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

            PdfPTable headTable = new PdfPTable(1);
            headTable.DefaultCell.Padding = 4;
            float[] headTableWidths = new float[] { 100f };
            headTable.SetWidths(headTableWidths);
            headTable.WidthPercentage = 95f;

            PdfPCell HT00 = new PdfPCell(new Phrase("Profit Statement ", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            HT00.HorizontalAlignment = Element.ALIGN_CENTER;
            HT00.Border = 0;
            headTable.AddCell(HT00);

            PdfPCell sub00 = new PdfPCell(new Phrase("Date : " + DateTime.Now, new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub00.Border = 0;
            sub00.HorizontalAlignment = Element.ALIGN_RIGHT;
            headTable.AddCell(sub00);

            document.Add(headTable);


            #endregion

            #region data

            int ColCount = dtdata.Columns.Count;

            PdfPTable detailsTable = new PdfPTable(ColCount);
            detailsTable.DefaultCell.Padding = 4;
            detailsTable.SpacingBefore = 10f;
            detailsTable.WidthPercentage = 95f;

            PdfPCell detailHead = new PdfPCell(new Phrase("Month", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
            detailsTable.AddCell(detailHead);

            foreach (DataRow r in dtyear.Rows)
            {
                detailHead = new PdfPCell(new Phrase(r["YearId"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);
            }

            PdfPCell emptyDetail = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
            emptyDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            emptyDetail.Border = 0;
            PdfPCell dirExpenseCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
            dirExpenseCell.Border = 0;

            foreach (DataRow rows in dtdata.Rows)
            {
                try
                {
                    dirExpenseCell = new PdfPCell(new Phrase(rows["Month"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    dirExpenseCell.PaddingLeft = 5f;
                    detailsTable.AddCell(dirExpenseCell);
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }
                foreach (DataRow r in dtyear.Rows)
                {
                    try
                    {
                        dirExpenseCell = new PdfPCell(new Phrase(rows[r["YearId"].ToString()].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        dirExpenseCell.PaddingRight = 5f;
                        detailsTable.AddCell(dirExpenseCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }
                }
            }

            document.Add(detailsTable);


            #endregion

            document.Close();
            HttpContext.Current.Response.Write(document);
            HttpContext.Current.Response.End();
        }
    }
}