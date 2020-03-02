var addToBlackList = function (value) {
    $.post(
        '/search/addToBlacklist', {item : value }
    );
};

var addToWhiteList = function (value) {
    $.post(
        '/search/addTowhiteList', { item: value }
    );
};

var addToBlackListAndRemoveFromWhiteList = function (value) {
    $.post(
        '/search/addToBlacklist', { item: value }
    );
    $.post(
        '/search/removeFromWhitelist', { item: value }
    );
};

$('.btn').click(function() {
    $(this).closest("tr").remove();
});


