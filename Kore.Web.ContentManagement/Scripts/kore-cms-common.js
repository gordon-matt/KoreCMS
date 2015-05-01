var emptyGuid = '00000000-0000-0000-0000-000000000000';

var currentSection = $("#grid-section");

function switchSection(section) {
    if (section.attr("id") == currentSection.attr("id")) {
        return;
    }
    currentSection.hide("fast");
    section.show("fast");
    currentSection = section;
};

if (typeof String.prototype.startsWith != 'function') {
    String.prototype.startsWith = function (str) {
        return this.slice(0, str.length) == str;
    };
}
if (typeof String.prototype.endsWith != 'function') {
    String.prototype.endsWith = function (str) {
        return this.slice(-str.length) == str;
    };
}

function getLocalStorageKeys() {
    var keys = [];
    for (var i = 0; i < localStorage.length; i++) {
        keys[i] = localStorage.key(i);
    }
    return keys;
};

function escapeRegExp(string) {
    return string.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
}

function replaceAll(string, find, replace) {
    return string.replace(new RegExp(escapeRegExp(find), 'g'), replace);
}

var koreDefaultTinyMCEConfig = {
    theme: "modern",
    plugins: [
        "advlist autolink lists link image charmap print preview hr anchor pagebreak",
        "searchreplace wordcount visualblocks visualchars code fullscreen",
        "insertdatetime media nonbreaking save table contextmenu directionality",
        "emoticons template paste textcolor",
        "kore_contentzone"
    ],
    toolbar1: "insertfile undo redo | styleselect | ltr rtl | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image",
    toolbar2: "print preview media | forecolor backcolor emoticons",
    toolbar3: "contentzone",
    image_advtab: true,
    image_dimensions: false,
    templates: [
        { title: 'Test template 1', content: 'Test 1' },
        { title: 'Test template 2', content: 'Test 2' }
    ],
    //force_br_newlines: false,
    //force_p_newlines: false,
    //forced_root_block: '',
    height: 400,
    file_browser_callback: elFinderBrowserTinyMCE
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

function elFinderReplaceIcons() {
    $(".elfinder-button-icon.elfinder-button-icon-back").removeClass().addClass("fa fa-arrow-left");
    $(".elfinder-button-icon.elfinder-button-icon-forward").removeClass().addClass("fa fa-arrow-right");
    $(".elfinder-button-icon.elfinder-button-icon-reload").removeClass().addClass("fa fa-refresh");
    $(".elfinder-button-icon.elfinder-button-icon-home").removeClass().addClass("fa fa-home");
    $(".elfinder-button-icon.elfinder-button-icon-up").removeClass().addClass("fa fa-level-up");
    $(".elfinder-button-icon.elfinder-button-icon-mkfile").removeClass().addClass("fa fa-file-o");
    $(".elfinder-button-icon.elfinder-button-icon-upload").removeClass().addClass("fa fa-upload");
    $(".elfinder-button-icon.elfinder-button-icon-open").removeClass().addClass("fa fa-folder-open-o");
    $(".elfinder-button-icon.elfinder-button-icon-download").removeClass().addClass("fa fa-save");
    $(".elfinder-button-icon.elfinder-button-icon-info").removeClass().addClass("fa fa-info");
    $(".elfinder-button-icon.elfinder-button-icon-quicklook").removeClass().addClass("fa fa-search");
    $(".elfinder-button-icon.elfinder-button-icon-copy").removeClass().addClass("fa fa-copy");
    $(".elfinder-button-icon.elfinder-button-icon-cut").removeClass().addClass("fa fa-cut");
    $(".elfinder-button-icon.elfinder-button-icon-paste").removeClass().addClass("fa fa-paste");
    $(".elfinder-button-icon.elfinder-button-icon-rm").removeClass().addClass("fa fa-trash-o");
    $(".elfinder-button-icon.elfinder-button-icon-duplicate").removeClass().addClass("fa fa-copy");
    $(".elfinder-button-icon.elfinder-button-icon-edit").removeClass().addClass("fa fa-edit");
    $(".elfinder-button-icon.elfinder-button-icon-resize").removeClass().addClass("fa fa-arrows-alt");
    $(".elfinder-button-icon.elfinder-button-icon-view").removeClass().addClass("fa fa-th-list");
    $(".elfinder-button-icon.elfinder-button-icon-view.elfinder-button-icon-view-list").removeClass().addClass("fa fa-list");
    $(".elfinder-button-icon.elfinder-button-icon-sort").removeClass().addClass("fa fa-sort-alpha-asc");

    $(".elfinder-button-icon.elfinder-button-icon-mkdir")
        .removeClass()
        .addClass("fa fa-stack-1x fa-folder-open-o")
        .parent().addClass("fa-stack")
        .append('<span class="fa fa-stack-1x fa-sub fa-plus text-primary"></span>');
};

function elFinderReplaceIconsLarge() {
    $(".elfinder-button-icon.elfinder-button-icon-back").removeClass().addClass("fa fa-2x fa-arrow-left");
    $(".elfinder-button-icon.elfinder-button-icon-forward").removeClass().addClass("fa fa-2x fa-arrow-right");
    $(".elfinder-button-icon.elfinder-button-icon-reload").removeClass().addClass("fa fa-2x fa-refresh");
    $(".elfinder-button-icon.elfinder-button-icon-home").removeClass().addClass("fa fa-2x fa-home");
    $(".elfinder-button-icon.elfinder-button-icon-up").removeClass().addClass("fa fa-2x fa-level-up");
    $(".elfinder-button-icon.elfinder-button-icon-mkfile").removeClass().addClass("fa fa-2x fa-file-o");
    $(".elfinder-button-icon.elfinder-button-icon-upload").removeClass().addClass("fa fa-2x fa-upload");
    $(".elfinder-button-icon.elfinder-button-icon-open").removeClass().addClass("fa fa-2x fa-folder-open-o");
    $(".elfinder-button-icon.elfinder-button-icon-download").removeClass().addClass("fa fa-2x fa-save");
    $(".elfinder-button-icon.elfinder-button-icon-info").removeClass().addClass("fa fa-2x fa-info");
    $(".elfinder-button-icon.elfinder-button-icon-quicklook").removeClass().addClass("fa fa-2x fa-search");
    $(".elfinder-button-icon.elfinder-button-icon-copy").removeClass().addClass("fa fa-2x fa-copy");
    $(".elfinder-button-icon.elfinder-button-icon-cut").removeClass().addClass("fa fa-2x fa-cut");
    $(".elfinder-button-icon.elfinder-button-icon-paste").removeClass().addClass("fa fa-2x fa-paste");
    $(".elfinder-button-icon.elfinder-button-icon-rm").removeClass().addClass("fa fa-2x fa-trash-o");
    $(".elfinder-button-icon.elfinder-button-icon-duplicate").removeClass().addClass("fa fa-2x fa-copy");
    $(".elfinder-button-icon.elfinder-button-icon-edit").removeClass().addClass("fa fa-2x fa-edit");
    $(".elfinder-button-icon.elfinder-button-icon-resize").removeClass().addClass("fa fa-2x fa-arrows-alt");
    $(".elfinder-button-icon.elfinder-button-icon-view").removeClass().addClass("fa fa-2x fa-th-list");
    $(".elfinder-button-icon.elfinder-button-icon-view.elfinder-button-icon-view-list").removeClass().addClass("fa fa-2x fa-list");
    $(".elfinder-button-icon.elfinder-button-icon-sort").removeClass().addClass("fa fa-2x fa-sort-alpha-asc");

    $(".elfinder-button-icon.elfinder-button-icon-mkdir")
        .removeClass()
        .addClass("fa fa-stack-2x fa-folder-open-o")
        .parent().addClass("fa-stack fa-lg")
        .append('<span class="fa fa-stack-1x fa-sub fa-plus text-primary"></span>');
};

if (typeof tinyMCEContentCss !== 'undefined') {
    koreDefaultTinyMCEConfig.content_css = tinyMCEContentCss;
}

$(document).ready(function () {
    jQuery.validator.setDefaults({
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        }
    });
});

function isFunction(functionToCheck) {
    var getType = {};
    return functionToCheck && getType.toString.call(functionToCheck) === '[object Function]';
}