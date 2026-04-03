<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Customer.ascx.cs" Inherits="AmarCentre.Transactions.UserControl.Customer" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Masters/UserControl/UCCustCategory.ascx" TagName="CCategory"
    TagPrefix="AmarCentre" %>
    <div class="popupBackground">
    </div>
    <div class="animated halfPopUp" >
       
        <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <div class="Adding_heading">
                    Customer/زبون

                </div>
                <table class="formTable">
                    <tr>
                        <td style="width: 48%">Name/اسم <span style="color: Red">&nbsp*</span>
                            <asp:TextBox ID="txt_name" CssClass="txt" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txt_name"
                                ValidationGroup="saveCustomer" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                InitialValue=""></asp:RequiredFieldValidator>
                        </td>
                        <td style="width: 48%">Arabic Name/الاسم بالعربي  </span>
                            <asp:TextBox ID="txtArabicName" CssClass="txt" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Agent/وكيل
                        
                            <telerik:RadComboBox ID="drpagent" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" rendermode="Lightweight"
                                EmptyMessage="Search Agent..." OnClientFocus="OnClientKeyPressing" Style="overflow: hidden; width: 97%; border: none!important;">
                            </telerik:RadComboBox>
                        </td>
                        <td>Sponsor
                                             <telerik:RadComboBox ID="drpSponser" Sort="Ascending" Filter="Contains" runat="server"
                                                 AllowCustomText="false" RenderMode="Lightweight"
                                                 EmptyMessage="Search Sponsor..." OnClientFocus="OnClientKeyPressing" Style="overflow: hidden; width: 97%; border: none!important;">
                                             </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Mobile /هاتف<span style="color: Red">&nbsp*</span>
                            <asp:TextBox ID="txt_mob" runat="server" MaxLength="10" class="txt numbers_only"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_mob"
                                ValidationGroup="saveCustomer" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                InitialValue=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Please Enter 10 digit"
                                ValidationGroup="saveCustomer" ControlToValidate="txt_mob" Style="color: Red"
                                ValidationExpression="^[0-9]{10}$" Display="Dynamic">
                            </asp:RegularExpressionValidator>
                        </td>
                        <td>Phone Number/رقم الهاتف 
                            <br />
                            <asp:TextBox ID="txt_phn" runat="server" class="txt numbers_only"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Email (with comma if multiple)
                            <br />
                            <asp:TextBox ID="txt_email" CssClass="txt" TextMode="MultiLine" runat="server"></asp:TextBox>
                            <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Please Enter Valid Email ID"
                                ValidationGroup="saveCustomer" ControlToValidate="txt_email" Style="color: Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                Display="Dynamic">
                            </asp:RegularExpressionValidator>--%>
                        </td>
                        <td>CC Email (with comma if multiple)
                            <br />
                            <asp:TextBox ID="txtccmail" CssClass="txt" TextMode="MultiLine" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>TRN
                            <br />
                            <asp:TextBox ID="txt_trn" CssClass="txt" runat="server"></asp:TextBox>
                        </td>
                        <td>Contact Person
                                            <br />
                            <asp:TextBox ID="txtCperson" CssClass="txt" runat="server"></asp:TextBox>

                        </td>

                    </tr>
                    <tr>
                        <td>MOHRE No
                                            <br />
                            <asp:TextBox ID="txtmohre" CssClass="txt" runat="server"></asp:TextBox>
                        </td>
                        <td>License No
                                            <br />
                            <asp:TextBox ID="txtlicense" CssClass="txt" runat="server"></asp:TextBox>

                        </td>

                    </tr>
                    <tr>
                        <td>Emirate 
                                            <telerik:RadComboBox ID="drpEmirate" Sort="Ascending" Filter="Contains" runat="server"
                                                AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search ..."
                                                OnClientFocus="OnClientKeyPressing" OnClientBlur="ValidateCombo" Style="overflow: hidden; width: 96%; border: none!important;">
                                            </telerik:RadComboBox>
                        </td>
                        <td>Customer Category
                                             <asp:UpdatePanel ID="updCategory" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                 <ContentTemplate>
                                                     <telerik:RadComboBox ID="drpCategory" Sort="Ascending" Filter="Contains" runat="server"
                                                         AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                                         OnClientBlur="ValidateCombo" EmptyMessage="Search ..." AutoPostBack="true"
                                                         OnSelectedIndexChanged="drpCategory_SelectedIndexChanged"
                                                         Style="overflow: hidden; width: 96%; border: none!important;">
                                                     </telerik:RadComboBox>
                                                 </ContentTemplate>
                                             </asp:UpdatePanel>
                        </td>

                    </tr>
                    <tr>
                        <td>WhatsApp Number (with CountryCode)
                             <asp:TextBox ID="txtWhatsappNo" runat="server" MaxLength="20" class="txt"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Enter Valid Character"
                                ValidationGroup="saveCustomer" ControlToValidate="txtWhatsappNo" Style="color: Red"
                                ValidationExpression="^[0-9+]+$" Display="Dynamic">
                            </asp:RegularExpressionValidator>
                        </td>
                        <td>
                            <asp:Panel ID="pnlcompanygrp" runat="server" Visible="false">
                                Company Group
        <telerik:RadComboBox ID="drpcompanygrp" Sort="Ascending" Filter="Contains" runat="server"
            AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
            OnClientBlur="ValidateCombo" EmptyMessage="Search ..."
            Style="overflow: hidden; width: 96%; border: none!important;">
        </telerik:RadComboBox>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>Address/العنوان 
                            <br />
                            <asp:TextBox ID="txt_address" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td>Remark/تعليق
                            <br />
                            <asp:TextBox ID="txt_remark" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Panel ID="pnlchkcompanygrp" runat="server" Visible="false">
                                <asp:CheckBox ID="chkcompanygrp" runat="server" Text="Is Main Company" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chkIsTyping" runat="server" Text="Is Typing Center" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="Upd_CreditAmount_Panel" runat="server" ChildrenAsTriggers="false"
                                UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lbl_msg" runat="server" ForeColor="Red"></asp:Label>

                                    <asp:Panel ID="pnl_CreditAmount" runat="server" Visible="false">
                                        Credit Amount/مبلغ الائتمان <span style="color: Red">&nbsp*</span>
                                        <asp:TextBox ID="txt_CreditAmount" runat="server" class="txt numbers_only"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_CreditAmount"
                                            ValidationGroup="saveCustomer" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                            InitialValue=""></asp:RequiredFieldValidator>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:CheckBox ID="chk_IsCredit" runat="server" Text="" AutoPostBack="true" OnCheckedChanged="chk_IsCredit_OnCheckedChanged"
                                Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div>
                                <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                <asp:HiddenField ID="hdn_user_id" runat="server" />
                                <asp:HiddenField ID="hdn_agentid" runat="server" />
                                  <asp:HiddenField ID="hdnDefaultEmirate" runat="server" />
  <asp:HiddenField ID="hdnIsprofessionversion" runat="server" />
                                <asp:Button ID="btn_saveCustomer" class="butn_save" ValidationGroup="saveCustomer"
                                    OnClick="btn_saveCustomer_OnClick" runat="server" Text="Save/حفظ" />
                                <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_OnClick" />
                                <asp:Button ID="Button2" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_OnClick" />
                                <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                            </div>
                        </td>
                    </tr>
                </table>

                <div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
   
     <asp:UpdatePanel ID="updCategoryPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" >
     <ContentTemplate>
         <asp:Panel ID="pnlCategory" Visible="false" runat="server">
             <AmarCentre:CCategory ID="UCCategory" runat="server" />
         </asp:Panel>
     </ContentTemplate>
 </asp:UpdatePanel>    
    </div>

