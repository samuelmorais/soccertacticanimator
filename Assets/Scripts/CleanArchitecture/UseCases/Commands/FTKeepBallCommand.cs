using System;
using UnityEngine;
using System.Collections.Generic;

namespace FootTactic
{
    public class FTKeepBallCommand : TMAbstractCommand
    {
        int player1;
        int player2;
        
        public FTKeepBallCommand(int player1, int player2)
        {
            this.player1 = player1;
            this.player2 = player2;
        }

        public override void Execute()
        {
            FTController.PlayerControllers[player2].AddAnimationCurrentFrame(FTConstants.ANIM_KEEP_BALL_INDEX, getPlayerDirection(), FTConstants.TIME_PASS, BallEventData.ActionEnum.TRAP);
        }

        float getPlayerDirection()
        {
            return FTController.Players[player1].transform.localRotation.eulerAngles.y;
        }


        public override TMCommandInterface GetUndoCommand()
        {
            return new FTUndoKeepBallCommand(player1, player2);
        }

    }

    public class FTUndoKeepBallCommand : TMAbstractCommand
    {
        int player1;
        int player2;

        public FTUndoKeepBallCommand(int player1, int player2)
        {
            this.player1 = player1;
            this.player2 = player2;
        }

        public override void Execute()
        {

        }

        public override TMCommandInterface GetUndoCommand()
        {
            return new FTKeepBallCommand(player1, player2);
        }
    }
}

