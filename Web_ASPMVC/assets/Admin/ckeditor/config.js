/// <reference path="../ckfinder/ckfinder.html" />
/// <reference path="../ckfinder/ckfinder.html" />
/**
 * @license Copyright (c) 2003-2020, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';
    config.syntaxhighlight_lang = 'csharp';
    config.syntaxhighlight_hideControls = true;
    config.language = 'vi';
    config.filebrowserBrowseUrl = '/assets/admin/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = '/assets/admin/ckfinder.html?Type=Images';
    config.filebrowserFlashBrowseUrl = '/assets/admin/ckfinder.html?Type=Flash';
    config.filebrowserUploadUrl = '/assets/admin/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '/Data_Img';
    config.filebrowserFlashUploadUrl = '/assets/admin/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';

    CKFinder.setupCKEditor(null, '/assets/admin/ckfinder/'); //trỏ từ CKEditor đến CKFinder
};