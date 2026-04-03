<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="WhatsappDashboard.aspx.cs" Inherits="AmarCentre.WhatsappDashboard" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        html, body {
            height: 100%;
            margin: 0;
        }
        #myFrame {
            width: 100%;
            height: 100%;
            border: none;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdn_user_id" runat="server" />
    <%--<asp:HiddenField ID="hdn_user_id" runat="server" />
     <div style="width:95%;border: 0.5px solid white; padding:1%;margin:1%;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);">
          Customer <span style="color: Red">&nbsp*</span>
                                <telerik:RadComboBox ID="drpCustomer" ClientIDMode="AutoID" Sort="Ascending" EmptyMessage="Search Customer..."
                                    Filter="Contains" AllowCustomText="true" RenderMode="Lightweight" OnClientFocus="OnClientKeyPressing"
                                    OnClientBlur="ValidateCombo" runat="server" Style="height: 24px !important; width: 35%;
                                    overflow: hidden; border: none!important;">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="drpCustomer"
                                    Display="Dynamic" ValidationGroup="save" ErrorMessage="Required" Style="color: Red"
                                    InitialValue=""></asp:RequiredFieldValidator>

           <asp:Button ID="btn_search" ValidationGroup="save" class="butn" runat="server" OnClick="btn_search_Click"
                                        Text="Fill Details" />
     </div>--%>

      <asp:UpdatePanel ID="updFilldetails" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
   
                <div style="position:relative; height:100vh; width:100%;">
    <iframe id="myFrame" runat="server" src="WhatsappmessagePage.aspx"
            style="position:absolute; top:0; left:0; width:100%; height:100%; border:none;">
    </iframe>
</div>


                </ContentTemplate>
          </asp:UpdatePanel>

</asp:Content>
