using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class TurnManager : MonoBehaviour
    {

        private int currentTurnIndex;
        private Turn currentTurn;

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

            gridManager.Init();
            Character[] characters = PlaceCharacters();

            turnSequence = TurnSequenceHelper.GetCharacterSequence(characters).ToArray();

            currentTurn = new Turn(turnSequence[0]);
            currentTurnIndex = 0;

        }

        private Character[] PlaceCharacters()
        {
            Character[] characters = FindObjectsOfType<Character>();

            foreach (var character in characters)
            {
                Node n = gridManager.GetNode(character.transform.position);
                if (n != null)
                {
                    character.transform.position = n.worldPosition;
                    character.currentNode = n;
                    n.character = character;
                }
                else
                {
                    Debug.LogError("Character " + character.name + "is out of bounds");
                }
            }

            return characters;
        }



        // Update is called once per frame
        void Update()
        {

            if (currentTurn.Execute(this))
            {
                if (currentTurnIndex == turnSequence.Length - 1)
                {
                    currentTurnIndex = 0;
                }
                else
                {
                    currentTurnIndex++;
                }

                currentTurn = new Turn(turnSequence[currentTurnIndex]);

            }

        }
    }

}