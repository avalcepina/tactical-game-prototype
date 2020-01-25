using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class MoveCharacter : ITurnState
    {
        Turn turn;
        List<Node> path;
        int currentNodeIndex = 0;
        float movementspeed = 1.5f;

        public MoveCharacter(Turn turn, List<Node> path)
        {
            this.turn = turn;
            this.path = path;
        }

        public ITurnState execute()
        {

            Debug.Log("Inside MoveCharacter");

            // var v3 = new Vector3(0.0f, Input.GetAxis("Horizontal"), 0.0f);

            // turn.cameraController.RotateCamera(v3);

            Character character = turn.character;

            character.transform.LookAt(path[currentNodeIndex].worldPosition);

            //character.transform.position = path[currentNodeIndex].worldPosition;


            // float step = movementspeed * Time.deltaTime; // calculate distance to move
            character.transform.position = Vector3.MoveTowards(character.transform.position, path[currentNodeIndex].worldPosition, movementspeed * Time.deltaTime);

            //            character.transform.Translate(Vector3.left * movementspeed * Time.deltaTime);

            if (Vector3.Distance(character.transform.position, path[currentNodeIndex].worldPosition) <= 0)
            {

                character.currentNode = path[currentNodeIndex];

            }
            else
            {

                return this;

            }



            if (currentNodeIndex == (path.Count - 1))
            {

                character.actionPoints = character.actionPoints - (path.Count);

                turn.cameraController.PositionCamera(character.currentNode.worldPosition);

                if (character.actionPoints > 0)
                {

                    return new CalculateReachableNodesState(turn);

                }

                return new EndingState(turn);

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
