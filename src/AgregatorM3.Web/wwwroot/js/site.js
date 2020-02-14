var addToBlackList = function (value) {
    $.post(
        '/home/addToBlacklist', {item : value }
    );
};

var addToWhiteList = function (value) {
    $.post(
        '/home/addTowhiteList', { item: value }
    );
};

$('.btn').click(function() {
    $(this).closest("tr").remove();
});


