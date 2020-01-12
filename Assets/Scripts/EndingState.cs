using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class EndingState : ITurnState
    {
        Turn turn;
        GridManager gridManager;


        public EndingState(Turn turn, GridManager gridManager)
        {
            this.turn = turn;
            this.gridManager = gridManager;

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