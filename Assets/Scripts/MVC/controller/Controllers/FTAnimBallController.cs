using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using thelab.mvc;
using System;

namespace FootTactic
{
    
	public class FTAnimBallController : Controller<FTApplication>
    {
        public FTAnimBallView ballView;
        

		public override void OnNotification(string p_event, UnityEngine.Object p_target, params object[] p_data)
        {
            switch (p_event)
            {
                case "RenderAnim":
                    ballView.UpdatePosition();
                    break;

                case "KeepBall":
                    KeepBall();
                    break;

                case "ChangeAnim@up":
                    ballView.UpdatePosition();
                    break;
            }
        }

        public void KeepBall()
        { 
            Vector3 pos = app.controller.selectedPlayer.transform.localPosition + Vector3.forward * 0.05f + Vector3.right * 0.04f;
            ballView.AddKeyFrame(new FTKeyFrameData(app.controller.Anim.CurrentTime, pos, app.controller.Anim.CurrentAnimation,0,app.controller.selectedPlayer.PlayerIndex,BallEventData.BodyPartEnum.RIGHT_FOOT,BallEventData.ActionEnum.TRAP));
            ballView.UpdatePosition();
        }
        
	}
}
