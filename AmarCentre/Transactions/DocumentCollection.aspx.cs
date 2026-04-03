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
    public partial class DocumentCollection : System.Web.UI.Page
    {
        Master_Bal obj_master = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
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
                previlage_action_check();
                fillCustomer();
                fill_Document();
                grid_fill(1, 10, "", "", "");
            }
        }

        public void fillCustomer()
        {
            DataTable dt = obj_trans.Drp_Customer();
            Drp_Cust.DataSource = dt;
            Drp_Cust.DataTextField = "text";
            Drp_Cust.DataValueField = "value";
            Drp_Cust.DataBind();
        }

        //Drop Down Document Type
        public void fill_Document()
        {
            drp_doc.Items.Clear();
            DataTable dt = obj_master.fill_drp_DocType();
            drp_doc.DataSource = dt;
            drp_doc.DataTextField = "Text";
            drp_doc.DataValueField = "Value";
            drp_doc.DataBind();
            drp_doc.Text = "";
        }

        //Get List 
        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_trans.ListDocColl(page_number, page_size, filter, column, order);
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
            Upd_Nav_Panel.Update();
            Upd_List_Panel.Update();
        }

        //exel export
        public void btnexcel_export_OnClick(object sender, EventArgs e)
        {
            DataTable dt = obj_trans.DocCollExcel();
            dt.Columns["Sl_No"].ColumnName = "Sl No.";

            if (dt.Rows.Count > 0)
            {

                StringWriter sw = obj_common.ExportToExcel(dt, "DocumentList");

                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                HttpContext.Current.Response.Write(style);
                Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        //rpt Command
        protected void rpt_list_OnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            Clear();
            HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdn_id");
            DataSet ds = obj_trans.EditDocColl(Convert.ToInt32(hdn_rpt_id.Value));
            DataTable dt = ds.Tables[0];
            lbl_code.Text = dt.Rows[0]["Code"].ToString();
            Drp_Cust.SelectedValue = dt.Rows[0]["Customer_Id"].ToString();
            on_date.DbSelectedDate = dt.Rows[0]["Dated"].ToString();
            txt_desc.Text = dt.Rows[0]["Description"].ToString();

            hdn_id.Value = dt.Rows[0][0].ToString();

            Rpt_Doc.DataSource = ds.Tables[1];
            Rpt_Doc.DataBind();
            int j = Rpt_Doc.Items.Count;
            lbl_message.Text = j.ToString() + " Documents added";

            btn_delete.Visible = hdn_delete.Value == "0" ? false : true;
            btn_save.Visible = hdn_update.Value == "0" ? false : true;
            btn_save_print.Visible = hdn_update_N_print.Value == "0" ? false : true;
            btn_print.Visible = hdn_print.Value == "0" ? false : true;
            pnl_add.Visible = true;

            Upd_Add_Panel.Update();
        }

        //Save Button
        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            DataTable dt = fill_doc();
            if (dt.Rows.Count == 0)
            {
                lbl_msg.Text = "Add Document to proceed !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                int res = obj_trans.insertupdateDocCollection(Convert.ToInt32(hdn_id.Value), DateTime.ParseExact(CalDate(on_date), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Convert.ToInt32(Drp_Cust.SelectedValue), txt_desc.Text, Convert.ToInt32(hdn_user_id.Value), dt);
                if (res > 0)
                {
                    grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                    Clear();
                    lbl_msg.Text = "Saved Successfully !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                }
                else
                {
                    lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                }
            }
            Upd_Add_Panel.Update();
        }

        protected void btn_save_print_OnClick(object sender, EventArgs e)
        {
            DataTable dt = fill_doc();
            if (dt.Rows.Count == 0)
            {
                lbl_msg.Text = "Add Document to proceed !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                int res = obj_trans.insertupdateDocCollection(Convert.ToInt32(hdn_id.Value), DateTime.ParseExact(CalDate(on_date), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Convert.ToInt32(Drp_Cust.SelectedValue), txt_desc.Text, Convert.ToInt32(hdn_user_id.Value), dt);
                if (res > 0)
                {
                    grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                    Clear();
                    lbl_msg.Text = "Saved Successfully !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);

                    string url = "../Reports/DocumentCollPrint.aspx?id=" + res;
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
                }
                else
                {
                    lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                }
            }
            Upd_Add_Panel.Update();
        }

        protected void btn_delete_OnClick(object sender, EventArgs e)
        {
            int res = obj_trans.deleteDocColl(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                lbl_msg.Text = "Deleted Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            Upd_Add_Panel.Update();
        }
        //Reset Button
        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
        }

        /*Print*/
        protected void btn_print_OnClick(object sender, EventArgs e)
        {
            string url = "../Reports/DocumentCollPrint.aspx?id=" + Convert.ToInt32(hdn_id.Value);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        public void fu_documents_OnFileUploaded(object sender, FileUploadedEventArgs e)
        {
            fu_documents.TargetFolder = "~/UploadedFiles";
            DataTable dt = obj_common.Get_File_Code("CustDoc");
            if (dt.Rows.Count > 0 & dt.Rows[0][0].ToString() != "")
            {
                string files_name = dt.Rows[0][0].ToString() + e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();
                e.File.SaveAs(Path.Combine(Server.MapPath(fu_documents.TargetFolder), files_name));

                try
                {
                    //in backup folder also
                    DataTable dtgen = obj_master.Edit_GeneralSettings();
                    File.Copy((Path.Combine(Server.MapPath(fu_documents.TargetFolder), files_name)),
                        (Path.Combine(dtgen.Rows[0]["BackupDrivePath"].ToString() + "UploadedFiles", files_name)), false);
                }
                catch (Exception cc) { }

                hdn_doc_sav.Value = files_name;
            }
        }

        protected void btn_new_line_OnClick(object sender, EventArgs e)
        {
            DataTable dt_doc = new DataTable();
            dt_doc.Columns.Add("D_id", typeof(int));
            dt_doc.Columns.Add("Doc_Id", typeof(int));
            dt_doc.Columns.Add("Doc_name", typeof(string));
            dt_doc.Columns.Add("Doc_num", typeof(string));
            dt_doc.Columns.Add("Remark", typeof(string));
            dt_doc.Columns.Add("Valid_From", typeof(DateTime));
            dt_doc.Columns.Add("Valid_To", typeof(DateTime));
            dt_doc.Columns.Add("NewRemark", typeof(string));
            dt_doc.Columns.Add("filename", typeof(string));

            if (Rpt_Doc.Items.Count > 0)
            {
                foreach (RepeaterItem itm in Rpt_Doc.Items)
                {
                    HiddenField hdn_doc_id = (HiddenField)itm.FindControl("hdn_doc_id");
                    HiddenField hdn_D_id = (HiddenField)itm.FindControl("hdn_D_id");
                    HiddenField hdn_file = (HiddenField)itm.FindControl("hdn_file");

                    HiddenField v_frm = (HiddenField)itm.FindControl("v_frm");
                    HiddenField v_to = (HiddenField)itm.FindControl("v_to");

                    Label lbl_name = (Label)itm.FindControl("lbl_name");
                    Label lbl_num = (Label)itm.FindControl("lbl_num");
                    Label lbl_remark = (Label)itm.FindControl("lbl_remark");
                    Label lbl_from = (Label)itm.FindControl("lbl_from");
                    Label lbl_to = (Label)itm.FindControl("lbl_to");
                    Label lbl_newremark = (Label)itm.FindControl("lbl_newremark");

                    dt_doc.Rows.Add(Convert.ToInt32(hdn_D_id.Value), Convert.ToInt32(hdn_doc_id.Value),
                        lbl_name.Text, lbl_num.Text, lbl_remark.Text, v_frm.Value == "" ? null : v_frm.Value, v_to.Value == "" ? null : v_to.Value, lbl_newremark.Text, hdn_file.Value);
                }
            }
            if (drp_doc.SelectedValue != "")
            {
                dt_doc.Rows.Add(0, Convert.ToInt32(drp_doc.SelectedValue), drp_doc.SelectedItem.Text, txt_num.Text, txt_remark.Text,
                    valid_from.SelectedDate, valid_to.SelectedDate, txt_remNew.Text, hdn_doc_sav.Value);
                hdn_doc_sav.Value = "";
                drp_doc.ClearSelection();
                txt_remark.Text = "";
                txt_num.Text = "";
                valid_from.SelectedDate = null;
                valid_to.SelectedDate = null;
                txt_remNew.Text = "";
            }

            Rpt_Doc.DataSource = dt_doc;
            Rpt_Doc.DataBind();
            int j = Rpt_Doc.Items.Count;
            lbl_message.Text = j.ToString() + " Documents added";
            Upd_doc.Update();
        }

        protected void btn_remove_line_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            DataTable dt_doc = new DataTable();
            dt_doc.Columns.Add("D_id", typeof(int));
            dt_doc.Columns.Add("Doc_Id", typeof(int));
            dt_doc.Columns.Add("Doc_name", typeof(string));
            dt_doc.Columns.Add("Doc_num", typeof(string));
            dt_doc.Columns.Add("Remark", typeof(string));
            dt_doc.Columns.Add("Valid_From", typeof(DateTime));
            dt_doc.Columns.Add("Valid_To", typeof(DateTime));
            dt_doc.Columns.Add("NewRemark", typeof(string));
            dt_doc.Columns.Add("filename", typeof(string));

            if (Rpt_Doc.Items.Count > 0)
            {
                foreach (RepeaterItem itm in Rpt_Doc.Items)
                {
                    HiddenField hdn_doc_id = (HiddenField)itm.FindControl("hdn_doc_id");
                    HiddenField hdn_D_id = (HiddenField)itm.FindControl("hdn_D_id");
                    HiddenField hdn_file = (HiddenField)itm.FindControl("hdn_file");
                    HiddenField v_frm = (HiddenField)itm.FindControl("v_frm");
                    HiddenField v_to = (HiddenField)itm.FindControl("v_to");

                    Label lbl_name = (Label)itm.FindControl("lbl_name");
                    Label lbl_num = (Label)itm.FindControl("lbl_num");
                    Label lbl_remark = (Label)itm.FindControl("lbl_remark");
                    Label lbl_from = (Label)itm.FindControl("lbl_from");
                    Label lbl_to = (Label)itm.FindControl("lbl_to");
                    Label lbl_newremark = (Label)itm.FindControl("lbl_newremark");

                    dt_doc.Rows.Add(Convert.ToInt32(hdn_D_id.Value), Convert.ToInt32(hdn_doc_id.Value), lbl_name.Text, lbl_num.Text,
                        lbl_remark.Text, v_frm.Value == "" ? null : v_frm.Value, v_to.Value == "" ? null : v_to.Value, lbl_newremark.Text, hdn_file.Value);
                }
            }

            dt_doc.Rows.RemoveAt(itemrp.ItemIndex);

            Rpt_Doc.DataSource = dt_doc;
            Rpt_Doc.DataBind();
            int j = Rpt_Doc.Items.Count;
            lbl_message.Text = j.ToString() + " Documents added";
            Upd_doc.Update();
        }

        protected void rpt_doc_list_OnItemCommand(object s, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Download")
            {
                HiddenField hdn_fnm = (HiddenField)e.Item.FindControl("hdn_file");

                try
                {
                    if (hdn_fnm.Value != "")
                    {
                        string strURL = hdn_fnm.Value;
                        string[] ext = hdn_fnm.Value.Split('.');
                        string extension = ext[1];
                        string fil_name = strURL;
                        string full_name = Server.MapPath("~/UploadedFiles/" + fil_name);
                        Response.ContentType = "APPLICATION/OCTET-STREAM";
                        String Header = "Attachment; Filename=\"" + fil_name + "\"";
                        Response.AppendHeader("Content-Disposition", Header);
                        System.IO.FileInfo Dfile = new System.IO.FileInfo(Server.MapPath("~/UploadedFiles/" + fil_name));
                        Response.WriteFile(Dfile.FullName);
                        //Don't forget to add the following line
                        Response.End();
                    }
                }

                catch (Exception ex)
                {
                }
            }
        }

        public DataTable fill_doc()
        {
            DataTable dt_doc = new DataTable();
            dt_doc.Columns.Add("D_id", typeof(int));
            dt_doc.Columns.Add("Doc_Id", typeof(int));
            dt_doc.Columns.Add("Doc_num", typeof(string));
            dt_doc.Columns.Add("Remark", typeof(string));
            dt_doc.Columns.Add("Valid_From", typeof(DateTime));
            dt_doc.Columns.Add("Valid_To", typeof(DateTime));
            dt_doc.Columns.Add("NewRemark", typeof(string));
            dt_doc.Columns.Add("filename", typeof(string));

            if (Rpt_Doc.Items.Count > 0)
            {
                foreach (RepeaterItem itm in Rpt_Doc.Items)
                {
                    HiddenField hdn_doc_id = (HiddenField)itm.FindControl("hdn_doc_id");
                    HiddenField hdn_D_id = (HiddenField)itm.FindControl("hdn_D_id");
                    HiddenField hdn_file = (HiddenField)itm.FindControl("hdn_file");
                    HiddenField v_frm = (HiddenField)itm.FindControl("v_frm");
                    HiddenField v_to = (HiddenField)itm.FindControl("v_to");

                    Label lbl_num = (Label)itm.FindControl("lbl_num");
                    Label lbl_remark = (Label)itm.FindControl("lbl_remark");
                    Label lbl_from = (Label)itm.FindControl("lbl_from");
                    Label lbl_to = (Label)itm.FindControl("lbl_to");
                    Label lbl_newremark = (Label)itm.FindControl("lbl_newremark");

                    dt_doc.Rows.Add(Convert.ToInt32(hdn_D_id.Value), Convert.ToInt32(hdn_doc_id.Value), lbl_num.Text,
                        lbl_remark.Text, v_frm.Value == "" ? null : v_frm.Value, v_to.Value == "" ? null : v_to.Value, lbl_newremark.Text, hdn_file.Value);
                }
            }

            return dt_doc;
        }

        /*Code for Display*/
        public void Get_Code()
        {
            DataTable dt = obj_common.Get_Code(72);
            if (dt.Rows.Count > 0)
                lbl_code.Text = dt.Rows[0][0].ToString();
        }

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

        //Clear all Data
        public void Clear()
        {
            Get_Code();
            Drp_Cust.ClearSelection();
            Drp_Cust.Text = "";
            txt_desc.Text = "";
            hdn_doc_sav.Value = "";
            hdn_id.Value = "0";
            txt_num.Text = "";
            txt_remark.Text = "";
            drp_doc.ClearSelection();
            drp_doc.Text = "";
            valid_from.SelectedDate = null;
            valid_to.SelectedDate = null;
            txt_remNew.Text = "";
            Rpt_Doc.DataSource = null;
            Rpt_Doc.DataBind();
            lbl_message.Text = "0 Document added";
            on_date.DbSelectedDate = DateTime.Now.Date;

            btn_save.Visible = hdn_add.Value == "0" ? false : true;
            btn_save_print.Visible = hdn_add_N_print.Value == "0" ? false : true;
            btn_print.Visible = false;
            btn_delete.Visible = false;

            Upd_Add_Panel.Update();
        }

        //Search
        protected void txt_search_OnTextChanged(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), txt_search.Text, Common_order_column.Value, Common_asc_desc.Value);
        }

        #region Navigation

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

        //Check Privilege
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    int val = obj_common.Form_Previlage_Validation(72, Convert.ToInt32(hdn_user_id.Value));
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
                    DataTable dt = obj_common.Action_Previlage_Validation(72, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdn_update.Value = dt.Rows[1][1].ToString();
                        hdn_delete.Value = dt.Rows[2][1].ToString();
                        hdn_print.Value = dt.Rows[3][1].ToString();
                        hdn_add_N_print.Value = dt.Rows[4][1].ToString();
                        hdn_update_N_print.Value = dt.Rows[5][1].ToString();
                    }
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


        /*Calucate the Date*/
        public string CalDate(Telerik.Web.UI.RadDatePicker Dates)
        {
            string month = Dates.SelectedDate.Value.Month.ToString();
            if (month != "10" && month != "11" && month != "12")
                month = "0" + month;
            string day = Dates.SelectedDate.Value.Day.ToString();
            for (int i = 0; i < 10; i++)
            {
                if (Convert.ToInt32(day) == i)
                    day = "0" + day;
            }
            string year = Dates.SelectedDate.Value.Year.ToString();
            return day + '/' + month + '/' + year;
        }
    }
}