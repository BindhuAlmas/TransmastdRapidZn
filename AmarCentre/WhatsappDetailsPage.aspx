<%@ Page Title="" Language="C#"  Async="true"
 AutoEventWireup="true"   CodeBehind="WhatsappDetailsPage.aspx.cs"  Inherits ="AmarCentre.WhatsappDetailsPage" %>


<!DOCTYPE html>
<html>
<head runat="server">
    <title>Message Details</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .details-card {
            max-width: 800px;
            margin: 30px auto;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
            border-radius: 8px;
        }
        .details-card .card-header {
            background-color: #0d6efd;
            color: white;
            font-weight: bold;
        }
        .details-label {
            font-weight: bold;
            color: #333;
        }
        .status-success {
            color: green;
            font-weight: bold;
        }
        .status-failed {
            color: red;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="card details-card">
            <div class="card-header">
                Message Details
            </div>
            <div class="card-body">
                <div class="row mb-2">
                    <div class="col-md-4 details-label">Customer Code:</div>
                    <div class="col-md-8"><asp:Label ID="lblCustomerCode" runat="server" /></div>
                </div>
                <div class="row mb-2">
                    <div class="col-md-4 details-label">Customer Name:</div>
                    <div class="col-md-8"><asp:Label ID="lblCustomerName" runat="server" /></div>
                </div>
                
                <div class="row mb-2">
                    <div class="col-md-4 details-label">Recipient Phone Number:</div>
                    <div class="col-md-8"><asp:Label ID="lblRecipientPhoneNumber" runat="server" /></div>
                </div>
                <div class="row mb-2">
                    <div class="col-md-4 details-label">Message Content:</div>
                    <div class="col-md-8"><asp:Label ID="lblMessageContent" runat="server" /></div>
                </div>
                <div class="row mb-2">
                    <div class="col-md-4 details-label">Message Sent Date:</div>
                    <div class="col-md-8"><asp:Label ID="lblSentDate" runat="server" /></div>
                </div>
                <div class="row mb-2">
                    <div class="col-md-4 details-label">Send Status:</div>
                    <div class="col-md-8"><asp:Label ID="lblSendStatus" runat="server" /></div>
                </div>
                <div class="row mb-2">
                    <div class="col-md-4 details-label">Media Type:</div>
                    <div class="col-md-8"><asp:Label ID="lblMediaType" runat="server" /></div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>