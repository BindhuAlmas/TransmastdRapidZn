<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ServiceCompletion.aspx.cs" Inherits="AmarCentre.Transactions.ServiceCompletion" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function Confirm() {
            if (confirm("Insufficent Balance in account. Do you want to continue ?")) {
                document.getElementById('<%= Button2.ClientID%>').click();
                return;
            } else {
                return false;
            }
        }
        function ConfirmE() {
            if (confirm("Expense amount greater than service amount. Do you want to continue ?")) {
                document.getElementById('<%= Button2.ClientID%>').click();
                return;
            } else {
                return false;
            }
        }

        function pageLoad() {


            jQuery(document).on('keyup', function (evt) {
                if (evt.keyCode == 27) {
                    //alert('Esc key pressed.');
                }
            });
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

            $('.jcalculation').blur(function (e) {
                var Qty = 1;

                if ($.trim($('.Qty').val()) != '') {
                    Qty = parseFloat($('.Qty').val());
                    if (parseFloat(Qty) == 0) {
                        Qty = 1;
                    }
                }
                var Amt = 0;
                var VAT = 0;
                var PayableAmt = 0;
                if ($.trim($(this).closest("tr").find('.amt').val()) != '') {
                    Amt = parseFloat($(this).closest("tr").find('.amt').val());
                }
                if ($.trim($(this).closest("tr").find('.vat').val()) != '') {
                    VAT = parseFloat($(this).closest("tr").find('.vat').val());
                }
                PayableAmt = (parseFloat(Amt) + parseFloat(VAT)) * parseFloat(Qty);
                $(this).closest("tr").find('.payableAmount').val(parseFloat(PayableAmt).toFixed(2));
                $(this).closest("tr").find('.paidAmount').val(parseFloat(PayableAmt).toFixed(2));

                Calculation();
            });
            $('.jcalculationout').blur(function (e) {
                Calculation();
            });

            $('.Qty').blur(function (e) {
                var Qty = 0;
                var IncQty = 0;
                if ($.trim($('.Qty').val()) != '') {
                    Qty = parseFloat($('.Qty').val());
                }
                if ($.trim($('#hdn_InComQty').val()) != '') {
                    IncQty = parseFloat($('#hdn_InComQty').val());
                }
                if (parseFloat(Qty) > parseFloat(IncQty)) {
                    alert("Quantity Cannot be greater than Pending Quantity");
                    $('.Qty').val('');
                }

                $('.amt').each(function () {  // paidamount
                    var Amt = 0;
                    var VAT = 0;
                    var PayableAmt = 0;
                    if ($.trim($(this).closest("tr").find('.amt').val()) != '') {
                        Amt = parseFloat($(this).closest("tr").find('.amt').val());
                    }
                    if ($.trim($(this).closest("tr").find('.vat').val()) != '') {
                        VAT = parseFloat($(this).closest("tr").find('.vat').val());
                    }
                    PayableAmt = (parseFloat(Amt) + parseFloat(VAT)) * parseFloat(Qty);
                    $(this).closest("tr").find('.paidAmount').val(parseFloat(PayableAmt).toFixed(2));
                });

                Calculation();
            });

            function Calculation() {
                var Qty = 1;
                var AmtSingleQty = 0;
                var GrndTotAmt = 0;

                if ($.trim($('.Qty').val()) != '') {
                    Qty = parseFloat($('.Qty').val());
                    if (parseFloat(Qty) == 0) {
                        Qty = 1;
                    }
                }
                $('.amt').each(function () {
                    var Amt = 0;
                    var VAT = 0;
                    var PayableAmt = 0;
                    if ($.trim($(this).closest("tr").find('.amt').val()) != '') {
                        Amt = parseFloat($(this).closest("tr").find('.amt').val());
                    }
                    if ($.trim($(this).closest("tr").find('.vat').val()) != '') {
                        VAT = parseFloat($(this).closest("tr").find('.vat').val());
                    }
                    AmtSingleQty = parseFloat(AmtSingleQty) + parseFloat(Amt) + parseFloat(VAT);
                    PayableAmt = (parseFloat(Amt) + parseFloat(VAT)) * parseFloat(Qty);
                    $(this).closest("tr").find('.payableAmount').val(parseFloat(PayableAmt).toFixed(2));
                    //$(this).closest("tr").find('.paidAmount').val(parseFloat(PayableAmt).toFixed(2));
                });

                var Amtout = 0;
                var VATout = 0;
                var PayableAmtout = 0;
                if ($.trim($('.amtout').val()) != '') {
                    Amtout = parseFloat($('.amtout').val());
                }
                if ($.trim($('.vatout').val()) != '') {
                    VATout = parseFloat($('.vatout').val());
                }
                AmtSingleQty = parseFloat(AmtSingleQty) + parseFloat(Amtout) + parseFloat(VATout);
                PayableAmtout = (parseFloat(Amtout) + parseFloat(VATout)) * parseFloat(Qty);
                $('.payableAmountout').val(parseFloat(PayableAmtout).toFixed(2));
                $('.paidAmountout').val(parseFloat(PayableAmtout).toFixed(2));


                $('.amtSQty').val(parseFloat(AmtSingleQty).toFixed(2));

                GrndTotAmt = parseFloat(AmtSingleQty) * parseFloat(Qty);
                $('.totAmt').val(parseFloat(GrndTotAmt).toFixed(2));
            }

            $('.paidAmount').blur(function (e) {
                var PayableAmount = 0;
                var PaidAmount = 0;
                if ($.trim($(this).closest("tr").find('.payableAmount').val()) != '') {
                    PayableAmount = parseFloat($(this).closest("tr").find('.payableAmount').val());
                }
                if ($.trim($(this).closest("tr").find('.paidAmount').val()) != '') {
                    PaidAmount = parseFloat($(this).closest("tr").find('.paidAmount').val());
                }
                if (parseFloat(PaidAmount) > parseFloat(PayableAmount)) {
                    alert("Amount Cannot be greater than Payable Amount");
                    $(this).closest("tr").find('.paidAmount').val('');
                }

            });

            $('.inlineQty').blur(function (e) {
                var Qty = 0;
                var IncQty = 0;
                var AmtSingleQty = 0;
                var GrndTotAmt = 0;
                if ($.trim($(this).closest("tr").find('.inlineQty').val()) != '') {
                    Qty = parseFloat($(this).closest("tr").find('.inlineQty').val());
                }
                if ($.trim($(this).closest("tr").find('.inComQty').val()) != '') {
                    IncQty = parseFloat($(this).closest("tr").find('.inComQty').val());
                }
                if (parseFloat(Qty) > parseFloat(IncQty)) {
                    Qty = 0;
                    alert("Quantity Cannot be greater than Pending Quantity");
                    $(this).closest("tr").find('.inlineQty').val('');
                }
                if ($.trim($(this).closest("tr").find('.inlineamtSQty').val()) != '') {
                    AmtSingleQty = parseFloat($(this).closest("tr").find('.inlineamtSQty').val());
                }
                GrndTotAmt = parseFloat(AmtSingleQty) * parseFloat(Qty);
                $(this).closest("tr").find('.inlinetotAmt').val(parseFloat(GrndTotAmt).toFixed(2));
            });

            //addtionalExpense

            $('.jcalculationAddtnl').blur(function (e) {
                CalculationAddtional();
            });

            function CalculationAddtional() {
                var Qty = 1;
                var AmtSingleQty = 0;
                var GrndTotAmt = 0;

                $('.amtAddtnl').each(function () {
                    var Amt = 0;
                    var VAT = 0;
                    var PayableAmt = 0;
                    if ($.trim($(this).closest("tr").find('.amtAddtnl').val()) != '') {
                        Amt = parseFloat($(this).closest("tr").find('.amtAddtnl').val());
                    }
                    if ($.trim($(this).closest("tr").find('.vatAddtnl').val()) != '') {
                        VAT = parseFloat($(this).closest("tr").find('.vatAddtnl').val());
                    }
                    AmtSingleQty = parseFloat(AmtSingleQty) + parseFloat(Amt) + parseFloat(VAT);
                    PayableAmt = (parseFloat(Amt) + parseFloat(VAT)) * parseFloat(Qty);
                    $(this).closest("tr").find('.payableAmountAddtnl').val(parseFloat(PayableAmt).toFixed(2));
                    $(this).closest("tr").find('.paidAmountAddtnl').val(parseFloat(PayableAmt).toFixed(2));
                });

                GrndTotAmt = parseFloat(AmtSingleQty);
                $('.totAmtAddtnl').val(parseFloat(GrndTotAmt).toFixed(2));
            }

            $('.paidAmountAddtnl').blur(function (e) {
                var PayableAmount = 0;
                var PaidAmount = 0;
                if ($.trim($(this).closest("tr").find('.payableAmountAddtnl').val()) != '') {
                    PayableAmount = parseFloat($(this).closest("tr").find('.payableAmountAddtnl').val());
                }
                if ($.trim($(this).closest("tr").find('.paidAmountAddtnl').val()) != '') {
                    PaidAmount = parseFloat($(this).closest("tr").find('.paidAmountAddtnl').val());
                }
                if (parseFloat(PaidAmount) > parseFloat(PayableAmount)) {
                    alert("Amount Cannot be greater than Payable Amount");
                    $(this).closest("tr").find('.paidAmountAddtnl').val('');
                }
            });
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
        Service Completion/استكمال الخدمة
        <asp:Button ID="btnexcel_export" runat="server" class="btn_excel right_align_list"
            ToolTip="Export to Excel" OnClick="btnexcel_export_OnClick" />
       
            <asp:TextBox ID="txt_search" runat="server" class="txt_search" AutoPostBack="true"
                OnTextChanged="txt_search_OnTextChanged" Width="16%" style="float: right;" placeholder="Search"></asp:TextBox>
       
        <telerik:RadComboBox ID="drpStatus" Sort="Ascending" Filter="Contains" runat="server"
            AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
            OnClientBlur="ValidateCombo" EmptyMessage="Search Status..." AutoPostBack="true"
            OnSelectedIndexChanged="drpStatusOnSelectedIndexChanged" Style="overflow: hidden; width: 16%; border: none!important; float: right; padding-right: 5px; margin-top: 0px">
            <Items>
                <telerik:RadComboBoxItem Value="1" Text="All" />
                <telerik:RadComboBoxItem Value="2" Text="Pending" Selected="true" />
                <telerik:RadComboBoxItem Value="3" Text="Processing" />
                <telerik:RadComboBoxItem Value="4" Text="Completed" />
                <telerik:RadComboBoxItem Value="5" Text="Invoice Cancelled" />
              
            </Items>
        </telerik:RadComboBox>
          <telerik:RadComboBox ID="drpserviceStatusfilter" Sort="Ascending" Filter="Contains" runat="server"
      AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
      OnClientBlur="ValidateCombo" EmptyMessage="Search Service Status..." AutoPostBack="true"
      OnSelectedIndexChanged="drpStatusOnSelectedIndexChanged" Style="overflow: hidden; width: 16%; border: none!important; float: right; padding-right: 5px; margin-top: 0px">
  </telerik:RadComboBox>
        <telerik:RadComboBox ID="drpInvoiceCreator" Sort="Ascending" Filter="Contains" runat="server"
            AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
            OnClientBlur="ValidateCombo" EmptyMessage="Search Invoice Creator..." AutoPostBack="true"
            OnSelectedIndexChanged="drpStatusOnSelectedIndexChanged" Style="overflow: hidden; width: 16%; border: none!important; float: right; padding-right: 5px; margin-top: 0px">
        </telerik:RadComboBox>
        <asp:Button ID="Button2" runat="server" Style="display: none" Text="" OnClick="callSAveCompletion" />
        <%--dont delete--%>
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
                            <th style="width: 5%;">Sl No
                            </th>
                            <th style="width: 8%;">Code / رمز
                            </th>
                            <th style="width: 20%;">Customer / زبون
                            </th>
                            <th style="width: 13%;">Particulars
                            </th>
                            <th style="width: 8%;">Date / تاريخ
                            </th>
                            <th style="width: 8%;">Amount / المبلغ
                            </th>
                            <th style="width: 8%;">Status / الحالة
                            </th>
                            <th style="width: 6%;">Action/عمل
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rpt_list" runat="server" OnItemCommand="rpt_list_OnItemCommand" OnItemDataBound="rpt_list_ItemDataBound">
                            <ItemTemplate>
                                <tr runat="server" id="trmainlist">
                                    <td>
                                        <%#Eval("RowNum")%>.
                                        <asp:HiddenField ID="hdn_id" runat="server" Value='<%#Eval("Id")%>' />
                                        <asp:HiddenField ID="hdnDescpCount" runat="server" Value='<%#Eval("DescpCount")%>' />
                                    </td>
                                    <td>
                                        <%#Eval("Code")%>
                                    </td>
                                    <td>
                                        <%#Eval("Name")%>
                                    </td>
                                    <td>
                                        <%#Eval("Particulars")%>
                                    </td>
                                    <td>
                                        <%#Eval("Dateds")%>
                                    </td>
                                    <td>
                                        <%#Eval("Grand_Total")%>
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
                                    Service Completion / استكمال الخدمة
                                </div>
                                <table class="Action/عملTable">
                                    <tr>
                                        <td style="width: 25%">Invoice Code / رمز الفاتورة
                                            <asp:TextBox ID="lbl_Code" TabIndex="-1" runat="server" class="txt read_Only labelled"
                                                Font-Bold="true" Text=""></asp:TextBox>
                                            Customer Name
                                        <asp:TextBox ID="lblcustomer_name" runat="server" class="txt read_Only labelled"
                                            Font-Bold="true" Text=""></asp:TextBox>
                                        </td>
                                        <td colspan="3"></td>
                                    </tr>
                                </table>
                                <div id="div_item_new" runat="server" style="width: 100%; overflow: auto;">
                                    <asp:UpdatePanel ID="Upd_ItemList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table class="listTable">
                                                <thead>
                                                    <tr style="text-align: center">
                                                        <th style="width: 20%">Service / الخدمات
                                                        </th>
                                                        <th style="width: 12%">Particulars / تفاصيل
                                                        </th>
                                                        <th style="width: 9%" runat="server" id="thServAmt">Service Amount / المبلغ
                                                        </th>
                                                        <th style="width: 7%">Invoice Qty / كمية الفاتورة
                                                        </th>
                                                        <th style="width: 7%">Pending Qty / الكمية المعلقة
                                                        </th>
                                                        <th style="width: 6%">Qty / كمية
                                                        </th>
                                                        <th style="width: 10%">Vendor
                                                        </th>
                                                        <th style="width: 9%">Expense Amount
                                                        </th>
                                                        <th style="width: 12%">Account
                                                        </th>
                                                        <th style="width: 9%">Date / تاريخ
                                                        </th>
                                                        <th style="width: 9%" id="thSCStatus" runat="server">Status
                                                        </th>
                                                        <th style="width: 7%">Action/عمل
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rpt_Item_list" runat="server" OnItemDataBound="rpt_Item_listOnItemDataBound">
                                                        <ItemTemplate>
                                                            <tr style="text-align: center" runat="server" id="trService">
                                                                <td style="text-align: left">
                                                                    <asp:HiddenField ID="hdn_catgory_id" runat="server" Value='<%#Eval("CategoryId") %>' />
                                                                    <asp:HiddenField ID="hdn_service_id" runat="server" Value='<%#Eval("Service_Id") %>' />
                                                                    <asp:Label ID="lbl_service" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label>
                                                                </td>
                                                                <td style="text-align: left">
                                                                    <asp:Label ID="lblparticlr" runat="server" Text='<%#Eval("Particulars") %>'></asp:Label>
                                                                </td>
                                                                <td style="text-align: left" runat="server" id="tdServAmt">
                                                                    <asp:Label ID="lblserAmt" runat="server" Text='<%#Eval("AfterDiscount_Total") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hdnSingleamountIn" runat="server" Value='<%#Eval("AfterDiscount_PriceWitTax") %>' />

                                                                </td>
                                                                <td style="text-align: left">
                                                                    <asp:TextBox ID="txt_InvQty" class="numbers_only read_Only invQty txt asLabel" Width="75%"
                                                                        TabIndex="-1" runat="server" Text='<%#Eval("InvoiceQuantity") %>'></asp:TextBox>
                                                                </td>
                                                                <td style="text-align: left">
                                                                    <asp:TextBox ID="txt_InComQty" class="numbers_only read_Only inComQty inline txt asLabel"
                                                                        TabIndex="-1" Width="75%" runat="server" Text='<%#Eval("InComQuantity") %>'></asp:TextBox>
                                                                </td>
                                                                <td style="text-align: left">
                                                                    <asp:TextBox ID="txtInlineQty" class="numbers_only txt inlineQty" Width="75%" runat="server"
                                                                        Text='<%#Eval("Quantity") %>'></asp:TextBox>
                                                                    <asp:Label ID="lblcomplete" runat="server" Text="Completed" ForeColor="Green" Font-Bold="true"></asp:Label>
                                                                    <asp:RequiredFieldValidator ID="RqtxtQty" runat="server" ControlToValidate="txtInlineQty"
                                                                        ValidationGroup="inlineSave" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                        InitialValue="">
                                                                    </asp:RequiredFieldValidator>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("vendorname") %>
                                                                </td>
                                                                <td style="text-align: left; display: none">
                                                                    <asp:TextBox ID="txtInlineAmtSQty" class="numbers_only read_Only asLabel txt inlineamtSQty"
                                                                        TabIndex="-1" Width="75%" runat="server" Text='<%#Eval("AmtForSingleQty") %>'></asp:TextBox>
                                                                </td>
                                                                <td style="text-align: left">
                                                                    <asp:TextBox ID="txtInlineTotAmt" class="numbers_only read_Only asLabel txt inlinetotAmt"
                                                                        TabIndex="-1" Width="75%" runat="server" Text='<%#Eval("TotalAmount") %>'></asp:TextBox>
                                                                </td>
                                                                <td style="text-align: left">
                                                                    <%#Eval("ExpenseAccount") %>
                                                                </td>
                                                                <td>
                                                                    <telerik:RadDatePicker ID="InlineSerComDate" Width="110px" runat="server" SelectedDate='<%#Eval("SerComDate") %>'
                                                                        DateInput-DateFormat="dd/MM/yyyy">
                                                                        <Calendar runat="server" ID="Calendar1" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                                            ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                                            <SpecialDays>
                                                                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                                </telerik:RadCalendarDay>
                                                                            </SpecialDays>
                                                                        </Calendar>
                                                                    </telerik:RadDatePicker>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="InlineSerComDate"
                                                                        ValidationGroup="inlineSave" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                        InitialValue="">
                                                                    </asp:RequiredFieldValidator>
                                                                </td>
                                                                <td style="text-align: left" id="tdSCStatus" runat="server">
                                                                    <%#Eval("ServiceStatusname") %>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btn_expDetail_line" CssClass="btn_edit" runat="server" OnClick="btn_expDetail_line_OnClick"
                                                                        ToolTip="Edit" />
                                                                    <asp:Button ID="btnInlineSave" CssClass="btn_completeTick" runat="server" OnClick="btnInlineSave_OnClick"
                                                                        ValidationGroup="inlineSave" ToolTip="Complete" />
                                                                    <asp:Button ID="btnInlineNew" runat="server" ToolTip="Add" class="btn_add_new" OnClick="btn_AddtnlExpenseinline_OnClick" />
                                                                    <asp:Button ID="btnServiceCompletionView" CssClass="btn_view" runat="server" OnClick="btnServiceCompletionView_OnClick"
                                                                        ToolTip="View" />
                                                                    <asp:Button ID="btnserviceStatus" CssClass="btn_Statusicon" runat="server" OnClick="btnChangestatusOnClick" ToolTip="Service Status" />
                                                                    <asp:Button ID="btnsetDescpy" Visible='<%# !Convert.ToBoolean(Eval("IsviewD")) %>'
                                                                        CssClass="btn_Dicon" runat="server" OnClick="btnsetDescpyOnClick" ToolTip="Set Discrepancy" />

                                                                    <asp:Button ID="btnclrDescpy" Visible='<%# Convert.ToBoolean(Eval("IsviewD")) %>'
                                                                        CssClass="btn_CDicon" runat="server" OnClick="btnClearDescpyOnClick" ToolTip='<%#Eval("DescrepancyRemark") %>' />
                                                                    <asp:HiddenField ID="hdn_D_id" runat="server" Value='<%#Eval("D_id") %>' />
                                                                    <asp:HiddenField ID="hdnDescrepncy" runat="server" Value='<%#Eval("IsDescrepancy") %>' />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <div style="height: 10px">
                                    </div>
                                </div>
                                <table class="ActionTable">
                                    <tr>
                                        <td colspan="4" rowspan="3" style="text-align: right">
                                            <asp:HiddenField ID="hdn_invId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdn_invStatus" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdn_user_id" runat="server" />
                                            <asp:HiddenField ID="hdnSCPredateDays" runat="server" />
                                            <asp:HiddenField ID="hdnIsDisplaySCStatus" runat="server" />
                                            <asp:HiddenField ID="hdnallowSCExceed" runat="server" />
                                            <asp:HiddenField ID="hdnIsHideServiceAmtInSC" runat="server" />

                                            <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_OnClick" />
                                            <asp:Button ID="Button1" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_OnClick" />
                                            <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdn_update" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdn_delete" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdn_complete" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdnsetdescrepancy" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdncleardescrepancy" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdnServiceStatus" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdn_debitnote" runat="server" Value="0" />
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
                                <div>
                                    <div id="div4" class="messageAlerterror div_poperror animated" style="display: none" runat="server">
                                        <div class="tick">
                                            &#10007
                                        </div>
                                        <div>
                                            <asp:Label ID="lblerrormsg" runat="server" class="messageLabel"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <asp:UpdatePanel ID="Upd_Expense_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnl_Expense_Panel" Visible="false" runat="server">
                                            <table class="listTable">
                                                <thead>
                                                    <tr>
                                                        <th style="width: 10%">Expense / مصروف
                                                        </th>
                                                        <th style="width: 8%">Amount / المبلغ
                                                        </th>
                                                        <th style="width: 6%">VAT / ضريبة
                                                        </th>
                                                        <th style="width: 10%">Vendor Commission
                                                        </th>
                                                        <th style="width: 10%">Vendor / بائع
                                                        </th>
                                                        <th style="width: 10%">Payment Mode / طريقة الدفع
                                                        </th>
                                                        <th style="width: 10%">Account / الحساب
                                                        </th>
                                                        <th style="width: 10%">Payable Amount / المبلغ المستحق
                                                        </th>
                                                        <th style="width: 10%">Paid Amount / المبلغ المدفوع
                                                        </th>
                                                        <th style="width: 3%">Action/عمل
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <asp:Repeater ID="rpt_expense_list" runat="server" OnItemDataBound="rpt_expense_list_OnItemDataBound">
                                                    <ItemTemplate>
                                                        <tr class="temp">
                                                            <td>
                                                                <asp:HiddenField ID="hdnSerComDetailId" runat="server" Value='<%#Eval("SerComDetailId") %>' />
                                                                <asp:HiddenField ID="hdn_expenseId" runat="server" Value='<%#Eval("ExpenseId") %>' />
                                                                <asp:Label ID="lbl_Expense" runat="server" Text='<%# Eval("ExpenseName") %>' />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_amt" Class="txt numbers_only jcalculation amt" runat="server"
                                                                    Text='<%#Eval("Amount") %>'></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_amt"
                                                                    ValidationGroup="save" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                    InitialValue="">
                                                                </asp:RequiredFieldValidator>
                                                            </td>

                                                            <td>
                                                                <asp:TextBox ID="txt_vat" Class="txt numbers_only jcalculation vat" runat="server"
                                                                    Text='<%#Eval("VAT") %>'></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6a" runat="server" ControlToValidate="txt_vat"
                                                                    ValidationGroup="save" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                    InitialValue="">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtVendorCommissionIn" Class="txt numbers_only" runat="server"
                                                                    Text='<%#Eval("VendorCommission") %>'></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:HiddenField ID="hdn_vendorId" runat="server" Value='<%#Eval("VendorId") %>' />
                                                                <telerik:RadComboBox ID="drp_vendor" Sort="Ascending" Filter="Contains" runat="server"
                                                                    AllowCustomText="false" DropDownWidth="200px" RenderMode="Lightweight" EmptyMessage="Search Vendor..."
                                                                    Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                    OnClientBlur="ValidateCombo">
                                                                </telerik:RadComboBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drp_vendor"
                                                                    ValidationGroup="save" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                    InitialValue="">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:HiddenField ID="hdn_payModeId" runat="server" Value='<%#Eval("PayModeId") %>' />
                                                                <telerik:RadComboBox ID="drp_payMode" Sort="Ascending" Filter="Contains" runat="server"
                                                                    AllowCustomText="false" RenderMode="Lightweight" DropDownWidth="200px" EmptyMessage="Search Payment Mode..."
                                                                    Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                    OnClientBlur="ValidateCombo" ClientIDMode="AutoID" OnSelectedIndexChanged="drp_payMode_OnSelectedIndexChanged"
                                                                    AutoPostBack="true">
                                                                </telerik:RadComboBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="drp_payMode"
                                                                    ValidationGroup="save" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                    InitialValue="">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="Upd_Account_Panel" runat="server" ChildrenAsTriggers="false"
                                                                    UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:HiddenField ID="hdn_accountId" runat="server" Value='<%#Eval("AccountId") %>' />
                                                                        <telerik:RadComboBox ID="drp_account" DropDownWidth="200px" Sort="Ascending" Filter="Contains" runat="server"
                                                                            AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Account..."
                                                                            Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                            OnClientBlur="ValidateCombo">
                                                                        </telerik:RadComboBox>
                                                                        <asp:RequiredFieldValidator ID="rqdaccountIn" runat="server" ControlToValidate="drp_account"
                                                                            ValidationGroup="save" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                            InitialValue="">
                                                                        </asp:RequiredFieldValidator>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_payableAmount" Class="txt numbers_only read_Only payableAmount"
                                                                    runat="server" Text='<%#Eval("PayableAmount") %>'></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="updpaidAmountIn" runat="server" ChildrenAsTriggers="false"
                                                                    UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txt_paidAmount" Class="txt numbers_only paidAmount" runat="server"
                                                                            Text='<%#Eval("PaidAmount") %>'></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldVtor4" runat="server" ControlToValidate="txt_paidAmount"
                                                                            ValidationGroup="save" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                            InitialValue="">
                                                                        </asp:RequiredFieldValidator>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnInlineEdit" runat="server" OnClick="btnInlineEdit_OnClick" ToolTip="Edit"
                                                                    class="btn_edit" />
                                                                <asp:Button ID="btnInlineDelete" class="btn_delete" runat="server" ToolTip="Delete"
                                                                    OnClick="btnInlineDelete_OnClick" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <tr class="temp">
                                                    <td>
                                                        <telerik:RadComboBox ID="drpNewExpense" Sort="Ascending" Filter="Contains" runat="server"
                                                            AllowCustomText="false" RenderMode="Lightweight" DropDownWidth="200px" EmptyMessage="Search Expense..."
                                                            Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                            OnClientBlur="ValidateCombo">
                                                        </telerik:RadComboBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="drpNewExpense"
                                                            ValidationGroup="inlineadd" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                            InitialValue=""></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNewAmt" Class="txt numbers_only jcalculationout amtout" runat="server"
                                                            Text=""></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtNewAmt"
                                                            ValidationGroup="inlineadd" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                            InitialValue=""></asp:RequiredFieldValidator>
                                                    </td>

                                                    <td>
                                                        <asp:TextBox ID="txtNewVat" Class="txt numbers_only jcalculationout vatout" runat="server"
                                                            Text=""></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtNewVat"
                                                            ValidationGroup="inlineadd" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                            InitialValue=""></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtVendorCommissionOut" Class="txt numbers_only jcalculationout amtout" runat="server"
                                                            Text=""></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ControlToValidate="txtNewAmt"
                                                            ValidationGroup="inlineadd" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                            InitialValue=""></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <telerik:RadComboBox ID="drpNewVendor" Sort="Ascending" Filter="Contains" runat="server"
                                                            AllowCustomText="false" RenderMode="Lightweight" DropDownWidth="200px" EmptyMessage="Search Vendor..."
                                                            Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                            OnClientBlur="ValidateCombo">
                                                        </telerik:RadComboBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="drpNewVendor"
                                                            ValidationGroup="inlineadd" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                            InitialValue=""></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <telerik:RadComboBox ID="drpNewPayMode" Sort="Ascending" Filter="Contains" runat="server"
                                                            AllowCustomText="false" RenderMode="Lightweight" DropDownWidth="200px" EmptyMessage="Search Payment Mode..."
                                                            Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                            OnClientBlur="ValidateCombo" ClientIDMode="AutoID" OnSelectedIndexChanged="drpNewPayMode_OnSelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </telerik:RadComboBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="drpNewPayMode"
                                                            ValidationGroup="inlineadd" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                            InitialValue="">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdNewAccountPanel" runat="server" ChildrenAsTriggers="false"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <telerik:RadComboBox ID="drpNewAccount" Sort="Ascending" Filter="Contains" runat="server"
                                                                    AllowCustomText="false" RenderMode="Lightweight" DropDownWidth="200px" EmptyMessage="Search Account..."
                                                                    Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                    OnClientBlur="ValidateCombo">
                                                                </telerik:RadComboBox>
                                                                <asp:RequiredFieldValidator ID="rqdaccOut" runat="server" ControlToValidate="drpNewAccount"
                                                                    ValidationGroup="inlineadd" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                    InitialValue="">
                                                                </asp:RequiredFieldValidator>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNewPayableAmount" Class="txt numbers_only read_Only payableAmountout"
                                                            runat="server" Text=""></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="updpaidAmountOut" runat="server" ChildrenAsTriggers="false"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtNewPaidAmount" Class="txt numbers_only paidAmountout" runat="server"
                                                                    Text=""></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtNewPaidAmount"
                                                                    ValidationGroup="inlineadd" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                    InitialValue="">
                                                                </asp:RequiredFieldValidator>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnInlineNew" runat="server" OnClick="btnInlineNew_OnClick" ToolTip="Add"
                                                            class="btn_add_new" ValidationGroup="inlineadd" />
                                                        <asp:HiddenField ID="hdnNewIndexId" runat="server" Value="" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="formTable">
                                                <tr>
                                                    <td style="width: 25%">Quantity / كمية <span style="color: Red">&nbsp*</span>
                                                    </td>
                                                    <td style="width: 25%">
                                                        <asp:HiddenField ID="hdn_InComQty" runat="server" Value="0" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hdn_InvDetailId" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hdnSingleamount" runat="server" Value="0" />

                                                        <asp:TextBox ID="txt_Qty" class="numbers_only Qty" Width="75%" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txt_Qty"
                                                            ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                            InitialValue="">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td style="width: 25%"></td>
                                                    <td style="width: 25%"></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">Date / تاريخ<span style="color: Red">&nbsp*</span>
                                                    </td>
                                                    <td style="width: 25%">
                                                        <telerik:RadDatePicker ID="SerComDate" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                            <Calendar runat="server" ID="Calendar1" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                                ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                                <SpecialDays>
                                                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                    </telerik:RadCalendarDay>
                                                                </SpecialDays>
                                                            </Calendar>
                                                        </telerik:RadDatePicker>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="SerComDate"
                                                            ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                            InitialValue="">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td style="width: 25%"></td>
                                                    <td style="width: 25%"></td>
                                                </tr>
                                                <tr>
                                                    <td>Amount For Single Qty / المبلغ للكمية الواحدة
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_amtSQty" class="numbers_only read_Only amtSQty" Width="75%"
                                                            runat="server"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2"></td>
                                                </tr>
                                                <tr>
                                                    <td>Total Amount / المبلغ الإجمالي
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_totAmt" class="numbers_only read_Only totAmt" Width="75%" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2"></td>
                                                </tr>
                                                <tr>
                                                    <td>Remarks / ملاحظات
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox class="txtarea" Style="width: 90%" TextMode="MultiLine" ID="txtscremark"
                                                            runat="server"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                            <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                            <asp:Button ID="btn_save" class="butn_save" ValidationGroup="save" OnClick="btn_save_OnClick"
                                                runat="server" Text="Save/حفظ" />
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                <asp:UpdatePanel ID="updAddtnlExpense" runat="server" ChildrenAsTriggers="false"
                                    UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlAddtnlExpense" Visible="false" runat="server">
                                            <table class="listTable">
                                                <thead>
                                                    <tr>
                                                        <th style="width: 200px">Expense / مصروف
                                                        </th>
                                                        <th style="width: 100px">Amount / المبلغ
                                                        </th>
                                                        <th style="width: 100px">VAT / ضريبة
                                                        </th>
                                                        <th>Vendor / بائع
                                                        </th>
                                                        <th>Payment Mode / طريقة الدفع
                                                        </th>
                                                        <th>Account / الحساب
                                                        </th>
                                                        <th style="width: 100px">Payable Amount / المبلغ المستحق
                                                        </th>
                                                        <th style="width: 100px">Paid Amount / المبلغ المدفوع
                                                        </th>
                                                        <th>Action/عمل
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <asp:Repeater ID="rptAddtnlExpense" runat="server" OnItemDataBound="rptAddtnlExpenseOnItemDataBound">
                                                    <ItemTemplate>
                                                        <tr class="temp">
                                                            <td>
                                                                <asp:HiddenField ID="hdnSerComDetailId_AE" runat="server" Value='<%#Eval("SerComDetailId") %>' />
                                                                <asp:HiddenField ID="hdn_expenseId_AE" runat="server" Value='<%#Eval("ExpenseId") %>' />
                                                                <asp:Label ID="lbl_Expense_AE" runat="server" Text='<%# Eval("ExpenseName") %>' />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_amt_AE" Class="txt numbers_only jcalculationAddtnl amtAddtnl" runat="server"
                                                                    Text='<%#Eval("Amount") %>'></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5_AE" runat="server" ControlToValidate="txt_amt_AE"
                                                                    ValidationGroup="save" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                    InitialValue="">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_vat_AE" Class="txt numbers_only jcalculationAddtnl vatAddtnl" runat="server"
                                                                    Text='<%#Eval("VAT") %>'></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6a_AE" runat="server" ControlToValidate="txt_vat_AE"
                                                                    ValidationGroup="save" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                    InitialValue="">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:HiddenField ID="hdn_vendorId_AE" runat="server" Value='<%#Eval("VendorId") %>' />
                                                                <telerik:RadComboBox ID="drp_vendor_AE" Sort="Ascending" Filter="Contains" runat="server"
                                                                    AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Vendor..."
                                                                    Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                    OnClientBlur="ValidateCombo">
                                                                </telerik:RadComboBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1_AE" runat="server" ControlToValidate="drp_vendor_AE"
                                                                    ValidationGroup="save" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                    InitialValue="">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:HiddenField ID="hdn_payModeId_AE" runat="server" Value='<%#Eval("PayModeId") %>' />
                                                                <telerik:RadComboBox ID="drp_payMode_AE" Sort="Ascending" Filter="Contains" runat="server"
                                                                    AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Payment Mode..."
                                                                    Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                    OnClientBlur="ValidateCombo" ClientIDMode="AutoID" OnSelectedIndexChanged="drp_payMode_OnSelectedIndexChanged"
                                                                    AutoPostBack="true">
                                                                </telerik:RadComboBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6_AE" runat="server" ControlToValidate="drp_payMode_AE"
                                                                    ValidationGroup="save" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                    InitialValue="">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="Upd_Account_Panel" runat="server" ChildrenAsTriggers="false"
                                                                    UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:HiddenField ID="hdn_accountId_AE" runat="server" Value='<%#Eval("AccountId") %>' />
                                                                        <telerik:RadComboBox ID="drp_account_AE" Sort="Ascending" Filter="Contains" runat="server"
                                                                            AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Account..."
                                                                            Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                            OnClientBlur="ValidateCombo">
                                                                        </telerik:RadComboBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2_AE" runat="server" ControlToValidate="drp_account_AE"
                                                                            ValidationGroup="save" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                            InitialValue="">
                                                                        </asp:RequiredFieldValidator>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_payableAmount_AE" Class="txt numbers_only payableAmountAddtnl"
                                                                    runat="server" Text='<%#Eval("PayableAmount") %>'></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_paidAmount_AE" Class="txt numbers_only paidAmountAddtnl" runat="server"
                                                                    Text='<%#Eval("PaidAmount") %>'></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4_AE" runat="server" ControlToValidate="txt_paidAmount_AE"
                                                                    ValidationGroup="save" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                    InitialValue="">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnInlineEdit" runat="server" OnClick="btnInlineEditAddtnl_OnClick" ToolTip="Edit"
                                                                    class="btn_edit" />
                                                                <asp:Button ID="btnInlineDelete" class="btn_delete" runat="server" ToolTip="Delete"
                                                                    OnClick="btnInlineDeleteAddtnl_OnClick" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <tr class="temp">
                                                    <td>
                                                        <telerik:RadComboBox ID="drpAddExpense" Sort="Ascending" Filter="Contains" runat="server"
                                                            AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Expense..."
                                                            Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                            OnClientBlur="ValidateCombo">
                                                        </telerik:RadComboBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="drpAddExpense"
                                                            ValidationGroup="inlineaddtnl" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                            InitialValue=""></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAddAmt" Class="txt numbers_only jcalculationAddtnl amtAddtnl" runat="server"
                                                            Text=""></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtAddAmt"
                                                            ValidationGroup="inlineaddtnl" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                            InitialValue=""></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAddVat" Class="txt numbers_only jcalculationAddtnl vatAddtnl" runat="server"
                                                            Text=""></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtAddVat"
                                                            ValidationGroup="inlineaddtnl" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                            InitialValue=""></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <telerik:RadComboBox ID="drpAddVendor" Sort="Ascending" Filter="Contains" runat="server"
                                                            AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Vendor..."
                                                            Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                            OnClientBlur="ValidateCombo">
                                                        </telerik:RadComboBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="drpAddVendor"
                                                            ValidationGroup="inlineaddtnl" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                            InitialValue=""></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <telerik:RadComboBox ID="drpAddPayMode" Sort="Ascending" Filter="Contains" runat="server"
                                                            AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Payment Mode..."
                                                            Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                            OnClientBlur="ValidateCombo" ClientIDMode="AutoID" OnSelectedIndexChanged="drpAddPayMode_OnSelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </telerik:RadComboBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="drpAddPayMode"
                                                            ValidationGroup="inlineaddtnl" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                            InitialValue="">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdAddAccountPanel" runat="server" ChildrenAsTriggers="false"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <telerik:RadComboBox ID="drpAddAccount" Sort="Ascending" Filter="Contains" runat="server"
                                                                    AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Account..."
                                                                    Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                    OnClientBlur="ValidateCombo">
                                                                </telerik:RadComboBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="drpAddAccount"
                                                                    ValidationGroup="inlineaddtnl" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                    InitialValue="">
                                                                </asp:RequiredFieldValidator>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAddPayableAmount" Class="txt numbers_only read_Only payableAmountAddtnl"
                                                            runat="server" Text=""></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="txtAddPayableAmount"
                                                            ValidationGroup="inlineaddtnl" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                            InitialValue="">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAddPaidAmount" Class="txt numbers_only paidAmountAddtnl" runat="server"
                                                            Text=""></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="txtAddPaidAmount"
                                                            ValidationGroup="inlineaddtnl" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                            InitialValue="">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="Button3" runat="server" OnClick="btnInlineNewAddtnl_OnClick" ToolTip="Add"
                                                            class="btn_add_new" ValidationGroup="inlineaddtnl" />
                                                        <asp:HiddenField ID="hdnAddIndexId" runat="server" Value="" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="formTable">
                                                <tr>
                                                    <td style="width: 25%">Date / تاريخ<span style="color: Red">&nbsp*</span>
                                                    </td>
                                                    <td style="width: 25%">
                                                        <asp:HiddenField ID="hdn_InvDetailIdAddtnl" runat="server" Value="0" ClientIDMode="Static" />

                                                        <telerik:RadDatePicker ID="SerComDateAdd" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                            <Calendar runat="server" ID="Calendar2" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                                ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                                <SpecialDays>
                                                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                    </telerik:RadCalendarDay>
                                                                </SpecialDays>
                                                            </Calendar>
                                                        </telerik:RadDatePicker>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="SerComDateAdd"
                                                            ValidationGroup="saveAdd" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                            InitialValue="">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td style="width: 25%"></td>
                                                    <td style="width: 25%"></td>
                                                </tr>
                                                <tr>
                                                    <td>Total Amount / المبلغ الإجمالي
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAddTotal" class="numbers_only read_only totAmtAddtnl" Width="75%" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="txtAddTotal"
                                                            ValidationGroup="saveAdd" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                            InitialValue="">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td colspan="2"></td>
                                                </tr>
                                                <tr>
                                                    <td>Remarks / ملاحظات
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox class="txtarea" Style="width: 90%" TextMode="MultiLine" ID="txtRemarkSC"
                                                            runat="server"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                            <asp:HiddenField ID="HiddenField4" runat="server" Value="0" />
                                            <asp:Button ID="Button4" class="butn_save" ValidationGroup="saveAdd" OnClick="btn_saveAddtnl_OnClick"
                                                runat="server" Text="Save/حفظ" />
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <asp:UpdatePanel ID="UpdInvoiceDetId" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnInvoiceDetId" runat="server" Value="0" />
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </asp:Panel>
                <asp:UpdatePanel ID="updmsg" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
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
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="updServiceStatus" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlServiceStatus" Visible="false" runat="server">
                            <div class="popupBackground">
                            </div>
                            <div class="animated smallPopUp">
                                <div class="Adding_heading">
                                    Change Status
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td>Status <span style="color: Red">&nbsp*</span>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadComboBox ID="drpchangestatus" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                OnClientBlur="ValidateCombo" EmptyMessage="Search Status..." Style="overflow: hidden; width: 86%; border: none!important; padding-right: 5px; margin-top: 0px">
                                               
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="drpchangestatus"
                                                ValidationGroup="saveCS" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue="">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:Button ID="btnChangestatussave" runat="server" class="butn_save" ValidationGroup="saveCS"
                                                    Text="Save/حفظ" OnClick="btnChangestatussaveOnClick" />
                                                <asp:Button ID="Button6" class="butn" runat="server" Text="Close/إغلاق"
                                                    OnClick="btnChangestatuscloseOnClick" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdDiscrepancy" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlDiscrepancy" Visible="false" runat="server">
                            <div class="popupBackground">
                            </div>
                            <div class="animated smallPopUp">
                                <div class="Adding_heading">
                                    <asp:Label ID="lbldes" runat="server" Text="Set Descrepancy"></asp:Label>
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td>Remarks / ملاحظات <span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txtDiscrepancyremark" class="txtarea" TextMode="MultiLine" Width="85%"
                                                runat="server" Text=""></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="txtDiscrepancyremark"
                                                ValidationGroup="Discrepancysave" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue="">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:Button ID="btnDiscrepancysave" runat="server" class="butn_save" ValidationGroup="txtDiscrepancyremark"
                                                    Text="Save/حفظ" OnClick="btnDiscrepancysaveOnClick" OnClientClick="if (Page_ClientValidate('Discrepancysave') == false) return(false);else return confirm('Do you really want to Save.. ?');" />
                                                <asp:Button ID="btnclrDiscrepancysave" runat="server" class="butn_save" ValidationGroup="txtDiscrepancyremark"
                                                    Text="Save/حفظ" OnClick="btnClearDiscrepancysaveOnClick" OnClientClick="if (Page_ClientValidate('Discrepancysave') == false) return(false);else return confirm('Do you really want to Save.. ?');" />

                                                <asp:Button ID="btnDiscrepancyclose" class="butn" runat="server" Text="Close/إغلاق"
                                                    OnClick="btnDiscrepancycloseOnClick" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="Upd_TransaDetail_Panel" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnl_transaDetail" Visible="false" runat="server">
                            <div class="popupBackground">
                            </div>
                            <div class="animated smallPopUp">
                                <div class="Adding_heading">
                                    Transaction Detail / تفاصيل الصفقة
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td>
                                            <div id="div2" runat="server" style="width: 100%; overflow: auto;">
                                                <div style="height: 10px">
                                                </div>
                                                <table class="listTable">
                                                    <thead>
                                                        <tr style="text-align: center">
                                                            <th style="width: 3%">Sl.No/رقم
                                                            </th>
                                                            <th style="width: 10%">Transaction Number / رقم التحويلة
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater ID="rpt_TransacDetail" runat="server">
                                                            <ItemTemplate>
                                                                <tr style="text-align: center">
                                                                    <td>
                                                                        <%# Container.ItemIndex + 1 %>
                                                                    </td>
                                                                    <td style="text-align: left">
                                                                        <asp:TextBox ID="txt_transNumber" class="txt" Width="75%" runat="server" Text='<%#Eval("TransactionNumber") %>'></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txt_transNumber"
                                                                            ValidationGroup="finalsave" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                            InitialValue=""></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tbody>
                                                </table>
                                                <div style="height: 10px">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="Updfu_SCFiles" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table class="listTable">
                                                        <tr>
                                                            <td>
                                                                <telerik:RadAsyncUpload ID="fu_SCFiles" Width="80%" MaxFileSize="500000000"
                                                                    OnFileUploaded="fu_SCFilesOnFileUploaded" runat="server">
                                                                </telerik:RadAsyncUpload>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnadddEdit" runat="server" class="btn_add_new" OnClick="btnadddEdit_Click" />
                                                                <asp:HiddenField ID="hdnfilename" runat="server" />
                                                                <asp:HiddenField ID="hdnfilenamesave" runat="server" />

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2"><b>Files</b></td>
                                                        </tr>
                                                        <asp:Repeater ID="rptfileupl" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:Label ID="lblfileupl" Text='<%#Eval("FileNames") %>' runat="server"></asp:Label>
                                                                        <asp:Label ID="lblfilesaveupl" Text='<%#Eval("FileSaveNames") %>' Visible="false" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:Button ID="btn_FinalSave" runat="server" class="butn_save" ValidationGroup="finalsave"
                                                    Text="Save/حفظ" OnClick="btn_FinalSave_OnClick" OnClientClick="javascript : return confirm('Do you really want to Save.. ?');" />
                                                <asp:Button ID="btn_TransDetail_Close" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_TransDetail_Close_OnClick" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdServiceCompletionView" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="rptfiledown" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Panel ID="pnlServiceCompletionView" Visible="false" runat="server">
                            <div class="popupBackground">
                            </div>
                            <div class="animated halfPopUp">
                                <div class="Adding_heading">
                                    Service Completion Batch / دفعة إكمال الخدمة
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td>
                                            <div id="div3" runat="server" style="width: 100%; overflow: auto;">
                                                <div style="height: 10px">
                                                </div>
                                                <table class="listTable">
                                                    <thead>
                                                        <tr style="text-align: center">
                                                            <th style="width: 6%">Ref no</th>
                                                            <th style="width: 6%">Completed Quantity /خدمة اكتمال الكمية
                                                            </th>
                                                            <th style="width: 11%">Amount For Single Qty / المبلغ للكمية الواحدة
                                                            </th>
                                                            <th style="width: 12%">Account / الحساب
                                                            </th>
                                                            <th style="width: 8%">Total Amount / المبلغ الإجمالي
                                                            </th>
                                                            <th style="width: 7%">Date / تاريخ الإنشاء
                                                            </th>
                                                            <th style="width: 3%">Action /عمل
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater ID="rptServiceCompletionEdit" runat="server" OnItemDataBound="rptServiceCompletionEdit_OnItemDataBound">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <%#Eval("Code")%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:HiddenField ID="hdnSerCompletionId" runat="server" Value='<%#Eval("Id") %>' />
                                                                        <asp:HiddenField ID="hdnStatus" runat="server" Value='<%#Eval("Status") %>' />
                                                                        <%#Eval("Quantity")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("AmtForSingleQty")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("Account")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("TotalAmount")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("CreatedDate")%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnServiceCompletionEdit" CssClass="btn_edit" runat="server" OnClick="btnServiceCompletionEdit_OnClick"
                                                                            ToolTip="Edit" />
                                                                        <asp:Button ID="btnServiceCompletionDelete" CssClass="btn_delete" runat="server"
                                                                            OnClick="btnServiceCompletionDelete_OnClick" ToolTip="Delete" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');" />
                                                                        <asp:Button ID="btnDebitNote" CssClass="btn_debitnote" runat="server" OnClick="btnDebitNote_Click"
                                                                            ToolTip="DebitNote" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tbody>
                                                </table>
                                                <div style="height: 10px">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlfiledwn" Visible="false" runat="server">
                                                <div style="height: 10px">
                                                </div>
                                                <table class="listTable">
                                                    <thead>
                                                        <tr style="text-align: center">
                                                            <th style="width: 28%">File
                                                            </th>
                                                            <th style="width: 8%">Action
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater ID="rptfiledown" runat="server" OnItemCommand="rptfileOnItemCommand">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblfile" runat="server" Text='<%#Eval("FileNames") %>'></asp:Label>
                                                                        <asp:HiddenField ID="hdnfilesave" runat="server" Value='<%#Eval("FileSAveNames") %>' />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnDownload" runat="server" class="btn_doc_down" ToolTip="Download File"
                                                                            CommandName="Download" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="pnlDescHistory" visible="false" runat="server">
                                                <div style="height: 10px">
                                                </div>
                                                Descrepancy History
                                                        <table class="listTable">
                                                            <thead>
                                                                <tr>
                                                                    <th style="text-align: center; width: 30%">Remark/تعليق
                                                                    </th>
                                                                    <th style="text-align: center; width: 10%">Done By/تم بواسطة
                                                                    </th>
                                                                    <th style="text-align: center; width: 10%">Date/تاريخ
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <asp:Repeater ID="rptDescHis" runat="server">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td>
                                                                            <%#Eval("Remarks")%>
                                                                        </td>
                                                                        <td>
                                                                            <%#Eval("DoneBy")%>
                                                                        </td>
                                                                        <td>
                                                                            <%#Eval("Dates")%>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:Button ID="btnViewClose" class="butn" runat="server" Text="Close/أغلق" OnClick="btnViewClose_OnClick" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="UpdDebitNote" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">

                    <ContentTemplate>
                        <asp:Panel ID="pnlDebitNote" Visible="false" runat="server">
                            <div class="popupBackground">
                            </div>
                            <div class="animated largePopUp" >
                                <div class="Adding_headingLargepopup">
                                    Debit Note
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td style="width: 33%">Code 
                                            <asp:HiddenField ID="hdnDebitNoteId" runat="server" />
                                            <asp:TextBox ID="txtDNCode" runat="server" class="txt read_Only" Font-Bold="true"
                                                Text=""></asp:TextBox>

                                        </td>
                                        <td style="width: 33%">Date / تاريخ <span style="color: Red">&nbsp*</span>
                                            <br />
                                            <telerik:RadDatePicker ID="dn_date" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                <Calendar runat="server" ID="Calendar3" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                    ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                    <SpecialDays>
                                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                        </telerik:RadCalendarDay>
                                                    </SpecialDays>
                                                </Calendar>
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator ID="req_on_date" runat="server" ControlToValidate="dn_date"
                                                ValidationGroup="savedb" ErrorMessage="Required" Style="color: Red" InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        
                                        <td style="width: 33%">SC No 
                                            <asp:TextBox ID="txtdnscno" runat="server" class="txt" Font-Bold="true" ReadOnly="true"
                                                Text=""></asp:TextBox>
                                            <asp:HiddenField ID="hdndnscid" runat="server" />

                                        </td>
                                         </tr>
                                    <tr>
                                        <td style="width: 33%">Qty
                                            <asp:TextBox ID="txtdnqty" runat="server" class="txt numbers_only" ReadOnly="true" Font-Bold="true"
                                                Text=""></asp:TextBox>

                                        </td>
                                   
                                        <td style="width: 33%">Invoice No
                                                        <asp:TextBox ID="txtDNInvNo" runat="server" class="txt" Font-Bold="true"
                                                           ReadOnly="true" Text=""></asp:TextBox>
                                            <asp:HiddenField ID="hdndninvid" runat="server" />
                                        </td>
                                        <td>Service Name
                                                        <asp:TextBox ID="txtDNServiceName" runat="server" class="txt" Font-Bold="true"
                                                         ReadOnly="true"   Text=""></asp:TextBox>
                                        </td>
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
                                                                            <asp:TextBox ID="txt_dnpaidAmount" Width="90%" Class="txt numbers_only dnpaidAmount" runat="server"
                                                                               ></asp:TextBox>
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
                                                                            <asp:TextBox ID="txtdnreceivedamt" Width="90%" Class="txt numbers_only txtdnreceivedamt" runat="server"
                                                                              ></asp:TextBox>
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
                                        <td>
                                            <div>
                                                <asp:Button ID="btnDebitNoteSave" class="butn_save" ValidationGroup="savedb" OnClick="btnDebitNoteSave_Click"
                                                    runat="server" Text="Save/حفظ" />
                                                <asp:Button ID="btndnclose" class="butn" runat="server" Text="Close/أغلق" OnClick="btndnclose_Click" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
