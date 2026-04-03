<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Customer.Master" AutoEventWireup="true" CodeBehind="CustomerDocDwnld.aspx.cs" Inherits="AmarCentre.Customer.CustomerDocDwnld" %>




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
                <span style="font-size: 30px; cursor: pointer; color: white;" class="nav">&#9776; Download Document</span>
                <span style="font-size: 30px; cursor: pointer; color: white;" class="nav2">&#9776; Download Document</span>
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

         <div style="text-align: right; margin-right: 1%">
        <asp:TextBox ID="txt_search" runat="server" class="txt_search" AutoPostBack="true" Width="25%"
            OnTextChanged="txt_search_OnTextChanged" placeholder="Search"></asp:TextBox>
 </div>
        <div style="height: 10px"></div>
        <asp:HiddenField ID="hdn_user_id" runat="server" />

        <div>
            <div class="list-div">
                <div class="listbox">
                    <div class="content-box">
                        <asp:UpdatePanel ID="Upd_List_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table>
                                    <thead>
                                        <tr style="text-align: center; background-color: #272e56;">
                                            <th style="width: 3%;">Sl
                                            </th>
                                            <th style="width: 15%;">Staff
                                            </th>
                                            <%--<th style="width: 10%;">Document Name
                                            </th>--%>
                                            <th style="width: 10%;">Document Type
                                            </th>
                                            <th style="width: 12%;">Document Number
                                            </th>
                                            <th style="width: 8%;">Valid From
                                            </th>
                                            <th style="width: 8%;">Valid Till
                                            </th>
                                            <th style="width: 5%;">Action
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rpt_list" runat="server" OnItemCommand="rpt_doc_list_OnItemCommand_Staff">
                                            <ItemTemplate>
                                                <tr style="text-align: center; background-color: #272e56;" onmouseover="this.style.backgroundColor='#1b203d';" onmouseout="this.style.backgroundColor='#272e56';">
                                                    <td>
                                                        <%# Eval("dt_indx")%>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_staffname" runat="server" Text='<%# Eval("StaffName")%>'></asp:Label>
                                                    </td>
                                                  <%--  <td>
                                                        <asp:Label ID="lbl_docname" runat="server" Text='<%# Eval("Document_name")%>'></asp:Label>
                                                    </td>--%>
                                                    <td>
                                                        <asp:Label ID="lbl_doc_type_name" runat="server" Text='<%# Eval("doc_type")%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_docnum" runat="server" Text='<%# Eval("DocNumber")%>'></asp:Label><br />
                                                    </td>
                                                    <td style="text-align: left;">
                                                        <asp:Label ID="lbl_from" runat="server" Text='<%#string.Format("{0:dd/MM/yyyy}", Eval("Valid_From"))%>'></asp:Label>
                                                    </td>
                                                    <td style="text-align: left;">
                                                        <asp:Label ID="lbl_to" runat="server" Text='<%#string.Format("{0:dd/MM/yyyy}", Eval("Valid_To"))%>'></asp:Label>
                                                        <asp:Label ID="lbl_remark" Visible="false" runat="server" Text='<%# Eval("Remark")%>'></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:HiddenField ID="hdn_indx" Value='<%#Eval("dt_indx")%>' runat="server" />
                                                        <asp:HiddenField ID="hdnVyr" Value='<%#Eval("ValidityYear")%>' runat="server" />
                                                        <asp:HiddenField ID="hdn_doc_Id" Value='<%#Eval("DocumentTypeId")%>' runat="server" />
                                                        <asp:HiddenField ID="hdn_id" Value='<%#Eval("Id")%>' runat="server" />
                                                        <asp:Label ID="lbl_doc_name" Visible="false" runat="server" Text='<%# Eval("DocumentName")%>'></asp:Label>
                                                        <asp:HiddenField ID="hdn_dnm" Value='<%#Eval("DocumentSave")%>' runat="server" />
                                                        <asp:HiddenField ID="v_frm" runat="server" Value='<%#Eval("Valid_From")%>' />
                                                        <asp:HiddenField ID="v_to" runat="server" Value='<%#Eval("Valid_To")%>' />
                                                        <asp:Button ID="btn_doc_dwnld" ToolTip="Download" CssClass="btn_doc_down" runat="server"
                                                            CommandName="Download" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr>
                                            <td colspan="7" class="navigationRow">
                                                <asp:UpdatePanel ID="Upd_Nav_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:HiddenField ID="Common_order_column" runat="server" />
                                                        <asp:HiddenField ID="Common_asc_desc" runat="server" />
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
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="rpt_list" />
                                                    </Triggers>
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

                </div>
            </div>
        </div>
    </div>
</asp:Content>

