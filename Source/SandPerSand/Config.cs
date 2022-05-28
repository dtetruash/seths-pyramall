﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
namespace SandPerSand
{
    public class Conf
    {
        public static class Depth
        {
            public static float Player = 0.3f;
            public static float PlayerFront;
            public static float PlayerBack = 0.31f;

            // tiled depth (not in use)
            public static Dictionary<string, float> Tiled =
                new Dictionary<string, float> {
                    {"Background",0.8f},
                    {"Platform",0.7f},
                    {"Marker",0.7f},
                    //Player 0.3f
                    {"Foreground",0.2f},
                    {"Detail",1f},
                };
        };

        //(not in use)
        public static class GoLayer
        {
            public static int Player = 1;
            public static int Item = 2;
            public static int Other = 3;
        }

        public static class Time
        {
            public static float ShopTime = 10f;
        }


        public static Vector2 HardJumpVelocity = new Vector2(27, 16);
        public static float HardJumpDistance = 3f;

        public static Vector2 SandDetectVec = new Vector2(0.2f, 0);
        // sandDetector = (+/-) SandDetectVec
        // detectPosition = Transform.Position + sandDetector;
        // deleteSandPosition = Transform.Position - sandDetector;
        public static float SandDetectRemoveRadius = 0.1f;
    }
}
