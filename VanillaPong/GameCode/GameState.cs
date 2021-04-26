﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VanillaPong.GameCode
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
        
        public void MovePlayer(int player, string direction)
        {
            switch (direction)
            {
                case "UP":
                    if (player == 1)
                        Player1PosTop--;
                    if (player == 2)
                        Player2PosTop--;
                    break;

                case "DOWN":
                    if (player == 1)
                        Player1PosTop++;
                    if (player == 2)
                        Player2PosTop++;
                    break;
            }
        }

        internal void Update()
        {
            if (BallLeft < 0)
                ballDirection = 4;
            if (BallLeft > 1000)
                ballDirection = -4;
            BallLeft = BallLeft + ballDirection;

            if (BallTop < 0)
                ballVDirection = 4;
            if (BallTop > 625)
                ballVDirection = -4;
            BallTop = BallTop + ballVDirection;
        }

        private int ballDirection { get; set; } = -4;
        private int ballVDirection { get; set; } = -4;
    }
}
