<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Customer.Master" AutoEventWireup="true" CodeBehind="CHome.aspx.cs" Inherits="AmarCentre.CHome" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script type="text/javascript">
        $(".nav").click(function () {
            $("#mySidenav").css('width', '70px');
            $("#main").css('margin-left', '70px');
            $(".logo").css('visibility', 'hidden');
            $(".logo span").css('visibility', 'visible');
            $(".logo span").css('margin-left', '-10px');
            $(".icon-a").css('visibility', 'hidden');
            $(".icons").css('visibility', 'visible');
            $(".icons").css('margin-left', '-8px');
            $(".nav").css('display', 'none');
            $(".nav2").css('display', 'block');

        });

        $(".nav2").click(function () {
            $("#mySidenav").css('width', '300px');
            $("#main").css('margin-left', '300px');
            $(".logo").css('visibility', 'visible');
            $(".logo span").css('visibility', 'visible');
            $(".icon-a").css('visibility', 'visible');
            $(".icons").css('visibility', 'visible');
            $(".nav").css('display', 'block');
            $(".nav2").css('display', 'none');

        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main">
        <asp:HiddenField ID="hdnuserid" runat="server" />
        <div class="head">
            <div class="col-div-6">
                <span style="font-size: 30px; cursor: pointer; color: white;" class="nav">&#9776; Dashboard</span>
                <span style="font-size: 30px; cursor: pointer; color: white;" class="nav2">&#9776; Dashboard</span>

            </div>


            <div class="col-div-6">

                <div class="profile">
                    <img src="Images/profiles.png" class="pro-img" />


                    <p>
                        <asp:Label ID="lbl_User_name" runat="server" Font-Size="Large" ForeColor="White"></asp:Label>
                        <asp:HiddenField ID="hdn_language" runat="server" />

                        <%--<span>Designation</span>--%>
                    </p>
                </div>
            </div>
            <div class="clearfix"></div>
        </div>

        <div class="col-div-3">
            <div class="box">
                <p>
                    <asp:Label CssClass="boxlbl" ID="InvoiceCount" runat="server" Text="0"></asp:Label>
                    <br />
                    <span class="boxspan">Total Invoice</span>
                </p>
                <i class="fa-solid fa-file-invoice-dollar box-icon"></i>
            </div>
        </div>
        <div class="col-div-3">
            <div class="box">
                <p>
                    <asp:Label CssClass="boxlbl" ID="ServiceCount" runat="server" Text="0"></asp:Label><br />
                    <span class="boxspan">Total Service</span>
                </p>
                <i class="fa-solid fa-layer-group box-icon"></i>
            </div>
        </div>
        <div class="col-div-3">
            <div class="box">
                <p>
                    <asp:Label CssClass="boxlbl" ID="PendingServiceCount" runat="server" Text="0"></asp:Label><br />
                    <span class="boxspan">Pending Service</span>
                </p>
                <i class="fa-solid fa-hourglass-half box-icon"></i>
            </div>
        </div>
        <div class="col-div-3">
            <div class="box">
                <p>
                    <asp:Label CssClass="boxlbl" ID="TotalPayable" runat="server" Text="0"></asp:Label><br />
                    <span class="boxspan">Total Payable</span>
                </p>
                <i class="fa-solid fa-hand-holding-dollar box-icon"></i><%--image--%>
            </div>
        </div>
        <div class="clearfix"></div>
        <br />
        <br />

        <div class="col-div-8">
            <div class="box-8">
                <div class="content-box" id="divInvoice" runat="server">
                    <p>
                        LATEST INVOICE<span>
                            <asp:Button ID="btnInvoiceListview" runat="server" class="HomeButtons" PostBackUrl="../Customer/InvoiceList.aspx" Text="View All" BorderStyle="None" ForeColor="#ff0066" Font-Bold="true" />

                        </span>
                    </p>
                    <br />
                    <table>
                        <tr>

                            <th>Name
                            </th>
                            <th>Date
                            </th>
                            <th>Amount
                            </th>
                        </tr>

                        <asp:Repeater ID="rptInvoice" runat="server">
                            <ItemTemplate>
                                <tr>

                                    <td><%#Eval("Code")%></td>
                                    <td><%#Eval("Date")%></td>
                                    <td><%#Eval("Amount")%></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>

                </div>
            </div>
        </div>
        <div class="col-div-4">
            <div class="box-4">
                <div class="content-box">

                    <div class="circle-wrap">
                        <div class="circle">
                            <div class="mask full">
                                <div class="fill"></div>
                            </div>
                            <div class="mask half">
                                <div class="fill"></div>
                            </div>
                            <div class="inside-circle">Welcome</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="clearfix"></div>

    </div>
</asp:Content>

