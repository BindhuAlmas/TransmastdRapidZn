using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AmarCentre.BAL;
using System.IO;
using System.Globalization;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace AmarCentre.Reports
{
    public partial class DocumentCollPrint : System.Web.UI.Page
    {
        Report_Bal rep1 = new Report_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {

            int Id;

            try
            {
                Id = Convert.ToInt32(Request.QueryString["id"].ToString());
            }
            catch
            {
                Id = 0;
            }

            DataSet ds2 = rep1.DocumentCollectionPrint(Id);
            DataTable dt2 = ds2.Tables[0];
            DataTable dt2_cust = ds2.Tables[1];

            Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=DocumnentCollection.pdf");
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
            incomzdvgsdzbg.WidthPercentage = 100;
            incomzdvgsdzbg.DefaultCell.Padding = 4;
            PdfPCell cell1 = new PdfPCell(new Phrase("Document Collection Acknowledgement", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            cell1.Border = 0;
            cell1.HorizontalAlignment = 1;
            incomzdvgsdzbg.AddCell(cell1);

            PdfPCell docDate = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace('-', '/'), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            docDate.Border = 0;
            docDate.MinimumHeight = 20;
            docDate.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            incomzdvgsdzbg.AddCell(docDate);

            document.Add(incomzdvgsdzbg);

            if (dt2_cust.Rows.Count > 0)
            {
                PdfPTable headr = new PdfPTable(4);
                headr.DefaultCell.Padding = 4;
                headr.SpacingBefore = 20;
                headr.WidthPercentage = 100;
                float[] widths2 = new float[] { 25f, 75f, 75f, 25f };
                headr.SetWidths(widths2);

                PdfPCell Serial_Nocu = new PdfPCell(new Phrase("Customer : ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Serial_Nocu.HorizontalAlignment = Element.ALIGN_LEFT;
                Serial_Nocu.Border = 0;
                headr.AddCell(Serial_Nocu);
                PdfPCell Sertty = new PdfPCell(new Phrase(dt2_cust.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sertty.HorizontalAlignment = Element.ALIGN_LEFT;
                Sertty.Border = 0;
                headr.AddCell(Sertty);

                PdfPCell Serial_Nocudd = new PdfPCell(new Phrase("Code : ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Serial_Nocudd.HorizontalAlignment = Element.ALIGN_RIGHT;
                Serial_Nocudd.Border = 0;
                headr.AddCell(Serial_Nocudd);
                PdfPCell Serttyss = new PdfPCell(new Phrase(dt2_cust.Rows[0]["Code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Serttyss.HorizontalAlignment = Element.ALIGN_LEFT;
                Serttyss.Border = 0;
                headr.AddCell(Serttyss);

                PdfPCell SralAdress = new PdfPCell(new Phrase("Address : ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                SralAdress.HorizontalAlignment = Element.ALIGN_LEFT;
                SralAdress.Border = 0;
                headr.AddCell(SralAdress);
                PdfPCell Serttysssad = new PdfPCell(new Phrase(dt2_cust.Rows[0]["Address"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Serttysssad.HorizontalAlignment = Element.ALIGN_LEFT;
                Serttysssad.Border = 0;
                headr.AddCell(Serttysssad);

                PdfPCell Serial_Nocuss = new PdfPCell(new Phrase("Date : ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Serial_Nocuss.HorizontalAlignment = Element.ALIGN_RIGHT;
                Serial_Nocuss.Rowspan = 4;
                Serial_Nocuss.Border = 0;
                headr.AddCell(Serial_Nocuss);
                PdfPCell Serttysss = new PdfPCell(new Phrase(dt2_cust.Rows[0]["Ondate"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Serttysss.HorizontalAlignment = Element.ALIGN_LEFT;
                Serttysss.Border = 0;
                Serttysss.Rowspan = 4;
                headr.AddCell(Serttysss);

                PdfPCell SralAdresssaaf = new PdfPCell(new Phrase("Remark : ", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                SralAdresssaaf.HorizontalAlignment = Element.ALIGN_LEFT;
                SralAdresssaaf.Border = 0;
                headr.AddCell(SralAdresssaaf);
                PdfPCell Serttysssqqad = new PdfPCell(new Phrase(dt2_cust.Rows[0]["Description"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Serttysssqqad.HorizontalAlignment = Element.ALIGN_LEFT;
                Serttysssqqad.Border = 0;
                headr.AddCell(Serttysssqqad);

                document.Add(headr);

                PdfPTable income_details = new PdfPTable(7);
                income_details.DefaultCell.Padding = 4;
                income_details.SpacingBefore = 20;
                income_details.WidthPercentage = 100;
                float[] widths = new float[] { 10f, 20f, 15f, 20f, 15f, 15f, 20f };
                income_details.SetWidths(widths);
                PdfPCell Serial_No = new PdfPCell(new Phrase("Sl No", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Serial_No.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Serial_No);
                PdfPCell Sert = new PdfPCell(new Phrase("Document", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sert.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Sert);
                PdfPCell Ser = new PdfPCell(new Phrase("Document Number", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Ser.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Ser);
                PdfPCell Qua = new PdfPCell(new Phrase("Particulars", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Qua.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Qua);
                PdfPCell Sertf = new PdfPCell(new Phrase("Valid from", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Sertf.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Sertf);
                PdfPCell Serv = new PdfPCell(new Phrase("Valid To", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Serv.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Serv);
                PdfPCell Quar = new PdfPCell(new Phrase("Remark", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                Quar.HorizontalAlignment = Element.ALIGN_CENTER;
                income_details.AddCell(Quar);

                int i = 0;
                foreach (DataRow rows in dt2.Rows)
                {
                    try
                    {
                        PdfPCell serial_no = new PdfPCell(new Phrase((++i).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        serial_no.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        income_details.AddCell(serial_no);
                    }
                    catch (Exception ee)
                    {
                        income_details.AddCell("N/A");
                    }
                    try
                    {
                        PdfPCell typee = new PdfPCell(new Phrase(rows["Doc_name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        typee.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        income_details.AddCell(typee);
                    }
                    catch (Exception eee)
                    {
                        income_details.AddCell("N/A");
                    }
                    try
                    {
                        PdfPCell typee = new PdfPCell(new Phrase(rows["Doc_num"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        typee.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        income_details.AddCell(typee);
                    }
                    catch (Exception eee)
                    {
                        income_details.AddCell("N/A");
                    }

                    try
                    {
                        PdfPCell Stypee = new PdfPCell(new Phrase(rows["Remark"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        Stypee.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        income_details.AddCell(Stypee);
                    }
                    catch (Exception eee)
                    {
                        income_details.AddCell("N/A");
                    }
                    try
                    {
                        PdfPCell Stypee = new PdfPCell(new Phrase(rows["VFrom"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        Stypee.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        income_details.AddCell(Stypee);
                    }
                    catch (Exception eee)
                    {
                        income_details.AddCell("N/A");
                    }
                    try
                    {
                        PdfPCell Stypee = new PdfPCell(new Phrase(rows["VTo"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        Stypee.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        income_details.AddCell(Stypee);
                    }
                    catch (Exception eee)
                    {
                        income_details.AddCell("N/A");
                    }
                    try
                    {
                        PdfPCell Stypee = new PdfPCell(new Phrase(rows["NewRemark"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                        Stypee.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        income_details.AddCell(Stypee);
                    }
                    catch (Exception eee)
                    {
                        income_details.AddCell("N/A");
                    }
                }
                document.Add(income_details);
            }

            else
            {
                PdfPTable bill_details4 = new PdfPTable(1);
                bill_details4.DefaultCell.Padding = 5;
                bill_details4.SpacingBefore = 10;

                PdfPCell remarks = new PdfPCell(new Phrase("No Record", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL)));
                remarks.Border = 0;
                remarks.Colspan = 1;
                remarks.HorizontalAlignment = Element.ALIGN_LEFT;
                bill_details4.AddCell(remarks);

                document.Add(bill_details4);
            }

            document.Close();
            HttpContext.Current.Response.Write(document);
            HttpContext.Current.Response.End();
        }
    }
}