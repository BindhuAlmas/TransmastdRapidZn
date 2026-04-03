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
    public partial class Accounts : System.Web.UI.Page
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
                fillType();
                Clear();
                grid_fill(1, 10, "", "", "");
            }
        }

        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_master.Get_List_Bank_Account(page_number, page_size, filter, column, order);
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
            DataTable dt = obj_master.Get_List_Bank_Account_Excel();
            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "BankAccountList");

                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                HttpContext.Current.Response.Write(style);
                Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        public void fillType()
        {
            DataTable dt = obj_master.fill_bank_Type();
            drp_Type.DataSource = dt;
            drp_Type.DataTextField = "text";
            drp_Type.DataValueField = "value";
            drp_Type.DataBind();
        }

        protected void rpt_list_OnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            Clear();
            HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdn_id");
            DataTable dt = obj_master.Edit_Bank_Account(Convert.ToInt32(hdn_rpt_id.Value));
            txt_disp_name.Text = dt.Rows[0]["Display_Name"].ToString();
            txt_prv_name.Text = dt.Rows[0]["Provider_Name"].ToString();
            txt_desc.Text = dt.Rows[0]["Description"].ToString();
            drp_Type.SelectedValue = dt.Rows[0]["AccountTypeId"].ToString();
            chk_Rec.Checked = (dt.Rows[0]["IsConfirmNeed"].ToString() == "" ? false : Convert.ToBoolean(dt.Rows[0]["IsConfirmNeed"]));
            chk_comm.Checked = (dt.Rows[0]["IsCommssionApp"].ToString() == "" ? false : Convert.ToBoolean(dt.Rows[0]["IsCommssionApp"]));
            txt_comm.Text = dt.Rows[0]["CommissionPer"].ToString();
            chk_edhm.Checked = (dt.Rows[0]["IsCompanyEdhirham"].ToString() == "" ? false : Convert.ToBoolean(dt.Rows[0]["IsCompanyEdhirham"]));
            chkIsNomad.Checked =  Convert.ToBoolean(dt.Rows[0]["IsNomad"]);
            chkIsVatApplicable.Checked = Convert.ToBoolean(dt.Rows[0]["IsVatApplicable"]);
            txtTRN.Text = dt.Rows[0]["TRN"].ToString();

            hdn_id.Value = dt.Rows[0][0].ToString();

            lbl_bal.Text = dt.Rows[0]["Balance"].ToString();

            btn_delete.Visible = hdn_delete.Value == "0" ? false : true;
            btn_save.Visible = hdn_update.Value == "0" ? false : true;
            btn_OB.Visible = hdn_OB.Value == "0" ? false : true;
            pnl_add.Visible = true;
            Upd_Add_Panel.Update();
        }

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Insert_Update_Bank_Account(Convert.ToInt32(hdn_id.Value), txt_disp_name.Text, txt_prv_name.Text, Convert.ToInt32(drp_Type.SelectedValue),
                txt_desc.Text, Convert.ToInt32(hdn_user_id.Value), Convert.ToInt32(chk_Rec.Checked),
                txt_comm.Text == "" ? (decimal?)null : Convert.ToDecimal(txt_comm.Text), Convert.ToInt32(chk_comm.Checked), 
                Convert.ToInt32(chk_edhm.Checked),Convert.ToInt32(chkIsNomad.Checked), Convert.ToInt32(chkIsVatApplicable.Checked),
                txtTRN.Text);
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
        }

        protected void btn_delete_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Delete_Bank_Account(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_user_id.Value));
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
            drp_Type.ClearSelection();
            drp_Type.Text = "";
            try
            {
                drp_Type.SelectedValue = "1";
            }
            catch (Exception x) { }
            txt_desc.Text = "";
            txt_disp_name.Text = "";
            txt_prv_name.Text = "";
            hdn_id.Value = "0";
            txt_open_bal.Text = "";
            ob_date.DbSelectedDate = DateTime.Now;
            lbl_bal.Text = "";
            chk_Rec.Checked =chk_edhm.Checked= false;
            chk_comm.Checked = false;
            txt_comm.Text = txtTRN.Text= "";
            chkIsNomad.Checked = false;
            chkIsVatApplicable.Checked = false;

            btn_delete.Visible = false;
            btn_OB.Visible = false;
            btn_save.Visible = hdn_add.Value == "0" ? false : true;

            Upd_Add_PanelInner.Update();
        }

        #region Opening balance

        //Reset Button
        protected void btn_OB_OnClick(object sender, EventArgs e)
        {
            pnl_obalance.Visible = true;
            DataTable dt = obj_master.Edit_Bank_Account(Convert.ToInt32(hdn_id.Value));

            txt_open_bal.Text = dt.Rows[0]["OBal"].ToString();
             //txt_open_bal.Enabled = ob_date.Enabled = true;

            if (dt.Rows[0]["ODate"].ToString() != "")
            {
                ob_date.DbSelectedDate = dt.Rows[0]["ODate"].ToString();
                //txt_open_bal.Enabled = ob_date.Enabled = false;
            }
            else
                ob_date.DbSelectedDate = DateTime.Now;

            Upd_OB_Panel.Update();
        }

        //Reset Button
        protected void btn_OBSave_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Update_OB_Bank_Account(Convert.ToInt32(hdn_id.Value), Convert.ToDecimal(txt_open_bal.Text),
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

        //Reset Button
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

                    int val = obj_common.Form_Previlage_Validation(13, Convert.ToInt32(hdn_user_id.Value));
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

                    DataTable dt = obj_common.Action_Previlage_Validation(13, Convert.ToInt32(hdn_user_id.Value));
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