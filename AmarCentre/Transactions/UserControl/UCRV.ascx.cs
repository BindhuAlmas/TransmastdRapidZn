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
using System.Diagnostics.Eventing.Reader;

namespace AmarCentre.Transactions.UserControl
{
    public partial class UCRV : System.Web.UI.UserControl
    {

        Master_Bal obj_master = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
        Voucher BalVoucher = new Voucher();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void UCPageLoad(int PageId, int RVId, string filter = "", int Count = 10)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
            }
            else
            {
                hdnPageId.Value = PageId.ToString();    // 1-RV , 2-home
                hdn_user_id.Value = Session["User_Id"].ToString();
                hdnfilter.Value = filter;
                hdnCount.Value = Count.ToString();

                fillIcnomeTypes();
                previlage_action_check();
                Clear();
                if (RVId > 0)
                {
                    BindData(RVId);
                }
            }
        }

        public void BindData(int RVId)
        {
            Clear();
            DataSet ds = BalVoucher.Edit_ReceiptVoucher(RVId);
            DataTable dt = ds.Tables[0];
            DataTable dtInvoice = ds.Tables[1];
            DataTable dtInvSum = ds.Tables[2];

            DataTable dtInvoiceCI = ds.Tables[3];
            DataTable dtInvSumCI = ds.Tables[4];

            hdn_id.Value = dt.Rows[0]["Id"].ToString();
            lblCode.Text = dt.Rows[0]["Code"].ToString();
            dtdated.DbSelectedDate = dt.Rows[0]["Date"].ToString();
            drpFrom.SelectedValue = dt.Rows[0]["Type"].ToString();
            drpFromOnSelectedIndexChanged(null, null);
            if (drpFrom.SelectedValue == "1" || drpFrom.SelectedValue == "6")
            {
                drpCustomer.SelectedValue = dt.Rows[0]["CustomerId"].ToString();
                drpCustomerPaymentType.SelectedValue = dt.Rows[0]["CustomerPaymentType"].ToString();
                drpCustomerOnSelectedIndexChanged(null, null);
            }
            else if (drpFrom.SelectedValue == "2" || drpFrom.SelectedValue == "8" || drpFrom.SelectedValue == "9")
            {
                drpVendor.SelectedValue = dt.Rows[0]["VendorId"].ToString();
                drpVendorOnSelectedIndexChanged(null, null);
            }
            else if (drpFrom.SelectedValue == "3")
            {
                drpEmployee.SelectedValue = dt.Rows[0]["EmployeeId"].ToString();
                drpEmployeeOnSelectedIndexChanged(null, null);
            }
            else if (drpFrom.SelectedValue == "5")
            {
                drpLoan.SelectedValue = dt.Rows[0]["LoanId"].ToString();
                drpLoanOnSelectedIndexChanged(null, null);
            }
            else if (drpFrom.SelectedValue == "7")
                drpDeposit.SelectedValue = dt.Rows[0]["DepositId"].ToString();
            else if (drpFrom.SelectedValue == "4")
                drpParty.SelectedValue = dt.Rows[0]["PartyId"].ToString();
            else if (drpFrom.SelectedValue == "10")
            {
                drpAsset.SelectedValue= dt.Rows[0]["AssetId"].ToString();
                drpParty.SelectedValue = dt.Rows[0]["PartyId"].ToString();
            }
            else if (drpFrom.SelectedValue == "11" ) //CompanyGroup
            {
                drpCustomerPaymentType.SelectedValue = dt.Rows[0]["CustomerPaymentType"].ToString();
                drpCompanyGroup.SelectedValue = dt.Rows[0]["CompanyGroupId"].ToString();
                drpCompanyGroup_SelectedIndexChanged(null, null);
            }

            drpIncomeType.SelectedValue = dt.Rows[0]["IncomeId"].ToString();
            drpToType.SelectedValue = dt.Rows[0]["ToType"].ToString();
            drpToTypeOnSelectedIndexChanged(null, null);
            txtAmountMain.Text = dt.Rows[0]["Amount"].ToString();
            txtChargedAmt.Text = dt.Rows[0]["ChargedAmount"].ToString();
            drpPettyCash.SelectedValue = dt.Rows[0]["CashAccountId"].ToString();
            if (drpToType.SelectedValue == "2" || drpToType.SelectedValue == "6" || drpToType.SelectedValue == "10")
                fillBankAccountEdit(Convert.ToInt32(dt.Rows[0]["BankAccountId"].ToString()));
            drpBankAccount.SelectedValue = dt.Rows[0]["BankAccountId"].ToString();
            drpLoanAccount.SelectedValue = dt.Rows[0]["LoanAccountId"].ToString();

            onchangedrp_bank(null, null);
            dtChequeDate.DbSelectedDate = dt.Rows[0]["ChequeDate"].ToString();
            txtTransaction.Text = dt.Rows[0]["TransactionDetails"].ToString();
            txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
            hdnfilenameup.Value = dt.Rows[0]["Filenames"].ToString();
            hdnfilenamesaveup.Value = dt.Rows[0]["FilenamesSave"].ToString();
            lblfileupl.Text = dt.Rows[0]["Filenames"].ToString();

            hdnCustomerPaymentType.Value = dt.Rows[0]["CustomerPaymentType"].ToString();
            txt_commsn.Text = dt.Rows[0]["BankCommission"].ToString();
            txttax.Text = dt.Rows[0]["TaxPercentage"].ToString();
            txtCommissionVat.Text = dt.Rows[0]["CommissionVat"].ToString();

            rpt_invoiceList.DataSource = dtInvoice;
            rpt_invoiceList.DataBind();

            //lblTDebitAmount.Text = dtInvSum.Rows[0]["TotalDebitAmount"].ToString();
            lblTReceivableAmount.Text = dtInvSum.Rows[0]["TotalReceivable"].ToString();
            txtOutstandingAmount.Text = dtInvSum.Rows[0]["OutstandingAmount"].ToString();

            rptCustomerInvoice.DataSource = dtInvoiceCI;
            rptCustomerInvoice.DataBind();

            lblTReceivableAmount_CI.Text = dtInvSumCI.Rows[0]["TotalReceivable"].ToString();
            txtOutstandingAmount_CI.Text = dtInvSumCI.Rows[0]["OutstandingAmount"].ToString();


            if (hdnCustomerPaymentType.Value == "1")
            {
                txtTotal.Text = dt.Rows[0]["Amount"].ToString();
                txtAmountMain.ReadOnly = true;
            }
            //else if (hdnCustomerPaymentType.Value == "2")
            //{
            //    txtCITotal.Text = dt.Rows[0]["Amount"].ToString();
            //    txtAmountMain.ReadOnly = true;
            //}
            else
            {
                txtTotal.Text = "";
                txtAmountMain.ReadOnly = false;
            }
            updInvoiceList.Update();
            updCustomerInvoiceList.Update();

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
            if (dt.Rows[0]["IsEnable"].ToString() == "0" && dt.Rows[0]["Invoicecode"].ToString()!="")
            {
                lblenablemsg.Text = "Update & Cancel not Allowed. Check invoice history of "+ dt.Rows[0]["Invoicecode"].ToString()  + " for details. ";
                btnOpenCancel.Visible = btnSave.Visible = btnSavePrint.Visible = false;
            }

            Upd_Add_Panel.Update();
        }


        public void fillIcnomeTypes()
        {
            DataTable dt = BalVoucher.GetIncomeTypeList();
            drpIncomeType.DataSource = dt;
            drpIncomeType.DataValueField = "Value";
            drpIncomeType.DataTextField = "Text";
            drpIncomeType.DataBind();

            int val = obj_common.Form_Previlage_Validation(21, Convert.ToInt32(hdn_user_id.Value));
            if (val == 1)
            {
                RadComboBoxItem CodeItem = new RadComboBoxItem();
                CodeItem.Text = "New Entry";
                CodeItem.Value = "0";
                drpIncomeType.Items.Insert(0, CodeItem);
            }
            DataTable dtgen = obj_master.Edit_GeneralSettings();
            if (dtgen.Rows[0]["IsProfessionVersion"].ToString() == "0")
                drpFrom.Items.Remove(1);
        }

        protected void drpIncomeType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (drpIncomeType.SelectedValue == "0")
            {
                pnlIncome.Visible = true;
                UC_Income.PageLoad();
                UpdIncomePanel.Update();
            }
        }

        protected void drpParty_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (drpParty.SelectedValue == "0")
            {
                PartyPanel.Visible = true;
                UC_Party.UCPageLoad(1);
                updPartyPanel.Update();
            }
        }
        protected void drpAsset_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            lblassetvalue.Text = "";
            if (drpAsset.SelectedValue !="")
            {
                DataSet ds = obj_master.EditFixedAsset(Convert.ToInt32(drpAsset.SelectedValue));
                DataTable dt = ds.Tables[0];
                lblassetvalue.Text ="Current Value : "+ dt.Rows[0]["CurrentValue"].ToString();
            }
            updasset2.Update();
        }
        public void drpFromOnSelectedIndexChanged(Object sender, EventArgs e)
        {
            drpDeposit.Items.Clear();
            drpDeposit.Visible = false;
            drpDeposit.Text = "";

            drpCompanyGroup.Items.Clear();
            drpCompanyGroup.Text = "";
            drpCompanyGroup.Visible = false;

            drpLoan.Items.Clear();
            drpLoan.Visible = false;
            drpLoan.Text = "";

            drpCustomer.Visible = false;
            drpCustomer.Items.Clear();
            drpCustomer.Text = "";

            drpVendor.Visible = false;
            drpVendor.Items.Clear();
            drpVendor.Text = "";

            drpEmployee.Visible = false;
            drpEmployee.Items.Clear();
            drpEmployee.Text = "";

            drpParty.Visible = false;
            drpParty.Items.Clear();
            drpParty.Text = "";

            rqSource.Enabled = false;
            rqSource.ControlToValidate = "drpCustomer";
            lblFromLabel.Visible = false;
            RemoveAdvanceOption();

            divVdeposit.Visible = false;
            rptVdeposit.DataSource = null;
            rptVdeposit.DataBind();
            updVdeposit.Update();

            drpCustomerPaymentType.ClearSelection();
            drpCustomerPaymentType.Text = "";
            divCustomerPaymentType.Visible = false;

            drpAsset.Items.Clear();
            drpAsset.Text = "";
            lblassetvalue.Text = "";
            pnlasset1.Visible = pnlasset2.Visible = drpAssetrqd.Enabled = false;

            if (drpFrom.SelectedValue != "")
            {
                if (drpFrom.SelectedValue == "1" || drpFrom.SelectedValue == "6") //Customer or customer invocie
                {
                    drpToType.Items.Insert(3, new RadComboBoxItem("Advance", "4"));

                    drpCustomer.DataSource = BalVoucher.GetCreditCustomer();
                    drpCustomer.DataValueField = "Value";
                    drpCustomer.DataTextField = "Text";
                    drpCustomer.DataBind();
                    drpCustomer.Visible = true;

                    divCustomerPaymentType.Visible = (drpFrom.SelectedValue == "1") ? true : false;

                    lblFromLabel.Text = "Customer Name/زبون";
                    lblFromLabel.Visible = true;
                    rqSource.ValidationGroup = "save";
                    rqSource.ControlToValidate = "drpCustomer";
                    rqSource.Enabled = true;
                }
                else if (drpFrom.SelectedValue == "11") //Company Group 
                {
                    drpToType.Items.Insert(3, new RadComboBoxItem("Advance", "4"));
                    drpCompanyGroup.DataSource = BalVoucher.FillCompanyGroup();
                    drpCompanyGroup.DataValueField = "Id";
                    drpCompanyGroup.DataTextField = "Name";
                    drpCompanyGroup.DataBind();
                    drpCompanyGroup.Visible = true;
                    drpCompanyGroup.ClearSelection();
                    drpCompanyGroup.Text = "";

                    divCustomerPaymentType.Visible = true;

                  lblFromLabel.Text = "Company";
                    lblFromLabel.Visible = true;
                    rqSource.ValidationGroup = "save";
                    rqSource.ControlToValidate = "drpCompanyGroup";
                }
                else if (drpFrom.SelectedValue == "2" || drpFrom.SelectedValue == "8" || drpFrom.SelectedValue == "9") //vendor , deposit return, commission return
                {
                    drpVendor.Visible = true;
                    drpVendor.DataSource = BalVoucher.GetVendorList();
                    drpVendor.DataValueField = "Value";
                    drpVendor.DataTextField = "Text";
                    drpVendor.DataBind();
                    drpVendor.ClearSelection();

                    lblFromLabel.Text = "Vendor Name/بائع";
                    lblFromLabel.Visible = true;
                    rqSource.ValidationGroup = "save";
                    rqSource.ControlToValidate = "drpVendor";
                    rqSource.Enabled = true;
                }
                else if (drpFrom.SelectedValue == "3")
                {
                    drpEmployee.DataSource = BalVoucher.DrpEmployeeTrans(Convert.ToInt32(hdn_id.Value)); // BalVoucher.GetEmployeeList();
                    drpEmployee.DataValueField = "Value";
                    drpEmployee.DataTextField = "Text";
                    drpEmployee.DataBind();
                    drpEmployee.Visible = true;
                    drpEmployee.ClearSelection();

                    lblFromLabel.Text = "Employee Name/موظف";
                    lblFromLabel.Visible = true;
                    rqSource.ValidationGroup = "save";
                    rqSource.ControlToValidate = "drpEmployee";
                    rqSource.Enabled = true;
                }
                else if (drpFrom.SelectedValue == "5")
                {
                    drpLoan.DataSource = BalVoucher.GetLoan();
                    drpLoan.DataValueField = "Value";
                    drpLoan.DataTextField = "Text";
                    drpLoan.DataBind();
                    drpLoan.Visible = true;
                    drpLoan.ClearSelection();

                    lblFromLabel.Text = "Loan Name/قرض";
                    lblFromLabel.Visible = true;
                    rqSource.ValidationGroup = "save";
                    rqSource.ControlToValidate = "drpLoan";
                    rqSource.Enabled = true;
                }
                else if (drpFrom.SelectedValue == "4")
                {
                    drpParty.DataSource = BalVoucher.fillParty();
                    drpParty.DataValueField = "Value";
                    drpParty.DataTextField = "Text";
                    drpParty.DataBind();
                    drpParty.Visible = true;
                    drpParty.ClearSelection();

                    RadComboBoxItem CodeItem = new RadComboBoxItem();
                    CodeItem.Text = "New Entry";
                    CodeItem.Value = "0";
                    drpParty.Items.Insert(0, CodeItem);

                    lblFromLabel.Text = "Party Name";
                    lblFromLabel.Visible = true;
                    rqSource.ValidationGroup = "no";
                    rqSource.ControlToValidate = "drpEmployee";
                }
                else if (drpFrom.SelectedValue == "10")
                {
                    drpParty.DataSource = BalVoucher.fillParty();
                    drpParty.DataValueField = "Value";
                    drpParty.DataTextField = "Text";
                    drpParty.DataBind();
                    drpParty.Visible = true;
                    drpParty.ClearSelection();

                    drpAsset.DataSource = BalVoucher.fillasset();
                    drpAsset.DataValueField = "Id";
                    drpAsset.DataTextField = "Name";
                    drpAsset.DataBind();
                    drpAsset.ClearSelection();

                    pnlasset1.Visible = pnlasset2.Visible = drpAssetrqd.Enabled = true;

                    RadComboBoxItem CodeItem = new RadComboBoxItem();
                    CodeItem.Text = "New Entry";
                    CodeItem.Value = "0";
                    drpParty.Items.Insert(0, CodeItem);

                    lblFromLabel.Text = "Party Name";
                    lblFromLabel.Visible = true;
                    rqSource.ValidationGroup = "no";
                    rqSource.ControlToValidate = "drpEmployee";
                }
                else if (drpFrom.SelectedValue == "7")  //Deposit return
                {
                    drpDeposit.DataSource = BalVoucher.GetDepositTypeList();
                    drpDeposit.DataValueField = "Value";
                    drpDeposit.DataTextField = "Text";
                    drpDeposit.DataBind();
                    drpDeposit.Visible = true;

                    lblFromLabel.Text = "Deposit Type";
                    lblFromLabel.Visible = true;
                    rqSource.ValidationGroup = "save";
                    rqSource.ControlToValidate = "drpDeposit";
                    rqSource.Enabled = true;
                }
            }
            UpdTo.Update();
            UpdFrom.Update();
            hdnCustomerPaymentType.Value = "0";
            btnInvDetails.Visible = false;
            updSaving.Update();
            lblReceivable.Text = "";
            lblPayable.Text = "";
            updAccountDetails.Update();
            txtAmountMain.ReadOnly = false;
            updAmountMain.Update();
            updCPaytype.Update();
            updinvoicebtn.Update();
            updasset.Update();
            updasset2.Update();
        }

        protected void RemoveAdvanceOption()
        {
            if (drpToType.Items.Count > 6)
            {
                drpToType.Items.Remove(3);
            }
            if (drpToType.Items.Count > 6)
            {
                RemoveAdvanceOption();
            }
            UpdToType.Update();
        }

        protected void drpToTypeOnSelectedIndexChanged(Object sender, EventArgs e)
        {
            txt_commsn.Text = "";

            txtAmountMain.ReadOnly = false;
            updAmountMain.Update();
            txtCommissionVat.Text = "";
            pnlCommissionVat.Visible = false;
            hdnisCommissionVat.Value = "0";

            lblChargedAmt.Visible = txtChargedAmt.Visible = false;
            txtChargedAmt.Text = "";

            drpPettyCash.Visible = false;
            drpPettyCash.Items.Clear();
            drpPettyCash.Text = "";

            drpBankAccount.Visible = false;
            drpBankAccount.Items.Clear();
            drpBankAccount.Text = "";

            drpLoanAccount.Visible = false;
            drpLoanAccount.Items.Clear();
            drpLoanAccount.Text = "";

            lblChequeDate.Visible = false;
            dtChequeDate.DbSelectedDate = "";
            dtChequeDate.Visible = false;
            rqChequeDate.ValidationGroup = "no";

            lblToLabel.Text = "Bank Name/بنك";
            lblToLabel.Visible = false;
            rqTo.ValidationGroup = "no";
            rqTo.ControlToValidate = "drpBankAccount";

            if (drpToType.SelectedValue != "")
            {
                if (drpToType.SelectedValue == "1")
                {
                    drpPettyCash.DataSource = BalVoucher.GetPettyCashAccountList(Convert.ToInt32(hdn_user_id.Value));
                    drpPettyCash.DataValueField = "Value";
                    drpPettyCash.DataTextField = "Text";
                    drpPettyCash.DataBind();
                    drpPettyCash.Visible = true;

                    lblToLabel.Text = "Cash Name/اسم المصروفات النثرية";
                    lblToLabel.Visible = true;
                    rqTo.ValidationGroup = "save";
                    rqTo.ControlToValidate = "drpPettyCash";
                }
                else if (drpToType.SelectedValue == "2" || drpToType.SelectedValue == "6")
                {
                    drpBankAccount.DataSource = BalVoucher.GetBankAccountList(Convert.ToInt32(hdn_user_id.Value));
                    drpBankAccount.DataValueField = "Value";
                    drpBankAccount.DataTextField = "Text";
                    drpBankAccount.DataBind();
                    drpBankAccount.Visible = true;

                    lblToLabel.Text = "Bank Name/بنك";
                    lblToLabel.Visible = true;
                    rqTo.ValidationGroup = "save";
                    rqTo.ControlToValidate = "drpBankAccount";

                    if (drpToType.SelectedValue == "2")
                        lblChargedAmt.Visible = txtChargedAmt.Visible = true;
                    if (drpToType.SelectedValue == "6")
                        pnlCommissionVat.Visible = true;
                }
                else if (drpToType.SelectedValue == "10"  ) // nomad
                {
                    drpBankAccount.DataSource = BalVoucher.GetNomadBankAccountList(Convert.ToInt32(hdn_user_id.Value));
                    drpBankAccount.DataValueField = "Value";
                    drpBankAccount.DataTextField = "Text";
                    drpBankAccount.DataBind();
                    drpBankAccount.Visible = true;

                    lblToLabel.Text = "Bank Name/بنك";
                    lblToLabel.Visible = true;
                    rqTo.ValidationGroup = "save";
                    rqTo.ControlToValidate = "drpBankAccount";

                }
                else if (drpToType.SelectedValue == "3")
                {
                    lblToLabel.Text = "Bank Name/بنك";
                    lblToLabel.Visible = false;
                    rqTo.ValidationGroup = "no";
                    rqTo.ControlToValidate = "drpBankAccount";

                    lblChequeDate.Visible = true;
                    dtChequeDate.DbSelectedDate = "";
                    dtChequeDate.Visible = true;
                    rqChequeDate.ValidationGroup = "save";
                }
                else if (drpToType.SelectedValue == "4")
                {
                    lblToLabel.Text = "Bank Name/بنك";
                    lblToLabel.Visible = false;
                    rqTo.ValidationGroup = "no";
                    rqTo.ControlToValidate = "drpBankAccount";

                    if (hdnCustomerPaymentType.Value == "1" || hdnCustomerPaymentType.Value == "2")
                    {
                        if ((txtAmountMain.Text.ToString() != "" ? Convert.ToDouble(txtAmountMain.Text) : 0) >
                            (lblPayable.Text.ToString() != "" ? (Convert.ToDouble(lblPayable.Text)) : 0))
                        {
                            hdnCustomerPaymentType.Value = "0";
                            updSaving.Update();
                            txtAmountMain.Text = "";
                            updAmountMain.Update();
                        }
                    }
                    else
                    {
                        txtAmountMain.Text = "";
                        txtAmountMain.ReadOnly = true;
                        updAmountMain.Update();
                    }
                }
                else if (drpToType.SelectedValue == "5")
                {
                    drpLoanAccount.DataSource = BalVoucher.GetLoan();
                    drpLoanAccount.DataValueField = "Value";
                    drpLoanAccount.DataTextField = "Text";
                    drpLoanAccount.DataBind();
                    drpLoanAccount.Visible = true;

                    lblToLabel.Text = "Loan";
                    lblToLabel.Visible = true;
                    rqTo.ValidationGroup = "save";
                    rqTo.ControlToValidate = "drpLoanAccount";
                }
            }

            UpdCheque.Update();
            upd_commsn.Update();
            UpdTo.Update();
            updCommissionVat.Update();
        }

        protected void onchangedrp_bank(object sender, EventArgs e)
        {
            hdn_bankcommsn.Value = hdnisCommissionVat.Value = "0";

            if (drpBankAccount.SelectedValue != "" && drpToType.SelectedValue == "6")// only for card swipe
            {
                DataTable dt = obj_master.Edit_Bank_Account(Convert.ToInt32(drpBankAccount.SelectedValue));
                hdnisCommissionVat.Value = dt.Rows[0]["IsVatApplicable"].ToString();
                if (dt.Rows[0]["IsCommssionApp"].ToString() == "1" & dt.Rows[0]["CommissionPer"].ToString() != "")
                    hdn_bankcommsn.Value = dt.Rows[0]["CommissionPer"].ToString();
            }
            UpdTo.Update();
            CalCommission();
        }
        public void CalCommission()
        {
            txt_commsn.Text = txtCommissionVat.Text = "";
            decimal commsn = 0, vat = 0;
            if (txtAmountMain.Text != "" & hdn_bankcommsn.Value != "0")
            {
                commsn = (Convert.ToDecimal(txtAmountMain.Text) * (Convert.ToDecimal(hdn_bankcommsn.Value) / 100));
                txt_commsn.Text = commsn.ToString("0.00");
            }
            vat = (commsn * Convert.ToDecimal(0.05));
            if (hdnisCommissionVat.Value == "1")
                txtCommissionVat.Text = vat.ToString("0.00");

            upd_commsn.Update();
            updCommissionVat.Update();
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


        public void fillBankAccountEdit(int AccountId)
        {
            drpBankAccount.DataSource = BalVoucher.GetBankAccountListEdit(Convert.ToInt32(hdn_user_id.Value), AccountId);
            drpBankAccount.DataValueField = "Value";
            drpBankAccount.DataTextField = "Text";
            drpBankAccount.DataBind();
            drpBankAccount.Visible = true;
            drpBankAccount.ClearSelection();
            drpBankAccount.Text = "";
        }

        protected void btnInvDetailsOnClick(object sender, EventArgs e)
        {

            if (drpFrom.SelectedValue == "6")
            {
                pnlCustmrInvoice.Visible = true;
                if (hdnCustomerPaymentType.Value == "0")
                {
                    txtCITotal.Text = "";
                    DataSet ds = BalVoucher.GetCustOutStandingInvoiceList_CI(Convert.ToInt32(drpCustomer.SelectedValue));
                    rptCustomerInvoice.DataSource = ds.Tables[0];
                    rptCustomerInvoice.DataBind();
                    lblTReceivableAmount_CI.Text = ds.Tables[1].Rows[0]["TotalReceivable"].ToString();
                    txtOutstandingAmount_CI.Text = ds.Tables[1].Rows[0]["TotalReceivable"].ToString();
                }
                updCustomerInvoiceList.Update();
            }
            else if (drpFrom.SelectedValue == "1")
            {
                pnlInvoice.Visible = true;
                hdnIsinvoice.Value = "0";
                updisinvoice.Update();
                if (hdnCustomerPaymentType.Value == "0")
                {
                    txtTotal.Text = "";
                    DataSet ds = BalVoucher.GetCustOutStandingInvoiceList(Convert.ToInt32(drpCustomer.SelectedValue));
                    rpt_invoiceList.DataSource = ds.Tables[0];
                    rpt_invoiceList.DataBind();
                    lblTReceivableAmount.Text = ds.Tables[1].Rows[0]["TotalReceivable"].ToString();
                    txtOutstandingAmount.Text = ds.Tables[1].Rows[0]["TotalReceivable"].ToString();
                }
                updInvoiceList.Update();
            }
        }

        protected void drpCustomerOnSelectedIndexChanged(object sender, EventArgs e)
        {
            txtAmountMain.ReadOnly = false;
            hdnCustomerPaymentType.Value = "0";
            btnInvDetails.Visible = false;
            RemoveAdvanceOption();

            if (drpCustomer.SelectedValue != "")
            {
                if (drpFrom.SelectedValue == "6") //customerinvocie
                {
                    DataTable dt = BalVoucher.GetCustomerInvoiceById(Convert.ToInt32(drpCustomer.SelectedValue));
                    lblReceivable.Text = dt.Rows[0]["Receivable"].ToString();
                    lblPayable.Text = dt.Rows[0]["Payable"].ToString();

                    btnInvDetails.Visible = true;
                    drpToType.Items.Insert(3, new RadComboBoxItem("Advance", "4"));
                    UpdToType.Update();
                }
                else
                {
                    DataTable dt = BalVoucher.GetCustomerById(Convert.ToInt32(drpCustomer.SelectedValue));
                    lblReceivable.Text = dt.Rows[0]["Receivable"].ToString();
                    lblPayable.Text = dt.Rows[0]["TotalPayable"].ToString();
                }
                if (drpCustomerPaymentType.SelectedValue == "1")
                {
                    txtAmountMain.ReadOnly = true;
                    btnInvDetails.Visible = true;
                    drpToType.Items.Insert(3, new RadComboBoxItem("Advance", "4"));
                    UpdToType.Update();
                }
            }
            else
                lblReceivable.Text = lblPayable.Text = "";

            updAccountDetails.Update();
            updSaving.Update();
            updAmountMain.Update();
            updinvoicebtn.Update();
        }
        protected void drpCompanyGroup_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            txtAmountMain.ReadOnly = false;
            txtAmountMain.Text = "";
            lblReceivable.Text = "";
            lblPayable.Text = "";
            btnCompanyInvDetails.Visible = false;
            hdnCustomerPaymentType.Value = "0";
            RemoveAdvanceOption();

            if (drpCompanyGroup.SelectedValue != "")
            {
                DataSet ds = BalVoucher.GetCompanyGroupInvoiceDetails(Convert.ToInt32(drpCompanyGroup.SelectedValue), Convert.ToInt32(hdn_id.Value));
                lblReceivable.Text = ds.Tables[2].Rows[0]["TotalReceivable"].ToString();
                lblPayable.Text = ds.Tables[2].Rows[0]["CGAdvance"].ToString();

                if (drpCustomerPaymentType.SelectedValue == "1")
                {
                    txtAmountMain.ReadOnly = true;
                    btnCompanyInvDetails.Visible = true;
                    drpToType.Items.Insert(3, new RadComboBoxItem("Advance", "4"));
                }
            }
            UpdToType.Update();
            updinvoicebtn.Update();
            updSaving.Update();
            updAccountDetails.Update();
            updAmountMain.Update();
        }
        protected void drpCustomerPaymentTypeOnSelectedIndexChanged(object sender, EventArgs e)
        {
            txtAmountMain.ReadOnly = false;
            hdnCustomerPaymentType.Value = "0";
            btnInvDetails.Visible = false;
            RemoveAdvanceOption();

            if (drpCustomer.SelectedValue != "")
            {
                if (drpFrom.SelectedValue == "6") //customerinvocie
                {
                    DataTable dt = BalVoucher.GetCustomerInvoiceById(Convert.ToInt32(drpCustomer.SelectedValue));
                    lblReceivable.Text = dt.Rows[0]["Receivable"].ToString();
                    lblPayable.Text = dt.Rows[0]["Payable"].ToString();

                    btnInvDetails.Visible = true;
                    drpToType.Items.Insert(3, new RadComboBoxItem("Advance", "4"));
                    UpdToType.Update();
                }
                else
                {
                    DataTable dt = BalVoucher.GetCustomerById(Convert.ToInt32(drpCustomer.SelectedValue));
                    lblReceivable.Text = dt.Rows[0]["Receivable"].ToString();
                    lblPayable.Text = dt.Rows[0]["TotalPayable"].ToString();
                }
                if (drpCustomerPaymentType.SelectedValue == "1")
                {
                    txtAmountMain.ReadOnly = true;
                    btnInvDetails.Visible = true;
                    drpToType.Items.Insert(3, new RadComboBoxItem("Advance", "4"));
                    UpdToType.Update();
                }
            }
            else if (drpCompanyGroup.SelectedValue != "")
            {
                DataSet ds = BalVoucher.GetCompanyGroupInvoiceDetails(Convert.ToInt32(drpCompanyGroup.SelectedValue), Convert.ToInt32(hdn_id.Value));
                lblReceivable.Text = ds.Tables[2].Rows[0]["TotalReceivable"].ToString();
                lblPayable.Text = ds.Tables[2].Rows[0]["CGAdvance"].ToString();

                if (drpCustomerPaymentType.SelectedValue == "1")
                {
                    txtAmountMain.ReadOnly = true;
                    btnCompanyInvDetails.Visible = true;
                    drpToType.Items.Insert(3, new RadComboBoxItem("Advance", "4"));
                }
            }
            else
                lblReceivable.Text = lblPayable.Text = "";

            updAccountDetails.Update();
            updSaving.Update();
            updAmountMain.Update();
            updinvoicebtn.Update();
        }

        protected void drpVendorOnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblReceivable.Text = "";
            lblPayable.Text = "";
            btnInvDetails.Visible = false;
            divVdeposit.Visible = false;
            updVdeposit.Update();

            if (drpVendor.SelectedValue != "" && drpFrom.SelectedValue == "2")
            {
                DataTable dt = BalVoucher.GetVendorById(Convert.ToInt32(drpVendor.SelectedValue));
                lblReceivable.Text = dt.Rows[0]["Receivable"].ToString();
                lblPayable.Text = dt.Rows[0]["Payable"].ToString();
                btnInvDetails.Visible = false;
            }
            else if (drpVendor.SelectedValue != "" && drpFrom.SelectedValue == "9")  // vendor commission
            {
                DataTable dt = BalVoucher.GetVendorCommissionbal(Convert.ToInt32(drpVendor.SelectedValue));
                lblReceivable.Text = dt.Rows[0]["Receivable"].ToString();
                lblPayable.Text = "0.00";
            }
            else if (drpVendor.SelectedValue != "" && drpFrom.SelectedValue == "8")
            {
                if (hdn_id.Value == "0")
                {
                    divVdeposit.Visible = true;
                    DataTable dt = BalVoucher.VdepositList(Convert.ToInt32(drpVendor.SelectedValue));
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
                    DataTable dt = BalVoucher.EditVdepositList(Convert.ToInt32(drpVendor.SelectedValue), Convert.ToInt32(hdn_id.Value));
                    rptVdeposit.DataSource = dt;
                    rptVdeposit.DataBind();

                    decimal outsum = 0;
                    foreach (DataRow r in dt.Rows)
                        outsum += Convert.ToDecimal(r["Balance"].ToString());
                    txtVdepositTotAmt.Text = outsum.ToString();
                }
                updVdeposit.Update();
            }

            updAccountDetails.Update();
            updSaving.Update();
            updinvoicebtn.Update();
        }
        protected void drpEmployeeOnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpEmployee.SelectedValue != "")
            {
                DataTable dt = BalVoucher.GetEmployeeById(Convert.ToInt32(drpEmployee.SelectedValue));
                lblReceivable.Text = dt.Rows[0]["Receivable"].ToString();
                lblPayable.Text = dt.Rows[0]["Payable"].ToString();
                btnInvDetails.Visible = false;
                updSaving.Update();
                updAccountDetails.Update();
            }
            else
            {
                lblReceivable.Text = "";
                lblPayable.Text = "";
                btnInvDetails.Visible = false;
                updSaving.Update();
                updAccountDetails.Update();
            }
            updinvoicebtn.Update();
        }
        protected void drpLoanOnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpLoan.SelectedValue != "")
            {
                DataTable dt = BalVoucher.GetLoanById(Convert.ToInt32(drpLoan.SelectedValue));
                lblReceivable.Text = dt.Rows[0]["Receivable"].ToString();
                lblPayable.Text = dt.Rows[0]["Payable"].ToString();
                btnInvDetails.Visible = false;
                updSaving.Update();
                updAccountDetails.Update();
            }
            else
            {
                lblReceivable.Text = "";
                lblPayable.Text = "";
                btnInvDetails.Visible = false;
                updSaving.Update();
                updAccountDetails.Update();
            }
            updinvoicebtn.Update();
        }

        #region CompanyGroupInvoice

        protected void btnInvDetailsOnClickCG(object sender, EventArgs e)
        {
            pnlInvoiceCG.Visible = true;
            txtAmtAutoCG.Text = txtTotalCG.Text = "";

            DataSet ds = BalVoucher.GetCompanyGroupInvoiceDetails(Convert.ToInt32(drpCompanyGroup.SelectedValue), Convert.ToInt32(hdn_id.Value));

            if (hdn_id.Value == "0")
            {
                rpt_invoiceListCG.DataSource = ds.Tables[0];
                rpt_invoiceListCG.DataBind();

                lblTReceivableAmountCG.Text = ds.Tables[2].Rows[0]["TotalReceivable"].ToString();
            }
            else
            {
                rpt_invoiceListCG.DataSource = ds.Tables[1];
                rpt_invoiceListCG.DataBind();

                lblTReceivableAmountCG.Text = txtTotalCG.Text = ds.Tables[3].Rows[0]["TotalReceivable"].ToString();
            }

            updInvoiceListCG.Update();
        }

        protected void btnProceedOnClickCG(object sender, EventArgs e)
        {
            int selected = 0;
            foreach (RepeaterItem item in rpt_invoiceListCG.Items)
            {
                CheckBox chkSelectCG = (CheckBox)item.FindControl("chkSelectCG");
                if (chkSelectCG.Checked == true)
                    selected += 1;
            }
            if (selected == 0 || txtTotalCG.Text == "")
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Add Invoice details to Continue.!');", true);

            else
            {
                hdnCustomerPaymentType.Value = "1";
                txtAmountMain.Text = txtTotalCG.Text;
                txtAmountMain.ReadOnly = true;
                updAmountMain.Update();

                pnlInvoiceCG.Visible = false;
                updInvoiceListCG.Update();
                CalCommission();
            }
        }

        protected void btnCloseClickCG(object sender, EventArgs e)
        {
            txtAmountMain.Text = "";
            txtAmountMain.ReadOnly = false;
            updAmountMain.Update();

            pnlInvoiceCG.Visible = false;
            updInvoiceListCG.Update();
            CalCommission();
        }

        protected void chkSelectOnCheckedChangedCG(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;
            UpdatePanel updAmountCG = (UpdatePanel)itemrp.FindControl("updAmountCG");
            TextBox txtAmountCG = (TextBox)itemrp.FindControl("txtAmountCG");

            TextBox txtReceivableamtCG = (TextBox)itemrp.FindControl("txtReceivableamtCG");
            CheckBox chkSelectCG = (CheckBox)itemrp.FindControl("chkSelectCG");
            if (chkSelectCG.Checked)
                txtAmountCG.Text = txtReceivableamtCG.Text;
            else
                txtAmountCG.Text = "";

            txtAmountCG.Enabled = chkSelectCG.Checked;
            updAmountCG.Update();
            AmountCalCulationCG();
        }

        public void AmountCalCulationCG()
        {
            decimal TotalAmount = 0;
            decimal OTAmount = 0;
            foreach (RepeaterItem item in rpt_invoiceListCG.Items)
            {
                CheckBox chkSelectCG = (CheckBox)item.FindControl("chkSelectCG");
                TextBox txtAmountCG = (TextBox)item.FindControl("txtAmountCG");
                TextBox txtReceivableamtCG = (TextBox)item.FindControl("txtReceivableamtCG");
                if (chkSelectCG.Checked == true)
                    TotalAmount = TotalAmount + (txtAmountCG.Text == "" ? 0 : Convert.ToDecimal(txtAmountCG.Text));
                OTAmount = OTAmount + ((txtReceivableamtCG.Text == "" ? 0 : Convert.ToDecimal(txtReceivableamtCG.Text)) - (txtAmountCG.Text == "" ? 0 : Convert.ToDecimal(txtAmountCG.Text)));
            }
            txtTotalCG.Text = TotalAmount.ToString();
            updTotalInvoiceAmountCG.Update();
        }

        protected void btnAllocOnClickCG(object sender, EventArgs e)
        {
            if (txtAmtAutoCG.Text != "")
            {
                decimal balamt = 0;
                foreach (RepeaterItem item in rpt_invoiceListCG.Items)
                {
                    CheckBox chkSelectCG = (CheckBox)item.FindControl("chkSelectCG");
                    TextBox txtReceivableamtCG = (TextBox)item.FindControl("txtReceivableamtCG");
                    balamt = balamt + Convert.ToDecimal(txtReceivableamtCG.Text);
                    chkSelectCG.Checked = false;
                    txtTotalCG.Text = "";
                }
                if (balamt < Convert.ToDecimal(txtAmtAutoCG.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Amount cannot be greater than Outstanding amount!');", true);
                    txtAmtAutoCG.Text = "";
                }
                else
                {
                    decimal Amount = Convert.ToDecimal(txtAmtAutoCG.Text);
                    txtTotalCG.Text = Amount.ToString();

                    foreach (RepeaterItem item in rpt_invoiceListCG.Items)
                    {
                        CheckBox chkSelectCG = (CheckBox)item.FindControl("chkSelectCG");
                        TextBox txtReceivableamtCG = (TextBox)item.FindControl("txtReceivableamtCG");
                        TextBox txtAmountCG = (TextBox)item.FindControl("txtAmountCG");
                        if (Amount > 0)
                        {
                            chkSelectCG.Checked = true;
                            if (Amount > Convert.ToDecimal(txtReceivableamtCG.Text))
                            {
                                txtAmountCG.Text = txtReceivableamtCG.Text;
                                Amount = Amount - Convert.ToDecimal(txtReceivableamtCG.Text);
                            }
                            else
                            {
                                txtAmountCG.Text = Amount.ToString();
                                Amount = 0;
                            }
                        }
                    }
                }
            }
            UpdatePanel2CG.Update();
        }

        #endregion

        protected void btnProceedOnClick(object sender, EventArgs e)
        {
            DataTable dt = obj_master.Edit_GeneralSettings();
            int isinvoice = 0;
            foreach (RepeaterItem item in rpt_invoiceList.Items)
            {
                CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                HiddenField hdnInvId = (HiddenField)item.FindControl("hdnInvId");

                if (chkSelect.Checked == true && hdnInvId.Value != "")
                {
                    isinvoice = 1;
                }
            }

            if (dt.Rows[0]["EnableCustomerInvoice"].ToString() == "1" && isinvoice == 1)
            {
                hdnCustomerPaymentType.Value = "0";
                txtAmountMain.ReadOnly = false;
                updSaving.Update();
                updAmountMain.Update();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Invoice Payment can done only through customer invoice type.!');", true);

            }
            else if (txtTotal.Text != "")
            {
                hdnCustomerPaymentType.Value = "1";
                txtAmountMain.Text = txtTotal.Text;
                txtAmountMain.ReadOnly = true;
                updSaving.Update();
                updAmountMain.Update();
                pnlInvoice.Visible = false;
                updInvoiceList.Update();
                CalCommission();
            }
            else
            {
                txtAmountMain.ReadOnly = false;
                updSaving.Update();
                updAmountMain.Update();
                pnlInvoice.Visible = false;
                updInvoiceList.Update();
            }
        }

        protected void btnCIProceedOnClick(object sender, EventArgs e)
        {
            hdnCustomerPaymentType.Value = "2";
            txtAmountMain.Text = txtCITotal.Text;
            txtAmountMain.ReadOnly = true;
            updSaving.Update();
            updAmountMain.Update();
            pnlCustmrInvoice.Visible = false;
            updCustomerInvoiceList.Update();

            CalCommission();
        }

        protected void btnAdvanceProceedOnClick(object sender, EventArgs e)
        {
            hdnCustomerPaymentType.Value = "0";
            txtAmountMain.Text = "";
            txtAmountMain.ReadOnly = false;
            updSaving.Update();
            updAmountMain.Update();
            pnlInvoice.Visible = false;
            updInvoiceList.Update();

            pnlCustmrInvoice.Visible = false;
            updCustomerInvoiceList.Update();
            CalCommission();
        }
        protected void chkSelectOnCheckedChanged(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;
            UpdatePanel updAmount = (UpdatePanel)itemrp.FindControl("updAmount");
            TextBox txtAmount = (TextBox)itemrp.FindControl("txtAmount");
            HiddenField hdnInvId = (HiddenField)itemrp.FindControl("hdnInvId");

            TextBox txtReceivableamt = (TextBox)itemrp.FindControl("txtReceivableamt");
            CheckBox chkSelect = (CheckBox)itemrp.FindControl("chkSelect");
            if (chkSelect.Checked)
            {
                hdnIsinvoice.Value = hdnInvId.Value != "" ? "1" : "0";
                txtAmount.Text = txtReceivableamt.Text;
                updisinvoice.Update();
            }
            else
                txtAmount.Text = "";
            txtAmount.Enabled = chkSelect.Checked;
            updAmount.Update();
            AmountCalCulation();
        }

        protected void CIchkSelectOnCheckedChanged(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;
            UpdatePanel updAmount = (UpdatePanel)itemrp.FindControl("updAmount");
            TextBox txtAmount = (TextBox)itemrp.FindControl("txtAmountCI");
            TextBox txtReceivableamt = (TextBox)itemrp.FindControl("txtReceivableamt");
            CheckBox chkSelect = (CheckBox)itemrp.FindControl("chkSelect");
            if (chkSelect.Checked)
                txtAmount.Text = txtReceivableamt.Text;
            else
                txtAmount.Text = "";
            txtAmount.Enabled = chkSelect.Checked;
            updAmount.Update();
            CIAmountCalCulation();
        }


        protected void rpt_invoiceList_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox chkSelect = (CheckBox)e.Item.FindControl("chkSelect");
                TextBox txtAmount = (TextBox)e.Item.FindControl("txtAmount");
                HiddenField hdnInvStatus = (HiddenField)e.Item.FindControl("hdnInvStatus");
                txtAmount.Enabled = chkSelect.Checked;
                //if (hdnInvStatus.Value == "0")
                //{
                //    chkSelect.Enabled = true;
                //    txtAmount.Enabled = chkSelect.Checked;
                //}
                //else
                //{
                //    chkSelect.Enabled = false;
                //    txtAmount.Attributes.Add("class", "read_Only");
                //}
            }

        }

        protected void rpt_CIinvoiceList_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox chkSelect = (CheckBox)e.Item.FindControl("chkSelect");
                TextBox txtAmount = (TextBox)e.Item.FindControl("txtAmountCI");
                HiddenField hdnInvStatus = (HiddenField)e.Item.FindControl("hdnInvStatus");
                txtAmount.Enabled = chkSelect.Checked;
            }

        }

        public void AmountCalCulation()
        {
            decimal TotalAmount = 0;
            decimal OTAmount = 0;
            foreach (RepeaterItem item in rpt_invoiceList.Items)
            {
                CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                TextBox txtAmount = (TextBox)item.FindControl("txtAmount");
                TextBox txtReceivableamt = (TextBox)item.FindControl("txtReceivableamt");
                if (chkSelect.Checked == true)
                    TotalAmount = TotalAmount + (txtAmount.Text == "" ? 0 : Convert.ToDecimal(txtAmount.Text));
                OTAmount = OTAmount + ((txtReceivableamt.Text == "" ? 0 : Convert.ToDecimal(txtReceivableamt.Text)) - (txtAmount.Text == "" ? 0 : Convert.ToDecimal(txtAmount.Text)));
            }
            txtTotal.Text = TotalAmount.ToString();
            updTotalInvoiceAmount.Update();
            txtOutstandingAmount.Text = OTAmount.ToString();
            updOutstandingAmount.Update();
        }

        public void CIAmountCalCulation()
        {
            decimal TotalAmount = 0;
            decimal OTAmount = 0;
            foreach (RepeaterItem item in rptCustomerInvoice.Items)
            {
                CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                TextBox txtAmount = (TextBox)item.FindControl("txtAmountCI");
                TextBox txtReceivableamt = (TextBox)item.FindControl("txtReceivableamt");
                if (chkSelect.Checked == true)
                    TotalAmount = TotalAmount + (txtAmount.Text == "" ? 0 : Convert.ToDecimal(txtAmount.Text));
                OTAmount = OTAmount + ((txtReceivableamt.Text == "" ? 0 : Convert.ToDecimal(txtReceivableamt.Text)) - (txtAmount.Text == "" ? 0 : Convert.ToDecimal(txtAmount.Text)));
            }
            txtCITotal.Text = TotalAmount.ToString();
            updCITotal.Update();
            txtOutstandingAmount_CI.Text = OTAmount.ToString();
            updCIOutStndng.Update();
        }

        protected void btnAllocOnClick(object sender, EventArgs e)
        {
            if (txtAmtAuto.Text != "")
            {
                decimal balamt = 0;
                foreach (RepeaterItem item in rpt_invoiceList.Items)
                {
                    CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                    TextBox txt_balance_amount = (TextBox)item.FindControl("txtReceivableamt");
                    TextBox txt_pay_amount = (TextBox)item.FindControl("txtAmount");
                    balamt = balamt + Convert.ToDecimal(txt_balance_amount.Text);
                    chkSelect.Checked = false;
                    txtTotal.Text = "";
                }
                if (balamt < Convert.ToDecimal(txtAmtAuto.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Amount cannot be greater than Outstanding amount!');", true);
                    txtAmtAuto.Text = "";
                }
                else
                {
                    decimal Amount = Convert.ToDecimal(txtAmtAuto.Text);
                    txtTotal.Text = Amount.ToString();
                    txtAmountMain.ReadOnly = true;

                    foreach (RepeaterItem item in rpt_invoiceList.Items)
                    {
                        CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                        TextBox txt_balance_amount = (TextBox)item.FindControl("txtReceivableamt");
                        TextBox txt_pay_amount = (TextBox)item.FindControl("txtAmount");
                        if (Amount > 0)
                        {
                            chkSelect.Checked = true;
                            if (Amount > Convert.ToDecimal(txt_balance_amount.Text))
                            {
                                txt_pay_amount.Text = txt_balance_amount.Text;
                                Amount = Amount - Convert.ToDecimal(txt_balance_amount.Text);
                            }
                            else
                            {
                                txt_pay_amount.Text = Amount.ToString();
                                Amount = 0;
                            }
                        }
                    }
                }
            }
            UpdatePanel2.Update();
        }

        protected void btnAllocCIOnClick(object sender, EventArgs e)
        {
            if (txtAmtAutoCI.Text != "")
            {
                decimal balamt = 0;
                foreach (RepeaterItem item in rptCustomerInvoice.Items)
                {
                    CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                    TextBox txt_balance_amount = (TextBox)item.FindControl("txtReceivableamt");
                    TextBox txt_pay_amount = (TextBox)item.FindControl("txtAmountCI");
                    balamt = balamt + Convert.ToDecimal(txt_balance_amount.Text);
                    chkSelect.Checked = false;
                    txtCITotal.Text = "";
                }
                if (balamt < Convert.ToDecimal(txtAmtAutoCI.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Amount cannot be greater than Outstanding amount!');", true);
                    txtAmtAutoCI.Text = "";
                }
                else
                {
                    decimal Amount = Convert.ToDecimal(txtAmtAutoCI.Text);
                    txtCITotal.Text = Amount.ToString();
                    txtAmountMain.ReadOnly = true;

                    foreach (RepeaterItem item in rptCustomerInvoice.Items)
                    {
                        CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                        TextBox txt_balance_amount = (TextBox)item.FindControl("txtReceivableamt");
                        TextBox txt_pay_amount = (TextBox)item.FindControl("txtAmountCI");
                        if (Amount > 0)
                        {
                            chkSelect.Checked = true;
                            if (Amount > Convert.ToDecimal(txt_balance_amount.Text))
                            {
                                txt_pay_amount.Text = txt_balance_amount.Text;
                                Amount = Amount - Convert.ToDecimal(txt_balance_amount.Text);
                            }
                            else
                            {
                                txt_pay_amount.Text = Amount.ToString();
                                Amount = 0;
                            }
                        }
                    }
                }
            }
            UpdatePanel3.Update();
        }

        public int SaveRV()
        {
            int res = 0;
            int? intnull = null;
            DateTime? datenull = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("InvoiceId", typeof(int));
            dt.Columns.Add("InvoiceStatus", typeof(int));
            dt.Columns.Add("ReceivableAmount", typeof(double));
            dt.Columns.Add("PaidAmount", typeof(double));

            DataTable dtCI = new DataTable();
            dtCI.Columns.Add("InvoiceId", typeof(int));
            dtCI.Columns.Add("InvoiceStatus", typeof(int));
            dtCI.Columns.Add("ReceivableAmount", typeof(double));
            dtCI.Columns.Add("PaidAmount", typeof(double));

            DataTable dtCG = new DataTable();
            dtCG.Columns.Add("InvoiceId", typeof(int));
            dtCG.Columns.Add("InvoiceStatus", typeof(int));
            dtCG.Columns.Add("CustomerId", typeof(int));
            dtCG.Columns.Add("ReceivableAmount", typeof(double));
            dtCG.Columns.Add("PaidAmount", typeof(double));

            if (hdnCustomerPaymentType.Value == "1")
            {
                foreach (RepeaterItem item in rpt_invoiceList.Items)
                {
                    CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                    if (chkSelect.Checked == true)
                    {
                        HiddenField hdnInvId = (HiddenField)item.FindControl("hdnInvId");
                        HiddenField hdnInvStatus = (HiddenField)item.FindControl("hdnInvStatus");
                        TextBox txtReceivableamt = (TextBox)item.FindControl("txtReceivableamt");
                        TextBox txtAmount = (TextBox)item.FindControl("txtAmount");
                        dt.Rows.Add(hdnInvId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvId.Value), Convert.ToInt32(hdnInvStatus.Value), Convert.ToDouble(txtReceivableamt.Text), Convert.ToDouble(txtAmount.Text));
                    }
                }
            }

            foreach (RepeaterItem item in rpt_invoiceListCG.Items)
            {
                CheckBox chkSelectCG = (CheckBox)item.FindControl("chkSelectCG");
                if (chkSelectCG.Checked == true)
                {
                    HiddenField hdnInvIdCG = (HiddenField)item.FindControl("hdnInvIdCG");
                    HiddenField hdnInvStatusCG = (HiddenField)item.FindControl("hdnInvStatusCG");
                    HiddenField hdnCustomerIdCG = (HiddenField)item.FindControl("hdnCustomerIdCG");
                    TextBox txtReceivableamtCG = (TextBox)item.FindControl("txtReceivableamtCG");
                    TextBox txtAmountCG = (TextBox)item.FindControl("txtAmountCG");
                    dtCG.Rows.Add(Convert.ToInt32(hdnInvIdCG.Value), Convert.ToInt32(hdnInvStatusCG.Value), Convert.ToInt32(hdnCustomerIdCG.Value),
                        Convert.ToDouble(txtReceivableamtCG.Text), Convert.ToDouble(txtAmountCG.Text));
                }
            }

            if (hdnCustomerPaymentType.Value == "2")
            {
                foreach (RepeaterItem item in rptCustomerInvoice.Items)
                {
                    CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                    if (chkSelect.Checked == true)
                    {
                        HiddenField hdnInvId = (HiddenField)item.FindControl("hdnInvId");
                        HiddenField hdnInvStatus = (HiddenField)item.FindControl("hdnInvStatus");
                        TextBox txtReceivableamt = (TextBox)item.FindControl("txtReceivableamt");
                        TextBox txtAmount = (TextBox)item.FindControl("txtAmountCI");
                        dtCI.Rows.Add(hdnInvId.Value == "" ? (int?)null : Convert.ToInt32(hdnInvId.Value), Convert.ToInt32(hdnInvStatus.Value), Convert.ToDouble(txtReceivableamt.Text),
                            Convert.ToDouble(txtAmount.Text));
                    }
                }
            }

            DataTable dtvdeposit = new DataTable();
            dtvdeposit.Columns.Add("InvoiceId", typeof(int));
            dtvdeposit.Columns.Add("InvoiceDetId", typeof(int));
            dtvdeposit.Columns.Add("Pay", typeof(decimal));

            foreach (RepeaterItem itm in rptVdeposit.Items)
            {
                HiddenField hdninvoiceId = (HiddenField)itm.FindControl("hdninvoiceId");
                HiddenField hdninvdetId = (HiddenField)itm.FindControl("hdninvdetId");
                HiddenField hdn_D_id = (HiddenField)itm.FindControl("hdn_D_id");
                TextBox txtVdepositPayAmt = (TextBox)itm.FindControl("txtVdepositPayAmt");
                CheckBox chk_select = (CheckBox)itm.FindControl("chk_select");

                if (chk_select.Checked)
                {
                    dtvdeposit.Rows.Add(Convert.ToInt32(hdninvoiceId.Value), hdninvdetId.Value == "" ? (int?)null : Convert.ToInt32(hdninvdetId.Value),
                        Convert.ToDecimal(txtVdepositPayAmt.Text));
                }
            }


            if (hdn_id.Value == "0" && drpToType.SelectedValue == "4" &&
                drpCustomer.SelectedValue != "" && (Convert.ToDecimal(txtAmountMain.Text) > (lblPayable.Text == "" ? 0 : Convert.ToDecimal(lblPayable.Text))))
            {
                res = -1;
            }
            else if (hdn_id.Value == "0" && drpToType.SelectedValue == "4" &&
                drpCompanyGroup.SelectedValue != "" && (Convert.ToDecimal(txtAmountMain.Text) > (lblPayable.Text == "" ? 0 : Convert.ToDecimal(lblPayable.Text))))
            {
                res = -2;
            }
            else
            {
                if (drpToType.SelectedValue == "1" && drpPettyCash.SelectedValue == "")
                {
                    res = -3;
                }
                else if ((drpToType.SelectedValue == "2" || drpToType.SelectedValue == "6" || drpToType.SelectedValue == "10") && drpBankAccount.SelectedValue == "")
                {
                    res = -4;
                }
                else if (drpFrom.SelectedValue == "8" && dtvdeposit.Rows.Count == 0)
                {
                    res = -5;
                }
                else if (drpFrom.SelectedValue == "1" && drpCustomerPaymentType.SelectedValue == "1" && dt.Rows.Count == 0)
                {
                    res = -6;
                }
                else if (drpFrom.SelectedValue == "11" && drpCustomerPaymentType.SelectedValue == "1" && dtCG.Rows.Count == 0)
                {
                    res = -7;
                }

                else
                {
                    res = BalVoucher.InsertUpdateReceiptVoucher(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(drpFrom.SelectedValue), DateTime.ParseExact(CalDate(dtdated), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                       drpCustomer.SelectedValue != "" ? Convert.ToInt32(drpCustomer.SelectedValue) : intnull,
                       drpVendor.SelectedValue != "" ? Convert.ToInt32(drpVendor.SelectedValue) : intnull,
                       drpEmployee.SelectedValue != "" ? Convert.ToInt32(drpEmployee.SelectedValue) : intnull,
                       drpLoan.SelectedValue != "" ? Convert.ToInt32(drpLoan.SelectedValue) : intnull,
                       Convert.ToInt32(drpIncomeType.SelectedValue), Convert.ToDouble(txtAmountMain.Text),
                       hdnCustomerPaymentType.Value == "1" ? (txtOutstandingAmount.Text == "" ? 0 : Convert.ToDecimal(txtOutstandingAmount.Text)) :
                       (hdnCustomerPaymentType.Value == "2" ? (txtOutstandingAmount_CI.Text == "" ? 0 : Convert.ToDecimal(txtOutstandingAmount_CI.Text)) : (decimal?)null),
                       Convert.ToInt32(drpToType.SelectedValue),
                       drpPettyCash.SelectedValue != "" ? Convert.ToInt32(drpPettyCash.SelectedValue) : intnull,
                       drpBankAccount.SelectedValue != "" ? Convert.ToInt32(drpBankAccount.SelectedValue) : intnull,
                       dtChequeDate.DbSelectedDate != null ? DateTime.ParseExact(CalDate(dtChequeDate), "dd/MM/yyyy", CultureInfo.InvariantCulture) : datenull,
                       drpToType.SelectedValue == "3" ? 1 : 0, datenull, txtTransaction.Text, txtRemarks.Text, Convert.ToInt32(hdn_user_id.Value), dt,
                       txt_commsn.Text == "" ? (decimal?)null : Convert.ToDecimal(txt_commsn.Text), dtCI,
                       drpDeposit.SelectedValue != "" ? Convert.ToInt32(drpDeposit.SelectedValue) : intnull,
                       txttax.Text == "" ? 0 : Convert.ToDecimal(txttax.Text), txtChargedAmt.Text == "" ? (decimal?)null : Convert.ToDecimal(txtChargedAmt.Text),
                       drpLoanAccount.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpLoanAccount.SelectedValue),
                       dtvdeposit, drpParty.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpParty.SelectedValue),
                       (drpFrom.SelectedValue == "1" || drpFrom.SelectedValue == "11") ? Convert.ToInt32(drpCustomerPaymentType.SelectedValue) : (int?)null,
                        hdnfilenameup.Value, hdnfilenamesaveup.Value, drpAsset.SelectedValue == "" ? (int?)null :
                        Convert.ToInt32(drpAsset.SelectedValue), txtCommissionVat.Text == "" ? 0 : Convert.ToDecimal(txtCommissionVat.Text)
                      , drpFrom.SelectedValue == "11" ? Convert.ToInt32(drpCompanyGroup.SelectedValue) : (int?)null, dtCG);
                }
            }
            return res;
        }

        protected void saveReceiptVoucher(object sender, EventArgs e)
        {
            int res = SaveRV();
            if (res > 0)
            {
                Clear();
                lblAlertCommn.Text = "Saved Successfully !..";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
                //lbl_msgin.Text = "Saved Successfully !..";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else if (res ==-1)
            {
                lblAlertCommn.Text = "Payment Amount cannot be greater than advance amount.!";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
            }
            else if (res == -2)
            {
                lblAlertCommn.Text = "Payment Amount cannot be greater than advance amount.!";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
            }
            else if (res == -3)
            {
                lblAlertCommn.Text = "Select cash account.!";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
            }
            else if (res == -4)
            {
                lblAlertCommn.Text = "Select bank account.!";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
            }
            else if (res == -5)
            {
                lblAlertCommn.Text = "Select Invoice details.!";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
            }
            else if (res == -6)
            {
                lblAlertCommn.Text = "Select Invoice details.!";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
            }
            else if (res == -7)
            {
                lblAlertCommn.Text = "Select Invoice details.!";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
            }
            else if (res == 0)
            {
                lblAlertCommn.Text = "Sorry Failed to Process Your Request";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
                //lbl_msgin.Text = "Sorry Failed to Process Your Request !..";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            Upd_Add_PanelInner.Update();
        }

        protected void saveprintReceiptVoucher(object sender, EventArgs e)
        {
            int res = SaveRV();
            if (res > 0)
            {
                Clear();
                lblAlertCommn.Text = "Saved Successfully !..";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
                //lbl_msgin.Text = "Saved Successfully !..";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);

                DataTable dt = obj_master.Edit_GeneralSettings();
                int Format = Convert.ToInt32(dt.Rows[0]["ReceiptVoucherFormat"].ToString());

                string url = "";
                if (Format == 3)
                    url = "../Reports/RVPrintFormat3.aspx?id=" + res;
                else
                    url = "../Reports/ReceiptVoucher.aspx?id=" + res;
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
            }
            else if (res == -1)
            {
                lblAlertCommn.Text = "Payment Amount cannot be greater than advance amount.!";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
            }
            else if (res == -2)
            {
                lblAlertCommn.Text = "Payment Amount cannot be greater than advance amount.!";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
            }
            else if (res == -3)
            {
                lblAlertCommn.Text = "Select cash account.!";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
            }
            else if (res == -4)
            {
                lblAlertCommn.Text = "Select bank account.!";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
            }
            else if (res == -5)
            {
                lblAlertCommn.Text = "Select Invoice details.!";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
            }
            else if (res == -6)
            {
                lblAlertCommn.Text = "Select Invoice details.!";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
            }
            else if (res == -7)
            {
                lblAlertCommn.Text = "Select Invoice details.!";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
            }
            else if (res == 0)
            {
                lblAlertCommn.Text = "Sorry Failed to Process Your Request";
                pnlAlertCommn.Visible = true;
                updAlertCommn.Update();
                //lbl_msgin.Text = "Sorry Failed to Process Your Request !..";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            Upd_Add_PanelInner.Update();
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            CancelDeleteReceiptVoucher(2);
        }

        protected void btnDelete_OnClick(object sender, EventArgs e)
        {
            CancelDeleteReceiptVoucher(3);
        }

        public void CancelDeleteReceiptVoucher(int Status)
        {
            int res = BalVoucher.CancelDeleteReceiptVoucher(Convert.ToInt32(hdn_id.Value), Status, txtCancelRemark.Text, Convert.ToInt32(hdn_user_id.Value));
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
                lblAlertCommn.Text = "Sorry Failed to Process Your Request";
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
        protected void btnAlertCloseOnClick(object sender, EventArgs e)
        {
            lblAlertCommn.Text = "";
            pnlAlertCommn.Visible = false;
            updAlertCommn.Update();
        }

        protected void btnOpenDelete_OnClick(object sender, EventArgs e)
        {
            lblCancel.Text = "Delete Receipt Voucher";
            txtCancelRemark.Text = "";
            btnCancel.Visible = false;
            btnDelete.Visible = true;
            pnlCancel.Visible = true;
            updCancel.Update();
        }

        protected void btnOpenCancel_OnClick(object sender, EventArgs e)
        {
            lblCancel.Text = "Cancel Receipt Voucher";
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

        protected void btnPrint_OnClick(object sender, EventArgs e)
        {
            DataTable dt = obj_master.Edit_GeneralSettings();
            int Format = Convert.ToInt32(dt.Rows[0]["ReceiptVoucherFormat"].ToString());

            string url = "";
            if (Format == 3)
                url = "../Reports/RVPrintFormat3.aspx?id=" + Convert.ToInt32(hdn_id.Value);
            else
                url = "../Reports/ReceiptVoucher.aspx?id=" + Convert.ToInt32(hdn_id.Value);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }
        //Calculate Date
        protected void btnReset_OnClick(object sender, EventArgs e)
        {
            Clear();
        }

        protected void btnClose_OnClick(object sender, EventArgs e)
        {
            if (hdnPageId.Value == "1")  //RV
            {
                Panel pnl_add = (Panel)this.Parent.FindControl("pnl_add");
                UpdatePanel Upd_Add_Panel = (UpdatePanel)this.Parent.FindControl("Upd_Add_Panel");
                ((ReceiptVoucher)this.Page).grid_fill(1, Convert.ToInt32(hdnCount.Value), hdnfilter.Value, "", "");

                pnl_add.Visible = false;
                Upd_Add_Panel.Update();
            }
            else if (hdnPageId.Value == "2")  //home
            {
                Panel pnlRVadd = (Panel)this.Parent.FindControl("pnlRVadd");
                UpdatePanel UpdRVadd = (UpdatePanel)this.Parent.FindControl("UpdRVadd");

                pnlRVadd.Visible = false;
                UpdRVadd.Update();
            }
        }


        public void Clear()
        {
            RemoveAdvanceOption();
            hdn_id.Value = "0";
            dtdated.DbSelectedDate = DateTime.Now;
            hdnfilenamesaveup.Value = hdnfilenameup.Value = lblfileupl.Text = "";
            drpFrom.ClearSelection();
            drpFrom.Text = "";
            drpFromOnSelectedIndexChanged(null, null);
            drpCustomer.ClearSelection();
            drpCustomer.Text = lblenablemsg.Text = "";
            drpVendor.ClearSelection();
            drpVendor.Text = "";
            drpEmployee.ClearSelection();
            drpEmployee.Text = "";
            drpLoan.ClearSelection();
            drpLoan.Text = "";
            txt_commsn.Text = hdn_bankcommsn.Value = "0";
            txttax.Text = "";
            rptVdeposit.DataSource = null;
            rptVdeposit.DataBind();
            txtVdepositTotAmt.Text = "";
            divVdeposit.Visible = false;

            drpIncomeType.ClearSelection();
            drpIncomeType.Text = "";
            drpToType.ClearSelection();
            drpToType.Text = "";
            drpToTypeOnSelectedIndexChanged(null, null);
            drpPettyCash.ClearSelection();
            drpPettyCash.Text = "";
            drpBankAccount.ClearSelection();
            drpBankAccount.Text = "";
            drpLoanAccount.ClearSelection();
            drpLoanAccount.Text = "";
            txtAmountMain.ReadOnly = btnCompanyInvDetails.Visible = false;
            txtAmountMain.Text = "";
            dtChequeDate.DbSelectedDate = "";
            txtTransaction.Text = "";
            txtRemarks.Text = "";
            lblReceivable.Text = "";
            lblPayable.Text = "";

            txtTotal.Text = "0";
            txtAmtAuto.Text = txtAmtAutoCI.Text = "";
            hdnCustomerPaymentType.Value = "0";

            rpt_invoiceList.DataSource = null;
            rpt_invoiceList.DataBind();
            lblTReceivableAmount.Text = "";
            txtOutstandingAmount.Text = "";

            btnSave.Visible = hdn_add.Value == "0" ? false : true;
            btnSavePrint.Visible = hdn_add_N_print.Value == "0" ? false : true;
            btnPrint.Visible = false;
            btnOpenCancel.Visible = false;
            btnOpenDelete.Visible = false;
            btnInvDetails.Visible = false;
            Get_Code();

            Upd_Add_PanelInner.Update();
        }

        /*Code for Display*/
        public void Get_Code()
        {
            DataTable dt = obj_common.Get_Code(23);
            if (dt.Rows.Count > 0)
                lblCode.Text = dt.Rows[0][0].ToString();
        }

        /*Check Action Privilege*/
        public void previlage_action_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    DataTable dt = obj_common.Action_Previlage_Validation(23, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdn_update.Value = dt.Rows[1][1].ToString();
                        hdn_delete.Value = dt.Rows[2][1].ToString();
                        hdn_print.Value = dt.Rows[3][1].ToString();
                        hdn_add_N_print.Value = dt.Rows[4][1].ToString();
                        hdn_update_N_print.Value = dt.Rows[5][1].ToString();
                        hdn_cancel.Value = dt.Rows[6][1].ToString();
                        hdnsendmail.Value = dt.Rows[7][1].ToString();

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