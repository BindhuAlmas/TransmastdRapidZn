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
    public partial class ReceiptPrint : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();
        Master_Bal obj_master = new Master_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
                int id = Convert.ToInt32(Request.QueryString["Id"]);
                DataSet ds = obj_report.Receipt_Print(id);
                DataTable dt_rec = ds.Tables[0];
                DataTable dt_invD = ds.Tables[1];
                DataTable dt_sum = ds.Tables[2];

            Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);

                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=ReceiptPrint.pdf");
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

                PdfPTable Subhead = new PdfPTable(5);
                Subhead.DefaultCell.Padding = 4;
                Subhead.SpacingBefore = 20f;
                float[] widths = new float[] { 18f, 25f, 40f, 25f, 15f };
                Subhead.SetWidths(widths);
                Subhead.WidthPercentage = 90f;

                // empty cell

                PdfPCell Empty = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                Empty.Border = 0;
                Empty.HorizontalAlignment = Element.ALIGN_LEFT;

                //end

                PdfPCell sub1 = new PdfPCell(new Phrase("Customer :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                sub1.Border = 0;
                sub1.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub1);

                PdfPCell sub12 = new PdfPCell(new Phrase(dt_rec.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                sub12.Border = 0;
                sub12.Colspan = 2;
                sub12.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub12);

                PdfPCell sub13 = new PdfPCell(new Phrase("Receipt No :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                sub13.Border = 0;
                sub13.HorizontalAlignment = Element.ALIGN_RIGHT;
                Subhead.AddCell(sub13);

                PdfPCell sub14 = new PdfPCell(new Phrase(dt_rec.Rows[0]["Code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                sub14.Border = 0;
                sub14.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub14);

                PdfPCell sub21 = new PdfPCell(new Phrase("Contact :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                sub21.Border = 0;
                sub21.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub21);

                PdfPCell sub122 = new PdfPCell(new Phrase(dt_rec.Rows[0]["Mobile_num"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                sub122.Border = 0;
                sub122.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub122);

                Subhead.AddCell(Empty);

                PdfPCell sub132 = new PdfPCell(new Phrase("Invoice :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                sub132.Border = 0;
                sub132.HorizontalAlignment = Element.ALIGN_RIGHT;
                Subhead.AddCell(sub132);

                PdfPCell sub142 = new PdfPCell(new Phrase(dt_rec.Rows[0]["InvoiceCode"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                sub142.Border = 0;
                sub142.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub142);

                PdfPCell sub21add = new PdfPCell(new Phrase("TRN :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                sub21add.Border = 0;
                sub21add.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub21add);

                PdfPCell sub122add = new PdfPCell(new Phrase(dt_rec.Rows[0]["TRN"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                sub122add.Border = 0;
                sub122add.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub122add);

                Subhead.AddCell(Empty);

                sub132 = new PdfPCell(new Phrase("Date :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                sub132.Border = 0;
                sub132.HorizontalAlignment = Element.ALIGN_RIGHT;
                Subhead.AddCell(sub132);

                sub142 = new PdfPCell(new Phrase(dt_rec.Rows[0]["Date"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                sub142.Border = 0;
                sub142.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub142);

                sub21add = new PdfPCell(new Phrase("Address :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                sub21add.Border = 0;
                sub21add.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub21add);

                sub122add = new PdfPCell(new Phrase(dt_rec.Rows[0]["Address"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                sub122add.Border = 0;
                sub122add.Colspan = 2;
                sub122add.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub122add);

                //if (dt1.Rows[0]["TaxPrint"].ToString() == "1")
                //{
                //    sub122add = new PdfPCell(new Phrase("TRN : " + dt1.Rows[0]["TRN"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                //    sub122add.Border = 0;
                //    sub122add.Colspan = 2;
                //    sub122add.HorizontalAlignment = Element.ALIGN_RIGHT;
                //    Subhead.AddCell(sub122add);

                //}
                //else
                //{
                //    Subhead.AddCell(Empty);
                //    Subhead.AddCell(Empty);
                //}

                document.Add(Subhead);

                #endregion

                #region data

                if (dt_invD.Rows.Count > 0)
                {
                    PdfPTable emp_details  = new PdfPTable(9);

                    emp_details.DefaultCell.Padding = 4;
                    float[] widthsdet = new float[] { 8f, 25f, 25f, 12f, 8f, 9f, 12f, 14f, 12f };
                    emp_details.SetWidths(widthsdet);

                    emp_details.SpacingBefore = 20f;
                    emp_details.WidthPercentage = 90f;
                    PdfPCell SN = new PdfPCell(new Phrase("S.No", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    SN.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(SN);
                    PdfPCell ty = new PdfPCell(new Phrase("Service", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ty.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(ty);
                    PdfPCell nam = new PdfPCell(new Phrase("Particular", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    nam.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(nam);
                    PdfPCell da = new PdfPCell(new Phrase("Unit Price", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    da.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(da);
                    PdfPCell inc = new PdfPCell(new Phrase("Qty", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    inc.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(inc);

                        inc = new PdfPCell(new Phrase("Tax", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                        inc.HorizontalAlignment = Element.ALIGN_CENTER;
                        emp_details.AddCell(inc);
                        da = new PdfPCell(new Phrase("Amt with Tax", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                        da.HorizontalAlignment = Element.ALIGN_CENTER;
                        emp_details.AddCell(da);

                    PdfPCell exp = new PdfPCell(new Phrase("Discount", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    exp.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(exp);
                    PdfPCell exptt = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    exptt.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(exptt);

                    BaseFont bfTimes = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Font/arabtype.ttf"), BaseFont.IDENTITY_H, true);
                    iTextSharp.text.Font arbfnt = new iTextSharp.text.Font(bfTimes, 17, Font.NORMAL);


                    int i = 0;

                    foreach (DataRow rows in dt_invD.Rows)
                    {
                        try
                        {
                            PdfPCell sn = new PdfPCell(new Phrase((++i).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            sn.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            emp_details.AddCell(sn);
                        }
                        catch (Exception ee)
                        {
                            emp_details.AddCell("N/A");
                        }
                        try
                        {
                            PdfPTable intble = new PdfPTable(1);
                            intble.WidthPercentage = 95f;

                            PdfPCell TP = new PdfPCell(new Phrase(rows["Name"].ToString() + "\n", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            TP.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            TP.Border = 0;
                            intble.AddCell(TP);

                            //if (dt1.Rows[0]["IsServArbc"].ToString() == "1")
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
                            emp_details.AddCell("N/A");
                        }

                        try
                        {
                            PdfPCell REM = new PdfPCell(new Phrase(rows["Particulars"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            REM.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            emp_details.AddCell(REM);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("N/A");
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Price"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(DT);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("N/A");
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Quantity"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(DT);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("N/A");
                        }

                            try
                            {
                                PdfPCell DT = new PdfPCell(new Phrase(rows["TaxAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                                DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                emp_details.AddCell(DT);
                            }
                            catch (Exception eee)
                            {
                                emp_details.AddCell("");
                            }
                            try
                            {
                                PdfPCell DT = new PdfPCell(new Phrase(rows["PriceWithTax"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                                DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                emp_details.AddCell(DT);
                            }
                            catch (Exception eee)
                            {
                                emp_details.AddCell("");
                            }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["DisCount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(DT);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("N/A");
                        }

                        try
                        {

                            PdfPCell DT = new PdfPCell(new Phrase(rows["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(DT);

                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("N/A");
                        }

                    }

                    PdfPCell DTw = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    DTw.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DTw.Colspan = 4;
                    DTw.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                    DTw.MinimumHeight = 20f;
                    emp_details.AddCell(DTw);

                    PdfPCell DTwt = new PdfPCell(new Phrase(dt_sum.Rows[0]["Quantity"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    DTwt.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DTwt.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                    emp_details.AddCell(DTwt);
                        DTwt = new PdfPCell(new Phrase(dt_sum.Rows[0]["TaxAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        DTwt.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        DTwt.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                        emp_details.AddCell(DTwt);
                        DTwt = new PdfPCell(new Phrase(dt_sum.Rows[0]["PriceWithTax"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        DTwt.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        DTwt.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                        emp_details.AddCell(DTwt);
                    DTwt = new PdfPCell(new Phrase(dt_sum.Rows[0]["Discount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    DTwt.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DTwt.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                    emp_details.AddCell(DTwt);
                    DTwt = new PdfPCell(new Phrase(dt_sum.Rows[0]["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    DTwt.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DTwt.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                    emp_details.AddCell(DTwt);

                    document.Add(emp_details);

                    PdfPTable totalexp = new PdfPTable(4);
                    totalexp.DefaultCell.Padding = 4;
                    float[] widths1 = new float[] { 55f, 20f, 10f, 13f };
                    totalexp.SetWidths(widths1);
                    totalexp.SpacingBefore = 10;
                    totalexp.WidthPercentage = 90f;

                    totalexp.AddCell(Empty);

                    PdfPCell tot = new PdfPCell(new Phrase("Total Amount :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    tot.Border = 0;
                    tot.HorizontalAlignment = Element.ALIGN_LEFT;
                    totalexp.AddCell(tot);

                    PdfPCell tot1 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    tot1.Border = 0;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot1);

                    tot1 = new PdfPCell(new Phrase(dt_rec.Rows[0]["Grand_Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    tot1.Border = 0;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot1);

                    totalexp.AddCell(Empty);

                    PdfPCell totw1 = new PdfPCell(new Phrase("Total Discount :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    totw1.Border = 0;
                    totw1.HorizontalAlignment = Element.ALIGN_LEFT;
                    totalexp.AddCell(totw1);

                    tot1 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    tot1.Border = 0;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot1);

                    PdfPCell tot2 = new PdfPCell(new Phrase(dt_rec.Rows[0]["Total_Discount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    tot2.Border = 0;
                    tot2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot2);

                    totalexp.AddCell(Empty);

                    PdfPCell totw1w = new PdfPCell(new Phrase("Total After Discount :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    totw1w.Border = 0;
                    totw1w.HorizontalAlignment = Element.ALIGN_LEFT;
                    totalexp.AddCell(totw1w);

                    tot1 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    tot1.Border = 0;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot1);

                    PdfPCell tot2ww = new PdfPCell(new Phrase(dt_rec.Rows[0]["AfterDiscount_GrandTotal"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    tot2ww.Border = 0;
                    tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot2ww);

                    totalexp.AddCell(Empty);

                    PdfPCell totw1wp = new PdfPCell(new Phrase("Paid Amount :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    totw1wp.Border = 0;
                    totw1wp.HorizontalAlignment = Element.ALIGN_LEFT;
                    totalexp.AddCell(totw1wp);

                    tot1 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    tot1.Border = 0;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot1);

                    PdfPCell totw1wpd = new PdfPCell(new Phrase(dt_rec.Rows[0]["Amount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    totw1wpd.Border = 0;
                    totw1wpd.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(totw1wpd);

                    totalexp.AddCell(Empty);

                    PdfPCell totw1wpbl = new PdfPCell(new Phrase("Balance :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    totw1wpbl.Border = 0;
                    totw1wpbl.HorizontalAlignment = Element.ALIGN_LEFT;
                    totalexp.AddCell(totw1wpbl);

                    tot1 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    tot1.Border = 0;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot1);

                    PdfPCell totw1wpdbl = new PdfPCell(new Phrase(dt_rec.Rows[0]["Receivable"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    totw1wpdbl.Border = 0;
                    totw1wpdbl.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(totw1wpdbl);

                    PdfPCell sub132r = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    sub132r.Border = 0;
                    sub132r.Colspan = 4;
                    sub132r.MinimumHeight = 7f;
                    sub132r.HorizontalAlignment = Element.ALIGN_LEFT;
                    totalexp.AddCell(sub132r);

                    PdfPCell totw1wpsss = new PdfPCell(new Phrase("AED " + ConvertNumbertoWords(Convert.ToDecimal(dt_rec.Rows[0]["Amount"])) + " Only", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLDITALIC)));
                    totw1wpsss.Border = 0;
                    totw1wpsss.Colspan = 4;
                    totw1wpsss.HorizontalAlignment = Element.ALIGN_LEFT;
                    totalexp.AddCell(totw1wpsss);

                    totalexp.AddCell(sub132r);

                    sub132r = new PdfPCell(new Phrase("Prepared By :   " + dt_rec.Rows[0]["PreparedBy"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    sub132r.Border = 0;
                    sub132r.Colspan = 4;
                    sub132r.HorizontalAlignment = Element.ALIGN_LEFT;
                    totalexp.AddCell(sub132r);

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

                //#region Remark

                //if (dt1.Rows[0]["IsAddRemark"].ToString() == "1")
                //{
                //    Subhead = new PdfPTable(5);
                //    Subhead.DefaultCell.Padding = 4;
                //    Subhead.SpacingBefore = 20f;
                //    Subhead.SetWidths(widths);
                //    Subhead.WidthPercentage = 90f;

                //    PdfPCell sub2d1 = new PdfPCell(new Phrase("Remark :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                //    sub2d1.Border = 0;
                //    sub2d1.HorizontalAlignment = Element.ALIGN_LEFT;
                //    Subhead.AddCell(sub2d1);

                //    Subhead.AddCell(Empty);
                //    Subhead.AddCell(Empty);
                //    Subhead.AddCell(Empty);
                //    Subhead.AddCell(Empty);

                //    PdfPCell sub2dre1 = new PdfPCell(new Phrase("   " + dt1.Rows[0]["Remarks"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                //    sub2dre1.Border = 0;
                //    sub2dre1.Colspan = 5;
                //    sub2dre1.HorizontalAlignment = Element.ALIGN_LEFT;
                //    Subhead.AddCell(sub2dre1);

                //    document.Add(Subhead);
                //}

                //#endregion

                

                //#region remarks

                //if (dt_gen.Rows.Count > 0)
                //{
                //    if (dt_gen.Rows[0]["Receipt_Remark"].ToString() != "")
                //    {
                //        PdfPTable Ftr = new PdfPTable(1);
                //        Ftr.DefaultCell.Padding = 4;
                //        Ftr.SpacingBefore = 20f;
                //        Ftr.WidthPercentage = 90f;

                //        PdfPCell TP = new PdfPCell(new Phrase(dt_gen.Rows[0]["Receipt_Remark"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                //        TP.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                //        TP.Border = 0;
                //        Ftr.AddCell(TP);

                //        document.Add(Ftr);
                //    }
                //}

                //#endregion

                //#region Footer

                //if (Application["PrintFooter"] != "")
                //{

                //    PdfPTable footer = new PdfPTable(1);
                //    footer.DefaultCell.Padding = 4;
                //    footer.SpacingAfter = 20f;
                //    footer.TotalWidth = document.PageSize.Width - (2 * document.LeftMargin);
                //    footer.WidthPercentage = 90f;

                //    string imageURL = Server.MapPath("../Images/" + Application["PrintFooter"]);
                //    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(new Uri(imageURL));
                //    jpg.ScaleToFit(470f, 450f);

                //    PdfPCell Fotservice = new PdfPCell(jpg, true);
                //    Fotservice.Border = 0;
                //    Fotservice.FixedHeight = jpg.Height;
                //    Fotservice.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                //    footer.AddCell(Fotservice);

                //    footer.WriteSelectedRows(0, -1, document.LeftMargin, jpg.Height / 2 + 20, writer.DirectContent);

                //}

                //#endregion

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