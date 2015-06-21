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
    self.isDownloadable = ko.observable(null);
    self.popoverHtml = ko.observable(null);

    self.create = function () {
        self.id(0);
        self.title(null);
        self.thumbnailUrl(null);
        self.videoUrl(null);
        self.mobileVideoUrl(null);
        self.posterUrl(null);
        self.mobilePosterUrl(null);
        self.isDownloadable(null);
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

    self.validator = $("#videos-form-section-form").validate({
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

    self.availableVideos = ko.observableArray([]);
    self.availableVideosTotal = ko.observable(0);
    self.availableVideosPageIndex = ko.observable(1);
    self.availableVideosPageSize = 25;

    self.availableVideosPageCount = function () {
        return Math.ceil(self.availableVideosTotal() / self.availableVideosPageSize);
    };

    self.create = function () {
        self.id(0);
        self.name(null);

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
                var selectedVideos = $("#sortable2 li").map(function () {
                    return $(this).data("video-id");
                }).get();

                if (selectedVideos.length > 0) {
                    $.ajax({
                        url: playlistApiUrl + "/UpdatePlaylistVideos",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({
                            playlistId: self.id(),
                            videoIds: selectedVideos.join("|")
                        }),
                        async: false
                    })
                    .done(function (json) {
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        $.notify(translations.InsertRecordError, "error");
                        console.log(textStatus + ': ' + errorThrown);
                    });
                }

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

breeze.config.initializeAdapterInstances({ dataService: "OData" });
var manager = new breeze.EntityManager('/odata/fwd/royal-video-player');

var pagerInitialized = false;

var viewModel;
$(document).ready(function () {
    viewModel = new ViewModel();
    getAvailableVideos();
    ko.applyBindings(viewModel);

    switchSection($("#playlists-grid-section"));

    $(function () {
        $("#sortable1, #sortable2").sortable({
            connectWith: "#sortable2",
            // Idea: maybe no need this - just grab all when ready to save...
            //stop: function (event, ui) {
            //    //TODO: check if its from sortable1 or sortable2. If sortable1, then add to array...
            //    //  if sortable2, then rearrange items in array
            //    var videoId = $(ui.item).data("video-id");
            //    viewModel.playlist.selectedVideos.push(videoId);
            //}
        }).disableSelection();
    });

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
                '<a onclick="viewModel.playlist.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.Edit + '</a>' +
                '<a onclick="viewModel.playlist.delete(\'#=Id#\')" class="btn btn-danger btn-xs">' + translations.Delete + '</a>' +
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
            filterable: true
        }, {
            field: "Title",
            title: translations.Columns.Video.Title,
            filterable: true
        }, {
            field: "Id",
            title: " ",
            template:
                '<div class="btn-group">' +
                '<a onclick="viewModel.video.edit(\'#=Id#\')" class="btn btn-default btn-xs">' + translations.Edit + '</a>' +
                '<a onclick="viewModel.video.delete(\'#=Id#\')" class="btn btn-danger btn-xs">' + translations.Delete + '</a>' +
                '</div>',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 180
        }]
    });


});

function getAvailableVideos() {
    var query = new breeze.EntityQuery()
        .from("VideoApi")
        .orderBy("Title asc")
        .skip((viewModel.playlist.availableVideosPageIndex() - 1) * viewModel.playlist.availableVideosPageSize)
        .take(viewModel.playlist.availableVideosPageSize)
        .inlineCount();

    manager.executeQuery(query).then(function (data) {
        viewModel.playlist.availableVideos([]);
        viewModel.playlist.availableVideosTotal(data.inlineCount);

        $(data.httpResponse.data.results).each(function () {
            var current = this;
            var entry = new VideoModel();
            entry.id(current.Id);
            entry.title(current.Title);
            entry.thumbnailUrl(current.ThumbnailUrl);
            entry.videoUrl(current.VideoUrl);
            entry.mobileVideoUrl(current.MobileVideoUrl);
            entry.posterUrl(current.PosterUrl);
            entry.mobilePosterUrl(current.MobilePosterUrl);
            entry.isDownloadable(current.IsDownloadable);
            entry.popoverHtml(current.PopoverHtml);

            viewModel.playlist.availableVideos.push(entry);
        });

        if (!pagerInitialized) {
            $('#pager').bootpag({
                total: viewModel.playlist.availableVideosPageCount(),
                page: viewModel.playlist.availableVideosPageIndex(),
                maxVisible: 5,
                leaps: true,
                firstLastUse: true,
            }).on("page", function (event, num) {
                viewModel.playlist.availableVideosPageIndex(num);
                getAvailableVideos();
            });
            pagerInitialized = true;
        }
    }).fail(function (e) {
        alert(e);
    });
};