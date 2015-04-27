'use strict'

var BlogEntryModel = function () {
    var self = this;

    self.headline = ko.observable('');
    self.userName = ko.observable('');
    self.dateCreated = ko.observable('');
    self.shortDescription = ko.observable('');
    self.fullDescription = ko.observable('');
};

var ViewModel = function () {
    var self = this;
    self.entries = ko.observableArray([]);
};

var viewModel;
$(document).ready(function () {
    viewModel = new ViewModel();

    breeze.config.initializeAdapterInstances({ dataService: "OData" });
    var manager = new breeze.EntityManager('/odata/kore/cms');
    var query = new breeze.EntityQuery().from("Blogs");

    manager.executeQuery(query).then(function (data) {
        $.each(data.httpResponse.data.results, function () {
            var current = $(this).children().first();
            alert(JSON.stringify(current));
            var entry = new BlogEntryModel();
            entry.headline(current.Headline);
            entry.userName(current.UserName);
            entry.dateCreated(current.DateCreated);
            entry.shortDescription(current.ShortDescription);
            entry.fullDescription(current.FullDescription);
            viewModel.entries.push(entry);
        });

        ko.applyBindings(viewModel);
    }).fail(function (e) {
        alert(e);
    });
});