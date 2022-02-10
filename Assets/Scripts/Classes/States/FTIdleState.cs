using System;
namespace FootTactic
{

    public class FTIdleState : FTControlStateBase
    {
        int _player;

        public static readonly int Index = 0;

        public FTIdleState(FTController controller) : base(controller) {

        }
        
        public override void OnEnter()
        {
            _player = Controller.selectedPlayer.PlayerIndex;
        }

        public override void OnExit()
        {
            FTTargetController.targetView.Disable();
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
            
            if (action == FTAction.selectPlayer)
            {
                Controller.State.ToState(Controller.SelectedState);
                Controller.inputHandler.SetPlayer1(Controller.selectedPlayer.PlayerIndex);
                Controller.inputHandler.SetPlayer2(-1);
                Controller.Notify("OpenMenu");
            }

        }

    }
}

