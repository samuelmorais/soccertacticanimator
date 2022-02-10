using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Animancer;

namespace FootTactic {
    public class FTFrame
    {
       
        IFrameBehaviour playBehaviour;
        private readonly Dictionary<FrameBehaviourType, FrameBehaviourFactory> _factories;

        private FTFrame()
        {
            _factories = new Dictionary<FrameBehaviourType, FrameBehaviourFactory>();

            foreach (FrameBehaviourType frameType in Enum.GetValues(typeof(FrameBehaviourType)))
            {
                var factory = (FrameBehaviourFactory)Activator.CreateInstance(Type.GetType("FootTactic." + Enum.GetName(typeof(FrameBehaviourType), frameType) + "Factory"));
                _factories.Add(frameType, factory);
            }
        }
        
        public static FTFrame InitializeFactories() => new FTFrame();

        public IFrameBehaviour ExecuteCreation(
            FrameBehaviourType frameBehaviour,
            float _speed,
            Quaternion _direction,
            Vector3 _position,
            float _timeElapsed,
            bool _defaultFrame,            
            int _animationIndex = 0) =>
            _factories[frameBehaviour].Create(
                _speed,
                _direction,
                _position,
                _timeElapsed,
                _defaultFrame,
                _animationIndex
            );
    }


    public interface IFrameBehaviour
    {

        void Play(Transform t, AnimancerComponent animancer);
        
    }

    public struct FTFrameData
    {
        public float speed;
        public Quaternion direction;
        public Vector3 position;
        public float timeElapsed;
        public bool defaultFrame;
        public int animationIndex;

        public FTFrameData(float _speed,
            Quaternion _direction,
            Vector3 _position,
            float _timeElapsed,
            bool _defaultFrame,
            int _animationIndex)
        {
            speed = _speed;
            direction = _direction;
            position = _position;
            timeElapsed = _timeElapsed;
            defaultFrame = _defaultFrame;
            animationIndex = _animationIndex;
        }
    }

    public struct FTBallFrameData
    {
        public Vector3 position;
        public int playerIndex;
        public BallEventData.BodyPartEnum BodyPart;
        public BallEventData.ActionEnum Action;

        public FTBallFrameData(Vector3 position, int playerIndex, BallEventData.BodyPartEnum bodyPart, BallEventData.ActionEnum action)
        {
            this.position = position;
            this.BodyPart = bodyPart;
            this.playerIndex = playerIndex;
            this.Action = action;
        }
    }

    public abstract class FrameBehaviour : MonoBehaviour
    {
        public float speed;
        public Quaternion direction;
        public Vector3 position;
        public float timeElapsed;
        public bool defaultFrame;
        
        public void ApplyDefaultAnimBehaviour(Transform t)
        {
            t.LookAt(GameObject.Find("Ball").transform);
        }
        public AnimStates animStates;
        

    }

    public class LocomotionBehaviour : FrameBehaviour, IFrameBehaviour
    {
        public LocomotionBehaviour(float _speed,
            Quaternion _direction,
            Vector3 _position,
            float _timeElapsed,
            bool _defaultFrame)
        {
            speed = _speed;
            direction = _direction;
            position = _position;
            timeElapsed = _timeElapsed;
            defaultFrame = _defaultFrame;
            animStates = FindObjectOfType<AnimStates>();
        }


        public void Play(Transform t, AnimancerComponent animancer)
        {

            t.localPosition = position;
            t.localRotation = direction;

            PlayAnimation(animancer);

            if (defaultFrame) { ApplyDefaultAnimBehaviour(t); }
        }

        void PlayAnimation(AnimancerComponent animancer)
        {
            var state = animancer.CurrentState;
                                   
            if (speed.Equals(0))
            {
                animancer.Play(animStates.Idle);
            }
            else if (speed < 2f)
            {
                animancer.Play(animStates.Walk);
            }
            else if (speed < 4f)
            {
                animancer.Play(animStates.Run);
            }
            else
            {
                animancer.Play(animStates.Dash);
            }

            state.NormalizedTime = timeElapsed % animancer.CurrentState.Length;

            state.Speed = 0;

        }

        
    }

    public class AnimationBehaviour : FrameBehaviour, IFrameBehaviour
    {
        
        public int animationIndex;
        

        public AnimationBehaviour(float _speed,
            Quaternion _direction,
            Vector3 _position,
            float _timeElapsed,
            bool _defaultFrame,
            int _animationIndex)
        {
            speed = _speed;
            direction = _direction;
            position = _position;
            timeElapsed = _timeElapsed;
            defaultFrame = _defaultFrame;
            animationIndex = _animationIndex;
            animStates = FindObjectOfType<AnimStates>();
        }

        

        public void Play(Transform t, AnimancerComponent animancer)
        {

            t.localPosition = position;
            t.localRotation = direction;
            t.localPosition = new Vector3(t.localPosition.x, t.localPosition.z, 0) / 0.05f;
            
            PlayAnimation(animancer);

            if (defaultFrame) { ApplyDefaultAnimBehaviour(t); }
        }

        void PlayAnimation(AnimancerComponent animancer)
        {
            var state = animancer.CurrentState;

            state.Speed = 0;

        }




    }

    public abstract class FrameBehaviourFactory
    {
        public abstract IFrameBehaviour Create(
            float _speed,
            Quaternion _direction,
            Vector3 _position,
            float _timeElapsed,
            bool _defaultFrame,
            int _animationIndex = 0);

    }

    public class LocomotionBehaviourFactory : FrameBehaviourFactory
    {
        public override IFrameBehaviour Create(
            float _speed,
            Quaternion _direction,
            Vector3 _position,
            float _timeElapsed,
            bool _defaultFrame,
            int _animationIndex = 0) => new LocomotionBehaviour(
                 _speed,
                 _direction,
                 _position,
                 _timeElapsed,
                 _defaultFrame);
    }

    public class AnimationBehaviourFactory : FrameBehaviourFactory
    {
        public override IFrameBehaviour Create(float _speed,
            Quaternion _direction,
            Vector3 _position,
            float _timeElapsed,
            bool _defaultFrame,
            int _animationIndex) => new AnimationBehaviour(_speed,
                 _direction,
                 _position,
                 _timeElapsed,
                 _defaultFrame,
                 _animationIndex);
    }

    public enum FrameBehaviourType
    {
        LocomotionBehaviour,
        AnimationBehaviour
    }

}