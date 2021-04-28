using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VanillaPong.Hubs;
using VanillaPong.Models;

namespace VanillaPong.GameCode
{
    public class LobbyStateSender
    {
        private HubService _hubService;
        private IHubContext<GameHub> _gameHubContext;
        Timer _tm;

        public LobbyStateSender(HubService hubService, IHubContext<GameHub> gameHubContext)
        {
            _gameHubContext = gameHubContext;
            _hubService = hubService;
            _tm = new Timer(
                UpdateLobbies,
                null,
                100,
                100);
        }

        protected void UpdateLobbies(object state)
        {
            SendStates(_hubService.GetLobbies());
            _hubService.RemoveDeadLobbies();
            _gameHubContext.Clients.Groups("lobby").SendAsync("LobbyUpdate", _hubService.GetHubState().Lobbies.Where(l => l.State.ReadyToStart == false));
        }
        internal void SendStates(List<Lobby> lobbies)
        {
            foreach (var lobby in lobbies)
            {
                if (lobby.LastUpdate < DateTime.Now.Subtract(TimeSpan.FromSeconds(60)))
                {
                    lobby.RemoveMe = true;
                }
                _hubService.SimulateGame(lobby.Name);
                _gameHubContext.Clients.Groups(lobby.Name).SendAsync("StateUpdate", lobby.State);
                _hubService.ClearSentLocations(lobby.Name);
            }
        }
    }
}
