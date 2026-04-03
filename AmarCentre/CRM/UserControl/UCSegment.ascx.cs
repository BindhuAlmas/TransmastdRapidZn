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
    public partial class UCSegment : System.Web.UI.UserControl
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
            int res = masterBAL.InsertUpdateSegment(Convert.ToInt32(hdnId.Value), txtName.Text,
                txtDescription.Text, Convert.ToInt32(hdnUserId.Value));
            if (res > 0)
            {
                RadComboBox drpSegment = (RadComboBox)this.Parent.FindControl("drpSegment");
                drpSegment.DataSource = masterBAL.DrpSegment();
                drpSegment.DataValueField = "Value";
                drpSegment.DataTextField = "Text";
                drpSegment.DataBind();
                drpSegment.Text = "";

                RadComboBoxItem CodeItem = new RadComboBoxItem();
                CodeItem.Text = "New Entry";
                CodeItem.Value = "0";
                drpSegment.Items.Insert(0, CodeItem);

                drpSegment.SelectedValue = res.ToString();

                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }

            Panel pnlSegment = (Panel)this.Parent.FindControl("pnlSegment");
            UpdatePanel updSegmentPanel = (UpdatePanel)this.Parent.FindControl("updSegmentPanel");
            UpdatePanel updSegment = (UpdatePanel)this.Parent.FindControl("updSegment");
            updSegment.Update();
            pnlSegment.Visible = false;
            updSegmentPanel.Update();

        }

        protected void btnCloseOnClick(object sender, EventArgs e)
        {
            RadComboBox drpSegment = (RadComboBox)this.Parent.FindControl("drpSegment");
            drpSegment.ClearSelection();
            drpSegment.Text = "";
            Panel pnlSegment = (Panel)this.Parent.FindControl("pnlSegment");
            UpdatePanel updSegmentPanel = (UpdatePanel)this.Parent.FindControl("updSegmentPanel");
            UpdatePanel updSegment = (UpdatePanel)this.Parent.FindControl("updSegment");
            updSegment.Update();
            pnlSegment.Visible = false;
            updSegmentPanel.Update();
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