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
    public partial class TaxInvoiceFormat13 : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();
        Master_Bal obj_master = new Master_Bal();


        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"]);
            DataSet ds = obj_report.TaxInvoicePrint13(id);
            DataTable dt_inv = ds.Tables[0];
            DataTable dt_invD = ds.Tables[1];
            DataTable dt_cust = ds.Tables[2];
            DataTable dt_sum = ds.Tables[3];
            DataTable dtGeneral = ds.Tables[4];

            Document document = new Document(PageSize.A4, 20f, 20f, 10f, 0f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=InvoicePrint.pdf");
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
            document.Open();

            PdfPTable table1 = new PdfPTable(6);
            table1.DefaultCell.Padding = 4;
            float[] widths = new float[] { 25f, 15f, 10f, 12f, 10f, 18f };
            table1.SetWidths(widths);
            table1.WidthPercentage = 100f;
            table1.SpacingBefore = 5f;

            if (dt_inv.Rows[0]["InvoiceType"].ToString() == "1")
            {
                PdfPCell lines1 = new PdfPCell(new Phrase("TAX INVOICE", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
                //lines.Border = 0;
                lines1.Colspan = 6;
                lines1.MinimumHeight = 20f;
                lines1.HorizontalAlignment = Element.ALIGN_CENTER;
                table1.AddCell(lines1);
            }
            else if (dt_inv.Rows[0]["InvoiceType"].ToString() == "2")
            {
                PdfPCell lines1 = new PdfPCell(new Phrase(" INVOICE", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
                //lines.Border = 0;
                lines1.Colspan = 6;
                lines1.MinimumHeight = 20f;
                lines1.HorizontalAlignment = Element.ALIGN_CENTER;
                table1.AddCell(lines1);
            }

            if (dtGeneral.Rows[0]["PrintHeader"].ToString() != "")
            {
                string imageURL = Server.MapPath("../UploadedImage/" + dtGeneral.Rows[0]["PrintHeader"].ToString());
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(new Uri(imageURL));
                jpg.ScaleToFit(550f, 450f);

                PdfPCell Fotservice = new PdfPCell(jpg, true);
                //Fotservice.Border = 0;
                Fotservice.Colspan = 6;
                Fotservice.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table1.AddCell(Fotservice);
            }

            #region header

            PdfPCell lines = new PdfPCell(new Phrase("Invoice No : " + dt_inv.Rows[0]["Code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            //lines.Border = 0;
            lines.MinimumHeight = 20f;
            lines.HorizontalAlignment = Element.ALIGN_CENTER;
            table1.AddCell(lines);

            if (dt_inv.Rows[0]["InvoiceType"].ToString() == "1")
            {

                PdfPCell sub04 = new PdfPCell(new Phrase("TRN : " + dt_inv.Rows[0]["InvoiceTRN"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                sub04.Colspan = 3;
                sub04.HorizontalAlignment = Element.ALIGN_CENTER;
                table1.AddCell(sub04);
            }
            else if (dt_inv.Rows[0]["InvoiceType"].ToString() == "2")
            {

                PdfPCell sub04 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                sub04.Colspan = 3;
                sub04.HorizontalAlignment = Element.ALIGN_CENTER;
                table1.AddCell(sub04);
            }


            lines = new PdfPCell(new Phrase("Invoice Date : " + dt_inv.Rows[0]["Dates"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            lines.Colspan = 2;
            lines.HorizontalAlignment = Element.ALIGN_CENTER;
            table1.AddCell(lines);


            Phrase ph1 = new Phrase();
            ph1.Add(new Chunk("Customer : ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            ph1.Add(new Chunk("\n\n" + dt_cust.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
            ph1.Add(new Chunk("\n" + dt_cust.Rows[0]["Addressline"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            lines = new PdfPCell(ph1);
            lines.Colspan = 2;
            lines.Rowspan = 3;
            lines.MinimumHeight = 70f;
            lines.HorizontalAlignment = Element.ALIGN_LEFT;
            table1.AddCell(lines);

            if (dt_inv.Rows[0]["InvoiceType"].ToString() == "1")
            {
                ph1 = new Phrase();
                ph1.Add(new Chunk("Customer TRN : ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                lines = new PdfPCell(ph1);
                lines.Colspan = 2;
                lines.Border = 0;
                lines.HorizontalAlignment = Element.ALIGN_LEFT;
                table1.AddCell(lines);

                ph1 = new Phrase();
                ph1.Add(new Chunk(dt_cust.Rows[0]["TRN"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                lines = new PdfPCell(ph1);
                lines.Colspan = 2;
                lines.Border = Rectangle.RIGHT_BORDER;
                lines.HorizontalAlignment = Element.ALIGN_LEFT;
                table1.AddCell(lines);
            }
            else if (dt_inv.Rows[0]["InvoiceType"].ToString() == "2")
            {
                ph1 = new Phrase();
                ph1.Add(new Chunk("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                lines = new PdfPCell(ph1);
                lines.Colspan = 2;
                lines.Border = 0;
                lines.HorizontalAlignment = Element.ALIGN_LEFT;
                table1.AddCell(lines);

                ph1 = new Phrase();
                ph1.Add(new Chunk("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                lines = new PdfPCell(ph1);
                lines.Colspan = 2;
                lines.Border = Rectangle.RIGHT_BORDER;
                lines.HorizontalAlignment = Element.ALIGN_LEFT;
                table1.AddCell(lines);
            }

           

            ph1 = new Phrase();
            ph1.Add(new Chunk("Contact No : ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            lines = new PdfPCell(ph1);
            lines.Colspan = 2;
            lines.Border = 0;
            lines.HorizontalAlignment = Element.ALIGN_LEFT;
            table1.AddCell(lines);

            ph1 = new Phrase();
            ph1.Add(new Chunk(dt_cust.Rows[0]["Mobile_num"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            lines = new PdfPCell(ph1);
            lines.Colspan = 2;
            lines.Border = Rectangle.RIGHT_BORDER;
            lines.HorizontalAlignment = Element.ALIGN_LEFT;
            table1.AddCell(lines);

            if(dt_inv.Rows[0]["Subject"].ToString()!="")
            {
                ph1 = new Phrase();
                ph1.Add(new Chunk("Subject :", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                lines = new PdfPCell(ph1);
                lines.Colspan = 2;
                lines.Border = 0;
                lines.HorizontalAlignment = Element.ALIGN_LEFT;
                table1.AddCell(lines);

                ph1 = new Phrase();
                ph1.Add(new Chunk(dt_inv.Rows[0]["Subject"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                lines = new PdfPCell(ph1);
                lines.Colspan = 2;
                lines.Border = Rectangle.RIGHT_BORDER;
                lines.HorizontalAlignment = Element.ALIGN_LEFT;
                table1.AddCell(lines);
            }
            else
            {
                ph1 = new Phrase();
                ph1.Add(new Chunk(" ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                lines = new PdfPCell(ph1);
                lines.Colspan = 4;
                lines.Border = Rectangle.RIGHT_BORDER;
                lines.HorizontalAlignment = Element.ALIGN_LEFT;
                table1.AddCell(lines);

            }


            #endregion

            #region data
            if (dt_invD.Rows.Count > 0)
            {
                if (dt_inv.Rows[0]["InvoiceType"].ToString() == "1")
                {
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Description", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    PdfPCell DetailH01 = new PdfPCell(ph1);
                    DetailH01.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH01.Colspan = 2;
                    table1.AddCell(DetailH01);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Qty ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    DetailH01 = new PdfPCell(ph1);
                    DetailH01.HorizontalAlignment = Element.ALIGN_CENTER;
                    table1.AddCell(DetailH01);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Rate", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    PdfPCell DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    table1.AddCell(DetailH04);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Tax %", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    table1.AddCell(DetailH04);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Amount", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.MinimumHeight = 20f;
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    table1.AddCell(DetailH04);

                    int i = 0;

                    foreach (DataRow rows in dt_invD.Rows)
                    {

                        try
                        {
                            PdfPCell sn = new PdfPCell(new Phrase((++i).ToString() + "    " + rows["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            sn.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            sn.Colspan = 2;
                            sn.MinimumHeight = 20f;
                            sn.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            table1.AddCell(sn);
                        }
                        catch (Exception eee)
                        {
                            table1.AddCell("");
                        }


                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Quantity"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            DT.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            table1.AddCell(DT);

                        }
                        catch (Exception eee)
                        {
                            table1.AddCell("");
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Amount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            DT.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            table1.AddCell(DT);

                        }
                        catch (Exception eee)
                        {
                            table1.AddCell("");
                        }


                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["TaxPer"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            DT.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            table1.AddCell(DT);
                        }
                        catch (Exception eee)
                        {
                            table1.AddCell("");
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            DT.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            table1.AddCell(DT);
                        }
                        catch (Exception eee)
                        {
                            table1.AddCell("");
                        }
                    }

                    int rc = dt_invD.Rows.Count;
                    float hgt = 350 - (20 * rc);
                    if (dtGeneral.Rows[0]["PrintFooter"].ToString() != "")
                    {
                        if (hgt > 100)
                            hgt = hgt - 100;
                        else
                            hgt = 0;
                    }

                    PdfPCell DTe = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    DTe.MinimumHeight = hgt;
                    DTe.Colspan = 2;
                    table1.AddCell(DTe);

                    DTe = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    DTe.MinimumHeight = hgt;
                    table1.AddCell(DTe);

                    DTe = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    DTe.MinimumHeight = hgt;
                    table1.AddCell(DTe);

                    DTe = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    DTe.MinimumHeight = hgt;
                    table1.AddCell(DTe);

                    DTe = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    DTe.MinimumHeight = hgt;
                    table1.AddCell(DTe);

                    DTe = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    DTe.Colspan = 2;
                    DTe.Rowspan = 3;
                    table1.AddCell(DTe);

                    DTe = new PdfPCell(new Phrase("Total Amount", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    DTe.Colspan = 3;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                    table1.AddCell(DTe);

                    DTe = new PdfPCell(new Phrase(dt_sum.Rows[0]["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                    table1.AddCell(DTe);

                    DTe = new PdfPCell(new Phrase("Taxable Amount", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    DTe.Colspan = 3;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table1.AddCell(DTe);

                    DTe = new PdfPCell(new Phrase(dt_sum.Rows[0]["TaxableAmt"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table1.AddCell(DTe);

                    DTe = new PdfPCell(new Phrase("Tax Amount", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    DTe.Colspan = 3;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table1.AddCell(DTe);

                    DTe = new PdfPCell(new Phrase(dt_sum.Rows[0]["TTAx"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table1.AddCell(DTe);

                }

                else if (dt_inv.Rows[0]["InvoiceType"].ToString() == "2")
                {
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Description", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    PdfPCell DetailH01 = new PdfPCell(ph1);
                    DetailH01.HorizontalAlignment = Element.ALIGN_CENTER;
                    DetailH01.Colspan = 2;
                    table1.AddCell(DetailH01);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Rate", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    PdfPCell DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    table1.AddCell(DetailH04);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Qty ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    DetailH01 = new PdfPCell(ph1);
                    DetailH01.HorizontalAlignment = Element.ALIGN_CENTER;
                    table1.AddCell(DetailH01);

                    //ph1 = new Phrase();
                    //ph1.Add(new Chunk("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //DetailH04 = new PdfPCell(ph1);
                    //DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    //table1.AddCell(DetailH04);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Amount", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    DetailH04 = new PdfPCell(ph1);
                    //DetailH04.MinimumHeight = 20f;
                    DetailH04.Colspan = 2;
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    table1.AddCell(DetailH04);

                    int i = 0;

                    foreach (DataRow rows in dt_invD.Rows)
                    {

                        try
                        {
                            PdfPCell sn = new PdfPCell(new Phrase((++i).ToString() + "    " + rows["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            sn.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            sn.Colspan = 2;
                            sn.MinimumHeight = 20f;
                            sn.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            table1.AddCell(sn);
                        }
                        catch (Exception eee)
                        {
                            table1.AddCell("");
                        }
                       
                        try
                        {
                            decimal rate = 0;
                            rate = Convert.ToDecimal(rows["Total"]) / Convert.ToDecimal(rows["Quantity"]);
                            //PdfPCell DT = new PdfPCell(new Phrase(rows["Amount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            PdfPCell DT = new PdfPCell(new Phrase(rate.ToString("F2"), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            DT.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            table1.AddCell(DT);

                        }
                        catch (Exception eee)
                        {
                            table1.AddCell("");
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Quantity"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            DT.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            table1.AddCell(DT);

                        }
                        catch (Exception eee)
                        {
                            table1.AddCell("");
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            DT.Colspan = 2;
                            DT.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            table1.AddCell(DT);
                        }
                        catch (Exception eee)
                        {
                            table1.AddCell("");
                        }
                    }

                    int rc = dt_invD.Rows.Count;
                    float hgt = 350 - (20 * rc);
                    if (dtGeneral.Rows[0]["PrintFooter"].ToString() != "")
                    {
                        if (hgt > 100)
                            hgt = hgt - 100;
                        else
                            hgt = 0;
                    }

                    PdfPCell DTe = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    DTe.MinimumHeight = hgt;
                    DTe.Colspan = 2;
                    table1.AddCell(DTe);

                    DTe = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    DTe.MinimumHeight = hgt;
                    table1.AddCell(DTe);

                    DTe = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    DTe.MinimumHeight = hgt;
                    table1.AddCell(DTe);

                    DTe = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DTe.MinimumHeight = hgt;
                    DTe.Colspan = 2;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table1.AddCell(DTe);

                    //DTe = new PdfPCell(new Phrase("555555", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    //DTe.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    ////DTe.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    //DTe.MinimumHeight = hgt;
                    //table1.AddCell(DTe);

                    DTe = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    DTe.Colspan = 2;
                    DTe.Rowspan = 3;
                    table1.AddCell(DTe);

                    DTe = new PdfPCell(new Phrase("Total Amount", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    DTe.Colspan = 3;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                    table1.AddCell(DTe);

                    DTe = new PdfPCell(new Phrase(dt_sum.Rows[0]["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                    table1.AddCell(DTe);

                    DTe = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    DTe.Colspan = 3;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table1.AddCell(DTe);

                    DTe = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table1.AddCell(DTe);

                    DTe = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    DTe.Colspan = 3;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table1.AddCell(DTe);

                    DTe = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    DTe.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    DTe.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table1.AddCell(DTe);

                }
                //////

                PdfPCell summ = new PdfPCell(new Phrase("Amount Chargable ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                summ.HorizontalAlignment = Element.ALIGN_LEFT;
                summ.MinimumHeight = 20;
                table1.AddCell(summ);

                summ = new PdfPCell(new Phrase(ConvertNumbertoWords(Convert.ToDecimal(dt_sum.Rows[0]["Total"])), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                summ.HorizontalAlignment = Element.ALIGN_LEFT;
                summ.Colspan = 4;
                table1.AddCell(summ);

                summ = new PdfPCell(new Phrase("AED         " + dt_sum.Rows[0]["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                summ.HorizontalAlignment = Element.ALIGN_RIGHT;
                table1.AddCell(summ);

                PdfPCell tot2ww = new PdfPCell(new Phrase("Receiver's Signature", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot2ww.HorizontalAlignment = Element.ALIGN_LEFT;
                tot2ww.VerticalAlignment = Element.ALIGN_BOTTOM;
                tot2ww.MinimumHeight = 100f;
                tot2ww.Colspan = 2;
                table1.AddCell(tot2ww);

                tot2ww = new PdfPCell(new Phrase("Authorised Signatory", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot2ww.Colspan = 4;
                tot2ww.HorizontalAlignment = Element.ALIGN_LEFT;
                tot2ww.VerticalAlignment = Element.ALIGN_BOTTOM;
                table1.AddCell(tot2ww);

                document.Add(table1);

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
                    PdfPCell Fotname = new PdfPCell(new Phrase("Software from www.almasit.ae", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
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

                PdfPCell Fotservice = new PdfPCell(new Phrase("Software from www.almasit.ae", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
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