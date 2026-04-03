<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="EmployeeSalary.aspx.cs" Inherits="AmarCentre.Masters.EmployeeSalary" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function ToggleDiv() {
            $('.div_pop:hidden').show();
            setTimeout(function () { $(".div_pop").hide(); }, 2000);
        }
        function pageLoad() {
            $('.div_items').click(function (e) {

                $('.div_items').css('background-color', 'White');
                $('.div_items').css('color', 'Black');
                $(this).css('background-color', '#0078d7');
                $(this).css('color', 'White');

            }
            );

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
            $('.readOnly').attr('readonly', true);

            /*Unit Price,Amount,Discount*/
            $('.amt').blur(function (e) {

                Calc();
            });


        }

        function Calc() {
            var ILTotAmt = 0;
            var GrandTotAmt = 0;

            $('.amt').each(function () {
                if ($(this).closest("tr").find('.amt').val() != '') {
                    if ($(this).closest("tr").find('#hdn_Type').val() == '1') {
                        ILTotAmt = ILTotAmt + parseFloat($(this).closest("tr").find('.amt').val());
                    }
                    else {
                        ILTotAmt = ILTotAmt - parseFloat($(this).closest("tr").find('.amt').val());
                    }
                }
            });

            $('.tot_amt').val(parseFloat(ILTotAmt).toFixed(2));
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
        Employee Salary/راتب الموظف

        <asp:Button ID="btn_addnew" runat="server"  class="btnAddNew" OnClick="btn_newentry_OnClick" />
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
                            <th style="width: 5%;">Sl No/رقم
                            </th>
                            <th style="width: 10%;">Code/رمز

                            </th>
                            <th style="width: 15%;">Employee Name/موظف

                            </th>
                            <th style="width: 5%;">Action/عمل
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
                                        <%#Eval("Name")%>
                                    </td>
                                    <td class="listTableActionButtonDiv">
                                        <asp:Button ID="btn_edit" runat="server" class="btn_edit" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td colspan="4" class="navigationRow">
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
                    <div class="animated halfPopUp">
                        <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="Adding_heading">
                                    Employee Salary/راتب الموظف
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td>Code/رمز
                                            <asp:Label ID="lbl_Code" runat="server" class="Eng_lang" Font-Bold="true" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Employee <span style="color: Red">&nbsp*</span>
                                            <telerik:RadComboBox ID="drp_empl" Sort="Ascending" runat="server" RenderMode="Lightweight"
                                                EmptyMessage="Search Employee..." AllowCustomText="true" Filter="Contains" OnClientFocus="OnClientKeyPressing"
                                                OnClientBlur="ValidateCombo" Style="overflow: hidden; width: 46%; border: none!important;">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="drp_empl"
                                                ValidationGroup="save" ErrorMessage="Required" Style="color: Red" InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="Upd_ItemList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div id="div_item_new" runat="server">
                                                    </div>
                                                    <div style="height: 10px">
                                                    </div>
                                                    <table class="listTable">
                                                        <thead>
                                                            <tr style="text-align: center">
                                                                <th style="width: 3%">Sl.No/رقم
                                                                </th>
                                                                <th style="width: 13%">Salary Type/نوع الراتب
                                                                </th>
                                                                <th style="width: 7%">Amount/المبلغ
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:Repeater ID="rpt_Item_list" runat="server">
                                                                <ItemTemplate>
                                                                    <tr style="text-align: center">
                                                                        <td>
                                                                            <%# Container.ItemIndex + 1 %>
                                                                        </td>
                                                                        <td style="text-align: left">
                                                                            <asp:HiddenField ID="hdn_salary_id" runat="server" Value='<%#Eval("SalaryId") %>' />
                                                                            <asp:HiddenField ID="hdn_D_Id" runat="server" Value='<%#Eval("D_Id") %>' />
                                                                            <asp:HiddenField ID="hdn_Type" ClientIDMode="Static" runat="server" Value='<%#Eval("Type") %>' />
                                                                            <asp:Label ID="lbl_item" runat="server" Text='<%#Eval("Name") %>'></asp:Label>
                                                                        </td>
                                                                        <td style="text-align: left">
                                                                            <asp:TextBox ID="txt_Amount" class="numbers_only amt inline txt" runat="server"
                                                                                Text='<%#Eval("Amount") %>'></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </tbody>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Total Amount/المبلغ الإجمالي
                        <br />
                                            <asp:TextBox class="tot_amt readOnly txt" Width="46%" ID="txt_tot_amt" runat="server"></asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_user_id" runat="server" />
                                                <asp:Button ID="btn_save" class="butn_save" ValidationGroup="save" OnClick="btn_save_OnClick"
                                                    runat="server" Text="Save/حفظ" />
                                                <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_OnClick" />
                                                <asp:Button ID="btn_delete" class="butn_delete" runat="server" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');"
                                                    Visible="false" Text="Delete/حذف" OnClick="btn_delete_OnClick" />
                                                <asp:Button ID="Button2" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_OnClick" />
                                                <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_update" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_delete" runat="server" Value="0" />
                                            </div>
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
