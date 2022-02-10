using System;
namespace FootTactic
{

    public class FTSelectedState : FTControlStateBase
    {
        int _player;

        public static readonly int Index = 0;

        public FTSelectedState(FTController controller) : base(controller) {

        }
        
        public override void OnEnter()
        {
            _player = Controller.selectedPlayer.PlayerIndex;
        }

        public override void OnExit()
        {

        }

        public override void Update()
        {

        }

        public override int GetStateIndex()
        {
            return Index;
        }

        public override void HandleInput(FTAction action)
        {
            if (action == FTAction.pass)
            {
                Controller.State.ToState(Controller.PassedState);
                Controller.inputHandler.SetPlayer1(Controller.selectedPlayer.PlayerIndex);
                Controller.inputHandler.SetPlayer2(-1);
            }

            if (action == FTAction.kick)
            {
                Controller.State.ToState(Controller.KickedState);
                Controller.inputHandler.SetPlayer1(Controller.selectedPlayer.PlayerIndex);
                FTTargetController.targetView.Enable();
                Controller.inputHandler.SetPlayer2(-1);
            }

            if (action == FTAction.selectPlayer)
            {
                Controller.State.ToState(Controller.SelectedState);
                Controller.inputHandler.SetPlayer1(Controller.selectedPlayer.PlayerIndex);
                Controller.inputHandler.SetPlayer2(-1);
                Controller.Notify("OpenMenu");
            }

            if (action == FTAction.deselectPlayer)
            {
                Controller.State.ToState(Controller.IdleState);
                Controller.inputHandler.SetPlayer1(-1);
                Controller.inputHandler.SetPlayer2(-1);               
            }

            if (action == FTAction.animation)
            {
                Controller.State.ToState(Controller.AnimatatedState);
                Controller.inputHandler.SetPlayer1(Controller.selectedPlayer.PlayerIndex);
                FTTargetController.targetView.Enable();
                Controller.inputHandler.SetPlayer2(-1);
            }

        }

    }
}

