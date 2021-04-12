"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();

var _playerId = 0;
var _playerName = '';

connection.on("GameState", function (state, player) {

    updateState(state);
    console.log(state.pressedKey);
});

connection.start().then(function () {
    setInterval(gameThread, 10);
}).catch(function (err) {
    return console.error(err.toString());
});

function gameThread() {
    var pressedKey = '';
    if (pressedKeys[38] === true) {
        pressedKey = 'UP'
    }
    if (pressedKeys[40] === true) {
        pressedKey = 'DOWN'
    }
    connection.invoke("SendKey", parseInt(_playerId), pressedKey, _playerName).catch(function (err) {
        return console.error(err.toString());
    });
}

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

var pressedKeys = {};
window.onkeyup = function (e) { pressedKeys[e.keyCode] = false; }
window.onkeydown = function (e) { pressedKeys[e.keyCode] = true; }