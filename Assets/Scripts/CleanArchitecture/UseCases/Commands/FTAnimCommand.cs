using System;
using UnityEngine;
using System.Collections.Generic;

namespace FootTactic
{
    public class FTAnimCommand : TMAbstractCommand
    {
        int player1;
        int player2;
        int animIndex;
        float rotation;
        public FTAnimCommand(int player1, int player2, int animIndex, float rotation)
        {
            this.player1 = player1;
            this.player2 = player2;
            this.animIndex = animIndex;
            this.rotation = rotation;
        }

        public override void Execute()
        {
            FTController.PlayerControllers[player1].RemovePreviousAnimationIfThereIsAnyInconsistency(animIndex);
            if(player2 >= 0){
                FTController.PlayerControllers[player1].AddAnimationCurrentFrame(animIndex, getPassDirection());
                FTController.PlayerControllers[player2].AddAnimationCurrentFrame(
                    FTConstants.ANIM_KEEP_BALL_INDEX,
                    getReceiveDirection(),
                    FTConstants.TIME_PASS,
                    BallEventData.ActionEnum.TRAP);
            }
            else{
                FTController.PlayerControllers[player1].AddAnimationCurrentFrame(animIndex, getAnimDirection());
            }           
        }

        float getPassDirection()
        {
            Vector3 direction = FTController.Players[player2].transform.position - FTController.Players[player1].transform.position;
            return Quaternion.LookRotation(direction).eulerAngles.y;
        }

        float getAnimDirection()
        {
            Vector3 direction = FTTargetController.TargetPosition - FTController.Players[player1].transform.position;
            return Quaternion.LookRotation(direction).eulerAngles.y;
        }

        float getReceiveDirection()
        {
            Vector3 direction = FTController.Players[player1].transform.position - FTController.Players[player2].transform.position;
            return Quaternion.LookRotation(direction).eulerAngles.y;
        }

        public override TMCommandInterface GetUndoCommand()
        {
            return new FTUndoAnimCommand(player1, player2, animIndex, rotation);
        }

    }

    public class FTUndoAnimCommand : TMAbstractCommand
    {
        int player1;
        int player2;
        int animIndex;
        float rotation;
        public FTUndoAnimCommand(int player1, int player2, int animIndex, float rotation)
        {
            this.player1 = player1;
            this.player2 = player2;
            this.animIndex = animIndex;
            this.rotation = rotation;
        }

        public override void Execute()
        {

        }

        public override TMCommandInterface GetUndoCommand()
        {
            return new FTAnimCommand(player1, player2, animIndex, rotation);
        }
    }
}

