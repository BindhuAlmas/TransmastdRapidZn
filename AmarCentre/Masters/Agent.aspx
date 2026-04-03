<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true"
    CodeBehind="Agent.aspx.cs" Inherits="AmarCentre.Masters.Agent" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">

        function pageLoad() {

            $('.numbers_only').keydown(function (e) {
                if ($.inArray(e.keyCode, [8, 9, 27, 13, 110, 190]) !== -1 ||
                // Allow: Ctrl+A, Command+A
                    (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                // Allow: home, end, left, right, down, up
                    (e.keyCode >= 35 && e.keyCode <= 40)) {
                    // let it happen, don't do anything
                    return;
                }
                // Ensure that it is a number and stop the keypress
                if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
                }
            });

            /*Read Only*/
            $('.readOnly').attr('readonly', true);

            $('.CommonDiscount').blur(function (e) {
                $('.disAmt').val($('.CommonDiscount').val());
                DiscountCalculation();
            });


            $('.disAmt').blur(function (e) {
                DiscountCalculation();
            });

            function DiscountCalculation() {
                $('.disAmt').each(function () {
                    var Amt = 0;
                    var DisAmt = 0;
                    var AfterDis = 0;
                    if ($(this).closest("tr").find('.amt').val() != '') {
                        Amt = parseFloat($(this).closest("tr").find('.amt').val());
                    }
                    if ($(this).closest("tr").find('.disAmt').val() != '') {
                        DisAmt = parseFloat($(this).closest("tr").find('.disAmt').val());
                    }
                    AfterDis = parseFloat(Amt) - parseFloat(DisAmt);
                    $(this).closest("tr").find('.afterDis').val(parseFloat(AfterDis).toFixed(2));
                });
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="HeadIng_Div">
        Agent/وكيل

        <asp:Button ID="btn_addnew" runat="server" class="btnAddNew" OnClick="btn_newentry_OnClick" />
        <asp:Button ID="btnexcel_export" runat="server" class="btn_excel right_align_list"
            ToolTip="Export to Excel" OnClick="btnexcel_export_OnClick" />
        <div class="searchDiv">
            <asp:TextBox ID="txt_search" runat="server" class="txt_search" AutoPostBack="true"
                OnTextChanged="txt_search_OnTextChanged" placeholder="Search"></asp:TextBox>
        </div>
    </div>
    <div>
        <asp:UpdatePanel ID="Upd_List_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="Common_order_column" runat="server" />
                <asp:HiddenField ID="Common_asc_desc" runat="server" />
                <div class="list_info" style="display: none">
                </div>
                <table class="listTable">
                    <thead>
                        <tr>
                            <th style="width: 5%">
                                Sl No/رقم
                            </th>
                            <th style="width: 15%">
                                Name/اسم
                            </th>
                            <th  style="width: 8%">
                                Mobile/هاتف
                            </th>
                            <th style="width: 9%">
                                Remark/تعليق
                            </th >
                            <th style="width: 5%">
                                Action/عمل
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rpt_list" runat="server" OnItemCommand="rpt_list_OnItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%#Eval("RowNum")%>.
                                        <asp:HiddenField ID="hdn_id" runat="server" Value='<%#Eval("Id")%>' />
                                    </td>
                                    <td>
                                        <%#Eval("Name")%>
                                    </td>
                                    <td>
                                        <%#Eval("Mobile_num")%>
                                    </td>
                                    <td>
                                        <%#Eval("ShortDescription")%>
                                    </td>
                                    <td class="listTableActionButtonDiv">
                                        <asp:Button ID="btn_edit" runat="server" class="btn_edit" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td colspan="5" class="navigationRow">
                                <asp:UpdatePanel ID="Upd_Nav_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lbl_page_info" runat="server" class="pageInfo"></asp:Label>
                                        <asp:Button ID="btn_first" runat="server" class="navigationButton" Text="<<" OnClick="btn_first_OnClick" />
                                        <asp:Button ID="btn_prev" runat="server" class="navigationButton" Text="<" OnClick="btn_prev_OnClick" />
                                        <asp:Label ID="lbl_page_number" Style="font-weight: bold; margin-left: 5px; margin-right: 5px;
                                            text-align: center;" runat="server"></asp:Label>
                                        <asp:Button ID="btn_next" class="navigationButton" runat="server" Text=">" OnClick="btn_next_OnClick" />
                                        <asp:Button ID="btn_last" class="navigationButton" runat="server" Text=">>" OnClick="btn_last_OnClick" />
                                        <asp:DropDownList ID="drp_count" class="pageSize" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="drp_count_OnSelectedIndexChanged">
                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hdn_filter" runat="server" />
                                        <asp:HiddenField ID="hdn_last_page" runat="server" />
                                        <div class="head_second_div" style="display: none">
                                            <asp:HiddenField ID="hdn_total" runat="server" Value="0" />
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnexcel_export" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
        </div>
    </div>
    <div>
        <asp:UpdatePanel ID="Upd_Add_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnl_add" Visible="false" runat="server">
                    <div class="popupBackground">
                    </div>
                    <div class="animated smallPopUp" style="width:45%">
                        <asp:UpdatePanel ID="Upd_Add_PanelInner" runat="server" ChildrenAsTriggers="false"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="Adding_heading">
                                    Agent/وكيل
                                </div>
                                <table class="formTable">
                                    <tr>
                                        <td>
                                            Name/اسم <span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txt_name" CssClass="txt" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txt_name"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            Arabic Name/الاسم بالعربي  </span>
                                            <asp:TextBox ID="txtArabicName" CssClass="txt" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Mobile /هاتف<span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txt_mob" runat="server" class="txt numbers_only"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_mob"
                                                ValidationGroup="save" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            Phone Number/رقم الهاتف 
                                            <br />
                                            <asp:TextBox ID="txt_phn" runat="server" class="txt numbers_only"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Email/البريد الالكتروني 
                                            <br />
                                            <asp:TextBox ID="txt_email" CssClass="txt" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Please Enter Valid Email ID"
                                                ValidationGroup="save" ControlToValidate="txt_email" Style="color: Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                        </td>
                                        <td>
                                            TRN
                                            <br />
                                            <asp:TextBox ID="txt_trn" CssClass="txt" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Address/العنوان 
                                            <br />
                                            <asp:TextBox ID="txt_address" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                        <td>
                                            Remark/تعليق
                                            <br />
                                            <asp:TextBox ID="txt_remark" CssClass="txtarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                         <td>
                                            Profit %
                                            <br />
                                            <asp:TextBox ID="txtprofit" class="txt numbers_only" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div>
                                                <asp:HiddenField ID="hdn_id" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_user_id" runat="server" />
                                                <asp:HiddenField ID="hdn_ServiceCommission_id" runat="server" />
                                                <asp:Button ID="btn_save" class="butn_save" ValidationGroup="save" OnClick="btn_save_OnClick"
                                                    runat="server" Text="Save/حفظ" />
                                                <asp:Button ID="btn_delete" class="butn_delete" runat="server" OnClientClick="javascript : return confirm('Do you really want to Delete.. ?');"
                                                    Visible="false" Text="Delete/حذف" OnClick="btn_delete_OnClick" />
                                                 <asp:Button ID="btn_OB" class="butn_save" runat="server" Visible="false" Text="Opening Balance/الرصيد المفتوح "
                                                    OnClick="btn_OB_OnClick" />
                                                 <asp:Button ID="btn_ServiceCommission" class="butn" runat="server" Text="Service Commission" OnClick="btn_ServiceCommission_OnClick" />
                                                <asp:Button ID="btn_reset" class="butn" runat="server" Text="Reset/إعادة تعيين" OnClick="btn_reset_OnClick" />
                                                <asp:Button ID="Button2" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_OnClick" />
                                                
                                                <asp:HiddenField ID="hdn_add" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_update" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_delete" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_servicecommission" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_OB" runat="server" Value="0" />

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
                            &#10004</div>
                        <div>
                            <asp:Label ID="lbl_msg" runat="server" class="messageLabel"></asp:Label>
                        </div>
                    </div>
                </div>
                 <div>
                    <div id="div1" class="messageAlerterror div_poperror animated" style="display: none" runat="server">
                        <div class="tick">
                            &#10007</div>
                        <div>
                            <asp:Label ID="lblerrormsg" runat="server" class="messageLabel"></asp:Label>
                        </div>
                    </div>
                </div>
                 <asp:UpdatePanel ID="Upd_OB_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnl_obalance" Visible="false" runat="server">
                            <div class="popupBackground">
                            </div>
                            <div class="animated smallPopUp">
                                <div class="Adding_heading">
                                    Opening Balance/الرصيد المفتوح 
                                </div>
                                <table class="formTable">
                                   
                                    <tr>
                                        <td>Payable<span style="color: Red">&nbsp*</span>
                                            <asp:TextBox ID="txt_open_bal" runat="server" class="txt numbers_only"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txt_open_bal"
                                                ValidationGroup="Ob_add" Display="Dynamic" ErrorMessage="Required" Style="color: Red"
                                                InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Date/تاريخ <span style="color: Red">&nbsp*</span>
                                            <telerik:RadDatePicker ID="ob_date" runat="server" DateInput-DateFormat="dd/MM/yyyy">
                                                <Calendar runat="server" ID="Calendar3" CssClass="rtlSupport" ShowOtherMonthsDays="False"
                                                    ShowRowHeaders="False" UseColumnHeadersAsSelectors="False">
                                                    <SpecialDays>
                                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#9D9D9D">
                                                        </telerik:RadCalendarDay>
                                                    </SpecialDays>
                                                </Calendar>
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator ID="Rqd_date" runat="server" ControlToValidate="ob_date"
                                                ValidationGroup="Ob_add" Display="Dynamic" ErrorMessage="Required" Style="color: Red" InitialValue=""></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:Button ID="btn_OBSave" runat="server" class="butn_save" ValidationGroup="Ob_add"
                                                    Text="Save/حفظ" OnClick="btn_OBSave_OnClick" />
                                                <asp:Button ID="btn_close" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_ob_OnClick" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>


            </ContentTemplate>
        </asp:UpdatePanel>
         <asp:UpdatePanel ID="Upd_Service_Detail_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
     <ContentTemplate>
         <asp:Panel ID="pnl_Service_Detail" Visible="false" runat="server">
             <div class="popupBackground">
             </div>
             <div class="animated largePopUp">
                 <div class="Adding_heading">
                     Service Detail/بيانات الخدمة
                 </div>
                 <br />
                 <div class="searchDiv" style="width: 65%">

                    
                     <div style="float: right">
                         <asp:TextBox ID="txtsearchservice" runat="server" class="txt_search " placeholder="Search Service"
                             AutoPostBack="true" OnTextChanged="txtsearchservice_TextChanged"></asp:TextBox>
                     </div>
                 </div>
                 <br />
                 <div style="overflow: auto; max-height: 75%; clear: both">
                     <asp:UpdatePanel ID="updservicelist" runat="server" ChildrenAsTriggers="false"
                         UpdateMode="Conditional">
                         <ContentTemplate>
                             <table class="listTable">
                                 <thead>
                                     <tr>
                                         <th style="width: 5%">Sl No/رقم
                                         </th>
                                         <th style="width: 25%">Service/الخدمات
                                         </th>
                                         <th style="width: 10%">Amount/المبلغ
                                         </th>
                                        
                                         <th style="width: 15%">Commission Amount/المبلغ العمولة
                                         </th>
                                         
                                     </tr>
                                 </thead>
                                 <tbody>
                                     <asp:Repeater ID="rpt_serdetail" runat="server">
                                         <ItemTemplate>
                                             <tr>
                                                 <td>
                                                     <%# Container.ItemIndex + 1 %>
                                                     <asp:HiddenField ID="hdn_AgentSerDetailId" runat="server" Value='<%#Eval("AgentSerDetailId") %>' />
                                                 </td>
                                                 <td>
                                                     <asp:HiddenField ID="hdn_serviceId" runat="server" Value='<%#Eval("ServiceId") %>' />
                                                     <asp:Label ID="lbl_name" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label>
                                                 </td>
                                                 <td>
                                                     <asp:TextBox ID="txt_amt" Class="txt readOnly amt" runat="server" Text='<%#Eval("Price") %>'></asp:TextBox>
                                                 </td>
                                                 <td>
                                                     <asp:TextBox ID="txtCommissionAmount" Class="txt numbers_only" runat="server" Text='<%#Eval("CommissionAmount") %>'></asp:TextBox>
                                                 </td>
                                                
                                             </tr>
                                         </ItemTemplate>
                                     </asp:Repeater>
                                 </tbody>
                             </table>
                         </ContentTemplate>
                     </asp:UpdatePanel>
                 </div>
                 <table class="formTable">
                     <tr>
                         <td>
                             <div>
                                 <asp:Button ID="Button1" runat="server" class="butn_save" ValidationGroup="save_serdetail"
                                     Text="Save/حفظ" OnClick="btn_SDSave_OnClick" />
                                 <asp:Button ID="Button3" class="butn" runat="server" Text="Close/أغلق" OnClick="btn_close_sd_OnClick" />
                             </div>
                         </td>
                     </tr>
                 </table>
             </div>
         </asp:Panel>
     </ContentTemplate>
 </asp:UpdatePanel>

    </div>
</asp:Content>
