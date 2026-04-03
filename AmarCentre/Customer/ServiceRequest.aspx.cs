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
    public partial class ServiceRequest : System.Web.UI.Page
    {
        Transaction_Bal obj_trans = new Transaction_Bal();
        Master_Bal obj_mas = new Master_Bal();
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
                lbl_User_name.Text = Session["User_Name"].ToString();
                Session["User_Name"] = lbl_User_name.Text;

                previlage_check();
                Clear();
                grid_fill(1, 10, "", "", "");
                fill_Templates();
            }
        }

        public void fill_Templates()
        {
            drpTemplates.Items.Clear();
            DataTable dt = obj_trans.GetTemplates();
            drpTemplates.DataSource = dt;
            drpTemplates.DataTextField = "Text";
            drpTemplates.DataValueField = "Value";
            drpTemplates.DataBind();
            drpTemplates.Text = "";
        }

        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_trans.Get_ListServiceRequest(page_number, page_size, filter, Convert.ToInt32(hdn_user_id.Value));
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

        protected void rpt_list_OnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdn_id");
            if (e.CommandName == "Edit")
            {
                Clear();
                pnl_add.Visible = true;

                DataSet ds = obj_trans.EditServiceRequest(Convert.ToInt32(hdn_rpt_id.Value));
                DataTable dt1 = ds.Tables[0];
                DataTable dt_ser = ds.Tables[1];
                DataTable dtdoc = ds.Tables[2];

                hdn_id.Value = dt1.Rows[0]["Id"].ToString();
                lbl_Code.Text = dt1.Rows[0]["Code"].ToString();
                job_date.DbSelectedDate = dt1.Rows[0]["Dated"].ToString();
                hdnRequestStatus.Value = dt1.Rows[0]["StatusId"].ToString();
                drpTemplates.SelectedValue = dt1.Rows[0]["TemplateId"].ToString();
                txtApplicant.Text= dt1.Rows[0]["Applicant"].ToString();
                rpt_Item_list.DataSource = dt_ser;
                rpt_Item_list.DataBind();
                lblstatus.Text = "Status : " + dt1.Rows[0]["StatusName"].ToString();

                rptDocumentOut.DataSource = dtdoc;
                rptDocumentOut.DataBind();

                btn_cancel.Visible = btn_save.Visible = false;
                btnhistory.Visible = true;

                //1-requested 2-processed 3-cancelled 4-Actionrequired 5-completed
                if (hdnRequestStatus.Value == "1")
                {
                    btn_cancel.Visible = btn_save.Visible = true;
                    lblreject.Text = dt1.Rows[0]["RejectRemark"].ToString();
                }
               
                Upd_Add_Panel.Update();
            }
        }

        protected void drpTemplatesOnSelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtgen = obj_mas.Edit_GeneralSettings();

            int hdnSerPriceWTax = Convert.ToInt32(dtgen.Rows[0]["ServicePriceWithTax"].ToString());
            int hdnTaxAppliedWithDiscount = Convert.ToInt32(dtgen.Rows[0]["TaxAppliedWithDiscount"].ToString());
            int hdnDefaultInvoiceType = Convert.ToInt32(dtgen.Rows[0]["InvoiceType"].ToString());

            DataTable dtTemplates = new DataTable();
            dtTemplates.Columns.Add("TemplatesId", typeof(int));
            dtTemplates.Rows.Add(Convert.ToInt32(drpTemplates.SelectedValue));

            DataTable dt = obj_trans.GetServiceDetailsTemplate_invrecpt(dtTemplates, 1, hdnSerPriceWTax, 0,
                hdnTaxAppliedWithDiscount, hdnDefaultInvoiceType, (int?)null);

            rpt_Item_list.DataSource = dt;
            rpt_Item_list.DataBind();
            Upd_Item_Panel.Update();
        }

        public DataSet fill_Detail()
        {
            DataTable dt_ser = new DataTable();
            dt_ser.Columns.Add("D_id", typeof(int));
            dt_ser.Columns.Add("DepartmentId", typeof(int));
            dt_ser.Columns.Add("ServiceId", typeof(int));
            dt_ser.Columns.Add("ApplicantName", typeof(string));

            DataTable dtdoc = new DataTable();
            dtdoc.Columns.Add("RequestDetailId", typeof(int));
            dtdoc.Columns.Add("FileNames", typeof(string));
            dtdoc.Columns.Add("FileNamesSave", typeof(string));

            foreach (RepeaterItem itm in rpt_Item_list.Items)
            {
                HiddenField hdnDId = (HiddenField)itm.FindControl("hdnDId");
                RadComboBox drpDepartment = (RadComboBox)itm.FindControl("drpDepartment");
                RadComboBox drpService = (RadComboBox)itm.FindControl("drpService");
                TextBox txt_Applname = (TextBox)itm.FindControl("txt_Applname");
                HiddenField hdnServiceStatus = (HiddenField)itm.FindControl("hdnServiceStatus");

                if (drpService.SelectedValue != "")
                {
                    dt_ser.Rows.Add(Convert.ToInt32(hdnDId.Value), drpDepartment.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpDepartment.SelectedValue),
                        drpService.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpService.SelectedValue), txtApplicant.Text); // txt_Applname.Text

                    Repeater rptDocument = (Repeater)itm.FindControl("rptDocument");
                    foreach (RepeaterItem itmdoc in rptDocument.Items)
                    {
                        HiddenField hdnFilename = (HiddenField)itmdoc.FindControl("hdnFilename");
                        HiddenField hdnFilenameSave = (HiddenField)itmdoc.FindControl("hdnFilenameSave");

                        dtdoc.Rows.Add(Convert.ToInt32(hdnDId.Value),  hdnFilename.Value, hdnFilenameSave.Value);
                    }
                }
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(dt_ser);
            ds.Tables.Add(dtdoc);

            return ds;
        }

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            int res = 0;
            DataSet ds = fill_Detail();
            DataTable dtdoc = new DataTable();
            dtdoc.Columns.Add("FileNames", typeof(string));
            dtdoc.Columns.Add("FileNamesSave", typeof(string));

            foreach (RepeaterItem itmdoc in rptDocumentOut.Items)
            {
                HiddenField hdnFilenameOut = (HiddenField)itmdoc.FindControl("hdnFilenameOut");
                HiddenField hdnFilenameSaveOut = (HiddenField)itmdoc.FindControl("hdnFilenameSaveOut");

                dtdoc.Rows.Add(hdnFilenameOut.Value, hdnFilenameSaveOut.Value);
            }
            if(hdnFilenameSaveRDOut.Value!="")
                dtdoc.Rows.Add(hdnFilenameRDOut.Value, hdnFilenameSaveRDOut.Value);

            if (ds.Tables[0].Rows.Count > 0)
            {
                res = obj_trans.Insert_UpdateServiceRequest(Convert.ToInt32(hdn_id.Value), job_date.SelectedDate, Convert.ToInt32(hdn_user_id.Value),
                (int?)null,  ds.Tables[0], ds.Tables[1], lblreject.Text,Convert.ToInt32(drpTemplates.SelectedValue), dtdoc,txtApplicant.Text);
                if (res > 0)
                {
                    grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                    Clear();
                    lbl_msgin.Text = "Saved Successfully !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                }
                else
                {
                    lbl_msgin.Text = "Sorry Failed to Process Your Request !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                }
            }
            else
            {
                lbl_msgin.Text = "Add Service to Continue !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
           
            Upd_Add_PanelInner.Update();
        }

        protected void btn_DeleteOnClick(object sender, EventArgs e)
        {
            int res = obj_trans.DeleteRequest(Convert.ToInt32(hdn_id.Value));
            if (res > 0)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                
                lbl_msgin.Text = "Deleted Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                pnl_add.Visible = false;
                Upd_Add_Panel.Update();
            }
            else
            {
                lbl_msgin.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
        }

        protected void radDoc_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            radDocOut.TargetFolder = "~/UploadedFiles";
            DataTable dt = obj_common.Get_File_Code("SreqDoc");

            string files_name = dt.Rows[0][0].ToString() + e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();
            e.File.SaveAs(Path.Combine(Server.MapPath(radDocOut.TargetFolder), files_name));

            hdnFilenameRDOut.Value = e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();
            hdnFilenameSaveRDOut.Value = files_name;

            updFileuploadOut.Update();
        }

        protected void btn_newdocOut_Click(object sender, EventArgs e)
        {
            DataTable dtdoc = new DataTable();
            dtdoc.Columns.Add("FileNames", typeof(string));
            dtdoc.Columns.Add("FileNamesSave", typeof(string));

            foreach (RepeaterItem itmdoc in rptDocumentOut.Items)
            {
                HiddenField hdnFilenameOut = (HiddenField)itmdoc.FindControl("hdnFilenameOut");
                HiddenField hdnFilenameSaveOut = (HiddenField)itmdoc.FindControl("hdnFilenameSaveOut");

                dtdoc.Rows.Add(hdnFilenameOut.Value, hdnFilenameSaveOut.Value);
            }
            if (hdnFilenameRDOut.Value != "")
                dtdoc.Rows.Add(hdnFilenameRDOut.Value, hdnFilenameSaveRDOut.Value);

            rptDocumentOut.DataSource = dtdoc;
            rptDocumentOut.DataBind();

            hdnFilenameRDOut.Value = hdnFilenameSaveRDOut.Value = "";

            updFileuploadOut.Update();
        }

        protected void rptDocumentOut_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Download")
            {
                HiddenField hdnFilenameSaveOut = (HiddenField)e.Item.FindControl("hdnFilenameSaveOut");
                HiddenField hdnFilenameOut = (HiddenField)e.Item.FindControl("hdnFilenameOut");

                try
                {
                    if (hdnFilenameSaveOut.Value != "")
                    {
                        string fil_name = hdnFilenameSaveOut.Value;
                        string full_name = Server.MapPath("~/UploadedFiles/" + fil_name);
                        Response.ContentType = "APPLICATION/OCTET-STREAM";
                        String Header = "Attachment; Filename=\"" + hdnFilenameOut.Value + "\"";
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
            else if (e.CommandName == "DeleteFile")
            {
                DataTable dtdoc = new DataTable();
                dtdoc.Columns.Add("FileNames", typeof(string));
                dtdoc.Columns.Add("FileNamesSave", typeof(string));

                foreach (RepeaterItem itmdoc in rptDocumentOut.Items)
                {
                    HiddenField hdnFilenameOut = (HiddenField)itmdoc.FindControl("hdnFilenameOut");
                    HiddenField hdnFilenameSaveOut = (HiddenField)itmdoc.FindControl("hdnFilenameSaveOut");

                    dtdoc.Rows.Add(hdnFilenameOut.Value, hdnFilenameSaveOut.Value);
                }

                dtdoc.Rows.RemoveAt(e.Item.ItemIndex);

                rptDocumentOut.DataSource = dtdoc;
                rptDocumentOut.DataBind();

                updFileuploadOut.Update();
            }
        }

        #region Service list  
        //not using now
        protected void rptitemlistDatabound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField hdnDepartmentId = (HiddenField)e.Item.FindControl("hdnDepartmentId");
            RadComboBox drpDepartment = (RadComboBox)e.Item.FindControl("drpDepartment");
            drpDepartment.Items.Clear();
            drpDepartment.DataSource = obj_mas.Drp_Department();
            drpDepartment.DataValueField = "Value";
            drpDepartment.DataTextField = "Text";
            drpDepartment.DataBind();
            drpDepartment.SelectedValue = hdnDepartmentId.Value;

            HiddenField hdnServiceId = (HiddenField)e.Item.FindControl("hdnServiceId");
            RadComboBox drpService = (RadComboBox)e.Item.FindControl("drpService");
            drpService.Items.Clear();
            drpService.DataSource = obj_mas.DrpServicebyDepartment(hdnDepartmentId.Value == "" ? 0 : Convert.ToInt32(hdnDepartmentId.Value));
            drpService.DataValueField = "Id";
            drpService.DataTextField = "Name";
            drpService.DataBind();
            drpService.SelectedValue = hdnServiceId.Value;
        }

        protected void drpService_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            RadComboBox drp = (RadComboBox)sender;
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            HiddenField hdnDepartmentId = (HiddenField)itemrp.FindControl("hdnDepartmentId");
            RadComboBox drpDepartment = (RadComboBox)itemrp.FindControl("drpDepartment");
            UpdatePanel UpdDepartment = (UpdatePanel)itemrp.FindControl("UpdDepartment");
            if (drp.SelectedValue != "")
            {
                DataSet dtAccount = obj_mas.Edit_Service(Convert.ToInt32(drp.SelectedValue));
                if (drpDepartment.SelectedValue == "")
                {
                    drpDepartment.SelectedValue = dtAccount.Tables[0].Rows[0]["DepartmentId"].ToString();
                    hdnDepartmentId.Value = dtAccount.Tables[0].Rows[0]["DepartmentId"].ToString();
                }
            }
            UpdDepartment.Update();
        }

        protected void drpDepartmentOnSelectedIndexChanged(object sender, EventArgs e)
        {
            RadComboBox drp = (RadComboBox)sender;
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            RadComboBox drpService = (RadComboBox)itemrp.FindControl("drpService");
            UpdatePanel UpdService = (UpdatePanel)itemrp.FindControl("UpdService");
            HiddenField hdnServiceId = (HiddenField)itemrp.FindControl("hdnServiceId");

            drpService.DataSource = obj_mas.DrpServicebyDepartment(drp.SelectedValue == "" ? 0 : Convert.ToInt32(drp.SelectedValue));
            drpService.DataValueField = "Id";
            drpService.DataTextField = "Name";
            drpService.DataBind();
            hdnServiceId.Value = "";
            drpService.ClearSelection();
            drpService.Text = "";
            UpdService.Update();
        }

        public void RadAsyncUpload1_OnFileUploaded(object sender, FileUploadedEventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            RadAsyncUpload RadAsyncUpload1 = (RadAsyncUpload)itemrp.FindControl("RadAsyncUpload1");
            UpdatePanel updFileupload = (UpdatePanel)itemrp.FindControl("updFileupload");
            HiddenField hdnFilenameRD = (HiddenField)itemrp.FindControl("hdnFilenameRD");
            HiddenField hdnFilenameSaveRD = (HiddenField)itemrp.FindControl("hdnFilenameSaveRD");

            RadAsyncUpload1.TargetFolder = "~/UploadedFiles";
            DataTable dt = obj_common.Get_File_Code("SreqDoc");

            string files_name = dt.Rows[0][0].ToString()+ e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();
            e.File.SaveAs(Path.Combine(Server.MapPath(RadAsyncUpload1.TargetFolder), files_name));

            hdnFilenameRD.Value = e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();
            hdnFilenameSaveRD.Value = files_name;

            updFileupload.Update();
        }

        protected void rptDocument_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Download")
            {
                HiddenField hdnFilenameSave = (HiddenField)e.Item.FindControl("hdnFilenameSave");
                HiddenField hdnFilename = (HiddenField)e.Item.FindControl("hdnFilename");

                try
                {
                    if (hdnFilenameSave.Value != "")
                    {
                        string fil_name = hdnFilenameSave.Value;
                        string full_name = Server.MapPath("~/UploadedFiles/" + fil_name);
                        Response.ContentType = "APPLICATION/OCTET-STREAM";
                        String Header = "Attachment; Filename=\"" + hdnFilename.Value + "\"";
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
            else if (e.CommandName == "DeleteFile")
            {
                Control sendercontrol = (Control)source;
                RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

                Repeater rptDocument = (Repeater)itemrp.FindControl("rptDocument");
                UpdatePanel updFileupload = (UpdatePanel)itemrp.FindControl("updFileupload");

                DataTable dtdoc = new DataTable();
                dtdoc.Columns.Add("FileNames", typeof(string));
                dtdoc.Columns.Add("FileNamesSave", typeof(string));

                foreach (RepeaterItem itmdoc in rptDocument.Items)
                {
                    HiddenField hdnFilename = (HiddenField)itmdoc.FindControl("hdnFilename");
                    HiddenField hdnFilenameSave = (HiddenField)itmdoc.FindControl("hdnFilenameSave");

                    dtdoc.Rows.Add(hdnFilename.Value, hdnFilenameSave.Value);
                }

                dtdoc.Rows.RemoveAt(e.Item.ItemIndex);

                rptDocument.DataSource = dtdoc;
                rptDocument.DataBind();

                updFileupload.Update();
            }
        }

        protected void rpt_Item_list_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Document")
            {
                Panel pnlDocument = (Panel)e.Item.FindControl("pnlDocument");
                UpdatePanel UpdDocument = (UpdatePanel)e.Item.FindControl("UpdDocument");
                pnlDocument.Visible = true;
                UpdDocument.Update();
            }
            else if (e.CommandName == "CloseDocument")
            {
                Panel pnlDocument = (Panel)e.Item.FindControl("pnlDocument");
                UpdatePanel UpdDocument = (UpdatePanel)e.Item.FindControl("UpdDocument");

                pnlDocument.Visible = false;
                UpdDocument.Update();
            }
            else if (e.CommandName == "adddocIn")
            {
                Repeater rptDocument = (Repeater)e.Item.FindControl("rptDocument");
                UpdatePanel updFileupload = (UpdatePanel)e.Item.FindControl("updFileupload");
                HiddenField hdnFilenameRD = (HiddenField)e.Item.FindControl("hdnFilenameRD");
                HiddenField hdnFilenameSaveRD = (HiddenField)e.Item.FindControl("hdnFilenameSaveRD");

                DataTable dtdoc = new DataTable();
                dtdoc.Columns.Add("FileNames", typeof(string));
                dtdoc.Columns.Add("FileNamesSave", typeof(string));

                foreach (RepeaterItem itmdoc in rptDocument.Items)
                {
                    HiddenField hdnFilename = (HiddenField)itmdoc.FindControl("hdnFilename");
                    HiddenField hdnFilenameSave = (HiddenField)itmdoc.FindControl("hdnFilenameSave");

                    dtdoc.Rows.Add(hdnFilename.Value, hdnFilenameSave.Value);
                }
                if (hdnFilenameRD.Value != "")
                    dtdoc.Rows.Add(hdnFilenameRD.Value, hdnFilenameSaveRD.Value);

                rptDocument.DataSource = dtdoc;
                rptDocument.DataBind();

                updFileupload.Update();
            }
            else if (e.CommandName == "Add" || e.CommandName == "Delete")
            {
                DataTable dt_ser = new DataTable();
                dt_ser.Columns.Add("D_id", typeof(int));
                dt_ser.Columns.Add("ServiceId", typeof(int));
                dt_ser.Columns.Add("DepartmentId", typeof(int));
                dt_ser.Columns.Add("ApplicantName", typeof(string));

                DataTable dtdoc = new DataTable();
                dtdoc.Columns.Add("RequestDetailId", typeof(int));
                dtdoc.Columns.Add("FileNames", typeof(string));
                dtdoc.Columns.Add("FileNamesSave", typeof(string));

                foreach (RepeaterItem itm in rpt_Item_list.Items)
                {
                    HiddenField hdnDId = (HiddenField)itm.FindControl("hdnDId");
                    RadComboBox drpDepartment = (RadComboBox)itm.FindControl("drpDepartment");
                    RadComboBox drpService = (RadComboBox)itm.FindControl("drpService");
                    TextBox txt_Applname = (TextBox)itm.FindControl("txt_Applname");

                    if (drpService.SelectedValue != "")
                    {
                        dt_ser.Rows.Add(Convert.ToInt32(hdnDId.Value), Convert.ToInt32(drpService.SelectedValue), drpDepartment.SelectedValue == "" ?
                        (int?)null : Convert.ToInt32(drpDepartment.SelectedValue), txt_Applname.Text);

                        Repeater rptDocument = (Repeater)itm.FindControl("rptDocument");
                        foreach (RepeaterItem itmdoc in rptDocument.Items)
                        {
                            HiddenField hdnFilename = (HiddenField)itmdoc.FindControl("hdnFilename");
                            HiddenField hdnFilenameSave = (HiddenField)itmdoc.FindControl("hdnFilenameSave");

                            dtdoc.Rows.Add(Convert.ToInt32(hdnDId.Value), hdnFilename.Value, hdnFilenameSave.Value);
                        }
                    }
                }

                if (e.CommandName == "Add")
                    dt_ser.Rows.Add(Convert.ToInt32(hdnDetailIndexId.Value) - 1, null, null);

                else if (e.CommandName == "Delete")
                {
                    dt_ser.Rows.RemoveAt(e.Item.ItemIndex);
                    if (dt_ser.Rows.Count == 0)
                        dt_ser.Rows.Add(Convert.ToInt32(hdnDetailIndexId.Value) - 1, null, null);
                }

                rpt_Item_list.DataSource = dt_ser;
                rpt_Item_list.DataBind();

                foreach (RepeaterItem itm in rpt_Item_list.Items)
                {
                    HiddenField hdnDId = (HiddenField)itm.FindControl("hdnDId");
                    Repeater rptDocument = (Repeater)itm.FindControl("rptDocument");

                    DataTable dtdocin = dtdoc.Clone();
                    foreach (DataRow rin in dtdoc.Rows)
                    {
                        if (rin["RequestDetailId"].ToString() == hdnDId.Value)
                            dtdocin.ImportRow(rin);
                    }
                    rptDocument.DataSource = dtdocin;
                    rptDocument.DataBind();
                }
                Upd_Item_Panel.Update();
            }
        }

        #endregion

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

        public void Clear()
        {
            Session["User_Id"] = hdn_user_id.Value;
            hdn_id.Value = "0";
            job_date.DbSelectedDate = DateTime.Now;
            hdnDetailIndexId.Value = "-1";
            lblreject.Text =lblstatus.Text= "";
            btnhistory.Visible = false;
            drpTemplates.ClearSelection();
            drpTemplates.Text =txtApplicant.Text= "";

            DataTable dt_ser = new DataTable();
            dt_ser.Columns.Add("D_id", typeof(int));
            dt_ser.Columns.Add("ServiceId", typeof(int));
            dt_ser.Columns.Add("DepartmentId", typeof(int));
            dt_ser.Columns.Add("ApplicantName", typeof(string));

            dt_ser.Rows.Add(-1, null, null, "");

            rpt_Item_list.DataSource = dt_ser;
            rpt_Item_list.DataBind();

            rptDocumentOut.DataSource = null;
            rptDocumentOut.DataBind();

            btn_cancel.Visible = false;
            btn_save.Visible = true;

            Get_Code();
            Upd_Add_PanelInner.Update();
        }

        public void Get_Code()
        {
            DataTable dt = obj_common.Get_CustomerPage("CustomerRequest");
            if (dt.Rows.Count > 0)
                lbl_Code.Text = dt.Rows[0][0].ToString();
        }

        #region History

        protected void btnhistory_OnClick(object sender, EventArgs e)
        {
            grid_fill_his();
            pnlhistry.Visible = true;
            Updhistory.Update();
        }

        protected void btn_histry_Close_OnClick(object sender, EventArgs e)
        {
            pnlhistry.Visible = false;
            Updhistory.Update();
        }

        public void grid_fill_his()
        {
            DataSet ds = obj_trans.listRequestHistry(Convert.ToInt32(hdn_id.Value));
            DataTable dt = ds.Tables[0];

            rpt_His.DataSource = dt;
            rpt_His.DataBind();

            Updhistory.Update();
        }

        #endregion

        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.CustomerForm_Previlage_Validation(1, Convert.ToInt32(hdn_user_id.Value));
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