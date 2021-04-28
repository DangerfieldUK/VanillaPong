using System.Collections.Generic;
using System.Linq;

namespace VanillaPong.Models
{
    public class GameState
    {
        public int Player1PosTop { get; set; } = 200;
        public int Player2PosTop { get; set; } = 100;
        public int BallLeft { get; set; } = 400;
        public int BallTop { get; set; } = 300;
        public int Player1Score { get; set; } = 0;
        public int Player2Score { get; set; } = 0;
        public string Player1Name { get; set; } = "Player 1";
        public string Player2Name { get; set; } = "Awaiting player...";
        public bool ReadyToStart { get; set; } = false;
        public bool InPlay { get; set; } = false;
        public bool Player1Scores { get; set; } = false;
        public bool Player2Scores { get; set; } = false;

        public List<Location> Player1Locations { get; set; } = new List<Location>();

        public List<Location> Player2Locations { get; set; } = new List<Location>();
        public List<Location> BallLocations { get; set; } = new List<Location>();
        public int LastPlayer1Position { get; internal set; }
        public int LastPlayer2Position { get; internal set; }
        public Location BallLastPosition { get; internal set; }
        internal int ballDirection { get; set; } = 4;
        internal int ballDirectionX { get; set; } = 4;
    }
}
