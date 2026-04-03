<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="CustomerInvoice.aspx.cs" Inherits="AmarCentre.Transactions.CustomerInvoice" %>

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

            /*CheckBox*/
            $('.supchkitem').click(function () {
                Calc();
               // Sup_Calculate();
            });

            function Calc() {
                var ILTotAmt = 0;
                var GrndTotAmt = 0;
                var PresentTot = 0;

                var DiscTotAmt = 0;
                var totQty = 0;
                var Presentqty = 0;
                var PresentDis = 0;
                var GrndDis = 0;
                var isdispdisc = 1;

//                if ($('#hdn_shwdiscount').val() != '') {
//                    isdispdisc = parseInt($('#hdn_shwdiscount').val());
//                }

                $('.invtot').each(function () {
                    var Amt = 0;
                    var Dis = 0;
                    if ($(this).closest('tr').find(':checkbox').prop('checked')) {

                        if ($(this).closest("tr").find('.invtot').val() != '') {
                            Amt = parseFloat($(this).closest("tr").find('.invtot').val());
                        }

                        if (isdispdisc = "1") {
                            if ($(this).closest("tr").find('.InvDdiscount').val() != '') {
                                Dis = parseFloat($(this).closest("tr").find('.InvDdiscount').val());
                            }
                            if ($(this).closest("tr").find('.InvDQty').val() != '') {
                                totQty = parseFloat($(this).closest("tr").find('.InvDQty').val());
                            }
                        }

                        ILTotAmt = parseFloat(ILTotAmt) + parseFloat(Amt);
                        DiscTotAmt = parseFloat(DiscTotAmt) + (parseFloat(Dis) * parseFloat(totQty));
                    }
                });
//                if ($('.il_tot_amt').val() != '') {
//                    PresentTot = parseFloat($('.il_tot_amt').val());
//                }

//                if (isdispdisc = "1") {
//                    if ($('.discount').val() != '') {
//                        PresentDis = parseFloat($('.discount').val());
//                    }
//                    if ($('.qty').val() != '') {
//                        Presentqty = parseFloat($('.qty').val());
//                    }
//                }

                GrndTotAmt = parseFloat(ILTotAmt) + parseFloat(PresentTot);
                GrndDis = parseFloat(DiscTotAmt) + (parseFloat(PresentDis) * parseFloat(Presentqty));

                /*Amount Round Value */
                var substr = GrndTotAmt.toString().split('.');
                var AmtAfterDecimal = (parseFloat(GrndTotAmt) - parseFloat(substr[0])).toFixed(2);
                var AmtBeforeDecimal = (parseFloat(GrndTotAmt) - parseFloat(AmtAfterDecimal)).toFixed(2);
                var AmtDecimal = 0.00;
                var Final = 0;
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
                $('.tot_discount').val(parseFloat(GrndDis).toFixed(2));

                /*End of Amount Round Value*/
            }
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
       Customer Invoice 
           <asp:Button ID="btn_filter" runat="server" class="filter right_align_list"  OnClick="btn_filter_OnClick" />
        <asp:Button ID="btn_addnew" runat="server" Text="+" class="btnAddNew" OnClick="btn_newentry_OnClick" />
      
        <div class="searchDiv">
            <asp:TextBox ID="txt_search" runat="server" class="txt_search" AutoPostBack="true"
                OnTextChanged="txt_search_OnTextChanged" placeholder="Search"></asp:TextBox>
        </div>
    </div>
    <asp:UpdatePanel ID="upd_nav_filter" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnl_filter" Visible="false" runat="server">
                <div class="animated smallPopUp">
                    <div class="Adding_heading">
                        Search
                    </div>
                    <table class="formTable">
                        <tr>
                            <td>
                                 From Date
                                <br />
                                <telerik:RadDatePicker ID="radfilterdate" runat="server" class="input-boder" DateInput-DateFormat="dd/MM/yyyy">
                                    <Calendar runat="server" ID="Calendar4" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                        ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                        <SpecialDays>
                                            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                            </telerik:RadCalendarDay>
                                        </SpecialDays>
                                    </Calendar>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                 To Date
                                <br />
                                <telerik:RadDatePicker ID="radtodate" runat="server" class="input-boder" DateInput-DateFormat="dd/MM/yyyy">
                                    <Calendar runat="server" ID="Calendar5" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                        ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                        <SpecialDays>
                                            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                            </telerik:RadCalendarDay>
                                        </SpecialDays>
                                    </Calendar>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Customer
                                <telerik:RadComboBox ID="drp_Cust" Sort="Ascending" Filter="Contains" runat="server"
                                    AllowCustomText="true" RenderMode="Lightweight"
                                    EmptyMessage="Search Customer..." OnClientFocus="OnClientKeyPressing" 
                                    Style="overflow: hidden; width: 96%; border: none!important;">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                Agent
                                <telerik:RadComboBox ID="drpagent" Sort="Ascending" Filter="Contains" runat="server"
                                    AllowCustomText="true"  RenderMode="Lightweight"
                                    EmptyMessage="Search Agent..." OnClientFocus="OnClientKeyPressing" 
                                    Style="overflow: hidden; width: 96%; border: none!important;">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                       
                        <tr>
                            <td>
                                <asp:Button ID="btn_search"  class="butn" runat="server" OnClick="btn_search_Click"
                                    Text="Search" />
                                <asp:Button ID="btnexcel_export" runat="server" class="butn"
            Text="Generate Excel" OnClick="btnexcel_export_OnClick" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
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
                                Sl No /رقم
                            </th>
                            <th style="width: 10%;">
                                Code / رمز
                            </th>
                            <th style="width: 15%;">
                                Customer / زبون
                            </th>
                            <th style="width: 10%;">
                                Date / تاريخ
                            </th>
                            <th style="width: 10%;">
                                Amount / المبلغ
                            </th>
                             <th style="width: 5%;">
                                Status 
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
                                        <%#Eval("Name")%>
                                    </td>
                                    <td>
                                        <%#Eval("Dateds")%>
                                    </td>
                                    <td>
                                        <%#Eval("Grand_Total")%>
                                    </td>
                                     <td>
                                        <%#Eval("Statusname")%>
                                    </td>
                                    <td class="listTableActionButtonDiv">
                                        <asp:HiddenField ID="hdnIsCredit" runat="server" Value='<%#Eval("IsCredit")%>' />
                                        <asp:HiddenField ID="hdnReceived" runat="server" Value='<%#Eval("Received")%>' />
                                        <asp:HiddenField ID="hdnAfterDiscountGrandTotal" runat="server" Value='<%#Eval("AfterDiscount_GrandTotal")%>' />
                                        <asp:Button ID="btn_edit" runat="server" class="btn_edit" ToolTip="Edit" CommandName="Edit" />
                                        <asp:Button ID="btnTaxInvoicePrint" runat="server" class="btn_print" ToolTip="Print"
                                            CommandName="Print" />
                                      
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td colspan="7" class="navigationRow">
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
                        <asp:UpdatePanel ID="upd_main" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div id="div_main" runat="server">
                                    <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
                                        UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="Adding_heading">
                                               Customer Invoice 
                                            </div>
                                            <table class="formTable">
                                                <tr>
                                                    <td style="width: 33%">
                                                         Code / رمز
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
                                                    <td>
                                                    
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Customer Name / اسم الزبون <span style="color: Red">&nbsp*</span>
                                                        <asp:UpdatePanel ID="Upd_CustomerDrop_Panel" runat="server" ChildrenAsTriggers="false"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <telerik:RadComboBox ID="drp_customer" Sort="Ascending" Filter="Contains" runat="server"
                                                                    AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                                    AutoPostBack="true" OnClientBlur="ValidateCombo" EmptyMessage="Search Name..."
                                                                    OnSelectedIndexChanged="drp_customer_OnSelectedIndexChanged" Style="overflow: hidden;
                                                                    width: 96%; border: none!important;">
                                                                </telerik:RadComboBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="drp_customer"
                                                                    ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                                    InitialValue=""></asp:RequiredFieldValidator>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                     <td >
                                                       From Date  
                                                        <br />
                                                        <telerik:RadDatePicker ID="frmdate" AutoPostBack="true" OnSelectedDateChanged="drp_customer_OnSelectedIndexChanged" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                            <Calendar runat="server" ID="Calendar2" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                                ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                                <SpecialDays>
                                                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                    </telerik:RadCalendarDay>
                                                                </SpecialDays>
                                                            </Calendar>
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                    <td >
                                                       To Date 
                                                        <br />
                                                        <telerik:RadDatePicker ID="todate"  AutoPostBack="true" OnSelectedDateChanged="drp_customer_OnSelectedIndexChanged" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                            <Calendar runat="server" ID="Calendar3" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                                ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                                <SpecialDays>
                                                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                    </telerik:RadCalendarDay>
                                                                </SpecialDays>
                                                            </Calendar>
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                </tr>
                                               
                                                <tr>
                                                 <td>
                                                        Invoice / فاتورة <br />
                                                        <asp:UpdatePanel ID="UpdInvoicePanel" runat="server" ChildrenAsTriggers="false"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <telerik:RadComboBox ID="drp_Invoice" Sort="Ascending" Filter="Contains" runat="server"
                                                                    AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                                    EnableCheckAllItemsCheckBox="true" CheckBoxes="true" EmptyMessage="Search Invoice.."
                                                                    Style="overflow: hidden;width: 96%; border: none!important;" >
                                                                </telerik:RadComboBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>

                                                     <td>
                                                        Service  <br />
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="false"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <telerik:RadComboBox ID="drpService" Sort="Ascending" Filter="Contains" runat="server"
                                                                    AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                                     EmptyMessage="Search Service.."
                                                                    Style="overflow: hidden;width: 96%; border: none!important;" >
                                                                </telerik:RadComboBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td> <asp:Button ID="Button5" class="butn" runat="server"  Text="Search"
                                                                  OnClick="btn_SearchInvoice_OnClick"  /></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <div id="div_item_new" runat="server" style="width: 100%; overflow: auto;">
                                                            <div style="height: 10px">
                                                            </div>
                                                            <asp:UpdatePanel ID="Upd_Item_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table class="listTable">
                                                                        <thead>
                                                                            <tr style="text-align: center">
                                                                                <th style="width: 3%">
                                                                                 <asp:CheckBox ID="chkSelall" runat="server" class="supchkitemall" AutoPostBack="true" OnCheckedChanged="chkboxall_checked" />
                                                                                </th>
                                                                                 <th style="width: 5%" >
                                                                                    Invoice / فاتورة
                                                                                </th>
                                                                                <th style="width: 23%" colspan="2">
                                                                                    Service / الخدمات
                                                                                </th>
                                                                                <th style="width: 11%">
                                                                                    Particulars / تفاصيل
                                                                                </th>
                                                                                <th style="width: 7%">
                                                                                    Price / السعر
                                                                                </th>
                                                                                <th style="width: 5%; display: none">
                                                                                    Service Charge / تكلفة الخدمة
                                                                                </th>
                                                                                <th style="width: 6%">
                                                                                    Fine / مبلغ الغرامة
                                                                                </th>
                                                                                <th runat="server" id="th_discount" style="width: 7%">
                                                                                    Discount / خصم
                                                                                </th>
                                                                                <th style="width: 5%">
                                                                                    Qty / الكمية
                                                                                </th>
                                                                                <th style="width: 2%; display: none">
                                                                                    VAT / ضريبة
                                                                                </th>
                                                                                <th style="width: 6%">
                                                                                    Tax / ضريبة
                                                                                </th>
                                                                                <th style="width: 10%">
                                                                                    Amt With Tax 
                                                                                </th>
                                                                                <th style="width: 9%">
                                                                                    Total / مجموع
                                                                                </th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <asp:Repeater ID="rpt_Item_list" runat="server" >
                                                                                <ItemTemplate>
                                                                                    <tr style="text-align: center">
                                                                                        <td>
                                                                                            <asp:CheckBox ID="chkSel" runat="server" class="supchkitem" Checked='<%#Convert.ToBoolean(Eval("CheckBoxValue"))%>'/>
                                                                                        </td>
                                                                                         <td style="text-align: left">
                                                                                            <asp:Label ID="lblInvCode" Width="95%" TabIndex="-1" runat="server" Text='<%#Eval("InvoiceCode") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td style="text-align: left" colspan="2">
                                                                                            <asp:HiddenField ID="hdnInvDId" runat="server" Value='<%#Eval("D_id") %>' />
                                                                                            <asp:HiddenField ID="hdnInvoiceDetailId" runat="server" Value='<%#Eval("InvoiceDetailId") %>' />
                                                                                            <asp:HiddenField ID="hdnInvoiceId" runat="server" Value='<%#Eval("InvoiceId") %>' />
                                                                                            <asp:Label ID="lblServiceFullName" runat="server" TabIndex="-1" Text='<%#Eval("ServiceFullName") %>' />

                                                                                        </td>
                                                                                        <td style="text-align: left">
                                                                                            <asp:Label ID="lblInvDdesc" Width="95%" TabIndex="-1" runat="server" Text='<%#Eval("Particulars") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td style="text-align: left">
                                                                                            <asp:TextBox ID="txtInvDDisplayPrice" class="txt unit_amtD read_Only numbers_only asLabel"
                                                                                                Width="85%" runat="server" Text='<%#Eval("DisplayPrice") %>' TabIndex="-1"></asp:TextBox>
                                                                                        </td>
                                                                                        <td style="text-align: left; display: none">
                                                                                            <asp:TextBox ID="txtInvDAddServiceCharge" class="txt serCharge_amtD read_Only numbers_only asLabel"
                                                                                                Width="85%" runat="server" Text='<%#Eval("AdditionalServiceCharge") %>' TabIndex="-1"></asp:TextBox>
                                                                                        </td>
                                                                                        <td style="text-align: left">
                                                                                            <asp:TextBox ID="txtInvDFine" class="txt fine_amtD read_Only numbers_only asLabel"
                                                                                                Width="85%" runat="server" Text='<%#Eval("Fine") %>' TabIndex="-1"></asp:TextBox>
                                                                                        </td>
                                                                                        <td runat="server" id="td_discount" style="text-align: left">
                                                                                            <asp:TextBox ID="txtInvDdiscount" class="read_Only discountD InvDdiscount asLabel txt"
                                                                                                Width="85%" runat="server" Text='<%#Eval("Discount") %>'></asp:TextBox>
                                                                                        </td>
                                                                                        <td style="text-align: left">
                                                                                            <asp:TextBox ID="txtInvDQty" class="numbers_only txt qtyD read_Only  InvDQty asLabel"
                                                                                                Width="75%" runat="server" Text='<%#Eval("Quantity") %>' TabIndex="-1"></asp:TextBox>
                                                                                        </td>
                                                                                        <td style="text-align: left; display: none">
                                                                                            <asp:TextBox TabIndex="-1" ID="txtInvDVatPer" class="numbers_only read_Only txt asLabel"
                                                                                                Width="95%" runat="server" Text='<%#Eval("Tax") %>'></asp:TextBox>
                                                                                        </td>
                                                                                        <td style="text-align: left">
                                                                                            <asp:TextBox TabIndex="-1" ID="txtInvDTaxAmount" class="numbers_only taxamtD read_Only txt asLabel"
                                                                                                Width="95%" runat="server" Text='<%#Eval("TaxAmount") %>'></asp:TextBox>
                                                                                            <asp:HiddenField ID="hdnInvDTax" ClientIDMode="Static" runat="server" Value='<%#Eval("Tax") %>' />
                                                                                        </td>
                                                                                        <td style="text-align: left">
                                                                                            <asp:TextBox ID="txtInvDPriceWitTax" TabIndex="-1" class="numbers_only Prc_amtD read_Only txt asLabel"
                                                                                                Width="95%" runat="server" Text='<%#Eval("PriceWitTax") %>'></asp:TextBox>
                                                                                        </td>
                                                                                        <td style="text-align: left">
                                                                                            <asp:TextBox ID="txtInvDTotal" TabIndex="-1" class="numbers_only invtot il_tot_amtD read_Only txt asLabel"
                                                                                                Width="95%" runat="server" Text='<%#Eval("Total") %>'></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>
                                                                            <tr runat="server" id="tr_maindiscount">
                                                                                <td colspan="11" style="text-align: right">
                                                                                    Discount / خصم
                                                                                </td>
                                                                                <td colspan="2">
                                                                                    <asp:UpdatePanel ID="Updtxt_totDiscount" runat="server" ChildrenAsTriggers="false"
                                                                                        UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox TabIndex="-1" Style="border: medium none; color: Red; font-size: 24px;
                                                                                                text-align: right; width: 95%" class="txt tot_discount readOnly" ID="txt_totDiscount"
                                                                                                runat="server"></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td runat="server" id="td_total"  colspan="11" style="text-align: right">
                                                                                Total Amount / المبلغ الإجمالي
                                                                                </td>
                                                                                <td colspan="2">
                                                                                    <asp:UpdatePanel ID="Upd_Total_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox TabIndex="-1" Style="border: medium none; color: Red; font-size: 24px;
                                                                                                text-align: right; width: 95%" class="txt tot_grnd_amt readOnly txt" ID="txt_grand"
                                                                                                runat="server"></asp:TextBox>
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
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        Remarks / ملاحظات
                                                        <asp:TextBox class="txtarea" Style="width: 90%" TextMode="MultiLine" ID="txt_remark"
                                                            runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:UpdatePanel ID="Upd_total" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                                                <%--Regarding Customer User Control--%>
                                                                <asp:HiddenField ID="hdn_user_id" runat="server" />
                                                                <asp:HiddenField ID="hdnInvoiceStatus" ClientIDMode="Static" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_CurrentInvoiceReceivable" ClientIDMode="Static" runat="server" Value="0" />

                                                                <asp:Button ID="btn_save" class="butn_save" ValidationGroup="save" OnClick="btn_save_OnClick"
                                                                    runat="server" Text="Save/حفظ" />
                                                                <asp:Button ID="btn_save_print" class="butn_save" ValidationGroup="save" OnClick="btn_save_print_OnClick"
                                                                    runat="server" Text="Save & Print/حفظ وطباعة" />
                                                                <asp:Button ID="btn_print" class="butn" runat="server" Text="Print/طباعة" OnClick="btn_print_OnClick" />
                                                                <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_OnClick" />
                                                                <asp:Button ID="Button1" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_OnClick" />
                                                                <asp:Button ID="btn_cancel" class="butn_delete" runat="server" OnClientClick="javascript : return confirm('Do you really want to Cancel.. ?');"
                                                                    Visible="false" Text="Cancel/إلغاء" OnClick="btn_Cancelmain_OnClick" />
                                                                <asp:Button ID="btn_history" class="butn" runat="server" Visible="false" Text="History/سجل"
                                                                    OnClick="btn_histry_OnClick" />
                                                               
                                                                <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_update" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_print" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_add_N_print" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_update_N_print" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_cancel" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_histry" runat="server" Value="0" />
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
                                </div>
                                <div id="div_trans_main" visible="false" runat="server">
                                    <div class="Adding_heading">
                                        Invoice History/تاريخ الفاتورة
                                    </div>
                                    <%--<div style="position: absolute; right: 45px; text-align: right; top: 8%;">
                                        <asp:LinkButton ID="LinkButton4" runat="server" Style="color: Blue" OnClick="btn_histry_Close_OnClick">Close</asp:LinkButton>
                                    </div>--%>
                                    <table style="margin-left: 20px; width: 60%">
                                        <tr>
                                            <td>
                                                From/من عند
                                                <br />
                                                <telerik:RadDatePicker ID="date_from" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                    <Calendar runat="server" ID="Calendar12" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                        ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                        <SpecialDays>
                                                            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                            </telerik:RadCalendarDay>
                                                        </SpecialDays>
                                                    </Calendar>
                                                </telerik:RadDatePicker>
                                            </td>
                                            <td>
                                                To/إلى
                                                <br />
                                                <telerik:RadDatePicker ID="date_to" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                    <Calendar runat="server" ID="Calendare2" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                        ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                        <SpecialDays>
                                                            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                            </telerik:RadCalendarDay>
                                                        </SpecialDays>
                                                    </Calendar>
                                                </telerik:RadDatePicker>
                                            </td>
                                            <td>
                                                <asp:Button ID="Button6" class="butn" OnClick="btn_his_seacrh_OnClick" runat="server"
                                                    Text="Search" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="padding: 20px; width: 100%">
                                        <tr>
                                            <td colspan="4">
                                                <asp:UpdatePanel ID="Upd_History" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btn_ex_his" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <div id="div_menu" runat="server" style="width: 100%; min-height: 300px; max-height: 300px;
                                                            overflow: auto;">
                                                            <asp:Button ID="btn_ex_his" runat="server" Style="float: right" class="btn_excel right_align_list"
                                                                ToolTip="Export to Excel" OnClick="btnexcel_exportHis_OnClick" />
                                                            <table class="listTable">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="text-align: center; width: 5%">
                                                                            Sl/رقم
                                                                        </th>
                                                                        <th style="text-align: center; width: 30%">
                                                                            Remark/تعليق
                                                                        </th>
                                                                        <th style="text-align: center; width: 10%">
                                                                            Done By/تم بواسطة
                                                                        </th>
                                                                        <th style="text-align: center; width: 10%">
                                                                            Date/تاريخ
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                                <asp:Repeater ID="rpt_His" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td style="text-align: center;">
                                                                                <%#Eval("SLNo")%>
                                                                            </td>
                                                                            <td>
                                                                                <%#Eval("Remark")%>
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
                                                                <tr>
                                                                    <td colspan="4" class="navigationRow">
                                                                        <asp:UpdatePanel ID="upd_his_nav" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:Label ID="lbl_page_info1" runat="server" class="pageInfo"></asp:Label>
                                                                                <asp:Button ID="Button7" runat="server" class="navigationButton" Text="<<" OnClick="btn_first1_OnClick" />
                                                                                <asp:Button ID="Button8" runat="server" class="navigationButton" Text="<" OnClick="btn_prev1_OnClick" />
                                                                                <asp:Label ID="lbl_page_number1" Style="font-weight: bold; margin-left: 5px; margin-right: 5px;
                                                                                    text-align: center;" runat="server"></asp:Label>
                                                                                <asp:Button ID="Button9" class="navigationButton" runat="server" Text=">" OnClick="btn_next1_OnClick" />
                                                                                <asp:Button ID="Button10" class="navigationButton" runat="server" Text=">>" OnClick="btn_last1_OnClick" />
                                                                                <asp:DropDownList ID="drp_count1" class="pageSize" runat="server" AutoPostBack="true"
                                                                                    OnSelectedIndexChanged="drp_count1_OnSelectedIndexChanged">
                                                                                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                                    <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                                                    <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                                                    <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                                <asp:HiddenField ID="hdn_last_page1" runat="server" />
                                                                                <asp:HiddenField ID="hdn_total1" runat="server" Value="0" />
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Button ID="Button4" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_histry_Close_OnClick" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="upd_cancl" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnl_cancl" runat="server" Visible="false">
                <div class="popupBackground">
                </div>
                <div class="animated smallPopUp">
                    <div class="Adding_heading">
                        Cancel Invoice/الغاء الفاتورة
                    </div>
                    <div runat="server" visible="false" id="div_candet">
                        <div style="padding: 10px">
                            <b>Select the entries you want to cancel before cancelling invoice/حدد الادخالات التي
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
                                <asp:TextBox ID="txt_cancelremark" CssClass="txtarea" TextMode="MultiLine" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txt_cancelremark"
                                    ValidationGroup="cancl" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="Button2" class="butn_save" ValidationGroup="cancl" OnClick="btn_cancel_OnClick"
                                    runat="server" Text="Cancel/إلغاء" />
                                <asp:Button ID="Button3" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_cnclse_OnClick" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
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
</asp:Content>

