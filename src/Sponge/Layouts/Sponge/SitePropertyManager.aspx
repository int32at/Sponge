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

<asp:Content ID="cnt1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Sponge Site Property Manager
</asp:Content>
<asp:Content ID="cnt2" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Sponge Site Property Manager
</asp:Content>
<asp:Content ID="cnt3" ContentPlaceHolderID="PlaceHolderPageDescription" runat="server">
</asp:Content>
<asp:Content ID="cnt4" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <SharePoint:CssRegistration ID="coreCss" runat="server" Name="core.css" />  
    <SharePoint:ScriptLink ID="coreJs" Language="javascript" Name="core.js" runat="server" />

    <!-- SPONGE REGISTRATION -->
    <SharePoint:ScriptLink ID="jQuery"  runat="server" Language="javascript" Name="/_layouts/Sponge/scripts/jquery-1.7.2.min.js"/>

    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            OnPropertyNameChanged();
            $("#aspnetForm").keypress(function (e) {
                if (e.which == 13) {
                    return false;
                }
            });
        });

        function SetEditProperty(propertyName, propertyValue) {

            var propertyNameInput = $('[id$="PropertyTextBox"]');
            propertyNameInput.val(propertyName);
            var propertyValueInput = $('[id$="ValueTextBox"]');
            propertyValueInput.val(propertyValue);
            propertyValueInput.focus();
            propertyValueInput.select();
            OnPropertyNameChanged();
        }

        function IsPropertyExisting(propertyName) {

            var prefix = "";

            if ($.trim(propertyName) == '')
                return false;

            var propertyNameWithPrefix = prefix + propertyName;

            var isExisting = false;
            $('[id$="PropertyTable"] tr').each(function () {

                var currentPropertyName = $(this).children().first().text();
                if (($.trim(currentPropertyName.toLowerCase()) == $.trim(propertyName.toLowerCase())) || ($.trim(currentPropertyName.toLowerCase()) == $.trim(propertyNameWithPrefix.toLowerCase()))) {

                    isExisting = true;
                    return false;
                }
            });

            return isExisting;
        }

        function OnPropertyNameChanged(e) {

            var propertyName = $('[id$="PropertyTextBox"]').val();
            if (IsPropertyExisting(propertyName)) {

                $('[id$="bttnAdd"]').val('Update');
            }
            else {

                $('[id$="bttnAdd"]').val('Add');
            }

            if (e == undefined || e == null)
                return;

            if (e.keyCode == 13) {

                $('[id$="ValueTextBox"]').focus();
            }
        }

        function OnPropertyValueChanged(e) {

            if (e == undefined || e == null)
                return;

            if (e.keyCode == 13) {
                __doPostBack('<%= bttnAdd.UniqueID %>', '')
            }
        }

    </script>
</asp:Content>
<asp:Content ID="cnt5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table class="properties" border="0" width="100%" cellspacing="0" cellpadding="0">
        <wssuc:ButtonSection ShowStandardCancelButton="false" TopButtons="true" BottomSpacing="5"
            ShowSectionLine="false" runat="server">
            <template_buttons>
				<asp:Button runat="server" class="ms-ButtonHeightWidth" OnClick="OKButton_Click" Text="<%$Resources:wss,multipages_okbutton_text%>" id="topOKButton" accesskey="<%$Resources:wss,okbutton_accesskey%>"/>
				<asp:Button runat="server" class="ms-ButtonHeightWidth" OnClick="CancelButton_Click" Text="<%$Resources:wss,multipages_cancelbutton_text%>" id="topCancelButton" accesskey="<%$Resources:wss,cancelbutton_accesskey%>"/>
            </template_buttons>
        </wssuc:ButtonSection>
        <wssuc:InputFormSection ID="propertySection" Title="Custom Site Properties" runat="server">
            <template_inputformcontrols>
				<wssuc:InputFormControl runat="server">
					<Template_Control>
                        <asp:Label ID="lblProperty" Text="Property:" runat="server" Width="60" />
                        <asp:TextBox ID="txtKey" OnKeyPress="OnPropertyNameChanged(event);" OnKeyUp="OnPropertyNameChanged();" OnChange="OnPropertyNameChanged();" runat="server" Width="250"/>
                        <asp:Button ID="bttnAdd" OnClick="Add_Click" Text="Add" runat="server" Width="80" class="ms-ButtonHeightWidth"/><br />
                        <asp:Label ID="lblValue" Text="Value:" runat="server" Width="60" />
                        <asp:TextBox ID="txtValue" OnKeyPress="OnPropertyValueChanged(event);" runat="server" Width="250"/><br /><br />
                        <asp:Table ID="propertyTable" runat="server" CssClass="sponge-spm-table" CellPadding="2" CellSpacing="0" BorderWidth="1" BorderStyle="Solid" BackColor="#F9F9F9">
                            <asp:TableHeaderRow ID="propertyTableHeaderRow" runat="server">
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
    <wssuc:ButtonSection ShowStandardCancelButton="false" runat="server">
        <template_buttons>
				<asp:Button runat="server" class="ms-ButtonHeightWidth" OnClick="OKButton_Click" Text="<%$Resources:wss,multipages_okbutton_text%>" id="bottomOKButton" accesskey="<%$Resources:wss,okbutton_accesskey%>"/>
				<asp:Button runat="server" class="ms-ButtonHeightWidth" OnClick="CancelButton_Click" Text="<%$Resources:wss,multipages_cancelbutton_text%>" id="bottomCancelButton" accesskey="<%$Resources:wss,cancelbutton_accesskey%>"/>
			</template_buttons>
    </wssuc:ButtonSection>
    <SharePoint:FormDigest ID="digest" runat="server" />
</asp:Content>
