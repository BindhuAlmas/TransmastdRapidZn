<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="ServiceProfitStatement.aspx.cs" Inherits="AmarCentre.Reports.ServiceProfitStatement" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Service Profit Statement/بيان ربح الخدمة
        <asp:Button ID="btn_filter" runat="server" class="filter right_align_list" OnClick="btn_filter_OnClick" />
    </div>
    <asp:UpdatePanel ID="upd_nav_filter" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnl_filter" runat="server">
                <div class="animated smallPopUpFilter">
                    <div class="Adding_heading">
                        Search
                    </div>
                    <table class="formTable">
                        <tr>
                            <td>From Date
                                <br />
                                <telerik:RadDatePicker ID="txtFromDate" runat="server" class="input-boder" DateInput-DateFormat="dd/MM/yyyy">
                                    <Calendar runat="server" ID="Calendar1" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                        ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                        <SpecialDays>
                                            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                            </telerik:RadCalendarDay>
                                        </SpecialDays>
                                    </Calendar>
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFromDate"
                                    Display="Dynamic" ValidationGroup="sumry" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>To Date
                                <br />
                                <telerik:RadDatePicker ID="txtToDate" runat="server" class="input-boder" DateInput-DateFormat="dd/MM/yyyy">
                                    <Calendar runat="server" ID="Calendar2" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                        ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                        <SpecialDays>
                                            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                            </telerik:RadCalendarDay>
                                        </SpecialDays>
                                    </Calendar>
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtToDate"
                                    Display="Dynamic" ValidationGroup="sumry" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>Customer 
                                <telerik:RadComboBox ID="drpCustomer" Sort="Ascending" EmptyMessage="Search Customer..."
                                    CheckBoxes="true"
                                    Filter="Contains" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                    runat="server" Style="height: 24px !important; width: 86%; overflow: hidden; border: none!important;">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                           <td>
                               Agent
                             
                               <telerik:RadComboBox ID="drpagent" Sort="Ascending" EmptyMessage="Search Agent..."
                                Filter="Contains" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                runat="server" Style="height: 24px !important; width: 86%; overflow: hidden; border: none!important;">
                            </telerik:RadComboBox>
                           </td>
                       </tr>
                        <tr>
                            <td>Department 
                                <telerik:RadComboBox ID="drpDepartment" Sort="Ascending" EmptyMessage="Search Department..."
                                    CheckBoxes="true"
                                    Filter="Contains" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                    runat="server" Style="height: 24px !important; width: 86%; overflow: hidden; border: none!important;">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Service
                                <telerik:RadComboBox ID="drpService" Sort="Ascending" EmptyMessage="Search Service..."
                                    CheckBoxes="true"
                                    Filter="Contains" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                    runat="server" Style="height: 24px !important; width: 86%; overflow: hidden; border: none!important;">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Vendor
                                <telerik:RadComboBox ID="drpVendor" Sort="Ascending" EmptyMessage="Search Vendor..."
                                    Filter="Contains" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                    runat="server" Style="height: 24px !important; width: 86%; overflow: hidden; border: none!important;">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                           <tr>
                            <td>
                                Invoice
                                <telerik:RadComboBox ID="drpInvoice" Sort="Ascending" EmptyMessage="Search Invoice..."
                                CheckBoxes="true" 
                                    Filter="Contains" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                  runat="server" Style="height: 24px !important; width: 86%; overflow: hidden;
                                    border: none!important;">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Employee
                                <br />
                                <telerik:RadComboBox ID="drpEmployee" Sort="Ascending" EmptyMessage="Search Employee..."
                                    CheckBoxes="true"
                                    Filter="Contains" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                    runat="server" Style="height: 24px !important; width: 86%; overflow: hidden; border: none!important;">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btn_search" class="butn" runat="server" OnClick="btn_search_OnClick"
                                    Text="Search" />
                                <asp:Button ID="btn_excel" class="butn" runat="server" Text="Generate Excel"
                                    OnClick="btn_excel_OnClick" />
                                 <asp:Button ID="Button1" class="butn" runat="server" ValidationGroup="sumry" Text="Summary"
                                    OnClick="btnSummary_OnClick" />
                                <asp:HiddenField ID="hdn_user_id" runat="server" />
                                <asp:Button ID="Button2" class="butn" runat="server" ValidationGroup="save" Text="Generate Pdf"
                                OnClick="btnPdfOnClick" />
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
                        <table class="listTable" style="width: 120%; border: 1px">
                            <thead>
                                <tr style="text-align: center">
                                    <th style="width: 5px; white-space: nowrap">Sl No.
                                    </th>
                                    <th style="padding: 5px">Inv.Date
                                    </th>
                                      <th style="padding: 5px">SC Date
                                    </th>
                                    <th style="padding: 5px;">Invoice
                                    </th>
                                    <th style="padding: 5px;">Customer
                                    </th>
                                    <th style="padding: 5px;">Agent
                                    </th>
                                    <th style="padding: 5px;">Service
                                    </th>
                                    <th style="padding: 5px;">Quantity
                                    </th>
                                    <th style="padding: 5px;">Employee
                                    </th>
                                    <th style="padding: 5px;">Payment mode
                                    </th>
                                      <th style="padding: 5px;">Vendor
                                    </th>
                                    <th style="padding: 5px;">Invoice Amount
                                    </th>
                                    <th style="padding: 5px;">Expense
                                    </th>
                                   
                                    <th style="padding: 5px;">Vendor Commission
                                    </th>
                                    <th style="padding: 5px;">Received Tax
                                    </th>
                                    <th style="padding: 5px;">Paid Tax
                                    </th>
                                    <th style="padding: 5px;">Tax Payable
                                    </th>
                                    <th style="padding: 5px;">Customer Commission
                                    </th>
                                    <th style="padding: 5px;">
                                        Incentive
                                    </th>
                                    <th style="padding: 5px;">
                                        Profit
                                    </th>
                                      <th style="padding: 5px;">
                                       Agent Profit
                                    </th>
                                    <th style="padding: 5px;">Net Profit
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rpt_list" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td style="text-align: center">
                                                <%#Eval("Sl_No")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("InvoiceDate")%>
                                            </td>
                                             <td style="padding-left: 3px;">
                                                <%#Eval("SCDate")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("InvoiceCode")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("Customer")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("Agent")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("Service")%>
                                            </td>
                                            <td style="padding-left: 3px; text-align: center">
                                                <%#Eval("Quantity")%>
                                            </td>
                                            <td style="padding-left: 3px; text-align: center">
                                                <%#Eval("Employee")%>
                                            </td>
                                            <td style="padding-left: 3px; text-align: center">
                                                <%#Eval("Paymentmode")%>
                                            </td>
                                              <td style="padding-left: 3px; text-align: center">
                                                <%#Eval("Vendorname")%>
                                            </td>
                                            <td style="padding-left: 3px; text-align: right">
                                                <%#Eval("InvoiceAmount")%>
                                            </td>
                                            <td style="padding-left: 3px; text-align: right">
                                                <%#Eval("Expense")%>
                                            </td>
                                         
                                              <td style="padding-left: 3px; text-align: right">
                                                <%#Eval("vendorcommission")%>
                                            </td>
                                            <td style="padding-left: 3px; text-align: right">
                                                <%#Eval("ReceivedTax")%>
                                            </td>
                                            <td style="padding-left: 3px; text-align: right">
                                                <%#Eval("PaidTax")%>
                                            </td>
                                            <td style="padding-left: 3px; text-align: right">
                                                <%#Eval("TaxPayable")%>
                                            </td>
                                            <td style="padding-left: 3px; text-align: right">
                                                <%#Eval("commission")%>
                                            </td>
                                              <td style="padding-left: 3px;text-align:right">
                                                <%#Eval("Incentive")%>
                                            </td>
                                            <td style="padding-left: 3px;text-align:right">
                                                <%#Eval("Profit")%>
                                            </td>
                                               <td style="padding-left: 3px;text-align:right">
                                                <%#Eval("AgentProfit")%>
                                            </td>
                                            <td style="padding-left: 3px; text-align: right">
                                                <%#Eval("NetProfit")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <tr>
                                    <td colspan="11" style="text-align: right">Total
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblinvAmount" Text="" runat="server"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lbl_expnse" Text="" runat="server"></asp:Label>
                                    </td>
                                  
                                      <td style="text-align: right">
                                        <asp:Label ID="lblvendorcommission" Text="" runat="server"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lbl_rectax" Text="" runat="server"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lbl_paytax" Text="" runat="server"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lbl_payabletax" Text="" runat="server"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lbl_commssion" Text="" runat="server"></asp:Label>
                                    </td>
                                      <td style="text-align: right">
                                        <asp:Label ID="lblincentive" Text="" runat="server"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lbl_pft" Text="" runat="server"></asp:Label>
                                    </td>
                                      <td style="text-align: right">
                                        <asp:Label ID="lblagentpft" Text="" runat="server"></asp:Label>
                                    </td>
                                      <td style="text-align: right">
                                        <asp:Label ID="lblnetpft" Text="" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="15" class="navigationRow">
                                        <asp:UpdatePanel ID="Upd_Nav_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="navigation_table">
                                                    <asp:Label ID="lbl_page_info" runat="server" class="pageInfo"></asp:Label>
                                                    <asp:Button ID="btn_first" runat="server" class="navigationButton" Text="<<" OnClick="btn_first_OnClick" />
                                                    <asp:Button ID="btn_prev" runat="server" class="navigationButton" Text="<" OnClick="btn_prev_OnClick" />
                                                    <asp:Label ID="lbl_page_number" Style="font-weight: bold; margin-left: 5px; margin-right: 5px; text-align: center;"
                                                        runat="server"></asp:Label>
                                                    <asp:Button ID="btn_next" class="navigationButton" runat="server" Text=">" OnClick="btn_next_OnClick" />
                                                    <asp:Button ID="btn_last" class="navigationButton" runat="server" Text=">>" OnClick="btn_last_OnClick" />
                                                    <asp:DropDownList ID="drp_count" class="pageSize" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="drp_count_OnSelectedIndexChanged">
                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:HiddenField ID="hdn_last_page" runat="server" />
                                                    <asp:HiddenField ID="hdn_filter" runat="server" />
                                                    <asp:HiddenField ID="hdn_total" runat="server" Value="0" />
                                                    <asp:HiddenField ID="Common_order_column" runat="server" />
                                                    <asp:HiddenField ID="Common_asc_desc" runat="server" />
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
          
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_excel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
