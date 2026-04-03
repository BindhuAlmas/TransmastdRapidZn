<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCVendor.ascx.cs" Inherits="AmarCentre.Masters.UserControl.UCVendor" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Panel ID="Panel1" runat="server">
    <div class="popupBackground">
    </div>
    <div class="animated smallPopUp">
        <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <div class="Adding_heading">
                    Vendor/بائع
                </div>
                <table class="formTable">
                    <tr>
                        <td>Name/اسم <span style="color: Red">&nbsp*</span>
                            <asp:TextBox ID="txt_name" CssClass="txt" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txt_name"
                                ValidationGroup="saveV" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                InitialValue=""></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Address/العنوان
                                            <br />
                            <asp:TextBox ID="txt_address" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Mobile /هاتف<span style="color: Red">&nbsp*</span>
                            <asp:TextBox ID="txt_mob" runat="server" class="txt numbers_only"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_mob"
                                ValidationGroup="saveV" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                InitialValue=""></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Email /البريد الالكتروني
                            <asp:TextBox ID="txt_email" CssClass="txt" runat="server"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Please Enter Valid Email ID"
                                ValidationGroup="saveV" ControlToValidate="txt_email" Style="color: Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                Display="Dynamic">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>TRN
                            <asp:TextBox ID="txt_trn" CssClass="txt" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td>Commission
                            <asp:TextBox ID="txtcommission" class="txt numbers_only" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr >
                        <td>
                            <asp:CheckBox ID="chkIsAlsoCustomer" runat="server" Text="Is Also Customer" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div>
                                <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                <asp:HiddenField ID="hdn_user_id" runat="server" />
                                <asp:HiddenField ID="hdnPageId" runat="server" />

                                <asp:Button ID="btn_save" class="butn_save" ValidationGroup="saveV" OnClick="btn_save_OnClick"
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



