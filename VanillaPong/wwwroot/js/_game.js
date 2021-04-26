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
        VP.Game.UpdateNamesAndScoresFromState();
        if (VP.Game.State.readyToStart === true && VP.Game.State.inPlay === false) {
            VP.Game.Element_StartPlay.style.display = 'block';
        }
    },

    SetupListeners: function () {
        // Server sent lobby state update
        VP.Shared.Connection.on('StateUpdate', function (state) {
            VP.Game.State = state;
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

    OpponentBat: function () {
        switch (VP.Game.Player) {
            case 1:
                return VP.Game.Element_Player2_Paddle;

            case 2:
                return VP.Game.Element_Player1_Paddle;
        }
    },

    OpponentLocations: function () {
        switch (VP.Game.Player) {
            case 1:
                return VP.Game.State.player2Locations;

            case 2:
                return VP.Game.State.player1Locations;
        }
    },

    OpponentLastLocTimeStamp: 0,

    MoveOpponent: function () {
        for (var i = 0; i < VP.Game.OpponentLocations().length; i++) {
            var loc = VP.Game.OpponentLocations()[i];
            if (VP.Game.OpponentLastLocTimeStamp === 0) {
                VP.Game.OpponentLastLocTimeStamp = loc.timeStamp;
                VP.Game.OpponentBat().style.top = loc.position + "px";
            } else {
                if (VP.Game.OpponentLastLocTimeStamp < loc.timeStamp) {
                    VP.Game.OpponentLastLocTimeStamp = loc.timeStamp;
                    VP.Game.OpponentBat().style.top = loc.position + "px";
                    break;
                }
            }
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

    MyLocations: [],

    SendServerBatLocation: function () {
        var topVal = parseInt(VP.Game.MyBat().style.top, 10);
        VP.Game.MyLocations.push(topVal);
        if (VP.Game.MyLocations.length == 5) {
            VP.Shared.Connection.invoke("SendLocation", VP.Lobby.LobbyName, VP.Game.Player, VP.Game.MyLocations).catch(function (err) {
                return alert(err.toString());
            });
            VP.Game.MyLocations = [];
        }
    },

    ClientLoop: function () {
        VP.Game.CheckPressedKey();
        VP.Game.MoveBat();
        VP.Game.SendServerBatLocation();
        VP.Game.MoveOpponent();
    },

    Init: function () {
        VP.Game.SetupListeners();
        VP.Game.SetupControls();
        VP.Game.UpdateNamesAndScoresFromState();
        setInterval(VP.Game.ClientLoop, 10);
    }
};