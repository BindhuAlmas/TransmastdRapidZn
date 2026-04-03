<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="AmarCentre.Dashboard" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Src="~/Transactions/UserControl/UCInvoice.ascx" TagName="InvoiceUC" TagPrefix="AmarCentre" %>
<%@ Register Src="~/Transactions/UserControl/UCRV.ascx" TagName="RVUC" TagPrefix="AmarCentre" %>
<%@ Register Src="~/Transactions/UserControl/UCPV.ascx" TagName="PVUC" TagPrefix="AmarCentre" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .container {
            background-color: #f8f8fb;
        }
        /* SECTION */
        .section-title {
            padding: 1%;
            padding-bottom: 0px;
            font-size: 12px;
            color: #777;
            font-weight: bold;
            letter-spacing: 1px;
        }
        /* GRID */
        .card-grid {
            display: grid;
            grid-template-columns: repeat(4,1fr);
            gap: 15px;
            padding: 1%;
        }
        /* ACTION CARDS */
        .noDecorationcc {
            text-decoration: none;
            color: white !important;
            letter-spacing: 1px;
        }

        .action-card {
            padding: 10%;
            border-radius: 10px;
            display: flex;
            flex-direction: column;
            gap: 10px;
            font-weight: bold;
            text-align: center;
        }

        .c1 {
            background: linear-gradient(45deg,#1da1a8,#2c7efc);
        }

        .c2 {
            background: linear-gradient(45deg,#3b2cc7,#7c3aed);
        }

        .c3 {
            background: linear-gradient(45deg,#ff0057,#ff4d88);
        }

        .c4 {
            background: linear-gradient(306deg,#32c9ab,#27ae60);
        }

        .icon-box {
            font-size: 20px;
        }

        /* INFO CARDS */
        .info-card {
            padding: 20px;
            border-radius: 10px;
            color: white;
            font-weight: bold;
            letter-spacing: 1px;
            font-size: 12px;
        }

        .pink {
            background: linear-gradient(45deg,#ff0066,#ff4fa3);
        }

        .green {
            background: linear-gradient(45deg,#1f9d55,#38c172);
        }

        .orange {
            background: linear-gradient(45deg,#f39c12,#f1c40f);
        }

        .blue {
            background: linear-gradient(45deg,#3498db,#5dade2);
        }

        .yellow {
            background: linear-gradient(45deg,#e19113,#f7b731);
        }
        /* BOTTOM GRID */

        .bottom-grid {
            padding: 1%;
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 2%;
        }
        /* PROFIT */

        .profit-card {
            background: white;
            border-radius: 10px;
            padding: 5%;
            box-shadow: 0 2px 10px rgba(0,0,0,0.05);
        }

        .profiticon {
            width: 21px;
            height: 21px;
        }
        /*Bank&Cash*/

        .account-card-container {
            display: grid;
            grid-template-columns: repeat(3, 1fr); /* 3 per row */
            gap: 20px;
            padding: 20px;
        }

        /* Card */
        .account-card {
            display: flex;
            align-items: center;
            padding: 15px;
            border-radius: 16px;
            background: #ffffff;
            box-shadow: 0 4px 12px rgba(0,0,0,0.08);
            transition: 0.3s;
            cursor: pointer;
        }

            .account-card:hover {
                transform: translateY(-6px);
                box-shadow: 0 10px 22px rgba(0,0,0,0.15);
            }

        /* Icon */
        .account-icon {
            width: 25px;
            height: 25px;
            /* border-radius: 50%;*/
            display: flex;
            align-items: center;
            justify-content: center;
            margin-right: 15px;
        }

        /* Text */
        .account-title {
            font-size: 15px;
            color: #555;
        }

        .account-row-line {
            display: flex;
            justify-content: space-between; /* LEFT + RIGHT */
            align-items: center;
            width: 100%;
        }

        .account-amount {
            font-size: 18px;
            font-weight: bold;
            margin-top: 5px;
        }

        /* 🔥 LIGHT GREEN */
        .card-positive {
            background: #eafaf1; /* light green */
            border-left: 5px solid #2ecc71;
        }

        /* 🔥 LIGHT RED */
        .card-negative {
            background: #fdecea; /* light red */
            border-left: 5px solid #e74c3c;
        }

        .card-zero {
            background: white; /* light red */
            border-left: 5px solid #bbc1bd;
        }

        /*Loan*/

        .loan-container {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 18px;
            padding: 15px;
        }

        .loan-card {
            background: #ffffff;
            border-radius: 14px;
            padding: 16px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.08);
            transition: 0.3s;
            cursor: pointer;
            position: relative;
        }

            .loan-card:hover {
                transform: translateY(-5px);
            }

        /* Header */
        .loan-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            font-weight: 600;
            font-size: 16px;
            margin-bottom: 10px;
        }

        .loancard-icon {
            width: 32px;
            height: 32px;
            display: flex;
            align-items: center;
            justify-content: center;
            border-radius: 50%;
            font-size: 14px;
        }

        /* Has Credit Card */
        .loancard-yes {
            color: #d60000;
        }

        /* No Credit Card */
        .loancard-no {
            display: none;
        }

        /* Amounts */
        .loanamount {
            display: flex;
            justify-content: space-between;
            margin: 6px 0;
            font-size: 14px;
        }

        .loanpositive {
            color: #1a7f4b;
            font-weight: bold;
        }

        .loannegative {
            color: #d60000;
            font-weight: bold;
        }
        /* Due highlight */
        .loanoverdue {
            color: white;
            background: #d60000;
            padding: 3px 8px;
            border-radius: 6px;
        }

        .loanupcoming {
            color: #b26a00;
            background: #fff3cd;
            padding: 3px 8px;
            border-radius: 6px;
        }
    </style>

    <style type="text/css">
        .divstyle {
            width: 47%;
            float: left;
            min-height: 82%;
            height: auto;
            text-align: center;
            border: 0.5px solid white;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }

        .section3 {
            border-radius: 25%;
            background-color: Navy;
            color: White;
            font-weight: bold;
            padding-top: 10%;
            height: 40px;
        }

        .section2 {
            border-radius: 80%;
            background-color: #C71585;
            color: White;
            font-weight: bold;
            padding-top: 30%;
            height: 80px;
            width: 75%;
            margin-left: 12%;
        }

        .section1 {
            background-color: #e9083c;
            color: White;
            font-weight: bold;
            font-size: 12px;
        }

        .profit {
            border-radius: 80%;
            background-color: #eff5fc;
            font-weight: bold;
            padding-top: 17%;
            padding-left: 12%;
            height: 170px;
            width: 35%;
            margin-left: 27%;
            margin-bottom: 10%;
            margin-top: 5%;
            vertical-align: middle;
            cursor: pointer;
        }

            .profit:hover .divpftlist {
                display: inline-block;
            }

        .divpftlist {
            display: none;
            width: 16%;
            min-height: 100px;
            border-radius: 4%;
            background-color: #e817174f;
            position: absolute;
            top: 40%;
            left: 40%;
        }

        .DashTbl {
            border: none;
            width: 98%;
        }

            .DashTbl th {
                /*color: #da1b6b;*/
                border-bottom: 0.5px solid #c8ccc8;
                font-size: 15px;
                padding: 1%;
                text-align: left;
            }

            .DashTbl td {
                border-bottom: 0.5px solid #c8ccc8;
                font-size: 14px;
                padding: 1.5%;
            }

        h3 {
            padding-left: 2%;
            text-align: center;
            color: #d91a1a;
            text-transform: uppercase;
        }


        .DetBox {
            margin: 1.5%;
            font-family: Cambria;
            font-size: 15px;
            font-weight: bold;
            cursor: pointer;
            color: white;
            padding: 1%;
            padding-top: 2%;
            height: 80px;
            width: 20%;
            border-radius: 5px;
            float: left;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }

        .SingleBox {
            margin: 1.5%;
            background-color: mintcream;
            font-family: Cambria;
            font-size: 15px;
            font-weight: bold;
            cursor: pointer;
            padding: 1%;
            padding-top: 2%;
            height: 60px;
            width: 45%;
            border-radius: 5px;
            float: left;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }

        .LinkBox {
            margin: 1.6%;
            font-family: Cambria;
            font-size: 18px;
            font-weight: bold;
            cursor: pointer;
            color: white;
            padding-top: 4%;
            height: 70px;
            width: 21%;
            border-radius: 5px;
            text-decoration: none;
            text-align: center;
            float: left;
            vertical-align: middle;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }
    </style>

    <script type="text/javascript" language="javascript">

        function Confirm() {
            if (confirm("Insufficent Balance in account. Do you want to continue ?")) {
                document.getElementById('<%= Button15.ClientID%>').click();
                return;
            } else {
                return false;
            }
        }

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

            //Invoice Section
            {

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

            //Rv section
            {
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

            }

            //PV Section
            {
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

    <style type="text/css">
        /* GRID */
        .chart-grid{
               padding: 1%;
 display: grid;
 grid-template-columns: 1fr 1fr;
 gap: 2%;
        }
         .chart-card {
     background: #ffffff;
     border-radius: 14px;
     padding: 16px;
     box-shadow: 0 4px 12px rgba(0,0,0,0.08);
     transition: 0.3s;
     cursor: pointer;
     position: relative;
     height:300px;
 }
        .radial-card {
            width: 30%;
            /*height: 220px;*/
            position: relative;
            display: inline-block;
            margin: 1%;
            cursor: pointer;
          
        }
               
        svg {
            transform: rotate(-90deg);
            width: 100%;
            height: 100%;
        }
        /* Background ring */
        .bg {
            fill: none;
            stroke: #eee;
            stroke-width: 12;
        }

        /* Progress ring */
        .progress {
            fill: none;
            stroke-width: 12;
            stroke-linecap: round;
            stroke-dasharray: 502;
            stroke-dashoffset: 502;
            transition: stroke-dashoffset 1.5s ease, filter 0.3s;
        }
     
        /* Glow hover effect */
        .radial-card:hover .progress {
            filter: drop-shadow(0 0 8px rgba(100, 100, 255, 0.7));
        }

        /* Center text */
        .center-text {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            font-size: 17px;
            font-weight: bold;
        }

        /* Tooltip */
        .radial-card::after {
            content: attr(data-label) " (" attr(data-percentage) "%)";
            position: absolute;
            bottom: -30px;
            left: 50%;
            transform: translateX(-50%);
            background: #333;
            color: #fff;
            padding: 5px 10px;
            font-size: 12px;
            border-radius: 5px;
            opacity: 0;
            transition: 0.3s;
            white-space: nowrap;
        }

        .radial-card:hover::after {
            opacity: 1;
        }
   
.item {
    margin-bottom: 15px;
}

.top {
    display: flex;
    justify-content: space-between;
    margin-bottom: 4px;
    font-size: 12px;
}

.title {
    color: #444;
}

.bar {
    width: 60%;
    height: 8px;
    background: #eee;
    border-radius: 10px;
    overflow: hidden;
}
.fill {
    height: 100%;
    width: 0%; /* start from 0 */
    border-radius: 10px;
    transition: width 1.2s ease-in-out;
}
</style>


    <script type="text/javascript">
        document.addEventListener("DOMContentLoaded", function () {

            document.querySelectorAll(".radial-card").forEach(function (card) {

                let percent = parseInt(card.getAttribute("data-percentage"));
                let circle = card.querySelector(".progress");

                let radius = 80;
                let circumference = 2 * Math.PI * radius;
                let offset = circumference - (percent / 100) * circumference;

                circle.style.strokeDasharray = circumference;

                setTimeout(() => {
                    circle.style.strokeDashoffset = offset;
                }, 300);

            });

        });
     
    </script>

    <style type="text/css">
          .card {
        width: 97%;
        background: #ffffff; /* white card */
    }
    canvas {
        width: 100% !important;
    }
    </style>

     <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
      <script type="text/javascript">

          function loadChart(labels, lastYearData, currentYearData) {

              // Get existing chart instance by canvas ID
              let existingChart = Chart.getChart("myChart");

              if (existingChart) {
                  existingChart.destroy();
              }
              const ctx = document.getElementById('myChart').getContext('2d');

              // Glow plugin
              const glowPlugin = {
                  id: 'glow',
                  beforeDatasetsDraw(chart) {
                      const ctx = chart.ctx;
                      ctx.save();
                      ctx.shadowColor = '#aaa';
                      ctx.shadowBlur = 15;
                  },
                  afterDatasetsDraw(chart) {
                      chart.ctx.restore();
                  }
              };

              // Gradients
              const g1 = ctx.createLinearGradient(0, 0, 0, 400);
              g1.addColorStop(0, "rgba(255,0,128,0.4)");
              g1.addColorStop(1, "rgba(255,0,128,0)");

              const g2 = ctx.createLinearGradient(0, 0, 0, 400);
              g2.addColorStop(0, "rgba(0,200,255,0.4)");
              g2.addColorStop(1, "rgba(0,200,255,0)");

              window.myChart = new Chart(ctx, {
                  type: 'line',
                  data: {
                      labels: labels,
                      datasets: [
                          {
                              label: 'Last Year',
                              data: lastYearData,
                              borderColor: '#ff0080',
                              backgroundColor: g1,
                              tension: 0.5,
                              fill: true,
                              borderWidth: 1,
                              pointRadius: 0,
                              pointHoverRadius: 6
                          },
                          {
                              label: 'Current Year',
                              data: currentYearData,
                              borderColor: '#00c8ff',
                              backgroundColor: g2,
                              tension: 0.5,
                              fill: true,
                              borderWidth: 1,
                              pointRadius: 0,
                              pointHoverRadius: 6
                          }
                      ]
                  },
                  options: {
                      responsive: true,
                      interaction: {
                          mode: 'index',
                          intersect: false
                      },
                      plugins: {
                          legend: {
                              position: 'bottom'
                          },
                          tooltip: {
                              backgroundColor: '#111',
                              titleColor: '#fff',
                              bodyColor: '#fff'
                          }
                      },
                      scales: {
                          x: {
                              grid: {
                                  color: '#fff'
                              }
                          },
                          y: {
                              grid: {
                                  color: '#fff'
                              }
                          }
                      }
                  },
                  plugins: [glowPlugin]
              });
          }
      </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="container">
        <asp:HiddenField ID="hdn_user_id" runat="server" />
        <asp:Button ID="Button15" runat="server" Style="display: none" Text="" OnClick="callSAveCompletion" />

        <div class="section-title">
            <asp:Label ID="lblcharthead" runat="server" ></asp:Label>
        </div>
        <div class="chart-grid">
            <div runat="server" id="divchart">

                <div class="chart-card">
                    <b>SERVICE COMPLETION STATUS</b>
                    <br />
                    <br />
                    <asp:Repeater ID="rptChart" runat="server">
                        <ItemTemplate>

                            <div class="radial-card"
                                data-percentage='<%# Eval("Percentage") %>'
                                data-label='<%# Eval("Label") %>'>

                                <svg viewBox="0 0 200 200" xmlns="http://www.w3.org/2000/svg">

                                    <defs>
                                        <!-- UNIQUE gradient -->
                                        <linearGradient id="grad_<%# Container.ItemIndex %>" x1="0%" y1="0%" x2="100%" y2="100%">
                                            <stop offset="0%" stop-color='<%# Eval("S2") %>' />
                                            <stop offset="100%" stop-color='<%# Eval("S1") %>' />
                                        </linearGradient>

                                        <!-- UNIQUE glow filter -->
                                        <filter id="glow_<%# Container.ItemIndex %>" x="-50%" y="-50%" width="200%" height="200%">
                                            <feGaussianBlur stdDeviation="3" result="blur" />
                                            <feMerge>
                                                <feMergeNode in="blur" />
                                                <feMergeNode in="SourceGraphic" />
                                            </feMerge>
                                        </filter>
                                    </defs>

                                    <!-- Background -->
                                    <circle cx="100" cy="100" r="80" class="bg" />

                                    <!-- Progress -->
                                    <circle cx="100" cy="100" r="80"
                                        class="progress clickable"
                                        stroke='url(#grad_<%# Container.ItemIndex %>)'
                                        filter='url(#glow_<%# Container.ItemIndex %>)' />

                                </svg>

                                <!-- Center text -->
                                <div class="center-text">
                                    <%# Eval("Percentage") %>%
                                </div>

                            </div>

                        </ItemTemplate>
                    </asp:Repeater>
               
                <asp:Repeater ID="rptProgress" runat="server">
    <ItemTemplate>

        <div class="item">
            <div class="top">
                <span class="title"><%# Eval("Label") %></span>
            </div>
            <div class="bar">
         <div style='<%# string.Format(
    "background: linear-gradient(90deg, {0}, {1}); width:{2}%; height:8px; border-radius:10px;",
    Eval("S1"), 
    Eval("S2"), 
    Eval("Percentage")
) %>'>
</div></div>
           
        </div>

    </ItemTemplate>
</asp:Repeater>    
                </div>
            </div>
            <div runat="server" id="divchartbar">
                <div class="chart-card">
                    <b>LAST YEAR VS CURRENT YEAR PROFIT</b>
                    <br />
                    <br />
                    <div class="card">
                        <canvas id="myChart"></canvas>
                    </div>
                    <div style="text-align: left; padding-left: 5%; color:dimgray">
                       <span> <asp:Label ID="LTotal" runat="server"></asp:Label></span> 
                      <span style="padding-left:4%">    <asp:Label ID="CTotal" runat="server"></asp:Label></span>
                    </div>
                </div>
            </div>
        </div>
        <div style="clear: both"></div>
        <div class="section-title">QUICK ACTIONS</div>
        <div class="card-grid">
            <div class="action-card c1" runat="server" id="divlnkinvoice">
                <div class="icon-box">📄</div>
                <asp:LinkButton runat="server" ID="lnkinvoice" CssClass="noDecorationcc" OnClick="lnkinvoice_Click">
        New Invoice
                </asp:LinkButton>
            </div>
            <div class="action-card c2" runat="server" id="divlnkSC">
                <div class="icon-box">✔</div>
                <asp:LinkButton runat="server" ID="lnkSC" PostBackUrl="~/Transactions/ServiceCompletion.aspx" class="noDecorationcc">
       Service Completion
                </asp:LinkButton>
            </div>
            <div class="action-card c3" runat="server" id="divlnkRV">
                <div class="icon-box">💰</div>
                <asp:LinkButton runat="server" ID="lnkRV" OnClick="lnkRV_Click" class="noDecorationcc">
        New Receipt Voucher
                </asp:LinkButton>
            </div>
            <div class="action-card c4" runat="server" id="divlnkPV">
                <div class="icon-box">💳</div>
                <asp:LinkButton runat="server" ID="lnkPV" OnClick="lnkPV_Click" class="noDecorationcc">
        New Payment Voucher
                </asp:LinkButton>
            </div>
        </div>
        <div style="clear: both"></div>

        <div id="divsummarytop" runat="server" visible="false" style="width: 97%">
            <div class="section-title">FINANCIAL OVERVIEW</div>
            <div class="card-grid">
                <div class="info-card pink" runat="server">
                    <asp:LinkButton ID="lnkbtndeferedincome" runat="server" OnClick="lnkbtndeferedincome_Click" Style="text-decoration: none; color: white">
                        <div style="float: left; width: 73%">
                            DEFERRED INCOME
                    <div style="padding: 7%; font-size: 16px;">
                        <asp:Label ID="Credit" runat="server" Text=""></asp:Label>
                    </div>
                        </div>
                        <div style="width: 13%; float: left; padding-top: 7%">
                            <img src="Images/credit-card.png" style="height: 30px; width: 30px" alt="" />
                        </div>
                    </asp:LinkButton>
                </div>

                <div class="info-card green">
                    <asp:LinkButton ID="lnkbtnreceivable" runat="server" OnClick="lnkbtnreceivable_Click" Style="text-decoration: none; color: white">
                        <div style="float: left; width: 73%">
                            RECEIVABLE
                    <div style="padding: 7%; font-size: 16px;">
                        <asp:Label ID="Receivable" runat="server" Text=""></asp:Label>
                    </div>
                        </div>
                        <div style="width: 13%; float: left; padding-top: 7%">
                            <img src="Images/money.png" style="height: 30px; width: 30px" alt="" />
                        </div>
                    </asp:LinkButton>
                </div>
                <div class="info-card orange">

                    <asp:LinkButton ID="lnkCustAdvance" runat="server" OnClick="lnkCustAdvance_Click" Style="text-decoration: none; color: white">
                        <div style="float: left; width: 73%">
                            CUSTOMER ADVANCE
                    <div style="padding: 7%; font-size: 16px;">
                        <asp:Label ID="CustomerAdvance" runat="server" Text=""></asp:Label>
                    </div>
                        </div>
                        <div style="width: 13%; float: left; padding-top: 7%">
                            <img src="Images/Pendingservice.png" style="height: 30px; width: 30px" alt="" />
                        </div>
                    </asp:LinkButton>
                </div>
                <div class="info-card blue">
                    <asp:LinkButton ID="lnkbtnVendrBalance" runat="server" OnClick="lnkbtnVendrBalance_Click" Style="text-decoration: none; color: white">
                        <div style="float: left; width: 73%">
                            VENDOR OUTSTANDING
                    <div style="padding: 7%; font-size: 16px;">
                        <asp:Label ID="VendorOustanding" runat="server" Text=""></asp:Label>
                    </div>
                        </div>
                        <div style="width: 13%; float: left; padding-top: 7%">
                            <img src="Images/Payable.png" style="height: 30px; width: 30px" alt="" />
                        </div>
                    </asp:LinkButton>
                </div>
            </div>
            <div class="bottom-grid">
                <div class="info-card yellow ">
                    <asp:LinkButton ID="lnkscreceivable" runat="server" OnClick="lnkscreceivable_Click" Style="text-decoration: none; color: white">
                        <div style="float: left; width: 73%">
                            SC BASED RECEIVABLE
                    <div style="padding: 4%; font-size: 16px;">
                        <asp:Label ID="lblSCReceivable" runat="server" Text=""></asp:Label>
                    </div>
                        </div>
                        <div style="width: 13%; float: left; padding-top: 7%">
                            <img src="Images/money.png" style="height: 30px; width: 30px" alt="" />
                        </div>
                    </asp:LinkButton>
                </div>

                <div class="profit-card">
                    <asp:LinkButton ID="lnktodayserviceprft" runat="server" OnClick="lnktodayserviceprft_Click" Style="text-decoration: none; color: white">
                        <div style="float: right; width: 27%; color: gray; font-size: 20px;">
                            <img src="../Images/up.png" class="titleimg" alt="" />
                            <asp:Label ID="lblpftamt" runat="server"></asp:Label>
                        </div>
                        <div style="float: left; width: 30%; text-align: right; padding-right: 14%;">
                            <span style="font-size: large; color: #545252">Today's Profit</span>
                            <br />

                            <asp:Label ID="lblpftdate" runat="server" Style="padding-left: 4%; color: red; padding-top: 4%;" Text=""></asp:Label>
                        </div>
                    </asp:LinkButton>
                </div>
            </div>
        </div>
        <div style="clear: both"></div>
        <div runat="server" id="divaccountSummary">
            <div class="section-title">BANK & CASH ACCOUNTS</div>
            <div class="account-card-container">
                <asp:Repeater ID="rptAccount" runat="server">
                    <ItemTemplate>
                        <div class='account-card <%# Convert.ToDecimal(Eval("Balance")) == 0 ?"card-zero"  : (Convert.ToDecimal(Eval("Balance")) >= 0 ? "card-positive" : "card-negative") %>'>
                            <div class="account-icon">
                                <asp:Image ID="img_profile" runat="server" ImageUrl='<%#Eval("ProfileImage")%>' class="Profile_image" />
                            </div>
                            <div class="account-row-line">
                                <div class="account-title">
                                    <asp:HiddenField ID="hdnaccountType" runat="server" Value='<%#Eval("accountType")%>' />
                                    <asp:HiddenField ID="hdnAccntId" runat="server" Value='<%#Eval("id")%>' />
                                    <asp:LinkButton ID="lnkaccname" Style="text-decoration: none; color: black" runat="server" Text='<%#Eval("Name")%>' OnClick="lnkaccname_Click"></asp:LinkButton>
                                </div>
                                <div class='account-amount'>
                                    <asp:LinkButton ID="lnkaccbalance" Style="text-decoration: none"
                                        runat="server" Text='<%#Eval("Balance")%>' OnClick="lnkaccname_Click"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <div style="clear: both"></div>
        <div runat="server" id="divLoan">
            <div class="section-title">LOAN ACCOUNT</div>
            <div class="loan-container">
                <asp:Repeater ID="rptLoan" runat="server">
                    <ItemTemplate>
                        <div class="loan-card">

                            <div class="loan-header">
                                <asp:HiddenField ID="hdnLoanAccntId" runat="server" Value='<%#Eval("id")%>' />
                                <span>
                                    <asp:LinkButton ID="lnkloanaccname" Style="text-decoration: none; color: black" runat="server" Text='<%#Eval("Name")%>' OnClick="lnkloanaccname_Click"></asp:LinkButton>
                                </span>
                                <span class='loancard-icon <%# Convert.ToString(Eval("isCreditCard"))=="1" ? "loancard-yes" : "loancard-no" %>'>💳
                                </span>
                            </div>

                            <div class="loanamount">
                                <span>Receivable</span>
                                <span class="loanpositive"><%# Eval("Receivable") %></span>
                            </div>

                            <div class="loanamount">
                                <span>Payable</span>
                                <span class="loannegative"><%# Eval("Payable") %></span>
                            </div>

                            <div class="loanamount">
                                <span>Credit Amount</span>
                                <span><%# Eval("CreditAmount") %></span>
                            </div>

                            <div class="loanamount">
                                <span>Next Due Date </span><b>
                                    <span>
                                        <%# Eval("DueDate") != DBNull.Value ? ((DateTime)Eval("DueDate")).ToString("dd-MM-yy") : "-" %>
                                    </span></b>
                            </div>

                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <div style="clear: both"></div>


        <div class="week_activity divstyle" style="float: left; background-image: linear-gradient(45deg, #FFFFF4, transparent);" runat="server" id="divtopup">
            <h3 style="text-align: center">TopUp Balance
            </h3>
            <div style="height: 380px; overflow-y: auto">
                <table class="DashTbl" style="width: 98%; padding: 1%">
                    <tr style="color: brown; font-weight: bold">
                        <th style="text-align: center">#
                        </th>
                        <th>Name
                        </th>
                        <th style="text-align: center">Balance
                        </th>

                    </tr>
                    <asp:Repeater ID="rptTopup" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 5%;">
                                    <%# Container.ItemIndex + 1 %>
                                </td>
                                <td style="width: 25%; text-align: left">
                                    <%#Eval("Name")%>
                                </td>
                                <td style="text-align: center; width: 10%">
                                    <%#Eval("Balance")%>
                                </td>

                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>

                </table>
            </div>
        </div>

         
        <div style="clear: both"></div>
        <div class="week_activity divstyle" style="width: 65%" runat="server" id="divtopservice" visible="false">
            <h3 style="padding-left: 2%; text-align: center">
                <asp:Label ID="Label2" Text="Last Month Top Service" runat="server"></asp:Label></h3>

            <asp:Chart ID="ChartService" BorderSkin-SkinStyle="Emboss" Width="700px" Height="375px" runat="server" Palette="Bright">
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">
                        <AxisX LineColor="Gray">
                            <MajorGrid LineColor="Gray" LineDashStyle="NotSet" />
                        </AxisX>
                        <AxisY LineColor="Gray">
                            <MajorGrid LineColor="Gray" LineDashStyle="NotSet" />
                        </AxisY>
                    </asp:ChartArea>
                </ChartAreas>
                <Legends>
                    <asp:Legend Name="Legend1" Docking="Right">
                    </asp:Legend>
                </Legends>
            </asp:Chart>

        </div>

        <div class="week_activity divstyle" style="width: 30%; background-image: linear-gradient(45deg, #FFFFF4, transparent);" runat="server" id="divTopEmployee" visible="false">
            <h3 style="padding-left: 2%; text-align: center">
                <asp:Label ID="Label1" Text="Last Month Top Employee" runat="server"></asp:Label></h3>

            <table style="border: none; width: 98%">
                <asp:Repeater ID="rptEmployee" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td style="width: 20%; padding: 3%">
                                <div style="height: 30px; padding-left: 25%; width: 30px;">
                                    <asp:Image ID="img_profile" runat="server" ImageUrl='<%#Eval("ProfileImage")%>' class="Profile_image" />
                                </div>
                            </td>
                            <td style="width: 60%; text-align: left">
                                <%#Eval("Name")%>
                            </td>
                            <td style="width: 20%">
                                <div style="height: 25px; padding-top: 8px; width: 35px; border-radius: 50%; border: 1px solid #91919163; box-shadow: 0px 0px 10px 4px whitesmoke;">
                                    <%#Eval("Counts")%>
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>

            </table>

        </div>


        <asp:UpdatePanel ID="UpdInvoiceadd" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlInvoiceadd" Visible="false" runat="server">
                    <div class="popupBackground">
                    </div>
                    <div class="animated largePopUp" style="left: 2%; width: 96%">
                        <AmarCentre:InvoiceUC ID="UCInvoice" runat="server" />
                    </div>
                </asp:Panel>

            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="UpdRVadd" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlRVadd" Visible="false" runat="server">
                    <div class="popupBackground">
                    </div>
                    <div class="animated halfPopUp">
                        <AmarCentre:RVUC ID="UCRV" runat="server" />
                    </div>
                </asp:Panel>

            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdPVadd" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlPVadd" Visible="false" runat="server">
                    <div class="popupBackground">
                    </div>
                    <div class="animated halfPopUp">
                        <AmarCentre:PVUC ID="UCPV" runat="server" />
                    </div>
                </asp:Panel>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

   

</asp:Content>

