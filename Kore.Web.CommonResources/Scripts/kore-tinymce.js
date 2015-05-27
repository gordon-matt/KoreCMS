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

if (typeof tinyMCEContentCss !== 'undefined') {
    koreDefaultTinyMCEConfig.content_css = tinyMCEContentCss;
}