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

namespace AmarCentre.Masters
{
    public partial class VisaArrivalList : System.Web.UI.Page
    {
        System_Utilities obj_common = new System_Utilities();
        Report_Bal obj_report = new Report_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
            }
            if (!IsPostBack)
            {
                hdn_user_id.Value = Session["User_Id"].ToString();
                previlage_check();
                txtFromdate.SelectedDate = DateTime.Now;
                txtTodate.SelectedDate = DateTime.Now.AddDays(30);
                grid_fill();
            }
        }

        public void grid_fill()
        {
            DataSet ds = obj_report.VisaArrivalList(txtFromdate.SelectedDate, txtTodate.SelectedDate);
            rpt_list.DataSource = ds.Tables[0];
            rpt_list.DataBind();

            Upd_List_Panel.Update();
        }
        protected void btn_search_OnClick(object sender, EventArgs e)
        {
            grid_fill();
        }

        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    DataTable dt = obj_common.Action_Previlage_Validation(67, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows[8][1].ToString() != "1")
                    {
                        Response.Redirect("../Landing.aspx");
                    }
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