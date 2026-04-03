using System;
using AmarCentre.BAL;
using System.Data;

namespace AmarCentre.Layout
{
    public partial class Customer : System.Web.UI.MasterPage
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
                hdn_user_id.Value = Session["User_Id"].ToString();
                Session["User_Id"] = hdn_user_id.Value;

                hdn_name.Value = Session["User_Name"].ToString();
                Session["User_Name"] = hdn_name.Value;

              
                DataSet dsMenu = obj_master.Get_MenuListCustomer(Convert.ToInt32(hdn_user_id.Value));
                DataTable dt = dsMenu.Tables[0];

                btnServiceRequest.Visible = dt.Rows[0]["checkedint"].ToString() == "1" ? true : false;
                btnSCStatus.Visible = dt.Rows[1]["checkedint"].ToString() == "1" ? true : false;
                btnInvoiceList.Visible = dt.Rows[2]["checkedint"].ToString() == "1" ? true : false;
                btnSOA.Visible = dt.Rows[3]["checkedint"].ToString() == "1" ? true : false;
                btnDocumentExpiry.Visible = dt.Rows[4]["checkedint"].ToString() == "1" ? true : false;
                btnCustomerDocDwnld.Visible = dt.Rows[5]["checkedint"].ToString() == "1" ? true : false;

            }
        }

    }
}