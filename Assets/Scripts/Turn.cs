using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Turn
    {


        private ITurnState currentState;
        private Dictionary<ulong, Node> reachableNodes;

        GridManager gridManager;
        Character character;

        public Character GetCharacter()
        {
            return character;
        }

        public Turn(Character character, GridManager gridManager)
        {
            this.character = character;
            this.gridManager = gridManager;
            this.currentState = new DetectMouseState(this, gridManager);
        }

        public void SetReachableNodes(Dictionary<ulong, Node> reachableNodes)
        {
            this.reachableNodes = reachableNodes;
        }

        public Dictionary<ulong, Node> GetReachableNodes()
        {
            return this.reachableNodes;
        }


        public bool Execute(TurnManager turnManager)
        {
            Debug.Log("Executing turn with state " + currentState.GetType().Name);

            currentState = currentState.execute();

            Debug.Log("Completing turn with state " + currentState.GetType().Name);

            return currentState.isEndingState();
        }

    }
}

