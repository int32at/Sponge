(function ($) {
    $.chat = function (options) {
        $.chat.settings = $.extend({
            list: "",
            container: "",
            autoReload: 0,
            rowLimit: 10
        }, options || {});

        renderChatControl();
        register();
        reloadPosts();
        autoReloadPosts();

        function renderChatControl() {
            var box = "<div id='sponge_chat_box'/>";
            var msg = "<input name='sponge_chat_usrmsg' type='text' id='sponge_chat_usrmsg' class='sponge_chat_usrmsg' />";
            var send = "<button type='button' name='sponge_chat_submit' id='sponge_chat_submit' class='sponge_chat_submit'>Send</button>";

            var html = box += msg += send;

            $($.chat.settings.container).html(html);
        }

        function register() {
            $("#sponge_chat_submit").click(function () {
                var txt = $("#sponge_chat_usrmsg").val();

                var ctx = SP.ClientContext.get_current();
                var list = ctx.get_web().get_lists().getByTitle($.chat.settings.list);

                var itemCreateInfo = new SP.ListItemCreationInformation();
                this.spongeChatItem = list.addItem(itemCreateInfo);

                this.spongeChatItem.set_item('Message', txt);
                this.spongeChatItem.update();
                ctx.load(this.spongeChatItem);

                ctx.executeQueryAsync(
            	Function.createDelegate(this,
            		function () {
            		    //alert('Item created: ' + this.spongeChatItem.get_id());
            		    SP.UI.Notify.addNotification('Your message has been posted.', false);

            		    //clean default values
            		    $("#sponge_chat_usrmsg").val("");

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
            $("#sponge_chat_box").html("");

            var ctx = SP.ClientContext.get_current();
            var list = ctx.get_web().get_lists().getByTitle($.chat.settings.list);

            var camlQuery = new SP.CamlQuery();
            camlQuery.set_viewXml("<View><Query><OrderBy><FieldRef Name='ID' Ascending='FALSE'/></OrderBy><Where><Geq><FieldRef Name=\'ID\'/>' + '<Value Type=\'Number\'>0</Value></Geq></Where></Query><RowLimit>" + $.chat.settings.rowLimit + "</RowLimit></View>");
            this.spongeChatItems = list.getItems(camlQuery);

            ctx.load(this.spongeChatItems);
            ctx.executeQueryAsync(
            	Function.createDelegate(this,
           		function () {
           		    var listItemInfo = '';
           		    var enumerator = this.spongeChatItems.getEnumerator();

           		    while (enumerator.moveNext()) {
           		        var item = enumerator.get_current();

           		        var msg = createMessageEntry(item);
           		        $("#sponge_chat_box").append(msg);
           		    }
           		}),
				Function.createDelegate(this,
            	function (sender, args) {
            	    alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
            	})
			);
        }

        function autoReloadPosts() {
            if ($.chat.settings.autoReload != "undefined" && $.chat.settings.autoReload > 0)
                setInterval(reloadPosts, $.chat.settings.autoReload);
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

            return "<li id='sponge_chat_entry'>" +
						"<div id='sponge_chat_msg'>" + msg + "</div>" +
						"<div id='sponge_chat_meta'>" + author + "  -  " + date.toLocaleString() + "</div>" +
					"</li>";
        }
    }
})(jQuery);