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
    public partial class CustomerSOAPdfFormat5: System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            int CustomerId = Convert.ToInt32(Request.QueryString["Cus"]);
            int PaymentStatus = Convert.ToInt32(Request.QueryString["PaymentStatus"]);
            int CompletionStatus = Convert.ToInt32(Request.QueryString["CompletionStatus"]);

            DateTime FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            DateTime ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            DataSet ds = obj_report.CustomerSOAPrintFormat5(FromDate, ToDate, CustomerId, PaymentStatus, CompletionStatus);
            DataTable dtCustomer = ds.Tables[0];
            DataTable dtDetails = ds.Tables[1];
            DataTable dtSum = ds.Tables[2];
            DataTable dtgen = ds.Tables[3];

            Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=" + dtCustomer.Rows[0]["Name"].ToString().Replace(",", "") + ".pdf");
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
            document.Open();
            if (dtgen.Rows[0]["PrintHeader"].ToString() != "")
            {

                string imageURL = Server.MapPath("../UploadedImage/" + dtgen.Rows[0]["PrintHeader"].ToString());
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
            float[] headTableWidths = new float[] { 100f };
            headTable.SetWidths(headTableWidths);
            headTable.WidthPercentage = 95f;

            PdfPCell HT00 = new PdfPCell(new Phrase("Customer SOA Report ", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            HT00.HorizontalAlignment = Element.ALIGN_CENTER;
            HT00.Border = 0;
            headTable.AddCell(HT00);

            HT00 = new PdfPCell(new Phrase(dtCustomer.Rows[0]["FromToDate"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            HT00.HorizontalAlignment = Element.ALIGN_CENTER;
            HT00.Border = 0;
            headTable.AddCell(HT00);
            /*End of Row*/
            document.Add(headTable);


            PdfPTable subHeadTable = new PdfPTable(2);
            subHeadTable.DefaultCell.Padding = 4;
            float[] subHeadTableWidths = new float[] { 60f,40f };
            subHeadTable.SetWidths(subHeadTableWidths);
            subHeadTable.WidthPercentage = 95f;

            PdfPCell sub00 = new PdfPCell(new Phrase("Customer :    " + dtCustomer.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub00.Border = 0;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            subHeadTable.AddCell(sub00);

             sub00 = new PdfPCell(new Phrase("Date : " + DateTime.Now, new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub00.Border = 0;
            sub00.HorizontalAlignment = Element.ALIGN_RIGHT;
            subHeadTable.AddCell(sub00);
           
            /*End of Row*/

            sub00 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub00.Border = 0;
            sub00.HorizontalAlignment = Element.ALIGN_RIGHT;
            subHeadTable.AddCell(sub00);
            /*End of Row*/

            sub00 = new PdfPCell(new Phrase("TRN : " + dtgen.Rows[0]["TRN"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub00.Border = 0;
            sub00.HorizontalAlignment = Element.ALIGN_RIGHT;
            subHeadTable.AddCell(sub00);

            document.Add(subHeadTable);


            #endregion

            #region data

            if (dtDetails.Rows.Count > 0)
            {
                PdfPTable detailsTable = new PdfPTable(8);
                detailsTable.DefaultCell.Padding = 4;
                float[] detailsTableWidthsdet = new float[] { 8f, 3f, 12f, 8f, 7f, 7f, 7f, 7f };
                detailsTable.SetWidths(detailsTableWidthsdet);
                detailsTable.SpacingBefore = 10f;
                detailsTable.WidthPercentage = 95f;

                decimal Bal = 0, BalDisp = 0;

                PdfPCell detailHead = new PdfPCell(new Phrase("Date", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailHead.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                detailsTable.AddCell(detailHead);
                detailHead = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailHead.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                detailsTable.AddCell(detailHead);
                detailHead = new PdfPCell(new Phrase("Particulars", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailHead.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Vch Type", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailHead.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Vch No", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailHead.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Debit", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailHead.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Credit", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailHead.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Balance", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailHead.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                detailsTable.AddCell(detailHead);

                PdfPCell emptyDetail = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                emptyDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                emptyDetail.Border = 0;

                PdfPCell detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                string Dates = "";
                foreach (DataRow rows in dtDetails.Rows)
                {
                    try
                    {
                        if (Dates != rows["Dated"].ToString())
                        {
                            Dates = rows["Dated"].ToString();
                            detailCell = new PdfPCell(new Phrase(rows["Dated"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        }
                        else
                            detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));

                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        detailCell.Border = 0;
                        detailsTable.AddCell(detailCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }
                    try
                    {
                        detailCell = new PdfPCell(new Phrase(rows["SubType"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        detailCell.Border = 0;
                        detailsTable.AddCell(detailCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }
                    try
                    {
                        detailCell = new PdfPCell(new Phrase(rows["PayType"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        detailCell.Border = 0;
                        detailsTable.AddCell(detailCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }
                    try
                    {
                        detailCell = new PdfPCell(new Phrase(rows["Vchtype"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        detailCell.Border = 0;
                        detailsTable.AddCell(detailCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }

                    try
                    {
                        detailCell = new PdfPCell(new Phrase(rows["Code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        detailCell.Border = 0;
                        detailsTable.AddCell(detailCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }
                    try
                    {
                        detailCell = new PdfPCell(new Phrase(rows["Debit"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
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
                        detailCell = new PdfPCell(new Phrase(rows["Credit"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
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
                        Bal = Bal + Convert.ToDecimal(rows["Balance"]);
                        BalDisp = (Bal < 0) ? Bal * -1 : Bal;
                        string balDisp = (Bal < 0) ? (BalDisp.ToString() + " Dr") : (Bal.ToString() + " Cr");
                        detailCell = new PdfPCell(new Phrase(balDisp, new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        detailCell.Border = 0;
                        detailsTable.AddCell(detailCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }
                    if (rows["Particulars"].ToString() != "")
                    {
                        detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        detailCell.Border = 0;
                        detailCell.Colspan = 2;
                        detailsTable.AddCell(detailCell);

                        detailCell = new PdfPCell(new Phrase(rows["Particulars"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        detailCell.Border = 0;
                        detailCell.Colspan = 3;
                        detailsTable.AddCell(detailCell);

                        detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        detailCell.Border = 0;
                        detailCell.Colspan =3;
                        detailsTable.AddCell(detailCell);
                    }
                }
                detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                detailCell.Colspan = 8;
                detailCell.Border = 0;
                detailCell.MinimumHeight = 5f;
                detailsTable.AddCell(detailCell);

                detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.Border = 0;
                detailsTable.AddCell(detailCell);
                detailCell = new PdfPCell(new Phrase("By", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                detailCell.Border = 0;
                detailsTable.AddCell(detailCell);

                detailCell = new PdfPCell(new Phrase("Closing Balance", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                detailCell.Colspan = 3;
                detailCell.Border = 0;
                detailsTable.AddCell(detailCell);

                detailCell = new PdfPCell(new Phrase(dtSum.Rows[0]["Debit"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.Border = PdfPCell.TOP_BORDER;
                detailsTable.AddCell(detailCell);

                detailCell = new PdfPCell(new Phrase(dtSum.Rows[0]["Credit"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.Border = PdfPCell.TOP_BORDER;
                detailsTable.AddCell(detailCell);

                detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.Border = 0;
                detailsTable.AddCell(detailCell);

                //nextrow
                detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.Border = 0;
                detailCell.Colspan = 5;
                detailsTable.AddCell(detailCell);

                if (Bal < 0)
                    detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                else
                    detailCell = new PdfPCell(new Phrase(Bal.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.Border = PdfPCell.BOTTOM_BORDER;
                detailsTable.AddCell(detailCell);

                if (Bal < 0)
                    detailCell = new PdfPCell(new Phrase(BalDisp.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                else
                    detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.Border = PdfPCell.BOTTOM_BORDER;
                detailsTable.AddCell(detailCell);

                detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.Border =0;
                detailsTable.AddCell(detailCell);

                //new row
               
                detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                detailCell.Colspan = 5;
                detailCell.Border = 0;
                detailsTable.AddCell(detailCell);

                detailCell = new PdfPCell(new Phrase(dtSum.Rows[0]["DebitSum"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.Border = PdfPCell.BOTTOM_BORDER;
                detailsTable.AddCell(detailCell);

                detailCell = new PdfPCell(new Phrase(dtSum.Rows[0]["CreditSum"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.Border = PdfPCell.BOTTOM_BORDER;
                detailsTable.AddCell(detailCell);

                detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.Border = 0;
                detailsTable.AddCell(detailCell);

                //new row

                detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                detailCell.Colspan = 5;
                detailCell.Border = 0;
                detailsTable.AddCell(detailCell);

                detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.Border = PdfPCell.BOTTOM_BORDER;
                detailsTable.AddCell(detailCell);

                detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.Border = PdfPCell.BOTTOM_BORDER;
                detailsTable.AddCell(detailCell);

                detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.Border = 0;
                detailsTable.AddCell(detailCell);

                document.Add(detailsTable);
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