using System;
namespace FootTactic
{
    public class FTPassedState : FTControlStateBase
    {
        int _player;

        public static readonly int Index = 1;

        public FTPassedState(FTController controller) : base(controller)
        {

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
            switch (action)
            {
                case FTAction.keepBall:
                    Controller.State.ToState(Controller.IdleState);
                    Controller.inputHandler.SetPlayer1(_player);
                    Controller.inputHandler.SetPlayer2(Controller.selectedPlayer.PlayerIndex);
                    Controller.inputHandler.Pass();
                    break;

                case FTAction.selectPlayer:
                    if(Controller.selectedPlayer.PlayerIndex != _player)
                    {
                        Controller.inputHandler.SetPlayer1(_player);
                        Controller.inputHandler.SetPlayer2(Controller.selectedPlayer.PlayerIndex);
                        Controller.inputHandler.Pass();
                        Controller.Anim.PlayWithTimeout(FTConstants.TIME_PASS + FTConstants.TIME_PASS_RUN);
                        Controller.State.ToState(Controller.IdleState);
                    }
                    break;

            }
        }

    }

}

