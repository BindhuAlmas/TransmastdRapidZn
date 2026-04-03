<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCMail.ascx.cs" Inherits="AmarCentre.Transactions.UserControl.UCMail" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Panel ID="Panel1" runat="server">
    <div class="popupBackground">
    </div>
    <div class="animated smallPopUp">
        <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <div class="Adding_heading">
                    Send Mail
                </div>
                <table class="formTable">
                    <tr>
                        <td>

                            <asp:UpdatePanel ID="UpdMailList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table class="listTable">
                                        <thead>
                                            <tr>
                                                <th style="width: 80%">Mail Id
                                                </th>
                                                <th style="width: 5%"> 
                                                    <asp:Button ID="btn_serDetail_newEntry" OnClick="btn_serDetail_newEntry_Click" ToolTip="Add Mail Id" ValidationGroup="add" runat="server" class="btn_add_new" />
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptmaildetail" runat="server" OnItemCommand="rptmaildetail_ItemCommand">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtmail" Class="txt" Width="90%" runat="server" Text='<%#Eval("MailId") %>'></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Please Enter Valid Email ID"
                                                                ValidationGroup="add" ControlToValidate="txtmail" Style="color: Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                                Display="Dynamic">
                                                            </asp:RegularExpressionValidator>
                                                              <asp:RequiredFieldValidator ID="reqtxtmail" runat="server" ControlToValidate="txtmail"
                                                            ValidationGroup="add" ErrorMessage="*" Style="color: Red" InitialValue=""></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btn_remove_line" class="btn_delete" CommandName="Delete" runat="server" ToolTip="Delete" />
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
                    <tr style="display:none">
                        <td>
                            <asp:Panel ID="pnlFile" runat="server" Visible="false">
                                Add Attachment
                            <br />
                                <telerik:RadAsyncUpload ID="fu_file" MaxFileSize="500000000" runat="server"
                                    MaxFileInputsCount="1" OnFileUploaded="fu_File_OnFileUploaded">
                                </telerik:RadAsyncUpload>
                                <asp:UpdatePanel ID="Upd_fufile" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="hdn_file" runat="server" Value="" />
                                        <asp:Label ID="lblnofile" runat="server" Text="Please upload file to proceed" Visible="false"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                             <asp:UpdatePanel ID="updcustomerid" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="hdncustomerId" runat="server" />
                                          </ContentTemplate>
                                </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div>
                                <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                <asp:HiddenField ID="hdn_user_id" runat="server" />
                                <asp:HiddenField ID="hdnPageId" runat="server" />
                                <asp:HiddenField ID="hdnfromdate" runat="server" />
                                <asp:HiddenField ID="hdntodate" runat="server" />
                                <asp:HiddenField ID="hdnPaymentStatus" runat="server" />
                                <asp:HiddenField ID="hdnCompletionStatus" runat="server" />

                                <asp:Button ID="btn_save" class="butn_save" OnClick="btn_save_OnClick"
                                    runat="server" Text="Send Mail" />
                                <asp:Button ID="Button2" class="butn" runat="server" Text="Close" OnClick="btn_close_OnClick" />
                                
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




