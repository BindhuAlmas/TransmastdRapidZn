using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using AmarCentre.BAL;
using System.Web.UI.DataVisualization.Charting;
using AmarCentre.Layout;
using System.Drawing;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using WAImplementation.Library;
using WAImplementation;
using System.Threading.Tasks;
using System.Web.Configuration;
using AmarCentre.WAImplementation;
using System.Threading;

namespace AmarCentre
{
    public partial class WhatsappmessagePage : System.Web.UI.Page
    {
        Master_Bal obj_master = new Master_Bal();
        Transaction_Bal obj_trans = new Transaction_Bal();
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
                
                AccountBalanceofMsg();
            }
        }
        
        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    LoadMessages();
        //}

        protected void gvMessages_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvMessages_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        

        protected async void btnSearch_Click(object sender, EventArgs e)
        {
            await LoadMessages();
        }
        protected async void gvMessages_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvMessages.PageIndex = e.NewPageIndex;
            //await LoadMessages(); // Rebind your data
        }

        private async Task LoadMessages()
        {
            DateTimeOffset startDateTime = DateTimeOffset.Parse(txtFromDate.Text);
            DateTimeOffset endDateTime = DateTimeOffset.Parse(txtToDate.Text);

            string docTypes = Request.Form[ddlDocType.UniqueID];
            string status = ddlStatus.SelectedValue;

            string keywords = "";

            //if (!string.IsNullOrEmpty(docTypes))
            //    keywords += docTypes + " ";

            //if (!string.IsNullOrEmpty(status) && status != "All")
            //    keywords += status;

            keywords = keywords.Trim();
            string baseurl = WebConfigurationManager.AppSettings["WABaseUrl"].ToString();
            string wakey = WebConfigurationManager.AppSettings["WAKey"].ToString();
            if (baseurl != string.Empty && wakey != string.Empty)
            {
                var response = await Mascom.Api.Client.Messages.GetMessageHistoryAsync(
                    WebConfigurationManager.AppSettings["WABaseUrl"].ToString(),//"https://wamanager.almasdemo.com",
                    WebConfigurationManager.AppSettings["WAKey"].ToString(),//"39729f7f0b8749fe9ecb93ea6d3997015c08bb4734d44061b75e896aa6be70c3" ,
                    startDateTime,
                    endDateTime,
                    keywords
                );

                if (response.Data != null)
                {
                    string json = response.Data.ToString();
                    List<SentHistory> sentHistoryList =
                        JsonConvert.DeserializeObject<List<SentHistory>>(json);
                    WABAL wabal = new WABAL();
                    DataTable dtMessageId = new DataTable();
                    dtMessageId.Columns.Add("MessageId", typeof(string));
                    sentHistoryList.ForEach(x =>
                    {
                        DataRow drow = dtMessageId.NewRow();
                        drow["MessageId"] = x.MessageId.ToString();
                        dtMessageId.Rows.Add(drow);
                    });
                    DataTable dtMessage = wabal.GetWAMessages(dtMessageId);
                    var list = dtMessage.AsEnumerable()
                 .Select(r => new
                 {
                     MessageId = r.Field<string>("MessageId"),
                     CustopmerName = r.Field<string>("customername"),
                     mobilenumber = r.Field<string>("mobilenumber"),
                     documenttype = r.Field<string>("documenttypename"),
                     documentdescription = r.Field<string>("documentdescription")
                 })
                 .ToList();

                    dtMessageId.Columns.Add("MessageDate", typeof(DateTime));
                    string selectedStatus = ddlStatus.SelectedValue;


                    var listresult =
        from message1 in sentHistoryList
        join message2 in list
            on message1.MessageId.ToString() equals message2.MessageId
        where selectedStatus == "All" ||
         (message1.LifecycleStatus.HasDelivered && selectedStatus == "Delivered") ||
         (message1.LifecycleStatus.HasRead && selectedStatus == "Read") ||
         (message1.LifecycleStatus.HasSent && selectedStatus == "Sent") ||
         (!message1.LifecycleStatus.HasDelivered &&
          !message1.LifecycleStatus.HasRead &&
          !message1.LifecycleStatus.HasSent &&
          selectedStatus == "Failed")

        orderby message1.MessageSentDate descending
        select new
        {
            MessageSentDate = message1.MessageSentDate.ToString("dd-MM-yyyy hh:mm tt"),
            message2.CustopmerName,
            message2.mobilenumber,
            message2.documenttype,
            message2.documentdescription,
            MessageId = message1.MessageId,
            Status =
                message1.LifecycleStatus.HasRead ? "Read" :
                message1.LifecycleStatus.HasDelivered ? "Delivered" :
                message1.LifecycleStatus.HasSent ? "Send" :
                "Failed",
            StatusClass =
                message1.LifecycleStatus.HasRead ? "read" :
                message1.LifecycleStatus.HasDelivered ? "delivered" :
                message1.LifecycleStatus.HasSent ? "sent" :
                "failed",
            deliverytime = Math.Round(message1.LifecycleStatus.HasDelivered == true ? (message1.LifecycleStatus.DeliveredDateTime - message1.LifecycleStatus.SentDateTime).TotalSeconds : 0)




        };
                    sentHistoryList.RemoveAll(s => !listresult.Any(l => l.MessageId.ToString() == s.MessageId.ToString()));

                    //list.ForEach(i =>
                    //{
                    //    sentHistoryList.ForEach(x =>
                    //    {
                    //        if(x.MessageId==i.MessageId)
                    //        {

                    //        }
                    //    })
                    //})
                    gvMessages2.DataSource = listresult;
                    gvMessages2.DataBind();
                    DataTable  dtErrorMessageCount = wabal.GetWACachedMessagesCount(startDateTime.DateTime, endDateTime.DateTime);
                    int errormessages = 0;
                    if(dtErrorMessageCount!=null)
                    {
                        if(dtErrorMessageCount.Rows.Count>0)
                        {
                            if(dtErrorMessageCount.Rows[0]["errormessagecount"]!=DBNull.Value)
                            {
                                errormessages = Convert.ToInt32(dtErrorMessageCount.Rows[0]["errormessagecount"]);
                            }
                        }
                    }
                    BindSummary(sentHistoryList,errormessages);
                    AccountBalanceofMsg();
                }
            }
        }
        private void BindSummary(List<SentHistory> data,int errormessagecount)
        {
            int total = data.Count;
            int delivered = data.Count(x => x.LifecycleStatus.HasDelivered == true && x.LifecycleStatus.HasRead == false);
            int read = data.Count(x => x.LifecycleStatus.HasRead == true);
            int failed = data.Count(x => x.LifecycleStatus.HasFailed == true);
            int pending = data.Count(x => x.LifecycleStatus.HasSent == false && x.LifecycleStatus.HasFailed==false);
            total = total;// + errormessagecount;
            // Trend logic (dummy for now)
            string up = "↑";
            string down = "↓";
            decimal totalmessage = 1;
            totalmessage = Convert.ToDecimal(total);
            if (totalmessage == 0)
                totalmessage = 1;
            decimal percent = ((decimal)(Convert.ToDecimal(delivered)/totalmessage) * 100);
            decimal percentread = ((decimal)(Convert.ToDecimal(read) / totalmessage) * 100);
            decimal percentfailed = ((decimal)(Convert.ToDecimal(failed) / totalmessage) * 100);
            decimal percentpending= ((decimal)(Convert.ToDecimal(pending) / totalmessage) * 100);

            var summary = new List<SummaryCard>
            { 
        new SummaryCard { Label = "Total", Value = total, Trend = "", TrendClass = "" },
        new SummaryCard { Label = "Delivered", Value = delivered, Trend = $"↑ {percent.ToString("0.##")}%", TrendClass = "trend-up" },
        new SummaryCard { Label = "Read", Value = read, Trend = $"↑ {percentread.ToString("0.##")}%", TrendClass = "trend-up" },
        new SummaryCard { Label = "Failed", Value = failed, Trend = $"↓ {percentfailed.ToString("0.##")}%", TrendClass = "trend-down" },
        new SummaryCard { Label = "Pending", Value = pending, Trend = $"↓ {percentpending.ToString("0.##")}%", TrendClass = "trend-down" },
        new SummaryCard { Label = "<div style='font-size:small;'>Waiting to be Sent</div>", Value = errormessagecount, Trend = $"<br><div style='font-size:smaller'>Please check your internet connection</div>", TrendClass = "" }
            };

            rptSummary.DataSource = summary;
            rptSummary.DataBind();
        }

        
        protected async void gvMessages_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            HiddenField hdnMessageId = (HiddenField)e.Item.FindControl("hdn_MessageId");
            if (e.CommandName == "SendAgain" || e.CommandName == "Retry")
            {
                DataTable dtMessage = obj_common.GetWhatsappMessageDelivered(hdnMessageId.Value);
                string[] parameters = dtMessage.Rows[0]["BodyParameters"].ToString().Split(',');

                bool success = await obj_common.SendWAMessage(
                    dtMessage.Rows[0]["RecipientPhoneNumber"].ToString(),
                    dtMessage.Rows[0]["TemplateCode"].ToString(),
                    parameters,
                    dtMessage.Rows[0]["MediaTypeName"].ToString(),
                    Convert.ToInt32(dtMessage.Rows[0]["Priority"].ToString()), dtMessage.Rows[0]["doctype"].ToString(), dtMessage.Rows[0]["documentdescription"].ToString(), dtMessage.Rows[0]["docexpirydate"]!=DBNull.Value?Convert.ToDateTime(dtMessage.Rows[0]["docexpirydate"]):(DateTime?)null
                );

                if (success)
                {

                    ScriptManager.RegisterStartupScript(this, GetType(), "msg",
    "Swal.fire('Success!', 'Message sent successfully!', 'success');", true);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg",
    "Swal.fire('Oops...', 'Message failed to send.', 'error');", true);
                }
                //await LoadMessages();
            }
            else if (e.CommandName == "Details")
            {
                // Build the URL for the details page, passing the MessageId as query string
                string url = "WhatsappDetailsPage.aspx?MessageId=" + hdnMessageId.Value;

                // Open in a new browser tab
                ScriptManager.RegisterStartupScript(this, GetType(), "newTab",
                    "window.open('" + url + "', '_blank');", true);
            }

        }
        private async void AccountBalanceofMsg()
        {
            string baseurl = WebConfigurationManager.AppSettings["WABaseUrl"].ToString();
            string wakey = WebConfigurationManager.AppSettings["WAKey"].ToString();
            if (baseurl != string.Empty && wakey != string.Empty)
            {
                var response2 = await Mascom.Api.Client.Messages.GetAccountBalance(baseurl, wakey);
                int balance = int.Parse(response2.Data.ToString());//whatsapp message balance
                lblremainingmsg.Text = balance.ToString();

            }

        }
        protected void gvMessages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void btn_first_Click(object sender, EventArgs e)
        {

        }

        protected void btn_prev_Click(object sender, EventArgs e)
        {

        }

        protected void btn_next_Click(object sender, EventArgs e)
        {

        }

        protected void btn_last_Click(object sender, EventArgs e)
        {

        }

        protected void drp_count_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //protected void btnSendAgain_Click(object sender, EventArgs e)
        //{
            
        //    HiddenField hdn_rpt_id = (HiddenField)e.Item.FindControl("hdn_MessageId");
        //    DataTable dtMessage=obj_common.GetWhatsappMessageDelivered()
        //}
        //private (string Trend, string TrendClass) GetTrend(int current, int previous)
        //{
        //    if (previous == 0)
        //    {
        //        if (current == 0)
        //            return ("0%", ""); // no change
        //        else
        //            return ("↑ 100%", "trend-up"); // from 0 to something
        //    }

        //    decimal percent = ((decimal)(current - previous) / previous) * 100;
        //    percent = Math.Round(percent, 1); // 1 decimal place

        //    if (percent > 0)
        //        return ($"↑ {percent}%", "trend-up");

        //    if (percent < 0)
        //        return ($"↓ {Math.Abs(percent)}%", "trend-down");

        //    return ("0%", "");
        //}

    }
}