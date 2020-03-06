var addToBlackList = function (value) {
    $.post(
        '/search/addToBlacklist',
        { item: value }
    );
};

var addToWhiteList = function (value) {
    $.post(
        '/search/addTowhiteList',
        { item: value }
    );
};

var addToBlackListAndRemoveFromWhiteList = function (value) {
    $.post('/search/addToBlacklist',
        { item: value }
    );
    $.post(
        '/search/removeFromWhitelist',
        { item: value }
    );
};

$('#search').click(function (event) {
    $.post('/search/GetData',
        { parameters: {
            priceMin: $('#priceMin').val(),
            priceMax: $('#priceMax').val()
        }}
    );
    event.preventDefault();
});

$(document).on("click", ".btn", function () {
    $(this).closest("tr").remove();
});


