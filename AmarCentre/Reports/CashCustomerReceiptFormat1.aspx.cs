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
    public partial class CashCustomerReceiptFormat1 : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();
        Master_Bal obj_master = new Master_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"]);
            DataSet ds = obj_report.CashCustomerReceiptPrint(id);
            DataTable dt = ds.Tables[0];

            Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=CashReceiptPrint.pdf");
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

            #region header

            BaseFont bfTimes = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Font/arabtype.ttf"), BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font arbfnt = new iTextSharp.text.Font(bfTimes, 10, Font.NORMAL);
            iTextSharp.text.Font arbsmallbold = new iTextSharp.text.Font(bfTimes, 8, Font.BOLD);
            iTextSharp.text.Font arbfntbld = new iTextSharp.text.Font(bfTimes, 15, Font.BOLD);

            //PdfPTable table1 = new PdfPTable(1);
            //table1.DefaultCell.Padding = 4;
            //PdfPCell cell1 = new PdfPCell(new Phrase("Sales Order INVOICE", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            //cell1.Border = 0;
            //cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            //table1.AddCell(cell1);

            //document.Add(table1);

            PdfPTable Subhead = new PdfPTable(5);
            Subhead.DefaultCell.Padding = 4;
            float[] widths = new float[] { 15f, 25f, 25f, 30f, 25f };
            Subhead.SetWidths(widths);
            Subhead.WidthPercentage = 95f;


            PdfPCell Empty = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            Empty.Border = 0;
            Empty.HorizontalAlignment = Element.ALIGN_LEFT;

            PdfPCell EmptyWithTopBorder = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            EmptyWithTopBorder.Border = PdfPCell.TOP_BORDER;
            EmptyWithTopBorder.HorizontalAlignment = Element.ALIGN_LEFT;

            PdfPCell sub00 = new PdfPCell(new Phrase(dt.Rows[0]["CustomerName"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub00.Border = PdfPCell.TOP_BORDER;
            sub00.Colspan = 2;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            Subhead.AddCell(EmptyWithTopBorder);

            Phrase ph1 = new Phrase();
            ph1.Add(new Chunk("CASH RECEIPT\n", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            ph1.Add(new Chunk("الايصال", arbfntbld));
            PdfPCell sub04 = new PdfPCell(ph1);
            //new PdfPCell(new Phrase("CASH RECEIPT", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            sub04.Border = PdfPCell.TOP_BORDER;
            sub04.Colspan = 2;
            sub04.HorizontalAlignment = Element.ALIGN_CENTER;
            sub04.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(sub04);
            /*End of Row*/
            ph1 = new Phrase();
            ph1.Add(new Chunk(dt.Rows[0]["TRN"].ToString() + " :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("رقم ضريبي", arbfnt));
            ph1.Add(new Chunk(" / Customer TRN", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            PdfPCell sub10 = new PdfPCell(ph1);
            //new PdfPCell(new Phrase("Customer TRN: " + dt.Rows[0]["TRN"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub10.Border = 0;
            sub10.HorizontalAlignment = Element.ALIGN_RIGHT;
            sub10.VerticalAlignment = Element.ALIGN_TOP;
            sub10.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            sub10.Colspan = 3;
            Subhead.AddCell(sub10);



            //Subhead.AddCell(Empty);


            System.IO.MemoryStream mem = new MemoryStream();
            Barcode128 barImg = new Barcode128();
            barImg.Code = dt.Rows[0]["InvoiceCode"].ToString();
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

            ph1 = new Phrase();
            ph1.Add(new Chunk(dt.Rows[0]["Mobile_num"].ToString() + " :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("موبايل", arbfnt));
            ph1.Add(new Chunk(" / Mob", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));

            ph1.Add(new Chunk("\n", arbfnt));
            ph1.Add(new Chunk(dt.Rows[0]["Phone_num"].ToString() + " :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("هاتف", arbfnt));
            ph1.Add(new Chunk(" / Tel", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            PdfPCell sub30 = new PdfPCell(ph1);
            //PdfPCell sub30 = new PdfPCell(new Phrase("Mob: " + dt.Rows[0]["Mobile_num"].ToString() + ", " +
            //    "Tel: " + dt.Rows[0]["Phone_num"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub30.Border = 0;
            sub30.Colspan = 3;
            sub30.HorizontalAlignment = Element.ALIGN_RIGHT;
            sub30.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(sub30);

            ph1 = new Phrase();
            ph1.Add(new Chunk(" :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("تاريخ الدفع", arbfnt));
            ph1.Add(new Chunk(" / Payment Date", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            PdfPCell sub33 = new PdfPCell(ph1);
            //PdfPCell sub33 = new PdfPCell(new Phrase("Payment Date:", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub33.Border = 0;
            sub33.HorizontalAlignment = Element.ALIGN_LEFT;
            sub33.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(sub33);

            PdfPCell sub34 = new PdfPCell(new Phrase(dt.Rows[0]["Date"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub34.Border = 0;
            sub34.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub34);
            /*End of Row*/

            ph1 = new Phrase();
            ph1.Add(new Chunk(dt.Rows[0]["Email"].ToString() + " :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("ايميل", arbfnt));
            ph1.Add(new Chunk(" / Email", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            PdfPCell sub40 = new PdfPCell(ph1);
            //PdfPCell sub40 = new PdfPCell(new Phrase("Email: " + dt.Rows[0]["Email"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub40.Border = 0;
            sub40.Colspan = 2;
            sub40.HorizontalAlignment = Element.ALIGN_RIGHT;
            sub40.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(sub40);

            //Subhead.AddCell(Empty);
            ph1 = new Phrase();
            ph1.Add(new Chunk(" :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("رقم الفاتورة", arbfnt));
            ph1.Add(new Chunk(" / Invoice No", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            PdfPCell sub23 = new PdfPCell(ph1);
            //PdfPCell sub23 = new PdfPCell(new Phrase("Sales Order No:", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub23.Border = 0;
            sub23.Colspan = 2;
            sub23.HorizontalAlignment = Element.ALIGN_LEFT;
            sub23.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(sub23);

            PdfPCell sub24 = new PdfPCell(new Phrase(dt.Rows[0]["InvoiceCode"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub24.Border = 0;
            sub24.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub24);
            /*End of Row*/

            Subhead.AddCell(Empty);
            Subhead.AddCell(Empty);
            //Subhead.AddCell(Empty);

            ph1 = new Phrase();
            ph1.Add(new Chunk(" :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("رقم الايصال", arbfnt));
            ph1.Add(new Chunk(" / Receipt No", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            PdfPCell sub33a = new PdfPCell(ph1);
            //PdfPCell sub33a = new PdfPCell(new Phrase("Receipt No:", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub33a.Border = 0;
            sub33a.Colspan = 2;
            sub33a.HorizontalAlignment = Element.ALIGN_LEFT;
            sub33a.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            Subhead.AddCell(sub33a);

            PdfPCell sub24a = new PdfPCell(new Phrase(dt.Rows[0]["Code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub24a.Border = 0;
            sub24a.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub24a);
            /*End of Row*/
            document.Add(Subhead);
            PdfPTable DetailsTable = new PdfPTable(5);
            DetailsTable.DefaultCell.Padding = 4;
            float[] DetailsTableWidth = new float[] { 25f, 25f, 20f, 25f, 25f };
            DetailsTable.SetWidths(DetailsTableWidth);
            DetailsTable.WidthPercentage = 95f;
            float RowHeight = 25f;
            float SmallRowHeight = 20f;
            ph1 = new Phrase();
            ph1.Add(new Chunk(" :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("طريقة الدفع", arbfnt));
            ph1.Add(new Chunk(" / Payment Made By", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub33a = new PdfPCell(ph1);
            //sub33a = new PdfPCell(new Phrase("Payment Made By:", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub33a.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
            sub33a.HorizontalAlignment = Element.ALIGN_RIGHT;
            sub33a.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            sub33a.VerticalAlignment = Element.ALIGN_MIDDLE;
            sub33a.Colspan = 2;
            sub33a.FixedHeight = RowHeight;
            DetailsTable.AddCell(sub33a);

            sub33a = new PdfPCell(new Phrase(dt.Rows[0]["PaymentMode"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub33a.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
            sub33a.HorizontalAlignment = Element.ALIGN_LEFT;
            sub33a.VerticalAlignment = Element.ALIGN_MIDDLE;
            sub33a.FixedHeight = RowHeight;
            DetailsTable.AddCell(sub33a);

            sub33a = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub33a.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
            sub33a.HorizontalAlignment = Element.ALIGN_LEFT;
            sub33a.Colspan = 2;
            sub33a.FixedHeight = RowHeight;
            DetailsTable.AddCell(sub33a);
            /*End of Row*/

            ph1 = new Phrase();
            ph1.Add(new Chunk(" :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("المبلغ الصافي", arbfnt));
            ph1.Add(new Chunk(" / Net Amount", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub33a = new PdfPCell(ph1);
            //sub33a = new PdfPCell(new Phrase("Net Amount:", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub33a.Border = 0;
            sub33a.HorizontalAlignment = Element.ALIGN_RIGHT;
            sub33a.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            sub33a.VerticalAlignment = Element.ALIGN_MIDDLE;
            sub33a.Colspan = 4;
            sub33a.FixedHeight = SmallRowHeight;
            DetailsTable.AddCell(sub33a);

            sub33a = new PdfPCell(new Phrase(dt.Rows[0]["PendingAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub33a.Border = 0;
            sub33a.HorizontalAlignment = Element.ALIGN_RIGHT;
            sub33a.VerticalAlignment = Element.ALIGN_MIDDLE;
            sub33a.FixedHeight = SmallRowHeight;
            DetailsTable.AddCell(sub33a);
            /*End of Row*/

            //ph1 = new Phrase();
            //ph1.Add(new Chunk(" :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            //ph1.Add(new Chunk("المبلغ المستلم", arbfnt));
            //ph1.Add(new Chunk(" / Received Amount", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            //sub33a = new PdfPCell(ph1);
            ////sub33a = new PdfPCell(new Phrase("Received Amount:", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            //sub33a.Border = 0;
            //sub33a.HorizontalAlignment = Element.ALIGN_RIGHT;
            //sub33a.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            //sub33a.VerticalAlignment = Element.ALIGN_MIDDLE;
            //sub33a.Colspan = 4;
            //sub33a.FixedHeight = SmallRowHeight;
            //DetailsTable.AddCell(sub33a);

            //sub33a = new PdfPCell(new Phrase(dt.Rows[0]["ReceivedAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            //sub33a.Border = 0;
            //sub33a.HorizontalAlignment = Element.ALIGN_RIGHT;
            //sub33a.VerticalAlignment = Element.ALIGN_MIDDLE;
            //sub33a.FixedHeight = SmallRowHeight;
            //DetailsTable.AddCell(sub33a);
            ///*End of Row*/

            ph1 = new Phrase();
            ph1.Add(new Chunk(" :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("المبلغ المدفوع", arbfnt));
            ph1.Add(new Chunk(" / Paid Amount", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub33a = new PdfPCell(ph1);
            sub33a.Border = 0;
            sub33a.HorizontalAlignment = Element.ALIGN_RIGHT;
            sub33a.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            sub33a.VerticalAlignment = Element.ALIGN_MIDDLE;
            sub33a.Colspan = 4;
            sub33a.FixedHeight = SmallRowHeight;
            DetailsTable.AddCell(sub33a);

            sub33a = new PdfPCell(new Phrase(dt.Rows[0]["Amount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub33a.Border = 0;
            sub33a.HorizontalAlignment = Element.ALIGN_RIGHT;
            sub33a.VerticalAlignment = Element.ALIGN_MIDDLE;
            sub33a.FixedHeight = SmallRowHeight;
            DetailsTable.AddCell(sub33a);
            /*End of Row*/

            ph1 = new Phrase();
            ph1.Add(new Chunk(" :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("الرصيد", arbfnt));
            ph1.Add(new Chunk(" / Balance", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub33a = new PdfPCell(ph1);
            //sub33a = new PdfPCell(new Phrase("Balance:", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub33a.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
            sub33a.HorizontalAlignment = Element.ALIGN_RIGHT;
            sub33a.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            sub33a.VerticalAlignment = Element.ALIGN_MIDDLE;
            sub33a.Colspan = 4;
            sub33a.FixedHeight = SmallRowHeight;
            DetailsTable.AddCell(sub33a);

            sub33a = new PdfPCell(new Phrase(dt.Rows[0]["Balance"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub33a.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
            sub33a.HorizontalAlignment = Element.ALIGN_RIGHT;
            sub33a.VerticalAlignment = Element.ALIGN_MIDDLE;
            sub33a.FixedHeight = SmallRowHeight;
            DetailsTable.AddCell(sub33a);
            /*End of Row*/
            document.Add(DetailsTable);

            PdfPTable totalexp = new PdfPTable(3);
            totalexp.DefaultCell.Padding = 4;
            float[] widths1 = new float[] { 13f, 20f, 55f };
            totalexp.SetWidths(widths1);
            totalexp.SpacingBefore = 5;
            totalexp.WidthPercentage = 95f;

            sub33a = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub33a.Border = PdfPCell.TOP_BORDER;
            sub33a.Colspan = 3;
            sub33a.HorizontalAlignment = Element.ALIGN_RIGHT;
            sub33a.VerticalAlignment = Element.ALIGN_MIDDLE;
            totalexp.AddCell(sub33a);
            /*End of Row*/
            document.Add(totalexp);

            PdfPTable DetailLine = new PdfPTable(3);
            DetailLine.DefaultCell.Padding = 4;
            float[] DetailLinewidths1 = new float[] { 13f, 40f, 35f };
            DetailLine.SetWidths(DetailLinewidths1);
            DetailLine.SpacingBefore = 80;
            DetailLine.WidthPercentage = 95f;


            /*End of Row*/
            sub33a = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub33a.Border = 0;
            sub33a.Colspan = 2;
            sub33a.HorizontalAlignment = Element.ALIGN_RIGHT;
            sub33a.VerticalAlignment = Element.ALIGN_MIDDLE;
            DetailLine.AddCell(sub33a);

            ph1 = new Phrase();
            ph1.Add(new Chunk("تم الاستلام بـ", arbfnt));
            ph1.Add(new Chunk(" / Received", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub33a = new PdfPCell(ph1);
            //sub33a = new PdfPCell(new Phrase("Received", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub33a.Border = PdfPCell.TOP_BORDER;
            sub33a.HorizontalAlignment = Element.ALIGN_CENTER;
            sub33a.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            sub33a.VerticalAlignment = Element.ALIGN_MIDDLE;
            DetailLine.AddCell(sub33a);
            /*End of Row*/

            document.Add(DetailLine);


            #endregion

            PdfPTable footer = new PdfPTable(1);
            footer.DefaultCell.Padding = 4;
            footer.SpacingAfter = 20f;
            footer.TotalWidth = document.PageSize.Width - (2 * document.LeftMargin);
            footer.WidthPercentage = 90f;


            PdfPCell Fotservice = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            Fotservice.Border = PdfPCell.BOTTOM_BORDER;
            Fotservice.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            footer.AddCell(Fotservice);

            ph1 = new Phrase();
            ph1.Add(new Chunk(DateTime.Now + " :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            ph1.Add(new Chunk("تم الطباعة بتاريخ", arbfnt));
            ph1.Add(new Chunk(" / Printed On", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            Fotservice = new PdfPCell(ph1);
            //Fotservice = new PdfPCell(new Phrase("Printed On : " + DateTime.Now, new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            Fotservice.Border = 0;
            Fotservice.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            Fotservice.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            footer.AddCell(Fotservice);

            footer.WriteSelectedRows(0, -1, document.LeftMargin, footer.TotalHeight / 2 + 20, writer.DirectContent);

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