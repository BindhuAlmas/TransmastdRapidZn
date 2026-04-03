<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true"
    CodeBehind="Customer.aspx.cs" Inherits="AmarCentre.Masters.Customer" %>
<%@ Register Src="~/Transactions/UserControl/UCMail.ascx" TagName="MailUC" TagPrefix="AmarCentre" %>

<%@ Register Src="~/Masters/UserControl/UCCustCategory.ascx" TagName="CCategory"
    TagPrefix="AmarCentre" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function ShowMenuForm(id) {
            window.radopen("DisplayMenuCustomer.aspx?userid=" + id, "Menudisplay");
            return false;
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
            $('.readOnly').attr('readonly', true);

            $('.CommonDiscount').blur(function (e) {
                $('.disAmt').val($('.CommonDiscount').val());
                DiscountCalculation();
            });


            $('.disAmt,.addAmt').blur(function (e) {
                DiscountCalculation();
            });

            function DiscountCalculation() {
                $('.disAmt').each(function () {
                    var Amt = 0;
                    var DisAmt = 0;
                    var AfterDis = 0;
                    var addAmt = 0;

                    if ($(this).closest("tr").find('.amt').val() != '') {
                        Amt = parseFloat($(this).closest("tr").find('.amt').val());
                    }
                    if ($(this).closest("tr").find('.disAmt').val() != '') {
                        DisAmt = parseFloat($(this).closest("tr").find('.disAmt').val());
                    }
                    if ($(this).closest("tr").find('.addAmt').val() != '') {
                        addAmt = parseFloat($(this).closest("tr").find('.addAmt').val());
                    }
                    AfterDis = parseFloat(Amt) - parseFloat(DisAmt) + parseFloat(addAmt);;

                    $(this).closest("tr").find('.afterDis').val(parseFloat(AfterDis).toFixed(2));
                });
            }


        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Customer/زبون
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
                            <th style="width: 5%;">Sl No/رقم
                            </th>
                            <th style="width: 20%;">Name/اسم
                            </th>
                            <th style="width: 15%;">Contact Person
                            </th>
                            <th style="width: 7%;">Mobile/هاتف
                            </th>
                            <th style="width: 10%;">Remark/تعليق
                            </th>
                            <th style="width: 5%;">Action/عمل
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
                                        <%#Eval("Name")%>
                                    </td>
                                    <td>
                                        <%#Eval("ContactPerson")%>
                                    </td>
                                    <td>
                                        <%#Eval("Mobile_num")%>
                                    </td>
                                    <td>
                                        <%#Eval("ShortDescription")%>
                                    </td>
                                    <td class="listTableActionButtonDiv">
                                        <asp:Button ID="btn_edit" runat="server" class="btn_edit" />
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
                        <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="Adding_heading">
                                    Customer/زبون
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td style="width: 47%">Name/اسم <span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txt_name" CssClass="txt" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txt_name"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 47%">Arabic Name/الاسم بالعربي </span>
                                            <asp:TextBox ID="txtArabicName" CssClass="txt" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Agent/وكيل
                                            <telerik:RadComboBox ID="drpagent" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" RenderMode="Lightweight"
                                                EmptyMessage="Search Agent..." OnClientFocus="OnClientKeyPressing" Style="overflow: hidden; width: 97%; border: none!important;">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>Sponsor
                                             <telerik:RadComboBox ID="drpSponser" Sort="Ascending" Filter="Contains" runat="server"
                                                 AllowCustomText="false" RenderMode="Lightweight"
                                                 EmptyMessage="Search Sponsor..." OnClientFocus="OnClientKeyPressing" Style="overflow: hidden; width: 97%; border: none!important;">
                                             </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Mobile /هاتف<span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txt_mob" runat="server" MaxLength="10" class="txt numbers_only"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_mob"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Please Enter 10 digit"
                                                ValidationGroup="save" ControlToValidate="txt_mob" Style="color: Red"
                                                ValidationExpression="^[0-9]{10}$" Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                        </td>
                                        <td>Phone Number/رقم الهاتف
                                            <br />
                                            <asp:TextBox ID="txt_phn" runat="server" class="txt numbers_only"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Email (with comma if multiple)
                                            <br />
                                            <asp:TextBox ID="txt_email" CssClass="txt" TextMode="MultiLine" runat="server"></asp:TextBox>
                                          <%--  <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Please Enter Valid Email ID"
                                                ValidationGroup="save" ControlToValidate="txt_email" Style="color: Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                Display="Dynamic">
                                            </asp:RegularExpressionValidator>--%>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txt_email"
                                                ValidationGroup="mail" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        <td>CC Email (with comma if multiple)
                                            <br />
                                            <asp:TextBox ID="txtccmail" CssClass="txt" TextMode="MultiLine" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>TRN
                                            <br />
                                            <asp:TextBox ID="txt_trn" CssClass="txt" runat="server"></asp:TextBox>
                                        </td>
                                        <td>Contact Person
                                            <br />
                                            <asp:TextBox ID="txtCperson" CssClass="txt" runat="server"></asp:TextBox>

                                        </td>

                                    </tr>
                                    <tr>
                                        <td>MOHRE No
                                            <br />
                                            <asp:TextBox ID="txtmohre" CssClass="txt" runat="server"></asp:TextBox>
                                        </td>
                                        <td>License No
                                            <br />
                                            <asp:TextBox ID="txtlicense" CssClass="txt" runat="server"></asp:TextBox>

                                        </td>

                                    </tr>
                                    <tr>
                                        <td>Emirate 
                                            <telerik:RadComboBox ID="drpEmirate" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search ..."
                                                OnClientFocus="OnClientKeyPressing" OnClientBlur="ValidateCombo" Style="overflow: hidden; width: 96%; border: none!important;">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>Customer Category
                                             <asp:UpdatePanel ID="updCategory" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                 <ContentTemplate>
                                                     <telerik:RadComboBox ID="drpCategory" Sort="Ascending" Filter="Contains" runat="server"
                                                         AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                         OnClientBlur="ValidateCombo" EmptyMessage="Search ..." AutoPostBack="true"
                                                         OnSelectedIndexChanged="drpCategory_SelectedIndexChanged"
                                                         Style="overflow: hidden; width: 96%; border: none!important;">
                                                     </telerik:RadComboBox>
                                                 </ContentTemplate>
                                             </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>User Name/اسم المستخدم  
                             <asp:TextBox ID="txt_userName" class="txt " runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txt_userName"
                                                ValidationGroup="mail" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        <td>Password /كلمة مرور 
                                  <asp:TextBox ID="txt_password" class="txt" autocomplete="new-password" TextMode="Password" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txt_password"
                                                ValidationGroup="mail" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>WhatsApp Number (with CountryCode)
                             <asp:TextBox ID="txtWhatsappNo" runat="server" MaxLength="20" class="txt"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Enter Valid Character"
                                                ValidationGroup="saveCustomer" ControlToValidate="txtWhatsappNo" Style="color: Red"
                                                ValidationExpression="^[0-9+]+$" Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                        </td>
                                        <td>
                                                    <asp:Panel ID="pnlcompanygrp" runat="server" Visible="false">
                                                        Company Group
                                              <telerik:RadComboBox ID="drpcompanygrp" Sort="Ascending" Filter="Contains" runat="server"
                                                  AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                  OnClientBlur="ValidateCombo" EmptyMessage="Search ..."
                                                  Style="overflow: hidden; width: 96%; border: none!important;">
                                              </telerik:RadComboBox>
                                                    </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Address/العنوان
                                            <br />
                                            <asp:TextBox ID="txt_address" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                        <td>Remark/تعليق
                                            <br />
                                            <asp:TextBox ID="txt_remark" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                                    <asp:Panel ID="pnlchkcompanygrp" runat="server" Visible="false">
                                                        <asp:CheckBox ID="chkcompanygrp" runat="server" Text="Is Main Company" />
                                                    </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkCommissionApplicable" runat="server" Text="" />Is Commission
                                            Applicable/هل تطبيق العمولة
                                      <br />
                                            <asp:CheckBox ID="chkIsTyping" runat="server" Text="Is Typing Center" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chk_IsCredit" runat="server" Text="" AutoPostBack="true" OnCheckedChanged="chk_IsCredit_OnCheckedChanged" />Is
                                            Credit/هو الائتمان
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="Upd_CreditAmount_Panel" runat="server" ChildrenAsTriggers="false"
                                                UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnl_CreditAmount" runat="server" Visible="false">
                                                        Credit Amount/المبلغ الائتمان <span style="color: Red">&nbsp*</span>
                                                        <asp:TextBox ID="txt_CreditAmount" runat="server" class="txt numbers_only"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_CreditAmount"
                                                            ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                            InitialValue=""></asp:RequiredFieldValidator>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table class="listTable">
                                                <thead>
                                                    <tr>
                                                        <th>Receivable/ذمم مدينة
                                                        </th>
                                                        <th>Advance/مقدم
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblReceivable" runat="server" class="lbl"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblPayable" runat="server" class="lbl"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div>
                                                <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_user_id" runat="server" />
                                                <asp:Button ID="btn_save" class="butn_save" ValidationGroup="save" OnClick="btn_save_OnClick"
                                                    runat="server" Text="Save/حفظ" />
                                                <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_OnClick" />
                                                <asp:Button ID="btn_delete" class="butn_delete" runat="server" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');"
                                                    Visible="false" Text="Delete/حذف" OnClick="btn_delete_OnClick" />
                                                <asp:Button ID="btn_OB" class="butn_save" runat="server" Visible="false" Text="Opening Balance/الرصيد المفتوح "
                                                    OnClick="btn_OB_OnClick" />
                                                <asp:Button ID="btn_doc" class="butn" runat="server" Visible="false" Text="Company Documents/وثيقة"
                                                    OnClick="btn_docadd_OnClick" />
                                                <asp:Button ID="btn_doc_Staff" class="butn" runat="server" Visible="false" Text="Staff Documents/مستندات العاملين"
                                                    OnClick="btn_docadd_OnClick_Staff" />
                                                <asp:Button ID="btn_usercred" class="butn" runat="server" Visible="false" Text="Crediential/الاعتماد "
                                                    OnClick="btn_usercred_OnClick" />
                                                <asp:Button ID="btn_serviceDiscount" class="butn_save" runat="server" Visible="false"
                                                    Text="Service Discount/خصم الخدمة" OnClick="btn_serviceDiscount_OnClick" />
                                                <asp:Button ID="btnMenuPrivilge" class="butn" runat="server" Visible="false" Text="Menu Privilege"
                                                    OnClick="btnmenu_Click" />
                                                <asp:Button ID="btnmail" class="butn_save" ValidationGroup="mail" OnClick="btnmail_OnClick"
                                                    runat="server" Text="Send Login Detail" />
                                                <asp:Button ID="btn_Mailhistory" class="butn" runat="server" Visible="false" Text="Mail History"
                                                    OnClick="btn_Mailhistry_OnClick" />
                                                <asp:Button ID="Button2" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_OnClick" />
                                                <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_update" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_delete" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_OB" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_doc" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_cred" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_servicediscount" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnmenuprivilege" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_doc_Staff" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_histry" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnmail" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnDefaultEmirate" runat="server" />
                                                <asp:HiddenField ID="hdnIsprofessionversion" runat="server" />

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
                            <asp:Label ID="lbl_msg" runat="server" class="messageLabel"></asp:Label>
                        </div>
                    </div>
                </div>
                <div>
                    <div id="div1" class="messageAlerterror div_poperror animated" style="display: none" runat="server">
                        <div class="tick">
                            &#10007
                        </div>
                        <div>
                            <asp:Label ID="lblerrormsg" runat="server" class="messageLabel"></asp:Label>
                        </div>
                    </div>
                </div>

                <%-- Customer Mail History               ----------------------------------------------------------%>

                <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="PanelHis" Visible="false" runat="server">
                            <div class="popupBackground">
                            </div>
                            <div class="animated largePopUp">
                                <div class="Adding_heading">
                                    Mail History
                                </div>

                                <table style="margin-left: 20px; width: 60%; display: none;">
                                    <tr>
                                        <td>From/من عند
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
                                        <td>To/إلى
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
                                            <asp:Button ID="Button4" class="butn" OnClick="btn_his_seacrh_OnClick" runat="server"
                                                Text="Search" />
                                        </td>
                                    </tr>
                                </table>

                                <table style="margin-left: 20px; width: 70%;">
                                    <tr>
                                        <td style="width: 25%">Document Type
                                            <telerik:RadComboBox ID="drpDocument" Sort="Ascending" EmptyMessage="Search Document..."
                                                Filter="Contains" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                runat="server" Style="height: 24px !important; width: 86%; overflow: hidden; border: none!important;">
                                            </telerik:RadComboBox>
                                        </td>

                                        <td style="width: 25%">Customer Staff
                                            <telerik:RadComboBox ID="drpCustStaff" Sort="Ascending" EmptyMessage="Search Customer Staff..."
                                                Filter="Contains" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                runat="server" Style="height: 24px !important; width: 86%; overflow: hidden; border: none!important;">
                                            </telerik:RadComboBox>
                                        </td>

                                        <td>
                                            <asp:Button ID="Button12" class="butn" OnClick="btn_his_seacrh_OnClick" runat="server"
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
                                                    <div id="div_menu" runat="server" style="width: 100%; height: 70%; overflow: auto;">
                                                        <asp:Button ID="btn_ex_his" runat="server" Style="float: right" class="btn_excel right_align_list"
                                                            ToolTip="Export to Excel" OnClick="btnexcel_exportHis_OnClick" />
                                                        <table class="listTable">
                                                            <thead>
                                                                <tr>
                                                                    <th style="text-align: center; width: 5%">Sl
                                                                    </th>
                                                                    <th style="text-align: center; width: 15%">Document Type
                                                                    </th>
                                                                    <th style="text-align: center; width: 20%">Staff Name
                                                                    </th>
                                                                    <th style="text-align: center; width: 10%">Expiry Date
                                                                    </th>

                                                                    <th style="text-align: center; width: 10%">Mail Sent By
                                                                    </th>
                                                                    <th style="text-align: center; width: 10%">Mail Sent On
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
                                                                            <%#Eval("Name")%>
                                                                        </td>
                                                                        <td>
                                                                            <%#Eval("StaffName")%>
                                                                        </td>
                                                                        <td>
                                                                            <%#Eval("ExpiryDate")%>
                                                                        </td>

                                                                        <td>
                                                                            <%#Eval("DoneBy")%>
                                                                        </td>
                                                                        <td>
                                                                            <%#Eval("MailSentOn")%>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                            <tr>
                                                                <td colspan="6" class="navigationRow">
                                                                    <asp:UpdatePanel ID="upd_his_nav" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:Label ID="lbl_page_info1" runat="server" class="pageInfo"></asp:Label>
                                                                            <asp:Button ID="Button7" runat="server" class="navigationButton" Text="<<" OnClick="btn_first1_Mail_OnClick" />
                                                                            <asp:Button ID="Button8" runat="server" class="navigationButton" Text="<" OnClick="btn_prev1_Mail_OnClick" />
                                                                            <asp:Label ID="lbl_page_number1" Style="font-weight: bold; margin-left: 5px; margin-right: 5px; text-align: center;"
                                                                                runat="server"></asp:Label>
                                                                            <asp:Button ID="Button9" class="navigationButton" runat="server" Text=">" OnClick="btn_next1_Mail_OnClick" />
                                                                            <asp:Button ID="Button10" class="navigationButton" runat="server" Text=">>" OnClick="btn_last1_Mail_OnClick" />
                                                                            <asp:DropDownList ID="drp_count1" class="pageSize" runat="server" AutoPostBack="true"
                                                                                OnSelectedIndexChanged="drp_count1_OnSelectedIndexChanged">
                                                                                <asp:ListItem Text="12" Value="12"></asp:ListItem>
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
                                            <asp:Button ID="Button11" class="butn" runat="server" Text="Close" OnClick="btn_histry_Close_OnClick" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>




                <%--                -----------------------------------------------------------%>

                          <asp:UpdatePanel ID="UpdMailPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" style="height: 100%;">
    <ContentTemplate>
        <asp:Panel ID="pnlMail" Visible="false" runat="server">
            <AmarCentre:MailUC ID="EmailUC" runat="server" />
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

                <asp:UpdatePanel ID="Upd_OB_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnl_obalance" Visible="false" runat="server">
                            <div class="popupBackground">
                            </div>
                            <div class="animated smallPopUp">
                                <div class="Adding_heading">
                                    Opening Balance/الرصيد المفتوح
                                </div>
                                <asp:UpdatePanel ID="Upd_OBIn" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table class="formTable">
                                            <tr>
                                                <td>Balance Type/نوع الرصيد <span style="color: Red">&nbsp*</span>
                                                    <telerik:RadComboBox ID="drp_obType" Sort="Ascending" Filter="Contains" runat="server"
                                                        AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                        OnClientBlur="ValidateCombo" EmptyMessage="Search Balance Type..." Style="overflow: hidden; width: 96%; border: none!important;">
                                                        <Items>
                                                            <telerik:RadComboBoxItem Value="1" Text="Receivable" />
                                                            <telerik:RadComboBoxItem Value="2" Text="Payable" />
                                                        </Items>
                                                    </telerik:RadComboBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="drp_obType"
                                                        ValidationGroup="Ob_add" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Balance/توازن <span style="color: Red">&nbsp*</span>
                                                    <asp:TextBox ID="txt_open_bal" runat="server" class="txt numbers_only"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_open_bal"
                                                        ValidationGroup="Ob_add" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Date/تاريخ <span style="color: Red">&nbsp*</span>
                                                    <telerik:RadDatePicker ID="ob_date" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                        <Calendar runat="server" ID="Calendar3" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                            ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                            <SpecialDays>
                                                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                </telerik:RadCalendarDay>
                                                            </SpecialDays>
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                                    <asp:RequiredFieldValidator ID="Rqd_date" runat="server" ControlToValidate="ob_date"
                                                        ValidationGroup="Ob_add" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div>
                                                        <asp:Label ID="lblerr" runat="server" Visible="false"></asp:Label>
                                                        <asp:Button ID="btn_OBSave" runat="server" class="butn_save" ValidationGroup="Ob_add"
                                                            Text="Save/حفظ" OnClick="btn_OBSave_OnClick" />
                                                        <asp:Button ID="btnOBClear" runat="server" class="butn_save" Text="Clear OB" OnClick="btn_OBClear_OnClick" />
                                                        <asp:Button ID="btn_close" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_ob_OnClick" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="updCategoryPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" style="height: 100%;">
                    <ContentTemplate>
                        <asp:Panel ID="pnlCategory" Visible="false" runat="server">
                            <AmarCentre:CCategory ID="UCCategory" runat="server" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="Upd_Document_Panel" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnl_document" Visible="false" runat="server">
                            <div class="popupBackground">
                            </div>
                            <div class="animated largePopUp">
                                <div class="Adding_heading">
                                    Document/وثيقة
                                </div>
                                <div style="margin-top: 10px;">
                                    <span style="margin-left: 25px; font-weight: bold; font-size: 1.2em;">Customer:</span>
                                    <asp:Label ID="lbl_cusName" runat="server" Style="font-weight: bold; font-size: 1.2em;"></asp:Label>
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="Upd_docadd" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div id="div_document_new" runat="server">
                                                        <table class="formTable">
                                                            <tr>
                                                                <td style="width: 20%">Document Type<span style="color: Red">&nbsp*</span>
                                                                    <telerik:RadComboBox ID="drp_doc" Sort="Ascending" Filter="Contains" runat="server"
                                                                        AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search Document..."
                                                                        Style="overflow: hidden; width: 96%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                        OnClientBlur="ValidateCombo">
                                                                    </telerik:RadComboBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="drp_doc"
                                                                        ValidationGroup="doc_add" ErrorMessage="Required" Display="Dynamic" Style="color: Red"
                                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td style="width: 75%; border-left: 1px solid gray" rowspan="9">
                                                                    <div class="HeadIng_Div">
                                                                        Document List/قائمة الخصم
                                    <div class="searchDiv">
                                        <asp:TextBox ID="txt_search_doc" runat="server" AutoPostBack="true" OnTextChanged="txt_doc_search_OnTextChanged"
                                            class="txt_search" placeholder="Search" Style="float: right; width: 61%"></asp:TextBox>
                                    </div>
                                                                    </div>

                                                                    <div>
                                                                        <asp:UpdatePanel ID="Upd_doc" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <table class="listTable">
                                                                                    <thead>
                                                                                        <tr>
                                                                                            <th class="listTableSlNo">Sl No/رقم
                                                                                            </th>
                                                                                            <th style="width: 200px;">Document Name/اسم المستندات
                                                                                            </th>
                                                                                            <th>Document Type
                                                                                            </th>
                                                                                            <th>Document Number
                                                                                            </th>
                                                                                            <th>Valid From/صالح من تاريخ
                                                                                            </th>
                                                                                            <th>Valid Till/صالح ل
                                                                                            </th>
                                                                                            <th>Remark/تعليق
                                                                                            </th>
                                                                                            <th class="listTableAction">Action/عمل
                                                                                            </th>
                                                                                        </tr>
                                                                                    </thead>
                                                                                    <tbody>
                                                                                        <asp:Repeater ID="rpt_doc_list" runat="server" OnItemCommand="rpt_doc_list_OnItemCommand">
                                                                                            <ItemTemplate>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <%# Container.ItemIndex + 1 %>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lbl_docname" runat="server" Text='<%# Eval("Document_name")%>'></asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lbl_doc_type_name" runat="server" Text='<%# Eval("doc_type")%>'></asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lbl_docnum" runat="server" Text='<%# Eval("DocNumber")%>'></asp:Label><br />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lbl_from" runat="server" Text='<%#string.Format("{0:dd/MM/yyyy}", Eval("Valid_From"))%>'></asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lbl_to" runat="server" Text='<%#string.Format("{0:dd/MM/yyyy}", Eval("Valid_To"))%>'></asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lbl_remark" runat="server" Text='<%# Eval("Remark")%>'></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="listTableActionButtonDiv">
                                                                                                        <asp:HiddenField ID="hdn_indx" Value='<%#Eval("dt_indx")%>' runat="server" />
                                                                                                        <asp:HiddenField ID="hdnVyr" Value='<%#Eval("ValidityYear")%>' runat="server" />

                                                                                                        <asp:HiddenField ID="hdn_doc_Id" Value='<%#Eval("DocumentTypeId")%>' runat="server" />
                                                                                                        <asp:HiddenField ID="hdn_id" Value='<%#Eval("Id")%>' runat="server" />
                                                                                                        <asp:Label ID="lbl_doc_name" Visible="false" runat="server" Text='<%# Eval("Documentname")%>'></asp:Label>
                                                                                                        <asp:HiddenField ID="hdn_dnm" Value='<%#Eval("DocumentSave")%>' runat="server" />
                                                                                                        <asp:HiddenField ID="v_frm" runat="server" Value='<%#Eval("Valid_From")%>' />
                                                                                                        <asp:HiddenField ID="v_to" runat="server" Value='<%#Eval("Valid_To")%>' />
                                                                                                        <asp:Button ID="btn_doc_dwnld" ToolTip="Download" CssClass="btn_doc_down" runat="server"
                                                                                                            CommandName="Download" />
                                                                                                        <asp:Button ID="btn_edit" ToolTip="Edit" CssClass="btn_edit" runat="server" CommandName="Edit" />
                                                                                                        <asp:Button ID="btn_remove_line" class="btn_delete" runat="server" ToolTip="Delete Document"
                                                                                                            OnClick="btn_remove_line_OnClick" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </ItemTemplate>
                                                                                        </asp:Repeater>
                                                                                        <tr>
                                                                                            <td colspan="8" class="navigationRow">
                                                                                                <asp:UpdatePanel ID="Upd_Nav_Doc" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:Label ID="lbl_page_infoD" runat="server" class="pageInfo"></asp:Label>
                                                                                                        <asp:Button ID="btn_firstD" runat="server" class="navigationButton" Text="<<" OnClick="btn_first1_OnClick" />
                                                                                                        <asp:Button ID="btn_prevD" runat="server" class="navigationButton" Text="<" OnClick="btn_prev1_OnClick" />
                                                                                                        <asp:Label ID="lbl_page_numberD" Style="font-weight: bold; margin-left: 5px; margin-right: 5px; text-align: center;"
                                                                                                            runat="server"></asp:Label>
                                                                                                        <asp:Button ID="btn_nextD" class="navigationButton" runat="server" Text=">" OnClick="btn_next1_OnClick" />
                                                                                                        <asp:Button ID="btn_lastD" class="navigationButton" runat="server" Text=">>" OnClick="btn_last1_OnClick" />
                                                                                                        <asp:DropDownList ID="drp_countD" class="pageSize" runat="server" AutoPostBack="true"
                                                                                                            OnSelectedIndexChanged="drp_countD_OnSelectedIndexChanged">
                                                                                                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                                                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                                                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                                                                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                                                                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                                        <asp:HiddenField ID="hdn_filterD" runat="server" />
                                                                                                        <asp:HiddenField ID="hdn_last_pageD" runat="server" />
                                                                                                        <div class="head_second_divD" style="display: none">
                                                                                                            <asp:HiddenField ID="hdn_totalD" runat="server" Value="0" />
                                                                                                        </div>
                                                                                                    </ContentTemplate>
                                                                                                    <Triggers>
                                                                                                        <asp:PostBackTrigger ControlID="rpt_doc_list" />
                                                                                                    </Triggers>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Document Number <span style="color: Red">&nbsp*</span>
                                                                    <br />
                                                                    <asp:TextBox ID="txt_doc_no" CssClass="txt" runat="server"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="Requiredtxt_doc_no" runat="server" ControlToValidate="txt_doc_no"
                                                                        ValidationGroup="doc_add" ErrorMessage="Required" Display="Dynamic" Style="color: Red"
                                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Document Name/اسم المستندات
                                                                    <br />
                                                                    <asp:TextBox ID="txt_docname" CssClass="txt" runat="server"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Valid From/صالح من تاريخ
                                                                    <br />
                                                                    <telerik:RadDatePicker ID="valid_from" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                                        <Calendar runat="server" ID="Calendar2" CssClass="rtlSupport" ShowOtherMonthsDays="False"
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
                                                                <td>Validity years
                                                                    <asp:TextBox ID="txtValidityyr" AutoPostBack="true" OnTextChanged="txtValidityyr_TextChanged" runat="server" class="txt numbers_only"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:UpdatePanel ID="updVTo" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            Valid To/صالح ل
                                                                    <br />
                                                                            <telerik:RadDatePicker ID="valid_to" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                                                <Calendar runat="server" ID="Calendar4" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                                                    ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                                                    <SpecialDays>
                                                                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                                        </telerik:RadCalendarDay>
                                                                                    </SpecialDays>
                                                                                </Calendar>
                                                                            </telerik:RadDatePicker>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td>Remark/تعليق
                                                                    <br />
                                                                    <asp:TextBox ID="txt_docremark" CssClass="txtarea" TextMode="MultiLine" runat="server"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">Upload File/ملفات محملة
                                                                    <br />
                                                                    <telerik:RadAsyncUpload ID="fu_documents" Width="80%" MaxFileSize="500000000" runat="server"
                                                                        MaxFileInputsCount="1" OnFileUploaded="fu_documents_OnFileUploaded">
                                                                    </telerik:RadAsyncUpload>
                                                                    <asp:Label ID="lab_doc_name_out" runat="server" Text=""></asp:Label>
                                                                    <asp:HiddenField ID="hdn_doc_name" runat="server" />
                                                                    <asp:HiddenField ID="hdn_doc_sav" runat="server" />
                                                                    <asp:HiddenField ID="hdn_doc_index_Id" runat="server" Value="0" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="Button5" runat="server" class="butn_save" Text="Save/حفظ" OnClick="btn_DocSave_OnClick" />
                                                                    <asp:Button ID="btn_Dreset" class="butn" runat="server" Text="Reset/إعادة تعيين"
                                                                        OnClick="btn_reset_doc_OnClick" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td>

                                                                    <asp:Button ID="Button6" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_Docclose_OnClick" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>

                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="Upd_User_Cred_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnl_User_Cred" Visible="false" runat="server">
                            <div class="popupBackground">
                            </div>
                            <div class="animated largePopUp">
                                <div class="Adding_heading">
                                    User Credentials/اوراق اعتماد المستخدم
                                </div>
                                <table class="table_style" style="width: 100%; padding: 2px;" border="0">
                                    <thead>
                                        <tr>
                                            <td colspan="5">
                                                <b><u>Imigration Online/الهجرة على الانترنت </u></b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%">Company Name/اسم الشركة
                                            </td>
                                            <td style="width: 15%">User ID/اسم المستخدم
                                            </td>
                                            <td style="width: 15%">Password/كلمة المرور
                                            </td>
                                            <td style="width: 15%">Bank User/مستخدم البنك
                                            </td>
                                            <td style="width: 15%">Bank Password/كلمة مرور البنك
                                            </td>
                                            <td style="width: 15%">Bank Pin/رمز البنك
                                            </td>
                                            <td style="width: 15%">RSA PIN/رمز RSA
                                            </td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="Txt_imi_name" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="txt_u_id" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="Txt_im_pass" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="Txt_im_bu" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="txt_im_bp" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="Txt_im_bkpin" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="txt_im_rsa_pin" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table class="table_style" style="width: 100%; padding: 2px;" border="0">
                                    <thead>
                                        <tr>
                                            <td colspan="5">
                                                <b><u>Dubai Municipality/بلدية دبي </u></b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%">Company Name/اسم الشركة
                                            </td>
                                            <td style="width: 15%">DM User
                                            </td>
                                            <td style="width: 15%">DM Password
                                            </td>
                                            <td style="width: 15%">Admin User
                                            </td>
                                            <td style="width: 15%">Admin Password
                                            </td>
                                            <td style="width: 15%">Email User/مستخدم الايميل
                                            </td>
                                            <td style="width: 15%">Email Password/كلمة مرور الايميل
                                            </td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="Txt_mun_name" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="Txt_dm_user" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="Txt_dm_pass" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="Txt_Ad_user" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="Txt_ad_pass" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="Txt_em_user" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="Txt_em_pass" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table class="table_style" style="width: 100%; padding: 2px;" border="0">
                                    <thead>
                                        <tr>
                                            <td colspan="5">
                                                <b><u>Tasheel User/مستخدم تسهيل </u></b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%">Company Name/اسم الشركة
                                            </td>
                                            <td style="width: 15%">Super User
                                            </td>
                                            <td style="width: 15%">Password/كلمة مرور
                                            </td>
                                            <td style="width: 15%">Thasheel User/مستخدم تسهيل
                                            </td>
                                            <td style="width: 15%">Thasheel Pass/رمز تسهيل
                                            </td>
                                            <td style="width: 15%">Email ID/الايميل
                                            </td>
                                            <td style="width: 15%">Mobile/هاتف
                                            </td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="Txt_thu_name" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="Txt_sup_user" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="Txt_thu_pass" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="Txt_thu_usr" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="Txt_thu_passs" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="txt_thu_mail" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="txt_thu_mob" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table class="table_style" style="width: 100%; padding: 2px;" border="0">
                                    <thead>
                                        <tr>
                                            <td colspan="4">
                                                <b><u>Netwals Service/خدمات الشبكة </u></b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%">User/اسم المستخدم
                                            </td>
                                            <td style="width: 15%">Password/كلمة المرور
                                            </td>
                                            <td style="width: 15%">E-Mail/البريد الالكتروني
                                            </td>
                                            <td style="width: 15%">Mobile/هاتف
                                            </td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="Txt_net_user" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="Txt_net_pass" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="Txt_net_mail" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox class="txt" ID="Txt_net_mob" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <br />
                                <asp:Button ID="btn1" runat="server" Text="Save/حفظ" class="butn_save" OnClick="btn_credSave_click" />
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="Upd_Service_Detail_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnl_Service_Detail" Visible="false" runat="server">
                            <div class="popupBackground">
                            </div>
                            <div class="animated largePopUp">
                                <div class="Adding_heading">
                                    Service Detail/بيانات الخدمة
                                </div>
                                <br />
                                <div class="searchDiv" style="width: 65%">

                                    <div style="float: right">
                                        <asp:TextBox ID="txtCommonDiscount" runat="server" class="txt_search numbers_only CommonDiscount"
                                            placeholder="Apply this discount for all"></asp:TextBox>
                                    </div>
                                    <div style="float: right">
                                        <asp:TextBox ID="txtsearchservice" runat="server" class="txt_search " placeholder="Search Service"
                                            AutoPostBack="true" OnTextChanged="txtsearchservice_TextChanged"></asp:TextBox>
                                    </div>
                                </div>
                                <br />
                                <div style="overflow: auto; max-height: 75%; clear: both">
                                    <asp:UpdatePanel ID="updservicelist" runat="server" ChildrenAsTriggers="false"
                                        UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table class="listTable">
                                                <thead>
                                                    <tr>
                                                        <th style="width: 5%">Sl No/رقم
                                                        </th>
                                                        <th style="width: 25%">Service/الخدمات
                                                        </th>
                                                        <th style="width: 10%">Amount/المبلغ
                                                        </th>
                                                        <th style="width: 10%">Discount/الخصم
                                                        </th>
                                                        <th style="width: 10%">Addition
                                                        </th>
                                                        <th style="width: 12%">After Discount/Addition
                                                        </th>
                                                        <th style="width: 15%">Commission Amount/المبلغ العمولة
                                                        </th>

                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rpt_serdetail" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <%# Container.ItemIndex + 1 %>
                                                                    <asp:HiddenField ID="hdn_cusSerDetailId" runat="server" Value='<%#Eval("CusSerDetailId") %>' />
                                                                </td>
                                                                <td>
                                                                    <asp:HiddenField ID="hdn_serviceId" runat="server" Value='<%#Eval("ServiceId") %>' />
                                                                    <asp:Label ID="lbl_name" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_amt" Class="txt readOnly amt" runat="server" Text='<%#Eval("Price") %>'></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_disAmt" Class="txt numbers_only disAmt" runat="server" Text='<%#Eval("DiscountAmount") %>'></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txt_disAmt"
                                                                        ValidationGroup="save_serdetail" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                        InitialValue="">
                                                                    </asp:RequiredFieldValidator>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_addAmt" Class="txt numbers_only addAmt" runat="server" Text='<%#Eval("Addition") %>'></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_afterDis" Class="txt readOnly afterDis" runat="server" Text='<%#Eval("FinalPrice") %>'></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCommissionAmount" Class="txt numbers_only" runat="server" Text='<%#Eval("CommissionAmount") %>'></asp:TextBox>
                                                                </td>

                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:Button ID="Button1" runat="server" class="butn_save" ValidationGroup="save_serdetail"
                                                    Text="Save/حفظ" OnClick="btn_SDSave_OnClick" />
                                                <asp:Button ID="Button3" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_sd_OnClick" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdServiceExpiryPanel" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlServiceExpiry" Visible="false" runat="server">
                            <div class="popupBackground">
                            </div>
                            <div class="animated largePopUp">
                                <div class="Adding_heading">
                                    Service Expiry/انتهاء الخدمة
                                </div>
                                <div>
                                    <asp:UpdatePanel ID="Upd_List_Panel_SE" runat="server" ChildrenAsTriggers="false"
                                        UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table class="listTable">
                                                <thead>
                                                    <tr>
                                                        <th class="listTableSlNo">Sl No/رقم
                                                        </th>
                                                        <th style="width: 200px;">Service/الخدمات
                                                        </th>
                                                        <th>Invoice Code/رمز الفاتورة
                                                        </th>
                                                        <th>Quantity/الكمية
                                                        </th>
                                                        <th>Start Date
                                                        </th>
                                                        <th>Expiry Date
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptServiceExpiry" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <%# Eval("RowNum")%>
                                                                </td>
                                                                <td>
                                                                    <%# Eval("Service")%>
                                                                </td>
                                                                <td>
                                                                    <%# Eval("InvoiceCode")%>
                                                                </td>
                                                                <td>
                                                                    <%# Eval("Quantity")%>
                                                                </td>
                                                                <td>
                                                                    <%# Eval("StartDate")%>
                                                                </td>
                                                                <td>
                                                                    <%# Eval("ExpiryDate")%>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                    <tr>
                                                        <td colspan="6" class="navigationRow">
                                                            <asp:UpdatePanel ID="Upd_Nav_Panel_SE" runat="server" ChildrenAsTriggers="false"
                                                                UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Label ID="lbl_page_info_SE" runat="server" class="pageInfo"></asp:Label>
                                                                    <asp:Button ID="btn_first_SE" runat="server" class="navigationButton" Text="<<" OnClick="btn_first_SE_OnClick" />
                                                                    <asp:Button ID="btn_prev_SE" runat="server" class="navigationButton" Text="<" OnClick="btn_prev_SE_OnClick" />
                                                                    <asp:Label ID="lbl_page_number_SE" Style="font-weight: bold; margin-left: 5px; margin-right: 5px; text-align: center;"
                                                                        runat="server"></asp:Label>
                                                                    <asp:Button ID="btn_next_SE" class="navigationButton" runat="server" Text=">" OnClick="btn_next_SE_OnClick" />
                                                                    <asp:Button ID="btn_last_SE" class="navigationButton" runat="server" Text=">>" OnClick="btn_last_SE_OnClick" />
                                                                    <asp:DropDownList ID="drp_count_SE" class="pageSize" runat="server" AutoPostBack="true"
                                                                        OnSelectedIndexChanged="drp_count_SE_OnSelectedIndexChanged">
                                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:HiddenField ID="hdn_filter_SE" runat="server" />
                                                                    <asp:HiddenField ID="hdn_last_page_SE" runat="server" />
                                                                    <div class="head_second_div" style="display: none">
                                                                        <asp:HiddenField ID="hdn_total_SE" runat="server" Value="0" />
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:Button ID="btnCloseSE" class="butn" runat="server" Text="Close/أغلق" OnClick="btnCloseSE_OnClick" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="Upd_Document_Panel_Staff" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnl_document_Staff" Visible="false" runat="server">
                            <div class="popupBackground">
                            </div>
                            <div class="animated largePopUp">
                                <div class="Adding_heading">
                                    Document/وثيقة
                                </div>
                                <div style="margin-top: 10px;">
                                    <span style="margin-left: 22px; font-weight: bold; font-size: 1.2em;">Customer:</span>
                                    <asp:Label ID="lbl_StaffCusName" runat="server" Style="font-weight: bold; font-size: 1.2em;"></asp:Label>
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="updStaffFile" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <telerik:RadAsyncUpload ID="fu_DocUpload" MaxFileSize="500000000" runat="server"
                                                        MaxFileInputsCount="1" OnFileUploaded="fu_DocUpload_OnFileUploaded">
                                                    </telerik:RadAsyncUpload>
                                                    <asp:Button ID="btnStaffDocUpload" class="butn" runat="server" OnClick="btnStaffDocUpload_Click" Text="Upload File" />
                                                    <asp:Button ID="btnstaffdocformatDwn" class="butn" runat="server" OnClick="btnstaffdocformatDwn_Click" Text="Download File Format" />
                                                    <asp:HiddenField ID="hdnStaffFile" runat="server" />
                                                    <asp:HiddenField ID="hdnStafffileExtension" runat="server" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnstaffdocformatDwn" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="Upd_docadd_Staff" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div id="div_document_new_Staff" runat="server">
                                                        <table class="formTable">
                                                            <tr>
                                                                <td style="width: 20%">Staff/العاملين <span style="color: Red">&nbsp*</span>
                                                                    <br />
                                                                    <asp:TextBox ID="txt_staff" CssClass="txt" runat="server"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_staff"
                                                                        ValidationGroup="doc_add_Staff" ErrorMessage="Required" Display="Dynamic" Style="color: Red"
                                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td style="width: 75%; border-left: 1px solid gray" rowspan="10">
                                                                    <div class="HeadIng_Div">
                                                                        Document List/قائمة الخصم
                                                                    <div class="searchDiv">
                                                                        <asp:TextBox ID="txt_search_doc_Staff" runat="server" AutoPostBack="true" OnTextChanged="txt_doc_search_OnTextChanged_Staff"
                                                                            class="txt_search" placeholder="Search" Style="float: right; width: 61%"></asp:TextBox>
                                                                    </div>
                                                                    </div>
                                                                    <div>
                                                                        <asp:UpdatePanel ID="Upd_doc_Staff" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <table class="listTable">
                                                                                    <thead>
                                                                                        <tr>
                                                                                            <th style="width: 3%;">Sl/رقم
                                                                                            </th>
                                                                                            <th style="width: 10%;">Staff/العاملين
                                                                                            </th>
                                                                                            <th style="width: 10%;">Contact No/هاتف
                                                                                            </th>
                                                                                            <th style="width: 10%;">Document Name/اسم المستندات
                                                                                            </th>
                                                                                            <th style="width: 10%;">Document Type
                                                                                            </th>
                                                                                            <th style="width: 10%;">Document Number
                                                                                            </th>
                                                                                            <th style="width: 10%;">Valid From/صالح من تاريخ
                                                                                            </th>
                                                                                            <th style="width: 10%;">Valid Till/صالح ل
                                                                                            </th>
                                                                                            <th style="width: 5%;">Action/عمل
                                                                                            </th>
                                                                                        </tr>
                                                                                    </thead>
                                                                                    <tbody>
                                                                                        <asp:Repeater ID="rpt_doc_list_Staff" runat="server" OnItemCommand="rpt_doc_list_OnItemCommand_Staff">
                                                                                            <ItemTemplate>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <%-- <%# Container.ItemIndex + 1 %>--%>
                                                                                                        <%# Eval("dt_indx")%>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lbl_staffname" runat="server" Text='<%# Eval("StaffName")%>'></asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lbl_staffNo" runat="server" Text='<%# Eval("StaffMobile")%>'></asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lbl_docname" runat="server" Text='<%# Eval("Document_name")%>'></asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lbl_doc_type_name" runat="server" Text='<%# Eval("doc_type")%>'></asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lbl_docnum" runat="server" Text='<%# Eval("DocNumber")%>'></asp:Label><br />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lbl_from" runat="server" Text='<%#string.Format("{0:dd/MM/yyyy}", Eval("Valid_From"))%>'></asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lbl_to" runat="server" Text='<%#string.Format("{0:dd/MM/yyyy}", Eval("Valid_To"))%>'></asp:Label>
                                                                                                        <asp:Label ID="lbl_remark" Visible="false" runat="server" Text='<%# Eval("Remark")%>'></asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:HiddenField ID="hdnVyr" Value='<%#Eval("ValidityYear")%>' runat="server" />
                                                                                                        <asp:HiddenField ID="hdn_doc_Id" Value='<%#Eval("DocumentTypeId")%>' runat="server" />
                                                                                                        <asp:HiddenField ID="hdn_id" Value='<%#Eval("Id")%>' runat="server" />
                                                                                                        <asp:Label ID="lbl_doc_name" Visible="false" runat="server" Text='<%# Eval("DocumentName")%>'></asp:Label>
                                                                                                        <asp:HiddenField ID="hdn_dnm" Value='<%#Eval("DocumentSave")%>' runat="server" />
                                                                                                        <asp:HiddenField ID="v_frm" runat="server" Value='<%#Eval("Valid_From")%>' />
                                                                                                        <asp:HiddenField ID="v_to" runat="server" Value='<%#Eval("Valid_To")%>' />
                                                                                                        <asp:Button ID="btn_doc_dwnld" ToolTip="Download" CssClass="btn_doc_down" runat="server"
                                                                                                            CommandName="Download" />
                                                                                                        <asp:Button ID="btn_edit" ToolTip="Edit" CssClass="btn_edit" runat="server" CommandName="Edit" />
                                                                                                        <asp:Button ID="btn_remove_line" class="btn_delete" runat="server" ToolTip="Delete Document"
                                                                                                            OnClick="btn_remove_line_OnClick_Staff" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </ItemTemplate>
                                                                                        </asp:Repeater>
                                                                                        <tr>
                                                                                            <td colspan="9" class="navigationRow">
                                                                                                <asp:UpdatePanel ID="Upd_Nav_Doc_Staff" runat="server" ChildrenAsTriggers="false"
                                                                                                    UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:Label ID="lbl_page_infoD_Staff" runat="server" class="pageInfo"></asp:Label>
                                                                                                        <asp:Button ID="btn_firstD_Staff" runat="server" class="navigationButton" Text="<<"
                                                                                                            OnClick="btn_first1_OnClick_Staff" />
                                                                                                        <asp:Button ID="btn_prevD_Staff" runat="server" class="navigationButton" Text="<"
                                                                                                            OnClick="btn_prev1_OnClick_Staff" />
                                                                                                        <asp:Label ID="lbl_page_numberD_Staff" Style="font-weight: bold; margin-left: 5px; margin-right: 5px; text-align: center;"
                                                                                                            runat="server"></asp:Label>
                                                                                                        <asp:Button ID="btn_nextD_Staff" class="navigationButton" runat="server" Text=">"
                                                                                                            OnClick="btn_next1_OnClick_Staff" />
                                                                                                        <asp:Button ID="btn_lastD_Staff" class="navigationButton" runat="server" Text=">>"
                                                                                                            OnClick="btn_last1_OnClick_Staff" />
                                                                                                        <asp:DropDownList ID="drp_countD_Staff" class="pageSize" runat="server" AutoPostBack="true"
                                                                                                            OnSelectedIndexChanged="drp_countD_OnSelectedIndexChanged_Staff">
                                                                                                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                                                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                                                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                                                                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                                                                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                                        <asp:HiddenField ID="hdn_filterD_Staff" runat="server" />
                                                                                                        <asp:HiddenField ID="hdn_last_pageD_Staff" runat="server" />
                                                                                                        <div class="head_second_divD_Staff" style="display: none">
                                                                                                            <asp:HiddenField ID="hdn_totalD_Staff" runat="server" Value="0" />
                                                                                                        </div>
                                                                                                    </ContentTemplate>
                                                                                                    <Triggers>
                                                                                                        <asp:PostBackTrigger ControlID="rpt_doc_list_Staff" />
                                                                                                    </Triggers>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 24%">Mobile/هاتف <span style="color: Red">&nbsp*</span>
                                                                    <br />
                                                                    <asp:TextBox ID="txt_staffmob" CssClass="txt" runat="server"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="Requiredtxt_staffmob5" runat="server" ControlToValidate="txt_staffmob"
                                                                        ValidationGroup="doc_add_Staff" ErrorMessage="Required" Display="Dynamic" Style="color: Red"
                                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 24%">Document Type<span style="color: Red">&nbsp*</span>
                                                                    <telerik:RadComboBox ID="drp_doc_Staff" Sort="Ascending" Filter="Contains" runat="server"
                                                                        AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search Document..."
                                                                        Style="overflow: hidden; width: 96%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                        OnClientBlur="ValidateCombo">
                                                                    </telerik:RadComboBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6_Staff" runat="server" ControlToValidate="drp_doc_Staff"
                                                                        ValidationGroup="doc_add_Staff" ErrorMessage="Required" Display="Dynamic" Style="color: Red"
                                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 24%">Document Number
                                                                    <br />
                                                                    <asp:TextBox ID="txt_doc_no_Staff" CssClass="txt" runat="server"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Valid From/صالح من تاريخ
                                                                    <br />
                                                                    <telerik:RadDatePicker ID="valid_from_Staff" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                                        <Calendar runat="server" ID="Calendar2_Staff" CssClass="rtlSupport" ShowOtherMonthsDays="False"
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
                                                                <td>Validity years
                                                                    <asp:TextBox ID="txtvalidtiyStaff" AutoPostBack="true" OnTextChanged="txtvalidtiyStaffTextChanged" runat="server" class="txt numbers_only"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:UpdatePanel ID="updVToStaff" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                Valid To/صالح ل
                                                                    <br />
                                                                                <telerik:RadDatePicker ID="valid_to_Staff" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                                                    <Calendar runat="server" ID="Calendar4_Staff" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                                                        ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                                                        <SpecialDays>
                                                                                            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                                            </telerik:RadCalendarDay>
                                                                                        </SpecialDays>
                                                                                    </Calendar>
                                                                                </telerik:RadDatePicker>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Document name/اسم المستندات
                                                                    <br />
                                                                        <asp:TextBox ID="txt_docname_Staff" CssClass="txt" runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Remark/تعليق
                                                                    <br />
                                                                        <asp:TextBox ID="txt_docremark_Staff" CssClass="txtarea" TextMode="MultiLine" runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Upload File/ملفات محملة
                                                                    <br />
                                                                        <telerik:RadAsyncUpload ID="fu_documents_Staff" Width="80%" MaxFileSize="500000000"
                                                                            runat="server" MaxFileInputsCount="1" OnFileUploaded="fu_documents_OnFileUploaded_Staff">
                                                                        </telerik:RadAsyncUpload>
                                                                        <asp:Label ID="lab_doc_name_out_Staff" runat="server" Text=""></asp:Label>
                                                                        <asp:HiddenField ID="hdn_doc_name_Staff" runat="server" />
                                                                        <asp:HiddenField ID="hdn_doc_sav_Staff" runat="server" />
                                                                        <asp:HiddenField ID="hdn_doc_index_Id_Staff" runat="server" Value="0" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="Button5_Staff" runat="server" class="butn_save" Text="Save/حفظ" OnClick="btn_DocSave_OnClick_Staff" />
                                                                        <asp:Button ID="btn_Dreset_Staff" class="butn" runat="server" Text="Reset/إعادة تعيين"
                                                                            OnClick="btn_reset_doc_OnClick_Staff" />
                                                                    </td>
                                                                    <td>

                                                                        <asp:Button ID="Button6_Staff" class="butn" runat="server" Text="Close/قريب" OnClick="btn_Docclose_OnClick_Staff" />
                                                                    </td>
                                                                </tr>
                                                        </table>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>

                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="Menudisplay" runat="server" Title="Menu Details" Height="420px"
                            Width="610px" Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false"
                            Modal="true" VisibleStatusbar="false" />
                    </Windows>
                </telerik:RadWindowManager>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
</asp:Content>
