<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="SoftwareConfiguration.aspx.cs" Inherits="AmarCentre.Masters.SoftwareConfiguration" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Software Configuration
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
                            Customer Discount Type
                        </td>
                        <td style="width: 25%">
                            <telerik:RadComboBox ID="drpCDType" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search ..." Style="overflow: hidden;
                                width: 90%; border: none!important;">
                            </telerik:RadComboBox>
                        </td>
                 </tr>
                    
                 <tr>
                     <td colspan="2">
                         <asp:CheckBox ID="chk_SerPriceWTax" runat="server" Text="Service Price With Tax/سعر الخدمة مع الضرائب" />
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
                                 <telerik:RadComboBoxItem Value="3" Text="Format 3" />

                             </Items>
                         </telerik:RadComboBox>
                     </td> 
                 </tr>
                 <tr>
                     <td colspan="2">
                         <asp:CheckBox ID="chkPrintTerms" runat="server" Text="Sales Order Print Terms/شروط طباعة أمر المبيعات" />
                     </td>
                    
                          
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
                                     <telerik:RadComboBoxItem Value="10" Text="Format 10" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                 </tr>
                 <tr>
                     <td colspan="2">
                          <asp:CheckBox ID="chkDepartmentRequired" runat="server" Text="Department Required In Service/القسم مطلوب في الخدمة" />
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
                                 <telerik:RadComboBoxItem Value="7" Text="Format 7" />
                                  <telerik:RadComboBoxItem Value="8" Text="Format 8" />
                             </Items>
                         </telerik:RadComboBox>
                     </td>
                 </tr>
                 <tr>
                     <td colspan="2">
                         <asp:CheckBox ID="chkCategoryRequired" runat="server" Text="Category Required In Service/الفئة المطلوبة في الخدمة" />
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
                                  <telerik:RadComboBoxItem Value="4" Text="Format 4" />
                                  <telerik:RadComboBoxItem Value="5" Text="Format 5" />
                                 <telerik:RadComboBoxItem Value="6" Text="Format 6" />
                             </Items>
                         </telerik:RadComboBox>
                     </td>
                 </tr>
                 <tr>
                     <td colspan="2">
                         <asp:CheckBox ID="chkSubCategoryRequired" runat="server" Text="Sub Category Required In Service/الفئة الفرعية المطلوبة في الخدمة" />
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
                         <asp:CheckBox ID="chkDisplayDiscount" Text="Display Discount in invoice"
                             runat="server" />
                          <asp:CheckBox ID="chkQutnEdit" Text="Quotation Editable in Invoice" Visible="false"  runat="server" />
                     </td>
                      <td>
                     Default Emirate
                     </td>
                     <td>
                         <telerik:RadComboBox ID="drpEmirate" Sort="Ascending" Filter="Contains" runat="server"
                             AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                             OnClientBlur="ValidateCombo" EmptyMessage="Search ..." Style="overflow: hidden;
                             width: 90%; border: none!important;">
                         </telerik:RadComboBox>
                     </td>
                 </tr>
                   <tr>
                     <td colspan="2">
                         <asp:CheckBox ID="chkIsEditInvoiceCreator" runat="server" Text="Is Allow to edit invoice creator" />
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
                         <asp:CheckBox ID="chkremark" Text="Add remark in invoice print" runat="server" />
                     </td>
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
                            <telerik:RadComboBoxItem Value="17" Text="Format 17" />
                             <telerik:RadComboBoxItem Value="18" Text="Format 18" />
                             <telerik:RadComboBoxItem Value="19" Text="Format 19" />
                             <telerik:RadComboBoxItem Value="20" Text="Format 20" />
                              <telerik:RadComboBoxItem Value="21" Text="Format 21" />
                               <telerik:RadComboBoxItem Value="22" Text="Format 22" />
                            <telerik:RadComboBoxItem Value="23" Text="Format 23" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
                    
                 </tr>
                 
                 <tr>
                      <td colspan="2">
                        <asp:CheckBox ID="chkIsDisableRoundOff" Text="Is Disable Round Off" runat="server" />
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
                            <asp:CheckBox ID="chkIsSoftareNameAdd" Text="Print Software Name in Invoice" runat="server" />
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
                           <asp:CheckBox ID="chktemplate" Text="Display Template name in invoice Print" runat="server" />
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
                       <asp:CheckBox ID="chkdepartmentInInvoiceVisible" Text="Display Department in Invoice" runat="server" />
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
                               <asp:CheckBox ID="chkIsDisplaySCStatus" Text="Display SC Process Status" runat="server" />
                        </td>
                       <td>
                          Agent Commission Type
                       </td>
                       <td>
                           <telerik:RadComboBox ID="drpAgentCommission" Sort="Ascending" Filter="Contains"
                               runat="server" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                               OnClientBlur="ValidateCombo" EmptyMessage="Search Agent Commission Type..." Style="overflow: hidden;
                               width: 90%; border: none!important;">
                               <Items>
                                   <telerik:RadComboBoxItem Value="1" Text="Profit" />
                                   <telerik:RadComboBoxItem Value="2" Text="Service wise" />
                               </Items>
                           </telerik:RadComboBox>
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drpAgentCommission"
                               ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                               InitialValue=""></asp:RequiredFieldValidator>
                       </td>
                     
                 </tr>
                <tr>
                       <td colspan="2">
                         <asp:CheckBox ID="chkIsHideServiceAmtInSC" Text="Hide ServiceAmt In SC" runat="server" />
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
     <asp:CheckBox ID="chkIncentivePercentage" Text="Is Incentive Calculation in Percentage" runat="server" />
</td>
                      <td > Predate Days in SC
                     </td>
                      <td style="width: 29%">
                         <asp:TextBox ID="txtscpredate" CssClass="txt numbers_only" Style="width: 90%;" runat="server"></asp:TextBox>
                     </td>
                 </tr>
                 <tr>
                     <td colspan="2">
                         <asp:CheckBox ID="chkIsAddCreatedByInInvoicePrint" Text="Is Add CreatedBy In InvoicePrint" runat="server" />
                     </td>
                     <td colspan="2"></td>
                 </tr>
                  <tr>
     <td colspan="2">
         <asp:CheckBox ID="chkIsSCViewDepartmentBase" Text="Is SCView Department Based" runat="server" />
     </td>
     <td colspan="2"></td>
 </tr>
                 <tr style="display:none">
                     <td colspan="2">
                         <asp:CheckBox ID="chkCustinv" Text="Enable Customer Invoice" runat="server" />
                     </td>
                     <td colspan="2"></td>
                 </tr>
                 
                 <tr>
                  
                     <td>Default Quotation Remark
                     </td>
                     <td rowspan="2">
                         <asp:TextBox class="txtarea" Style="width: 90%" TextMode="MultiLine" ID="txtQremark"
                             runat="server"></asp:TextBox>
                     </td>

                        <td rowspan="2">Default Invoice Remark
                        </td>
                        <td rowspan="2">
                            <asp:TextBox class="txtarea" Style="width: 90%" TextMode="MultiLine" ID="txtDinvoiceremark"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>

               <tr>


                         <%--<td colspan="2">
                          </td>--%>
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
