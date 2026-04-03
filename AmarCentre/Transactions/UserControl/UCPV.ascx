<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCPV.ascx.cs" Inherits="AmarCentre.Transactions.UserControl.UCPV" %>

<%@ Register Src="~/Masters/UserControl/UCExpense.ascx" TagName="ExpenseMaster"
    TagPrefix="AmarCentre" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:UpdatePanel ID="Upd_Add_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnl_add" runat="server">

            <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
                UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="Adding_heading">
                        Payment Voucher/قسيمة دفع
                    </div>
                    <table class="formTable">
                        <tr>
                            <td style="width: 33%">Code/رمز
                                            <asp:Label ID="lblCode" Font-Bold="true" runat="server" class="lbl"></asp:Label>
                            </td>
                            <td style="width: 33%">Date/تاريخ <span style="color: Red">&nbsp*</span>
                                <telerik:raddatepicker id="dtdated" runat="server" dateinput-dateformat="dd/MM/yyyy">
                                                <Calendar runat="server" ID="Calendar1" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                    ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                    <SpecialDays>
                                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                        </telerik:RadCalendarDay>
                                                    </SpecialDays>
                                                </Calendar>
                                            </telerik:raddatepicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="dtdated"
                                    ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                            <td style="width: 33%" rowspan="5">
                                <asp:UpdatePanel ID="updVdeposit" runat="server" ChildrenAsTriggers="false"
                                    UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div id="divVdeposit" runat="server" visible="false">
                                            <div style="overflow: auto; max-height: 350px;">
                                                <table class="listTable" style="width: 97%">
                                                    <thead>
                                                        <tr>
                                                            <th style="width: 15%">Select
                                                            </th>
                                                            <th style="width: 27%">Invoice

                                                            </th>
                                                            <th style="width: 27%">Pending 
                                                            </th>
                                                            <th style="width: 27%">Pay
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater ID="rptVdeposit" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chk_select" runat="server" class="chk_sel Vdepositsupchkitem" Checked='<%# Convert.ToBoolean(Eval("Selectd")) %>' />
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("Code") %>
                                                                        <asp:HiddenField ID="hdninvoiceId" runat="server" Value='<%#Eval("InvoiceId") %>' />
                                                                        <asp:HiddenField ID="hdninvdetId" runat="server" Value='<%#Eval("InvoiceDetId") %>' />
                                                                        <asp:HiddenField ID="hdn_D_id" runat="server" Value='<%#Eval("Id") %>' />
                                                                    </td>
                                                                    <td style="text-align: right">
                                                                        <asp:TextBox ID="txtVdepositBalAmt" runat="server" class="txt_lable read_Only VdepositBalAmt"
                                                                            Text='<%#Eval("Balance") %>'></asp:TextBox>
                                                                    </td>
                                                                    <td style="text-align: right">
                                                                        <asp:TextBox ID="txtVdepositPayAmt" runat="server" Font-Size="12px" class="txt_100 numbers_only VdepositPayAmt"
                                                                            Text='<%#Eval("Pay") %>'></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tbody>
                                                    <tr>
                                                        <td colspan="3" style="text-align: right">Total
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtVdepositTotAmt" runat="server" class="txt_lable VdepositTotAmt read_Only"></asp:TextBox>

                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </td>
                        </tr>
                        <tr>
                            <td>To Type/لكتابة <span style="color: Red">&nbsp*</span>
                                <telerik:radcombobox id="drpTo" sort="Ascending" filter="Contains" runat="server"
                                    onselectedindexchanged="drpToOnSelectedIndexChanged" allowcustomtext="false" rendermode="Lightweight"
                                    onclientfocus="OnClientKeyPressing" autopostback="true" onclientblur="ValidateCombo"
                                    emptymessage="Search Type..." style="overflow: hidden; width: 96%; border: none!important;">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="1" Text="Customer" />
                                                    <telerik:RadComboBoxItem Value="2" Text="Vendor" />
                                                    <telerik:RadComboBoxItem Value="3" Text="Employee" />
                                                    <telerik:RadComboBoxItem Value="4" Text="Cash" />
                                                    <telerik:RadComboBoxItem Value="5" Text="Bank Account" />
                                                    <telerik:RadComboBoxItem Value="7" Text="Loan" />
                                                    <telerik:RadComboBoxItem Value="8" Text="Commission" />
                                                    <telerik:RadComboBoxItem Value="9" Text="Partner" />
                                                    <telerik:RadComboBoxItem Value="10" Text="Deposit" />
                                                    <telerik:RadComboBoxItem Value="12" Text="Agent" />
                                                     <telerik:RadComboBoxItem Value="11" Text="VAT Payment" />
                                                     <telerik:RadComboBoxItem Value="13" Text="Customer Deposit Return" />
                                                    <telerik:RadComboBoxItem Value="6" Text="General Expense" />
                                                </Items>
                                            </telerik:radcombobox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="drpTo"
                                    ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdTo" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblToLabel" runat="server" class="lbl" Text="Customer Name"></asp:Label>
                                        <telerik:radcombobox id="drpCustomer" sort="Ascending" filter="Contains" runat="server"
                                            allowcustomtext="false" rendermode="Lightweight" onclientfocus="OnClientKeyPressing"
                                            onselectedindexchanged="drpCustomerOnSelectedIndexChanged" autopostback="true"
                                            onclientblur="ValidateCombo" emptymessage="Search Name..." style="overflow: hidden; width: 96%; border: none!important;"
                                            visible="true">
                                                    </telerik:radcombobox>
                                        <telerik:radcombobox id="drpVendor" sort="Ascending" filter="Contains" runat="server"
                                            allowcustomtext="false" rendermode="Lightweight" onclientfocus="OnClientKeyPressing"
                                            autopostback="true" onselectedindexchanged="drpVendorOnSelectedIndexChanged"
                                            onclientblur="ValidateCombo" emptymessage="Search Vendor..." style="overflow: hidden; width: 96%; border: none!important;"
                                            visible="false">
                                                    </telerik:radcombobox>
                                        <telerik:radcombobox id="drpEmployee" sort="Ascending" filter="Contains" runat="server"
                                            allowcustomtext="false" rendermode="Lightweight" onclientfocus="OnClientKeyPressing"
                                            onselectedindexchanged="drpEmployeeOnSelectedIndexChanged" autopostback="true"
                                            onclientblur="ValidateCombo" emptymessage="Search Employee..." style="overflow: hidden; width: 96%; border: none!important;"
                                            visible="false">
                                                    </telerik:radcombobox>
                                        <telerik:radcombobox id="drpPettyCash" sort="Ascending" filter="Contains" runat="server"
                                            allowcustomtext="false" rendermode="Lightweight" onclientfocus="OnClientKeyPressing"
                                            onclientblur="ValidateCombo" emptymessage="Search Name..." style="overflow: hidden; width: 96%; border: none!important;"
                                            visible="false">
                                                    </telerik:radcombobox>
                                        <telerik:radcombobox id="drpBankAccount" sort="Ascending" filter="Contains" runat="server"
                                            allowcustomtext="false" rendermode="Lightweight" onclientfocus="OnClientKeyPressing"
                                            onclientblur="ValidateCombo" emptymessage="Search Name..." style="overflow: hidden; width: 96%; border: none!important;"
                                            visible="false">
                                                    </telerik:radcombobox>
                                        <telerik:radcombobox id="drpLoan" sort="Ascending" filter="Contains" runat="server"
                                            onselectedindexchanged="drpLoanOnSelectedIndexChanged" autopostback="true"
                                            allowcustomtext="false" rendermode="Lightweight" onclientfocus="OnClientKeyPressing"
                                            onclientblur="ValidateCombo" emptymessage="Search Name..." style="overflow: hidden; width: 96%; border: none!important;"
                                            visible="false">
                                                    </telerik:radcombobox>
                                        <telerik:radcombobox id="drpPartner" sort="Ascending" filter="Contains" runat="server"
                                            allowcustomtext="false" rendermode="Lightweight" onclientfocus="OnClientKeyPressing"
                                            autopostback="true" onselectedindexchanged="drpPartnerOnSelectedIndexChanged"
                                            onclientblur="ValidateCombo" emptymessage="Search Name..." style="overflow: hidden; width: 96%; border: none!important;"
                                            visible="false">
                                                    </telerik:radcombobox>
                                        <telerik:radcombobox id="drpDeposit" sort="Ascending" filter="Contains" runat="server"
                                            allowcustomtext="false" rendermode="Lightweight" onclientfocus="OnClientKeyPressing"
                                            onclientblur="ValidateCombo" emptymessage="Search Name..." style="overflow: hidden; width: 96%; border: none!important;"
                                            visible="false">
                                                    </telerik:radcombobox>
                                        <telerik:radcombobox id="drpAgent" sort="Ascending" filter="Contains" runat="server"
                                            allowcustomtext="false" rendermode="Lightweight" onclientfocus="OnClientKeyPressing"
                                            onclientblur="ValidateCombo" emptymessage="Search Name..." autopostback="true"
                                            style="overflow: hidden; width: 96%; border: none!important;"
                                            onselectedindexchanged="drpAgent_SelectedIndexChanged" visible="false">
                                                    </telerik:radcombobox>
                                        <telerik:radcombobox id="drpSupplier" sort="Ascending" filter="Contains" runat="server"
                                            allowcustomtext="false" rendermode="Lightweight" onclientfocus="OnClientKeyPressing"
                                            onclientblur="ValidateCombo" emptymessage="Search Name..."
                                            style="overflow: hidden; width: 96%; border: none!important;" visible="false">
                                                    </telerik:radcombobox>
                                        <asp:RequiredFieldValidator ID="rqSource" runat="server" ControlToValidate="drpCustomer"
                                            ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                            InitialValue=""></asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>

                        <tr>
                            <td>Expense Type/نوع النفقات <span style="color: Red">&nbsp*</span>
                                <asp:UpdatePanel ID="UpdExpenseDrop_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <telerik:radcombobox id="drpExpenseType" sort="Ascending" filter="Contains" runat="server"
                                            allowcustomtext="false" rendermode="Lightweight" onclientfocus="OnClientKeyPressing"
                                            autopostback="true" onselectedindexchanged="drpExpenseTypeOnSelectedIndexChanged"
                                            onclientblur="ValidateCombo" emptymessage="Search Expense Type..." style="overflow: hidden; width: 96%; border: none!important;">
                                            </telerik:radcombobox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drpExpenseType"
                                    ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updempSubtype" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblEmpSubType" Visible="false" runat="server" class="lbl" Text="Sub Type *"></asp:Label>
                                        <telerik:radcombobox id="drpEmpSubType" sort="Ascending" filter="Contains" runat="server"
                                            allowcustomtext="false" rendermode="Lightweight" onclientfocus="OnClientKeyPressing"
                                            autopostback="true" onselectedindexchanged="drpEmployeeOnSelectedIndexChanged"
                                            onclientblur="ValidateCombo" emptymessage="Search Type..." style="overflow: hidden; width: 96%; border: none!important;"
                                            visible="false">
                                                        <Items>
                                                            <telerik:RadComboBoxItem Value="1" Text="Incentive Payment" />
                                                            <telerik:RadComboBoxItem Value="2" Text="Salary" />
                                                        </Items>
                                                    </telerik:radcombobox>
                                        <asp:RequiredFieldValidator ID="rqdEmpSubtype" runat="server" ControlToValidate="drpEmpSubType"
                                            ValidationGroup="save" Display="Dynamic" Enabled="false" ErrorMessage="Required"
                                            Style="color: Red" InitialValue=""></asp:RequiredFieldValidator>
                                        <div runat="server" id="divDepreciatn">
                                            Depreciation Period
                                                     <telerik:radcombobox id="drpDepreciationPeriod" sort="Ascending" filter="Contains" runat="server"
                                                         allowcustomtext="true" rendermode="Lightweight" onclientfocus="OnClientKeyPressing"
                                                         onclientblur="ValidateCombo" emptymessage="Search ..." style="overflow: hidden; width: 96%; border: none!important;">
                                                    </telerik:radcombobox>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                  <asp:UpdatePanel ID="updBillNo" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlBillNo" runat="server">
                                            Invoice no
                                            <asp:TextBox ID="txtBillNo" class="txt" runat="server"></asp:TextBox>
                                        </asp:Panel>
                                        </ContentTemplate>
                                      </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>From Type/من النوع <span style="color: Red">&nbsp*</span>
                                <telerik:radcombobox id="drpFromType" sort="Ascending" filter="Contains" runat="server"
                                    onselectedindexchanged="drpFromTypeOnSelectedIndexChanged" allowcustomtext="false"
                                    rendermode="Lightweight" onclientfocus="OnClientKeyPressing" autopostback="true"
                                    onclientblur="ValidateCombo" emptymessage="Search Name/اسم..." style="overflow: hidden; width: 96%; border: none!important;">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="1" Text="Cash" />
                                                    <telerik:RadComboBoxItem Value="2" Text="Bank Transaction" />
                                                    <telerik:RadComboBoxItem Value="3" Text="Cheque" />
                                                     <telerik:RadComboBoxItem Value="5" Text="Loan" />
                                                </Items>
                                            </telerik:radcombobox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="drpFromType"
                                    ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdFrom" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblFromLabel" runat="server" class="lbl" Text="PettyCash"></asp:Label>
                                        <telerik:radcombobox id="drpPettyCashFrom" runat="server"
                                            onselectedindexchanged="drpPettyCashFromOnSelectedIndexChanged" allowcustomtext="false"
                                            rendermode="Lightweight" onclientfocus="OnClientKeyPressing" autopostback="true"
                                            onclientblur="ValidateCombo" emptymessage="Search Name/اسم..." style="overflow: hidden; width: 96%; border: none!important;">
                                                    </telerik:radcombobox>
                                        <telerik:radcombobox id="drpBankAccountFrom" runat="server"
                                            onselectedindexchanged="drpBankAccountFromOnSelectedIndexChanged" allowcustomtext="false"
                                            rendermode="Lightweight" onclientfocus="OnClientKeyPressing" autopostback="true"
                                            onclientblur="ValidateCombo" emptymessage="Search Name/اسم..." style="overflow: hidden; width: 96%; border: none!important;"
                                            visible="false">
                                                    </telerik:radcombobox>
                                        <telerik:radcombobox id="drpLoanFrom" runat="server" allowcustomtext="false" autopostback="true"
                                            rendermode="Lightweight" onclientfocus="OnClientKeyPressing" onselectedindexchanged="drpLoanFrom_SelectedIndexChanged"
                                            onclientblur="ValidateCombo" emptymessage="Search Name..." style="overflow: hidden; width: 96%; border: none!important;"
                                            visible="false">
                                                    </telerik:radcombobox>
                                        <asp:RequiredFieldValidator ID="rqFrom" runat="server" ControlToValidate="drpPettyCashFrom"
                                            ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                            InitialValue=""></asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>Amount/المبلغ <span style="color: Red">&nbsp*</span>
                                <asp:UpdatePanel ID="updAmountMain" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtAmountMain" class="numbers_only txt Payamt" runat="server"></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtAmountMain"
                                    ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                            <td>Bank Commission/عمولة <span style="color: Red">&nbsp*</span>
                                <asp:TextBox ID="txtCommission" class="numbers_only txt" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtCommission"
                                    ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>Tax Type/نوع الضريبة <span style="color: Red">&nbsp*</span>
                                <telerik:radcombobox id="drpTaxType" sort="Ascending" filter="Contains" runat="server"
                                    allowcustomtext="false" rendermode="Lightweight" onclientfocus="OnClientKeyPressing"
                                    onclientblur="ValidateCombo" emptymessage="Search Tax Type..." style="overflow: hidden; width: 96%; border: none!important;">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="1" Text="Percentage" />
                                                    <telerik:RadComboBoxItem Value="2" Text="Amount" />
                                                </Items>
                                            </telerik:radcombobox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="drpTaxType"
                                    ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                            <td>Tax/ضريبة <span style="color: Red">&nbsp*</span>
                                <asp:UpdatePanel ID="UpdTaxPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtTax" class="numbers_only txt" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtTax"
                                            ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                            InitialValue=""></asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdCheque" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblChequeDate" runat="server" class="lbl" Text="Cheque Date/ تحقق من التاريخ"
                                            Visible="false"></asp:Label>
                                        <telerik:raddatepicker id="dtChequeDate" runat="server" dateinput-dateformat="dd/MM/yyyy"
                                            visible="false">
                                                        <Calendar runat="server" ID="Calendar2" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                            ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                            <SpecialDays>
                                                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                </telerik:RadCalendarDay>
                                                            </SpecialDays>
                                                        </Calendar>
                                                    </telerik:raddatepicker>
                                        <asp:RequiredFieldValidator ID="rqChequeDate" runat="server" ControlToValidate="dtChequeDate"
                                            ValidationGroup="no" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                            InitialValue=""></asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="2">Transaction Details/تفاصيل الصفقه
                                            <asp:TextBox ID="txtTransaction" class="txt" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">Remarks/ملاحظات
                                            <asp:TextBox ID="txtRemarks" class="txtarea" TextMode="MultiLine" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <telerik:radasyncupload id="fu_Files" width="80%" maxfilesize="500000000"
                                    onfileuploaded="fu_FilesOnFileUploaded" runat="server">
                                            </telerik:radasyncupload>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="Updfu_Files" runat="server" ChildrenAsTriggers="false"
                                    UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="hdnfilenameup" runat="server" />
                                        <asp:HiddenField ID="hdnfilenamesaveup" runat="server" />
                                        <asp:LinkButton ID="lblfileupl" OnClick="lblfileupl_Click" runat="server"></asp:LinkButton>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="lblfileupl" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="updAccountDetails" runat="server" ChildrenAsTriggers="false"
                                    UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table class="listTable" runat="server" id="tblPay">
                                            <thead>
                                                <tr>
                                                    <th style="background-color: #513b71; border: 1px solid #dddddd; color: white; padding: 7px; text-align: left;">Payable/مدفوعة
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblPayable" ClientIDMode="Static" runat="server" class="lbl"></asp:Label>
                                                        <asp:HiddenField ID="hdnPayable" runat="server" ClientIDMode="Static" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <table class="listTable">
                                            <thead>
                                                <tr>
                                                    <th>Balance/توازن
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblBalance" runat="server" class="lbl"></asp:Label>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:UpdatePanel ID="updSaving" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnSave" class="butn_save" ValidationGroup="save" runat="server" Text="Save/حفظ" OnClick="Save"
                                            OnClientClick="if (Page_ClientValidate('save') == false) return(false);else return confirm('Do you really want to Save.. ?');" />
                                        <asp:Button ID="btnSavePrint" class="butn_save" ValidationGroup="save" runat="server" Text="Save & Print/حفظ وطباعة" OnClick="btnSavePrintOnClick"
                                            OnClientClick="if (Page_ClientValidate('save') == false) return(false);else return confirm('Do you really want to Save.. ?');" />
                                        <asp:Button ID="btnPrint" class="butn_save" ValidationGroup="save" runat="server"
                                            Text="Print/طباعة" OnClick="btnPrintOnClick" />
                                        <asp:Button ID="btnOpenCancel" class="butn_delete" runat="server" Text="Cancel/إلغاء" OnClientClick="javascript : return confirm('Do you really want to cancel.. ?');"
                                            OnClick="btnOpenCancel_OnClick" />
                                        <asp:Button ID="btnReset" class="butn" runat="server" OnClientClick="javascript : return confirm('Do you really want to reset.. ?');"
                                            Visible="true" Text="Reset/إعادة تعيين" OnClick="btnReset_OnClick" />
                                        <asp:Button ID="btnOpenDelete" class="butn_delete" runat="server" Text="Delete/حذف" OnClientClick="javascript : return confirm('Do you really want to delete.. ?');"
                                            OnClick="btnOpenDelete_OnClick" />
                                        <asp:Button ID="Button1" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_OnClick" />
                                        <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdn_update" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdn_delete" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdn_print" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdn_add_N_print" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdn_update_N_print" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdn_cancel" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdn_user_id" runat="server" />
                                         <asp:HiddenField ID="hdnPageId" runat="server"  />
                                            <asp:HiddenField ID="hdnfilter" runat="server"  />
                                            <asp:HiddenField ID="hdnCount" runat="server"  />

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    <div>
                        <div id="div1" class="messageAlert div_pop animated" style="display: none" runat="server">
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
        <asp:UpdatePanel ID="updAlertCommn" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlAlertCommn" Visible="false" runat="server">
                    <div class="popupBackground">
                    </div>
                    <div class="animated smallPopUpCentre" style="min-height: 100px; padding: 2%; overflow: hidden">
                        <asp:Label ID="lblAlertCommn" Font-Size="16px" runat="server"></asp:Label>
                        <div>
                            <br />
                            <asp:Button ID="Button22" Width="75px" Height="30px" runat="server" CssClass="butn" Text="Close" OnClick="btnAlertCloseOnClick" />
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdExpensePanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" style="height: 100%;">
            <ContentTemplate>
                <asp:Panel ID="pnlExpense" Visible="false" runat="server">
                    <AmarCentre:ExpenseMaster ID="UC_Expense" runat="server" />
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="updCancel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlCancel" runat="server" Visible="false">
            <div class="popupBackground">
            </div>
            <div class="animated smallPopUp">
                <div class="Adding_heading">
                    <asp:Label ID="lblCancel" runat="server" Text=""></asp:Label>
                </div>
                <table class="formTable">
                    <tr>
                        <td>Remark/تعليق <span style="color: Red">&nbsp*</span>
                            <asp:TextBox ID="txtCancelRemark" class="txtarea" TextMode="MultiLine" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtCancelRemark"
                                ValidationGroup="cancel" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                InitialValue=""></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnCancel" class="butn_save" ValidationGroup="cancel" OnClick="btnCancel_OnClick"
                                runat="server" Text="Cancel" />
                            <asp:Button ID="btnDelete" class="butn_save" ValidationGroup="cancel" OnClick="btnDelete_OnClick"
                                runat="server" Text="Delete/حذف" />
                            <asp:Button ID="btnCloseCancel" class="butn" runat="server" Text="Close/أغلق" OnClick="btnCloseCancel_OnClick" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
