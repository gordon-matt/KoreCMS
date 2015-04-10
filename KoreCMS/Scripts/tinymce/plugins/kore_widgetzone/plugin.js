tinymce.PluginManager.add('kore_widgetzone', function (editor, url) {
    // Add a button that opens a window
    editor.addButton('widgetzone', {
        title: 'Widget Zone',
        image: url + '/img/kore_widgetzone.png',
        onclick: function () {
            // Open window
            editor.windowManager.open({
                title: 'Kore Widget Zone Plugin',
                body: [
                    { type: 'textbox', name: 'zone', label: 'Zone Name' }
                ],
                onsubmit: function (e) {
                    // Insert content when the window form is submitted
                    editor.insertContent('[[WidgetZone:' + e.data.zone + ']]');
                }
            });
        }
    });
});