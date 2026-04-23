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
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Net.Mime;
using System.Web.Configuration;
using System.Data.OleDb;

namespace AmarCentre.CRM
{
    public partial class LeadCreation : System.Web.UI.Page
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
                Clear();
                fillgridList(1, 10, "", "", "");
                FillDropdown();
            }
        }

        public void FillDropdown()
        {
            DataSet ds = TransBal.drpforLead();

            drpEmployee.DataSource = ds.Tables[0];
            drpEmployee.DataValueField = "Value";
            drpEmployee.DataTextField = "Text";
            drpEmployee.DataBind();

            drpSource.DataSource = ds.Tables[1];
            drpSource.DataValueField = "Value";
            drpSource.DataTextField = "Text";
            drpSource.DataBind();

            RadComboBoxItem CodeItem = new RadComboBoxItem();
            CodeItem.Text = "New Entry";
            CodeItem.Value = "0";
            drpSource.Items.Insert(0, CodeItem);

            drpStatusfilter.DataSource = ds.Tables[2];
            drpStatusfilter.DataValueField = "Value";
            drpStatusfilter.DataTextField = "Text";
            drpStatusfilter.DataBind();

            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "All";
            CodeItem.Value = "0";
            drpStatusfilter.Items.Insert(0, CodeItem);

            DataTable dtp = ds.Tables[3];
            drpprorityfilter.DataSource = dtp;
            drpprorityfilter.DataValueField = "Value";
            drpprorityfilter.DataTextField = "Text";
            drpprorityfilter.DataBind();

            CodeItem = new RadComboBoxItem();
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

            drpJurisdiction.DataSource = ds.Tables[4];
            drpJurisdiction.DataValueField = "Value";
            drpJurisdiction.DataTextField = "Text";
            drpJurisdiction.DataBind();

            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "New Entry";
            CodeItem.Value = "0";
            drpJurisdiction.Items.Insert(0, CodeItem);

            drpCity.DataSource = ds.Tables[5];
            drpCity.DataValueField = "Value";
            drpCity.DataTextField = "Text";
            drpCity.DataBind();

            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "New Entry";
            CodeItem.Value = "0";
            drpCity.Items.Insert(0, CodeItem);

            drpSegment.DataSource = ds.Tables[6];
            drpSegment.DataValueField = "Value";
            drpSegment.DataTextField = "Text";
            drpSegment.DataBind();

            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "New Entry";
            CodeItem.Value = "0";
            drpSegment.Items.Insert(0, CodeItem);

            drpDocumentType.DataSource = ds.Tables[7];
            drpDocumentType.DataValueField = "Value";
            drpDocumentType.DataTextField = "Text";
            drpDocumentType.DataBind();
           
        }

        public void Get_Code()
        {
            DataTable dt = systemUtilities.Get_Code(138);
            if (dt.Rows.Count > 0)
                lbl_Code.Text = dt.Rows[0][0].ToString();
        }

        public void fillgridList(int PageNumber, int PageSize, string Filter,
            string OrderByColumnName, string OrderBy)
        {
            DataTable dtEmployeeList = TransBal.GetLeadList(PageNumber, PageSize, Filter,
                drpStatusfilter.SelectedValue == "" ? 0 : Convert.ToInt32(drpStatusfilter.SelectedValue),
                drpprorityfilter.SelectedValue == "" ? 0 : Convert.ToInt32(drpprorityfilter.SelectedValue),
                Convert.ToInt32(hdnUserId.Value));
            rptList.DataSource = dtEmployeeList;
            rptList.DataBind();
            if (dtEmployeeList.Rows.Count > 0)
            {
                lblPageInfo.Text = "Showing Results "
                    + dtEmployeeList.Rows[0]["StartNumber"].ToString() + " - "
                    + dtEmployeeList.Rows[dtEmployeeList.Rows.Count - 1]["RowNum"].ToString()
                    + " Out of " + dtEmployeeList.Rows[0]["CurrentCount"].ToString() + " Records";
                hdnFilter.Value = dtEmployeeList.Rows[0]["Filter"].ToString();
                hdnOrderByColumnName.Value = dtEmployeeList.Rows[0]["OrderByColumnName"].ToString();
                hdnOrderBy.Value = dtEmployeeList.Rows[0]["OrderBy"].ToString();
                hdnLastPage.Value = dtEmployeeList.Rows[0]["LastPage"].ToString();
                lblPageNumber.Text = dtEmployeeList.Rows[0]["PageNumber"].ToString();
                hdnTotal.Value = dtEmployeeList.Rows[0]["CurrentCount"].ToString();
            }
            else
            {
                lblPageInfo.Text = "Showing Results 0 - 0 Out of 0 Records";
                hdnFilter.Value = txtSearch.Text;
                hdnLastPage.Value = "0";
                lblPageNumber.Text = "1";
                hdnTotal.Value = "0";
            }
            UpdPanelNavigation.Update();
            UpdPanelList.Update();
        }

        public void btnExportToExcelOnClick(object sender, EventArgs e)
        {
            DataTable dtEmployeeList = TransBal.GetLeadCreationListExcel(
                Convert.ToInt32(hdnUserId.Value));
            if (dtEmployeeList.Rows.Count > 0)
            {
                StringWriter sw = systemUtilities.ExportToExcel(dtEmployeeList, "LeadCreationList");
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                HttpContext.Current.Response.Write(style);
                Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        protected void rptListOnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            HiddenField hdnRptId = (HiddenField)e.Item.FindControl("hdnId");

            if (e.CommandName == "Edit")
            {
                Clear();
                DataSet dsLeadDetails = TransBal.EditLeadCreation(Convert.ToInt32(hdnRptId.Value));
                DataTable dtBasic = dsLeadDetails.Tables[0];
                DataTable dtService = dsLeadDetails.Tables[1];
                DataTable dtDocument = dsLeadDetails.Tables[2];

                txtAddress.Text = dtBasic.Rows[0]["Address"].ToString();
                txtMobileNumber.Text = dtBasic.Rows[0]["MobileNumber"].ToString();
                txtEmailId.Text = dtBasic.Rows[0]["EmailId"].ToString();
                txtName.Text = dtBasic.Rows[0]["ContactPersonName"].ToString();
                hdnId.Value = dtBasic.Rows[0]["Id"].ToString();
                drpSource.SelectedValue = dtBasic.Rows[0]["LeadSourceId"].ToString();
                txtphone.Text = dtBasic.Rows[0]["LandPhoneNo"].ToString();
                drpPriority.SelectedValue = dtBasic.Rows[0]["Priority"].ToString();
                txtcompany.Text = dtBasic.Rows[0]["CompanyName"].ToString();
                txtResponse.Text = dtBasic.Rows[0]["CustomerResponse"].ToString();
                Followupdate.DbSelectedDate = dtBasic.Rows[0]["NextFollowupDate"].ToString();
                radFollowupTime.DbSelectedDate = dtBasic.Rows[0]["NextFollowupTime"].ToString();
                leadDate.DbSelectedDate = dtBasic.Rows[0]["LeadDate"].ToString();
                hdnStatus.Value = dtBasic.Rows[0]["Status"].ToString();
                txtActivity.Text = dtBasic.Rows[0]["Activity"].ToString();
                txtCPDesig.Text = dtBasic.Rows[0]["ContactPersonDesig"].ToString();
                txtwebsite.Text = dtBasic.Rows[0]["Website"].ToString();
                lbl_Code.Text = dtBasic.Rows[0]["Code"].ToString();
                txtCampaign.Text = dtBasic.Rows[0]["Campaign"].ToString();
                txtCountryCodeCN.Text = dtBasic.Rows[0]["CountryCodeCN"].ToString();
                txtCountryCodeLPN.Text = dtBasic.Rows[0]["CountryCodeLPN"].ToString();
                drpSegment.SelectedValue = dtBasic.Rows[0]["SegmentId"].ToString();
                drpCity.SelectedValue = dtBasic.Rows[0]["CityId"].ToString();

                // NEW FIELDS
                txtLeadBrand.Text = dtBasic.Rows[0]["LeadBrand"].ToString();
                txtPassportNo.Text = dtBasic.Rows[0]["PassportNo"].ToString();
                dpPassportIssueDate.DbSelectedDate = dtBasic.Rows[0]["PassportIssueDate"].ToString();
                dpPassportExpiryDate.DbSelectedDate = dtBasic.Rows[0]["PassportExpiryDate"].ToString();
                dpDOB.DbSelectedDate = dtBasic.Rows[0]["DOB"].ToString();
                drpCurrentStatus.SelectedValue = dtBasic.Rows[0]["CurrentStatus"].ToString();
                txtNationality.Text = dtBasic.Rows[0]["Nationality"].ToString();
                drpMaritalStatus.SelectedValue = dtBasic.Rows[0]["MartialStatus"].ToString();
                txtMotherName.Text = dtBasic.Rows[0]["MotherName"].ToString();

                drpEmployee.DataSource = TransBal.DrpEmployeeTrans(1);
                drpEmployee.DataValueField = "Value";
                drpEmployee.DataTextField = "Text";
                drpEmployee.DataBind();
                drpEmployee.SelectedValue = dtBasic.Rows[0]["AssignedEmployeeId"].ToString();

                if (dtService.Rows.Count == 0)
                    dtService.Rows.Add(0, null);
                rptservice.DataSource = dtService;
                rptservice.DataBind();

                rptDocs.DataSource = dtDocument;
                rptDocs.DataBind();


                //BindDocsRepeater();

                if (dtBasic.Rows[0]["isclosed"].ToString() == "1")
                    btnSave.Visible = btnCreateQutn.Visible = false;
                else
                {
                    btnSave.Visible = hdnUpdate.Value == "0" ? false : true;
                    btnCreateQutn.Visible = hdnCreateQutn.Value == "0" ? false : true;
                }

                btnHistory.Visible = hdnHistory.Value == "0" ? false : true;

                if (dtBasic.Rows[0]["Status"].ToString() != "2" &&
                    dtBasic.Rows[0]["Status"].ToString() != "3" &&
                    dtBasic.Rows[0]["isclosed"].ToString() != "1")
                    btnDelete.Visible = hdnDelete.Value == "0" ? false : true;
                else
                    btnDelete.Visible = false;

                btnMail.Visible = hdnSendMail.Value == "0" ? false : true;
                btnAgreementPrint.Visible = true;
                PanelAdd.Visible = true;
                UpdPanelAdd.Update();
            }
            else if(e.CommandName=="Print")
            {
                string url = "";
                    url = "../Reports/AgreementPdf.aspx?LeadId=" + Convert.ToInt32(hdnRptId.Value);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);

            }
        }

        protected void btnAgreementPrint_Click(object sender, EventArgs e)
        {
            string url = "";
            url = "../Reports/AgreementPdf.aspx?LeadId=" + Convert.ToInt32(hdnId.Value);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);

        }
        public void fu_FilesOnFileUploaded(object sender, FileUploadedEventArgs e)
        {
            
            fu_Files.TargetFolder = "~/UploadedFiles";

            foreach (UploadedFile upfile in fu_Files.UploadedFiles)
            {
               
                    DataTable dtprefix = systemUtilities.Get_File_Code("AllFile");
                    string files_namesave = dtprefix.Rows[0][0].ToString() + upfile.FileName;

                    upfile.SaveAs(Path.Combine(Server.MapPath(fu_Files.TargetFolder), files_namesave));

                try
                {
                    DataTable dtgen = masterBAL.Edit_GeneralSettings();

                    File.Copy(
                        Path.Combine(Server.MapPath(fu_Files.TargetFolder), files_namesave),
                        Path.Combine(dtgen.Rows[0]["BackupDrivePath"].ToString() + "UploadedFiles",
                            files_namesave),
                        false);
                }
                catch
                {

                }
                hdnfilenameup.Value = upfile.FileName;
                hdnfilenamesaveup.Value = files_namesave;
            }

            Updfu_Files.Update();
            
        }

        protected void btnAddDocument_Click(object sender, EventArgs e)
        {
            DataTable dt_doc = new DataTable();
            dt_doc.Columns.Add("Id", typeof(int));
            dt_doc.Columns.Add("DocumentId", typeof(int));
            dt_doc.Columns.Add("DocumentName", typeof(string));
            dt_doc.Columns.Add("Filenames", typeof(string));
            dt_doc.Columns.Add("FilenameSave", typeof(string));

            foreach (RepeaterItem itm in rptDocs.Items)
            {

                HiddenField hdnDocId = (HiddenField)itm.FindControl("hdnDocId");
                HiddenField hdnDocumentTypeId = (HiddenField)itm.FindControl("hdnDocumentTypeId");
                Label lblDocumentType = (Label)itm.FindControl("lblDocumentType");
                LinkButton lblfileupl = (LinkButton)itm.FindControl("lblfileupl");
                HiddenField hdnfilesaveupl = (HiddenField)itm.FindControl("hdnfilesaveupl");

                dt_doc.Rows.Add(Convert.ToInt32(hdnDocId.Value),Convert.ToInt32(hdnDocumentTypeId.Value), lblDocumentType.Text,
                    lblfileupl.Text, hdnfilesaveupl.Value);
            }

            if (hdnfilenameup.Value != "" && hdnfilenamesaveup.Value != "" && drpDocumentType.SelectedValue != "")
                dt_doc.Rows.Add(0, Convert.ToInt32(drpDocumentType.SelectedValue), drpDocumentType.SelectedItem.Text,
                    hdnfilenameup.Value, hdnfilenamesaveup.Value);

            drpDocumentType.ClearSelection();
            drpDocumentType.Text = "";
            hdnfilenamesaveup.Value = hdnfilenameup.Value = "";

            rptDocs.DataSource = dt_doc;
            rptDocs.DataBind();

            updDocumentList.Update();
        }

        protected void rptDocs_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                DataTable dt_doc = new DataTable();
                dt_doc.Columns.Add("Id", typeof(int));
                dt_doc.Columns.Add("DocumentId", typeof(int));
                dt_doc.Columns.Add("DocumentName", typeof(string));
                dt_doc.Columns.Add("Filenames", typeof(string));
                dt_doc.Columns.Add("FilenameSave", typeof(string));

                foreach (RepeaterItem itm in rptDocs.Items)
                {

                    HiddenField hdnDocId = (HiddenField)itm.FindControl("hdnDocId");
                    HiddenField hdnDocumentTypeId = (HiddenField)itm.FindControl("hdnDocumentTypeId");
                    Label lblDocumentType = (Label)itm.FindControl("lblDocumentType");
                    LinkButton lblfileupl = (LinkButton)itm.FindControl("lblfileupl");
                    HiddenField hdnfilesaveupl = (HiddenField)itm.FindControl("hdnfilesaveupl");

                    dt_doc.Rows.Add(Convert.ToInt32(hdnDocId.Value), Convert.ToInt32(hdnDocumentTypeId.Value), lblDocumentType.Text,
                        lblfileupl.Text, hdnfilesaveupl.Value);
                }

                dt_doc.Rows.RemoveAt(e.Item.ItemIndex);

                rptDocs.DataSource = dt_doc;
                rptDocs.DataBind();

                updDocumentList.Update();
            }
           else if (e.CommandName == "Download")
            {
                HiddenField hdnfilesaveupl = (HiddenField)e.Item.FindControl("hdnfilesaveupl");
                LinkButton lblfileupl = (LinkButton)e.Item.FindControl("lblfileupl");

                try
                {
                    if (hdnfilesaveupl.Value != "")
                    {
                        string fil_name = hdnfilesaveupl.Value;
                        string full_name = Server.MapPath("~/UploadedFiles/" + fil_name);
                        Response.ContentType = "APPLICATION/OCTET-STREAM";
                        String Header = "Attachment; Filename=\"" + lblfileupl.Text + "\"";
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

            DataSet ds = masterBAL.DrpQuestion123(
                hdnDepartmentId.Value == "" ? 0 : Convert.ToInt32(hdnDepartmentId.Value));

            drpSerCategory.Text = "";
            drpSerCategory.DataSource = ds.Tables[0];
            drpSerCategory.DataTextField = "Text";
            drpSerCategory.DataValueField = "Value";
            drpSerCategory.DataBind();
            drpSerCategory.SelectedValue = hdnSerCategoryId.Value;
        }

        protected void drpStatusfilterOnSelectedIndexChanged(object sender,
            RadComboBoxSelectedIndexChangedEventArgs e)
        {
            fillgridList(1, Convert.ToInt32(drpPageSize.SelectedValue),
                hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
        }

        protected void drpSource_SelectedIndexChanged(object sender,
            RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (drpSource.SelectedValue == "0")
            {
                int val = systemUtilities.Form_Previlage_Validation(133,
                    Convert.ToInt32(hdnUserId.Value));
                if (val == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message",
                        "alert('Sorry you do not have privilege to create new lead source..!');", true);
                    drpSource.ClearSelection();
                    updSource.Update();
                }
                else
                {
                    pnlsource.Visible = true;
                    UCLeadsource.PageLoad();
                    updSourcePanel.Update();
                }
            }
        }

        protected void drpJurisdiction_SelectedIndexChanged(object sender,
            RadComboBoxSelectedIndexChangedEventArgs e)
        { }

        protected void drpSegment_SelectedIndexChanged(object sender,
            RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (drpSegment.SelectedValue == "0")
            {
                pnlSegment.Visible = true;
                UCSegment.PageLoad();
                updSegmentPanel.Update();
            }
        }

        protected void drpcity_SelectedIndexChanged(object sender,
            RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (drpCity.SelectedValue == "0")
            {
                pnlCity.Visible = true;
                UCCity.PageLoad();
                updCityPanel.Update();
            }
        }

        protected void drpPriority_SelectedIndexChanged(object sender,
            RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (drpPriority.SelectedValue == "0")
            {
                int val = systemUtilities.Form_Previlage_Validation(134,
                    Convert.ToInt32(hdnUserId.Value));
                if (val == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message",
                        "alert('Sorry you do not have privilege to create new Priority..!');", true);
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
                    dtService.Rows.Add(
                        Convert.ToInt32(hdnDId.Value),
                        drpDepartment.SelectedValue == "" ? (int?)null
                            : Convert.ToInt32(drpDepartment.SelectedValue),
                        drpSerCategory.SelectedValue == "" ? (int?)null
                            : Convert.ToInt32(drpSerCategory.SelectedValue));
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
            UpdatePanel UpdSerCategoryDropdown = (UpdatePanel)itemrp.FindControl("UpdSerCategoryDropdown");
            RadComboBox drpDepartment = (RadComboBox)itemrp.FindControl("drpDepartment");
            RadComboBox drpSerCategory = (RadComboBox)itemrp.FindControl("drpSerCategory");
            HiddenField hdnDepartmentId = (HiddenField)itemrp.FindControl("hdnDepartmentId");
            HiddenField hdnSerCategoryId = (HiddenField)itemrp.FindControl("hdnSerCategoryId");

            if (contrlName == "drpDepartment")
            {
                drpSerCategory.Text = "";
                drpSerCategory.ClearSelection();
                hdnDepartmentId.Value = drpDepartment.SelectedValue;
                hdnSerCategoryId.Value = "";
            }

            int Department = drpDepartment.SelectedValue == ""
                ? 0 : Convert.ToInt32(drpDepartment.SelectedValue);
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

        protected void btnMailOnClick(object sender, EventArgs e)
        {
            int res = 0;
            if (hdnId.Value == "0")
            {
                res = SaveLead();
                fillgridList(Convert.ToInt32(lblPageNumber.Text),
                    Convert.ToInt32(drpPageSize.SelectedValue),
                    hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
                Clear();
            }
            else if (hdnStatus.Value != "3")
            {
                res = SaveLead();
                fillgridList(Convert.ToInt32(lblPageNumber.Text),
                    Convert.ToInt32(drpPageSize.SelectedValue),
                    hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
                Clear();
            }
            else
                res = Convert.ToInt32(hdnId.Value);

            DataTable dt = TransBal.LeadMailBody(res);
            if (dt.Rows.Count > 0)
            {
                EmailUC.UCPageLoad(1, res, dt.Rows[0]["Toaddress"].ToString());
                pnlMail.Visible = true;
                UpdMailPanel.Update();
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request. ";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup",
                    "ToggleDiv();", true);
            }

            UpdPanelAdd.Update();
        }

        protected void btnSaveOnClick(object sender, EventArgs e)
        {
            int res = SaveLead();
            if (res > 0)
            {
                fillgridList(Convert.ToInt32(lblPageNumber.Text),
                    Convert.ToInt32(drpPageSize.SelectedValue),
                    hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup",
                    "ToggleDiv();", true);
                PanelAdd.Visible = false;
            }
            else if (res == -1)
            {
                lbl_msg.Text = "Add questionaire details !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup",
                    "ToggleDiv();", true);
            }
            else if (res == -2)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Mobile number already exist.!');", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup",
                    "ToggleDiv();", true);
            }

            UpdPanelAdd.Update();
        }

        public int SaveLead()
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
                    dtService.Rows.Add(
                        Convert.ToInt32(hdnDId.Value),
                        drpDepartment.SelectedValue == "" ? (int?)null
                            : Convert.ToInt32(drpDepartment.SelectedValue),
                        drpSerCategory.SelectedValue == "" ? (int?)null
                            : Convert.ToInt32(drpSerCategory.SelectedValue),
                        (int?)null, (int?)null, 0);
            }

            DataTable dt_doc = new DataTable();
            dt_doc.Columns.Add("Id", typeof(int));
            dt_doc.Columns.Add("DocumentId", typeof(int));            
            dt_doc.Columns.Add("Filenames", typeof(string));
            dt_doc.Columns.Add("FilenameSave", typeof(string));

            foreach (RepeaterItem itm in rptDocs.Items)
            {

                HiddenField hdnDocId = (HiddenField)itm.FindControl("hdnDocId");
                HiddenField hdnDocumentTypeId = (HiddenField)itm.FindControl("hdnDocumentTypeId");
                LinkButton lblfileupl = (LinkButton)itm.FindControl("lblfileupl");
                HiddenField hdnfilesaveupl = (HiddenField)itm.FindControl("hdnfilesaveupl");

                dt_doc.Rows.Add(Convert.ToInt32(hdnDocId.Value), Convert.ToInt32(hdnDocumentTypeId.Value),
                    lblfileupl.Text, hdnfilesaveupl.Value);
            }



            int? currentStatusVal = drpCurrentStatus.SelectedValue == "" ? (int?)null
                : Convert.ToInt32(drpCurrentStatus.SelectedValue);
            int? maritalStatusVal = drpMaritalStatus.SelectedValue == "" ? (int?)null
                : Convert.ToInt32(drpMaritalStatus.SelectedValue);

            int res = TransBal.InsertUpdateLeadCreation(
                Convert.ToInt32(hdnId.Value),
                txtcompany.Text, txtAddress.Text, txtMobileNumber.Text, txtEmailId.Text,
                Convert.ToInt32(hdnUserId.Value),
                drpEmployee.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpEmployee.SelectedValue),
                txtphone.Text, txtcompany.Text, txtResponse.Text,
                Convert.ToInt32(drpPriority.SelectedValue),
                drpSource.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpSource.SelectedValue),
                Followupdate.SelectedDate, ApprxClosingDate.SelectedDate,
                dtService, radFollowupTime.SelectedDate,
                drpJurisdiction.SelectedValue == "" ? (int?)null
                    : Convert.ToInt32(drpJurisdiction.SelectedValue),
                leadDate.SelectedDate,
                txtActivity.Text, txtCPDesig.Text, txtwebsite.Text, txtCampaign.Text,
                txtCountryCodeCN.Text, txtCountryCodeLPN.Text,
                drpCity.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpCity.SelectedValue),
                drpSegment.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpSegment.SelectedValue),
                // NEW FIELDS
                txtLeadBrand.Text, txtPassportNo.Text,
                dpPassportIssueDate.SelectedDate, dpPassportExpiryDate.SelectedDate,
                currentStatusVal, dpDOB.SelectedDate,
                txtNationality.Text, maritalStatusVal, txtMotherName.Text,
                dt_doc

            );

            return res;
        }

        protected void btnCreateQutn_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Transactions/Quotation.aspx?LeadId=" + hdnId.Value);
        }

        protected void btnDeleteOnClick(object sender, EventArgs e)
        {
            int res = TransBal.DeleteLeadCreation(Convert.ToInt32(hdnId.Value),
                Convert.ToInt32(hdnUserId.Value));
            if (res == 1)
            {
                fillgridList(Convert.ToInt32(lblPageNumber.Text),
                    Convert.ToInt32(drpPageSize.SelectedValue),
                    hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
                Clear();
                lbl_msg.Text = "Deleted Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup",
                    "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup",
                    "ToggleDiv();", true);
            }

            PanelAdd.Visible = false;
            UpdPanelAdd.Update();
        }

        protected void btnResetOnClick(object sender, EventArgs e)
        {
            Clear();
            Followupdate.MinDate = ApprxClosingDate.MinDate = DateTime.Now;
            UpdPanelAddInner.Update();
        }

        protected void btnCloseOnClick(object sender, EventArgs e)
        {
            PanelAdd.Visible = false;
            UpdPanelAdd.Update();
        }

        protected void btnNewEntryOnClick(object sender, EventArgs e)
        {
            Clear();
            PanelAdd.Visible = true;
            Followupdate.MinDate = ApprxClosingDate.MinDate = DateTime.Now;
            UpdPanelAdd.Update();
        }

        #region Excel Upload

        protected void btnupload_Click(object sender, EventArgs e)
        {
            pnlupload.Visible = true;
            UpdPanelAdd.Update();
        }

        protected void btnverfy_Click(object sender, EventArgs e)
        {
            DataTable ContentTable = new DataTable();
            ContentTable.Columns.Add("Date", typeof(DateTime));
            ContentTable.Columns.Add("Campaign", typeof(string));
            ContentTable.Columns.Add("CompanyName", typeof(string));
            ContentTable.Columns.Add("ContactPersonName", typeof(string));
            ContentTable.Columns.Add("CountryCodeContactNumber", typeof(string));
            ContentTable.Columns.Add("ContactNumber", typeof(string));
            ContentTable.Columns.Add("ContactPersondesignation", typeof(string));
            ContentTable.Columns.Add("LandPhoneNoCountryCode", typeof(string));
            ContentTable.Columns.Add("LandPhoneNo", typeof(string));
            ContentTable.Columns.Add("Email", typeof(string));
            ContentTable.Columns.Add("Website", typeof(string));  // LeadBrand
            ContentTable.Columns.Add("Activity", typeof(string)); // Scope
            ContentTable.Columns.Add("ActivityDescription", typeof(string));
            ContentTable.Columns.Add("CustomerResponse", typeof(string));
            ContentTable.Columns.Add("LeadSourceId", typeof(int));
            ContentTable.Columns.Add("AssignedEmployeeId", typeof(int));
            ContentTable.Columns.Add("PriorityId", typeof(int));
            ContentTable.Columns.Add("SegmentId", typeof(int));
            ContentTable.Columns.Add("CityId", typeof(int));

            foreach (RepeaterItem itm in rptuploaddetail.Items)
            {
                Label lbldate = (Label)itm.FindControl("lbldate");
                Label lblCompanyName = (Label)itm.FindControl("lblCompanyName");
                Label lblContactNumber = (Label)itm.FindControl("lblContactNumber");
                Label lblEmail = (Label)itm.FindControl("lblEmail");
                Label lblLeadBrand = (Label)itm.FindControl("lblLeadBrand");
                Label lblScope = (Label)itm.FindControl("lblScope");
                HiddenField hdnLeadSourceId = (HiddenField)itm.FindControl("hdnLeadSourceId");

                if (lbldate.Text != "" && lblCompanyName.Text != "")
                    ContentTable.Rows.Add(
                        Convert.ToDateTime(lbldate.Text),
                        "",                                                           // Campaign
                        lblCompanyName.Text,                                          // CompanyName
                        "",                                                           // ContactPersonName
                        "",                                                           // CountryCodeContactNumber
                        lblContactNumber != null ? lblContactNumber.Text : "",        // ContactNumber
                        "",                                                           // ContactPersondesignation
                        "",                                                           // LandPhoneNoCountryCode
                        "",                                                           // LandPhoneNo
                        lblEmail != null ? lblEmail.Text : "",                        // Email
                        lblLeadBrand != null ? lblLeadBrand.Text : "",                // Website = LeadBrand
                        lblScope != null ? lblScope.Text : "",                        // Activity = Scope
                        "",                                                           // ActivityDescription
                        "",                                                           // CustomerResponse
                        hdnLeadSourceId != null && hdnLeadSourceId.Value != ""
                            ? Convert.ToInt32(hdnLeadSourceId.Value) : (int?)null,    // LeadSourceId
                        (int?)null,                                                   // AssignedEmployeeId
                        (int?)null,                                                   // PriorityId
                        (int?)null,                                                   // SegmentId
                        (int?)null                                                    // CityId
                    );
            }

            int res = TransBal.InsertLeadList(ContentTable, Convert.ToInt32(hdnUserId.Value));
            if (res > 0)
            {
                fillgridList(Convert.ToInt32(lblPageNumber.Text),
                    Convert.ToInt32(drpPageSize.SelectedValue),
                    hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                pnlupload.Visible = false;
            }
            else if (res == -1)
            {
                lbl_msg.Text = "Add questionaire details !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            UpdPanelAdd.Update();
        }

        protected void Btndwnformat_Click(object sender, EventArgs e)
        {
            try
            {
                string fil_name = "LeadUploadFormat.xls";
                Response.ContentType = "APPLICATION/OCTET-STREAM";
                Response.AppendHeader("Content-Disposition",
                    "Attachment; Filename=\"" + fil_name + "\"");
                System.IO.FileInfo Dfile = new System.IO.FileInfo(
                    Server.MapPath("~/UploadedFiles/" + fil_name));
                Response.WriteFile(Dfile.FullName);
                Response.End();
            }
            catch { }
        }

        public void fu_DocUpload_OnFileUploaded(object sender, FileUploadedEventArgs e)
        {
            fu_DocUpload.TargetFolder = "~/UploadedFiles";
            string Prefix = "F-";
            string files_name = hdnleadFile.Value =
                Prefix + e.File.GetNameWithoutExtension() + e.File.GetExtension();
            e.File.SaveAs(Path.Combine(Server.MapPath(fu_DocUpload.TargetFolder), files_name));

            try
            {
                DataTable dtgen = masterBAL.Edit_GeneralSettings();
                File.Copy(
                    Path.Combine(Server.MapPath(fu_DocUpload.TargetFolder), files_name),
                    Path.Combine(dtgen.Rows[0]["BackupDrivePath"].ToString() + "UploadedFiles",
                        files_name), false);
            }
            catch { }

            hdnleadfileExtension.Value = e.File.GetExtension();
            updleadFile.Update();
        }

        protected void btnleadUpload_Click(object sender, EventArgs e)
        {
            DataTable ContentTable = new DataTable();
            ContentTable.Columns.Add("Date", typeof(DateTime));
            ContentTable.Columns.Add("Campaign", typeof(string));
            ContentTable.Columns.Add("CompanyName", typeof(string));
            ContentTable.Columns.Add("ContactPersonName", typeof(string));
            ContentTable.Columns.Add("Segment", typeof(string));
            ContentTable.Columns.Add("LeadSource", typeof(string));
            ContentTable.Columns.Add("City", typeof(string));
            ContentTable.Columns.Add("CountryCodeContactNumber", typeof(string));
            ContentTable.Columns.Add("ContactNumber", typeof(string));
            ContentTable.Columns.Add("ContactPersondesignation", typeof(string));
            ContentTable.Columns.Add("LandPhoneNoCountryCode", typeof(string));
            ContentTable.Columns.Add("LandPhoneNo", typeof(string));
            ContentTable.Columns.Add("Email", typeof(string));
            ContentTable.Columns.Add("Website", typeof(string)); //leadbrand
            ContentTable.Columns.Add("AssignedEmployee", typeof(string));
            ContentTable.Columns.Add("Priority", typeof(string));
            //ContentTable.Columns.Add("Segment", typeof(string));  // index 4
            ContentTable.Columns.Add("Activity", typeof(string));
            ContentTable.Columns.Add("ActivityDescription", typeof(string));
            ContentTable.Columns.Add("CustomerResponse", typeof(string));

            rptuploaddetail.DataSource = null;
            rptuploaddetail.DataBind();

            if (hdnleadFile.Value != "")
            {
                string connString = "";
                string filepath = Path.Combine(Server.MapPath("~/UploadedFiles"), hdnleadFile.Value);
                string ext = hdnleadfileExtension.Value.ToLower().Trim();

                if (ext == ".xls")
                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath
                        + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                else if (ext == ".xlsx")
                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath
                        + ";Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1\"";

                OleDbConnection OledbConn = new OleDbConnection(connString);
                try
                {
                    OleDbCommand OledbCmd = new OleDbCommand();
                    OledbCmd.Connection = OledbConn;
                    OledbConn.Open();
                    var sheetNames = OledbConn.GetSchema("Tables");
                    OledbCmd.CommandText = "Select * from ["
                        + sheetNames.Rows[0]["TABLE_NAME"].ToString() + "]";
                    OleDbDataReader dr = OledbCmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            try
                            {
                                ContentTable.Rows.Add(
                                    dr["Date"].ToString().Trim() == string.Empty ? (DateTime?)null
                                        : Convert.ToDateTime(dr["Date"].ToString().Trim()),
                                    "",                                        // Campaign
                                    dr["Lead Name"].ToString().Trim(),         // CompanyName
                                    "",                                        // ContactPersonName
                                    "",                                        // Segment
                                    dr["Platform"].ToString().Trim(),          // LeadSource
                                    "",                                        // City
                                    "",                                        // CountryCodeContactNumber
                                    dr["Phone Number"].ToString().Trim(),      // ContactNumber
                                    "",                                        // ContactPersondesignation
                                    "",                                        // LandPhoneNoCountryCode
                                    "",                                        // LandPhoneNo
                                    dr["Email"].ToString().Trim(),             // Email
                                    dr["Lead Brand"].ToString().Trim(),        // Website = LeadBrand
                                    "",                                        // AssignedEmployee
                                    "",                                        // Priority
                                    dr["Scope"].ToString().Trim(),             // Activity = Scope
                                    "",                                        // ActivityDescription
                                    ""                                         // CustomerResponse
                                );
                            }
                            catch (Exception ex1)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "showalert",
                                    "alert('" + ex1.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
                            }
                        }
                    }
                    dr.Close();
                    OledbConn.Close();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert",
                        "alert('Error: " + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert",
                    "alert('Please upload the file');", true);
            }

            if (ContentTable.Rows.Count > 0)
            {
                DataSet ds = TransBal.LeadUploadTable(ContentTable, Convert.ToInt32(hdnUserId.Value));
                DataTable restble = ds.Tables[0];
                string msg = ds.Tables[1].Rows[0][0].ToString();

                if (restble.Rows.Count > 0)
                {
                    rptuploaddetail.DataSource = restble;
                    rptuploaddetail.DataBind();
                    if (msg != "")
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert",
                            "alert('" + msg + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert",
                        "alert('Sorry failed to process your request. Try again');", true);
                }
            }
            updleadFileList.Update();
        }

        protected void btnverfyclose_Click(object sender, EventArgs e)
        {
            pnlupload.Visible = false;
            UpdPanelAdd.Update();
        }

        #endregion

        public void Clear()
        {
            hdnId.Value = "0";
            txtName.Text = "";
            txtAddress.Text = txtResponse.Text = "";
            txtEmailId.Text = txtcompany.Text = "";
            txtMobileNumber.Text = txtphone.Text = "";
            drpEmployee.ClearSelection();
            drpEmployee.Text = "";
            drpPriority.ClearSelection();
            drpPriority.Text = "";
            drpSource.ClearSelection();
            drpSource.Text = "";
            drpJurisdiction.ClearSelection();
            drpJurisdiction.Text = txtActivity.Text = txtCPDesig.Text = txtwebsite.Text = "";
            Followupdate.DbSelectedDate = ApprxClosingDate.DbSelectedDate = "";
            radFollowupTime.SelectedDate = null;
            Followupdate.MinDate = DateTime.Now.AddDays(-365);
            leadDate.SelectedDate = DateTime.Now;
            txtCountryCodeCN.Text = "+971";
            txtCampaign.Text = txtCountryCodeLPN.Text = "";
            drpCity.ClearSelection();
            drpCity.Text = "";
            drpSegment.ClearSelection();
            drpSegment.Text = "";

            // NEW FIELDS
            txtLeadBrand.Text = "";
            txtPassportNo.Text = "";
            dpPassportIssueDate.DbSelectedDate = "";
            dpPassportExpiryDate.DbSelectedDate = "";
            drpCurrentStatus.ClearSelection();
            drpCurrentStatus.Text = "";
            dpDOB.DbSelectedDate = "";
            txtNationality.Text = "";
            drpMaritalStatus.ClearSelection();
            drpMaritalStatus.Text = "";
            txtMotherName.Text = "";


            // Reset service repeater
            DataTable dtService = new DataTable();
            dtService.Columns.Add("Id", typeof(int));
            dtService.Columns.Add("DepartmentId", typeof(int));
            dtService.Columns.Add("CategoryId", typeof(int));
            dtService.Rows.Add(0, null);
            rptservice.DataSource = dtService;
            rptservice.DataBind();

            rptDocs.DataSource = null;
            rptDocs.DataBind();

            btnDelete.Visible = btnAgreementPrint.Visible= btnHistory.Visible = btnCreateQutn.Visible = false;
            btnSave.Visible = hdnAdd.Value == "0" ? false : true;
            btnMail.Visible = hdnSendMail.Value == "0" ? false : true;
            Get_Code();
            UpdPanelAddInner.Update();
        }

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
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow",
                "window.open('" + url
                + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,"
                + "location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
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
            DataTable dt = TransBal.ListLeadHistory(Convert.ToInt32(hdnId.Value),
                page_number, page_size);
            rptHistory.DataSource = dt;
            rptHistory.DataBind();

            if (dt.Rows.Count > 0)
            {
                lbl_page_info1.Text = "Showing Results "
                    + dt.Rows[0]["StartNumber"].ToString() + " - "
                    + dt.Rows[dt.Rows.Count - 1]["RowNum"].ToString()
                    + " Out of " + dt.Rows[0]["CurrentCount"].ToString() + " Records";
                hdn_last_page1.Value = dt.Rows[0]["LastPage"].ToString();
                lbl_page_number1.Text = dt.Rows[0]["PageNumber"].ToString();
                hdn_total1.Value = dt.Rows[0]["CurrentCount"].ToString();
            }
            else
            {
                lbl_page_info1.Text = "Showing Results 0 - 0 Out of 0 Records";
                hdn_last_page1.Value = "0";
                lbl_page_number1.Text = "1";
                hdn_total1.Value = "0";
            }
            upd_his_nav.Update();
            Upd_History.Update();
        }

        #region His Navigation
        protected void btn_first1_OnClick(object sender, EventArgs e)
        { grid_fill_his(1, Convert.ToInt32(drp_count1.SelectedValue)); }

        protected void btn_prev1_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number1.Text) > 1)
                grid_fill_his(Convert.ToInt32(lbl_page_number1.Text) - 1,
                    Convert.ToInt32(drp_count1.SelectedValue));
        }

        protected void btn_next1_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number1.Text) < Convert.ToInt32(hdn_last_page1.Value))
                grid_fill_his(Convert.ToInt32(lbl_page_number1.Text) + 1,
                    Convert.ToInt32(drp_count1.SelectedValue));
        }

        protected void btn_last1_OnClick(object sender, EventArgs e)
        {
            grid_fill_his(Convert.ToInt32(hdn_last_page1.Value),
            Convert.ToInt32(drp_count1.SelectedValue));
        }

        protected void drp_count1_OnSelectedIndexChanged(object sender, EventArgs e)
        { grid_fill_his(1, Convert.ToInt32(drp_count1.SelectedValue)); }
        #endregion

        #endregion

        #region Navigation

        protected void txtSearchOnTextChanged(object sender, EventArgs e)
        {
            fillgridList(1, Convert.ToInt32(drpPageSize.SelectedValue),
                txtSearch.Text, hdnOrderByColumnName.Value, hdnOrderBy.Value);
        }

        protected void btnFirstOnClick(object sender, EventArgs e)
        {
            fillgridList(1, Convert.ToInt32(drpPageSize.SelectedValue),
            hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
        }

        protected void btnPreviousOnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lblPageNumber.Text) > 1)
                fillgridList(Convert.ToInt32(lblPageNumber.Text) - 1,
                    Convert.ToInt32(drpPageSize.SelectedValue),
                    hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
        }

        protected void btnNextOnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lblPageNumber.Text) < Convert.ToInt32(hdnLastPage.Value))
                fillgridList(Convert.ToInt32(lblPageNumber.Text) + 1,
                    Convert.ToInt32(drpPageSize.SelectedValue),
                    hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
        }

        protected void btnLastOnClick(object sender, EventArgs e)
        {
            fillgridList(Convert.ToInt32(hdnLastPage.Value),
            Convert.ToInt32(drpPageSize.SelectedValue),
            hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
        }

        protected void drpPageSizeOnSelectedIndexChanged(object sender, EventArgs e)
        {
            fillgridList(1, Convert.ToInt32(drpPageSize.SelectedValue),
            hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
        }

        #endregion

        public void CheckPrivilege()
        {
            try
            {
                if (hdnUserId.Value != null)
                {
                    int val = systemUtilities.Form_Previlage_Validation(138,
                        Convert.ToInt32(hdnUserId.Value));
                    if (val == 0) Response.Redirect("../Landing.aspx");
                }
                else Response.Redirect("../Landing.aspx");
            }
            catch { Response.Redirect("../Landing.aspx"); }
        }

        public void CheckActionPrivilege()
        {
            try
            {
                if (hdnUserId.Value != null)
                {
                    DataTable dtSubMenuAction = systemUtilities.Action_Previlage_Validation(138,
                        Convert.ToInt32(hdnUserId.Value));
                    if (dtSubMenuAction.Rows.Count > 0)
                    {
                        hdnAdd.Value = dtSubMenuAction.Rows[0][1].ToString();
                        hdnUpdate.Value = dtSubMenuAction.Rows[1][1].ToString();
                        hdnDelete.Value = dtSubMenuAction.Rows[2][1].ToString();
                        hdnHistory.Value = dtSubMenuAction.Rows[3][1].ToString();
                        hdnSendMail.Value = dtSubMenuAction.Rows[4][1].ToString();
                        hdnCreateQutn.Value = dtSubMenuAction.Rows[5][1].ToString();
                    }
                    btnSave.Visible = hdnAdd.Value == "0" ? false : true;
                }
                else Response.Redirect("../Landing.aspx");
            }
            catch { Response.Redirect("../Landing.aspx"); }
        }

      
    }
}