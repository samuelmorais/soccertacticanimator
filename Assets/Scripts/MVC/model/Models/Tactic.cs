using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace FootTactic{
	public class Tactic : ITactic
	{
        public Dictionary<int,FTAnimationPlayer[]> playersAnimations { get; set; }
        public FTAnimationBall[] ballAnimations { get; set; }

		public Tactic(AnimStates animStates){
            Name = "FootTactic";
            
            InitializeAnimations(animStates);

            AddDefaultTeams();

            LoadTeams();

        }

        public IFTTeam[] Teams { get; set; }

        public Tactic(QuickType.TacticJson tacticJson, AnimStates animStates)
        {

            InitializeAnimations(animStates);

            Name = "FootTactic";

            AddDefaultTeams();

            AddPlayersAnimations(tacticJson);

            AddBallAnimations(tacticJson);

        }



        public string Name
		{
            get; set;
		}

        public List<FTAnimationData> Animations
        {
            get;
            set;
        }

        
        public void InitializeAnimations(AnimStates animStates)
        {
            Animations = new List<FTAnimationData>();
            
            FTAnimationData idle = new FTAnimationData("Idle", 0, animStates.Animations[0].length, new List<FTAnimEvent>(), Vector3.zero, 0);
            Animations.Add(idle);

            var animEventsPass = new List<FTAnimEvent>();
            animEventsPass.Add(
                new FTAnimEvent(
                FTConstants.RIGHT_FOOT_GROUND_OFFSET,
                0.25f,
                BallEventData.ActionEnum.IMPACT_WEAK,
                BallEventData.BodyPartEnum.RIGHT_FOOT,
                Vector3.forward
                ));
            FTAnimationData pass = new FTAnimationData("Pass", 1, animStates.Animations[1].length, animEventsPass, Vector3.zero, 0);
            Animations.Add(pass);

            var animEventsKick = new List<FTAnimEvent>();
            animEventsKick.Add(
                new FTAnimEvent(
                FTConstants.RIGHT_FOOT_GROUND_OFFSET,
                0.3f,
                BallEventData.ActionEnum.IMPACT_HARD,
                BallEventData.BodyPartEnum.RIGHT_FOOT,
                Vector3.forward
                )); 
            FTAnimationData kick = new FTAnimationData("Kick", 2, animStates.Animations[2].length, animEventsKick, Vector3.zero, 0);
            Animations.Add(kick);


            var animEventsKeepBall = new List<FTAnimEvent>();
            animEventsKeepBall.Add(
                new FTAnimEvent(
                FTConstants.RIGHT_FOOT_GROUND_OFFSET,
                0.5f,
                BallEventData.ActionEnum.TRAP,
                BallEventData.BodyPartEnum.RIGHT_FOOT,
                Vector3.forward
                ));
            FTAnimationData keepTheBall = new FTAnimationData("KeepTheBall", 3, animStates.Animations[3].length, animEventsKeepBall, Vector3.zero, 0);
            Animations.Add(keepTheBall);

            ImportAnimEditorAnimations(animStates);           
        }

        void ImportAnimEditorAnimations(AnimStates animStates){
            animStates.Anims.ToList().ForEach(
                anim => {
                    
                    var events = new List<FTAnimEvent>();
                    anim.BallEvents.ToList().ForEach(
                        evnt => events.Add(new FTAnimEvent(evnt.OffsetFromRoot,
                                                            evnt.Time,
                                                            evnt.Action,
                                                            evnt.BodyPart,
                                                            evnt.Direction))
                    );
                    Animations.Add(new FTAnimationData(anim.Name,
                                    anim.Index,
                                    anim.AnimationClip.length,
                                    events,
                                    anim.FinalPosition,
                                    anim.FinalRotation));
                }
            );
        }


        public void UpdateAnimations(FTAnimBallView ballView) {

            playersAnimations = new Dictionary<int, FTAnimationPlayer[]>();
            ballAnimations = new FTAnimationBall[10];
            foreach (var playerView in FTController.Players.Values)
            {
                playersAnimations[playerView.PlayerIndex] = playerView.GetAnimations();
            }
            ballAnimations = ballView.GetAnimations();
        }

        public void LoadTeams()
        {
           
            foreach (var playerView in FTController.Players.Values)
            {
                if(playerView.PlayerStyle != null && playerView.PlayerIndex > 0)
                {
                    var teamIndex = FTUtil.GetTeamIndex(playerView.PlayerIndex);
                   playerView.PlayerStyle.SetPlayerVisual(Teams[teamIndex], Teams[teamIndex].Players.Find(p => p.PlayerIndex == playerView.PlayerIndex));                    
                }
            }
        }

        public void LoadAnimations(FTAnimBallView ballView)
        {
            foreach (var playerView in FTController.Players.Values)
            {
                playerView.SetAnimations(playersAnimations[playerView.PlayerIndex]);
            }

            ballView.SetAnimations(ballAnimations);

        }

        void ITactic.InitializeAnimations(AnimStates animStates)
        {
            throw new System.NotImplementedException();
        }

        void AddDefaultTeams()
        {
            List<IFTPlayer> playersTeam1 = new List<IFTPlayer>();
            List<IFTPlayer> playersTeam2 = new List<IFTPlayer>();
            for (var i = 0; i < FTConstants.NUM_PLAYERS_PER_TEAM; i++)
            {
                playersTeam1.Add(new FTPlayer($"Player {i + 1}", i + 1, Random.Range(0,19), Random.Range(0, 5), Random.Range(0, 5), Random.Range(0, 8), Random.Range(0,1), Random.Range(0,1), FTConstants.TEAM1_START_INDEX + i, FTUtil.GetPlayerObjByIndex(FTConstants.TEAM1_START_INDEX+i)));
                playersTeam2.Add(new FTPlayer($"Player {i + 1}", i + 1, Random.Range(0, 19), Random.Range(0, 5), Random.Range(0, 5), Random.Range(0, 8), Random.Range(0, 1), Random.Range(0, 1), FTConstants.TEAM2_START_INDEX + i, FTUtil.GetPlayerObjByIndex(FTConstants.TEAM2_START_INDEX + i)));
            }
            Teams = new IFTTeam[2];
            Teams[0] = new FTTeam("Team 1", 0, "#0000ff", "#ffffff", "#0000ff", "#0000ff", "#ffffff", "#0000ff", playersTeam1);
            Teams[1] = new FTTeam("Team 2", 0, "#ff0000", "#ffffff", "#ff0000", "#ff0000", "#ffffff", "#ff0000", playersTeam2);
        }

        void AddPlayersAnimations(QuickType.TacticJson tacticJson)
        {
            playersAnimations = new Dictionary<int, FTAnimationPlayer[]>();
            foreach (string iStr in tacticJson.PlayersAnimations.Keys)
            {
                int i = int.Parse(iStr);
                playersAnimations[i] = new FTAnimationPlayer[FTConstants.NUM_ANIMS];

                for (var j = 0; j < FTConstants.NUM_ANIMS; j++)
                {
                    if (tacticJson.PlayersAnimations[iStr][j].NumFrames > 0)
                    {
                        playersAnimations[i][j] = new FTAnimationPlayer();
                        playersAnimations[i][j].NumFrames = (int)tacticJson.PlayersAnimations[iStr][j].NumFrames;
                        var l = tacticJson.PlayersAnimations[iStr][j].NumFrames;
                        for (var k = 0; k < l; k++)
                        {
                            QuickType.PlayersAnimationKeyFrame kf = tacticJson.PlayersAnimations[iStr][j].KeyFrames[k];
                            playersAnimations[i][j].KeyFrames[k] =
                                new FTKeyFrame(
                                new Vector3((float)kf.Vec.X, (float)kf.Vec.Y, (float)kf.Vec.Z)
                                , kf.Time, kf.Smooth, (int)kf.AnimIndex, kf.Rotation.Equals(FTConstants.UNSET_ROTATION) ? Mathf.Infinity : kf.Rotation);
                        }


                    }

                }
            }
        }

        void AddBallAnimations(QuickType.TacticJson tacticJson)
        {

            ballAnimations = new FTAnimationBall[FTConstants.NUM_ANIMS];
            for (var i = 0; i < FTConstants.NUM_ANIMS; i++)
            {
                ballAnimations[i] = new FTAnimationBall();
                ballAnimations[i].NumFrames = (int)tacticJson.BallAnimations[i].NumFrames;

                if (tacticJson.BallAnimations[i].NumFrames > 0)
                {

                    var l = tacticJson.BallAnimations[i].NumFrames;

                    for (var k = 0; k < l; k++)
                    {
                        QuickType.BallAnimationKeyFrame kf = tacticJson.BallAnimations[i].KeyFrames[k];

                        ballAnimations[i].KeyFrames[k] =
                            new FTKeyFrameBall(
                            new Vector3((float)kf.Vec.X, (float)kf.Vec.Y, (float)kf.Vec.Z)
                            , kf.Time, (BallEventData.BodyPartEnum)kf.BodyPart, (BallEventData.ActionEnum)kf.EventType, (int)kf.PlayerIndex, kf.Smooth);
                    }
                }
            }
        }

        public Color[] GetTeamColors(int teamIndex)
        {
            Color[] colors = new Color[6];
            ColorUtility.TryParseHtmlString(Teams[teamIndex].Color1, out colors[0]);
            ColorUtility.TryParseHtmlString(Teams[teamIndex].Color2, out colors[1]);
            ColorUtility.TryParseHtmlString(Teams[teamIndex].Color3, out colors[2]);
            ColorUtility.TryParseHtmlString(Teams[teamIndex].GKColor1, out colors[3]);
            ColorUtility.TryParseHtmlString(Teams[teamIndex].GKColor2 , out colors[4]);
            ColorUtility.TryParseHtmlString(Teams[teamIndex].GKColor3 , out colors[5]);
            return colors;
        }

        public string GetTeamName(int teamIndex)
        {
            return Teams[teamIndex].Name;           
        }
    }
}

