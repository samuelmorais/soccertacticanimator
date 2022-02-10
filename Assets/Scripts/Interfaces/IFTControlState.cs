using System;
namespace FootTactic{

    public interface IFTControlState
    {
        void OnEnter();

        void OnExit();

        void ToState(IFTControlState targetState);

        void Update();

        void HandleInput(FTAction action);

        int GetStateIndex();
    }

}