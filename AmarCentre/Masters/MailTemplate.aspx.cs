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
    public partial class MailTemplate : System.Web.UI.Page
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
                hdnUserId.Value = Session["User_Id"].ToString();
                CheckPrivilege();
                CheckActionPrivilege();
                Clear();
                RadCreation();
                fillgridList(1, 10, "", "", "");

            }
        }

        public void RadCreation()
        {
            //add a new Toolbar dynamically
            EditorToolGroup dynamicToolbar = new EditorToolGroup();
            RadEditor3.Tools.Add(dynamicToolbar);

            //add a custom dropdown and set its items and dimension attributes
            EditorDropDown ddn = new EditorDropDown("DynamicDropdown");
            ddn.Text = "Insert Replace";

            ddn.Attributes["width"] = "110px";
            ddn.Attributes["popupwidth"] = "240px";
            ddn.Attributes["popupheight"] = "100px";

            ddn.Items.Add("Customer Name", @"//CustomerName//");
            ddn.Items.Add("Document Details", @"//DetailTable//");
            ddn.Items.Add("Expiry Date", @"//ExpiryDate//");
            ddn.Items.Add("Document Type", @"//DocumentType//");
            ddn.Items.Add("Quotation No", @"//QuotationNo//");
            ddn.Items.Add("Invoice No", @"//InvoiceNo//");
            ddn.Items.Add("Receipt No", @"//ReceiptNo//");
            ddn.Items.Add("ReceiptVoucher No", @"//ReceiptVoucherNo//");

            dynamicToolbar.Tools.Add(ddn);

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
        }

        public void fillgridList(int PageNumber, int PageSize, string Filter, string OrderByColumnName, string OrderBy)
        {
            DataTable dtDesignationList = obj_master.GetTemplateList(PageNumber, PageSize, Filter, OrderByColumnName, OrderBy);
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
            DataTable dtDesignationList = obj_master.GeTemplateListExcel();
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
            DataTable dtDesignationDetails = obj_master.EditTemplate(Convert.ToInt32(hdnRptId.Value));
            hdnId.Value = dtDesignationDetails.Rows[0]["Id"].ToString();
            txtName.Text = dtDesignationDetails.Rows[0]["Name"].ToString();
            RadEditor3.Content = dtDesignationDetails.Rows[0]["Description"].ToString();
            txtSubject.Text = dtDesignationDetails.Rows[0]["Subject"].ToString();
            hdnIsDeleteAllow.Value = dtDesignationDetails.Rows[0]["IsDeleteAllow"].ToString();

            btnSave.Visible = hdnUpdate.Value == "0" ? false : true;
            btnDelete.Visible = hdnDelete.Value == "0" ? false : true;

            if(dtDesignationDetails.Rows[0]["IsDeleteAllow"].ToString()=="0")
            {
                btnDelete.Visible = false;
                txtName.ReadOnly = true;
            }

            PanelAdd.Visible = true;
            UpdPanelAdd.Update();
        }

        protected void btnSaveOnClick(object sender, EventArgs e)
        {
            int res = obj_master.InsertUpdateTemplate(Convert.ToInt32(hdnId.Value), txtName.Text, txtSubject.Text,
                RadEditor3.Content, Convert.ToInt32(hdnUserId.Value));
            if (res >0)
            {
                fillgridList(Convert.ToInt32(lblPageNumber.Text), Convert.ToInt32(drpPageSize.SelectedValue), hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
                Clear();
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

        protected void btnDeleteOnClick(object sender, EventArgs e)
        {
            int res = obj_master.DeleteTemplate(Convert.ToInt32(hdnId.Value), Convert.ToInt32(hdnUserId.Value));
            if (res == 1)
            {
                fillgridList(Convert.ToInt32(lblPageNumber.Text), Convert.ToInt32(drpPageSize.SelectedValue), hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
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
            txtName.Text = "";
            RadEditor3.Content = "";
            txtSubject.Text = "";
            hdnIsDeleteAllow.Value = "1";
            txtName.ReadOnly = false;

            btnDelete.Visible = false;
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

                    int val = obj_common.Form_Previlage_Validation(68, Convert.ToInt32(hdnUserId.Value));
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

                    DataTable dtSubMenuAction = obj_common.Action_Previlage_Validation(68, Convert.ToInt32(hdnUserId.Value));
                    if (dtSubMenuAction.Rows.Count > 0)
                    {
                        hdnAdd.Value = dtSubMenuAction.Rows[0][1].ToString();
                        hdnUpdate.Value = dtSubMenuAction.Rows[1][1].ToString();
                        hdnDelete.Value = dtSubMenuAction.Rows[2][1].ToString();
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