<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="AgentOutstanding.aspx.cs" Inherits="AmarCentre.Reports.AgentOutstanding" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
       Agent Commission Outstanding Report
        <asp:Button ID="btnexcel_export" runat="server" class="btn_excel right_align_list"
            ToolTip="Export to Excel" OnClick="btn_excel_OnClick" />
             <asp:HiddenField ID="hdn_user_id" runat="server" />
    </div>
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
                                       Name 
                                    </th>
                                    <th style="padding: 5px; white-space: nowrap;width:10%">
                                       Mobile No
                                    </th>
                                    <th style="padding: 5px;width:10%">
                                      Outstanding Amount
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
                                                <%#Eval("Name")%>
                                            </td>
                                            <td style="padding-left: 3px; white-space: nowrap">
                                                <%#Eval("ContactNo")%>
                                            </td>
                                            <td style="padding-left: 3px; white-space: nowrap;text-align:right">
                                                <%#Eval("Amount")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <tr>
                                    <td colspan="3">
                                        Total
                                    </td>
                                    <td style="text-align:right">
                                        <asp:Label ID="lblTotalOSAmount" Text="" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="navigationRow">
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
            <asp:PostBackTrigger ControlID="btnexcel_export" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

