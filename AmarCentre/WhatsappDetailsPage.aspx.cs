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
    public partial class WhatsappDetailsPage : System.Web.UI.Page
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
                string messageId = Request.QueryString["MessageId"];
                if (!string.IsNullOrEmpty(messageId))
                {
                    MessageDetails(messageId);
                }
            }
        }
        private async void MessageDetails(string messageId)
        {
            if (!string.IsNullOrEmpty(messageId))
            {
                Guid messageid = Guid.Parse(messageId);
                string baseurl = WebConfigurationManager.AppSettings["WABaseUrl"];
                string wakey = WebConfigurationManager.AppSettings["WAKey"];

                if (!string.IsNullOrEmpty(baseurl) && !string.IsNullOrEmpty(wakey))
                {
                    var response = await Mascom.Api.Client.Messages.GetMessageDetailsAsync(
                        baseurl,
                        wakey,
                        messageid);

                    if (response.Data != null)
                    {
                        string json = response.Data.ToString();
                        SentHistory sentHistoryList =
                            JsonConvert.DeserializeObject<SentHistory>(json);
                        // Assuming response.Data has properties like CustomerCode, CustomerName, etc.
                        lblCustomerCode.Text = sentHistoryList.CustomerCode;
                        lblCustomerName.Text = sentHistoryList.CustomerName;
                        //lblPhoneNumberId.Text = sentHistoryList.PhoneNumberId.ToString();
                        //lblPhoneNumber.Text = sentHistoryList.SenderPhoneNumber;
                        lblRecipientPhoneNumber.Text = sentHistoryList.RecipientPhoneNumber;
                        lblMessageContent.Text = sentHistoryList.MessageContent;
                        lblSentDate.Text = sentHistoryList.MessageSentDate.ToString("MM/dd/yyyy hh:mm:ss tt");

                        if (sentHistoryList.CurrentStatus.Equals("Success", StringComparison.OrdinalIgnoreCase))
                        {
                            lblSendStatus.Text = "<span class='status-success'>Success</span>";
                        }
                        else
                        {
                            lblSendStatus.Text = "<span class='status-failed'>Failed</span>";
                        }

                        lblMediaType.Text = sentHistoryList.MediaTypeName;
                    }
                }
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
        //private async void LoadMessages()
        //{
        //    //string fromDate = txtFromDate.Text;
        //    //string toDate = txtToDate.Text;
        //    //string docType = ddlDocType.SelectedValue;
        //    //string status = ddlStatus.SelectedValue;

        //    //using (HttpClient client = new HttpClient())
        //    //{
        //    //    client.BaseAddress = new Uri("https://yourapi.com/");

        //    //    var payload = new
        //    //    {
        //    //        FromDate = fromDate,
        //    //        ToDate = toDate,
        //    //        DocumentType = docType,
        //    //        Status = status
        //    //    };

        //    //    var json = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
        //    //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    //    HttpResponseMessage response = await client.PostAsync("api/whatsapp/filter", content);

        //    //    if (response.IsSuccessStatusCode)
        //    //    {
        //    //        string result = await response.Content.ReadAsStringAsync();

        //    //        var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MessageModel>>(result);

        //    //        gvMessages.DataSource = data;
        //    //        gvMessages.DataBind();

        //    //        //BindSummary(data);
        //    //    }
        //    //    else
        //    //    {
        //    //        ScriptManager.RegisterStartupScript(this, GetType(), "err",
        //    //            "alert('Unable to load data from API');", true);
        //    //    }
        //    //}
        //    DateTimeOffset startDateTime = DateTimeOffset.Parse("01/11/2025");
        //    DateTimeOffset endDateTime = DateTimeOffset.Parse("30/11/2025");

        //    string keywords = ""; //Provide keywords to search
        //    string sortKey = "CustomerId"; //See Text for Other Options
        //    int sortOrder = 1; //See text for Other Options
        //    long startRowNumber = 1;
        //    long endRowNumber = 100;

        //    var response = await Mascom.Api.Client.Messages.GetMessageHistoryAsync("https://wamanager.almasdemo.com",
        //        "39729f7f0b8749fe9ecb93ea6d3997015c08bb4734d44061b75e896aa6be70c3",
        //        startDateTime, endDateTime, keywords, sortKey, sortOrder, startRowNumber, endRowNumber);

        //    if (response.Data != null)
        //    {
        //        string json = response.Data.ToString();
        //        List<SentHistory> sentHistoryList =
        //            JsonConvert.DeserializeObject<List<SentHistory>>(json);

        //        //Use sentHistoryList as needed, it contains the list of messages matching the criteria
        //    }
        //}

        

        

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