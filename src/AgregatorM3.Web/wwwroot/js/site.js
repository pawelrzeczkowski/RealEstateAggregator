var addToBlackList = function (value) {
    $.post(
        '/home/blackList', {item : value }
    );
};

$('.btn-danger').click(function() {
    $(this).closest("tr").remove();
});

