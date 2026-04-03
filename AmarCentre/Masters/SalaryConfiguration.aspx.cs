using AmarCentre.BAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;

namespace AmarCentre.Masters
{
    public partial class SalaryConfiguration : System.Web.UI.Page
    {
        System_Utilities obj_common = new System_Utilities();
        Master_Bal obj_master = new Master_Bal();
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
                fillDateDropDown();
                fill_Data();

            }
        }
        public void fillDateDropDown()
        {
            RadComboBoxItem CodeItem;
            for (int date = 1; date <= 31; date++)
            {
                CodeItem = new RadComboBoxItem();
                CodeItem.Text = date.ToString();
                CodeItem.Value = date.ToString();
                drpSPFromDate.Items.Add(CodeItem);
            }
            for (int date = 1; date <= 31; date++)
            {
                CodeItem = new RadComboBoxItem();
                CodeItem.Text = date.ToString();
                CodeItem.Value = date.ToString();
                drpSPToDate.Items.Add(CodeItem);
            }

            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "Sunday";
            CodeItem.Value = "1";
            drpWeekendDays.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "Monday";
            CodeItem.Value = "2";
            drpWeekendDays.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "Tuesday";
            CodeItem.Value = "3";
            drpWeekendDays.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "Wednesday";
            CodeItem.Value = "4";
            drpWeekendDays.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "Thursday";
            CodeItem.Value = "5";
            drpWeekendDays.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "Friday";
            CodeItem.Value = "6";
            drpWeekendDays.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "Saturday";
            CodeItem.Value = "7";
            drpWeekendDays.Items.Add(CodeItem);

        }
        public void fill_Data()
        {
            DataSet ds = obj_master.EditSalaryConfiguration();
            DataTable dt = ds.Tables[0];
            DataTable dtWeekendDays = ds.Tables[1];
            if (dt.Rows.Count > 0)
            {

                hdn_id.Value = dt.Rows[0]["Id"].ToString();
                drpSPFromDate.SelectedValue = dt.Rows[0]["SalaryProcessFromDate"].ToString();
                drpSPToDate.SelectedValue = dt.Rows[0]["SalaryProcessToDate"].ToString();
                chkOTApplicable.Checked = Convert.ToBoolean(dt.Rows[0]["OverTimeApplicable"]);
                pnlOvertime.Visible= Convert.ToBoolean(dt.Rows[0]["OverTimeApplicable"]);
                txtOTNormalDay.Text= dt.Rows[0]["OTOnNormalDays"].ToString();
                txtOTWeekend.Text = dt.Rows[0]["OTOnWeekendDays"].ToString();
                txtOTHoliday.Text = dt.Rows[0]["OTOnHolidays"].ToString();
                txtWorkingHours.Text = dt.Rows[0]["WorkingHours"].ToString();
                rbBasedOnMonth.Checked = true;
                rbBasedOnWorkingDays.Checked = false;
                if (dt.Rows[0]["SalaryBasedonDays"].ToString() == "1")
                {
                    rbBasedOnMonth.Checked = true;
                    rbBasedOnWorkingDays.Checked = false;
                }
                else if (dt.Rows[0]["SalaryBasedonDays"].ToString() == "2")
                {
                    rbBasedOnMonth.Checked = false;
                    rbBasedOnWorkingDays.Checked = true;
                }
                foreach (DataRow dr in dtWeekendDays.Rows)
                {
                    RadComboBoxItem item = (RadComboBoxItem)(drpWeekendDays.FindItemByValue(dr["DaysNo"].ToString()));
                    item.Checked = true;
                    item.Selected = true;
                }
            }
        }

        protected void chkOTApplicableOnCheckedChanged(object sender, EventArgs e)
        {
            txtOTNormalDay.Text = txtOTWeekend.Text = txtOTHoliday.Text = "";
            pnlOvertime.Visible = chkOTApplicable.Checked;
            UpdOvertime.Update();
        }

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            DataTable dtWeekendDays = new DataTable();
            dtWeekendDays.Columns.Add("DaysNo", typeof(int));
            dtWeekendDays.Columns.Add("DaysName", typeof(string));
            foreach (RadComboBoxItem item in drpWeekendDays.Items)
            {
                if (item.Checked)
                    dtWeekendDays.Rows.Add(Convert.ToInt32(item.Value),item.Text);
            }
            int res =obj_master.InsertSalaryConfiguration(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(drpSPFromDate.SelectedValue),
                Convert.ToInt32(drpSPToDate.SelectedValue), Convert.ToInt32(chkOTApplicable.Checked),
                txtOTNormalDay.Text==""?(decimal?)null:Convert.ToDecimal(txtOTNormalDay.Text),
                txtOTWeekend.Text == "" ? (decimal?)null : Convert.ToDecimal(txtOTWeekend.Text),
                txtOTHoliday.Text == "" ? (decimal?)null : Convert.ToDecimal(txtOTHoliday.Text), Convert.ToInt32(txtWorkingHours.Text),rbBasedOnMonth.Checked==true?1:2,
                dtWeekendDays,Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                fill_Data();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            Upd_Add_Panel.Update();
        }

        //Check Privilege
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.Form_Previlage_Validation(47, Convert.ToInt32(hdn_user_id.Value));
                    if (val == 0)
                    {
                        Response.Redirect("~/Landing.aspx");
                    }
                }
                else
                {
                    Response.Redirect("~/Landing.aspx");
                }
            }
            catch
            {
                Response.Redirect("~/Landing.aspx");
            }
        }

        public void previlage_action_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    DataTable dt = obj_common.Action_Previlage_Validation(47, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                    }
                    btn_save.Visible = hdn_add.Value == "0" ? false : true;
                }
                else
                {
                    Response.Redirect("../Login.aspx");
                }
            }
            catch
            {
                Response.Redirect("../Login.aspx");
            }
        }
    }
}