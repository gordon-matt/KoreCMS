﻿'use strict'

var ViewModel = function () {
    var self = this;

    self.id = ko.observable(emptyGuid);
    self.pageId = ko.observable(emptyGuid);
    self.pageTypeId = ko.observable(emptyGuid);
    self.archivedDate = ko.observable(null);
    self.name = ko.observable('');
    self.slug = ko.observable('');
    self.fields = ko.observable('');
    self.isEnabled = ko.observable(false);
    self.dateCreatedUtc = ko.observable();
    self.dateModifiedUtc = ko.observable();
    self.cultureCode = ko.observable('');
    self.refId = ko.observable(null);

    self.view = function (id) {
        $.ajax({
            url: "/odata/kore/cms/HistoricPages(guid'" + id + "')",
            type: "GET",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.pageId(json.PageId);
            self.pageTypeId(json.PageTypeId);
            self.archivedDate(json.ArchivedDate);
            self.name(json.Name);
            self.slug(json.Slug);
            self.fields(json.Fields);
            self.isEnabled(json.IsEnabled);
            self.dateCreatedUtc(json.DateCreatedUtc);
            self.dateModifiedUtc(json.DateModifiedUtc);
            self.cultureCode(json.CultureCode);
            self.refId(json.RefId);

            switchSection($("#details-section"));
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.restore = function () {
        $.ajax({
            url: "/odata/kore/cms/HistoricPages(guid'" + self.id() + "')/RestoreVersion",
            type: "POST"
        })
        .done(function (json) {
            $('#Grid').data('kendoGrid').dataSource.read();
            $('#Grid').data('kendoGrid').refresh();
            switchSection($("#grid-section"));
            $.notify(translations.PageHistoryRestoreSuccess, "success");
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.PageHistoryRestoreError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.cancel = function () {
        switchSection($("#grid-section"));
    };;
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
                    url: "/odata/kore/cms/HistoricPages?$filter=PageId eq guid'" + pageId + "'",
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
                        ArchivedDate: { type: "date" },
                        IsEnabled: { type: "boolean" }
                    }
                }
            },
            pageSize: 10,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            sort: { field: "ArchivedDate", dir: "desc" }
        },
        filterable: false,
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
            template: '<a href="/#=Slug#" target="_blank">#=Name#</a>',
        }, {
            field: "ArchivedDate",
            title: "Archived Date",
            format: "{0:yyyy-MM-dd HH:mm:ss}",
            width: 150
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group"><a onclick="viewModel.view(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.View + '</a></div>',
            attributes: { "class": "text-center" },
            width: 70
        }]
    });
});