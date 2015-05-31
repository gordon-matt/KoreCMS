'use strict'

function imagePickerCallback(url) {
    viewModel.teaserImageUrl(url);
};

var ViewModel = function () {
    var self = this;

    self.id = ko.observable(emptyGuid);
    self.headline = ko.observable(null);
    self.slug = ko.observable(null);
    self.teaserImageUrl = ko.observable(null);
    self.shortDescription = ko.observable(null);
    self.fullDescription = ko.observable(null);
    self.useExternalLink = ko.observable(false);
    self.externalLink = ko.observable(null);
    self.metaKeywords = ko.observable(null);
    self.metaDescription = ko.observable(null);

    self.tinyMCE_fullDescription = koreDefaultTinyMCEConfig;

    self.create = function () {
        self.id(emptyGuid);
        self.headline(null);
        self.slug(null);
        self.teaserImageUrl(null);
        self.shortDescription(null);
        self.fullDescription('');
        self.useExternalLink(false);
        self.externalLink(null);
        self.metaKeywords(null);
        self.metaDescription(null);

        self.validator.resetForm();
        switchSection($("#form-section"));
        $("#form-section-legend").html(translations.Create);
    };

    self.edit = function (id) {
        $.ajax({
            url: "/odata/kore/cms/BlogApi(guid'" + id + "')",
            type: "GET",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.headline(json.Headline);
            self.slug(json.Slug);
            self.teaserImageUrl(json.TeaserImageUrl);
            self.shortDescription(json.ShortDescription);
            self.fullDescription(json.FullDescription);
            self.useExternalLink(json.UseExternalLink);
            self.externalLink(json.ExternalLink);
            self.metaKeywords(json.MetaKeywords);
            self.metaDescription(json.MetaDescription);

            self.validator.resetForm();
            switchSection($("#form-section"));
            $("#form-section-legend").html(translations.Edit);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.delete = function (id) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: "/odata/kore/cms/BlogApi(guid'" + id + "')",
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

        if (!$("#form-section-form").valid()) {
            return false;
        }

        var record = {
            Id: self.id(),
            Headline: self.headline(),
            Slug: self.slug(),
            TeaserImageUrl: self.teaserImageUrl(),
            ShortDescription: self.shortDescription(),
            FullDescription: self.fullDescription(),
            UseExternalLink: self.useExternalLink(),
            ExternalLink: self.externalLink(),
            MetaKeywords: self.metaKeywords(),
            MetaDescription: self.metaDescription()
        };

        if (isNew) {
            $.ajax({
                url: "/odata/kore/cms/BlogApi",
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
                url: "/odata/kore/cms/BlogApi(guid'" + self.id() + "')",
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
        switchSection($("#grid-section"));
    };

    self.validator = $("#form-section-form").validate({
        rules: {
            Headline: { required: true, maxlength: 255 },
            Slug: { required: true, maxlength: 255 },
            TeaserImageUrl: { maxlength: 255 },
            ExternalLink: { maxlength: 255 },
            MetaKeywords: { maxlength: 255 },
            MetaDescription: { maxlength: 255 }
        }
    });
};

var viewModel;
$(document).ready(function () {
    viewModel = new ViewModel();
    ko.applyBindings(viewModel);

    switchSection($("#grid-section"));

    $("#Grid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: "/odata/kore/cms/BlogApi",
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
                        Headline: { type: "string" },
                        DateCreatedUtc: { type: "date" }
                    }
                }
            },
            pageSize: gridPageSize,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            sort: { field: "DateCreatedUtc", dir: "desc" }
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
            field: "Headline",
            title: translations.Columns.Headline,
            filterable: true
        }, {
            field: "DateCreatedUtc",
            title: translations.Columns.DateCreatedUtc,
            filterable: true,
            format: '{0:yyyy-MM-dd}'
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group"><a onclick="viewModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.Edit + '</a>' +
                '<a onclick="viewModel.delete(\'#=Id#\')" class="btn btn-danger btn-xs">' + translations.Delete + '</a>' +
                '</div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 150
        }]
    });
});