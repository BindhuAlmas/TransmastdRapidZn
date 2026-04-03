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
using iTextSharp.text.pdf.draw;

namespace AmarCentre.Reports
{
    public partial class PaymentVoucher : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);

            DataSet ds = obj_report.PaymentVoucher_Print(id);
            DataTable dt1 = ds.Tables[0];
            DataTable dt_gen = ds.Tables[1];

            var pgSize = new iTextSharp.text.Rectangle(700, 400);

            Document document = new Document(PageSize.A4, 25f, 25f, 0f, 0f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=PaymentVoucher.pdf");
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
            document.Open();

            #region headr

            if (dt_gen.Rows[0]["PrintHeader"].ToString() != "")
            {

                PdfPTable ftrtbl = new PdfPTable(1);
                //ftrtbl.DefaultCell.PaddingLeft = 40;
                ftrtbl.DefaultCell.FixedHeight = 130f;
                ftrtbl.DefaultCell.Border = 0;
                ftrtbl.WidthPercentage = 95f;

                string imageURL = Server.MapPath("../UploadedImage/" + dt_gen.Rows[0]["PrintHeader"].ToString());
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(new Uri(imageURL));
                //Resize image depend upon your need
                jpg.ScaleToFit(550f, 450f);
                //Give space before image
                //Give some space after the image
                jpg.Alignment = Element.ALIGN_CENTER;

                ftrtbl.AddCell(jpg);


                document.Add(ftrtbl);
            }

            DottedLineSeparator dottedline = new DottedLineSeparator();
            dottedline.Gap = 3f;
            Phrase datePhrase = new Phrase();
            datePhrase.Add(dottedline);

            PdfPCell hdempty = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            hdempty.Border = 0;
            hdempty.HorizontalAlignment = PdfPCell.ALIGN_LEFT;

            PdfPTable headrrow12 = new PdfPTable(2);
            headrrow12.DefaultCell.Padding = 4;
            headrrow12.WidthPercentage = 95;
            headrrow12.SpacingAfter = 10;
            float[] headrrow12wh = new float[] {75f, 25f };
            headrrow12.SetWidths(headrrow12wh);

            PdfPCell hdng = new PdfPCell(new Phrase("PAYMENT VOUCHER", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD)));
            hdng.Border = 0;
            hdng.Colspan = 2;
            hdng.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            headrrow12.AddCell(hdng);

            headrrow12.AddCell(hdempty);
            PdfPCell hdemptydateno = new PdfPCell(new Phrase("No : " + dt1.Rows[0]["Code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            hdemptydateno.Border = 0;
            hdemptydateno.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            headrrow12.AddCell(hdemptydateno);

            headrrow12.AddCell(hdempty);
            PdfPCell hdemptydate = new PdfPCell(new Phrase("Date : " + dt1.Rows[0]["Date"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            hdemptydate.Border = 0;
            hdemptydate.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            headrrow12.AddCell(hdemptydate);

            if (dt1.Rows[0]["PVTRN"].ToString() != "")
            {
                headrrow12.AddCell(hdempty);
                hdemptydate = new PdfPCell(new Phrase("TRN : " + dt1.Rows[0]["PVTRN"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                hdemptydate.Border = 0;
                hdemptydate.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                headrrow12.AddCell(hdemptydate);
            }

            document.Add(headrrow12);

            #endregion


            PdfPTable row1 = new PdfPTable(2);
            row1.DefaultCell.Padding = 4;
            row1.WidthPercentage = 95;
            row1.SpacingAfter = 10f;
            float[] row1w = new float[] { 15f, 80f };
            row1.SetWidths(row1w);

            PdfPCell row1Cell00 = new PdfPCell(new Phrase("Paid to ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            row1Cell00.Border = 0;
            row1Cell00.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            row1.AddCell(row1Cell00);


            PdfPCell row1Cell01 = new PdfPCell(new Phrase(dt1.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            row1Cell01.Border = 0;
            row1Cell01.PaddingLeft = 10f;
            row1Cell01.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            row1Cell01.PaddingTop = -1f;
            row1.AddCell(row1Cell01);

            PdfPCell row1Cell10 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            row1Cell10.Border = 0;
            row1Cell10.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            row1.AddCell(row1Cell10);

            PdfPCell row1Cell11 = new PdfPCell(datePhrase);
            row1Cell11.Border = 0;
            row1Cell11.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            row1Cell11.PaddingTop = -13f;
            row1.AddCell(row1Cell11);

            document.Add(row1);

            PdfPTable row2 = new PdfPTable(3);
            row2.DefaultCell.Padding = 4;
            row2.WidthPercentage = 95;
            row2.SpacingAfter = 10f;
            float[] row2w = new float[] { 50f, 157f, 60f };
            row2.SetWidths(row2w);

            PdfPCell row2Cell00 = new PdfPCell(new Phrase("The Sum of " + "AED", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            row2Cell00.Border = 0;
            row2Cell00.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            row2.AddCell(row2Cell00);


            PdfPCell row2Cell01 = new PdfPCell(new Phrase(" " + ConvertNumbertoWords(Convert.ToDecimal(dt1.Rows[0]["Amount"])) + " Only ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            row2Cell01.Border = 0;
            row2Cell01.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            row2Cell01.PaddingTop = -1f;
            row2Cell01.Colspan = 2;
            row2.AddCell(row2Cell01);

            PdfPCell row2Cell10 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            row2Cell10.Border = 0;
            row2Cell10.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            row2.AddCell(row2Cell10);

            PdfPCell row2Cell11 = new PdfPCell(datePhrase);
            row2Cell11.Border = 0;
            row2Cell11.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            row2Cell11.PaddingTop = -14f;
            row2Cell11.Colspan = 2;
            row2.AddCell(row2Cell11);

            document.Add(row2);

            PdfPTable row3 = new PdfPTable(6);
            row3.DefaultCell.Padding = 4;
            row3.WidthPercentage = 95;
            row3.SpacingAfter = 10f;
            float[] row3w = new float[] { 45f, 45f, 12f, 35f, 13f, 75f };
            row3.SetWidths(row3w);

            PdfPCell row3Cell00 = new PdfPCell(new Phrase("By Cash/Cheque No", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            row3Cell00.Border = 0;
            row3Cell00.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            row3.AddCell(row3Cell00);


            PdfPCell row3Cell01 = new PdfPCell(new Phrase(dt1.Rows[0]["PaymentType"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            row3Cell01.Border = 0;
            row3Cell01.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            row3Cell01.PaddingTop = -1f;
            row3.AddCell(row3Cell01);


            PdfPCell row3Cell02 = new PdfPCell(new Phrase("Date", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            row3Cell02.Border = 0;
            row3Cell02.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            row3.AddCell(row3Cell02);

            PdfPCell row3Cell03 = new PdfPCell(new Phrase(dt1.Rows[0]["PayDate"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            row3Cell03.Border = 0;
            row3Cell03.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            row3Cell03.PaddingTop = -1f;
            row3.AddCell(row3Cell03);

            PdfPCell row3Cell04 = new PdfPCell(new Phrase("Bank", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            row3Cell04.Border = 0;
            row3Cell04.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            row3.AddCell(row3Cell04);

            PdfPCell row3Cell05 = new PdfPCell(new Phrase(dt1.Rows[0]["BankName"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            row3Cell05.Border = 0;
            row3Cell05.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            row3Cell05.PaddingTop = -1f;
            row3.AddCell(row3Cell05);

            PdfPCell row3Cell10 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            row3Cell10.Border = 0;
            row3Cell10.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            row3.AddCell(row3Cell10);

            PdfPCell row3Cell11 = new PdfPCell(datePhrase);
            row3Cell11.Border = 0;
            row3Cell11.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            row3Cell11.PaddingTop = -14f;
            row3.AddCell(row3Cell11);


            PdfPCell row3Cell12 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            row3Cell12.Border = 0;
            row3Cell12.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            row3.AddCell(row3Cell12);

            row3.AddCell(row3Cell11);
            row3.AddCell(row3Cell12);
            row3.AddCell(row3Cell11);

            document.Add(row3);

            PdfPTable row4 = new PdfPTable(3);
            row4.DefaultCell.Padding = 4;
            row4.WidthPercentage = 95;
            row4.SpacingAfter = 10f;
            float[] row4w = new float[] { 20f, 197f, 60f };
            row4.SetWidths(row4w);

            PdfPCell row4Cell00 = new PdfPCell(new Phrase("Being ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            row4Cell00.Border = 0;
            row4Cell00.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            row4.AddCell(row4Cell00);

            PdfPCell row4Cell01 = new PdfPCell(new Phrase(dt1.Rows[0]["ExpenseType"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            row4Cell01.Border = 0;
            row4Cell01.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            row4Cell01.PaddingTop = -1f;
            row4Cell01.Colspan = 2;
            row4.AddCell(row4Cell01);

            PdfPCell row4Cell10 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            row4Cell10.Border = 0;
            row4Cell10.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            row4.AddCell(row4Cell10);


            PdfPCell row4Cell11 = new PdfPCell(datePhrase);
            row4Cell11.Border = 0;
            row4Cell11.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            row4Cell11.PaddingTop = -14f;
            row4Cell11.Colspan = 2;
            row4.AddCell(row4Cell11);

            document.Add(row4);

            //line 5
            if (dt1.Rows[0]["RmarksTrans"].ToString() != "")
            {
                PdfPTable rowline5 = new PdfPTable(3);
                rowline5.DefaultCell.Padding = 4;
                rowline5.WidthPercentage = 95;
                rowline5.SpacingAfter = 10f;
                float[] row5f = new float[] { 20f, 197f, 60f };
                rowline5.SetWidths(row4w);

                PdfPCell R5 = new PdfPCell(new Phrase("Note ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                R5.Border = 0;
                R5.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                rowline5.AddCell(R5);

                R5 = new PdfPCell(new Phrase(dt1.Rows[0]["RmarksTrans"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                R5.Border = 0;
                R5.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                R5.PaddingTop = -1f;
                R5.Colspan = 2;
                rowline5.AddCell(R5);

                R5 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                R5.Border = 0;
                R5.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                rowline5.AddCell(R5);

                R5 = new PdfPCell(datePhrase);
                R5.Border = 0;
                R5.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                R5.PaddingTop = -14f;
                R5.Colspan = 2;
                rowline5.AddCell(R5);

                document.Add(rowline5);
            }


            PdfPTable base1 = new PdfPTable(8);
            base1.DefaultCell.Padding = 4;
            base1.WidthPercentage = 95;
            base1.SpacingBefore = 20f;
            float[] base1wd = new float[] { 20f, 15f, 35f, 18f, 32f, 20f, 30f, 30f };
            base1.SetWidths(base1wd);

            PdfPCell base1cel1 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            base1cel1.Border = 0;
            base1cel1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            base1.AddCell(base1cel1);

            PdfPCell base1cel2 = new PdfPCell(new Phrase("Amount", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            base1cel2.Border = 0;
            base1cel2.Colspan = 2;
            base1cel2.PaddingLeft = 5f;
            base1cel2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            base1.AddCell(base1cel2);

            PdfPCell base1cel3 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            base1cel3.Border = 0;
            base1cel3.Colspan = 2;
            base1cel3.PaddingLeft = 5f;
            base1cel3.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            base1.AddCell(base1cel3);

            PdfPCell base1cel4 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            base1cel4.Border = 0;
            base1cel4.Colspan = 2;
            base1cel4.PaddingLeft = 5f;
            base1cel4.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            base1.AddCell(base1cel4);
            base1.AddCell(base1cel1);
            //

            PdfPCell base1cel1lin = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            base1cel1lin.Border = 0;
            base1cel1lin.Colspan = 8;
            base1cel1lin.MinimumHeight = 10f;
            base1cel1lin.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            base1.AddCell(base1cel1lin);

            //
            base1.AddCell(base1cel1);

            PdfPCell base1cel32 = new PdfPCell(new Phrase("AED", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            base1cel32.Border = 0;
            base1cel32.PaddingLeft = 5f;
            base1cel32.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            base1.AddCell(base1cel32);
            PdfPCell base1cel233 = new PdfPCell(new Phrase(dt1.Rows[0]["Amount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            base1cel233.Border = 0;
            base1cel233.PaddingLeft = 5f;
            base1cel233.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            base1.AddCell(base1cel233);

            PdfPCell base1cel22 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            base1cel22.Border = 0;
            base1cel22.PaddingLeft = 5f;
            base1cel22.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            base1.AddCell(base1cel22);
            PdfPCell base1cel223 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            base1cel223.Border = 0;
            base1cel223.PaddingLeft = 5f;
            base1cel223.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            base1.AddCell(base1cel223);

            PdfPCell base1cel42 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            base1cel42.Border = 0;
            base1cel42.PaddingLeft = 5f;
            base1cel42.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            base1.AddCell(base1cel42);
            PdfPCell base1cel423 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            base1cel423.Border = 0;
            base1cel423.PaddingLeft = 5f;
            base1cel423.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            base1.AddCell(base1cel423);
            base1.AddCell(base1cel1);

            //

            base1.AddCell(base1cel1);
            base1.AddCell(base1cel1);

            PdfPCell dottdln = new PdfPCell(datePhrase);
            dottdln.Border = 0;
            dottdln.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            dottdln.PaddingTop = -11f;
            base1.AddCell(dottdln);

            base1.AddCell(base1cel1);
            //base1.AddCell(dottdln);

            base1.AddCell(base1cel1);
            //base1.AddCell(dottdln);

            base1.AddCell(base1cel1);

            //
            base1.AddCell(base1cel1lin);

            //

            document.Add(base1);

            PdfPTable row5 = new PdfPTable(5);
            row5.DefaultCell.Padding = 4;
            row5.WidthPercentage = 95;
            row5.SpacingBefore = 20f;
            float[] row5w = new float[] { 50f, 20f, 30f, 20f, 30f };
            row5.SetWidths(row5w);

            PdfPCell row5Cell00 = new PdfPCell(new Phrase("Prepared By", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            row5Cell00.Border = 0;
            row5Cell00.MinimumHeight = 25f;
            row5Cell00.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
            row5Cell00.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            row5.AddCell(row5Cell00);

            row5Cell00 = new PdfPCell(new Phrase("Verified By", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            row5Cell00.Border = 0;
            row5Cell00.Colspan = 2;
            row5Cell00.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
            row5Cell00.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            row5.AddCell(row5Cell00);

            row5Cell00 = new PdfPCell(new Phrase("Approved By", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            row5Cell00.Border = 0;
            row5Cell00.Colspan = 2;
            row5Cell00.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
            row5Cell00.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            row5.AddCell(row5Cell00);

            //empty row
            row5Cell00 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            row5Cell00.Border = 0;
            row5Cell00.Colspan = 5;
            row5Cell00.MinimumHeight = 40f;
            row5Cell00.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
            row5Cell00.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            row5.AddCell(row5Cell00);

            row5Cell00 = new PdfPCell(new Phrase("Received By", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            row5Cell00.Border = 0;
            row5Cell00.Colspan = 3;
            row5Cell00.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            row5.AddCell(row5Cell00);

            PdfPCell row5Cell01 = new PdfPCell(new Phrase("For " + Application["Company"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            row5Cell01.Border = 0;
            row5Cell01.Colspan = 2;
            row5Cell01.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            row5.AddCell(row5Cell01);

            row5Cell00 = new PdfPCell(new Phrase(dt1.Rows[0]["Signature"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            row5Cell00.Border = 0;
            row5Cell00.Colspan = 5;
            row5Cell00.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            row5.AddCell(row5Cell00);

            //

            document.Add(row5);

            #region Footer

            if (Application["PrintFooter"] != "")
            {

                PdfPTable footer = new PdfPTable(1);
                footer.DefaultCell.Padding = 4;
                footer.SpacingAfter = 20f;
                footer.TotalWidth = document.PageSize.Width - (2 * document.LeftMargin);
                footer.WidthPercentage = 95f;

                string imageURL = Server.MapPath("../UploadedImage/" + Application["PrintFooter"]);
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(new Uri(imageURL));
                jpg.ScaleToFit(470f, 450f);

                PdfPCell Fotservice = new PdfPCell(jpg, true);
                Fotservice.Border = 0;
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