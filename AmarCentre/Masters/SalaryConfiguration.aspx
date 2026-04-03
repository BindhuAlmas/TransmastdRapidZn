<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true"
    CodeBehind="SalaryConfiguration.aspx.cs" Inherits="AmarCentre.Masters.SalaryConfiguration" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Salary Configuration/تكوين الراتب
        <div class="searchDiv">
        </div>
    </div>
    <asp:UpdatePanel ID="Upd_Add_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <div class="list_info" style="display: none">
                </div>
                <table>
                    <tr>
                        <td>
                            Salary Process From Date/عملية الراتب من تاريخ <span style="color: Red">&nbsp*</span>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drpSPFromDate" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search From Date..." Style="overflow: hidden;
                                width: 110%; border: none!important;">
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="drpSPFromDate"
                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                InitialValue=""></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Salary Process To Date/عملية الرانب حتى الان <span style="color: Red">&nbsp*</span>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drpSPToDate" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                OnClientBlur="ValidateCombo" EmptyMessage="Search To Date..." Style="overflow: hidden;
                                width: 110%; border: none!important;">
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drpSPToDate"
                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                InitialValue=""></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chkOTApplicable" runat="server" Text="Overtime Applicable/العمل الاضافي المطبق "
                                AutoPostBack="true" OnCheckedChanged="chkOTApplicableOnCheckedChanged" />
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="UpdOvertime" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlOvertime" Visible="false" runat="server">
                            <table>
                                <tr>
                                    <td style="width: 52%">
                                        OT On Normal Day/في الايام العادية <span style="color: Red">&nbsp*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOTNormalDay" class="txt numbers_only" Style="width: 134% !important"
                                            runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtOTNormalDay"
                                            ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                            InitialValue=""></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        OT On Weekend/في ايام عطلة نهاية الاسبوع <span style="color: Red">&nbsp*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOTWeekend" class="txt numbers_only" Style="width: 134% !important"
                                            runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtOTWeekend"
                                            ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                            InitialValue=""></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        OT On Public Holiday/في العطلات الرسمية <span style="color: Red">&nbsp*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOTHoliday" class="txt numbers_only" Style="width: 134% !important"
                                            runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtOTHoliday"
                                            ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                            InitialValue=""></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                </tr>
                <table>
                    <tr>
                        <td>
                            General Working Hours/ساعات العمل العامة <span style="color: Red">&nbsp*</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtWorkingHours" class="txt numbers_only" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtWorkingHours"
                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                InitialValue=""></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rbBasedOnMonth" runat="server" GroupName="SalaryBasedOn" />Salary
                            Based on Month/الراتب على اساس الشهر
                        </td>
                        <td>
                            <asp:RadioButton ID="rbBasedOnWorkingDays" runat="server" GroupName="SalaryBasedOn" />Salary
                            Based on Working Days/الراتب على اساس ايام العمل
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Weekend Days/ايام عطلة نهاية الاسبوع <span style="color: Red">&nbsp*</span>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="drpWeekendDays" Sort="Ascending" Filter="Contains" runat="server"
                                AllowCustomText="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" RenderMode="Lightweight"
                                EmptyMessage="Search Weekend Days..." OnClientFocus="OnClientKeyPressing" Style="overflow: hidden;
                                width: 97%; border: none!important;">
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="drpWeekendDays"
                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                InitialValue=""></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                            <asp:HiddenField ID="hdn_user_id" runat="server" Value="0" />
                            <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                            <asp:Button ID="btn_save" class="butn_save" OnClick="btn_save_OnClick" ValidationGroup="save"
                                runat="server" Text="Save/حفظ" />
                        </td>
                    </tr>
                </table>
                <div>
                </div>
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
