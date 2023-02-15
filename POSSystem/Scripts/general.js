function _getOptions(pType, param) {
    var _data = {
        type: pType,
        param: param
    }
    var request = $.ajax({
        type: 'POST',
        dataType: 'json',
        async: false,
        url: SITE_URL + '/Common/getOptions',
        data: JSON.stringify(_data),
        contentType: "application/json; charset=utf-8",
    });
    var result = request.responseJSON;
    return (result && result.data && result.data.length > 0) ? result.data : [];
}

//matcher select2
function _matcher(params, data) {
    // If there are no search terms, return all of the data
    if ($.trim(params.term) === '') {
        return data;
    }

    // Do not display the item if there is no 'text' property
    if (typeof data.text === 'undefined') {
        return null;
    }

    // `params.term` should be the term that is used for searching
    // `data.text` is the text that is displayed for the data object
    if (String(data.id).toLowerCase().indexOf(params.term.toLowerCase()) > -1 || String(data.text).toLowerCase().indexOf(params.term.toLowerCase()) > -1) {
        var modifiedData = $.extend({}, data, true);

        // You can return modified objects from here
        // This includes matching the `children` how you want in nested data sets
        return modifiedData;
    }

    // Return `null` if the term should not be displayed
    return null;
}
