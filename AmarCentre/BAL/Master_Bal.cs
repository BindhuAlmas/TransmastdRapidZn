using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace AmarCentre.BAL
{
    public class Master_Bal
    {
        #region Common

        public DataSet drpforcustomer()
        {
            Database_Operations db_obj = new Database_Operations("drpforcustomer", true);
            return (db_obj.GetDataSet());
        }
        public DataSet drpforEmployee()
        {
            Database_Operations db_obj = new Database_Operations("drpforEmployee", true);
            return (db_obj.GetDataSet());
        }
        public DataTable DrpCustCategory()
        {
            Database_Operations databaseOperations = new Database_Operations("DrpCustCategory", true);
            return (databaseOperations.GetDataTable());
        }

        public DataTable DrpLeadSource()
        {
            Database_Operations databaseOperations = new Database_Operations("DrpLeadSource", true);
            return (databaseOperations.GetDataTable());
        }

        public DataTable DrpPriority()
        {
            Database_Operations databaseOperations = new Database_Operations("DrpPriority", true);
            return (databaseOperations.GetDataTable());
        }

        public DataTable DrpStatus()
        {
            Database_Operations databaseOperations = new Database_Operations("DrpStatus", true);
            return (databaseOperations.GetDataTable());
        }

        public DataTable DrpJurisdiction()
        {
            Database_Operations databaseOperations = new Database_Operations("DrpJurisdiction", true);
            return (databaseOperations.GetDataTable());
        }

        public DataTable DrpActivity()
        {
            Database_Operations databaseOperations = new Database_Operations("DrpActivity", true);
            return (databaseOperations.GetDataTable());
        }

        public DataTable DrpLeadEmployee(int userId)
        {
            Database_Operations db_obj = new Database_Operations("DrpLeadEmployee", true);
            db_obj.AddParameter("@userId", userId);
            return db_obj.GetDataTable();
        }

        public DataTable DrpLeadDepartment()
        {
            Database_Operations db_obj = new Database_Operations("DrpLeadDepartment", true);
            return db_obj.GetDataTable();
        }

        public DataSet DrpQuestion123(int? LeadDepartmentId)
        {
            Database_Operations db_obj = new Database_Operations("DrpQuestion123", true);
            db_obj.AddParameter("@LeadDepartmentId", LeadDepartmentId);
            return db_obj.GetDataSet();
        }

        public DataTable DocExpiryMail(int Id, int DocType,int UserId)
        {
            Database_Operations db_obj = new Database_Operations("DocExpiryMail", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@DocType", DocType);
            db_obj.AddParameter("@UserId", UserId);
            return (db_obj.GetDataTable());
        }

        public DataSet DrpfillForServicePage() //1-category,2-department 3-general
        {
            Database_Operations db_obj = new Database_Operations("DrpfillForServicePage", true);
            return db_obj.GetDataSet();
        }

        public DataTable DrpfillCompany()
        {
            Database_Operations db_obj = new Database_Operations("DrpfillCompany", true);
            return db_obj.GetDataTable();
        }

        public DataTable DrpTemplate()
        {
            Database_Operations databaseOperations = new Database_Operations("DrpTemplate", true);
            return (databaseOperations.GetDataTable());
        }

        public DataTable Drp_Design()
        {
            Database_Operations db_obj = new Database_Operations("Drp_Design", true);
            return (db_obj.GetDataTable());
        }

        public DataSet Drp_GetEmpDrp()
        {
            Database_Operations db_obj = new Database_Operations("GetEmpDrp", true);
            return (db_obj.GetDataSet());
        }

        public DataTable Drp_Reporting(int PresentEmpId,int TransId=0)
        {
            Database_Operations db_obj = new Database_Operations("Drp_Reporting", true);
            db_obj.AddParameter("@PresentEmpId", PresentEmpId);
            db_obj.AddParameter("@TransId", TransId);
            return (db_obj.GetDataTable());
        }

        public DataTable Drp_Employee()
        {
            Database_Operations db_obj = new Database_Operations("drp_Employee", true);
            return db_obj.GetDataTable();
        }

        public DataTable Drp_ServiceCategory()
        {
            Database_Operations db_obj = new Database_Operations("List_ServiceCategory_Drp", true);
            return db_obj.GetDataTable();
        }

        public DataTable Drp_Expense()
        {
            Database_Operations db_obj = new Database_Operations("List_Expense_Drp", true);
            return db_obj.GetDataTable();
        }
        public DataTable Drp_Income()
        {
            Database_Operations db_obj = new Database_Operations("Drp_Income", true);
            return (db_obj.GetDataTable());
        }
        public DataTable Drp_Vendor()
        {
            Database_Operations db_obj = new Database_Operations("List_Vendor_Drp", true);
            return db_obj.GetDataTable();
        }

        public DataTable Drp_PaymentMode()
        {
            Database_Operations db_obj = new Database_Operations("List_PaymentMode_Drp", true);
            return db_obj.GetDataTable();
        }

        public DataTable Drp_Account()
        {
            Database_Operations db_obj = new Database_Operations("List_Account_Drp", true);
            return db_obj.GetDataTable();
        }

        public DataTable Drp_PettyCashAccount()
        {
            Database_Operations db_obj = new Database_Operations("List_PettyCashAccount_Drp", true);
            return db_obj.GetDataTable();
        }

        public DataTable DrpBankAccount()
        {
            Database_Operations db_obj = new Database_Operations("List_BankAccount_Drp", true);
            return db_obj.GetDataTable();
        }

        public DataTable Drp_PaymentMode_WithoutCredit()
        {
            Database_Operations db_obj = new Database_Operations("List_PaymentMode_WithoutCredit_Drp", true);
            return db_obj.GetDataTable();
        }

        public DataTable Drp_Account_Filter_PayMode(int PayModeId)
        {
            Database_Operations db_obj = new Database_Operations("List_Account_Filter_PayMode_Drp", true);
            db_obj.AddParameter("@PayModeId", PayModeId);
            return db_obj.GetDataTable();
        }

        public DataTable Drp_Department()
        {
            Database_Operations db_obj = new Database_Operations("List_Department_Drp", true);
            return db_obj.GetDataTable();
        }

        public DataTable Drp_SerSubCategory_Filetr_SerCategory(int SerCategoryId)
        {
            Database_Operations db_obj = new Database_Operations("List_SerSubCategory_Filter_SerCategory_Drp", true);
            db_obj.AddParameter("@SerCategoryId", SerCategoryId);
            return db_obj.GetDataTable();
        }
        public DataTable DrpServicebyDepartment(int? DepartmentId)
        {
            Database_Operations db_obj = new Database_Operations("DrpServicebyDepartment", true);
            db_obj.AddParameter("@DepartmentId", DepartmentId);
            return db_obj.GetDataTable();
        }
        #endregion

        #region home

        public DataTable getPLeadHome(DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db = new Database_Operations("getPLeadHome", true);
            db.AddParameter("@Fromdate", Fromdate);
            db.AddParameter("@Todate", Todate);
            return db.GetDataTable();
        }

        public DataTable getCLeadHome(DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db = new Database_Operations("getCLeadHome", true);
            db.AddParameter("@Fromdate", Fromdate);
            db.AddParameter("@Todate", Todate);
            return db.GetDataTable();
        }

        public DataTable LeadEscalationMail()
        {
            Database_Operations db_obj = new Database_Operations("LeadEscalationMail", true);
            return db_obj.GetDataTable();
        }

        public DataSet getCRMDashboard(DateTime? Fromdate, DateTime? Todate, int? SourceId, int? EmployeeId,
            int? SegmentId, int? priorityId, int? StatusId,
            string Activity, int userId)
        {
            Database_Operations db_obj = new Database_Operations("getCRMDashboard", true);
            db_obj.AddParameter("@Fromdate", Fromdate);
            db_obj.AddParameter("@Todate", Todate);
            db_obj.AddParameter("@SourceId", SourceId);
            db_obj.AddParameter("@EmployeeId", EmployeeId);
            db_obj.AddParameter("@SegmentId", SegmentId);
            db_obj.AddParameter("@priorityId", priorityId);
            db_obj.AddParameter("@StatusId", StatusId);
            db_obj.AddParameter("@Activity", Activity);
            db_obj.AddParameter("@userId", userId);
            return db_obj.GetDataSet();
        }

        public DataTable GetupcomingFollowuplist(DateTime? Fromdate, DateTime? Todate)
        {
            Database_Operations db_obj = new Database_Operations("GetupcomingFollowuplist", true);
            db_obj.AddParameter("@Fromdate", Fromdate);
            db_obj.AddParameter("@Todate", Todate);
            return db_obj.GetDataTable();
        }

        public DataSet GetCustomerDashboard(int CustomerId)
        {
            Database_Operations db = new Database_Operations("GetCustomerDashboard", true);
            db.AddParameter("@CustomerId", CustomerId);
            return db.GetDataSet();
        }

        public DataTable Get_CompaintList()
        {
            Database_Operations db = new Database_Operations("List_ComplaintNewHome", true);
            return db.GetDataTable();
        }

        public DataTable Get_newsList()
        {
            Database_Operations db = new Database_Operations("Get_newsList", true);
            return db.GetDataTable();
        }

        public DataTable GetSCChart()
        {
            Database_Operations db = new Database_Operations("GetSCChart", true);
            return db.GetDataTable();
        }

        public DataSet GetMonthlyProfitChart()
        {
            Database_Operations db = new Database_Operations("GetMonthlyProfit", true);
            return db.GetDataSet();
        }

        public DataTable DocumentMailChart()
        {
            Database_Operations db = new Database_Operations("DocumentMailChart", true);
            return db.GetDataTable();
        }
        public DataSet GetTopUp_Balance()
        {
            Database_Operations db = new Database_Operations("GetTopUp_Balance", true);
            return db.GetDataSet();
        }
        public DataTable GetReceivableChart()
        {
            Database_Operations db = new Database_Operations("GetRecChart", true);
            return db.GetDataTable();
        }
        public DataSet TopEmpolyeeService()
        {
            Database_Operations db = new Database_Operations("TopEmpolyeeService", true);
            return db.GetDataSet();
        }
        public DataSet GetSummary()
        {
            Database_Operations db = new Database_Operations("GetSummary", true);
            return db.GetDataSet();
        }

        public DataSet GetLoanSummaryhome()
        {
            Database_Operations db = new Database_Operations("GetLoanSummaryhome", true);
            return db.GetDataSet();
        }

        public DataSet DeadlineExcel(DateTime? Fromdate, DateTime? Todate, int? C_id)
        {
            Database_Operations db_obj = new Database_Operations("DeadlineExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@C_id", C_id);
            return db_obj.GetDataSet();
        }

        public DataSet CustomerDocumentExcel(DateTime? Fromdate, DateTime? Todate, int? C_id, int? SponserId)
        {
            Database_Operations db_obj = new Database_Operations("CustomerDocumentExcel", true);
            db_obj.AddParameter("@FromDate", Fromdate);
            db_obj.AddParameter("@ToDate", Todate);
            db_obj.AddParameter("@C_id", C_id);
            db_obj.AddParameter("@SponserId", SponserId);
            return db_obj.GetDataSet();
        }

        #endregion

        #region Menu Page

        //Get Main Menu
        public DataTable Get_Main_Menu()
        {
            Database_Operations db_obj = new Database_Operations("Get_Main_Menu_Page", true);
            return (db_obj.GetDataTable());
        }
        //drp_mainmenu  
        public DataTable drp_Get_Main_Menu()
        {
            Database_Operations db_obj = new Database_Operations("drp_Get_Main_Menu", true);
            return (db_obj.GetDataTable());
        }

        //Get Sub Menu
        public DataTable Get_Sub_Menu(int MainMenuId)
        {
            Database_Operations db_obj = new Database_Operations("Get_Sub_Menu_Page", true);
            db_obj.AddParameter("@MainMenuId", MainMenuId);
            return (db_obj.GetDataTable());
        }

        //Save Main Menu
        public int Insert_Update_Main_Menu(int MMId, string MMName, int DisplayOrder, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Main_Menu", true);
            db_obj.AddParameter("@Id", MMId);
            db_obj.AddParameter("@Name", MMName);
            db_obj.AddParameter("@Order_By", DisplayOrder);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        //Delete Main Menu
        public int Delete_Main_Menu(int MMId, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Main_Menu", true);
            db_obj.AddParameter("@Id", MMId);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        //Save Sub Menu
        public int Insert_Update_Sub_Menu(int SMId, int MMId, string SMName, string SMDest, int DisplayOrder, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Sub_Menu", true);
            db_obj.AddParameter("@Id", SMId);
            db_obj.AddParameter("@MainMenuId", MMId);
            db_obj.AddParameter("@Name", SMName);
            db_obj.AddParameter("@Destination", SMDest);
            db_obj.AddParameter("@Order_By", DisplayOrder);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        //Delete Sub Menu
        public int Delete_Sub_Menu(int SMId, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Sub_Menu", true);
            db_obj.AddParameter("@Id", SMId);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        //Edit Sub Menu
        public DataTable Edit_Sub_Menu(int SubMenuId)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Sub_Menu_Page", true);
            db_obj.AddParameter("@Id", SubMenuId);
            return (db_obj.GetDataTable());
        }

        #endregion
        
        #region Designation

        public DataTable Get_List_Designation(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Designation", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public DataTable Get_List_Designation_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_Designation_Excel", true);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_Designation(int id, string name, string descr, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Designation", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@Description", descr);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet Edit_Designation(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Designation", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int Delete_Designation(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Designation", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region Salary Type

        //Get List of Data
        public DataTable Get_List_Salary_Type(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Salary_Type", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_Salary_Type(int id, string name, string Description, int type, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Salary_Type", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@Description", Description);
            db_obj.AddParameter("@Type", type);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet Edit_Salary_Type(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Salary_Type", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int Delete_Salary_Type(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Salary_Type", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_List_Salary_Type_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_Salary_Type_Excel", true);
            return db_obj.GetDataTable();
        }

        #endregion

        #region Expense

        public DataTable fill_exp_Type()
        {
            Database_Operations db_obj = new Database_Operations("fill_exp_Type", true);
            return (db_obj.GetDataTable());
        }

        public DataTable Get_List_Expense(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Expense", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_Expense(int id, string name, string Description, int exptype, decimal tax,int TaxAppliForCommision, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Expense", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@Description", Description);
            db_obj.AddParameter("@exptype", exptype);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddParameter("@tax", tax);
            db_obj.AddParameter("@TaxAppliForCommision", TaxAppliForCommision);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet Edit_Expense(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Expense", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int Delete_Expense(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Expense", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_List_Expense_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_Expense_Excel", true);
            return db_obj.GetDataTable();
        }


        #endregion

        #region Account Type

        //Get List of Data
        public DataTable Get_List_Account_Type(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Account_Type", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_Account_Type(int id, string type, string desc, int? oredr, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Account_Type", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@type", type);
            db_obj.AddParameter("@desc", desc);
            db_obj.AddParameter("@oredr", oredr);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet Edit_Account_Type(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Account_Type", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int Delete_Account_Type(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Account_Type", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_List_Account_Type_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_Account_Type_Excel", true);
            return db_obj.GetDataTable();
        }

        #endregion

        #region Vendor

        //Get List of Data
        public DataTable Get_List_Vendor(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Vendor", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_Vendor(int id, string name, string address, string mobno, string mail, int UserId, string TRn,
            decimal VendorCommission,int IsAlsoCustomer)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Vendor", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@address", address);
            db_obj.AddParameter("@Mobile", mobno);
            db_obj.AddParameter("@mail", mail);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddParameter("@TRn", TRn);
            db_obj.AddParameter("@VendorCommission", VendorCommission);
            db_obj.AddParameter("@IsAlsoCustomer", IsAlsoCustomer);

            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet Edit_Vendor(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Vendor", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public DataTable GetVendorOB(int id)
        {
            Database_Operations db_obj = new Database_Operations("GetVendorOB", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataTable());
        }

        public int ClearVendorOB(int id, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("ClearVendorOB", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int Delete_Vendor(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Vendor", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_List_Vendor_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_Vendor_Excel", true);
            return db_obj.GetDataTable();
        }

        public int Update_OB_Vendor(int id, int OBType, decimal OB, DateTime? obdate, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("update_Vendor_OB", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@OBType", OBType);
            db_obj.AddParameter("@OB", OB);
            db_obj.AddParameter("@obdate", obdate);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        //public DataSet list_VendorHistry(DateTime? from, DateTime? to, int Qid, int page_number, int page_size)
        //{
        //    Database_Operations db_obj = new Database_Operations("List_VendorHistry", true);
        //    db_obj.AddParameter("@From", from);
        //    db_obj.AddParameter("@To", to);
        //    db_obj.AddParameter("@employee_id", Qid);
        //    db_obj.AddParameter("@page_number", page_number);
        //    db_obj.AddParameter("@count", page_size);
        //    return db_obj.GetDataSet();
        //}

        #endregion

        #region customer

        public DataTable CustomerMail(int CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("CustomerMail", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            return db_obj.GetDataTable();
        }
        public DataSet Get_MenuListCustomer(int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Get_MenuListCustomer", true);
            db_obj.AddParameter("@UserId", UserId);
            return db_obj.GetDataSet();
        }

        public int Update_CustomerMenu(int id, DataTable dt_submenu)
        {
            Database_Operations db_obj = new Database_Operations("Update_CustomerMenu", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@dt_submenu", dt_submenu);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable fill_drp_CustomerStaff(int CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("drp_CustomerStaff", true);
            db_obj.AddParameter("@CustId", CustomerId);
            return (db_obj.GetDataTable());
        }

        public DataSet list_CustMailHistry(DateTime? from, DateTime? to, int Qid, int? DocumentId, string CustStaff, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("List_CustomerMailHistory", true);
            db_obj.AddParameter("@From", from);
            db_obj.AddParameter("@To", to);
            db_obj.AddParameter("@Qid", Qid);
            db_obj.AddParameter("@DocumentId", DocumentId);
            db_obj.AddParameter("@CustStaff", CustStaff);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            return db_obj.GetDataSet();
        }

        public void DeleteCustomerDocument(int id)
        {
            Database_Operations db_obj = new Database_Operations("DeleteCustomerDocument", true);
            db_obj.AddParameter("@Id", id);
            db_obj.ExecuteQuery();
        }

        public void DeleteCustomerStaffDocument(int id)
        {
            Database_Operations db_obj = new Database_Operations("DeleteCustomerStaffDocument", true);
            db_obj.AddParameter("@Id", id);
            db_obj.ExecuteQuery();
        }

        public DataTable Get_List_Customer(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Customer", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public DataTable List_Customer_Excel()
        {
            Database_Operations db = new Database_Operations("List_Customer_Excel", true);
            return db.GetDataTable();
        }

        public DataSet Edit_Customer(int id)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Customer", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public DataTable GetCustomerOB(int id)
        {
            Database_Operations db_obj = new Database_Operations("GetCustomerOB", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataTable());
        }

        public DataSet Edit_CustomerDocs(int id)
        {
            Database_Operations db_obj = new Database_Operations("Edit_CustomerDocs", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }
        public int InsertUpdateCCategory(int Id, string Name,  int UserId)
        {
            Database_Operations databaseOperations = new Database_Operations("InsertUpdateCCategory", true);
            databaseOperations.AddParameter("@Id", Id);
            databaseOperations.AddParameter("@Name", Name);
            databaseOperations.AddParameter("@UserId", UserId);
            databaseOperations.AddOutputParameter("@Result");
            databaseOperations.ExecuteQuery();
            return Convert.ToInt32(databaseOperations.SqlCmd.Parameters["@Result"].Value.ToString());
        }
        public int Insert_Update_Customer(int id, string name, string address, string Mobile_num, string phone_num, string email,
            string remark, int user_id, string TRN, int IsCredit,decimal? CreditAmount,int IsCommissionApplicable, string ArabicName,
            DataTable dt_agnt,string ContactPerson,string mohreno,string licenseno,int? SponserId,int IsTypingCenter,int? EmirateId,
            int? CCategory,string CCMail,string WhatsappNo,string UserName,string Passwords,
            int? CompanyGroupId,int IsMainCompany)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Customer", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@IsMainCompany", IsMainCompany);
            db_obj.AddParameter("@CompanyGroupId", CompanyGroupId);
            db_obj.AddParameter("@UserName", UserName);
            db_obj.AddParameter("@Passwords", Passwords);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@WhatsappNo", WhatsappNo);
            db_obj.AddParameter("@address", address);
            db_obj.AddParameter("@Mobile_num", Mobile_num);
            db_obj.AddParameter("@phone_num", phone_num);
            db_obj.AddParameter("@email", email);
            db_obj.AddParameter("@remark", remark);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddParameter("@TRN", TRN);
            db_obj.AddParameter("@IsCredit", IsCredit);
            db_obj.AddParameter("@CreditAmount", CreditAmount);
            db_obj.AddParameter("@IsCommissionApplicable", IsCommissionApplicable);
            db_obj.AddParameter("@ArabicName", ArabicName, 1);
            db_obj.AddParameter("@ContactPerson", ContactPerson);
            db_obj.AddParameter("@mohreno", mohreno);
            db_obj.AddParameter("@licenseno", licenseno);
            db_obj.AddParameter("@dt_agnt", dt_agnt);
            db_obj.AddParameter("@SponserId", SponserId);
            db_obj.AddParameter("@IsTypingCenter", IsTypingCenter);
            db_obj.AddParameter("@EmirateId", EmirateId);
            db_obj.AddParameter("@CCategory", CCategory);
            db_obj.AddParameter("@CCMail", CCMail);

            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int UpdateCustomerCredit(int id, decimal Amount ,int user_id)
        {
            Database_Operations db_obj = new Database_Operations("UpdateCustomerCredit", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Amount", Amount);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int Delete_Customer(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Customer", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable fill_drp_DocType()
        {
            Database_Operations db_obj = new Database_Operations("drp_DocType", true);
            return (db_obj.GetDataTable());
        }

        public DataTable fill_drp_Agent()
        {
            Database_Operations db_obj = new Database_Operations("drp_Agent", true);
            return (db_obj.GetDataTable());
        }

        public DataTable fillsponser()
        {
            Database_Operations db_obj = new Database_Operations("drpSponser", true);
            return (db_obj.GetDataTable());
        }

        public DataTable fillEmirate()
        {
            Database_Operations db_obj = new Database_Operations("drpEmirate", true);
            return (db_obj.GetDataTable());
        }

        public int Update_OB_Customer(int id,int OBType, decimal OB, DateTime? obdate, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("Update_Customer_OB", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@OBType", OBType);
            db_obj.AddParameter("@OB", OB);
            db_obj.AddParameter("@obdate", obdate);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int ClearCustomerOB(int id,  int user_id)
        {
            Database_Operations db_obj = new Database_Operations("ClearCustomerOB", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }
               
        public DataSet ListCustomerDocument(int Pagenumber, int Count, string filter, int CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("ListCustomerDocument", true);
            db_obj.AddParameter("@page_number", Pagenumber);
            db_obj.AddParameter("@count", Count);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@CustomerId", CustomerId);
            return db_obj.GetDataSet();
        }

        public DataSet ListCustomerStaffDocument(int Pagenumber, int Count, string filter, int CustomerId)
        {
            Database_Operations db_obj = new Database_Operations("ListCustomerStaffDocument", true);
            db_obj.AddParameter("@page_number", Pagenumber);
            db_obj.AddParameter("@count", Count);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@CustomerId", CustomerId);
            return db_obj.GetDataSet();
        }

        public int Update_CustomerDocument(int Id, int Customerid, int DocumentId, string DocNumber,
            DateTime? from_date, DateTime? Expiry_date, string Document_name, string Remark, int? ValidityYear, int user_id, string filename, string filenamesave)
        {
            Database_Operations db_obj = new Database_Operations("Update_CustomerDocument", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Customerid", Customerid);
            db_obj.AddParameter("@DocumentId", DocumentId);
            db_obj.AddParameter("@DocNumber", DocNumber);
            db_obj.AddParameter("@from_date", from_date);
            db_obj.AddParameter("@Expiry_date", Expiry_date);
            db_obj.AddParameter("@Document_name", Document_name);
            db_obj.AddParameter("@Remark", Remark);
            db_obj.AddParameter("@ValidityYear", ValidityYear);
            db_obj.AddParameter("@filename", filename);
            db_obj.AddParameter("@filenamesave", filenamesave);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int Update_CustomerDocumentStaff(int CSId, int Customerid, string Staffname, string StaffMob, int DocumentId, string DocNumber,
            DateTime? from_date, DateTime? Expiry_date, string Document_name, string Remark, int? ValidityYear, int user_id, string filename,string filenamesave)
        {
            Database_Operations db_obj = new Database_Operations("Update_CustomerDocumentStaff", true);
            db_obj.AddParameter("@CSId", CSId);
            db_obj.AddParameter("@Customerid", Customerid);
            db_obj.AddParameter("@Staffname", Staffname);
            db_obj.AddParameter("@StaffMob", StaffMob);
            db_obj.AddParameter("@DocumentId", DocumentId);
            db_obj.AddParameter("@DocNumber", DocNumber);
            db_obj.AddParameter("@from_date", from_date);
            db_obj.AddParameter("@Expiry_date", Expiry_date);
            db_obj.AddParameter("@Document_name", Document_name);
            db_obj.AddParameter("@Remark", Remark);
            db_obj.AddParameter("@ValidityYear", ValidityYear);
            db_obj.AddParameter("@filename", filename);
            db_obj.AddParameter("@filenamesave", filenamesave);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int Update_CustomerDocument_StaffbyFile( int Customerid,DataTable ContentTable)
        {
            Database_Operations db_obj = new Database_Operations("Update_CustomerDocument_StaffbyFile", true);
            db_obj.AddParameter("@Customerid", Customerid);
            db_obj.AddParameter("@ContentTable", ContentTable);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int Update_CustCredtnl(int id, string imcnname, string imuid, string impass, string imbnkuser, string imbnkpss, string imbnkpin, string imrsapin,
            string dmcnname, string dmuser, string dmpass, string dmadmuser, string dmadpass, string dmemuser, string dmempass,
             string tucnname, string tuspuser, string tupass, string tuuser, string tuuspass, string tueid, string tumob,
            string nsuser, string nspass, string nsemail, string nsmobile, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("Update_CustCrediental", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@imcnname", imcnname);
            db_obj.AddParameter("@imuid", imuid);
            db_obj.AddParameter("@impass", impass);
            db_obj.AddParameter("@imbnkuser", imbnkuser);
            db_obj.AddParameter("@imbnkpss", imbnkpss);
            db_obj.AddParameter("@imbnkpin", imbnkpin);
            db_obj.AddParameter("@imrsapin", imrsapin);


            db_obj.AddParameter("@dmcnname", dmcnname);
            db_obj.AddParameter("@dmuser", dmuser);
            db_obj.AddParameter("@dmpass", dmpass);
            db_obj.AddParameter("@dmadmuser", dmadmuser);
            db_obj.AddParameter("@dmadpass", dmadpass);
            db_obj.AddParameter("@dmemuser", dmemuser);
            db_obj.AddParameter("@dmempass", dmempass);

            db_obj.AddParameter("@tucnname", tucnname);
            db_obj.AddParameter("@tuspuser", tuspuser);
            db_obj.AddParameter("@tupass", tupass);
            db_obj.AddParameter("@tuuser", tuuser);
            db_obj.AddParameter("@tuuspass", tuuspass);
            db_obj.AddParameter("@tueid", tueid);
            db_obj.AddParameter("@tumob", tumob);

            db_obj.AddParameter("@nsuser", nsuser);
            db_obj.AddParameter("@nspass", nspass);
            db_obj.AddParameter("@nsemail", nsemail);
            db_obj.AddParameter("@nsmobile", nsmobile);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet list_CustCreditHistry(DateTime? from, DateTime? to, int Qid, int page_number, int page_size)
        {
            Database_Operations db_obj = new Database_Operations("List_CreditHistory", true);
            db_obj.AddParameter("@From", from);
            db_obj.AddParameter("@To", to);
            db_obj.AddParameter("@Qid", Qid);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            return db_obj.GetDataSet();
        }

        //public DataSet list_CustStatHistry(DateTime? from, DateTime? to, int Qid, int page_number, int page_size)
        //{
        //    Database_Operations db_obj = new Database_Operations("List_Customer_SOA", true);
        //    db_obj.AddParameter("@fromdate", from);
        //    db_obj.AddParameter("@todate", to);
        //    db_obj.AddParameter("@Cust_id", Qid);
        //    db_obj.AddParameter("@page_number", page_number);
        //    db_obj.AddParameter("@count", page_size);
        //    return db_obj.GetDataSet();
        //}

        public DataTable Get_List_Customer_ServiceDetail(int Id,string filter)
        {
            Database_Operations db_obj = new Database_Operations("List_Customer_ServiceDetail", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@filter", filter);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_CustomerServiceDetail(int Id, DataTable dt_serDetail, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_CustomerServiceDetail", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@dt_serDetail", dt_serDetail);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable GetCustomerServiceExpires(int CustomerId,int page_number, int page_size, string filter)
        {
            Database_Operations db_obj = new Database_Operations("GetCustomerServiceExpires", true);
            db_obj.AddParameter("@CustomerId", CustomerId);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            return db_obj.GetDataTable();
        }

        #endregion

        #region Sequence

        //Get List of Data
        public DataTable Get_List_Sequence(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Sequence", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        //Get List of Data
        public DataTable Get_List_Sequence_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_Sequence_Excel", true);
            return db_obj.GetDataTable();
        }

        //Particular Team
        public DataTable Edit_Sequence(int Id)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Sequence", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataTable());
        }

        //Delete Team
        public int Delete_Sequence(int Id, int User_Id)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Sequence", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@User_Id", User_Id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        //Save Team
        public int Insert_Update_Sequence(int Id, string category, string prefix, string seperator, int current_no, int increment, int? menu_id,
            int? MiniDig, int User_Id)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Sequence", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@category", category);
            db_obj.AddParameter("@prefix", prefix);
            db_obj.AddParameter("@seperator", seperator);
            db_obj.AddParameter("@current_no", current_no);
            db_obj.AddParameter("@increment", increment);
            db_obj.AddParameter("@menu_id", menu_id);
            db_obj.AddParameter("@MiniDig", MiniDig);
            db_obj.AddParameter("@User_Id", User_Id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_Sub_Menu()
        {
            Database_Operations db_obj = new Database_Operations("drp_sub_menu", true);
            return (db_obj.GetDataTable());
        }

        #endregion

        #region Employee

        //Get List of Data
        public DataTable Get_List_Employee(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Employee", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        //Particular User
        public DataSet Edit_Employee(int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Employee", true);
            db_obj.AddParameter("@Id", UserId);
            return (db_obj.GetDataSet());
        }

        public int Insert_Update_Employee(int Id, string Code, string Name, string UserName, string UserPwd,
             string AddressLine, string MobileNum, string Email, string photo, string photo_save,
             string phnnum, int DesignationId, int? ReportingId, int IncentiveApplicable,
             int Language, DataTable dt_pettyCashAccount, DataTable dtBankAccount, int User_Id, int? DefaultAccont,
             int IsEnable, DataTable dtLoanAccount,DataTable dtdepartment)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Employee", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Code", Code);
            db_obj.AddParameter("@Name", Name);
            db_obj.AddParameter("@Username", UserName);
            db_obj.AddParameter("@Password", UserPwd);
            db_obj.AddParameter("@AddressLine", AddressLine);
            db_obj.AddParameter("@MobileNum", MobileNum);
            db_obj.AddParameter("@Email", Email);
            db_obj.AddParameter("@Photo", photo);
            db_obj.AddParameter("@Photo_save", photo_save);
            db_obj.AddParameter("@Phone_number", phnnum);
            db_obj.AddParameter("@DesignationId", DesignationId);
            db_obj.AddParameter("@ReportingId", ReportingId);
            db_obj.AddParameter("@IncentiveApplicable", IncentiveApplicable);
            db_obj.AddParameter("@Language", Language);
            db_obj.AddParameter("@dt_pettyCashAccount", dt_pettyCashAccount);
            db_obj.AddParameter("@dtBankAccount", dtBankAccount);
            db_obj.AddParameter("@User_Id", User_Id);
            db_obj.AddParameter("@DefaultAccont", DefaultAccont);
            db_obj.AddParameter("@IsEnable", IsEnable);
            db_obj.AddParameter("@dtLoanAccount", dtLoanAccount);
            db_obj.AddParameter("@dtdepartment", dtdepartment);

            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }
        public DataTable DrpLoanAccount()
        {
            Database_Operations db_obj = new Database_Operations("List_LoanAccount_Drp", true);
            return db_obj.GetDataTable();
        }
        public int Delete_Employee(int id, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Employee", true);
            db_obj.AddParameter("@id", id);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable List_Employee_Excel(int UserId)
        {
            Database_Operations db_obj = new Database_Operations("List_Employee_Excel", true);
            db_obj.AddParameter("@UserId", UserId);
            return db_obj.GetDataTable();
        }

        public DataTable EmpGetOtherDetail(int UserId)
        {
            Database_Operations db_obj = new Database_Operations("EmpGetOtherDetail", true);
            db_obj.AddParameter("@Id", UserId);
            return (db_obj.GetDataTable());
        }
        public int Update_EmployeeOtherDetail(int id,DateTime? DOJ,DateTime? DOB,string Profsn,int? Prf_id,
            string nation,int? nationId, int? probstat,int? contrct, int? gender,string mol)
        {
            Database_Operations db_obj = new Database_Operations("Update_EmployeeOtherDetail", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@DOJ", DOJ);
            db_obj.AddParameter("@DOB", DOB);
            db_obj.AddParameter("@Profsn", Profsn);
            db_obj.AddParameter("@Prf_id", Prf_id);
            db_obj.AddParameter("@nation", nation);
            db_obj.AddParameter("@nationId", nationId);
            db_obj.AddParameter("@probstat", probstat);
            db_obj.AddParameter("@contrct", contrct);
            db_obj.AddParameter("@gender", gender);
            db_obj.AddParameter("@mol", mol);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }
        public int Update_OB_Employee(int id, int OBType, decimal OB, DateTime obdate, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("Update_Employee_OB", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@OBType", OBType);
            db_obj.AddParameter("@OpeningBalance", OB);
            db_obj.AddParameter("@Balance",OBType==2?OB*-1:OB);
            db_obj.AddParameter("@obdate", obdate);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int Update_Password(int User_Id,string OldPwd, string Pwd)
        {
            Database_Operations db_obj = new Database_Operations("Update_Password", true);
            db_obj.AddParameter("@Id", User_Id);
            db_obj.AddParameter("@Pwd", Pwd);
            db_obj.AddParameter("@OldPwd", OldPwd);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int Update_Profile(int Id, string Name,string MobileNumber,string PhoneNumber,
            string Email, string PhotoFile, string PhotoFileSave )
        {
            Database_Operations db_obj = new Database_Operations("Update_Employee_Pfl", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@MobileNumber", MobileNumber);
            db_obj.AddParameter("@Email", Email);
            db_obj.AddParameter("@Name", Name);
            db_obj.AddParameter("@Photo", PhotoFile);
            db_obj.AddParameter("@PhotoSave", PhotoFileSave);
            db_obj.AddParameter("@PhoneNumber", PhoneNumber);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int Update_EmpDoc(int id, DataTable dt_doc, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("Update_EmpDoc", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@dt_doc", dt_doc);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet Get_MenuList(int UserId,int LoginUserId)
        {
            Database_Operations db_obj = new Database_Operations("Get_List_Sub_Menu", true);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddParameter("@LoginUserId", LoginUserId);
            return db_obj.GetDataSet();
        }

        public int Update_EmpMenu(int id, DataTable dt_Action, DataTable dt_submenu)
        {
            Database_Operations db_obj = new Database_Operations("Update_EmpMenu", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@dt_Action", dt_Action);
            db_obj.AddParameter("@dt_submenu", dt_submenu);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        //Particular User
        public DataSet Edit_EmployeeAccount(int UserId, int Acctyp)
        {
            Database_Operations db_obj = new Database_Operations("Edit_EmployeeAccount", true);
            db_obj.AddParameter("@Id", UserId);
            db_obj.AddParameter("@Acctype", Acctyp);
            return (db_obj.GetDataSet());
        }

        public int Update_EmployeeAcc(int id, DataTable dtpetty, int? defPetty, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("Update_EmployeeAcc", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@dtpetty", dtpetty);
            db_obj.AddParameter("@defPetty", defPetty);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_List_EmployeeApplicableLeave(int Id)
        {
            Database_Operations db_obj = new Database_Operations("List_EmployeeApplicableLeave", true);
            db_obj.AddParameter("@Id", Id);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_EmployeeApplicableLeave(int Id, DataTable dt_ApplicableLeave, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_EmployeeApplicableLeave", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@dt_ApplicableLeave", dt_ApplicableLeave);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable GetEmpoyeeIncentive(int Id)
        {
            Database_Operations db_obj = new Database_Operations("GetEmpoyeeIncentive", true);
            db_obj.AddParameter("@Id", Id);
            return db_obj.GetDataTable();
        }

        public int InsertUpdateEmpoyeeIncentive(int Id, DataTable dt_serDetail, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("InsertUpdateEmpoyeeIncentive", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@dt_serDetail", dt_serDetail);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region Employee salary

        public DataTable Drp_Employee_sal()
        {
            Database_Operations db_obj = new Database_Operations("drp_Employee_Sal", true);
            return db_obj.GetDataTable();
        }

        public DataTable Drp_EmployeeSalaryEdit(int CurrentEmpId)
        {
            Database_Operations db_obj = new Database_Operations("Drp_EmployeeSalaryEdit", true);
            db_obj.AddParameter("@CurrentEmpId", CurrentEmpId);
            return db_obj.GetDataTable();
        }

        /*Fill List*/
        public DataTable Get_List_Employee_Salary(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Employee_Salary", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        /*Insert & Update Data*/
        public int Insert_Update_Employee_Salary(int Id, int EmployeeId, decimal Total_Salary, DataTable dt_terms, int UserId)
        {
            try
            {
                Database_Operations db_obj = new Database_Operations("Insert_Update_Employee_Salary", true);
                db_obj.AddParameter("@Id", Id);
                db_obj.AddParameter("@EmployeeId", EmployeeId);
                db_obj.AddParameter("@dtDetails", dt_terms);
                db_obj.AddParameter("@Total_Salary", Total_Salary);
                db_obj.AddParameter("@User_Id", UserId);
                db_obj.AddOutputParameter("@Result");
                db_obj.ExecuteQuery();
                return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
            }
            catch(Exception e)
            {

                return 0;
            }
        }

        public int Delete_Employee_Salary(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("delete_Employee_Salary", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        /*Particulat Data*/
        public DataSet Edit_Employee_Salary(int id)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Employee_Salary", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public DataTable List_Salary()
        {
            Database_Operations db_obj = new Database_Operations("Get_salaryList", true);
            return (db_obj.GetDataTable());
        }

        /*Get Detail in Excel*/
        public DataTable Get_List_Employee_Salary_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_Employee_Salary_Excel", true);
            return db_obj.GetDataTable();
        }

        #endregion

        #region Bank Account

        //Get List of Data
        public DataTable Get_List_Bank_Account(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Bank_Account", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_Bank_Account(int Id, string Dispname, string Prvdrname, int type, string dessc, int UserId, int IsConfirmNeed,
            decimal? CommPer, int IscommsnApp, int IscompnyEdhrhm,int IsNomad,int IsVatApplicable,string TRN)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Bank_Account", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@IsVatApplicable", IsVatApplicable);
            db_obj.AddParameter("@TRN", TRN);
            db_obj.AddParameter("@Dispname", Dispname);
            db_obj.AddParameter("@Prvdrname", Prvdrname);
            db_obj.AddParameter("@Type", type);
            db_obj.AddParameter("@dessc", dessc);
            db_obj.AddParameter("@IsConfirmNeed", IsConfirmNeed);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddParameter("@CommPer", CommPer);
            db_obj.AddParameter("@IscommsnApp", IscommsnApp);
            db_obj.AddParameter("@IscompnyEdhrhm", IscompnyEdhrhm);
            db_obj.AddParameter("@IsNomad", IsNomad);

            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Edit_Bank_Account(int Id)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Bank_Account", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataTable());
        }

        public int Delete_Bank_Account(int Id, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Bank_Account", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable fill_bank_Type()
        {
            Database_Operations db_obj = new Database_Operations("fill_bank_Type", true);
            return (db_obj.GetDataTable());
        }

        public DataTable Get_List_Bank_Account_Excel()
        {
            Database_Operations db = new Database_Operations("List_Bank_Account_Excel", true);
            return db.GetDataTable();
        }

        public int Update_OB_Bank_Account(int id, decimal OB, DateTime obdate, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("Update_Bank_Account_OB", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@OB", OB);
            db_obj.AddParameter("@obdate", obdate);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        //public DataTable Get_BankAvailAmt(int id)
        //{
        //    Database_Operations db_obj = new Database_Operations("Get_AvailBank", true);
        //    db_obj.AddParameter("@Id", id);
        //    return (db_obj.GetDataTable());
        //}

        #endregion

        #region Service

        public int Insert_Update_AgentServiceCommission(int Id, DataTable dt_serDetail, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_AgentServiceCommission", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@dt_serDetail", dt_serDetail);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_List_AgentServiceCommission(int Id, string filter)
        {
            Database_Operations db_obj = new Database_Operations("List_AgentServiceCommission", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@filter", filter);
            return db_obj.GetDataTable();
        }

        //Get List of Data
        public DataTable Get_List_Service(int page_number, int page_size, string filter, string column, string order,int? D_id)
        {
            Database_Operations db_obj = new Database_Operations("List_Service", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            db_obj.AddParameter("@D_id", D_id);
            return db_obj.GetDataTable();
        }

        public DataTable Get_List_Service_Excel(int? D_id)
        {
            Database_Operations db_obj = new Database_Operations("List_Service_Excel", true);
            db_obj.AddParameter("@D_id", D_id);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_Service(int id, string name, string nameArabic, int? ServiceCategoryId, int? ServiceSubCategoryId,
     int? DepartmentId, decimal Price, decimal Tax, int IncApp, int Enable, string Remark, DataTable dt_serDetail,
      int Validity, int? ValidityExpiresOn, int UserId, DataTable dtfollwdetail, int IsRefundable, int IsSetZeroPaidAmt, int? DocumentId,
      int IsSCNotRequired)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Service", true);
            db_obj.SqlCmd.Parameters.Add("@Id", SqlDbType.Int);
            db_obj.SqlCmd.Parameters[0].Value = id;
            db_obj.SqlCmd.Parameters.Add("@Name", SqlDbType.NVarChar);
            db_obj.SqlCmd.Parameters[1].Value = name;
            db_obj.SqlCmd.Parameters.Add("@NameArabic", SqlDbType.NVarChar);
            db_obj.SqlCmd.Parameters[2].Value = nameArabic;
            db_obj.SqlCmd.Parameters.Add("@ServiceCategoryId", SqlDbType.Int);
            db_obj.SqlCmd.Parameters[3].Value = ServiceCategoryId;
            db_obj.SqlCmd.Parameters.Add("@ServiceSubCategoryId", SqlDbType.Int);
            db_obj.SqlCmd.Parameters[4].Value = ServiceSubCategoryId;
            db_obj.SqlCmd.Parameters.Add("@DepartmentId", SqlDbType.Int);
            db_obj.SqlCmd.Parameters[5].Value = DepartmentId;
            db_obj.SqlCmd.Parameters.Add("@Price", SqlDbType.Decimal);
            db_obj.SqlCmd.Parameters[6].Value = Price;
            db_obj.SqlCmd.Parameters.Add("@Tax", SqlDbType.Decimal);
            db_obj.SqlCmd.Parameters[7].Value = Tax;
            db_obj.AddParameter("@IncApp", IncApp);
            db_obj.AddParameter("@Enable", Enable);
            db_obj.AddParameter("@Remark", Remark);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddParameter("@Validity", Validity);
            db_obj.AddParameter("@ValidityExpiresOn", ValidityExpiresOn);
            db_obj.AddParameter("@dt_serDetail", dt_serDetail);
            db_obj.AddParameter("@dtfollwdetail", dtfollwdetail);
            db_obj.AddParameter("@IsRefundable", IsRefundable);
            db_obj.AddParameter("@IsSetZeroPaidAmt", IsSetZeroPaidAmt);
            db_obj.AddParameter("@DocumentId", DocumentId);
            db_obj.AddParameter("@IsSCNotRequired", IsSCNotRequired);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet Edit_Service(int Id)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Service", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataSet());
        }

        public int Delete_Service(int Id, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Service", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region Petty Cash Account

        //Get List of Data
        public DataTable Get_List_PettyCashAccount(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_PettyCashAccount", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_PettyCashAccount(int id, string name, string desc, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_PettyCashAccount", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@desc", desc);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet Edit_PettyCashAccount(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("Edit_PettyCashAccount", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        //public DataTable Get_CashAvailAmt(int id)
        //{
        //    Database_Operations db_obj = new Database_Operations("Get_AvailCash", true);
        //    db_obj.AddParameter("@Id", id);
        //    return (db_obj.GetDataTable());
        //}

        public int Delete_PettyCashAccount(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_PettyCashAccount", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_List_PettyCashAccount_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_PettyCashAccount_Excel", true);
            return db_obj.GetDataTable();
        }

        public int Update_OB_PettyCashAccount(int id, decimal OB, DateTime obdate, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("Update_PettyCashAccount_OB", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@OB", OB);
            db_obj.AddParameter("@obdate", obdate);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region General Settings

        public int UpdateCompany_GeneralSettings(string compname, string compmail, string compphone)
        {
            Database_Operations db_obj = new Database_Operations("UpdateCompany_GeneralSettings", true);

            db_obj.AddParameter("@compname", compname);
            db_obj.AddParameter("@compmail", compmail);
            db_obj.AddParameter("@compphone", compphone);
            return (db_obj.ExecuteQuery());
        }
        public DataTable Edit_GeneralSettings()
        {
            Database_Operations db_obj = new Database_Operations("Edit_GeneralSettings", true);
            return (db_obj.GetDataTable());
        }

        public int insert_SoftwareConfiguration(int Id, int SerComWOPayment, int SerPriceWTax,
        int InvoiceFormat, int PrintTerms, int DepartmentRequired, int CategoryRequired, int SubCategoryRequired,  int UserId,
        int? defpaymode, int Isdisplydiscnt,  int CustomerSOAPdfFormat,
          int isaddremark, int enblCustinvoice,  int ReceiptFormat,
         int QuotationPrint, int CIPrint, int DebitorsReportFormat, int ReceiptVoucherFormat,
         int? VendorStmtFormat,int SalesOrderPrint, int IsTemplateView, int SCPredateDays,
          int IsQuotaionEditable, string DefaultQutotnRemark, string DefaultInvoiceRemark,
          int DepartmentInInvoiceVisible, int IsDisplaySCStatus, int? TransEditdaylimit,int IsDisableRoundOff, int SCView,
           int IsHideServiceAmtInSC, int? AgentCommission, int? ProfitExpenseType,int IsEditInvoiceCreator,
           int? CustomerDiscounttype,int? DefaultEmirate,int IncentivePercentage,int IsAddCreatedByInInvoicePrint,
           int IsSCViewDepartmentBase)
        {
            Database_Operations db_obj = new Database_Operations("insert_SoftwareConfiguration", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@IsSCViewDepartmentBase", IsSCViewDepartmentBase);
            db_obj.AddParameter("@IsAddCreatedByInInvoicePrint", IsAddCreatedByInInvoicePrint);
            db_obj.AddParameter("@IncentivePercentage", IncentivePercentage);
            db_obj.AddParameter("@SCView", SCView);
            db_obj.AddParameter("@IsHideServiceAmtInSC", IsHideServiceAmtInSC);
            db_obj.AddParameter("@IsDisableRoundOff", IsDisableRoundOff);
            db_obj.AddParameter("@TransEditdaylimit", TransEditdaylimit);
            db_obj.AddParameter("@IsDisplaySCStatus", IsDisplaySCStatus);
            db_obj.AddParameter("@DepartmentInInvoiceVisible", DepartmentInInvoiceVisible);
            db_obj.AddParameter("@DefaultInvoiceRemark", DefaultInvoiceRemark);
            db_obj.AddParameter("@IsQuotaionEditable", IsQuotaionEditable);
            db_obj.AddParameter("@DefaultQutotnRemark", DefaultQutotnRemark);
            db_obj.AddParameter("@SCPredateDays", SCPredateDays);
            db_obj.AddParameter("@IsTemplateView", IsTemplateView);
            db_obj.AddParameter("@SalesOrderPrint", SalesOrderPrint);
            db_obj.AddParameter("@SerComWOPayment", SerComWOPayment);
            db_obj.AddParameter("@SerPriceWTax", SerPriceWTax);
            db_obj.AddParameter("@InvoiceFormat", InvoiceFormat);
            db_obj.AddParameter("@PrintTerms", PrintTerms);
            db_obj.AddParameter("@DepartmentRequired", DepartmentRequired);
            db_obj.AddParameter("@CategoryRequired", CategoryRequired);
            db_obj.AddParameter("@SubCategoryRequired", SubCategoryRequired);
            db_obj.AddParameter("@defpaymode", defpaymode);
            db_obj.AddParameter("@Isdisplydiscnt", Isdisplydiscnt);
            db_obj.AddParameter("@CustomerSOAPdfFormat", CustomerSOAPdfFormat);
            db_obj.AddParameter("@isaddremark", isaddremark);
            db_obj.AddParameter("@enblCustinvoice", enblCustinvoice);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddParameter("@ReceiptFormat", ReceiptFormat);
            db_obj.AddParameter("@QuotationPrint", QuotationPrint);
            db_obj.AddParameter("@CIPrint", CIPrint);
            db_obj.AddParameter("@DebitorsReportFormat", DebitorsReportFormat);
            db_obj.AddParameter("@ReceiptVoucherFormat", ReceiptVoucherFormat);
            db_obj.AddParameter("@VendorStmtFormat", VendorStmtFormat);
            db_obj.AddParameter("@AgentCommission", AgentCommission);
            db_obj.AddParameter("@ProfitExpenseType", ProfitExpenseType);
            db_obj.AddParameter("@IsEditInvoiceCreator", IsEditInvoiceCreator);
            db_obj.AddParameter("@CustomerDiscounttype", CustomerDiscounttype);
            db_obj.AddParameter("@DefaultEmirate", DefaultEmirate);

            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }
        public int insert_Administration(int Id, string PrintHeader, int? FineExpenseType,int UserId,
           int? defpaymode, int TaxAppliedWithDiscount,int? SendAgreementExpiredMailBefore, int SCInInvoice, string PrintFooter,
               int? Secondarymailday,  int IsEmployeeBasedSCList, int ShowDeletedSC, decimal? DefaultBankCharge,
              int? RefundableExpenseId,int IsMobileDupAllow, int IsTaxPrintForAll,
              int IsAllowSCAmountExceed, string MailSignature, int IsCommissionEditableInInvoice,
              int AdminDesginId, int? TemplateId, string companymail, string Companypwd, string ccmail, string TRN,
            int DefaultInvoiceType, string Companyname,string CompanyPhone,string CompanyContactPerson,
            decimal? VATOB,DateTime? VATOBDate)
        {
            Database_Operations db_obj = new Database_Operations("insert_Administration", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@TRN", TRN);
            db_obj.AddParameter("@DefaultInvoiceType", DefaultInvoiceType);
            db_obj.AddParameter("@Companyname", Companyname);
            db_obj.AddParameter("@companymail", companymail);
            db_obj.AddParameter("@Companypwd", Companypwd);
            db_obj.AddParameter("@CompanyPhone", CompanyPhone);
            db_obj.AddParameter("@CompanyContactPerson", CompanyContactPerson);
            db_obj.AddParameter("@ccmail", ccmail);
            db_obj.AddParameter("@IsCommissionEditableInInvoice", IsCommissionEditableInInvoice);
            db_obj.AddParameter("@MailSignature", MailSignature);
            db_obj.AddParameter("@IsAllowSCAmountExceed", IsAllowSCAmountExceed);
            db_obj.AddParameter("@IsTaxPrintForAll", IsTaxPrintForAll);
            db_obj.AddParameter("@IsMobileDupAllow", IsMobileDupAllow);
            db_obj.AddParameter("@DefaultBankCharge", DefaultBankCharge);
            db_obj.AddParameter("@ShowDeletedSC", ShowDeletedSC);
            db_obj.AddParameter("@IsEmployeeBasedSCList", IsEmployeeBasedSCList);
            db_obj.AddParameter("@Secondarymailday", Secondarymailday);
            db_obj.AddParameter("@PrintHeader", PrintHeader);
            db_obj.AddParameter("@FineExpenseType", FineExpenseType);
            db_obj.AddParameter("@defpaymode", defpaymode);
            db_obj.AddParameter("@TaxAppliedWithDiscount", TaxAppliedWithDiscount);
            db_obj.AddParameter("@SendAgreementExpiredMailBefore", SendAgreementExpiredMailBefore);
            db_obj.AddParameter("@SCInInvoice", SCInInvoice);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddParameter("@PrintFooter", PrintFooter);
            db_obj.AddParameter("@RefundableExpenseId", RefundableExpenseId);
            db_obj.AddParameter("@AdminDesginId", AdminDesginId);
            db_obj.AddParameter("@TemplateId", TemplateId);
            db_obj.AddParameter("@VATOB", VATOB);
            db_obj.AddParameter("@VATOBDate", VATOBDate);

            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int Insert_GeneralSettings(int Id, string PrintHeader, int SerComWOPayment, int? FineExpenseType, int SerPriceWTax,
            int InvoiceFormat, int PrintTerms, int DepartmentRequired, int CategoryRequired, int SubCategoryRequired, string TRN, int UserId,
           int? defpaymode, int Isdisplydiscnt,int TaxAppliedWithDiscount, int DefaultInvoiceType, int CustomerSOAPdfFormat,
              int? SendAgreementExpiredMailBefore, int? TemplateId, int SCInInvoice,int isaddremark,int enblCustinvoice, string companymail, int ReceiptFormat,
           int QuotationPrint,string PrintFooter,int CIPrint,string Companypwd, int DebitorsReportFormat, int ReceiptVoucherFormat,
              int? CustomerDiscounttype,int? Secondarymailday,int? ProfitExpenseType,int IsSoftareNameAdd,string Companyname,int? VendorStmtFormat,
              int SalesOrderPrint,int IsEmployeeBasedSCList,int ShowDeletedSC,decimal? DefaultBankCharge,int IsTemplateView,int SCPredateDays,
              int IsQuotaionEditable,string DefaultQutotnRemark,string DefaultInvoiceRemark,int? RefundableExpenseId,
              int DepartmentInInvoiceVisible,int IsMobileDupAllow,int IsTaxPrintForAll,int IsDisplaySCStatus,int? TransEditdaylimit,
              int IsAllowSCAmountExceed,int IsDisableRoundOff,int SCView,string MailSignature,int IsCommissionEditableInInvoice,
              int AdminDesginId,int IsHideServiceAmtInSC)
        {
            Database_Operations db_obj = new Database_Operations("Insert_GeneralSettings", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@SCView", SCView);
            db_obj.AddParameter("@IsHideServiceAmtInSC", IsHideServiceAmtInSC);
            db_obj.AddParameter("@IsCommissionEditableInInvoice", IsCommissionEditableInInvoice);
            db_obj.AddParameter("@MailSignature", MailSignature);
            db_obj.AddParameter("@IsAllowSCAmountExceed", IsAllowSCAmountExceed);
            db_obj.AddParameter("@IsDisableRoundOff", IsDisableRoundOff);
            db_obj.AddParameter("@TransEditdaylimit", TransEditdaylimit);
            db_obj.AddParameter("@IsDisplaySCStatus", IsDisplaySCStatus);
            db_obj.AddParameter("@IsTaxPrintForAll", IsTaxPrintForAll);
            db_obj.AddParameter("@DepartmentInInvoiceVisible", DepartmentInInvoiceVisible);
            db_obj.AddParameter("@IsMobileDupAllow", IsMobileDupAllow);
            db_obj.AddParameter("@DefaultInvoiceRemark", DefaultInvoiceRemark);
            db_obj.AddParameter("@IsQuotaionEditable", IsQuotaionEditable);
            db_obj.AddParameter("@DefaultQutotnRemark", DefaultQutotnRemark);
            db_obj.AddParameter("@SCPredateDays", SCPredateDays);
            db_obj.AddParameter("@IsTemplateView", IsTemplateView);
            db_obj.AddParameter("@DefaultBankCharge", DefaultBankCharge);
            db_obj.AddParameter("@ShowDeletedSC", ShowDeletedSC);
            db_obj.AddParameter("@IsEmployeeBasedSCList", IsEmployeeBasedSCList);
            db_obj.AddParameter("@SalesOrderPrint", SalesOrderPrint);
            db_obj.AddParameter("@Secondarymailday", Secondarymailday);
            db_obj.AddParameter("@IsSoftareNameAdd", IsSoftareNameAdd);
            db_obj.AddParameter("@PrintHeader", PrintHeader);
            db_obj.AddParameter("@SerComWOPayment", SerComWOPayment);
            db_obj.AddParameter("@FineExpenseType", FineExpenseType);
            db_obj.AddParameter("@SerPriceWTax", SerPriceWTax);
            db_obj.AddParameter("@InvoiceFormat", InvoiceFormat);
            db_obj.AddParameter("@PrintTerms", PrintTerms);
            db_obj.AddParameter("@DepartmentRequired", DepartmentRequired);
            db_obj.AddParameter("@CategoryRequired", CategoryRequired);
            db_obj.AddParameter("@SubCategoryRequired", SubCategoryRequired);
            db_obj.AddParameter("@TRN", TRN);
            db_obj.AddParameter("@defpaymode", defpaymode);
            db_obj.AddParameter("@Isdisplydiscnt", Isdisplydiscnt);
            db_obj.AddParameter("@TaxAppliedWithDiscount", TaxAppliedWithDiscount);
            db_obj.AddParameter("@DefaultInvoiceType", DefaultInvoiceType);
            db_obj.AddParameter("@CustomerSOAPdfFormat", CustomerSOAPdfFormat);
            db_obj.AddParameter("@SendAgreementExpiredMailBefore", SendAgreementExpiredMailBefore);
            db_obj.AddParameter("@TemplateId", TemplateId);
            db_obj.AddParameter("@SCInInvoice", SCInInvoice);
            db_obj.AddParameter("@isaddremark", isaddremark);
            db_obj.AddParameter("@enblCustinvoice", enblCustinvoice);
            db_obj.AddParameter("@companymail", companymail);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddParameter("@ReceiptFormat", ReceiptFormat);
            db_obj.AddParameter("@QuotationPrint", QuotationPrint);
            db_obj.AddParameter("@PrintFooter", PrintFooter);
            db_obj.AddParameter("@CIPrint", CIPrint);
            db_obj.AddParameter("@Companypwd", Companypwd);
            db_obj.AddParameter("@DebitorsReportFormat", DebitorsReportFormat);
            db_obj.AddParameter("@ReceiptVoucherFormat", ReceiptVoucherFormat);
            db_obj.AddParameter("@CustomerDiscounttype", CustomerDiscounttype);
            db_obj.AddParameter("@ProfitExpenseType", ProfitExpenseType);
            db_obj.AddParameter("@Companyname", Companyname);
            db_obj.AddParameter("@VendorStmtFormat", VendorStmtFormat);
            db_obj.AddParameter("@RefundableExpenseId", RefundableExpenseId);
            db_obj.AddParameter("@AdminDesginId", AdminDesginId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region Income

        public DataTable Get_List_Income(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Income", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_Income(int id, string name, string Description, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Income", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@Description", Description);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet Edit_Income(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Income", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int Delete_Income(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Income", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_List_Income_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_Income_Excel", true);
            return db_obj.GetDataTable();
        }


        #endregion

        #region Service Category

        public DataTable Get_List_ServiceCategory(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_ServiceCategory", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_ServiceCategory(int id, string name, string Description, int UserId,string ArabicName)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_ServiceCategory", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@Description", Description);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddParameter("@ArabicName", ArabicName, 1);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet Edit_ServiceCategory(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("Edit_ServiceCategory", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int Delete_ServiceCategory(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_ServiceCategory", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_List_ServiceCategory_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_ServiceCategory_Excel", true);
            return db_obj.GetDataTable();
        }


        #endregion

        #region Department

        public DataTable Get_List_Department(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Department", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_Department(int id, string name, string Description, int UserId,String ArabicName)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Department", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@Description", Description);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddParameter("@ArabicName", ArabicName,1);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet Edit_Department(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Department", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int Delete_Department(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Department", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_List_Department_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_Department_Excel", true);
            return db_obj.GetDataTable();
        }


        #endregion

        #region Service Sub Category

        public DataTable Get_List_ServiceSubCategory(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_ServiceSubCategory", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_ServiceSubCategory(int id, string name,int ServiceCategoryId, string Description, int UserId,string ArabicName)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_ServiceSubCategory", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@ServiceCategoryId", ServiceCategoryId);
            db_obj.AddParameter("@Description", Description);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddParameter("@ArabicName", ArabicName,1);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet Edit_ServiceSubCategory(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("Edit_ServiceSubCategory", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int Delete_ServiceSubCategory(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_ServiceSubCategory", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_List_ServiceSubCategory_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_ServiceSubCategory_Excel", true);
            return db_obj.GetDataTable();
        }


        #endregion

        #region Document

        //Get List of Data
        public DataTable Get_List_Document(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Document", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_Document(int id, string name, string Description, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Document", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@Description", Description);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet Edit_Document(int Id)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Document", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataSet());
        }

        public int Delete_Document(int Id, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Document", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_List_Document_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_Document_Excel", true);
            return db_obj.GetDataTable();
        }

        #endregion

        #region Loan

        //Get List of Data
        public DataTable Get_List_Loan(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Loan", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_Loan(int id, string name, string address, string mobno, string mail, int UserId, string TRn, 
            decimal? CreditAmount, int isCreditCard, int? dueDate)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Loan", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@address", address);
            db_obj.AddParameter("@Mobile", mobno);
            db_obj.AddParameter("@mail", mail);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddParameter("@TRN", TRn);
            db_obj.AddParameter("@CreditAmount", CreditAmount);
            db_obj.AddParameter("@isCreditCard", isCreditCard);
            db_obj.AddParameter("@dueDay", dueDate);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Edit_Loan(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Loan", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataTable());
        }


        public int Delete_Loan(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Loan", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_List_Loan_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_Loan_Excel", true);
            return db_obj.GetDataTable();
        }
        public DataTable GetLoanOB(int id)
        {
            Database_Operations db_obj = new Database_Operations("GetLoanOB", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataTable());
        }

        public int ClearLoanOB(int id, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("ClearLoanOB", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }
        public int Update_OB_Loan(int Id, int OBType, decimal OpeningBalance, DateTime? OBDate, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("Update_Loan_OB", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@OBType", OBType);
            db_obj.AddParameter("@OpeningBalance", OpeningBalance);
            db_obj.AddParameter("@Balance", OBType == 2 ? OpeningBalance * -1 : OpeningBalance);
            db_obj.AddParameter("@OBDate", OBDate);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region Salary Configuration

        public DataSet EditSalaryConfiguration()
        {
            Database_Operations db_obj = new Database_Operations("EditSalaryConfiguration", true);
            return (db_obj.GetDataSet());
        }

        public int InsertSalaryConfiguration(int Id,int SPFromDate,int SPToDate,int OTApplicable,
            decimal? OTNormalDay,decimal? OTWeekend,decimal? OTHoliday,int WorkingHours,int SalBasedOnDays,
            DataTable dtWeekendDays, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("InsertSalaryConfiguration", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@SPFromDate", SPFromDate);
            db_obj.AddParameter("@SPToDate", SPToDate);
            db_obj.AddParameter("@OTApplicable", OTApplicable);
            db_obj.AddParameter("@OTNormalDay", OTNormalDay);
            db_obj.AddParameter("@OTWeekend", OTWeekend);
            db_obj.AddParameter("@OTHoliday", OTHoliday);
            db_obj.AddParameter("@WorkingHours", WorkingHours);
            db_obj.AddParameter("@SalBasedOnDays", SalBasedOnDays);
            db_obj.AddParameter("@dtWeekendDays", dtWeekendDays);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region Holiday

        //Get List of Data
        public DataTable Get_List_Holiday(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Holiday", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_Holiday(int Id, string Name,DateTime Date, string Description, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Holiday", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@Name", Name);
            db_obj.AddParameter("@Date", Date);
            db_obj.AddParameter("@Description", Description);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet Edit_Holiday(int Id)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Holiday", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataSet());
        }

        public int Delete_Holiday(int Id, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Holiday", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_List_Holiday_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_Holiday_Excel", true);
            return db_obj.GetDataTable();
        }

        #endregion

        #region Templates

        public DataTable GetService()
        {
            Database_Operations db_obj = new Database_Operations("GetService", true);
            return (db_obj.GetDataTable());
        }

        //Get List of Data
        public DataTable Get_List_Templates(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Templates", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_Templates(int id, string name,DataTable dtServices, string Description, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Templates", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@dtServices", dtServices);
            db_obj.AddParameter("@Description", Description);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet Edit_Templates(int Id)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Templates", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataSet());
        }

        public int Delete_Templates(int Id, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Templates", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_List_Templates_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_Templates_Excel", true);
            return db_obj.GetDataTable();
        }

        #endregion

        #region Agent

        public DataTable Get_List_Agent(int page_number, int page_size, string filter)
        {
            Database_Operations db_obj = new Database_Operations("List_Agent", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            return db_obj.GetDataTable();
        }

        public DataTable Get_List_Agent_Excel()
        {
            Database_Operations db = new Database_Operations("List_Agent_Excel", true);
            return db.GetDataTable();
        }

        public DataSet Edit_Agent(int id)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Agent", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }
        public int Update_OBAgent(int Id, int OBType, decimal OpeningBalance, DateTime? OBDate, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("Update_OBAgent", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@OBType", OBType);
            db_obj.AddParameter("@OpeningBalance", OpeningBalance);
            db_obj.AddParameter("@OBDate", OBDate);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int Insert_Update_Agent(int id, string name, string address, string Mobile_num, string phone_num, string email,
            string remark, int user_id, string TRN, string ArabicName,decimal ProfitPer)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Agent", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@address", address);
            db_obj.AddParameter("@Mobile_num", Mobile_num);
            db_obj.AddParameter("@phone_num", phone_num);
            db_obj.AddParameter("@email", email);
            db_obj.AddParameter("@remark", remark);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddParameter("@TRN", TRN);
            db_obj.AddParameter("@ProfitPer", ProfitPer);
            db_obj.AddParameter("@ArabicName", ArabicName, 1);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int Delete_Agent(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Agent", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region Leave

        public DataTable Get_List_Leave(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Leave", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_Leave(int id, string name, string Description, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Leave", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@Description", Description);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet Edit_Leave(int Id)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Leave", true);
            db_obj.AddParameter("@Id", Id);
            return (db_obj.GetDataSet());
        }

        public int Delete_Leave(int Id, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Leave", true);
            db_obj.AddParameter("@Id", Id);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_List_Leave_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_Leave_Excel", true);
            return db_obj.GetDataTable();
        }


        #endregion

        #region Template

        //Get List of Data
        public DataTable GetTemplateList(int PageNumber, int PageSize, string Filter, string OrderByColumnName, string OrderBy)
        {
            Database_Operations databaseOperations = new Database_Operations("GetTemplateList", true);
            databaseOperations.AddParameter("@PageNumber", PageNumber);
            databaseOperations.AddParameter("@PageSize", PageSize);
            databaseOperations.AddParameter("@Filter", Filter);
            databaseOperations.AddParameter("@OrderByColumnName", OrderByColumnName);
            databaseOperations.AddParameter("@OrderBy", OrderBy);
            return databaseOperations.GetDataTable();
        }

        public DataTable GeTemplateListExcel()
        {
            Database_Operations databaseOperations = new Database_Operations("GetTemplateListExcel", true);
            return databaseOperations.GetDataTable();
        }

        //Particular User
        public DataTable EditTemplate(int Id)
        {
            Database_Operations databaseOperations = new Database_Operations("EditTemplate", true);
            databaseOperations.AddParameter("@Id", Id);
            return (databaseOperations.GetDataTable());
        }

        public int InsertUpdateTemplate(int Id, string Name, string Subject, string Description, int UserId)
        {
            Database_Operations databaseOperations = new Database_Operations("InsertUpdateTemplate", true);
            databaseOperations.AddParameter("@Id", Id);
            databaseOperations.AddParameter("@Name", Name);
            databaseOperations.AddParameter("@Subject", Subject);
            databaseOperations.AddParameter("@Description", Description,1);
            databaseOperations.AddParameter("@UserId", UserId);
            databaseOperations.AddOutputParameter("@Result");
            databaseOperations.ExecuteQuery();
            return Convert.ToInt32(databaseOperations.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int DeleteTemplate(int Id, int UserId)
        {
            Database_Operations databaseOperations = new Database_Operations("DeleteTemplate", true);
            databaseOperations.AddParameter("@Id", Id);
            databaseOperations.AddParameter("@UserId", UserId);
            databaseOperations.AddOutputParameter("@Result");
            databaseOperations.ExecuteQuery();
            return Convert.ToInt32(databaseOperations.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region Document Agent

        public DataTable Get_List_DocAgent(int page_number, int page_size, string filter)
        {
            Database_Operations db_obj = new Database_Operations("Get_List_DocAgent", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            return db_obj.GetDataTable();
        }

        public DataTable Get_List_DocAgent_Excel()
        {
            Database_Operations db = new Database_Operations("Get_List_DocAgent_Excel", true);
            return db.GetDataTable();
        }

        public DataSet Edit_DocAgent(int id)
        {
            Database_Operations db_obj = new Database_Operations("Edit_DocAgent", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public int Insert_Update_DocAgent(int id, string name, string address, string Mobile_num, 
            string ContactPersn, string email,int user_id)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_DocAgent", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@address", address);
            db_obj.AddParameter("@Mobile_num", Mobile_num);
            db_obj.AddParameter("@email", email);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddParameter("@ContactPersn", ContactPersn);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int Delete_DocAgent(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_DocAgent", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region Shareholder

        //Get List of Data
        public DataTable ListShareholder(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("ListShareholder", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_UpdateShareholder(int id, string name, decimal SharePercentage, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_UpdateShareholder", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@name", name);
            db_obj.AddParameter("@SharePercentage", SharePercentage);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet EditShareholder(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("EditShareholder", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int DeleteShareholder(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("DeleteShareholder", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable ListExcelShareholder()
        {
            Database_Operations db_obj = new Database_Operations("ListExcelShareholder", true);
            return db_obj.GetDataTable();
        }

        public int Update_ShareHolder_OB(int id, decimal OB, DateTime obdate, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("Update_ShareHolder_OB", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@OB", OB);
            db_obj.AddParameter("@obdate", obdate);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region DepositType

        public DataTable ListDepositType(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("ListDepositType", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_UpdateDepositType(int id, string name, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_UpdateDepositType", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@name", name);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet EditDepositType(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("EditDepositType", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int DeleteDepositType(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("DeleteDepositType", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable ListExcelDepositType()
        {
            Database_Operations db_obj = new Database_Operations("ListExcelDepositType", true);
            return db_obj.GetDataTable();
        }

        #endregion

        #region Sponser

        public DataTable Get_ListSponser(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("Get_ListSponser", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public DataTable Get_ListSponserExcel()
        {
            Database_Operations db = new Database_Operations("Get_ListSponserExcel", true);
            return db.GetDataTable();
        }

        public DataSet EditSponser(int id)
        {
            Database_Operations db_obj = new Database_Operations("EditSponser", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public DataSet EditSponserDocs(int id)
        {
            Database_Operations db_obj = new Database_Operations("EditSponserDocs", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public int Insert_UpdateSponser(int id, string name, string address, string Mobile_num, string phone_num, string email,
            string remark, int user_id,  string ArabicName,string UAEPass)
        {
            Database_Operations db_obj = new Database_Operations("Insert_UpdateSponser", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@address", address);
            db_obj.AddParameter("@Mobile_num", Mobile_num);
            db_obj.AddParameter("@phone_num", phone_num);
            db_obj.AddParameter("@email", email);
            db_obj.AddParameter("@remark", remark);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddParameter("@UAEPass", UAEPass);
            db_obj.AddParameter("@ArabicName", ArabicName, 1);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }
       
        public int DeleteSponser(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("DeleteSponser", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int UpdateSponserDocument(int id, DataTable dt_doc, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("UpdateSponserDocument", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@dt_doc", dt_doc);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion


        public int InsertCustomerAdvnce( DataTable dt_serDetail, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("InsertCustomerAdvnce", true);
            db_obj.AddParameter("@dt_serDetail", dt_serDetail);
            db_obj.AddParameter("@UserId", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #region Checklist

        public DataTable Get_ListChecklist(int page_number, int page_size, string filter)
        {
            Database_Operations db_obj = new Database_Operations("Get_ListChecklist", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            return db_obj.GetDataTable();
        }

        public int Insert_UpdateChecklist(int id, string name, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_UpdateChecklist", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@name", name);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet EditChecklist(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("EditChecklist", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int DeleteChecklist(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("DeleteChecklist", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_ListChecklistExcel()
        {
            Database_Operations db_obj = new Database_Operations("Get_ListChecklistExcel", true);
            return db_obj.GetDataTable();
        }

        #endregion

        #region Company

        public DataTable Get_ListCompany(int page_number, int page_size, string filter)
        {
            Database_Operations db_obj = new Database_Operations("Get_ListCompany", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            return db_obj.GetDataTable();
        }

        public DataTable Get_ListCompanyExcel()
        {
            Database_Operations db = new Database_Operations("Get_ListCompanyExcel", true);
            return db.GetDataTable();
        }

        public DataSet EditCompany(int id)
        {
            Database_Operations db_obj = new Database_Operations("EditCompany", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public int Insert_UpdateCompany(int id, string name, string address, string Mobile_num, string ContactPerson, string email,
            string remark, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("Insert_UpdateCompany", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@address", address);
            db_obj.AddParameter("@Mobile_num", Mobile_num);
            db_obj.AddParameter("@ContactPerson", ContactPerson);
            db_obj.AddParameter("@email", email);
            db_obj.AddParameter("@remark", remark);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int DeleteCompany(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("DeleteCompany", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region CompanyStaff

        public DataTable Get_ListCompanyStaff(int page_number, int page_size, string filter)
        {
            Database_Operations db_obj = new Database_Operations("Get_ListCompanyStaff", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            return db_obj.GetDataTable();
        }

        public DataTable Get_ListCompanyStaffExcel()
        {
            Database_Operations db = new Database_Operations("Get_ListCompanyStaffExcel", true);
            return db.GetDataTable();
        }

        public DataSet EditCompanyStaff(int id)
        {
            Database_Operations db_obj = new Database_Operations("EditCompanyStaff", true);
            db_obj.AddParameter("@Id", id);
            return (db_obj.GetDataSet());
        }

        public int Insert_UpdateCompanyStaff(int id, string name, string address, string Mobile_num,int CompanyId,decimal AgreementAmount, string email,
            string remark, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("Insert_UpdateCompanyStaff", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@address", address);
            db_obj.AddParameter("@Mobile_num", Mobile_num);
            db_obj.AddParameter("@CompanyId", CompanyId);
            db_obj.AddParameter("@AgreementAmount", AgreementAmount);
            db_obj.AddParameter("@email", email);
            db_obj.AddParameter("@remark", remark);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int DeleteCompanyStaff(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("DeleteCompanyStaff", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int InsertCompanyStaffPayment(int id, DateTime? Paydate, string remark, decimal Amount, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("InsertCompanyStaffPayment", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Paydate", Paydate);
            db_obj.AddParameter("@remark", remark);
            db_obj.AddParameter("@Amount", Amount);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int DeleteCompanyStaffPayment(int id,int  DId)
        {
            Database_Operations db_obj = new Database_Operations("DeleteCompanyStaffPayment", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@DId", DId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int InsertCompletechecklist(int StaffId, int DId,int CheckListId, int user_id)
        {
            Database_Operations db_obj = new Database_Operations("InsertCompletechecklist", true);
            db_obj.AddParameter("@StaffId", StaffId);
            db_obj.AddParameter("@DId", DId);
            db_obj.AddParameter("@CheckListId", CheckListId);
            db_obj.AddParameter("@user_id", user_id);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int InsertFilechecklist(int StaffId, int DId, int CheckListId,string Filenames,string filenamesave)
        {
            Database_Operations db_obj = new Database_Operations("InsertFilechecklist", true);
            db_obj.AddParameter("@StaffId", StaffId);
            db_obj.AddParameter("@DId", DId);
            db_obj.AddParameter("@CheckListId", CheckListId);
            db_obj.AddParameter("@Filenames", Filenames);
            db_obj.AddParameter("@filenamesave", filenamesave);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int InsertExpensechecklist(int StaffId, int DId, int CheckListId, decimal Expense)
        {
            Database_Operations db_obj = new Database_Operations("InsertExpensechecklist", true);
            db_obj.AddParameter("@StaffId", StaffId);
            db_obj.AddParameter("@DId", DId);
            db_obj.AddParameter("@CheckListId", CheckListId);
            db_obj.AddParameter("@Expense", Expense);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }


        #endregion

        #region Supplier

        public DataTable Get_ListSupplier(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Supplier", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_UpdateSupplier(int id, string name, string address, string mobno, string mail, int UserId,string TRN)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Supplier", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@address", address);
            db_obj.AddParameter("@Mobile", mobno);
            db_obj.AddParameter("@mail", mail);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddParameter("@TRN", TRN);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet EditSupplier(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Supplier", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int DeleteSupplier(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Supplier", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_ListSupplierExcel()
        {
            Database_Operations db_obj = new Database_Operations("List_Supplier_Excel", true);
            return db_obj.GetDataTable();
        }

        #endregion

        #region Party

        public DataTable Get_ListParty(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Party", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_UpdateParty(int id, string name, string address, string mobno, string mail, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_Party", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@address", address);
            db_obj.AddParameter("@Mobile", mobno);
            db_obj.AddParameter("@mail", mail);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet EditParty(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("Edit_Party", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int DeleteParty(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_Party", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_ListPartyExcel()
        {
            Database_Operations db_obj = new Database_Operations("List_Party_Excel", true);
            return db_obj.GetDataTable();
        }

        #endregion

        #region LeadSource

        public DataTable GetLeadSourceList(int PageNumber, int PageSize, string Filter, string OrderByColumnName, string OrderBy)
        {
            Database_Operations databaseOperations = new Database_Operations("GetLeadSourceList", true);
            databaseOperations.AddParameter("@PageNumber", PageNumber);
            databaseOperations.AddParameter("@PageSize", PageSize);
            databaseOperations.AddParameter("@Filter", Filter);
            databaseOperations.AddParameter("@OrderByColumnName", OrderByColumnName);
            databaseOperations.AddParameter("@OrderBy", OrderBy);
            return databaseOperations.GetDataTable();
        }

        public DataTable GetLeadSourceListExcel()
        {
            Database_Operations databaseOperations = new Database_Operations("GetLeadSourceListExcel", true);
            return databaseOperations.GetDataTable();
        }

        public DataTable EditLeadSource(int Id)
        {
            Database_Operations databaseOperations = new Database_Operations("EditLeadSource", true);
            databaseOperations.AddParameter("@Id", Id);
            return (databaseOperations.GetDataTable());
        }

        public int InsertUpdateLeadSource(int Id, string Name, string Description, int UserId)
        {
            Database_Operations databaseOperations = new Database_Operations("InsertUpdateLeadSource", true);
            databaseOperations.AddParameter("@Id", Id);
            databaseOperations.AddParameter("@Name", Name);
            databaseOperations.AddParameter("@Description", Description);
            databaseOperations.AddParameter("@UserId", UserId);
            databaseOperations.AddOutputParameter("@Result");
            databaseOperations.ExecuteQuery();
            return Convert.ToInt32(databaseOperations.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int DeleteLeadSource(int Id, int UserId)
        {
            Database_Operations databaseOperations = new Database_Operations("DeleteLeadSource", true);
            databaseOperations.AddParameter("@Id", Id);
            databaseOperations.AddParameter("@UserId", UserId);
            databaseOperations.AddOutputParameter("@Result");
            databaseOperations.ExecuteQuery();
            return Convert.ToInt32(databaseOperations.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region LeadDepartment

        public DataTable Get_ListLeadDepartment(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_LeadDepartment", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_UpdateLeadDepartment(int id, string name, string Description, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_UpdateLeadDepartment", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@Description", Description);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet EditLeadDepartment(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("EditLeadDepartment", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int DeleteLeadDepartment(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("DeleteLeadDepartment", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_ListLeadDepartment_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_LeadDepartment_Excel", true);
            return db_obj.GetDataTable();
        }

        #endregion

        #region Question1

        public DataTable Get_ListQuestion1(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Question1", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_UpdateQuestion1(int id, string name, string Description, int UserId, int LeadDepartmentId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_UpdateQuestion1", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@LeadDepartmentId", LeadDepartmentId);
            db_obj.AddParameter("@Description", Description);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet EditQuestion1(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("EditQuestion1", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int DeleteQuestion1(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("DeleteQuestion1", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_ListQuestion1_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_Question1Excel", true);
            return db_obj.GetDataTable();
        }

        #endregion

        #region Question2

        public DataTable Get_ListQuestion2(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Question2", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_UpdateQuestion2(int id, string name, string Description, int UserId, int LeadDepartmentId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_UpdateQuestion2", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@LeadDepartmentId", LeadDepartmentId);
            db_obj.AddParameter("@Description", Description);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet EditQuestion2(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("EditQuestion2", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int DeleteQuestion2(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("DeleteQuestion2", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_ListQuestion2_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_Question2Excel", true);
            return db_obj.GetDataTable();
        }

        #endregion

        #region Question3

        public DataTable Get_ListQuestion3(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_Question3", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_UpdateQuestion3(int id, string name, string Description, int UserId, int LeadDepartmentId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_UpdateQuestion3", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@LeadDepartmentId", LeadDepartmentId);
            db_obj.AddParameter("@Description", Description);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet EditQuestion3(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("EditQuestion3", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int DeleteQuestion3(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("DeleteQuestion3", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable Get_ListQuestion3_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_Question3Excel", true);
            return db_obj.GetDataTable();
        }

        #endregion

        #region Priority

        public DataTable GetPriorityList(int PageNumber, int PageSize, string Filter, string OrderByColumnName, string OrderBy)
        {
            Database_Operations databaseOperations = new Database_Operations("GetPriorityList", true);
            databaseOperations.AddParameter("@PageNumber", PageNumber);
            databaseOperations.AddParameter("@PageSize", PageSize);
            databaseOperations.AddParameter("@Filter", Filter);
            databaseOperations.AddParameter("@OrderByColumnName", OrderByColumnName);
            databaseOperations.AddParameter("@OrderBy", OrderBy);
            return databaseOperations.GetDataTable();
        }

        public DataTable GetPriorityListExcel()
        {
            Database_Operations databaseOperations = new Database_Operations("GetPriorityListExcel", true);
            return databaseOperations.GetDataTable();
        }

        public DataTable EditPriority(int Id)
        {
            Database_Operations databaseOperations = new Database_Operations("EditPriority", true);
            databaseOperations.AddParameter("@Id", Id);
            return (databaseOperations.GetDataTable());
        }

        public int InsertUpdatePriority(int Id, string Name, string Description, int UserId)
        {
            Database_Operations databaseOperations = new Database_Operations("InsertUpdatePriority", true);
            databaseOperations.AddParameter("@Id", Id);
            databaseOperations.AddParameter("@Name", Name);
            databaseOperations.AddParameter("@Description", Description);
            databaseOperations.AddParameter("@UserId", UserId);
            databaseOperations.AddOutputParameter("@Result");
            databaseOperations.ExecuteQuery();
            return Convert.ToInt32(databaseOperations.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int DeletePriority(int Id, int UserId)
        {
            Database_Operations databaseOperations = new Database_Operations("DeletePriority", true);
            databaseOperations.AddParameter("@Id", Id);
            databaseOperations.AddParameter("@UserId", UserId);
            databaseOperations.AddOutputParameter("@Result");
            databaseOperations.ExecuteQuery();
            return Convert.ToInt32(databaseOperations.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

         

        #region Status

        public DataTable GetStatusList(int PageNumber, int PageSize, string Filter, string OrderByColumnName, string OrderBy)
        {
            Database_Operations databaseOperations = new Database_Operations("GetStatusList", true);
            databaseOperations.AddParameter("@PageNumber", PageNumber);
            databaseOperations.AddParameter("@PageSize", PageSize);
            databaseOperations.AddParameter("@Filter", Filter);
            databaseOperations.AddParameter("@OrderByColumnName", OrderByColumnName);
            databaseOperations.AddParameter("@OrderBy", OrderBy);
            return databaseOperations.GetDataTable();
        }

        public DataTable GetStatusListExcel()
        {
            Database_Operations databaseOperations = new Database_Operations("GetStatusListExcel", true);
            return databaseOperations.GetDataTable();
        }

        public DataTable EditStatus(int Id)
        {
            Database_Operations databaseOperations = new Database_Operations("EditStatus", true);
            databaseOperations.AddParameter("@Id", Id);
            return (databaseOperations.GetDataTable());
        }

        public int InsertUpdateStatus(int Id, string Name, string Description, int UserId, int IsClosed)
        {
            Database_Operations databaseOperations = new Database_Operations("InsertUpdateStatus", true);
            databaseOperations.AddParameter("@Id", Id);
            databaseOperations.AddParameter("@Name", Name);
            databaseOperations.AddParameter("@Description", Description);
            databaseOperations.AddParameter("@IsClosed", IsClosed);
            databaseOperations.AddParameter("@UserId", UserId);
            databaseOperations.AddOutputParameter("@Result");
            databaseOperations.ExecuteQuery();
            return Convert.ToInt32(databaseOperations.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public int DeleteStatus(int Id, int UserId)
        {
            Database_Operations databaseOperations = new Database_Operations("DeleteStatus", true);
            databaseOperations.AddParameter("@Id", Id);
            databaseOperations.AddParameter("@UserId", UserId);
            databaseOperations.AddOutputParameter("@Result");
            databaseOperations.ExecuteQuery();
            return Convert.ToInt32(databaseOperations.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        public int InsertUpdateSegment(int Id, string Name, string Description, int UserId)
        {
            Database_Operations databaseOperations = new Database_Operations("InsertUpdateSegment", true);
            databaseOperations.AddParameter("@Id", Id);
            databaseOperations.AddParameter("@Name", Name);
            databaseOperations.AddParameter("@Description", Description);
            databaseOperations.AddParameter("@UserId", UserId);
            databaseOperations.AddOutputParameter("@Result");
            databaseOperations.ExecuteQuery();
            return Convert.ToInt32(databaseOperations.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable DrpSegment()
        {
            Database_Operations databaseOperations = new Database_Operations("DrpSegment", true);
            return (databaseOperations.GetDataTable());
        }

        public int InsertUpdateCity(int Id, string Name, string Description, int UserId)
        {
            Database_Operations databaseOperations = new Database_Operations("InsertUpdateCity", true);
            databaseOperations.AddParameter("@Id", Id);
            databaseOperations.AddParameter("@Name", Name);
            databaseOperations.AddParameter("@Description", Description);
            databaseOperations.AddParameter("@UserId", UserId);
            databaseOperations.AddOutputParameter("@Result");
            databaseOperations.ExecuteQuery();
            return Convert.ToInt32(databaseOperations.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataTable DrpCity()
        {
            Database_Operations databaseOperations = new Database_Operations("DrpCity", true);
            return (databaseOperations.GetDataTable());
        }

        public DataSet EmployeePerformanceReportList(DateTime? fromdate,DateTime? todate,int? EmployeeId, int page_number, int page_size)
        {
            Database_Operations databaseOperations = new Database_Operations("EmployeePerformanceReportList", true);
            databaseOperations.AddParameter("@Fromdate", fromdate);
            databaseOperations.AddParameter("@Todate", todate);
            databaseOperations.AddParameter("@EmployeeId", EmployeeId);
            databaseOperations.AddParameter("@page_number", page_number);
            databaseOperations.AddParameter("@page_size", page_size);
            return (databaseOperations.GetDataSet());
        }

        public DataSet EmployeePerformanceReportExcel(DateTime? fromdate, DateTime? todate, int? EmployeeId )
        {
            Database_Operations databaseOperations = new Database_Operations("EmployeePerformanceReportExcel", true);
            databaseOperations.AddParameter("@Fromdate", fromdate);
            databaseOperations.AddParameter("@Todate", todate);
            databaseOperations.AddParameter("@EmployeeId", EmployeeId);
            return (databaseOperations.GetDataSet());
        }

#region  Activity
        public DataTable GetActivityListExcel()
        {
            Database_Operations databaseOperations = new Database_Operations("GetActivityListExcel", true);
            return databaseOperations.GetDataTable();
        }
        public DataTable GetActivityList(int PageNumber, int PageSize, string Filter, string OrderByColumnName, string OrderBy)
        {
            Database_Operations databaseOperations = new Database_Operations("GetActivityList", true);
            databaseOperations.AddParameter("@PageNumber", PageNumber);
            databaseOperations.AddParameter("@PageSize", PageSize);
            databaseOperations.AddParameter("@Filter", Filter);
            databaseOperations.AddParameter("@OrderByColumnName", OrderByColumnName);
            databaseOperations.AddParameter("@OrderBy", OrderBy);
            return databaseOperations.GetDataTable();
        }
        public DataTable EditActivity(int Id)
        {
            Database_Operations databaseOperations = new Database_Operations("EditActivity", true);
            databaseOperations.AddParameter("@Id", Id);
            return (databaseOperations.GetDataTable());
        }
        public int InsertUpdateActivity(int Id, string Name, string Description, int UserId)
        {
            Database_Operations databaseOperations = new Database_Operations("InsertUpdateActivity", true);
            databaseOperations.AddParameter("@Id", Id);
            databaseOperations.AddParameter("@Name", Name);
            databaseOperations.AddParameter("@Description", Description);
            databaseOperations.AddParameter("@UserId", UserId);
            databaseOperations.AddOutputParameter("@Result");
            databaseOperations.ExecuteQuery();
            return Convert.ToInt32(databaseOperations.SqlCmd.Parameters["@Result"].Value.ToString());
        }
        public int DeleteActivity(int Id, int UserId)
        {
            Database_Operations databaseOperations = new Database_Operations("DeleteActivity", true);
            databaseOperations.AddParameter("@Id", Id);
            databaseOperations.AddParameter("@UserId", UserId);
            databaseOperations.AddOutputParameter("@Result");
            databaseOperations.ExecuteQuery();
            return Convert.ToInt32(databaseOperations.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region ServiceStatus

        public DataTable ListServiceStatus(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("ListServiceStatus", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public DataTable ListServiceStatusExcel()
        {
            Database_Operations db_obj = new Database_Operations("ListServiceStatusExcel", true);
            return db_obj.GetDataTable();
        }

        public int Insert_UpdateServiceStatus(int id, string name, string descr, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Insert_UpdateServiceStatus", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);
            db_obj.AddParameter("@Description", descr);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet EditServiceStatus(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("EditServiceStatus", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int DeleteServiceStatus(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("DeleteServiceStatus", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        #endregion

        #region LeaveEntry 
        public DataTable DrpLeaveType()
        {
            Database_Operations db_obj = new Database_Operations("DrpLeaveType", true);
            return db_obj.GetDataTable();
        }

        public DataTable Get_List_LeaveEntry(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_LeaveEntry", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public DataTable Get_List_LeaveEntry_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_LeaveEntry_Excel", true);
            return db_obj.GetDataTable();
        }

        public int Insert_Update_LeaveEntry(int id, int employeeId, int leavetype, DateTime from, DateTime to, string reason, int userid)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_LeaveEntry", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@EmployeeId", employeeId);
            db_obj.AddParameter("@leavetype", leavetype);
            db_obj.AddParameter("@from", from);
            db_obj.AddParameter("@to", to);
            db_obj.AddParameter("@reason", reason);
            db_obj.AddParameter("@User_Id", userid);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet Edit_LeaveEntry(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("Edit_LeaveMaster", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int Delete_LeaveEntry(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_LeaveEntry", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }
        public int Insert_Update_LeaveType(int id, string name, int userid)
        {
            Database_Operations db_obj = new Database_Operations("Insert_Update_LeaveType", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@Name", name);

            db_obj.AddParameter("@User_Id", userid);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }
        public DataTable Get_List_LeaveType(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("List_LeaveType", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }
        public DataSet Edit_LeaveType(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("Edit_LeaveType", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int Delete_LeaveType(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("Delete_LeaveType", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }
        public DataTable Get_List_LeaveType_Excel()
        {
            Database_Operations db_obj = new Database_Operations("List_LeaveType_Excel", true);
            return db_obj.GetDataTable();
        }
        #endregion

        #region FixedAsset

        //Get List of Data
        public DataTable Get_ListFixedAsset(int page_number, int page_size, string filter, string column, string order)
        {
            Database_Operations db_obj = new Database_Operations("Get_ListFixedAsset", true);
            db_obj.AddParameter("@page_number", page_number);
            db_obj.AddParameter("@count", page_size);
            db_obj.AddParameter("@filter_condition", filter);
            db_obj.AddParameter("@Column_name", column);
            db_obj.AddParameter("@asc_desc", order);
            return db_obj.GetDataTable();
        }

        public int Insert_UpdateFixedAsset(int id, string name, string desc, decimal? CurrentValue, int UserId,
            DateTime? OpeningDate)
        {
            Database_Operations db_obj = new Database_Operations("Insert_UpdateFixedAsset", true);
            db_obj.AddParameter("@Id", id);
            db_obj.AddParameter("@type", name);
            db_obj.AddParameter("@desc", desc);
            db_obj.AddParameter("@CurrentValue", CurrentValue);
            db_obj.AddParameter("@User_Id", UserId);
            db_obj.AddParameter("@OpeningDate", OpeningDate);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }

        public DataSet EditFixedAsset(int incmid)
        {
            Database_Operations db_obj = new Database_Operations("EditFixedAsset", true);
            db_obj.AddParameter("@Id", incmid);
            return (db_obj.GetDataSet());
        }

        public int DeleteFixedAsset(int incmid, int UserId)
        {
            Database_Operations db_obj = new Database_Operations("DeleteFixedAsset", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }
        public int DisposeFixedAsset(int incmid, int UserId,DateTime? disposaldate,string remark)
        {
            Database_Operations db_obj = new Database_Operations("DisposeFixedAsset", true);
            db_obj.AddParameter("@Id", incmid);
            db_obj.AddParameter("@user_id", UserId);
            db_obj.AddParameter("@disposaldate", disposaldate);
            db_obj.AddParameter("@remark", remark);
            db_obj.AddOutputParameter("@Result");
            db_obj.ExecuteQuery();
            return Convert.ToInt32(db_obj.SqlCmd.Parameters["@Result"].Value.ToString());
        }
        public DataTable Get_ListFixedAssetExcel()
        {
            Database_Operations db_obj = new Database_Operations("Get_ListFixedAssetExcel", true);
            return db_obj.GetDataTable();
        }
        public DataTable Assethistory(int Id)
        {
            Database_Operations db_obj = new Database_Operations("Assethistory", true);
            db_obj.AddParameter("@Id", Id);
            return db_obj.GetDataTable();
        }
        #endregion
    }
}