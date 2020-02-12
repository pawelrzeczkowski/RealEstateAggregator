var addToBlackList = function (item) {
    var itemToRemove = item;
    $.ajax({
        url: '/home/AddToBlacklist',
        type: 'POST',
        data: JSON.stringify({ 'item': itemToRemove }),
        contentType: "application/json"
});
};

$('.btn-danger').click(function() {
    $(this).closest("tr").remove();
});

