<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="Profitsharing.aspx.cs" Inherits="AmarCentre.Transactions.Profitsharing" %>


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

            $('.read_Only').attr('readonly', true);

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Profit Sharing
        <asp:Button ID="btn_addnew" runat="server" class="btnAddNew" OnClick="btn_newentry_OnClick" />
        <asp:Button ID="btnexcel_export" runat="server" class="btn_excel right_align_list"
            ToolTip="Export to Excel" OnClick="btnexcel_export_OnClick" />
        <div class="searchDiv">
            <asp:TextBox ID="txt_search" runat="server" class="txt_search" AutoPostBack="true"
                OnTextChanged="txt_search_OnTextChanged" placeholder="Search"></asp:TextBox>
        </div>
    </div>
    <div>
        <asp:UpdatePanel ID="Upd_List_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="Common_order_column" runat="server" />
                <asp:HiddenField ID="Common_asc_desc" runat="server" />
                <div class="list_info" style="display: none">
                </div>
                <table class="listTable">
                    <thead>
                        <tr>
                            <th class="listTableSlNo" style="width: 5%;">Sl No/رقم
                            </th>
                            <th style="width: 10%;">Code/رمز
                            </th>
                            <th style="width: 10%;">Date
                            </th>
                            <th style="width: 15%;">Month/Year/شهر /سنة
                            </th>
                            <th style="width: 10%;">Status
                            </th>
                            <th class="listTableAction" style="width: 5%;">Action/عمل
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rpt_list" runat="server" OnItemCommand="rpt_list_OnItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%#Eval("RowNum")%>.
                                        <asp:HiddenField ID="hdn_id" runat="server" Value='<%#Eval("Id")%>' />
                                    </td>
                                    <td>
                                        <%#Eval("Code")%>
                                    </td>
                                    <td>
                                        <%#Eval("Dates")%>
                                    </td>
                                    <td>
                                        <%#Eval("MonthAndYear")%>
                                    </td>
                                    <td>
                                        <%#Eval("Statusname")%>
                                    </td>
                                    <td class="listTableActionButtonDiv">
                                        <asp:Button ID="btn_edit" runat="server" class="btn_edit" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td colspan="6" class="navigationRow">
                                <asp:UpdatePanel ID="Upd_Nav_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
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
                                        <asp:HiddenField ID="hdn_filter" runat="server" />
                                        <asp:HiddenField ID="hdn_last_page" runat="server" />
                                        <div class="head_second_div" style="display: none">
                                            <asp:HiddenField ID="hdn_total" runat="server" Value="0" />
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnexcel_export" />
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
    <div>
        <asp:UpdatePanel ID="Upd_Add_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnl_add" Visible="false" runat="server">
                    <div class="popupBackground">
                    </div>
                    <div class="animated largePopUp">
                        <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="Adding_headingLargepopup">
                                    Profit Sharing
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td style="width: 33%">Code/رمز
                                            <asp:TextBox ID="lbl_Code" runat="server" class="txt read_Only" Style="width: 70%;"
                                                Font-Bold="true" Text=""></asp:TextBox>
                                        </td>
                                        <td style="width: 33%">Date / تاريخ <span style="color: Red">&nbsp*</span>
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
                                            <asp:RequiredFieldValidator ID="req_on_date" runat="server" ControlToValidate="job_date"
                                                ValidationGroup="save" ErrorMessage="Required" Style="color: Red" InitialValue=""></asp:RequiredFieldValidator>

                                        </td>
                                        <td style="width: 33%">Month/شهر<span style="color: Red">&nbsp*</span>
                                            <telerik:RadComboBox ID="drpMonth" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                OnClientBlur="ValidateCombo" AutoPostBack="true" OnSelectedIndexChanged="FillProfitDetails"
                                                EmptyMessage="Search Month..." Style="overflow: hidden; width: 70%; border: none!important;">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="drpMonth"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Year/سنة<span style="color: Red">&nbsp*</span>
                                            <telerik:RadComboBox ID="drpYear" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                AutoPostBack="true" OnSelectedIndexChanged="FillProfitDetails" OnClientBlur="ValidateCombo"
                                                EmptyMessage="Search Year..." Style="overflow: hidden; width: 70%; border: none!important;">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="drpYear"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>

                                        <td>Net Profit
                                             <asp:UpdatePanel ID="UpdCurrentProfit" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                 <ContentTemplate>
                                                     <asp:TextBox ID="txtCurrentProfit" runat="server" class="txt read_Only" Style="width: 70%;"
                                                         Text=""></asp:TextBox>
                                                 </ContentTemplate>
                                             </asp:UpdatePanel>
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                                <asp:UpdatePanel ID="Upd_ItemList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table class="formTable" style="width: 98%;">
                                            <tr>
                                                <td style="width: 48%"><b>Partner Share<br />
                                                </b></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>

                                                    <table class="listTable">
                                                        <thead>
                                                            <tr style="text-align: center">
                                                                <th style="width: 3%">Sl./رقم
                                                                </th>
                                                                <th style="width: 23%">Partner
                                                                </th>

                                                                <th style="width: 9%">Share %
                                                                </th>
                                                                <th style="width: 9%">Amount
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:Repeater ID="rptPartnerShare" runat="server">
                                                                <ItemTemplate>
                                                                    <tr style="text-align: center">
                                                                        <td>
                                                                            <%# Container.ItemIndex + 1 %>
                                                                        </td>
                                                                        <td style="text-align: left">
                                                                            <asp:HiddenField ID="hdnPartnerId" runat="server" Value='<%#Eval("PartnerId") %>' />
                                                                            <asp:Label ID="lblPartnername" TabIndex="-1" runat="server" Text='<%#Eval("Name") %>'></asp:Label>
                                                                        </td>
                                                                        <td style="text-align: left">
                                                                            <asp:TextBox ID="txtSharePercentage" TabIndex="-1" class="numbers_only txt asLabel read_Only"
                                                                                Width="85%" runat="server" Text='<%#Eval("SharePercentage") %>'></asp:TextBox>
                                                                        </td>
                                                                        <td style="text-align: left">
                                                                            <asp:TextBox ID="txtPartnerShare" TabIndex="-1" class="numbers_only txt asLabel read_Only"
                                                                                Width="85%" runat="server" Text='<%#Eval("PartnerShare") %>'></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </tbody>
                                                    </table>
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <table class="formTable">
                                    <tr>
                                        <td colspan="2">Remarks/ملاحظات
                                            <asp:TextBox class="txtarea" Style="width: 50%" TextMode="MultiLine" ID="txtRemark"
                                                runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" rowspan="3">
                                            <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdn_user_id" runat="server" />
                                            <asp:Button ID="btn_save" class="butn_save" ValidationGroup="save" OnClick="btn_save_OnClick"
                                                runat="server" Text="Save/حفظ" />
                                            <asp:Button ID="btnCancel" class="butn_delete" runat="server" Text="Cancel/إلغاء"
                                                OnClientClick="javascript : return confirm('Do you really want to cancel.. ?');"
                                                OnClick="btnCancel_Click" />
                                            <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_OnClick" />
                                            <asp:Button ID="Button1" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_OnClick" />
                                            <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdncancel" runat="server" Value="0" />

                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </asp:Panel>
                <div>
                    <div id="div_pop2" class="messageAlert div_pop animated" style="display: none" runat="server">
                        <div class="tick">
                            &#10004
                        </div>
                        <div>
                            <asp:Label ID="lbl_msgin" runat="server" class="messageLabel"></asp:Label>
                        </div>
                    </div>
                </div>
                 <div>
                    <div id="div1" class="messageAlerterror div_poperror animated" style="display: none" runat="server">
                        <div class="tick">
                            &#10007</div>
                        <div>
                            <asp:Label ID="lblerrormsg" runat="server" class="messageLabel"></asp:Label>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

