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
    public partial class Sponser : System.Web.UI.Page
    {
        Master_Bal obj_master = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();

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
                fill_Document();
                Clear();
                grid_fill(1, 10, "", "", "");
            }
        }

        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_master.Get_ListSponser(page_number, page_size, filter, column, order);
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

        public void btnexcel_export_OnClick(object sender, EventArgs e)
        {
            DataTable dt = obj_master.Get_ListSponserExcel();
            dt.Columns["Sl_No"].ColumnName = "Sl No.";
            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "Sponser");

                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                HttpContext.Current.Response.Write(style);
                Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        protected void rpt_list_OnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            Clear();
            HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdn_id");
            DataSet ds = obj_master.EditSponser(Convert.ToInt32(hdn_rpt_id.Value));
            DataTable dt = ds.Tables[0];
            DataTable dt_doc = ds.Tables[1];

            txt_name.Text = dt.Rows[0]["Name"].ToString();
            txtArabicName.Text = dt.Rows[0]["ArabicName"].ToString();
            txt_address.Text = dt.Rows[0]["Address"].ToString();
            txt_mob.Text = dt.Rows[0]["Mobile_num"].ToString();
            txt_phn.Text = dt.Rows[0]["Phone_num"].ToString();
            txt_remark.Text = dt.Rows[0]["Remark"].ToString();
            txt_email.Text = dt.Rows[0]["Email"].ToString();
            hdn_id.Value = dt.Rows[0]["Id"].ToString();
            txtuaepass.Text = dt.Rows[0]["UAEPass"].ToString();

            Session["dt_doc"] = dt_doc;
            fill_rpt(dt_doc, 1, Convert.ToInt32(drp_countD.SelectedValue));


            btn_delete.Visible = hdn_delete.Value == "0" ? false : true;
            btn_save.Visible = hdn_update.Value == "0" ? false : true;
            btn_doc.Visible = hdn_doc.Value == "0" ? false : true;

            pnl_add.Visible = true;
            Upd_Add_Panel.Update();
        }

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            
            int res = obj_master.Insert_UpdateSponser(Convert.ToInt32(hdn_id.Value), txt_name.Text, txt_address.Text,
                txt_mob.Text, txt_phn.Text, txt_email.Text, txt_remark.Text, Convert.ToInt32(hdn_user_id.Value), txtArabicName.Text,txtuaepass.Text);
            if (res > 0)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                lbl_msg.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lblerrormsg.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
            }
            pnl_add.Visible = false;
            Upd_Add_Panel.Update();
        }

        protected void btn_delete_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.DeleteSponser(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                lbl_msg.Text = "Deleted Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lblerrormsg.Text = "Unable to delete. Entry may be used in transaction Page !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDivError();", true);
            }
            pnl_add.Visible = false;
            Upd_Add_Panel.Update();
        }

        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
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
        public void Clear()
        {
            txt_name.Text = "";
            txtArabicName.Text = "";
            txt_address.Text = "";
            txt_mob.Text = "";
            txt_phn.Text = "";
            txt_remark.Text = "";
            txt_email.Text = "";
            txtuaepass.Text = "";
            hdn_id.Value = "0";

           
            btn_save.Visible = hdn_add.Value == "0" ? false : true;
            btn_delete.Visible = false;
            btn_doc.Visible =  false;

            Upd_Add_PanelInner.Update();
        }

        protected void txt_search_OnTextChanged(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), txt_search.Text, Common_order_column.Value, Common_asc_desc.Value);
            Upd_List_Panel.Update();
        }

        #region Document Upload

        protected void btn_docadd_OnClick(object sender, EventArgs e)
        {
            pnl_document.Visible = true;
            Clear_documnt();
            DataSet ds = obj_master.EditSponser(Convert.ToInt32(hdn_id.Value));
            DataTable dt_doc = ds.Tables[1];
            Session["dt_doc"] = dt_doc;
            fill_rpt(dt_doc, 1, Convert.ToInt32(drp_countD.SelectedValue));

            Upd_Document_Panel.Update();
        }

        protected void btn_Docclose_OnClick(object sender, EventArgs e)
        {
            pnl_document.Visible = false;
            Upd_Document_Panel.Update();
        }

        public void Clear_documnt()
        {
            drp_doc.ClearSelection();
            drp_doc.Text = "";
            valid_from.SelectedDate = null;
            valid_to.SelectedDate = null;
            hdn_doc_name.Value = "";
            lab_doc_name_out.Text = "";
            hdn_doc_sav.Value = "";
            txt_doc_no.Text = "";
            txt_docname.Text = "";
            txt_docremark.Text = "";
            hdn_doc_index_Id.Value = "0";
            txtValidityyr.Text = "";
        }

        protected void txtValidityyr_TextChanged(object sender, EventArgs e)
        {
            DateTime? Expirydate = null;
            if (txtValidityyr.Text != "" && valid_from.DbSelectedDate != null)
            {
                Expirydate = valid_from.SelectedDate.Value.AddYears(Convert.ToInt32(txtValidityyr.Text));
                valid_to.SelectedDate = Expirydate.Value.AddDays(-1);
                updVTo.Update();
            }
        }

        protected void btn_DocSave_OnClick(object sender, EventArgs e)
        {
            int res = obj_master.UpdateSponserDocument(Convert.ToInt32(hdn_id.Value), fill_Detail(), Convert.ToInt32(hdn_user_id.Value));
            if (res == 1)
            {
                pnl_document.Visible = false;
            }
            else
            {
            }
            Upd_Document_Panel.Update();
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

                hdn_doc_name.Value = e.File.GetNameWithoutExtension().ToString() + e.File.GetExtension();
                hdn_doc_sav.Value = files_name;
                lab_doc_name_out.Text = hdn_doc_name.Value;
            }
        }

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

        protected void rpt_doc_list_OnItemCommand(object s, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Download")
            {
                HiddenField hdn_fnm = (HiddenField)e.Item.FindControl("hdn_dnm");

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
            else if (e.CommandName == "Edit")
            {
                HiddenField hdn_id = (HiddenField)e.Item.FindControl("hdn_id");
                HiddenField hdn_doc_Id = (HiddenField)e.Item.FindControl("hdn_doc_Id");
                HiddenField hdn_dnm = (HiddenField)e.Item.FindControl("hdn_dnm");
                Label lbl_doc_name = (Label)e.Item.FindControl("lbl_doc_name");
                Label lbl_doc_type_name = (Label)e.Item.FindControl("lbl_doc_type_name");
                Label lbl_docnum = (Label)e.Item.FindControl("lbl_docnum");
                Label lbl_docname = (Label)e.Item.FindControl("lbl_docname");
                Label lbl_remark = (Label)e.Item.FindControl("lbl_remark");

                Label lbl_from = (Label)e.Item.FindControl("lbl_from");
                Label lbl_to = (Label)e.Item.FindControl("lbl_to");
                HiddenField v_frm = (HiddenField)e.Item.FindControl("v_frm");
                HiddenField v_to = (HiddenField)e.Item.FindControl("v_to");
                HiddenField hdnVyr = (HiddenField)e.Item.FindControl("hdnVyr");

                Clear_documnt();

                drp_doc.SelectedValue = hdn_doc_Id.Value;
                valid_from.DbSelectedDate = v_frm.Value == "" ? (DateTime?)null : Convert.ToDateTime(v_frm.Value);
                valid_to.DbSelectedDate = v_to.Value == "" ? (DateTime?)null : Convert.ToDateTime(v_to.Value);
                lab_doc_name_out.Text = lbl_doc_name.Text;
                hdn_doc_name.Value = lbl_doc_name.Text;
                hdn_doc_sav.Value = hdn_dnm.Value;
                hdn_doc_index_Id.Value = hdn_id.Value;
                txt_doc_no.Text = lbl_docnum.Text;
                txt_docname.Text = lbl_docname.Text;
                txt_docremark.Text = lbl_remark.Text;
                txtValidityyr.Text = hdnVyr.Value;

                Upd_docadd.Update();
            }
        }

        /*Remove Line*/
        protected void btn_remove_line_OnClick(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;

            HiddenField hdn_indx = (HiddenField)itemrp.FindControl("hdn_indx");
            DataTable dt_doc = (DataTable)Session["dt_doc"];
            DataTable dt_doc_add = new DataTable();
            dt_doc_add = dt_doc.Clone();

            foreach (DataRow rows in dt_doc.Rows)
            {
                if (hdn_indx.Value != rows["dt_indx"].ToString())
                {
                    dt_doc_add.Rows.Add(dt_doc_add.Rows.Count + 1, Convert.ToInt32(rows["id"]), rows["DocNumber"].ToString(), Convert.ToInt32(rows["DocumentTypeId"]),
                        rows["doc_type"].ToString(), rows["DocumentName"].ToString(), rows["DocumentSave"].ToString(),
                          rows["Valid_From"].ToString() == "" ? null : rows["Valid_From"].ToString(), rows["Valid_To"].ToString() == "" ? null : rows["Valid_To"].ToString(),
                          rows["Document_name"].ToString(), rows["Remark"].ToString(), rows["ValidityYear"].ToString() == "" ? (int?)null : Convert.ToInt32(rows["ValidityYear"]));
                }
            }
            Session["dt_doc"] = dt_doc_add;
            fill_rpt(dt_doc_add, 1, Convert.ToInt32(drp_countD.SelectedValue));

            Upd_doc.Update();
        }

        protected void btn_reset_doc_OnClick(object sender, EventArgs e)
        {
            Clear_documnt();
            Upd_docadd.Update();
        }

        protected void btn_add_doc_OnClick(object sender, EventArgs e)
        {
            DataTable dt_doc = (DataTable)Session["dt_doc"];
            DataTable dt_doc_add = new DataTable();
            dt_doc_add = dt_doc.Clone();

            foreach (DataRow rows in dt_doc.Rows)
            {
                if (hdn_doc_index_Id.Value != "0" && rows["id"].ToString() == hdn_doc_index_Id.Value)
                {
                    dt_doc_add.Rows.Add(Convert.ToInt32(rows["dt_indx"]), Convert.ToInt32(rows["id"]), txt_doc_no.Text, Convert.ToInt32(drp_doc.SelectedValue), drp_doc.SelectedItem.Text, hdn_doc_name.Value,
                        hdn_doc_sav.Value, valid_from.DbSelectedDate != null ? DateTime.ParseExact(CalDate(valid_from), "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null,
                        valid_to.DbSelectedDate != null ? DateTime.ParseExact(CalDate(valid_to), "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null, txt_docname.Text, txt_docremark.Text,
                        txtValidityyr.Text == "" ? (int?)null : Convert.ToInt32(txtValidityyr.Text));
                }
                else
                {
                    dt_doc_add.Rows.Add(Convert.ToInt32(rows["dt_indx"]), Convert.ToInt32(rows["id"]), rows["DocNumber"].ToString(), Convert.ToInt32(rows["DocumentTypeId"]),
                        rows["doc_type"].ToString(), rows["DocumentName"].ToString(), rows["DocumentSave"].ToString(),
                           rows["Valid_From"].ToString() == "" ? null : rows["Valid_From"].ToString(), rows["Valid_To"].ToString() == "" ? null : rows["Valid_To"].ToString(),
                           rows["Document_name"].ToString(), rows["Remark"].ToString(), rows["ValidityYear"].ToString() == "" ? (int?)null : Convert.ToInt32(rows["ValidityYear"]));
                }
            }
            if (hdn_doc_index_Id.Value == "0")
            {
                DateTime? Expirydate = valid_to.SelectedDate;
                if (valid_to.DbSelectedDate == null && txtValidityyr.Text != "" && valid_from.DbSelectedDate != null)
                {
                    Expirydate = valid_from.SelectedDate.Value.AddYears(Convert.ToInt32(txtValidityyr.Text));
                }


                dt_doc_add.Rows.Add(dt_doc.Rows.Count + 1, "-" + (dt_doc.Rows.Count + 1).ToString(), txt_doc_no.Text, Convert.ToInt32(drp_doc.SelectedValue),
                    drp_doc.SelectedItem.Text, hdn_doc_name.Value, hdn_doc_sav.Value, valid_from.DbSelectedDate != null ? DateTime.ParseExact(CalDate(valid_from), "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null,
                 //valid_to.DbSelectedDate != null ? DateTime.ParseExact(CalDate(valid_to), "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null
                 Expirydate, txt_docname.Text, txt_docremark.Text, txtValidityyr.Text == "" ? (int?)null : Convert.ToInt32(txtValidityyr.Text));
            }

            Session["dt_doc"] = dt_doc_add;

            fill_rpt(dt_doc_add, 1, Convert.ToInt32(drp_countD.SelectedValue));

            Clear_documnt();
            Upd_docadd.Update();
            Upd_doc.Update();
        }

        public DataTable fill_Detail()
        {
            DataTable dtdoc = (DataTable)Session["dt_doc"];
            try
            {
                dtdoc.Columns.Remove("dt_indx");
                dtdoc.Columns.Remove("doc_type");
            }
            catch { }

            return dtdoc;
        }

        //filter Search
        protected void txt_doc_search_OnTextChanged(object sender, EventArgs e)
        {
            DataTable dtnin = (DataTable)Session["dt_doc"];

            DataTable dh = new DataTable();

            if (dtnin != null)
            {
                dh = dtnin.Clone();

                DataRow[] dr = dtnin.Select("Document_name LIKE '%" + txt_search_doc.Text + "%' or doc_type LIKE '%" + txt_search_doc.Text + "%' or DocNumber like '%" + txt_search_doc.Text + "%'");
                int cv = dr.Length;
                if (cv > 0)
                {
                    dh = dr.CopyToDataTable();
                    rpt_doc_list.DataSource = dh;
                    rpt_doc_list.DataBind();
                    fill_rpt(dh, 1, Convert.ToInt32(drp_countD.SelectedValue));
                }
                else
                {
                    rpt_doc_list.DataSource = dh;
                    rpt_doc_list.DataBind();
                    fill_rpt(dh, 1, Convert.ToInt32(drp_countD.SelectedValue));
                }
            }
        }

        public void fill_rpt(DataTable dt_doc, int PageNo, int count)
        {
            int Current_count = dt_doc.Rows.Count;
            int last_page = Current_count / count;
            int start_number = (PageNo - 1) * count + 1;
            int end_num = PageNo * count;
            int last_page_reminder = Current_count % count;
            if (last_page_reminder != 0)
            {
                last_page = last_page + 1;
            }

            DataTable dh = new DataTable();
            dh = dt_doc.Clone();

            foreach (DataRow rows in dt_doc.Rows)
            {
                if (Convert.ToInt32(rows["dt_indx"]) >= start_number && Convert.ToInt32(rows["dt_indx"]) <= end_num)
                {
                    dh.Rows.Add(Convert.ToInt32(rows["dt_indx"]), Convert.ToInt32(rows["id"]), rows["DocNumber"].ToString(), Convert.ToInt32(rows["DocumentTypeId"]),
                           rows["doc_type"].ToString(), rows["DocumentName"].ToString(), rows["DocumentSave"].ToString(),
                          rows["Valid_From"].ToString() == "" ? null : rows["Valid_From"].ToString(), rows["Valid_To"].ToString() == "" ? null : rows["Valid_To"].ToString(),
                          rows["Document_name"].ToString(), rows["Remark"].ToString(), rows["ValidityYear"].ToString() == "" ? (int?)null : Convert.ToInt32(rows["ValidityYear"]));
                }
            }
            rpt_doc_list.DataSource = dh;
            rpt_doc_list.DataBind();

            if (dh.Rows.Count > 0)
            {
                lbl_page_infoD.Text = "Showing Results " + start_number.ToString() + " - " + dh.Rows[dh.Rows.Count - 1]["dt_indx"].ToString() + " Out of " + Current_count.ToString() + " Records";
                hdn_last_pageD.Value = last_page.ToString();
                lbl_page_numberD.Text = PageNo.ToString();
                hdn_totalD.Value = Current_count.ToString();
            }
            else
            {
                lbl_page_infoD.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
                hdn_last_pageD.Value = "0";
                lbl_page_numberD.Text = "1";
                hdn_totalD.Value = "0";
            }
            Upd_Nav_Doc.Update();
            Upd_doc.Update();
        }

        #region Navigation Doc

        //First Page
        protected void btn_first1_OnClick(object sender, EventArgs e)
        {
            DataTable dt_doc = (DataTable)Session["dt_doc"];
            fill_rpt(dt_doc, 1, Convert.ToInt32(drp_countD.SelectedValue));
        }

        //Previous Page
        protected void btn_prev1_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_numberD.Text) > 1)
            {
                DataTable dt_doc = (DataTable)Session["dt_doc"];
                fill_rpt(dt_doc, Convert.ToInt32(lbl_page_numberD.Text) - 1, Convert.ToInt32(drp_countD.SelectedValue));
            }
        }

        //Next Page
        protected void btn_next1_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_numberD.Text) < Convert.ToInt32(hdn_last_pageD.Value))
            {
                DataTable dt_doc = (DataTable)Session["dt_doc"];
                fill_rpt(dt_doc, Convert.ToInt32(lbl_page_numberD.Text) + 1, Convert.ToInt32(drp_countD.SelectedValue));
            }
        }

        //Last Page
        protected void btn_last1_OnClick(object sender, EventArgs e)
        {
            DataTable dt_doc = (DataTable)Session["dt_doc"];
            fill_rpt(dt_doc, Convert.ToInt32(hdn_last_pageD.Value), Convert.ToInt32(drp_countD.SelectedValue));
        }

        //Page Data Count
        protected void drp_countD_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt_doc = (DataTable)Session["dt_doc"];
            fill_rpt(dt_doc, 1, Convert.ToInt32(drp_countD.SelectedValue));
        }

        #endregion

        #endregion

        #region Navigation

        //First Page
        protected void btn_first_OnClick(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
            Upd_List_Panel.Update();
        }

        //Previous Page
        protected void btn_prev_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number.Text) > 1)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text) - 1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Upd_List_Panel.Update();
            }
        }

        //Next Page
        protected void btn_next_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number.Text) < Convert.ToInt32(hdn_last_page.Value))
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text) + 1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Upd_List_Panel.Update();
            }
        }

        //Last Page
        protected void btn_last_OnClick(object sender, EventArgs e)
        {

            grid_fill(Convert.ToInt32(hdn_last_page.Value), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
            Upd_List_Panel.Update();
        }

        //Page Data Count
        protected void drp_count_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
            Upd_List_Panel.Update();
        }

        #endregion

        //Calculate Date
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

        //Check Privilege
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.Form_Previlage_Validation(106, Convert.ToInt32(hdn_user_id.Value));
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

                    DataTable dt = obj_common.Action_Previlage_Validation(106, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdn_update.Value = dt.Rows[1][1].ToString();
                        hdn_delete.Value = dt.Rows[2][1].ToString();
                        hdn_doc.Value = dt.Rows[3][1].ToString();
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
    }
}