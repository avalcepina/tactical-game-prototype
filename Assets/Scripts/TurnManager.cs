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

        public GridManager gridManager;

        // Start is called before the first frame update
        void Start()
        {

            gridManager.Init();
            Character[] characters = PlaceCharacters();

            Debug.Log("Calculating T=turn sequence");

            turnSequence = TurnSequenceHelper.GetCharacterSequence(characters).ToArray();

            Debug.Log("Turn sequence has been calculated");

            currentTurn = new Turn(turnSequence[0], gridManager);

            Debug.Log("Current payer is " + currentTurn.GetCharacter().name + " of team " + currentTurn.GetCharacter().team);

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

                currentTurn = new Turn(turnSequence[currentTurnIndex], gridManager);

            }

        }
    }

}