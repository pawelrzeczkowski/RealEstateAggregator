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

$(document).on("click", "#whitelist-remove-btn", function () {
    let value = getUrl(this);
    addToBlacklist(value);
    removeFromWhiteList(value);
});

$(document).on("click", "#whitelist-add-btn", function () {
    let value = getUrl(this);
    addToWhiteList(value);
    decrementResultCounter();
});

$(document).on("click", "#blacklist-add-btn", function () {
    let value = getUrl(this);
    addToBlacklist(value);
    decrementResultCounter();
});

function getUrl(el) {
    return $(el).closest('tr').find('td:first-child').text();
}

function addToWhiteList(value) {
    $.post('/search/addToWhiteList', { item: value });
}

function removeFromWhiteList(value) {
    $.post('/search/removeFromWhitelist', { item: value });
}

function addToBlacklist(value) {
    $.post('/search/addToBlacklist', { item: value });
}

function decrementResultCounter() {
    let newVal = parseInt(document.getElementById('offersCounter').innerText, 10) - 1;
    document.getElementById("offersCounter").innerText  = newVal;
}


