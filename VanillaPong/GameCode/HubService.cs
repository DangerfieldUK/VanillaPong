using System;
using System.Collections.Generic;
using System.Linq;
using VanillaPong.Models;

namespace VanillaPong.GameCode
{
    public class HubService
    {
        private readonly HubState _hubState;

        public HubService(HubState hubState)
        {
            _hubState = hubState;
        }


        internal void MarkLocationsSent(string lobbyName)
        {
            var lobby = GetLobby(lobbyName);
            lobby.State.Player1Locations = new List<PlayerLocation>();
            lobby.State.Player2Locations = new List<PlayerLocation>();
        }

        internal List<Lobby> GetLobbies()
        {
            return _hubState.Lobbies;
        }

        public HubState GetHubState()
        {
            return _hubState;
        }

        internal void AddPlayer(string name)
        {
            _hubState.Players.Add(name);
        }

        internal void AddLobby(Lobby lobby)
        {
            _hubState.Lobbies.Add(lobby);
        }

        internal void UpdateLobbies()
        {
            foreach (var lobby in _hubState.Lobbies)
            {
                lobby.State.Update();
            }
        }

        internal void SetPlayer2Name(string playerName, string lobbyName)
        {
            GetLobby(lobbyName).State.Player2Name = playerName;
            GetLobby(lobbyName).State.ReadyToStart = true;
        }

        internal Lobby GetLobby(string lobbyName)
        {
            return _hubState.Lobbies.Where(l => l.Name == lobbyName).Single();
        }

        internal void SaveLocations(string lobbyName, int playerNumber, int[] locations)
        {
            var lobby = GetLobby(lobbyName);
            var timeStamp = DateTime.Now.Subtract(TimeSpan.FromMilliseconds(locations.Length * 10));
            var playerLocations = new List<PlayerLocation>();
            foreach (var loc in locations)
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
} 