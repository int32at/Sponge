<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/ButtonSection.ascx" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EventHandlerManager.aspx.cs" Inherits="Sponge.Pages.EventHandlerManager" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table class="propertysheet" border="0" width="100%" cellspacing="0" cellpadding="0">
        <wssuc:inputformsection id="sctListSelection" title="Select List" runat="server">
            <template_description>
                Select the name of the List or Document Library that you want to (un)register Event Handlers.
 			</template_description>
            <template_inputformcontrols>
                <wssuc:InputFormControl runat="server" LabelText="List / Document Library:">
					<Template_Control>
                        <asp:DropDownList ID="ddlLists" runat="server" OnSelectedIndexChanged="ListIndex_Changed"></asp:DropDownList>
                    </Template_Control>
				</wssuc:InputFormControl>
			</template_inputformcontrols>
        </wssuc:inputformsection>
        <wssuc:inputformsection id="sctExistingEventHandlers" title="Select List" runat="server">
            <template_description>
                A list of existing Event Handlers that are already registered on this List or Document Library.
 			</template_description>
            <template_inputformcontrols>
                <wssuc:InputFormControl runat="server" LabelText="Instance Name:">
					<Template_Control>
                        <asp:ListBox ID="lbExistingHandlers" runat="server"></asp:ListBox>
                    </Template_Control>
				</wssuc:InputFormControl>
			</template_inputformcontrols>
        </wssuc:inputformsection>
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
        <wssuc:buttonsection showstandardcancelbutton="false" topbuttons="true" bottomspacing="5" showsectionline="false" runat="server">
            <template_buttons>
				<asp:Button ID="Button1" runat="server" class="ms-ButtonHeightWidth" OnClick="RegisterButton_Click" Text="<%$Resources:wss,multipages_okbutton_text%>" accesskey="<%$Resources:wss,okbutton_accesskey%>"/>
                <asp:Button ID="Button2" runat="server" class="ms-ButtonHeightWidth" OnClick="CancelButton_Click" Text="<%$Resources:wss,multipages_cancelbutton_text%>" accesskey="<%$Resources:wss,cancelbutton_accesskey%>"/>
            </template_buttons>
        </wssuc:buttonsection>
    </table>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Sponge Event Handler Manager
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Sponge Event Handler Manager
</asp:Content>
