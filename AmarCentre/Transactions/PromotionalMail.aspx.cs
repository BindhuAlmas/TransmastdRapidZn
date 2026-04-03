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
using System.Threading;
using System.Web.Configuration;

namespace AmarCentre.Transaction
{
    public partial class PromotionalMail : System.Web.UI.Page
    {
        Master_Bal obj_master = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
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
                RadCreation();
                fillgridList(1, 10, "", "", "");
                FillDropdown();
            }
        }

        public void FillDropdown()
        {
            drpTemplate.DataSource = TransBal.DrpMailTemplate();
            drpTemplate.DataValueField = "Value";
            drpTemplate.DataTextField = "Text";
            drpTemplate.DataBind();

            drpName.DataSource = TransBal.DrpMailReceiver(1);
            drpName.DataValueField = "Value";
            drpName.DataTextField = "Text";
            drpName.DataBind();
        }

        public void RadCreation()
        {
            EditorToolGroup dynamicToolbar = new EditorToolGroup();
            RadEditor3.Tools.Add(dynamicToolbar);

            EditorTool ForeColor = new EditorTool("ForeColor");
            EditorTool FontSize = new EditorTool("FontSize");
            EditorTool JustifyLeft = new EditorTool("JustifyLeft");
            EditorTool JustifyRight = new EditorTool("JustifyRight");
            EditorTool JustifyCenter = new EditorTool("JustifyCenter");
            EditorTool JustifyFull = new EditorTool("JustifyFull");
            EditorTool Italic = new EditorTool("Italic");
            EditorTool Underline = new EditorTool("Underline");
            EditorTool InsertHorizontalRule = new EditorTool("InsertHorizontalRule");
            EditorTool InsertOrderedList = new EditorTool("InsertOrderedList");
            EditorTool InsertUnorderedList = new EditorTool("InsertUnorderedList");

            dynamicToolbar.Tools.Add(ForeColor);
            dynamicToolbar.Tools.Add(FontSize);
            dynamicToolbar.Tools.Add(JustifyLeft);
            dynamicToolbar.Tools.Add(JustifyRight);
            dynamicToolbar.Tools.Add(JustifyCenter);
            dynamicToolbar.Tools.Add(JustifyFull);
            dynamicToolbar.Tools.Add(Italic);
            dynamicToolbar.Tools.Add(Underline);
            dynamicToolbar.Tools.Add(InsertHorizontalRule);
            dynamicToolbar.Tools.Add(InsertOrderedList);
            dynamicToolbar.Tools.Add(InsertUnorderedList);

            RadEditor3.Height = 350;
        }

        public void fillgridList(int PageNumber, int PageSize, string Filter, string OrderByColumnName, string OrderBy)
        {
            DataTable dtDesignationList = TransBal.GetPromotionalMailList(PageNumber, PageSize, Filter, OrderByColumnName, OrderBy);
            rptList.DataSource = dtDesignationList;
            rptList.DataBind();
            if (dtDesignationList.Rows.Count > 0)
            {
                lblPageInfo.Text = "Showing Results " + dtDesignationList.Rows[0]["StartNumber"].ToString() + " - " + dtDesignationList.Rows[dtDesignationList.Rows.Count - 1]["RowNum"].ToString() + " Out of " + dtDesignationList.Rows[0]["CurrentCount"].ToString() + " Records";
                hdnFilter.Value = dtDesignationList.Rows[0]["Filter"].ToString();
                hdnOrderByColumnName.Value = dtDesignationList.Rows[0]["OrderByColumnName"].ToString();
                hdnOrderBy.Value = dtDesignationList.Rows[0]["OrderBy"].ToString();
                hdnLastPage.Value = dtDesignationList.Rows[0]["LastPage"].ToString();
                lblPageNumber.Text = dtDesignationList.Rows[0]["PageNumber"].ToString();
                hdnTotal.Value = dtDesignationList.Rows[0]["CurrentCount"].ToString();

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

        public void btnExportToExcelOnClick(object sender, EventArgs e)
        {
            DataTable dtDesignationList = TransBal.GePromotionalMailExcel();
            if (dtDesignationList.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dtDesignationList, "TemplateList");

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
            Clear();
            HiddenField hdnRptId = (HiddenField)e.Item.FindControl("hdnId");
            DataSet ds = TransBal.EditPromotionalMail(Convert.ToInt32(hdnRptId.Value));
            DataTable dt = ds.Tables[0];

            hdnId.Value = dt.Rows[0]["Id"].ToString();
            //foreach (DataRow dr in ds.Tables[2].Rows)
            //{
            //    RadComboBoxItem item = (RadComboBoxItem)(drpType.FindItemByValue(dr["TypeId"].ToString()));
            //    item.Checked = true;
            //    item.Selected = true;
            //}
            //drpTypeOnSelectedIndexChanged(null, null);
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                RadComboBoxItem item = (RadComboBoxItem)(drpName.FindItemByValue(dr["ReceiverId"].ToString()));
                item.Checked = true;
                item.Selected = true;
            }
            drpTemplate.SelectedValue = dt.Rows[0]["MailTemplateId"].ToString();
            RadEditor3.Content = dt.Rows[0]["MailContent"].ToString();
            txtSubject.Text = dt.Rows[0]["MailSubject"].ToString();
            btnSave.Visible =  false ;
            PanelAdd.Visible = true;
            UpdPanelAdd.Update();
        }

        protected void drpTemplateOnSelectedIndexChanged(object sender, EventArgs e)
        {
            RadEditor3.Content = txtSubject.Text = "";
            if (drpTemplate.SelectedValue != "")
            {
                DataTable dtDesignationDetails = obj_master.EditTemplate(Convert.ToInt32(drpTemplate.SelectedValue));
                RadEditor3.Content = dtDesignationDetails.Rows[0]["Description"].ToString();
                txtSubject.Text = dtDesignationDetails.Rows[0]["Subject"].ToString();
            }
            UpdPanelAddInner.Update();
        }

        protected void drpTypeOnSelectedIndexChanged(object sender, EventArgs e)
        {
            //DataTable dttype = new DataTable();
            //dttype.Columns.Add("ReceiverId", typeof(int));

            //foreach (RadComboBoxItem item in drpType.Items)
            //{
            //    if (item.Checked)
            //    {
            //        DataRow dr = dttype.NewRow();
            //        dttype.Rows.Add(Convert.ToInt32(item.Value));
            //    }
            //}

            //drpName.Items.Clear();
            //drpName.Text = "";
            //if (dttype.Rows.Count > 0)
            //{
            //    drpName.DataSource = TransBal.DrpMailReceiver(dttype);
            //    drpName.DataValueField = "Value";
            //    drpName.DataTextField = "Text";
            //    drpName.DataBind();
            //}
            //updName.Update();
        }

        protected void btnSaveOnClick(object sender, EventArgs e)
        {
            DataTable dtGeneralSettingsDetail = obj_master.Edit_GeneralSettings();
            string fromAddress = dtGeneralSettingsDetail.Rows[0]["CompanyMail"].ToString();
            string fromPassword = dtGeneralSettingsDetail.Rows[0]["CompanyEmailPwd"].ToString();

            if (fromAddress != "" && fromPassword != "")
            {
                DataTable dtReceiver = new DataTable();
                dtReceiver.Columns.Add("ReceiverId", typeof(int));

                foreach (RadComboBoxItem item in drpName.Items)
                {
                    if (item.Checked)
                    {
                        DataRow dr = dtReceiver.NewRow();
                        dtReceiver.Rows.Add(Convert.ToInt32(item.Value));
                    }
                }

                int res = TransBal.InsertPromotionalMail(Convert.ToInt32(hdnId.Value), Currentdate.SelectedDate,
                  drpTemplate.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpTemplate.SelectedValue),
                   txtSubject.Text, RadEditor3.Content, Convert.ToInt32(hdnUserId.Value), dtReceiver, hdn_AttachImgSaveAs.Value);
                if (res > 0)
                {
                    DataTable dtGetEmailListP = TransBal.GetEmailListP(res);

                    string Filepath = "";
                    if (hdn_AttachImgSaveAs.Value != "")
                        Filepath = Server.MapPath("~/MailImage/" + hdn_AttachImgSaveAs.Value);

                    string OnlineSoln = WebConfigurationManager.AppSettings["OnlineSoln"].ToString();

                    if (OnlineSoln == "0")
                    {
                        string RadContent = RadEditor3.Content;
                        string subjct = txtSubject.Text;
                        ThreadStart sms_thread = new ThreadStart(() => obj_common.SendMailPromotion(MailBody(RadContent), dtGetEmailListP,
                           subjct, fromAddress, fromPassword, Filepath));
                        Thread t1 = new Thread(sms_thread);
                        t1.Start();
                    }
                    else
                    {
                        obj_common.SendMailPromotion(MailBody(RadEditor3.Content), dtGetEmailListP,
                            txtSubject.Text, fromAddress, fromPassword, Filepath);
                    }

                    fillgridList(Convert.ToInt32(lblPageNumber.Text), Convert.ToInt32(drpPageSize.SelectedValue), hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
                    Clear();
                }
                else
                {
                    lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                }

                PanelAdd.Visible = false;
                UpdPanelAdd.Update();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Enter Mail details in general settings!');", true);
            }
        }

        public string MailBody(string Contents)
        {
            string mailBody = "";
           
            mailBody = mailBody + @"
<html lang=""en"">
    <head>    
        <meta content=""text/html; charset=utf-8"" http-equiv=""Content-Type"">
       
        <style type=""text/css"">
            HTML{background-color: white;}

        </style>
    </head><body>";
            mailBody = mailBody + Contents + "</body>";
            return mailBody;
        }

        public void fuAttachImgOnFileUploaded(object sender, FileUploadedEventArgs e)
        {
            fuAttachImg.TargetFolder = "~/MailImage";

            string files_name = e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();
            e.File.SaveAs(Path.Combine(Server.MapPath(fuAttachImg.TargetFolder), files_name));
            hdn_AttachImgSaveAs.Value = files_name;
           
            Upd_fuAttachImg.Update();
        }

        protected void btnResetOnClick(object sender, EventArgs e)
        {
            Clear();
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
            UpdPanelAdd.Update();
        }

        public void Clear()
        {
            hdnId.Value = "0";
            Currentdate.SelectedDate = DateTime.Now;
            drpType.ClearSelection();
            drpType.Text = "";
            drpName.ClearSelection();
            drpName.Text = "";
            drpTemplate.ClearSelection();
            drpTemplate.Text = "";
            RadEditor3.Content = "";
            txtSubject.Text = hdn_AttachImgSaveAs.Value = "";

            btnSave.Visible = hdnAdd.Value == "0" ? false : true;

            UpdPanelAddInner.Update();
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

        public void CheckPrivilege()
        {
            try
            {
                if (hdnUserId.Value != null)
                {

                    int val = obj_common.Form_Previlage_Validation(94, Convert.ToInt32(hdnUserId.Value));
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

                    DataTable dtSubMenuAction = obj_common.Action_Previlage_Validation(94, Convert.ToInt32(hdnUserId.Value));
                    if (dtSubMenuAction.Rows.Count > 0)
                    {
                        hdnAdd.Value = dtSubMenuAction.Rows[0][1].ToString();
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