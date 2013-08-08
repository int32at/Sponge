var sponge = window.sponge || {};

sponge.actions = function () {
    var getSelectedItemIds = function () {
        var selectedItems = SP.ListOperation.Selection.getSelectedItems();
        var selectedIds = new Array();

        if (selectedItems.length > 0) {
            for (var i in selectedItems)
                selectedIds.push(selectedItems[i].id);
        }

        var single = GetAttributeFromItemTable(itemTable, "ItemId", "Id");
        if (single != "undefined" && single != null)
            selectedIds.push(single);

        return selectedIds;
    }

    return {
        copyListItems: function () {
            if (!confirm("Are you sure you want to copy?"))
                return;

            var ids = getSelectedItemIds();


            for (var i in ids) {
                sponge.actions.copyListItem(ids[i]);
            }
        },

        copyListItem: function (id) {
            var listId = SP.ListOperation.Selection.getSelectedList();
            var ctx = SP.ClientContext.get_current();
            var list = ctx.get_web().get_lists().getById(listId);

            var item = list.getItemById(id);
            var contentType = item.get_contentType();
            this.fields = contentType.get_fields();

            ctx.load(item);
            ctx.load(this.fields);

            ctx.executeQueryAsync(
            	Function.createDelegate(this,
            		function () {
            		    var itemCreateInfo = new SP.ListItemCreationInformation();
            		    var newItem = list.addItem(itemCreateInfo);
            		    var flds = this.fields.getEnumerator();

            		    while (flds.moveNext()) {
            		        var field = flds.get_current();

            		        if (field.get_readOnlyField())
            		            continue;

            		        var internalName = field.get_internalName();

            		        if (internalName == "ContentType")
            		            internalName = "ContentTypeId";

            		        var value = item.get_item(internalName);

            		        if (internalName == "Title") {
            		            if (value == null) value = "";
            		            value += " (Copy)";
            		        }
            		        //set the field value and update it
            		        newItem.set_item(internalName, value);
            		    }

            		    newItem.update();

            		    ctx.executeQueryAsync(
							Function.createDelegate(this,
								function () {
								    //SP.UI.Notify.addNotification('Item has been successfully copied.', false);
								    var url = location.href;
								    if (0 > url.indexOf('?')) {
								        url += '?InitialTabId=Ribbon%2EListItem\u0026VisibilityContext=WSSTabPersistence';
								    } else if (0 > url.indexOf('InitialTabId')) {
								        url += '\u0026InitialTabId=Ribbon%2EListItem\u0026VisibilityContext=WSSTabPersistence';
								    }
								    SP.Utilities.HttpUtility.navigateTo(url);
								}),
							Function.createDelegate(this,
								function (sender, args) {
								    alert("Copy List Item failed. " + args.get_message() + "\n" + args.get_stackTrace());
								})
						);
            		}),
            	Function.createDelegate(this,
            		function (sender, args) {
            		    alert("Copy List Item failed. " + args.get_message() + "\n" + args.get_stackTrace());
            		})
			);
        },

        multiApproveItems: function () {
            var listId = SP.ListOperation.Selection.getSelectedList();
            var ctx = SP.ClientContext.get_current();
            var list = ctx.get_web().get_lists().getById(listId);

            var ids = getSelectedItemIds();
            for (var i in ids) {
                this.item = list.getItemById(ids[i]);
                this.item.set_item('_ModerationStatus', 0);
                this.item.update();
                ctx.load(this.item);
                ctx.executeQueryAsync(
					Function.createDelegate(this,
						function () {
						}),
					Function.createDelegate(this,
						function (sender, args) {
						    alert("Multi Approve failed. " + args.get_message() + "\n" + args.get_stackTrace());
						})
				);
            }

            SP.UI.Notify.addNotification("Approved all items.", false);
        },

        isMultieApproveItemsEnabled: function () {
            return true;
        }
    }
}();

sponge.config = function () {
    var self = this;
    self.cfg = null;
    return {
        init: function (appName, url, callback) {
            var method = "";
            var data = "";

            //central
            if (url == null || url == "undefined") {
                method = "GetCentralJson";
                data = "{ appName: '" + appName + "'}";
            }
                //relative
            else {
                method = "GetRelativeJson";
                data = "{ appName: '" + appName + "', spongeUrl: '" + url + "'}";
            }

            jQuery.ajax({
                type: "POST",
                url: "_layouts/Sponge/ConfigService.asmx/" + method,
                data: data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var cfg = null;
                    if (msg != "undefined" && msg.d != "undefined") {
                        self.cfg = jQuery.parseJSON(msg.d);
                    }

                    if (callback && typeof (callback) != "undefined") {
                        sponge.common.executeCallback(callback);
                    }
                },
                error: function (msg) {
                    alert("ERROR" + JSON.stringify(msg));
                }
            });
        },

        get: function (name) {
            var result = jQuery.grep(self.cfg.Items, function (e) { return e.Key == name; })

            if (result.length > 0)
                return result[0].Value;
        },

        name: function () {
            return self.cfg.Name;
        }
    }
}();

sponge.common = function () {
    return {
        executeFunctionByName: function (callback, context) {
            var args = null;
            if (arguments.length == 3) args = arguments[2];
            var namespaces = callback.split(".");
            var func = namespaces.pop();

            for (var i = 0; i < namespaces.length; i++) {
                context = context[namespaces[i]];
            }

            var params = [];
            params.push(args);

            return context[func].apply(context, params);
        },

        executeCallback: function (callback, args) {
            if (typeof (callback) == 'function') {
                var params = [];
                params.push(args);
                callback.apply(this, params);
            }
            else {
                if (callback.length > 0)
                    sponge.common.executeFunctionByName(callback, window, args);
            }
        }
    }
}();
