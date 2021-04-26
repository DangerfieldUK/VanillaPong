using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
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
            _hubService.UpdateLobbies();
            SendStates(_hubService.GetLobbies());
        }
        internal void SendStates(List<Lobby> lobbies)
        {
            foreach (var lobby in lobbies)
            {
                _gameHubContext.Clients.Groups(lobby.Name).SendAsync("StateUpdate", lobby.State);
                _hubService.MarkLocationsSent(lobby.Name);
            }
        }
    }
}
