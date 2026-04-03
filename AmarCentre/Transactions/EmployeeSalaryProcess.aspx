<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true"
    CodeBehind="EmployeeSalaryProcess.aspx.cs" Inherits="AmarCentre.Transactions.EmployeeSalaryProcess" %>

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
            /*Unit Price,Amount,Discount*/
            $('.inline').blur(function (e) {
                $('.salary').each(function () {
                    var Salary = 0;
                    var Addition = 0;
                    var Deduction = 0;
                    var IncentiveAmt = 0;

                    var TotalSalary = 0;


                    if ($(this).closest("tr").find('.salary').val() != '') {
                        Salary = parseFloat($(this).closest("tr").find('.salary').val());
                    }
                    if ($(this).closest("tr").find('.addition').val() != '') {
                        Addition = parseFloat($(this).closest("tr").find('.addition').val());
                    }

                    if ($(this).closest("tr").find('.deduction').val() != '') {
                        Deduction = parseFloat($(this).closest("tr").find('.deduction').val());
                    }
                    //if ($(this).closest("tr").find('.incentiveAmt').val() != '') {
                    //    IncentiveAmt = parseFloat($(this).closest("tr").find('.incentiveAmt').val());
                    //}

                    TotalSalary = (parseFloat(Salary) + parseFloat(Addition) - parseFloat(Deduction) + parseFloat(IncentiveAmt)).toFixed(2);

                    $(this).closest("tr").find('.totalSalary').val(parseFloat(TotalSalary).toFixed(2));

                });
                Calc();
            });

            function Calc() {
                var Total = 0;
                $('.totalSalary').each(function () {
                    var TotalSalary = 0;
                    if ($(this).closest("tr").find('.totalSalary').val() != '') {
                        TotalSalary = parseFloat($(this).closest("tr").find('.totalSalary').val());
                    }
                    Total = parseFloat(Total) + parseFloat(TotalSalary);
                });

                $('.total').val(parseFloat(Total).toFixed(2));
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Employee Salary Process/عملية راتب الموظف
        <asp:Button ID="btn_addnew" runat="server"   class="btnAddNew" OnClick="btn_newentry_OnClick" />
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
                             <th style="width: 10%;">
                               Date/ تاريخ
                            </th>
                            <th style="width: 10%;">
                                Code/رمز
                            </th>
                            <th style="width: 15%;">
                                Month/Year/شهر /سنة
                            </th>
                            <th style="width: 10%;">
                                Amount/المبلغ
                            </th>
                            <th class="listTableAction" style="width: 5%;">
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
                                        <%#Eval("Date")%>
                                    </td>
                                    <td>
                                        <%#Eval("Code")%>
                                    </td>
                                    <td>
                                        <%#Eval("MonthAndYear")%>
                                    </td>
                                    <td>
                                        <%#Eval("Amount")%>
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
                    <div class="animated largePopUp">
                        <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="Adding_headingLargepopup">
                                    Employee Salary Process/عملية راتب الموظف
                                </div>
                                <table class="formTable" >
                                    <tr>
                                        <td style="width: 25%">
                                            Code/رمز
                                            <asp:TextBox ID="lbl_Code" runat="server" class="txt read_Only" Style="width: 90%;"
                                                Font-Bold="true" Text=""></asp:TextBox>
                                        </td>
                                     <td style="width: 25%">Date / تاريخ <span style="color: Red">&nbsp*</span>
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
                                        <td style="width: 25%">
                                            Month/شهر<span style="color: Red">&nbsp*</span>
                                            <telerik:RadComboBox ID="drpMonth" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                OnClientBlur="ValidateCombo" AutoPostBack="true" OnSelectedIndexChanged="EmployeeDetails"
                                                EmptyMessage="Search Month..." Style="overflow: hidden; width: 90%; border: none!important;">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="drpMonth"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                   
                                        <td style="width: 25%">
                                            Year/سنة<span style="color: Red">&nbsp*</span>
                                            <telerik:RadComboBox ID="drpYear" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                AutoPostBack="true" OnSelectedIndexChanged="EmployeeDetails" OnClientBlur="ValidateCombo"
                                                EmptyMessage="Search Year..." Style="overflow: hidden; width: 90%; border: none!important;">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="drpYear"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                                <table class="formTable" style="width: 98%;">
                                    <tr>
                                        <td colspan="4">
                                            <div id="div_item_new" runat="server" style="width: 99%; overflow: auto;">
                                                <div style="height: 10px">
                                                </div>
                                                <asp:UpdatePanel ID="Upd_ItemList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table class="listTable">
                                                            <thead>
                                                                <tr style="text-align: center">
                                                                    <th style="width: 3%">
                                                                        Sl
                                                                    </th>
                                                                    <th style="width: 23%">
                                                                        Employee/موظف
                                                                    </th>
                                                                    <th style="width: 10%">
                                                                        Salary/راتب
                                                                    </th>
                                                                    <th style="width: 10%">
                                                                        Addition/اضافة
                                                                    </th>
                                                                    <th style="width: 10%">
                                                                        Deduction/المستقطع
                                                                    </th>
                                                                    <%--<th style="width: 8%">
                                                                        Incentive/حافز
                                                                    </th>--%>
                                                                    <th style="width: 9%">
                                                                        Total/مجموع
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <asp:Repeater ID="rpt_Item_list" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr style="text-align: center">
                                                                            <td>
                                                                                <%# Container.ItemIndex + 1 %>
                                                                                <asp:HiddenField ID="hdnEmpSalaryProcessDId" runat="server" Value='<%#Eval("EmpSalaryProcessDId") %>' />
                                                                                <asp:HiddenField ID="hdnEmpAttendanceDId" runat="server" Value='<%#Eval("EmpAttendanceDId") %>' />
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:HiddenField ID="hdnEmployeeId" runat="server" Value='<%#Eval("EmployeeId") %>' />
                                                                                <asp:Label ID="lblEmployeename" TabIndex="-1" runat="server" Text='<%#Eval("EmployeeName") %>'></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txtSalary" TabIndex="-1" class="numbers_only salary txt asLabel read_Only"
                                                                                    Width="85%" runat="server" Text='<%#Eval("Salary") %>'></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txtAddition" class="numbers_only inline addition txt" Width="85%"
                                                                                    runat="server" Text='<%#Eval("Addition") %>'></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txtDeduction" class="numbers_only inline deduction txt" Width="85%"
                                                                                    runat="server" Text='<%#Eval("Deduction") %>'></asp:TextBox>
                                                                            </td>
                                                                            <%--<td style="text-align: left">
                                                                                <asp:TextBox ID="txtIncentiveAmount" TabIndex="-1" class="numbers_only incentiveAmt txt asLabel read_Only"
                                                                                    Width="85%" runat="server" Text='<%#Eval("IncentiveAmount") %>'></asp:TextBox>
                                                                            </td>--%>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txtTotalSalary" TabIndex="-1" class="numbers_only totalSalary txt asLabel read_Only"
                                                                                    Width="85%" runat="server" Text='<%#Eval("TotalSalary") %>'></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                                <tr>
                                                                    <td colspan="4">
                                                                    </td>
                                                                    <td style="text-align: right">
                                                                        Total/مجموع
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox Style="border: medium none; color: Red; font-size: 24px; text-align: right;
                                                                            width: 95%" class="txt total readOnly" ID="txtTotal" runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>
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
                                        <td colspan="4">
                                            Remarks/ملاحظات
                                            <asp:TextBox class="txtarea" Style="width: 50%" TextMode="MultiLine" ID="txtRemark"
                                                runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" rowspan="3">
                                            <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdn_user_id" runat="server" />
                                            <asp:Button ID="btn_save" class="butn_save" ValidationGroup="save" OnClick="btn_save_OnClick"
                                                runat="server" Text="Save/حفظ" />
                                            <asp:Button ID="btnSavePrint" class="butn_save" ValidationGroup="save" runat="server"
                                                Text="Save & Print" OnClick="btnSavePrintOnClick" />
                                            <asp:Button ID="btnPrint" class="butn_save" runat="server" Text="Print" OnClick="btnPrintOnClick" />
                                            <asp:Button ID="btnDelete" class="butn_delete" runat="server" Text="Delete/حذف" OnClientClick="javascript : return confirm('Do you really want to delete.. ?');"
                                                OnClick="btnDelete_OnClick" />
                                            <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_OnClick" />
                                            <asp:Button ID="Button1" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_OnClick" />
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
