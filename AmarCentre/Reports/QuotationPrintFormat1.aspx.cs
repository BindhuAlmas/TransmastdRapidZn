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
    public partial class QuotationPrintFormat1 : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();
        Master_Bal obj_master = new Master_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"]);
            DataSet ds = obj_report.QuotationPrint(id);
            DataTable dt_inv = ds.Tables[0];
            DataTable dt_invD = ds.Tables[1];
            DataTable dt_cust = ds.Tables[2];
            DataTable dt_sum = ds.Tables[3];
            DataTable dtGenSetting = ds.Tables[4];
            bool PrintTerms = Convert.ToBoolean(ds.Tables[4].Rows[0]["PrintTerms"]);
            Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=QuotationPrint.pdf");
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
            // BaseFont bfTimes = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Font/arialuni.ttf"), BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font arbfnt = new iTextSharp.text.Font(bfTimes, 10, Font.NORMAL);
            iTextSharp.text.Font arbsmallbold = new iTextSharp.text.Font(bfTimes, 8, Font.BOLD);
            iTextSharp.text.Font arbfntbld = new iTextSharp.text.Font(bfTimes, 15, Font.BOLD);

            #region header

            //BaseFont bfTimes = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Font/arabtype.ttf"), BaseFont.IDENTITY_H, true);
            //iTextSharp.text.Font arbfnt = new iTextSharp.text.Font(bfTimes, 17, Font.NORMAL);
            //iTextSharp.text.Font arbfntbld = new iTextSharp.text.Font(bfTimes, 21, Font.BOLD);

            //PdfPTable table1 = new PdfPTable(1);
            //table1.DefaultCell.Padding = 4;
            //PdfPCell cell1 = new PdfPCell(new Phrase("Sales Order INVOICE", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            //cell1.Border = 0;
            //cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            //table1.AddCell(cell1);

            //document.Add(table1);

            PdfPTable Subhead = new PdfPTable(2);
            Subhead.DefaultCell.Padding = 4;
            float[] widths = new float[] { 65f, 35f };
            Subhead.SetWidths(widths);
            Subhead.WidthPercentage = 95f;


            PdfPCell Empty = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            Empty.Border = 0;
            Empty.HorizontalAlignment = Element.ALIGN_LEFT;

            PdfPCell EmptyWithTopBorder = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            EmptyWithTopBorder.Border = PdfPCell.TOP_BORDER;
            EmptyWithTopBorder.HorizontalAlignment = Element.ALIGN_LEFT;

            Subhead.AddCell(EmptyWithTopBorder);

            PdfPTable headInnerTable1 = new PdfPTable(1);
            headInnerTable1.DefaultCell.Border = 0;
            headInnerTable1.WidthPercentage = 95f;
            headInnerTable1.SpacingAfter = 2f;

            PdfPCell HIT1 = new PdfPCell(new Phrase("QUOTATION", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            HIT1.HorizontalAlignment = Element.ALIGN_CENTER;
            HIT1.Border = 0;
            headInnerTable1.AddCell(HIT1);

            Phrase HIT2 = new Phrase();
            HIT2.Add(new Chunk("اقتباس", arbfntbld));

            HIT1 = new PdfPCell(HIT2);
            HIT1.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            HIT1.HorizontalAlignment = Element.ALIGN_CENTER;
            HIT1.Border = 0;
            headInnerTable1.AddCell(HIT1);
            PdfPCell sub04 = new PdfPCell(headInnerTable1);
            sub04.Border = PdfPCell.TOP_BORDER;
            sub04.HorizontalAlignment = Element.ALIGN_CENTER;
            Subhead.AddCell(sub04);
            /*End of Row*/


            Phrase ph1 = new Phrase();
            ph1.Add(new Chunk("Customer : " + dt_cust.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
            if (dt_cust.Rows[0]["Address"].ToString() != "")
            {
                ph1.Add(new Chunk("\n", arbfnt));
                ph1.Add(new Chunk(dt_cust.Rows[0]["Address"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                ph1.Add(new Chunk("\n", arbfnt));
            }
            if (dt_cust.Rows[0]["TRN"].ToString() != "" )
            {
                ph1.Add(new Chunk("\n", arbfnt));
                ph1.Add(new Chunk(dt_cust.Rows[0]["TRN"].ToString() + " :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                ph1.Add(new Chunk("رقم ضريبي", arbfnt));
                ph1.Add(new Chunk(" / Customer TRN", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            }
            PdfPCell sub30 = new PdfPCell(ph1);
            sub30.Border = 0;
            sub30.HorizontalAlignment = Element.ALIGN_RIGHT;
            sub30.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(sub30);


            System.IO.MemoryStream mem = new MemoryStream();
            Barcode128 barImg = new Barcode128();
            barImg.Code = dt_inv.Rows[0]["Code"].ToString();
            barImg.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White).Save(mem, System.Drawing.Imaging.ImageFormat.Png);
            iTextSharp.text.Image imgs = iTextSharp.text.Image.GetInstance(mem.ToArray());
            mem.Flush();
            mem.Close();

            PdfPCell sub13 = new PdfPCell(imgs);
            sub13.Border = 0;
            sub13.MinimumHeight = 50f;
            sub13.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            sub13.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            Subhead.AddCell(sub13);
            /*End of Row*/

            ph1 = new Phrase();
            ph1.Add(new Chunk(dt_cust.Rows[0]["Mobile_num"].ToString() + " :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("موبايل", arbfnt));
            ph1.Add(new Chunk(" / Mob", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            if (dt_cust.Rows[0]["Phone_num"].ToString() != "")
            {
                ph1.Add(new Chunk("\n", arbfnt));
                ph1.Add(new Chunk(dt_cust.Rows[0]["Phone_num"].ToString() + " :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                ph1.Add(new Chunk("هاتف", arbfnt));
                ph1.Add(new Chunk(" / Tel", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            }
            sub30 = new PdfPCell(ph1);
            sub30.Border = 0;
            sub30.HorizontalAlignment = Element.ALIGN_RIGHT;
            sub30.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(sub30);

            ph1 = new Phrase();
            ph1.Add(new Chunk(dt_inv.Rows[0]["Code"].ToString()+" :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("سؤال رقم", arbfnt));
            ph1.Add(new Chunk(" / Quotation No", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            PdfPCell sub23 = new PdfPCell(ph1);
            sub23.Border = 0;
            sub23.HorizontalAlignment = Element.ALIGN_LEFT;
            sub23.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(sub23);

            ph1 = new Phrase();
            ph1.Add(new Chunk(dt_cust.Rows[0]["Email"].ToString() + " :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("ايميل", arbfnt));
            ph1.Add(new Chunk(" / Email", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            PdfPCell sub40 = new PdfPCell(ph1);
            sub40.Border = 0;
            sub40.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            sub40.HorizontalAlignment = Element.ALIGN_RIGHT;
            Subhead.AddCell(sub40);

            ph1 = new Phrase();
            ph1.Add(new Chunk(dt_inv.Rows[0]["QuotationDates"].ToString()+" :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("تاريخ الاقتباس", arbfnt));
            ph1.Add(new Chunk(" / Date", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            PdfPCell sub33 = new PdfPCell(ph1);
            sub33.Border = 0;
            sub33.HorizontalAlignment = Element.ALIGN_LEFT;
            sub33.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(sub33);


            if (dt_inv.Rows[0]["Subject"].ToString() != "")
            {
                PdfPCell sub34 = new PdfPCell(new Phrase("Subject : " + dt_inv.Rows[0]["Subject"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                sub34.Border = 0;
                sub34.MinimumHeight = 15f;
                sub34.VerticalAlignment = Rectangle.ALIGN_BOTTOM;
                sub34.Colspan = 2;
                sub34.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub34);
            }


            PdfPCell EmptyWithBottomBorder = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            EmptyWithBottomBorder.Border = PdfPCell.BOTTOM_BORDER;
            EmptyWithBottomBorder.HorizontalAlignment = Element.ALIGN_LEFT;

            document.Add(Subhead);

            #endregion

            #region data

            if (dt_invD.Rows.Count > 0)
            {
                PdfPTable emp_details = new PdfPTable(8);
                emp_details.DefaultCell.Padding = 4;
                float[] widthsdet = new float[] { 5f, 33f, 9f, 9f, 9f, 7f, 7f, 11f };
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
                ph1.Add(new Chunk("VAT\n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                ph1.Add(new Chunk("ضريبة", arbsmallbold));
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
                        //PdfPCell REM = new PdfPCell(new Phrase(rows["Name"].ToString() + rows["ParticularsD"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        //REM.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        //REM.Border = 0;
                        //emp_details.AddCell(REM);

                        PdfPTable intble = new PdfPTable(1);
                        intble.DefaultCell.Border = 0;
                        intble.WidthPercentage = 95f;
                        intble.SpacingAfter = 2f;

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
                        //REM.Colspan = 2;
                        REM.Border = 0;
                        emp_details.AddCell(REM);
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
                DetailEnd00.Colspan = 2;
                DetailEnd00.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                DetailEnd00.Border = PdfPCell.TOP_BORDER;
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

                PdfPTable totalexp = new PdfPTable(2);
                totalexp.DefaultCell.Padding = 4;
                float[] widths1 = new float[] { 70f,30f };
                totalexp.SetWidths(widths1);
                totalexp.SpacingBefore = 10;
                totalexp.WidthPercentage = 95f;

                ph1 = new Phrase();
                ph1.Add(new Chunk("AED " + ConvertNumbertoWords(Convert.ToDecimal(dt_sum.Rows[0]["Total"])) + " Only :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLDITALIC)));
                ph1.Add(new Chunk("المجموع الاجمالي", arbfnt));
                ph1.Add(new Chunk(" / Grand Total", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLDITALIC)));
                PdfPCell toddddt2wwDD = new PdfPCell(ph1);
                toddddt2wwDD.Border = 0;
                toddddt2wwDD.Colspan = 2;
                toddddt2wwDD.HorizontalAlignment = Element.ALIGN_RIGHT;
                toddddt2wwDD.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                totalexp.AddCell(toddddt2wwDD);

                ph1 = new Phrase();
                ph1.Add(new Chunk("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLDITALIC)));
                 toddddt2wwDD = new PdfPCell(ph1);
                toddddt2wwDD.Border = 0;
                toddddt2wwDD.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(toddddt2wwDD);

                ph1 = new Phrase();
                ph1.Add(new Chunk("Created by", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                toddddt2wwDD = new PdfPCell(ph1);
                toddddt2wwDD.Border = 0;
                toddddt2wwDD.HorizontalAlignment = Element.ALIGN_CENTER;
                totalexp.AddCell(toddddt2wwDD);

                ph1 = new Phrase();
                ph1.Add(new Chunk("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLDITALIC)));
                toddddt2wwDD = new PdfPCell(ph1);
                toddddt2wwDD.Border = 0;
                toddddt2wwDD.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(toddddt2wwDD);

                ph1 = new Phrase();
                ph1.Add(new Chunk(dt_inv.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                toddddt2wwDD = new PdfPCell(ph1);
                toddddt2wwDD.Border = 0;
                toddddt2wwDD.HorizontalAlignment = Element.ALIGN_CENTER;
                totalexp.AddCell(toddddt2wwDD);

                if (dt_inv.Rows[0]["Remarks"].ToString() != "")
                {
                    PdfPCell totw1wpsss = new PdfPCell(new Phrase("Remark : " + dt_inv.Rows[0]["Remarks"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    totw1wpsss.Border = 0;
                    totw1wpsss.MinimumHeight = 25f;
                    totw1wpsss.VerticalAlignment = Rectangle.ALIGN_BOTTOM;
                    totw1wpsss.Colspan = 2;
                    totw1wpsss.HorizontalAlignment = Element.ALIGN_LEFT;
                    totalexp.AddCell(totw1wpsss);
                }

                document.Add(totalexp);

                if (dtGenSetting.Rows[0]["PrintFooter"].ToString() != "")
                {

                   

                    PdfPTable footer = new PdfPTable(1);
                    footer.DefaultCell.Padding = 4;
                    footer.SpacingAfter = 20f;
                    footer.TotalWidth = document.PageSize.Width - (2 * document.LeftMargin);
                    footer.WidthPercentage = 90f;

                    string imageURL = Server.MapPath("../UploadedImage/" + dtGenSetting.Rows[0]["PrintFooter"].ToString());
                    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(new Uri(imageURL));
                    jpg.ScaleToFit(470f, 450f);

                    PdfPCell Fotservice = new PdfPCell(jpg, true);
                    Fotservice.Border = 0;
                    //Fotservice.FixedHeight = jpg.Height;
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

                document.Close();
                HttpContext.Current.Response.Write(document);
                HttpContext.Current.Response.End();
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