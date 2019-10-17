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

            Node characterNode = turn.GetCharacter().currentNode;

            Debug.Log("Requesting pathfinding");
            Debug.Log("Character Node - x " + characterNode.x + " y " + characterNode.y + " z " + characterNode.z);
            Debug.Log("Target Node - x " + targetNode.x + " y " + targetNode.y + " z " + targetNode.z);

            PathfinderMaster.singleton.RequestPathfind(characterNode, targetNode, PathfinderCallback, gridManager);

            return this;

        }

        void PathfinderCallback(List<Node> p)
        {
            if (p == null)
            {
                Debug.Log("P is null!");
                return;
            }

            Debug.Log("Invoking pathfinding callback with result size " + p.Count);

            isPathfinding = false;

            if (p == null)
            {
                Debug.Log("Path is not valid");

                return;
            }
            else
            {
                path = p;

                LineRenderer lineRenderer = gridManager.GetLineRenderer();
                lineRenderer.positionCount = path.Count;
                var t = Time.time;

                Debug.Log("Path size is " + path.Count);

                for (int i = 0; i < path.Count; i++)
                {


                    Node pathNode = path[i];

                    Debug.Log("Position " + i + " in line renderer is x " + pathNode.x + " y " + pathNode.y + " z " + pathNode.z);

                    lineRenderer.SetPosition(i, new Vector3(pathNode.worldPosition.x, pathNode.worldPosition.y + 0.5f, pathNode.worldPosition.z));
                }

            }

        }

        public bool isEndingState()
        {
            return false;
        }

    }



}

