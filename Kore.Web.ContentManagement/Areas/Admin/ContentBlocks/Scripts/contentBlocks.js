﻿'use strict'

var ZoneModel = function () {
    var self = this;
    self.id = ko.observable(emptyGuid);
    self.name = ko.observable(null);

    self.create = function () {
        self.id(emptyGuid);
        self.name(null);
        self.validator.resetForm();
        switchSection($("#zones-edit-section"));
    };

    self.edit = function (id) {
        $.ajax({
            url: "/odata/kore/cms/ZoneApi(guid'" + id + "')",
            type: "GET",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.name(json.Name);
            self.validator.resetForm();
            switchSection($("#zones-edit-section"));
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.delete = function (id) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: "/odata/kore/cms/ZoneApi(guid'" + id + "')",
                type: "DELETE",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#ZoneGrid').data('kendoGrid').dataSource.read();
                $('#ZoneGrid').data('kendoGrid').refresh();

                $('#ZoneId option[value="' + id + '"]').remove();
                $('#Create_ZoneId option[value="' + id + '"]').remove();

                $.notify(translations.DeleteRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.DeleteRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.save = function () {
        if (!$("#zone-edit-section-form").valid()) {
            return false;
        }

        var record = {
            Id: self.id(),
            Name: self.name(),
        };

        if (self.id() == emptyGuid) {
            // INSERT
            $.ajax({
                url: "/odata/kore/cms/ZoneApi",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#ZoneGrid').data('kendoGrid').dataSource.read();
                $('#ZoneGrid').data('kendoGrid').refresh();

                switchSection($("#zones-grid-section"));

                // Update zone drop downs
                $('#ZoneId').append($('<option>', {
                    value: json.Id,
                    text: record.Name
                }));
                $('#Create_ZoneId').append($('<option>', {
                    value: json.Id,
                    text: record.Name
                }));

                $.notify(translations.InsertRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.InsertRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
        else {
            // UPDATE
            $.ajax({
                url: "/odata/kore/cms/ZoneApi(guid'" + self.id() + "')",
                type: "PUT",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#ZoneGrid').data('kendoGrid').dataSource.read();
                $('#ZoneGrid').data('kendoGrid').refresh();

                switchSection($("#zones-grid-section"));

                // Update zone drop downs
                $('#ZoneId option[value="' + record.Id + '"]').text(record.Name);
                $('#Create_ZoneId option[value="' + record.Id + '"]').text(record.Name);

                $.notify(translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.showContentBlocks = function () {
        switchSection($("#grid-section"));
    };

    self.cancel = function () {
        switchSection($("#zones-grid-section"));
    };

    self.validator = $("#zone-edit-section-form").validate({
        rules: {
            Zone_Name: { required: true, maxlength: 255 }
        }
    });
};

var ViewModel = function () {
    var self = this;

    self.id = ko.observable(emptyGuid);
    self.title = ko.observable(null);
    self.order = ko.observable(0);
    self.isEnabled = ko.observable(false);
    self.blockName = ko.observable(null);
    self.blockType = ko.observable(null);
    self.zoneId = ko.observable(emptyGuid);
    self.displayCondition = ko.observable(null);
    self.blockValues = ko.observable(null);
    self.pageId = ko.observable(null);
    self.cultureCode = ko.observable(null);
    self.refId = ko.observable(null);

    self.contentBlockModelStub = null;

    self.zoneModel = new ZoneModel();

    self.create = function () {
        self.id(emptyGuid);
        self.title(null);
        self.order(0);
        self.isEnabled(false);
        self.blockName(null);
        self.blockType(null);
        self.zoneId(emptyGuid);
        self.displayCondition(null);
        self.blockValues(null);
        self.pageId(pageId);
        self.cultureCode(null);
        self.refId(null);

        // Clean up from previously injected html/scripts
        if (self.contentBlockModelStub != null && typeof self.contentBlockModelStub.cleanUp === 'function') {
            self.contentBlockModelStub.cleanUp();
        }
        self.contentBlockModelStub = null;

        // Remove Old Scripts
        var oldScripts = $('script[data-block-script="true"]');

        if (oldScripts.length > 0) {
            $.each(oldScripts, function () {
                $(this).remove();
            });
        }

        var elementToBind = $("#block-details")[0];
        ko.cleanNode(elementToBind);
        $("#block-details").html("");

        self.createFormValidator.resetForm();
        switchSection($("#create-section"));
    };

    self.edit = function (id) {
        $.ajax({
            url: "/odata/kore/cms/ContentBlockApi(guid'" + id + "')",
            type: "GET",
            //contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.title(json.Title);
            self.order(json.Order);
            self.isEnabled(json.IsEnabled);
            self.blockName(json.BlockName);
            self.blockType(json.BlockType);
            self.zoneId(json.ZoneId);
            self.displayCondition(json.DisplayCondition);
            self.blockValues(json.BlockValues);
            self.pageId(json.PageId);
            self.cultureCode(json.CultreCode);
            self.refId(json.RefId);

            $.ajax({
                url: "/admin/blocks/content-blocks/get-editor-ui/" + self.id(),
                type: "GET",
                dataType: "json",
                async: false
            })
            .done(function (json) {

                // Clean up from previously injected html/scripts
                if (self.contentBlockModelStub != null && typeof self.contentBlockModelStub.cleanUp === 'function') {
                    self.contentBlockModelStub.cleanUp();
                }
                self.contentBlockModelStub = null;

                // Remove Old Scripts
                var oldScripts = $('script[data-block-script="true"]');

                if (oldScripts.length > 0) {
                    $.each(oldScripts, function () {
                        $(this).remove();
                    });
                }

                var elementToBind = $("#block-details")[0];
                ko.cleanNode(elementToBind);
                $("#block-details").html("");

                var result = $(json.Content);

                // Add new HTML
                var content = $(result.filter('#block-content')[0]);
                var details = $('<div>').append(content.clone()).html();
                $("#block-details").html(details);

                // Add new Scripts
                var scripts = result.filter('script');

                $.each(scripts, function () {
                    var script = $(this);
                    script.attr("data-block-script", "true");//for some reason, .data("block-script", "true") doesn't work here
                    script.appendTo('body');
                });

                // Update Bindings
                // Ensure the function exists before calling it...
                if (typeof contentBlockModel != null) {
                    self.contentBlockModelStub = contentBlockModel;
                    if (typeof self.contentBlockModelStub.updateModel === 'function') {
                        self.contentBlockModelStub.updateModel();
                    }
                    ko.applyBindings(viewModel, elementToBind);
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });

            self.editFormValidator.resetForm();
            switchSection($("#edit-section"));
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.delete = function (id) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: "/odata/kore/cms/ContentBlockApi(guid'" + id + "')",
                type: "DELETE",
                async: false
            })
            .done(function (json) {
                $('#Grid').data('kendoGrid').dataSource.read();
                $('#Grid').data('kendoGrid').refresh();

                $.notify(translations.DeleteRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.DeleteRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.save = function () {
        var isNew = (self.id() == emptyGuid);

        if (isNew) {
            if (!$("#create-section-form").valid()) {
                return false;
            }
        }
        else {
            if (!$("#edit-section-form").valid()) {
                return false;
            }
        }

        var cultureCode = self.cultureCode();
        if (cultureCode == '') {
            cultureCode = null;
        }

        // ensure the function exists before calling it...
        if (self.contentBlockModelStub != null && typeof self.contentBlockModelStub.onBeforeSave === 'function') {
            self.contentBlockModelStub.onBeforeSave();
        }

        var record = {
            Id: self.id(),
            Title: self.title(),
            Order: self.order(),
            IsEnabled: self.isEnabled(),
            BlockName: self.blockName(),
            BlockType: self.blockType(),
            ZoneId: self.zoneId(),
            DisplayCondition: self.displayCondition(),
            BlockValues: self.blockValues(),
            PageId: self.pageId(),
            CultureCode: cultureCode,
            RefId: self.refId()
        };

        if (isNew) {
            $.ajax({
                url: "/odata/kore/cms/ContentBlockApi",
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
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.InsertRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
        else {
            $.ajax({
                url: "/odata/kore/cms/ContentBlockApi(guid'" + self.id() + "')",
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
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.cancel = function () {
        // Clean up from previously injected html/scripts
        if (self.contentBlockModelStub != null && typeof self.contentBlockModelStub.cleanUp === 'function') {
            self.contentBlockModelStub.cleanUp();
        }
        self.contentBlockModelStub = null;

        // Remove Old Scripts
        var oldScripts = $('script[data-block-script="true"]');

        if (oldScripts.length > 0) {
            $.each(oldScripts, function () {
                $(this).remove();
            });
        }

        var elementToBind = $("#block-details")[0];
        ko.cleanNode(elementToBind);
        $("#block-details").html("");

        switchSection($("#grid-section"));
    };

    self.toggleEnabled = function (id, isEnabled) {
        var patch = {
            IsEnabled: !isEnabled
        };

        $.ajax({
            url: "/odata/kore/cms/ContentBlockApi(guid'" + id + "')",
            type: "PATCH",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(patch),
            dataType: "json",
            async: false
        })
        .done(function (json) {
            $('#Grid').data('kendoGrid').dataSource.read();
            $('#Grid').data('kendoGrid').refresh();

            $.notify(translations.UpdateRecordSuccess, "success");
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.UpdateRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.showZones = function () {
        switchSection($("#zones-grid-section"));
    };

    self.createFormValidator = $("#create-section-form").validate({
        rules: {
            Create_Title: { required: true, maxlength: 255 }
        }
    });

    self.editFormValidator = $("#edit-section-form").validate({
        rules: {
            Title: { required: true, maxlength: 255 },
            BlockName: { required: true, maxlength: 255 },
            BlockType: { maxlength: 1024 }
        }
    });
};

var viewModel;
$(document).ready(function () {
    viewModel = new ViewModel();
    ko.applyBindings(viewModel);

    var ds = {
        type: "odata",
        transport: {
            read: {
                url: "/odata/kore/cms/ContentBlockApi",
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
                fields: {
                    Title: { type: "string" },
                    BlockName: { type: "string" },
                    Order: { type: "number" },
                    IsEnabled: { type: "boolean" }
                }
            }
        },
        pageSize: gridPageSize,
        serverPaging: true,
        serverFiltering: true,
        serverSorting: true,
        sort: { field: "Title", dir: "asc" }
    };

    // Override grid data source if necessary (to filter by Page ID)
    if (pageId && pageId != '') {
        ds.transport.read.url = "/odata/kore/cms/ContentBlockApi/GetByPageId";
        ds.transport.read.type = "POST";
        ds.transport.read.contentType = "application/json";
        ds.transport.parameterMap = function (options, operation) {
            if (operation === "read") {
                return kendo.stringify({
                    pageId: pageId
                });
            }
        };
    }

    $("#Grid").kendoGrid({
        data: null,
        dataSource: ds,
        filterable: true,
        sortable: {
            allowUnsort: false
        },
        pageable: {
            refresh: true
        },
        scrollable: false,
        columns: [{
            field: "Title",
            title: translations.Columns.Title,
            filterable: true
        }, {
            field: "BlockName",
            title: translations.Columns.BlockType,
            filterable: true
        }, {
            field: "Order",
            title: translations.Columns.Order,
            filterable: false
        }, {
            field: "IsEnabled",
            title: translations.Columns.IsEnabled,
            template: '<i class="fa #=IsEnabled ? \'fa-check text-success\' : \'fa-times text-danger\'#"></i>',
            attributes: { "class": "text-center" },
            filterable: true,
            width: 70
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group"><a onclick="viewModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.Edit + '</a>' +
                '<a onclick="viewModel.delete(\'#=Id#\')" class="btn btn-danger btn-xs">' + translations.Delete + '</a>' +
                '<a onclick="viewModel.toggleEnabled(\'#=Id#\', #=IsEnabled#)" class="btn btn-default btn-xs">' + translations.Toggle + '</a></div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 250
        }]
    });

    $("#ZoneGrid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: "/odata/kore/cms/ZoneApi",
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
                    fields: {
                        Name: { type: "string" }
                    }
                }
            },
            pageSize: gridPageSize,
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
            title: translations.Columns.Name,
            filterable: true
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group"><a onclick="viewModel.zoneModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.Edit + '</a>' +
                '<a onclick="viewModel.zoneModel.delete(\'#=Id#\')" class="btn btn-danger btn-xs">' + translations.Delete + '</a></div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 120
        }]
    });
});