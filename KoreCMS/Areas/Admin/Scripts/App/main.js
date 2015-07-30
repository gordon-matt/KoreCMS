var paths = {
    'text': '../../../../Scripts/text',
    'durandal': '../../../../Scripts/durandal',
    'plugins': '../../../../Scripts/durandal/plugins',
    'transitions': '../../../../Scripts/durandal/transitions',

    //'bootstrap': '../../../../Scripts/bootstrap',
    'breeze': '../../../../Scripts/breeze.debug',
    'chosen': '../../../../Scripts/chosen.jquery',
    'datajs': '../../../../Scripts/datajs-1.1.3',
    'jqueryval': '../../../../Scripts/jquery.validate',
    'jqueryval-unobtrusive': '../../../../Scripts/jquery.validate.unobtrusive',
    'kendo': '../../../../Scripts/kendo/2014.1.318/kendo.web.min',
    'kendo-knockout': '../../../../Scripts/knockout-kendo.min',
    'knockout-mapping': '../../../../Scripts/knockout.mapping-latest.debug',
    'notify': '../../../../Scripts/notify.min',
    'OData': '../../../../Scripts/datajs-1.1.3',
    'Q': '../../../../Scripts/q',
    'tinymce': '../../../../Scripts/tinymce/tinymce.min',
    'tinymce-jquery': '../../../../Scripts/tinymce/jquery.tinymce.min',
    'tinymce-knockout': '../../../../Scripts/wysiwyg.min',
};

var shim = {
    //'bootstrap': ['jquery'],
    'breeze': ['jquery', 'datajs', 'Q'],
    'datajs': ['jquery'],
    'jqueryval': ['jquery'],
    'jqueryval-unobtrusive': ['jquery', 'jqueryval'],
    'kendo-knockout': ['kendo', 'knockout'],
    'knockout-mapping': ['knockout'],
    'OData': ['datajs'],
    'tinymce-jquery': ['jquery', 'tinymce'],
    'tinymce-knockout': ['knockout', 'tinymce', 'tinymce-jquery']
};

$.ajax({
    url: "/admin/get-requirejs-config",
    type: "GET",
    dataType: "json",
    async: false
}).done(function (json) {
    for (item in json.Paths) {
        if (json.Paths[item]) {
            paths[item] = json.Paths[item];
        }
        else {
            paths[item] = "viewmodels/dummy";
        }
    }
    for (item in json.Shim) {
        shim[item] = json.Shim[item];
    }
}).fail(function (jqXHR, textStatus, errorThrown) {
    console.log(textStatus + ': ' + errorThrown);
});

requirejs.config({
    paths: paths,
    shim: shim
});

define('jquery', function () { return jQuery; });
define('knockout', ko);

define(function (require) {
    var system = require('durandal/system');
    var app = require('durandal/app');
    var viewLocator = require('durandal/viewLocator');
    var viewEngine = require('durandal/viewEngine');
    var binder = require('durandal/binder');
    //require('bootstrap');

    //>>excludeStart("build", true);
    system.debug(true);
    //>>excludeEnd("build");

    app.title = 'Kore CMS';

    app.configurePlugins({
        router: true,
        dialog: true
    });

    app.start().then(function() {
        //Replace 'viewmodels' in the moduleId with 'views' to locate the view.
        //Look for partial views in a 'views' folder in the root.
        viewEngine.viewExtension = '/';

        viewLocator.convertModuleIdToViewId = function (moduleId) {
            //console.log('!  Trying to find view for: ' + moduleId);
            return moduleId.replace('viewmodels', '');
        };

        //// As per http://durandaljs.com/documentation/KendoUI.html
        //kendo.ns = "kendo-";
        //binder.binding = function (obj, view) {
        //    kendo.bind(view, obj.viewModel || obj);
        //};

        //Show the app by setting the root view model for our application with a transition.
        app.setRoot('viewmodels/admin/shell', 'entrance');
    });
});