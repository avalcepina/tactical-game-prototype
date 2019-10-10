using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class DetectMouseState : ITurnState
    {
        Turn turn;
        GridManager gridManager;

        public DetectMouseState(Turn turn, GridManager gridManager)
        {
            this.turn = turn;
            this.gridManager = gridManager;
        }

        public ITurnState execute()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000))
                {

                    Node targetNode = gridManager.GetNode(hit.point);

                    if (targetNode != null)
                    {


                        return new PathfindingState(turn, gridManager, targetNode);

                    }


                }

            }

            return this;

        }

        public bool isEndingState()
        {
            return false;
        }
    }



}