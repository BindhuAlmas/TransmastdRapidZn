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
    public partial class TaxInvoiceFormat7 : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();
        Master_Bal obj_master = new Master_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"]);
            DataSet ds = obj_report.TaxInvoicePrintFormat2(id);
            DataTable dt_inv = ds.Tables[0];
            DataTable dt_invD = ds.Tables[1];
            DataTable dt_cust = ds.Tables[2];
            DataTable dt_sum = ds.Tables[3];
            DataTable dtGeneral = ds.Tables[4];

            Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", dt_inv.Rows[0]["InvoiceType"].ToString() == "1" ? "inline;filename=TaxInvoicePrint.pdf" : "inline;filename=InvoicePrint.pdf");
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
            iTextSharp.text.Font arbfnt = new iTextSharp.text.Font(bfTimes, 10, Font.NORMAL);
            iTextSharp.text.Font arbsmallbold = new iTextSharp.text.Font(bfTimes, 8, Font.BOLD);
            iTextSharp.text.Font arbfntbld = new iTextSharp.text.Font(bfTimes, 15, Font.BOLD);
            #region header

            PdfPTable Subhead = new PdfPTable(2);
            Subhead.DefaultCell.Padding = 4;
            float[] widths = new float[] { 65f, 35f };
            Subhead.SetWidths(widths);
            Subhead.WidthPercentage = 97f;


            PdfPCell Empty = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            //Empty.Border = 0;
            Empty.HorizontalAlignment = Element.ALIGN_LEFT;

            PdfPCell EmptyWithTopBorder = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            EmptyWithTopBorder.Border = PdfPCell.TOP_BORDER;
            EmptyWithTopBorder.HorizontalAlignment = Element.ALIGN_LEFT;

            PdfPCell sub00 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub00.Border = PdfPCell.TOP_BORDER;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            Phrase ph1 = new Phrase();
            ph1.Add(new Chunk(dt_inv.Rows[0]["InvoiceType"].ToString() == "1" ? "TAX INVOICE\n" : "INVOICE\n", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            if (dt_inv.Rows[0]["InvoiceType"].ToString() == "1")
                ph1.Add(new Chunk("فاتورة ضريبية", arbfntbld));
            else if (dt_inv.Rows[0]["InvoiceType"].ToString() == "2")
                ph1.Add(new Chunk("فاتورة", arbfntbld));
            if (dt_inv.Rows[0]["InvoiceTRN"].ToString() != "")
                ph1.Add(new Chunk("\n\nTRN : " + dt_inv.Rows[0]["InvoiceTRN"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));

            PdfPCell sub04 = new PdfPCell(ph1);
            sub04.Border = PdfPCell.TOP_BORDER;
            sub04.HorizontalAlignment = Element.ALIGN_CENTER;
            sub04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(sub04);
            /*End of Row*/

            ph1 = new Phrase();
            ph1.Add(new Chunk("Customer : " + dt_cust.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
            if (dt_cust.Rows[0]["Address"].ToString() != "")
            {
                ph1.Add(new Chunk("\n", arbfnt));
                ph1.Add(new Chunk(dt_cust.Rows[0]["Address"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                ph1.Add(new Chunk("\n", arbfnt));
            }
            if (dt_cust.Rows[0]["TRN"].ToString() != "" && dt_inv.Rows[0]["InvoiceType"].ToString() == "1")
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
            ph1.Add(new Chunk("رقم الفاتورة", arbfnt));
            ph1.Add(new Chunk(" / Invoice No", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            PdfPCell sub23 = new PdfPCell(ph1);
            sub23.Border = 0;
            sub23.HorizontalAlignment = Element.ALIGN_RIGHT;
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
            ph1.Add(new Chunk(dt_inv.Rows[0]["Dates"].ToString() + " :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("تاريخ الفاتورة", arbfnt));
            ph1.Add(new Chunk(" / Invoice Date", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            PdfPCell sub33 = new PdfPCell(ph1);
            sub33.Border = 0;
            sub33.HorizontalAlignment = Element.ALIGN_RIGHT;
            sub33.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(sub33);

            if (dt_inv.Rows[0]["Subject"].ToString() != "")
            {
                sub00 = new PdfPCell(new Phrase("Subject : " + dt_inv.Rows[0]["Subject"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                sub00.Border = 0;
                sub00.Colspan = 2;
                sub00.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub00);
            }

            document.Add(Subhead);

            #endregion

            #region data

            if (dt_invD.Rows.Count > 0)
            {
                /*Tax Invoice Type*/
                if (dt_inv.Rows[0]["InvoiceType"].ToString() == "1")
                {
                    PdfPTable emp_details = new PdfPTable(10);
                    emp_details.DefaultCell.Padding = 4;
                    float[] widthsdet = new float[] { 5f, 30f, 13f, 9f, 9f, 9f, 7f, 8f, 7f, 11f };
                    emp_details.SetWidths(widthsdet);
                    emp_details.SpacingBefore = 10f;
                    emp_details.WidthPercentage = 95f;
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("No\n", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    ph1.Add(new Chunk("رقم", arbsmallbold));
                    PdfPCell DetailH01 = new PdfPCell(ph1);
                    DetailH01.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH01.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH01.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH01);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Type of Transactions\n", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    ph1.Add(new Chunk("نوع المعاملة", arbsmallbold));
                    PdfPCell DetailH02 = new PdfPCell(ph1);
                    DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                    //DetailH02.Colspan = 2;
                    DetailH02.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH02.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH02);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Particulars\n", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    ph1.Add(new Chunk("تفاصيل", arbsmallbold));
                    DetailH02 = new PdfPCell(ph1);
                    DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                    //DetailH02.Colspan = 2;
                    DetailH02.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH02.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH02);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Persons\n", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    ph1.Add(new Chunk("الأشخاص", arbsmallbold));
                    PdfPCell DetailH03 = new PdfPCell(ph1);
                    DetailH03.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH03.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH03.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH03);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Amount\n", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    ph1.Add(new Chunk("المبلغ", arbsmallbold));
                    PdfPCell DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH04);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Discount\n", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    ph1.Add(new Chunk("خصم", arbsmallbold));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH04);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("VAT %\n", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH04);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("VAT Amount\n", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    ph1.Add(new Chunk("ضريبة", arbsmallbold));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH04);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Fine\n", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    ph1.Add(new Chunk("مبلغ الغرامة", arbsmallbold));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH04);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Total Amount\n", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
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
                            REM.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
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
                            PdfPCell DT = new PdfPCell(new Phrase(rows["TotalAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
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

                    PdfPCell tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    tot1.Border = PdfPCell.TOP_BORDER;
                    tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(tot1);

                    tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TDiscount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
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
                else if (dt_inv.Rows[0]["InvoiceType"].ToString() == "2")/*Normal Invoice Type*/
                {
                    PdfPTable emp_details = new PdfPTable(8);
                    emp_details.DefaultCell.Padding = 4;
                    float[] widthsdet = new float[] { 5f, 30f, 13f, 10f, 10f, 10f, 10f, 12f };
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
                    ph1.Add(new Chunk("Total Amount\n", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk("المبلغ الإجمالي", arbsmallbold));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    emp_details.AddCell(DetailH04);
                    //PdfPCell DetailH01 = new PdfPCell(new Phrase("No", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //DetailH01.HorizontalAlignment = Element.ALIGN_CENTER;
                    //DetailH01.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    //emp_details.AddCell(DetailH01);
                    //PdfPCell DetailH02 = new PdfPCell(new Phrase("Type of Transactions", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                    ////DetailH02.Colspan = 2;
                    //DetailH02.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    //emp_details.AddCell(DetailH02);
                    //PdfPCell DetailH03 = new PdfPCell(new Phrase("Persons", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //DetailH03.HorizontalAlignment = Element.ALIGN_CENTER;
                    //DetailH03.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    //emp_details.AddCell(DetailH03);
                    //PdfPCell DetailH04 = new PdfPCell(new Phrase("Fees", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    //DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    //emp_details.AddCell(DetailH04);
                    //DetailH04 = new PdfPCell(new Phrase("Typing Fees", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    //DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    //emp_details.AddCell(DetailH04);
                    //DetailH04 = new PdfPCell(new Phrase("Discount", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    //DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    //emp_details.AddCell(DetailH04);
                    //DetailH04 = new PdfPCell(new Phrase("VAT", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    //DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    //emp_details.AddCell(DetailH04);
                    //DetailH04 = new PdfPCell(new Phrase("Amount", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    //DetailH04.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    //emp_details.AddCell(DetailH04);

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
                            PdfPCell DT = new PdfPCell(new Phrase(rows["TotalAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
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

                    PdfPCell tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
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
                float[] widths1 = new float[] { 70f, 15f, 10f };
                totalexp.SetWidths(widths1);
                totalexp.SpacingBefore = 10;
                totalexp.WidthPercentage = 95f;

                decimal OBRec = Convert.ToDecimal(dt_cust.Rows[0]["Receivable"]);
                decimal OBPay = Convert.ToDecimal(dt_cust.Rows[0]["Payable"]);
                decimal OB = 0, TA = 0;
                if (OBPay > 0)
                {
                    OB = -OBPay;
                }
                else if (OBRec > 0)
                {
                    OB = OBRec - Convert.ToDecimal(dt_sum.Rows[0]["Total"]);
                }
                TA = OB + Convert.ToDecimal(dt_sum.Rows[0]["Total"]);

                ph1 = new Phrase();
                ph1.Add(new Chunk("AED " + ConvertNumbertoWords(Convert.ToDecimal(dt_sum.Rows[0]["Total"])) + " Only :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLDITALIC)));
                ph1.Add(new Chunk("المجموع الاجمالي", arbfnt));
                ph1.Add(new Chunk(" / Grand Total", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLDITALIC)));
                PdfPCell toddddt2wwDD = new PdfPCell(ph1);
                //PdfPCell toddddt2wwDD = new PdfPCell(new Phrase("Grand Total: " + ConvertNumbertoWords(Convert.ToDecimal(dt_sum.Rows[0]["Total"])) + " Only", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLDITALIC)));
                toddddt2wwDD.Border = 0;
                //toddddt2wwDD.Colspan = 3;
                toddddt2wwDD.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                toddddt2wwDD.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(toddddt2wwDD);

                ph1 = new Phrase();
                ph1.Add(new Chunk("Old Balance : " , new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                sub40 = new PdfPCell(ph1);
                sub40.Border = 0;
                //sub40.Colspan = 2;
                //sub40.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                sub40.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(sub40);
                ph1 = new Phrase();
                ph1.Add(new Chunk( OB.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                sub40 = new PdfPCell(ph1);
                sub40.Border = 0;
                sub40.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(sub40);

                ph1 = new Phrase();
                ph1.Add(new Chunk("Total Amount : ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                sub40 = new PdfPCell(ph1);
                sub40.Border = 0;
                sub40.Colspan = 2;
                //sub40.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                sub40.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(sub40);
                ph1 = new Phrase();
                ph1.Add(new Chunk(TA.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                sub40 = new PdfPCell(ph1);
                sub40.Border = 0;
                sub40.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(sub40);

                document.Add(totalexp);

                #region Remark

                PdfPTable terms = new PdfPTable(3);
                terms.DefaultCell.Padding = 4;
                float[] widthsTerms = new float[] { 50f, 25f, 25f };
                terms.SetWidths(widthsTerms);
                terms.SpacingBefore = 10;
                terms.WidthPercentage = 95f;

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

                document.Add(terms);

                #endregion
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