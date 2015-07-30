var viewModel;

define(function (require) {
    'use strict'

    var $ = require('jquery');
    var jqueryval = require('jqueryval');
    var ko = require('knockout');
    var kendo = require('kendo');
    //var ko_kendo = require('kendo-knockout');
    var notify = require('notify');
    var datajs = require('datajs');
    var odata = require('OData');
    var Q = require('Q');
    var breeze = require('breeze');
    var chosen = require('chosen');
    var tinymce = require('tinymce');
    var tinymce_jquery = require('tinymce-jquery');
    var tinymce_knockout = require('tinymce-knockout');

    var kCommon = require('kore-common');
    var kSections = require('kore-section-switching');
    var kJVal = require('kore-jqueryval');
    var kChosenKo = require('kore-chosen-knockout');
    var kTiny = require('kore-tinymce');

    var postApiUrl = "/odata/kore/cms/BlogPostApi";
    var categoryApiUrl = "/odata/kore/cms/BlogCategoryApi";
    var tagApiUrl = "/odata/kore/cms/BlogTagApi";

    var PostModel = function () {
        var self = this;

        self.id = ko.observable(emptyGuid);
        self.categoryId = ko.observable(0);
        self.headline = ko.observable(null);
        self.slug = ko.observable(null);
        self.teaserImageUrl = ko.observable(null);
        self.shortDescription = ko.observable(null);
        self.fullDescription = ko.observable(null);
        self.useExternalLink = ko.observable(false);
        self.externalLink = ko.observable(null);
        self.metaKeywords = ko.observable(null);
        self.metaDescription = ko.observable(null);

        self.availableTags = ko.observableArray([]);
        self.chosenTags = ko.observableArray([]);

        self.validator = false;

        self.init = function () {
            self.validator = $("#post-form-section-form").validate({
                rules: {
                    CategoryId: { required: true },
                    Headline: { required: true, maxlength: 255 },
                    Slug: { required: true, maxlength: 255 },
                    TeaserImageUrl: { maxlength: 255 },
                    ExternalLink: { maxlength: 255 },
                    MetaKeywords: { maxlength: 255 },
                    MetaDescription: { maxlength: 255 }
                }
            });

            var manager = new breeze.EntityManager('/odata/kore/cms');

            var query = new breeze.EntityQuery()
                .from("BlogTagApi")
                .orderBy("Name asc");

            manager.executeQuery(query).then(function (data) {
                self.availableTags([]);
                self.chosenTags([]);

                $(data.httpResponse.data.results).each(function () {
                    var current = this;
                    self.availableTags.push({ Id: current.Id, Name: current.Name });
                });
            }).fail(function (e) {
                alert(e);
            });

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
                    pageSize: viewModel.gridPageSize,
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
                    title: viewModel.translations.Columns.Post.Headline,
                    filterable: true
                }, {
                    field: "DateCreatedUtc",
                    title: viewModel.translations.Columns.Post.DateCreatedUtc,
                    filterable: true,
                    format: '{0:G}',
                    width: 200
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group"><a onclick="viewModel.postModel.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + viewModel.translations.Edit + '</a>' +
                        '<a onclick="viewModel.postModel.delete(\'#=Id#\')" class="btn btn-danger btn-xs">' + viewModel.translations.Delete + '</a>' +
                        '</div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 150
                }]
            });
        };
        self.create = function () {
            self.id(emptyGuid);
            self.categoryId(0);
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
            $("#post-form-section-legend").html(viewModel.translations.Create);
        };
        self.edit = function (id) {
            $.ajax({
                url: postApiUrl + "(guid'" + id + "')?$expand=Tags",
                type: "GET",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.id(json.Id);
                self.categoryId(json.CategoryId);
                self.headline(json.Headline);
                self.slug(json.Slug);
                self.teaserImageUrl(json.TeaserImageUrl);
                self.shortDescription(json.ShortDescription);
                self.fullDescription(json.FullDescription);
                self.useExternalLink(json.UseExternalLink);
                self.externalLink(json.ExternalLink);
                self.metaKeywords(json.MetaKeywords);
                self.metaDescription(json.MetaDescription);

                self.chosenTags([]);
                $(json.Tags).each(function (index, item) {
                    self.chosenTags.push(item.TagId);
                });

                self.validator.resetForm();
                switchSection($("#post-form-section"));
                $("#post-form-section-legend").html(viewModel.translations.Edit);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(viewModel.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.remove = function (id) {
            if (confirm(viewModel.translations.DeleteRecordConfirm)) {
                $.ajax({
                    url: postApiUrl + "(guid'" + id + "')",
                    type: "DELETE",
                    async: false
                })
                .done(function (json) {
                    $('#PostGrid').data('kendoGrid').dataSource.read();
                    $('#PostGrid').data('kendoGrid').refresh();

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

            if (!$("#post-form-section-form").valid()) {
                return false;
            }

            var tags = self.chosenTags().map(function (item) {
                return {
                    TagId: item
                }
            });

            var record = {
                Id: self.id(),
                CategoryId: self.categoryId(),
                Headline: self.headline(),
                Slug: self.slug(),
                TeaserImageUrl: self.teaserImageUrl(),
                ShortDescription: self.shortDescription(),
                FullDescription: self.fullDescription(),
                UseExternalLink: self.useExternalLink(),
                ExternalLink: self.externalLink(),
                MetaKeywords: self.metaKeywords(),
                MetaDescription: self.metaDescription(),
                Tags: tags
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

                    $.notify(viewModel.translations.InsertRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.InsertRecordError, "error");
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

                    $.notify(viewModel.translations.UpdateRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.UpdateRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.cancel = function () {
            switchSection($("#post-grid-section"));
        };
    };

    var CategoryModel = function () {
        var self = this;

        self.id = ko.observable(0);
        self.name = ko.observable(null);
        self.urlSlug = ko.observable(null);

        self.validator = false;

        self.init = function () {
            self.validator = $("#category-form-section-form").validate({
                rules: {
                    Category_Name: { required: true, maxlength: 255 },
                    Category_UrlSlug: { required: true, maxlength: 255 }
                }
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
                    title: viewModel.translations.Columns.Category.Name,
                    filterable: true
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group"><a onclick="viewModel.categoryModel.edit(#=Id#)" class="btn btn-default btn-xs">' + viewModel.translations.Edit + '</a>' +
                        '<a onclick="viewModel.categoryModel.delete(#=Id#)" class="btn btn-danger btn-xs">' + viewModel.translations.Delete + '</a>' +
                        '</div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 150
                }]
            });
        };
        self.create = function () {
            self.id(0);
            self.name(null);
            self.urlSlug(null);

            self.validator.resetForm();
            switchSection($("#category-form-section"));
            $("#category-form-section-legend").html(viewModel.translations.Create);
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
                $("#category-form-section-legend").html(viewModel.translations.Edit);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(viewModel.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.delete = function (id) {
            if (confirm(viewModel.translations.DeleteRecordConfirm)) {
                $.ajax({
                    url: categoryApiUrl + "(" + id + ")",
                    type: "DELETE",
                    async: false
                })
                .done(function (json) {
                    $('#CategoryGrid').data('kendoGrid').dataSource.read();
                    $('#CategoryGrid').data('kendoGrid').refresh();

                    $.notify(viewModel.translations.DeleteRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.DeleteRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.save = function () {
            var isNew = (self.id() == 0);

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

                    $.notify(viewModel.translations.InsertRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.InsertRecordError, "error");
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

                    $.notify(viewModel.translations.UpdateRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.UpdateRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.cancel = function () {
            switchSection($("#category-grid-section"));
        };
    };

    var TagModel = function () {
        var self = this;

        self.id = ko.observable(0);
        self.name = ko.observable(null);
        self.urlSlug = ko.observable(null);

        self.validator = false;

        self.init = function () {
            self.validator = $("#tag-form-section-form").validate({
                rules: {
                    Tag_Name: { required: true, maxlength: 255 },
                    Tag_UrlSlug: { required: true, maxlength: 255 }
                }
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
                    title: viewModel.translations.Columns.Tag.Name,
                    filterable: true
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group"><a onclick="viewModel.tagModel.edit(#=Id#)" class="btn btn-default btn-xs">' + viewModel.translations.Edit + '</a>' +
                        '<a onclick="viewModel.tagModel.delete(#=Id#)" class="btn btn-danger btn-xs">' + viewModel.translations.Delete + '</a>' +
                        '</div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 150
                }]
            });
        };
        self.create = function () {
            self.id(0);
            self.name(null);
            self.urlSlug(null);

            self.validator.resetForm();
            switchSection($("#tag-form-section"));
            $("#tag-form-section-legend").html(viewModel.translations.Create);
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
                $("#tag-form-section-legend").html(viewModel.translations.Edit);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(viewModel.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.delete = function (id) {
            if (confirm(viewModel.translations.DeleteRecordConfirm)) {
                $.ajax({
                    url: tagApiUrl + "(" + id + ")",
                    type: "DELETE",
                    async: false
                })
                .done(function (json) {
                    $('#TagGrid').data('kendoGrid').dataSource.read();
                    $('#TagGrid').data('kendoGrid').refresh();

                    $.notify(viewModel.translations.DeleteRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.DeleteRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.save = function () {
            var isNew = (self.id() == 0);

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

                    $.notify(viewModel.translations.InsertRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.InsertRecordError, "error");
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

                    $.notify(viewModel.translations.UpdateRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(viewModel.translations.UpdateRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.cancel = function () {
            switchSection($("#tag-grid-section"));
        };
    };

    var ViewModel = function () {
        var self = this;

        self.gridPageSize = 10;
        self.translations = false;

        self.postModel = false;
        self.categoryModel = false;
        self.tagModel = false;
        self.tinyMCE_fullDescription = koreDefaultTinyMCEConfig;

        self.modalDismissed = false;

        self.activate = function () {
            self.postModel = new PostModel();
            self.categoryModel = new CategoryModel();
            self.tagModel = new TagModel();
        };
        self.attached = function () {
            currentSection = $("#post-grid-section");

            // Load translations first, else will have errors
            $.ajax({
                url: "/admin/blog/get-translations",
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

            breeze.config.initializeAdapterInstances({ dataService: "OData" });
            self.postModel.init();
            self.categoryModel.init();
            self.tagModel.init();

            $('#myModal').on('hidden.bs.modal', function () {
                if (!modalDismissed) {
                    var url = $('#TeaserImageUrl').val();
                    url = "/Media/Uploads/" + url;
                    viewModel.postModel.teaserImageUrl(url);
                }
                modalDismissed = false;
            });
        };

        self.dismissModal = function() {
            self.modalDismissed = true;
            $('#myModal').modal('hide');
        };

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

    viewModel = new ViewModel();
    return viewModel;
});