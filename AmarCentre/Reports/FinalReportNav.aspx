<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="FinalReportNav.aspx.cs" Inherits="AmarCentre.Reports.FinalReportNav" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
       Final VAT Report/تقرير ضريبة القيمة المضافة النهائي

        <asp:Button ID="btn_filter" runat="server" class="filter right_align_list" OnClick="btn_filter_OnClick" />
    </div>
    <asp:UpdatePanel ID="upd_nav_filter" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnl_filter" runat="server">
                <div class="animated smallPopUpFilter">
                    <div class="Adding_heading">
                        Search
                    </div>
                    <table class="formTable">
                        <tr>
                            <td>
                               Year <span style="color: Red">&nbsp*</span>
                                <telerik:RadComboBox ID="drpYear" Sort="Ascending" Filter="Contains" runat="server"
                                    AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                     OnClientBlur="ValidateCombo" EmptyMessage="Search Year..."
                                     Style="overflow: hidden;
                                    width: 96%; border: none!important;">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="drpYear"
                                    Display="Dynamic" ValidationGroup="save" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                 From Month <span style="color: Red">&nbsp*</span>
                                <telerik:RadComboBox ID="drpFromMnth" Sort="Ascending" Filter="Contains" runat="server"
                                    AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                     OnClientBlur="ValidateCombo" EmptyMessage="Search Month..."
                                     Style="overflow: hidden;
                                    width: 96%; border: none!important;">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drpFromMnth"
                                    Display="Dynamic" ValidationGroup="save" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                To Month <span style="color: Red">&nbsp*</span>
                                <telerik:RadComboBox ID="drpToMnth" Sort="Ascending" Filter="Contains" runat="server"
                                    AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                    OnClientBlur="ValidateCombo" EmptyMessage="Search Name..."
                                     Style="overflow: hidden;
                                    width: 96%; border: none!important;">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="drpToMnth"
                                    Display="Dynamic" ValidationGroup="save" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btn_pdf" class="butn" runat="server" ValidationGroup="save" Text="Generate PDF"
                                    OnClick="btnPdfOnClick" />
                                <asp:HiddenField ID="hdn_user_id" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


