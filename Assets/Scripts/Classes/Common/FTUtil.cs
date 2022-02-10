using System;
using UnityEngine;
namespace FootTactic
{
    public static class FTUtil
    {
        public static string TransformButtonNameToModel(string buttonName)
        {
            return "Player"+buttonName.Replace("FTPlayerBtn", "") + "Model";
        }

        public static FTAnimPlayerView GetPlayerByIndex(int PlayerIndex)
        {
            return GameObject.Find("Player" + PlayerIndex + "Model") != null? GameObject.Find("Player"+PlayerIndex+ "Model").GetComponent<FTAnimPlayerView>(): null;
        }

        public static GameObject GetPlayerObjByIndex(int PlayerIndex)
        {
            return GameObject.Find("Player" + PlayerIndex + "Model");
        }

        public static int GetPlayerTeamIndex(int PlayerIndex)
        {
            if(PlayerIndex >= FTConstants.TEAM2_START_INDEX)
            {
                return PlayerIndex - FTConstants.TEAM2_START_INDEX;
            }
            else
            {
                return PlayerIndex - FTConstants.TEAM1_START_INDEX;
            }
        }

        public static int GetTeamIndex(int PlayerIndex)
        {
            if (PlayerIndex >= FTConstants.TEAM2_START_INDEX)
            {
                return 1;
            }
            if(PlayerIndex >= FTConstants.TEAM1_START_INDEX)
            {
                return 0;
            }
            
            return -1;
            
        }

        public static FTButtonPlayerView GetPlayerButtonByIndex(int PlayerIndex)
        {
            return GameObject.Find("FTPlayerBtn" + PlayerIndex) != null ? GameObject.Find("FTPlayerBtn" + PlayerIndex).GetComponent<FTButtonPlayerView>() : null;
        }

        public static string TransformName(string buttonName)
        {
            return buttonName.Replace("FTPlayerBtn", "") + "Model";
        }

        public static string TransformButtonName(string buttonName)
        {
           return buttonName.Replace("FTPlayerBtn", "P");
        }

        public static Vector3 SafeBallPosition(Vector3 originalPosition)
        {
            return new Vector3(originalPosition.x, originalPosition.y < 0.175f ? 0.175f : originalPosition.y, originalPosition.z);
        }

        public static Vector3 ConvertPosition(Vector3 originalPosition)
        {
            return  new Vector3(originalPosition.x, 0, originalPosition.z);
        }

        public static void DestroyGameObjectsWithTag(string tag)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject go in objects)
                GameObject.Destroy(go);
        }

        public static bool IsPlayerOfThisTeam(int player, int team)
        {
            return (player >= team*100 && player < (team*100 + 100));
        }
    }


}
