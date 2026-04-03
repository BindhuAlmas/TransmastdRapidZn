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
    public partial class TaxInvoiceFormat10 : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();
        Master_Bal obj_master = new Master_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"]);
            DataSet ds = obj_report.TaxInvoicePrint(id);
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
            BaseFont bfTimesV0 = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Font/arabtypeV0.ttf"), BaseFont.IDENTITY_H, true);

            //iTextSharp.text.Font arbfnt = new iTextSharp.text.Font(bfTimes, 9, Font.NORMAL);
            iTextSharp.text.Font arbfnt = new iTextSharp.text.Font(bfTimesV0, 9, Font.NORMAL);
            iTextSharp.text.Font arbsmallbold = new iTextSharp.text.Font(bfTimes, 11, Font.NORMAL);
            iTextSharp.text.Font arbfntbld = new iTextSharp.text.Font(bfTimesV0, 16, Font.NORMAL);
            iTextSharp.text.Font arbfntbldN = new iTextSharp.text.Font(bfTimes, 14, Font.NORMAL);

            #region header

            PdfPTable table1 = new PdfPTable(1);
            table1.DefaultCell.Padding = 4;

            Phrase ph1 = new Phrase();
            ph1.Add(new Chunk(dt_inv.Rows[0]["InvoiceType"].ToString() == "1" ? "Tax Invoice\n" : "Invoice\n", new Font(Font.FontFamily.UNDEFINED, 14, Font.BOLD)));
            if (dt_inv.Rows[0]["InvoiceType"].ToString() == "1")
                ph1.Add(new Chunk("فاتورة ضريبية ", arbfntbldN));
            else if (dt_inv.Rows[0]["InvoiceType"].ToString() == "2")
                ph1.Add(new Chunk("فاتورة  ", arbfntbldN));

            PdfPCell sub04 = new PdfPCell(ph1);
            //sub04.MinimumHeight = 40f;
            sub04.Border = 0;
            sub04.HorizontalAlignment = Element.ALIGN_CENTER;
            sub04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            table1.AddCell(sub04);

            System.IO.MemoryStream mem = new MemoryStream();
            Barcode128 barImg = new Barcode128();
            barImg.Code = dt_inv.Rows[0]["Code"].ToString();
            barImg.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White).Save(mem, System.Drawing.Imaging.ImageFormat.Png);
            iTextSharp.text.Image imgs = iTextSharp.text.Image.GetInstance(mem.ToArray());
            mem.Flush();
            mem.Close();

            PdfPCell Cuscell22 = new PdfPCell(imgs);
            Cuscell22.Border = 0;
            Cuscell22.MinimumHeight = 50f;
            Cuscell22.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            Cuscell22.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            table1.AddCell(Cuscell22);

            document.Add(table1);

            PdfPTable Subhead = new PdfPTable(7);
            Subhead.DefaultCell.Padding = 4;
            float[] widths = new float[] { 21f,5f, 25f, 38f, 15f,5f, 25f };
            Subhead.SetWidths(widths);
            Subhead.WidthPercentage = 100f;

            // empty cell

            PdfPCell Empty = new PdfPCell(new Phrase("", new Font(Font.FontFamily.UNDEFINED, 11, Font.NORMAL)));
            Empty.Border = 0;
            Empty.HorizontalAlignment = Element.ALIGN_LEFT;

            //end

            PdfPCell sub1 = new PdfPCell(new Phrase("Customer ", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            sub1.Border = 0;
            sub1.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub1);
            sub1 = new PdfPCell(new Phrase(":", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            sub1.Border = 0;
            sub1.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub1);

            PdfPCell sub12 = new PdfPCell(new Phrase(dt_cust.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
            sub12.Border = 0;
            sub12.Colspan = 2;
            sub12.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub12);

            PdfPCell sub13 = new PdfPCell(new Phrase("Invoice No ", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            sub13.Border = 0;
            sub13.HorizontalAlignment = Element.ALIGN_RIGHT;
            Subhead.AddCell(sub13);
            sub1 = new PdfPCell(new Phrase(":", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            sub1.Border = 0;
            sub1.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub1);


            PdfPCell sub14 = new PdfPCell(new Phrase(dt_inv.Rows[0]["Code"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
            sub14.Border = 0;
            sub14.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub14);

            PdfPCell sub21 = new PdfPCell(new Phrase("Contact ", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            sub21.Border = 0;
            sub21.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub21);
            sub1 = new PdfPCell(new Phrase(":", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            sub1.Border = 0;
            sub1.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub1);

            PdfPCell sub122 = new PdfPCell(new Phrase(dt_cust.Rows[0]["Mobile_num"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
            sub122.Border = 0;
            sub122.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub122);

            Subhead.AddCell(Empty);

            PdfPCell sub132 = new PdfPCell(new Phrase("Date ", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            sub132.Border = 0;
            sub132.HorizontalAlignment = Element.ALIGN_RIGHT;
            Subhead.AddCell(sub132);
            sub1 = new PdfPCell(new Phrase(":", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            sub1.Border = 0;
            sub1.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub1);

            PdfPCell sub142 = new PdfPCell(new Phrase(dt_inv.Rows[0]["Dates"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
            sub142.Border = 0;
            sub142.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub142);

            PdfPCell sub21add = new PdfPCell(new Phrase("Customer TRN ", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            sub21add.Border = 0;
            sub21add.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub21add);
            sub1 = new PdfPCell(new Phrase(":", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            sub1.Border = 0;
            sub1.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub1);

            PdfPCell sub122add = new PdfPCell(new Phrase(dt_cust.Rows[0]["TRN"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
            sub122add.Border = 0;
            sub122add.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub122add);

            Subhead.AddCell(Empty);

            if (dt_inv.Rows[0]["InvoiceTRN"].ToString() == "")
            {
                sub122add = new PdfPCell(new Phrase("", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                sub122add.Border = 0;
                sub122add.Colspan = 3;
                sub122add.HorizontalAlignment = Element.ALIGN_RIGHT;
                Subhead.AddCell(sub122add);
            }
            else
            {
                sub122add = new PdfPCell(new Phrase("TRN  ", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                sub122add.Border = 0;
                sub122add.HorizontalAlignment = Element.ALIGN_RIGHT;
                Subhead.AddCell(sub122add);
                sub1 = new PdfPCell(new Phrase(":", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                sub1.Border = 0;
                sub1.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub1);

                sub142 = new PdfPCell(new Phrase(dt_inv.Rows[0]["InvoiceTRN"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
                sub142.Border = 0;
                sub142.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub142);
            }

            sub21add = new PdfPCell(new Phrase("Address ", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            sub21add.Border = 0;
            sub21add.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub21add);
            sub1 = new PdfPCell(new Phrase(":", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            sub1.Border = 0;
            sub1.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub1);

            sub122add = new PdfPCell(new Phrase(dt_cust.Rows[0]["Address"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
            sub122add.Border = 0;
            sub122add.Colspan = 5;
            sub122add.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub122add);

            sub1 = new PdfPCell(new Phrase("Prepared By ", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            sub1.Border = 0;
            sub1.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub1);
            sub1 = new PdfPCell(new Phrase(":", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            sub1.Border = 0;
            sub1.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub1);

            sub12 = new PdfPCell(new Phrase(dt_inv.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
            sub12.Border = 0;
            sub12.Colspan = 5;
            sub12.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub12);

            if (dt_inv.Rows[0]["Subject"].ToString() != "")
            {
                sub1 = new PdfPCell(new Phrase("Subject ", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                sub1.Border = 0;
                sub1.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub1);
                sub1 = new PdfPCell(new Phrase(":", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                sub1.Border = 0;
                sub1.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub1);

                sub12 = new PdfPCell(new Phrase(dt_inv.Rows[0]["Subject"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
                sub12.Border = 0;
                sub12.Colspan = 5;
                sub12.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub12);
            }


            document.Add(Subhead);

            #endregion

            #region data

            if (dt_invD.Rows.Count > 0)
            {
                /*Tax Invoice Type*/
                //if (dt_inv.Rows[0]["InvoiceType"].ToString() == "1")
                //{
                PdfPTable emp_details = new PdfPTable(6);

                emp_details.DefaultCell.Padding = 4;
                float[] widthsdet = new float[] { 4f, 25f, 10f, 8f,  5f, 9f };
                emp_details.SetWidths(widthsdet);

                emp_details.SpacingBefore = 20f;
                emp_details.WidthPercentage = 100f;

                ph1 = new Phrase();
                ph1.Add(new Chunk("No\n", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                ph1.Add(new Chunk("رقم", arbsmallbold));
                PdfPCell DetailH01 = new PdfPCell(ph1);
                DetailH01.HorizontalAlignment = Element.ALIGN_CENTER;
                DetailH01.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                emp_details.AddCell(DetailH01);
                ph1 = new Phrase();
                ph1.Add(new Chunk("Service\n", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                ph1.Add(new Chunk("خدمات", arbsmallbold));
                PdfPCell DetailH02 = new PdfPCell(ph1);
                DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                DetailH02.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                emp_details.AddCell(DetailH02);
               
                ph1 = new Phrase();
                ph1.Add(new Chunk("Applicant Name\n", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                ph1.Add(new Chunk("مقدم الطلب", arbsmallbold));
                PdfPCell DetailH03 = new PdfPCell(ph1);
                DetailH03.HorizontalAlignment = Element.ALIGN_CENTER;
                DetailH03.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                emp_details.AddCell(DetailH03);
                ph1 = new Phrase();
                ph1.Add(new Chunk("Price\n", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                ph1.Add(new Chunk("سعر", arbsmallbold));
                PdfPCell DetailH04 = new PdfPCell(ph1);
                DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                emp_details.AddCell(DetailH04);

                //ph1 = new Phrase();
                //ph1.Add(new Chunk("Fine\n", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                //ph1.Add(new Chunk("غرامة", arbsmallbold));
                //DetailH03 = new PdfPCell(ph1);
                //DetailH03.HorizontalAlignment = Element.ALIGN_CENTER;
                //DetailH03.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                //emp_details.AddCell(DetailH03);
               
                ph1 = new Phrase();
                ph1.Add(new Chunk("VAT\n", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                ph1.Add(new Chunk("ضريبة", arbsmallbold));
                DetailH04 = new PdfPCell(ph1);
                DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                emp_details.AddCell(DetailH04);
                ph1 = new Phrase();
                ph1.Add(new Chunk("Total\n", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                ph1.Add(new Chunk(" اجمالي", arbsmallbold));
                DetailH04 = new PdfPCell(ph1);
                DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                emp_details.AddCell(DetailH04);

                int i = 0;
                decimal P = 0, T = 0, Tot = 0, F = 0;

                foreach (DataRow rows in dt_invD.Rows)
                {
                    try
                    {
                        PdfPCell sn = new PdfPCell(new Phrase((++i).ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
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

                        PdfPCell TP = new PdfPCell(new Phrase(rows["Name"].ToString() + "\n", new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
                        TP.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        TP.Border = 0;
                        intble.AddCell(TP);

                        TP = new PdfPCell(new Phrase(rows["NameInArabic"].ToString(), arbfnt));
                        TP.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                        TP.Border = 0;
                        intble.AddCell(TP);

                        emp_details.AddCell(intble);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }

                    try
                    {
                        PdfPCell REM = new PdfPCell(new Phrase(rows["ParticularsD"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
                        REM.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        emp_details.AddCell(REM);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("N/A");
                    }
                  
                    try
                    {
                        PdfPCell DT = new PdfPCell(new Phrase(rows["Amount"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
                        DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        emp_details.AddCell(DT);
                        P = P + Convert.ToDecimal(rows["Amount"]);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    //try
                    //{
                    //    PdfPCell DT = new PdfPCell(new Phrase(rows["Fine"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
                    //    DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    //    emp_details.AddCell(DT);
                    //    if (rows["Fine"].ToString() != "")
                    //        F = F + Convert.ToDecimal(rows["Fine"]);
                    //}
                    //catch (Exception eee)
                    //{
                    //    emp_details.AddCell("");
                    //}
                    try
                    {
                        PdfPCell DT = new PdfPCell(new Phrase(rows["TaxAmount"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
                        DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        emp_details.AddCell(DT);
                        T = T + Convert.ToDecimal(rows["TaxAmount"]);

                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell DT = new PdfPCell(new Phrase(rows["Total"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
                        DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        emp_details.AddCell(DT);
                        Tot = Tot + Convert.ToDecimal(rows["Total"]);

                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                }

                PdfPCell totw1wf = new PdfPCell(new Phrase("Total ", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                totw1wf.HorizontalAlignment = Element.ALIGN_RIGHT;
                totw1wf.Colspan = 3;
                emp_details.AddCell(totw1wf);

                totw1wf = new PdfPCell(new Phrase(P.ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                totw1wf.HorizontalAlignment = Element.ALIGN_RIGHT;
                emp_details.AddCell(totw1wf);
                //totw1wf = new PdfPCell(new Phrase(F.ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                //totw1wf.HorizontalAlignment = Element.ALIGN_RIGHT;
                //emp_details.AddCell(totw1wf);
                totw1wf = new PdfPCell(new Phrase(T.ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                totw1wf.HorizontalAlignment = Element.ALIGN_RIGHT;
                emp_details.AddCell(totw1wf);

                totw1wf = new PdfPCell(new Phrase(Tot.ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                totw1wf.HorizontalAlignment = Element.ALIGN_RIGHT;
                emp_details.AddCell(totw1wf);

                document.Add(emp_details);

                PdfPTable totalexp = new PdfPTable(4);
                totalexp.DefaultCell.Padding = 4;
                float[] widths1 = new float[] { 55f, 20f, 10f, 13f };
                totalexp.SetWidths(widths1);
                totalexp.SpacingBefore = 10;
                totalexp.WidthPercentage = 100f;

                totalexp.AddCell(Empty);

                PdfPCell totw1w = new PdfPCell(new Phrase("Grand Total :", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                totw1w.Border = 0;
                totw1w.HorizontalAlignment = Element.ALIGN_LEFT;
                totalexp.AddCell(totw1w);

                PdfPCell tot1 = new PdfPCell(new Phrase("AED", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                tot1.Border = 0;
                tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot1);

                PdfPCell tot2ww = new PdfPCell(new Phrase(dt_sum.Rows[0]["Total"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                tot2ww.Border = 0;
                tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot2ww);

                decimal GrandtotalFinal = Convert.ToDecimal(dt_sum.Rows[0]["Total"].ToString());

                if (dt_inv.Rows[0]["ChargedAmount"].ToString() != "0.00")
                {
                    GrandtotalFinal = GrandtotalFinal + Convert.ToDecimal(dt_inv.Rows[0]["ChargedAmount"].ToString());
                    totalexp.AddCell(Empty);

                    PdfPCell totw1wcc = new PdfPCell(new Phrase("Charged Amount :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    totw1wcc.Border = 0;
                    totw1wcc.HorizontalAlignment = Element.ALIGN_LEFT;
                    totalexp.AddCell(totw1wcc);
                    tot1 = new PdfPCell(new Phrase("AED", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                    tot1.Border = 0;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot1);

                    totw1wcc = new PdfPCell(new Phrase(dt_inv.Rows[0]["ChargedAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    totw1wcc.Border = 0;
                    totw1wcc.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(totw1wcc);

                    totalexp.AddCell(Empty);

                    totw1wcc = new PdfPCell(new Phrase("Total :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    totw1wcc.Border = 0;
                    totw1wcc.HorizontalAlignment = Element.ALIGN_LEFT;
                    totalexp.AddCell(totw1wcc);
                    tot1 = new PdfPCell(new Phrase("AED", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                    tot1.Border = 0;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot1);

                    totw1wcc = new PdfPCell(new Phrase(GrandtotalFinal.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    totw1wcc.Border = 0;
                    totw1wcc.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(totw1wcc);
                }

                PdfPCell totw1wpsss = new PdfPCell(new Phrase("AED " + ConvertNumbertoWords(GrandtotalFinal) + " Only", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLDITALIC)));
                totw1wpsss.Border = 0;
                totw1wpsss.Colspan = 4;
                totw1wpsss.HorizontalAlignment = Element.ALIGN_LEFT;
                totalexp.AddCell(totw1wpsss);

                document.Add(totalexp);

            }

            #region Remark

            PdfPTable terms = new PdfPTable(3);
            terms.DefaultCell.Padding = 4;
            float[] widthsTerms = new float[] { 50f, 25f, 25f };
            terms.SetWidths(widthsTerms);
            terms.SpacingBefore = 10;
            terms.WidthPercentage = 100f;

            PdfPCell EmptyTerms = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            EmptyTerms.HorizontalAlignment = Element.ALIGN_LEFT;

            if (dtGeneral.Rows[0]["IsAddRemark"].ToString() != "0")
            {
                ph1 = new Phrase();
                ph1.Add(new Chunk(" : ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                ph1.Add(new Chunk("ملاحظات", arbfnt));
                ph1.Add(new Chunk(" / Remarks", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));

                PdfPCell remarksCell = new PdfPCell(ph1);
                remarksCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                remarksCell.Border = 0;
                remarksCell.Colspan = 3;
                remarksCell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                terms.AddCell(remarksCell);

                ph1 = new Phrase();
                ph1.Add(new Chunk(dt_inv.Rows[0]["Remarks"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));

                remarksCell = new PdfPCell(ph1);
                remarksCell.HorizontalAlignment = Element.ALIGN_LEFT;
                remarksCell.Border = 0;
                remarksCell.Colspan = 3;
                //remarksCell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                terms.AddCell(remarksCell);
            }

            /*End of Row*/

            EmptyTerms = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            EmptyTerms.Border = PdfPCell.BOTTOM_BORDER;
            EmptyTerms.HorizontalAlignment = Element.ALIGN_LEFT;
            EmptyTerms.Colspan = 3;
            terms.AddCell(EmptyTerms);
            /*End of Row*/
            if (dtGeneral.Rows[0]["IsAddCreatedByInInvoicePrint"].ToString() == "1")
            {
                EmptyTerms = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                EmptyTerms.Border = 0;
                EmptyTerms.HorizontalAlignment = Element.ALIGN_LEFT;
                terms.AddCell(EmptyTerms);

                ph1 = new Phrase();
                ph1.Add(new Chunk(" :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                ph1.Add(new Chunk("صنع من قبل", arbfnt));
                ph1.Add(new Chunk(" / Created By", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                PdfPCell termsCells = new PdfPCell(ph1);
                termsCells.Border = 0;
                termsCells.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                termsCells.HorizontalAlignment = Element.ALIGN_RIGHT;
                terms.AddCell(termsCells);
                termsCells = new PdfPCell(new Phrase(dt_inv.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                termsCells.Border = 0;
                termsCells.HorizontalAlignment = Element.ALIGN_LEFT;
                terms.AddCell(termsCells);
            }

            EmptyTerms = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            EmptyTerms.Border = 0;
            EmptyTerms.Colspan = 3;
            EmptyTerms.MinimumHeight = 10f;
            EmptyTerms.HorizontalAlignment = Element.ALIGN_LEFT;
            terms.AddCell(EmptyTerms);

            PdfPCell termsCell = new PdfPCell(new Phrase("This document is computer generated and does not require signature or stamp inorder to be considered valid.", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            termsCell.Border = 0;
            termsCell.Colspan = 3;
            termsCell.HorizontalAlignment = Element.ALIGN_LEFT;
            terms.AddCell(termsCell);

            document.Add(terms);

            #endregion

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
                    PdfPCell Fotname = new PdfPCell(new Phrase("Software from www.almasit.ae", new Font(Font.FontFamily.UNDEFINED, 9, Font.NORMAL)));
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

                PdfPCell Fotservice = new PdfPCell(new Phrase("Software from www.almasit.ae", new Font(Font.FontFamily.UNDEFINED, 9, Font.NORMAL)));
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