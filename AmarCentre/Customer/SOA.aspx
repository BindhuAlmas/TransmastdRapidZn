<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Customer.Master" AutoEventWireup="true" CodeBehind="SOA.aspx.cs" Inherits="AmarCentre.Reports.SOA" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main">

        <div class="head">
            <div class="col-div-6">
                <span style="font-size: 30px; cursor: pointer; color: white;" class="nav">&#9776; Account Statement</span>
                <span style="font-size: 30px; cursor: pointer; color: white;" class="nav2">&#9776;Account Statement</span>
            </div>
            <div class="col-div-6">
                <div class="profile">
                    <img src="../Images/profiles.png" class="pro-img" />
                    <p>
                        <asp:Label ID="lbl_User_name" runat="server" Font-Size="Large" ForeColor="White"></asp:Label>
                    </p>
                </div>
            </div>
            <div class="clearfix"></div>
        </div>
        <asp:Button ID="btn_filter" runat="server" Visible="false" class="filter right_align_list" OnClick="btn_filter_OnClick" />
   
    <asp:UpdatePanel ID="upd_nav_filter" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnl_filter" runat="server" Visible="false">
                <div class="animated smallPopUpCustomer">
                    <div class="headpopup">
                        Search
                    </div>
                    
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div>
        <div class="list-div" >
            <div class="listbox">
                <div class="content-box">
                    <asp:UpdatePanel ID="Upd_addpanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" >
                        <ContentTemplate>
                            <table class="formTable">
    <tr>
        <td>From Date<span style="color: Red">&nbsp*</span>
            <telerik:RadDatePicker ID="txtFromDate" runat="server" class="input-boder" DateInput-DateFormat="dd/MM/yyyy">
                <Calendar runat="server" ID="Calendar1" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                    ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                    <SpecialDays>
                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                        </telerik:RadCalendarDay>
                    </SpecialDays>
                </Calendar>
            </telerik:RadDatePicker>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFromDate"
                Display="Dynamic" ValidationGroup="save" ErrorMessage="Required" Style="color: Red"
                InitialValue=""></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td>To Date<span style="color: Red">&nbsp*</span>
            <telerik:RadDatePicker ID="txtToDate" runat="server" class="input-boder" DateInput-DateFormat="dd/MM/yyyy">
                <Calendar runat="server" ID="Calendar2" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                    ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                    <SpecialDays>
                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                        </telerik:RadCalendarDay>
                    </SpecialDays>
                </Calendar>
            </telerik:RadDatePicker>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtToDate"
                Display="Dynamic" ValidationGroup="save" ErrorMessage="Required" Style="color: Red"
                InitialValue=""></asp:RequiredFieldValidator>
        </td>
    </tr>

    <tr>
        <td>
           <%-- <asp:Button ID="btn_search" ValidationGroup="save" class="butn" runat="server" OnClick="btn_search_OnClick"
                Text="Search" />
            <asp:Button ID="btn_excel" class="butn" runat="server" ValidationGroup="save" Text="Generate Excel"
                OnClick="btn_excel_OnClick" />--%>
            <asp:Button ID="btnPdf" class="butn" runat="server" ValidationGroup="save" Text="Generate PDF"
                OnClick="btnPdfOnClick" />
            <asp:HiddenField ID="hdn_user_id" runat="server" />
        </td>
    </tr>
</table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
 </div>
</asp:Content>
