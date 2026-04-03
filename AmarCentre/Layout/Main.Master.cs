using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AmarCentre.BAL;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.IO;

namespace AmarCentre.Layout
{
    public partial class Main : System.Web.UI.MasterPage
    {
        System_Utilities obj_common = new System_Utilities();
        Master_Bal obj_master = new Master_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
            }
            if (!IsPostBack)
            {
                lbl_User_name.Text = Session["User_Name"].ToString();
                hdn_user_id.Value = Session["User_Id"].ToString();
                lblDesignation.Text = Session["DesignationName"].ToString();
                hdn_language.Value = Session["language"].ToString();
                Session["User_Id"] = hdn_user_id.Value;
                Session["User_Name"] = lbl_User_name.Text;
                Session["DesignationName"] = lblDesignation.Text;
                Session["language"] = hdn_language.Value;

                string pflimg= Session["ProfilePhotoSave"].ToString();
                Session["ProfilePhotoSave"] = pflimg;

                lbldash.Text = "Dashboard";
                lbldoc.Text = "Document Expiry";

                DataTable dtprv = obj_common.Action_Previlage_Validation(67, Convert.ToInt32(hdn_user_id.Value));
                if (dtprv.Rows.Count > 0)
                {
                    lbldoc.Visible = dtprv.Rows[4][1].ToString() == "1" ? true : false;
                    lbldeadline.Visible = dtprv.Rows[5][1].ToString() == "1" ? true : false;
                    lblfollowup.Visible = dtprv.Rows[7][1].ToString() == "1" ? true : false;
                    lblvisalist.Visible= dtprv.Rows[8][1].ToString() == "1" ? true : false;
                    lblCustDash.Visible = dtprv.Rows[11][1].ToString() == "1" ? true : false;
                    lblWhatsappDash.Visible = dtprv.Rows[12][1].ToString() == "1" ? true : false;

                }
                if (Session["ProfilePhotoSave"].ToString() != "")
                    img_profile.ImageUrl = "~/UploadedImage/" + Session["ProfilePhotoSave"].ToString();
                else
                    img_profile.ImageUrl = "~/Images/profiles.png";

                DataSet ds = obj_common.Get_Main_Menu(Convert.ToInt32(hdn_user_id.Value), Convert.ToInt32(hdn_language.Value));
                DataTable dt = ds.Tables[0];
                rpt_main_menu.DataSource = dt;
                rpt_main_menu.DataBind();

                lblCompany.Text = ds.Tables[1].Rows[0]["CompanyName"].ToString();

            }
        }

        protected void rpt_main_menu_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField hdn_id = (HiddenField)e.Item.FindControl("hdn_id");
            Repeater rpt_side_menu_here = (Repeater)e.Item.FindControl("rpt_side_menu");

            DataTable dt = obj_common.Get_Sub_Menu(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_user_id.Value), Convert.ToInt32(hdn_language.Value));
            rpt_side_menu_here.DataSource = dt;
            rpt_side_menu_here.DataBind();
        }

        #region Password Change

        protected void lnkPasswordChangeOnClick(object sender, EventArgs e)
        {
            lnkPasswordChangeOnClick();
        }
        public void lnkPasswordChangeOnClick()
        {
            ClearPassword();
            PanelPasswordChange.Visible = true;
            UpdPasswordChangePanel.Update();
        }
        protected void btnSavePassword_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Update_Password(Convert.ToInt32(hdn_user_id.Value), txtCurrentPassword.Text,txtNewPassword.Text);
            if (res == 1)
            {
                ClearPassword();
                lblProfileMsg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MasterPopup", "ToggleMasterDiv();", true);
                PanelPasswordChange.Visible = false;
                
            }
            else if(res==-1)
            {
                lblProfileMsg.Text = "Current Password Is Wrong !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MasterPopup", "ToggleMasterDiv();", true);
                //UpdPasswordChangeMsg.Update();
            }
            else 
            {
                lblProfileMsg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MasterPopup", "ToggleMasterDiv();", true);
                //UpdPasswordChangeMsg.Update();
            }
            UpdPasswordChangePanel.Update();
        }
        protected void btnResetPassword_OnClick(object sender, EventArgs e)
        {
            ClearPassword();
            UpdPasswordChangeInner.Update();
        }
        protected void btnClosePassword_OnClick(object sender, EventArgs e)
        {
            PanelPasswordChange.Visible = false;
            UpdPasswordChangePanel.Update();
        }

        public void ClearPassword()
        {
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";

            txtCurrentPassword.Attributes["value"] = "";
            txtNewPassword.Attributes["value"] = "";
            txtConfirmPassword.Attributes["value"] = "";
        }
        #endregion

        #region Profile

        protected void lnkProfileOnclick(object sender, EventArgs e)
        {
            lnkProfileOnclick();
        }
        public void lnkProfileOnclick()
        {
            FillProfile();
            PanelProfile.Visible = true;
            UpdProfilePanel.Update();
        }

        public void fuProfilePhoto_OnFileUploaded(object sender, FileUploadedEventArgs e)
        {
            DataTable dt = obj_common.Get_File_Code("EmpProfile");
            if (dt.Rows.Count > 0 & dt.Rows[0][0].ToString() != "")
            {
                fuProfilePhoto.TargetFolder = "~/UploadedImage";

                string files_name = dt.Rows[0][0].ToString() + e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();
                e.File.SaveAs(Path.Combine(Server.MapPath(fuProfilePhoto.TargetFolder), files_name));
                hdnProfileFileName.Value = e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();
                hdnProfileFileSaveName.Value = files_name;
                Session["ProfilePhotoSave"] = hdnProfileFileSaveName.Value;
            }
            UpdProfilePhoto.Update();
        }

        protected void btnSaveProfile_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.Update_Profile(Convert.ToInt32(hdn_user_id.Value), txtProfileName.Text, txtProfileMobile.Text,
                txtProfilePhone.Text, txtProfileEmail.Text, hdnProfileFileName.Value, hdnProfileFileSaveName.Value);
            if (res == 1)
            {
                FillProfile();
                lblProfileMsg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MasterPopup", "ToggleMasterDiv();", true);
                PanelProfile.Visible = false;

            }
            else
            {
                lblProfileMsg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MasterPopup", "ToggleMasterDiv();", true);
                //UpdPasswordChangeMsg.Update();
            }
            UpdProfilePanel.Update();
            //UpdatePanel2.Update();
        }
        protected void btnResetProfile_OnClick(object sender, EventArgs e)
        {
            FillProfile();
            UpdProfileInner.Update();
        }
        protected void btnCloseProfile_OnClick(object sender, EventArgs e)
        {
            PanelProfile.Visible = false;
            UpdProfilePanel.Update();
        }

        public void FillProfile()
        {
            DataSet ds = obj_master.Edit_Employee(Convert.ToInt32(hdn_user_id.Value));
            DataTable dt = ds.Tables[0];
            txtProfileName.Text = dt.Rows[0]["Name"].ToString();
            txtProfileMobile.Text = dt.Rows[0]["MobileNum"].ToString();
            txtProfilePhone.Text = dt.Rows[0]["Phone_number"].ToString();
            txtProfileEmail.Text = dt.Rows[0]["EmailId"].ToString();
            hdnProfileFileName.Value = dt.Rows[0]["ProfilePhoto"].ToString();
            hdnProfileFileSaveName.Value = dt.Rows[0]["ProfilePhotoSave"].ToString();
            if (hdnProfileFileName.Value != "")
                imgProfilePhoto.ImageUrl = "~/UploadedImage/" + hdnProfileFileSaveName.Value;
            else
                imgProfilePhoto.ImageUrl = "~/Images/defaultimage.png";
        }
        #endregion
    }
}