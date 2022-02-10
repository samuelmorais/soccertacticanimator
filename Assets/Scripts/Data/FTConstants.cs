using System;
using UnityEngine;
namespace FootTactic
{
    public static class FTConstants
    {
        public const float ANIM_SIZE = 15f;
        public const int ANIM_IDLE_INDEX = 1;
        public const int ANIM_PASS_INDEX = 1;
        public const int ANIM_KICK_INDEX = 2;
        public const int ANIM_KEEP_BALL_INDEX = 3;
        public const float TIME_PASS = 1f;
        public const float TIME_PASS_RUN = 1f;
        public const float TIME_KICK = 1f;
        public const float BALL_DISTANCE_TO_ACTIVATE_ACTIONS = 3f;
        public const float MIN_MOVEMENT_PLAYER = 0.1f;
        public const float MAX_SPEED_ALLOWED = 10f; // meters/second
        public static Vector3 RIGHT_FOOT_GROUND_OFFSET = new Vector3(0.15f, 0.15f, 0.4f);
        public const float UNSET_ROTATION = -1000000f;
        public const string UNSET_ROTATION_STR = "-1000000";
        public const int NUM_PLAYERS_PER_TEAM = 11;
        public const int NUM_ANIMS = 10;
        public const int TEAM1_START_INDEX = 101;
        public const int TEAM2_START_INDEX = 201;
    }
}

