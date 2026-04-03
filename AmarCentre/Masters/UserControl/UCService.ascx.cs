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
using AmarCentre.Masters;
using AmarCentre.Transactions;

namespace AmarCentre.Masters.UserControl
{
    public partial class UCService : System.Web.UI.UserControl
    {
        Master_Bal obj_master = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        public void UCPageLoad(int PageId,int ServiceId)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
            }
            else
            {
                hdnPageId.Value = PageId.ToString();    //  1-Servicemaster,    2-invoice,    3-Qtn
                hdn_user_id.Value = Session["User_Id"].ToString();
                previlage_action_check();
                GetDrpFill();
                Clear();
                if(ServiceId>0)
                {
                    BindData(ServiceId);
                }
                Upd_Add_PanelInner.Update();
            }
        }

        public void GetDrpFill()
        {
            DataSet ds = obj_master.DrpfillForServicePage();

            DataTable dt = ds.Tables[3];
            DepartmentRFValidator.Enabled = Convert.ToBoolean(dt.Rows[0]["DepartmentRequiredInService"]);
            CategoryRFValidator.Enabled = Convert.ToBoolean(dt.Rows[0]["CategoryRequiredInService"]);
            SubCategoryRFValidator.Enabled = Convert.ToBoolean(dt.Rows[0]["SubCategoryRequiredInService"]);
            DepartmentSpan.Visible = Convert.ToBoolean(dt.Rows[0]["DepartmentRequiredInService"]);
            CategorySpan.Visible = tdCat.Visible = Convert.ToBoolean(dt.Rows[0]["CategoryRequiredInService"]);
            SubCategorySpan.Visible = tdSubcat.Visible = Convert.ToBoolean(dt.Rows[0]["SubCategoryRequiredInService"]);

            drp_serCat.Items.Clear();
            drp_serCat.DataSource = ds.Tables[0];
            drp_serCat.DataTextField = "Name";
            drp_serCat.DataValueField = "Id";
            drp_serCat.DataBind();

            drpDepartment.Items.Clear();
            drpDepartment.DataSource = ds.Tables[1];
            drpDepartment.DataTextField = "Name";
            drpDepartment.DataValueField = "Id";
            drpDepartment.DataBind();

            drpDocument.Items.Clear();
            drpDocument.DataSource = ds.Tables[2];
            drpDocument.DataTextField = "Name";
            drpDocument.DataValueField = "Id";
            drpDocument.DataBind();

            int val = obj_common.Form_Previlage_Validation(32, Convert.ToInt32(hdn_user_id.Value));
            if (val == 1)
            {
                RadComboBoxItem CodeItem = new RadComboBoxItem();
                CodeItem.Text = "New Entry";
                CodeItem.Value = "0";
                drpDepartment.Items.Insert(0, CodeItem);
            }
        }

       public void BindData(int ServiceId)
        {
            Clear();
            DataSet ds = obj_master.Edit_Service(ServiceId);

            DataTable dt = ds.Tables[0];
            lbl_code.Text = dt.Rows[0]["Code"].ToString();
            txt_name.Text = dt.Rows[0]["Name"].ToString();
            txt_nameArabic.Text = dt.Rows[0]["NameInArabic"].ToString();
            drp_serCat.SelectedValue = dt.Rows[0]["ServiceCategoryId"].ToString();
            drp_serCat_OnSelectedIndexChanged(null, null);
            drpSerSubCategory.SelectedValue = dt.Rows[0]["ServiceSubCategoryId"].ToString();
            drpDepartment.SelectedValue = dt.Rows[0]["DepartmentId"].ToString();
            txt_price.Text = dt.Rows[0]["Price"].ToString();
            txt_tax.Text = dt.Rows[0]["Tax"].ToString();
            chk_incApp.Checked = Convert.ToBoolean(dt.Rows[0]["IncentiveApplicable"]);
            chk_enable.Checked = Convert.ToBoolean(dt.Rows[0]["Enable"]);
            txt_desc.Text = dt.Rows[0]["Remark"].ToString();
            chkValidity.Checked = Convert.ToBoolean(dt.Rows[0]["Validity"]);
            chkValidity_OnCheckedChanged(null, null);
            txtValidityExpiresOn.Text = dt.Rows[0]["ValidityExpiresOn"].ToString();
            chkrefund.Checked = Convert.ToBoolean(dt.Rows[0]["IsRefundable"]);
            chkIsSetZeroPaidAmt.Checked = Convert.ToBoolean(dt.Rows[0]["IsSetZeroPaidAmt"]);
            drpDocument.SelectedValue = dt.Rows[0]["DocumentId"].ToString();
            chkIsSCNotRequired.Checked = Convert.ToBoolean(dt.Rows[0]["IsSCNotRequired"]);

            DataTable dt_serDetail = ds.Tables[1];
            rpt_serdetail.DataSource = dt_serDetail;
            rpt_serdetail.DataBind();
            hdn_id.Value = ServiceId.ToString();

            DataTable dtfollwdetail = ds.Tables[2];
            if (dtfollwdetail.Rows.Count == 0)
                dtfollwdetail.Rows.Add(0, null);
            rptsubservice.DataSource = dtfollwdetail;
            rptsubservice.DataBind();

            btn_delete.Visible = hdn_delete.Value == "0" ? false : true;
            btn_save.Visible = hdn_update.Value == "0" ? false : true;
        }

        protected void rptsubservice_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField hdnDepartmentId = (HiddenField)e.Item.FindControl("hdnDepartmentId");
            RadComboBox drpDepartIn = (RadComboBox)e.Item.FindControl("drpDepartIn");
            drpDepartIn.Items.Clear();
            drpDepartIn.DataSource = obj_master.Drp_Department();
            drpDepartIn.DataValueField = "Value";
            drpDepartIn.DataTextField = "Text";
            drpDepartIn.DataBind();
            drpDepartIn.SelectedValue = hdnDepartmentId.Value;

            HiddenField hdnsubserviceId = (HiddenField)e.Item.FindControl("hdnsubserviceId");
            RadComboBox drpSubserviceIn = (RadComboBox)e.Item.FindControl("drpSubserviceIn");
            drpSubserviceIn.Items.Clear();
            drpSubserviceIn.DataSource = obj_master.DrpServicebyDepartment(hdnDepartmentId.Value==""?(int?)null : 
                Convert.ToInt32(hdnDepartmentId.Value) );
            drpSubserviceIn.DataValueField = "Id";
            drpSubserviceIn.DataTextField = "Name";
            drpSubserviceIn.DataBind();
            drpSubserviceIn.SelectedValue = hdnsubserviceId.Value;
        }

        protected void rptsubservice_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            DataTable dtdetail = new DataTable();
            dtdetail.Columns.Add("Id", typeof(int));
            dtdetail.Columns.Add("DepartmentId", typeof(int));
            dtdetail.Columns.Add("SubServiceId", typeof(int));
            dtdetail.Columns.Add("DeadlineDays", typeof(int));

            foreach (RepeaterItem itm in rptsubservice.Items)
            {
                HiddenField hdnDId = (HiddenField)itm.FindControl("hdnDId");
                RadComboBox drpDepartIn = (RadComboBox)itm.FindControl("drpDepartIn");
                RadComboBox drpSubserviceIn = (RadComboBox)itm.FindControl("drpSubserviceIn");
                TextBox txtDays = (TextBox)itm.FindControl("txtDays");
                if (drpSubserviceIn.SelectedValue != "" && txtDays.Text != "")
                    dtdetail.Rows.Add(Convert.ToInt32(hdnDId.Value), drpDepartIn.SelectedValue == "" ? (int?)null :
                        Convert.ToInt32(drpDepartIn.SelectedValue), Convert.ToInt32(drpSubserviceIn.SelectedValue),
                        Convert.ToInt32(txtDays.Text));
            }

            if (e.CommandName == "Add")
                dtdetail.Rows.Add(0, null);
            else if (e.CommandName == "Delete")
            {
                dtdetail.Rows.RemoveAt(e.Item.ItemIndex);
                if (dtdetail.Rows.Count == 0)
                    dtdetail.Rows.Add(0, null);
            }

            rptsubservice.DataSource = dtdetail;
            rptsubservice.DataBind();

            updSubservice.Update();
        }

        protected void drpDepartIn_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            RadComboBox drpSubserviceIn = (RadComboBox)itemrp.FindControl("drpSubserviceIn");
            RadComboBox drpDepartIn = (RadComboBox)itemrp.FindControl("drpDepartIn");
            UpdatePanel updSubserviceIn = (UpdatePanel)itemrp.FindControl("updSubserviceIn");

            drpSubserviceIn.Items.Clear();
            drpSubserviceIn.DataSource = obj_master.DrpServicebyDepartment(drpDepartIn.SelectedValue == "" ? (int?)null :
                Convert.ToInt32(drpDepartIn.SelectedValue));
            drpSubserviceIn.DataValueField = "Id";
            drpSubserviceIn.DataTextField = "Name";
            drpSubserviceIn.DataBind();

            updSubserviceIn.Update();
        }

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            int res = 0;
            DataTable dt_serDetail = fill_ServiceDetail();

            DataTable dtfollwdetail = new DataTable();
            dtfollwdetail.Columns.Add("Id", typeof(int));
            dtfollwdetail.Columns.Add("SubServiceId", typeof(int));
            dtfollwdetail.Columns.Add("DepartmentId", typeof(int));
            dtfollwdetail.Columns.Add("DeadlineDays", typeof(int));

            foreach (RepeaterItem itm in rptsubservice.Items)
            {
                HiddenField hdnDId = (HiddenField)itm.FindControl("hdnDId");
                RadComboBox drpDepartIn = (RadComboBox)itm.FindControl("drpDepartIn");
                RadComboBox drpSubserviceIn = (RadComboBox)itm.FindControl("drpSubserviceIn");
                TextBox txtDays = (TextBox)itm.FindControl("txtDays");
                if (drpSubserviceIn.SelectedValue != "" && txtDays.Text != "")
                    dtfollwdetail.Rows.Add(Convert.ToInt32(hdnDId.Value), Convert.ToInt32(drpSubserviceIn.SelectedValue),
                        drpDepartIn.SelectedValue == "" ? (int?)null :Convert.ToInt32(drpDepartIn.SelectedValue), 
                        Convert.ToInt32(txtDays.Text));
            }

            res = obj_master.Insert_Update_Service(Convert.ToInt32(hdn_id.Value), txt_name.Text,
                txt_nameArabic.Text, drp_serCat.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_serCat.SelectedValue), 
                drpSerSubCategory.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpSerSubCategory.SelectedValue),
                drpDepartment.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpDepartment.SelectedValue), Convert.ToDecimal(txt_price.Text), Convert.ToDecimal(txt_tax.Text),
                Convert.ToInt32(chk_incApp.Checked), Convert.ToInt32(chk_enable.Checked), txt_desc.Text,dt_serDetail,
                Convert.ToInt32(chkValidity.Checked), chkValidity.Checked == true ? Convert.ToInt32(txtValidityExpiresOn.Text) : (int?)null,
                Convert.ToInt32(hdn_user_id.Value), dtfollwdetail,Convert.ToInt32(chkrefund.Checked),Convert.ToInt32(chkIsSetZeroPaidAmt.Checked),
                drpDocument.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpDocument.SelectedValue), Convert.ToInt32(chkIsSCNotRequired.Checked));
            if (res >0)
            {
                Clear();
            }

            if (hdnPageId.Value == "1")
            {
                ((Service)this.Page).DisplayMsg(res);

                Panel pnl_add = (Panel)this.Parent.FindControl("pnl_add");
                UpdatePanel Upd_Add_Panel = (UpdatePanel)this.Parent.FindControl("Upd_Add_Panel");

                pnl_add.Visible = false;
                Upd_Add_Panel.Update();
            }
            else if(hdnPageId.Value=="2")
            {
                ((Invoice)this.Page).fillServices(res);

                Panel pnlServiceAdd = (Panel)this.Parent.FindControl("pnlServiceAdd");
                UpdatePanel UpdServicepnlAdd = (UpdatePanel)this.Parent.FindControl("UpdServicepnlAdd");

                pnlServiceAdd.Visible = false;
                UpdServicepnlAdd.Update();
            }
            else if (hdnPageId.Value == "3")
            {
                ((Quotation)this.Page).fillServices(res);

                Panel pnlServiceAdd = (Panel)this.Parent.FindControl("pnlServiceAdd");
                UpdatePanel UpdServicepnlAdd = (UpdatePanel)this.Parent.FindControl("UpdServicepnlAdd");

                pnlServiceAdd.Visible = false;
                UpdServicepnlAdd.Update();
            }
        }

        public DataTable fill_ServiceDetail()
        {
            DataTable dt_serDetail = new DataTable();
            dt_serDetail.Columns.Add("SerDetailId", typeof(int));
            dt_serDetail.Columns.Add("ExpenseId", typeof(int));
            dt_serDetail.Columns.Add("Amount", typeof(decimal));
            dt_serDetail.Columns.Add("VAT", typeof(decimal));
            dt_serDetail.Columns.Add("VendorId", typeof(int));
            dt_serDetail.Columns.Add("PayModeId", typeof(int));
            dt_serDetail.Columns.Add("AccountId", typeof(int));
            dt_serDetail.Columns.Add("TaxExempt", typeof(int));
            dt_serDetail.Columns.Add("vendorCommission", typeof(decimal));

            if (rpt_serdetail.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_serdetail.Items)
                {
                    HiddenField hdn_serDetailId = (HiddenField)itm.FindControl("hdn_serDetailId");
                    RadComboBox drp_expense = (RadComboBox)itm.FindControl("drp_expense");
                    TextBox txt_amt = (TextBox)itm.FindControl("txt_amt");
                    TextBox txt_vat = (TextBox)itm.FindControl("txt_vat");
                    RadComboBox drp_vendor = (RadComboBox)itm.FindControl("drp_vendor");
                    RadComboBox drp_payMode = (RadComboBox)itm.FindControl("drp_payMode");
                    RadComboBox drp_account = (RadComboBox)itm.FindControl("drp_account");
                    CheckBox chk_taxExempt = (CheckBox)itm.FindControl("chk_taxExempt");
                    TextBox txtvendorCommission = (TextBox)itm.FindControl("txtvendorCommission");

                    if (drp_expense.SelectedValue != "")
                    {
                        dt_serDetail.Rows.Add(Convert.ToInt32(hdn_serDetailId.Value), Convert.ToInt32(drp_expense.SelectedValue),
                           txt_amt.Text == "" ? 0 : Convert.ToDecimal(txt_amt.Text), txt_vat.Text == "" ? 0 : Convert.ToDecimal(txt_vat.Text),
                            drp_vendor.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_vendor.SelectedValue),
                            drp_payMode.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_payMode.SelectedValue),
                            drp_account.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_account.SelectedValue),
                            Convert.ToInt32(chk_taxExempt.Checked), 
                            txtvendorCommission.Text==""?0:Convert.ToDecimal(txtvendorCommission.Text) ) ;
                    }
                }
            }
            return dt_serDetail;
        }

        protected void btn_delete_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Delete_Service(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                Clear();
            }

            if (hdnPageId.Value == "1")
            {
                ((Service)this.Page).DisplayMsg(res);

                Panel pnl_add = (Panel)this.Parent.FindControl("pnl_add");
                UpdatePanel Upd_Add_Panel = (UpdatePanel)this.Parent.FindControl("Upd_Add_Panel");

                pnl_add.Visible = false;
                Upd_Add_Panel.Update();
            }
        }

        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
        }

        protected void drpDepartment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (drpDepartment.SelectedValue == "0")
            {
                pnlDepartment.Visible = true;
                UC_Department.PageLoad();
                UpdDepartmentPanel.Update();
            }
        }

        protected void drpDocument_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (drpDocument.SelectedValue == "0")
            {
                pnlDepartment.Visible = true;
                UC_Department.PageLoad();
                UpdDepartmentPanel.Update();
            }
        }

        protected void btnexpense_OnClick(object sender, EventArgs e)
        {
                pnlExpense.Visible = true;
                UC_Expense.UCPageLoad(2);
                UpdExpensePanel.Update();
        }

        protected void btnvendorOnClick(object sender, EventArgs e)
        {
            pnlVendor.Visible = true;
            UC_Vendor.UCPageLoad(1);
            UpdVendorPanel.Update();
        }

        protected void drp_serCat_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            drpSerSubCategory.ClearSelection();
            drpSerSubCategory.Text = "";
            drpSerSubCategory.Items.Clear();
            DataTable dt = obj_master.Drp_SerSubCategory_Filetr_SerCategory(drp_serCat.SelectedValue == "" ? 0 : Convert.ToInt32(drp_serCat.SelectedValue));
            drpSerSubCategory.DataSource = dt;
            drpSerSubCategory.DataTextField = "Text";
            drpSerSubCategory.DataValueField = "Value";
            drpSerSubCategory.DataBind();

            Upd_SerSubCategory_Panel.Update();
        }

        protected void chkValidity_OnCheckedChanged(object sender, EventArgs e)
        {
            txtValidityExpiresOn.Text = "";
            pnl_Validity.Visible = chkValidity.Checked;
            Upd_Validity_Panel.Update();
            pnl_document.Visible = chkValidity.Checked;
            Upd_Document_Panel.Update();
        }

        protected void rpt_serdetail_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField hdn_expenseId = (HiddenField)e.Item.FindControl("hdn_expenseId");
            RadComboBox drp_expense = (RadComboBox)e.Item.FindControl("drp_expense");
            drp_expense.Items.Clear();
            DataTable dtExpense = obj_master.Drp_Expense();
            drp_expense.DataSource = dtExpense;
            drp_expense.DataValueField = "Value";
            drp_expense.DataTextField = "Text";
            drp_expense.DataBind();
            drp_expense.SelectedValue = hdn_expenseId.Value;

            HiddenField hdn_vendorId = (HiddenField)e.Item.FindControl("hdn_vendorId");
            RadComboBox drp_vendor = (RadComboBox)e.Item.FindControl("drp_vendor");
            drp_vendor.Items.Clear();
            DataTable dtVendor = obj_master.Drp_Vendor();
            drp_vendor.DataSource = dtVendor;
            drp_vendor.DataValueField = "Value";
            drp_vendor.DataTextField = "Text";
            drp_vendor.DataBind();
            drp_vendor.SelectedValue = hdn_vendorId.Value;

            HiddenField hdn_payModeId = (HiddenField)e.Item.FindControl("hdn_payModeId");
            RadComboBox drp_payMode = (RadComboBox)e.Item.FindControl("drp_payMode");
            drp_payMode.Items.Clear();
            DataTable dtPayMode = obj_master.Drp_PaymentMode_WithoutCredit();
            drp_payMode.DataSource = dtPayMode;
            drp_payMode.DataValueField = "Value";
            drp_payMode.DataTextField = "Text";
            drp_payMode.DataBind();
            drp_payMode.SelectedValue = hdn_payModeId.Value;
            drp_payMode.Items.Remove(drp_payMode.Items.FindItemByValue("2"));/*Remove Cheque*/

            HiddenField hdn_accountId = (HiddenField)e.Item.FindControl("hdn_accountId");
            RadComboBox drp_account = (RadComboBox)e.Item.FindControl("drp_account");
            RequiredFieldValidator rqdaccountIn = (RequiredFieldValidator)e.Item.FindControl("rqdaccountIn");

            drp_account.Items.Clear();
            if (hdn_payModeId.Value != "")
            {
                DataTable dtAccount = obj_master.Drp_Account_Filter_PayMode(Convert.ToInt32(hdn_payModeId.Value));
                drp_account.DataSource = dtAccount;
                drp_account.DataValueField = "Value";
                drp_account.DataTextField = "Text";
                drp_account.DataBind();
            }
            drp_account.SelectedValue = hdn_accountId.Value;
            if (drp_payMode.SelectedValue == "7" || drp_payMode.SelectedValue == "8" || drp_payMode.SelectedValue == "9")   //topup,customercard, payinglater 
            {
                rqdaccountIn.Enabled = false;
            }
            HiddenField hdn_taxExempt = (HiddenField)e.Item.FindControl("hdn_taxExempt");
            CheckBox chk_taxExempt = (CheckBox)e.Item.FindControl("chk_taxExempt");
            chk_taxExempt.Checked = Convert.ToBoolean(Convert.ToInt32(hdn_taxExempt.Value));
        }

        protected void btn_serDetail_newEntry_OnClick(object sender, EventArgs e)
        {
            DataTable dt_serDetail = new DataTable();
            dt_serDetail.Columns.Add("SerDetailId", typeof(int));
            dt_serDetail.Columns.Add("ExpenseId", typeof(int));
            dt_serDetail.Columns.Add("Amount", typeof(decimal));
            dt_serDetail.Columns.Add("VAT", typeof(decimal));
            dt_serDetail.Columns.Add("VendorId", typeof(int));
            dt_serDetail.Columns.Add("PayModeId", typeof(int));
            dt_serDetail.Columns.Add("AccountId", typeof(int));
            dt_serDetail.Columns.Add("TaxExempt", typeof(int));
            dt_serDetail.Columns.Add("vendorCommission", typeof(decimal));

            if (rpt_serdetail.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_serdetail.Items)
                {
                    HiddenField hdn_serDetailId = (HiddenField)itm.FindControl("hdn_serDetailId");
                    RadComboBox drp_expense = (RadComboBox)itm.FindControl("drp_expense");
                    TextBox txt_amt = (TextBox)itm.FindControl("txt_amt");
                    TextBox txt_vat = (TextBox)itm.FindControl("txt_vat");
                    RadComboBox drp_vendor = (RadComboBox)itm.FindControl("drp_vendor");
                    RadComboBox drp_payMode = (RadComboBox)itm.FindControl("drp_payMode");
                    RadComboBox drp_account = (RadComboBox)itm.FindControl("drp_account");
                    CheckBox chk_taxExempt = (CheckBox)itm.FindControl("chk_taxExempt");
                    TextBox txtvendorCommission = (TextBox)itm.FindControl("txtvendorCommission");

                    if (drp_expense.SelectedValue != "")
                    {
                        dt_serDetail.Rows.Add(Convert.ToInt32(hdn_serDetailId.Value), Convert.ToInt32(drp_expense.SelectedValue),
                            Convert.ToDecimal(txt_amt.Text), Convert.ToDecimal(txt_vat.Text),
                            drp_vendor.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_vendor.SelectedValue),
                            drp_payMode.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_payMode.SelectedValue),
                            drp_account.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_account.SelectedValue), 
                            Convert.ToInt32(chk_taxExempt.Checked),
                             txtvendorCommission.Text == "" ? (decimal?)null : Convert.ToDecimal(txtvendorCommission.Text));
                    }
                }
            }

            dt_serDetail.Rows.Add(-(rpt_serdetail.Items.Count + 1), null, 0.00, 0.00, null, null, null, 0);
            rpt_serdetail.DataSource = dt_serDetail;
            rpt_serdetail.DataBind();


            Upd_ItemList.Update();
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

            drp_account.Items.Clear();
            if (drp.SelectedValue != "")
            {
                DataTable dtAccount = obj_master.Drp_Account_Filter_PayMode(Convert.ToInt32(drp.SelectedValue));
                drp_account.DataSource = dtAccount;
                drp_account.DataValueField = "Value";
                drp_account.DataTextField = "Text";
                drp_account.DataBind();
            }
            if (drp.SelectedValue == "7" || drp.SelectedValue == "8" || drp.SelectedValue == "9") //topup,customercard, payinglater 
            {
                rqdaccountIn.Enabled = false;
            }
            hdn_accountId.Value = "";
            drp_account.ClearSelection();
            drp_account.Text = "";
            Upd_Account_Panel.Update();
        }

        protected void btn_remove_line_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            DataTable dt_serDetail = new DataTable();
            dt_serDetail.Columns.Add("SerDetailId", typeof(int));
            dt_serDetail.Columns.Add("ExpenseId", typeof(int));
            dt_serDetail.Columns.Add("Amount", typeof(decimal));
            dt_serDetail.Columns.Add("VAT", typeof(decimal));
            dt_serDetail.Columns.Add("VendorId", typeof(int));
            dt_serDetail.Columns.Add("PayModeId", typeof(int));
            dt_serDetail.Columns.Add("AccountId", typeof(int));
            dt_serDetail.Columns.Add("TaxExempt", typeof(int));
            dt_serDetail.Columns.Add("vendorCommission", typeof(decimal));

            if (rpt_serdetail.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_serdetail.Items)
                {
                    HiddenField hdn_serDetailId = (HiddenField)itm.FindControl("hdn_serDetailId");
                    RadComboBox drp_expense = (RadComboBox)itm.FindControl("drp_expense");
                    TextBox txt_amt = (TextBox)itm.FindControl("txt_amt");
                    TextBox txt_vat = (TextBox)itm.FindControl("txt_vat");
                    RadComboBox drp_vendor = (RadComboBox)itm.FindControl("drp_vendor");
                    RadComboBox drp_payMode = (RadComboBox)itm.FindControl("drp_payMode");
                    RadComboBox drp_account = (RadComboBox)itm.FindControl("drp_account");
                    CheckBox chk_taxExempt = (CheckBox)itm.FindControl("chk_taxExempt");
                    TextBox txtvendorCommission = (TextBox)itm.FindControl("txtvendorCommission");

                    if (drp_expense.SelectedValue != "")
                    {
                        dt_serDetail.Rows.Add(Convert.ToInt32(hdn_serDetailId.Value), Convert.ToInt32(drp_expense.SelectedValue),
                            Convert.ToDecimal(txt_amt.Text), Convert.ToDecimal(txt_vat.Text),
                            drp_vendor.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_vendor.SelectedValue),
                            drp_payMode.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_payMode.SelectedValue),
                            drp_account.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_account.SelectedValue),
                            Convert.ToInt32(chk_taxExempt.Checked),
                             txtvendorCommission.Text == "" ? (decimal?)null : Convert.ToDecimal(txtvendorCommission.Text));
                    }
                }
            }
            dt_serDetail.Rows.RemoveAt(itemrp.ItemIndex);
            if (dt_serDetail.Rows.Count == 0)
                dt_serDetail.Rows.Add(-1, null, 0.00, 0.00, null, null, null, 0);
            rpt_serdetail.DataSource = dt_serDetail;
            rpt_serdetail.DataBind();


            Upd_ItemList.Update();
        }

        protected void btn_close_OnClick(object sender, EventArgs e)
        {
            if (hdnPageId.Value == "1")
            {
                Panel pnl_add = (Panel)this.Parent.FindControl("pnl_add");
                UpdatePanel Upd_Add_Panel = (UpdatePanel)this.Parent.FindControl("Upd_Add_Panel");

                pnl_add.Visible = false;
                Upd_Add_Panel.Update();
            }
            else if (hdnPageId.Value == "2")
            {
                Panel pnlServiceAdd = (Panel)this.Parent.FindControl("pnlServiceAdd");
                UpdatePanel UpdServicepnlAdd = (UpdatePanel)this.Parent.FindControl("UpdServicepnlAdd");

                pnlServiceAdd.Visible = false;
                UpdServicepnlAdd.Update();
            }
            else if (hdnPageId.Value == "3")
            {
                Panel pnlServiceAdd = (Panel)this.Parent.FindControl("pnlServiceAdd");
                UpdatePanel UpdServicepnlAdd = (UpdatePanel)this.Parent.FindControl("UpdServicepnlAdd");

                pnlServiceAdd.Visible = false;
                UpdServicepnlAdd.Update();
            }
        }

        public void Clear()
        {
            Get_Code();
            txt_name.Text = "";
            txt_nameArabic.Text = "";
            drp_serCat.ClearSelection();
            drp_serCat.Text = "";
            drp_serCat_OnSelectedIndexChanged(null, null);

            drpDepartment.ClearSelection();
            drpDepartment.Text = "";
            drpDocument.ClearSelection();
            drpDocument.Text = "";
            txt_price.Text = "";
            txt_tax.Text = "";
            chk_incApp.Checked =chkrefund.Checked= chkIsSCNotRequired.Checked= false;
            chk_enable.Checked = true;
            txt_desc.Text = "";
            txtValidityExpiresOn.Text = "";
            chkValidity.Checked = false;
            chkValidity_OnCheckedChanged(null, null);
            DataTable dt_serDetail = new DataTable();
            dt_serDetail.Columns.Add("SerDetailId", typeof(int));
            dt_serDetail.Columns.Add("ExpenseId", typeof(int));
            dt_serDetail.Columns.Add("Amount", typeof(decimal));
            dt_serDetail.Columns.Add("VAT", typeof(decimal));
            dt_serDetail.Columns.Add("VendorId", typeof(int));
            dt_serDetail.Columns.Add("PayModeId", typeof(int));
            dt_serDetail.Columns.Add("AccountId", typeof(int));
            dt_serDetail.Columns.Add("TaxExempt", typeof(int));
            dt_serDetail.Columns.Add("vendorCommission", typeof(decimal));

            dt_serDetail.Rows.Add(-1, null, 0.00, 0.00, null, null, null, 0);
            rpt_serdetail.DataSource = dt_serDetail;
            rpt_serdetail.DataBind();

            hdn_id.Value = "0";

            DataTable dtdetail = new DataTable();
            dtdetail.Columns.Add("Id", typeof(int));
            dtdetail.Columns.Add("DepartmentId", typeof(int));
            dtdetail.Columns.Add("SubServiceId", typeof(int));
            dtdetail.Columns.Add("DeadlineDays", typeof(int));
            dtdetail.Rows.Add(0, null);

            rptsubservice.DataSource = dtdetail;
            rptsubservice.DataBind();

            btn_delete.Visible = false;
            btn_save.Visible = hdn_add.Value == "0" ? false : true;

            int val = obj_common.Form_Previlage_Validation(7, Convert.ToInt32(hdn_user_id.Value));
            btnexpense.Visible = (val == 1) ? true : false;

            Upd_Add_PanelInner.Update();
        }

        public void Get_Code()
        {
            DataTable dt = obj_common.Get_Code(14);
            if (dt.Rows.Count > 0)
                lbl_code.Text = dt.Rows[0][0].ToString();
        }
       
        public void previlage_action_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    DataTable dt = obj_common.Action_Previlage_Validation(14, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdn_update.Value = dt.Rows[1][1].ToString();
                        hdn_delete.Value = dt.Rows[2][1].ToString();
                    }
                    btn_save.Visible = hdn_add.Value == "0" ? false : true;
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