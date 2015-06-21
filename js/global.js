//*** api constants
var SERVICE_URI_BASE = 'Service.asmx/';

var FUNC_NEW_SIGNATURE = SERVICE_URI_BASE + 'NewSignature';
var FUNC_GET_SIGNATURES = SERVICE_URI_BASE + 'GetSignatures';
var FUNC_NEW_SITE = SERVICE_URI_BASE + 'NewSite';
var FUNC_GET_SITES = SERVICE_URI_BASE + 'GetSites';

//*** ajax constants
var AJAX_TYPE = 'POST'; //*** --- or --- GET
var AJAX_DATATYPE = 'json'; //*** 'jsonp' is required for cross-domain requests; use 'json' for same-domain requests
var AJAX_CONTENTTYPE = 'application/json; charset=utf-8';
var AJAX_TIMEOUT = 60000; //*** 60 seconds


function makeAjaxCall(url, requestJsonData) {

    return $.ajax({
        type: AJAX_TYPE,
        contentType: AJAX_CONTENTTYPE,
        dataType: AJAX_DATATYPE,
        timeout: AJAX_TIMEOUT,
        cache: false,
        abortOnRetry: true,
        url: url,
        data: requestJsonData,
        async: true
    });

}


function convertQueryStringToJSON(qs) {

    var pairs = qs.split('&');

    var result = {};
    pairs.forEach(function (pair) {
        pair = pair.split('=');
        result[pair[0]] = decodeURIComponent(pair[1] || '');
    });

    return JSON.parse(JSON.stringify(result));

}


function checkZipCodeValidation(zip) {

    //console.log(zip);
    var res = /^\d{5}(?:[-\s]\d{4})?$/.test(zip);
    return res;

}


function checkEmailValidation(email) {

    //console.log(email);
    var res = /^([^\x00-\x20\x22\x28\x29\x2c\x2e\x3a-\x3c\x3e\x40\x5b-\x5d\x7f-\xff]+|\x22([^\x0d\x22\x5c\x80-\xff]|\x5c[\x00-\x7f])*\x22)(\x2e([^\x00-\x20\x22\x28\x29\x2c\x2e\x3a-\x3c\x3e\x40\x5b-\x5d\x7f-\xff]+|\x22([^\x0d\x22\x5c\x80-\xff]|\x5c[\x00-\x7f])*\x22))*\x40([^\x00-\x20\x22\x28\x29\x2c\x2e\x3a-\x3c\x3e\x40\x5b-\x5d\x7f-\xff]+|\x5b([^\x0d\x5b-\x5d\x80-\xff]|\x5c[\x00-\x7f])*\x5d)(\x2e([^\x00-\x20\x22\x28\x29\x2c\x2e\x3a-\x3c\x3e\x40\x5b-\x5d\x7f-\xff]+|\x5b([^\x0d\x5b-\x5d\x80-\xff]|\x5c[\x00-\x7f])*\x5d))*$/.test(email);
    return res;

}