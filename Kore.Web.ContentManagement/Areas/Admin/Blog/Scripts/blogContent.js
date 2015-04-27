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

    self.total = ko.observable(0);
    self.pageIndex = ko.observable(1);
    self.pageSize = 5;

    self.pageCount = function () {
        return Math.ceil(self.total() / self.pageSize);
    };

    self.showMain = function () {
        switchSection($('#main-section'));
    };
};

breeze.config.initializeAdapterInstances({ dataService: "OData" });
var manager = new breeze.EntityManager('/odata/kore/cms');

var pagerInitialized = false;

var viewModel;
$(document).ready(function () {
    viewModel = new ViewModel();
    runQuery();
    ko.applyBindings(viewModel);
});

function runQuery() {
    var query = new breeze.EntityQuery()
        .from("Blogs")
        .orderBy("DateCreated desc")
        .skip((viewModel.pageIndex() - 1) * viewModel.pageSize)
        .take(viewModel.pageSize)
        .inlineCount();

    manager.executeQuery(query).then(function (data) {
        viewModel.entries([]);
        viewModel.selected(new BlogEntryModel());
        viewModel.total(data.inlineCount);
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

        if (!pagerInitialized) {
            $('#pager').bootpag({
                total: viewModel.pageCount(),
                page: viewModel.pageIndex(),
                maxVisible: 5,
                leaps: true,
                firstLastUse: true,
            }).on("page", function (event, num) {
                viewModel.pageIndex(num);
                runQuery();
            });
            pagerInitialized = true;
        }
    }).fail(function (e) {
        alert(e);
    });
};