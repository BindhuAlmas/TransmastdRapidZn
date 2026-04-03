using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AmarCentre.BAL;
using Telerik.Web.UI;

namespace AmarCentre.CRM
{
    public partial class LeadFollowup : System.Web.UI.Page
    {
        System_Utilities systemUtilities = new System_Utilities();
        Master_Bal masterBAL = new Master_Bal();
        Transaction_Bal TransBal = new Transaction_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
            }
            if (!IsPostBack)
            {
                hdnUserId.Value = Session["User_Id"].ToString();
                CheckPrivilege();
                CheckActionPrivilege();
                Filldropdown();
                Clear();
                fillgridList(1, 10, "", "", "");

                if (Request.QueryString["LeadId"] != null)
                {
                    hdnPageId.Value = Request.QueryString["PageId"].ToString();
                    fillLeadDetails(Convert.ToInt32(Request.QueryString["LeadId"].ToString()));
                }
            }
        }

        public void Filldropdown()
        {
            DataSet ds = TransBal.drpforLead();

            DataTable dtp= ds.Tables[3];

            drpprorityfilter.DataSource = dtp;
            drpprorityfilter.DataValueField = "Value";
            drpprorityfilter.DataTextField = "Text";
            drpprorityfilter.DataBind();

            RadComboBoxItem CodeItem = new RadComboBoxItem();
            CodeItem.Text = "All";
            CodeItem.Value = "0";
            drpprorityfilter.Items.Insert(0, CodeItem);

            drpPriority.DataSource = dtp;
            drpPriority.DataValueField = "Value";
            drpPriority.DataTextField = "Text";
            drpPriority.DataBind();

            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "New Entry";
            CodeItem.Value = "0";
            drpPriority.Items.Insert(0, CodeItem);

            drpStatusfilter.DataSource = ds.Tables[2];
            drpStatusfilter.DataValueField = "Value";
            drpStatusfilter.DataTextField = "Text";
            drpStatusfilter.DataBind();

            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "All";
            CodeItem.Value = "0";
            drpStatusfilter.Items.Insert(0, CodeItem);

            drpStatus.DataSource = ds.Tables[2];
            drpStatus.DataValueField = "Value";
            drpStatus.DataTextField = "Text";
            drpStatus.DataBind();

            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "New Entry";
            CodeItem.Value = "0";
            drpStatus.Items.Insert(0, CodeItem);

            drpSegmentfilter.DataSource = ds.Tables[6];
            drpSegmentfilter.DataValueField = "Value";
            drpSegmentfilter.DataTextField = "Text";
            drpSegmentfilter.DataBind();

            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "All";
            CodeItem.Value = "0";
            drpSegmentfilter.Items.Insert(0, CodeItem);
            drpSegment.DataSource = ds.Tables[6];
            drpSegment.DataValueField = "Value";
            drpSegment.DataTextField = "Text";
            drpSegment.DataBind();

            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "New Entry";
            CodeItem.Value = "0";
            drpSegment.Items.Insert(0, CodeItem);
        }

        public void fillgridList(int PageNumber, int PageSize, string Filter, string OrderByColumnName, string OrderBy)
        {
            DataTable dtEmployeeList = TransBal.GetLeadFolowupList(PageNumber, PageSize, Filter,
                Convert.ToInt32(hdnUserId.Value), drpStatusfilter.SelectedValue == "" ? 0 : Convert.ToInt32(drpStatusfilter.SelectedValue),
                drpprorityfilter.SelectedValue == "" ? 0 : Convert.ToInt32(drpprorityfilter.SelectedValue),txt_reg_Frm_date.SelectedDate,
                txt_reg_to_date.SelectedDate, drpSegmentfilter.SelectedValue == "" ? 0 : Convert.ToInt32(drpSegmentfilter.SelectedValue));
            rptList.DataSource = dtEmployeeList;
            rptList.DataBind();
            if (dtEmployeeList.Rows.Count > 0)
            {
                lblPageInfo.Text = "Showing Results " + dtEmployeeList.Rows[0]["StartNumber"].ToString() + " - " + dtEmployeeList.Rows[dtEmployeeList.Rows.Count - 1]["RowNum"].ToString() + " Out of " + dtEmployeeList.Rows[0]["CurrentCount"].ToString() + " Records";
                hdnFilter.Value = dtEmployeeList.Rows[0]["Filter"].ToString();
                hdnOrderByColumnName.Value = dtEmployeeList.Rows[0]["OrderByColumnName"].ToString();
                hdnOrderBy.Value = dtEmployeeList.Rows[0]["OrderBy"].ToString();
                hdnLastPage.Value = dtEmployeeList.Rows[0]["LastPage"].ToString();
                lblPageNumber.Text = dtEmployeeList.Rows[0]["PageNumber"].ToString();
                hdnTotal.Value = dtEmployeeList.Rows[0]["CurrentCount"].ToString();

            }
            else
            {
                lblPageInfo.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
                hdnFilter.Value = txtSearch.Text;
                hdnLastPage.Value = "0";
                lblPageNumber.Text = "1";
                hdnTotal.Value = "0";
            }
            UpdPanelNavigation.Update();
            UpdPanelList.Update();
        }

        protected void drpStatusfilterOnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            fillgridList(1, Convert.ToInt32(drpPageSize.SelectedValue), hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
        }

        public void btnExportToExcelOnClick(object sender, EventArgs e)
        {
            DataTable dtEmployeeList = TransBal.GetLeadFolowupListExcel(Convert.ToInt32(hdnUserId.Value));
            if (dtEmployeeList.Rows.Count > 0)
            {
                StringWriter sw = systemUtilities.ExportToExcel(dtEmployeeList, "LeadList");
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                HttpContext.Current.Response.Write(style);
                Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        public void fillLeadDetails(int LeadId)
        {
            DataSet dsLeadDetails = TransBal.EditLeadCreation(LeadId);
            DataTable dtBasic = dsLeadDetails.Tables[0];
            DataTable dtService = dsLeadDetails.Tables[1];

            txtMobileNumber.Text = dtBasic.Rows[0]["MobileNumber"].ToString();
            txtName.Text = dtBasic.Rows[0]["ContactPersonName"].ToString();
            hdnId.Value = dtBasic.Rows[0]["Id"].ToString();
            drpPriority.SelectedValue = dtBasic.Rows[0]["Priority"].ToString();
            txtcompany.Text = dtBasic.Rows[0]["CompanyName"].ToString();
            //txtActivity.Text = dtBasic.Rows[0]["Activity"].ToString();
            txtCPDesig.Text = dtBasic.Rows[0]["ContactPersonDesig"].ToString();
            txtwebsite.Text = dtBasic.Rows[0]["Website"].ToString();
            txtActivityDesc.Text = dtBasic.Rows[0]["Address"].ToString();
            drpSegment.SelectedValue = dtBasic.Rows[0]["SegmentId"].ToString();
            if (dtService.Rows.Count == 0)
                dtService.Rows.Add(0, null);
            rptservice.DataSource = dtService;
            rptservice.DataBind();


            if ( dtBasic.Rows[0]["isclosed"].ToString() == "1") // closed
                btnSave.Visible = btnCreateQutn.Visible = false;
            else
            {
                btnSave.Visible = hdnAdd.Value == "0" ? false : true;
                btnCreateQutn.Visible = hdnCreateQutn.Value == "0" ? false : true;
            }

            btnHistory.Visible = hdnHistory.Value == "0" ? false : true;

            PanelAdd.Visible = true;
            UpdPanelAdd.Update();
        }

        protected void rptListOnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            Clear();
            HiddenField hdnRptId = (HiddenField)e.Item.FindControl("hdnId");
            fillLeadDetails(Convert.ToInt32(hdnRptId.Value));
        }

        protected void btnSaveOnClick(object sender, EventArgs e)
        {

            DataTable dtService = new DataTable();
            dtService.Columns.Add("Id", typeof(int));
            dtService.Columns.Add("DepartmentId", typeof(int));
            dtService.Columns.Add("CategoryId", typeof(int));
            dtService.Columns.Add("SubCategoryId", typeof(int));
            dtService.Columns.Add("ServiceId", typeof(int));
            dtService.Columns.Add("Price", typeof(decimal));

            foreach (RepeaterItem itm in rptservice.Items)
            {
                HiddenField hdnDId = (HiddenField)itm.FindControl("hdnDId");
                RadComboBox drpDepartment = (RadComboBox)itm.FindControl("drpDepartment");
                RadComboBox drpSerCategory = (RadComboBox)itm.FindControl("drpSerCategory");

                if (drpDepartment.SelectedValue != "" && drpSerCategory.SelectedValue != "")
                    dtService.Rows.Add(Convert.ToInt32(hdnDId.Value),
                        drpDepartment.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpDepartment.SelectedValue),
                        drpSerCategory.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpSerCategory.SelectedValue),
                        (int?)null, (int?)null, 0);
            }

            //int res = TransBal.InsertLeadFolowup(Convert.ToInt32(hdnId.Value), Convert.ToInt32(hdnUserId.Value),
            //     txtResponse.Text, Convert.ToInt32(drpStatus.SelectedValue),
            //    Followupdate.SelectedDate, Currentdate.SelectedDate, txtremark.Text, radFollowupTime.SelectedDate, dtService,
            //    txtcompany.Text,Convert.ToInt32(drpPriority.SelectedValue), txtActivity.Text, txtCPDesig.Text, txtwebsite.Text);
            int res = TransBal.InsertLeadFolowup(Convert.ToInt32(hdnId.Value), Convert.ToInt32(hdnUserId.Value),
                 txtResponse.Text, Convert.ToInt32(drpStatus.SelectedValue),
                Followupdate.SelectedDate, Currentdate.SelectedDate, txtremark.Text, radFollowupTime.SelectedDate, dtService,
                txtcompany.Text, Convert.ToInt32(drpPriority.SelectedValue), Convert.ToInt32(drpSegment.SelectedValue), txtCPDesig.Text, txtwebsite.Text);
            if (res > 0)
            {
                fillgridList(Convert.ToInt32(lblPageNumber.Text), Convert.ToInt32(drpPageSize.SelectedValue), hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
                Clear();
                if (hdnPageId.Value == "1")
                    Response.Redirect("../Home.aspx");
                else if (hdnPageId.Value == "2")
                    Response.Redirect("../CRM/Tasks.aspx");
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }

            PanelAdd.Visible = false;
            UpdPanelAdd.Update();
        }
        protected void btnCreateQutn_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Transactions/Quotation.aspx?LeadId=" + hdnId.Value);
        }
        protected void drpStatusOnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpStatus.SelectedValue == "0")
            {
                int val = systemUtilities.Form_Previlage_Validation(79, Convert.ToInt32(hdnUserId.Value));
                if (val == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Sorry you do not have privilege to create new status..!');", true);
                    drpStatus.ClearSelection();
                    updStatus.Update();
                }
                else
                {
                    pnlStatus.Visible = true;
                    UCStatus.PageLoad();
                    updStatusPanel.Update();
                }
            }
            else
            {
                ReqFollowupdate.Enabled = pnlNextFollw.Visible = true;
                ReqFollowuptime.Enabled = pnlNextFollwTime.Visible = true;
                DataTable dtBasic = masterBAL.EditStatus(Convert.ToInt32(drpStatus.SelectedValue));

                if (dtBasic.Rows[0]["Isclosed"].ToString()=="1" || drpStatus.SelectedValue == "3")
                {
                    ReqFollowupdate.Enabled = pnlNextFollw.Visible = false;
                    ReqFollowuptime.Enabled = pnlNextFollwTime.Visible = false;
                }
                UpdFollowupdate.Update();
                UpdFollowupTime.Update();
            }
        }
        protected void drpSegment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (drpSegment.SelectedValue == "0")
            {
                pnlSegment.Visible = true;
                UCSegment.PageLoad();
                updSegmentPanel.Update();
            }
        }
        protected void drpPriority_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (drpPriority.SelectedValue == "0")
            {
                int val = systemUtilities.Form_Previlage_Validation(77, Convert.ToInt32(hdnUserId.Value));
                if (val == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Sorry you do not have privilege to create new Priority..!');", true);
                    drpPriority.ClearSelection();
                    updPriority.Update();
                }
                else
                {
                    pnlPriority.Visible = true;
                    UCPriority.PageLoad();
                    updPriorityPanel.Update();
                }
            }
        }

        protected void rptserviceOnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            DataTable dtService = new DataTable();
            dtService.Columns.Add("Id", typeof(int));
            dtService.Columns.Add("DepartmentId", typeof(int));
            dtService.Columns.Add("CategoryId", typeof(int));

            foreach (RepeaterItem itm in rptservice.Items)
            {
                HiddenField hdnDId = (HiddenField)itm.FindControl("hdnDId");
                RadComboBox drpDepartment = (RadComboBox)itm.FindControl("drpDepartment");
                RadComboBox drpSerCategory = (RadComboBox)itm.FindControl("drpSerCategory");

                if (drpDepartment.SelectedValue != "" && drpSerCategory.SelectedValue != "")
                    dtService.Rows.Add(Convert.ToInt32(hdnDId.Value),
                        drpDepartment.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpDepartment.SelectedValue),
                        drpSerCategory.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpSerCategory.SelectedValue));
            }
            if (e.CommandName == "Delete")
            {
                int indx = e.Item.ItemIndex;
                if (indx < dtService.Rows.Count)
                    dtService.Rows.RemoveAt(indx);
            }
            dtService.Rows.Add(0, null);

            rptservice.DataSource = dtService;
            rptservice.DataBind();
            UpdService.Update();
        }

        protected void rptserviceOnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RadComboBox drpDepartment = (RadComboBox)e.Item.FindControl("drpDepartment");
            RadComboBox drpSerCategory = (RadComboBox)e.Item.FindControl("drpSerCategory");
            HiddenField hdnDepartmentId = (HiddenField)e.Item.FindControl("hdnDepartmentId");
            HiddenField hdnSerCategoryId = (HiddenField)e.Item.FindControl("hdnSerCategoryId");

            drpDepartment.Text = "";
            drpDepartment.DataSource = masterBAL.DrpLeadDepartment();
            drpDepartment.DataTextField = "Text";
            drpDepartment.DataValueField = "Value";
            drpDepartment.DataBind();
            drpDepartment.SelectedValue = hdnDepartmentId.Value;

            DataSet ds = masterBAL.DrpQuestion123(hdnDepartmentId.Value == "" ? 0 : Convert.ToInt32(hdnDepartmentId.Value));

            drpSerCategory.Text = "";
            drpSerCategory.DataSource = ds.Tables[0];
            drpSerCategory.DataTextField = "Text";
            drpSerCategory.DataValueField = "Value";
            drpSerCategory.DataBind();
            drpSerCategory.SelectedValue = hdnSerCategoryId.Value;
        }

        protected void btnanwser_Click(object sender, EventArgs e)
        {
            pnlAnswer.Visible = true;
            UCAnswer.UCPageLoad();
            updAnswer.Update();
        }

        protected void btnQn_Click(object sender, EventArgs e)
        {
            pnlQuestion.Visible = true;
            UCQuestion.UCPageLoad();
            updQuestion.Update();
        }

        protected void drpFilterOnSelectedIndexChanged(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            String contrlName = sendercontrol.ID;

            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;
            //UpdatePanel UpdDepartmentDropdown = (UpdatePanel)itemrp.FindControl("UpdDepartmentDropdown");
            UpdatePanel UpdSerCategoryDropdown = (UpdatePanel)itemrp.FindControl("UpdSerCategoryDropdown");
            //UpdatePanel UpdSerSubCategoryDropdown = (UpdatePanel)itemrp.FindControl("UpdSerSubCategoryDropdown");
            //UpdatePanel UpdServiceDropdown = (UpdatePanel)itemrp.FindControl("UpdServiceDropdown");
            RadComboBox drpDepartment = (RadComboBox)itemrp.FindControl("drpDepartment");
            RadComboBox drpSerCategory = (RadComboBox)itemrp.FindControl("drpSerCategory");
            //RadComboBox drpSerSubCategory = (RadComboBox)itemrp.FindControl("drpSerSubCategory");
            //RadComboBox drpService = (RadComboBox)itemrp.FindControl("drpService");
            HiddenField hdnDepartmentId = (HiddenField)itemrp.FindControl("hdnDepartmentId");
            HiddenField hdnSerCategoryId = (HiddenField)itemrp.FindControl("hdnSerCategoryId");

            drpSerCategory.Text = "";
            drpSerCategory.ClearSelection();
            hdnDepartmentId.Value = drpDepartment.SelectedValue;
            hdnSerCategoryId.Value = "";// hdnSerSubCategoryId.Value = "";

            int Department = drpDepartment.SelectedValue == "" ? 0 : Convert.ToInt32(drpDepartment.SelectedValue);
            DataSet ds = masterBAL.DrpQuestion123(Department);

            drpSerCategory.ClearSelection();
            drpSerCategory.Text = "";
            drpSerCategory.Items.Clear();
            drpSerCategory.DataSource = ds.Tables[0];
            drpSerCategory.DataTextField = "Text";
            drpSerCategory.DataValueField = "Value";
            drpSerCategory.DataBind();

            UpdSerCategoryDropdown.Update();
        }

        protected void btnCloseOnClick(object sender, EventArgs e)
        {
            if (hdnPageId.Value == "1")
                Response.Redirect("../Home.aspx");
            else if (hdnPageId.Value == "2")
                Response.Redirect("../CRM/Tasks.aspx");
            PanelAdd.Visible = false;
            UpdPanelAdd.Update();
        }

        public void Clear()
        {
            hdnId.Value = "0";
            txtName.Text = "";
            txtremark.Text = txtResponse.Text = "";
            txtMobileNumber.Text =  "";
            drpStatus.ClearSelection();
            drpStatus.Text = txtActivityDesc.Text= "";
            Followupdate.DbSelectedDate = "";
            radFollowupTime.SelectedDate = null;
            Currentdate.SelectedDate = DateTime.Now;
            ReqFollowupdate.Enabled = pnlNextFollw.Visible = true;
            ReqFollowuptime.Enabled = pnlNextFollwTime.Visible = true;
            Followupdate.MinDate = DateTime.Now;

            btnHistory.Visible = false;
            btnSave.Visible = false;

            UpdPanelAddInner.Update();
        }

        protected void btn_search_OnClick(object sender, EventArgs e)
        {
            fillgridList(1, Convert.ToInt32(drpPageSize.SelectedValue), hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
            pnl_filter.Visible = false;
            upd_nav_filter.Update();
        }

        protected void btn_filter_OnClick(object sender, EventArgs e)
        {
            if (pnl_filter.Visible == true)
            {
                pnl_filter.Visible = false;
            }
            else
            {
                pnl_filter.Visible = true;
            }
            upd_nav_filter.Update();
        }

        #region Navigation

        //Search
        protected void txtSearchOnTextChanged(object sender, EventArgs e)
        {
            fillgridList(1, Convert.ToInt32(drpPageSize.SelectedValue), txtSearch.Text, hdnOrderByColumnName.Value, hdnOrderBy.Value);
        }

        //First Page
        protected void btnFirstOnClick(object sender, EventArgs e)
        {
            fillgridList(1, Convert.ToInt32(drpPageSize.SelectedValue), hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
        }

        //Previous Page
        protected void btnPreviousOnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lblPageNumber.Text) > 1)
            {
                fillgridList(Convert.ToInt32(lblPageNumber.Text) - 1, Convert.ToInt32(drpPageSize.SelectedValue), hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
            }
        }

        //Next Page
        protected void btnNextOnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lblPageNumber.Text) < Convert.ToInt32(hdnLastPage.Value))
            {
                fillgridList(Convert.ToInt32(lblPageNumber.Text) + 1, Convert.ToInt32(drpPageSize.SelectedValue), hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
            }
        }

        //Last Page
        protected void btnLastOnClick(object sender, EventArgs e)
        {
            fillgridList(Convert.ToInt32(hdnLastPage.Value), Convert.ToInt32(drpPageSize.SelectedValue), hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
        }

        //Page Data Count
        protected void drpPageSizeOnSelectedIndexChanged(object sender, EventArgs e)
        {
            fillgridList(1, Convert.ToInt32(drpPageSize.SelectedValue), hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
        }

        #endregion

        #region History

        protected void btn_excelhis_OnClick(object sender, EventArgs e)
        {
            DataTable dt = TransBal.ListLeadHistoryPrintExcel(Convert.ToInt32(hdnId.Value));

            if (dt.Rows.Count > 0)
            {
                StringWriter sw = systemUtilities.ExportToExcel(dt, "LeadHistory");
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                HttpContext.Current.Response.Write(style);
                Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        protected void btn_pdfhis_OnClick(object sender, EventArgs e)
        {
            string url = "../CRM/LeadHistory.aspx?LeadId=" + Convert.ToInt32(hdnId.Value);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        protected void btnhistrymainOnClick(object sender, EventArgs e)
        {
            grid_fill_his(1, 10);
            pnlHistory.Visible = true;
            updHistoryMain.Update();
        }

        protected void btn_histry_Close_OnClick(object sender, EventArgs e)
        {
            pnlHistory.Visible = false;
            updHistoryMain.Update();
        }

        public void grid_fill_his(int page_number, int page_size)
        {
            DataTable dt = TransBal.ListLeadHistory(Convert.ToInt32(hdnId.Value),page_number, page_size);
            rptHistory.DataSource = dt;
            rptHistory.DataBind();

            if (dt.Rows.Count > 0)
            {
                lbl_page_info1.Text = "Showing Results " + dt.Rows[0]["StartNumber"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["RowNum"].ToString() + " Out of " + dt.Rows[0]["CurrentCount"].ToString() + " Records";
                hdn_last_page1.Value = dt.Rows[0]["LastPage"].ToString();
                lbl_page_number1.Text = dt.Rows[0]["PageNumber"].ToString();
                hdn_total1.Value = dt.Rows[0]["CurrentCount"].ToString();
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
        protected void btn_first1_OnClick(object sender, EventArgs e)
        {
            grid_fill_his(1, Convert.ToInt32(drp_count1.SelectedValue));
        }

        //Previous Page
        protected void btn_prev1_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number1.Text) > 1)
            {
                grid_fill_his(Convert.ToInt32(lbl_page_number1.Text) - 1, Convert.ToInt32(drp_count1.SelectedValue));
            }
        }

        //Next Page
        protected void btn_next1_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number1.Text) < Convert.ToInt32(hdn_last_page1.Value))
            {
                grid_fill_his(Convert.ToInt32(lbl_page_number1.Text) + 1, Convert.ToInt32(drp_count1.SelectedValue));
            }
        }

        //Last Page
        protected void btn_last1_OnClick(object sender, EventArgs e)
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

        public void CheckPrivilege()
        {
            try
            {
                if (hdnUserId.Value != null)
                {

                    int val = systemUtilities.Form_Previlage_Validation(139, Convert.ToInt32(hdnUserId.Value));
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

        public void CheckActionPrivilege()
        {
            try
            {
                if (hdnUserId.Value != null)
                {

                    DataTable dtSubMenuAction = systemUtilities.Action_Previlage_Validation(139, Convert.ToInt32(hdnUserId.Value));
                    if (dtSubMenuAction.Rows.Count > 0)
                    {
                        hdnAdd.Value = dtSubMenuAction.Rows[0][1].ToString();
                        hdnHistory.Value = dtSubMenuAction.Rows[1][1].ToString();
                        hdnCreateQutn.Value = dtSubMenuAction.Rows[2][1].ToString();
                    }
                    btnSave.Visible = hdnAdd.Value == "0" ? false : true;
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