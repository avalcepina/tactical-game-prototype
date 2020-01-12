using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SA
{

    public class StartingState : ITurnState
    {
        Turn turn;
        GridManager gridManager;


        public StartingState(Turn turn, GridManager gridManager)
        {
            this.turn = turn;
            this.gridManager = gridManager;

        }


        public ITurnState execute()
        {

            Debug.Log("Inside StartingState");

            Character currentCharacter = turn.GetCharacter();

            Dictionary<ulong, Node> reachableNodes = PathfinderMaster.singleton.RequestReachableNodes(currentCharacter.currentNode, currentCharacter.actionPoints, gridManager);

            gridManager.HighlightNodes(reachableNodes.Values.ToList());

            turn.SetReachableNodes(reachableNodes);

            return new DetectMouseState(turn, gridManager);

        }

        public bool isEndingState()
        {
            return true;
        }

    }



}