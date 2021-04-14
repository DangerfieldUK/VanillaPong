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
    }

    public class Lobby
    {
        public string Name { get; set; }
        public GameState State { get; set; }
    }
}
