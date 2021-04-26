var VP = extend(true, {}, VP);

VP.Lobby = {
    PlayerName: '',
    LobbyName: '',

    Element_LobbyContainer: document.getElementsByClassName("lobbies")[0],
    Element_NameSubmit: document.getElementById("nameSubmit"),
    Element_LobbiesPlayer: document.getElementsByClassName("lobbies-player")[0],
    Element_LobbiesPanel: document.getElementsByClassName("lobbies-panel")[0],
    Element_NameDisplay: document.getElementById("name-display"),
    Element_LobbiesList: document.getElementsByClassName("lobbies-list")[0],
    Element_CreateLobbyButton: document.getElementById("newLobbyCreate"),

    SetupEvents: function () {
        // Confirm Name Button
        VP.Lobby.Element_NameSubmit.onclick = (function () {
            if (VP.Lobby.PlayerName.length < 3)
                return alert("Name must be three characters or more.");
            VP.Shared.Connection.invoke("CheckName", VP.Lobby.PlayerName).catch(function (err) {
                return alert(err.toString());
            });
        });

        // Create Lobby Button
        VP.Lobby.Element_CreateLobbyButton.onclick = (function () {
            if (VP.Lobby.LobbyName.length < 3)
                return alert("Name must be three characters or more.");

            VP.Shared.Connection.invoke("CheckLobbyName", VP.Lobby.LobbyName, VP.Lobby.PlayerName).catch(function (err) {
                return alert(err.toString());
            });
        });
    },

    SetupListeners: function () {
        // Server sent New Player Name response
        VP.Shared.Connection.on("CheckName", function (ok) {
            if (ok === true) {
                VP.Lobby.Element_NameSubmit.style.display = "none";
                VP.Lobby.Element_LobbiesPlayer.style.display = "none";
                VP.Lobby.Element_LobbiesPanel.style.display = "block";
                VP.Lobby.Element_NameDisplay.innerText = VP.Lobby.PlayerName;
            } else {
                alert("Sorry, that name is taken.. please try another.")
            }
        });

        // Server sent Lobby Update
        VP.Shared.Connection.on("LobbyUpdate", function (lobbies) {
            VP.Lobby.Element_LobbiesList.innerHTML = "";
            lobbies.forEach(function (lobby) {
                VP.Lobby.LobbyName = lobby.name;
                VP.Lobby.Element_LobbiesList.innerHTML +=
                    "<li>"
                    + lobby.name
                    + "<button id=\"join" + lobby.name + "\" " 
                    + "onclick=\"VP.Lobby.JoinLobby('" + lobby.name + "')\">Join</button></li>";
            })
        });

        // Server sent Create Lobby response (Player 1)
        VP.Shared.Connection.on("CheckLobbyName", function (ok, state) {
            if (ok === true) {
                VP.Game.State = state;
                VP.Lobby.Element_LobbyContainer.style.display = "none";
                VP.Game.Element_GameContainer.style.display = "block";
                VP.Game.Element_Scores.style.display = "block";
                VP.Game.Init();
            } else {
                alert("Sorry, that lobby already exists.. please try another name.")
            }
        });

        // Server sent Join Existing Lobby response (Player 2)
        VP.Shared.Connection.on("JoinExistingLobby", function (state) {
            VP.Game.State = state;
            VP.Lobby.Element_LobbyContainer.style.display = "none";
            VP.Game.Element_GameContainer.style.display = "block";
            VP.Game.Element_Scores.style.display = "block";
            VP.Game.Player = 2;
            VP.Game.Init();
        });
    },

    JoinLobby: function (lobbyName) {
        VP.Shared.Connection.invoke("JoinExistingLobby", lobbyName, VP.Lobby.PlayerName).catch(function (err) {
            return alert(err.toString());
        });
    },

    Init: function () {
        VP.Lobby.SetupEvents();

        VP.Lobby.SetupListeners();
    }
};