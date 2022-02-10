using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;
using UnityEngine.UI;
using TMPro;

namespace FootTactic
{
    public class FTButtonPlayerView : DragView<FTApplication>
    {
        Vector3 lastValidPosition;

        [SerializeField]
        TextMeshProUGUI textName;

        [SerializeField]
        TextMeshProUGUI textNumber;

        public Vector3 LastValidPosition { get => lastValidPosition; set => lastValidPosition = value; }

        public void OnSelected()
        {
            GetComponent<Image>().sprite  = app.view.imageSelected;
        }

        public void OnUnSelected()
        {
            GetComponent<Image>().sprite = app.view.imageUnselected;
        }

        public void ReturnToLastValidPosition()
        {
            transform.position = lastValidPosition;
        }

        public void SaveValidPosition()
        {
            lastValidPosition = transform.position;
        }

        public void CheckValidPosition(bool isValid)
        {
            if (isValid) SaveValidPosition();
            else ReturnToLastValidPosition();
        }

        public void SetPlayerName(string _name)
        {
            if (textName != null && _name != "") { 
                textName.text = _name;
                VerifyPlayerNameVisibility(_name);
            }
            else
                Debug.LogError("textName null in player " + gameObject.name);
        }

        public void SetPlayerNumber(int number)
        {
            if (textNumber != null && number > 0)
                textNumber.text = number.ToString();
            else
                Debug.LogError("textNumber null in player " + gameObject.name);
        }

        private void Awake()
        {
            textName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            textNumber = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        void VerifyPlayerNameVisibility(string _name)
        {
            textName.gameObject.SetActive(!(_name == "" || _name == null || _name.StartsWith("Player ", System.StringComparison.InvariantCultureIgnoreCase)));
        }

        public void SetColorsOfTeamOnButton(EditTeamView editTeamView)
        {
            GetComponent<Image>().color = editTeamView.shirtColor.color;
            var colors = GetComponent<Button>().colors;
            colors.normalColor = editTeamView.shirtColor.color;
            GetComponent<Button>().colors = colors;
            
        }

    }

}

