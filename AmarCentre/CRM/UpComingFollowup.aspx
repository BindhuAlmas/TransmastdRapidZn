<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="UpComingFollowup.aspx.cs" Inherits="AmarCentre.CRM.UpComingFollowup" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
     Upcoming Followup
             <asp:HiddenField ID="hdn_user_id" runat="server" />
    </div>
    <asp:UpdatePanel ID="Upd_addpanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="overflow-x: auto; min-height: 250px; width: 100%">
                <asp:UpdatePanel ID="Upd_List_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table style="width:90%">
                            <tr>
                                <td style="width:30%; padding-left:2%">From <br /> 
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
                                <td style="width:30%">To <br /> 
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
                                <td style="width:30%">  
                                    <asp:Button ID="btn_search" class="butn" runat="server" OnClick="btn_search_OnClick"
                                    Text="Search" /> 
                                      <asp:Button ID="btnexcel_export" class="butn" runat="server" OnClick="btn_excel_OnClick"
                                    Text="Generate Excel" /> 
                                </td>
                            </tr>
                        </table>
                        <table class="listTable" style="width: 98%; border: 1px">
                            <thead>
                                <tr style="text-align: center">
                                    <th style="width: 3%;">Sl
                                    </th>
                                    <%--<th style="width: 13%;">Contact Person
                                    </th>--%>
                                      <th style="width: 12%;">Lead
                                    </th>
                                    <th style="width: 12%;">Activity
                                    </th>
                                    <th style="width: 5%;">Contact No
                                    </th>
                                    <th style="width: 9%;">Follow up Date
                                    </th>
                                    <th style="width: 9%;">Follow up Time
                                    </th>
                                    <th style="width: 10%;">Status
                                    </th>
                                     <th style="width: 10%;">Assigned Employee
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rpt_list" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td style="text-align: center">
                                              <%# Container.ItemIndex + 1 %>
                                            </td>
                                           <%--<td>
                                        <%#Eval("ContactPersonName")%>
                                    </td>--%>
                                    <td>
                                        <%#Eval("CompanyName")%>
                                    </td>
                                             <td>
                                        <%#Eval("Activity")%>
                                    </td>
                                    <td>
                                        <%#Eval("MobileNumber")%>
                                    </td>
                                    <td>
                                        <%#Eval("NextFollowupDate")%>
                                    </td>
                                              <td>
                                        <%#Eval("NextFollowupTime")%>
                                    </td>
                                    <td>
                                        <%#Eval("Statusname")%>
                                    </td>
                                             <td>
                                        <%#Eval("AssignedEmployee")%>
                                    </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnexcel_export" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


