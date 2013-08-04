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