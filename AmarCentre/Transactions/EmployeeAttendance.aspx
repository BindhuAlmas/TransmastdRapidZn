<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true"
    CodeBehind="EmployeeAttendance.aspx.cs" Inherits="AmarCentre.Transactions.EmployeeAttendance" %>

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

            $('.EmpAttendedDays').blur(function (e) {
                $('.EmpAttendedDays').each(function () {
                    var TotWorkingDay = 0;
                    var EmpAttendedDays = 0;
                    var Salary = 0;
                    var ApplicableSalary = 0;
                    if ($('.TotWorkingDays').val() != '') {
                        TotWorkingDay = parseInt($('.TotWorkingDays').val());
                    }

                    if ($('.EmpAttendedDays').val() != '') {
                        EmpAttendedDays = parseFloat($('.EmpAttendedDays').val());
                    }

                    if ($('.Salary').val() != '') {
                        Salary = parseFloat($('.Salary').val());
                    }
                    if (parseInt(TotWorkingDay) > 0) {
                        ApplicableSalary = ((parseFloat(Salary) / parseInt(TotWorkingDay)) * parseFloat(EmpAttendedDays)).toFixed(2);
                    } else {
                        ApplicableSalary = 0;
                    }

                    $('.ApplicableSalary').val(parseFloat(ApplicableSalary).toFixed(2));
                });
            });

            $('.EmpAttendedDaysin').blur(function (e) {
                $('.EmpAttendedDaysin').each(function () {
                    var TotWorkingDay = 0;
                    var EmpAttendedDays = 0;
                    var Salary = 0;
                    var ApplicableSalary = 0;

                    if ($(this).closest("tr").find('.TotWorkingDaysin').val() != '') {
                        TotWorkingDay = parseInt($(this).closest("tr").find('.TotWorkingDaysin').val());
                    }
                    if ($(this).closest("tr").find('.EmpAttendedDaysin').val() != '') {
                        EmpAttendedDays = parseFloat($(this).closest("tr").find('.EmpAttendedDaysin').val());
                    }
                    if ($(this).closest("tr").find('.Salaryin').val() != '') {
                        Salary = parseFloat($(this).closest("tr").find('.Salaryin').val());
                    }
                    if (parseInt(TotWorkingDay) > 0) {
                        ApplicableSalary = ((parseFloat(Salary) / parseInt(TotWorkingDay)) * parseFloat(EmpAttendedDays)).toFixed(2);
                    }
                    $(this).closest("tr").find('.ApplicableSalaryin').val(parseFloat(ApplicableSalary).toFixed(2));
                });
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Employee Attendance/حضور الموظف
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
                            <th class="listTableSlNo" style="width: 5%;">
                                Sl No/رقم
                            </th>
                            <th style="width: 10%;">
                                Code/رمز
                            </th>
                            <th style="width: 15%;">
                                Month/Year/شهر /سنة
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
                                        <%#Eval("Code")%>
                                    </td>
                                    <td>
                                        <%#Eval("MonthAndYear")%>
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
                        <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="Adding_headingLargepopup">
                                    Employee Attendance/حضور الموظف
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td  style="width:32%">
                                            Code/رمز
                                            <asp:TextBox ID="lbl_Code" runat="server" class="txt read_Only" Style="width: 90%;"
                                                Font-Bold="true" Text=""></asp:TextBox>
                                        </td>
                                   
                                        <td style="width:32%"> 
                                            Month/شهر<span style="color: Red">&nbsp*</span>
                                            <telerik:RadComboBox ID="drpMonth" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                OnClientBlur="ValidateCombo" EmptyMessage="Search Month..." AutoPostBack="true"
                                                OnSelectedIndexChanged="MonthYearChanged" Style="overflow: hidden; width: 90%;
                                                border: none!important;">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="drpMonth"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                   
                                        <td style="width:32%">
                                            Year/سنة<span style="color: Red">&nbsp*</span>
                                            <telerik:RadComboBox ID="drpYear" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                OnClientBlur="ValidateCombo" EmptyMessage="Search Year..." AutoPostBack="true"
                                                OnSelectedIndexChanged="MonthYearChanged" Style="overflow: hidden; width: 90%;
                                                border: none!important;">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="drpYear"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr style="display:none">
                                        <td>
                                            Attendance From Document/الحضور من الوثائق
                                            <br />
                                            <asp:UpdatePanel ID="UpdDocument" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <telerik:RadAsyncUpload ID="fuDocument" MaxFileSize="500000000" runat="server" MaxFileInputsCount="1"
                                                        OnFileUploaded="fuDocumentOnFileUploaded">
                                                    </telerik:RadAsyncUpload>
                                                    <asp:HiddenField ID="hdnfileName" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnfileSaveName" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnfileExtension" runat="server" Value="" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr style="display:none">
                                        <td colspan="2">
                                            <asp:Button ID="btnProcess" class="butn_save" ValidationGroup="save" OnClick="btnProcessOnClick"
                                                runat="server" Text="Process" />
                                        </td>
                                    </tr>
                                </table>
                                <table class="formTable" >
                                    <tr>
                                        <td colspan="4">
                                            <div id="div_item_new" runat="server" style="width: 100%; overflow: auto;">
                                                <div style="height: 10px">
                                                </div>
                                                <asp:UpdatePanel ID="Upd_ItemList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table class="listTable">
                                                            <thead>
                                                                <tr style="text-align: center">
                                                                    <th style="width: 3%">
                                                                        Sl./رقم
                                                                    </th>
                                                                    <th style="width: 23%">
                                                                        Employee/موظف
                                                                    </th>
                                                                    <th style="width: 10%">
                                                                        Total Working Days/مجموع أيام العمل
                                                                    </th>
                                                                    <th style="width: 10%">
                                                                        Employee Attended Days/أيام حضور الموظف
                                                                    </th>
                                                                    <th style="width: 5%">
                                                                        OT at Working Days/في ايام العمل
                                                                    </th>
                                                                    <th style="width: 5%">
                                                                        OT at Weekend/في نهاية عطلة الاسبوع
                                                                    </th>
                                                                    <th style="width: 5%">
                                                                        OT at Holiday/في العطلات الرسمية
                                                                    </th>
                                                                    <th style="width: 5%">
                                                                        Salary/راتب
                                                                    </th>
                                                                    <th style="width: 5%">
                                                                        Applicable Salary/راتب التطبيق
                                                                    </th>
                                                                    <th style="width: 5%">
                                                                        Action /عمل
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <asp:Repeater ID="rpt_Item_list" runat="server" >  <%--OnItemDataBound="rpt_Item_list_OnItemDataBound"--%>
                                                                    <ItemTemplate>
                                                                        <tr style="text-align: center">
                                                                            <td>
                                                                                <%# Container.ItemIndex + 1 %>
                                                                                <%--<asp:HiddenField ID="hdnEmpSalaryProcessDId" runat="server" Value='<%#Eval("EmpSalaryProcessDId") %>' />--%>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:HiddenField ID="hdnAttDId" runat="server" Value='<%#Eval("Id") %>' />
                                                                                <asp:HiddenField ID="hdnAttDEmployeeId" runat="server" Value='<%#Eval("EmployeeId") %>' />
                                                                                <asp:HiddenField ID="hdnFromExcel" runat="server" Value='<%#Eval("FromExcel") %>' />
                                                                                <asp:Label ID="lblAttDEmployeename" TabIndex="-1" runat="server" Text='<%#Eval("EmployeeName") %>'></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txtAttDTotWorkingDays" TabIndex="-1" class="numbers_only txt asLabel read_Only TotWorkingDaysin"
                                                                                    Width="85%" runat="server" Text='<%#Eval("TotalWorkingDays") %>'></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txtAttDEmpAttendedDays" TabIndex="-1" class="numbers_only txt   EmpAttendedDaysin "
                                                                                    Width="85%" runat="server" Text='<%#Eval("EmployeeWorkedDays") %>'></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txtAttDOTAtWorking" TabIndex="-1" class="numbers_only txt asLabel read_Only"
                                                                                    Width="85%" runat="server" Text='<%#Eval("OTAtWorking") %>'></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txtAttDOTAtWeekend" TabIndex="-1" class="numbers_only txt asLabel read_Only"
                                                                                    Width="85%" runat="server" Text='<%#Eval("OTAtWeekend") %>'></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txtAttDOTAtHoliday" TabIndex="-1" class="numbers_only txt asLabel read_Only"
                                                                                    Width="85%" runat="server" Text='<%#Eval("OTAtHoliday") %>'></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txtAttDSalary" TabIndex="-1" class="numbers_only txt asLabel read_Only Salaryin"
                                                                                    Width="85%" runat="server" Text='<%#Eval("Salary") %>'></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txtAttDApplicableSalary" TabIndex="-1" class="numbers_only txt asLabel read_Only ApplicableSalaryin"
                                                                                    Width="85%" runat="server" Text='<%#Eval("ApplicableSalary") %>'></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: center;">
                                                                                <asp:Button ID="btn_edit_line" runat="server" OnClick="btn_edit_line_OnClick" ToolTip="Edit"
                                                                                    class="btn_edit"  Visible="false" />
                                                                                <asp:Button ID="btn_remove_line" CommandName="Delete" class="btn_delete" runat="server"
                                                                                    ToolTip="Delete" OnClick="btn_remove_line_OnClick" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                                <tr style="text-align: center; ">
                                                                    <td>
                                                                        <asp:Label ID="lblRepeaterSNo" Text="" TabIndex="-1" runat="server" />
                                                                        <%--<asp:HiddenField ID="hdnEmpSalaryProcessDId" runat="server" Value='<%#Eval("EmpSalaryProcessDId") %>' />--%>
                                                                    </td>
                                                                    <td style="text-align: left">
                                                                        <asp:HiddenField ID="hdn_AttDetailId" runat="server" Value="" />
                                                                        <asp:UpdatePanel ID="UpdEmployeeDropdown" runat="server" ChildrenAsTriggers="false"
                                                                            UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <telerik:RadComboBox ID="drpEmployee" Sort="Ascending" Filter="Contains" runat="server"
                                                                                    AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Employee..."
                                                                                    OnSelectedIndexChanged="drpEmployeeOnSelectedIndexChanged" AutoPostBack="true"
                                                                                    Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                                    OnClientBlur="ValidateCombo">
                                                                                </telerik:RadComboBox>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="*" runat="server"
                                                                                    ControlToValidate="drpEmployee" ValidationGroup="addService" Style="color: Red"
                                                                                    Display="Dynamic"></asp:RequiredFieldValidator>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                    <td style="text-align: left">
                                                                        <asp:UpdatePanel ID="UpdTotWorkingDays" runat="server" ChildrenAsTriggers="false"
                                                                            UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:TextBox ID="txtTotWorkingDays" TabIndex="-1" class="numbers_only txt asLabel read_Only TotWorkingDays"
                                                                                    Width="85%" runat="server" Text=""></asp:TextBox>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                    <td style="text-align: left">
                                                                        <asp:UpdatePanel ID="UpdEmpAttendedDays" runat="server" ChildrenAsTriggers="false"
                                                                            UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:TextBox ID="txtEmpAttendedDays" class="numbers_only txt EmpAttendedDays" Width="85%"
                                                                                    runat="server" Text=""></asp:TextBox>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="*" runat="server"
                                                                                    ControlToValidate="txtEmpAttendedDays" ValidationGroup="addService" Style="color: Red"
                                                                                    Display="Dynamic"></asp:RequiredFieldValidator>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                    <td style="text-align: left">
                                                                        <asp:UpdatePanel ID="UpdOTAtWorking" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:TextBox ID="txtOTAtWorking" TabIndex="-1" class="numbers_only txt asLabel read_Only"
                                                                                    Width="85%" runat="server" Text=""></asp:TextBox>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                    <td style="text-align: left">
                                                                        <asp:UpdatePanel ID="UpdOTAtWeekend" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:TextBox ID="txtOTAtWeekend" TabIndex="-1" class="numbers_only txt asLabel read_Only"
                                                                                    Width="85%" runat="server" Text=""></asp:TextBox>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                    <td style="text-align: left">
                                                                        <asp:UpdatePanel ID="UpdOTAtHoliday" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:TextBox ID="txtOTAtHoliday" TabIndex="-1" class="numbers_only txt asLabel read_Only"
                                                                                    Width="85%" runat="server" Text=""></asp:TextBox>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                    <td style="text-align: left">
                                                                        <asp:UpdatePanel ID="UpdSalary" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:TextBox ID="txtSalary" TabIndex="-1" class="numbers_only txt asLabel read_Only Salary"
                                                                                    Width="85%" runat="server" Text=""></asp:TextBox>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                    <td style="text-align: left">
                                                                        <asp:UpdatePanel ID="UpdApplicableSalary" runat="server" ChildrenAsTriggers="false"
                                                                            UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:TextBox ID="txtApplicableSalary" TabIndex="-1" class="numbers_only txt asLabel read_Only ApplicableSalary"
                                                                                    Width="85%" runat="server" Text=""></asp:TextBox>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                    <td style="text-align: center;">
                                                                        <asp:Button ID="btn_new_line" runat="server" OnClick="btn_new_line_OnClick" ToolTip="Add"
                                                                            class="btn_add_new" ValidationGroup="addService" />
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
                                    <td colspan="4" rowspan="3">
                                        <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdn_user_id" runat="server" />
                                        <asp:Button ID="btn_save" class="butn_save" ValidationGroup="save" OnClick="btn_save_OnClick"
                                            runat="server" Text="Save/حفظ" />
                                        <asp:Button ID="btnDelete" class="butn_delete" runat="server" Text="Delete/حذف" OnClientClick="javascript : return confirm('Do you really want to delete.. ?');"
                                            OnClick="btnDelete_OnClick" />
                                        <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_OnClick" />
                                        <asp:Button ID="Button1" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_OnClick" />
                                        <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdn_update" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdn_delete" runat="server" Value="0" />
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
