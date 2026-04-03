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
using iTextSharp.text.pdf;
using System.Drawing.Text;
using System.Drawing.Printing;
using System.Web.Services;


namespace AmarCentre.Transactions.UserControl
{
    public partial class UCInvoice : System.Web.UI.UserControl
    {
        Transaction_Bal obj_trans = new Transaction_Bal();
        Master_Bal obj_mas = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
        Report_Bal obj_report = new Report_Bal();
        Voucher BalVoucher = new Voucher();
        public int ReceiptIdpub = 0;

        //public static DataTable dtCustomername = new DataTable();
        //public static DataTable dtCustomernameAgent = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void UCPageLoad(int PageId, int InvoiceId, string filter = "",int Count=10)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
            }
            else
            {
                hdnPageId.Value = PageId.ToString();    // 1-invoice , 2-home
                hdn_user_id.Value = Session["User_Id"].ToString();
                hdnLanguage.Value = GetLanguage(Convert.ToInt32(hdn_user_id.Value));
                hdnfilter.Value = filter;
                hdnCount.Value = Count.ToString();
                previlage_check();
                filldrops();
                Clear();

                if (InvoiceId > 0)
                {
                    BindData(InvoiceId);
                }
            }
        }

        public void UCPageLoadCR(int PageId, int RequestId)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
            }
            else
            {
                hdnPageId.Value = PageId.ToString();    // 1-invoice , 2-home , 3-CR
                hdn_user_id.Value = Session["User_Id"].ToString();
                hdnLanguage.Value = "1";
                hdnfilter.Value = "";
                previlage_check();
                filldrops();
                Clear();
                if (RequestId > 0)
                {
                    hdnrequestId.Value = RequestId.ToString();
                    FillDatas(RequestId);
                }
            }
        }

        public void FillDatas(int RequestId)
        {
            DataSet ds = obj_trans.EditRequestForInvoice(RequestId, Convert.ToInt32(hdnLanguage.Value), Convert.ToInt32(hdnSerPriceWTax.Value));
            DataTable dt1 = ds.Tables[0];
            DataTable dt_ser = ds.Tables[1];
            DataTable dtdoc = ds.Tables[2];

            drp_customer.SelectedValue = dt1.Rows[0]["CustomerId"].ToString();
            drp_customer_OnSelectedIndexChanged(null, null);
            drp_customer.Enabled = false;
            //rbTaxInvoice.Checked = true;
            //rbNormalInvoice.Checked = false;

            rpt_Item_list.DataSource = dt_ser;
            rpt_Item_list.DataBind();

            //foreach (RepeaterItem itm in rpt_Item_list.Items)
            //{
            //    HiddenField hdnDId = (HiddenField)itm.FindControl("hdnInvDId");
            //    Repeater rptDocument = (Repeater)itm.FindControl("rptDocument");

            //    DataTable dt = dtdoc.Clone();
            //    foreach (DataRow rin in dtdoc.Rows)
            //    {
            //        if (rin["RequestDetailId"].ToString() == hdnDId.Value)
            //            dt.ImportRow(rin);
            //    }

            //    rptDocument.DataSource = dt;
            //    rptDocument.DataBind();
            //}

            InlineCalculation();

            lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
            int hdnval = -1;
            foreach (RepeaterItem itm in rpt_Item_list.Items)
            {
                HiddenField hdnInvDId = (HiddenField)itm.FindControl("hdnInvDId");
                int inhdnval = Convert.ToInt32(hdnInvDId.Value);
                if (inhdnval < hdnval)
                    hdnval = inhdnval;
            }
            hdnval = hdnval - 1;
            hdn_InvDetailId.Value = hdnval.ToString();

            Upd_Add_Panel.Update();

            //hdn_InvDetailId.Value = "-" + (rpt_Item_list.Items.Count + 1).ToString();
        }


        public void fill_Customer()
        {
            drp_customer.DataSource = obj_trans.Drp_Customer();
            drp_customer.DataTextField = "Text";
            drp_customer.DataValueField = "Value";
            drp_customer.DataBind();
            drp_customer.Text = "";

            RadComboBoxItem CodeItem = new RadComboBoxItem();
            CodeItem.Text = "New Entry";
            CodeItem.Value = "0";
            drp_customer.Items.Insert(0, CodeItem);
        }

        public void filldrops()
        {
            DataSet ds = obj_trans.DrpForInvoice();

            //Invoice.dtCustomername = ds.Tables[0];

            //drp_customer.DataSource = ds.Tables[0];
            //drp_customer.DataTextField = "Name";
            //drp_customer.DataValueField = "Id";
            //drp_customer.DataBind();

            //RadComboBoxItem CodeItem = new RadComboBoxItem();
            //CodeItem.Text = "New Entry";
            //CodeItem.Value = "0";
            //drp_customer.Items.Insert(0, CodeItem);

            drpagent.DataSource = ds.Tables[1];
            drpagent.DataTextField = "Name";
            drpagent.DataValueField = "Id";
            drpagent.DataBind();
            drpagent.Text = "";

            drpTemplates.DataSource = ds.Tables[2];
            drpTemplates.DataTextField = "Name";
            drpTemplates.DataValueField = "Id";
            drpTemplates.DataBind();

            DataTable dtgen = ds.Tables[3];
            hdn_shwdiscount.Value = dtgen.Rows[0]["DisplayDiscountInInvoice"].ToString();
            hdnAgentCommmissionType.Value = dtgen.Rows[0]["AgentCommission"].ToString()==""?"1": dtgen.Rows[0]["AgentCommission"].ToString();

            hdnTaxAppliedWithDiscount.Value = dtgen.Rows[0]["TaxAppliedWithDiscount"].ToString();
            hdnSCInInvoice.Value = dtgen.Rows[0]["SCInInvoice"].ToString();
            hdnDefaultInvoiceType.Value = dtgen.Rows[0]["InvoiceType"].ToString();
            drpSerCategory.Visible = Convert.ToBoolean(dtgen.Rows[0]["CategoryRequiredInService"]);
            drpSerSubCategory.Visible = Convert.ToBoolean(dtgen.Rows[0]["SubCategoryRequiredInService"]);
            drpDepartment.Visible = Convert.ToBoolean(dtgen.Rows[0]["DepartmentInInvoiceVisible"]);
            hdnSerPriceWTax.Value = dtgen.Rows[0]["ServicePriceWithTax"].ToString();
            hdnDefaultBankCharge.Value = dtgen.Rows[0]["DefaultBankCharge"].ToString();
            hdnIsQuotaionEditable.Value = dtgen.Rows[0]["IsQuotaionEditable"].ToString();
            hdnIsTaxprintall.Value = dtgen.Rows[0]["IsTaxPrintForAll"].ToString();
            hdnDepartmentInInvoiceVisible.Value = dtgen.Rows[0]["DepartmentInInvoiceVisible"].ToString();
            hdnInvoiceFormatGen.Value = dtgen.Rows[0]["InvoiceFormat"].ToString();
            hdnIsDisableRoundOff.Value = dtgen.Rows[0]["IsDisableRoundOff"].ToString();
            hdnIsCommissionEditableInInvoice.Value = dtgen.Rows[0]["IsCommissionEditableInInvoice"].ToString();
            hdnIsEditInvoiceCreator.Value = dtgen.Rows[0]["IsEditInvoiceCreator"].ToString();

            drpinvoiceCreator.DataSource = ds.Tables[4];
            drpinvoiceCreator.DataTextField = "Name";
            drpinvoiceCreator.DataValueField = "Id";
            drpinvoiceCreator.DataBind();

            if (hdnAgentCommmissionType.Value == "1")
            {
                th_AgentCommission.Attributes.Add("style", "display:none");
                td_mainAgentCommission.Attributes.Add("style", "display:none");
            }

            if (hdn_shwdiscount.Value != "1")
            {
                th_discount.Attributes.Add("style", "display:none");
                td_maindiscount.Attributes.Add("style", "display:none");
                tr_maindiscount.Attributes.Add("style", "display:none");
                if (hdnAgentCommmissionType.Value == "1")
                {
                    td_total.Attributes.Add("colspan", "11");
                    tdroundoff.Attributes.Add("colspan", "11");
                    tdtxtCommssnTotal.Attributes.Add("colspan", "11");
                }
                else
                {
                    td_total.Attributes.Add("colspan", "12");
                    tdroundoff.Attributes.Add("colspan", "12");
                    tdtxtCommssnTotal.Attributes.Add("colspan", "12");
                }
            }
            else
            {
                if (hdnAgentCommmissionType.Value == "1")
                {
                    td_total.Attributes.Add("colspan", "12");
                    tdroundoff.Attributes.Add("colspan", "12");
                    tdtxtCommssnTotal.Attributes.Add("colspan", "12");
                    tr_maindiscountIn.Attributes.Add("colspan", "12");
                }
                else
                {
                    td_total.Attributes.Add("colspan", "13");
                    tdroundoff.Attributes.Add("colspan", "13");
                    tdtxtCommssnTotal.Attributes.Add("colspan", "13");
                    tr_maindiscountIn.Attributes.Add("colspan", "13");
                }
            }
        }

        #region  customerdrp

        protected void btnNewCustomer_Click(object sender, EventArgs e)
        {
            pnl_Customer.Visible = true;
            UC_Customer.PageLoad(drpagent.SelectedValue == "" ? 0 : Convert.ToInt32(drpagent.SelectedValue));
            Upd_Customer_Panel.Update();
        }

        public void fillnewcustomer(int res, string name, int? agentId)
        {
            Invoice.dtCustomername.Rows.Add(res, name);

            if (agentId > 0)
                Invoice.dtCustomernameAgent.Rows.Add(res, name);
        }
       
        //private const int ItemsPerRequest = 10;

        //[WebMethod]
        //public static RadComboBoxData GetCustomerNames(RadComboBoxContext context)
        //{
        //    DataTable data = GetData(context.Text);

        //    RadComboBoxData comboData = new RadComboBoxData();

        //    int itemOffset = context.NumberOfItems;
        //    int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
        //    comboData.EndOfItems = endOffset == data.Rows.Count;

        //    List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(endOffset - itemOffset);

        //    for (int i = itemOffset; i < endOffset; i++)
        //    {
        //        RadComboBoxItemData itemData = new RadComboBoxItemData();
        //        itemData.Text = data.Rows[i]["Text"].ToString();
        //        itemData.Value = data.Rows[i]["Value"].ToString();
        //        result.Add(itemData);
        //    }

        //    comboData.Items = result.ToArray();
        //    return comboData;
        //}

        //private static DataTable GetData(string text)
        //{
        //    DataTable dh = new DataTable();
        //    if (dtCustomernameAgent.Rows.Count > 0)
        //    {
        //        dh = dtCustomernameAgent.Clone();

        //        DataRow[] dr = dtCustomernameAgent.Select("Text LIKE '%" + text + "%'");
        //        int cv = dr.Length;

        //        if (cv > 0)
        //        {
        //            dh = dr.CopyToDataTable();
        //        }
        //    }
        //    else
        //    {
        //        dh = dtCustomername.Clone();

        //        DataRow[] dr = dtCustomername.Select("Text LIKE '%" + text + "%'");
        //        int cv = dr.Length;

        //        if (cv > 0)
        //        {
        //            dh = dr.CopyToDataTable();
        //        }
        //    }

        //    return dh;
        //}

        #endregion

        public string GetLanguage(int UserId)
        {
            DataTable dt = obj_trans.GetEmployeeLanguage(UserId);
            return dt.Rows[0][0].ToString();
        }

        public void BindData(int InvoiceId)
        {
            DataSet ds = obj_trans.Edit_Invoice(InvoiceId, Convert.ToInt32(hdnLanguage.Value), Convert.ToInt32(hdnSerPriceWTax.Value));
            DataTable dt1 = ds.Tables[0];/*invoic*/
            DataTable dt_ser = ds.Tables[1];/* Detail*/

            hdn_id.Value = dt1.Rows[0]["Id"].ToString();
            lbl_Code.Text = dt1.Rows[0]["Code"].ToString();
            job_date.DbSelectedDate = dt1.Rows[0]["Dated"].ToString();
            drpagent.SelectedValue = dt1.Rows[0]["agentId"].ToString();
            drp_customer.SelectedValue = dt1.Rows[0]["Customer_Id"].ToString();
            hdn_CurrentInvoiceReceivable.Value = dt1.Rows[0]["Receivable"].ToString();
            drp_customer_OnSelectedIndexChanged(null, null);
            txt_token.Enabled =   false;
            drp_customer.Enabled = drpagent.Enabled = false;
            fill_Edit_Quotation(drp_customer.SelectedValue == "" ? 0 : Convert.ToInt32(drp_customer.SelectedValue), InvoiceId);
            drp_quot.SelectedValue = dt1.Rows[0]["Quotation_id"].ToString();
            drp_quot.Enabled = drp_quot.SelectedValue == "" ? true : false;
            rbTaxInvoice.Checked = true;
            rbNormalInvoice.Checked = false;
            hdnreceived.Value = dt1.Rows[0]["Received"].ToString();
            drpInvoiceFormat.SelectedValue = dt1.Rows[0]["InvoiceFormat"].ToString();
            txtSubject.Text = dt1.Rows[0]["subject"].ToString();
            txtBillingname.Text = dt1.Rows[0]["BillingName"].ToString();
            drpinvoiceCreator.SelectedValue = dt1.Rows[0]["Updated_by"].ToString();
            hdnSerPriceWTax.Value = dt1.Rows[0]["IsSerPriceWithTax"].ToString();

            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                RadComboBoxItem item = (RadComboBoxItem)(drpTemplates.FindItemByValue(dr["TemplateId"].ToString()));
                item.Checked = true;
                item.Selected = true;
            }

            if (dt1.Rows[0]["InvoiceType"].ToString() == "1")
            {
                rbTaxInvoice.Checked = true;
                rbNormalInvoice.Checked = false;
            }
            else if (dt1.Rows[0]["InvoiceType"].ToString() == "2")
            {
                rbTaxInvoice.Checked = false;
                rbNormalInvoice.Checked = true;
            }
            if (hdn_shwdiscount.Value == "1")
            {
                txt_grand.Text = dt1.Rows[0]["AfterDiscount_GrandTotal"].ToString();
                txt_totDiscount.Text = dt1.Rows[0]["Total_Discount"].ToString();
            }
            else
                txt_grand.Text = dt1.Rows[0]["Grand_Total"].ToString();

            txt_remark.Text = dt1.Rows[0]["Remarks"].ToString();
            txtroundoff.Text = dt1.Rows[0]["RoundedOff"].ToString();
            decimal CommissionTotal = (dt1.Rows[0]["CommissionAmount"].ToString() == "" ? 0 : Convert.ToDecimal(dt1.Rows[0]["CommissionAmount"])) +
                (dt1.Rows[0]["AgentCommissiontotal"].ToString() == "" ? 0 : Convert.ToDecimal(dt1.Rows[0]["AgentCommissiontotal"]));
            txtCommssnTotal.Text = CommissionTotal.ToString();

            hdnInvoiceStatus.Value = dt1.Rows[0]["Status"].ToString();
            rpt_Item_list.DataSource = dt_ser;
            rpt_Item_list.DataBind();

            lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
            hdn_InvDetailId.Value = "-" + (rpt_Item_list.Items.Count + 1).ToString();

            btn_save.Visible = hdn_update.Value == "0" ? false : true;
            btn_save_print.Visible = hdn_update_N_print.Value == "0" ? false : true;
            btn_print.Visible = hdn_print.Value == "0" ? false : true;
            btnDuplicate.Visible = hdnduplicate.Value == "0" ? false : true;

            if (hdn_IsCredit.Value == "1" || hdnIsTaxprintall.Value=="1")
                btn_TaxInvoicePrint.Visible = hdn_TaxInvoicePrint.Value == "0" ? false : true;
            else
            {
                if ((dt1.Rows[0]["Received"].ToString() == "" ? "0" : dt1.Rows[0]["Received"].ToString()) ==
                    (dt1.Rows[0]["AfterDiscount_GrandTotal"].ToString() == "" ? "0" : dt1.Rows[0]["AfterDiscount_GrandTotal"].ToString()))
                    btn_TaxInvoicePrint.Visible = hdn_TaxInvoicePrint.Value == "0" ? false : true;
                else
                    btn_TaxInvoicePrint.Visible = false;

                //pnlbankcharge.Visible = true;
                //drpPayType.SelectedValue = dt1.Rows[0]["PaymentType"].ToString();
                //txtbankchargeper.Text= dt1.Rows[0]["ChargedPer"].ToString();
                //txtCharged.Text= dt1.Rows[0]["ChargedAmount"].ToString();
            }
            pnlbankcharge.Visible = true;
            drpPayType.SelectedValue = dt1.Rows[0]["PaymentType"].ToString();
            txtbankchargeper.Text = dt1.Rows[0]["ChargedPer"].ToString();
            txtCharged.Text = dt1.Rows[0]["ChargedAmount"].ToString();

            btn_cancel.Visible = hdn_cancel.Value == "0" ? false : true;
            btn_history.Visible = hdn_histry.Value == "0" ? false : true;
            btnSplitInvoice.Visible = hdnSplitInvoice.Value == "0" ? false : true;

            if (dt1.Rows[0]["Status"].ToString() == "2" || dt1.Rows[0]["Status"].ToString() == "3") // 2-cancel 3-delete
                btn_cancel.Visible = btn_save.Visible = btn_save_print.Visible = btnMakePay.Visible = btn_TaxInvoicePrint.Visible = false;

            if (dt1.Rows[0]["IsAllowEdit"].ToString() == "0")
                btn_save.Visible = btn_save_print.Visible = btn_TaxInvoicePrint.Visible = false;

            if (dt1.Rows[0]["IsAllowCancel"].ToString() == "0") // 2-cancel 3-delete ,creditnoted
                btn_cancel.Visible = false;

            Upd_Add_Panel.Update();
        }

        protected void txt_token_OnTextChanged(object sender, EventArgs e)
        {
            if (txt_token.Text != "")
            {
                //fill_Customer();
                DataTable dt1 = obj_trans.Get_Customerdetail(txt_token.Text);
                if (dt1.Rows.Count > 0)
                {
                    RadComboBoxItem CodeItem = new RadComboBoxItem();
                    CodeItem.Text = dt1.Rows[0]["Name"].ToString();
                    CodeItem.Value = dt1.Rows[0]["Customer_Id"].ToString();
                    drp_customer.Items.Insert(drp_customer.Items.Count, CodeItem);

                    drp_customer.SelectedValue = dt1.Rows[0]["Customer_id"].ToString();
                }
                drp_customer_OnSelectedIndexChanged(null, null);
                Upd_CustomerDrop_Panel.Update();
            }
        }

        protected void drp_agent_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpagent.SelectedValue != "")
            {
                drp_customer.Items.Clear();
                DataTable dt = obj_trans.Drp_Customer_FAgent(Convert.ToInt32(drpagent.SelectedValue));
                drp_customer.DataSource = dt;
                drp_customer.DataTextField = "Text";
                drp_customer.DataValueField = "Value";
                drp_customer.DataBind();
                drp_customer.Text = "";

                RadComboBoxItem CodeItem = new RadComboBoxItem();
                CodeItem.Text = "New Entry";
                CodeItem.Value = "0";
                drp_customer.Items.Insert(0, CodeItem);
            }
            else
            {
                fill_Customer();
            }
            Upd_CustomerDrop_Panel.Update();
        }

        protected void drp_customer_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            /*Change in here should be checked in Customer.ascx also*/
            pnl_CreditDetail.Visible = false;
            hdn_IsCredit.Value = hdnCustCommsnApplcable.Value = "0";
            lblCreditLimit.Text = "";
            lblCurrentCreditAmt.Text = "";
            btn_TaxInvoicePrint.Visible = false;

            drpPayType.SelectedValue = "1";
            txtbankchargeper.Text = txtCharged.Text = "";
            //pnlbankcharge.Visible = false;

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
                        UC_Customer.PageLoad(drpagent.SelectedValue == "" ? 0 : Convert.ToInt32(drpagent.SelectedValue));
                        Upd_Customer_Panel.Update();
                    }
                }
                else
                {
                    DataSet ds= obj_trans.Get_CustomerCreditDetail(Convert.ToInt32(drp_customer.SelectedValue));
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        hdnCustCommsnApplcable.Value = dt.Rows[0]["CommissionApplicable"].ToString();

                        decimal CurrentInvoiceCredit = hdn_CurrentInvoiceReceivable.Value == "" ? 0 : Convert.ToDecimal(hdn_CurrentInvoiceReceivable.Value);
                        decimal CurrentCredit = Convert.ToDecimal(dt.Rows[0]["Receivable"].ToString()) - CurrentInvoiceCredit;
                        hdn_IsCredit.Value = dt.Rows[0]["IsCredit"].ToString();
                        lblCreditLimit.Text = dt.Rows[0]["CreditAmount"].ToString();
                        lblCurrentCreditAmt.Text = CurrentCredit.ToString();
                        pnl_CreditDetail.Visible = hdn_IsCredit.Value == "1" ? true : false;
                        if (hdn_IsCredit.Value == "1" || hdnIsTaxprintall.Value == "1")
                            btn_TaxInvoicePrint.Visible = hdn_TaxInvoicePrint.Value == "0" ? false : true;
                        //else
                        //    pnlbankcharge.Visible = true;
                    }
                    DataTable dt2 = ds.Tables[1];
                    if (dt2.Rows.Count == 1)
                    {
                        drpagent.SelectedValue = dt2.Rows[0]["AgentId"].ToString();
                        Upd_agentDrop_Panel.Update();
                    }

                    fill_Quotation(Convert.ToInt32(drp_customer.SelectedValue));
                }
            }
            else
            {
                fill_Quotation(0);
            }
            Upd_btnTaxInvoicePrint.Update();
            Upd_CreditDetail_Panel.Update();
            updBankCharge.Update();
        }

        protected void drpPayTypeOnSelectedIndexChanged(object sender, EventArgs e)
        {
            txtbankchargeper.Text = txtCharged.Text = "";
            if (drpPayType.SelectedValue == "2")
            {
                txtbankchargeper.Text = hdnDefaultBankCharge.Value;
                if (txt_grand.Text != "")
                    txtCharged.Text = (Convert.ToDecimal(txt_grand.Text) * (Convert.ToDecimal(txtbankchargeper.Text) / 100)).ToString("0.00");
            }
            updBankCharge.Update();
        }

        public bool CheckCreditAmount()
        {
            bool ProceedSave;
            if (hdnisreceiptclick.Value == "1")
            {
                ProceedSave = true;
            }
            //else if (hdnIsTaxprintall.Value == "1")
            //{
            //    ProceedSave = true;
            //}
            else if (hdn_IsCredit.Value == "1")
            {
                decimal GrandTotal = Convert.ToDecimal(txt_grand.Text);
                decimal CurrentCredit = Convert.ToDecimal(lblCurrentCreditAmt.Text);
                decimal CreditLimit = Convert.ToDecimal(lblCreditLimit.Text);

                if ((GrandTotal + CurrentCredit) > CreditLimit)
                {
                    ProceedSave = false;
                    pnlAlert.Visible = true;
                    updAlert.Update();
                }
                else
                {
                    ProceedSave = true;
                }
            }
            else
            {
                ProceedSave = true;
            }
            return ProceedSave;
        }

        #region setcredit

        protected void btnNoOnClick(object sender, EventArgs e)
        {
            pnlAlert.Visible = false;
            updAlert.Update();
        }

        protected void btnYesOnClick(object sender, EventArgs e)
        {
            pnlAlert.Visible = false;
            updAlert.Update();
            txt_CreditAmount.Text = "";
            txt_CreditAmountLimit.Text = lblCreditLimit.Text;
            txt_CreditAmountCurrent.Text = lblCurrentCreditAmt.Text;
            pnlSetCredit.Visible = true;
            updSetCredit.Update();
        }

        protected void btnSetYesOnClick(object sender, EventArgs e)
        {
            int res = obj_mas.UpdateCustomerCredit(Convert.ToInt32(drp_customer.SelectedValue),
                Convert.ToDecimal(txt_CreditAmount.Text), Convert.ToInt32(hdn_user_id.Value));
            if (res > 0)
            {
                DataTable dt = obj_trans.Get_CustomerCreditDetail(Convert.ToInt32(drp_customer.SelectedValue)).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    lblCreditLimit.Text = dt.Rows[0]["CreditAmount"].ToString();
                    Upd_CreditDetail_Panel.Update();
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Credit Amount updated');", true);
            }
            pnlSetCredit.Visible = false;
            updSetCredit.Update();
        }

        protected void btnSetNoOnClick(object sender, EventArgs e)
        {
            pnlSetCredit.Visible = false;
            updSetCredit.Update();
        }

        #endregion

        public int SaveInvoiceWitDisc()
        {
            int res = 0;
            DataTable dt_deatils = fill_Detail_witdisc();
            if (dt_deatils.Rows.Count > 0)
            {
                int paytype = 1;
                decimal bankcharge = 0;
                decimal chargedamt = 0;
                //if (hdn_IsCredit.Value != "1" && drpPayType.SelectedValue == "2")
                //{
                //    paytype = 2;
                //    bankcharge = txtbankchargeper.Text == "" ? 0 : Convert.ToDecimal(txtbankchargeper.Text);
                //    chargedamt = txtCharged.Text == "" ? 0 : Convert.ToDecimal(txtCharged.Text);
                //}
                if (drpPayType.SelectedValue == "2")
                {
                    paytype = 2;
                    bankcharge = txtbankchargeper.Text == "" ? 0 : Convert.ToDecimal(txtbankchargeper.Text);
                    chargedamt = txtCharged.Text == "" ? 0 : Convert.ToDecimal(txtCharged.Text);
                }

                if (drpService.SelectedValue == "" && (txt_displayPrice.Text != "" || txt_Qty.Text != ""))
                {
                    InlineCalculation();
                }
                res = obj_trans.Insert_Update_InvoiceWitDisc(Convert.ToInt32(hdn_id.Value), job_date.SelectedDate,
                Convert.ToInt32(drp_customer.SelectedValue), txt_remark.Text, Convert.ToInt32(hdn_user_id.Value), 
                Convert.ToDecimal(txt_grand.Text),
                dt_deatils, drp_quot.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_quot.SelectedValue),
                txt_totDiscount.Text == "" ? 0 : Convert.ToDecimal(txt_totDiscount.Text), rbTaxInvoice.Checked == true ? 1 : 2,
                Convert.ToInt32(hdnTaxAppliedWithDiscount.Value), paytype, bankcharge, chargedamt,
                drpagent.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpagent.SelectedValue),
                Convert.ToInt32(drpInvoiceFormat.SelectedValue), txtroundoff.Text != "" ? Convert.ToDecimal(txtroundoff.Text) : 0,
                txtSubject.Text,txtBillingname.Text, Convert.ToInt32(drpinvoiceCreator.SelectedValue));
                if (txt_amtPayNow.Text != "" && res != 0)
                {
                    SaveReceipt(res);
                }
            }
            else
            {
                res = -2;
            }
            return res;
        }

        public DataTable fill_Detail_witdisc()
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
            dt_ser.Columns.Add("Discount", typeof(decimal));
            dt_ser.Columns.Add("Tax", typeof(decimal));
            dt_ser.Columns.Add("Deadline", typeof(DateTime));
            dt_ser.Columns.Add("TemplateId", typeof(int));
            dt_ser.Columns.Add("QuotationDetailId", typeof(int));
            dt_ser.Columns.Add("ServiceCommission", typeof(decimal));
            dt_ser.Columns.Add("CustomerStaff", typeof(string));
            dt_ser.Columns.Add("AgentCommission", typeof(decimal)); 

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
                    TextBox txtInvDdiscount = (TextBox)itm.FindControl("txtInvDdiscount");
                    HiddenField hdnInvDTax = (HiddenField)itm.FindControl("hdnInvDTax");
                    TextBox txtInvDExpense = (TextBox)itm.FindControl("txtInvDExpense");
                    RadDatePicker deadlineIn = (RadDatePicker)itm.FindControl("deadlineIn");
                    HiddenField hdnTemplateId = (HiddenField)itm.FindControl("hdnTemplateId");
                    HiddenField hdnQuotationDetailId = (HiddenField)itm.FindControl("hdnQuotationDetailId");
                    CheckBox chk_sel = (CheckBox)itm.FindControl("chk_sel");
                    TextBox txtInvDCommissionS = (TextBox)itm.FindControl("txtInvDCommissionS");
                    TextBox txtCustomerStaffIn = (TextBox)itm.FindControl("txtCustomerStaffIn");
                    TextBox txtAgentCommission = (TextBox)itm.FindControl("txtAgentCommission");

                    if (hdnIsQuotaionEditablePrime.Value == "1")
                    {
                        if (chk_sel.Checked)
                            dt_ser.Rows.Add(Convert.ToInt32(hdnInvDId.Value), hdnInvDCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDCategoryId.Value),
                    Convert.ToInt32(hdnInvDServiceId.Value), lblInvDdesc.Text, Convert.ToDecimal(hdnInvDPrice.Value), Convert.ToDecimal(txtInvDExpense.Text),//hdnInvDExpense.Value),
                    Convert.ToDecimal(hdnInvDServiceCharge.Value), txtInvDAddServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDAddServiceCharge.Text),
                    Convert.ToDecimal(txtInvDQty.Text), Convert.ToDecimal(txtInvDTaxAmount.Text),
                    Convert.ToDecimal(txtInvDPriceWitTax.Text), txtInvDFine.Text == "" ? 0 : Convert.ToDecimal(txtInvDFine.Text),
                    Convert.ToDecimal(txtInvDTotal.Text), txtInvDdiscount.Text == "" ? 0 : Convert.ToDecimal(txtInvDdiscount.Text),
                    Convert.ToDecimal(hdnInvDTax.Value), deadlineIn.SelectedDate,
                    hdnTemplateId.Value == "" ? (int?)null : Convert.ToInt32(hdnTemplateId.Value),
                    hdnQuotationDetailId.Value == "" ? (int?)null : Convert.ToInt32(hdnQuotationDetailId.Value),
                    txtInvDCommissionS.Text == "" ? 0 : Convert.ToDecimal(txtInvDCommissionS.Text),
                    txtCustomerStaffIn.Text, txtAgentCommission.Text == "" ? 0 : Convert.ToDecimal(txtAgentCommission.Text));
                    }
                    else
                        if (hdnInvDServiceId.Value != "")
                        dt_ser.Rows.Add(Convert.ToInt32(hdnInvDId.Value), hdnInvDCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDCategoryId.Value),
                  Convert.ToInt32(hdnInvDServiceId.Value), lblInvDdesc.Text, Convert.ToDecimal(hdnInvDPrice.Value), Convert.ToDecimal(txtInvDExpense.Text),//hdnInvDExpense.Value),
                  Convert.ToDecimal(hdnInvDServiceCharge.Value), txtInvDAddServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDAddServiceCharge.Text),
                  Convert.ToDecimal(txtInvDQty.Text), Convert.ToDecimal(txtInvDTaxAmount.Text),
                  Convert.ToDecimal(txtInvDPriceWitTax.Text), txtInvDFine.Text == "" ? 0 : Convert.ToDecimal(txtInvDFine.Text),
                  Convert.ToDecimal(txtInvDTotal.Text), txtInvDdiscount.Text == "" ? 0 : Convert.ToDecimal(txtInvDdiscount.Text),
                  Convert.ToDecimal(hdnInvDTax.Value), deadlineIn.SelectedDate,
                  hdnTemplateId.Value == "" ? (int?)null : Convert.ToInt32(hdnTemplateId.Value),
                  hdnQuotationDetailId.Value == "" ? (int?)null : Convert.ToInt32(hdnQuotationDetailId.Value),
                  txtInvDCommissionS.Text == "" ? 0 : Convert.ToDecimal(txtInvDCommissionS.Text), txtCustomerStaffIn.Text,
                  txtAgentCommission.Text == "" ? 0 : Convert.ToDecimal(txtAgentCommission.Text));
                }
            }
            if (drpService.SelectedValue != "" && txt_displayPrice.Text != "" && txt_Qty.Text != "")
            {
                dt_ser.Rows.Add(Convert.ToInt32(hdn_InvDetailId.Value), hdnSerCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnSerCategoryId.Value),
                    Convert.ToInt32(drpService.SelectedValue), txt_desc.Text, Convert.ToDecimal(hdnPrice.Value), Convert.ToDecimal(txtExpense.Text),//hdn_expn.Value),
                       Convert.ToDecimal(hdn_sc.Value), 0,
                       Convert.ToDecimal(txt_Qty.Text), Convert.ToDecimal(txt_taxamt.Text),
                       Convert.ToDecimal(txt_PriceWitTax.Text), txtFine.Text == "" ? 0 : Convert.ToDecimal(txtFine.Text),
                       Convert.ToDecimal(txt_totPrice.Text), txt_discount.Text == "" ? 0 : Convert.ToDecimal(txt_discount.Text),
                       Convert.ToDecimal(hdn_tax.Value), deadline.SelectedDate, (int?)null, (int?)null,
                       txtCommissionSOut.Text == "" ? 0 : Convert.ToDecimal(txtCommissionSOut.Text), txtCustomerStaffOut.Text,
                       txtAgentCommissionOut.Text == "" ? 0 : Convert.ToDecimal(txtAgentCommissionOut.Text));
            }
            return dt_ser;
        }

        #region ReceivedAmount check

        public int CheckReceivedAmount(int Action)
        {
            int ProceedSave;
            if (hdnaction.Value == "-1")
            {
                ProceedSave = 1;
            }
            else
            {
                if (Convert.ToDecimal(hdnreceived.Value) > Convert.ToDecimal(txt_grand.Text))
                {
                    ProceedSave = 0;
                    decimal advance = Convert.ToDecimal(hdnreceived.Value) - Convert.ToDecimal(txt_grand.Text);
                    lblAlertReceivedamt.Text = "Recieved amount: " + hdnreceived.Value + " is greater than updated invoice amount: " + txt_grand.Text + ". Extra paid amount: " + advance.ToString() + " will add to customer advance.And receipt update will block. Do you want to proceed ?";
                    hdnaction.Value = Action.ToString();
                    pnlAlertReceivedamt.Visible = true;
                    updAlertReceivedamt.Update();
                }
                else
                {
                    ProceedSave = 1;
                }
            }
            return ProceedSave;
        }

        protected void btnRANoOnClick(object sender, EventArgs e)
        {
            hdnaction.Value = "0";
            pnlAlertReceivedamt.Visible = false;
            updAlertReceivedamt.Update();
        }

        protected void btnRAYesOnClick(object sender, EventArgs e)
        {
            pnlAlertReceivedamt.Visible = false;

            if (hdnaction.Value == "1")
            {
                hdnaction.Value = "-1";
                updAlertReceivedamt.Update();

                btn_save_OnClick(null, null);
            }
            else if (hdnaction.Value == "2")
            {
                hdnaction.Value = "-1";
                updAlertReceivedamt.Update();

                btn_save_print_OnClick(null, null);
            }
            else if (hdnaction.Value == "3")
            {
                hdnaction.Value = "-1";
                updAlertReceivedamt.Update();

                btn_TaxInvoicePrint_OnClick(null, null);
            }

        }

        #endregion

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            int R = 1;
            if (hdn_id.Value != "0")
            {
                R = CheckReceivedAmount(1);
            }
            if (R == 1)
            {
                if (CheckCreditAmount())
                {
                    int res = 0;
                    if (hdnSCInInvoice.Value != "1")
                    {
                        res = SaveInvoiceWitDisc();
                    }
                    else
                    {
                        res = SaveInvoiceWitDiscSC();
                    }
                    if (res == -2)
                    {
                        lblAlertCommn.Text = "Add Service to continue !.";
                        pnlAlertCommn.Visible = true;
                        updAlertCommn.Update();
                    }
                   else if (res > 0)
                    {
                        if (drp_quot.SelectedValue != "")
                        {
                            int resin = obj_trans.updateleadstatus((int?)null, (int?)null, res, 3, Convert.ToInt32(hdn_user_id.Value));
                        }
                        if (hdnrequestId.Value != "" && hdnrequestId.Value != "0")
                        {
                            int crres = obj_trans.ProcessedCustomerRequest(Convert.ToInt32(hdnrequestId.Value), res, Convert.ToInt32(hdn_user_id.Value),
                                hdn_id.Value == "0" ? 0 : 1);
                        }
                        Clear();
                        lblAlertCommn.Text = "Saved Successfully !...";
                        pnlAlertCommn.Visible = true;
                        updAlertCommn.Update();
                    }
                    else
                    {
                        lblAlertCommn.Text = "Sorry Failed to Process Your Request !.";
                        pnlAlertCommn.Visible = true;
                        updAlertCommn.Update();
                    }
                }
            }
            Upd_Add_PanelInner.Update();
        }

        protected void btn_save_print_OnClick(object sender, EventArgs e)
        {
            int R = 1;
            if (hdn_id.Value != "0")
            {
                R = CheckReceivedAmount(2);
            }
            if (R == 1)
            {
                if (CheckCreditAmount())
                {
                    int res = 0;
                    if (hdnSCInInvoice.Value != "1")
                        res = SaveInvoiceWitDisc();
                    else
                        res = SaveInvoiceWitDiscSC();

                    if (res == -2)
                    {
                        lblAlertCommn.Text = "Add Service to continue !.";
                        pnlAlertCommn.Visible = true;
                        updAlertCommn.Update();
                    }
                    else if (res > 0)
                    {
                        if (drp_quot.SelectedValue != "")
                        {
                            int resin = obj_trans.updateleadstatus((int?)null, (int?)null, res, 3, Convert.ToInt32(hdn_user_id.Value));
                        }
                        if (hdnrequestId.Value != "0")
                        {
                            int crres = obj_trans.ProcessedCustomerRequest(Convert.ToInt32(hdnrequestId.Value), res, Convert.ToInt32(hdn_user_id.Value),
                                hdn_id.Value == "0" ? 0 : 1);
                        }
                        Clear();
                        lblAlertCommn.Text = "Saved Successfully !...";
                        pnlAlertCommn.Visible = true;
                        updAlertCommn.Update();

                        DataTable dt = obj_mas.Edit_GeneralSettings();
                        int Format = Convert.ToInt32(drpInvoiceFormat.SelectedValue);
                        string url = "";
                        if (dt.Rows[0]["SalesOrderPrint"].ToString() == "1")
                        {
                            if (Format == 1)
                                url = "../Reports/SalesOrderFormat1.aspx?id=" + res;
                            else if (Format == 2 || Format == 7)
                                url = "../Reports/SalesOrderFormat2.aspx?id=" + res;
                            else if (Format == 3)
                                url = "../Reports/SalesOrderFormat3.aspx?id=" + res;
                            else if (Format == 9)
                                url = "../Reports/SalesOrderFormat9.aspx?id=" + res;
                            else if (Format == 20)
                                url = "../Reports/SalesOrderFormat20.aspx?id=" + res;
                            else if (Format == 23)
                                url = "../Reports/SalesOrderFormat23.aspx?id=" + res;
                            else
                                url = "../Reports/SalesOrderPrint.aspx?id=" + res;

                        }
                        else
                        {
                            url = "../Reports/SalesorderPOS.aspx?id=" + res;
                        }
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
                    }
                    else
                    {
                        lblAlertCommn.Text = "Sorry Failed to Process Your Request !.";
                        pnlAlertCommn.Visible = true;
                        updAlertCommn.Update();
                    }
                }
            }
            Upd_Add_PanelInner.Update();
        }
        protected void btnAlertCloseOnClick(object sender, EventArgs e)
        {
            lblAlertCommn.Text = "";
            pnlAlertCommn.Visible = false;
            updAlertCommn.Update();
        }
        protected void btn_print_OnClick(object sender, EventArgs e)
        {
            DataTable dt = obj_mas.Edit_GeneralSettings();
            int Format = Convert.ToInt32(drpInvoiceFormat.SelectedValue);
            string url = "";
            if (dt.Rows[0]["SalesOrderPrint"].ToString() == "1")
            {
                if (Format == 1)
                    url = "../Reports/SalesOrderFormat1.aspx?id=" + Convert.ToInt32(hdn_id.Value);
                else if (Format == 2 || Format == 7)
                    url = "../Reports/SalesOrderFormat2.aspx?id=" + Convert.ToInt32(hdn_id.Value);
                else if (Format == 3)
                    url = "../Reports/SalesOrderFormat3.aspx?id=" + Convert.ToInt32(hdn_id.Value);
                else if (Format == 9)
                    url = "../Reports/SalesOrderFormat9.aspx?id=" + Convert.ToInt32(hdn_id.Value);
                else if (Format == 20)
                    url = "../Reports/SalesOrderFormat20.aspx?id=" + Convert.ToInt32(hdn_id.Value);
                else if (Format == 23)
                    url = "../Reports/SalesOrderFormat23.aspx?id=" + Convert.ToInt32(hdn_id.Value);
                else
                    url = "../Reports/SalesOrderPrint.aspx?id=" + Convert.ToInt32(hdn_id.Value);
            }
            else
            {
                url = "../Reports/SalesorderPOS.aspx?id=" + Convert.ToInt32(hdn_id.Value);
            }
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        protected void btn_TaxInvoicePrint_OnClick(object sender, EventArgs e)
        {
            int Format = Convert.ToInt32(drpInvoiceFormat.SelectedValue);
            string url = "";
            if (Format == 1)
                url = "../Reports/TaxInvoiceFormat1.aspx?id=";
            else if (Format == 2)
                url = "../Reports/TaxInvoiceFormat2.aspx?id=";
            else if (Format == 3)
                url = "../Reports/TaxInvoiceFormat3.aspx?id=";
            else if (Format == 5)
                url = "../Reports/TaxInvoiceFormat5.aspx?id=";
            else if (Format == 6)
                url = "../Reports/TaxInvoiceFormat6.aspx?id=";
            else if (Format == 7)
                url = "../Reports/TaxInvoiceFormat7.aspx?id=";
            else if (Format == 8)
                url = "../Reports/TaxInvoiceFormat8.aspx?id=";
            else if (Format == 9)
                url = "../Reports/TaxInvoiceFormat9.aspx?id=";
            else if (Format == 10)
                url = "../Reports/TaxInvoiceFormat10.aspx?id=";
            else if (Format == 11)
                url = "../Reports/TaxInvoiceFormat11.aspx?id=";
            else if (Format == 4)
                url = "../Reports/TaxInvoiceFormat4.aspx?id=";
            else if (Format == 12)
                url = "../Reports/TaxInvoiceFormat12.aspx?id=";
            else if (Format == 13)
                url = "../Reports/TaxInvoiceFormat13.aspx?id=";
            else if (Format == 14)
                url = "../Reports/TaxInvoiceFormat14.aspx?id=";
            else if (Format == 15)
                url = "../Reports/TaxInvoiceFormat15.aspx?id=";
            else if (Format == 16)
                url = "../Reports/TaxInvoiceFormat16.aspx?id=";
            else if (Format == 17)
                url = "../Reports/TaxInvoiceFormat17.aspx?id=";
            else if (Format == 18)
                url = "../Reports/TaxInvoiceFormat18.aspx?id=";
            else if (Format == 19)
                url = "../Reports/TaxInvoiceFormat19.aspx?id=";
            else if (Format == 20)
                url = "../Reports/TaxInvoiceFormat20.aspx?id=";
            else if (Format == 21)
                url = "../Reports/TaxInvoiceFormat21.aspx?id=";
            else if (Format == 22)
                url = "../Reports/TaxInvoiceFormat22.aspx?id=";
            else if (Format == 23)
                url = "../Reports/TaxInvoiceFormat23.aspx?id=";

            if (hdn_IsCredit.Value == "0" && hdnIsTaxprintall.Value=="0")
            {
                url = url + Convert.ToInt32(hdn_id.Value);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
            }
            else
            {
                if (hdnInvoiceStatus.Value == "0" && ((hdn_add.Value == "1" && hdn_id.Value == "0") || (hdn_update.Value == "1" && hdn_id.Value != "0")))
                {
                    int R = 1;
                    if (hdn_id.Value != "0")
                    {
                        R = CheckReceivedAmount(3);
                    }
                    if (R == 1)
                    {
                        if (CheckCreditAmount())
                        {
                            int res = 0;
                            if (hdnSCInInvoice.Value != "1")
                                res = SaveInvoiceWitDisc();
                            else
                                res = SaveInvoiceWitDiscSC();

                            if (res == -2)
                            {
                                lblAlertCommn.Text = "Add Service to continue !.";
                                pnlAlertCommn.Visible = true;
                                updAlertCommn.Update();
                            }
                            else if (res > 0)
                            {
                                if (drp_quot.SelectedValue != "")
                                {
                                    int resin = obj_trans.updateleadstatus((int?)null, (int?)null, res, 3, Convert.ToInt32(hdn_user_id.Value));
                                }
                                if (hdnrequestId.Value != "0")
                                {
                                    int crres = obj_trans.ProcessedCustomerRequest(Convert.ToInt32(hdnrequestId.Value), res, Convert.ToInt32(hdn_user_id.Value),
                                        hdn_id.Value == "0" ? 0 : 1);
                                }

                                Clear();
                                lblAlertCommn.Text = "Saved Successfully !...";
                                pnlAlertCommn.Visible = true;
                                updAlertCommn.Update();
                                url = url + res;
                                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
                            }
                            else
                            {
                                lbl_msgin.Text = "Sorry Failed to Process Your Request !..";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                            }
                        }
                    }
                    Upd_Add_PanelInner.Update();
                }
                else
                {
                    url = url + Convert.ToInt32(hdn_id.Value);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
                }
            }
        }

        protected void btnSplitInvoice_OnClick(object sender, EventArgs e)
        {
            DataTable dt_deatils = fillDetail_SI();

            int res = 0;
            if (dt_deatils.Rows.Count > 0)
            {
                int paytype = 1;
                decimal bankcharge = 0;
                decimal chargedamt = 0;
                //if (hdn_IsCredit.Value != "1" && drpPayType.SelectedValue == "2")
                //{
                //    paytype = 2;
                //    bankcharge = txtbankchargeper.Text == "" ? 0 : Convert.ToDecimal(txtbankchargeper.Text);
                //    chargedamt = txtCharged.Text == "" ? 0 : Convert.ToDecimal(txtCharged.Text);
                //}
                if (drpPayType.SelectedValue == "2")
                {
                    paytype = 2;
                    bankcharge = txtbankchargeper.Text == "" ? 0 : Convert.ToDecimal(txtbankchargeper.Text);
                    chargedamt = txtCharged.Text == "" ? 0 : Convert.ToDecimal(txtCharged.Text);
                }

                res = obj_trans.Insert_SplitInvoice(Convert.ToInt32(hdn_id.Value), DateTime.ParseExact(CalDate(job_date), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Convert.ToInt32(drp_customer.SelectedValue), txt_remark.Text, Convert.ToInt32(drpinvoiceCreator.SelectedValue),//Convert.ToInt32(hdn_user_id.Value),
                dt_deatils, drp_quot.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_quot.SelectedValue),
                rbTaxInvoice.Checked == true ? 1 : 2, Convert.ToInt32(hdnTaxAppliedWithDiscount.Value), paytype, bankcharge, chargedamt,
                drpagent.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpagent.SelectedValue),
                Convert.ToInt32(drpInvoiceFormat.SelectedValue));

                if (res > 0)
                {
                    Clear();
                    lbl_msgin.Text = "Saved Successfully !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Sorry Failed to Process Your Request !..');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Add Service to Continue !..');", true);
            }
        }

        public DataTable fillDetail_SI()
        {
            DataTable dt_ser = new DataTable();
            dt_ser.Columns.Add("D_id", typeof(int));

            if (rpt_Item_list.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_Item_list.Items)
                {
                    HiddenField hdnInvDId = (HiddenField)itm.FindControl("hdnInvDId");
                    CheckBox chksel = (CheckBox)itm.FindControl("chk_sel");
                    if (chksel.Checked)
                        dt_ser.Rows.Add(Convert.ToInt32(hdnInvDId.Value));
                }
            }
            return dt_ser;
        }

        #region Receipt

        protected void drp_payModeRec_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            pnl_PayMode_Panel.Visible = false;
            pnl_Cheque_Panel.Visible = false;
            cheque_date.DbSelectedDate = "";
            txt_chqNumber.Text = "";
            txtCommissionVat.Text = "";
            pnlCommissionVat.Visible = false;

            hdn_bankcommsn.Value =hdnisCommissionVat.Value= "0";
            txt_commsn.Text = txtadvance.Text = txtRecChargedAmt.Text = "";
            txtadvance.Visible = trRecChargedAmt.Visible = false;

            drpPettyCash.ClearSelection();
            drpPettyCash.Text = "";
            drpPettyCash.Items.Clear();
            drpPettyCash.Visible = false;

            drpBankAccount.Items.Clear();
            drpBankAccount.Visible = false;
            drpBankAccount.ClearSelection();
            drpBankAccount.Text = "";

            drpLoan.ClearSelection();
            drpLoan.Text = "";
            drpLoan.Items.Clear();
            drpLoan.Visible = false;

            pnl_Cheque_Panel.Visible = false;

            if (drp_payMode.SelectedValue == "1")/*PettyCash*/
            {
                drpPettyCash.DataSource = BalVoucher.GetPettyCashAccountList(Convert.ToInt32(hdn_user_id.Value));
                drpPettyCash.DataValueField = "Value";
                drpPettyCash.DataTextField = "Text";
                drpPettyCash.DataBind();
                drpPettyCash.Visible = true;
                drpPettyCash.ClearSelection();
                drpPettyCash.Text = "";
                if (drpPettyCash.Items.Count == 1)
                    drpPettyCash.SelectedValue = drpPettyCash.Items[0].Value;

                lblToLabel.Text = "Petty Cash Name / اسم المصروفات النثرية";
                lblToLabel.Visible = true;
                rqTo.ValidationGroup = "saverec";
                rqTo.ControlToValidate = "drpPettyCash";

                pnl_PayMode_Panel.Visible = true;
            }
            else if (drp_payMode.SelectedValue == "2" || drp_payMode.SelectedValue == "6")/*Bank Transfer*/
            {
                drpBankAccount.DataSource = BalVoucher.GetBankAccountList(Convert.ToInt32(hdn_user_id.Value));
                drpBankAccount.DataValueField = "Value";
                drpBankAccount.DataTextField = "Text";
                drpBankAccount.DataBind();
                drpBankAccount.Visible = true;

                lblToLabel.Text = "Bank Name / اسم البنك";
                lblToLabel.Visible = true;
                rqTo.ValidationGroup = "saverec";
                rqTo.ControlToValidate = "drpBankAccount";

                if (hdnpaymenttype.Value != "2" && drp_payMode.SelectedValue == "2")
                    trRecChargedAmt.Visible = true;
                if (drp_payMode.SelectedValue == "6")
                    pnlCommissionVat.Visible = true;

                pnl_PayMode_Panel.Visible = true;
            }
            else if (drp_payMode.SelectedValue == "3")/*Cheque*/
            {
                lblToLabel.Text = "Bank Name / اسم البنك";
                lblToLabel.Visible = false;
                rqTo.ValidationGroup = "no";
                rqTo.ControlToValidate = "drpBankAccount";

                pnl_PayMode_Panel.Visible = false;
                pnl_Cheque_Panel.Visible = true;
            }
            else if (drp_payMode.SelectedValue == "4")/*Advance*/
            {
                lblToLabel.Text = "Advance";
                lblToLabel.Visible = true;
                rqTo.ValidationGroup = "no";
                rqTo.ControlToValidate = "drpBankAccount";

                if (drp_customer.SelectedValue != "")
                {
                    txtadvance.Text = obj_mas.Edit_Customer(Convert.ToInt32(drp_customer.SelectedValue)).Tables[0].Rows[0]["Payable"].ToString();
                }
                txtadvance.Visible = true;

                pnl_PayMode_Panel.Visible = true;
            }
            else if (drp_payMode.SelectedValue == "5")/*Loan*/
            {
                drpLoan.DataSource = BalVoucher.GetLoan();
                drpLoan.DataValueField = "Value";
                drpLoan.DataTextField = "Text";
                drpLoan.DataBind();
                drpLoan.Visible = true;

                lblToLabel.Text = "Loan";
                lblToLabel.Visible = true;
                rqTo.ValidationGroup = "saverec";
                rqTo.ControlToValidate = "drpLoan";

                pnl_PayMode_Panel.Visible = true;
            }

            updreceiptIn.Update();
            Upd_PayMode_Panel.Update();
            upd_commsn.Update();
            Upd_Cheque_Panel.Update();
            updCommissionVat.Update();
        }

        protected void btnMakePay_OnClick(object sender, EventArgs e)
        {
            pnlreceipt.Visible = true;
            hdnisreceiptclick.Value = "0";
            DataTable dt = obj_common.Get_Code(18);
            if (dt.Rows.Count > 0)
                txtcode_Rec.Text = dt.Rows[0][0].ToString();
            txtInvCode_Rec.Text = lbl_Code.Text;
            ReceiptDate.SelectedDate = DateTime.Now;
            txtrecdiscount.Text = txt_totDiscount.Text;
            txtrectotal.Text = txt_grand.Text;
            txtrecRemark.Text =txtCommissionVat.Text= "";
            pnlCommissionVat.Visible = false;

            drp_payMode.SelectedValue = "1";
            drp_payMode.Enabled = true;
            txtChargedAmountRec.Text = hdnpaymenttype.Value = "";
            trChargedAmount.Visible = false;
            txt_amtPayNow.ReadOnly = false;

            if (hdn_id.Value == "0")
            {
                txt_amtPayNow.Text = txt_pendingAmt.Text = txtrectotal.Text;

                if (drpPayType.SelectedValue == "2" && txtCharged.Text != "")
                {
                    drp_payMode.SelectedValue = "2";
                    hdnpaymenttype.Value = "2";
                    drp_payModeRec_OnSelectedIndexChanged(null, null);
                    drp_payMode.Enabled = false;
                    trChargedAmount.Visible = true;
                    txtChargedAmountRec.Text = txtCharged.Text;
                    txt_amtPayNow.Text = (Convert.ToDecimal(txtrectotal.Text) + Convert.ToDecimal(txtCharged.Text)).ToString();
                    txt_amtPayNow.ReadOnly = true;
                }
            }
            else
            {
                DataSet ds = obj_trans.Get_Invoice(lbl_Code.Text, Convert.ToInt32(hdn_user_id.Value));
                DataTable dt1 = ds.Tables[0];/*invoic*/
                DataTable dt_ser = ds.Tables[1];/* Detail*/
                //txt_pendingAmt.Text = dt1.Rows[0]["PendingAmount"].ToString();
                //txt_amtPayNow.Text = dt1.Rows[0]["PendingAmount"].ToString();
                hdn_receivedAmt.Value = dt1.Rows[0]["Received"].ToString();

                txt_pendingAmt.Text = txt_amtPayNow.Text = (Convert.ToDecimal(txt_grand.Text) - Convert.ToDecimal(dt1.Rows[0]["Received"])).ToString();

                if (drpPayType.SelectedValue == "2" && txtCharged.Text != "")
                {
                    drp_payMode.SelectedValue = "2";
                    hdnpaymenttype.Value = "2";
                    drp_payModeRec_OnSelectedIndexChanged(null, null);
                    drp_payMode.Enabled = false;
                    trChargedAmount.Visible = true;
                    txtChargedAmountRec.Text = txtCharged.Text;
                    txt_amtPayNow.ReadOnly = true;
                }
            }
            upd_receipt.Update();
        }

        protected void btn_CloseReceipt_OnClick(object sender, EventArgs e)
        {
            pnlreceipt.Visible = false;
            ClearReceipt();
            upd_receipt.Update();
        }

        protected void btn_SaveReceipt_OnClick(object sender, EventArgs e)
        {
            hdnreceiptprint.Value = "0";
            hdnisreceiptclick.Value = "1";
            btn_save_OnClick(null, null);
            hdnisreceiptclick.Value = "0";
            pnlreceipt.Visible = false;
            upd_receipt.Update();
        }

        protected void btn_SavePrintReceipt_OnClick(object sender, EventArgs e)
        {

            hdnreceiptprint.Value = "1";
            hdnisreceiptclick.Value = "1";
            btn_save_OnClick(null, null);
            hdnisreceiptclick.Value = "0";
            pnlreceipt.Visible = false;
            upd_receipt.Update();

        }

        public void ClearReceipt()
        {
            hdn_bankcommsn.Value =hdnisCommissionVat.Value= "0";
            txt_commsn.Text = txtspotCommission.Text = "";
            hdn_receivedAmt.Value = "";
            txt_pendingAmt.Text =txtCommissionVat.Text= "";
            txt_amtPayNow.Text = "";
            txt_ReceivedAmt.Text = "";
            txt_Balance.Text = "";
            drp_payMode.ClearSelection();
            drp_payMode.Text = "";
            drp_payMode.SelectedValue = "1";
            drp_payModeRec_OnSelectedIndexChanged(null, null);
            drpBankAccount.ClearSelection();
            drpBankAccount.Text = "";
            cheque_date.DbSelectedDate = "";
            txt_chqNumber.Text = txtadvance.Text = "";
            pnlCommissionVat.Visible = false;

            upd_receipt.Update();
        }

        protected void onchangedrp_bank(object sender, EventArgs e)
        {
            hdn_bankcommsn.Value = hdnisCommissionVat.Value = "0";

            if (drpBankAccount.SelectedValue != "" && drp_payMode.SelectedValue == "6")// only for card swipe
            {
                DataTable dt = obj_mas.Edit_Bank_Account(Convert.ToInt32(drpBankAccount.SelectedValue));
                hdnisCommissionVat.Value = dt.Rows[0]["IsVatApplicable"].ToString();
                if (dt.Rows[0]["IsCommssionApp"].ToString() == "1" & dt.Rows[0]["CommissionPer"].ToString() != "")
                    hdn_bankcommsn.Value = dt.Rows[0]["CommissionPer"].ToString();
            }
            Upd_PayMode_Panel.Update();
            CalCommission();
        }

        public void CalCommission()
        {
            txt_commsn.Text = txtCommissionVat.Text = "";
            decimal commsn = 0, vat = 0;
            if (txt_amtPayNow.Text != "" & hdn_bankcommsn.Value != "0")
            {
                commsn = (Convert.ToDecimal(txt_amtPayNow.Text) * (Convert.ToDecimal(hdn_bankcommsn.Value) / 100));
                txt_commsn.Text = commsn.ToString("0.00");
            }
            vat = (commsn * Convert.ToDecimal(0.05));
            if (hdnisCommissionVat.Value == "1")
                txtCommissionVat.Text = vat.ToString("0.00");

            upd_commsn.Update();
            updCommissionVat.Update();
        }

        public void SaveReceipt(int invId)
        {
            if ((drp_payMode.SelectedValue == "4") && ((txtadvance.Text == "" ? 0 : Convert.ToDecimal(txtadvance.Text)) < Convert.ToDecimal(txt_amtPayNow.Text)))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Paid amount cannot be greater than advance amount.!');", true);
            }
            else
            {
                DataSet ds = obj_trans.Edit_Invoice(Convert.ToInt32(invId), Convert.ToInt32(hdnLanguage.Value), Convert.ToInt32(hdnSerPriceWTax.Value));
                DataTable dt1 = ds.Tables[0];/*invoic*/

                ds = obj_trans.Get_Invoice(dt1.Rows[0]["Code"].ToString(), Convert.ToInt32(hdn_user_id.Value));
                dt1 = ds.Tables[0];/*invoic*/
                DataTable dt_serInvd = ds.Tables[1];/* Detail*/

                DataTable dt_ser = new DataTable();
                dt_ser.Columns.Add("D_id", typeof(int));
                dt_ser.Columns.Add("CategoryId", typeof(int));
                dt_ser.Columns.Add("Service_Id", typeof(int));
                dt_ser.Columns.Add("Price", typeof(decimal));
                dt_ser.Columns.Add("Expense", typeof(decimal));
                dt_ser.Columns.Add("ServiceCharge", typeof(decimal));
                dt_ser.Columns.Add("Discount", typeof(decimal));
                dt_ser.Columns.Add("Quantity", typeof(decimal));
                dt_ser.Columns.Add("TaxAmount", typeof(decimal));
                dt_ser.Columns.Add("PriceWitTax", typeof(decimal));
                dt_ser.Columns.Add("Total", typeof(decimal));
                //dt_ser.Columns.Add("Deadline", typeof(DateTime));

                foreach (DataRow r in dt_serInvd.Rows)
                {
                    dt_ser.Rows.Add(Convert.ToInt32(r["D_id"]), r["CategoryId"].ToString() == "" ? (int?)null : Convert.ToInt32(r["CategoryId"].ToString()),
                        Convert.ToInt32(r["Service_Id"]), Convert.ToDecimal(r["Price"]), Convert.ToDecimal(r["Expense"]),
                             Convert.ToDecimal(r["ServiceCharge"]), r["Discount"].ToString() == "" ? (decimal?)null : Convert.ToDecimal(r["Discount"]),
                             Convert.ToDecimal(r["Quantity"]), Convert.ToDecimal(r["TaxAmount"]), Convert.ToDecimal(r["PriceWitTax"]),
                             Convert.ToDecimal(r["Total"]));
                }

                decimal paynow = Convert.ToDecimal(txt_amtPayNow.Text);
                decimal SpotCommission = txtspotCommission.Text == "" ? 0 : Convert.ToDecimal(txtspotCommission.Text);

                if (txtChargedAmountRec.Text != "" && Convert.ToDecimal(txtChargedAmountRec.Text) > 0 && dt1.Rows[0]["PaymentType"].ToString() == "2")
                {
                    paynow = Convert.ToDecimal(txt_pendingAmt.Text) - SpotCommission;
                }

                int res = obj_trans.Insert_Update_Receipt(0, ReceiptDate.SelectedDate,
                        invId, txtrecRemark.Text, txtrecdiscount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtrecdiscount.Text),
                        Convert.ToDecimal(txtrectotal.Text), paynow,// Convert.ToDecimal(txt_amtPayNow.Text),
                        Convert.ToInt32(drp_payMode.SelectedValue), drpBankAccount.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpBankAccount.SelectedValue),
                        drpPettyCash.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpPettyCash.SelectedValue),
                        drp_payMode.SelectedValue == "3" ? cheque_date.SelectedDate : (DateTime?)null,
                        drp_payMode.SelectedValue == "3" ? txt_chqNumber.Text : "",
                        Convert.ToDecimal(txt_pendingAmt.Text), paynow,// Convert.ToDecimal(txt_amtPayNow.Text),
                        0, dt_ser, Convert.ToInt32(hdn_user_id.Value), //Convert.ToInt32(drpinvoiceCreator.SelectedValue),
                        txt_commsn.Text == "" ? (decimal?)null : Convert.ToDecimal(txt_commsn.Text),
                         (txtChargedAmountRec.Text == "" ? 0 : Convert.ToDecimal(txtChargedAmountRec.Text)),
                          (txtRecChargedAmt.Text == "" ? (decimal?)null : Convert.ToDecimal(txtRecChargedAmt.Text)),
                          drpLoan.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpLoan.SelectedValue), SpotCommission,
                          txtCommissionVat.Text == "" ? 0 : Convert.ToDecimal(txtCommissionVat.Text)
                         );

                if (hdnreceiptprint.Value == "1")
                {
                    DataTable dt = obj_mas.Edit_GeneralSettings();
                    string url = "";

                    if (dt.Rows[0]["ReceiptFormat"].ToString() == "1")
                    {
                        url = "../Reports/CashReceiptFormat1.aspx?id=" + res;
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
                    }
                    else if (dt.Rows[0]["ReceiptFormat"].ToString() == "2")
                    {
                        //InchPrint(RId);
                        //InchPrint(RId);
                        url = "../Reports/CashReceiptPOS.aspx?id=" + res;
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
                    }
                    else if (dt.Rows[0]["ReceiptFormat"].ToString() == "3")
                    {
                        url = "../Reports/CashReceiptFormat2.aspx?id=" + res;
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
                    }
                    else if (dt.Rows[0]["ReceiptFormat"].ToString() == "4")
                    {
                        url = "../Reports/CashReceiptFormat3.aspx?id=" + res;
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
                    }
                    else if (dt.Rows[0]["ReceiptFormat"].ToString() == "5")
                    {
                        url = "../Reports/CashReceiptFormat5.aspx?id=" + res;
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
                    }
                    else if (dt.Rows[0]["ReceiptFormat"].ToString() == "6")
                    {
                        url = "../Reports/CashReceiptFormat6.aspx?id=" + res;
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
                    }
                }
            }
        }

        #region print

        public void InchPrint()
        {
            PrinterSettings settings = new PrinterSettings();
            string printname = settings.PrinterName;

            //DataSet ds = obj_report.CashReceiptPrint(ReceiptIdpub);
            //DataTable dt = ds.Tables[0];
            //DataTable dt_invD = ds.Tables[1];

            int servicelen = rpt_Item_list.Items.Count * 15;
            if (Application["PrintHeader"] != "")
            {
                servicelen = servicelen + 65;
            }

            PrintDocument doc = new PrintDocument();
            doc.PrinterSettings.PrinterName = printname;
            doc.DefaultPageSettings.PaperSize = new PaperSize("PaperA4", 300, 300 + servicelen);
            //doc.DocumentName = Server.MapPath("~") + "CashReceiptPrint.pdf";
            doc.PrintPage += new PrintPageEventHandler(PrintHandler);
            doc.Print();
        }
        private void PrintHandler(object sender, PrintPageEventArgs ppeArgs)
        {
            decimal Balance = Convert.ToDecimal(txt_pendingAmt.Text) - Convert.ToDecimal(txt_amtPayNow.Text);
            decimal PendingAmount = Convert.ToDecimal(txt_pendingAmt.Text);
            decimal ReceivedAmount = Convert.ToDecimal(txt_amtPayNow.Text);

            int servicelen = rpt_Item_list.Items.Count * 15;
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
            barImg.Code = lbl_Code.Text;
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

            g.DrawString(drp_customer.SelectedItem.Text, FontNormalBold, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
            g.DrawString(Convert.ToDateTime(ReceiptDate.SelectedDate).ToString("dd/MM/yyyy"), FontNormalBold, System.Drawing.Brushes.Black, 240, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 15;
            g.DrawString("Invoice No / ", FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
            g.DrawString("رقم الفاتورة", arbfnt, System.Drawing.Brushes.Black, 70, currentY, new System.Drawing.StringFormat());
            g.DrawString(" : " + lbl_Code.Text, FontNormal, System.Drawing.Brushes.Black, 125, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 15;
            g.DrawString("Receipt No / ", FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
            g.DrawString("رقم الايصال", arbfnt, System.Drawing.Brushes.Black, 70, currentY, new System.Drawing.StringFormat());
            g.DrawString(" : " + txtcode_Rec.Text, FontNormal, System.Drawing.Brushes.Black, 125, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 20;
            g.DrawString("Service / ", FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
            g.DrawString("الخدمات", arbfnt, System.Drawing.Brushes.Black, 55, currentY, new System.Drawing.StringFormat());

            foreach (RepeaterItem itm in rpt_Item_list.Items)
            {
                HiddenField hdnInvDServiceId = (HiddenField)itm.FindControl("hdnInvDServiceId");
                DataSet ds = obj_mas.Edit_Service(Convert.ToInt32(hdnInvDServiceId.Value));
                DataTable dt = ds.Tables[0];
                currentY = currentY + 15;

                g.DrawString(dt.Rows[0]["Name"].ToString(), FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
                if (dt.Rows[0]["NameInArabic"].ToString() != "")
                {
                    currentY = currentY + 10;
                    g.DrawString(dt.Rows[0]["NameInArabic"].ToString(), arbfnt, System.Drawing.Brushes.Black, 100, currentY, new System.Drawing.StringFormat());
                }
            }
            if (drpService.SelectedValue != "" && txt_displayPrice.Text != "" && txt_Qty.Text != "")
            {
                DataSet ds = obj_mas.Edit_Service(Convert.ToInt32(drpService.SelectedValue));
                DataTable dt = ds.Tables[0];
                currentY = currentY + 15;
                g.DrawString(dt.Rows[0]["Name"].ToString(), FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
                if (dt.Rows[0]["NameInArabic"].ToString() != "")
                {
                    currentY = currentY + 10;
                    g.DrawString(dt.Rows[0]["NameInArabic"].ToString(), arbfnt, System.Drawing.Brushes.Black, 100, currentY, new System.Drawing.StringFormat());
                }
            }

            currentY = currentY + 15;
            g.DrawString("Net Amount / ", FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
            g.DrawString("المبلغ الصافي", arbfnt, System.Drawing.Brushes.Black, 75, currentY, new System.Drawing.StringFormat());
            g.DrawString(" : " + PendingAmount.ToString(), FontNormal, System.Drawing.Brushes.Black, 135, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 15;
            g.DrawString("Paid Amount / ", FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
            g.DrawString("المبلغ المدفوع", arbfnt, System.Drawing.Brushes.Black, 75, currentY, new System.Drawing.StringFormat());
            g.DrawString(" : " + ReceivedAmount.ToString(), FontNormal, System.Drawing.Brushes.Black, 135, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 15;
            g.DrawString("Balance / ", FontNormal, System.Drawing.Brushes.Black, initlX, currentY, new System.Drawing.StringFormat());
            g.DrawString("الرصيد", arbfnt, System.Drawing.Brushes.Black, 55, currentY, new System.Drawing.StringFormat());
            g.DrawString(" : " + Balance.ToString(), FontNormal, System.Drawing.Brushes.Black, 135, currentY, new System.Drawing.StringFormat());
            currentY = currentY + 25;
            g.DrawString("* * *", FontNormal, System.Drawing.Brushes.Black, 140, currentY, new System.Drawing.StringFormat());

        }

        #endregion
        #endregion

        #region cancel

        protected void btn_Cancelmain_OnClick(object sender, EventArgs e)
        {
            pnl_cancl.Visible = true;
            txt_cancelremark.Text = "";

            DataTable dt = obj_trans.GetInvoiceCancelDetail(Convert.ToInt32(hdn_id.Value));
            rpt_cancelList.DataSource = dt;
            rpt_cancelList.DataBind();
            div_candet.Visible = dt.Rows.Count > 0 ? true : false;

            upd_cancl.Update();
        }

        protected void btn_cnclse_OnClick(object sender, EventArgs e)
        {
            pnl_cancl.Visible = false;
            txt_cancelremark.Text = "";
            upd_cancl.Update();
        }

        protected void btn_cancel_OnClick(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rpt_cancelList.Items)
            {
                CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    HiddenField hdndetId = (HiddenField)item.FindControl("hdndetId");
                    HiddenField hdn_type = (HiddenField)item.FindControl("hdn_type");

                    if (hdn_type.Value == "1")
                    {
                        int ress = obj_trans.DeleteServiceCompletion(Convert.ToInt32(hdndetId.Value), Convert.ToInt32(hdn_user_id.Value));
                    }
                    else if (hdn_type.Value == "2")
                    {
                        int ress = obj_trans.CancelDeleteReceipt(Convert.ToInt32(hdndetId.Value), 2, "Invoice Cancelled", Convert.ToInt32(hdn_user_id.Value));
                    }
                    else if (hdn_type.Value == "3")
                    {
                        DataTable dt_rv = obj_trans.get_receiptvoucherdet(Convert.ToInt32(hdndetId.Value));
                        if (dt_rv.Rows.Count > 0)
                        {
                            if (dt_rv.Rows[0][0].ToString() != "0")
                            {
                                int resf = BalVoucher.CancelDeleteReceiptVoucher(Convert.ToInt32(dt_rv.Rows[0][0].ToString()), 2, "Invoice Cancelled", Convert.ToInt32(hdn_user_id.Value));
                            }
                            else
                            {
                                int resf = obj_trans.CancelsingleReceiptVoucherentry(Convert.ToInt32(hdndetId.Value), Convert.ToInt32(hdn_user_id.Value));
                            }
                        }
                    }
                    else if (hdn_type.Value == "4")
                    {
                        DataTable dt_rv = obj_trans.getVendrBMdet(Convert.ToInt32(hdndetId.Value));
                        if (dt_rv.Rows.Count > 0)
                        {
                            if (dt_rv.Rows[0][0].ToString() != "0")
                            {
                                int resf = obj_trans.CancelVendorBalMap(Convert.ToInt32(dt_rv.Rows[0][0].ToString()),  Convert.ToInt32(hdn_user_id.Value));
                            }
                            else
                            {
                                int resf = obj_trans.CancelsingleVBMentry(Convert.ToInt32(hdndetId.Value), Convert.ToInt32(hdn_user_id.Value));
                            }
                        }
                    }
                }
            }

            int res = obj_trans.Cancel_Invoice(Convert.ToInt32(hdn_id.Value), txt_cancelremark.Text, Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                Clear();
                lbl_msg.Text = "Cancelled !..";

            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
            }
            pnl_cancl.Visible = false;
            txt_cancelremark.Text = "";
            upd_cancl.Update();
            Upd_Add_Panel.Update();
        }
        #endregion

        public void fill_Quotation(int CusId)
        {
            drp_quot.Items.Clear();
            DataTable dt = obj_trans.Drp_Quotation(CusId);
            drp_quot.DataSource = dt;
            drp_quot.DataTextField = "Text";
            drp_quot.DataValueField = "Value";
            drp_quot.DataBind();
            drp_quot.Text = "";

            UpdQuotationPanel.Update();

            //drp_quo_OnSelectedIndexChanged(null, null);
        }

        //Drop Down Quotation
        public void fill_Edit_Quotation(int CusId, int inv_id)
        {
            drp_quot.Items.Clear();
            DataTable dt = obj_trans.Drp_Quotation_Edit(CusId, inv_id);
            drp_quot.DataSource = dt;
            drp_quot.DataTextField = "Text";
            drp_quot.DataValueField = "Value";
            drp_quot.DataBind();
            drp_quot.Text = "";
        }

        protected void drp_quo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (drp_quot.SelectedValue != "")
            {
                if (hdnIsQuotaionEditable.Value == "0")
                {
                    trnewline.Visible = false;
                    hdnIsQuotaionEditablePrime.Value = "1";
                    Upd_total.Update();
                }

                int InvoiceType = rbTaxInvoice.Checked ? 1 : 2;
                DataTable dt = new DataTable();
                if (hdn_shwdiscount.Value != "1")
                    dt = obj_trans.GetQuotationDetails(drp_quot.SelectedValue == "" ? 0 : Convert.ToInt32(drp_quot.SelectedValue),
                        Convert.ToInt32(hdnLanguage.Value), Convert.ToInt32(hdnSerPriceWTax.Value), InvoiceType, drpagent.SelectedValue == "" ? 0 : Convert.ToInt32(drpagent.SelectedValue)); //pooja
                else
                    dt = obj_trans.GetQuotationDetails_invrecpt(drp_quot.SelectedValue == "" ? 0 : Convert.ToInt32(drp_quot.SelectedValue),
                        Convert.ToInt32(hdnLanguage.Value), Convert.ToInt32(hdnSerPriceWTax.Value), Convert.ToInt32(hdnTaxAppliedWithDiscount.Value),
                        InvoiceType, drpagent.SelectedValue == "" ? 0 : Convert.ToInt32(drpagent.SelectedValue)); //pooja
                rpt_Item_list.DataSource = dt;
                rpt_Item_list.DataBind();
                InlineCalculation();
                lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
                hdn_InvDetailId.Value = "-" + (rpt_Item_list.Items.Count + 1).ToString();
                Upd_Item_Panel.Update();
            }
            else
            {
                trnewline.Visible = true;
                hdnIsQuotaionEditablePrime.Value = "0";
                Upd_total.Update();

                rpt_Item_list.DataSource = null;
                rpt_Item_list.DataBind();
                InlineCalculation();
                lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
                hdn_InvDetailId.Value = "-" + (rpt_Item_list.Items.Count + 1).ToString();
                Upd_Item_Panel.Update();
            }

        }

        protected void drpTemplatesOnSelectedIndexChanged(object sender, EventArgs e)
        {
            int InvoiceType = rbTaxInvoice.Checked ? 1 : 2;
            DataTable dtTemplates = new DataTable();
            dtTemplates.Columns.Add("TemplatesId", typeof(int));
            foreach (RadComboBoxItem item in drpTemplates.Items)
            {
                if (item.Checked)
                    dtTemplates.Rows.Add(Convert.ToInt32(item.Value));
            }
            DataTable dt = new DataTable();
            if (hdn_shwdiscount.Value != "1")
                dt = obj_trans.GetServiceDetailsTemplate(dtTemplates, Convert.ToInt32(hdnLanguage.Value), Convert.ToInt32(hdnSerPriceWTax.Value), InvoiceType,
                     drp_customer.SelectedValue == "" ? 0 : Convert.ToInt32(drp_customer.SelectedValue), drpagent.SelectedValue == "" ? 0 : Convert.ToInt32(drpagent.SelectedValue));//pooja
            else
                dt = obj_trans.GetServiceDetailsTemplate_invrecpt(dtTemplates, Convert.ToInt32(hdnLanguage.Value),
                  Convert.ToInt32(hdnSerPriceWTax.Value), drp_customer.SelectedValue == "" ? 0 : Convert.ToInt32(drp_customer.SelectedValue), Convert.ToInt32(hdnTaxAppliedWithDiscount.Value), InvoiceType,
                   drpagent.SelectedValue == "" ? 0 : Convert.ToInt32(drpagent.SelectedValue));//pooja
            rpt_Item_list.DataSource = dt;
            rpt_Item_list.DataBind();
            InlineCalculation();
            lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
            hdn_InvDetailId.Value = "-" + (rpt_Item_list.Items.Count + 1).ToString();
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

            hdnDepartment.Value = "";
            hdnDepartmentId.Value = "";
            hdnSerCategory.Value = "";
            hdnSerCategoryId.Value = "";
            txtAgentCommissionOut.Text = ""; //pooja added
            hdnSerSubCategory.Value = "";
            hdnSerSubCategoryId.Value = "";
            txt_desc.Text = "";
            txt_displayPrice.Text = "";
            hdnPrice.Value = "";
            hdn_expn.Value = hdn_sc.Value = txtExpense.Text = "0";
            txtServiceCharge.Text = "";
            txt_Qty.Text = "";
            txt_taxamt.Text = "";
            hdn_tax.Value = "0";
            txtVatPer.Text = "";
            txt_PriceWitTax.Text = "";
            hdnFineApplicable.Value = "0";
            txtFine.Text = "";
            txt_totPrice.Text = "";
            txt_discount.Text = "";

            UpdTxtDescription.Update();
            UpdTxtPrice.Update();
            UpdTxtServiceCharge.Update();
            UpdTxtQty.Update();
            UpdTxtTaxAmt.Update();
            UpdTxtPriceWithTax.Update();
            UpdTxtFine.Update();
            UpdTxtTotPrice.Update();
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
            else if (contrlName == "drpSerCategory")
            {
                drpSerSubCategory.Text = "";
                drpService.Text = "";
                drpSerSubCategory.ClearSelection();
                drpService.ClearSelection();
            }
            else if (contrlName == "drpSerSubCategory")
            {
                drpService.Text = "";
                drpService.ClearSelection();
            }
            fill_FilterDropDown(1);
        }

        protected void drpFilter_OnSelectedIndexChangedIn(object sender, EventArgs e)
        {
            RadComboBox drp = (RadComboBox)sender;
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            RadComboBox drpServiceIn = (RadComboBox)itemrp.FindControl("drpServiceIn");
            RadComboBox drpDepartmentIn = (RadComboBox)itemrp.FindControl("drpDepartmentIn");
            RadComboBox drpSerCategoryIn = (RadComboBox)itemrp.FindControl("drpSerCategoryIn");
            RadComboBox drpSerSubCategoryIn = (RadComboBox)itemrp.FindControl("drpSerSubCategoryIn");

            HiddenField hdnInvDServiceId = (HiddenField)itemrp.FindControl("hdnInvDServiceId");
            HiddenField hdnInvDDepartmentId = (HiddenField)itemrp.FindControl("hdnInvDDepartmentId");
            HiddenField hdnInvDCategoryId = (HiddenField)itemrp.FindControl("hdnInvDCategoryId");
            HiddenField hdnInvDSerSubCategoryId = (HiddenField)itemrp.FindControl("hdnInvDSerSubCategoryId");

            String contrlName = drp.ID;// sendercontrol.ID;
            if (contrlName == "drpDepartmentIn")
            {
                drpSerCategoryIn.Text = "";
                hdnInvDCategoryId.Value = "";
                drpSerSubCategoryIn.Text = "";
                hdnInvDSerSubCategoryId.Value = "";
                drpServiceIn.Text = "";
                drpSerCategoryIn.ClearSelection();
                drpSerSubCategoryIn.ClearSelection();
                drpServiceIn.ClearSelection();
                hdnInvDServiceId.Value = "";
            }
            else if (contrlName == "drpSerCategoryIn")
            {
                drpSerSubCategoryIn.Text = "";
                drpServiceIn.Text = "";
                hdnInvDSerSubCategoryId.Value = "";
                drpSerSubCategoryIn.ClearSelection();
                drpServiceIn.ClearSelection();
                hdnInvDServiceId.Value = "";
            }
            else if (contrlName == "drpSerSubCategoryIn")
            {
                drpServiceIn.Text = "";
                drpServiceIn.ClearSelection();
                hdnInvDServiceId.Value = "";
            }

            {
                hdnInvDDepartmentId.Value = drpDepartmentIn.SelectedValue;
                hdnInvDCategoryId.Value = drpSerCategoryIn.SelectedValue;
                hdnInvDSerSubCategoryId.Value = drpSerSubCategoryIn.SelectedValue;

                DataSet ds = obj_trans.GetServiceFilter(1, drpDepartmentIn.SelectedValue == "" ? 0 : Convert.ToInt32(drpDepartmentIn.SelectedValue),
                     drpSerCategoryIn.SelectedValue == "" ? 0 : Convert.ToInt32(drpSerCategoryIn.SelectedValue),
                      drpSerSubCategoryIn.SelectedValue == "" ? 0 : Convert.ToInt32(drpSerSubCategoryIn.SelectedValue), 1);

                DataTable dtService = ds.Tables[3];
                if (contrlName == "drpSerCategoryIn")
                {
                    drpSerSubCategoryIn.DataSource = ds.Tables[2];
                    drpSerSubCategoryIn.DataValueField = "Value";
                    drpSerSubCategoryIn.DataTextField = "Text";
                    drpSerSubCategoryIn.DataBind();
                }

                drpSerCategoryIn.Visible = drpSerCategory.Visible;

                drpSerSubCategoryIn.Visible = drpSerSubCategory.Visible;

                drpServiceIn.DataSource = dtService;
                drpServiceIn.DataValueField = "Value";
                drpServiceIn.DataTextField = "Text";
                drpServiceIn.DataBind();

                TextBox txtInvDDisplayPrice = (TextBox)itemrp.FindControl("txtInvDDisplayPrice");
                HiddenField hdnInvDPrice = (HiddenField)itemrp.FindControl("hdnInvDPrice");
                HiddenField hdnInvDExpense = (HiddenField)itemrp.FindControl("hdnInvDExpense");
                HiddenField hdnInvDServiceCharge = (HiddenField)itemrp.FindControl("hdnInvDServiceCharge");
                TextBox txtInvDAddServiceCharge = (TextBox)itemrp.FindControl("txtInvDAddServiceCharge");
                TextBox txtInvDQty = (TextBox)itemrp.FindControl("txtInvDQty");
                TextBox txtInvDTaxAmount = (TextBox)itemrp.FindControl("txtInvDTaxAmount");
                HiddenField hdnInvDTax = (HiddenField)itemrp.FindControl("hdnInvDTax");
                TextBox txtInvDPriceWitTax = (TextBox)itemrp.FindControl("txtInvDPriceWitTax");
                HiddenField hdnInvDFineApplicable = (HiddenField)itemrp.FindControl("hdnInvDFineApplicable");
                TextBox txtInvDFine = (TextBox)itemrp.FindControl("txtInvDFine");
                TextBox txtInvDTotal = (TextBox)itemrp.FindControl("txtInvDTotal");
                TextBox txtInvDdiscount = (TextBox)itemrp.FindControl("txtInvDdiscount");
                TextBox txtInvDExpense = (TextBox)itemrp.FindControl("txtInvDExpense");
                TextBox txtInvDVatPer = (TextBox)itemrp.FindControl("txtInvDVatPer");
                TextBox txtInvDServiceCharge = (TextBox)itemrp.FindControl("txtInvDServiceCharge");

                txtInvDDisplayPrice.Text = "0";
                hdnInvDPrice.Value = "0";
                hdnInvDExpense.Value = hdnInvDServiceCharge.Value = txtInvDExpense.Text = txtInvDServiceCharge.Text = "0";
                txtInvDAddServiceCharge.Text = "0";
                txtInvDQty.Text = "1";
                txtInvDTaxAmount.Text = "0";
                hdnInvDTax.Value = "0";
                txtInvDVatPer.Text = "";
                txtInvDPriceWitTax.Text = "0";
                hdnInvDFineApplicable.Value = "0";
                txtInvDFine.Text = "0";
                txtInvDTotal.Text = "0";
                txtInvDdiscount.Text = "0";
                Upd_Item_Panel.Update();

                InlineCalculation();

            }
        }

        protected void drpService_OnSelectedIndexChangedIn(object sender, EventArgs e)
        {
            RadComboBox drp = (RadComboBox)sender;
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            RadComboBox drpServiceIn = (RadComboBox)itemrp.FindControl("drpServiceIn");
            RadComboBox drpDepartmentIn = (RadComboBox)itemrp.FindControl("drpDepartmentIn");
            RadComboBox drpSerCategoryIn = (RadComboBox)itemrp.FindControl("drpSerCategoryIn");
            RadComboBox drpSerSubCategoryIn = (RadComboBox)itemrp.FindControl("drpSerSubCategoryIn");

            HiddenField hdnInvDServiceId = (HiddenField)itemrp.FindControl("hdnInvDServiceId");
            HiddenField hdnInvDDepartmentId = (HiddenField)itemrp.FindControl("hdnInvDDepartmentId");
            HiddenField hdnInvDCategoryId = (HiddenField)itemrp.FindControl("hdnInvDCategoryId");
            HiddenField hdnInvDSerSubCategoryId = (HiddenField)itemrp.FindControl("hdnInvDSerSubCategoryId");

            HiddenField hdnInvDDepartment = (HiddenField)itemrp.FindControl("hdnInvDDepartment");
            HiddenField hdnInvDSerCategory = (HiddenField)itemrp.FindControl("hdnInvDSerCategory");
            HiddenField hdnInvDSerSubCategory = (HiddenField)itemrp.FindControl("hdnInvDSerSubCategory");

            TextBox txtInvDDisplayPrice = (TextBox)itemrp.FindControl("txtInvDDisplayPrice");
            HiddenField hdnInvDPrice = (HiddenField)itemrp.FindControl("hdnInvDPrice");
            HiddenField hdnInvDExpense = (HiddenField)itemrp.FindControl("hdnInvDExpense");
            HiddenField hdnInvDServiceCharge = (HiddenField)itemrp.FindControl("hdnInvDServiceCharge");
            TextBox txtInvDAddServiceCharge = (TextBox)itemrp.FindControl("txtInvDAddServiceCharge");
            TextBox txtInvDQty = (TextBox)itemrp.FindControl("txtInvDQty");
            TextBox txtInvDTaxAmount = (TextBox)itemrp.FindControl("txtInvDTaxAmount");
            HiddenField hdnInvDTax = (HiddenField)itemrp.FindControl("hdnInvDTax");
            TextBox txtInvDPriceWitTax = (TextBox)itemrp.FindControl("txtInvDPriceWitTax");
            HiddenField hdnInvDFineApplicable = (HiddenField)itemrp.FindControl("hdnInvDFineApplicable");
            TextBox txtInvDFine = (TextBox)itemrp.FindControl("txtInvDFine");
            TextBox txtInvDTotal = (TextBox)itemrp.FindControl("txtInvDTotal");
            TextBox txtInvDdiscount = (TextBox)itemrp.FindControl("txtInvDdiscount");
            TextBox txtInvDExpense = (TextBox)itemrp.FindControl("txtInvDExpense");
            TextBox txtInvDVatPer = (TextBox)itemrp.FindControl("txtInvDVatPer");
            TextBox txtInvDCommissionS = (TextBox)itemrp.FindControl("txtInvDCommissionS");
            TextBox txtInvDServiceCharge = (TextBox)itemrp.FindControl("txtInvDServiceCharge");
            TextBox txtAgentCommission = (TextBox)itemrp.FindControl("txtAgentCommission"); 

            txtInvDDisplayPrice.Text = "";
            hdnInvDPrice.Value = "";
            hdnInvDExpense.Value = hdnInvDServiceCharge.Value = txtInvDExpense.Text = txtInvDServiceCharge.Text = "0";
            txtInvDAddServiceCharge.Text = "0";
            txtInvDQty.Text = "1";
            txtInvDTaxAmount.Text = "";
            hdnInvDTax.Value = "0";
            txtInvDVatPer.Text = "";
            txtInvDPriceWitTax.Text = "";
            hdnInvDFineApplicable.Value = "0";
            txtInvDFine.Text = "";
            txtInvDTotal.Text = "";
            txtInvDdiscount.Text = txtInvDCommissionS.Text = "";
            txtAgentCommission.Text = "";//pooja added

            if (drp.SelectedValue != "")
            {
                int InvoiceType = rbTaxInvoice.Checked ? 1 : 2;
                DataTable Amount = new DataTable();
                if (hdn_shwdiscount.Value != "1")
                    Amount = obj_trans.Get_Services_Amount(Convert.ToInt32(drp.SelectedValue), drp_customer.SelectedValue == "" ? 0 :
                        Convert.ToInt32(drp_customer.SelectedValue), Convert.ToInt32(hdnLanguage.Value), Convert.ToInt32(hdnSerPriceWTax.Value), InvoiceType,
                         drpagent.SelectedValue == "" ? 0 : Convert.ToInt32(drpagent.SelectedValue));//pooja added
                else
                    Amount = obj_trans.Get_Services_Amount_invrecpt(Convert.ToInt32(drp.SelectedValue), 1, Convert.ToInt32(hdnLanguage.Value),
                      Convert.ToInt32(hdnSerPriceWTax.Value), drp_customer.SelectedValue == "" ? 0 : Convert.ToInt32(drp_customer.SelectedValue), Convert.ToInt32(hdnTaxAppliedWithDiscount.Value), InvoiceType,
                       drpagent.SelectedValue == "" ? 0 : Convert.ToInt32(drpagent.SelectedValue));//pooja added

                if (Amount.Rows.Count > 0)
                {
                    hdnInvDServiceId.Value = drp.SelectedValue;
                    txtInvDDisplayPrice.Text = Amount.Rows[0]["DisplayPrice"].ToString();
                    hdnInvDPrice.Value = Amount.Rows[0]["Price"].ToString();
                    txtInvDTaxAmount.Text = Amount.Rows[0]["TaxAmount"].ToString();
                    txtInvDPriceWitTax.Text = Amount.Rows[0]["PriceWitTax"].ToString();
                    txtInvDTotal.Text = Amount.Rows[0]["Total"].ToString();
                    hdnInvDFineApplicable.Value = Amount.Rows[0]["FineApplicable"].ToString();
                    txtInvDCommissionS.Text = Amount.Rows[0]["ServiceCommission"].ToString();
                    txtAgentCommission.Text = Amount.Rows[0]["AgentCommission"].ToString();//pooja added
                    if (hdn_shwdiscount.Value == "1")
                        txtInvDdiscount.Text = Amount.Rows[0]["Discount"].ToString();

                    hdnInvDExpense.Value = txtInvDExpense.Text = Amount.Rows[0]["Expense"].ToString();
                    txtInvDVatPer.Text = hdnInvDTax.Value = Amount.Rows[0]["Tax"].ToString();
                    txtInvDServiceCharge.Text = hdnInvDServiceCharge.Value = Amount.Rows[0]["ServiceCharge"].ToString();

                    hdnInvDDepartment.Value = Amount.Rows[0]["DepartmentName"].ToString();
                    hdnInvDDepartmentId.Value = Amount.Rows[0]["DepartmentId"].ToString();
                    hdnInvDSerCategory.Value = Amount.Rows[0]["SerCategoryName"].ToString();
                    hdnInvDCategoryId.Value = Amount.Rows[0]["ServiceCategoryId"].ToString();
                    hdnInvDSerSubCategory.Value = Amount.Rows[0]["SerSubCategoryName"].ToString();
                    hdnInvDSerSubCategoryId.Value = Amount.Rows[0]["ServiceSubCategoryId"].ToString();
                    drpDepartmentIn.SelectedValue = Amount.Rows[0]["DepartmentId"].ToString();
                    drpSerCategoryIn.SelectedValue = Amount.Rows[0]["ServiceCategoryId"].ToString();
                    drpSerSubCategoryIn.SelectedValue = Amount.Rows[0]["ServiceSubCategoryId"].ToString();
                }
            }

            Upd_Item_Panel.Update();

            InlineCalculation();
        }

        protected void drpService_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hdnDepartment.Value = "";
            hdnDepartmentId.Value = "";
            hdnSerCategory.Value = "";
            hdnSerCategoryId.Value = "";
            hdnSerSubCategory.Value = "";
            hdnSerSubCategoryId.Value = "";
            txtAgentCommissionOut.Text = "";

            //txt_desc.Text = "";
            txt_displayPrice.Text = "";
            hdnPrice.Value = "";
            hdn_expn.Value = hdn_sc.Value = txtExpense.Text = "0";
            txtServiceCharge.Text = "0";
            txt_Qty.Text = "";
            txt_taxamt.Text = "";
            hdn_tax.Value = "0";
            txtVatPer.Text = "";
            txt_PriceWitTax.Text = "";
            hdnFineApplicable.Value = "0";
            txtFine.Text = "";
            txt_totPrice.Text = txtCommissionSOut.Text = "";
            

            if (drpService.SelectedValue == "0")
            {
                UC_Service.UCPageLoad(2, 0);
                pnlServiceAdd.Visible = true;
                UpdServicepnlAdd.Update();
            }

            else if (drpService.SelectedValue != "")
            {
                int InvoiceType = rbTaxInvoice.Checked ? 1 : 2;
                DataTable Amount = new DataTable();
                if (hdn_shwdiscount.Value != "1")
                    Amount = obj_trans.Get_Services_Amount(Convert.ToInt32(drpService.SelectedValue), drp_customer.SelectedValue == "" ? 0 :
                        Convert.ToInt32(drp_customer.SelectedValue), Convert.ToInt32(hdnLanguage.Value), Convert.ToInt32(hdnSerPriceWTax.Value), InvoiceType,
                        drpagent.SelectedValue == "" ? 0 : Convert.ToInt32(drpagent.SelectedValue));//pooja added
                else
                    Amount = obj_trans.Get_Services_Amount_invrecpt(Convert.ToInt32(drpService.SelectedValue), 1, Convert.ToInt32(hdnLanguage.Value),
                      Convert.ToInt32(hdnSerPriceWTax.Value), drp_customer.SelectedValue == "" ? 0 : Convert.ToInt32(drp_customer.SelectedValue), Convert.ToInt32(hdnTaxAppliedWithDiscount.Value), InvoiceType,
                      drpagent.SelectedValue == "" ? 0 : Convert.ToInt32(drpagent.SelectedValue));//pooja added

                if (Amount.Rows.Count > 0)
                {
                    txt_displayPrice.Text = Amount.Rows[0]["DisplayPrice"].ToString();
                    hdnPrice.Value = Amount.Rows[0]["Price"].ToString();
                    txt_Qty.Text = "1";
                    txt_taxamt.Text = Amount.Rows[0]["TaxAmount"].ToString();
                    txt_PriceWitTax.Text = Amount.Rows[0]["PriceWitTax"].ToString();
                    txt_totPrice.Text = Amount.Rows[0]["Total"].ToString();
                    hdnFineApplicable.Value = Amount.Rows[0]["FineApplicable"].ToString();
                    txtCommissionSOut.Text = Amount.Rows[0]["ServiceCommission"].ToString();
                   
                    if (hdn_shwdiscount.Value == "1")
                        txt_discount.Text = Amount.Rows[0]["Discount"].ToString();

                    hdn_expn.Value = txtExpense.Text = Amount.Rows[0]["Expense"].ToString();
                    hdn_tax.Value = Amount.Rows[0]["Tax"].ToString();
                    txtVatPer.Text = Amount.Rows[0]["Tax"].ToString();
                    txtServiceCharge.Text = hdn_sc.Value = Amount.Rows[0]["ServiceCharge"].ToString();
                    hdnDepartment.Value = Amount.Rows[0]["DepartmentName"].ToString();
                    hdnDepartmentId.Value = Amount.Rows[0]["DepartmentId"].ToString();
                    hdnSerCategory.Value = Amount.Rows[0]["SerCategoryName"].ToString();
                    hdnSerCategoryId.Value = Amount.Rows[0]["ServiceCategoryId"].ToString();
                    hdnSerSubCategory.Value = Amount.Rows[0]["SerSubCategoryName"].ToString();
                    hdnSerSubCategoryId.Value = Amount.Rows[0]["ServiceSubCategoryId"].ToString();
                    drpDepartment.SelectedValue = Amount.Rows[0]["DepartmentId"].ToString();
                    drpSerCategory.SelectedValue = Amount.Rows[0]["ServiceCategoryId"].ToString();
                    drpSerSubCategory.SelectedValue = Amount.Rows[0]["ServiceSubCategoryId"].ToString();
                    txtAgentCommissionOut.Text = Amount.Rows[0]["AgentCommission"].ToString();//pooja added
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
            UpdExpense.Update();
            UpdTxtFine.Update();
            UpdTxtTotPrice.Update();
            Updtxt_discount.Update();
            UpdTxtTaxPer.Update();
            updCommissionSOut.Update();
            updAgentCommissionOut.Update();
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
            decimal Total_Amt = 0, TotDiscount = 0, totQty = 0, TotCommisn = 0;

            decimal tot = 0, totdis = 0, qty = 0, totcommsnag = 0, totcommsn = 0;

            foreach (RepeaterItem itm in rpt_Item_list.Items)
            {
                TextBox txtInvDTotal = (TextBox)itm.FindControl("txtInvDTotal");
                TextBox txtInvDdiscount = (TextBox)itm.FindControl("txtInvDdiscount");
                TextBox txtInvDQty = (TextBox)itm.FindControl("txtInvDQty");
                TextBox txtInvDCommissionS = (TextBox)itm.FindControl("txtInvDCommissionS");
                TextBox txtAgentCommission = (TextBox)itm.FindControl("txtAgentCommission");

                tot = txtInvDTotal.Text == "" ? 0 : Convert.ToDecimal(txtInvDTotal.Text);
                totdis = txtInvDdiscount.Text == "" ? 0 : Convert.ToDecimal(txtInvDdiscount.Text);
                totQty = txtInvDQty.Text == "" ? 0 : Convert.ToDecimal(txtInvDQty.Text);
                totcommsn = txtInvDCommissionS.Text == "" ? 0 : Convert.ToDecimal(txtInvDCommissionS.Text);
                totcommsnag = txtAgentCommission.Text == "" ? 0 : Convert.ToDecimal(txtAgentCommission.Text);

                Total_Amt += tot;
                TotDiscount = TotDiscount + Convert.ToDecimal(totQty * totdis);
                TotCommisn = TotCommisn + Convert.ToDecimal(totQty * totcommsn) + Convert.ToDecimal(totQty * totcommsnag);
            }
            if (drpService.SelectedValue != "" && txt_displayPrice.Text != "" && txt_Qty.Text != "")
            {
                Total_Amt += txt_totPrice.Text == "" ? 0 : Convert.ToDecimal(txt_totPrice.Text);
                qty = Convert.ToDecimal(txt_Qty.Text);
                TotDiscount += (qty * (txt_discount.Text == "" ? 0 : Convert.ToDecimal(txt_discount.Text)));
                //TotCommisn += (qty * (txtCommissionSOut.Text == "" ? 0 : Convert.ToDecimal(txtCommissionSOut.Text)));
                TotCommisn = TotCommisn+(qty * (txtCommissionSOut.Text == "" ? 0 : Convert.ToDecimal(txtCommissionSOut.Text)))
                    + (qty * (txtAgentCommissionOut.Text == "" ? 0 : Convert.ToDecimal(txtAgentCommissionOut.Text)));

            }

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

            txtroundoff.Text = (Convert.ToDecimal(Final) - Convert.ToDecimal(Total_Amt)).ToString("0.00");
            txt_grand.Text = (Convert.ToDecimal(Final)).ToString("0.00");

            if (drpPayType.SelectedValue == "2" && (txtbankchargeper.Text != "0" && txtbankchargeper.Text != ""))
            {
                txtCharged.Text = (Convert.ToDecimal(Final) * (Convert.ToDecimal(txtbankchargeper.Text) / 100)).ToString("#0.00");
                updBankCharge.Update();
            }

            txt_totDiscount.Text = (Convert.ToDecimal(TotDiscount)).ToString("0.00");
            txtCommssnTotal.Text = (Convert.ToDecimal(TotCommisn)).ToString("0.00");
            updCommssnTotal.Update();
            Updtxt_totDiscount.Update();
            Upd_Total_Panel.Update();
            updRoundoff.Update();
        }

        protected void rptitemlistDatabound(object sender, RepeaterItemEventArgs e)
        {
            if (hdn_shwdiscount.Value != "1")
            {
                var td = (HtmlTableCell)e.Item.FindControl("td_discount");
                td.Attributes.Add("style", "display:none");
            }
            if (hdnAgentCommmissionType.Value == "1")
            {
                var td = (HtmlTableCell)e.Item.FindControl("td_AgentCommission");
                td.Attributes.Add("style", "display:none");
            }
            Button btnCompleSC = (Button)e.Item.FindControl("btnCompleSC");

            if (hdnInvoiceStatus.Value == "2" || hdnInvoiceStatus.Value == "3")
                btnCompleSC.Visible = false;
            else
                btnCompleSC.Visible = hdnSCInInvoice.Value == "1" ? true : false;

            HiddenField hdndeadline = (HiddenField)e.Item.FindControl("hdndeadline");
            RadDatePicker deadlineIn = (RadDatePicker)e.Item.FindControl("deadlineIn");
            deadlineIn.SelectedDate = hdndeadline.Value == "" ? (DateTime?)null : Convert.ToDateTime(hdndeadline.Value);

            HiddenField hdnInvDDepartmentId = (HiddenField)e.Item.FindControl("hdnInvDDepartmentId");
            HiddenField hdnInvDCategoryId = (HiddenField)e.Item.FindControl("hdnInvDCategoryId");
            HiddenField hdnInvDSerSubCategoryId = (HiddenField)e.Item.FindControl("hdnInvDSerSubCategoryId");

            RadComboBox drpDepartmentIn = (RadComboBox)e.Item.FindControl("drpDepartmentIn");
            drpDepartmentIn.Items.Clear();
            drpDepartmentIn.DataSource = obj_mas.Drp_Department();
            drpDepartmentIn.DataValueField = "Value";
            drpDepartmentIn.DataTextField = "Text";
            drpDepartmentIn.DataBind();
            drpDepartmentIn.SelectedValue = hdnInvDDepartmentId.Value;
            drpDepartmentIn.Visible = drpDepartment.Visible;

            DataSet ds = obj_trans.GetServiceFilter(0, hdnInvDDepartmentId.Value == "" ? 0 : Convert.ToInt32(hdnInvDDepartmentId.Value),
                 hdnInvDCategoryId.Value == "" ? 0 : Convert.ToInt32(hdnInvDCategoryId.Value),
                  hdnInvDSerSubCategoryId.Value == "" ? 0 : Convert.ToInt32(hdnInvDSerSubCategoryId.Value), 1);
            DataTable dtSerCategory = ds.Tables[1];
            DataTable dtSerSubCategory = ds.Tables[2];
            DataTable dtService = ds.Tables[3];

            RadComboBox drpSerCategoryIn = (RadComboBox)e.Item.FindControl("drpSerCategoryIn");
            drpSerCategoryIn.Items.Clear();
            drpSerCategoryIn.DataSource = dtSerCategory;
            drpSerCategoryIn.DataValueField = "Value";
            drpSerCategoryIn.DataTextField = "Text";
            drpSerCategoryIn.DataBind();
            drpSerCategoryIn.SelectedValue = hdnInvDCategoryId.Value;
            drpSerCategoryIn.Visible = drpSerCategory.Visible;

            RadComboBox drpSerSubCategoryIn = (RadComboBox)e.Item.FindControl("drpSerSubCategoryIn");
            drpSerSubCategoryIn.Items.Clear();
            drpSerSubCategoryIn.DataSource = dtSerSubCategory;
            drpSerSubCategoryIn.DataValueField = "Value";
            drpSerSubCategoryIn.DataTextField = "Text";
            drpSerSubCategoryIn.DataBind();
            drpSerSubCategoryIn.SelectedValue = hdnInvDSerSubCategoryId.Value;
            drpSerSubCategoryIn.Visible = drpSerSubCategory.Visible;

            HiddenField hdnInvDServiceId = (HiddenField)e.Item.FindControl("hdnInvDServiceId");
            RadComboBox drpServiceIn = (RadComboBox)e.Item.FindControl("drpServiceIn");
            drpServiceIn.Items.Clear();
            drpServiceIn.DataSource = dtService;
            drpServiceIn.DataValueField = "Value";
            drpServiceIn.DataTextField = "Text";
            drpServiceIn.DataBind();
            drpServiceIn.SelectedValue = hdnInvDServiceId.Value;

            HiddenField hdnIsCompleted = (HiddenField)e.Item.FindControl("hdnIsCompleted");
            if (Convert.ToDecimal(hdnIsCompleted.Value) != 0)
            {
                Button btn_remove_line = (Button)e.Item.FindControl("btn_remove_line");
                btn_remove_line.Visible = btnCompleSC.Visible = false;
                drpDepartmentIn.Enabled = false;
                drpSerCategoryIn.Enabled = false;
                drpSerSubCategoryIn.Enabled = false;
                drpServiceIn.Enabled = false;
            }

        }

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
            dt_ser.Columns.Add("Discount", typeof(decimal));
            dt_ser.Columns.Add("Deadline", typeof(DateTime));
            dt_ser.Columns.Add("TemplateId", typeof(int));
            dt_ser.Columns.Add("QuotationDetailId", typeof(int));
            dt_ser.Columns.Add("CompletedQuantity", typeof(decimal));
            dt_ser.Columns.Add("ServiceCommission", typeof(decimal));
            dt_ser.Columns.Add("CustomerStaff", typeof(string));
            dt_ser.Columns.Add("AgentCommission", typeof(decimal));//pooja added

            dt_ser.Columns.Add("SerComDate", typeof(DateTime));
            dt_ser.Columns.Add("ExpQty", typeof(int));
            dt_ser.Columns.Add("ExpSinglAmt", typeof(decimal));
            dt_ser.Columns.Add("ExpTotAmt", typeof(decimal));
           

            DataTable dt_exp = new DataTable();
            dt_exp.Columns.Add("InvDId", typeof(int));
            dt_exp.Columns.Add("SerComDetailId", typeof(int));
            dt_exp.Columns.Add("ExpenseId", typeof(int));
            dt_exp.Columns.Add("Amount", typeof(decimal));
            dt_exp.Columns.Add("VAT", typeof(decimal));
            dt_exp.Columns.Add("VendorId", typeof(int));
            dt_exp.Columns.Add("PayModeId", typeof(int));
            dt_exp.Columns.Add("AccountId", typeof(int));
            dt_exp.Columns.Add("PayableAmount", typeof(decimal));
            dt_exp.Columns.Add("PaidAmount", typeof(decimal));

            DataTable dt_trans = new DataTable();
            dt_trans.Columns.Add("InvDId", typeof(int));
            dt_trans.Columns.Add("TransactionNumber", typeof(string));

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
                    RadDatePicker deadlineIn = (RadDatePicker)itm.FindControl("deadlineIn");
                    HiddenField hdnTemplateId = (HiddenField)itm.FindControl("hdnTemplateId");
                    HiddenField hdnQuotationDetailId = (HiddenField)itm.FindControl("hdnQuotationDetailId");
                    HiddenField hdnIsCompleted = (HiddenField)itm.FindControl("hdnIsCompleted");
                    TextBox txtInvDCommissionS = (TextBox)itm.FindControl("txtInvDCommissionS");
                    TextBox txtCustomerStaffIn = (TextBox)itm.FindControl("txtCustomerStaffIn");
                    TextBox txtAgentCommission = (TextBox)itm.FindControl("txtAgentCommission");

                    //sc
                    TextBox txtExpenseQty = (TextBox)itm.FindControl("txtExpenseQty");
                    TextBox txtExpenseSinglAmt = (TextBox)itm.FindControl("txtExpenseSinglAmt");
                    TextBox txtExpenseTotalAmt = (TextBox)itm.FindControl("txtExpenseTotalAmt");
                    RadDatePicker ExpenseSerComDate = (RadDatePicker)itm.FindControl("ExpenseSerComDate");

                    Repeater rptTransCode = (Repeater)itm.FindControl("rptTransCode");
                    Repeater rptexpensein = (Repeater)itm.FindControl("rptexpensein");

                    dt_ser.Rows.Add(Convert.ToInt32(hdnInvDId.Value), hdnInvDDepartmentId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDDepartmentId.Value),
                     hdnInvDCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDCategoryId.Value), hdnInvDSerSubCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDSerSubCategoryId.Value),
                    Convert.ToInt32(hdnInvDServiceId.Value), hdnInvDDepartment.Value, hdnInvDSerCategory.Value, hdnInvDSerSubCategory.Value,
                    lblServiceFullName.Text, lblInvDdesc.Text, Convert.ToDecimal(txtInvDDisplayPrice.Text), Convert.ToDecimal(hdnInvDPrice.Value),
                    Convert.ToDecimal(txtInvDExpense.Text),//hdnInvDExpense.Value),
                    Convert.ToDecimal(hdnInvDServiceCharge.Value), txtInvDAddServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDAddServiceCharge.Text),
                    Convert.ToDecimal(txtInvDQty.Text), Convert.ToDecimal(txtInvDTaxAmount.Text), Convert.ToDecimal(hdnInvDTax.Value),
                    Convert.ToDecimal(txtInvDPriceWitTax.Text), Convert.ToInt32(hdnInvDFineApplicable.Value), txtInvDFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDFine.Text),
                    Convert.ToDecimal(txtInvDTotal.Text), txtInvDdiscount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDdiscount.Text),
                    deadlineIn.SelectedDate, hdnTemplateId.Value == "" ? (int?)null : Convert.ToInt32(hdnTemplateId.Value),
                     hdnQuotationDetailId.Value == "" ? (int?)null : Convert.ToInt32(hdnQuotationDetailId.Value),
                     Convert.ToDecimal(hdnIsCompleted.Value), txtInvDCommissionS.Text == "" ? 0 : Convert.ToDecimal(txtInvDCommissionS.Text),
                     txtCustomerStaffIn.Text, txtAgentCommission.Text == "" ? 0 : Convert.ToDecimal(txtAgentCommission.Text),

                    ExpenseSerComDate.SelectedDate == null ? (DateTime?)null : DateTime.ParseExact(CalDate(ExpenseSerComDate), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                   txtExpenseQty.Text == "" ? (decimal?)null : Convert.ToDecimal(txtExpenseQty.Text),
                   txtExpenseSinglAmt.Text == "" ? (decimal?)null : Convert.ToDecimal(txtExpenseSinglAmt.Text),
                   txtExpenseTotalAmt.Text == "" ? (decimal?)null : Convert.ToDecimal(txtExpenseTotalAmt.Text));


                    foreach (RepeaterItem itms in rptTransCode.Items)
                    {
                        TextBox txtTransCode = (TextBox)itms.FindControl("txtTransCode");
                        dt_trans.Rows.Add(Convert.ToInt32(hdnInvDId.Value), txtTransCode.Text);
                    }

                    foreach (RepeaterItem itms in rptexpensein.Items)
                    {
                        TextBox txtSerComDetailId = (TextBox)itms.FindControl("txtSerComDetailId");
                        TextBox txtExpenseId = (TextBox)itms.FindControl("txtExpenseId");
                        TextBox txtAmount = (TextBox)itms.FindControl("txtAmount");
                        TextBox txtVAT = (TextBox)itms.FindControl("txtVAT");
                        TextBox txtVendorId = (TextBox)itms.FindControl("txtVendorId");
                        TextBox txtPayModeId = (TextBox)itms.FindControl("txtPayModeId");
                        TextBox txtAccountId = (TextBox)itms.FindControl("txtAccountId");
                        TextBox txtPayableAmount = (TextBox)itms.FindControl("txtPayableAmount");
                        TextBox txtPaidAmount = (TextBox)itms.FindControl("txtPaidAmount");

                        dt_exp.Rows.Add(Convert.ToInt32(hdnInvDId.Value),
                             txtSerComDetailId.Text == "" ? (int?)null : Convert.ToInt32(txtSerComDetailId.Text),
                             txtExpenseId.Text == "" ? (int?)null : Convert.ToInt32(txtExpenseId.Text),
                              txtAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtAmount.Text),
                              txtVAT.Text == "" ? (decimal?)null : Convert.ToDecimal(txtVAT.Text),
                              txtVendorId.Text == "" ? (int?)null : Convert.ToInt32(txtVendorId.Text),
                              txtPayModeId.Text == "" ? (int?)null : Convert.ToInt32(txtPayModeId.Text),
                              txtAccountId.Text == "" ? (int?)null : Convert.ToInt32(txtAccountId.Text),
                              txtPayableAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtPayableAmount.Text),
                              txtPaidAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtPaidAmount.Text));
                    }
                }
            }

            if (drpService.SelectedValue != "" && txt_displayPrice.Text != "" && txt_Qty.Text != "")
            {
                dt_ser.Rows.Add(Convert.ToInt32(hdn_InvDetailId.Value), hdnDepartmentId.Value == "" ? (int?)null : Convert.ToInt32(hdnDepartmentId.Value),
                    hdnSerCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnSerCategoryId.Value), hdnSerSubCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnSerSubCategoryId.Value), Convert.ToInt32(drpService.SelectedValue),
                    hdnDepartment.Value, hdnSerCategory.Value, hdnSerSubCategory.Value,
                    (hdnDepartment.Value + '/' + hdnSerCategory.Value + '/' + hdnSerSubCategory.Value + '/' + drpService.Text), txt_desc.Text, Convert.ToDecimal(txt_displayPrice.Text), Convert.ToDecimal(hdnPrice.Value), Convert.ToDecimal(txtExpense.Text),//hdn_expn.Value),
                       Convert.ToDecimal(hdn_sc.Value), 0, Convert.ToDecimal(txt_Qty.Text), Convert.ToDecimal(txt_taxamt.Text), Convert.ToDecimal(hdn_tax.Value),
                       Convert.ToDecimal(txt_PriceWitTax.Text), Convert.ToInt32(hdnFineApplicable.Value),
                       txtFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtFine.Text), Convert.ToDecimal(txt_totPrice.Text),
                       txt_discount.Text == "" ? (decimal?)null : Convert.ToDecimal(txt_discount.Text), deadline.SelectedDate, (int?)null, (int?)null, 0,
                      txtCommissionSOut.Text == "" ? 0 : Convert.ToDecimal(txtCommissionSOut.Text), txtCustomerStaffOut.Text,
                      txtAgentCommissionOut.Text == "" ? 0 : Convert.ToDecimal(txtAgentCommissionOut.Text),
                       (DateTime?)null);
            }
            rpt_Item_list.DataSource = dt_ser;
            rpt_Item_list.DataBind();

            //sc
            foreach (RepeaterItem itm in rpt_Item_list.Items)
            {
                Repeater rptTransCode = (Repeater)itm.FindControl("rptTransCode");
                Repeater rptexpensein = (Repeater)itm.FindControl("rptexpensein");
                HiddenField hdnInvDId = (HiddenField)itm.FindControl("hdnInvDId");

                DataTable dt_trans_in = new DataTable();
                dt_trans_in.Columns.Add("TransactionNumber", typeof(string));

                foreach (DataRow r in dt_trans.Rows)
                {
                    if (r["InvDId"].ToString() == hdnInvDId.Value)
                    {
                        dt_trans_in.Rows.Add(r["TransactionNumber"].ToString());
                    }
                }

                rptTransCode.DataSource = dt_trans_in;
                rptTransCode.DataBind();

                DataTable dt_expin = new DataTable();
                dt_expin.Columns.Add("InvDId", typeof(int));
                dt_expin.Columns.Add("SerComDetailId", typeof(int));
                dt_expin.Columns.Add("ExpenseId", typeof(int));
                dt_expin.Columns.Add("Amount", typeof(decimal));
                dt_expin.Columns.Add("VAT", typeof(decimal));
                dt_expin.Columns.Add("VendorId", typeof(int));
                dt_expin.Columns.Add("PayModeId", typeof(int));
                dt_expin.Columns.Add("AccountId", typeof(int));
                dt_expin.Columns.Add("PayableAmount", typeof(decimal));
                dt_expin.Columns.Add("PaidAmount", typeof(decimal));

                foreach (DataRow r in dt_exp.Rows)
                {
                    if (r["InvDId"].ToString() == hdnInvDId.Value)
                    {
                        dt_expin.Rows.Add(Convert.ToInt32(r["InvDId"]), Convert.ToInt32(r["SerComDetailId"]), Convert.ToInt32(r["ExpenseId"]),
                            Convert.ToDecimal(r["Amount"]), Convert.ToDecimal(r["VAT"]), Convert.ToInt32(r["VendorId"]), Convert.ToInt32(r["PayModeId"])
                            , Convert.ToInt32(r["AccountId"]), Convert.ToDecimal(r["PayableAmount"]), Convert.ToDecimal(r["PaidAmount"]));
                    }
                }

                rptexpensein.DataSource = dt_expin;
                rptexpensein.DataBind();
            }

            ClearServiceDetail();
            if (hdnDepartmentInInvoiceVisible.Value == "1")
                drpDepartment.Focus();
            else
                drpService.Focus();

            Upd_Item_Panel.Update();
        }

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
            dt_ser.Columns.Add("Discount", typeof(decimal));
            dt_ser.Columns.Add("Deadline", typeof(DateTime));
            dt_ser.Columns.Add("TemplateId", typeof(int));
            dt_ser.Columns.Add("QuotationDetailId", typeof(int));
            dt_ser.Columns.Add("CompletedQuantity", typeof(decimal));
            dt_ser.Columns.Add("ServiceCommission", typeof(decimal));
            dt_ser.Columns.Add("CustomerStaff", typeof(string));
            dt_ser.Columns.Add("AgentCommission", typeof(decimal));//pooja added

            //sc
            dt_ser.Columns.Add("ExpQty", typeof(int));
            dt_ser.Columns.Add("ExpSinglAmt", typeof(decimal));
            dt_ser.Columns.Add("ExpTotAmt", typeof(decimal));
            dt_ser.Columns.Add("SerComDate", typeof(DateTime));

            DataTable dt_exp = new DataTable();
            dt_exp.Columns.Add("InvDId", typeof(int));
            dt_exp.Columns.Add("SerComDetailId", typeof(int));
            dt_exp.Columns.Add("ExpenseId", typeof(int));
            dt_exp.Columns.Add("Amount", typeof(decimal));
            dt_exp.Columns.Add("VAT", typeof(decimal));
            dt_exp.Columns.Add("VendorId", typeof(int));
            dt_exp.Columns.Add("PayModeId", typeof(int));
            dt_exp.Columns.Add("AccountId", typeof(int));
            dt_exp.Columns.Add("PayableAmount", typeof(decimal));
            dt_exp.Columns.Add("PaidAmount", typeof(decimal));

            DataTable dt_trans = new DataTable();
            dt_trans.Columns.Add("InvDId", typeof(int));
            dt_trans.Columns.Add("TransactionNumber", typeof(string));

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
                    RadDatePicker deadlineIn = (RadDatePicker)itm.FindControl("deadlineIn");
                    HiddenField hdnTemplateId = (HiddenField)itm.FindControl("hdnTemplateId");
                    HiddenField hdnQuotationDetailId = (HiddenField)itm.FindControl("hdnQuotationDetailId");
                    HiddenField hdnIsCompleted = (HiddenField)itm.FindControl("hdnIsCompleted");
                    TextBox txtInvDCommissionS = (TextBox)itm.FindControl("txtInvDCommissionS");
                    TextBox txtCustomerStaffIn = (TextBox)itm.FindControl("txtCustomerStaffIn");
                    TextBox txtAgentCommission = (TextBox)itm.FindControl("txtAgentCommission");//pooja added
                    //sc
                    TextBox txtExpenseQty = (TextBox)itm.FindControl("txtExpenseQty");
                    TextBox txtExpenseSinglAmt = (TextBox)itm.FindControl("txtExpenseSinglAmt");
                    TextBox txtExpenseTotalAmt = (TextBox)itm.FindControl("txtExpenseTotalAmt");
                    RadDatePicker ExpenseSerComDate = (RadDatePicker)itm.FindControl("ExpenseSerComDate");

                    Repeater rptTransCode = (Repeater)itm.FindControl("rptTransCode");
                    Repeater rptexpensein = (Repeater)itm.FindControl("rptexpensein");

                    dt_ser.Rows.Add(Convert.ToInt32(hdnInvDId.Value), hdnInvDDepartmentId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDDepartmentId.Value),
                     hdnInvDCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDCategoryId.Value), hdnInvDSerSubCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDSerSubCategoryId.Value),
                    Convert.ToInt32(hdnInvDServiceId.Value), hdnInvDDepartment.Value, hdnInvDSerCategory.Value, hdnInvDSerSubCategory.Value,
                    lblServiceFullName.Text, lblInvDdesc.Text, Convert.ToDecimal(txtInvDDisplayPrice.Text),
                    Convert.ToDecimal(hdnInvDPrice.Value), Convert.ToDecimal(txtInvDExpense.Text),//hdnInvDExpense.Value),
                    Convert.ToDecimal(hdnInvDServiceCharge.Value), txtInvDAddServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDAddServiceCharge.Text),
                    Convert.ToDecimal(txtInvDQty.Text), Convert.ToDecimal(txtInvDTaxAmount.Text), Convert.ToDecimal(hdnInvDTax.Value),
                    Convert.ToDecimal(txtInvDPriceWitTax.Text), Convert.ToInt32(hdnInvDFineApplicable.Value),
                    txtInvDFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDFine.Text), Convert.ToDecimal(txtInvDTotal.Text),
                    txtInvDdiscount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDdiscount.Text), deadlineIn.SelectedDate,
                     hdnTemplateId.Value == "" ? (int?)null : Convert.ToInt32(hdnTemplateId.Value),
                     hdnQuotationDetailId.Value == "" ? (int?)null : Convert.ToInt32(hdnQuotationDetailId.Value),
                     Convert.ToDecimal(hdnIsCompleted.Value), txtInvDCommissionS.Text == "" ? 0 : Convert.ToDecimal(txtInvDCommissionS.Text),
                     txtCustomerStaffIn.Text, txtAgentCommission.Text == "" ? 0 : Convert.ToDecimal(txtAgentCommission.Text),

                     txtExpenseQty.Text == "" ? (decimal?)null : Convert.ToDecimal(txtExpenseQty.Text),
                   txtExpenseSinglAmt.Text == "" ? (decimal?)null : Convert.ToDecimal(txtExpenseSinglAmt.Text),
                   txtExpenseTotalAmt.Text == "" ? (decimal?)null : Convert.ToDecimal(txtExpenseTotalAmt.Text),
                   ExpenseSerComDate.SelectedDate == null ? (DateTime?)null : DateTime.ParseExact(CalDate(ExpenseSerComDate), "dd/MM/yyyy", CultureInfo.InvariantCulture));


                    foreach (RepeaterItem itms in rptTransCode.Items)
                    {
                        TextBox txtTransCode = (TextBox)itms.FindControl("txtTransCode");
                        dt_trans.Rows.Add(Convert.ToInt32(hdnInvDId.Value), txtTransCode.Text);
                    }

                    foreach (RepeaterItem itms in rptexpensein.Items)
                    {
                        TextBox txtSerComDetailId = (TextBox)itms.FindControl("txtSerComDetailId");
                        TextBox txtExpenseId = (TextBox)itms.FindControl("txtExpenseId");
                        TextBox txtAmount = (TextBox)itms.FindControl("txtAmount");
                        TextBox txtVAT = (TextBox)itms.FindControl("txtVAT");
                        TextBox txtVendorId = (TextBox)itms.FindControl("txtVendorId");
                        TextBox txtPayModeId = (TextBox)itms.FindControl("txtPayModeId");
                        TextBox txtAccountId = (TextBox)itms.FindControl("txtAccountId");
                        TextBox txtPayableAmount = (TextBox)itms.FindControl("txtPayableAmount");
                        TextBox txtPaidAmount = (TextBox)itms.FindControl("txtPaidAmount");

                        dt_exp.Rows.Add(Convert.ToInt32(hdnInvDId.Value),
                             txtSerComDetailId.Text == "" ? (int?)null : Convert.ToInt32(txtSerComDetailId.Text),
                             txtExpenseId.Text == "" ? (int?)null : Convert.ToInt32(txtExpenseId.Text),
                              txtAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtAmount.Text),
                              txtVAT.Text == "" ? (decimal?)null : Convert.ToDecimal(txtVAT.Text),
                              txtVendorId.Text == "" ? (int?)null : Convert.ToInt32(txtVendorId.Text),
                              txtPayModeId.Text == "" ? (int?)null : Convert.ToInt32(txtPayModeId.Text),
                              txtAccountId.Text == "" ? (int?)null : Convert.ToInt32(txtAccountId.Text),
                              txtPayableAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtPayableAmount.Text),
                              txtPaidAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtPaidAmount.Text));
                    }

                }
            }

            dt_ser.Rows.RemoveAt(itemrp.ItemIndex);
            rpt_Item_list.DataSource = dt_ser;
            rpt_Item_list.DataBind();

            //sc
            foreach (RepeaterItem itm in rpt_Item_list.Items)
            {
                Repeater rptTransCode = (Repeater)itm.FindControl("rptTransCode");
                Repeater rptexpensein = (Repeater)itm.FindControl("rptexpensein");
                HiddenField hdnInvDId = (HiddenField)itm.FindControl("hdnInvDId");

                DataTable dt_trans_in = new DataTable();
                dt_trans_in.Columns.Add("TransactionNumber", typeof(string));

                foreach (DataRow r in dt_trans.Rows)
                {
                    if (r["InvDId"].ToString() == hdnInvDId.Value)
                    {
                        dt_trans_in.Rows.Add(r["TransactionNumber"].ToString());
                    }
                }

                rptTransCode.DataSource = dt_trans_in;
                rptTransCode.DataBind();

                DataTable dt_expin = new DataTable();
                dt_expin.Columns.Add("InvDId", typeof(int));
                dt_expin.Columns.Add("SerComDetailId", typeof(int));
                dt_expin.Columns.Add("ExpenseId", typeof(int));
                dt_expin.Columns.Add("Amount", typeof(decimal));
                dt_expin.Columns.Add("VAT", typeof(decimal));
                dt_expin.Columns.Add("VendorId", typeof(int));
                dt_expin.Columns.Add("PayModeId", typeof(int));
                dt_expin.Columns.Add("AccountId", typeof(int));
                dt_expin.Columns.Add("PayableAmount", typeof(decimal));
                dt_expin.Columns.Add("PaidAmount", typeof(decimal));

                foreach (DataRow r in dt_exp.Rows)
                {
                    if (r["InvDId"].ToString() == hdnInvDId.Value)
                    {
                        dt_expin.Rows.Add(Convert.ToInt32(r["InvDId"]), Convert.ToInt32(r["SerComDetailId"]), Convert.ToInt32(r["ExpenseId"]),
                            Convert.ToDecimal(r["Amount"]), Convert.ToDecimal(r["VAT"]), Convert.ToInt32(r["VendorId"]), Convert.ToInt32(r["PayModeId"])
                            , Convert.ToInt32(r["AccountId"]), Convert.ToDecimal(r["PayableAmount"]), Convert.ToDecimal(r["PaidAmount"]));
                    }
                }

                rptexpensein.DataSource = dt_expin;
                rptexpensein.DataBind();
            }


            lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
            InlineCalculation();
            //Upd_InvoiceDetail_Panel.Update();
            Upd_Item_Panel.Update();
        }


        protected void btnDuplicateInvoice_OnClick(object sender, EventArgs e)
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
            dt_ser.Columns.Add("Discount", typeof(decimal));
            dt_ser.Columns.Add("Deadline", typeof(DateTime));
            dt_ser.Columns.Add("TemplateId", typeof(int));
            dt_ser.Columns.Add("QuotationDetailId", typeof(int));
            dt_ser.Columns.Add("CompletedQuantity", typeof(decimal));
            dt_ser.Columns.Add("ServiceCommission", typeof(decimal));
            dt_ser.Columns.Add("CustomerStaff", typeof(string));
            dt_ser.Columns.Add("AgentCommission", typeof(decimal));

            dt_ser.Columns.Add("SerComDate", typeof(DateTime));
            dt_ser.Columns.Add("ExpQty", typeof(int));
            dt_ser.Columns.Add("ExpSinglAmt", typeof(decimal));
            dt_ser.Columns.Add("ExpTotAmt", typeof(decimal));


            int i = 0;
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
                RadDatePicker deadlineIn = (RadDatePicker)itm.FindControl("deadlineIn");
                HiddenField hdnTemplateId = (HiddenField)itm.FindControl("hdnTemplateId");
                HiddenField hdnQuotationDetailId = (HiddenField)itm.FindControl("hdnQuotationDetailId");
                HiddenField hdnIsCompleted = (HiddenField)itm.FindControl("hdnIsCompleted");
                TextBox txtInvDCommissionS = (TextBox)itm.FindControl("txtInvDCommissionS");
                TextBox txtCustomerStaffIn = (TextBox)itm.FindControl("txtCustomerStaffIn");
                TextBox txtAgentCommission = (TextBox)itm.FindControl("txtAgentCommission");

                //sc
                TextBox txtExpenseQty = (TextBox)itm.FindControl("txtExpenseQty");
                TextBox txtExpenseSinglAmt = (TextBox)itm.FindControl("txtExpenseSinglAmt");
                TextBox txtExpenseTotalAmt = (TextBox)itm.FindControl("txtExpenseTotalAmt");
                RadDatePicker ExpenseSerComDate = (RadDatePicker)itm.FindControl("ExpenseSerComDate");

                Repeater rptTransCode = (Repeater)itm.FindControl("rptTransCode");
                Repeater rptexpensein = (Repeater)itm.FindControl("rptexpensein");

                dt_ser.Rows.Add(--i, hdnInvDDepartmentId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDDepartmentId.Value),
                 hdnInvDCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDCategoryId.Value), hdnInvDSerSubCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDSerSubCategoryId.Value),
                Convert.ToInt32(hdnInvDServiceId.Value), hdnInvDDepartment.Value, hdnInvDSerCategory.Value, hdnInvDSerSubCategory.Value,
                lblServiceFullName.Text, lblInvDdesc.Text, Convert.ToDecimal(txtInvDDisplayPrice.Text), Convert.ToDecimal(hdnInvDPrice.Value),
                Convert.ToDecimal(txtInvDExpense.Text),//hdnInvDExpense.Value),
                Convert.ToDecimal(hdnInvDServiceCharge.Value), txtInvDAddServiceCharge.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDAddServiceCharge.Text),
                Convert.ToDecimal(txtInvDQty.Text), Convert.ToDecimal(txtInvDTaxAmount.Text), Convert.ToDecimal(hdnInvDTax.Value),
                Convert.ToDecimal(txtInvDPriceWitTax.Text), Convert.ToInt32(hdnInvDFineApplicable.Value),
                txtInvDFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDFine.Text),
                Convert.ToDecimal(txtInvDTotal.Text), txtInvDdiscount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtInvDdiscount.Text),
                deadlineIn.SelectedDate, hdnTemplateId.Value == "" ? (int?)null : Convert.ToInt32(hdnTemplateId.Value),
                 (int?)null,
                0, txtInvDCommissionS.Text == "" ? 0 : Convert.ToDecimal(txtInvDCommissionS.Text),
                 txtCustomerStaffIn.Text, txtAgentCommission.Text == "" ? 0 : Convert.ToDecimal(txtAgentCommission.Text),

                (DateTime?)null);

            }

            if (drpService.SelectedValue != "" && txt_displayPrice.Text != "" && txt_Qty.Text != "")
            {
                dt_ser.Rows.Add(--i, hdnDepartmentId.Value == "" ? (int?)null : Convert.ToInt32(hdnDepartmentId.Value),
                    hdnSerCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnSerCategoryId.Value), hdnSerSubCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnSerSubCategoryId.Value), Convert.ToInt32(drpService.SelectedValue),
                    hdnDepartment.Value, hdnSerCategory.Value, hdnSerSubCategory.Value,
                    (hdnDepartment.Value + '/' + hdnSerCategory.Value + '/' + hdnSerSubCategory.Value + '/' + drpService.Text), txt_desc.Text, Convert.ToDecimal(txt_displayPrice.Text), Convert.ToDecimal(hdnPrice.Value), Convert.ToDecimal(txtExpense.Text),//hdn_expn.Value),
                       Convert.ToDecimal(hdn_sc.Value), 0, Convert.ToDecimal(txt_Qty.Text), Convert.ToDecimal(txt_taxamt.Text), Convert.ToDecimal(hdn_tax.Value),
                       Convert.ToDecimal(txt_PriceWitTax.Text), Convert.ToInt32(hdnFineApplicable.Value),
                       txtFine.Text == "" ? (decimal?)null : Convert.ToDecimal(txtFine.Text), Convert.ToDecimal(txt_totPrice.Text),
                       txt_discount.Text == "" ? (decimal?)null : Convert.ToDecimal(txt_discount.Text), deadline.SelectedDate, (int?)null, (int?)null, 0,
                      txtCommissionSOut.Text == "" ? 0 : Convert.ToDecimal(txtCommissionSOut.Text), txtCustomerStaffOut.Text,
                      txtAgentCommissionOut.Text == "" ? 0 : Convert.ToDecimal(txtAgentCommissionOut.Text),

                      (DateTime?)null);
            }

            Clear();
            hdn_InvDetailId.Value = (--i).ToString();
            rpt_Item_list.DataSource = dt_ser;
            rpt_Item_list.DataBind();
            InlineCalculation();

            Upd_Add_PanelInner.Update();
        }

        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
        }

        #region SC

        public int SaveInvoiceWitDiscSC()
        {
            int res = 0;
            DataSet ds = fill_Detail_witdiscSC();
            DataTable dt_deatils = ds.Tables[0];
            DataTable dtexpense = ds.Tables[1];
            DataTable dtTrans = ds.Tables[2];
            if (dt_deatils.Rows.Count > 0)
            {
                int paytype = 1;
                decimal bankcharge = 0;
                decimal chargedamt = 0;
                //if (hdn_IsCredit.Value != "1" && drpPayType.SelectedValue == "2")
                //{
                //    paytype = 2;
                //    bankcharge = txtbankchargeper.Text == "" ? 0 : Convert.ToDecimal(txtbankchargeper.Text);
                //    chargedamt = txtCharged.Text == "" ? 0 : Convert.ToDecimal(txtCharged.Text);
                //}
                if (drpPayType.SelectedValue == "2")
                {
                    paytype = 2;
                    bankcharge = txtbankchargeper.Text == "" ? 0 : Convert.ToDecimal(txtbankchargeper.Text);
                    chargedamt = txtCharged.Text == "" ? 0 : Convert.ToDecimal(txtCharged.Text);
                }

                if (drpService.SelectedValue == "" && (txt_displayPrice.Text != "" || txt_Qty.Text != ""))
                {
                    InlineCalculation();
                }
                res = obj_trans.Insert_Update_InvoiceWitDiscSC(Convert.ToInt32(hdn_id.Value), job_date.SelectedDate,
                Convert.ToInt32(drp_customer.SelectedValue), txt_remark.Text, Convert.ToInt32(hdn_user_id.Value), 
                Convert.ToDecimal(txt_grand.Text),
                dt_deatils, drp_quot.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_quot.SelectedValue),
                txt_totDiscount.Text == "" ? 0 : Convert.ToDecimal(txt_totDiscount.Text), rbTaxInvoice.Checked == true ? 1 : 2,
                Convert.ToInt32(hdnTaxAppliedWithDiscount.Value), dtexpense, dtTrans, paytype, bankcharge, chargedamt,
                 drpagent.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpagent.SelectedValue),
                 Convert.ToInt32(drpInvoiceFormat.SelectedValue), txtroundoff.Text != "" ? Convert.ToDecimal(txtroundoff.Text) : 0,
                 txtSubject.Text,txtBillingname.Text,Convert.ToInt32(drpinvoiceCreator.SelectedValue));

                if (txt_amtPayNow.Text != "" && res != 0)
                {
                    SaveReceipt(res);
                }
            }
            else
            {
                res = -2;
            }
            return res;
        }

        public DataSet fill_Detail_witdiscSC()
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
            dt_ser.Columns.Add("Discount", typeof(decimal));
            dt_ser.Columns.Add("Tax", typeof(decimal));
            dt_ser.Columns.Add("Deadline", typeof(DateTime));
            dt_ser.Columns.Add("TemplateId", typeof(int));
            dt_ser.Columns.Add("QuotationDetailId", typeof(int));
            dt_ser.Columns.Add("ServiceCommission", typeof(decimal));
            dt_ser.Columns.Add("CustomerStaff", typeof(string));
            dt_ser.Columns.Add("AgentCommission", typeof(decimal));//pooja added

            //sc
            dt_ser.Columns.Add("SerComDate", typeof(DateTime));
            dt_ser.Columns.Add("ExpQty", typeof(int));
            dt_ser.Columns.Add("ExpSinglAmt", typeof(decimal));
            dt_ser.Columns.Add("ExpTotAmt", typeof(decimal));

            DataTable dt_exp = new DataTable();
            dt_exp.Columns.Add("InvDId", typeof(int));
            dt_exp.Columns.Add("SerComDetailId", typeof(int));
            dt_exp.Columns.Add("ExpenseId", typeof(int));
            dt_exp.Columns.Add("Amount", typeof(decimal));
            dt_exp.Columns.Add("VAT", typeof(decimal));
            dt_exp.Columns.Add("VendorId", typeof(int));
            dt_exp.Columns.Add("PayModeId", typeof(int));
            dt_exp.Columns.Add("AccountId", typeof(int));
            dt_exp.Columns.Add("PayableAmount", typeof(decimal));
            dt_exp.Columns.Add("PaidAmount", typeof(decimal));
            dt_exp.Columns.Add("Vendorcommission", typeof(decimal));

            DataTable dt_trans = new DataTable();
            dt_trans.Columns.Add("InvDId", typeof(int));
            dt_trans.Columns.Add("TransactionNumber", typeof(string));

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
                    TextBox txtInvDdiscount = (TextBox)itm.FindControl("txtInvDdiscount");
                    HiddenField hdnInvDTax = (HiddenField)itm.FindControl("hdnInvDTax");
                    TextBox txtInvDExpense = (TextBox)itm.FindControl("txtInvDExpense");
                    RadDatePicker deadlineIn = (RadDatePicker)itm.FindControl("deadlineIn");
                    HiddenField hdnTemplateId = (HiddenField)itm.FindControl("hdnTemplateId");
                    HiddenField hdnQuotationDetailId = (HiddenField)itm.FindControl("hdnQuotationDetailId");
                    TextBox txtInvDCommissionS = (TextBox)itm.FindControl("txtInvDCommissionS");
                    TextBox txtCustomerStaffIn = (TextBox)itm.FindControl("txtCustomerStaffIn");
                    TextBox txtAgentCommission = (TextBox)itm.FindControl("txtAgentCommission");

                    //sc
                    TextBox txtExpenseQty = (TextBox)itm.FindControl("txtExpenseQty");
                    TextBox txtExpenseSinglAmt = (TextBox)itm.FindControl("txtExpenseSinglAmt");
                    TextBox txtExpenseTotalAmt = (TextBox)itm.FindControl("txtExpenseTotalAmt");
                    RadDatePicker ExpenseSerComDate = (RadDatePicker)itm.FindControl("ExpenseSerComDate");

                    Repeater rptTransCode = (Repeater)itm.FindControl("rptTransCode");
                    Repeater rptexpensein = (Repeater)itm.FindControl("rptexpensein");

                    if (hdnInvDServiceId.Value != "")
                        dt_ser.Rows.Add(Convert.ToInt32(hdnInvDId.Value), hdnInvDCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvDCategoryId.Value),
                    Convert.ToInt32(hdnInvDServiceId.Value), lblInvDdesc.Text, Convert.ToDecimal(hdnInvDPrice.Value),
                    Convert.ToDecimal(txtInvDExpense.Text),//hdnInvDExpense.Value),
                    Convert.ToDecimal(hdnInvDServiceCharge.Value), txtInvDAddServiceCharge.Text == "" ? 0 : Convert.ToDecimal(txtInvDAddServiceCharge.Text),
                    Convert.ToDecimal(txtInvDQty.Text), Convert.ToDecimal(txtInvDTaxAmount.Text),
                    Convert.ToDecimal(txtInvDPriceWitTax.Text), txtInvDFine.Text == "" ? 0 : Convert.ToDecimal(txtInvDFine.Text),
                    Convert.ToDecimal(txtInvDTotal.Text), txtInvDdiscount.Text == "" ? 0 : Convert.ToDecimal(txtInvDdiscount.Text),
                    Convert.ToDecimal(hdnInvDTax.Value), deadlineIn.SelectedDate,
                    hdnTemplateId.Value == "" ? (int?)null : Convert.ToInt32(hdnTemplateId.Value),
                    hdnQuotationDetailId.Value == "" ? (int?)null : Convert.ToInt32(hdnQuotationDetailId.Value),
                    txtInvDCommissionS.Text == "" ? 0 : Convert.ToDecimal(txtInvDCommissionS.Text), txtCustomerStaffIn.Text,
                     txtAgentCommission.Text == "" ? 0 : Convert.ToDecimal(txtAgentCommission.Text),

                       ExpenseSerComDate.SelectedDate == null ? (DateTime?)null : DateTime.ParseExact(CalDate(ExpenseSerComDate), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                   txtExpenseQty.Text == "" ? (decimal?)null : Convert.ToDecimal(txtExpenseQty.Text),
                   txtExpenseSinglAmt.Text == "" ? (decimal?)null : Convert.ToDecimal(txtExpenseSinglAmt.Text),
                   txtExpenseTotalAmt.Text == "" ? (decimal?)null : Convert.ToDecimal(txtExpenseTotalAmt.Text)
                  ); 

                    foreach (RepeaterItem itms in rptTransCode.Items)
                    {
                        TextBox txtTransCode = (TextBox)itms.FindControl("txtTransCode");
                        dt_trans.Rows.Add(Convert.ToInt32(hdnInvDId.Value), txtTransCode.Text);
                    }

                    foreach (RepeaterItem itms in rptexpensein.Items)
                    {
                        TextBox txtSerComDetailId = (TextBox)itms.FindControl("txtSerComDetailId");
                        TextBox txtExpenseId = (TextBox)itms.FindControl("txtExpenseId");
                        TextBox txtAmount = (TextBox)itms.FindControl("txtAmount");
                        TextBox txtVAT = (TextBox)itms.FindControl("txtVAT");
                        TextBox txtVendorId = (TextBox)itms.FindControl("txtVendorId");
                        TextBox txtPayModeId = (TextBox)itms.FindControl("txtPayModeId");
                        TextBox txtAccountId = (TextBox)itms.FindControl("txtAccountId");
                        TextBox txtPayableAmount = (TextBox)itms.FindControl("txtPayableAmount");
                        TextBox txtPaidAmount = (TextBox)itms.FindControl("txtPaidAmount");

                        dt_exp.Rows.Add(Convert.ToInt32(hdnInvDId.Value),
                             txtSerComDetailId.Text == "" ? (int?)null : Convert.ToInt32(txtSerComDetailId.Text),
                             txtExpenseId.Text == "" ? (int?)null : Convert.ToInt32(txtExpenseId.Text),
                              txtAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtAmount.Text),
                              txtVAT.Text == "" ? (decimal?)null : Convert.ToDecimal(txtVAT.Text),
                              txtVendorId.Text == "" ? (int?)null : Convert.ToInt32(txtVendorId.Text),
                              txtPayModeId.Text == "" ? (int?)null : Convert.ToInt32(txtPayModeId.Text),
                              txtAccountId.Text == "" ? (int?)null : Convert.ToInt32(txtAccountId.Text),
                              txtPayableAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtPayableAmount.Text),
                              txtPaidAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txtPaidAmount.Text), 0);
                    }
                }
            }
            if (drpService.SelectedValue != "" && txt_displayPrice.Text != "" && txt_Qty.Text != "")
            {
                dt_ser.Rows.Add(Convert.ToInt32(hdn_InvDetailId.Value), hdnSerCategoryId.Value == "" ? (int?)null : Convert.ToInt32(hdnSerCategoryId.Value),
                    Convert.ToInt32(drpService.SelectedValue), txt_desc.Text, Convert.ToDecimal(hdnPrice.Value), Convert.ToDecimal(txtExpense.Text),//hdn_expn.Value),
                       Convert.ToDecimal(hdn_sc.Value), 0,
                       Convert.ToDecimal(txt_Qty.Text), Convert.ToDecimal(txt_taxamt.Text),
                       Convert.ToDecimal(txt_PriceWitTax.Text), txtFine.Text == "" ? 0 : Convert.ToDecimal(txtFine.Text),
                       Convert.ToDecimal(txt_totPrice.Text), txt_discount.Text == "" ? 0 : Convert.ToDecimal(txt_discount.Text),
                       Convert.ToDecimal(hdn_tax.Value), deadline.SelectedDate, (int?)null, (int?)null,
                       txtCommissionSOut.Text == "" ? 0 : Convert.ToDecimal(txtCommissionSOut.Text), txtCustomerStaffOut.Text,
                      txtAgentCommissionOut.Text == "" ? 0 : Convert.ToDecimal(txtAgentCommissionOut.Text));
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt_ser);
            ds.Tables.Add(dt_exp);
            ds.Tables.Add(dt_trans);

            return ds;
        }

        protected void btnCompleSC_OnClick(object sender, EventArgs e)
        {
            ClearSC();
            hdn_ExpinvD_id.Value = "0";
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;
            HiddenField hdnInvDIdP = (HiddenField)itemrp.FindControl("hdnInvDId");
            HiddenField hdnInvDServiceIdP = (HiddenField)itemrp.FindControl("hdnInvDServiceId");
            Label lblServiceFullNameP = (Label)itemrp.FindControl("lblServiceFullName");
            TextBox txtInvDQtyP = (TextBox)itemrp.FindControl("txtInvDQty");
            TextBox txtInvDFineP = (TextBox)itemrp.FindControl("txtInvDFine");

            hdn_ExpinvD_id.Value = hdnInvDIdP.Value;
            hdn_fineAmt.Value = txtInvDFineP.Text == "" ? "0" : txtInvDFineP.Text;
            lblcomplete.Visible = false;
            btn_expDetail_line.Visible = btnInlineSave.Visible = txtInlineQty.Visible = true;

            if (hdn_id.Value == "0")
            {
                DataTable dt = obj_trans.ServiceAmtForSingleQty(Convert.ToInt32(hdnInvDServiceIdP.Value));
                lbl_service.Text = lblServiceFullNameP.Text;
                hdn_service_id.Value = hdnInvDServiceIdP.Value;
                hdn_ExpinvD_id.Value = hdnInvDIdP.Value;
                txt_InvQty.Text = txtInvDQtyP.Text;
                txt_InComQty.Text = txtInvDQtyP.Text;
                txtInlineQty.Text = "1";
                if (dt.Rows.Count > 0)
                {
                    txtInlineAmtSQty.Text = (Convert.ToDecimal(dt.Rows[0]["AmtForSingleQty"].ToString()) + (txtInvDFineP.Text == "" ? 0 : Convert.ToDecimal(txtInvDFineP.Text))).ToString();
                    txtInlineTotAmt.Text = (Convert.ToDecimal(dt.Rows[0]["AmtForSingleQty"].ToString()) + (txtInvDFineP.Text == "" ? 0 : Convert.ToDecimal(txtInvDFineP.Text))).ToString();
                    InlineSerComDate.DbSelectedDate = dt.Rows[0]["SerComDate"].ToString();
                }
                else
                {
                    txtInlineAmtSQty.Text = "0";
                    txtInlineTotAmt.Text = "0";
                    InlineSerComDate.DbSelectedDate = DateTime.Now;
                }
            }
            else
            {

                if (Convert.ToInt32(hdn_ExpinvD_id.Value) <= 0)
                {
                    DataTable dt = obj_trans.ServiceAmtForSingleQty(Convert.ToInt32(hdnInvDServiceIdP.Value));
                    lbl_service.Text = lblServiceFullNameP.Text;
                    hdn_service_id.Value = hdnInvDServiceIdP.Value;
                    hdn_ExpinvD_id.Value = hdnInvDIdP.Value;
                    txt_InvQty.Text = txtInvDQtyP.Text;
                    txt_InComQty.Text = txtInvDQtyP.Text;
                    txtInlineQty.Text = "1";

                    if (dt.Rows.Count > 0)
                    {
                        txtInlineAmtSQty.Text = (Convert.ToDecimal(dt.Rows[0]["AmtForSingleQty"].ToString()) + (txtInvDFineP.Text == "" ? 0 : Convert.ToDecimal(txtInvDFineP.Text))).ToString();
                        txtInlineTotAmt.Text = (Convert.ToDecimal(dt.Rows[0]["AmtForSingleQty"].ToString()) + (txtInvDFineP.Text == "" ? 0 : Convert.ToDecimal(txtInvDFineP.Text))).ToString();
                        InlineSerComDate.DbSelectedDate = dt.Rows[0]["SerComDate"].ToString();
                    }
                    else
                    {
                        txtInlineAmtSQty.Text = "0";
                        txtInlineTotAmt.Text = "0";
                        InlineSerComDate.DbSelectedDate = DateTime.Now;
                    }
                }
                else
                {
                    lbl_service.Text = lblServiceFullNameP.Text;
                    hdn_service_id.Value = hdnInvDServiceIdP.Value;
                    hdn_ExpinvD_id.Value = hdnInvDIdP.Value;

                    DataSet ds = obj_trans.Get_InvDetail_ServiceCompletionINVSC(Convert.ToInt32(hdn_ExpinvD_id.Value), Convert.ToInt32(hdn_user_id.Value));
                    DataTable dt_ser = ds.Tables[0];/* Detail*/

                    txt_InvQty.Text = dt_ser.Rows[0]["InvoiceQuantity"].ToString();
                    txt_InComQty.Text = dt_ser.Rows[0]["InComQuantity"].ToString();
                    txtInlineQty.Text = dt_ser.Rows[0]["Quantity"].ToString();
                    txtInlineAmtSQty.Text = dt_ser.Rows[0]["AmtForSingleQty"].ToString();
                    txtInlineTotAmt.Text = dt_ser.Rows[0]["TotalAmount"].ToString();
                    InlineSerComDate.DbSelectedDate = dt_ser.Rows[0]["SerComDate"].ToString();
                    if (txt_InComQty.Text == "0" || txt_InComQty.Text == "0.00")
                    {
                        lblcomplete.Visible = true;
                        btn_expDetail_line.Visible = btnInlineSave.Visible = txtInlineQty.Visible = false;
                    }
                }

            }
            pnlSC.Visible = true;
            UpdSC.Update();
        }

        public void ClearSC()
        {
            hdn_InvDetailId.Value = "0";
            hdn_InComQty.Value = "0";
            DataTable dt_exp = new DataTable();
            dt_exp.Columns.Add("SerComDetailId", typeof(int));
            dt_exp.Columns.Add("ExpenseId", typeof(int));
            dt_exp.Columns.Add("ExpenseName", typeof(string));
            dt_exp.Columns.Add("Amount", typeof(decimal));
            dt_exp.Columns.Add("VAT", typeof(decimal));
            dt_exp.Columns.Add("VendorId", typeof(int));
            dt_exp.Columns.Add("PayModeId", typeof(int));
            dt_exp.Columns.Add("AccountId", typeof(int));
            dt_exp.Columns.Add("PayableAmount", typeof(decimal));
            dt_exp.Columns.Add("PaidAmount", typeof(decimal));

            rpt_expense_list.DataSource = dt_exp;
            rpt_expense_list.DataBind();
            txtscqty.Text = "";
            txtscqty.Enabled = true;
            txt_amtSQty.Text = "";
            txt_totAmt.Text = "";
            SerComDate.DbSelectedDate = DateTime.Now;
            pnlSC.Visible = false;
            pnl_Expense_Panel.Visible = false;
            UpdSC.Update();
        }

        protected void btn_expDetail_line_OnClick(object sender, EventArgs e)
        {
            txtscqty.Text = "";
            txt_amtSQty.Text = "";
            txt_totAmt.Text = "";
            //txtscqty.Text = txtInlineQty.Text;
            //txt_amtSQty.Text = txtInlineAmtSQty.Text;
            //txt_totAmt.Text = txtInlineTotAmt.Text;
            SerComDate.DbSelectedDate = InlineSerComDate.SelectedDate;
            txtscqty.Enabled = true;
            hdn_InComQty.Value = txt_InComQty.Text;
            pnl_Expense_Panel.Visible = true;

            if (hdn_id.Value == "0")
            {
                DataSet ds = obj_trans.Get_SerExpenseDetail_SC_byService(Convert.ToInt32(hdn_service_id.Value));
                DataTable dt1 = ds.Tables[0];/*invoic*/
                foreach (DataRow r in dt1.Rows)
                {
                    if (r["ExpenseId"].ToString() == r["FineId"].ToString())
                    {
                        r["Amount"] = hdn_fineAmt.Value;
                    }
                }
                rpt_expense_list.DataSource = dt1;
                rpt_expense_list.DataBind();
            }
            else
            {

                if (Convert.ToInt32(hdn_ExpinvD_id.Value) <= 0)
                {
                    DataSet ds = obj_trans.Get_SerExpenseDetail_SC_byService(Convert.ToInt32(hdn_service_id.Value));
                    DataTable dt1 = ds.Tables[0];/*invoic*/
                    foreach (DataRow r in dt1.Rows)
                    {
                        if (r["ExpenseId"].ToString() == r["FineId"].ToString())
                        {
                            r["Amount"] = hdn_fineAmt.Value;
                        }
                    }
                    rpt_expense_list.DataSource = dt1;
                    rpt_expense_list.DataBind();
                }
                else
                {
                    DataSet ds = obj_trans.Get_SerExpenseDetail_ServiceCompletion(Convert.ToInt32(hdn_ExpinvD_id.Value));
                    DataTable dt1 = ds.Tables[0];/*invoic*/
                    rpt_expense_list.DataSource = dt1;
                    rpt_expense_list.DataBind();
                }

            }
            Upd_Expense_Panel.Update();
        }

        protected void btnInlineExpenseSave_OnClick(object sender, EventArgs e)
        {
            int DisplayMessage = 0;
            txtscqty.Text = txtInlineQty.Text;
            txt_amtSQty.Text = txtInlineAmtSQty.Text;
            txt_totAmt.Text = txtInlineTotAmt.Text;
            SerComDate.DbSelectedDate = InlineSerComDate.SelectedDate;
            txtscqty.Enabled = true;
            hdn_InComQty.Value = txt_InComQty.Text;
            pnl_Expense_Panel.Visible = false;

            if (hdn_id.Value == "0")
            {
                DataSet ds = obj_trans.Get_SerExpenseDetail_SC_byService(Convert.ToInt32(hdn_service_id.Value));
                DataTable dt1 = ds.Tables[0];/*invoic*/
                foreach (DataRow r in dt1.Rows)
                {
                    if (r["ExpenseId"].ToString() == r["FineId"].ToString())
                    {
                        r["Amount"] = hdn_fineAmt.Value;
                    }
                }
                rpt_expense_list.DataSource = dt1;
                rpt_expense_list.DataBind();
            }
            else
            {
                if (Convert.ToInt32(hdn_ExpinvD_id.Value) <= 0)
                {
                    DataSet ds = obj_trans.Get_SerExpenseDetail_SC_byService(Convert.ToInt32(hdn_service_id.Value));
                    DataTable dt1 = ds.Tables[0];/*invoic*/
                    foreach (DataRow r in dt1.Rows)
                    {
                        if (r["ExpenseId"].ToString() == r["FineId"].ToString())
                        {
                            r["Amount"] = hdn_fineAmt.Value;
                        }
                    }
                    rpt_expense_list.DataSource = dt1;
                    rpt_expense_list.DataBind();
                }
                else
                {
                    DataSet ds = obj_trans.Get_SerExpenseDetail_ServiceCompletion(Convert.ToInt32(hdn_ExpinvD_id.Value));
                    DataTable dt1 = ds.Tables[0];/*invoic*/
                    rpt_expense_list.DataSource = dt1;
                    rpt_expense_list.DataBind();
                }
            }

            DataTable dt_exp = new DataTable();
            dt_exp.Columns.Add("SerComDetailId", typeof(int));
            dt_exp.Columns.Add("ExpenseId", typeof(int));
            dt_exp.Columns.Add("ExpenseName", typeof(string));
            dt_exp.Columns.Add("Amount", typeof(decimal));
            dt_exp.Columns.Add("VAT", typeof(decimal));
            dt_exp.Columns.Add("VendorId", typeof(int));
            dt_exp.Columns.Add("PayModeId", typeof(int));
            dt_exp.Columns.Add("AccountId", typeof(int));
            dt_exp.Columns.Add("PayableAmount", typeof(decimal));
            dt_exp.Columns.Add("PaidAmount", typeof(decimal));

            foreach (RepeaterItem expItem in rpt_expense_list.Items)
            {
                HiddenField hdn_expenseId = (HiddenField)expItem.FindControl("hdn_expenseId");
                Label lbl_Expense = (Label)expItem.FindControl("lbl_Expense");
                TextBox txt_amt = (TextBox)expItem.FindControl("txt_amt");
                TextBox txt_vat = (TextBox)expItem.FindControl("txt_vat");
                RadComboBox drp_vendor = (RadComboBox)expItem.FindControl("drp_vendor");
                RadComboBox drp_payMode = (RadComboBox)expItem.FindControl("drp_payMode");
                RadComboBox drp_account = (RadComboBox)expItem.FindControl("drp_account");
                TextBox txt_payableAmount = (TextBox)expItem.FindControl("txt_payableAmount");
                TextBox txt_paidAmount = (TextBox)expItem.FindControl("txt_paidAmount");
                decimal PayableAmount = 0;
                if (drp_vendor.SelectedValue != "" && drp_payMode.SelectedValue != "" && drp_account.SelectedValue != "")
                {
                    PayableAmount = (Convert.ToDecimal(txt_amt.Text) + Convert.ToDecimal(txt_vat.Text)) * Convert.ToDecimal(txtscqty.Text);
                    dt_exp.Rows.Add(0, Convert.ToInt32(hdn_expenseId.Value), lbl_Expense.Text,
                Convert.ToDecimal(txt_amt.Text), Convert.ToDecimal(txt_vat.Text),
                Convert.ToInt32(drp_vendor.SelectedValue), Convert.ToInt32(drp_payMode.SelectedValue),
                Convert.ToInt32(drp_account.SelectedValue), PayableAmount, PayableAmount);
                }
                else
                {
                    DisplayMessage = 1;
                    break;
                }
            }
            if (DisplayMessage == 1)
            {
                pnl_Expense_Panel.Visible = true;
                txtscqty.Text = "";
                txt_amtSQty.Text = "";
                txt_totAmt.Text = "";
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please Add Service Expense Detail');", true);
            }
            else
            {
                rpt_expense_list.DataSource = dt_exp;
                rpt_expense_list.DataBind();
                btn_saveSC_OnClick(null, null);
            }
            Upd_Expense_Panel.Update();
        }

        protected void btn_closeSC_OnClick(object sender, EventArgs e)
        {
            pnlSC.Visible = false;
            UpdSC.Update();
        }

        protected void btn_saveSC_OnClick(object sender, EventArgs e)
        {
            if (rpt_expense_list.Items.Count > 0)
            {
                int NoOfRows = Convert.ToInt32(txtscqty.Text);
                DataTable dt_trans = new DataTable();
                dt_trans.Columns.Add("TransactionNumber", typeof(string));

                for (int i = 0; i < NoOfRows; i++)
                {
                    dt_trans.Rows.Add("");
                }
                rpt_TransacDetail.DataSource = dt_trans;
                rpt_TransacDetail.DataBind();
                pnl_transaDetail.Visible = true;
                Upd_TransaDetail_Panel.Update();
            }
            else
            {
                DataSet dt_deatils = SCfill_Detail();
                SaveServiceCompletion(dt_deatils);
                pnl_transaDetail.Visible = false;
                Upd_Add_PanelInner.Update();
                pnlSC.Visible = false;
                UpdSC.Update();
            }
        }

        public void SaveServiceCompletion(DataSet ds_deatils)
        {
            foreach (RepeaterItem itm in rpt_Item_list.Items)
            {
                HiddenField hdnInvDId = (HiddenField)itm.FindControl("hdnInvDId");
                TextBox txtExpenseQty = (TextBox)itm.FindControl("txtExpenseQty");
                TextBox txtExpenseSinglAmt = (TextBox)itm.FindControl("txtExpenseSinglAmt");
                TextBox txtExpenseTotalAmt = (TextBox)itm.FindControl("txtExpenseTotalAmt");
                RadDatePicker ExpenseSerComDate = (RadDatePicker)itm.FindControl("ExpenseSerComDate");

                Repeater rptTransCode = (Repeater)itm.FindControl("rptTransCode");
                Repeater rptexpensein = (Repeater)itm.FindControl("rptexpensein");


                if (hdnInvDId.Value == hdn_ExpinvD_id.Value)
                {
                    txtExpenseQty.Text = txtscqty.Text;
                    txtExpenseSinglAmt.Text = txt_amtSQty.Text;
                    txtExpenseTotalAmt.Text = txt_totAmt.Text;
                    ExpenseSerComDate.DbSelectedDate = DateTime.ParseExact(CalDate(SerComDate), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    rptexpensein.DataSource = ds_deatils.Tables[0];
                    rptexpensein.DataBind();
                    rptTransCode.DataSource = ds_deatils.Tables[1];
                    rptTransCode.DataBind();

                    break;
                }
            }
            Upd_Item_Panel.Update();
        }

        public DataSet SCfill_Detail()
        {
            DataSet ds = new DataSet();
            DataTable dt_exp = new DataTable();
            dt_exp.Columns.Add("InvDId", typeof(int));
            dt_exp.Columns.Add("SerComDetailId", typeof(int));
            dt_exp.Columns.Add("ExpenseId", typeof(int));
            dt_exp.Columns.Add("Amount", typeof(decimal));
            dt_exp.Columns.Add("VAT", typeof(decimal));
            dt_exp.Columns.Add("VendorId", typeof(int));
            dt_exp.Columns.Add("PayModeId", typeof(int));
            dt_exp.Columns.Add("AccountId", typeof(int));
            dt_exp.Columns.Add("PayableAmount", typeof(decimal));
            dt_exp.Columns.Add("PaidAmount", typeof(decimal));
            dt_exp.Columns.Add("Vendorcommission", typeof(decimal));

            DataTable dt_trans = new DataTable();
            dt_trans.Columns.Add("TransactionNumber", typeof(string));

            if (rpt_expense_list.Items.Count > 0)
            {
                foreach (RepeaterItem expItem in rpt_expense_list.Items)
                {
                    HiddenField hdnSerComDetailId = (HiddenField)expItem.FindControl("hdnSerComDetailId");
                    HiddenField hdn_expenseId = (HiddenField)expItem.FindControl("hdn_expenseId");
                    TextBox txt_amt = (TextBox)expItem.FindControl("txt_amt");
                    TextBox txt_vat = (TextBox)expItem.FindControl("txt_vat");
                    RadComboBox drp_vendor = (RadComboBox)expItem.FindControl("drp_vendor");
                    RadComboBox drp_payMode = (RadComboBox)expItem.FindControl("drp_payMode");
                    RadComboBox drp_account = (RadComboBox)expItem.FindControl("drp_account");
                    TextBox txt_payableAmount = (TextBox)expItem.FindControl("txt_payableAmount");
                    TextBox txt_paidAmount = (TextBox)expItem.FindControl("txt_paidAmount");

                    dt_exp.Rows.Add(Convert.ToInt32(hdn_InvDetailId.Value), Convert.ToInt32(hdnSerComDetailId.Value), Convert.ToInt32(hdn_expenseId.Value),
                Convert.ToDecimal(txt_amt.Text), Convert.ToDecimal(txt_vat.Text),
                Convert.ToInt32(drp_vendor.SelectedValue), Convert.ToInt32(drp_payMode.SelectedValue),
               drp_account.SelectedValue==""?(int?)null: Convert.ToInt32(drp_account.SelectedValue), Convert.ToDecimal(txt_payableAmount.Text),
                Convert.ToDecimal(txt_paidAmount.Text), 0);
                }
            }
            if (rpt_TransacDetail.Items.Count > 0)
            {
                foreach (RepeaterItem Item in rpt_TransacDetail.Items)
                {
                    TextBox txt_transNumber = (TextBox)Item.FindControl("txt_transNumber");
                    dt_trans.Rows.Add(txt_transNumber.Text);
                }
            }
            ds.Tables.Add(dt_exp);
            ds.Tables.Add(dt_trans);
            return ds;
        }

        public void callSAveCompletion(object sender, EventArgs e)
        {
            DataSet dt_deatils = SCfill_Detail();
            SaveServiceCompletion(dt_deatils);
            pnlSC.Visible = false;
            UpdSC.Update();
            pnl_transaDetail.Visible = false;
            Upd_TransaDetail_Panel.Update();
        }

        protected void btn_FinalSave_OnClick(object sender, EventArgs e)
        {
            DataSet dt_deatils = SCfill_Detail();

            if (dt_deatils.Tables[0].Rows.Count > 0)
            {
                //int chk = 0;
                //if (dt_deatils.Tables[0].Rows[0]["PayModeId"].ToString() == "3")
                //{
                //    int bankId = Convert.ToInt32(dt_deatils.Tables[0].Rows[0]["AccountId"]);
                //    DataTable dt = obj_mas.Edit_Bank_Account(bankId);
                //    if (Convert.ToDecimal(dt.Rows[0]["Balance"]) < Convert.ToDecimal(txt_totAmt.Text == "" ? "0" : txt_totAmt.Text))
                //    {
                //        chk = 1;
                //        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Confirm()", true);
                //    }
                //}
                //if (chk == 0)
                //{
                    SaveServiceCompletion(dt_deatils);
                    pnlSC.Visible = false;
                    UpdSC.Update();
                    pnl_transaDetail.Visible = false;
                    Upd_TransaDetail_Panel.Update();
                //}
            }
            else
            {
                lbl_msg.Text = "Add Quantity to Continue !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                pnl_transaDetail.Visible = false;
                Upd_TransaDetail_Panel.Update();
            }
        }

        protected void btn_TransDetail_Close_OnClick(object sender, EventArgs e)
        {
            pnl_transaDetail.Visible = false;
            Upd_TransaDetail_Panel.Update();
        }

        protected void rpt_expense_list_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdn_vendorId = (HiddenField)e.Item.FindControl("hdn_vendorId");
                RadComboBox drp_vendor = (RadComboBox)e.Item.FindControl("drp_vendor");
                drp_vendor.Items.Clear();
                DataTable dtVendor = obj_mas.Drp_Vendor();
                drp_vendor.DataSource = dtVendor;
                drp_vendor.DataValueField = "Value";
                drp_vendor.DataTextField = "Text";
                drp_vendor.DataBind();
                drp_vendor.SelectedValue = hdn_vendorId.Value;

                HiddenField hdn_payModeId = (HiddenField)e.Item.FindControl("hdn_payModeId");
                RadComboBox drp_payMode = (RadComboBox)e.Item.FindControl("drp_payMode");
                drp_payMode.Items.Clear();
                DataTable dtPayMode = obj_mas.Drp_PaymentMode_WithoutCredit();
                drp_payMode.DataSource = dtPayMode;
                drp_payMode.DataValueField = "Value";
                drp_payMode.DataTextField = "Text";
                drp_payMode.DataBind();
                drp_payMode.SelectedValue = hdn_payModeId.Value;
                drp_payMode.Items.Remove(drp_payMode.Items.FindItemByValue("2"));/*Remove Cheque*/

                HiddenField hdn_accountId = (HiddenField)e.Item.FindControl("hdn_accountId");
                RadComboBox drp_account = (RadComboBox)e.Item.FindControl("drp_account");
                drp_account.Items.Clear();
                if (hdn_payModeId.Value != "")
                {
                    DataTable dtAccount = obj_trans.ListAccountInServCompletion(Convert.ToInt32(hdn_payModeId.Value), Convert.ToInt32(hdn_user_id.Value),
                        hdn_accountId.Value == "" ? 0 : Convert.ToInt32(hdn_accountId.Value));
                    drp_account.DataSource = dtAccount;
                    drp_account.DataValueField = "Value";
                    drp_account.DataTextField = "Text";
                    drp_account.DataBind();
                }
                drp_account.SelectedValue = hdn_accountId.Value;
            }

        }

        protected void drp_payMode_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            RadComboBox drp = (RadComboBox)sender;
            RepeaterItem itm = (RepeaterItem)drp.Parent;
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            HiddenField hdn_accountId = (HiddenField)itm.FindControl("hdn_accountId");
            RadComboBox drp_account = (RadComboBox)itm.FindControl("drp_account");
            UpdatePanel Upd_Account_Panel = (UpdatePanel)itm.FindControl("Upd_Account_Panel");
            RequiredFieldValidator rqdaccountIn = (RequiredFieldValidator)itm.FindControl("rqdaccountIn");
            TextBox txt_paidAmount = (TextBox)itm.FindControl("txt_paidAmount");
            UpdatePanel updpaidAmountIn = (UpdatePanel)itm.FindControl("updpaidAmountIn");

            rqdaccountIn.Enabled = true;
            txt_paidAmount.ReadOnly = false;
            drp_account.Items.Clear();
            if (drp.SelectedValue != "")
            {
                DataTable dtAccount = obj_trans.ListAccountInServCompletion(Convert.ToInt32(drp.SelectedValue), Convert.ToInt32(hdn_user_id.Value), 0);
                drp_account.DataSource = dtAccount;
                drp_account.DataValueField = "Value";
                drp_account.DataTextField = "Text";
                drp_account.DataBind();
            }
            hdn_accountId.Value = "";
            drp_account.ClearSelection();
            drp_account.Text = "";

            if (drp.SelectedValue == "7"|| drp.SelectedValue == "9")
            {
                rqdaccountIn.Enabled = false;
                txt_paidAmount.ReadOnly = true;
                txt_paidAmount.Text = "0";
            }
            else if (drp.SelectedValue == "8")
            {
                rqdaccountIn.Enabled = false;
            }
            updpaidAmountIn.Update();

            Upd_Account_Panel.Update();
        }

        #endregion

        #region History

        protected void btn_histry_OnClick(object sender, EventArgs e)
        {
            date_from.SelectedDate = null;
            date_to.SelectedDate = null;

            grid_fill_his(1, 12);

            div_main.Visible = false;
            div_trans_main.Visible = true;
            upd_main.Update();
        }

        protected void btn_his_seacrh_OnClick(object sender, EventArgs e)
        {
            grid_fill_his(1, 10);

            Upd_History.Update();
        }

        protected void btnexcel_exportHis_OnClick(object sender, EventArgs e)
        {
            DataSet ds = obj_trans.list_InvHistry(date_from.SelectedDate, date_to.SelectedDate, Convert.ToInt32(hdn_id.Value),
               1, Convert.ToInt32(drp_count1.SelectedValue));
            DataTable dt = ds.Tables[0];

            dt.Columns.Remove("current_count");
            dt.Columns.Remove("page_number");
            dt.Columns.Remove("Page_size");
            dt.Columns.Remove("start_num");
            dt.Columns.Remove("end_num");
            dt.Columns.Remove("last_page");

            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "Invoicehistory");
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                HttpContext.Current.Response.Write(style);
                Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        protected void btn_histry_Close_OnClick(object sender, EventArgs e)
        {
            div_main.Visible = true;
            div_trans_main.Visible = false;
            upd_main.Update();
        }

        public void grid_fill_his(int page_number, int page_size)
        {
            DataSet ds = obj_trans.list_InvHistry(date_from.SelectedDate, date_to.SelectedDate, Convert.ToInt32(hdn_id.Value),
                page_number, page_size);
            DataTable dt = ds.Tables[0];

            rpt_His.DataSource = dt;
            rpt_His.DataBind();

            if (dt.Rows.Count > 0)
            {
                lbl_page_info1.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["SLNo"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
                hdn_last_page1.Value = dt.Rows[0]["last_page"].ToString();
                lbl_page_number1.Text = dt.Rows[0]["page_number"].ToString();
                hdn_total1.Value = dt.Rows[0]["current_count"].ToString();
            }
            else
            {
                lbl_page_info1.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
                hdn_last_page1.Value = "0";
                lbl_page_number1.Text = "1";
                hdn_total1.Value = "0";
            }
            upd_his_nav.Update();
            Upd_History.Update();
        }

        #region his Navigation

        //First Page
        protected void btn_first1_OnClick(object sender, EventArgs e)
        {
            grid_fill_his(1, Convert.ToInt32(drp_count1.SelectedValue));
        }

        //Previous Page
        protected void btn_prev1_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number1.Text) > 1)
            {
                grid_fill_his(Convert.ToInt32(lbl_page_number1.Text) - 1, Convert.ToInt32(drp_count1.SelectedValue));
            }
        }

        //Next Page
        protected void btn_next1_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number1.Text) < Convert.ToInt32(hdn_last_page1.Value))
            {
                grid_fill_his(Convert.ToInt32(lbl_page_number1.Text) + 1, Convert.ToInt32(drp_count1.SelectedValue));
            }
        }

        //Last Page
        protected void btn_last1_OnClick(object sender, EventArgs e)
        {
            grid_fill_his(Convert.ToInt32(hdn_last_page1.Value), Convert.ToInt32(drp_count1.SelectedValue));
        }

        //Page Data Count
        protected void drp_count1_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            grid_fill_his(1, Convert.ToInt32(drp_count1.SelectedValue));
        }

        #endregion

        #endregion

        protected void btn_close_OnClick(object sender, EventArgs e)
        {
            if (hdnPageId.Value == "1")  //invocie
            {
                Panel pnl_add = (Panel)this.Parent.FindControl("pnl_add");
                UpdatePanel Upd_Add_Panel = (UpdatePanel)this.Parent.FindControl("Upd_Add_Panel");
                ((Invoice)this.Page).grid_fill(1, Convert.ToInt32(hdnCount.Value), hdnfilter.Value, "", "");

                pnl_add.Visible = false;
                Upd_Add_Panel.Update();
            }
            else if (hdnPageId.Value == "2")  //home
            {
                Panel pnlInvoiceadd = (Panel)this.Parent.FindControl("pnlInvoiceadd");
                UpdatePanel UpdInvoiceadd = (UpdatePanel)this.Parent.FindControl("UpdInvoiceadd");

                pnlInvoiceadd.Visible = false;
                UpdInvoiceadd.Update();
            }
            else if (hdnPageId.Value == "3") //cr
            {
                Panel pnlInvoiceadd = (Panel)this.Parent.FindControl("pnlInvoiceadd");
                UpdatePanel UpdInvoiceadd = (UpdatePanel)this.Parent.FindControl("UpdInvoiceadd");
                Panel pnl_add = (Panel)this.Parent.FindControl("pnl_add");
                UpdatePanel Upd_Add_Panel = (UpdatePanel)this.Parent.FindControl("Upd_Add_Panel");

                ((CustomerRequest)this.Page).grid_fill(1, 10, "", "", "");

                pnlInvoiceadd.Visible = false;
                UpdInvoiceadd.Update();
                pnl_add.Visible = false;
                Upd_Add_Panel.Update();
            }
        }

        public void ClearServiceDetail()
        {
            lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
            int hdnval = 0;
            foreach (RepeaterItem itm in rpt_Item_list.Items)
            {
                HiddenField hdnInvDId = (HiddenField)itm.FindControl("hdnInvDId");
                int inhdnval = Convert.ToInt32(hdnInvDId.Value);
                if (inhdnval < hdnval)
                    hdnval = inhdnval;
            }
            hdnval = hdnval - 1;
            hdn_InvDetailId.Value = hdnval.ToString();// "-" + (rpt_Item_list.Items.Count + 1).ToString();
            //hdn_InvDetailId.Value="-"+(rpt_Item_list.Items.Count + 1).ToString();
            drpDepartment.ClearSelection();
            drpDepartment.Text = "";
            drpSerCategory.ClearSelection();
            drpSerCategory.Text = "";
            drpSerSubCategory.ClearSelection();
            drpSerSubCategory.Text = "";
            drpService.ClearSelection();
            drpService.Text = "";
            txtCommissionSOut.Text = txtCustomerStaffOut.Text= "";
            deadline.SelectedDate = null;

            fill_FilterDropDown(0);
        }
                 
        public void Clear()
        {
            hdn_PageName.Value = "Invoice";/*Used in Customer User Control*/
            hdn_id.Value = "0";
            drp_customer.ClearSelection();
            drp_customer.Text = "";
            txt_token.Enabled =  true;
            drpagent.ClearSelection();
            drpagent.Text = "";
            drp_agent_OnSelectedIndexChanged(null, null);
            drp_customer.Enabled = drpagent.Enabled = true;
            drp_customer_OnSelectedIndexChanged(null, null);
            drp_quot.Items.Clear();
            drp_quot.Text = "";
            txtadvance.Text = txtSubject.Text = "";
            hdnaction.Value = hdnrequestId.Value= "0";
            updAlertReceivedamt.Update();
            drpPayType.SelectedValue = "1";
            txtbankchargeper.Text = txtCharged.Text = "";
            //pnlbankcharge.Visible = false;
            trnewline.Visible = true;
            hdnIsQuotaionEditablePrime.Value = hdnCustCommsnApplcable.Value = "0";
            drpInvoiceFormat.SelectedValue = hdnInvoiceFormatGen.Value;
            txtroundoff.Text = txtBillingname.Text= "";

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
            hdn_CurrentInvoiceReceivable.Value = "0";
            drp_quot.Enabled = true;
            //txt_remark.Text = "";
            txt_grand.Text = "";
            hdnInvoiceStatus.Value = "0";
            txt_token.Text = "";
            txt_discount.Text = "";
            DataTable dt = obj_mas.Edit_GeneralSettings();
            txt_remark.Text = dt.Rows[0]["DefaultInvoiceRemark"].ToString();

            if (hdnIsEditInvoiceCreator.Value == "0")
            {
                drpinvoiceCreator.SelectedValue = hdn_user_id.Value;
                pnlinvoiceCreator.Visible = false;
            }
            else
            {
                drpinvoiceCreator.ClearSelection();
                drpinvoiceCreator.Text = "";
                pnlinvoiceCreator.Visible = true;
            }

            job_date.DbSelectedDate = DateTime.Now;

            drpTemplates.Text = string.Empty;
            drpTemplates.ClearCheckedItems();
            drpTemplates.ClearSelection();

            rpt_Item_list.DataSource = null;
            rpt_Item_list.DataBind();
            ClearServiceDetail();
            btnDuplicate.Visible = false;
            btn_save.Visible = hdn_add.Value == "0" ? false : true;
            btn_save_print.Visible = hdn_add_N_print.Value == "0" ? false : true;
            btnMakePay.Visible = hdnMakeReceipt.Value == "0" ? false : true;

            btn_print.Visible = btnSplitInvoice.Visible = false;
            btn_TaxInvoicePrint.Visible = false;
            btn_cancel.Visible = false;
            btn_history.Visible = false;
            Get_Code();

            Upd_Add_PanelInner.Update();

            ClearReceipt();
        }

        public void Get_Code()
        {
            DataTable dt = obj_common.Get_Code(16);
            if (dt.Rows.Count > 0)
                lbl_Code.Text = dt.Rows[0][0].ToString();
        }


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
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdn_update.Value = dt.Rows[1][1].ToString();
                        hdn_print.Value = dt.Rows[2][1].ToString();
                        hdn_add_N_print.Value = dt.Rows[3][1].ToString();
                        hdn_update_N_print.Value = dt.Rows[4][1].ToString();
                        hdn_histry.Value = dt.Rows[5][1].ToString();
                        hdn_cancel.Value = dt.Rows[6][1].ToString();
                        hdn_TaxInvoicePrint.Value = dt.Rows[7][1].ToString();
                        hdnMakeReceipt.Value = dt.Rows[8][1].ToString();
                        hdnSplitInvoice.Value = "0";// dt.Rows[9][1].ToString();
                        hdnsendmail.Value = dt.Rows[10][1].ToString();
                        hdnduplicate.Value = dt.Rows[11][1].ToString();
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