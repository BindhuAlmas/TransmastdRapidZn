<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="CompanyGroupSOA.aspx.cs" Inherits="AmarCentre.Reports.CompanyGroupSOA" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
      Company Group SOA
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
                            <td>
                                From Date<span style="color: Red">&nbsp*</span>
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
                                    Display="Dynamic" ValidationGroup="save" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                To Date<span style="color: Red">&nbsp*</span>
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
                                    Display="Dynamic" ValidationGroup="save" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Company <span style="color: Red">&nbsp*</span>
                                <telerik:RadComboBox ID="drpCompany" ClientIDMode="AutoID" Sort="Ascending" EmptyMessage="Search ..."
                                    Filter="Contains" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                    OnClientBlur="ValidateCombo" runat="server" Style="height: 24px !important; width: 86%;
                                    overflow: hidden; border: none!important;">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="drpCompany"
                                    Display="Dynamic" ValidationGroup="save" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                      
                            <tr>
                                <td>
                                    <asp:Button ID="btn_search" ValidationGroup="save" class="butn" runat="server" OnClick="btn_search_OnClick"
                                        Text="Search" />
                                    <asp:Button ID="btn_excel" class="butn" runat="server" ValidationGroup="save" Text="Generate Excel"
                                        OnClick="btn_excel_OnClick" />
                                    <asp:Button ID="btnPdf" class="butn" runat="server" ValidationGroup="save" Text="Generate PDF"
                                        OnClick="btnPdfOnClick" />
                                    <asp:HiddenField ID="hdn_user_id" runat="server" />
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
                                    <th style="padding: 5px">
                                        From Date
                                    </th>
                                    <th style="padding: 5px">
                                        To Date
                                    </th>
                                    <th style="padding: 5px">
                                       Company
                                    </th>
                                
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td style="padding-left: 3px; white-space: nowrap">
                                        <asp:Label ID="lblFromDate" runat="server" Text="" TabIndex="-1"></asp:Label>
                                    </td>
                                    <td style="padding-left: 3px; white-space: nowrap">
                                        <asp:Label ID="lblToDate" runat="server" Text="" TabIndex="-1"></asp:Label>
                                    </td>
                                    <td style="padding-left: 3px;">
                                        <asp:Label ID="lblCustomerName" runat="server" Text="" TabIndex="-1"></asp:Label>
                                    </td>
                                
                                </tr>
                            </tbody>
                        </table>
                        <br />
                        <table class="listTable" style="width: 98%; border: 1px">
                            <thead>
                                <tr style="text-align: center">
                                    <th style="width: 5px; white-space: nowrap">
                                        Sl No.
                                    </th>
                                     <th style="padding: 5px; white-space: nowrap">
                                        Customer
                                    </th>
                                    <th style="padding: 5px; white-space: nowrap">
                                        Invoice Code
                                    </th>
                                    <th style="padding: 5px; white-space: nowrap">
                                        Receipt Code
                                    </th>
                                    <th style="padding: 5px">
                                        Date
                                    </th>
                                    <th style="width: 15%; padding: 5px;">
                                        Description
                                    </th>
                                    <th style="width: 15%; padding: 5px; ">
                                        Person Name
                                    </th>
                                   
                                    <th style="width: 15%; padding: 5px; ">
                                        Govt Fee
                                    </th>
                                    <th style="width: 15%; padding: 5px;">
                                        Typing Charge
                                    </th>
                                    <th style="width: 15%; padding: 5px; ">
                                        Tax
                                    </th>
                                    <th style="width: 15%; padding: 5px; ">
                                        Fine
                                    </th>
                                    <th style="width: 15%; padding: 5px; ">
                                        Discount
                                    </th>
                                    <th style="width: 15%; padding: 5px; ">
                                        Total
                                    </th>
                                    <th style="width: 15%; padding: 5px;">
                                        Paid
                                    </th>
                                    <th style="width: 15%; padding: 5px; ">
                                        Balance
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
                                                <%#Eval("Name")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("Code")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("ReceiptCode")%>
                                            </td>
                                            <td style="padding-left: 3px; ">
                                                <%#Eval("Date")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("Description")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("PersonName")%>
                                            </td>
                                          
                                            <td style="padding-left: 3px; text-align: right">
                                                <%#Eval("GovtFee")%>
                                            </td>
                                            <td style="padding-left: 3px; text-align: right">
                                                <%#Eval("TypingCharge")%>
                                            </td>
                                            <td style="padding-left: 3px; text-align: right">
                                                <%#Eval("Tax")%>
                                            </td>
                                            <td style="padding-left: 3px; text-align: right">
                                                <%#Eval("Fine")%>
                                            </td>
                                            <td style="padding-left: 3px; text-align: right">
                                                <%#Eval("Discount")%>
                                            </td>
                                            <td style="padding-left: 3px; text-align: right">
                                                <%#Eval("Total")%>
                                            </td>
                                            <td style="padding-left: 3px; text-align: right">
                                                <%#Eval("Paid")%>
                                            </td>
                                            <td style="padding-left: 3px; text-align: right">
                                                <%#Eval("Balance")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <tr>
                                    <td colspan="7">
                                        Total
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblTotalGovtFee" Text="" runat="server"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblTotalTypingCharge" Text="" runat="server"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblTotalTax" Text="" runat="server"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblTotalFine" Text="" runat="server"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblTotalDiscount" Text="" runat="server"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblTotalInvoiceAmount" Text="" runat="server"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblTotalReceivedAmount" Text="" runat="server"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblTotalOutstandingAmount" Text="" runat="server"></asp:Label>
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
                                                    <asp:Label ID="lbl_page_number" Style="font-weight: bold; margin-left: 5px; margin-right: 5px;
                                                        text-align: center;" runat="server"></asp:Label>
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

