<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true"
    CodeBehind="QuickReceipt.aspx.cs" Inherits="AmarCentre.Transactions.QuickReceipt" %>

<%@ Register Src="~/Transactions/UserControl/Customer.ascx" TagName="CustomerMaster"
    TagPrefix="AmarCentre" %>
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

            $("input[id='rbTaxInvoice']").change(function () {
                TaxChange();
            });
            $("input[id='rbNormalInvoice']").change(function () {
                TaxChange();
            });

            /*Unit Price,Amount,Discount*/
            $('.inline').blur(function (e) {
                OutsideRepeaterCalculation();
            });

            //sc section
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

            $('.jcalculation').blur(function (e) {
                Calculation();
            });

            $('.scQty').blur(function (e) {
                var Qty = 0;
                var IncQty = 0;
                if ($.trim($('.scQty').val()) != '') {
                    Qty = parseFloat($('.scQty').val());
                }
                if ($.trim($('#hdn_InComQty').val()) != '') {
                    IncQty = parseFloat($('#hdn_InComQty').val());
                }
                if (parseFloat(Qty) > parseFloat(IncQty)) {
                    alert("Quantity Cannot be greater than Pending Quantity");
                    $('.scQty').val('');
                }
                Calculation();
            });
            function Calculation() {
                var Qty = 0;
                var AmtSingleQty = 0;
                var GrndTotAmt = 0;

                if ($.trim($('.scQty').val()) != '') {
                    Qty = parseFloat($('.scQty').val());
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
                    $(this).closest("tr").find('.paidAmount').val(parseFloat(PayableAmt).toFixed(2));
                });
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

            //sc section end

            function TaxChange() {
                InsideRepeaterCalculation();
                OutsideRepeaterCalculation();
            }
            function InsideRepeaterCalculation() {
                $('.unit_amtD').each(function () {
                    var UP = 0;
                    var Qty = 0;
                    var Amt = 0;
                    var TotAmt = 0;
                    var DiscntAmt = 0;

                    var expamt = 0;
                    var SCamt = 0;
                    var TxtSCAmt = 0
                    var TotSCAmt = 0;
                    var taxamt = 0;
                    var taxper = 0;
                    var pricewittax = 0;
                    var fine = 0;
                    var fineapplicable = 0;
                    var exptot = 0;
                    var taxtot = 0;
                    var SerPriceWithTax = 0;
                    var Price = 0;
                    var TaxAppliedWithDiscount = 0;
                    var InvoiceType = 0;
                    if ($('#rbTaxInvoice').is(':checked')) {
                        InvoiceType = 1;
                    }
                    else {
                        InvoiceType = 2;
                    }
                    /*Service Price With Tax*/
                    if ($('#hdnSerPriceWTax').val() != '') {
                        SerPriceWithTax = parseInt($('#hdnSerPriceWTax').val());
                    }
                    if ($('#hdnTaxAppliedWithDiscount').val() != '') {
                        TaxAppliedWithDiscount = parseInt($('#hdnTaxAppliedWithDiscount').val());
                    }
                    /*Display Price*/
                    if ($(this).closest("tr").find('.unit_amtD').val() != '') {
                        UP = parseFloat($(this).closest("tr").find('.unit_amtD').val());
                    }
                    /*Additional Service Charge*/
                    /*As of 03-12-2018 it is hidden*/
                    if ($(this).closest("tr").find('.serCharge_amtD').val() != '') {
                        TxtSCAmt = parseFloat($(this).closest("tr").find('.serCharge_amtD').val());
                    }
                    /*Quantity*/
                    if ($(this).closest("tr").find('.qtyD').val() != '') {
                        Qty = parseFloat($(this).closest("tr").find('.qtyD').val());
                    }
                    if ($(this).closest("tr").find('.discountD').val() != '') {
                        DiscntAmt = parseFloat($(this).closest("tr").find('.discountD').val());
                    }
                    /*Expense*/
                    if ($(this).closest("tr").find('#hdnInvDExpense').val() != '') {
                        expamt = parseFloat($(this).closest("tr").find('#hdnInvDExpense').val());
                    }
                    /*Fine Applicable*/
                    if ($(this).closest("tr").find('#hdnInvDFineApplicable').val() != '') {
                        fineapplicable = parseFloat($(this).closest("tr").find('#hdnInvDFineApplicable').val());
                    }
                    /*Tax Percentage*/
                    if ($(this).closest("tr").find('#hdnInvDTax').val() != '') {
                        taxper = parseFloat($(this).closest("tr").find('#hdnInvDTax').val());
                    }
                    /*Fine*/
                    if ($(this).closest("tr").find('.fine_amtD').val() != '') {
                        fine = parseFloat($(this).closest("tr").find('.fine_amtD').val());
                        if ((fine > 0) && fineapplicable == 0) {
                            fine = 0;
                            $(this).closest("tr").find('.fine_amtD').val('');
                            alert('Fine Account for this service is not set');
                        }
                    }

                    TotSCAmt = parseFloat(TxtSCAmt) + parseFloat(SCamt); /*This was used in taxamt before PriceWith Tax concept*/
                    /*Tax Amount*/
                    if (InvoiceType == 1) {
                        if (TaxAppliedWithDiscount == 0) {
                            if (SerPriceWithTax == 0) {/*(Price-Expense)*TaxPer/100*/
                                taxamt = ((parseFloat(UP) - parseFloat(expamt)) * parseFloat(taxper) / 100).toFixed(2);
                            } else if (SerPriceWithTax == 1) {/*(Price-Expense)*TaxPer/(100+Taxper)*/
                                taxamt = (((parseFloat(UP) - parseFloat(expamt)) * parseFloat(taxper)) / (100 + parseFloat(taxper))).toFixed(2);
                            }
                        } else if (TaxAppliedWithDiscount == 1) {
                            if (SerPriceWithTax == 0) {/*(Price-Expense)*TaxPer/100*/
                                taxamt = ((parseFloat(UP) - parseFloat(DiscntAmt) - parseFloat(expamt)) * parseFloat(taxper) / 100).toFixed(2);
                            } else if (SerPriceWithTax == 1) {/*(Price-Expense)*TaxPer/(100+Taxper)*/
                                taxamt = (((parseFloat(UP) - parseFloat(DiscntAmt) - parseFloat(expamt)) * parseFloat(taxper)) / (100 + parseFloat(taxper))).toFixed(2);
                            }
                        }
                    }
                    else if (InvoiceType == 2) {
                        taxamt = 0.00;
                    }
                    /*Price*/
                    if (SerPriceWithTax == 0) {
                        Price = parseFloat(UP);
                    } else if (SerPriceWithTax == 1) {
                        Price = parseFloat(UP) - parseFloat(taxamt);
                    }
                    /*Service Charge*/
                    if (SerPriceWithTax == 0) {
                        SCamt = parseFloat(UP) - parseFloat(expamt);
                    } else if (SerPriceWithTax == 1) {
                        SCamt = parseFloat(UP) - parseFloat(expamt) - parseFloat(taxamt);
                    }
                    /*Price With Tax*/
                    pricewittax = parseFloat(Price) + parseFloat(taxamt) + parseFloat(TxtSCAmt) + parseFloat(fine);

                    TotAmt = ((parseFloat(pricewittax) - parseFloat(DiscntAmt)) * parseFloat(Qty)).toFixed(2);

                    $(this).closest("tr").find('.taxamtD').val(parseFloat(taxamt).toFixed(2));
                    $(this).closest("tr").find('#hdnInvDPrice').val(parseFloat(Price).toFixed(2));
                    $(this).closest("tr").find('#hdnInvDServiceCharge').val(parseFloat(SCamt).toFixed(2));
                    $(this).closest("tr").find('.Prc_amtD').val(parseFloat(pricewittax).toFixed(2));
                    $(this).closest("tr").find('.il_tot_amtD').val(parseFloat(TotAmt).toFixed(2));
                });
            }

            function OutsideRepeaterCalculation() {
                $('.unit_amt').each(function () {
                    var UP = 0;
                    var Qty = 0;
                    var Amt = 0;
                    var TotAmt = 0;
                    var DiscntAmt = 0;

                    var expamt = 0;
                    var SCamt = 0;
                    var TxtSCAmt = 0
                    var TotSCAmt = 0;
                    var taxamt = 0;
                    var taxper = 0;
                    var pricewittax = 0;
                    var fine = 0;
                    var fineapplicable = 0;
                    var exptot = 0;
                    var taxtot = 0;
                    var SerPriceWithTax = 0;
                    var Price = 0;
                    var TaxAppliedWithDiscount = 0;
                    var InvoiceType = 0;
                    if ($('#rbTaxInvoice').is(':checked')) {
                        InvoiceType = 1;
                    }
                    else {
                        InvoiceType = 2;
                    }
                    /*Service Price With Tax*/
                    if ($('#hdnSerPriceWTax').val() != '') {
                        SerPriceWithTax = parseInt($('#hdnSerPriceWTax').val());
                    }
                    if ($('#hdnTaxAppliedWithDiscount').val() != '') {
                        TaxAppliedWithDiscount = parseInt($('#hdnTaxAppliedWithDiscount').val());
                    }
                    /*Display Price*/
                    if ($(this).closest("tr").find('.unit_amt').val() != '') {
                        UP = parseFloat($(this).closest("tr").find('.unit_amt').val());
                    }
                    /*Additional Service Charge*/
                    /*As of 03-12-2018 it is hidden*/
                    if ($(this).closest("tr").find('.serCharge_amt').val() != '') {
                        TxtSCAmt = parseFloat($(this).closest("tr").find('.serCharge_amt').val());
                    }
                    /*Quantity*/
                    if ($(this).closest("tr").find('.qty').val() != '') {
                        Qty = parseFloat($(this).closest("tr").find('.qty').val());
                    }
                    if ($(this).closest("tr").find('.discount').val() != '') {
                        DiscntAmt = parseFloat($(this).closest("tr").find('.discount').val());
                    }
                    /*Expense*/
                    if ($(this).closest("tr").find('#hdn_expn').val() != '') {
                        expamt = parseFloat($(this).closest("tr").find('#hdn_expn').val());
                    }
                    /*Fine Applicable*/
                    if ($(this).closest("tr").find('#hdnFineApplicable').val() != '') {
                        fineapplicable = parseFloat($(this).closest("tr").find('#hdnFineApplicable').val());
                    }
                    /*Tax Percentage*/
                    if ($(this).closest("tr").find('#hdn_tax').val() != '') {
                        taxper = parseFloat($(this).closest("tr").find('#hdn_tax').val());
                    }
                    /*Fine*/
                    if ($(this).closest("tr").find('.fine_amt').val() != '') {
                        fine = parseFloat($(this).closest("tr").find('.fine_amt').val());
                        if ((fine > 0) && fineapplicable == 0) {
                            fine = 0;
                            $(this).closest("tr").find('.fine_amt').val('');
                            alert('Fine Account for this service is not set');
                        }
                    }

                    TotSCAmt = parseFloat(TxtSCAmt) + parseFloat(SCamt); /*This was used in taxamt before PriceWith Tax concept*/
                    /*Tax Amount*/
                    if (InvoiceType == 1) {
                        if (TaxAppliedWithDiscount == 0) {
                            if (SerPriceWithTax == 0) {/*(Price-Expense)*TaxPer/100*/
                                taxamt = ((parseFloat(UP) - parseFloat(expamt)) * parseFloat(taxper) / 100).toFixed(2);
                            } else if (SerPriceWithTax == 1) {/*(Price-Expense)*TaxPer/(100+Taxper)*/
                                taxamt = (((parseFloat(UP) - parseFloat(expamt)) * parseFloat(taxper)) / (100 + parseFloat(taxper))).toFixed(2);
                            }
                        } else if (TaxAppliedWithDiscount == 1) {
                            if (SerPriceWithTax == 0) {/*(Price-Expense)*TaxPer/100*/
                                taxamt = ((parseFloat(UP) - parseFloat(DiscntAmt) - parseFloat(expamt)) * parseFloat(taxper) / 100).toFixed(2);
                            } else if (SerPriceWithTax == 1) {/*(Price-Expense)*TaxPer/(100+Taxper)*/
                                taxamt = (((parseFloat(UP) - parseFloat(DiscntAmt) - parseFloat(expamt)) * parseFloat(taxper)) / (100 + parseFloat(taxper))).toFixed(2);
                            }
                        }
                    } else if (InvoiceType == 2) {
                        taxamt = 0.00;
                    }
                    /*Price*/
                    if (SerPriceWithTax == 0) {
                        Price = parseFloat(UP);
                    } else if (SerPriceWithTax == 1) {
                        Price = parseFloat(UP) - parseFloat(taxamt);
                    }
                    /*Service Charge*/
                    if (SerPriceWithTax == 0) {
                        SCamt = parseFloat(UP) - parseFloat(expamt);
                    } else if (SerPriceWithTax == 1) {
                        SCamt = parseFloat(UP) - parseFloat(expamt) - parseFloat(taxamt);
                    }
                    /*Price With Tax*/
                    pricewittax = parseFloat(Price) + parseFloat(taxamt) + parseFloat(TxtSCAmt) + parseFloat(fine);

                    TotAmt = ((parseFloat(pricewittax) - parseFloat(DiscntAmt)) * parseFloat(Qty)).toFixed(2);

                    $(this).closest("tr").find('.taxamt').val(parseFloat(taxamt).toFixed(2));
                    $(this).closest("tr").find('#hdnPrice').val(parseFloat(Price).toFixed(2));
                    $(this).closest("tr").find('#hdn_sc').val(parseFloat(SCamt).toFixed(2));
                    $(this).closest("tr").find('.Prc_amt').val(parseFloat(pricewittax).toFixed(2));
                    $(this).closest("tr").find('.il_tot_amt').val(parseFloat(TotAmt).toFixed(2));
                });
                Calc();
            }

            function CalcCommsn() {
                var Commsn = 0;
                var bankcmper = 0;
                var AmtPayingNow = 0;
                bankcmper = $('#hdn_bankcommsn').val();
                if ($('.amtPayNow').val() != '') {
                    AmtPayingNow = parseFloat($('.amtPayNow').val());
                }
                Commsn = parseFloat(AmtPayingNow) * parseFloat(bankcmper) / 100;
                $('.comssnAmt').val(Commsn);
            }

            function Calc() {
                var ILTotAmt = 0;
                var GrndTotAmt = 0;
                var DiscTotAmt = 0;
                var totQty = 0;
                var Presentqty = 0;
                var PresentDis = 0;
                var GrndDis = 0;

                var PresentTot = 0;
                $('.invtot').each(function () {
                    var Amt = 0;
                    var Dis = 0;

                    if ($(this).closest("tr").find('.invtot').val() != '') {
                        Amt = parseFloat($(this).closest("tr").find('.invtot').val());
                    }
                    if ($(this).closest("tr").find('.InvDdiscount').val() != '') {
                        Dis = parseFloat($(this).closest("tr").find('.InvDdiscount').val());
                    }
                    if ($(this).closest("tr").find('.InvDQty').val() != '') {
                        totQty = parseFloat($(this).closest("tr").find('.InvDQty').val());
                    }
                    ILTotAmt = parseFloat(ILTotAmt) + parseFloat(Amt);
                    DiscTotAmt = parseFloat(DiscTotAmt) + (parseFloat(Dis) * parseFloat(totQty));

                });
                if ($('.il_tot_amt').val() != '') {
                    PresentTot = parseFloat($('.il_tot_amt').val());
                }
                if ($('.discount').val() != '') {
                    PresentDis = parseFloat($('.discount').val());
                }
                if ($('.qty').val() != '') {
                    Presentqty = parseFloat($('.qty').val());
                }
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

                /*End of Amount Round Value*/

                Final = (parseFloat(AmtBeforeDecimal) + parseFloat(AmtDecimal)).toFixed(2);

                $('.tot_grnd_amt').val(parseFloat(Final).toFixed(2));
                $('.tot_discount').val(parseFloat(GrndDis).toFixed(2));

                if ($('#hdn_receivedAmt').val() != '') {
                    ReceivedAmt = parseFloat($('#hdn_receivedAmt').val());
                }
                PendingAmt = parseFloat(Final) - parseFloat(ReceivedAmt);

                $('.pendingAmt').val(parseFloat(PendingAmt).toFixed(2));
                $('.amtPayNow').val(parseFloat(PendingAmt).toFixed(2));
                $('.rAmt').val(parseFloat(PendingAmt).toFixed(2));

                CheckAmountPayingNow();
            }

            $('.amtPayNow').blur(function (e) {
                var PendingAmt = 0;
                var AmtPayingNow = 0;
                var RAmt = 0;

                if ($('.pendingAmt').val() != '') {
                    PendingAmt = parseFloat($('.pendingAmt').val());
                }
                if ($('.amtPayNow').val() != '') {
                    AmtPayingNow = parseFloat($('.amtPayNow').val());
                }
                if ($('.rAmt').val() != '') {
                    RAmt = parseFloat($('.rAmt').val());
                }
                if (PendingAmt < AmtPayingNow) {
                    alert('Amount cannot be greater than Pending Amount');
                    $('.amtPayNow').val('');
                    $('.balanceAmt').val($('.receivedAmt').val());
                    $('.amtPayNow').focus();
                }
                else if (RAmt < AmtPayingNow) {
                    alert('You cannot pay an amount greater than received amount');
                    $('.amtPayNow').val('');
                    $('.amtPayNow').focus();
                }
                else {
                    FillBalanceAmount();
                }
            });
            $('.rAmt').blur(function (e) {
                FillBalanceAmount();
            });
            function CheckAmountPayingNow() {
                var PendingAmt = 0;
                var AmtPayingNow = 0;
                var RAmt = 0;
                if ($('.pendingAmt').val() != '') {
                    PendingAmt = parseFloat($('.pendingAmt').val());
                }
                if ($('.amtPayNow').val() != '') {
                    AmtPayingNow = parseFloat($('.amtPayNow').val());
                }
                if ($('.rAmt').val() != '') {
                    RAmt = parseFloat($('.rAmt').val());
                }
                if (PendingAmt < AmtPayingNow) {
                    alert('Amount cannot be greater than Pending Amount');
                    $('.amtPayNow').val('');
                    $('.balanceAmt').val($('.receivedAmt').val());
                    $('.amtPayNow').focus();
                }

                else {
                    FillBalanceAmount();
                }
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
        Quick Receipt/إيصال
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
                            <th class="listTableAction" style="width: 5%;">
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
                                        <asp:HiddenField ID="hdn_rec_id" runat="server" Value='<%#Eval("ReceiptId")%>' />
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
                                    <td class="listTableActionButtonDiv">
                                        <asp:HiddenField ID="hdnIsCredit" runat="server" Value='<%#Eval("IsCredit")%>' />
                                        <asp:HiddenField ID="hdnReceived" runat="server" Value='<%#Eval("Received")%>' />
                                        <asp:HiddenField ID="hdnAfterDiscountGrandTotal" runat="server" Value='<%#Eval("AfterDiscount_GrandTotal")%>' />
                                        <asp:Button ID="btn_edit" runat="server" class="btn_edit" ToolTip="Edit" CommandName="Edit" />
                                        <asp:Button ID="btnTaxInvoicePrint" runat="server" class="btn_print" ToolTip="Tax Invoice Print"
                                            CommandName="TaxInvoicePrint" />
                                        <asp:Button ID="btnSalesOrderPrint" runat="server" class="btn_print" ToolTip="Sales Order Print"
                                            CommandName="SalesOrderPrint" />
                                        <asp:Button ID="btnReceiptPrint" runat="server" class="btn_print" ToolTip="Receipt Print"
                                            CommandName="Print" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td colspan="6" class="navigationRow">
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
                                                Quick Receipt/إيصال
                                            </div>
                                            <table class="formTable">
                                                <tr>
                                                    <td style="width: 33%">
                                                        Invoice Code / رمز الفاتورة
                                                        <asp:TextBox ID="lbl_Code" runat="server" class="txt read_Only" Font-Bold="true"
                                                            Text=""></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        Receipt Code / رمز الاستلام
                                                        <asp:TextBox ID="lbl_RecCode" runat="server" class="txt read_Only" Font-Bold="true"
                                                            Text=""></asp:TextBox>
                                                    </td>
                                                    <td rowspan="3">
                                                        <asp:UpdatePanel ID="Upd_CreditDetail_Panel" runat="server" ChildrenAsTriggers="false"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Panel ID="pnl_CreditDetail" runat="server" Visible="false">
                                                                    <table class="listTable">
                                                                        <thead>
                                                                            <tr>
                                                                                <th>
                                                                                    Credit Detail / تفاصيل الائتمان
                                                                                </th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td>
                                                                                    Credit Available الائتمان المتاح
                                                                                    <asp:HiddenField ID="hdn_IsCredit" runat="server" Value="" />
                                                                                    <asp:HiddenField ID="hdn_CurrentInvoiceReceivable" runat="server" Value="" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    Credit Limit الحد الائتماني
                                                                                    <asp:Label ID="lblCreditLimit" runat="server"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    Current Credit Amount مبلغ الائتمان الحالي
                                                                                    <asp:Label ID="lblCurrentCreditAmt" runat="server"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
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
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rbTaxInvoice" Name="rbInputType" ClientIDMode="Static" runat="server"
                                                            GroupName="InvoiceType" />Tax Invoice/فاتورة ضريبية
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rbNormalInvoice" Name="rbInputType" ClientIDMode="Static" runat="server"
                                                            GroupName="InvoiceType" />Normal Invoice/فاتورة عادية
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Quotation/اقتباس<br />
                                                        <asp:UpdatePanel ID="UpdQuotationPanel" runat="server" ChildrenAsTriggers="false"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <telerik:RadComboBox ID="drp_quot" Sort="Ascending" Filter="Contains" runat="server"
                                                                    AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                                    OnClientBlur="ValidateCombo" EmptyMessage="Search Quotation..." Style="overflow: hidden;
                                                                    width: 96%; border: none!important;" AutoPostBack="true" OnSelectedIndexChanged="drp_quo_OnSelectedIndexChanged">
                                                                </telerik:RadComboBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Templates/قوالب<br />
                                                        <telerik:RadComboBox ID="drpTemplates" Sort="Ascending" Filter="Contains" runat="server"
                                                            AllowCustomText="false" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" RenderMode="Lightweight"
                                                            EmptyMessage="Search Templates..." OnClientFocus="OnClientKeyPressing" AutoPostBack="true"
                                                            OnSelectedIndexChanged="drpTemplatesOnSelectedIndexChanged" Style="overflow: hidden;
                                                            width: 97%; border: none!important;">
                                                        </telerik:RadComboBox>
                                                    </td>
                                                    <td>
                                                    </td>
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
                                                                                    Sl./رقم
                                                                                </th>
                                                                                <th style="width: 23%" colspan="2">
                                                                                    Service / الخدمات
                                                                                </th>
                                                                                <th style="width: 15%">
                                                                                    Particulars / تفاصيل
                                                                                </th>
                                                                                <th style="width: 10%">
                                                                                    Price / السعر
                                                                                </th>
                                                                                <th style="width: 10%; display: none">
                                                                                    Service Charge / تكلفة الخدمة
                                                                                </th>
                                                                                <th style="width: 10%">
                                                                                    Fine / مبلغ الغرامة
                                                                                </th>
                                                                                <th style="width: 10%">
                                                                                    Discount / خصم
                                                                                </th>
                                                                                <th style="width: 5%">
                                                                                    Qty / الكمية
                                                                                </th>
                                                                                <th style="width: 7%">
                                                                                    Tax / ضريبة
                                                                                </th>
                                                                                <th style="width: 10%">
                                                                                    Amt With Tax / ضريبة
                                                                                </th>
                                                                                <th style="width: 8%">
                                                                                    Total / مجموع
                                                                                </th>
                                                                                <th style="width: 10%">
                                                                                    Action/عمل
                                                                                </th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <asp:Repeater ID="rpt_Item_list" runat="server" OnItemDataBound="rptitemlistDatabound">
                                                                                <ItemTemplate>
                                                                                    <tr style="text-align: center">
                                                                                        <td>
                                                                                            <%# Container.ItemIndex + 1 %>
                                                                                        </td>
                                                                                        <td style="text-align: left" colspan="2">
                                                                                            <asp:HiddenField ID="hdnInvDId" runat="server" Value='<%#Eval("D_id") %>' />
                                                                                            <asp:HiddenField ID="hdnInvDDepartmentId" runat="server" Value='<%#Eval("DepartmentId") %>' />
                                                                                            <asp:HiddenField ID="hdnInvDCategoryId" runat="server" Value='<%#Eval("CategoryId") %>' />
                                                                                            <asp:HiddenField ID="hdnInvDSerSubCategoryId" runat="server" Value='<%#Eval("ServiceSubCategoryId") %>' />
                                                                                            <asp:HiddenField ID="hdnInvDServiceId" runat="server" Value='<%#Eval("Service_Id") %>' />
                                                                                            <asp:HiddenField ID="hdnInvDDepartment" runat="server" Value='<%#Eval("DepartmentName") %>' />
                                                                                            <asp:HiddenField ID="hdnInvDSerCategory" runat="server" Value='<%#Eval("SerCategoryName") %>' />
                                                                                            <asp:HiddenField ID="hdnInvDSerSubCategory" runat="server" Value='<%#Eval("SerSubCategoryName") %>' />
                                                                                            <asp:Label ID="lblServiceFullName" runat="server" TabIndex="-1" Text='<%#Eval("ServiceFullName") %>' />
                                                                                        </td>
                                                                                        <td style="text-align: left">
                                                                                            <asp:Label ID="lblInvDdesc" Width="95%" TabIndex="-1" runat="server" Text='<%#Eval("Particulars") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td style="text-align: left">
                                                                                            <asp:TextBox ID="txtInvDDisplayPrice" class="txt unit_amtD read_Only numbers_only asLabel"
                                                                                                Width="85%" runat="server" Text='<%#Eval("DisplayPrice") %>' TabIndex="-1"></asp:TextBox>
                                                                                            <asp:HiddenField ID="hdnInvDExpense" ClientIDMode="Static" runat="server" Value='<%#Eval("Expense") %>' />
                                                                                            <asp:HiddenField ID="hdnInvDServiceCharge" ClientIDMode="Static" runat="server" Value='<%#Eval("ServiceCharge") %>' />
                                                                                            <asp:HiddenField ID="hdnInvDPrice" ClientIDMode="Static" runat="server" Value='<%#Eval("Price") %>' />
                                                                                        </td>
                                                                                        <td style="text-align: left; display: none">
                                                                                            <asp:TextBox ID="txtInvDAddServiceCharge" class="txt serCharge_amtD read_Only numbers_only asLabel"
                                                                                                Width="85%" runat="server" Text='<%#Eval("AdditionalServiceCharge") %>' TabIndex="-1"></asp:TextBox>
                                                                                        </td>
                                                                                        <td style="text-align: left">
                                                                                            <asp:TextBox ID="txtInvDFine" class="txt fine_amtD read_Only numbers_only asLabel"
                                                                                                Width="85%" runat="server" Text='<%#Eval("Fine") %>' TabIndex="-1"></asp:TextBox>
                                                                                            <asp:HiddenField ID="hdnInvDFineApplicable" ClientIDMode="Static" runat="server"
                                                                                                Value='<%#Eval("FineApplicable") %>' />
                                                                                        </td>
                                                                                        <td style="text-align: left">
                                                                                            <asp:TextBox ID="txtInvDdiscount" class="read_Only discountD InvDdiscount asLabel txt"
                                                                                                Width="85%" runat="server" Text='<%#Eval("Discount") %>'></asp:TextBox>
                                                                                        </td>
                                                                                        <td style="text-align: left">
                                                                                            <asp:TextBox ID="txtInvDQty" class="numbers_only qtyD  InvDQty txt read_Only asLabel"
                                                                                                Width="75%" runat="server" Text='<%#Eval("Quantity") %>' TabIndex="-1"></asp:TextBox>
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
                                                                                            <asp:TextBox ID="txtInvDTotal" TabIndex="-1" class="numbers_only il_tot_amtD invtot read_Only txt asLabel"
                                                                                                Width="95%" runat="server" Text='<%#Eval("Total") %>'></asp:TextBox>
                                                                                        </td>
                                                                                        <td style="text-align: center;">
                                                                                            <asp:Button ID="btn_edit_line" runat="server" OnClick="btn_edit_line_OnClick" ToolTip="Edit"
                                                                                                class="btn_edit" />
                                                                                                 <asp:Button ID="btnCompleSC" CssClass="btn_completeTick" runat="server" OnClick="btnCompleSC_OnClick"
                                                                                                ToolTip="Service Completion" />
                                                                                            <asp:Button ID="btn_remove_line" CommandName="Delete" class="btn_delete" runat="server"
                                                                                                ToolTip="Delete" OnClick="btn_remove_line_OnClick" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');" />
                                                                                        </td>
                                                                                        <td style="display: none">
                                                                                            <asp:TextBox ID="txtExpenseQty" runat="server" Text='<%#Eval("ExpQty") %>'></asp:TextBox>
                                                                                            <asp:TextBox ID="txtExpenseSinglAmt" runat="server" Text='<%#Eval("ExpSinglAmt") %>'></asp:TextBox>
                                                                                            <asp:TextBox ID="txtExpenseTotalAmt" runat="server" Text='<%#Eval("ExpTotAmt") %>'></asp:TextBox>
                                                                                            <telerik:RadDatePicker ID="ExpenseSerComDate" runat="server" DbSelectedDate='<%#Eval("SerComDate") %>'
                                                                                                DateInput-DateFormat="dd/MM/yyyy">
                                                                                                <Calendar runat="server" ID="Calendaree1" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                                                                    ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                                                                    <SpecialDays>
                                                                                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                                                        </telerik:RadCalendarDay>
                                                                                                    </SpecialDays>
                                                                                                </Calendar>
                                                                                            </telerik:RadDatePicker>
                                                                                            <asp:Repeater ID="rptTransCode" runat="server">
                                                                                                <ItemTemplate>
                                                                                                    <table>
                                                                                                        <asp:TextBox ID="txtTransCode" runat="server" Text='<%#Eval("TransAction/عملNumber") %>'></asp:TextBox>
                                                                                                    </table>
                                                                                                </ItemTemplate>
                                                                                            </asp:Repeater>
                                                                                            <asp:Repeater ID="rptexpensein" runat="server">
                                                                                                <ItemTemplate>
                                                                                                    <table>
                                                                                                        <asp:TextBox ID="txtInvDId" runat="server" Text='<%#Eval("InvDId") %>'></asp:TextBox>
                                                                                                        <asp:TextBox ID="txtSerComDetailId" runat="server" Text='<%#Eval("SerComDetailId") %>'></asp:TextBox>
                                                                                                        <asp:TextBox ID="txtExpenseId" runat="server" Text='<%#Eval("ExpenseId") %>'></asp:TextBox>
                                                                                                        <asp:TextBox ID="txtAmount" runat="server" Text='<%#Eval("Amount") %>'></asp:TextBox>
                                                                                                        <asp:TextBox ID="txtVAT" runat="server" Text='<%#Eval("VAT") %>'></asp:TextBox>
                                                                                                        <asp:TextBox ID="txtVendorId" runat="server" Text='<%#Eval("VendorId") %>'></asp:TextBox>
                                                                                                        <asp:TextBox ID="txtPayModeId" runat="server" Text='<%#Eval("PayModeId") %>'></asp:TextBox>
                                                                                                        <asp:TextBox ID="txtAccountId" runat="server" Text='<%#Eval("AccountId") %>'></asp:TextBox>
                                                                                                        <asp:TextBox ID="txtPayableAmount" runat="server" Text='<%#Eval("PayableAmount") %>'></asp:TextBox>
                                                                                                        <asp:TextBox ID="txtPaidAmount" runat="server" Text='<%#Eval("PaidAmount") %>'></asp:TextBox>
                                                                                                    </table>
                                                                                                </ItemTemplate>
                                                                                            </asp:Repeater>
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>
                                                                            <tr style="text-align: center">
                                                                                <td>
                                                                                    <asp:Label ID="lblRepeaterSNo" Text="" TabIndex="-1" runat="server" />
                                                                                </td>
                                                                                <td style="text-align: left">
                                                                                    <asp:HiddenField ID="hdn_InvDetailId" runat="server" Value="" />
                                                                                    <asp:UpdatePanel ID="UpdDepartmentDropdown" runat="server" ChildrenAsTriggers="false"
                                                                                        UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <telerik:RadComboBox ID="drpDepartment" Sort="Ascending" Filter="Contains" runat="server"
                                                                                                AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Department..."
                                                                                                OnSelectedIndexChanged="drpFilter_OnSelectedIndexChanged" AutoPostBack="true"
                                                                                                ClientIDMode="AutoID" Style="overflow: hidden; width: 85%; border: none!important;"
                                                                                                OnClientFocus="OnClientKeyPressing" OnClientBlur="ValidateCombo">
                                                                                            </telerik:RadComboBox>
                                                                                            <asp:HiddenField ID="hdnDepartment" runat="server" Value="" />
                                                                                            <asp:HiddenField ID="hdnDepartmentId" runat="server" Value="" />
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                    <asp:UpdatePanel ID="UpdSerCategoryDropdown" runat="server" ChildrenAsTriggers="false"
                                                                                        UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <telerik:RadComboBox ID="drpSerCategory" Sort="Ascending" Filter="Contains" runat="server"
                                                                                                AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Category..."
                                                                                                OnSelectedIndexChanged="drpFilter_OnSelectedIndexChanged" AutoPostBack="true"
                                                                                                ClientIDMode="AutoID" Style="overflow: hidden; width: 85%; border: none!important;"
                                                                                                OnClientFocus="OnClientKeyPressing" OnClientBlur="ValidateCombo">
                                                                                            </telerik:RadComboBox>
                                                                                            <asp:HiddenField ID="hdnSerCategory" runat="server" Value="" />
                                                                                            <asp:HiddenField ID="hdnSerCategoryId" runat="server" Value="" />
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td style="text-align: left">
                                                                                    <asp:UpdatePanel ID="UpdSerSubCategoryDropdown" runat="server" ChildrenAsTriggers="false"
                                                                                        UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <telerik:RadComboBox ID="drpSerSubCategory" Sort="Ascending" Filter="Contains" runat="server"
                                                                                                AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Sub Category..."
                                                                                                OnSelectedIndexChanged="drpFilter_OnSelectedIndexChanged" AutoPostBack="true"
                                                                                                ClientIDMode="AutoID" Style="overflow: hidden; width: 85%; border: none!important;"
                                                                                                OnClientFocus="OnClientKeyPressing" OnClientBlur="ValidateCombo">
                                                                                            </telerik:RadComboBox>
                                                                                            <asp:HiddenField ID="hdnSerSubCategory" runat="server" Value="" />
                                                                                            <asp:HiddenField ID="hdnSerSubCategoryId" runat="server" Value="" />
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                    <asp:UpdatePanel ID="UpdServiceDropdown" runat="server" ChildrenAsTriggers="false"
                                                                                        UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <telerik:RadComboBox ID="drpService" Sort="Ascending" Filter="Contains" runat="server"
                                                                                                AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Service..."
                                                                                                OnSelectedIndexChanged="drpService_OnSelectedIndexChanged" AutoPostBack="true"
                                                                                                ClientIDMode="AutoID" Style="overflow: hidden; width: 85%; border: none!important;"
                                                                                                OnClientFocus="OnClientKeyPressing" OnClientBlur="ValidateCombo">
                                                                                            </telerik:RadComboBox>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="drpService"
                                                                                                ValidationGroup="addService" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                                                InitialValue=""></asp:RequiredFieldValidator>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td style="text-align: left">
                                                                                    <asp:UpdatePanel ID="UpdTxtDescription" runat="server" ChildrenAsTriggers="false"
                                                                                        UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_desc" Width="95%" CssClass="txt" runat="server" Text=""></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td style="text-align: right">
                                                                                    <asp:UpdatePanel ID="UpdTxtPrice" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_displayPrice" Style="text-align: right" class="numbers_only unit_amt inline txt"
                                                                                                Width="85%" runat="server" Text=""></asp:TextBox>
                                                                                            <asp:HiddenField ID="hdn_expn" ClientIDMode="Static" runat="server" Value="" />
                                                                                            <asp:HiddenField ID="hdn_sc" ClientIDMode="Static" runat="server" Value="" />
                                                                                            <asp:HiddenField ID="hdnPrice" ClientIDMode="Static" runat="server" Value="" />
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ErrorMessage="*" runat="server"
                                                                                                ControlToValidate="txt_displayPrice" ValidationGroup="addService" Style="color: Red"
                                                                                                Display="Dynamic"></asp:RequiredFieldValidator>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td style="text-align: left; display: none">
                                                                                    <asp:UpdatePanel ID="UpdTxtServiceCharge" runat="server" ChildrenAsTriggers="false"
                                                                                        UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txtServiceCharge" Style="text-align: right" class="numbers_only serCharge_amt inline txt"
                                                                                                Width="85%" runat="server" Text=""></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td style="text-align: right">
                                                                                    <asp:UpdatePanel ID="UpdTxtFine" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txtFine" Style="text-align: right" class="numbers_only fine_amt inline txt"
                                                                                                Width="85%" runat="server" Text=""></asp:TextBox>
                                                                                            <asp:HiddenField ID="hdnFineApplicable" ClientIDMode="Static" runat="server" Value="" />
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td style="text-align: right">
                                                                                    <asp:UpdatePanel ID="Updtxt_discount" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_discount" Style="text-align: right" class="numbers_only discount inline txt"
                                                                                                Width="85%" runat="server"></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td style="text-align: right">
                                                                                    <asp:UpdatePanel ID="UpdTxtQty" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_Qty" Style="text-align: right" class="numbers_only qty inline txt"
                                                                                                Width="75%" runat="server" Text=""></asp:TextBox>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="*" runat="server"
                                                                                                ControlToValidate="txt_Qty" ValidationGroup="addService" Style="color: Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td style="text-align: left">
                                                                                    <asp:UpdatePanel ID="UpdTxtTaxAmt" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox TabIndex="-1" ID="txt_taxamt" Style="text-align: right" class="numbers_only taxamt read_Only txt asLabel"
                                                                                                Width="95%" runat="server" Text=""></asp:TextBox>
                                                                                            <asp:HiddenField ID="hdn_tax" ClientIDMode="Static" runat="server" Value="" />
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td style="text-align: right">
                                                                                    <asp:UpdatePanel ID="UpdTxtPriceWithTax" runat="server" ChildrenAsTriggers="false"
                                                                                        UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_PriceWitTax" Style="text-align: right" TabIndex="-1" class="numbers_only Prc_amt read_Only txt asLabel"
                                                                                                Width="95%" runat="server" Text=""></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td style="text-align: right">
                                                                                    <asp:UpdatePanel ID="UpdTxtTotPrice" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_totPrice" Style="text-align: right" TabIndex="-1" class="numbers_only il_tot_amt read_Only txt asLabel"
                                                                                                Width="95%" runat="server" Text=""></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td style="text-align: center;">
                                                                                    <asp:Button ID="btn_new_line" runat="server" OnClick="btn_new_line_OnClick" ToolTip="Add"
                                                                                        class="btn_add_new" ValidationGroup="addService" />
                                                                                </td>
                                                                                <%-- </ContentTemplate>
                                                                                        </asp:UpdatePanel>--%>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="8">
                                                                                </td>
                                                                                <td colspan="2" style="text-align: right">
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
                                                                                <td colspan="8">
                                                                                </td>
                                                                                <td colspan="2" style="text-align: right">
                                                                                    Total / مجموع
                                                                                </td>
                                                                                <td colspan="2">
                                                                                    <asp:UpdatePanel ID="Updtxt_grand" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox TabIndex="-1" Style="border: medium none; color: Red; font-size: 24px;
                                                                                                text-align: right; width: 95%" class="txt tot_grnd_amt readOnly txt_80" ID="txt_grand"
                                                                                                runat="server"></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="8">
                                                                                </td>
                                                                                <td colspan="2" style="text-align: right">
                                                                                    Pending / قيد الانتظار
                                                                                </td>
                                                                                <td colspan="2" style="text-align: right">
                                                                                    <asp:UpdatePanel ID="Updtxt_pendingAmt" runat="server" ChildrenAsTriggers="false"
                                                                                        UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox TabIndex="-1" Style="border: medium none; color: Red; font-size: 24px;
                                                                                                text-align: right; width: 95%" class="txt pendingAmt readOnly txt_80" ID="txt_pendingAmt"
                                                                                                runat="server"></asp:TextBox>
                                                                                            <asp:HiddenField ID="hdn_receivedAmt" runat="server" Value="0" ClientIDMode="Static" />
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="8">
                                                                                </td>
                                                                                <td colspan="2" style="text-align: right">
                                                                                    Paid Amount / المبلغ المدفوع<span style="color: Red">&nbsp*</span>
                                                                                </td>
                                                                                <td colspan="2" style="text-align: right;">
                                                                                    <asp:UpdatePanel ID="Updtxt_amtPayNow" runat="server" ChildrenAsTriggers="false"
                                                                                        UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox class="txt amtPayNow numbers_only txt_80" Style="text-align: right"
                                                                                                ID="txt_amtPayNow" runat="server"></asp:TextBox>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Required"
                                                                                                runat="server" ControlToValidate="txt_amtPayNow" ValidationGroup="save" InitialValue=""
                                                                                                Style="color: Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="8">
                                                                                </td>
                                                                                <td colspan="2" style="text-align: right">
                                                                                    Bank Commission/عمولة البنك
                                                                                </td>
                                                                                <td colspan="2">
                                                                                    <asp:UpdatePanel ID="upd_commsn" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox class="txt comssnAmt numbers_only txt_80" ID="txt_commsn" runat="server"></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="8">
                                                                                </td>
                                                                                <td colspan="2" style="text-align: right">
                                                                                    Received Amount / المبلغ الذي تسلمه<span style="color: Red">&nbsp*</span>
                                                                                </td>
                                                                                <td colspan="2" style="text-align: right">
                                                                                    <asp:UpdatePanel ID="Updtxt_ReceivedAmt" runat="server" ChildrenAsTriggers="false"
                                                                                        UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox class="txt rAmt numbers_only txt_80" Style="text-align: right" ID="txt_ReceivedAmt"
                                                                                                runat="server"></asp:TextBox>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ErrorMessage="Required"
                                                                                                runat="server" ControlToValidate="txt_ReceivedAmt" ValidationGroup="save" InitialValue=""
                                                                                                Style="color: Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="8">
                                                                                </td>
                                                                                <td colspan="2" style="text-align: right">
                                                                                    Balance / توازن
                                                                                </td>
                                                                                <td colspan="2">
                                                                                    <asp:UpdatePanel ID="Updtxt_Balance" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox TabIndex="-1" Style="border: medium none; color: Red; font-size: 24px;
                                                                                                text-align: right; width: 95%" class="txt balanceAmt readOnly txt_80" ID="txt_Balance"
                                                                                                runat="server"></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
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
                                                    <td colspan="2" rowspan="5">
                                                        Remarks / ملاحظات
                                                        <asp:TextBox class="txtarea" Style="width: 90%" TextMode="MultiLine" ID="txt_remark"
                                                            runat="server"></asp:TextBox>
                                                    </td>
                                                    
                                                </tr>
                                                <tr>
                                                   <td>
                                                       Card Type <span style="color: Red">&nbsp*</span>
                                                        <asp:UpdatePanel ID="Upd_cardtype" runat="server" ChildrenAsTriggers="false"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <telerik:RadComboBox ID="drpCardType" Sort="Ascending" Filter="Contains" runat="server"
                                                                    AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Card Type..."
                                                                    Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                    OnClientBlur="ValidateCombo" OnSelectedIndexChanged="drp_cardType_OnSelectedIndexChanged"
                                                                    AutoPostBack="true">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Value="1" Text="Customer E-dhirham Card" />
                                                                        <telerik:RadComboBoxItem Value="2" Text="Company E-dhirham Card" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="drpCardType"
                                                            ValidationGroup="save" ErrorMessage="Required" Style="color: Red" InitialValue=""
                                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="Upd_CardAcc" runat="server" ChildrenAsTriggers="false"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Panel ID="pnlCardAcc" Visible="false" runat="server">
                                                                    <asp:Label ID="Label3" runat="server" class="lbl" Text="Company E-dhirham Account/حساب الدرهم الالكتروني للشركة "></asp:Label>
                                                                    <telerik:RadComboBox ID="drp_CardAcc" Sort="Ascending" Filter="Contains" runat="server"
                                                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                                       OnClientBlur="ValidateCombo" EmptyMessage="Search Name..."
                                                                        Style="overflow: hidden; width: 85%; border: none!important;">
                                                                    </telerik:RadComboBox>
                                                                    <asp:RequiredFieldValidator ID="ReqCardAcc" runat="server" ControlToValidate="drp_CardAcc"
                                                                        ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                <td>
                                                        Payment Mode / طريقة الدفع <span style="color: Red">&nbsp*</span>
                                                        <asp:UpdatePanel ID="UpdDrpPaymentModePAnel" runat="server" ChildrenAsTriggers="false"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <telerik:RadComboBox ID="drp_payMode" Sort="Ascending" Filter="Contains" runat="server"
                                                                    AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Payment Mode..."
                                                                    Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                    OnClientBlur="ValidateCombo" OnSelectedIndexChanged="drp_payMode_OnSelectedIndexChanged"
                                                                    AutoPostBack="true">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Value="1" Text="Petty Cash" />
                                                                        <telerik:RadComboBoxItem Value="2" Text="Bank TransAction/عمل" />
                                                                        <telerik:RadComboBoxItem Value="3" Text="Cheque" />
                                                                        <%--<telerik:RadComboBoxItem Value="4" Text="Credit" />--%>
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="drp_payMode"
                                                            ValidationGroup="save" ErrorMessage="Required" Style="color: Red" InitialValue=""
                                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="Upd_PayMode_Panel" runat="server" ChildrenAsTriggers="false"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Panel ID="pnl_PayMode_Panel" Visible="false" runat="server">
                                                                    <asp:Label ID="lblToLabel" runat="server" class="lbl" Text="PettyCash"></asp:Label>
                                                                    <telerik:RadComboBox ID="drpPettyCash" Sort="Ascending" Filter="Contains" runat="server"
                                                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                                       OnClientBlur="ValidateCombo" EmptyMessage="Search Name..."
                                                                        Style="overflow: hidden; width: 85%; border: none!important;">
                                                                    </telerik:RadComboBox>
                                                                    <telerik:RadComboBox ID="drpBankAccount" Sort="Ascending" Filter="Contains" runat="server"
                                                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="onchangedrp_bank" OnClientBlur="ValidateCombo"
                                                                        EmptyMessage="Search Name..." Style="overflow: hidden; width: 85%; border: none!important;"
                                                                        Visible="false">
                                                                    </telerik:RadComboBox>
                                                                    <asp:RequiredFieldValidator ID="rqTo" runat="server" ControlToValidate="drpPettyCash"
                                                                        ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                                    <asp:HiddenField ID="hdn_bankcommsn" ClientIDMode="Static" runat="server" />
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="Upd_Cheque_Panel" runat="server" ChildrenAsTriggers="false"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Panel ID="pnl_Cheque_Panel" Visible="false" runat="server">
                                                                    <table style="width: 100%;">
                                                                        <tr>
                                                                            <td>
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
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="cheque_date"
                                                                                    Display="Dynamic" ValidationGroup="save" ErrorMessage="Required" Style="color: Red"
                                                                                    InitialValue=""></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                Cheque Number / رقم الشيك <span style="color: Red">&nbsp*</span>
                                                                                <asp:TextBox ID="txt_chqNumber" class="txt" runat="server"></asp:TextBox>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txt_chqNumber"
                                                                                    Display="Dynamic" ValidationGroup="save" ErrorMessage="Required" Style="color: Red"
                                                                                    InitialValue=""></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:UpdatePanel ID="Upd_total" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_PageName" runat="server" Value="Invoice" />
                                                                <%--Regarding Customer User Control--%>
                                                                <asp:HiddenField ID="hdn_user_id" runat="server" />
                                                                <asp:HiddenField ID="hdnLanguage" runat="server" />
                                                                <asp:HiddenField ID="hdnSCInInvoice" ClientIDMode="Static" runat="server" />
                                                                <asp:HiddenField ID="hdnInvoiceStatus" ClientIDMode="Static" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdnSerPriceWTax" ClientIDMode="Static" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdnTaxAppliedWithDiscount" ClientIDMode="Static" runat="server" />
                                                                <asp:HiddenField ID="hdnDefaultInvoiceType" ClientIDMode="Static" runat="server" />
                                                                <asp:Button ID="btn_save" class="butn_save" ValidationGroup="save" OnClick="btn_save_OnClick"
                                                                    runat="server" Text="Save/حفظ" />
                                                                <asp:Button ID="btn_Salesprint" class="butn" ValidationGroup="save" OnClick="btn_Salesprint_OnClick"
                                                                    runat="server" Text="Sales Order Print/حفظ وطباعة" />
                                                                <asp:Button ID="btn_ReceiptPrint" class="butn" runat="server" ValidationGroup="save"
                                                                    Text="Receipt Print" OnClick="btn_ReceiptPrint_OnClick" />
                                                                <asp:Button ID="btn_cancel" class="butn_delete" runat="server" OnClientClick="javascript : return confirm('Do you really want to Cancel.. ?');"
                                                                    Visible="false" Text="Cancel/إلغاء" OnClick="btn_Cancelmain_OnClick" />
                                                                <asp:Button ID="btn_history" class="butn" runat="server" Visible="false" Text="History/سجل"
                                                                    OnClick="btn_histry_OnClick" />
                                                                <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_OnClick" />
                                                                <asp:Button ID="Button1" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_OnClick" />
                                                                <asp:UpdatePanel ID="Upd_btnTaxInvoicePrint" runat="server" ChildrenAsTriggers="false"
                                                                    UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:Button ID="btn_TaxInvoicePrint" class="butn" runat="server" Text="Tax Invoice Print/طباعة الفاتورة الضريبية "
                                                                            OnClick="btn_TaxInvoicePrint_OnClick" />
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_update" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_Salesprint" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_Receiptprint" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_cancel" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_histry" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_TaxInvoicePrint" runat="server" Value="0" />
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
        <asp:UpdatePanel ID="Upd_Customer_Panel" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional" style="height: 100%;">
            <ContentTemplate>
                <asp:Panel ID="pnl_Customer" Visible="false" runat="server">
                    <AmarCentre:CustomerMaster ID="UC_Customer" runat="server" />
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

     <%--  SC--%>
 <asp:UpdatePanel ID="UpdSC" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlSC" Visible="false" runat="server">
                    <div class="popupBackground">
                    </div>
                    <div class="animated largePopUp" style="width:90%">
                        <asp:UpdatePanel ID="UpdSCIn" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="Adding_heading">
                                    Service Completion / استكمال الخدمة
                                </div>
                                <div id="div2" runat="server" style="width: 100%; overflow: auto;">
                                    <asp:UpdatePanel ID="Upd_ItemList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table class="listTable">
                                                <thead>
                                                    <tr style="text-align: center">
                                                        <th style="width: 30%">
                                                            Service / الخدمات
                                                        </th>
                                                        <th style="width: 9%">
                                                            Invoice Quantity / كمية الفاتورة
                                                        </th>
                                                        <th style="width: 9%">
                                                            Pending Quantity / الكمية المعلقة
                                                        </th>
                                                        <th style="width: 9%">
                                                            Quantity / كمية
                                                        </th>
                                                        <th style="width: 10%">
                                                            Amount For Single Qty / المبلغ للكمية الواحدة
                                                        </th>
                                                        <th style="width: 9%">
                                                            Total Amount / المبلغ الإجمالي
                                                        </th>
                                                        <th style="width: 9%">
                                                            Date / تاريخ
                                                        </th>
                                                        <th style="width: 7%">
                                                            Action/عمل
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr style="text-align: center">
                                                        <td style="text-align: left">
                                                            <asp:HiddenField ID="hdn_service_id" runat="server" />
                                                            <asp:HiddenField ID="hdn_ExpinvD_id" runat="server" />
                                                            <asp:Label ID="lbl_service" runat="server"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:TextBox ID="txt_InvQty" class="numbers_only read_Only invQty txt asLabel" Width="75%"
                                                                TabIndex="-1" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:TextBox ID="txt_InComQty" class="numbers_only read_Only inComQty inline txt asLabel"
                                                                TabIndex="-1" Width="75%" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:TextBox ID="txtInlineQty" class="numbers_only txt inlineQty" Width="75%" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RqtxtQty" runat="server" ControlToValidate="txtInlineQty"
                                                                ValidationGroup="inlineSave" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                InitialValue="">
                                                            </asp:RequiredFieldValidator>
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:TextBox ID="txtInlineAmtSQty" class="numbers_only read_Only asLabel txt inlineamtSQty"
                                                                TabIndex="-1" Width="75%" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:TextBox ID="txtInlineTotAmt" class="numbers_only read_Only asLabel txt inlinetotAmt"
                                                                TabIndex="-1" Width="75%" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <telerik:RadDatePicker ID="InlineSerComDate" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                                <Calendar runat="server" ID="Calendar41" CssClass="rtlSupport" ShowOtherMonthsDays="False"
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
                                                        <td>
                                                         <asp:Button ID="btn_expDetail_line" CssClass="btn_edit" runat="server" OnClick="btn_expDetail_line_OnClick"
                                                                        ToolTip="Edit" />
                                                            <asp:Button ID="btnInlineSave" CssClass="btn_completeTick" runat="server" OnClick="btnInlineExpenseSave_OnClick"
                                                                ValidationGroup="inlineSave" ToolTip="Complete" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <table class="ActionTable">
                                    <tr>
                                        <td colspan="4" rowspan="3" style="text-align: right">
                                            <asp:Button ID="Button11" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_closeSC_OnClick" />
                                        </td>
                                    </tr>
                                </table>
                                <div>
                                    <div id="div3" class="messageAlert div_pop animated" style="display: none" runat="server">
                                        <div class="tick">
                                            &#10004
                                        </div>
                                        <div>
                                            <asp:Label ID="Label1" runat="server" class="messageLabel"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <asp:UpdatePanel ID="Upd_Expense_Panel" runat="server" ChildrenAsTriggers="false"
                                    UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnl_Expense_Panel" Visible="false" runat="server">
                                            <table class="listTable">
                                                <thead>
                                                    <tr>
                                                        <th style="width: 200px">
                                                            Expense / مصروف
                                                        </th>
                                                        <th style="width: 100px">
                                                            Amount / المبلغ
                                                        </th>
                                                        <th style="width: 100px">
                                                            VAT / ضريبة
                                                        </th>
                                                        <th>
                                                            Vendor / بائع
                                                        </th>
                                                        <th>
                                                            Payment Mode / طريقة الدفع
                                                        </th>
                                                        <th>
                                                            Account / الحساب
                                                        </th>
                                                        <th style="width: 100px">
                                                            Payable Amount / المبلغ المستحق
                                                        </th>
                                                        <th style="width: 100px">
                                                            Paid Amount / المبلغ المدفوع
                                                        </th>
                                                      <%--  <th>
                                                            Action/عمل
                                                        </th>--%>
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
                                                                <asp:HiddenField ID="hdn_vendorId" runat="server" Value='<%#Eval("VendorId") %>' />
                                                                <telerik:RadComboBox ID="drp_vendor" Sort="Ascending" Filter="Contains" runat="server"
                                                                    AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Vendor..."
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
                                                                    AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Payment Mode..."
                                                                    Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                    OnClientBlur="ValidateCombo" ClientIDMode="AutoID" OnSelectedIndexChanged="drp_payMode_OnSelectedIndexChangedSC"
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
                                                                        <telerik:RadComboBox ID="drp_account" Sort="Ascending" Filter="Contains" runat="server"
                                                                            AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Account..."
                                                                            Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                            OnClientBlur="ValidateCombo">
                                                                        </telerik:RadComboBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator42" runat="server" ControlToValidate="drp_account"
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
                                                                <asp:TextBox ID="txt_paidAmount" Class="txt numbers_only paidAmount" runat="server"
                                                                    Text='<%#Eval("PaidAmount") %>'></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_paidAmount"
                                                                    ValidationGroup="save" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                    InitialValue="">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                          <%--  <td>
                                                                <asp:Button ID="btnInlineEdit" runat="server" OnClick="btnInlineEdit_OnClick" ToolTip="Edit"
                                                                    class="btn_edit" />
                                                            </td>--%>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                            <table class="formTable">
                                                <tr>
                                                    <td style="width: 25%">
                                                        Quantity / كمية <span style="color: Red">&nbsp*</span>
                                                    </td>
                                                    <td style="width: 25%">
                                                        <asp:HiddenField ID="hdn_InComQty" runat="server" Value="0" ClientIDMode="Static" />
                                                        <asp:TextBox ID="txtscqty" class="numbers_only scQty" Width="75%" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="txtscqty"
                                                            ValidationGroup="savesc" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                            InitialValue="">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td style="width: 25%">
                                                    </td>
                                                    <td style="width: 25%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        Date / تاريخ<span style="color: Red">&nbsp*</span>
                                                    </td>
                                                    <td style="width: 25%">
                                                        <telerik:RadDatePicker ID="SerComDate" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                            <Calendar runat="server" ID="Calendar3" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                                ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                                <SpecialDays>
                                                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                    </telerik:RadCalendarDay>
                                                                </SpecialDays>
                                                            </Calendar>
                                                        </telerik:RadDatePicker>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="SerComDate"
                                                            ValidationGroup="savesc" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                            InitialValue="">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td style="width: 25%">
                                                    </td>
                                                    <td style="width: 25%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Amount For Single Qty / المبلغ للكمية الواحدة
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_amtSQty" class="numbers_only read_Only amtSQty" Width="75%"
                                                            runat="server"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Total Amount / المبلغ الإجمالي
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_totAmt" class="numbers_only read_Only totAmt" Width="75%" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Button ID="Button12" class="butn_save" ValidationGroup="savesc" OnClick="btn_saveSC_OnClick"
                                                runat="server" Text="Save/حفظ" />
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </asp:Panel>
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
                                            <div id="div5" runat="server" style="width: 100%; overflow: auto;">
                                                <div style="height: 10px">
                                                </div>
                                                <table class="listTable">
                                                    <thead>
                                                        <tr style="text-align: center">
                                                            <th style="width: 3%">
                                                                Sl.No
                                                            </th>
                                                            <th style="width: 10%">
                                                                Transaction Number / رقم التحويلة
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
                                                                        <asp:TextBox ID="txt_transNumber" class="txt" Width="75%" runat="server" Text='<%#Eval("TransAction/عملNumber") %>'></asp:TextBox>
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
                                            <div>
                                                <asp:Button ID="btn_FinalSave" runat="server" class="butn_save" ValidationGroup="finalsave"
                                                    Text="Save/حفظ" OnClick="btn_FinalSave_OnClick" />
                                                <asp:Button ID="btn_TransDetail_Close" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_TransDetail_Close_OnClick" />
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
   

</asp:Content>
