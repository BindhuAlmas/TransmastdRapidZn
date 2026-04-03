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
    public partial class UCAnswer : System.Web.UI.UserControl
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
            filldropdwn();
            Clear();
        }

        public void filldropdwn()
        {
            drpdepartment.DataSource = masterBAL.DrpLeadDepartment();
            drpdepartment.DataTextField = "Text";
            drpdepartment.DataValueField = "Value";
            drpdepartment.DataBind();
        }

        protected void btnSaveOnClick(object sender, EventArgs e)
        {
            int res = masterBAL.Insert_UpdateQuestion1(Convert.ToInt32(hdnId.Value), txt_name.Text, txtDescription.Text,
                 Convert.ToInt32(hdnUserId.Value), Convert.ToInt32(drpdepartment.SelectedValue));
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
                    dtService.Rows.Add(0,null);
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

            Panel pnlAnswer = (Panel)this.Parent.FindControl("pnlAnswer");
            UpdatePanel updAnswer = (UpdatePanel)this.Parent.FindControl("updAnswer");
            pnlAnswer.Visible = false;
            updAnswer.Update();
        }

        protected void btnCloseOnClick(object sender, EventArgs e)
        {
            Panel pnlAnswer = (Panel)this.Parent.FindControl("pnlAnswer");
            UpdatePanel updAnswer = (UpdatePanel)this.Parent.FindControl("updAnswer");
            pnlAnswer.Visible = false;
            updAnswer.Update();
        }

        public void Clear()
        {
            hdnId.Value = "0";
            txt_name.Text = "";
            txtDescription.Text = "";
            drpdepartment.ClearSelection();
            drpdepartment.Text = "";
            btnSave.Visible = hdnAdd.Value == "0" ? false : true;
            UpdPanelAddInner.Update();
        }

        public void CheckActionPrivilege()
        {
            try
            {
                if (hdnUserId.Value != null)
                {

                    DataTable dtSubMenuAction = systemUtilities.Action_Previlage_Validation(137, Convert.ToInt32(hdnUserId.Value));
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