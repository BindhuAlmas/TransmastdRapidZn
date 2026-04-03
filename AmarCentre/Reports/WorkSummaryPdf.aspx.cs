using System;
using System.Web;
using System.Data;
using AmarCentre.BAL;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace AmarCentre.Reports
{
    public partial class WorkSummaryPdf : System.Web.UI.Page
    {
        Report_Bal rep1 = new Report_Bal();
        System_Utilities obj_common = new System_Utilities();
        dtClass dtc = new dtClass();

        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            DateTime FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            DataTable dtEmployee = dtc.returndtmultiple();

            DataSet ds = rep1.WorkSummaryPdf(FromDate, ToDate, dtEmployee);
            DataTable dtbas = ds.Tables[0];
            DataTable dtdet = ds.Tables[1];
            DataTable dtsumry = ds.Tables[2];
            DataTable dtcredit = ds.Tables[3];
            DataTable dtdebit = ds.Tables[4];

            Document document = new Document(PageSize.A4, 20f, 20f, 0f, 20f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=WorkSummaryPdf.pdf");
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
            document.Open();

            if (Application["PrintHeader"] != "")
            {

                PdfPTable ftrtbl = new PdfPTable(1);
                ftrtbl.DefaultCell.PaddingLeft = 10;
                ftrtbl.DefaultCell.FixedHeight = 130f;
                ftrtbl.DefaultCell.Border = 0;
                ftrtbl.SpacingAfter = 5f;
                ftrtbl.WidthPercentage = 100f;

                string imageURL = Server.MapPath("../UploadedImage/" + Application["PrintHeader"]);
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(new Uri(imageURL));
                //Resize image depend upon your need
                jpg.ScaleToFit(550f, 450f);
                //Give space before image
                //Give some space after the image
                jpg.Alignment = Element.ALIGN_CENTER;

                ftrtbl.AddCell(jpg);

                document.Add(ftrtbl);
            }
            PdfPTable incomzdvgsdzbg = new PdfPTable(1);
            incomzdvgsdzbg.DefaultCell.Padding = 4;
            incomzdvgsdzbg.WidthPercentage = 90f;
            PdfPCell cell1 = new PdfPCell(new Phrase("Work Summary", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            cell1.Border = 0;
            cell1.HorizontalAlignment = 1;
            incomzdvgsdzbg.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase("From " + Convert.ToDateTime(FromDate).ToString("dd/MM/yyyy") + " To " + Convert.ToDateTime(ToDate).ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
            cell2.Border = 0;
            cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            incomzdvgsdzbg.AddCell(cell2);

            //PdfPCell cell2 = new PdfPCell(new Phrase("Date " + Convert.ToDateTime(FromDate).ToString("dd/MM/yyyy") , new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
            //cell2.Border = 0;
            //cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            //incomzdvgsdzbg.AddCell(cell2);

            PdfPCell docDate = new PdfPCell(new Phrase("Printed on : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace('-', '/'), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            docDate.Border = 0;
            docDate.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            incomzdvgsdzbg.AddCell(docDate);
            if (dtbas.Rows.Count > 0)
            {
                docDate = new PdfPCell(new Phrase("Employee : " + dtbas.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
                docDate.Border = 0;
                docDate.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                incomzdvgsdzbg.AddCell(docDate);
            }

            document.Add(incomzdvgsdzbg);

            PdfPTable income_details = new PdfPTable(2);
            income_details.DefaultCell.Padding = 4;
            income_details.SpacingBefore = 20;
            income_details.WidthPercentage = 90;
            float[] widths = new float[] { 70f,30f };
            income_details.SetWidths(widths);

            //if (dtinv.Rows.Count > 0)
            {

                PdfPCell Sert = new PdfPCell(new Phrase("Total Invoice Amount", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_LEFT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingLeft = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);
                Sert = new PdfPCell(new Phrase(dtdet.Rows[0]["invoiceAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_RIGHT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingRight = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);

                Sert = new PdfPCell(new Phrase("Received Amount", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_LEFT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingLeft = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);
                Sert = new PdfPCell(new Phrase(dtdet.Rows[0]["ReceivedToday"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_RIGHT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingRight = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);

                Sert = new PdfPCell(new Phrase("Cash IN", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_LEFT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingLeft = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);
                Sert = new PdfPCell(new Phrase(dtsumry.Rows[0]["Credit"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_RIGHT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingRight = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);

                foreach (DataRow r in dtcredit.Rows)
                {
                    if (r["AccountType"].ToString() == "1")
                    {
                        Sert = new PdfPCell(new Phrase(r["AccountName"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                        Sert.HorizontalAlignment = Element.ALIGN_LEFT;
                        Sert.MinimumHeight = 20f;
                        Sert.PaddingLeft = 25f;
                        Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                        income_details.AddCell(Sert);
                        Sert = new PdfPCell(new Phrase(r["Credit"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                        Sert.HorizontalAlignment = Element.ALIGN_RIGHT;
                        Sert.MinimumHeight = 20f;
                        Sert.PaddingRight = 25f;
                        Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                        income_details.AddCell(Sert);
                    }
                }

                Sert = new PdfPCell(new Phrase("Bank IN", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_LEFT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingLeft = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);
                Sert = new PdfPCell(new Phrase(dtsumry.Rows[1]["Credit"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_RIGHT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingRight = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);

                foreach (DataRow r in dtcredit.Rows)
                {
                    if (r["AccountType"].ToString() == "2")
                    {
                        Sert = new PdfPCell(new Phrase(r["AccountName"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                        Sert.HorizontalAlignment = Element.ALIGN_LEFT;
                        Sert.MinimumHeight = 20f;
                        Sert.PaddingLeft = 25f;
                        Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                        income_details.AddCell(Sert);
                        Sert = new PdfPCell(new Phrase(r["Credit"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                        Sert.HorizontalAlignment = Element.ALIGN_RIGHT;
                        Sert.MinimumHeight = 20f;
                        Sert.PaddingRight = 25f;
                        Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                        income_details.AddCell(Sert);
                    }
                }


                Sert = new PdfPCell(new Phrase("PAYMENT", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_LEFT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingLeft = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);
                Sert = new PdfPCell(new Phrase((Convert.ToDecimal(dtsumry.Rows[0]["Debit"]) + Convert.ToDecimal(ds.Tables[2].Rows[1]["Debit"])).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_RIGHT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingRight = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);

                Sert = new PdfPCell(new Phrase("Cash OUT", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_LEFT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingLeft = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);
                Sert = new PdfPCell(new Phrase(dtsumry.Rows[0]["Debit"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_RIGHT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingRight = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);

                foreach (DataRow r in dtdebit.Rows)
                {
                    if (r["AccountType"].ToString() == "1")
                    {
                        Sert = new PdfPCell(new Phrase(r["AccountName"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                        Sert.HorizontalAlignment = Element.ALIGN_LEFT;
                        Sert.MinimumHeight = 20f;
                        Sert.PaddingLeft = 25f;
                        Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                        income_details.AddCell(Sert);
                        Sert = new PdfPCell(new Phrase(r["Debit"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                        Sert.HorizontalAlignment = Element.ALIGN_RIGHT;
                        Sert.MinimumHeight = 20f;
                        Sert.PaddingRight = 25f;
                        Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                        income_details.AddCell(Sert);
                    }
                }

                Sert = new PdfPCell(new Phrase("Bank OUT", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_LEFT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingLeft = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);
                Sert = new PdfPCell(new Phrase(ds.Tables[2].Rows[1]["Debit"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_RIGHT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingRight = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);

                foreach (DataRow r in dtdebit.Rows)
                {
                    if (r["AccountType"].ToString() == "2")
                    {
                        Sert = new PdfPCell(new Phrase(r["AccountName"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                        Sert.HorizontalAlignment = Element.ALIGN_LEFT;
                        Sert.MinimumHeight = 20f;
                        Sert.PaddingLeft = 25f;
                        Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                        income_details.AddCell(Sert);
                        Sert = new PdfPCell(new Phrase(r["Debit"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
                        Sert.HorizontalAlignment = Element.ALIGN_RIGHT;
                        Sert.MinimumHeight = 20f;
                        Sert.PaddingRight = 25f;
                        Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                        income_details.AddCell(Sert);
                    }
                }

                Sert = new PdfPCell(new Phrase("Credit Received", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_LEFT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingLeft = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);
                Sert = new PdfPCell(new Phrase(dtdet.Rows[0]["CreditReceived"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_RIGHT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingRight = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);

                
                Sert = new PdfPCell(new Phrase("Credit", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_LEFT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingLeft = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);
                Sert = new PdfPCell(new Phrase(dtdet.Rows[0]["TodayInvoiceCredit"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_RIGHT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingRight = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);

                Sert = new PdfPCell(new Phrase("Current Credit", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_LEFT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingLeft = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);
                Sert = new PdfPCell(new Phrase(dtdet.Rows[0]["InvoiceCredit"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_RIGHT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingRight = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);

                Sert = new PdfPCell(new Phrase("Processing Completed", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_LEFT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingLeft = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);
                Sert = new PdfPCell(new Phrase(dtdet.Rows[0]["WorkProcessingCompleted"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_RIGHT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingRight = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);

                
                Sert = new PdfPCell(new Phrase("Work In Processing", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_LEFT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingLeft = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);
                Sert = new PdfPCell(new Phrase(dtdet.Rows[0]["TodayWorkProcessing"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_RIGHT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingRight = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);

                Sert = new PdfPCell(new Phrase("Current Work In Processing", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_LEFT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingLeft = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);
                Sert = new PdfPCell(new Phrase(dtdet.Rows[0]["WorkProcessing"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_RIGHT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingRight = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);


                Sert = new PdfPCell(new Phrase("Service Profit", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_LEFT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingLeft = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);
                Sert = new PdfPCell(new Phrase(dtdet.Rows[0]["Serviceprofit"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_RIGHT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingRight = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);

                Sert = new PdfPCell(new Phrase("Net Cash", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_LEFT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingLeft = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);
                Sert = new PdfPCell(new Phrase(ds.Tables[2].Rows[0]["NetAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_RIGHT;
                Sert.MinimumHeight = 25f;
                Sert.PaddingRight = 15f;
                Sert.VerticalAlignment = Element.ALIGN_MIDDLE;
                income_details.AddCell(Sert);


                document.Add(income_details);
            }

            //else
            //{
            //    PdfPTable bill_details4 = new PdfPTable(1);
            //    bill_details4.DefaultCell.Padding = 5;
            //    bill_details4.SpacingBefore = 10;

            //    PdfPCell remarks = new PdfPCell(new Phrase("No Record", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
            //    remarks.Border = 0;
            //    remarks.Colspan = 1;
            //    remarks.HorizontalAlignment = Element.ALIGN_LEFT;
            //    bill_details4.AddCell(remarks);

            //    document.Add(bill_details4);
            //}

            document.Close();
            HttpContext.Current.Response.Write(document);
            HttpContext.Current.Response.End();
        }

    }
}