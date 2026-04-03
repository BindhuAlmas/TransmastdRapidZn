<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCDepartment.ascx.cs" Inherits="AmarCentre.Masters.UserControl.UCDepartment" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Panel ID="Panel1" runat="server">
    <div class="popupBackground">
    </div>
    <div class="animated smallPopUp">
        <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <div class="Adding_heading">
                    Department/قسم
                </div>
                <table class="formTable">
                    <tr>
                        <td>Name/اسم <span style="color: Red">&nbsp*</span>
                            <asp:TextBox ID="txt_name" CssClass="txt" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txt_name"
                                ValidationGroup="saveD" Display="Dynamic" ErrorMessage="Required" Style="color: Red" InitialValue=""></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Arabic Name/الاسم بالعربي  </span>
                                            <asp:TextBox ID="txtArabicName" CssClass="txt" runat="server"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td>Description/وصف
                                            <br />
                            <asp:TextBox ID="txt_desc" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div>
                                <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                <asp:HiddenField ID="hdn_user_id" runat="server" />
                                <asp:Button ID="btn_save" class="butn_save" ValidationGroup="saveD" OnClick="btn_save_OnClick"
                                    runat="server" Text="Save/حفظ" />
                                <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_OnClick" />
                                <asp:Button ID="Button2" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_OnClick" />
                                <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
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

