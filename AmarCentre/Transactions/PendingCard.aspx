<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="PendingCard.aspx.cs" Inherits="AmarCentre.Transactions.PendingCard" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        <asp:HiddenField ID="hdn_user_id" runat="server" Value="" />

        <asp:HiddenField ID="hdn_add" runat="server" />
        Pending Card Collection List
        <div class="searchDiv">
            <asp:TextBox ID="txt_search" runat="server" class="txt_search" AutoPostBack="true" OnTextChanged="drpfilterOnSelectedIndexChanged"
                placeholder="Search"></asp:TextBox>

        </div>
        <telerik:RadComboBox ID="drpStatus" Sort="Ascending" Filter="Contains" runat="server"
    AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing" AutoPostBack="true" OnSelectedIndexChanged="drpStatus_SelectedIndexChanged"
    OnClientBlur="ValidateCombo" EmptyMessage="Search ..."
    
    Style="overflow: hidden; width: 16%; border: none!important; float: right; padding-right: 5px; margin-top: 0px">

    <Items>
        <telerik:RadComboBoxItem Text="Pending" Value="0" Selected="true" />
        <telerik:RadComboBoxItem Text="Processed" Value="1" />
    </Items>
</telerik:RadComboBox>
        <telerik:RadComboBox ID="drpBankAccountfilter" Sort="Ascending" Filter="Contains" runat="server"
            AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
            OnClientBlur="ValidateCombo" EmptyMessage="Search ..." AutoPostBack="true"
            OnSelectedIndexChanged="drpfilterOnSelectedIndexChanged" Style="overflow: hidden; width: 16%;
            border: none!important; float: right; padding-right: 5px; margin-top: 0px">
        </telerik:RadComboBox>
    
    </div>
    <div>
        <asp:UpdatePanel ID="Upd_List_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                 <asp:UpdatePanel ID="updtotal" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div>
                    <div style="float: right;width:10%">
                        <asp:Button ID="btnProceed" class="butn_save" runat="server" Text="Proceed" OnClick="btnProceed_Click"
                            OnClientClick="javascript : return confirm('Do you really want to Save.. ?');" />
                    </div>
                    <div style="border: medium none; color: Red; font-size: 24px; float: right;width:10%">
                        <asp:TextBox ReadOnly="true" Style="border: medium none; color: Red; font-size: 24px;" class="txt" ID="txtTotal" runat="server"></asp:TextBox>
                    </div>
                    <div style="border: medium none; color: Red; font-size: 24px; float: right;width:10%">
                        Total
                    </div>
                </div>
                </ContentTemplate>
                     </asp:UpdatePanel>
                <table class="listTable">
                    <thead>
                        <tr>
                            <th style="width: 4%;">
                                <asp:CheckBox ID="chkselectall" runat="server" AutoPostBack="true" OnCheckedChanged="chkselectall_CheckedChanged" />
                            </th>
                            <th style="width: 8%;">
                                Ref No
                            </th>
                              <th style="width: 8%;">
                               Date
                            </th>
                            <th style="width: 25%;">
                               Customer
                            </th>
                            <th style="width: 8%;">
                               Receipt Amount
                            </th>
                              <th style="width: 8%;">
                               Commission %
                            </th>
                             <th style="width: 8%;">
                               Commission Amount
                            </th>
                            <th style="width: 8%;">VAT
                            </th>
                             <th style="width: 10%;">
                              Credit Amount
                            </th>
                            <th style="width: 15%;">
                              Bank 
                            </th>
                            <th style="width: 10%;">
                               Credited Date * 
                            </th>
                          
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rpt_list" runat="server" OnItemDataBound="rpt_list_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                         <asp:CheckBox ID="chkselectIn" runat="server"  AutoPostBack="true" OnCheckedChanged="chkselectIn_CheckedChanged" />
                                        <%--<%# Container.ItemIndex + 1 %>.--%>
                                        <asp:HiddenField ID="hdn_id" runat="server" Value='<%#Eval("Id")%>' />
                                        <asp:HiddenField ID="hdnTypeId" runat="server" Value='<%#Eval("TypeId")%>' />
                                        <asp:HiddenField ID="hdnstatus" runat="server" Value='<%#Eval("IsCardCollect")%>' />
                                        <asp:HiddenField ID="hdnAccountId" runat="server" Value='<%#Eval("BankAccountId")%>' />
                                          <asp:HiddenField ID="hdnAmount" runat="server" Value='<%#Eval("Amount")%>' />
                                    </td>
                                    <td>
                                        <%#Eval("Code")%>
                                    </td>
                                     <td>
                                        <%#Eval("RecDate")%>
                                    </td>
                                      <td>
                                        <%#Eval("Customer")%>
                                    </td>
                                      <td>
                                        <%#Eval("Amount")%>
                                    </td>
                                     <td>
                                        <%#Eval("CommissionPer")%>
                                    </td>
                                     <td>
                                          <asp:TextBox   class="txt" ID="txtBankCommission" AutoPostBack="true" ClientIDMode="AutoID" OnTextChanged="txtBankCommission_TextChanged" Text='<%#Eval("BankCommission")%>' Width="85%" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox class="txt" ID="txtCommissionVat" AutoPostBack="true" ClientIDMode="AutoID" OnTextChanged="txtBankCommission_TextChanged" Text='<%#Eval("CommissionVat")%>' Width="85%" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="updCreditAmountIn" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:HiddenField ID="hdnCreditAmount" runat="server" Value='<%#Eval("CreditAmount")%>' />
                                                <asp:Label ID="lblCreditAmount" runat="server" Text='<%#Eval("CreditAmount")%>' />

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                     <td>
                                   
                                       <telerik:RadComboBox  ID="drpBankAccount" runat="server" AllowCustomText="false"
                                           RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing" OnClientBlur="ValidateCombo"
                                        DropDownWidth="150px"   EmptyMessage="Search Name..." Style="overflow: hidden; width: 85%; border: none!important;">
                                       </telerik:RadComboBox>
                                       
                                   </td>
                                    <td>
                                        <%--<telerik:RadDatePicker ID="Carddate" Width="110px" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                            <Calendar runat="server" ID="Calendar2" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                ShowRowHeaders="False" UseColumnHeadersAsSelectors="False" SelectedDate='<%#Eval("CardCollectDate")%>'>
                                                <SpecialDays>
                                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                    </telerik:RadCalendarDay>
                                                </SpecialDays>
                                            </Calendar>
                                        </telerik:RadDatePicker>--%>
                                        <asp:label   class="txt" ID="lbldate"    Text='<%# Eval("CardCollectDate") == DBNull.Value ? "" : ((DateTime)Eval("CardCollectDate")).ToString("dd-MM-yyyy") %>' width="110px" runat  ="server"></asp:label>
                                        <telerik:RadDatePicker ID="Carddate" Width="110px" runat="server"
                                            DateInput-DateFormat="dd/MM/yyyy"
                                            >
                                            <Calendar runat="server" ID="Calendar2" CssClass="rtlSupport"
                                                ShowOtherMonthsDays="False" ShowRowHeaders="False"
                                                UseColumnHeadersAsSelectors="False">
                                                <SpecialDays>
                                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D" />
                                                </SpecialDays>
                                            </Calendar>
                                        </telerik:RadDatePicker>

                                      
                                    </td>
                               
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
        </div>
    </div>
  
</asp:Content>

