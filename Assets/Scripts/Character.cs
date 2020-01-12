using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class Character : MonoBehaviour
    {

        [SerializeField]
        public int initiative;

        [SerializeField]
        public int actionPoints;

        [SerializeField]
        public string team;

        public Node currentNode;

    }

}