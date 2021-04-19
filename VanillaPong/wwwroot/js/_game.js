var VP = extend(true, {}, VP);

VP.Game = {
    State: {},

    Element_GameContainer: document.getElementsByClassName("game")[0],

    Element_Scores: document.getElementsByClassName('scores')[0],
    Element_Player1_Score: document.getElementsByClassName('score1')[0],
    Element_Player2_Score: document.getElementsByClassName('score2')[0],

    Element_Player1_Paddle: document.getElementsByClassName('player1')[0],
    Element_Player2_Paddle: document.getElementsByClassName('player2')[0],

    Element_Ball: document.getElementsByClassName('ball')[0],

    UpdateNamesAndScoresFromState: function () {
        VP.Game.Element_Player1_Score.innerHTML = VP.Game.State.player1Name + ': ' + VP.Game.State.player1Score;
        VP.Game.Element_Player2_Score.innerHTML = VP.Game.State.player2Name + ': ' + VP.Game.State.player2Score;
    }
};






////connection.start().then(function () {
////    setInterval(gameThread, 10);
////}).catch(function (err) {
////    return console.error(err.toString());
////});

////function gameThread() {
////    var pressedKey = '';
////    if (pressedKeys[38] === true) {
////        pressedKey = 'UP'
////    }
////    if (pressedKeys[40] === true) {
////        pressedKey = 'DOWN'
////    }
////    connection.invoke("SendKey", parseInt(_playerId), pressedKey, _playerName).catch(function (err) {
////        return console.error(err.toString());
////    });
////}

////var pressedKeys = {};
////window.onkeyup = function (e) { pressedKeys[e.keyCode] = false; }
////window.onkeydown = function (e) { pressedKeys[e.keyCode] = true; }


