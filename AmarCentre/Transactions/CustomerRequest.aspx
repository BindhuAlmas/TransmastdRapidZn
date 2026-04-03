<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="CustomerRequest.aspx.cs" Inherits="AmarCentre.Transactions.CustomerRequest" %>

<%@ Register Src="~/Transactions/UserControl/UCInvoice.ascx" TagName="InvoiceUC"
    TagPrefix="AmarCentre" %>
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
        }


        function OnClientFileUploaded(sender, args) {
            var info = args.get_fileInfo();
            if (info.CustomError != null) {
                alert(info.CustomError);
            }
        }


    </script>
    <script type="text/javascript" >

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

            $('.CommissionD ,.CommissionOut').focus(function (e) {
                if ($('input[id$=hdnCustCommsnApplcable]').val() == '0') {
                    $('.CommissionD').attr('readonly', true);
                    $('.CommissionOut').attr('readonly', true);
                }
                else {
                    if ($('input[id$=hdnIsCommissionEditableInInvoice]').val() == '0') {
                        $('.CommissionD').attr('readonly', true);
                        $('.CommissionOut').attr('readonly', true);
                    }
                    else {
                        $('.CommissionD').attr('readonly', false);
                        $('.CommissionOut').attr('readonly', false);
                    }
                }
            });

            //make receipt
            {
                $('.amtPayNow').blur(function (e) {
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
                    if ($('.amtPayNow').val() != '') {
                        AmtPayingNow = parseFloat($('.amtPayNow').val());
                    }

                    if ($('.ChargedAmountRec').val() != '' & $('#hdnpaymenttype').val() == '2') {
                        ChargedAmount = parseFloat($('.ChargedAmountRec').val());
                    }
                    if (parseFloat(PendingAmt) + parseFloat(ChargedAmount) < (parseFloat(AmtPayingNow) + parseFloat(spotcommsn))) {
                        alert('Amount cannot be greater than Pending Amount');
                        $('.amtPayNow').val('');
                        $('.balanceAmt').val($('.rAmt').val());
                        $('.amtPayNow').focus();
                    }

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

                    if ($('.ChargedAmountRec').val() != '' & $('#hdnpaymenttype').val() == '2') {
                        ChargedAmount = parseFloat($('.ChargedAmountRec').val());
                    }
                    AmtPayingNow = parseFloat(PendingAmt) - parseFloat(ChargedAmount) - parseFloat(spotcommsn);
                    $('.amtPayNow').val(AmtPayingNow.toFixed(2));

                    CalcCommsn();
                });

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

            }

            //for prime
            {
                $('.qtyD ,.unit_amtD,.fine_amtD,.discountD,.Expense_amtD').focus(function (e) {
                    if ($('input[id$=hdnIsQuotaionEditablePrime]').val() == '1') {
                        $('.qtyD').attr('readonly', true);
                        $('.unit_amtD').attr('readonly', true);
                        $('.fine_amtD').attr('readonly', true);
                        $('.discountD').attr('readonly', true);
                        $('.Expense_amtD').attr('readonly', true);
                    }
                });

                $('.supchkitem').click(function () {
                    if ($('#hdnIsQuotaionEditablePrime').val() == '1') {
                        Sup_Calculate();
                    }
                });

                function Sup_Calculate() {
                    var total = 0;
                    var isdispdisc = 0;
                    var DiscTotAmt = 0;
                    if ($('#hdn_shwdiscount').val() != '') {
                        isdispdisc = parseInt($('#hdn_shwdiscount').val());
                    }
                    $('.il_tot_amtD').each(function () {
                        var Dis = 0;
                        var totQty = 0;
                        if ($(this).closest('tr').find(':checkbox').prop('checked')) {
                            if ($(this).val() != '') {
                                total = (parseFloat($(this).val()) + parseFloat(total)).toFixed(2);
                            }

                            if (isdispdisc = "1") {
                                if ($(this).closest("tr").find('.InvDdiscount').val() != '') {
                                    Dis = parseFloat($(this).closest("tr").find('.InvDdiscount').val());
                                }
                                if ($(this).closest("tr").find('.InvDQty').val() != '') {
                                    totQty = parseFloat($(this).closest("tr").find('.InvDQty').val());
                                }
                            }
                            DiscTotAmt = parseFloat(DiscTotAmt) + (parseFloat(Dis) * parseFloat(totQty));
                        }
                    });
                    $('.tot_grnd_amt').val(parseFloat(total).toFixed(2));
                    $('.tot_discount').val(parseFloat(DiscTotAmt).toFixed(2));

                }

            }
            //prime end

            //sc section
            {
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
            }
            //sc section end



            $('.Expense_amtD').blur(function (e) {
                var exp = 0;
                if ($(this).closest("tr").find('.Expense_amtD').val() != '') {
                    exp = parseFloat($(this).closest("tr").find('.Expense_amtD').val());
                }
                $(this).closest("tr").find('#hdnInvDExpense').val(exp);
                InsideRepeaterCalculation();
            });

            $('.qtyD ,.unit_amtD,.fine_amtD,.discountD').blur(function (e) {
                InsideRepeaterCalculation();
            });

            $('.CommissionD,.CommissionOut,.AgentCommissionD,.AgentCommissionOut').blur(function (e) {
                Calc();
            });

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
                    var isdispdisc = 0;

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
                    if ($('#hdn_shwdiscount').val() != '') {
                        isdispdisc = parseInt($('#hdn_shwdiscount').val());
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
                    //if ($(this).closest("tr").find('.serCharge_amtD').val() != '') {
                    //    TxtSCAmt = parseFloat($(this).closest("tr").find('.serCharge_amtD').val());
                    //}
                    /*Quantity*/
                    if ($(this).closest("tr").find('.qtyD').val() != '') {
                        Qty = parseFloat($(this).closest("tr").find('.qtyD').val());
                    }

                    if (isdispdisc = "1") {
                        if ($(this).closest("tr").find('.discountD').val() != '') {
                            DiscntAmt = parseFloat($(this).closest("tr").find('.discountD').val());
                        }
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

                    //TotAmt = (parseFloat(pricewittax) * parseFloat(Qty)).toFixed(2);
                    TotAmt = ((parseFloat(pricewittax) - parseFloat(DiscntAmt)) * parseFloat(Qty)).toFixed(2);

                    $(this).closest("tr").find('.taxamtD').val(parseFloat(taxamt).toFixed(2));
                    $(this).closest("tr").find('#hdnInvDPrice').val(parseFloat(Price).toFixed(2));
                    $(this).closest("tr").find('#hdnInvDServiceCharge').val(parseFloat(SCamt).toFixed(2));
                    $(this).closest("tr").find('.txtInvDServiceCharge').val(parseFloat(SCamt).toFixed(2));

                    $(this).closest("tr").find('.Prc_amtD').val(parseFloat(pricewittax).toFixed(2));
                    $(this).closest("tr").find('.il_tot_amtD').val(parseFloat(TotAmt).toFixed(2));
                });
                Calc();
            }

            function OutsideRepeaterCalculation() {
                $('.unit_amt').each(function () {
                    var UP = 0;
                    var Qty = 0;
                    var Amt = 0;
                    var TotAmt = 0;
                    var DiscntAmt = 0;
                    var isdispdisc = 0;

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
                    if ($('#hdn_shwdiscount').val() != '') {
                        isdispdisc = parseInt($('#hdn_shwdiscount').val());
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
                    //if ($(this).closest("tr").find('.txtServiceCharge').val() != '') {
                    //    TxtSCAmt = parseFloat($(this).closest("tr").find('.txtServiceCharge').val());
                    //}
                    /*Quantity*/
                    if ($(this).closest("tr").find('.qty').val() != '') {
                        Qty = parseFloat($(this).closest("tr").find('.qty').val());
                    }

                    if (isdispdisc = "1") {
                        if ($(this).closest("tr").find('.discount').val() != '') {
                            DiscntAmt = parseFloat($(this).closest("tr").find('.discount').val());
                        }
                    }

                    /*Expense*/
                    //                    if ($(this).closest("tr").find('#hdn_expn').val() != '') {
                    //                        expamt = parseFloat($(this).closest("tr").find('#hdn_expn').val());
                    //                    }
                    if ($(this).closest("tr").find('.Expense_amt').val() != '') {
                        expamt = parseFloat($(this).closest("tr").find('.Expense_amt').val());
                        $(this).closest("tr").find('#hdn_expn').val($(this).closest("tr").find('.Expense_amt').val());
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

                    //TotAmt = (parseFloat(pricewittax) * parseFloat(Qty)).toFixed(2);
                    TotAmt = ((parseFloat(pricewittax) - parseFloat(DiscntAmt)) * parseFloat(Qty)).toFixed(2);

                    $(this).closest("tr").find('.taxamt').val(parseFloat(taxamt).toFixed(2));
                    $(this).closest("tr").find('#hdnPrice').val(parseFloat(Price).toFixed(2));
                    $(this).closest("tr").find('#hdn_sc').val(parseFloat(SCamt).toFixed(2));
                    $(this).closest("tr").find('.txtServiceCharge').val(parseFloat(SCamt).toFixed(2));

                    $(this).closest("tr").find('.Prc_amt').val(parseFloat(pricewittax).toFixed(2));
                    $(this).closest("tr").find('.il_tot_amt').val(parseFloat(TotAmt).toFixed(2));
                });
                Calc();
            }

            function Calc() {
                var ILTotAmt = 0;
                var GrndTotAmt = 0;
                var PresentTot = 0;

                var DiscTotAmt = 0;
                var ComTotAmt = 0;
                var ComTotAmtag = 0;

                var totQty = 0;
                var Presentqty = 0;
                var PresentDis = 0;
                var GrndDis = 0;
                var GrndCom = 0;
                var PresentCom = 0;
                var PresentComag = 0;

                var isdispdisc = 0;

                if ($('#hdn_shwdiscount').val() != '') {
                    isdispdisc = parseInt($('#hdn_shwdiscount').val());
                }

                $('.invtot').each(function () {
                    var Amt = 0;
                    var Dis = 0;
                    var Com = 0;
                    var Comag = 0;

                    if ($(this).closest("tr").find('.invtot').val() != '') {
                        Amt = parseFloat($(this).closest("tr").find('.invtot').val());
                    }
                    if ($(this).closest("tr").find('.InvDQty').val() != '') {
                        totQty = parseFloat($(this).closest("tr").find('.InvDQty').val());
                    }
                    if ($(this).closest("tr").find('.CommissionD').val() != '') {
                        Com = parseFloat($(this).closest("tr").find('.CommissionD').val());
                    }
                    if ($(this).closest("tr").find('.AgentCommissionD').val() != '') {
                        Comag = parseFloat($(this).closest("tr").find('.AgentCommissionD').val());
                    }
                    if (isdispdisc = "1") {
                        if ($(this).closest("tr").find('.InvDdiscount').val() != '') {
                            Dis = parseFloat($(this).closest("tr").find('.InvDdiscount').val());
                        }
                    }

                    ILTotAmt = parseFloat(ILTotAmt) + parseFloat(Amt);
                    DiscTotAmt = parseFloat(DiscTotAmt) + (parseFloat(Dis) * parseFloat(totQty));
                    ComTotAmt = parseFloat(ComTotAmt) + (parseFloat(Com) * parseFloat(totQty));
                    ComTotAmtag = parseFloat(ComTotAmtag) + (parseFloat(Comag) * parseFloat(totQty));

                });
                if ($('.il_tot_amt').val() != '') {
                    PresentTot = parseFloat($('.il_tot_amt').val());
                }
                if ($('.qty').val() != '') {
                    Presentqty = parseFloat($('.qty').val());
                }
                if ($('.CommissionOut').val() != '') {
                    PresentCom = parseFloat($('.CommissionOut').val());
                }
                if ($('.AgentCommissionOut').val() != '') {
                    PresentComag = parseFloat($('.AgentCommissionOut').val());
                }
                if (isdispdisc = "1") {
                    if ($('.discount').val() != '') {
                        PresentDis = parseFloat($('.discount').val());
                    }
                }

                GrndTotAmt = parseFloat(ILTotAmt) + parseFloat(PresentTot);
                GrndDis = parseFloat(DiscTotAmt) + (parseFloat(PresentDis) * parseFloat(Presentqty));
                GrndCom = parseFloat(ComTotAmt) + (parseFloat(PresentCom) * parseFloat(Presentqty)) +
                    parseFloat(ComTotAmtag) + (parseFloat(PresentComag) * parseFloat(Presentqty));

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
                    $('.txtroundoff').val((parseFloat(Final) - parseFloat(GrndTotAmt)).toFixed(2));
                }
                else {
                    $('.tot_grnd_amt').val(parseFloat(GrndTotAmt).toFixed(2));
                    $('.txtroundoff').val(0);
                }

                $('.tot_discount').val(parseFloat(GrndDis).toFixed(2));
                $('.txtCommssnTotal').val(parseFloat(GrndCom).toFixed(2));

                var txtbankchargeper = 0;
                var txtCharged = 0;
                if ($('.txtbankchargeper').val() != '') {
                    txtbankchargeper = parseFloat($('.txtbankchargeper').val());
                    txtCharged = parseFloat(Final) * (parseFloat(txtbankchargeper) / 100);
                    $('.txtCharged').val(parseFloat(txtCharged).toFixed(2));

                }

                /*End of Amount Round Value*/
            }

            $('.txtbankchargeper').blur(function (e) {
                var txtbankchargeper = 0;
                var txtCharged = 0;
                var Final = 0;
                if ($('.tot_grnd_amt').val() != '') {
                    Final = parseFloat($('.tot_grnd_amt').val());
                }
                if ($('.txtbankchargeper').val() != '') {
                    txtbankchargeper = parseFloat($('.txtbankchargeper').val());
                    txtCharged = parseFloat(Final) * (parseFloat(txtbankchargeper) / 100);
                    $('.txtCharged').val(parseFloat(txtCharged).toFixed(2));
                }
            });

        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Customer Request
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
                            <th class="listTableSlNo" style="width: 5%;">Sl No /رقم
                            </th>
                            <th style="width: 8%;">Code / الشفرة
                            </th>
                             <th style="width: 15%;">Customer / زبون
                            </th>
                            <th style="width: 10%;">Date / تاريخ
                            </th>
                            <th style="width: 10%;">
                                Invoice Code / رمز الفاتورة
                            </th>
                            <th style="width: 10%;">Status / الحالة
                            </th>
                            <th class="listTableAction" style="width: 5%;">Action/عمل
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
                                        <%#Eval("InvoiceCode")%>
                                    </td>
                                    <td>
                                        <%#Eval("StatusName")%>
                                    </td>
                                    <td >
                                        <asp:Button ID="btn_edit" runat="server" class="btn_edit" ToolTip="Edit" CommandName="Edit" />
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
                        <asp:UpdatePanel ID="upd_main" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div id="div_main" runat="server">
                                    <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
                                        UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="Adding_heading">
                                                Request
                                            </div>
                                            <table class="formTable">
                                                <tr>
                                                    <td style="width: 33%">Code 
                                                        <asp:TextBox ID="lbl_Code" runat="server" class="txt read_Only" Font-Bold="true"
                                                            Text=""></asp:TextBox>
                                                    </td>
                                                     <td style="width: 33%">Customer 
                                                        <asp:TextBox ID="txtCustomer" runat="server" class="txt read_Only" Font-Bold="true"
                                                            Text=""></asp:TextBox>
                                                    </td>
                                                    <td style="width: 33%">Date / تاريخ 
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
                                                        
                                                    </td>
                                                   
                                                </tr>
                                                <tr>
                                                     <td>
                                                      Template 
                                                        <telerik:RadComboBox ID="drpTemplates" Sort="Ascending" Filter="Contains" runat="server"
                                                            AllowCustomText="false" RenderMode="Lightweight" Enabled="false"
                                                            EmptyMessage="Search ..." OnClientFocus="OnClientKeyPressing"  Style="overflow: hidden;
                                                            width: 97%; border: none!important;">
                                                        </telerik:RadComboBox>
                                                    </td>
                                                    <td> Applicant
                                                            <asp:TextBox ID="txtApplicant" Width="95%" CssClass="txt" runat="server"></asp:TextBox></td>
                                                    <td ></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblstatus" runat="server" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td colspan="2"></td>
                                                </tr>
                                                <tr>
                                                     <td colspan="2">Remark 
                                                        <asp:TextBox ID="txtremark" runat="server" class="txtarea" TextMode="MultiLine" Text=""></asp:TextBox>
                                                    </td>
                                                    <td></td>
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
                                                                                <th style="width: 3%">Sl./رقم
                                                                                </th>
                                                                                <th style="width: 22%" colspan="2">Department
                                                                                </th>
                                                                                <th style="width: 22%" colspan="2">Service / الخدمات
                                                                                </th>
                                                                                <th style="width: 11%">Applicant Name / اسم التطبيق
                                                                                </th>
                                                                                <th style="width: 7%">Action/عمل
                                                                                </th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <asp:Repeater ID="rpt_Item_list" runat="server" OnItemDataBound="rptitemlistDatabound" OnItemCommand="rpt_Item_list_ItemCommand">
                                                                                <ItemTemplate>
                                                                                    <tr style="text-align: center" runat="server" id="tr_in">
                                                                                        <td style="width: 5%">
                                                                                            <%# Container.ItemIndex + 1 %>
                                                                                            <asp:HiddenField ID="hdnDId" runat="server" Value='<%#Eval("D_id") %>' />
                                                                                        </td>
                                                                                        <td style="text-align: left; width: 15%" colspan="2">
                                                                                            <asp:UpdatePanel ID="UpdDepartment" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                                <ContentTemplate>
                                                                                                    <asp:HiddenField ID="hdnDepartmentId" runat="server" Value='<%#Eval("DepartmentId") %>' />
                                                                                                    <telerik:RadComboBox ID="drpDepartment" Sort="Ascending" Filter="Contains" runat="server"
                                                                                                        AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search ..."
                                                                                                        AutoPostBack="true" OnSelectedIndexChanged="drpDepartmentOnSelectedIndexChanged" ClientIDMode="AutoID"
                                                                                                        Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                                                        OnClientBlur="ValidateCombo">
                                                                                                    </telerik:RadComboBox>
                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>
                                                                                        </td>
                                                                                        <td style="text-align: left; width: 15%" colspan="2">
                                                                                            <asp:UpdatePanel ID="UpdService" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                                <ContentTemplate>
                                                                                                    <asp:HiddenField ID="hdnServiceId" runat="server" Value='<%#Eval("ServiceId") %>' />
                                                                                                    <telerik:RadComboBox ID="drpService" Sort="Ascending" Filter="Contains" runat="server"
                                                                                                        AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search ..."
                                                                                                        AutoPostBack="true" OnSelectedIndexChanged="drpService_OnSelectedIndexChanged" ClientIDMode="AutoID"
                                                                                                        Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                                                        OnClientBlur="ValidateCombo">
                                                                                                    </telerik:RadComboBox>
                                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="drpService"
                                                                                                        ValidationGroup="save_serdetail" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                                                        InitialValue="">
                                                                                                    </asp:RequiredFieldValidator>
                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>
                                                                                        </td>
                                                                                        <td style="text-align: left; width: 10%">
                                                                                            <asp:TextBox ID="txt_Applname" Width="95%" CssClass="txt" runat="server" Text='<%#Eval("ApplicantName") %>'></asp:TextBox>
                                                                                        </td>
                                                                                       
                                                                                        <td style="text-align: center; width: 7%">
                                                                                            <asp:Button ID="Button2" runat="server" CommandName="Add" ToolTip="Add"
                                                                                                ValidationGroup="save_serdetail" class="btn_add_new" />
                                                                                            <asp:Button ID="btn_remove_line" CommandName="Delete" class="btn_delete" runat="server"
                                                                                                ToolTip="Delete" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');" />
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
                                                    </td>
                                                </tr>
                                                  <tr>
                                                    <td colspan="3">
                                                         <asp:UpdatePanel ID="updFileuploadOut" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                             <ContentTemplate>
                                                                 <table class="Intable" style="width: 45%; text-align: left">
                                                                     <tr style="font-weight: bold; font-size:16px">
                                                                         <td style="width: 80%;">Files</td>
                                                                     </tr>
                                                                     <asp:Repeater ID="rptDocumentOut" runat="server" OnItemCommand="rptDocumentOut_ItemCommand">
                                                                         <ItemTemplate>
                                                                             <tr>
                                                                                 <td  style="padding:1%; vertical-align:middle">
                                                                                     <asp:LinkButton ID="lnkin" runat="server" Text='<%#Eval("FileNames") %>' CommandName="Download"></asp:LinkButton>
                                                                                     
                                                                                     <asp:HiddenField ID="hdnFilenameOut" runat="server" Value='<%#Eval("FileNames") %>' />
                                                                                     <asp:HiddenField ID="hdnFilenameSaveOut" runat="server" Value='<%#Eval("FileNamesSave") %>' />
                                                                                 </td>
                                                                             </tr>
                                                                         </ItemTemplate>
                                                                     </asp:Repeater>
                                                                 </table>
                                                             </ContentTemplate>
                                                             <Triggers>
                                                                 <asp:PostBackTrigger ControlID="rptDocumentOut" />
                                                             </Triggers>
                                                         </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hdn_user_id" runat="server" />
                                                        <asp:HiddenField ID="hdnDetailIndexId" runat="server" />
                                                        <asp:HiddenField ID="hdnLanguage" runat="server" />
                                                        <asp:HiddenField ID="hdnRequestStatus" runat="server" />
                                                        <asp:HiddenField ID="hdnreject" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hdnCreateInvoice" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hdnsave" runat="server" Value="0" />

                                                         <asp:Button ID="btnsave" class="butn_save" runat="server" Visible="false" Text="Save" OnClick="btnsaveClick" />
                                                         <asp:Button ID="btnnewReq" class="butn_save" runat="server" Visible="false" Text="Create Invoice" OnClick="btninvoice_Click" />
                                                        <asp:Button ID="btnReject" class="butn" runat="server" Visible="false" Text="History" OnClick="btnhistory_OnClick" />
                                                        <asp:Button ID="Button1" class="butn" runat="server" Text="Close" OnClick="btn_close_OnClick" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <div>
                                                <div id="div1" class="messageAlert div_pop animated" style="display: none" runat="server">
                                                    <div class="tick">
                                                        &#10004
                                                    </div>
                                                    <div>
                                                        <asp:Label ID="lbl_msgin" Font-Bold="true" ForeColor="White" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>

                                <asp:UpdatePanel ID="Updhistory" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlhistry" runat="server" Visible="false">
                                            <div class="popupBackground">
                                            </div>
                                            <div class="animated halfPopUp" style="width: 65%; text-align: left">
                                                <div class="Adding_heading">
                                                    History
                                                </div>
                                                <table class="listTable">
                                                    <thead>
                                                        <tr>
                                                            <th style="text-align: center; width: 5%">Sl/رقم
                                                            </th>
                                                            <th style="text-align: center; width: 25%">Action
                                                            </th>
                                                            <th style="text-align: center; width: 25%">Remark/تعليق
                                                            </th>
                                                            <th style="text-align: center; width: 10%">Done By
                                                            </th>
                                                            <th style="text-align: center; width: 15%">Date/تاريخ
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <asp:Repeater ID="rpt_His" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="text-align: center;">
                                                                    <%# Container.ItemIndex + 1 %>
                                                                </td>
                                                                 <td>
                                                                    <%#Eval("ActionRemark")%>
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

                                                </table>

                                                <asp:Button ID="Button54" class="butn" runat="server" Text="Close/قريب" OnClick="btn_histry_Close_OnClick" />
                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdInvoiceadd" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                 <asp:Panel ID="pnlInvoiceadd" Visible="false" runat="server">
                    <div class="popupBackground">
                    </div>
                    <div class="animated largePopUp">
                       <AmarCentre:InvoiceUC ID="UCInvoice" runat="server" />
                    </div>
                </asp:Panel>
                <div>
                    <div id="div_pop2" class="messageAlert div_pop animated" style="display: none" runat="server">
                        <div class="tick">
                            &#10004</div>
                        <div>
                            <asp:Label ID="lbl_msg" runat="server" class="messageLabel"></asp:Label>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>


