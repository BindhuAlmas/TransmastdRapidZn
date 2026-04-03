<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="Quotation.aspx.cs" Inherits="AmarCentre.Transactions.Quotation" %>
<%@ Register Src="~/Transactions/UserControl/Customer.ascx" TagName="CustomerMaster"
    TagPrefix="AmarCentre" %>
<%@ Register Src="~/Transactions/UserControl/UCMail.ascx" TagName="MailUC" TagPrefix="AmarCentre" %>
<%@ Register Src="~/Masters/UserControl/UCService.ascx" TagName="ServiceMaster"
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

            function TaxChange() {
                InsideRepeaterCalculation();
                OutsideRepeaterCalculation();
            }

            /*Unit Price,Amount,Discount*/
            $('.inline').blur(function (e) {
                OutsideRepeaterCalculation();
            });

             /*Unit PriceIn,qtyin,fineIn*/
            $('.unit_amtD,.fine_amtD,.qtyD,.discountD,.unit_expD').blur(function (e) {
                InsideRepeaterCalculation();
                Calc();
            });
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
                    var InvoiceType = 0;
                    var TaxAppliedWithDiscount = 0;

                    if ($('#rbTaxInvoice').is(':checked')) {
                        InvoiceType = 1;
                    }
                    else {
                        InvoiceType = 2;
                    }
                    /*Service Price With Tax*/
                    if ($('#hdnSerPriceWTax').val() != '') {
                        SerPriceWithTax=parseInt($('#hdnSerPriceWTax').val());
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
                    //if ($(this).closest("tr").find('#hdnInvDExpense').val() != '') {
                    //    expamt = parseFloat($(this).closest("tr").find('#hdnInvDExpense').val());
                    //}
                    if ($(this).closest("tr").find('.unit_expD').val() != '') {
                        expamt = parseFloat($(this).closest("tr").find('.unit_expD').val());
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

                    TotSCAmt = parseFloat(TxtSCAmt) + parseFloat(SCamt);/*This was used in taxamt before PriceWith Tax concept*/
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

                        //if (SerPriceWithTax == 0) {/*(Price-Expense)*TaxPer/100*/
                        //    taxamt = ((parseFloat(UP) - parseFloat(expamt)) * parseFloat(taxper) / 100).toFixed(2);
                        //} else if (SerPriceWithTax == 1) {/*(Price-Expense)*TaxPer/(100+Taxper)*/
                        //    taxamt = (((parseFloat(UP) - parseFloat(expamt)) * parseFloat(taxper)) / (100 + parseFloat(taxper))).toFixed(2);
                        //}
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
                        SCamt= parseFloat(UP) - parseFloat(expamt)- parseFloat(taxamt);
                    }
                    /*Price With Tax*/
                    pricewittax = parseFloat(Price) + parseFloat(taxamt) + parseFloat(TxtSCAmt) + parseFloat(fine);

                    //TotAmt = (parseFloat(pricewittax) * parseFloat(Qty)).toFixed(2);
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
                    var InvoiceType = 0;
                    var TaxAppliedWithDiscount = 0;

                    if ($('#rbTaxInvoice').is(':checked')) {
                        InvoiceType = 1;
                    }
                    else {
                        InvoiceType = 2;
                    }
                    /*Service Price With Tax*/
                    if ($('#hdnSerPriceWTax').val() != '') {
                        SerPriceWithTax=parseInt($('#hdnSerPriceWTax').val());
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
                    /*Expense*/
                    //if ($(this).closest("tr").find('#hdn_expn').val() != '') {
                    //    expamt = parseFloat($(this).closest("tr").find('#hdn_expn').val());
                    //}
                    if ($(this).closest("tr").find('.unit_exp').val() != '') {
                        expamt = parseFloat($(this).closest("tr").find('.unit_exp').val());
                    }
                    /*Fine Applicable*/
                    if ($(this).closest("tr").find('#hdnFineApplicable').val() != '') {
                        fineapplicable = parseFloat($(this).closest("tr").find('#hdnFineApplicable').val());
                    }
                    /*Tax Percentage*/
                    if ($(this).closest("tr").find('#hdn_tax').val() != '') {
                        taxper = parseFloat($(this).closest("tr").find('#hdn_tax').val());
                    }
                    if ($(this).closest("tr").find('.discount').val() != '') {
                            DiscntAmt = parseFloat($(this).closest("tr").find('.discount').val());
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

                    TotSCAmt = parseFloat(TxtSCAmt) + parseFloat(SCamt);/*This was used in taxamt before PriceWith Tax concept*/
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
                        //if (SerPriceWithTax == 0) {/*(Price-Expense)*TaxPer/100*/
                        //    taxamt = ((parseFloat(UP) - parseFloat(expamt)) * parseFloat(taxper) / 100).toFixed(2);
                        //} else if (SerPriceWithTax == 1) {/*(Price-Expense)*TaxPer/(100+Taxper)*/
                        //    taxamt = (((parseFloat(UP) - parseFloat(expamt)) * parseFloat(taxper)) / (100 + parseFloat(taxper))).toFixed(2);
                        //}
                    } else if (InvoiceType == 1) {
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
                        SCamt= parseFloat(UP) - parseFloat(expamt)- parseFloat(taxamt);
                    }
                    /*Price With Tax*/
                    pricewittax = parseFloat(Price) + parseFloat(taxamt) + parseFloat(TxtSCAmt) + parseFloat(fine);

                    //TotAmt = (parseFloat(pricewittax) * parseFloat(Qty)).toFixed(2);
                    TotAmt = ((parseFloat(pricewittax) - parseFloat(DiscntAmt)) * parseFloat(Qty)).toFixed(2);

                    $(this).closest("tr").find('.taxamt').val(parseFloat(taxamt).toFixed(2));
                    $(this).closest("tr").find('#hdnPrice').val(parseFloat(Price).toFixed(2));
                    $(this).closest("tr").find('#hdn_sc').val(parseFloat(SCamt).toFixed(2));
                    $(this).closest("tr").find('.Prc_amt').val(parseFloat(pricewittax).toFixed(2));
                    $(this).closest("tr").find('.il_tot_amt').val(parseFloat(TotAmt).toFixed(2));
                });
                Calc();
            }
            function Calc() {
                var ILTotAmt = 0;
                var GrndTotAmt = 0;
                var PresentTot = 0;
                $('.invtot').each(function () {
                    var Amt = 0;

                    if ($(this).closest("tr").find('.invtot').val() != '') {
                        Amt = parseFloat($(this).closest("tr").find('.invtot').val());
                    }

                    ILTotAmt = parseFloat(ILTotAmt) + parseFloat(Amt);
                });
                if ($('.il_tot_amt').val() != '') {
                    PresentTot = parseFloat($('.il_tot_amt').val());
                }
                GrndTotAmt = parseFloat(ILTotAmt) + parseFloat(PresentTot);

                /*Amount Round Value */
                if ($('#hdnIsDisableRoundOff').val() != '1') {
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
                }
                else {
                    $('.tot_grnd_amt').val(parseFloat(GrndTotAmt).toFixed(2));
                }
                /*End of Amount Round Value*/
            }
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Quotation/اقتباس
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
                            <th  style="width: 3%;">
                                Sl 
                            </th>
                            <th style="width: 6%;">
                                Code / رمز
                            </th>
                             <th style="width: 8%;">
                                Date / تاريخ
                            </th>
                            <th style="width:20%;">
                                Customer / زبون
                            </th>
                            <th style="width: 13%;">
                               Subject
                            </th>
                            <th style="width: 8%;">
                                Amount / المبلغ
                            </th>
                             <th style="width: 9%;">
                              Invoice Amount
                            </th>
                              <th style="width: 9%;">
  Status
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
                                        <%#Eval("Dateds")%>
                                    </td>
                                    <td>
                                        <%#Eval("Name")%>
                                    </td>
                                    <td>
                                        <%#Eval("Subject")%>
                                    </td>
                                    <td>
                                        <%#Eval("Grand_Total")%>
                                    </td>
                                      <td>
                                        <%#Eval("InvoiceAmount")%>
                                    </td>
                                      <td>
    <%#Eval("Statusname")%>
</td>
                                    <td >
                                        <asp:Button ID="btn_edit" runat="server" class="btn_edit" ToolTip="Edit" CommandName="Edit" />
                                        <asp:Button ID="btnPrint" runat="server" class="btn_print" ToolTip="Print"
                                            CommandName="Print" />
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
                        <asp:UpdatePanel ID="upd_main" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div id="div_main" runat="server">
                                    <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
                                        UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="Adding_headingLargepopup">
                                               Quotation/اقتباس
                                            </div>
                                            <table class="formTable">
                                                <tr>
                                                    <td style="width: 33%">
                                                        Quotation Code / رمز الفاتورة
                                                        <asp:TextBox ID="lbl_Code" runat="server" class="txt read_Only" Font-Bold="true" Text=""></asp:TextBox>
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
                                                       
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Customer Name / اسم الزبون <span style="color: Red">&nbsp*</span>
                                                        <asp:UpdatePanel ID="Upd_CustomerDrop_Panel" runat="server" ChildrenAsTriggers="false"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <telerik:RadComboBox ID="drp_customer" Sort="Ascending" Filter="Contains" runat="server"
                                                                    AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
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
                                                    <td>
                                                        Templates/قوالب<br />
                                                        <telerik:RadComboBox ID="drpTemplates" Sort="Ascending" Filter="Contains" runat="server"
                                                            AllowCustomText="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" RenderMode="Lightweight"
                                                            EmptyMessage="Search Templates..." OnClientFocus="OnClientKeyPressing"
                                                            Style="overflow: hidden; width: 85%; border: none!important;float:left">
                                                        </telerik:RadComboBox>
                                                        <asp:Button ID="btngoTemp" Text="Go" style="float:left;margin-left:1%;margin-top:1%"  runat="server" OnClick="drpTemplatesOnSelectedIndexChanged" />
                                                    </td>
                                                    <td>
                                                        Subject
                                                         <asp:TextBox ID="txtSubject" CssClass="txt" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr style="display: none">
                                                    <td>
                                                        <asp:RadioButton ID="rbTaxInvoice" Name="rbInputType" ClientIDMode="Static" runat="server" GroupName="InvoiceType" />Tax Quotation/الاقتباس الضريبي
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rbNormalInvoice" Name="rbInputType" ClientIDMode="Static" runat="server" GroupName="InvoiceType" />Normal Quotation/الاقتباس العادي
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
                                                                                <th style="width: 20%" colspan="2">
                                                                                    Service / الخدمات
                                                                                </th>
                                                                                <th style="width: 12%">
                                                                                    Particulars / تفاصيل
                                                                                </th>
                                                                                <th style="width: 8%">
                                                                                    Price / السعر
                                                                                </th>
                                                                                 <th style="width: 9%">Govt.Fee / رسوم الحكومة
                                                                                </th>
                                                                                <th style="width: 10%;display:none">
                                                                                    Service Charge / تكلفة الخدمة
                                                                                </th>
                                                                                <th style="width: 8%">
                                                                                   Fine / مبلغ الغرامة
                                                                                </th>
                                                                                <th style="width: 6%">
                                                                                    Discount
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
                                                                                <th style="width: 10%">
                                                                                    Total / مجموع
                                                                                </th>
                                                                                <th style="width: 10%">
                                                                                    Action/عمل
                                                                                </th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <asp:Repeater ID="rpt_Item_list" runat="server">
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
                                                                                            <asp:TextBox ID="lblInvDdesc" Width="95%" TabIndex="-1" runat="server" Text='<%#Eval("Particulars") %>'></asp:TextBox>
                                                                                        </td>
                                                                                        <td style="text-align: left">
                                                                                            <asp:TextBox ID="txtInvDDisplayPrice" class="txt unit_amtD numbers_only" Width="85%"
                                                                                                runat="server" Text='<%#Eval("DisplayPrice") %>' TabIndex="-1"></asp:TextBox>
                                                                                            <asp:HiddenField ID="hdnInvDExpense" ClientIDMode="Static" runat="server" Value='<%#Eval("Expense") %>' />
                                                                                            <asp:HiddenField ID="hdnInvDServiceCharge" ClientIDMode="Static" runat="server" Value='<%#Eval("ServiceCharge") %>' />
                                                                                            <asp:HiddenField ID="hdnInvDPrice" ClientIDMode="Static" runat="server" Value='<%#Eval("Price") %>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtInvDExpense" class="txt unit_expD numbers_only" Width="85%"
                                                                                                runat="server" Text='<%#Eval("Expense") %>' TabIndex="-1"></asp:TextBox>
                                                                                            <%--<%#Eval("Expense") %>--%>
                                                                                        </td>
                                                                                        <td style="text-align: left;display:none">
                                                                                             <asp:TextBox ID="txtInvDAddServiceCharge" class="txt serCharge_amtD read_Only numbers_only asLabel" Width="85%"
                                                                                                runat="server" Text='<%#Eval("AdditionalServiceCharge") %>' TabIndex="-1"></asp:TextBox>
                                                                                            </td>
                                                                                        <td style="text-align: left">
                                                                                            
                                                                                             <asp:TextBox ID="txtInvDFine" class="txt fine_amtD numbers_only" Width="85%"
                                                                                                runat="server" Text='<%#Eval("Fine") %>' TabIndex="-1" ></asp:TextBox>
                                                                                            <asp:HiddenField ID="hdnInvDFineApplicable" ClientIDMode="Static" runat="server" Value='<%#Eval("FineApplicable") %>' />
                                                                                            </td>
                                                                                          <td runat="server" style="text-align: left">
                                                                                            <asp:TextBox ID="txtInvDdiscount" class=" discountD InvDdiscount  txt"
                                                                                                Width="85%" runat="server" Text='<%#Eval("Discount") %>'></asp:TextBox>
                                                                                        </td>
                                                                                        <td style="text-align: left">
                                                                                            <asp:TextBox ID="txtInvDQty" class="numbers_only qtyD txt" Width="75%"
                                                                                                runat="server" Text='<%#Eval("Quantity") %>' TabIndex="-1"></asp:TextBox>
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
                                                                                            <asp:HiddenField ID="hdnTemplateId" ClientIDMode="Static" runat="server" Value='<%#Eval("TemplateId") %>' />

                                                                                            <asp:Button ID="btn_edit_line" runat="server" OnClick="btn_edit_line_OnClick" ToolTip="Edit"
                                                                                                class="btn_edit" Visible="false" />
                                                                                            <asp:Button ID="btn_remove_line" CommandName="Delete" class="btn_delete" runat="server"
                                                                                                ToolTip="Delete" OnClick="btn_remove_line_OnClick" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>
                                                                            <tr style="text-align: center">
                                                                                <%--  <asp:UpdatePanel ID="Upd_ServicePanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                <ContentTemplate>--%>
                                                                                <td>
                                                                                    <asp:Label ID="lblRepeaterSNo" Text="" TabIndex="-1" runat="server" />
                                                                                </td>
                                                                                <td style="text-align: left" colspan="2">
                                                                                    <asp:HiddenField ID="hdn_QuoDetailId" runat="server" Value="" />
                                                                                     <div style="clear: both">
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
                                                                                          </div>
                                                                                    <div style="float: left; width: 47%">
                                                                                    <asp:UpdatePanel ID="UpdSerCategoryDropdown" runat="server" ChildrenAsTriggers="false"
                                                                                        UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <telerik:RadComboBox ID="drpSerCategory" Sort="Ascending" Filter="Contains" runat="server"
                                                                                                AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Category..."
                                                                                                OnSelectedIndexChanged="drpFilter_OnSelectedIndexChanged" AutoPostBack="true" Visible="false"
                                                                                                ClientIDMode="AutoID" Style="overflow: hidden; width: 85%; border: none!important;"
                                                                                                OnClientFocus="OnClientKeyPressing" OnClientBlur="ValidateCombo">
                                                                                            </telerik:RadComboBox>
                                                                                            <asp:HiddenField ID="hdnSerCategory" runat="server" Value="" />
                                                                                            <asp:HiddenField ID="hdnSerCategoryId" runat="server" Value="" />
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                 </div>
                                                                                    <div style="float: left; width: 47%">
                                                                                    <asp:UpdatePanel ID="UpdSerSubCategoryDropdown" runat="server" ChildrenAsTriggers="false"
                                                                                        UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <telerik:RadComboBox ID="drpSerSubCategory" Sort="Ascending" Filter="Contains" runat="server"
                                                                                                AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Sub Category..."
                                                                                                OnSelectedIndexChanged="drpFilter_OnSelectedIndexChanged" AutoPostBack="true" Visible="false"
                                                                                                ClientIDMode="AutoID" Style="overflow: hidden; width: 85%; border: none!important;"
                                                                                                OnClientFocus="OnClientKeyPressing" OnClientBlur="ValidateCombo">
                                                                                            </telerik:RadComboBox>
                                                                                            <asp:HiddenField ID="hdnSerSubCategory" runat="server" Value="" />
                                                                                            <asp:HiddenField ID="hdnSerSubCategoryId" runat="server" Value="" />
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                         </div>
                                                                                    <div style="clear: both">
                                                                                    <asp:UpdatePanel ID="UpdServiceDropdown" runat="server" ChildrenAsTriggers="false"
                                                                                        UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <telerik:RadComboBox ID="drpService" Sort="Ascending" Filter="Contains" runat="server"
                                                                                                AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Service..."
                                                                                                OnSelectedIndexChanged="drpService_OnSelectedIndexChanged" AutoPostBack="true" DropDownWidth="700px"
                                                                                                ClientIDMode="AutoID" Style="overflow: hidden; width: 85%; border: none!important;"
                                                                                                OnClientFocus="OnClientKeyPressing" OnClientBlur="ValidateCombo">
                                                                                            </telerik:RadComboBox>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="drpService"
                                                                                                ValidationGroup="addService" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                                                InitialValue=""></asp:RequiredFieldValidator>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                        </div>
                                                                                </td>
                                                                                <td style="text-align: left">
                                                                                    <asp:UpdatePanel ID="UpdTxtDescription" runat="server" ChildrenAsTriggers="false"
                                                                                        UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_desc" Width="95%" CssClass="txt" runat="server" Text=""></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td style="text-align: left">
                                                                                    <asp:UpdatePanel ID="UpdTxtPrice" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_displayPrice" class="numbers_only unit_amt inline txt" Width="85%"
                                                                                                runat="server" Text=""></asp:TextBox>
                                                                                            <asp:HiddenField ID="hdn_expn" ClientIDMode="Static" runat="server" Value="" />
                                                                                            <asp:HiddenField ID="hdn_sc" ClientIDMode="Static" runat="server" Value="" />
                                                                                            <asp:HiddenField ID="hdnPrice" ClientIDMode="Static" runat="server" Value="" />
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ErrorMessage="*" runat="server"
                                                                                                ControlToValidate="txt_displayPrice" ValidationGroup="addService" Style="color: Red"
                                                                                                Display="Dynamic"></asp:RequiredFieldValidator>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                 <td style="text-align: left">
                                                                                    <asp:UpdatePanel ID="updexpense" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txtexpense" class="numbers_only unit_exp inline txt" Width="85%"
                                                                                                runat="server" Text=""></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                 <td style="text-align: left;display:none">
                                                                                     <asp:UpdatePanel ID="UpdTxtServiceCharge" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txtServiceCharge" class="numbers_only serCharge_amt inline txt" Width="85%"
                                                                                                runat="server" Text=""></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td style="text-align: left">
                                                                                   
                                                                                    <asp:UpdatePanel ID="UpdTxtFine" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txtFine" class="numbers_only fine_amt inline txt" Width="85%"
                                                                                                runat="server" Text=""></asp:TextBox>
                                                                                            <asp:HiddenField ID="hdnFineApplicable" ClientIDMode="Static" runat="server" Value="" />
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                  <td  style="text-align: right">
                                                                                    <asp:UpdatePanel ID="Updtxt_discount" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_discount" Style="text-align: right" class="numbers_only discount inline txt"
                                                                                                Width="85%" runat="server"></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td style="text-align: left">
                                                                                    <asp:UpdatePanel ID="UpdTxtQty" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_Qty" class="numbers_only qty inline txt" Width="75%" runat="server"
                                                                                                Text=""></asp:TextBox>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="*" runat="server"
                                                                                                ControlToValidate="txt_Qty" ValidationGroup="addService" Style="color: Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td style="text-align: left">
                                                                                    <asp:UpdatePanel ID="UpdTxtTaxAmt" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox TabIndex="-1" ID="txt_taxamt" class="numbers_only taxamt read_Only txt asLabel"
                                                                                                Width="95%" runat="server" Text=""></asp:TextBox>
                                                                                            <asp:HiddenField ID="hdn_tax" ClientIDMode="Static" runat="server" Value="" />
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td style="text-align: left">
                                                                                    <asp:UpdatePanel ID="UpdTxtPriceWithTax" runat="server" ChildrenAsTriggers="false"
                                                                                        UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_PriceWitTax" TabIndex="-1" class="numbers_only Prc_amt read_Only txt asLabel"
                                                                                                Width="95%" runat="server" Text=""></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                
                                                                                <td style="text-align: left">
                                                                                    <asp:UpdatePanel ID="UpdTxtTotPrice" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_totPrice" TabIndex="-1" class="numbers_only il_tot_amt read_Only txt asLabel"
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
                                                                                <td colspan="9" style="text-align:right">
                                                                                    Total
                                                                                </td>
                                                                                <td colspan="4">
                                                                                    <asp:UpdatePanel ID="Upd_Total_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox TabIndex="-1" Style="border: medium none; color: Red; font-size: 24px;
                                                                                                text-align: right; width: 95%" class="txt tot_grnd_amt read_Only txt" ID="txt_grand"
                                                                                                runat="server"></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                              
                                                                            <%-- </ContentTemplate>
                                                                                 </asp:UpdatePanel>--%>
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
                                                                <asp:HiddenField ID="hdn_PageName" runat="server" Value="Invoice" />
                                                                 <asp:HiddenField ID="hdnLeadId" runat="server" Value="0" />
                                                                <%--Regarding Customer User Control--%>
                                                                <asp:HiddenField ID="hdn_user_id" runat="server" />
                                                                <asp:HiddenField ID="hdnLanguage" runat="server" />
                                                                <asp:HiddenField ID="hdnDefaultInvoiceType" ClientIDMode="Static" runat="server" />
                                                                <asp:HiddenField ID="hdnSerPriceWTax" ClientIDMode="Static" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdnIsDisableRoundOff" ClientIDMode="Static" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdnTaxAppliedWithDiscount" ClientIDMode="Static" runat="server" />

                                                                <asp:Button ID="btn_save" class="butn_save" ValidationGroup="save" OnClick="btn_save_OnClick"
                                                                    runat="server" Text="Save/حفظ" />
                                                                <asp:Button ID="btn_save_print" class="butn_save" ValidationGroup="save" OnClick="btn_save_print_OnClick"
                                                                    runat="server" Text="Save & Print/حفظ وطباعة" />
                                                                  <asp:Button ID="btnOpenCancel" class="butn_delete" runat="server" Text="Cancel/إلغاء"
      OnClientClick="javascript : return confirm('Do you really want to cancel.. ?');"
      OnClick="btnOpenCancel_Click" />
                                                                <asp:Button ID="btn_print" class="butn" runat="server" Text="Print/طباعة" OnClick="btn_print_OnClick" />
                                                                <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_OnClick" />
                                                                <asp:Button ID="Button1" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_OnClick" />
                                                                <asp:Button ID="btnNewVersion" class="butn" runat="server" Visible="false" Text="New Version/نسخة جديدة"
                                                                    OnClick="btnNewVersionOnClick" />
                                                              
                                                                <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_update" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_print" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_add_N_print" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_update_N_print" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_newVersion" runat="server" Value="0" />
                                                                 <asp:HiddenField ID="hdnsendmail" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_cancel" runat="server" Value="0" />

                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div id="divQuotationHistory" runat="server" visible="false">
                                            <table class="listTable">
                    <thead>
                        <tr>
                            <th class="listTableSlNo" style="width: 5%;">
                                Sl No /رقم
                            </th>
                            <th style="width: 10%;">
                                Code / رمز
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
                        <asp:Repeater ID="rptQuotationHistory" runat="server" OnItemCommand="rptQuotationHistoryOnItemCommand"
                            OnItemDataBound="rptQuotationHistoryOnItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%# Container.ItemIndex + 1 %>.
                                        <asp:HiddenField ID="hdn_id" runat="server" Value='<%#Eval("Id")%>' />
                                    </td>
                                    <td>
                                        <%#Eval("Code")%>
                                    </td>
                                    <td>
                                        <%#Eval("Date")%>
                                    </td>
                                    <td>
                                        <%#Eval("Grand_Total")%>
                                    </td>
                                    <td class="listTableActionButtonDiv">
                                        <asp:Button ID="btnDefault" runat="server" class="btnDefault" ToolTip="Default" CommandName="Default" />
                                        <asp:Button ID="btnPrint" runat="server" class="btn_print" ToolTip="Print"
                                            CommandName="Print" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
                                                </div>
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
                    <div id="div2" class="messageAlerterror div_poperror animated" style="display: none" runat="server">
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

         <asp:UpdatePanel ID="UpdMailPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlMail" Visible="false" runat="server">
                <AmarCentre:MailUC ID="EmailUC" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
          <asp:UpdatePanel ID="UpdServicepnlAdd" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" style="height: 100%;">
            <ContentTemplate>
                <asp:Panel ID="pnlServiceAdd" Visible="false" runat="server">
                     <div class="popupBackground">
                    </div>
                    <div class="animated largePopUp">
                     <AmarCentre:ServiceMaster ID="UC_Service" runat="server" />
                         </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    
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
    <%--</ContentTemplate>
        </asp:UpdatePanel>
    </div>--%>
</asp:Content>
