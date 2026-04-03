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
    public partial class UCStatus : System.Web.UI.UserControl
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
            int res = masterBAL.InsertUpdateStatus(Convert.ToInt32(hdnId.Value), txtName.Text,
                txtDescription.Text, Convert.ToInt32(hdnUserId.Value),Convert.ToInt32(ChkClosed.Checked) );
            if (res > 0)
            {
                RadComboBox drpStatus = (RadComboBox)this.Parent.FindControl("drpStatus");
                drpStatus.DataSource = masterBAL.DrpStatus();
                drpStatus.DataValueField = "Value";
                drpStatus.DataTextField = "Text";
                drpStatus.DataBind();
                drpStatus.Text = "";

                RadComboBoxItem CodeItem = new RadComboBoxItem();
                CodeItem.Text = "New Entry";
                CodeItem.Value = "0";
                drpStatus.Items.Insert(0, CodeItem);

                drpStatus.SelectedValue = res.ToString();

                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }

            Panel pnlStatus = (Panel)this.Parent.FindControl("pnlStatus");
            UpdatePanel updStatusPanel = (UpdatePanel)this.Parent.FindControl("updStatusPanel");
            UpdatePanel updStatus = (UpdatePanel)this.Parent.FindControl("updStatus");
            updStatus.Update();
            pnlStatus.Visible = false;
            updStatusPanel.Update();

        }

        protected void btnCloseOnClick(object sender, EventArgs e)
        {
            RadComboBox drpStatus = (RadComboBox)this.Parent.FindControl("drpStatus");
            drpStatus.ClearSelection();
            drpStatus.Text = "";
            Panel pnlStatus = (Panel)this.Parent.FindControl("pnlStatus");
            UpdatePanel updStatusPanel = (UpdatePanel)this.Parent.FindControl("updStatusPanel");
            UpdatePanel updStatus = (UpdatePanel)this.Parent.FindControl("updStatus");
            updStatus.Update();
            pnlStatus.Visible = false;
            updStatusPanel.Update();
        }

        public void Clear()
        {
            hdnId.Value = "0";
            txtName.Text = "";
            txtDescription.Text = "";
            ChkClosed.Checked = false;
            btnSave.Visible = hdnAdd.Value == "0" ? false : true;
            UpdPanelAddInner.Update();
        }

        public void CheckActionPrivilege()
        {
            try
            {
                if (hdnUserId.Value != null)
                {

                    DataTable dtSubMenuAction = systemUtilities.Action_Previlage_Validation(79, Convert.ToInt32(hdnUserId.Value));
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