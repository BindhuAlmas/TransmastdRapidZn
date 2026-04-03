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
    public partial class CustomerInvoiceFormat3 : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();
        Master_Bal obj_master = new Master_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"]);
            DataSet ds = obj_report.CustomerInvoicePrintFormat1(id);
            DataTable dt_inv = ds.Tables[0];
            DataTable dt_invD = ds.Tables[1];
            DataTable dt_cust = ds.Tables[2];
            DataTable dt_sum = ds.Tables[3];
            DataTable dtTRNFromGeneral = ds.Tables[4];

            Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", dtTRNFromGeneral.Rows[0]["DefaultInvoiceType"].ToString() == "1" ? "inline;filename=TaxInvoicePrint.pdf" : "inline;filename=InvoicePrint.pdf");
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
            BaseFont bfTimes = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Font/arabtype.ttf"), BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font arbfnt = new iTextSharp.text.Font(bfTimes, 10, Font.NORMAL);
            iTextSharp.text.Font arbsmallbold = new iTextSharp.text.Font(bfTimes, 8, Font.BOLD);
            iTextSharp.text.Font arbfntbld = new iTextSharp.text.Font(bfTimes, 15, Font.BOLD);
            //BaseFont bfTimes = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Font/ROYAL.ttf"), BaseFont.IDENTITY_H, true);
            //iTextSharp.text.Font arbfnt = new iTextSharp.text.Font(bfTimes, 18, Font.NORMAL,BaseColor.BLACK);
            //iTextSharp.text.Font arbsmallbold = new iTextSharp.text.Font(bfTimes, 18, Font.BOLD);
            //iTextSharp.text.Font arbfntbld = new iTextSharp.text.Font(bfTimes, 18, Font.BOLD);
            #region header

            PdfPTable Subhead = new PdfPTable(5);
            Subhead.DefaultCell.Padding = 4;
            float[] widths = new float[] { 15f, 25f, 25f, 30f, 25f };
            Subhead.SetWidths(widths);
            Subhead.WidthPercentage = 97f;


            PdfPCell Empty = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            Empty.Border = 0;
            Empty.HorizontalAlignment = Element.ALIGN_LEFT;

            PdfPCell EmptyWithTopBorder = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            EmptyWithTopBorder.Border = PdfPCell.TOP_BORDER;
            EmptyWithTopBorder.HorizontalAlignment = Element.ALIGN_LEFT;

            PdfPCell sub00 = new PdfPCell(new Phrase(dt_cust.Rows[0]["Name"].ToString() + dt_cust.Rows[0]["AddressD"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub00.Border = PdfPCell.TOP_BORDER;
            sub00.Colspan = 2;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            Subhead.AddCell(EmptyWithTopBorder);

            Phrase ph1 = new Phrase();
            ph1.Add(new Chunk(dtTRNFromGeneral.Rows[0]["DefaultInvoiceType"].ToString() == "1" ? "TAX INVOICE\n" : "INVOICE\n", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            if (dtTRNFromGeneral.Rows[0]["DefaultInvoiceType"].ToString() == "1")
                ph1.Add(new Chunk("فاتورة ضريبية", arbfntbld));
            else if (dtTRNFromGeneral.Rows[0]["DefaultInvoiceType"].ToString() == "2")
                ph1.Add(new Chunk("فاتورة", arbfntbld));
            if (dtTRNFromGeneral.Rows[0]["DefaultInvoiceType"].ToString() != "")
                ph1.Add(new Chunk("\n\nTRN : " + dtTRNFromGeneral.Rows[0]["TRN"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));

            PdfPCell sub04 = new PdfPCell(ph1);
            //PdfPCell sub04 = new PdfPCell(new Phrase("TAX INVOICE", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            sub04.Border = PdfPCell.TOP_BORDER;
            sub04.Colspan = 2;
            sub04.HorizontalAlignment = Element.ALIGN_CENTER;
            sub04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(sub04);
            /*End of Row*/

            ph1 = new Phrase();
            ph1.Add(new Chunk(dt_cust.Rows[0]["TRN"].ToString() + " :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("رقم ضريبي", arbfnt));
            ph1.Add(new Chunk(" / Customer TRN", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            PdfPCell sub10 = new PdfPCell(ph1);
            sub10.Border = 0;
            sub10.HorizontalAlignment = Element.ALIGN_RIGHT;
            sub10.VerticalAlignment = Element.ALIGN_BOTTOM;
            sub10.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            sub10.Colspan = 3;
            Subhead.AddCell(sub10);



            //Subhead.AddCell(Empty);


            System.IO.MemoryStream mem = new MemoryStream();
            Barcode128 barImg = new Barcode128();
            barImg.Code = dt_inv.Rows[0]["Code"].ToString();
            barImg.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White).Save(mem, System.Drawing.Imaging.ImageFormat.Png);
            iTextSharp.text.Image imgs = iTextSharp.text.Image.GetInstance(mem.ToArray());
            mem.Flush();
            mem.Close();

            PdfPCell sub13 = new PdfPCell(imgs);
            sub13.Border = 0;
            sub13.Colspan = 2;
            sub13.MinimumHeight = 50f;
            sub13.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            sub13.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            Subhead.AddCell(sub13);
            /*End of Row*/

            Subhead.AddCell(Empty);
            Subhead.AddCell(Empty);
            Subhead.AddCell(Empty);

            ph1 = new Phrase();
            ph1.Add(new Chunk(" :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("رقم الفاتورة", arbfnt));
            ph1.Add(new Chunk(" / Invoice No", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            PdfPCell sub23 = new PdfPCell(ph1);
            //PdfPCell sub23 = new PdfPCell(new Phrase("Invoice No:", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub23.Border = 0;
            sub23.HorizontalAlignment = Element.ALIGN_RIGHT;
            sub23.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(sub23);

            PdfPCell sub24 = new PdfPCell(new Phrase(dt_inv.Rows[0]["Code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub24.Border = 0;
            sub24.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub24);
            /*End of Row*/

            ph1 = new Phrase();
            ph1.Add(new Chunk(dt_cust.Rows[0]["Mobile_num"].ToString() + " :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("موبايل", arbfnt));
            ph1.Add(new Chunk(" / Mob", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));

            ph1.Add(new Chunk("\n", arbfnt));
            ph1.Add(new Chunk(dt_cust.Rows[0]["Phone_num"].ToString() + " :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("هاتف", arbfnt));
            ph1.Add(new Chunk(" / Tel", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            PdfPCell sub30 = new PdfPCell(ph1);
            //PdfPCell sub30 = new PdfPCell(new Phrase("Mob: " + dt_cust.Rows[0]["Mobile_num"].ToString() + ", " +
            //    "Tel: " + dt_cust.Rows[0]["Phone_num"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub30.Border = 0;
            sub30.Colspan = 3;
            sub30.HorizontalAlignment = Element.ALIGN_RIGHT;
            sub30.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(sub30);

            ph1 = new Phrase();
            ph1.Add(new Chunk(" :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("تاريخ الفاتورة", arbfnt));
            ph1.Add(new Chunk(" / Invoice Date", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            PdfPCell sub33 = new PdfPCell(ph1);
            //PdfPCell sub33 = new PdfPCell(new Phrase("Invoice Date:", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub33.Border = 0;
            sub33.HorizontalAlignment = Element.ALIGN_RIGHT;
            sub33.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(sub33);

            PdfPCell sub34 = new PdfPCell(new Phrase(dt_inv.Rows[0]["Dates"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub34.Border = 0;
            sub34.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub34);
            /*End of Row*/

            PdfPCell EmptyWithBottomBorder = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            EmptyWithBottomBorder.Border = PdfPCell.BOTTOM_BORDER;
            EmptyWithBottomBorder.HorizontalAlignment = Element.ALIGN_LEFT;

            ph1 = new Phrase();
            ph1.Add(new Chunk(dt_cust.Rows[0]["Email"].ToString() + " :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("ايميل", arbfnt));
            ph1.Add(new Chunk(" / Email", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            PdfPCell sub40 = new PdfPCell(ph1);
            //PdfPCell sub40 = new PdfPCell(new Phrase("Email: " + dt_cust.Rows[0]["Email"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub40.Border = 0;
            sub40.Colspan = 2;
            sub40.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            //sub40.Border = PdfPCell.BOTTOM_BORDER;
            sub40.HorizontalAlignment = Element.ALIGN_RIGHT;
            Subhead.AddCell(sub40);

            Subhead.AddCell(Empty);
            Subhead.AddCell(Empty);
            Subhead.AddCell(Empty);
            /*End of Row*/
            document.Add(Subhead);

            #endregion

            #region data

            if (dt_invD.Rows.Count > 0)
            {
                /*Tax Invoice Type*/
                if (dtTRNFromGeneral.Rows[0]["DefaultInvoiceType"].ToString() == "1")
                {
                    PdfPTable emp_details = new PdfPTable(10);
                    emp_details.DefaultCell.Padding = 4;
                    float[] widthsdet = new float[] { 5f, 30f, 13f, 8f,  9f, 9f, 6f, 8f, 7f, 11f };
                    emp_details.SetWidths(widthsdet);
                    emp_details.SpacingBefore = 10f;
                    emp_details.WidthPercentage = 97f;
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("No\n", new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD)));
                    ph1.Add(new Chunk("رقم", arbsmallbold));
                    PdfPCell DetailH01 = new PdfPCell(ph1);
                    DetailH01.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH01.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH01.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH01);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Type of Transactions\n", new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD)));
                    ph1.Add(new Chunk("نوع المعاملة", arbsmallbold));
                    PdfPCell DetailH02 = new PdfPCell(ph1);
                    DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                    //DetailH02.Colspan = 2;
                    DetailH02.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH02.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH02);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Particulars\n", new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD)));
                    ph1.Add(new Chunk("تفاصيل", arbsmallbold));
                    DetailH02 = new PdfPCell(ph1);
                    DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                    //DetailH02.Colspan = 2;
                    DetailH02.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH02.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH02);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Quantity\n", new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD)));
                    ph1.Add(new Chunk("الكمية", arbsmallbold));
                    PdfPCell DetailH03 = new PdfPCell(ph1);
                    DetailH03.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH03.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH03.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH03);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Fees\n", new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD)));
                    ph1.Add(new Chunk("رسوم", arbsmallbold));
                    PdfPCell DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH04);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Typing Fees\n", new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD)));
                    ph1.Add(new Chunk("رسوم طباعة", arbsmallbold));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH04);
                    //ph1 = new Phrase();
                    //ph1.Add(new Chunk("Discount\n", new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD)));
                    //ph1.Add(new Chunk("خصم", arbsmallbold));
                    //DetailH04 = new PdfPCell(ph1);
                    //DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    //DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    //DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    //emp_details.AddCell(DetailH04);
                    //ph1 = new Phrase();
                    //ph1.Add(new Chunk("Typing Fees After Discount\n", new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD)));
                    //ph1.Add(new Chunk("رسوم طباعة", arbsmallbold));
                    //DetailH04 = new PdfPCell(ph1);
                    //DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    //DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    //DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    //emp_details.AddCell(DetailH04);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("VAT %\n", new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD)));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH04);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("VAT Amount\n", new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD)));
                    ph1.Add(new Chunk("ضريبة", arbsmallbold));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH04);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Fine\n", new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD)));
                    ph1.Add(new Chunk("مبلغ الغرامة", arbsmallbold));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH04);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Amount\n", new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD)));
                    ph1.Add(new Chunk("المبلغ", arbsmallbold));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH04);


                    PdfPCell EmptyDetails = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    EmptyDetails.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    EmptyDetails.Border = 0;
                    int i = 0;

                    foreach (DataRow rows in dt_invD.Rows)
                    {
                        try
                        {
                            PdfPCell sn = new PdfPCell(new Phrase((++i).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            sn.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            sn.Border = 0;
                            emp_details.AddCell(sn);
                        }
                        catch (Exception ee)
                        {
                            emp_details.AddCell(EmptyDetails);
                        }
                        try
                        {
                            PdfPTable intble = new PdfPTable(1);
                            intble.DefaultCell.Border = 0;
                            intble.WidthPercentage = 95f;

                            PdfPCell TP = new PdfPCell(new Phrase(rows["Name"].ToString() + "\n", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            TP.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            TP.Border = 0;
                            intble.AddCell(TP);

                            Phrase phrase14 = new Phrase();
                            phrase14.Add(new Chunk(rows["NameInArabic"].ToString(), arbfnt));

                            TP = new PdfPCell(phrase14);
                            TP.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                            TP.Border = 0;
                            intble.AddCell(TP);
                            PdfPCell REM = new PdfPCell(intble);
                            REM.Border = 0;
                            emp_details.AddCell(REM);
                        }
                        catch (Exception eee)
                        {
                            PdfPCell REM = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            REM.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            REM.Colspan = 2;
                            REM.Border = 0;
                            emp_details.AddCell(REM);
                        }
                        try
                        {
                            PdfPCell REM = new PdfPCell(new Phrase(rows["ParticularsD"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            REM.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            REM.Border = 0;
                            emp_details.AddCell(REM);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell(EmptyDetails);
                        }
                        try
                        {
                            PdfPCell REM = new PdfPCell(new Phrase(rows["Quantity"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            REM.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            REM.Border = 0;
                            emp_details.AddCell(REM);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell(EmptyDetails);
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Expense"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            DT.Border = 0;
                            emp_details.AddCell(DT);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell(EmptyDetails);
                        }
                        //try
                        //{
                        //    PdfPCell DT = new PdfPCell(new Phrase(rows["TotalServiceCharge"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        //    DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        //    DT.Border = 0;
                        //    emp_details.AddCell(DT);
                        //}
                        //catch (Exception eee)
                        //{
                        //    emp_details.AddCell(EmptyDetails);
                        //}
                        //try
                        //{
                        //    PdfPCell DT = new PdfPCell(new Phrase(rows["Discount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        //    DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        //    DT.Border = 0;
                        //    emp_details.AddCell(DT);
                        //}
                        //catch (Exception eee)
                        //{
                        //    emp_details.AddCell(EmptyDetails);
                        //}
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["TotalServiceChargeafterDiscount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            DT.Border = 0;
                            emp_details.AddCell(DT);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell(EmptyDetails);
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["TaxPer"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            DT.Border = 0;
                            emp_details.AddCell(DT);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell(EmptyDetails);
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["TaxAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            DT.Border = 0;
                            emp_details.AddCell(DT);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell(EmptyDetails);
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Fine"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            DT.Border = 0;
                            emp_details.AddCell(DT);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell(EmptyDetails);
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            DT.Border = 0;
                            emp_details.AddCell(DT);

                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell(EmptyDetails);
                        }

                    }
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("المجموع", arbfnt));
                    ph1.Add(new Chunk(" / Total", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    PdfPCell DetailEnd00 = new PdfPCell(ph1);
                    DetailEnd00.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DetailEnd00.Colspan = 3;
                    DetailEnd00.Border = PdfPCell.TOP_BORDER;
                    DetailEnd00.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailEnd00);

                    DetailEnd00 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TQuantity"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    DetailEnd00.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    DetailEnd00.Border = PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailEnd00);

                    PdfPCell tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TExpense"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    tot1.Border = PdfPCell.TOP_BORDER;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(tot1);

                    //tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TSC"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    //tot1.Border = PdfPCell.TOP_BORDER;
                    //tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //emp_details.AddCell(tot1);

                    //tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TDiscount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    //tot1.Border = PdfPCell.TOP_BORDER;
                    //tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //emp_details.AddCell(tot1);
                    tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TSCAfterDiscount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    tot1.Border = PdfPCell.TOP_BORDER;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(tot1);

                    tot1 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    tot1.Border = PdfPCell.TOP_BORDER;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(tot1);
                    tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TTAx"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    tot1.Border = PdfPCell.TOP_BORDER;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(tot1);

                    tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TFine"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    tot1.Border = PdfPCell.TOP_BORDER;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(tot1);

                    tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    tot1.Border = PdfPCell.TOP_BORDER;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(tot1);

                    document.Add(emp_details);
                }
                else if (dtTRNFromGeneral.Rows[0]["DefaultInvoiceType"].ToString() == "2")/*Normal Invoice Type*/
                {
                    PdfPTable emp_details = new PdfPTable(9);
                    emp_details.DefaultCell.Padding = 4;
                    float[] widthsdet = new float[] { 5f, 30f, 13f, 10f, 10f, 10f, 10f, 10f, 11f };
                    emp_details.SetWidths(widthsdet);
                    emp_details.SpacingBefore = 10f;
                    emp_details.WidthPercentage = 95f;
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("No\n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("رقم", arbsmallbold));
                    PdfPCell DetailH01 = new PdfPCell(ph1);
                    DetailH01.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH01.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH01.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH01);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Type of Transactions\n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("نوع المعاملة", arbsmallbold));
                    PdfPCell DetailH02 = new PdfPCell(ph1);
                    DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                    //DetailH02.Colspan = 2;
                    DetailH02.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH02.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH02);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Particulars\n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("تفاصيل", arbsmallbold));
                    DetailH02 = new PdfPCell(ph1);
                    DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                    //DetailH02.Colspan = 2;
                    DetailH02.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH02.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH02);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Persons\n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("الأشخاص", arbsmallbold));
                    PdfPCell DetailH03 = new PdfPCell(ph1);
                    DetailH03.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH03.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH03.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH03);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Fees\n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("رسوم", arbsmallbold));
                    PdfPCell DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH04);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Typing Fees\n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("رسوم طباعة", arbsmallbold));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH04);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Discount\n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("خصم", arbsmallbold));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH04);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Fine\n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("مبلغ الغرامة", arbsmallbold));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH04);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Amount\n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("المبلغ", arbsmallbold));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH04);


                    PdfPCell EmptyDetails = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    EmptyDetails.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    EmptyDetails.Border = 0;
                    int i = 0;

                    foreach (DataRow rows in dt_invD.Rows)
                    {
                        try
                        {
                            PdfPCell sn = new PdfPCell(new Phrase((++i).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            sn.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            sn.Border = 0;
                            emp_details.AddCell(sn);
                        }
                        catch (Exception ee)
                        {
                            emp_details.AddCell(EmptyDetails);
                        }
                        try
                        {
                            //PdfPCell REM = new PdfPCell(new Phrase(rows["Name"].ToString()  + rows["Particulars"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            //REM.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            //REM.Border = 0;
                            //emp_details.AddCell(REM);
                            PdfPTable intble = new PdfPTable(1);
                            intble.DefaultCell.Border = 0;
                            intble.WidthPercentage = 95f;

                            PdfPCell TP = new PdfPCell(new Phrase(rows["Name"].ToString() + "\n", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            TP.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            TP.Border = 0;
                            intble.AddCell(TP);

                            Phrase phrase14 = new Phrase();
                            phrase14.Add(new Chunk(rows["NameInArabic"].ToString(), arbfnt));

                            TP = new PdfPCell(phrase14);
                            TP.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                            TP.Border = 0;
                            intble.AddCell(TP);
                            PdfPCell REM = new PdfPCell(intble);
                            REM.Border = 0;
                            emp_details.AddCell(REM);
                        }
                        catch (Exception eee)
                        {
                            PdfPCell REM = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            REM.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            REM.Colspan = 2;
                            REM.Border = 0;
                            emp_details.AddCell(REM);
                        }
                        try
                        {
                            PdfPCell REM = new PdfPCell(new Phrase(rows["ParticularsD"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            REM.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            REM.Border = 0;
                            emp_details.AddCell(REM);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell(EmptyDetails);
                        }
                        try
                        {
                            PdfPCell REM = new PdfPCell(new Phrase(rows["Quantity"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            REM.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            REM.Border = 0;
                            emp_details.AddCell(REM);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell(EmptyDetails);
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Expense"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            DT.Border = 0;
                            emp_details.AddCell(DT);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell(EmptyDetails);
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["TotalServiceCharge"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            DT.Border = 0;
                            emp_details.AddCell(DT);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell(EmptyDetails);
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Discount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            DT.Border = 0;
                            emp_details.AddCell(DT);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell(EmptyDetails);
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Fine"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            DT.Border = 0;
                            emp_details.AddCell(DT);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell(EmptyDetails);
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            DT.Border = 0;
                            emp_details.AddCell(DT);

                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell(EmptyDetails);
                        }

                    }
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("المجموع", arbfnt));
                    ph1.Add(new Chunk(" / Total", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    PdfPCell DetailEnd00 = new PdfPCell(ph1);
                    //PdfPCell DetailEnd00 = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    DetailEnd00.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DetailEnd00.Colspan = 3;
                    DetailEnd00.Border = PdfPCell.TOP_BORDER;
                    DetailEnd00.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailEnd00);

                    DetailEnd00 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TQuantity"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    DetailEnd00.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    DetailEnd00.Border = PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailEnd00);

                    PdfPCell tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TExpense"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    tot1.Border = PdfPCell.TOP_BORDER;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(tot1);

                    tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TSC"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    tot1.Border = PdfPCell.TOP_BORDER;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(tot1);

                    tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TDiscount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    tot1.Border = PdfPCell.TOP_BORDER;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(tot1);

                    tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TFine"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    tot1.Border = PdfPCell.TOP_BORDER;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(tot1);

                    tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    tot1.Border = PdfPCell.TOP_BORDER;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(tot1);

                    document.Add(emp_details);
                }
                PdfPTable totalexp = new PdfPTable(3);
                totalexp.DefaultCell.Padding = 4;
                float[] widths1 = new float[] { 55f, 20f, 13f };
                totalexp.SetWidths(widths1);
                totalexp.SpacingBefore = 10;
                totalexp.WidthPercentage = 95f;

                ph1 = new Phrase();
                ph1.Add(new Chunk("AED " + ConvertNumbertoWords(Convert.ToDecimal(dt_sum.Rows[0]["Total"])) + " Only :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLDITALIC)));
                ph1.Add(new Chunk("المجموع الاجمالي", arbfnt));
                ph1.Add(new Chunk(" / Grand Total", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLDITALIC)));
                PdfPCell toddddt2wwDD = new PdfPCell(ph1);
                //PdfPCell toddddt2wwDD = new PdfPCell(new Phrase("Grand Total: " + ConvertNumbertoWords(Convert.ToDecimal(dt_sum.Rows[0]["Total"])) + " Only", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLDITALIC)));
                toddddt2wwDD.Border = 0;
                toddddt2wwDD.Colspan = 3;
                toddddt2wwDD.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                toddddt2wwDD.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(toddddt2wwDD);

                document.Add(totalexp);

                PdfPTable terms = new PdfPTable(5);
                terms.DefaultCell.Padding = 4;
                float[] widthsTerms = new float[] { 23f, 27f, 4f, 24f, 20f };
                terms.SetWidths(widthsTerms);
                terms.SpacingBefore = 10;
                terms.WidthPercentage = 95f;

                if (dtTRNFromGeneral.Rows[0]["IsAddRemark"].ToString() != "0")
                {
                    ph1 = new Phrase();
                    ph1.Add(new Chunk(" :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                    ph1.Add(new Chunk("ملاحظات", arbfnt));
                    ph1.Add(new Chunk(" / Remarks", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                    PdfPCell remarksCell = new PdfPCell(ph1);
                    remarksCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    remarksCell.Border = 0;
                    remarksCell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    terms.AddCell(remarksCell);

                    PdfPCell EmptyTerms1 = new PdfPCell(new Phrase(dt_inv.Rows[0]["Remarks"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                    EmptyTerms1.HorizontalAlignment = Element.ALIGN_LEFT;
                    EmptyTerms1.Colspan = 4;
                    EmptyTerms1.Border = 0;
                    terms.AddCell(EmptyTerms1);
                }
                /*End of Row*/

                PdfPCell EmptyTerms = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                EmptyTerms.Border = PdfPCell.BOTTOM_BORDER;
                EmptyTerms.HorizontalAlignment = Element.ALIGN_LEFT;
                EmptyTerms.Colspan = 5;
                terms.AddCell(EmptyTerms);
                /*End of Row*/
                EmptyTerms = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                EmptyTerms.Border = 0;
                EmptyTerms.HorizontalAlignment = Element.ALIGN_LEFT;
                EmptyTerms.Colspan = 3;
                terms.AddCell(EmptyTerms);

                ph1 = new Phrase();
                ph1.Add(new Chunk(" :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                ph1.Add(new Chunk("صنع من قبل", arbfnt));
                ph1.Add(new Chunk(" / Created By", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                PdfPCell termsCell = new PdfPCell(ph1);
                //PdfPCell termsCell = new PdfPCell(new Phrase("Created By: ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                termsCell.Border = 0;
                termsCell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                termsCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //termsCell.Colspan = 2;
                terms.AddCell(termsCell);
                termsCell = new PdfPCell(new Phrase(dt_inv.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                termsCell.Border = 0;
                termsCell.HorizontalAlignment = Element.ALIGN_LEFT;
                terms.AddCell(termsCell);

                document.Add(terms);
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
            if ((number / 100000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000) + " Lakhs ";
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
                    words += " Fills";
                }
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                    {
                        words += " " + unitsMap[number % 10];
                        words += " Fills";
                    }
                    else
                    {
                        words += " Fills";
                    }
                }
            }
            return words;
        }
    }
}