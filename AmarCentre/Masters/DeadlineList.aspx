<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="DeadlineList.aspx.cs" Inherits="AmarCentre.Masters.DeadlineList" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
     Deadline List
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
                        <td>Customer
                           <telerik:RadComboBox ID="drpCustomer" Sort="Ascending" Filter="Contains" runat="server"
                               AllowCustomText="true" RenderMode="Lightweight"
                               EmptyMessage="Search Customer..." OnClientFocus="OnClientKeyPressing"
                               Style="overflow: hidden; width: 86%; border: none!important;">
                           </telerik:RadComboBox>
                       </td>
                    </tr>
                    <tr>
                        <td>Service
                           <telerik:RadComboBox ID="drpService" Sort="Ascending" Filter="Contains" runat="server"
                               AllowCustomText="true" RenderMode="Lightweight"
                               EmptyMessage="Search Service..." OnClientFocus="OnClientKeyPressing"
                               Style="overflow: hidden; width: 86%; border: none!important;">
                           </telerik:RadComboBox>
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
                                    <th style="width: 3%;">Sl
                                    </th>
                                    <th style="width: 12%;">Customer
                                    </th>
                                    <th style="width: 12%;">Agent
                                        </th>
                                    <th style="width: 7%;">Invoice
                                    </th>
                                    <th style="width: 8%;">Invoice Date
                                    </th>
                                    <th style="width: 15%;">Service
                                    </th>
                                    <th style="width: 12%;">Particular
                                    </th>
                                    <th style="width: 8%;">Deadline Date
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rpt_list" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td style="text-align: center">
                                                <%#Eval("RowNum")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("Customer")%>
                                            </td>
                                              <td style="padding-left: 3px;">
                                                  <%#Eval("Agent")%>
                                              </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("Code")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("InvoiceDate")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("Service")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("Particulars")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("deadline")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnexcel_export" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


