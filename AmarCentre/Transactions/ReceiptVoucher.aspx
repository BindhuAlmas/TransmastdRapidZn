<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true"
    CodeBehind="ReceiptVoucher.aspx.cs" Inherits="AmarCentre.Transactions.ReceiptVoucher" %>

<%@ Register Src="~/Transactions/UserControl/UCRV.ascx" TagName="RVUC"
    TagPrefix="AmarCentre" %>
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

            $('.Vdepositsupchkitem').click(function () {
                Sup_Calculate();
            });

            $('.VdepositPayAmt').blur(function (e) {
                Sup_Calculate();
            });

            function Sup_Calculate() {
                var count = 0;
                var total = 0;
                var chk = 0;

                $(".VdepositPayAmt").each(function () {
                    var OSAmt = 0;
                    OSAmt = $(this).closest('tr').find('.VdepositBalAmt').val();
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

                $(".VdepositTotAmt").val(total);
                if (chk == "1") {
                    $(".mainamount").val(total);
                    $(".mainamount").attr('readonly', true);
                }
                else {
                    $(".mainamount").val('');
                    $(".mainamount").attr('readonly', false);
                }
            }


            function CalcCommsn() {
                var Commsn = 0;
                var bankcmper = 0;
                var AmtPayingNow = 0;
                bankcmper = $('#hdn_bankcommsn').val();
                if ($('#txtAmountMain').val() != '') {
                    AmtPayingNow = parseFloat($('#txtAmountMain').val());
                }
                Commsn = parseFloat(AmtPayingNow) * parseFloat(bankcmper) / 100;
                $('.comssnAmt').val(Commsn);
            }
            $('#txtAmountMain').blur(function (e) {
                CalcCommsn();
            });

            $('.paidAmt').blur(function (e) {
                //$('.totype').val()
                var TotAmt = 0;
                var OTAmt = 0;
                $('.paidAmt').each(function () {
                    var PaidAmt = 0;
                    var RAmt = 0;
                    if ($.trim($(this).closest("tr").find('.paidAmt').val()) != '') {
                        PaidAmt = parseFloat($(this).closest("tr").find('.paidAmt').val());
                    }
                    if ($.trim($(this).closest("tr").find('.receivableamt').val()) != '') {
                        RAmt = parseFloat($(this).closest("tr").find('.receivableamt').val());
                    }
                    if (parseFloat(PaidAmt) > parseFloat(RAmt)) {
                        alert('Amount cannot be greater than Receivable Amount');
                        $(this).closest("tr").find('.paidAmt').val('');
                        PaidAmt = 0;
                    }
                    TotAmt = parseFloat(TotAmt) + parseFloat(PaidAmt);

                    CalcCommsn();
                });



                $('.receivableamt').each(function () {
                    var PaidAmt = 0;
                    var RAmt = 0;
                    if ($.trim($(this).closest("tr").find('.paidAmt').val()) != '') {
                        PaidAmt = parseFloat($(this).closest("tr").find('.paidAmt').val());
                    }
                    if ($.trim($(this).closest("tr").find('.receivableamt').val()) != '') {
                        RAmt = parseFloat($(this).closest("tr").find('.receivableamt').val());
                    }
                    if (parseFloat(PaidAmt) > parseFloat(RAmt)) {
                        PaidAmt = 0;
                    }
                    OTAmt = parseFloat(OTAmt) + (parseFloat(RAmt) - parseFloat(PaidAmt));

                    CalcCommsn();
                });
                if ($('.totype').val() == 'Advance') {
                    var advance = parseFloat(0 + $('.advance').text());
                    if (parseFloat(TotAmt) > advance) {
                        alert('Amount exceeding the advance amount');
                        TotAmt = parseFloat(TotAmt) - parseFloat($(this).val()) + 0;
                        //console.log($(this).val());
                        $(this).val('');

                        //console.log(TotAmt);
                        //TotAmt = parseFloat(TotAmt) - parseFloat($(this).val())+0;
                    }
                }

                $('.OutstandingAmount').val(parseFloat(OTAmt).toFixed(2));
                $('.total').val(parseFloat(TotAmt).toFixed(2));

                CalcCommsn();
            });

            $('.paidAmtCG').blur(function (e) {
                //$('.totype').val()
                var TotAmt = 0;
                var OTAmt = 0;
                $('.paidAmtCG').each(function () {
                    var PaidAmt = 0;
                    var RAmt = 0;
                    if ($.trim($(this).closest("tr").find('.paidAmtCG').val()) != '') {
                        PaidAmt = parseFloat($(this).closest("tr").find('.paidAmtCG').val());
                    }
                    if ($.trim($(this).closest("tr").find('.receivableamtCG').val()) != '') {
                        RAmt = parseFloat($(this).closest("tr").find('.receivableamtCG').val());
                    }
                    if (parseFloat(PaidAmt) > parseFloat(RAmt)) {
                        alert('Amount cannot be greater than Receivable Amount');
                        $(this).closest("tr").find('.paidAmtCG').val('');
                        PaidAmt = 0;
                    }
                    TotAmt = parseFloat(TotAmt) + parseFloat(PaidAmt);

                    CalcCommsn();
                });



                $('.receivableamtCG').each(function () {
                    var PaidAmt = 0;
                    var RAmt = 0;
                    if ($.trim($(this).closest("tr").find('.paidAmtCG').val()) != '') {
                        PaidAmt = parseFloat($(this).closest("tr").find('.paidAmtCG').val());
                    }
                    if ($.trim($(this).closest("tr").find('.receivableamtCG').val()) != '') {
                        RAmt = parseFloat($(this).closest("tr").find('.receivableamtCG').val());
                    }
                    if (parseFloat(PaidAmt) > parseFloat(RAmt)) {
                        PaidAmt = 0;
                    }
                    OTAmt = parseFloat(OTAmt) + (parseFloat(RAmt) - parseFloat(PaidAmt));

                    CalcCommsn();
                });

                $('.totalCG').val(parseFloat(TotAmt).toFixed(2));
                CalcCommsn();
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Receipt Voucher/سند القبض
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
                            <th style="width: 3%;">Sl
                            </th>
                            <th style="width: 7%;">Code/رمز
                            </th>
                            <th style="width: 20%;">Name/اسم
                            </th>
                            <th style="width: 15%;">Income Type/نوع الدخل
                            </th>
                            <th style="width: 10%;">Date/تاريخ
                            </th>
                            <th style="width: 8%;">Amount/المبلغ
                            </th>
                            <th style="width: 8%;">Status/الحالة
                            </th>
                            <th style="width: 5%;">Action
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
                                        <asp:HiddenField ID="hdntypeid" runat="server" Value='<%#Eval("Type")%>' />

                                    </td>
                                    <td>
                                        <%#Eval("Code")%>
                                    </td>
                                    <td>
                                        <%#Eval("Name")%>
                                    </td>
                                    <td>
                                        <%#Eval("IncomeTypeName")%>
                                    </td>
                                    <td>
                                        <%#Eval("Dateds")%>
                                    </td>
                                    <td>
                                        <%#Eval("AmountWitTax")%>
                                    </td>
                                    <td>
                                        <%#Eval("StatusName")%>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_edit" runat="server" class="btn_edit" CommandName="Edit" />
                                        <asp:Button ID="btnSendmail" runat="server" class="btnsendmail" ToolTip="Send Mail"
                                            CommandName="Sendmail" />
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
                    <div class="animated halfPopUp">
                        <AmarCentre:RVUC ID="UCRV" runat="server" />
                                                    <asp:HiddenField ID="hdn_user_id" runat="server" />
                                                    <asp:HiddenField ID="hdnsendmail" runat="server" />

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
                <asp:UpdatePanel ID="UpdMailPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMail" Visible="false" runat="server">
                            <AmarCentre:MailUC ID="EmailUC" runat="server" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
</asp:Content>
