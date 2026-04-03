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
    public partial class TaxInvoiceFormat12 : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();
        Master_Bal obj_master = new Master_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"]);
            DataSet ds = obj_report.TaxInvoicePrint11_12(id);
            DataTable dt_inv = ds.Tables[0];
            DataTable dt_invD = ds.Tables[1];
            DataTable dt_cust = ds.Tables[2];
            DataTable dt_sum = ds.Tables[3];
            DataTable dtGeneral = ds.Tables[4];

            Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);

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

            #region header

            PdfPTable table1 = new PdfPTable(5);
            table1.DefaultCell.Padding = 4;
            float[] widths = new float[] { 10f, 10f, 35f, 10f, 10f };
            table1.SetWidths(widths);
            table1.WidthPercentage = 100f;
            table1.SpacingBefore = 10f;

            if (dt_inv.Rows[0]["InvoiceTRN"].ToString() != "")
            {
                PdfPCell sub04 = new PdfPCell(new Phrase("TRN : " + dt_inv.Rows[0]["InvoiceTRN"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                sub04.Border = 0;
                sub04.Colspan = 5;
                sub04.HorizontalAlignment = Element.ALIGN_CENTER;
                table1.AddCell(sub04);
            }
            else
            {
                PdfPCell sub04 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                sub04.Border = 0;
                sub04.Colspan = 5;
                sub04.HorizontalAlignment = Element.ALIGN_CENTER;
                table1.AddCell(sub04);

            }

            PdfPCell lines = new PdfPCell(new Phrase("Invoice No : ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            lines.Border = 0;
            lines.MinimumHeight = 20f;
            lines.HorizontalAlignment = Element.ALIGN_LEFT;
            table1.AddCell(lines);

            lines = new PdfPCell(new Phrase(dt_inv.Rows[0]["Code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            lines.Border = 0;
            lines.HorizontalAlignment = Element.ALIGN_LEFT;
            table1.AddCell(lines);

            if (dt_inv.Rows[0]["InvoiceType"].ToString() == "1")
            {
                lines = new PdfPCell(new Phrase("Tax Invoice", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD)));
                lines.Border = 0;
                lines.MinimumHeight = 20f;
                lines.HorizontalAlignment = Element.ALIGN_CENTER;
                table1.AddCell(lines);
            }
            else if (dt_inv.Rows[0]["InvoiceType"].ToString() == "2")
            {
                lines = new PdfPCell(new Phrase("Invoice", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD)));
                lines.Border = 0;
                lines.MinimumHeight = 20f;
                lines.HorizontalAlignment = Element.ALIGN_CENTER;
                table1.AddCell(lines);
            }

            lines = new PdfPCell(new Phrase("Invoice Date : ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            lines.Border = 0;
            lines.HorizontalAlignment = Element.ALIGN_LEFT;
            table1.AddCell(lines);

            lines = new PdfPCell(new Phrase(dt_inv.Rows[0]["Dates"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            lines.Border = 0;
            lines.HorizontalAlignment = Element.ALIGN_LEFT;
            table1.AddCell(lines);

            document.Add(table1);

            PdfPTable Subhead = new PdfPTable(2);
            Subhead.DefaultCell.Padding = 4;
            widths = new float[] { 50f, 50f };
            Subhead.SetWidths(widths);
            Subhead.WidthPercentage = 100f;

            Phrase ph1 = new Phrase();
            ph1.Add(new Chunk("Customer : ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
            if (dt_inv.Rows[0]["BillingName"].ToString() != "")
                ph1.Add(new Chunk(dt_inv.Rows[0]["BillingName"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            else
                ph1.Add(new Chunk(dt_cust.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            lines = new PdfPCell(ph1);
            lines.Colspan = 2;
            lines.MinimumHeight = 25f;
            lines.HorizontalAlignment = Element.ALIGN_LEFT;
            lines.VerticalAlignment = Element.ALIGN_MIDDLE;
            Subhead.AddCell(lines);

            if (dt_inv.Rows[0]["BillingName"].ToString() == "")
            {
                if (dt_cust.Rows[0]["Address"].ToString() != "")
                {
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Address : ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk(dt_cust.Rows[0]["Addressline"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    lines = new PdfPCell(ph1);
                    lines.Colspan = 2;
                    lines.MinimumHeight = 25f;
                    lines.VerticalAlignment = Element.ALIGN_MIDDLE;
                    lines.HorizontalAlignment = Element.ALIGN_LEFT;
                    Subhead.AddCell(lines);
                }

                if (dt_cust.Rows[0]["TRN"].ToString() != "")
                {
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Contact No : ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk(dt_cust.Rows[0]["Mobile_num"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    lines = new PdfPCell(ph1);
                    lines.MinimumHeight = 25f;
                    lines.VerticalAlignment = Element.ALIGN_MIDDLE;
                    lines.HorizontalAlignment = Element.ALIGN_LEFT;
                    Subhead.AddCell(lines);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Customer TRN : ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk(dt_cust.Rows[0]["TRN"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    lines = new PdfPCell(ph1);
                    lines.MinimumHeight = 25f;
                    lines.VerticalAlignment = Element.ALIGN_MIDDLE;
                    lines.HorizontalAlignment = Element.ALIGN_LEFT;
                    Subhead.AddCell(lines);
                }
                else
                {
                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Contact No : ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    ph1.Add(new Chunk(dt_cust.Rows[0]["Mobile_num"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    lines = new PdfPCell(ph1);
                    lines.Colspan = 2;
                    lines.MinimumHeight = 25f;
                    lines.VerticalAlignment = Element.ALIGN_MIDDLE;
                    lines.HorizontalAlignment = Element.ALIGN_LEFT;
                    Subhead.AddCell(lines);
                }
            }
            if (dt_inv.Rows[0]["Agent"].ToString() != "")
            {
                ph1 = new Phrase();
                ph1.Add(new Chunk("Agent : ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                ph1.Add(new Chunk(dt_inv.Rows[0]["Agent"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                lines = new PdfPCell(ph1);
                lines.Colspan = 2;
                lines.MinimumHeight = 25f;
                lines.VerticalAlignment = Element.ALIGN_MIDDLE;
                lines.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(lines);
            }


            if (dt_inv.Rows[0]["subject"].ToString() != "")
            {
                ph1 = new Phrase();
                ph1.Add(new Chunk("Subject : ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                ph1.Add(new Chunk(dt_inv.Rows[0]["subject"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                lines = new PdfPCell(ph1);
                lines.Colspan = 2;
                lines.MinimumHeight = 25f;
                lines.VerticalAlignment = Element.ALIGN_MIDDLE;
                lines.HorizontalAlignment = Element.ALIGN_LEFT;
                Subhead.AddCell(lines);
            }

            document.Add(Subhead);

            #endregion

            #region data

            if (dt_invD.Rows.Count > 0)
            {
                if (dt_inv.Rows[0]["InvoiceType"].ToString() == "1")
                {
                    PdfPTable emp_details = new PdfPTable(8);

                    emp_details.DefaultCell.Padding = 4;
                    float[] widthsdet = new float[] { 4f, 25f, 10f, 5f, 9f, 9f, 5f, 9f };
                    emp_details.SetWidths(widthsdet);

                    emp_details.SpacingBefore = 20f;
                    emp_details.WidthPercentage = 100f;

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("No.", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    PdfPCell DetailH01 = new PdfPCell(ph1);
                    DetailH01.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(DetailH01);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Service", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    PdfPCell DetailH02 = new PdfPCell(ph1);
                    DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(DetailH02);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Applicant ", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    PdfPCell DetailH03 = new PdfPCell(ph1);
                    DetailH03.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(DetailH03);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Qty ", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    DetailH03 = new PdfPCell(ph1);
                    DetailH03.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(DetailH03);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Amount", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    PdfPCell DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(DetailH04);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Discount", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(DetailH04);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Tax", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(DetailH04);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Total", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.MinimumHeight = 20f;
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(DetailH04);

                    int i = 0;

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
                            PdfPCell REM = new PdfPCell(new Phrase(rows["ParticularsD"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            REM.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            emp_details.AddCell(REM);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("N/A");
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Quantity"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            emp_details.AddCell(DT);

                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("");
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Amount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(DT);

                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("");
                        }

                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Discount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(DT);

                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("");
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["SingleTaxAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(DT);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("");
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(DT);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("");
                        }
                    }

                    PdfPCell summ = new PdfPCell(new Phrase("Total ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    summ.HorizontalAlignment = Element.ALIGN_CENTER;
                    summ.Colspan = 4;
                    emp_details.AddCell(summ);

                    summ = new PdfPCell(new Phrase(dt_sum.Rows[0]["TSCExp"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    summ.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(summ);
                    summ = new PdfPCell(new Phrase(dt_sum.Rows[0]["TDiscount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    summ.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(summ);
                    summ = new PdfPCell(new Phrase(dt_sum.Rows[0]["TTAx"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    summ.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(summ);
                    summ = new PdfPCell(new Phrase(dt_sum.Rows[0]["TotalNoRound"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    summ.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(summ);

                    document.Add(emp_details);

                }

                else if (dt_inv.Rows[0]["InvoiceType"].ToString() == "2")/*Normal Invoice Type*/
                {

                    PdfPTable emp_details = new PdfPTable(7);

                    emp_details.DefaultCell.Padding = 4;
                    float[] widthsdet = new float[] { 4f, 25f, 10f, 5f, 9f, 9f, 9f };
                    emp_details.SetWidths(widthsdet);

                    emp_details.SpacingBefore = 20f;
                    emp_details.WidthPercentage = 100f;

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("No.", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    PdfPCell DetailH01 = new PdfPCell(ph1);
                    DetailH01.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(DetailH01);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Service", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    PdfPCell DetailH02 = new PdfPCell(ph1);
                    if (dt_inv.Rows[0]["ParticularCount"].ToString() == "0")
                        DetailH02.Colspan = 2;
                    DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(DetailH02);

                    if (dt_inv.Rows[0]["ParticularCount"].ToString() != "0")
                    {
                        ph1 = new Phrase();
                        ph1.Add(new Chunk("Applicant ", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                        DetailH02 = new PdfPCell(ph1);
                        DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                        emp_details.AddCell(DetailH02);
                    }

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Qty ", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    DetailH02 = new PdfPCell(ph1);
                    DetailH02.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(DetailH02);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Amount", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    PdfPCell DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(DetailH04);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Discount", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(DetailH04);

                    ph1 = new Phrase();
                    ph1.Add(new Chunk("Total", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD)));
                    DetailH04 = new PdfPCell(ph1);
                    DetailH04.MinimumHeight = 20f;
                    DetailH04.HorizontalAlignment = Element.ALIGN_CENTER;
                    emp_details.AddCell(DetailH04);

                    int i = 0;

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
                            if (dt_inv.Rows[0]["ParticularCount"].ToString() == "0")
                                sn.Colspan = 2;
                            emp_details.AddCell(sn);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("");
                        }
                        if (dt_inv.Rows[0]["ParticularCount"].ToString() != "0")
                        {
                            try
                            {
                                PdfPCell REM = new PdfPCell(new Phrase(rows["ParticularsD"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                                REM.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                emp_details.AddCell(REM);
                            }
                            catch (Exception eee)
                            {
                                emp_details.AddCell("N/A");
                            }
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Quantity"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            emp_details.AddCell(DT);

                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("");
                        }
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Amount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(DT);

                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("");
                        }

                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Discount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(DT);

                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("");
                        }
                        //try
                        //{
                        //    PdfPCell DT = new PdfPCell(new Phrase(rows["SingleTaxAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                        //    DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        //    emp_details.AddCell(DT);
                        //}
                        //catch (Exception eee)
                        //{
                        //    emp_details.AddCell("");
                        //}
                        try
                        {
                            PdfPCell DT = new PdfPCell(new Phrase(rows["Total"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                            DT.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            emp_details.AddCell(DT);
                        }
                        catch (Exception eee)
                        {
                            emp_details.AddCell("");
                        }
                    }

                    PdfPCell summ = new PdfPCell(new Phrase("Total ", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    summ.HorizontalAlignment = Element.ALIGN_CENTER;
                    summ.Colspan = 4;
                   
                    emp_details.AddCell(summ);

                    summ = new PdfPCell(new Phrase(dt_sum.Rows[0]["TSCExp"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    summ.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(summ);
                    summ = new PdfPCell(new Phrase(dt_sum.Rows[0]["TDiscount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    summ.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(summ);
                    //summ = new PdfPCell(new Phrase(dt_sum.Rows[0]["TTAx"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    //summ.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //emp_details.AddCell(summ);
                    summ = new PdfPCell(new Phrase(dt_sum.Rows[0]["TotalNoRound"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    summ.HorizontalAlignment = Element.ALIGN_RIGHT;
                    emp_details.AddCell(summ);

                    document.Add(emp_details);

                }

                PdfPTable totalexp = new PdfPTable(3);
                totalexp.DefaultCell.Padding = 4;
                float[] widths1 = new float[] { 55f, 30f, 15f };
                totalexp.SetWidths(widths1);
                totalexp.SpacingBefore = 10;
                totalexp.WidthPercentage = 100f;

                PdfPCell tot1 = new PdfPCell(new Phrase("AED " + ConvertNumbertoWords(Convert.ToDecimal(dt_sum.Rows[0]["Total"])), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                tot1.Border = 0;
                tot1.Colspan = 3;
                tot1.MinimumHeight = 25f;
                tot1.HorizontalAlignment = Element.ALIGN_CENTER;
                totalexp.AddCell(tot1);

                PdfPCell tot2ww = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot2ww.Border = 0;
                tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot2ww);

                tot2ww = new PdfPCell(new Phrase("Grand Total", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                tot2ww.MinimumHeight = 20f;
                totalexp.AddCell(tot2ww);
                tot2ww = new PdfPCell(new Phrase(dt_sum.Rows[0]["TotalNoRound"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot2ww);

                tot2ww = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot2ww.Border = 0;
                tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot2ww);

                tot2ww = new PdfPCell(new Phrase("Rounded off", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                tot2ww.MinimumHeight = 20f;
                totalexp.AddCell(tot2ww);
                tot2ww = new PdfPCell(new Phrase(dt_inv.Rows[0]["RoundedOff"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot2ww);

                tot2ww = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot2ww.Border = 0;
                tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot2ww);

                tot2ww = new PdfPCell(new Phrase("Discount Allowed", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                tot2ww.MinimumHeight = 20f;
                totalexp.AddCell(tot2ww);
                tot2ww = new PdfPCell(new Phrase(dt_sum.Rows[0]["TDiscount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot2ww);

                tot2ww = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot2ww.Border = 0;
                tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot2ww);

                tot2ww = new PdfPCell(new Phrase("Paid Amount", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                tot2ww.MinimumHeight = 20f;
                totalexp.AddCell(tot2ww);
                tot2ww = new PdfPCell(new Phrase(dt_inv.Rows[0]["Received"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot2ww);

                tot2ww = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot2ww.Border = 0;
                tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot2ww);

                tot2ww = new PdfPCell(new Phrase("Balance", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                tot2ww.MinimumHeight = 20f;
                totalexp.AddCell(tot2ww);
                tot2ww = new PdfPCell(new Phrase(dt_inv.Rows[0]["Receivable"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                tot2ww.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalexp.AddCell(tot2ww);

                if (dtGeneral.Rows[0]["IsAddRemark"].ToString() != "0" && dt_inv.Rows[0]["Remarks"].ToString() !="")
                {
                    tot2ww = new PdfPCell(new Phrase("Remark", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    tot2ww.MinimumHeight = 20f;
                    tot2ww.Colspan = 3;
                    tot2ww.Border = 0;
                    totalexp.AddCell(tot2ww);

                    tot2ww = new PdfPCell(new Phrase(dt_inv.Rows[0]["Remarks"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                    tot2ww.MinimumHeight = 20f;
                    tot2ww.Colspan = 3;
                    tot2ww.Border = 0;
                    totalexp.AddCell(tot2ww);

                   
                }
                if (dtGeneral.Rows[0]["IsAddCreatedByInInvoicePrint"].ToString() == "1")
                {
                    tot2ww = new PdfPCell(new Phrase("Created By : " + dt_inv.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    tot2ww.Border = 0;
                    tot2ww.HorizontalAlignment = Element.ALIGN_LEFT;
                    totalexp.AddCell(tot2ww);

                    tot2ww = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                    tot2ww.Border = 0;
                    tot2ww.Colspan = 2;
                    tot2ww.HorizontalAlignment = Element.ALIGN_CENTER;
                    totalexp.AddCell(tot2ww);
                }
                tot2ww = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                tot2ww.MinimumHeight = 20f;
                tot2ww.Colspan = 3;
                tot2ww.Border = 0;
                totalexp.AddCell(tot2ww);

                tot2ww = new PdfPCell(new Phrase("Authorised Signatory", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                tot2ww.Border = 0;
                tot2ww.HorizontalAlignment = Element.ALIGN_CENTER;
                totalexp.AddCell(tot2ww);

                tot2ww = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
                tot2ww.Border = 0;
                tot2ww.Colspan = 2;
                tot2ww.HorizontalAlignment = Element.ALIGN_CENTER;
                totalexp.AddCell(tot2ww);

                document.Add(totalexp);

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