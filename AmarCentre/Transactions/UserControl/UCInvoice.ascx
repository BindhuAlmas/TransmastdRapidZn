<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCInvoice.ascx.cs" Inherits="AmarCentre.Transactions.UserControl.UCInvoice" %>
<%@ Register Src="~/Masters/UserControl/UCService.ascx" TagName="ServiceMaster"
    TagPrefix="AmarCentre" %>
<%@ Register Src="~/Transactions/UserControl/Customer.ascx" TagName="CustomerMaster"
    TagPrefix="AmarCentre" %>
<%@ Register Src="~/Transactions/UserControl/UCMail.ascx" TagName="MailUC" TagPrefix="AmarCentre" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:UpdatePanel ID="Upd_Add_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdatePanel ID="upd_main" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div id="div_main" runat="server">
                                     <asp:Button ID="Button15" runat="server" style="display:none" Text="" OnClick="callSAveCompletion" />
                                    <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
                                        UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="Adding_headingLargepopup">
                                                Invoice / فاتورة
                                            </div>
                                            <table class="formTable">
                                                <tr>
                                                    <td style="width: 33%">
                                                        Invoice Code / رمز الفاتورة
                                                        <asp:TextBox ID="lbl_Code" runat="server" class="txt read_Only" Font-Bold="true"
                                                            Text=""></asp:TextBox>
                                                         <asp:TextBox ID="txt_token" AutoPostBack="true" OnTextChanged="txt_token_OnTextChanged"
                                                            class="txt" runat="server" Visible="false"></asp:TextBox>
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
                                                    <td rowspan="3">
                                                        <asp:UpdatePanel ID="Upd_CreditDetail_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Panel ID="pnl_CreditDetail" runat="server" Visible="false">
                                                                    <table class="listTable">
                                                                        <thead>
                                                                            <tr>
                                                                                <th>
                                                                                    Credit Detail / تفاصيل الائتمان
                                                                                </th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td>
                                                                                    Credit Available الائتمان المتاح
                                                                                    <asp:HiddenField ID="hdn_IsCredit" runat="server" Value="" />
                                                                                    <asp:HiddenField ID="hdn_CurrentInvoiceReceivable" runat="server" Value="" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    Credit Limit الحد الائتماني
                                                                                    <asp:Label ID="lblCreditLimit" runat="server"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    Current Credit Amount مبلغ الائتمان الحالي
                                                                                    <asp:Label ID="lblCurrentCreditAmt" runat="server"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                          <asp:UpdatePanel ID="Upd_agentDrop_Panel" runat="server" ChildrenAsTriggers="false"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                        Agent/وكيل
                                                        <telerik:RadComboBox ID="drpagent" Sort="Ascending" Filter="Contains" runat="server"
                                                            AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                            AutoPostBack="true" OnClientBlur="ValidateCombo" EmptyMessage="Search Name/اسم..."
                                                            OnSelectedIndexChanged="drp_agent_OnSelectedIndexChanged" Style="overflow: hidden;
                                                            width: 96%; border: none!important;">
                                                        </telerik:RadComboBox>
                                                                 </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                     <td>
                                                        Customer Name / اسم الزبون <span style="color: Red">&nbsp*</span>
                                                           <asp:UpdatePanel ID="Upd_CustomerDrop_Panel" runat="server" ChildrenAsTriggers="false"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <telerik:RadComboBox ID="drp_customer" Sort="Ascending" Filter="Contains" runat="server"
                                                                    AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                                    AutoPostBack="true" OnClientBlur="ValidateCombo" EmptyMessage="Search Name/اسم..."
                                                                    OnSelectedIndexChanged="drp_customer_OnSelectedIndexChanged" Style="overflow: hidden;
                                                                    width: 94%; border: none!important;">
                                                                </telerik:RadComboBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="drp_customer"
                                                                    ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                                    InitialValue=""></asp:RequiredFieldValidator>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                      
                                                    </td>
                                                </tr>
                                                <tr style="display: none">
                                                    <td>
                                                        <asp:RadioButton ID="rbTaxInvoice" Name="rbInputType" ClientIDMode="Static" runat="server"
                                                            GroupName="InvoiceType" />Tax Invoice/فاتورة ضريبية
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rbNormalInvoice" Name="rbInputType" ClientIDMode="Static" runat="server"
                                                            GroupName="InvoiceType" />Normal Invoice/فاتورة عادية
                                                    </td>
                                                </tr>
                                                <tr>
                                                   <td>
                                                        Billing Name
                                                         <asp:TextBox ID="txtBillingname" CssClass="txt" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        Quotation/اقتباس<br />
                                                        <asp:UpdatePanel ID="UpdQuotationPanel" runat="server" ChildrenAsTriggers="false"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <telerik:RadComboBox ID="drp_quot" Sort="Ascending" Filter="Contains" runat="server"
                                                                    AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                                    OnClientBlur="ValidateCombo" EmptyMessage="Search Quotation..." Style="overflow: hidden;
                                                                    width: 96%; border: none!important;" AutoPostBack="true" OnSelectedIndexChanged="drp_quo_OnSelectedIndexChanged">
                                                                </telerik:RadComboBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                   
                                                </tr>
                                                <tr>
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
                                                                                <th ></th>
                                                                                <th style="width: 3%">Sl.
                                                                                </th>
                                                                                <th style="width: 18%" colspan="2">Service / الخدمات
                                                                                </th>
                                                                                <th style="width: 10%">Particulars / تفاصيل
                                                                                </th>
                                                                                 <th style="width: 10%">Employee
                                                                                </th>
                                                                                <th style="width: 7%">Deadline</th>
                                                                                <th style="width: 7%">Price / السعر
                                                                                </th>
                                                                                <th style="width: 7%">Govt.Fee / رسوم الحكومة
                                                                                </th>
                                                                                <th style="width: 7%">
                                                                                    Service Charge
                                                                                </th>
                                                                                <th style="width: 5%">Fine / مبلغ الغرامة
                                                                                </th>
                                                                                <th runat="server" id="th_discount" style="width: 5%">Discount / خصم
                                                                                </th>
                                                                                 <th style="width: 5%">Service Commission
                                                                                </th>
                                                                                 <th runat="server" id="th_AgentCommission" style="width: 5%">Agent Commission
                                                                                </th>
                                                                                <th style="width: 4%">Qty
                                                                                </th>
                                                                                <th style="width: 5%">Tax
                                                                                </th>
                                                                                <th style="width: 7%">Amt With Tax
                                                                                </th>
                                                                                <th style="width: 7%">Total / مجموع
                                                                                </th>
                                                                                <th style="width: 5%">Action
                                                                                </th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <asp:Repeater ID="rpt_Item_list" runat="server" OnItemDataBound="rptitemlistDatabound">
                                                                                <ItemTemplate>
                                                                                    <tr style="text-align: center">
                                                                                        <td>
                                                                                            <asp:CheckBox ID="chk_sel" class="chk_sel supchkitem" runat="server" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <%# Container.ItemIndex + 1 %>
                                                                                        </td>
                                                                                        <td style="text-align: left" colspan="2">
                                                                                            <asp:HiddenField ID="hdnInvDId" runat="server" Value='<%#Eval("D_id") %>' />
                                                                                            <asp:HiddenField ID="hdnInvDDepartment" runat="server" Value='<%#Eval("DepartmentName") %>' />
                                                                                            <asp:HiddenField ID="hdnInvDSerCategory" runat="server" Value='<%#Eval("SerCategoryName") %>' />
                                                                                            <asp:HiddenField ID="hdnInvDSerSubCategory" runat="server" Value='<%#Eval("SerSubCategoryName") %>' />
                                                                                            <asp:Label ID="lblServiceFullName" Visible="false" runat="server" TabIndex="-1" Text='<%#Eval("ServiceFullName") %>' />
                                                                                            <asp:HiddenField ID="hdnInvDDepartmentId" runat="server" Value='<%#Eval("DepartmentId") %>' />
                                                                                            <asp:HiddenField ID="hdnInvDCategoryId" runat="server" Value='<%#Eval("CategoryId") %>' />
                                                                                            <asp:HiddenField ID="hdnInvDSerSubCategoryId" runat="server" Value='<%#Eval("ServiceSubCategoryId") %>' />
                                                                                            <asp:HiddenField ID="hdnInvDServiceId" runat="server" Value='<%#Eval("Service_Id") %>' />

                                                                                            <asp:HiddenField ID="hdnQuotationDetailId" runat="server" Value='<%#Eval("QuotationDetailId") %>' />


                                                                                            <div style="clear: both">

                                                                                                <telerik:RadComboBox ID="drpDepartmentIn" Sort="Ascending" Filter="Contains" runat="server"
                                                                                                    AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Department..."
                                                                                                    OnSelectedIndexChanged="drpFilter_OnSelectedIndexChangedIn" AutoPostBack="true"
                                                                                                    ClientIDMode="AutoID" DropDownWidth="500px"  Style="overflow: hidden; width: 95%; border: none!important;"
                                                                                                    OnClientFocus="OnClientKeyPressing" OnClientBlur="ValidateCombo">
                                                                                                </telerik:RadComboBox>

                                                                                            </div>
                                                                                            <div style="float: left; width: 47%">

                                                                                                <telerik:RadComboBox ID="drpSerCategoryIn" Sort="Ascending" Filter="Contains" runat="server"
                                                                                                    AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Category..."
                                                                                                    OnSelectedIndexChanged="drpFilter_OnSelectedIndexChangedIn" AutoPostBack="true"
                                                                                                    ClientIDMode="AutoID"  DropDownWidth="500px" Style="overflow: hidden; width: 97%; border: none!important;"
                                                                                                    OnClientFocus="OnClientKeyPressing" OnClientBlur="ValidateCombo">
                                                                                                </telerik:RadComboBox>

                                                                                            </div>
                                                                                            <div style="float: left; width: 47%">

                                                                                                <telerik:RadComboBox ID="drpSerSubCategoryIn" Sort="Ascending" Filter="Contains" runat="server"
                                                                                                    AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Sub Category..."
                                                                                                    OnSelectedIndexChanged="drpFilter_OnSelectedIndexChangedIn" AutoPostBack="true"
                                                                                                    ClientIDMode="AutoID" DropDownWidth="500px"  Style="overflow: hidden; width: 97%; border: none!important;"
                                                                                                    OnClientFocus="OnClientKeyPressing" OnClientBlur="ValidateCombo">
                                                                                                </telerik:RadComboBox>

                                                                                            </div>
                                                                                            <div style="clear: both">

                                                                                                <telerik:RadComboBox ID="drpServiceIn" Sort="Ascending" Filter="Contains" runat="server"
                                                                                                    AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Service..."
                                                                                                    OnSelectedIndexChanged="drpService_OnSelectedIndexChangedIn" AutoPostBack="true"
                                                                                                    NoWrap="true" DropDownWidth="700px" ClientIDMode="AutoID" Style="width: 95%; border: none!important;"
                                                                                                    OnClientFocus="OnClientKeyPressing" OnClientBlur="ValidateCombo">
                                                                                                </telerik:RadComboBox>

                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="text-align: left">
                                                                                            <asp:TextBox ID="lblInvDdesc" Width="90%" TabIndex="-1" TextMode="MultiLine" runat="server" Text='<%#Eval("Particulars") %>'></asp:TextBox>
                                                                                        </td>
                                                                                         <td style="text-align: left">
                                                                                            <asp:TextBox ID="txtCustomerStaffIn" Width="90%" TabIndex="-1" runat="server" TextMode="MultiLine" Text='<%#Eval("CustomerStaff") %>'></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <telerik:RadDatePicker ID="deadlineIn" runat="server" DateInput-DateFormat="dd/MM/yyyy" Width="100px">
                                                                                                <Calendar runat="server" ID="Calendar1" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                                                                    ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                                                                    <SpecialDays>
                                                                                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                                                        </telerik:RadCalendarDay>
                                                                                                    </SpecialDays>
                                                                                                </Calendar>
                                                                                            </telerik:RadDatePicker>
                                                                                            <asp:HiddenField ID="hdndeadline" runat="server" Value='<%#Eval("Deadline") %>' />
                                                                                        </td>
                                                                                        <td style="text-align: left">
                                                                                            <asp:TextBox ID="txtInvDDisplayPrice" class="txt unit_amtD  numbers_only "
                                                                                                Width="85%" runat="server" Text='<%#Eval("DisplayPrice") %>' TabIndex="-1"></asp:TextBox>
                                                                                            <asp:HiddenField ID="hdnInvDExpense" ClientIDMode="Static" runat="server" Value='<%#Eval("Expense") %>' />
                                                                                            <asp:HiddenField ID="hdnInvDServiceCharge" ClientIDMode="Static" runat="server" Value='<%#Eval("ServiceCharge") %>' />
                                                                                            <asp:HiddenField ID="hdnInvDPrice" ClientIDMode="Static" runat="server" Value='<%#Eval("Price") %>' />
                                                                                          
                                                                                        </td>
                                                                                        <td style="text-align: left;">
                                                                                            <asp:TextBox ID="txtInvDExpense" class="txt Expense_amtD  numbers_only "
                                                                                                Width="85%" runat="server" Text='<%#Eval("Expense") %>' TabIndex="-1"></asp:TextBox>
                                                                                        </td>
                                                                                          <td style="text-align: left;">
                                                                                            <asp:TextBox ID="txtInvDServiceCharge" class="txt txtInvDServiceCharge  read_Only "
                                                                                                Width="85%" runat="server" Text='<%#Eval("ServiceCharge") %>' TabIndex="-1"></asp:TextBox>
                                                                                        </td>
                                                                                        <td style="text-align: left">
                                                                                            <asp:TextBox ID="txtInvDFine" class="txt fine_amtD  numbers_only "
                                                                                                Width="85%" runat="server" Text='<%#Eval("Fine") %>' TabIndex="-1"></asp:TextBox>
                                                                                            <asp:HiddenField ID="hdnInvDFineApplicable" ClientIDMode="Static" runat="server"
                                                                                                Value='<%#Eval("FineApplicable") %>' />
                                                                                        </td>
                                                                                        <td runat="server" id="td_discount" style="text-align: left">
                                                                                            <asp:TextBox ID="txtInvDdiscount" class=" discountD InvDdiscount  txt"
                                                                                                Width="85%" runat="server" Text='<%#Eval("Discount") %>'></asp:TextBox>
                                                                                        </td>
                                                                                         <td style="text-align: left">
                                                                                            <asp:TextBox ID="txtInvDCommissionS" class="numbers_only txt CommissionD "
                                                                                                Width="75%" runat="server" Text='<%#Eval("ServiceCommission") %>' TabIndex="-1"></asp:TextBox>
                                                                                        </td>
                                                                                         <td runat="server" id="td_AgentCommission" style="text-align: left">
                                                                                            <asp:TextBox ID="txtAgentCommission" class=" AgentCommissionD  txt numbers_only"
                                                                                                Width="75%" runat="server" Text='<%#Eval("AgentCommission") %>'></asp:TextBox>
                                                                                        </td>

                                                                                        <td style="text-align: left">
                                                                                            <asp:TextBox ID="txtInvDQty" class="numbers_only txt qtyD  InvDQty "
                                                                                                Width="75%" runat="server" Text='<%#Eval("Quantity") %>' TabIndex="-1"></asp:TextBox>
                                                                                        </td>
                                                                                        <td style="text-align: left; display: none">
                                                                                            <asp:TextBox TabIndex="-1" ID="txtInvDVatPer" class="numbers_only read_Only txt asLabel"
                                                                                                Width="95%" runat="server" Text='<%#Eval("Tax") %>'></asp:TextBox>
                                                                                            <asp:TextBox ID="txtInvDAddServiceCharge" class="txt  read_Only numbers_only asLabel"
                                                                                                Width="85%" runat="server" Text='<%#Eval("AdditionalServiceCharge") %>' TabIndex="-1"></asp:TextBox>
                                                                                        </td>
                                                                                        <td style="text-align: left">
                                                                                            <asp:TextBox TabIndex="-1" ID="txtInvDTaxAmount" class="numbers_only taxamtD read_Only txt asLabel"
                                                                                                Width="95%" runat="server" Text='<%#Eval("TaxAmount") %>'></asp:TextBox>
                                                                                            <asp:HiddenField ID="hdnInvDTax" ClientIDMode="Static" runat="server" Value='<%#Eval("Tax") %>' />
                                                                                            <asp:HiddenField ID="hdnTemplateId" ClientIDMode="Static" runat="server" Value='<%#Eval("TemplateId") %>' />

                                                                                        </td>
                                                                                        <td style="text-align: left">
                                                                                            <asp:TextBox ID="txtInvDPriceWitTax" TabIndex="-1" class="numbers_only Prc_amtD read_Only txt asLabel"
                                                                                                Width="95%" runat="server" Text='<%#Eval("PriceWitTax") %>'></asp:TextBox>
                                                                                        </td>
                                                                                        <td style="text-align: left">
                                                                                            <asp:TextBox ID="txtInvDTotal" TabIndex="-1" class="numbers_only invtot il_tot_amtD read_Only txt asLabel"
                                                                                                Width="95%" runat="server" Text='<%#Eval("Total") %>'></asp:TextBox>
                                                                                        </td>
                                                                                        <td style="text-align: center;">
                                                                                            <asp:HiddenField ID="hdnIsCompleted" ClientIDMode="Static" runat="server" Value='<%#Eval("CompletedQuantity") %>' />
                                                                                            <asp:Button ID="btnCompleSC" CssClass="btn_completeTick" runat="server" OnClick="btnCompleSC_OnClick"
                                                                                                ToolTip="Service Completion" />
                                                                                            <asp:Button ID="btn_remove_line" CommandName="Delete" class="btn_delete" runat="server"
                                                                                                ToolTip="Delete" OnClick="btn_remove_line_OnClick" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');" />
                                                                                        </td>
                                                                                        <td style="display: none">
                                                                                            <asp:TextBox ID="txtExpenseQty" runat="server" Text='<%#Eval("ExpQty") %>'></asp:TextBox>
                                                                                            <asp:TextBox ID="txtExpenseSinglAmt" runat="server" Text='<%#Eval("ExpSinglAmt") %>'></asp:TextBox>
                                                                                            <asp:TextBox ID="txtExpenseTotalAmt" runat="server" Text='<%#Eval("ExpTotAmt") %>'></asp:TextBox>
                                                                                            <telerik:RadDatePicker ID="ExpenseSerComDate" runat="server" DbSelectedDate='<%#Eval("SerComDate") %>'
                                                                                                DateInput-DateFormat="dd/MM/yyyy">
                                                                                                <Calendar runat="server" ID="Calendaree1" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                                                                    ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                                                                    <SpecialDays>
                                                                                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                                                        </telerik:RadCalendarDay>
                                                                                                    </SpecialDays>
                                                                                                </Calendar>
                                                                                            </telerik:RadDatePicker>
                                                                                            <asp:Repeater ID="rptTransCode" runat="server">
                                                                                                <ItemTemplate>
                                                                                                    <table>
                                                                                                        <asp:TextBox ID="txtTransCode" runat="server" Text='<%#Eval("TransactionNumber") %>'></asp:TextBox>
                                                                                                    </table>
                                                                                                </ItemTemplate>
                                                                                            </asp:Repeater>
                                                                                            <asp:Repeater ID="rptexpensein" runat="server">
                                                                                                <ItemTemplate>
                                                                                                    <table>
                                                                                                        <asp:TextBox ID="txtInvDId" runat="server" Text='<%#Eval("InvDId") %>'></asp:TextBox>
                                                                                                        <asp:TextBox ID="txtSerComDetailId" runat="server" Text='<%#Eval("SerComDetailId") %>'></asp:TextBox>
                                                                                                        <asp:TextBox ID="txtExpenseId" runat="server" Text='<%#Eval("ExpenseId") %>'></asp:TextBox>
                                                                                                        <asp:TextBox ID="txtAmount" runat="server" Text='<%#Eval("Amount") %>'></asp:TextBox>
                                                                                                        <asp:TextBox ID="txtVAT" runat="server" Text='<%#Eval("VAT") %>'></asp:TextBox>
                                                                                                        <asp:TextBox ID="txtVendorId" runat="server" Text='<%#Eval("VendorId") %>'></asp:TextBox>
                                                                                                        <asp:TextBox ID="txtPayModeId" runat="server" Text='<%#Eval("PayModeId") %>'></asp:TextBox>
                                                                                                        <asp:TextBox ID="txtAccountId" runat="server" Text='<%#Eval("AccountId") %>'></asp:TextBox>
                                                                                                        <asp:TextBox ID="txtPayableAmount" runat="server" Text='<%#Eval("PayableAmount") %>'></asp:TextBox>
                                                                                                        <asp:TextBox ID="txtPaidAmount" runat="server" Text='<%#Eval("PaidAmount") %>'></asp:TextBox>
                                                                                                    </table>
                                                                                                </ItemTemplate>
                                                                                            </asp:Repeater>
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>
                                                                            <tr style="text-align: center" runat="server" id="trnewline">
                                                                                <td></td>
                                                                                <td>
                                                                                    <asp:Label ID="lblRepeaterSNo" Text="" TabIndex="-1" runat="server" />
                                                                                </td>
                                                                                <td style="text-align: left" colspan="2">
                                                                                    <asp:HiddenField ID="hdn_InvDetailId" runat="server" Value="" />
                                                                                    
                                                                                    <div style="clear: both">
                                                                                        <asp:UpdatePanel ID="UpdDepartmentDropdown" runat="server" ChildrenAsTriggers="false"
                                                                                            UpdateMode="Conditional">
                                                                                            <ContentTemplate>
                                                                                                <telerik:RadComboBox ID="drpDepartment" Sort="Ascending" Filter="Contains" runat="server"
                                                                                                    AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Department..."
                                                                                                    OnSelectedIndexChanged="drpFilter_OnSelectedIndexChanged" AutoPostBack="true"
                                                                                                    ClientIDMode="AutoID" Style="overflow: hidden; width: 95%; border: none!important;"
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
                                                                                                    OnSelectedIndexChanged="drpFilter_OnSelectedIndexChanged" AutoPostBack="true"
                                                                                                    ClientIDMode="AutoID" Style="overflow: hidden; width: 97%; border: none!important;"
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
                                                                                                    OnSelectedIndexChanged="drpFilter_OnSelectedIndexChanged" AutoPostBack="true"
                                                                                                    ClientIDMode="AutoID" Style="overflow: hidden; width: 97%; border: none!important;"
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
                                                                                                    OnSelectedIndexChanged="drpService_OnSelectedIndexChanged" AutoPostBack="true"
                                                                                                    NoWrap="true" DropDownWidth="700px" ClientIDMode="AutoID" Style="width: 95%; border: none!important;"
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
                                                                                            <asp:TextBox ID="txt_desc" Width="90%" TextMode="MultiLine"    runat="server" Text=""></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                 <td style="text-align: left">
                                                                                    <asp:UpdatePanel ID="updCustomerStaffOut" runat="server" ChildrenAsTriggers="false"
                                                                                        UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txtCustomerStaffOut" Width="90%" TextMode="MultiLine"  runat="server" Text=""></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td>
                                                                                    <telerik:RadDatePicker ID="deadline" runat="server" DateInput-DateFormat="dd/MM/yyyy" Width="100px">
                                                                                        <Calendar runat="server" ID="Calendar5" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                                                            ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                                                            <SpecialDays>
                                                                                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                                                </telerik:RadCalendarDay>
                                                                                            </SpecialDays>
                                                                                        </Calendar>
                                                                                    </telerik:RadDatePicker>
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
                                                                                <td style="text-align: left;">
                                                                                    <asp:UpdatePanel ID="UpdExpense" runat="server" ChildrenAsTriggers="false"
                                                                                        UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txtExpense" class="numbers_only Expense_amt inline txt"
                                                                                                Width="85%" runat="server" Text=""></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td style="text-align: left; ">
                                                                                    <asp:UpdatePanel ID="UpdTxtServiceCharge" runat="server" ChildrenAsTriggers="false"
                                                                                        UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txtServiceCharge" class="numbers_only txtServiceCharge inline txt"
                                                                                                Width="85%" runat="server" Text=""></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td style="text-align: left">
                                                                                    <asp:UpdatePanel ID="UpdTxtFine" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txtFine" class="numbers_only fine_amt inline txt" Width="85%" runat="server"
                                                                                                Text=""></asp:TextBox>
                                                                                            <asp:HiddenField ID="hdnFineApplicable" ClientIDMode="Static" runat="server" Value="" />
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td runat="server" id="td_maindiscount" style="text-align: right">
                                                                                    <asp:UpdatePanel ID="Updtxt_discount" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_discount" Style="text-align: right" class="numbers_only discount inline txt"
                                                                                                Width="85%" runat="server"></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td>
                                                                                     <asp:UpdatePanel ID="updCommissionSOut" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                     <asp:TextBox ID="txtCommissionSOut" class="numbers_only txt CommissionOut " Width="75%" runat="server" ></asp:TextBox>
                                                                                             </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                 <td runat="server" id="td_mainAgentCommission" style="text-align: right">
                                                                                    <asp:UpdatePanel ID="updAgentCommissionOut" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txtAgentCommissionOut" Style="text-align: right" class="numbers_only AgentCommissionOut txt"
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
                                                                                <td style="text-align: left; display: none">
                                                                                    <asp:UpdatePanel ID="UpdTxtTaxPer" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox TabIndex="-1" ID="txtVatPer" class="numbers_only read_Only txt asLabel"
                                                                                                Width="95%" runat="server" Text=""></asp:TextBox>
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
                                                                            <tr runat="server" id="tr_maindiscount">
                                                                                <td></td>
                                                                                <td  runat="server" id="tr_maindiscountIn" style="text-align: right">Discount </td>
                                                                                <td colspan="5">
                                                                                    <asp:UpdatePanel ID="Updtxt_totDiscount" runat="server" ChildrenAsTriggers="false"
                                                                                        UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox TabIndex="-1" Style="border: medium none; color: Red; font-size: 24px; text-align: right; width: 95%"
                                                                                                class="txt tot_discount read_Only" ID="txt_totDiscount"
                                                                                                runat="server"></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                             <tr>
                                                                                <td></td>
                                                                                <td runat="server" style="text-align: right" id="tdtxtCommssnTotal">Commission</td>
                                                                                <td colspan="5">
                                                                                    <asp:UpdatePanel ID="updCommssnTotal" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox TabIndex="-1" Style="border: medium none; color: Red; font-size: 24px; text-align: right; width: 95%"
                                                                                                class="txt txtCommssnTotal read_Only" ID="txtCommssnTotal"
                                                                                                runat="server"></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                             <tr>
                                                                                <td></td>
                                                                                <td runat="server" style="text-align: right" id="tdroundoff">Round off</td>
                                                                                <td colspan="5">
                                                                                    <asp:UpdatePanel ID="updRoundoff" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox TabIndex="-1" Style="border: medium none; color: Red; font-size: 24px; text-align: right; width: 95%"
                                                                                                class="txt txtroundoff read_Only" ID="txtroundoff"
                                                                                                runat="server"></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td runat="server" style="text-align: right" id="td_total">Grand Total</td>
                                                                                <td colspan="5">
                                                                                    <asp:UpdatePanel ID="Upd_Total_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox TabIndex="-1" Style="border: medium none; color: Red; font-size: 24px; text-align: right; width: 95%"
                                                                                                class="txt tot_grnd_amt read_Only" ID="txt_grand"
                                                                                                runat="server"></asp:TextBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
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
                                                    <td>
                                                        <asp:UpdatePanel ID="updBankCharge" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Panel ID="pnlbankcharge" runat="server">
                                                                    <table>
                                                                        <tr>
                                                                            <td>Payment Type</td>
                                                                            <td>
                                                                                <telerik:RadComboBox ID="drpPayType"  runat="server" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                                                    AutoPostBack="true" OnClientBlur="ValidateCombo" OnSelectedIndexChanged="drpPayTypeOnSelectedIndexChanged" >
                                                                                    <Items>
                                                                                        <telerik:RadComboBoxItem Value="1" Text="Cash Payment" />
                                                                                        <telerik:RadComboBoxItem Value="2" Text="Card Payment" />
                                                                                    </Items>
                                                                                </telerik:RadComboBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Bank Charge(%)
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox class="txt numbers_only txtbankchargeper" Style="width: 90%"  ID="txtbankchargeper" runat="server"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Charged Amount
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox class="txt numbers_only read_Only txtCharged" Style="width: 90%"  ID="txtCharged" runat="server"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Print Format <span style="color: Red">&nbsp*</span>
                                                        <telerik:RadComboBox ID="drpInvoiceFormat" Sort="Ascending" Filter="Contains" runat="server"
                                                            AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                            OnClientBlur="ValidateCombo" EmptyMessage="Search Invoice Format..." Style="overflow: hidden; width: 90%; border: none!important;">
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
                                                        <asp:RequiredFieldValidator ID="reqdrpInvoiceFormat" runat="server" ControlToValidate="drpInvoiceFormat"
                                                            ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                            InitialValue=""></asp:RequiredFieldValidator>
                                                    </td>
                                                     <td><asp:Panel ID="pnlinvoiceCreator" Visible="false" runat="server">
                                                         Invoice Creator <span style="color: Red">&nbsp*</span>
                                                        <telerik:RadComboBox ID="drpinvoiceCreator" Sort="Ascending" Filter="Contains" runat="server"
                                                            AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                            OnClientBlur="ValidateCombo" EmptyMessage="Search ..." Style="overflow: hidden; width: 90%; border: none!important;">
                                                        </telerik:RadComboBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="drpinvoiceCreator"
                                                            ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                            InitialValue=""></asp:RequiredFieldValidator>
                                                         </asp:Panel>
                                                    </td>
                                                    <td colspan="2"></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:UpdatePanel ID="Upd_total" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdnreceived" runat="server" Value="0" />

                                                                <asp:HiddenField ID="hdn_PageName" runat="server" Value="Invoice" />
                                                                <%--Regarding Customer User Control--%>
                                                                <asp:HiddenField ID="hdn_user_id" runat="server" />
                                                                <asp:HiddenField ID="hdnLanguage" runat="server" />
                                                                <asp:HiddenField ID="hdnPageId" runat="server" />
                                                                <asp:HiddenField ID="hdnfilter" runat="server" />
                                                                <asp:HiddenField ID="hdnCount" runat="server" />
                                                                <asp:HiddenField ID="hdnrequestId" runat="server" />

                                                                <asp:HiddenField ID="hdn_shwdiscount" ClientIDMode="Static" runat="server" />
                                                                <asp:HiddenField ID="hdnAgentCommmissionType" ClientIDMode="Static" runat="server" />
                                                                <asp:HiddenField ID="hdnSCInInvoice" ClientIDMode="Static" runat="server" />
                                                                <asp:HiddenField ID="hdnTaxAppliedWithDiscount" ClientIDMode="Static" runat="server" />
                                                                <asp:HiddenField ID="hdnDefaultInvoiceType" ClientIDMode="Static" runat="server" />
                                                                <asp:HiddenField ID="hdnSerPriceWTax" ClientIDMode="Static" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdnInvoiceStatus" ClientIDMode="Static" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdnDefaultBankCharge" ClientIDMode="Static" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdnIsQuotaionEditable" ClientIDMode="Static" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdnIsQuotaionEditablePrime" ClientIDMode="Static" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdnIsTaxprintall" ClientIDMode="Static" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdnDepartmentInInvoiceVisible" ClientIDMode="Static" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdnInvoiceFormatGen" ClientIDMode="Static" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdnIsDisableRoundOff" ClientIDMode="Static" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdnIsCommissionEditableInInvoice" ClientIDMode="Static" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdnIsEditInvoiceCreator" ClientIDMode="Static" runat="server" Value="0" />


                                                                <asp:Button ID="btn_save" class="butn_save" ValidationGroup="save" OnClick="btn_save_OnClick"
                                                                    runat="server" Text="Save/حفظ"  OnClientClick="if (Page_ClientValidate('save') == false) return(false);else return confirm('Do you really want to Save.. ?');"/>
                                                                <asp:Button ID="btn_save_print" class="butn_save" ValidationGroup="save" OnClick="btn_save_print_OnClick"
                                                                    runat="server" Text="Save & Print/حفظ وطباعة"  OnClientClick="if (Page_ClientValidate('save') == false) return(false);else return confirm('Do you really want to Save.. ?');"/>
                                                                      <asp:Button ID="btnMakePay" class="butn" ValidationGroup="save" runat="server" Visible="false" Text="Make Receipt/إيصال"
                                                                    OnClick="btnMakePay_OnClick" />
                                                                <asp:Button ID="btn_print" class="butn" runat="server" Text="Print/طباعة" OnClick="btn_print_OnClick" />
                                                                  <asp:Button ID="btnDuplicate" class="butn" runat="server" Visible="false" Text="Duplicate Invoice"
                                                                    OnClick="btnDuplicateInvoice_OnClick" />
                                                                    <asp:Button ID="btnSplitInvoice" class="butn" runat="server" Visible="false" Text="Split Invoice"
                                                                    OnClick="btnSplitInvoice_OnClick" />
                                                                <asp:Button ID="btn_cancel" class="butn_delete" runat="server" OnClientClick="javascript : return confirm('Do you really want to Cancel.. ?');"
                                                                    Visible="false" Text="Cancel/إلغاء" OnClick="btn_Cancelmain_OnClick" />
                                                                <asp:Button ID="btn_history" class="butn" runat="server" Visible="false" Text="History/سجل"
                                                                    OnClick="btn_histry_OnClick" />
                                                                <asp:UpdatePanel ID="Upd_btnTaxInvoicePrint" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:Button ID="btn_TaxInvoicePrint" class="butn" runat="server" Text="Tax Invoice Print/طباعة الفاتورة الضريبية "
                                                                            OnClick="btn_TaxInvoicePrint_OnClick" />
                                                                  <asp:HiddenField ID="hdnCustCommsnApplcable" ClientIDMode="Static" runat="server" Value="0" />
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                  <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_OnClick" />
                                                                <asp:Button ID="Button1" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_OnClick" />

                                                                <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_update" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_print" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_add_N_print" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_update_N_print" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_cancel" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_histry" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdn_TaxInvoicePrint" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdnMakeReceipt"  runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdnSplitInvoice" runat="server" Value="0" />
                                                                 <asp:HiddenField ID="hdnsendmail" runat="server" Value="0" />
                                                                 <asp:HiddenField ID="hdnduplicate" runat="server" Value="0" />

                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
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
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div id="div_trans_main" visible="false" runat="server">
                                    <div class="Adding_heading">
                                        Invoice History/تاريخ الفاتورة
                                    </div>
                                    <%--<div style="position: absolute; right: 45px; text-align: right; top: 8%;">
                                        <asp:LinkButton ID="LinkButton4" runat="server" Style="color: Blue" OnClick="btn_histry_Close_OnClick">Close</asp:LinkButton>
                                    </div>--%>
                                    <table style="margin-left: 20px; width: 60%">
                                        <tr>
                                            <td>
                                                From/من عند
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
                                            <td>
                                                To/إلى
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
                                                <asp:Button ID="Button6" class="butn" OnClick="btn_his_seacrh_OnClick" runat="server"
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
                                                                        <th style="text-align: center; width: 5%">
                                                                            Sl/رقم
                                                                        </th>
                                                                        <th style="text-align: center; width: 30%">
                                                                            Remark/تعليق
                                                                        </th>
                                                                        <th style="text-align: center; width: 10%">
                                                                            Done By/تم بواسطة
                                                                        </th>
                                                                        <th style="text-align: center; width: 10%">
                                                                            Date/تاريخ
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
                                                                <tr>
                                                                    <td colspan="4" class="navigationRow">
                                                                        <asp:UpdatePanel ID="upd_his_nav" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:Label ID="lbl_page_info1" runat="server" class="pageInfo"></asp:Label>
                                                                                <asp:Button ID="Button7" runat="server" class="navigationButton" Text="<<" OnClick="btn_first1_OnClick" />
                                                                                <asp:Button ID="Button8" runat="server" class="navigationButton" Text="<" OnClick="btn_prev1_OnClick" />
                                                                                <asp:Label ID="lbl_page_number1" Style="font-weight: bold; margin-left: 5px; margin-right: 5px;
                                                                                    text-align: center;" runat="server"></asp:Label>
                                                                                <asp:Button ID="Button9" class="navigationButton" runat="server" Text=">" OnClick="btn_next1_OnClick" />
                                                                                <asp:Button ID="Button10" class="navigationButton" runat="server" Text=">>" OnClick="btn_last1_OnClick" />
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
                                                <asp:Button ID="Button4" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_histry_Close_OnClick" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>

   <asp:UpdatePanel ID="Upd_Customer_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" >
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
          <asp:UpdatePanel ID="UpdServicepnlAdd" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" >
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

  <asp:UpdatePanel ID="upd_cancl" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnl_cancl" runat="server" Visible="false">
                <div class="popupBackground">
                </div>
                <div class="animated smallPopUp">
                    <div class="Adding_heading">
                        Cancel Invoice/الغاء الفاتورة
                    </div>
                    <div runat="server" visible="false" id="div_candet">
                        <div style="padding: 10px">
                            <b>Select the entries you want to cancel before cancelling invoice/حدد الادخالات التي
                                تريد الغاؤها قبل الغاء الفاتورة </b>
                        </div>
                        <table class="listTable">
                            <thead>
                                <tr>
                                    <th class="listTableSlNo" style="width: 5%;">
                                        Select/اختار
                                    </th>
                                    <th style="width: 20%;">
                                        Remark/تعليق
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rpt_cancelList" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="hdndetId" runat="server" Value='<%#Eval("Id")%>' />
                                                <asp:HiddenField ID="hdn_type" runat="server" Value='<%#Eval("Type")%>' />
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </td>
                                            <td>
                                                <%#Eval("Remark")%>
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
                                Remark/تعليق <span style="color: Red">&nbsp*</span>
                                <asp:TextBox ID="txt_cancelremark" CssClass="txtarea" TextMode="MultiLine" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txt_cancelremark"
                                    ValidationGroup="cancl" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="Button2" class="butn_save" ValidationGroup="cancl" OnClick="btn_cancel_OnClick"
                                  OnClientClick="if (Page_ClientValidate('cancl') == false) return(false);else return confirm('Do you really want to cancel.. ?');"  runat="server" Text="Cancel/إلغاء" />
                                <asp:Button ID="Button3" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_cnclse_OnClick" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upd_receipt" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlreceipt" runat="server" Visible="false">
                <div class="popupBackground">
                </div>
                <div class="animated halfPopUp">
                    <div class="Adding_heading">
                        Receipt/إيصال
                    </div>
                    <asp:UpdatePanel ID="updreceiptIn" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table class="formTable">
                                <tr>
                                    <td style="width: 48%">Invoice Code / رمز الفاتورة
                                <asp:TextBox ID="txtInvCode_Rec" ReadOnly="true" CssClass="txt" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width: 48%">Date / تاريخ <span style="color: Red">&nbsp*</span>
                                        <br />
                                        <telerik:RadDatePicker ID="ReceiptDate" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                            <Calendar runat="server" ID="Calendar3" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                <SpecialDays>
                                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                    </telerik:RadCalendarDay>
                                                </SpecialDays>
                                            </Calendar>
                                        </telerik:RadDatePicker>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ReceiptDate"
                                            ValidationGroup="saverec" ErrorMessage="Required" Style="color: Red" InitialValue=""></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Receipt Code / رمز الاستلام
                                <asp:TextBox ID="txtcode_Rec" ReadOnly="true" CssClass="txt" runat="server"></asp:TextBox>
                                    </td>
                                    <td>Total / مجموع
                                <asp:TextBox TabIndex="-1" class="txt  read_Only " ID="txtrectotal" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Discount / خصم
                                <asp:TextBox TabIndex="-1" class="txt  read_Only" ID="txtrecdiscount" runat="server"></asp:TextBox>
                                    </td>


                                    <td>Pending / قيد الانتظار
                                    <asp:TextBox TabIndex="-1" class="txt pendingAmt read_Only " ID="txt_pendingAmt"
                                        runat="server"></asp:TextBox>
                                        <asp:HiddenField ID="hdn_receivedAmt" runat="server" Value="0" ClientIDMode="Static" />
                                    </td>
                                </tr>
                               
                                <tr id="trChargedAmount" runat="server">
                                    <td>Charged Amount
                                    <asp:TextBox class="txt ChargedAmountRec numbers_only " ID="txtChargedAmountRec" runat="server"></asp:TextBox>
                                        <asp:HiddenField ID="hdnpaymenttype" runat="server" Value="0" ClientIDMode="Static" />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>Spot Commission 
                                         <asp:TextBox class="txt spotcommsn numbers_only " ID="txtspotCommission" runat="server"></asp:TextBox>
                                    </td>
                                    <td>Received Amount / المبلغ الذي تسلمه<span style="color: Red">&nbsp*</span>
                                        <asp:TextBox class="txt amtPayNow numbers_only " ID="txt_amtPayNow" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator23" ErrorMessage="Required"
                                            runat="server" ControlToValidate="txt_amtPayNow" ValidationGroup="saverec" InitialValue=""
                                            Style="color: Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td>Received Amount / المبلغ الذي تسلمه<span style="color: Red">&nbsp*</span>
                                        <asp:TextBox class="txt rAmt numbers_only " ID="txt_ReceivedAmt" runat="server"></asp:TextBox>
                                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ErrorMessage="Required"
                                        runat="server" ControlToValidate="txt_ReceivedAmt" ValidationGroup="saverec" InitialValue=""
                                        Style="color: Red" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td>Balance / توازن
                                    <asp:TextBox TabIndex="-1" class="txt balanceAmt read_Only " ID="txt_Balance"
                                        runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Payment Mode / طريقة الدفع <span style="color: Red">&nbsp*</span>
                                        <asp:UpdatePanel ID="UpdDrpPaymentModePAnel" runat="server" ChildrenAsTriggers="false"
                                            UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <telerik:RadComboBox ID="drp_payMode" Sort="Ascending" Filter="StartsWith" runat="server"
                                                    AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Payment Mode..."
                                                    Style="overflow: hidden; width: 95%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                    OnClientBlur="ValidateCombo" OnSelectedIndexChanged="drp_payModeRec_OnSelectedIndexChanged"
                                                    AutoPostBack="true">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Value="1" Text="Cash" />
                                                        <telerik:RadComboBoxItem Value="2" Text="Bank Transaction" />
                                                        <telerik:RadComboBoxItem Value="3" Text="Cheque" />
                                                        <telerik:RadComboBoxItem Value="4" Text="Advance" />
                                                        <telerik:RadComboBoxItem Value="5" Text="Loan" />
                                                        <telerik:RadComboBoxItem Value="6" Text="Card Swipe" />

                                                    </Items>
                                                </telerik:RadComboBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator34" runat="server" ControlToValidate="drp_payMode"
                                            ValidationGroup="saverec" ErrorMessage="Required" Style="color: Red" InitialValue=""
                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="Upd_PayMode_Panel" runat="server" ChildrenAsTriggers="false"
                                            UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnl_PayMode_Panel" Visible="false" runat="server">
                                                    <asp:Label ID="lblToLabel" runat="server" class="lbl" Text="PettyCash"></asp:Label>
                                                    <telerik:RadComboBox ID="drpPettyCash" runat="server"
                                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                        OnClientBlur="ValidateCombo" EmptyMessage="Search Name..." Style="overflow: hidden; width: 95%; border: none!important;">
                                                    </telerik:RadComboBox>
                                                    <telerik:RadComboBox ID="drpBankAccount" runat="server"
                                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                        AutoPostBack="true" OnSelectedIndexChanged="onchangedrp_bank" OnClientBlur="ValidateCombo"
                                                        EmptyMessage="Search Name..." Style="overflow: hidden; width: 95%; border: none!important;"
                                                        Visible="false">
                                                    </telerik:RadComboBox>
                                                     <telerik:RadComboBox ID="drpLoan" runat="server"
                                                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                                       OnClientBlur="ValidateCombo"
                                                                        EmptyMessage="Search Name..." Style="overflow: hidden; width: 85%; border: none!important;"
                                                                        Visible="false">
                                                                    </telerik:RadComboBox>
                                                    <asp:RequiredFieldValidator ID="rqTo" runat="server" ControlToValidate="drpPettyCash"
                                                        ValidationGroup="saverec" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                    <asp:HiddenField ID="hdn_bankcommsn" ClientIDMode="Static" runat="server" />
                                                     <asp:HiddenField ID="hdnisCommissionVat" ClientIDMode="Static" runat="server" />
                                                    <asp:TextBox TabIndex="-1" class="txt AdvanceAmt read_Only " ID="txtadvance" runat="server"></asp:TextBox>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                               <tr>
                                    <td>Bank Commission/عمولة البنك
                                        <asp:UpdatePanel ID="upd_commsn" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:TextBox class="txt comssnAmt numbers_only " ID="txt_commsn" runat="server"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                   <td>
                                       <asp:UpdatePanel ID="updCommissionVat" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                           <ContentTemplate>

                                               <asp:Panel ID="pnlCommissionVat" Visible="false" runat="server">
                                                   Vat on Commission
                                                <asp:TextBox class="txt numbers_only txt_80 txtCommissionVat" ID="txtCommissionVat" runat="server"></asp:TextBox>
                                               </asp:Panel>

                                           </ContentTemplate>
                                       </asp:UpdatePanel>
                                   </td>
                                </tr>
                                <tr runat="server" id="trRecChargedAmt">
                                    <td>Charged Amount
                                        <asp:TextBox class="txt numbers_only " ID="txtRecChargedAmt" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="Upd_Cheque_Panel" runat="server" ChildrenAsTriggers="false"
                                            UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnl_Cheque_Panel" Visible="false" runat="server">
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 50%">Cheque Date / تحقق من التاريخ <span style="color: Red">&nbsp*</span>
                                                                <telerik:RadDatePicker ID="cheque_date" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                                    <Calendar runat="server" ID="Calendar4" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                                        ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                                        <SpecialDays>
                                                                            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                            </telerik:RadCalendarDay>
                                                                        </SpecialDays>
                                                                    </Calendar>
                                                                </telerik:RadDatePicker>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="cheque_date"
                                                                    Display="Dynamic" ValidationGroup="saverec" ErrorMessage="Required" Style="color: Red"
                                                                    InitialValue=""></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td style="width: 50%">Cheque Number / رقم الشيك <span style="color: Red">&nbsp*</span>
                                                                <asp:TextBox ID="txt_chqNumber" class="txt" runat="server"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txt_chqNumber"
                                                                    Display="Dynamic" ValidationGroup="saverec" ErrorMessage="Required" Style="color: Red"
                                                                    InitialValue=""></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                               <tr>
                                <td colspan="2">
                                                        Remarks / ملاحظات
                                                        <asp:TextBox class="txtarea" Style="width: 90%" TextMode="MultiLine" ID="txtrecRemark"
                                                            runat="server"></asp:TextBox>
                                                    </td>
                                   </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="Button5" class="butn_save" ValidationGroup="saverec" OnClick="btn_SaveReceipt_OnClick"
                                            OnClientClick="if (Page_ClientValidate('saverec') == false) return(false);else return confirm('Do you really want to Save.. ?');"
                                            runat="server" Text="Save/حفظ" />
                                        <asp:Button ID="Button14" class="butn_save" ValidationGroup="saverec" OnClick="btn_SavePrintReceipt_OnClick"
                                            OnClientClick="if (Page_ClientValidate('saverec') == false) return(false);else return confirm('Do you really want to Save.. ?');"
                                            runat="server" Text="Save & Print/حفظ وطباعة" />
                                        <asp:Button ID="Button13" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_CloseReceipt_OnClick" />
                                        <asp:HiddenField ID="hdnisreceiptclick" runat="server" />
                                        <asp:HiddenField ID="hdnreceiptprint" runat="server" />

                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
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
    <%--  SC--%>
    <asp:UpdatePanel ID="UpdSC" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlSC" Visible="false" runat="server">
                <div class="popupBackground">
                </div>
                <div class="animated largePopUp" style="width: 90%">
                    <asp:UpdatePanel ID="UpdSCIn" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="Adding_heading">
                                Service Completion / استكمال الخدمة
                            </div>
                            <div id="div2" runat="server" style="width: 100%; overflow: auto;">
                                <asp:UpdatePanel ID="Upd_ItemList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table class="listTable">
                                            <thead>
                                                <tr style="text-align: center">
                                                    <th style="width: 30%">
                                                        Service / الخدمات
                                                    </th>
                                                    <th style="width: 9%">
                                                        Invoice Quantity / كمية الفاتورة
                                                    </th>
                                                    <th style="width: 9%">
                                                        Pending Quantity / الكمية المعلقة
                                                    </th>
                                                    <th style="width: 9%">
                                                        Quantity / كمية
                                                    </th>
                                                    <th style="width: 10%">
                                                        Amount For Single Qty / المبلغ للكمية الواحدة
                                                    </th>
                                                    <th style="width: 9%">
                                                        Total Amount / المبلغ الإجمالي
                                                    </th>
                                                    <th style="width: 9%">
                                                        Date / تاريخ
                                                    </th>
                                                    <th style="width: 7%">
                                                        Action/عمل
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr style="text-align: center">
                                                    <td style="text-align: left">
                                                        <asp:HiddenField ID="hdn_service_id" runat="server" />
                                                        <asp:HiddenField ID="hdn_ExpinvD_id" runat="server" />
                                                        <asp:HiddenField ID="hdn_fineAmt" runat="server" />
                                                        <asp:Label ID="lbl_service" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="text-align: left">
                                                        <asp:TextBox ID="txt_InvQty" class="numbers_only read_Only invQty txt asLabel" Width="75%"
                                                            TabIndex="-1" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td style="text-align: left">
                                                        <asp:TextBox ID="txt_InComQty" class="numbers_only read_Only inComQty inline txt asLabel"
                                                            TabIndex="-1" Width="75%" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td style="text-align: left">
                                                        <asp:TextBox ID="txtInlineQty" class="numbers_only txt inlineQty" Width="75%" runat="server"></asp:TextBox>
                                                        <asp:Label ID="lblcomplete" runat="server" Visible="false" Text="Completed" ForeColor="Green" Font-Bold="true"></asp:Label>
                                                        <asp:RequiredFieldValidator ID="RqtxtQty" runat="server" ControlToValidate="txtInlineQty"
                                                            ValidationGroup="inlineSave" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                            InitialValue="">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td style="text-align: left">
                                                        <asp:TextBox ID="txtInlineAmtSQty" class="numbers_only read_Only asLabel txt inlineamtSQty"
                                                            TabIndex="-1" Width="75%" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td style="text-align: left">
                                                        <asp:TextBox ID="txtInlineTotAmt" class="numbers_only read_Only asLabel txt inlinetotAmt"
                                                            TabIndex="-1" Width="75%" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <telerik:RadDatePicker ID="InlineSerComDate" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                            <Calendar runat="server" ID="Calendar41" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                                ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                                <SpecialDays>
                                                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                    </telerik:RadCalendarDay>
                                                                </SpecialDays>
                                                            </Calendar>
                                                        </telerik:RadDatePicker>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="InlineSerComDate"
                                                            ValidationGroup="inlineSave" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                            InitialValue="">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btn_expDetail_line" CssClass="btn_edit" runat="server" OnClick="btn_expDetail_line_OnClick"
                                                            ToolTip="Edit" />
                                                        <asp:Button ID="btnInlineSave" CssClass="btn_completeTick" runat="server" OnClick="btnInlineExpenseSave_OnClick"
                                                            ValidationGroup="inlineSave" ToolTip="Complete" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <table class="ActionTable">
                                <tr>
                                    <td colspan="4" rowspan="3" style="text-align: right">
                                        <asp:Button ID="Button11" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_closeSC_OnClick" />
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <div id="div3" class="messageAlert div_pop animated" style="display: none" runat="server">
                                    <div class="tick">
                                        &#10004
                                    </div>
                                    <div>
                                        <asp:Label ID="Label1" runat="server" class="messageLabel"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="Upd_Expense_Panel" runat="server" ChildrenAsTriggers="false"
                                UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="pnl_Expense_Panel" Visible="false" runat="server">
                                        <table class="listTable">
                                            <thead>
                                                <tr>
                                                    <th style="width: 200px">
                                                        Expense / مصروف
                                                    </th>
                                                    <th style="width: 100px">
                                                        Amount / المبلغ
                                                    </th>
                                                    <th style="width: 100px">
                                                        VAT / ضريبة
                                                    </th>
                                                    <th>
                                                        Vendor / بائع
                                                    </th>
                                                    <th>
                                                        Payment Mode / طريقة الدفع
                                                    </th>
                                                    <th>
                                                        Account / الحساب
                                                    </th>
                                                    <th style="width: 100px">
                                                        Payable Amount / المبلغ المستحق
                                                    </th>
                                                    <th style="width: 100px">
                                                        Paid Amount / المبلغ المدفوع
                                                    </th>
                                                    <%--  <th>
                                                            Action/عمل
                                                        </th>--%>
                                                </tr>
                                            </thead>
                                            <asp:Repeater ID="rpt_expense_list" runat="server" OnItemDataBound="rpt_expense_list_OnItemDataBound">
                                                <ItemTemplate>
                                                    <tr class="temp">
                                                        <td>
                                                            <asp:HiddenField ID="hdnSerComDetailId" runat="server" Value='<%#Eval("SerComDetailId") %>' />
                                                            <asp:HiddenField ID="hdn_expenseId" runat="server" Value='<%#Eval("ExpenseId") %>' />
                                                            <asp:Label ID="lbl_Expense" runat="server" Text='<%# Eval("ExpenseName") %>' />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_amt" Class="txt numbers_only jcalculation amt" runat="server"
                                                                Text='<%#Eval("Amount") %>'></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_amt"
                                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                InitialValue="">
                                                            </asp:RequiredFieldValidator>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_vat" Class="txt numbers_only jcalculation vat" runat="server"
                                                                Text='<%#Eval("VAT") %>'></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6a" runat="server" ControlToValidate="txt_vat"
                                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                InitialValue="">
                                                            </asp:RequiredFieldValidator>
                                                        </td>
                                                        <td>
                                                            <asp:HiddenField ID="hdn_vendorId" runat="server" Value='<%#Eval("VendorId") %>' />
                                                            <telerik:RadComboBox ID="drp_vendor" Sort="Ascending" Filter="Contains" runat="server"
                                                                AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Vendor..."
                                                                Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                OnClientBlur="ValidateCombo">
                                                            </telerik:RadComboBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drp_vendor"
                                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                InitialValue="">
                                                            </asp:RequiredFieldValidator>
                                                        </td>
                                                        <td>
                                                            <asp:HiddenField ID="hdn_payModeId" runat="server" Value='<%#Eval("PayModeId") %>' />
                                                            <telerik:RadComboBox ID="drp_payMode" Sort="Ascending" Filter="Contains" runat="server"
                                                                AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Payment Mode..."
                                                                Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                OnClientBlur="ValidateCombo" ClientIDMode="AutoID" OnSelectedIndexChanged="drp_payMode_OnSelectedIndexChanged"
                                                                AutoPostBack="true">
                                                            </telerik:RadComboBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="drp_payMode"
                                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                InitialValue="">
                                                            </asp:RequiredFieldValidator>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="Upd_Account_Panel" runat="server" ChildrenAsTriggers="false"
                                                                UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:HiddenField ID="hdn_accountId" runat="server" Value='<%#Eval("AccountId") %>' />
                                                                    <telerik:RadComboBox ID="drp_account" Sort="Ascending" Filter="Contains" runat="server"
                                                                        AllowCustomText="false" RenderMode="Lightweight" EmptyMessage="Search Account..."
                                                                        Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                        OnClientBlur="ValidateCombo">
                                                                    </telerik:RadComboBox>
                                                                    <asp:RequiredFieldValidator ID="rqdaccountIn" runat="server" ControlToValidate="drp_account"
                                                                        ValidationGroup="save" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                        InitialValue="">
                                                                    </asp:RequiredFieldValidator>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_payableAmount" Class="txt numbers_only read_Only payableAmount"
                                                                runat="server" Text='<%#Eval("PayableAmount") %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                              <asp:UpdatePanel ID="updpaidAmountIn" runat="server" ChildrenAsTriggers="false"
                                                                    UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                            <asp:TextBox ID="txt_paidAmount" Class="txt numbers_only paidAmount" runat="server"
                                                                Text='<%#Eval("PaidAmount") %>'></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_paidAmount"
                                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                InitialValue="">
                                                            </asp:RequiredFieldValidator>
                                                                         </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                        </td>
                                                        <%--  <td>
                                                                <asp:Button ID="btnInlineEdit" runat="server" OnClick="btnInlineEdit_OnClick" ToolTip="Edit"
                                                                    class="btn_edit" />
                                                            </td>--%>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                        <table class="formTable">
                                            <tr>
                                                <td style="width: 25%">
                                                    Quantity / كمية <span style="color: Red">&nbsp*</span>
                                                </td>
                                                <td style="width: 25%">
                                                    <asp:HiddenField ID="hdn_InComQty" runat="server" Value="0" ClientIDMode="Static" />
                                                    <asp:TextBox ID="txtscqty" class="numbers_only scQty" Width="75%" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="txtscqty"
                                                        ValidationGroup="savesc" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                        InitialValue="">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td style="width: 25%">
                                                </td>
                                                <td style="width: 25%">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%">
                                                    Date / تاريخ<span style="color: Red">&nbsp*</span>
                                                </td>
                                                <td style="width: 25%">
                                                    <telerik:RadDatePicker ID="SerComDate" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                        <Calendar runat="server" ID="Calendar2" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                            ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                            <SpecialDays>
                                                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                </telerik:RadCalendarDay>
                                                            </SpecialDays>
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="SerComDate"
                                                        ValidationGroup="savesc" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                        InitialValue="">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td style="width: 25%">
                                                </td>
                                                <td style="width: 25%">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Amount For Single Qty / المبلغ للكمية الواحدة
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_amtSQty" class="numbers_only read_Only amtSQty" Width="75%"
                                                        runat="server"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Total Amount / المبلغ الإجمالي
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_totAmt" class="numbers_only read_Only totAmt" Width="75%" runat="server"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Button ID="Button12" class="butn_save" ValidationGroup="savesc" OnClick="btn_saveSC_OnClick"
                                            runat="server" Text="Save/حفظ" />
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
            <asp:UpdatePanel ID="Upd_TransaDetail_Panel" runat="server" ChildrenAsTriggers="false"
                UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnl_transaDetail" Visible="false" runat="server">
                        <div class="popupBackground">
                        </div>
                        <div class="animated smallPopUp">
                            <div class="Adding_heading">
                                TransAction Detail / تفاصيل الصفقة
                            </div>
                            <table class="formTable">
                                <tr>
                                    <td>
                                        <div id="div5" runat="server" style="width: 100%; overflow: auto;">
                                            <div style="height: 10px">
                                            </div>
                                            <table class="listTable">
                                                <thead>
                                                    <tr style="text-align: center">
                                                        <th style="width: 3%">
                                                            Sl.No/رقم
                                                        </th>
                                                        <th style="width: 10%">
                                                            TransAction Number / رقم التحويلة
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rpt_TransacDetail" runat="server">
                                                        <ItemTemplate>
                                                            <tr style="text-align: center">
                                                                <td>
                                                                    <%# Container.ItemIndex + 1 %>
                                                                </td>
                                                                <td style="text-align: left">
                                                                    <asp:TextBox ID="txt_transNumber" class="txt" Width="75%" runat="server" Text='<%#Eval("TransActionNumber") %>'></asp:TextBox>
                                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txt_transNumber"
                                                                        ValidationGroup="finalsave" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                        InitialValue=""></asp:RequiredFieldValidator>--%>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                            <div style="height: 10px">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <asp:Button ID="btn_FinalSave" runat="server" class="butn_save" ValidationGroup="finalsave"
                                                Text="Save/حفظ" OnClick="btn_FinalSave_OnClick" />
                                            <asp:Button ID="btn_TransDetail_Close" class="butn" runat="server" Text="Close/أغلق"
                                                OnClick="btn_TransDetail_Close_OnClick" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="updAlert" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlAlert" Visible="false" runat="server">
                        <div class="popupBackground">
                        </div>
                        <div class="animated smallPopUpCentre" style="min-height: 100px; padding: 2%; overflow: hidden">
                            Amount cannot be greater than Credit Limit. Do you want to Continue ?
                            <div>
                                <br />
                                <asp:Button ID="Button16" Width="75px" Height="30px" runat="server" Text="Ok" OnClick="btnYesOnClick" />
                                <asp:Button ID="Button18" Width="75px" Height="30px" runat="server" Text="Cancel" OnClick="btnNoOnClick" />
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
              <asp:UpdatePanel ID="updAlertReceivedamt" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlAlertReceivedamt" Visible="false" runat="server">
                        <div class="popupBackground">
                        </div>
                        <div class="animated smallPopUpCentre" style="min-height: 100px; padding: 2%; overflow: hidden">
                           <asp:Label ID="lblAlertReceivedamt" runat="server"></asp:Label>
                            <asp:HiddenField ID="hdnaction" runat="server" />
                            <div>
                                <br />
                                <asp:Button ID="Button20" Width="75px" Height="30px" runat="server" Text="Proceed" OnClick="btnRAYesOnClick" />
                                <asp:Button ID="Button21" Width="75px" Height="30px" runat="server" Text="Cancel" OnClick="btnRANoOnClick" />
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="updSetCredit" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlSetCredit" Visible="false" runat="server">
                        <div class="popupBackground">
                        </div>
                        <div class="animated smallPopUpCentre" style="min-height: 100px; padding: 2%; overflow: hidden">
                        Current Credit limit
                            <br />
                            <asp:TextBox ID="txt_CreditAmountLimit" ReadOnly="true" class="numbers_only txt" runat="server"></asp:TextBox>
                            <br />
                            Current Credit
                            <br />
                            <asp:TextBox ID="txt_CreditAmountCurrent" ReadOnly="true" class="numbers_only txt" runat="server"></asp:TextBox>
                            <br />
                            Set Credit limit
                            <br />
                            <asp:TextBox ID="txt_CreditAmount" class="numbers_only txt" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txt_CreditAmount"
                                ValidationGroup="saveset" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                InitialValue="">
                            </asp:RequiredFieldValidator>
                            <div>
                                <br />
                                <asp:Button ID="Button17" Width="75px" Height="30px" runat="server" ValidationGroup="saveset" Text="Save"
                                    OnClick="btnSetYesOnClick" />
                                <asp:Button ID="Button19" Width="75px" Height="30px" runat="server" Text="Cancel" OnClick="btnSetNoOnClick" />
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

              <asp:UpdatePanel ID="updAlertCommn" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlAlertCommn" Visible="false" runat="server">
                    <div class="popupBackground">
                    </div>
                    <div class="animated smallPopUpCentre" style="min-height: 100px; padding: 2%; overflow: hidden">
                        <asp:Label ID="lblAlertCommn" Font-Size="16px" runat="server"></asp:Label>
                        <div>
                            <br />
                            <asp:Button ID="Button22" Width="75px" Height="30px" runat="server" CssClass="butn" Text="Close" OnClick="btnAlertCloseOnClick" />
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        </ContentTemplate>
    </asp:UpdatePanel>