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
    public partial class TaxInvoiceFormat14 : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();
        Master_Bal obj_master = new Master_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime currentDateTime = DateTime.Now;
            int id = Convert.ToInt32(Request.QueryString["Id"]);
            DataSet ds = obj_report.TaxInvoicePrint(id);
            DataTable dt_inv = ds.Tables[0];
            DataTable dt_invD = ds.Tables[1];
            DataTable dt_cust = ds.Tables[2];
            DataTable dt_sum = ds.Tables[3];
            DataTable dtGeneral = ds.Tables[4];

            Document document = new Document(PageSize.A4, 20f, 20f, 0f, 0f);

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
            iTextSharp.text.Font arbsmallbold = new iTextSharp.text.Font(bfTimesV0, 14, Font.NORMAL);
            iTextSharp.text.Font arbfntbld = new iTextSharp.text.Font(bfTimesV0, 16, Font.NORMAL);
            iTextSharp.text.Font arbfntbldN = new iTextSharp.text.Font(bfTimes, 14, Font.NORMAL);

            #region header

            PdfPTable table1 = new PdfPTable(1);
            table1.DefaultCell.Padding = 4;
            float[] widthsset = new float[] { 35f };
            table1.SetWidths(widthsset);
            table1.WidthPercentage = 35f;

            Phrase ph1 = new Phrase();
            ph1.Add(new Chunk(dt_inv.Rows[0]["InvoiceType"].ToString() == "1" ? "فاتورة ضريبية " : " فاتورة ", arbfntbldN));
            if (dt_inv.Rows[0]["InvoiceType"].ToString() == "1")
                ph1.Add(new Chunk("Tax Invoice ", arbfntbldN));
            else if (dt_inv.Rows[0]["InvoiceType"].ToString() == "2")
                ph1.Add(new Chunk(" Invoice ", arbfntbldN));

            //Phrase ph1 = new Phrase();
            //ph1.Add(new Chunk(" فاتورة ", arbfntbldN));
            //    ph1.Add(new Chunk(" Invoice ", arbfntbldN));

            PdfPCell sub04 = new PdfPCell(ph1);
            sub04.MinimumHeight = 25f;
            sub04.BorderWidth = 1;
           
            sub04.HorizontalAlignment = Element.ALIGN_CENTER;
            sub04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            sub04.Border = PdfPCell.BOTTOM_BORDER;

            table1.AddCell(sub04);

            PdfPCell EmptyCell2 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            EmptyCell2.Border = 0;
            EmptyCell2.PaddingBottom = 15f;
            EmptyCell2.HorizontalAlignment = Element.ALIGN_LEFT;
           
            EmptyCell2.Colspan = 2;
            table1.AddCell(EmptyCell2);

            document.Add(table1);

            /////////////////////////////////////////////////////

            PdfPTable Subhead = new PdfPTable(4);
            Subhead.DefaultCell.Padding = 4;
            float[] widths = new float[] { 25f, 25f, 25f, 25f };
            Subhead.SetWidths(widths);
            Subhead.WidthPercentage = 100f;

            System.IO.MemoryStream mem = new MemoryStream();
            Barcode128 barImg = new Barcode128();
            barImg.Code = dt_inv.Rows[0]["Code"].ToString();
            barImg.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White).Save(mem, System.Drawing.Imaging.ImageFormat.Png);
            iTextSharp.text.Image imgs = iTextSharp.text.Image.GetInstance(mem.ToArray());
            mem.Flush();
            mem.Close();

            PdfPCell Cuscell22 = new PdfPCell(imgs);
            Cuscell22.MinimumHeight = 50f;
            Cuscell22.PaddingLeft = 5f;
            Cuscell22.Colspan = 2;
            Cuscell22.Border=PdfPCell.LEFT_BORDER| PdfPCell.TOP_BORDER|PdfPCell.RIGHT_BORDER;
            Cuscell22.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            Cuscell22.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            Subhead.AddCell(Cuscell22);

            PdfPCell EmptyCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            EmptyCell.Border = 0;
            EmptyCell.PaddingLeft = 5f;
            EmptyCell.HorizontalAlignment = Element.ALIGN_LEFT;
            EmptyCell.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER| PdfPCell.TOP_BORDER;
            EmptyCell.Colspan = 2;
            Subhead.AddCell(EmptyCell);
         
            if (dt_inv.Rows[0]["InvoiceTRN"].ToString() != "")
            {
                PdfPCell sub122add = new PdfPCell(new Phrase("TRN : ", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                sub122add.MinimumHeight = 15f;
                sub122add.PaddingLeft = 5f;
                sub122add.HorizontalAlignment = Element.ALIGN_LEFT;
                sub122add.Border = PdfPCell.LEFT_BORDER;
                Subhead.AddCell(sub122add);

               PdfPCell sub142 = new PdfPCell(new Phrase(dt_inv.Rows[0]["InvoiceTRN"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
                sub142.HorizontalAlignment = Element.ALIGN_LEFT;
                sub142.MinimumHeight = 15f;
                sub142.PaddingLeft = 5f;
                sub142.HorizontalAlignment = Element.ALIGN_LEFT;
                sub142.Border = PdfPCell.RIGHT_BORDER;
                Subhead.AddCell(sub142);

                sub142 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
                sub142.HorizontalAlignment = Element.ALIGN_LEFT;
                sub142.MinimumHeight = 15f;
                sub142.Colspan = 2;
                sub142.HorizontalAlignment = Element.ALIGN_LEFT;
                sub142.Border = PdfPCell.RIGHT_BORDER;
                Subhead.AddCell(sub142);
            }
           
            PdfPCell sub13 = new PdfPCell(new Phrase("Invoice No :", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            sub13.MinimumHeight = 15f;
            sub13.PaddingLeft = 5f;
            sub13.Border = PdfPCell.LEFT_BORDER;
            sub13.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub13);

            PdfPCell sub14 = new PdfPCell(new Phrase(dt_inv.Rows[0]["Code"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
            sub14.MinimumHeight = 15f;
            sub14.PaddingLeft = 5f;
            sub14.Border = PdfPCell.RIGHT_BORDER;
            sub14.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub14);

            PdfPCell sub21add = new PdfPCell(new Phrase("Customer Ref No :", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            sub21add.MinimumHeight = 15f;
            sub21add.PaddingLeft = 5f;
            sub21add.HorizontalAlignment = Element.ALIGN_LEFT;
            sub21add.Border = PdfPCell.LEFT_BORDER;
            Subhead.AddCell(sub21add);

            PdfPCell sub122trn = new PdfPCell(new Phrase("", new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
            sub122trn.MinimumHeight = 15f;
            sub122trn.PaddingLeft = 5f;
            sub122trn.HorizontalAlignment = Element.ALIGN_LEFT;
            sub122trn.Border = PdfPCell.RIGHT_BORDER;
            Subhead.AddCell(sub122trn);

            PdfPCell sub132 = new PdfPCell(new Phrase("Date :", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            sub132.MinimumHeight = 15f;
            sub132.PaddingLeft = 5f;
            sub132.HorizontalAlignment = Element.ALIGN_LEFT;
            sub132.Border = PdfPCell.LEFT_BORDER;
            Subhead.AddCell(sub132);

            PdfPCell sub144 = new PdfPCell(new Phrase(currentDateTime.ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
            sub144.MinimumHeight = 15f;
            sub144.PaddingLeft = 5f;
            sub144.HorizontalAlignment = Element.ALIGN_LEFT;
            sub144.Border = PdfPCell.RIGHT_BORDER;
            Subhead.AddCell(sub144);

            PdfPCell sub1 = new PdfPCell(new Phrase("Customer Name :", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            sub1.MinimumHeight = 15f;
            sub1.PaddingLeft = 5f;
            sub1.HorizontalAlignment = Element.ALIGN_LEFT;
            sub1.Border = PdfPCell.LEFT_BORDER;
            Subhead.AddCell(sub1);

            PdfPCell sub12 = new PdfPCell(new Phrase(dt_cust.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
            sub12.MinimumHeight = 15f;
            sub12.PaddingLeft = 5f;
            sub12.HorizontalAlignment = Element.ALIGN_LEFT;
            sub12.Border = PdfPCell.RIGHT_BORDER;
            Subhead.AddCell(sub12);

            sub132 = new PdfPCell(new Phrase("Invoiced At :", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            sub132.MinimumHeight = 15f;
            sub132.PaddingLeft = 5f;
            sub132.HorizontalAlignment = Element.ALIGN_LEFT;
            sub132.Border = PdfPCell.LEFT_BORDER;
            Subhead.AddCell(sub132);

            sub144 = new PdfPCell(new Phrase(dt_inv.Rows[0]["Dates"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
            sub144.MinimumHeight = 15f;
            sub144.PaddingLeft = 5f;
            sub144.HorizontalAlignment = Element.ALIGN_LEFT;
            sub144.Border = PdfPCell.RIGHT_BORDER;
            Subhead.AddCell(sub144);

            sub1 = new PdfPCell(new Phrase("Mobile No :", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            sub1.MinimumHeight = 15f;
            sub1.PaddingLeft = 5f;
            sub1.HorizontalAlignment = Element.ALIGN_LEFT;
            sub1.Border = PdfPCell.LEFT_BORDER;
            Subhead.AddCell(sub1);

            sub12 = new PdfPCell(new Phrase(dt_cust.Rows[0]["Mobile_num"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
            sub12.MinimumHeight = 15f;
            sub12.PaddingLeft = 5f;
            sub12.HorizontalAlignment = Element.ALIGN_LEFT;
            sub12.Border = PdfPCell.RIGHT_BORDER;
            Subhead.AddCell(sub12);


            PdfPCell sub126 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
            sub126.MinimumHeight = 15f;
            sub126.PaddingLeft = 5f;
            sub126.PaddingBottom = 10f;
            sub126.HorizontalAlignment = Element.ALIGN_LEFT;
            sub126.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            sub126.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
            Subhead.AddCell(sub126);

            PdfPCell sub127 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
            sub127.MinimumHeight = 15f;
            sub127.PaddingLeft = 5f;
            sub127.PaddingBottom = 10f;
            sub127.HorizontalAlignment = Element.ALIGN_LEFT;
            sub127.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            sub127.Border =  PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER;
            Subhead.AddCell(sub127);

            if (dt_cust.Rows[0]["TRN"].ToString() != "")
            {
                PdfPCell sub21 = new PdfPCell(new Phrase("TRN :", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                sub21.MinimumHeight = 15f;
                sub21.PaddingLeft = 5f;
                sub21.PaddingBottom = 10f;
                sub21.HorizontalAlignment = Element.ALIGN_LEFT;
                sub21.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                sub21.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                Subhead.AddCell(sub21);

                PdfPCell sub122 = new PdfPCell(new Phrase(dt_cust.Rows[0]["TRN"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
                sub122.MinimumHeight = 15f;
                sub122.PaddingLeft = 5f;
                sub122.PaddingBottom = 10f;
                sub122.HorizontalAlignment = Element.ALIGN_LEFT;
                sub122.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                sub122.Border = PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER;
                Subhead.AddCell(sub122);
            }
            else
            {
                PdfPCell sub21 = new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                sub21.MinimumHeight = 15f;
                sub21.PaddingLeft = 5f;
                sub21.PaddingBottom = 10f;
                sub21.Colspan = 2;
                sub21.HorizontalAlignment = Element.ALIGN_LEFT;
                sub21.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                sub21.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER| PdfPCell.RIGHT_BORDER;
                Subhead.AddCell(sub21);
            }

            document.Add(Subhead);

            #endregion

            ////////////////////////////////////////////////////


            #region data

            if (dt_invD.Rows.Count > 0)
            {
                /*Tax Invoice Type*/
                if (dt_inv.Rows[0]["InvoiceType"].ToString() == "1")
                {
                    PdfPTable emp_details = new PdfPTable(8);

                    emp_details.DefaultCell.Padding = 4;
                    float[] widthsdet = new float[] { 4f, 30f, 12f, 6f, 9f, 8f, 7f, 9f };
                    emp_details.SetWidths(widthsdet);

                    emp_details.SpacingBefore = 20f;
                    emp_details.WidthPercentage = 100f;

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("No\n", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                    ph1.Add(new Chunk("رقم", arbsmallbold));
                    PdfPCell DetailH01 = new PdfPCell(ph1);
                    DetailH01.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH01.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    DetailH01.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailH01);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Service\n", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                    ph1.Add(new Chunk(" الخدمات", arbsmallbold));
                    PdfPCell DetailH02 = new PdfPCell(ph1);
                    DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH02.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    DetailH02.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailH02);


                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Particulars\n", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                    ph1.Add(new Chunk("تفاصيل", arbsmallbold));
                    PdfPCell DetailH02a = new PdfPCell(ph1);
                    DetailH02a.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH02a.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    DetailH02a.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailH02a);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Qty\n", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                    ph1.Add(new Chunk(" كمية ", arbsmallbold));
                    PdfPCell DetailH03 = new PdfPCell(ph1);
                    DetailH03.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH03.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    DetailH03.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailH03);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Govt Fees  & Bank Chrg\n", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                    ph1.Add(new Chunk("", arbsmallbold));
                    PdfPCell DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailH04);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Trans. Charge\n", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                    ph1.Add(new Chunk("", arbsmallbold));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailH04);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Tax\n", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                    ph1.Add(new Chunk("ضريبة", arbsmallbold));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailH04);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Total\n", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                    ph1.Add(new Chunk(" مجموع", arbsmallbold));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
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
                            PdfPCell REM = new PdfPCell(new Phrase(rows["Quantity"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
                            REM.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(REM);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("N/A");
                        }
                        try
                        {
                            decimal gvtfee = (rows["Expense"].ToString() == "" ? 0 : Convert.ToDecimal(rows["Expense"].ToString())) +
                                (rows["Fine"].ToString() == "" ? 0 : Convert.ToDecimal(rows["Fine"].ToString()));

                            PdfPCell DT = new PdfPCell(new Phrase(gvtfee.ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(DT);

                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("");
                        }

                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["TotalServiceCharge"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(DT);

                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("");
                        }

                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["SingleTaxAmount"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(DT);
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
                    //change data from DB
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("المبلغ الإجمالي", arbfnt));
                    ph1.Add(new Chunk(" Total Amount", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                    PdfPCell DetailEnd01 = new PdfPCell(ph1);
                    //DetailEnd00.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DetailEnd01.Colspan = 7;
                    DetailEnd01.MinimumHeight = 20f;
                    DetailEnd01.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailEnd01);

                    DetailEnd01 = new PdfPCell(new Phrase(dt_sum.Rows[0]["NetAmount"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                    DetailEnd01.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    emp_details.AddCell(DetailEnd01);


                    ph1 = new Phrase();

                    ph1.Add(new Chunk(" Total TAX (VAT)5%", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                    DetailEnd01 = new PdfPCell(ph1);
                    //DetailEnd00.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DetailEnd01.Colspan = 7;
                    DetailEnd01.MinimumHeight = 20f;
                    DetailEnd01.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailEnd01);

                    DetailEnd01 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TTAx"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                    DetailEnd01.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    emp_details.AddCell(DetailEnd01);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("صافي المجموع", arbfnt));
                    ph1.Add(new Chunk(" Net Total", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                    DetailEnd01 = new PdfPCell(ph1);
                    //DetailEnd00.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DetailEnd01.Colspan = 7;
                    DetailEnd01.Border = 0;
                    DetailEnd01.MinimumHeight = 20f;
                    DetailEnd01.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailEnd01);

                    DetailEnd01 = new PdfPCell(new Phrase(dt_sum.Rows[0]["Total"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                    DetailEnd01.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(DetailEnd01);

                    ph1 = new Phrase();
                    //ph1.Add(new Chunk("صافي المجموع", arbfnt));
                    ph1.Add(new Chunk(" Paid Amount", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                    DetailEnd01 = new PdfPCell(ph1);
                    //DetailEnd00.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DetailEnd01.Colspan = 7;
                    DetailEnd01.MinimumHeight = 20f;
                    DetailEnd01.Border = 0;
                    DetailEnd01.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailEnd01);

                    DetailEnd01 = new PdfPCell(new Phrase(dt_inv.Rows[0]["Received"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                    DetailEnd01.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(DetailEnd01);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk(" Balance", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                    DetailEnd01 = new PdfPCell(ph1);
                    //DetailEnd00.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DetailEnd01.Colspan = 7;
                    DetailEnd01.MinimumHeight = 20f;
                    DetailEnd01.Border = 0;
                    DetailEnd01.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailEnd01);

                    DetailEnd01 = new PdfPCell(new Phrase(dt_inv.Rows[0]["Receivable"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                    DetailEnd01.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(DetailEnd01);

                    document.Add(emp_details);

                }

                else if (dt_inv.Rows[0]["InvoiceType"].ToString() == "2")/*Normal Invoice Type*/
                {

                    PdfPTable emp_details = new PdfPTable(7);

                    emp_details.DefaultCell.Padding = 4;
                    float[] widthsdet = new float[] { 4f, 30f, 12f, 6f, 9f, 8f, 9f };
                    emp_details.SetWidths(widthsdet);

                    emp_details.SpacingBefore = 20f;
                    emp_details.WidthPercentage = 100f;

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("No\n", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                    ph1.Add(new Chunk("رقم", arbsmallbold));
                    PdfPCell DetailH01 = new PdfPCell(ph1);
                    DetailH01.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH01.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    DetailH01.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailH01);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Service\n", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                    ph1.Add(new Chunk(" الخدمات", arbsmallbold));
                    PdfPCell DetailH02 = new PdfPCell(ph1);
                    DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH02.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    DetailH02.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailH02);


                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Particulars\n", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                    ph1.Add(new Chunk(" تفاصيل", arbsmallbold));
                    PdfPCell DetailH02a = new PdfPCell(ph1);
                    DetailH02a.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH02a.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    DetailH02a.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailH02a);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Qty\n", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                    ph1.Add(new Chunk(" كمية ", arbsmallbold));
                    PdfPCell DetailH03 = new PdfPCell(ph1);
                    DetailH03.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH03.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    DetailH03.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailH03);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Govt Fees  & Bank Chrg\n", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                    ph1.Add(new Chunk("", arbsmallbold));
                    PdfPCell DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailH04);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Trans. Charge\n", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                    ph1.Add(new Chunk("", arbsmallbold));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    DetailH04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailH04);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Total\n", new Font(Font.FontFamily.UNDEFINED, 9, Font.BOLD)));
                    ph1.Add(new Chunk(" مجموع", arbsmallbold));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH04.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
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
                            PdfPCell REM = new PdfPCell(new Phrase(rows["Quantity"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
                            REM.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(REM);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("N/A");
                        }
                        try
                        {
                            decimal gvtfee = (rows["Expense"].ToString() == "" ? 0 : Convert.ToDecimal(rows["Expense"].ToString())) +
                               (rows["Fine"].ToString() == "" ? 0 : Convert.ToDecimal(rows["Fine"].ToString()));

                            PdfPCell DT = new PdfPCell(new Phrase(gvtfee.ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(DT);

                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("");
                        }

                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["TotalServiceCharge"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(DT);

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

                    PdfPCell DetailEnd01 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                    DetailEnd01.HorizontalAlignment = Element.ALIGN_RIGHT;
                    DetailEnd01.Border = 0;
                    DetailEnd01.Colspan = 4;
                    emp_details.AddCell(DetailEnd01);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("صافي المجموع", arbfnt));
                    ph1.Add(new Chunk(" Net Total", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                    DetailEnd01 = new PdfPCell(ph1);
                    //DetailEnd00.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DetailEnd01.Colspan = 2;
                    DetailEnd01.MinimumHeight = 20f;
                    DetailEnd01.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailEnd01);

                    DetailEnd01 = new PdfPCell(new Phrase(dt_sum.Rows[0]["Total"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                    DetailEnd01.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(DetailEnd01);

                    DetailEnd01 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                    DetailEnd01.HorizontalAlignment = Element.ALIGN_RIGHT;
                    DetailEnd01.Border = 0;
                    DetailEnd01.Colspan = 4;
                    emp_details.AddCell(DetailEnd01);

                    ph1 = new Phrase();
                    //ph1.Add(new Chunk("صافي المجموع", arbfnt));
                    ph1.Add(new Chunk(" Paid Amount", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                    DetailEnd01 = new PdfPCell(ph1);
                    //DetailEnd00.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DetailEnd01.Colspan = 2;
                    DetailEnd01.MinimumHeight = 20f;
                    DetailEnd01.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailEnd01);

                    DetailEnd01 = new PdfPCell(new Phrase(dt_inv.Rows[0]["Received"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                    DetailEnd01.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(DetailEnd01);

                    DetailEnd01 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                    DetailEnd01.HorizontalAlignment = Element.ALIGN_RIGHT;
                    DetailEnd01.Border = 0;
                    DetailEnd01.Colspan = 4;
                    emp_details.AddCell(DetailEnd01);
                    ph1 = new Phrase();
                    ph1.Add(new Chunk(" Balance", new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                    DetailEnd01 = new PdfPCell(ph1);
                    //DetailEnd00.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DetailEnd01.Colspan = 2;
                    DetailEnd01.MinimumHeight = 20f;
                    DetailEnd01.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    emp_details.AddCell(DetailEnd01);

                    DetailEnd01 = new PdfPCell(new Phrase(dt_inv.Rows[0]["Receivable"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.BOLD)));
                    DetailEnd01.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(DetailEnd01);

                    document.Add(emp_details);
                }


                /////////////////////////////////////////////

                PdfPTable table2 = new PdfPTable(1);
                table2.DefaultCell.Padding = 4;
                float[] widthssetS = new float[] { 100F };
                table2.SetWidths(widthssetS);
                table2.SpacingBefore = 20f;
                table2.WidthPercentage = 100f;

                if (dtGeneral.Rows[0]["IsAddCreatedByInInvoicePrint"].ToString() == "1")
                {
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Prepared By : " + dt_inv.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
                    PdfPCell DetailEnd00in = new PdfPCell(ph1);
                    DetailEnd00in.Border = 0;
                    DetailEnd00in.MinimumHeight = 20f;
                    DetailEnd00in.VerticalAlignment = Element.ALIGN_TOP;
                    DetailEnd00in.HorizontalAlignment = Element.ALIGN_LEFT;
                    table2.AddCell(DetailEnd00in);
                }


                ph1 = new Phrase();
                ph1.Add(new Chunk("Kindly check the invoice and documents before leaving the counter", new Font(Font.FontFamily.UNDEFINED, 10, Font.NORMAL)));
                PdfPCell DetailEnd00 = new PdfPCell(ph1);
                DetailEnd00.Border = 0;
                DetailEnd00.MinimumHeight = 20f;
                DetailEnd00.VerticalAlignment = Element.ALIGN_TOP;
                DetailEnd00.HorizontalAlignment = Element.ALIGN_LEFT;
                table2.AddCell(DetailEnd00);

              

                document.Add(table2);
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