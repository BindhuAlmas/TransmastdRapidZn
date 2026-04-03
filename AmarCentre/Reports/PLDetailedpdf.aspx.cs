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
    public partial class PLDetailedpdf : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            DateTime ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            int StatusId = Convert.ToInt32(Request.QueryString["StatusId"]);

            DataSet ds = obj_report.ProfitLossStatementDetPdf(FromDate, ToDate, StatusId);
            DataTable dtDate = ds.Tables[0];
            DataTable dtDirectExpense = ds.Tables[1];
            //DataTable dtDirectIncome = ds.Tables[2];
            DataTable dtIndirectExpense = ds.Tables[2];
            DataTable dtIndirectIncome = ds.Tables[3];
            DataTable dtNet = ds.Tables[4];
            DataTable dtTotal = ds.Tables[5];

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

            PdfPCell HT00 = new PdfPCell(new Phrase("Profit & Loss Statement ", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            HT00.HorizontalAlignment = Element.ALIGN_CENTER;
            HT00.Border = 0;
            headTable.AddCell(HT00);
            /*End of Row*/
            document.Add(headTable);


            PdfPTable subHeadTable = new PdfPTable(1);
            subHeadTable.DefaultCell.Padding = 4;
            float[] subHeadTableWidths = new float[] { 120f };
            subHeadTable.SetWidths(subHeadTableWidths);
            subHeadTable.WidthPercentage = 95f;

            PdfPCell sub00 = new PdfPCell(new Phrase("Date : " + dtDate.Rows[0][0].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub00.Border = 0;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            subHeadTable.AddCell(sub00);
            /*End of Row*/

            sub00 = new PdfPCell(new Phrase("Period : " + dtDate.Rows[0][1].ToString() + " to " + dtDate.Rows[0][2].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub00.Border = 0;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            subHeadTable.AddCell(sub00);
            /*End of Row*/

            document.Add(subHeadTable);


            #endregion

            #region data
            PdfPTable detailsTable = new PdfPTable(4);
            detailsTable.DefaultCell.Padding = 4;
            float[] detailsTableWidthsdet = new float[] { 31f, 31f, 31f, 31f };
            detailsTable.SetWidths(detailsTableWidthsdet);
            detailsTable.SpacingBefore = 10f;
            detailsTable.WidthPercentage = 95f;

            PdfPCell emptyDetail = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
            emptyDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            emptyDetail.Border = 0;

            PdfPCell detailHead = new PdfPCell(new Phrase("Particulars", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
            detailHead.Colspan = 2;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("Particulars", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
            detailHead.Colspan = 2;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("Direct Expense", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            detailHead.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Colspan = 2;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("Direct Income", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            detailHead.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Colspan = 2;
            detailsTable.AddCell(detailHead);

            PdfPCell dirExpenseCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));

            foreach (DataRow rows in dtDirectExpense.Rows)
            {
                try
                {
                    if (rows["orders"].ToString() == "1")
                    {
                        dirExpenseCell = new PdfPCell(new Phrase(rows["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                        dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        dirExpenseCell.Border = PdfPCell.LEFT_BORDER;
                        detailsTable.AddCell(dirExpenseCell);
                    }
                    else if (rows["orders"].ToString() == "2")
                    {
                        dirExpenseCell = new PdfPCell(new Phrase(rows["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        dirExpenseCell.Border = PdfPCell.LEFT_BORDER;
                        dirExpenseCell.PaddingLeft = 5f;
                        detailsTable.AddCell(dirExpenseCell);
                    }
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }
                try
                {
                    if (rows["orders"].ToString() == "1")
                    {
                        dirExpenseCell = new PdfPCell(new Phrase(rows["ExpenseAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                        dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        dirExpenseCell.Border = PdfPCell.RIGHT_BORDER;
                        detailsTable.AddCell(dirExpenseCell);
                    }
                    else if (rows["orders"].ToString() == "2")
                    {
                        dirExpenseCell = new PdfPCell(new Phrase(rows["ExpenseAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        dirExpenseCell.Border = PdfPCell.RIGHT_BORDER;
                        detailsTable.AddCell(dirExpenseCell);
                    }
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }

                try
                {
                    if (rows["orders"].ToString() == "1")
                    {
                        dirExpenseCell = new PdfPCell(new Phrase(rows["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                        dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        dirExpenseCell.Border = PdfPCell.LEFT_BORDER;
                        detailsTable.AddCell(dirExpenseCell);
                    }
                    else if (rows["orders"].ToString() == "2")
                    {
                        dirExpenseCell = new PdfPCell(new Phrase(rows["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        dirExpenseCell.Border = PdfPCell.LEFT_BORDER;
                        dirExpenseCell.PaddingLeft = 5f;
                        detailsTable.AddCell(dirExpenseCell);
                    }
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }
                try
                {
                    if (rows["orders"].ToString() == "1")
                    {
                        dirExpenseCell = new PdfPCell(new Phrase(rows["IncomeAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                        dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        dirExpenseCell.Border = PdfPCell.RIGHT_BORDER;
                        detailsTable.AddCell(dirExpenseCell);
                    }
                    else if (rows["orders"].ToString() == "2")
                    {
                        dirExpenseCell = new PdfPCell(new Phrase(rows["IncomeAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        dirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        dirExpenseCell.Border = PdfPCell.RIGHT_BORDER;
                        detailsTable.AddCell(dirExpenseCell);
                    }
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }
            }

            detailHead = new PdfPCell(new Phrase("Gross Profit", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
            detailHead.Border = PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER;
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase(dtTotal.Rows[0][2].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
            detailHead.Border = PdfPCell.TOP_BORDER | PdfPCell.RIGHT_BORDER;
            detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            detailHead.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Colspan = 2;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("Indirect Expense", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            detailHead.Border = PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.RIGHT_BORDER;
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Colspan = 2;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("Indirect Income", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            detailHead.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Colspan = 2;
            detailsTable.AddCell(detailHead);

            PdfPTable indirExpenseTable = new PdfPTable(3);
            indirExpenseTable.DefaultCell.Padding = 2;
            float[] indirExpenseTableWidthsdet = new float[] { 2f, 30f, 30f };
            indirExpenseTable.SetWidths(indirExpenseTableWidthsdet);
            indirExpenseTable.SpacingBefore = 2f;
            indirExpenseTable.WidthPercentage = 95f;
            PdfPCell indirExpenseCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
            foreach (DataRow rows in dtIndirectExpense.Rows)
            {
                indirExpenseTable.AddCell(emptyDetail);
                try
                {
                    indirExpenseCell = new PdfPCell(new Phrase(rows["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    indirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    indirExpenseCell.Border = 0;
                    indirExpenseTable.AddCell(indirExpenseCell);
                }
                catch (Exception ee)
                {
                    indirExpenseTable.AddCell(emptyDetail);
                }
                try
                {
                    indirExpenseCell = new PdfPCell(new Phrase(rows["ExpenseAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    indirExpenseCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    indirExpenseCell.Border = 0;
                    indirExpenseTable.AddCell(indirExpenseCell);
                }
                catch (Exception ee)
                {
                    indirExpenseTable.AddCell(emptyDetail);
                }
            }
            detailHead = new PdfPCell(indirExpenseTable);
            detailHead.Colspan = 2;
            detailHead.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
            detailsTable.AddCell(detailHead);

            PdfPTable indirIncomeTable = new PdfPTable(3);
            indirIncomeTable.DefaultCell.Padding = 2;
            float[] indirIncomeTableWidthsdet = new float[] { 2f, 30f, 30f };
            indirIncomeTable.SetWidths(indirIncomeTableWidthsdet);
            indirIncomeTable.SpacingBefore = 2f;
            indirIncomeTable.WidthPercentage = 95f;
            PdfPCell indirIncomeCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
            foreach (DataRow rows in dtIndirectIncome.Rows)
            {
                indirIncomeTable.AddCell(emptyDetail);
                try
                {
                    indirIncomeCell = new PdfPCell(new Phrase(rows["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    indirIncomeCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    indirIncomeCell.Border = 0;
                    indirIncomeTable.AddCell(indirIncomeCell);
                }
                catch (Exception ee)
                {
                    indirIncomeTable.AddCell(emptyDetail);
                }
                try
                {
                    indirIncomeCell = new PdfPCell(new Phrase(rows["IncomeAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    indirIncomeCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    indirIncomeCell.Border = 0;
                    indirIncomeTable.AddCell(indirIncomeCell);
                }
                catch (Exception ee)
                {
                    indirIncomeTable.AddCell(emptyDetail);
                }
            }
            detailHead = new PdfPCell(indirIncomeTable);
            detailHead.Colspan = 2;
            detailHead.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("Net Profit", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            detailHead.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase(dtNet.Rows[0][0].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            detailHead.Border = PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
            detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("Net Loss", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            detailHead.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase(dtNet.Rows[0][1].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            detailHead.Border = PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
            detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            detailHead.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase(dtTotal.Rows[0][0].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            detailHead.Border = PdfPCell.BOTTOM_BORDER;
            detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            detailHead.Border = PdfPCell.BOTTOM_BORDER;
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase(dtTotal.Rows[0][1].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            detailHead.Border = PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER;
            detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
            detailsTable.AddCell(detailHead);

            document.Add(detailsTable);


            #endregion

            document.Close();
            HttpContext.Current.Response.Write(document);
            HttpContext.Current.Response.End();
        }
    }
}