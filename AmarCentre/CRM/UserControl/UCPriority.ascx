<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCPriority.ascx.cs" Inherits="AmarCentre.CRM.UserControl.UCPriority" %>


 <asp:Panel ID="PanelAdd"  runat="server">
                    <div class="popupBackground">
                    </div>
                    <div class="animated smallPopUp">
                        <asp:UpdatePanel ID="UpdPanelAddInner" runat="server" ChildrenAsTriggers="false"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="Adding_heading">
                                    Priority
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td>
                                            Name <span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txtName" CssClass="txt" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtName"
                                                ValidationGroup="saveuc" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Description
                                            <br />
                                            <asp:TextBox ID="txtDescription" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div>
                                                <asp:HiddenField ID="hdnId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnUserId" runat="server" />
                                                <asp:Button ID="btnSave" class="butn_save" ValidationGroup="saveuc" OnClick="btnSaveOnClick"
                                                    runat="server" Text="Save" />
                                                <asp:Button ID="btnClose" class="butn" runat="server" Text="Close" OnClick="btnCloseOnClick" />
                                                <asp:HiddenField ID="hdnAdd" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnUpdate" runat="server" Value="0" />
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