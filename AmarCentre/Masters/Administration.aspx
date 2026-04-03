<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="Administration.aspx.cs" Inherits="AmarCentre.Masters.Administration" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Administration
        <div class="searchDiv">
        </div>
    </div>
    <asp:UpdatePanel ID="Upd_Add_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <div class="list_info" style="display: none">
                </div>
                <table style="width: 98%">
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chkTaxAppliedWithDiscount" Text="Tax Applied With Discount"
                                runat="server" />
                        </td>
                        <td>Company Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtCompanyname" CssClass="txt" Style="width: 90%;" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chkscinvoice" Text="SC in invoice" runat="server" />
                        </td>
                         <td>Company Phone No
                        </td>
                        <td>
                            <asp:TextBox ID="txtCompanyPhone" CssClass="txt" Style="width: 90%;" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chkEmpSC" Text="Employee Based Transaction" runat="server" />
                        </td>
                         <td>Company Contact Person Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtCompanyContactPerson" CssClass="txt" Style="width: 90%;" runat="server"></asp:TextBox>
                        </td>
                    </tr>


                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chkIsMobileDupAllow" Text="Allow Customer Mobile no Duplication" runat="server" />
                        </td>
                        <td>TRN
                        </td>
                        <td>
                            <asp:TextBox ID="txtTRN" CssClass="txt" Style="width: 90%;" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chkIstaxprintforall" Text="Tax Print for all Customer" runat="server" />
                        </td>
                       <td>Default Invoice Type<span style="color: Red">&nbsp*</span>

                        </td>
                        <td>
                            <telerik:RadComboBox ID="drpDefaultInvoiceType" Sort="Ascending" Filter="Contains"
                                runat="server" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search Invoice Type..." Style="overflow: hidden; width: 90%; border: none!important;">
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
                            <asp:CheckBox ID="chkIsAllowSCAmountExceed" Text="Is Allow SC Amount Exceed Service Amount" runat="server" />
                        </td>
                       <td style="width: 25%">Company Email
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
                            <asp:CheckBox ID="chkIsCommissionEditableInInvoice" Text="Is Commission Editable In Invoice" runat="server" />
                        </td>
                        <td>Company Email Password
                        </td>
                        <td>
                            <asp:TextBox ID="txtCompanyEmailPwd" CssClass="txt" TextMode="Password" Style="width: 90%;"
                                autocomplete="new-password" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chkdeltdSC" Text="Include deleted SC in Statment" runat="server" />
                        </td>
                         <td>CC Mail
                        </td>
                        <td>
                            <asp:TextBox ID="txtccmail" CssClass="txt" Style="width: 90%;" runat="server"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>

                        <td>Fine Expense 
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drpExpense" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search Expense..." Style="overflow: hidden; width: 90%; border: none!important;">
                            </telerik:RadComboBox>
                        </td>
                          <td>Expiry Email Template
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
                        <td>Admin Designation</td>
                        <td>
                            <telerik:RadComboBox ID="drpAdminDesign" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search Designation..." Style="overflow: hidden; width: 90%; border: none!important;">
                            </telerik:RadComboBox>
                        </td>
                        <td style="width: 25%">Primary mail notification day
                        </td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtSendAgreementExpiredMailBefore" CssClass="txt numbers_only" Style="width: 90%;"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td>Refundable Expense Type
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drprefundexpense" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search ..." Style="overflow: hidden; width: 90%; border: none!important;">
                            </telerik:RadComboBox>
                        </td>
                          <td style="width: 25%">Secondary mail notification day
                        </td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtDocExpireSecondaryMailDays" CssClass="txt numbers_only" Style="width: 90%;"
                                runat="server"></asp:TextBox>
                        </td>
                       
                    </tr>
                    <tr>
                        <td style="width: 25%">Default BankCharge(%)
                        </td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtDefaultBankCharge" CssClass="txt numbers_only" Style="width: 90%;" runat="server"></asp:TextBox>
                        </td>
                        <td style="width: 25%">VAT OpeningBalance
                        </td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtVATOB" CssClass="txt numbers_only" Style="width: 90%;"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"></td>
                        <td>VAT OBDate</td>
                        <td>
                            <telerik:RadDatePicker ID="radVATOBDate" runat="server" DateInput-DateFormat="dd/MM/yyyy">
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
                        <td>Print Header(1200 x 215)
                        </td>
                        <td colspan="3">
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
                                                <asp:Image ID="img_upld_fu_printHeader" runat="server" Style="border: 1px solid black; border-radius: 5px; float: left; height: 100%; margin: 9px 0 9px 9px; padding: 3px; width: 60%;" />
                                            </asp:HyperLink>
                                            <span style="float: left;">
                                                <asp:Button ID="btnclosePH" Font-Bold="true" Style="border-radius: 10px; background-color: #e87b7b"
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
                        <td>Print Footer(820 x 115)
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
                                                <asp:Image ID="img_upld_fu_printfootr" runat="server" Style="border: 1px solid black; border-radius: 5px; float: left; height: 100%; margin: 9px 0 9px 9px; padding: 3px; width: 60%;" />
                                            </asp:HyperLink>
                                            <span style="float: left;">
                                                <asp:Button ID="btnclosePF" Font-Bold="true" Style="border-radius: 10px; background-color: #e87b7b"
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
                        <td>Mail Signature (max. 800 x 200)
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
                                                <asp:Image ID="imgfu_MailFile" runat="server" Style="border: 1px solid black; border-radius: 5px; float: left; height: 100%; margin: 9px 0 9px 9px; padding: 3px; width: 60%;" />
                                            </asp:HyperLink>
                                            <span style="float: left;">
                                                <asp:Button ID="btnfu_MailFile" Font-Bold="true" Style="border-radius: 10px; background-color: #e87b7b"
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
                        <td>Default Payment mode for Quick receipt/وضع الدفع الافتراضي للاستلام السريع
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drp_paymode" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search To Payment mode..." Style="overflow: hidden; width: 90%; border: none!important;">
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
                            &#10004
                        </div>
                        <div>
                            <asp:Label ID="lbl_msg" runat="server" class="messageLabel"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
