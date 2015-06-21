'use strict'

$(document).ready(function () {
    $("#Grid").kendoGrid({
        data: null,
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: "/odata/kore/web/ThemeApi",
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
                        PreviewImageUrl: { type: "string" },
                        Title: { type: "string" },
                        PreviewText: { type: "string" },
                        SupportRtl: { type: "boolean" },
                        MobileTheme: { type: "boolean" },
                        IsDefaultDesktopTheme: { type: "boolean" },
                        IsDefaultMobileTheme: { type: "boolean" }
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
            field: "PreviewImageUrl",
            title: translations.Columns.PreviewImageUrl,
            template: '<img src="#=PreviewImageUrl#" alt="#=Title#" class="thumbnail" style="max-width:200px;" />',
            filterable: false,
            width: 200
        }, {
            field: "Title",
            title: translations.Columns.Title,
            filterable: true
        }, {
            field: "SupportRtl",
            title: translations.Columns.SupportRtl,
            template: '<i class="kore-icon #=SupportRtl ? \'kore-icon-ok text-success\' : \'kore-icon-no text-danger\'#"></i>',
            attributes: { "class": "text-center" },
            filterable: true,
            width: 70
        }, {
            field: "MobileTheme",
            title: translations.Columns.MobileTheme,
            template: '<i class="kore-icon #=MobileTheme ? \'kore-icon-ok text-success\' : \'kore-icon-no text-danger\'#"></i>',
            attributes: { "class": "text-center" },
            filterable: true,
            width: 70
        }, {
            field: "IsDefaultDesktopTheme",
            title: translations.Columns.IsDefaultDesktopTheme,
            template:
                '# if(IsDefaultDesktopTheme) {# <i class="kore-icon kore-icon-ok-circle kore-icon-2x text-success"></i> #} ' +
                'else {# <a href="javascript:void(0);" onclick="setDesktopTheme(\'#=Title#\')" class="btn btn-default btn-sm">#=translations.Set#</a> #} #',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 130
        }, {
            field: "IsDefaultMobileTheme",
            title: translations.Columns.IsDefaultMobileTheme,
            template:
                '# if(IsDefaultMobileTheme) {# <i class="kore-icon kore-icon-ok-circle kore-icon-2x text-success"></i> #} ' +
                'else {# <a href="javascript:void(0);" onclick="setMobileTheme(\'#=Title#\')" class="btn btn-default btn-sm">#=translations.Set#</a> #} #',
            attributes: { "class": "text-center" },
            filterable: false,
            width: 130
        }]
    });
});

function setDesktopTheme(name) {
    $.ajax({
        url: "/odata/kore/web/ThemeApi/SetDesktopTheme",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ themeName: name }),
        dataType: "json",
        async: false
    })
    .done(function (json) {
        $('#Grid').data('kendoGrid').dataSource.read();
        $('#Grid').data('kendoGrid').refresh();
        $.notify(translations.SetDesktopThemeSuccess, "success");
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        $.notify(translations.SetDesktopThemeError + ": " + jqXHR.responseText || textStatus, "error");
    });
}

function setMobileTheme(name) {
    $.ajax({
        url: "/odata/kore/web/ThemeApi/SetMobileTheme",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ themeName: name }),
        dataType: "json",
        async: false
    })
    .done(function (json) {
        $('#Grid').data('kendoGrid').dataSource.read();
        $('#Grid').data('kendoGrid').refresh();
        $.notify(translations.SetMobileThemeSuccess, "success");
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        $.notify(translations.SetMobileThemeError + ": " + jqXHR.responseText || textStatus, "error");
    });
}