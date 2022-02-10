using System;
namespace FootTactic
{

    public abstract class FTControlStateBase : IFTControlState
    {
        FTController _controller;

        public FTController Controller
        {
            get
            {
                return _controller;
            }
        }

        public FTControlStateBase(FTController controller)
        {
            _controller = controller;
        }

        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual void ToState(IFTControlState targetState)
        {
            _controller.State.OnExit();
            _controller.State = targetState;
            _controller.State.OnEnter();
        }

        public abstract void Update();

        public abstract void HandleInput(FTAction action);

        public abstract int GetStateIndex();
        
    }
}