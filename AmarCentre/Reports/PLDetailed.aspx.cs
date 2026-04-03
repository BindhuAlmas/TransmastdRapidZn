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
    public partial class PLDetailed : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            DateTime ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);

            DataSet ds = obj_report.PLDetailed(FromDate, ToDate);
            DataTable dtDetailed = ds.Tables[0];
           

            Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=ProfitLossStatement.pdf");
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

            PdfPCell HT00 = new PdfPCell(new Phrase("ProfitLoss Detailed Report ", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
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

            PdfPTable detailsTable = new PdfPTable(4);
            detailsTable.DefaultCell.Padding = 4;
            float[] detailsTableWidthsdet = new float[] {37f,13f,37f,13f};
            detailsTable.SetWidths(detailsTableWidthsdet);
            detailsTable.SpacingBefore = 10f;
            detailsTable.WidthPercentage = 95f;

            PdfPCell detailHead = new PdfPCell(new Phrase("Particulars", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("Expense", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("Particulars ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase(" Income", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailsTable.AddCell(detailHead);

            PdfPCell emptyDetail = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
            emptyDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            emptyDetail.Border = 0;

            PdfPCell dirExpenseCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));

           foreach (DataRow rows in dtDetailed.Rows)
            {
                try
                {
                    if (rows["mainorder"].ToString() == "1" || rows["mainorder"].ToString() == "3" || rows["mainorder"].ToString() == "6" || rows["mainorder"].ToString() == "7")
                        dirExpenseCell = new PdfPCell(new Phrase(rows["NameExpense"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    else
                    {
                        dirExpenseCell = new PdfPCell(new Phrase(rows["NameExpense"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        dirExpenseCell.PaddingLeft = 10f;
                    }
                    dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;

                    if (rows["mainorder"].ToString() == "6" || rows["mainorder"].ToString() == "7")
                        dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    detailsTable.AddCell(dirExpenseCell);
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }
                try
                {
                    if (rows["mainorder"].ToString() == "1" || rows["mainorder"].ToString() == "3" || rows["mainorder"].ToString() == "6" || rows["mainorder"].ToString() == "7")
                        dirExpenseCell = new PdfPCell(new Phrase(rows["Expense"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    else
                        dirExpenseCell = new PdfPCell(new Phrase(rows["Expense"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    dirExpenseCell.PaddingRight = 10f;
                    dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    detailsTable.AddCell(dirExpenseCell);
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }
                try
                {
                    if (rows["mainorder"].ToString() == "1" || rows["mainorder"].ToString() == "3" || rows["mainorder"].ToString() == "6" || rows["mainorder"].ToString() == "7")
                        dirExpenseCell = new PdfPCell(new Phrase(rows["NameIncome"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    else
                    {
                        dirExpenseCell = new PdfPCell(new Phrase(rows["NameIncome"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        dirExpenseCell.PaddingLeft = 10f;
                    }
                    dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    if (rows["mainorder"].ToString() == "6" || rows["mainorder"].ToString() == "7")
                        dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                   
                    detailsTable.AddCell(dirExpenseCell);
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }
                try
                {
                    if (rows["mainorder"].ToString() == "1" || rows["mainorder"].ToString() == "3" || rows["mainorder"].ToString() == "6" || rows["mainorder"].ToString() == "7")
                        dirExpenseCell = new PdfPCell(new Phrase(rows["Income"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    else
                        dirExpenseCell = new PdfPCell(new Phrase(rows["Income"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    dirExpenseCell.PaddingRight = 10f;
                    dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    detailsTable.AddCell(dirExpenseCell);
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
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