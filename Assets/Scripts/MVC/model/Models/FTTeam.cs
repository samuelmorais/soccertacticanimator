using System;
using System.Collections.Generic;

namespace FootTactic
{
    public interface IFTTeam
    {
        string Name { get; }        
        int Kit { get; }
        string Color1 { get; }
        string Color2 { get; }
        string Color3 { get; }
        string GKColor1 { get; }
        string GKColor2 { get; }
        string GKColor3 { get; }
        List<IFTPlayer> Players { get; }
        void SetName(string name);
        void SaveTeam(IFTTeam team);
    }

    public class FTTeam : IFTTeam
    {
        public string Name { get; private set; }        
        public int Kit { get; private set; }
        public string Color1 { get; private set; }
        public string Color2 { get; private set; }
        public string Color3 { get; private set; }
        public string GKColor1 { get; private set; }
        public string GKColor2 { get; private set; }
        public string GKColor3 { get; private set; }
        public List<IFTPlayer> Players { get; private set;  }

        public FTTeam(string name, int kit, string color1, string color2, string color3, string color1gk, string color2gk, string color3gk,  List<IFTPlayer> players)
        {
            Name = name;
            Kit = kit;
            Color1 = color1;
            Color2 = color2;
            Color3 = color3;
            GKColor1 = color1gk;
            GKColor2 = color2gk;
            GKColor3 = color3gk;
            Players = players;
        }

        public FTTeam(string name, int kit, string color1, string color2, string color3, string color1gk, string color2gk, string color3gk)
        {
            Name = name;
            Kit = kit;
            Color1 = color1;
            Color2 = color2;
            Color3 = color3;
            GKColor1 = color1gk;
            GKColor2 = color2gk;
            GKColor3 = color3gk;            
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SaveTeam(IFTTeam team)
        {
            Name = team.Name;
            Kit = team.Kit;
            Color1 = "#" + team.Color1;
            Color2 = "#" + team.Color2;
            Color3 = "#"+ team.Color3;
            GKColor1 = "#" + team.GKColor1;
            GKColor2 = "#" + team.GKColor2;
            GKColor3 = "#" + team.GKColor3;
        }
    }
}

