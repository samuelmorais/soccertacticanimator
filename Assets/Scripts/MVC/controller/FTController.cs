using UnityEngine;
using System.Collections;
using thelab.mvc;
using System;
using FootTactic;
using System.Collections.Generic;

namespace FootTactic
{
    
    public class FTController : Controller<FTApplication>
    {
		
		public FTAnimationController Anim;
        public FTContextMenu ContextMenu;
 
        [HideInInspector]
        public FTAnimPlayerView selectedPlayer;

        [SerializeField]
        public static Dictionary<int, FTAnimPlayerView> Players;

        [SerializeField]
        public static Dictionary<int, FTAnimPlayerController> PlayerControllers;

        [SerializeField]
        public FTAnimBallView Ball;

        public FTInputHandler inputHandler;

        private IFTControlState _state;

        public IFTControlState State
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }

        public IFTControlState PassedState;
        public IFTControlState IdleState;
        public IFTControlState KickedState;
        public IFTControlState SelectedState;

        public IFTControlState AnimatatedState;

        public void Update()
        {
            this.State.Update();
        }

        

        /// <summary>
        /// Handle notifications from the application.
        /// </summary>
        /// <param name="p_event"></param>
        /// <param name="p_target"></param>
        /// <param name="p_data"></param>
        public override void OnNotification(string p_event, UnityEngine.Object p_target, params object[] p_data)
		{   
			switch (p_event)
			{
                case "AppQuit":
                    QuitApplication();
                    break;

                case "Pass":
                    OnPass();
                    break;

                case "Kick":
                    OnKick();
                    break;

                case "KeepBall":
                    OnKeepBall();
                    break; 

                case "AnimHeader1":
                    OnAnimate();
                    break;               

                case "PlayerButton@up":
                    OnPlayerSelected((int)p_data[0]);
                    break;

                case "ClosedMenu":
                    OnPlayerDeselected();
                    break;

                
                case "Field2D@up":
                    OnClickField((Vector3)p_data[0]);
                    break;

                case "Save":
                    app.view.ShowSaveAsDialog();
                    break;

                case "Open":
                    app.view.ShowOpenDialog();
                    break;

                case "OpennedFile":
                    OnOpennedFile();
                    break;
            }
		}
        
        void QuitApplication() {
            Application.Quit();
        }



        private void Start()
        {
            IdleState = new FTIdleState(this);
            PassedState = new FTPassedState(this);
            SelectedState = new FTSelectedState(this);
            KickedState = new FTKickedState(this);
            AnimatatedState = new FTAnimatedState(this);
            Players = new Dictionary<int, FTAnimPlayerView>();
            PlayerControllers = new Dictionary<int, FTAnimPlayerController>();
            foreach (FTAnimPlayerView playerView in FindObjectsOfType<FTAnimPlayerView>())
            {
                Players.Add(playerView.PlayerIndex, playerView);
            }
            foreach (FTAnimPlayerController playerController in FindObjectsOfType<FTAnimPlayerController>())
            {
                PlayerControllers.Add(playerController.PlayerIndex, playerController);
            }
            State = IdleState;

            Notify("PlayersCreated");
        }

        void OnPass()
        {
            _state.HandleInput(FTAction.pass);
        }

        void OnAnimate()
        {
            _state.HandleInput(FTAction.animation);
        }

        void OnKick()
        {
            _state.HandleInput(FTAction.kick);
        }

        void OnKeepBall()
        {
            _state.HandleInput(FTAction.keepBall);
        }

        void OnPlayerSelected(int selectedPlayerIndex)
        {
            selectedPlayer = Players[selectedPlayerIndex];
            ContextMenu.BallCommandsAreDisabled = !Ball.IsBallNearPlayer(selectedPlayer.transform.position);
            
            _state.HandleInput(FTAction.selectPlayer);
        }

        void OnPlayerDeselected()
        {
            Debug.Log("Deselect");
            _state.HandleInput(FTAction.deselectPlayer);
        }

        void OnClickField(Vector3 pos)
        {
            FTTargetController.TargetPosition = pos;
            _state.HandleInput(FTAction.clickOnField);
        }

        void OnOpennedFile()
        {
            Anim.ResetState();
        }

    }
}
