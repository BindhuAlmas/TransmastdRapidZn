<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCRV.ascx.cs" Inherits="AmarCentre.Transactions.UserControl.UCRV" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Masters/UserControl/UCIncome.ascx" TagName="IncomeMaster" TagPrefix="AmarCentre" %>
<%@ Register Src="~/Transactions/UserControl/UCMail.ascx" TagName="MailUC" TagPrefix="AmarCentre" %>
<%@ Register Src="~/Transactions/UserControl/UCParty.ascx" TagName="PartyMaster" TagPrefix="AmarCentre" %>

  <asp:UpdatePanel ID="Upd_Add_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnl_add" runat="server">
                        <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="Adding_heading">
                                    Receipt Voucher/سند القبض
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td style="width:33%">
                                            Code/رمز
                                            <asp:Label ID="lblCode" Font-Bold="true" runat="server" class="lbl"></asp:Label>
                                        </td>
                                   
                                        <td style="width:33%">
                                            Date/تاريخ <span style="color: Red">&nbsp*</span>
                                            <telerik:RadDatePicker ID="dtdated" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                <Calendar runat="server" ID="Calendar1" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                    ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                    <SpecialDays>
                                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                        </telerik:RadCalendarDay>
                                                    </SpecialDays>
                                                </Calendar>
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="dtdated"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width:33%" rowspan="5">
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
                                            <asp:UpdatePanel ID="updinvoicebtn" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div style="top: 30%; position: fixed">
                                                        <asp:Button ID="btnInvDetails" OnClick="btnInvDetailsOnClick" class="butn_delete" runat="server"
                                                            Text="Invoice Details/بيانات الفاتورة " Visible="false" />

                                                        <asp:Button ID="btnCompanyInvDetails" OnClick="btnInvDetailsOnClickCG" class="butn_delete" runat="server"
                                                            Text="Invoice Details/بيانات الفاتورة " Visible="false" />
                                                    </div>

                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            From Type/من النوع <span style="color: Red">&nbsp*</span>
                                            <telerik:RadComboBox ID="drpFrom" Sort="Ascending" Filter="Contains" runat="server"
                                                OnSelectedIndexChanged="drpFromOnSelectedIndexChanged" AllowCustomText="false"
                                                RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing" AutoPostBack="true"
                                                OnClientBlur="ValidateCombo" EmptyMessage="Search Type..." Style="overflow: hidden;
                                                width: 96%; border: none!important;">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="1" Text="Customer" />
                                                     <telerik:RadComboBoxItem Value="11" Text="Company Group" />
                                                    <telerik:RadComboBoxItem Value="2" Text="Vendor" />
                                                    <telerik:RadComboBoxItem Value="3" Text="Employee" />
                                                    <telerik:RadComboBoxItem Value="5" Text="Loan" />
                                                    <telerik:RadComboBoxItem Value="6" Text="Customer Invoice" />
                                                    <telerik:RadComboBoxItem Value="7" Text="Deposit Return" />
                                                    <telerik:RadComboBoxItem Value="8" Text="Vendor Deposit Return" />
                                                    <telerik:RadComboBoxItem Value="9" Text="Vendor Commission" />
                                                     <telerik:RadComboBoxItem Value="10" Text="Asset Sale" />
                                                    <telerik:RadComboBoxItem Value="4" Text="General Income" />
                                                </Items>
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="drpFrom"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdFrom" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblFromLabel" runat="server" class="lbl" Text="Customer Name/اسم *"></asp:Label>
                                                    <telerik:RadComboBox ID="drpCustomer" Sort="Ascending" Filter="Contains" runat="server"
                                                        OnSelectedIndexChanged="drpCustomerOnSelectedIndexChanged" AllowCustomText="false"
                                                        RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing" AutoPostBack="true"
                                                        OnClientBlur="ValidateCombo" EmptyMessage="Search Name/اسم..." Style="overflow: hidden;
                                                        width: 96%; border: none!important;" Visible="true">
                                                    </telerik:RadComboBox>
                                                    <telerik:RadComboBox ID="drpCompanyGroup" Sort="Ascending" Filter="Contains" runat="server"
                                                        OnSelectedIndexChanged="drpCompanyGroup_SelectedIndexChanged" AllowCustomText="false"
                                                        RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing" AutoPostBack="true"
                                                        OnClientBlur="ValidateCombo" EmptyMessage="Search Name/اسم..." Style="overflow: hidden; width: 96%; border: none!important;"
                                                        Visible="true">
                                                    </telerik:RadComboBox>
                                                    <telerik:RadComboBox ID="drpVendor" Sort="Ascending" Filter="Contains" runat="server"
                                                        OnSelectedIndexChanged="drpVendorOnSelectedIndexChanged" AllowCustomText="false"
                                                        RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing" AutoPostBack="true"
                                                        OnClientBlur="ValidateCombo" EmptyMessage="Search Vendor..." Style="overflow: hidden;
                                                        width: 96%; border: none!important;" Visible="false">
                                                    </telerik:RadComboBox>
                                                    <telerik:RadComboBox ID="drpEmployee" Sort="Ascending" Filter="Contains" runat="server"
                                                        OnSelectedIndexChanged="drpEmployeeOnSelectedIndexChanged" AllowCustomText="false"
                                                        RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing" AutoPostBack="true"
                                                        OnClientBlur="ValidateCombo" EmptyMessage="Search Employee..." Style="overflow: hidden;
                                                        width: 96%; border: none!important;" Visible="false">
                                                    </telerik:RadComboBox>
                                                    <telerik:RadComboBox ID="drpLoan" Sort="Ascending" Filter="Contains" runat="server"
                                                        OnSelectedIndexChanged="drpLoanOnSelectedIndexChanged" AllowCustomText="false"
                                                        RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing" AutoPostBack="true"
                                                        OnClientBlur="ValidateCombo" EmptyMessage="Search Loan..." Style="overflow: hidden;
                                                        width: 96%; border: none!important;" Visible="false">
                                                    </telerik:RadComboBox>
                                                    <telerik:RadComboBox ID="drpDeposit" Sort="Ascending" Filter="Contains" runat="server"
                                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                          OnClientBlur="ValidateCombo" EmptyMessage="Search Name..." Style="overflow: hidden;
                                                        width: 96%; border: none!important;" Visible="false">
                                                    </telerik:RadComboBox>
                                                      <telerik:RadComboBox ID="drpParty" Sort="Ascending" Filter="Contains" runat="server"
                                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                          AutoPostBack="true" OnSelectedIndexChanged="drpParty_SelectedIndexChanged"
                                                          OnClientBlur="ValidateCombo" EmptyMessage="Search Name..." Style="overflow: hidden;
                                                        width: 96%; border: none!important;" Visible="false">
                                                    </telerik:RadComboBox>
                                                    <asp:RequiredFieldValidator ID="rqSource" runat="server" ControlToValidate="drpCustomer"
                                                        ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="updasset" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlasset1" runat="server">
                                                        Asset
                                                        <telerik:RadComboBox ID="drpAsset" Sort="Ascending" Filter="Contains" runat="server"
                                                            OnSelectedIndexChanged="drpAsset_SelectedIndexChanged" AllowCustomText="false"
                                                            RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing" AutoPostBack="true"
                                                            OnClientBlur="ValidateCombo" EmptyMessage="Search Name/اسم..." Style="overflow: hidden; width: 96%; border: none!important;"
                                                            Visible="true">
                                                        </telerik:RadComboBox>
                                                        <asp:RequiredFieldValidator ID="drpAssetrqd" runat="server" ControlToValidate="drpAsset"
                                                            ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                            InitialValue=""></asp:RequiredFieldValidator>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="updasset2" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlasset2" runat="server">
                                                        <br />
                                                        <asp:Label ID="lblassetvalue" runat="server"></asp:Label>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Income Type/نوع الدخل <span style="color: Red">&nbsp*</span>
                                             <asp:UpdatePanel ID="UpdIncomeDrop_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                            <telerik:RadComboBox ID="drpIncomeType" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                OnSelectedIndexChanged="drpIncomeType_SelectedIndexChanged"
                                                AutoPostBack="true" OnClientBlur="ValidateCombo" EmptyMessage="Search Income Type..."
                                                Style="overflow: hidden; width: 96%; border: none!important;">
                                            </telerik:RadComboBox>
                                                     </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drpIncomeType"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="updCPaytype" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div runat="server" id="divCustomerPaymentType" visible="false">
                                                        Payment Type <span style="color: Red">&nbsp*</span>
                                                        <telerik:RadComboBox ID="drpCustomerPaymentType" Sort="Ascending" Filter="Contains" runat="server"
                                                            AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                            OnSelectedIndexChanged="drpCustomerPaymentTypeOnSelectedIndexChanged"
                                                            AutoPostBack="true" OnClientBlur="ValidateCombo" EmptyMessage="Search  Type..."
                                                            Style="overflow: hidden; width: 96%; border: none!important;">
                                                            <Items>
                                                                <telerik:RadComboBoxItem Value="1" Text="Invoice Payment" />
                                                                <telerik:RadComboBoxItem Value="2" Text="Advance Payment" />
                                                            </Items>
                                                        </telerik:RadComboBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="drpCustomerPaymentType"
                                                            ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                            InitialValue=""></asp:RequiredFieldValidator>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td>
                                            Amount/المبلغ <span style="color: Red">&nbsp*</span>
                                            <asp:UpdatePanel ID="updAmountMain" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtAmountMain" ClientIDMode="Static" class="numbers_only txt mainamount" runat="server"></asp:TextBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtAmountMain"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        <td>Tax Percentage
                                          <asp:TextBox ID="txttax" class="numbers_only txt txttax" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            To Type/لكتابة <span style="color: Red">&nbsp*</span>
                                            <asp:UpdatePanel ID="UpdToType" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <telerik:RadComboBox ID="drpToType"  runat="server" 
                                                        CssClass="totype" OnSelectedIndexChanged="drpToTypeOnSelectedIndexChanged" AllowCustomText="false"
                                                        RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing" AutoPostBack="true"
                                                        OnClientBlur="ValidateCombo" EmptyMessage="Search Name..." Style="overflow: hidden;
                                                        width: 96%; border: none!important;">
                                                        <Items>
                                                            <telerik:RadComboBoxItem Value="1" Text="Cash" />
                                                            <telerik:RadComboBoxItem Value="2" Text="Bank Transaction" />
                                                            <telerik:RadComboBoxItem Value="3" Text="Cheque" />
                                                            <telerik:RadComboBoxItem Value="5" Text="Loan" />
                                                            <telerik:RadComboBoxItem Value="6" Text="Card Swipe" />
                                                             <telerik:RadComboBoxItem Value="10" Text="Nomad" />
                                                        </Items>
                                                    </telerik:RadComboBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="drpToType"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                   
                                        <td>
                                            <asp:UpdatePanel ID="UpdTo" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblToLabel" runat="server" class="lbl" Text="PettyCash"></asp:Label>
                                                    <telerik:RadComboBox ID="drpPettyCash" runat="server"
                                                        AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                        AutoPostBack="true" OnClientBlur="ValidateCombo" EmptyMessage="Search Name..."
                                                        Style="overflow: hidden; width: 96%; border: none!important;">
                                                    </telerik:RadComboBox>
                                                    <telerik:RadComboBox ID="drpBankAccount" runat="server"
                                                        AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                        AutoPostBack="true" OnSelectedIndexChanged="onchangedrp_bank" OnClientBlur="ValidateCombo"
                                                        EmptyMessage="Search Name..." Style="overflow: hidden; width: 96%; border: none!important;"
                                                        Visible="false">
                                                    </telerik:RadComboBox>
                                                      <telerik:RadComboBox ID="drpLoanAccount" runat="server"
                                                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                                       OnClientBlur="ValidateCombo"
                                                                        EmptyMessage="Search Name..." Style="overflow: hidden; width: 85%; border: none!important;"
                                                                        Visible="false">
                                                                    </telerik:RadComboBox>
                                                    <asp:RequiredFieldValidator ID="rqTo" runat="server" ControlToValidate="drpPettyCash"
                                                        ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                    <asp:HiddenField ID="hdn_bankcommsn" ClientIDMode="Static" runat="server" />
                                                                        <asp:HiddenField ID="hdnisCommissionVat" ClientIDMode="Static" runat="server" />
                                               
                                                    </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Bank Commission/عمولة البنك
                                            <asp:UpdatePanel ID="upd_commsn" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:TextBox class="txt comssnAmt numbers_only" ID="txt_commsn" runat="server"></asp:TextBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            </td>
                                        <td>
                                            <asp:UpdatePanel ID="updCommissionVat" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>

                                                    <asp:Panel ID="pnlCommissionVat" Visible="false" runat="server">
                                                        Vat on Commission
                                                    <asp:TextBox class="txt numbers_only txtCommissionVat" ID="txtCommissionVat" runat="server"></asp:TextBox>
                                                    </asp:Panel>

                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:UpdatePanel ID="UpdCheque" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblChequeDate" runat="server" class="lbl" Text="Cheque Date/ تحقق من التاريخ" Visible="false"></asp:Label>
                                                    <telerik:RadDatePicker ID="dtChequeDate" runat="server" DateInput-DateFormat="dd/MM/yyyy"
                                                        Visible="false">
                                                        <Calendar runat="server" ID="Calendar2" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                            ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                            <SpecialDays>
                                                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                </telerik:RadCalendarDay>
                                                            </SpecialDays>
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                                    <asp:RequiredFieldValidator ID="rqChequeDate" runat="server" ControlToValidate="dtChequeDate"
                                                        ValidationGroup="no" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                     <asp:Label ID="lblChargedAmt" runat="server" Text="Charged Amount" Visible="false"></asp:Label>
                                                     <asp:TextBox class="txt numbers_only" ID="txtChargedAmt" runat="server" Visible="false"></asp:TextBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            Transaction Details/تفاصيل الصفقه
                                            <asp:TextBox ID="txtTransaction" class="txt" runat="server"></asp:TextBox>
                                        </td>
                                        </tr>
                                        <tr>
                                        <td colspan="2">
                                            Remarks/ملاحظات
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
                                                    <table class="listTable">
                                                        <thead>
                                                            <tr>
                                                                <th>
                                                                    Receivable/ذمم مدينة 
                                                                </th>
                                                                <th>
                                                                    Payable/مدفوعة 
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblReceivable" runat="server" class="lbl"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblPayable" runat="server" class="lbl advance"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </tbody>
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
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:UpdatePanel ID="updSaving" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblenablemsg" runat="server" ForeColor="Red"></asp:Label>
                                                    <br />
                                                    <asp:Button ID="btnSave" class="butn_save" ValidationGroup="save" runat="server"
                                                         OnClientClick="if (Page_ClientValidate('save') == false) return(false);else return confirm('Do you really want to Save.. ?');"
                                                        Text="Save/حفظ" OnClick="saveReceiptVoucher" />
                                                    <asp:Button ID="btnSavePrint" class="butn_save" ValidationGroup="save" runat="server"
                                                         OnClientClick="if (Page_ClientValidate('save') == false) return(false);else return confirm('Do you really want to Save.. ?');"
                                                        Text="Save & Print/حفظ وطباعة" OnClick="saveprintReceiptVoucher" />
                                                    <asp:Button ID="btnPrint" class="butn_save" ValidationGroup="save" runat="server"
                                                        Text="Print/طباعة" OnClick="btnPrint_OnClick" />
                                                    <asp:Button ID="btnOpenCancel" class="butn_delete" runat="server" Text="Cancel/إلغاء" OnClientClick="javascript : return confirm('Do you really want to cancel.. ?');"
                                                        OnClick="btnOpenCancel_OnClick" />
                                                  
                                                    <asp:Button ID="btnReset" class="butn_save" runat="server" OnClientClick="javascript : return confirm('Do you really want to reset.. ?');"
                                                        Visible="true" Text="Reset/إعادة تعيين" OnClick="btnReset_OnClick" />
                                                    <asp:Button ID="btnOpenDelete" class="butn_delete" runat="server" Text="Delete/حذف" OnClientClick="javascript : return confirm('Do you really want to delete.. ?');"
                                                        OnClick="btnOpenDelete_OnClick" />
                                                    
                                                    <asp:Button ID="btnClose" class="butn" runat="server" Text="Close/أغلق" OnClick="btnClose_OnClick" />
                                                    <asp:HiddenField ID="hdnCustomerPaymentType" Value="0" runat="server" />
                                                    <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdn_update" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdn_delete" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdn_print" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdn_add_N_print" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdn_update_N_print" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdn_cancel" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdn_id" runat="server" Value="0" ClientIDMode="Static" />
                                                    <asp:HiddenField ID="hdn_user_id" runat="server" />
                                            <asp:HiddenField ID="hdnsendmail" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdnPageId" runat="server"  />
                                            <asp:HiddenField ID="hdnfilter" runat="server"  />
<asp:HiddenField ID="hdnCount" runat="server"  />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
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
               <asp:UpdatePanel ID="UpdIncomePanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" >
                            <ContentTemplate>
                                <asp:Panel ID="pnlIncome" Visible="false" runat="server">
                                    <AmarCentre:IncomeMaster ID="UC_Income" runat="server" />
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                  <asp:UpdatePanel ID="updPartyPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" >
                            <ContentTemplate>
                                <asp:Panel ID="PartyPanel" Visible="false" runat="server">
                                    <AmarCentre:PartyMaster ID="UC_Party" runat="server" />
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                 <asp:UpdatePanel ID="UpdMailPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlMail" Visible="false" runat="server">
                <AmarCentre:MailUC ID="EmailUC" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel> 
                
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
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updInvoiceList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlInvoice" Visible="false" runat="server">
                    <div class="popupBackground">
                    </div>
                    <div class="animated smallPopUp" style="width: 55%">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div style="padding-left: 10px">
                                    <asp:TextBox ID="txtAmtAuto" runat="server" class="numbers_only txt" Width="35%"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtAmtAuto"
                                        ValidationGroup="autosave" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                        InitialValue=""></asp:RequiredFieldValidator>
                                    <asp:Button ID="Button3" class="butn_save" ValidationGroup="autosave" runat="server"
                                        Text="AutoAllocate" OnClick="btnAllocOnClick" />
                                </div>
                                <div class="Adding_heading">
                                    Outstanding invoice/فاتورة المعلقة
                                </div>
                                <table class="listTable">
                                    <thead>
                                        <tr>
                                            <th   style="width: 5%;">
                                                Select/اختار
                                            </th>
                                            <th style="width: 12%;">
                                                Invoice/فاتورة
                                            </th>
                                            <th style="width: 12%;">
                                                Date/تاريخ
                                            </th>
                                             <th style="width: 12%;">
                                               Subject
                                            </th>
                                            <th style="width: 20%;">
                                                Invoice Amount/المبلغ الفاتورة
                                            </th>
                                            <th style="width: 20%;">
                                                Receivable Amount/المبلغ المستحق
                                            </th>
                                            <th style="width: 20%;">
                                                Received Amount/المبلغ المستلم
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rpt_invoiceList" runat="server" OnItemDataBound="rpt_invoiceList_OnItemDataBound">
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:HiddenField ID="hdnInvId" runat="server" Value='<%#Eval("Id")%>' />
                                                        <asp:HiddenField ID="hdnInvStatus" runat="server" Value='<%#Eval("InvoiceStatus")%>' />
                                                        <asp:CheckBox ID="chkSelect" runat="server" Checked='<%#Convert.ToBoolean(Eval("CheckBoxValue"))%>'
                                                            AutoPostBack="true" OnCheckedChanged="chkSelectOnCheckedChanged" />
                                                    </td>
                                                    <td>
                                                        <%#Eval("Code")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("Dated")%>
                                                    </td>
                                                     <td>
                                                        <%#Eval("subject")%>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblInvoiceAmount" runat="server" Text='<%#Eval("InvoiceAmount")%>' class=""></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtReceivableamt" runat="server" Text='<%#Eval("Receivable")%>'
                                                            class="txt read_Only receivableamt"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="updAmount" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtAmount" runat="server" Text='<%#Eval("PaymentAmount")%>' class="txt numbers_only paidAmt"
                                                                    Enabled="false"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr>
                                           
                                            <td colspan="5">
                                                <asp:UpdatePanel ID="updisinvoice" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:HiddenField ID="hdnIsinvoice" runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                Total/مجموع
                                            </td>
                                          
                                            <td>
                                                <asp:Label ID="lblTReceivableAmount" runat="server" Text="" class=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="updTotalInvoiceAmount" runat="server" ChildrenAsTriggers="false"
                                                    UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtTotal" runat="server" Text="0" class="txt total read_Only"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" style="text-align: right">
                                                Outstanding Amount/المبلغ رهيبة

                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="updOutstandingAmount" runat="server" ChildrenAsTriggers="false"
                                                    UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtOutstandingAmount" runat="server" Text="0" class="txt OutstandingAmount read_Only"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="7" >
                                                <asp:Button ID="btnProceed" class="butn_save" runat="server" Text="Proceed" OnClick="btnProceedOnClick" />
                                              <asp:Button ID="btnAdvanceProceed" class="butn_save" runat="server" Text="Close"
                                                    OnClick="btnAdvanceProceedOnClick" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

<asp:UpdatePanel ID="updInvoiceListCG" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
      <ContentTemplate>
          <asp:Panel ID="pnlInvoiceCG" Visible="false" runat="server">
              <div class="popupBackground">
              </div>
              <div class="animated smallPopUp" style="width: 60%">
                  <asp:UpdatePanel ID="UpdatePanel2CG" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                      <ContentTemplate>
                          <div class="Adding_heading">
                              Outstanding invoice/فاتورة المعلقة
                          </div>
                           <div style="padding-left: 10px">
                              <asp:TextBox ID="txtAmtAutoCG" runat="server" class="numbers_only txt" Width="35%"></asp:TextBox>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator5CG" runat="server" ControlToValidate="txtAmtAutoCG"
                                  ValidationGroup="autosaveCG" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                  InitialValue=""></asp:RequiredFieldValidator>
                              <asp:Button ID="Button3CG" class="butn_save" ValidationGroup="autosaveCG" runat="server"
                                  Text="Auto Allocate" OnClick="btnAllocOnClickCG" />
                          </div>
                          <table class="listTable">
                              <thead>
                                  <tr>
                                      <th >
                                        
                                      </th>
                                       <th style="width: 20%;">
                                        Company
                                      </th>
                                      <th style="width: 10%;">
                                          Invoice/فاتورة
                                      </th>
                                      <th style="width: 10%;">
                                          Date/تاريخ
                                      </th>
                                      <th style="width: 20%;">
                                          Invoice Amount/المبلغ الفاتورة
                                      </th>
                                      <th style="width: 20%;">
                                          Receivable Amount/المبلغ المستحق
                                      </th>
                                      <th style="width: 35%;">
                                          Received Amount/المبلغ المستلم
                                      </th>
                                  </tr>
                              </thead>
                              <tbody>
                                  <asp:Repeater ID="rpt_invoiceListCG" runat="server" >
                                      <ItemTemplate>
                                          <tr>
                                              <td>
                                                  <asp:HiddenField ID="hdnInvIdCG" runat="server" Value='<%#Eval("Id")%>' />
                                                  <asp:HiddenField ID="hdnCustomerIdCG" runat="server" Value='<%#Eval("CustomerId")%>' />
                                                  <asp:HiddenField ID="hdnInvStatusCG" runat="server" Value='<%#Eval("InvoiceStatus")%>' />
                                                  <asp:CheckBox ID="chkSelectCG" runat="server" Checked='<%#Convert.ToBoolean(Eval("CheckBoxValue"))%>'
                                                      AutoPostBack="true" OnCheckedChanged="chkSelectOnCheckedChangedCG" />
                                              </td>
                                              <td>
                                                  <%#Eval("Name")%>
                                              </td>
                                              <td>
                                                  <%#Eval("Code")%>
                                              </td>
                                              <td>
                                                  <%#Eval("Dated")%>
                                              </td>
                                              <td>
                                                  <asp:Label ID="lblInvoiceAmountCG" runat="server" Text='<%#Eval("InvoiceAmount")%>' class=""></asp:Label>
                                              </td>
                                              <td>
                                                  <asp:TextBox ID="txtReceivableamtCG" runat="server" Text='<%#Eval("Receivable")%>'
                                                      class="txt read_Only receivableamtCG"></asp:TextBox>
                                              </td>
                                              <td>
                                                  <asp:UpdatePanel ID="updAmountCG" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                      <ContentTemplate>
                                                          <asp:TextBox ID="txtAmountCG" runat="server" Text='<%#Eval("PaymentAmount")%>' class="txt numbers_only paidAmtCG"
                                                              Enabled='<%#Convert.ToBoolean(Eval("CheckBoxValue"))%>'></asp:TextBox>
                                                      </ContentTemplate>
                                                  </asp:UpdatePanel>
                                              </td>
                                          </tr>
                                      </ItemTemplate>
                                  </asp:Repeater>
                                  <tr>
                                      
                                      <td colspan="5">
                                        
                                          Total/مجموع
                                      </td>
                                      <td>
                                          <asp:Label ID="lblTReceivableAmountCG" runat="server" Text="" class=""></asp:Label>
                                      </td>
                                      <td>
                                          <asp:UpdatePanel ID="updTotalInvoiceAmountCG" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                              <ContentTemplate>
                                                  <asp:TextBox ID="txtTotalCG" runat="server" Text="0" class="txt totalCG read_Only"></asp:TextBox>
                                              </ContentTemplate>
                                          </asp:UpdatePanel>
                                      </td>
                                  </tr>
                                  <tr>
                                      <td colspan="7" >
                                          <asp:Button ID="btnProceedCG" class="butn_save" runat="server" Text="Proceed" OnClick="btnProceedOnClickCG" />
                                           <asp:Button ID="btnCloseCG" class="butn_save" runat="server" Text="Close"
                                              OnClick="btnCloseClickCG" />
                                      </td>
                                  </tr>
                              </tbody>
                          </table>
                      </ContentTemplate>
                  </asp:UpdatePanel>
              </div>
          </asp:Panel>
      </ContentTemplate>
  </asp:UpdatePanel>
        <asp:UpdatePanel ID="updCustomerInvoiceList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlCustmrInvoice" Visible="false" runat="server">
                    <div class="popupBackground">
                    </div>
                    <div class="animated smallPopUp" style="width: 50%">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>

                             <div style="padding-left: 10px">
                                    <asp:TextBox ID="txtAmtAutoCI" runat="server" class="numbers_only txt" Width="35%"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtAmtAutoCI"
                                        ValidationGroup="autosaveCI" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                        InitialValue=""></asp:RequiredFieldValidator>
                                    <asp:Button ID="Button4" class="butn_save" ValidationGroup="autosaveCI" runat="server"
                                        Text="AutoAllocate" OnClick="btnAllocCIOnClick" />
                                </div>

                                <div class="Adding_heading">
                                    Outstanding invoice/فاتورة المعلقة
                                </div>
                                <table class="listTable">
                                    <thead>
                                        <tr>
                                            <th class="listTableSlNo" style="width: 5%;">
                                                Select/اختار
                                            </th>
                                            <th style="width: 20%;">
                                                Invoice/فاتورة
                                            </th>
                                            <th style="width: 20%;">
                                                Date/تاريخ
                                            </th>
                                            <th style="width: 20%;">
                                                Invoice Amount/المبلغ الفاتورة
                                            </th>
                                            <th style="width: 20%;">
                                                Receivable Amount/المبلغ المستحق
                                            </th>
                                            <th style="width: 35%;">
                                                Received Amount/المبلغ المستلم
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptCustomerInvoice" runat="server" OnItemDataBound="rpt_CIinvoiceList_OnItemDataBound">
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:HiddenField ID="hdnInvId" runat="server" Value='<%#Eval("Id")%>' />
                                                        <asp:HiddenField ID="hdnInvStatus" runat="server" Value='<%#Eval("InvoiceStatus")%>' />
                                                        <asp:CheckBox ID="chkSelect" runat="server" Checked='<%#Convert.ToBoolean(Eval("CheckBoxValue"))%>'
                                                            AutoPostBack="true" OnCheckedChanged="CIchkSelectOnCheckedChanged" />
                                                    </td>
                                                    <td>
                                                        <%#Eval("Code")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("Dated")%>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblInvoiceAmount" runat="server" Text='<%#Eval("InvoiceAmount")%>' class=""></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtReceivableamt" runat="server" Text='<%#Eval("Receivable")%>'
                                                            class="txt read_Only receivableamt"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="updAmount" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtAmountCI" runat="server" Text='<%#Eval("PaymentAmount")%>' class="txt numbers_only paidAmt"
                                                                    Enabled="false"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td colspan="2">
                                                Total/مجموع
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTReceivableAmount_CI" runat="server" Text="" class=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="updCITotal" runat="server" ChildrenAsTriggers="false"
                                                    UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtCITotal" runat="server" Text="0" class="txt total read_Only"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5" style="text-align: right">
                                                Outstanding Amount/المبلغ رهيبة

                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="updCIOutStndng" runat="server" ChildrenAsTriggers="false"
                                                    UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtOutstandingAmount_CI" runat="server" Text="0" class="txt OutstandingAmount read_Only"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" style="text-align: center">
                                                <asp:Button ID="Button1" class="butn_save" runat="server" Text="Proceed" OnClick="btnCIProceedOnClick" />
                                           <asp:Button ID="Button2" class="butn_save" runat="server" Text="Close"
                                                    OnClick="btnAdvanceProceedOnClick" />
                                                </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </asp:Panel>
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
                                <td>
                                    Remark/تعليق <span style="color: Red">&nbsp*</span>
                                    <asp:TextBox ID="txtCancelRemark" class="txtarea" TextMode="MultiLine" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtCancelRemark"
                                        ValidationGroup="cancel" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                        InitialValue=""></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnCancel" class="butn_save" ValidationGroup="cancel" OnClick="btnCancel_OnClick"
                                        runat="server" Text="Cancel/إلغاء" />
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