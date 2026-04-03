<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true"
    CodeBehind="SeqNumber.aspx.cs" Inherits="AmarCentre.Masters.SeqNumber" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Sequence Number/رقم التسلسل
        <asp:Button ID="btn_addnew" runat="server"   class="btnAddNew" OnClick="btn_newentry_OnClick" />
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
                              <th style="width: 5%">Sl No/رقم
                            </th>
                            <th style="width: 15%"> Category/الفئة
                            </th>
                             <th style="width: 12%">   Menu/القائمة 
                            </th>
                            <th style="width: 8%"> Current No/الرقم الحالي 
                            </th>
                            <th style="width: 5%">Action/عمل
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
                                        <%#Eval("Category")%>
                                    </td>
                                     <td>
                                        <%#Eval("MenuName")%>
                                    </td>
                                    <td>
                                        <%#Eval("CurrentNo")%>
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
                    <div class="animated smallPopUp">
                     <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                        <div class="Adding_heading">
                            Sequence Number/رقم التسلسل
                        </div>
                        <table class="formTable">
                            <tr>
                                <td style="width: 50%">
                                    Category/الفئة <span style="color: Red">&nbsp*</span>
                                    <asp:TextBox ID="txt_category" CssClass="txt" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txt_category"
                                        ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                        InitialValue=""></asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 50%">
                                    Prefix/اختصار  <span style="color: Red">&nbsp*</span>
                                    <asp:TextBox ID="txt_prefix" CssClass="txt" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_prefix"
                                        ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                        InitialValue=""></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Seperator/منفصل 
                                    <asp:TextBox ID="txt_seperator" CssClass="txt" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    Current No/الرقم الحالي  <span style="color: Red">&nbsp*</span>
                                    <asp:TextBox ID="txt_current_no" runat="server" class="txt numbers_only"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_current_no"
                                        ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                        InitialValue=""></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Increment/زيادة راتب  <span style="color: Red">&nbsp*</span>
                                    <asp:TextBox ID="txt_increment" class="txt numbers_only" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_increment"
                                        ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                        InitialValue=""></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    Menu/القائمة  <span style="color: Red">&nbsp*</span>
                                    <telerik:RadComboBox ID="drp_menus" Sort="Ascending" runat="server" RenderMode="Lightweight"
                                        EmptyMessage="Search Menu..." AllowCustomText="true" Filter="Contains" OnClientFocus="OnClientKeyPressing"
                                        OnClientBlur="ValidateCombo" Style="overflow: hidden; width: 96%; border: none!important;">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="drp_menus"
                                    ValidationGroup="save"  Display="Dynamic" ErrorMessage="Required" Style="color: Red" InitialValue=""></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Mini. Digit/ارقام مصغرة 
                                    <asp:TextBox ID="txt_moni_dig" runat="server" class="txt numbers_only"></asp:TextBox>
                                </td>
                                <td>
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
