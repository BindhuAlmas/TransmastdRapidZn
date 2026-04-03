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
    public partial class CustomerSOAPdfFormat9 : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            int CustomerId = Convert.ToInt32(Request.QueryString["Cus"]);
            int PaymentStatus = Convert.ToInt32(Request.QueryString["PaymentStatus"]);
            int CompletionStatus = Convert.ToInt32(Request.QueryString["CompletionStatus"]);

            DateTime FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            DateTime ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            DataSet ds = obj_report.CustomerSOAPrintFormat8(FromDate, ToDate, CustomerId, PaymentStatus, CompletionStatus);
            DataTable dtCustomer = ds.Tables[0];
            DataTable dtDetails = ds.Tables[1];
            DataTable dtSubDetails = ds.Tables[2];
            DataTable dtInvoiceSum = ds.Tables[3];
            DataTable dtreceipt = ds.Tables[4];
            DataTable dtReceiptTot = ds.Tables[5];
            DataTable dtOb = ds.Tables[6];

            Document document = new Document(PageSize.A4, 20f, 20f, 10f, 10f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=" + dtCustomer.Rows[0]["Name"].ToString().Replace(",", "") + ".pdf");
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

            decimal totob = Convert.ToDecimal(dtOb.Rows[0][0]);
            decimal totinvsum = 0;
            decimal totrecsum = 0;
            totinvsum = (dtInvoiceSum.Rows[0][0].ToString() == "" ? 0 : Convert.ToDecimal(dtInvoiceSum.Rows[0][0])) + (totob);
            totrecsum = dtReceiptTot.Rows[0]["ReceivedAmount"].ToString() == "" ? 0 : (Convert.ToDecimal(dtReceiptTot.Rows[0]["ReceivedAmount"]));

            #region header

            PdfPTable incomzdvgsdzbg = new PdfPTable(1);
            incomzdvgsdzbg.DefaultCell.Padding = 4;
            incomzdvgsdzbg.WidthPercentage = 90f;
            PdfPCell cell1 = new PdfPCell(new Phrase("Customer SOA", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            cell1.Border = 0;
            cell1.HorizontalAlignment = 1;
            incomzdvgsdzbg.AddCell(cell1);
            if (CompletionStatus == 1)
            {
                cell1 = new PdfPCell(new Phrase("(Completed Transactions)", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                cell1.Border = 0;
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                incomzdvgsdzbg.AddCell(cell1);
            }
            else if (CompletionStatus == 2)
            {
                cell1 = new PdfPCell(new Phrase("(Pending Transactions)", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                cell1.Border = 0;
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                incomzdvgsdzbg.AddCell(cell1);
            }
            if (FromDate != null && ToDate != null)
            {
                PdfPCell cell2 = new PdfPCell(new Phrase("From: " + Convert.ToDateTime(FromDate).ToString("dd/MM/yyyy") + " To: " + Convert.ToDateTime(ToDate).ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                cell2.Border = 0;
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                incomzdvgsdzbg.AddCell(cell2);
            }
            else if (FromDate != null && ToDate == null)
            {
                PdfPCell cell2 = new PdfPCell(new Phrase("From: " + Convert.ToDateTime(FromDate).ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                cell2.Border = 0;
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                incomzdvgsdzbg.AddCell(cell2);
            }
            else if (FromDate == null && ToDate != null)
            {
                PdfPCell cell2 = new PdfPCell(new Phrase(" To: " + Convert.ToDateTime(ToDate).ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                cell2.Border = 0;
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                incomzdvgsdzbg.AddCell(cell2);
            }
            PdfPCell docDate = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace('-', '/'), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            docDate.Border = 0;
            docDate.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            incomzdvgsdzbg.AddCell(docDate);
            if (dtCustomer.Rows[0]["TRN"].ToString() != "")
            {
                docDate = new PdfPCell(new Phrase("TRN : " + dtCustomer.Rows[0]["TRN"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                docDate.Border = 0;
                docDate.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                incomzdvgsdzbg.AddCell(docDate);
            }

            document.Add(incomzdvgsdzbg);

            PdfPTable headr = new PdfPTable(3);
            headr.DefaultCell.Padding = 4;
            headr.WidthPercentage = 100;
            float[] widths2 = new float[] { 15f, 60f, 25f };
            headr.SetWidths(widths2);

            PdfPCell Serial_Nocu = new PdfPCell(new Phrase("Customer :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            Serial_Nocu.HorizontalAlignment = Element.ALIGN_LEFT;
            Serial_Nocu.Border = 0;
            headr.AddCell(Serial_Nocu);
            PdfPCell Sertty = new PdfPCell(new Phrase(dtCustomer.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            Sertty.HorizontalAlignment = Element.ALIGN_LEFT;
            Sertty.Border = 0;
            headr.AddCell(Sertty);

            Sertty = new PdfPCell(new Phrase("Account Summary", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            Sertty.HorizontalAlignment = Element.ALIGN_LEFT;
            Sertty.Border = 0;
            headr.AddCell(Sertty);

            PdfPCell Serial_Noeecu = new PdfPCell(new Phrase("Mobile :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            Serial_Noeecu.HorizontalAlignment = Element.ALIGN_LEFT;
            Serial_Noeecu.Border = 0;
            headr.AddCell(Serial_Noeecu);
            PdfPCell Serteety = new PdfPCell(new Phrase(dtCustomer.Rows[0]["Mobile_num"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            Serteety.HorizontalAlignment = Element.ALIGN_LEFT;
            Serteety.Border = 0;
            headr.AddCell(Serteety);

            Sertty = new PdfPCell(new Phrase("Previous Balance : " + dtOb.Rows[0][0].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            Sertty.HorizontalAlignment = Element.ALIGN_LEFT;
            Sertty.Border = 0;
            headr.AddCell(Sertty);

            Serial_Noeecu = new PdfPCell(new Phrase("TRN :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            Serial_Noeecu.HorizontalAlignment = Element.ALIGN_LEFT;
            Serial_Noeecu.Border = 0;
            headr.AddCell(Serial_Noeecu);
            Serteety = new PdfPCell(new Phrase(dtCustomer.Rows[0]["CustomerTRN"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            Serteety.HorizontalAlignment = Element.ALIGN_LEFT;
            Serteety.Border = 0;
            headr.AddCell(Serteety);

            Sertty = new PdfPCell(new Phrase("Invoiced Amount : " + (totinvsum - totob).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            Sertty.HorizontalAlignment = Element.ALIGN_LEFT;
            Sertty.Border = 0;
            headr.AddCell(Sertty);

            PdfPCell addd = new PdfPCell(new Phrase("Address :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            addd.HorizontalAlignment = Element.ALIGN_LEFT;
            addd.Border = 0;
            headr.AddCell(addd);
            PdfPCell Serteesty = new PdfPCell(new Phrase(dtCustomer.Rows[0]["Address"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            Serteesty.HorizontalAlignment = Element.ALIGN_LEFT;
            Serteesty.Border = 0;
            headr.AddCell(Serteesty);

            Sertty = new PdfPCell(new Phrase("Amount Received : " + (totrecsum).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            Sertty.HorizontalAlignment = Element.ALIGN_LEFT;
            Sertty.Border = 0;
            headr.AddCell(Sertty);

            addd = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            addd.HorizontalAlignment = Element.ALIGN_LEFT;
            addd.Border = 0;
            addd.Colspan = 2;
            headr.AddCell(addd);

            Sertty = new PdfPCell(new Phrase("Balance Amount : " + (totinvsum - totrecsum).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            Sertty.HorizontalAlignment = Element.ALIGN_LEFT;
            Sertty.Border = 0;
            headr.AddCell(Sertty);

            document.Add(headr);

            #endregion

            #region data

            PdfPTable income_details = new PdfPTable(9);
            income_details.DefaultCell.Padding = 4;
            income_details.SpacingBefore = 5;
            income_details.WidthPercentage = 100;
            float[] widths = new float[] { 7f, 10f, 8f, 30f, 10f, 10f, 9f, 10f, 10f };
            income_details.SetWidths(widths);


            if (dtDetails.Rows.Count > 0 || dtreceipt.Rows.Count > 0)
            {

                if (dtDetails.Rows.Count > 0)
                {
                    PdfPCell invhd = new PdfPCell(new Phrase("Invoice", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD)));
                    invhd.HorizontalAlignment = Element.ALIGN_LEFT;
                    invhd.Border = 0;
                    invhd.Colspan = 9;
                    invhd.MinimumHeight = 20;
                    income_details.AddCell(invhd);

                    PdfPCell Serial_No = new PdfPCell(new Phrase("Sl No", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    Serial_No.HorizontalAlignment = Element.ALIGN_CENTER;
                    income_details.AddCell(Serial_No);
                    PdfPCell Sert = new PdfPCell(new Phrase("Date", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    Sert.HorizontalAlignment = Element.ALIGN_CENTER;
                    income_details.AddCell(Sert);
                    PdfPCell Ser = new PdfPCell(new Phrase("Invoice", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    Ser.HorizontalAlignment = Element.ALIGN_CENTER;
                    income_details.AddCell(Ser);
                    PdfPCell Qua = new PdfPCell(new Phrase("Service", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    Qua.HorizontalAlignment = Element.ALIGN_CENTER;
                    income_details.AddCell(Qua);
                    PdfPCell Quanot = new PdfPCell(new Phrase("Quantity", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    Quanot.HorizontalAlignment = Element.ALIGN_CENTER;
                    income_details.AddCell(Quanot);
                    Quanot = new PdfPCell(new Phrase("Rate", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    Quanot.HorizontalAlignment = Element.ALIGN_CENTER;
                    income_details.AddCell(Quanot);
                    Quanot = new PdfPCell(new Phrase("Discount", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    Quanot.HorizontalAlignment = Element.ALIGN_CENTER;
                    income_details.AddCell(Quanot);
                    PdfPCell Quano = new PdfPCell(new Phrase("Amount", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    Quano.HorizontalAlignment = Element.ALIGN_CENTER;
                    income_details.AddCell(Quano);
                    Quano = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    Quano.HorizontalAlignment = Element.ALIGN_CENTER;
                    income_details.AddCell(Quano);

                    int i = 0;
                    foreach (DataRow rows in dtDetails.Rows)
                    {
                        DataTable dh = new DataTable();
                        dh = dtSubDetails.Clone();

                        string query = "Code LIKE '%" + rows["Code"].ToString() + "%'";

                        DataRow[] dr = dtSubDetails.Select(query);
                        int cv = dr.Length;
                        if (cv > 0)
                        {
                            dh = dr.CopyToDataTable();
                        }

                        try
                        {
                            PdfPCell serial_no = new PdfPCell(new Phrase((++i).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            serial_no.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            serial_no.Border = Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER;
                            serial_no.Rowspan = dh.Rows.Count;
                            income_details.AddCell(serial_no);
                        }
                        catch (Exception ee)
                        {
                            income_details.AddCell("");
                        }
                        try
                        {
                            PdfPCell typee = new PdfPCell(new Phrase(rows["Dated"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            typee.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            typee.Border = Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER;
                            typee.Rowspan = dh.Rows.Count;
                            income_details.AddCell(typee);
                        }
                        catch (Exception eee)
                        {
                            income_details.AddCell("");
                        }
                        try
                        {
                            PdfPCell typee = new PdfPCell(new Phrase(rows["Code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            typee.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            typee.Border = Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER;
                            typee.Rowspan = dh.Rows.Count;
                            income_details.AddCell(typee);
                        }
                        catch (Exception eee)
                        {
                            income_details.AddCell("");
                        }

                        int rr = 0;
                        foreach (DataRow r in dh.Rows)
                        {

                            PdfPCell Stypee = new PdfPCell(new Phrase(r["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            Stypee.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            Stypee.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                            if (rr == 0)
                                Stypee.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER;
                            income_details.AddCell(Stypee);

                            Stypee = new PdfPCell(new Phrase(r["Quantity"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            Stypee.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            Stypee.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                            if (rr == 0)
                                Stypee.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER;
                            income_details.AddCell(Stypee);

                            Stypee = new PdfPCell(new Phrase(r["RateWitDis"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            Stypee.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            Stypee.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                            if (rr == 0)
                                Stypee.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER;
                            income_details.AddCell(Stypee);

                            Stypee = new PdfPCell(new Phrase(r["Discount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            Stypee.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            Stypee.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                            if (rr == 0)
                                Stypee.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER;
                            income_details.AddCell(Stypee);

                            Stypee = new PdfPCell(new Phrase(r["AmountNoFine"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            Stypee.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            Stypee.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                            if (rr == 0)
                                Stypee.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER;
                            income_details.AddCell(Stypee);

                            if (rr == 0)
                            {
                                try
                                {
                                    PdfPCell typee = new PdfPCell(new Phrase(rows["Amount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                                    typee.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                    typee.Border = Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER;
                                    //typee.Rowspan = dh.Rows.Count;
                                    income_details.AddCell(typee);
                                }
                                catch (Exception eee)
                                {
                                    income_details.AddCell("");
                                }
                            }
                            //else if (rr == dh.Rows.Count-1)
                            //{
                            //    Stypee = new PdfPCell(new Phrase(r["AmountNoFine"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            //    Stypee.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            //    Stypee.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER; ;
                            //}
                            else
                            {
                                Stypee = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                                Stypee.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                Stypee.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                                income_details.AddCell(Stypee);
                            }

                            rr++;
                        }

                    }

                    PdfPCell tot = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    tot.HorizontalAlignment = Element.ALIGN_RIGHT;
                    tot.Colspan = 8;
                    tot.Border = Rectangle.TOP_BORDER;
                    income_details.AddCell(tot);

                    PdfPCell totinv = new PdfPCell(new Phrase((totinvsum - totob).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    totinv.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totinv.Border = Rectangle.TOP_BORDER;
                    income_details.AddCell(totinv);

                    document.Add(income_details);
                }

                if (dtreceipt.Rows.Count > 0)
                {
                    PdfPTable Paymnt = new PdfPTable(5);
                    Paymnt.DefaultCell.Padding = 4;
                    Paymnt.SpacingBefore = 20;
                    Paymnt.WidthPercentage = 100;
                    float[] widthsp = new float[] { 5f, 10f, 20f, 20f, 10f };
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

                    PdfPCell tots = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    tots.HorizontalAlignment = Element.ALIGN_RIGHT;
                    tots.Colspan = 4;
                    tots.Border = 0;
                    Paymnt.AddCell(tots);

                    PdfPCell totiddnv = new PdfPCell(new Phrase(totrecsum.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    totiddnv.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totiddnv.Border = 0;
                    Paymnt.AddCell(totiddnv);


                    PdfPCell totsss = new PdfPCell(new Phrase("Balance", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    totsss.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totsss.Colspan = 4;
                    totsss.Border = 0;
                    totsss.VerticalAlignment = Element.ALIGN_MIDDLE;
                    totsss.MinimumHeight = 30;
                    Paymnt.AddCell(totsss);

                    PdfPCell totiddssnv = new PdfPCell(new Phrase((totinvsum - totrecsum).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    totiddssnv.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totiddssnv.Border = 0;
                    totiddssnv.VerticalAlignment = Element.ALIGN_MIDDLE;
                    Paymnt.AddCell(totiddssnv);

                    document.Add(Paymnt);
                }

            }

            #endregion

            #region Footer

            if (Application["PrintFooter"].ToString() != "")
            {
                PdfPTable footer = new PdfPTable(1);
                footer.DefaultCell.Padding = 4;
                //footer.SpacingAfter = 20f;
                footer.TotalWidth = document.PageSize.Width - (2 * document.LeftMargin);
                footer.WidthPercentage = 90f;

                string imageURL = Server.MapPath("../UploadedImage/" + Application["PrintFooter"].ToString());
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
    }
}