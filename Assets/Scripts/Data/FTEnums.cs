using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FootTactic
{
    public class FTEnums
    {
        public enum FTAnimationType {
            Idle = 0,
            Pass = 1,
            Kick = 2,
            KeepBall = 3
        };
    }

    public class FTNofication
    {
        private FTNofication(string value) { Value = value; }

        public string Value { get; set; }

        public static FTNofication PlayAnim { get { return new FTNofication("PlayAnim"); } }
        public static FTNofication StopAnim { get { return new FTNofication("StopAnim"); } }
        public static FTNofication PauseAnim { get { return new FTNofication("PauseAnim"); } }
        public static FTNofication NextFrame { get { return new FTNofication("NextFrame"); } }
        public static FTNofication PreviouFrame { get { return new FTNofication("PreviosFrame"); } }
    }

}
