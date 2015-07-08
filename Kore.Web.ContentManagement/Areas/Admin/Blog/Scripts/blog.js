'use strict'

var postApiUrl = "/odata/kore/cms/BlogPostApi";
var tagApiUrl = "/odata/kore/cms/BlogTagApi";
var categoryApiUrl = "/odata/kore/cms/BlogCategoryApi";

var TagModel = function () {
    var self = this;

    self.id = ko.observable(0);
    self.name = ko.observable(null);
    self.urlSlug = ko.observable(null);

    self.create = function () {
        self.id(0);
        self.name(null);
        self.urlSlug(null);

        self.validator.resetForm();
        switchSection($("#tag-form-section"));
        $("#tag-form-section-legend").html(translations.Create);
    };

    self.edit = function (id) {
        $.ajax({
            url: tagApiUrl + "(" + id + ")",
            type: "GET",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.name(json.Name);
            self.urlSlug(json.UrlSlug);

            self.validator.resetForm();
            switchSection($("#tag-form-section"));
            $("#tag-form-section-legend").html(translations.Edit);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.delete = function (id) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: tagApiUrl + "(" + id + ")",
                type: "DELETE",
                async: false
            })
            .done(function (json) {
                $('#TagGrid').data('kendoGrid').dataSource.read();
                $('#TagGrid').data('kendoGrid').refresh();

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

        if (!$("#tag-form-section-form").valid()) {
            return false;
        }

        var record = {
            Id: self.id(),
            Name: self.name(),
            UrlSlug: self.urlSlug(),
        };

        if (isNew) {
            $.ajax({
                url: tagApiUrl,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#TagGrid').data('kendoGrid').dataSource.read();
                $('#TagGrid').data('kendoGrid').refresh();

                switchSection($("#tag-grid-section"));

                $.notify(translations.InsertRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.InsertRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
        else {
            $.ajax({
                url: tagApiUrl + "(" + self.id() + ")",
                type: "PUT",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#TagGrid').data('kendoGrid').dataSource.read();
                $('#TagGrid').data('kendoGrid').refresh();

                switchSection($("#tag-grid-section"));

                $.notify(translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.cancel = function () {
        switchSection($("#tag-grid-section"));
    };

    self.validator = $("#tag-form-section-form").validate({
        rules: {
            Tag_Name: { required: true, maxlength: 255 },
            Tag_UrlSlug: { required: true, maxlength: 255 }
        }
    });
};

var CategoryModel = function () {
    var self = this;

    self.id = ko.observable(0);
    self.name = ko.observable(null);
    self.urlSlug = ko.observable(null);

    self.create = function () {
        self.id(0);
        self.name(null);
        self.urlSlug(null);

        self.validator.resetForm();
        switchSection($("#category-form-section"));
        $("#category-form-section-legend").html(translations.Create);
    };

    self.edit = function (id) {
        $.ajax({
            url: categoryApiUrl + "(" + id + ")",
            type: "GET",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.name(json.Name);
            self.urlSlug(json.UrlSlug);

            self.validator.resetForm();
            switchSection($("#category-form-section"));
            $("#category-form-section-legend").html(translations.Edit);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.delete = function (id) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: categoryApiUrl + "(" + id + ")",
                type: "DELETE",
                async: false
            })
            .done(function (json) {
                $('#CategoryGrid').data('kendoGrid').dataSource.read();
                $('#CategoryGrid').data('kendoGrid').refresh();

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

        if (!$("#category-form-section-form").valid()) {
            return false;
        }

        var record = {
            Id: self.id(),
            Name: self.name(),
            UrlSlug: self.urlSlug(),
        };

        if (isNew) {
            $.ajax({
                url: categoryApiUrl,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#CategoryGrid').data('kendoGrid').dataSource.read();
                $('#CategoryGrid').data('kendoGrid').refresh();

                switchSection($("#category-grid-section"));

                $.notify(translations.InsertRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.InsertRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
        else {
            $.ajax({
                url: categoryApiUrl + "(" + self.id() + ")",
                type: "PUT",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#CategoryGrid').data('kendoGrid').dataSource.read();
                $('#CategoryGrid').data('kendoGrid').refresh();

                switchSection($("#category-grid-section"));

                $.notify(translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.cancel = function () {
        switchSection($("#category-grid-section"));
    };

    self.validator = $("#category-form-section-form").validate({
        rules: {
            Category_Name: { required: true, maxlength: 255 },
            Category_UrlSlug: { required: true, maxlength: 255 }
        }
    });
};

var PostModel = function () {
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
        switchSection($("#post-form-section"));
        $("#post-form-section-legend").html(translations.Create);
    };

    self.edit = function (id) {
        $.ajax({
            url: postApiUrl + "(guid'" + id + "')",
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
            switchSection($("#post-form-section"));
            $("#post-form-section-legend").html(translations.Edit);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.delete = function (id) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: postApiUrl + "(guid'" + id + "')",
                type: "DELETE",
                async: false
            })
            .done(function (json) {
                $('#PostGrid').data('kendoGrid').dataSource.read();
                $('#PostGrid').data('kendoGrid').refresh();

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

        if (!$("#post-form-section-form").valid()) {
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
                url: postApiUrl,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#PostGrid').data('kendoGrid').dataSource.read();
                $('#PostGrid').data('kendoGrid').refresh();

                switchSection($("#post-grid-section"));

                $.notify(translations.InsertRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.InsertRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
        else {
            $.ajax({
                url: postApiUrl + "(guid'" + self.id() + "')",
                type: "PUT",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#PostGrid').data('kendoGrid').dataSource.read();
                $('#PostGrid').data('kendoGrid').refresh();

                switchSection($("#post-grid-section"));

                $.notify(translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.cancel = function () {
        switchSection($("#post-grid-section"));
    };

    self.validator = $("#post-form-section-form").validate({
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

var ViewModel = function () {
    var self = this;

    self.categoryModel = new CategoryModel();
    self.postModel = new PostModel();
    self.tagModel = new TagModel();

    self.tinyMCE_fullDescription = koreDefaultTinyMCEConfig;

    self.showCategories = function () {
        switchSection($("#category-grid-section"));
    };

    self.showPosts = function () {
        switchSection($("#post-grid-section"));
    };

    self.showTags = function () {
        switchSection($("#tag-grid-section"));
    };
};

var viewModel;
$(document).ready(function () {
    viewModel = new ViewModel();
    ko.applyBindings(viewModel);

    switchSection($("#grid-section"));

    $("#PostGrid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: postApiUrl,
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
            title: translations.Columns.Post.Headline,
            filterable: true
        }, {
            field: "DateCreatedUtc",
            title: translations.Columns.Post.DateCreatedUtc,
            filterable: true,
            format: '{0:G}'
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group"><a onclick="viewModel.postModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.Edit + '</a>' +
                '<a onclick="viewModel.postModel.delete(\'#=Id#\')" class="btn btn-danger btn-xs">' + translations.Delete + '</a>' +
                '</div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 150
        }]
    });

    $("#CategoryGrid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: categoryApiUrl,
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
            title: translations.Columns.Category.Name,
            filterable: true
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group"><a onclick="viewModel.categoryModel.edit(#=Id#)" class="btn btn-default btn-xs">' + translations.Edit + '</a>' +
                '<a onclick="viewModel.categoryModel.delete(#=Id#)" class="btn btn-danger btn-xs">' + translations.Delete + '</a>' +
                '</div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 150
        }]
    });

    $("#TagGrid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: tagApiUrl,
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
            title: translations.Columns.Tag.Name,
            filterable: true
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group"><a onclick="viewModel.tagModel.edit(#=Id#)" class="btn btn-default btn-xs">' + translations.Edit + '</a>' +
                '<a onclick="viewModel.tagModel.delete(#=Id#)" class="btn btn-danger btn-xs">' + translations.Delete + '</a>' +
                '</div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 150
        }]
    });
});