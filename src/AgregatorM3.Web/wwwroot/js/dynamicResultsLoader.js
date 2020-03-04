"use strict";
console.log('000dupa1');

var connection = new signalR.HubConnectionBuilder().withUrl("/dynamicResultsHub").build();
console.log('000dupa2');

//Disable send button until connection is established
document.getElementById("search").disabled = true;

connection.on("ReceiveMessage", function (serviceName, message) {
    console.log('000dupa3');
    var li = document.createElement("li");
    li.textContent = message;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("search").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("search").addEventListener("click", function (event) {
    connection.invoke("SendMessage", "init", "action").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});