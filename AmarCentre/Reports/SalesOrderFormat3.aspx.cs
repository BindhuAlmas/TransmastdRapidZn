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
    public partial class SalesOrderFormat3 : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();
        Master_Bal obj_master = new Master_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"]);
            DataSet ds = obj_report.SalesOrderPrintFormat2(id);
            DataTable dt_inv = ds.Tables[0];
            DataTable dt_invD = ds.Tables[1];
            DataTable dt_cust = ds.Tables[2];
            DataTable dt_sum = ds.Tables[3];
            DataTable dtGeneral = ds.Tables[4];
            bool PrintTerms = Convert.ToBoolean(ds.Tables[4].Rows[0]["PrintTerms"]);
            Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=SalesOrderPrint.pdf");
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
            // BaseFont bfTimes = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Font/arialuni.ttf"), BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font arbfnt = new iTextSharp.text.Font(bfTimes, 10, Font.NORMAL);
            iTextSharp.text.Font arbsmallbold = new iTextSharp.text.Font(bfTimes, 8, Font.BOLD);
            iTextSharp.text.Font arbfntbld = new iTextSharp.text.Font(bfTimes, 15, Font.BOLD);

            #region header

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

            PdfPCell HIT1 = new PdfPCell(new Phrase("SALES ORDER", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            HIT1.HorizontalAlignment = Element.ALIGN_CENTER;
            HIT1.Border = 0;
            headInnerTable1.AddCell(HIT1);

            Phrase HIT2 = new Phrase();
            HIT2.Add(new Chunk("أمر البيع", arbfntbld));

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
            if (dt_cust.Rows[0]["TRN"].ToString() != "")
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
            ph1.Add(new Chunk(dt_inv.Rows[0]["Code"].ToString() + " :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("رقم أمر البيع", arbfnt));
            ph1.Add(new Chunk(" / Sales Order No", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
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
            sub40.HorizontalAlignment = Element.ALIGN_RIGHT;
            sub40.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(sub40);

            ph1 = new Phrase();
            ph1.Add(new Chunk(dt_inv.Rows[0]["SalesOrderDates"].ToString() + " :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("تاريخ أمر البيع", arbfnt));
            ph1.Add(new Chunk(" / Date", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            PdfPCell sub33 = new PdfPCell(ph1);
            sub33.Border = 0;
            sub33.HorizontalAlignment = Element.ALIGN_LEFT;
            sub33.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(sub33);

            PdfPCell EmptyWithBottomBorder = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            EmptyWithBottomBorder.Border = PdfPCell.BOTTOM_BORDER;
            EmptyWithBottomBorder.HorizontalAlignment = Element.ALIGN_LEFT;

            document.Add(Subhead);

            #endregion

            #region data

            if (dt_invD.Rows.Count > 0)
            {
                /*Tax Invoice Type*/
                if (dt_inv.Rows[0]["InvoiceType"].ToString() == "1")
                {
                    PdfPTable emp_details = new PdfPTable(9);
                    emp_details.DefaultCell.Padding = 4;
                    float[] widthsdet = new float[] { 5f, 30f, 13f, 9f, 9f,6f, 8f, 7f, 11f };
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
                    ph1.Add(new Chunk("Amount\n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("المبلغ", arbsmallbold));
                    PdfPCell DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH04);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("VAT % \n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //ph1.Add(new Chunk("", arbsmallbold));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    //DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH04);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("VAT Amount\n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
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
                    ph1.Add(new Chunk("Total Amount\n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("المبلغ الإجمالي", arbsmallbold));
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
                            PdfPCell DT = new PdfPCell(new Phrase(rows["AfterDiscount_Price"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
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
                            PdfPCell DT = new PdfPCell(new Phrase(rows["AfterTaxAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
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
                            if (dt_inv.Rows[0]["DisplDiscount"].ToString() == "1" & rows["AfterDiscount_TotalSO"].ToString() != "")
                            {
                                PdfPCell DT = new PdfPCell(new Phrase(rows["AfterDiscount_TotalSO"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                                DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                DT.Border = 0;
                                emp_details.AddCell(DT);
                            }
                            else
                            {
                                PdfPCell DT = new PdfPCell(new Phrase(rows["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                                DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                DT.Border = 0;
                                emp_details.AddCell(DT);
                            }
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
                    DetailEnd00.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailEnd00.Border = PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailEnd00);

                    DetailEnd00 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TQuantity"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    DetailEnd00.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    DetailEnd00.Border = PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailEnd00);

                    PdfPCell tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TAfterDiscount_Price"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
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

                    tot1 = new PdfPCell(new Phrase((dt_inv.Rows[0]["DisplDiscount"].ToString() == "1" & dt_sum.Rows[0]["AfterDiscount_Total"].ToString() != "") ? dt_sum.Rows[0]["AfterDiscount_Total"].ToString() : dt_sum.Rows[0]["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    tot1.Border = PdfPCell.TOP_BORDER;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(tot1);

                    document.Add(emp_details);
                }
                else if (dt_inv.Rows[0]["InvoiceType"].ToString() == "2")/*Normal Invoice Type*/
                {
                    PdfPTable emp_details = new PdfPTable(7);
                    emp_details.DefaultCell.Padding = 4;
                    float[] widthsdet = new float[] { 5f, 30f, 13f, 10f, 10f,  10f, 12f };
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
                    ph1.Add(new Chunk("Amount\n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("المبلغ", arbsmallbold));
                    PdfPCell DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH04);
                    //ph1 = new Phrase();
                    //ph1.Add(new Chunk("Discount\n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //ph1.Add(new Chunk("خصم", arbsmallbold));
                    //DetailH04 = new PdfPCell(ph1);
                    //DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    //DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    //DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    //emp_details.AddCell(DetailH04);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Fine\n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("مبلغ الغرامة", arbsmallbold));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH04);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Total Amount\n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("المبلغ الإجمالي", arbsmallbold));
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
                            PdfPCell DT = new PdfPCell(new Phrase(rows["AfterDiscount_Price"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
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
                        //    PdfPCell DT = new PdfPCell(new Phrase(rows["SalesOrderDiscount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
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
                            if (dt_inv.Rows[0]["DisplDiscount"].ToString() == "1" & rows["AfterDiscount_TotalSO"].ToString() != "")
                            {
                                PdfPCell DT = new PdfPCell(new Phrase(rows["AfterDiscount_TotalSO"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                                DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                DT.Border = 0;
                                emp_details.AddCell(DT);
                            }
                            else
                            {
                                PdfPCell DT = new PdfPCell(new Phrase(rows["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                                DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                DT.Border = 0;
                                emp_details.AddCell(DT);
                            }
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
                    DetailEnd00.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailEnd00.Border = PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailEnd00);

                    DetailEnd00 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TQuantity"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    DetailEnd00.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    DetailEnd00.Border = PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailEnd00);

                    PdfPCell tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TAfterDiscount_Price"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    tot1.Border = PdfPCell.TOP_BORDER;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(tot1);

                    //tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["SalesOrderTDiscount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    //tot1.Border = PdfPCell.TOP_BORDER;
                    //tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //emp_details.AddCell(tot1);

                    tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TFine"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    tot1.Border = PdfPCell.TOP_BORDER;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(tot1);

                    tot1 = new PdfPCell(new Phrase((dt_inv.Rows[0]["DisplDiscount"].ToString() == "1" & dt_sum.Rows[0]["AfterDiscount_Total"].ToString() != "") ? dt_sum.Rows[0]["AfterDiscount_Total"].ToString() : dt_sum.Rows[0]["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
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

                decimal GrandtotalFinal = Convert.ToDecimal((dt_inv.Rows[0]["DisplDiscount"].ToString() == "1" & dt_sum.Rows[0]["AfterDiscount_Total"].ToString() != "") ? dt_sum.Rows[0]["AfterDiscount_Total"].ToString() : dt_sum.Rows[0]["Total"].ToString());

                if (dt_inv.Rows[0]["ChargedAmount"].ToString() != "0.00")
                {
                    GrandtotalFinal = GrandtotalFinal + Convert.ToDecimal(dt_inv.Rows[0]["ChargedAmount"].ToString());
                    totalexp.AddCell(Empty);

                    PdfPCell totw1w = new PdfPCell(new Phrase("Charged Amount :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    totw1w.Border = 0;
                    totw1w.HorizontalAlignment = Element.ALIGN_LEFT;
                    totalexp.AddCell(totw1w);

                    PdfPCell tot2ww = new PdfPCell(new Phrase(dt_inv.Rows[0]["ChargedAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    tot2ww.Border = 0;
                    tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot2ww);

                    totalexp.AddCell(Empty);

                    totw1w = new PdfPCell(new Phrase("Total :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    totw1w.Border = 0;
                    totw1w.HorizontalAlignment = Element.ALIGN_LEFT;
                    totalexp.AddCell(totw1w);

                    tot2ww = new PdfPCell(new Phrase(GrandtotalFinal.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    tot2ww.Border = 0;
                    tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalexp.AddCell(tot2ww);
                }


                ph1 = new Phrase();
                ph1.Add(new Chunk("AED " + ConvertNumbertoWords(GrandtotalFinal) + " Only :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLDITALIC)));
                ph1.Add(new Chunk("المجموع الاجمالي", arbfnt));
                ph1.Add(new Chunk(" / Grand Total", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLDITALIC)));
                PdfPCell toddddt2wwDD = new PdfPCell(ph1);
                toddddt2wwDD.Border = 0;
                toddddt2wwDD.Colspan = 3;
                toddddt2wwDD.HorizontalAlignment = Element.ALIGN_RIGHT;
                toddddt2wwDD.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                totalexp.AddCell(toddddt2wwDD);

                document.Add(totalexp);

                if (dtGeneral.Rows[0]["IsAddRemark"].ToString() != "0")
                {
                    PdfPTable terms = new PdfPTable(3);
                    terms.DefaultCell.Padding = 4;
                    float[] widthsTerms = new float[] { 50f, 25f, 25f };
                    terms.SetWidths(widthsTerms);
                    terms.SpacingBefore = 10;
                    terms.WidthPercentage = 95f;

                    PdfPCell EmptyTerms = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    EmptyTerms.HorizontalAlignment = Element.ALIGN_LEFT;

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
                    terms.AddCell(remarksCell);

                    document.Add(terms);

                }

                if (PrintTerms)
                {
                    PdfPTable terms = new PdfPTable(5);
                    terms.DefaultCell.Padding = 4;
                    float[] widthsTerms = new float[] { 23f, 12f, 15f, 24f, 24f };
                    terms.SetWidths(widthsTerms);
                    terms.SpacingBefore = 10;
                    terms.WidthPercentage = 95f;

                    ph1 = new Phrase();
                    ph1.Add(new Chunk(":", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    ph1.Add(new Chunk("الاحكام والشروط", arbsmallbold));
                    ph1.Add(new Chunk(" / Terms and Conditions", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    PdfPCell termsCell = new PdfPCell(ph1);
                    termsCell.Border = PdfPCell.BOTTOM_BORDER;
                    termsCell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    termsCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    termsCell.Colspan = 2;
                    terms.AddCell(termsCell);
                    PdfPCell EmptyTerms = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    EmptyTerms.Border = 0;
                    EmptyTerms.HorizontalAlignment = Element.ALIGN_LEFT;
                    EmptyTerms.Colspan = 3;
                    terms.AddCell(EmptyTerms);
                    /*End of Row*/

                    termsCell = new PdfPCell(new Phrase("This is only a sales order and not a final invoice.", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                    termsCell.Border = 0;
                    termsCell.Colspan = 3;
                    termsCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    terms.AddCell(termsCell);
                    EmptyTerms = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                    EmptyTerms.Border = 0;
                    EmptyTerms.HorizontalAlignment = Element.ALIGN_LEFT;
                    EmptyTerms.Colspan = 2;
                    terms.AddCell(EmptyTerms);
                    /*End of Row*/
                    termsCell = new PdfPCell(new Phrase("Cash refundable only within 10 working days, days from issue " +
                        "of sales order if not used.", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                    termsCell.Border = 0;
                    termsCell.Colspan = 3;
                    termsCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    terms.AddCell(termsCell);
                    EmptyTerms = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    EmptyTerms.Border = 0;
                    EmptyTerms.HorizontalAlignment = Element.ALIGN_LEFT;
                    EmptyTerms.Colspan = 2;
                    terms.AddCell(EmptyTerms);

                    document.Add(terms);
                }
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
                float jhgt = footer.GetRowHeight(0) + 40;

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