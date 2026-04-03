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
    public partial class DebitShortList : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            DateTime ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);

            DataSet ds = obj_report.PLDetailed(FromDate, ToDate);
            DataTable dtSummary = ds.Tables[2];

            Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=DebitList.pdf");
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
            BaseFont bfTimes = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Font/arabtype.ttf"), BaseFont.IDENTITY_H, true);
            // BaseFont bfTimes = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Font/arialuni.ttf"), BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font arbfnt = new iTextSharp.text.Font(bfTimes, 10, Font.NORMAL);
            iTextSharp.text.Font arbsmallbold = new iTextSharp.text.Font(bfTimes, 8, Font.BOLD);
            iTextSharp.text.Font arbfntbld = new iTextSharp.text.Font(bfTimes, 15, Font.BOLD);

            #region header

            PdfPTable headTable = new PdfPTable(1);
            headTable.DefaultCell.Padding = 4;
            float[] headTableWidths = new float[] { 120f };
            headTable.SetWidths(headTableWidths);
            headTable.WidthPercentage = 95f;

            PdfPCell HT00 = new PdfPCell(new Phrase("Debit Shortlist ", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            HT00.HorizontalAlignment = Element.ALIGN_CENTER;
            HT00.Border = 0;
            headTable.AddCell(HT00);
            /*End of Row*/

            PdfPCell sub00 = new PdfPCell(new Phrase("From: " + Convert.ToDateTime(FromDate).ToString("dd/MM/yyyy") + " To: " + Convert.ToDateTime(ToDate).ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
            sub00.Border = 0;
            sub00.HorizontalAlignment = Element.ALIGN_CENTER;
            headTable.AddCell(sub00);
            /*End of Row*/

            sub00 = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace('-', '/'), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub00.Border = 0;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            headTable.AddCell(sub00);
            /*End of Row*/

            document.Add(headTable);


            #endregion

            #region data

            if (dtSummary.Rows.Count > 0)
            {
                PdfPTable detailsTable = new PdfPTable(4);
                detailsTable.DefaultCell.Padding = 4;
                float[] detailsTableWidthsdet = new float[] { 10f, 50f, 20f, 20f };
                detailsTable.SetWidths(detailsTableWidthsdet);
                detailsTable.SpacingBefore = 10f;
                detailsTable.WidthPercentage = 95f;

                PdfPCell detailHead = new PdfPCell(new Phrase("Sl No", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Name", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Contact No", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Amount ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                PdfPCell emptyDetail = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                emptyDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                emptyDetail.Border = 0;

                PdfPCell dirExpenseCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));

                decimal Total = 0;
                int i = 0;
                foreach (DataRow rows in dtSummary.Rows)
                {
                    try
                    {
                        dirExpenseCell = new PdfPCell(new Phrase((++i).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        dirExpenseCell.PaddingLeft = 5f;
                        detailsTable.AddCell(dirExpenseCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }
                    try
                    {
                        dirExpenseCell = new PdfPCell(new Phrase(rows["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        dirExpenseCell.PaddingLeft = 5f;
                        detailsTable.AddCell(dirExpenseCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }
                    try
                    {
                        dirExpenseCell = new PdfPCell(new Phrase(rows["Mobile_num"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        dirExpenseCell.PaddingLeft = 5f;
                        detailsTable.AddCell(dirExpenseCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }
                    try
                    {
                        dirExpenseCell = new PdfPCell(new Phrase(rows["Amount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        dirExpenseCell.PaddingRight = 5f;
                        detailsTable.AddCell(dirExpenseCell);

                        Total = Total + Convert.ToDecimal(rows["Amount"]);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }
                }

                dirExpenseCell = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                dirExpenseCell.Colspan = 3;
                dirExpenseCell.PaddingRight = 5f;
                detailsTable.AddCell(dirExpenseCell);

                dirExpenseCell = new PdfPCell(new Phrase(Total.ToString("0.00"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                dirExpenseCell.PaddingRight = 5f;
                detailsTable.AddCell(dirExpenseCell);

                document.Add(detailsTable);
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