<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="DebitNote.aspx.cs" Inherits="AmarCentre.Transactions.DebitNote" %>

<%@ Register Src="~/Transactions/UserControl/UCMail.ascx" TagName="MailUC" TagPrefix="AmarCentre" %>

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

            $('.dnpaidAmount').blur(function (e) {


                CalculationDebitNote();

            });
            $('.txtdnreceivedamt').blur(function (e) {
                var dnamount = 0;
                if ($.trim($(this).closest("tr").find('.dnpaidAmount').val()) != '') {
                    dnamount = parseFloat($(this).closest("tr").find('.dnpaidAmount').val());
                }
                var dnreceivedamt = 0;
                if ($.trim($(this).closest("tr").find('.txtdnreceivedamt').val()) != '') {
                    dnreceivedamt = parseFloat($(this).closest("tr").find('.txtdnreceivedamt').val());
                }

                if (parseFloat(dnreceivedamt) > parseFloat(dnamount)) {
                    alert("Amount Cannot be greater than debitnote Amount");
                    $(this).closest("tr").find('.txtdnreceivedamt').val('').focus();
                    return;
                }

                CalculationDebitNote();

            });
            function CalculationDebitNote() {

                var GrndTotAmt = 0;

                $('.dnpaidAmount').each(function () {
                    var Amt = 0;
                    var VAT = 0;
                    var PayableAmt = 0;
                    if ($.trim($(this).closest("tr").find('.dnpaidAmount').val()) != '') {
                        Amt = parseFloat($(this).closest("tr").find('.dnpaidAmount').val());
                    }

                    GrndTotAmt = GrndTotAmt + (parseFloat(Amt));
                });


                $('.tot_grnd_amt').val(parseFloat(GrndTotAmt).toFixed(2));
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Debit Note
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
                            <th style="width: 5%;">Sl No /رقم
                            </th>
                            <th style="width: 8%;">Date / تاريخ
                            </th>
                            <th style="width: 8%;">Code / رمز
                            </th>
                            <th style="width: 10%;">Invoice Code / رمز الفاتورة
                            </th>
                            <th style="width: 10%;">Vendor
                            </th>
                            <th style="width: 10%;">Customer
                            </th>
                            <th style="width: 10%;">Service
                            </th>

                            <%--<th style="width: 20%;">Customer / زبون
                            </th>--%>
                            <th style="width: 10%;">Amount / المبلغ
                            </th>
                            <th style="width: 8%;">Status
                            </th>
                            <th style="width: 5%;">Action/عمل
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rpt_list" runat="server" OnItemCommand="rpt_list_OnItemCommand"
                            OnItemDataBound="rpt_list_OnItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%#Eval("RowNum")%>.
                                        <asp:HiddenField ID="hdn_id" runat="server" Value='<%#Eval("Id")%>' />
                                    </td>
                                    <td>
                                        <%#Eval("Dateds")%>
                                    </td>
                                    <td>
                                        <%#Eval("Code")%>
                                    </td>
                                    <td>
                                        <%#Eval("InvoiceCode")%>
                                    </td>
                                    <td>
                                        <%#Eval("Vendor")%>
                                    </td>
                                    <td>
                                        <%#Eval("Customer")%>
                                    </td>
                                    <td>
                                        <%#Eval("Service")%>
                                    </td>
                                    <%--<td>
                                        <%#Eval("Name")%>
                                    </td>--%>

                                    <td>
                                        <%#Eval("TotalAmount")%>
                                    </td>
                                    <td>
                                        <%#Eval("StatusName")%>
                                    </td>
                                    <td class="listTableActionButtonDiv">
                                        <asp:Button ID="btn_edit" runat="server" class="btn_edit" ToolTip="Edit" CommandName="Edit" />
                                        <asp:Button ID="btnPrint" runat="server" class="btn_print" ToolTip="Print" CommandName="Print" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td colspan="10" class="navigationRow">
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
                                    Debit Note
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td style="width: 25%">Code
                                            <asp:TextBox ID="lbl_Code" runat="server" class="txt read_Only" Font-Bold="true"
                                                Text=""></asp:TextBox>
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

                                        <td style="width: 25%">Customer
                                            <telerik:RadComboBox ID="drp_customer" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                AutoPostBack="true" OnClientBlur="ValidateCombo" EmptyMessage="Search Name..."
                                                OnSelectedIndexChanged="drp_customer_OnSelectedIndexChanged" Style="overflow: hidden; width: 96%; border: none!important;">
                                            </telerik:RadComboBox>
                                        </td>

                                        <td style="width: 25%">
                                            <asp:UpdatePanel ID="updinvoiceDrp" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    Invoice Code / رمز الفاتورة
                                                    <telerik:RadComboBox ID="drpInvoice" Sort="Ascending" Filter="Contains" runat="server"
                                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                        AutoPostBack="true" OnClientBlur="ValidateCombo" EmptyMessage="Search invoice..."
                                                        OnSelectedIndexChanged="drpInvoiceOnSelectedIndexChanged" Style="overflow: hidden; width: 96%; border: none!important;">
                                                    </telerik:RadComboBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%">
                                            <asp:UpdatePanel ID="updsc" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    SC No
                                                    <telerik:RadComboBox ID="drpSC" Sort="Ascending" Filter="Contains" runat="server"
                                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                        AutoPostBack="true" OnClientBlur="ValidateCombo" EmptyMessage="Search sc..."
                                                        OnSelectedIndexChanged="drpSC_SelectedIndexChanged" Style="overflow: hidden; width: 96%; border: none!important;">
                                                    </telerik:RadComboBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>Service Name
                                                        <asp:TextBox ID="txtDNServiceName" runat="server" class="txt read_Only" Font-Bold="true"
                                                            Text=""></asp:TextBox>
                                        </td>
                                        <td style="width: 25%">Qty
                                            <asp:TextBox ID="txtdnqty" runat="server" class="txt numbers_only read_Only" Font-Bold="true"
                                                Text=""></asp:TextBox>

                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                                <table class="formTable">
                                    <tr>
                                        <td>
                                            <div id="div5" runat="server" style="width: 100%; overflow: auto;">
                                                <div style="height: 10px">
                                                </div>
                                                <table class="listTable">
                                                    <thead>
                                                        <tr>
                                                            <th style="width: 10%">Expense
                                                            </th>
                                                            <th style="width: 8%">Amount
                                                            </th>
                                                            <th style="width: 6%">VAT
                                                            </th>
                                                            <th style="width: 20%">Vendor
                                                            </th>

                                                            <th style="width: 8%">Total Amount
                                                            </th>
                                                            <th style="width: 10%">Debit Note Amt.
                                                            </th>
                                                            <th style="width: 10%">Received Amt.
                                                            </th>
                                                            <th style="width: 15%">Payment Mode
                                                            </th>
                                                            <th style="width: 15%">Account
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <asp:Repeater ID="rptdnexpense" runat="server" OnItemDataBound="rptdnexpense_ItemDataBound">
                                                        <ItemTemplate>
                                                            <tr class="temp">
                                                                <td>
                                                                    <asp:HiddenField ID="hdndnSerComDetailId" runat="server" Value='<%#Eval("SerComDetailId") %>' />
                                                                    <asp:HiddenField ID="hdn_expenseId" runat="server" Value='<%#Eval("ExpenseId") %>' />
                                                                    <asp:Label ID="lbl_Expense" runat="server" Text='<%# Eval("ExpenseName") %>' />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="txt_amt" runat="server"
                                                                        Text='<%#Eval("Amount") %>'></asp:Label>

                                                                </td>

                                                                <td>
                                                                    <asp:Label ID="txt_vat" runat="server"
                                                                        Text='<%#Eval("VAT") %>'></asp:Label>

                                                                </td>

                                                                <td>

                                                                    <asp:HiddenField ID="hdn_vendorId" runat="server" Value='<%#Eval("VendorId") %>' />
                                                                    <asp:Label ID="txtdnvendor" Class="" runat="server" Text='<%#Eval("vendorname") %>'></asp:Label>
                                                                </td>

                                                                <td>
                                                                    <asp:Label ID="txt_payableAmount" Class="" runat="server" Text='<%#Eval("PayableAmount") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="upddnpaidAmountIn" runat="server" ChildrenAsTriggers="false"
                                                                        UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox ID="txt_dnpaidAmount" Width="90%" Text='<%#Eval("DebitNoteAmt") %>' Class="txt numbers_only dnpaidAmount" runat="server"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldVtor44" runat="server" ControlToValidate="txt_dnpaidAmount"
                                                                                ValidationGroup="savedb" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                                InitialValue="">
                                                                            </asp:RequiredFieldValidator>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="upddnreceivedAmountIn" runat="server" ChildrenAsTriggers="false"
                                                                        UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox ID="txtdnreceivedamt" Width="90%" Text='<%#Eval("debitnotereceAmt") %>' Class="txt numbers_only txtdnreceivedamt" runat="server"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtdnreceivedamt"
                                                                                ValidationGroup="savedb" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                                InitialValue="">
                                                                            </asp:RequiredFieldValidator>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td>
                                                                    <asp:HiddenField ID="hdn_dnpayModeId" runat="server" Value='<%#Eval("PayModeId") %>' />
                                                                    <telerik:RadComboBox ID="drp_dnpayMode" Sort="Ascending" Filter="Contains" runat="server"
                                                                        AllowCustomText="false" RenderMode="Lightweight" DropDownWidth="200px" EmptyMessage="Search Payment Mode..."
                                                                        Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                        OnClientBlur="ValidateCombo" ClientIDMode="AutoID" OnSelectedIndexChanged="drp_dnpayMode_SelectedIndexChanged"
                                                                        AutoPostBack="true">
                                                                        <Items>
                                                                            <telerik:RadComboBoxItem Value="1" Text="Petty Cash" />
                                                                            <telerik:RadComboBoxItem Value="3" Text="Internet Banking" />
                                                                            <telerik:RadComboBoxItem Value="5" Text="Loan" />
                                                                            <telerik:RadComboBoxItem Value="7" Text="Top Up" />
                                                                            <telerik:RadComboBoxItem Value="9" Text="Receiving Later" />
                                                                        </Items>
                                                                    </telerik:RadComboBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator66" runat="server" ControlToValidate="drp_dnpayMode"
                                                                        ValidationGroup="savedb" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                        InitialValue="">
                                                                    </asp:RequiredFieldValidator>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="Upd_dnAccount_Panel" runat="server" ChildrenAsTriggers="false"
                                                                        UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:HiddenField ID="hdn_dnaccountId" runat="server" Value='<%#Eval("AccountId") %>' />
                                                                            <telerik:RadComboBox ID="drp_dnaccount" DropDownWidth="200px" Sort="Ascending" Filter="Contains" runat="server"
                                                                                AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Account..."
                                                                                Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                                OnClientBlur="ValidateCombo">
                                                                            </telerik:RadComboBox>
                                                                            <asp:RequiredFieldValidator ID="rqddnaccountIn" runat="server" ControlToValidate="drp_dnaccount"
                                                                                ValidationGroup="savedb" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                                InitialValue="">
                                                                            </asp:RequiredFieldValidator>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>

                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                    <tr>
                                                        <td colspan="6" style="text-align: right">Total Amount
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:TextBox TabIndex="-1" Style="border: medium none; color: Red; font-size: 24px; text-align: right; width: 95%"
                                                                class="txt tot_grnd_amt read_Only txt_80" ID="txt_grand" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div style="height: 10px">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Button ID="btnDebitNoteSave" class="butn_save" ValidationGroup="save" OnClick="btnDebitNoteSave_Click"
                                                runat="server" Text="Save/حفظ" />
                                            <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_OnClick" />
                                            <asp:Button ID="btndnclose" class="butn" runat="server" Text="Close/أغلق" OnClick="btndnclose_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" rowspan="3" style="text-align: right">
                                            <asp:HiddenField ID="hdn_invId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdn_invStatus" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdn_user_id" runat="server" />

                                            <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdn_update" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdn_id" runat="server" Value="0" />

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
                            &#10007
                        </div>
                        <div>
                            <asp:Label ID="lblerrormsg" runat="server" class="messageLabel"></asp:Label>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>

</asp:Content>

