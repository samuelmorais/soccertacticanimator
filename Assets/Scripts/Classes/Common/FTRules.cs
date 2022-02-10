using System;
using UnityEngine;
namespace FootTactic
{
    public static class FTRules
    {
        public static bool IsAnimationsInconsistent(int currentAnimIndex, int previousAnimIndex){
            if(currentAnimIndex == 4 && previousAnimIndex == 0){
                return true;
            }
            return false;
        }
    }
}