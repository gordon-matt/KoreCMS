﻿define(function (require) {
    'use strict'

    var $ = require('jquery');
    var ko = require('knockout');

    require('jqueryval');
    require('kendo');
    require('notify');
    require('tinymce');
    require('tinymce-jquery');
    require('tinymce-knockout');

    require('kore-common');
    require('kore-section-switching');
    require('kore-jqueryval');
    require('kore-tinymce');

    var ViewModel = function () {
        var self = this;

        self.gridPageSize = 10;
        self.translations = false;

        self.id = ko.observable(emptyGuid);
        self.name = ko.observable(null);
        self.ownerId = ko.observable(null);
        self.subject = ko.observable(null);
        self.body = ko.observable(null);
        self.enabled = ko.observable(false);

        self.validator = false;
        self.tinyMCEConfig = koreDefaultTinyMCEConfig;

        self.attached = function () {
            currentSection = $("#grid-section");

            // Load translations first, else will have errors
            $.ajax({
                url: "/admin/messaging/templates/get-translations",
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

            self.validator = $("#form-section-form").validate({
                rules: {
                    Name: { required: true, maxlength: 255 },
                    Subject: { required: true, maxlength: 255 },
                    Body: { required: true }
                }
            });

            $("#Grid").kendoGrid({
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: "/odata/kore/cms/MessageTemplateApi",
                            dataType: "json"
                        },
                        parameterMap: function (options, operation) {
                            var paramMap = kendo.data.transports.odata.parameterMap(options);
                            if (paramMap.$inlinecount) {
                                if (paramMap.$inlinecount == "allpages") {
                                    paramMap.$count = true;
                                }
                                delete paramMap.$inlinecount;
                            }
                            if (paramMap.$filter) {
                                paramMap.$filter = paramMap.$filter.replace(/substringof\((.+),(.*?)\)/, "contains($2,$1)");
                            }
                            return paramMap;
                        }
                    },
                    schema: {
                        data: function (data) {
                            return data.value;
                        },
                        total: function (data) {
                            return data["@odata.count"];
                        },
                        model: {
                            id: "Id",
                            fields: {
                                Name: { type: "string" },
                                Subject: { type: "string" },
                                Enabled: { type: "boolean" }
                            }
                        }
                    },
                    pageSize: self.gridPageSize,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    sort: { field: "Name", dir: "asc" }
                },
                dataBound: function (e) {
                    var body = this.element.find("tbody")[0];
                    if (body) {
                        ko.cleanNode(body);
                        ko.applyBindings(ko.dataFor(body), body);
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
                    field: "Name",
                    title: self.translations.Columns.Name,
                    filterable: true
                }, {
                    field: "Subject",
                    title: self.translations.Columns.Subject,
                    filterable: true
                }, {
                    field: "Enabled",
                    title: self.translations.Columns.Enabled,
                    template: '<i class="fa #=Enabled ? \'fa-check text-success\' : \'fa-times text-danger\'#"></i>',
                    attributes: { "class": "text-center" },
                    filterable: true,
                    width: 70
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group">' +
                        '<button type="button" data-bind="click: edit.bind($data,\'#=Id#\')" class="btn btn-default btn-sm" title="' + self.translations.Edit + '"><i class="fa fa-edit"></i></button>' +
                        '<button type="button" data-bind="click: remove.bind($data,\'#=Id#\')" class="btn btn-danger btn-sm" title="' + self.translations.Delete + '"><i class="fa fa-remove"></i></button>' +
                        '<button type="button" data-bind="click: toggleEnabled.bind($data,\'#=Id#\', #=Enabled#)" class="btn btn-default btn-sm" title="' + self.translations.Toggle + '"><i class="fa #=Enabled ? \'fa-toggle-on text-success\' : \'fa-toggle-off text-danger\'#"></i></button>' +
                        '</div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 150
                }]
            });
        };
        self.create = function () {
            self.id(emptyGuid);
            self.name(null);
            self.ownerId(null);
            self.subject(null);
            self.body('');
            self.enabled(false);

            $("#tokens-list").html("");

            self.validator.resetForm();
            switchSection($("#form-section"));
            $("#form-section-legend").html(self.translations.Create);
        };
        self.edit = function (id) {
            $.ajax({
                url: "/odata/kore/cms/MessageTemplateApi(" + id + ")",
                type: "GET",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.id(json.Id);
                self.name(json.Name);
                self.ownerId(json.OwnerId);
                self.subject(json.Subject);
                self.body(json.Body);
                self.enabled(json.Enabled);

                $("#tokens-list").html("");

                $.ajax({
                    url: "/odata/kore/cms/MessageTemplateApi/Default.GetTokens",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ templateName: json.Name }),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    if (json.value && json.value.length > 0) {
                        var s = '';
                        $.each(json.value, function () {
                            s += '<li>' + this + '</li>';
                        });
                        $("#tokens-list").html(s);
                    }
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    //$.notify(self.translations.GetRecordError, "error");
                    $.notify(self.translations.GetTokensError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });

                self.validator.resetForm();
                switchSection($("#form-section"));
                $("#form-section-legend").html(self.translations.Edit);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.remove = function (id) {
            if (confirm(self.translations.DeleteRecordConfirm)) {
                $.ajax({
                    url: "/odata/kore/cms/MessageTemplateApi(" + id + ")",
                    type: "DELETE",
                    async: false
                })
                .done(function (json) {
                    $('#Grid').data('kendoGrid').dataSource.read();
                    $('#Grid').data('kendoGrid').refresh();

                    $.notify(self.translations.DeleteRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.translations.DeleteRecordError, "error");
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
                OwnerId: self.ownerId(),
                Subject: self.subject(),
                Body: self.body(),
                Enabled: self.enabled()
            };

            if (self.id() == emptyGuid) {
                // INSERT
                $.ajax({
                    url: "/odata/kore/cms/MessageTemplateApi",
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

                    $.notify(self.translations.InsertRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.translations.InsertRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
            else {
                // UPDATE
                $.ajax({
                    url: "/odata/kore/cms/MessageTemplateApi(" + self.id() + ")",
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

                    $.notify(self.translations.UpdateRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.translations.UpdateRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.cancel = function () {
            switchSection($("#grid-section"));
        };
        self.toggleEnabled = function (id, isEnabled) {
            var patch = {
                Enabled: !isEnabled
            };

            $.ajax({
                url: "/odata/kore/cms/MessageTemplateApi(" + id + ")",
                type: "PATCH",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(patch),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#Grid').data('kendoGrid').dataSource.read();
                $('#Grid').data('kendoGrid').refresh();

                $.notify(self.translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
    };

    var viewModel = new ViewModel();
    return viewModel;
});