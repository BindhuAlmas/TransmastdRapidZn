using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AmarCentre.BAL;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Text;
using Telerik.Web.UI;
using System.Threading;

namespace AmarCentre
{
    public partial class Landing : System.Web.UI.Page
    {
        Master_Bal obj_master = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
        Transaction_Bal obj_trans = new Transaction_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Abandon();
            }

        }

        #region Login


        protected void btn_login_OnClick(object sender, EventArgs e)
        {
            DataTable dt_valid = obj_common.validation_check();
            if (dt_valid.Rows.Count > 0 && dt_valid.Rows[0][0].ToString() == "False")
            {
                lblVE.Text = "Error Code : VEx00043.The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. Failed to connect to database when attempting to connect to SQL server";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Validity Expired.!');", true);
            }
            else
            {
                DataSet dsLoginResult = obj_common.User_Verification(txt_Uname.Text, txt_pass.Text);
                DataTable dt_user = dsLoginResult.Tables[0];

                DataSet dsdocexp = obj_common.DocumentExpiredDetails();
                DataTable dtExpiredDoc = dsdocexp.Tables[0]; //DocumentExpiry

                DataTable dt = obj_master.Edit_GeneralSettings();
                string cc_companymail = dt.Rows[0]["CompanyMail"].ToString();
                string signpath = "";
                if (dt.Rows[0]["MailSignature"].ToString() != "")
                    signpath = Server.MapPath("~/UploadedImage/" + dt.Rows[0]["MailSignature"].ToString());

                if (dt.Rows[0]["IsProfessionVersion"].ToString() == "1")
                {
                    if (dtExpiredDoc.Rows.Count>0)
                    {
                        ThreadStart sms_thread = new ThreadStart(() => obj_common.SendAgreementExpiredMail(dtExpiredDoc, cc_companymail,
                            signpath));
                        Thread t1 = new Thread(sms_thread);
                        t1.Start();
                    }
                }

                if (dt.Rows[0]["IsWhatsappAlertOn"].ToString() == "1")
                {
                    DataTable dtExpiredWAP = dsdocexp.Tables[1]; //Whatsapp

                    if (dtExpiredWAP.Rows.Count>0)
                    {
                        ThreadStart sms_thread2 = new ThreadStart(() => obj_common.SendWAExpiredAlert(dtExpiredWAP,
                            dt.Rows[0]["CompanyName"].ToString(), dt.Rows[0]["CompanyPhone"].ToString(),
                            dt.Rows[0]["CompanyContactPerson"].ToString())
                            );
                        Thread t12 = new Thread(sms_thread2);
                        t12.Start();
                    }
                }

                if (dt_user.Rows.Count > 0)
                {
                    if (dt_user.Rows[0]["usertype"].ToString() == "2")
                    {
                        Session["User_Id"] = dt_user.Rows[0]["Id"];
                        Session["User_Name"] = dt_user.Rows[0]["Name"];
                        Session["language"] = "1";
                        Response.Redirect("CHome.aspx");
                    }
                    else
                    {
                        Session["User_Id"] = dt_user.Rows[0]["Id"];
                        Session["User_Name"] = dt_user.Rows[0]["Name"];
                        Session["DesignationName"] = dt_user.Rows[0]["DesignationName"];
                        Session["ProfilePhoto"] = @"../UploadedImage/" + dt_user.Rows[0]["ProfilePhotoSave"];
                        Session["ProfilePhotoSave"] = dt_user.Rows[0]["ProfilePhotoSave"];
                        Session["LoginDayCount"] = dt_user.Rows[0]["LoginDayCount"];

                        Session["language"] = "1";
                        Response.Redirect("Home.aspx");
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('You Entered Wrong Username or Password.!');", true);
                    txt_Uname.Text = "";
                }
            }

        }

        #endregion

    }
}