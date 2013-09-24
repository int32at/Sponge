ExecuteOrDelayUntilScriptLoaded(sponge, "sp.js");

function sponge() {
    sponge.common = function () {
        return {
            executeCallback: function (callback) {
                if (callback != null && typeof callback !== "undefined")
                    callback();
            },

            isArray: function (object) {
                return toString.call(object) === "[object Array]";
            },

            encodeXmlString: function (txt) {
                txt = String(txt);
                txt = jQuery.trim(txt);
                txt = txt.replace(/&/g, "&amp;");
                txt = txt.replace(/</g, "&lt;");
                txt = txt.replace(/>/g, "&gt;");
                txt = txt.replace(/'/g, "&apos;");
                txt = txt.replace(/"/g, "&quot;");

                return txt;
            },

            format: function () {
                var s = arguments[0];
                for (var i = 0; i < arguments.length - 1; i++) {
                    var reg = new RegExp("\\{" + i + "\\}", "gm");
                    s = s.replace(reg, arguments[i + 1]);
                }

                return s;
            },

            bytesToSize: function (bytes, precision) {
                var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
                var posttxt = 0;
                if (bytes == 0) return 'n/a';
                if (bytes < 1024) {
                    return Number(bytes) + " " + sizes[posttxt];
                }
                while (bytes >= 1024) {
                    posttxt++;
                    bytes = bytes / 1024;
                }
                return bytes.toPrecision(precision) + " " + sizes[posttxt];
            }
        };
    }();

    sponge.ctx = function () {
        var self = this;
        self.ctx = new SP.ClientContext.get_current();
        self.web = ctx.get_web();

        return {
            current: self.ctx,
            web: self.web,

            execute: function (success, error) {
                self.ctx.executeQueryAsync(
                    Function.createDelegate(this, function () {
                        sponge.common.executeCallback(success);
                    }),
                    Function.createDelegate(this, function (sender, args) {
                        sponge.common.executeCallback(error(sender, args));
                    })
                );
            },

            load: function (objects, properties, success, error) {
                if (typeof objects !== "undefined") {
                    if (!sponge.common.isArray(objects)) {
                        if (properties !== null) {
                            self.ctx.load(objects, properties);
                        }
                        else {
                            self.ctx.load(objects);
                        }
                    }
                    else {
                        //load multiple objects at once
                        for (var i = 0; i < objects.length; i++) {
                            if (typeof objects[i].fields !== "undefined") {
                                self.ctx.load(objects[i].data, objects[i].fields);
                            }
                            else {
                                self.ctx.load(objects[i]);
                            }
                        }
                    }
                }

                sponge.ctx.execute(success, error);
            }
        };
    }();

    sponge.user = function () {
        return {
            loadCurrentUser: function (succcess, error) {
                var usr = sponge.ctx.web.get_currentUser();

                sponge.ctx.load(usr, null,
                   function () {
                       sponge.common.executeCallback(succcess(usr));
                   },
                   function (sender, args) {
                       sponge.common.executeCallback(error(sender, args));
                   }
               );
            }
        };
    }();

    sponge.lists = function () {
        return {
            getCurrentList: function () {
                var listId = SP.ListOperation.Selection.getSelectedList(sponge.ctx);

                if (listId !== null && listId !== "undefined") {
                    return sponge.ctx.web.get_lists().getById(listId);
                }
            },

            getDefaultQuery: function () {
                var camlQuery = new SP.CamlQuery();
                camlQuery.set_viewXml('<View><Query><Where><Geq><FieldRef Name=\'ID\'/>' +
                 '<Value Type=\'Number\'>1</Value></Geq></Where></Query><RowLimit>0</RowLimit></View>');
                return camlQuery;
            },

            getSelectedItemIds: function () {
                var selected = new Array();

                var items = SP.ListOperation.Selection.getSelectedItems(sponge.ctx);
                for (var i = 0; i < items.length; i++) {
                    selected.push(items[i].id);
                }

                var singleId = GetAttributeFromItemTable(itemTable, "ItemId", "Id");

                if (singleId !== null) {
                    selected.push(singleId);
                }

                return selected;
            },

            getSelectedItems: function () {
                var selected = new Array();
                var list = sponge.lists.getCurrentList();
                var items = sponge.lists.getSelectedItemIds();

                if (typeof list === "undefined")
                    return selected;

                for (var i = 0; i < items.length; i++) {
                    selected.push({ data: list.getItemById(items[i]) });
                }

                return selected;
            },

            loadSelectedItems: function (success, error) {
                var items = sponge.lists.getSelectedItems();
                sponge.ctx.load(items, null,
                    function () {
                        var array = new Array();

                        for (var i = 0; i < items.length; i++) {
                            array.push(items[i].data);
                        }
                        sponge.common.executeCallback(success(array));
                    },
                    function (sender, args) {
                        sponge.common.executeCallback(error(sender, args));
                    }
                );
            },

            loadCurrentList: function (success, error) {
                var list = sponge.lists.getCurrentList();

                sponge.ctx.load(list, null,
                    function () {
                        sponge.common.executeCallback(success(list));
                    },
                    function (sender, args) {
                        sponge.common.executeCallback(error(sender, args));
                    }
                );
            },

            createList: function (name, type) {
                listInfo = new SP.ListCreationInformation();
                listInfo.set_title(name);
                listInfo.set_templateType(type);
                list = sponge.ctx.web.get_lists().add(listInfo);
                return list;
            }
        };
    }();

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
                var fields = this.fields;
                ctx.executeQueryAsync(
                    Function.createDelegate(this,
                        function () {
                            var itemCreateInfo = new SP.ListItemCreationInformation();
                            var newItem = list.addItem(itemCreateInfo);
                            var flds = fields.getEnumerator();

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

    sponge.logging = function () {
        return {

            log: function (lvl, msg, url) {
                var data = "";
                var method = "";

                if (url == null || url == "undefined") {
                    method = "LogCentral";
                    data = "{ lvl: '" + lvl + "', msg: '" + msg + "'}";
                }
                else {
                    method = "LogRelative";
                    data = "{ spongeUrl: '" + url + "', lvl: '" + lvl + "', msg: '" + msg + "'}";
                }
                jQuery.ajax({
                    type: "POST",
                    url: "_layouts/Sponge/LoggingService.asmx/" + method,
                    data: data,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                    },
                    error: function (msg) {
                        alert("ERROR" + JSON.stringify(msg));
                    }
                });
            }
        }
    }();

    sponge.config = function () {
        var self = this;
        self.cfg = null;
        return {
            init: function (appName, url, callback) {
                var method = '';
                var data = '';

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
                var result = jQuery.grep(self.cfg.Items, function (e) { return e.Key == name; });

                if (result.length > 0)
                    return result[0].Value;
            },

            name: function () {
                return self.cfg.Name;
            }
        }
    }();

    sponge.forms = function () {

        function field(data, name, type) {
            self = this;
            self.data = data;
            self.name = name;
            self.type = type;

            return {
                data: self.data,
                name: self.name,

                type: function () {
                    var id = self.data.attr("id");

                    if (typeof id === "undefined")
                        return self.type;

                    return id.substr(id.lastIndexOf("_") + 1, id.length)
                },

                row: function () {
                    var row = undefined;

                    switch (this.type()) {
                        case "DateTimeFieldDate":
                            //wtf right?
                            row = self.data.closest("tr").parent().parent().parent().parent().parent()
                            break;

                        case "upLevelDiv":
                            //love those nested sharepoint controls...
                            row = self.data.closest("tr").parent().parent().parent().parent().parent().parent().parent().parent().parent().parent();
                            break;

                        case "SelectResult":
                            row = self.data.closest("tr").parent().parent().parent().parent().parent();
                            break;

                        case "MultiCheckBox":
                            row = self.data.closest("tr").parent().parent().parent().parent().parent();
                            break;

                        default:
                            row = self.data.closest("tr");
                    }

                    return row;
                },

                disable: function () {
                    self.data.attr("disabled", "disabled");
                },

                hide: function () {
                    this.row().hide();
                },

                show: function () {
                    this.row().show();
                },

                get: function () {

                    var value = undefined;

                    switch (this.type()) {
                        case "Lookup":
                            var option = $("option:selected", self.data);
                            value = { id: option.val(), text: option.text() };
                            break;

                        case "SelectResult":
                            var options = new Array();
                            $("option", self.data).each(function (i, o) {
                                o = $(o);

                                options.push({ id: o.val(), text: o.text() });
                            });
                            value = options;
                            break;

                        case "BooleanField":
                            var checked = self.data.attr("checked");
                            value = typeof checked != "undefined";
                            break;

                        case "DateTimeFieldDate":
                            value = new Date(self.data.val());
                            break;

                        case "upLevelDiv":
                            value = self.data.text();
                            break;

                        case "MultiCheckBox":
                            var options = new Array();
                            $(self.data).each(function (i, o) {
                                o = $(o);
                                var checked = typeof $("input", o).attr("checked") !== "undefined";

                                options.push({ checked: checked, text: o.text() });
                            });
                            value = options;
                            break;

                        case "inplacerte":
                            value = self.data.html();
                            break;

                        default:
                            value = self.data.val();
                    }

                    return value;
                },

                set: function (value) {
                    switch (this.type()) {
                        case "BooleanField":
                            self.data.val(value);
                            self.data.attr("checked", value);
                            break;

                        case "upLevelDiv":
                            self.data.append(value);
                            var check = this.row().find("img[Title='Check Names']:first");
                            check.click();
                            break;

                        case "SelectResult":
                            var options = this.row().find("select[title='" + self.name + " possible values'] option:contains('" + value + "')");

                            options.each(function (i, o) {
                                self.data.append(o);
                            });
                            break;

                        case "MultiCheckBox":
                            self.data.each(function (i, o) {

                                if (o.title == value) {
                                    var checkbox = $("input", o);
                                    var checked = typeof $("input", o).attr("checked") !== "undefined";
                                    checkbox.attr("checked", !checked);
                                }
                            });
                            break;

                        case "inplacerte": //multi line tb
                            self.data.html("");
                            self.data.html(value);
                            break;

                        default:
                            self.data.val(value);
                    }
                },

                remove: function (value) {
                    switch (this.type()) {
                        case "SelectResult":
                            var appendix = "";
                            if (typeof value !== "undefined") {
                                appendix = ":contains('" + value + "')";
                            }
                            var options = this.row().find("select[title='" + self.name + " selected values'] option" + appendix);
                            var possible = this.row().find("select[title='" + self.name + " possible values']");

                            options.each(function (i, o) {
                                o = $(o);
                                possible.append(o.clone());
                                o.remove();
                            });
                            break;

                        case "upLevelDiv":
                            var newItems = new Array();

                            $(self.data.text().toLowerCase().split(";")).each(function (i, o) {
                                if (typeof value !== "undefined" && o.trim() !== value.toLowerCase()) {
                                    newItems.push(o.trim());
                                }
                            });


                            self.data.text(newItems.join(";"));

                            var check = this.row().find("img[Title='Check Names']:first");
                            check.click();

                            break;
                        default:
                            self.data.val(value);
                    }
                }
            }
        };

        return {
            fields: function (name) {

                var type = undefined;
                //text values
                var f = $("input[Title='" + name + "']");

                //dropdowns
                if (f.length == 0) {
                    f = $("Select[Title='" + name + "']");
                }

                //multi dropdown
                if (f.length == 0) {
                    f = $("Select[Title='" + name + " selected values'");
                }

                //people picker
                if (f.length == 0) {
                    var row = $("nobr").filter(function () {
                        return $(this).contents().eq(0).text() === name;
                    }).closest("tr");

                    f = row.find("div[name='upLevelDiv']");
                }

                //multi checkboxes
                if (f.length == 0) {
                    var row = $("nobr:contains('" + name + "')").closest("tr");
                    f = row.find("span .ms-RadioText");
                    type = "MultiCheckBox"
                }

                //multiline textbox
                if (f.length == 0) {
                    row = $("nobr").filter(function () {
                        return $(this).contents().eq(0).text() === name;
                    }).closest("tr");

                    f = row.find("div[role=textbox]");
                }

                //if length is still 0, we can use the field is not on the mask
                if (f.length == 0)
                    throw "Field '" + name + "' not found or supported.";

                return new field(f, name, type);
            },

            buttons: function (name) {
                return new field($("input[value='" + name + "']"));
            }
        };
    }();

    sponge.locale = function () {

        var self = this;
        self.dict = null;

        var printf = function (str, args) {
            if (!args) return str;

            var result = '';
            var search = /%(\d+)\$s/g;

            var matches = search.exec(str);

            while (matches) {
                var index = parseInt(matches[1], 10) - 1;
                str = str.replace('%' + matches[1] + '\$s', (args[index]));
                matches = search.exec(str);
            }

            var parts = str.split('%s');

            if (parts.length > 1) {
                for (var i = 0; i < args.length; i++) {
                    if (parts[i].length > 0 && parts[i].lastIndexOf('%') == (parts[i].length - 1)) {
                        parts[i] += 's' + parts.splice(i + 1, 1)[0];
                    }

                    result += parts[i] + args[i];
                }
            }

            return result + parts[parts.length - 1];
        };

        var executeCallback = function (callback) {
            if (callback != null && typeof callback !== "undefined")
                callback();
        };

        return {
            init: function (path, lang, callback) {
                self.langId = lang;
                self.path = path;
                var url = path + "/" + self.langId + ".js";
                $.getJSON(url, function (data) {
                    self.dict = data;
                    executeCallback(callback);
                });
            },

            dict: function () {
                return self.dict;
            },

            get: function (key, params) {
                var result = key;
                if (self.dict && self.dict[key]) {
                    result = self.dict[key];
                }

                return printf(result, params);
            }
        };
    }();
}