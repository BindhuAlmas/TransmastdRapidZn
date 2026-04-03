<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true"
    CodeBehind="ServiceCompletionRep_Nav.aspx.cs" Inherits="AmarCentre.Reports.ServiceCompletionRep_Nav" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Service Completion Report/تقرير إنجاز الخدمة
        <asp:Button ID="btn_filter" runat="server" class="filter right_align_list" OnClick="btn_filter_OnClick" />
        <%-- SEARCH BOX - exactly same pattern as Service page --%>
        <div class="searchDiv">
            <asp:TextBox ID="txt_search" runat="server" class="txt_search" 
                AutoPostBack="true"
                OnTextChanged="txt_search_OnTextChanged" 
                placeholder="Search">
            </asp:TextBox>
        </div>
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
                                <br />
                                <telerik:RadDatePicker ID="txt_reg_Frm_date" runat="server" class="input-boder" DateInput-DateFormat="dd/MM/yyyy">
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
                                <br />
                                <telerik:RadDatePicker ID="txt_reg_to_date" runat="server" class="input-boder" DateInput-DateFormat="dd/MM/yyyy">
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
                                Customer
                                <telerik:RadComboBox ID="drp_Cust" Sort="Ascending" Filter="Contains" runat="server"
                                    AllowCustomText="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" RenderMode="Lightweight"
                                    EmptyMessage="Search Customer..." OnClientFocus="OnClientKeyPressing" 
                                    Style="overflow: hidden; width: 96%; border: none!important;">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                Agent
                                <telerik:RadComboBox ID="drpagent" Sort="Ascending" Filter="Contains" runat="server"
                                    AllowCustomText="true"  RenderMode="Lightweight"
                                    EmptyMessage="Search Agent..." OnClientFocus="OnClientKeyPressing" 
                                    Style="overflow: hidden; width: 96%; border: none!important;">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Department
                                <telerik:RadComboBox ID="drpDepartment"  ClientIDMode="AutoID" Sort="Ascending"
                                    Filter="Contains" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing" 
                                    EmptyMessage="Search Department..." CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                    runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpDepartment_OnSelectedIndexChanged"  Style="height: 24px !important;
                                    width: 96%; overflow: hidden; border: none!important;">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Service
                                <asp:UpdatePanel ID="UpdServicePanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                               <ContentTemplate>
                                <telerik:RadComboBox ID="drp_Service"  ClientIDMode="AutoID" Sort="Ascending"
                                    Filter="Contains" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing" 
                                    EmptyMessage="Search Service..." CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                    runat="server"  Style="height: 24px !important;
                                    width: 96%; overflow: hidden; border: none!important;">
                                </telerik:RadComboBox>
                                 </ContentTemplate>
                                    </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr style="display:none">
                            <td>
                                Bank
                                <telerik:RadComboBox ID="drp_Bank" Sort="Ascending" Filter="Contains" runat="server"
                                    CheckBoxes="true" EnableCheckAllItemsCheckBox="true" AllowCustomText="true" RenderMode="Lightweight"
                                    EmptyMessage="Search Bank..." OnClientFocus="OnClientKeyPressing"
                                    Style="overflow: hidden; width: 96%; border: none!important;">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            Employee <br />
                                <telerik:RadComboBox ID="drpEmployee" ClientIDMode="AutoID" Sort="Ascending" EmptyMessage="Search Employee..."
                                    Filter="Contains" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                    OnClientBlur="ValidateCombo" runat="server" Style="height: 24px !important; width: 96%; overflow: hidden;
                                    border: none!important;">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                          <tr>
                              <td>
                                  <asp:Panel ID="pnlstatus" runat="server">
                                      Approval  Status
                                       <telerik:RadComboBox ID="drpServiceStatus" Sort="Ascending" Filter="Contains" runat="server"
                                           AllowCustomText="true" RenderMode="Lightweight"
                                           EmptyMessage="Search ..." OnClientFocus="OnClientKeyPressing"
                                           Style="overflow: hidden; width: 96%; border: none!important;">
                                         
                                       </telerik:RadComboBox>
                                  </asp:Panel>
                              </td>
                          </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btn_search" ValidationGroup="save" class="butn" runat="server" OnClick="btn_search_OnClick"
                                    Text="Search" />
                                <asp:Button ID="btn_excel" class="butn" runat="server" ValidationGroup="save" Text="Generate Excel"
                                    OnClick="btn_excel_OnClick" />
                                <asp:HiddenField ID="hdn_user_id" runat="server" />
                                 <asp:Button ID="Button1" class="butn" runat="server" ValidationGroup="save" Text="Generate Pdf"
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
                        <table class="listTable" style="width: 98%; border: 1px">
                            <thead>
                                <tr style="text-align: center">
                                    <th style="width: 5px; white-space: nowrap">
                                        Sl No.
                                    </th>
                                    <th style="padding:5px">
                                        Date
                                    </th>
                                    <th style="padding:5px; white-space: nowrap">
                                        Invoice No
                                    </th>
                                    <th style="padding: 5px;">
                                        Customer
                                    </th>
                                     <th style="padding:5px; white-space: nowrap">
                                        Agent
                                    </th>
                                    <th style="padding:5px; white-space: nowrap">
                                        Service
                                    </th>
                                     <th style="padding:5px; white-space: nowrap">
                                       Applicant
                                    </th>
                                    <th style="padding:5px; white-space: nowrap">
                                       Vendor
                                    </th>
                                    <th style="padding:5px; white-space: nowrap">
                                        Employee
                                    </th>
                                     <th style="padding:5px; white-space: nowrap">
                                        Service Amount
                                    </th>
                                    <th style="padding:5px; white-space: nowrap">
                                        Expense Amount
                                    </th>
                                    <th style="padding:5px; white-space: nowrap">
                                        Mode of payment
                                    </th>
                                    <th style="padding:5px; white-space: nowrap">
                                        Transaction No
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
                                            <td style="padding-left: 3px; white-space: nowrap">
                                                <%#Eval("Invoice")%>
                                            </td>
                                            <td style="padding-left: 3px; white-space: nowrap">
                                                <%#Eval("Customer")%>
                                            </td>
                                             <td style="padding-left: 3px; white-space: nowrap">
                                                <%#Eval("Agent")%>
                                            </td>
                                              <td style="padding-left: 3px; white-space: nowrap">
                                                <%#Eval("Service")%>
                                            </td>
                                              <td style="padding-left: 3px; white-space: nowrap">
                                                <%#Eval("Particulars")%>
                                            </td>
                                             <td style="padding-left: 3px; white-space: nowrap">
                                                <%#Eval("Vendor")%>
                                            </td>
                                            <td style="padding-left: 3px; white-space: nowrap">
                                                <%#Eval("Employee")%>
                                            </td>
                                             <td style="padding-left: 3px; white-space: nowrap">
                                                <%#Eval("ServiceAmount")%>
                                            </td>
                                            <td style="padding-left: 3px; white-space: nowrap">
                                                <%#Eval("Expense")%>
                                            </td>
                                            <td style="padding-left: 3px; white-space: nowrap">
                                                <%#Eval("Paymentmode")%>
                                            </td>
                                            <td style="padding-left: 3px; white-space: nowrap">
                                                <%#Eval("TransactionNumber")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <tr>
                                    <td colspan="13" class="navigationRow">
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
