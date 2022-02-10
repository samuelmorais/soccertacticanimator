using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;
using System.Linq;

namespace FootTactic
{


    public class PlayersAssetsView : View<FTApplication>
    {
      
		//public AnimatorOverrideController animOverrideCtl;

		public static PlayersAssetsView instance;

        List<PlayerSkinAsset> playerSkins;

        List<PlayerHairAsset> playerHairs;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }

        private void Start()
        {
            InitializeSkins();
        }

        public GameObject[] HairStyles;
        public Color[] HairColors;
        public Texture2D[] Skins;
        public Texture2D shirtTexture;
        public Texture2D shortTexture;
        public Texture2D socksTexture;

        public Texture2D[] leftNumbers;

        public Texture2D[] rightNumbers;

        public Texture2D[] centerNumbers;

        void InitializeSkins()
        {
            playerSkins = new List<PlayerSkinAsset>();
            foreach(Texture2D skin in Skins)
            {
                var face = int.Parse(skin.name.Substring(5,3));
                var tatoo = int.Parse(skin.name.Substring(15, 3)) > 0;
                var beard = int.Parse(skin.name.Substring(12, 2)) > 0;
                var color = int.Parse(skin.name.Substring(9, 2));
                playerSkins.Add(new PlayerSkinAsset(beard, tatoo, color, face, skin));
            }
        }

        public GameObject CreateHair(int hairStyle, int hairColor = -1)
        {
            var returnHair = Instantiate(HairStyles[hairStyle]);
            if(hairColor >= 0)
            {
                returnHair.GetComponent<Renderer>().material.SetColor("_Color", HairColors[hairColor]);
            }            
            return returnHair;
        }

        public Texture2D GetPlayerSkin(bool _tatoo, bool _beard, int _color, int _face)
        {
            var returnSkin = Skins[0];

            PlayerSkinAsset playerSkin = playerSkins.Find(x =>
            x.Face == _face &&
            x.Color == _color &&
            x.Tatoo == _tatoo &&
            x.Beard == _beard);
                
            if (playerSkin != null)
            {
                returnSkin = playerSkin.texture;
            }
            else
            {
                playerSkin = playerSkins.Find(x =>
                    x.Face == _face &&
                    x.Color == _color &&                    
                    x.Beard == _beard);

                if (playerSkin != null)
                {
                    returnSkin = playerSkin.texture;
                }
                else
                {
                    playerSkin = playerSkins.Find(x =>
                    x.Face == _face &&
                    x.Color == _color &&
                    x.Tatoo == _tatoo);

                    if (playerSkin != null)
                    {
                        returnSkin = playerSkin.texture;
                    }
                    else
                    {
                        playerSkin = playerSkins.Find(x => x.Face == _face && x.Color == _color);

                        if (playerSkin != null)
                        {
                            returnSkin = playerSkin.texture;
                        }
                        
                    }
                }
            }
            
            return returnSkin;
            
        }

    }

    public class PlayerSkinAsset
    {
        bool tatoo;
        bool beard;
        int color;
        int face;
        public Texture2D texture;
        public bool Tatoo { get => tatoo; set => tatoo = value; }
        public bool Beard { get => beard; set => beard = value; }
        public int Color { get => color; set => color = value; }
        public int Face { get => face; set => face = value; }

        public PlayerSkinAsset(bool _tatoo, bool _beard, int _color, int _face, Texture2D _texture)
        {
            tatoo = _tatoo;
            beard = _beard;
            color = _color;
            face = _face;
            texture = _texture;
        }
    }


    public class PlayerHairAsset
    {
        int color;
        int hairType;
        public GameObject hairGO;
        public int Color { get => color; set => color = value; }
        public int HairType { get => hairType; set => hairType = value; }

        public PlayerHairAsset(int _color, int _hairType, GameObject _hairGO)
        {
            color = _color;
            hairType = _hairType;
            hairGO = _hairGO;
            
        }
    }

}
