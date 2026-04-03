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
    public partial class QuotationPrintFormat4 : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();
        Master_Bal obj_master = new Master_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"]);

            DataSet ds = obj_report.QuotationPrintFormat2(id);
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
            if (dtGenSetting.Rows[0]["PrintHeader"].ToString()  != "")
            {

                string imageURL = Server.MapPath("../UploadedImage/" + dtGenSetting.Rows[0]["PrintHeader"].ToString());
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

            Phrase ph1 = new Phrase();
            ph1.Add(new Chunk("Proforma Invoice\n", new Font(Font.FontFamily.UNDEFINED, 14, Font.BOLD)));
            ph1.Add(new Chunk("الفاتورة الأولية", arbfntbldN));

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

            PdfPTable Subhead = new PdfPTable(4);
            Subhead.DefaultCell.Padding = 4;
            float[] widths = new float[] { 15f, 55f, 15f, 20f };
            Subhead.SetWidths(widths);
            Subhead.WidthPercentage = 100f;

            // empty cell

            PdfPCell Empty = new PdfPCell(new Phrase("", new Font(Font.FontFamily.UNDEFINED, 11, Font.NORMAL)));
            Empty.Border = 0;
            Empty.HorizontalAlignment = Element.ALIGN_LEFT;

            //end
            PdfPCell sub1 = new PdfPCell(new Phrase("Customer :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            sub1.Border = 0;
            sub1.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub1);

            PdfPCell sub12 = new PdfPCell(new Phrase(dt_cust.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            sub12.Border = 0;
            sub12.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub12);

            PdfPCell sub13 = new PdfPCell(new Phrase("TRN :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            sub13.Border = 0;
            sub13.HorizontalAlignment = Element.ALIGN_RIGHT;
            Subhead.AddCell(sub13);

            PdfPCell sub14 = new PdfPCell(new Phrase(dtGenSetting.Rows[0]["TRN"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            sub14.Border = 0;
            sub14.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub14);

            PdfPCell sub21 = new PdfPCell(new Phrase("Contact :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            sub21.Border = 0;
            sub21.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub21);

            PdfPCell sub122 = new PdfPCell(new Phrase(dt_cust.Rows[0]["Mobile_num"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            sub122.Border = 0;
            sub122.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub122);

            sub13 = new PdfPCell(new Phrase("Quotation No :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            sub13.Border = 0;
            sub13.HorizontalAlignment = Element.ALIGN_RIGHT;
            Subhead.AddCell(sub13);

            sub14 = new PdfPCell(new Phrase(dt_inv.Rows[0]["Code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            sub14.Border = 0;
            sub14.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub14);

            PdfPCell sub21add = new PdfPCell(new Phrase("License Type :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            sub21add.Border = 0;
            sub21add.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub21add);

            PdfPCell sub122add = new PdfPCell(new Phrase(dt_inv.Rows[0]["subject"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            sub122add.Border = 0;
            sub122add.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub122add);

            PdfPCell sub132 = new PdfPCell(new Phrase("Date :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            sub132.Border = 0;
            sub132.HorizontalAlignment = Element.ALIGN_RIGHT;
            Subhead.AddCell(sub132);

            PdfPCell sub142 = new PdfPCell(new Phrase(dt_inv.Rows[0]["Dates"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            sub142.Border = 0;
            sub142.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub142);

            document.Add(Subhead);

            #endregion

            #region data

            if (dt_invD.Rows.Count > 0)
            {
                PdfPTable emp_details = new PdfPTable(4);

                emp_details.DefaultCell.Padding = 4;
                float[] widthsdet = new float[] { 4f, 25f, 20f, 10f };
                emp_details.SetWidths(widthsdet);

                emp_details.SpacingBefore = 20f;
                emp_details.WidthPercentage = 100f;

                PdfPCell hd = new PdfPCell(new Phrase("Sl", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                hd.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                hd.MinimumHeight = 20f;
                emp_details.AddCell(hd);

                hd = new PdfPCell(new Phrase("Description", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                hd.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                hd.MinimumHeight = 20f;
                emp_details.AddCell(hd);

                hd = new PdfPCell(new Phrase("Dubai Company Registration - DED", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                hd.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                hd.MinimumHeight = 20f;
                emp_details.AddCell(hd);

                hd = new PdfPCell(new Phrase("Price", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                hd.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                hd.MinimumHeight = 20f;
                emp_details.AddCell(hd);

                int i = 0;
                decimal Tot = 0;

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
                        PdfPCell sn = new PdfPCell(new Phrase(rows["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        sn.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        emp_details.AddCell(sn);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }

                    try
                    {
                        PdfPCell REM = new PdfPCell(new Phrase(rows["Sdesc"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        REM.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        emp_details.AddCell(REM);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("N/A");
                    }

                    try
                    {
                        PdfPCell DT = new PdfPCell(new Phrase(rows["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        emp_details.AddCell(DT);
                        Tot = Tot + Convert.ToDecimal(rows["Total"]);

                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                }

                PdfPCell totw1wf = new PdfPCell(new Phrase("Total ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                totw1wf.HorizontalAlignment = Element.ALIGN_RIGHT;
                totw1wf.Colspan = 3;
                emp_details.AddCell(totw1wf);

                totw1wf = new PdfPCell(new Phrase(Tot.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                totw1wf.HorizontalAlignment = Element.ALIGN_RIGHT;
                emp_details.AddCell(totw1wf);

                document.Add(emp_details);

                PdfPTable totalexp = new PdfPTable(4);
                totalexp.DefaultCell.Padding = 4;
                float[] widths1 = new float[] { 25f, 25f, 25f, 25f };
                totalexp.SetWidths(widths1);
                totalexp.SpacingBefore = 10;
                totalexp.WidthPercentage = 100f;

                //PdfPCell totw1wpsss = new PdfPCell(new Phrase("AED " + ConvertNumbertoWords(Convert.ToDecimal(dt_sum.Rows[0]["Total"])) + " Only", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLDITALIC)));
                //totw1wpsss.Border = 0;
                //totw1wpsss.Colspan = 4;
                //totw1wpsss.HorizontalAlignment = Element.ALIGN_LEFT;
                //totalexp.AddCell(totw1wpsss);

                ph1 = new Phrase();
                ph1.Add(new Chunk("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLDITALIC)));
                PdfPCell toddddt2wwDD = new PdfPCell(ph1);
                toddddt2wwDD.Border = 0;
                toddddt2wwDD.Colspan = 3;
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
                toddddt2wwDD.Colspan = 3;
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
                    totw1wpsss.Colspan = 4;
                    totw1wpsss.HorizontalAlignment = Element.ALIGN_LEFT;
                    totalexp.AddCell(totw1wpsss);
                }


                document.Add(totalexp);
            }

            #endregion

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