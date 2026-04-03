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

namespace AmarCentre.Transactions
{
    public partial class Profitsharing : System.Web.UI.Page
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
            DataTable dt = obj_trans.ListProfitsharing(page_number, page_size, filter);
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
            DataTable dt = obj_trans.ListExcelProfitsharing();
            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "Profitsharing");
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
            int lastyear = DateTime.Now.Year + 1;
            for (int date = 2020; date <= lastyear; date++)
            {
                CodeItem = new RadComboBoxItem();
                CodeItem.Text = date.ToString();
                CodeItem.Value = date.ToString();
                drpYear.Items.Add(CodeItem);
            }
        }

        protected void rpt_list_OnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            Clear();
            pnl_add.Visible = true;
            HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdn_id");
            Filldata(Convert.ToInt32(hdn_rpt_id.Value));
            Upd_Add_Panel.Update();
        }

        public void Filldata(int Id)
        {
            DataSet ds = obj_trans.EditProfitsharing(Id);
            DataTable dt1 = ds.Tables[0];

            hdn_id.Value = dt1.Rows[0]["Id"].ToString();
            lbl_Code.Text = dt1.Rows[0]["Code"].ToString();
            job_date.DbSelectedDate = dt1.Rows[0]["Dated"].ToString();
            drpMonth.SelectedValue = dt1.Rows[0]["MonthId"].ToString();
            drpYear.SelectedValue = dt1.Rows[0]["YearId"].ToString();

            txtRemark.Text = dt1.Rows[0]["Remarks"].ToString();
            txtCurrentProfit.Text = ds.Tables[0].Rows[0]["NetProfit"].ToString();

            rptPartnerShare.DataSource = ds.Tables[1];
            rptPartnerShare.DataBind();

            btnCancel.Visible = hdncancel.Value == "0" ? false : true;
            if (dt1.Rows[0]["StatusId"].ToString() == "2")
                btnCancel.Visible = false;

            btn_save.Visible = false;
            Upd_Add_PanelInner.Update();
        }

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            DataSet ds = fill_Detail();
            if (ds.Tables[0].Rows.Count > 0)
            {
                int res = obj_trans.InsertProfitSharing(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(drpMonth.SelectedValue),
                        Convert.ToInt32(drpYear.SelectedValue), job_date.SelectedDate, Convert.ToDecimal(txtCurrentProfit.Text), txtRemark.Text,
                        ds.Tables[0], Convert.ToInt32(hdn_user_id.Value));

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
            }
            else
            {
                lblerrormsg.Text = "Partner details not added !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
            }
            Upd_Add_PanelInner.Update();
        }

        public DataSet fill_Detail()
        {
           
            DataTable dtpartner = new DataTable();
            dtpartner.Columns.Add("PartnerId", typeof(int));
            dtpartner.Columns.Add("SharePercentage", typeof(decimal));
            dtpartner.Columns.Add("PartnerShare", typeof(decimal));

            foreach (RepeaterItem itm in rptPartnerShare.Items)
            {
                HiddenField hdnPartnerId = (HiddenField)itm.FindControl("hdnPartnerId");
                TextBox txtSharePercentage = (TextBox)itm.FindControl("txtSharePercentage");
                TextBox txtPartnerShare = (TextBox)itm.FindControl("txtPartnerShare");

                dtpartner.Rows.Add(Convert.ToInt32(hdnPartnerId.Value), txtSharePercentage.Text == "" ? 0 : Convert.ToDecimal(txtSharePercentage.Text),
                   txtPartnerShare.Text == "" ? 0 : Convert.ToDecimal(txtPartnerShare.Text));
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dtpartner);

            return ds;
        }

        protected void FillProfitDetails(object sender, EventArgs e)
        {
            ClearSubdata();
            if (drpMonth.SelectedValue != "" & drpYear.SelectedValue != "")
            {
                DataSet ds = obj_trans.GetProfitsharing(Convert.ToInt32(drpMonth.SelectedValue), Convert.ToInt32(drpYear.SelectedValue));
                if (ds.Tables[2].Rows[0]["ExistId"].ToString() != "0")
                {
                    Filldata(Convert.ToInt32(ds.Tables[2].Rows[0]["ExistId"]));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Already Processed.!');", true);
                }
                else
                {
                    txtCurrentProfit.Text = ds.Tables[0].Rows[0]["NetProfit"].ToString();
                    rptPartnerShare.DataSource = ds.Tables[1];
                    rptPartnerShare.DataBind();
                }
            }
            Upd_Add_PanelInner.Update();
        }

        public void ClearSubdata()
        {
            txtRemark.Text = "";
            hdn_id.Value = "0";
            job_date.SelectedDate = DateTime.Now;
            txtCurrentProfit.Text = "";
            btn_save.Visible = hdn_add.Value == "0" ? false : true;
            btnCancel.Visible = false;
            Get_Code();
            rptPartnerShare.DataSource = null;
            rptPartnerShare.DataBind();
            Upd_Add_PanelInner.Update();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            int res = 0;
            res = obj_trans.CancelProfitSharing(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_user_id.Value));
            if (res > 0)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                ClearSubdata();
                drpYear.ClearSelection();
                drpYear.Text = "";
                drpMonth.ClearSelection();
                drpMonth.Text = "";
                lbl_msgin.Text = "Cancelled Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lblerrormsg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
            }
            Upd_Add_PanelInner.Update();
        }

        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
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
            txtRemark.Text = "";
            job_date.SelectedDate = DateTime.Now;
            txtCurrentProfit.Text = "";
            FillProfitDetails(null, null);
            btnCancel.Visible = false;

            btn_save.Visible = hdn_add.Value == "0" ? false : true;
            Get_Code();

            Upd_Add_PanelInner.Update();
        }

        /*Code for Display*/
        public void Get_Code()
        {
            DataTable dt = obj_common.Get_Code(103);
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
                    DataTable dt = obj_common.Action_Previlage_Validation(103, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdncancel.Value = dt.Rows[1][1].ToString();
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

                    int val = obj_common.Form_Previlage_Validation(103, Convert.ToInt32(hdn_user_id.Value));
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