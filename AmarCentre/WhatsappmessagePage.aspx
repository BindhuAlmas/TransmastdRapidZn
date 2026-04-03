<%@ Page Title="" Language="C#"  Async="true"
 AutoEventWireup="true"   CodeBehind="WhatsappmessagePage.aspx.cs"  Inherits ="AmarCentre.WhatsappmessagePage" %>






<!DOCTYPE html>
<html>
<head runat="server">
    <title>WhatsApp Message Status Dashboard</title>

    <!-- Bootstrap + AdminLTE -->
    <link href="~/Styles/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Styles/AdminLTE.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <style>
        .filter-panel {
            background: #fff;
            padding: 15px;
            border-radius: 6px;
            margin-bottom: 20px;
            box-shadow: 0 1px 3px rgba(0,0,0,0.1);
        }
        .summary-box {
            background: #fff;
            padding: 20px;
            border-radius: 6px;
            text-align: center;
            box-shadow: 0 1px 3px rgba(0,0,0,0.1);
            margin-bottom: 20px;
        }
        .summary-value {
            font-size: 26px;
            font-weight: bold;
        }
        .badge-status {
            padding: 5px 10px;
            border-radius: 4px;
            color: #fff;
        }
        .badge-delivered { background: #28a745; }
        .badge-read { background: #17a2b8; }
        .badge-failed { background: #dc3545; }
        .badge-pending { background: #ffc107; color: #000;

        }
        .badge-status.delivered { background-color: #28a745; }
.badge-status.read { background-color: #17a2b8; }
.badge-status.sent { background-color: #007bff; }
.badge-status.failed { background-color: #dc3545; }
/* Pagination Row */
.navigationRow {
    text-align: center;
    padding: 12px 0;
    background: #f9f9f9;
}

/* Page Info Label */
.pageInfo {
    font-size: 14px;
    margin-right: 15px;
    color: #555;
}

/* Pagination Buttons */
.navigationButton {
    display: inline-block;
    padding: 6px 12px;
    margin: 0 3px;
    border: 1px solid #d2d6de;
    background-color: #fff;
    color: #444;
    border-radius: 4px;
    cursor: pointer;
    transition: all 0.2s ease-in-out;
}

.navigationButton:hover {
    background-color: #3c8dbc;
    color: #fff;
    border-color: #367fa9;
}

/* Disabled State */
.navigationButton[disabled] {
    background-color: #eee;
    color: #999;
    cursor: not-allowed;
    border-color: #ddd;
}

/* Page Number Label */
#lbl_page_number {
    font-size: 15px;
    font-weight: bold;
    padding: 0 10px;
    color: #333;
}

/* Page Size Dropdown */
.pageSize {
    padding: 5px 8px;
    margin-left: 10px;
    border-radius: 4px;
    border: 1px solid #d2d6de;
    background-color: #fff;
    color: #444;
}
.trend-up {
    color: #28a745;   /* green */
    font-weight: 600;
}

.trend-down {
    color: #dc3545;   /* red */
    font-weight: 600;
}


    </style>
</head>

<body class="hold-transition sidebar-mini">
<form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
<div class="container mt-4">

    <!-- ✅ FILTER PANEL -->
    <asp:HiddenField ID="hdn_user_id" runat="server" />
    <div class="filter-panel">
        <div class="row">

            <div class="col-md-6">
                <label><b>Account Balance (Remaining Message Credits):</b></label>
                <asp:Label ID="lblremainingmsg" runat="server" style="font-weight:bold;"></asp:Label>
                </div>
            </div>
        <div class="row">

            <div class="col-md-3">
                <label>From Date</label>
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date" />
            </div>

            <div class="col-md-3">
                <label>To Date</label>
                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date" />
            </div>

            <div class="col-md-3">
                <label>Document Type</label>
                <asp:DropDownList ID="ddlDocType" CssClass="form-control" runat="server">
                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
    <asp:ListItem Text="Quotation" Value="Quotation"></asp:ListItem>
    <asp:ListItem Text="Invoice" Value="Invoice"></asp:ListItem>
    <asp:ListItem Text="Receipt" Value="Receipt"></asp:ListItem>
</asp:DropDownList>
            </div>

            <div class="col-md-3">
                <label>Status</label>
                <asp:DropDownList ID="ddlStatus" CssClass="form-control" runat="server" >
                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                    <asp:ListItem Text="Sent" Value="Sent"></asp:ListItem>
                    <asp:ListItem Text="Delivered" Value="Delivered"></asp:ListItem>
                    <asp:ListItem Text="Read" Value="Read"></asp:ListItem>
                    <asp:ListItem Text="Failed" Value="Failed"></asp:ListItem>
                    </asp:DropDownList>
            </div>

        </div>

        <div class="row mt-3">
            <div class="col-md-12 text-right">
                <asp:Button ID="btnSearch" Text="Search" CssClass="btn btn-primary" runat="server" OnClick="btnSearch_Click" />
            </div>
        </div>
    </div>

    <!-- ✅ SUMMARY CARDS -->
    <div class="row">
        <asp:Repeater ID="rptSummary" runat="server">
            <ItemTemplate>
                <div class="col-md-2">
                    <div class="summary-box">
                        <h5><%# Eval("Label") %></h5>
                        <div class="summary-value"><%# Eval("Value") %></div>
                        <div class="<%# Eval("TrendClass") %>"><%# Eval("Trend") %></div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <!-- ✅ MESSAGE TABLE -->
    <%--<div class="card">--%>
        <%--<div class="card-header">
            <h3 class="card-title">Message Status Log</h3>
        </div>--%>

        <div class="card-body table-responsive">
            <%--<asp:GridView ID="gvMessages" runat="server" AutoGenerateColumns="False"
                CssClass="table table-bordered table-striped" 
                OnRowDataBound="gvMessages_RowDataBound"
                OnRowCommand="gvMessages_RowCommand" AllowPaging="true"
    PageSize="10"
    OnPageIndexChanging="gvMessages_PageIndexChanging"
>

                <Columns>
                    <asp:BoundField DataField="MessageSentDate" HeaderText="Date & Time" />
                    <asp:BoundField DataField="CustopmerName" HeaderText="Customer Name" />
                    <asp:BoundField DataField="mobilenumber" HeaderText="Mobile Number" />
                    <asp:BoundField DataField="documenttype" HeaderText="Document Type" />
                    <asp:BoundField DataField="documentdescription" HeaderText="PDF No." />

                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='badge-status <%# Eval("StatusClass") %>'>
                                <%# Eval("Status") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="deliverytime" HeaderText="Delivery Time" />

                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:Button ID="btnView" runat="server" Text="View"
                                CommandName="View" CommandArgument='<%# Eval("MessageId") %>'
                                CssClass="btn btn-sm btn-info" />

                            <asp:Button ID="btnRetry" runat="server" Text="Retry"
                                CommandName="Retry" CommandArgument='<%# Eval("MessageId") %>'
                                CssClass="btn btn-sm btn-warning" />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>--%>
            <asp:UpdatePanel ID="Upd_List_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="Common_order_column" runat="server" />
                <asp:HiddenField ID="Common_asc_desc" runat="server" />
                <%--<div class="list_info" style="display: none">
                </div>--%>
                <table class="table table-bordered table-striped">
                    <thead>
                        <tr>
                            <th style="width: auto;">
                                Date
                            </th>
                            <th style="width: auto;">
                                Customer
                            </th>
                            <th style="width:  auto;">
                                Mobile Number
                            </th>
                             <th style="width:  auto;">
                                Doc.Type
                            </th>
                            <th style="width: auto;">
                                Doc.No
                            </th>
                            
                            <th style="width:  auto;">
                                Status
                            </th>
                            <th  style="width:  auto;">
                                Delivery Time (in sec.)
                            </th>
                            <th  style="width:  auto;">
                                Retry
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="gvMessages2" runat="server" OnItemCommand="gvMessages_ItemCommand" OnItemDataBound="gvMessages_ItemDataBound">
                            <ItemTemplate>
                                <tr runat="server" id="trmainlist">
                                    <td>
                                        <%#Eval("MessageSentDate")%>.
                                        <%--<asp:HiddenField ID="hdn_id" runat="server" Value='<%#Eval("Id")%>' />
                                        <asp:HiddenField ID="hdnDescpCount" runat="server" Value='<%#Eval("DescpCount")%>' />--%>
                                        <asp:HiddenField ID="hdn_MessageId" runat="server" Value='<%#Eval("MessageId")%>' />
                                    </td>
                                    <td>
                                        <%#Eval("CustopmerName")%>
                                    </td>
                                    <td>
                                        <%#Eval("mobilenumber")%>
                                    </td>
                                      <td>
                                        <%#Eval("documenttype")%>
                                    </td>
                                    <td>
                                        <%#Eval("documentdescription")%>
                                    </td>
                                    <td>
                                        <span class='badge-status <%# Eval("StatusClass") %>'>
                                <%# Eval("Status") %>
                            </span>
                                    </td>
                                    <td>
                                        <%#Eval("deliverytime")%>
                                    </td>
                                    <%--<td class="listTableActionButtonDiv">
                                        <asp:Button ID="btn_edit" runat="server" class="btn_edit" />
                                    </td>--%>
                                    <td>
                                        <%--<asp:Button ID="btnView" runat="server" Text="View"
                                CommandName="View" CommandArgument='<%# Eval("MessageId") %>'
                                CssClass="btn btn-sm btn-info" />--%>

                            <asp:Button ID="btnRetry" runat="server" Text="Retry"
                                CommandName="Retry" CommandArgument='<%# Eval("MessageId") %>'
                                CssClass="btn btn-sm btn-warning" Visible='<%# Eval("Status").ToString() == "Failed" ? true : false %>'/>
                                        <asp:Button ID="btnSendAgain" runat="server" Text="Send Again"
                                CommandName="SendAgain" CommandArgument='<%# Eval("MessageId") %>'
                                CssClass="btn btn-sm btn-warning" Visible='<%# Eval("Status").ToString() == "Delivered" || Eval("Status").ToString()=="Read" ? true : false %>' />
    <asp:Button ID="btnDetails" runat="server" Text="Details"
                                CommandName="Details" CommandArgument='<%# Eval("MessageId") %>'
                                CssClass="btn btn-sm btn-warning" Visible='<%# Eval("Status").ToString() == "Failed" ? true : false %>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <%--<tr>
                            <td colspan="8" class="navigationRow">
                                <asp:UpdatePanel ID="Upd_Nav_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lbl_page_info" runat="server" class="pageInfo"></asp:Label>
                                        <asp:Button ID="btn_first" runat="server" class="navigationButton" Text="<<" OnClick="btn_first_Click" />
                                        <asp:Button ID="btn_prev" runat="server" class="navigationButton" Text="<" OnClick="btn_prev_Click" />
                                        <asp:Label ID="lbl_page_number" Style="font-weight: bold; margin-left: 5px; margin-right: 5px;
                                            text-align: center;" runat="server"></asp:Label>
                                        <asp:Button ID="btn_next" class="navigationButton" runat="server" Text=">" OnClick="btn_next_Click" />
                                        <asp:Button ID="btn_last" class="navigationButton" runat="server" Text=">>" OnClick="btn_last_Click" />
                                        <asp:DropDownList ID="drp_count" class="pageSize" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="drp_count_SelectedIndexChanged">
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
                                    
                                </asp:UpdatePanel>
                            </td>
                        </tr>--%>
                    </tbody>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>
    </div>

</div>

</form>
</body>
</html>