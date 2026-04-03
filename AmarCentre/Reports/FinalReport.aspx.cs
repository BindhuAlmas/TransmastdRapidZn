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
    public partial class FinalReport : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            int? year = null, FromMnth = null, ToMnth = null;
            try
            {
                year = Convert.ToInt32(Request.QueryString["year"]);
            }
            catch { year = 0; }
            try
            {
                FromMnth = Convert.ToInt32(Request.QueryString["FromMnth"]);
            }
            catch { FromMnth = 0; }
            try
            {
                ToMnth = Convert.ToInt32(Request.QueryString["ToMnth"]);
            }
            catch { ToMnth = 0; }


            DataSet ds = obj_report.FinalVATReportPdf(year, FromMnth, ToMnth);
            DataTable dtgen = ds.Tables[0];
            DataTable dtsales = ds.Tables[1];
            DataTable dtsalesSum = ds.Tables[2];
            DataTable dtpur = ds.Tables[3];
            DataTable dtpurSum = ds.Tables[4];
            DataTable dtexp = ds.Tables[5];
            DataTable dtexpSum = ds.Tables[6];
            DataTable dtPESum = ds.Tables[7];
            DataTable dtPTSum = ds.Tables[8];
            DataTable dtemirate = ds.Tables[9];

            Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=VatReport.pdf");
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


            iTextSharp.text.BaseColor Red = new iTextSharp.text.BaseColor(System.Drawing.Color.Brown);
            iTextSharp.text.BaseColor LightGray = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            iTextSharp.text.BaseColor lightrred = new iTextSharp.text.BaseColor(255, 239, 213);
            iTextSharp.text.BaseColor yellow = new iTextSharp.text.BaseColor(System.Drawing.Color.Yellow);
            iTextSharp.text.BaseColor white = new iTextSharp.text.BaseColor(System.Drawing.Color.White);
            iTextSharp.text.BaseColor navblue = new iTextSharp.text.BaseColor(54, 90, 118);
            iTextSharp.text.BaseColor lightnavblue = new iTextSharp.text.BaseColor(149, 186, 215);


            #region header

            PdfPTable headTable = new PdfPTable(1);
            headTable.DefaultCell.Padding = 4;
            float[] headTableWidths = new float[] { 100f };
            headTable.SetWidths(headTableWidths);
            headTable.SpacingAfter = 10f;
            headTable.WidthPercentage = 95f;

            PdfPCell HT00 = new PdfPCell(new Phrase(dtgen.Rows[0]["TRNstring"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD, Red)));
            HT00.HorizontalAlignment = Element.ALIGN_LEFT;
            HT00.Border = 0;
            headTable.AddCell(HT00);

            HT00 = new PdfPCell(new Phrase(dtgen.Rows[0]["GIBANstring"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD, Red)));
            HT00.HorizontalAlignment = Element.ALIGN_LEFT;
            HT00.Border = 0;
            headTable.AddCell(HT00);

            HT00 = new PdfPCell(new Phrase(dtgen.Rows[0]["MonthString"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD, Red)));
            HT00.HorizontalAlignment = Element.ALIGN_LEFT;
            HT00.Border = 0;
            headTable.AddCell(HT00);

            HT00 = new PdfPCell(new Phrase("FINAL REPORT", new Font(Font.FontFamily.TIMES_ROMAN, 13, Font.BOLD)));
            HT00.HorizontalAlignment = Element.ALIGN_CENTER;
            HT00.VerticalAlignment = Element.ALIGN_MIDDLE;
            HT00.Border = 0;
            HT00.BackgroundColor = LightGray;
            HT00.MinimumHeight = 20f;
            headTable.AddCell(HT00);

            /*End of Row*/
            document.Add(headTable);

            #endregion

            PdfPTable detailsTable = new PdfPTable(4);
            detailsTable.DefaultCell.Padding = 4;
            float[] detailsTableWidthsdet = new float[] { 45f, 20f, 15f, 15f };
            detailsTable.SetWidths(detailsTableWidthsdet);
            detailsTable.SpacingAfter = 10f;
            detailsTable.WidthPercentage = 95f;

            PdfPCell detailHead = new PdfPCell(new Phrase("DESCRIPTION", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
            detailHead.Border = 0;
            detailHead.MinimumHeight = 17f;
            detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
            detailHead.BackgroundColor = LightGray;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("TAXABLE AMOUNT", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
            detailHead.Border = 0;
            detailHead.BackgroundColor = LightGray;
            detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("TAX 5%", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
            detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
            detailHead.Border = 0;
            detailHead.BackgroundColor = LightGray;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("GRAND TOTAL", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
            detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
            detailHead.Border = 0;
            detailHead.BackgroundColor = LightGray;
            detailsTable.AddCell(detailHead);

            PdfPCell emptyDetail = new PdfPCell(new Phrase("", new Font(Font.FontFamily.UNDEFINED, 9, Font.NORMAL)));
            emptyDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            emptyDetail.Border = 0;

            PdfPCell detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.UNDEFINED, 9, Font.NORMAL)));
            detailCell.Border = 0;

            PdfPCell detailCellhyp = new PdfPCell(new Phrase("-", new Font(Font.FontFamily.UNDEFINED, 9, Font.NORMAL)));
            detailCellhyp.Border = 0;
            detailCellhyp.HorizontalAlignment = Element.ALIGN_RIGHT;

            PdfPCell detailCellhypT = new PdfPCell(new Phrase("-", new Font(Font.FontFamily.UNDEFINED, 9, Font.NORMAL)));
            detailCellhypT.BackgroundColor = lightrred;
            detailCellhypT.HorizontalAlignment = Element.ALIGN_RIGHT;
            detailCellhypT.Border = 0;

            #region SALES (LOCAL)

            detailHead = new PdfPCell(new Phrase("SALES (LOCAL)", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Colspan = 4;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);

            foreach (DataRow rows in dtsales.Rows)
            {
                try
                {
                    detailCell = new PdfPCell(new Phrase(rows["MonthNme"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.ITALIC)));
                    detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    detailCell.Border = 0;
                    detailsTable.AddCell(detailCell);
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }
                try
                {
                    detailCell = new PdfPCell(new Phrase(rows["Taxable"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.ITALIC)));
                    detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    detailCell.Border = 0;
                    detailsTable.AddCell(detailCell);
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }
                try
                {
                    detailCell = new PdfPCell(new Phrase(rows["TaxAmount"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.ITALIC)));
                    detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    detailCell.Border = 0;
                    detailsTable.AddCell(detailCell);
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }
                try
                {
                    detailCell = new PdfPCell(new Phrase(rows["Total"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.ITALIC)));
                    detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    detailCell.Border = 0;
                    detailsTable.AddCell(detailCell);
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }
            }
            detailCell = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
            detailCell.BackgroundColor = lightrred;
            detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            detailCell.Border = 0;
            detailsTable.AddCell(detailCell);

            try
            {
                detailCell = new PdfPCell(new Phrase(dtsalesSum.Rows[0]["Taxable"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.BackgroundColor = lightrred;
                detailCell.Border = 0;
                detailsTable.AddCell(detailCell);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }
            try
            {
                detailCell = new PdfPCell(new Phrase(dtsalesSum.Rows[0]["TaxAmount"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.BackgroundColor = lightrred;
                detailCell.Border = 0;
                detailsTable.AddCell(detailCell);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }
            try
            {
                detailCell = new PdfPCell(new Phrase(dtsalesSum.Rows[0]["Total"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.BackgroundColor = lightrred;
                detailCell.Border = 0;
                detailsTable.AddCell(detailCell);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }

            #endregion

            #region SALES (no tax)

            detailHead = new PdfPCell(new Phrase("SALE (EXPORT/WITH OUT TAX)", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Colspan = 4;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);

            foreach (DataRow rows in dtsales.Rows)
            {
                try
                {
                    detailCell = new PdfPCell(new Phrase(rows["MonthNme"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.ITALIC)));
                    detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    detailCell.Border = 0;
                    detailsTable.AddCell(detailCell);
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }

                detailsTable.AddCell(detailCellhyp);
                detailsTable.AddCell(detailCellhyp);
                detailsTable.AddCell(detailCellhyp);
            }
            detailCell = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.UNDEFINED,8, Font.BOLD)));
            detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            detailCell.BackgroundColor = lightrred;
            detailCell.Border = 0;
            detailsTable.AddCell(detailCell);

            detailsTable.AddCell(detailCellhypT);
            detailsTable.AddCell(detailCellhypT);
            detailsTable.AddCell(detailCellhypT);

            #endregion

            detailHead = new PdfPCell(new Phrase("GRAND TOTAL", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
            detailHead.Border = 0;
            detailHead.MinimumHeight = 17f;
            detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
            detailHead.BackgroundColor = LightGray;
            detailsTable.AddCell(detailHead);

            try
            {
                detailHead = new PdfPCell(new Phrase(dtsalesSum.Rows[0]["Taxable"].ToString(), new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailHead.BackgroundColor = LightGray;
                detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }

            try
            {
                detailHead = new PdfPCell(new Phrase(dtsalesSum.Rows[0]["TaxAmount"].ToString(), new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
                detailHead.Border = 0;
                detailHead.BackgroundColor = LightGray;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }
            try
            {
                detailHead = new PdfPCell(new Phrase(dtsalesSum.Rows[0]["Total"].ToString(), new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
                detailHead.Border = 0;
                detailHead.BackgroundColor = LightGray;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }

            #region Purchase (LOCAL)

            detailHead = new PdfPCell(new Phrase("PURCHASE (LOCAL)", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Colspan = 4;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);

            foreach (DataRow rows in dtpur.Rows)
            {
                try
                {
                    detailCell = new PdfPCell(new Phrase(rows["MonthNme"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.ITALIC)));
                    detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    detailCell.Border = 0;
                    detailsTable.AddCell(detailCell);
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }
                try
                {
                    detailCell = new PdfPCell(new Phrase(rows["Taxable"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.ITALIC)));
                    detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    detailCell.Border = 0;
                    detailsTable.AddCell(detailCell);
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }
                try
                {
                    detailCell = new PdfPCell(new Phrase(rows["TaxAmount"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.ITALIC)));
                    detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    detailCell.Border = 0;
                    detailsTable.AddCell(detailCell);
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }
                try
                {
                    detailCell = new PdfPCell(new Phrase(rows["Total"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.ITALIC)));
                    detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    detailCell.Border = 0;
                    detailsTable.AddCell(detailCell);
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }
            }
            detailCell = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.UNDEFINED, 8, Font.BOLD)));
            detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            detailCell.BackgroundColor = lightrred;
            detailCell.Border = 0;
            detailsTable.AddCell(detailCell);

            try
            {
                detailCell = new PdfPCell(new Phrase(dtpurSum.Rows[0]["Taxable"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.BackgroundColor = lightrred;
                detailCell.Border = 0;
                detailsTable.AddCell(detailCell);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }
            try
            {
                detailCell = new PdfPCell(new Phrase(dtpurSum.Rows[0]["TaxAmount"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.BackgroundColor = lightrred;
                detailCell.Border = 0;
                detailsTable.AddCell(detailCell);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }
            try
            {
                detailCell = new PdfPCell(new Phrase(dtpurSum.Rows[0]["Total"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.BackgroundColor = lightrred;
                detailCell.Border = 0;
                detailsTable.AddCell(detailCell);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }

            #endregion

            #region Purcahse (no tax)

            detailHead = new PdfPCell(new Phrase("PURCHASE (IMPORT/WITH OUT TAX)", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Colspan = 4;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);

            foreach (DataRow rows in dtsales.Rows)
            {
                try
                {
                    detailCell = new PdfPCell(new Phrase(rows["MonthNme"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.ITALIC)));
                    detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    detailCell.Border = 0;
                    detailsTable.AddCell(detailCell);
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }

                detailsTable.AddCell(detailCellhyp);
                detailsTable.AddCell(detailCellhyp);
                detailsTable.AddCell(detailCellhyp);
            }
            detailCell = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.UNDEFINED, 8, Font.BOLD)));
            detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            detailCell.Border = 0;
            detailCell.BackgroundColor = lightrred;
            detailsTable.AddCell(detailCell);

            detailsTable.AddCell(detailCellhypT);
            detailsTable.AddCell(detailCellhypT);
            detailsTable.AddCell(detailCellhypT);

            #endregion

            #region EXPENSE

            detailHead = new PdfPCell(new Phrase("EXPENSE", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Colspan = 4;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);

            foreach (DataRow rows in dtexp.Rows)
            {
                try
                {
                    detailCell = new PdfPCell(new Phrase(rows["MonthNme"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.ITALIC)));
                    detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    detailCell.Border = 0;
                    detailsTable.AddCell(detailCell);
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }
                try
                {
                    detailCell = new PdfPCell(new Phrase(rows["Taxable"].ToString(), new Font(Font.FontFamily.UNDEFINED,8, Font.ITALIC)));
                    detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    detailCell.Border = 0;
                    detailsTable.AddCell(detailCell);
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }
                try
                {
                    detailCell = new PdfPCell(new Phrase(rows["TaxAmount"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.ITALIC)));
                    detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    detailCell.Border = 0;
                    detailsTable.AddCell(detailCell);
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }
                try
                {
                    detailCell = new PdfPCell(new Phrase(rows["Total"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.ITALIC)));
                    detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    detailCell.Border = 0;
                    detailsTable.AddCell(detailCell);
                }
                catch (Exception ee)
                {
                    detailsTable.AddCell(emptyDetail);
                }
            }
            detailCell = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.UNDEFINED, 8, Font.BOLD)));
            detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            detailCell.BackgroundColor = lightrred;
            detailCell.Border = 0;
            detailsTable.AddCell(detailCell);

            try
            {
                detailCell = new PdfPCell(new Phrase(dtexpSum.Rows[0]["Taxable"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.BackgroundColor = lightrred;
                detailCell.Border = 0;
                detailsTable.AddCell(detailCell);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }
            try
            {
                detailCell = new PdfPCell(new Phrase(dtexpSum.Rows[0]["TaxAmount"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.BackgroundColor = lightrred;
                detailCell.Border = 0;
                detailsTable.AddCell(detailCell);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }
            try
            {
                detailCell = new PdfPCell(new Phrase(dtexpSum.Rows[0]["Total"].ToString(), new Font(Font.FontFamily.UNDEFINED, 8, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.BackgroundColor = lightrred;
                detailCell.Border = 0;
                detailsTable.AddCell(detailCell);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }

            #endregion

            detailHead = new PdfPCell(new Phrase("GRAND TOTAL", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
            detailHead.Border = 0;
            detailHead.MinimumHeight = 17f;
            detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
            detailHead.BackgroundColor = LightGray;
            detailsTable.AddCell(detailHead);
            try
            {
                detailHead = new PdfPCell(new Phrase(dtPESum.Rows[0]["Taxable"].ToString(), new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailHead.BackgroundColor = LightGray;
                detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }
            try
            {
                detailHead = new PdfPCell(new Phrase(dtPESum.Rows[0]["TaxAmount"].ToString(), new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
                detailHead.Border = 0;
                detailHead.BackgroundColor = LightGray;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }
            try
            {
                detailHead = new PdfPCell(new Phrase(dtPESum.Rows[0]["Total"].ToString(), new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
                detailHead.Border = 0;
                detailHead.BackgroundColor = LightGray;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }
            try
            {
                detailHead = new PdfPCell(new Phrase("", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                detailHead.Border = 0;
                detailHead.MinimumHeight = 10f;
                detailHead.Colspan = 4;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }


            detailHead = new PdfPCell(new Phrase("PAYABLE TAX AMOUNT", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailHead.MinimumHeight = 17f;
            detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
            detailHead.BackgroundColor = LightGray;
            detailsTable.AddCell(detailHead);

            try
            {
                detailHead = new PdfPCell(new Phrase("(" + dtPTSum.Rows[0]["PayableTaxSum"].ToString() + ")", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailHead.Colspan = 3;
                detailHead.MinimumHeight = 17f;
                detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
                detailHead.BackgroundColor = yellow;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailHead = new PdfPCell(new Phrase("", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailHead.Colspan = 3;
                detailHead.MinimumHeight = 17f;
                detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
                detailHead.BackgroundColor = yellow;
                detailsTable.AddCell(detailHead);
            }

            document.Add(detailsTable);


            #region item

            detailsTable = new PdfPTable(4);
            detailsTable.DefaultCell.Padding = 4;
            detailsTableWidthsdet = new float[] { 45f, 20f, 15f, 15f };
            detailsTable.SetWidths(detailsTableWidthsdet);
            detailsTable.SpacingAfter = 20f;
            detailsTable.WidthPercentage = 95f;

            detailHead = new PdfPCell(new Phrase("Items", new Font(Font.FontFamily.UNDEFINED, 7, Font.BOLD, white)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
            detailHead.BackgroundColor = navblue;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("Amount (AED)", new Font(Font.FontFamily.UNDEFINED, 7, Font.BOLD, white)));
            detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
            detailHead.Border = 0;
            detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
            detailHead.BackgroundColor = navblue;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("VAT Amount (AED)", new Font(Font.FontFamily.UNDEFINED, 7, Font.BOLD, white)));
            detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
            detailHead.Border = 0;
            detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
            detailHead.BackgroundColor = navblue;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("Adjustment (AED)", new Font(Font.FontFamily.UNDEFINED, 7, Font.BOLD, white)));
            detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
            detailHead.Border = 0;
            detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
            detailHead.BackgroundColor = navblue;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("VAT on Sales and All Other Outputs", new Font(Font.FontFamily.UNDEFINED, 8, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailHead.Colspan = 4;
            detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
            detailHead.BackgroundColor = lightnavblue;
            detailsTable.AddCell(detailHead);


            PdfPCell detailHeadzero = new PdfPCell(new Phrase("0.00", new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
            detailHeadzero.HorizontalAlignment = Element.ALIGN_RIGHT;
            detailHeadzero.Border = 0;

            detailHead = new PdfPCell(new Phrase("Standard rated supplies in Abu Dhabi", new Font(Font.FontFamily.UNDEFINED, 7, Font.ITALIC)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);

            try
            {
                detailHead = new PdfPCell(new Phrase(dtemirate.Rows[0][0].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(detailHeadzero);
            }

            try
            {
                detailHead = new PdfPCell(new Phrase(dtemirate.Rows[0][1].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(detailHeadzero);
            }
            detailsTable.AddCell(detailHeadzero);

            detailHead = new PdfPCell(new Phrase("Standard rated supplies in Dubai", new Font(Font.FontFamily.UNDEFINED, 7, Font.ITALIC)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);
            try
            {
                detailHead = new PdfPCell(new Phrase(dtemirate.Rows[1][0].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(detailHeadzero);
            }

            try
            {
                detailHead = new PdfPCell(new Phrase(dtemirate.Rows[1][1].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(detailHeadzero);
            }
            //try
            //{
            //    detailHead = new PdfPCell(new Phrase(dtsalesSum.Rows[0]["Taxable"].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
            //    detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    detailHead.Border = 0;
            //    detailsTable.AddCell(detailHead);
            //}
            //catch (Exception ee)
            //{
            //    detailsTable.AddCell(emptyDetail);
            //}

            //try
            //{
            //    detailHead = new PdfPCell(new Phrase(dtsalesSum.Rows[0]["TaxAmount"].ToString(), new Font(Font.FontFamily.UNDEFINED,7, Font.NORMAL)));
            //    detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    detailHead.Border = 0;
            //    detailsTable.AddCell(detailHead);
            //}
            //catch (Exception ee)
            //{
            //    detailsTable.AddCell(emptyDetail);
            //}
            detailsTable.AddCell(detailHeadzero);

            detailHead = new PdfPCell(new Phrase("Standard rated supplies in Sharjah", new Font(Font.FontFamily.UNDEFINED, 7, Font.ITALIC)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);

            try
            {
                detailHead = new PdfPCell(new Phrase(dtemirate.Rows[2][0].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(detailHeadzero);
            }

            try
            {
                detailHead = new PdfPCell(new Phrase(dtemirate.Rows[2][1].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(detailHeadzero);
            }
            detailsTable.AddCell(detailHeadzero);

            detailHead = new PdfPCell(new Phrase("Standard rated supplies in Ajman", new Font(Font.FontFamily.UNDEFINED, 7, Font.ITALIC)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);

            try
            {
                detailHead = new PdfPCell(new Phrase(dtemirate.Rows[3][0].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(detailHeadzero);
            }

            try
            {
                detailHead = new PdfPCell(new Phrase(dtemirate.Rows[3][1].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(detailHeadzero);
            }
            detailsTable.AddCell(detailHeadzero);

            detailHead = new PdfPCell(new Phrase("Standard rated supplies in Umm Al Quwain", new Font(Font.FontFamily.UNDEFINED,7, Font.ITALIC)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);

            try
            {
                detailHead = new PdfPCell(new Phrase(dtemirate.Rows[4][0].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(detailHeadzero);
            }

            try
            {
                detailHead = new PdfPCell(new Phrase(dtemirate.Rows[4][1].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(detailHeadzero);
            }
            detailsTable.AddCell(detailHeadzero);

            detailHead = new PdfPCell(new Phrase("Standard rated supplies in Ras Al Khaimah", new Font(Font.FontFamily.UNDEFINED, 7, Font.ITALIC)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);

            try
            {
                detailHead = new PdfPCell(new Phrase(dtemirate.Rows[5][0].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(detailHeadzero);
            }

            try
            {
                detailHead = new PdfPCell(new Phrase(dtemirate.Rows[5][1].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(detailHeadzero);
            }
            detailsTable.AddCell(detailHeadzero);

            detailHead = new PdfPCell(new Phrase("Standard rated supplies in Fujairah", new Font(Font.FontFamily.UNDEFINED, 7, Font.ITALIC)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);

            try
            {
                detailHead = new PdfPCell(new Phrase(dtemirate.Rows[6][0].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(detailHeadzero);
            }

            try
            {
                detailHead = new PdfPCell(new Phrase(dtemirate.Rows[6][1].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(detailHeadzero);
            }
            detailsTable.AddCell(detailHeadzero);

            detailHead = new PdfPCell(new Phrase("Tax Refunds provided to Tourists under the Tax Refunds for Tourists Scheme", new Font(Font.FontFamily.UNDEFINED, 7, Font.ITALIC)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);

            detailsTable.AddCell(detailHeadzero);
            detailsTable.AddCell(detailHeadzero);
            detailsTable.AddCell(detailHeadzero);

            detailHead = new PdfPCell(new Phrase("Supplies subject to the reverse charge provisions", new Font(Font.FontFamily.UNDEFINED, 7, Font.ITALIC)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);

            detailsTable.AddCell(detailHeadzero);
            detailsTable.AddCell(detailHeadzero);
            detailsTable.AddCell(detailHeadzero);

            detailHead = new PdfPCell(new Phrase("Zero rated supplies", new Font(Font.FontFamily.UNDEFINED, 7, Font.ITALIC)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);

            detailsTable.AddCell(detailHeadzero);
            detailsTable.AddCell(detailHeadzero);
            detailsTable.AddCell(detailHeadzero);

            detailHead = new PdfPCell(new Phrase("Exempt supplies", new Font(Font.FontFamily.UNDEFINED, 7, Font.ITALIC)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);

            detailsTable.AddCell(detailHeadzero);
            detailsTable.AddCell(detailHeadzero);
            detailsTable.AddCell(detailHeadzero);

            detailHead = new PdfPCell(new Phrase("Goods imported into the UAE", new Font(Font.FontFamily.UNDEFINED, 7, Font.ITALIC)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);

            detailsTable.AddCell(detailHeadzero);
            detailsTable.AddCell(detailHeadzero);
            detailsTable.AddCell(detailHeadzero);

            detailHead = new PdfPCell(new Phrase("Adjustments to goods imported into the UAE", new Font(Font.FontFamily.UNDEFINED, 7, Font.ITALIC)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);

            detailsTable.AddCell(detailHeadzero);
            detailsTable.AddCell(detailHeadzero);
            detailsTable.AddCell(detailHeadzero);

            detailHead = new PdfPCell(new Phrase("Totals", new Font(Font.FontFamily.UNDEFINED,7, Font.ITALIC)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailHead.BackgroundColor = lightrred;
            detailsTable.AddCell(detailHead);

            try
            {
                detailHead = new PdfPCell(new Phrase(dtsalesSum.Rows[0]["Taxable"].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailHead.BackgroundColor = lightrred;
                detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }

            try
            {
                detailHead = new PdfPCell(new Phrase(dtsalesSum.Rows[0]["TaxAmount"].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
                detailHead.Border = 0;
                detailHead.BackgroundColor = lightrred;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }
            detailHead = new PdfPCell(new Phrase("-", new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
            detailHead.Border = 0;
            detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
            detailHead.BackgroundColor = lightrred;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("VAT on Expenses and All Other Inputs", new Font(Font.FontFamily.UNDEFINED, 8, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailHead.Colspan = 4;
            detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
            detailHead.BackgroundColor = lightnavblue;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("Standard rated expenses", new Font(Font.FontFamily.UNDEFINED, 7, Font.ITALIC)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);

            try
            {
                detailHead = new PdfPCell(new Phrase(dtPESum.Rows[0]["Taxable"].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }

            try
            {
                detailHead = new PdfPCell(new Phrase(dtPESum.Rows[0]["TaxAmount"].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }
            detailsTable.AddCell(detailHeadzero);

            detailHead = new PdfPCell(new Phrase("Supplies subject to the reverse charge provisions", new Font(Font.FontFamily.UNDEFINED, 7, Font.ITALIC)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);

            detailsTable.AddCell(detailHeadzero);
            detailsTable.AddCell(detailHeadzero);
            detailsTable.AddCell(detailHeadzero);

            detailHead = new PdfPCell(new Phrase("Totals", new Font(Font.FontFamily.UNDEFINED, 7, Font.ITALIC)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailHead.BackgroundColor = lightrred;
            detailsTable.AddCell(detailHead);

            try
            {
                detailHead = new PdfPCell(new Phrase(dtPESum.Rows[0]["Taxable"].ToString(), new Font(Font.FontFamily.UNDEFINED,7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailHead.BackgroundColor = lightrred;
                detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }

            try
            {
                detailHead = new PdfPCell(new Phrase(dtPESum.Rows[0]["TaxAmount"].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
                detailHead.Border = 0;
                detailHead.BackgroundColor = lightrred;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }
            detailHead = new PdfPCell(new Phrase("-", new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
            detailHead.Border = 0;
            detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
            detailHead.BackgroundColor = lightrred;
            detailsTable.AddCell(detailHead);


            detailHead = new PdfPCell(new Phrase("Net VAT Due", new Font(Font.FontFamily.UNDEFINED, 8, Font.BOLD)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailHead.Colspan = 4;
            detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
            detailHead.BackgroundColor = lightnavblue;
            detailsTable.AddCell(detailHead);

            detailHead = new PdfPCell(new Phrase("Total value of due tax for the period", new Font(Font.FontFamily.UNDEFINED, 7, Font.ITALIC)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);

            try
            {
                detailHead = new PdfPCell(new Phrase(dtsalesSum.Rows[0]["TaxAmount"].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Colspan = 3;
                detailHead.Border = 0;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }

            detailHead = new PdfPCell(new Phrase("Total value of recoverable tax for the period", new Font(Font.FontFamily.UNDEFINED, 7, Font.ITALIC)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailsTable.AddCell(detailHead);

            try
            {
                detailHead = new PdfPCell(new Phrase(dtPESum.Rows[0]["TaxAmount"].ToString(), new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Colspan = 3;
                detailHead.Border = 0;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailsTable.AddCell(emptyDetail);
            }

            detailHead = new PdfPCell(new Phrase("Payable tax for the period", new Font(Font.FontFamily.UNDEFINED, 7, Font.ITALIC)));
            detailHead.HorizontalAlignment = Element.ALIGN_LEFT;
            detailHead.Border = 0;
            detailHead.BackgroundColor = lightrred;
            detailsTable.AddCell(detailHead);

            try
            {
                detailHead = new PdfPCell(new Phrase("(" + dtPTSum.Rows[0]["PayableTaxSum"].ToString() + ")", new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailHead.Colspan = 3;
                detailHead.BackgroundColor = lightrred;
                detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
                detailsTable.AddCell(detailHead);
            }
            catch (Exception ee)
            {
                detailHead = new PdfPCell(new Phrase("", new Font(Font.FontFamily.UNDEFINED, 7, Font.NORMAL)));
                detailHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                detailHead.Border = 0;
                detailHead.Colspan = 3;
                detailHead.BackgroundColor = lightrred;
                detailHead.VerticalAlignment = Element.ALIGN_MIDDLE;
                detailsTable.AddCell(detailHead);
            }

            document.Add(detailsTable);

            #endregion

            document.Close();
            HttpContext.Current.Response.Write(document);
            HttpContext.Current.Response.End();
        }
    }
}