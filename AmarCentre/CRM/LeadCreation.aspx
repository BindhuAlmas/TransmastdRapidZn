<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true"
    CodeBehind="LeadCreation.aspx.cs" Inherits="AmarCentre.CRM.LeadCreation" %>
<%@ Register Src="~/CRM/UserControl/UCLeadsource.ascx" TagName="Leadsource"
    TagPrefix="AmarCentre" %>
<%@ Register Src="~/CRM/UserControl/UCPriority.ascx" TagName="Priority"
    TagPrefix="AmarCentre" %>
<%@ Register Src="~/CRM/UserControl/UCJuris.ascx" TagName="Jurisdiction"
    TagPrefix="AmarCentre" %>
<%@ Register Src="~/CRM/UserControl/UCAnswer.ascx" TagName="Answer"
    TagPrefix="AmarCentre" %>
<%@ Register Src="~/CRM/UserControl/UCQuestion.ascx" TagName="Question"
    TagPrefix="AmarCentre" %>
<%@ Register Src="~/CRM/UserControl/UCCity.ascx" TagName="City"
    TagPrefix="AmarCentre" %>
<%@ Register Src="~/CRM/UserControl/UCSegment.ascx" TagName="Segment"
    TagPrefix="AmarCentre" %>

<%@ Register Src="~/Transactions/UserControl/UCMail.ascx" TagName="MailUC" TagPrefix="AmarCentre" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <link href="../Styles/telerikeditor.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/telerikeditor.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Lead Creation
        <asp:Button ID="btnNewEntry" runat="server"   class="btnAddNew" OnClick="btnNewEntryOnClick" />
        <asp:Button ID="btnExportToExcel" runat="server" class="btn_excel right_align_list"
            ToolTip="Export to Excel" OnClick="btnExportToExcelOnClick" />
         <asp:Button ID="btnupload" runat="server" Text="Upload Excel"
             style=" float: right;background-color:#2b66b1;margin-right:1%;
                cursor:pointer; color:white;border:none;height:20px" OnClick="btnupload_Click" />
        <div class="searchDiv">
            <asp:TextBox ID="txtSearch" runat="server" class="txt_search" AutoPostBack="true"
                OnTextChanged="txtSearchOnTextChanged" placeholder="Search"></asp:TextBox>
        </div>
        <telerik:RadComboBox ID="drpStatusfilter" Sort="Ascending" Filter="Contains" runat="server"
            AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
            OnClientBlur="ValidateCombo" EmptyMessage="Search Status..." AutoPostBack="true"
            OnSelectedIndexChanged="drpStatusfilterOnSelectedIndexChanged"
            Style="overflow: hidden; width: 16%; border: none!important; float: right; padding-right: 5px; margin-top: 0px">
        </telerik:RadComboBox>
        <telerik:RadComboBox ID="drpprorityfilter" Sort="Ascending" Filter="Contains" runat="server"
            AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
            AutoPostBack="true" OnSelectedIndexChanged="drpStatusfilterOnSelectedIndexChanged"
            OnClientBlur="ValidateCombo" EmptyMessage="Search Priority..." 
            Style="overflow: hidden;  width: 16%; border: none!important; float: right; padding-right: 5px; margin-top: 0px">
        </telerik:RadComboBox>
    </div>
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
                            <th style="width: 4%;">Sl No
                            </th>
                             <th style="width: 5%;">Code
                            </th>
                            <th style="width: 5%;">Date
                            </th>
                            <th style="width: 13%;">Lead Name
                            </th>
                            <th style="width: 10%;">Contact No
                            </th>
                            <th style="width: 8%;">Status
                            </th>
                              <th style="width: 8%;">Priority
                            </th>
                            <th style="width: 4%;">Action
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
                                     <td>
                                        <%#Eval("Code")%>
                                    </td>
                                    <td>
                                        <%#Eval("LeadDates")%>
                                    </td>
                                    <td>
                                        <%#Eval("CompanyName")%>
                                    </td>
                                    <td>
                                        <%#Eval("MobileNumber")%>
                                    </td>
                                    <td>
                                        <%#Eval("Statusname")%>
                                    </td>
                                      <td>
                                        <%#Eval("Priorityname")%>
                                    </td>
                                    <td class="listTableActionButtonDiv">
                                        <asp:Button ID="btnEdit" runat="server" commandname="Edit" class="btn_edit" />
                                          <asp:Button ID="btnprint" runat="server" ToolTip="AgreementPrint" commandname="Print" class="btn_print" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td colspan="10" class="navigationRow">
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
                                    Lead Creation
                                </div>
                                <table class="formTable">
                                    
                                     <tr>
                                        <td  style="width:33%">
                                            Code
                                            <asp:TextBox ID="lbl_Code" runat="server" class="txt read_Only" Font-Bold="true" Text=""></asp:TextBox>
                                        </td>
                                          <td style="width: 33%">Date <span style="color: Red">&nbsp*</span>
                                             <telerik:RadDatePicker ID="leadDate" runat="server" class="input-boder" DateInput-DateFormat="dd/MM/yyyy">
                                        <Calendar runat="server" ID="Calendar3" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                            ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                            <SpecialDays>
                                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                </telerik:RadCalendarDay>
                                            </SpecialDays>
                                        </Calendar>
                                    </telerik:RadDatePicker>
                                        </td>
                                           
                                        <td style="width: 33%">Lead Name <span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txtcompany" CssClass="txt" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtcompany"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>

                                    <tr>
                                         <td style="width: 33%">Contact Number <span style="color: Red">&nbsp*</span>
                                             <br />
                                               <asp:TextBox ID="txtCountryCodeCN" runat="server" Style="float: left; margin-right: 5%" Width="20%" class="txt"></asp:TextBox>
                                            <asp:TextBox ID="txtMobileNumber" class="txt numbers_only" Style="float: left" Width="67%" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtMobileNumber"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        
                                         <td>Email 
                                            <asp:TextBox ID="txtEmailId" CssClass="txt" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Please Enter Valid Email ID"
                                                ValidationGroup="save" ControlToValidate="txtEmailId" Style="color: Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                          
                                        </td>
                                        <td>Lead Source
                                             <asp:UpdatePanel ID="updSource" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                 <ContentTemplate>
                                                     <telerik:RadComboBox ID="drpSource" Sort="Ascending" Filter="Contains" runat="server"
                                                         AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                         OnClientBlur="ValidateCombo" EmptyMessage="Search Lead Source..." AutoPostBack="true"
                                                         OnSelectedIndexChanged="drpSource_SelectedIndexChanged"
                                                         Style="overflow: hidden; width: 96%; border: none!important;">
                                                     </telerik:RadComboBox>
                                                 </ContentTemplate>
                                             </asp:UpdatePanel>
                                        </td>
                            </tr>
                                    <tr>
                                         <td>Assigned Employee <span style="color: Red">&nbsp*</span>
                                            <telerik:RadComboBox ID="drpEmployee" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                OnClientBlur="ValidateCombo" EmptyMessage="Search Employee..." Style="overflow: hidden; width: 96%; border: none!important;">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="drpEmployee"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        <td>Priority <span style="color: Red">&nbsp*</span>
                                             <asp:UpdatePanel ID="updPriority" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                 <ContentTemplate>
                                            <telerik:RadComboBox ID="drpPriority" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                AutoPostBack="true" OnSelectedIndexChanged="drpPriority_SelectedIndexChanged"
                                                OnClientBlur="ValidateCombo" EmptyMessage="Search Priority..." Style="overflow: hidden; width: 96%; border: none!important;">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="drpPriority"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                                     </ContentTemplate>
                                             </asp:UpdatePanel>
                                        </td>
                                         <td>Activity <span style="color: Red">&nbsp*</span>
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
                                        <td style="display:none">
                                           Jurisdiction
                                             <asp:UpdatePanel ID="updJurisdiction" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                 <ContentTemplate>
                                                     <telerik:RadComboBox ID="drpJurisdiction" Sort="Ascending" Filter="Contains" runat="server"
                                                         AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                         OnClientBlur="ValidateCombo" EmptyMessage="Search ..." AutoPostBack="true"
                                                         OnSelectedIndexChanged="drpJurisdiction_SelectedIndexChanged"
                                                         Style="overflow: hidden; width: 96%; border: none!important;">
                                                     </telerik:RadComboBox>
                                                 </ContentTemplate>
                                             </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td colspan="2">Activity Description
                                            <br />
                                            <asp:TextBox ID="txtAddress" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <%-- ==================== NEW FIELDS START ==================== --%>
                                    <tr>
                                        <td style="width: 33%">Lead Brand
                                            <asp:TextBox ID="txtLeadBrand" CssClass="txt" runat="server"></asp:TextBox>
                                        </td>
                                        <td style="width: 33%">Passport No
                                            <asp:TextBox ID="txtPassportNo" CssClass="txt" runat="server"></asp:TextBox>
                                        </td>
                                        <td style="width: 33%">Passport Issue Date
                                            <telerik:RadDatePicker ID="dpPassportIssueDate" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                <Calendar runat="server" ID="CalPassportIssue" CssClass="rtlSupport" ShowOtherMonthsDays="False"
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
                                        <td style="width: 33%">Passport Expiry Date
                                            <telerik:RadDatePicker ID="dpPassportExpiryDate" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                <Calendar runat="server" ID="CalPassportExpiry" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                    ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                    <SpecialDays>
                                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                        </telerik:RadCalendarDay>
                                                    </SpecialDays>
                                                </Calendar>
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td style="width: 33%">Current Status
                                             <telerik:RadComboBox ID="drpCurrentStatus" Sort="Ascending" Filter="Contains" runat="server"
                                                 AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                 OnClientBlur="ValidateCombo" EmptyMessage="Search ..."
                                                 Style="overflow: hidden; width: 96%; border: none!important;">
                                                 <Items>
                                                     <telerik:RadComboBoxItem Value="1" Text="Inside Country" />
                                                     <telerik:RadComboBoxItem Value="2" Text="Outside Country" />
                                                 </Items>
                                             </telerik:RadComboBox>
                                            
                                        </td>
                                        <td style="width: 33%">Date of Birth
                                            <telerik:RadDatePicker ID="dpDOB" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                <Calendar runat="server" ID="CalDOB" CssClass="rtlSupport" ShowOtherMonthsDays="False"
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
                                        <td style="width: 33%">Nationality
                                            <asp:TextBox ID="txtNationality" CssClass="txt" runat="server"></asp:TextBox>
                                        </td>
                                        <td style="width: 33%">Marital Status
                                            <telerik:RadComboBox ID="drpMaritalStatus" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                OnClientBlur="ValidateCombo" EmptyMessage="Search ..."
                                                Style="overflow: hidden; width: 96%; border: none!important;">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="1" Text="Married" />
                                                    <telerik:RadComboBoxItem Value="2" Text="UnMarried" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                        <td style="width: 33%">Mother Name
                                            <asp:TextBox ID="txtMotherName" CssClass="txt" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <%-- ==================== NEW FIELDS END ==================== --%>

                                    <tr>
                                         <td style="display:none">Approx. Closing Date <span style="color: Red">&nbsp*</span>
                                            <telerik:RadDatePicker ID="ApprxClosingDate" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                <Calendar runat="server" ID="Calendar1" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                    ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                    <SpecialDays>
                                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                        </telerik:RadCalendarDay>
                                                    </SpecialDays>
                                                </Calendar>
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td>Next Follow up Date <span style="color: Red">&nbsp*</span>
                                            <telerik:RadDatePicker ID="Followupdate" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                <Calendar runat="server" ID="Calendar2" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                    ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                    <SpecialDays>
                                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                        </telerik:RadCalendarDay>
                                                    </SpecialDays>
                                                </Calendar>
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="Followupdate"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        <td>Time <span style="color: Red">&nbsp*</span>
                                            <telerik:RadTimePicker ID="radFollowupTime" runat="server">
                                            </telerik:RadTimePicker>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="radFollowupTime"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td colspan="3">Customer Response
                                            <br />
                                            <asp:TextBox ID="txtResponse" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:UpdatePanel ID="updDocumentList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="rptDocs" />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <table class="listTable" style="width: 65%">
                                                        <thead>
                                                            <tr style="text-align: center">
                                                                <th style="width: 30%">Document Type</th>
                                                                <th style="width: 50%">File</th>
                                                                <th style="width: 20%">Action</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:Repeater ID="rptDocs" runat="server"
                                                                OnItemCommand="rptDocs_ItemCommand">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblDocumentType" runat="server"
                                                                                Text='<%# Eval("DocumentName") %>'>
                                                                            </asp:Label>
                                                                            <asp:HiddenField ID="hdnDocId" runat="server"
                                                                                Value='<%# Eval("Id") %>' />
                                                                            <asp:HiddenField ID="hdnDocumentTypeId" runat="server"
                                                                                Value='<%# Eval("DocumentId") %>' />
                                                                        </td>
                                                                        <td>
                                                                            <asp:LinkButton ID="lblfileupl" runat="server"
                                                                                Text='<%# Eval("FileNames") %>'
                                                                                CommandName="Download"
                                                                                ForeColor="Blue"
                                                                                Style="text-decoration: underline; font-size: 12px;">
                                                                            </asp:LinkButton>
                                                                            <asp:HiddenField ID="hdnfilesaveupl" runat="server"
                                                                                Value='<%# Eval("FilenameSave") %>' />
                                                                        </td>
                                                                        <td style="text-align: center">
                                                                            <asp:Button ID="btn_delete" runat="server"
                                                                                CssClass="btn_delete"
                                                                                ToolTip="Delete"
                                                                                CommandName="Delete"
                                                                                CausesValidation="false"
                                                                                OnClientClick="return confirm('Are you sure you want to delete this document?');" />
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                            <tr>
                                                                <td>
                                                                    <telerik:RadComboBox ID="drpDocumentType"
                                                                        Sort="Ascending" Filter="Contains" runat="server"
                                                                        AllowCustomText="true" RenderMode="Lightweight"
                                                                        OnClientFocus="OnClientKeyPressing"
                                                                        OnClientBlur="ValidateCombo"
                                                                        EmptyMessage="Search Document Type..."
                                                                        Style="overflow: hidden; width: 96%; border: none!important;">
                                                                    </telerik:RadComboBox>
                                                                </td>
                                                                <td>
                                                                    <telerik:RadAsyncUpload ID="fu_Files" runat="server"
                                                                        Width="80%" MaxFileSize="500000000"
                                                                        OnFileUploaded="fu_FilesOnFileUploaded"
                                                                        MultipleFileSelection="Disabled">
                                                                    </telerik:RadAsyncUpload>
                                                                    <asp:UpdatePanel ID="Updfu_Files" runat="server"
                                                                        ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:HiddenField ID="hdnfilenameup" runat="server" />
                                                                            <asp:HiddenField ID="hdnfilenamesaveup" runat="server" />
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td style="text-align: center">
                                                                    <asp:Button ID="btnAddDocument" runat="server"
                                                                        CssClass="btnAdd"
                                                                        Text="+"
                                                                        ToolTip="Add Document"
                                                                        CausesValidation="false"
                                                                        OnClick="btnAddDocument_Click" />
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr >
                                        <td colspan="3">
                                             <asp:Button ID="btnanwser" class="butn_save" style="float:right"  OnClick="btnanwser_Click"
                                                    runat="server" Text="Add New Answer" />
                                              <asp:Button ID="Button1" class="butn_save" style="float:right"  OnClick="btnQn_Click"
                                                    runat="server" Text="Add New Question" />
                                            
                                            <asp:UpdatePanel ID="UpdService" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table class="listTable">
                                                        <thead>
                                                            <tr>
                                                             <th style="width:5%"></th>
                                                                <th style="width: 25%">Question
                                                                </th>
                                                                <th style="width: 25%">Answer
                                                                </th>
                                                                <th style="width:5%">
                                                                    Action
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:Repeater ID="rptservice" runat="server" OnItemDataBound="rptserviceOnItemDataBound"
                                                                OnItemCommand="rptserviceOnItemCommand">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                          <td >
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
                                                                        <td >
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
                                        <td colspan="3">
                                            <div>
                                                <asp:HiddenField ID="hdnId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnStatus" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnUserId" runat="server" />
                                                <asp:Button ID="btnSave" class="butn_save" ValidationGroup="save" OnClick="btnSaveOnClick"
                                                    runat="server" Text="Save" />
                                                <asp:Button ID="btnCreateQutn" class="butn_save" ValidationGroup="save" OnClick="btnCreateQutn_Click"
                                                    runat="server" Text="Create Quotation" />
                                                <asp:Button ID="btnMail" class="butn_save" ValidationGroup="save" OnClick="btnMailOnClick"
                                                    runat="server" Text="Send Mail" />
                                                 <asp:Button ID="btnAgreementPrint" class="butn_save"   OnClick="btnAgreementPrint_Click"
     runat="server" Text="Send Mail" />
                                                <asp:Button ID="btnDelete" class="butn_delete" runat="server" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');"
                                                    Visible="false" Text="Delete" OnClick="btnDeleteOnClick" />
                                                <asp:Button ID="btnHistory" class="butn_save" OnClick="btnhistrymainOnClick" runat="server"
                                                    Text="History" />
                                                <asp:Button ID="btnReset" class="butn" runat="server" Text="Reset" OnClick="btnResetOnClick" />
                                                <asp:Button ID="btnClose" class="butn" runat="server" Text="Close" OnClick="btnCloseOnClick" />
                                                <asp:HiddenField ID="hdnAdd" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnUpdate" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnDelete" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnHistory" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnSendMail" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnCreateQutn" runat="server" Value="0" />
                                            </div>
                                        </td>
                                    </tr>
                              
                                <tr style="display:none;">
    <td>
        <td style="width: 33%">Campaign <span style="color: Red">&nbsp*</span>
        <asp:TextBox ID="txtCampaign" CssClass="txt" runat="server"></asp:TextBox>
    </td>
        <td style="width: 33%">Contact Person Name <span style="color: Red">&nbsp*</span>
        <asp:TextBox ID="txtName" CssClass="txt" runat="server"></asp:TextBox>
    </td>
   
    <td style="width:33%">Contact Person designation
        <asp:TextBox ID="txtCPDesig" class="txt" runat="server"></asp:TextBox>
    </td>
        <td style="width: 33%">Land Phone No <br />
        <asp:TextBox ID="txtCountryCodeLPN" runat="server" Style="float: left; margin-right: 5%" Width="20%" class="txt"></asp:TextBox>
        <asp:TextBox ID="txtphone" class="txt numbers_only"  Style="float: left" Width="67%" runat="server"></asp:TextBox>
    </td>
        <td>Website 
        <asp:TextBox ID="txtwebsite" CssClass="txt" runat="server"></asp:TextBox>
    </td>
        <td >
       City
         <asp:UpdatePanel ID="updCity" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
             <ContentTemplate>
                 <telerik:RadComboBox ID="drpCity" Sort="Ascending" Filter="Contains" runat="server"
                     AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                     OnClientBlur="ValidateCombo" EmptyMessage="Search ..." AutoPostBack="true"
                     OnSelectedIndexChanged="drpcity_SelectedIndexChanged"
                     Style="overflow: hidden; width: 96%; border: none!important;">
                 </telerik:RadComboBox>
             </ContentTemplate>
         </asp:UpdatePanel>
    </td> 
        <td>
        Segment
         <asp:TextBox ID="txtActivity" CssClass="txt" runat="server"></asp:TextBox>
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
                                            <div style="float:right">
                                            <asp:Button ID="btnexcel_export" runat="server" class="btn_excel "
                                                ToolTip="Export to Excel" OnClick="btn_excelhis_OnClick" />
                                                <span style="color: Red">&nbsp&nbsp</span>
                                            <asp:Button ID="Button2" runat="server" class="btn_print "
                                                ToolTip="Export to pdf" OnClick="btn_pdfhis_OnClick" />
                                                <span style="color: Red">&nbsp&nbsp</span>
                                                </div>
                                            <div style="clear:both"></div>
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

                  <asp:UpdatePanel ID="UpdMailPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" style="height: 100%;">
            <ContentTemplate>
                <asp:Panel ID="pnlMail" Visible="false" runat="server">
                    <AmarCentre:MailUC ID="EmailUC" runat="server" />
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

                <asp:UpdatePanel ID="updSourcePanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" style="height: 100%;">
                    <ContentTemplate>
                        <asp:Panel ID="pnlsource" Visible="false" runat="server">
                            <AmarCentre:Leadsource ID="UCLeadsource" runat="server" />
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
                <asp:UpdatePanel ID="updJurisdictionPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" style="height: 100%;">
                    <ContentTemplate>
                        <asp:Panel ID="pnlJurisdiction" Visible="false" runat="server">
                            <AmarCentre:Jurisdiction ID="UCJurisdiction" runat="server" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                 <asp:UpdatePanel ID="updCityPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" style="height: 100%;">
                    <ContentTemplate>
                        <asp:Panel ID="pnlCity" Visible="false" runat="server">
                            <AmarCentre:City ID="UCCity" runat="server" />
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


                <asp:Panel ID="pnlupload" Visible="false" runat="server">
                    <div class="popupBackground">
                    </div>
                    <div class="animated largePopUp">
                        <asp:UpdatePanel ID="upduploadexcel" runat="server" ChildrenAsTriggers="false"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="Adding_heading">
                                    Upload Excel
                                </div>
                                <table class="formTable">
                                     <tr>
                                        <td>
                                            <asp:UpdatePanel ID="updleadFile" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                   
                                                    <telerik:RadAsyncUpload ID="fu_DocUpload" MaxFileSize="500000000" runat="server"
                                                        MaxFileInputsCount="1" OnFileUploaded="fu_DocUpload_OnFileUploaded">
                                                    </telerik:RadAsyncUpload>
                                                   <asp:Button ID="btnleadUpload" class="butn" runat="server" OnClick="btnleadUpload_Click" Text="Upload File" />
                                                    <asp:Button ID="Btndwnformat" class="butn_save" OnClick="Btndwnformat_Click"
                                                    runat="server" Text="Download Excel Format" />
                                                    <asp:HiddenField ID="hdnleadFile" runat="server" />
                                                    <asp:HiddenField ID="hdnleadfileExtension" runat="server" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="Btndwnformat" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                   <td>     
                                         <asp:UpdatePanel ID="updleadFileList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                    <table class="listTable">
                                        <thead>
                                            <tr>
                                                 
                                                 <th>Sl</th>
                                                <th style="color:red">Date</th>
                                                <%--<th style="color:red">Campaign</th>--%>
                                                <th style="color:red">LeadName</th>
                                                <%--<th style="color:red">ContactPersonName</th>--%>
                                                <%--<th style="color:red">Segment</th>--%>
                                                <th style="color:red">Platform (Lead Source)</th>
                                                <%--<th style="color:red">City</th>--%>
                                                <th style="color:red">ContactNumber</th>
                                                <%--<th>ContactPersondesignation</th>--%>
                                                <%--<th>LandPhoneNo</th>--%>
                                                <th>Email</th>
                                                <%--<th>Website</th>--%>
                                                <%--<th>AssignedEmployee</th>--%>
                                                <%--<th>Priority</th>--%>
                                                <%--<th>Activity</th>--%>
                                                <%--<th>ActivityDescription</th>--%>
                                                <%--<th>CustomerResponse</th>--%>
                                                <th>Lead Brand</th>
                                                <th>Scope</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptuploaddetail" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><%# Container.ItemIndex + 1 %></td>
                                                        <td><asp:Label ID="lbldate" runat="server" Text='<%#Eval("Date")%>'></asp:Label></td>
                                                        <%--<td><asp:Label ID="lblCampaign" runat="server" Text='<%#Eval("Campaign")%>'></asp:Label></td>--%>
                                                        <td><asp:Label ID="lblCompanyName" runat="server" Text='<%#Eval("CompanyName")%>'></asp:Label></td>
                                                        <%--<td><asp:Label ID="lblContactPersonName" runat="server" Text='<%#Eval("ContactPersonName")%>'></asp:Label></td>--%>
                                                        <%--<td><%#Eval("Segment")%><asp:HiddenField ID="hdnSegmentId" runat="server" Value='<%#Eval("SegmentId")%>' /></td>--%>
                                                        <td>
                                                            <%#Eval("LeadSource")%>
                                                            <asp:HiddenField ID="hdnLeadSourceId" runat="server" Value='<%#Eval("LeadSourceId")%>' />
                                                        </td>
                                                        <%--<td><%#Eval("City")%><asp:HiddenField ID="hdnCityId" runat="server" Value='<%#Eval("CityId")%>' /></td>--%>
                                                        <td>
                                                            <%#Eval("CountryCodeContactNumber")%> <%#Eval("ContactNumber")%>
                                                            <asp:Label ID="lblCountryCodeContactNumber" Visible="false" runat="server" Text='<%#Eval("CountryCodeContactNumber")%>'></asp:Label>
                                                            <asp:Label ID="lblContactNumber" runat="server" Visible="false" Text='<%#Eval("ContactNumber")%>'></asp:Label>
                                                        </td>
                                                        <%--<td><asp:Label ID="lblContactPersondesignation" runat="server" Text='<%#Eval("ContactPersondesignation")%>'></asp:Label></td>--%>
                                                        <%--<td><%#Eval("LandPhoneNoCountryCode")%> <%#Eval("LandPhoneNo")%></td>--%>
                                                        <td><asp:Label ID="lblEmail" runat="server" Text='<%#Eval("Email")%>'></asp:Label></td>
                                                        <%--<td><asp:Label ID="lblWebsite" runat="server" Text='<%#Eval("Website")%>'></asp:Label></td>--%>
                                                        <%--<td><%#Eval("AssignedEmployee")%><asp:HiddenField ID="hdnAssignedEmployeeId" runat="server" Value='<%#Eval("AssignedEmployeeId")%>' /></td>--%>
                                                        <%--<td><%#Eval("Priority")%><asp:HiddenField ID="hdnPriorityId" runat="server" Value='<%#Eval("PriorityId")%>' /></td>--%>
                                                        <%--<td><asp:Label ID="lblActivity" runat="server" Text='<%#Eval("Activity")%>'></asp:Label></td>--%>
                                                        <%--<td><asp:Label ID="lblActivityDescription" runat="server" Text='<%#Eval("ActivityDescription")%>'></asp:Label></td>--%>
                                                        <%--<td><asp:Label ID="lblCustomerResponse" runat="server" Text='<%#Eval("CustomerResponse")%>'></asp:Label></td>--%>
                                                        <%-- Change LeadBrand eval from "LeadBrand" to "Website" --%>
                                                        <td><asp:Label ID="lblLeadBrand" runat="server" Text='<%#Eval("Website")%>'></asp:Label></td>

                                                        <%-- Change Scope eval from "Scope" to "Segment" --%>
                                                        <td><%#Eval("Segment")%><asp:HiddenField ID="hdnSegmentId" runat="server" Value='<%#Eval("SegmentId")%>' /></td>
    
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                     <asp:Button ID="btnverfy" class="butn_save" OnClick="btnverfy_Click"
                                                    runat="server" Text="Verified and Save" />
                                     <asp:Button ID="btnverfyclose" class="butn_save" OnClick="btnverfyclose_Click"
                                                    runat="server" Text="Close" />
                                    </ContentTemplate>
                               </asp:UpdatePanel>
                            </td> 
                        </tr>
                    </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </asp:Panel>

                </ContentTemplate>
        </asp:UpdatePanel>
        
    </div>
</asp:Content>
