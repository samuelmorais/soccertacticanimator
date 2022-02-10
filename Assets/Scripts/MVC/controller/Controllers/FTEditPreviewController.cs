using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;
namespace FootTactic
{

    public class FTEditPreviewController : Controller<FTApplication>
    {
        public FTPlayerPreview editPlayerView;
        public EditPlayerView editPlayer;
        public override void OnNotification(string p_event, UnityEngine.Object p_target, params object[] p_data)
        {
            InitializeEditPlayerView();
            if (editPlayer != null && !editPlayer.JustInitialized)
            {
                editPlayerView.CameraFocusOnFace();
                switch (p_event)
                {
                    case "EditPlayerHair@change":
                        editPlayerView.playerStyle.SetHair(((FTDropDownView)p_data[0]).dropdown.value, editPlayer.HairColor.value);
                        break;
                    case "EditPlayerHairColor@change":
                        editPlayerView.playerStyle.SetHairColor(((FTDropDownView)p_data[0]).dropdown.value);
                        break;
                    case "EditPlayerSkin@change":
                    case "EditPlayerFace@change":
                    case "EditPlayerBeard@change":
                    case "EditPlayerTatoo@change":
                        ApplyChanges();
                        break;
                    case "UpdatePlayerPreview":
                        editPlayerView.playerStyle.ApplyStyle(editPlayer);
                        break;
                }
            }
            
        }

        void InitializeEditPlayerView()
        {
            if(editPlayer == null)
            {
                editPlayer = FindObjectOfType<EditPlayerView>();
            }
        }

        void ApplyChanges()
        {
            editPlayerView.playerStyle.SetStyle(editPlayer.Skin.value, editPlayer.Face.value, editPlayer.Tatoo.value, editPlayer.Beard.value);
        }



    }
}