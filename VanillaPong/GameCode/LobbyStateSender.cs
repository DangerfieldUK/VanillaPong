using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VanillaPong.Hubs;

namespace VanillaPong.GameCode
{
    public class LobbyStateSender
    {
        private IHubContext<GameHub> _gameHubContext;
        private readonly HubState _hubState;
        Timer _tm;

        public LobbyStateSender(IHubContext<GameHub> gameHubContext, HubState hubState)
        {
            _gameHubContext = gameHubContext;
            _hubState = hubState;

            _tm = new Timer(
                UpdateLobbies,
                null,
                40,
                40);
        }

        protected void UpdateLobbies(object state)
        {
            foreach (var lobby in _hubState.Lobbies)
            {
                if (lobby.Name != null)
                {
                    lobby.State.Update();
                    _gameHubContext.Clients.Groups(lobby.Name).SendAsync("GameState", lobby.State);
                }
            }
        }

       
    }
}
