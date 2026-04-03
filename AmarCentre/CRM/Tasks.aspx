<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true"
    CodeBehind="Tasks.aspx.cs" Inherits="AmarCentre.CRM.Tasks" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Report
             <asp:Button ID="btn_filter" runat="server" class="filter right_align_list" OnClick="btn_filter_OnClick" />
        <asp:HiddenField ID="hdnUserId" runat="server" />
    </div>
      <asp:UpdatePanel ID="upd_nav_filter" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnl_filter" runat="server">
                <div class="animated smallPopUp">
                    <div class="Adding_heading">
                        Search
                    </div>
                    <table class="formTable">
                        <tr>
                                <td >From
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
                            </tr><tr>
                                <td >To
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
                               </tr><tr>
                                <td >Lead Source
                                      <telerik:RadComboBox ID="drpSource" Sort="Ascending" Filter="Contains" runat="server"
                                          AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                          OnClientBlur="ValidateCombo" EmptyMessage="Search Lead Source..."
                                          Style="overflow: hidden; width: 96%; border: none!important;">
                                      </telerik:RadComboBox>
                                </td>
                               </tr><tr>
                                <td >Assigned Employee
                                      <telerik:RadComboBox ID="drpEmployee" Sort="Ascending" Filter="Contains" runat="server"
                                          AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                          OnClientBlur="ValidateCombo" EmptyMessage="Search Employee..." 
                                          Style="overflow: hidden; width: 96%; border: none!important;">
                                      </telerik:RadComboBox>
                                </td>
                                </tr>
                            <tr style="display:none;">
                                <td >Segment
                                    <telerik:RadComboBox ID="drpJurisdiction" Sort="Ascending" Filter="Contains" runat="server"
                                        AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                        OnClientBlur="ValidateCombo" EmptyMessage="Search ..."
                                        Style="overflow: hidden; width: 96%; border: none!important;">
                                    </telerik:RadComboBox>

                                </td>
                               </tr><tr>
                                <td >Priority
                                    <br />
                                    <telerik:RadComboBox ID="drpPriority" Sort="Ascending" Filter="Contains" runat="server"
                                        AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                        OnClientBlur="ValidateCombo" EmptyMessage="Search Priority..." Style="overflow: hidden; width: 96%; border: none!important;">
                                    </telerik:RadComboBox>

                                </td>
                               </tr><tr>
                                <td >Status
                                      <telerik:RadComboBox ID="drpStatus" Sort="Ascending" Filter="Contains" runat="server"
                                          AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                          OnClientBlur="ValidateCombo" EmptyMessage="Search Status..."
                                          Style="overflow: hidden; width: 96%; border: none!important;">
                                      </telerik:RadComboBox>
                                </td>
                               </tr><tr style="display:none;">
                                <td >Activity
                                      <telerik:RadComboBox ID="drpActivity" Sort="Ascending" Filter="Contains" runat="server"
                                          AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                          OnClientBlur="ValidateCombo" EmptyMessage="Search ..."
                                          Style="overflow: hidden; width: 96%; border: none!important;">
                                      </telerik:RadComboBox>
                                </td>
                               </tr><tr>
                                <td >
                                     <asp:Button ID="btn_search" class="butn" runat="server" OnClick="btn_search_OnClick"
                                        Text="Search" />
                                    <asp:Button ID="btnexcel_export" class="butn" runat="server" OnClick="btn_excel_OnClick"
                                        Text="Generate Excel" />
                                </td>
                            </tr>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="Upd_addpanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="overflow-x: auto; min-height: 250px; width: 100%">
                <asp:UpdatePanel ID="Upd_List_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                       
                        <table class="listTable" style="width: 98%; border: 1px">
                            <thead>
                                <tr style="text-align: center">
                                    <th style="width: 3%;">Sl 
                                    </th>
                                     <th style="width: 7%;">Lead Date
                                    </th>
                                    <%--<th style="width: 13%;">Contact Person
                                    </th>--%>
                                    <th style="width: 8%;">Lead Source
                                    </th>
                                     <th style="width: 9%;">Employee
                                    </th>
                                    <th style="width: 9%;">Activity
                                    </th>
                                    <%--<th style="width: 9%;">Jurisdiction
                                    </th>--%>
                                     <th style="width: 9%;">Next Followup
                                    </th>
                                    <th style="width: 9%;">Priority
                                    </th>
                                     <%--<th style="width: 7%;">Activity
                                    </th>--%>
                                    <th style="width: 7%;">Status
                                    </th>
                                       <th style="width: 7%;">Contact No
                                    </th>
                                     <th style="width: 7%;">Email
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rpt_list" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td style="text-align: center">
                                               <%#Eval("Sl")%>
                                            </td>
                                            <td>
                                                <%#Eval("Date")%>
                                            </td>
                                            <%--<td>
                                                <%#Eval("ContactPerson")%>
                                            </td>--%>
                                            <td>
                                                <%#Eval("LeadSource")%>
                                            </td>
                                            <td>
                                                <%#Eval("Assigned Employee")%>
                                            </td>
                                            <td>
                                                <%#Eval("Activity")%>
                                            </td>
                                            <%--<td>
                                                <%#Eval("Jurisdiction")%>
                                            </td>--%>
                                             <td>
                                                <%#Eval("NextFollowup")%>
                                            </td>
                                              <td>
                                                <%#Eval("Priority")%>
                                            </td>
                                            <%--<td>
                                                <%#Eval("Segmentname")%>
                                            </td>--%>
                                            <td>
                                                <%#Eval("Status")%>
                                            </td>
                                            <td>
                                                <%#Eval("ContactNo")%>
                                            </td>
                                             <td>
                                                <%#Eval("EmailId")%>
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


