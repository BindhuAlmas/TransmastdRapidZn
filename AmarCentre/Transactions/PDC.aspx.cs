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

namespace AmarCentre.Transactions
{
    public partial class PDC : System.Web.UI.Page
    {
        Master_Bal obj_master = new Master_Bal();
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
                fillBank();
                ClearReceipt();
                ClearPayment();
                FillList();
            }
        }

        protected void drpStatusOnSelectedIndexChanged(object sender, EventArgs e)
        {
            FillList();
        }

        public void fillBank()
        {
            drpBankAccount.DataSource = BalVoucher.GetBankAccountList(Convert.ToInt32(hdn_user_id.Value));
            drpBankAccount.DataValueField = "Value";
            drpBankAccount.DataTextField = "Text";
            drpBankAccount.DataBind();
            drpBankAccount.Visible = true;
           
        }
        protected void listAction(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;
            Label ChequeDate = (Label)itemrp.FindControl("ChequeDate");
            Label PaidFrom = (Label)itemrp.FindControl("PaidFrom");
            Label Receiver = (Label)itemrp.FindControl("Receiver");
            Label Amount = (Label)itemrp.FindControl("Amount");
            Label Type = (Label)itemrp.FindControl("Type");
            HiddenField hdn_id = (HiddenField)itemrp.FindControl("hdn_id");
            HiddenField hdnaftercom = (HiddenField)itemrp.FindControl("hdnaftercom");
            HiddenField hdnTypeId = (HiddenField)itemrp.FindControl("hdnTypeId");
            if (hdnTypeId.Value == "3")
            {
                lblChequeDate.Text = ChequeDate.Text;
                lblFrom.Text = Receiver.Text;
                lblAmount.Text = Amount.Text;
                lblAmountAfterComm.Text = hdnaftercom.Value;
                lblfromBank.Text = PaidFrom.Text;
                dtCollectionDate.DbSelectedDate = null;
                hdnId.Value = hdn_id.Value;
                pnl_add.Visible = true;
                Upd_Add_Panel.Update();
            }
            else
            {
                lblRcvChequeDate.Text = ChequeDate.Text;
                lblRcvFrom.Text = PaidFrom.Text;
                lblRcvAmount.Text = Amount.Text;
                pnlReceipt.Visible = true;
                hdnRecId.Value = hdn_id.Value;
                drpBankAccount.ClearSelection();
                drpBankAccount.Text = "";
                dtRcvCollectionDate.DbSelectedDate = null;
                hdnRecTypeId.Value = hdnTypeId.Value;
                updUpdateReceiptCheque.Update();

            }
        }

        public void FillList()
        {
            DataTable dt = BalVoucher.PDCList(drpStatus.SelectedValue==""?1:Convert.ToInt32(drpStatus.SelectedValue),txt_search.Text);
            rpt_list.DataSource = dt;
            rpt_list.DataBind();
            Upd_List_Panel.Update();
        }

        protected void ClosePaymentCheque(object sender,EventArgs e)
        {
            int res=BalVoucher.ClosingPaymentCheque(Convert.ToInt32( hdnId.Value), DateTime.ParseExact(CalDate(dtCollectionDate), "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Convert.ToInt32(hdn_user_id.Value));
            if (res ==1)
            {
                FillList();
                Upd_List_Panel.Update();
                ClearPayment();
                lbl_msgPayment.Text = "Saved Successfully !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            else
            {
                lbl_msgPayment.Text = "Sorry Failed to Process Your Request !..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
            }
            pnl_add.Visible = false;
            Upd_Add_Panel.Update();
            
        }
        protected void CloseReceiptCheque(object sender, EventArgs e)
        {
            if (hdnRecTypeId.Value == "2")
            {

                int res = BalVoucher.ClosingReceiptVoucherCheque(Convert.ToInt32(hdnRecId.Value), Convert.ToInt32(drpBankAccount.SelectedValue), DateTime.ParseExact(CalDate(dtRcvCollectionDate), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Convert.ToInt32(hdn_user_id.Value));
                if (res == 1)
                {
                    FillList();
                    Upd_List_Panel.Update();
                    ClearReceipt();
                    lbl_msgReceipt.Text = "Saved Successfully !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                }
                else
                {
                    lbl_msgReceipt.Text = "Sorry Failed to Process Your Request !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                }
                pnlReceipt.Visible = false;
                updUpdateReceiptCheque.Update();
            }
            else {
                int res = BalVoucher.ClosingReceiptCheque(Convert.ToInt32(hdnRecId.Value), Convert.ToInt32(drpBankAccount.SelectedValue), DateTime.ParseExact(CalDate(dtRcvCollectionDate), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Convert.ToInt32(hdn_user_id.Value));
                if (res == 1)
                {
                    FillList();
                    Upd_List_Panel.Update();
                    ClearReceipt();
                    lbl_msgReceipt.Text = "Saved Successfully !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                }
                else
                {
                    lbl_msgReceipt.Text = "Sorry Failed to Process Your Request !..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "ToggleDiv();", true);
                }
                pnlReceipt.Visible = false;
                updUpdateReceiptCheque.Update();
            }
        }
        public void btnexcel_export_OnClick(object sender, EventArgs e)
        {
            DataTable dt = BalVoucher.PDCList(1,"");
            //dt.Columns["Sl_No"].ColumnName = "Sl No.";
            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "PDC");

                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                HttpContext.Current.Response.Write(style);
                Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        protected void btn_newentry_OnClick(object sender, EventArgs e)
        {
           
            pnl_add.Visible = true;
            Upd_Add_Panel.Update();
        }

        protected void btnPaymentClose_OnClick(object sender, EventArgs e)
        {
            pnl_add.Visible = false;
            Upd_Add_Panel.Update();
        }

        protected void btnReceiptClose_OnClick(object sender, EventArgs e)
        {
            pnlReceipt.Visible = false;
            updUpdateReceiptCheque.Update();
        }

        public void ClearPayment()
        {
            hdnId.Value = "0";
            lblChequeDate.Text = "";
            lblFrom.Text = "";
            lblAmount.Text = "";
            lblAmountAfterComm.Text = "";
            lblfromBank.Text = "";
            dtCollectionDate.DbSelectedDate = "";

            btn_save.Visible = hdn_add.Value == "0" ? false : true;
            Upd_Add_Panel.Update();
        }

        public void ClearReceipt()
        {
            hdnRecId.Value = "0";
            lblRcvChequeDate.Text = "";
            lblRcvFrom.Text = "";
            lblRcvAmount.Text = "";
            dtRcvCollectionDate.DbSelectedDate = "";
            drpBankAccount.ClearSelection();
            drpBankAccount.Text = "";

            btnSaveReceiptCheque.Visible = hdn_add.Value == "0" ? false : true;
            updUpdateReceiptCheque.Update();
        }

            //Check Privilege
            public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.Form_Previlage_Validation(26, Convert.ToInt32(hdn_user_id.Value));
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

        /*Check Action Privilege*/
        public void previlage_action_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    DataTable dt = obj_common.Action_Previlage_Validation(26, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows.Count > 0)
                    {
                        hdn_add.Value = dt.Rows[0][1].ToString();
                    }
                    btn_save.Visible = hdn_add.Value == "0" ? false : true;
                    btnSaveReceiptCheque.Visible = hdn_add.Value == "0" ? false : true;
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