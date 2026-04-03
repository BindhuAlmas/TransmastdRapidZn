using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using AmarCentre.BAL;
using System.Web.Services;
using Telerik.Web.UI;

namespace AmarCentre.Transactions
{
    public partial class Invoice : System.Web.UI.Page
    {
        Transaction_Bal obj_trans = new Transaction_Bal();
        Master_Bal obj_mas = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
        Report_Bal obj_report = new Report_Bal();
        Voucher BalVoucher = new Voucher();
        public int ReceiptIdpub = 0;

        public static DataTable dtCustomername = new DataTable();
        public static DataTable dtCustomernameAgent = new DataTable();

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
                OnPageLoad();
                grid_fill(1, 10, "", "", "");
            }
        }


        public void OnPageLoad()
        {
            DataTable dt = obj_mas.Edit_GeneralSettings();
            hdnIsTaxprintall.Value = dt.Rows[0]["IsTaxPrintForAll"].ToString();
        }

        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = new DataTable();
            if (page_number == 1 && filter == "")
            {
                dt = obj_trans.ListInvoiceCustm(page_size, Convert.ToInt32(hdn_user_id.Value), drpinvStatus.SelectedValue==""?1:
                    Convert.ToInt32(drpinvStatus.SelectedValue) );
            }
            else
            {
                dt = obj_trans.List_Invoice(page_number, page_size, filter, Convert.ToInt32(hdn_user_id.Value), drpinvStatus.SelectedValue == "" ? 1 :
                    Convert.ToInt32(drpinvStatus.SelectedValue));
            }
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
            DataTable dt = obj_trans.Get_List_invoice_Excel(Convert.ToInt32(hdn_user_id.Value));
            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "Invoice");
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                HttpContext.Current.Response.Write(style);
                Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        /*rpt_list OnItemCommand*/
        protected void rpt_list_OnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdn_id");
            HiddenField hdnInvPrint = (HiddenField)e.Item.FindControl("hdnInvPrint");

            if (e.CommandName == "Edit")
            {
                pnl_add.Visible = true;
                UCInvoice.UCPageLoad(1, Convert.ToInt32(hdn_rpt_id.Value), txt_search.Text,Convert.ToInt32(drp_count.SelectedValue));
                Upd_Add_Panel.Update();
            }
            else if (e.CommandName == "TaxInvoicePrint")
            {
                int Format = Convert.ToInt32(hdnInvPrint.Value);
                string url = "";
                if(Format==1)
                    url = "../Reports/TaxInvoiceFormat1.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if(Format==2)
                    url = "../Reports/TaxInvoiceFormat2.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 3)
                    url = "../Reports/TaxInvoiceFormat3.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);

                else if (Format == 5)
                    url = "../Reports/TaxInvoiceFormat5.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 6)
                    url = "../Reports/TaxInvoiceFormat6.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 7)
                    url = "../Reports/TaxInvoiceFormat7.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format ==8)
                    url = "../Reports/TaxInvoiceFormat8.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 9)
                    url = "../Reports/TaxInvoiceFormat9.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 10)
                    url = "../Reports/TaxInvoiceFormat10.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 11)
                    url = "../Reports/TaxInvoiceFormat11.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 4)
                    url = "../Reports/TaxInvoiceFormat4.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);

                else if (Format == 12)
                    url = "../Reports/TaxInvoiceFormat12.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 13)
                    url = "../Reports/TaxInvoiceFormat13.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 14)
                    url = "../Reports/TaxInvoiceFormat14.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 15) // F2
                    url = "../Reports/TaxInvoiceFormat15.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 16)  
                    url = "../Reports/TaxInvoiceFormat16.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 17)
                    url = "../Reports/TaxInvoiceFormat17.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 18)
                    url = "../Reports/TaxInvoiceFormat18.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 19)
                    url = "../Reports/TaxInvoiceFormat19.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 20)
                    url = "../Reports/TaxInvoiceFormat20.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 21)
                    url = "../Reports/TaxInvoiceFormat21.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 22)
                    url = "../Reports/TaxInvoiceFormat22.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 23)
                    url = "../Reports/TaxInvoiceFormat23.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
            }
            else if (e.CommandName == "SalesOrderPrint")
            {
                DataTable dt= obj_mas.Edit_GeneralSettings();
                int Format = Convert.ToInt32(hdnInvPrint.Value);
                string url = "";
                if (dt.Rows[0]["SalesOrderPrint"].ToString() == "1")
                {
                    if (Format == 1)
                        url = "../Reports/SalesOrderFormat1.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                    else if (Format == 2 || Format == 7)
                        url = "../Reports/SalesOrderFormat2.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                    else if (Format == 3)
                        url = "../Reports/SalesOrderFormat3.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                    else if (Format == 9)
                        url = "../Reports/SalesOrderFormat9.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                    else if (Format == 20)
                        url = "../Reports/SalesOrderFormat20.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                    else if (Format == 23)
                        url = "../Reports/SalesOrderFormat23.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                    else
                        url = "../Reports/SalesOrderPrint.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                }
                else
                {
                    url = "../Reports/SalesorderPOS.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                }
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
            }
            else if (e.CommandName == "Sendmail")
            {
                EmailUC.UCPageLoad(2, Convert.ToInt32(hdn_rpt_id.Value));
                pnlMail.Visible = true;
                UpdMailPanel.Update();
            }
        }

        protected void rpt_list_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Button btnTaxInvoicePrint = (Button)e.Item.FindControl("btnTaxInvoicePrint");
            Button btnSalesOrderPrint = (Button)e.Item.FindControl("btnSalesOrderPrint");
            HiddenField hdnIsCredit = (HiddenField)e.Item.FindControl("hdnIsCredit");
            HiddenField hdnReceived = (HiddenField)e.Item.FindControl("hdnReceived");
            HiddenField hdnAfterDiscountGrandTotal = (HiddenField)e.Item.FindControl("hdnAfterDiscountGrandTotal");
            btnSalesOrderPrint.Visible = hdn_print.Value == "0" ? false : true;
            Button btnSendmail = (Button)e.Item.FindControl("btnSendmail");
            btnSendmail.Visible = hdnsendmail.Value == "0" ? false : true;

            if (hdnIsCredit.Value == "1" || hdnIsTaxprintall.Value == "1")
                btnTaxInvoicePrint.Visible = hdn_TaxInvoicePrint.Value == "0" ? false : true;
            else
            {
                if ( ((hdnReceived.Value == "" ? "0" : hdnReceived.Value) ==
                    (hdnAfterDiscountGrandTotal.Value == "" ? "0" : hdnAfterDiscountGrandTotal.Value)) )
                    btnTaxInvoicePrint.Visible = hdn_TaxInvoicePrint.Value == "0" ? false : true;
                else
                    btnTaxInvoicePrint.Visible = btnSendmail.Visible = false;
            }

            var lblPaymentStatus = e.Item.FindControl("lblPaymentStatus") as Label;
            HiddenField hdnPaymentStatus = (HiddenField)e.Item.FindControl("hdnPaymentStatus");
            if (hdnPaymentStatus.Value == "UnPaid")
            {
                lblPaymentStatus.Style.Add("color", "red");
            }
            else if(hdnPaymentStatus.Value == "Partially Paid")
            {
                lblPaymentStatus.Style.Add("color", "lightgreen");
            }
            else if (hdnPaymentStatus.Value == "Fully Paid")
            {
                lblPaymentStatus.Style.Add("color", "darkgreen");
            }

        }   



        public void fillServices(int res)
        {
            UCInvoice.fillServices(res);
        }

        protected void btn_newentry_OnClick(object sender, EventArgs e)
        {
            pnl_add.Visible = true;
            UCInvoice.UCPageLoad(1, 0, txt_search.Text, Convert.ToInt32(drp_count.SelectedValue));
            Upd_Add_Panel.Update();
        }

        protected void callSAveCompletion(object sender, EventArgs e)
        {
            UCInvoice.callSAveCompletion(null, null);
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

        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    DataSet ds = obj_common.FormAction_Previlage_Validation(16, Convert.ToInt32(hdn_user_id.Value));
                    DataTable dtm = ds.Tables[0];
                    DataTable dt = ds.Tables[1];

                    if (dtm.Rows[0][0].ToString() == "0")
                    {
                        Response.Redirect("../Landing.aspx");
                    }

                    if (dt.Rows.Count > 0)
                    {
                        hdn_print.Value = dt.Rows[2][1].ToString();
                        hdn_TaxInvoicePrint.Value = dt.Rows[7][1].ToString();
                        hdnsendmail.Value = dt.Rows[10][1].ToString();
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
    }
}