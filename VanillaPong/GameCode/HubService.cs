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


        internal void ClearSentLocations(string lobbyName)
        {
            var lobby = GetLobby(lobbyName);
            if (lobby.State.Player1Locations.Any())
            {
                var lastPosition = lobby.State.Player1Locations.OrderByDescending(x => x.TimeStamp).First();
                lobby.State.LastPlayer1Position = lastPosition.Position;
            }
            lobby.State.Player1Locations = new List<Location>();
            if (lobby.State.Player2Locations.Any())
            {
                var lastPosition = lobby.State.Player2Locations.OrderByDescending(x => x.TimeStamp).First();
                lobby.State.LastPlayer2Position = lastPosition.Position;
            }
            lobby.State.Player2Locations = new List<Location>();
            lobby.State.BallLastPosition = lobby.State.BallLocations.Any() ? lobby.State.BallLocations.OrderByDescending(x => x.TimeStamp).First() : null;
            lobby.State.BallLocations = new List<Location>();
        }

        internal void RemoveDeadLobbies()
        {
            _hubState.Lobbies = _hubState.Lobbies.Where(x => x.RemoveMe == false).ToList();
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

        internal void SimulateGame(string name)
        {
            var lobby = GetLobby(name);
            var state = lobby.State;
            if (state.InPlay)
            {
                var timeStamp = DateTime.Now;
                var past100Ms = timeStamp.Subtract(TimeSpan.FromMilliseconds(100));
                var rand = new Random();
                for (var i = 0; i < 10; i++)
                {
                    if (!state.InPlay)
                        break;
                    if (!state.BallLocations.Any() && state.BallLastPosition == null)
                    {
                        state.BallLocations.Add(new Location()
                        {
                            Position = 10 + rand.Next(605),
                            PositionX = 20 + rand.Next(980),
                            TimeStamp = past100Ms.AddMilliseconds(i * 10).Ticks
                        });
                        if (state.BallLocations.Single().PositionX < 500)
                            state.ballDirectionX = 4;
                        else
                            state.ballDirectionX = -4;
                        continue;
                    }

                    var lastloc = !state.BallLocations.Any()
                        ? state.BallLastPosition :
                        state.BallLocations.OrderByDescending(x => x.TimeStamp).First();

                    // move ball
                    var newLoc = lastloc.ShallowCopy();
                    newLoc.TimeStamp = past100Ms.AddMilliseconds(i * 10).Ticks;
                    // vertical
                    if (newLoc.Position <= 0)
                        state.ballDirection = 4;
                    if (newLoc.Position >= 605)
                        state.ballDirection = -4;
                    newLoc.Position = newLoc.Position + state.ballDirection;
                    // horizontal
                    if (newLoc.PositionX < 0)
                        state.ballDirectionX = 4;
                    if (newLoc.PositionX >= 980)
                        state.ballDirectionX = -4;
                    newLoc.PositionX = newLoc.PositionX + state.ballDirectionX;
                    // add to history
                    state.BallLocations.Add(newLoc);



                    if (newLoc.PositionX <= 4)
                    {
                        var pos = state.LastPlayer1Position;
                        if (state.Player1Locations.Any())
                        {
                            pos = state.Player1Locations.Count() > i ? state.Player1Locations.OrderBy(x => x.TimeStamp).Skip(i).First().Position : state.Player1Locations.OrderBy(x => x.TimeStamp).Last().Position;
                        }
                        if (newLoc.Position < pos - 19 || newLoc.Position > pos + 199)
                        {
                            state.Player2Scores = true;
                            state.Player2Score++;
                            state.InPlay = false;
                        }
                    }
                    if (newLoc.PositionX >= 976)
                    {
                        var pos = state.LastPlayer2Position;
                        if (state.Player2Locations.Any())
                        {
                            pos = state.Player2Locations.Count() > i ? state.Player2Locations.OrderBy(x => x.TimeStamp).Skip(i).First().Position : state.Player2Locations.OrderBy(x => x.TimeStamp).Last().Position;
                        }
                        if (newLoc.Position < pos - 19 || newLoc.Position > pos + 199)
                        {
                            state.Player1Scores = true;
                            state.Player1Score++;
                            state.InPlay = false;
                        }
                    }
                }
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
            lobby.LastUpdate = DateTime.Now;
            var timeStamp = DateTime.Now.Subtract(TimeSpan.FromMilliseconds(locations.Length * 10));
            var playerLocations = new List<Location>();
            foreach (var loc in locations)
            {
                playerLocations.Add(new Location()
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

        internal void StartPlay(string lobbyName, int playerNumber)
        {
            if (GetLobby(lobbyName).State.InPlay == false && GetLobby(lobbyName).State.ReadyToStart == true)
            {
                GetLobby(lobbyName).State.InPlay = true;
            }
        }
    }
}