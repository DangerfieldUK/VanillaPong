var VP = extend(true, {}, VP);

VP.App = {
    Init: function () {
        console.log("application init.");

        VP.Shared.EstablishServerConnection();

        VP.Lobby.Init();
    }
};

VP.App.Init();