using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using AmarCentre.BAL;
using System.Web.UI.HtmlControls;
using System.Globalization;
using Telerik.Web.UI;

namespace AmarCentre.Customer
{
    public partial class InvoiceList : System.Web.UI.Page
    {
        Transaction_Bal obj_trans = new Transaction_Bal();
        Master_Bal obj_mas = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
            }
            if (!IsPostBack)
            {
                hdn_user_id.Value = Session["User_Id"].ToString();
                lbl_User_name.Text = Session["User_Name"].ToString();
                hdn_user_id.Value = Session["User_Id"].ToString();
                previlage_check();
                grid_fill(1, 10, "", "", "");
            }
        }

        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_trans.ListCustomerInvoiceList(page_number, page_size, filter, Convert.ToInt32(hdn_user_id.Value));
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

        protected void rpt_list_OnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdn_id");
            HiddenField hdnInvPrint = (HiddenField)e.Item.FindControl("hdnInvPrint");

            if (e.CommandName == "Edit")
            {
                pnl_add.Visible = true;

                DataSet ds = obj_trans.EditCustomerInvoiceList(Convert.ToInt32(hdn_rpt_id.Value));
                DataTable dt1 = ds.Tables[0];
                DataTable dt_ser = ds.Tables[1];

                hdn_id.Value = dt1.Rows[0]["Id"].ToString();
                lbl_Code.Text = dt1.Rows[0]["Code"].ToString();
                txtamount.Text = dt1.Rows[0]["AfterDiscount_GrandTotal"].ToString();
                job_date.DbSelectedDate = dt1.Rows[0]["Dated"].ToString();
                rpt_Item_list.DataSource = dt_ser;
                rpt_Item_list.DataBind();

                Upd_Add_Panel.Update();
            }
            else if (e.CommandName == "TaxInvoicePrint")
            {
                int Format = Convert.ToInt32(hdnInvPrint.Value);
                string url = "";
                if (Format == 1)
                    url = "../Reports/TaxInvoiceFormat1.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 2)
                    url = "../Reports/TaxInvoiceFormat2.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 3)
                    url = "../Reports/TaxInvoiceFormat3.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);

                else if (Format == 5)
                    url = "../Reports/TaxInvoiceFormat5.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 6)
                    url = "../Reports/TaxInvoiceFormat6.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 7)
                    url = "../Reports/TaxInvoiceFormat7.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 8)
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
                DataTable dt = obj_mas.Edit_GeneralSettings();
                int Format = Convert.ToInt32(dt.Rows[0]["InvoiceFormat"]);
                string url = "";
                if (dt.Rows[0]["SalesOrderPrint"].ToString() == "1")
                {
                    if (Format == 1)
                        url = "../Reports/SalesOrderFormat1.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                    else if (Format == 2 || Format == 7)
                        url = "../Reports/SalesOrderFormat2.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                    else if (Format == 3)
                        url = "../Reports/SalesOrderFormat3.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                    else if (Format == 10)
                        url = "../Reports/SalesOrderFormat10.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                    else
                        url = "../Reports/SalesOrderPrint.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                }
                else
                {
                    url = "../Reports/SalesorderPOS.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                }
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
            }
        }

        protected void rpt_list_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataTable dt = obj_mas.Edit_GeneralSettings();
            string ii = dt.Rows[0]["IsTaxPrintForAll"].ToString();

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Button btnTaxInvoicePrint = (Button)e.Item.FindControl("btnTaxInvoicePrint");
                HiddenField hdnIsCredit = (HiddenField)e.Item.FindControl("hdnIsCredit");
                HiddenField hdnReceived = (HiddenField)e.Item.FindControl("hdnReceived");
                HiddenField hdnAfterDiscountGrandTotal = (HiddenField)e.Item.FindControl("hdnAfterDiscountGrandTotal");
                if (hdnIsCredit.Value == "1" || ii=="1")
                    btnTaxInvoicePrint.Visible =  true;
                else
                {
                    if ((hdnReceived.Value == "" ? "0" : hdnReceived.Value) ==
                        (hdnAfterDiscountGrandTotal.Value == "" ? "0" : hdnAfterDiscountGrandTotal.Value))
                        btnTaxInvoicePrint.Visible = true;
                    else
                        btnTaxInvoicePrint.Visible = false;

                }
            }
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

        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.CustomerForm_Previlage_Validation(3, Convert.ToInt32(hdn_user_id.Value));
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
    }
}