<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Customer.Master" AutoEventWireup="true" CodeBehind="ServiceRequest.aspx.cs" Inherits="AmarCentre.Transactions.ServiceRequest" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">

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
            $('.read_Only').attr('readonly', true);
        }

    </script>
    <style type="text/css">
        .formTable td {
            border-bottom: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main">

        <div class="head">
            <div class="col-div-6">
                <span style="font-size: 30px; cursor: pointer; color: white;" class="nav">&#9776; Service Request</span>
                <span style="font-size: 30px; cursor: pointer; color: white;" class="nav2">&#9776; Service Request</span>
            </div>
            <div class="col-div-6">
                <div class="profile">
                    <img src="../Images/profiles.png" class="pro-img" />
                    <p>
                        <asp:Label ID="lbl_User_name" runat="server" Font-Size="Large" ForeColor="White"></asp:Label>
                    </p>
                </div>
            </div>
            <div class="clearfix"></div>
        </div>
        <div style="text-align: right; margin-right: 1%">
            <asp:Button ID="btn_addnew" runat="server" Text="Create New Request" class="butn_save" OnClick="btn_newentry_OnClick" />
            <asp:TextBox ID="txt_search" runat="server" class="txt_search" AutoPostBack="true" Width="25%"
                OnTextChanged="txt_search_OnTextChanged" placeholder="Search"></asp:TextBox>
        </div>
        <div style="height: 10px"></div>

        <div>
            <div class="list-div">
                <div class="listbox">
                    <div class="content-box">
                        <asp:UpdatePanel ID="Upd_List_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>

                                <asp:HiddenField ID="Common_order_column" runat="server" />
                                <asp:HiddenField ID="Common_asc_desc" runat="server" />
                                <div class="list_info" style="display: none">
                                </div>
                                <table>
                                    <tr>
                                        <th style="width: 5%;">Sl 
                                        </th>
                                        <th style="width: 7%;">Code 
                                        </th>
                                        <th style="width: 7%;">Date 
                                        </th>
                                         <th style="width: 15%;">Service 
                                        </th>
                                         <th style="width: 12%;">Applicant 
                                        </th>
                                        <th style="width: 8%;">Status
                                        </th>
                                        <th style="width: 5%;">Action
                                        </th>
                                    </tr>
                                    <asp:Repeater ID="rpt_list" runat="server" OnItemCommand="rpt_list_OnItemCommand">
                                        <ItemTemplate>
                                            <tr style="background-color: #272e56;" onmouseover="this.style.backgroundColor='#1b203d';" onmouseout="this.style.backgroundColor='#272e56';">
                                                <td>
                                                    <%#Eval("RowNum")%>.
                                        <asp:HiddenField ID="hdn_id" runat="server" Value='<%#Eval("Id")%>' />
                                                </td>
                                                <td>
                                                    <%#Eval("Code")%>
                                                </td>
                                                <td>
                                                    <%#Eval("Dateds")%>
                                                </td>
                                                <td>
                                                    <%#Eval("Service")%>
                                                </td>
                                                <td>
                                                    <%#Eval("Applicant")%>
                                                </td>
                                                <td>
                                                    <%#Eval("StatusName")%>
                                                </td>
                                               <%-- <td>
                                                    <%#Eval("RejectRemark")%>
                                                </td>--%>
                                                <td style="text-align: center">
                                                    <asp:Button ID="btn_edit" runat="server" class="btn_edit" ToolTip="Edit" CommandName="Edit" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <tr>
                                        <td colspan="7" class="navigationRow">
                                            <asp:UpdatePanel ID="Upd_Nav_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lbl_page_info" runat="server" class="pageInfo"></asp:Label>
                                                    <asp:Button ID="btn_first" runat="server" Text="<<" OnClick="btn_first_OnClick" />
                                                    <asp:Button ID="btn_prev" runat="server" Text="<" OnClick="btn_prev_OnClick" />
                                                    <asp:Label ID="lbl_page_number" Style="font-weight: bold; margin-left: 5px; margin-right: 5px; text-align: center;"
                                                        runat="server"></asp:Label>
                                                    <asp:Button ID="btn_next" runat="server" Text=">" OnClick="btn_next_OnClick" />
                                                    <asp:Button ID="btn_last" runat="server" Text=">>" OnClick="btn_last_OnClick" />
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
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>


                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <div>
            </div>
        </div>
        <div>
            <asp:UpdatePanel ID="Upd_Add_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnl_add" Visible="false" runat="server">
                        <div class="popupBackground">
                        </div>
                        <div class="animated halfPopUpCustomer">
                            <asp:UpdatePanel ID="upd_main" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div id="div_main" runat="server">
                                        <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
                                            UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="headpopup">
                                                    Service Request
                                                </div>
                                                <table class="formTable">
                                                    <tr>
                                                        <td style="width: 48%">Code 
                                                        <asp:TextBox ID="lbl_Code" runat="server" class="txt read_Only" Font-Bold="true"
                                                            Text=""></asp:TextBox>
                                                        </td>
                                                        <td style="width: 48%">Date <span style="color: Red">&nbsp*</span>
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
                                                    </tr>
                                                    <tr>
                                                        <td>Service <span style="color: Red">&nbsp*</span>
                                                            <telerik:RadComboBox ID="drpTemplates" Sort="Ascending" Filter="Contains" runat="server"
                                                                AllowCustomText="false" RenderMode="Lightweight"
                                                                EmptyMessage="Search ..." OnClientFocus="OnClientKeyPressing" AutoPostBack="true"
                                                                OnSelectedIndexChanged="drpTemplatesOnSelectedIndexChanged" Style="overflow: hidden; width: 97%; border: none!important;">
                                                            </telerik:RadComboBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drpTemplates"
                                                                ValidationGroup="save" ErrorMessage="Required" Style="color: Red" InitialValue=""></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td>
                                                            Applicant
                                                            <asp:TextBox ID="txtApplicant" Width="95%" CssClass="txt" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Upload Documents
                                                         <asp:UpdatePanel ID="updFileuploadOut" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                             <ContentTemplate>
                                                                 <telerik:RadAsyncUpload ID="radDocOut" MaxFileSize="10240000" runat="server"
                                                                     MaxFileInputsCount="1" OnFileUploaded="radDoc_FileUploaded">
                                                                 </telerik:RadAsyncUpload>
                                                                 <asp:HiddenField ID="hdnFilenameRDOut" runat="server" />
                                                                 <asp:HiddenField ID="hdnFilenameSaveRDOut" runat="server" />
                                                                 <asp:Button ID="btn_newdocOut" runat="server" OnClick="btn_newdocOut_Click" ToolTip="Add" class="btn_add_new" />

                                                                 <table class="Intable" style="width: 45%; text-align: left">
                                                                     <tr style="font-weight: bold; font-size: 16px">
                                                                         <td style="width: 80%;">File</td>
                                                                         <td style="width: 19%;">Action</td>
                                                                     </tr>
                                                                     <asp:Repeater ID="rptDocumentOut" runat="server" OnItemCommand="rptDocumentOut_ItemCommand">
                                                                         <ItemTemplate>
                                                                             <tr>
                                                                                 <td style="padding: 1%; vertical-align: middle">
                                                                                     <%#Eval("FileNames") %>
                                                                                     <asp:HiddenField ID="hdnFilenameOut" runat="server" Value='<%#Eval("FileNames") %>' />
                                                                                     <asp:HiddenField ID="hdnFilenameSaveOut" runat="server" Value='<%#Eval("FileNamesSave") %>' />
                                                                                 </td>
                                                                                 <td style="padding: 1%">
                                                                                     <asp:Button ID="btn_doc_dwnldOut" ToolTip="Download" CssClass="btn_doc_down" runat="server"
                                                                                         CommandName="Download" />
                                                                                     <asp:Button ID="Button15Out" ToolTip="Delete" CssClass="btn_delete" runat="server"
                                                                                         CommandName="DeleteFile" />
                                                                                 </td>
                                                                             </tr>

                                                                         </ItemTemplate>
                                                                     </asp:Repeater>
                                                                 </table>
                                                             </ContentTemplate>
                                                             <Triggers>
                                                                 <asp:PostBackTrigger ControlID="rptDocumentOut" />
                                                             </Triggers>
                                                         </asp:UpdatePanel>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblstatus" runat="server" Font-Bold="true"></asp:Label>
                                                            <asp:Label ID="lblreject" Visible="false" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>

                                                    <tr style="display: none">
                                                        <td colspan="2">
                                                            <div id="div_item_new" runat="server" style="width: 100%; overflow: auto;">
                                                                <div style="height: 10px">
                                                                </div>
                                                                <asp:UpdatePanel ID="Upd_Item_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <table class="listTable">
                                                                            <thead>
                                                                                <tr style="text-align: center">
                                                                                    <th style="width: 3%">Sl
                                                                                    </th>
                                                                                    <th style="width: 22%">Department
                                                                                    </th>
                                                                                    <th style="width: 22%">Service 
                                                                                    </th>
                                                                                    <th style="width: 10%">Applicant
                                                                                    </th>
                                                                                    <th style="width: 7%">Action
                                                                                    </th>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>
                                                                                <asp:Repeater ID="rpt_Item_list" runat="server" OnItemDataBound="rptitemlistDatabound" OnItemCommand="rpt_Item_list_ItemCommand">
                                                                                    <ItemTemplate>
                                                                                        <tr style="text-align: center" runat="server" id="tr_in">
                                                                                            <td style="width: 5%">
                                                                                                <%# Container.ItemIndex + 1 %>
                                                                                                <asp:HiddenField ID="hdnDId" runat="server" Value='<%#Eval("D_id") %>' />

                                                                                            </td>
                                                                                            <td style="text-align: left; width: 15%">
                                                                                                <asp:UpdatePanel ID="UpdDepartment" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:HiddenField ID="hdnDepartmentId" runat="server" Value='<%#Eval("DepartmentId") %>' />
                                                                                                        <telerik:RadComboBox ID="drpDepartment" Sort="Ascending" Filter="Contains" runat="server"
                                                                                                            AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search ..."
                                                                                                            AutoPostBack="true" OnSelectedIndexChanged="drpDepartmentOnSelectedIndexChanged" ClientIDMode="AutoID"
                                                                                                            Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                                                            OnClientBlur="ValidateCombo">
                                                                                                        </telerik:RadComboBox>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
                                                                                            <td style="text-align: left; width: 15%">
                                                                                                <asp:UpdatePanel ID="UpdService" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:HiddenField ID="hdnServiceId" runat="server" Value='<%#Eval("ServiceId") %>' />
                                                                                                        <telerik:RadComboBox ID="drpService" Sort="Ascending" Filter="Contains" runat="server"
                                                                                                            AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search ..."
                                                                                                            AutoPostBack="true" OnSelectedIndexChanged="drpService_OnSelectedIndexChanged" ClientIDMode="AutoID"
                                                                                                            Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                                                            OnClientBlur="ValidateCombo">
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="drpService"
                                                                                                            ValidationGroup="save_serdetail" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                                                                            InitialValue="">
                                                                                                        </asp:RequiredFieldValidator>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
                                                                                            <td style="text-align: left; width: 10%;">
                                                                                                <asp:TextBox ID="txt_Applname" Width="95%" CssClass="txt" runat="server"></asp:TextBox>
                                                                                            </td>
                                                                                            <td style="text-align: center; width: 7%">
                                                                                                <asp:Button ID="Button2" runat="server" CommandName="Add" ToolTip="Add"
                                                                                                    ValidationGroup="save_serdetail" class="btn_add_new" />
                                                                                                <asp:Button ID="btn_edit_line" runat="server" ToolTip="Upload Document" CommandName="Document"
                                                                                                    class="btn_doc_up" />
                                                                                                <asp:Button ID="btn_remove_line" CommandName="Delete" class="btn_delete" runat="server"
                                                                                                    ToolTip="Delete" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');" />
                                                                                                <asp:UpdatePanel ID="UpdDocument" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                                    <Triggers>
                                                                                                        <asp:PostBackTrigger ControlID="rptDocument" />
                                                                                                        <asp:PostBackTrigger ControlID="btn_newdoc" />
                                                                                                    </Triggers>
                                                                                                    <ContentTemplate>
                                                                                                        <asp:Panel ID="pnlDocument" Visible="false" runat="server">
                                                                                                            <div class="popupBackground">
                                                                                                            </div>
                                                                                                            <div class="animated smallPopUp" style="width: 35%; text-align: left">
                                                                                                                <div class="Adding_heading">
                                                                                                                    Upload Documents / تحميل المرفقات
                                                                                                                </div>
                                                                                                                <telerik:RadAsyncUpload ID="RadAsyncUpload1" MaxFileSize="1024000" runat="server"
                                                                                                                    MaxFileInputsCount="1" OnFileUploaded="RadAsyncUpload1_OnFileUploaded">
                                                                                                                </telerik:RadAsyncUpload>
                                                                                                                <asp:HiddenField ID="hdnFilenameRD" runat="server" />
                                                                                                                <asp:HiddenField ID="hdnFilenameSaveRD" runat="server" />
                                                                                                                <asp:Button ID="btn_newdoc" runat="server" CommandName="adddocIn" ToolTip="Add" class="btn_add_new" />
                                                                                                                <asp:UpdatePanel ID="updFileupload" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                                                                                    <ContentTemplate>
                                                                                                                        <table class="Intable" style="width: 95%; text-align: left">
                                                                                                                            <tr style="font-weight: bold">
                                                                                                                                <td>File</td>
                                                                                                                                <td>Action</td>
                                                                                                                            </tr>
                                                                                                                            <asp:Repeater ID="rptDocument" runat="server" OnItemCommand="rptDocument_ItemCommand">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <tr>
                                                                                                                                        <td style="width: 55%;">
                                                                                                                                            <%#Eval("FileNames") %>
                                                                                                                                            <asp:HiddenField ID="hdnFilename" runat="server" Value='<%#Eval("FileNames") %>' />
                                                                                                                                            <asp:HiddenField ID="hdnFilenameSave" runat="server" Value='<%#Eval("FileNamesSave") %>' />
                                                                                                                                        </td>
                                                                                                                                        <td style="width: 35%;">
                                                                                                                                            <asp:Button ID="btn_doc_dwnld" ToolTip="Download" CssClass="btn_doc_down" runat="server"
                                                                                                                                                CommandName="Download" />
                                                                                                                                            <asp:Button ID="Button15" ToolTip="Delete" CssClass="btn_delete" runat="server"
                                                                                                                                                CommandName="DeleteFile" />
                                                                                                                                        </td>
                                                                                                                                    </tr>

                                                                                                                                </ItemTemplate>
                                                                                                                            </asp:Repeater>
                                                                                                                        </table>
                                                                                                                    </ContentTemplate>
                                                                                                                </asp:UpdatePanel>

                                                                                                                <asp:Button ID="btnCloseDoc" runat="server" CssClass="butn" Text="Save & Close" CommandName="CloseDocument" />

                                                                                                            </div>
                                                                                                        </asp:Panel>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>


                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                </asp:Repeater>
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
                                                            <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdn_user_id" runat="server" />
                                                            <asp:HiddenField ID="hdnDetailIndexId" runat="server" />
                                                            <asp:HiddenField ID="hdnLanguage" runat="server" />
                                                            <asp:HiddenField ID="hdnRequestStatus" ClientIDMode="Static" runat="server" Value="0" />
                                                            <asp:Button ID="btn_save" class="butn_save" ValidationGroup="save" OnClick="btn_save_OnClick"
                                                                runat="server" Text="Save" OnClientClick="if (Page_ClientValidate('save') == false) return(false);else return confirm('Do you really want to Save.. ?');" />
                                                            <asp:Button ID="btnhistory" class="butn_save" runat="server" Visible="false" Text="History" OnClick="btnhistory_OnClick" />
                                                            <asp:Button ID="btn_cancel" class="butn_delete" runat="server" Visible="false" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');"
                                                                Text="Delete" OnClick="btn_DeleteOnClick" />
                                                            <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset" OnClick="btn_reset_OnClick" />
                                                            <asp:Button ID="Button1" class="butn" runat="server" Text="Close" OnClick="btn_close_OnClick" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div>
                                                    <div id="div1" class="messageAlert div_pop animated" style="display: none" runat="server">
                                                        <div class="tick">
                                                            &#10004
                                                        </div>
                                                        <div>
                                                            <asp:Label ID="lbl_msgin" Font-Bold="true" ForeColor="White" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                    <asp:UpdatePanel ID="Updhistory" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlhistry" runat="server" Visible="false">
                                                <div class="popupBackground">
                                                </div>
                                                <div class="animated halfPopUpCustomer" style="text-align: left">
                                                    <div class="headpopup">
                                                        History
                                                    </div>
                                                    <table>
                                                        <tr>
                                                            <th style="width: 5%;">Sl
                                                            </th>
                                                            <th style="width: 30%;">Action
                                                            </th>
                                                            <th style="width: 15%;">Done By
                                                            </th>
                                                            <th style="width: 15%;">Date
                                                            </th>
                                                        </tr>
                                                        <asp:Repeater ID="rpt_His" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td style="text-align: center;">
                                                                        <%# Container.ItemIndex + 1 %>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("ActionRemark")%>
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

                                                    </table>

                                                    <asp:Button ID="Button54" class="butn" runat="server" Text="Close" OnClick="btn_histry_Close_OnClick" />
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

