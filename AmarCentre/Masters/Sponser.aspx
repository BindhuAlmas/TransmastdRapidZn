<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="Sponser.aspx.cs" Inherits="AmarCentre.Masters.Sponser" %>


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
           
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
      Sponsor
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
                            <th style="width: 5%;">
                                Sl No/رقم
                            </th>
                            <th style="width: 20%;">
                                Name/اسم
                            </th>
                            <th style="width: 7%;">
                                Mobile/هاتف
                            </th>
                            <th style="width: 13%;">
                                Remark/تعليق
                            </th>
                            <th style="width: 5%;">
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
                                   Sponsor
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td>
                                            Name/اسم <span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txt_name" CssClass="txt" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txt_name"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            Arabic Name/الاسم بالعربي </span>
                                            <asp:TextBox ID="txtArabicName" CssClass="txt" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td>
                                            Mobile /هاتف<span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txt_mob" runat="server" class="txt numbers_only"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_mob"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            Phone Number/رقم الهاتف
                                            <br />
                                            <asp:TextBox ID="txt_phn" runat="server" class="txt numbers_only"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Email/البريد الالكتروني
                                            <br />
                                            <asp:TextBox ID="txt_email" CssClass="txt" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Please Enter Valid Email ID"
                                                ValidationGroup="save" ControlToValidate="txt_email" Style="color: Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                        </td>
                                        <td>
                                            UAE Pass
                                            <br />
                                            <asp:TextBox ID="txtuaepass" runat="server" class="txt"></asp:TextBox>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td colspan="2">
                                            Address/العنوان
                                            <br />
                                            <asp:TextBox ID="txt_address" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                         </tr>
                                    
                                    <tr>
                                        <td  colspan="2">
                                            Remark/تعليق
                                            <br />
                                            <asp:TextBox ID="txt_remark" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                  
                                    <tr>
                                        <td colspan="2">
                                            <div>
                                                <asp:HiddenField ID="hdn_user_id" runat="server" />
                                                <asp:HiddenField ID="hdn_id" runat="server" />

                                                <asp:Button ID="btn_save" class="butn_save" ValidationGroup="save" OnClick="btn_save_OnClick"
                                                    runat="server" Text="Save/حفظ" />
                                                <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_OnClick" />
                                                <asp:Button ID="btn_delete" class="butn_delete" runat="server" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');"
                                                    Visible="false" Text="Delete/حذف" OnClick="btn_delete_OnClick" />
                                               
                                                <asp:Button ID="btn_doc" class="butn" runat="server" Visible="false" Text="Documents/وثيقة"
                                                    OnClick="btn_docadd_OnClick" />
                                                
                                                <asp:Button ID="Button2" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_OnClick" />
                                                <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_update" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_delete" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_doc" runat="server" Value="0" />
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
                                                                <td style="width:20%">
                                                                    Document Type<span style="color: Red">&nbsp*</span>
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
                                                                <td >
                                                                    Document Number
                                                                    <br />
                                                                    <asp:TextBox ID="txt_doc_no" CssClass="txt" runat="server"></asp:TextBox>
                                                                </td>
                                                                </tr>
                                                              <tr>
                                                                <td>
                                                                    Document Name/اسم المستندات
                                                                    <br />
                                                                    <asp:TextBox ID="txt_docname" CssClass="txt" runat="server"></asp:TextBox>
                                                                </td>
                                                                 </tr>
                                                            <tr>
                                                                <td>
                                                                    Valid From/صالح من تاريخ
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
                                                                <td >
                                                                    Remark/تعليق
                                                                    <br />
                                                                    <asp:TextBox ID="txt_docremark" CssClass="txtarea" TextMode="MultiLine" runat="server"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    Upload File/ملفات محملة
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
                                                                        Text="Add/اضافة " OnClick="btn_add_doc_OnClick" />
                                                                    <asp:Button ID="btn_Dreset" class="butn" runat="server" Text="Reset/إعادة تعيين"
                                                                        OnClick="btn_reset_doc_OnClick" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td >
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
             
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

