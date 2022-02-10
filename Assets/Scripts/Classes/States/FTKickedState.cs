using System;
namespace FootTactic
{

    public class FTKickedState : FTControlStateBase
    {
        int _player;

        public static readonly int Index = 0;

        public FTKickedState(FTController controller) : base(controller) {

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

            if (action == FTAction.selectPlayer)
            {
                Controller.State.ToState(Controller.SelectedState);
                Controller.inputHandler.SetPlayer1(Controller.selectedPlayer.PlayerIndex);
                Controller.inputHandler.SetPlayer2(-1);
            }

            if(action == FTAction.clickOnField)                    
            {
                Controller.inputHandler.SetPlayer1(_player);
                Controller.inputHandler.SetPos1(FTTargetController.TargetPosition);
                Controller.inputHandler.Kick();
                Controller.Anim.PlayWithTimeout(FTConstants.TIME_PASS + FTConstants.TIME_PASS_RUN);
                Controller.State.ToState(Controller.IdleState);
            }

          
        }

    }
}

