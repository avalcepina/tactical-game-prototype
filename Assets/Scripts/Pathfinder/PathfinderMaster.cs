﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

namespace SA
{

    public class PathfinderMaster : MonoBehaviour
    {

        public static PathfinderMaster singleton;

        List<Pathfinder> currentJobs;
        List<Pathfinder> todoJobs;
        int maxSimulatenous = 3;

        public float timerThreshold = 5;

        private void Awake()
        {
            currentJobs = new List<Pathfinder>();
            todoJobs = new List<Pathfinder>();
            singleton = this;

        }

        public void RequestPathfind(Node start, Node end, Pathfinder.PathfindingComplete callback, GridManager gridManager, Dictionary<ulong, Node> reachableNodes)
        {

            Debug.Log("Creating new pathfinding job");

            Pathfinder newJob = new Pathfinder(start, end, callback, gridManager, reachableNodes);
            todoJobs.Add(newJob);

        }

        public Dictionary<UInt64, Node> RequestReachableNodes(Node start, int maxCost, GridManager gridManager)
        {

            Debug.Log("Creating new pathfinding job");

            Pathfinder pf = new Pathfinder(start, null, null, gridManager);
            return pf.getReachableNodes(maxCost);

        }

        private void Update()
        {

            int i = 0;
            float delta = Time.deltaTime;

            while (i < currentJobs.Count)
            {

                if (currentJobs[i].jobDone)
                {
                    currentJobs[i].NotifyComplete();
                    currentJobs.RemoveAt(i);
                }
                else
                {
                    currentJobs[i].timer += delta;

                    if (currentJobs[i].timer > timerThreshold)
                    {
                        currentJobs[i].jobDone = true;
                    }

                    i++;
                }
            }

            if (todoJobs.Count > 0 && currentJobs.Count < maxSimulatenous)
            {

                Pathfinder job = todoJobs[0];
                todoJobs.RemoveAt(0);
                currentJobs.Add(job);

                Thread newThread = new Thread(job.FindPath);
                newThread.Start();
            }

        }

    }

}