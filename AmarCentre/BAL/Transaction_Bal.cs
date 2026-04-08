using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace AmarCentre.BAL
{
    public class Transaction_Bal
    {
        #region Common

        public DataTable DrpVendorCustomer()
        {
            Database_Operations db_obj = new Database_Operations("DrpVendorCustomer", true);
            return db_obj.GetDataTable();
        }

        public DataSet drpforLead() //1-employee,2-remark,3-QuotationMailTemplate,4-GeneralSettings
        {
            Database_Operations databaseOperations = new Database_Operations("drpforLead", true);
            return (databaseOperations.GetDataSet());
        }

        public DataTable DrpEmployeeTrans(int TransId = 1)
        {
            Database_Operations db_obj = new Database_Operations("DrpEmployeeTrans", true);
            db_obj.AddParameter("@TransId", TransId);
            return db_obj.GetDataTable();
        }

        public DataSet getDetailForMail(int id, int PageId)
        {
            Database_Operations db_obj = new Database_Operations("getDetailForMail", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@PageId", PageId);
            return (db_obj.GetDataSet());
        }

        public DataSet getCustomerMail(int Id,int pageId)  //1-Quotation  2-invoice , 3- receipt, 4-receiptvoucher ,
                                                           ////5 -docexpiry, 6-sc&soa , 7-customerlogin detail
        {
            Database_Operations db_obj = new Database_Operations("getCustomerMail", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@pageId", pageId);
            return db_obj.GetDataSet();
        }

        public DataTable getCustomerCCMail(int CustomerId)   
        {
            Database_Operations db_obj = new Database_Operations("getCustomerCCMail", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            return db_obj.GetDataTable();
        }

        public DataSet DrpForSC() //1-expense,2-vendr,3-paymode,4-employee
        {
            Database_Operations databaseOperations = new Database_Operations("DrpForSC", true);
            return (databaseOperations.GetDataSet());
        }

        public DataSet DrpForInvoice() //1-customer,2-agent,3-template,4-generl
        {
            Database_Operations databaseOperations = new Database_Operations("DrpForInvoice", true);
            return (databaseOperations.GetDataSet());
        }

        public DataTable DrpMailTemplate()
        {
            Database_Operations databaseOperations = new Database_Operations("DrpMailTemplate", true);
            return (databaseOperations.GetDataTable());
        }

        public DataTable DrpMailReceiver(int dttype)
        {
            Database_Operations databaseOperations = new Database_Operations("DrpMailReceiver", true);
            databaseOperations.AddParameter("@dttype", dttype);
            return (databaseOperations.GetDataTable());
        }

        public DataTable Drp_Customer()
        {
            Database_Operations db_obj = new Database_Operations("List_Customer_Drp", true);
            return db_obj.GetDataTable();
        }

        public DataTable Drp_Customer_FAgent(int agentId)
        {
            Database_Operations db_obj = new Database_Operations("Drp_Customer_FAgent", true);
            db_obj.AddParameter("@agentId", agentId);
            return db_obj.GetDataTable();
        }

        public DataTable Drp_CustomerWithMobileNo()
        {
            Database_Operations db_obj = new Database_Operations("List_CustomerWithMobileNo_Drp", true);
            return db_obj.GetDataTable();
        }

        public DataTable GetTemplates()
        {
            Database_Operations db_obj = new Database_Operations("GetTemplates", true);
            return db_obj.GetDataTable();
        }
        //public DataTable Drp_Customer()
        //{
        //    Database_Operations db_obj = new Database_Operations("drp_Customer", true);
        //    return (db_obj.GetDataTable());
        //}

        public DataTable Get_Services_Amount(int SerId, int CusId, int Language, int SerPriceWithTax, int InvoiceType, int AgId)
        {
            Database_Operations db_obj = new Database_Operations("Get_Services_Amount", true);
            db_obj.AddParameter("@SerId", SerId);
            db_obj.AddParameter("@CusId", CusId);
            db_obj.AddParameter("@Language", Language);
            db_obj.AddParameter("@SerPriceWithTax", SerPriceWithTax);
            db_obj.AddParameter("@InvoiceType", InvoiceType);
            db_obj.AddParameter("@AgId", AgId);
            return (db_obj.GetDataTable());
        }

        public DataTable Drp_Service(int Cat_id)
        {
            Database_Operations db_obj = new Database_Operations("drp_service_cat", true);
            db_obj.AddParameter("@Cat_id", Cat_id);
            return (db_obj.GetDataTable());
        }

        public DataTable Drp_Service_catrgy()
        {
            Database_Operations db_obj = new Database_Operations("Drp_Service_catrgy", true);
            return db_obj.GetDataTable();
        }

        public DataTable Drp_PaymentMode()
        {
            Database_Operations db_obj = new Database_Operations("List_PaymentMode_Drp", true);
            return db_obj.GetDataTable();
        }

        public DataTable Drp_Account_Filter_PayMode(int PayModeId)
        {
            Database_Operations db_obj = new Database_Operations("List_Account_Filter_PayMode_Drp", true);
            db_obj.AddParameter("@PayModeId", PayModeId);
            return db_obj.GetDataTable();
        }

        public DataTable GetServiceDetailsTemplate(DataTable dtTemplates, int Language, int SerPriceWithTax, int InvoiceType, int CustomerId, int AgId)
        {
            Database_Operations db_obj = new Database_Operations("GetServiceDetailsTemplate", true);
            db_obj.AddParameter("@dtTemplates", dtTemplates);
            db_obj.AddParameter("@Language", Language);
            db_obj.AddParameter("@SerPriceWithTax", SerPriceWithTax);
            db_obj.AddParameter("@InvoiceType", InvoiceType);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@AgId", AgId);
            return (db_obj.GetDataTable());
        }
        #endregion

        #region Assign Token

        //Get List of Data
        public DataTable Get_List_Assign_Token(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Assign_Token", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_Assign_Token(int Id, int CustomerId, string TokenNumber, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Assign_Token", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@TokenNumber", TokenNumber);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int Delete_Assign_Token(int Id, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Assign_Token", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_List_Assign_Token_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_Assign_Token_Excel", true);
            return db_obj.GetDataTable();
        }

        #endregion

        #region Invoice

        public int GetInvoiceFormat()
        {
            Database_Operations db_obj = new Database_Operations("GetInvoiceFormat", true);
            db_obj.AddOutputParameter("@Format");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Format"].Value.ToString());
        }

        public DataTable GetEmployeeLanguage(int UserId)
        {
            Database_Operations db_obj = new Database_Operations("GetEmployeeLanguage", true);
            db_obj.AddParameter("@UserId", UserId);
            return (db_obj.GetDataTable());
        }

        public DataTable GetServicePriceWithTax()
        {
            Database_Operations db_obj = new Database_Operations("GetServicePriceWithTax", true);
            return (db_obj.GetDataTable());
        }

        public DataSet GetServiceFilter(int filterby, int Department, int SerCategory, int SerSubCategory, int Language)
        {
            Database_Operations db_obj = new Database_Operations("GetServiceFilter", true);
            db_obj.AddParameter("@filterby", filterby);
            db_obj.AddParameter("@Department", Department);
            db_obj.AddParameter("@SerCategory", SerCategory);
            db_obj.AddParameter("@SerSubCategory", SerSubCategory);
            db_obj.AddParameter("@Language", Language);
            return (db_obj.GetDataSet());
        }

        public DataTable Get_Customerdetail(string token)
        {
            Database_Operations db_obj = new Database_Operations("Get_Customerdetail", true);
            db_obj.AddParameter("@token", token);
            return (db_obj.GetDataTable());
        }

        public DataSet Get_CustomerCreditDetail(int CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("Get_CustomerCreditDetail", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            return (db_obj.GetDataSet());
        }

        /*Get Detail in Excel*/
        public DataTable Get_List_invoice_Excel(int userid)
        {
            Database_Operations db_obj = new Database_Operations("List_Invoice_Excel", true);
            db_obj.AddParameter("@userid", userid);
            return db_obj.GetDataTable();
        }
        public DataTable ListInvoiceCustm( int page_size, int userid,int? InvoiceStatus)
        {
            Database_Operations db_obj = new Database_Operations("ListInvoiceCustm", true);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@userid", userid);
            db_obj.AddParameter("@InvoiceStatus", InvoiceStatus);
            return db_obj.GetDataTable();
        }
        public DataTable List_Invoice(int page_number, int page_size, string filter,int userid, int? InvoiceStatus)
        {
            Database_Operations db_obj = new Database_Operations("List_Invoice", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@userid", userid);
            db_obj.AddParameter("@InvoiceStatus", InvoiceStatus);
            return db_obj.GetDataTable();
        }

        public DataSet Edit_Invoice(int id, int Language, int SerPriceWithTax)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Invoice", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Language", Language);
            db_obj.AddParameter("@SerPriceWithTax", SerPriceWithTax);
            return (db_obj.GetDataSet());
        }

        public int Insert_SplitInvoice(int Id, DateTime Qdate, int Cust_id, string remark, int UserId,
        DataTable dt_serv, int? Quot_id, int InvoiceType, int TaxAppliedWithDiscount, int paytype,
         decimal bankcharge, decimal chargedamt, int? AgentId, int InvoiceFormat)
        {
            Database_Operations db_obj = new Database_Operations("Insert_SplitInvoice", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Qdate", Qdate);
            db_obj.AddParameter("@Cust_id", Cust_id);
            db_obj.AddParameter("@remark", remark);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddParameter("@dt_serv", dt_serv);
            db_obj.AddParameter("@Quot_id", Quot_id);
            db_obj.AddParameter("@InvoiceType", InvoiceType);
            db_obj.AddParameter("@TaxAppliedWithDiscount", TaxAppliedWithDiscount);
            db_obj.AddParameter("@paytype", paytype);
            db_obj.AddParameter("@bankcharge", bankcharge);
            db_obj.AddParameter("@chargedamt", chargedamt);
            db_obj.AddParameter("@AgentId", AgentId);
            db_obj.AddParameter("@InvoiceFormat", InvoiceFormat);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

    
        public int Insert_Update_InvoiceWitDisc(int Id, DateTime? Qdate, int Cust_id, string remark, int UserId, decimal? TotGrand,
         DataTable dt_serv, int? Quot_id, decimal Dicounttot, int InvoiceType, int TaxAppliedWithDiscount, int paytype, 
         decimal bankcharge, decimal chargedamt,int? AgentId,int InvoiceFormat,decimal RoundedOff,string subject,string BillingName,
         int? InvoiceCreater)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_InvoiceWitDisc", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@InvoiceCreater", InvoiceCreater);
            db_obj.AddParameter("@Qdate", Qdate);
            db_obj.AddParameter("@Cust_id", Cust_id);
            db_obj.AddParameter("@TotGrand", TotGrand);
            db_obj.AddParameter("@remark", remark);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddParameter("@dt_serv", dt_serv);
            db_obj.AddParameter("@Quot_id", Quot_id);
            db_obj.AddParameter("@Disounttot", Dicounttot);
            db_obj.AddParameter("@InvoiceType", InvoiceType);
            db_obj.AddParameter("@TaxAppliedWithDiscount", TaxAppliedWithDiscount);
            db_obj.AddParameter("@paytype", paytype);
            db_obj.AddParameter("@bankcharge", bankcharge);
            db_obj.AddParameter("@chargedamt", chargedamt);
            db_obj.AddParameter("@AgentId", AgentId);
            db_obj.AddParameter("@InvoiceFormat", InvoiceFormat);
            db_obj.AddParameter("@RoundedOff", RoundedOff);
            db_obj.AddParameter("@subject", subject);
            db_obj.AddParameter("@BillingName", BillingName);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

   
        public int Insert_Update_InvoiceWitDiscSC(int Id, DateTime? Qdate, int Cust_id, string remark, int UserId, decimal? TotGrand,
         DataTable dt_serv, int? Quot_id, decimal Dicounttot, int InvoiceType, int TaxAppliedWithDiscount, DataTable dtexpense, DataTable dtTrans
            , int paytype, decimal bankcharge, decimal chargedamt,int? AgentId,int InvoiceFormat, decimal RoundedOff,string subject,
          string BillingName,int? InvoiceCreater)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_InvoiceWitDiscSC", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@InvoiceCreater", InvoiceCreater);
            db_obj.AddParameter("@Qdate", Qdate);
            db_obj.AddParameter("@Cust_id", Cust_id);
            db_obj.AddParameter("@TotGrand", TotGrand);
            db_obj.AddParameter("@remark", remark);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddParameter("@dt_serv", dt_serv);
            db_obj.AddParameter("@Quot_id", Quot_id);
            db_obj.AddParameter("@Disounttot", Dicounttot);
            db_obj.AddParameter("@InvoiceType", InvoiceType);
            db_obj.AddParameter("@TaxAppliedWithDiscount", TaxAppliedWithDiscount);
            db_obj.AddParameter("@dtexpense", dtexpense);
            db_obj.AddParameter("@dtTrans", dtTrans);
            db_obj.AddParameter("@paytype", paytype);
            db_obj.AddParameter("@bankcharge", bankcharge);
            db_obj.AddParameter("@chargedamt", chargedamt);
            db_obj.AddParameter("@AgentId", AgentId);
            db_obj.AddParameter("@InvoiceFormat", InvoiceFormat);
            db_obj.AddParameter("@RoundedOff", RoundedOff);
            db_obj.AddParameter("@subject", subject);
            db_obj.AddParameter("@BillingName", BillingName);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }


        public int Cancel_Invoice(int incmid, string reasn, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Cancel_Invoice", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddParameter("@reasn", reasn);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable GetInvoiceCancelDetail(int inv_id)
        {
            Database_Operations db_obj = new Database_Operations("GetInvoiceCancelDetail", true);
            db_obj.AddParameter("@inv_id", inv_id);
            return (db_obj.GetDataTable());
        }
        public DataTable getVendrBMdet(int detid)
        {
            Database_Operations db_obj = new Database_Operations("getVendrBMdet", true);
            db_obj.AddParameter("@detid", detid);
            return (db_obj.GetDataTable());
        }

        public int CancelsingleVBMentry(int Id, int CreatedBy)
        {
            Database_Operations db_obj = new Database_Operations("CancelsingleVBMentry", true);
            db_obj.AddParameter("@detid", Id);
            db_obj.AddParameter("@UserId", CreatedBy);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }
        public DataTable get_receiptvoucherdet(int detid)
        {
            Database_Operations db_obj = new Database_Operations("get_receiptvoucherdet", true);
            db_obj.AddParameter("@detid", detid);
            return (db_obj.GetDataTable());
        }
        
        public int CancelsingleReceiptVoucherentry(int Id, int CreatedBy)
        {
            Database_Operations db_obj = new Database_Operations("CancelsingleReceiptVoucherentry", true);
            db_obj.AddParameter("@detid", Id);
            db_obj.AddParameter("@UserId", CreatedBy);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Drp_Quotation(int CusId)
        {
            Database_Operations db_obj = new Database_Operations("drp_Quotation", true);
            db_obj.AddParameter("@CusId", CusId);
            return (db_obj.GetDataTable());
        }

        public DataTable Drp_Quotation_Edit(int CusId, int inv_id)
        {
            Database_Operations db_obj = new Database_Operations("drp_Quotation_Edit", true);
            db_obj.AddParameter("@CusId", CusId);
            db_obj.AddParameter("@inv_id", inv_id);
            return (db_obj.GetDataTable());
        }

        public DataTable GetQuotationDetails(int QuotationId, int Language, int SerPriceWithTax, int InvoiceType, int AgId)
        {
            Database_Operations db_obj = new Database_Operations("GetQuotationDetails", true);
            db_obj.AddParameter("@QuotationId", QuotationId);
            db_obj.AddParameter("@Language", Language);
            db_obj.AddParameter("@SerPriceWithTax", SerPriceWithTax);
            db_obj.AddParameter("@InvoiceType", InvoiceType);
            db_obj.AddParameter("@AgId", AgId);
            return (db_obj.GetDataTable());
        }
        //public DataSet Edit_Quotation_invoice(int id)
        //{
        //    Database_Operations db_obj = new Database_Operations("Edit_Quotan_invoice", true);
        //    db_obj.AddParameter("@Id", id);
        //    return (db_obj.GetDataSet());
        //}

        public DataSet list_InvHistry(DateTime? from, DateTime? to, int Qid, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("List_InvHistory", true);
            db_obj.AddParameter("@From", from);
            db_obj.AddParameter("@To", to);
            db_obj.AddParameter("@Qid", Qid);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            return db_obj.GetDataSet();
        }

        public DataSet Edit_Invoice_SC(int id, int invd_id)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Invoice_SC", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@invd_id", invd_id);
            return (db_obj.GetDataSet());
        }

        public DataTable ServiceAmtForSingleQty(int ServiceId)
        {
            Database_Operations db_obj = new Database_Operations("ServiceAmtForSingleQty", true);
            db_obj.AddParameter("@ServiceId", ServiceId);
            return (db_obj.GetDataTable());
        }

        public DataSet Get_SerExpenseDetail_SC_byService(int ServiceId)
        {
            Database_Operations db_obj = new Database_Operations("Get_SerExpenseDetail_SC_byService", true);
            db_obj.AddParameter("@ServiceId", ServiceId);
            return (db_obj.GetDataSet());
        }

        public DataSet Get_InvDetail_ServiceCompletionINVSC(int InvoiceDetailId, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Get_InvDetail_ServiceCompletionINVSC", true);
            db_obj.AddParameter("@InvoiceDetailId", InvoiceDetailId);
            db_obj.AddParameter("@UserId", UserId);
            return (db_obj.GetDataSet());
        }

        public int CustomerCancelsingleReceiptVoucherentry(int Id, int CreatedBy)
        {
            Database_Operations db_obj = new Database_Operations("CustomerCancelsingleReceiptVoucherentry", true);
            db_obj.AddParameter("@detid", Id);
            db_obj.AddParameter("@UserId", CreatedBy);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region Receipt

        public DataTable DrpPendingInvoice(int CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("DrpPendingInvoice", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            return db_obj.GetDataTable();
        }   

        public DataTable List_Receipt(int page_number, int page_size, string filter, string column, string order, int userid,int StatusId)
        {
            Database_Operations db_obj = new Database_Operations("List_Receipt", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            db_obj.AddParameter("@userid", userid);
            db_obj.AddParameter("@StatusId", StatusId);
            return db_obj.GetDataTable();
        }

        public DataTable Get_List_Receipt_Excel(int userid)
        {
            Database_Operations db_obj = new Database_Operations("List_Receipt_Excel", true);
            db_obj.AddParameter("@userid", userid);
            return db_obj.GetDataTable();
        }

        public DataSet Edit_Receipt(int Id, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Receipt", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@UserId", UserId);
            return (db_obj.GetDataSet());
        }

        public DataSet Get_Invoice(string InvoiceCode, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Get_Invoice", true);
            db_obj.AddParameter("@InvoiceCode", InvoiceCode);
            db_obj.AddParameter("@UserId", UserId);
            return (db_obj.GetDataSet());
        }

        public int Insert_Update_Receipt(int Id, DateTime? Rdate, int InvoiceId, string Remark, decimal? TotalDiscount, decimal GrandTotAmt,
             decimal AmtPayNow, int PayModeId, int? AccountId, int? PettyCashId, DateTime? ChequeDate, string ChequeNumber, decimal PendingAmount,
            decimal ReceivedAmount, decimal Balance, DataTable dt_serv, int UserId, decimal? BankCommsn,decimal ChargedAmount,
            decimal? ChargedAmountReceipt,int? LoanId,decimal SpotCommission, decimal CommissionVat)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Receipt", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Rdate", Rdate);
            db_obj.AddParameter("@InvoiceId", InvoiceId);
            db_obj.AddParameter("@Remark", Remark);
            db_obj.AddParameter("@ChargedAmountReceipt", ChargedAmountReceipt);
            db_obj.AddParameter("@ChargedAmount", ChargedAmount);
            db_obj.AddParameter("@TotalDiscount", TotalDiscount);
            db_obj.AddParameter("@GrandTotAmt", GrandTotAmt);
            db_obj.AddParameter("@AmtPayNow", AmtPayNow);
            db_obj.AddParameter("@PayModeId", PayModeId);
            db_obj.AddParameter("@AccountId", AccountId);
            db_obj.AddParameter("@PettyCashId", PettyCashId);
            db_obj.AddParameter("@ChequeDate", ChequeDate);
            db_obj.AddParameter("@ChequeNumber", ChequeNumber);
            db_obj.AddParameter("@PendingAmount", PendingAmount);
            db_obj.AddParameter("@ReceivedAmount", ReceivedAmount);
            db_obj.AddParameter("@BankCommsn", BankCommsn);
            db_obj.AddParameter("@Balance", Balance);
            db_obj.AddParameter("@LoanId", LoanId);
            db_obj.AddParameter("@dt_serv", dt_serv);
            db_obj.AddParameter("@SpotCommission", SpotCommission);
            db_obj.AddParameter("@CommissionVat", CommissionVat);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable GetReceiptCancelDetail(int rec_id)
        {
            Database_Operations db_obj = new Database_Operations("GetReceiptCancelDetail", true);
            db_obj.AddParameter("@rec_id", rec_id);
            return (db_obj.GetDataTable());
        }

        public int CancelDeleteReceipt(int Id, int Status, string CancellationRemark, int CreatedBy)
        {
            Database_Operations db_obj = new Database_Operations("CancelDeleteReceipt", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Status", Status);
            db_obj.AddParameter("@CancellationRemark", CancellationRemark);
            db_obj.AddParameter("@UserId", CreatedBy);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region Service Completion

        public DataSet DebitNoteServiceCompletion(int SerCompletionId)
        {
            Database_Operations db_obj = new Database_Operations("DebitNoteServiceCompletion", true);
            db_obj.AddParameter("@SerCompletionId ", SerCompletionId);
            return (db_obj.GetDataSet());
        }
      

        public DataTable ListAccountInServCompletion(int PayModeId, int UserId, int AccountId)
        {
            Database_Operations db_obj = new Database_Operations("ListAccountInServCompletion", true);
            db_obj.AddParameter("@PayModeId", PayModeId);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddParameter("@AccountId", AccountId);
            return db_obj.GetDataTable();
        }

        public DataTable Get_List_Invoice_ForServiceCompletion_Excel(int Status)
        {
            Database_Operations db_obj = new Database_Operations("List_Invoice_ForServiceCompletion_Excel", true);
            db_obj.AddParameter("@Status", Status);
            return db_obj.GetDataTable();
        }

        public DataTable GetAllServiceSC(int page_number, int page_size, string filter, int Status, int UserId, 
            int? InvoiceCreator,int? ServiceStatusid)
        {
            Database_Operations db_obj = new Database_Operations("GetAllServiceSC", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Status", Status);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddParameter("@InvoiceCreator", InvoiceCreator);
            db_obj.AddParameter("@ServiceStatusid", ServiceStatusid);
            return db_obj.GetDataTable();
        }

        public DataTable GetAllServiceSCExcel(int Status)
        {
            Database_Operations db_obj = new Database_Operations("GetAllServiceSCExcel", true);
            db_obj.AddParameter("@Status", Status);
            return db_obj.GetDataTable();
        }

        public DataTable Get_List_Invoice_ForSC_Custm( int page_size, int UserId, int? InvoiceCreator,int? ServiceStatusid)
        {
            Database_Operations db_obj = new Database_Operations("Get_List_Invoice_ForSC_Custm", true);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddParameter("@InvoiceCreator", InvoiceCreator);
            db_obj.AddParameter("@ServiceStatusid", ServiceStatusid);
            return db_obj.GetDataTable();
        }

        public DataTable Get_List_Invoice_ForServiceCompletion(int page_number, int page_size, string filter,
            string column, string order, int Status,int UserId,int? InvoiceCreator, int? ServiceStatusid)
        {
            Database_Operations db_obj = new Database_Operations("List_Invoice_ForServiceCompletion", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            db_obj.AddParameter("@Status", Status);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddParameter("@InvoiceCreator", InvoiceCreator);
            db_obj.AddParameter("@ServiceStatusid", ServiceStatusid);
            return db_obj.GetDataTable();
        }
        public DataSet Get_InvDetail_ServiceCompletion(int InvoiceId, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Get_InvDetail_ServiceCompletion", true);
            db_obj.AddParameter("@InvoiceId", InvoiceId);
            db_obj.AddParameter("@UserId", UserId);
            return (db_obj.GetDataSet());
        }

        public DataSet Get_SerExpenseDetail_ServiceCompletion(int InvDetailId)
        {
            Database_Operations db_obj = new Database_Operations("Get_SerExpenseDetail_ServiceCompletion", true);
            db_obj.AddParameter("@InvDetailId", InvDetailId);
            return (db_obj.GetDataSet());
        }

        public int UpdateDescrepancy(int InvDetId, int IsDescrepancy, string Remark, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("UpdateDescrepancy", true);
            db_obj.AddParameter("@InvDetId", InvDetId);
            db_obj.AddParameter("@IsDescrepancy", IsDescrepancy);
            db_obj.AddParameter("@Remark", Remark);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }
        public int UpdateServiceStatus(int InvDetId, int StatusId, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("UpdateServiceStatus", true);
            db_obj.AddParameter("@InvDetId", InvDetId);
            db_obj.AddParameter("@StatusId", StatusId);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int Insert_Update_ServiceCompletion(int Id, int InvoiceId, int InvDetailId, decimal Quantity, decimal AmtSingleQty,
            decimal TotalAmount, DataSet dt_serv, DateTime? SerComDate, int UserId,string scremark, DataTable dtSCfile)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_ServiceCompletion", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@InvoiceId", InvoiceId);
            db_obj.AddParameter("@InvDetailId", InvDetailId);
            db_obj.AddParameter("@Quantity", Quantity);
            db_obj.AddParameter("@AmtSingleQty", AmtSingleQty);
            db_obj.AddParameter("@TotalAmount", TotalAmount);
            db_obj.AddParameter("@SerComDate", SerComDate);
            db_obj.AddParameter("@dt_expense", dt_serv.Tables[0]);
            db_obj.AddParameter("@dt_trans", dt_serv.Tables[1]);
            db_obj.AddParameter("@scremark", scremark);
            db_obj.AddParameter("@dtSCfile", dtSCfile);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int Insert_Update_ServiceCompletionAddtional(int Id, int InvoiceId, int InvDetailId, int Quantity, decimal AmtSingleQty,
         decimal TotalAmount, DataTable dt_serv, DateTime? SerComDate, int UserId,string Remarks)
        {
            Database_Operations db_obj = new Database_Operations("InsertUpdateSCAddtionalExpense", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@InvoiceId", InvoiceId);
            db_obj.AddParameter("@InvDetailId", InvDetailId);
            db_obj.AddParameter("@Quantity", Quantity);
            db_obj.AddParameter("@AmtSingleQty", AmtSingleQty);
            db_obj.AddParameter("@TotalAmount", TotalAmount);
            db_obj.AddParameter("@SerComDate", SerComDate);
            db_obj.AddParameter("@dt_expense", dt_serv);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddParameter("@Remarks", Remarks);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet GetServiceCompletionView(int InvoiceDetailId)
        {
            Database_Operations db_obj = new Database_Operations("GetServiceCompletion", true);
            db_obj.AddParameter("@InvoiceDetailId", InvoiceDetailId);
            return (db_obj.GetDataSet());
        }

        public DataSet EditServiceCompletion(int SerCompletionId)
        {
            Database_Operations db_obj = new Database_Operations("EditServiceCompletion", true);
            db_obj.AddParameter("@SerCompletionId ", SerCompletionId);
            return (db_obj.GetDataSet());
        }

        public int DeleteServiceCompletion(int Id, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("DeleteServiceCompletion", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region Employee Salary Process

        public DataTable Get_List_EmployeeSalaryProcess(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_EmployeeSalaryProcess", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public DataTable Get_List_EmployeeSalaryProcess_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_EmployeeSalaryProcess_Excel", true);
            return db_obj.GetDataTable();
        }

        public DataSet Edit_EmployeeSalaryProcess(int Id)
        {
            Database_Operations db_obj = new Database_Operations("Edit_EmployeeSalaryProcess", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataSet());
        }

        public DataSet GetEmployeeSalaryDetails(int Month, int Year)
        {
            Database_Operations db_obj = new Database_Operations("GetEmployeeSalaryDetails", true);
            db_obj.AddParameter("@Month", Month);
            db_obj.AddParameter("@Year", Year);
            return (db_obj.GetDataSet());
        }

        public int Insert_Update_EmployeeSalaryProcess(int Id, int Month, int Year, decimal Amount, string Remark,
             DataTable dtDetail, int UserId,DateTime? Dated)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_EmployeeSalaryProcess", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Month", Month);
            db_obj.AddParameter("@Year", Year);
            db_obj.AddParameter("@Amount", Amount);
            db_obj.AddParameter("@Remark", Remark);
            db_obj.AddParameter("@dtDetail", dtDetail);
            db_obj.AddParameter("@Dated", Dated);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int Delete_EmployeeSalaryProcess(int Id, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_EmployeeSalaryProcess", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region Employee Attendance

        public DataTable Get_List_EmployeeAttendance(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_EmployeeAttendance", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public DataTable Get_List_EmployeeAttendance_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_EmployeeAttendance_Excel", true);
            return db_obj.GetDataTable();
        }

        public DataSet Edit_EmployeeAttendance(int Id)
        {
            Database_Operations db_obj = new Database_Operations("Edit_EmployeeAttendance", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataSet());
        }

        public DataSet GetEmployeeDetailFromExcel(int Month, int Year, DataTable dtEmp)
        {
            Database_Operations db_obj = new Database_Operations("GetEmployeeDetailFromExcel", true);
            db_obj.AddParameter("@Month", Month);
            db_obj.AddParameter("@Year", Year);
            db_obj.AddParameter("@dtEmp", dtEmp);
            return db_obj.GetDataSet();
        }

        public DataTable GetEmployeeSalary(int EmployeeId, int Month, int Year)
        {
            Database_Operations db_obj = new Database_Operations("GetEmployeeSalary", true);
            db_obj.AddParameter("@Month", Month);
            db_obj.AddParameter("@Year", Year);
            db_obj.AddParameter("@EmployeeId", EmployeeId);
            return db_obj.GetDataTable();
        }

        public DataTable GetEmployeeListForAttendance(DataTable dtEmp)
        {
            Database_Operations db_obj = new Database_Operations("GetEmployeeListForAttendance", true);
            db_obj.AddParameter("@dtEmp", dtEmp);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_EmployeeAttendance(int Id, int Month, int Year, string FileName, string FileSavedName, string FileExtension,
             DataTable dtDetail, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_EmployeeAttendance", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Month", Month);
            db_obj.AddParameter("@Year", Year);
            db_obj.AddParameter("@FileName", FileName);
            db_obj.AddParameter("@FileSavedName", FileSavedName);
            db_obj.AddParameter("@FileExtension", FileExtension);
            db_obj.AddParameter("@dtDetail", dtDetail);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet GetEmployeeAttendance(int Month, int Year)
        {
            Database_Operations db_obj = new Database_Operations("GetEmployeeAttendance", true);
            db_obj.AddParameter("@Month", Month);
            db_obj.AddParameter("@Year", Year);
            return (db_obj.GetDataSet());
        }

        public int Delete_EmployeeAttendance(int Id, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_EmployeeAttendance", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region Quotation

        public DataTable Get_List_Quotation_Excel(int UserId)
        {
            Database_Operations db_obj = new Database_Operations("List_Quotation_Excel", true);
            db_obj.AddParameter("@UserId", UserId);
            return db_obj.GetDataTable();
        }

        public DataTable List_Quotation(int page_number, int page_size, string filter, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("List_Quotation", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@UserId", UserId);
            return db_obj.GetDataTable();
        }

        public DataSet Edit_Quotation(int Id, int Language, int SerPriceWithTax)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Quotation", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Language", Language);
            db_obj.AddParameter("@SerPriceWithTax", SerPriceWithTax);
            return (db_obj.GetDataSet());
        }

        public int Insert_Update_Quotation(int Id, DateTime? Qdate, int Cust_id, string remark, int UserId, decimal? TotGrand,
         DataTable dt_serv, int Version, int QuotationType,string Subject)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Quotation", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Qdate", Qdate);
            db_obj.AddParameter("@Cust_id", Cust_id);
            db_obj.AddParameter("@TotGrand", TotGrand);
            db_obj.AddParameter("@remark", remark);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddParameter("@dt_serv", dt_serv);
            db_obj.AddParameter("@Version", Version);
            db_obj.AddParameter("@QuotationType", QuotationType);
            db_obj.AddParameter("@Subject", Subject);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int GenerateDefaultQuotation(int Id, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("GenerateDefaultQuotation", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet GetQuotationbyLead(int LeadId)
        {
            Database_Operations db_obj = new Database_Operations("GetQuotationbyLead", true);
            db_obj.AddParameter("@LeadId", LeadId);
            return (db_obj.GetDataSet());
        }

        public int updateleadstatus(int? LeadId, int? QuotationId, int? InvoiceId, int Status, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("updateleadstatus", true);
            db_obj.AddParameter("@LeadId", LeadId);
            db_obj.AddParameter("@QuotationId", QuotationId);
            db_obj.AddParameter("@InvoiceId", InvoiceId);
            db_obj.AddParameter("@Status", Status);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int CancelQuotation(int Id,   string CancellationRemark, int CreatedBy)
        {
            Database_Operations db_obj = new Database_Operations("CancelQuotation", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@CancellationRemark", CancellationRemark);
            db_obj.AddParameter("@UserId", CreatedBy);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region QuickReceipt

        public DataTable GetQuotationDetails_invrecpt(int QuotationId, int Language, int SerPriceWithTax, int TaxAppliedWithDiscount,
     int InvoiceType, int AgId)
        {
            Database_Operations db_obj = new Database_Operations("GetQuotationDetails_invrecpt", true);
            db_obj.AddParameter("@QuotationId", QuotationId);
            db_obj.AddParameter("@Language", Language);
            db_obj.AddParameter("@SerPriceWithTax", SerPriceWithTax);
            db_obj.AddParameter("@TaxAppliedWithDiscount", TaxAppliedWithDiscount);
            db_obj.AddParameter("@InvoiceType", InvoiceType);
            db_obj.AddParameter("@AgId", AgId);
            return (db_obj.GetDataTable());
        }

        public DataTable GetServiceDetailsTemplate_invrecpt(DataTable dtTemplates, int Language,
            int SerPriceWithTax, int C_id, int TaxAppliedWithDiscount, int InvoiceType, int? AgId)
        {
            Database_Operations db_obj = new Database_Operations("GetServiceDetailsTemplate_invrecpt", true);
            db_obj.AddParameter("@dtTemplates", dtTemplates);
            db_obj.AddParameter("@Language", Language);
            db_obj.AddParameter("@SerPriceWithTax", SerPriceWithTax);
            db_obj.AddParameter("@C_id", C_id);
            db_obj.AddParameter("@TaxAppliedWithDiscount", TaxAppliedWithDiscount);
            db_obj.AddParameter("@InvoiceType", InvoiceType);
            db_obj.AddParameter("@AgId", AgId);
            return (db_obj.GetDataTable());
        }

        public DataTable Get_Services_Amount_invrecpt(int SerId, int CusId, int Language, int SerPriceWithTax, int C_id, int TaxAppliedWithDiscount, int InvoiceType, int AgId)
        {
            Database_Operations db_obj = new Database_Operations("Get_Services_Amount_invrecpt", true);
            db_obj.AddParameter("@SerId", SerId);
            db_obj.AddParameter("@CusId", CusId);
            db_obj.AddParameter("@Language", Language);
            db_obj.AddParameter("@SerPriceWithTax", SerPriceWithTax);
            db_obj.AddParameter("@C_id", C_id);
            db_obj.AddParameter("@TaxAppliedWithDiscount", TaxAppliedWithDiscount);
            db_obj.AddParameter("@InvoiceType", InvoiceType);
            db_obj.AddParameter("@AgId", AgId);
            return (db_obj.GetDataTable());
        }

        public int Insert_Update_Invoice_recSC(int Id, DateTime Qdate, int Cust_id, string remark, int UserId, decimal? TotGrand,
      DataTable dt_serv, int? Quot_id, int InvoiceType, int TaxAppliedWithDiscount, DataTable dtexpense, DataTable dtTrans)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Invoice_recSC", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Qdate", Qdate);
            db_obj.AddParameter("@Cust_id", Cust_id);
            db_obj.AddParameter("@TotGrand", TotGrand);
            db_obj.AddParameter("@remark", remark);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddParameter("@dt_serv", dt_serv);
            db_obj.AddParameter("@Quot_id", Quot_id);
            db_obj.AddParameter("@TaxAppliedWithDiscount", TaxAppliedWithDiscount);
            db_obj.AddParameter("@InvoiceType", InvoiceType);
            db_obj.AddParameter("@dtexpense", dtexpense);
            db_obj.AddParameter("@dtTrans", dtTrans);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }
        public int Insert_Update_Invoice_rec(int Id, DateTime Qdate, int Cust_id, string remark, int UserId, decimal? TotGrand,
      DataTable dt_serv, int? Quot_id, int InvoiceType, int TaxAppliedWithDiscount)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Invoice_rec", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Qdate", Qdate);
            db_obj.AddParameter("@Cust_id", Cust_id);
            db_obj.AddParameter("@TotGrand", TotGrand);
            db_obj.AddParameter("@remark", remark);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddParameter("@dt_serv", dt_serv);
            db_obj.AddParameter("@Quot_id", Quot_id);
            db_obj.AddParameter("@TaxAppliedWithDiscount", TaxAppliedWithDiscount);
            db_obj.AddParameter("@InvoiceType", InvoiceType);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int Insert_Update_Receipt_inv(int Id, DateTime Rdate, int InvoiceId, string Remark,
          decimal? TotalDiscount, decimal GrandTotAmt, decimal AmtPayNow, int PayModeId, int? AccountId,
          int? PettyCashId, DateTime? ChequeDate, string ChequeNumber, decimal PendingAmount, decimal ReceivedAmount, decimal Balance, int UserId,
            decimal? BankCommssn, int cardtype, int? CardAccount)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Receipt_inv", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Rdate", Rdate);
            db_obj.AddParameter("@InvoiceId", InvoiceId);
            db_obj.AddParameter("@Remark", Remark);
            db_obj.AddParameter("@TotalDiscount", TotalDiscount);
            db_obj.AddParameter("@GrandTotAmt", GrandTotAmt);
            db_obj.AddParameter("@AmtPayNow", AmtPayNow);
            db_obj.AddParameter("@PayModeId", PayModeId);
            db_obj.AddParameter("@AccountId", AccountId);
            db_obj.AddParameter("@PettyCashId", PettyCashId);
            db_obj.AddParameter("@ChequeDate", ChequeDate);
            db_obj.AddParameter("@ChequeNumber", ChequeNumber);
            db_obj.AddParameter("@PendingAmount", PendingAmount);
            db_obj.AddParameter("@ReceivedAmount", ReceivedAmount);
            db_obj.AddParameter("@Balance", Balance);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddParameter("@BankCommssn", BankCommssn);
            db_obj.AddParameter("@cardtype", cardtype);
            db_obj.AddParameter("@CardAccount", CardAccount);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_List_InvoiceREceipt(int page_number, int page_size, string filter, string column, string order, int userid)
        {
            Database_Operations db_obj = new Database_Operations("Get_List_InvoiceREceipt", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            db_obj.AddParameter("@userid", userid);
            return db_obj.GetDataTable();
        }

        public DataSet Edit_InvoiceREceipt(int id, int Language, int SerPriceWithTax)
        {
            Database_Operations db_obj = new Database_Operations("Edit_InvoiceREceipt", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Language", Language);
            db_obj.AddParameter("@SerPriceWithTax", SerPriceWithTax);
            return (db_obj.GetDataSet());
        }

        public DataSet Edit_ReceiptInv(int Id, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Edit_ReceiptInv", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@UserId", UserId);
            return (db_obj.GetDataSet());
        }

        public int Cancel_InvoiceReceipt(int incmid, string reasn, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Cancel_InvoiceReceipt", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddParameter("@reasn", reasn);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region Bank Reconciliation

        public DataTable Get_List_BankReconciliation(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_BankReconciliation", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public DataTable Get_List_BankReconciliation_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_BankReconciliation_Excel", true);
            return db_obj.GetDataTable();
        }

        public DataSet Edit_BankReconciliation(int Id)
        {
            Database_Operations db_obj = new Database_Operations("Edit_BankReconciliation", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataSet());
        }

        public DataSet GetBankReconciliation(DateTime FromDate, DateTime ToDate, int BankAccountId, DataTable dtDetails)
        {
            //Database_Operations db_obj = new Database_Operations("GetBankReconciliation", true);
            Database_Operations db_obj = new Database_Operations("GetBankReconciliationNew", true);
            db_obj.AddParameter("@FromDate", FromDate);
            db_obj.AddParameter("@ToDate", ToDate);
            db_obj.AddParameter("@BankAccountId", BankAccountId);
            db_obj.AddParameter("@dtDetails", dtDetails);
            return db_obj.GetDataSet();
        }

        public int Insert_Update_BankReconciliation(int Id, DateTime FromDate, DateTime ToDate, int BankAccountId, string FileName, string FileSavedName, string FileExtension,
             DataTable dtDetail, int UserId)
        {
            //Database_Operations db_obj = new Database_Operations("Insert_Update_BankReconciliation", true);
            Database_Operations db_obj = new Database_Operations("InsertUpdateBankReconciliation", true);

            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@FromDate", FromDate);
            db_obj.AddParameter("@ToDate", ToDate);
            db_obj.AddParameter("@BankAccountId", BankAccountId);
            db_obj.AddParameter("@FileName", FileName);
            db_obj.AddParameter("@FileSavedName", FileSavedName);
            db_obj.AddParameter("@FileExtension", FileExtension);
            db_obj.AddParameter("@dtDetail", dtDetail);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int Delete_BankReconciliation(int Id, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_BankReconciliation", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region Customer Invoice

        public DataTable Get_List_CustomerInvoice(int page_number, int page_size, string filter, string column, string order, int userid,
            DateTime? invoicedate,int? CustomerId,int? AgentId,DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("Get_List_CustomerInvoice", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            db_obj.AddParameter("@userid", userid);
            db_obj.AddParameter("@invoicedate", invoicedate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@AgentId", AgentId);
            db_obj.AddParameter("@Todate", Todate);
            return db_obj.GetDataTable();
        }

        /*Get Detail in Excel*/
        public DataTable Get_List_Customerinvoice_Excel(int userid, DateTime? invoicedate, int? CustomerId, int? AgentId, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("Get_List_Customerinvoice_Excel", true);
            db_obj.AddParameter("@userid", userid);
            db_obj.AddParameter("@invoicedate", invoicedate);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@AgentId", AgentId);
            db_obj.AddParameter("@Todate", Todate);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_CustomerInvoice(int Id, DateTime Qdate, int Cust_id, string remark, int UserId,
            decimal? TotDiscount, decimal? TotGrand, DataTable dt_serv)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_CustomerInvoice", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Qdate", Qdate);
            db_obj.AddParameter("@Cust_id", Cust_id);
            db_obj.AddParameter("@TotDiscount", TotDiscount);
            db_obj.AddParameter("@TotGrand", TotGrand);
            db_obj.AddParameter("@remark", remark);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddParameter("@dt_serv", dt_serv);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet Edit_CustomerInvoice(int id)
        {
            Database_Operations db_obj = new Database_Operations("Edit_CustomerInvoice", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public DataTable getCustomerInvoiceCancelDetail(int inv_id)
        {
            Database_Operations db_obj = new Database_Operations("CustomerInvoiceCancelDetail", true);
            db_obj.AddParameter("@inv_id", inv_id);
            return (db_obj.GetDataTable());
        }



        public int Cancel_CustomerInvoice(int incmid, string reasn, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Cancel_CustomerInvoice", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddParameter("@reasn", reasn);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable drp_PendingInvoice(int CusId, int id, DateTime? frmdate, DateTime? todate)
        {
            Database_Operations db_obj = new Database_Operations("drp_PendingInvoice", true);
            db_obj.AddParameter("@CusId", CusId);
            db_obj.AddParameter("@id", id);
            db_obj.AddParameter("@frmdate", frmdate);
            db_obj.AddParameter("@todate", todate);
            return db_obj.GetDataTable();
        }

        public DataTable GetInvoiceDetails(DataTable dtinvoice, int id, DateTime? frmdate, DateTime? todate, int? service, int CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("GetInvoiceDetails", true);
            db_obj.AddParameter("@dtinvoice", dtinvoice);
            db_obj.AddParameter("@id", id);
            db_obj.AddParameter("@frmdate", frmdate);
            db_obj.AddParameter("@todate", todate);
            db_obj.AddParameter("@service", service);
            db_obj.AddParameter("@CustomerId", CustomerId);
            return (db_obj.GetDataTable());
        }

        public DataSet list_CustomerInvHistry(DateTime? from, DateTime? to, int Qid, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("list_CustomerInvHistry", true);
            db_obj.AddParameter("@From", from);
            db_obj.AddParameter("@To", to);
            db_obj.AddParameter("@Qid", Qid);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            return db_obj.GetDataSet();
        }

        #endregion

        #region Customer Receipt

        public DataTable Get_List_CustomerReceipt(int page_number, int page_size, string filter, string column, string order, int userid)
        {
            Database_Operations db_obj = new Database_Operations("List_CustomerReceipt", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            db_obj.AddParameter("@userid", userid);
            return db_obj.GetDataTable();
        }

        public DataTable Get_List_CustomerReceipt_Excel(int userid)
        {
            Database_Operations db_obj = new Database_Operations("List_CustomerReceipt_Excel", true);
            db_obj.AddParameter("@userid", userid);
            return db_obj.GetDataTable();
        }

        public DataSet Edit_CustomerReceipt(int Id, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Edit_CustomerReceipt", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@UserId", UserId);
            return (db_obj.GetDataSet());
        }

        public DataSet Get_CustomerInvoice(string InvoiceCode, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Get_CustomerInvoice", true);//Get_Invoice
            db_obj.AddParameter("@InvoiceCode", InvoiceCode);
            db_obj.AddParameter("@UserId", UserId);
            return (db_obj.GetDataSet());
        }

        public int Insert_Update_CustomerReceipt(int Id, DateTime Rdate, int InvoiceId, string Remark,
            decimal? TotalDiscount, decimal GrandTotAmt, decimal AmtPayNow, int PayModeId, int? AccountId,
            int? PettyCashId, DateTime? ChequeDate, string ChequeNumber, decimal PendingAmount, decimal ReceivedAmount, decimal Balance,
            DataTable dt_serv, int UserId, decimal? BankCommsn)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_CustomerReceipt", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Rdate", Rdate);
            db_obj.AddParameter("@InvoiceId", InvoiceId);
            db_obj.AddParameter("@Remark", Remark);
            db_obj.AddParameter("@TotalDiscount", TotalDiscount);
            db_obj.AddParameter("@GrandTotAmt", GrandTotAmt);
            db_obj.AddParameter("@AmtPayNow", AmtPayNow);
            db_obj.AddParameter("@PayModeId", PayModeId);
            db_obj.AddParameter("@AccountId", AccountId);
            db_obj.AddParameter("@PettyCashId", PettyCashId);
            db_obj.AddParameter("@ChequeDate", ChequeDate);
            db_obj.AddParameter("@ChequeNumber", ChequeNumber);
            db_obj.AddParameter("@PendingAmount", PendingAmount);
            db_obj.AddParameter("@ReceivedAmount", ReceivedAmount);
            db_obj.AddParameter("@BankCommsn", BankCommsn);
            db_obj.AddParameter("@Balance", Balance);
            db_obj.AddParameter("@dt_serv", dt_serv);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        //public DataTable GetCustomerReceiptCancelDetail(int rec_id)
        //{
        //    Database_Operations db_obj = new Database_Operations("GetReceiptCancelDetail", true);
        //    db_obj.AddParameter("@rec_id", rec_id);
        //    return (db_obj.GetDataTable());
        //}

        public int CustomerCancelDeleteReceipt(int Id, int Status, string CancellationRemark, int CreatedBy)
        {
            Database_Operations db_obj = new Database_Operations("CustomerCancelDeleteReceipt", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Status", Status);
            db_obj.AddParameter("@CancellationRemark", CancellationRemark);
            db_obj.AddParameter("@UserId", CreatedBy);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region Documnet collection

        public int insertupdateDocCollection(int id, DateTime ondate, int custid, string desc, int UserId, DataTable dt_doc)
        {
            Database_Operations db_obj = new Database_Operations("insertupdateDocCollection", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@ondate", ondate);
            db_obj.AddParameter("@custid", custid);
            db_obj.AddParameter("@desc", desc);
            db_obj.AddParameter("@dt_doc", dt_doc);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet EditDocColl(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("EditDocColl", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        //Get List of Data
        public DataTable ListDocColl(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("ListDocColl", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int deleteDocColl(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("deleteDocColl", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable DocCollExcel()
        {
            Database_Operations db_obj = new Database_Operations("DocCollExcel", true);
            return db_obj.GetDataTable();
        }

        #endregion

        #region  Doc Return

        public DataTable ListDocReturn(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("ListDocReturn", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public DataTable DocReturnExcel()
        {
            Database_Operations db_obj = new Database_Operations("DocReturnExcel", true);
            return db_obj.GetDataTable();
        }

        public DataSet EditDocReturn(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("EditDocReturn", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public DataSet EditDocReturnDoc(int incmid, int CustId)
        {
            Database_Operations db_obj = new Database_Operations("EditDocReturnDoc", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@CustId", CustId);
            return (db_obj.GetDataSet());
        }

        public int insertupdateDocReturn(int id, DateTime ondate, int custid, string desc, int UserId, DataTable dt_doc)
        {
            Database_Operations db_obj = new Database_Operations("insertupdateDocReturn", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@ondate", ondate);
            db_obj.AddParameter("@custid", custid);
            db_obj.AddParameter("@desc", desc);
            db_obj.AddParameter("@dt_doc", dt_doc);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int deleteDocReturn(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("deleteDocReturn", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet GetForDocReturn(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("GetForDocReturn", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        #endregion

        #region doc transfer

        public DataTable Drp_DocAgent()
        {
            Database_Operations db_obj = new Database_Operations("Drp_DocAgent", true);
            return (db_obj.GetDataTable());
        }

        //Get List of Data
        public DataTable Get_List_DocTrans(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_DocTrans", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public DataTable excel_get_DocTransf_type()
        {
            Database_Operations db_obj = new Database_Operations("excel_get_DocTrans_type", true);
            return db_obj.GetDataTable();
        }

        public int Delete_DocTransfer(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("delete_DocTransfer", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int Insert_Update_DocumentTransf(int id, DateTime ondate, int custid, string desc, int UserId, DataTable dt_doc)
        {
            Database_Operations db_obj = new Database_Operations("insert_update_DocTransfr", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@ondate", ondate);
            db_obj.AddParameter("@custid", custid);
            db_obj.AddParameter("@desc", desc);
            db_obj.AddParameter("@dt_doc", dt_doc);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet Edit_DocTransfr(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("Edit_DocTransf", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public DataSet Get_Pending_Doc(string filter)
        {
            Database_Operations db_obj = new Database_Operations("Get_Pending_Doc", true);
            db_obj.AddParameter("@filter", filter);
            return (db_obj.GetDataSet());
        }

        #endregion

        #region  Doc Return from agent

        public DataTable Get_List_DocReturnAgent(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_DocReturnAgent", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public DataTable excel_get_DocReAgenttype()
        {
            Database_Operations db_obj = new Database_Operations("excel_get_DocAgent_type", true);
            return db_obj.GetDataTable();
        }

        public DataSet Edit_DocReturnAgent(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("Edit_DocReturnAgent", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public DataSet Edit_DocReturnAgent_Doc(int incmid, int CustId)
        {
            Database_Operations db_obj = new Database_Operations("Edit_DocReturnAgent_Doc", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@CustId", CustId);
            return (db_obj.GetDataSet());
        }

        public int Insert_Update_DocumentreturnAgent(int id, DateTime ondate, int custid, string desc, int UserId, DataTable dt_doc)
        {
            Database_Operations db_obj = new Database_Operations("insert_update_DocReturnAgent", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@ondate", ondate);
            db_obj.AddParameter("@custid", custid);
            db_obj.AddParameter("@desc", desc);
            db_obj.AddParameter("@dt_doc", dt_doc);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int Delete_DocReturnAgent(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("delete_DocReturnAgent", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet Get_DocReturnAgent(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("GetFor_DocReturnAgent", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        #endregion

        #region PromotionalMail

        public DataTable GetPromotionalMailList(int PageNumber, int PageSize, string Filter, string OrderByColumnName, string OrderBy)
        {
            Database_Operations databaseOperations = new Database_Operations("GetPromotionalMailList", true);
            databaseOperations.AddParameter("@PageNumber", PageNumber);
            databaseOperations.AddParameter("@PageSize", PageSize);
            databaseOperations.AddParameter("@Filter", Filter);
            databaseOperations.AddParameter("@OrderByColumnName", OrderByColumnName);
            databaseOperations.AddParameter("@OrderBy", OrderBy);
            return databaseOperations.GetDataTable();
        }

        public DataTable GePromotionalMailExcel()
        {
            Database_Operations databaseOperations = new Database_Operations("GePromotionalMailExcel", true);
            return databaseOperations.GetDataTable();
        }

        public DataSet EditPromotionalMail(int Id)
        {
            Database_Operations databaseOperations = new Database_Operations("EditPromotionalMail", true);
            databaseOperations.AddParameter("@Id", Id);
            return (databaseOperations.GetDataSet());
        }

        public int InsertPromotionalMail(int Id, DateTime? Dates, int? TemplateId, string Subject, string MailContent,
            int UserId, DataTable dtReceiver, string FileName)
        {
            Database_Operations databaseOperations = new Database_Operations("InsertPromotionalMail", true);
            databaseOperations.AddParameter("@Id", Id);
            databaseOperations.AddParameter("@Dates", Dates);
            databaseOperations.AddParameter("@TemplateId", TemplateId);
            databaseOperations.AddParameter("@Subject", Subject);
            databaseOperations.AddParameter("@MailContent", MailContent);
            databaseOperations.AddParameter("@dtReceiver", dtReceiver);
            databaseOperations.AddParameter("@FileName", FileName);
            databaseOperations.AddParameter("@UserId", UserId);
            databaseOperations.AddOutputParameter("@Result");
            databaseOperations.ExecuteQuery();
            return Convert.ToInt32(databaseOperations.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable GetEmailListP(int Id)
        {
            Database_Operations databaseOperations = new Database_Operations("GetEmailListP", true);
            databaseOperations.AddParameter("@Id", Id);
            return (databaseOperations.GetDataTable());
        }

        #endregion

        #region Profitsharing

        public DataSet GetProfitsharing(int Month, int Year)
        {
            Database_Operations db_obj = new Database_Operations("GetProfitsharing", true);
            db_obj.AddParameter("@Month", Month);
            db_obj.AddParameter("@Year", Year);
            return (db_obj.GetDataSet());
        }

        public int InsertProfitSharing(int Id, int Month, int Year, DateTime? Dated,  decimal NetProfit,
             string Remark,DataTable dtPartner, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("InsertProfitSharing", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Month", Month);
            db_obj.AddParameter("@Year", Year);
            db_obj.AddParameter("@Dated", Dated);
            db_obj.AddParameter("@Remark", Remark);
            db_obj.AddParameter("@NetProfit", NetProfit);
            db_obj.AddParameter("@dtPartner", dtPartner);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int CancelProfitSharing(int Id,  int UserId)
        {
            Database_Operations db_obj = new Database_Operations("CancelProfitSharing", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet EditProfitsharing(int Id)
        {
            Database_Operations db_obj = new Database_Operations("EditProfitsharing", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataSet());
        }

        public DataTable ListProfitsharing(int page_number, int page_size, string filter)
        {
            Database_Operations db_obj = new Database_Operations("ListProfitsharing", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            return db_obj.GetDataTable();
        }

        public DataTable ListExcelProfitsharing()
        {
            Database_Operations db_obj = new Database_Operations("ListExcelProfitsharing", true);
            return db_obj.GetDataTable();
        }

        #endregion

        #region VisaArrival

        public DataTable Get_ListVisaArrival(int page_number, int page_size, string filter)
        {
            Database_Operations db_obj = new Database_Operations("Get_ListVisaArrival", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            return db_obj.GetDataTable();
        }

        public int InsertUpdateVisaArrival(int id, string Name,  DateTime? Dated, DateTime? reacheddate, DateTime? returndate)
        {
            Database_Operations db_obj = new Database_Operations("InsertUpdateVisaArrival", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", Name);
            db_obj.AddParameter("@Dated", Dated);
            db_obj.AddParameter("@reacheddate", reacheddate);
            db_obj.AddParameter("@returndate", returndate);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet EditVisaArrival(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("EditVisaArrival", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int DeleteVisaArrival(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("DeleteVisaArrival", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable VisaArrival_Excel()
        {
            Database_Operations db_obj = new Database_Operations("VisaArrival_Excel", true);
            return db_obj.GetDataTable();
        }

        #endregion

        #region Creditnote

        public DataTable DrpPendingCreditInvoice(int CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("DrpPendingCreditInvoice", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            return db_obj.GetDataTable();
        }

        public DataSet Get_CreditInvoice(int InvoiceId, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Get_CreditInvoice", true);
            db_obj.AddParameter("@InvoiceId", InvoiceId);
            db_obj.AddParameter("@UserId", UserId);
            return (db_obj.GetDataSet());
        }

        public DataTable Get_ListCreditnote(int page_number, int page_size, string filter, string column, string order, int userid)
        {
            Database_Operations db_obj = new Database_Operations("Get_ListCreditnote", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            db_obj.AddParameter("@userid", userid);
            return db_obj.GetDataTable();
        }

        public DataTable Get_ListCreditnoteExcel(int userid)
        {
            Database_Operations db_obj = new Database_Operations("Get_ListCreditnoteExcel", true);
            db_obj.AddParameter("@userid", userid);
            return db_obj.GetDataTable();
        }

        public DataSet EditCreditnote(int Id, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("EditCreditnote", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@UserId", UserId);
            return (db_obj.GetDataSet());
        }

        public int Insert_UpdateCreditnote(int Id, DateTime? Rdate, int InvoiceId,int CustomerId, string Remark,
          decimal TotalTax, decimal TotalAmount, DataTable dt_serv, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_UpdateCreditnote", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Rdate", Rdate);
            db_obj.AddParameter("@InvoiceId", InvoiceId);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@Remark", Remark);
            db_obj.AddParameter("@TotalTax", TotalTax);
            db_obj.AddParameter("@TotalAmount", TotalAmount);
            db_obj.AddParameter("@dt_serv", dt_serv);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }


        public int CancelCreditnote(int Id, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("CancelCreditnote", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        // =============================================
        // Updated section only: #region LeadCreation in Transaction_Bal.cs
        // Replace the existing InsertUpdateLeadCreation method with this one.
        // All other methods remain unchanged.
        // =============================================

        #region LeadCreation

        public DataTable GetLeadList(int PageNumber, int PageSize, string Filter, int StatusId, int PriorityId, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("GetLeadList", true);
            db_obj.AddParameter("@PageNumber", PageNumber);
            db_obj.AddParameter("@PageSize", PageSize);
            db_obj.AddParameter("@Filter", Filter);
            db_obj.AddParameter("@StatusId", StatusId);
            db_obj.AddParameter("@PriorityId", PriorityId);
            db_obj.AddParameter("@UserId", UserId);
            return db_obj.GetDataTable();
        }

        public DataTable GetLeadCreationListExcel(int UserId)
        {
            Database_Operations db_obj = new Database_Operations("GetLeadCreationListExcel", true);
            db_obj.AddParameter("@UserId", UserId);
            return db_obj.GetDataTable();
        }

        public DataSet EditLeadCreation(int Id)
        {
            Database_Operations db_obj = new Database_Operations("EditLeadCreation", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataSet());
        }

        public DataTable LeadMailBody(int LeadId)
        {
            Database_Operations db_obj = new Database_Operations("LeadMailBody", true);
            db_obj.AddParameter("@LeadId", LeadId);
            return db_obj.GetDataTable();
        }

        /// <summary>
        /// Insert or update a Lead record.
        /// New parameters added: LeadBrand, PassportNo, PassportIssueDate, PassportExpiryDate,
        /// CurrentStatus, DOB, Nationality, MaritalStatus, MotherName.
        /// </summary>
        public int InsertUpdateLeadCreation(
            int Id, string Name, string Address, string MobileNumber, string EmailId, int UserId,
            int? EmployeeId, string phone, string company, string response, int Priority, int? SourceId,
            DateTime? Follwup, DateTime? apprclosingdate, DataTable dtService, DateTime? NextFollowupTime,
            int? JurisdictionId, DateTime? LeadDate,
            string Activity, string ContactPersonDesig, string Website, string Campaign,
            string CountryCodeCN, string CountryCodeLPN, int? CityId, int? SegmentId,
            // NEW FIELDS
            string LeadBrand, string PassportNo,
            DateTime? PassportIssueDate, DateTime? PassportExpiryDate,
            int? CurrentStatus, DateTime? DOB,
            string Nationality, int? MaritalStatus, string MotherName)
        {
            Database_Operations db_obj = new Database_Operations("InsertUpdateLeadCreation", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Name", Name);
            db_obj.AddParameter("@Address", Address);
            db_obj.AddParameter("@MobileNumber", MobileNumber);
            db_obj.AddParameter("@EmployeeId", EmployeeId);
            db_obj.AddParameter("@EmailId", EmailId);
            db_obj.AddParameter("@phone", phone);
            db_obj.AddParameter("@company", company);
            db_obj.AddParameter("@response", response);
            db_obj.AddParameter("@Priority", Priority);
            db_obj.AddParameter("@SourceId", SourceId);
            db_obj.AddParameter("@Follwup", Follwup);
            db_obj.AddParameter("@apprclosingdate", apprclosingdate);
            db_obj.AddParameter("@dtService", dtService);
            db_obj.AddParameter("@NextFollowupTime", NextFollowupTime);
            db_obj.AddParameter("@JurisdictionId", JurisdictionId);
            db_obj.AddParameter("@LeadDate", LeadDate);
            db_obj.AddParameter("@Activity", Activity);
            db_obj.AddParameter("@ContactPersonDesig", ContactPersonDesig);
            db_obj.AddParameter("@Website", Website);
            db_obj.AddParameter("@Campaign", Campaign);
            db_obj.AddParameter("@CountryCodeCN", CountryCodeCN);
            db_obj.AddParameter("@CountryCodeLPN", CountryCodeLPN);
            db_obj.AddParameter("@CityId", CityId);
            db_obj.AddParameter("@SegmentId", SegmentId);
            // NEW FIELDS
            db_obj.AddParameter("@LeadBrand", LeadBrand);
            db_obj.AddParameter("@PassportNo", PassportNo);
            db_obj.AddParameter("@PassportIssueDate", PassportIssueDate);
            db_obj.AddParameter("@PassportExpiryDate", PassportExpiryDate);
            db_obj.AddParameter("@CurrentStatus", CurrentStatus);
            db_obj.AddParameter("@DOB", DOB);
            db_obj.AddParameter("@Nationality", Nationality);
            db_obj.AddParameter("@MaritalStatus", MaritalStatus);
            db_obj.AddParameter("@MotherName", MotherName);
            db_obj.AddParameter("@UserId", UserId);

            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int DeleteLeadCreation(int Id, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("DeleteLeadCreation", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet LeadUploadTable(DataTable ContentTable, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("LeadUploadTable", true);
            db_obj.AddParameter("@ContentTable", ContentTable);
            db_obj.AddParameter("@UserId", UserId);
            return db_obj.GetDataSet();
        }

        public int InsertLeadList(DataTable ContentTable, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("InsertLeadList", true);
            db_obj.AddParameter("@ContentTable", ContentTable);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }


        /// <summary>Fetch all document rows saved against a lead.</summary>
        public DataTable GetLeadDocuments(int LeadId)
        {
            Database_Operations db_obj = new Database_Operations("GetLeadDocuments", true);
            db_obj.AddParameter("@LeadId", LeadId);
            return db_obj.GetDataTable();
        }

        /// <summary>
        /// Save (insert new / update existing) document rows for a lead.
        /// Uses the LeadDocumentType table-valued parameter.
        /// </summary>
        public int SaveLeadDocuments(int LeadId, DataTable dtDocuments, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("SaveLeadDocuments", true);
            db_obj.AddParameter("@LeadId", LeadId);
            db_obj.AddParameter("@dtDocuments", dtDocuments);   // TVP – LeadDocumentType
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        /// <summary>Delete a single document record by Id.</summary>
        public int DeleteLeadDocument(int Id, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("DeleteLeadDocument", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region LeadTransfer

        public DataTable GetLeadTransferBulkList(int EmployeeId)
        {
            Database_Operations databaseOperations = new Database_Operations("GetLeadTransferBulkList", true);
            databaseOperations.AddParameter("@EmployeeId", EmployeeId);
            return databaseOperations.GetDataTable();
        }

        public DataTable GetLeadTransferList(int PageNumber, int PageSize, string Filter, string OrderByColumnName, string OrderBy)
        {
            Database_Operations databaseOperations = new Database_Operations("GetLeadTransferList", true);
            databaseOperations.AddParameter("@PageNumber", PageNumber);
            databaseOperations.AddParameter("@PageSize", PageSize);
            databaseOperations.AddParameter("@Filter", Filter);
            databaseOperations.AddParameter("@OrderByColumnName", OrderByColumnName);
            databaseOperations.AddParameter("@OrderBy", OrderBy);
            return databaseOperations.GetDataTable();
        }

        public DataTable GetLeadTransferListExcel()
        {
            Database_Operations databaseOperations = new Database_Operations("GetLeadTransferListExcel", true);
            return databaseOperations.GetDataTable();
        }

        public int UpdateLeadTransfer(int Id, int UserId, int? EmployeeId, string Remark)
        {
            Database_Operations databaseOperations = new Database_Operations("UpdateLeadTransfer", true);
            databaseOperations.AddParameter("@Id", Id);
            databaseOperations.AddParameter("@EmployeeId", EmployeeId);
            databaseOperations.AddParameter("@Remark", Remark);
            databaseOperations.AddParameter("@UserId", UserId);
            databaseOperations.AddOutputParameter("@Result");
            databaseOperations.ExecuteQuery();
            return Convert.ToInt32(databaseOperations.SqlCmd.Parameters["@Result"].Value.ToString());
        }
        public void UpdateLeadTransferBulk(DataTable dtdetail, int UserId, int EmployeeId)
        {
            Database_Operations databaseOperations = new Database_Operations("UpdateLeadTransferBulk", true);
            databaseOperations.AddParameter("@dtdetail", dtdetail);
            databaseOperations.AddParameter("@UserId", UserId);
            databaseOperations.AddParameter("@EmployeeId", EmployeeId);
            databaseOperations.ExecuteQuery();
        }

        #endregion

        #region LeadFolowup

        public DataTable GetLeadFolowupList(int PageNumber, int PageSize, string Filter, int UserId, int StatusId, int PriorityId,
            DateTime? Fromdate, DateTime? Todate, int SegmentId)
        {
            Database_Operations db_obj = new Database_Operations("GetLeadFolowupList", true);
            db_obj.AddParameter("@PageNumber", PageNumber);
            db_obj.AddParameter("@PageSize", PageSize);
            db_obj.AddParameter("@Filter", Filter);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddParameter("@StatusId", StatusId);
            db_obj.AddParameter("@PriorityId", PriorityId);
            db_obj.AddParameter("@Fromdate", Fromdate);
            db_obj.AddParameter("@Todate", Todate);
            db_obj.AddParameter("@SegmentId", SegmentId);
            return db_obj.GetDataTable();
        }

        public DataTable GetLeadFolowupListExcel(int UserId)
        {
            Database_Operations db_obj = new Database_Operations("GetLeadFolowupListExcel", true);
            db_obj.AddParameter("@UserId", UserId);
            return db_obj.GetDataTable();
        }

        public int InsertLeadFolowup(int Id, int UserId, string response, int Status, DateTime? Follwup,
            DateTime? currentdate, string Remark, DateTime? NextFollowupTime, DataTable dtService, string company, int PriorityId,
            int SegmentId, string ContactPersonDesig, string Website)
        {
            Database_Operations db_obj = new Database_Operations("InsertLeadFolowup", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Status", Status);
            db_obj.AddParameter("@currentdate", currentdate);
            db_obj.AddParameter("@Remark", Remark);
            db_obj.AddParameter("@response", response);
            db_obj.AddParameter("@Follwup", Follwup);
            db_obj.AddParameter("@NextFollowupTime", NextFollowupTime);
            db_obj.AddParameter("@dtService", dtService);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddParameter("@company", company);
            db_obj.AddParameter("@PriorityId", PriorityId);
            db_obj.AddParameter("@SegmentId", SegmentId);
            db_obj.AddParameter("@ContactPersonDesig", ContactPersonDesig);
            db_obj.AddParameter("@Website", Website);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable ListLeadHistory(int LeadId, int PageNumber, int PageSize)
        {
            Database_Operations db_obj = new Database_Operations("ListLeadHistory", true);
            db_obj.AddParameter("@PageNumber", PageNumber);
            db_obj.AddParameter("@PageSize", PageSize);
            db_obj.AddParameter("@LeadId", LeadId);
            return db_obj.GetDataTable();
        }
        public DataTable ListLeadHistoryPrintExcel(int LeadId)
        {
            Database_Operations db_obj = new Database_Operations("ListLeadHistoryPrintExcel", true);
            db_obj.AddParameter("@LeadId", LeadId);
            return db_obj.GetDataTable();
        }

        #endregion

        #region Task

        public DataTable GetPendingTask(int UserId)
        {
            Database_Operations db_obj = new Database_Operations("GetPendingTask", true);
            db_obj.AddParameter("@UserId", UserId);
            return db_obj.GetDataTable();
        }

        #endregion

        #region VendorBalMap

        public DataTable listVendorBalMap(int page_number, int page_size, string filter, string column, string order, int userid)
        {
            Database_Operations db_obj = new Database_Operations("listVendorBalMap", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@userid", userid);
            return db_obj.GetDataTable();
        }

        public DataTable listVendorBalMapExcel()
        {
            Database_Operations db_obj = new Database_Operations("listVendorBalMapExcel", true);
            return db_obj.GetDataTable();
        }

        public DataSet EditVendorBalMap(int Id )
        {
            Database_Operations db_obj = new Database_Operations("EditVendorBalMap", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataSet());
        }

        public int InsertUpdateVendorBalMap(int Id, DateTime? Rdate, int VendorId,int CustomerId, decimal TotalAmount, DataTable dt_serv, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("InsertUpdateVendorBalMap", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Rdate", Rdate);
            db_obj.AddParameter("@VendorId", VendorId);
            db_obj.AddParameter("@TotalAmount", TotalAmount);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@dt_serv", dt_serv);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int CancelVendorBalMap(int Id, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("CancelVendorBalMap", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region debitnote

        public DataTable Get_ListDebitnote(int page_number, int page_size, string filter, string column, string order, int userid)
        {
            Database_Operations db_obj = new Database_Operations("Get_ListDebitnote", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            db_obj.AddParameter("@userid", userid);
            return db_obj.GetDataTable();
        }//added by faisal 
        public DataTable Get_ListDebitnoteExcel(int userid)
        {
            Database_Operations db_obj = new Database_Operations("Get_ListDebitnoteExcel", true);
            db_obj.AddParameter("@userid", userid);
            return db_obj.GetDataTable();
        }//added by Faisal
        public DataSet EditDebitnote(int Id, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Editdebitnote", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@UserId", UserId);
            return (db_obj.GetDataSet());
        }

        public int Insert_UpdateDebitnote(int Id, DateTime? Rdate, int InvoiceId, int SCId, decimal Qty, string Remark,
                  decimal TotalAmount, DataTable dt_serv, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_UpdateDebitnote", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Rdate", Rdate);
            db_obj.AddParameter("@SCId", SCId);
            db_obj.AddParameter("@Qty", Qty);
            db_obj.AddParameter("@Remark", Remark);
            db_obj.AddParameter("@invoiceid", InvoiceId);
            db_obj.AddParameter("@TotalAmount", TotalAmount);
            db_obj.AddParameter("@dt_serv", dt_serv);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }
        
        public DataTable Drp_InvoicebyCustomer(int CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("drp_invoicebycustomer", true);
            db_obj.AddParameter("@customerid", CustomerId);
            return (db_obj.GetDataTable());
        }//faisal 31-10-2025
        public DataTable Drp_DebitnoteSCbyInvoice(int InvoiceId)
        {
            Database_Operations db_obj = new Database_Operations("drp_debitnotescbyinvoice", true);
            db_obj.AddParameter("@invoiceid", InvoiceId);
            return (db_obj.GetDataTable());
        }//faisal 31-10-2025

        #endregion

        #region CustomerPort

        public DataTable ListCustomerInvoiceList(int page_number, int page_size, string filter, int userid)
        {
            Database_Operations db_obj = new Database_Operations("ListCustomerInvoiceList", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@userid", userid);
            return db_obj.GetDataTable();
        }

        public DataSet EditCustomerInvoiceList(int id)
        {
            Database_Operations db_obj = new Database_Operations("EditCustomerInvoiceList", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public DataSet CChequeDetailList(int? CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("CChequeDetailList", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            return db_obj.GetDataSet();
        }

        public DataTable GetSCListCustomer(int page_number, int page_size, string filter, int CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("GetSCListCustomer", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@CustomerId", CustomerId);
            return db_obj.GetDataTable();
        }
        public DataSet ListDocumentDwnloadInCustomerPort(int Pagenumber, int Count, string filter, int CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("ListDocumentDwnloadInCustomerPort", true);
            db_obj.AddParameter("@page_number", Pagenumber);
            db_obj.AddParameter("@count", Count);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@CustomerId", CustomerId);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerHomedetail(int CustomerId)
        {
            Database_Operations obj_db = new Database_Operations("CustomerHomedetail", true);
            obj_db.AddParameter("@CustomerId", CustomerId);
            return (obj_db.GetDataSet());
        }

        public DataSet GetInvoiceList(int userid)
        {
            Database_Operations db = new Database_Operations("GetInvoiceList", true);
            db.AddParameter("@userid", userid);
            return db.GetDataSet();
        }

        public DataTable Get_ListServiceRequest(int page_number, int page_size, string filter, int userid)
        {
            Database_Operations db_obj = new Database_Operations("ListServiceRequest", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@userid", userid);
            return db_obj.GetDataTable();
        }

        public DataSet EditServiceRequest(int id)
        {
            Database_Operations db_obj = new Database_Operations("EditServiceRequest", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public int DeleteRequest(int Id)
        {
            Database_Operations db_obj = new Database_Operations("DeleteRequest", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int Insert_UpdateServiceRequest(int Id, DateTime? Qdate, int? Cust_id, int? userid, DataTable dtService,
            DataTable dtDocserivce, string remark, int TemplateId, DataTable dtdocmain, string Applicant)
        {
            Database_Operations db_obj = new Database_Operations("Insert_UpdateServiceRequest", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Qdate", Qdate);
            db_obj.AddParameter("@CustomerId", Cust_id);
            db_obj.AddParameter("@dtService", dtService);
            db_obj.AddParameter("@dtDocserivce", dtDocserivce);
            db_obj.AddParameter("@remark", remark);
            db_obj.AddParameter("@userid", userid);
            db_obj.AddParameter("@TemplateId", TemplateId);
            db_obj.AddParameter("@dtdocmain", dtdocmain);
            db_obj.AddParameter("@Applicant", Applicant);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet listRequestHistry(int Rid)
        {
            Database_Operations db_obj = new Database_Operations("listRequestHistry", true);
            db_obj.AddParameter("@Rid", Rid);
            return db_obj.GetDataSet();
        }


        #endregion

        #region CustomerRequest

        public DataSet EditRequestForInvoice(int Requestid, int Language, int SerPriceWithTax)
        {
            Database_Operations db_obj = new Database_Operations("EditRequestForInvoice", true);
            db_obj.AddParameter("@Id", Requestid);
            db_obj.AddParameter("@Language", Language);
            db_obj.AddParameter("@SerPriceWithTax", SerPriceWithTax);
            return (db_obj.GetDataSet());
        }

        public DataTable Get_ListCustomerRequest(int page_number, int page_size, string filter)
        {
            Database_Operations db_obj = new Database_Operations("Get_ListCustomerRequest", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            return db_obj.GetDataTable();
        }

        public int RejectCustomerRequest(int Id, string RejectRemark)
        {
            Database_Operations db_obj = new Database_Operations("RejectCustomerRequest", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@RejectRemark", RejectRemark);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int ProcessedCustomerRequest(int Id, int InvoiceId, int UserId, int Inserttype)
        {
            Database_Operations db_obj = new Database_Operations("ProcessedCustomerRequest", true);
            db_obj.AddParameter("@RequestId", Id);
            db_obj.AddParameter("@InvoiceId", InvoiceId);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddParameter("@Inserttype", Inserttype);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion
    }
}