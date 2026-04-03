<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="CreditorsReport.aspx.cs" Inherits="AmarCentre.Reports.CreditorsReport" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Deferred Income report
      <%-- Creditor's Report/تقرير الدائنين--%>
        <asp:Button ID="btn_filter" runat="server" class="filter right_align_list" OnClick="btn_filter_OnClick" />
        <%--<asp:Button ID="btnexcel_export" runat="server" class="btn_excel right_align_list"
            ToolTip="Export to Excel" OnClick="btn_excel_OnClick" />--%>
             <asp:HiddenField ID="hdn_user_id" runat="server" />
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
                                Agent  
                                <telerik:RadComboBox ID="drpAgent" ClientIDMode="AutoID" Sort="Ascending" EmptyMessage="Search Agent..."
                                    Filter="Contains" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                    OnClientBlur="ValidateCombo" runat="server" Style="height: 24px !important; width: 86%; overflow: hidden;
                                    border: none!important;">
                                </telerik:RadComboBox>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="drpAgent"
                                    Display="Dynamic" ValidationGroup="save" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Customer  
                                <telerik:RadComboBox ID="drpCustomer" ClientIDMode="AutoID" Sort="Ascending" EmptyMessage="Search Customer..."
                                    Filter="Contains" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                    OnClientBlur="ValidateCombo" runat="server" Style="height: 24px !important; width: 86%; overflow: hidden;
                                    border: none!important;">
                                </telerik:RadComboBox>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drpCustomer"
                                    Display="Dynamic" ValidationGroup="save" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btn_search" ValidationGroup="save" class="butn" runat="server" OnClick="btn_search_Click"
                                    Text="Search" />
                                <asp:Button ID="btn_excel" class="butn" runat="server" ValidationGroup="save" Text="Generate Excel"
                                    OnClick="btn_excel_OnClick" />
                                     
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
                                    <th style="width: 5px; white-space: nowrap;width:5%">
                                        Sl No.
                                    </th>
                                    <th style="padding: 5px; white-space: nowrap;width:20%">
                                      Agent Name 
                                    </th>
                                    <th style="padding: 5px; white-space: nowrap;width:20%">
                                      Customer Name 
                                    </th>
                                    <th style="padding: 5px;width:10%">
                                      InvoiceNo
                                    </th>
                                    <th style="padding: 5px;width:10%">
                                      InvoiceAmount
                                    </th>
                                    <th style="padding: 5px;width:10%">
                                      PaidAmount
                                    </th>
                                    <th style="padding: 5px;width:10%">
                                      Balance
                                    </th>
                                    <th style="padding: 5px;width:10%">
                                      GovtFee(Total)
                                    </th>
                                    <th style="padding: 5px;width:10%">
                                      Service(Total)
                                    </th>
                                    <th style="padding: 5px;width:10%">
                                      GovtFeeWorkDone
                                    </th>
                                    <th style="padding: 5px;width:10%">
                                      ServiceWorkDone
                                    </th>
                                     <th style="padding: 5px;width:10%">
                                      CreditAmount
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
                                            <td >
                                                <%#Eval("Agent")%>
                                            </td>
                                            <td >
                                                <%#Eval("Name")%>
                                            </td>
                                             <td >
                                                <%#Eval("InvoiceNo")%>
                                            </td>
                                             <td style="text-align:right" >
                                                <%#Eval("InvoiceAmount")%>
                                            </td>
                                              <td style="text-align:right" >
                                                <%#Eval("PaidAmount")%>
                                            </td>
                                              <td style="text-align:right" >
                                                <%#Eval("Balance")%>
                                            </td>
                                              <td style="text-align:right" >
                                                <%#Eval("GovtFee")%>
                                            </td>
                                              <td style="text-align:right" >
                                                <%#Eval("Service")%>
                                            </td>
                                              <td style="text-align:right" >
                                                <%#Eval("GovtFeeWorkDone")%>
                                            </td>
                                              <td style="text-align:right" >
                                                <%#Eval("ServiceWorkDone")%>
                                            </td>
                                            <td style="text-align:right" >
                                                <%#Eval("CreditAmount")%>
                                            </td>
                                        </tr>
                                       
                                    </ItemTemplate>
                                </asp:Repeater>
                                <tr>
                                    <td colspan="11" style="text-align:right">
                                        Total 
                                    </td>
                                    <td style="text-align:right">
                                        <asp:Label ID="lblTotal" Text="" runat="server"></asp:Label>
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
            <br />
            <br />
            <div class="">
            </div>
            </div> </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_excel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

