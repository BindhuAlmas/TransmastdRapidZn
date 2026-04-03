<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true"
    CodeBehind="Receipt.aspx.cs" Inherits="AmarCentre.Transactions.Receipt" %>
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

            /*Unit Price,Amount,Discount*/
            $('.discount').blur(function (e) {
                $('.discount').each(function () {
                    var Price = 0;
                    var Qty = 0;
                    var Discount = 0;
                    var expamt = 0;
                    var taxper = 0;
                    var TaxAmt = 0;
                    var PriceWTax = 0;
                    var TotAmt = 0;
                    var Fine = 0;
                    var AddSC = 0;
                    var InvoiceType = 0;
                    if ($('#hdnTaxAppliedWithDiscount').val() != '') {
                        TaxAppliedWithDiscount = parseInt($('#hdnTaxAppliedWithDiscount').val());
                    }
                    if ($('#hdnInvoiceType').val() != '') {
                        InvoiceType = parseInt($('#hdnInvoiceType').val());
                    }
                    if ($(this).closest("tr").find('.price').val() != '') {
                        Price = parseFloat($(this).closest("tr").find('.price').val());
                    }
                    /*Expense*/
                    if ($(this).closest("tr").find('#hdn_expn').val() != '') {
                        expamt = parseFloat($(this).closest("tr").find('#hdn_expn').val());
                    }
                    if ($(this).closest("tr").find('.qty').val() != '') {
                        Qty = parseFloat($(this).closest("tr").find('.qty').val());
                    }

                    if ($(this).closest("tr").find('.discount').val() != '') {
                        Discount = parseFloat($(this).closest("tr").find('.discount').val());
                    }
                    /*Tax Percentage*/
                    if ($(this).closest("tr").find('#hdn_tax').val() != '') {
                        taxper = parseFloat($(this).closest("tr").find('#hdn_tax').val());
                    }

                    if ($(this).closest("tr").find('.taxamt').val() != '') {
                        TaxAmt = parseFloat($(this).closest("tr").find('.taxamt').val());
                    }

                    if ($(this).closest("tr").find('#hdn_fine').val() != '') {
                        Fine = parseFloat($(this).closest("tr").find('#hdn_fine').val());
                    }

                    if ($(this).closest("tr").find('#hdnAddServiceCharge').val() != '') {
                        AddSC = parseFloat($(this).closest("tr").find('#hdnAddServiceCharge').val());
                    }
                    if (InvoiceType == 1) {
                        if (TaxAppliedWithDiscount == 1) {
                            TaxAmt = ((parseFloat(Price) - parseFloat(Discount) - parseFloat(expamt)) * parseFloat(taxper) / 100).toFixed(2);
                        }
                    }
                    PriceWTax = parseFloat(Price) - parseFloat(Discount) + parseFloat(TaxAmt) + parseFloat(Fine) + parseFloat(AddSC);

                    TotAmt = ((parseFloat(PriceWTax) * parseFloat(Qty))).toFixed(2);

                    $(this).closest("tr").find('.taxamt').val(parseFloat(TaxAmt).toFixed(2));
                    $(this).closest("tr").find('.Prc_amt').val(parseFloat(PriceWTax).toFixed(2));
                    $(this).closest("tr").find('.il_tot_amt').val(parseFloat(TotAmt).toFixed(2));

                });
                Calc();
            });

            function Calc() {
                var ILTotAmt = 0;
                var ILTotDiscount = 0;
                var GrndTotAmt = 0;
                var ReceivedAmt = 0;
                var PendingAmt = 0;
                $('.rAmt').val('');
                $('.discount').each(function () {
                    var Amt = 0;
                    var Discount = 0;
                    var Qty = 0;
                    var InlineTotDisc = 0;
                    if ($(this).closest("tr").find('.il_tot_amt').val() != '') {
                        Amt = parseFloat($(this).closest("tr").find('.il_tot_amt').val());
                    }
                    if ($(this).closest("tr").find('.qty').val() != '') {
                        Qty = parseFloat($(this).closest("tr").find('.qty').val());
                    }
                    if ($(this).closest("tr").find('.discount').val() != '') {
                        Discount = parseFloat($(this).closest("tr").find('.discount').val());
                    }
                    InlineTotDisc = parseFloat(Discount) * parseFloat(Qty)
                    ILTotAmt = parseFloat(ILTotAmt) + parseFloat(Amt);
                    ILTotDiscount = parseFloat(ILTotDiscount) + parseFloat(InlineTotDisc);
                });

                GrndTotAmt = (parseFloat(ILTotAmt)).toFixed(2);

                var Final = GrndTotAmt;
                /*Amount Round Value */
                if ($('#hdnIsDisableRoundOff').val() != '1') {
                    var substr = GrndTotAmt.toString().split('.');
                    var AmtAfterDecimal = (parseFloat(GrndTotAmt) - parseFloat(substr[0])).toFixed(2);
                    var AmtBeforeDecimal = (parseFloat(GrndTotAmt) - parseFloat(AmtAfterDecimal)).toFixed(2);
                    var AmtDecimal = 0.00;
                    Final = 0;
                    if (parseFloat(AmtAfterDecimal) <= 0.12) {
                        AmtDecimal = 0.00;
                    }
                    else if (parseFloat(AmtAfterDecimal) >= 0.13 && parseFloat(AmtAfterDecimal) <= 0.37) {
                        AmtDecimal = 0.25;
                    }
                    else if (parseFloat(AmtAfterDecimal) >= 0.38 && parseFloat(AmtAfterDecimal) <= 0.62) {
                        AmtDecimal = 0.50;
                    }
                    else if (parseFloat(AmtAfterDecimal) >= 0.63 && parseFloat(AmtAfterDecimal) <= 0.87) {
                        AmtDecimal = 0.75;
                    }
                    else if (parseFloat(AmtAfterDecimal) >= 0.88) {
                        AmtDecimal = 1;
                    }
                    Final = (parseFloat(AmtBeforeDecimal) + parseFloat(AmtDecimal)).toFixed(2);
                    $('.tot_grnd_amt').val(parseFloat(Final).toFixed(2));
                }
                else {
                    $('.tot_grnd_amt').val(parseFloat(GrndTotAmt).toFixed(2));
                }
                /*End of Amount Round Value*/

                if ($('#hdn_receivedAmt').val() != '') {
                    ReceivedAmt = parseFloat($('#hdn_receivedAmt').val());
                }
                PendingAmt = parseFloat(Final) - parseFloat(ReceivedAmt);

                $('.tot_discount').val(parseFloat(ILTotDiscount).toFixed(2));
                $('.pendingAmt').val(parseFloat(PendingAmt).toFixed(2));
                if ($('.ChargedAmount').val() != '' & $('#hdnpaymenttype').val()=='2') {
                    PendingAmt =parseFloat(PendingAmt)+ parseFloat($('.ChargedAmount').val());
                }

                $('.amtPayNow').val(parseFloat(PendingAmt).toFixed(2));

                CheckAmountPayingNow();
            }

            $('.amtPayNow').blur(function (e) {
                var PendingAmt = 0;
                var AmtPayingNow = 0;
                var spotcommsn = 0;
                var ChargedAmount = 0;
                if ($('.pendingAmt').val() != '') {
                    PendingAmt = parseFloat($('.pendingAmt').val());
                }
                if ($('.amtPayNow').val() != '') {
                    AmtPayingNow = parseFloat($('.amtPayNow').val());
                }
                  if ($('.spotcommsn').val() != '') {
                        spotcommsn = parseFloat($('.spotcommsn').val());
                    }
                 if ($('.ChargedAmount').val() != '' & $('#hdnpaymenttype').val()=='2') {
                    ChargedAmount = parseFloat($('.ChargedAmount').val());
                }

                if ( parseFloat(PendingAmt)+ parseFloat(ChargedAmount) < (parseFloat(AmtPayingNow) + parseFloat(spotcommsn))) {
                    alert('Amount cannot be greater than Pending Amount');
                    $('.amtPayNow').val('');
                    $('.balanceAmt').val($('.receivedAmt').val());
                    $('.amtPayNow').focus();
                }
                CalcCommsn();
            });

             $('.spotcommsn').blur(function (e) {
                    var PendingAmt = 0;
                    var AmtPayingNow = 0;
                    var spotcommsn = 0;
                     var ChargedAmount = 0;

                    if ($('.pendingAmt').val() != '') {
                        PendingAmt = parseFloat($('.pendingAmt').val());
                    }
                      if ($('.spotcommsn').val() != '') {
                        spotcommsn = parseFloat($('.spotcommsn').val());
                    }
                   
                    if ($('.ChargedAmount').val() != '' & $('#hdnpaymenttype').val() == '2') {
                        ChargedAmount = parseFloat($('.ChargedAmount').val());
                     }
                     AmtPayingNow = parseFloat(PendingAmt) - parseFloat(ChargedAmount) - parseFloat(spotcommsn);
                     $('.amtPayNow').val(AmtPayingNow.toFixed(2));

                     CalcCommsn();
                });

            function CalcCommsn() {
                var Commsn = 0;
                var bankcmper = 0;
                var AmtPayingNow = 0;
                var commsvat = 0;
                var isvatapp = 0;

                isvatapp = $('#hdnisCommissionVat').val();
                bankcmper = $('#hdn_bankcommsn').val();
                if ($('.amtPayNow').val() != '') {
                    AmtPayingNow = parseFloat($('.amtPayNow').val());
                }
                Commsn = parseFloat(AmtPayingNow) * parseFloat(bankcmper) / 100;
                $('.comssnAmt').val(Commsn.toFixed(2));
                commsvat = parseFloat(Commsn) * parseFloat(0.05);
                $('.txtCommissionVat').val(commsvat.toFixed(2));

            }

            function CheckAmountPayingNow() {
                var PendingAmt = 0;
                var AmtPayingNow = 0;
                var spotcommsn = 0;
                var ChargedAmount = 0;

                if ($('.pendingAmt').val() != '') {
                    PendingAmt = parseFloat($('.pendingAmt').val());
                }
                if ($('.amtPayNow').val() != '') {
                    AmtPayingNow = parseFloat($('.amtPayNow').val());
                }
                  if ($('.spotcommsn').val() != '') {
                        spotcommsn = parseFloat($('.spotcommsn').val());
                    }
                 if ($('.ChargedAmount').val() != '' & $('#hdnpaymenttype').val()=='2') {
                    ChargedAmount = parseFloat($('.ChargedAmount').val());
                }

                if ( parseFloat(PendingAmt)+ parseFloat(ChargedAmount) <  (parseFloat(AmtPayingNow)+ parseFloat(spotcommsn)) ) {
                    alert('Amount cannot be greater than Pending Amount');
                    $('.amtPayNow').val('');
                    $('.balanceAmt').val($('.receivedAmt').val());
                    $('.amtPayNow').focus();
                }

                else {
                    FillBalanceAmount();
                }

                CalcCommsn();
            }
            function FillBalanceAmount() {
                var RAmt = 0;
                var Balance = 0;
                var AmtPayingNow = 0;
                var PendingAmt = 0;
                if ($('.amtPayNow').val() != '') {
                    AmtPayingNow = parseFloat($('.amtPayNow').val());
                }
                if ($('.pendingAmt').val() != '') {
                    PendingAmt = parseFloat($('.pendingAmt').val());
                }
                if ($('.rAmt').val() != '') {
                    RAmt = parseFloat($('.rAmt').val());
                    if (parseFloat(RAmt) < parseFloat(AmtPayingNow)) {
                        $('.amtPayNow').val($('.rAmt').val());
                        AmtPayingNow = parseFloat($('.amtPayNow').val());
                        Balance = parseFloat(RAmt) - parseFloat(AmtPayingNow);
                        $('.balanceAmt').val(parseFloat(Balance).toFixed(2));
                    } else {

                        Balance = parseFloat(RAmt) - parseFloat(AmtPayingNow);
                        $('.balanceAmt').val(parseFloat(Balance).toFixed(2));
                    }
                }
                else {
                    $('.balanceAmt').val('');
                }
                CalcCommsn();
            }
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Receipt/إيصال
        <asp:Button ID="btn_addnew" runat="server"  class="btnAddNew" OnClick="btn_newentry_OnClick" />
        <asp:Button ID="btnexcel_export" runat="server" class="btn_excel right_align_list"
            ToolTip="Export to Excel" OnClick="btnexcel_export_OnClick" />
        <div class="searchDiv">
            <asp:TextBox ID="txt_search" runat="server" class="txt_search" AutoPostBack="true"
                OnTextChanged="txt_search_OnTextChanged" placeholder="Search"></asp:TextBox>
        </div>
          <telerik:RadComboBox ID="drprecStatus" Sort="Ascending" Filter="Contains" runat="server"
            AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
            OnClientBlur="ValidateCombo" EmptyMessage="Search ..." AutoPostBack="true"
            OnSelectedIndexChanged="txt_search_OnTextChanged" Style="overflow: hidden;
            width: 16%; border: none!important; float: right; padding-right: 5px; margin-top: 0px">
             <Items>
                 <telerik:RadComboBoxItem Value="1" Text="Active" Selected="true" />
                 <telerik:RadComboBoxItem Value="2" Text="Cancelled" />
             </Items>
        </telerik:RadComboBox>
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
                                Sl No /رقم
                            </th>
                            <th style="width: 8%;">
                                Code / رمز
                            </th>
                            <th style="width: 10%;">
                                Invoice Code / رمز الفاتورة
                            </th>
                            <th style="width: 20%;">
                                Customer / زبون
                            </th>
                            <th style="width: 8%;">
                                Date / تاريخ
                            </th>
                            <th style="width: 10%;">
                               Invoice Amount / المبلغ
                            </th>
                             <th style="width: 10%;">
                               Paid Amount / المبلغ
                            </th>
                            <th style="width: 8%;">
                                Status/الحالة
                            </th>
                            <th style="width: 6%;">
                                Action/عمل
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
                                        <%#Eval("Code")%>
                                    </td>
                                    <td>
                                        <%#Eval("InvoiceCode")%>
                                    </td>
                                    <td>
                                        <%#Eval("Name")%>
                                    </td>
                                    <td>
                                        <%#Eval("Dateds")%>
                                    </td>
                                    <td>
                                        <%#Eval("AfterDiscount_GrandTotal")%>
                                    </td>
                                    <td>
                                        <%#Eval("Amount")%>
                                    </td>
                                    <td>
                                        <%#Eval("StatusName")%>
                                    </td>
                                    <td >
                                        <asp:Button ID="btn_edit" runat="server" class="btn_edit" ToolTip="Edit" CommandName="Edit" />
                                        <asp:Button ID="btnPrint" runat="server" class="btn_print" ToolTip="Print" CommandName="Print" />
                                          <asp:Button ID="btnSendmail" runat="server" class="btnsendmail" ToolTip="Send Mail"
                                            CommandName="Sendmail" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td colspan="9" class="navigationRow">
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
                                    Receipt / إيصال
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td style="width: 33%">
                                            Receipt Code / رمز الاستلام
                                            <asp:TextBox ID="lbl_Code" runat="server" class="txt read_Only" Font-Bold="true"
                                                Text=""></asp:TextBox>
                                        </td>
                                        <td style="width: 33%">
                                            Date / تاريخ <span style="color: Red">&nbsp*</span>
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
                                        <td style="width: 33%">
                                            <asp:UpdatePanel ID="updinvoice" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    Invoice Code / رمز الفاتورة
                                                    <asp:TextBox ID="txt_invCode" AutoPostBack="true" OnTextChanged="txt_invCode_OnTextChanged"
                                                        class="txt" runat="server"></asp:TextBox>
                                                    <asp:HiddenField ID="hdn_invId" runat="server" Value="0" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                         <td style="width: 33%">
                                            Customer
                                            <telerik:RadComboBox ID="drp_customer" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                AutoPostBack="true" OnClientBlur="ValidateCombo" EmptyMessage="Search Name..."
                                                OnSelectedIndexChanged="drp_customer_OnSelectedIndexChanged" Style="overflow: hidden;
                                                width: 96%; border: none!important;">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td style="width: 33%">
                                            <asp:UpdatePanel ID="updinvoiceDrp" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    Invoice Code / رمز الفاتورة
                                                    <telerik:RadComboBox ID="drpInvoice" Sort="Ascending" Filter="Contains" runat="server"
                                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                        AutoPostBack="true" OnClientBlur="ValidateCombo" EmptyMessage="Search invoice..."
                                                        OnSelectedIndexChanged="drpInvoiceOnSelectedIndexChanged" Style="overflow: hidden;
                                                        width: 96%; border: none!important;">
                                                    </telerik:RadComboBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td style="width: 33%">
                                            Customer Name/ اسم الزبون
                                            <br />
                                            <asp:TextBox ID="txt_customerName" runat="server" class="txt read_Only" Font-Bold="true"
                                                Text=""></asp:TextBox>
                                            <asp:HiddenField ID="hdn_customerId" runat="server" Value="" />
                                           

                                        </td>
                                       
                                    </tr>
                                    <tr>
                                        <td style="width: 33%">
                                            <br />
                                            <asp:TextBox ID="txt_quotCode" runat="server" class="txt read_Only" Font-Bold="true"
                                                Visible="false" Text=""></asp:TextBox>
                                            <asp:HiddenField ID="hdn_quotId" runat="server" Value="" />
                                        </td>
                                        <td>

                                        </td>
                                        <td>
                                              <asp:UpdatePanel ID="updadvance" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lbladvance" runat="server" Font-Bold="true" ></asp:Label>
                                                         <asp:HiddenField ID="hdnAdvance" runat="server" Value="" />
                                                        </ContentTemplate>
                                                  </asp:UpdatePanel>
                                        </td>
                                    </tr>
                               </table>
                                <table class="formTable">
                                    <tr>
                                        <td colspan="4">
                                            <div id="div_item_new" runat="server" style="width: 100%; overflow: auto;">
                                                <asp:UpdatePanel ID="Upd_ItemList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table class="listTable">
                                                            <thead>
                                                                <tr style="text-align: center">
                                                                    <th style="width: 3%">Sl./رقم
                                                                    </th>
                                                                    <th style="width: 23%">Service / الخدمات
                                                                    </th>
                                                                    <th style="width: 15%">Particulars / تفاصيل
                                                                    </th>
                                                                    <th style="width: 10%">Price / السعر
                                                                    </th>
                                                                    <th style="width: 10%">Discount / خصم
                                                                    </th>
                                                                    <th style="width: 5%">Qty / الكمية
                                                                    </th>
                                                                    <th style="width: 7%">Tax / ضريبة
                                                                    </th>
                                                                    <th style="width: 10%">Amt With Tax / ضريبة
                                                                    </th>
                                                                    <th style="width: 10%">Total / مجموع
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <asp:Repeater ID="rpt_Item_list" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr style="text-align: center">
                                                                            <td>
                                                                                <%# Container.ItemIndex + 1 %>
                                                                                <asp:HiddenField ID="hdn_D_id" runat="server" Value='<%#Eval("D_id") %>' />
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:HiddenField ID="hdn_catgory_id" runat="server" Value='<%#Eval("CategoryId") %>' />
                                                                                <asp:HiddenField ID="hdn_service_id" runat="server" Value='<%#Eval("Service_Id") %>' />
                                                                                <asp:Label ID="lbl_service" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:Label ID="lbl_desc" runat="server" Text='<%#Eval("Particulars") %>'></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txt_price" TabIndex="-1" class="numbers_only read_Only price inline txt asLabel"
                                                                                    Width="85%" runat="server" Text='<%#Eval("Price") %>'></asp:TextBox>
                                                                                <asp:HiddenField ID="hdn_expn" ClientIDMode="Static" runat="server" Value='<%#Eval("Expense") %>' />
                                                                                <asp:HiddenField ID="hdn_sc" ClientIDMode="Static" runat="server" Value='<%#Eval("ServiceCharge") %>' />
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txt_discount" class="numbers_only discount inline txt" Width="75%"
                                                                                    runat="server" Text='<%#Eval("Discount") %>'></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txt_Qty" TabIndex="-1" class="numbers_only read_Only qty inline txt asLabel"
                                                                                    Width="75%" runat="server" Text='<%#Eval("Quantity") %>'></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txt_tax" TabIndex="-1" class="numbers_only taxamt read_Only txt asLabel"
                                                                                    Width="95%" runat="server" Text='<%#Eval("TaxAmount") %>'></asp:TextBox>
                                                                                <asp:HiddenField ID="hdn_tax" ClientIDMode="Static" runat="server" Value='<%#Eval("Tax") %>' />
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txt_priceWitTax" TabIndex="-1" class="numbers_only Prc_amt read_Only txt asLabel"
                                                                                    Width="95%" runat="server" Text='<%#Eval("PriceWitTax") %>'></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txt_totPrice" TabIndex="-1" class="numbers_only il_tot_amt read_Only txt asLabel"
                                                                                    Width="95%" runat="server" Text='<%#Eval("Total") %>'></asp:TextBox>
                                                                                <asp:HiddenField ID="hdn_fine" ClientIDMode="Static" runat="server" Value='<%#Eval("Fine") %>' />
                                                                                <asp:HiddenField ID="hdnAddServiceCharge" ClientIDMode="Static" runat="server" Value='<%#Eval("AdditionalServiceCharge") %>' />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                                <tr>
                                                                    <td colspan="7" style="text-align: right;">Discount / خصم
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:TextBox TabIndex="-1" Style="border: medium none; color: Red; font-size: 24px; text-align: right; width: 95%"
                                                                            class="txt tot_discount read_Only" ID="txt_totDiscount"
                                                                            runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="7" style="text-align: right">Total / مجموع
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:TextBox TabIndex="-1" Style="border: medium none; color: Red; font-size: 24px; text-align: right; width: 95%"
                                                                            class="txt tot_grnd_amt read_Only txt_80" ID="txt_grand"
                                                                            runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td colspan="7" style="text-align: right">Pending / قيد الانتظار
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:TextBox TabIndex="-1" Style="border: medium none; color: Red; font-size: 24px; text-align: right; width: 95%"
                                                                            class="txt pendingAmt read_Only txt_80" ID="txt_pendingAmt"
                                                                            runat="server"></asp:TextBox>
                                                                        <asp:HiddenField ID="hdn_receivedAmt" runat="server" Value="0" ClientIDMode="Static" />
                                                                    </td>
                                                                </tr>
                                                                 <tr>
                                                                    <td colspan="7" style="text-align: right">Spot Commission
                                                                    </td>
                                                                    <td colspan="2">
                                                                          <asp:TextBox class="txt spotcommsn numbers_only txt_80" ID="txtspotCommission"
                                                                            Style="font-size: 24px; text-align: right; width: 95%" runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trChargedAmount" runat="server">
                                                                    <td colspan="7" style="text-align: right">Charged Amount
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:TextBox TabIndex="-1" Style="border: medium none; color: Red; font-size: 24px; text-align: right; width: 95%"
                                                                            class="txt ChargedAmount read_Only txt_80" ID="txtChargedAmount" runat="server"></asp:TextBox>
                                                                        <asp:HiddenField ID="hdnpaymenttype" runat="server" Value="0" ClientIDMode="Static" />
                                                                    </td>
                                                                </tr>
                                                                <tr style="display: none">
                                                                    
                                                                    <td colspan="2">
                                                                        <asp:TextBox class="txt rAmt numbers_only txt_80" ID="txt_ReceivedAmt" runat="server"></asp:TextBox>
                                                                    
                                                                        <asp:TextBox TabIndex="-1" Style="border: medium none; color: Red; font-size: 24px; text-align: right; width: 95%"
                                                                            class="txt balanceAmt read_Only txt_80" ID="txt_Balance"
                                                                            runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="7" style="text-align: right">Received Amount / المبلغ الذي تسلمه<span style="color: Red">&nbsp*</span>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:TextBox class="txt amtPayNow numbers_only txt_80" ID="txt_amtPayNow"
                                                                            Style="font-size: 24px; text-align: right; width: 95%"
                                                                            runat="server"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Required"
                                                                            runat="server" ControlToValidate="txt_amtPayNow" ValidationGroup="save" InitialValue=""
                                                                            Style="color: Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="7" style="text-align: right">Bank Commission/عمولة البنك
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:UpdatePanel ID="upd_commsn" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:TextBox class="txt comssnAmt numbers_only txt_80" ID="txt_commsn"
                                                                                    Style="font-size: 24px; text-align: right; width: 95%" runat="server"></asp:TextBox>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <div style="height: 10px">
                                                </div>
                                            </div>
                                            <div>
                                                <table style="width:95%">
                                                    <tr>
                                                        <td style="width: 25%">Payment Mode / طريقة الدفع <span style="color: Red">&nbsp*</span>
                                                            <asp:UpdatePanel ID="UpdDrpPaymentModePAnel" runat="server" ChildrenAsTriggers="false"
                                                                UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <telerik:RadComboBox ID="drp_payMode" runat="server"
                                                                        AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Payment Mode..."
                                                                        Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                        OnClientBlur="ValidateCombo" OnSelectedIndexChanged="drp_payMode_OnSelectedIndexChanged"
                                                                        AutoPostBack="true">
                                                                        <Items>
                                                                            <telerik:RadComboBoxItem Value="1" Text="Cash" />
                                                                            <telerik:RadComboBoxItem Value="2" Text="Bank Transaction" />
                                                                            <telerik:RadComboBoxItem Value="3" Text="Cheque" />
                                                                            <telerik:RadComboBoxItem Value="4" Text="Advance" />
                                                                            <telerik:RadComboBoxItem Value="5" Text="Loan" />
                                                                            <telerik:RadComboBoxItem Value="6" Text="Card Swipe" />
                                                                            <telerik:RadComboBoxItem Value="10" Text="Nomad" />
                                                                        </Items>
                                                                    </telerik:RadComboBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="drp_payMode"
                                                                ValidationGroup="save" ErrorMessage="Required" Style="color: Red" InitialValue=""
                                                                Display="Dynamic"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td style="width: 25%">
                                                            <asp:UpdatePanel ID="Upd_PayMode_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Panel ID="pnl_PayMode_Panel" Visible="false" runat="server">

                                                                        <asp:Label ID="lblToLabel" runat="server" class="lbl" Text="PettyCash"></asp:Label>
                                                                        <telerik:RadComboBox ID="drpPettyCash" runat="server"
                                                                            AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                                            OnClientBlur="ValidateCombo" EmptyMessage="Search Name..." Style="overflow: hidden; width: 85%; border: none!important;">
                                                                        </telerik:RadComboBox>
                                                                        <telerik:RadComboBox ID="drpBankAccount" runat="server"
                                                                            AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                                            AutoPostBack="true" OnSelectedIndexChanged="onchangedrp_bank" OnClientBlur="ValidateCombo"
                                                                            EmptyMessage="Search Name..." Style="overflow: hidden; width: 85%; border: none!important;"
                                                                            Visible="false">
                                                                        </telerik:RadComboBox>
                                                                        <telerik:RadComboBox ID="drpLoan" runat="server"
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

                                                                    </asp:Panel>

                                                                    <asp:Panel ID="pnl_Cheque_Panel" Visible="false" runat="server">
                                                                        Cheque Date / تحقق من التاريخ <span style="color: Red">&nbsp*</span>
                                                                        <telerik:RadDatePicker ID="cheque_date" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                                            <Calendar runat="server" ID="Calendar2" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                                                ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                                                <SpecialDays>
                                                                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                                    </telerik:RadCalendarDay>
                                                                                </SpecialDays>
                                                                            </Calendar>
                                                                        </telerik:RadDatePicker>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="cheque_date"
                                                                            Display="Dynamic" ValidationGroup="save" ErrorMessage="Required" Style="color: Red"
                                                                            InitialValue=""></asp:RequiredFieldValidator>
                                                                    </asp:Panel>

                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td style="width: 25%">
                                                            <asp:UpdatePanel ID="Upd_Cheque_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Panel ID="pnlRecChargedAmt" Visible="false" runat="server">
                                                                        Charged Amount
                                                                     <asp:TextBox class="txt numbers_only txt_80" ID="txtRecChargedAmt" runat="server"></asp:TextBox>
                                                                    </asp:Panel>
                                                                    <asp:Panel ID="pnlCommissionVat" Visible="false" runat="server">
                                                                       Vat on Commission
                                                                        <asp:TextBox class="txt numbers_only txt_80 txtCommissionVat" ID="txtCommissionVat" runat="server"></asp:TextBox>
                                                                    </asp:Panel>
                                                                    <asp:Panel ID="pnlchqno" runat="server" Visible="false">
                                                                        Cheque Number / رقم الشيك <span style="color: Red">&nbsp*</span>
                                                                        <asp:TextBox ID="txt_chqNumber" class="txt" runat="server"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_chqNumber"
                                                                            Display="Dynamic" ValidationGroup="save" ErrorMessage="Required" Style="color: Red"
                                                                            InitialValue=""></asp:RequiredFieldValidator>
                                                                    </asp:Panel>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>

                                                        </td>
                                                        <td style="width:25%"></td>
                                                    </tr>

                                                    <tr>
                                                        <td colspan="3">Remarks / ملاحظات
                                            <asp:TextBox class="txtarea" Style="width: 90%" TextMode="MultiLine" ID="txt_remark" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                    <asp:Label ID="lblenablemsg" runat="server" ForeColor="Red"></asp:Label>
                                                            <br />
                                                            <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdn_user_id" runat="server" />
                                                            <asp:HiddenField ID="hdnTaxAppliedWithDiscount" ClientIDMode="Static" runat="server" />
                                                            <asp:HiddenField ID="hdnInvoiceType" ClientIDMode="Static" runat="server" />
                                                                <asp:HiddenField ID="hdnIsDisableRoundOff" ClientIDMode="Static" runat="server" Value="0" />

                                                            <asp:Button ID="btn_save" class="butn_save" ValidationGroup="save" OnClick="btn_save_OnClick"
                                                                OnClientClick="if (Page_ClientValidate('save') == false) return(false);else return confirm('Do you really want to Save.. ?');"
                                                                runat="server" Text="Save/حفظ" />
                                                            <asp:Button ID="btn_save_print" class="butn_save" ValidationGroup="save" OnClick="btn_save_print_OnClick"
                                                                OnClientClick="if (Page_ClientValidate('save') == false) return(false);else return confirm('Do you really want to Save.. ?');"
                                                                runat="server" Text="Save & Print/حفظ وطباعة" />
                                                            <asp:Button ID="btnOpenCancel" class="butn_delete" runat="server" Text="Cancel/إلغاء"
                                                                OnClientClick="javascript : return confirm('Do you really want to cancel.. ?');"
                                                                OnClick="btnOpenCancel_OnClick" />
                                                            <asp:Button ID="btn_print" class="butn" runat="server" Text="Print" OnClick="btn_print_OnClick" />
                                                            <asp:Button ID="btnOpenDelete" class="butn_delete" runat="server" Text="Delete/حذف"
                                                                OnClientClick="javascript : return confirm('Do you really want to delete.. ?');"
                                                                OnClick="btnOpenDelete_OnClick" />
                                                            <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_OnClick" />
                                                            <asp:Button ID="Button1" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_OnClick" />
                                                            <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdn_delete" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdn_print" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdn_add_N_print" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdn_cancel" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnupdate" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnupdateNPrint" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnsendmail" runat="server" Value="0" />

                                                        </td>

                                                    </tr>
                                                </table>
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
        <asp:UpdatePanel ID="updCancel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlCancel" runat="server" Visible="false">
                    <div class="popupBackground">
                    </div>
                    <div class="animated smallPopUp">
                        <div class="Adding_heading">
                            <asp:Label ID="lblCancel" runat="server" Text=""></asp:Label>
                        </div>
                        <div runat="server" visible="false" id="div_candet">
                            <div style="padding: 10px">
                                <b>Select the entries you want to cancel before cancelling Receipt/حدد الادخالات التي
                                    تريد الغاؤها قبل الغاء الفاتورة </b>
                            </div>
                            <table class="listTable">
                                <thead>
                                    <tr>
                                        <th class="listTableSlNo" style="width: 5%;">
                                            Select/اختار
                                        </th>
                                        <th style="width: 20%;">
                                            Remark/تعليق
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rpt_cancelList" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:HiddenField ID="hdndetId" runat="server" Value='<%#Eval("Id")%>' />
                                                    <asp:HiddenField ID="hdn_type" runat="server" Value='<%#Eval("Type")%>' />
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                </td>
                                                <td>
                                                    <%#Eval("Remark")%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
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
                                      OnClientClick="if (Page_ClientValidate('cancel') == false) return(false);else return confirm('Do you really want to cancel.. ?');"  runat="server" Text="Cancel" />
                                    <asp:Button ID="btnDelete" class="butn_save" ValidationGroup="cancel" OnClick="btnDelete_OnClick"
                                      OnClientClick="if (Page_ClientValidate('cancel') == false) return(false);else return confirm('Do you really want to delete.. ?');"  runat="server" Text="Delete/حذف" />
                                    <asp:Button ID="btnCloseCancel" class="butn" runat="server" Text="Close/أغلق" OnClick="btnCloseCancel_OnClick" />
                                </td>
                            </tr>
                        </table>
                    </div>
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
    </div>
   
</asp:Content>
