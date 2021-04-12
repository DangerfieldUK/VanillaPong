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
        private readonly GameState _state;

        public GameHub(GameState state)
        {
            _state = state;
        }

        public async Task SendKey(int player, string key, string playerName)
        {
            if (player == 1)
                _state.Player1Name = playerName;
            if (player == 2)
                _state.Player2Name = playerName;

            switch (key)
            {
                case "UP":
                    _state.MovePlayer(player, key);
                    break;

                case "DOWN":
                    _state.MovePlayer(player, key);
                    break;

                default:

                    break;
            }

            await Clients.All.SendAsync("GameState", _state, player);
        }
    }
}
