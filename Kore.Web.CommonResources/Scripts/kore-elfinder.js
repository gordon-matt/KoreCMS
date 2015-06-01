function elFinderReplaceIcons() {
    $(".elfinder-button-icon.elfinder-button-icon-back").removeClass().addClass("kore-icon kore-icon-arrow-left");
    $(".elfinder-button-icon.elfinder-button-icon-forward").removeClass().addClass("kore-icon kore-icon-arrow-right");
    $(".elfinder-button-icon.elfinder-button-icon-reload").removeClass().addClass("kore-icon kore-icon-refresh");
    $(".elfinder-button-icon.elfinder-button-icon-home").removeClass().addClass("kore-icon kore-icon-home");
    $(".elfinder-button-icon.elfinder-button-icon-up").removeClass().addClass("kore-icon kore-icon-level-up");
    $(".elfinder-button-icon.elfinder-button-icon-mkfile").removeClass().addClass("kore-icon kore-icon-file");
    $(".elfinder-button-icon.elfinder-button-icon-upload").removeClass().addClass("kore-icon kore-icon-upload");
    $(".elfinder-button-icon.elfinder-button-icon-open").removeClass().addClass("kore-icon kore-icon-folder-open");
    $(".elfinder-button-icon.elfinder-button-icon-download").removeClass().addClass("kore-icon kore-icon-save");
    $(".elfinder-button-icon.elfinder-button-icon-info").removeClass().addClass("kore-icon kore-icon-info");
    $(".elfinder-button-icon.elfinder-button-icon-quicklook").removeClass().addClass("kore-icon kore-icon-search");
    $(".elfinder-button-icon.elfinder-button-icon-copy").removeClass().addClass("kore-icon kore-icon-copy");
    $(".elfinder-button-icon.elfinder-button-icon-cut").removeClass().addClass("kore-icon kore-icon-cut");
    $(".elfinder-button-icon.elfinder-button-icon-paste").removeClass().addClass("kore-icon kore-icon-paste");
    $(".elfinder-button-icon.elfinder-button-icon-rm").removeClass().addClass("kore-icon kore-icon-trash");
    $(".elfinder-button-icon.elfinder-button-icon-duplicate").removeClass().addClass("kore-icon kore-icon-duplicate");
    $(".elfinder-button-icon.elfinder-button-icon-edit").removeClass().addClass("kore-icon kore-icon-edit");
    $(".elfinder-button-icon.elfinder-button-icon-resize").removeClass().addClass("kore-icon kore-icon-resize");
    $(".elfinder-button-icon.elfinder-button-icon-view").removeClass().addClass("kore-icon kore-icon-th-list");
    $(".elfinder-button-icon.elfinder-button-icon-view.elfinder-button-icon-view-list").removeClass().addClass("kore-icon kore-icon-list");
    $(".elfinder-button-icon.elfinder-button-icon-sort").removeClass().addClass("kore-icon kore-icon-sort-alpha-asc");

    $(".elfinder-button-icon.elfinder-button-icon-mkdir")
        .removeClass()
        .addClass("kore-icon kore-icon-stack-1x kore-icon-folder-open")
        .parent().addClass("kore-icon-stack")
        .append('<span class="kore-icon kore-icon-stack-1x kore-icon-sub kore-icon-add text-primary"></span>');
};

function elFinderReplaceIconsLarge() {
    $(".elfinder-button-icon.elfinder-button-icon-back").removeClass().addClass("kore-icon kore-icon-arrow-left kore-icon-2x");
    $(".elfinder-button-icon.elfinder-button-icon-forward").removeClass().addClass("kore-icon kore-icon-arrow-right kore-icon-2x");
    $(".elfinder-button-icon.elfinder-button-icon-reload").removeClass().addClass("kore-icon kore-icon-refresh kore-icon-2x");
    $(".elfinder-button-icon.elfinder-button-icon-home").removeClass().addClass("kore-icon kore-icon-home kore-icon-2x");
    $(".elfinder-button-icon.elfinder-button-icon-up").removeClass().addClass("kore-icon kore-icon-level-up kore-icon-2x");
    $(".elfinder-button-icon.elfinder-button-icon-mkfile").removeClass().addClass("kore-icon kore-icon-file kore-icon-2x");
    $(".elfinder-button-icon.elfinder-button-icon-upload").removeClass().addClass("kore-icon kore-icon-upload kore-icon-2x");
    $(".elfinder-button-icon.elfinder-button-icon-open").removeClass().addClass("kore-icon kore-icon-folder-open kore-icon-2x");
    $(".elfinder-button-icon.elfinder-button-icon-download").removeClass().addClass("kore-icon kore-icon-save kore-icon-2x");
    $(".elfinder-button-icon.elfinder-button-icon-info").removeClass().addClass("kore-icon kore-icon-info kore-icon-2x");
    $(".elfinder-button-icon.elfinder-button-icon-quicklook").removeClass().addClass("kore-icon kore-icon-search kore-icon-2x");
    $(".elfinder-button-icon.elfinder-button-icon-copy").removeClass().addClass("kore-icon kore-icon-copy kore-icon-2x");
    $(".elfinder-button-icon.elfinder-button-icon-cut").removeClass().addClass("kore-icon kore-icon-cut kore-icon-2x");
    $(".elfinder-button-icon.elfinder-button-icon-paste").removeClass().addClass("kore-icon kore-icon-paste kore-icon-2x");
    $(".elfinder-button-icon.elfinder-button-icon-rm").removeClass().addClass("kore-icon kore-icon-trash kore-icon-2x");
    $(".elfinder-button-icon.elfinder-button-icon-duplicate").removeClass().addClass("kore-icon kore-icon-duplicate kore-icon-2x");
    $(".elfinder-button-icon.elfinder-button-icon-edit").removeClass().addClass("kore-icon kore-icon-edit kore-icon-2x");
    $(".elfinder-button-icon.elfinder-button-icon-resize").removeClass().addClass("kore-icon kore-icon-resize kore-icon-2x");
    $(".elfinder-button-icon.elfinder-button-icon-view").removeClass().addClass("kore-icon kore-icon-th-list kore-icon-2x");
    $(".elfinder-button-icon.elfinder-button-icon-view.elfinder-button-icon-view-list").removeClass().addClass("kore-icon kore-icon-list kore-icon-2x");
    $(".elfinder-button-icon.elfinder-button-icon-sort").removeClass().addClass("kore-icon kore-icon-sort-alpha-asc kore-icon-2x");

    $(".elfinder-button-icon.elfinder-button-icon-mkdir")
        .removeClass()
        .addClass("kore-icon kore-icon-stack-2x kore-icon-folder-open")
        .parent().addClass("kore-icon-stack kore-icon-lg")
        .append('<span class="kore-icon kore-icon-stack-1x kore-icon-sub kore-icon-add text-primary"></span>');
};

function elFinderBrowserTinyMCE(field_name, url, type, win) {
    tinymce.activeEditor.windowManager.open({
        file: '/admin/media/media-library/browse',// use an absolute path!
        title: 'elFinder 2.0',
        width: 800,
        height: 480,
        resizable: 'yes'
    }, {
        setUrl: function (url) {
            win.document.getElementById(field_name).value = url;
        }
    });
    return false;
}

function elFinderBrowserBootBox() {
    $.get('/admin/media/media-library/browse-partial', function (data) {
        bootbox.dialog({
            message: data,
            title: "elFinder 2.0",
            buttons: {
                danger: {
                    label: "Cancel",
                    className: "btn-default",
                    callback: function () {
                        // Do nothing, just remember to have a imagePickerCallback(url) callback function in your own code
                    }
                }
            }
        });
        elFinderReplaceIcons();
    });
};