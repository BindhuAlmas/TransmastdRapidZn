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
    public partial class LeadTransfer : System.Web.UI.Page
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
            DataTable dt = masterBAL.Drp_Employee();
            drpEmployee.DataSource = dt;
            drpEmployee.DataValueField = "Value";
            drpEmployee.DataTextField = "Text";
            drpEmployee.DataBind();

            drpEmployeeTransfer.DataSource = dt;
            drpEmployeeTransfer.DataValueField = "Value";
            drpEmployeeTransfer.DataTextField = "Text";
            drpEmployeeTransfer.DataBind();

            drpEmployeeTransferbulk.DataSource = dt;
            drpEmployeeTransferbulk.DataValueField = "Value";
            drpEmployeeTransferbulk.DataTextField = "Text";
            drpEmployeeTransferbulk.DataBind();
        }

        public void fillgridList(int PageNumber, int PageSize, string Filter, string OrderByColumnName, string OrderBy)
        {
            DataTable dtEmployeeList = TransBal.GetLeadTransferList(PageNumber, PageSize, Filter, OrderByColumnName, OrderBy);
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

        public void btnExportToExcelOnClick(object sender, EventArgs e)
        {
            DataTable dtEmployeeList = TransBal.GetLeadTransferListExcel();
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

        protected void rptListOnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            Clear();
            HiddenField hdnRptId = (HiddenField)e.Item.FindControl("hdnId");
            DataSet dsLeadDetails = TransBal.EditLeadCreation(Convert.ToInt32(hdnRptId.Value));
            DataTable dtBasic = dsLeadDetails.Tables[0];

            txtMobileNumber.Text = dtBasic.Rows[0]["MobileNumber"].ToString();
            txtName.Text = dtBasic.Rows[0]["ContactPersonName"].ToString();
            hdnId.Value = dtBasic.Rows[0]["Id"].ToString();
            txtEmployee.Text = dtBasic.Rows[0]["AssignedEmployeeName"].ToString();

            btnSave.Visible = hdnAdd.Value == "0" ? false : true;

            PanelAdd.Visible = true;
            UpdPanelAdd.Update();
        }

        protected void btnSaveOnClick(object sender, EventArgs e)
        {
            int res = TransBal.UpdateLeadTransfer(Convert.ToInt32(hdnId.Value),  Convert.ToInt32(hdnUserId.Value),
               Convert.ToInt32(drpEmployeeTransfer.SelectedValue), txtRemark.Text);
            if (res > 0)
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
       
        protected void btnCloseOnClick(object sender, EventArgs e)
        {
            PanelAdd.Visible = false;
            UpdPanelAdd.Update();
        }

        protected void drpEmployee_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            DataTable dtEmployeeList = TransBal.GetLeadTransferBulkList(drpEmployee.SelectedValue==""?0:Convert.ToInt32(drpEmployee.SelectedValue));
            rptbulklist.DataSource = dtEmployeeList;
            rptbulklist.DataBind();

            updbulklist.Update();
        }

        protected void chkselall_CheckedChanged(object sender, EventArgs e)
        {
            foreach (RepeaterItem itm in rptbulklist.Items)
            {
                CheckBox chksel = (CheckBox)itm.FindControl("chksel");
                chksel.Checked = chkselall.Checked;
            }
            updbulklist.Update();
        }

        protected void btnBulktransfer_Click(object sender, EventArgs e)
        {
            drpEmployee.ClearSelection();
            drpEmployee.Text = "";
            drpEmployeeTransferbulk.ClearSelection();
            drpEmployeeTransferbulk.Text = "";

            rptbulklist.DataSource = null;
            rptbulklist.DataBind();
            chkselall.Checked = false;

            btnsavebulk.Visible = hdnAdd.Value != "0" ? true : false;

            pnlbulk.Visible = true;
            UpdPanelAdd.Update();
        }

        protected void btnsavebulk_Click(object sender, EventArgs e)
        {
            DataTable dtlist = new DataTable();
            dtlist.Columns.Add("LeadId", typeof(int));

            foreach (RepeaterItem itm in rptbulklist.Items)
            {
                HiddenField hdnbulkId = (HiddenField)itm.FindControl("hdnbulkId");
                CheckBox chksel = (CheckBox)itm.FindControl("chksel");
                if (chksel.Checked)
                    dtlist.Rows.Add(Convert.ToInt32(hdnbulkId.Value));
            }
            if(dtlist.Rows.Count==0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Select an entry to proceed !');", true);
            else
            {
                TransBal.UpdateLeadTransferBulk(dtlist,Convert.ToInt32(hdnUserId.Value),Convert.ToInt32(drpEmployeeTransferbulk.SelectedValue));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Saved Successfully !');", true);

                fillgridList(1, 10, "", "", "");

                pnlbulk.Visible = false;
                UpdPanelAdd.Update();
            }
        }

        protected void btnClosebulkOnClick(object sender, EventArgs e)
        {
            pnlbulk.Visible = false;
            UpdPanelAdd.Update();
        }

        public void Clear()
        {
            hdnId.Value = "0";
            txtName.Text = "";
            txtRemark.Text = "";
            txtMobileNumber.Text ="";
            txtEmployee.Text= "";
            drpEmployeeTransfer.ClearSelection();
            drpEmployeeTransfer.Text = "";

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

                    int val = systemUtilities.Form_Previlage_Validation(141, Convert.ToInt32(hdnUserId.Value));
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

                    DataTable dtSubMenuAction = systemUtilities.Action_Previlage_Validation(141, Convert.ToInt32(hdnUserId.Value));
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