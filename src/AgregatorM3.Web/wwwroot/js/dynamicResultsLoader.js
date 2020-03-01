"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/dynamicResultsHub").build();

//Disable send button until connection is established
document.getElementById("search").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    li.textContent = message;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("search").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});