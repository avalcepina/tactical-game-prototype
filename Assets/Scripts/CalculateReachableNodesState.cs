using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SA
{

    public class CalculateReachableNodesState : ITurnState
    {

        Turn turn;

        public CalculateReachableNodesState(Turn turn)
        {
            this.turn = turn;
        }


        public ITurnState execute()
        {

            Debug.Log("Inside CalculateReachableNodes");

            Character character = turn.character;

            Dictionary<ulong, Node> reachableNodes = PathfinderMaster.singleton.RequestReachableNodes(character.currentNode, character.actionPoints, turn.gridManager);

            turn.gridManager.HighlightNodes(reachableNodes.Values.ToList());

            turn.SetReachableNodes(reachableNodes);

            return new DetectMouseState(turn);


        }

        public bool isEndingState()
        {
            return false;
        }
    }

}