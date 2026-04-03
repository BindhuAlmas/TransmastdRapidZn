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
    public partial class UCLeadsource : System.Web.UI.UserControl
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
            int res = masterBAL.InsertUpdateLeadSource(Convert.ToInt32(hdnId.Value), txtName.Text,
                txtDescription.Text, Convert.ToInt32(hdnUserId.Value));
            if (res > 0)
            {
                RadComboBox drpSource = (RadComboBox)this.Parent.FindControl("drpSource");
                drpSource.DataSource = masterBAL.DrpLeadSource();
                drpSource.DataValueField = "Value";
                drpSource.DataTextField = "Text";
                drpSource.DataBind();
                drpSource.Text = "";

                RadComboBoxItem CodeItem = new RadComboBoxItem();
                CodeItem.Text = "New Entry";
                CodeItem.Value = "0";
                drpSource.Items.Insert(0, CodeItem);

                drpSource.SelectedValue = res.ToString();

                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }

            Panel pnlsource = (Panel)this.Parent.FindControl("pnlsource");
            UpdatePanel updSourcePanel = (UpdatePanel)this.Parent.FindControl("updSourcePanel");
            UpdatePanel updSource = (UpdatePanel)this.Parent.FindControl("updSource");
            updSource.Update();
            pnlsource.Visible = false;
            updSourcePanel.Update();

        }

        protected void btnCloseOnClick(object sender, EventArgs e)
        {
            RadComboBox drpSource = (RadComboBox)this.Parent.FindControl("drpSource");
            drpSource.ClearSelection();
            drpSource.Text = "";
            Panel pnlsource = (Panel)this.Parent.FindControl("pnlsource");
            UpdatePanel updSourcePanel = (UpdatePanel)this.Parent.FindControl("updSourcePanel");
            UpdatePanel updSource = (UpdatePanel)this.Parent.FindControl("updSource");
            updSource.Update();
            pnlsource.Visible = false;
            updSourcePanel.Update();
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

                    DataTable dtSubMenuAction = systemUtilities.Action_Previlage_Validation(133, Convert.ToInt32(hdnUserId.Value));
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