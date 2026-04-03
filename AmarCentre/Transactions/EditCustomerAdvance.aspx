<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="EditCustomerAdvance.aspx.cs" Inherits="AmarCentre.Transactions.EditCustomerAdvance" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="Upd_Add_Panel" runat="server" ChildrenAsTriggers="false"
        UpdateMode="Conditional">
        <ContentTemplate>
            <div class="HeadIng_Div">
                Edit Customer Advance
            </div>
            <table class="formTable">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="Upd_ItemList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table class="listTable">
                                    <thead>
                                        <tr>
                                            <th style="width:5%">Sl No/رقم
                                            </th>
                                            <th style="width:15%">Customer
                                            </th>
                                            <th style="width:10%">Current Advance
                                            </th>
                                            <th style="width:10%">Actual Advance
                                            </th>
                                            <th style="width:5%">Action/عمل
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rpt_serdetail" runat="server" OnItemDataBound="rpt_serdetail_OnItemDataBound">
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <%# Container.ItemIndex + 1 %>
                                                    </td>
                                                    <td>
                                                        <asp:HiddenField ID="hdnCustomerId" runat="server" Value='<%#Eval("CustomerId") %>' />
                                                        <telerik:RadComboBox ID="drpCustomer" Sort="Ascending" Filter="Contains" runat="server"
                                                            AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search Customer..."
                                                            Style="overflow: hidden; width: 85%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                            ClientIDMode="AutoID" OnSelectedIndexChanged="drpCustomerOnSelectedIndexChanged"
                                                            AutoPostBack="true" OnClientBlur="ValidateCombo">
                                                        </telerik:RadComboBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4a" runat="server" ControlToValidate="drpCustomer"
                                                            ValidationGroup="save_serdetail" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                            InitialValue="">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="Updadvance" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblAdvance" runat="server" Text='<%#Eval("Advance") %>'></asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_amt" Class="txt numbers_only" runat="server" Text='<%#Eval("Amount") %>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_amt"
                                                            ValidationGroup="save_serdetail" Display="Dynamic" ErrorMessage="*" Style="color: Red"
                                                            InitialValue="">
                                                        </asp:RequiredFieldValidator>
                                                    </td>

                                                    <td class="listTableActionButtonDiv">
                                                        <asp:Button ID="btn_serDetail_newEntry" runat="server" class="btn_add_new"
                                                            ValidationGroup="save_serdetail" OnClick="btn_serDetail_newEntry_OnClick" />
                                                        <asp:Button ID="btn_remove_line" class="btn_delete" runat="server" ToolTip="Delete"
                                                            OnClick="btn_remove_line_OnClick" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');" />
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
                        <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                        <asp:HiddenField ID="hdn_user_id" runat="server" />
                        <asp:HiddenField ID="hdn_add" runat="server" />

                        <asp:Button ID="btn_save" class="butn_save" ValidationGroup="save" OnClick="btn_save_OnClick"
                            runat="server" Text="Save/حفظ" />
                        <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_OnClick" />

                    </td>
                </tr>
            </table>
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
</asp:Content>
