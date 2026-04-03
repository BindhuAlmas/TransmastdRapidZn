<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true"
    CodeBehind="Home.aspx.cs" Inherits="AmarCentre.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /*@import url('https://fonts.googleapis.com/css?family=Quicksand&display=swap');

        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }*/

        h3 {
            font-family: Quicksand;
        }

        .alerts {
            text-align: center;
            font-size: 20px;
            word-spacing: 3px;
            border: 1px solid gray;
            width: 40%;
            padding: 1%;
            border-radius: 15px;
            background-color: #aef93a;
            left: 37%;
            position: fixed;
        }

        .alertspt {
            text-align: center;
            font-size: 20px;
            word-spacing: 3px;
            border: 1px solid gray;
            width: 40%;
            padding: 1%;
            border-radius: 15px;
            left: 37%;
            position: fixed;
        }
        .alertsptexpd {
            text-align: center;
            font-size: 20px;
            word-spacing: 3px;
            width: 41%;
            left: 37%;
            position: fixed;
        }
        .closed {
            border-color: #408e20;
            color: #408e20;
            position: absolute;
            width: 30px;
            height: 30px;
            opacity: 0.5;
            border-width: 2px;
            border-style: solid;
            border-radius: 50%;
            right: 15px;
            top: 25px;
            text-align: center;
            font-size: 1.6em;
            cursor: pointer;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function Close() {
            $('.alerts').fadeOut(700);
        }
        function Closespt() {
            $('.alertspt').fadeOut(700);
        }

    </script>
    <style type="text/css">
        /* Apply css properties to h1 element */
        h1 {
            text-align: center;
        }

        /* Create a container using CSS properties */
        .container {
            top: 5%;
            left: 10%;
            position: relative;
            text-align: center;
            transform: translate(-50%, -50%);
        }

        /* Apply CSS properties to ui-widgets class */
        .ui-widgets1 {
            position: relative;
            display: inline-block;
            width: 3rem;
            height: 3rem;
            border-radius: 9rem;
            margin: 0.4rem;
            border: 0.6rem solid #40d940;
            box-shadow: inset 0 0 7px grey;
            text-align: center;
            box-sizing: border-box;
        }

        .ui-widgets2 {
            position: relative;
            display: inline-block;
            width: 3rem;
            height: 3rem;
            border-radius: 9rem;
            margin: 0.4rem;
            border: 0.6rem solid #40d940;
            box-shadow: inset 0 0 7px grey;
            border-bottom-color: red;
            text-align: center;
            box-sizing: border-box;
        }

        .ui-widgets3 {
            position: relative;
            display: inline-block;
            width: 3rem;
            height: 3rem;
            border-radius: 9rem;
            margin: 0.4rem;
            border: 0.6rem solid #40d940;
            box-shadow: inset 0 0 7px grey;
            border-left-color: red;
            border-top-color: #40d940;
            border-right-color: #40d940;
            border-bottom-color: red;
            text-align: center;
            box-sizing: border-box;
        }

        .ui-widgets4 {
            position: relative;
            display: inline-block;
            width: 3rem;
            height: 3rem;
            border-radius: 9rem;
            margin: 0.4rem;
            border: 0.6rem solid red;
            box-shadow: inset 0 0 7px grey;
            border-top-color: #40d940;
            text-align: center;
            box-sizing: border-box;
        }

        .ui-widgets5 {
            position: relative;
            display: inline-block;
            width: 3rem;
            height: 3rem;
            border-radius: 9rem;
            margin: 0.4rem;
            border: 0.6rem solid red;
            box-shadow: inset 0 0 7px grey;
            text-align: center;
            box-sizing: border-box;
        }

        /*  Apply css properties to the second 
            child of ui-widgets class */
        .ui-widgets:nth-child(2) {
            border-top-color: chartreuse;
            border-right-color: white;
            border-left-color: palegreen;
            border-bottom-color: white;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdn_user_id" runat="server" />

    <asp:Panel ID="pnluser" runat="server" Style="position: relative; top: 13%">
        <div style="text-align: center; font-size: 25px; word-spacing: 3px; text-transform: capitalize">
            <span style="float: left; padding-left: 32%">
                <img alt="" src="Images/hm6.png" width="100%" /></span>
            <span style="float: left; padding-left: 3%">
                <br />
                <span style="color: #08065a">Welcome
                    <asp:Label ID="lbluser" runat="server" Text="USER"></asp:Label><br />
                </span>
                <span style="color: green">Have a Nice Day </span></span>
        </div>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlsup1" runat="server" Style="position: relative; top: 32%">
        <div class="alertspt"  >
            
            <span style="float: left; padding-left: 25%">
                <asp:Label ID="lblsupportalert" Font-Bold="true" runat="server"></asp:Label>
            </span>
            <span style="float: left; padding-left: 5%">
                <asp:Panel ID="pnl1" Visible="false" runat="server" CssClass="ui-widgets1"></asp:Panel>
                <asp:Panel ID="pnl2" Visible="false" runat="server" CssClass="ui-widgets2"></asp:Panel>
                <asp:Panel ID="pnl3" Visible="false" runat="server" CssClass="ui-widgets3"></asp:Panel>
                <asp:Panel ID="pnl4" Visible="false" runat="server" CssClass="ui-widgets4"></asp:Panel>
                <asp:Panel ID="pnl5" Visible="false" runat="server" CssClass="ui-widgets5"></asp:Panel>
            </span>
                
           
        </div>
        <div style="height:120px;"></div>
        <div class="alerts">
            <span style="float: left; padding-left: 4%">
                <img alt="" src="Images/pendvicon.png" width="60%" /></span>
            <span style="float: left; padding-left: 3%">Kindly ensure that you<br />
                perform regular data backups<br />
                to prevent any data loss</span>

        </div>
    </asp:Panel>
      <asp:Panel ID="pnlsup2" runat="server" Style="position: relative; top: 32%">
     <div class="alertsptexpd" style="background-image:url(Images/hm7bg.png);background-repeat:no-repeat;
background-size:cover; height:100px; ">
    <h3> You are not under AMC.<br /> Kindly renew it.</h3> 
     </div>
           <div style="height:120px;"></div>
     <div class="alerts">
         <span style="float: left; padding-left: 4%">
             <img alt="" src="Images/pendvicon.png" width="60%" /></span>
         <span style="float: left; padding-left: 3%">Kindly ensure that you<br />
             perform regular data backups<br />
             to prevent any data loss</span>

     </div>


     
      </asp:Panel>

</asp:Content>
