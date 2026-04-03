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
using System.Data.OleDb;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text.RegularExpressions;
using SautinSoft;
using System.Xml.Xsl;
using System.Xml;

namespace AmarCentre.Transactions
{
    public partial class BankReconciliation : System.Web.UI.Page
    {
        Transaction_Bal obj_trans = new Transaction_Bal();
        System_Utilities obj_common = new System_Utilities();
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
                fillBankAccount();
                Clear();
                grid_fill(1, 10, "", "", "");
            }
        }

        //Get List 
        public void grid_fill(int page_number, int page_size, string filter, string column, string order)
        {
            DataTable dt = obj_trans.Get_List_BankReconciliation(page_number, page_size, filter, column, order);
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

        /*Export To Excel*/
        public void btnexcel_export_OnClick(object sender, EventArgs e)
        {
            DataTable dt = obj_trans.Get_List_BankReconciliation_Excel();
            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "BankReconciliation");
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                HttpContext.Current.Response.Write(style);
                Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        public void fillBankAccount()
        {
            drpBankAccount.Items.Clear();
            DataTable dt = BalVoucher.GetBankAccountList(Convert.ToInt32(hdn_user_id.Value));
            drpBankAccount.DataSource = dt;
            drpBankAccount.DataTextField = "Text";
            drpBankAccount.DataValueField = "Value";
            drpBankAccount.DataBind();
        }

        /*rpt_list OnItemCommand*/
        protected void rpt_list_OnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            Clear();
            pnl_add.Visible = true;
            HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdn_id");
            DataSet ds = obj_trans.Edit_BankReconciliation(Convert.ToInt32(hdn_rpt_id.Value));
            DataTable dt1 = ds.Tables[0];
            DataTable dtDetail = ds.Tables[1];/* Detail*/
            DataTable dtsum = ds.Tables[2];

            hdn_id.Value = dt1.Rows[0]["Id"].ToString();
            lbl_Code.Text = dt1.Rows[0]["Code"].ToString();
            txtFromDate.DbSelectedDate = dt1.Rows[0]["FromDate"].ToString();
            txtToDate.DbSelectedDate = dt1.Rows[0]["ToDate"].ToString();
            drpBankAccount.SelectedValue = dt1.Rows[0]["BankAccountId"].ToString();
            drpBankAccount.Enabled = false;
            hdnfileName.Value = dt1.Rows[0]["FileName"].ToString();
            hdnfileSaveName.Value = dt1.Rows[0]["FileSavedName"].ToString();
            hdnfileExtension.Value = dt1.Rows[0]["FileExtension"].ToString();
            rpt_Item_list.DataSource = dtDetail;
            rpt_Item_list.DataBind();

            lblBSAmount.Text = dtsum.Rows[0]["BankStatementAmount"].ToString();
            lblTransmasAmount.Text = dtsum.Rows[0]["ApplicationAmount"].ToString();
            lblTransmasAmountDiff.Text = dtsum.Rows[0]["ApplicationAmountDifference"].ToString();
            lblBSAmountDiff.Text = dtsum.Rows[0]["BankStatementAmountDifference"].ToString();

            btn_save.Visible = hdn_update.Value == "0" ? false : true;
            btnDelete.Visible = hdn_delete.Value == "0" ? false : true;

            Upd_Add_Panel.Update();
        }

        /*Save*/
        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            DataTable dt_details = fill_Detail();

            int res = 0;
            if (dt_details.Rows.Count > 0)
            {
                res = obj_trans.Insert_Update_BankReconciliation(Convert.ToInt32(hdn_id.Value),
                    DateTime.ParseExact(CalDate(txtFromDate), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    DateTime.ParseExact(CalDate(txtToDate), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Convert.ToInt32(drpBankAccount.SelectedValue), hdnfileName.Value, hdnfileSaveName.Value,
                    hdnfileExtension.Value,dt_details, Convert.ToInt32(hdn_user_id.Value));
            }
            else
            {
                lbl_msgin.Text = "Add Reconciliation to Continue !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            if (res > 0)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                lbl_msgin.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msgin.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            Upd_Add_PanelInner.Update();
        }


        /*Data To Save*/
        public DataTable fill_Detail()
        {
            DataTable ContentTable = null;
            ContentTable = new DataTable();
            ContentTable.Columns.Add("Date", typeof(DateTime));
            ContentTable.Columns.Add("DisplayDate", typeof(string));
            ContentTable.Columns.Add("TransactionId", typeof(string));
            ContentTable.Columns.Add("Comment", typeof(string));
            ContentTable.Columns.Add("BankStatementAmount", typeof(string));
            ContentTable.Columns.Add("ApplicationAmount ", typeof(string));
            ContentTable.Columns.Add("ApplicationAmountDifference", typeof(string));
            ContentTable.Columns.Add("BankStatementAmountDifference", typeof(string));
            foreach (RepeaterItem item in rpt_Item_list.Items)
            {
                HiddenField hdnDate = (HiddenField)item.FindControl("hdnDate");
                Label lblDisplayDate = (Label)item.FindControl("lblDisplayDate");
                Label lblTransID = (Label)item.FindControl("lblTransID");
                Label lblComment = (Label)item.FindControl("lblComment");
                Label lblBankStatementAmount = (Label)item.FindControl("lblBankStatementAmount");
                Label lblApplicationAmount = (Label)item.FindControl("lblApplicationAmount");
                Label lblApplicationAmountDifference = (Label)item.FindControl("lblApplicationAmountDifference");
                Label lblBankStatementAmountDifference = (Label)item.FindControl("lblBankStatementAmountDifference");

                ContentTable.Rows.Add(hdnDate.Value, lblDisplayDate.Text, lblTransID.Text, lblComment.Text,
                    lblBankStatementAmount.Text == "" ? null : lblBankStatementAmount.Text,
                    lblApplicationAmount.Text == "" ? null :lblApplicationAmount.Text,
                    lblApplicationAmountDifference.Text == "" ?null :lblApplicationAmountDifference.Text,
                    lblBankStatementAmountDifference.Text == "" ? null : lblBankStatementAmountDifference.Text);
            }
            return ContentTable;
        }

        protected void btnDelete_OnClick(object sender, EventArgs e)
        {
            int res = obj_trans.Delete_BankReconciliation(Convert.ToInt32(hdn_id.Value), Convert.ToInt32(hdn_user_id.Value));
            if (res > 0)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text), Convert.ToInt32(drp_count.SelectedValue), hdn_filter.Value, Common_order_column.Value, Common_asc_desc.Value);
                Clear();
                lbl_msgin.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msgin.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            //pnl_add.Visible = false;
            Upd_Add_PanelInner.Update();
        }

        /*Reset*/
        protected void btn_reset_OnClick(object sender, EventArgs e)
        {
            Clear();
        }

       
        public void fuDocumentOnFileUploaded(object sender, FileUploadedEventArgs e)
        {

            fuDocument.TargetFolder = "~/UploadedFiles";
            DataTable dt = obj_common.Get_File_Code("Bank Reconciliation Document");
            if (dt.Rows.Count > 0)
            {
                hdnfileSaveName.Value = dt.Rows[0][0].ToString() + e.File.GetNameWithoutExtension() + e.File.GetExtension();
                hdnfileName.Value = e.File.GetNameWithoutExtension() + e.File.GetExtension();
                hdnfileExtension.Value = e.File.GetExtension();
                e.File.SaveAs(Path.Combine(Server.MapPath(fuDocument.TargetFolder), hdnfileSaveName.Value));

            }

            if (hdnfileExtension.Value == ".pdf")
            {
                string fileName = Path.Combine(Server.MapPath(fuDocument.TargetFolder), hdnfileSaveName.Value);

                SautinSoft.PdfFocus f = new PdfFocus();
                f.OpenPdf(fileName);

                if (f.PageCount > 0)
                {
                    hdnfileSaveName.Value = dt.Rows[0][0].ToString() + e.File.GetNameWithoutExtension() + ".xml";
                    hdnfileName.Value = e.File.GetNameWithoutExtension() + ".xml";
                    hdnfileExtension.Value = ".xml";
                    fileName = Path.Combine(Server.MapPath(fuDocument.TargetFolder), hdnfileSaveName.Value);
                    f.ToXml(fileName);
                }
            }
            
            UpdDocument.Update();
        }

        public string stringReverseString1(string str)
        {
            string[] arry = str.Split('-');
            string[] arrynew = new string[3];

            arrynew[2] = arry[0];
            arrynew[1] = arry[1];
            arrynew[0] = arry[2];

            string gg = arrynew[0] + "-" + arrynew[1] + "-" + arrynew[2];
            return gg;
        }

        public DataTable ReadFile(string filename, string extrn)
        {
            string connString = "";
            DataTable ContentTable = null;
            ContentTable = new DataTable();
            ContentTable.Columns.Add("TransID", typeof(string));
            ContentTable.Columns.Add("Date", typeof(DateTime));
            //ContentTable.Columns.Add("CustomerName", typeof(string));
            //ContentTable.Columns.Add("Type", typeof(string));
            ContentTable.Columns.Add("Amount", typeof(decimal));
            ContentTable.Columns.Add("Comm", typeof(decimal));
            ContentTable.Columns.Add("VAT", typeof(decimal));

            if (hdnfileExtension.Value == ".xml")
            {
                string fileName = Path.Combine(Server.MapPath(fuDocument.TargetFolder), hdnfileSaveName.Value);

                XmlReader reader = XmlReader.Create(fileName);
                int  t = 0, r = 0, c = 0;
                string TransID = "", Date3 = "", Amount = "", Comm = "", VAT = "";

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        string rowtype = reader.Name;
                        if (rowtype == "page")
                        {
                            r = c = 0;
                            TransID = ""; Date3 = ""; Amount = ""; Comm = ""; VAT = "";
                        }
                        else if (rowtype == "table")
                        {
                            t = t + 1;
                            r = 0;
                        }
                        else if (rowtype == "row")
                        {
                            r = r + 1;
                            c = 0;
                        }
                        else if (rowtype == "cell" && r > 1 && t == 6)
                        {
                            c = c + 1;
                            if (c == 2)
                                Date3 = reader.ReadString();
                            else if (c == 8)
                                Amount = reader.ReadString();
                            else if (c == 9)
                                Comm = reader.ReadString();
                            else if (c == 10)
                                VAT = reader.ReadString();
                            else if (c == 13)
                                TransID = reader.ReadString();
                        }
                        else if (rowtype == "cell" && r > 0 && t > 6)
                        {
                            c = c + 1;
                            if (c == 2)
                                Date3 = reader.ReadString();
                            else if (c == 8)
                                Amount = reader.ReadString();
                            else if (c == 9)
                                Comm = reader.ReadString();
                            else if (c == 10)
                                VAT = reader.ReadString();
                            else if (c == 13)
                                TransID = reader.ReadString();
                        }
                        if (c == 14 && Amount != "" && TransID != "" && r > 0 && t > 0)
                        {
                            string kkkk = Date3.Substring(0, 10);
                            string lk = stringReverseString1(kkkk.Replace('/', '-'));
                            DateTime oDate = DateTime.Parse(lk);
                            ContentTable.Rows.Add(TransID, oDate, Amount, Comm, VAT);
                            TransID = ""; Date3 = ""; Amount = ""; Comm = ""; VAT = "";
                        }
                    }
                }

                return ContentTable;
            }
            else
            {

                if (extrn == ".xls")
                {
                    //Connectionstring for excel v8.0    
                    connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1;TypeGuessRows=0\"";
                }
                else
                {
                    //Connectionstring fo excel v12.0    
                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1;TypeGuessRows=0\"";
                }
                OleDbConnection OledbConn = new OleDbConnection(connString);
                try
                {

                    OleDbCommand OledbCmd = new OleDbCommand();
                    OledbCmd.Connection = OledbConn;
                    OledbConn.Open();
                    var sheetNames = OledbConn.GetSchema("Tables");

                    OledbCmd.CommandText = "Select * from [" + sheetNames.Rows[0]["TABLE_NAME"].ToString() + "]";
                    OleDbDataReader dr = OledbCmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                           
                            try
                            {
                                if (dr["PRAN No"].ToString().Trim() != string.Empty && dr["Date"].ToString().Trim() != string.Empty && dr["Amount"].ToString().Trim() != string.Empty && dr["Comm"].ToString().Trim() != string.Empty && dr["VAT"].ToString().Trim() != string.Empty
                                   && dr["PRAN No"].ToString().Trim() != " " && dr["Date"].ToString().Trim() != " " && dr["Amount"].ToString().Trim() != " " && dr["Comm"].ToString().Trim() != " " && dr["VAT"].ToString().Trim() != " ")
                                    ContentTable.Rows.Add(dr["PRAN No"].ToString().Trim(), dr["Date"].ToString().Trim(),
                                        Convert.ToDecimal(dr["Amount"].ToString().Trim()), Convert.ToDecimal(dr["Comm"].ToString().Trim()), Convert.ToDecimal(dr["VAT"].ToString().Trim()));
                            }
                            catch (Exception ex) {
                                try
                                {
                                    if (dr["TransID"].ToString().Trim() != string.Empty && dr["Date"].ToString().Trim() != string.Empty && dr["Type"].ToString().Trim() != string.Empty && dr["Amount"].ToString().Trim() != string.Empty && dr["Comm"].ToString().Trim() != string.Empty && dr["VAT"].ToString().Trim() != string.Empty
                                        && dr["TransID"].ToString().Trim() != " " && dr["Date"].ToString().Trim() != " " && dr["Type"].ToString().Trim() != " " && dr["Amount"].ToString().Trim() != " " && dr["Comm"].ToString().Trim() != " " && dr["VAT"].ToString().Trim() != " ")
                                        ContentTable.Rows.Add(dr["TransID"].ToString().Trim(), dr["Date"].ToString().Trim(), dr["Customer Name"].ToString(), dr["Type"].ToString(),
                                            Convert.ToDecimal(dr["Amount"].ToString().Trim()), Convert.ToDecimal(dr["Comm"].ToString().Trim()), Convert.ToDecimal(dr["VAT"].ToString().Trim()));
                                }
                                catch (Exception ex1)
                                {
                                }
                            }
                        }
                    }
                    dr.Close();

                    OledbConn.Close();
                    return ContentTable;
                }
                catch (Exception ex)
                {
                    //OledbConn.Close();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please upload the correct file format');", true);
                    return ContentTable;
                }
            }
        }

        public void btnexcel_exportList_OnClick(object sender, EventArgs e)
        {
            DataTable ContentTable = null;
            ContentTable = new DataTable();
            ContentTable.Columns.Add("SlNo", typeof(string));
            ContentTable.Columns.Add("Date", typeof(string));
            ContentTable.Columns.Add("Transaction Id/PRAN", typeof(string));
            ContentTable.Columns.Add("Comment", typeof(string));
            ContentTable.Columns.Add("BankStatement Amount", typeof(string));
            ContentTable.Columns.Add("Transmas Amount ", typeof(string));
            ContentTable.Columns.Add("Transmas Amount Difference", typeof(string));
            ContentTable.Columns.Add("BankStatement Amount Difference", typeof(string));

            DataTable ContentSumTable = null;
            ContentSumTable = new DataTable();
            ContentSumTable.Columns.Add("Total BankStatement Amount", typeof(string));
            ContentSumTable.Columns.Add("Total Transmas Amount ", typeof(string));
            ContentSumTable.Columns.Add("Total Transmas Amount Difference", typeof(string));
            ContentSumTable.Columns.Add("Total BankStatement Amount Difference", typeof(string));
            int i = 1;
            foreach (RepeaterItem item in rpt_Item_list.Items)
            {
                Label lblDisplayDate = (Label)item.FindControl("lblDisplayDate");
                Label lblTransID = (Label)item.FindControl("lblTransID");
                Label lblComment = (Label)item.FindControl("lblComment");
                Label lblBankStatementAmount = (Label)item.FindControl("lblBankStatementAmount");
                Label lblApplicationAmount = (Label)item.FindControl("lblApplicationAmount");
                Label lblApplicationAmountDifference = (Label)item.FindControl("lblApplicationAmountDifference");
                Label lblBankStatementAmountDifference = (Label)item.FindControl("lblBankStatementAmountDifference");

                ContentTable.Rows.Add(i.ToString(), lblDisplayDate.Text, lblTransID.Text, lblComment.Text, lblBankStatementAmount.Text,
                    lblApplicationAmount.Text, lblApplicationAmountDifference.Text, lblBankStatementAmountDifference.Text);
                i++;
            }
            if (rpt_Item_list.Items.Count > 0)
            {
                ContentSumTable.Rows.Add(lblBSAmount.Text,lblTransmasAmount.Text,lblTransmasAmountDiff.Text,lblBSAmountDiff.Text);
            }

            if (ContentTable.Rows.Count > 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=BankReconciliation.xls");
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.RowDataBound += new GridViewRowEventHandler(GridView1RowDataBound);
                GridView1.DataSource = ContentTable;
                GridView1.DataBind();



                for (int Row = 0; Row < GridView1.Rows.Count; Row++)
                {
                    //Apply text style to each Row
                    GridView1.Rows[Row].Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);

                if (ContentSumTable.Rows.Count > 0)
                {
                    GridView g3 = new GridView();
                    g3.AllowPaging = false;
                    g3.DataSource = ContentSumTable;
                    g3.DataBind();
                    g3.HeaderRow.Style.Add("background-color", "#ccc");
                    for (int Row = 0; Row < g3.Rows.Count; Row++)
                    {
                        //Apply text style to each Row
                        g3.Rows[Row].Attributes.Add("class", "textmode");

                    }
                    g3.RenderControl(hw);

                }
                string style = @"<style> .textmode { mso-number-format:\@; word-wrap: break-word; } </style>";
                HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        protected void btnProcessOnClick(object sender, EventArgs e)
        {
            if (hdnfileSaveName.Value != "")
            {
                DataSet ds = obj_trans.GetBankReconciliation(DateTime.ParseExact(CalDate(txtFromDate), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    DateTime.ParseExact(CalDate(txtToDate), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Convert.ToInt32(drpBankAccount.SelectedValue), ReadFile(Path.Combine(Server.MapPath("~/UploadedFiles"), hdnfileSaveName.Value), hdnfileExtension.Value));
                DataTable dtDetail = ds.Tables[0];
                DataTable dtsum = ds.Tables[1];
                rpt_Item_list.DataSource = dtDetail;
                rpt_Item_list.DataBind();
                if (dtsum.Rows.Count > 0)
                {
                    lblBSAmount.Text = dtsum.Rows[0]["BankStatementAmount"].ToString();
                    lblTransmasAmount.Text = dtsum.Rows[0]["ApplicationAmount"].ToString();
                    lblTransmasAmountDiff.Text = dtsum.Rows[0]["ApplicationAmountDifference"].ToString();
                    lblBSAmountDiff.Text = dtsum.Rows[0]["BankStatementAmountDifference"].ToString();
                }
                else
                {
                    lblBSAmount.Text = "";
                    lblTransmasAmount.Text = "";
                    lblTransmasAmountDiff.Text = "";
                    lblBSAmountDiff.Text = "";
                }
                Upd_ItemList.Update();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please upload the file');", true);
            }
        }

        void GridView1RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Text = String.Format("&nbsp;{0}&nbsp;", e.Row.Cells[2].Text);
            }
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

        /*Clear All the Data*/
        public void Clear()
        {
            hdn_id.Value = "0";
            drpBankAccount.ClearSelection();
            drpBankAccount.Text = "";
            drpBankAccount.Enabled = true;
            txtFromDate.DbSelectedDate = "";
            txtToDate.DbSelectedDate = "";
            hdnfileName.Value = "";
            hdnfileSaveName.Value = "";
            hdnfileExtension.Value = "";
            DataTable dtDetails = new DataTable();
            dtDetails.Columns.Add("Date", typeof(DateTime));
            dtDetails.Columns.Add("Remarks", typeof(string));
            dtDetails.Columns.Add("TransactionNo", typeof(string));
            dtDetails.Columns.Add("Debit", typeof(decimal));
            dtDetails.Columns.Add("Credit", typeof(decimal));
            dtDetails.Columns.Add("Balance", typeof(decimal));

            rpt_Item_list.DataSource = dtDetails;
            rpt_Item_list.DataBind();

            lblBSAmount.Text = "";
            lblTransmasAmount.Text = "";
            lblTransmasAmountDiff.Text = "";
            lblBSAmountDiff.Text = "";

            btn_save.Visible = hdn_add.Value == "0" ? false : true;
            btnDelete.Visible = false;
            Get_Code();

            Upd_Add_PanelInner.Update();
        }

        /*Code for Display*/
        public void Get_Code()
        {
            DataTable dt = obj_common.Get_Code(60);
            if (dt.Rows.Count > 0)
                lbl_Code.Text = dt.Rows[0][0].ToString();
        }

        /*Check Action Privilege*/
        public void previlage_action_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    DataTable dt = obj_common.Action_Previlage_Validation(60, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                        hdn_update.Value = dt.Rows[1][1].ToString();
                        hdn_delete.Value = dt.Rows[2][1].ToString();
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

        /*Check Form Privilege*/
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.Form_Previlage_Validation(60, Convert.ToInt32(hdn_user_id.Value));
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