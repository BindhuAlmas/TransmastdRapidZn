<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisplayMenu.aspx.cs" Inherits="AmarCentre.Masters.DisplayMenu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hdn_user_id" runat="server" />

        <telerik:RadSkinManager ID="RadSkinManager1" Visible="false" runat="server" ShowChooser="True"
            PersistenceMode="Cookie">
        </telerik:RadSkinManager>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnablePageMethods="true"
            EnableScriptGlobalization="true" EnableScriptLocalization="true">
        </telerik:RadScriptManager>
        <div>
            <telerik:RadTreeView ID="RadTreeView1" runat="server" CheckBoxes="True" TriStateCheckBoxes="true"
                CheckChildNodes="true">
                <Nodes>
                </Nodes>
            </telerik:RadTreeView>
            <br />
            <telerik:RadButton ID="btnSaveHead" runat="server" Text="Save" Width="70px" OnClick="btnSave_Click">
            </telerik:RadButton>
        </div>
    </form>
</body>
</html>
