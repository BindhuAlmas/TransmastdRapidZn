<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Customer.Master" AutoEventWireup="true" CodeBehind="InvoiceList.aspx.cs" Inherits="AmarCentre.Customer.InvoiceList" %>


<%@ Register Src="~/Transactions/UserControl/Customer.ascx" TagName="CustomerMaster"
    TagPrefix="AmarCentre" %>
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
                <span style="font-size: 30px; cursor: pointer; color: white;" class="nav">&#9776; Invoice List</span>
                <span style="font-size: 30px; cursor: pointer; color: white;" class="nav2">&#9776; Invoice List</span>
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
        <div style="height: 10px"></div>
        </div>
        <%--</div>--%>
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
                                        <tr style=" background-color: #272e56;">
                                            <th style="width: 5%;text-align: center;">Sl No
                                            </th>
                                            <th style="width: 10%;text-align: center;">Code
                                            </th>
                                            <th style="width: 10%;text-align: center;">Date 
                                            </th>
                                            <th style="width: 10%;text-align: center;">Amount 
                                            </th>
                                            <th style="width: 10%;text-align: center;">Status  
                                            </th>
                                            <th style="width: 8%;text-align: center;">Action
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rpt_list" runat="server" OnItemCommand="rpt_list_OnItemCommand" OnItemDataBound="rpt_list_OnItemDataBound">
                                            <ItemTemplate>
                                                <tr style=" background-color: #272e56;" onmouseover="this.style.backgroundColor='#1b203d';" onmouseout="this.style.backgroundColor='#272e56';">
                                                    <td style="text-align: center;" >
                                                        <%#Eval("RowNum")%>.
                                        <asp:HiddenField ID="hdn_id" runat="server" Value='<%#Eval("Id")%>' />
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <%#Eval("Code")%>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <%#Eval("Dateds")%>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <%#Eval("AfterDiscount_GrandTotal")%>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <%#Eval("StatusName")%>
                                                    </td>

                                                    <td style="text-align:center">
                                                       <%-- <asp:Button ID="btn_edit" runat="server" class="btn_edit" ToolTip="Edit" CommandName="Edit" />--%>
                                                        <asp:Button ID="btnTaxInvoicePrint" runat="server" class="btn_print" ToolTip="Tax Invoice Print"
                                                            CommandName="TaxInvoicePrint" />
                                                       <%-- <asp:Button ID="btnSalesOrderPrint" runat="server" class="btn_print" ToolTip="Sales Order Print"
                                                            CommandName="SalesOrderPrint" />--%>

                                                        <asp:HiddenField ID="hdnIsCredit" runat="server" Value='<%#Eval("IsCredit")%>' />
                                                        <asp:HiddenField ID="hdnReceived" runat="server" Value='<%#Eval("Received")%>' />
                                                        <asp:HiddenField ID="hdnAfterDiscountGrandTotal" runat="server" Value='<%#Eval("AfterDiscount_GrandTotal")%>' />
                                                        <asp:HiddenField ID="hdnInvPrint" runat="server" Value='<%#Eval("InvoiceFormat")%>' />

                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr>
                                            <td colspan="6" class="navigationRow">
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
                    </div>
                </div>
            </div>
            <div>
            </div>
        </div>
        <div>
            <asp:UpdatePanel ID="Upd_Add_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnl_add" Visible="false" runat="server">
                        <div class="popupBackground">
                        </div>
                        <div class="animated halfPopUpCustomer">
                            <asp:UpdatePanel ID="upd_main" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div id="div_main" runat="server">
                                        <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
                                            UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="headpopup">
                                                    Invoice Details
                                                </div>
                                                <table class="formTable">
                                                    <tr>
                                                        <td style="width: 48%">Code 
                                                        <asp:TextBox ID="lbl_Code" runat="server" class="txt read_Only" Font-Bold="true"
                                                            Text=""></asp:TextBox>
                                                        </td>
                                                        <td style="width: 48%">Date  
                                                        <br />
                                                            <telerik:RadDatePicker ID="job_date" runat="server" DateInput-DateFormat="dd/MM/yyyy">
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
                                                        <td style="width: 48%">Total Amount 
                                                        <asp:TextBox ID="txtamount" runat="server" class="txt read_Only" Font-Bold="true"
                                                            Text=""></asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <div id="div_item_new" runat="server" style="width: 100%; overflow: auto;">
                                                                <div style="height: 10px">
                                                                </div>
                                                                <asp:UpdatePanel ID="Upd_Item_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <table>
                                                                            <thead>
                                                                                <tr style="text-align: center">
                                                                                    <th style="width: 3%">Sl.
                                                                                    </th>
                                                                                    <th style="width: 22%">Service
                                                                                    </th>
                                                                                    <th style="width: 22%">Particular
                                                                                    </th>
                                                                                    <th style="width: 10%">Quantity
                                                                                    </th>
                                                                                    <th style="width: 12%">Amount
                                                                                    </th>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>
                                                                                <asp:Repeater ID="rpt_Item_list" runat="server">
                                                                                    <ItemTemplate>
                                                                                        <tr style="text-align: center" runat="server" id="tr_in">
                                                                                            <td style="width: 5%">
                                                                                                <%# Container.ItemIndex + 1 %>
                                                                                            </td>
                                                                                            <td style="text-align: left">
                                                                                                <%#Eval("Name")%>
                                                                                            </td>
                                                                                            <td style="text-align: left">
                                                                                                <%#Eval("Particulars")%>
                                                                                            </td>
                                                                                            <td>
                                                                                                <%#Eval("Quantity")%>
                                                                                            </td>
                                                                                            <td>
                                                                                                <%#Eval("AfterDiscount_Total")%>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                </asp:Repeater>
                                                                            </tbody>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                <div style="height: 10px">
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdn_user_id" runat="server" />
                                                            <asp:Button ID="Button1" class="butn" runat="server" Text="Close/قريب" OnClick="btn_close_OnClick" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div>
                                                    <div id="div1" class="messageAlert div_pop animated" style="display: none" runat="server">
                                                        <div class="tick">
                                                            &#10004
                                                        </div>
                                                        <div>
                                                            <asp:Label ID="lbl_msgin" Font-Bold="true" ForeColor="White" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
</asp:Content>


