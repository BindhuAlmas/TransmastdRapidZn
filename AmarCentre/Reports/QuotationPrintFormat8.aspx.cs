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
using System.Threading;
using System.Net;
namespace AmarCentre.Reports
{
    public partial class QuotationPrintFormat8 : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"]);
            DataSet ds = obj_report.QuotationPrintFormat8(id);
            DataTable dtBasic = ds.Tables[0];
            DataTable dtdetail = ds.Tables[1];
            DataTable dt_cust = ds.Tables[2];
            DataTable dt_sum = ds.Tables[3];
            DataTable dtGen = ds.Tables[4];

            string filname = dtBasic.Rows[0]["Code"].ToString();

            Document document = new Document(PageSize.A4, 20f, 20f, 5f, 0f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=" + filname + ".pdf");
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

            BaseFont Calibrifnt = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Font/Calibri.ttf"), BaseFont.IDENTITY_H, true);
            BaseFont Cambriafnt = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Font/Cambria Regular.ttf"), BaseFont.IDENTITY_H, true);
            BaseFont Robotofnt = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Font/Roboto Bold.ttf"), BaseFont.IDENTITY_H, true);
            BaseFont Cambriafntbd = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Font/Cambria Bold.ttf"), BaseFont.IDENTITY_H, true);

            iTextSharp.text.Font Calibri9 = new iTextSharp.text.Font(Calibrifnt, 9, Font.NORMAL);
            iTextSharp.text.Font Calibri10 = new iTextSharp.text.Font(Calibrifnt, 10, Font.NORMAL);
            iTextSharp.text.Font Calibri10bd = new iTextSharp.text.Font(Calibrifnt, 10, Font.BOLD);
            iTextSharp.text.Font Calibri14 = new iTextSharp.text.Font(Calibrifnt, 14, Font.NORMAL);
            iTextSharp.text.Font Calibri10wt = new iTextSharp.text.Font(Calibrifnt, 10, Font.NORMAL, BaseColor.WHITE);
            iTextSharp.text.Font Calibri20wt = new iTextSharp.text.Font(Calibrifnt, 20, Font.NORMAL, BaseColor.WHITE);
            iTextSharp.text.Font Calibri10rd = new iTextSharp.text.Font(Calibrifnt, 10, Font.NORMAL, BaseColor.RED);

            iTextSharp.text.Font Cambria9 = new iTextSharp.text.Font(Cambriafnt, 9, Font.NORMAL);
            iTextSharp.text.Font Cambria10 = new iTextSharp.text.Font(Cambriafnt, 10, Font.NORMAL);

            iTextSharp.text.Font Cambria9bd = new iTextSharp.text.Font(Cambriafntbd, 9, Font.NORMAL);

            iTextSharp.text.Font Roboto10 = new iTextSharp.text.Font(Robotofnt, 10, Font.NORMAL);
            iTextSharp.text.Font Roboto14 = new iTextSharp.text.Font(Robotofnt, 14, Font.BOLD, BaseColor.WHITE);

            iTextSharp.text.BaseColor Blackbg = new iTextSharp.text.BaseColor(1, 36, 58);
            iTextSharp.text.BaseColor whitebg = new iTextSharp.text.BaseColor(System.Drawing.Color.White);
            iTextSharp.text.BaseColor LightGray = new iTextSharp.text.BaseColor(245, 245, 245);

            #region header

            PdfPTable Subhead = new PdfPTable(3);
            Subhead.DefaultCell.Padding = 4;
            float[] widths = new float[] { 35f, 30f, 35f };
            Subhead.SetWidths(widths);
            Subhead.WidthPercentage = 95f;

            PdfPCell Empty = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            Empty.Border = 0;
            Empty.HorizontalAlignment = Element.ALIGN_LEFT;

            PdfPCell EmptyWithTopBorder = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            EmptyWithTopBorder.Border = PdfPCell.TOP_BORDER;
            EmptyWithTopBorder.HorizontalAlignment = Element.ALIGN_LEFT;

            Phrase ph1 = new Phrase();
            ph1.Add(new Chunk("QUOTATION DATE: ", new Font(Cambria9)));
            ph1.Add(new Chunk(dtBasic.Rows[0]["Dates"].ToString(), new Font(Cambria9bd)));
            PdfPCell sub00 = new PdfPCell(new Phrase(ph1));
            sub00.Border = 0;
            sub00.Colspan = 3;
            sub00.Padding = 5f;
            sub00.MinimumHeight = 20f;
            sub00.HorizontalAlignment = Element.ALIGN_RIGHT;
            Subhead.AddCell(sub00);

            ph1 = new Phrase();
            ph1.Add(new Chunk(dtGen.Rows[0]["CompanyName"].ToString(), new Font(Cambria10)));
            //"\n\nTRN : " + dtBasic.Rows[0]["InvoiceTRN"].ToString() + "\n", new Font(Cambria10)));
            sub00 = new PdfPCell(new Phrase(ph1));
            sub00.Border = 0;
            sub00.MinimumHeight = 30f;
            sub00.Padding = 5f;
            sub00.BackgroundColor = whitebg;
            Subhead.AddCell(sub00);

            PdfPTable intble = new PdfPTable(1);
            intble.DefaultCell.Border = 0;
            intble.WidthPercentage = 95f;

            PdfPCell TP = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD, whitebg)));
            TP.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            TP.Border = 0;
            TP.MinimumHeight = 15f;
            intble.AddCell(TP);

            TP = new PdfPCell(new Phrase("QUOTATION", new Font(Roboto14)));
            TP.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            TP.Border = 0;
            TP.MinimumHeight = 30f;
            TP.HorizontalAlignment = Element.ALIGN_CENTER;
            TP.VerticalAlignment = Element.ALIGN_MIDDLE;
            TP.BackgroundColor = Blackbg;
            intble.AddCell(TP);

            TP = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD, whitebg)));
            TP.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            TP.Border = 0;
            TP.MinimumHeight = 15f;
            intble.AddCell(TP);

            PdfPCell REM = new PdfPCell(intble);
            REM.Border = 0;
            REM.Padding = 15f;
            Subhead.AddCell(REM);

            ph1 = new Phrase();
            ph1.Add(new Chunk("BUYER\n\n", new Font(Cambria9bd)));
            ph1.Add(new Chunk(dt_cust.Rows[0]["Name"].ToString() + (dt_cust.Rows[0]["Address"].ToString() != "" ? ("\n\n" + dt_cust.Rows[0]["Address"].ToString()) : "") 
              , new Font(Cambria10)));

            // (dt_cust.Rows[0]["TRN"].ToString()!=""?( "\n\nTRN : " + dt_cust.Rows[0]["TRN"].ToString()):"")
            sub00 = new PdfPCell(new Phrase(ph1));
            sub00.Border = 0;
            sub00.MinimumHeight = 30f;
            sub00.Padding = 5f;
            sub00.BackgroundColor = whitebg;
            Subhead.AddCell(sub00);


            ph1 = new Phrase();
            ph1.Add(new Chunk("QUOTATION NO : ", new Font(Cambria9)));
            ph1.Add(new Chunk(dtBasic.Rows[0]["Code"].ToString(), new Font(Cambria9bd)));
            sub00 = new PdfPCell(new Phrase(ph1));
            sub00.Border = 0;
            sub00.Colspan = 3;
            sub00.PaddingLeft = 5f;
            Subhead.AddCell(sub00);

            if (dtBasic.Rows[0]["subject"].ToString() != "")
            {
                ph1 = new Phrase();
                ph1.Add(new Chunk("SUBJECT : ", new Font(Cambria9)));
                ph1.Add(new Chunk(dtBasic.Rows[0]["subject"].ToString(), new Font(Cambria9bd)));
                sub00 = new PdfPCell(new Phrase(ph1));
                sub00.Border = 0;
                sub00.Colspan = 3;
                sub00.PaddingLeft = 5f;
                Subhead.AddCell(sub00);
            }

            document.Add(Subhead);

            #endregion

            #region data

            if (dtdetail.Rows.Count > 0)
            {

                PdfPTable emp_details = new PdfPTable(7);
                emp_details.DefaultCell.Padding = 4;
                float[] widthsdet = new float[] { 30f, 20f, 6f, 12f, 12f, 10f, 10f };
                emp_details.SetWidths(widthsdet);
                emp_details.WidthPercentage = 95f;
                emp_details.SpacingBefore = 10f;

                ph1 = new Phrase();
                ph1.Add(new Chunk("DESCRIPTION", new Font(Roboto10)));
                PdfPCell DetailH02 = new PdfPCell(ph1);
                DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                DetailH02.VerticalAlignment = Element.ALIGN_MIDDLE;
                DetailH02.Border = 0;
                emp_details.AddCell(DetailH02);

                ph1 = new Phrase();
                ph1.Add(new Chunk("PARTICULARS", new Font(Roboto10)));
                DetailH02 = new PdfPCell(ph1);
                DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                DetailH02.VerticalAlignment = Element.ALIGN_MIDDLE;
                DetailH02.Border = 0;
                emp_details.AddCell(DetailH02);

                ph1 = new Phrase();
                ph1.Add(new Chunk("QTY", new Font(Roboto10)));
                DetailH02 = new PdfPCell(ph1);
                DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                DetailH02.VerticalAlignment = Element.ALIGN_MIDDLE;
                DetailH02.Border = 0;
                emp_details.AddCell(DetailH02);

                ph1 = new Phrase();
                ph1.Add(new Chunk("UNIT PRICE", new Font(Roboto10)));
                DetailH02 = new PdfPCell(ph1);
                DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                DetailH02.VerticalAlignment = Element.ALIGN_MIDDLE;
                DetailH02.Border = 0;
                emp_details.AddCell(DetailH02);

                //ph1 = new Phrase();
                //ph1.Add(new Chunk("FINE", new Font(Roboto10)));
                //DetailH02 = new PdfPCell(ph1);
                //DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                //DetailH02.VerticalAlignment = Element.ALIGN_MIDDLE;
                //DetailH02.Border = 0;
                //emp_details.AddCell(DetailH02);

                //ph1 = new Phrase();
                //ph1.Add(new Chunk("VAT", new Font(Roboto10)));
                //DetailH02 = new PdfPCell(ph1);
                //DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                //DetailH02.VerticalAlignment = Element.ALIGN_MIDDLE;
                //DetailH02.Border = 0;
                //emp_details.AddCell(DetailH02);

                ph1 = new Phrase();
                ph1.Add(new Chunk("TAXABLE VALUE", new Font(Roboto10)));
                DetailH02 = new PdfPCell(ph1);
                DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                DetailH02.VerticalAlignment = Element.ALIGN_MIDDLE;
                DetailH02.Border = 0;
                emp_details.AddCell(DetailH02);

                ph1 = new Phrase();
                ph1.Add(new Chunk("VAT AMOUNT", new Font(Roboto10)));
                DetailH02 = new PdfPCell(ph1);
                DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                DetailH02.VerticalAlignment = Element.ALIGN_MIDDLE;
                DetailH02.Border = 0;
                emp_details.AddCell(DetailH02);

                ph1 = new Phrase();
                ph1.Add(new Chunk("AMOUNT", new Font(Roboto10)));
                DetailH02 = new PdfPCell(ph1);
                DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                DetailH02.VerticalAlignment = Element.ALIGN_MIDDLE;
                DetailH02.Border = 0;
                DetailH02.MinimumHeight = 25f;
                emp_details.AddCell(DetailH02);

                PdfPCell EmptyDetails = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                EmptyDetails.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                EmptyDetails.Border = 0;

                decimal vat = 0;

                foreach (DataRow rows in dtdetail.Rows)
                {
                    try
                    {
                        PdfPCell REMIN = new PdfPCell(new Phrase(rows["Name"].ToString(), new Font(Calibri9)));
                        REMIN.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        REMIN.Border = PdfPCell.TOP_BORDER;
                        REMIN.BorderColor = iTextSharp.text.BaseColor.GRAY;
                        REMIN.MinimumHeight = 25f;
                        REMIN.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                        emp_details.AddCell(REMIN);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell REMIN = new PdfPCell(new Phrase(rows["ParticularsD"].ToString(), new Font(Calibri9)));
                        REMIN.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        REMIN.Border = PdfPCell.TOP_BORDER;
                        REMIN.BorderColor = iTextSharp.text.BaseColor.GRAY;
                        REMIN.MinimumHeight = 25f;
                        REMIN.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                        emp_details.AddCell(REMIN);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell DT = new PdfPCell(new Phrase(rows["Quantity"].ToString(), new Font(Cambria10)));
                        DT.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        DT.Border = PdfPCell.TOP_BORDER;
                        DT.BorderColor = iTextSharp.text.BaseColor.GRAY;
                        DT.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                        emp_details.AddCell(DT);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell REMIN = new PdfPCell(new Phrase(rows["AmountNoDisTax"].ToString(), new Font(Cambria10)));
                        REMIN.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        REMIN.Border = PdfPCell.TOP_BORDER;
                        REMIN.BorderColor = iTextSharp.text.BaseColor.GRAY;
                        REMIN.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                        emp_details.AddCell(REMIN);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }

                    //try
                    //{
                    //    PdfPCell DT = new PdfPCell(new Phrase(rows["Fine"].ToString(), new Font(Cambria10)));
                    //    DT.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    //    DT.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    //    DT.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    //    emp_details.AddCell(DT);
                    //}
                    //catch (Exception eee)
                    //{
                    //    emp_details.AddCell("");
                    //}
                    //try
                    //{
                    //    PdfPCell DT = new PdfPCell(new Phrase(rows["TaxPer"].ToString() + "%", new Font(Cambria10)));
                    //    DT.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    //    DT.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    //    DT.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    //    emp_details.AddCell(DT);

                    //}
                    //catch (Exception eee)
                    //{
                    //    emp_details.AddCell("");
                    //}

                    try
                    {
                        PdfPCell DT = new PdfPCell(new Phrase(rows["TaxableamountafterDiscount"].ToString(), new Font(Cambria10)));
                        DT.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        DT.Border = PdfPCell.TOP_BORDER;
                        DT.BorderColor = iTextSharp.text.BaseColor.GRAY;
                        DT.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                        emp_details.AddCell(DT);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell DT = new PdfPCell(new Phrase(rows["SingleTaxAmount"].ToString(), new Font(Cambria10)));
                        DT.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        DT.Border = PdfPCell.TOP_BORDER;
                        DT.BorderColor = iTextSharp.text.BaseColor.GRAY;
                        DT.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                        emp_details.AddCell(DT);

                        vat = vat + Convert.ToDecimal(rows["TaxAmount"]);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell DT = new PdfPCell(new Phrase(rows["Total"].ToString(), new Font(Cambria10)));
                        DT.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        DT.Border = PdfPCell.TOP_BORDER;
                        DT.BorderColor = iTextSharp.text.BaseColor.GRAY;
                        DT.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                        emp_details.AddCell(DT);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }


                }
                PdfPCell DTO = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                DTO.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                DTO.Border = PdfPCell.TOP_BORDER;
                DTO.BorderColor = iTextSharp.text.BaseColor.GRAY;
                DTO.Colspan = 8;
                DTO.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                emp_details.AddCell(DTO);

                DTO = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                DTO.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                DTO.Border = 0;
                DTO.Colspan = 8;
                DTO.MinimumHeight = 20f;
                DTO.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                emp_details.AddCell(DTO);

                DTO = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                DTO.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                DTO.Border = 0;
                DTO.Colspan = 4;
                DTO.MinimumHeight = 20f;
                DTO.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                emp_details.AddCell(DTO);

                DTO = new PdfPCell(new Phrase("DISCOUNT", new Font(Calibri10wt)));
                DTO.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                //DTO.Border = 0;
                DTO.Colspan = 2;
                DTO.MinimumHeight = 20f;
                DTO.PaddingLeft = 15f;
                DTO.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                DTO.BackgroundColor = Blackbg;
                DTO.BorderColor = Blackbg;
                emp_details.AddCell(DTO);

                DTO = new PdfPCell(new Phrase("AED " + dtBasic.Rows[0]["Total_Discount"].ToString(), new Font(Calibri10wt)));
                DTO.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                //DTO.Border = 0;
                DTO.Colspan = 2;
                DTO.MinimumHeight = 20f;
                DTO.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                DTO.BackgroundColor = Blackbg;
                DTO.BorderColor = Blackbg;
                emp_details.AddCell(DTO);


                DTO = new PdfPCell(new Phrase("TOTAL EXCLUDING VAT", new Font(Calibri10)));
                DTO.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                DTO.Border = 0;
                DTO.PaddingLeft = 15f;
                DTO.MinimumHeight = 20f;
                DTO.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                DTO.BackgroundColor = LightGray;
                emp_details.AddCell(DTO);

                DTO = new PdfPCell(new Phrase("TOTAL VAT", new Font(Calibri10)));
                DTO.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                DTO.Border = 0;
                DTO.Colspan = 3;
                DTO.MinimumHeight = 20f;
                DTO.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                DTO.BackgroundColor = LightGray;
                emp_details.AddCell(DTO);

                DTO = new PdfPCell(new Phrase("GRAND TOTAL", new Font(Calibri10wt)));
                DTO.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                //DTO.Border = 0;
                DTO.Colspan = 5;
                DTO.MinimumHeight = 20f;
                DTO.PaddingLeft = 15f;
                DTO.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                DTO.BackgroundColor = Blackbg;
                DTO.BorderColor = Blackbg;
                emp_details.AddCell(DTO);


                DTO = new PdfPCell(new Phrase("AED " + (Convert.ToDecimal(dtBasic.Rows[0]["AfterDiscount_GrandTotal"]) - vat).ToString(), new Font(Calibri14)));
                DTO.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                DTO.Border = 0;
                DTO.PaddingLeft = 15f;
                DTO.MinimumHeight = 30f;
                DTO.VerticalAlignment = PdfPCell.ALIGN_TOP;
                DTO.BackgroundColor = LightGray;
                emp_details.AddCell(DTO);

                DTO = new PdfPCell(new Phrase("AED " + vat.ToString(), new Font(Calibri14)));
                DTO.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                DTO.Border = 0;
                DTO.Colspan = 3;
                DTO.MinimumHeight = 30f;
                DTO.VerticalAlignment = PdfPCell.ALIGN_TOP;
                DTO.BackgroundColor = LightGray;
                emp_details.AddCell(DTO);

                DTO = new PdfPCell(new Phrase("AED " + dtBasic.Rows[0]["AfterDiscount_GrandTotal"].ToString(), new Font(Calibri20wt)));
                DTO.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                //DTO.Border = 0;
                DTO.Colspan = 5;
                DTO.PaddingLeft = 15f;
                DTO.MinimumHeight = 30f;
                DTO.VerticalAlignment = PdfPCell.ALIGN_TOP;
                DTO.BackgroundColor = Blackbg;
                DTO.BorderColor = Blackbg;
                emp_details.AddCell(DTO);

                ///

                document.Add(emp_details);

                PdfPTable totalexp = new PdfPTable(1);
                totalexp.DefaultCell.Padding = 4;
                float[] widths1 = new float[] { 100f };
                totalexp.SetWidths(widths1);
                totalexp.SpacingBefore = 30;
                totalexp.WidthPercentage = 95f;

                if (dtBasic.Rows[0]["Remarks"].ToString() != "")
                {
                    PdfPCell tot2ww = new PdfPCell(new Phrase("Remark", new Font(Calibri10)));
                    tot2ww.MinimumHeight = 20f;
                    tot2ww.Border = 0;
                    totalexp.AddCell(tot2ww);

                    tot2ww = new PdfPCell(new Phrase(dtBasic.Rows[0]["Remarks"].ToString(), new Font(Calibri10)));
                    tot2ww.MinimumHeight = 20f;
                    tot2ww.Border = 0;
                    totalexp.AddCell(tot2ww);

                    tot2ww = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                    tot2ww.MinimumHeight = 20f;
                    tot2ww.Border = 0;
                    totalexp.AddCell(tot2ww);
                }

                DTO = new PdfPCell(new Phrase("Prepared by : " + dtBasic.Rows[0]["Name"].ToString(), new Font(Calibri10)));
                DTO.Border = 0;
                DTO.MinimumHeight = 25F;
                DTO.HorizontalAlignment = Element.ALIGN_LEFT;
                totalexp.AddCell(DTO);

                DTO = new PdfPCell(new Phrase("This is a computer generated document and does not need a signature.", new Font(Calibri9)));
                DTO.Border = 0;
                DTO.MinimumHeight = 25F;
                DTO.HorizontalAlignment = Element.ALIGN_LEFT;
                totalexp.AddCell(DTO);

                //DTO = new PdfPCell(new Phrase("VERIFIED BY", new Font(Calibri10rd)));
                //DTO.Border = 0;
                //DTO.MinimumHeight = 20F;
                //DTO.HorizontalAlignment = Element.ALIGN_LEFT;
                //totalexp.AddCell(DTO);

                //DTO = new PdfPCell(new Phrase("SIGNATURE", new Font(Calibri10rd)));
                //DTO.Border = 0;
                //DTO.HorizontalAlignment = Element.ALIGN_RIGHT;
                //DTO.PaddingRight = 20F;
                //totalexp.AddCell(DTO);

                //DTO = new PdfPCell(new Phrase(dtBasic.Rows[0]["Name"].ToString(), new Font(Cambria9)));
                //DTO.Border = 0;
                //DTO.Colspan = 2;
                //DTO.MinimumHeight = 20F;
                //DTO.HorizontalAlignment = Element.ALIGN_LEFT;
                //totalexp.AddCell(DTO);

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

            if (dtGen.Rows[0]["PrintFooter"].ToString() != "")
            {
                PdfPTable footer = new PdfPTable(1);
                footer.DefaultCell.Padding = 4;
                //footer.SpacingAfter = 20f;
                footer.TotalWidth = document.PageSize.Width - (2 * document.LeftMargin);
                footer.WidthPercentage = 90f;

                string imageURL = Server.MapPath("../UploadedImage/" + dtGen.Rows[0]["PrintFooter"].ToString());
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(new Uri(imageURL));
                jpg.ScaleToFit(470f, 450f);

                PdfPCell Fotservice = new PdfPCell(jpg, true);
                Fotservice.Border = 0;
                Fotservice.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                footer.AddCell(Fotservice);

                if (dtGen.Rows[0]["IsSoftareNameAdd"].ToString() == "1")
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

            else if (dtGen.Rows[0]["IsSoftareNameAdd"].ToString() == "1")
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