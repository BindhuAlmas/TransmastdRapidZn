using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using AmarCentre.BAL;
using System.Globalization;
using Telerik.Web.UI;
using iTextSharp.text.pdf;
using System.Drawing.Printing;
using System.Drawing.Text;

namespace AmarCentre.Transactions
{
    public partial class CreditNote : System.Web.UI.Page
    {
        Transaction_Bal obj_trans = new Transaction_Bal();
        Master_Bal obj_mas = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
        Report_Bal obj_report = new Report_Bal();
        Voucher BalVoucher = new Voucher();
        public int ReceiptIdpub;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
            }
            if (!IsPostBack)
            {
                hdn_user_id.Value = Session["User_Id"].ToString();
                previlage_check();
                previlage_action_check();
                Clear();
                grid_fill(1, 10, "", "", "");
                fill_Customer();
            }
        }

        public void fill_Customer()
        {
            drp_customer.Items.Clear();
            DataTable dt = obj_trans.Drp_Customer();
            drp_customer.DataSource = dt;
            drp_customer.DataTextField = "Text";
            drp_customer.DataValueField = "Value";
            drp_customer.DataBind();
        }

        protected void drp_customer_OnSelectedIndexChanged(Object sender, EventArgs e)
        {
            DataTable dt = obj_trans.DrpPendingCreditInvoice(drp_customer.SelectedValue == "" ? 0 : Convert.ToInt32(drp_customer.SelectedValue));
            drpInvoice.DataSource = dt;
            drpInvoice.DataTextField = "Code";
            drpInvoice.DataValueField = "Id";
            drpInvoice.DataBind();
            drpInvoice.ClearSelection();
            drpInvoice.Text = "";

            updinvoiceDrp.Update();
        }

        protected void drpInvoiceOnSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (drpInvoice.SelectedValue != "")
            {
                DataSet ds = obj_trans.Get_CreditInvoice(Convert.ToInt32(drpInvoice.SelectedValue), Convert.ToInt32(hdn_user_id.Value));
                DataTable dt_ser = ds.Tables[0];/* Detail*/
                DataTable dt1 = ds.Tables[1];/*invoic*/

                if (dt1.Rows.Count > 0)
                {
                    txt_grand.Text = dt1.Rows[0]["TotalAmount"].ToString();
                    txtTotalTax.Text = dt1.Rows[0]["TotalTax"].ToString();

                    rpt_Item_list.DataSource = dt_ser;
                    rpt_Item_list.DataBind();
                }
                Upd_Add_PanelInner.Update();

            }
            else
                Clear();
        }

        //Get List 
        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_trans.Get_ListCreditnote(page_number, page_size, filter, column, order, Convert.ToInt32(hdn_user_id.Value));
            rpt_list.DataSource = dt;
            rpt_list.DataBind();
            if (dt.Rows.Count > 0)
            {
                lbl_page_info.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["RowNum"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
                hdn_filter.Value = dt.Rows[0]["filter"].ToString();
                Common_order_column.Value = dt.Rows[0]["column_name"].ToString();
                Common_asc_desc.Value = dt.Rows[0]["asc_desc"].ToString();
                hdn_last_page.Value = dt.Rows[0]["last_page"].ToString();
                lbl_page_number.Text = dt.Rows[0]["page_number"].ToString();
                hdn_total.Value = dt.Rows[0]["current_count"].ToString();
            }
            else
            {
                lbl_page_info.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
                hdn_filter.Value = txt_search.Text;
                hdn_last_page.Value = "0";
                lbl_page_number.Text = "1";
                hdn_total.Value = "0";
            }
            Upd_List_Panel.Update();
            Upd_Nav_Panel.Update();
        }

        /*Export To Excel*/
        public void btnexcel_export_OnClick(object sender, EventArgs e)
        {
            DataTable dt = obj_trans.Get_ListCreditnoteExcel(Convert.ToInt32(hdn_user_id.Value));
            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "CreditNote");
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                HttpContext.Current.Response.Write(style);
                Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        protected void rpt_list_OnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdn_id");
            if (e.CommandName == "Edit")
            {
                Clear();
                pnl_add.Visible = true;

                DataSet ds = obj_trans.EditCreditnote(Convert.ToInt32(hdn_rpt_id.Value), Convert.ToInt32(hdn_user_id.Value));
                DataTable dt1 = ds.Tables[0];/*invoic*/
                DataTable dt_ser = ds.Tables[1];/* Detail*/

                hdn_id.Value = dt1.Rows[0]["Id"].ToString();
                lbl_Code.Text = dt1.Rows[0]["Code"].ToString();
                job_date.DbSelectedDate = dt1.Rows[0]["Dated"].ToString();
                drp_customer.SelectedValue = dt1.Rows[0]["CustomerId"].ToString();
                RadComboBoxItem CodeItem = new RadComboBoxItem();
                CodeItem.Text = dt1.Rows[0]["InvoiceCode"].ToString();
                CodeItem.Value = dt1.Rows[0]["InvoiceId"].ToString();
                drpInvoice.Items.Insert(0, CodeItem);
                drpInvoice.SelectedValue = dt1.Rows[0]["InvoiceId"].ToString();

                drpInvoice.Enabled = drp_customer.Enabled = false;

                txtTotalTax.Text = dt1.Rows[0]["TotalTax"].ToString();
                txt_grand.Text = dt1.Rows[0]["TotalAmount"].ToString();
                txt_remark.Text = dt1.Rows[0]["Remarks"].ToString();
               
                rpt_Item_list.DataSource = dt_ser;
                rpt_Item_list.DataBind();

                btn_save_print.Visible = btn_save.Visible = btn_cancel.Visible = false;
                //btn_save_print.Visible = hdnupdateNPrint.Value == "0" ? false : true;
                btn_print.Visible = hdn_print.Value == "0" ? false : true;
                if (dt1.Rows[0]["Statusid"].ToString() == "1")
                    btn_cancel.Visible = hdncancel.Value == "0" ? false : true;

                Upd_Add_Panel.Update();
            }
            else if (e.CommandName == "Print")
            {
                string url = "../Reports/CreditNotePrint.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
            }
        }

        protected void rpt_list_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Button btnPrint = (Button)e.Item.FindControl("btnPrint");
            btnPrint.Visible = hdn_print.Value == "0" ? false : true;
        }

        /*Save*/
        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            int res = SaveCreditNote();
            if (res > 0)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                lbl_msgin.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lblerrormsg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
            }
            //pnl_add.Visible = false;
            Upd_Add_PanelInner.Update();
        }

        public int SaveCreditNote()
        {
            DataTable dt_deatils = fill_Detail();

            int res = 0;
            if (dt_deatils.Rows.Count > 0)
            {
                res = obj_trans.Insert_UpdateCreditnote(Convert.ToInt32(hdn_id.Value), job_date.SelectedDate,
                    Convert.ToInt32(drpInvoice.SelectedValue), Convert.ToInt32(drp_customer.SelectedValue), txt_remark.Text,
                    txtTotalTax.Text == "" ? 0 : Convert.ToDecimal(txtTotalTax.Text),
                    Convert.ToDecimal(txt_grand.Text), dt_deatils, Convert.ToInt32(hdn_user_id.Value));
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Add Service to Continue.!');", true);
            }
            return res;
        }

        /*Save & Print*/  // not using
        protected void btn_save_print_OnClick(object sender, EventArgs e)
        {
            int res = SaveCreditNote();

            if (res > 0)
            {
                string url = "../Reports/CreditNotePrint.aspx?id=" + res;
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);

                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                lbl_msgin.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msgin.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            Upd_Add_PanelInner.Update();
        }
        protected void btn_cancel_OnClick(object sender, EventArgs e)
        {
            int res = obj_trans.CancelCreditnote(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_user_id.Value));
            if (res > 0)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                lbl_msgin.Text = "Cancelled Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lblerrormsg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
            }
            Upd_Add_PanelInner.Update();
        }
        protected void btn_print_OnClick(object sender, EventArgs e)
        {
            string url = "../Reports/CreditNotePrint.aspx?id=" + Convert.ToInt32(hdn_id.Value);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        public DataTable fill_Detail()
        {
            DataTable dt_ser = new DataTable();
            dt_ser.Columns.Add("D_id", typeof(int));
            dt_ser.Columns.Add("InvoiceDetId", typeof(int));
            dt_ser.Columns.Add("Quantity", typeof(decimal));
            dt_ser.Columns.Add("Tax", typeof(decimal));
            dt_ser.Columns.Add("Total", typeof(decimal));

            decimal tax = 0, Total = 0;
            foreach (RepeaterItem itm in rpt_Item_list.Items)
            {
                HiddenField hdn_D_id = (HiddenField)itm.FindControl("hdn_D_id");
                HiddenField hdnInvoiceDetId = (HiddenField)itm.FindControl("hdnInvoiceDetId");
                TextBox txt_Qty = (TextBox)itm.FindControl("txt_Qty");
                TextBox txt_taxamt = (TextBox)itm.FindControl("txt_tax");
                TextBox txt_totPrice = (TextBox)itm.FindControl("txt_totPrice");
                CheckBox chksel = (CheckBox)itm.FindControl("chksel");
                if (chksel.Checked==true && txt_Qty.Text!=""&& txt_totPrice.Text!="")
                {
                    dt_ser.Rows.Add(Convert.ToInt32(hdn_D_id.Value), Convert.ToInt32(hdnInvoiceDetId.Value), Convert.ToDecimal(txt_Qty.Text),
                       txt_taxamt.Text==""?0: Convert.ToDecimal(txt_taxamt.Text), Convert.ToDecimal(txt_totPrice.Text));
                    tax = tax + Convert.ToDecimal(txt_taxamt.Text);
                    Total = Total + Convert.ToDecimal(txt_totPrice.Text);
                }
            }
            txtTotalTax.Text = tax.ToString();
            txt_grand.Text = Total.ToString();
            Upd_ItemList.Update();
            return dt_ser;
        }
       
        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
        }

        #region Navigation

        /*txt_search OnTextChanged*/
        protected void txt_search_OnTextChanged(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), txt_search.Text, Common_order_column.Value, Common_asc_desc.Value);
        }

        //First Page
        protected void btn_first_OnClick(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
        }

        //Previous Page
        protected void btn_prev_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number.Text) > 1)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text) - 1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
            }
        }

        //Next Page
        protected void btn_next_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number.Text) < Convert.ToInt32(hdn_last_page.Value))
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text) + 1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
            }
        }

        //Last Page
        protected void btn_last_OnClick(object sender, EventArgs e)
        {

            grid_fill(Convert.ToInt32(hdn_last_page.Value), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
        }

        //Page Data Count
        protected void drp_count_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
        }

        #endregion

        protected void btn_close_OnClick(object sender, EventArgs e)
        {
            pnl_add.Visible = false;
            Upd_Add_Panel.Update();
        }

        protected void btn_newentry_OnClick(object sender, EventArgs e)
        {
            Clear();
            pnl_add.Visible = true;
            Upd_Add_Panel.Update();
        }

        /*Clear All the Data*/
        public void Clear()
        {
            hdn_id.Value = "0";
            drp_customer.ClearSelection();
            drpInvoice.Items.Clear();
            drpInvoice.Text = "";
            drpInvoice.Enabled = drp_customer.Enabled = true;

            job_date.DbSelectedDate = DateTime.Now;
            txtTotalTax.Text = "";
            txt_grand.Text = "";
            txt_remark.Text = "";
           
            rpt_Item_list.DataSource = null;
            rpt_Item_list.DataBind();

            btn_save.Visible = hdn_add.Value == "0" ? false : true;
            btn_save_print.Visible = hdn_add_N_print.Value == "0" ? false : true;
            btn_cancel.Visible =  btn_print.Visible = false;
            Get_Code();

            Upd_Add_PanelInner.Update();
        }

        /*Code for Display*/
        public void Get_Code()
        {
            DataTable dt = obj_common.Get_Code(122);
            if (dt.Rows.Count > 0)
                lbl_Code.Text = dt.Rows[0][0].ToString();
        }

        /*Check Action Privilege*/
        public void previlage_action_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    DataTable dt = obj_common.Action_Previlage_Validation(122, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdncancel.Value = dt.Rows[1][1].ToString();
                        hdn_add_N_print.Value = dt.Rows[2][1].ToString();
                        //hdnupdateNPrint.Value = dt.Rows[3][1].ToString();
                        hdn_print.Value = dt.Rows[3][1].ToString();
                    }
                    btn_save.Visible = hdn_add.Value == "0" ? false : true;
                    btn_save_print.Visible = hdn_add_N_print.Value == "0" ? false : true;
                }
                else
                {
                    Response.Redirect("../Landing.aspx");
                }
            }
            catch
            {
                Response.Redirect("../Landing.aspx");
            }
        }

        /*Check Form Privilege*/
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.Form_Previlage_Validation(122, Convert.ToInt32(hdn_user_id.Value));
                    if (val == 0)
                    {
                        Response.Redirect("../Landing.aspx");
                    }

                }
                else
                {
                    Response.Redirect("../Landing.aspx");
                }
            }
            catch
            {
                Response.Redirect("../Landing.aspx");
            }
        }

        /*Calucate the Date*/
        public string CalDate(Telerik.Web.UI.RadDatePicker Dates)
        {
            string month = Dates.SelectedDate.Value.Month.ToString();
            if (month != "10" && month != "11" && month != "12")
                month = "0" + month;
            string day = Dates.SelectedDate.Value.Day.ToString();
            for (int i = 0; i < 10; i++)
            {
                if (Convert.ToInt32(day) == i)
                    day = "0" + day;
            }
            string year = Dates.SelectedDate.Value.Year.ToString();
            return day + '/' + month + '/' + year;
        }

        #region print

        public void InchPrint(int ReceiptId)
        {
            ReceiptIdpub = ReceiptId;
            PrinterSettings settings = new PrinterSettings();
            string printname = settings.PrinterName;

            DataSet ds = obj_report.CashReceiptPrint(ReceiptIdpub);
            DataTable dt = ds.Tables[0];
            DataTable dt_invD = ds.Tables[1];

            int servicelen = dt_invD.Rows.Count * 15;
            if (Application["PrintHeader"] != "")
            {
                servicelen = servicelen + 65;
            }
            try
            {
                PrintDocument doc = new PrintDocument();
                doc.PrinterSettings.PrinterName = printname;
                doc.DefaultPageSettings.PaperSize = new PaperSize("PaperA4", 300, 300 + servicelen);
                doc.DocumentName = Server.MapPath("~") + "CashReceiptPrint.pdf";
                doc.PrintPage += new PrintPageEventHandler(PrintHandler);
                doc.Print();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('" + ex.Message + "');", true);

            }
        }
        private void PrintHandler(object sender, PrintPageEventArgs ppeArgs)
        {
            DataSet ds = obj_report.CashReceiptPrint(ReceiptIdpub);
            DataTable dt = ds.Tables[0];
            DataTable dt_invD = ds.Tables[1];

            int servicelen = dt_invD.Rows.Count * 15;
            float currentY = 10;
            int initlX = 8;

            if (Application["PrintHeader"] != "")
            {
                servicelen = servicelen + 65;
                string imageURL = Server.MapPath("../UploadedImage/" + Application["PrintHeader"]);
                System.Drawing.Bitmap image1 = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(imageURL, true);
                System.Drawing.Bitmap resized = new System.Drawing.Bitmap(image1, new System.Drawing.Size(280, 60));
                System.Drawing.TextureBrush texture = new System.Drawing.TextureBrush(resized);
                System.Drawing.Graphics formGraphics = ppeArgs.Graphics;
                formGraphics.FillRectangle(texture, new System.Drawing.RectangleF(10, 10, 270, 50));
                currentY = currentY + 60;
            }

            var foo = new PrivateFontCollection();
            foo.AddFontFile(HttpContext.Current.Server.MapPath("~/Font/arabtype.ttf"));

            System.Drawing.Font arbfnt = new System.Drawing.Font((System.Drawing.FontFamily)foo.Families[0], 8f);
            System.Drawing.Font arbsmallbold = new System.Drawing.Font((System.Drawing.FontFamily)foo.Families[0], 6f);
            System.Drawing.Font arbfntbld = new System.Drawing.Font((System.Drawing.FontFamily)foo.Families[0], 10f);

            System.Drawing.Font Fontboldhead = new System.Drawing.Font("Times New Roman", 9, System.Drawing.FontStyle.Bold);
            System.Drawing.Font FontNormal = new System.Drawing.Font("Times New Roman", 8, System.Drawing.FontStyle.Regular);
            System.Drawing.Font FontNormalBold = new System.Drawing.Font("Times New Roman", 8, System.Drawing.FontStyle.Bold);

            System.Drawing.Graphics g = ppeArgs.Graphics;

            g.DrawString("CASH RECEIPT", Fontboldhead, System.Drawing.Brushes.Black, 100, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 15;
            g.DrawString("الايصال", arbfntbld, System.Drawing.Brushes.Black, 125, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 25;

            System.IO.Stream mem = new MemoryStream();
            Barcode128 barImg = new Barcode128();
            barImg.Code = dt.Rows[0]["InvoiceCode"].ToString();
            barImg.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White).Save(mem, System.Drawing.Imaging.ImageFormat.Png);
            System.Drawing.Bitmap image1s = (System.Drawing.Bitmap)System.Drawing.Image.FromStream(mem, true);
            mem.Flush();
            mem.Close();
            System.Drawing.Bitmap resizeds = new System.Drawing.Bitmap(image1s, new System.Drawing.Size(100, 35));
            System.Drawing.TextureBrush textures = new System.Drawing.TextureBrush(resizeds);
            System.Drawing.Graphics formGraphicss = ppeArgs.Graphics;
            System.Drawing.PointF Loc = new System.Drawing.PointF(100, currentY);
            System.Drawing.SizeF SizeFc = new System.Drawing.SizeF(100, 35);
            formGraphicss.FillRectangle(textures, new System.Drawing.RectangleF(Loc, SizeFc));
            currentY = currentY + 45;

            g.DrawString(dt.Rows[0]["CustomerName"].ToString(), FontNormalBold, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
            g.DrawString(dt.Rows[0]["Date"].ToString(), FontNormalBold, System.Drawing.Brushes.Black, 240, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 15;
            g.DrawString("Invoice No / ", FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
            g.DrawString("رقم الفاتورة", arbfnt, System.Drawing.Brushes.Black, 70, currentY, new System.Drawing.StringFormat());
            g.DrawString(" : " + dt.Rows[0]["InvoiceCode"].ToString(), FontNormal, System.Drawing.Brushes.Black, 125, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 15;
            g.DrawString("Receipt No / ", FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
            g.DrawString("رقم الايصال", arbfnt, System.Drawing.Brushes.Black, 70, currentY, new System.Drawing.StringFormat());
            g.DrawString(" : " + dt.Rows[0]["Code"].ToString(), FontNormal, System.Drawing.Brushes.Black, 125, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 20;
            g.DrawString("Service / ", FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
            g.DrawString("الخدمات", arbfnt, System.Drawing.Brushes.Black, 55, currentY, new System.Drawing.StringFormat());
            //currentY = currentY + 15;
            //System.Drawing.Pen p1 = new System.Drawing.Pen(System.Drawing.Color.Black, 0.5f);
            //System.Drawing.Point point1 = new System.Drawing.Point(10, Convert.ToInt32(currentY));
            //System.Drawing.Point point2 = new System.Drawing.Point(290, Convert.ToInt32(currentY));
            //g.DrawLine(p1, point1, point2);

            foreach (DataRow r in dt_invD.Rows)
            {
                currentY = currentY + 15;

                g.DrawString(r["Name"].ToString(), FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
                if (r["NameInArabic"].ToString() != "")
                {
                    currentY = currentY + 10;
                    g.DrawString(r["NameInArabic"].ToString(), arbfnt, System.Drawing.Brushes.Black, 100, currentY, new System.Drawing.StringFormat());
                }
            }
            //currentY = currentY + 15;
            //point1 = new System.Drawing.Point(10, Convert.ToInt32(currentY));
            //point2 = new System.Drawing.Point(290, Convert.ToInt32(currentY));
            //g.DrawLine(p1, point1, point2);
            currentY = currentY + 15;
            g.DrawString("Net Amount / ", FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
            g.DrawString("المبلغ الصافي", arbfnt, System.Drawing.Brushes.Black, 75, currentY, new System.Drawing.StringFormat());
            g.DrawString(" : " + dt.Rows[0]["PendingAmount"].ToString(), FontNormal, System.Drawing.Brushes.Black, 135, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 15;
            g.DrawString("Paid Amount / ", FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
            g.DrawString("المبلغ المدفوع", arbfnt, System.Drawing.Brushes.Black, 75, currentY, new System.Drawing.StringFormat());
            g.DrawString(" : " + dt.Rows[0]["Amount"].ToString(), FontNormal, System.Drawing.Brushes.Black, 135, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 15;
            g.DrawString("Balance / ", FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
            g.DrawString("الرصيد", arbfnt, System.Drawing.Brushes.Black, 55, currentY, new System.Drawing.StringFormat());
            g.DrawString(" : " + dt.Rows[0]["Receivable"].ToString(), FontNormal, System.Drawing.Brushes.Black, 135, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 25;
            g.DrawString("* * *", FontNormal, System.Drawing.Brushes.Black, 140, currentY, new System.Drawing.StringFormat());

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
            if ((number / 100000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000) + " Lakhs ";
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
                    words += " Fills";
                }
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                    {
                        words += " " + unitsMap[number % 10];
                        words += " Fills";
                    }
                    else
                    {
                        words += " Fills";
                    }
                }
            }
            return words;
        }

        #endregion
    }
}