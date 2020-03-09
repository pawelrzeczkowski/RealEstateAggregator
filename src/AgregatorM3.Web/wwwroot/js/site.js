var addToBlackList = function (value) {
    $.post(
        '/search/addToBlacklist',
        { item: value }
    );
    decrementResultCounter();
};

var addToWhiteList = function (value) {
    $.post(
        '/search/addTowhiteList',
        { item: value }
    );
    decrementResultCounter();
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

function decrementResultCounter() {
    let newVal = parseInt(document.getElementById('offersCounter').innerText, 10) - 1
    document.getElementById("offersCounter").innerText  = newVal;
}


