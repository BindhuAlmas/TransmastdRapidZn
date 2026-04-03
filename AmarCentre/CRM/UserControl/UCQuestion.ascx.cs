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

namespace AmarCentre.CRM.UserControl
{
    public partial class UCQuestion : System.Web.UI.UserControl
    {
        System_Utilities systemUtilities = new System_Utilities();
        Master_Bal masterBAL = new Master_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void UCPageLoad()
        {
            hdnUserId.Value = Session["User_Id"].ToString();
            CheckActionPrivilege();
            Clear();
        }

        protected void btnSaveOnClick(object sender, EventArgs e)
        {
            int res = masterBAL.Insert_UpdateLeadDepartment(Convert.ToInt32(hdnId.Value), txtName.Text,
                txtDescription.Text, Convert.ToInt32(hdnUserId.Value));
            if (res > 0)
            {
                Repeater rptservice = (Repeater)this.Parent.FindControl("rptservice");
                UpdatePanel UpdService = (UpdatePanel)this.Parent.FindControl("UpdService");

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
                if (dtService.Rows.Count == 0)
                    dtService.Rows.Add(0, null);
                rptservice.DataSource = dtService;
                rptservice.DataBind();

                UpdService.Update();

                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            Panel pnlQuestion = (Panel)this.Parent.FindControl("pnlQuestion");
            UpdatePanel updQuestion = (UpdatePanel)this.Parent.FindControl("updQuestion");
            pnlQuestion.Visible = false;
            updQuestion.Update();
        }   

        protected void btnCloseOnClick(object sender, EventArgs e)
        {
            Panel pnlQuestion = (Panel)this.Parent.FindControl("pnlQuestion");
            UpdatePanel updQuestion = (UpdatePanel)this.Parent.FindControl("updQuestion");
            pnlQuestion.Visible = false;
            updQuestion.Update();
        }

        public void Clear()
        {
            hdnId.Value = "0";
            txtName.Text = "";
            txtDescription.Text = "";
            btnSave.Visible = hdnAdd.Value == "0" ? false : true;
            UpdPanelAddInner.Update();
        }

        public void CheckActionPrivilege()
        {
            try
            {
                if (hdnUserId.Value != null)
                {

                    DataTable dtSubMenuAction = systemUtilities.Action_Previlage_Validation(136, Convert.ToInt32(hdnUserId.Value));
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