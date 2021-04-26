using System.Collections.Generic;

namespace VanillaPong.Models
{
    public class HubState
    {
        public List<string> Players { get; set; } = new List<string>();
        public List<Lobby> Lobbies { get; set; } = new List<Lobby>();
    }
}
