<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="LeadTransfer.aspx.cs" Inherits="AmarCentre.CRM.LeadTransfer" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Lead Transfer
        <asp:Button ID="btnExportToExcel" runat="server" class="btn_excel right_align_list"
            ToolTip="Export to Excel" OnClick="btnExportToExcelOnClick" />
        <asp:Button ID="btnBulktransfer" runat="server" Text="Bulk Transfer"
            Style="float: right; background-color: #2b66b1; margin-right: 1%; cursor: pointer; color: white; border: none; height: 20px"
            OnClick="btnBulktransfer_Click" />
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
                            <th style="width: 5%;">Sl No
                            </th>
                            <%--<th style="width: 12%;">Contact Person
                            </th>--%>
                            <th style="width: 10%;">Contact No
                            </th>
                            <th style="width: 8%;">Assigned Employee
                            </th>
                            <th style="width: 5%;">Action
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
                                        <%#Eval("MobileNumber")%>
                                    </td>

                                    <td>
                                        <%#Eval("Employee")%>
                                    </td>
                                    <td class="listTableActionButtonDiv">
                                        <asp:Button ID="btnEdit" runat="server" class="btn_edit" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td colspan="5" class="navigationRow">
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
                    <div class="animated smallPopUp">
                        <asp:UpdatePanel ID="UpdPanelAddInner" runat="server" ChildrenAsTriggers="false"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="Adding_heading">
                                    Lead Transfer
                                </div>
                                <table class="formTable">
                                    <tr style="display:none;">
                                        <td>Contact Person
                                            <asp:TextBox ID="txtName" ReadOnly="true" CssClass="txt" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Contact Number 
                                            <asp:TextBox ID="txtMobileNumber" ReadOnly="true" class="txt numbers_only" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Assigned Employee
                                            <asp:TextBox ID="txtEmployee" ReadOnly="true" CssClass="txt" runat="server"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Reassign to <span style="color: Red">&nbsp*</span>
                                            <telerik:RadComboBox ID="drpEmployeeTransfer" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                OnClientBlur="ValidateCombo" EmptyMessage="Search Employee..." Style="overflow: hidden; width: 96%; border: none!important;">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="drpEmployeeTransfer"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Remark <span style="color: Red">&nbsp*</span>
                                            <br />
                                            <asp:TextBox ID="txtRemark" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtRemark"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:HiddenField ID="hdnId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnUserId" runat="server" />
                                                <asp:Button ID="btnSave" class="butn_save" ValidationGroup="save" OnClick="btnSaveOnClick"
                                                    runat="server" Text="Save" />
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


                <asp:Panel ID="pnlbulk" Visible="false" runat="server">
                    <div class="popupBackground">
                    </div>
                    <div class="animated smallPopUp">
                        <asp:UpdatePanel ID="updbulk" runat="server" ChildrenAsTriggers="false"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="Adding_heading">
                                    Bulk Lead Transfer
                                </div>
                                <table class="formTable">

                                    <tr>
                                        <td>Employee <span style="color: Red">&nbsp*</span>
                                            <telerik:RadComboBox ID="drpEmployee" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                AutoPostBack="true" OnSelectedIndexChanged="drpEmployee_SelectedIndexChanged"
                                                OnClientBlur="ValidateCombo" EmptyMessage="Search Employee..." Style="overflow: hidden; width: 96%; border: none!important;">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="drpEmployee"
                                                ValidationGroup="savebulk" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Reassign to <span style="color: Red">&nbsp*</span>
                                            <telerik:RadComboBox ID="drpEmployeeTransferbulk" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                OnClientBlur="ValidateCombo" EmptyMessage="Search Employee..." Style="overflow: hidden; width: 96%; border: none!important;">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="drpEmployeeTransferbulk"
                                                ValidationGroup="savebulk" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="updbulklist" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table class="listTable">
                                                        <thead>
                                                            <tr>
                                                                <th style="width: 5%;"> <asp:CheckBox ID="chkselall" AutoPostBack="true" OnCheckedChanged="chkselall_CheckedChanged" runat="server" />
                                                                </th>
                                                                <%--<th style="width: 20%;">Contact Person
                                                                </th>--%>
                                                                <th style="width: 10%;">Contact No
                                                                </th>

                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:Repeater ID="rptbulklist" runat="server">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox ID="chksel" runat="server" />
                                                                            <asp:HiddenField ID="hdnbulkId" runat="server" Value='<%#Eval("Id")%>' />
                                                                        </td>

                                                                        <%--<td>
                                                                            <%#Eval("ContactPersonName")%>
                                                                        </td>--%>
                                                                        <td>
                                                                            <%#Eval("MobileNumber")%>
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
                                        <td>
                                            <div>
                                                <asp:Button ID="btnsavebulk" class="butn_save" ValidationGroup="savebulk" OnClick="btnsavebulk_Click"
                                                    runat="server" Text="Save" />
                                                <asp:Button ID="Button2" class="butn" runat="server" Text="Close" OnClick="btnClosebulkOnClick" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

