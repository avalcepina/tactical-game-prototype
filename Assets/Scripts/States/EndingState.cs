using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class EndingState : ITurnState
    {

        Turn turn;

        public EndingState(Turn turn)
        {
            this.turn = turn;

        }


        public ITurnState execute()
        {

            Debug.Log("Inside EndingState");

            return this;

        }

        public bool isEndingState()
        {
            return true;
        }

    }



}