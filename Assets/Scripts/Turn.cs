using System.Collections;
using UnityEngine;

namespace SA
{
    public class Turn
    {


        private ITurnState currentState;

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

        public bool Execute(TurnManager turnManager)
        {
            Debug.Log("Executing turn with state " + currentState.GetType().Name);

            currentState = currentState.execute();

            Debug.Log("Completing turn with state " + currentState.GetType().Name);

            return currentState.isEndingState();
        }

    }
}

