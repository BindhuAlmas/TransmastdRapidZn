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
using System.Drawing.Printing;


namespace AmarCentre.Reports
{
    public partial class CashReceiptFormat2 : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();
        Master_Bal obj_master = new Master_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"]);
            DataSet ds = obj_report.CashReceiptPrintF2(id);
            DataTable dtCustomer = ds.Tables[0];
            DataTable dtInvoice = ds.Tables[1];
            DataTable dtReceipt = ds.Tables[2];
            DataTable dtgen = ds.Tables[3];

            Document document = new Document(PageSize.A4, 20f, 20f, 10f, 10f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=REceiptPrint.pdf");
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

            PdfPTable table1 = new PdfPTable(1);
            table1.DefaultCell.Padding = 4;
            PdfPCell cell1 = new PdfPCell(new Phrase("Receipt", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            cell1.Border = 0;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            table1.AddCell(cell1);

            document.Add(table1);

            PdfPTable Subhead = new PdfPTable(6);
            Subhead.DefaultCell.Padding = 4;
            Subhead.SpacingBefore = 20f;
            float[] widths = new float[] { 15f,5f,45f,12f,5f,18f };
            Subhead.SetWidths(widths);
            Subhead.WidthPercentage = 100f;

            // empty cell

            PdfPCell Empty = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            Empty.Border = 0;
            Empty.HorizontalAlignment = Element.ALIGN_LEFT;

            //end

            PdfPCell sub1 = new PdfPCell(new Phrase("Customer ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub1.Border = 0;
            sub1.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub1);
            sub1 = new PdfPCell(new Phrase(" :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub1.Border = 0;
            sub1.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub1);

            PdfPCell sub12 = new PdfPCell(new Phrase(dtCustomer.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub12.Border = 0;
            sub12.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub12);

            PdfPCell sub13 = new PdfPCell(new Phrase("Receipt No ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub13.Border = 0;
            sub13.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub13);
            sub13 = new PdfPCell(new Phrase(" :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub13.Border = 0;
            sub13.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub13);

            PdfPCell sub14 = new PdfPCell(new Phrase(dtInvoice.Rows[0]["Reciptcode"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub14.Border = 0;
            sub14.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub14);

            PdfPCell sub21 = new PdfPCell(new Phrase("Contact ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub21.Border = 0;
            sub21.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub21);
            sub21 = new PdfPCell(new Phrase(" :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub21.Border = 0;
            sub21.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub21);

            PdfPCell sub122 = new PdfPCell(new Phrase(dtCustomer.Rows[0]["Mobile_num"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub122.Border = 0;
            sub122.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub122);

            PdfPCell sub132 = new PdfPCell(new Phrase("Invoice ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub132.Border = 0;
            sub132.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub132);
            sub132 = new PdfPCell(new Phrase(" :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub132.Border = 0;
            sub132.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub132);

            PdfPCell sub142 = new PdfPCell(new Phrase(dtInvoice.Rows[0]["Code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub142.Border = 0;
            sub142.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub142);

            PdfPCell sub21add = new PdfPCell(new Phrase("TRN ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub21add.Border = 0;
            sub21add.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub21add);
            sub21add = new PdfPCell(new Phrase(" : ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub21add.Border = 0;
            sub21add.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub21add);

            PdfPCell sub122add = new PdfPCell(new Phrase(dtCustomer.Rows[0]["TRN"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub122add.Border = 0;
            sub122add.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub122add);

            sub132 = new PdfPCell(new Phrase("Date ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub132.Border = 0;
            sub132.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub132);
            sub132 = new PdfPCell(new Phrase(" : ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub132.Border = 0;
            sub132.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub132);

            sub142 = new PdfPCell(new Phrase(dtInvoice.Rows[0]["QDate"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub142.Border = 0;
            sub142.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub142);

            sub21add = new PdfPCell(new Phrase("Address ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub21add.Border = 0;
            sub21add.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub21add);
            sub21add = new PdfPCell(new Phrase(" : ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub21add.Border = 0;
            sub21add.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub21add);

            sub122add = new PdfPCell(new Phrase(dtCustomer.Rows[0]["Address"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub122add.Border = 0;
            //sub122add.Colspan = 2;
            sub122add.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub122add);

            sub132 = new PdfPCell(new Phrase("TRN ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub132.Border = 0;
            sub132.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub132);
            sub132 = new PdfPCell(new Phrase(" : ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub132.Border = 0;
            sub132.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub132);

            sub142 = new PdfPCell(new Phrase(dtInvoice.Rows[0]["InvoiceTRN"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub142.Border = 0;
            sub142.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub142);

            if (dtInvoice.Rows[0]["subject"].ToString() != "")
            {
                sub21add = new PdfPCell(new Phrase("Subject ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                sub21add.Border = 0;
                sub21add.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub21add);
                sub21add = new PdfPCell(new Phrase(" : ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                sub21add.Border = 0;
                sub21add.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub21add);

                sub122add = new PdfPCell(new Phrase(dtInvoice.Rows[0]["subject"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                sub122add.Border = 0;
                sub122add.Colspan = 4;
                sub122add.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub122add);
            }

            document.Add(Subhead);

            #endregion

            #region data

            if (dtReceipt.Rows.Count > 0)
            {
                PdfPTable emp_details = new PdfPTable(7);
                //if (dtCustomer.Rows[0]["TaxPrint"].ToString() == "1")
                //    emp_details = new PdfPTable(9);

                emp_details.DefaultCell.Padding = 4;
                float[] widthsdet = new float[] { 8f, 25f, 20f, 12f, 11f, 14f, 14f };
                //if (dtCustomer.Rows[0]["TaxPrint"].ToString() == "1")
                //    widthsdet = new float[] { 8f, 25f, 25f, 12f, 8f, 9f, 12f, 14f, 12f };
                emp_details.SetWidths(widthsdet);

                emp_details.SpacingBefore = 20f;
                emp_details.WidthPercentage = 100f;

                PdfPCell SN = new PdfPCell(new Phrase("S.No", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                SN.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(SN);
                PdfPCell ty = new PdfPCell(new Phrase("Service", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                ty.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(ty);
                PdfPCell nam = new PdfPCell(new Phrase("Applicant", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                nam.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(nam);
                PdfPCell da = new PdfPCell(new Phrase("Unit Price", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                da.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(da);
                PdfPCell inc = new PdfPCell(new Phrase("Qty", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                inc.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(inc);
                //if (dtCustomer.Rows[0]["TaxPrint"].ToString() == "1")
                //{

                //    inc = new PdfPCell(new Phrase("Tax", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                //    inc.HorizontalAlignment = Element.ALIGN_CENTER;
                //    emp_details.AddCell(inc);
                //    da = new PdfPCell(new Phrase("Amt with Tax", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                //    da.HorizontalAlignment = Element.ALIGN_CENTER;
                //    emp_details.AddCell(da);
                //}
                PdfPCell exp = new PdfPCell(new Phrase("Discount", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                exp.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(exp);
                PdfPCell exptt = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                exptt.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(exptt);

                BaseFont bfTimes = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Font/arabtype.ttf"), BaseFont.IDENTITY_H, true);
                iTextSharp.text.Font arbfnt = new iTextSharp.text.Font(bfTimes, 17, Font.NORMAL);


                int i = 0;
                int qty = 0;
                foreach (DataRow rows in dtReceipt.Rows)
                {
                    try
                    {
                        PdfPCell sn = new PdfPCell(new Phrase((++i).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        sn.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        emp_details.AddCell(sn);
                    }
                    catch (Exception ee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPTable intble = new PdfPTable(1);
                        intble.WidthPercentage = 95f;

                        PdfPCell TP = new PdfPCell(new Phrase(rows["Name"].ToString() + "\n", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        TP.Border = 0;
                        intble.AddCell(TP);

                        //if (dtCustomer.Rows[0]["IsServArbc"].ToString() == "1")
                        //{
                        //    TP = new PdfPCell(new Phrase(rows["Name_arbic"].ToString(), arbfnt));
                        //    TP.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                        //    TP.Border = 0;
                        //    intble.AddCell(TP);
                        //}

                        emp_details.AddCell(intble);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }

                    try
                    {
                        PdfPCell REM = new PdfPCell(new Phrase(rows["Particulars"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        REM.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        emp_details.AddCell(REM);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell DT = new PdfPCell(new Phrase(rows["Singletotal"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        emp_details.AddCell(DT);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell DT = new PdfPCell(new Phrase(rows["Quantity"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        emp_details.AddCell(DT);

                        //qty = qty + Convert.ToInt32(rows["Quantity"]);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                   
                    try
                    {
                        PdfPCell DT = new PdfPCell(new Phrase(rows["Discount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        emp_details.AddCell(DT);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }

                    try
                    {
                        PdfPCell DT = new PdfPCell(new Phrase(rows["AfterDiscount_Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        emp_details.AddCell(DT);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }

                }

                PdfPCell DTw = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                DTw.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                DTw.Colspan = 5;
                DTw.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                DTw.MinimumHeight = 20f;
                emp_details.AddCell(DTw);

                //PdfPCell DTwt = new PdfPCell(new Phrase(qty.ToString("0.00"), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                //DTwt.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                //DTwt.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                //emp_details.AddCell(DTwt);

                PdfPCell DTwt = new PdfPCell(new Phrase(dtInvoice.Rows[0]["Total_Discount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                DTwt.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                DTwt.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                emp_details.AddCell(DTwt);
                DTwt = new PdfPCell(new Phrase(dtInvoice.Rows[0]["AfterDiscount_GrandTotal"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                DTwt.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                DTwt.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                emp_details.AddCell(DTwt);

                document.Add(emp_details);

                PdfPTable totalexp = new PdfPTable(4);
                totalexp.DefaultCell.Padding = 4;
                float[] widths1 = new float[] { 54f, 21f, 10f, 13f };
                totalexp.SetWidths(widths1);
                totalexp.SpacingBefore = 10;
                totalexp.WidthPercentage = 100f;

                totalexp.AddCell(Empty);

                PdfPCell tot = new PdfPCell(new Phrase("Total Amount :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot.Border = 0;
                tot.HorizontalAlignment = Element.ALIGN_LEFT;
                totalexp.AddCell(tot);

                PdfPCell tot1 = new PdfPCell(new Phrase("AED", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                tot1.Border = 0;
                tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot1);

                tot1 = new PdfPCell(new Phrase(dtInvoice.Rows[0]["Grand_Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot1.Border = 0;
                tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot1);

                totalexp.AddCell(Empty);

                PdfPCell totw1 = new PdfPCell(new Phrase("Total Discount :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                totw1.Border = 0;
                totw1.HorizontalAlignment = Element.ALIGN_LEFT;
                totalexp.AddCell(totw1);

                tot1 = new PdfPCell(new Phrase("AED", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                tot1.Border = 0;
                tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot1);

                PdfPCell tot2 = new PdfPCell(new Phrase(dtInvoice.Rows[0]["Total_Discount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot2.Border = 0;
                tot2.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot2);

                totalexp.AddCell(Empty);

                PdfPCell totw1w = new PdfPCell(new Phrase("Total After Discount :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                totw1w.Border = 0;
                totw1w.HorizontalAlignment = Element.ALIGN_LEFT;
                totalexp.AddCell(totw1w);

                tot1 = new PdfPCell(new Phrase("AED", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                tot1.Border = 0;
                tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot1);

                PdfPCell tot2ww = new PdfPCell(new Phrase(dtInvoice.Rows[0]["AfterDiscount_GrandTotal"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot2ww.Border = 0;
                tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot2ww);

                if(dtInvoice.Rows[0]["PaymentType"].ToString()=="2" && Convert.ToDecimal(dtInvoice.Rows[0]["ChargedAmount"].ToString())>0)
                {

                    totalexp.AddCell(Empty);

                    PdfPCell totw1wpcc = new PdfPCell(new Phrase("Card Charge Amount :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    totw1wpcc.Border = 0;
                    totw1wpcc.HorizontalAlignment = Element.ALIGN_LEFT;
                    totalexp.AddCell(totw1wpcc);

                    totw1wpcc = new PdfPCell(new Phrase("AED", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    totw1wpcc.Border = 0;
                    totw1wpcc.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(totw1wpcc);

                     totw1wpcc = new PdfPCell(new Phrase(dtInvoice.Rows[0]["ChargedAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    totw1wpcc.Border = 0;
                    totw1wpcc.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(totw1wpcc);

                   
                }

                totalexp.AddCell(Empty);

                PdfPCell totw1wp = new PdfPCell(new Phrase("Paid Amount :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                totw1wp.Border = 0;
                totw1wp.HorizontalAlignment = Element.ALIGN_LEFT;
                totalexp.AddCell(totw1wp);

                tot1 = new PdfPCell(new Phrase("AED", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                tot1.Border = 0;
                tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot1);

                PdfPCell totw1wpd = new PdfPCell(new Phrase(dtInvoice.Rows[0]["Received"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                totw1wpd.Border = 0;
                totw1wpd.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(totw1wpd);

                if (dtInvoice.Rows[0]["PreReceived"].ToString() != "0.00")
                {
                    totalexp.AddCell(Empty);

                    totw1wp = new PdfPCell(new Phrase("Previously Paid :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    totw1wp.Border = 0;
                    totw1wp.HorizontalAlignment = Element.ALIGN_LEFT;
                    totalexp.AddCell(totw1wp);

                    tot1 = new PdfPCell(new Phrase("AED", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    tot1.Border = 0;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot1);

                    totw1wpd = new PdfPCell(new Phrase(dtInvoice.Rows[0]["PreReceived"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    totw1wpd.Border = 0;
                    totw1wpd.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(totw1wpd);
                }

                totalexp.AddCell(Empty);

                PdfPCell totw1wpbl = new PdfPCell(new Phrase("Balance :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                totw1wpbl.Border = 0;
                totw1wpbl.HorizontalAlignment = Element.ALIGN_LEFT;
                totalexp.AddCell(totw1wpbl);

                tot1 = new PdfPCell(new Phrase("AED", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                tot1.Border = 0;
                tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot1);

                PdfPCell totw1wpdbl = new PdfPCell(new Phrase(dtInvoice.Rows[0]["Receivable"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                totw1wpdbl.Border = 0;
                totw1wpdbl.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(totw1wpdbl);

                PdfPCell sub132r = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                sub132r.Border = 0;
                sub132r.Colspan = 4;
                sub132r.MinimumHeight = 7f;
                sub132r.HorizontalAlignment = Element.ALIGN_LEFT;
                totalexp.AddCell(sub132r);

                if (dtInvoice.Rows[0]["ReceiptRemark"].ToString() != "")
                {
                    PdfPCell sub2d1 = new PdfPCell(new Phrase("Remark : " + dtInvoice.Rows[0]["ReceiptRemark"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                    sub2d1.Border = 0;
                    sub2d1.Colspan = 4;
                    sub2d1.HorizontalAlignment = Element.ALIGN_LEFT;
                    totalexp.AddCell(sub2d1);
                }

                PdfPCell totw1wpsss = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                totw1wpsss.Border = 0;
                totw1wpsss.MinimumHeight = 15f;
                totw1wpsss.Colspan = 4;
                totw1wpsss.HorizontalAlignment = Element.ALIGN_LEFT;
                totalexp.AddCell(totw1wpsss);

                 totw1wpsss = new PdfPCell(new Phrase("AED " + ConvertNumbertoWords(Convert.ToDecimal(dtInvoice.Rows[0]["Received"])) + " Only", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLDITALIC)));
                totw1wpsss.Border = 0;
                totw1wpsss.Colspan = 4;
                totw1wpsss.HorizontalAlignment = Element.ALIGN_LEFT;
                totalexp.AddCell(totw1wpsss);

                totalexp.AddCell(sub132r);

                sub132r = new PdfPCell(new Phrase("Prepared By :   " + dtInvoice.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                sub132r.Border = 0;
                sub132r.Colspan = 4;
                sub132r.HorizontalAlignment = Element.ALIGN_LEFT;
                totalexp.AddCell(sub132r);

                sub132r = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                sub132r.Border = PdfPCell.BOTTOM_BORDER;
                sub132r.HorizontalAlignment = Element.ALIGN_LEFT;
                sub132r.Colspan = 4;
                totalexp.AddCell(sub132r);
                /*End of Row*/
                sub132r = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                sub132r.Border = 0;
                sub132r.Colspan = 4;
                sub132r.MinimumHeight = 10f;
                sub132r.HorizontalAlignment = Element.ALIGN_LEFT;
                totalexp.AddCell(sub132r);

                PdfPCell termsCell = new PdfPCell(new Phrase("This document is computer generated and does not require signature or stamp inorder to be considered valid.", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                termsCell.Border = 0;
                termsCell.Colspan = 4;
                termsCell.HorizontalAlignment = Element.ALIGN_LEFT;
                totalexp.AddCell(termsCell);

                document.Add(totalexp);

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

            #region Footer

            if (Application["PrintFooter"] != "")
            {

                PdfPTable footer = new PdfPTable(1);
                footer.DefaultCell.Padding = 4;
                footer.SpacingAfter = 20f;
                footer.TotalWidth = document.PageSize.Width - (2 * document.LeftMargin);
                footer.WidthPercentage = 100f;

                string imageURL = Server.MapPath("../UploadedImage/" + Application["PrintFooter"]);
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(new Uri(imageURL));
                jpg.ScaleToFit(470f, 450f);

                PdfPCell Fotservice = new PdfPCell(jpg, true);
                Fotservice.Border = 0;
                Fotservice.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                footer.AddCell(Fotservice);

                float curY = 0;
                curY = writer.GetVerticalPosition(true);
                float jhgt = footer.GetRowHeight(0) + 30;

                if (jhgt < (curY - 10))
                    footer.WriteSelectedRows(0, -1, document.LeftMargin, jhgt, writer.DirectContent);
                else
                {
                    document.NewPage();
                    footer.WriteSelectedRows(0, -1, document.LeftMargin, jhgt, writer.DirectContent);
                }
            }

            #endregion

            document.Close();
            HttpContext.Current.Response.Write(document);
            HttpContext.Current.Response.End();
        }

        public static string ConvertNumbertoWords(Decimal Number_Value)
        {
            int number = Convert.ToInt32(Math.Floor(Number_Value));
            if (number == 0)
                return "Zero";
            if (number < 0)
                return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = "";
            if ((number / 1000000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000000) + " Million ";
                number %= 1000000;
            }
            if (number == 100000)
            {
                words += ConvertNumbertoWords(number / 100000) + " Hundred Thousand "; //+ " LAKHS ";
                number %= 100000;
            }
            else if ((number / 100000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000) + " Hundred "; //+ " LAKHS ";
                number %= 100000;
            }
            if ((number / 1000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000) + " Thousand ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += ConvertNumbertoWords(number / 100) + " Hundred ";
                number %= 100;
            }
            if (number > 0)
            {
                if (words != "")
                    words += " ";
                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }

            number = (int)((Number_Value - (int)Number_Value) * 100);
            if (number > 0)
            {
                if (words != "")
                    words += " and ";
                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
                if (number < 20)
                {
                    words += unitsMap[number];
                    words += " Fils";
                }
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                    {
                        words += " " + unitsMap[number % 10];
                        words += " Fils";
                    }
                    else
                    {
                        words += " Fils";
                    }
                }
            }
            return words;
        }
    }
}