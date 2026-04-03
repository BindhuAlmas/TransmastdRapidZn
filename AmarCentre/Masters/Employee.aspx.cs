using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AmarCentre.BAL;
using System.Data;
using System.IO;
using System.Globalization;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

namespace AmarCentre.Masters
{
    public partial class Employee : System.Web.UI.Page
    {
        Master_Bal obj_master = new Master_Bal();
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
                filldrops();
                fill_Reporting(Convert.ToInt32(hdn_id.Value));
                //filldefaultAccount();

                Clear();
                
                grid_fill(1, 10, "", "", "");

                txt_password.Attributes["value"] = string.Empty;
                txt_password.Attributes["Text"] = string.Empty;
                txt_userName.Attributes["value"] = null;
                txt_password.Attributes.Add("value", null);

            }
            if (IsPostBack)
            {
                txt_password.Attributes["value"] = txt_password.Text;
            }
            
        }

        public void filldrops()
        {
            DataSet ds = obj_master.drpforEmployee();

            drp_pettyCash.Items.Clear();
            drp_pettyCash.DataSource = ds.Tables[0];
            drp_pettyCash.DataTextField = "text";
            drp_pettyCash.DataValueField = "value";
            drp_pettyCash.DataBind();

            drpBankAccount.Items.Clear();
            drpBankAccount.DataSource = ds.Tables[1];
            drpBankAccount.DataTextField = "text";
            drpBankAccount.DataValueField = "value";
            drpBankAccount.DataBind();

            drpLoanAccount.Items.Clear();
            drpLoanAccount.DataSource = ds.Tables[2];
            drpLoanAccount.DataTextField = "text";
            drpLoanAccount.DataValueField = "value";
            drpLoanAccount.DataBind();

            drp_Design.Items.Clear();
            drp_Design.DataSource = ds.Tables[3];
            drp_Design.DataTextField = "text";
            drp_Design.DataValueField = "value";
            drp_Design.DataBind();

            drp_doc.Items.Clear();
            drp_doc.DataSource = ds.Tables[4];
            drp_doc.DataTextField = "Text";
            drp_doc.DataValueField = "Value";
            drp_doc.DataBind();
            drp_doc.Text = "";

            drpDepartment.Items.Clear();
            drpDepartment.DataSource = ds.Tables[5];
            drpDepartment.DataTextField = "Text";
            drpDepartment.DataValueField = "Value";
            drpDepartment.DataBind();
            drpDepartment.Text = "";
        }
        public void fuProfilePhoto_OnFileUploaded(object sender, FileUploadedEventArgs e)
        {
            DataTable dt = obj_common.Get_File_Code("EmpProfile");
            if (dt.Rows.Count > 0 & dt.Rows[0][0].ToString() != "")
            {
                fuProfilePhoto.TargetFolder = "~/UploadedImage";

                string files_name = dt.Rows[0][0].ToString() + e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();
                e.File.SaveAs(Path.Combine(Server.MapPath(fuProfilePhoto.TargetFolder), files_name));

                try
                {
                    //in backup folder also
                    DataTable dtgen = obj_master.Edit_GeneralSettings();
                    File.Copy((Path.Combine(Server.MapPath(fu_documents.TargetFolder), files_name)),
                        (Path.Combine(dtgen.Rows[0]["BackupDrivePath"].ToString() + "UploadedImage", files_name)), false);
                }
                catch (Exception cc) { }

                hdn_photo.Value = e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();
                hdn_photo_save.Value = files_name;
                Session["ProfilePhotoSave"] = hdn_photo_save.Value;
            }
            UpdProfilePhoto.Update();
        }
     
        public void fill_Reporting(int PresentEmpId)
        {
            drp_Reporting.Items.Clear();
            DataTable dt = obj_master.Drp_Reporting(PresentEmpId, Convert.ToInt32(hdn_id.Value));
            drp_Reporting.DataSource = dt;
            drp_Reporting.DataTextField = "text";
            drp_Reporting.DataValueField = "value";
            drp_Reporting.DataBind();
        }

        public void filldefaultAccount()
        {
            drp_accQrec.Items.Clear();

            DataTable dtgen = obj_master.Edit_GeneralSettings();
            if (dtgen.Rows[0]["DefaultPayModeInQuickReceipt"].ToString() == "1") //petty
            {
                drp_accQrec.DataSource = obj_master.Drp_PettyCashAccount();
                drp_accQrec.DataTextField = "text";
                drp_accQrec.DataValueField = "value";
                drp_accQrec.DataBind();
            }
            else if (dtgen.Rows[0]["DefaultPayModeInQuickReceipt"].ToString() == "2") //bank
            {
                drp_accQrec.DataSource = obj_master.DrpBankAccount();
                drp_accQrec.DataTextField = "text";
                drp_accQrec.DataValueField = "value";
                drp_accQrec.DataBind();
            }
            else if (dtgen.Rows[0]["DefaultPayModeInQuickReceipt"].ToString() == "3") //cheque
            {
                drp_accQrec.DataSource = obj_master.DrpBankAccount();
                drp_accQrec.DataTextField = "text";
                drp_accQrec.DataValueField = "value";
                drp_accQrec.DataBind();
            }
        }

        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_master.Get_List_Employee(page_number, page_size, filter, column, order);
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
            Upd_Nav_Panel.Update();
            Upd_List_Panel.Update();
        }

        //exel export
        public void btnexcel_export_OnClick(object sender, EventArgs e)
        {
            DataTable dt = obj_master.List_Employee_Excel(Convert.ToInt32(hdn_user_id.Value));
            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "EmployeeList");


                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                HttpContext.Current.Response.Write(style);
                Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        //rpt Command
        protected void rpt_list_OnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            Clear();
            HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdn_id");
            DataSet ds = obj_master.Edit_Employee(Convert.ToInt32(hdn_rpt_id.Value));
            DataTable dt = ds.Tables[0];
            DataTable dt_doc = ds.Tables[2];
            DataTable dt_pettyCashAccount = ds.Tables[3];
            DataTable dtBankAccount = ds.Tables[4];
            DataTable dtLoanAccount = ds.Tables[5];
            DataTable dtdepartment = ds.Tables[6];

            txt_userName.ReadOnly = true;
            txt_userName.Text = dt.Rows[0]["UserName"].ToString();
            txt_password.Text = dt.Rows[0]["Passwords"].ToString();
            txt_password.Attributes["value"] = dt.Rows[0]["Passwords"].ToString();
            txt_present_add.Text = dt.Rows[0]["PresentAddress"].ToString();
            txt_mobile.Text = dt.Rows[0]["MobileNum"].ToString();
            txt_phn.Text = dt.Rows[0]["Phone_number"].ToString();
            txt_email.Text = dt.Rows[0]["EmailId"].ToString();
            txtCode.Text = dt.Rows[0]["Code"].ToString();
            txt_name.Text = dt.Rows[0]["Name"].ToString();
            hdn_id.Value = dt.Rows[0]["id"].ToString();
            hdn_photo.Value = dt.Rows[0]["ProfilePhoto"].ToString();
            hdn_photo_save.Value = dt.Rows[0]["ProfilePhotoSave"].ToString();
            drp_Design.SelectedValue = dt.Rows[0]["DesignationId"].ToString();
            fill_Reporting(Convert.ToInt32(hdn_id.Value));
            drp_Reporting.SelectedValue = dt.Rows[0]["ReportingId"].ToString();
            chk_IsIncApp.Checked = Convert.ToBoolean(dt.Rows[0]["IncentiveApplicable"]);
            chkenable.Checked = Convert.ToBoolean(dt.Rows[0]["IsEnable"]);
            if (dt.Rows[0]["IncentiveApplicable"].ToString() == "True")
            {
                btnIncentive.Visible = hdnIncentive.Value == "0" ? false : true;
                Upd_IncApp_Panel1.Update();
            }
            //drpLanguage.SelectedValue = dt.Rows[0]["Language"].ToString();
            lbl_bal.Text = dt.Rows[0]["Balance"].ToString();
            drp_accQrec.SelectedValue = dt.Rows[0]["DefaultPaymentAccount"].ToString();

            img_profile.Visible = true;
            if (hdn_photo.Value != "")
                img_profile.ImageUrl = "~/UploadedImage/" + hdn_photo_save.Value;
            else
                img_profile.ImageUrl = "~/Images/defaultimage.png";
            drp_pettyCash.Text = string.Empty;
            drp_pettyCash.ClearCheckedItems();
            drpBankAccount.Text = string.Empty;
            drpBankAccount.ClearCheckedItems();
            drpLoanAccount.Text = string.Empty;
            drpLoanAccount.ClearCheckedItems();
            drpDepartment.Text = string.Empty;
            drpDepartment.ClearCheckedItems();

            foreach (DataRow dr in dt_pettyCashAccount.Rows)
            {
                RadComboBoxItem item = (RadComboBoxItem)(drp_pettyCash.FindItemByValue(dr["PettyCashAccountId"].ToString()));
                item.Selected = true;
                item.Checked = true;
            }
            foreach (DataRow dr in dtBankAccount.Rows)
            {
                RadComboBoxItem item = (RadComboBoxItem)(drpBankAccount.FindItemByValue(dr["BankAccountId"].ToString()));
                item.Selected = true;
                item.Checked = true;
            }
            foreach (DataRow dr in dtLoanAccount.Rows)
            {
                RadComboBoxItem item = (RadComboBoxItem)(drpLoanAccount.FindItemByValue(dr["LoanAccountId"].ToString()));
                item.Selected = true;
                item.Checked = true;
            }
            foreach (DataRow dr in dtdepartment.Rows)
            {
                RadComboBoxItem item = (RadComboBoxItem)(drpDepartment.FindItemByValue(dr["DepartmentId"].ToString()));
                item.Selected = true;
                item.Checked = true;
            }

            btn_save.Visible = hdn_update.Value == "0" ? false : true;
            btn_OB.Visible = hdn_OB.Value == "0" ? false : true;
            btn_doc.Visible = hdn_doc.Value == "0" ? false : true;
            btn_menu.Visible = hdn_menu.Value == "0" ? false : true;
            btn_other.Visible = hdn_other.Value == "0" ? false : true;

            if (hdn_user_id.Value == hdn_rpt_id.Value)
                btn_delete.Visible = false;
            else
                btn_delete.Visible = hdn_delete.Value == "0" ? false : true;

            DataTable dtgen = obj_master.Edit_GeneralSettings();
            hdnIncPerc.Value = dtgen.Rows[0]["IncentivePercentage"].ToString();
            if (hdnIncPerc.Value == "1")
            {
                th_incamt.Attributes.Add("style", "display:none");
                th_incperc.Attributes.Add("style", "display:table-cell");
                txtCommonAmount.Visible = false;
            }
            else
            {
                th_incperc.Attributes.Add("style", "display:none");
                th_incamt.Attributes.Add("style", "display:table-cell");
                txtCommonPer.Visible = false;
            }

            pnl_add.Visible = true;
            Upd_Add_Panel.Update();
        }

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            DataTable dt_pettyCashAccount = new DataTable();
            dt_pettyCashAccount.Columns.Add("PettyCashAccountId", typeof(int));
            DataTable dtBankAccount = new DataTable();
            dtBankAccount.Columns.Add("BankAccountId", typeof(int));
            DataTable dtLoanAccount = new DataTable();
            dtLoanAccount.Columns.Add("loanAccountId", typeof(int));
            DataTable dtdepartment = new DataTable();
            dtdepartment.Columns.Add("departmentid", typeof(int));

            foreach (RadComboBoxItem item in drp_pettyCash.CheckedItems)
            {
                dt_pettyCashAccount.Rows.Add(Convert.ToInt32(item.Value));
            }
            foreach (RadComboBoxItem item in drpBankAccount.CheckedItems)
            {
                dtBankAccount.Rows.Add(Convert.ToInt32(item.Value));
            }
            foreach (RadComboBoxItem item in drpLoanAccount.CheckedItems)
            {
                dtLoanAccount.Rows.Add(Convert.ToInt32(item.Value));
            }
            foreach (RadComboBoxItem item in drpDepartment.CheckedItems)
            {
                dtdepartment.Rows.Add(Convert.ToInt32(item.Value));
            }
            int res = obj_master.Insert_Update_Employee(Convert.ToInt32(hdn_id.Value),txtCode.Text, txt_name.Text,
                txt_userName.Text, txt_password.Text, txt_present_add.Text,txt_mobile.Text,
                txt_email.Text, hdn_photo.Value, hdn_photo_save.Value, txt_phn.Text,
                Convert.ToInt32(drp_Design.SelectedValue), drp_Reporting.SelectedValue==""?(int?)null:Convert.ToInt32(drp_Reporting.SelectedValue),
                Convert.ToInt32(chk_IsIncApp.Checked),1,
                dt_pettyCashAccount, dtBankAccount, Convert.ToInt32(hdn_user_id.Value), 
                drp_accQrec.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_accQrec.SelectedValue),
                Convert.ToInt32(chkenable.Checked), dtLoanAccount, dtdepartment);
            if (res == 1)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else if (res == 2)
            {
                lblerrormsg.Text = "User Name Already Exist !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
            }
            else
            {
                lblerrormsg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
            }
            pnl_add.Visible = false;

            Upd_Add_Panel.Update();
        }
        
        protected void btn_delete_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Delete_Employee(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                lbl_msg.Text = "Deleted Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lblerrormsg.Text = "Unable to delete. Entry may be used in transactions !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
            }
            pnl_add.Visible = false;

            Upd_Add_Panel.Update();
        }

        protected void chk_IsIncApp_OnCheckedChanged(object sender, EventArgs e)
        {
            if (chk_IsIncApp.Checked)
                btnIncentive.Visible = hdnIncentive.Value == "0" ? false : true;
            else
                btnIncentive.Visible = false;
            Upd_IncApp_Panel1.Update();
        }

        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
        }
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
        public void Clear()
        {
            hdn_id.Value = "0";
            txtCode.Text = "";
            txt_userName.Text = "";
            txt_password.Text = "";
            txt_password.Attributes["value"] = "";
            txt_userName.ReadOnly = false;
            img_profile.ImageUrl = "~/Images/defaultimage.png";
            txt_present_add.Text = "";
            txt_mobile.Text = "";
            txt_email.Text = "";
            txt_name.Text = "";
            hdn_photo.Value = "";
            hdn_photo_save.Value = "";
            drp_obType.ClearSelection();
            drp_obType.Text = "";
            txt_open_bal.Text = "";
            txt_phn.Text = "";
            drp_Design.ClearSelection();
            drp_Design.Text = "";
            fill_Reporting(Convert.ToInt32(hdn_id.Value));
            drp_Reporting.ClearSelection();
            drp_Reporting.Text = "";
           
            chk_IsIncApp.Checked = false;
            chk_IsIncApp_OnCheckedChanged(null,null);
            ob_date.DbSelectedDate = DateTime.Now;

            drp_pettyCash.Text = string.Empty;
            drp_pettyCash.ClearCheckedItems();
            drpBankAccount.Text = string.Empty;
            drpBankAccount.ClearCheckedItems();
            drpLoanAccount.Text = string.Empty;
            drpLoanAccount.ClearCheckedItems();
            drpDepartment.Text = string.Empty;
            drpDepartment.ClearCheckedItems();
            foreach (RadComboBoxItem itm in drp_pettyCash.Items)
                itm.Checked = true;
            foreach (RadComboBoxItem itm in drpBankAccount.Items)
                itm.Checked = true;
            foreach (RadComboBoxItem itm in drpLoanAccount.Items)
                itm.Checked = true;
            drp_accQrec.ClearSelection();
            drp_accQrec.Text = "";
            chkenable.Checked = true;

            lbl_bal.Text = "";
            btn_delete.Visible = false;
            btn_OB.Visible = false;
            btn_doc.Visible = false;
            btn_menu.Visible = false;
            btn_other.Visible = btnIncentive.Visible = false;
            btn_save.Visible = hdn_add.Value == "0" ? false : true;
            
            Upd_Add_PanelInner.Update();
        }

        #region Menu

        protected void btn_menu_OnClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ShowMenuForm(" + hdn_id.Value + ");", true);
        }

        #endregion

        #region Opening balance

        protected void btn_OB_OnClick(object sender, EventArgs e)
        {
            pnl_obalance.Visible = true;
            DataSet ds = obj_master.Edit_Employee(Convert.ToInt32(hdn_id.Value));
            DataTable dt = ds.Tables[0];

            drp_obType.SelectedValue = dt.Rows[0]["OpeningBalanceType"].ToString();
            txt_open_bal.Text = dt.Rows[0]["OBal"].ToString();
            if (dt.Rows[0]["ODate"].ToString() != "")
                ob_date.DbSelectedDate = dt.Rows[0]["ODate"].ToString();
            else
                ob_date.DbSelectedDate = DateTime.Now;

            Upd_OB_Panel.Update();
        }

        protected void btn_OBSave_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Update_OB_Employee(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(drp_obType.SelectedValue), Convert.ToDecimal(txt_open_bal.Text),
                DateTime.ParseExact(CalDate(ob_date), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                pnl_obalance.Visible = false;
            }
            else
            {
            }
            Upd_OB_Panel.Update();
        }

        protected void btn_close_ob_OnClick(object sender, EventArgs e)
        {
            pnl_obalance.Visible = false;
            Upd_OB_Panel.Update();
        }

        #endregion

        #region other details

        protected void btn_other_OnClick(object sender, EventArgs e)
        {
            pnlOther.Visible = true;
            DOJ.DbSelectedDate = null;
            DOB.DbSelectedDate = null;
            drpPrefssn.ClearSelection();
            drpPrefssn.Text = "";
            drp_nation.ClearSelection();
            drp_nation.Text = "";
            drp_pro_status.ClearSelection();
            drp_pro_status.Text = "";
            drp_cont.ClearSelection();
            drp_cont.Text = "";
            drp_gender.ClearSelection();
            drp_gender.Text = "";
            txt_mol.Text = "";

            drp_nation.DataSource = obj_master.Drp_GetEmpDrp().Tables[0];
            drp_nation.DataTextField = "Name";
            drp_nation.DataValueField = "Id";
            drp_nation.DataBind();

            drpPrefssn.DataSource = obj_master.Drp_GetEmpDrp().Tables[1];
            drpPrefssn.DataTextField = "Name";
            drpPrefssn.DataValueField = "Id";
            drpPrefssn.DataBind();

            DataTable dtemp = obj_master.EmpGetOtherDetail(Convert.ToInt32(hdn_id.Value));
            if (dtemp.Rows.Count > 0)
            {
                DOJ.DbSelectedDate = dtemp.Rows[0]["DateOfJoin"].ToString();
                DOB.DbSelectedDate = dtemp.Rows[0]["DateOfBirth"].ToString();
                drpPrefssn.SelectedValue = dtemp.Rows[0]["ProfessionId"].ToString();
                drp_nation.SelectedValue = dtemp.Rows[0]["NatiionalityId"].ToString();
                drp_pro_status.SelectedValue = dtemp.Rows[0]["ProbationStatus"].ToString();
                drp_cont.SelectedValue = dtemp.Rows[0]["ContractType"].ToString();
                drp_gender.SelectedValue = dtemp.Rows[0]["Gender"].ToString();
                txt_mol.Text = dtemp.Rows[0]["MOL"].ToString();
            }

            Upd_otherD.Update();
        }

        protected void btn_close_other_OnClick(object sender, EventArgs e)
        {
            pnlOther.Visible = false;
            Upd_otherD.Update();
        }

        protected void btn_otherSave_OnClick(object sender, EventArgs e)
        {
            if (DOJ.DbSelectedDate != null || DOB.DbSelectedDate != null || drpPrefssn.Text != "" || drp_nation.Text != "" || drp_pro_status.SelectedValue != "" ||
                drp_cont.SelectedValue != "" || drp_gender.SelectedValue != "" || txt_mol.Text != "")
            {
                obj_master.Update_EmployeeOtherDetail(Convert.ToInt32(hdn_id.Value), DOJ.SelectedDate, DOB.SelectedDate,
                     drpPrefssn.Text, drpPrefssn.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpPrefssn.SelectedValue),
                     drp_nation.Text, drp_nation.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_nation.SelectedValue),
                     drp_pro_status.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_pro_status.SelectedValue),
                     drp_cont.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_cont.SelectedValue),
                     drp_gender.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_gender.SelectedValue), txt_mol.Text);
            }

            pnlOther.Visible = false;
            Upd_otherD.Update();
        }

        #endregion

        #region Applicable Leave

        protected void btnApplicableLeave_OnClick(object sender, EventArgs e)
        {
            pnlApplicableLeave.Visible = true;
            DataTable dt = obj_master.Get_List_EmployeeApplicableLeave(Convert.ToInt32(hdn_id.Value));
            rpt_ApplicableLeave.DataSource = dt;
            rpt_ApplicableLeave.DataBind();

            UpdApplicableLeave.Update();
        }

        protected void btnApplicableLeaveClose_OnClick(object sender, EventArgs e)
        {
            pnlApplicableLeave.Visible = false;
            UpdApplicableLeave.Update();
        }

        protected void btnApplicableLeaveSave_OnClick(object sender, EventArgs e)
        {
            int res = 0;
            DataTable dt_ApplicableLeave = fill_ApplicableLeave();
            if (dt_ApplicableLeave.Rows.Count > 0)
                res = obj_master.Insert_Update_EmployeeApplicableLeave(Convert.ToInt32(hdn_id.Value),
                dt_ApplicableLeave, Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
                pnlApplicableLeave.Visible = false;
            UpdApplicableLeave.Update();
        }

        public DataTable fill_ApplicableLeave()
        {
            DataTable dt_ApplicableLeave = new DataTable();
            dt_ApplicableLeave.Columns.Add("EALId", typeof(int));
            dt_ApplicableLeave.Columns.Add("LeaveId", typeof(int));
            dt_ApplicableLeave.Columns.Add("ApplicableLeave", typeof(int));

            if (rpt_ApplicableLeave.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_ApplicableLeave.Items)
                {
                    HiddenField hdn_EALId = (HiddenField)itm.FindControl("hdn_EALId");
                    HiddenField hdn_LeaveId = (HiddenField)itm.FindControl("hdn_LeaveId");
                    TextBox txtApplicableLeave = (TextBox)itm.FindControl("txtApplicableLeave");


                    dt_ApplicableLeave.Rows.Add(Convert.ToInt32(hdn_EALId.Value),
                        Convert.ToInt32(hdn_LeaveId.Value), Convert.ToInt32(txtApplicableLeave.Text));
                }
            }
            return dt_ApplicableLeave;
        }

        #endregion

        #region Document Upload

        protected void btn_docadd_OnClick(object sender, EventArgs e)
        {
            pnl_document.Visible = true;
            Clear_documnt();
            DataSet ds = obj_master.Edit_Employee(Convert.ToInt32(hdn_id.Value));
            DataTable dt_doc = ds.Tables[2];
            Session["dtEmpdoc"] = dt_doc;
            fill_rpt(dt_doc, 1, Convert.ToInt32(drp_countD.SelectedValue));
            Upd_Document_Panel.Update();
        }
        protected void btn_Docclose_OnClick(object sender, EventArgs e)
        {
            pnl_document.Visible = false;
            Upd_Document_Panel.Update();
        }

        public void Clear_documnt()
        {
            drp_doc.ClearSelection();
            drp_doc.Text = "";
            valid_from.SelectedDate = null;
            valid_to.SelectedDate = null;
            hdn_doc_name.Value = "";
            lab_doc_name_out.Text = "";
            hdn_doc_sav.Value = "";
            txt_doc_no.Text = "";
            hdn_doc_index_Id.Value = "0";
            txtValidityyr.Text = "";
        }

        protected void txtValidityyr_TextChanged(object sender, EventArgs e)
        {
            DateTime? Expirydate = null;
            if (txtValidityyr.Text != "" && valid_from.DbSelectedDate != null)
            {
                Expirydate = valid_from.SelectedDate.Value.AddYears(Convert.ToInt32(txtValidityyr.Text));
                valid_to.SelectedDate = Expirydate.Value.AddDays(-1);
                updVTo.Update();
            }
        }

        protected void btn_DocSave_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Update_EmpDoc(Convert.ToInt32(hdn_id.Value), fill_Detail(), Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                pnl_document.Visible = false;
            }
            else
            {
            }
            Upd_Document_Panel.Update();
        }

        public void fu_documents_OnFileUploaded(object sender, FileUploadedEventArgs e)
        {
            fu_documents.TargetFolder = "~/UploadedFiles";
            DataTable dt = obj_common.Get_File_Code("EmpDoc");
            if (dt.Rows.Count > 0 & dt.Rows[0][0].ToString() != "")
            {
                string files_name = dt.Rows[0][0].ToString() + e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();
                e.File.SaveAs(Path.Combine(Server.MapPath(fu_documents.TargetFolder), files_name));

                try
                {
                    //in backup folder also
                    DataTable dtgen = obj_master.Edit_GeneralSettings();
                    File.Copy((Path.Combine(Server.MapPath(fu_documents.TargetFolder), files_name)),
                        (Path.Combine(dtgen.Rows[0]["BackupDrivePath"].ToString() + "UploadedFiles", files_name)), false);
                }
                catch (Exception cc) { }

                hdn_doc_name.Value = e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();
                hdn_doc_sav.Value = files_name;
                lab_doc_name_out.Text = hdn_doc_name.Value;
            }
        }

        
        protected void rpt_doc_list_OnItemCommand(object s, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Download")
            {
                HiddenField hdn_fnm = (HiddenField)e.Item.FindControl("hdn_dnm");

                try
                {
                    if (hdn_fnm.Value != "")
                    {
                        string strURL = hdn_fnm.Value;
                        string[] ext = hdn_fnm.Value.Split('.');
                        string extension = ext[1];
                        string fil_name = strURL;
                        string full_name = Server.MapPath("~/UploadedFiles/" + fil_name);
                        Response.ContentType = "APPLICATION/OCTET-STREAM";
                        String Header = "Attachment; Filename=\"" + fil_name + "\"";
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
            else if (e.CommandName == "Edit")
            {
                HiddenField hdn_id = (HiddenField)e.Item.FindControl("hdn_id");
                HiddenField hdn_doc_Id = (HiddenField)e.Item.FindControl("hdn_doc_Id");
                HiddenField hdn_dnm = (HiddenField)e.Item.FindControl("hdn_dnm");
                Label lbl_doc_name = (Label)e.Item.FindControl("lbl_doc_name");
                Label lbl_doc_type_name = (Label)e.Item.FindControl("lbl_doc_type_name");
                Label lbl_docnum = (Label)e.Item.FindControl("lbl_docnum");
                HiddenField hdnVyr = (HiddenField)e.Item.FindControl("hdnVyr");

                Label lbl_from = (Label)e.Item.FindControl("lbl_from");
                Label lbl_to = (Label)e.Item.FindControl("lbl_to");
                HiddenField v_frm = (HiddenField)e.Item.FindControl("v_frm");
                HiddenField v_to = (HiddenField)e.Item.FindControl("v_to");
                Clear_documnt();

                drp_doc.SelectedValue = hdn_doc_Id.Value;
                valid_from.DbSelectedDate = v_frm.Value == "" ? (DateTime?)null : Convert.ToDateTime(v_frm.Value);
                valid_to.DbSelectedDate = v_to.Value == "" ? (DateTime?)null : Convert.ToDateTime(v_to.Value);
                lab_doc_name_out.Text = lbl_doc_name.Text;
                hdn_doc_name.Value = lbl_doc_name.Text;
                hdn_doc_sav.Value = hdn_dnm.Value;
                hdn_doc_index_Id.Value = hdn_id.Value;
                txt_doc_no.Text = lbl_docnum.Text;
                txtValidityyr.Text = hdnVyr.Value;

                Upd_docadd.Update();
            }
        }

        /*Remove Line*/
        protected void btn_remove_line_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            HiddenField hdn_indx = (HiddenField)itemrp.FindControl("hdn_indx");
            DataTable dt_doc = (DataTable)Session["dtEmpdoc"];
            DataTable dt_doc_add = new DataTable();
            dt_doc_add = dt_doc.Clone();

            foreach (DataRow rows in dt_doc.Rows)
            {
                if (hdn_indx.Value != rows["dt_indx"].ToString())
                {
                    dt_doc_add.Rows.Add(dt_doc_add.Rows.Count + 1, Convert.ToInt32(rows["id"]), rows["DocNumber"].ToString(), Convert.ToInt32(rows["DocumentTypeId"]),
                        rows["doc_type"].ToString(), rows["DocumentName"].ToString(), rows["DocumentSave"].ToString(),
                          rows["Valid_From"].ToString() == "" ? null : rows["Valid_From"].ToString(),
                          rows["Valid_To"].ToString() == "" ? null : rows["Valid_To"].ToString(),
                           rows["ValidityYear"].ToString() == "" ? (int?)null : Convert.ToInt32(rows["ValidityYear"]));
                }
            }
            Session["dtEmpdoc"] = dt_doc_add;
            fill_rpt(dt_doc_add, 1, Convert.ToInt32(drp_countD.SelectedValue));

            Upd_doc.Update();
        }

        protected void btn_reset_doc_OnClick(object sender, EventArgs e)
        {
            Clear_documnt();
            Upd_docadd.Update();
        }

        protected void btn_add_doc_OnClick(object sender, EventArgs e)
        {
            DataTable dt_doc = (DataTable)Session["dtEmpdoc"];
            DataTable dt_doc_add = new DataTable();
            dt_doc_add = dt_doc.Clone();

            foreach (DataRow rows in dt_doc.Rows)
            {
                if (hdn_doc_index_Id.Value != "0" && rows["id"].ToString() == hdn_doc_index_Id.Value)
                {
                    dt_doc_add.Rows.Add(Convert.ToInt32(rows["dt_indx"]), Convert.ToInt32(rows["id"]), txt_doc_no.Text, Convert.ToInt32(drp_doc.SelectedValue), drp_doc.SelectedItem.Text, hdn_doc_name.Value,
                        hdn_doc_sav.Value, valid_from.DbSelectedDate != null ? DateTime.ParseExact(CalDate(valid_from), "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null,
                        valid_to.DbSelectedDate != null ? DateTime.ParseExact(CalDate(valid_to), "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null,
                         txtValidityyr.Text == "" ? (int?)null : Convert.ToInt32(txtValidityyr.Text));
                }
                else
                {
                    dt_doc_add.Rows.Add(Convert.ToInt32(rows["dt_indx"]), Convert.ToInt32(rows["id"]), rows["DocNumber"].ToString(), Convert.ToInt32(rows["DocumentTypeId"]),
                        rows["doc_type"].ToString(), rows["DocumentName"].ToString(), rows["DocumentSave"].ToString(),
                           rows["Valid_From"].ToString() == "" ? null : rows["Valid_From"].ToString(), 
                           rows["Valid_To"].ToString() == "" ? null : rows["Valid_To"].ToString(), rows["ValidityYear"].ToString() == "" ? (int?)null : Convert.ToInt32(rows["ValidityYear"]));
                }
            }
            if (hdn_doc_index_Id.Value == "0")
            {
                dt_doc_add.Rows.Add(dt_doc.Rows.Count + 1, "-" + (dt_doc.Rows.Count + 1).ToString(), txt_doc_no.Text, Convert.ToInt32(drp_doc.SelectedValue),
                    drp_doc.SelectedItem.Text, hdn_doc_name.Value, hdn_doc_sav.Value, valid_from.DbSelectedDate != null ? DateTime.ParseExact(CalDate(valid_from), "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null,
                        valid_to.DbSelectedDate != null ? DateTime.ParseExact(CalDate(valid_to), "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null,
                         txtValidityyr.Text == "" ? (int?)null : Convert.ToInt32(txtValidityyr.Text));
            }

            Session["dtEmpdoc"] = dt_doc_add;

            fill_rpt(dt_doc_add, 1, Convert.ToInt32(drp_countD.SelectedValue));

            Clear_documnt();
            Upd_docadd.Update();
            Upd_doc.Update();
        }

        public DataTable fill_Detail()
        {
            DataTable dt_doc = (DataTable)Session["dtEmpdoc"];
            try
            {
                dt_doc.Columns.Remove("dt_indx");
                dt_doc.Columns.Remove("doc_type");
            }
            catch { }
            return dt_doc;
        }

        //filter Search
        protected void txt_doc_search_OnTextChanged(object sender, EventArgs e)
        {
            DataTable dtnin = (DataTable)Session["dtEmpdoc"];

            DataTable dh = new DataTable();

            if (dtnin != null)
            {
                dh = dtnin.Clone();

                DataRow[] dr = dtnin.Select("DocumentName LIKE '%" + txt_search_doc.Text + "%' or doc_type LIKE '%" + txt_search_doc.Text + "%' or DocNumber like '%" + txt_search_doc.Text + "%'");
                int cv = dr.Length;
                if (cv > 0)
                {
                    dh = dr.CopyToDataTable();
                    rpt_doc_list.DataSource = dh;
                    rpt_doc_list.DataBind();
                    fill_rpt(dh, 1, Convert.ToInt32(drp_countD.SelectedValue));
                }
                else
                {
                    rpt_doc_list.DataSource = dh;
                    rpt_doc_list.DataBind();
                    fill_rpt(dh, 1, Convert.ToInt32(drp_countD.SelectedValue));
                }
            }
        }

        public void fill_rpt(DataTable dt_doc, int PageNo, int count)
        {
            int Current_count = dt_doc.Rows.Count;
            int last_page = Current_count / count;
            int start_number = (PageNo - 1) * count + 1;
            int end_num = PageNo * count;
            int last_page_reminder = Current_count % count;
            if (last_page_reminder != 0)
            {
                last_page = last_page + 1;
            }

            DataTable dh = new DataTable();
            dh = dt_doc.Clone();

            foreach (DataRow rows in dt_doc.Rows)
            {
                if (Convert.ToInt32(rows["dt_indx"]) >= start_number && Convert.ToInt32(rows["dt_indx"]) <= end_num)
                {
                    dh.Rows.Add(Convert.ToInt32(rows["dt_indx"]), Convert.ToInt32(rows["id"]), rows["DocNumber"].ToString(), Convert.ToInt32(rows["DocumentTypeId"]),
                           rows["doc_type"].ToString(), rows["DocumentName"].ToString(), rows["DocumentSave"].ToString(),
                          rows["Valid_From"].ToString() == "" ? null : rows["Valid_From"].ToString(), 
                          rows["Valid_To"].ToString() == "" ? null : rows["Valid_To"].ToString(), rows["ValidityYear"].ToString() == "" ? (int?)null : Convert.ToInt32(rows["ValidityYear"]));
                }
            }
            rpt_doc_list.DataSource = dh;
            rpt_doc_list.DataBind();

            if (dh.Rows.Count > 0)
            {
                lbl_page_infoD.Text = "Showing Results " + start_number.ToString() + " - " + dh.Rows[dh.Rows.Count - 1]["dt_indx"].ToString() + " Out of " + Current_count.ToString() + " Records";
                hdn_last_pageD.Value = last_page.ToString();
                lbl_page_numberD.Text = PageNo.ToString();
                hdn_totalD.Value = Current_count.ToString();
            }
            else
            {
                lbl_page_infoD.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
                hdn_last_pageD.Value = "0";
                lbl_page_numberD.Text = "1";
                hdn_totalD.Value = "0";
            }
            Upd_Nav_Doc.Update();
            Upd_doc.Update();
        }

        #region Navigation Doc

        //First Page
        protected void btn_first1_OnClick(object sender, EventArgs e)
        {
            DataTable dt_doc = (DataTable)Session["dtEmpdoc"];
            fill_rpt(dt_doc, 1, Convert.ToInt32(drp_countD.SelectedValue));
        }

        //Previous Page
        protected void btn_prev1_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_numberD.Text) > 1)
            {
                DataTable dt_doc = (DataTable)Session["dtEmpdoc"];
                fill_rpt(dt_doc, Convert.ToInt32(lbl_page_numberD.Text) - 1, Convert.ToInt32(drp_countD.SelectedValue));
            }
        }

        //Next Page
        protected void btn_next1_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_numberD.Text) < Convert.ToInt32(hdn_last_pageD.Value))
            {
                DataTable dt_doc = (DataTable)Session["dtEmpdoc"];
                fill_rpt(dt_doc, Convert.ToInt32(lbl_page_numberD.Text) + 1, Convert.ToInt32(drp_countD.SelectedValue));
            }
        }

        //Last Page
        protected void btn_last1_OnClick(object sender, EventArgs e)
        {
            DataTable dt_doc = (DataTable)Session["dtEmpdoc"];
            fill_rpt(dt_doc, Convert.ToInt32(hdn_last_pageD.Value), Convert.ToInt32(drp_countD.SelectedValue));
        }

        //Page Data Count
        protected void drp_countD_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt_doc = (DataTable)Session["dtEmpdoc"];
            fill_rpt(dt_doc, 1, Convert.ToInt32(drp_countD.SelectedValue));
        }

        #endregion

        #endregion

        #region Incentive amount

        protected void btnIncentiveamountOnClick(object sender, EventArgs e)
        {
            pnl_Service_Detail.Visible = true;
            txtCommonAmount.Text = "";
            DataTable dt = obj_master.GetEmpoyeeIncentive(Convert.ToInt32(hdn_id.Value));
            rpt_serdetail.DataSource = dt;
            rpt_serdetail.DataBind();

            Upd_Service_Detail_Panel.Update();
        }

        protected void btn_SDSave_OnClick(object sender, EventArgs e)
        {
            int res = 0;
            DataTable dt_serDetail = fill_ServiceDetail();
            if (dt_serDetail.Rows.Count > 0)
                res = obj_master.InsertUpdateEmpoyeeIncentive(Convert.ToInt32(hdn_id.Value),
                dt_serDetail, Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                pnl_Service_Detail.Visible = false;
            }
            else
            {
            }
            Upd_Service_Detail_Panel.Update();
        }

        public DataTable fill_ServiceDetail()
        {
            DataTable dt_serDetail = new DataTable();
            dt_serDetail.Columns.Add("DId", typeof(int));
            dt_serDetail.Columns.Add("ServiceId", typeof(int));
            dt_serDetail.Columns.Add("IncentiveAmount", typeof(decimal));
            dt_serDetail.Columns.Add("IncentivePercentage", typeof(decimal));

            if (rpt_serdetail.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_serdetail.Items)
                {
                    HiddenField hdn_DId = (HiddenField)itm.FindControl("hdn_DId");
                    HiddenField hdn_serviceId = (HiddenField)itm.FindControl("hdn_serviceId");
                    TextBox txt_Incamt = (TextBox)itm.FindControl("txt_Incamt");
                    TextBox txt_IncPer = (TextBox)itm.FindControl("txt_IncPer");

                    dt_serDetail.Rows.Add(Convert.ToInt32(hdn_DId.Value),
                        Convert.ToInt32(hdn_serviceId.Value),txt_Incamt.Text==""?0: Convert.ToDecimal(txt_Incamt.Text),
                        txt_IncPer.Text == "" ? 0 : Convert.ToDecimal(txt_IncPer.Text));
                }
            }
            return dt_serDetail;
        }

        protected void btn_close_sd_OnClick(object sender, EventArgs e)
        {
            pnl_Service_Detail.Visible = false;
            Upd_Service_Detail_Panel.Update();
        }

        #endregion

        #region Navigation

        //Search
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

        //Calculate Date
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

        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.Form_Previlage_Validation(11, Convert.ToInt32(hdn_user_id.Value));
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

        public void previlage_action_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    DataTable dt = obj_common.Action_Previlage_Validation(11, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdn_update.Value = dt.Rows[1][1].ToString();
                        hdn_delete.Value = dt.Rows[2][1].ToString();
                        hdn_OB.Value = dt.Rows[3][1].ToString();
                        hdn_doc.Value = dt.Rows[4][1].ToString();
                        hdn_menu.Value = dt.Rows[5][1].ToString();
                        hdn_other.Value = dt.Rows[6][1].ToString();
                        hdnIncentive.Value = dt.Rows[7][1].ToString();
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

        protected void rpt_serdetail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (hdnIncPerc.Value == "True")
            {
                var td = (HtmlTableCell)e.Item.FindControl("td_incamt");
                td.Attributes.Add("style", "display:none");
            }
            else
            {
                var td = (HtmlTableCell)e.Item.FindControl("td_incperc");
                td.Attributes.Add("style", "display:none");
            }
        }
    }
}