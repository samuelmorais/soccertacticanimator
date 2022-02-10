using System;
using UnityEngine;
using System.Collections.Generic;

namespace FootTactic
{
    public class FTKickCommand : TMAbstractCommand
    {
        int _player1;
        int _player2;
        
        public FTKickCommand(int player1, int player2)
        {
            _player1 = player1;
            _player2 = player2;
        }

        public override void Execute()
        {
            FTController.PlayerControllers[_player1].AddAnimationCurrentFrame(FTConstants.ANIM_KICK_INDEX, getKickDirection());
            
        }

        float getKickDirection()
        {
            Vector3 direction = FTTargetController.TargetPosition - FTController.Players[_player1].transform.position;
            return Quaternion.LookRotation(direction).eulerAngles.y;
        }


        public override TMCommandInterface GetUndoCommand()
        {
            return new FTUndoKickCommand(_player1, _player2);
        }

    }

    public class FTUndoKickCommand : TMAbstractCommand
    {
        int _player1;
        int _player2;

        public FTUndoKickCommand(int player1, int player2)
        {
            _player1 = player1;
            _player2 = player2;
        }

        public override void Execute()
        {

        }

        public override TMCommandInterface GetUndoCommand()
        {
            return new FTKickCommand(_player1, _player2);
        }
    }
}

