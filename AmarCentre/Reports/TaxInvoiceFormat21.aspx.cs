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
    public partial class TaxInvoiceFormat21 : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();
        Master_Bal obj_master = new Master_Bal();


        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"]);
            DataSet ds = obj_report.TaxInvoicePrint11_12(id);
            DataTable dt_inv = ds.Tables[0];
            DataTable dt_invD = ds.Tables[1];
            DataTable dt_cust = ds.Tables[2];
            DataTable dt_sum = ds.Tables[3];
            DataTable dtGeneral = ds.Tables[4];

            Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=InvoicePrint.pdf");
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
            document.Open();
            if (dtGeneral.Rows[0]["PrintHeader"].ToString() != "")
            {

                string imageURL = Server.MapPath("../UploadedImage/" + dtGeneral.Rows[0]["PrintHeader"].ToString());
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

            iTextSharp.text.Font arbsmallN = new iTextSharp.text.Font(bfTimes, 10, Font.NORMAL);
            iTextSharp.text.Font arbsmallbld = new iTextSharp.text.Font(bfTimes, 10, Font.BOLD);
            iTextSharp.text.Font arbfntN = new iTextSharp.text.Font(bfTimes, 12, Font.NORMAL);
            iTextSharp.text.Font arbfntbld = new iTextSharp.text.Font(bfTimes, 12, Font.BOLD);

            iTextSharp.text.Font timessmallN = new iTextSharp.text.Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL);
            iTextSharp.text.Font timessmallbld = new iTextSharp.text.Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD);
            iTextSharp.text.Font timesfntN = new iTextSharp.text.Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL);
            iTextSharp.text.Font timesfntbld = new iTextSharp.text.Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD);

            Phrase ph1 = new Phrase();

            #region header

            PdfPTable table1 = new PdfPTable(5);
            table1.DefaultCell.Padding = 4;
            float[] widths = new float[] { 20f, 10f, 35f, 25f, 10f };
            table1.SetWidths(widths);
            table1.WidthPercentage = 100f;
            table1.SpacingBefore = 10f;

            if (dt_inv.Rows[0]["InvoiceTRN"].ToString() != "")
            {
                ph1 = new Phrase();
                ph1.Add(new Chunk(dt_inv.Rows[0]["InvoiceTRN"].ToString(), timessmallN));
                ph1.Add(new Chunk(": TRN", timessmallN));
                ph1.Add(new Chunk(" رقم التسجيل الضريبي", arbsmallN));

                PdfPCell sub04 = new PdfPCell(ph1);
                sub04.Border = 0;
                sub04.Colspan = 5;
                sub04.HorizontalAlignment = Element.ALIGN_CENTER;
                sub04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                table1.AddCell(sub04);

            }

            ph1 = new Phrase();
            ph1.Add(new Chunk(" : Invoice No ", timessmallbld));
            ph1.Add(new Chunk("رقم الفاتورة", arbsmallN));

            PdfPCell lines = new PdfPCell(ph1);
            lines.Border = 0;
            lines.HorizontalAlignment = Element.ALIGN_LEFT;
            lines.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            table1.AddCell(lines);

            lines = new PdfPCell(new Phrase(dt_inv.Rows[0]["Code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            lines.Border = 0;
            lines.HorizontalAlignment = Element.ALIGN_LEFT;
            table1.AddCell(lines);

            ph1 = new Phrase();
            ph1.Add(new Chunk("فاتورة ضريبية", arbfntN));
            ph1.Add(new Chunk("\n", arbfntbld));
            ph1.Add(new Chunk(" Tax Invoice ", timesfntbld));

            lines = new PdfPCell(ph1);
            lines.Border = 0;
            lines.HorizontalAlignment = Element.ALIGN_CENTER;
            lines.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            table1.AddCell(lines);

            ph1 = new Phrase();
            ph1.Add(new Chunk(" : Invoice Date ", timessmallbld));
            ph1.Add(new Chunk("تاريخ الفاتورة", arbsmallN));

            lines = new PdfPCell(ph1);
            lines.Border = 0;
            lines.HorizontalAlignment = Element.ALIGN_LEFT;
            lines.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            table1.AddCell(lines);

            lines = new PdfPCell(new Phrase(dt_inv.Rows[0]["Dates"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            lines.Border = 0;
            lines.HorizontalAlignment = Element.ALIGN_LEFT;
            table1.AddCell(lines);

            document.Add(table1);

            PdfPTable Subhead = new PdfPTable(3);
            Subhead.DefaultCell.Padding = 4;
            Subhead.SpacingBefore = 15f;
            widths = new float[] { 35f, 25f, 40f };
            Subhead.SetWidths(widths);
            Subhead.WidthPercentage = 100f;

            ph1 = new Phrase();
            ph1.Add(new Chunk(" : Customer ", timessmallbld));
            ph1.Add(new Chunk("اسم العميل", arbsmallN));

            lines = new PdfPCell(ph1);
            lines.MinimumHeight = 25f;
            lines.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER;
            lines.HorizontalAlignment = Element.ALIGN_RIGHT;
            lines.VerticalAlignment = Element.ALIGN_MIDDLE;
            lines.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(lines);

            ph1 = new Phrase();
            ph1.Add(new Chunk(" " + dt_cust.Rows[0]["Name"].ToString() + " ", timessmallN));
            ph1.Add(new Chunk(" " + dt_cust.Rows[0]["ArabicName"].ToString() + " ", arbsmallN));

            lines = new PdfPCell(ph1);
            lines.Colspan = 2;
            lines.MinimumHeight = 25f;
            lines.Border = PdfPCell.TOP_BORDER | PdfPCell.RIGHT_BORDER;
            lines.HorizontalAlignment = Element.ALIGN_RIGHT;
            lines.VerticalAlignment = Element.ALIGN_MIDDLE;
            lines.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(lines);

            if (dt_cust.Rows[0]["Address"].ToString() != "")
            {
                ph1 = new Phrase();
                ph1.Add(new Chunk("Address : ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                ph1.Add(new Chunk(dt_cust.Rows[0]["Addressline"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                lines = new PdfPCell(ph1);
                lines.Colspan = 3;
                lines.MinimumHeight = 25f;
                lines.VerticalAlignment = Element.ALIGN_MIDDLE;
                lines.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(lines);
            }

            ph1 = new Phrase();
            ph1.Add(new Chunk(" : Customer TRN ", timessmallbld));
            ph1.Add(new Chunk("رقم التسجيل الضريبي للعميل", arbsmallN));

            lines = new PdfPCell(ph1);
            lines.MinimumHeight = 25f;
            lines.VerticalAlignment = Element.ALIGN_MIDDLE;
            lines.HorizontalAlignment = Element.ALIGN_RIGHT;
            lines.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER;
            lines.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(lines);

            ph1 = new Phrase();
            ph1.Add(new Chunk(dt_cust.Rows[0]["TRN"].ToString(), timessmallN));

            lines = new PdfPCell(ph1);
            lines.MinimumHeight = 25f;
            lines.Border = 3;
            lines.VerticalAlignment = Element.ALIGN_MIDDLE;
            lines.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(lines);

            ph1 = new Phrase();
            ph1.Add(new Chunk(dt_cust.Rows[0]["Mobile_num"].ToString(), timessmallN));
            ph1.Add(new Chunk(" : Contact No ", timessmallbld));
            ph1.Add(new Chunk("رقم الاتصال", arbsmallN));

            lines = new PdfPCell(ph1);
            lines.MinimumHeight = 25f;
            lines.VerticalAlignment = Element.ALIGN_MIDDLE;
            lines.HorizontalAlignment = Element.ALIGN_RIGHT;
            lines.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(lines);

            document.Add(Subhead);

            #endregion

            #region data

            if (dt_invD.Rows.Count > 0)
            {
                {
                    PdfPTable emp_details = new PdfPTable(8);

                    emp_details.DefaultCell.Padding = 4;
                    float[] widthsdet = new float[] { 4f, 25f, 10f, 5f, 8f, 9f, 9f, 9f };
                    emp_details.SetWidths(widthsdet);

                    emp_details.SpacingBefore = 20f;
                    emp_details.WidthPercentage = 100f;

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("رقم", arbsmallN));
                    ph1.Add(new Chunk("\n", arbsmallN));
                    ph1.Add(new Chunk("No", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    PdfPCell DetailH01 = new PdfPCell(ph1);
                    DetailH01.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH01.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(DetailH01);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("الخدمة", arbsmallN));
                    ph1.Add(new Chunk("\n", arbsmallN));
                    ph1.Add(new Chunk("Service", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    PdfPCell DetailH02 = new PdfPCell(ph1);
                    DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH02.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailH02);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("مقدم الطلب", arbsmallN));
                    ph1.Add(new Chunk("\n", arbsmallN));
                    ph1.Add(new Chunk("Applicant ", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    PdfPCell DetailH03 = new PdfPCell(ph1);
                    DetailH03.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH03.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailH03);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("الكمية", arbsmallN));
                    ph1.Add(new Chunk("\n", arbsmallN));
                    ph1.Add(new Chunk("Qty ", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    DetailH03 = new PdfPCell(ph1);
                    DetailH03.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH03.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailH03);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("رسوم  حكومية", arbsmallN));
                    ph1.Add(new Chunk("\n", arbsmallN));
                    ph1.Add(new Chunk("Govt. Fee", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    PdfPCell DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailH04);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("الخصم", arbsmallN));
                    ph1.Add(new Chunk("\n", arbsmallN));
                    ph1.Add(new Chunk("Discount", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailH04);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("الخدمة مع الضريبة", arbsmallN));
                    ph1.Add(new Chunk("\n", arbsmallN));
                    ph1.Add(new Chunk("Service incl vat", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailH04);

                    //ph1 = new Phrase();
                    //ph1.Add(new Chunk("الضريبة", arbsmallN));
                    //ph1.Add(new Chunk("\n", arbsmallN));
                    //ph1.Add(new Chunk("Tax", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    //DetailH04 = new PdfPCell(ph1);
                    //DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    //DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    //emp_details.AddCell(DetailH04);

                    //ph1 = new Phrase();
                    //ph1.Add(new Chunk("الإجمالي", arbsmallN));
                    //ph1.Add(new Chunk("\n", arbsmallN));
                    //ph1.Add(new Chunk("Total", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    //DetailH04 = new PdfPCell(ph1);
                    //DetailH04.MinimumHeight = 20f;
                    //DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    //DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    //emp_details.AddCell(DetailH04);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("الإجمالي مع الضريبة", arbsmallN));
                    ph1.Add(new Chunk("\n", arbsmallN));
                    ph1.Add(new Chunk("Total incl vat", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.MinimumHeight = 20f;
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailH04);

                    int i = 0;

                    foreach (DataRow rows in dt_invD.Rows)
                    {
                        try
                        {
                            PdfPCell sn = new PdfPCell(new Phrase((++i).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            sn.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            sn.MinimumHeight = 20f;
                            emp_details.AddCell(sn);
                        }
                        catch (Exception ee)
                        {
                            emp_details.AddCell("");
                        }
                        try
                        {

                            PdfPTable intble = new PdfPTable(1);
                            intble.DefaultCell.Border = 0;
                            intble.WidthPercentage = 95f;

                            Phrase phrase14 = new Phrase();
                            phrase14.Add(new Chunk(rows["NameInArabic"].ToString(), arbsmallN));

                            PdfPCell TP = new PdfPCell(phrase14);
                            TP.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                            TP.Border = 0;
                            intble.AddCell(TP);

                            TP = new PdfPCell(new Phrase(rows["Name"].ToString() + "\n", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            TP.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            TP.Border = 0;
                            intble.AddCell(TP);

                            PdfPCell REM = new PdfPCell(intble);
                            //REM.Border = 0;
                            emp_details.AddCell(REM);

                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("");
                        }

                        try
                        {
                            PdfPCell REM = new PdfPCell(new Phrase(rows["ParticularsD"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            REM.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            emp_details.AddCell(REM);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("N/A");
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Quantity"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            emp_details.AddCell(DT);

                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("");
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Expense"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(DT);

                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("");
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Discount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(DT);

                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("");
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase((Convert.ToDecimal(rows["TotalServiceCharge"]) + Convert.ToDecimal(rows["SingleTaxAmount"]) - Convert.ToDecimal(rows["Discount"])).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(DT);

                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("");
                        }

                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(DT);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("");
                        }
                    }

                    ph1 = new Phrase();
                    ph1.Add(new Chunk(" Total ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("الإجمالي", arbsmallN));
                    PdfPCell summ = new PdfPCell(ph1);
                    summ.MinimumHeight = 20f;
                    summ.HorizontalAlignment = Element.ALIGN_CENTER;
                    summ.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    summ.Colspan = 4;
                    emp_details.AddCell(summ);

                    summ = new PdfPCell(new Phrase(dt_sum.Rows[0]["TExpense"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    summ.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(summ);
                    summ = new PdfPCell(new Phrase(dt_sum.Rows[0]["TDiscount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    summ.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(summ);
                    //summ = new PdfPCell(new Phrase((Convert.ToDecimal(dt_sum.Rows[0]["TSC"]) + Convert.ToDecimal(dt_sum.Rows[0]["TTAx"])).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //summ.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //emp_details.AddCell(summ);
                    summ = new PdfPCell(new Phrase((Convert.ToDecimal(dt_sum.Rows[0]["TotalNoRound"]) - Convert.ToDecimal(dt_sum.Rows[0]["TExpense"])).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    summ.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(summ);
                    summ = new PdfPCell(new Phrase(dt_sum.Rows[0]["TotalNoRound"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    summ.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(summ);

                    document.Add(emp_details);

                    PdfPTable totalexp = new PdfPTable(3);
                    totalexp.DefaultCell.Padding = 4;
                    float[] widths1 = new float[] { 50f, 35f, 15f };
                    totalexp.SetWidths(widths1);
                    totalexp.SpacingBefore = 10;
                    totalexp.WidthPercentage = 100f;

                    //PdfPCell tot2ww = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //tot2ww.Border = 0;
                    //tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //totalexp.AddCell(tot2ww);

                    //ph1 = new Phrase();
                    //ph1.Add(new Chunk(" Grand Total ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //ph1.Add(new Chunk("الإجمالي الكلي", arbsmallN));
                    //tot2ww = new PdfPCell(ph1);
                    //tot2ww.MinimumHeight = 20f;
                    //tot2ww.HorizontalAlignment = Element.ALIGN_LEFT;
                    //tot2ww.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    //totalexp.AddCell(tot2ww);

                    //tot2ww = new PdfPCell(new Phrase(dt_sum.Rows[0]["GrandTotal"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //totalexp.AddCell(tot2ww);

                    PdfPCell tot2ww = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    tot2ww.Border = 0;
                    tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot2ww);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk(" Total Before Vat ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("الإجمالي قبل الضريبة  ", arbsmallN));
                    tot2ww = new PdfPCell(ph1);
                    tot2ww.MinimumHeight = 20f;
                    tot2ww.HorizontalAlignment = Element.ALIGN_LEFT;
                    tot2ww.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    totalexp.AddCell(tot2ww);

                    tot2ww = new PdfPCell(new Phrase((Convert.ToDecimal(dt_sum.Rows[0]["Total"]) - Convert.ToDecimal(dt_sum.Rows[0]["TTAx"])).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot2ww);

                    tot2ww = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    tot2ww.Border = 0;
                    tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot2ww);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk(" Vat ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("الضريبة", arbsmallN));
                    tot2ww = new PdfPCell(ph1);
                    tot2ww.MinimumHeight = 20f;
                    tot2ww.HorizontalAlignment = Element.ALIGN_LEFT;
                    tot2ww.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    totalexp.AddCell(tot2ww);

                    tot2ww = new PdfPCell(new Phrase(dt_sum.Rows[0]["TTAx"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot2ww);

                    tot2ww = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    tot2ww.Border = 0;
                    tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot2ww);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk(" Total incl Vat ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("الإجمالي مع الضريبة", arbsmallN));
                    tot2ww = new PdfPCell(ph1);
                    tot2ww.MinimumHeight = 20f;
                    tot2ww.HorizontalAlignment = Element.ALIGN_LEFT;
                    tot2ww.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    totalexp.AddCell(tot2ww);

                    tot2ww = new PdfPCell(new Phrase(dt_sum.Rows[0]["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot2ww);

                    tot2ww = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    tot2ww.Border = 0;
                    tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot2ww);

                    ph1 = new Phrase();

                    ph1.Add(new Chunk(" Rounded off ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("المبلغ التقريبي ", arbsmallN));
                    tot2ww = new PdfPCell(ph1);
                    tot2ww.MinimumHeight = 20f;
                    tot2ww.HorizontalAlignment = Element.ALIGN_LEFT;
                    tot2ww.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    totalexp.AddCell(tot2ww);

                    //tot2ww = new PdfPCell(new Phrase("Rounded off", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //tot2ww.MinimumHeight = 20f;
                    //totalexp.AddCell(tot2ww);
                    tot2ww = new PdfPCell(new Phrase(dt_inv.Rows[0]["RoundedOff"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot2ww);

                    //tot2ww = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //tot2ww.Border = 0;
                    //tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //totalexp.AddCell(tot2ww);

                    //ph1 = new Phrase();

                    //ph1.Add(new Chunk(" Discount Allowed ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //ph1.Add(new Chunk("الخصم المسموح به", arbsmallN));
                    //tot2ww = new PdfPCell(ph1);
                    //tot2ww.MinimumHeight = 20f;
                    //tot2ww.HorizontalAlignment = Element.ALIGN_LEFT;
                    //tot2ww.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    //totalexp.AddCell(tot2ww);

                    //tot2ww = new PdfPCell(new Phrase(dt_sum.Rows[0]["TDiscount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //totalexp.AddCell(tot2ww);

                    //tot2ww = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //tot2ww.Border = 0;
                    //tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //totalexp.AddCell(tot2ww);

                    //ph1 = new Phrase();

                    //ph1.Add(new Chunk(" Net Amount ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //ph1.Add(new Chunk("المبلغ الصافي ", arbsmallN));
                    //tot2ww = new PdfPCell(ph1);
                    //tot2ww.MinimumHeight = 20f;
                    //tot2ww.HorizontalAlignment = Element.ALIGN_LEFT;
                    //tot2ww.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    //totalexp.AddCell(tot2ww);

                    //tot2ww = new PdfPCell(new Phrase(dt_sum.Rows[0]["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //totalexp.AddCell(tot2ww);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("المفوض بالتوقيع", arbsmallN));
                    ph1.Add(new Chunk("\n", arbsmallN));
                    ph1.Add(new Chunk("Authorised Signatory", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    tot2ww = new PdfPCell(ph1);
                    tot2ww.MinimumHeight = 20f;
                    tot2ww.Border = 0;
                    tot2ww.HorizontalAlignment = Element.ALIGN_CENTER;
                    tot2ww.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    totalexp.AddCell(tot2ww);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk(" Paid Amount ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("المبلغ المدفوع", arbsmallN));
                    tot2ww = new PdfPCell(ph1);
                    tot2ww.MinimumHeight = 20f;
                    tot2ww.HorizontalAlignment = Element.ALIGN_LEFT;
                    tot2ww.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    totalexp.AddCell(tot2ww);

                    //tot2ww = new PdfPCell(new Phrase("Paid Amount", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //tot2ww.MinimumHeight = 20f;
                    //totalexp.AddCell(tot2ww);
                    tot2ww = new PdfPCell(new Phrase(dt_inv.Rows[0]["Received"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot2ww);

                    if (dtGeneral.Rows[0]["IsAddCreatedByInInvoicePrint"].ToString() == "1")
                    {
                        ph1 = new Phrase();
                        ph1.Add(new Chunk(" Created by : ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                        ph1.Add(new Chunk(dt_inv.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                        ph1.Add(new Chunk("كتب بواسطة", arbsmallN));
                        tot2ww = new PdfPCell(ph1);
                        tot2ww.MinimumHeight = 20f;
                        tot2ww.Border = 0;
                        tot2ww.HorizontalAlignment = Element.ALIGN_CENTER;
                        tot2ww.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                        totalexp.AddCell(tot2ww);
                    }
                    else
                    {
                        ph1 = new Phrase();
                        ph1.Add(new Chunk(" ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                        tot2ww = new PdfPCell(ph1);
                        tot2ww.MinimumHeight = 20f;
                        tot2ww.Border = 0;
                        tot2ww.HorizontalAlignment = Element.ALIGN_CENTER;
                        tot2ww.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                        totalexp.AddCell(tot2ww);
                    }

                    ph1 = new Phrase();
                    ph1.Add(new Chunk(" Balance ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("الرصيد", arbsmallN));
                    tot2ww = new PdfPCell(ph1);
                    tot2ww.MinimumHeight = 20f;
                    tot2ww.HorizontalAlignment = Element.ALIGN_LEFT;
                    tot2ww.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    totalexp.AddCell(tot2ww);

                    //tot2ww = new PdfPCell(new Phrase("Balance", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //tot2ww.MinimumHeight = 20f;
                    //totalexp.AddCell(tot2ww);
                    tot2ww = new PdfPCell(new Phrase(dt_inv.Rows[0]["Receivable"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot2ww);

                    PdfPCell tot1 = new PdfPCell(new Phrase("AED " + ConvertNumbertoWords(Convert.ToDecimal(dt_sum.Rows[0]["Total"])), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    tot1.Border = 0;
                    tot1.Colspan = 3;
                    tot1.MinimumHeight = 25f;
                    tot1.HorizontalAlignment = Element.ALIGN_CENTER;
                    totalexp.AddCell(tot1);

                    document.Add(totalexp);

                }

            }


            #endregion

            #region Footer

            if (dtGeneral.Rows[0]["PrintFooter"].ToString() != "")
            {
                PdfPTable footer = new PdfPTable(1);
                footer.DefaultCell.Padding = 4;
                //footer.SpacingAfter = 20f;
                footer.TotalWidth = document.PageSize.Width - (2 * document.LeftMargin);
                footer.WidthPercentage = 90f;

                string imageURL = Server.MapPath("../UploadedImage/" + dtGeneral.Rows[0]["PrintFooter"].ToString());
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(new Uri(imageURL));
                jpg.ScaleToFit(470f, 450f);

                PdfPCell Fotservice = new PdfPCell(jpg, true);
                Fotservice.Border = 0;
                Fotservice.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                footer.AddCell(Fotservice);

                if (dtGeneral.Rows[0]["IsSoftareNameAdd"].ToString() == "1")
                {
                    PdfPCell Fotname = new PdfPCell(new Phrase("Software from www.almasit.ae", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    Fotname.Border = 0;
                    Fotname.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    footer.AddCell(Fotname);
                }
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

            else if (dtGeneral.Rows[0]["IsSoftareNameAdd"].ToString() == "1")
            {

                PdfPTable footer = new PdfPTable(1);
                footer.DefaultCell.Padding = 4;
                footer.SpacingAfter = 20f;
                footer.TotalWidth = document.PageSize.Width - (2 * document.LeftMargin);
                footer.WidthPercentage = 90f;

                PdfPCell Fotservice = new PdfPCell(new Phrase("Software from www.almasit.ae", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                Fotservice.Border = 0;
                Fotservice.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                footer.AddCell(Fotservice);

                float curY = 0;
                curY = writer.GetVerticalPosition(true);
                float jhgt = 30;

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