using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using AmarCentre.BAL;
using Telerik.Web.UI;
using System.Data.OleDb;
using Telerik.Web.UI;

namespace AmarCentre.Masters
{
    public partial class Customer : System.Web.UI.Page
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
                fill_Document();
                filldrops();
                Clear();
                grid_fill(1, 10, "", "", "");
            }
        }

        public void filldrops()
        {
            DataSet ds = obj_master.drpforcustomer();

            drpagent.Items.Clear();
            drpagent.DataSource = ds.Tables[0];
            drpagent.DataTextField = "Text";
            drpagent.DataValueField = "Value";
            drpagent.DataBind();
            drpagent.Text = "";

            drpSponser.Items.Clear();
            drpSponser.DataSource = ds.Tables[1];
            drpSponser.DataTextField = "Text";
            drpSponser.DataValueField = "Value";
            drpSponser.DataBind();
            drpSponser.Text = "";

            drpEmirate.Items.Clear();
            drpEmirate.DataSource = ds.Tables[2];
            drpEmirate.DataTextField = "Text";
            drpEmirate.DataValueField = "Value";
            drpEmirate.DataBind();
            drpEmirate.Text = "";

            drpCategory.Items.Clear();
            drpCategory.DataSource = ds.Tables[3];
            drpCategory.DataTextField = "Text";
            drpCategory.DataValueField = "Value";
            drpCategory.DataBind();
            drpCategory.Text = "";

            RadComboBoxItem CodeItem = new RadComboBoxItem();
            CodeItem.Text = "New Entry";
            CodeItem.Value = "0";
            drpCategory.Items.Insert(0, CodeItem);

            DataTable dt = ds.Tables[4];
            hdnDefaultEmirate.Value = dt.Rows[0]["DefaultEmirate"].ToString();
            hdnIsprofessionversion.Value =  dt.Rows[0]["IsProfessionVersion"].ToString();

            drpcompanygrp.Items.Clear();
            drpcompanygrp.DataSource = ds.Tables[5];
            drpcompanygrp.DataTextField = "Name";
            drpcompanygrp.DataValueField = "Id";
            drpcompanygrp.DataBind();
            drpcompanygrp.Text = "";
        }

        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_master.Get_List_Customer(page_number, page_size, filter, column, order);
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
            DataTable dt = obj_master.List_Customer_Excel();
            dt.Columns["Sl_No"].ColumnName = "Sl No.";
            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "Customer");

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
            DataSet ds = obj_master.Edit_Customer(Convert.ToInt32(hdn_rpt_id.Value));
            DataTable dt = ds.Tables[0];

            foreach (DataRow dr in ds.Tables[4].Rows)
            {
                RadComboBoxItem item = (RadComboBoxItem)(drpagent.FindItemByValue(dr["AgentId"].ToString()));
                item.Checked = true;
                item.Selected = true;
            }

            txt_name.Text = dt.Rows[0]["Name"].ToString();
            txtArabicName.Text = dt.Rows[0]["ArabicName"].ToString();
            txt_address.Text = dt.Rows[0]["Address"].ToString();
            txt_mob.Text = dt.Rows[0]["Mobile_num"].ToString();
            txt_phn.Text = dt.Rows[0]["Phone_num"].ToString();
            txt_remark.Text = dt.Rows[0]["Remark"].ToString();
            txt_email.Text = dt.Rows[0]["Email"].ToString();
            txt_trn.Text = dt.Rows[0]["TRN"].ToString();
            chk_IsCredit.Checked = Convert.ToBoolean(dt.Rows[0]["IsCredit"]);
            chkIsTyping.Checked = Convert.ToBoolean(dt.Rows[0]["IsTypingCenter"]);
            chkCommissionApplicable.Checked = Convert.ToBoolean(dt.Rows[0]["CommissionApplicable"]);
            pnl_CreditAmount.Visible = chk_IsCredit.Checked;
            txt_CreditAmount.Text= dt.Rows[0]["CreditAmount"].ToString();
            hdn_id.Value = dt.Rows[0]["Id"].ToString();
            txtCperson.Text = dt.Rows[0]["ContactPerson"].ToString();
            txtmohre.Text = dt.Rows[0]["MohreNo"].ToString();
            txtlicense.Text = dt.Rows[0]["licenseNo"].ToString();
            drpSponser.SelectedValue = dt.Rows[0]["SponserId"].ToString();
            lbl_cusName.Text = dt.Rows[0]["Name"].ToString();
            lbl_StaffCusName.Text = dt.Rows[0]["Name"].ToString();
            drpEmirate.SelectedValue = dt.Rows[0]["EmiratesId"].ToString();
            drpCategory.SelectedValue = dt.Rows[0]["CategoryId"].ToString();
            txtccmail.Text = dt.Rows[0]["CCMail"].ToString();
            txtWhatsappNo.Text = dt.Rows[0]["WhatsappNo"].ToString();
            txt_userName.Text = dt.Rows[0]["UserName"].ToString();
            txt_password.Text = dt.Rows[0]["Passwords"].ToString();
            txt_password.Attributes["value"] = dt.Rows[0]["Passwords"].ToString();
            txt_userName.ReadOnly = dt.Rows[0]["UserName"].ToString() != "" ? true : false;
            drpcompanygrp.SelectedValue = dt.Rows[0]["CompanyGroupId"].ToString();
            chkcompanygrp.Checked = Convert.ToBoolean(dt.Rows[0]["IsMainCompany"]);

            lblPayable.Text = dt.Rows[0]["TotalPayable"].ToString();
            lblReceivable.Text = dt.Rows[0]["Receivable"].ToString();
            
            btn_delete.Visible = hdn_delete.Value == "0" ? false : true;
            btn_save.Visible = hdn_update.Value == "0" ? false : true;
            btn_OB.Visible = hdn_OB.Value == "0" ? false : true;
            btn_doc.Visible = hdn_doc.Value == "0" ? false : true;
            btn_doc_Staff.Visible = hdn_doc_Staff.Value == "0" ? false : true;
            btn_Mailhistory.Visible = hdn_histry.Value == "0" ? false : true;
            btnmail.Visible = hdnmail.Value == "0" ? false : true;

            btn_usercred.Visible = hdn_cred.Value == "0" ? false : true;
            btn_serviceDiscount.Visible = hdn_servicediscount.Value == "0" ? false : true;
            btnMenuPrivilge.Visible = hdnmenuprivilege.Value == "0" ? false : true;
            pnl_add.Visible = true;
            Upd_Add_Panel.Update();
        }

        protected void btnmail_OnClick(object sender, EventArgs e)
        {
            DataTable dt = obj_master.CustomerMail(Convert.ToInt32(hdn_id.Value));
            if (dt.Rows.Count > 0)
            {
                EmailUC.UCPageLoad(7, Convert.ToInt32(hdn_id.Value), dt.Rows[0]["Toaddress"].ToString());
                pnlMail.Visible = true;
                UpdMailPanel.Update();
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            //pnl_add.Visible = false;
            //Upd_Add_Panel.Update();
        }

        protected void chk_IsCredit_OnCheckedChanged(object sender, EventArgs e)
        {
            txt_CreditAmount.Text = "";
            pnl_CreditAmount.Visible = chk_IsCredit.Checked;
            Upd_CreditAmount_Panel.Update();
        }

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            DataTable dt_agnt = new DataTable();
            dt_agnt.Columns.Add("Id", typeof(int));

            foreach (RadComboBoxItem item in drpagent.Items)
            {
                if (item.Checked)
                {
                    DataRow dr = dt_agnt.NewRow();
                    dt_agnt.Rows.Add(Convert.ToInt32(item.Value));
                }
            }

            int res = obj_master.Insert_Update_Customer(Convert.ToInt32(hdn_id.Value), txt_name.Text, txt_address.Text,
                txt_mob.Text, txt_phn.Text, txt_email.Text, txt_remark.Text, Convert.ToInt32(hdn_user_id.Value), txt_trn.Text
                , Convert.ToInt32(chk_IsCredit.Checked), txt_CreditAmount.Text == "" ? (decimal?)null : Convert.ToDecimal(txt_CreditAmount.Text),
                Convert.ToInt32(chkCommissionApplicable.Checked), txtArabicName.Text, dt_agnt, txtCperson.Text, txtmohre.Text, txtlicense.Text,
                drpSponser.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpSponser.SelectedValue),
                Convert.ToInt32(chkIsTyping.Checked), drpEmirate.SelectedValue==""?(int?)null:Convert.ToInt32(drpEmirate.SelectedValue),
                 drpCategory.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpCategory.SelectedValue),txtccmail.Text,txtWhatsappNo.Text,
                 txt_userName.Text,txt_password.Text,drpcompanygrp.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpcompanygrp.SelectedValue),
                Convert.ToInt32(chkcompanygrp.Checked));
            if (res == -1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Mobile Number Already exist.!');", true);
            }

            else if (res > 0)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                pnl_add.Visible = false;
                Upd_Add_Panel.Update();

            }
            else
            {
                lblerrormsg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
                pnl_add.Visible = false;
                Upd_Add_Panel.Update();

            }

        }

        protected void btn_delete_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Delete_Customer(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_user_id.Value));
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

        protected void drpCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (drpCategory.SelectedValue == "0")
            {
                pnlCategory.Visible = true;
                UCCategory.PageLoad();
                updCategoryPanel.Update();
            }
        }

        public void Clear()
        {
            txt_name.Text = "";
            txtArabicName.Text = "";
            txt_userName.Text = txt_password.Text = "";
            txt_password.Attributes["value"] = "";
            txt_userName.ReadOnly = false;
            txt_address.Text = "";
            txt_mob.Text = txtWhatsappNo.Text= "";
            txt_phn.Text = "";
            txt_remark.Text = "";
            txt_email.Text = "";
            drp_obType.ClearSelection();
            drp_obType.Text = "";
            txt_open_bal.Text = "";
            ob_date.DbSelectedDate = DateTime.Now;
            txt_trn.Text = "";
            chk_IsCredit.Checked =chkIsTyping.Checked= false;
            txt_CreditAmount.Text = "";
            pnl_CreditAmount.Visible = chk_IsCredit.Checked;
            chkCommissionApplicable.Checked = false;
            drpagent.Text = string.Empty;
            drpagent.ClearCheckedItems();
            txtCperson.Text = txtmohre.Text = txtlicense.Text = "";
            drpSponser.ClearSelection();
            drpSponser.Text = "";
            drpEmirate.ClearSelection();
            drpEmirate.Text = "";
            drpCategory.ClearSelection();
            drpCategory.Text =txtccmail.Text= "";
            drpEmirate.SelectedValue = hdnDefaultEmirate.Value;
            chkcompanygrp.Checked = false;
            drpcompanygrp.ClearSelection();
            drpcompanygrp.Text = "";
            pnlchkcompanygrp.Visible= pnlcompanygrp.Visible=(hdnIsprofessionversion.Value=="1"?true:false);

            Txt_imi_name.Text = "";
            txt_u_id.Text = "";
            Txt_im_pass.Text = "";
            Txt_im_bu.Text = "";
            txt_im_bp.Text = "";
            Txt_im_bkpin.Text = "";
            txt_im_rsa_pin.Text = "";

            Txt_mun_name.Text = "";
            Txt_dm_user.Text = "";
            Txt_dm_pass.Text = "";
            Txt_Ad_user.Text = "";
            Txt_ad_pass.Text = "";
            Txt_em_user.Text = "";
            Txt_em_pass.Text = "";

            Txt_thu_name.Text = "";
            Txt_sup_user.Text = "";
            Txt_thu_pass.Text = "";
            Txt_thu_usr.Text = "";
            Txt_thu_passs.Text = "";
            txt_thu_mail.Text = "";
            txt_thu_mob.Text = "";

            Txt_net_user.Text = "";
            Txt_net_pass.Text = "";
            Txt_net_mail.Text = "";
            Txt_net_mob.Text = "";

            hdn_id.Value = "0";

            lblPayable.Text = "";
            lblReceivable.Text = "";
            btn_save.Visible = hdn_add.Value == "0" ? false : true;
            btn_delete.Visible = false;
            btn_OB.Visible = false;
            btn_doc.Visible =btn_doc_Staff.Visible= false;
            btn_usercred.Visible = false;
            btn_serviceDiscount.Visible = false;
            btnMenuPrivilge.Visible = false;
            btn_Mailhistory.Visible=btnmail.Visible= false; 

            Upd_Add_PanelInner.Update();
        }
        //Search
        protected void txt_search_OnTextChanged(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), txt_search.Text, Common_order_column.Value, Common_asc_desc.Value);
            Upd_List_Panel.Update();
        }

        #region MailHistory

        protected void btn_Mailhistry_OnClick(object sender, EventArgs e)
        {
            //MailHistory
            drpCustStaff.Items.Clear();
            DataTable dt2 = obj_master.fill_drp_CustomerStaff(Convert.ToInt32(hdn_id.Value));
            drpCustStaff.DataSource = dt2;
            drpCustStaff.DataTextField = "Text";
            drpCustStaff.DataValueField = "Value";
            drpCustStaff.DataBind();
            drpCustStaff.Text = "";

            date_from.SelectedDate = null;
            date_to.SelectedDate = null;
            drpCustStaff.ClearSelection();
            drpCustStaff.Text = "";

            drpDocument.ClearSelection();
            drpDocument.Text = "";

            grid_fill_his(1, 12);

            PanelHis.Visible = true;
            UpdatePanel1.Update();
        }

        protected void btn_his_seacrh_OnClick(object sender, EventArgs e)
        {
            grid_fill_his(1, 10);

            Upd_History.Update();
        }

        protected void btnexcel_exportHis_OnClick(object sender, EventArgs e)
        {
            DataSet ds = obj_master.list_CustMailHistry(date_from.SelectedDate, date_to.SelectedDate, Convert.ToInt32(hdn_id.Value),
                  drpDocument.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpDocument.SelectedValue),
                 drpCustStaff.SelectedValue,
               1, Convert.ToInt32(drp_count1.SelectedValue));
            DataTable dt = ds.Tables[0];

            dt.Columns.Remove("current_count");
            dt.Columns.Remove("page_number");
            dt.Columns.Remove("Page_size");
            dt.Columns.Remove("start_num");
            dt.Columns.Remove("end_num");
            dt.Columns.Remove("last_page");

            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "CustomerMailhistory");
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                HttpContext.Current.Response.Write(style);
                Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        protected void btn_histry_Close_OnClick(object sender, EventArgs e)
        {

            PanelHis.Visible = false;
            UpdatePanel1.Update();

        }

        public void grid_fill_his(int page_number, int page_size)
        {


            DataSet ds = obj_master.list_CustMailHistry(date_from.SelectedDate, date_to.SelectedDate, Convert.ToInt32(hdn_id.Value),
                drpDocument.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpDocument.SelectedValue),
                 drpCustStaff.SelectedValue,
                page_number, page_size);
            DataTable dt = ds.Tables[0];

            rpt_His.DataSource = dt;
            rpt_His.DataBind();

            if (dt.Rows.Count > 0)
            {
                lbl_page_info1.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["SLNo"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
                hdn_last_page1.Value = dt.Rows[0]["last_page"].ToString();
                lbl_page_number1.Text = dt.Rows[0]["page_number"].ToString();
                hdn_total1.Value = dt.Rows[0]["current_count"].ToString();
            }
            else
            {
                lbl_page_info1.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
                hdn_last_page1.Value = "0";
                lbl_page_number1.Text = "1";
                hdn_total1.Value = "0";
            }
            upd_his_nav.Update();
            Upd_History.Update();
        }

        #region his Navigation

        //First Page
        protected void btn_first1_Mail_OnClick(object sender, EventArgs e)
        {
            grid_fill_his(1, Convert.ToInt32(drp_count1.SelectedValue));
        }

        //Previous Page
        protected void btn_prev1_Mail_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number1.Text) > 1)
            {
                grid_fill_his(Convert.ToInt32(lbl_page_number1.Text) - 1, Convert.ToInt32(drp_count1.SelectedValue));
            }
        }

        //Next Page
        protected void btn_next1_Mail_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number1.Text) < Convert.ToInt32(hdn_last_page1.Value))
            {
                grid_fill_his(Convert.ToInt32(lbl_page_number1.Text) + 1, Convert.ToInt32(drp_count1.SelectedValue));
            }
        }

        //Last Page
        protected void btn_last1_Mail_OnClick(object sender, EventArgs e)
        {
            grid_fill_his(Convert.ToInt32(hdn_last_page1.Value), Convert.ToInt32(drp_count1.SelectedValue));
        }

        //Page Data Count
        protected void drp_count1_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            grid_fill_his(1, Convert.ToInt32(drp_count1.SelectedValue));
        }

        #endregion

        #endregion

        #region Add user credtnl

        protected void btn_usercred_OnClick(object sender, EventArgs e)
        {
            pnl_User_Cred.Visible = true;
            DataSet ds = obj_master.Edit_Customer(Convert.ToInt32(hdn_id.Value));
            DataTable dt4 = ds.Tables[3];
            if (dt4.Rows.Count > 0)
            {
                Txt_imi_name.Text = dt4.Rows[0]["io_cmpname"].ToString();
                txt_u_id.Text = dt4.Rows[0]["io_uid"].ToString();
                Txt_im_pass.Text = dt4.Rows[0]["io_pass"].ToString();
                Txt_im_bu.Text = dt4.Rows[0]["io_bankuser"].ToString();
                txt_im_bp.Text = dt4.Rows[0]["io_bnkpass"].ToString();
                Txt_im_bkpin.Text = dt4.Rows[0]["io_bnhpin"].ToString();
                txt_im_rsa_pin.Text = dt4.Rows[0]["io_rsapin"].ToString();

                Txt_mun_name.Text = dt4.Rows[0]["dm_cmpname"].ToString();
                Txt_dm_user.Text = dt4.Rows[0]["dm_user"].ToString();
                Txt_dm_pass.Text = dt4.Rows[0]["dm_pass"].ToString();
                Txt_Ad_user.Text = dt4.Rows[0]["dm_adminuser"].ToString();
                Txt_ad_pass.Text = dt4.Rows[0]["dm_admpass"].ToString();
                Txt_em_user.Text = dt4.Rows[0]["dm_emausr"].ToString();
                Txt_em_pass.Text = dt4.Rows[0]["dm_empass"].ToString();


                Txt_thu_name.Text = dt4.Rows[0]["tu_cmpname"].ToString();
                Txt_sup_user.Text = dt4.Rows[0]["tu_spuser"].ToString();
                Txt_thu_pass.Text = dt4.Rows[0]["tu_pass"].ToString();
                Txt_thu_usr.Text = dt4.Rows[0]["tu_thuser"].ToString();
                Txt_thu_passs.Text = dt4.Rows[0]["tu_thpass"].ToString();
                txt_thu_mail.Text = dt4.Rows[0]["tu_emaid"].ToString();
                txt_thu_mob.Text = dt4.Rows[0]["tu_mobile"].ToString();


                Txt_net_user.Text = dt4.Rows[0]["ns_user"].ToString();
                Txt_net_pass.Text = dt4.Rows[0]["ns_pass"].ToString();
                Txt_net_mail.Text = dt4.Rows[0]["ns_email"].ToString();
                Txt_net_mob.Text = dt4.Rows[0]["ns_mobile"].ToString();
            }
            Upd_User_Cred_Panel.Update();
        }

        protected void btn_credSave_click(object sender, EventArgs e)
        {
            int res = obj_master.Update_CustCredtnl(Convert.ToInt32(hdn_id.Value), Txt_imi_name.Text, txt_u_id.Text, Txt_im_pass.Text, Txt_im_bu.Text, txt_im_bp.Text,
                Txt_im_bkpin.Text, txt_im_rsa_pin.Text, Txt_mun_name.Text, Txt_dm_user.Text, Txt_dm_pass.Text, Txt_Ad_user.Text, Txt_ad_pass.Text,
                Txt_em_user.Text, Txt_em_pass.Text, Txt_thu_name.Text, Txt_sup_user.Text, Txt_thu_pass.Text, Txt_thu_usr.Text, Txt_thu_passs.Text,
                txt_thu_mail.Text, txt_thu_mob.Text, Txt_net_user.Text, Txt_net_pass.Text, Txt_net_mail.Text, Txt_net_mob.Text, Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                pnl_User_Cred.Visible = false;
            }
            else
            {
            }
            Upd_User_Cred_Panel.Update();
        }

        protected void cre_link_OnClick(object sender, EventArgs e)
        {
            pnl_User_Cred.Visible = false;
            Upd_User_Cred_Panel.Update();
        }

        #endregion


        #region Opening balance

        protected void btn_OB_OnClick(object sender, EventArgs e)
        {
            pnl_obalance.Visible = true;
            DataTable dt = obj_master.GetCustomerOB(Convert.ToInt32(hdn_id.Value));

            drp_obType.Enabled = ob_date.Enabled = btnOBClear.Enabled = true;
            txt_open_bal.ReadOnly = false;

            if (dt.Rows[0]["IsEditAllow"].ToString()=="1")
            {
                btn_OBSave.Enabled = false;
                btnOBClear.Enabled = true;
                if(dt.Rows[0]["OpeningBalanceType"].ToString()=="")
                {
                    btn_OBSave.Enabled = true;
                    btnOBClear.Enabled = false;
                }
            }
            if (dt.Rows[0]["IsEditAllow"].ToString() == "0")
            {
                drp_obType.Enabled = ob_date.Enabled = false;
                txt_open_bal.ReadOnly = true;
                btn_OBSave.Enabled =  false; //btnOBClear.Enabled =
            }

            drp_obType.SelectedValue = dt.Rows[0]["OpeningBalanceType"].ToString();
            txt_open_bal.Text = dt.Rows[0]["OBal"].ToString();
            ob_date.DbSelectedDate = dt.Rows[0]["ODate"].ToString();
            lblerr.Text= dt.Rows[0]["Errormsg"].ToString();

            Upd_OB_Panel.Update();
        }

        protected void btn_OBClear_OnClick(object sender, EventArgs e)
        {
            if (lblerr.Text != "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('"+lblerr.Text+"');", true);
            }
            else
            {
                int res = obj_master.ClearCustomerOB(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_user_id.Value));
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
            }
            Upd_OBIn.Update();
        }

        protected void btn_OBSave_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Update_OB_Customer(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(drp_obType.SelectedValue),
                Convert.ToDecimal(txt_open_bal.Text), ob_date.SelectedDate,
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


        #region Document Upload

        protected void btn_docadd_OnClick(object sender, EventArgs e)
        {
            pnl_document.Visible = true;
            Clear_documnt();
            fill_rpt( 1, Convert.ToInt32(drp_countD.SelectedValue));

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
            txt_docname.Text = "";
            txt_docremark.Text = "";
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
            int res = obj_master.Update_CustomerDocument(Convert.ToInt32(hdn_doc_index_Id.Value), Convert.ToInt32(hdn_id.Value),
                Convert.ToInt32(drp_doc.SelectedValue), txt_doc_no.Text, valid_from.SelectedDate,
               valid_to.SelectedDate, txt_docname.Text, txt_docremark.Text, txtValidityyr.Text == "" ? (int?)null :
               Convert.ToInt32(txtValidityyr.Text), Convert.ToInt32(hdn_user_id.Value), hdn_doc_name.Value, hdn_doc_sav.Value
               );

            if (res == 1)
            {
                Clear_documnt();
            }
            else
            {
            }
            fill_rpt(1, Convert.ToInt32(drp_countD.SelectedValue));
            Upd_docadd.Update();
        }

        public void fu_documents_OnFileUploaded(object sender, FileUploadedEventArgs e)
        {
            
            fu_documents.TargetFolder = "~/UploadedFiles";
            DataTable dt = obj_common.Get_File_Code("CustDoc");
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

        public void fill_Document()
        {
            drp_doc.Items.Clear();
            DataTable dt = obj_master.fill_drp_DocType();
            drp_doc.DataSource = dt;
            drp_doc.DataTextField = "Text";
            drp_doc.DataValueField = "Value";
            drp_doc.DataBind();
            drp_doc.Text = "";

            drp_doc_Staff.Items.Clear();
            drp_doc_Staff.DataSource = dt;
            drp_doc_Staff.DataTextField = "Text";
            drp_doc_Staff.DataValueField = "Value";
            drp_doc_Staff.DataBind();
            drp_doc_Staff.Text = "";

            //MailHistory

            drpDocument.Items.Clear();
            DataTable dt1 = obj_master.fill_drp_DocType();
            drpDocument.DataSource = dt1;
            drpDocument.DataTextField = "Text";
            drpDocument.DataValueField = "Value";
            drpDocument.DataBind();
            drpDocument.Text = "";

          


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
                HiddenField hdn_idDoc = (HiddenField)e.Item.FindControl("hdn_id");
                HiddenField hdn_doc_Id = (HiddenField)e.Item.FindControl("hdn_doc_Id");
                HiddenField hdn_dnm = (HiddenField)e.Item.FindControl("hdn_dnm");
                Label lbl_doc_name = (Label)e.Item.FindControl("lbl_doc_name");
                Label lbl_doc_type_name = (Label)e.Item.FindControl("lbl_doc_type_name");
                Label lbl_docnum = (Label)e.Item.FindControl("lbl_docnum");
                Label lbl_docname = (Label)e.Item.FindControl("lbl_docname");
                Label lbl_remark = (Label)e.Item.FindControl("lbl_remark");

                Label lbl_from = (Label)e.Item.FindControl("lbl_from");
                Label lbl_to = (Label)e.Item.FindControl("lbl_to");
                HiddenField v_frm = (HiddenField)e.Item.FindControl("v_frm");
                HiddenField v_to = (HiddenField)e.Item.FindControl("v_to");
                HiddenField hdnVyr = (HiddenField)e.Item.FindControl("hdnVyr");

                Clear_documnt();

                drp_doc.SelectedValue = hdn_doc_Id.Value;
                valid_from.DbSelectedDate = v_frm.Value == "" ? (DateTime?)null :  Convert.ToDateTime(v_frm.Value);
                valid_to.DbSelectedDate = v_to.Value == "" ? (DateTime?)null : Convert.ToDateTime(v_to.Value);
                lab_doc_name_out.Text = lbl_doc_name.Text;
                hdn_doc_name.Value = lbl_doc_name.Text;
                hdn_doc_sav.Value = hdn_dnm.Value;
                hdn_doc_index_Id.Value = hdn_idDoc.Value;
                txt_doc_no.Text = lbl_docnum.Text;
                txt_docname.Text = lbl_docname.Text;
                txt_docremark.Text = lbl_remark.Text;
                txtValidityyr.Text = hdnVyr.Value;

                Upd_docadd.Update();
            }
        }

        protected void btn_remove_line_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;
            HiddenField hdn_idDoc = (HiddenField)itemrp.FindControl("hdn_id");

            obj_master.DeleteCustomerDocument(Convert.ToInt32(hdn_idDoc.Value));

            fill_rpt( 1, Convert.ToInt32(drp_countD.SelectedValue));

            Upd_doc.Update();
        }

        protected void btn_reset_doc_OnClick(object sender, EventArgs e)
        {
            Clear_documnt();
            Upd_docadd.Update();
        }

        protected void txt_doc_search_OnTextChanged(object sender, EventArgs e)
        {
            fill_rpt( 1, Convert.ToInt32(drp_countD.SelectedValue));
        }

        public void fill_rpt(int PageNo, int count)
        {
            DataSet ds = obj_master.ListCustomerDocument(PageNo, count, txt_search_doc.Text, Convert.ToInt32(hdn_id.Value));
            DataTable dt = ds.Tables[0];
            rpt_doc_list.DataSource = dt;
            rpt_doc_list.DataBind();

            if (dt.Rows.Count > 0)
            {
                lbl_page_infoD.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["dt_indx"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
                hdn_last_pageD.Value = dt.Rows[0]["last_page"].ToString();
                lbl_page_numberD.Text = dt.Rows[0]["page_number"].ToString();
                hdn_totalD.Value = dt.Rows[0]["current_count"].ToString();
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
            fill_rpt( 1, Convert.ToInt32(drp_countD.SelectedValue));
        }

        //Previous Page
        protected void btn_prev1_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_numberD.Text) > 1)
            {
                fill_rpt( Convert.ToInt32(lbl_page_numberD.Text) - 1, Convert.ToInt32(drp_countD.SelectedValue));
            }
        }

        //Next Page
        protected void btn_next1_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_numberD.Text) < Convert.ToInt32(hdn_last_pageD.Value))
            {
                fill_rpt( Convert.ToInt32(lbl_page_numberD.Text) + 1, Convert.ToInt32(drp_countD.SelectedValue));
            }
        }

        //Last Page
        protected void btn_last1_OnClick(object sender, EventArgs e)
        {
            fill_rpt(Convert.ToInt32(hdn_last_pageD.Value), Convert.ToInt32(drp_countD.SelectedValue));
        }

        //Page Data Count
        protected void drp_countD_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            fill_rpt( 1, Convert.ToInt32(drp_countD.SelectedValue));
        }

        #endregion

        #endregion

        #region Document Upload staff

        protected void btnStaffDocUpload_Click(object sender, EventArgs e)
        {
            DataTable ContentTable = new DataTable();
            ContentTable.Columns.Add("StaffName", typeof(string));
            ContentTable.Columns.Add("ContactNo", typeof(string));
            ContentTable.Columns.Add("DocumentType", typeof(string));
            ContentTable.Columns.Add("DocumentName", typeof(string));
            ContentTable.Columns.Add("DocumentNumber", typeof(string));
            ContentTable.Columns.Add("ValidFrom", typeof(DateTime));
            ContentTable.Columns.Add("ExpiryDate", typeof(DateTime));

            if (hdnStaffFile.Value != "")
            {
                string connString = "";
                string filepath = Path.Combine(Server.MapPath("~/UploadedFiles"), hdnStaffFile.Value);
                if (hdnStafffileExtension.Value == ".xls")
                {
                    //Connectionstring for excel v8.0    
                    connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1;TypeGuessRows=0\"";
                }

                OleDbConnection OledbConn = new OleDbConnection(connString);
                try
                {

                    OleDbCommand OledbCmd = new OleDbCommand();
                    OledbCmd.Connection = OledbConn;
                    OledbConn.Open();
                    var sheetNames = OledbConn.GetSchema("Tables");

                    OledbCmd.CommandText = "Select * from [" + sheetNames.Rows[0]["TABLE_NAME"].ToString() + "]";
                    OleDbDataReader dr = OledbCmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            try
                            {
                                if (dr["StaffName"].ToString().Trim() != string.Empty && dr["DocumentType"].ToString().Trim() != string.Empty)

                                    ContentTable.Rows.Add(dr["StaffName"].ToString().Trim(), dr["ContactNo"].ToString().Trim(),
                                        dr["DocumentType"].ToString().Trim(), dr["DocumentName"].ToString().Trim(), dr["DocumentNumber"].ToString().Trim(),
                                      dr["ValidFrom"].ToString().Trim() == string.Empty ? (DateTime?)null : Convert.ToDateTime(dr["ValidFrom"].ToString().Trim()),
                                       dr["ExpiryDate"].ToString().Trim() == string.Empty ? (DateTime?)null : Convert.ToDateTime(dr["ExpiryDate"].ToString().Trim()));
                            }
                            catch (Exception ex1)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('" + ex1.Message + "');", true);

                            }
                        }
                    }

                    dr.Close();
                    OledbConn.Close();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please upload the correct file format');", true);
                }
            }

            if (ContentTable.Rows.Count > 0)
            {
                int res = obj_master.Update_CustomerDocument_StaffbyFile(Convert.ToInt32(hdn_id.Value), ContentTable);

                if (res == 1)
                {
                    Clear_documnt_Staff();
                    fill_rpt_Staff(1, Convert.ToInt32(drp_countD_Staff.SelectedValue));
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Uploaded Successfully');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Sorry failed to process your request. Try again');", true);
                }
            }
        }

        public void fu_DocUpload_OnFileUploaded(object sender, FileUploadedEventArgs e)
        {
            fu_DocUpload.TargetFolder = "~/UploadedFiles";
            DataTable dt = obj_common.Get_File_Code("AllFile");

            string Prefix = "F-";
            if (dt.Rows.Count > 0 & dt.Rows[0][0].ToString() != "")
                Prefix = dt.Rows[0][0].ToString();

            string files_name = hdnStaffFile.Value = Prefix + e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();
            e.File.SaveAs(Path.Combine(Server.MapPath(fu_DocUpload.TargetFolder), files_name));

            try
            {
                //in backup folder also
                DataTable dtgen = obj_master.Edit_GeneralSettings();
                File.Copy((Path.Combine(Server.MapPath(fu_DocUpload.TargetFolder), files_name)),
                    (Path.Combine(dtgen.Rows[0]["BackupDrivePath"].ToString() + "UploadedFiles", files_name)), false);
            }
            catch (Exception cc) { }

            hdnStafffileExtension.Value = e.File.GetExtension();

            updStaffFile.Update();
        }

        protected void btnstaffdocformatDwn_Click(object sender, EventArgs e)
        {
            try
            {
                string fil_name = "StaffDocumentFormat.xls";
                string full_name = Server.MapPath("~/UploadedFiles/" + fil_name);
                Response.ContentType = "APPLICATION/OCTET-STREAM";
                String Header = "Attachment; Filename=\"" + fil_name + "\"";
                Response.AppendHeader("Content-Disposition", Header);
                System.IO.FileInfo Dfile = new System.IO.FileInfo(Server.MapPath("~/UploadedFiles/" + fil_name));
                Response.WriteFile(Dfile.FullName);
                //Don't forget to add the following line
                Response.End();
            }
            catch (Exception ex)
            {
            }
        }

        protected void btn_docadd_OnClick_Staff(object sender, EventArgs e)
        {
            pnl_document_Staff.Visible = true;
            fill_rpt_Staff( 1, Convert.ToInt32(drp_countD_Staff.SelectedValue));

            Clear_documnt_Staff();
            Upd_Document_Panel_Staff.Update();
        }

        protected void btn_Docclose_OnClick_Staff(object sender, EventArgs e)
        {
            pnl_document_Staff.Visible = false;
            Upd_Document_Panel_Staff.Update();
        }

        public void Clear_documnt_Staff()
        {
            drp_doc_Staff.ClearSelection();
            drp_doc_Staff.Text = "";
            valid_from_Staff.SelectedDate = null;
            valid_to_Staff.SelectedDate = null;
            hdn_doc_name_Staff.Value = "";
            lab_doc_name_out_Staff.Text = "";
            hdn_doc_sav_Staff.Value = "";
            txt_doc_no_Staff.Text = "";
            txt_docname_Staff.Text = "";
            txt_docremark_Staff.Text = "";
            hdn_doc_index_Id_Staff.Value = "0";
            txt_staff.Text = "";
            txt_staffmob.Text = "";
            txtvalidtiyStaff.Text = "";
            hdnStaffFile.Value = hdnStafffileExtension.Value= "";

            updStaffFile.Update();
            Upd_docadd_Staff.Update();
        }

        protected void txtvalidtiyStaffTextChanged(object sender, EventArgs e)
        {
            DateTime? Expirydate = null;
            if (txtvalidtiyStaff.Text != "" && valid_from_Staff.DbSelectedDate != null)
            {
                Expirydate = valid_from_Staff.SelectedDate.Value.AddYears(Convert.ToInt32(txtvalidtiyStaff.Text));
                valid_to_Staff.SelectedDate = Expirydate.Value.AddDays(-1);
                updVToStaff.Update();
            }
        }

        protected void btn_DocSave_OnClick_Staff(object sender, EventArgs e)
        {
            int res = obj_master.Update_CustomerDocumentStaff(Convert.ToInt32(hdn_doc_index_Id_Staff.Value), Convert.ToInt32(hdn_id.Value), txt_staff.Text,
               txt_staffmob.Text, Convert.ToInt32(drp_doc_Staff.SelectedValue), txt_doc_no_Staff.Text, valid_from_Staff.SelectedDate,
               valid_to_Staff.SelectedDate, txt_docname_Staff.Text, txt_docremark_Staff.Text, txtvalidtiyStaff.Text == "" ? (int?)null :
               Convert.ToInt32(txtvalidtiyStaff.Text), Convert.ToInt32(hdn_user_id.Value), hdn_doc_name_Staff.Value, hdn_doc_sav_Staff.Value
               );

            string name = txt_staff.Text;
            string mob = txt_staffmob.Text;

            if (res == 1)
            {
                Clear_documnt_Staff();
            }
            else
            {
            }
            fill_rpt_Staff(1, Convert.ToInt32(drp_countD_Staff.SelectedValue));
            txt_staff.Text = name;
            txt_staffmob.Text = mob;
            Upd_docadd_Staff.Update();
        }

        public void fu_documents_OnFileUploaded_Staff(object sender, FileUploadedEventArgs e)
        {
            fu_documents_Staff.TargetFolder = "~/UploadedFiles";
            DataTable dt = obj_common.Get_File_Code("CustDoc");
            if (dt.Rows.Count > 0 & dt.Rows[0][0].ToString() != "")
            {
                string files_name = dt.Rows[0][0].ToString() + e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();
                e.File.SaveAs(Path.Combine(Server.MapPath(fu_documents_Staff.TargetFolder), files_name));

                try
                {
                    //in backup folder also
                    DataTable dtgen = obj_master.Edit_GeneralSettings();
                    File.Copy((Path.Combine(Server.MapPath(fu_documents_Staff.TargetFolder), files_name)),
                        (Path.Combine(dtgen.Rows[0]["BackupDrivePath"].ToString() + "UploadedFiles", files_name)), false);
                }
                catch (Exception cc) { }

                hdn_doc_name_Staff.Value = e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();
                hdn_doc_sav_Staff.Value = files_name;
                lab_doc_name_out_Staff.Text = hdn_doc_name_Staff.Value;
            }
        }

        protected void rpt_doc_list_OnItemCommand_Staff(object s, RepeaterCommandEventArgs e)
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
                Label lbl_docname = (Label)e.Item.FindControl("lbl_docname");
                Label lbl_remark = (Label)e.Item.FindControl("lbl_remark");
                Label lbl_staffname = (Label)e.Item.FindControl("lbl_staffname");
                Label lbl_staffNo = (Label)e.Item.FindControl("lbl_staffNo");

                Label lbl_from = (Label)e.Item.FindControl("lbl_from");
                Label lbl_to = (Label)e.Item.FindControl("lbl_to");
                HiddenField v_frm = (HiddenField)e.Item.FindControl("v_frm");
                HiddenField v_to = (HiddenField)e.Item.FindControl("v_to");
                HiddenField hdnVyr = (HiddenField)e.Item.FindControl("hdnVyr");

                Clear_documnt_Staff();

                drp_doc_Staff.SelectedValue = hdn_doc_Id.Value;
                valid_from_Staff.DbSelectedDate = v_frm.Value == "" ? (DateTime?)null : Convert.ToDateTime(v_frm.Value);
                valid_to_Staff.DbSelectedDate = v_to.Value == "" ? (DateTime?)null : Convert.ToDateTime(v_to.Value);
                lab_doc_name_out_Staff.Text = lbl_doc_name.Text;
                hdn_doc_name_Staff.Value = lbl_doc_name.Text;
                hdn_doc_sav_Staff.Value = hdn_dnm.Value;
                hdn_doc_index_Id_Staff.Value = hdn_id.Value;
                txt_doc_no_Staff.Text = lbl_docnum.Text;
                txt_docname_Staff.Text = lbl_docname.Text;
                txt_docremark_Staff.Text = lbl_remark.Text;
                txt_staff.Text = lbl_staffname.Text;
                txt_staffmob.Text = lbl_staffNo.Text;
                txtvalidtiyStaff.Text = hdnVyr.Value;

                Upd_docadd_Staff.Update();
            }
        }

        protected void btn_remove_line_OnClick_Staff(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            HiddenField hdn_id = (HiddenField)itemrp.FindControl("hdn_id");
            obj_master.DeleteCustomerStaffDocument(Convert.ToInt32(hdn_id.Value));
            fill_rpt_Staff(1, Convert.ToInt32(drp_countD_Staff.SelectedValue));

            Upd_doc_Staff.Update();
        }

        protected void btn_reset_doc_OnClick_Staff(object sender, EventArgs e)
        {
            Clear_documnt_Staff();
            Upd_docadd_Staff.Update();
        }

        protected void txt_doc_search_OnTextChanged_Staff(object sender, EventArgs e)
        {
            fill_rpt_Staff(1, Convert.ToInt32(drp_countD_Staff.SelectedValue));
        }

        public void fill_rpt_Staff( int PageNo, int count)
        {
            DataSet ds = obj_master.ListCustomerStaffDocument(PageNo, count, txt_search_doc_Staff.Text, Convert.ToInt32(hdn_id.Value));
            DataTable dt = ds.Tables[0];
            rpt_doc_list_Staff.DataSource = dt;
            rpt_doc_list_Staff.DataBind();

            if (dt.Rows.Count > 0)
            {
                lbl_page_infoD_Staff.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["dt_indx"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
                hdn_last_pageD_Staff.Value = dt.Rows[0]["last_page"].ToString();
                lbl_page_numberD_Staff.Text = dt.Rows[0]["page_number"].ToString();
                hdn_totalD_Staff.Value = dt.Rows[0]["current_count"].ToString();
            }
            else
            {
                lbl_page_infoD_Staff.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
                hdn_last_pageD_Staff.Value = "0";
                lbl_page_numberD_Staff.Text = "1";
                hdn_totalD_Staff.Value = "0";
            }

            Upd_Nav_Doc_Staff.Update();
            Upd_doc_Staff.Update();
        }

        #region Navigation Doc _Staff

        //First Page
        protected void btn_first1_OnClick_Staff(object sender, EventArgs e)
        {
            DataTable dt_doc = (DataTable)Session["dt_doc_Staff"];
            fill_rpt_Staff( 1, Convert.ToInt32(drp_countD_Staff.SelectedValue));
        }

        //Previous Page
        protected void btn_prev1_OnClick_Staff(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_numberD_Staff.Text) > 1)
            {
                DataTable dt_doc = (DataTable)Session["dt_doc_Staff"];
                fill_rpt_Staff( Convert.ToInt32(lbl_page_numberD_Staff.Text) - 1, Convert.ToInt32(drp_countD_Staff.SelectedValue));
            }
        }

        //Next Page
        protected void btn_next1_OnClick_Staff(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_numberD_Staff.Text) < Convert.ToInt32(hdn_last_pageD_Staff.Value))
            {
                DataTable dt_doc = (DataTable)Session["dt_doc_Staff"];
                fill_rpt_Staff( Convert.ToInt32(lbl_page_numberD_Staff.Text) + 1, Convert.ToInt32(drp_countD_Staff.SelectedValue));
            }
        }

        //Last Page
        protected void btn_last1_OnClick_Staff(object sender, EventArgs e)
        {
            DataTable dt_doc = (DataTable)Session["dt_doc_Staff"];
            fill_rpt_Staff( Convert.ToInt32(hdn_last_pageD_Staff.Value), Convert.ToInt32(drp_countD_Staff.SelectedValue));
        }

        //Page Data Count
        protected void drp_countD_OnSelectedIndexChanged_Staff(object sender, EventArgs e)
        {
            DataTable dt_doc = (DataTable)Session["dt_doc_Staff"];
            fill_rpt_Staff( 1, Convert.ToInt32(drp_countD_Staff.SelectedValue));
        }

        #endregion

        #endregion

        #region History

        protected void btn_histry_OnClick(object sender, EventArgs e)
        {
            //date_from.SelectedDate = null;
            //date_to.SelectedDate = null;

            //grid_fill_his(1, 10);

            //div_main.Visible = false;
            //div_trans_main.Visible = true;
            //Upd_Main.Update();
        }

        //protected void btn_his_seacrh_OnClick(object sender, EventArgs e)
        //{
        //    grid_fill_his(1, 10);

        //    Upd_History.Update();
        //}

        //protected void btnexcel_exportHis_OnClick(object sender, EventArgs e)
        //{
        //    DataSet ds = obj_master.list_CustStatHistry(date_from.SelectedDate, date_to.SelectedDate, Convert.ToInt32(hdn_id.Value),
        //        Convert.ToInt32(lbl_page_number2.Text), Convert.ToInt32(drp_count1.SelectedValue));
        //    DataTable dt = ds.Tables[0];

        //    dt.Columns.Remove("current_count");
        //    dt.Columns.Remove("page_number");
        //    dt.Columns.Remove("Page_size");
        //    dt.Columns.Remove("start_num");
        //    dt.Columns.Remove("end_num");
        //    dt.Columns.Remove("last_page");

        //    if (dt.Rows.Count > 0)
        //    {
        //        StringWriter sw = obj_common.ExportToExcel(dt, "History");
        //        string style = @"<style> .textmode { mso-number-format:\@; } </style>";
        //        Response.Write(style);
        //        HttpContext.Current.Response.Write(style);
        //        Response.Output.Write(sw.ToString());
        //        HttpContext.Current.Response.Flush();
        //        HttpContext.Current.Response.End();
        //    }
        //}

        //protected void btn_histry_Close_OnClick(object sender, EventArgs e)
        //{
        //    div_main.Visible = true;
        //    div_trans_main.Visible = false;
        //    Upd_Main.Update();
        //}

        //public void grid_fill_his(int page_number, int page_size)
        //{
        //    DataSet ds = obj_master.list_CustStatHistry(date_from.SelectedDate, date_to.SelectedDate, Convert.ToInt32(hdn_id.Value),
        //        page_number, page_size);
        //    DataTable dt = ds.Tables[0];

        //    rpt_His.DataSource = dt;
        //    rpt_His.DataBind();

        //    if (dt.Rows.Count > 0)
        //    {
        //        lbl_page_info2.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["SLNo"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
        //        hdn_last_page2.Value = dt.Rows[0]["last_page"].ToString();
        //        lbl_page_number2.Text = dt.Rows[0]["page_number"].ToString();
        //        hdn_total2.Value = dt.Rows[0]["current_count"].ToString();
        //    }
        //    else
        //    {
        //        lbl_page_info2.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
        //        hdn_last_page2.Value = "0";
        //        lbl_page_number2.Text = "1";
        //        hdn_total2.Value = "0";
        //    }
        //    upd_his_nav.Update();
        //    Upd_History.Update();
        //}

        //#region his Navigation

        ////First Page
        //protected void btn_first2_OnClick(object sender, EventArgs e)
        //{
        //    grid_fill_his(1, Convert.ToInt32(drp_count1.SelectedValue));
        //}

        ////Previous Page
        //protected void btn_prev2_OnClick(object sender, EventArgs e)
        //{
        //    if (Convert.ToInt32(lbl_page_number2.Text) > 1)
        //    {
        //        grid_fill_his(Convert.ToInt32(lbl_page_number2.Text) - 1, Convert.ToInt32(drp_count1.SelectedValue));
        //    }
        //}

        ////Next Page
        //protected void btn_next2_OnClick(object sender, EventArgs e)
        //{
        //    if (Convert.ToInt32(lbl_page_number2.Text) < Convert.ToInt32(hdn_last_page2.Value))
        //    {
        //        grid_fill_his(Convert.ToInt32(lbl_page_number2.Text) + 1, Convert.ToInt32(drp_count1.SelectedValue));
        //    }
        //}

        ////Last Page
        //protected void btn_last2_OnClick(object sender, EventArgs e)
        //{
        //    grid_fill_his(Convert.ToInt32(hdn_last_page2.Value), Convert.ToInt32(drp_count1.SelectedValue));
        //}

        ////Page Data Count
        //protected void drp_count2_OnSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    grid_fill_his(1, Convert.ToInt32(drp_count1.SelectedValue));
        //}

        //#endregion

        #endregion

        #region Service Discount

        protected void btn_serviceDiscount_OnClick(object sender, EventArgs e)
        {
            pnl_Service_Detail.Visible = true;
            txtCommonDiscount.Text =txtsearchservice.Text= "";

            txtsearchservice_TextChanged(null, null);

            Upd_Service_Detail_Panel.Update();
        }

        protected void txtsearchservice_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = obj_master.Get_List_Customer_ServiceDetail(Convert.ToInt32(hdn_id.Value), txtsearchservice.Text);
            rpt_serdetail.DataSource = dt;
            rpt_serdetail.DataBind();

            updservicelist.Update();
        }

        protected void btn_SDSave_OnClick(object sender, EventArgs e)
        {
            int res = 0;
            DataTable dt_serDetail = fill_ServiceDetail();
            if (dt_serDetail.Rows.Count > 0)
                res = obj_master.Insert_Update_CustomerServiceDetail(Convert.ToInt32(hdn_id.Value),
                dt_serDetail,Convert.ToInt32(hdn_user_id.Value));
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
            dt_serDetail.Columns.Add("CusSerDetailId", typeof(int));
            dt_serDetail.Columns.Add("ServiceId", typeof(int));
            dt_serDetail.Columns.Add("DiscountAmount", typeof(decimal));
            dt_serDetail.Columns.Add("CommissionAmount", typeof(decimal));
            dt_serDetail.Columns.Add("AdditionAmount", typeof(decimal));//pooja added

            if (rpt_serdetail.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_serdetail.Items)
                {
                    HiddenField hdn_cusSerDetailId = (HiddenField)itm.FindControl("hdn_cusSerDetailId");
                    HiddenField hdn_serviceId = (HiddenField)itm.FindControl("hdn_serviceId");
                    TextBox txt_disAmt = (TextBox)itm.FindControl("txt_disAmt");
                    TextBox txtCommissionAmount = (TextBox)itm.FindControl("txtCommissionAmount");
                    TextBox txt_addAmt = (TextBox)itm.FindControl("txt_addAmt"); //pooja added


                    dt_serDetail.Rows.Add(Convert.ToInt32(hdn_cusSerDetailId.Value), 
                        Convert.ToInt32(hdn_serviceId.Value), Convert.ToDecimal(txt_disAmt.Text),
                        txtCommissionAmount.Text==""?0:Convert.ToDecimal(txtCommissionAmount.Text), txt_addAmt.Text == "" ? 0 : Convert.ToDecimal(txt_addAmt.Text));
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

        #region Service Expiry

        protected void btnServiceExpires_OnClick(object sender, EventArgs e)
        {
            pnlServiceExpiry.Visible = true;
            fill_ServiceExpires(1, 10,"");
            UpdServiceExpiryPanel.Update();
        }

        public void fill_ServiceExpires(int page_number, int page_size, string filter)
        {
            DataTable dt = obj_master.GetCustomerServiceExpires(Convert.ToInt32(hdn_id.Value),page_number, page_size, filter);
            rptServiceExpiry.DataSource = dt;
            rptServiceExpiry.DataBind();
            if (dt.Rows.Count > 0)
            {
                lbl_page_info_SE.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["RowNum"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
                hdn_filter_SE.Value = dt.Rows[0]["filter"].ToString();
                hdn_last_page_SE.Value = dt.Rows[0]["last_page"].ToString();
                lbl_page_number_SE.Text = dt.Rows[0]["page_number"].ToString();
                hdn_total_SE.Value = dt.Rows[0]["current_count"].ToString();
            }
            else
            {
                lbl_page_info_SE.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
                hdn_filter_SE.Value = txt_search.Text;
                hdn_last_page_SE.Value = "0";
                lbl_page_number_SE.Text = "1";
                hdn_total_SE.Value = "0";
            }
            Upd_Nav_Panel_SE.Update();
            Upd_List_Panel_SE.Update();
        }


        protected void btnCloseSE_OnClick(object sender, EventArgs e)
        {
            pnlServiceExpiry.Visible = false;
            UpdServiceExpiryPanel.Update();
        }

        #region Navigation Service Expiry

        //First Page
        protected void btn_first_SE_OnClick(object sender, EventArgs e)
        {
            fill_ServiceExpires(1, Convert.ToInt32(drp_count_SE.SelectedValue), hdn_filter_SE.Value);
            Upd_List_Panel_SE.Update();
        }

        //Previous Page
        protected void btn_prev_SE_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number_SE.Text) > 1)
            {
                fill_ServiceExpires(Convert.ToInt32(lbl_page_number_SE.Text) - 1, Convert.ToInt32(drp_count_SE.SelectedValue), hdn_filter_SE.Value);
                Upd_List_Panel_SE.Update();
            }
        }

        //Next Page
        protected void btn_next_SE_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number_SE.Text) < Convert.ToInt32(hdn_last_page_SE.Value))
            {
                fill_ServiceExpires(Convert.ToInt32(lbl_page_number_SE.Text) + 1, Convert.ToInt32(drp_count_SE.SelectedValue), hdn_filter_SE.Value);
                Upd_List_Panel_SE.Update();
            }
        }

        //Last Page
        protected void btn_last_SE_OnClick(object sender, EventArgs e)
        {

            fill_ServiceExpires(Convert.ToInt32(hdn_last_page_SE.Value), Convert.ToInt32(drp_count_SE.SelectedValue), hdn_filter_SE.Value);
            Upd_List_Panel_SE.Update();
        }

        //Page Data Count
        protected void drp_count_SE_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            fill_ServiceExpires(1, Convert.ToInt32(drp_count_SE.SelectedValue), hdn_filter_SE.Value);
            Upd_List_Panel_SE.Update();
        }

        #endregion

        #endregion

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

        protected void btnmenu_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ShowMenuForm(" + hdn_id.Value + ");", true);
        }

        //Check Privilege
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.Form_Previlage_Validation(8, Convert.ToInt32(hdn_user_id.Value));
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

                    DataTable dt = obj_common.Action_Previlage_Validation(8, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdn_update.Value = dt.Rows[1][1].ToString();
                        hdn_delete.Value = dt.Rows[2][1].ToString();
                        hdn_OB.Value = dt.Rows[3][1].ToString();
                        hdn_doc.Value = dt.Rows[4][1].ToString();
                        hdn_cred.Value = dt.Rows[5][1].ToString();
                        hdn_servicediscount.Value = dt.Rows[6][1].ToString();
                        hdnmenuprivilege.Value =  dt.Rows[7][1].ToString();
                        hdn_doc_Staff.Value = dt.Rows[8][1].ToString();
                        hdn_histry.Value = dt.Rows[9][1].ToString();
                        hdnmail.Value = dt.Rows[10][1].ToString();

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