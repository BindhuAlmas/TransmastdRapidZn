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

namespace AmarCentre.Customer
{
    public partial class CustomerDocDwnld : System.Web.UI.Page
    {

        Master_Bal obj_master = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
        Transaction_Bal objtrans = new Transaction_Bal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdn_user_id.Value = Session["User_Id"].ToString();
                lbl_User_name.Text = Session["User_Name"].ToString();
                hdn_user_id.Value = Session["User_Id"].ToString();

                previlage_check();
                grid_fill( 1, 10,"","","");
            }
        }

        protected void rpt_doc_list_OnItemCommand_Staff(object s, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Download")
            {
                HiddenField hdn_fnm = (HiddenField)e.Item.FindControl("hdn_dnm");

                try
                {
                    if (hdn_fnm.Value != "")
                    {
                        string fil_name = hdn_fnm.Value;
                        string full_name = Server.MapPath("~/UploadedFiles/" + fil_name);
                        Response.ContentType = "APPLICATION/OCTET-STREAM";
                        String Header = "Attachment; Filename=\"" + fil_name + "\"";
                        Response.AppendHeader("Content-Disposition", Header);
                        System.IO.FileInfo Dfile = new System.IO.FileInfo(Server.MapPath("~/UploadedFiles/" + fil_name));
                        Response.WriteFile(Dfile.FullName);
                        Response.End();
                    }
                }

                catch (Exception ex)
                {
                }
            }
        }

        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataSet ds = objtrans.ListDocumentDwnloadInCustomerPort(page_number, page_size, filter, Convert.ToInt32(hdn_user_id.Value));
            DataTable dt = ds.Tables[0];
            rpt_list.DataSource = dt;
            rpt_list.DataBind();
            if (dt.Rows.Count > 0)
            {
                lbl_page_info.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["dt_indx"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
                hdn_filter.Value = dt.Rows[0]["filter"].ToString();
                hdn_last_page.Value = dt.Rows[0]["last_page"].ToString();
                lbl_page_number.Text = dt.Rows[0]["page_number"].ToString();
                hdn_total.Value = dt.Rows[0]["current_count"].ToString();
            }
            else
            {
                lbl_page_info.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
                hdn_filter.Value = txt_search.Text;
                hdn_last_page.Value = "0";
                lbl_page_number.Text = "1";
                hdn_total.Value = "0";
            }
            Upd_List_Panel.Update();
            Upd_Nav_Panel.Update();
        }

        #region Navigation

        /*txt_search OnTextChanged*/
        protected void txt_search_OnTextChanged(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), txt_search.Text, Common_order_column.Value, Common_asc_desc.Value);
        }

        //First Page
        protected void btn_first_OnClick(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
        }

        //Previous Page
        protected void btn_prev_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number.Text) > 1)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text) - 1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
            }
        }

        //Next Page
        protected void btn_next_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number.Text) < Convert.ToInt32(hdn_last_page.Value))
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text) + 1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
            }
        }

        //Last Page
        protected void btn_last_OnClick(object sender, EventArgs e)
        {

            grid_fill(Convert.ToInt32(hdn_last_page.Value), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
        }

        //Page Data Count
        protected void drp_count_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
        }

        #endregion

        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.CustomerForm_Previlage_Validation(7, Convert.ToInt32(hdn_user_id.Value));
                    if (val == 0)
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