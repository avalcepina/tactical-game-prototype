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
        int currentNodeIndex = 0;

        public MoveCharacter(Turn turn, GridManager gridManager, List<Node> path)
        {
            this.turn = turn;
            this.gridManager = gridManager;
            this.path = path;
        }

        public ITurnState execute()
        {

            Debug.Log("Inside MoveCharacter");

            Character character = turn.GetCharacter();

            character.transform.LookAt(path[currentNodeIndex].worldPosition);

            character.transform.position = path[currentNodeIndex].worldPosition;

            character.currentNode = path[currentNodeIndex];

            if (currentNodeIndex == (path.Count - 1))
            {

                character.actionPoints = character.actionPoints - (path.Count - 1);

                if (character.actionPoints > 0)
                {

                    return new DetectMouseState(turn, gridManager);

                }

                return new EndingState(turn, gridManager);

            }
            else
            {

                currentNodeIndex = currentNodeIndex + 1;

                return this;

            }



        }

        public bool isEndingState()
        {
            return false;
        }

    }



}
