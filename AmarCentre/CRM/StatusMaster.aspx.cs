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
    public partial class StatusMaster : System.Web.UI.Page
    {
        System_Utilities systemUtilities = new System_Utilities();
        Master_Bal masterBAL = new Master_Bal();

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
            }
        }

        public void fillgridList(int PageNumber, int PageSize, string Filter, string OrderByColumnName, string OrderBy)
        {
            DataTable dtStatusList = masterBAL.GetStatusList(PageNumber, PageSize, Filter, OrderByColumnName, OrderBy);
            rptList.DataSource = dtStatusList;
            rptList.DataBind();
            if (dtStatusList.Rows.Count > 0)
            {
                lblPageInfo.Text = "Showing Results " + dtStatusList.Rows[0]["StartNumber"].ToString() + " - " + dtStatusList.Rows[dtStatusList.Rows.Count - 1]["RowNum"].ToString() + " Out of " + dtStatusList.Rows[0]["CurrentCount"].ToString() + " Records";
                hdnFilter.Value = dtStatusList.Rows[0]["Filter"].ToString();
                hdnOrderByColumnName.Value = dtStatusList.Rows[0]["OrderByColumnName"].ToString();
                hdnOrderBy.Value = dtStatusList.Rows[0]["OrderBy"].ToString();
                hdnLastPage.Value = dtStatusList.Rows[0]["LastPage"].ToString();
                lblPageNumber.Text = dtStatusList.Rows[0]["PageNumber"].ToString();
                hdnTotal.Value = dtStatusList.Rows[0]["CurrentCount"].ToString();

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
            DataTable dtStatusList = masterBAL.GetStatusListExcel();
            if (dtStatusList.Rows.Count > 0)
            {
                StringWriter sw = systemUtilities.ExportToExcel(dtStatusList, "StatusListExcel");

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
            DataTable dtBasic = masterBAL.EditStatus(Convert.ToInt32(hdnRptId.Value));

            txtDescription.Text = dtBasic.Rows[0]["Description"].ToString();
            txtName.Text = dtBasic.Rows[0]["Name"].ToString();
            hdnId.Value = dtBasic.Rows[0]["Id"].ToString();
            ChkClosed.Checked = Convert.ToBoolean(dtBasic.Rows[0]["IsClosed"]);

            if (dtBasic.Rows[0]["IsDeleteallow"].ToString() == "0")
                btnSave.Visible = btnDelete.Visible = false;
            else
            {
                btnSave.Visible = hdnUpdate.Value == "0" ? false : true;
                btnDelete.Visible = hdnDelete.Value == "0" ? false : true;
            }

            PanelAdd.Visible = true;
            UpdPanelAdd.Update();
        }

        protected void btnSaveOnClick(object sender, EventArgs e)
        {
            int res = masterBAL.InsertUpdateStatus(Convert.ToInt32(hdnId.Value), txtName.Text,
                txtDescription.Text, Convert.ToInt32(hdnUserId.Value),Convert.ToInt32(ChkClosed.Checked) );
            if (res > 0)
            {
                fillgridList(Convert.ToInt32(lblPageNumber.Text), Convert.ToInt32(drpPageSize.SelectedValue), hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lblerrormsg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
            }

            PanelAdd.Visible = false;
            UpdPanelAdd.Update();
        }

        protected void btnDeleteOnClick(object sender, EventArgs e)
        {
            int res = masterBAL.DeleteStatus(Convert.ToInt32(hdnId.Value), Convert.ToInt32(hdnUserId.Value));
            if (res == 1)
            {
                fillgridList(Convert.ToInt32(lblPageNumber.Text), Convert.ToInt32(drpPageSize.SelectedValue), hdnFilter.Value, hdnOrderByColumnName.Value, hdnOrderBy.Value);
                Clear();
                lbl_msg.Text = "Deleted Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lblerrormsg.Text = "Unable to delete. Entry may be used in lead Page !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
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
            txtDescription.Text = "";
            btnDelete.Visible = false;
            ChkClosed.Checked = false;
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

                    int val = systemUtilities.Form_Previlage_Validation(135, Convert.ToInt32(hdnUserId.Value));
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

                    DataTable dtSubMenuAction = systemUtilities.Action_Previlage_Validation(135, Convert.ToInt32(hdnUserId.Value));
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