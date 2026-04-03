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

namespace AmarCentre.Transactions.UserControl
{
    public partial class UCPV : System.Web.UI.UserControl
    {
        Master_Bal obj_master = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
        Voucher BalVoucher = new Voucher();
        DataTable dtCustomer;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void UCPageLoad(int PageId, int PVId, string filter = "",int Count=10)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
            }
            else
            {
                hdnPageId.Value = PageId.ToString();    // 1-PV , 2-home
                hdn_user_id.Value = Session["User_Id"].ToString();
                hdnfilter.Value = filter;
                hdnCount.Value = Count.ToString();

                fillGeneralExpenseList();
                previlage_action_check();
                Clear();
                if (PVId > 0)
                {
                    BindData(PVId);
                }
            }
        }
        public void fillGeneralExpenseList()
        {
            DataTable dt = BalVoucher.GetExpenseList();
            drpExpenseType.DataSource = dt;
            drpExpenseType.DataValueField = "Value";
            drpExpenseType.DataTextField = "Text";
            drpExpenseType.DataBind();

            int val = obj_common.Form_Previlage_Validation(7, Convert.ToInt32(hdn_user_id.Value));
            if (val == 1)
            {
                RadComboBoxItem CodeItem = new RadComboBoxItem();
                CodeItem.Text = "New Entry";
                CodeItem.Value = "0";
                drpExpenseType.Items.Insert(0, CodeItem);
            }

            dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Text", typeof(string));
            for (int i = 1; i <= 60; i++)
                dt.Rows.Add(i, i.ToString());
            drpDepreciationPeriod.DataSource = dt;
            drpDepreciationPeriod.DataValueField = "Id";
            drpDepreciationPeriod.DataTextField = "Text";
            drpDepreciationPeriod.DataBind();
        }

        public void BindData(int PVId)
        {
            Clear();
            DataSet ds = BalVoucher.Edit_PaymentVoucher(PVId);
            DataTable dt = ds.Tables[0];
            hdn_id.Value = dt.Rows[0]["Id"].ToString();
            lblCode.Text = dt.Rows[0]["Code"].ToString();
            dtdated.DbSelectedDate = dt.Rows[0]["Date"].ToString();
            drpTo.SelectedValue = dt.Rows[0]["ToType"].ToString();
            drpToOnSelectedIndexChanged(null, null);
            if (drpTo.SelectedValue == "1" || drpTo.SelectedValue == "8")
                drpCustomer.SelectedValue = dt.Rows[0]["CustomerId"].ToString();
            else if (drpTo.SelectedValue == "2")
                drpVendor.SelectedValue = dt.Rows[0]["VendorId"].ToString();
            else if (drpTo.SelectedValue == "3")
            {
                drpEmpSubType.SelectedValue = dt.Rows[0]["EmployeeSubType"].ToString();
                drpEmployee.SelectedValue = dt.Rows[0]["EmployeeId"].ToString();
                drpEmployeeOnSelectedIndexChanged(null, null);
            }
            else if (drpTo.SelectedValue == "4")
                drpPettyCash.SelectedValue = dt.Rows[0]["CashAccountId"].ToString();
            else if (drpTo.SelectedValue == "5")
                drpBankAccount.SelectedValue = dt.Rows[0]["BankAccountId"].ToString();
            else if (drpTo.SelectedValue == "6")
                drpVendor.SelectedValue = dt.Rows[0]["VendorId"].ToString();
            else if (drpTo.SelectedValue == "7")
                drpLoan.SelectedValue = dt.Rows[0]["LoanId"].ToString();
            else if (drpTo.SelectedValue == "13")
            {
                drpCustomer.SelectedValue = dt.Rows[0]["CustomerId"].ToString();
                drpCustomerOnSelectedIndexChanged(null, null);
            }
            else if (drpTo.SelectedValue == "9")
            {
                drpPartner.SelectedValue = dt.Rows[0]["PartnerId"].ToString();
                drpPartnerOnSelectedIndexChanged(null, null);
            }
            else if (drpTo.SelectedValue == "10")
                drpDeposit.SelectedValue = dt.Rows[0]["DepositId"].ToString();
            else if (drpTo.SelectedValue == "12")
            {
                drpAgent.SelectedValue = dt.Rows[0]["AgentId"].ToString();
                drpAgent_SelectedIndexChanged(null, null);
                lblPayable.Text = hdnPayable.Value != "" ?
                    (Convert.ToDecimal(hdnPayable.Value) + Convert.ToDecimal(dt.Rows[0]["Amount"])).ToString() :
                     dt.Rows[0]["Amount"].ToString();
                hdnPayable.Value = lblPayable.Text;
            }
            drpExpenseType.SelectedValue = dt.Rows[0]["ExpenseType"].ToString();
            drpDepreciationPeriod.SelectedValue = dt.Rows[0]["DepressionPeriod"].ToString();
            drpFromType.SelectedValue = dt.Rows[0]["FromType"].ToString();
            drpFromTypeOnSelectedIndexChanged(null, null);
            if (drpFromType.SelectedValue == "1")
            {
                drpPettyCashFrom.SelectedValue = dt.Rows[0]["CashAccountIdFrom"].ToString();
                drpPettyCashFromOnSelectedIndexChanged(null, null);
            }
            else if (drpFromType.SelectedValue == "5")
            {
                drpLoanFrom.SelectedValue = dt.Rows[0]["LoanFromId"].ToString();
                drpLoanFrom_SelectedIndexChanged(null, null);
            }
            else
            {
                fillBankAccountEdit(Convert.ToInt32(dt.Rows[0]["BankAccountIdFrom"].ToString()));
                drpBankAccountFrom.SelectedValue = dt.Rows[0]["BankAccountIdFrom"].ToString();
                drpBankAccountFromOnSelectedIndexChanged(null, null);
            }
            txtAmountMain.Text = dt.Rows[0]["Amount"].ToString();
            txtCommission.Text = dt.Rows[0]["Commission"].ToString();
            drpTaxType.SelectedValue = dt.Rows[0]["TaxType"].ToString();
            txtTax.Text = dt.Rows[0]["DisplayTax"].ToString();
            dtChequeDate.DbSelectedDate = dt.Rows[0]["ChequeDate"].ToString();
            txtTransaction.Text = dt.Rows[0]["TransactionDetails"].ToString();
            txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
            hdnfilenameup.Value = dt.Rows[0]["Filenames"].ToString();
            hdnfilenamesaveup.Value = dt.Rows[0]["FilenamesSave"].ToString();
            lblfileupl.Text = dt.Rows[0]["Filenames"].ToString();
            txtBillNo.Text = dt.Rows[0]["BillNo"].ToString();

            btnSave.Visible = false;
            btnSavePrint.Visible = false;
            btnPrint.Visible = hdn_print.Value == "0" ? false : true;
            btnOpenCancel.Visible = false;
            btnOpenDelete.Visible = false;
            if (dt.Rows[0]["Status"].ToString() == "1")
            {
                btnOpenCancel.Visible = hdn_cancel.Value == "0" ? false : true;
                //btnOpenDelete.Visible = hdn_delete.Value == "0" ? false : true;
                btnSave.Visible = hdn_update.Value == "0" ? false : true;
                btnSavePrint.Visible = hdn_update_N_print.Value == "0" ? false : true;
            }
            if (dt.Rows[0]["IsAllowEdit"].ToString() == "0")
                btnSave.Visible = btnSavePrint.Visible = false;

            Upd_Add_Panel.Update();
        }
        protected void drpToOnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblPayable.Text = hdnPayable.Value = "";
            tblPay.Visible = false;
            updAccountDetails.Update();
            txtAmountMain.Text = "";
            txtAmountMain.ReadOnly = false;
            updAmountMain.Update();

            lblEmpSubType.Visible = false;
            drpEmpSubType.Visible = false;
            drpEmpSubType.ClearSelection();
            drpEmpSubType.Text = "";
            rqdEmpSubtype.Enabled = false;
            drpDepreciationPeriod.ClearSelection();
            drpDepreciationPeriod.Text = "";
            divDepreciatn.Visible = false;
            updempSubtype.Update();
            pnlBillNo.Visible = false;
            txtBillNo.Text = "";

            drpCustomer.Items.Clear();
            drpCustomer.Visible = false;
            drpCustomer.Text = "";

            drpVendor.Items.Clear();
            drpVendor.Visible = false;
            drpVendor.Text = "";

            drpEmployee.Items.Clear();
            drpEmployee.Visible = false;
            drpEmployee.Text = "";

            drpPettyCash.Items.Clear();
            drpPettyCash.Visible = false;
            drpPettyCash.Text = "";

            drpBankAccount.Items.Clear();
            drpBankAccount.Visible = false;
            drpBankAccount.Text = "";

            drpLoan.Items.Clear();
            drpLoan.Visible = false;
            drpLoan.Text = "";

            drpPartner.Items.Clear();
            drpPartner.Visible = false;
            drpPartner.Text = "";

            drpDeposit.Items.Clear();
            drpDeposit.Visible = false;
            drpDeposit.Text = lblToLabel.Text = "";

            drpAgent.Items.Clear();
            drpAgent.Visible = false;
            drpAgent.Text = "";

            drpSupplier.Items.Clear();
            drpSupplier.Visible = false;
            drpSupplier.Text = "";

            rqSource.ValidationGroup = "no";
            rqSource.ControlToValidate = "drpBankAccount";
            UpdTo.Update();

            divVdeposit.Visible = false;
            rptVdeposit.DataSource = null;
            rptVdeposit.DataBind();
            updVdeposit.Update();

            if (drpTo.SelectedValue != "")

            {
                if (drpTo.SelectedValue == "1" || drpTo.SelectedValue == "13") //Customer , depositreturn
                {
                    dtCustomer = new DataTable();
                    dtCustomer = BalVoucher.GetCustomer();
                    drpCustomer.DataSource = dtCustomer;
                    drpCustomer.DataValueField = "Value";
                    drpCustomer.DataTextField = "Text";
                    drpCustomer.DataBind();
                    drpCustomer.Visible = true;

                    lblToLabel.Text = "Customer Name";
                    lblToLabel.Visible = true;
                    rqSource.ValidationGroup = "save";
                    rqSource.ControlToValidate = "drpCustomer";

                    UpdTo.Update();

                }
                else
                    if (drpTo.SelectedValue == "2")  //Vendor
                {
                    DataTable dt = new DataTable();
                    drpVendor.Visible = true;
                    dt = BalVoucher.GetVendorList();
                    drpVendor.DataSource = dt;
                    drpVendor.DataValueField = "Value";
                    drpVendor.DataTextField = "Text";
                    drpVendor.DataBind();

                    lblToLabel.Text = "Vendor Name";
                    lblToLabel.Visible = true;
                    rqSource.ValidationGroup = "save";
                    rqSource.ControlToValidate = "drpVendor";

                    UpdTo.Update();
                }
                else if (drpTo.SelectedValue == "3")        //Employee
                {
                    //DataTable dt = new DataTable();
                    //dt = BalVoucher.GetEmployeeList();
                    drpEmployee.DataSource = BalVoucher.DrpEmployeeTrans(Convert.ToInt32(hdn_id.Value));
                    drpEmployee.DataValueField = "Value";
                    drpEmployee.DataTextField = "Text";
                    drpEmployee.DataBind();
                    drpEmployee.Visible = true;

                    lblToLabel.Text = "Employee Name";
                    lblToLabel.Visible = true;
                    rqSource.ValidationGroup = "save";
                    rqSource.ControlToValidate = "drpEmployee";
                    UpdTo.Update();

                    lblEmpSubType.Visible = true;
                    drpEmpSubType.Visible = true;
                    drpEmpSubType.ClearSelection();
                    drpEmpSubType.Text = "";
                    drpEmpSubType.SelectedValue = "2";
                    rqdEmpSubtype.Enabled = true;
                    updempSubtype.Update();

                }
                else if (drpTo.SelectedValue == "4")
                {
                    DataTable dt = new DataTable();
                    dt = BalVoucher.GetAllPettyCashAccountList();
                    drpPettyCash.DataSource = dt;
                    drpPettyCash.DataValueField = "Value";
                    drpPettyCash.DataTextField = "Text";
                    drpPettyCash.DataBind();
                    drpPettyCash.Visible = true;

                    lblToLabel.Text = "Petty Cash Name";
                    lblToLabel.Visible = true;
                    rqSource.ValidationGroup = "save";
                    rqSource.ControlToValidate = "drpPettyCash";

                    UpdTo.Update();
                }
                else if (drpTo.SelectedValue == "5")
                {
                    DataTable dt = new DataTable();
                    dt = BalVoucher.GetAllBankAccountList();
                    drpBankAccount.DataSource = dt;
                    drpBankAccount.DataValueField = "Value";
                    drpBankAccount.DataTextField = "Text";
                    drpBankAccount.DataBind();
                    drpBankAccount.Visible = true;

                    lblToLabel.Text = "Bank Name";
                    lblToLabel.Visible = true;
                    rqSource.ValidationGroup = "save";
                    rqSource.ControlToValidate = "drpBankAccount";

                    UpdTo.Update();
                }
                else if (drpTo.SelectedValue == "7")  //loan
                {
                    DataTable dt = new DataTable();
                    dt = BalVoucher.GetLoan();
                    drpLoan.DataSource = dt;
                    drpLoan.DataValueField = "Value";
                    drpLoan.DataTextField = "Text";
                    drpLoan.DataBind();
                    drpLoan.Visible = true;

                    lblToLabel.Text = "Loan Name";
                    lblToLabel.Visible = true;
                    rqSource.ValidationGroup = "save";
                    rqSource.ControlToValidate = "drpLoan";

                    UpdTo.Update();
                }
                else if (drpTo.SelectedValue == "6")  // general
                {
                    divDepreciatn.Visible = true;

                    drpVendor.Visible = true;
                    drpVendor.DataSource = BalVoucher.GetVendorList();
                    drpVendor.DataValueField = "Value";
                    drpVendor.DataTextField = "Text";
                    drpVendor.DataBind();

                    lblToLabel.Text = "Vendor Name";
                    lblToLabel.Visible = true;

                    pnlBillNo.Visible = true;

                    UpdTo.Update();
                }
                else if (drpTo.SelectedValue == "8")
                {
                    dtCustomer = new DataTable();
                    dtCustomer = BalVoucher.GetCommissionApplicableCustomer();
                    drpCustomer.DataSource = dtCustomer;
                    drpCustomer.DataValueField = "Value";
                    drpCustomer.DataTextField = "Text";
                    drpCustomer.DataBind();
                    drpCustomer.Visible = true;

                    lblToLabel.Text = "Customer Name";
                    lblToLabel.Visible = true;
                    rqSource.ValidationGroup = "save";
                    rqSource.ControlToValidate = "drpCustomer";

                    UpdTo.Update();

                }

                else if (drpTo.SelectedValue == "9")
                {
                    drpPartner.DataSource = BalVoucher.GetPartnerList();
                    drpPartner.DataValueField = "Value";
                    drpPartner.DataTextField = "Text";
                    drpPartner.DataBind();
                    drpPartner.Visible = true;

                    lblToLabel.Text = "Partner";
                    lblToLabel.Visible = true;
                    rqSource.ValidationGroup = "save";
                    rqSource.ControlToValidate = "drpPartner";

                    UpdTo.Update();
                }

                else if (drpTo.SelectedValue == "10") //deposit
                {
                    drpDeposit.DataSource = BalVoucher.GetDepositTypeList();
                    drpDeposit.DataValueField = "Value";
                    drpDeposit.DataTextField = "Text";
                    drpDeposit.DataBind();
                    drpDeposit.Visible = true;

                    lblToLabel.Text = "Deposit Type";
                    lblToLabel.Visible = true;
                    rqSource.ValidationGroup = "save";
                    rqSource.ControlToValidate = "drpDeposit";

                    UpdTo.Update();
                }
                else if (drpTo.SelectedValue == "11") //vat
                {
                    DataTable dt = BalVoucher.GetTaxAmount();
                    lblPayable.Text = hdnPayable.Value = dt.Rows[0]["TaxPayable"].ToString();
                    tblPay.Visible = true;
                    updAccountDetails.Update();
                }
                else if (drpTo.SelectedValue == "12") //Agent
                {
                    drpAgent.DataSource = BalVoucher.fill_drp_Agent();
                    drpAgent.DataValueField = "Value";
                    drpAgent.DataTextField = "Text";
                    drpAgent.DataBind();
                    drpAgent.Visible = true;

                    lblToLabel.Text = "Agent";
                    lblToLabel.Visible = true;
                    rqSource.ValidationGroup = "save";
                    rqSource.ControlToValidate = "drpAgent";

                    UpdTo.Update();
                }
            }

            updBillNo.Update();
        }

        protected void drpEmployeeOnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpEmployee.SelectedValue != "")
            {
                if (drpEmpSubType.SelectedValue == "1")
                {
                    DataTable dt = BalVoucher.GetEmployeeById(Convert.ToInt32(drpEmployee.SelectedValue));
                    lblPayable.Text = hdnPayable.Value = dt.Rows[0]["IncentiveAmount"].ToString();
                }
                else
                {
                    DataTable dt = BalVoucher.GetEmployeeById(Convert.ToInt32(drpEmployee.SelectedValue));
                    lblPayable.Text = hdnPayable.Value = dt.Rows[0]["Payable"].ToString();
                }
                tblPay.Visible = true;
                updAccountDetails.Update();
            }
            else
            {
                lblPayable.Text = hdnPayable.Value = "";
                tblPay.Visible = false;
                updAccountDetails.Update();
            }
        }

        protected void drpCustomerOnSelectedIndexChanged(object sender, EventArgs e)
        {
            divVdeposit.Visible = false;
            updVdeposit.Update();

            if (drpCustomer.SelectedValue != "" && drpTo.SelectedValue == "13")
            {
                if (hdn_id.Value == "0")
                {
                    divVdeposit.Visible = true;
                    DataTable dt = BalVoucher.CdepositList(Convert.ToInt32(drpCustomer.SelectedValue));
                    rptVdeposit.DataSource = dt;
                    rptVdeposit.DataBind();

                    decimal outsum = 0;
                    foreach (DataRow r in dt.Rows)
                        outsum += Convert.ToDecimal(r["Balance"].ToString());
                    txtVdepositTotAmt.Text = outsum.ToString();
                }
                else
                {
                    divVdeposit.Visible = true;
                    DataTable dt = BalVoucher.EditCdepositList(Convert.ToInt32(drpCustomer.SelectedValue), Convert.ToInt32(hdn_id.Value));
                    rptVdeposit.DataSource = dt;
                    rptVdeposit.DataBind();

                    decimal outsum = 0;
                    foreach (DataRow r in dt.Rows)
                        outsum += Convert.ToDecimal(r["Balance"].ToString());
                    txtVdepositTotAmt.Text = outsum.ToString();
                }
                updVdeposit.Update();
            }
            else if (drpCustomer.SelectedValue != "" && drpTo.SelectedValue == "1")
            {
                DataSet ds = obj_master.Edit_Customer(Convert.ToInt32(drpCustomer.SelectedValue));
                lblPayable.Text = hdnPayable.Value = ds.Tables[0].Rows[0]["TotalPayable"].ToString();
                tblPay.Visible = true;
                updAccountDetails.Update();
            }
            else if (drpCustomer.SelectedValue != "" && drpTo.SelectedValue == "8")
            {
                DataTable dt = BalVoucher.getCustomerCommissionBalance(Convert.ToInt32(drpCustomer.SelectedValue));
                lblPayable.Text = hdnPayable.Value = dt.Rows[0]["Amount"].ToString();
                tblPay.Visible = true;
                updAccountDetails.Update();
            }
            else
            {
                lblPayable.Text = hdnPayable.Value = "";
                tblPay.Visible = false;
                updAccountDetails.Update();
            }
        }

        protected void drpVendorOnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpVendor.SelectedValue != "")
            {
                DataSet ds = obj_master.Edit_Vendor(Convert.ToInt32(drpVendor.SelectedValue));
                lblPayable.Text = hdnPayable.Value = ds.Tables[0].Rows[0]["Payable"].ToString();
                tblPay.Visible = true;
                updAccountDetails.Update();
            }
            else
            {
                lblPayable.Text = hdnPayable.Value = "";
                tblPay.Visible = false;
                updAccountDetails.Update();
            }
        }

        protected void drpLoanOnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpLoan.SelectedValue != "")
            {
                DataTable dt = obj_master.Edit_Loan(Convert.ToInt32(drpLoan.SelectedValue));
                lblPayable.Text = hdnPayable.Value = dt.Rows[0]["Payable"].ToString();
                tblPay.Visible = true;
                updAccountDetails.Update();
            }
            else
            {
                lblPayable.Text = hdnPayable.Value = "";
                tblPay.Visible = false;
                updAccountDetails.Update();
            }
        }

        protected void drpPartnerOnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpPartner.SelectedValue != "")
            {
                DataSet ds = obj_master.EditShareholder(Convert.ToInt32(drpPartner.SelectedValue));
                lblPayable.Text = hdnPayable.Value = ds.Tables[0].Rows[0]["Balance"].ToString();

                tblPay.Visible = true;
                updAccountDetails.Update();
            }
            else
            {
                lblPayable.Text = hdnPayable.Value = "";
                tblPay.Visible = false;
                updAccountDetails.Update();
            }
        }

        protected void drpFromTypeOnSelectedIndexChanged(object sender, EventArgs e)
        {
            drpLoanFrom.DataSource = "";
            drpLoanFrom.DataBind();
            drpLoanFrom.Visible = false;
            drpLoanFrom.ClearSelection();
            drpLoanFrom.Text = "";

            if (drpFromType.SelectedValue != "")

            {
                if (drpFromType.SelectedValue == "1")
                {
                    DataTable dt = new DataTable();
                    dt = BalVoucher.GetPettyCashAccountList(Convert.ToInt32(hdn_user_id.Value));
                    drpPettyCashFrom.DataSource = dt;
                    drpPettyCashFrom.DataValueField = "Value";
                    drpPettyCashFrom.DataTextField = "Text";
                    drpPettyCashFrom.DataBind();
                    drpPettyCashFrom.Visible = true;
                    drpPettyCashFrom.ClearSelection();
                    drpPettyCashFrom.Text = "";


                    drpBankAccountFrom.DataSource = "";
                    drpBankAccountFrom.DataBind();
                    drpBankAccountFrom.Visible = false;
                    drpBankAccountFrom.ClearSelection();
                    drpBankAccountFrom.Text = "";

                    lblFromLabel.Text = "Cash Name";
                    lblFromLabel.Visible = true;
                    rqFrom.ValidationGroup = "save";
                    rqFrom.ControlToValidate = "drpPettyCashFrom";
                    UpdFrom.Update();

                    lblChequeDate.Visible = false;
                    dtChequeDate.DbSelectedDate = "";
                    dtChequeDate.Visible = false;
                    rqChequeDate.ValidationGroup = "no";
                    UpdCheque.Update();
                }
                else if (drpFromType.SelectedValue == "2")
                {
                    DataTable dt = new DataTable();
                    dt = BalVoucher.GetBankAccountList(Convert.ToInt32(hdn_user_id.Value));
                    drpBankAccountFrom.DataSource = dt;
                    drpBankAccountFrom.DataValueField = "Value";
                    drpBankAccountFrom.DataTextField = "Text";
                    drpBankAccountFrom.DataBind();
                    drpBankAccountFrom.Visible = true;
                    drpBankAccountFrom.ClearSelection();
                    drpBankAccountFrom.Text = "";


                    drpPettyCashFrom.DataSource = "";
                    drpPettyCashFrom.DataBind();
                    drpPettyCashFrom.Visible = false;
                    drpPettyCashFrom.ClearSelection();
                    drpPettyCashFrom.Text = "";

                    lblFromLabel.Text = "Bank Name";
                    lblFromLabel.Visible = true;
                    rqFrom.ValidationGroup = "save";
                    rqFrom.ControlToValidate = "drpBankAccountFrom";
                    UpdFrom.Update();

                    lblChequeDate.Visible = false;
                    dtChequeDate.DbSelectedDate = "";
                    dtChequeDate.Visible = false;
                    rqChequeDate.ValidationGroup = "no";
                    UpdCheque.Update();
                }
                else if (drpFromType.SelectedValue == "3")
                {


                    DataTable dt = new DataTable();
                    dt = BalVoucher.GetBankAccountList(Convert.ToInt32(hdn_user_id.Value));
                    drpBankAccountFrom.DataSource = dt;
                    drpBankAccountFrom.DataValueField = "Value";
                    drpBankAccountFrom.DataTextField = "Text";
                    drpBankAccountFrom.DataBind();
                    drpBankAccountFrom.Visible = true;
                    drpBankAccountFrom.ClearSelection();
                    drpBankAccountFrom.Text = "";

                    drpPettyCashFrom.DataSource = "";
                    drpPettyCashFrom.DataBind();
                    drpPettyCashFrom.Visible = false;
                    drpPettyCashFrom.ClearSelection();
                    drpPettyCashFrom.Text = "";

                    lblFromLabel.Text = "Bank Name";
                    lblFromLabel.Visible = true;
                    rqFrom.ValidationGroup = "save";
                    rqFrom.ControlToValidate = "drpBankAccountFrom";
                    UpdFrom.Update();

                    lblChequeDate.Visible = true;
                    dtChequeDate.DbSelectedDate = "";
                    dtChequeDate.Visible = true;
                    rqChequeDate.ValidationGroup = "save";
                    UpdCheque.Update();
                }
                else if (drpFromType.SelectedValue == "5")
                {
                    drpPettyCashFrom.DataSource = "";
                    drpPettyCashFrom.DataBind();
                    drpPettyCashFrom.Visible = false;
                    drpPettyCashFrom.ClearSelection();
                    drpPettyCashFrom.Text = "";

                    drpBankAccountFrom.DataSource = "";
                    drpBankAccountFrom.DataBind();
                    drpBankAccountFrom.Visible = false;
                    drpBankAccountFrom.ClearSelection();
                    drpBankAccountFrom.Text = "";

                    drpLoanFrom.DataSource = BalVoucher.GetLoan();
                    drpLoanFrom.DataValueField = "Value";
                    drpLoanFrom.DataTextField = "Text";
                    drpLoanFrom.DataBind();
                    drpLoanFrom.Visible = true;

                    lblFromLabel.Text = "Loan Name";
                    lblFromLabel.Visible = true;
                    rqFrom.ValidationGroup = "save";
                    rqFrom.ControlToValidate = "drpLoanFrom";

                    lblChequeDate.Visible = false;
                    dtChequeDate.DbSelectedDate = "";
                    dtChequeDate.Visible = false;
                    rqChequeDate.ValidationGroup = "no";
                    UpdCheque.Update();
                }
            }
            else
            {
                drpPettyCashFrom.DataSource = "";
                drpPettyCashFrom.DataBind();
                drpPettyCashFrom.Visible = false;
                drpPettyCashFrom.ClearSelection();
                drpPettyCashFrom.Text = "";


                drpBankAccountFrom.DataSource = "";
                drpBankAccountFrom.DataBind();
                drpBankAccountFrom.Visible = false;
                drpBankAccountFrom.ClearSelection();
                drpBankAccountFrom.Text = "";

                lblFromLabel.Text = "Petty Cash Name";
                lblFromLabel.Visible = false;
                rqFrom.ValidationGroup = "no";
                rqFrom.ControlToValidate = "drpPettyCashFrom";
                UpdFrom.Update();

                lblChequeDate.Visible = false;
                dtChequeDate.DbSelectedDate = "";
                dtChequeDate.Visible = false;
                rqChequeDate.ValidationGroup = "no";
                UpdCheque.Update();
            }

            UpdFrom.Update();
        }
        public void fillBankAccountEdit(int AccountId)
        {
            DataTable dt = new DataTable();
            dt = BalVoucher.GetBankAccountListEdit(Convert.ToInt32(hdn_user_id.Value), AccountId);
            drpBankAccountFrom.DataSource = dt;
            drpBankAccountFrom.DataValueField = "Value";
            drpBankAccountFrom.DataTextField = "Text";
            drpBankAccountFrom.DataBind();
            drpBankAccountFrom.Visible = true;
            drpBankAccountFrom.ClearSelection();
            drpBankAccountFrom.Text = "";
        }
        protected void drpPettyCashFromOnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblBalance.Text = "";
            if (drpPettyCashFrom.SelectedValue != "")
            {
                if (drpTo.SelectedValue == "4" & (drpPettyCashFrom.SelectedValue == drpPettyCash.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('From and to account cannot be same.!');", true);

                    drpPettyCash.ClearSelection();
                    drpPettyCash.Text = "";
                    UpdTo.Update();
                }
                else
                {
                    DataTable dt = BalVoucher.GetPettycashById(Convert.ToInt32(drpPettyCashFrom.SelectedValue));
                    lblBalance.Text = dt.Rows[0]["Balance"].ToString();
                }
            }
            updAccountDetails.Update();
        }

        protected void drpBankAccountFromOnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblBalance.Text = "";
            if (drpBankAccountFrom.SelectedValue != "")
            {
                if (drpTo.SelectedValue == "5" & (drpBankAccountFrom.SelectedValue == drpBankAccount.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('From and to account cannot be same.!');", true);

                    drpBankAccount.ClearSelection();
                    drpBankAccount.Text = "";
                    UpdTo.Update();
                }
                else
                {
                    DataTable dt = BalVoucher.GetBankAccountById(Convert.ToInt32(drpBankAccountFrom.SelectedValue));
                    lblBalance.Text = dt.Rows[0]["Balance"].ToString();
                }
            }
            updAccountDetails.Update();
        }

        protected void drpLoanFrom_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            lblBalance.Text = "";
            if (drpLoanFrom.SelectedValue != "")
            {
                DataTable dt = obj_master.Edit_Loan(Convert.ToInt32(drpLoanFrom.SelectedValue));
                lblBalance.Text = dt.Rows[0]["Balance"].ToString();
            }
            updAccountDetails.Update();
        }

        protected void drpExpenseTypeOnSelectedIndexChanged(object sender, EventArgs e)
        {
            txtTax.Text = "";
            if (drpExpenseType.SelectedValue != "")
            {
                if (drpExpenseType.SelectedValue == "0")
                {
                    pnlExpense.Visible = true;
                    UC_Expense.UCPageLoad(1);
                    UpdExpensePanel.Update();
                }
                else
                {
                    DataTable dt = BalVoucher.GetExpenseTaxAmount(Convert.ToInt32(drpExpenseType.SelectedValue));
                    if (dt.Rows.Count > 0)
                        txtTax.Text = dt.Rows[0]["Tax"].ToString();
                }
            }
            UpdTaxPanel.Update();
        }

        protected void drpAgent_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            txtAmountMain.Text = "";
            if (drpAgent.SelectedValue != "")
            {
                DataTable dtpay = BalVoucher.AgentProfitBalance(Convert.ToInt32(drpAgent.SelectedValue));
                lblPayable.Text = hdnPayable.Value = (dtpay.Rows.Count > 0) ? dtpay.Rows[0][0].ToString() : "0";
                tblPay.Visible = true;
            }
            else
            {
                lblPayable.Text = hdnPayable.Value = "";
                tblPay.Visible = false;
            }
            updAccountDetails.Update();
            updAmountMain.Update();
        }

        protected void lblfileupl_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnfilenamesaveup.Value != "")
                {
                    string fil_name = hdnfilenamesaveup.Value;
                    string full_name = Server.MapPath("~/UploadedFiles/" + fil_name);
                    Response.ContentType = "APPLICATION/OCTET-STREAM";
                    String Header = "Attachment; Filename=\"" + lblfileupl.Text + "\"";
                    Response.AppendHeader("Content-Disposition", Header);
                    System.IO.FileInfo Dfile = new System.IO.FileInfo(Server.MapPath("~/UploadedFiles/" + fil_name));
                    Response.WriteFile(Dfile.FullName);
                    //Don't forget to add the following line
                    Response.End();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void fu_FilesOnFileUploaded(object sender, FileUploadedEventArgs e)
        {
            fu_Files.TargetFolder = "~/UploadedFiles";

            foreach (UploadedFile upfile in fu_Files.UploadedFiles)
            {
                DataTable dtprefix = obj_common.Get_File_Code("AllFile");
                string files_namesave = dtprefix.Rows[0][0].ToString() + upfile.FileName;
                upfile.SaveAs(Path.Combine(Server.MapPath(fu_Files.TargetFolder), files_namesave));
                try
                {
                    //in backup folder also
                    DataTable dtgen = obj_master.Edit_GeneralSettings();
                    File.Copy((Path.Combine(Server.MapPath(fu_Files.TargetFolder), files_namesave)),
                        (Path.Combine(dtgen.Rows[0]["BackupDrivePath"].ToString() + "UploadedFiles", files_namesave)), false);
                }
                catch (Exception cc) { }
                hdnfilenameup.Value = upfile.FileName;
                hdnfilenamesaveup.Value = files_namesave;
            }

            Updfu_Files.Update();
        }

        public int SavePV()
        {
            DataTable dtvdeposit = new DataTable();
            dtvdeposit.Columns.Add("InvoiceId", typeof(int));
            dtvdeposit.Columns.Add("InvoiceDetId", typeof(int));
            dtvdeposit.Columns.Add("Pay", typeof(decimal));

            foreach (RepeaterItem itm in rptVdeposit.Items)
            {
                HiddenField hdninvoiceId = (HiddenField)itm.FindControl("hdninvoiceId");
                HiddenField hdninvdetId = (HiddenField)itm.FindControl("hdninvdetId");
                TextBox txtVdepositPayAmt = (TextBox)itm.FindControl("txtVdepositPayAmt");
                CheckBox chk_select = (CheckBox)itm.FindControl("chk_select");

                if (chk_select.Checked)
                {
                    dtvdeposit.Rows.Add(Convert.ToInt32(hdninvoiceId.Value), hdninvdetId.Value == "" ? (int?)null : Convert.ToInt32(hdninvdetId.Value),
                        Convert.ToDecimal(txtVdepositPayAmt.Text));
                }
            }

            int res = -1;
            if (Convert.ToInt32(drpTo.SelectedValue) == 1 && hdnPayable.Value != "" && Convert.ToDecimal(txtAmountMain.Text) > Convert.ToDecimal(hdnPayable.Value))
            {
                lblAlertCommn.Text = "Amount Cannot be greater than Payable Amount!";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Amount Cannot be greater than Payable Amount!');", true);
            }
            else
            {
                if (drpFromType.SelectedValue == "1" && drpPettyCashFrom.SelectedValue == "")
                {
                    lblAlertCommn.Text = "Select cash account.!";
                    pnlAlertCommn.Visible = true;
                    updAlertCommn.Update();
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Select cash account.!');", true);
                }
                else if (drpFromType.SelectedValue == "2" && drpBankAccountFrom.SelectedValue == "")
                {
                    lblAlertCommn.Text = "Select bank account.!";
                    pnlAlertCommn.Visible = true;
                    updAlertCommn.Update();
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Select bank account.!');", true);
                }
                else if (drpTo.SelectedValue == "13" && dtvdeposit.Rows.Count == 0)
                {
                    lblAlertCommn.Text = "Select Invoice details.!";
                    pnlAlertCommn.Visible = true;
                    updAlertCommn.Update();
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Select Invoice details.!');", true);
                }
                else
                {
                    int? nulls = null;
                    DateTime? nulldate = null;
                    res = BalVoucher.PaymentVoucherInsertUpdate(Convert.ToInt32(hdn_id.Value), DateTime.ParseExact(CalDate(dtdated), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                       Convert.ToInt32(drpTo.SelectedValue),
                       drpCustomer.SelectedValue != "" ? Convert.ToInt32(drpCustomer.SelectedValue) : nulls,
                        drpVendor.SelectedValue != "" ? Convert.ToInt32(drpVendor.SelectedValue) : nulls,
                         drpEmployee.SelectedValue != "" ? Convert.ToInt32(drpEmployee.SelectedValue) : nulls,
                        drpPettyCash.SelectedValue != "" ? Convert.ToInt32(drpPettyCash.SelectedValue) : nulls,
                         drpBankAccount.SelectedValue != "" ? Convert.ToInt32(drpBankAccount.SelectedValue) : nulls,
                         drpLoan.SelectedValue != "" ? Convert.ToInt32(drpLoan.SelectedValue) : nulls,
                         Convert.ToInt32(drpExpenseType.SelectedValue), Convert.ToInt32(drpFromType.SelectedValue),
                          drpPettyCashFrom.SelectedValue != "" ? Convert.ToInt32(drpPettyCashFrom.SelectedValue) : nulls,
                           drpBankAccountFrom.SelectedValue != "" ? Convert.ToInt32(drpBankAccountFrom.SelectedValue) : nulls,
                           Convert.ToDouble(txtAmountMain.Text), txtCommission.Text != "" ? Convert.ToDouble(txtCommission.Text) : 0,
                           txtTax.Text != "" ? Convert.ToDouble(txtTax.Text) : 0, dtChequeDate.DbSelectedDate != null ? DateTime.ParseExact(CalDate(dtChequeDate), "dd/MM/yyyy", CultureInfo.InvariantCulture) : nulldate, drpFromType.SelectedValue == "3" ? 1 : 0, nulldate,
                           txtTransaction.Text, txtRemarks.Text, Convert.ToInt32(drpTaxType.SelectedValue), Convert.ToInt32(hdn_user_id.Value),
                           drpEmpSubType.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpEmpSubType.SelectedValue),
                           drpPartner.SelectedValue != "" ? Convert.ToInt32(drpPartner.SelectedValue) : nulls,
                           drpDeposit.SelectedValue != "" ? Convert.ToInt32(drpDeposit.SelectedValue) : nulls,
                           drpTo.SelectedValue == "6" ? ((drpDepreciationPeriod.SelectedValue == "" && drpDepreciationPeriod.Text != "") ? Convert.ToInt32(drpDepreciationPeriod.Text) :
                           (drpDepreciationPeriod.SelectedValue != "" ? Convert.ToInt32(drpDepreciationPeriod.SelectedValue) : nulls)) : nulls,
                           drpTo.SelectedValue == "12" ? Convert.ToInt32(drpAgent.SelectedValue) : (int?)null, dtvdeposit,
                            drpFromType.SelectedValue == "5" ? Convert.ToInt32(drpLoanFrom.SelectedValue) : (int?)null,
                            drpSupplier.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpSupplier.SelectedValue), 
                            hdnfilenameup.Value, hdnfilenamesaveup.Value, txtBillNo.Text);
                }
            }
            return res;
        }

        public void Save(object sender, EventArgs e)
        {
            int res = SavePV();
            if (res > 0)
            {
                Clear();
                lblAlertCommn.Text = "Saved Successfully !..";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
                //lbl_msgin.Text = "Saved Successfully !..";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else if (res == 0)
            {
                lblAlertCommn.Text = "Sorry Failed to Process Your Request !..";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
                //lbl_msgin.Text = "Sorry Failed to Process Your Request !..";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            Upd_Add_PanelInner.Update();
        }

        public void btnSavePrintOnClick(object sender, EventArgs e)
        {
            int res = SavePV();
            if (res > 0)
            {
                Clear();
                lblAlertCommn.Text = "Saved Successfully !..";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
                //lbl_msgin.Text = "Saved Successfully !..";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                string url = "";
                url = "../Reports/PaymentVoucher.aspx?id=" + res;
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
            }
            else if (res == 0)
            {
                lblAlertCommn.Text = "Sorry Failed to Process Your Request !..";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
                //lbl_msgin.Text = "Sorry Failed to Process Your Request !..";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            Upd_Add_PanelInner.Update();
        }

        protected void btnPrintOnClick(object sender, EventArgs e)
        {
            string url = "";
            url = "../Reports/PaymentVoucher.aspx?id=" + Convert.ToInt32(hdn_id.Value);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        protected void btnReset_OnClick(object sender, EventArgs e)
        {
            Clear();
        }

        protected void btn_close_OnClick(object sender, EventArgs e)
        {
            if (hdnPageId.Value == "1")  //RV
            {
                Panel pnl_add = (Panel)this.Parent.FindControl("pnl_add");
                UpdatePanel Upd_Add_Panel = (UpdatePanel)this.Parent.FindControl("Upd_Add_Panel");
                ((PaymentVoucher)this.Page).grid_fill(1, Convert.ToInt32(hdnCount.Value), hdnfilter.Value, "", "");

                pnl_add.Visible = false;
                Upd_Add_Panel.Update();
            }
            else if (hdnPageId.Value == "2")  //home
            {
                Panel pnlPVadd = (Panel)this.Parent.FindControl("pnlPVadd");
                UpdatePanel UpdPVadd = (UpdatePanel)this.Parent.FindControl("UpdPVadd");

                pnlPVadd.Visible = false;
                UpdPVadd.Update();
            }
        }

        protected void btnAlertCloseOnClick(object sender, EventArgs e)
        {
            lblAlertCommn.Text = "";
            pnlAlertCommn.Visible = false;
            updAlertCommn.Update();
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            CancelDeletePaymentVoucher(2);
        }

        protected void btnDelete_OnClick(object sender, EventArgs e)
        {
            CancelDeletePaymentVoucher(3);
        }

        public void CancelDeletePaymentVoucher(int Status)
        {
            int res = BalVoucher.CancelDeletePaymentVoucher(Convert.ToInt32(hdn_id.Value), Status, txtCancelRemark.Text, Convert.ToInt32(hdn_user_id.Value));
            if (res > 0)
            {
                Clear();
                lblAlertCommn.Text = "Saved Successfully !..";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
                //lbl_msgin.Text = "Saved Successfully !..";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lblAlertCommn.Text = "Sorry Failed to Process Your Request !..";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
                //lbl_msgin.Text = "Sorry Failed to Process Your Request !..";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            Upd_Add_PanelInner.Update();
            txtCancelRemark.Text = "";
            pnlCancel.Visible = false;
            updCancel.Update();
        }

        protected void btnOpenDelete_OnClick(object sender, EventArgs e)
        {
            lblCancel.Text = "Delete Payment Voucher";
            txtCancelRemark.Text = "";
            btnCancel.Visible = false;
            btnDelete.Visible = true;
            pnlCancel.Visible = true;
            updCancel.Update();
        }

        protected void btnOpenCancel_OnClick(object sender, EventArgs e)
        {
            lblCancel.Text = "Cancel Payment Voucher";
            txtCancelRemark.Text = "";
            btnCancel.Visible = true;
            btnDelete.Visible = false;
            pnlCancel.Visible = true;
            updCancel.Update();
        }

        protected void btnCloseCancel_OnClick(object sender, EventArgs e)
        {
            txtCancelRemark.Text = "";
            pnlCancel.Visible = false;
            updCancel.Update();
        }

        public void Clear()
        {
            hdn_id.Value = "0";
            dtdated.DbSelectedDate = DateTime.Now;
            drpTo.ClearSelection();
            hdnfilenamesaveup.Value = hdnfilenameup.Value = lblfileupl.Text = "";
            drpTo.Text = "";
            drpToOnSelectedIndexChanged(null, null);
            drpCustomer.ClearSelection();
            drpCustomer.Text = "";
            drpVendor.ClearSelection();
            drpVendor.Text = "";
            drpEmployee.ClearSelection();
            drpEmployee.Text = "";
            drpPettyCash.ClearSelection();
            drpPettyCash.Text = "";
            drpBankAccount.ClearSelection();
            drpBankAccount.Text = "";
            drpLoan.ClearSelection();
            drpLoan.Text = "";
            drpExpenseType.ClearSelection();
            drpExpenseType.Text = "";
            drpPartner.ClearSelection();
            drpPartner.Text = "";
            drpFromType.ClearSelection();
            drpFromType.Text = "";
            drpFromTypeOnSelectedIndexChanged(null, null);
            drpPettyCashFrom.ClearSelection();
            drpPettyCashFrom.Text = "";
            drpBankAccountFrom.ClearSelection();
            drpBankAccountFrom.Text = "";
            txtAmountMain.ReadOnly = false;
            txtAmountMain.Text = "";
            drpTaxType.SelectedValue = "1";
            txtTax.Text = "";
            txtCommission.Text = "0";
            dtChequeDate.DbSelectedDate = "";
            txtTransaction.Text = "";
            txtRemarks.Text = "";
            lblBalance.Text = "";
            lblFromLabel.Text = "";
            lblFromLabel.Visible = false;
            lblToLabel.Text = "";
            lblToLabel.Visible = false;
            lblPayable.Text = hdnPayable.Value = "";
            tblPay.Visible = false;
            divVdeposit.Visible = false;
            rptVdeposit.DataSource = null;
            rptVdeposit.DataBind();
            updVdeposit.Update();

            btnSave.Visible = hdn_add.Value == "0" ? false : true;
            btnSavePrint.Visible = hdn_add_N_print.Value == "0" ? false : true;
            btnPrint.Visible = false;
            btnOpenCancel.Visible = false;
            btnOpenDelete.Visible = false;
            Get_Code();

            Upd_Add_PanelInner.Update();
        }

        /*Code for Display*/
        public void Get_Code()
        {
            DataTable dt = obj_common.Get_Code(24);
            if (dt.Rows.Count > 0)
                lblCode.Text = dt.Rows[0][0].ToString();
        }

        public void previlage_action_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    DataTable dt = obj_common.Action_Previlage_Validation(24, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdn_update.Value = dt.Rows[1][1].ToString();
                        hdn_delete.Value = dt.Rows[2][1].ToString();
                        hdn_print.Value = dt.Rows[3][1].ToString();
                        hdn_add_N_print.Value = dt.Rows[4][1].ToString();
                        hdn_update_N_print.Value = dt.Rows[5][1].ToString();
                        hdn_cancel.Value = dt.Rows[6][1].ToString();
                    }
                    btnSave.Visible = hdn_add.Value == "0" ? false : true;
                    btnSavePrint.Visible = hdn_add_N_print.Value == "0" ? false : true;
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