﻿//
// GameManager.cs
//
// Author:
//       Yauheni Pakala <evgeniy.pakalo@gmail.com>
//
// Copyright (c) 2018 Yauheni Pakala
//

using System;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Snake
{
    public class GameManager
    {
        readonly GameScene scene;

        double nextTime = 0;
        double timeExtension = 0.15;

        int playerDirection = 4;

        Random random = new Random();

        public GameManager(GameScene scene)
        {
            this.scene = scene;
        }

        public void InitGame()
        {
            scene.playerPositions.Add(new Pos(10, 10));
            scene.playerPositions.Add(new Pos(10, 11));
            scene.playerPositions.Add(new Pos(10, 12));

            RenderChange();

            GenerateNewPoint();
        }

        void RenderChange()
        {
            foreach (var cell in scene.gameArray)
            {
                if (scene.playerPositions.Any(x => x.X == cell.X && x.Y == cell.Y))
                {
                    cell.Node.FillColor = UIColor.Cyan;
                }
                else
                {
                    cell.Node.FillColor = UIColor.Clear;

                    if (scene.scorePos != null) {
                        if (Math.Floor(scene.scorePos.X) == cell.Y &&
                            Math.Floor(scene.scorePos.Y) == cell.X)
                        {
                            cell.Node.FillColor = UIColor.Red;
                        }
                    }
                }
            }
        }

        void GenerateNewPoint()
        {
            var randomX = random.NextDouble() * 19; // numCols - 1
            var randomY = random.NextDouble() * 39; // numRows - 1
            scene.scorePos = new CGPoint(x: randomX, y: randomY);
        }

        public void Update(double time)
        {
            if (Math.Abs(nextTime) < 0)
            {
                nextTime = time + timeExtension;
            }
            else
            {
                if (time >= nextTime)
                {
                    nextTime = time + timeExtension;

                    UpdatePlayerPosition();
                }
            }
        }

        void UpdatePlayerPosition()
        {
            var xChange = -1;
            var yChange = 0;

            switch (playerDirection) 
            {
                case 1:
                    //left
                    xChange = -1;
                    yChange = 0;
                    break;
                case 2:
                    //up
                    xChange = 0;
                    yChange = -1;
                    break;
                case 3:
                    //right
                    xChange = 1;
                    yChange = 0;
                    break;
                case 4:
                    //down
                    xChange = 0;
                    yChange = 1;
                    break;
                default:
                    break;
            }

            if (scene.playerPositions.Count > 0)
            {
                var start = scene.playerPositions.Count - 1;
                while (start > 0)
                {
                    scene.playerPositions[start] = scene.playerPositions[start - 1];
                    start -= 1;
                }
                scene.playerPositions[0] = new Pos(scene.playerPositions[0].X + yChange, scene.playerPositions[0].Y + xChange);
            }

            if (scene.playerPositions.Count > 0) {
                
                var pos = scene.playerPositions[0];

                var x = pos.Y;
                var y = pos.X;

                if (y > 40) {
                    pos.X = 0;
                }
                else if (y < 0) {
                    pos.X = 40;
                }
                else if (x > 20) {
                    pos.Y = 0;
                }
                else if (x < 0) {
                    pos.Y = 20;
                }
            }

            RenderChange();
        }

        public void Swipe(int id)
        {
            if (!(id == 2 && playerDirection == 4) &&
                !(id == 4 && playerDirection == 2))
            {
                if (!(id == 1 && playerDirection == 3) &&
                    !(id == 3 && playerDirection == 1))
                {
                    playerDirection = id;
                }
            }
        }
    }
}
