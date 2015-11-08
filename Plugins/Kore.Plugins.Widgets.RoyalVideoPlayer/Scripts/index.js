define(function (require) {
    'use strict'

    var $ = require('jquery');
    var ko = require('knockout');

    require('jqueryval');
    require('kendo');
    require('notify');

    require('kore-section-switching');
    require('kore-jqueryval');

    var playlistApiUrl = "/odata/fwd/royal-video-player/PlaylistApi";
    var videoApiUrl = "/odata/fwd/royal-video-player/VideoApi";

    var VideoModel = function (parent) {
        var self = this;

        self.parent = parent;
        self.id = ko.observable(0);
        self.title = ko.observable(null);
        self.thumbnailUrl = ko.observable(null);
        self.videoUrl = ko.observable(null);
        self.mobileVideoUrl = ko.observable(null);
        self.posterUrl = ko.observable(null);
        self.mobilePosterUrl = ko.observable(null);
        self.isDownloadable = ko.observable(false);
        self.popoverHtml = ko.observable(null);

        self.playlists = ko.observableArray([]);

        self.validator = false;

        self.init = function () {
            self.validator = $("#video-playlists-form-section-form").validate({
                rules: {
                    Title: { required: true, maxlength: 255 },
                    ThumbnailUrl: { required: true, maxlength: 255 },
                    VideoUrl: { required: true, maxlength: 255 },
                    MobileVideoUrl: { maxlength: 255 },
                    PosterUrl: { required: true, maxlength: 255 },
                    MobilePosterUrl: { maxlength: 255 }
                }
            });

            $("#VideoGrid").kendoGrid({
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: videoApiUrl,
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
                            fields: {
                                Title: { type: "string" },
                                ThumbnailUrl: { type: "string" },
                                VideoUrl: { type: "string" },
                                MobileVideoUrl: { type: "string" },
                                PosterUrl: { type: "string" },
                                MobilePosterUrl: { type: "string" },
                                IsDownloadable: { type: "boolean" },
                                PopoverHtml: { type: "string" }
                            }
                        }
                    },
                    pageSize: self.parent.gridPageSize,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    sort: { field: "Title", dir: "asc" }
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
                    field: "ThumbnailUrl",
                    title: self.parent.translations.Columns.Video.ThumbnailUrl,
                    template: '<img src="#=ThumbnailUrl#" alt="#=Title#" class="thumbnail" style="max-width:200px;" />',
                    filterable: false,
                    width: 200
                }, {
                    field: "Title",
                    title: self.parent.translations.Columns.Video.Title,
                    filterable: true
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group">' +
                        '<a data-bind="click: video.edit.bind($data,#=Id#)" class="btn btn-default btn-xs">' + self.parent.translations.Edit + '</a>' +
                        '<a data-bind="click: video.remove.bind($data,#=Id#)" class="btn btn-danger btn-xs">' + self.parent.translations.Delete + '</a>' +
                        '<a data-bind="click: video.editPlaylists.bind($data,#=Id#)" class="btn btn-default btn-xs">' + self.parent.translations.Playlists + '</a>' +
                        '</div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 210
                }]
            });
        };
        self.create = function () {
            self.id(0);
            self.title(null);
            self.thumbnailUrl(null);
            self.videoUrl(null);
            self.mobileVideoUrl(null);
            self.posterUrl(null);
            self.mobilePosterUrl(null);
            self.isDownloadable(false);
            self.popoverHtml(null);

            self.validator.resetForm();
            switchSection($("#videos-form-section"));
            $("#videos-form-section-legend").html(self.parent.translations.Create);
        };
        self.edit = function (id) {
            $.ajax({
                url: videoApiUrl + "(" + id + ")",
                type: "GET",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.id(json.Id);
                self.title(json.Title);
                self.thumbnailUrl(json.ThumbnailUrl);
                self.videoUrl(json.VideoUrl);
                self.mobileVideoUrl(json.MobileVideoUrl);
                self.posterUrl(json.PosterUrl);
                self.mobilePosterUrl(json.MobilePosterUrl);
                self.isDownloadable(json.IsDownloadable);
                self.popoverHtml(json.PopoverHtml);

                self.validator.resetForm();
                switchSection($("#videos-form-section"));
                $("#videos-form-section-legend").html(self.parent.translations.Edit);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.parent.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.remove = function (id) {
            if (confirm(self.parent.translations.DeleteRecordConfirm)) {
                $.ajax({
                    url: videoApiUrl + "(" + id + ")",
                    type: "DELETE",
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    $('#VideoGrid').data('kendoGrid').dataSource.read();
                    $('#VideoGrid').data('kendoGrid').refresh();
                    $.notify(self.parent.translations.DeleteRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.DeleteRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.save = function () {
            var isNew = (self.id() == 0);

            if (!$("#videos-form-section-form").valid()) {
                return false;
            }

            var record = {
                Id: self.id(),
                Title: self.title(),
                ThumbnailUrl: self.thumbnailUrl(),
                VideoUrl: self.videoUrl(),
                MobileVideoUrl: self.mobileVideoUrl(),
                PosterUrl: self.posterUrl(),
                MobilePosterUrl: self.mobilePosterUrl(),
                IsDownloadable: self.isDownloadable(),
                PopoverHtml: self.popoverHtml(),
            };

            if (isNew) {
                $.ajax({
                    url: videoApiUrl,
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(record),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    $('#VideoGrid').data('kendoGrid').dataSource.read();
                    $('#VideoGrid').data('kendoGrid').refresh();

                    switchSection($("#videos-grid-section"));

                    $.notify(self.parent.translations.InsertRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.InsertRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
            else {
                $.ajax({
                    url: videoApiUrl + "(" + self.id() + ")",
                    type: "PUT",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(record),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    $('#VideoGrid').data('kendoGrid').dataSource.read();
                    $('#VideoGrid').data('kendoGrid').refresh();

                    switchSection($("#videos-grid-section"));

                    $.notify(self.parent.translations.UpdateRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.UpdateRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.cancel = function () {
            switchSection($("#videos-grid-section"));
        };
        self.editPlaylists = function (id) {
            self.id(id);
            self.playlists([]);

            $.ajax({
                url: playlistApiUrl + "/Default.GetPlaylistsForVideo",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ videoId: id }),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                if (json.value && json.value.length > 0) {
                    $.each(json.value, function (index, item) {
                        self.playlists.push(item.toString());
                    });
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.parent.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });

            switchSection($("#video-playlists-form-section"));
        };
        self.editPlaylists_cancel = function () {
            switchSection($("#videos-grid-section"));
        };
        self.editPlaylists_save = function () {
            var data = {
                videoId: self.id(),
                playlists: self.playlists()
            };

            $.ajax({
                url: videoApiUrl + "/Default.AssignVideoToPlaylists",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(data),
                async: false
            })
            .done(function (json) {
                switchSection($("#videos-grid-section"));
                $.notify(self.parent.translations.SavePlaylistsSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.parent.translations.SavePlaylistsError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
    };

    var PlaylistModel = function (parent) {
        var self = this;

        self.parent = parent;
        self.id = ko.observable(0);
        self.name = ko.observable(null);

        self.isEditMode = ko.observable(false);

        self.validator = false;

        self.init = function () {
            self.validator = $("#playlists-form-section-form").validate({
                rules: {
                    Name: { required: true, maxlength: 255 }
                }
            });

            $("#PlaylistGrid").kendoGrid({
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: playlistApiUrl,
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
                            fields: {
                                Name: { type: "string" }
                            }
                        }
                    },
                    pageSize: self.parent.gridPageSize,
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
                    title: self.parent.translations.Columns.Playlist.Name,
                    filterable: true
                }, {
                    field: "Id",
                    title: " ",
                    template:
                        '<div class="btn-group">' +
                        '<a data-bind="click: playlist.edit.bind($data,#=Id#)" class="btn btn-default btn-xs">' + self.parent.translations.Edit + '</a>' +
                        '<a data-bind="click: playlist.remove.bind($data,#=Id#)" class="btn btn-danger btn-xs">' + self.parent.translations.Delete + '</a>' +
                        '</div>',
                    attributes: { "class": "text-center" },
                    filterable: false,
                    width: 180
                }]
            });

            $("#PlaylistVideoGrid").kendoGrid({
                data: null,
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: videoApiUrl + "/Default.GetVideosByPlaylistId(playlistId=" + self.parent.playlist.id() + ")",
                            dataType: "json",
                        },
                        parameterMap: function (options, operation) {
                            var paramMap = kendo.data.transports.odata.parameterMap(options, operation);
                            if (operation === "read") {
                                if (paramMap.$inlinecount) {
                                    if (paramMap.$inlinecount == "allpages") {
                                        paramMap.$count = true;
                                    }
                                    delete paramMap.$inlinecount;
                                }
                                if (paramMap.$filter) {
                                    paramMap.$filter = paramMap.$filter.replace(/substringof\((.+),(.*?)\)/, "contains($2,$1)");
                                }
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
                                Title: { type: "string" },
                                ThumbnailUrl: { type: "string" },
                                VideoUrl: { type: "string" },
                                MobileVideoUrl: { type: "string" },
                                PosterUrl: { type: "string" },
                                MobilePosterUrl: { type: "string" },
                                IsDownloadable: { type: "boolean" },
                                PopoverHtml: { type: "string" }
                            }
                        }
                    },
                    batch: false,
                    pageSize: self.parent.gridPageSize,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    sort: { field: "Title", dir: "asc" }
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
                    field: "ThumbnailUrl",
                    title: self.parent.translations.Columns.Video.ThumbnailUrl,
                    template: '<img src="#=ThumbnailUrl#" alt="#=Title#" class="thumbnail" style="max-width:200px;" />',
                    filterable: false,
                    width: 200
                }, {
                    field: "Title",
                    title: self.parent.translations.Columns.Video.Title,
                    filterable: true
                }]
            });
        };
        self.create = function () {
            self.id(0);
            self.name(null);

            self.isEditMode(false);
            self.validator.resetForm();
            switchSection($("#playlists-form-section"));
            $("#playlists-form-section-legend").html(self.parent.translations.Create);
        };
        self.edit = function (id) {
            $.ajax({
                url: playlistApiUrl + "(" + id + ")",
                type: "GET",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.id(json.Id);
                self.name(json.Name);

                self.isEditMode(true);

                var grid = $('#PlaylistVideoGrid').data('kendoGrid');
                grid.dataSource.transport.parameterMap = function (options, operation) {
                    if (operation === "read") {
                        return kendo.stringify({
                            playlistId: self.parent.playlist.id()
                        });
                    }
                };
                grid.dataSource.read();
                grid.refresh();

                self.validator.resetForm();
                switchSection($("#playlists-form-section"));
                $("#playlists-form-section-legend").html(self.parent.translations.Edit);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(self.parent.translations.GetRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
        self.remove = function (id) {
            if (confirm(self.parent.translations.DeleteRecordConfirm)) {
                $.ajax({
                    url: playlistApiUrl + "(" + id + ")",
                    type: "DELETE",
                    async: false
                })
                .done(function (json) {
                    $('#PlaylistGrid').data('kendoGrid').dataSource.read();
                    $('#PlaylistGrid').data('kendoGrid').refresh();

                    $.notify(self.parent.translations.DeleteRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.DeleteRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.save = function () {

            if (!$("#playlists-form-section-form").valid()) {
                return false;
            }

            var record = {
                Id: self.id(),
                Name: self.name()
            };

            if (self.id() == 0) {
                // INSERT
                $.ajax({
                    url: playlistApiUrl,
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(record),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    $('#PlaylistGrid').data('kendoGrid').dataSource.read();
                    $('#PlaylistGrid').data('kendoGrid').refresh();

                    switchSection($("#playlists-grid-section"));
                    $.notify(self.parent.translations.InsertRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.InsertRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
            else {
                // UPDATE
                $.ajax({
                    url: playlistApiUrl + "(" + self.id() + ")",
                    type: "PUT",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(record),
                    dataType: "json",
                    async: false
                })
                .done(function (json) {
                    $('#PlaylistGrid').data('kendoGrid').dataSource.read();
                    $('#PlaylistGrid').data('kendoGrid').refresh();

                    switchSection($("#playlists-grid-section"));

                    $.notify(self.parent.translations.UpdateRecordSuccess, "success");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.notify(self.parent.translations.UpdateRecordError, "error");
                    console.log(textStatus + ': ' + errorThrown);
                });
            }
        };
        self.cancel = function () {
            switchSection($("#playlists-grid-section"));
        };
    };

    var ViewModel = function () {
        var self = this;

        self.gridPageSize = 10;
        self.translations = false;

        self.playlist = false;
        self.video = false;

        self.modalDismissed = false;

        self.activate = function () {
            self.playlist = new PlaylistModel(self);
            self.video = new VideoModel(self);
        };
        self.attached = function () {
            currentSection = $("#playlists-grid-section");

            // Load translations first, else will have errors
            $.ajax({
                url: "/plugins/royalvideoplayer/get-translations",
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

            self.playlist.init();
            self.video.init();

            $('#thumbnailUrlModal').on('hidden.bs.modal', function () {
                if (!self.modalDismissed) {
                    var url = $('#ThumbnailUrl').val();
                    url = "/Media/Uploads/" + url;
                    self.video.thumbnailUrl(url);
                }
                self.modalDismissed = false;
            });
            $('#videoUrlModal').on('hidden.bs.modal', function () {
                if (!self.modalDismissed) {
                    var url = $('#VideoUrl').val();
                    url = "/Media/Uploads/" + url;
                    self.video.videoUrl(url);
                }
                self.modalDismissed = false;
            });
            $('#mobileVideoUrlModal').on('hidden.bs.modal', function () {
                if (!self.modalDismissed) {
                    var url = $('#MobileVideoUrl').val();
                    url = "/Media/Uploads/" + url;
                    self.video.mobileVideoUrl(url);
                }
                self.modalDismissed = false;
            });
            $('#posterUrlModal').on('hidden.bs.modal', function () {
                if (!self.modalDismissed) {
                    var url = $('#PosterUrl').val();
                    url = "/Media/Uploads/" + url;
                    self.video.posterUrl(url);
                }
                self.modalDismissed = false;
            });
            $('#mobilePosterUrlModal').on('hidden.bs.modal', function () {
                if (!self.modalDismissed) {
                    var url = $('#MobilePosterUrl').val();
                    url = "/Media/Uploads/" + url;
                    self.video.mobilePosterUrl(url);
                }
                self.modalDismissed = false;
            });
        };
        self.showPlaylists = function () {
            switchSection($("#playlists-grid-section"));
        };
        self.showVideos = function () {
            switchSection($("#videos-grid-section"));
        };
        self.dismissModal = function (modalId) {
            self.modalDismissed = true;
            $('#' + modalId).modal('hide');
        };
    };

    var viewModel = new ViewModel();
    return viewModel;
});