<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Get.aspx.cs" Inherits="Sponge.AdminPages.CorrelationViewer.Get" DynamicMasterPageFile="~masterurl/default.master" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/ButtonSection.ascx" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
 <style>
 .s4-specialNavLinkList
 {
    display:none !important;
 }
 </style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<div style="text-align: right; padding: .2em;"><a href="http://bramnuyts.wordpress.com" target="_blank">http://bramnuyts.wordpress.com</a></div>
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="ms-descriptiontext">
    <wssuc:InputFormSection ID="GetCorrelationIdSection" runat="server" Title="Correlation ID"
        Description="Specify the correlation ID which will be queried to the farm.">
        <template_inputformcontrols>
                <wssuc:InputFormControl ID="GetCorrelationIdControl" runat="server" LabelText="Correlation ID:" LabelAssociatedControlId="txt_CorrelationID">
                    <Template_Control>
                        <asp:TextBox ID="txt_CorrelationID" runat="server" Width="100%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="txt_CorrelationID_ReqValidation" runat="server" ErrorMessage="You must specify a value for this field." ControlToValidate="txt_CorrelationID" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="txt_CorrelationID_IsGuidValidation" runat="server" ErrorMessage="You must enter a valid GUID." ControlToValidate="txt_CorrelationID" Display="Dynamic" ValidationExpression="^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$"></asp:RegularExpressionValidator>
                    </Template_Control>
                </wssuc:InputFormControl>
            </template_inputformcontrols>
    </wssuc:InputFormSection>
    <wssuc:InputFormSection ID="StartDateSection" runat="server" Title="Start Date"
        Description="Specify the start date for the query. Defaults to today minus 1 hour.">
        <template_inputformcontrols>
                <wssuc:InputFormControl ID="StartDateControl" runat="server" LabelText="Start Date:" LabelAssociatedControlId="date_StartDate">
                    <Template_Control>
                        <SharePoint:DateTimeControl ID="date_StartDate" runat="server" IsRequiredField="True" AutoPostBack="True"></SharePoint:DateTimeControl>
                    </Template_Control>
                </wssuc:InputFormControl>
            </template_inputformcontrols>
    </wssuc:InputFormSection>
    <wssuc:InputFormSection ID="EndDateSection" runat="server" Title="End Date"
        Description="Specify the end date for the query. Defaults to today. Warning: a big gap between the start and end date might lead to a very long querytime.">
        <template_inputformcontrols>
                <wssuc:InputFormControl ID="EndDateControl" runat="server" LabelText="End Date:" LabelAssociatedControlId="date_EndDate">
                    <Template_Control>
                        <SharePoint:DateTimeControl ID="date_EndDate" runat="server" IsRequiredField="True" AutoPostBack="True"></SharePoint:DateTimeControl>
                    </Template_Control>
                </wssuc:InputFormControl>
            </template_inputformcontrols>
    </wssuc:InputFormSection>
    <wssuc:ButtonSection runat="server">
        <template_buttons>
			<asp:Button UseSubmitBehavior="false" runat="server" class="ms-ButtonHeightWidth" Text="OK" id="BtnSubmitBottom" Enabled="true" OnClick="Btn_Ok_Click" />
		</template_buttons>
    </wssuc:ButtonSection>
</table>
    <asp:Label ID="lbl_Status" runat="server"></asp:Label>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Get Correlation ID Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
Get Correlation ID
</asp:Content>