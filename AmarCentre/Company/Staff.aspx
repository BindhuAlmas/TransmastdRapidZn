<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="Staff.aspx.cs" Inherits="AmarCentre.Company.Staff" %>


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

            $('.readOnly').attr('readonly', true);

            $('.txtpay').blur(function (e) {
                var pay = 0;
                var bal = 0;
                if ($('.txtpaybal').val() != '') {
                    bal = parseFloat($('.txtpaybal').val());
                }
                if ($('.txtpay').val() != '') {
                    pay = parseFloat($('.txtpay').val());
                }
                if (parseFloat(pay) > parseFloat(bal)) {
                    alert('Amount cannot be greater than Balance amount');
                    $('.txtpay').val('');
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Company Staff
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
                            <th style="width: 5%">Sl No/رقم
                            </th>
                            <th style="width: 15%">Name/اسم
                            </th>
                            <th style="width: 8%">Company
                            </th>
                            <th style="width: 9%">Contact No
                            </th>
                            <th style="width: 5%">Action/عمل
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
                                        <%#Eval("Name")%>
                                    </td>
                                    <td>
                                        <%#Eval("CompanyName")%>
                                    </td>
                                    <td>
                                        <%#Eval("ContactNo")%>
                                    </td>
                                    <td class="listTableActionButtonDiv">
                                        <asp:Button ID="btn_edit" runat="server" class="btn_edit" />
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
                    <div class="animated smallPopUp">
                        <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="Adding_heading">
                                    Company Staff
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td>Name/اسم <span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txt_name" CssClass="txt" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txt_name"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        <td>Company<span style="color: Red">&nbsp*</span>
                                            <telerik:RadComboBox ID="drpCompany" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" RenderMode="Lightweight"
                                                EmptyMessage="Search Company..." OnClientFocus="OnClientKeyPressing" Style="overflow: hidden; width: 97%; border: none!important;">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drpCompany"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Contact No<span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txt_mob" runat="server" class="txt numbers_only"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_mob"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>

                                        <td>Email/البريد الالكتروني 
                                            <br />
                                            <asp:TextBox ID="txt_email" CssClass="txt" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Please Enter Valid Email ID"
                                                ValidationGroup="save" ControlToValidate="txt_email" Style="color: Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td colspan="2">Address/العنوان 
                                            <br />
                                            <asp:TextBox ID="txt_address" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">Remark/تعليق
                                            <br />
                                            <asp:TextBox ID="txt_remark" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Agreement Amount<span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txtAgreeamt" runat="server" class="txt numbers_only"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAgreeamt"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>

                                        <td>Received Amount
                                            <asp:TextBox ID="txtPaid" class="txt readOnly" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div>
                                                <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_user_id" runat="server" />
                                                <asp:Button ID="btn_save" class="butn_save" ValidationGroup="save" OnClick="btn_save_OnClick"
                                                    runat="server" Text="Save/حفظ" />
                                                <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_OnClick" />
                                                <asp:Button ID="btn_delete" class="butn_delete" runat="server" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');"
                                                    Visible="false" Text="Delete/حذف" OnClick="btn_delete_OnClick" />
                                                <asp:Button ID="btnchecklist" class="butn_save" runat="server" Visible="false" Text="Checklist" OnClick="btnchecklist_Click" />
                                                <asp:Button ID="btnpayment" class="butn_save" runat="server" Visible="false" Text="Payment" OnClick="btnpayment_Click" />
                                                <asp:Button ID="Button2" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_OnClick" />
                                                <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_update" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_delete" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnchecklist" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnpayment" runat="server" Value="0" />
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
                <asp:UpdatePanel ID="Upd_PaymentPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlPayment" Visible="false" runat="server">
                            <div class="popupBackground">
                            </div>
                            <div class="animated smallPopUp" style="width: 50%">
                                <asp:UpdatePanel ID="Upd_PaymentPanelIn" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>

                                        <div class="Adding_heading">
                                            Payment Detail
                                        </div>
                                        <table class="formTable">
                                            <tr>
                                                <td>Balance
                                            <asp:TextBox ID="txtpaybal" runat="server" class="txt readOnly txtpaybal"></asp:TextBox>

                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>Received Amount <span style="color: Red">&nbsp*</span>
                                                    <asp:TextBox ID="txtPay" runat="server" class="txt numbers_only txtpay"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtPay"
                                                        ValidationGroup="payadd" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                </td>
                                                <td>Date/تاريخ <span style="color: Red">&nbsp*</span>
                                                    <telerik:RadDatePicker ID="paydate" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                        <Calendar runat="server" ID="Calendar3" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                            ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                            <SpecialDays>
                                                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                </telerik:RadCalendarDay>
                                                            </SpecialDays>
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                                    <asp:RequiredFieldValidator ID="Rqd_date" runat="server" ControlToValidate="paydate"
                                                        ValidationGroup="payadd" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">Remark/تعليق
                                            <br />
                                                    <asp:TextBox ID="txtPayremark" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Button ID="btn_PaySave" runat="server" class="butn_save" ValidationGroup="payadd"
                                                        Text="Save/حفظ" OnClick="btn_PaySave_Click" />
                                                    <asp:Button ID="btnpayclose" class="butn" runat="server" Text="Close/أغلق" OnClick="btnpayclose_Click" />
                                                </td>
                                            </tr>
                                        </table>

                                        <div style="height: 10px"></div>
                                        <asp:Panel runat="server" ID="pnlpaymenthistory" Visible="false">
                                            <table class="listTable">
                                                <thead>
                                                    <tr>
                                                        <th style="width: 5%">Sl
                                                        </th>
                                                        <th style="width: 9%">Date
                                                        </th>
                                                        <th style="width: 8%">Amount
                                                        </th>
                                                        <th style="width: 15%">Remark
                                                        </th>
                                                        <th style="width: 10%">Done by
                                                        </th>
                                                        <th style="width: 13%">Created date
                                                        </th>
                                                        <th style="width: 5%">Action
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptpayhistory" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <%# Container.ItemIndex+1%>
                                                                    <asp:HiddenField runat="server" ID="hdnDId" Value='<%#Eval("Id")%>' />
                                                                </td>
                                                                <td>
                                                                    <%#Eval("Paydate")%>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("PaidAmount")%>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("Remark")%>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("Name")%>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("CreatedDates")%>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btn_paydelete" runat="server" class="btn_delete" OnClick="btn_paydelete_Click"
                                                                        OnClientClick="javascript : return confirm('Do you really want to delete.. ?');" />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </asp:Panel>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="UpdCheckList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlCheckList" Visible="false" runat="server">
                            <div class="popupBackground">
                            </div>
                            <div class="animated smallPopUp" style="width: 50%">
                                <div class="Adding_heading">
                                    CheckList
                                </div>
                                <asp:UpdatePanel ID="updCheckListIn" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table class="listTable">
                                            <thead>
                                                <tr>
                                                    <th style="width: 5%">Sl
                                                    </th>
                                                    <th style="width: 15%">Particular
                                                    </th>
                                                     <th style="width: 10%">Expense
                                                    </th>
                                                    <th style="width: 10%">Status
                                                    </th>
                                                    <th style="width: 8%">Action
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="rptChecklist" runat="server" OnItemCommand="rptChecklist_ItemCommand">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <%# Container.ItemIndex+1%>
                                                                <asp:HiddenField ID="hdnDId" runat="server" Value='<%#Eval("Id")%>' />
                                                                <asp:HiddenField ID="hdnexpense" runat="server" Value='<%#Eval("Expense")%>' />
                                                                <asp:HiddenField ID="hdnchkId" runat="server" Value='<%#Eval("CheckListId")%>' />
                                                            </td>
                                                            <td>
                                                                <%#Eval("Name")%>
                                                            </td>
                                                             <td>
                                                                 <%#Eval("Expense")%>
                                                                  
                                                            </td>
                                                            <td>
                                                                <%#Eval("Status")%>
                                                            </td>
                                                            <td>
                                                                <asp:HiddenField ID="hdnfilename" runat="server" Value='<%#Eval("Filenames")%>' />
                                                                <asp:HiddenField ID="hdnfilenamesave" runat="server" Value='<%#Eval("FilenameSave")%>' />
                                                                <asp:Button ID="btnExpense" CssClass="btn_edit" runat="server" CommandName="Expense"
                                                                   ToolTip="Add/Edit Expense"  />
                                                                <asp:Button ID="btnInlineSave" CssClass="btn_completeTick" runat="server" CommandName="Complete"
                                                                    Visible='<%# Convert.ToBoolean(Eval("IsCbtnView")) %>' ToolTip="Complete" />
                                                                <asp:Button ID="Button3" CssClass="btn_doc_up" runat="server" CommandName="UploadFile"
                                                                    Visible='<%# Convert.ToBoolean(Eval("IsFbtnview")) %>' ToolTip="UploadFile" />
                                                                <asp:Button ID="Button4" CssClass="btn_doc_down" runat="server" CommandName="DownloadFile"
                                                                    Visible='<%# !Convert.ToBoolean(Eval("IsFbtnview")) %>' ToolTip="DownloadFile" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>
                                        <div>
                                            <asp:Button ID="Button1" class="butn" runat="server" Text="Close/أغلق" OnClick="btnchecklistclose_Click" />
                                        </div>

                                        <asp:UpdatePanel ID="Updfileup" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlfileup" Visible="false" runat="server">
                                                    <div class="popupBackground">
                                                    </div>
                                                    <div class="animated smallPopUp">
                                                        <div class="Adding_heading">
                                                            Fileupload
                                                        </div>
                                                        <table class="formTable">
                                                            <tr>
                                                                <td>
                                                                    <telerik:RadAsyncUpload ID="fu_fileupload" MaxFileSize="500000000" runat="server"
                                                                        MaxFileInputsCount="1" OnFileUploaded="fu_fileupload_FileUploaded">
                                                                    </telerik:RadAsyncUpload>
                                                                    <asp:HiddenField ID="hdnfilenameout" runat="server" />
                                                                    <asp:HiddenField ID="hdnfilenamesaveout" runat="server" />
                                                                    <asp:HiddenField ID="hdnDidout" runat="server" />
                                                                    <asp:HiddenField ID="hdnchkIdOut" runat="server" />


                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div>
                                                                        <asp:Button ID="btnFileupload" runat="server" class="butn_save" Text="Upload" OnClick="btnFileupload_Click" />
                                                                        <asp:Button ID="btnFUclose" class="butn" runat="server" Text="Close/إغلاق" OnClick="btnFUclose_Click" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <asp:UpdatePanel ID="updexpense" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlexpense" Visible="false" runat="server">
                                                    <div class="popupBackground">
                                                    </div>
                                                    <div class="animated smallPopUp">
                                                        <div class="Adding_heading">
                                                            Add/Edit Expense
                                                        </div>
                                                        <table class="formTable">
                                                            <tr>
                                                                <td>
                                                                   <asp:TextBox ID="txtExpense" runat="server" class="txt numbers_only"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtExpense"
                                                        ValidationGroup="Expenseadd" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div>
                                                                         <asp:HiddenField ID="hdnDidExp" runat="server" />
                                                                    <asp:HiddenField ID="hdnchkIdExp" runat="server" />

                                                                        <asp:Button ID="Button5" runat="server" ValidationGroup="Expenseadd" class="butn_save" Text="Save" OnClick="btnExpenseaddClick" />
                                                                        <asp:Button ID="Button6" class="butn" runat="server" Text="Close/إغلاق" OnClick="btnExpenseaddclose_Click" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="rptChecklist" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>


