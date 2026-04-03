<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true"
    CodeBehind="Employee.aspx.cs" Inherits="AmarCentre.Masters.Employee" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function ShowMenuForm(id) {
            window.radopen("DisplayMenu.aspx?userid=" + id, "Menudisplay");
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

            $('.CommonAmt').blur(function (e) {
                $('.txt_Incamt').val($('.CommonAmt').val());
            });

            $('.CommonPer').blur(function (e) {
                $('.txt_IncPer').val($('.CommonPer').val());
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Employee/موظف

        <asp:Button ID="btn_addnew" runat="server" class="btnAddNew" OnClick="btn_newentry_OnClick" />
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
                            <th style="width: 5%;">
                                Sl No/رقم
                            </th>
                            <th style="width: 12%;">
                                Name/اسم
                            </th>
                            <th style="width: 8%;">
                                Mobile/هاتف

                            </th>
                            <th style="width: 10%;">
                                Designation/تعيين

                            </th>
                            <th style="width: 6%;">
                                Action/عمل
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
                                        <%#Eval("MobileNum")%>
                                    </td>
                                    <td>
                                        <%#Eval("DesignationName")%>
                                    </td>
                                    <td class="listTableActionButtonDiv">
                                        <asp:Button ID="btn_edit" runat="server" class="btn_edit" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td colspan="5" class="navigationRow">
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
      
    </div>
    <div>
        <asp:UpdatePanel ID="Upd_Add_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnl_add" Visible="false" runat="server">
                    <div class="popupBackground">
                    </div>
                    <div class="animated halfPopUp" >
                        <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="Adding_heading">
                                    Employee/موظف
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td style="width:48%">
                                            Code/رمز <span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txtCode" CssClass="txt" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtCode"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        <td rowspan="5" style="width:48%">
                                             <asp:UpdatePanel ID="UpdProfilePhoto" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="profile_img_div" style="width: 151px">
                                                        <asp:Image ID="img_profile" runat="server" Style="border: 1px solid orange; height: 100%;
                                                            width: 100%;" ImageUrl="~/Images/defaultimage.png" />
                                                    </div>
                                                    <telerik:RadAsyncUpload ID="fuProfilePhoto" MaxFileSize="500000000" runat="server"
                                                        MaxFileInputsCount="1" OnFileUploaded="fuProfilePhoto_OnFileUploaded" Width="100%">
                                                    </telerik:RadAsyncUpload>
                                                    <asp:HiddenField ID="hdn_photo" runat="server" />
                                                    <asp:HiddenField ID="hdn_photo_save" runat="server" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Name/اسم <span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txt_name" CssClass="txt" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txt_name"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Designation/تعيين<span style="color: Red">&nbsp*</span>
                                            <telerik:RadComboBox ID="drp_Design" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search Designation..."
                                                OnClientFocus="OnClientKeyPressing" OnClientBlur="ValidateCombo" Style="overflow: hidden;
                                                width: 96%; border: none!important;">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="drp_Design"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Reporting Person/الشخص المبلغ
                                            <br />
                                            <telerik:RadComboBox ID="drp_Reporting" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search Reporting..."
                                                OnClientFocus="OnClientKeyPressing" OnClientBlur="ValidateCombo" Style="overflow: hidden;
                                                width: 96%; border: none!important;">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Address/العنوان 
                                            <br />
                                            <asp:TextBox ID="txt_present_add" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Email/البريد الالكتروني 
                                            <asp:TextBox ID="txt_email" CssClass="txt" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Please Enter Valid Email ID"
                                                ValidationGroup="save" ControlToValidate="txt_email" Style="color: Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                        </td>
                                        <td>
                                            Mobile /هاتف <span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txt_mobile" class="txt numbers_only" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_mobile"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Phone Number/رقم الهاتف 
                                            <br />
                                            <asp:TextBox ID="txt_phn" class="txt numbers_only" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            Petty Cash Account/حساب المصروفات النثرية
                                            <br />
                                            <telerik:RadComboBox ID="drp_pettyCash" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search Petty Cash Account"
                                                EnableCheckAllItemsCheckBox="true" CheckBoxes="true" Style="overflow: hidden;
                                                width: 96%; border: none!important;">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Bank Account/بنك
                                            <br />
                                            <telerik:RadComboBox ID="drpBankAccount" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search Bank Account"
                                                EnableCheckAllItemsCheckBox="true" CheckBoxes="true" Style="overflow: hidden;
                                                width: 96%; border: none!important;">
                                            </telerik:RadComboBox>
                                        </td>
                                          <td>
      Loan Account
      <br />
      <telerik:RadComboBox ID="drpLoanAccount" Sort="Ascending" Filter="Contains" runat="server"
          AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search Loan Account"
           Style="overflow: hidden;
          width: 96%; border: none!important;" EnableCheckAllItemsCheckBox="true" CheckBoxes="true">
      </telerik:RadComboBox>
  </td>
                                      
                                    </tr>
                                    <tr>
                                                                                <td>
   Department
    <br />
    <telerik:RadComboBox ID="drpDepartment" Sort="Ascending" Filter="Contains" runat="server"
        AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search Department"
         Style="overflow: hidden;
        width: 96%; border: none!important;" EnableCheckAllItemsCheckBox="true" CheckBoxes="true">
    </telerik:RadComboBox>
</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            User Name/اسم المستخدم  <span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txt_userName" class="txt " runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_userName"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            Password /كلمة مرور <span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txt_password" class="txt" autocomplete="new-password" TextMode="Password" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txt_password"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                             <asp:CheckBox ID="chkenable" runat="server" Text="Is Enable" />
                                             <asp:CheckBox ID="chk_IsIncApp" runat="server" Text="Incentive Applicable" OnCheckedChanged="chk_IsIncApp_OnCheckedChanged"
     AutoPostBack="true" />
                                        </td>
                                      
                                    </tr>
                                    <tr style="display:none">
                                        
                                         <td>
                                              <telerik:RadComboBox ID="drp_accQrec" Sort="Ascending" Filter="Contains" runat="server" visible="false"
                                                AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search Default Account"
                                                Style="overflow: hidden; width: 96%; border: none!important;">
                                            </telerik:RadComboBox>
                                           
                                        </td>
                                     <td>
                                           
                                        </td>
                                    </tr>
                                   <%-- <tr>
                                         <td>
                                            <asp:UpdatePanel ID="Upd_IncApp_Panel1" runat="server" ChildrenAsTriggers="false"
                                                UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnl_IncApp1" Visible="false" runat="server">
                                                        Target Count/العدد المستهدف  <span style="color: Red">&nbsp*</span>
                                                        <asp:TextBox ID="txt_targetcount" class="txt numbers_only" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_targetcount"
                                                            ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                            InitialValue=""></asp:RequiredFieldValidator>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td> 
                                        <td>
                                            <asp:UpdatePanel ID="Upd_IncApp_Panel2" runat="server" ChildrenAsTriggers="false"
                                                UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnl_IncApp2" Visible="false" runat="server">
                                                        Incentive Amount/المبلغ الحافز <span style="color: Red">&nbsp*</span>
                                                        <asp:TextBox ID="txt_incentAmt" class="txt numbers_only" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txt_incentAmt"
                                                            ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                            InitialValue=""></asp:RequiredFieldValidator>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td>
                                            Balance/توازن
                                            <br />
                                            <asp:Label ID="lbl_bal" runat="server"></asp:Label>
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
                                                <asp:Button ID="btn_OB" class="butn" runat="server" Visible="false" Text="Opening Balance/الرصيد المفتوح "
                                                    OnClick="btn_OB_OnClick" />
                                                <asp:Button ID="btn_menu" class="butn_save" OnClick="btn_menu_OnClick" runat="server"
                                                    Text="Menu Privilege/امتياز القائمة " />
                                                <asp:UpdatePanel ID="Upd_IncApp_Panel1" runat="server" ChildrenAsTriggers="false"
                                                    UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnIncentive" class="butn_save" OnClick="btnIncentiveamountOnClick" runat="server"
                                                            Text=" Incentive Amount/المبلغ الحافز  " />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <asp:Button ID="btn_doc" class="butn_save" runat="server" Visible="false" Text="Add Document/اضافة مستندات "
                                                    OnClick="btn_docadd_OnClick" />
                                                <asp:Button ID="btn_other" class="butn" runat="server" Visible="false" Text="Other Details/معلومات اخري "
                                                    OnClick="btn_other_OnClick" />
                                                <asp:Button ID="Button2" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_OnClick" />
                                                <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_update" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_delete" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_menu" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_OB" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_doc" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_other" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnIncentive" runat="server" Value="0" />

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
                            &#10004</div>
                        <div>
                            <asp:Label ID="lbl_msg" runat="server" class="messageLabel"></asp:Label>
                        </div>
                    </div>
                </div>
                 <div>
                    <div id="div1" class="messageAlerterror div_poperror animated" style="display: none" runat="server">
                        <div class="tick">
                            &#10007</div>
                        <div>
                            <asp:Label ID="lblerrormsg" runat="server" class="messageLabel"></asp:Label>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel ID="Upd_otherD" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlOther" Visible="false" runat="server">
                            <div class="popupBackground">
                            </div>
                            <div class="animated smallPopUp">
                                <div class="Adding_heading">
                                    Other Details/معلومات اخري 
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td>
                                            Date of Join/تاريخ الانضمام
                                            <telerik:RadDatePicker ID="DOJ" runat="server" DateInput-DateFormat="dd/MM/yyyy">
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
                                            Date of Birth/تاريخ الولادة
                                            <telerik:RadDatePicker ID="DOB" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                <Calendar runat="server" ID="Calendar5" CssClass="rtlSupport" ShowOtherMonthsDays="False"
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
                                            MOL
                                            <asp:TextBox ID="txt_mol" runat="server" CssClass="txt"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Profession/مهنة
                                            <telerik:RadComboBox ID="drpPrefssn" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                EmptyMessage="Search Profession..." Style="overflow: hidden; width: 96%; border: none!important;">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Nationality/جنسية
                                            <telerik:RadComboBox ID="drp_nation" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                EmptyMessage="Search Nationality..." Style="overflow: hidden; width: 96%; border: none!important;">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Probation Status/حالة الاختبار
                                            <telerik:RadComboBox ID="drp_pro_status" TabIndex="9" Sort="Ascending" Filter="Contains"
                                                runat="server" AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search Probation Status"
                                              OnClientBlur="ValidateCombo"  Style="height: 24px !important; overflow: hidden; width: 96%; border: none!important;">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="1" Text="Probation" />
                                                    <telerik:RadComboBoxItem Value="2" Text="Permanent" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Contract Type/نوع العقد
                                            <telerik:RadComboBox ID="drp_cont" Sort="Ascending" TabIndex="10" Filter="Contains"
                                               OnClientBlur="ValidateCombo" runat="server" AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search Contract Type"
                                                Style="height: 24px !important; overflow: hidden; width: 96%; border: none!important;">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="1" Text="Limited" />
                                                    <telerik:RadComboBoxItem Value="2" Text="UnLimited" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Gender/جنس
                                            <telerik:RadComboBox ID="drp_gender" Sort="Ascending" Filter="Contains" TabIndex="12"
                                               OnClientBlur="ValidateCombo" runat="server" AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search Gender"
                                                Style="height: 24px !important; overflow: hidden; width: 96%; border: none!important;">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="1" Text="Female" />
                                                    <telerik:RadComboBoxItem Value="2" Text="Male" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:Button ID="Button1" runat="server" class="butn_save" Text="Save/حفظ" OnClick="btn_otherSave_OnClick" />
                                                <asp:Button ID="Button3" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_other_OnClick" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdApplicableLeave" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlApplicableLeave" Visible="false" runat="server">
                            <div class="popupBackground">
                            </div>
                            <div class="animated smallPopUp">
                                <div class="Adding_heading">
                                    Applicable Leave/طلب الاجازة 
                                </div>
                                <table class="listTable">
                                            <thead>
                                                <tr style="text-align: center">
                                                    <td style="width: 3%">
                                                        Sl.No/رقم
                                                    </td>
                                                    <td style="width: 13%">
                                                        Leave/إجازة
                                                    </td>
                                                    <td style="width: 7%">
                                                        Applicable Days/عدد ايام طلب 
                                                    </td>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="rpt_ApplicableLeave" runat="server">
                                                    <ItemTemplate>
                                                        <tr style="text-align: center">
                                                            <td>
                                                                <%# Container.ItemIndex + 1 %>
                                                            </td>
                                                            <td style="text-align: left">
                                                                <asp:HiddenField ID="hdn_EALId" runat="server" Value='<%#Eval("EALId") %>' />
                                                                <asp:HiddenField ID="hdn_LeaveId" runat="server" Value='<%#Eval("LeaveId") %>' />
                                                                <asp:Label ID="lblLeave" runat="server" Text='<%#Eval("Name") %>'></asp:Label>
                                                            </td>
                                                            <td style="text-align: left">
                                                                <asp:TextBox ID="txtApplicableLeave" class="numbers_only txt" runat="server"
                                                                    Text='<%#Eval("ApplicableLeave") %>'></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtApplicableLeave"
                                                            ValidationGroup="saveApplicableLeave" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                            InitialValue="">
                                                        </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>
                                <table class="formTable">
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:Button ID="btnApplicableLeaveSave" runat="server" class="butn_save" Text="Save/حفظ" ValidationGroup="ApplicableLeave" OnClick="btnApplicableLeaveSave_OnClick" />
                                                <asp:Button ID="btnApplicableLeaveClose" class="butn" runat="server" Text="Close/أغلق" OnClick="btnApplicableLeaveClose_OnClick" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
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
                                <table class="formTable">
                                    <tr>
                                        <td>
                                            Balance Type/نوع الرصيد  <span style="color: Red">&nbsp*</span>
                                            <telerik:RadComboBox ID="drp_obType" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                OnClientBlur="ValidateCombo" EmptyMessage="Search Balance Type..." Style="overflow: hidden;
                                                width: 96%; border: none!important;">
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
                                        <td>
                                            Balance/توازن <span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txt_open_bal" runat="server" class="txt numbers_only"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txt_open_bal"
                                                ValidationGroup="Ob_add" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Date/تاريخ <span style="color: Red">&nbsp*</span>
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
                                                <asp:Button ID="btn_OBSave" runat="server" class="butn_save" ValidationGroup="Ob_add"
                                                    Text="Save/حفظ" OnClick="btn_OBSave_OnClick" />
                                                <asp:Button ID="btn_close" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_ob_OnClick" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
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
                                <table class="formTable">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="Upd_docadd" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div id="div_document_new" runat="server">
                                                        <table class="formTable">
                                                            <tr>
                                                                <td style="width: 25%">Document Type<span style="color: Red">&nbsp*</span>
                                                                    <telerik:RadComboBox ID="drp_doc" Sort="Ascending" Filter="Contains" runat="server"
                                                                        AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search Document..."
                                                                        Style="overflow: hidden; width: 96%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                        OnClientBlur="ValidateCombo">
                                                                    </telerik:RadComboBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="drp_doc"
                                                                        ValidationGroup="doc_add" ErrorMessage="Required" Display="Dynamic" Style="color: Red"
                                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td style="width: 70%; border-left: 1px solid gray" rowspan="7">
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
                                                                                            <th class="listTableSlNo">Sl/رقم
                                                                                            </th>
                                                                                            <th>Document Type
                                                                                            </th>
                                                                                            <th>Document Number
                                                                                            </th>
                                                                                            <th>Valid From/صالح من تاريخ 
                                                                                            </th>
                                                                                            <th>Valid Till/صالح ل 
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
                                                                                            <td colspan="6" class="navigationRow">
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
                                                                <td>Document Number
                                                                    <br />
                                                                    <asp:TextBox ID="txt_doc_no" CssClass="txt" runat="server"></asp:TextBox>
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
                                                                <td>Upload File/ملفات محملة 
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
                                                                    <asp:Button ID="btn_add" runat="server" ValidationGroup="doc_add" class="butn_save"
                                                                        Text="Add" OnClick="btn_add_doc_OnClick" />
                                                                    <asp:Button ID="btn_Dreset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_doc_OnClick" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td>
                                                                    <asp:Button ID="Button5" runat="server" class="butn_save" Text="Save/حفظ" OnClick="btn_DocSave_OnClick" />
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
                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="Menudisplay" runat="server" Title="Menu Details" Height="420px"
                            Width="610px" Left="150px" ReloadOnShow="true" ShowContentDuringLoad="false"
                            Modal="true" VisibleStatusbar="false" />
                    </Windows>
                </telerik:RadWindowManager>
                
                <asp:UpdatePanel ID="Upd_Service_Detail_Panel" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnl_Service_Detail" Visible="false" runat="server">
                            <div class="popupBackground">
                            </div>
                            <asp:HiddenField ID="hdnIncPerc" runat="server" />
                            <div class="animated largePopUp">
                                <div class="Adding_heading">
                                    Service Detail/بيانات الخدمة
                                </div>
                                <br />
                                <div >
                                    <asp:TextBox ID="txtCommonAmount" runat="server" class="txt_search numbers_only CommonAmt" style=" width:35%;margin-left: 10px;"
                                        placeholder="Apply this Amount for all as Incentive Amount"></asp:TextBox>
                                         <asp:TextBox ID="txtCommonPer" runat="server" class="txt_search numbers_only CommonPer" style=" width:35%"
                                        placeholder="Apply this Amount for all as Incentive Percentage"></asp:TextBox>
                                </div>
                                <br />
                                <div style="overflow: auto; max-height: 75%; clear: both">
                                    <table class="listTable">
                                        <thead>
                                            <tr>
                                                <th class="listTableSlNo">
                                                    Sl No/رقم
                                                </th>
                                                <th>
                                                    Service/الخدمات
                                                </th>
                                                <th id="th_incamt" runat="server">
                                                   Incentive Amount/المبلغ الحافز
                                                </th>
                                                <th id="th_incperc" runat="server">
                                                   Incentive Percentage
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rpt_serdetail" runat="server" OnItemDataBound="rpt_serdetail_ItemDataBound">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <%# Container.ItemIndex + 1 %>
                                                            <asp:HiddenField ID="hdn_DId" runat="server" Value='<%#Eval("DId") %>' />
                                                        </td>
                                                        <td>
                                                            <asp:HiddenField ID="hdn_serviceId" runat="server" Value='<%#Eval("ServiceId") %>' />
                                                            <asp:Label ID="lbl_name" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label>
                                                        </td>
                                                        <td id="td_incamt" runat="server">
                                                            <asp:TextBox ID="txt_Incamt" Class="txt numbers_only txt_Incamt " runat="server" Text='<%#Eval("IncentiveAmount") %>'></asp:TextBox>
                                                        </td>
                                                        <td id="td_incperc" runat="server">
                                                            <asp:TextBox ID="txt_IncPer" Class="txt numbers_only txt_IncPer" runat="server" Text='<%#Eval("IncentivePercentage") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:Button ID="Button4" runat="server" class="butn_save" ValidationGroup="save_serdetail"
                                                    Text="Save/حفظ" OnClick="btn_SDSave_OnClick" />
                                                <asp:Button ID="Button7" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_sd_OnClick" />
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
    </div>
</asp:Content>
