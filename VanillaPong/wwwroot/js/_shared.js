var VP = extend(true, {}, VP);

VP.Shared = {
    Connection: {},

    EstablishServerConnection: function () {
        VP.Shared.Connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
        VP.Shared.Connection.start().then(function () {
            document.getElementById("connecting").style.display = "none";
            document.getElementsByClassName("lobbies")[0].style.display = "block";
        }).catch(function (err) {
            return alert(err.toString());
        });
    }
};