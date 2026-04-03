<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Customer.Master" AutoEventWireup="true" CodeBehind="DocumentExpiry.aspx.cs" Inherits="AmarCentre.Customer.DocumentExpiry" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main">

        <div class="head">
            <div class="col-div-6">
                <span style="font-size: 30px; cursor: pointer; color: white;" class="nav">&#9776; Document Expiry</span>
                <span style="font-size: 30px; cursor: pointer; color: white;" class="nav2">&#9776; Document Expiry</span>
            </div>
            <div class="col-div-6">
                <div class="profile">
                    <img src="../Images/profiles.png" class="pro-img" />
                    <p>
                        <asp:Label ID="lbl_User_name" runat="server" Font-Size="Large" ForeColor="White"></asp:Label>
                        <asp:HiddenField ID="hdn_user_id" runat="server" />
                    </p>
                </div>
            </div>
            <div class="clearfix"></div>
        </div>

        <div>
            <div class="list-div">
                <div class="listbox">
                    <div class="content-box">

                        <asp:UpdatePanel ID="Upd_addpanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:UpdatePanel ID="Upd_List_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>

                                        <table style="width: 90%">
                                            <tr>
                                                <td style="width: 30%; padding-left: 2%">From
                                                    <br />
                                                    <telerik:RadDatePicker ID="txtFromdate" runat="server" class="input-boder" DateInput-DateFormat="dd/MM/yyyy">
                                                        <Calendar runat="server" ID="Calendar1" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                            ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                            <SpecialDays>
                                                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                </telerik:RadCalendarDay>
                                                            </SpecialDays>
                                                        </Calendar>
                                                    </telerik:RadDatePicker>

                                                </td>
                                                <td style="width: 30%">To
                                                    <br />
                                                    <telerik:RadDatePicker ID="txtTodate" runat="server" class="input-boder" DateInput-DateFormat="dd/MM/yyyy">
                                                        <Calendar runat="server" ID="Calendar2" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                            ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                            <SpecialDays>
                                                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                                </telerik:RadCalendarDay>
                                                            </SpecialDays>
                                                        </Calendar>
                                                    </telerik:RadDatePicker>

                                                </td>
                                                <td style="width: 30%">Document Type
                                                                    <telerik:RadComboBox ID="drp_doc" Sort="Ascending" Filter="Contains" runat="server"
                                                                        AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search Document..."
                                                                        Style="overflow: hidden; width: 80%; border: none!important;" OnClientFocus="OnClientKeyPressing"
                                                                        OnClientBlur="ValidateCombo">
                                                                    </telerik:RadComboBox>

                                                </td>

                                                <td style="width: 10%">
                                                    <asp:Button ID="btn_search" class="butn" runat="server" OnClick="btn_search_OnClick"
                                                        Text="Search" />
                                                </td>
                                            </tr>
                                        </table>
                                        <table>
                                            <thead>
                                                <tr style="text-align: center; background-color: #272e56;">
                                                    <th style="width: 5%;">Sl No
                                                    </th>

                                                    <th style="width: 10%;">Document Type
                                                    </th>
                                                    <th style="width: 12%;">Employee Name
                                                    </th>
                                                    <th style="width: 12%;">Document Number
                                                    </th>
                                                    <th style="width: 8%;">Valid From
                                                    </th>
                                                    <th style="width: 8%;">Valid Till
                                                    </th>
                                                    <th style="width: 10%;">Remark
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="rpt_list" runat="server">
                                                    <ItemTemplate>
                                                        <tr style="text-align: center; background-color: #272e56;" onmouseover="this.style.backgroundColor='#1b203d';" onmouseout="this.style.backgroundColor='#272e56';">
                                                            <td style="text-align: center">
                                                                <%# Container.ItemIndex + 1 %>
                                                            </td>

                                                            <td style="padding-left: 3px; text-align: left;">
                                                                <%#Eval("DocumentType")%>
                                                            </td>
                                                            <td style="padding-left: 3px; text-align: left;">
                                                                <%#Eval("EmployeeName")%>
                                                            </td>
                                                            <td style="padding-left: 3px; text-align: left;">
                                                                <%#Eval("DocumentNo")%>
                                                            </td>
                                                            <td style="padding-left: 3px; text-align: center;">
                                                                <%#Eval("ValidFrom")%>
                                                            </td>
                                                            <td style="padding-left: 3px; text-align: center;">
                                                                <%#Eval("Expirydate")%>
                                                            </td>
                                                            <td style="padding-left: 3px;">
                                                                <%#Eval("Remark")%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>


                                            </tbody>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

