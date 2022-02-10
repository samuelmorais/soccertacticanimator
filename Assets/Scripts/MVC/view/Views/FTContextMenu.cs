using UnityEngine;
using UnityEngine.UI;
using thelab.mvc;
using Battlehub.UIControls.MenuControl;
using System;

namespace FootTactic
{
    public class FTContextMenu : View<FTApplication>
    {
        [SerializeField]
        private Text m_output = null;
        public Camera camera2D;
        public GameObject imageBackground;
        public bool BallCommandsAreDisabled
        {
            get;
            set;
        }

        public void OnValidateCmd(MenuItemValidationArgs args)
        {
            if((args.Command == "Pass" || args.Command == "Kick" || args.Command == "KeepBall") &&
                BallCommandsAreDisabled)
            {
                args.IsValid = false;
            }
        }

        public void OnCmd(string cmd)
        {
            Debug.Log("Run Cmd: " + cmd);

            if (IsAnimationCommand(cmd))
            {
                HandleAnimationCommand(cmd);
            }
            else
            {
                HandleCommonCommands(cmd);
            }

            m_output.text = "Last Cmd: " + cmd;
        }

        private void HandleAnimationCommand(string cmd)
        {
            Notify(cmd);
        }

        bool IsAnimationCommand(string cmd)
        {
            return cmd.ToLower().StartsWith("anim");
        }

        void HandleCommonCommands(string cmd)
        {
            switch (cmd)
            {
                case "Open":
                    Notify("Open");
                    break;

                case "Create":

                    break;

                case "Save":
                    Notify("Save");
                    break;

                case "SaveAs":
                    Notify("SaveAs");
                    break;

                case "Exit":
                    Notify("AppQuit");
                    break;

                case "3D":
                    Notify("Enable3DView");
                    break;

                case "2D":
                    Notify("Enable2DView");
                    break;

                case "KeepBall":
                    Notify("KeepBall");
                    break;

                case "Pass":
                    Notify("Pass");
                    break;

                case "Kick":
                    Notify("Kick");
                    break;

                case "EditPlayer":
                    Notify("EditPlayer");
                    break;

                case "EditTeam":
                    Notify("EditTeam");
                    break;
            }
        }
        
    }
}
