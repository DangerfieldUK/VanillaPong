var VP = extend(true, {}, VP);

VP.Game = {
    State: {},
    PressedKeys: {},
    PressedKey: '',
    Player: 1,

    Element_GameContainer: document.getElementsByClassName("game")[0],

    Element_Scores: document.getElementsByClassName('scores')[0],

    Element_Player1_Paddle: document.getElementsByClassName('player1')[0],
    Element_Player1Name: document.getElementById('player1name'),
    Element_Player1Score: document.getElementById('player1score'),

    Element_Player2_Paddle: document.getElementsByClassName('player2')[0],
    Element_Player2Name: document.getElementById('player2name'),
    Element_Player2Score: document.getElementById('player2score'),

    Element_Ball: document.getElementsByClassName('ball')[0],

    Element_StartPlay: document.getElementById('startplay'),

    UpdateNamesAndScoresFromState: function () {
        VP.Game.Element_Player1Score.innerHTML = VP.Game.State.player1Score;
        VP.Game.Element_Player2Score.innerHTML = VP.Game.State.player2Score;
        VP.Game.Element_Player1Name.innerHTML = VP.Game.State.player1Name;
        VP.Game.Element_Player2Name.innerHTML = VP.Game.State.player2Name;
    },

    StateUpdate: function () {
        if (VP.Game.State.readyToStart === true && VP.Game.State.inPlay === false) {
            VP.Game.Element_StartPlay.style.display = 'block';
        }
    },

    SetupListeners: function () {
        // Server sent lobby state update
        VP.Shared.Connection.on('StateUpdate', function (state) {
            State = state;
            VP.Game.StateUpdate();
        });
    },

    SetupControls: function () {
        window.onkeyup = function (e) { VP.Game.PressedKeys[e.keyCode] = false; }
        window.onkeydown = function (e) { VP.Game.PressedKeys[e.keyCode] = true; }
    },

    CheckPressedKey: function () {
        VP.Game.PressedKey = '';
        if (VP.Game.PressedKeys[38] === true) {
            VP.Game.PressedKey = 'UP'
        }
        if (VP.Game.PressedKeys[40] === true) {
            VP.Game.PressedKey = 'DOWN'
        }
        if (VP.Game.PressedKeys[32] === true) {
            VP.Game.PressedKey = 'SPACE'
        }
    },

    MyBat: function () {
        switch (VP.Game.Player) {
            case 1:
                return VP.Game.Element_Player1_Paddle;

            case 2:
                return VP.Game.Element_Player2_Paddle;
        }
    },

    MoveBat: function () {
        switch (VP.Game.PressedKey) {
            case 'UP':
                var topVal = parseInt(VP.Game.MyBat().style.top, 10);
                if (topVal > 0)
                    VP.Game.MyBat().style.top = (topVal - 2) + "px";
                break;

            case 'DOWN':
                var topVal = parseInt(VP.Game.MyBat().style.top, 10);
                if (topVal < 425)
                    VP.Game.MyBat().style.top = (topVal + 2) + "px";
                break;
        }
    },

    SendServerBatLocation: function() {

    },

    ClientLoop: function () {
        VP.Game.CheckPressedKey();
        VP.Game.MoveBat();
        VP.Game.SendServerBatLocation();
    },

    Init: function () {
        VP.Game.SetupListeners();
        VP.Game.SetupControls();
        VP.Game.UpdateNamesAndScoresFromState();
        setInterval(VP.Game.ClientLoop, 10);
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




