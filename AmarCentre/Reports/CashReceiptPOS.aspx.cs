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
    public partial class CashReceiptPOS : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();
        Master_Bal obj_master = new Master_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"]);
            DataSet ds = obj_report.CashReceiptPrint(id);
            DataTable dt = ds.Tables[0];
            DataTable dt_invD = ds.Tables[1];
            DataTable dtgen = ds.Tables[2];
            //DataTable dt_sum = ds.Tables[3];

            Rectangle PS = new Rectangle(300, 300);
            Document document = new Document(PS,25f,25f,0f,0f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=CashReceiptPrint.pdf");
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
            document.Open();

            int PH = 0;
            //if (Application["PrintHeader"] != "")
            //{

            //    string imageURL = Server.MapPath("../UploadedImage/" + Application["PrintHeader"]);
            //    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(new Uri(imageURL));
            //    //Resize image depend upon your need
            //    jpg.ScaleToFit(250f, 230f);
              
            //    jpg.Alignment = Element.ALIGN_CENTER;
            //    PH = 1;
            //    document.Add(jpg);
            //}

            PdfPTable Subhead = new PdfPTable(2);
            Subhead.DefaultCell.Padding = 4;
            float[] widths = new float[] { 30f, 70f };
            Subhead.SetWidths(widths);
            Subhead.WidthPercentage = 100f;

            PdfPCell sub00 = new PdfPCell(new Phrase(dtgen.Rows[0]["CompanyName"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD)));
            sub00.Border = 0;
            sub00.MinimumHeight = 25f;
            sub00.HorizontalAlignment = Element.ALIGN_CENTER;
            sub00.Colspan = 2;
            sub00.VerticalAlignment = Element.ALIGN_TOP;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase("RECEIPT", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.Border = 0;
            sub00.MinimumHeight = 20f;
            sub00.HorizontalAlignment = Element.ALIGN_CENTER;
            sub00.Colspan = 2;
            sub00.VerticalAlignment = Element.ALIGN_TOP;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase("Customer", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER);
            sub00.MinimumHeight = PH == 1 ? 22f : 27f;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase(dt.Rows[0]["CustomerName"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER);
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase("ID Number", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER);
            sub00.MinimumHeight = PH == 1 ? 22f : 27f;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase(dt.Rows[0]["IdNo"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER);
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase("Voucher #", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER);
            sub00.MinimumHeight = PH == 1 ? 22f : 27f;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase(dt.Rows[0]["InvoiceCode"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER);
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase("Date", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER);
            sub00.MinimumHeight = PH == 1 ? 22f : 27f;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase(dt.Rows[0]["Date"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER);
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase("Cashier", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER);
            sub00.MinimumHeight = PH == 1 ? 22f : 27f;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase(dt.Rows[0]["Cashier"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER);
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase("Amount", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide(Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER );
            sub00.MinimumHeight = PH == 1 ? 22f : 27f;
            Subhead.AddCell(sub00);

            sub00 = new PdfPCell(new Phrase(dt.Rows[0]["Amount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.DisableBorderSide( Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER);
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            Subhead.AddCell(sub00);

            document.Add(Subhead);


            document.Close();
            HttpContext.Current.Response.Write(document);
            HttpContext.Current.Response.End();
        }

    }
}