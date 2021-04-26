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

        internal void SendLocation(string lobbyName, int playerNumber, int[] topVals)
        {
            var lobby = Lobbies.Where(l => l.Name == lobbyName).Single();
            var timeStamp = DateTime.Now.Subtract(TimeSpan.FromMilliseconds(topVals.Length * 10));
            var playerLocations = new List<PlayerLocation>();
            foreach (var loc in topVals)
            {
                playerLocations.Add(new PlayerLocation()
                {
                    Position = loc,
                    TimeStamp = timeStamp.Ticks
                });
                timeStamp = timeStamp.AddMilliseconds(10);
            }
            switch (playerNumber)
            {
                case 1:
                    lobby.State.Player1Locations.AddRange(playerLocations);
                    break;

                case 2:
                    lobby.State.Player2Locations.AddRange(playerLocations);
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
