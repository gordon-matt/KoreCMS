var viewModel;

define(function (require) {
    'use strict'

    var $ = require('jquery');
    var jqueryval = require('jqueryval');
    var ko = require('knockout');
    var koMap = require('knockout-mapping');
    var kendo = require('kendo');
    //var ko_kendo = require('kendo-knockout');
    var notify = require('notify');

    var tinymce = require('tinymce');
    var tinymce_jquery = require('tinymce-jquery');
    var tinymce_knockout = require('tinymce-knockout');

    var kCommon = require('kore-common');
    var kSections = require('kore-section-switching');
    var kJVal = require('kore-jqueryval');
    var kChosenKo = require('kore-chosen-knockout');
    var kTiny = require('kore-tinymce');

    ko.mapping = koMap;

    var BlockModel = function () {
        var self = this;

        self.id = ko.observable(emptyGuid);
        self.entityType = ko.observable(entityType);
        self.entityId = ko.observable(entityId);
        self.blockName = ko.observable(null);
        self.blockType = ko.observable(null);
        self.title = ko.observable(null);
        self.zoneId = ko.observable(emptyGuid);
        self.order = ko.observable(0);
        self.isEnabled = ko.observable(false);
        self.blockValues = ko.observable(null);
        self.customTemplatePath = ko.observable(null);

        self.createFormValidator = false;
        self.editFormValidator = false;

        self.contentBlockModelStub = null;

        self.init = function () {
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

            $("#Grid").kendoGrid({
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: "/odata/kore/cms/EntityTypeContentBlockApi?$filter=EntityType eq '" + entityType + "' and EntityId eq '" + entityId + "'",
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
                    pageSize: viewModel.gridPageSize,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    sort: { field: "Title", dir: "asc" }
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
                    field: "Title",
                    title: viewModel.translations.Columns.Title,
                    filterable: true
                }, {
                    field: "BlockName",
                    title: viewModel.translations.Columns.BlockType,
                    filterable: true
                }, {
                    field: "Order",
                    title: viewModel.translations.Columns.Order,
                    filterable: false
                }, {
                    field: "IsEnabled",
                    title: viewModel.translations.Columns.IsEnabled,
                    template: '<i class="fa #=IsEnabled ? \'fa-check text-success\' : \'fa-times text-danger\'#"></i>',
                    attributes: { "class": "text-center" },
                    filterable: true,
                    width: 70
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group"><a onclick="viewModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + viewModel.translations.Edit + '</a>' +
                        '<a onclick="viewModel.remove(\'#=Id#\')" class="btn btn-danger btn-xs">' + viewModel.translations.Delete + '</a>' +
                        '<a onclick="viewModel.toggleEnabled(\'#=Id#\', #=IsEnabled#)" class="btn btn-default btn-xs">' + viewModel.translations.Toggle + '</a></div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 250
                }]
            });
        };
        self.create = function () {
            self.id(emptyGuid);
            self.entityType(entityType);
            self.entityId(entityId);
            self.blockName(null);
            self.blockType(null);
            self.title(null);
            self.zoneId(emptyGuid);
            self.order(0);
            self.isEnabled(false);
            self.blockValues(null);
            self.customTemplatePath(null);

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
                url: "/odata/kore/cms/EntityTypeContentBlockApi(guid'" + id + "')",
                type: "GET",
                //contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.id(json.Id);
                self.entityType(json.EntityType);
                self.entityId(json.EntityId);
                self.blockName(json.BlockName);
                self.blockType(json.BlockType);
                self.title(json.Title);
                self.zoneId(json.ZoneId);
                self.order(json.Order);
                self.isEnabled(json.IsEnabled);
                self.blockValues(json.BlockValues);
                self.customTemplatePath(json.CustomTemplatePath);

                $.ajax({
                    url: "/admin/blocks/entity-type-content-blocks/get-editor-ui/" + self.id(),
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
                    $.notify(viewModel.translations.GetRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });

                self.editFormValidator.resetForm();
                switchSection($("#edit-section"));
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(viewModel.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.remove = function (id) {
            if (confirm(viewModel.translations.DeleteRecordConfirm)) {
                $.ajax({
                    url: "/odata/kore/cms/EntityTypeContentBlockApi(guid'" + id + "')",
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

            // ensure the function exists before calling it...
            if (self.contentBlockModelStub != null && typeof self.contentBlockModelStub.onBeforeSave === 'function') {
                self.contentBlockModelStub.onBeforeSave();
            }

            var record = {
                Id: self.id(),
                EntityType: self.entityType(),
                EntityId: self.entityId(),
                BlockName: self.blockName(),
                BlockType: self.blockType(),
                Title: self.title(),
                ZoneId: self.zoneId(),
                Order: self.order(),
                IsEnabled: self.isEnabled(),
                BlockValues: self.blockValues(),
                CustomTemplatePath: self.customTemplatePath()
            };

            if (isNew) {
                $.ajax({
                    url: "/odata/kore/cms/EntityTypeContentBlockApi",
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
                $.ajax({
                    url: "/odata/kore/cms/EntityTypeContentBlockApi(guid'" + self.id() + "')",
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
                url: "/odata/kore/cms/EntityTypeContentBlockApi(guid'" + id + "')",
                type: "PATCH",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(patch),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#Grid').data('kendoGrid').dataSource.read();
                $('#Grid').data('kendoGrid').refresh();

                $.notify(viewModel.translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(viewModel.translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
    };

    var ZoneModel = function () {
        var self = this;
        self.id = ko.observable(emptyGuid);
        self.name = ko.observable(null);

        self.validator = false;

        self.init = function () {
            self.validator = $("#zone-edit-section-form").validate({
                rules: {
                    Zone_Name: { required: true, maxlength: 255 }
                }
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
                    pageSize: viewModel.gridPageSize,
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
                    title: viewModel.translations.Columns.Name,
                    filterable: true
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group"><a onclick="viewModel.zoneModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + viewModel.translations.Edit + '</a>' +
                        '<a onclick="viewModel.zoneModel.remove(\'#=Id#\')" class="btn btn-danger btn-xs">' + viewModel.translations.Delete + '</a></div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 120
                }]
            });
        };
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
                $.notify(viewModel.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.remove = function (id) {
            if (confirm(viewModel.translations.DeleteRecordConfirm)) {
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

                    $.notify(viewModel.translations.DeleteRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.DeleteRecordError, "error");
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

                    $.notify(viewModel.translations.UpdateRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.UpdateRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.cancel = function () {
            switchSection($("#zones-grid-section"));
        };
    };

    var ViewModel = function () {
        var self = this;

        self.gridPageSize = 10;
        self.entityType = null;
        self.entityId = null;
        self.translations = false;

        self.blockModel = false;
        self.zoneModel = false;

        self.activate = function (entityType, entityId) {
            self.entityType = entityType;
            self.entityId = entityId;

            self.blockModel = new BlockModel();
            self.zoneModel = new ZoneModel();
        };
        self.attached = function () {
            currentSection = $("#grid-section");

            // Load translations first, else will have errors
            $.ajax({
                url: "/admin/blocks/entity-type-content-blocks/get-translations",
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

            self.blockModel.init();
            self.zoneModel.init();
        };
        self.showBlocks = function () {
            switchSection($("#grid-section"));
        };
        self.showZones = function () {
            switchSection($("#zones-grid-section"));
        };
    };

    viewModel = new ViewModel();
    return viewModel;
});