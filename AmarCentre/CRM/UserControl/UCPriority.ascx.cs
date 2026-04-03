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
    public partial class UCPriority : System.Web.UI.UserControl
    {
        System_Utilities systemUtilities = new System_Utilities();
        Master_Bal masterBAL = new Master_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void PageLoad()
        {
            hdnUserId.Value = Session["User_Id"].ToString();
            CheckActionPrivilege();
            Clear();
        }

        protected void btnSaveOnClick(object sender, EventArgs e)
        {
            int res = masterBAL.InsertUpdatePriority(Convert.ToInt32(hdnId.Value), txtName.Text,
                txtDescription.Text, Convert.ToInt32(hdnUserId.Value));
            if (res > 0)
            {
                RadComboBox drpPriority = (RadComboBox)this.Parent.FindControl("drpPriority");
                drpPriority.DataSource = masterBAL.DrpPriority();
                drpPriority.DataValueField = "Value";
                drpPriority.DataTextField = "Text";
                drpPriority.DataBind();
                drpPriority.Text = "";

                RadComboBoxItem CodeItem = new RadComboBoxItem();
                CodeItem.Text = "New Entry";
                CodeItem.Value = "0";
                drpPriority.Items.Insert(0, CodeItem);

                drpPriority.SelectedValue = res.ToString();

                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }

            Panel pnlPriority = (Panel)this.Parent.FindControl("pnlPriority");
            UpdatePanel updPriorityPanel = (UpdatePanel)this.Parent.FindControl("updPriorityPanel");
            UpdatePanel updPriority = (UpdatePanel)this.Parent.FindControl("updPriority");
            updPriority.Update();
            pnlPriority.Visible = false;
            updPriorityPanel.Update();

        }

        protected void btnCloseOnClick(object sender, EventArgs e)
        {
            RadComboBox drpPriority = (RadComboBox)this.Parent.FindControl("drpPriority");
            drpPriority.ClearSelection();
            drpPriority.Text = "";
            Panel pnlPriority = (Panel)this.Parent.FindControl("pnlPriority");
            UpdatePanel updPriorityPanel = (UpdatePanel)this.Parent.FindControl("updPriorityPanel");
            UpdatePanel updPriority = (UpdatePanel)this.Parent.FindControl("updPriority");
            updPriority.Update();
            pnlPriority.Visible = false;
            updPriorityPanel.Update();
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

                    DataTable dtSubMenuAction = systemUtilities.Action_Previlage_Validation(134, Convert.ToInt32(hdnUserId.Value));
                    if (dtSubMenuAction.Rows.Count > 0)
                    {
                        hdnAdd.Value = dtSubMenuAction.Rows[0][1].ToString();
                        hdnUpdate.Value = dtSubMenuAction.Rows[1][1].ToString();
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