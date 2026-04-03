<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true"
    CodeBehind="PaymentVoucher.aspx.cs" Inherits="AmarCentre.Transactions.PaymentVoucher" %>
<%@ Register Src="~/Masters/UserControl/UCExpense.ascx" TagName="ExpenseMaster"
    TagPrefix="AmarCentre" %>
<%@ Register Src="~/Masters/UserControl/UCSupplier.ascx" TagName="SupplierMaster"
    TagPrefix="AmarCentre" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
      <script type="text/javascript">
            function stopRKey(evt) {
                var evt = (evt) ? evt : ((event) ? event : null);
                var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
                if ((evt.keyCode == 13) && (node.type == "text")) { return false; }
            }
            document.onkeypress = stopRKey;
        </script>
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

             /*final settlement*/
             {
                 $('.OfcSettlesupchkitem').click(function () {
                     Sup_CalculateOFS();
                 });

                 $('.OfcSettlePayAmt').blur(function (e) {
                     Sup_CalculateOFS();
                 });

                 function Sup_CalculateOFS() {
                     var count = 0;
                     var total = 0;
                     var chk = 0;

                     $(".OfcSettlePayAmt").each(function () {
                         var OSAmt = 0;
                         OSAmt = $(this).closest('tr').find('.OfcSettleBalAmt').val();
                         if ($(this).closest('tr').find(':checkbox').prop('checked')) {
                             if ($(this).val() != '') {
                                 if (parseFloat($(this).val()) <= parseFloat(OSAmt)) {
                                     total = (parseFloat($(this).val()) + parseFloat(total)).toFixed(2);
                                     chk = 1;
                                 }
                                 else {
                                     alert('Amount cannot be greater than Balance');
                                     $(this).val('');
                                 }
                             }
                         }
                     });

                     $(".OfcSettleTotAmt").val(total);
                     if (chk == "1") {
                         $(".Payamt").val(total);
                         $(".Payamt").attr('readonly', true);
                     }
                     else {
                         $(".Payamt").val('');
                         $(".Payamt").attr('readonly', false);
                     }
                 }
             }

             /*Maintenance*/
             {
                 $('.Maintncesupchkitem').click(function () {
                     Sup_CalculateMaintnce();
                 });

                 $('.MaintncePayAmt').blur(function (e) {
                     Sup_CalculateMaintnce();
                 });

                 function Sup_CalculateMaintnce() {
                     var count = 0;
                     var total = 0;
                     var chk = 0;

                     $(".MaintncePayAmt").each(function () {
                         var OSAmt = 0;
                         OSAmt = $(this).closest('tr').find('.MaintnceBalAmt').val();
                         if ($(this).closest('tr').find(':checkbox').prop('checked')) {
                             if ($(this).val() != '') {
                                 if (parseFloat($(this).val()) <= parseFloat(OSAmt)) {
                                     total = (parseFloat($(this).val()) + parseFloat(total)).toFixed(2);
                                     chk = 1;
                                 }
                                 else {
                                     alert('Amount cannot be greater than Balance');
                                     $(this).val('');
                                 }
                             }
                         }
                     });

                     $(".MaintnceTotAmt").val(total);
                     if (chk == "1") {
                         $(".Payamt").val(total);
                         $(".Payamt").attr('readonly', true);
                     }
                     else {
                         $(".Payamt").val('');
                         $(".Payamt").attr('readonly', false);
                     }
                 }
             }
         }
            </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Payment Voucher
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
                            <th  style="width: 5%;">
                                Sl No   

                            </th>
                            <th style="width: 8%;">
                                Code 

                            </th>
                            <th style="width: 20%;">
                                Name
                            </th>
                            <th style="width: 13%;">
                                Expense Type
                            </th>
                            <th style="width: 8%;">
                                Date
                            </th>
                            <th style="width: 9%;">
                                Amount
                            </th>
                            <th style="width: 8%;">
                                Status
                            </th>
                            <th style="width: 7%;">
                                Action
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
                                    <td>
                                        <%#Eval("ExpenseTypeName")%>
                                    </td>
                                    <td>
                                        <%#Eval("Dateds")%>
                                    </td>
                                    <td>
                                        <%#Eval("Amount")%>
                                    </td>
                                    <td>
                                        <%#Eval("StatusName")%>
                                    </td>
                                    <td class="listTableActionButtonDiv">
                                        <asp:Button ID="btn_edit" runat="server" class="btn_edit" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td colspan="8" class="navigationRow">
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
                                    Payment Voucher
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td style="width: 32%">
                                            Code
                                            <asp:Label ID="lblCode" Font-Bold="true" runat="server" class="lbl"></asp:Label>
                                        </td>
                                        <td style="width: 32%">
                                            Date <span style="color: Red">&nbsp*</span>
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
                                        <td style="width: 35%" rowspan="9">
                                            <asp:UpdatePanel ID="updOfcSettle" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div id="divOfcSettle" runat="server" visible="false">
                                                        <div style="overflow: auto; max-height: 350px;">
                                                            <table class="listTable" style="width: 97%">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="width: 15%">Select
                                                                        </th>
                                                                        <th style="width: 27%">Ref No
                                                                        </th>
                                                                        <th style="width: 27%">Pending 
                                                                        </th>
                                                                        <th style="width: 27%">Pay
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:Repeater ID="rptOfcSettle" runat="server">
                                                                        <ItemTemplate>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chk_select" runat="server" class="chk_sel OfcSettlesupchkitem" Checked='<%# Convert.ToBoolean(Eval("Selectd")) %>' />
                                                                                       </td>
                                                                                <td>
                                                                                    <%#Eval("Code") %>
                                                                                   <asp:HiddenField ID="hdnInvoiceId" runat="server" Value='<%#Eval("InvoiceId") %>' />
                                                                                    <asp:HiddenField ID="hdnTCId" runat="server" Value='<%#Eval("TCId") %>' />
                                                                                    <asp:HiddenField ID="hdn_D_id" runat="server" Value='<%#Eval("Id") %>' />
                                                                                </td>
                                                                                <td style="text-align: right">
                                                                                    <asp:TextBox ID="txtOfcSettleBalAmt" runat="server" class="txt_lable read_Only OfcSettleBalAmt"
                                                                                        Text='<%#Eval("Balance") %>'></asp:TextBox>
                                                                                </td>
                                                                                <td style="text-align: right">
                                                                                    <asp:TextBox ID="txtOfcSettlePayAmt" runat="server" Font-Size="12px" class="txt_100 numbers_only OfcSettlePayAmt"
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
                                                                        <asp:TextBox ID="txtOfcSettleTotAmt" runat="server" class="txt_lable OfcSettleTotAmt read_Only"></asp:TextBox>

                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                            <asp:UpdatePanel ID="updMaintnce" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div id="divMaintnce" runat="server" visible="false">
                                                        <div style="overflow: auto; max-height: 350px;">
                                                            <table class="listTable" style="width: 97%">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="width: 15%">Select
                                                                        </th>
                                                                        <th style="width: 27%">Ref No
                                                                        </th>
                                                                        <th style="width: 27%">Pending 
                                                                        </th>
                                                                        <th style="width: 27%">Pay
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:Repeater ID="rptMaintnce" runat="server">
                                                                        <ItemTemplate>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chk_selectM" runat="server" class="chk_sel Maintncesupchkitem" Checked='<%# Convert.ToBoolean(Eval("Selectd")) %>' />
                                                                                       </td>
                                                                                <td>
                                                                                    <%#Eval("Code") %> 
                                                                                    <asp:HiddenField ID="hdnMaintnceId" runat="server" Value='<%#Eval("MaintenanceId") %>' />
                                                                                    <asp:HiddenField ID="hdn_MD_id" runat="server" Value='<%#Eval("Id") %>' />
                                                                                </td>
                                                                                <td style="text-align: right">
                                                                                    <asp:TextBox ID="txtMaintnceBalAmt" runat="server" class="txt_lable read_Only MaintnceBalAmt"
                                                                                        Text='<%#Eval("Balance") %>'></asp:TextBox>
                                                                                </td>
                                                                                <td style="text-align: right">
                                                                                    <asp:TextBox ID="txtMaintncePayAmt" runat="server" Font-Size="12px" class="txt_100 numbers_only MaintncePayAmt"
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
                                                                        <asp:TextBox ID="txtMaintnceTotAmt" runat="server" class="txt_lable MaintnceTotAmt read_Only"></asp:TextBox>

                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:Label ID="lblTrn" runat="server" class="lbl" Text="TRN"></asp:Label>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                          </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            To Type <span style="color: Red">&nbsp*</span>
                                            <telerik:RadComboBox ID="drpTo" Sort="Ascending" Filter="Contains" runat="server"
                                                OnSelectedIndexChanged="drpToOnSelectedIndexChanged" AllowCustomText="false" RenderMode="Lightweight"
                                                OnClientFocus="OnClientKeyPressing" AutoPostBack="true" OnClientBlur="ValidateCombo"
                                                EmptyMessage="Search Type..." Style="overflow: hidden; width: 96%; border: none!important;">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="1" Text="Tenant" />
                                                    <telerik:RadComboBoxItem Value="2" Text="LandLord" />
                                                    <telerik:RadComboBoxItem Value="3" Text="Employee" />
                                                     <telerik:RadComboBoxItem Value="10" Text="Loan" />
                                                    <telerik:RadComboBoxItem Value="9" Text="Partner" />
                                                     <telerik:RadComboBoxItem Value="7" Text="FinalSettlement" />
                                                     <telerik:RadComboBoxItem Value="8" Text="Maintenance" />
                                                    <telerik:RadComboBoxItem Value="4" Text="Cash" />
                                                    <telerik:RadComboBoxItem Value="5" Text="Bank Account" />
                                                     <telerik:RadComboBoxItem Value="11" Text="VAT Payment" />
                                                     <telerik:RadComboBoxItem Value="12" Text="Security Deposit Return" />
                                                    <telerik:RadComboBoxItem Value="6" Text="General Expense" />
                                                </Items>
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="drpTo"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                   
                                    <td>
                                            <asp:UpdatePanel ID="UpdTo" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblToLabel" runat="server" class="lbl" Text=""></asp:Label>
                                                    <telerik:RadComboBox ID="drpCustomer" Sort="Ascending" Filter="Contains" runat="server"
                                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                        OnSelectedIndexChanged="drpCustomerOnSelectedIndexChanged" AutoPostBack="true"
                                                        OnClientBlur="ValidateCombo" EmptyMessage="Search Name..." Style="overflow: hidden;
                                                        width: 96%; border: none!important;" Visible="true">
                                                    </telerik:RadComboBox>
                                                    <telerik:RadComboBox ID="drpVendor" Sort="Ascending" Filter="Contains" runat="server"
                                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                         AutoPostBack="true" OnSelectedIndexChanged="drpVendorOnSelectedIndexChanged"
                                                        OnClientBlur="ValidateCombo" EmptyMessage="Search LandLord..." Style="overflow: hidden;
                                                        width: 96%; border: none!important;" Visible="false">
                                                    </telerik:RadComboBox>
                                                    <telerik:RadComboBox ID="drpEmployee" Sort="Ascending" Filter="Contains" runat="server"
                                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                        OnSelectedIndexChanged="drpEmployeeOnSelectedIndexChanged" AutoPostBack="true"
                                                        OnClientBlur="ValidateCombo" EmptyMessage="Search Employee..." Style="overflow: hidden;
                                                        width: 96%; border: none!important;" Visible="false">
                                                    </telerik:RadComboBox>
                                                      <telerik:RadComboBox ID="drpSupplier" Sort="Ascending" Filter="Contains" runat="server"
                                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                        OnSelectedIndexChanged="drpSupplierOnSelectedIndexChanged" AutoPostBack="true"
                                                        OnClientBlur="ValidateCombo" EmptyMessage="Search ..." Style="overflow: hidden;
                                                        width: 96%; border: none!important;" Visible="false">
                                                    </telerik:RadComboBox>
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
                                                    <telerik:RadComboBox ID="drpPettyCash" Sort="Ascending" Filter="Contains" runat="server"
                                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                        OnClientBlur="ValidateCombo" EmptyMessage="Search Name..." Style="overflow: hidden;
                                                        width: 96%; border: none!important;" Visible="false">
                                                    </telerik:RadComboBox>
                                                    <telerik:RadComboBox ID="drpBankAccount" Sort="Ascending" Filter="Contains" runat="server"
                                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
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
                                            <asp:UpdatePanel ID="updProperty" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlproperty" runat="server" Visible="false">
                                                        Property
                                                     <telerik:RadComboBox ID="drpBuilding" Sort="Ascending" Filter="Contains" runat="server"
                                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                       AutoPostBack="true" OnSelectedIndexChanged="drpBuilding_SelectedIndexChanged"
                                                        OnClientBlur="ValidateCombo" EmptyMessage="Search Name..." Style="overflow: hidden;
                                                        width: 96%; border: none!important;">
                                                    </telerik:RadComboBox>
                                                            <asp:RequiredFieldValidator ID="rqdProperty" runat="server" ControlToValidate="drpBuilding"
                                                        ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                   </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                             
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="updFlat" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                      <asp:Panel ID="pnlFlat" runat="server" Visible="false">
                                                    Flat 
                                                    <telerik:RadComboBox ID="drpFlat" Sort="Ascending" Filter="Contains" runat="server"
                                                        AutoPostBack="true" OnSelectedIndexChanged="drpFlat_SelectedIndexChanged"
                                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                       OnClientBlur="ValidateCombo" EmptyMessage="Search Flat..."
                                                        Style="overflow: hidden; width: 96%; border: none!important;">
                                                    </telerik:RadComboBox>
                                                            <asp:RequiredFieldValidator ID="rqdFlat" runat="server" ControlToValidate="drpFlat"
                                                        ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                        InitialValue=""></asp:RequiredFieldValidator>  
                                                  </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                  </tr>
                                    <tr>
                                        <td>
                                            Expense Type <span style="color: Red">&nbsp*</span>
                                             <asp:UpdatePanel ID="UpdExpenseDrop_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                            <telerik:RadComboBox ID="drpExpenseType" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                AutoPostBack="true" OnSelectedIndexChanged="drpExpenseTypeOnSelectedIndexChanged"
                                                OnClientBlur="ValidateCombo" EmptyMessage="Search Expense Type..." Style="overflow: hidden;
                                                width: 96%; border: none!important;">
                                            </telerik:RadComboBox>
                                                     </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drpExpenseType"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                       
                                    </tr>
                                  
                                    <tr>
                                      <td>
                                           <asp:UpdatePanel ID="updinvoiceno" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlinvoiceno" runat="server" Visible="false">
                                                        Invoice No
                                                    <asp:TextBox ID="txtinvoiceno" class="txt" runat="server"></asp:TextBox>

                                                   </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                      </td>
                                      <td></td>
                                  </tr>
                                    <tr>
                                        <td>
                                               <asp:UpdatePanel ID="updfromtype" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                            From Type  <span style="color: Red">&nbsp*</span>
                                            <telerik:RadComboBox ID="drpFromType" Sort="Ascending" Filter="Contains" runat="server"
                                                OnSelectedIndexChanged="drpFromTypeOnSelectedIndexChanged" AllowCustomText="false"
                                                RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing" AutoPostBack="true"
                                                OnClientBlur="ValidateCombo" EmptyMessage="Search Name..." Style="overflow: hidden;
                                                width: 96%; border: none!important;">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="1" Text="Cash" />
                                                    <telerik:RadComboBoxItem Value="2" Text="Bank Transaction" />
                                                    <telerik:RadComboBoxItem Value="3" Text="Cheque" />
                                                     <telerik:RadComboBoxItem Value="5" Text="Loan" />
                                                </Items>
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="drpFromType"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                                     </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdFrom" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblFromLabel" runat="server" class="lbl" Text="PettyCash"></asp:Label>
                                                    <telerik:RadComboBox ID="drpPettyCashFrom"  runat="server"
                                                        OnSelectedIndexChanged="drpPettyCashFromOnSelectedIndexChanged" AllowCustomText="false"
                                                        RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing" AutoPostBack="true"
                                                        OnClientBlur="ValidateCombo" EmptyMessage="Search Name..." Style="overflow: hidden;
                                                        width: 96%; border: none!important;">
                                                    </telerik:RadComboBox>
                                                    <telerik:RadComboBox ID="drpBankAccountFrom"  runat="server"
                                                        OnSelectedIndexChanged="drpBankAccountFromOnSelectedIndexChanged" AllowCustomText="false"
                                                        RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing" AutoPostBack="true"
                                                        OnClientBlur="ValidateCombo" EmptyMessage="Search Name..." Style="overflow: hidden;
                                                        width: 96%; border: none!important;" Visible="false">
                                                    </telerik:RadComboBox>
                                                      <telerik:radcombobox id="drpLoanFrom" runat="server" allowcustomtext="false" autopostback="true"
                                            rendermode="Lightweight" onclientfocus="OnClientKeyPressing" onselectedindexchanged="drpLoanFrom_SelectedIndexChanged"
                                            onclientblur="ValidateCombo" emptymessage="Search Name..." style="overflow: hidden; width: 96%; border: none!important;"
                                            visible="false">
                                                    </telerik:radcombobox>
                                                    <telerik:radcombobox id="drpRVCheque" runat="server" allowcustomtext="false"  
                                            rendermode="Lightweight" onclientfocus="OnClientKeyPressing" AutoPostBack="true"
                                                        OnSelectedIndexChanged="drpRVCheque_SelectedIndexChanged"
                                            onclientblur="ValidateCombo" emptymessage="Search ChequeNo..." style="overflow: hidden; width: 96%; border: none!important;"
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
                                        <td>
                                            Amount <span style="color: Red">&nbsp*</span>
                                            <asp:UpdatePanel ID="updAmountMain" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtAmountMain" class="numbers_only txt Payamt" runat="server"></asp:TextBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtAmountMain"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            Bank Commission <span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txtCommission" class="numbers_only txt" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtCommission"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Tax Type <span style="color: Red">&nbsp*</span>
                                            <telerik:RadComboBox ID="drpTaxType" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                OnClientBlur="ValidateCombo" EmptyMessage="Search Tax Type..." Style="overflow: hidden;
                                                width: 96%; border: none!important;">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="1" Text="Percentage" />
                                                    <telerik:RadComboBoxItem Value="2" Text="Amount" />
                                                </Items>
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="drpTaxType"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            Tax <span style="color: Red">&nbsp*</span>
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
                                                    <asp:Label ID="lblChequeDate" runat="server" class="lbl" Text="Cheque Date"
                                                        Visible="false"></asp:Label>
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
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            Transaction Details
                                            <asp:TextBox ID="txtTransaction" class="txt" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            Remarks
                                            <asp:TextBox ID="txtRemarks" class="txtarea" TextMode="MultiLine" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            Attachment
                                            <asp:FileUpload ID="fileUpload" runat="server" class="txt" />
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
                                                                <th style="background-color: #513b71; border: 1px solid #dddddd; color: white; padding: 7px;
                                                                    text-align: left;">
                                                                    Payable
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
                                                                <th>
                                                                    Balance
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
                                                    <asp:Button ID="btnSave" class="butn_save" ValidationGroup="save" runat="server"
                                                        Text="Save" OnClick="Save" 
                                                     OnClientClick="if (Page_ClientValidate('save') == false) return(false);else return confirm('Do you really want to Save.. ?');" />
                                                    <asp:Button ID="btnSavePrint" class="butn_save" ValidationGroup="save" runat="server"
                                                        Text="Save & Print" OnClick="btnSavePrintOnClick" 
                                                     OnClientClick="if (Page_ClientValidate('save') == false) return(false);else return confirm('Do you really want to Save.. ?');" />
                                                    <asp:Button ID="btnPrint" class="butn_save" ValidationGroup="save" runat="server"
                                                        Text="Print" OnClick="btnPrintOnClick" />
                                                    <asp:Button ID="btnOpenCancel" class="butn_delete" runat="server" Text="Cancel" OnClientClick="javascript : return confirm('Do you really want to cancel.. ?');"
                                                        OnClick="btnOpenCancel_OnClick" />
                                                    <asp:Button ID="btnReset" class="butn_delete" runat="server" OnClientClick="javascript : return confirm('Do you really want to reset.. ?');"
                                                        Visible="true" Text="Reset" OnClick="btnReset_OnClick" />
                                                    <asp:Button ID="Button1" class="butn" runat="server" Text="Close" OnClick="btn_close_OnClick" />
                                                    <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdn_update" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdn_print" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdn_add_N_print" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdn_update_N_print" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdn_cancel" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdn_user_id" runat="server" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                                <div>
                                    <div id="div1" class="messageAlert div_pop animated" style="display: none" runat="server">
                                        <div class="tick">
                                            &#10004</div>
                                        <div>
                                            <asp:Label ID="lbl_msgin" runat="server" class="messageLabel"></asp:Label>
                                        </div>
                                    </div>
                                </div>
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
                                <td>
                                    Remark <span style="color: Red">&nbsp*</span>
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
                                   
                                    <asp:Button ID="btnCloseCancel" class="butn" runat="server" Text="Close" OnClick="btnCloseCancel_OnClick" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="Upd_Add_PanelSupplier" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlSupplier" Visible="false" runat="server">
                        <AmarCentre:supplierMaster ID="UCSupplier" runat="server" />
                </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
    </div>
</asp:Content>
