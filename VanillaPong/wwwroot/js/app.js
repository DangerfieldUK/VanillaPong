"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
connection.start().then(function () {
    document.getElementById("connecting").style.display = "none";
    document.getElementsByClassName("lobbies")[0].style.display = "block";
}).catch(function (err) {
    return alert(err.toString());
});
var _playerName = '';
var _lobbyName = '';

// on name entry continue button click
document.getElementById("nameSubmit").onclick = (function () {
    // verify name is not blank or two short
    if (_playerName.length < 3)
        return alert("Name must be three characters or more.");

    connection.invoke("CheckName", _playerName).catch(function (err) {
        return alert(err.toString());
    });
});

// verify name is not taken
connection.on("CheckName", function (ok) {
    if (ok === true) {
        // hide lobbies-player and show lobbies-panel
        // join a group called lobbies in signal r so that you are subscribed to lobby updates
        document.getElementsByClassName("lobbies-player")[0].style.display = "none";
        document.getElementsByClassName("lobbies-panel")[0].style.display = "block";
        document.getElementById("name-display").innerText = _playerName;
    } else {
        alert("Sorry, that name is taken.. please try another.")
    }
});

// update the local list of lobbies
connection.on("LobbyUpdate", function (lobbies) {
    console.log(lobbies);
});

// on create lobby button click
document.getElementById("newLobbyCreate").onclick = (function () {
    // verify name is not blank or two short
    if (_lobbyName.length < 5)
        return alert("Name must be five characters or more.");

    connection.invoke("CheckLobbyName", _lobbyName, _playerName).catch(function (err) {
        return alert(err.toString());
    });
});

// verify name is not taken
connection.on("CheckLobbyName", function (ok) {
    if (ok === true) {
        // hide lobbies-player and show lobbies-panel
        // join a group called lobbies in signal r so that you are subscribed to lobby updates
        document.getElementsByClassName("lobbies")[0].style.display = "none";
        document.getElementsByClassName("game")[0].style.display = "block";
    } else {
        alert("Sorry, that lobby already exists.. please try another name.")
    }
});


// show all open lobbies (lobbies awaiting a competitor)
// when you click join on a lobby
// join the game group
// leave the lobbies group
// hide the lobbies panel
// show the game
// begin play


connection.on("GameState", function (state) {
    updateState(state);
});



//connection.on("GameState", function (state, player) {

//    updateState(state);
//    console.log(state.pressedKey);
//});

//connection.start().then(function () {
//    setInterval(gameThread, 10);
//}).catch(function (err) {
//    return console.error(err.toString());
//});

//function gameThread() {
//    var pressedKey = '';
//    if (pressedKeys[38] === true) {
//        pressedKey = 'UP'
//    }
//    if (pressedKeys[40] === true) {
//        pressedKey = 'DOWN'
//    }
//    connection.invoke("SendKey", parseInt(_playerId), pressedKey, _playerName).catch(function (err) {
//        return console.error(err.toString());
//    });
//}

function updateState(state) {
    document.getElementsByClassName('score1')[0].innerHTML = state.player1Name + ': ' +  state.player1Score;
    document.getElementsByClassName('score2')[0].innerHTML = state.player2Name + ': ' + state.player2Score;

    // set the player paddle positions
    document.getElementsByClassName('player1')[0].style.top = state.player1PosTop + 'px';
    document.getElementsByClassName('player2')[0].style.top = state.player2PosTop + 'px';

    // set the ball position
    document.getElementsByClassName('ball')[0].style.top = state.ballTop;
    document.getElementsByClassName('ball')[0].style.left = state.ballLeft;
}

//var pressedKeys = {};
//window.onkeyup = function (e) { pressedKeys[e.keyCode] = false; }
//window.onkeydown = function (e) { pressedKeys[e.keyCode] = true; }