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

        // ─────────────────────────────────────────────────────────────
        // Page Load
        // ─────────────────────────────────────────────────────────────
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_Id"] == null)
                Response.Redirect("~/Landing.aspx");

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

        // ─────────────────────────────────────────────────────────────
        // Dropdowns
        // ─────────────────────────────────────────────────────────────
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
        }

        public void Get_Code()
        {
            DataTable dt = systemUtilities.Get_Code(138);
            if (dt.Rows.Count > 0)
                lbl_Code.Text = dt.Rows[0][0].ToString();
        }

        // ─────────────────────────────────────────────────────────────
        // Grid / List
        // ─────────────────────────────────────────────────────────────
        public void fillgridList(int PageNumber, int PageSize, string Filter, string OrderByColumnName, string OrderBy)
        {
            DataTable dtEmployeeList = TransBal.GetLeadList(
                PageNumber, PageSize, Filter,
                drpStatusfilter.SelectedValue == "" ? 0 : Convert.ToInt32(drpStatusfilter.SelectedValue),
                drpprorityfilter.SelectedValue == "" ? 0 : Convert.ToInt32(drpprorityfilter.SelectedValue),
                Convert.ToInt32(hdnUserId.Value));

            rptList.DataSource = dtEmployeeList;
            rptList.DataBind();

            if (dtEmployeeList.Rows.Count > 0)
            {
                lblPageInfo.Text = "Showing Results " + dtEmployeeList.Rows[0]["StartNumber"].ToString()
                    + " - " + dtEmployeeList.Rows[dtEmployeeList.Rows.Count - 1]["RowNum"].ToString()
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

        // ─────────────────────────────────────────────────────────────
        // Excel Export
        // ─────────────────────────────────────────────────────────────
        public void btnExportToExcelOnClick(object sender, EventArgs e)
        {
            DataTable dtEmployeeList = TransBal.GetLeadCreationListExcel(Convert.ToInt32(hdnUserId.Value));
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

        // ─────────────────────────────────────────────────────────────
        // Edit Lead (list item click)
        // ─────────────────────────────────────────────────────────────
        protected void rptListOnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            Clear();
            HiddenField hdnRptId = (HiddenField)e.Item.FindControl("hdnId");
            DataSet dsLeadDetails = TransBal.EditLeadCreation(Convert.ToInt32(hdnRptId.Value));
            DataTable dtBasic = dsLeadDetails.Tables[0];
            DataTable dtService = dsLeadDetails.Tables[1];

            txtAddress.Text = dtBasic.Rows[0]["Address"].ToString();
            txtMobileNumber.Text = dtBasic.Rows[0]["MobileNumber"].ToString();
            txtEmailId.Text = dtBasic.Rows[0]["EmailId"].ToString();
            txtName.Text = dtBasic.Rows[0]["ContactPersonName"].ToString();
            hdnId.Value = dtBasic.Rows[0]["Id"].ToString();
            drpSource.SelectedValue = dtBasic.Rows[0]["LeadSourceId"].ToString();
            txtphone.Text = dtBasic.Rows[0]["LandPhoneNo"].ToString();
            drpPriority.SelectedValue = dtBasic.Rows[0]["Priority"].ToString();
            txtcompany.Text = dtBasic.Rows[0]["CompanyName"].ToString();
            txtCompanyName.Text = dtBasic.Rows[0]["CompanyName"].ToString(); // Company Name field
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

            // New fields
            txtLeadBrand.Text = dtBasic.Rows[0]["LeadBrand"].ToString();
            txtPassportNo.Text = dtBasic.Rows[0]["PassportNo"].ToString();
            dpPassportIssueDate.DbSelectedDate = dtBasic.Rows[0]["PassportIssueDate"].ToString();
            dpPassportExpiryDate.DbSelectedDate = dtBasic.Rows[0]["PassportExpiryDate"].ToString();
            dpDOB.DbSelectedDate = dtBasic.Rows[0]["DOB"].ToString();
            drpCurrentStatus.SelectedValue = dtBasic.Rows[0]["CurrentStatus"].ToString();
            txtNationality.Text = dtBasic.Rows[0]["Nationality"].ToString();
            drpMaritalStatus.SelectedValue = dtBasic.Rows[0]["MaritalStatus"].ToString();
            txtMotherName.Text = dtBasic.Rows[0]["MotherName"].ToString();

            // Employee dropdown
            drpEmployee.DataSource = TransBal.DrpEmployeeTrans(1);
            drpEmployee.DataValueField = "Value";
            drpEmployee.DataTextField = "Text";
            drpEmployee.DataBind();
            drpEmployee.SelectedValue = dtBasic.Rows[0]["AssignedEmployeeId"].ToString();

            // Q&A service rows
            if (dtService.Rows.Count == 0)
                dtService.Rows.Add(0, null);
            rptservice.DataSource = dtService;
            rptservice.DataBind();

            // Load existing documents
            LoadDocuments(Convert.ToInt32(hdnRptId.Value));

            // Button visibility
            if (dtBasic.Rows[0]["isclosed"].ToString() == "1")
                btnSave.Visible = btnCreateQutn.Visible = false;
            else
            {
                btnSave.Visible = hdnUpdate.Value == "0" ? false : true;
                btnCreateQutn.Visible = hdnCreateQutn.Value == "0" ? false : true;
            }

            btnHistory.Visible = hdnHistory.Value == "0" ? false : true;

            if (dtBasic.Rows[0]["Status"].ToString() != "2" && dtBasic.Rows[0]["Status"].ToString() != "3" &&
                dtBasic.Rows[0]["isclosed"].ToString() != "1")
                btnDelete.Visible = hdnDelete.Value == "0" ? false : true;
            else
                btnDelete.Visible = false;

            btnMail.Visible = hdnSendMail.Value == "0" ? false : true;

            PanelAdd.Visible = true;
            UpdPanelAdd.Update();
        }

        // ─────────────────────────────────────────────────────────────
        // Q&A Repeater
        // ─────────────────────────────────────────────────────────────
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

        // ─────────────────────────────────────────────────────────────
        // ============================================================
        // DOCUMENT UPLOAD SECTION
        // ============================================================
        // ─────────────────────────────────────────────────────────────

        /// <summary>Returns a fresh empty DataTable matching LeadDocument structure.</summary>
        private DataTable GetDocumentDataTable()
        {
            DataTable dtDoc = new DataTable();
            dtDoc.Columns.Add("Id", typeof(int));
            dtDoc.Columns.Add("DocumentId", typeof(int));
            dtDoc.Columns.Add("Filenames", typeof(string));
            dtDoc.Columns.Add("FilenameSave", typeof(string));
            return dtDoc;
        }

        /// <summary>Load documents for a saved lead and bind the repeater.</summary>
        private void LoadDocuments(int leadId)
        {
            DataTable dtDocs = TransBal.GetLeadDocuments(leadId);
            if (dtDocs == null || dtDocs.Rows.Count == 0)
            {
                DataTable dtEmpty = GetDocumentDataTable();
                dtEmpty.Rows.Add(0, DBNull.Value, "", "");
                rptDocuments.DataSource = dtEmpty;
            }
            else
            {
                rptDocuments.DataSource = dtDocs;
            }
            rptDocuments.DataBind();
            UpdDocUpload.Update();
        }

        /// <summary>Bind document type dropdown and set selected value for each row.</summary>
        protected void rptDocumentsOnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            RadComboBox drpDocumentType = (RadComboBox)e.Item.FindControl("drpDocumentType");
            HiddenField hdnDocumentId = (HiddenField)e.Item.FindControl("hdnDocumentId");
            HiddenField hdnFileName = (HiddenField)e.Item.FindControl("hdnFileName");
            Button btnDocDownload = (Button)e.Item.FindControl("btnDocDownload");

            // Use existing fill_drp_DocType() from Master_Bal (calls drp_DocType SP)
            DataTable dtDocTypes = masterBAL.fill_drp_DocType();
            drpDocumentType.DataSource = dtDocTypes;
            drpDocumentType.DataTextField = "Text";
            drpDocumentType.DataValueField = "Value";
            drpDocumentType.DataBind();

            if (hdnDocumentId.Value != "" && hdnDocumentId.Value != "0")
            {
                try { drpDocumentType.SelectedValue = hdnDocumentId.Value; }
                catch { }
            }

            // Show download button only when a file is attached
            if (btnDocDownload != null)
                btnDocDownload.Visible = hdnFileName.Value != "";
        }

        /// <summary>Handle Add / Delete / Download commands in the document repeater.</summary>
        protected void rptDocumentsOnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Download")
            {
                HiddenField hdnFileNameSave = (HiddenField)e.Item.FindControl("hdnFileNameSave");
                HiddenField hdnFileName = (HiddenField)e.Item.FindControl("hdnFileName");

                if (hdnFileNameSave.Value != "")
                {
                    string filePath = Server.MapPath("~/UploadedFiles/" + hdnFileNameSave.Value);
                    if (File.Exists(filePath))
                    {
                        Response.ContentType = "APPLICATION/OCTET-STREAM";
                        Response.AppendHeader("Content-Disposition",
                            "Attachment; Filename=\"" + hdnFileName.Value + "\"");
                        Response.WriteFile(filePath);
                        Response.End();
                    }
                }
                return;
            }

            // Collect current rows
            DataTable dtDoc = CollectDocumentRows();

            if (e.CommandName == "Delete")
            {
                // Delete from DB if it is an existing saved row
                HiddenField hdnDocId = (HiddenField)e.Item.FindControl("hdnDocId");
                if (hdnDocId != null &&
                    hdnDocId.Value != "0" && hdnDocId.Value != "" &&
                    hdnId.Value != "0")
                {
                    try
                    {
                        TransBal.DeleteLeadDocument(
                            Convert.ToInt32(hdnDocId.Value),
                            Convert.ToInt32(hdnUserId.Value));
                    }
                    catch { }
                }

                int indx = e.Item.ItemIndex;
                if (indx < dtDoc.Rows.Count)
                    dtDoc.Rows.RemoveAt(indx);

                // Always keep at least one empty row
                if (dtDoc.Rows.Count == 0)
                    dtDoc.Rows.Add(0, DBNull.Value, "", "");
            }
            else if (e.CommandName == "Add")
            {
                dtDoc.Rows.Add(0, DBNull.Value, "", "");
            }

            rptDocuments.DataSource = dtDoc;
            rptDocuments.DataBind();
            UpdDocUpload.Update();
        }

        /// <summary>Read all document repeater rows into a DataTable.</summary>
        public DataTable CollectDocumentRows()
        {
            DataTable dtDoc = GetDocumentDataTable();

            foreach (RepeaterItem itm in rptDocuments.Items)
            {
                if (itm.ItemType != ListItemType.Item &&
                    itm.ItemType != ListItemType.AlternatingItem)
                    continue;

                HiddenField hdnDocId = (HiddenField)itm.FindControl("hdnDocId");
                RadComboBox drpDocType = (RadComboBox)itm.FindControl("drpDocumentType");
                HiddenField hdnFileName = (HiddenField)itm.FindControl("hdnFileName");
                HiddenField hdnFileNameSave = (HiddenField)itm.FindControl("hdnFileNameSave");

                dtDoc.Rows.Add(
                    Convert.ToInt32(string.IsNullOrEmpty(hdnDocId.Value) ? "0" : hdnDocId.Value),
                    drpDocType.SelectedValue == "" ? (object)DBNull.Value : Convert.ToInt32(drpDocType.SelectedValue),
                    hdnFileName.Value,
                    hdnFileNameSave.Value);
            }
            return dtDoc;
        }

        /// <summary>Handle async file upload for document rows.</summary>
        public void fuDocFileOnFileUploaded(object sender, FileUploadedEventArgs e)
        {
            RadAsyncUpload fuDocFile = (RadAsyncUpload)sender;
            fuDocFile.TargetFolder = "~/UploadedFiles";

            string prefix = "LD-";
            string savedName = prefix + e.File.GetNameWithoutExtension() + e.File.GetExtension();
            string originalName = e.File.GetName();

            e.File.SaveAs(Path.Combine(Server.MapPath(fuDocFile.TargetFolder), savedName));

            // Walk up to the RepeaterItem to update hidden fields & label
            Control parent = fuDocFile.Parent;
            while (parent != null && !(parent is RepeaterItem))
                parent = parent.Parent;

            if (parent is RepeaterItem item)
            {
                UpdatePanel updDocFile = (UpdatePanel)item.FindControl("UpdDocFile");
                HiddenField hdnFileName = (HiddenField)item.FindControl("hdnFileName");
                HiddenField hdnFileNameSave = (HiddenField)item.FindControl("hdnFileNameSave");
                Label lblFileName = (Label)item.FindControl("lblFileName");
                Button btnDocDownload = (Button)item.FindControl("btnDocDownload");

                hdnFileName.Value = originalName;
                hdnFileNameSave.Value = savedName;

                if (lblFileName != null)
                {
                    lblFileName.Text = originalName;
                    lblFileName.Visible = true;
                }
                if (btnDocDownload != null)
                    btnDocDownload.Visible = true;

                if (updDocFile != null)
                    updDocFile.Update();
            }

            // Backup copy
            try
            {
                DataTable dtgen = masterBAL.Edit_GeneralSettings();
                string backupPath = dtgen.Rows[0]["BackupDrivePath"].ToString() + "UploadedFiles";
                if (Directory.Exists(backupPath))
                {
                    File.Copy(
                        Path.Combine(Server.MapPath(fuDocFile.TargetFolder), savedName),
                        Path.Combine(backupPath, savedName),
                        false);
                }
            }
            catch { }
        }

        // ─────────────────────────────────────────────────────────────
        // Dropdown Change Events
        // ─────────────────────────────────────────────────────────────
        protected void drpStatusfilterOnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            fillgridList(1, Convert.ToInt32(drpPageSize.SelectedValue),
                hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
        }

        protected void drpSource_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (drpSource.SelectedValue == "0")
            {
                int val = systemUtilities.Form_Previlage_Validation(133, Convert.ToInt32(hdnUserId.Value));
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

        protected void drpJurisdiction_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e) { }

        protected void drpSegment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (drpSegment.SelectedValue == "0")
            {
                pnlSegment.Visible = true;
                UCSegment.PageLoad();
                updSegmentPanel.Update();
            }
        }

        protected void drpcity_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (drpCity.SelectedValue == "0")
            {
                pnlCity.Visible = true;
                UCCity.PageLoad();
                updCityPanel.Update();
            }
        }

        protected void drpPriority_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (drpPriority.SelectedValue == "0")
            {
                int val = systemUtilities.Form_Previlage_Validation(134, Convert.ToInt32(hdnUserId.Value));
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

        protected void drpFilterOnSelectedIndexChanged(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            string contrlName = sendercontrol.ID;

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

        // ─────────────────────────────────────────────────────────────
        // Add New Answer / Question
        // ─────────────────────────────────────────────────────────────
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

        // ─────────────────────────────────────────────────────────────
        // Save / Mail / Quotation
        // ─────────────────────────────────────────────────────────────
        protected void btnMailOnClick(object sender, EventArgs e)
        {
            int res = 0;
            if (hdnId.Value == "0")
            {
                res = SaveLead();
                fillgridList(Convert.ToInt32(lblPageNumber.Text), Convert.ToInt32(drpPageSize.SelectedValue),
                    hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
                Clear();
            }
            else if (hdnStatus.Value != "3")
            {
                res = SaveLead();
                fillgridList(Convert.ToInt32(lblPageNumber.Text), Convert.ToInt32(drpPageSize.SelectedValue),
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            UpdPanelAdd.Update();
        }

        protected void btnSaveOnClick(object sender, EventArgs e)
        {
            int res = SaveLead();
            if (res > 0)
            {
                fillgridList(Convert.ToInt32(lblPageNumber.Text), Convert.ToInt32(drpPageSize.SelectedValue),
                    hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                PanelAdd.Visible = false;
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

        /// <summary>Core save method. Returns lead Id on success, 0 on failure, -1 if Q&A missing.</summary>
        public int SaveLead()
        {
            // ── Q&A service table ──────────────────────────────────────
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
                        drpDepartment.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpDepartment.SelectedValue),
                        drpSerCategory.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpSerCategory.SelectedValue),
                        (int?)null, (int?)null, 0);
            }

            int? currentStatusVal = drpCurrentStatus.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpCurrentStatus.SelectedValue);
            int? maritalStatusVal = drpMaritalStatus.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpMaritalStatus.SelectedValue);

            int res = TransBal.InsertUpdateLeadCreation(
                Convert.ToInt32(hdnId.Value),
                txtcompany.Text,
                txtAddress.Text,
                txtMobileNumber.Text,
                txtEmailId.Text,
                Convert.ToInt32(hdnUserId.Value),
                drpEmployee.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpEmployee.SelectedValue),
                txtphone.Text,
                txtcompany.Text,
                txtResponse.Text,
                Convert.ToInt32(drpPriority.SelectedValue),
                drpSource.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpSource.SelectedValue),
                Followupdate.SelectedDate,
                ApprxClosingDate.SelectedDate,
                dtService,
                radFollowupTime.SelectedDate,
                drpJurisdiction.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpJurisdiction.SelectedValue),
                leadDate.SelectedDate,
                txtActivity.Text,
                txtCPDesig.Text,
                txtwebsite.Text,
                txtCampaign.Text,
                txtCountryCodeCN.Text,
                txtCountryCodeLPN.Text,
                drpCity.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpCity.SelectedValue),
                drpSegment.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpSegment.SelectedValue),
                // New fields
                txtLeadBrand.Text,
                txtPassportNo.Text,
                dpPassportIssueDate.SelectedDate,
                dpPassportExpiryDate.SelectedDate,
                currentStatusVal,
                dpDOB.SelectedDate,
                txtNationality.Text,
                maritalStatusVal,
                txtMotherName.Text
            );

            // ── Save documents after lead is saved ─────────────────────
            if (res > 0)
            {
                DataTable dtDocAll = CollectDocumentRows();
                DataTable dtDocToSave = GetDocumentDataTable();

                foreach (DataRow row in dtDocAll.Rows)
                {
                    // Only save rows that have a file attached
                    if (row["FilenameSave"].ToString() != "")
                        dtDocToSave.ImportRow(row);
                }

                if (dtDocToSave.Rows.Count > 0)
                {
                    try
                    {
                        TransBal.SaveLeadDocuments(res, dtDocToSave,
                            Convert.ToInt32(hdnUserId.Value));
                    }
                    catch { }
                }
            }

            return res;
        }

        protected void btnCreateQutn_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Transactions/Quotation.aspx?LeadId=" + hdnId.Value);
        }

        protected void btnDeleteOnClick(object sender, EventArgs e)
        {
            int res = TransBal.DeleteLeadCreation(
                Convert.ToInt32(hdnId.Value), Convert.ToInt32(hdnUserId.Value));

            if (res == 1)
            {
                fillgridList(Convert.ToInt32(lblPageNumber.Text), Convert.ToInt32(drpPageSize.SelectedValue),
                    hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
                Clear();
                lbl_msg.Text = "Deleted Successfully !..";
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

        // ─────────────────────────────────────────────────────────────
        // Clear
        // ─────────────────────────────────────────────────────────────
        public void Clear()
        {
            hdnId.Value = "0";
            txtName.Text = "";
            txtAddress.Text = txtResponse.Text = "";
            txtEmailId.Text = txtcompany.Text = "";
            txtCompanyName.Text = "";
            txtMobileNumber.Text = txtphone.Text = "";
            drpEmployee.ClearSelection(); drpEmployee.Text = "";
            drpPriority.ClearSelection(); drpPriority.Text = "";
            drpSource.ClearSelection(); drpSource.Text = "";
            drpJurisdiction.ClearSelection();
            drpJurisdiction.Text = txtActivity.Text = txtCPDesig.Text = txtwebsite.Text = "";
            Followupdate.DbSelectedDate = ApprxClosingDate.DbSelectedDate = "";
            radFollowupTime.SelectedDate = null;
            Followupdate.MinDate = DateTime.Now.AddDays(-365);
            leadDate.SelectedDate = DateTime.Now;
            txtCountryCodeCN.Text = "+971";
            txtCampaign.Text = txtCountryCodeLPN.Text = "";
            drpCity.ClearSelection(); drpCity.Text = "";
            drpSegment.ClearSelection(); drpSegment.Text = "";

            // New fields
            txtLeadBrand.Text = "";
            txtPassportNo.Text = "";
            dpPassportIssueDate.DbSelectedDate = "";
            dpPassportExpiryDate.DbSelectedDate = "";
            drpCurrentStatus.SelectedIndex = 0;
            dpDOB.DbSelectedDate = "";
            txtNationality.Text = "";
            drpMaritalStatus.SelectedIndex = 0;
            txtMotherName.Text = "";

            // Q&A
            DataTable dtService = new DataTable();
            dtService.Columns.Add("Id", typeof(int));
            dtService.Columns.Add("DepartmentId", typeof(int));
            dtService.Columns.Add("CategoryId", typeof(int));
            dtService.Rows.Add(0, null);
            rptservice.DataSource = dtService;
            rptservice.DataBind();

            // Documents – reset to one empty row
            DataTable dtDoc = GetDocumentDataTable();
            dtDoc.Rows.Add(0, DBNull.Value, "", "");
            rptDocuments.DataSource = dtDoc;
            rptDocuments.DataBind();

            btnDelete.Visible = false;
            btnHistory.Visible = false;
            btnCreateQutn.Visible = false;
            btnSave.Visible = hdnAdd.Value == "0" ? false : true;
            btnMail.Visible = hdnSendMail.Value == "0" ? false : true;

            Get_Code();
            UpdPanelAddInner.Update();
        }

        // ─────────────────────────────────────────────────────────────
        // Excel Upload
        // ─────────────────────────────────────────────────────────────
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
            ContentTable.Columns.Add("Website", typeof(string));
            ContentTable.Columns.Add("Activity", typeof(string));
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
                Label lblCampaign = (Label)itm.FindControl("lblCampaign");
                Label lblContactPersonName = (Label)itm.FindControl("lblContactPersonName");
                Label lblContactPersondesignation = (Label)itm.FindControl("lblContactPersondesignation");
                Label lblContactNumber = (Label)itm.FindControl("lblContactNumber");
                Label lblCountryCodeContactNumber = (Label)itm.FindControl("lblCountryCodeContactNumber");
                Label lblLandPhoneNoCountryCode = (Label)itm.FindControl("lblLandPhoneNoCountryCode");
                Label lblLandPhoneNo = (Label)itm.FindControl("lblLandPhoneNo");
                Label lblEmail = (Label)itm.FindControl("lblEmail");
                Label lblWebsite = (Label)itm.FindControl("lblWebsite");
                Label lblActivity = (Label)itm.FindControl("lblActivity");
                Label lblActivityDescription = (Label)itm.FindControl("lblActivityDescription");
                Label lblCustomerResponse = (Label)itm.FindControl("lblCustomerResponse");
                HiddenField hdnLeadSourceId = (HiddenField)itm.FindControl("hdnLeadSourceId");
                HiddenField hdnAssignedEmployeeId = (HiddenField)itm.FindControl("hdnAssignedEmployeeId");
                HiddenField hdnPriorityId = (HiddenField)itm.FindControl("hdnPriorityId");
                HiddenField hdnCityId = (HiddenField)itm.FindControl("hdnCityId");
                HiddenField hdnSegmentId = (HiddenField)itm.FindControl("hdnSegmentId");

                if (lbldate.Text != "" && lblCompanyName.Text != "" &&
                    lblCampaign.Text != "" && lblContactPersonName.Text != "")
                    ContentTable.Rows.Add(
                        Convert.ToDateTime(lbldate.Text),
                        lblCampaign.Text, lblCompanyName.Text, lblContactPersonName.Text,
                        lblCountryCodeContactNumber.Text, lblContactNumber.Text,
                        lblContactPersondesignation.Text,
                        lblLandPhoneNoCountryCode.Text, lblLandPhoneNo.Text,
                        lblEmail.Text, lblWebsite.Text, lblActivity.Text,
                        lblActivityDescription.Text, lblCustomerResponse.Text,
                        hdnLeadSourceId.Value == "" ? (int?)null : Convert.ToInt32(hdnLeadSourceId.Value),
                        hdnAssignedEmployeeId.Value == "" ? (int?)null : Convert.ToInt32(hdnAssignedEmployeeId.Value),
                        hdnPriorityId.Value == "" ? (int?)null : Convert.ToInt32(hdnPriorityId.Value),
                        hdnSegmentId.Value == "" ? (int?)null : Convert.ToInt32(hdnSegmentId.Value),
                        hdnCityId.Value == "" ? (int?)null : Convert.ToInt32(hdnCityId.Value));
            }

            int res = TransBal.InsertLeadList(ContentTable, Convert.ToInt32(hdnUserId.Value));
            if (res > 0)
            {
                fillgridList(Convert.ToInt32(lblPageNumber.Text), Convert.ToInt32(drpPageSize.SelectedValue),
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
                string full_name = Server.MapPath("~/UploadedFiles/" + fil_name);
                Response.ContentType = "APPLICATION/OCTET-STREAM";
                Response.AppendHeader("Content-Disposition", "Attachment; Filename=\"" + fil_name + "\"");
                System.IO.FileInfo Dfile = new System.IO.FileInfo(Server.MapPath("~/UploadedFiles/" + fil_name));
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
                Prefix + e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();
            e.File.SaveAs(Path.Combine(Server.MapPath(fu_DocUpload.TargetFolder), files_name));

            try
            {
                DataTable dtgen = masterBAL.Edit_GeneralSettings();
                File.Copy(
                    Path.Combine(Server.MapPath(fu_DocUpload.TargetFolder), files_name),
                    Path.Combine(dtgen.Rows[0]["BackupDrivePath"].ToString() + "UploadedFiles", files_name),
                    false);
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
            ContentTable.Columns.Add("Website", typeof(string));
            ContentTable.Columns.Add("AssignedEmployee", typeof(string));
            ContentTable.Columns.Add("Priority", typeof(string));
            ContentTable.Columns.Add("Activity", typeof(string));
            ContentTable.Columns.Add("ActivityDescription", typeof(string));
            ContentTable.Columns.Add("CustomerResponse", typeof(string));

            rptuploaddetail.DataSource = null;
            rptuploaddetail.DataBind();

            if (hdnleadFile.Value != "")
            {
                string connString = "";
                string filepath = Path.Combine(Server.MapPath("~/UploadedFiles"), hdnleadFile.Value);
                if (hdnleadfileExtension.Value == ".xls")
                    connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath
                        + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1;TypeGuessRows=0\"";

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
                                ContentTable.Rows.Add(
                                    dr["Date"].ToString().Trim() == string.Empty
                                        ? (DateTime?)null : Convert.ToDateTime(dr["Date"].ToString().Trim()),
                                    dr["Campaign"].ToString().Trim(),
                                    dr["CompanyName"].ToString().Trim(),
                                    dr["ContactPersonName"].ToString().Trim(),
                                    dr["Segment"].ToString().Trim(),
                                    dr["LeadSource"].ToString().Trim(),
                                    dr["City"].ToString().Trim(),
                                    dr["CountryCodeContactNumber"].ToString().Trim(),
                                    dr["ContactNumber"].ToString().Trim(),
                                    dr["ContactPersondesignation"].ToString().Trim(),
                                    dr["LandPhoneNoCountryCode"].ToString().Trim(),
                                    dr["LandPhoneNo"].ToString().Trim(),
                                    dr["Email"].ToString().Trim(),
                                    dr["Website"].ToString().Trim(),
                                    dr["AssignedEmployee"].ToString().Trim(),
                                    dr["Priority"].ToString().Trim(),
                                    dr["Activity"].ToString().Trim(),
                                    dr["ActivityDescription"].ToString().Trim(),
                                    dr["CustomerResponse"].ToString().Trim());
                            }
                            catch (Exception ex1)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "showalert",
                                    "alert('" + ex1.Message + "');", true);
                            }
                        }
                    }
                    dr.Close();
                    OledbConn.Close();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert",
                        "alert('Please upload the correct file format');", true);
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

        // ─────────────────────────────────────────────────────────────
        // History
        // ─────────────────────────────────────────────────────────────
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
                "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no," +
                "location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
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
            DataTable dt = TransBal.ListLeadHistory(Convert.ToInt32(hdnId.Value), page_number, page_size);
            rptHistory.DataSource = dt;
            rptHistory.DataBind();

            if (dt.Rows.Count > 0)
            {
                lbl_page_info1.Text = "Showing Results " + dt.Rows[0]["StartNumber"].ToString()
                    + " - " + dt.Rows[dt.Rows.Count - 1]["RowNum"].ToString()
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

        #region History Navigation
        protected void btn_first1_OnClick(object sender, EventArgs e) { grid_fill_his(1, Convert.ToInt32(drp_count1.SelectedValue)); }
        protected void btn_prev1_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number1.Text) > 1)
                grid_fill_his(Convert.ToInt32(lbl_page_number1.Text) - 1, Convert.ToInt32(drp_count1.SelectedValue));
        }
        protected void btn_next1_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number1.Text) < Convert.ToInt32(hdn_last_page1.Value))
                grid_fill_his(Convert.ToInt32(lbl_page_number1.Text) + 1, Convert.ToInt32(drp_count1.SelectedValue));
        }
        protected void btn_last1_OnClick(object sender, EventArgs e) { grid_fill_his(Convert.ToInt32(hdn_last_page1.Value), Convert.ToInt32(drp_count1.SelectedValue)); }
        protected void drp_count1_OnSelectedIndexChanged(object sender, EventArgs e) { grid_fill_his(1, Convert.ToInt32(drp_count1.SelectedValue)); }
        #endregion

        // ─────────────────────────────────────────────────────────────
        // List Navigation
        // ─────────────────────────────────────────────────────────────
        protected void txtSearchOnTextChanged(object sender, EventArgs e)
        {
            fillgridList(1, Convert.ToInt32(drpPageSize.SelectedValue),
                txtSearch.Text, hdnOrderByColumnName.Value, hdnOrderBy.Value);
        }
        protected void btnFirstOnClick(object sender, EventArgs e) { fillgridList(1, Convert.ToInt32(drpPageSize.SelectedValue), hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value); }
        protected void btnPreviousOnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lblPageNumber.Text) > 1)
                fillgridList(Convert.ToInt32(lblPageNumber.Text) - 1, Convert.ToInt32(drpPageSize.SelectedValue), hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
        }
        protected void btnNextOnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lblPageNumber.Text) < Convert.ToInt32(hdnLastPage.Value))
                fillgridList(Convert.ToInt32(lblPageNumber.Text) + 1, Convert.ToInt32(drpPageSize.SelectedValue), hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
        }
        protected void btnLastOnClick(object sender, EventArgs e) { fillgridList(Convert.ToInt32(hdnLastPage.Value), Convert.ToInt32(drpPageSize.SelectedValue), hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value); }
        protected void drpPageSizeOnSelectedIndexChanged(object sender, EventArgs e) { fillgridList(1, Convert.ToInt32(drpPageSize.SelectedValue), hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value); }

        // ─────────────────────────────────────────────────────────────
        // Privilege Checks
        // ─────────────────────────────────────────────────────────────
        public void CheckPrivilege()
        {
            try
            {
                if (hdnUserId.Value != null)
                {
                    int val = systemUtilities.Form_Previlage_Validation(138, Convert.ToInt32(hdnUserId.Value));
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
                    DataTable dtSubMenuAction = systemUtilities.Action_Previlage_Validation(138, Convert.ToInt32(hdnUserId.Value));
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