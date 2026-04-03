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
    public partial class ApplicantSOAPdf : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            int CustomerId = Convert.ToInt32(Request.QueryString["Cus"]);
            string ApplicantName = Request.QueryString["ApplicantName"].ToString();

            DateTime FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            DateTime ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            DataSet ds = obj_report.ApplicantSOAPdf(FromDate, ToDate, CustomerId, ApplicantName);
            DataTable dtCustomer = ds.Tables[0];
            DataTable dtDetails = ds.Tables[1];
            DataTable dtSum = ds.Tables[2];

            Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=ApplicantSOAPdf.pdf");
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
            table1.WidthPercentage = 100;
            PdfPCell cell1 = new PdfPCell(new Phrase("Applicant SOA", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            cell1.Border = 0;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            table1.AddCell(cell1);

            PdfPCell cell45 = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace('-', '/'), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            cell45.Border = 0;
            cell45.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            table1.AddCell(cell45);

            document.Add(table1);

            PdfPTable tbl_basic = new PdfPTable(2);
            tbl_basic.DefaultCell.Padding = 4;
            float[] widths1 = new float[] { 50f, 50f };
            tbl_basic.SetWidths(widths1);
            tbl_basic.HorizontalAlignment = Element.ALIGN_LEFT;
            tbl_basic.WidthPercentage = 50f;
            tbl_basic.SpacingBefore = 10f;
            tbl_basic.SpacingAfter = 10f;

            PdfPCell cust21 = new PdfPCell(new Phrase("Customer", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            cust21.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            cust21.PaddingLeft = 10f;
            tbl_basic.AddCell(cust21);
            PdfPCell cust1 = new PdfPCell(new Phrase(dtCustomer.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            cust1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            cust1.PaddingLeft = 10f;
            tbl_basic.AddCell(cust1);
            cust21 = new PdfPCell(new Phrase("Applicant Name", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            cust21.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            cust21.PaddingLeft = 10f;
            tbl_basic.AddCell(cust21);
            cust1 = new PdfPCell(new Phrase(dtCustomer.Rows[0]["Applicantname"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            cust1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            cust1.PaddingLeft = 10f;
            tbl_basic.AddCell(cust1);
            //cust21 = new PdfPCell(new Phrase("MOHRE No", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            //cust21.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            //cust21.PaddingLeft = 10f;
            //tbl_basic.AddCell(cust21);
            //cust1 = new PdfPCell(new Phrase(dtCustomer.Rows[0]["MohreNo"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            //cust1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            //cust1.PaddingLeft = 10f;
            //tbl_basic.AddCell(cust1);
            //cust21 = new PdfPCell(new Phrase("License No", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            //cust21.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            //cust21.PaddingLeft = 10f;
            //tbl_basic.AddCell(cust21);
            //cust1 = new PdfPCell(new Phrase(dtCustomer.Rows[0]["licenseNo"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            //cust1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            //cust1.PaddingLeft = 10f;
            //tbl_basic.AddCell(cust1);

            PdfPCell CustAdv = new PdfPCell(new Phrase("Opening Balance", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            CustAdv.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            CustAdv.PaddingLeft = 10f;
            tbl_basic.AddCell(CustAdv);
            PdfPCell CustAdv1 = new PdfPCell(new Phrase(dtCustomer.Rows[0]["OpenBalance"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            CustAdv1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            CustAdv1.PaddingLeft = 10f;
            tbl_basic.AddCell(CustAdv1);

            PdfPCell CustOut = new PdfPCell(new Phrase("Outstanding Amount", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            CustOut.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            CustOut.PaddingLeft = 10f;
            tbl_basic.AddCell(CustOut);
            PdfPCell CustOut1 = new PdfPCell(new Phrase(dtSum.Rows.Count > 0 ?(Convert.ToDecimal(dtSum.Rows[0]["Receivable"]) + Convert.ToDecimal(dtCustomer.Rows[0]["OpenBalance"])).ToString() :
                dtCustomer.Rows[0]["OpenBalance"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            CustOut1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            CustOut1.PaddingLeft = 10f;
            tbl_basic.AddCell(CustOut1);

            //CustAdv = new PdfPCell(new Phrase("Advance Payment", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            //CustAdv.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            //CustAdv.PaddingLeft = 10f;
            //tbl_basic.AddCell(CustAdv);
            //CustAdv1 = new PdfPCell(new Phrase(dtCustomer.Rows[0]["Payable"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            //CustAdv1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            //CustAdv1.PaddingLeft = 10f;
            //tbl_basic.AddCell(CustAdv1);

            PdfPCell frmdate = new PdfPCell(new Phrase("From", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            frmdate.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            frmdate.PaddingLeft = 10f;
            tbl_basic.AddCell(frmdate);
            PdfPCell frmdate1 = new PdfPCell(new Phrase((FromDate != null) ? Convert.ToDateTime(FromDate).ToString("dd/MM/yyyy") : "", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            frmdate1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            frmdate1.PaddingLeft = 10f;
            tbl_basic.AddCell(frmdate1);

            PdfPCell todate = new PdfPCell(new Phrase("To", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            todate.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            todate.PaddingLeft = 10f;
            tbl_basic.AddCell(todate);
            PdfPCell todate1 = new PdfPCell(new Phrase((ToDate != null) ? Convert.ToDateTime(ToDate).ToString("dd/MM/yyyy") : "", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            todate1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            todate1.PaddingLeft = 10f;
            tbl_basic.AddCell(todate1);

            document.Add(tbl_basic);

            #endregion

            #region data

            if (dtDetails.Rows.Count > 0)
            {
                PdfPTable emp_details = new PdfPTable(9);
                emp_details.DefaultCell.Padding = 4;
                emp_details.WidthPercentage = 100;
                float[] widths = new float[] { 6f, 10f, 10f, 23f, 17f, 6f, 9f, 9f, 12f };
                emp_details.SetWidths(widths);
                PdfPCell SN = new PdfPCell(new Phrase("Sl.No", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                SN.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(SN);
                SN = new PdfPCell(new Phrase("Date", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                SN.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(SN);
                PdfPCell tp = new PdfPCell(new Phrase("Invoice No", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tp.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(tp);
                PdfPCell tp2 = new PdfPCell(new Phrase("Description", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tp2.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(tp2);
                PdfPCell app = new PdfPCell(new Phrase("Applicant Name", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                app.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(app);
                PdfPCell qty = new PdfPCell(new Phrase("Qty", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                qty.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(qty);
                PdfPCell amt = new PdfPCell(new Phrase("Amount", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                amt.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(amt);
                PdfPCell Paidamt = new PdfPCell(new Phrase("Paid Amount", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                Paidamt.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(Paidamt);
                PdfPCell outst = new PdfPCell(new Phrase("Outstanding", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                outst.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(outst);

                int i = 0;
                foreach (DataRow rows in dtDetails.Rows)
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
                        PdfPCell TP = new PdfPCell(new Phrase(rows["Dated"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell TP = new PdfPCell(new Phrase(rows["Code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE;
                        TP.PaddingLeft = 10f;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell TP = new PdfPCell(new Phrase(rows["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        TP.PaddingLeft = 10f;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell TP = new PdfPCell(new Phrase(rows["ApplicantName"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        TP.PaddingLeft = 10f;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell TP = new PdfPCell(new Phrase(rows["Quantity"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE;
                        TP.PaddingLeft = 10f;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    //try
                    //{
                    //    PdfPTable servtbl = new PdfPTable(1);
                    //    servtbl.WidthPercentage = 100;
                    //    float[] serwidth = new float[] { 100f };
                    //    servtbl.SetWidths(serwidth);

                    //    int servcnt = 0;
                    //    for (int j = 0; j < dtSubDetails.Rows.Count; j++)
                    //    {
                    //        if (dtSubDetails.Rows[j]["InvoiceId"].ToString() == rows["InvoiceId"].ToString())
                    //        {
                    //            ++servcnt;
                    //            PdfPCell TP = new PdfPCell(new Phrase(dtSubDetails.Rows[j]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    //            TP.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    //            TP.MinimumHeight = 15f;
                    //            if (servcnt == Convert.ToInt32(rows["cnt"]))
                    //                TP.Border = 0;
                    //            else
                    //                TP.Border = 2;
                    //            TP.PaddingLeft = 10f;
                    //            servtbl.AddCell(TP);
                    //        }
                    //    }

                    //    PdfPCell TPinnr1 = new PdfPCell();
                    //    TPinnr1.AddElement(servtbl);
                    //    emp_details.AddCell(TPinnr1);
                    //}
                    //catch (Exception eee)
                    //{
                    //    emp_details.AddCell("");
                    //}
                    //try
                    //{
                    //    PdfPTable servtbl = new PdfPTable(1);
                    //    servtbl.WidthPercentage = 100;
                    //    float[] serwidth = new float[] { 100f };
                    //    servtbl.SetWidths(serwidth);

                    //    int servcnt = 0;
                    //    for (int j = 0; j < dtSubDetails.Rows.Count; j++)
                    //    {
                    //        if (dtSubDetails.Rows[j]["InvoiceId"].ToString() == rows["InvoiceId"].ToString())
                    //        {
                    //            ++servcnt;
                    //            PdfPCell TP = new PdfPCell(new Phrase(dtSubDetails.Rows[j]["ApplicantName"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    //            TP.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    //            if (servcnt == Convert.ToInt32(rows["cnt"]))
                    //                TP.Border = 0;
                    //            else
                    //                TP.Border = 2;
                    //            TP.MinimumHeight = 15f;
                    //            TP.PaddingLeft = 10f;
                    //            servtbl.AddCell(TP);
                    //        }
                    //    }

                    //    PdfPCell TPinnr1 = new PdfPCell();
                    //    TPinnr1.AddElement(servtbl);
                    //    emp_details.AddCell(TPinnr1);
                    //}
                    //catch (Exception eee)
                    //{
                    //    emp_details.AddCell("");
                    //}
                    //try
                    //{
                    //    PdfPTable servtbl = new PdfPTable(1);
                    //    servtbl.DefaultCell.Padding = 4;
                    //    servtbl.WidthPercentage = 100;
                    //    float[] serwidth = new float[] { 100f };
                    //    servtbl.SetWidths(serwidth);
                    //    int servcnt = 0;
                    //    for (int j = 0; j < dtSubDetails.Rows.Count; j++)
                    //    {
                    //        if (dtSubDetails.Rows[j]["InvoiceId"].ToString() == rows["InvoiceId"].ToString())
                    //        {
                    //            ++servcnt;
                    //            PdfPCell TP = new PdfPCell(new Phrase(dtSubDetails.Rows[j]["Quantity"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    //            TP.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    //            if (servcnt == Convert.ToInt32(rows["cnt"]))
                    //                TP.Border = 0;
                    //            else
                    //                TP.Border = 2;
                    //            TP.MinimumHeight = 15f;
                    //            TP.PaddingLeft = 10f;
                    //            servtbl.AddCell(TP);
                    //        }
                    //    }

                    //    PdfPCell TPinnr1 = new PdfPCell();
                    //    TPinnr1.AddElement(servtbl);
                    //    emp_details.AddCell(TPinnr1);
                    //}
                    //catch (Exception eee)
                    //{
                    //    emp_details.AddCell("");
                    //}
                    try
                    {
                        PdfPCell TP = new PdfPCell(new Phrase(rows["AfterDiscount_Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE;
                        TP.PaddingLeft = 10f;
                        TP.MinimumHeight = 10f;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell TP = new PdfPCell(new Phrase(rows["receivedAmtService"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE;
                        TP.PaddingLeft = 10f;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell TP = new PdfPCell(new Phrase(rows["Balance"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE;
                        TP.PaddingLeft = 10f;
                        emp_details.AddCell(TP);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }

                }

                PdfPCell total = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                total.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                total.Colspan = 6;
                total.PaddingLeft = 10f;
                emp_details.AddCell(total);

                PdfPCell total1 = new PdfPCell(new Phrase(dtSum.Rows[0]["Amount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                total1.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE;
                total1.PaddingLeft = 10f;
                emp_details.AddCell(total1);

                PdfPCell total2 = new PdfPCell(new Phrase(dtSum.Rows[0]["Received"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                total2.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE;
                total2.PaddingLeft = 10f;
                emp_details.AddCell(total2);

                PdfPCell total3 = new PdfPCell(new Phrase(dtSum.Rows[0]["Receivable"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                total3.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE;
                total3.PaddingLeft = 10f;
                emp_details.AddCell(total3);

                document.Add(emp_details);

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