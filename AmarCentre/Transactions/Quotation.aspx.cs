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

namespace AmarCentre.Transactions
{
    public partial class Quotation : System.Web.UI.Page
    {
        Transaction_Bal obj_trans = new Transaction_Bal();
        Master_Bal obj_mas = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
        Report_Bal obj_report = new Report_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
            }
            if (!IsPostBack)
            {
                hdn_user_id.Value = Session["User_Id"].ToString();
                hdnLanguage.Value = GetLanguage(Convert.ToInt32(hdn_user_id.Value));
                previlage_check();
                previlage_action_check();
                fill_Customer();
                fill_Templates();
                OnpageLoad();
                Clear();
                grid_fill(1, 10, "", "", "");

                if (Request.QueryString["LeadId"] != null)
                {
                    DataSet ds = obj_trans.GetQuotationbyLead(Convert.ToInt32(Request.QueryString["LeadId"].ToString()));
                    DataTable dtcus = ds.Tables[0];
                    //DataTable dt = ds.Tables[1];
                    //DataTable dtsum = ds.Tables[2];

                    fill_Customer();
                    drp_customer.SelectedValue = dtcus.Rows[0]["Id"].ToString();
                    hdnLeadId.Value = Request.QueryString["LeadId"].ToString();

                    pnl_add.Visible = true;
                    Upd_Add_Panel.Update();
                }
            }
        }

        public string GetLanguage(int UserId)
        {
            DataTable dt = obj_trans.GetEmployeeLanguage(UserId);
            return dt.Rows[0][0].ToString();
        }

        public void OnpageLoad()
        {
            DataTable dt = obj_mas.Edit_GeneralSettings();
            hdnDefaultInvoiceType.Value = dt.Rows[0]["InvoiceType"].ToString();
            hdnSerPriceWTax.Value = dt.Rows[0]["ServicePriceWithTax"].ToString();
            hdnIsDisableRoundOff.Value = dt.Rows[0]["IsDisableRoundOff"].ToString();
            hdnTaxAppliedWithDiscount.Value = dt.Rows[0]["TaxAppliedWithDiscount"].ToString();
        }

        //Get List 
        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_trans.List_Quotation(page_number, page_size, filter, Convert.ToInt32(hdn_user_id.Value));
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
            DataTable dt = obj_trans.Get_List_Quotation_Excel(Convert.ToInt32(hdn_user_id.Value));
            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "Quotation");
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
            if (e.CommandName == "Edit")
            {
                Clear();
                pnl_add.Visible = true;

                DataSet ds = obj_trans.Edit_Quotation(Convert.ToInt32(hdn_rpt_id.Value), Convert.ToInt32(hdnLanguage.Value), Convert.ToInt32(hdnSerPriceWTax.Value));
                DataTable dt1 = ds.Tables[0];/*invoic*/
                DataTable dt_ser = ds.Tables[1];/* Detail*/
                DataTable dtQuotationHistory = ds.Tables[2];
                hdn_id.Value = dt1.Rows[0]["Id"].ToString();
                lbl_Code.Text = dt1.Rows[0]["Code"].ToString();
                job_date.DbSelectedDate = dt1.Rows[0]["Dated"].ToString();
                drp_customer.SelectedValue = dt1.Rows[0]["Customer_Id"].ToString();
                txtSubject.Text = dt1.Rows[0]["Subject"].ToString();

                rbTaxInvoice.Checked = true;
                rbNormalInvoice.Checked = false;
                if (dt1.Rows[0]["QuotationType"].ToString() == "1")
                {
                    rbTaxInvoice.Checked = true;
                    rbNormalInvoice.Checked = false;
                }
                else if (dt1.Rows[0]["QuotationType"].ToString() == "2")
                {
                    rbTaxInvoice.Checked = false;
                    rbNormalInvoice.Checked = true;
                }
                txt_grand.Text = dt1.Rows[0]["Grand_Total"].ToString();
                txt_remark.Text = dt1.Rows[0]["Remarks"].ToString();

                rpt_Item_list.DataSource = dt_ser;
                rpt_Item_list.DataBind();

                lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
                hdn_QuoDetailId.Value = "-" + (rpt_Item_list.Items.Count + 1).ToString();

                if (dtQuotationHistory.Rows.Count > 0)
                {
                    divQuotationHistory.Visible = true;
                    rptQuotationHistory.DataSource = dtQuotationHistory;
                    rptQuotationHistory.DataBind();
                }

                btn_save.Visible = hdn_update.Value == "0" ? false : true;
                btn_save_print.Visible = hdn_update_N_print.Value == "0" ? false : true;
                btn_print.Visible = hdn_print.Value == "0" ? false : true;
                btnNewVersion.Visible = hdn_newVersion.Value == "0" ? false : true;
                btnOpenCancel.Visible = hdn_cancel.Value == "0" ? false : true;

                if (dt1.Rows[0]["IsAllowEdit"].ToString() == "0" || dt1.Rows[0]["Statusid"].ToString() == "3")
                {
                    btn_save.Visible = btn_save_print.Visible = btnOpenCancel.Visible= false;
                }
                if (dt1.Rows[0]["Statusid"].ToString() == "2") // invoice created
                {
                    btnOpenCancel.Visible = false;
                }

                Upd_Add_Panel.Update();
            }
            else if (e.CommandName == "Print")
            {
                DataTable dt = obj_mas.Edit_GeneralSettings();

                int Format = dt.Rows[0]["QuotationFormat"].ToString() == "" ? 1 : Convert.ToInt32(dt.Rows[0]["QuotationFormat"].ToString());
                string url = "";
                  if (Format == 2)
                    url = "../Reports/QuotationPrintFormat2.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 3)
                    url = "../Reports/QuotationPrintFormat3.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 4)
                    url = "../Reports/QuotationPrintFormat4.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 5)
                    url = "../Reports/QuotationPrintFormat5.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 6)
                    url = "../Reports/QuotationPrintFormat6.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 7)
                    url = "../Reports/QuotationPrintFormat7.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 8)
                    url = "../Reports/QuotationPrintFormat8.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else
                    url = "../Reports/QuotationPrintFormat1.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
            }
            else if (e.CommandName == "Sendmail")
            {
                EmailUC.UCPageLoad(1, Convert.ToInt32(hdn_rpt_id.Value));
                pnlMail.Visible = true;
                UpdMailPanel.Update();
            }
        }

        protected void rpt_list_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Button btnPrint = (Button)e.Item.FindControl("btnPrint");
            btnPrint.Visible = hdn_print.Value == "0" ? false : true;
            Button btnSendmail = (Button)e.Item.FindControl("btnSendmail");
            btnSendmail.Visible = hdnsendmail.Value == "0" ? false : true;
        }

        protected void drp_customer_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (drp_customer.SelectedValue != "")
            {
                if (drp_customer.SelectedValue == "0")
                {
                     int val = obj_common.Form_Previlage_Validation(8, Convert.ToInt32(hdn_user_id.Value));
                     if (val == 0)
                     {
                         ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Sorry you do not have privilege to create new customer..!');", true);
                         drp_customer.ClearSelection();
                         Upd_CustomerDrop_Panel.Update();
                     }
                     else
                     {
                         pnl_Customer.Visible = true;
                         UC_Customer.PageLoad(0);
                         Upd_Customer_Panel.Update();
                     }
                }
            }
        }

        public int SaveQuotation(int Version)
        {
            int res = 0;
            DataTable dt_deatils = fill_Detail();
            if (dt_deatils.Rows.Count > 0)
            {
                if (drpService.SelectedValue == "" && (txt_displayPrice.Text != "" || txt_Qty.Text != ""))
                {
                    InlineCalculation();
                }
                res = obj_trans.Insert_Update_Quotation(Convert.ToInt32(hdn_id.Value), job_date.SelectedDate,
                Convert.ToInt32(drp_customer.SelectedValue), txt_remark.Text, Convert.ToInt32(hdn_user_id.Value), Convert.ToDecimal(txt_grand.Text),
                dt_deatils, Version, rbTaxInvoice.Checked == true ? 1 : 2, txtSubject.Text);
            }
            else
            {
                lbl_msgin.Text = "Add Service to Continue !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            return res;
        }

        /*Data To Save*/
        public DataTable fill_Detail()
        {
            DataTable dt_ser = new DataTable();
            dt_ser.Columns.Add("D_id", typeof(int));
            dt_ser.Columns.Add("CategoryId", typeof(int));
            dt_ser.Columns.Add("Service_Id", typeof(int));
            dt_ser.Columns.Add("Particulars", typeof(string));
            dt_ser.Columns.Add("Price", typeof(decimal));
            dt_ser.Columns.Add("Expense", typeof(decimal));
            dt_ser.Columns.Add("ServiceCharge", typeof(decimal));
            dt_ser.Columns.Add("AdditionalServiceCharge", typeof(decimal));
            dt_ser.Columns.Add("Quantity", typeof(decimal));
            dt_ser.Columns.Add("TaxAmount", typeof(decimal));
            dt_ser.Columns.Add("PriceWitTax", typeof(decimal));
            dt_ser.Columns.Add("Fine", typeof(decimal));
            dt_ser.Columns.Add("Total", typeof(decimal));
            dt_ser.Columns.Add("TemplateId", typeof(int));
            dt_ser.Columns.Add("Discount", typeof(decimal));

            if (rpt_Item_list.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_Item_list.Items)
                {
                    HiddenField hdnInvDId = (HiddenField)itm.FindControl("hdnInvDId");
                    HiddenField hdnInvDCategoryId = (HiddenField)itm.FindControl("hdnInvDCategoryId");
                    HiddenField hdnInvDServiceId = (HiddenField)itm.FindControl("hdnInvDServiceId");
                    TextBox lblInvDdesc = (TextBox)itm.FindControl("lblInvDdesc");
                    TextBox txtInvDDisplayPrice = (TextBox)itm.FindControl("txtInvDDisplayPrice");
                    HiddenField hdnInvDExpense = (HiddenField)itm.FindControl("hdnInvDExpense");
                    HiddenField hdnInvDServiceCharge = (HiddenField)itm.FindControl("hdnInvDServiceCharge");
                    HiddenField hdnInvDPrice = (HiddenField)itm.FindControl("hdnInvDPrice");
                    TextBox txtInvDAddServiceCharge = (TextBox)itm.FindControl("txtInvDAddServiceCharge");
                    TextBox txtInvDQty = (TextBox)itm.FindControl("txtInvDQty");
                    TextBox txtInvDTaxAmount = (TextBox)itm.FindControl("txtInvDTaxAmount");
                    TextBox txtInvDPriceWitTax = (TextBox)itm.FindControl("txtInvDPriceWitTax");
                    TextBox txtInvDFine = (TextBox)itm.FindControl("txtInvDFine");
                    TextBox txtInvDTotal = (TextBox)itm.FindControl("txtInvDTotal");
                    HiddenField hdnTemplateId = (HiddenField)itm.FindControl("hdnTemplateId");
                    TextBox txtInvDdiscount = (TextBox)itm.FindControl("txtInvDdiscount");
                    TextBox txtInvDExpense = (TextBox)itm.FindControl("txtInvDExpense");

                    //dt_ser.Rows.Add(Convert.ToInt32(hdnInvDId.Value), hdnInvDCategoryId.Value==""?(int?)null: Convert.ToInt32(hdnInvDCategoryId.Value),
                    //Convert.ToInt32(hdnInvDServiceId.Value), lblInvDdesc.Text, Convert.ToDecimal(hdnInvDPrice.Value), Convert.ToDecimal(hdnInvDExpense.Value),
                    //Convert.ToDecimal(hdnInvDServiceCharge.Value), txtInvDAddServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDAddServiceCharge.Text),
                    //Convert.ToDecimal(txtInvDQty.Text), Convert.ToDecimal(txtInvDTaxAmount.Text),Convert.ToDecimal(txtInvDPriceWitTax.Text), 
                    //txtInvDFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDFine.Text), Convert.ToDecimal(txtInvDTotal.Text),
                    //hdnTemplateId.Value==""?(int?)null:Convert.ToInt32(hdnTemplateId.Value),
                    //txtInvDdiscount.Text==""?0:Convert.ToDecimal(txtInvDdiscount.Text) );

                    dt_ser.Rows.Add(Convert.ToInt32(hdnInvDId.Value), hdnInvDCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDCategoryId.Value),
                    Convert.ToInt32(hdnInvDServiceId.Value), lblInvDdesc.Text, Convert.ToDecimal(hdnInvDPrice.Value), Convert.ToDecimal(txtInvDExpense.Text),
                    Convert.ToDecimal(hdnInvDServiceCharge.Value), txtInvDAddServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDAddServiceCharge.Text),
                    Convert.ToDecimal(txtInvDQty.Text), Convert.ToDecimal(txtInvDTaxAmount.Text), Convert.ToDecimal(txtInvDPriceWitTax.Text),
                    txtInvDFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDFine.Text), Convert.ToDecimal(txtInvDTotal.Text),
                    hdnTemplateId.Value == "" ? (int?)null : Convert.ToInt32(hdnTemplateId.Value),
                    txtInvDdiscount.Text == "" ? 0 : Convert.ToDecimal(txtInvDdiscount.Text));
                }
            }
            if (drpService.SelectedValue != "" && txt_displayPrice.Text != "" && txt_Qty.Text != "")
            {
                //dt_ser.Rows.Add(Convert.ToInt32(hdn_QuoDetailId.Value), hdnSerCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnSerCategoryId.Value),
                //    Convert.ToInt32(drpService.SelectedValue), txt_desc.Text, Convert.ToDecimal(hdnPrice.Value), Convert.ToDecimal(hdn_expn.Value),
                //       Convert.ToDecimal(hdn_sc.Value), txtServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtServiceCharge.Text),
                //       Convert.ToDecimal(txt_Qty.Text), Convert.ToDecimal(txt_taxamt.Text),Convert.ToDecimal(txt_PriceWitTax.Text),
                //       txtFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtFine.Text), Convert.ToDecimal(txt_totPrice.Text), (int?)null,
                //       txt_discount.Text == "" ? 0 : Convert.ToDecimal(txt_discount.Text));
                dt_ser.Rows.Add(Convert.ToInt32(hdn_QuoDetailId.Value), hdnSerCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnSerCategoryId.Value),
                   Convert.ToInt32(drpService.SelectedValue), txt_desc.Text, Convert.ToDecimal(hdnPrice.Value), Convert.ToDecimal(txtexpense.Text),
                      Convert.ToDecimal(hdn_sc.Value), txtServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtServiceCharge.Text),
                      Convert.ToDecimal(txt_Qty.Text), Convert.ToDecimal(txt_taxamt.Text), Convert.ToDecimal(txt_PriceWitTax.Text),
                      txtFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtFine.Text), Convert.ToDecimal(txt_totPrice.Text), (int?)null,
                      txt_discount.Text == "" ? 0 : Convert.ToDecimal(txt_discount.Text));
            }
            return dt_ser;
        }

        /*Save*/
        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            int res = SaveQuotation(0);

            if (res > 0)
            {
                if (hdnLeadId.Value != "0")
                {
                    int resin = obj_trans.updateleadstatus(Convert.ToInt32(hdnLeadId.Value), res, (int?)null, 2, Convert.ToInt32(hdn_user_id.Value));
                }

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

            Upd_Add_PanelInner.Update();
        }

        /*Save & Print*/
        protected void btn_save_print_OnClick(object sender, EventArgs e)
        {
            int res = SaveQuotation(0);
            if (res > 0)
            {
                if (hdnLeadId.Value != "0")
                {
                    int resin = obj_trans.updateleadstatus(Convert.ToInt32(hdnLeadId.Value), res, (int?)null, 2, Convert.ToInt32(hdn_user_id.Value));
                }

                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                lbl_msgin.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                
                DataTable dt = obj_mas.Edit_GeneralSettings();

                int Format = dt.Rows[0]["QuotationFormat"].ToString() == "" ? 1 : Convert.ToInt32(dt.Rows[0]["QuotationFormat"].ToString());
                string url = "";
                  if (Format == 2)
                    url = "../Reports/QuotationPrintFormat2.aspx?id=" + res;
                else if (Format == 3)
                    url = "../Reports/QuotationPrintFormat3.aspx?id=" + res;
                else if (Format == 4)
                    url = "../Reports/QuotationPrintFormat4.aspx?id=" + res;
                else if (Format == 5)
                    url = "../Reports/QuotationPrintFormat5.aspx?id=" + res;
                else if (Format == 6)
                    url = "../Reports/QuotationPrintFormat6.aspx?id=" + res;
                else if (Format == 7)
                    url = "../Reports/QuotationPrintFormat7.aspx?id=" + res;
                else if (Format == 8)
                    url = "../Reports/QuotationPrintFormat8.aspx?id=" + res;
               
                else
                    url = "../Reports/QuotationPrintFormat1.aspx?id=" + res;

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
            }
            else
            {
                lblerrormsg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
            }

            Upd_Add_PanelInner.Update();
        }

        /*New Version*/
        protected void btnNewVersionOnClick(object sender, EventArgs e)
        {
            int res = SaveQuotation(1);

            if (res > 0)
            {
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

        /*Print*/
        protected void btn_print_OnClick(object sender, EventArgs e)
        {
            DataTable dt = obj_mas.Edit_GeneralSettings();

            int Format = dt.Rows[0]["QuotationFormat"].ToString() == "" ? 1 : Convert.ToInt32(dt.Rows[0]["QuotationFormat"].ToString());
            string url = "";
             if (Format == 2)
                url = "../Reports/QuotationPrintFormat2.aspx?id=" + Convert.ToInt32(hdn_id.Value);
            else if (Format == 4)
                url = "../Reports/QuotationPrintFormat4.aspx?id=" + Convert.ToInt32(hdn_id.Value);
            else if (Format == 3)
                url = "../Reports/QuotationPrintFormat3.aspx?id=" + Convert.ToInt32(hdn_id.Value);
            else if (Format == 5)
                url = "../Reports/QuotationPrintFormat5.aspx?id=" + Convert.ToInt32(hdn_id.Value);
            else if (Format == 6)
                url = "../Reports/QuotationPrintFormat6.aspx?id=" + Convert.ToInt32(hdn_id.Value);
            else if (Format == 7)
                url = "../Reports/QuotationPrintFormat7.aspx?id=" + Convert.ToInt32(hdn_id.Value);
            else if (Format == 8)
                url = "../Reports/QuotationPrintFormat8.aspx?id=" + Convert.ToInt32(hdn_id.Value);
            
            else
                url = "../Reports/QuotationPrintFormat1.aspx?id=" + Convert.ToInt32(hdn_id.Value);

            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        /*Customer */
        public void fill_Customer()
        {
            drp_customer.Items.Clear();
            DataTable dt = obj_trans.Drp_Customer();
            drp_customer.DataSource = dt;
            drp_customer.DataTextField = "Text";
            drp_customer.DataValueField = "Value";
            drp_customer.DataBind();

            RadComboBoxItem CodeItem = new RadComboBoxItem();
            CodeItem.Text = "New Entry";
            CodeItem.Value = "0";
            drp_customer.Items.Insert(0, CodeItem);
        }

        public void fill_Templates()
        {
            drpTemplates.Items.Clear();
            DataTable dt = obj_trans.GetTemplates();
            drpTemplates.DataSource = dt;
            drpTemplates.DataTextField = "Text";
            drpTemplates.DataValueField = "Value";
            drpTemplates.DataBind();
        }

        protected void drpTemplatesOnSelectedIndexChanged(object sender, EventArgs e)
        {
            int InvoiceType = rbTaxInvoice.Checked == true ? 1 : 2;
            DataTable dtTemplates = new DataTable();
            dtTemplates.Columns.Add("TemplatesId", typeof(int));
            foreach (RadComboBoxItem item in drpTemplates.Items)
            {
                if (item.Checked)
                    dtTemplates.Rows.Add(Convert.ToInt32(item.Value));
            }
            DataTable dt = obj_trans.GetServiceDetailsTemplate(dtTemplates, Convert.ToInt32(hdnLanguage.Value), Convert.ToInt32(hdnSerPriceWTax.Value), InvoiceType,
                drp_customer.SelectedValue == "" ? 0 : Convert.ToInt32(drp_customer.SelectedValue),0);
            rpt_Item_list.DataSource = dt;
            rpt_Item_list.DataBind();
            InlineCalculation();
            lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
            hdn_QuoDetailId.Value = "-" + (rpt_Item_list.Items.Count + 1).ToString();
            Upd_Item_Panel.Update();
        }

        public void fill_FilterDropDown(int filterby)
        {
            int Department = drpDepartment.SelectedValue == "" ? 0 : Convert.ToInt32(drpDepartment.SelectedValue);
            int SerCategory = drpSerCategory.SelectedValue == "" ? 0 : Convert.ToInt32(drpSerCategory.SelectedValue);
            int SerSubCategory = drpSerSubCategory.SelectedValue == "" ? 0 : Convert.ToInt32(drpSerSubCategory.SelectedValue);
            DataSet ds = obj_trans.GetServiceFilter(filterby, Department, SerCategory, SerSubCategory, Convert.ToInt32(hdnLanguage.Value));
            DataTable dtDepartment = ds.Tables[0];
            DataTable dtSerCategory = ds.Tables[1];
            DataTable dtSerSubCategory = ds.Tables[2];
            DataTable dtService = ds.Tables[3];

            if (drpDepartment.SelectedValue == "")
            {
                drpDepartment.ClearSelection();
                drpDepartment.Text = "";
                drpDepartment.Items.Clear();
                drpDepartment.DataSource = dtDepartment;
                drpDepartment.DataTextField = "Text";
                drpDepartment.DataValueField = "Value";
                drpDepartment.DataBind();
                UpdDepartmentDropdown.Update();
            }
            if (drpSerCategory.SelectedValue == "")
            {
                drpSerCategory.ClearSelection();
                drpSerCategory.Text = "";
                drpSerCategory.Items.Clear();
                drpSerCategory.DataSource = dtSerCategory;
                drpSerCategory.DataTextField = "Text";
                drpSerCategory.DataValueField = "Value";
                drpSerCategory.DataBind();
                UpdSerCategoryDropdown.Update();
            }
            if (drpSerSubCategory.SelectedValue == "")
            {
                drpSerSubCategory.ClearSelection();
                drpSerSubCategory.Text = "";
                drpSerSubCategory.Items.Clear();
                drpSerSubCategory.DataSource = dtSerSubCategory;
                drpSerSubCategory.DataTextField = "Text";
                drpSerSubCategory.DataValueField = "Value";
                drpSerSubCategory.DataBind();
                UpdSerSubCategoryDropdown.Update();
            }
            drpService.ClearSelection();
            drpService.Text = "";
            drpService.Items.Clear();
            drpService.DataSource = dtService;
            drpService.DataTextField = "Text";
            drpService.DataValueField = "Value";
            drpService.DataBind();
            int val = obj_common.Form_Previlage_Validation(14, Convert.ToInt32(hdn_user_id.Value));
            if (val == 1)
            {
                RadComboBoxItem CodeItem = new RadComboBoxItem();
                CodeItem.Text = "New Entry";
                CodeItem.Value = "0";
                drpService.Items.Insert(0, CodeItem);
            }
            UpdServiceDropdown.Update();
            //drpService_OnSelectedIndexChanged(null, null);


            hdnDepartment.Value = "";
            hdnDepartmentId.Value = "";
            hdnSerCategory.Value = "";
            hdnSerCategoryId.Value = "";
            hdnSerSubCategory.Value = "";
            hdnSerSubCategoryId.Value = "";
            txt_desc.Text = "";
            txt_displayPrice.Text = "";
            hdnPrice.Value = "";
            hdn_expn.Value = hdn_sc.Value = txtexpense.Text= "0";
            txtServiceCharge.Text = "";
            txt_Qty.Text = "";
            txt_taxamt.Text = "";
            hdn_tax.Value = "0";
            txt_PriceWitTax.Text = "";
            hdnFineApplicable.Value = "0";
            txtFine.Text = "";
            txt_totPrice.Text =txt_discount.Text= "";


            UpdTxtDescription.Update();
            UpdTxtPrice.Update();
            UpdTxtServiceCharge.Update();
            UpdTxtQty.Update();
            UpdTxtTaxAmt.Update();
            UpdTxtPriceWithTax.Update();
            UpdTxtFine.Update();
            UpdTxtTotPrice.Update();
            Updtxt_discount.Update();
            InlineCalculation();

        }

        protected void drpFilter_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            String contrlName = sendercontrol.ID;
            if (contrlName == "drpDepartment")
            {
                drpSerCategory.Text = "";

                drpSerSubCategory.Text = "";
                drpService.Text = "";

                drpSerCategory.ClearSelection();
                drpSerSubCategory.ClearSelection();
                drpService.ClearSelection();
            }
            else
                if (contrlName == "drpSerCategory")
            {

                drpSerSubCategory.Text = "";
                drpService.Text = "";
                drpSerSubCategory.ClearSelection();
                drpService.ClearSelection();
            }
            else
                    if (contrlName == "drpSerSubCategory")
            {


                drpService.Text = "";

                drpService.ClearSelection();
            }
            fill_FilterDropDown(1);
        }

        /*drp_item OnSelectedIndexChanged*/
        protected void drpService_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hdnDepartment.Value = "";
            hdnDepartmentId.Value = "";
            hdnSerCategory.Value = "";
            hdnSerCategoryId.Value = "";
            hdnSerSubCategory.Value = "";
            hdnSerSubCategoryId.Value = "";
            txt_desc.Text = "";
            txt_displayPrice.Text = "";
            hdnPrice.Value = "";
            hdn_expn.Value = hdn_sc.Value = txtexpense.Text= "0";
            txtServiceCharge.Text = "";
            txt_Qty.Text = "";
            txt_taxamt.Text = "";
            hdn_tax.Value = "0";
            txt_PriceWitTax.Text = "";
            hdnFineApplicable.Value = "0";
            txtFine.Text = "";
            txt_totPrice.Text = txt_discount.Text= "";

            if (drpService.SelectedValue == "0")
            {
                UC_Service.UCPageLoad(3, 0);
                pnlServiceAdd.Visible = true;
                UpdServicepnlAdd.Update();
            }

            else if (drpService.SelectedValue != "")
            {
                int InvoiceType = rbTaxInvoice.Checked == true ? 1 : 2;
                DataTable Amount = new DataTable();
                Amount = obj_trans.Get_Services_Amount(Convert.ToInt32(drpService.SelectedValue), 1, Convert.ToInt32(hdnLanguage.Value), Convert.ToInt32(hdnSerPriceWTax.Value), InvoiceType,0);

                if (Amount.Rows.Count > 0)
                {
                    txt_displayPrice.Text = Amount.Rows[0]["DisplayPrice"].ToString();
                    hdnPrice.Value = Amount.Rows[0]["Price"].ToString();
                    txt_Qty.Text = "1";
                    txt_taxamt.Text = Amount.Rows[0]["TaxAmount"].ToString();
                    txt_PriceWitTax.Text = Amount.Rows[0]["PriceWitTax"].ToString();
                    txt_totPrice.Text = Amount.Rows[0]["Total"].ToString();
                    hdnFineApplicable.Value = Amount.Rows[0]["FineApplicable"].ToString();

                    hdn_expn.Value = txtexpense.Text= Amount.Rows[0]["Expense"].ToString();
                    hdn_tax.Value = Amount.Rows[0]["Tax"].ToString();
                    hdn_sc.Value = Amount.Rows[0]["ServiceCharge"].ToString();
                    hdnDepartment.Value = Amount.Rows[0]["DepartmentName"].ToString();
                    hdnDepartmentId.Value = Amount.Rows[0]["DepartmentId"].ToString();
                    hdnSerCategory.Value = Amount.Rows[0]["SerCategoryName"].ToString();
                    hdnSerCategoryId.Value = Amount.Rows[0]["ServiceCategoryId"].ToString();
                    hdnSerSubCategory.Value = Amount.Rows[0]["SerSubCategoryName"].ToString();
                    hdnSerSubCategoryId.Value = Amount.Rows[0]["ServiceSubCategoryId"].ToString();
                    drpDepartment.SelectedValue = Amount.Rows[0]["DepartmentId"].ToString();
                    drpSerCategory.SelectedValue = Amount.Rows[0]["ServiceCategoryId"].ToString();
                    drpSerSubCategory.SelectedValue = Amount.Rows[0]["ServiceSubCategoryId"].ToString();
                }
            }
            UpdDepartmentDropdown.Update();
            UpdSerCategoryDropdown.Update();
            UpdSerSubCategoryDropdown.Update();
            UpdTxtDescription.Update();
            UpdTxtPrice.Update();
            UpdTxtServiceCharge.Update();
            UpdTxtQty.Update();
            UpdTxtTaxAmt.Update();
            UpdTxtPriceWithTax.Update();
            UpdTxtFine.Update();
            UpdTxtTotPrice.Update();
            updexpense.Update();
            Updtxt_discount.Update();
            InlineCalculation();

        }

        public void fillServices(int res)
        {
            drpDepartment.DataSource = obj_mas.Drp_Department();
            drpDepartment.DataTextField = "Text";
            drpDepartment.DataValueField = "Value";
            drpDepartment.DataBind();
            UpdDepartmentDropdown.Update();

            drpService.DataSource = obj_trans.Drp_Service(0);
            drpService.DataTextField = "Text";
            drpService.DataValueField = "Value";
            drpService.DataBind();
            drpService.SelectedValue = res.ToString();
            UpdServiceDropdown.Update();
            drpService_OnSelectedIndexChanged(null, null);
        }

        public void InlineCalculation()
        {
            decimal Total_Amt = 0;

            decimal tot = 0;

            foreach (RepeaterItem itm in rpt_Item_list.Items)
            {
                TextBox txtInvDTotal = (TextBox)itm.FindControl("txtInvDTotal");
                tot = txtInvDTotal.Text == "" ? 0 : Convert.ToDecimal(txtInvDTotal.Text);
                Total_Amt += tot;
            }
            if (drpService.SelectedValue != "" && txt_displayPrice.Text != "" && txt_Qty.Text != "")
                Total_Amt += txt_totPrice.Text == "" ? 0 : Convert.ToDecimal(txt_totPrice.Text);

            decimal Final = Total_Amt;
            if (hdnIsDisableRoundOff.Value == "0")
            {
                string[] substr = Total_Amt.ToString().Split('.');
                decimal AmtAfterDecimal = Total_Amt - Convert.ToDecimal(substr[0]);
                decimal AmtBeforeDecimal = Total_Amt - AmtAfterDecimal;
                decimal AmtDecimal = 0;
                Final = 0;
                if (AmtAfterDecimal <= 0.12M)
                {
                    AmtDecimal = 0;
                }
                else if ((AmtAfterDecimal >= 0.13M) && (AmtAfterDecimal <= 0.37M))
                {
                    AmtDecimal = 0.25M;
                }
                else if ((AmtAfterDecimal >= 0.38M) && (AmtAfterDecimal <= 0.62M))
                {
                    AmtDecimal = 0.50M;
                }
                else if ((AmtAfterDecimal >= 0.63M) && (AmtAfterDecimal <= 0.87M))
                {
                    AmtDecimal = 0.75M;
                }
                else if (AmtAfterDecimal >= 0.88M)
                {
                    AmtDecimal = 1;
                }
                Final = AmtBeforeDecimal + AmtDecimal;
            }
            txt_grand.Text = (Convert.ToDecimal(Final)).ToString("0.00");

            Upd_Total_Panel.Update();
        }

        /*New Item*/
        protected void btn_new_line_OnClick(object sender, EventArgs e)
        {
            DataTable dt_ser = new DataTable();
            dt_ser.Columns.Add("D_id", typeof(int));
            dt_ser.Columns.Add("DepartmentId", typeof(int));
            dt_ser.Columns.Add("CategoryId", typeof(int));
            dt_ser.Columns.Add("ServiceSubCategoryId", typeof(int));
            dt_ser.Columns.Add("Service_Id", typeof(int));
            dt_ser.Columns.Add("DepartmentName", typeof(string));
            dt_ser.Columns.Add("SerCategoryName", typeof(string));
            dt_ser.Columns.Add("SerSubCategoryName", typeof(string));
            dt_ser.Columns.Add("ServiceFullName", typeof(string));
            dt_ser.Columns.Add("Particulars", typeof(string));
            dt_ser.Columns.Add("DisplayPrice", typeof(decimal));
            dt_ser.Columns.Add("Price", typeof(decimal));
            dt_ser.Columns.Add("Expense", typeof(decimal));
            dt_ser.Columns.Add("ServiceCharge", typeof(decimal));
            dt_ser.Columns.Add("AdditionalServiceCharge", typeof(decimal));
            dt_ser.Columns.Add("Quantity", typeof(decimal));
            dt_ser.Columns.Add("TaxAmount", typeof(decimal));
            dt_ser.Columns.Add("Tax", typeof(decimal));
            dt_ser.Columns.Add("PriceWitTax", typeof(decimal));
            dt_ser.Columns.Add("FineApplicable", typeof(int));
            dt_ser.Columns.Add("Fine", typeof(decimal));
            dt_ser.Columns.Add("Total", typeof(decimal));
            dt_ser.Columns.Add("TemplateId", typeof(int));
            dt_ser.Columns.Add("Discount", typeof(decimal));

            if (rpt_Item_list.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_Item_list.Items)
                {
                    HiddenField hdnInvDId = (HiddenField)itm.FindControl("hdnInvDId");
                    HiddenField hdnInvDDepartmentId = (HiddenField)itm.FindControl("hdnInvDDepartmentId");
                    HiddenField hdnInvDCategoryId = (HiddenField)itm.FindControl("hdnInvDCategoryId");
                    HiddenField hdnInvDSerSubCategoryId = (HiddenField)itm.FindControl("hdnInvDSerSubCategoryId");
                    HiddenField hdnInvDServiceId = (HiddenField)itm.FindControl("hdnInvDServiceId");
                    HiddenField hdnInvDDepartment = (HiddenField)itm.FindControl("hdnInvDDepartment");
                    HiddenField hdnInvDSerCategory = (HiddenField)itm.FindControl("hdnInvDSerCategory");
                    HiddenField hdnInvDSerSubCategory = (HiddenField)itm.FindControl("hdnInvDSerSubCategory");
                    Label lblServiceFullName = (Label)itm.FindControl("lblServiceFullName");
                    TextBox lblInvDdesc = (TextBox)itm.FindControl("lblInvDdesc");
                    TextBox txtInvDDisplayPrice = (TextBox)itm.FindControl("txtInvDDisplayPrice");
                    HiddenField hdnInvDPrice = (HiddenField)itm.FindControl("hdnInvDPrice");
                    HiddenField hdnInvDExpense = (HiddenField)itm.FindControl("hdnInvDExpense");
                    HiddenField hdnInvDServiceCharge = (HiddenField)itm.FindControl("hdnInvDServiceCharge");
                    TextBox txtInvDAddServiceCharge = (TextBox)itm.FindControl("txtInvDAddServiceCharge");
                    TextBox txtInvDQty = (TextBox)itm.FindControl("txtInvDQty");
                    TextBox txtInvDTaxAmount = (TextBox)itm.FindControl("txtInvDTaxAmount");
                    HiddenField hdnInvDTax = (HiddenField)itm.FindControl("hdnInvDTax");
                    TextBox txtInvDPriceWitTax = (TextBox)itm.FindControl("txtInvDPriceWitTax");
                    HiddenField hdnInvDFineApplicable = (HiddenField)itm.FindControl("hdnInvDFineApplicable");
                    TextBox txtInvDFine = (TextBox)itm.FindControl("txtInvDFine");
                    TextBox txtInvDTotal = (TextBox)itm.FindControl("txtInvDTotal");
                    HiddenField hdnTemplateId = (HiddenField)itm.FindControl("hdnTemplateId");
                    TextBox txtInvDdiscount = (TextBox)itm.FindControl("txtInvDdiscount");
                    TextBox txtExpense = (TextBox)itm.FindControl("txtInvDExpense");

                    //dt_ser.Rows.Add(Convert.ToInt32(hdnInvDId.Value), hdnInvDDepartmentId.Value==""?(int?)null: Convert.ToInt32(hdnInvDDepartmentId.Value),
                    // hdnInvDCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDCategoryId.Value), hdnInvDSerSubCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDSerSubCategoryId.Value),
                    //Convert.ToInt32(hdnInvDServiceId.Value), hdnInvDDepartment.Value, hdnInvDSerCategory.Value, hdnInvDSerSubCategory.Value,
                    //lblServiceFullName.Text, lblInvDdesc.Text, Convert.ToDecimal(txtInvDDisplayPrice.Text), Convert.ToDecimal(hdnInvDPrice.Value), Convert.ToDecimal(hdnInvDExpense.Value),
                    //Convert.ToDecimal(hdnInvDServiceCharge.Value), txtInvDAddServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDAddServiceCharge.Text),
                    //Convert.ToDecimal(txtInvDQty.Text), Convert.ToDecimal(txtInvDTaxAmount.Text), Convert.ToDecimal(hdnInvDTax.Value),
                    //Convert.ToDecimal(txtInvDPriceWitTax.Text), Convert.ToInt32(hdnInvDFineApplicable.Value),
                    //txtInvDFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDFine.Text), Convert.ToDecimal(txtInvDTotal.Text),
                    //hdnTemplateId.Value==""?(int?)null:Convert.ToInt32(hdnTemplateId.Value),
                    //txtInvDdiscount.Text==""?0:Convert.ToDecimal(txtInvDdiscount.Text));
                    dt_ser.Rows.Add(Convert.ToInt32(hdnInvDId.Value), hdnInvDDepartmentId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDDepartmentId.Value),
                     hdnInvDCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDCategoryId.Value), hdnInvDSerSubCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDSerSubCategoryId.Value),
                    Convert.ToInt32(hdnInvDServiceId.Value), hdnInvDDepartment.Value, hdnInvDSerCategory.Value, hdnInvDSerSubCategory.Value,
                    lblServiceFullName.Text, lblInvDdesc.Text, Convert.ToDecimal(txtInvDDisplayPrice.Text), Convert.ToDecimal(hdnInvDPrice.Value), Convert.ToDecimal(txtExpense.Text),
                    Convert.ToDecimal(hdnInvDServiceCharge.Value), txtInvDAddServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDAddServiceCharge.Text),
                    Convert.ToDecimal(txtInvDQty.Text), Convert.ToDecimal(txtInvDTaxAmount.Text), Convert.ToDecimal(hdnInvDTax.Value),
                    Convert.ToDecimal(txtInvDPriceWitTax.Text), Convert.ToInt32(hdnInvDFineApplicable.Value),
                    txtInvDFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDFine.Text), Convert.ToDecimal(txtInvDTotal.Text),
                    hdnTemplateId.Value == "" ? (int?)null : Convert.ToInt32(hdnTemplateId.Value),
                    txtInvDdiscount.Text == "" ? 0 : Convert.ToDecimal(txtInvDdiscount.Text));
                }
            }

            if (drpService.SelectedValue != "" && txt_displayPrice.Text != "" && txt_Qty.Text != "")
            {
                //dt_ser.Rows.Add(0, hdnDepartmentId.Value==""?(int?)null:Convert.ToInt32(hdnDepartmentId.Value),
                //    hdnSerCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnSerCategoryId.Value), hdnSerSubCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnSerSubCategoryId.Value), Convert.ToInt32(drpService.SelectedValue),
                //    hdnDepartment.Value, hdnSerCategory.Value, hdnSerSubCategory.Value,
                //    (hdnDepartment.Value + '/' + hdnSerCategory.Value + '/' + hdnSerSubCategory.Value + '/' + drpService.Text), txt_desc.Text, Convert.ToDecimal(txt_displayPrice.Text), Convert.ToDecimal(hdnPrice.Value), Convert.ToDecimal(hdn_expn.Value),
                //       Convert.ToDecimal(hdn_sc.Value), txtServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtServiceCharge.Text), Convert.ToDecimal(txt_Qty.Text), Convert.ToDecimal(txt_taxamt.Text), Convert.ToDecimal(hdn_tax.Value),
                //       Convert.ToDecimal(txt_PriceWitTax.Text), Convert.ToInt32(hdnFineApplicable.Value), 
                //       txtFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtFine.Text), Convert.ToDecimal(txt_totPrice.Text),
                //       (int?)null,txt_discount.Text==""?0:Convert.ToDecimal(txt_discount.Text));
                dt_ser.Rows.Add(0, hdnDepartmentId.Value == "" ? (int?)null : Convert.ToInt32(hdnDepartmentId.Value),
                    hdnSerCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnSerCategoryId.Value), hdnSerSubCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnSerSubCategoryId.Value), Convert.ToInt32(drpService.SelectedValue),
                    hdnDepartment.Value, hdnSerCategory.Value, hdnSerSubCategory.Value,
                    (hdnDepartment.Value + '/' + hdnSerCategory.Value + '/' + hdnSerSubCategory.Value + '/' + drpService.Text), txt_desc.Text, Convert.ToDecimal(txt_displayPrice.Text), Convert.ToDecimal(hdnPrice.Value), Convert.ToDecimal(txtexpense.Text),
                       Convert.ToDecimal(hdn_sc.Value), txtServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtServiceCharge.Text), Convert.ToDecimal(txt_Qty.Text), Convert.ToDecimal(txt_taxamt.Text), Convert.ToDecimal(hdn_tax.Value),
                       Convert.ToDecimal(txt_PriceWitTax.Text), Convert.ToInt32(hdnFineApplicable.Value),
                       txtFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtFine.Text), Convert.ToDecimal(txt_totPrice.Text),
                       (int?)null, txt_discount.Text == "" ? 0 : Convert.ToDecimal(txt_discount.Text));
            }
            rpt_Item_list.DataSource = dt_ser;
            rpt_Item_list.DataBind();
            ClearServiceDetail();
            drpDepartment.Focus();
            //Upd_InvoiceDetail_Panel.Update();
            Upd_Item_Panel.Update();
        }

        /*Remove Item*/
        protected void btn_remove_line_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            DataTable dt_ser = new DataTable();
            dt_ser.Columns.Add("D_id", typeof(int));
            dt_ser.Columns.Add("DepartmentId", typeof(int));
            dt_ser.Columns.Add("CategoryId", typeof(int));
            dt_ser.Columns.Add("ServiceSubCategoryId", typeof(int));
            dt_ser.Columns.Add("Service_Id", typeof(int));
            dt_ser.Columns.Add("DepartmentName", typeof(string));
            dt_ser.Columns.Add("SerCategoryName", typeof(string));
            dt_ser.Columns.Add("SerSubCategoryName", typeof(string));
            dt_ser.Columns.Add("ServiceFullName", typeof(string));
            dt_ser.Columns.Add("Particulars", typeof(string));
            dt_ser.Columns.Add("DisplayPrice", typeof(decimal));
            dt_ser.Columns.Add("Price", typeof(decimal));
            dt_ser.Columns.Add("Expense", typeof(decimal));
            dt_ser.Columns.Add("ServiceCharge", typeof(decimal));
            dt_ser.Columns.Add("AdditionalServiceCharge", typeof(decimal));
            dt_ser.Columns.Add("Quantity", typeof(decimal));
            dt_ser.Columns.Add("TaxAmount", typeof(decimal));
            dt_ser.Columns.Add("Tax", typeof(decimal));
            dt_ser.Columns.Add("PriceWitTax", typeof(decimal));
            dt_ser.Columns.Add("FineApplicable", typeof(int));
            dt_ser.Columns.Add("Fine", typeof(decimal));
            dt_ser.Columns.Add("Total", typeof(decimal));
            dt_ser.Columns.Add("TemplateId", typeof(int));
            dt_ser.Columns.Add("Discount", typeof(decimal));


            if (rpt_Item_list.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_Item_list.Items)
                {
                    HiddenField hdnInvDId = (HiddenField)itm.FindControl("hdnInvDId");
                    HiddenField hdnInvDDepartmentId = (HiddenField)itm.FindControl("hdnInvDDepartmentId");
                    HiddenField hdnInvDCategoryId = (HiddenField)itm.FindControl("hdnInvDCategoryId");
                    HiddenField hdnInvDSerSubCategoryId = (HiddenField)itm.FindControl("hdnInvDSerSubCategoryId");
                    HiddenField hdnInvDServiceId = (HiddenField)itm.FindControl("hdnInvDServiceId");
                    HiddenField hdnInvDDepartment = (HiddenField)itm.FindControl("hdnInvDDepartment");
                    HiddenField hdnInvDSerCategory = (HiddenField)itm.FindControl("hdnInvDSerCategory");
                    HiddenField hdnInvDSerSubCategory = (HiddenField)itm.FindControl("hdnInvDSerSubCategory");
                    Label lblServiceFullName = (Label)itm.FindControl("lblServiceFullName");
                    TextBox lblInvDdesc = (TextBox)itm.FindControl("lblInvDdesc");
                    TextBox txtInvDDisplayPrice = (TextBox)itm.FindControl("txtInvDDisplayPrice");
                    HiddenField hdnInvDPrice = (HiddenField)itm.FindControl("hdnInvDPrice");
                    HiddenField hdnInvDExpense = (HiddenField)itm.FindControl("hdnInvDExpense");
                    HiddenField hdnInvDServiceCharge = (HiddenField)itm.FindControl("hdnInvDServiceCharge");
                    TextBox txtInvDAddServiceCharge = (TextBox)itm.FindControl("txtInvDAddServiceCharge");
                    TextBox txtInvDQty = (TextBox)itm.FindControl("txtInvDQty");
                    TextBox txtInvDTaxAmount = (TextBox)itm.FindControl("txtInvDTaxAmount");
                    HiddenField hdnInvDTax = (HiddenField)itm.FindControl("hdnInvDTax");
                    TextBox txtInvDPriceWitTax = (TextBox)itm.FindControl("txtInvDPriceWitTax");
                    HiddenField hdnInvDFineApplicable = (HiddenField)itm.FindControl("hdnInvDFineApplicable");
                    TextBox txtInvDFine = (TextBox)itm.FindControl("txtInvDFine");
                    TextBox txtInvDTotal = (TextBox)itm.FindControl("txtInvDTotal");
                    HiddenField hdnTemplateId = (HiddenField)itm.FindControl("hdnTemplateId");
                    TextBox txtInvDdiscount = (TextBox)itm.FindControl("txtInvDdiscount");
                    TextBox txtExpense = (TextBox)itm.FindControl("txtInvDExpense");

                    //dt_ser.Rows.Add(Convert.ToInt32(hdnInvDId.Value), hdnInvDDepartmentId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDDepartmentId.Value),
                    // hdnInvDCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDCategoryId.Value), hdnInvDSerSubCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDSerSubCategoryId.Value),
                    //Convert.ToInt32(hdnInvDServiceId.Value), hdnInvDDepartment.Value, hdnInvDSerCategory.Value, hdnInvDSerSubCategory.Value,
                    //lblServiceFullName.Text, lblInvDdesc.Text, Convert.ToDecimal(txtInvDDisplayPrice.Text),
                    //Convert.ToDecimal(hdnInvDPrice.Value), Convert.ToDecimal(hdnInvDExpense.Value),
                    //Convert.ToDecimal(hdnInvDServiceCharge.Value), txtInvDAddServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDAddServiceCharge.Text),
                    //Convert.ToDecimal(txtInvDQty.Text), Convert.ToDecimal(txtInvDTaxAmount.Text), Convert.ToDecimal(hdnInvDTax.Value),
                    //Convert.ToDecimal(txtInvDPriceWitTax.Text), Convert.ToInt32(hdnInvDFineApplicable.Value),
                    //txtInvDFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDFine.Text), Convert.ToDecimal(txtInvDTotal.Text),
                    //hdnTemplateId.Value == "" ? (int?)null : Convert.ToInt32(hdnTemplateId.Value),
                    // txtInvDdiscount.Text == "" ? 0 : Convert.ToDecimal(txtInvDdiscount.Text));
                    dt_ser.Rows.Add(Convert.ToInt32(hdnInvDId.Value), hdnInvDDepartmentId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDDepartmentId.Value),
                     hdnInvDCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDCategoryId.Value), hdnInvDSerSubCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDSerSubCategoryId.Value),
                    Convert.ToInt32(hdnInvDServiceId.Value), hdnInvDDepartment.Value, hdnInvDSerCategory.Value, hdnInvDSerSubCategory.Value,
                    lblServiceFullName.Text, lblInvDdesc.Text, Convert.ToDecimal(txtInvDDisplayPrice.Text),
                    Convert.ToDecimal(hdnInvDPrice.Value), Convert.ToDecimal(txtExpense.Text),
                    Convert.ToDecimal(hdnInvDServiceCharge.Value), txtInvDAddServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDAddServiceCharge.Text),
                    Convert.ToDecimal(txtInvDQty.Text), Convert.ToDecimal(txtInvDTaxAmount.Text), Convert.ToDecimal(hdnInvDTax.Value),
                    Convert.ToDecimal(txtInvDPriceWitTax.Text), Convert.ToInt32(hdnInvDFineApplicable.Value),
                    txtInvDFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDFine.Text), Convert.ToDecimal(txtInvDTotal.Text),
                    hdnTemplateId.Value == "" ? (int?)null : Convert.ToInt32(hdnTemplateId.Value),
                     txtInvDdiscount.Text == "" ? 0 : Convert.ToDecimal(txtInvDdiscount.Text));

                }
            }

            dt_ser.Rows.RemoveAt(itemrp.ItemIndex);
            rpt_Item_list.DataSource = dt_ser;
            rpt_Item_list.DataBind();

            lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
            InlineCalculation();
            //Upd_InvoiceDetail_Panel.Update();
            Upd_Item_Panel.Update();
        }

        /*Edit Item*/
        protected void btn_edit_line_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            HiddenField hdnInvDIdP = (HiddenField)itemrp.FindControl("hdnInvDId");
            HiddenField hdnInvDDepartmentIdP = (HiddenField)itemrp.FindControl("hdnInvDDepartmentId");
            HiddenField hdnInvDCategoryIdP = (HiddenField)itemrp.FindControl("hdnInvDCategoryId");
            HiddenField hdnInvDSerSubCategoryIdP = (HiddenField)itemrp.FindControl("hdnInvDSerSubCategoryId");
            HiddenField hdnInvDServiceIdP = (HiddenField)itemrp.FindControl("hdnInvDServiceId");
            HiddenField hdnInvDDepartmentP = (HiddenField)itemrp.FindControl("hdnInvDDepartment");
            HiddenField hdnInvDSerCategoryP = (HiddenField)itemrp.FindControl("hdnInvDSerCategory");
            HiddenField hdnInvDSerSubCategoryP = (HiddenField)itemrp.FindControl("hdnInvDSerSubCategory");
            Label lblServiceFullNameP = (Label)itemrp.FindControl("lblServiceFullName");
            TextBox lblInvDdescP = (TextBox)itemrp.FindControl("lblInvDdesc");
            TextBox txtInvDDisplayPriceP = (TextBox)itemrp.FindControl("txtInvDDisplayPrice");
            HiddenField hdnInvDPriceP = (HiddenField)itemrp.FindControl("hdnInvDPrice");
            HiddenField hdnInvDExpenseP = (HiddenField)itemrp.FindControl("hdnInvDExpense");
            HiddenField hdnInvDServiceChargeP = (HiddenField)itemrp.FindControl("hdnInvDServiceCharge");
            TextBox txtInvDAddServiceChargeP = (TextBox)itemrp.FindControl("txtInvDAddServiceCharge");
            TextBox txtInvDQtyP = (TextBox)itemrp.FindControl("txtInvDQty");
            TextBox txtInvDTaxAmountP = (TextBox)itemrp.FindControl("txtInvDTaxAmount");
            HiddenField hdnInvDTaxP = (HiddenField)itemrp.FindControl("hdnInvDTax");
            TextBox txtInvDPriceWitTaxP = (TextBox)itemrp.FindControl("txtInvDPriceWitTax");
            HiddenField hdnInvDFineApplicableP = (HiddenField)itemrp.FindControl("hdnInvDFineApplicable");
            TextBox txtInvDFineP = (TextBox)itemrp.FindControl("txtInvDFine");
            TextBox txtInvDTotalP = (TextBox)itemrp.FindControl("txtInvDTotal");
            TextBox txtInvDdiscountP = (TextBox)itemrp.FindControl("txtInvDdiscount");
            TextBox txtExpense = (TextBox)itemrp.FindControl("txtInvDExpense");
            ClearServiceDetail();

            hdn_QuoDetailId.Value = hdnInvDIdP.Value;
            drpDepartment.SelectedValue = hdnInvDDepartmentIdP.Value;
            hdnDepartmentId.Value = hdnInvDDepartmentIdP.Value;
            drpSerCategory.SelectedValue = hdnInvDCategoryIdP.Value;
            hdnSerCategoryId.Value = hdnInvDCategoryIdP.Value;
            drpSerSubCategory.SelectedValue = hdnInvDSerSubCategoryIdP.Value;
            hdnSerSubCategoryId.Value = hdnInvDSerSubCategoryIdP.Value;
            drpService.SelectedValue = hdnInvDServiceIdP.Value;
            hdnDepartment.Value = hdnInvDDepartmentP.Value;
            hdnSerCategory.Value = hdnInvDSerCategoryP.Value;
            hdnSerSubCategory.Value = hdnInvDSerSubCategoryP.Value;
            txt_desc.Text = lblInvDdescP.Text;
            txt_displayPrice.Text = txtInvDDisplayPriceP.Text;
            hdnPrice.Value = hdnInvDPriceP.Value;
            hdn_expn.Value = hdnInvDExpenseP.Value;
            hdn_sc.Value = hdnInvDServiceChargeP.Value;
            txtServiceCharge.Text = txtInvDAddServiceChargeP.Text;
            txt_Qty.Text = txtInvDQtyP.Text;
            txt_taxamt.Text = txtInvDTaxAmountP.Text;
            hdn_tax.Value = hdnInvDTaxP.Value;
            txt_PriceWitTax.Text = txtInvDPriceWitTaxP.Text;
            hdnFineApplicable.Value = hdnInvDFineApplicableP.Value;
            txtFine.Text = txtInvDFineP.Text;
            txt_totPrice.Text = txtInvDTotalP.Text;
            txt_discount.Text = txtInvDdiscountP.Text;
            txtexpense.Text = txtExpense.Text;

            DataTable dt_ser = new DataTable();
            dt_ser.Columns.Add("D_id", typeof(int));
            dt_ser.Columns.Add("DepartmentId", typeof(int));
            dt_ser.Columns.Add("CategoryId", typeof(int));
            dt_ser.Columns.Add("ServiceSubCategoryId", typeof(int));
            dt_ser.Columns.Add("Service_Id", typeof(int));
            dt_ser.Columns.Add("DepartmentName", typeof(string));
            dt_ser.Columns.Add("SerCategoryName", typeof(string));
            dt_ser.Columns.Add("SerSubCategoryName", typeof(string));
            dt_ser.Columns.Add("ServiceFullName", typeof(string));
            dt_ser.Columns.Add("Particulars", typeof(string));
            dt_ser.Columns.Add("DisplayPrice", typeof(decimal));
            dt_ser.Columns.Add("Price", typeof(decimal));
            dt_ser.Columns.Add("Expense", typeof(decimal));
            dt_ser.Columns.Add("ServiceCharge", typeof(decimal));
            dt_ser.Columns.Add("AdditionalServiceCharge", typeof(decimal));
            dt_ser.Columns.Add("Quantity", typeof(decimal));
            dt_ser.Columns.Add("TaxAmount", typeof(decimal));
            dt_ser.Columns.Add("Tax", typeof(decimal));
            dt_ser.Columns.Add("PriceWitTax", typeof(decimal));
            dt_ser.Columns.Add("FineApplicable", typeof(int));
            dt_ser.Columns.Add("Fine", typeof(decimal));
            dt_ser.Columns.Add("Total", typeof(decimal));
            dt_ser.Columns.Add("Discount", typeof(decimal));


            if (rpt_Item_list.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_Item_list.Items)
                {
                    HiddenField hdnInvDId = (HiddenField)itm.FindControl("hdnInvDId");
                    HiddenField hdnInvDDepartmentId = (HiddenField)itm.FindControl("hdnInvDDepartmentId");
                    HiddenField hdnInvDCategoryId = (HiddenField)itm.FindControl("hdnInvDCategoryId");
                    HiddenField hdnInvDSerSubCategoryId = (HiddenField)itm.FindControl("hdnInvDSerSubCategoryId");
                    HiddenField hdnInvDServiceId = (HiddenField)itm.FindControl("hdnInvDServiceId");
                    HiddenField hdnInvDDepartment = (HiddenField)itm.FindControl("hdnInvDDepartment");
                    HiddenField hdnInvDSerCategory = (HiddenField)itm.FindControl("hdnInvDSerCategory");
                    HiddenField hdnInvDSerSubCategory = (HiddenField)itm.FindControl("hdnInvDSerSubCategory");
                    Label lblServiceFullName = (Label)itm.FindControl("lblServiceFullName");
                    TextBox lblInvDdesc = (TextBox)itm.FindControl("lblInvDdesc");
                    TextBox txtInvDDisplayPrice = (TextBox)itm.FindControl("txtInvDDisplayPrice");
                    HiddenField hdnInvDPrice = (HiddenField)itm.FindControl("hdnInvDPrice");
                    HiddenField hdnInvDExpense = (HiddenField)itm.FindControl("hdnInvDExpense");
                    HiddenField hdnInvDServiceCharge = (HiddenField)itm.FindControl("hdnInvDServiceCharge");
                    TextBox txtInvDAddServiceCharge = (TextBox)itm.FindControl("txtInvDAddServiceCharge");
                    TextBox txtInvDQty = (TextBox)itm.FindControl("txtInvDQty");
                    TextBox txtInvDTaxAmount = (TextBox)itm.FindControl("txtInvDTaxAmount");
                    HiddenField hdnInvDTax = (HiddenField)itm.FindControl("hdnInvDTax");
                    TextBox txtInvDPriceWitTax = (TextBox)itm.FindControl("txtInvDPriceWitTax");
                    HiddenField hdnInvDFineApplicable = (HiddenField)itm.FindControl("hdnInvDFineApplicable");
                    TextBox txtInvDFine = (TextBox)itm.FindControl("txtInvDFine");
                    TextBox txtInvDTotal = (TextBox)itm.FindControl("txtInvDTotal");
                    TextBox txtInvDdiscount = (TextBox)itm.FindControl("txtInvDdiscount");
                    TextBox txtInvDExpense = (TextBox)itm.FindControl("txtInvDExpense");

                    //dt_ser.Rows.Add(Convert.ToInt32(hdnInvDId.Value), hdnInvDDepartmentId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDDepartmentId.Value),
                    // hdnInvDCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDCategoryId.Value), hdnInvDSerSubCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDSerSubCategoryId.Value),
                    //Convert.ToInt32(hdnInvDServiceId.Value), hdnInvDDepartment.Value, hdnInvDSerCategory.Value, hdnInvDSerSubCategory.Value,
                    //lblServiceFullName.Text, lblInvDdesc.Text, Convert.ToDecimal(txtInvDDisplayPrice.Text), Convert.ToDecimal(hdnInvDPrice.Value), Convert.ToDecimal(hdnInvDExpense.Value),
                    //Convert.ToDecimal(hdnInvDServiceCharge.Value), txtInvDAddServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDAddServiceCharge.Text),
                    //Convert.ToDecimal(txtInvDQty.Text), Convert.ToDecimal(txtInvDTaxAmount.Text), Convert.ToDecimal(hdnInvDTax.Value),
                    //Convert.ToDecimal(txtInvDPriceWitTax.Text), Convert.ToInt32(hdnInvDFineApplicable.Value), 
                    //txtInvDFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDFine.Text), Convert.ToDecimal(txtInvDTotal.Text),
                    //txtInvDdiscount.Text==""?0:Convert.ToDecimal(txtInvDdiscount.Text));
                    dt_ser.Rows.Add(Convert.ToInt32(hdnInvDId.Value), hdnInvDDepartmentId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDDepartmentId.Value),
                     hdnInvDCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDCategoryId.Value), hdnInvDSerSubCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDSerSubCategoryId.Value),
                    Convert.ToInt32(hdnInvDServiceId.Value), hdnInvDDepartment.Value, hdnInvDSerCategory.Value, hdnInvDSerSubCategory.Value,
                    lblServiceFullName.Text, lblInvDdesc.Text, Convert.ToDecimal(txtInvDDisplayPrice.Text), Convert.ToDecimal(hdnInvDPrice.Value), Convert.ToDecimal(txtInvDExpense.Text),
                    Convert.ToDecimal(hdnInvDServiceCharge.Value), txtInvDAddServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDAddServiceCharge.Text),
                    Convert.ToDecimal(txtInvDQty.Text), Convert.ToDecimal(txtInvDTaxAmount.Text), Convert.ToDecimal(hdnInvDTax.Value),
                    Convert.ToDecimal(txtInvDPriceWitTax.Text), Convert.ToInt32(hdnInvDFineApplicable.Value),
                    txtInvDFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDFine.Text), Convert.ToDecimal(txtInvDTotal.Text),
                    txtInvDdiscount.Text == "" ? 0 : Convert.ToDecimal(txtInvDdiscount.Text));
                }
            }

            dt_ser.Rows.RemoveAt(itemrp.ItemIndex);
            rpt_Item_list.DataSource = dt_ser;
            rpt_Item_list.DataBind();

            lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
            //Upd_InvoiceDetail_Panel.Update();
            Upd_Item_Panel.Update();
        }

        /*Reset*/
        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
        }

        protected void rptQuotationHistoryOnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdn_id");
            if (e.CommandName == "Default")
            {
                int res = obj_trans.GenerateDefaultQuotation(Convert.ToInt32(hdn_rpt_id.Value),Convert.ToInt32(hdn_user_id.Value));

                if (res > 0)
                {
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
            else if (e.CommandName == "Print")
            {
                DataTable dt = obj_mas.Edit_GeneralSettings();

                int Format = dt.Rows[0]["QuotationFormat"].ToString() == "" ? 1 : Convert.ToInt32(dt.Rows[0]["QuotationFormat"].ToString());
                string url = "";
                if (Format == 3)
                    url = "../Reports/QuotationPrintFormat3.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 2)
                    url = "../Reports/QuotationPrintFormat2.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 4)
                    url = "../Reports/QuotationPrintFormat4.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 5)
                    url = "../Reports/QuotationPrintFormat5.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 6)
                    url = "../Reports/QuotationPrintFormat6.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 7)
                    url = "../Reports/QuotationPrintFormat7.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else if (Format == 8)
                    url = "../Reports/QuotationPrintFormat8.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);
                else
                    url = "../Reports/QuotationPrintFormat1.aspx?id=" + Convert.ToInt32(hdn_rpt_id.Value);

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
            }
        }

        protected void rptQuotationHistoryOnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Button btnPrint = (Button)e.Item.FindControl("btnPrint");
                btnPrint.Visible = hdn_print.Value == "0" ? false : true;
            }

        }
        
        protected void btnOpenCancel_Click(object sender, EventArgs e)
        {
            int res = obj_trans.CancelQuotation(Convert.ToInt32(hdn_id.Value), "", Convert.ToInt32(hdn_user_id.Value));
            if (res > 0)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                lbl_msgin.Text = "Cancelled Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msgin.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            Upd_Add_PanelInner.Update();
           
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

        public void ClearServiceDetail()
        {
            lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
            hdn_QuoDetailId.Value = "-" + (rpt_Item_list.Items.Count + 1).ToString();
            drpDepartment.ClearSelection();
            drpDepartment.Text = "";
            drpSerCategory.ClearSelection();
            drpSerCategory.Text = "";
            drpSerSubCategory.ClearSelection();
            drpSerSubCategory.Text = "";
            drpService.ClearSelection();
            drpService.Text = "";

            fill_FilterDropDown(0);
        }

        /*Clear All the Data*/
        public void Clear()
        {
            hdn_PageName.Value = "Quotation";/*Used in Customer User Control*/
            hdn_id.Value = hdnLeadId.Value= "0";
            drp_customer.ClearSelection();
            drp_customer.Text = txtSubject.Text="";
            txt_remark.Text = "";
            txt_grand.Text = "";
            DataTable dt = obj_mas.Edit_GeneralSettings();
            txt_remark.Text = dt.Rows[0]["DefaultQutotnRemark"].ToString();

            if (hdnDefaultInvoiceType.Value == "1")
            {
                rbTaxInvoice.Checked = true;
                rbNormalInvoice.Checked = false;
            }
            else if (hdnDefaultInvoiceType.Value == "2")
            {
                rbTaxInvoice.Checked = false;
                rbNormalInvoice.Checked = true;
            }
            job_date.DbSelectedDate = DateTime.Now;

            drpDepartment.ClearSelection();
            drpDepartment.Text = "";
            drpSerCategory.ClearSelection();
            drpSerCategory.Text = "";
            drpSerSubCategory.ClearSelection();
            drpSerSubCategory.Text = "";
            drpService.ClearSelection();
            drpService.Text = "";
            fill_FilterDropDown(0);

            drpTemplates.Text = string.Empty;
            drpTemplates.ClearCheckedItems();
            drpTemplates.ClearSelection();

            rpt_Item_list.DataSource = null;
            rpt_Item_list.DataBind();
            ClearServiceDetail();

            divQuotationHistory.Visible = false;
            rptQuotationHistory.DataSource = null;
            rptQuotationHistory.DataBind();
            btn_save.Visible = hdn_add.Value == "0" ? false : true;
            btn_save_print.Visible = hdn_add_N_print.Value == "0" ? false : true;
            btn_print.Visible = false;
            btnNewVersion.Visible =btnOpenCancel.Visible= false;
            Get_Code();

            Upd_Add_PanelInner.Update();
        }

        /*Code for Display*/
        public void Get_Code()
        {
            DataTable dt = obj_common.Get_Code(53);
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
                    DataTable dt = obj_common.Action_Previlage_Validation(53, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdn_update.Value = dt.Rows[1][1].ToString();
                        hdn_print.Value = dt.Rows[2][1].ToString();
                        hdn_add_N_print.Value = dt.Rows[3][1].ToString();
                        hdn_update_N_print.Value = dt.Rows[4][1].ToString();
                        hdn_newVersion.Value = dt.Rows[5][1].ToString();
                        hdnsendmail.Value = dt.Rows[6][1].ToString();
                        hdn_cancel.Value = dt.Rows[7][1].ToString();
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

                    int val = obj_common.Form_Previlage_Validation(53, Convert.ToInt32(hdn_user_id.Value));
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

    }
}