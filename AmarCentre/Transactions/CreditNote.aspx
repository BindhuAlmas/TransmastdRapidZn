<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="CreditNote.aspx.cs" Inherits="AmarCentre.Transactions.CreditNote" %>

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
            $('.qty').blur(function (e) {
                $('.qty').each(function () {
                    var Pqty = 0;
                    var Qty = 0;
                    var Ptaxamt = 0;
                    var Pil_tot_amt = 0;
                    var TaxAmt = 0;
                    var TotAmt = 0;
                   
                    if ($(this).closest("tr").find('.Pqty').val() != '') {
                        Pqty = parseFloat($(this).closest("tr").find('.Pqty').val());
                    }
                    if ($(this).closest("tr").find('.Ptaxamt').val() != '') {
                        Ptaxamt = parseFloat($(this).closest("tr").find('.Ptaxamt').val());
                    }
                    if ($(this).closest("tr").find('.qty').val() != '') {
                        Qty = parseFloat($(this).closest("tr").find('.qty').val());
                    }
                    if ($(this).closest("tr").find('.Pil_tot_amt').val() != '') {
                        Pil_tot_amt = parseFloat($(this).closest("tr").find('.Pil_tot_amt').val());
                    }

                    if (parseFloat(Qty) > parseFloat(Pqty)) {
                        alert('Quantity should be less than Pending Quantity.');
                        Qty = 0;
                        $(this).closest("tr").find('.qty').val('');
                    }

                    TaxAmt = ((parseFloat(Ptaxamt) / parseFloat(Pqty)) * parseFloat(Qty)).toFixed(2);
                    TotAmt = ((parseFloat(Pil_tot_amt) / parseFloat(Pqty)) * parseFloat(Qty)).toFixed(2);

                    $(this).closest("tr").find('.taxamt').val(parseFloat(TaxAmt).toFixed(2));
                    $(this).closest("tr").find('.il_tot_amt').val(parseFloat(TotAmt).toFixed(2));

                });
                Calc();
            });

             $('.chkclick').click(function () {
                Calc();
            });
            $('.taxamt,.il_tot_amt').blur(function (e) {
                Calc();
            });
            function Calc() {
                var TaxAmtTotal = 0;
                var TotAmt = 0;
                $('.qty').each(function () {
                    var taxamt = 0;
                    var il_tot_amt = 0;
                    if ($(this).closest('tr').find(':checkbox').prop('checked')) {
                        if ($(this).closest("tr").find('.taxamt').val() != '') {
                            taxamt = parseFloat($(this).closest("tr").find('.taxamt').val());
                        }
                        if ($(this).closest("tr").find('.il_tot_amt').val() != '') {
                            il_tot_amt = parseFloat($(this).closest("tr").find('.il_tot_amt').val());
                        }

                        TaxAmtTotal = (parseFloat(TaxAmtTotal) + parseFloat(taxamt)).toFixed(2);
                        TotAmt = (parseFloat(TotAmt) + parseFloat(il_tot_amt)).toFixed(2);
                    }
                });

                $('.tottaxamt').val(parseFloat(TaxAmtTotal).toFixed(2));
                $('.tot_grnd_amt').val(parseFloat(TotAmt).toFixed(2));
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Credit Note
        <asp:Button ID="btn_addnew" runat="server"  class="btnAddNew" OnClick="btn_newentry_OnClick" />
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
                            <th style="width: 20%;">Customer / زبون
                            </th>
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
                                        <%#Eval("Name")%>
                                    </td>

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
                                    Credit Note
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td style="width: 33%">Code
                                            <asp:TextBox ID="lbl_Code" runat="server" class="txt read_Only" Font-Bold="true"
                                                Text=""></asp:TextBox>
                                        </td>
                                        <td style="width: 33%">Date / تاريخ <span style="color: Red">&nbsp*</span>
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
                                        <td style="width: 33%"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 33%">Customer
                                            <telerik:RadComboBox ID="drp_customer" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                AutoPostBack="true" OnClientBlur="ValidateCombo" EmptyMessage="Search Name..."
                                                OnSelectedIndexChanged="drp_customer_OnSelectedIndexChanged" Style="overflow: hidden; width: 96%; border: none!important;">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td style="width: 33%">
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
                                         <td style="width: 33%"></td>
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
                                                                    <th style="width: 3%">Select
                                                                    </th>
                                                                    <th style="width: 23%">Service / الخدمات
                                                                    </th>
                                                                    <th style="width: 15%">Particulars / تفاصيل
                                                                    </th>
                                                                    <th style="width: 6%">Pending Qty
                                                                    </th>
                                                                    <th style="width: 7%">Tax / ضريبة
                                                                    </th>
                                                                    <th style="width: 9%">Total / مجموع
                                                                    </th>
                                                                    <th style="width: 10%">Received Amount
                                                                    </th>
                                                                    <th style="width: 5%">Qty / الكمية
                                                                    </th>
                                                                    <th style="width: 7%">Tax / ضريبة
                                                                    </th>
                                                                    <th style="width: 9%">Total / مجموع
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <asp:Repeater ID="rpt_Item_list" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr style="text-align: center">
                                                                            <td>
                                                                                <asp:CheckBox id="chksel" runat="server" class="chkclick"  Checked='<%#Convert.ToBoolean(Eval("Selected"))%>' />
                                                                                <asp:HiddenField ID="hdn_D_id" runat="server" Value='<%#Eval("D_id") %>' />
                                                                                <asp:HiddenField ID="hdnInvoiceDetId" runat="server" Value='<%#Eval("InvoiceDetId") %>' />
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:Label ID="lbl_service" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:Label ID="lbl_desc" runat="server" Text='<%#Eval("Particulars") %>'></asp:Label>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txt_QtyP" TabIndex="-1" class="numbers_only read_Only Pqty inline txt asLabel"
                                                                                    Width="75%" runat="server" Text='<%#Eval("PendingQuantity") %>'></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txt_taxP" TabIndex="-1" class="numbers_only Ptaxamt read_Only txt asLabel"
                                                                                    Width="90%" runat="server" Text='<%#Eval("TaxAmount") %>'></asp:TextBox>
                                                                                <asp:HiddenField ID="hdn_tax" ClientIDMode="Static" runat="server" Value='<%#Eval("Tax") %>' />
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txt_totPriceP" TabIndex="-1" class="numbers_only Pil_tot_amt read_Only txt asLabel"
                                                                                    Width="90%" runat="server" Text='<%#Eval("ServiceAmount") %>'></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txtreceived" TabIndex="-1" class="numbers_only  read_Only txt asLabel"
                                                                                    Width="90%" runat="server" Text='<%#Eval("ReceivedAmount") %>'></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txt_Qty" TabIndex="-1" class="numbers_only  qty  txt "
                                                                                    Width="75%" runat="server" Text='<%#Eval("Quantity") %>'></asp:TextBox>
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txt_tax" TabIndex="-1" class="numbers_only taxamt txt "
                                                                                    Width="90%" runat="server" Text='<%#Eval("Tax") %>'></asp:TextBox>
                                                                                <asp:HiddenField ID="HiddenField1" ClientIDMode="Static" runat="server" Value='<%#Eval("Tax") %>' />
                                                                            </td>
                                                                            <td style="text-align: left">
                                                                                <asp:TextBox ID="txt_totPrice" TabIndex="-1" class="numbers_only il_tot_amt txt txt_totPrice"
                                                                                    Width="90%" runat="server" Text='<%#Eval("Total") %>'></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                                <tr>
                                                                    <td colspan="8" style="text-align: right">Total Tax
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:TextBox TabIndex="-1" Style="border: medium none; color: Red; font-size: 24px; text-align: right; width: 95%"
                                                                            class="txt tottaxamt read_Only txt_80" ID="txtTotalTax" runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td colspan="8" style="text-align: right">Total Amount
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:TextBox TabIndex="-1" Style="border: medium none; color: Red; font-size: 24px; text-align: right; width: 95%"
                                                                            class="txt tot_grnd_amt read_Only txt_80" ID="txt_grand" runat="server"></asp:TextBox>
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
                                                <table style="width: 95%">
                                                    <tr>
                                                        <td colspan="3">Remarks / ملاحظات
                                            <asp:TextBox class="txtarea" Style="width: 90%" TextMode="MultiLine" ID="txt_remark" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdn_user_id" runat="server" />
                                                            <asp:Button ID="btn_save" class="butn_save" ValidationGroup="save" OnClick="btn_save_OnClick"
                                                                OnClientClick="if (Page_ClientValidate('save') == false) return(false);else return confirm('Do you really want to Save.. ?');"
                                                                runat="server" Text="Save/حفظ" />
                                                            <asp:Button ID="btn_save_print" class="butn_save" ValidationGroup="save" OnClick="btn_save_print_OnClick"
                                                                OnClientClick="if (Page_ClientValidate('save') == false) return(false);else return confirm('Do you really want to Save.. ?');"
                                                                runat="server" Text="Save & Print/حفظ وطباعة" />
                                                            <asp:Button ID="btn_print" class="butn" runat="server" Text="Print" OnClick="btn_print_OnClick" />
                                                             <asp:Button ID="btn_cancel" class="butn_delete"  OnClick="btn_cancel_OnClick"
                                                                OnClientClick="javascript : return confirm('Do you really want to cancel.. ?');"
                                                                runat="server" Text="Cancel/إلغاء" />
                                                            <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_OnClick" />
                                                            <asp:Button ID="Button1" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_OnClick" />
                                                            <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdn_print" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdn_add_N_print" runat="server" Value="0" />
                                                           <%-- <asp:HiddenField ID="hdnupdate" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnupdateNPrint" runat="server" Value="0" />--%>
                                                            <asp:HiddenField ID="hdncancel" runat="server" Value="0" />

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

    </div>

</asp:Content>

