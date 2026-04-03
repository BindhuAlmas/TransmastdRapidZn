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
    public partial class SalesorderPOS : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();
        Master_Bal obj_master = new Master_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"]);

            DataSet ds = obj_report.SalesOrderPrint(id);
            DataTable dt_inv = ds.Tables[0];
            DataTable dt_invD = ds.Tables[1];
            DataTable dt_cust = ds.Tables[2];
            DataTable dt_sum = ds.Tables[3];
            DataTable dtGeneral = ds.Tables[4];

            float hgt = dt_invD.Rows.Count * 26;

            Rectangle PS = new Rectangle(300, 350 + hgt);
            Document document = new Document(PS, 20f, 20f, 0f, 0f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=SalesOrder.pdf");
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
            document.Open();

            int PH = 0;

            PdfPTable Subhead = new PdfPTable(2);
            Subhead.DefaultCell.Padding = 4;
            float[] widths = new float[] { 35f, 65f };
            Subhead.SetWidths(widths);
            Subhead.SpacingAfter = 10f;
            Subhead.WidthPercentage = 100f;

            PdfPCell sub00 = new PdfPCell(new Phrase(dtGeneral.Rows[0]["CompanyName"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD)));
            sub00.Border = 0;
            sub00.MinimumHeight = 25f;
            sub00.HorizontalAlignment = Element.ALIGN_CENTER;
            sub00.Colspan = 2;
            sub00.VerticalAlignment = Element.ALIGN_TOP;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase("Sales Order", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.Border = 0;
            sub00.MinimumHeight = 20f;
            sub00.HorizontalAlignment = Element.ALIGN_CENTER;
            sub00.Colspan = 2;
            sub00.VerticalAlignment = Element.ALIGN_TOP;
            Subhead.AddCell(sub00);

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
            Cuscell22.Colspan = 2;
            Cuscell22.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            Cuscell22.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            Subhead.AddCell(Cuscell22);

            sub00 = new PdfPCell(new Phrase("Date", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER);
            sub00.MinimumHeight = PH == 1 ? 21f : 26f;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase(dt_inv.Rows[0]["SalesOrderDates"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER);
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase("Customer", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER);
            sub00.MinimumHeight = PH == 1 ? 21f : 26f;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase(dt_cust.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER);
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase("Contact No", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER);
            sub00.MinimumHeight = PH == 1 ? 21f : 26f;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase(dt_cust.Rows[0]["Mobile_num"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER);
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase("Invoice No", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER);
            sub00.MinimumHeight = PH == 1 ? 21f : 26f;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase(dt_inv.Rows[0]["Code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER);
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            foreach (DataRow rows in dt_invD.Rows)
            {
                sub00 = new PdfPCell(new Phrase(rows["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                sub00.DisableBorderSide(Rectangle.TOP_BORDER |  Rectangle.BOTTOM_BORDER);
                sub00.Colspan = 2;
                sub00.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(sub00);

                string total = "";
                if (dt_inv.Rows[0]["DisplDiscount"].ToString() == "1" & rows["AfterDiscount_TotalSO"].ToString() != "")
                    total = rows["AfterDiscount_TotalSO"].ToString();
                else
                    total = rows["Total"].ToString();

                sub00 = new PdfPCell(new Phrase(rows["ParticularsD"].ToString()+"       "+ total, new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                sub00.DisableBorderSide(Rectangle.TOP_BORDER |  Rectangle.BOTTOM_BORDER);
                sub00.PaddingRight = 5f;
                sub00.HorizontalAlignment = Element.ALIGN_RIGHT;
                sub00.Colspan = 2;
                Subhead.AddCell(sub00);
            }

            //sub00 = new PdfPCell(new Phrase("Date", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            //sub00.DisableBorderSide(Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER);
            //sub00.MinimumHeight = PH == 1 ? 21f : 26f;
            //sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            //Subhead.AddCell(sub00);

            //sub00 = new PdfPCell(new Phrase(dt.Rows[0]["Date"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            //sub00.DisableBorderSide(Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER);
            //sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            //Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase("Total Amount", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER);
            sub00.MinimumHeight = PH == 1 ? 21f : 26f;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase(dt_sum.Rows[0]["AfterDiscount_Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER);
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase("Cashier", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER );
            sub00.MinimumHeight = PH == 1 ? 21f : 26f;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase(dt_inv.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER );
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            document.Add(Subhead);


            document.Close();
            HttpContext.Current.Response.Write(document);
            HttpContext.Current.Response.End();
        }

    }
}