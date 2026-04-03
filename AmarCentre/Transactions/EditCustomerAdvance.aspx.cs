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

namespace AmarCentre.Transactions
{
    public partial class EditCustomerAdvance : System.Web.UI.Page
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
                hdn_user_id.Value = Session["User_Id"].ToString();
                previlage_check();
                previlage_action_check();
                Clear();
            }
        }

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            int res = 0;
            DataTable dt_serDetail = fill_ServiceDetail();
            res = obj_master.InsertCustomerAdvnce(dt_serDetail,  Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            Upd_Add_Panel.Update();
        }

        public DataTable fill_ServiceDetail()
        {
            DataTable dt_serDetail = new DataTable();
            dt_serDetail.Columns.Add("CustomerId", typeof(int));
            dt_serDetail.Columns.Add("Advance", typeof(decimal));
            dt_serDetail.Columns.Add("Amount", typeof(decimal));

            if (rpt_serdetail.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_serdetail.Items)
                {
                    RadComboBox drpCustomer = (RadComboBox)itm.FindControl("drpCustomer");
                    TextBox txt_amt = (TextBox)itm.FindControl("txt_amt");
                    Label lblAdvance = (Label)itm.FindControl("lblAdvance");

                    if (drpCustomer.SelectedValue != "" && txt_amt.Text != "")
                    {
                        dt_serDetail.Rows.Add(Convert.ToInt32(drpCustomer.SelectedValue), lblAdvance.Text == "" ? 0 : Convert.ToDecimal(lblAdvance.Text),
                           txt_amt.Text == "" ? 0 : Convert.ToDecimal(txt_amt.Text));
                    }
                }
            }
            return dt_serDetail;
        }

        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
        }

        protected void rpt_serdetail_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField hdnCustomerId = (HiddenField)e.Item.FindControl("hdnCustomerId");
            RadComboBox drpCustomer = (RadComboBox)e.Item.FindControl("drpCustomer");
            drpCustomer.Items.Clear();
            DataTable dtPayMode = objtrans.Drp_Customer();
            drpCustomer.DataSource = dtPayMode;
            drpCustomer.DataValueField = "Value";
            drpCustomer.DataTextField = "Text";
            drpCustomer.DataBind();
            drpCustomer.SelectedValue = hdnCustomerId.Value;

        }

        /*Add Service Detail*/
        protected void btn_serDetail_newEntry_OnClick(object sender, EventArgs e)
        {
            DataTable dt_serDetail = new DataTable();
            dt_serDetail.Columns.Add("CustomerId", typeof(int));
            dt_serDetail.Columns.Add("Advance", typeof(decimal));
            dt_serDetail.Columns.Add("Amount", typeof(decimal));

            if (rpt_serdetail.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_serdetail.Items)
                {
                    RadComboBox drpCustomer = (RadComboBox)itm.FindControl("drpCustomer");
                    TextBox txt_amt = (TextBox)itm.FindControl("txt_amt");
                    Label lblAdvance = (Label)itm.FindControl("lblAdvance");

                    if (drpCustomer.SelectedValue != "" && txt_amt.Text != "")
                    {
                        dt_serDetail.Rows.Add(Convert.ToInt32(drpCustomer.SelectedValue), lblAdvance.Text == "" ? 0 : Convert.ToDecimal(lblAdvance.Text),
                           txt_amt.Text == "" ? 0 : Convert.ToDecimal(txt_amt.Text));
                    }
                }
            }
            dt_serDetail.Rows.Add(0,null);
            rpt_serdetail.DataSource = dt_serDetail;
            rpt_serdetail.DataBind();

            Upd_ItemList.Update();
        }

        protected void drpCustomerOnSelectedIndexChanged(object sender, EventArgs e)
        {
            RadComboBox drp = (RadComboBox)sender;
            RepeaterItem itm = (RepeaterItem)drp.Parent;
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            TextBox txt_amt = (TextBox)itm.FindControl("txt_amt");
            Label lblAdvance = (Label)itm.FindControl("lblAdvance");
            UpdatePanel Updadvance = (UpdatePanel)itm.FindControl("Updadvance");
            txt_amt.Text = "";
            if (drp.SelectedValue != "")
            {
                DataTable dtAccount = obj_master.Edit_Customer(Convert.ToInt32(drp.SelectedValue)).Tables[0];
                lblAdvance.Text = dtAccount.Rows[0]["Payable"].ToString();
            }
            Updadvance.Update();
        }

        /*Remove Service Detail*/
        protected void btn_remove_line_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            DataTable dt_serDetail = new DataTable();
            dt_serDetail.Columns.Add("CustomerId", typeof(int));
            dt_serDetail.Columns.Add("Advance", typeof(decimal));
            dt_serDetail.Columns.Add("Amount", typeof(decimal));

            if (rpt_serdetail.Items.Count > 0)
            {
                foreach (RepeaterItem itm in rpt_serdetail.Items)
                {
                    RadComboBox drpCustomer = (RadComboBox)itm.FindControl("drpCustomer");
                    TextBox txt_amt = (TextBox)itm.FindControl("txt_amt");
                    Label lblAdvance = (Label)itm.FindControl("lblAdvance");

                    if (drpCustomer.SelectedValue != "" && txt_amt.Text != "")
                    {
                        dt_serDetail.Rows.Add(Convert.ToInt32(drpCustomer.SelectedValue), lblAdvance.Text == "" ? 0 : Convert.ToDecimal(lblAdvance.Text),
                           txt_amt.Text == "" ? 0 : Convert.ToDecimal(txt_amt.Text));
                    }
                }
            }
            dt_serDetail.Rows.RemoveAt(itemrp.ItemIndex);
            if (dt_serDetail.Rows.Count == 0)
                dt_serDetail.Rows.Add(0,null);
            rpt_serdetail.DataSource = dt_serDetail;
            rpt_serdetail.DataBind();

            Upd_ItemList.Update();
        }

        public void Clear()
        {
            DataTable dt_serDetail = new DataTable();
            dt_serDetail.Columns.Add("CustomerId", typeof(int));
            dt_serDetail.Columns.Add("Advance", typeof(decimal));
            dt_serDetail.Columns.Add("Amount", typeof(decimal));
            dt_serDetail.Rows.Add(0,null);
            rpt_serdetail.DataSource = dt_serDetail;
            rpt_serdetail.DataBind();
            hdn_id.Value = "0";

            btn_save.Visible = hdn_add.Value == "0" ? false : true;

            Upd_Add_Panel.Update();
        }

        /*Code for Display*/
     
        //Check Privilege
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.Form_Previlage_Validation(110, Convert.ToInt32(hdn_user_id.Value));
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

        //Check Privilege
        public void previlage_action_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    DataTable dt = obj_common.Action_Previlage_Validation(110, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                    }

                    //hdn_add.Value = "1";
                    btn_save.Visible = hdn_add.Value == "0" ? false : true;
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