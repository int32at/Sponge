<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShoutboxWebPart.ascx.cs" Inherits="Sponge.WebParts.ShoutboxWebPart" %>

<script src="_layouts/Sponge/scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="_layouts/Sponge/scripts/jquery.sponge.shoutbox-1.0.js" type="text/javascript"></script>
<link type="text/css" rel="stylesheet" href="_layouts/Sponge/styles/sponge.css" />

<div id="sponge_shoutbox_wrapper">
</div>

<script type="text/javascript">
    ExecuteOrDelayUntilScriptLoaded(initSpongeShoutBox, "sp.js");

    function initSpongeShoutBox() {
        $.Shoutbox({
            list: "<%=GetSpongeShoutboxListName()%>",
            container: "#sponge_shoutbox_wrapper",
            autoReload: 0,
            rowLimit: <%=GetRowLimit()%>
        });
    }
</script>
