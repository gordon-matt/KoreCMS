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
    require('bootstrap-fileinput');

    var ViewModel = function () {
        var self = this;

        self.gridPageSize = 10;
        self.translations = false;

        self.id = ko.observable(emptyGuid);
        self.name = ko.observable(null);
        self.cultureCode = ko.observable(null);
        self.isRTL = ko.observable(false);
        self.isEnabled = ko.observable(false);
        self.sortOrder = ko.observable(0);

        self.validator = false;

        self.attached = function () {
            currentSection = $("#grid-section");

            // Load translations first, else will have errors
            $.ajax({
                url: "/admin/localization/languages/get-translations",
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
                    CultureCode: { required: true, maxlength: 10 },
                    SortOrder: { required: true }
                }
            });

            $("#Upload").fileinput({
                uploadUrl: '/admin/localization/languages/import-language-pack',
                uploadAsync: false,
                maxFileCount: 1,
                showPreview: false,
                showRemove: false,
                allowedFileExtensions: ['json']
            });
            $('#Upload').on('filebatchuploadsuccess', function (event, data, previewId, index) {
                var response = data.response;
                $('#Grid').data('kendoGrid').dataSource.read();
                $('#Grid').data('kendoGrid').refresh();
                switchSection($("#grid-section"));
                $.notify(response.Message, "success");
            });

            $("#Grid").kendoGrid({
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: "/odata/kore/cms/LanguageApi",
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
                                Name: { type: "string" },
                                CultureCode: { type: "string" },
                                IsEnabled: { type: "boolean" },
                                SortOrder: { type: "number" }
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
                    title: self.translations.Columns.Name,
                    filterable: true
                }, {
                    field: "CultureCode",
                    title: self.translations.Columns.CultureCode,
                    filterable: true,
                    width: 70
                }, {
                    field: "IsEnabled",
                    title: self.translations.Columns.IsEnabled,
                    template: '<i class="fa #=IsEnabled ? \'fa-check text-success\' : \'fa-times text-danger\'#"></i>',
                    attributes: { "class": "text-center" },
                    filterable: true,
                    width: 70
                }, {
                    field: "SortOrder",
                    title: self.translations.Columns.SortOrder,
                    filterable: true,
                    width: 70
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group"><a onclick="viewModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + self.translations.Edit + '</a>' +
                        '<a onclick="viewModel.remove(\'#=Id#\')" class="btn btn-danger btn-xs">' + self.translations.Delete + '</a>' +
                        '<a href="/admin/localization/localizable-strings/#=Id#" class="btn btn-primary btn-xs">' + self.translations.Localize + '</a>' +
                        '</div>',
                    //TODO: '<a onclick="viewModel.setDefault(\'#=Id#\', #=IsEnabled#)" class="btn btn-default btn-xs">Set Default</a></div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 170
                }]
            });
        };
        self.create = function () {
            self.id(emptyGuid);
            self.name(null);
            self.cultureCode(null);
            self.isRTL(false);
            self.isEnabled(false);
            self.sortOrder(0);

            self.validator.resetForm();
            switchSection($("#form-section"));
            $("#form-section-legend").html(self.translations.Create);
        };
        self.edit = function (id) {
            $.ajax({
                url: "/odata/kore/cms/LanguageApi(guid'" + id + "')",
                type: "GET",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.id(json.Id);
                self.name(json.Name);
                self.cultureCode(json.CultureCode);
                self.isRTL(json.IsRTL);
                self.isEnabled(json.IsEnabled);
                self.sortOrder(json.SortOrder);

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
                    url: "/odata/kore/cms/LanguageApi(guid'" + id + "')",
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

            var cultureCode = self.cultureCode();
            if (cultureCode == '') {
                cultureCode = null;
            }

            var record = {
                Id: self.id(),
                Name: self.name(),
                CultureCode: cultureCode,
                IsRTL: self.isRTL(),
                IsEnabled: self.isEnabled(),
                SortOrder: self.sortOrder()
            };

            if (self.id() == emptyGuid) {
                // INSERT
                $.ajax({
                    url: "/odata/kore/cms/LanguageApi",
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
                    url: "/odata/kore/cms/LanguageApi(guid'" + self.id() + "')",
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
        self.onCultureCodeChanged = function () {
            var cultureName = $('#CultureCode option:selected').text();
            self.name(cultureName);
        };
        self.clear = function () {
            $.ajax({
                url: "/odata/kore/cms/LanguageApi/ResetLocalizableStrings",
                type: "POST"
            })
            .done(function (json) {
                $.notify(self.translations.ResetLocalizableStringsSuccess, "success");
                setTimeout(function () {
                    window.location.reload();
                }, 500);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.translations.ResetLocalizableStringsError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.importLanguagePack = function () {
            switchSection($("#upload-section"));
        };
    };

    viewModel = new ViewModel();
    return viewModel;
});