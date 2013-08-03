(function ($) {
    $.shoutbox = function (options) {
        $.shoutbox.settings = $.extend({
            list: "",
            container: "",
            autoReload: 0,
            rowLimit: 10
        }, options || {});

        renderShoutboxControl();
        register();
        reloadPosts();
        autoReloadPosts();

        function renderShoutboxControl() {
            var box = "<div id='sponge_shoutbox_box'/>";
            var msg = "<input name='sponge_shoutbox_usrmsg' type='text' id='sponge_shoutbox_usrmsg' class='sponge_shoutbox_usrmsg' />";
            var send = "<button type='button' name='sponge_shoutbox_submit' id='sponge_shoutbox_submit' class='sponge_shoutbox_submit'>Send</button>";

            var html = box += msg += send;

            $($.shoutbox.settings.container).html(html);
        }

        function register() {
            $("#sponge_shoutbox_submit").click(function () {
                var txt = $("#sponge_shoutbox_usrmsg").val();

                var ctx = SP.ClientContext.get_current();
                var list = ctx.get_web().get_lists().getByTitle($.shoutbox.settings.list);

                var itemCreateInfo = new SP.ListItemCreationInformation();
                this.spongeShoutboxItem = list.addItem(itemCreateInfo);

                this.spongeShoutboxItem.set_item('Message', txt);
                this.shoutbox.update();
                ctx.load(this.spongeShoutboxItem);

                ctx.executeQueryAsync(
            	Function.createDelegate(this,
            		function () {
            		    //alert('Item created: ' + this.spongeshoutboxItem.get_id());
            		    SP.UI.Notify.addNotification('Your message has been posted.', false);

            		    //clean default values
            		    $("#sponge_shoutbox_usrmsg").val("");

            		    reloadPosts();
            		}),
            	Function.createDelegate(this,
            		function (sender, args) {
            		    alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
            		})
				);
            });
        }

        function reloadPosts() {
            $("#sponge_shoutbox_box").html("");

            var ctx = SP.ClientContext.get_current();
            var list = ctx.get_web().get_lists().getByTitle($.shoutbox.settings.list);

            var camlQuery = new SP.CamlQuery();
            camlQuery.set_viewXml("<View><Query><OrderBy><FieldRef Name='ID' Ascending='FALSE'/></OrderBy><Where><Geq><FieldRef Name=\'ID\'/>' + '<Value Type=\'Number\'>0</Value></Geq></Where></Query><RowLimit>" + $.shoutbox.settings.rowLimit + "</RowLimit></View>");
            this.spongeShoutboxItems = list.getItems(camlQuery);

            ctx.load(this.spongeShoutboxItems);
            ctx.executeQueryAsync(
            	Function.createDelegate(this,
           		function () {
           		    var listItemInfo = '';
           		    var enumerator = this.spongeShoutboxItems.getEnumerator();

           		    while (enumerator.moveNext()) {
           		        var item = enumerator.get_current();

           		        var msg = createMessageEntry(item);
           		        $("#sponge_shoutbox_box").append(msg);
           		    }
           		}),
				Function.createDelegate(this,
            	function (sender, args) {
            	    alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
            	})
			);
        }

        function autoReloadPosts() {
            if ($.shoutbox.settings.autoReload != "undefined" && $.shoutbox.settings.autoReload > 0)
                setInterval(reloadPosts, $.shoutbox.settings.autoReload);
        }

        function strip(html) {
            var tmp = document.createElement("DIV");
            tmp.innerHTML = html;
            return tmp.textContent || tmp.innerText || "";
        }

        function createMessageEntry(item) {
            var msg = strip(item.get_item('Message'));
            var author = item.get_item("Author").get_lookupValue();
            var date = new Date(item.get_item("Created"));

            return "<li id='sponge_shoutbox_entry'>" +
						"<div id='sponge_shoutbox_msg'>" + msg + "</div>" +
						"<div id='sponge_shoutbox_meta'>" + author + "  -  " + date.toLocaleString() + "</div>" +
					"</li>";
        }
    }
})(jQuery);