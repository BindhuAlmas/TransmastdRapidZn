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

namespace AmarCentre.Masters
{
    public partial class Agent : System.Web.UI.Page
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
            DataTable dt = obj_master.Get_List_Agent(page_number, page_size, filter);
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
            DataTable dt = obj_master.Get_List_Agent_Excel();
            dt.Columns["Sl_No"].ColumnName = "Sl No.";
            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "Agent");

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
            DataSet ds = obj_master.Edit_Agent(Convert.ToInt32(hdn_rpt_id.Value));
            DataTable dt = ds.Tables[0];

            txt_name.Text = dt.Rows[0]["Name"].ToString();
            txtArabicName.Text = dt.Rows[0]["ArabicName"].ToString();
            txt_address.Text = dt.Rows[0]["Address"].ToString();
            txt_mob.Text = dt.Rows[0]["Mobile_num"].ToString();
            txt_phn.Text = dt.Rows[0]["Phone_num"].ToString();
            txt_remark.Text = dt.Rows[0]["Remark"].ToString();
            txt_email.Text = dt.Rows[0]["Email"].ToString();
            txt_trn.Text = dt.Rows[0]["TRN"].ToString();
            hdn_id.Value = dt.Rows[0][0].ToString();
            txtprofit.Text= dt.Rows[0]["ProfitPer"].ToString();
          

            btn_delete.Visible = hdn_delete.Value == "0" ? false : true;
            btn_save.Visible = hdn_update.Value == "0" ? false : true;
            btn_OB.Visible = hdn_OB.Value == "0" ? false : true;

            btn_ServiceCommission.Visible = hdn_ServiceCommission_id.Value == "2" && hdn_servicecommission.Value != "0";
            if (hdn_ServiceCommission_id.Value == "2")
            {
                txtprofit.Text = "0";
                txtprofit.Enabled = false;
            }
            else
            {
                txtprofit.Enabled = true;
            }


            //txtprofit.Enabled = hdn_ServiceCommission_id.Value == "2" ? false : true; 
            pnl_add.Visible = true;
            Upd_Add_Panel.Update();
        }

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Insert_Update_Agent(Convert.ToInt32(hdn_id.Value), txt_name.Text, txt_address.Text,
                txt_mob.Text, txt_phn.Text, txt_email.Text, txt_remark.Text, Convert.ToInt32(hdn_user_id.Value), txt_trn.Text, 
                txtArabicName.Text, txtprofit.Text==""?0:Convert.ToDecimal(txtprofit.Text));
            if (res > 0)
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
            int res = obj_master.Delete_Agent(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_user_id.Value));
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

        #region Opening balance

        protected void btn_OB_OnClick(object sender, EventArgs e)
        {
            pnl_obalance.Visible = true;
            DataSet ds = obj_master.Edit_Agent(Convert.ToInt32(hdn_id.Value));
            DataTable dt = ds.Tables[0];

            txt_open_bal.Text = dt.Rows[0]["OBal"].ToString();
            ob_date.Enabled = true;
            txt_open_bal.ReadOnly = false;

            if (dt.Rows[0]["ODate"].ToString() != "")
            {
                ob_date.DbSelectedDate = dt.Rows[0]["ODate"].ToString();
                ob_date.Enabled = false;
                txt_open_bal.ReadOnly = true;
            }
            else
                ob_date.DbSelectedDate = DateTime.Now;

            Upd_OB_Panel.Update();
        }

        protected void btn_OBSave_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Update_OBAgent(Convert.ToInt32(hdn_id.Value), 2, Convert.ToDecimal(txt_open_bal.Text),
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

        protected void btn_ServiceCommission_OnClick(object sender, EventArgs e) //pooja added
        {
            pnl_Service_Detail.Visible = true;
            txtsearchservice.Text = "";
            txtsearchservice_TextChanged(null, null);
            Upd_Service_Detail_Panel.Update();
        }

        protected void txtsearchservice_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = obj_master.Get_List_AgentServiceCommission(Convert.ToInt32(hdn_id.Value), txtsearchservice.Text);
            rpt_serdetail.DataSource = dt;
            rpt_serdetail.DataBind();

            updservicelist.Update();
        }

        protected void btn_SDSave_OnClick(object sender, EventArgs e)
        {
            int res = 0;
            DataTable dt_serDetail = fill_ServiceDetail();
            if (dt_serDetail.Rows.Count > 0)
                res = obj_master.Insert_Update_AgentServiceCommission(Convert.ToInt32(hdn_id.Value),
                dt_serDetail, Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Saved Successfully');", true);
            }
            else
            {
            }
            Upd_Service_Detail_Panel.Update();


        }

        public DataTable fill_ServiceDetail()
        {
            DataTable dt_serDetail = new DataTable();
            dt_serDetail.Columns.Add("AgentSerDetailId", typeof(int));
            dt_serDetail.Columns.Add("ServiceId", typeof(int));
            dt_serDetail.Columns.Add("CommissionAmount", typeof(decimal));

            if (rpt_serdetail.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_serdetail.Items)
                {
                    HiddenField hdn_AgentSerDetailId = (HiddenField)itm.FindControl("hdn_AgentSerDetailId");
                    HiddenField hdn_serviceId = (HiddenField)itm.FindControl("hdn_serviceId");
                    TextBox txtCommissionAmount = (TextBox)itm.FindControl("txtCommissionAmount");


                    dt_serDetail.Rows.Add(Convert.ToInt32(hdn_AgentSerDetailId.Value),
                        Convert.ToInt32(hdn_serviceId.Value), 
                        txtCommissionAmount.Text == "" ? 0 : Convert.ToDecimal(txtCommissionAmount.Text));
                }
            }
            return dt_serDetail;
        }

        protected void btn_close_sd_OnClick(object sender, EventArgs e)
        {
            pnl_Service_Detail.Visible = false;
            Upd_Service_Detail_Panel.Update();
        }

        //to this

        protected void btn_newentry_OnClick(object sender, EventArgs e)
        {
            Clear();
            pnl_add.Visible = true;
            Upd_Add_Panel.Update();
        }
        public void Clear()
        {
            txt_name.Text = txtprofit.Text = "";
            txtArabicName.Text = "";
            txt_address.Text = "";
            txt_mob.Text = "";
            txt_phn.Text = "";
            txt_remark.Text = "";
            txt_email.Text = "";
            txt_trn.Text = "";

            hdn_id.Value = "0";

            btn_save.Visible = hdn_add.Value == "0" ? false : true;

            btn_ServiceCommission.Visible = btn_OB.Visible = false;
            DataTable dtgen = obj_master.Edit_GeneralSettings();
            hdn_ServiceCommission_id.Value = dtgen.Rows[0]["AgentCommission"].ToString();
            txtprofit.Enabled = hdn_ServiceCommission_id.Value == "2" ? false : true;
            btn_delete.Visible = false;

            Upd_Add_PanelInner.Update();
        }
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

        //Check Privilege
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.Form_Previlage_Validation(63, Convert.ToInt32(hdn_user_id.Value));
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

                    DataTable dt = obj_common.Action_Previlage_Validation(63, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdn_update.Value = dt.Rows[1][1].ToString();
                        hdn_delete.Value = dt.Rows[2][1].ToString();
                        hdn_servicecommission.Value = dt.Rows[3][1].ToString();
                        hdn_OB.Value = dt.Rows[4][1].ToString();
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
    }
}