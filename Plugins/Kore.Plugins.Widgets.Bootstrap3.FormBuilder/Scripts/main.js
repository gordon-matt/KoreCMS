require.config({
    baseUrl: "Plugins/Widgets.Bootstrap3.FormBuilder/Scripts/",
    shim: {
        'backbone': {
            deps: ['underscore', 'jquery'],
            exports: 'Backbone'
        },
        'underscore': {
            exports: '_'
        },
        'bootstrap': {
            deps: ['jquery'],
            exports: '$.fn.popover'
        }
    },
    paths: {
        app: "..",
        collections: "../collections",
        data: "../data",
        models: "../models",
        helper: "../helper",
        templates: "../templates",
        views: "../views"
    }
});
require(['app/app'], function (app) {
    app.initialize();
});