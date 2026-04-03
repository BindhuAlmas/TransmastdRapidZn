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
    public partial class CustomerSOAPdfFormat10 : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            int CustomerId = Convert.ToInt32(Request.QueryString["Cus"]);
            int PaymentStatus = Convert.ToInt32(Request.QueryString["PaymentStatus"]);
            int CompletionStatus = Convert.ToInt32(Request.QueryString["CompletionStatus"]);

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
            DataSet ds = obj_report.CustomerSOAPrintFormat2(FromDate, ToDate, CustomerId, PaymentStatus, CompletionStatus);
            DataTable dtCustomer = ds.Tables[0];
            DataTable dtDetails = ds.Tables[1];
            DataTable dtSum = ds.Tables[2];
            DataTable dtgen = ds.Tables[3];

            //payment detais
            DataSet ds1 = obj_report.CustomerSOAPrintFormat8(FromDate, ToDate, CustomerId, PaymentStatus, CompletionStatus);
            DataTable dtreceipt = ds1.Tables[4];
            DataTable dtReceiptTot = ds1.Tables[5];

            Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=CustomerSOAPrintFormat2.pdf");
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
            float[] headTableWidths = new float[] { 120f };
            headTable.SetWidths(headTableWidths);
            headTable.WidthPercentage = 95f;

            PdfPCell HT00 = new PdfPCell(new Phrase("Statement of Account ", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            HT00.HorizontalAlignment = Element.ALIGN_CENTER;
            HT00.Border = 0;
            headTable.AddCell(HT00);
            /*End of Row*/
            document.Add(headTable);



            PdfPTable subHeadTable = new PdfPTable(3);
            subHeadTable.DefaultCell.Padding = 4;
            float[] subHeadTableWidths = new float[] { 40f, 20f, 50f };
            subHeadTable.SetWidths(subHeadTableWidths);
            subHeadTable.WidthPercentage = 95f;

            PdfPCell sub00 = new PdfPCell(new Phrase("Customer : " + dtCustomer.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub00.Border = 0;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            subHeadTable.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub00.Border = 0;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            subHeadTable.AddCell(sub00);

            //sub00 = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub00 = new PdfPCell(new Phrase("Date : " + DateTime.Now, new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub00.Border = 0;
            sub00.HorizontalAlignment = Element.ALIGN_RIGHT;
            subHeadTable.AddCell(sub00);
            /*End of Row*/

            sub00 = new PdfPCell(new Phrase("TRN :    " + dtCustomer.Rows[0]["TRN"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub00.Border = 0;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            subHeadTable.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub00.Border = 0;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            subHeadTable.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase("Period : " + dtCustomer.Rows[0]["Period"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub00.Border = 0;
            sub00.HorizontalAlignment = Element.ALIGN_RIGHT;
            subHeadTable.AddCell(sub00);
            /*End of Row*/

            sub00 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub00.Border = 0;
            sub00.Colspan = 2;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            subHeadTable.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase("TRN : " + dtgen.Rows[0]["TRN"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub00.Border = 0;
            sub00.HorizontalAlignment = Element.ALIGN_RIGHT;
            subHeadTable.AddCell(sub00);
            /*End of Row*/

            document.Add(subHeadTable);


            #endregion

            #region data

            if (dtDetails.Rows.Count > 0)
            {
                PdfPTable detailsTable = new PdfPTable(7);
                detailsTable.DefaultCell.Padding = 4;
                float[] detailsTableWidthsdet = new float[] { 5f, 15f, 20f, 30f, 15f, 15f, 15f };
                detailsTable.SetWidths(detailsTableWidthsdet);
                detailsTable.SpacingBefore = 10f;
                detailsTable.WidthPercentage = 95f;

                PdfPCell detailHead = new PdfPCell(new Phrase("Sl", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Invoice Date", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Invoice No", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Applicant Name", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Debit", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Credit", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Balance", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                PdfPCell emptyDetail = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                emptyDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                emptyDetail.Border = 0;

                PdfPCell detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                foreach (DataRow rows in dtDetails.Rows)
                {
                    try
                    {
                        detailCell = new PdfPCell(new Phrase(rows["RowNum"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        detailCell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER;
                        detailsTable.AddCell(detailCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }
                    try
                    {
                        detailCell = new PdfPCell(new Phrase(rows["Date"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        detailCell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER;
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
                        detailCell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER;
                        detailsTable.AddCell(detailCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }

                    try
                    {
                        detailCell = new PdfPCell(new Phrase(rows["Particulars"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        detailCell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER;
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
                        detailCell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER;
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
                        detailCell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER;
                        detailsTable.AddCell(detailCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }
                    try
                    {
                        detailCell = new PdfPCell(new Phrase(rows["Balance"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        detailCell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                        detailsTable.AddCell(detailCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }
                }
                detailCell = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.Colspan = 4;
                detailCell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER;
                detailsTable.AddCell(detailCell);

                detailCell = new PdfPCell(new Phrase(dtSum.Rows[0]["TotalDebit"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER;
                detailsTable.AddCell(detailCell);

                detailCell = new PdfPCell(new Phrase(dtSum.Rows[0]["TotalCredit"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER;
                detailsTable.AddCell(detailCell);

                detailCell = new PdfPCell(new Phrase(dtSum.Rows[0]["TotalBalance"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                detailsTable.AddCell(detailCell);

                document.Add(detailsTable);
            }

            if (dtreceipt.Rows.Count > 0)
            {
                PdfPTable Paymnt = new PdfPTable(5);
                Paymnt.DefaultCell.Padding = 4;
                Paymnt.SpacingBefore = 20;
                Paymnt.WidthPercentage = 95;
                float[] widthsp = new float[] { 5f, 10f, 20f, 20f, 10f };
                Paymnt.SetWidths(widthsp);

                PdfPCell irechd = new PdfPCell(new Phrase("Payments", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                irechd.HorizontalAlignment = Element.ALIGN_LEFT;
                irechd.Border = 0;
                irechd.Colspan = 5;
                irechd.MinimumHeight = 20;
                Paymnt.AddCell(irechd);

                PdfPCell Serial_Norec = new PdfPCell(new Phrase("Sl No", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Serial_Norec.HorizontalAlignment = Element.ALIGN_CENTER;
                Paymnt.AddCell(Serial_Norec);
                PdfPCell Sertre = new PdfPCell(new Phrase("Date", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sertre.HorizontalAlignment = Element.ALIGN_CENTER;
                Paymnt.AddCell(Sertre);
                PdfPCell Serss = new PdfPCell(new Phrase("Receipt No", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Serss.HorizontalAlignment = Element.ALIGN_CENTER;
                Paymnt.AddCell(Serss);
                PdfPCell Sersrem = new PdfPCell(new Phrase("Remarks", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sersrem.HorizontalAlignment = Element.ALIGN_CENTER;
                Paymnt.AddCell(Sersrem);
                PdfPCell Quassno = new PdfPCell(new Phrase("Amount", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Quassno.HorizontalAlignment = Element.ALIGN_CENTER;
                Paymnt.AddCell(Quassno);

                int i = 0;
                foreach (DataRow rows in dtreceipt.Rows)
                {
                    try
                    {
                        PdfPCell serial_no = new PdfPCell(new Phrase((++i).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        serial_no.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        Paymnt.AddCell(serial_no);
                    }
                    catch (Exception ee)
                    {
                        Paymnt.AddCell("");
                    }
                    try
                    {
                        PdfPCell typee = new PdfPCell(new Phrase(rows["Dated"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        typee.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        Paymnt.AddCell(typee);
                    }
                    catch (Exception eee)
                    {
                        Paymnt.AddCell("");
                    }
                    try
                    {
                        PdfPCell typee = new PdfPCell(new Phrase(rows["Code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        typee.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        Paymnt.AddCell(typee);
                    }
                    catch (Exception eee)
                    {
                        Paymnt.AddCell("");
                    }

                    try
                    {
                        PdfPCell typee = new PdfPCell(new Phrase(rows["Remarks"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        typee.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        Paymnt.AddCell(typee);
                    }
                    catch (Exception eee)
                    {
                        Paymnt.AddCell("");
                    }


                    try
                    {
                        PdfPCell Stypee = new PdfPCell(new Phrase(rows["ReceivedAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        Stypee.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        Paymnt.AddCell(Stypee);
                    }
                    catch (Exception eee)
                    {
                        Paymnt.AddCell("");
                    }
                }

                PdfPCell tots = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                tots.HorizontalAlignment = Element.ALIGN_RIGHT;
                tots.Colspan = 4;
                tots.Border = 0;
                Paymnt.AddCell(tots);

                decimal totrecsum = dtReceiptTot.Rows[0]["ReceivedAmount"].ToString() == "" ? 0 : (Convert.ToDecimal(dtReceiptTot.Rows[0]["ReceivedAmount"]));
                
                PdfPCell totiddnv = new PdfPCell(new Phrase(totrecsum.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                totiddnv.HorizontalAlignment = Element.ALIGN_RIGHT;
                totiddnv.Border = 0;
                Paymnt.AddCell(totiddnv);

                document.Add(Paymnt);
            }

            //else
            //{
            //    PdfPTable bill_details4 = new PdfPTable(1);
            //    bill_details4.DefaultCell.Padding = 4;
            //    bill_details4.SpacingBefore = 10;

            //    PdfPCell remarks = new PdfPCell(new Phrase("No Record", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
            //    remarks.Border = 0;
            //    remarks.Colspan = 1;
            //    remarks.HorizontalAlignment = Element.ALIGN_LEFT;
            //    bill_details4.AddCell(remarks);

            //    document.Add(bill_details4);
            //}

            #endregion

            document.Close();
            HttpContext.Current.Response.Write(document);
            HttpContext.Current.Response.End();
        }
    }
}