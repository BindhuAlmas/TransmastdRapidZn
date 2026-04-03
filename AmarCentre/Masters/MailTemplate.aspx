<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="MailTemplate.aspx.cs" Inherits="AmarCentre.Masters.MailTemplate" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <link href="../Styles/telerikeditor.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/telerikeditor.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="HeadIng_Div">
      Mail Template/قالب البريد
        <asp:Button ID="btnNewEntry" runat="server"   class="btnAddNew" OnClick="btnNewEntryOnClick" />
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
                            <th style="width: 5%;">
                                Sl No/رقم
                            </th>
                            <th style="width: 15%;">
                                Name/اسم
                            </th>
                            <th style="width: 15%;">
                                Subject/المادة 
                            </th>
                            <th style="width: 5%;">
                                Action/عمل
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
                                        <%#Eval("Name")%>
                                    </td>
                                    <td>
                                        <%#Eval("Subject")%>
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
                    <div class="animated halfPopUp" style="width: 70%" >
                        <asp:UpdatePanel ID="UpdPanelAddInner" runat="server" ChildrenAsTriggers="false"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="Adding_heading">
                                    Template/قالب البريد
                                </div>
                                <table class="formTable">
                                   
                                    <tr>
                                        <td>
                                            Name/اسم <span style="color: Red">&nbsp*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtName" CssClass="txt" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtName"                                          ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                      <tr>
                                        <td>
                                            Subject/المادة  <span style="color: Red">&nbsp*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSubject" CssClass="txt" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSubject"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Description/وصف
                                            <br />
                                        </td>
                                        <td>
                                           <div class="demo-containers">
                                                <div class="demo-container size-medium">
                                                    <telerik:RadEditor rendermode="Lightweight" runat="server" ID="RadEditor3" Height="340px"
                                                     Skin="Hay" Width="600px" OnClientCommandExecuting="TelerikDemo.OnClientCommandExecuting">
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
                                                <asp:HiddenField ID="hdnIsDeleteAllow" runat="server" />

                                                <asp:Button ID="btnSave" class="butn_save" ValidationGroup="save" OnClick="btnSaveOnClick"
                                                    runat="server" Text="Save/حفظ" />
                                                <asp:Button ID="btnDelete" class="butn_delete" runat="server" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');"
                                                    Visible="false" Text="Delete/حذف" OnClick="btnDeleteOnClick" />
                                                <asp:Button ID="btnReset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btnResetOnClick" />
                                                <asp:Button ID="btnClose" class="butn" runat="server" Text="Close/أغلق" OnClick="btnCloseOnClick" />
                                                <asp:HiddenField ID="hdnAdd" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnUpdate" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnDelete" runat="server" Value="0" />
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
