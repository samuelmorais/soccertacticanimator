using System;
using UnityEngine;
using System.Collections.Generic;

namespace FootTactic
{
    public class FTPassCommand : TMAbstractCommand
    {
        int _player1;
        int _player2;
        
        public FTPassCommand(int player1, int player2)
        {
            _player1 = player1;
            _player2 = player2;
        }

        public override void Execute()
        {
            FTController.PlayerControllers[_player1].AddAnimationCurrentFrame(FTConstants.ANIM_PASS_INDEX, getPassDirection());

            FTController.PlayerControllers[_player2].AddAnimationCurrentFrame(FTConstants.ANIM_KEEP_BALL_INDEX, getReceiveDirection(), FTConstants.TIME_PASS, BallEventData.ActionEnum.TRAP);
        }

        float getPassDirection()
        {
            Vector3 direction = FTController.Players[_player2].transform.position - FTController.Players[_player1].transform.position;
            return Quaternion.LookRotation(direction).eulerAngles.y;
        }

        float getReceiveDirection()
        {
            Vector3 direction = FTController.Players[_player1].transform.position - FTController.Players[_player2].transform.position;
            return Quaternion.LookRotation(direction).eulerAngles.y;
        }

        public override TMCommandInterface GetUndoCommand()
        {
            return new FTUndoPassCommand(_player1, _player2);
        }

    }

    public class FTUndoPassCommand : TMAbstractCommand
    {
        int _player1;
        int _player2;

        public FTUndoPassCommand(int player1, int player2)
        {
            _player1 = player1;
            _player2 = player2;
        }

        public override void Execute()
        {

        }

        public override TMCommandInterface GetUndoCommand()
        {
            return new FTPassCommand(_player1, _player2);
        }
    }
}

