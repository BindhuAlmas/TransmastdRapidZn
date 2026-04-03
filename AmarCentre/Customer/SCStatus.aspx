<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Customer.Master" AutoEventWireup="true" CodeBehind="SCStatus.aspx.cs" Inherits="AmarCentre.Customer.SCStatus" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">

        function pageLoad() {

            $('.numbers_only').keydown(function (e) {
                if ($.inArray(e.keyCode, [8, 9, 27, 13, 110, 190]) !== -1 ||
                    // Allow: Ctrl+A, Command+A
                    (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                    // Allow: home, end, left, right, down, up
                    (e.keyCode >= 35 && e.keyCode <= 40)) {
                    // let it happen, don't do anything
                    return;
                }
                // Ensure that it is a number and stop the keypress
                if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
                }
            });
            /*Read Only*/
            $('.read_Only').attr('readonly', true);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div id="main">
        <div class="head">
            <div class="col-div-6">
                <span style="font-size: 30px; cursor: pointer; color: white;" class="nav">&#9776; Work Status</span>
                <span style="font-size: 30px; cursor: pointer; color: white;" class="nav2">&#9776; Work Status</span>
            </div>
            <div class="col-div-6">
                <div class="profile">
                    <img src="../Images/profiles.png" class="pro-img" />
                    <p>
                        <asp:Label ID="lbl_User_name" runat="server" Font-Size="Large" ForeColor="White"></asp:Label>
                    </p>
                </div>
            </div>
            <div class="clearfix"></div>
        </div>

        <%--<div class="searchDiv">--%>

        <asp:TextBox ID="txt_search" runat="server" class="txt_search" AutoPostBack="true"
            OnTextChanged="txt_search_OnTextChanged" placeholder="Search"></asp:TextBox>

        <div style="height: 10px"></div>
        <asp:HiddenField ID="hdn_user_id" runat="server" />

        <div>
            <div class="list-div">
                <div class="listbox">
                    <div class="content-box">
                        <asp:UpdatePanel ID="Upd_List_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:HiddenField ID="Common_order_column" runat="server" />
                                <asp:HiddenField ID="Common_asc_desc" runat="server" />
                                <div class="list_info" style="display: none">
                                </div>
                                <table>
                                    <thead>
                                        <tr style="text-align: center; background-color: #272e56;">
                                            <th style="width: 6%;">Sl No 
                                            </th>
                                            <th style="width: 9%;">Code 
                                            </th>
                                            <th style="width: 9%;">Date
                                            </th>
                                            <th style="width: 25%;">Service
                                            </th>
                                            <th style="width: 15%;">Applicant
                                            </th>
                                            <th style="width: 9%;">Status
                                            </th>
                                            <th style="width: 5%;">Action
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rpt_list" runat="server" OnItemCommand="rpt_list_OnItemCommand">
                                            <ItemTemplate>
                                                <tr style="text-align: center; background-color: #272e56;" onmouseover="this.style.backgroundColor='#1b203d';" onmouseout="this.style.backgroundColor='#272e56';">
                                                    <td style="padding: 5px;">
                                                        <%#Eval("RowNum")%>.
                                        <asp:HiddenField ID="hdn_id" runat="server" Value='<%#Eval("invdId")%>' />
                                                    </td>
                                                    <td style="padding: 5px;">
                                                        <%#Eval("Code")%>
                                                    </td>
                                                    <td style="padding: 5px;">
                                                        <%#Eval("Dates")%>
                                                    </td>
                                                    <td style="padding: 5px;">
                                                        <%#Eval("Servicename")%>
                                                    </td>
                                                    <td style="padding: 5px;">
                                                        <%#Eval("Particulars")%>
                                                    </td>
                                                    <td style="padding: 5px;">
                                                        <%#Eval("CompletionStatus")%>
                                                    </td>
                                                    <td style="padding: 5px;">
                                                        <asp:Button ID="btn_edit" runat="server" CssClass="butn" Text="View Files" CommandName="Edit" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr>
                                            <td colspan="7" class="navigationRow">
                                                <asp:UpdatePanel ID="Upd_Nav_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lbl_page_info" runat="server" class="pageInfo"></asp:Label>
                                                        <asp:Button ID="btn_first" runat="server" Text="<<" OnClick="btn_first_OnClick" />
                                                        <asp:Button ID="btn_prev" runat="server" Text="<" OnClick="btn_prev_OnClick" />
                                                        <asp:Label ID="lbl_page_number" Style="font-weight: bold; margin-left: 5px; margin-right: 5px; text-align: center;"
                                                            runat="server"></asp:Label>
                                                        <asp:Button ID="btn_next" runat="server" Text=">" OnClick="btn_next_OnClick" />
                                                        <asp:Button ID="btn_last" runat="server" Text=">>" OnClick="btn_last_OnClick" />
                                                        <asp:DropDownList ID="drp_count" class="pageSize" runat="server" AutoPostBack="true"
                                                            OnSelectedIndexChanged="drp_count_OnSelectedIndexChanged">
                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="hdn_filter" runat="server" />
                                                        <asp:HiddenField ID="hdn_last_page" runat="server" />
                                                        <div class="head_second_div" style="display: none">
                                                            <asp:HiddenField ID="hdn_total" runat="server" Value="0" />
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div>
                        </div>
                    </div>
                    <div>
                        <asp:UpdatePanel ID="Upd_Add_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="rptfiledown" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Panel ID="pnl_add" Visible="false" runat="server">
                                    <div class="popupBackground">
                                    </div>
                                    <div class="animated smallPopUpCustomer" style="width: 35%">
                                        <div class="headpopup">
                                            Download Files
                                        </div>
                                        <table class="formTable">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="nofile" runat="server" Text="No File added" Visible="false"></asp:Label>
                                                    <asp:Panel ID="pnlfiledwn" Visible="false" runat="server">
                                                        <div style="height: 10px">
                                                        </div>
                                                        <table>
                                                            <%--have doubt--%>
                                                            <thead>
                                                                <tr style="text-align: center">
                                                                    <th style="width: 28%">File
                                                                    </th>
                                                                    <th style="width: 8%">Action
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <asp:Repeater ID="rptfiledown" runat="server" OnItemCommand="rptfileOnItemCommand">
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblfile" runat="server" Text='<%#Eval("FileNames") %>'></asp:Label>
                                                                                <asp:HiddenField ID="hdnfilesave" runat="server" Value='<%#Eval("FileSAveNames") %>' />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnDownload" runat="server" class="btn_doc_down" ToolTip="Download File"
                                                                                    CommandName="Download" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </tbody>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div>
                                                        <asp:Button ID="btnViewClose" class="butn" runat="server" Text="Close" OnClick="btnViewClose_OnClick" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
         </div>
</asp:Content>

