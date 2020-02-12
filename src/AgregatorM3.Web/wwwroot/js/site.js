var addToBlackList = function (item) {
    var dataToPost = '{item: "' + item + '" }';
    $.ajax({
        url: '/home/blackList',
        type: 'POST',
        data: dataToPost,
        contentType: "application/json; charset=utf-8",
        dataType: "json"
});
};

$('.btn-danger').click(function() {
    $(this).closest("tr").remove();
});

