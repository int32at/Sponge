<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="wssuc" TagName="LinksTable" Src="/_controltemplates/LinksTable.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="LinkSection" Src="/_controltemplates/LinkSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ActionBar" Src="/_controltemplates/ActionBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" Src="/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" Src="/_controltemplates/ToolBarButton.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="Welcome" Src="/_controltemplates/Welcome.ascx" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SitePropertyManager.aspx.cs" Inherits="Sponge.Layouts.Sponge.SitePropertyManager" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Sponge Site Property Manager
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea"
    runat="server">
    <a href="../settings.aspx">
        <SharePoint:EncodedLiteral ID="EncodedLiteral2" runat="server" Text="<%$Resources:wss,settings_pagetitle%>"
            EncodeMethod="HtmlEncode" /></a>&#32;<SharePoint:ClusteredDirectionalSeparatorArrow
                ID="ClusteredDirectionalSeparatorArrow1" runat="server" />
    Sponge Site Property Manager
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageDescription" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <SharePoint:CssRegistration ID="CssRegistration1" runat="server" Name="core.css" />
    <SharePoint:ScriptLink ID="ScriptLink1" Language="javascript" Name="core.js" runat="server" />
    <SharePoint:ScriptLink ID="ScriptLink2" Language="javascript" Name="/_layouts/Sponge/scripts/jquery-1.7.2.min.js"
        runat="server" />
    <script type="text/javascript" language="javascript">

        // client initialization
        $(document).ready(function () {

            // manage Add/Update button caption
            OnPropertyNameChanged();

            // suppress submit through Enter key
            $("#aspnetForm").keypress(function (e) {
                if (e.which == 13) {
                    return false;
                }
            });
        });

        // sets the specified property name and value
        // in the textboxes where the user can start
        // editing the property immediately
        function SetEditProperty(propertyName, propertyValue) {

            var propertyNameInput = $('[id$="PropertyTextBox"]');
            propertyNameInput.val(propertyName);
            var propertyValueInput = $('[id$="ValueTextBox"]');
            propertyValueInput.val(propertyValue);
            propertyValueInput.focus();
            propertyValueInput.select();
            OnPropertyNameChanged();
        }

        // iterate over the table rows to check if
        // a specific property exists
        function IsPropertyExisting(propertyName) {

            var prefix = "AEE_";

            if ($.trim(propertyName) == '')
                return false;

            var propertyNameWithPrefix = prefix + propertyName;

            var isExisting = false;
            $('[id$="PropertyTable"] tr').each(function () {

                var currentPropertyName = $(this).children().first().text();
                if (($.trim(currentPropertyName.toLowerCase()) == $.trim(propertyName.toLowerCase())) || ($.trim(currentPropertyName.toLowerCase()) == $.trim(propertyNameWithPrefix.toLowerCase()))) {

                    isExisting = true;
                    return false; // break jQuery "each"
                }
            });

            return isExisting;
        }

        // handles changes of the property name textbox
        function OnPropertyNameChanged(e) {

            // sets "Add" as button caption if the entered property
            // does not yet exit otherwise "Update" is set as caption
            var propertyName = $('[id$="PropertyTextBox"]').val();
            if (IsPropertyExisting(propertyName)) {

                $('[id$="AddPropertyButton"]').val('Update');
            }
            else {

                $('[id$="AddPropertyButton"]').val('Add');
            }

            // abort if no event info is available
            if (e == undefined || e == null)
                return;

            // set focus to value input if Enter is pressed
            if (e.keyCode == 13) {

                $('[id$="ValueTextBox"]').focus();
            }
        }

        // handles changes of the property value textbox
        function OnPropertyValueChanged(e) {

            if (e == undefined || e == null)
                return;

            // simulate Add/Update click in case
            // Enter was pressed
            if (e.keyCode == 13) {

                __doPostBack('<%= AddPropertyButton.UniqueID %>', '')
            }
        }

    </script>
    <style type="text/css">
        .sponge-logo
        {
            background-image: url("/_layouts/images/sponge/logo_s.png");
            background-repeat: no-repeat;
            background-position: left top;
        }

        .sponge-clearerrorbutton
        {
            float: right;
            margin-top: 5px;
        }

        .sponge-propertytable
        {
            min-width: 450px;
        }

        .ms-secondary-title
        {
            padding-bottom: 3px;
            display: block;
        }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table class="propertysheet sponge-logo" border="0" width="100%" cellspacing="0" cellpadding="0">
        <wssuc:ButtonSection ShowStandardCancelButton="false" TopButtons="true" BottomSpacing="5"
            ShowSectionLine="false" runat="server">
            <template_buttons>
				<asp:Button runat="server" class="ms-ButtonHeightWidth" OnClick="OKButton_Click" Text="<%$Resources:wss,multipages_okbutton_text%>" id="topOKButton" accesskey="<%$Resources:wss,okbutton_accesskey%>"/>
				<asp:Button runat="server" class="ms-ButtonHeightWidth" OnClick="CancelButton_Click" Text="<%$Resources:wss,multipages_cancelbutton_text%>" id="topCancelButton" accesskey="<%$Resources:wss,cancelbutton_accesskey%>"/>
            </template_buttons>
        </wssuc:ButtonSection>
        <wssuc:InputFormSection ID="PropertySection" Title="Custom Properties" runat="server">
            <template_inputformcontrols>
				<wssuc:InputFormControl runat="server">
					<Template_Control>
                        <asp:Label ID="lblProperty" Text="Property:" runat="server" Width="60" />
                        <asp:TextBox ID="PropertyTextBox" OnKeyPress="OnPropertyNameChanged(event);" OnKeyUp="OnPropertyNameChanged();" OnChange="OnPropertyNameChanged();" runat="server" Width="250"/>
                        <asp:Button ID="AddPropertyButton" OnClick="AddPropertyButton_Click" Text="Add" runat="server" Width="80" class="ms-ButtonHeightWidth"/><br />
                        <asp:Label ID="lblValue" Text="Value:" runat="server" Width="60" />
                        <asp:TextBox ID="ValueTextBox" OnKeyPress="OnPropertyValueChanged(event);" runat="server" Width="250"/><br /><br />
                        <asp:Table ID="PropertyTable" runat="server" CssClass="sponge-propertytable" CellPadding="2" CellSpacing="0" BorderWidth="1" BorderStyle="Solid" BackColor="#F9F9F9">
                            <asp:TableHeaderRow ID="PropertyTableHeaderRow" runat="server">
                                <asp:TableCell ID="TableCell1" runat="server" BorderWidth="1" BorderStyle="Dashed" BorderColor="#dbddde" >
                                    <div style="color:#0072bc;font-weight:bold;margin:3px">Property</div>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell2" runat="server" BorderWidth="1" BorderStyle="Dashed" BorderColor="#dbddde" >
                                    <div style="color:#0072bc;font-weight:bold;margin:3px">Value</div>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell3" runat="server" BorderWidth="1" BorderStyle="Dashed" BorderColor="#dbddde" >                                    
                                </asp:TableCell>                            
                            </asp:TableHeaderRow>
                        </asp:Table>         
                    </Template_Control>
				</wssuc:InputFormControl>
			</template_inputformcontrols>
        </wssuc:InputFormSection>
    </table>
    <SharePoint:DelegateControl ID="DelegateControl1" runat="server" ControlId="NavigationSettingsPanel1"
        Scope="Site" />
    <SharePoint:DelegateControl ID="DelegateControl2" runat="server" ControlId="NavigationSettingsPanel2"
        Scope="Web" />
    <wssuc:ButtonSection ShowStandardCancelButton="false" runat="server">
        <template_buttons>
				<asp:Button runat="server" class="ms-ButtonHeightWidth" OnClick="OKButton_Click" Text="<%$Resources:wss,multipages_okbutton_text%>" id="bottomOKButton" accesskey="<%$Resources:wss,okbutton_accesskey%>"/>
				<asp:Button runat="server" class="ms-ButtonHeightWidth" OnClick="CancelButton_Click" Text="<%$Resources:wss,multipages_cancelbutton_text%>" id="bottomCancelButton" accesskey="<%$Resources:wss,cancelbutton_accesskey%>"/>
			</template_buttons>
    </wssuc:ButtonSection>
    <SharePoint:FormDigest ID="FormDigest1" runat="server" />
</asp:Content>
