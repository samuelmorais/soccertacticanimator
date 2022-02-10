using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;
using UnityEngine.UI;
using TMPro;

namespace FootTactic
{

    public class FTPlayerClickHandler : View<FTApplication>
    {
        int playerIndex;

        void Start(){
            GetIndexFromName();
        }
        void OnMouseOver(){
            if(Input.GetMouseButtonUp(0)){
                Notify("PlayerButton@up", playerIndex, transform.position);
            }
            if(Input.GetMouseButtonDown(0)){
                Notify("PlayerButton@down", playerIndex, transform.position);
            }
        }

        void GetIndexFromName(){
            playerIndex = int.Parse(gameObject.name.Replace("Player", "").Replace("Model",""));
        }
    }
}