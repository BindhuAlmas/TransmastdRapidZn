<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCService.ascx.cs" Inherits="AmarCentre.Masters.UserControl.UCService" %>
<%@ Register Src="~/Masters/UserControl/UCDepartment.ascx" TagName="DepartmentMaster" TagPrefix="AmarCentre" %>
<%@ Register Src="~/Masters/UserControl/UCExpense.ascx" TagName="ExpenseMaster" TagPrefix="AmarCentre" %>
<%@ Register Src="~/Masters/UserControl/UCVendor.ascx" TagName="VendorMaster" TagPrefix="AmarCentre" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<script src="../Scripts/jquery.min.js" type="text/javascript"></script>

<div>

    <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
        UpdateMode="Conditional">
        <ContentTemplate>
            <div class="Adding_headingLargepopup">
                Service/الخدمات
            </div>
            <table class="formTable">
                <tr>
                    <td>Code/رمز<br />
                        <asp:Label ID="lbl_code" runat="server"></asp:Label>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Name/اسم <span style="color: Red">&nbsp*</span>
                        <asp:TextBox ID="txt_name" CssClass="txt" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txt_name"
                            ValidationGroup="saveser" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                            InitialValue=""></asp:RequiredFieldValidator>
                    </td>
                    <td>Name In Arabic/الاسم بالعربي 
                                            <br />
                        <asp:TextBox ID="txt_nameArabic" CssClass="txt" runat="server"></asp:TextBox>
                    </td>
                     <td>Department/قسم <span runat="server" id="DepartmentSpan" style="color: Red">&nbsp*</span>
                        <br />
                        <asp:UpdatePanel ID="UpdDepartmentDrop_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <telerik:RadComboBox ID="drpDepartment" Sort="Ascending" Filter="Contains" runat="server"
                                    AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                    AutoPostBack="true" OnSelectedIndexChanged="drpDepartment_SelectedIndexChanged"
                                    OnClientBlur="ValidateCombo" EmptyMessage="Search Department..." Style="overflow: hidden; width: 96%; border: none!important;">
                                </telerik:RadComboBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:RequiredFieldValidator ID="DepartmentRFValidator" runat="server" ControlToValidate="drpDepartment"
                            Display="Dynamic" ValidationGroup="saveser" ErrorMessage="Required" Style="color: Red"
                            InitialValue=""></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                   
                    <td runat="server" id="tdCat">Service Category/فئة الخدمة <span runat="server" id="CategorySpan" style="color: Red">&nbsp*</span>
                        <br />
                        <telerik:RadComboBox ID="drp_serCat" Sort="Ascending" Filter="Contains" runat="server"
                            AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                            AutoPostBack="true" OnSelectedIndexChanged="drp_serCat_OnSelectedIndexChanged"
                            OnClientBlur="ValidateCombo" EmptyMessage="Search Service Category..." Style="overflow: hidden; width: 96%; border: none!important;">
                        </telerik:RadComboBox>
                        <asp:RequiredFieldValidator ID="CategoryRFValidator" runat="server" ControlToValidate="drp_serCat"
                            Display="Dynamic" ValidationGroup="saveser" ErrorMessage="Required" Style="color: Red"
                            InitialValue=""></asp:RequiredFieldValidator>
                    </td>
                    <td runat="server" id="tdSubcat">Service Sub Category/فئة الخدمات الفرعية <span runat="server" id="SubCategorySpan" style="color: Red">&nbsp*</span>
                        <br />
                        <asp:UpdatePanel ID="Upd_SerSubCategory_Panel" runat="server" ChildrenAsTriggers="false"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                                <telerik:RadComboBox ID="drpSerSubCategory" Sort="Ascending" Filter="Contains" runat="server"
                                    AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                    OnClientBlur="ValidateCombo" EmptyMessage="Search Service Sub Category..." Style="overflow: hidden; width: 96%; border: none!important;">
                                </telerik:RadComboBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:RequiredFieldValidator ID="SubCategoryRFValidator" runat="server" ControlToValidate="drpSerSubCategory"
                            Display="Dynamic" ValidationGroup="saveser" ErrorMessage="Required" Style="color: Red"
                            InitialValue=""></asp:RequiredFieldValidator>
                    </td>
                    <td>Customer Price/السعر <span style="color: Red">&nbsp*</span>
                        <asp:TextBox ID="txt_price" CssClass="txt numbers_only" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_price"
                            ValidationGroup="saveser" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                            InitialValue=""></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Tax/ضريبة <span style="color: Red">&nbsp*</span>
                        <asp:TextBox ID="txt_tax" CssClass="txt numbers_only" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_tax"
                            ValidationGroup="saveser" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                            InitialValue=""></asp:RequiredFieldValidator>
                        <asp:CheckBox ID="chk_incApp" runat="server" Text="Incentive Applicable/تطبيق الحوافز " />
                        <br />
                        <asp:CheckBox ID="chk_enable" runat="server" Text="Enable" />
                    </td>
                    <td colspan="2">Description/وصف
                                            <br />
                        <asp:TextBox ID="txt_desc" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkValidity" runat="server" Text="" OnCheckedChanged="chkValidity_OnCheckedChanged"
                            AutoPostBack="true" />Validity/صلاحية 
                        <br />
                        <asp:CheckBox ID="chkrefund" runat="server" Text="Is Refundable" />
                         <br />
                        <asp:CheckBox ID="chkIsSCNotRequired"  runat="server" Text="No Cost for this service" />
                       
                        <asp:CheckBox ID="chkIsSetZeroPaidAmt" Visible="false" runat="server" Text="Set Expense Paid Amount Zero by default" />

                    </td>
                    <td>
                        <asp:UpdatePanel ID="Upd_Validity_Panel" runat="server" ChildrenAsTriggers="false"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnl_Validity" Visible="false" runat="server">
                                    Validity Year  <span style="color: Red">&nbsp*</span>
                                    <asp:TextBox ID="txtValidityExpiresOn" class="txt numbers_only" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtValidityExpiresOn"
                                        ValidationGroup="saveser" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                        InitialValue=""></asp:RequiredFieldValidator>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="Upd_Document_Panel" runat="server" ChildrenAsTriggers="false"
                                UpdateMode="Conditional">
                                <ContentTemplate>
                                         <asp:Panel ID="pnl_document" Visible="false" runat="server">
                                            Documents
                                    <telerik:RadComboBox ID="drpDocument" Sort="None"   Filter="StartsWith" runat="server"
                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                        OnClientBlur="ValidateCombo" OnSelectedIndexChanged="drpDocument_SelectedIndexChanged" EmptyMessage="Search Type..." Style="overflow: hidden;
                                        width: 96%; border: none!important;">
                                    </telerik:RadComboBox>
                                         </asp:Panel>
                                   </ContentTemplate>
                                </asp:UpdatePanel>
                      <%--  Documents
                                <telerik:RadComboBox ID="drpDocument" Sort="None"   Filter="StartsWith" runat="server"
                                    AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                    OnClientBlur="ValidateCombo" OnSelectedIndexChanged="drpDocument_SelectedIndexChanged" EmptyMessage="Search Type..." Style="overflow: hidden;
                                    width: 96%; border: none!important;">
                                </telerik:RadComboBox>--%>



                     </td>

                </tr>
            </table>
             <div class="HeadIng_Div">
              Followup Service details
            </div>
            <asp:UpdatePanel ID="updSubservice" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="listTable" style="width:75%">
                        <thead>
                            <tr>
                                <th style="width:7%">Sl No
                                </th>
                                <th style="width:25%">Department
                                </th>
                                <th style="width:25%">Service
                                </th>
                                <th style="width:15%">DeadlinePeriod (Days)
                                </th>
                                <th style="width:7%">Action
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptsubservice" runat="server" OnItemDataBound="rptsubservice_ItemDataBound" 
                                OnItemCommand="rptsubservice_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <%# Container.ItemIndex + 1 %>
                                            <asp:HiddenField ID="hdnDId" runat="server" Value='<%#Eval("Id") %>' />
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="hdnDepartmentId" runat="server" Value='<%#Eval("DepartmentId") %>' />
                                            <telerik:RadComboBox ID="drpDepartIn" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search ..." 
                                                ClientIDMode="AutoID" AutoPostBack="true" OnSelectedIndexChanged="drpDepartIn_SelectedIndexChanged"
                                                Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                OnClientBlur="ValidateCombo">
                                            </telerik:RadComboBox>
                                           
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="updSubserviceIn" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:HiddenField ID="hdnsubserviceId" runat="server" Value='<%#Eval("SubServiceId") %>' />
                                                    <telerik:RadComboBox ID="drpSubserviceIn" Sort="Ascending" Filter="Contains" runat="server"
                                                        AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search ..."
                                                        Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                        OnClientBlur="ValidateCombo" DropDownWidth="200px">
                                                    </telerik:RadComboBox>
                                                    <asp:RequiredFieldValidator ID="reqdrpSubserviceIn" runat="server" ControlToValidate="drpSubserviceIn"
                                                        ValidationGroup="savedd" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                        InitialValue="">
                                                    </asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDays" Class="txt numbers_only" runat="server" Text='<%#Eval("DeadlineDays") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="reqtxtDays" runat="server" ControlToValidate="txtDays"
                                                ValidationGroup="savedd" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                InitialValue="">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                        <td class="listTableActionButtonDiv">
                                            <asp:Button ID="btn_ss" runat="server" class="btn_add_new"
                                                ValidationGroup="savedd" CommandName="Add"  />
                                            <asp:Button ID="btn_ssr" class="btn_delete" runat="server" ToolTip="Delete"
                                               CommandName="Delete" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="HeadIng_Div">
                Service Expense Details/بيانات الخدمة 
                                <asp:Button ID="btn_serDetail_newEntry" runat="server"  class="btnAddNew"
                                    style="margin-right:5px"     OnClick="btn_serDetail_newEntry_OnClick" ValidationGroup="save_serdetail" />
                <asp:Button ID="btnexpense" runat="server" Text="Add Expense" style="margin-right:5px;float:right"  class="butn" Width="100px" Font-Size="Small" OnClick="btnexpense_OnClick" />
               <asp:Button ID="btnvendor" runat="server" Text="Add Vendor" style="margin-right: 5px;float:right" class="butn" Width="100px" Font-Size="Small" OnClick="btnvendorOnClick" />
            </div>
            <asp:UpdatePanel ID="Upd_ItemList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="listTable">
                        <thead>
                            <tr>
                                <th class="listTableSlNo">Sl No/رقم
                                </th>
                                <th>Expense/مصروف
                                </th>
                                <th>Amount/المبلغ
                                </th>
                                <th>VAT/ضريبة
                                </th>
                                <th>Vendor/بائع
                                </th>
                                <th>
                                    Vendor Commission
                                </th>
                                <th>Payment Mode/ طريقة الدفع
                                </th>
                                <th>Account/حسابات

                                </th>
                                <th>Tax Exempt/معفاة من الضريبة 
                                </th>
                                <th class="listTableAction">Action/عمل
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rpt_serdetail" runat="server" OnItemDataBound="rpt_serdetail_OnItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <%# Container.ItemIndex + 1 %>
                                            <asp:HiddenField ID="hdn_serDetailId" runat="server" Value='<%#Eval("SerDetailId") %>' />
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="hdn_expenseId" runat="server" Value='<%#Eval("ExpenseId") %>' />
                                            <telerik:RadComboBox ID="drp_expense" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search Expense..." DropDownWidth="200px"
                                                Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                OnClientBlur="ValidateCombo">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4a" runat="server" ControlToValidate="drp_expense"
                                                ValidationGroup="save_serdetail" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                InitialValue="">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_amt" Class="txt numbers_only" runat="server" Text='<%#Eval("Amount") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_amt"
                                                ValidationGroup="save_serdetail" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                InitialValue="">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_vat" Class="txt numbers_only" runat="server" Text='<%#Eval("VAT") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txt_vat"
                                                ValidationGroup="save_serdetail" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                InitialValue="">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="hdn_vendorId" runat="server" Value='<%#Eval("VendorId") %>' />
                                            <telerik:RadComboBox ID="drp_vendor" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search Vendor..." DropDownWidth="200px"
                                                Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                OnClientBlur="ValidateCombo">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="drp_vendor"
                                                ValidationGroup="save_serdetail" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                InitialValue="">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                          <td>
                                            <asp:TextBox ID="txtvendorCommission" Class="txt numbers_only" runat="server" Text='<%#Eval("vendorCommission") %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="hdn_payModeId" runat="server" Value='<%#Eval("PayModeId") %>' />
                                            <telerik:RadComboBox ID="drp_payMode" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search Payment Mode..." 
                                                Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                OnClientBlur="ValidateCombo" ClientIDMode="AutoID" OnSelectedIndexChanged="drp_payMode_OnSelectedIndexChanged"
                                                AutoPostBack="true">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="drp_payMode"
                                                ValidationGroup="save_serdetail" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                InitialValue="">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="Upd_Account_Panel" runat="server" ChildrenAsTriggers="false"
                                                UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:HiddenField ID="hdn_accountId" runat="server" Value='<%#Eval("AccountId") %>' />
                                                    <telerik:RadComboBox ID="drp_account" Sort="Ascending" Filter="Contains" runat="server"
                                                        AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search Account..." DropDownWidth="200px"
                                                        Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                        OnClientBlur="ValidateCombo">
                                                    </telerik:RadComboBox>
                                                    <asp:RequiredFieldValidator ID="rqdaccountIn" runat="server" ControlToValidate="drp_account"
                                                        ValidationGroup="save_serdetail" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                        InitialValue="">
                                                    </asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="hdn_taxExempt" runat="server" Value='<%#Eval("TaxExempt") %>' />
                                            <asp:CheckBox ID="chk_taxExempt" runat="server" Text="" />
                                        </td>
                                        <td class="listTableActionButtonDiv">
                                            <asp:Button ID="btn_serDetail_newEntry" runat="server" class="btn_add_new"
                                                ValidationGroup="save_serdetail" OnClick="btn_serDetail_newEntry_OnClick" />
                                            <asp:Button ID="btn_remove_line" class="btn_delete" runat="server" ToolTip="Delete"
                                                OnClick="btn_remove_line_OnClick" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <table class="formTable">
                <tr>
                    <td>
                        <div>
                            <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                            <asp:HiddenField ID="hdn_user_id" runat="server" />
                            <asp:HiddenField ID="hdnPageId" runat="server" />

                            <asp:Button ID="btn_save" class="butn_save" ValidationGroup="saveser" OnClick="btn_save_OnClick"
                                runat="server" Text="Save/حفظ" />
                            <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_OnClick" />
                            <asp:Button ID="btn_delete" class="butn_delete" runat="server" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');"
                                Visible="false" Text="Delete/حذف" OnClick="btn_delete_OnClick" />
                            <asp:Button ID="Button2" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_OnClick" />
                            <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                            <asp:HiddenField ID="hdn_update" runat="server" Value="0" />
                            <asp:HiddenField ID="hdn_delete" runat="server" Value="0" />
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdDepartmentPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" style="height: 100%;">
        <ContentTemplate>
            <asp:Panel ID="pnlDepartment" Visible="false" runat="server">
                <AmarCentre:DepartmentMaster ID="UC_Department" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
     <asp:UpdatePanel ID="UpdExpensePanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" style="height: 100%;">
        <ContentTemplate>
            <asp:Panel ID="pnlExpense" Visible="false" runat="server">
                <AmarCentre:ExpenseMaster ID="UC_Expense" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
     <asp:UpdatePanel ID="UpdVendorPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" style="height: 100%;">
        <ContentTemplate>
            <asp:Panel ID="pnlVendor" Visible="false" runat="server">
                <AmarCentre:VendorMaster ID="UC_Vendor" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
