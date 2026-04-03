<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true"
    CodeBehind="GeneralSettings.aspx.cs" Inherits="AmarCentre.Masters.GeneralSettings" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        General Settings/الاعدادات العامة
        <div class="searchDiv">
        </div>
    </div>
    <asp:UpdatePanel ID="Upd_Add_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <div class="list_info" style="display: none">
                </div>
                <table style="width:98%">
                  
                    <tr>
                        <td colspan="2">
                             <asp:CheckBox ID="chk_SerComWOPayment" Text="Service Completion Without Payment/إتمام الخدمة دون "
                                runat="server" />
                        </td>
                        <td style="width: 25%">
                            Company Email
                        </td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtmail" CssClass="txt" Style="width: 90%;" runat="server"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Please Enter Valid Email ID"
                                ValidationGroup="save" ControlToValidate="txtmail" Style="color: Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                Display="Dynamic">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chk_SerPriceWTax" runat="server" Text="Service Price With Tax/سعر الخدمة مع الضرائب" />
                        </td>
                        <td>
                            Company Email Password
                        </td>
                        <td>
                            <asp:TextBox ID="txtCompanyEmailPwd" CssClass="txt" TextMode="Password" Style="width: 90%;"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chkPrintTerms" runat="server" Text="Sales Order Print Terms/شروط طباعة أمر المبيعات" />
                        </td>
                         <td>
                           Company Name
                        </td>
                        <td>
                          <asp:TextBox ID="txtCompanyname" CssClass="txt" Style="width: 90%;" runat="server"></asp:TextBox>
                        </td>
                       
                    </tr>
                    <tr>
                        <td colspan="2">
                             <asp:CheckBox ID="chkDepartmentRequired" runat="server" Text="Department Required In Service/القسم مطلوب في الخدمة" />
                        </td>
                        <td>
                            TRN
                        </td>
                        <td>
                            <asp:TextBox ID="txtTRN" CssClass="txt" Style="width: 90%;" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chkCategoryRequired" runat="server" Text="Category Required In Service/الفئة المطلوبة في الخدمة" />
                        </td>
                         <td>
                            Default Invoice Type<span style="color: Red">&nbsp*</span>

                             <asp:CheckBox ID="chkQutnEdit" Text="Quotation Editable in Invoice" Visible="false"  runat="server" />
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drpDefaultInvoiceType" Sort="Ascending" Filter="Contains"
                                runat="server" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search Invoice Type..." Style="overflow: hidden;
                                width: 90%; border: none!important;">
                                <Items>
                                    <telerik:RadComboBoxItem Value="1" Text="Tax Invoice" />
                                    <telerik:RadComboBoxItem Value="2" Text="Normal Invoice" />
                                </Items>
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="drpDefaultInvoiceType"
                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                InitialValue=""></asp:RequiredFieldValidator>
                        </td>
                     
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chkSubCategoryRequired" runat="server" Text="Sub Category Required In Service/الفئة الفرعية المطلوبة في الخدمة" />
                        </td>
                        <td>
                            Customer Discount Type
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drpCDType" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search ..." Style="overflow: hidden;
                                width: 90%; border: none!important;">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chkDisplayDiscount" Text="Display Discount in invoice/عرض الخصم في الفاتورة"
                                runat="server" />
                        </td>
                         <td>
                           Profit Withdrawal Expense
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drpprofitExp" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search ..." Style="overflow: hidden;
                                width: 90%; border: none!important;">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chkTaxAppliedWithDiscount" Text="Tax Applied With Discount/الضرائب المطبقة مع الخصم"
                                runat="server" />
                        </td>
                        <td>
                          Vendor Statment Format
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drpVenStmt" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search ..." Style="overflow: hidden;
                                width: 90%; border: none!important;">
                                <Items>
                                    <telerik:RadComboBoxItem Value="1" Text="Format 1" />
                                    <telerik:RadComboBoxItem Value="2" Text="Format 2" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                       
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chkscinvoice" Text="SC in invoice" runat="server" />
                        </td>
                        <td>
                            Debtors Report Format
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drpDebtorsReport" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search Debtors Report Format..." Style="overflow: hidden;
                                width: 90%; border: none!important;">
                                <Items>
                                    <telerik:RadComboBoxItem Value="1" Text="Invoice Based" />
                                    <telerik:RadComboBoxItem Value="2" Text="SC Based" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chkremark" Text="Add remark in invoice print" runat="server" />
                        </td>
                        <td>
                           SalesOrder Print
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drpSalesorder" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search ..." Style="overflow: hidden;
                                width: 90%; border: none!important;">
                                <Items>
                                    <telerik:RadComboBoxItem Value="1" Text="Format 1" />
                                    <telerik:RadComboBoxItem Value="2" Text="Format 2" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chkCustinv" Text="Enable Customer Invoice" runat="server" />
                        </td>
                        <td style="width: 25%">
                           Primary mail notification day
                        </td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtSendAgreementExpiredMailBefore" CssClass="txt numbers_only" Style="width: 90%;"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chkIsSoftareNameAdd" Text="Print Software Name in Invoice" runat="server" />
                        </td>
                       <td style="width: 25%">
                           Secondary mail notification day
                        </td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtDocExpireSecondaryMailDays" CssClass="txt numbers_only" Style="width: 90%;"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                          <td colspan="2">
                            <asp:CheckBox ID="chkEmpSC" Text="Employee Based Transaction" runat="server" />
                        </td>
                       <td>
                            Expiry Email Template
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drpTemplate" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search Template..."
                                Style="overflow: hidden; width: 90%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                         <td colspan="2">
                            <asp:CheckBox ID="chkdeltdSC" Text="Include deleted SC in Statment" runat="server" />
                        </td>
                        <td style="width:20%">
                            Customer Invoice Print Format
                        </td>
                        <td style="width:29%">
                            <telerik:RadComboBox ID="drpInvoiceFormatCI" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search Format..." Style="overflow: hidden;
                                width: 90%; border: none!important;">
                                <Items>
                                    <telerik:RadComboBoxItem Value="1" Text="Format 1" />
                                    <telerik:RadComboBoxItem Value="2" Text="Format 2" />
                                    <telerik:RadComboBoxItem Value="3" Text="Format 3" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                     <tr>
                         <td colspan="2">
                               <asp:CheckBox ID="chktemplate" Text="Display Template name in invoice Print" runat="server" />
                        </td>
                        <td>
                            Receipt Print Format
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drpReceiptFormat" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search Receipt Format..." Style="overflow: hidden;
                                width: 90%; border: none!important;">
                                <Items>
                                    <telerik:RadComboBoxItem Value="1" Text="Format 1" />
                                    <telerik:RadComboBoxItem Value="2" Text="Format 2" />
                                    <telerik:RadComboBoxItem Value="3" Text="Format 3" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        
                    </tr>
                   <tr>
                        <td colspan="2">
                               <asp:CheckBox ID="chkdepartmentInInvoiceVisible" Text="Display Department in Invoice" runat="server" />
                        </td>
                         <td>
                            Quotation Print Format
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drpQuotationPrint" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search Quotation Format..." Style="overflow: hidden;
                                width:90%; border: none!important;">
                                <Items>
                                    <telerik:RadComboBoxItem Value="1" Text="Format 1" />
                                    <telerik:RadComboBoxItem Value="2" Text="Format 2" />
                                    <telerik:RadComboBoxItem Value="3" Text="Format 3" />
                                    <telerik:RadComboBoxItem Value="4" Text="Format 4" />
                                    <telerik:RadComboBoxItem Value="5" Text="Format 5" />
                                    <telerik:RadComboBoxItem Value="6" Text="Format 6" />

                                </Items>
                            </telerik:RadComboBox>
                        </td>
                   </tr>
                    <tr>
                         <td colspan="2">
                               <asp:CheckBox ID="chkIsMobileDupAllow" Text="Allow Customer Mobile no Duplication" runat="server" />
                        </td>
                         <td > Predate Days in SC
                        </td>
                         <td style="width: 29%">
                            <asp:TextBox ID="txtscpredate" CssClass="txt numbers_only" Style="width: 90%;" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                         <td colspan="2">
                               <asp:CheckBox ID="chkIstaxprintforall" Text="Tax Print for all Customer" runat="server" />
                        </td>
                            <td>
                            Receipt Voucher Print
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drpRVPrint" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search ..." Style="overflow: hidden;
                                width: 90%; border: none!important;">
                                <Items>
                                    <telerik:RadComboBoxItem Value="1" Text="Format 1" />
                                    <telerik:RadComboBoxItem Value="2" Text="Format 2" />
                                </Items>
                            </telerik:RadComboBox>
                        </td> 
                    </tr>
                    <tr>
                         <td colspan="2">
                               <asp:CheckBox ID="chkIsDisplaySCStatus" Text="Display SC Process Status" runat="server" />
                        </td>
                       
                       <td style="width: 20%">
                          Default BankCharge(%)
                        </td>
                        <td style="width: 29%">
                            <asp:TextBox ID="txtDefaultBankCharge" CssClass="txt numbers_only" Style="width: 90%;" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                   <tr>
                          <td colspan="2">
                               <asp:CheckBox ID="chkIsAllowSCAmountExceed" Text="Is Allow SC Amount Exceed Service Amount" runat="server" />
                        </td>
                        <td>
                            Transaction edit day limit
                        </td>
                        <td>
                              <asp:TextBox ID="txtTransEditdaylimit" CssClass="txt numbers_only" Style="width: 90%;" runat="server"></asp:TextBox>
                        </td>
                   </tr>
                    <tr>
                          <td colspan="2">
                               <asp:CheckBox ID="chkIsDisableRoundOff" Text="Is Disable Round Off" runat="server" />
                        </td>
                        <td>
                            SC View
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drpscview" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search ..." Style="overflow: hidden;
                                width:90%; border: none!important;">
                                <Items>
                                    <telerik:RadComboBoxItem Value="1" Text="Invoice Wise" />
                                    <telerik:RadComboBoxItem Value="2" Text="Service Wise" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                   </tr>
                    <tr>
                        <td colspan="2">
                             <asp:CheckBox ID="chkIsCommissionEditableInInvoice" Text="Is Commission Editable In Invoice" runat="server" />
                        </td>
                          <td>
                            Fine Expense 
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drpExpense" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search Expense..." Style="overflow: hidden;
                                width: 90%; border: none!important;">
                            </telerik:RadComboBox>
                        </td>
                        
                    </tr>
                    <tr>
                        <td colspan="2">
                             <asp:CheckBox ID="chkIsHideServiceAmtInSC" Text="Hide ServiceAmt In SC" runat="server" />
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                      <td>Admin Designation</td>
                        <td>
                            <telerik:RadComboBox ID="drpAdminDesign" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search Designation..." Style="overflow: hidden;
                                width: 90%; border: none!important;">
                            </telerik:RadComboBox>
                        </td>
                        <td rowspan="2">Default Quotation Remark
                        </td>
                        <td rowspan="2">
                            <asp:TextBox class="txtarea" Style="width: 90%" TextMode="MultiLine" ID="txtQremark"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                  
                    <tr>
                        <td>
                            Customer SOA PDF Format
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drpCustomerSOAPdfFormat" Sort="Ascending" Filter="Contains"
                                runat="server" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search Customer SOA Format..." Style="overflow: hidden;
                                width: 90%; border: none!important;">
                                <Items>
                                    <telerik:RadComboBoxItem Value="1" Text="Format 1" />
                                    <telerik:RadComboBoxItem Value="2" Text="Format 2" />
                                    <telerik:RadComboBoxItem Value="3" Text="Format 3" />
                                    <telerik:RadComboBoxItem Value="4" Text="Format 4" />
                                    <telerik:RadComboBoxItem Value="5" Text="Format 5" />
                                    <telerik:RadComboBoxItem Value="6" Text="Format 6" />
                                    <telerik:RadComboBoxItem Value="7" Text="Format 7" />
                                    <telerik:RadComboBoxItem Value="8" Text="Format 8" />
                                    <telerik:RadComboBoxItem Value="9" Text="Format 9" />

                                </Items>
                            </telerik:RadComboBox>
                        </td>
                       
                    </tr>
                      <tr>
                           <td>
                            Invoice Print Format
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drpInvoiceFormat" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search Invoice Format..." Style="overflow: hidden;
                                width: 90%; border: none!important;">
                                <Items>
                                    <telerik:RadComboBoxItem Value="1" Text="Format 1" />
                                    <telerik:RadComboBoxItem Value="2" Text="Format 2" />
                                    <telerik:RadComboBoxItem Value="3" Text="Format 3" />

                                    <telerik:RadComboBoxItem Value="5" Text="Format 4" />
                                    <telerik:RadComboBoxItem Value="6" Text="Format 5" />
                                    <telerik:RadComboBoxItem Value="7" Text="Format 6" />
                                    <telerik:RadComboBoxItem Value="8" Text="Format 7" />
                                    <telerik:RadComboBoxItem Value="9" Text="Format 8" />
                                    <telerik:RadComboBoxItem Value="10" Text="Format 9" />
                                    <telerik:RadComboBoxItem Value="11" Text="Format 10" />
                                    <telerik:RadComboBoxItem Value="4" Text="Format 11" />

                                    <telerik:RadComboBoxItem Value="12" Text="Format 12" />
                                    <telerik:RadComboBoxItem Value="13" Text="Format 13" />
                                    <telerik:RadComboBoxItem Value="14" Text="Format 14" />
                                    <telerik:RadComboBoxItem Value="15" Text="Format 15" />
                                    <telerik:RadComboBoxItem Value="16" Text="Format 16" />

                                </Items>
                            </telerik:RadComboBox>
                        </td>
                      
                       <td rowspan="2">Default Invoice Remark
                        </td>
                        <td rowspan="2">
                            <asp:TextBox class="txtarea" Style="width: 90%" TextMode="MultiLine" ID="txtDinvoiceremark"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                          <td>
                          Refundable Expense Type
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drprefundexpense" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search ..." Style="overflow: hidden;
                                width: 90%; border: none!important;">
                            </telerik:RadComboBox>
                        </td>
                        <td colspan="2"></td>
                    </tr>
                   
                    <tr>
                        <td>
                            Print Header(1200 x 215)
                        </td>
                        <td colspan="3" >
                            <telerik:RadAsyncUpload ID="fu_printHeader" MaxFileSize="500000000" runat="server"
                                MaxFileInputsCount="1" OnFileUploaded="fu_printHeader_OnFileUploaded">
                            </telerik:RadAsyncUpload>
                        </td>
                        
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:UpdatePanel ID="Upd_fu_printHeader" runat="server" ChildrenAsTriggers="false"
                                UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div>
                                        <div id="div_filUp_fu_printHeader" runat="server" style="width: 250px; height: 50px"
                                            visible="false">
                                            <asp:HyperLink ID="hyp_fu_printHeader" runat="server" Target="_blank">
                                                <asp:Image ID="img_upld_fu_printHeader" runat="server" Style="border: 1px solid black;
                                                    border-radius: 5px; float: left; height: 100%; margin: 9px 0 9px 9px; padding: 3px;
                                                    width: 60%;" /></asp:HyperLink>
                                                   <span style="float: left;">
                                                    <asp:Button ID="btnclosePH" Font-Bold="true"  style=" border-radius: 10px; background-color:#e87b7b " 
                                                    runat="server" ToolTip="Delete" OnClick="btnbtnclosePHOnClick" Text="x" />
                                                    </span>
                                            <asp:HiddenField ID="hdn_printHeader" runat="server" Value="" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Print Footer(820 x 115)
                        </td>
                        <td colspan="3">
                            <telerik:RadAsyncUpload ID="fu_printfooter" MaxFileSize="500000000" runat="server"
                                MaxFileInputsCount="1" OnFileUploaded="fu_printfooter_OnFileUploaded">
                            </telerik:RadAsyncUpload>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:UpdatePanel ID="Upd_fu_printfootr" runat="server" ChildrenAsTriggers="false"
                                UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div>
                                        <div id="div_filUp_fu_printfootr" runat="server" style="width: 250px; height: 50px"
                                            visible="false">
                                            <asp:HyperLink ID="hyp_fu_printfootr" runat="server" Target="_blank">
                                                <asp:Image ID="img_upld_fu_printfootr" runat="server" Style="border: 1px solid black;
                                                    border-radius: 5px; float: left; height: 100%; margin: 9px 0 9px 9px; padding: 3px;
                                                    width: 60%;" /> 
                                                    </asp:HyperLink>
                                                    <span style="float: left;">
                                                    <asp:Button ID="btnclosePF" Font-Bold="true"  style=" border-radius: 10px; background-color:#e87b7b " 
                                                    runat="server" ToolTip="Delete" Text="x" OnClick="btnbtnclosePFOnClick" />
                                                    </span>
                                            <asp:HiddenField ID="hdn_printfootr" runat="server" Value="" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                      <tr>
                        <td>
                         Mail Signature (max. 800 x 200)
                        </td>
                        <td colspan="3">
                            <telerik:RadAsyncUpload ID="fu_MailFile" MaxFileSize="500000000" runat="server"
                                MaxFileInputsCount="1" OnFileUploaded="fu_mailsign_OnFileUploaded">
                            </telerik:RadAsyncUpload>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:UpdatePanel ID="updfu_MailFile" runat="server" ChildrenAsTriggers="false"
                                UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div>
                                        <div id="divfu_MailFile" runat="server" style="width: 250px; height: 50px"
                                            visible="false">
                                            <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank">
                                                <asp:Image ID="imgfu_MailFile" runat="server" Style="border: 1px solid black;
                                                    border-radius: 5px; float: left; height: 100%; margin: 9px 0 9px 9px; padding: 3px;
                                                    width: 60%;" /> 
                                                    </asp:HyperLink>
                                                    <span style="float: left;">
                                                    <asp:Button ID="btnfu_MailFile" Font-Bold="true"  style=" border-radius: 10px; background-color:#e87b7b " 
                                                    runat="server" ToolTip="Delete" Text="x" OnClick="btnfu_MailFileOnClick" />
                                                    </span>
                                            <asp:HiddenField ID="hdnfu_MailFile" runat="server" Value="" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                            <asp:HiddenField ID="hdn_user_id" runat="server" Value="0" />
                            <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                            <asp:Button ID="btn_save" class="butn_save" OnClick="btn_save_OnClick" ValidationGroup="save"
                                runat="server" Text="Save/حفظ" />
                        </td>
                    </tr>

                      <tr style="display: none">
                        <td>
                            Default Payment mode for Quick receipt/وضع الدفع الافتراضي للاستلام السريع
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drp_paymode" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search To Payment mode..." Style="overflow: hidden;
                                width: 90%; border: none!important;">
                                <Items>
                                    <telerik:RadComboBoxItem Value="1" Text="Petty Cash" />
                                    <telerik:RadComboBoxItem Value="2" Text="Bank Transaction" />
                                    <telerik:RadComboBoxItem Value="3" Text="Cheque" />
                                    <%--<telerik:RadComboBoxItem Value="4" Text="Credit" />--%>
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                </table>
                <div>
                </div>
                <div>
                    <div id="div_pop2" class="messageAlert div_pop animated" style="display: none" runat="server">
                        <div class="tick">
                            &#10004</div>
                        <div>
                            <asp:Label ID="lbl_msg" runat="server" class="messageLabel"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
