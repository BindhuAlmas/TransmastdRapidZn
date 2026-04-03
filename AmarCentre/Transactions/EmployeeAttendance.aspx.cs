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
using System.Data.OleDb;

namespace AmarCentre.Transactions
{
    public partial class EmployeeAttendance : System.Web.UI.Page
    {
        Transaction_Bal obj_trans = new Transaction_Bal();
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
                previlage_check();
                previlage_action_check();
                fillMonth();
                fillYear();
                Clear();
                grid_fill(1, 10, "", "", "");
            }
        }

        //Get List 
        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_trans.Get_List_EmployeeAttendance(page_number, page_size, filter, column, order);
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
            DataTable dt = obj_trans.Get_List_EmployeeAttendance_Excel();
            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "EmployeeAttendance");
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                HttpContext.Current.Response.Write(style);
                Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        public void fillMonth()
        {
            RadComboBoxItem CodeItem;
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "January";
            CodeItem.Value = "1";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "February";
            CodeItem.Value = "2";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "March";
            CodeItem.Value = "3";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "April";
            CodeItem.Value = "4";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "May";
            CodeItem.Value = "5";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "June";
            CodeItem.Value = "6";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "July";
            CodeItem.Value = "7";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "August";
            CodeItem.Value = "8";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "September";
            CodeItem.Value = "9";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "October";
            CodeItem.Value = "10";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "November";
            CodeItem.Value = "11";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "December";
            CodeItem.Value = "12";
            drpMonth.Items.Add(CodeItem);
        }

        public void fillYear()
        {
            RadComboBoxItem CodeItem;
            int lastyear = DateTime.Now.Year;
            for (int date = lastyear; date >= 2018; date--)
            {
                CodeItem = new RadComboBoxItem();
                CodeItem.Text = date.ToString();
                CodeItem.Value = date.ToString();
                drpYear.Items.Add(CodeItem);
            }
        }

        /*rpt_list OnItemCommand*/
        protected void rpt_list_OnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            Clear();
            pnl_add.Visible = true;
            HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdn_id");
            DataSet ds = obj_trans.Edit_EmployeeAttendance(Convert.ToInt32(hdn_rpt_id.Value));
            DataTable dt1 = ds.Tables[0];
            DataTable dtDetail = ds.Tables[1];/* Detail*/

            hdn_id.Value = dt1.Rows[0]["Id"].ToString();
            lbl_Code.Text = dt1.Rows[0]["Code"].ToString();
            drpMonth.SelectedValue = dt1.Rows[0]["Month"].ToString();
            drpYear.SelectedValue = dt1.Rows[0]["Year"].ToString();
            drpMonth.Enabled = false;
            drpYear.Enabled = false;
            hdnfileName.Value = dt1.Rows[0]["FileName"].ToString();
            hdnfileSaveName.Value = dt1.Rows[0]["FileSavedName"].ToString();
            hdnfileExtension.Value = dt1.Rows[0]["FileExtension"].ToString();
            rpt_Item_list.DataSource = dtDetail;
            rpt_Item_list.DataBind();
            fillEmployee();

            lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
            hdn_AttDetailId.Value = "-" + (rpt_Item_list.Items.Count + 1).ToString();

            if (dt1.Rows[0]["SalaryProcessCount"].ToString() == "0")
            {
                btn_save.Visible = hdn_update.Value == "0" ? false : true;
                btnDelete.Visible = hdn_delete.Value == "0" ? false : true;
            }
            else
            {
                btn_save.Visible = false;
                btnDelete.Visible = false;
            }

            Upd_Add_Panel.Update();
        }

        protected void MonthYearChanged(object sender, EventArgs e)
        {
            
            if (drpMonth.SelectedValue != "" && drpYear.SelectedValue != "")
            {
                DataSet ds = obj_trans.GetEmployeeAttendance(Convert.ToInt32(drpMonth.SelectedValue), Convert.ToInt32(drpYear.SelectedValue));
                DataTable dt1 = ds.Tables[0];
                DataTable dtDetail = ds.Tables[1];/* Detail*/
                if (dt1.Rows.Count > 0)
                {
                    hdn_id.Value = dt1.Rows[0]["Id"].ToString();
                    lbl_Code.Text = dt1.Rows[0]["Code"].ToString();
                    drpMonth.SelectedValue = dt1.Rows[0]["Month"].ToString();
                    drpYear.SelectedValue = dt1.Rows[0]["Year"].ToString();
                    drpMonth.Enabled = false;
                    drpYear.Enabled = false;
                    hdnfileName.Value = dt1.Rows[0]["FileName"].ToString();
                    hdnfileSaveName.Value = dt1.Rows[0]["FileSavedName"].ToString();
                    hdnfileExtension.Value = dt1.Rows[0]["FileExtension"].ToString();
                    rpt_Item_list.DataSource = dtDetail;
                    rpt_Item_list.DataBind();
                    fillEmployee();

                    if (dt1.Rows[0]["SalaryProcessCount"].ToString() == "0")
                        btn_save.Visible = hdn_update.Value == "0" ? false : true;
                    else
                        btn_save.Visible = false;

                    Upd_Add_PanelInner.Update();
                }
                else if (dtDetail.Rows.Count > 0)
                {
                    rpt_Item_list.DataSource = dtDetail;
                    rpt_Item_list.DataBind();
                    fillEmployee();
                    Upd_ItemList.Update();
                }
                else
                {
                    DataTable dtEmp = new DataTable();
                    dtEmp.Columns.Add("Id", typeof(int));
                    dtEmp.Columns.Add("EmployeeId", typeof(int));
                    dtEmp.Columns.Add("FromExcel", typeof(int));
                    dtEmp.Columns.Add("EmployeeName", typeof(string));
                    dtEmp.Columns.Add("TotalWorkingDays", typeof(int));
                    dtEmp.Columns.Add("EmployeeWorkedDays", typeof(int));
                    dtEmp.Columns.Add("OTAtWorking", typeof(int));
                    dtEmp.Columns.Add("OTAtWeekend", typeof(int));
                    dtEmp.Columns.Add("OTAtHoliday", typeof(int));
                    dtEmp.Columns.Add("Salary", typeof(decimal));
                    dtEmp.Columns.Add("ApplicableSalary", typeof(decimal));

                    rpt_Item_list.DataSource = dtEmp;
                    rpt_Item_list.DataBind();
                    ClearEmployeeDetail();
                    fillEmployee();
                    Upd_ItemList.Update();
                }
            }
            
        }

        protected void btn_new_line_OnClick(object sender, EventArgs e)
        {
            DataTable dtEmp = new DataTable();
            dtEmp.Columns.Add("Id", typeof(int));
            dtEmp.Columns.Add("EmployeeId", typeof(int));
            dtEmp.Columns.Add("FromExcel", typeof(int));
            dtEmp.Columns.Add("EmployeeName", typeof(string));
            dtEmp.Columns.Add("TotalWorkingDays", typeof(int));
            dtEmp.Columns.Add("EmployeeWorkedDays", typeof(decimal));
            dtEmp.Columns.Add("OTAtWorking", typeof(int));
            dtEmp.Columns.Add("OTAtWeekend", typeof(int));
            dtEmp.Columns.Add("OTAtHoliday", typeof(int));
            dtEmp.Columns.Add("Salary", typeof(decimal));
            dtEmp.Columns.Add("ApplicableSalary", typeof(decimal));

            if (rpt_Item_list.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_Item_list.Items)
                {
                    HiddenField hdnAttDId = (HiddenField)itm.FindControl("hdnAttDId");
                    HiddenField hdnAttDEmployeeId = (HiddenField)itm.FindControl("hdnAttDEmployeeId");
                    HiddenField hdnFromExcel = (HiddenField)itm.FindControl("hdnFromExcel");
                    Label lblAttDEmployeeName = (Label)itm.FindControl("lblAttDEmployeeName");
                    TextBox txtAttDTotWorkingDays = (TextBox)itm.FindControl("txtAttDTotWorkingDays");
                    TextBox txtAttDEmpAttendedDays = (TextBox)itm.FindControl("txtAttDEmpAttendedDays");
                    TextBox txtAttDOTAtWorking = (TextBox)itm.FindControl("txtAttDOTAtWorking");
                    TextBox txtAttDOTAtWeekend = (TextBox)itm.FindControl("txtAttDOTAtWeekend");
                    TextBox txtAttDOTAtHoliday = (TextBox)itm.FindControl("txtAttDOTAtHoliday");
                    TextBox txtAttDSalary = (TextBox)itm.FindControl("txtAttDSalary");
                    TextBox txtAttDApplicableSalary = (TextBox)itm.FindControl("txtAttDApplicableSalary");

                    dtEmp.Rows.Add(Convert.ToInt32(hdnAttDId.Value), Convert.ToInt32(hdnAttDEmployeeId.Value),
                        Convert.ToInt32(hdnFromExcel.Value), lblAttDEmployeeName.Text, Convert.ToInt32(txtAttDTotWorkingDays.Text),
                    Convert.ToDecimal(txtAttDEmpAttendedDays.Text), Convert.ToInt32(txtAttDOTAtWorking.Text), Convert.ToInt32(txtAttDOTAtWeekend.Text),
                    Convert.ToInt32(txtAttDOTAtHoliday.Text), Convert.ToDecimal(txtAttDSalary.Text), Convert.ToDecimal(txtAttDApplicableSalary.Text));

                }
            }

            if (drpEmployee.SelectedValue != "" && txtEmpAttendedDays.Text != "" & txtSalary.Text!="")
            {
                dtEmp.Rows.Add(Convert.ToInt32(hdn_AttDetailId.Value),  Convert.ToInt32(drpEmployee.SelectedValue),
                    0, drpEmployee.Text, Convert.ToInt32(txtTotWorkingDays.Text),
                       Convert.ToDecimal(txtEmpAttendedDays.Text), Convert.ToInt32(txtOTAtWorking.Text), Convert.ToInt32(txtOTAtWeekend.Text),
                    Convert.ToInt32(txtOTAtHoliday.Text), Convert.ToDecimal(txtSalary.Text), Convert.ToDecimal(txtApplicableSalary.Text));
            }
            rpt_Item_list.DataSource = dtEmp;
            rpt_Item_list.DataBind();
            ClearEmployeeDetail();
            drpEmployee.Focus();
            fillEmployee();
            Upd_ItemList.Update();
        }

        protected void btn_remove_line_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            DataTable dtEmp = new DataTable();
            dtEmp.Columns.Add("Id", typeof(int));
            dtEmp.Columns.Add("EmployeeId", typeof(int));
            dtEmp.Columns.Add("FromExcel", typeof(int));
            dtEmp.Columns.Add("EmployeeName", typeof(string));
            dtEmp.Columns.Add("TotalWorkingDays", typeof(int));
            dtEmp.Columns.Add("EmployeeWorkedDays", typeof(decimal));
            dtEmp.Columns.Add("OTAtWorking", typeof(int));
            dtEmp.Columns.Add("OTAtWeekend", typeof(int));
            dtEmp.Columns.Add("OTAtHoliday", typeof(int));
            dtEmp.Columns.Add("Salary", typeof(decimal));
            dtEmp.Columns.Add("ApplicableSalary", typeof(decimal));


            if (rpt_Item_list.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_Item_list.Items)
                {
                    HiddenField hdnAttDId = (HiddenField)itm.FindControl("hdnAttDId");
                    HiddenField hdnAttDEmployeeId = (HiddenField)itm.FindControl("hdnAttDEmployeeId");
                    HiddenField hdnFromExcel = (HiddenField)itm.FindControl("hdnFromExcel");
                    Label lblAttDEmployeeName = (Label)itm.FindControl("lblAttDEmployeeName");
                    TextBox txtAttDTotWorkingDays = (TextBox)itm.FindControl("txtAttDTotWorkingDays");
                    TextBox txtAttDEmpAttendedDays = (TextBox)itm.FindControl("txtAttDEmpAttendedDays");
                    TextBox txtAttDOTAtWorking = (TextBox)itm.FindControl("txtAttDOTAtWorking");
                    TextBox txtAttDOTAtWeekend = (TextBox)itm.FindControl("txtAttDOTAtWeekend");
                    TextBox txtAttDOTAtHoliday = (TextBox)itm.FindControl("txtAttDOTAtHoliday");
                    TextBox txtAttDSalary = (TextBox)itm.FindControl("txtAttDSalary");
                    TextBox txtAttDApplicableSalary = (TextBox)itm.FindControl("txtAttDApplicableSalary");

                    //if (txtAttDEmpAttendedDays.Text != "")
                    dtEmp.Rows.Add(Convert.ToInt32(hdnAttDId.Value), Convert.ToInt32(hdnAttDEmployeeId.Value),
                        Convert.ToInt32(hdnFromExcel.Value), lblAttDEmployeeName.Text, Convert.ToInt32(txtAttDTotWorkingDays.Text),
                        txtAttDEmpAttendedDays.Text == "" ? 0 : Convert.ToDecimal(txtAttDEmpAttendedDays.Text), Convert.ToInt32(txtAttDOTAtWorking.Text), Convert.ToInt32(txtAttDOTAtWeekend.Text),
                    Convert.ToInt32(txtAttDOTAtHoliday.Text), Convert.ToDecimal(txtAttDSalary.Text), txtAttDApplicableSalary.Text==""?0: Convert.ToDecimal(txtAttDApplicableSalary.Text));

                }
            }

            dtEmp.Rows.RemoveAt(itemrp.ItemIndex);
            rpt_Item_list.DataSource = dtEmp;
            rpt_Item_list.DataBind();

            lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
            fillEmployee();
            Upd_ItemList.Update();
        }

        /*Edit Item*/
        protected void btn_edit_line_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            HiddenField hdnAttDIdP = (HiddenField)itemrp.FindControl("hdnAttDId");
            HiddenField hdnAttDEmployeeIdP = (HiddenField)itemrp.FindControl("hdnAttDEmployeeId");
            Label lblAttDEmployeeNameP = (Label)itemrp.FindControl("lblAttDEmployeeName");
            TextBox txtAttDTotWorkingDaysP = (TextBox)itemrp.FindControl("txtAttDTotWorkingDays");
            TextBox txtAttDEmpAttendedDaysP = (TextBox)itemrp.FindControl("txtAttDEmpAttendedDays");
            TextBox txtAttDOTAtWorkingP = (TextBox)itemrp.FindControl("txtAttDOTAtWorking");
            TextBox txtAttDOTAtWeekendP = (TextBox)itemrp.FindControl("txtAttDOTAtWeekend");
            TextBox txtAttDOTAtHolidayP = (TextBox)itemrp.FindControl("txtAttDOTAtHoliday");
            TextBox txtAttDSalaryP = (TextBox)itemrp.FindControl("txtAttDSalary");
            TextBox txtAttDApplicableSalaryP = (TextBox)itemrp.FindControl("txtAttDApplicableSalary");
            ClearEmployeeDetail();
            fillEmployeeEdit(hdnAttDEmployeeIdP.Value, lblAttDEmployeeNameP.Text);

            hdn_AttDetailId.Value = hdnAttDIdP.Value;
            drpEmployee.SelectedValue = hdnAttDEmployeeIdP.Value;
            txtTotWorkingDays.Text= txtAttDTotWorkingDaysP.Text;
            txtEmpAttendedDays.Text= txtAttDEmpAttendedDaysP.Text;
            txtOTAtWorking.Text= txtAttDOTAtWorkingP.Text;
            txtOTAtWeekend.Text= txtAttDOTAtWeekendP.Text;
            txtOTAtHoliday.Text= txtAttDOTAtHolidayP.Text;
            txtSalary.Text= txtAttDSalaryP.Text;
            txtApplicableSalary.Text= txtAttDApplicableSalaryP.Text;


            DataTable dtEmp = new DataTable();
            dtEmp.Columns.Add("Id", typeof(int));
            dtEmp.Columns.Add("EmployeeId", typeof(int));
            dtEmp.Columns.Add("FromExcel", typeof(int));
            dtEmp.Columns.Add("EmployeeName", typeof(string));
            dtEmp.Columns.Add("TotalWorkingDays", typeof(int));
            dtEmp.Columns.Add("EmployeeWorkedDays", typeof(decimal));
            dtEmp.Columns.Add("OTAtWorking", typeof(int));
            dtEmp.Columns.Add("OTAtWeekend", typeof(int));
            dtEmp.Columns.Add("OTAtHoliday", typeof(int));
            dtEmp.Columns.Add("Salary", typeof(decimal));
            dtEmp.Columns.Add("ApplicableSalary", typeof(decimal));


            if (rpt_Item_list.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_Item_list.Items)
                {
                    HiddenField hdnAttDId = (HiddenField)itm.FindControl("hdnAttDId");
                    HiddenField hdnAttDEmployeeId = (HiddenField)itm.FindControl("hdnAttDEmployeeId");
                    HiddenField hdnFromExcel = (HiddenField)itm.FindControl("hdnFromExcel");
                    Label lblAttDEmployeeName = (Label)itm.FindControl("lblAttDEmployeeName");
                    TextBox txtAttDTotWorkingDays = (TextBox)itm.FindControl("txtAttDTotWorkingDays");
                    TextBox txtAttDEmpAttendedDays = (TextBox)itm.FindControl("txtAttDEmpAttendedDays");
                    TextBox txtAttDOTAtWorking = (TextBox)itm.FindControl("txtAttDOTAtWorking");
                    TextBox txtAttDOTAtWeekend = (TextBox)itm.FindControl("txtAttDOTAtWeekend");
                    TextBox txtAttDOTAtHoliday = (TextBox)itm.FindControl("txtAttDOTAtHoliday");
                    TextBox txtAttDSalary = (TextBox)itm.FindControl("txtAttDSalary");
                    TextBox txtAttDApplicableSalary = (TextBox)itm.FindControl("txtAttDApplicableSalary");

                     if (txtAttDEmpAttendedDays.Text!="")
                    dtEmp.Rows.Add(Convert.ToInt32(hdnAttDId.Value), Convert.ToInt32(hdnAttDEmployeeId.Value),
                        Convert.ToInt32(hdnFromExcel.Value), lblAttDEmployeeName.Text, Convert.ToInt32(txtAttDTotWorkingDays.Text),
                    Convert.ToDecimal(txtAttDEmpAttendedDays.Text), Convert.ToInt32(txtAttDOTAtWorking.Text), Convert.ToInt32(txtAttDOTAtWeekend.Text),
                    Convert.ToInt32(txtAttDOTAtHoliday.Text), Convert.ToDecimal(txtAttDSalary.Text), Convert.ToDecimal(txtAttDApplicableSalary.Text));

                }
            }

            dtEmp.Rows.RemoveAt(itemrp.ItemIndex);
            rpt_Item_list.DataSource = dtEmp;
            rpt_Item_list.DataBind();

            lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
            Upd_ItemList.Update();
        }

        public void ClearEmployeeDetail()
        {
            lblRepeaterSNo.Text = (rpt_Item_list.Items.Count + 1).ToString();
            hdn_AttDetailId.Value = "-" + (rpt_Item_list.Items.Count + 1).ToString();
            drpEmployee.ClearSelection();
            drpEmployee.Text = "";

            drpEmployeeOnSelectedIndexChanged(null,null);
        }

        protected void rpt_Item_list_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Button btn_edit_line = (Button)e.Item.FindControl("btn_edit_line");
                Button btn_remove_line = (Button)e.Item.FindControl("btn_remove_line");
                HiddenField hdnFromExcel = (HiddenField)e.Item.FindControl("hdnFromExcel");
                btn_edit_line.Visible = btn_remove_line.Visible = (hdnFromExcel.Value == "0") ? true : false;
            }
        }

        protected void drpEmployeeOnSelectedIndexChanged(object sender, EventArgs e)
        {
            txtTotWorkingDays.Text = "";
            txtEmpAttendedDays.Text = "";
            txtOTAtWorking.Text = "";
            txtOTAtWeekend.Text = "";
            txtOTAtHoliday.Text = "";
            txtSalary.Text = "";
            txtApplicableSalary.Text = "";
            if (drpEmployee.SelectedValue != "" && drpMonth.SelectedValue!="" && drpYear.SelectedValue!="")
            {
                DataTable dt = obj_trans.GetEmployeeSalary(Convert.ToInt32(drpEmployee.SelectedValue), Convert.ToInt32(drpMonth.SelectedValue), Convert.ToInt32(drpYear.SelectedValue));
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Result"].ToString() == "1")
                    {
                        txtTotWorkingDays.Text = dt.Rows[0]["TotalWorkingDays"].ToString();
                        txtSalary.Text = dt.Rows[0]["Total_Salary"].ToString();
                        txtOTAtWorking.Text = "0";
                        txtOTAtWeekend.Text = "0";
                        txtOTAtHoliday.Text = "0";
                    }else if(dt.Rows[0]["Result"].ToString() == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please Provide Employee Salary');", true);
                    }
                }
            }
            UpdTotWorkingDays.Update();
            UpdEmpAttendedDays.Update();
            UpdOTAtWorking.Update();
            UpdOTAtWeekend.Update();
            UpdOTAtHoliday.Update();
            UpdSalary.Update();
            UpdApplicableSalary.Update();
        }

        public void fillEmployee()
        {
            DataTable dtEmp = new DataTable();
            dtEmp.Columns.Add("EmployeeId", typeof(int));

            if (rpt_Item_list.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_Item_list.Items)
                {
                    HiddenField hdnAttDEmployeeId = (HiddenField)itm.FindControl("hdnAttDEmployeeId");

                    dtEmp.Rows.Add(Convert.ToInt32(hdnAttDEmployeeId.Value));

                }
            }

            drpEmployee.Items.Clear();
            DataTable dt = obj_trans.GetEmployeeListForAttendance(dtEmp);
            drpEmployee.DataSource = dt;
            drpEmployee.DataTextField = "Text";
            drpEmployee.DataValueField = "Value";
            drpEmployee.DataBind();
        }
        public void fillEmployeeEdit(string Id,string Name)
        {
            RadComboBoxItem CodeItem;
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = Name;
            CodeItem.Value = Id;
            drpEmployee.Items.Add(CodeItem);
        }
        /*Save*/
        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            DataTable dt_details = fill_Detail();

            int res = 0;
            if (dt_details.Rows.Count > 0)
            {
                res = obj_trans.Insert_Update_EmployeeAttendance(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(drpMonth.SelectedValue),
                    Convert.ToInt32(drpYear.SelectedValue), hdnfileName.Value, hdnfileSaveName.Value, hdnfileExtension.Value,
                    dt_details, Convert.ToInt32(hdn_user_id.Value));
            }
            else
            {
                lbl_msgin.Text = "Add Employee Attendance to Continue !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            if (res > 0)
            {
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


        /*Data To Save*/
        public DataTable fill_Detail()
        {
            DataTable dtEmp = new DataTable();
            dtEmp.Columns.Add("Id", typeof(int));
            dtEmp.Columns.Add("EmployeeId", typeof(int));
            dtEmp.Columns.Add("FromExcel", typeof(int));
            dtEmp.Columns.Add("TotalWorkingDays", typeof(int));
            dtEmp.Columns.Add("EmployeeWorkedDays", typeof(decimal));
            dtEmp.Columns.Add("OTAtWorking", typeof(int));
            dtEmp.Columns.Add("OTAtWeekend", typeof(int));
            dtEmp.Columns.Add("OTAtHoliday", typeof(int));
            dtEmp.Columns.Add("Salary", typeof(decimal));
            dtEmp.Columns.Add("ApplicableSalary", typeof(decimal));

            if (rpt_Item_list.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_Item_list.Items)
                {
                    HiddenField hdnAttDId = (HiddenField)itm.FindControl("hdnAttDId");
                    HiddenField hdnAttDEmployeeId = (HiddenField)itm.FindControl("hdnAttDEmployeeId");
                    HiddenField hdnFromExcel = (HiddenField)itm.FindControl("hdnFromExcel");
                    TextBox txtAttDTotWorkingDays = (TextBox)itm.FindControl("txtAttDTotWorkingDays");
                    TextBox txtAttDEmpAttendedDays = (TextBox)itm.FindControl("txtAttDEmpAttendedDays");
                    TextBox txtAttDOTAtWorking = (TextBox)itm.FindControl("txtAttDOTAtWorking");
                    TextBox txtAttDOTAtWeekend = (TextBox)itm.FindControl("txtAttDOTAtWeekend");
                    TextBox txtAttDOTAtHoliday = (TextBox)itm.FindControl("txtAttDOTAtHoliday");
                    TextBox txtAttDSalary = (TextBox)itm.FindControl("txtAttDSalary");
                    TextBox txtAttDApplicableSalary = (TextBox)itm.FindControl("txtAttDApplicableSalary");

                    if (txtAttDEmpAttendedDays.Text!="")
                    dtEmp.Rows.Add(Convert.ToInt32(hdnAttDId.Value), Convert.ToInt32(hdnAttDEmployeeId.Value),
                        Convert.ToInt32(hdnFromExcel.Value), Convert.ToInt32(txtAttDTotWorkingDays.Text),
                    Convert.ToDecimal(txtAttDEmpAttendedDays.Text), Convert.ToInt32(txtAttDOTAtWorking.Text), Convert.ToInt32(txtAttDOTAtWeekend.Text),
                    Convert.ToInt32(txtAttDOTAtHoliday.Text), Convert.ToDecimal(txtAttDSalary.Text), Convert.ToDecimal(txtAttDApplicableSalary.Text));

                }
            }
            if (drpEmployee.SelectedValue != "" && txtEmpAttendedDays.Text != "" & txtSalary.Text != "")
            {
                dtEmp.Rows.Add(Convert.ToInt32(hdn_AttDetailId.Value), Convert.ToInt32(drpEmployee.SelectedValue),
                    0, Convert.ToInt32(txtTotWorkingDays.Text),
                       Convert.ToDecimal(txtEmpAttendedDays.Text), Convert.ToInt32(txtOTAtWorking.Text), Convert.ToInt32(txtOTAtWeekend.Text),
                    Convert.ToInt32(txtOTAtHoliday.Text), Convert.ToDecimal(txtSalary.Text), Convert.ToDecimal(txtApplicableSalary.Text));
            }

            return dtEmp;
        }

        protected void btnDelete_OnClick(object sender, EventArgs e)
        {
            int res = obj_trans.Delete_EmployeeAttendance(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_user_id.Value));
            if (res > 0)
            {
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
            //pnl_add.Visible = false;
            Upd_Add_PanelInner.Update();
        }

        /*Reset*/
        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
        }

        public void fuDocumentOnFileUploaded(object sender, FileUploadedEventArgs e)
        {

            fuDocument.TargetFolder = "~/UploadedFiles";
            DataTable dt = obj_common.Get_File_Code("Employee Attendance Document");
            if (dt.Rows.Count > 0 )
            {
                hdnfileSaveName.Value =dt.Rows[0][0].ToString()+e.File.GetNameWithoutExtension()+e.File.GetExtension();
                hdnfileName.Value = e.File.GetNameWithoutExtension() + e.File.GetExtension();
                hdnfileExtension.Value= e.File.GetExtension();
                e.File.SaveAs(Path.Combine(Server.MapPath(fuDocument.TargetFolder), hdnfileSaveName.Value));
                 
            }

            UpdDocument.Update();
        }

        public DataTable ReadFile(string filename, string extrn)
        {
            string connString = "";
            DataTable ContentTable = null;
            ContentTable = new DataTable();
            ContentTable.Columns.Add("EmployeeId", typeof(string));
            ContentTable.Columns.Add("Date", typeof(DateTime));
            //ContentTable.Columns.Add("TotalInTime", typeof(string));
            //ContentTable.Columns.Add("OT", typeof(string));
            //ContentTable.Columns.Add("UT", typeof(string));
            ContentTable.Columns.Add("Absent", typeof(string));
            if (extrn == ".xls")
            {
                //Connectionstring for excel v8.0    
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
            }
            else
            {
                //Connectionstring fo excel v12.0    
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1\"";
            }
            OleDbConnection OledbConn = new OleDbConnection(connString);
            try
            {

                OleDbCommand OledbCmd = new OleDbCommand();
                OledbCmd.Connection = OledbConn;
                OledbConn.Open();
                var sheetNames = OledbConn.GetSchema("Tables");
                
                OledbCmd.CommandText = "Select * from ["+ sheetNames.Rows[0]["TABLE_NAME"].ToString() + "]";
                OleDbDataReader dr = OledbCmd.ExecuteReader();
                
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (dr["Emp No#"].ToString().Trim() != string.Empty && dr["Date"].ToString().Trim() != string.Empty && dr["Emp No#"].ToString().Trim() != " " && dr["Date"].ToString().Trim() != " ")
                            ContentTable.Rows.Add(dr["Emp No#"].ToString().Trim(), dr["Date"].ToString().Trim(), dr["Absent"].ToString().Trim());

                    }
                }
                dr.Close();

                OledbConn.Close();
                return ContentTable;
            }
            catch (Exception ex)
            {
                //OledbConn.Close();
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please upload the correct file format');", true);
                return ContentTable;
            }
        }

        protected void btnProcessOnClick(object sender, EventArgs e)
        {
            if (hdnfileSaveName.Value != "")
            {
                DataSet ds = obj_trans.GetEmployeeDetailFromExcel(Convert.ToInt32(drpMonth.SelectedValue), Convert.ToInt32(drpYear.SelectedValue), ReadFile(Path.Combine(Server.MapPath("~/UploadedFiles"), hdnfileSaveName.Value), hdnfileExtension.Value));
                DataTable dtEmp = ds.Tables[0];
                DataTable dtResult = ds.Tables[1];
                if (rpt_Item_list.Items.Count > 0)
                {
                    foreach (RepeaterItem itm in rpt_Item_list.Items)
                    {
                        HiddenField hdnAttDId = (HiddenField)itm.FindControl("hdnAttDId");
                        HiddenField hdnAttDEmployeeId = (HiddenField)itm.FindControl("hdnAttDEmployeeId");
                        HiddenField hdnFromExcel = (HiddenField)itm.FindControl("hdnFromExcel");
                        Label lblAttDEmployeeName = (Label)itm.FindControl("lblAttDEmployeeName");
                        TextBox txtAttDTotWorkingDays = (TextBox)itm.FindControl("txtAttDTotWorkingDays");
                        TextBox txtAttDEmpAttendedDays = (TextBox)itm.FindControl("txtAttDEmpAttendedDays");
                        TextBox txtAttDOTAtWorking = (TextBox)itm.FindControl("txtAttDOTAtWorking");
                        TextBox txtAttDOTAtWeekend = (TextBox)itm.FindControl("txtAttDOTAtWeekend");
                        TextBox txtAttDOTAtHoliday = (TextBox)itm.FindControl("txtAttDOTAtHoliday");
                        TextBox txtAttDSalary = (TextBox)itm.FindControl("txtAttDSalary");
                        TextBox txtAttDApplicableSalary = (TextBox)itm.FindControl("txtAttDApplicableSalary");

                        if (hdnFromExcel.Value == "0" && dtEmp.Select("EmployeeId='"+ hdnAttDEmployeeId .Value+ "'").Length==0)
                        {
                            dtEmp.Rows.Add(Convert.ToInt32(hdnAttDId.Value), Convert.ToInt32(hdnAttDEmployeeId.Value),
                                Convert.ToInt32(hdnFromExcel.Value), lblAttDEmployeeName.Text, Convert.ToInt32(txtAttDTotWorkingDays.Text),
                            Convert.ToInt32(txtAttDEmpAttendedDays.Text), Convert.ToInt32(txtAttDOTAtWorking.Text), Convert.ToInt32(txtAttDOTAtWeekend.Text),
                            Convert.ToInt32(txtAttDOTAtHoliday.Text), Convert.ToDecimal(txtAttDSalary.Text), Convert.ToDecimal(txtAttDApplicableSalary.Text));
                        }

                    }
                }

                rpt_Item_list.DataSource = dtEmp;
                rpt_Item_list.DataBind();
                fillEmployee();
                ClearEmployeeDetail();

                if (dtResult.Rows[0]["Result"].ToString() == "0")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Some employee salary detail is missing');", true);
                }
                Upd_ItemList.Update();

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please upload the file');", true);
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

        protected void btn_newentry_OnClick(object sender, EventArgs e)
        {
            Clear();
            pnl_add.Visible = true;
            Upd_Add_Panel.Update();
        }

        /*Clear All the Data*/
        public void Clear()
        {
            hdn_id.Value = "0";
            drpMonth.ClearSelection();
            drpMonth.Text = "";
            drpYear.ClearSelection();
            drpYear.Text = "";
            drpMonth.Enabled = true;
            drpYear.Enabled = true;
            hdnfileName.Value = "";
            hdnfileSaveName.Value = "";
            hdnfileExtension.Value = "";
            DataTable dtEmp = new DataTable();
            dtEmp.Columns.Add("Id", typeof(int));
            dtEmp.Columns.Add("EmployeeId", typeof(int));
            dtEmp.Columns.Add("FromExcel", typeof(int));
            dtEmp.Columns.Add("EmployeeName", typeof(string));
            dtEmp.Columns.Add("TotalWorkingDays", typeof(int));
            dtEmp.Columns.Add("EmployeeWorkedDays", typeof(int));
            dtEmp.Columns.Add("OTAtWorking", typeof(int));
            dtEmp.Columns.Add("OTAtWeekend", typeof(int));
            dtEmp.Columns.Add("OTAtHoliday", typeof(int));
            dtEmp.Columns.Add("Salary", typeof(decimal));
            dtEmp.Columns.Add("ApplicableSalary", typeof(decimal));

            rpt_Item_list.DataSource = dtEmp;
            rpt_Item_list.DataBind();
            ClearEmployeeDetail();
            fillEmployee();
            btn_save.Visible = hdn_add.Value == "0" ? false : true;
            btnDelete.Visible = false;
            Get_Code();

            Upd_Add_PanelInner.Update();
        }

        /*Code for Display*/
        public void Get_Code()
        {
            DataTable dt = obj_common.Get_Code(46);
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
                    DataTable dt = obj_common.Action_Previlage_Validation(46, Convert.ToInt32(hdn_user_id.Value));
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

        /*Check Form Privilege*/
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.Form_Previlage_Validation(46, Convert.ToInt32(hdn_user_id.Value));
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