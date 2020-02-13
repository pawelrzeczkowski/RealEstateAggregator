var addToBlackList = function (value) {
    $.post(
        '/home/blackList', {item : value }
    );
};

var addToWhiteList = function (value) {
    $.post(
        '/home/whiteList', { item: value }
    );
};

$('.btn').click(function() {
    $(this).closest("tr").remove();
});


