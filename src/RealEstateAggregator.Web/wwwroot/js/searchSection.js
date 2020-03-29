$('#search').click(function (event) {
    getData();
    collapseSearch();
    showResultTable();
    //event.preventDefault();
});

$('.collapsible').click(function () {
    let collapse = document.getElementsByClassName("collapsible");
    let content = collapse[0].nextElementSibling;
    if (content.style.maxHeight) {
        content.style.maxHeight = null;
    } else {
        content.style.maxHeight = content.scrollHeight + "px";
    }
});

function getData() {
    $.post('/search/GetData', { parameters: {
        PriceFrom: $('#PriceFrom').val(),
        PriceTo: $('#PriceTo').val(),
        SurfaceFrom: $('#SurfaceFrom').val(),
        SurfaceTo: $('#SurfaceTo').val(),
        RoomsFrom: $('#RoomsFrom').val(),
        RoomsTo: $('#RoomsTo').val(),
        PricePerMeterTo: $('#PricePerMeterTo').val()
    }});
}

function showResultTable() {
    document.getElementById("resultTable").style.display = "table"; 
}

function collapseSearch() {
    let collapse = document.getElementsByClassName("collapsible");
    let content = collapse[0].nextElementSibling;
    content.style.maxHeight = null;
}

