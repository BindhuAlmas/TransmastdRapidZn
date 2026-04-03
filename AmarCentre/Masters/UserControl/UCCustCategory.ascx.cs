using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AmarCentre.BAL;
using System.Web.UI.HtmlControls;
using System.Globalization;
using Telerik.Web.UI;

namespace AmarCentre.Masters.UserControl
{
    public partial class UCCustCategory : System.Web.UI.UserControl
    {
        System_Utilities systemUtilities = new System_Utilities();
        Master_Bal masterBAL = new Master_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void PageLoad()
        {
            hdnUserId.Value = Session["User_Id"].ToString();
            Clear();
        }

        protected void btnSaveOnClick(object sender, EventArgs e)
        {
            int res = masterBAL.InsertUpdateCCategory(Convert.ToInt32(hdnId.Value), txtName.Text,Convert.ToInt32(hdnUserId.Value));
            if (res > 0)
            {
                RadComboBox drpCategory = (RadComboBox)this.Parent.FindControl("drpCategory");
                drpCategory.DataSource = masterBAL.DrpCustCategory();
                drpCategory.DataValueField = "Value";
                drpCategory.DataTextField = "Text";
                drpCategory.DataBind();
                drpCategory.Text = "";

                RadComboBoxItem CodeItem = new RadComboBoxItem();
                CodeItem.Text = "New Entry";
                CodeItem.Value = "0";
                drpCategory.Items.Insert(0, CodeItem);

                drpCategory.SelectedValue = res.ToString();

                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }

            Panel pnlCategory = (Panel)this.Parent.FindControl("pnlCategory");
            UpdatePanel updCategoryPanel = (UpdatePanel)this.Parent.FindControl("updCategoryPanel");
            UpdatePanel updCategory = (UpdatePanel)this.Parent.FindControl("updCategory");
            updCategory.Update();
            pnlCategory.Visible = false;
            updCategoryPanel.Update();

        }

        protected void btnCloseOnClick(object sender, EventArgs e)
        {
            RadComboBox drpCategory = (RadComboBox)this.Parent.FindControl("drpCategory");
            drpCategory.ClearSelection();
            drpCategory.Text = "";
            Panel pnlCategory = (Panel)this.Parent.FindControl("pnlCategory");
            UpdatePanel updCategoryPanel = (UpdatePanel)this.Parent.FindControl("updCategoryPanel");
            UpdatePanel updCategory = (UpdatePanel)this.Parent.FindControl("updCategory");
            updCategory.Update();
            pnlCategory.Visible = false;
            updCategoryPanel.Update();
        }

        public void Clear()
        {
            hdnId.Value = "0";
            txtName.Text = "";
            btnSave.Visible = true;
            UpdPanelAddInner.Update();
        }


    }
}