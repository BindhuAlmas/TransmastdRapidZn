<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="DebitReport.aspx.cs" Inherits="AmarCentre.Reports.DebitReport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Debtors Report
        <asp:Label ID="lblheading" runat="server"></asp:Label>
        <asp:Button ID="btnpdf" runat="server" class="btn_pdf right_align_list"
            ToolTip="Generate PDF" OnClick="btnPdfOnClick" />
        <asp:Button ID="btnexcel_export" runat="server" class="btn_excel "
            ToolTip="Export to Excel" OnClick="btn_excel_OnClick" />
        <telerik:RadComboBox ID="drptype" Sort="Ascending" Filter="Contains" runat="server"
            AllowCustomText="true" RenderMode="Lightweight" OnSelectedIndexChanged="drp_count_OnSelectedIndexChanged"
            EmptyMessage="Search ..." OnClientFocus="OnClientKeyPressing" AutoPostBack="true"
            Style="overflow: hidden; width: 20%; float: right; border: none!important;">
            <Items>
                <telerik:RadComboBoxItem Value="1" Text="All" />
                <telerik:RadComboBoxItem Value="2" Text="Typing Center" />
            </Items>
        </telerik:RadComboBox>
        <div class="searchDiv">

            <asp:TextBox ID="txt_search" runat="server" class="txt_search" AutoPostBack="true"
                OnTextChanged="drp_count_OnSelectedIndexChanged" placeholder="Search Customer"></asp:TextBox>
        </div>
        <asp:HiddenField ID="hdn_user_id" runat="server" />
        <asp:HiddenField ID="hdnformat" runat="server" />

    </div>
    <asp:UpdatePanel ID="Upd_addpanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="overflow-x: auto; min-height: 250px; width: 100%">
                <asp:UpdatePanel ID="Upd_List_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table class="listTable" style="width: 98%; border: 1px">
                            <thead>
                                <tr style="text-align: center">
                                    <th style="width: 5px; white-space: nowrap; width: 5%">Sl No.
                                    </th>

                                    <th style="padding: 5px; white-space: nowrap; width: 20%">Name 
                                    </th>
                                      <th style="padding: 5px; white-space: nowrap; width: 15%">Agent
                                    </th>
                                    <th style="padding: 5px; white-space: nowrap; width: 10%">Mobile No
                                    </th>
                                    <th style="padding: 5px; width: 10%">Amount
                                    </th>
                                   
                                    <th style="padding: 5px; white-space: nowrap; width: 7%">Category
                                    </th>
                                    <th style="padding: 5px; white-space: nowrap; width: 7%">Credit Customer
                                    </th>
                                    <th style="padding: 5px; white-space: nowrap; width: 7%">Is Typing Center
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
                                                <asp:HiddenField ID="hdnCustomerId" runat="server" Value='<%#Eval("id")%>' />
                                                <asp:LinkButton ID="lnkcust" runat="server" OnClick="lnkcust_Click" Text='<%#Eval("Name")%>'></asp:LinkButton>
                                            </td>
                                             <td style="padding-left: 3px;  ">
                                                <%#Eval("Agent")%>
                                            </td>
                                            <td style="padding-left: 3px; white-space: nowrap">
                                                <%#Eval("MobileNo")%>
                                            </td>
                                            <td style="padding-right: 3px; text-align:right">
                                                <%#Eval("Amount")%>
                                            </td>
                                              <td style="padding-left: 3px;  ">
                                                <%#Eval("CCategory")%>
                                            </td>
                                            <td style="padding-left: 3px; white-space: nowrap">
                                                <%#Eval("CreditCustomer")%>
                                            </td>
                                            <td style="padding-left: 3px; white-space: nowrap">
                                                <%#Eval("IsTypingCenters")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <tr>
                                    <td colspan="4">Total
                                    </td>
                                    <td style="text-align: right;padding-right: 3px;">
                                        <asp:Label ID="lblTotalAmount" Text="" runat="server"></asp:Label>
                                    </td>
                                    <td colspan="3"></td>
                                </tr>
                                <tr>
                                    <td colspan="8" class="navigationRow">
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
            <asp:PostBackTrigger ControlID="btnexcel_export" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


