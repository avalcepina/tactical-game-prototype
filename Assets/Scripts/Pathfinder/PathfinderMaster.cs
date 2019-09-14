using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

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

            singleton = this;

        }

        public void RequestPathfind(Character character, Node start, Node end, Pathfinder.PathfindingComplete callback, GridManager gridManager)
        {

            Pathfinder newJob = new Pathfinder(character, start, end, callback, gridManager);
            todoJobs.Add(newJob);

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