﻿using System;
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

        public delegate void PathfindingComplete(Node start, Node end, List<Node> n);
        public PathfindingComplete completeCallback;
        public List<Node> targetPath;
        public Dictionary<ulong, Node> reachableNodes;

        public Pathfinder(Node startNode, Node endNode, PathfindingComplete completeCallback, GridManager gridManager)
        {
            this.startNode = startNode;
            this.endNode = endNode;
            this.completeCallback = completeCallback;
            this.gridManager = gridManager;
        }

        public Pathfinder(Node startNode, Node endNode, PathfindingComplete completeCallback, GridManager gridManager, Dictionary<ulong, Node> reachableNodes) : this(startNode, endNode, completeCallback, gridManager)
        {

            this.reachableNodes = reachableNodes;
        }


        public void FindPath()
        {
            targetPath = FindPathInternal();
            jobDone = true;

            Debug.Log("Pathfinder job completed");

            Debug.Log("Pathfinder result path size is " + targetPath.Count);

            if (completeCallback != null)
            {
                completeCallback(startNode, endNode, targetPath);
            }

        }

        public void NotifyComplete()
        {

            if (completeCallback != null)
            {
                completeCallback(startNode, endNode, targetPath);
            }

        }

        public Dictionary<UInt64, Node> getReachableNodes(int maxDistance)
        {
            Debug.Log("Retrieveing all reachable nodes");

            List<Tuple<Node, int>> openSet = new List<Tuple<Node, int>>();
            Dictionary<UInt64, Node> closedSet = new Dictionary<UInt64, Node>();

            openSet.Add(new Tuple<Node, int>(startNode, 0));

            while (openSet.Count > 0)
            {

                List<Tuple<Node, int>> newNodes = new List<Tuple<Node, int>>();

                foreach (var currentTuple in openSet)
                {
                    if (currentTuple.Item2 < maxDistance)
                    {

                        foreach (Node neighbour in GetNeighbours(currentTuple.Item1))
                        {

                            if (!closedSet.ContainsKey(neighbour.Key) && neighbour.isWalkable)
                            {

                                newNodes.Add(new Tuple<Node, int>(neighbour, currentTuple.Item2 + 1));


                                closedSet[neighbour.Key] = neighbour;

                            }
                        }

                    }

                }

                openSet = newNodes;


            }

            return closedSet;

        }

        public List<Node> FindPathInternal()
        {

            Debug.Log("Commencing pathfinding");

            List<Node> path = new List<Node>();

            List<Node> openSet = new List<Node>();
            Dictionary<UInt64, Node> closedSet = new Dictionary<UInt64, Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {


                Node currentNode = openSet[0];

                if (currentNode == null)
                {

                    Debug.LogError("Current node is unexpectedly null!");
                    return path;
                }

                for (int i = 0; i < openSet.Count; i++)
                {

                    //|| (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    if (openSet[i].fCost < currentNode.fCost)
                    {

                        // if (!currentNode.Equals(openSet[i]))
                        // {
                        currentNode = openSet[i];
                        // }

                    }
                }

                openSet.Remove(currentNode);
                closedSet[currentNode.Key] = currentNode;

                if (currentNode.Equals(endNode))
                {


                    path = RetracePath(startNode, endNode);
                    break;
                }

                foreach (Node neighbour in GetNeighbours(currentNode))
                {


                    if (!closedSet.ContainsKey(neighbour.Key) && reachableNodes.ContainsKey(neighbour.Key))
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

            return path;
        }

        int GetDistance(Node start, Node end)
        {
            // int distanceX = Mathf.Abs(start.x - end.x);
            // int distanceZ = Mathf.Abs(start.z - end.z);


            // //D * (dx + dy) + (D2 - 2 * D) * min(dx, dy)

            // return Mathf.Max(distanceX, distanceZ);

            // if (distanceX > distanceZ)
            // {
            //     return 14 * distanceZ + 10 * (distanceX - distanceZ);
            // }

            // return 14 * distanceX + 10 * (distanceZ - distanceX);


            int dif_x = Mathf.Abs(start.x - end.x);
            int dif_z = Mathf.Abs(start.z - end.z);


            // if i set this below 2.00f he will move in zig zags
            if (dif_x > dif_z)
            {
                return 2 * dif_z + (dif_x - dif_z);
            }
            else
            {
                return 2 * dif_x + (dif_z - dif_x);
            }


        }

        List<Node> GetNeighbours(Node node)
        {
            List<Node> nodes = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {

                for (int z = -1; z <= 1; z++)
                {

                    if (x == 0 && z == 0)
                        continue;

                    int _x = x + node.x;
                    int _y = startNode.y;
                    int _z = z + node.z;

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

            if (node == null)
            {
                Debug.Log("Node is null");
                return null;
            }

            Node retValue = null;

            if (node.isWalkable)
            {

                retValue = node;
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


