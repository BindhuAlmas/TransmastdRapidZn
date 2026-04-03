<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="PLDetSum.aspx.cs" Inherits="AmarCentre.Reports.PLDetSum" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
       Profit Loss Report
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
                                From Date <span style="color: Red">&nbsp*</span>
                                <br />
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
                                    Display="Dynamic" ValidationGroup="Detailed" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                To Date <span style="color: Red">&nbsp*</span>
                                <br />
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
                                    Display="Dynamic" ValidationGroup="Detailed" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btn_search" ValidationGroup="Detailed" class="butn" runat="server" OnClick="btnPLDetailed_OnClick"
                                    Text="Detailed" />
                                     <asp:Button ID="btn_excelDet" class="butn" runat="server" ValidationGroup="Detailed" Text="Generate Excel"
                                    OnClick="btn_excelDet_OnClick" />
                                    <asp:Button ID="Button1" ValidationGroup="Detailed" class="butn" runat="server" OnClick="btnPLSummary_OnClick"
                                    Text="Summary" />
                                     <asp:Button ID="btn_excelSum" class="butn" runat="server" ValidationGroup="Detailed" Text="Generate Excel"
                                    OnClick="btn_excelSum_OnClick" />
                                <asp:HiddenField ID="hdn_user_id" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="Upd_addpanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
           
            <br />
            <div class="">
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_excelDet" />
            <asp:PostBackTrigger ControlID="btn_excelSum" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

