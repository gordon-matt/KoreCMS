var viewModel;

define(function (require) {
    'use strict'

    var $ = require('jquery');
    var jqueryval = require('jqueryval');
    var ko = require('knockout');
    var kendo = require('kendo');
    var notify = require('notify');

    require('kore-common');
    require('kore-section-switching');
    require('kore-jqueryval');

    var MenuItemModel = function () {
        var self = this;

        self.id = ko.observable(emptyGuid);
        self.menuId = ko.observable(emptyGuid);
        self.text = ko.observable(null);
        self.description = ko.observable(null);
        self.url = ko.observable(null);
        self.cssClass = ko.observable(null);
        self.position = ko.observable(0);
        self.parentId = ko.observable(null);
        self.enabled = ko.observable(false);
        self.isExternalUrl = ko.observable(false);
        self.refId = ko.observable(null);

        self.validator = false;

        self.init = function () {
            self.validator = $("#item-edit-section-form").validate({
                rules: {
                    Item_Text: { required: true, maxlength: 255 },
                    Item_Description: { maxlength: 255 },
                    Item_Url: { required: true, maxlength: 255 },
                    Item_CssClass: { maxlength: 128 }
                }
            });

            $("#ItemsGrid").kendoGrid({
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: "/odata/kore/cms/MenuItemApi",
                            dataType: "json"
                        },
                        parameterMap: function (options, operation) {
                            // call the default OData parameterMap
                            var paramMap = kendo.data.transports.odata.parameterMap(options, operation);

                            if (paramMap.$filter) {
                                // encode everything which looks like a GUID
                                // Fix from here: http://www.telerik.com/forums/guids-in-filters
                                var guid = /('[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}')/ig;
                                paramMap.$filter = paramMap.$filter.replace(guid, "guid$1");
                            }
                            return paramMap;
                        }
                    },
                    schema: {
                        data: function (data) {
                            return data.value;
                        },
                        total: function (data) {
                            return data["odata.count"];
                        },
                        model: {
                            id: "Id",
                            fields: {
                                Text: { type: "string" },
                                Url: { type: "string" },
                                Position: { type: "number" },
                                Enabled: { type: "boolean" }
                            }
                        }
                    },
                    pageSize: self.gridPageSize,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    sort: { field: "Text", dir: "asc" },
                    filter: {
                        logic: "and",
                        filters: [
                          { field: "MenuId", operator: "eq", value: viewModel.menuModel.id() },
                          { field: "ParentId", operator: "eq", value: null }
                        ]
                    }
                },
                filterable: true,
                sortable: {
                    allowUnsort: false
                },
                pageable: {
                    refresh: true
                },
                scrollable: false,
                columns: [{
                    field: "Text",
                    title: viewModel.translations.Columns.MenuItem.Text,
                    filterable: true
                }, {
                    field: "Url",
                    title: viewModel.translations.Columns.MenuItem.Url,
                    filterable: true
                }, {
                    field: "Position",
                    title: viewModel.translations.Columns.MenuItem.Position,
                    filterable: true,
                    width: 70
                }, {
                    field: "Enabled",
                    title: viewModel.translations.Columns.MenuItem.Enabled,
                    template: '<i class="fa #=Enabled ? \'fa-check text-success\' : \'fa-times text-danger\'#"></i>',
                    attributes: { "class": "text-center" },
                    filterable: true,
                    width: 70
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group"><a onclick="viewModel.menuItemModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + viewModel.translations.Edit + '</a>' +
                        '<a onclick="viewModel.menuItemModel.remove(\'#=Id#\', null)" class="btn btn-danger btn-xs">' + viewModel.translations.Delete + '</a>' +
                        '<a onclick="viewModel.menuItemModel.create(\'#=MenuId#\', \'#=Id#\')" class="btn btn-primary btn-xs">' + viewModel.translations.NewItem + '</a>' +
                        '<a onclick="viewModel.menuItemModel.toggleEnabled(\'#=Id#\',\'#=ParentId#\', #=Enabled#)" class="btn btn-default btn-xs">' + viewModel.translations.Toggle + '</a></div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 220
                }],
                detailTemplate: kendo.template($("#items-template").html()),
                detailInit: viewModel.menuItemModel.detailInit
            });
        };
        self.create = function (menuId, parentId) {
            self.id(emptyGuid);
            self.menuId(menuId);
            self.text(null);
            self.description(null);
            self.url(null);
            self.cssClass(null);
            self.position(0);
            self.parentId(parentId);
            self.enabled(false);
            self.isExternalUrl(false);
            self.refId(null);

            self.validator.resetForm();
            switchSection($("#items-edit-section"));
        };
        self.edit = function (id) {
            $.ajax({
                url: "/odata/kore/cms/MenuItemApi(guid'" + id + "')",
                type: "GET",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.id(json.Id);
                self.menuId(json.MenuId);
                self.text(json.Text);
                self.description(json.Description);
                self.url(json.Url);
                self.cssClass(json.CssClass);
                self.position(json.Position);
                self.parentId(json.ParentId);
                self.enabled(json.Enabled);
                self.isExternalUrl(json.IsExternalUrl);
                self.refId(json.RefId);

                self.validator.resetForm();
                switchSection($("#items-edit-section"));
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(viewModel.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.remove = function (id, parentId) {
            if (confirm(viewModel.translations.DeleteRecordConfirm)) {
                $.ajax({
                    url: "/odata/kore/cms/MenuItemApi(guid'" + id + "')",
                    type: "DELETE",
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    self.refreshGrid(parentId);
                    $.notify(viewModel.translations.DeleteRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.DeleteRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.save = function () {
            if (!$("#item-edit-section-form").valid()) {
                return false;
            }

            var parentId = self.parentId();

            var record = {
                Id: self.id(),
                MenuId: self.menuId(),
                Text: self.text(),
                Description: self.description(),
                Url: self.url(),
                CssClass: self.cssClass(),
                Position: self.position(),
                ParentId: parentId,
                Enabled: self.enabled(),
                IsExternalUrl: self.isExternalUrl(),
                RefId: self.refId()
            };

            if (self.id() == emptyGuid) {
                // INSERT
                $.ajax({
                    url: "/odata/kore/cms/MenuItemApi",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(record),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    self.refreshGrid(parentId);
                    switchSection($("#items-grid-section"));
                    $.notify(viewModel.translations.InsertRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.InsertRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
            else {
                // UPDATE
                $.ajax({
                    url: "/odata/kore/cms/MenuItemApi(guid'" + self.id() + "')",
                    type: "PUT",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(record),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    self.refreshGrid(parentId);
                    switchSection($("#items-grid-section"));
                    $.notify(viewModel.translations.UpdateRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.UpdateRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.goBack = function () {
            switchSection($("#grid-section"));
        };
        self.cancel = function () {
            switchSection($("#items-grid-section"));
        };
        self.toggleEnabled = function (id, parentId, isEnabled) {
            var patch = {
                Enabled: !isEnabled
            };

            $.ajax({
                url: "/odata/kore/cms/MenuItemApi(guid'" + id + "')",
                type: "PATCH",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(patch),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.refreshGrid(parentId);
                $.notify(viewModel.translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(viewModel.translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };

        self.refreshGrid = function(parentId) {
            if (parentId && (parentId != "null")) {
                try {
                    $('#items-grid-' + parentId).data('kendoGrid').dataSource.read();
                    $('#items-grid-' + parentId).data('kendoGrid').refresh();
                }
                catch (err) {
                    $('#ItemsGrid').data('kendoGrid').dataSource.read();
                    $('#ItemsGrid').data('kendoGrid').refresh();
                }
            }
            else {
                $('#ItemsGrid').data('kendoGrid').dataSource.read();
                $('#ItemsGrid').data('kendoGrid').refresh();
            }
        }
        self.detailInit = function(e) {
            var detailRow = e.detailRow;

            detailRow.find(".tabstrip").kendoTabStrip({
                animation: {
                    open: { effects: "fadeIn" }
                }
            });

            detailRow.find(".items-grid").kendoGrid({
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: "/odata/kore/cms/MenuItemApi",
                            dataType: "json"
                        },
                        parameterMap: function (options, operation) {
                            // call the default OData parameterMap
                            var paramMap = kendo.data.transports.odata.parameterMap(options, operation);

                            if (paramMap.$filter) {
                                // encode everything which looks like a GUID
                                // Fix from here: http://www.telerik.com/forums/guids-in-filters
                                var guid = /('[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}')/ig;
                                paramMap.$filter = paramMap.$filter.replace(guid, "guid$1");
                            }
                            return paramMap;
                        }
                    },
                    schema: {
                        data: function (data) {
                            return data.value;
                        },
                        total: function (data) {
                            return data["odata.count"];
                        },
                        model: {
                            id: "Id",
                            fields: {
                                Text: { type: "string" },
                                Url: { type: "string" },
                                Position: { type: "number" },
                                Enabled: { type: "boolean" }
                            }
                        }
                    },
                    pageSize: self.gridPageSize,
                    //serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    sort: { field: "Text", dir: "asc" },
                    filter: { field: "ParentId", operator: "eq", value: e.data.Id }
                },
                pageable: false,
                //pageable: {
                //    refresh: true
                //},
                scrollable: false,
                columns: [{
                    field: "Text",
                    title: viewModel.translations.Columns.MenuItem.Text,
                    filterable: true
                }, {
                    field: "Url",
                    title: viewModel.translations.Columns.MenuItem.Url,
                    filterable: true
                }, {
                    field: "Position",
                    title: viewModel.translations.Columns.MenuItem.Position,
                    filterable: true,
                    width: 70
                }, {
                    field: "Enabled",
                    title: viewModel.translations.Columns.MenuItem.Enabled,
                    template: '<i class="fa #=Enabled ? \'fa-check text-success\' : \'fa-times text-danger\'#"></i>',
                    attributes: { "class": "text-center" },
                    filterable: true,
                    width: 70
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group"><a onclick="viewModel.menuItemModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + viewModel.translations.Edit + '</a>' +
                        '<a onclick="viewModel.menuItemModel.remove(\'#=Id#\',\'#=ParentId#\')" class="btn btn-danger btn-xs">' + viewModel.translations.Delete + '</a>' +
                        '<a onclick="viewModel.menuItemModel.create(\'#=MenuId#\', \'#=Id#\')" class="btn btn-primary btn-xs">' + viewModel.translations.NewItem + '</a>' +
                        '<a onclick="viewModel.menuItemModel.toggleEnabled(\'#=Id#\',\'#=ParentId#\', #=Enabled#)" class="btn btn-default btn-xs">' + viewModel.translations.Toggle + '</a></div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 220
                }],
                detailTemplate: kendo.template($("#items-template").html()),
                detailInit: viewModel.menuItemModel.detailInit
            });
        }
    };

    var MenuModel = function () {
        var self = this;

        self.id = ko.observable(emptyGuid);
        self.name = ko.observable(null);
        self.urlFilter = ko.observable(null);

        self.validator = false;

        self.init = function () {
            self.validator = $("#form-section-form").validate({
                rules: {
                    Name: { required: true, maxlength: 255 }
                }
            });

            $("#Grid").kendoGrid({
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: "/odata/kore/cms/MenuApi",
                            dataType: "json"
                        }
                    },
                    schema: {
                        data: function (data) {
                            return data.value;
                        },
                        total: function (data) {
                            return data["odata.count"];
                        },
                        model: {
                            id: "Id",
                            fields: {
                                Name: { type: "string" },
                                UrlFilter: { type: "string" }
                            }
                        }
                    },
                    pageSize: self.gridPageSize,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    sort: { field: "Name", dir: "asc" }
                },
                filterable: true,
                sortable: {
                    allowUnsort: false
                },
                pageable: {
                    refresh: true
                },
                scrollable: false,
                columns: [{
                    field: "Name",
                    title: viewModel.translations.Columns.Menu.Name,
                    filterable: true
                }, {
                    field: "UrlFilter",
                    title: viewModel.translations.Columns.Menu.UrlFilter,
                    filterable: true
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group"><a onclick="viewModel.menuModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + viewModel.translations.Edit + '</a>' +
                        '<a onclick="viewModel.menuModel.remove(\'#=Id#\')" class="btn btn-danger btn-xs">' + viewModel.translations.Delete + '</a>' +
                        '<a onclick="viewModel.menuModel.items(\'#=Id#\')" class="btn btn-primary btn-xs">Items</a>' +
                        '</div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 170
                }]
            });
        };
        self.create = function () {
            self.id(emptyGuid);
            self.name(null);
            self.urlFilter(null);

            self.validator.resetForm();
            switchSection($("#form-section"));
            $("#form-section-legend").html(viewModel.translations.Create);
        };
        self.edit = function (id) {
            $.ajax({
                url: "/odata/kore/cms/MenuApi(guid'" + id + "')",
                type: "GET",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.id(json.Id);
                self.name(json.Name);
                self.urlFilter(json.UrlFilter);

                self.validator.resetForm();
                switchSection($("#form-section"));
                $("#form-section-legend").html(viewModel.translations.Edit);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(viewModel.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.remove = function (id) {
            if (confirm(viewModel.translations.DeleteRecordConfirm)) {
                $.ajax({
                    url: "/odata/kore/cms/MenuApi(guid'" + id + "')",
                    type: "DELETE",
                    async: false
                })
                .done(function (json) {
                    $('#Grid').data('kendoGrid').dataSource.read();
                    $('#Grid').data('kendoGrid').refresh();

                    $.notify(viewModel.translations.DeleteRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.DeleteRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.save = function () {

            if (!$("#form-section-form").valid()) {
                return false;
            }

            var record = {
                Id: self.id(),
                Name: self.name(),
                UrlFilter: self.urlFilter()
            };

            if (self.id() == emptyGuid) {
                // INSERT
                $.ajax({
                    url: "/odata/kore/cms/MenuApi",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(record),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    $('#Grid').data('kendoGrid').dataSource.read();
                    $('#Grid').data('kendoGrid').refresh();

                    switchSection($("#grid-section"));

                    $.notify(viewModel.translations.InsertRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.InsertRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
            else {
                // UPDATE
                $.ajax({
                    url: "/odata/kore/cms/MenuApi(guid'" + self.id() + "')",
                    type: "PUT",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(record),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    $('#Grid').data('kendoGrid').dataSource.read();
                    $('#Grid').data('kendoGrid').refresh();

                    switchSection($("#grid-section"));

                    $.notify(viewModel.translations.UpdateRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.UpdateRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.cancel = function () {
            switchSection($("#grid-section"));
        };
        self.items = function (id) {
            self.id(id);// to support "Create" button for when parent ID is null (top level items)
            var itemsGrid = $("#ItemsGrid").data("kendoGrid");
            itemsGrid.dataSource.filter([{
                logic: "and",
                filters: [
                  { field: "MenuId", operator: "eq", value: self.id() },
                  { field: "ParentId", operator: "eq", value: null }
                ]
            }]);
            itemsGrid.dataSource.read();
            itemsGrid.refresh();
            switchSection($("#items-grid-section"));
        };
    };

    var ViewModel = function () {
        var self = this;

        self.gridPageSize = 10;
        self.translations = false;

        self.menuModel = false;
        self.menuItemModel = false;

        self.activate = function () {
            self.menuModel = new MenuModel();
            self.menuItemModel = new MenuItemModel();
        };
        self.attached = function () {
            currentSection = $("#grid-section");

            // Load translations first, else will have errors
            $.ajax({
                url: "/admin/menus/get-translations",
                type: "GET",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.translations = json;
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            });

            self.gridPageSize = $("#GridPageSize").val();

            self.menuModel.init();
            self.menuItemModel.init();
        };
    };

    viewModel = new ViewModel();
    return viewModel;
});