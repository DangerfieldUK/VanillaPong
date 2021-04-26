using System.Collections.Generic;
using System.Linq;

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

        internal void SendLocation(string lobbyName, int playerNumber, int[] topVal)
        {
            _hubState.SendLocation(lobbyName, playerNumber, topVal);
        }
    }
}