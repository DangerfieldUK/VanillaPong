using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using VanillaPong.Hubs;

namespace VanillaPong.GameCode
{
    public class HubState
    {
        public List<string> Players { get; set; } = new List<string>();
        public List<Lobby> Lobbies { get; set; } = new List<Lobby>();

        internal void SetPlayer2Name(string playerName, string lobbyName)
        {
            var lobby = Lobbies.Where(l => l.Name == lobbyName).Single();
            lobby.State.Player2Name = playerName;
            lobby.State.ReadyToStart = true;
        }

        internal void SendLocation(string lobbyName, int playerNumber, int topVal)
        {
            var lobby = Lobbies.Where(l => l.Name == lobbyName).Single();
            var timeStamp = DateTime.Now;
            var playerLocation = new PlayerLocation()
            {
                Position = topVal,
                TimeStamp = timeStamp.Ticks
            };
            switch (playerNumber)
            {
                case 1:
                    lobby.State.Player1Locations.Add(playerLocation);
                    break;

                case 2:
                    lobby.State.Player2Locations.Add(playerLocation);
                    break;
            }
        }
    }

    public class Lobby
    {
        public string Name { get; set; }
        public GameState State { get; set; }
    }
}
