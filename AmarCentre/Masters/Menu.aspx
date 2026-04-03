<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true"
    CodeBehind="Menu.aspx.cs" Inherits="AmarCentre.Masters.Menu" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Menu Master/إعداد القائمة
    </div>
    <div>
        <asp:UpdatePanel ID="Upd_List_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="margin:2%;width:35%">
                    <asp:HiddenField ID="hdn_user_id" runat="server" />
                    <asp:HiddenField ID="hdn_add_MM" runat="server" Value="0" />
                    <asp:HiddenField ID="hdn_update_MM" runat="server" Value="0" />
                    <asp:HiddenField ID="hdn_delete_MM" runat="server" Value="0" />
                    <asp:HiddenField ID="hdn_add_SM" runat="server" Value="0" />
                    <asp:HiddenField ID="hdn_update_SM" runat="server" Value="0" />
                    <asp:HiddenField ID="hdn_delete_SM" runat="server" Value="0" />
                    <div class="div_items">
                        <span class="main_text">Main Menu/القائمة الرئيسية 
                            <asp:Button ID="btn_new_line" runat="server" ToolTip="Add Main Menu" class="btn_add_new"
                                OnClick="btn_new_line_OnClick" />
                        </span>
                    </div>
                    <asp:Repeater ID="rpt_main_menu" runat="server" OnItemDataBound="rpt_main_menu_OnItemDataBound">
                        <ItemTemplate>
                            <div style="border-bottom: 2px solid #52308d;clear:both">
                                <span>
                                    <asp:Button ID="btn_expand" runat="server" ToolTip="Expand Menu" class="btn_expand"
                                        OnClick="btn_expand_OnClick" />
                                    <asp:Button ID="btn_collapse" runat="server" Visible="false" ToolTip="Collapse Menu"
                                        class="btn_collapse" OnClick="btn_collapse_OnClick" />
                                    <asp:Label ID="lbl_main" runat="server" Text='<%#Eval("MainMenu")%>'></asp:Label>
                                    <asp:HiddenField ID="hdn_main" Value='<%#Eval("MainMenuId")%>' runat="server" />
                                    <asp:HiddenField ID="hdn_main_DO" Value='<%#Eval("Display_Order")%>' runat="server" />
                                    <asp:Button ID="btn_menu_edit" runat="server" Style="float: right" ToolTip="Edit Main Menu"
                                        class="btn_edit" OnClick="btn_menu_edit_OnClick" />
                                    <asp:Button ID="btn_add_sub_menu" runat="server" Style="float: right" ToolTip="Add Sub Menu"
                                        class="btn_add_new" OnClick="btn_add_sub_menu_OnClick" />
                                </span>
                                <br />
                            </div>
                            <div id="div_sub_menu" runat="server" visible="false" style="clear:both">
                                <asp:Repeater ID="rpt_sub_menu" runat="server">
                                    <ItemTemplate>
                                        <div style="border-bottom: 1px solid #52308d;clear:both">
                                            <span style="float:left">
                                                <%#Eval("SubMenu")%>
                                                <asp:HiddenField ID="hdn_sub" Value='<%#Eval("SubMenuId")%>' runat="server" />
                                               
                                            </span>
                                            <asp:Button ID="btn_sub_menu_edit" runat="server" Style="float: right" ToolTip="Edit Sub Menu"
                                                    class="btn_edit" OnClick="btn_sub_menu_edit_OnClick" />
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div></div>
        </div>
    <div >
        <asp:UpdatePanel ID="Upd_Add_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:Panel ID="pnl_add" Visible="false" runat="server">
                    <div class="popupBackground">
                    </div>
                    <div class="animated smallPopUp">
                <div id="div_main_edit" runat="server" visible="false">
                    <div class="Adding_heading">
                        Add/Edit Main Menu/اضافة - تعديل القائمة الرئيسية 
                    </div>
                    <table  class="formTable">
                        <tr>
                            <td >
                                Main Menu/القائمة الرئيسية  <span style="color: Red">&nbsp*</span>
                                <asp:TextBox ID="txt_main_menu" CssClass="txt" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txt_main_menu"
                                    ValidationGroup="save_M"  Display="Dynamic" ErrorMessage="Required" Style="color: Red" InitialValue=""></asp:RequiredFieldValidator>
                                <asp:HiddenField ID="hdn_add_M" runat="server" Value="0" />
                            </td>
                             </tr>
                        <tr>
                            <td >
                                Display Order/عرض النظام 
                                <asp:TextBox ID="txt_main_display_order" runat="server" class="txt numbers_only"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txt_main_display_order"
                                    ValidationGroup="save_M"  Display="Dynamic" ErrorMessage="Required" Style="color: Red" InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <asp:Button ID="btn_save_M" class="butn_save" runat="server" ValidationGroup="save_M"
                                    Text="Save/حفظ" OnClick="btn_save_M_OnClick" />
                                <asp:Button ID="btn_delete_M" class="butn_delete" runat="server" Text="Delete/حذف" Visible="false"
                                    OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');"
                                    OnClick="btn_delete_M_OnClick" />
                                <asp:Button ID="btn_main_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_close_main_menu_OnClick" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="div_sub_edit" runat="server" visible="false">
                    <div class="Adding_heading">
                        Add/Edit Sub Menu/اضافة - تعديل القائمة الفرعية 
                    </div>
                    <table  class="formTable">
                        <tr>
                            <td >
                                Main Menu/القائمة الرئيسية  <span style="color: Red">&nbsp*</span>
                                <asp:UpdatePanel ID="UpdMainMenuPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                                <telerik:RadComboBox ID="drp_mainmenuu" Sort="Ascending" Filter="Contains" runat="server"
                                    TabIndex="1" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search Main Menu..."
                                    Style="overflow: hidden; width: 96%; border: none!important;">
                                </telerik:RadComboBox>
                </ContentTemplate>
                </asp:UpdatePanel>
                
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="drp_mainmenuu"
                                    ValidationGroup="save_S"  Display="Dynamic" ErrorMessage="Required" Style="color: Red" InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                            </tr>
                        <tr>
                            <td >
                                Sub Menu/القائمة الفرعية  <span style="color: Red">&nbsp*</span>
                                <asp:TextBox ID="txt_sub_menu" runat="server" CssClass="txt"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_sub_menu"
                                    ValidationGroup="save_S"  Display="Dynamic" ErrorMessage="Required" Style="color: Red" InitialValue=""></asp:RequiredFieldValidator>
                                
                            </td>
                        </tr>
                        <tr>
                            <td >
                                Destination/المكان المقصود  <span style="color: Red">&nbsp*</span>
                                <asp:TextBox ID="txt_sub_dest" runat="server" CssClass="txt"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_sub_dest"
                                    ValidationGroup="save_S"  Display="Dynamic" ErrorMessage="Required" Style="color: Red" InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                            </tr>
                        <tr>
                            <td >
                                Display Order/عرض النظام  <span style="color: Red">&nbsp*</span>
                                <asp:TextBox ID="txt_display_order" runat="server" class="txt numbers_only"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_display_order"
                                    ValidationGroup="save_S"  Display="Dynamic" ErrorMessage="Required" Style="color: Red" InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                            </tr>
                        <tr>
                            <td >
                                <asp:Button ID="btn_save_S" class="butn_save" runat="server" ValidationGroup="save_S"
                                    Text="Save/حفظ" OnClick="btn_save_S_OnClick" />
                                <asp:Button ID="btn_delete_S" class="butn_delete" runat="server" Text="Delete/حذف" Visible="false"
                                    OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');"
                                    OnClick="btn_delete_S_OnClick" />
                                <asp:Button ID="btn_sub_rest" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_close_sub_menu_OnClick" />
                                <asp:HiddenField ID="hdn_add_S" runat="server" Value="0" />
                                <asp:HiddenField ID="hdn_add_S_Main" runat="server" Value="0" />
                            </td>
                        </tr>
                    </table>
                </div>
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
