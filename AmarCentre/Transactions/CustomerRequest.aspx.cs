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
    public partial class CustomerRequest : System.Web.UI.Page
    {
        Transaction_Bal obj_trans = new Transaction_Bal();
        Master_Bal obj_mas = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
        Report_Bal obj_report = new Report_Bal();
        Voucher BalVoucher = new Voucher();

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
                grid_fill(1, 10, "", "", "");
                fill_Templates();
                if (Request.QueryString["Id"] != null)
                {
                    Filldata(Convert.ToInt32(Request.QueryString["Id"].ToString()));
                }
            }
        }

        public void fill_Templates()
        {
            drpTemplates.Items.Clear();
            DataTable dt = obj_trans.GetTemplates();
            drpTemplates.DataSource = dt;
            drpTemplates.DataTextField = "Text";
            drpTemplates.DataValueField = "Value";
            drpTemplates.DataBind();
            drpTemplates.Text = "";
        }

        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_trans.Get_ListCustomerRequest(page_number, page_size, filter);
            rpt_list.DataSource = dt;
            rpt_list.DataBind();
            if (dt.Rows.Count > 0)
            {
                lbl_page_info.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["RowNum"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
                hdn_filter.Value = dt.Rows[0]["filter"].ToString();
                Common_order_column.Value = dt.Rows[0]["column_name"].ToString();
                Common_asc_desc.Value = dt.Rows[0]["asc_desc"].ToString();
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

        public void Filldata(int Id)
        {
            Clear();
            pnl_add.Visible = true;

            DataSet ds = obj_trans.EditServiceRequest(Id);
            DataTable dt1 = ds.Tables[0];
            DataTable dt_ser = ds.Tables[1];
            DataTable dtdoc = ds.Tables[2];

            hdn_id.Value = dt1.Rows[0]["Id"].ToString();
            lbl_Code.Text = dt1.Rows[0]["Code"].ToString();
            txtCustomer.Text = dt1.Rows[0]["Customer"].ToString();
            job_date.DbSelectedDate = dt1.Rows[0]["Dated"].ToString();
            hdnRequestStatus.Value = dt1.Rows[0]["StatusId"].ToString();
            drpTemplates.SelectedValue = dt1.Rows[0]["TemplateId"].ToString();
            txtApplicant.Text = dt1.Rows[0]["Applicant"].ToString();
            rpt_Item_list.DataSource = dt_ser;
            rpt_Item_list.DataBind();
            lblstatus.Text = "Status : " + dt1.Rows[0]["StatusName"].ToString();

            rptDocumentOut.DataSource = dtdoc;
            rptDocumentOut.DataBind();

            //foreach (RepeaterItem itm in rpt_Item_list.Items)
            //{
            //    HiddenField hdnDId = (HiddenField)itm.FindControl("hdnDId");
            //    Repeater rptDocument = (Repeater)itm.FindControl("rptDocument");

            //    DataTable dtdocin = dtdoc.Clone();
            //    foreach (DataRow rin in dtdoc.Rows)
            //    {
            //        if (rin["RequestDetailId"].ToString() == hdnDId.Value)
            //            dtdocin.ImportRow(rin);
            //    }

            //    rptDocument.DataSource = dtdocin;
            //    rptDocument.DataBind();
            //}

            btnReject.Visible = hdnreject.Value == "1" ? true : false;
            btnnewReq.Visible = hdnCreateInvoice.Value == "1" ? true : false;
            btnsave.Visible = hdnsave.Value == "1" ? true : false;

            if (hdnRequestStatus.Value != "1")
                btnsave.Visible = btnnewReq.Visible = false;

            Upd_Add_Panel.Update();
        }

        protected void rpt_list_OnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdn_id");
            if (e.CommandName == "Edit")
            {
                Filldata(Convert.ToInt32(hdn_rpt_id.Value));
            }
        }

        public DataSet fill_Detail()
        {
            DataTable dt_ser = new DataTable();
            dt_ser.Columns.Add("D_id", typeof(int));
            dt_ser.Columns.Add("DepartmentId", typeof(int));
            dt_ser.Columns.Add("ServiceId", typeof(int));
            dt_ser.Columns.Add("ApplicantName", typeof(string));

            DataTable dtdoc = new DataTable();
            dtdoc.Columns.Add("RequestDetailId", typeof(int));
            dtdoc.Columns.Add("FileNames", typeof(string));
            dtdoc.Columns.Add("FileNamesSave", typeof(string));

            foreach (RepeaterItem itm in rpt_Item_list.Items)
            {
                HiddenField hdnDId = (HiddenField)itm.FindControl("hdnDId");
                RadComboBox drpDepartment = (RadComboBox)itm.FindControl("drpDepartment");
                RadComboBox drpService = (RadComboBox)itm.FindControl("drpService");
                TextBox txt_Applname = (TextBox)itm.FindControl("txt_Applname");
                HiddenField hdnServiceStatus = (HiddenField)itm.FindControl("hdnServiceStatus");

                if (drpService.SelectedValue != "")
                {
                    dt_ser.Rows.Add(Convert.ToInt32(hdnDId.Value), drpDepartment.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpDepartment.SelectedValue),
                        drpService.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpService.SelectedValue), txt_Applname.Text);

                    //Repeater rptDocument = (Repeater)itm.FindControl("rptDocument");
                    //foreach (RepeaterItem itmdoc in rptDocument.Items)
                    //{
                    //    HiddenField hdnFilename = (HiddenField)itmdoc.FindControl("hdnFilename");
                    //    HiddenField hdnFilenameSave = (HiddenField)itmdoc.FindControl("hdnFilenameSave");

                    //    dtdoc.Rows.Add(Convert.ToInt32(hdnDId.Value), hdnFilename.Value, hdnFilenameSave.Value);
                    //}
                }
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(dt_ser);
            ds.Tables.Add(dtdoc);

            return ds;
        }

        protected void btnsaveClick(object sender, EventArgs e)
        {
            int res = 0;
            DataSet ds = fill_Detail();

            DataTable dtdoc = new DataTable();
            dtdoc.Columns.Add("FileNames", typeof(string));
            dtdoc.Columns.Add("FileNamesSave", typeof(string));

            foreach (RepeaterItem itmdoc in rptDocumentOut.Items)
            {
                HiddenField hdnFilenameOut = (HiddenField)itmdoc.FindControl("hdnFilenameOut");
                HiddenField hdnFilenameSaveOut = (HiddenField)itmdoc.FindControl("hdnFilenameSaveOut");

                dtdoc.Rows.Add(hdnFilenameOut.Value, hdnFilenameSaveOut.Value);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                res = obj_trans.Insert_UpdateServiceRequest(Convert.ToInt32(hdn_id.Value), job_date.SelectedDate, (int?)null, Convert.ToInt32(hdn_user_id.Value),
                  ds.Tables[0], ds.Tables[1], txtremark.Text,Convert.ToInt32(drpTemplates.SelectedValue), dtdoc,txtApplicant.Text);
                if (res > 0)
                {
                    grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                    lbl_msgin.Text = "Saved Successfully !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                }
                else
                {
                    lbl_msgin.Text = "Sorry Failed to Process Your Request !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                }
            }
            else
            {
                lbl_msgin.Text = "Add Service to Continue !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }

            pnl_add.Visible = false;
            Upd_Add_Panel.Update();
        }

        protected void btninvoice_Click(object sender, EventArgs e)
        {
            UCInvoice.UCPageLoadCR(3, Convert.ToInt32(hdn_id.Value));
            pnlInvoiceadd.Visible = true;
            UpdInvoiceadd.Update();
        }

        protected void rptitemlistDatabound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField hdnDepartmentId = (HiddenField)e.Item.FindControl("hdnDepartmentId");
            RadComboBox drpDepartment = (RadComboBox)e.Item.FindControl("drpDepartment");
            drpDepartment.Items.Clear();
            drpDepartment.DataSource = obj_mas.Drp_Department();
            drpDepartment.DataValueField = "Value";
            drpDepartment.DataTextField = "Text";
            drpDepartment.DataBind();
            drpDepartment.SelectedValue = hdnDepartmentId.Value;

            HiddenField hdnServiceId = (HiddenField)e.Item.FindControl("hdnServiceId");
            RadComboBox drpService = (RadComboBox)e.Item.FindControl("drpService");
            drpService.Items.Clear();
            drpService.DataSource = obj_mas.DrpServicebyDepartment(hdnDepartmentId.Value == "" ? 0 : Convert.ToInt32(hdnDepartmentId.Value));
            drpService.DataValueField = "Id";
            drpService.DataTextField = "Name";
            drpService.DataBind();
            drpService.SelectedValue = hdnServiceId.Value;
        }

        protected void drpService_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            RadComboBox drp = (RadComboBox)sender;
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            HiddenField hdnDepartmentId = (HiddenField)itemrp.FindControl("hdnDepartmentId");
            RadComboBox drpDepartment = (RadComboBox)itemrp.FindControl("drpDepartment");
            UpdatePanel UpdDepartment = (UpdatePanel)itemrp.FindControl("UpdDepartment");
            if (drp.SelectedValue != "")
            {
                DataSet dtAccount = obj_mas.Edit_Service(Convert.ToInt32(drp.SelectedValue));
                if (drpDepartment.SelectedValue == "")
                {
                    drpDepartment.SelectedValue = dtAccount.Tables[0].Rows[0]["DepartmentId"].ToString();
                    hdnDepartmentId.Value = dtAccount.Tables[0].Rows[0]["DepartmentId"].ToString();
                }
            }
            UpdDepartment.Update();
        }

        protected void drpDepartmentOnSelectedIndexChanged(object sender, EventArgs e)
        {
            RadComboBox drp = (RadComboBox)sender;
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            RadComboBox drpService = (RadComboBox)itemrp.FindControl("drpService");
            UpdatePanel UpdService = (UpdatePanel)itemrp.FindControl("UpdService");
            HiddenField hdnServiceId = (HiddenField)itemrp.FindControl("hdnServiceId");

            drpService.DataSource = obj_mas.DrpServicebyDepartment(drp.SelectedValue == "" ? 0 : Convert.ToInt32(drp.SelectedValue));
            drpService.DataValueField = "Id";
            drpService.DataTextField = "Name";
            drpService.DataBind();
            hdnServiceId.Value = "";
            drpService.ClearSelection();
            drpService.Text = "";
            UpdService.Update();
        }

        protected void rptDocumentOut_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Download")
            {
                HiddenField hdnFilenameSaveOut = (HiddenField)e.Item.FindControl("hdnFilenameSaveOut");
                HiddenField hdnFilenameOut = (HiddenField)e.Item.FindControl("hdnFilenameOut");

                try
                {
                    if (hdnFilenameSaveOut.Value != "")
                    {
                        string fil_name = hdnFilenameSaveOut.Value;
                        string full_name = Server.MapPath("~/UploadedFiles/" + fil_name);
                        Response.ContentType = "APPLICATION/OCTET-STREAM";
                        String Header = "Attachment; Filename=\"" + hdnFilenameOut.Value + "\"";
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

        protected void rpt_Item_list_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Document")
            {
                Panel pnlDocument = (Panel)e.Item.FindControl("pnlDocument");
                UpdatePanel UpdDocument = (UpdatePanel)e.Item.FindControl("UpdDocument");
                pnlDocument.Visible = true;
                UpdDocument.Update();
            }
            else if (e.CommandName == "CloseDocument")
            {
                Panel pnlDocument = (Panel)e.Item.FindControl("pnlDocument");
                UpdatePanel UpdDocument = (UpdatePanel)e.Item.FindControl("UpdDocument");

                pnlDocument.Visible = false;
                UpdDocument.Update();
            }
             else if (e.CommandName == "Add" || e.CommandName == "Delete")
            {
                DataTable dt_ser = new DataTable();
                dt_ser.Columns.Add("D_id", typeof(int));
                dt_ser.Columns.Add("ServiceId", typeof(int));
                dt_ser.Columns.Add("DepartmentId", typeof(int));
                dt_ser.Columns.Add("ApplicantName", typeof(string));

                DataTable dtdoc = new DataTable();
                dtdoc.Columns.Add("RequestDetailId", typeof(int));
                dtdoc.Columns.Add("FileNames", typeof(string));
                dtdoc.Columns.Add("FileNamesSave", typeof(string));

                foreach (RepeaterItem itm in rpt_Item_list.Items)
                {
                    HiddenField hdnDId = (HiddenField)itm.FindControl("hdnDId");
                    RadComboBox drpDepartment = (RadComboBox)itm.FindControl("drpDepartment");
                    RadComboBox drpService = (RadComboBox)itm.FindControl("drpService");
                    TextBox txt_Applname = (TextBox)itm.FindControl("txt_Applname");

                    if (drpService.SelectedValue != "")
                    {
                        dt_ser.Rows.Add(Convert.ToInt32(hdnDId.Value), Convert.ToInt32(drpService.SelectedValue), drpDepartment.SelectedValue == "" ?
                        (int?)null : Convert.ToInt32(drpDepartment.SelectedValue), txt_Applname.Text);

                        Repeater rptDocument = (Repeater)itm.FindControl("rptDocument");
                        foreach (RepeaterItem itmdoc in rptDocument.Items)
                        {
                            HiddenField hdnFilename = (HiddenField)itmdoc.FindControl("hdnFilename");
                            HiddenField hdnFilenameSave = (HiddenField)itmdoc.FindControl("hdnFilenameSave");

                            dtdoc.Rows.Add(Convert.ToInt32(hdnDId.Value), hdnFilename.Value, hdnFilenameSave.Value);
                        }
                    }
                }

                if (e.CommandName == "Add")
                    dt_ser.Rows.Add(Convert.ToInt32(hdnDetailIndexId.Value) - 1, null, null);

                else if (e.CommandName == "Delete")
                {
                    dt_ser.Rows.RemoveAt(e.Item.ItemIndex);
                    if (dt_ser.Rows.Count == 0)
                        dt_ser.Rows.Add(Convert.ToInt32(hdnDetailIndexId.Value) - 1, null, null);
                }

                rpt_Item_list.DataSource = dt_ser;
                rpt_Item_list.DataBind();

                foreach (RepeaterItem itm in rpt_Item_list.Items)
                {
                    HiddenField hdnDId = (HiddenField)itm.FindControl("hdnDId");
                    Repeater rptDocument = (Repeater)itm.FindControl("rptDocument");

                    DataTable dtdocin = dtdoc.Clone();
                    foreach (DataRow rin in dtdoc.Rows)
                    {
                        if (rin["RequestDetailId"].ToString() == hdnDId.Value)
                            dtdocin.ImportRow(rin);
                    }
                    rptDocument.DataSource = dtdocin;
                    rptDocument.DataBind();
                }
                Upd_Item_Panel.Update();
            }
        }

        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
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

        protected void btn_close_OnClick(object sender, EventArgs e)
        {
            pnl_add.Visible = false;
            Upd_Add_Panel.Update();
        }

        protected void btn_newentry_OnClick(object sender, EventArgs e)
        {
            Clear();
            pnl_add.Visible = true;
            Upd_Add_Panel.Update();
        }

        public void Clear()
        {
            Session["User_Id"] = hdn_user_id.Value;
            hdn_id.Value = "0";
            job_date.DbSelectedDate = DateTime.Now;
            hdnDetailIndexId.Value = "-1";
            txtremark.Text =txtApplicant.Text= "";

            rptDocumentOut.DataSource = null;
            rptDocumentOut.DataBind();

            btnsave.Visible = false;
            btnReject.Visible = false;
            btnnewReq.Visible = false;
            Upd_Add_PanelInner.Update();
        }

        #region History

        protected void btnhistory_OnClick(object sender, EventArgs e)
        {
            grid_fill_his();
            pnlhistry.Visible = true;
            Updhistory.Update();
        }

        protected void btn_histry_Close_OnClick(object sender, EventArgs e)
        {
            pnlhistry.Visible = false;
            Updhistory.Update();
        }

        public void grid_fill_his()
        {
            DataSet ds = obj_trans.listRequestHistry(Convert.ToInt32(hdn_id.Value));
            DataTable dt = ds.Tables[0];

            rpt_His.DataSource = dt;
            rpt_His.DataBind();

            Updhistory.Update();
        }

        #endregion

        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.Form_Previlage_Validation(151, Convert.ToInt32(hdn_user_id.Value));
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

        public void previlage_action_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    DataTable dt = obj_common.Action_Previlage_Validation(151, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdnreject.Value = dt.Rows[0][1].ToString();
                        hdnCreateInvoice.Value = dt.Rows[1][1].ToString();
                        hdnsave.Value = dt.Rows[2][1].ToString();
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