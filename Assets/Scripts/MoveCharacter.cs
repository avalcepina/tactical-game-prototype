using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class MoveCharacter : ITurnState
    {
        Turn turn;
        GridManager gridManager;
        List<Node> path;

        public MoveCharacter(Turn turn, GridManager gridManager, List<Node> path)
        {
            this.turn = turn;
            this.gridManager = gridManager;
            this.path = path;
        }

        public ITurnState execute()
        {

            Debug.Log("Inside MoveCharacter");

            return this;

        }

        public bool isEndingState()
        {
            return false;
        }

    }



}
