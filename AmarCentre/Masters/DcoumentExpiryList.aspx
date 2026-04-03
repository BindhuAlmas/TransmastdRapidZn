<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="DcoumentExpiryList.aspx.cs" Inherits="AmarCentre.Masters.DcoumentExpiryList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register Src="~/Transactions/UserControl/UCMail.ascx" TagName="MailUC" TagPrefix="AmarCentre" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Document Expiry List
        <asp:Button ID="btn_filter" runat="server" class="filter right_align_list" OnClick="btn_filter_OnClick" />
      
        <asp:HiddenField ID="hdn_user_id" runat="server" />
    </div>
    <asp:UpdatePanel ID="upd_nav_filter" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnl_filter" runat="server">
                <div class="animated smallPopUp">
                    <div class="Adding_heading">
                        Search
                    </div>
                    <table class="formTable">
                        <tr>
                            <td>From Date
                            <telerik:RadDatePicker ID="txtFromdate" runat="server" class="input-boder" DateInput-DateFormat="dd/MM/yyyy">
                                <Calendar runat="server" ID="Calendar1" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                    ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                        </telerik:RadCalendarDay>
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>To Date
                            <telerik:RadDatePicker ID="txtTodate" runat="server" class="input-boder" DateInput-DateFormat="dd/MM/yyyy">
                                <Calendar runat="server" ID="Calendar2" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                    ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                        </telerik:RadCalendarDay>
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>

                            </td>
                        </tr>
                        <tr>
                            <td>Agent
                                    <telerik:RadComboBox ID="drpagent" Sort="Ascending" Filter="Contains" runat="server"
                                        AllowCustomText="true" RenderMode="Lightweight"
                                        EmptyMessage="Search Agent..." OnClientFocus="OnClientKeyPressing"
                                        Style="overflow: hidden; width: 86%; border: none!important;">
                                    </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Document Type 
                             <telerik:RadComboBox ID="drpDocument" Sort="Ascending" EmptyMessage="Search Document..." CheckBoxes="true"
                                 Filter="Contains" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                 runat="server" Style="height: 24px !important; width: 86%; overflow: hidden; border: none!important;">
                             </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Customer 
                           <telerik:RadComboBox ID="drpCustomer" Sort="Ascending" EmptyMessage="Search Customer..."
                               AutoPostBack="true" OnSelectedIndexChanged="drpCustomer_SelectedIndexChanged"
                               Filter="Contains" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                               runat="server" Style="height: 24px !important; width: 86%; overflow: hidden; border: none!important;">
                           </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Staff
                                <asp:UpdatePanel ID="UpdCustStaffPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <telerik:RadComboBox ID="drpCustomerStaff" Sort="Ascending" EmptyMessage="Search Staff..."
                                            Filter="Contains" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                            runat="server" Style="height: 24px !important; width: 86%; overflow: hidden; border: none!important;">
                                        </telerik:RadComboBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>


                        <tr>
                            <td>
                                <asp:Button ID="btn_search" class="butn" runat="server" OnClick="btn_search_OnClick"
                                    Text="Search" />
                                  <asp:Button ID="btnexcel_export" runat="server" class="butn " Text="Generate Excel"
                                     OnClick="btn_excel_OnClick" />
                                <asp:Button ID="Button1" runat="server" class="butn " Text="Generate Pdf"
                                     OnClick="btnPdfOnClick" />
                                <asp:HiddenField ID="HiddenField1" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="Upd_addpanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="overflow-x: auto; min-height: 250px; width: 100%">
                <asp:UpdatePanel ID="Upd_List_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table class="listTable" style="width: 98%; border: 1px">
                            <thead>
                                <tr style="text-align: center">
                                    <th style="width: 3%;">Sl No
                                    </th>
                                    <th style="width: 14%;">Name
                                    </th>
                                    <th style="width: 7%;">Staff
                                    </th>
                                    <th style="width: 10%;">ContactNo
                                    </th>
                                    <th style="width: 10%;">Document Type
                                    </th>
                                    <th style="width: 10%;">Document Number
                                    </th>
                                    <th style="width: 9%;">Valid From
                                    </th>
                                    <th style="width: 9%;">Valid Till
                                    </th>
                                    <th style="width: 9%;">Days Remaining
                                    </th>
                                    <th style="width: 9%;">Document Status
                                    </th>
                                    <th style="width: 6%;">Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rpt_list" runat="server" OnItemCommand="rpt_list_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <td style="text-align: center">
                                                <asp:HiddenField ID="hdnId" runat="server" Value='<%#Eval("Id")%>' />
                                                <asp:HiddenField ID="hdnDoctype" runat="server" Value='<%#Eval("Doctype")%>' />
                                                <asp:HiddenField ID="hdnCustomerMail" runat="server" Value='<%#Eval("CustomerMail")%>' />
                                                <asp:HiddenField ID="hdnCustomerId" runat="server" Value='<%#Eval("Customer_Id")%>' />

                                                <asp:Label ID="lblesxpiry" runat="server"
                                                    ForeColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("labelColor").ToString()) %>' Text='<%# Container.ItemIndex + 1 %>'></asp:Label>
                                            </td>
                                           
                                            <td style="padding-left: 3px;">
                                                <asp:Label ID="lbleaxpiry" runat="server"
                                                    ForeColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("labelColor").ToString()) %>' Text='<%#Eval("Name")%>'></asp:Label>
                                            </td>
                                             <td style="padding-left: 3px;">
                                                <asp:Label ID="lblssexpiry" runat="server"
                                                    ForeColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("labelColor").ToString()) %>' Text='<%#Eval("Staff")%>'></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <asp:Label ID="lbleaaxpiry" runat="server"
                                                    ForeColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("labelColor").ToString()) %>' Text='<%#Eval("ContactNo")%>'></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <asp:Label ID="lblexpiddry" runat="server"
                                                    ForeColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("labelColor").ToString()) %>' Text='<%#Eval("DocumentType")%>'></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <asp:Label ID="lblexpdiry" runat="server"
                                                    ForeColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("labelColor").ToString()) %>' Text='<%#Eval("DocumentNo")%>'></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <asp:Label ID="lblexxpiry" runat="server"
                                                    ForeColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("labelColor").ToString()) %>' Text='<%#Eval("ValidFrom")%>'></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <asp:Label ID="lblexpiry" runat="server" Font-Bold="true"
                                                    ForeColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("labelColor").ToString()) %>' Text='<%#Eval("Expirydate")%>'></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <asp:Label ID="Label1" runat="server" Font-Bold="true"
                                                    ForeColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("labelColor").ToString()) %>' Text='<%#Eval("DaysRemaining")%>'></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <asp:Label ID="Label2" runat="server" Font-Bold="true"
                                                    ForeColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("labelColor").ToString()) %>' Text='<%#Eval("DocumentStatus")%>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnSendmail" runat="server" class="btnsendmail" ToolTip="Send Mail"
                                                    CommandName="Sendmail" Visible='<%#Convert.ToBoolean(Eval("Actionview")) %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnexcel_export" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>

            <div style="width: 57%" runat="server" id="divchartbar">

                <asp:Chart ID="Chart2" Width="500px" runat="server" Palette="BrightPastel" Visible="false">

                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1">
                            <AxisX LineColor="Gray" Interval="1">
                                <MajorGrid LineColor="Gray" LineDashStyle="NotSet" />
                            </AxisX>
                            <AxisY LineColor="Gray">
                                <MajorGrid LineColor="Gray" LineDashStyle="NotSet" />
                            </AxisY>
                        </asp:ChartArea>
                    </ChartAreas>
                    <Legends>
                        <asp:Legend Name="Legend1" Docking="Bottom">
                        </asp:Legend>
                    </Legends>
                </asp:Chart>

            </div>

            <asp:UpdatePanel ID="UpdMailPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlMail" Visible="false" runat="server">
                        <AmarCentre:MailUC ID="EmailUC" runat="server" />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

        </ContentTemplate>

    </asp:UpdatePanel>



</asp:Content>

