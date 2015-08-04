define(['plugins/router'], function (router) {
    var ViewModel = function () {
        var self = this;

        self.rebuild = function () {
            $.ajax({
                url: "/admin/indexing/rebuild",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false
            })
            .done(function (json) {
                window.location.reload(); //TODO: Find better way, so only current view is refreshed.
                //router.navigate('#indexing'); //doesn't work
                //router.activateItem().activate(); //doesn't work
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify("Error when trying to rebuild the index.", "error");
                console.log(textStatus + ': ' + errorThrown);
            });
        };
    };

    var viewModel = new ViewModel();
    return viewModel;
});