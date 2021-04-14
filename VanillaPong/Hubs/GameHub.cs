using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VanillaPong.GameCode;

namespace VanillaPong.Hubs
{
    public class GameHub : Hub
    {
        private readonly HubState _hubState;
        private readonly LobbyStateSender _lobbyStateSender;

        public GameHub(HubState hubState, LobbyStateSender lobbyStateSender)
        {
            _hubState = hubState;
            _lobbyStateSender = lobbyStateSender;
        }

        public async Task CheckName(string name)
        {
            if (_hubState.Players.Contains(name))
            {
                await Clients.Caller.SendAsync("CheckName", false);
            }
            else
            {
                _hubState.Players.Add(name);
                await Groups.AddToGroupAsync(Context.ConnectionId, "lobby");
                await Clients.Caller.SendAsync("LobbyUpdate", _hubState.Lobbies);
                await Clients.Caller.SendAsync("CheckName", true);
            }
        }

        public async Task CheckLobbyName(string lobbyName, string playerName)
        {
            if (_hubState.Lobbies.Any(l => l.Name == lobbyName) && lobbyName != "lobby")
            {
                await Clients.Caller.SendAsync("CheckLobbyName", false);
            }
            else
            {
                _hubState.Lobbies.Add(new Lobby()
                {
                    Name = lobbyName,
                    State = new GameState() { Player1Name = playerName }
                });
                await Groups.AddToGroupAsync(Context.ConnectionId, lobbyName);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, "lobby");
                await Clients.Groups("lobby").SendAsync("LobbyUpdate", _hubState.Lobbies);
                await Clients.Caller.SendAsync("CheckLobbyName", true);
            }
        }

        //public async Task SendKey(int player, string key, string playerName)
        //{
        //    if (player == 1)
        //        _state.Player1Name = playerName;
        //    if (player == 2)
        //        _state.Player2Name = playerName;

        //    switch (key)
        //    {
        //        case "UP":
        //            _state.MovePlayer(player, key);
        //            break;

        //        case "DOWN":
        //            _state.MovePlayer(player, key);
        //            break;

        //        default:
        //            _state.Update();
        //            break;
        //    }

        //    await Clients.All.SendAsync("GameState", _state, player);
        //}
    }
}
