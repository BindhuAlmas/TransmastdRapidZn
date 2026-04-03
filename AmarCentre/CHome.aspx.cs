using AmarCentre.BAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AmarCentre
{
    public partial class CHome : System.Web.UI.Page
    {
        Master_Bal obj_master = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
        Transaction_Bal objtrans = new Transaction_Bal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
            }
            if (!IsPostBack)
            {

                lbl_User_name.Text = Session["User_Name"].ToString();
                Session["User_Name"] = lbl_User_name.Text;
                hdnuserid.Value = Session["User_Id"].ToString();
                Session["User_Id"] = hdnuserid.Value;
                fillLatest_Invoice();

                DataSet dsMenu = obj_master.Get_MenuListCustomer(Convert.ToInt32(hdnuserid.Value));
                DataTable dt = dsMenu.Tables[0];
                btnInvoiceListview.Visible = dt.Rows[2]["checkedint"].ToString() == "1" ? true : false;

                DataSet ds = objtrans.CustomerHomedetail(Convert.ToInt32(hdnuserid.Value));
                InvoiceCount.Text = ds.Tables[0].Rows[0]["InvoiceCount"].ToString();
                ServiceCount.Text = ds.Tables[1].Rows[0]["ServiceCount"].ToString();
                PendingServiceCount.Text = ds.Tables[2].Rows[0]["PendingServiceCount"].ToString();
                if (ds.Tables[3].Rows.Count > 0)
                    TotalPayable.Text = ds.Tables[3].Rows[0]["TotalPayable"].ToString();
                else
                    TotalPayable.Text = "0.00";
            }
        }

        public void fillLatest_Invoice()
        {
            divInvoice.Visible = true;
            DataSet ds = objtrans.GetInvoiceList(Convert.ToInt32(hdnuserid.Value));
            if (ds.Tables[0].Rows.Count == 0)
            { divInvoice.Visible = false; }
            else
            {
                rptInvoice.DataSource = ds.Tables[0];
                rptInvoice.DataBind();
            }
        }
    }
}