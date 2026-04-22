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
    public partial class AgreementPdf : System.Web.UI.Page
    {
        Transaction_Bal TransBal = new Transaction_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {

            int leadId = Convert.ToInt32(Request.QueryString["LeadId"]);

            DataSet ds = TransBal.EditLeadCreation(leadId);

            DataRow row = ds.Tables[0].Rows[0];

            Document document = new Document(PageSize.A4, 35f, 35f, 28f, 28f);

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition",
                "inline;filename=Agreement_" + row["CompanyName"].ToString().Replace(",", "") + ".pdf");
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
            document.Open();

            Font fontLabel = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD);
            Font fontValue = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL);
            Font fHead = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD);
            Font fNormal = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL);
            Font fBold = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD);

            if (Application["PrintHeader"] != null && Application["PrintHeader"].ToString() != "")
            {
                string imageURL = Server.MapPath("../UploadedImage/" + Application["PrintHeader"]);
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(new Uri(imageURL));
                jpg.ScaleToFit(520f, 75f);
                jpg.SpacingAfter = 3f;
                jpg.Alignment = Element.ALIGN_CENTER;
                document.Add(jpg);
            }

            // heading
            PdfPTable tblTitle = new PdfPTable(1);
            tblTitle.WidthPercentage = 100;
            tblTitle.SpacingAfter = 2f;

            PdfPCell cellTitle = new PdfPCell(new Phrase(
                "LETTER OF AUTHORITY & SERVICE ACKNOWLEDGEMENT",
                new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD)));
            cellTitle.Border = 0;
            cellTitle.HorizontalAlignment = Element.ALIGN_LEFT;
            cellTitle.PaddingBottom = 2f;
            tblTitle.AddCell(cellTitle);

            PdfPCell cellDate = new PdfPCell(new Phrase(
                "Date:  " + DateTime.Now.ToString("dd/MM/yyyy"),
                new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL)));
            cellDate.Border = 0;
            cellDate.HorizontalAlignment = Element.ALIGN_CENTER;
            cellDate.PaddingBottom = 3f;
            tblTitle.AddCell(cellDate);

            document.Add(tblTitle);

            // table data
            PdfPTable tblInfo = new PdfPTable(2);
            tblInfo.WidthPercentage = 100;
            tblInfo.SpacingBefore = 3f;
            tblInfo.SpacingAfter = 5f;
            tblInfo.SetWidths(new float[] { 25f, 75f });

            Action<string, string> addInfoRow = (label, value) =>
            {
                PdfPCell lbl = new PdfPCell(new Phrase(label, fontLabel));
                lbl.Padding = 3f;
                lbl.HorizontalAlignment = Element.ALIGN_LEFT;
                tblInfo.AddCell(lbl);
                PdfPCell val = new PdfPCell(new Phrase(value, fontValue));
                val.Padding = 3f;
                val.HorizontalAlignment = Element.ALIGN_LEFT;
                tblInfo.AddCell(val);
            };

            addInfoRow("Client Name:", row["CompanyName"].ToString());
            addInfoRow("Passport Number:", row["PassportNo"].ToString());
            addInfoRow("Nationality:", row["Nationality"].ToString());

            string countryCode = row["CountryCodeCN"].ToString().Trim();
            string mobile = row["MobileNumber"].ToString().Trim();
            addInfoRow("Mobile Number:", (countryCode + " " + mobile).Trim());
            addInfoRow("Email Address:", row["EmailId"].ToString());

            document.Add(tblInfo);

            // declareation para
            PdfPTable tblDeclaration = new PdfPTable(1);
            tblDeclaration.WidthPercentage = 100;
            tblDeclaration.SpacingBefore = 3f;
            tblDeclaration.SpacingAfter = 4f;

            Phrase ph1 = new Phrase();
            ph1.Add(new Chunk("I, ", fontValue));
            ph1.Add(new Chunk(row["CompanyName"].ToString() + ", ", fontLabel));
            ph1.Add(new Chunk("holder of passport number ", fontValue));
            ph1.Add(new Chunk(row["PassportNo"].ToString() + ", ", fontLabel));
            ph1.Add(new Chunk("national of ", fontValue));
            ph1.Add(new Chunk(row["Nationality"].ToString(), fontLabel));
            ph1.Add(new Chunk(", hereby instruct and authorise ", fontValue));
            ph1.Add(new Chunk("Rapidzone", fontLabel));
            ph1.Add(new Chunk(" to act on my behalf in relation to the preparation, coordination," +
                              " submission, and follow-up of my ", fontValue));
            ph1.Add(new Chunk("UAE residence visa application", fontLabel));
            ph1.Add(new Chunk(" and all related document processing services.", fontValue));

            PdfPCell cellDeclaration = new PdfPCell(ph1);
            cellDeclaration.Padding = 5f;
            cellDeclaration.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cellDeclaration.Border = Rectangle.NO_BORDER;
            cellDeclaration.SetLeading(0f, 1.3f);
            tblDeclaration.AddCell(cellDeclaration);
            document.Add(tblDeclaration);

            // t and c heading
            PdfPTable tblDeclar = new PdfPTable(1);
            tblDeclar.WidthPercentage = 100;
            tblDeclar.SpacingAfter = 3f;

            PdfPCell cellDeclare = new PdfPCell(new Phrase(
                "I CONFIRM AND AGREE TO THE FOLLOWING TERMS AND CONDITIONS",
                new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD)));
            cellDeclare.Border = 0;
            cellDeclare.HorizontalAlignment = Element.ALIGN_CENTER;
            cellDeclare.PaddingBottom = 2f;
            tblDeclar.AddCell(cellDeclare);
            document.Add(tblDeclar);


            PdfPTable tblTAndC_Page1 = new PdfPTable(3);
            tblTAndC_Page1.WidthPercentage = 100;
            tblTAndC_Page1.SpacingBefore = 3f;
            tblTAndC_Page1.SpacingAfter = 6f;
            tblTAndC_Page1.SetWidths(new float[] { 47f, 6f, 47f });

            // p1 left
            PdfPCell leftCell_Page1 = new PdfPCell();
            leftCell_Page1.Border = Rectangle.NO_BORDER;
            leftCell_Page1.Padding = 3f;

            leftCell_Page1.AddElement(new Paragraph("1. Authority Granted", fHead) { SpacingAfter = 2f });
            leftCell_Page1.AddElement(new Paragraph("I hereby authorise Rapidzone to:", fNormal) { SpacingAfter = 2f });

            Paragraph b1 = new Paragraph(); b1.SpacingAfter = 2f;
            b1.Add(new Chunk("\u2022  ", fNormal));
            b1.Add(new Chunk("prepare, organise, and process my UAE residence visa application and related supporting documents;", fNormal));
            leftCell_Page1.AddElement(b1);

            Paragraph b2 = new Paragraph(); b2.SpacingAfter = 2f;
            b2.Add(new Chunk("\u2022  ", fNormal));
            b2.Add(new Chunk("coordinate with the relevant authorities, authorised channels, typing centres, medical centres, insurance providers, Emirates ID authorities, and other relevant service providers;", fNormal));
            leftCell_Page1.AddElement(b2);

            Paragraph b3 = new Paragraph(); b3.SpacingAfter = 5f;
            b3.Add(new Chunk("\u2022  ", fNormal));
            b3.Add(new Chunk("submit, process, and follow up on all related formalities in accordance with the applicable UAE immigration procedures.", fNormal));
            leftCell_Page1.AddElement(b3);

            leftCell_Page1.AddElement(new Paragraph("2. Host Company Clause", fHead) { SpacingAfter = 2f });
            Paragraph sec2 = new Paragraph(
                "The Client hereby instructs and authorises Rapidzone to process the Client's residence visa application through the Client's given host company, the details of which shall be provided separately by the host company, in accordance with the applicable UAE immigration procedures.",
                fNormal);
            sec2.SpacingAfter = 5f;
            leftCell_Page1.AddElement(sec2);

            leftCell_Page1.AddElement(new Paragraph("3. Total Service Cost", fHead) { SpacingAfter = 2f });
            Phrase ph3 = new Phrase();
            ph3.Add(new Chunk("The total agreed cost for the residence visa processing is ", fNormal));
            ph3.Add(new Chunk("AED 8,500, excluding health insurance", fBold));
            ph3.Add(new Chunk(". The Client shall be responsible for purchasing and paying for the required health insurance separately. Any additional fees, penalties, fines, or charges imposed by any government authority or third party outside the agreed scope of services shall also be payable separately by the Client.", fNormal));
            Paragraph sec3 = new Paragraph(ph3); sec3.SpacingAfter = 5f;
            leftCell_Page1.AddElement(sec3);

            tblTAndC_Page1.AddCell(leftCell_Page1);

            PdfPCell midCell_Page1 = new PdfPCell();
            midCell_Page1.Border = Rectangle.NO_BORDER;
            tblTAndC_Page1.AddCell(midCell_Page1);

            // p1 right
            PdfPCell rightCell_Page1 = new PdfPCell();
            rightCell_Page1.Border = Rectangle.NO_BORDER;
            rightCell_Page1.Padding = 3f;

            rightCell_Page1.AddElement(new Paragraph("4. Security Check Fee", fHead) { SpacingAfter = 2f });
            Phrase ph4 = new Phrase();
            ph4.Add(new Chunk("Where the Client commences the process through a security check, the Client shall pay ", fNormal));
            ph4.Add(new Chunk("AED 700 for the security check.", fBold));
            Paragraph sec4 = new Paragraph(ph4); sec4.SpacingAfter = 2f;
            rightCell_Page1.AddElement(sec4);

            Paragraph b4 = new Paragraph(); b4.SpacingAfter = 2f;
            b4.Add(new Chunk("\u2022  ", fNormal));
            b4.Add(new Chunk("The security check normally takes ", fNormal));
            b4.Add(new Chunk("2 working days", fBold));
            b4.Add(new Chunk(".", fNormal));
            rightCell_Page1.AddElement(b4);

            Paragraph b5 = new Paragraph(); b5.SpacingAfter = 2f;
            b5.Add(new Chunk("\u2022  ", fNormal));
            b5.Add(new Chunk("If the security check result is unsuccessful, the ", fNormal));
            b5.Add(new Chunk("AED 700 shall be strictly non-refundable", fBold));
            b5.Add(new Chunk(".", fNormal));
            rightCell_Page1.AddElement(b5);

            Paragraph b6r = new Paragraph(); b6r.SpacingAfter = 5f;
            b6r.Add(new Chunk("\u2022  ", fNormal));
            b6r.Add(new Chunk("If the security check result is successful, the ", fNormal));
            b6r.Add(new Chunk("AED 700 shall be adjusted toward the total agreed cost of AED 8,500", fBold));
            b6r.Add(new Chunk(".", fNormal));
            rightCell_Page1.AddElement(b6r);

            rightCell_Page1.AddElement(new Paragraph("5. Payment Terms", fHead) { SpacingAfter = 2f });
            rightCell_Page1.AddElement(new Paragraph("The Client agrees to the following payment structure:", fNormal) { SpacingAfter = 2f });
            rightCell_Page1.AddElement(new Paragraph("Standard Payment Plan", fBold) { SpacingAfter = 2f });

            Paragraph b7 = new Paragraph(); b7.SpacingAfter = 2f;
            b7.Add(new Chunk("\u2022  ", fNormal));
            b7.Add(new Chunk("AED 700", fBold));
            b7.Add(new Chunk(" payable in advance to start the security check process;", fNormal));
            rightCell_Page1.AddElement(b7);

            Paragraph b8r = new Paragraph(); b8r.SpacingAfter = 2f;
            b8r.Add(new Chunk("\u2022  ", fNormal));
            b8r.Add(new Chunk("AED 1,300", fBold));
            b8r.Add(new Chunk(" payable upon successful security check and commencement of application preparation;", fNormal));
            rightCell_Page1.AddElement(b8r);

            Paragraph b9r = new Paragraph(); b9r.SpacingAfter = 2f;
            b9r.Add(new Chunk("\u2022  ", fNormal));
            b9r.Add(new Chunk("AED 3,500", fBold));
            b9r.Add(new Chunk(" payable once the application is ready for submission;", fNormal));
            rightCell_Page1.AddElement(b9r);

            Paragraph b10 = new Paragraph(); b10.SpacingAfter = 4f;
            b10.Add(new Chunk("\u2022  ", fNormal));
            b10.Add(new Chunk("the remaining balance", fNormal));
            b10.Add(new Chunk(" payable at the final stage for medical, visa stamping, and Emirates ID processing.", fNormal));
            rightCell_Page1.AddElement(b10);

            rightCell_Page1.AddElement(new Paragraph("Alternative Payment Option", fBold) { SpacingAfter = 2f });
            Phrase ph5alt = new Phrase();
            ph5alt.Add(new Chunk("Following successful security approval, the Client may choose to pay ", fNormal));
            ph5alt.Add(new Chunk("AED 4,800", fBold));
            ph5alt.Add(new Chunk(" directly for the next processing stage.", fNormal));
            Paragraph sec5alt = new Paragraph(ph5alt); sec5alt.SpacingAfter = 5f;
            rightCell_Page1.AddElement(sec5alt);

            tblTAndC_Page1.AddCell(rightCell_Page1);
            document.Add(tblTAndC_Page1);

            //p2
            document.NewPage();


            PdfPTable tblTAndC_Page2 = new PdfPTable(3);
            tblTAndC_Page2.WidthPercentage = 100;
            tblTAndC_Page2.SpacingBefore = 3f;
            tblTAndC_Page2.SpacingAfter = 6f;
            tblTAndC_Page2.SetWidths(new float[] { 47f, 6f, 47f });

            // p2 left
            PdfPCell leftCell_Page2 = new PdfPCell();
            leftCell_Page2.Border = Rectangle.NO_BORDER;
            leftCell_Page2.Padding = 3f;

            leftCell_Page2.AddElement(new Paragraph("Urgent Completion Option", fBold) { SpacingAfter = 2f });
            Phrase phUrgent = new Phrase();
            phUrgent.Add(new Chunk("If the Client requires urgent completion, the Client may pay an additional ", fNormal));
            phUrgent.Add(new Chunk("AED 3,000", fBold));
            phUrgent.Add(new Chunk(", in which case the process may be completed within ", fNormal));
            phUrgent.Add(new Chunk("3 days", fBold));
            phUrgent.Add(new Chunk(", subject to appointment availability, authority requirements, and approvals.", fNormal));
            Paragraph secUrgent1 = new Paragraph(phUrgent); secUrgent1.SpacingAfter = 3f;
            leftCell_Page2.AddElement(secUrgent1);

            Paragraph secUrgent2 = new Paragraph(
                "If the urgent completion option is not selected, the remaining stage relating to medical, visa stamping, and Emirates ID may take up to 45 days, subject to the applicable process, appointment availability, and authority timelines.",
                fNormal);
            secUrgent2.SpacingAfter = 5f;
            leftCell_Page2.AddElement(secUrgent2);

            leftCell_Page2.AddElement(new Paragraph("6. Client Responsibilities", fHead) { SpacingAfter = 2f });
            leftCell_Page2.AddElement(new Paragraph("The Client undertakes to:", fNormal) { SpacingAfter = 2f });

            string[] duties = new string[]
            {
                "provide complete, true, and accurate documents and information;",
                "submit all required documents within the requested timeframe;",
                "attend all required medical, biometric, identification, or related appointments;",
                "comply with all applicable procedural requirements;",
                "make all agreed payments on time."
            };
            foreach (string duty in duties)
            {
                Paragraph bd = new Paragraph(); bd.SpacingAfter = 2f;
                bd.Add(new Chunk("\u2022  ", fNormal));
                bd.Add(new Chunk(duty, fNormal));
                leftCell_Page2.AddElement(bd);
            }

            Paragraph sec6Footer = new Paragraph(
                "Any delay, rejection, penalty, or issue arising from incomplete documentation, inaccurate information, non-attendance, or non-payment shall remain the sole responsibility of the Client.",
                fNormal);
            sec6Footer.SpacingAfter = 3f;
            leftCell_Page2.AddElement(sec6Footer);

            tblTAndC_Page2.AddCell(leftCell_Page2);

            PdfPCell midCell_Page2 = new PdfPCell();
            midCell_Page2.Border = Rectangle.NO_BORDER;
            tblTAndC_Page2.AddCell(midCell_Page2);

            //p2 right
            PdfPCell rightCell_Page2 = new PdfPCell();
            rightCell_Page2.Border = Rectangle.NO_BORDER;
            rightCell_Page2.Padding = 3f;

            rightCell_Page2.AddElement(new Paragraph("7. No Guarantee of Approval", fHead) { SpacingAfter = 2f });
            Paragraph sec7 = new Paragraph(
                "The Client acknowledges that Rapidzone is engaged solely for document processing, submission coordination, and related support services. Final approval, rejection, or delay of the residence visa application remains entirely at the discretion of the relevant UAE authorities and other competent entities. Rapidzone shall not be held responsible for any approval or rejection decision made by the immigration authorities.",
                fNormal);
            sec7.SpacingAfter = 5f;
            rightCell_Page2.AddElement(sec7);

            rightCell_Page2.AddElement(new Paragraph("8. Special Terms for Employment Residence Visa Applications", fHead) { SpacingAfter = 2f });
            Paragraph sec8a = new Paragraph(
                "Where the Client has paid AED 2,000 toward the processing of an employment residence visa, the Client acknowledges and agrees that approval is entirely at the sole discretion of the relevant UAE immigration authority, and Rapidzone has no role, influence, or control over the approval decision.",
                fNormal);
            sec8a.SpacingAfter = 3f;
            rightCell_Page2.AddElement(sec8a);

            Phrase ph8b = new Phrase();
            ph8b.Add(new Chunk("In the event of rejection by the relevant government authority, ", fNormal));
            ph8b.Add(new Chunk("AED 950 shall be non-refundable", fBold));
            ph8b.Add(new Chunk(", as this amount has already been applied toward the initial authority fees paid through the free zone, which are not refundable in the event of rejection. The remaining balance from the amount paid shall be refunded to the Client.", fNormal));
            Paragraph sec8b = new Paragraph(ph8b); sec8b.SpacingAfter = 5f;
            rightCell_Page2.AddElement(sec8b);

            rightCell_Page2.AddElement(new Paragraph("9. Non-Refundable and Third-Party Charges", fHead) { SpacingAfter = 2f });
            rightCell_Page2.AddElement(new Paragraph("The Client further acknowledges and agrees that:", fNormal) { SpacingAfter = 2f });

            Paragraph r1 = new Paragraph(); r1.SpacingAfter = 2f;
            r1.Add(new Chunk("\u2022  ", fNormal));
            r1.Add(new Chunk("the initial ", fNormal));
            r1.Add(new Chunk("AED 700 security check fee is non-refundable", fBold));
            r1.Add(new Chunk(" in the event of an unsuccessful result;", fNormal));
            rightCell_Page2.AddElement(r1);

            Paragraph r2 = new Paragraph(); r2.SpacingAfter = 2f;
            r2.Add(new Chunk("\u2022  ", fNormal));
            r2.Add(new Chunk("any amount paid toward completed work, third-party services, insurance, medical expenses, government fees, or related processing costs may be non-refundable once such costs have been incurred;", fNormal));
            rightCell_Page2.AddElement(r2);

            Paragraph r3 = new Paragraph(); r3.SpacingAfter = 3f;
            r3.Add(new Chunk("\u2022  ", fNormal));
            r3.Add(new Chunk("Rapidzone shall not be held responsible for any refusal, delay, hold, cancellation, or additional requirement imposed by the relevant authorities, host company, or any third party connected to the process.", fNormal));
            rightCell_Page2.AddElement(r3);

            tblTAndC_Page2.AddCell(rightCell_Page2);
            document.Add(tblTAndC_Page2);


            PdfPTable tblAcceptance = new PdfPTable(1);
            tblAcceptance.WidthPercentage = 100;
            tblAcceptance.SpacingBefore = 2f;
            tblAcceptance.SpacingAfter = 6f;

            PdfPCell cellAccHead = new PdfPCell(new Phrase("10. Acceptance", fHead));
            cellAccHead.Border = Rectangle.NO_BORDER;
            cellAccHead.PaddingBottom = 3f;
            tblAcceptance.AddCell(cellAccHead);

            PdfPCell cellAccBody = new PdfPCell(new Phrase(
                "I confirm that I have read, understood, and accepted the contents of this Letter of Authority & Service Acknowledgement. " +
                "I voluntarily authorise Rapidzone to proceed in accordance with the terms stated above.",
                fNormal));
            cellAccBody.Border = Rectangle.NO_BORDER;
            cellAccBody.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cellAccBody.SetLeading(0f, 1.4f);
            cellAccBody.PaddingBottom = 5f;
            tblAcceptance.AddCell(cellAccBody);

            document.Add(tblAcceptance);

            //sign
            PdfPTable tblSig = new PdfPTable(3);
            tblSig.WidthPercentage = 100;
            tblSig.SpacingBefore = 2f;
            tblSig.SpacingAfter = 4f;
            tblSig.SetWidths(new float[] { 40f, 20f, 40f });

            Font fSigLabel = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD);
            Font fSigLine = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL);

            Phrase phClientName = new Phrase();
            phClientName.Add(new Chunk("Client Name: ", fSigLabel));
            phClientName.Add(new Chunk("_______________________________", fSigLine));
            PdfPCell cellSigName = new PdfPCell(phClientName);
            cellSigName.Border = Rectangle.NO_BORDER;
            cellSigName.PaddingBottom = 6f;
            tblSig.AddCell(cellSigName);

            PdfPCell cellSigMid = new PdfPCell(new Phrase(" "));
            cellSigMid.Border = Rectangle.NO_BORDER;
            tblSig.AddCell(cellSigMid);

            Phrase phClientSig = new Phrase();
            phClientSig.Add(new Chunk("Client Signature: ", fSigLabel));
            phClientSig.Add(new Chunk("_______________________", fSigLine));
            PdfPCell cellSigSign = new PdfPCell(phClientSig);
            cellSigSign.Border = Rectangle.NO_BORDER;
            cellSigSign.PaddingBottom = 6f;
            tblSig.AddCell(cellSigSign);

            PdfPCell cellSigDate = new PdfPCell(new Phrase("Date:", fSigLabel));
            cellSigDate.Border = Rectangle.NO_BORDER;
            tblSig.AddCell(cellSigDate);

            PdfPCell cellSigEmpty1 = new PdfPCell(new Phrase(" "));
            cellSigEmpty1.Border = Rectangle.NO_BORDER;
            tblSig.AddCell(cellSigEmpty1);

            PdfPCell cellSigEmpty2 = new PdfPCell(new Phrase(" "));
            cellSigEmpty2.Border = Rectangle.NO_BORDER;
            tblSig.AddCell(cellSigEmpty2);

            document.Add(tblSig);

            document.Close();
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
    }
}