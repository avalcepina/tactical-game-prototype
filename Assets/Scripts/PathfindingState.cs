using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class PathfindingState : ITurnState
    {
        Turn turn;
        GridManager gridManager;
        Node targetNode;
        bool isPathfinding;
        List<Node> path;

        public PathfindingState(Turn turn, GridManager gridManager, Node targetNode)
        {
            this.turn = turn;
            this.gridManager = gridManager;
            this.isPathfinding = false;
            this.targetNode = targetNode;
            this.path = null;
        }

        public ITurnState execute()
        {

            Debug.Log("Inside Pathfinding. path  is " + path + " and pathfinding is " + isPathfinding);



            if (path != null)
            {
                Debug.Log("Path was found");

                return new MoveCharacter(turn, gridManager, path);
            }
            else
            {
                Debug.Log("Path is null");
            }

            if (isPathfinding)
            {
                Debug.Log("Pathfinding is undergoing");

                return this;
            }


            isPathfinding = true;

            Debug.Log("Requesting pathfinding");

            PathfinderMaster.singleton.RequestPathfind(turn.GetCharacter().currentNode, targetNode, PathfinderCallback, gridManager);

            return this;

        }

        void PathfinderCallback(List<Node> p)
        {
            Debug.Log("Invoking pathfinding callback with result size " + p.Count);

            isPathfinding = false;

            if (p == null)
            {
                Debug.Log("Path is not vlaid");

                return;
            }
            else
            {
                path = p;
            }

        }

        public bool isEndingState()
        {
            return false;
        }

    }



}

