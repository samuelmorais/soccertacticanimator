using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

namespace FootTactic
{
    public class FTButtonView : ButtonView<FTApplication>
    {
        [SerializeField]
        int PlayerIndex;
        public override void OnPointerUp(PointerEventData eventData)
        {
            down = false;            
            Notify(notification + "@up", PlayerIndex, transform.position);
            hold = 0f;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            down = false;
            Notify(notification + "@down", PlayerIndex, transform.position);
            hold = 0f;
        }



        private void Start()
        {
            try
            {
                PlayerIndex = int.Parse(Regex.Match(gameObject.name, @"\d+").Value);
            }
            catch(System.Exception ex)
            {
                PlayerIndex = 0;
            }
           
        }
    }

}

