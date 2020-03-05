"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/dynamicResultsHub").build();

//Disable send button until connection is established
document.getElementById("search").disabled = true;

connection.on("ReceiveMessage", function (serviceName, message) {
    console.log('dupa1');
    var htmlContent = "<tr><td><a href=\"" + message + "\" target=\"_blank\">" + message + "</a></td>" +
        "<td><button type=\"button\" class=\"btn btn-danger btn-wide\" onclick=\"addToBlackList('" + message + "');\">remove</button></td>" +
        "<td><button type=\"button\" class=\"btn btn-success btn-wide\" onclick=\"addToWhiteList('" + message + "');\">like</button></td></tr>";
    document.getElementById("resultsTbody").insertAdjacentHTML('beforeend', htmlContent);

});

connection.start().then(function () {
    document.getElementById("search").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});