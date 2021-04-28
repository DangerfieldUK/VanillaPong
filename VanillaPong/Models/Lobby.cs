using System;

namespace VanillaPong.Models
{
    public class Lobby
    {
        public string Name { get; set; }
        public GameState State { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.Now;
        public bool RemoveMe { get; internal set; }
    }
}
