"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/dynamicResultsHub").build();

//Disable send button until connection is established
document.getElementById("search").disabled = true;

connection.on("DisplayOfferLink", function (resultCounter, message) {

    let htmlContent = "<tr><td><a href=\"" + message + "\" target=\"_blank\">" + message + "</a></td>" +
        "<td><button type=\"button\" id=\"blacklist-add-btn\" class=\"btn btn-danger btn-wide\">remove</button></td>" +
        "<td><button type=\"button\" id=\"whitelist-add-btn\" class=\"btn btn-success btn-wide\">like</button></td></tr>";
    document.getElementById("resultsTbody").insertAdjacentHTML('beforeend', htmlContent);
    document.getElementById("offersCounter").innerText = resultCounter;
});

connection.on("DisplayProgress", function (message) {
    document.getElementById("progression").innerText = message;
});


connection.start().then(function () {
    document.getElementById("search").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});