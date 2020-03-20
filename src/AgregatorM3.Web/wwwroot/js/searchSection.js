$('#search').click(function (event) {
    getData();
    event.preventDefault();
});

function getData() {
    $.post('/search/GetData', { parameters: {
        priceMin: $('#priceMin').val(),
        priceMax: $('#priceMax').val()
    }});
}
