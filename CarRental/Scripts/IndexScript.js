function ShowImagePreview(imageUploader, previewImage) {
    if (imageUploader.files && imageUploader.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src', e.target.result);
        }
        reader.readAsDataURL(imageUploader.files[0]);
    }
}

function refreshAddNewTab(resetUrl, showViewTab) {
    $.ajax({
        type: 'GET',
        url: resetUrl,
        success: function (response) {
            $("#AddCar").html(response);
            $('ul.nav.nav-tabs a:eq(1)').html('Add New');
            if (showViewTab)
                $('ul.nav.nav-tabs a:eq(0)').tab('show');
        }

    });
}

function Edit(url) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {
            $("#AddCar").html(response);
            $('ul.nav.nav-tabs a:eq(1)').html('Редактировать');
            $('ul.nav.nav-tabs a:eq(1)').tab('show');
        }

    });
}

//function jQueryAjaxPost(form) {
//    $.validator.unobtrusive.parse(form);
//    if ($(form).valid()) {
//        var ajaxConfig = {
//            type: 'POST',
//            url: form.action,
//            data: new FormData(form),
//            success: function (response) {
//                if (response.success) {
//                    $("#listCar").html(response.html);
//                    refreshAddNewTab($(form).attr('data-restUrl'), true);
//                    $.notify(response.message, "success");
//                    if (typeof activatejQueryTable !== 'undefined' && $.isFunction(activatejQueryTable))
//                        activatejQueryTable();
//                }
//                else {
//                    $.notify(response.message, "error");
//                }
//            }
//        }
//        if ($(form).attr('enctype') == "multipart/form-data") {
//            ajaxConfig["contentType"] = false;
//            ajaxConfig["processData"] = false;
//        }
//        $.ajax(ajaxConfig);

//    }
//    return false;
//}
