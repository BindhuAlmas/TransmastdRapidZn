using AmarCentre.BAL;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AmarCentre.Reports
{
    public partial class CustomerBalanceDetail : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime? FromDate = null, ToDate = null;
            try
            {
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }
            catch (Exception cc)
            {
                FromDate = null;
            }
            try
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }
            catch (Exception cc)
            {
                ToDate = null;
            }
            int CustomerId = Convert.ToInt32(Request.QueryString["CustomerId"]);

            
            DataSet ds = obj_report.CustomerBalanceDetail_Pdf(FromDate, ToDate, CustomerId);
            DataTable dtCus= ds.Tables[0];
            DataTable dtRec = ds.Tables[1];
            DataTable dtPay = ds.Tables[2];
            DataTable dtRecTot = ds.Tables[3];
            DataTable dtPayTot = ds.Tables[4];
            DataTable dt_date = ds.Tables[5];


            Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=CustomerSOAPrint.pdf");
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

            PdfPTable headTable = new PdfPTable(1);
            headTable.DefaultCell.Padding = 4;
            float[] headTableWidths = new float[] { 120f };
            headTable.SetWidths(headTableWidths);
            headTable.WidthPercentage = 95f;

            PdfPCell HT00 = new PdfPCell(new Phrase("Customer Balance Detail ", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            HT00.HorizontalAlignment = Element.ALIGN_CENTER;
            HT00.Border = 0;
            headTable.AddCell(HT00);
            /*End of Row*/
            

            HT00 = new PdfPCell(new Phrase("From : " + dt_date.Rows[0]["FromDate"].ToString() + " To : " + dt_date.Rows[0]["ToDate"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            HT00.HorizontalAlignment = Element.ALIGN_CENTER;
            HT00.Border = 0;
            headTable.AddCell(HT00);

            document.Add(headTable);


            PdfPTable subHeadTable = new PdfPTable(1);
            subHeadTable.DefaultCell.Padding = 4;
            float[] subHeadTableWidths = new float[] { 120f };
            subHeadTable.SetWidths(subHeadTableWidths);
            subHeadTable.WidthPercentage = 95f;

            PdfPCell sub00 = new PdfPCell(new Phrase("Date : " + DateTime.Now, new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL)));
            sub00.Border = 0;
            sub00.HorizontalAlignment = Element.ALIGN_RIGHT;
            subHeadTable.AddCell(sub00);
            /*End of Row*/

            sub00 = new PdfPCell(new Phrase("Customer :    " + dtCus.Rows[0]["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            sub00.Border = 0;
            sub00.HorizontalAlignment = Element.ALIGN_LEFT;
            subHeadTable.AddCell(sub00);
            /*End of Row*/

            document.Add(subHeadTable);

            #endregion

            #region data

            PdfPTable income_details = new PdfPTable(5);
            income_details.DefaultCell.Padding = 4;
            income_details.SpacingBefore = 5;
            income_details.WidthPercentage = 100;
            float[] widths = new float[] { 7f, 10f, 20f, 20f,10f };
            income_details.SetWidths(widths);


            if (dtRec.Rows.Count > 0 || dtPay.Rows.Count > 0)
            {
               if (dtRec.Rows.Count > 0)
                {
                    PdfPCell extraLine = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10)));
                    extraLine.HorizontalAlignment = Element.ALIGN_LEFT;
                    extraLine.Border = 0;
                    extraLine.Colspan = 9;
                    extraLine.MinimumHeight = 15;
                    income_details.AddCell(extraLine);

                    PdfPCell invhd = new PdfPCell(new Phrase("Received Amount Details", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD)));
                    invhd.HorizontalAlignment = Element.ALIGN_LEFT;
                    invhd.Border = 0;
                    invhd.Colspan = 9;
                    invhd.MinimumHeight = 20;
                    income_details.AddCell(invhd);

                    income_details.AddCell(extraLine);


                    PdfPCell detailHead = new PdfPCell(new Phrase("Sl No", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                    income_details.AddCell(detailHead);

                    detailHead = new PdfPCell(new Phrase("Date", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                    income_details.AddCell(detailHead);

                    detailHead = new PdfPCell(new Phrase(" Code", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                    income_details.AddCell(detailHead);
                    

                    detailHead = new PdfPCell(new Phrase(" Remarks", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                    income_details.AddCell(detailHead);

                    detailHead = new PdfPCell(new Phrase(" Amount", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD)));
                    detailHead.HorizontalAlignment = Element.ALIGN_CENTER;
                    income_details.AddCell(detailHead);


                    PdfPCell emptyDetail = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    emptyDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    emptyDetail.Border = 0;

                    PdfPCell detailCell = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                    int i = 0;
                    foreach (DataRow rows in dtRec.Rows)
                    {
                        try
                        {
                            PdfPCell sn = new PdfPCell(new Phrase((++i).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            sn.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            income_details.AddCell(sn);
                        }
                        catch (Exception ee)
                        {
                            income_details.AddCell("N/A");
                        }                        
                        try
                        {
                            detailCell = new PdfPCell(new Phrase(rows["Date"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            detailCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            income_details.AddCell(detailCell);
                        }
                        catch (Exception ee)
                        {
                            income_details.AddCell(emptyDetail);
                        }
                        try
                        {
                            detailCell = new PdfPCell(new Phrase(rows["code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            detailCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            income_details.AddCell(detailCell);
                        }
                        catch (Exception ee)
                        {
                            income_details.AddCell(emptyDetail);
                        }
                        try
                        {
                            detailCell = new PdfPCell(new Phrase(rows["Remarks"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            detailCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            income_details.AddCell(detailCell);
                        }
                        catch (Exception ee)
                        {
                            income_details.AddCell(emptyDetail);
                        }
                        try
                        {
                            detailCell = new PdfPCell(new Phrase(rows["RVAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            detailCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            income_details.AddCell(detailCell);
                        }
                        catch (Exception ee)
                        {
                            income_details.AddCell(emptyDetail);
                        }
                       


                    }

                    PdfPCell tots = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    tots.HorizontalAlignment = Element.ALIGN_RIGHT;
                    tots.Colspan = 4;
                    tots.Border = 0;
                    income_details.AddCell(tots);

                    PdfPCell totiddnv = new PdfPCell(new Phrase(dtRecTot.Rows[0]["TotRvAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    totiddnv.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totiddnv.Border = 0;
                    income_details.AddCell(totiddnv);


                    document.Add(income_details);
                }

                if (dtPay.Rows.Count > 0)
                {
                    PdfPTable Paymnt = new PdfPTable(5);
                    Paymnt.DefaultCell.Padding =4 ;
                    Paymnt.SpacingBefore = 20;
                    Paymnt.WidthPercentage = 100;
                    float[] widthsp = new float[] { 7f, 10f, 10f,20f, 10f };
                    Paymnt.SetWidths(widthsp);

                    PdfPCell irechd = new PdfPCell(new Phrase("Work Completed Details", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD)));
                    irechd.HorizontalAlignment = Element.ALIGN_LEFT;
                    irechd.Border = 0;
                    irechd.Colspan = 5;
                    irechd.MinimumHeight = 20;
                    Paymnt.AddCell(irechd);

                    PdfPCell extraLine = new PdfPCell(new Phrase("", new Font(Font.FontFamily.TIMES_ROMAN, 10)));
                    extraLine.HorizontalAlignment = Element.ALIGN_LEFT;
                    extraLine.Border = 0;
                    extraLine.Colspan = 9;
                    extraLine.MinimumHeight = 15; 
                    Paymnt.AddCell(extraLine);

                    PdfPCell Serial_Norec = new PdfPCell(new Phrase("Sl No", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    Serial_Norec.HorizontalAlignment = Element.ALIGN_CENTER;
                    Paymnt.AddCell(Serial_Norec);
                    PdfPCell Serss = new PdfPCell(new Phrase("Date", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    Serss.HorizontalAlignment = Element.ALIGN_CENTER;
                    Paymnt.AddCell(Serss);
                    PdfPCell Sertre = new PdfPCell(new Phrase("Code", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    Sertre.HorizontalAlignment = Element.ALIGN_CENTER;
                    Paymnt.AddCell(Sertre);

                    PdfPCell Sersrems = new PdfPCell(new Phrase("Service", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    Sersrems.HorizontalAlignment = Element.ALIGN_CENTER;
                    Paymnt.AddCell(Sersrems);

                    PdfPCell Sersrem = new PdfPCell(new Phrase("Amount", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    Sersrem.HorizontalAlignment = Element.ALIGN_CENTER;
                    Paymnt.AddCell(Sersrem);
                   

                    int i = 0;
                    foreach (DataRow rows in dtPay.Rows)
                    {
                        try
                        {
                            PdfPCell serial_no = new PdfPCell(new Phrase((++i).ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            serial_no.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            Paymnt.AddCell(serial_no);
                        }
                        catch (Exception ee)
                        {
                            Paymnt.AddCell("");
                        }
                        try
                        {
                            PdfPCell typee = new PdfPCell(new Phrase(rows["Dated"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            typee.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            Paymnt.AddCell(typee);
                        }
                        catch (Exception eee)
                        {
                            Paymnt.AddCell("");
                        }
                        try
                        {
                            PdfPCell typee = new PdfPCell(new Phrase(rows["Code"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            typee.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            Paymnt.AddCell(typee);
                        }
                        catch (Exception eee)
                        {
                            Paymnt.AddCell("");
                        }
                        try
                        {
                            PdfPCell typee = new PdfPCell(new Phrase(rows["Name"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            typee.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            Paymnt.AddCell(typee);
                        }
                        catch (Exception eee)
                        {
                            Paymnt.AddCell("");
                        }
                        try
                        {
                            PdfPCell typee = new PdfPCell(new Phrase(rows["SCAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL)));
                            typee.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            Paymnt.AddCell(typee);
                        }
                        catch (Exception eee)
                        {
                            Paymnt.AddCell("");
                        }
                                                
                    }

                    PdfPCell tots = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    tots.HorizontalAlignment = Element.ALIGN_RIGHT;
                    tots.Colspan = 4;
                    tots.Border = 0;
                    Paymnt.AddCell(tots);

                    PdfPCell totiddnv = new PdfPCell(new Phrase(dtPayTot.Rows[0]["TotSCAmount"].ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
                    totiddnv.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totiddnv.Border = 0;
                    Paymnt.AddCell(totiddnv);


                   
                    

                    document.Add(Paymnt);
                }

            }

            #endregion

            document.Close();
            HttpContext.Current.Response.Write(document);
            HttpContext.Current.Response.End();
        }
    }
}