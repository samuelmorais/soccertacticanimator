using UnityEngine;
using System.Collections.Generic;


namespace FootTactic{
    public class FTInputHandler : MonoBehaviour
    {
        private int _player1;
        private int _player2;
        private Vector3 _pos1;
        private Vector3 _pos2;
           

        public void Pass() {
            TMHelper.CommandHistory.Do(new FTPassCommand(_player1, _player2));
        }

        public void Kick() {
            TMHelper.CommandHistory.Do(new FTKickCommand(_player1, _player2));
        }

        public void KeepTheBall() {
            TMHelper.CommandHistory.Do(new FTKeepBallCommand(_player1, _player2));
        }

        public void Animate(int animIndex) {
            TMHelper.CommandHistory.Do(new FTAnimCommand(_player1, _player2, animIndex, 0));
        }

        public void CatchTheBall() {  }

        public void SetPlayer1(int player1)
        {
            _player1 = player1;
        }

        public void SetPlayer2(int player2)
        {
            _player2 = player2;
        }

        public void SetPos1(Vector3 pos1)
        {
            _pos1 = pos1;
        }

        public void SetPos2(Vector3 pos2)
        {
            _pos2 = pos2;
        }

    }
}