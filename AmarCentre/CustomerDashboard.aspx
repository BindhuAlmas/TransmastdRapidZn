<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="CustomerDashboard.aspx.cs" Inherits="AmarCentre.CustomerDashboard" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .divstyle {
            width: 47%;
            float: left;
            min-height: 82%;
            height: auto;
            text-align: center;
            border: 0.5px solid white;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }

    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdn_user_id" runat="server" />
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
     </div>

      <asp:UpdatePanel ID="updFilldetails" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
    <div class="week_activity divstyle" visible="false" runat="server" id="divAccountSumry">
        <h3 >
            <asp:Label ID="lblSC" Text="Account Summary" runat="server"></asp:Label></h3>
        <asp:Chart ID="Chart1" Width="500px" runat="server" >
            <Series>
                <asp:Series Name="Series1" ChartType="Doughnut">
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1">
                    <AxisX LineColor="Gray">
                        <MajorGrid LineColor="Gray" />
                    </AxisX>
                    <AxisY LineColor="Gray">
                        <MajorGrid LineColor="Gray" />
                    </AxisY>
                    <Area3DStyle LightStyle="Realistic"></Area3DStyle>
                </asp:ChartArea>
            </ChartAreas>
            <Legends>
                <asp:Legend Name="Legend1">
                </asp:Legend>
            </Legends>
        </asp:Chart>
    </div>

    <div class="week_activity divstyle"  visible="false" runat="server" id="divTransSumry">
        <h3 ><asp:Label ID="Label1" Text="Transaction Summary" runat="server"></asp:Label></h3>
        <asp:Chart ID="Chart2" Width="500px" runat="server" >
            <Series>
                <asp:Series Name="Series1" ChartType="Doughnut">
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1">
                    <AxisX LineColor="Gray">
                        <MajorGrid LineColor="Gray" />
                    </AxisX>
                    <AxisY LineColor="Gray">
                        <MajorGrid LineColor="Gray" />
                    </AxisY>
                    <Area3DStyle LightStyle="Realistic"></Area3DStyle>
                </asp:ChartArea>
            </ChartAreas>
            <Legends>
                <asp:Legend Name="Legend1">
                </asp:Legend>
            </Legends>
        </asp:Chart>
    </div>

                <asp:Panel ID="pnlSalesRevenue" class="week_activity divstyle" visible="false" runat="server">
        <h3 ><asp:Label ID="Label2" Text="Revenue Vs Profit" runat="server"></asp:Label></h3>
        <div>
            <asp:Chart ID="SalesRevenueChart" runat="server" Width="500px" Height="400px">
                <Legends>
                    <asp:Legend Name="Legend1" Docking="Top" Alignment="Center"></asp:Legend>
                </Legends>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">
                        <AxisX Title="P"  LineColor="Gray">
                            <MajorGrid LineColor="Transparent" />
                        </AxisX>
                        <AxisY Title="Amount"  LineColor="Gray">
                            <MajorGrid LineColor="Gray" LineDashStyle="NotSet"/>
                        </AxisY>
                    </asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
        </div>

    </asp:Panel>

                 <asp:Panel ID="pnldocumentexpiry" class="week_activity divstyle" visible="false" runat="server">
        <h3 ><asp:Label ID="Label3" Text="Document Expiry" runat="server"></asp:Label></h3>
        <div>
              <asp:Chart ID="ChartDocu" Width="500px" Height="400px" runat="server" Palette="Excel">
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1">
                    <AxisX LineColor="Gray">
                        <MajorGrid LineColor="Gray" LineDashStyle="NotSet" />
                    </AxisX>
                    <AxisY LineColor="Gray">
                        <MajorGrid LineColor="Gray" LineDashStyle="NotSet" />
                    </AxisY>
                </asp:ChartArea>
            </ChartAreas>
            <Legends>
                <asp:Legend Name="Legend1" Docking="Bottom">
                </asp:Legend>
            </Legends>
        </asp:Chart>
        </div>

    </asp:Panel>

                  <asp:Panel ID="pnlDeadline" class="week_activity divstyle" visible="false" runat="server">
        <h3 ><asp:Label ID="Label4" Text="Deadline Transaction" runat="server"></asp:Label></h3>
        <div>
              <asp:Chart ID="ChartDeadline"  Width="500px" Height="400px" runat="server" Palette="Fire">
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1">
                    <AxisX LineColor="Gray">
                        <MajorGrid LineColor="Gray" LineDashStyle="NotSet" />
                    </AxisX>
                    <AxisY LineColor="Gray">
                        <MajorGrid LineColor="Gray" LineDashStyle="NotSet" />
                    </AxisY>
                </asp:ChartArea>
            </ChartAreas>
            <Legends>
                <asp:Legend Name="Legend1" Docking="Bottom">
                </asp:Legend>
            </Legends>
        </asp:Chart>
        </div>

    </asp:Panel>
                <div style="height:10px">
                    <br />
                </div>

                </ContentTemplate>
          </asp:UpdatePanel>

</asp:Content>
