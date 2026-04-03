<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true"
    CodeBehind="LeadFollowup.aspx.cs" Inherits="AmarCentre.CRM.LeadFollowup" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/CRM/UserControl/UCAnswer.ascx" TagName="Answer"
    TagPrefix="AmarCentre" %>
<%@ Register Src="~/CRM/UserControl/UCStatus.ascx" TagName="Status"
    TagPrefix="AmarCentre" %>
<%@ Register Src="~/CRM/UserControl/UCQuestion.ascx" TagName="Question"
    TagPrefix="AmarCentre" %>
<%@ Register Src="~/CRM/UserControl/UCPriority.ascx" TagName="Priority"
    TagPrefix="AmarCentre" %>
<%@ Register Src="~/CRM/UserControl/UCSegment.ascx" TagName="Segment"
    TagPrefix="AmarCentre" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Lead Followup
        <asp:Button ID="btn_filter" runat="server" ToolTip="Filter" class="filter right_align_list" OnClick="btn_filter_OnClick" />
        <asp:Button ID="btnExportToExcel" runat="server" class="btn_excel right_align_list"
            ToolTip="Export to Excel" OnClick="btnExportToExcelOnClick" />
        <div class="searchDiv">
            <asp:TextBox ID="txtSearch" runat="server" class="txt_search" AutoPostBack="true"
                OnTextChanged="txtSearchOnTextChanged" placeholder="Search"></asp:TextBox>
        </div>

    </div>
    <asp:UpdatePanel ID="upd_nav_filter" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnl_filter" runat="server" Visible="false">
                <div class="animated smallPopUp">
                    <div class="Adding_heading">
                        Search
                    </div>
                    <table class="formTable">
                        <tr>
                            <td>From Date
                                <telerik:RadDatePicker ID="txt_reg_Frm_date" runat="server" class="input-boder" DateInput-DateFormat="dd/MM/yyyy">
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
                            <td>To Date
                                <telerik:RadDatePicker ID="txt_reg_to_date" runat="server" class="input-boder" DateInput-DateFormat="dd/MM/yyyy">
                                    <Calendar runat="server" ID="Calendar4" CssClass="rtlSupport" ShowOtherMonthsDays="False"
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
                            <td>Status
                                <telerik:RadComboBox ID="drpStatusfilter" Sort="Ascending" Filter="Contains" runat="server"
                                    AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                    OnClientBlur="ValidateCombo" EmptyMessage="Search Status..."
                                    Style="overflow: hidden; width: 80%; border: none!important; padding-right: 5px; margin-top: 0px">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Priority
         <telerik:RadComboBox ID="drpprorityfilter" Sort="Ascending" Filter="Contains" runat="server"
             AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
             OnClientBlur="ValidateCombo" EmptyMessage="Search Priority..."
             Style="overflow: hidden; width: 80%; border: none!important; padding-right: 5px; margin-top: 0px">
         </telerik:RadComboBox>
                            </td>
                        </tr>
                          <tr style="display:none;">
                            <td>Segment
         <telerik:RadComboBox ID="drpSegmentfilter" Sort="Ascending" Filter="Contains" runat="server"
             AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
             OnClientBlur="ValidateCombo" EmptyMessage="Search Segment..."
             Style="overflow: hidden; width: 80%; border: none!important; padding-right: 5px; margin-top: 0px">
         </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btn_search" ValidationGroup="save" class="butn" runat="server" OnClick="btn_search_OnClick"
                                    Text="Search" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <asp:UpdatePanel ID="UpdPanelList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="hdnOrderByColumnName" runat="server" />
                <asp:HiddenField ID="hdnOrderBy" runat="server" />
                <div class="list_info" style="display: none">
                </div>
                <table class="listTable">
                    <thead>
                        <tr>
                            <th style="width: 3%;">Sl
                            </th>
                            <%--<th style="width: 13%;">Contact Person
                            </th>--%>
                            <th style="width: 10%;">Lead
                            </th>
                            <th style="width: 7%;">Contact No
                            </th>
                            <th style="width: 10%;">Next Follow up Date
                            </th>
                            <th style="width: 8%;">AssignedEmployee
                            </th>
                            <%--<th style="width: 8%;">Activity
                            </th>--%>
                            <th style="width: 8%;">Status
                            </th>
                             
                            <th style="width: 7%;">Priority
                            </th>
                             <th style="width: 10%;">Last Updated Date
                            </th>
                            <th style="width: 2%;">Action
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptListOnItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%#Eval("RowNum")%>.
                                        <asp:HiddenField ID="hdnId" runat="server" Value='<%#Eval("Id")%>' />
                                    </td>
                                    <%--<td>
                                        <%#Eval("ContactPersonName")%>
                                    </td>--%>
                                    <td>
                                        <%#Eval("CompanyName")%>
                                    </td>
                                    <td>
                                        <%#Eval("MobileNumber")%>
                                    </td>
                                    <td>
                                        <%#Eval("NextFollowupDate")%>
                                    </td>
                                    <td>
                                        <%#Eval("AssignedEmployee")%>
                                    </td>
                                      <%--<td>
                                        <%#Eval("Segmentname")%>
                                    </td>--%>
                                      <td>
                                        <%#Eval("Statusname")%>
                                    </td>
                                    <td>
                                        <%#Eval("Priorityname")%>
                                    </td>
                                     <td>
                                        <%#Eval("LastUpdatedDate")%>
                                    </td>
                                    <td class="listTableActionButtonDiv">
                                        <asp:Button ID="btnEdit" runat="server" class="btn_edit" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td colspan="11" class="navigationRow">
                                <asp:UpdatePanel ID="UpdPanelNavigation" runat="server" ChildrenAsTriggers="false"
                                    UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblPageInfo" runat="server" class="pageInfo"></asp:Label>
                                        <asp:Button ID="btnFirst" runat="server" class="navigationButton" Text="<<" OnClick="btnFirstOnClick" />
                                        <asp:Button ID="btnPrevious" runat="server" class="navigationButton" Text="<" OnClick="btnPreviousOnClick" />
                                        <asp:Label ID="lblPageNumber" Style="font-weight: bold; margin-left: 5px; margin-right: 5px; text-align: center;"
                                            runat="server"></asp:Label>
                                        <asp:Button ID="btnNext" class="navigationButton" runat="server" Text=">" OnClick="btnNextOnClick" />
                                        <asp:Button ID="btnLast" class="navigationButton" runat="server" Text=">>" OnClick="btnLastOnClick" />
                                        <asp:DropDownList ID="drpPageSize" class="pageSize" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="drpPageSizeOnSelectedIndexChanged">
                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hdnFilter" runat="server" />
                                        <asp:HiddenField ID="hdnLastPage" runat="server" />
                                        <div class="head_second_div" style="display: none">
                                            <asp:HiddenField ID="hdnTotal" runat="server" Value="0" />
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnExportToExcel" />
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
        <asp:UpdatePanel ID="UpdPanelAdd" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="PanelAdd" Visible="false" runat="server">
                    <div class="popupBackground">
                    </div>
                    <div class="animated halfPopUp">
                        <asp:UpdatePanel ID="UpdPanelAddInner" runat="server" ChildrenAsTriggers="false"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="Adding_heading">
                                    Lead Followup
                                </div>
                                <table class="formTable">
                                    <tr style="display:none;">
                                        <td style="width: 33%">Contact Person Name
                                            <asp:TextBox ID="txtName" ReadOnly="true" CssClass="txt" runat="server"></asp:TextBox>
                                        </td>
                                        <td style="width: 33%">Contact Person designation
                                            <asp:TextBox ID="txtCPDesig" class="txt" runat="server"></asp:TextBox>
                                        </td>
                                        <td>Website 
                                            <asp:TextBox ID="txtwebsite" CssClass="txt" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 33%">Date <span style="color: Red">&nbsp*</span>
                                            <telerik:RadDatePicker ID="Currentdate" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                <Calendar runat="server" ID="Calendar1" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                    ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                    <SpecialDays>
                                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                        </telerik:RadCalendarDay>
                                                    </SpecialDays>
                                                </Calendar>
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="Currentdate"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 33%">Lead Name<span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txtcompany" CssClass="txt" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtcompany"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 33%">Contact Number
                                            <asp:TextBox ID="txtMobileNumber" ReadOnly="true" class="txt numbers_only" runat="server"></asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr>
                                        
                                        
                                        
                                    </tr>
                                    <tr>
                                        <td>Status <span style="color: Red">&nbsp*</span>
                                            <asp:UpdatePanel ID="updStatus" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <telerik:RadComboBox ID="drpStatus" Sort="Ascending" Filter="Contains" runat="server"
                                                        AutoPostBack="true" OnSelectedIndexChanged="drpStatusOnSelectedIndexChanged"
                                                        AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                        OnClientBlur="ValidateCombo" EmptyMessage="Search Status..." Style="overflow: hidden; width: 96%; border: none!important;">
                                                    </telerik:RadComboBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="drpStatus"
                                                        ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>Priority <span style="color: Red">&nbsp*</span>
                                            <asp:UpdatePanel ID="updPriority" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <telerik:RadComboBox ID="drpPriority" Sort="Ascending" Filter="Contains" runat="server"
                                                        AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                        AutoPostBack="true" OnSelectedIndexChanged="drpPriority_SelectedIndexChanged"
                                                        OnClientBlur="ValidateCombo" EmptyMessage="Search Priority..." Style="overflow: hidden; width: 96%; border: none!important;">
                                                    </telerik:RadComboBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drpPriority"
                                                        ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>Activity <span style="color: Red">&nbsp*</span>
                                            <%--<asp:TextBox ID="txtActivity" CssClass="txt" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtActivity"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>--%>
                                            <asp:UpdatePanel ID="updSegment" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                 <ContentTemplate>
                                                     <telerik:RadComboBox ID="drpSegment" Sort="Ascending" Filter="Contains" runat="server"
                                                         AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                         OnClientBlur="ValidateCombo" EmptyMessage="Search ..." AutoPostBack="true"
                                                         OnSelectedIndexChanged="drpSegment_SelectedIndexChanged"
                                                         Style="overflow: hidden; width: 96%; border: none!important;">
                                                     </telerik:RadComboBox>
                                                 </ContentTemplate>
                                             </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td colspan="2">Activity Description
                                            <br />
                                            <asp:TextBox ID="txtActivityDesc" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="UpdFollowupdate" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlNextFollw" runat="server">
                                                        Next Follow up Date <span style="color: Red">&nbsp*</span>
                                                        <telerik:RadDatePicker ID="Followupdate" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                            <Calendar runat="server" ID="Calendar2" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                                ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                                <SpecialDays>
                                                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                    </telerik:RadCalendarDay>
                                                                </SpecialDays>
                                                            </Calendar>
                                                        </telerik:RadDatePicker>
                                                        <asp:RequiredFieldValidator ID="ReqFollowupdate" runat="server" ControlToValidate="Followupdate"
                                                            ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                            InitialValue=""></asp:RequiredFieldValidator>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdFollowupTime" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlNextFollwTime" runat="server">
                                                        Time <span style="color: Red">&nbsp*</span>
                                                        <telerik:RadTimePicker ID="radFollowupTime" runat="server">
                                                        </telerik:RadTimePicker>
                                                        <asp:RequiredFieldValidator ID="ReqFollowuptime" runat="server" ControlToValidate="radFollowupTime"
                                                            ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                            InitialValue=""></asp:RequiredFieldValidator>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">Customer Response
                                            <br />
                                            <asp:TextBox ID="txtResponse" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">Remark <span style="color: Red">&nbsp*</span>
                                            <br />
                                            <asp:TextBox ID="txtremark" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtremark"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Button ID="btnanwser" class="butn_save" Style="float: right" OnClick="btnanwser_Click"
                                                runat="server" Text="Add Answer" />

                                            <asp:Button ID="Button1" class="butn_save" Style="float: right" OnClick="btnQn_Click"
                                                runat="server" Text="Add New Question" />
                                            <asp:UpdatePanel ID="UpdService" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table class="listTable">
                                                        <thead>
                                                            <tr>
                                                                <th style="width: 5%"></th>
                                                                <th style="width: 25%">Question
                                                                </th>
                                                                <th style="width: 25%">Answer
                                                                </th>
                                                                <th style="width: 5%">Action
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:Repeater ID="rptservice" runat="server" OnItemDataBound="rptserviceOnItemDataBound"
                                                                OnItemCommand="rptserviceOnItemCommand">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td>
                                                                            <%# Container.ItemIndex + 1 %>
                                                                        </td>
                                                                        <td>
                                                                            <asp:HiddenField ID="hdnDId" runat="server" Value='<%#Eval("Id") %>' />

                                                                            <telerik:RadComboBox ID="drpDepartment" Sort="Ascending" Filter="Contains" runat="server"
                                                                                AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search ..."
                                                                                OnSelectedIndexChanged="drpFilterOnSelectedIndexChanged" AutoPostBack="true"
                                                                                ClientIDMode="AutoID" Style="overflow: hidden; width: 91%; border: none!important;"
                                                                                OnClientFocus="OnClientKeyPressing" OnClientBlur="ValidateCombo">
                                                                            </telerik:RadComboBox>
                                                                            <asp:HiddenField ID="hdnDepartmentId" runat="server" Value='<%#Eval("DepartmentId") %>' />

                                                                        </td>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="UpdSerCategoryDropdown" runat="server" ChildrenAsTriggers="false"
                                                                                UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <telerik:RadComboBox ID="drpSerCategory" Sort="Ascending" Filter="Contains" runat="server"
                                                                                        AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search ..."
                                                                                        DropDownWidth="300px" ClientIDMode="AutoID" Style="overflow: hidden; width: 95%; border: none!important;"
                                                                                        OnClientFocus="OnClientKeyPressing" OnClientBlur="ValidateCombo">
                                                                                    </telerik:RadComboBox>
                                                                                    <asp:HiddenField ID="hdnSerCategoryId" runat="server" Value='<%#Eval("CategoryId") %>' />
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btn_serDetail_newEntry" runat="server" class="btn_add_new" ValidationGroup="save_serdetail"
                                                                                CommandName="Add" />
                                                                            <asp:Button ID="btn_remove_line" class="btn_delete" runat="server" ToolTip="Delete"
                                                                                CommandName="Delete" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');" />
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </tbody>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div>
                                                <asp:HiddenField ID="hdnId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnUserId" runat="server" />
                                                <asp:HiddenField ID="hdnPageId" Value="0" runat="server" />
                                                <asp:Button ID="btnSave" class="butn_save" ValidationGroup="save" OnClick="btnSaveOnClick"
                                                    runat="server" Text="Save" />
                                                <asp:Button ID="btnCreateQutn" class="butn_save" OnClick="btnCreateQutn_Click"
                                                    runat="server" Text="Create Quotation" />
                                                <asp:Button ID="btnHistory" class="butn_save" OnClick="btnhistrymainOnClick" runat="server"
                                                    Text="History" />
                                                <asp:Button ID="btnClose" class="butn" runat="server" Text="Close" OnClick="btnCloseOnClick" />
                                                <asp:HiddenField ID="hdnAdd" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnHistory" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnCreateQutn" runat="server" Value="0" />

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
                <asp:UpdatePanel ID="updHistoryMain" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnexcel_export" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Panel ID="pnlHistory" runat="server" Visible="false">
                            <div class="popupBackground">
                            </div>
                            <div class="animated halfPopUp" style="width: 80%">
                                <div class="Adding_heading">
                                    History
                                </div>
                                <table style="padding: 20px; width: 100%">
                                    <tr>
                                        <td colspan="4">
                                            <div style="float: right">
                                                <asp:Button ID="btnexcel_export" runat="server" class="btn_excel "
                                                    ToolTip="Export to Excel" OnClick="btn_excelhis_OnClick" />
                                                <span style="color: Red">&nbsp&nbsp</span>
                                                <asp:Button ID="Button2" runat="server" class="btn_print "
                                                    ToolTip="Export to pdf" OnClick="btn_pdfhis_OnClick" />
                                                <span style="color: Red">&nbsp&nbsp</span>
                                            </div>
                                            <div style="clear: both"></div>
                                            <asp:UpdatePanel ID="Upd_History" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div id="div_menu" runat="server" style="width: 100%; min-height: 300px; max-height: 300px; overflow: auto;">
                                                        <table class="listTable">
                                                            <thead>
                                                                <tr>
                                                                    <th style="text-align: center; width: 3%">Sl
                                                                    </th>
                                                                    <th style="text-align: center; width: 5%">Date
                                                                    </th>
                                                                    <th style="text-align: center; width: 15%">Customer Response
                                                                    </th>
                                                                    <th style="text-align: center; width: 13%">Remark
                                                                    </th>
                                                                    <th style="text-align: center; width: 7%">Status
                                                                    </th>
                                                                    <th style="text-align: center; width: 11%">Next Followup Date
                                                                    </th>
                                                                    <th style="text-align: center; width: 10%">Done By
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <asp:Repeater ID="rptHistory" runat="server">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td style="text-align: center;">
                                                                            <%#Eval("RowNum")%>
                                                                        </td>
                                                                        <td>
                                                                            <%#Eval("Date")%>
                                                                        </td>
                                                                        <td>
                                                                            <%#Eval("CustomerResponse")%>
                                                                        </td>
                                                                        <td>
                                                                            <%#Eval("Remark")%>
                                                                        </td>
                                                                        <td>
                                                                            <%#Eval("Status")%>
                                                                        </td>
                                                                        <td>
                                                                            <%#Eval("NextFollowupDate")%>
                                                                        </td>
                                                                        <td>
                                                                            <%#Eval("Employee")%>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                            <tr>
                                                                <td colspan="7" class="navigationRow">
                                                                    <asp:UpdatePanel ID="upd_his_nav" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:Label ID="lbl_page_info1" runat="server" class="pageInfo"></asp:Label>
                                                                            <asp:Button ID="Button7" runat="server" class="navigationButton" Text="<<" OnClick="btn_first1_OnClick" />
                                                                            <asp:Button ID="Button8" runat="server" class="navigationButton" Text="<" OnClick="btn_prev1_OnClick" />
                                                                            <asp:Label ID="lbl_page_number1" Style="font-weight: bold; margin-left: 5px; margin-right: 5px; text-align: center;"
                                                                                runat="server"></asp:Label>
                                                                            <asp:Button ID="Button9" class="navigationButton" runat="server" Text=">" OnClick="btn_next1_OnClick" />
                                                                            <asp:Button ID="Button10" class="navigationButton" runat="server" Text=">>" OnClick="btn_last1_OnClick" />
                                                                            <asp:DropDownList ID="drp_count1" class="pageSize" runat="server" AutoPostBack="true"
                                                                                OnSelectedIndexChanged="drp_count1_OnSelectedIndexChanged">
                                                                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
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
                                            <asp:Button ID="Button4" class="butn" runat="server" Text="Close" OnClick="btn_histry_Close_OnClick" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="updAnswer" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" style="height: 100%;">
                    <ContentTemplate>
                        <asp:Panel ID="pnlAnswer" Visible="false" runat="server">
                            <AmarCentre:Answer ID="UCAnswer" runat="server" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="updQuestion" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" style="height: 100%;">
                    <ContentTemplate>
                        <asp:Panel ID="pnlQuestion" Visible="false" runat="server">
                            <AmarCentre:Question ID="UCQuestion" runat="server" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="updPriorityPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" style="height: 100%;">
                    <ContentTemplate>
                        <asp:Panel ID="pnlPriority" Visible="false" runat="server">
                            <AmarCentre:Priority ID="UCPriority" runat="server" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="updSegmentPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" style="height: 100%;">
                    <ContentTemplate>
                        <asp:Panel ID="pnlSegment" Visible="false" runat="server">
                            <AmarCentre:Segment ID="UCSegment" runat="server" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="updStatusPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" style="height: 100%;">
                    <ContentTemplate>
                        <asp:Panel ID="pnlStatus" Visible="false" runat="server">
                            <AmarCentre:Status ID="UCStatus" runat="server" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
