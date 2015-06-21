var PARAMS = {};
var REQUEST_DATA;
var GRID;

$(document).ready(function () {

    gridInit();

});


function gridInit() {

    //-------------------------
    //*** Grid Binding & Events
    //-------------------------

    var gridDatasource = new kendo.data.DataSource({
        transport: {

            read: function (request) {

                PARAMS = {};
                //PARAMS.Ticket = TICKET;
                REQUEST_DATA = JSON.stringify(PARAMS);

                var ajaxCall = makeAjaxCall(FUNC_GET_SITES, REQUEST_DATA);
                ajaxCall.success(function (responseJsonData) {
                    request.success(responseJsonData); // notify the kendo datasource that the request succeeded
                    var dataObj = responseJsonData.d;
                    if (dataObj[0]) {
                        var response = dataObj[0];
                        if (response.Error) { //*** if response is NOT undefined
                            console.log(response); //*** used for debugging and authentication failure action
                        } else {

                        }
                    }
                });
                ajaxCall.error(function (responseJsonData) {
                    request.error(responseJsonData); //*** notify the kendo datasource that the request failed
                });

            }, //*** read ends

            parameterMap: function (request, operation) {
                if (operation !== "read" && request.models) {
                    return {
                        models: kendo.stringify(request.models)
                    };
                }
            }

        }, //*** transport ends
        batch: false, //*** cannot be true in this setup.
        autoSync: false,
        pageSize: 20,
        schema: {
            data: 'd',
            total: function (result) { //*** without it pager does not initiate on page load.
                var data = this.data(result);
                return data ? data.length : 0;
            }
        }

    }); //*** dataSource ends


    $('#grid').kendoGrid({
        dataSource: gridDatasource,
        pageable: {
            refresh: true,
            info: true,
            buttonCount: 5
        },
        sortable: false, //*** messes up things when performed while adding a new row.
        reorderable: false,
        selectable: 'row', //*** necessary to send params from editables
        scrollable: false,
        toolbar: kendo.template($('#toolbar_template').html())
    });


    $('.k-grid-ClearSearch', '#grid').bind('click', function (e) {

        //*** clear search filter and elements
        var datasource = $('#grid').data('kendoGrid').dataSource;
        datasource.filter({});

        $('#searchbox').val('').focus();
        $('.k-grid-ClearSearch', '#grid').css('visibility', 'hidden');

    });


    $('#searchbox').keyup(function (e) {

        var searchText = $('#searchbox').val();
        var clearSearchBtn = $('.k-grid-ClearSearch', '#grid');
        var datasource = $('#grid').data('kendoGrid').dataSource;

        if (searchText.length == 0) {

            clearSearchBtn.css('visibility', 'hidden');
            datasource.filter({});

        } else {

            clearSearchBtn.css('visibility', 'visible');
            datasource.filter({
                logic: 'or',
                filters: [
                    {
                        field: 'Domain',
                        operator: 'contains',
                        value: searchText
                    },
                    {
                        field: 'Title',
                        operator: 'contains',
                        value: searchText
                    },
                    {
                        field: 'Email',
                        operator: 'contains',
                        value: searchText
                    }
                ]
            });

        }

    });
    
}