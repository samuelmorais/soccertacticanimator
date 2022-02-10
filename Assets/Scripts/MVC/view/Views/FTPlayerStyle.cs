using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using thelab.mvc;
using System;
using NG.Patterns.Structure.ObserverPattern;
using Animancer.Examples.StateMachines.Brains;
using Animancer.Examples.Locomotion;
using Animancer;
using System.Linq;

namespace FootTactic
{

    [RequireComponent(typeof(FTAnimPlayerView))]

    public class FTPlayerStyle : NotificationView<FTApplication>
    {
        FTAnimPlayerView PlayerView;
        [SerializeField]
        SkinnedMeshRenderer playerSkin;
        [SerializeField]
        MeshFilter hairMesh;
        [SerializeField]
        MeshRenderer hairRenderer;

        private void Start()
        {
            PlayerView = GetComponent<FTAnimPlayerView>();
            playerSkin = GetComponentInChildren<SkinnedMeshRenderer>();

        }

        public void SetName(string value)
        {
            if (FTController.PlayerControllers[PlayerView.PlayerIndex] != null && FTController.PlayerControllers[PlayerView.PlayerIndex].PlayerButton != null)
            {
                FTController.PlayerControllers[PlayerView.PlayerIndex].PlayerButton.SetPlayerName(value);
            }
        }

        public void SetNumber(int value)
        {
            if (FTController.PlayerControllers[PlayerView.PlayerIndex] != null && FTController.PlayerControllers[PlayerView.PlayerIndex].PlayerButton != null)
            {
                FTController.PlayerControllers[PlayerView.PlayerIndex].PlayerButton.SetPlayerNumber(value);
            }
            int leftNumberIndex = 0;
            int rightNumberIndex = 0;
            if (value >= 10 && value < 100)
            {
                var stringNumber = value.ToString();
                leftNumberIndex = int.Parse(stringNumber.Substring(0, 1));
                rightNumberIndex = int.Parse(stringNumber.Substring(1, 1));
                playerSkin.materials[2].SetTexture("_DecalTex", PlayersAssetsView.instance.centerNumbers[leftNumberIndex]);
                playerSkin.materials[4].SetTexture("_DecalTex", PlayersAssetsView.instance.centerNumbers[rightNumberIndex]);
            }
            else if (value < 10)
            {
                leftNumberIndex = value;
                rightNumberIndex = value;
                playerSkin.materials[2].SetTexture("_DecalTex", PlayersAssetsView.instance.leftNumbers[leftNumberIndex]);
                playerSkin.materials[4].SetTexture("_DecalTex", PlayersAssetsView.instance.rightNumbers[rightNumberIndex]);
            }
            else
            {
                Debug.LogError("Error: number greater then 100");
            }

            playerSkin.materials[2].SetTexture("_MainTex", PlayersAssetsView.instance.shirtTexture);
            playerSkin.materials[4].SetTexture("_MainTex", PlayersAssetsView.instance.shirtTexture);


        }



        public void SetTeamStyle(IFTTeam team, IFTPlayer player)
        {
            if (!player.isGoalKeeper)
            {
                Color color1;
                ColorUtility.TryParseHtmlString(team.Color1, out color1);
                Color color2;
                ColorUtility.TryParseHtmlString(team.Color2, out color2);
                Color color3;
                ColorUtility.TryParseHtmlString(team.Color3, out color3);
                playerSkin.materials[1].SetColor("_Color", color1);
                playerSkin.materials[3].SetColor("_Color", color1);
                playerSkin.materials[2].SetColor("_Color", color1);
                playerSkin.materials[4].SetColor("_Color", color1);
                playerSkin.materials[5].color = color2;
                playerSkin.materials[7].color = color3;
            }
            else
            {
                Color color1;
                ColorUtility.TryParseHtmlString(team.GKColor1, out color1);
                Color color2;
                ColorUtility.TryParseHtmlString(team.GKColor2, out color2);
                Color color3;
                ColorUtility.TryParseHtmlString(team.GKColor3, out color3);
                playerSkin.materials[1].SetColor("_Color", color1);
                playerSkin.materials[3].SetColor("_Color", color1);
                playerSkin.materials[2].SetColor("_Color", color1);
                playerSkin.materials[4].SetColor("_Color", color1);
                playerSkin.materials[5].color = color2;
                playerSkin.materials[7].color = color3;

            }
            //TODO: change decal material to allow any color. For now, needs to be dark because shader is not working with colored shirt.
            playerSkin.materials[2].SetColor("_DecalColor", Color.black);
            playerSkin.materials[4].SetColor("_DecalColor", Color.black);

        }

        bool IsGoakKeeper()
        {

            return PlayerView.PlayerIndex == 101 || PlayerView.PlayerIndex == 201;


        }

        int TeamIndex()
        {
            return PlayerView.PlayerIndex < 200 ? 1 : 2;
        }

        void ApplyTeamStyle(IFTTeam team, EditTeamView editTeam)
        {
            team.SaveTeam(new FTTeam(editTeam.TeamName.text,
                0,
                ColorUtility.ToHtmlStringRGB(editTeam.shirtColor.color),
                ColorUtility.ToHtmlStringRGB(editTeam.shortColor.color),
                ColorUtility.ToHtmlStringRGB(editTeam.socksColor.color),
                ColorUtility.ToHtmlStringRGB(editTeam.shirtColorGK.color),
                ColorUtility.ToHtmlStringRGB(editTeam.shortColorGK.color),
                ColorUtility.ToHtmlStringRGB(editTeam.socksColorGK.color)
                ));

        }

        void SaveTeamModel(EditTeamView editTeam)
        {
            if (TeamIndex() == 1)
            {
                ApplyTeamStyle(app.model.Tactic.Teams[0], editTeam);
            }
            else
            {
                ApplyTeamStyle(app.model.Tactic.Teams[1], editTeam);
            }
        }

        public void ApplyStyleOfTeam(EditTeamView editTeam)
        {

            if (IsGoakKeeper())
            {
                playerSkin.materials[1].SetColor("_Color", editTeam.shirtColorGK.color);
                playerSkin.materials[1].SetTexture("_MainTex", PlayersAssetsView.instance.shirtTexture);
                playerSkin.materials[3].SetColor("_Color", editTeam.shirtColorGK.color);
                playerSkin.materials[3].SetTexture("_MainTex", PlayersAssetsView.instance.shirtTexture);
                playerSkin.materials[5].color = editTeam.shortColorGK.color;
                playerSkin.materials[5].SetTexture("_MainTex", PlayersAssetsView.instance.shortTexture);
                playerSkin.materials[7].color = editTeam.socksColorGK.color;
                playerSkin.materials[7].SetTexture("_MainTex", PlayersAssetsView.instance.socksTexture);


            }
            else
            {
                playerSkin.materials[1].SetColor("_Color", editTeam.shirtColor.color);
                playerSkin.materials[1].SetTexture("_MainTex", PlayersAssetsView.instance.shirtTexture);
                playerSkin.materials[3].SetColor("_Color", editTeam.shirtColor.color);
                playerSkin.materials[3].SetTexture("_MainTex", PlayersAssetsView.instance.shirtTexture);
                playerSkin.materials[5].color = editTeam.shortColor.color;
                playerSkin.materials[5].SetTexture("_MainTex", PlayersAssetsView.instance.shortTexture);
                playerSkin.materials[7].color = editTeam.socksColor.color;
                playerSkin.materials[7].SetTexture("_MainTex", PlayersAssetsView.instance.socksTexture);

            }
        }

        public void SetTeamStyle(EditTeamView editTeam)
        {

            SaveTeamModel(editTeam);
            Notify("ApplyStyleOfTeam", editTeam, TeamIndex());

        }

        public void SetPlayerVisual(IFTTeam team, IFTPlayer player)
        {
            SetName(player.Name);
            SetNumber(player.Number);
            SetStyle(player.SkinColor, player.FaceStyle, player.Tatoo, player.Beard);
            SetHair(player.HairStyle, player.HairColor);
            if(team != null)
            {
                SetTeamStyle(team, player);
            }
        }

        public void ApplyStyle(EditPlayerView editPlayer)
        {
            SetStyle(editPlayer.Skin.value, editPlayer.Face.value, editPlayer.Tatoo.value, editPlayer.Beard.value);
            SetHair(editPlayer.Hair.value, editPlayer.HairColor.value);
            try
            {
                SetNumber(int.Parse(editPlayer.FieldPlayerNumber.text));
                SetName(editPlayer.FieldPlayerName.text);
            }
            catch (Exception ex)
            {
                Debug.Log("Invalid number passed to player number field: " + ex.Message);
            }

            
        }

        public void SetStyle(int skin, int face, int tatoo, int beard)
        {
            Texture2D skinPlayer = PlayersAssetsView.instance.GetPlayerSkin(tatoo > 0, beard > 0, skin, face);
            playerSkin.materials[0].SetTexture("_MainTex", skinPlayer);
        }

        public void SetHair(int hair, int hairColor = -1)
        {
            Transform tHair = transform.Find("Root/Hips/Spine/Spine1/Neck/Head");
            if (tHair.childCount > 0)
            {
                Destroy(tHair.GetChild(0).gameObject);
            }
            var goHair = PlayersAssetsView.instance.CreateHair(hair, hairColor);
            goHair.transform.parent = tHair;
            goHair.transform.localPosition = Vector3.zero;
            goHair.transform.localRotation = Quaternion.identity;            

            InitializeHairs(tHair);
        }

        public void SetHairColor(int hairColor)
        {
            Transform tHair = transform.Find("Root/Hips/Spine/Spine1/Neck/Head");
            if(tHair.childCount > 0)
            {
                tHair.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", PlayersAssetsView.instance.HairColors[hairColor]);
                InitializeHairs(tHair);
            }            
            
        }

        void InitializeHairs(Transform tHair)
        {
            hairMesh = tHair.GetComponentInChildren<MeshFilter>();
            hairRenderer = tHair.GetComponentInChildren<MeshRenderer>();
        }

        public void ApplyTeamStyle(EditTeamView editTeam)
        {

        }

    }
}
