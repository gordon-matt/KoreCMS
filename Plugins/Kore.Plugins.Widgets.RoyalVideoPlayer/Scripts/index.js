'use strict'

var playlistApiUrl = "/odata/fwd/royal-video-player/PlaylistApi";
var videoApiUrl = "/odata/fwd/royal-video-player/VideoApi";

var VideoModel = function () {
    var self = this;

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
        $("#videos-form-section-legend").html(translations.Create);
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
            $("#videos-form-section-legend").html(translations.Edit);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.delete = function (id) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: videoApiUrl + "(" + id + ")",
                type: "DELETE",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                $('#VideoGrid').data('kendoGrid').dataSource.read();
                $('#VideoGrid').data('kendoGrid').refresh();
                $.notify(translations.DeleteRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.DeleteRecordError, "error");
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

                $.notify(translations.InsertRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.InsertRecordError, "error");
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

                $.notify(translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.UpdateRecordError, "error");
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
            url: playlistApiUrl + "/GetPlaylistsForVideo",
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
            $.notify(translations.GetRecordError, "error");
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
            url: videoApiUrl + "/AssignVideoToPlaylists",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            async: false
        })
        .done(function (json) {
            switchSection($("#videos-grid-section"));
            $.notify(translations.SavePlaylistsSuccess, "success");
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.SavePlaylistsError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

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
};

var PlaylistModel = function () {
    var self = this;

    self.id = ko.observable(0);
    self.name = ko.observable(null);

    self.isEditMode = ko.observable(false);

    self.create = function () {
        self.id(0);
        self.name(null);

        self.isEditMode(false);
        self.validator.resetForm();
        switchSection($("#playlists-form-section"));
        $("#playlists-form-section-legend").html(translations.Create);
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
                        playlistId: viewModel.playlist.id()
                    });
                }
            };
            grid.dataSource.read();
            grid.refresh();

            self.validator.resetForm();
            switchSection($("#playlists-form-section"));
            $("#playlists-form-section-legend").html(translations.Edit);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify(translations.GetRecordError, "error");
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.delete = function (id) {
        if (confirm(translations.DeleteRecordConfirm)) {
            $.ajax({
                url: playlistApiUrl + "(" + id + ")",
                type: "DELETE",
                async: false
            })
            .done(function (json) {
                $('#PlaylistGrid').data('kendoGrid').dataSource.read();
                $('#PlaylistGrid').data('kendoGrid').refresh();

                $.notify(translations.DeleteRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.DeleteRecordError, "error");
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

                $.notify(translations.UpdateRecordSuccess, "success");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify(translations.UpdateRecordError, "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.cancel = function () {
        switchSection($("#playlists-grid-section"));
    };

    self.validator = $("#playlists-form-section-form").validate({
        rules: {
            Name: { required: true, maxlength: 255 }
        }
    });
};

var ViewModel = function () {
    var self = this;
    self.playlist = new PlaylistModel();
    self.video = new VideoModel();

    self.showPlaylists = function () {
        switchSection($("#playlists-grid-section"));
    };

    self.showVideos = function () {
        switchSection($("#videos-grid-section"));
    };
};

var viewModel;
$(document).ready(function () {
    viewModel = new ViewModel();
    ko.applyBindings(viewModel);

    switchSection($("#playlists-grid-section"));

    $("#PlaylistGrid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: playlistApiUrl,
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
            title: translations.Columns.Playlist.Name,
            filterable: true
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group">' +
                '<a onclick="viewModel.playlist.edit(#=Id#)" class="btn btn-default btn-xs">' + translations.Edit + '</a>' +
                '<a onclick="viewModel.playlist.delete(#=Id#)" class="btn btn-danger btn-xs">' + translations.Delete + '</a>' +
                '</div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 180
        }]
    });

    $("#VideoGrid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: videoApiUrl,
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
            pageSize: gridPageSize,
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
            field: "ThumbnailUrl",
            title: translations.Columns.Video.ThumbnailUrl,
            template: '<img src="#=ThumbnailUrl#" alt="#=Title#" class="thumbnail" style="max-width:200px;" />',
            filterable: false,
            width: 200
        }, {
            field: "Title",
            title: translations.Columns.Video.Title,
            filterable: true
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group">' +
                '<a onclick="viewModel.video.edit(#=Id#)" class="btn btn-default btn-xs">' + translations.Edit + '</a>' +
                '<a onclick="viewModel.video.delete(#=Id#)" class="btn btn-danger btn-xs">' + translations.Delete + '</a>' +
                '<a onclick="viewModel.video.editPlaylists(#=Id#)" class="btn btn-default btn-xs">' + translations.Playlists + '</a>' +
                '</div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 210
        }]
    });

    $("#PlaylistVideoGrid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: videoApiUrl + "/GetVideosByPlaylistId",
                    dataType: "json",
                    contentType: "application/json",
                    type: "POST"
                },
                parameterMap: function (options, operation) {
                    if (operation === "read") {
                        return kendo.stringify({
                            playlistId: viewModel.playlist.id()
                        });
                    }
                }
            },
            schema: {
                data: function (data) {
                    return data.value;
                },
                total: function (data) {
                    return data.value.length; // Special case (refer to note in VideoApiController)
                    //return data["odata.count"];
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
            pageSize: gridPageSize,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false,
            //serverPaging: true,  // Special case (refer to note in VideoApiController)
            //serverFiltering: true,
            //serverSorting: true,
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
            field: "ThumbnailUrl",
            title: translations.Columns.Video.ThumbnailUrl,
            template: '<img src="#=ThumbnailUrl#" alt="#=Title#" class="thumbnail" style="max-width:200px;" />',
            filterable: false,
            width: 200
        }, {
            field: "Title",
            title: translations.Columns.Video.Title,
            filterable: true
        }]
    });
});