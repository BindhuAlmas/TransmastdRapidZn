<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="VATStatement.aspx.cs" Inherits="AmarCentre.Reports.VATStatement" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        VAT Statement/بيان ضريبة القيمة المضافة
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
                                From Date
                                <telerik:RadDatePicker ID="txtFromDate" runat="server" class="input-boder" DateInput-DateFormat="dd/MM/yyyy">
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
                            <td>
                                To Date
                                <telerik:RadDatePicker ID="txtToDate" runat="server" class="input-boder" DateInput-DateFormat="dd/MM/yyyy">
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
                          <td>
                              Emirate
                                <telerik:RadComboBox ID="drpEmirate" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search ..."
                                                OnClientFocus="OnClientKeyPressing" OnClientBlur="ValidateCombo" Style="overflow: hidden; width: 96%; border: none!important;">
                                            </telerik:RadComboBox>
                          </td>
                      </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btn_search" class="butn" runat="server" OnClick="btn_search_OnClick"
                                    Text="Search" />
                                <asp:Button ID="btn_excel" class="butn" runat="server" ValidationGroup="save" Text="Generate Excel"
                                    OnClick="btn_excel_OnClick" />
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
                                     <th style="width: 3%; white-space: nowrap">
                                        Sl No.
                                    </th>
                                    <th style="width: 5%; padding: 5px">
                                        Date
                                    </th>
                                    <th style="width: 5%; padding: 5px">
                                        Ref No.
                                    </th>
                                     <th style="width: 15%; padding: 5px">
                                       Name
                                    </th>
                                      <th style="width: 5%; padding: 5px">
                                       Emirate
                                    </th>
                                    <th style="width: 8%; padding: 5px; white-space: nowrap">
                                        Description
                                    </th>
                                    <th style="width: 5%; padding: 5px;">
                                        TRN
                                    </th>
                                    <th style="width: 6%; padding: 5px;">
                                        Taxable Amount Received
                                    </th>
                                    <th style="width:4%;padding: 5px; white-space: nowrap">
                                        Received Tax
                                    </th>
                                     <th style="width: 6%; padding: 5px;">
                                        Taxable Amount Paid
                                    </th>
                                    <th style="width:4%;padding: 5px; white-space: nowrap">
                                        Paid Tax
                                    </th>
                                    <th style="width:4%;padding: 5px; white-space: nowrap">
                                        Tax Payable
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
                                            <td style="padding-left: 3px; white-space: nowrap">
                                                <%#Eval("Date")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("RefNo")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("Customer")%>
                                            </td>
                                             <td style="padding-left: 3px;">
                                                <%#Eval("Emirate")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("Description")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("TRN")%>
                                            </td>
                                            <td style="padding-left: 3px; white-space: nowrap;text-align:right">
                                                <%#Eval("TaxableAmount")%>
                                            </td>
                                            <td style="padding-left: 3px; white-space: nowrap;text-align:right">
                                                <%#Eval("ReceivedTax")%>
                                            </td>
                                             <td style="padding-left: 3px; white-space: nowrap;text-align:right">
                                                <%#Eval("TaxableAmountPaid")%>
                                            </td>
                                            <td style="padding-left: 3px; white-space: nowrap;text-align:right">
                                                <%#Eval("PaidTax")%>
                                            </td>
                                            <td style="padding-left: 3px; white-space: nowrap;text-align:right">
                                                <%#Eval("TaxPayable")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <tr>
                                    <td colspan="7">
                                        Total
                                    </td>
                                     <td style="text-align:right">
                                        <asp:Label ID="TaxableAmount" Text="" runat="server"></asp:Label>
                                    </td>
                                    <td style="text-align:right">
                                        <asp:Label ID="lblTotalReceivedTax" Text="" runat="server"></asp:Label>
                                    </td>
                                     <td style="text-align:right">
                                        <asp:Label ID="TaxableAmountPaid" Text="" runat="server"></asp:Label>
                                    </td>
                                    <td style="text-align:right">
                                        <asp:Label ID="lblTotalPaidTax" Text="" runat="server"></asp:Label>
                                    </td>
                                    <td style="text-align:right">
                                        <asp:Label ID="lblTotalTaxPayable" Text="" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="12" class="navigationRow">
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

