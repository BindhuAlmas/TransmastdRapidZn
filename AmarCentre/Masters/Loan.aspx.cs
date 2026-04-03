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

namespace AmarCentre.Masters
{
    public partial class Loan : System.Web.UI.Page
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
                Clear();
                grid_fill(1, 10, "", "", "");
            }
        }

        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_master.Get_List_Loan(page_number, page_size, filter, column, order);
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

        public void btnexcel_export_OnClick(object sender, EventArgs e)
        {
            DataTable dt = obj_master.Get_List_Loan_Excel();
            dt.Columns["Sl_No"].ColumnName = "Sl No.";
            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "Loan");

                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                HttpContext.Current.Response.Write(style);
                Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        protected void rpt_list_OnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            Clear();
            HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdn_id");
            DataTable dt = obj_master.Edit_Loan(Convert.ToInt32(hdn_rpt_id.Value));
            txt_name.Text = dt.Rows[0]["Name"].ToString();
            txt_address.Text = dt.Rows[0]["Address"].ToString();
            txt_mob.Text = dt.Rows[0]["Mobile"].ToString();
            txt_email.Text = dt.Rows[0]["Email"].ToString();
            txt_trn.Text = dt.Rows[0]["TRN"].ToString();
            lblReceivable.Text = dt.Rows[0]["Receivable"].ToString();
            lblPayable.Text = dt.Rows[0]["Payable"].ToString();
            radDueDate.SelectedValue = dt.Rows[0]["DueDay"].ToString();
            txt_CreditCardAmount.Text= dt.Rows[0]["CreditAmount"].ToString();
            chk_IsCreditCard.Checked = Convert.ToBoolean(dt.Rows[0]["isCreditCard"]);
            lblReceivable.Text = dt.Rows[0]["Receivable"].ToString();
            lblPayable.Text = dt.Rows[0]["Payable"].ToString();
            pnl_CreditCardAmount.Visible= chk_IsCreditCard.Checked;
            hdn_id.Value = dt.Rows[0][0].ToString();

            btn_delete.Visible = hdn_delete.Value == "0" ? false : true;
            btn_save.Visible = hdn_update.Value == "0" ? false : true;
            btn_OB.Visible = hdn_OB.Value == "0" ? false : true;
            pnl_add.Visible = true;
            Upd_Add_Panel.Update();
        }

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Insert_Update_Loan(Convert.ToInt32(hdn_id.Value), txt_name.Text,
                 txt_address.Text, txt_mob.Text, txt_email.Text, Convert.ToInt32(hdn_user_id.Value), txt_trn.Text,
                txt_CreditCardAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txt_CreditCardAmount.Text),
                Convert.ToInt32(chk_IsCreditCard.Checked), radDueDate.SelectedValue =="" ? (int?)null : Convert.ToInt32(radDueDate.SelectedValue));
            if (res == 1)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lblerrormsg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
            }
            pnl_add.Visible = false;
            Upd_Add_Panel.Update();
            Upd_CreditCardAmount_Panel.Update();
        }

        protected void btn_delete_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Delete_Loan(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_user_id.Value));
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
        protected void chk_IsCreditCard_OnCheckedChanged(object sender, EventArgs e)
        {
            txt_CreditCardAmount.Text = "";
            pnl_CreditCardAmount.Visible = chk_IsCreditCard.Checked;
            radDueDate.ClearSelection();
            radDueDate.Text = "";
            Upd_CreditCardAmount_Panel.Update();
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
            txt_name.Text = "";
            txt_address.Text = "";
            txt_mob.Text = "";
            txt_email.Text = "";
            drp_obType.ClearSelection();
            drp_obType.Text = "";
            txt_CreditCardAmount.Text = "";
            radDueDate.ClearSelection();
            radDueDate.Text = "";
            chk_IsCreditCard.Checked = pnl_CreditCardAmount.Visible = false;
            txt_open_bal.Text = "";
            ob_date.DbSelectedDate = DateTime.Now;
            lblReceivable.Text = "";
            lblPayable.Text = "";
            txt_trn.Text = "";
            hdn_id.Value = "0";

            btn_save.Visible = hdn_add.Value == "0" ? false : true;
            btn_delete.Visible = false;
            btn_OB.Visible = false;
            Upd_Add_PanelInner.Update();
        }

        #region Opening balance

        protected void btn_OB_OnClick(object sender, EventArgs e)
        {
            pnl_obalance.Visible = true;
            DataTable dt = obj_master.GetLoanOB(Convert.ToInt32(hdn_id.Value));

            drp_obType.Enabled = ob_date.Enabled = btnOBClear.Enabled = true;
            txt_open_bal.ReadOnly = false;

            if (dt.Rows[0]["IsEditAllow"].ToString() == "1")
            {
                btn_OBSave.Enabled = false;
                btnOBClear.Enabled = true;
                if (dt.Rows[0]["OpeningBalanceType"].ToString() == "")
                {
                    btn_OBSave.Enabled = true;
                    btnOBClear.Enabled = false;
                }
            }
            if (dt.Rows[0]["IsEditAllow"].ToString() == "0")
            {
                drp_obType.Enabled = ob_date.Enabled = false;
                txt_open_bal.ReadOnly = true;
                btn_OBSave.Enabled = false; //btnOBClear.Enabled =
            }

            drp_obType.SelectedValue = dt.Rows[0]["OpeningBalanceType"].ToString();
            txt_open_bal.Text = dt.Rows[0]["OBalance"].ToString();
            ob_date.DbSelectedDate = dt.Rows[0]["ODate"].ToString();

            Upd_OB_Panel.Update();
        }

        protected void btn_OBClear_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.ClearLoanOB(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                drp_obType.ClearSelection();
                drp_obType.Text = "";
                txt_open_bal.Text = "";
                ob_date.DbSelectedDate = DateTime.Now;

                btn_OBSave.Enabled = true;
                btnOBClear.Enabled = false;

                drp_obType.Enabled = ob_date.Enabled = true;
                txt_open_bal.ReadOnly = false;
            }
            else
            {
            }
            Upd_OBIn.Update();
        }

        protected void btn_OBSave_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Update_OB_Loan(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(drp_obType.SelectedValue), Convert.ToDecimal(txt_open_bal.Text),
                ob_date.SelectedDate,Convert.ToInt32(hdn_user_id.Value));
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


        //Search
        protected void txt_search_OnTextChanged(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), txt_search.Text, Common_order_column.Value, Common_asc_desc.Value);
            Upd_List_Panel.Update();
        }

        #region Navigation

        //First Page
        protected void btn_first_OnClick(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
            Upd_List_Panel.Update();
        }

        //Previous Page
        protected void btn_prev_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number.Text) > 1)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text) - 1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Upd_List_Panel.Update();
            }
        }

        //Next Page
        protected void btn_next_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number.Text) < Convert.ToInt32(hdn_last_page.Value))
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text) + 1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Upd_List_Panel.Update();
            }
        }

        //Last Page
        protected void btn_last_OnClick(object sender, EventArgs e)
        {

            grid_fill(Convert.ToInt32(hdn_last_page.Value), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
            Upd_List_Panel.Update();
        }

        //Page Data Count
        protected void drp_count_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
            Upd_List_Panel.Update();
        }

        #endregion

        //Check Privilege
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.Form_Previlage_Validation(44, Convert.ToInt32(hdn_user_id.Value));
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

        //Check Privilege
        public void previlage_action_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    DataTable dt = obj_common.Action_Previlage_Validation(44, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdn_update.Value = dt.Rows[1][1].ToString();
                        hdn_delete.Value = dt.Rows[2][1].ToString();
                        hdn_OB.Value = dt.Rows[3][1].ToString();
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
       
    }
}