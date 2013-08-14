<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WeatherWebPart.ascx.cs" Inherits="Sponge.WebParts.WeatherWebPart" %>

<script src="_layouts/Sponge/scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<link type="text/css" rel="stylesheet" href="_layouts/Sponge/styles/sponge.webparts.weather.css" />

<div id="weather" class="weatherFeed"></div>

<script type="text/javascript">
    $(document).ready(function() {
        $.getScript("_layouts/Sponge/scripts/jquery.geolocation.js", function() {
            $.geolocation.get({
                win: function(position) {
                    var lat = position.coords.latitude;
                    var lon = position.coords.longitude;

                    var now = new Date();
                    var query = "select woeid from geo.placefinder where text='" + lat + "," + lon + "' and gflags='R'";
                    var api = 'http://query.yahooapis.com/v1/public/yql?q=' + encodeURIComponent(query) + '&rnd=' + now.getFullYear() + now.getMonth() + now.getDay() + now.getHours() + '&format=json&callback=?';

                    $.getJSON(api, function(data) {

                        var result = $.parseJSON(JSON.stringify(data));

                        var woeid = result.query.results.Result.woeid;
                        loadWeatherWidget(woeid);
                    });
                },
                fail: function() {
                    $("#weather").append("<p>Your current browser does not support HTML geo-locations or you did not enable them. Please update to a modern browser.</p>");
                }
            });
        });
        
        function loadWeatherWidget(woeid) {
            $.getScript("_layouts/Sponge/scripts/jquery.zweatherfeed.min.js", function () {
                $('#weather').weatherfeed([woeid], {
                    woeid: true
                });
            });
        }
    });
</script>
