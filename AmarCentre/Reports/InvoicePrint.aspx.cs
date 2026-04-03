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
    public partial class InvoicePrint : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();
        Master_Bal obj_master = new Master_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"]);
            DataSet ds = obj_report.Invoice_Print(id);
            DataTable dt_inv = ds.Tables[0];
            DataTable dt_invD = ds.Tables[1];
            DataTable dt_cust = ds.Tables[2];
            DataTable dt_sum = ds.Tables[3];

            Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=InvoicePrint.pdf");
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

            //BaseFont bfTimes = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Font/arabtype.ttf"), BaseFont.IDENTITY_H, true);
            //iTextSharp.text.Font arbfnt = new iTextSharp.text.Font(bfTimes, 17, Font.NORMAL);
            //iTextSharp.text.Font arbfntbld = new iTextSharp.text.Font(bfTimes, 21, Font.BOLD);

            PdfPTable table1 = new PdfPTable(1);
            table1.DefaultCell.Padding = 4;
            PdfPCell cell1 = new PdfPCell(new Phrase("TAX INVOICE", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            cell1.Border = 0;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            table1.AddCell(cell1);
           
            document.Add(table1);

            PdfPTable Subhead = new PdfPTable(5);
            Subhead.DefaultCell.Padding = 4;
            float[] widths = new float[] { 15f, 25f, 40f, 15f, 25f };
            Subhead.SetWidths(widths);
            Subhead.WidthPercentage = 95f;

            // empty cell

            PdfPCell Empty = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            Empty.Border = 0;
            Empty.HorizontalAlignment = Element.ALIGN_LEFT;

            //end

            Subhead.AddCell(Empty);
            Subhead.AddCell(Empty);
            Subhead.AddCell(Empty);
            

            System.IO.MemoryStream mem = new MemoryStream();
            Barcode128 barImg = new Barcode128();
            barImg.Code = dt_inv.Rows[0]["Code"].ToString();
            barImg.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White).Save(mem, System.Drawing.Imaging.ImageFormat.Png);
            iTextSharp.text.Image imgs = iTextSharp.text.Image.GetInstance(mem.ToArray());
            mem.Flush();
            mem.Close();

            PdfPCell Cuscell22 = new PdfPCell(imgs);
            Cuscell22.Border = 0;
            Cuscell22.Colspan = 2;
            Cuscell22.MinimumHeight = 50f;
            Cuscell22.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            Cuscell22.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            Subhead.AddCell(Cuscell22);

            PdfPCell sub1 = new PdfPCell(new Phrase("Customer :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub1.Border = 0;
            sub1.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub1);

            PdfPCell sub12 = new PdfPCell(new Phrase(dt_cust.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub12.Border = 0;
            sub12.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub12);

            Subhead.AddCell(Empty);

            PdfPCell sub13 = new PdfPCell(new Phrase("Invoice No :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub13.Border = 0;
            sub13.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub13);

            PdfPCell sub14 = new PdfPCell(new Phrase(dt_inv.Rows[0]["Code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub14.Border = 0;
            sub14.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub14);

            PdfPCell sub21 = new PdfPCell(new Phrase("Mobile No :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub21.Border = 0;
            sub21.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub21);

            PdfPCell sub122 = new PdfPCell(new Phrase(dt_cust.Rows[0]["Mobile_num"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub122.Border = 0;
            sub122.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub122);

            Subhead.AddCell(Empty);

            PdfPCell sub132 = new PdfPCell(new Phrase("Date :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub132.Border = 0;
            sub132.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub132);

            PdfPCell sub142 = new PdfPCell(new Phrase(dt_inv.Rows[0]["Dates"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub142.Border = 0;
            sub142.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub142);

            PdfPCell sub21add = new PdfPCell(new Phrase("TRN :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub21add.Border = 0;
            sub21add.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub21add);

            PdfPCell sub122add = new PdfPCell(new Phrase(dt_cust.Rows[0]["TRN"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub122add.Border = 0;
            sub122add.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub122add);

            Subhead.AddCell(Empty);

            sub132 = new PdfPCell(new Phrase("Employee :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub132.Border = 0;
            sub132.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub132);

            sub142 = new PdfPCell(new Phrase(dt_inv.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub142.Border = 0;
            sub142.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub142);

             sub21add = new PdfPCell(new Phrase("Address :", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub21add.Border = 0;
            sub21add.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub21add);

            sub122add = new PdfPCell(new Phrase(dt_cust.Rows[0]["Address"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub122add.Border = 0;
            sub122add.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub122add);

            Subhead.AddCell(Empty);
            Subhead.AddCell(Empty);
            Subhead.AddCell(Empty);

            document.Add(Subhead);

            #endregion

            #region data

            if (dt_invD.Rows.Count > 0)
            {
                PdfPTable emp_details = new PdfPTable(9);
                emp_details.DefaultCell.Padding = 4;
                float[] widthsdet = new float[] { 5f, 20f, 20f, 10f, 10f, 7f, 9f,7f,11f };
                emp_details.SetWidths(widthsdet);
                emp_details.SpacingBefore = 20f;
                emp_details.WidthPercentage = 95f;
                PdfPCell SN = new PdfPCell(new Phrase("S.No", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                SN.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(SN);
                PdfPCell ty = new PdfPCell(new Phrase("Service", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                ty.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(ty);
                PdfPCell nam = new PdfPCell(new Phrase("Particulars", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                nam.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(nam);
                PdfPCell da = new PdfPCell(new Phrase("Govt. Fee", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                da.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(da);
                da = new PdfPCell(new Phrase("Service Charge", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                da.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(da);
                da = new PdfPCell(new Phrase("VAT %", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                da.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(da);
                da = new PdfPCell(new Phrase("VAT Amount", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                da.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(da);
                PdfPCell inc = new PdfPCell(new Phrase("Qty", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                inc.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(inc);
                PdfPCell exptt = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                exptt.HorizontalAlignment = Element.ALIGN_CENTER;
                emp_details.AddCell(exptt);

                int i = 0;

                foreach (DataRow rows in dt_invD.Rows)
                {
                    try
                    {
                        PdfPCell sn = new PdfPCell(new Phrase((++i).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        sn.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        emp_details.AddCell(sn);
                    }
                    catch (Exception ee)
                    {
                        emp_details.AddCell("N/A");
                    }
                    try
                    {
                        PdfPCell REM = new PdfPCell(new Phrase(rows["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        REM.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        emp_details.AddCell(REM);

                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }

                    try
                    {
                        PdfPCell REM = new PdfPCell(new Phrase(rows["Particulars"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        REM.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        emp_details.AddCell(REM);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell DT = new PdfPCell(new Phrase(rows["Expense"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        emp_details.AddCell(DT);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell DT = new PdfPCell(new Phrase(rows["ServiceCharge"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        emp_details.AddCell(DT);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell DT = new PdfPCell(new Phrase(rows["Tax"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        emp_details.AddCell(DT);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell DT = new PdfPCell(new Phrase(rows["TaxAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        emp_details.AddCell(DT);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }
                    try
                    {
                        PdfPCell DT = new PdfPCell(new Phrase(rows["Quantity"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        emp_details.AddCell(DT);
                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }

                    try
                    {
                        PdfPCell DT = new PdfPCell(new Phrase(rows["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        emp_details.AddCell(DT);

                    }
                    catch (Exception eee)
                    {
                        emp_details.AddCell("");
                    }

                }

                document.Add(emp_details);

                PdfPTable totalexp = new PdfPTable(3);
                totalexp.DefaultCell.Padding = 4;
                float[] widths1 = new float[] { 55f, 20f,13f };
                totalexp.SetWidths(widths1);
                totalexp.SpacingBefore = 10;
                totalexp.WidthPercentage = 95f;

                PdfPCell toddddt2wwDD = new PdfPCell(new Phrase("Grand Total: " + ConvertNumbertoWords(Convert.ToDecimal(dt_sum.Rows[0]["Total"])) + " Only", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLDITALIC)));
                toddddt2wwDD.Border = 0;
                toddddt2wwDD.HorizontalAlignment = Element.ALIGN_LEFT;
                totalexp.AddCell(toddddt2wwDD);

                PdfPCell tot = new PdfPCell(new Phrase("Govt. Fee :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot.Border = 0;
                tot.HorizontalAlignment = Element.ALIGN_LEFT;
                totalexp.AddCell(tot);

                PdfPCell tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TExpense"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                tot1.Border = 0;
                tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot1);

                totalexp.AddCell(Empty);

                tot = new PdfPCell(new Phrase("Service charge :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot.Border = 0;
                tot.HorizontalAlignment = Element.ALIGN_LEFT;
                totalexp.AddCell(tot);

                tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TSC"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                tot1.Border = 0;
                tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot1);

                totalexp.AddCell(Empty);

                PdfPCell totw1 = new PdfPCell(new Phrase("VAT :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                totw1.Border = 0;
                totw1.HorizontalAlignment = Element.ALIGN_LEFT;
                totalexp.AddCell(totw1);

                tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["TTAx"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                tot1.Border = 0;
                tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot1);

                totalexp.AddCell(Empty);

                PdfPCell totw1w = new PdfPCell(new Phrase("Grand Total :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                totw1w.Border = 0;
                totw1w.HorizontalAlignment = Element.ALIGN_LEFT;
                totalexp.AddCell(totw1w);

                tot1 = new PdfPCell(new Phrase(dt_sum.Rows[0]["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                tot1.Border = 0;
                tot1.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot1);

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
