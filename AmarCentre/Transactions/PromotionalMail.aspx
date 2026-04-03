<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true"
    CodeBehind="PromotionalMail.aspx.cs" Inherits="AmarCentre.Transaction.PromotionalMail" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <link href="../Styles/telerikeditor.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/telerikeditor.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Promotional Mails
        <asp:Button ID="btnNewEntry" runat="server" Text="+" class="btnAddNew" OnClick="btnNewEntryOnClick" />
        <asp:Button ID="btnExportToExcel" runat="server" class="btn_excel right_align_list"
            ToolTip="Export to Excel" OnClick="btnExportToExcelOnClick" />
        <div class="searchDiv">
            <asp:TextBox ID="txtSearch" runat="server" class="txt_search" AutoPostBack="true"
                OnTextChanged="txtSearchOnTextChanged" placeholder="Search"></asp:TextBox>
        </div>
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
                            <th style="width: 4%;">
                                Sl No
                            </th>
                            <th style="width: 10%;">
                                Date
                            </th>
                            <th style="width: 15%;">
                                Subject
                            </th>
                            <th style="width: 4%;">
                                Action
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
                                        <%#Eval("Date")%>
                                    </td>
                                    <td>
                                        <%#Eval("MailSubject")%>
                                    </td>
                                    <td class="listTableActionButtonDiv">
                                        <asp:Button ID="btnEdit" runat="server" class="btn_edit" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td colspan="4" class="navigationRow">
                                <asp:UpdatePanel ID="UpdPanelNavigation" runat="server" ChildrenAsTriggers="false"
                                    UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblPageInfo" runat="server" class="pageInfo"></asp:Label>
                                        <asp:Button ID="btnFirst" runat="server" class="navigationButton" Text="<<" OnClick="btnFirstOnClick" />
                                        <asp:Button ID="btnPrevious" runat="server" class="navigationButton" Text="<" OnClick="btnPreviousOnClick" />
                                        <asp:Label ID="lblPageNumber" Style="font-weight: bold; margin-left: 5px; margin-right: 5px;
                                            text-align: center;" runat="server"></asp:Label>
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
                    <div class="animated halfPopUp" >
                        <asp:UpdatePanel ID="UpdPanelAddInner" runat="server" ChildrenAsTriggers="false"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="Adding_heading">
                                    Send Mail
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td style="width: 47%">
                                            Date <span style="color: Red">&nbsp*</span>
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
                                        <td style="width: 47%">
                                            Mail Template
                                            <telerik:RadComboBox ID="drpTemplate" Sort="Ascending" Filter="Contains" runat="server"
                                                AutoPostBack="true" OnSelectedIndexChanged="drpTemplateOnSelectedIndexChanged"
                                                AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                OnClientBlur="ValidateCombo" EmptyMessage="Search Template..." Style="overflow: hidden;
                                                width: 96%; border: none!important;">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="display:none">
                                            Receiver Type <span style="color: Red">&nbsp*</span>
                                            <telerik:RadComboBox ID="drpType" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                              CheckBoxes="true"  AutoPostBack="true" OnSelectedIndexChanged="drpTypeOnSelectedIndexChanged" 
                                                EmptyMessage="Search Type..." Style="overflow: hidden; width: 96%; border: none!important;">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="1" Text="Customer" />
                                                    <telerik:RadComboBoxItem Value="2" Text="Lead" />
                                                </Items>
                                            </telerik:RadComboBox>
                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="drpType"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="updName" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    Name <span style="color: Red">&nbsp*</span>
                                                    <telerik:RadComboBox ID="drpName" Sort="Ascending" Filter="Contains" runat="server"
                                                        AllowCustomText="false" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                        CheckBoxes="true" EnableCheckAllItemsCheckBox="true" EmptyMessage="Search Name..."
                                                        Style="overflow: hidden; width: 96%; border: none!important;">
                                                    </telerik:RadComboBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="drpName"
                                                        ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                        InitialValue=""></asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            Subject <span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txtSubject" CssClass="txt" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSubject"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            Add Attachment
                                            <telerik:RadAsyncUpload ID="fuAttachImg" MaxFileSize="500000000" runat="server" MaxFileInputsCount="1"
                                                OnFileUploaded="fuAttachImgOnFileUploaded">
                                            </telerik:RadAsyncUpload>
                                              <asp:UpdatePanel ID="Upd_fuAttachImg" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                            <asp:HiddenField ID="hdn_AttachImgSaveAs" runat="server" Value="" />
                                            </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            Content
                                            <br />
                                            <div class="demo-containers">
                                                <div class="demo-container size-medium">
                                                    <telerik:RadEditor rendermode="Lightweight" runat="server" ID="RadEditor3" Height="350px" 
                                                        Skin="Hay" Width="90%" OnClientCommandExecuting="TelerikDemo.OnClientCommandExecuting">
                                                        <%-- <Tools>
                                                     <telerik:EditorToolGroup>
                                                     <telerik:EditorTool Name="" />
                                                     </telerik:EditorToolGroup>
                                                      </Tools>--%>
                                                        <Content>
                                                        </Content>
                                                    </telerik:RadEditor>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div>
                                                <asp:HiddenField ID="hdnId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnUserId" runat="server" />
                                                <asp:Button ID="btnSave" class="butn_save" ValidationGroup="save" OnClick="btnSaveOnClick"
                                                    runat="server" Text="Save" />
                                                <asp:Button ID="btnReset" class="butn" runat="server" Text="Reset" OnClick="btnResetOnClick" />
                                                <asp:Button ID="btnClose" class="butn" runat="server" Text="Close" OnClick="btnCloseOnClick" />
                                                <asp:HiddenField ID="hdnAdd" runat="server" Value="0" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
