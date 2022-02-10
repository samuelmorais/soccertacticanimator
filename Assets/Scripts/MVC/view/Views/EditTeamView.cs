using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using thelab.mvc;

namespace FootTactic
{

    public class EditTeamView : View<FTApplication>
    {
        public InputField TeamName;
        public Image shirtColor;
        public Image shortColor;
        public Image socksColor;
        public Image shirtColorGK;
        public Image shortColorGK;
        public Image socksColorGK;
        public GameObject ColorPickerPanel;
        public ColorPickerUtil.ColorPicker ColorPicker;
        int colorBeingEdited = 1;

        public void SaveColor()
        {
            switch (colorBeingEdited)
            {
                case 1:
                    shirtColor.color = ColorPicker.newColor;
                    break;
                case 2:
                    shortColor.color = ColorPicker.newColor;
                    break;
                case 3:
                    socksColor.color = ColorPicker.newColor;
                    break;
                case 4:
                    shirtColorGK.color = ColorPicker.newColor;
                    break;
                case 5:
                    shortColorGK.color = ColorPicker.newColor;
                    break;
                case 6:
                    socksColorGK.color = ColorPicker.newColor;
                    break;
            }
            CloseColorPicker();
        }

        public void CancelColor()
        {
            CloseColorPicker();
        }

        public void SetColorBeingEdited(int colorIndex)
        {
            colorBeingEdited = colorIndex;
        }

        void CloseColorPicker()
        {
            ColorPickerPanel.SetActive(false);
        }

        public void UpdateColorsToCurrentTeam(Color[] colors)
        {
            shirtColor.color = colors[0];
       
            shortColor.color = colors[1];
           
            socksColor.color = colors[2];
           
            shirtColorGK.color = colors[3];
           
            shortColorGK.color = colors[4];
           
            socksColorGK.color = colors[5];
        }
    }
}