<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Landing.aspx.cs" Inherits="AmarCentre.Landing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%@ register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<head id="Head1" runat="server">
    <link rel="shortcut icon" href="Images/Favicon.ico" />
    <link rel="icon" href="Images/Favicon.ico" type="image/png" />
    <link href="Styles/WOM.css" rel="stylesheet" type="text/css" />
    <link href="Styles/Login.css" rel="stylesheet" type="text/css" />
    <title>TRANSMAS - Our first step to customer</title>
    <style type="text/css">
        body
        {
        }
        .unme
        {
            height: 30px;
            width: 80%;
            border: none;
            border-radius: 10px;
            padding-left: 2%;
            display: block;
            margin-top: 10px;
            margin-left: 10%;
           /* background-color:#191123e0;*/
        }
        .lblUname
        {
           margin-left: 10%;
    margin-bottom: 10px;
    font-size: 16px;
    color: #24b6e3;
    font-weight: bold;
        }
        .btn
        {
            background-color: #0aaadb;
            border: medium none;
            color: white;
            height: 30px;
            width:30%;
            margin-top: 19px;
            margin-bottom: 10%;
            border-radius: 10px;
            margin-left: 10%;
            font-weight:bold;
        }
        
        .btn:hover
        {
            background-color: #0d7cc1;
            color: gold;
        }
.glow-border {
   padding: 2px;
  border-radius: 18px;
  background: linear-gradient(
    135deg,#00eaffa3, #7f3cff9e, #ff3cf29e
  );
  box-shadow:
    0 0 15px rgba(127, 60, 255, 0.6),
    0 0 35px rgba(0, 234, 255, 0.4);
  width: 25%;position: fixed;left: 37%;top: 20%;
}
.login-box img {
  max-width: 200px;
  height: auto;
  display: block;
  margin: 0 auto;
}
body {
  background: url('Images/bgnew.png') no-repeat center center;
  background-size: cover;
}
    </style>
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function ToggleDiv() {
            $('.div_pop:hidden').show();
            setTimeout(function () { $(".div_pop").hide(); }, 1000);
        }
        function pageLoad() {

        }

    </script>
</head>
<body>
     <%--<form id="form1" runat="server" style="background-image: url(Images/bgnw.jpg); height: 100%;--%>
     <form id="form1" runat="server" style="background-image: url(Images/bgnew.png); height: 100%;
    background-size: 100% 100%; background-repeat: no-repeat">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
<asp:Label ID="lblVE" runat="server" ForeColor="White" ></asp:Label>
        <div  class="glow-border">
           <div >
    <div id="div_login" runat="server" visible="true" style="border-radius: 10px;padding:1px;
    color: white;background-color: #200d3899;"> 
        <img src="Images/logotrans.png" style="margin-top: 2px; height: 100%; width: 100%;" />
        <table class="formTable" style="width: 100%;">
            <tr>
                <td>
                    <asp:Label ID="lbl_names" runat="server" CssClass="lblUname">User Name</asp:Label>
                    <asp:TextBox ID="txt_Uname" runat="server" class="unme"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" CssClass="lblUname">Password</asp:Label>
                    <asp:TextBox ID="txt_pass" TextMode="Password" runat="server" class="unme"></asp:TextBox>
                </td>
            </tr>
            <tr style="display:none">
                <td>
                    <asp:Label ID="Label2" runat="server" CssClass="lblUname">Language</asp:Label>
                    <telerik:RadComboBox ID="drpLanguage" Sort="Ascending" Filter="Contains" runat="server"
                        AllowCustomText="true" RenderMode="Lightweight" EmptyMessage="Search Language..."
                        Style="overflow: hidden; width: 80%; padding-left: 10%; border: none!important;">
                        <Items>
                            <telerik:RadComboBoxItem Value="1" Text="English" Selected="true" />
                            <telerik:RadComboBoxItem Value="2" Text="Arabic" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btn_Submit" runat="server" Text="SUBMIT" OnClick="btn_login_OnClick"
                        class="btn" />
                </td>
            </tr>
        </table>
    </div>
            </div></div>
     <div style="text-align: center; color: White; font-size: large; position: fixed; top: 90%;
        font-size: 12px; left: 39%;">
        <br />
        <b>Copyrights © 2026 <asp:HyperLink ID="HyperLink1" runat="server" Font-Underline="false" ForeColor="White" BorderStyle="None" NavigateUrl="http://www.almasit.ae">Almas IT Infrastructure</asp:HyperLink> . All rights reserved</b>
    </div>
    </form>
</body>
</html>
