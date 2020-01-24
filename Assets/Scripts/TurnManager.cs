using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SA
{

    public class TurnManager : MonoBehaviour
    {

        private int currentTurnIndex;
        private Turn currentTurn;

        private Character[] turnSequence;

        public LineRenderer pathViz;

        public GridManager gridManager;

        public CameraController cameraController;

        public Turn GetCurrentTurn()
        {
            return currentTurn;
        }

        // Start is called before the first frame update
        void Start()
        {

            gridManager.Init();
            Character[] characters = PlaceCharacters();

            Debug.Log("Calculating T=turn sequence");

            turnSequence = TurnSequenceHelper.GetCharacterSequence(characters).ToArray();

            Debug.Log("Turn sequence has been calculated");

            currentTurnIndex = 0;
            currentTurn = new Turn(turnSequence[0], gridManager, cameraController);

            Debug.Log("Current payer is " + currentTurn.character.name + " of team " + currentTurn.character.team);

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

                //TODO should be changed
                currentTurn = new Turn(turnSequence[currentTurnIndex], gridManager, cameraController);

            }

        }

    }

}