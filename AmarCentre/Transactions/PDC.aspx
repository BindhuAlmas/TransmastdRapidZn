<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true"
    CodeBehind="PDC.aspx.cs" Inherits="AmarCentre.Transactions.PDC" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        <asp:HiddenField ID="hdn_user_id" runat="server" Value="" />
        <asp:HiddenField ID="hdn_add" runat="server" />
        PDC List
        <asp:Button ID="btnexcel_export" runat="server" class="btn_excel right_align_list"
            ToolTip="Export to Excel" OnClick="btnexcel_export_OnClick" />
        <div class="searchDiv">
            <asp:TextBox ID="txt_search" runat="server" class="txt_search" AutoPostBack="true" OnTextChanged="drpStatusOnSelectedIndexChanged"
                placeholder="Search"></asp:TextBox>
        </div>
         <telerik:RadComboBox ID="drpStatus" Sort="Ascending" Filter="Contains" runat="server"
            AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
            OnClientBlur="ValidateCombo" EmptyMessage="Search Status..." AutoPostBack="true"
            OnSelectedIndexChanged="drpStatusOnSelectedIndexChanged" style="overflow: hidden;
            width: 16%; border: none!important; float: right; padding-right: 5px; margin-top: 0px">
            <Items>
                <telerik:RadComboBoxItem Value="1" Selected="true" Text="Pending" />
                <telerik:RadComboBoxItem Value="2" Text="Processed" />
            </Items>
        </telerik:RadComboBox>
    </div>
    <div>
        <asp:UpdatePanel ID="Upd_List_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <table class="listTable">
                    <thead>
                        <tr>
                            <th style="width:5%">
                                Sl 
                            </th>
                            <th style="width:10%">
                                Ref No
                            </th>
                            <th style="width: 10%;">
                                Cheque Date/ تحقق من التاريخ
                            </th>
                            <th style="width: 10%;">
                                Cheque No
                            </th>
                            <th style="width: 20%;">
                                From/من عند
                            </th>
                            <th style="width: 10%;">
                                To/إلى
                            </th>
                            <th style="width: 10%;">
                                Amount/المبلغ
                            </th>
                            <th style="width: 10%;">
                                Type/نوع
                            </th>
                            <th style="width: 5%;">
                                Action/عمل
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rpt_list" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%# Container.ItemIndex + 1 %>.
                                        <asp:HiddenField ID="hdn_id" runat="server" Value='<%#Eval("Id")%>' />
                                        <asp:HiddenField ID="hdnTypeId" runat="server" Value='<%#Eval("TypeId")%>' />
                                    </td>
                                    <td>
                                        <%#Eval("Code")%>
                                    </td>
                                    <td>
                                        <asp:Label ID="ChequeDate" runat="server" Text=' <%#Eval("ChequeDate")%>' CssClass="lbl"></asp:Label>
                                    </td>
                                     <td>
                                        <%#Eval("ChequeNo")%>
                                    </td>
                                    <td>
                                        <asp:Label ID="PaidFrom" runat="server" Text=' <%#Eval("PaidFrom")%>' CssClass="lbl"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Receiver" runat="server" Text=' <%#Eval("Receiver")%>' CssClass="lbl"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Amount" runat="server" Text=' <%#Eval("Amount")%>' CssClass="lbl"></asp:Label>
                                        <asp:HiddenField ID="hdnaftercom" runat="server" Value='<%#Eval("AfterCommission")%>' />
                                    </td>
                                    <td>
                                        <asp:Label ID="Type" runat="server" Text=' <%#Eval("Type")%>' CssClass="lbl"></asp:Label>
                                    </td>
                                    <td class="listTableActionButtonDiv">
                                        <asp:Button ID="btn_edit" runat="server" class="btn_edit" OnClick="listAction" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnexcel_export" />
            </Triggers>
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
                    <div class="animated smallPopUp">
                        <div class="Adding_heading">
                            PDC Closing/اغلاق قيمة الشيكات المؤجلة
                        </div>
                        <table class="formTable">
                            <tr>
                                <td>
                                    Date/تاريخ
                                    <asp:Label ID="lblChequeDate" runat="server" CssClass="lbl labelOutput"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    To/إلى
                                    <asp:Label ID="lblFrom" runat="server" CssClass="lbl labelOutput"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Amount/المبلغ
                                    <asp:Label ID="lblAmount" runat="server" CssClass="lbl labelOutput"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Amount After Commission/المبلغ بعد العمولة
                                    <asp:Label ID="lblAmountAfterComm" runat="server" CssClass="lbl labelOutput"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    From Bank/من البنك
                                    <asp:Label ID="lblfromBank" runat="server" CssClass="lbl labelOutput"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Collection Date/تاريخ التحصيل
                                    <telerik:RadDatePicker ID="dtCollectionDate" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                        <Calendar runat="server" ID="Calendar1" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                            ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                            <SpecialDays>
                                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                </telerik:RadCalendarDay>
                                            </SpecialDays>
                                        </Calendar>
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="dtCollectionDate"
                                        ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                        InitialValue=""></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btn_save" class="butn_save" ValidationGroup="save" OnClick="ClosePaymentCheque"
                                        runat="server" Text="Set as paid/تعيين المدفوعات " />
                                    <asp:Button ID="btnPaymentClose" class="butn" runat="server" Text="Close/أغلق" OnClick="btnPaymentClose_OnClick" />
                                    <asp:HiddenField ID="hdnId" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <div>
                    <div id="div_pop2" class="messageAlert div_pop animated" style="display: none" runat="server">
                        <div class="tick">
                            &#10004</div>
                        <div>
                            <asp:Label ID="lbl_msgPayment" runat="server" class="messageLabel"></asp:Label>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
        <asp:UpdatePanel ID="updUpdateReceiptCheque" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlReceipt" Visible="false" runat="server">
                    <div class="popupBackground">
                    </div>
                    <div class="animated smallPopUp">
                        <div class="Adding_heading">
                            PDC Closing-Receivable/اغلاق قيمة الشيكات المستحقة
                        </div>
                        <table class="formTable">
                            <tr>
                                <td>
                                    Date/تاريخ
                                    <asp:Label ID="lblRcvChequeDate" runat="server" CssClass="lbl labelOutput"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    From /من عند
                                    <asp:Label ID="lblRcvFrom" runat="server" CssClass="lbl labelOutput"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Amount/المبلغ
                                    <asp:Label ID="lblRcvAmount" runat="server" CssClass="lbl labelOutput"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Collection Date/تاريخ التحصيل
                                    <telerik:RadDatePicker ID="dtRcvCollectionDate" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                        <Calendar runat="server" ID="Calendar2" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                            ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                            <SpecialDays>
                                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                </telerik:RadCalendarDay>
                                            </SpecialDays>
                                        </Calendar>
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="dtRcvCollectionDate"
                                        ValidationGroup="save1" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                        InitialValue=""></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Bank/بنك
                                    <telerik:RadComboBox ID="drpBankAccount" Sort="Ascending" Filter="Contains" runat="server"
                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                        AutoPostBack="true" OnClientBlur="ValidateCombo" EmptyMessage="Search Name/اسم..."
                                        Style="overflow: hidden; width: 96%; border: none!important;" Visible="false">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="rqTo" runat="server" ControlToValidate="drpBankAccount"
                                        ValidationGroup="save1" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                        InitialValue=""></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnSaveReceiptCheque" class="butn_save" ValidationGroup="save1" OnClick="CloseReceiptCheque"
                                        runat="server" Text="Set as Received/تعيين المدفوعات " />
                                    <%-- <asp:Button ID="btnReset" class="butn_delete" runat="server" OnClientClick="javascript : return confirm('Do you really want to reset.. ?');"
                                                        Visible="true" Text="Reset/إعادة تعيين" OnClick="btnReset_OnClick" />--%>
                                    <asp:Button ID="btnReceiptClose" class="butn" runat="server" Text="Close/أغلق" OnClick="btnReceiptClose_OnClick" />
                                    <asp:HiddenField ID="hdnRecId" runat="server" />
                                    <asp:HiddenField ID="hdnRecTypeId" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <div>
                    <div>
                        <div id="div1" class="messageAlert div_pop animated" style="display: none" runat="server">
                            <div class="tick">
                                &#10004</div>
                            <div>
                                <asp:Label ID="lbl_msgReceipt" runat="server" class="messageLabel"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
    </div>
</asp:Content>
