using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using thelab.mvc;
namespace FootTactic
{

    public class EditPlayerView : View<FTApplication>
    {
        public Dropdown Skin;
        public Dropdown Face;
        public Dropdown Tatoo;
        public Dropdown Beard;
        public Dropdown Hair;
        public Dropdown HairColor;
        public InputField FieldPlayerName;
        public InputField FieldPlayerNumber;
        bool justInitialized = false;

        public bool JustInitialized { get => justInitialized; set => justInitialized = value; }

        public void InitializedFields(IFTPlayer player)
        {
            justInitialized = true;
            Skin.value = player.SkinColor;
            Face.value = player.FaceStyle;
            Tatoo.value = player.Tatoo;
            Beard.value = player.Beard;
            Hair.value = player.HairStyle;
            HairColor.value = player.HairColor;
            FieldPlayerName.text = player.Name;
            FieldPlayerNumber.text = player.Number.ToString();
            justInitialized = false;

            Notify("UpdatePlayerPreview");
        }
    }
}