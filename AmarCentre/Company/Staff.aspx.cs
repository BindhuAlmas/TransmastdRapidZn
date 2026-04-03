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

namespace AmarCentre.Company
{
    public partial class Staff : System.Web.UI.Page
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
                FillCompany();
                Clear();
                grid_fill(1, 10, "", "", "");
            }
        }

        public void FillCompany()
        {
            drpCompany.DataSource = obj_master.DrpfillCompany();
            drpCompany.DataValueField = "value";
            drpCompany.DataTextField = "Text";
            drpCompany.DataBind();
        }

        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_master.Get_ListCompanyStaff(page_number, page_size, filter);
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
            DataTable dt = obj_master.Get_ListCompanyStaffExcel();
            dt.Columns["Sl_No"].ColumnName = "Sl No.";
            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "Companystaff");

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
            DataSet ds = obj_master.EditCompanyStaff(Convert.ToInt32(hdn_rpt_id.Value));
            DataTable dt = ds.Tables[0];

            txt_name.Text = dt.Rows[0]["Name"].ToString();
            txt_address.Text = dt.Rows[0]["Address"].ToString();
            txt_mob.Text = dt.Rows[0]["ContactNo"].ToString();
            txt_remark.Text = dt.Rows[0]["Remark"].ToString();
            txt_email.Text = dt.Rows[0]["Email"].ToString();
            txtAgreeamt.Text = dt.Rows[0]["AgreementAmount"].ToString();
            txtPaid.Text = dt.Rows[0]["PaidAmount"].ToString();
            drpCompany.SelectedValue= dt.Rows[0]["CompanyId"].ToString();
            if (dt.Rows[0]["AgreementAmount"].ToString() != "")
                txtAgreeamt.ReadOnly = true;

            hdn_id.Value = dt.Rows[0][0].ToString();

            btn_delete.Visible = hdn_delete.Value == "0" ? false : true;
            btn_save.Visible = hdn_update.Value == "0" ? false : true;
            btnchecklist.Visible = hdnchecklist.Value == "0" ? false : true;
            btnpayment.Visible = hdnpayment.Value == "0" ? false : true;
            pnl_add.Visible = true;
            Upd_Add_Panel.Update();
        }

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Insert_UpdateCompanyStaff(Convert.ToInt32(hdn_id.Value), txt_name.Text, txt_address.Text,
                txt_mob.Text,Convert.ToInt32(drpCompany.SelectedValue),txtAgreeamt.Text==""?0:Convert.ToDecimal(txtAgreeamt.Text),
                txt_email.Text, txt_remark.Text, Convert.ToInt32(hdn_user_id.Value));
            if (res > 0)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            pnl_add.Visible = false;
            Upd_Add_Panel.Update();
        }

        protected void btn_delete_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.DeleteCompanyStaff(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                lbl_msg.Text = "Deleted Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            pnl_add.Visible = false;
            Upd_Add_Panel.Update();
        }

        protected void btnchecklist_Click(object sender, EventArgs e)
        {
            pnlCheckList.Visible = true;
            fillchecklist();
            UpdCheckList.Update();
        }

        public void fillchecklist()
        {
            DataSet ds = obj_master.EditCompanyStaff(Convert.ToInt32(hdn_id.Value));
            DataTable dtchklist = ds.Tables[1];

            rptChecklist.DataSource = dtchklist;
            rptChecklist.DataBind();
            updCheckListIn.Update();
        }

        protected void rptChecklist_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdnDId");
            HiddenField hdnchkId = (HiddenField)e.Item.FindControl("hdnchkId");
            HiddenField hdnexpense = (HiddenField)e.Item.FindControl("hdnexpense");
          

            if (e.CommandName == "Complete")
            {
                int res = obj_master.InsertCompletechecklist(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_rpt_id.Value),
            Convert.ToInt32(hdnchkId.Value), Convert.ToInt32(hdn_user_id.Value));
                if (res > 0)
                    fillchecklist();
            }
            else if (e.CommandName == "Expense")
            {
                pnlexpense.Visible = true;
                txtExpense.Text = hdnexpense.Value;
                hdnDidExp.Value = hdn_rpt_id.Value;
                hdnchkIdExp.Value = hdnchkId.Value;
                updexpense.Update();
            }
            else if (e.CommandName == "UploadFile")
            {
                pnlfileup.Visible = true;
                hdnDidout.Value = hdn_rpt_id.Value;
                hdnchkIdOut.Value = hdnchkId.Value;
                Updfileup.Update();
            }
            else if (e.CommandName == "DownloadFile")
            {
                HiddenField hdnfilesave = (HiddenField)e.Item.FindControl("hdnfilenamesave");
                HiddenField hdnfilename = (HiddenField)e.Item.FindControl("hdnfilename");

                try
                {
                    if (hdnfilesave.Value != "")
                    {
                        string fil_name = hdnfilesave.Value;
                        string full_name = Server.MapPath("~/UploadedFiles/" + fil_name);
                        Response.ContentType = "APPLICATION/OCTET-STREAM";
                        String Header = "Attachment; Filename=\"" + hdnfilename.Value + "\"";
                        Response.AppendHeader("Content-Disposition", Header);
                        System.IO.FileInfo Dfile = new System.IO.FileInfo(Server.MapPath("~/UploadedFiles/" + fil_name));
                        Response.WriteFile(Dfile.FullName);
                        Response.End();
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        protected void btnExpenseaddClick(object sender, EventArgs e)
        {
                int res = obj_master.InsertExpensechecklist(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdnDidExp.Value),
           Convert.ToInt32(hdnchkIdExp.Value), Convert.ToDecimal(txtExpense.Text));
                if (res > 0)
                {
                    fillchecklist();
                    pnlexpense.Visible = false;
                    updexpense.Update();
                }
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Sorry Failed to process then request!');", true);
        }

        protected void btnExpenseaddclose_Click(object sender, EventArgs e)
        {
            pnlexpense.Visible = false;
            updexpense.Update();
        }

        protected void btnFileupload_Click(object sender, EventArgs e)
        {
            if (hdnfilenamesaveout.Value != "")
            {
                int res = obj_master.InsertFilechecklist(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdnDidout.Value),
           Convert.ToInt32(hdnchkIdOut.Value), hdnfilenameout.Value, hdnfilenamesaveout.Value);
                if (res > 0)
                {
                    fillchecklist();
                    pnlfileup.Visible = false;
                    Updfileup.Update();
                }
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Sorry Failed to process then request!');", true);
            }
        }

        protected void btnFUclose_Click(object sender, EventArgs e)
        {
            pnlfileup.Visible = false;
            Updfileup.Update();
        }

        protected void fu_fileupload_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            fu_fileupload.TargetFolder = "~/UploadedFiles";

            DataTable dtprefix = obj_common.Get_File_Code("AllFile");
            string files_namesave = dtprefix.Rows[0][0].ToString() + e.File.FileName;
            e.File.SaveAs(Path.Combine(Server.MapPath(fu_fileupload.TargetFolder), files_namesave));


            try
            {
                //in backup folder also
                DataTable dtgen = obj_master.Edit_GeneralSettings();
                File.Copy((Path.Combine(Server.MapPath(fu_fileupload.TargetFolder), files_namesave)),
                    (Path.Combine(dtgen.Rows[0]["BackupDrivePath"].ToString() + "UploadedFiles", files_namesave)), false);
            }
            catch (Exception cc) { }

            hdnfilenameout.Value = e.File.FileName;
            hdnfilenamesaveout.Value = files_namesave;

            Updfileup.Update();
        }

        protected void btnchecklistclose_Click(object sender, EventArgs e)
        {
            pnlCheckList.Visible = false;
            UpdCheckList.Update();
        }

        protected void btnpayment_Click(object sender, EventArgs e)
        {
            pnlPayment.Visible = true;
            fillPaymentdetail();
            Upd_PaymentPanel.Update();
        }

        public void fillPaymentdetail()
        {
            txtPay.Text = txtpaybal.Text = txtPayremark.Text = "";
            paydate.SelectedDate = DateTime.Now;
            DataSet ds = obj_master.EditCompanyStaff(Convert.ToInt32(hdn_id.Value));
            DataTable dt = ds.Tables[0];
            DataTable dtpay = ds.Tables[2];
            txtpaybal.Text = dt.Rows[0]["Balance"].ToString();
            pnlpaymenthistory.Visible = false;

            if (dtpay.Rows.Count>0)
            {
                rptpayhistory.DataSource = dtpay;
                rptpayhistory.DataBind();
                pnlpaymenthistory.Visible = true;
            }
            Upd_PaymentPanelIn.Update();
        }

        protected void btn_PaySave_Click(object sender, EventArgs e)
        {
            int res = obj_master.InsertCompanyStaffPayment(Convert.ToInt32(hdn_id.Value), paydate.SelectedDate,
              txtPayremark.Text,Convert.ToDecimal(txtPay.Text), Convert.ToInt32(hdn_user_id.Value));
            if (res > 0)
            {
                fillPaymentdetail();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
        }

        protected void btn_paydelete_Click(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;
            HiddenField hdnDId = (HiddenField)itemrp.FindControl("hdnDId");

            int res = obj_master.DeleteCompanyStaffPayment(Convert.ToInt32(hdn_id.Value),Convert.ToInt32(hdnDId.Value));
            if (res > 0)
            {
                fillPaymentdetail();
                lbl_msg.Text = "Deleted Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
        }

        protected void btnpayclose_Click(object sender, EventArgs e)
        {
            pnlPayment.Visible = false;
            Upd_PaymentPanel.Update();
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
            drpCompany.ClearSelection();
            drpCompany.Text = "";
            txt_address.Text = "";
            txt_mob.Text = "";
            txt_remark.Text = "";
            txt_email.Text =txtAgreeamt.Text=txtPaid.Text= "";
            txtAgreeamt.ReadOnly = false;
            hdn_id.Value = "0";

            btn_save.Visible = hdn_add.Value == "0" ? false : true;
            btn_delete.Visible =btnchecklist.Visible=btnpayment.Visible= false;

            Upd_Add_PanelInner.Update();
        }
       
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

        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.Form_Previlage_Validation(60, Convert.ToInt32(hdn_user_id.Value));
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

                    DataTable dt = obj_common.Action_Previlage_Validation(60, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdn_update.Value = dt.Rows[1][1].ToString();
                        hdn_delete.Value = dt.Rows[2][1].ToString();
                        hdnchecklist.Value = dt.Rows[3][1].ToString();
                        hdnpayment.Value = dt.Rows[4][1].ToString();
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