using AmarCentre.BAL;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AmarCentre.Reports
{
    public partial class CustomerBalancePdf : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime? FromDate = null, ToDate = null;
            try
            {
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }
            catch (Exception cc)
            {
                FromDate = null;
            }
            try
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }
            catch (Exception cc)
            {
                ToDate = null;
            }

            DataSet ds = obj_report.CustomerBalancePdfExcel(FromDate, ToDate);
            DataTable dt = ds.Tables[0];
            DataTable dt_sum = ds.Tables[1];
            DataTable dt_date = ds.Tables[2];

            Document document = new Document(PageSize.A4, 10f, 10f, 10f, 10f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=CustomerBalancePdf.pdf");
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
            float[] headTableWidths = new float[] { 120f };
            headTable.SetWidths(headTableWidths);
            headTable.WidthPercentage = 95f;

            PdfPCell HT00 = new PdfPCell(new Phrase("Customer Balance", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            HT00.HorizontalAlignment = Element.ALIGN_CENTER;
            HT00.Border = 0;
            headTable.AddCell(HT00);

            HT00 = new PdfPCell(new Phrase("From : " + dt_date.Rows[0]["FromDate"].ToString() + " To : " + dt_date.Rows[0]["ToDate"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
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

            if (dt.Rows.Count > 0)
            {
                PdfPTable detailsTable = new PdfPTable(6);
                detailsTable.DefaultCell.Padding = 4;
                float[] detailsTableWidthsdet = new float[] { 3f, 15f, 10f, 10f, 10f, 10f };
                detailsTable.SetWidths(detailsTableWidthsdet);
                detailsTable.SpacingBefore = 10f;
                detailsTable.WidthPercentage = 95f;

                PdfPCell detailHead = new PdfPCell(new Phrase("Sl No", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase(" Customer", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Received Amount", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Work Completed Amount", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Customer Balance", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Customer Advance", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                
          
                ////////////

                PdfPCell emptyDetail = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                emptyDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                emptyDetail.Border = 0;

                PdfPCell detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                int i = 0;
                foreach (DataRow rows in dt.Rows)
                {
                    try
                    {
                        PdfPCell sn = new PdfPCell(new Phrase((++i).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        sn.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        detailsTable.AddCell(sn);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell("N/A");
                    }
                    try
                    {
                        detailCell = new PdfPCell(new Phrase(rows["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        detailsTable.AddCell(detailCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }
                    try
                    {
                        detailCell = new PdfPCell(new Phrase(rows["ReceivedAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        detailsTable.AddCell(detailCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }
                    try
                    {
                        detailCell = new PdfPCell(new Phrase(rows["WorkCompleted"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        detailsTable.AddCell(detailCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }
                    try
                    {
                        detailCell = new PdfPCell(new Phrase(rows["Receivable"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        detailsTable.AddCell(detailCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }
                    try
                    {
                        detailCell = new PdfPCell(new Phrase(rows["Payable"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        detailsTable.AddCell(detailCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }
                   
                }
                PdfPCell DTw = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                DTw.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                DTw.Colspan = 2;
                DTw.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                DTw.MinimumHeight = 20f;
                detailsTable.AddCell(DTw);

                PdfPCell DTwt = new PdfPCell(new Phrase(dt_sum.Rows[0]["TotalReceivedAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                DTwt.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                DTwt.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                detailsTable.AddCell(DTwt);
                DTwt = new PdfPCell(new Phrase(dt_sum.Rows[0]["TotalWorkCompleted"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                DTwt.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                DTwt.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                detailsTable.AddCell(DTwt);
                DTwt = new PdfPCell(new Phrase(dt_sum.Rows[0]["TotalReceivable"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                DTwt.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                DTwt.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                detailsTable.AddCell(DTwt);
                DTwt = new PdfPCell(new Phrase(dt_sum.Rows[0]["TotalPayable"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                DTwt.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                DTwt.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                detailsTable.AddCell(DTwt);

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