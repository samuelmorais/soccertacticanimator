using System;
using UnityEngine;

namespace FootTactic
{
    public interface IFTPlayer
    {
        string Name { get; }
        int Number { get; }
        int HairStyle { get; }
        int SkinColor { get; }
        int HairColor { get; }
        int FaceStyle { get; }
        int PlayerIndex { get; }
        bool isGoalKeeper { get; }
        int Tatoo { get; }
        int Beard { get; }
        GameObject Player { get; }

        void UpdateStyleAttributes(
            int hairStyle,
            int skinColor,
            int hairColor,
            int faceStyle,
            int tatoo,
            int beard);
        

        }

    public class FTPlayer : IFTPlayer
    {
        public string Name { get; private set; }
        public int Number { get; private set; }
        public int HairStyle { get; private set; }
        public int SkinColor { get; private set; }
        public int HairColor { get; private set; }
        public int Tatoo { get; private set; }
        public int Beard { get; private set; }
        public int FaceStyle { get; private set; }
        public int PlayerIndex { get; private set; }
        public bool isGoalKeeper { get { return PlayerIndex == 101 || PlayerIndex == 201; } }
        public GameObject Player { get; private set; }

        public FTPlayer(
            string name,
            int number,
            int hairStyle,
            int skinColor,
            int hairColor,
            int faceStyle,
            int tatoo,
            int beard,
            int playerIndex,
            GameObject player
        )
        {
            Name = name;
            Number = number;
            HairStyle = hairStyle;
            SkinColor = skinColor;
            HairColor = hairColor;
            FaceStyle = faceStyle;
            Tatoo = tatoo;
            Beard = beard;
            PlayerIndex = playerIndex;
            Player = player;
        }

        public FTPlayer(
            string name,
            int number,
            int hairStyle,
            int skinColor,
            int hairColor,
            int faceStyle,
            int tatoo,
            int beard
        )
        {
            Name = name;
            Number = number;
            HairStyle = hairStyle;
            SkinColor = skinColor;
            HairColor = hairColor;
            FaceStyle = faceStyle;
            Tatoo = tatoo;
            Beard = beard;
        }

        public void UpdateStyleAttributes(           
            int hairStyle,
            int skinColor,
            int hairColor,
            int faceStyle,
            int tatoo,
            int beard)
        {
            
            HairStyle = hairStyle;
            SkinColor = skinColor;
            HairColor = hairColor;
            FaceStyle = faceStyle;
            Tatoo = tatoo;
            Beard = beard;
            
        }
    }
}

