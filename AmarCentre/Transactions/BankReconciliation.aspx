<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true"
    CodeBehind="BankReconciliation.aspx.cs" Inherits="AmarCentre.Transactions.BankReconciliation" %>

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
        Bank Reconciliation/التسويات المصرفية
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
                            <th style="width: 10%;">
                                Code/رمز
                            </th>
                            <th style="width: 15%;">
                                From Date/من التاريخ
                            </th>
                            <th style="width: 15%;">
                                To Date/حتي اليوم
                            </th>
                            <th style="width: 15%;">
                                Bank Account/بنك
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
                                        <%#Eval("FromDate")%>
                                    </td>
                                    <td>
                                        <%#Eval("ToDate")%>
                                    </td>
                                    <td>
                                        <%#Eval("BankAccountName")%>
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
                                <div class="Adding_heading">
                                    Bank Reconciliation/التسويات المصرفية

                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td style="width: 25%">
                                             Code/رمز

                                            <asp:TextBox ID="lbl_Code" runat="server" class="txt read_Only" Style="width: 50%;"
                                                Font-Bold="true" Text=""></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            From Date/من التاريخ<span style="color: Red">&nbsp*</span>
                                            <telerik:RadDatePicker ID="txtFromDate" runat="server" class="input-boder" DateInput-DateFormat="dd/MM/yyyy">
                                                <Calendar runat="server" ID="Calendar1" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                    ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                    <SpecialDays>
                                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                        </telerik:RadCalendarDay>
                                                    </SpecialDays>
                                                </Calendar>
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtFromDate"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            To Date/حتي اليوم<span style="color: Red">&nbsp*</span>
                                            <telerik:RadDatePicker ID="txtToDate" runat="server" class="input-boder" DateInput-DateFormat="dd/MM/yyyy">
                                                <Calendar runat="server" ID="Calendar2" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                    ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                    <SpecialDays>
                                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                        </telerik:RadCalendarDay>
                                                    </SpecialDays>
                                                </Calendar>
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtToDate"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Bank Account/بنك<span style="color: Red">&nbsp*</span>
                                            <telerik:RadComboBox ID="drpBankAccount" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                OnClientBlur="ValidateCombo" EmptyMessage="Search Bank Account..." Style="overflow: hidden;
                                                width: 50%; border: none!important;">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drpBankAccount"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Upload Document/مستندات محملة 
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
                                    <tr>
                                        <td colspan="2">
                                            <asp:Button ID="btnProcess" class="butn_save" ValidationGroup="save" OnClick="btnProcessOnClick"
                                                runat="server" Text="Process/معالجة " />
                                        </td>
                                    </tr>
                                </table>
                                <table class="formTable" style="width: 100%;">
                                    <tr>
                                        <td colspan="4">
                                            <div id="div_item_new" runat="server" style="width: 100%; overflow: auto;">
                                                <div style="height: 10px">
                                                </div>
                                                <asp:UpdatePanel ID="Upd_ItemList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div style="height: 200px">
                                                            <asp:Button ID="btnexcelList" runat="server" class="btn_excel right_align_list" ToolTip="Export to Excel"
                                                                OnClick="btnexcel_exportList_OnClick" />
                                                            <table class="listTable">
                                                                <thead>
                                                                    <tr style="text-align: center">
                                                                        <th style="width: 5%; white-space: nowrap">
                                                                            Sl No/رقم.
                                                                        </th>
                                                                        <th style="width: 10%; padding: 5px">
                                                                            Date/تاريخ
                                                                        </th>
                                                                        <th style="width: 15%; padding: 5px; white-space: nowrap">
                                                                            TransAction Id/PRAN /رمز تعريف المعاملة 
                                                                        </th>
                                                                        <th style="width: 25%; padding: 5px; white-space: nowrap">
                                                                            Comment /تعليق - ملاحظة 
                                                                        </th>
                                                                        <th style="width: 10%; padding: 5px;">
                                                                            BankStatement Amount/مبلغ كشف الحساب البنكي 
                                                                        </th>
                                                                        <th style="width: 10%; padding: 5px;">
                                                                            Transmas Amount
                                                                        </th>
                                                                        <th style="width: 10%; padding: 5px;">
                                                                            Transmas Amount Difference
                                                                        </th>
                                                                        <th style="width: 10%; padding: 5px;">
                                                                            BankStatement Amount Difference/فرق مبلغ كشف الحساب البنكي 
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                 
                                                                    <asp:Repeater ID="rpt_Item_list" runat="server">
                                                                        <ItemTemplate>
                                                                       
                                                                            <tr>
                                                                                <td style="text-align: center">
                                                                                    <%# Container.ItemIndex + 1 %>
                                                                                </td>
                                                                                <td style="padding-left: 3px; white-space: nowrap">
                                                                                    <asp:HiddenField ID="hdnDate" runat="server" Value='<%#Eval("Date") %>' />
                                                                                    <asp:Label ID="lblDisplayDate" TabIndex="-1" runat="server" Text='<%#Eval("DisplayDate") %>'></asp:Label>
                                                                                </td>
                                                                                <td style="padding-left: 3px;">
                                                                                    <asp:Label ID="lblTransID" TabIndex="-1" runat="server" Text='<%#Eval("TransActionId") %>'></asp:Label>
                                                                                </td>
                                                                                <td style="padding-left: 3px;">
                                                                                    <asp:Label ID="lblComment" TabIndex="-1" runat="server" Text='<%#Eval("Comment") %>'></asp:Label>
                                                                                </td>
                                                                                <td style="text-align: right">
                                                                                    <asp:Label ID="lblBankStatementAmount" TabIndex="-1" runat="server" Text='<%#Eval("BankStatementAmount") %>'></asp:Label>
                                                                                </td>
                                                                                <td style="text-align: right">
                                                                                    <asp:Label ID="lblApplicationAmount" TabIndex="-1" runat="server" Text='<%#Eval("ApplicationAmount") %>'></asp:Label>
                                                                                </td>
                                                                                <td style="text-align: right">
                                                                                    <asp:Label ID="lblApplicationAmountDifference" TabIndex="-1" runat="server" Text='<%#Eval("ApplicationAmountDifference") %>'></asp:Label>
                                                                                </td>
                                                                                <td style="text-align: right">
                                                                                    <asp:Label ID="lblBankStatementAmountDifference" TabIndex="-1" runat="server" Text='<%#Eval("BankStatementAmountDifference") %>'></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                           
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                    <tr>
                                                                        <td colspan="4">
                                                                            Total/مجموع
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblBSAmount" runat="server" Text=""></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblTransmasAmount" runat="server" Text=""></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblTransmasAmountDiff" runat="server" Text=""></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblBSAmountDiff" runat="server" Text=""></asp:Label>
                                                                        </td>
                                                                    </tr> 
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnexcelList" />
                                                    </Triggers>
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
                                            <asp:Button ID="btnClose" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_OnClick" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
