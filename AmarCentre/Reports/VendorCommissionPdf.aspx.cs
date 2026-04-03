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
    public partial class VendorCommissionPdf : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            int VendorId = Convert.ToInt32(Request.QueryString["VendorId"]);

            DateTime FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            DateTime ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            DataSet ds = obj_report.VendorCommissionpdf(FromDate, ToDate, VendorId);
            DataTable dtEmp = ds.Tables[0];
            DataTable dt = ds.Tables[1];
            DataTable dtpayments = ds.Tables[2];
            DataTable dtpaysum = ds.Tables[3];

            Document document = new Document(PageSize.A4, 20f, 20f, 10f, 10f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=Vendorcomission.pdf");
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

            PdfPCell HT00 = new PdfPCell(new Phrase("Vendor Commission", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            HT00.HorizontalAlignment = Element.ALIGN_CENTER;
            HT00.Border = 0;
            headTable.AddCell(HT00);

            document.Add(headTable);

            PdfPTable tbl_basic = new PdfPTable(1);
            tbl_basic.DefaultCell.Padding = 4;
            float[] widths1 = new float[] { 100f };
            tbl_basic.SetWidths(widths1);
            tbl_basic.HorizontalAlignment = Element.ALIGN_LEFT;
            tbl_basic.WidthPercentage = 100f;
            tbl_basic.SpacingBefore = 10f;
            tbl_basic.SpacingAfter = 10f;

            PdfPCell cust21 = new PdfPCell(new Phrase("Vendor : "+ dtEmp.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            cust21.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cust21.Border = 0;
            tbl_basic.AddCell(cust21);

            PdfPCell OB = new PdfPCell(new Phrase("Opening Balance : " + dtEmp.Rows[0]["OpeningBalance"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            OB.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            OB.Border = 0;
            OB.Colspan = 2;
            tbl_basic.AddCell(OB);

            document.Add(tbl_basic);

            #endregion

            #region data

            if (dt.Rows.Count > 0)
            {
                PdfPTable detailsTable = new PdfPTable(7);
                detailsTable.DefaultCell.Padding = 4;
                float[] detailsTableWidthsdet = new float[] { 5f, 10f, 10f, 25f, 15f,20f,10f};
                detailsTable.SetWidths(detailsTableWidthsdet);
                detailsTable.SpacingBefore = 10f;
                detailsTable.WidthPercentage = 100f;

                PdfPCell detailHead = new PdfPCell(new Phrase("Sl", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Date", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Invoice No", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Customer", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("ApplicantName", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Service", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                detailHead = new PdfPCell(new Phrase("Receivable", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                detailsTable.AddCell(detailHead);

                

                PdfPCell emptyDetail = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                emptyDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                emptyDetail.Border = 0;

                PdfPCell detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                int i = 0;
                foreach (DataRow rows in dt.Rows)
                {
                    try
                    {
                        detailCell = new PdfPCell(new Phrase((++i).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        detailsTable.AddCell(detailCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }
                    try
                    {
                        detailCell = new PdfPCell(new Phrase(rows["Date"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        detailsTable.AddCell(detailCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }
                    try
                    {
                        detailCell = new PdfPCell(new Phrase(rows["Code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        detailsTable.AddCell(detailCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }

                    try
                    {
                        detailCell = new PdfPCell(new Phrase(rows["Customer"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        detailsTable.AddCell(detailCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }
                    try
                    {
                        detailCell = new PdfPCell(new Phrase(rows["ApplicantName"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        detailsTable.AddCell(detailCell);
                    }
                    catch (Exception ee)
                    {
                        detailsTable.AddCell(emptyDetail);
                    }

                    try
                    {
                        detailCell = new PdfPCell(new Phrase(rows["Particular"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        detailCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
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
                    

                }
                detailCell = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.Colspan = 4;
                detailCell.Border = 0;
                detailsTable.AddCell(detailCell);

                detailCell = new PdfPCell(new Phrase(dtpaysum.Rows[0]["Receivable"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                detailCell.Border = 0;
                detailsTable.AddCell(detailCell);
               
                document.Add(detailsTable);
            }

            if (dtpayments.Rows.Count > 0)
            {
                PdfPTable Paymnt = new PdfPTable(4);
                Paymnt.DefaultCell.Padding = 4;
                Paymnt.SpacingBefore = 20;
                Paymnt.WidthPercentage = 100;
                float[] widthsp = new float[] { 5f, 10f, 10f, 10f };
                Paymnt.SetWidths(widthsp);

                PdfPCell irechd = new PdfPCell(new Phrase("Payments", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD)));
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
                PdfPCell Quassno = new PdfPCell(new Phrase("Amount", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Quassno.HorizontalAlignment = Element.ALIGN_CENTER;
                Paymnt.AddCell(Quassno);

                int i = 0;
                foreach (DataRow rows in dtpayments.Rows)
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
                        PdfPCell typee = new PdfPCell(new Phrase(rows["Date"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
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
                        PdfPCell Stypee = new PdfPCell(new Phrase(rows["Amount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        Stypee.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        Paymnt.AddCell(Stypee);
                    }
                    catch (Exception eee)
                    {
                        Paymnt.AddCell("");
                    }
                }

                PdfPCell tots = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tots.HorizontalAlignment = Element.ALIGN_RIGHT;
                tots.Colspan = 3;
                tots.Border = 0;
                Paymnt.AddCell(tots);

                PdfPCell totiddnv = new PdfPCell(new Phrase(dtpaysum.Rows[0]["PVSum"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                totiddnv.HorizontalAlignment = Element.ALIGN_RIGHT;
                totiddnv.Border = 0;
                Paymnt.AddCell(totiddnv);


                PdfPCell totsss = new PdfPCell(new Phrase(" Balance", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                totsss.HorizontalAlignment = Element.ALIGN_RIGHT;
                totsss.Colspan = 3;
                totsss.Border = 0;
                totsss.VerticalAlignment = Element.ALIGN_MIDDLE;
                totsss.MinimumHeight = 30;
                Paymnt.AddCell(totsss);

                PdfPCell totiddssnv = new PdfPCell(new Phrase(dtpaysum.Rows[0]["Balance"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                totiddssnv.HorizontalAlignment = Element.ALIGN_RIGHT;
                totiddssnv.Border = 0;
                totiddssnv.VerticalAlignment = Element.ALIGN_MIDDLE;
                Paymnt.AddCell(totiddssnv);

                document.Add(Paymnt);
            }

            #endregion

            document.Close();
            HttpContext.Current.Response.Write(document);
            HttpContext.Current.Response.End();
        }
    }
}