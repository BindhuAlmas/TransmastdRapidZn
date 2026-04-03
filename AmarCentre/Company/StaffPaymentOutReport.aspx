<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="StaffPaymentOutReport.aspx.cs" Inherits="AmarCentre.Company.StaffPaymentOutReport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
      Payment Outstanding Report
        <asp:Button ID="btn_filter" runat="server" class="filter right_align_list" OnClick="btn_filter_OnClick" />
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
                            <td>
                               Company
                                <telerik:RadComboBox ID="drpCompany" Sort="Ascending" Filter="Contains" runat="server"
                                    AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                   OnClientBlur="ValidateCombo" EmptyMessage="Search Name..."
                                   Style="overflow: hidden;width: 96%; border: none!important;">
                                </telerik:RadComboBox>
                               
                            </td>
                        </tr>
                       
                        <tr>
                            <td>
                                <asp:Button ID="btn_search" ValidationGroup="save" class="butn" runat="server" OnClick="btn_search_OnClick"
                                    Text="Search" />
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
                                    <th style="width: 5%; white-space: nowrap">
                                        Sl No.
                                    </th>
                                    <th style="width: 15%;">
                                        Company
                                    </th>
                                    <th style="width: 15%;">
                                        Staff
                                    </th>
                                    <th style="width: 12%;">
                                        Contact No
                                    </th>
                                      <th style="width: 10%;">
                                        Expense
                                    </th> 
                                      <th style="width: 10%;">
                                        Amount
                                    </th>
                                     <th style="width: 10%;">
                                        Received
                                    </th>
                                     <th style="width: 10%;">
                                        Balance
                                    </th>
                                     <th style="width: 10%;">
                                        Company Outstanding
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
                                                <%#Eval("Company")%>
                                            </td>
                                              <td style="padding-left: 3px;">
                                                <%#Eval("Name")%>
                                            </td>
                                              <td style="padding-left: 3px;">
                                                <%#Eval("ContactNo")%>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <%#Eval("Expense")%>
                                            </td>
                                             <td style="padding-left: 3px; text-align: right">
                                                <%#Eval("AgreementAmount")%>
                                            </td>
                                              <td style="padding-left: 3px; text-align: right">
                                                <%#Eval("PaidAmount")%>
                                            </td>
                                            <td style="padding-left: 3px; text-align: right">
                                                <%#Eval("Balance")%>
                                            </td>
                                             <td style="padding-left: 3px; text-align: right">
                                                <%#Eval("CompanyOutstanding")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <tr>
                                    <td colspan="4">
                                        Total
                                    </td>
                                       <td style="text-align: right">
                                        <asp:Label ID="expense" Text="" runat="server"></asp:Label>
                                    </td>
                                     <td style="text-align: right">
                                        <asp:Label ID="lbltotal" Text="" runat="server"></asp:Label>
                                    </td>
                                      <td style="text-align: right">
                                        <asp:Label ID="lblpaid" Text="" runat="server"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblbalance" Text="" runat="server"></asp:Label>
                                    </td>
                                     <td style="text-align: right">
                                        <asp:Label ID="lblCompanyOut" Text="" runat="server"></asp:Label>
                                    </td>

                                </tr>
                                <tr>
                                    <td colspan="9" class="navigationRow">
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
    </asp:UpdatePanel>
</asp:Content>
