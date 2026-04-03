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
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace AmarCentre.Masters
{
    public partial class DcoumentExpiryList : System.Web.UI.Page
    {
        Master_Bal obj_master = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
        Report_Bal obj_report = new Report_Bal();
        Transaction_Bal obj_trans = new Transaction_Bal();

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
                fill_Drp_down();
                grid_fill();
            }
        }

        public void fill_Drp_down()
        {
            drpCustomer.Items.Clear();
            drpCustomer.DataSource = obj_report.Drp_Customer();
            drpCustomer.DataTextField = "text";
            drpCustomer.DataValueField = "value";
            drpCustomer.DataBind();

            drpDocument.Items.Clear();
            drpDocument.DataSource = obj_master.fill_drp_DocType();
            drpDocument.DataTextField = "Text";
            drpDocument.DataValueField = "Value";
            drpDocument.DataBind();

            drpagent.Items.Clear();
            drpagent.DataSource = obj_report.fill_drp_Agent();
            drpagent.DataTextField = "text";
            drpagent.DataValueField = "value";
            drpagent.DataBind();
            drpagent.Text = "";

            drpCustomer_SelectedIndexChanged(null, null);
        }


        protected void drpCustomer_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            int c_id = drpCustomer.SelectedValue == "" ? 0 : Convert.ToInt32(drpCustomer.SelectedValue);
            drpCustomerStaff.Text = "";
            drpCustomerStaff.ClearSelection();
            drpCustomerStaff.Items.Clear();
            drpCustomerStaff.DataSource = obj_report.drp_CustomerStaffForExpiry(c_id);
            drpCustomerStaff.DataTextField = "text";
            drpCustomerStaff.DataValueField = "value";
            drpCustomerStaff.DataBind();
            UpdCustStaffPanel.Update();
        }

        public void grid_fill()
        {

            DataTable dtdoc = new DataTable();
            dtdoc.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drpDocument.CheckedItems)
            {
                DataRow dr = dtdoc.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtdoc.Rows.Add(dr);
            }

            DataTable dt = obj_report.GetDocumentexpirylist(txtFromdate.SelectedDate, txtTodate.SelectedDate,
                drpCustomer.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpCustomer.SelectedValue), dtdoc,
                  drpagent.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpagent.SelectedValue), drpCustomerStaff.SelectedValue);
            rpt_list.DataSource = dt;
            rpt_list.DataBind();
         
            Upd_addpanel.Update();
        }

        protected void btnPdfOnClick(object sender, EventArgs e)
        {

            DataTable dtdoc = new DataTable();
            dtdoc.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drpDocument.CheckedItems)
            {
                DataRow dr = dtdoc.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtdoc.Rows.Add(dr);
            }

            Session["dtdoc"] = dtdoc;

            string url = "";
            url = "../Reports/DocumentExpiryPdf.aspx?FromDate=" + txtFromdate.SelectedDate + "&ToDate=" + txtTodate.SelectedDate +
               "&CustomerId=" + drpCustomer.SelectedValue + "&AgentId=" + drpagent.SelectedValue+ "&CustomerStaff=" + drpCustomerStaff.SelectedValue;
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        protected void rpt_list_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            HiddenField hdnId = (HiddenField)e.Item.FindControl("hdnId");
            HiddenField hdnDoctype = (HiddenField)e.Item.FindControl("hdnDoctype");
            HiddenField hdnCustomerMail = (HiddenField)e.Item.FindControl("hdnCustomerMail");
            HiddenField hdnCustomerId = (HiddenField)e.Item.FindControl("hdnCustomerId");

            if (e.CommandName == "Sendmail")
            {
                if (hdnDoctype.Value != "3" )
                {
                    EmailUC.UCDocPageLoad(Convert.ToInt32(hdnId.Value), hdnCustomerMail.Value, hdnDoctype.Value, hdnCustomerId.Value);
                    pnlMail.Visible = true;
                    UpdMailPanel.Update();
                }
            }
        }

        protected void btn_search_OnClick(object sender, EventArgs e)
        {
            grid_fill();
            pnl_filter.Visible = false;
            upd_nav_filter.Update();
        }
        protected void btn_excel_OnClick(object sender, EventArgs e)
        {
            DataTable dtdoc = new DataTable();
            dtdoc.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drpDocument.CheckedItems)
            {
                DataRow dr = dtdoc.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtdoc.Rows.Add(dr);
            }

            DataTable dt = obj_report.GetDocumentexpirylist(txtFromdate.SelectedDate, txtTodate.SelectedDate, 
                drpCustomer.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpCustomer.SelectedValue), dtdoc,
                   drpagent.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpagent.SelectedValue), drpCustomerStaff.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                dt.Columns.Remove("Expiry_date");
                dt.Columns.Remove("labelColor");
                dt.Columns.Remove("Id");
                dt.Columns.Remove("Doctype");
                dt.Columns.Remove("CustomerMail");

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=DocumentExcel.xls");
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();
                GridView1.RenderControl(hw);

                string style = @"<style> .textmode { mso-number-format:\@; word-wrap: break-word; } </style>";
                HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        protected void btn_filter_OnClick(object sender, EventArgs e)
        {
            if (pnl_filter.Visible == true)
            {
                pnl_filter.Visible = false;
            }
            else
            {
                pnl_filter.Visible = true;
            }
            upd_nav_filter.Update();
        }

        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    DataTable dt = obj_common.Action_Previlage_Validation(67, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows[4][1].ToString() != "1")
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