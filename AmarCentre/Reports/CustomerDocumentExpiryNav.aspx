<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="CustomerDocumentExpiryNav.aspx.cs" Inherits="AmarCentre.Reports.CustomerDocumentExpiryNav" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Customer Document Expiry /انتهاء صلاحية مستند العميل

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
                            <td>To Date
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
                         <td>Agent
                                 <telerik:RadComboBox ID="drpagent" Sort="Ascending" Filter="Contains" runat="server"
                                     AllowCustomText="true" RenderMode="Lightweight"
                                     EmptyMessage="Search Agent..." OnClientFocus="OnClientKeyPressing"
                                     Style="overflow: hidden; width: 86%; border: none!important;">
                                 </telerik:RadComboBox>
                         </td>
                     </tr>

                        <tr>
                            <td>
                                Customer 
                                <telerik:RadComboBox ID="drp_Cust" Sort="Ascending" Filter="Contains" runat="server"
                                    AllowCustomText="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" RenderMode="Lightweight"
                                    EmptyMessage="Search Customer..." OnClientFocus="OnClientKeyPressing" 
                                    Style="overflow: hidden; width: 86%; border: none!important;">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Sponsor
                                             <telerik:RadComboBox ID="drpSponser" Sort="Ascending" Filter="Contains" runat="server"
                                                 AllowCustomText="false" RenderMode="Lightweight"
                                                 EmptyMessage="Search Sponsor..." OnClientFocus="OnClientKeyPressing" Style="overflow: hidden; width: 86%; border: none!important;">
                                             </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btn_search" class="butn" runat="server" OnClick="btn_search_OnClick"
                                    Text="Search" />
                                <asp:Button ID="btn_excel" class="butn" runat="server" ValidationGroup="save" Text="Generate Excel"
                                    OnClick="btn_excel_OnClick" />
                                 <asp:Button ID="Button1" class="butn" runat="server" ValidationGroup="save" Text="Generate Pdf"
                                    OnClick="btnPdfOnClick" />
                                 <asp:Button ID="Button2" class="butn" runat="server" ValidationGroup="save" Text="Generate Excel Format 2"
                                    OnClick="btn_excelF2_OnClick" />
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
                                    <th style="width: 3%;">Sl No
                                    </th>
                                    <th style="width: 12%;">Customer
                                    </th>
                                    <th style="width: 12%;">ContactNo
                                    </th>
                                    <th style="width: 10%;">Document Name
                                    </th>
                                    <th style="width: 10%;">Document Type
                                    </th>
                                    <th style="width: 10%;">Document Number
                                    </th>
                                    <th style="width: 10%;">Valid From
                                    </th>
                                    <th style="width: 10%;">Valid Till
                                    </th>
                                    <th style="width: 10%;">Remark
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
                                                <%#Eval("ContactNo")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("DocumentName")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("doc_type")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("DocNumber")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("Valid_From")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("Valid_To")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("Remark")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>

                                <tr>
                                    <td colspan="9" class="navigationRow">
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
            <asp:PostBackTrigger ControlID="Button2" />

        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
