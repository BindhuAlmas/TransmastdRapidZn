<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="DocumentReturnAgent.aspx.cs" Inherits="AmarCentre.Transactions.DocumentReturnAgent" %>

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



        }

        function ValidateCombo(sender, eventArgs) {
            var textInTheCombo = sender.get_text();
            if (textInTheCombo != '') {
                var item = sender.findItemByText(textInTheCombo);
                //if there is no item with that text
                sender.get_text()
                if (!item) {
                    sender.set_text("");
                    alert('Select from the list...');

                }
            }
        }

        function OnClientKeyPressing(sender, args) {
            sender.showDropDown(); //show dropdown after entering some characters
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
       Document From Agent
        <asp:Button ID="btn_addnew" runat="server" Text="+" class="btnAddNew" OnClick="btn_newentry_OnClick" />
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
                            <th class="listTableSlNo" style="width: 5%;">
                                Sl No/رقم
                            </th>
                            <th style="width: 10%">
                                Code/الشفرة
                            </th>
                            <th style="width: 10%">
                                Date/تاريخ
                            </th>
                            <th style="width: 25%">
                                 Agent/وكيل
                            </th>
                            <th style="width: 5%">
                                Action/عمل
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
                                        <%#Eval("Name")%>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_new_line" runat="server" CommandName="Edit" ToolTip="Edit" class="btn_edit" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td colspan="5" class="navigationRow">
                                <asp:UpdatePanel ID="Upd_Nav_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
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
                    <div class="animated halfPopUp" >
                        <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="Adding_heading">
                                 Document From Agent
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td style="width: 15%">
                                            Code/الشفرة
                                            </td>
                                        <td  style="width: 25%">
                                            <asp:TextBox ID="lbl_code" runat="server" class="txt read_Only" Style="width: 50%;"
                                                Font-Bold="true" Text=""></asp:TextBox>
                                        </td>
                                         <td style="width: 15%">
                                            Date/تاريخ <span style="color: Red">&nbsp*</span>
                                        </td>
                                        <td style="width: 25%">
                                            <telerik:RadDatePicker ID="on_date" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                <Calendar runat="server" ID="Calendar1" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                    ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                    <SpecialDays>
                                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                        </telerik:RadCalendarDay>
                                                    </SpecialDays>
                                                </Calendar>
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator ID="req_on_date" runat="server" ControlToValidate="on_date"
                                                ValidationGroup="save" ErrorMessage="Required" Style="color: Red" InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Agent/وكيل<span style="color: Red">&nbsp*</span>
                                        </td>
                                        <td colspan="3">
                                            <telerik:RadComboBox ID="Drp_Cust" Sort="Ascending" Filter="Contains" runat="server"
                                                AutoPostBack="true" OnSelectedIndexChanged="drp_Cust_OnSelectedIndexChanged"
                                                AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Agent"
                                                Style="width: 50%; overflow: hidden; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                OnClientBlur="ValidateCombo">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ErrorMessage="Required"
                                                runat="server" ControlToValidate="Drp_Cust" ValidationGroup="save" Style="color: Red"
                                                Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:UpdatePanel ID="Upd_doc" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="Rpt_Doc" />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <table class="listTable" style="width: 98%; font-size: 15px">
                                                    <thead>
                                                        <tr>
                                                            <td colspan="8">
                                                                <span style="float: left">Document List/قائمة الخصم </span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th style="width: 5%">
                                                                Select / اختار
                                                            </th>
                                                            <th style="width: 3%">
                                                                Sl./رقم
                                                            </th>
                                                            <th style="width: 15%">
                                                                Document / وثيقة  
                                                            </th>
                                                            <th style="width: 10%">
                                                                Document No
                                                            </th>
                                                            <th style="width: 15%">
                                                                Particular / بصفة خاصة  
                                                            </th>
                                                            <th style="width: 10%">
                                                                Valid From / صالح من تاريخ 
                                                            </th>
                                                            <th style="width: 10%">
                                                                Valid To / صالح ل  

                                                            </th>
                                                            <th style="width: 10%">
                                                                Remarks / ملاحظات
                                                            </th>
                                                        </tr>
                                                        </thead>
                                                        <asp:Repeater ID="Rpt_Doc" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td style="text-align: center">
                                                                        <asp:CheckBox ID="chk_sel" runat="server" Checked='<%# Convert.ToBoolean(Eval("selected")) %>' />
                                                                    </td>
                                                                    <td>
                                                                        <%# Container.ItemIndex + 1 %>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbl_name" Text='<%#Eval("Doc_name") %>' runat="server"></asp:Label>
                                                                        <asp:HiddenField ID="hdn_doc_id" runat="server" Value='<%#Eval("Doc_Id") %>' />
                                                                        <asp:HiddenField ID="hdn_D_id" runat="server" Value='<%#Eval("D_id") %>' />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbl_num" Text='<%#Eval("Doc_num") %>' runat="server"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:HiddenField ID="hdn_file" runat="server" Value='<%#Eval("filename") %>' />
                                                                        <asp:Label ID="lbl_remark" Text='<%#Eval("Remark") %>' runat="server"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbl_from" runat="server" Text='<%#string.Format("{0:dd/MM/yyyy}", Eval("V_from"))%>'></asp:Label>
                                                                        <asp:HiddenField ID="v_frm" runat="server" Value='<%#Eval("V_from")%>' />
                                                                        <asp:HiddenField ID="v_to" runat="server" Value='<%#Eval("V_To")%>' />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbl_to" runat="server" Text='<%#string.Format("{0:dd/MM/yyyy}", Eval("V_To"))%>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbl_newremark" Text='<%#Eval("NewRemark") %>' runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Remark/تعليق
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txt_desc" TextMode="MultiLine" CssClass="txt_80" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdn_user_id" runat="server" />
                                            <asp:Button ID="btn_save" class="butn_save" ValidationGroup="save" OnClick="btn_save_OnClick"
                                                runat="server" Text="Save/حفظ" />
                                            <asp:Button ID="btn_save_print" class="butn_save" ValidationGroup="save" OnClick="btn_save_print_OnClick"
                                                runat="server" Text="Save & Print/حفظ وطباعة" />
                                            <asp:Button ID="btn_delete" class="butn_delete" runat="server" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');"
                                                Visible="false" Text="Delete/حذف" OnClick="btn_delete_OnClick" />
                                            <asp:Button ID="btn_print" class="butn" runat="server" Visible="false" Text="Print/طباعة"
                                                OnClick="btn_print_OnClick" />
                                            <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_OnClick" />
                                            <asp:Button ID="Button1" class="butn" runat="server" Text="Close/قريب" OnClick="btn_close_OnClick" />
                                            <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdn_update" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdn_delete" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdn_print" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdn_add_N_print" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdn_update_N_print" runat="server" Value="0" />
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
                            <asp:Label ID="lbl_msg" runat="server" class="messageLabel"></asp:Label>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
