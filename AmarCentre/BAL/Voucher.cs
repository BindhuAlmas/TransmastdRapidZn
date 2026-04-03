using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace AmarCentre.BAL
{
    public class Voucher
    {
        #region Common

        public DataTable DrpEmployeeTrans(int TransId)
        {
            Database_Operations db_obj = new Database_Operations("DrpEmployeeTrans", true);
            db_obj.AddParameter("@TransId", TransId);
            return db_obj.GetDataTable();
        }

        #endregion

        #region Receipt Voucher
        public DataSet GetCompanyGroupInvoiceDetails(int CompanyGroupId, int RVId)
        {
            Database_Operations db_obj = new Database_Operations("GetCompanyGroupInvoiceDetails", true);
            db_obj.AddParameter("@CompanyGroupId", CompanyGroupId);
            db_obj.AddParameter("@RVId", RVId);
            return (db_obj.GetDataSet());
        }
        public DataTable FillCompanyGroup()
        {
            Database_Operations db_obj = new Database_Operations("DrpCompanyGroup", true);
            return (db_obj.GetDataTable());
        }
        public DataTable fillParty()
        {
            Database_Operations db_obj = new Database_Operations("fillParty", true);
            return (db_obj.GetDataTable());
        }
        public DataTable fillasset()
        {
            Database_Operations db_obj = new Database_Operations("fillasset", true);
            return (db_obj.GetDataTable());
        }

        public DataTable GetEmployeeList()
        {
            Database_Operations db_obj = new Database_Operations("getEmployee", true);
            return (db_obj.GetDataTable());
        }
        public DataTable GetCustomer()
        {
            Database_Operations db_obj = new Database_Operations("getCustomer", true);
            return (db_obj.GetDataTable());
        }
        public DataTable GetLoan()
        {
            Database_Operations db_obj = new Database_Operations("getLoan", true);
            return (db_obj.GetDataTable());
        }
        public DataTable GetSupplier()
        {
            Database_Operations db_obj = new Database_Operations("GetSupplier", true);
            return (db_obj.GetDataTable());
        }
        public DataTable GetCreditCustomer() //changed to all customer in procedure
        {
            Database_Operations db_obj = new Database_Operations("getCreditCustomer", true);
            return (db_obj.GetDataTable());
        }
        public DataTable GetVendorList()
        {
            Database_Operations db_obj = new Database_Operations("getVendor", true);
            return (db_obj.GetDataTable());
        }
        public DataTable GetIncomeTypeList()
        {
            Database_Operations db_obj = new Database_Operations("getIncomeTypes", true);
            return (db_obj.GetDataTable());
        }
        public DataTable GetPettyCashAccountList(int UserId)
        {
            Database_Operations db_obj = new Database_Operations("getPettyCashAccount", true);
            db_obj.AddParameter("@UserId", UserId);
            return (db_obj.GetDataTable());
        }
        public DataTable GetPettyCashAccountList_QR(int UserId)
        {
            Database_Operations db_obj = new Database_Operations("getPettyCashAccount_QR", true);
            db_obj.AddParameter("@UserId", UserId);
            return (db_obj.GetDataTable());
        }
        public DataTable GetAllPettyCashAccountList()
        {
            Database_Operations db_obj = new Database_Operations("getAllPettyCashAccount", true);
            return (db_obj.GetDataTable());
        }
        public DataTable GetAllBankAccountList()
        {
            Database_Operations db_obj = new Database_Operations("getAllBankAccount", true);
            return (db_obj.GetDataTable());
        }
        public DataTable GetBankAccountList(int UserId)
        {
            Database_Operations db_obj = new Database_Operations("getPrivilegedBankAccount", true);
            db_obj.AddParameter("@UserId", UserId);
            return (db_obj.GetDataTable());
        }
        public DataTable GetNomadBankAccountList(int UserId)
        {
            Database_Operations db_obj = new Database_Operations("GetNomadBankAccountList", true);
            db_obj.AddParameter("@UserId", UserId);
            return (db_obj.GetDataTable());
        }
        public DataTable GetBankAccountList_QR(int UserId)
        {
            Database_Operations db_obj = new Database_Operations("getPrivilegedBankAccount_QR", true);
            db_obj.AddParameter("@UserId", UserId);
            return (db_obj.GetDataTable());
        }
        public DataTable GetEdhirhmBankAccountList_QR(int UserId)
        {
            Database_Operations db_obj = new Database_Operations("GetEdhirhmBankAccountList_QR", true);
            db_obj.AddParameter("@UserId", UserId);
            return (db_obj.GetDataTable());
        }
        public DataTable GetBankAccountListEdit(int UserId,int AccountId)
        {
            Database_Operations db_obj = new Database_Operations("getPrivilegedBankAccountEdit", true);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddParameter("@AccountId", AccountId);
            return (db_obj.GetDataTable());
        }
        public DataTable GetCustomerById(int Id)
        {
            Database_Operations db_obj = new Database_Operations("getCustomerById", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataTable());
        }
        public DataTable fill_drp_Agent()
        {
            Database_Operations db_obj = new Database_Operations("drp_Agent", true);
            return (db_obj.GetDataTable());
        }

        public DataTable GetCustomerInvoiceById(int Id)
        {
            Database_Operations db_obj = new Database_Operations("GetCustomerInvoiceById", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataTable());
        }

        public DataTable GetVendorById(int Id)
        {
            Database_Operations db_obj = new Database_Operations("getVendorById", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataTable());
        }
        public DataTable GetVendorCommissionbal(int Id)
        {
            Database_Operations db_obj = new Database_Operations("GetVendorCommissionbal", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataTable());
        }
        public DataTable GetEmployeeById(int Id)
        {
            Database_Operations db_obj = new Database_Operations("getEmployeeById", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataTable());
        }
        public DataTable GetLoanById(int Id)
        {
            Database_Operations db_obj = new Database_Operations("getLoanById", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataTable());
        }
        public DataSet GetCustOutStandingInvoiceList(int Id)
        {
            Database_Operations db_obj = new Database_Operations("GetCustOutStadInv", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataSet());
        }
        public DataSet GetCustOutStandingInvoiceList_CI(int Id)
        {
            Database_Operations db_obj = new Database_Operations("GetCustOutStadInv_CI", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataSet());
        }

        public int InsertUpdateReceiptVoucher(int Id,int Type, DateTime Date, int? CustomerId,
            int? VendorId, int? EmployeeId,int? LoanId, int IncomeId, double Amount,decimal? CustomerOutstandingAmount, int ToType,
            int? CashAccountId, int? BankAccountId, DateTime? ChequeDate, int ChequeStatus, DateTime? ChequeCollectedDate,
            String TransactionDetails, string Remarks, int CreatedBy, DataTable invoiceDetails,decimal? BankComssn,
            DataTable dtCIinvoiceDetails,int? DepositId,decimal TaxPercentage,decimal? ChargedAmount,int? LoanAccountId,
            DataTable dtvdeposit, int? PartyId,int? CustomerPaymentType, string Filenames, string FilenamesSave,
            int? AssetId,decimal CommissionVat, int? CompanyGroupId, DataTable dtCGinvoiceDetails)
        {
            Database_Operations db_obj = new Database_Operations("InsertUpdateReceiptVoucher", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@CompanyGroupId", CompanyGroupId);
            db_obj.AddParameter("@dtCGinvoiceDetails", dtCGinvoiceDetails);
            db_obj.AddParameter("@CommissionVat", CommissionVat);
            db_obj.AddParameter("@AssetId", AssetId);
            db_obj.AddParameter("@Filenames", Filenames);
            db_obj.AddParameter("@FilenamesSave", FilenamesSave);
            db_obj.AddParameter("@CustomerPaymentType", CustomerPaymentType);
            db_obj.AddParameter("@PartyId", PartyId);
            db_obj.AddParameter("@dtvdeposit", dtvdeposit);
            db_obj.AddParameter("@LoanAccountId", LoanAccountId);
            db_obj.AddParameter("@ChargedAmount", ChargedAmount);
            db_obj.AddParameter("@TaxPercentage", TaxPercentage);
            db_obj.AddParameter("@Type", Type);
            db_obj.AddParameter("@Date", Date);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@VendorId", VendorId);
            db_obj.AddParameter("@EmployeeId", EmployeeId);
            db_obj.AddParameter("@LoanId", LoanId);
            db_obj.AddParameter("@IncomeId", IncomeId);
            db_obj.AddParameter("@Amount", Amount);
            db_obj.AddParameter("@CustomerOutstandingAmount", CustomerOutstandingAmount);
            db_obj.AddParameter("@ToType", ToType);
            db_obj.AddParameter("@CashAccountId", CashAccountId);
            db_obj.AddParameter("@BankAccountId", BankAccountId);
            db_obj.AddParameter("@ChequeDate", ChequeDate);
            db_obj.AddParameter("@ChequeStatus", ChequeStatus);
            db_obj.AddParameter("@ChequeCollectedDate", ChequeCollectedDate);
            db_obj.AddParameter("@TransactionDetails", TransactionDetails);
            db_obj.AddParameter("@Remarks", Remarks);
            db_obj.AddParameter("@UserId", CreatedBy);
            db_obj.AddParameter("@InvDet", invoiceDetails);
            db_obj.AddParameter("@BankComssn", BankComssn);
            db_obj.AddParameter("@dtCIinvoiceDetails", dtCIinvoiceDetails);
            db_obj.AddParameter("@DepositId", DepositId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_List_ReceiptVoucher(int page_number, int page_size, string filter, string column, string order, int userid)
        {
            Database_Operations db_obj = new Database_Operations("List_ReceiptVoucher", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            db_obj.AddParameter("@userid", userid);
            return db_obj.GetDataTable();
        }

        public DataTable Get_List_ReceiptVoucher_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_ReceiptVoucher_Excel", true);
            return db_obj.GetDataTable();
        }

        public DataSet Edit_ReceiptVoucher(int Id)
        {
            Database_Operations db_obj = new Database_Operations("Edit_ReceiptVoucher", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataSet());
        }

        public int CancelDeleteReceiptVoucher(int Id,int Status,string CancellationRemark,int CreatedBy)
        {
            Database_Operations db_obj = new Database_Operations("CancelDeleteReceiptVoucher", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Status", Status);
            db_obj.AddParameter("@CancellationRemark", CancellationRemark);
            db_obj.AddParameter("@UserId", CreatedBy);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable VdepositList(int VendorId)
        {
            Database_Operations db_obj = new Database_Operations("VdepositList", true);
            db_obj.AddParameter("@VendorId", VendorId);
            return (db_obj.GetDataTable());
        }

        public DataTable EditVdepositList(int VendorId,int RVId)
        {
            Database_Operations db_obj = new Database_Operations("EditVdepositList", true);
            db_obj.AddParameter("@VendorId", VendorId);
            db_obj.AddParameter("@RVId", RVId);
            return (db_obj.GetDataTable());
        }

        #endregion

        #region Payment Voucher

        public DataTable GetTaxAmount()
        {
            Database_Operations db_obj = new Database_Operations("GetTaxAmount", true);
            return (db_obj.GetDataTable());
        }

        public DataTable getCustomerCommissionBalance(int CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("getCustomerCommissionBalance", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            return (db_obj.GetDataTable());
        }

        public DataTable CdepositList(int CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("CdepositList", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            return (db_obj.GetDataTable());
        }

        public DataTable EditCdepositList(int CustomerId, int PVId)
        {
            Database_Operations db_obj = new Database_Operations("EditCdepositList", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@PVId", PVId);
            return (db_obj.GetDataTable());
        }

        public DataTable AgentProfitBalance(int AgentId)
        {
            Database_Operations db_obj = new Database_Operations("AgentProfitBalance", true);
            db_obj.AddParameter("@AgentId", AgentId);
            return (db_obj.GetDataTable());
        }

        public DataTable GetDepositTypeList()
        {
            Database_Operations db_obj = new Database_Operations("drpDepositType", true);
            return (db_obj.GetDataTable());
        }

        public DataTable GetPartnerList()
        {
            Database_Operations db_obj = new Database_Operations("drpPartner", true);
            return (db_obj.GetDataTable());
        }

        public DataTable GetExpenseList()
        {
            Database_Operations db_obj = new Database_Operations("GetGeneralExpenseList", true);
            return (db_obj.GetDataTable());

        }
        public DataTable GetPettycashById(int Id)
        {
            Database_Operations db_obj = new Database_Operations("GetPettyCashById", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataTable());
        }
        public DataTable GetBankAccountById(int Id)
        {
            Database_Operations db_obj = new Database_Operations("GetBankAccountById", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataTable());
        }
        public DataTable GetExpenseTaxAmount(int ExpenseId)
        {
            Database_Operations db_obj = new Database_Operations("GetExpenseTaxAmount", true);
            db_obj.AddParameter("@Id", ExpenseId);
            return (db_obj.GetDataTable());

        }
        public DataTable GetCommissionApplicableCustomer()
        {
            Database_Operations db_obj = new Database_Operations("getCommissionApplicableCustomer", true);
            return (db_obj.GetDataTable());
        }
        public DataTable List_PaymentVoucher(int page_number, int page_size, string filter, string column, string order, int userid)
        {
            Database_Operations db_obj = new Database_Operations("List_PaymentVoucher", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            db_obj.AddParameter("@userid", userid);
            return db_obj.GetDataTable();
        }

        public DataTable Get_List_PaymentVoucher_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_PaymentVoucher_Excel", true);
            return db_obj.GetDataTable();
        }

        public DataSet Edit_PaymentVoucher(int Id)
        {
            Database_Operations db_obj = new Database_Operations("Edit_PaymentVoucher", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataSet());
        }

        public int PaymentVoucherInsertUpdate(int? Id,DateTime Date,int ToType,int? CustomerId,
            int? VendorId, int? EmployeeId, int? CashAccountId, int? BankAccountId,int? LoanId,int ExpenseType,int FromType,
            int? CashAccountIdFrom,int? BankAccountIdFrom,double Amount,double Commission,double Tax,DateTime? ChequeDate,int ChequeStatus,
            DateTime? ChequeCollectionDate,String TransactionDetails,string Remarks,int TaxType,int CreatedBy,int? EmpSubType,int? PartnerId,
            int? depositId,int? depreciationPeriod,int? AgentId, DataTable dtvdeposit,int? LoanFromId,int? SupplierId,
            string Filenames,string FilenamesSave,string BillNo
            )
        {
            int? nulls = null;
            Database_Operations db_obj = new Database_Operations("PaymentVoucherInsertUpdate", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Date", Date);
            db_obj.AddParameter("@Filenames", Filenames);
            db_obj.AddParameter("@FilenamesSave", FilenamesSave);
            db_obj.AddParameter("@SupplierId", SupplierId);
            db_obj.AddParameter("@LoanFromId", LoanFromId);
            db_obj.AddParameter("@dtvdeposit", dtvdeposit);
            db_obj.AddParameter("@ToType", ToType);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@VendorId", VendorId);
            db_obj.AddParameter("@EmployeeId", EmployeeId);
            db_obj.AddParameter("@CashAccountId", CashAccountId);
            db_obj.AddParameter("@BankAccountId", BankAccountId);
            db_obj.AddParameter("@LoanId", LoanId);
            db_obj.AddParameter("@ExpenseType", ExpenseType);
            db_obj.AddParameter("@FromType", FromType);
            db_obj.AddParameter("@CashAccountIdFrom", CashAccountIdFrom);
            db_obj.AddParameter("@BankAccountIdFrom", BankAccountIdFrom);
            db_obj.AddParameter("@Amount", Amount);
            db_obj.AddParameter("@Commission", Commission);
            db_obj.AddParameter("@TaxType", TaxType);
            db_obj.AddParameter("@Tax", Tax);
            db_obj.AddParameter("@ChequeDate", ChequeDate);
            db_obj.AddParameter("@ChequeStatus", ChequeStatus);
            db_obj.AddParameter("@ChequeCollectionDate", ChequeCollectionDate);
            db_obj.AddParameter("@TransactionDetails", TransactionDetails);
            db_obj.AddParameter("@Remarks", Remarks);
            db_obj.AddParameter("@UserId", CreatedBy);
            db_obj.AddParameter("@EmpSubType", EmpSubType);
            db_obj.AddParameter("@PartnerId", PartnerId);
            db_obj.AddParameter("@depositId", depositId);
            db_obj.AddParameter("@depreciationPeriod", depreciationPeriod);
            db_obj.AddParameter("@AgentId", AgentId);
            db_obj.AddParameter("@BillNo", BillNo);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int CancelDeletePaymentVoucher(int Id, int Status,string CancellationRemark, int CreatedBy)
        {
            Database_Operations db_obj = new Database_Operations("CancelDeletePaymentVoucher", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Status", Status);
            db_obj.AddParameter("@CancellationRemark", CancellationRemark);
            db_obj.AddParameter("@UserId", CreatedBy);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region PCC

        public DataTable GetPendingCardList(string Filter, int? BankAccountId,int status=0)
        {
            Database_Operations db_obj = new Database_Operations("GetPendingCardList", true);
            db_obj.AddParameter("@Filter", Filter);
            db_obj.AddParameter("@BankAccountId", BankAccountId);
            db_obj.AddParameter("@status", status);
            return (db_obj.GetDataTable());
        }

        public int CollectCardRCRV(int Id, DateTime? Collectdate,int BankAccountId,int Typeid, int CreatedBy,
            decimal BankCommission,decimal CommissionVat)
        {
            Database_Operations db_obj = new Database_Operations("CollectCardRCRV", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@CollectionDate", Collectdate);
            db_obj.AddParameter("@BankAccountId", BankAccountId);
            db_obj.AddParameter("@Typeid", Typeid);
            db_obj.AddParameter("@UserId", CreatedBy);
            db_obj.AddParameter("@BankCommission", BankCommission);
            db_obj.AddParameter("@CommissionVat", CommissionVat);

            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region PDC

        public DataTable PDCList(int StatusId,string Filter)
        {
            Database_Operations db_obj = new Database_Operations("PDCList", true);
            db_obj.AddParameter("@StatusId", StatusId);
            db_obj.AddParameter("@Filter", Filter);
            return (db_obj.GetDataTable());
        }

        public int ClosingPaymentCheque(int Id,DateTime closingDate, int CreatedBy)
        {
            Database_Operations db_obj = new Database_Operations("closingPaymentCheque", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@CollectionDate", closingDate);
            db_obj.AddParameter("@UserId", CreatedBy);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }
        public int ClosingReceiptVoucherCheque(int Id, int BankId,DateTime closingDate,int CreatedBy)
        {
            Database_Operations db_obj = new Database_Operations("ClsoingReceiptCheque", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@BankId", BankId);
            db_obj.AddParameter("@CollectionDate", closingDate);
            db_obj.AddParameter("@UserId", CreatedBy);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }
        public int ClosingReceiptCheque(int Id, int BankId, DateTime closingDate, int CreatedBy)
        {
            Database_Operations db_obj = new Database_Operations("ClsoingMainReceiptCheque", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@BankId", BankId);
            db_obj.AddParameter("@CollectionDate", closingDate);
            db_obj.AddParameter("@UserId", CreatedBy);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }
        #endregion



    }
}