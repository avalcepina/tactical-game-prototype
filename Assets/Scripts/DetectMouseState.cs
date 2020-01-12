
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Concurrent;

namespace SA
{

    public struct PathBoundaries
    {
        public UInt64 start, end;

        public PathBoundaries(UInt64 start, UInt64 end)
        {

            this.start = start;
            this.end = end;

        }

    }


    public class DetectMouseState : ITurnState
    {
        Turn turn;
        GridManager gridManager;

        ConcurrentDictionary<PathBoundaries, List<Node>> pathfindingCache;

        public DetectMouseState(Turn turn, GridManager gridManager)
        {
            this.turn = turn;
            this.gridManager = gridManager;
            this.pathfindingCache = new ConcurrentDictionary<PathBoundaries, List<Node>>();
        }

        public ITurnState execute()
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {

                Node targetNode = gridManager.GetNode(hit.point);

                PathBoundaries key = new PathBoundaries(turn.GetCharacter().currentNode.Key, targetNode.Key);

                if (pathfindingCache.ContainsKey(key))
                {

                    if (pathfindingCache[key].Count > 0)
                    {

                        Debug.Log("Using cached value");

                        List<Node> path = pathfindingCache[key];

                        LineRenderer lineRenderer = gridManager.GetLineRenderer();
                        lineRenderer.positionCount = path.Count;
                        var t = Time.time;

                        for (int i = 0; i < path.Count; i++)
                        {
                            Node pathNode = path[i];

                            lineRenderer.SetPosition(i, new Vector3(pathNode.worldPosition.x, pathNode.worldPosition.y + 0.5f, pathNode.worldPosition.z));
                        }

                        if (Input.GetMouseButtonDown(0))
                        {

                            return new MoveCharacter(this.turn, this.gridManager, path);

                        }


                    }
                    else
                    {

                        Debug.Log("Path is unavailable");

                    }

                }
                else
                {

                    Debug.Log("Requesting pathfinding");

                    pathfindingCache.TryAdd(key, new List<Node>());

                    PathfinderMaster.singleton.RequestPathfind(turn.GetCharacter().currentNode, targetNode, PathfinderCallback, gridManager, turn.GetReachableNodes());


                }


            }

            return this;

        }

        public bool isEndingState()
        {
            return false;
        }

        void PathfinderCallback(Node start, Node end, List<Node> path)
        {

            Debug.Log("Pathfinder callback");
            Debug.Log("Pathfinder callback path size is " + path.Count);

            if (path.Count > 0)
            {

                PathBoundaries key = new PathBoundaries(start.Key, end.Key);

                pathfindingCache[key] = path;


            }

        }

    }




}