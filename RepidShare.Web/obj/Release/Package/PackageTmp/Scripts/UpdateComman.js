function openEditDialog(url, titleText, dialogID, removeClasses) {
  
    $.ajax({
        type: "GET",
        url: encodeURI(url),
        cache: false,
        dataType: 'html',
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            // Security Issue Point 12, 12Jan2015
            if (XMLHttpRequest.status != 403) {
                $("#modal-body-content").html(XMLHttpRequest.responseText);
            }
        },
        success: function (data, textStatus, XMLHttpRequest) {
            $("#modal-body-content").html(data);
        },
        complete: function (XMLHttpRequest, textStatus) {
            //            $("#modal-body-content").html($("#" + dialogID).html());
            $("#modal-body-title").addClass("modelbox-title").html(titleText);
            //added by Lakhan for Removing class in case of full dialog and medium dialog            
            if (removeClasses == null) {
                $('#light-box').removeClass("model-full");
                $('#light-box').removeClass("model-medium");
            }
            $('#light-box').modal('toggle');
           
        }
    });

}