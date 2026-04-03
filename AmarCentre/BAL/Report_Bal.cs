using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace AmarCentre.BAL
{
    public class Report_Bal
    {
        #region Common
        public DataTable FillCompanyGroup()
        {
            Database_Operations db_obj = new Database_Operations("DrpCompanyGroup", true);
            return (db_obj.GetDataTable());
        }
        public DataTable DrpServiceStatuslist()
        {
            Database_Operations db_obj = new Database_Operations("DrpServiceStatuslist", true);
            return (db_obj.GetDataTable());
        }

        public DataTable fill_drp_CustomerStaff(int CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("drp_CustomerStaff", true);
            db_obj.AddParameter("@CustId", CustomerId);
            return (db_obj.GetDataTable());
        }

        public DataTable drp_CustomerStaffForExpiry(int CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("drp_CustomerStaffExpiry", true);
            db_obj.AddParameter("@CustId", CustomerId);
            return (db_obj.GetDataTable());
        }

        public DataTable fillParty()
        {
            Database_Operations db_obj = new Database_Operations("fillParty", true);
            return (db_obj.GetDataTable());
        }

        public DataTable fill_drp_Agent()
        {
            Database_Operations db_obj = new Database_Operations("drp_Agent", true);
            return (db_obj.GetDataTable());
        }

        public DataTable GetPartnerList()
        {
            Database_Operations db_obj = new Database_Operations("drpPartner", true);
            return (db_obj.GetDataTable());
        }

        public DataTable Drp_Employee()
        {
            Database_Operations db_obj = new Database_Operations("drp_Employee", true);
            return db_obj.GetDataTable();
        }

        public DataTable Drp_Customer()
        {
            Database_Operations db_obj = new Database_Operations("drp_Customer", true);
            return (db_obj.GetDataTable());
        }

        public DataTable Drp_Applicant(int CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("Drp_Applicant", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            return (db_obj.GetDataTable());
        }
        public DataTable Drp_Applicantdt_cust(DataTable dt_cust)
        {
            Database_Operations db_obj = new Database_Operations("Drp_Applicantdt_cust", true);
            db_obj.AddParameter("@dt_cust", dt_cust);
            return (db_obj.GetDataTable());
        }
        public DataTable DrpApplicantPending()
        {
            Database_Operations db_obj = new Database_Operations("DrpApplicantPending", true);
            return (db_obj.GetDataTable());
        }

        public DataTable Drp_Service(int Cat_id) // Cat_id=0 for all
        {
            Database_Operations db_obj = new Database_Operations("drp_service_cat", true);
            db_obj.AddParameter("@Cat_id", Cat_id);
            return (db_obj.GetDataTable());
        }

        public DataTable Drp_ServiceDep(int D_id) // D_id=0 for all
        {
            Database_Operations db_obj = new Database_Operations("Drp_ServiceDep", true);
            db_obj.AddParameter("@D_id", D_id);
            return (db_obj.GetDataTable());
        }

        public DataTable Drp_Bank(int UserId)
        {
            Database_Operations db_obj = new Database_Operations("getPrivilegedBankAccount", true);
            db_obj.AddParameter("@UserId", UserId);
            return (db_obj.GetDataTable());
        }
        public DataSet GetBankBalancebyUser(int userId = 1)
        {
            Database_Operations db = new Database_Operations("GetBankBalancebyUser", true);
            db.AddParameter("@userId", userId);
            return db.GetDataSet();
        }

        public DataTable Drp_Income()
        {
            Database_Operations db_obj = new Database_Operations("Drp_Income", true);
            return (db_obj.GetDataTable());
        }

        public DataTable Drp_Income_RVOther()
        {
            Database_Operations db_obj = new Database_Operations("Drp_Income_RVOther", true);
            return (db_obj.GetDataTable());
        }

        public DataTable Drp_Expense()
        {
            Database_Operations db_obj = new Database_Operations("Drp_Expense", true);
            return (db_obj.GetDataTable());
        }

        public DataTable Drp_Expense_PVOther()
        {
            Database_Operations db_obj = new Database_Operations("Drp_Expense_PVOther", true);
            return (db_obj.GetDataTable());
        }

        public DataTable GetServiceList(int ServiceType)
        {
            Database_Operations db_obj = new Database_Operations("GetServiceList", true);
            db_obj.AddParameter("@ServiceType", ServiceType);
            return (db_obj.GetDataTable());
        }

        public DataTable GetServiceListdt(DataTable dtServiceType)
        {
            Database_Operations db_obj = new Database_Operations("GetServiceListdt", true);
            db_obj.AddParameter("@dtServiceType", dtServiceType);
            return (db_obj.GetDataTable());
        }

        public DataTable Drp_Invoice()
        {
            Database_Operations db_obj = new Database_Operations("drp_Invoice", true);
            return (db_obj.GetDataTable());
        }

        public DataTable Drp_Department()
        {
            Database_Operations db_obj = new Database_Operations("drp_Department", true);
            return (db_obj.GetDataTable());
        }

        public DataTable Drp_ServiceFilterByDep(DataTable dt_department)
        {
            Database_Operations db_obj = new Database_Operations("drp_ServiceFilterByDep", true);
            db_obj.AddParameter("@dt_department", dt_department);
            return (db_obj.GetDataTable());
        }

        public DataTable GetPettyCashAccountList(int UserId)
        {
            Database_Operations db_obj = new Database_Operations("getPettyCashAccount", true);
            db_obj.AddParameter("@UserId", UserId);
            return (db_obj.GetDataTable());
        }

        public DataTable Drp_Loan()
        {
            Database_Operations db_obj = new Database_Operations("Drp_Loan", true);
            return (db_obj.GetDataTable());
        }

        public DataTable Drp_Vendor()
        {
            Database_Operations db_obj = new Database_Operations("Drp_Vendor", true);
            return (db_obj.GetDataTable());
        }

        #endregion

        #region prints

        public DataSet CashReceiptPrint3(int id)
        {
            Database_Operations db_obj = new Database_Operations("CashReceiptPrint3", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public DataSet DocTransfrPRint(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("DocTransferPrint", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public DataSet DocReturnAgent(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("DocReturnAgemtPrint", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public DataSet DocumentReturnPrint(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("DocReturnPrint", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public DataSet DocumentCollectionPrint(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("DocCollectionPrint", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public DataSet Invoice_Print(int id)
        {
            Database_Operations db_obj = new Database_Operations("Print_Invoice", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public DataSet SalesOrderPrint(int id)
        {
            Database_Operations db_obj = new Database_Operations("SalesOrderPrint", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public DataSet TaxInvoicePrint(int id)
        {
            Database_Operations db_obj = new Database_Operations("TaxInvoicePrint", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public DataSet TaxInvoicePrint11_12(int id)
        {
            Database_Operations db_obj = new Database_Operations("TaxInvoicePrint11_12", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public DataSet TaxInvoicePrint13(int id)
        {
            Database_Operations db_obj = new Database_Operations("TaxInvoicePrint13", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public DataSet CreditPrint(int id)
        {
            Database_Operations db_obj = new Database_Operations("CreditPrint", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public DataSet SalesOrderPrintFormat2(int id)
        {
            Database_Operations db_obj = new Database_Operations("SalesOrderPrintFormat2", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public DataSet TaxInvoicePrintFormat2(int id)
        {
            Database_Operations db_obj = new Database_Operations("TaxInvoicePrintFormat2", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public DataSet CustomerInvoicePrintFormat2(int id)
        {
            Database_Operations db_obj = new Database_Operations("CustomerInvoicePrintFormat2", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public DataSet CustomerInvoicePrintFormat1(int id)
        {
            Database_Operations db_obj = new Database_Operations("CustomerInvoicePrintFormat1", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public DataSet Receipt_Print(int id)
        {
            Database_Operations db_obj = new Database_Operations("Receipt_Print", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public DataSet CashReceiptPrint(int id)
        {
            Database_Operations db_obj = new Database_Operations("CashReceiptPrint", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }
        public DataSet CashReceiptPrintF2(int id)
        {
            Database_Operations db_obj = new Database_Operations("CashReceiptPrintF2", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }
        public DataSet CashCustomerReceiptPrint(int id)
        {
            Database_Operations db_obj = new Database_Operations("CashCustomerReceiptPrint", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public DataSet ReceiptVoucher_Print(int id)
        {
            Database_Operations db_obj = new Database_Operations("ReceiptVoucher_Print", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public DataSet PaymentVoucher_Print(int id)
        {
            Database_Operations db_obj = new Database_Operations("PaymentVoucher_Print", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public DataSet QuotationPrint(int id)
        {
            Database_Operations db_obj = new Database_Operations("QuotationPrint", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public DataSet QuotationPrintFormat2(int id)
        {
            Database_Operations db_obj = new Database_Operations("QuotationPrintFormat2", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }
        public DataSet QuotationPrintFormat8(int id)
        {
            Database_Operations db_obj = new Database_Operations("QuotationPrintFormat8", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }
        public DataSet Salary_processPrint(int id)
        {
            Database_Operations db_obj = new Database_Operations("Print_SAlaryProcess", true);
            db_obj.AddParameter("@SalId", id);
            return (db_obj.GetDataSet());
        }
        #endregion

        #region Reports

        public DataSet CustomerDocumentforC(DateTime? Fromdate, DateTime? Todate, int? C_id, int? doctype)
        {
            Database_Operations db_obj = new Database_Operations("CustomerDocumentforC", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@C_id", C_id);
            db_obj.AddParameter("@doctype", doctype);
            return db_obj.GetDataSet();
        }
        public DataSet CustomerDepositDetailList(int? CustomerId, DateTime? fromdate, DateTime? Todate, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("CustomerDepositDetailList", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@fromdate", fromdate);
            db_obj.AddParameter("@Todate", Todate);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerDepositSummaryPdf()
        {
            Database_Operations db_obj = new Database_Operations("CustomerDepositSummaryPdf", true);
            return db_obj.GetDataSet();
        }
        public DataSet CustomerDepositDetPdf()
        {
            Database_Operations db_obj = new Database_Operations("CustomerDepositDetPdf", true);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerDepositDetailExcel(int? CustomerId, DateTime? fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("CustomerDepositDetailExcel", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@fromdate", fromdate);
            db_obj.AddParameter("@Todate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet VendorDepositDetailList(int? VendorId, DateTime? fromdate, DateTime? Todate, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("VendorDepositDetailList", true);
            db_obj.AddParameter("@VendorId", VendorId);
            db_obj.AddParameter("@fromdate", fromdate);
            db_obj.AddParameter("@Todate", Todate);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet VendorDepositSummaryPdf()
        {
            Database_Operations db_obj = new Database_Operations("VendorDepositSummaryPdf", true);
            return db_obj.GetDataSet();
        }

        public DataSet VendorDepositDetailExcel(int? VendorId, DateTime? fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("VendorDepositDetailExcel", true);
            db_obj.AddParameter("@VendorId", VendorId);
            db_obj.AddParameter("@fromdate", fromdate);
            db_obj.AddParameter("@Todate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet DebitorsAgeing(int? CustomerId, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("DebitorsAgeing", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet DebitorsAgeingPdf(int? CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("DebitorsAgeingPdf", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            return db_obj.GetDataSet();
        }

        public DataSet DebitorsAgeingExcel(int? CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("DebitorsAgeingExcel", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            return db_obj.GetDataSet();
        }

        public DataTable GetDocumentexpirylist(DateTime? Fromdate, DateTime? Todate, int? CustomerId, DataTable dtdoc, int? AgentId,
             string CustomerStaff)
        {
            Database_Operations db_obj = new Database_Operations("GetDocumentexpirylist", true);
            db_obj.AddParameter("@Fromdate", Fromdate);
            db_obj.AddParameter("@Todate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@dtdoc", dtdoc);
            db_obj.AddParameter("@AgentId", AgentId);
            db_obj.AddParameter("@CustomerStaff", CustomerStaff);
            return db_obj.GetDataTable();
        }

        public DataSet PartnerStatment(int PartnerId, DateTime? Fromdate, DateTime? Todate, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("PartnerStatment", true);
            db_obj.AddParameter("@PartnerId", PartnerId);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet PartnerStatmentExcel(int PartnerId, DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("PartnerStatmentExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@PartnerId", PartnerId);
            return db_obj.GetDataSet();
        }

        public DataSet Document_collectionExcel(int? Cust_id, int? StatuId)
        {
            Database_Operations db_obj = new Database_Operations("DocumnetColl_Excel", true);
            db_obj.AddParameter("@CustId", Cust_id);
            db_obj.AddParameter("@StatuId", StatuId);
            return (db_obj.GetDataSet());
        }

        public DataSet Document_collectionAgentExcel(int? Cust_id, int? Agnt_id)
        {
            Database_Operations db_obj = new Database_Operations("DocumnetCollAgentExcel", true);
            db_obj.AddParameter("@CustId", Cust_id);
            db_obj.AddParameter("@Agnt_id", Agnt_id);
            return (db_obj.GetDataSet());
        }

        public DataSet Document_collection_List(int? Cust_id, int? StatuId, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("DocumnetColl_List", true);
            db_obj.AddParameter("@CustId", Cust_id);
            db_obj.AddParameter("@StatuId", StatuId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return (db_obj.GetDataSet());
        }

        public DataSet Document_collectionAgent_List(int? Cust_id, int? Agnt_id, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("DocumnetCollAgent_List", true);
            db_obj.AddParameter("@CustId", Cust_id);
            db_obj.AddParameter("@Agnt_id", Agnt_id);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return (db_obj.GetDataSet());
        }

        public DataSet SponserCompanylist(int? SponserId, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("SponserCompanylist", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@SponserId", SponserId);
            return db_obj.GetDataSet();
        }

        public DataSet SponserCompanyExcel(int? SponserId)
        {
            Database_Operations db_obj = new Database_Operations("SponserCompanyExcel", true);
            db_obj.AddParameter("@SponserId", SponserId);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerDocument(DateTime? Fromdate, DateTime? Todate, int page_number, int page_size, DataTable dt_cust, int? SponserId, int? AgentId)
        {
            Database_Operations db_obj = new Database_Operations("CustomerDocument", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@dt_cust", dt_cust);
            db_obj.AddParameter("@SponserId", SponserId);
            db_obj.AddParameter("@AgentId", AgentId);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerDocumentExcel(DateTime? Fromdate, DateTime? Todate, DataTable dt_cust, int? SponserId, int? AgentId)
        {
            Database_Operations db_obj = new Database_Operations("CustomerDocumentExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@dt_cust", dt_cust);
            db_obj.AddParameter("@SponserId", SponserId);
            db_obj.AddParameter("@AgentId", AgentId);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerDocumentExcelF2(DateTime? Fromdate, DateTime? Todate, DataTable dt_cust, int? SponserId, int? AgentId)
        {
            Database_Operations db_obj = new Database_Operations("CustomerDocumentExcelF2", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@dt_cust", dt_cust);
            db_obj.AddParameter("@SponserId", SponserId);
            db_obj.AddParameter("@AgentId", AgentId);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerDocumentStaff(DateTime? Fromdate, DateTime? Todate, int page_number, int page_size, int? C_id,
     int? SponserId, string CustomerStaff, int? AgentId)
        {
            Database_Operations db_obj = new Database_Operations("CustomerDocumentStaff", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@C_id", C_id);
            db_obj.AddParameter("@SponserId", SponserId);
            db_obj.AddParameter("@CustomerStaff", CustomerStaff);
            db_obj.AddParameter("@AgentId", AgentId);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerDocumentStaffExcel(DateTime? Fromdate, DateTime? Todate, int? C_id, int? SponserId, string CustomerStaff, int? AgentId)
        {
            Database_Operations db_obj = new Database_Operations("CustomerDocumentStaffExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@C_id", C_id);
            db_obj.AddParameter("@SponserId", SponserId);
            db_obj.AddParameter("@CustomerStaff", CustomerStaff);
            db_obj.AddParameter("@AgentId", AgentId);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerDocumentStaffExcelF2(DateTime? Fromdate, DateTime? Todate, int? C_id, int? SponserId, string CustomerStaff, int? AgentId)
        {
            Database_Operations db_obj = new Database_Operations("CustomerDocumentStaffExcelF2", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@C_id", C_id);
            db_obj.AddParameter("@SponserId", SponserId);
            db_obj.AddParameter("@CustomerStaff", CustomerStaff);
            db_obj.AddParameter("@AgentId", AgentId);
            return db_obj.GetDataSet();
        }


        public DataSet CustomerSOAPrintFormat3(DateTime? Fromdate, DateTime? Todate, int CustomerId, int PaymentStatus, int CompletionStatus)
        {
            Database_Operations db_obj = new Database_Operations("CustomerSOAPrintFormat3", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@PaymentStatus", PaymentStatus);
            db_obj.AddParameter("@CompletionStatus", CompletionStatus);
            return db_obj.GetDataSet();
        }

        public DataTable BankReconciliationStatement(DateTime Fromdate, DateTime Todate, int AccountId,
            int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("BankReconciliationStatement", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@Account", AccountId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataTable();
        }

        public DataTable BankReconciliationStatementExcel(DateTime Fromdate, DateTime Todate, int AccountId)
        {
            Database_Operations db_obj = new Database_Operations("BankReconciliationStatementExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@Account", AccountId);
            return db_obj.GetDataTable();
        }

        public DataTable Get_SC_Report(DateTime? Fromdate, DateTime? Todate, DataTable dt_service, DataTable dt_cust, DataTable dt_bank,
    DataTable dt_department, int? EmployeeId, int? AgentId, int? ServiceStatusId,string SearchText,
    int page_number,
     int page_size)
        {
            Database_Operations db_obj = new Database_Operations("Get_SC_Report", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@dt_service", dt_service);
            db_obj.AddParameter("@dt_cust", dt_cust);
            db_obj.AddParameter("@dt_bank", dt_bank);
            db_obj.AddParameter("@dt_department", dt_department);
            db_obj.AddParameter("@EmployeeId", EmployeeId);
            db_obj.AddParameter("@AgentId", AgentId);
            db_obj.AddParameter("@ServiceStatusId", ServiceStatusId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@SearchText", SearchText);
            return db_obj.GetDataTable();
        }
        public DataTable Get_SC_Excel(DateTime? Fromdate, DateTime? Todate, DataTable dt_service, DataTable dt_cust, DataTable dt_bank,
     DataTable dt_department, int? EmployeeId, int? AgentId, int? ServiceStatusId, string SearchText)
        {
            Database_Operations db_obj = new Database_Operations("Get_SC_Excel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@dt_service", dt_service);
            db_obj.AddParameter("@dt_cust", dt_cust);
            db_obj.AddParameter("@dt_bank", dt_bank);
            db_obj.AddParameter("@dt_department", dt_department);
            db_obj.AddParameter("@EmployeeId", EmployeeId);
            db_obj.AddParameter("@AgentId", AgentId);
            db_obj.AddParameter("@ServiceStatusId", ServiceStatusId);
            db_obj.AddParameter("@SearchText", SearchText);

            return db_obj.GetDataTable();
        }

        public DataSet Get_income_Report(DateTime? Fromdate, DateTime? Todate, DataTable dt_income,
           int page_number, int page_size, int? frmtype)
        {
            Database_Operations db_obj = new Database_Operations("Get_income_Report", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@dt_income", dt_income);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@frmtype", frmtype);

            return db_obj.GetDataSet();
        }

        public DataSet Get_income_Excel(DateTime? Fromdate, DateTime? Todate, DataTable dt_income, int? frmtype)
        {
            Database_Operations db_obj = new Database_Operations("Get_income_Excel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@dt_income", dt_income);
            db_obj.AddParameter("@frmtype", frmtype);

            return db_obj.GetDataSet();
        }

        public DataSet Get_expense_Report(DateTime? Fromdate, DateTime? Todate, DataTable dt_expense, int page_number, int page_size, int? totype)
        {
            Database_Operations db_obj = new Database_Operations("Get_expense_Report", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@dt_expense", dt_expense);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@totype", totype);

            return db_obj.GetDataSet();
        }

        public DataSet Get_Expense_Excel(DateTime? Fromdate, DateTime? Todate, DataTable dt_expense, int? totype)
        {
            Database_Operations db_obj = new Database_Operations("Get_Expense_Excel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@dt_expense", dt_expense);
            db_obj.AddParameter("@totype", totype);

            return db_obj.GetDataSet();
        }

        public DataSet Get_userservice_Report(DateTime? Fromdate, DateTime? Todate, int E_id, DataTable dtServiceType, DataTable dtService,
             DataTable dtCustomer, DataTable dtDepartment, DataTable dtInvoice, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("Get_userservice_ReportNew", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@E_id", E_id);
            db_obj.AddParameter("@dtServiceType", dtServiceType);
            db_obj.AddParameter("@dtService", dtService);
            db_obj.AddParameter("@dtCustomer", dtCustomer);
            db_obj.AddParameter("@dtDepartment", dtDepartment);
            db_obj.AddParameter("@dtInvoice", dtInvoice);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        //public DataSet Get_userservce_Excel(DateTime? Fromdate, DateTime? Todate, int E_id,int ServiceType, int? S_id)
        //{
        //    Database_Operations db_obj = new Database_Operations("Get_userservce_Excel", true);
        //    db_obj.AddParameter("@FromDate", Fromdate);
        //    db_obj.AddParameter("@ToDate", Todate);
        //    db_obj.AddParameter("@E_id", E_id);
        //    db_obj.AddParameter("@ServiceType", ServiceType);
        //    db_obj.AddParameter("@S_id", S_id);
        //    return db_obj.GetDataSet();
        //}

        public DataSet Get_userservce_Excel(DateTime? Fromdate, DateTime? Todate, int E_id, DataTable dtServiceType, DataTable dtService,
             DataTable dtCustomer, DataTable dtDepartment, DataTable dtInvoice)
        {
            Database_Operations db_obj = new Database_Operations("Get_userservce_Excelnew", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@E_id", E_id);
            db_obj.AddParameter("@dtServiceType", dtServiceType);
            db_obj.AddParameter("@dtService", dtService);
            db_obj.AddParameter("@dtCustomer", dtCustomer);
            db_obj.AddParameter("@dtDepartment", dtDepartment);
            db_obj.AddParameter("@dtInvoice", dtInvoice);
            return db_obj.GetDataSet();
        }

        public DataSet Get_debitors_Report(int page_number, int page_size, string filter, int Ctype)
        {
            Database_Operations db_obj = new Database_Operations("Get_debitors_Report", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@filter", filter);
            db_obj.AddParameter("@Ctype", Ctype);
            return db_obj.GetDataSet();
        }

        public DataSet Get_debitors_ReportFromat2(int page_number, int page_size, string filter, int Ctype)
        {
            Database_Operations db_obj = new Database_Operations("Get_debitors_ReportFromat2", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@filter", filter);
            db_obj.AddParameter("@Ctype", Ctype);
            return db_obj.GetDataSet();
        }

        public DataSet Get_debitorsReportCustomer(int CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("Get_debitorsReportCustomer", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            return db_obj.GetDataSet();
        }

        public DataSet Get_debitorsReportFromat2Customer(int CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("Get_debitorsReportFromat2Customer", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            return db_obj.GetDataSet();
        }

        public DataSet Get_debitors_Excel(int Ctype)
        {
            Database_Operations db_obj = new Database_Operations("Get_debitors_Excel", true);
            db_obj.AddParameter("@Ctype", Ctype);
            return db_obj.GetDataSet();
        }

        public DataSet Get_debitors_ExcelFromat2(int Ctype)
        {
            Database_Operations db_obj = new Database_Operations("Get_debitors_ExcelFromat2", true);
            db_obj.AddParameter("@Ctype", Ctype);
            return db_obj.GetDataSet();
        }

        public DataSet Detaileddebitors_Report(int page_number, int page_size, DateTime? FromDate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("Detaileddebitors_Report", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@FromDate", FromDate);
            db_obj.AddParameter("@Todate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet Detaileddebitors_ReportF2(int page_number, int page_size, DateTime? FromDate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("Detaileddebitors_ReportF2", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@FromDate", FromDate);
            db_obj.AddParameter("@Todate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet Detaileddebitors_Excel(DateTime? FromDate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("Detaileddebitors_Excel", true);
            db_obj.AddParameter("@FromDate", FromDate);
            db_obj.AddParameter("@Todate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet Detaileddebitors_ExcelF2(DateTime? FromDate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("Detaileddebitors_ExcelF2", true);
            db_obj.AddParameter("@FromDate", FromDate);
            db_obj.AddParameter("@Todate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet SundryDebitorsReport(int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("SundryDebitorsReport", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet SundryDebitorsExcelPdf()
        {
            Database_Operations db_obj = new Database_Operations("SundryDebitorsExcelPdf", true);
            return db_obj.GetDataSet();
        }

        public DataTable Get_CustomerAdvance_Report(int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("Get_CustomerAdvance_Report", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataTable();
        }

        public DataSet Get_CustomerAdvance_Excel()
        {
            Database_Operations db_obj = new Database_Operations("Get_CustomerAdvance_Excel", true);
            return db_obj.GetDataSet();
        }

        public DataSet ServiceProfitStatementNew(DateTime? Fromdate, DateTime? Todate, DataTable dtCustomer, DataTable dtDepartment,
      DataTable dtService, DataTable dtInvoice, DataTable dtEmply, int page_number, int page_size, int? VendorId, int? AgentId)
        {
            Database_Operations db_obj = new Database_Operations("ServiceProfitStatementNew", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@dtCustomer", dtCustomer);
            db_obj.AddParameter("@dtDepartment", dtDepartment);
            db_obj.AddParameter("@dtService", dtService);
            db_obj.AddParameter("@dtInvoice", dtInvoice);
            db_obj.AddParameter("@dtEmply", dtEmply);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@VendorId", VendorId);
            db_obj.AddParameter("@AgentId", AgentId);
            return db_obj.GetDataSet();
        }


        public DataSet ServiceProfitStatementExcelNew(DateTime? Fromdate, DateTime? Todate, DataTable dtCustomer, DataTable dtDepartment,
            DataTable dtService, DataTable dtInvoice, DataTable dtEmply, int? VendorId, int? AgentId)
        {
            Database_Operations db_obj = new Database_Operations("ServiceProfitStatementExcelNew", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@dtCustomer", dtCustomer);
            db_obj.AddParameter("@dtDepartment", dtDepartment);
            db_obj.AddParameter("@dtService", dtService);
            db_obj.AddParameter("@dtInvoice", dtInvoice);
            db_obj.AddParameter("@dtEmply", dtEmply);
            db_obj.AddParameter("@VendorId", VendorId);
            db_obj.AddParameter("@AgentId", AgentId);
            return db_obj.GetDataSet();
        }

        public DataSet ServiceProfitSummary(DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("ServiceProfitSummary", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            return db_obj.GetDataSet();
        }

        public DataTable UserLogReport(DateTime? Fromdate, DateTime? Todate, int? UserId, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("UserLogReport", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataTable();
        }

        public DataTable UserLogExcel(DateTime? Fromdate, DateTime? Todate, int? UserId)
        {
            Database_Operations db_obj = new Database_Operations("UserLogExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@UserId", UserId);
            return db_obj.GetDataTable();
        }

        public DataSet BankStatementReport(DateTime Fromdate, DateTime Todate, int AccountId, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("BankStatement", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@Account", AccountId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet BankStatementExcel(DateTime Fromdate, DateTime Todate, int AccountId)
        {
            Database_Operations db_obj = new Database_Operations("BankStatementExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@Account", AccountId);
            return db_obj.GetDataSet();
        }

        public DataSet BankStatementVersion2(DateTime? Fromdate, DateTime? Todate, int AccountId, int page_number, int page_size, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("BankStatementVersion2", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@Account", AccountId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@UserId", UserId);
            return db_obj.GetDataSet();
        }

        public DataSet BankStatementExcelVersion2(DateTime? Fromdate, DateTime? Todate, int AccountId, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("BankStatementExcelVersion2", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@Account", AccountId);
            db_obj.AddParameter("@UserId", UserId);
            return db_obj.GetDataSet();
        }

        public DataSet PettyCashStatementVersion2(DateTime? Fromdate, DateTime? Todate, int AccountId, int page_number, int page_size, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("PettyCashStatementVersion2", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@Account", AccountId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@UserId", UserId);
            return db_obj.GetDataSet();
        }

        public DataSet PettyCashStatementExcelVersion2(DateTime? Fromdate, DateTime? Todate, int CashId, int UserId = 1)
        {
            Database_Operations db_obj = new Database_Operations("PettyCashStatementExcelVersion2", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@Account", CashId);
            db_obj.AddParameter("@UserId", UserId);
            return db_obj.GetDataSet();
        }

        public DataSet ProfitLossStatementExcel(DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("ProfitLossStatementExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet ProfitLossStatementPdfIqbal(DateTime? Fromdate, DateTime? Todate, int StatusId)
        {
            Database_Operations db_obj = new Database_Operations("ProfitLossStatementPdfIqbal", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@StatusId", StatusId);
            return db_obj.GetDataSet();
        }

        public DataSet ProfitLossStatementDetPdf(DateTime? Fromdate, DateTime? Todate, int StatusId)
        {
            Database_Operations db_obj = new Database_Operations("ProfitLossStatementPdfIqbalDet", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@StatusId", StatusId);
            return db_obj.GetDataSet();
        }

        public DataSet PLDetailed(DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("PLDetailedSummary", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet ProfitLossYearWise(int Year)
        {
            Database_Operations db_obj = new Database_Operations("PLYearWise", true);
            db_obj.AddParameter("@Year", Year);
            return db_obj.GetDataSet();
        }

        public DataSet DiscountShortlistExcel(DateTime? Fromdate, DateTime? Todate, int? CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("DiscountShortlistExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);

            return db_obj.GetDataSet();
        }

        public DataSet DiscountShortlist(DateTime? Fromdate, DateTime? Todate, int? CustomerId, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("DiscountShortlist", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);

            return db_obj.GetDataSet();
        }

        public DataSet CustomerSOA(DateTime Fromdate, DateTime Todate, int CustomerId, int PaymentStatus,
            int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("CustomerSOA", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@PaymentStatus", PaymentStatus);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerSOAExcel(DateTime Fromdate, DateTime Todate, int CustomerId, int PaymentStatus)
        {
            Database_Operations db_obj = new Database_Operations("CustomerSOAExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@PaymentStatus", PaymentStatus);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerSOAVersion2(DateTime? Fromdate, DateTime? Todate, int CustomerId, int PaymentStatus,
            int page_number, int page_size, int CompletionStatus)
        {
            Database_Operations db_obj = new Database_Operations("CustomerSOAVersion2", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@PaymentStatus", PaymentStatus);
            db_obj.AddParameter("@CompletionStatus", CompletionStatus);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerSOAExcelVersion2(DateTime? Fromdate, DateTime? Todate, int CustomerId, int PaymentStatus, int CompletionStatus)
        {
            Database_Operations db_obj = new Database_Operations("CustomerSOAExcelVersion2", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@PaymentStatus", PaymentStatus);
            db_obj.AddParameter("@CompletionStatus", CompletionStatus);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerSOAPrint(DateTime Fromdate, DateTime Todate, int CustomerId, int PaymentStatus, int CompletionStatus)
        {
            Database_Operations db_obj = new Database_Operations("CustomerSOAPrint", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@PaymentStatus", PaymentStatus);
            db_obj.AddParameter("@CompletionStatus", CompletionStatus);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerSOAPrintFormat2(DateTime? Fromdate, DateTime? Todate, int CustomerId, int PaymentStatus, int CompletionStatus)
        {
            Database_Operations db_obj = new Database_Operations("CustomerSOAPrintFormat2", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@PaymentStatus", PaymentStatus);
            db_obj.AddParameter("@CompletionStatus", CompletionStatus);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerSOAPrintFormat4(DateTime Fromdate, DateTime Todate, int CustomerId, int PaymentStatus, int CompletionStatus)
        {
            Database_Operations db_obj = new Database_Operations("CustomerSOAPrintFormat4", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@PaymentStatus", PaymentStatus);
            db_obj.AddParameter("@CompletionStatus", CompletionStatus);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerSOAPrintFormat5(DateTime Fromdate, DateTime Todate, int CustomerId, int PaymentStatus, int CompletionStatus)
        {
            Database_Operations db_obj = new Database_Operations("CustomerSOAPrintFormat5", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@PaymentStatus", PaymentStatus);
            db_obj.AddParameter("@CompletionStatus", CompletionStatus);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerSOAPrintFormat6(DateTime Fromdate, DateTime Todate, int CustomerId, int PaymentStatus, int CompletionStatus)
        {
            Database_Operations db_obj = new Database_Operations("CustomerSOAPrintFormat6", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@PaymentStatus", PaymentStatus);
            db_obj.AddParameter("@CompletionStatus", CompletionStatus);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerSOAPrintFormat8(DateTime? Fromdate, DateTime? Todate, int CustomerId, int PaymentStatus, int CompletionStatus)
        {
            Database_Operations db_obj = new Database_Operations("CustomerSOAPrintFormat7", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@PaymentStatus", PaymentStatus);
            db_obj.AddParameter("@CompletionStatus", CompletionStatus);
            return db_obj.GetDataSet();
        }

        public DataSet BalanceSheet(int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("BalanceSheet", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet BalanceSheetExcel()
        {
            Database_Operations db_obj = new Database_Operations("BalanceSheetExcel", true);
            return db_obj.GetDataSet();
        }

        public DataSet InvoiceReport(DateTime? Fromdate, DateTime? Todate, int page_number, int page_size, int? EmployeeId,
            int PaymentStatus, int? CustomerId,int? AgentId)
        {
            Database_Operations db_obj = new Database_Operations("InvoiceReport", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@EmployeeId", EmployeeId);
            db_obj.AddParameter("@PaymentStatus", PaymentStatus);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@AgentId", AgentId);
            return db_obj.GetDataSet();
        }

        public DataSet InvoiceReportExcel(DateTime? Fromdate, DateTime? Todate, int? EmployeeId, int PaymentStatus, int? CustomerId
            , int? AgentId)
        {
            Database_Operations db_obj = new Database_Operations("InvoiceReportExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@EmployeeId", EmployeeId);
            db_obj.AddParameter("@PaymentStatus", PaymentStatus);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@AgentId", AgentId);
            return db_obj.GetDataSet();
        }
        public DataSet InvoiceSummaryReport(DateTime? Fromdate, DateTime? Todate,int? AgentId)
        {
            Database_Operations db_obj = new Database_Operations("InvoiceSummaryReport", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@AgentId", AgentId);
            return db_obj.GetDataSet();
        }


        public DataSet VATStatement(DateTime? Fromdate, DateTime? Todate, int page_number, int page_size,int? EmirateId)
        {
            Database_Operations db_obj = new Database_Operations("VATStatement", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@EmirateId", EmirateId);

            return db_obj.GetDataSet();
        }

        public DataSet VATStatementExcel(DateTime? Fromdate, DateTime? Todate, int? EmirateId)
        {
            Database_Operations db_obj = new Database_Operations("VATStatementExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@EmirateId", EmirateId);

            return db_obj.GetDataSet();
        }

        public DataSet LoanStatement(DateTime? Fromdate, DateTime? Todate, int LoanId, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("LoanStatement", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@LoanId", LoanId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet LoanStatementExcel(DateTime? Fromdate, DateTime? Todate, int LoanId)
        {
            Database_Operations db_obj = new Database_Operations("LoanStatementExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@LoanId", LoanId);
            return db_obj.GetDataSet();
        }
        public DataTable GetCommissionApplicableCustomer()
        {
            Database_Operations db_obj = new Database_Operations("getCommissionApplicableCustomer", true);
            return (db_obj.GetDataTable());
        }
        public DataSet CustomerCommissionStatement(int CustomerId, DateTime? Fromdate, DateTime? Todate, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("CustomerCommissionStatement", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerCommissionStatementExcel(int CustomerId, DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("CustomerCommissionStatementExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            return db_obj.GetDataSet();
        }
        public DataSet CustomerCommissionStatementPdf(int CustomerId, DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("CustomerCommissionStatementPdf", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            return db_obj.GetDataSet();
        }

        public DataSet VendorOutstandingReport(int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("VendorOutstandingReport", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet VendorOutstandingExcel()
        {
            Database_Operations db_obj = new Database_Operations("VendorOutstandingExcel", true);
            return db_obj.GetDataSet();
        }

        public DataSet VendorStatement(DateTime Fromdate, DateTime Todate, int VendorId,
            int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("VendorStatement", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@VendorId", VendorId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet VendorStatementExcel(DateTime Fromdate, DateTime Todate, int VendorId)
        {
            Database_Operations db_obj = new Database_Operations("VendorStatementExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@VendorId", VendorId);
            return db_obj.GetDataSet();
        }

        public DataSet VendorStatementVersion2(DateTime? Fromdate, DateTime? Todate, int VendorId, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("VendorStatementVersion2", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@VendorId", VendorId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet VendorStatementVersion2Pdf(DateTime? Fromdate, DateTime? Todate, int VendorId)
        {
            Database_Operations db_obj = new Database_Operations("VendorStatementVersion2Pdf", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@VendorId", VendorId);
            return db_obj.GetDataSet();
        }
        public DataSet VendorStatementVersion2PdfFormat2(DateTime? Fromdate, DateTime? Todate, int VendorId)
        {
            Database_Operations db_obj = new Database_Operations("VendorStatementVersion2PdfFormat2", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@VendorId", VendorId);
            return db_obj.GetDataSet();
        }

        public DataSet VendorStatementExcelVersion2(DateTime? Fromdate, DateTime? Todate, int VendorId)
        {
            Database_Operations db_obj = new Database_Operations("VendorStatementExcelVersion2", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@VendorId", VendorId);
            return db_obj.GetDataSet();
        }

        public DataSet CreditorsReport(int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("CreditorsReport", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataTable CreditorsReportDetails(int CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("CreditorsReportDetails", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            return db_obj.GetDataTable();
        }

        public DataSet CreditorsReportExcel()
        {
            Database_Operations db_obj = new Database_Operations("CreditorsReportExcel", true);
            return db_obj.GetDataSet();
        }

        public DataSet CreditorsReportNewList(int page_number, int page_size, int? customerid, int? agentid)
        {
            Database_Operations db_obj = new Database_Operations("CreditorsReportNewList", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@agentid", agentid);
            db_obj.AddParameter("@customerid", customerid);
            return db_obj.GetDataSet();
        }

        public DataSet CreditorsReportNewExcel(int? customerid, int? agentid)
        {
            Database_Operations db_obj = new Database_Operations("CreditorsReportNewExcel", true);
            db_obj.AddParameter("@agentid", agentid);
            db_obj.AddParameter("@customerid", customerid);
            return db_obj.GetDataSet();
        }

        public DataSet Get_Attendence_Report(int? month, int? year, int? Employeeid, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("Get_Attendence_Report", true);
            db_obj.AddParameter("@month", month);
            db_obj.AddParameter("@year", year);
            db_obj.AddParameter("@Employeeid", Employeeid);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);

            return db_obj.GetDataSet();
        }

        public DataSet Get_Attendence_Excel(int? month, int? year, int? Employeeid)
        {
            Database_Operations db_obj = new Database_Operations("Get_Attendence_Excel", true);
            db_obj.AddParameter("@month", month);
            db_obj.AddParameter("@year", year);
            db_obj.AddParameter("@Employeeid", Employeeid);
            return db_obj.GetDataSet();
        }

        public DataSet Get_salary_Report(DateTime? Fromdate, DateTime? Todate, int? Employeeid, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("Get_salary_Report", true);
            db_obj.AddParameter("@Fromdate", Fromdate);
            db_obj.AddParameter("@Todate", Todate);
            db_obj.AddParameter("@Employeeid", Employeeid);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet Get_salary_Excel(DateTime? Fromdate, DateTime? Todate, int? Employeeid)
        {
            Database_Operations db_obj = new Database_Operations("Get_salary_Excel", true);
            db_obj.AddParameter("@Fromdate", Fromdate);
            db_obj.AddParameter("@Todate", Todate);
            db_obj.AddParameter("@Employeeid", Employeeid);
            return db_obj.GetDataSet();
        }

        public DataSet BalancesheetF2(DateTime ToDate)
        {
            Database_Operations db_obj = new Database_Operations("BalancesheetF2", true);
            db_obj.AddParameter("@ToDate", ToDate);
            return db_obj.GetDataSet();
        }


        public DataSet BalancesheetAM(DateTime ToDate)
        {
            Database_Operations db_obj = new Database_Operations("BalancesheetAM", true);
            db_obj.AddParameter("@ToDate", ToDate);
            return db_obj.GetDataSet();
        }

        public DataSet ServiceExpiryStatement(DateTime? Fromdate, DateTime? Todate, DataTable dtCustomer,
         DataTable dtService, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("ServiceExpiryReport", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@dtCustomer", dtCustomer);
            db_obj.AddParameter("@dtService", dtService);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet ServiceExpiryExcel(DateTime? Fromdate, DateTime? Todate, DataTable dtCustomer, DataTable dtService)
        {
            Database_Operations db_obj = new Database_Operations("ServiceExpiryExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@dtCustomer", dtCustomer);
            db_obj.AddParameter("@dtService", dtService);
            return db_obj.GetDataSet();
        }

        public DataSet AgentSOAStatement(int AgentId, DataTable dtCustomer, int page_number, int page_size, int StatusId,
            DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("AgentSOAStatement", true);
            db_obj.AddParameter("@AgentId", AgentId);
            db_obj.AddParameter("@dtCustomer", dtCustomer);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@StatusId", StatusId);
            db_obj.AddParameter("@Fromdate", Fromdate);
            db_obj.AddParameter("@Todate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet AgentSOAExcel(int AgentId, DataTable dtCustomer, int StatusId, DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("AgentSOAExcel", true);
            db_obj.AddParameter("@AgentId", AgentId);
            db_obj.AddParameter("@dtCustomer", dtCustomer);
            db_obj.AddParameter("@StatusId", StatusId);
            db_obj.AddParameter("@Fromdate", Fromdate);
            db_obj.AddParameter("@Todate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet AgentSOAPrint(int AgentId, DataTable dtCustomer, int StatusId, DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("AgentSOAPrint", true);
            db_obj.AddParameter("@AgentId", AgentId);
            db_obj.AddParameter("@dtCustomer", dtCustomer);
            db_obj.AddParameter("@StatusId", StatusId);
            db_obj.AddParameter("@Fromdate", Fromdate);
            db_obj.AddParameter("@Todate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet OutstandingTaxPayable(DateTime? Fromdate, DateTime? Todate, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("OutstandingTaxPayable", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet OutstandingTaxPayableExcel(DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("OutstandingTaxPayableExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            return db_obj.GetDataSet();
        }

        public DataTable GetCI_Report(DateTime? Fromdate, DateTime? Todate, DataTable dt_service, int? CustomerId,
          int page_number, int page_size, int invStatus,int? AgentId)
        {
            Database_Operations db_obj = new Database_Operations("GetCI_Report", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@dt_service", dt_service);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@invStatus", invStatus);
            db_obj.AddParameter("@AgentId", AgentId);
            return db_obj.GetDataTable();
        }

        public DataTable GetCI_Excel(DateTime? Fromdate, DateTime? Todate, DataTable dt_service, int? CustomerId, int invStatus, int? AgentId)
        {
            Database_Operations db_obj = new Database_Operations("GetCI_Excel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@dt_service", dt_service);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@invStatus", invStatus);
            db_obj.AddParameter("@AgentId", AgentId);
            return db_obj.GetDataTable();
        }

        public DataSet worksheet_Report(DateTime? Fromdate, DateTime? Todate, int E_id, int? D_id, int? Serv_id, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("worksheet_Report", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@E_id", E_id);
            db_obj.AddParameter("@D_id", D_id);
            db_obj.AddParameter("@Serv_id", Serv_id);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet worksheet_ReportExcel(DateTime? Fromdate, DateTime? Todate, int E_id, int? D_id, int? Serv_id)
        {
            Database_Operations db_obj = new Database_Operations("worksheet_ReportExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@E_id", E_id);
            db_obj.AddParameter("@Serv_id", Serv_id);
            db_obj.AddParameter("@D_id", D_id);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerInvoiceReportDetail(DateTime? Fromdate, DateTime? Todate, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("CustomerInvoiceReportDetail", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerInvoiceReportDetailExcel(DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("CustomerInvoiceReportDetailExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet EmployeeIncentiveStatment(int EmployeeId, DateTime? Fromdate, DateTime? Todate, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("EmployeeIncentiveStatment", true);
            db_obj.AddParameter("@EmployeeId", EmployeeId);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet EmployeeIncentiveStatmentExcel(int EmployeeId, DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("EmployeeIncentiveStatmentExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@EmployeeId", EmployeeId);
            return db_obj.GetDataSet();
        }

        public DataSet FinalVATReportPdf(int? year, int? FromMnth, int? ToMnth)
        {
            Database_Operations db_obj = new Database_Operations("FinalVATReportPdf", true);
            db_obj.AddParameter("@year", year);
            db_obj.AddParameter("@FromMnth", FromMnth);
            db_obj.AddParameter("@ToMnth", ToMnth);

            return db_obj.GetDataSet();
        }

        public DataSet CIDebitorsreport(int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("CIDebitorsreport", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet CIDebitorsexcel()
        {
            Database_Operations db_obj = new Database_Operations("CIDebitorsexcel", true);
            return db_obj.GetDataSet();
        }

        public DataSet CICustomerSOAlist(DateTime Fromdate, DateTime Todate, int CustomerId, int PaymentStatus,
           int page_number, int page_size, int CompletionStatus)
        {
            Database_Operations db_obj = new Database_Operations("CICustomerSOAlist", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@PaymentStatus", PaymentStatus);
            db_obj.AddParameter("@CompletionStatus", CompletionStatus);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet CICustomerSOAPrintFormat2(DateTime Fromdate, DateTime Todate, int CustomerId, int PaymentStatus, int CompletionStatus)
        {
            Database_Operations db_obj = new Database_Operations("CICustomerSOAPrintFormat2", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@PaymentStatus", PaymentStatus);
            db_obj.AddParameter("@CompletionStatus", CompletionStatus);
            return db_obj.GetDataSet();
        }

        public DataSet ApplicantSOAlist(DateTime? Fromdate, DateTime? Todate, int CustomerId, string Applicantname,
           int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("ApplicantSOAlist", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@Applicantname", Applicantname);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet ApplicantSOAPdf(DateTime? Fromdate, DateTime? Todate, int CustomerId, string Applicantname)
        {
            Database_Operations db_obj = new Database_Operations("ApplicantSOAPdf", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@Applicantname", Applicantname);
            return db_obj.GetDataSet();
        }

        public DataSet DeadlineList(DateTime? Fromdate, DateTime? Todate, int page_number, int page_size, int? C_id)
        {
            Database_Operations db_obj = new Database_Operations("DeadlineList", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@C_id", C_id);
            return db_obj.GetDataSet();
        }

        public DataSet DeadlineExcel(DateTime? Fromdate, DateTime? Todate, int? C_id, int? AgentId, int? ServiceId)
        {
            Database_Operations db_obj = new Database_Operations("DeadlineExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@C_id", C_id);
            db_obj.AddParameter("@AgentId", AgentId);
            db_obj.AddParameter("@ServiceId", ServiceId);
            return db_obj.GetDataSet();
        }

        public DataSet FollowupList(DateTime? Fromdate, DateTime? Todate, int? CustomerId, int? AgentId, int? ServiceId)
        {
            Database_Operations db_obj = new Database_Operations("FollowupList", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@AgentId", AgentId);
            db_obj.AddParameter("@ServiceId", ServiceId);
            return db_obj.GetDataSet();
        }

        public DataSet VisaArrivalList(DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("VisaArrivalList", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet EmployeeInvoiceList(DateTime? Fromdate, DateTime? Todate, int EmployeeId, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("EmployeeInvoiceList", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@EmployeeId", EmployeeId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet EmployeeInvoiceExcel(DateTime? Fromdate, DateTime? Todate, int EmployeeId)
        {
            Database_Operations db_obj = new Database_Operations("EmployeeInvoiceExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@EmployeeId", EmployeeId);
            return db_obj.GetDataSet();
        }

        public DataSet QuotationInvoiceList(DateTime? Fromdate, DateTime? Todate, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("QuotationInvoiceReport", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet QuotationInvoiceExcel(DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("QuotationInvoiceExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet StaffPaymentoutList(int? CompanyId, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("StaffPaymentoutList", true);
            db_obj.AddParameter("@CompanyId", CompanyId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet StaffPaymentoutPdf(int? CompanyId)
        {
            Database_Operations db_obj = new Database_Operations("StaffPaymentoutPdf", true);
            db_obj.AddParameter("@CompanyId", CompanyId);
            return db_obj.GetDataSet();
        }

        public DataSet AgentProfitStatement(DateTime? Fromdate, DateTime? Todate, int AgentId, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("AgentProfitStatement", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@AgentId", AgentId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet AgentProfitStatementExcel(DateTime? Fromdate, DateTime? Todate, int AgentId)
        {
            Database_Operations db_obj = new Database_Operations("AgentProfitStatementExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@AgentId", AgentId);
            return db_obj.GetDataSet();
        }

        public DataSet PendingserviceList(DateTime? Fromdate, DateTime? Todate, DataTable dtService,
    DataTable dt_cust, int? DepartmentId, int? AgentId, int page_number, int page_size, int? ServiceStatusId,string Applicant)
        {
            Database_Operations db_obj = new Database_Operations("PendingserviceList", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@dt_cust", dt_cust);
            db_obj.AddParameter("@DepartmentId", DepartmentId);
            db_obj.AddParameter("@dtService", dtService);
            db_obj.AddParameter("@AgentId", AgentId);
            db_obj.AddParameter("@ServiceStatusId", ServiceStatusId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@Applicant", Applicant);

            return db_obj.GetDataSet();
        }

        public DataSet PendingserviceExcelPdf(DateTime? Fromdate, DateTime? Todate, DataTable dtService,
        DataTable dt_cust, int? DepartmentId, int? AgentId, int? ServiceStatusId, string Applicant)
        {
            Database_Operations db_obj = new Database_Operations("PendingserviceExcelPdf", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@dt_cust", dt_cust);
            db_obj.AddParameter("@DepartmentId", DepartmentId);
            db_obj.AddParameter("@dtService", dtService);
            db_obj.AddParameter("@AgentId", AgentId);
            db_obj.AddParameter("@ServiceStatusId", ServiceStatusId);
            db_obj.AddParameter("@Applicant", Applicant);

            return db_obj.GetDataSet();
        }

        public DataSet VendorCommissionlist(DateTime? Fromdate, DateTime? Todate, int VendorId, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("VendorCommissionlist", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@VendorId", VendorId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }


        public DataSet VendorCommissionpdf(DateTime? Fromdate, DateTime? Todate, int VendorId)
        {
            Database_Operations db_obj = new Database_Operations("VendorCommissionpdf", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@VendorId", VendorId);
            return db_obj.GetDataSet();
        }

        public DataSet DayBookdetail(DateTime? FromDate, DateTime? ToDate, int? UserId)
        {
            Database_Operations db_obj = new Database_Operations("DayBookdetail", true);
            db_obj.AddParameter("@FromDate", FromDate);
            db_obj.AddParameter("@ToDate", ToDate);
            db_obj.AddParameter("@UserId", UserId);
            return db_obj.GetDataSet();
        }

        public DataSet CreditProfitStatement(DateTime? Fromdate, DateTime? Todate, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("CreditProfitStatement", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet CreditProfitStatementExcel(DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("CreditProfitStatementExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet GetReceivableReport(int page_number, int page_size, string filter, int Ctype)
        {
            Database_Operations db_obj = new Database_Operations("GetReceivableReport", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@filter", filter);
            db_obj.AddParameter("@Ctype", Ctype);
            return db_obj.GetDataSet();
        }
        public DataSet GetReceivableExcel(string filter, int Ctype)
        {
            Database_Operations db_obj = new Database_Operations("GetReceivableExcel", true);
            db_obj.AddParameter("@filter", filter);
            db_obj.AddParameter("@Ctype", Ctype);
            return db_obj.GetDataSet();
        }

        public DataSet Supplier_Report(DateTime? Fromdate, DateTime? Todate, int? SupplierId, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("Supplier_Report", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@SupplierId", SupplierId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet Get_Supplier_Excel(DateTime? Fromdate, DateTime? Todate, int? SupplierId)
        {
            Database_Operations db_obj = new Database_Operations("Get_Supplier_Excel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@SupplierId", SupplierId);

            return db_obj.GetDataSet();
        }
        public DataTable Drp_Supplier()
        {
            Database_Operations db_obj = new Database_Operations("GetSupplier", true);
            return (db_obj.GetDataTable());
        }

        public DataSet WorkSummaryPdf(DateTime? FromDate, DateTime? ToDate, DataTable dtEmployee)
        {
            Database_Operations db_obj = new Database_Operations("WorkSummaryPdf", true);
            db_obj.AddParameter("@FromDate", FromDate);
            db_obj.AddParameter("@ToDate", ToDate);
            db_obj.AddParameter("@dtEmployee", dtEmployee);
            return db_obj.GetDataSet();
        }

        public DataSet Party_Report(DateTime? Fromdate, DateTime? Todate, int? PartyId, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("Party_Report", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@PartyId", PartyId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet Get_Party_Excel(DateTime? Fromdate, DateTime? Todate, int? PartyId)
        {
            Database_Operations db_obj = new Database_Operations("Get_Party_Excel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@PartyId", PartyId);

            return db_obj.GetDataSet();
        }

        public DataSet GetYearlyProfit(DataTable YearTbl)
        {
            Database_Operations db_obj = new Database_Operations("GetYearlyProfit", true);
            db_obj.AddParameter("@YearTbl", YearTbl);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerBalanceList(DateTime? Fromdate, DateTime? Todate, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("CustomerBalance", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerBalancePdfExcel(DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("CustomerBalancePdfExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerBalanceDetail_Pdf(DateTime? Fromdate, DateTime? Todate, int CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("CustomerBalanceDetail_Pdf", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CustomerId", CustomerId);

            return db_obj.GetDataSet();
        }

        public DataSet Debitors_ReportDateWise(int page_number, int page_size, string filter, int Ctype, DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("Debitors_ReportDateWise", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@filter", filter);
            db_obj.AddParameter("@Ctype", Ctype);
            db_obj.AddParameter("@Fromdate", Fromdate);
            db_obj.AddParameter("@Todate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet Debitors_ReportDateWisePdfExcel(int Ctype, DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("Debitors_ReportDateWisePdfExcel", true);
            db_obj.AddParameter("@Ctype", Ctype);
            db_obj.AddParameter("@Fromdate", Fromdate);
            db_obj.AddParameter("@Todate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet Debitors_ReportFromat2DateWise(int page_number, int page_size, string filter, int Ctype, DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("Debitors_ReportFromat2DateWise", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@filter", filter);
            db_obj.AddParameter("@Ctype", Ctype);
            db_obj.AddParameter("@Fromdate", Fromdate);
            db_obj.AddParameter("@Todate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet Debitors_ReportFromat2DateWisePdfExcel(int Ctype, DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("Debitors_ReportFromat2DateWisePdfExcel", true);
            db_obj.AddParameter("@Ctype", Ctype);
            db_obj.AddParameter("@Fromdate", Fromdate);
            db_obj.AddParameter("@Todate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet DebitorsReportCustomerDateWise(int CustomerId, DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("DebitorsReportCustomerDateWise", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@Fromdate", Fromdate);
            db_obj.AddParameter("@Todate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet debitorsReportFromat2CustomerDateWise(int CustomerId, DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("debitorsReportFromat2CustomerDateWise", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@Fromdate", Fromdate);
            db_obj.AddParameter("@Todate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet serviceDiscountList(DateTime? Fromdate, DateTime? Todate, DataTable dtService,
DataTable dt_cust, int? AgentId, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("serviceDiscountList", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@dt_cust", dt_cust);
            db_obj.AddParameter("@dtService", dtService);
            db_obj.AddParameter("@AgentId", AgentId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet serviceDiscountExcelPdf(DateTime? Fromdate, DateTime? Todate, DataTable dtService,
        DataTable dt_cust, int? AgentId)
        {
            Database_Operations db_obj = new Database_Operations("serviceDiscountExcelPdf", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@dt_cust", dt_cust);
            db_obj.AddParameter("@dtService", dtService);
            db_obj.AddParameter("@AgentId", AgentId);
            return db_obj.GetDataSet();
        }

        public DataSet VATReportF2List(DateTime? Fromdate, DateTime? Todate, int page_number, int page_size,int? EmirateId)
        {
            Database_Operations db_obj = new Database_Operations("VATReportF2List", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@EmirateId", EmirateId);
            return db_obj.GetDataSet();
        }

        public DataSet VATReportF2Excel(DateTime? Fromdate, DateTime? Todate, int? EmirateId)
        {
            Database_Operations db_obj = new Database_Operations("VATReportF2Excel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@EmirateId", EmirateId);
            return db_obj.GetDataSet();
        }

        public DataSet Get_debitors_ReportSOAF8(int page_number, int page_size, string filter, int Ctype)
        {
            Database_Operations db_obj = new Database_Operations("Get_debitors_ReportSOAF8", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@filter", filter);
            db_obj.AddParameter("@Ctype", Ctype);
            return db_obj.GetDataSet();
        }

        public DataSet Get_debitors_ExcelSOAF8(int Ctype)
        {
            Database_Operations db_obj = new Database_Operations("Get_debitors_ExcelSOAF8", true);
            db_obj.AddParameter("@Ctype", Ctype);
            return db_obj.GetDataSet();
        }

        public DataSet OBExcelPdf()
        {
            Database_Operations db_obj = new Database_Operations("OBExcelPdf", true);
            return db_obj.GetDataSet();
        }
        public DataSet AgentCommissionStatement(int AgentId, DateTime? Fromdate, DateTime? Todate, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("AgentCommissionStatement", true);
            db_obj.AddParameter("@AgentId", AgentId);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet AgentCommissionStatementExcel(int AgentId, DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("AgentCommissionStatementExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@AgentId", AgentId);
            return db_obj.GetDataSet();
        }

        public DataSet AgentCommissionStatementPdf(int AgentId, DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("AgentCommissionStatementPdf", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@AgentId", AgentId);
            return db_obj.GetDataSet();
        }

        public DataSet AgentCommissionOutstandingList(int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("AgentCommissionOutstandingList", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet AgentCommissionOutstandingPdfExcel()
        {
            Database_Operations db_obj = new Database_Operations("AgentCommissionOutstandingPdfExcel", true);
            return db_obj.GetDataSet();
        }

        public DataSet TrialBalance(  DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("TrialBalance", true);
            db_obj.AddParameter("@ToDate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet LeaveReport(DateTime? Fromdate, DateTime? Todate, int page_number, int page_size, int? EmployeeId
            )
        {
            Database_Operations db_obj = new Database_Operations("Get_LeaveEntryReport", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@E_id", EmployeeId);

            return db_obj.GetDataSet();
        }
        public DataSet LeaveReportExcel(DateTime? Fromdate, DateTime? Todate, int? EmployeeId
                    )
        {
            Database_Operations db_obj = new Database_Operations("Get_LeaveEntryReportExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@E_id", EmployeeId);

            return db_obj.GetDataSet();
        }

        public DataSet AccountSummary(int page_number, int page_size,DateTime? Fromdate,DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("AccountSummary", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            db_obj.AddParameter("@Fromdate", Fromdate);
            db_obj.AddParameter("@Todate", Todate);
            return db_obj.GetDataSet();
        }
        public DataSet AccountSummaryExcel( DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("AccountSummaryExcel", true);
            db_obj.AddParameter("@Fromdate", Fromdate);
            db_obj.AddParameter("@Todate", Todate);
            return db_obj.GetDataSet();
        }

        public DataSet CompanyGroupSOAList(DateTime? Fromdate, DateTime? Todate, int CompanyGroupId, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("CompanyGroupSOAList", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CompanyGroupId", CompanyGroupId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@page_size", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet CompanyGroupSOAExcel(DateTime? Fromdate, DateTime? Todate, int CompanyGroupId)
        {
            Database_Operations db_obj = new Database_Operations("CompanyGroupSOAExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CompanyGroupId", CompanyGroupId);
            return db_obj.GetDataSet();
        }
        public DataSet CompanyGroupSOAPdf(DateTime? Fromdate, DateTime? Todate, int CompanyGroupId)
        {
            Database_Operations db_obj = new Database_Operations("CompanyGroupSOAPdf", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@CompanyGroupId", CompanyGroupId);
            return db_obj.GetDataSet();
        }

        #endregion
    }
}