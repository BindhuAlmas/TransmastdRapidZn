<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Main.Master" AutoEventWireup="true" CodeBehind="WhatsappDashboardEmpty.aspx.cs" Inherits="AmarCentre.WhatsappDashboardEmpty"  Async="true"%>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <%--<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>--%>
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
        .wa-overlay {
    position: fixed;
    inset: 0;
    background: rgba(0,0,0,0.55);
    z-index: 999;
}

.wa-modal {
    position: fixed;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    background: #fff;
    width: 520px;
    padding: 35px 40px;
    border-radius: 10px;
    text-align: center;
    z-index: 1000;
    box-shadow: 0 10px 40px rgba(0,0,0,0.3);
    font-family: Arial, sans-serif;
}

.wa-icon {
    width: 70px;
    height: 70px;
    border-radius: 50%;
    border: 4px solid #3db7f0;
    color: #3db7f0;
    font-size: 36px;
    font-weight: bold;
    display: flex;
    align-items: center;
    justify-content: center;
    margin: 0 auto 15px;
}

.wa-modal h2 {
    margin: 10px 0;
    color: #555;
}

.wa-subtext {
    color: #666;
    font-size: 15px;
    margin-bottom: 25px;
}

.wa-info p {
    margin: 6px 0;
    color: #444;
    font-size: 15px;
}

.wa-buttons {
    margin-top: 30px;
    display: flex;
    justify-content: center;
    gap: 15px;
}

.wa-buttons input {
    padding: 12px 22px;
    border-radius: 5px;
    border: none;
    font-size: 15px;
    cursor: pointer;
    color: #fff;
}

.btn-submit {
    background: #6c63ff;
}

.btn-update {
    background: #e53935;
}

.btn-cancel {
    background: #6c757d;
}
.wa-icon.success {
    border-color: #9bd77d;
    color: #9bd77d;
    font-size: 36px;
}
.edit-overlay {
    position: fixed;
    inset: 0;
    background: rgba(0,0,0,0.5);
    z-index: 999;
}

.edit-modal {
    position: fixed;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    background: #fff;
    width: 420px;
    padding: 30px;
    border-radius: 8px;
    z-index: 1000;
    font-family: Arial, sans-serif;
}

.edit-field {
    margin-bottom: 18px;
}

.edit-field label {
    display: block;
    font-weight: 600;
    margin-bottom: 6px;
    color: #555;
}

.edit-input {
    width: 100%;
    padding: 10px 12px;
    border: 1px solid #ddd;
    border-radius: 5px;
    font-size: 14px;
}

.edit-note {
    font-size: 13px;
    color: #777;
    margin-top: 5px;
}

.edit-buttons {
    display: flex;
    justify-content: center;
    gap: 15px;
    margin-top: 25px;
}

.btn-save {
    background: #6c63ff;
    color: #fff;
    border: none;
    padding: 10px 25px;
    border-radius: 5px;
    cursor: pointer;
}

.btn-cancel {
    background: #6c757d;
    color: #fff;
    border: none;
    padding: 10px 25px;
    border-radius: 5px;
    cursor: pointer;
}



    </style>
        

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        <%--function submitLead() {
            //Swal.fire('Success!', 'asdfasdf', 'success');
            var companyName = $('#ContentPlaceHolder1_hdnCompanyName').val();
            var emailAddress = $('#ContentPlaceHolder1_hdnEmailAddress').val();
            var phoneNumber = $('#ContentPlaceHolder1_hdnPhoneNumber').val();
            var postUrl = $('#ContentPlaceHolder1_hdnWABaseUrl').val() + '/Leads/PostRequest';
            
            $.ajax({
                url: postUrl,
                type: "POST",
                data: {
                    CompanyName: companyName,
                    PhoneNumber: phoneNumber,
                    EmailAddress: emailAddress,
                    Context: "Transmas"
                },
                success: function (response) {
                    //Swal.fire('Success!', response.message, 'success');
                    alert(response.message);
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                    
                }

            });
        }
        function closeSuccess() {
            $('#<%= pnlSuccess.ClientID %>').hide();
            $('#<%= pnlOverlay.ClientID %>').hide();
        }--%>
        function submitLead() {

            var companyName = $('#ContentPlaceHolder1_hdnCompanyName').val();
            var emailAddress = $('#ContentPlaceHolder1_hdnEmailAddress').val();
            var phoneNumber = $('#ContentPlaceHolder1_hdnPhoneNumber').val();
            var postUrl = $('#ContentPlaceHolder1_hdnWABaseUrl').val() + '/Leads/PostRequest';

            $.ajax({
                url: postUrl,
                type: "POST",
                data: {
                    CompanyName: companyName,
                    PhoneNumber: phoneNumber,
                    EmailAddress: emailAddress,
                    Context: "Transmas"
                },
                success: function (response) {

                    // Hide confirm popup
                    $('#<%= pnlConfirm.ClientID %>').hide();

                // Fill success data
                $('#<%= lblSCompany.ClientID %>').text(companyName);
                $('#<%= lblSEmail.ClientID %>').text(emailAddress);
                $('#<%= lblSPhone.ClientID %>').text(phoneNumber);

                // Show success popup
                $('#<%= pnlSuccess.ClientID %>').show();
                $('#<%= pnlOverlay.ClientID %>').show();
            },
            error: function (xhr) {
                alert(xhr.responseText);
            }
        });
    }

    function closeSuccess() {
        $('#<%= pnlSuccess.ClientID %>').hide();
        $('#<%= pnlOverlay.ClientID %>').hide();
        }
        function showConfirmPopup() {
            alert('sdfasdf');
            // get values from hidden fields
            var company = $('#<%= hdnCompanyName.ClientID %>').val();
            var email = $('#<%= hdnEmailAddress.ClientID %>').val();
            var phone = $('#<%= hdnPhoneNumber.ClientID %>').val();

            // set labels text
            $('#<%= lblCompany.ClientID %>').text(company);
        $('#<%= lblEmail.ClientID %>').text(email);
        $('#<%= lblPhone.ClientID %>').text(phone);

        // show popup + overlay
        $('#<%= pnlConfirm.ClientID %>').show();
            <%--$('#<%= pnlOverlay.ClientID %>').show();--%>
            $('#<%= pnlConfirm.ClientID %>').fadeIn(200);
            <%--$('#<%= pnlOverlay.ClientID %>').fadeIn(200);--%>
            alert('sdfasdfasdf');
        }


    </script>
    

      <%--<asp:UpdatePanel ID="updFilldetails" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <ContentTemplate>--%>
        <div style="display:flex; justify-content:center; align-items:center; 
                    height:100vh; width:100%;">
            <div style="width:60%; border:1px solid #ccc; padding:25px; 
                        box-shadow:0 4px 8px rgba(0,0,0,0.1); border-radius:5px; 
                        background-color:#f9f9f9; text-align:center;">
                <p style="font-size:20px; font-weight:500; color:#333; margin-bottom:25px;">
                    Reach your customers instantly with WhatsApp notifications. 
                    This feature is not included in your current plan. 
                    To upgrade and enable WhatsApp messaging, please click 
                    <strong>"Arrange a Demo"</strong>.
                </p>
                <asp:HiddenField ID="hdnCompanyName" runat="server" />
                <asp:HiddenField ID="hdnEmailAddress" runat="server" />
                <asp:HiddenField ID="hdnPhoneNumber" runat="server" />
                <asp:HiddenField ID="hdnWABaseUrl" runat="server" />
                <%--<input type="button" id="btnArrangeDemo"  value="Arrange a Demo" class="butn"
                            style="background-color:#007bff; color:white; padding:12px 24px; 
                                   font-size:16px; border:none; border-radius:4px; cursor:pointer;" onclick="submitLead()"/>--%>
                <asp:Button ID="btn_addnew" runat="server" Text="Arrange a Demo"  OnClick="btn_addnew_Click"   CssClass="butn" style="background-color:#007bff; color:white; padding:12px 24px; 
                                   font-size:16px; border:none; border-radius:4px; cursor:pointer;"/>
            </div>
        </div>
        
    <%--</ContentTemplate>
</asp:UpdatePanel>--%>
    <asp:UpdatePanel ID="Upd_Add_Panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <%--<asp:Panel ID="pnl_add" Visible="false" runat="server">
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
                                </table>
                                </contenttemplate>
                            </asp:UpdatePanel>
                            </div>
                    </asp:Panel>--%>
                <!-- Overlay -->
<!-- Overlay -->
<asp:Panel ID="pnlOverlay" runat="server" CssClass="wa-overlay" Visible="false"></asp:Panel>

<!-- Popup -->
<asp:Panel ID="pnlConfirm" runat="server" CssClass="wa-modal" Visible="false">

    <div class="wa-icon">
        i
    </div>

    <h2>Confirm Contact Information</h2>
    <p class="wa-subtext">
        Please verify the contact information before submitting.
    </p>

    <div class="wa-info">
        <p><strong>Company:</strong> <asp:Label ID="lblCompany" runat="server" /></p>
        <p><strong>Email:</strong> <asp:Label ID="lblEmail" runat="server" /></p>
        <p><strong>Phone:</strong> <asp:Label ID="lblPhone" runat="server" /></p>
    </div>

    <div class="wa-buttons">
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn-submit" OnClick="btnSubmit_Click"/>
        <asp:Button ID="btnUpdate" runat="server" Text="Update Details" CssClass="btn-update" OnClick="btnUpdate_Click"/>
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn-cancel"
            OnClick="btnCancel_Click" />
    </div>

</asp:Panel>
                <!-- Success Popup -->
<asp:Panel ID="pnlSuccess" runat="server" CssClass="wa-modal" Visible="false">

    <div class="wa-icon success">
        ✓
    </div>

    <h2>Request Submitted</h2>

    <p class="wa-subtext">
        Your request has been submitted successfully.
        Our team will contact you shortly.
    </p>

    <div class="wa-info">
        <p><strong>Company:</strong> <asp:Label ID="lblSCompany" runat="server" /></p>
        <p><strong>Email:</strong> <asp:Label ID="lblSEmail" runat="server" /></p>
        <p><strong>Phone:</strong> <asp:Label ID="lblSPhone" runat="server" /></p>
    </div>

    <div class="wa-buttons">
        <asp:Button ID="btnOk" runat="server" Text="OK"
            CssClass="btn-submit"
            OnClientClick="closeSuccess(); return false;" />
    </div>

</asp:Panel>
                <!-- Overlay -->
<asp:Panel ID="pnlEditOverlay" runat="server" CssClass="edit-overlay" Visible="false"></asp:Panel>

<!-- Edit Popup -->
<asp:Panel ID="pnlEdit" runat="server" CssClass="edit-modal" Visible="false">

    <div class="edit-field">
        <label>Company Name</label>
        <asp:TextBox ID="txtCompany" runat="server" CssClass="edit-input" />
    </div>

    <div class="edit-field">
        <label>Email Address</label>
        <asp:TextBox ID="txtEmail" runat="server" CssClass="edit-input" />
    </div>

    <div class="edit-field">
        <label>Phone Number</label>
        <asp:TextBox ID="txtPhone" runat="server" CssClass="edit-input" />
    </div>

    <p class="edit-note">
        Phone or email must be provided.
    </p>

    <div class="edit-buttons">
        <asp:Button ID="btnSaveEdit" runat="server" Text="Save"
            CssClass="btn-save"
            OnClick="btnSaveEdit_Click" />

        <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel"
            CssClass="btn-cancel"
            OnClick="btnCancelEdit_Click" />
    </div>

</asp:Panel>




                </ContentTemplate>
            </asp:UpdatePanel>
        <%--</ContentTemplate>
          </asp:UpdatePanel>--%>
</asp:Content>
