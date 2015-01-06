'use strict'

var MenuItemVM = function () {
    var self = this;

    self.id = ko.observable(emptyGuid);
    self.menuId = ko.observable(emptyGuid);
    self.text = ko.observable('');
    self.description = ko.observable('');
    self.url = ko.observable('');
    self.cssClass = ko.observable('');
    self.position = ko.observable(0);
    self.parentId = ko.observable(null);
    self.enabled = ko.observable(false);
    self.isExternalUrl = ko.observable(false);
    self.refId = ko.observable(null);

    self.create = function (menuId, parentId) {
        self.id(emptyGuid);
        self.menuId(menuId);
        self.text('');
        self.description('');
        self.url('');
        self.cssClass('');
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
            url: "/odata/kore/cms/MenuItems(guid'" + id + "')",
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
        .fail(function () {
            $.notify(translations.GetRecordError, "error");
        });
    };

    self.delete = function (id, parentId) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: "/odata/kore/cms/MenuItems(guid'" + id + "')",
                type: "DELETE",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                if (parentId) {
                    $('#items-grid-' + parentId).data('kendoGrid').dataSource.read();
                    $('#items-grid-' + parentId).data('kendoGrid').refresh();
                }
                else {
                    $('#ItemsGrid').data('kendoGrid').dataSource.read();
                    $('#ItemsGrid').data('kendoGrid').refresh();
                }

                $.notify(translations.DeleteRecordSuccess, "success");
            })
            .fail(function () {
                $.notify(translations.DeleteRecordError, "error");
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
                url: "/odata/kore/cms/MenuItems",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                if (parentId) {
                    $('#items-grid-' + parentId).data('kendoGrid').dataSource.read();
                    $('#items-grid-' + parentId).data('kendoGrid').refresh();
                }
                else {
                    $('#ItemsGrid').data('kendoGrid').dataSource.read();
                    $('#ItemsGrid').data('kendoGrid').refresh();
                }
                switchSection($("#items-grid-section"));

                $.notify(translations.InsertRecordSuccess, "success");
            })
            .fail(function () {
                $.notify(translations.InsertRecordError, "error");
            });
        }
        else {
            // UPDATE
            $.ajax({
                url: "/odata/kore/cms/MenuItems(guid'" + self.id() + "')",
                type: "PUT",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                if (parentId) {
                    $('#items-grid-' + parentId).data('kendoGrid').dataSource.read();
                    $('#items-grid-' + parentId).data('kendoGrid').refresh();
                }
                else {
                    $('#ItemsGrid').data('kendoGrid').dataSource.read();
                    $('#ItemsGrid').data('kendoGrid').refresh();
                }

                switchSection($("#items-grid-section"));

                $.notify(translations.UpdateRecordSuccess, "success");
            })
            .fail(function () {
                $.notify(translations.UpdateRecordError, "error");
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
            url: "/odata/kore/cms/MenuItems(guid'" + id + "')",
            type: "PATCH",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(patch),
            dataType: "json",
            async: false
        })
        .done(function (json) {
            $('#items-grid-' + parentId).data('kendoGrid').dataSource.read();
            $('#items-grid-' + parentId).data('kendoGrid').refresh();

            $.notify(translations.UpdateRecordSuccess, "success");
        })
        .fail(function () {
            $.notify(translations.UpdateRecordError, "error");
        });
    };

    self.validator = $("#item-edit-section-form").validate({
        rules: {
            Item_Text: { required: true, maxlength: 255 },
            Item_Description: { maxlength: 255 },
            Item_Url: { required: true, maxlength: 255 },
            Item_CssClass: { maxlength: 128 }
        }
    });
};

var ViewModel = function () {
    var self = this;

    self.id = ko.observable(emptyGuid);
    self.name = ko.observable('');
    self.urlFilter = ko.observable('');

    self.menuItem = new MenuItemVM();

    self.create = function () {
        self.id(emptyGuid);
        self.name('');
        self.urlFilter('');

        self.validator.resetForm();
        switchSection($("#form-section"));
        $("#form-section-legend").html(translations.Create);
    };

    self.edit = function (id) {
        $.ajax({
            url: "/odata/kore/cms/Menus(guid'" + id + "')",
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
            $("#form-section-legend").html(translations.Edit);
        })
        .fail(function () {
            $.notify(translations.GetRecordError, "error");
        });
    };

    self.delete = function (id) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: "/odata/kore/cms/Menus(guid'" + id + "')",
                type: "DELETE",
                async: false
            })
            .done(function (json) {
                $('#Grid').data('kendoGrid').dataSource.read();
                $('#Grid').data('kendoGrid').refresh();

                $.notify(translations.DeleteRecordSuccess, "success");
            })
            .fail(function () {
                $.notify(translations.DeleteRecordError, "error");
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
                url: "/odata/kore/cms/Menus",
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

                $.notify(translations.InsertRecordSuccess, "success");
            })
            .fail(function () {
                $.notify(translations.InsertRecordError, "error");
            });
        }
        else {
            // UPDATE
            $.ajax({
                url: "/odata/kore/cms/Menus(guid'" + self.id() + "')",
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

                $.notify(translations.UpdateRecordSuccess, "success");
            })
            .fail(function () {
                $.notify(translations.UpdateRecordError, "error");
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

    self.validator = $("#form-section-form").validate({
        rules: {
            Name: { required: true, maxlength: 255 }
        }
    });
};

var viewModel;
$(document).ready(function () {
    viewModel = new ViewModel();
    ko.applyBindings(viewModel);

    $("#Grid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: "/odata/kore/cms/Menus",
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
            pageSize: 10,
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
            title: "Name",
            filterable: true
        }, {
            field: "UrlFilter",
            title: "URL Filter",
            filterable: true
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group"><a onclick="viewModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.Edit + '</a>' +
                '<a onclick="viewModel.delete(\'#=Id#\')" class="btn btn-danger btn-xs">' + translations.Delete + '</a>' +
                '<a onclick="viewModel.items(\'#=Id#\')" class="btn btn-primary btn-xs">Items</a>' +
                '</div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 170
        }],
        //detailTemplate: kendo.template($("#items-template").html()),
        //detailInit: detailInit,
        //dataBound: function () {
        //    this.expandRow(this.tbody.find("tr.k-master-row").first());
        //},
    });


    $("#ItemsGrid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: "/odata/kore/cms/MenuItems",
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
            pageSize: 10,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            sort: { field: "Text", dir: "asc" },
            filter: {
                logic: "and",
                filters: [
                  { field: "MenuId", operator: "eq", value: viewModel.id() },
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
            title: "Text",
            filterable: true
        }, {
            field: "Url",
            title: "URL",
            filterable: true
        }, {
            field: "Position",
            title: "Position",
            filterable: true,
            width: 70
        }, {
            field: "Enabled",
            title: "Enabled",
            template: '<i class="fa #=Enabled ? \'fa-check text-success\' : \'fa-times text-danger\'#"></i>',
            attributes: { "class": "text-center" },
            filterable: true,
            width: 70
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group"><a onclick="viewModel.menuItem.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.Edit + '</a>' +
                '<a onclick="viewModel.menuItem.delete(\'#=Id#\', null)" class="btn btn-danger btn-xs">' + translations.Delete + '</a>' +
                '<a onclick="viewModel.menuItem.create(\'#=MenuId#\', \'#=Id#\')" class="btn btn-primary btn-xs">' + translations.NewItem + '</a>' +
                '<a onclick="viewModel.menuItem.toggleEnabled(\'#=Id#\',\'#=ParentId#\', #=Enabled#)" class="btn btn-default btn-xs">' + translations.Toggle + '</a></div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 220
        }],
        detailTemplate: kendo.template($("#items-template").html()),
        detailInit: detailInit
    });

});

function detailInit(e) {
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
                    url: "/odata/kore/cms/MenuItems",
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
            pageSize: 10,
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
            title: "Text",
            filterable: true
        }, {
            field: "Url",
            title: "URL",
            filterable: true
        }, {
            field: "Position",
            title: "Position",
            filterable: true,
            width: 70
        }, {
            field: "Enabled",
            title: "Enabled",
            template: '<i class="fa #=Enabled ? \'fa-check text-success\' : \'fa-times text-danger\'#"></i>',
            attributes: { "class": "text-center" },
            filterable: true,
            width: 70
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group"><a onclick="viewModel.menuItem.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.Edit + '</a>' +
                '<a onclick="viewModel.menuItem.delete(\'#=Id#\',\'#=ParentId#\')" class="btn btn-danger btn-xs">' + translations.Delete + '</a>' +
                '<a onclick="viewModel.menuItem.create(\'#=MenuId#\', \'#=Id#\')" class="btn btn-primary btn-xs">' + translations.NewItem + '</a>' +
                '<a onclick="viewModel.menuItem.toggleEnabled(\'#=Id#\',\'#=ParentId#\', #=Enabled#)" class="btn btn-default btn-xs">' + translations.Toggle + '</a></div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 220
        }],
        detailTemplate: kendo.template($("#items-template").html()),
        detailInit: detailInit
    });
}