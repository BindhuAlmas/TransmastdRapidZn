<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="PLYearWise.aspx.cs" Inherits="AmarCentre.Reports.PLYearWise" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Profit Loss Monthwise Statement /بيان الأرباح والخسائر
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
                               Year <span style="color: Red">&nbsp*</span>
                                <telerik:RadComboBox ID="drpYear" Sort="Ascending" EmptyMessage="Search ..."
                                    Filter="Contains" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                    OnClientBlur="ValidateCombo" runat="server" Style="height: 24px !important; width: 86%; overflow: hidden;
                                    border: none!important;">
                                </telerik:RadComboBox>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drpYear"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btn_search" ValidationGroup="save" class="butn" runat="server" OnClick="btn_search_OnClick"
                                    Text="Search" />
                                <asp:Button ID="btnGeneratePdf" class="butn" runat="server" ValidationGroup="save" Text="Generate Pdf"
                                    OnClick="btnGeneratePdf_OnClick" />
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
                                    <th style=" white-space: nowrap">
                                       Month
                                    </th>
                                    <th style="padding: 5px">
                                        Income
                                    </th>
                                    <th style="padding: 5px; white-space: nowrap">
                                        Expense
                                    </th>
                                    <th style="padding: 5px; white-space: nowrap">
                                      Profit
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rpt_list" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td >
                                                <%#Eval("Monthnames")%>
                                            </td>
                                            <td style="padding-left: 3px; text-align:right; white-space: nowrap">
                                                <%#Eval("Income")%>
                                            </td>
                                            <td style="padding-left: 3px;text-align:right;">
                                                <%#Eval("Expense")%>
                                            </td>
                                            <td style="padding-left: 3px;text-align:right;">
                                                <%#Eval("Profit")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <tr>
                                    <td colspan="3">Total</td>
                                    <td style="padding-left: 3px;text-align:right;"><asp:Label ID="lbltotal" runat="server"></asp:Label> </td>
                                </tr>
                                    </tbody>
                          </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

