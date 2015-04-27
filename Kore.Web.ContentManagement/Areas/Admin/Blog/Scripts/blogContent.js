'use strict'

var currentSection = $("#main-section");

function switchSection(section) {
    if (section.attr("id") == currentSection.attr("id")) {
        return;
    }
    currentSection.hide("fast");
    section.show("fast");
    currentSection = section;
};

var BlogEntryModel = function () {
    var self = this;

    self.headline = ko.observable('');
    self.userName = ko.observable('');
    self.dateCreated = ko.observable('');
    self.shortDescription = ko.observable('');
    self.fullDescription = ko.observable('');

    self.showDetails = function () {
        viewModel.selected(self);
        switchSection($('#details-section'));
    };
};

var ViewModel = function () {
    var self = this;
    self.entries = ko.observableArray([]);
    self.selected = ko.observable(new BlogEntryModel());

    self.showMain = function () {
        switchSection($('#main-section'));
    };
};

var viewModel;
$(document).ready(function () {
    viewModel = new ViewModel();

    breeze.config.initializeAdapterInstances({ dataService: "OData" });
    var manager = new breeze.EntityManager('/odata/kore/cms');
    var query = new breeze.EntityQuery().from("Blogs");

    manager.executeQuery(query).then(function (data) {
        $(data.httpResponse.data.results).each(function () {
            var current = this;
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