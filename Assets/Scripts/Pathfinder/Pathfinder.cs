using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class Pathfinder
    {

        public GridManager gridManager;
        public Node startNode;
        public Node endNode;

        public volatile float timer;
        public volatile bool jobDone;

        public delegate void PathfindingComplete(List<Node> n);
        public PathfindingComplete completeCallback;
        public List<Node> targetPath;

        public Pathfinder(Node startNode, Node endNode, PathfindingComplete completeCallback, GridManager gridManager)
        {
            this.startNode = startNode;
            this.endNode = endNode;
            this.completeCallback = completeCallback;
            this.gridManager = gridManager;
        }


        public void FindPath()
        {
            targetPath = FindPathInternal();
            jobDone = true;

        }

        public void NotifyComplete()
        {

            if (completeCallback != null)
            {
                completeCallback(targetPath);
            }

        }

        public List<Node> FindPathInternal()
        {

            Debug.Log("Commencing pathfinding");

            List<Node> path = new List<Node>();
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            Debug.Log("Start node is x " + startNode.x + " y " + startNode.y + " z " + startNode.z);

            Debug.Log("Openset size is " + openSet.Count);


            while (openSet.Count > 0)
            {


                Node currentNode = openSet[0];

                for (int i = 0; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                    {

                        if (!currentNode.Equals(openSet[i]))
                        {
                            currentNode = openSet[i];
                        }

                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode.Equals(endNode))
                {
                    path = RetracePath(startNode, endNode);
                    break;
                }

                Debug.Log("Exploring neighbours " + GetNeighbours(currentNode).Count);

                foreach (Node neighbour in GetNeighbours(currentNode))
                {


                    if (!closedSet.Contains(neighbour))
                    {
                        float newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                        if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newMovementCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, endNode);
                            neighbour.parentNode = currentNode;
                            if (!openSet.Contains(neighbour))
                            {
                                openSet.Add(neighbour);
                            }
                        }
                    }
                }
            }

            Debug.Log("Pathfinder identified path with size " + path.Count);

            return path;
        }

        int GetDistance(Node start, Node end)
        {
            int distanceX = Mathf.Abs(start.x - end.x);
            int distanceZ = Mathf.Abs(start.z - end.z);

            if (distanceX > distanceZ)
            {
                return 14 * distanceZ + 10 * (distanceX - distanceZ);
            }

            return 14 * distanceX + 10 * (distanceZ - distanceX);
        }

        List<Node> GetNeighbours(Node node)
        {
            List<Node> nodes = new List<Node>();

            Debug.Log("Getting neighbours of node x " + node.x + " y " + node.y + " z " + node.z);


            for (int x = -1; x <= 1; x++)
            {

                for (int z = -1; z <= 1; z++)
                {

                    if (x == 0 && z == 0)
                        continue;

                    int _x = x + node.x;
                    int _y = startNode.y;
                    int _z = z + node.z;

                    Debug.Log("New neighbour x " + _x + " y " + _y + " z " + _z);
                    Node n = GetNode(_x, _z, _y);

                    Node newNode = GetNeighbour(n);

                    if (newNode != null)
                    {


                        nodes.Add(newNode);

                    }

                }



            }

            return nodes;

        }

        Node GetNode(int x, int z, int y)
        {
            return gridManager.GetNode(x, y, z);
        }

        Node GetNeighbour(Node node)
        {

            Node retValue = null;

            if (node.isWalkable)
            {
                Debug.Log("Node x " + node.x + " y " + node.y + " z " + node.z + " is walkable");

                retValue = node;
            }
            else
            {

                Debug.Log("Node x " + node.x + " y " + node.y + " z " + node.z + " is NOT walkable");
            }

            return retValue;
        }

        List<Node> RetracePath(Node start, Node end)
        {
            List<Node> path = new List<Node>();
            Node currentNode = end;
            while (currentNode != start)
            {
                path.Add(currentNode);
                currentNode = currentNode.parentNode;
            }
            path.Reverse();
            return path;
        }

    }

}


