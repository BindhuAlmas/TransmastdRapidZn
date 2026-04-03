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
    public partial class UCCity : System.Web.UI.UserControl
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
            int res = masterBAL.InsertUpdateCity(Convert.ToInt32(hdnId.Value), txtName.Text,
                txtDescription.Text, Convert.ToInt32(hdnUserId.Value));
            if (res > 0)
            {
                RadComboBox drpCity = (RadComboBox)this.Parent.FindControl("drpCity");
                drpCity.DataSource = masterBAL.DrpCity();
                drpCity.DataValueField = "Value";
                drpCity.DataTextField = "Text";
                drpCity.DataBind();
                drpCity.Text = "";

                RadComboBoxItem CodeItem = new RadComboBoxItem();
                CodeItem.Text = "New Entry";
                CodeItem.Value = "0";
                drpCity.Items.Insert(0, CodeItem);

                drpCity.SelectedValue = res.ToString();

                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }

            Panel pnlCity = (Panel)this.Parent.FindControl("pnlCity");
            UpdatePanel updCityPanel = (UpdatePanel)this.Parent.FindControl("updCityPanel");
            UpdatePanel updCity = (UpdatePanel)this.Parent.FindControl("updCity");
            updCity.Update();
            pnlCity.Visible = false;
            updCityPanel.Update();

        }

        protected void btnCloseOnClick(object sender, EventArgs e)
        {
            RadComboBox drpCity = (RadComboBox)this.Parent.FindControl("drpCity");
            drpCity.ClearSelection();
            drpCity.Text = "";
            Panel pnlCity = (Panel)this.Parent.FindControl("pnlCity");
            UpdatePanel updCityPanel = (UpdatePanel)this.Parent.FindControl("updCityPanel");
            UpdatePanel updCity = (UpdatePanel)this.Parent.FindControl("updCity");
            updCity.Update();
            pnlCity.Visible = false;
            updCityPanel.Update();
        }

        public void Clear()
        {
            hdnId.Value = "0";
            txtName.Text = "";
            txtDescription.Text = "";
            btnSave.Visible = true;
            UpdPanelAddInner.Update();
        }


    }
}