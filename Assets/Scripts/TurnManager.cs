using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class TurnManager : MonoBehaviour
    {

        private int currentTurn;

        private Character[] turnSequence;

        public LineRenderer pathViz;
        bool isPathfinding;
        public GridManager gridManager;

        public void PathfinderCall(Node targetNode, Character c)
        {

            if (!isPathfinding)
            {

                isPathfinding = true;

                PathfinderMaster.singleton.RequestPathfind(c, c.currentNode, targetNode, PathfinderCallback, gridManager);
            }

        }

        void PathfinderCallback(List<Node> p, Character c)
        {
            isPathfinding = false;

            if (p == null)
            {
                Debug.LogWarning("Path is not vlaid");
                return;
            }

            pathViz.positionCount = p.Count;
        }

        // Start is called before the first frame update
        void Start()
        {

            currentTurn = 0;

            Character[] characters = FindObjectsOfType<Character>();

            characters[0].initiative.CompareTo(characters[1].initiative);

            Array.Sort<Character>(characters, new Comparison<Character>(
                              (i1, i2) => i2.initiative.CompareTo(i1.initiative)));

            foreach (var item in characters)
            {



            }

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}