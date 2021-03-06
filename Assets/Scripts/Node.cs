﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class Node
    {
        public int x;
        public int y;
        public int z;

        public bool isWalkable;
        public Vector3 worldPosition;

        public GridObject obstacle;
        public Character character;

        public float hCost;
        public float gCost;
        public float fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        public Node parentNode;

        public UInt64 Key { get { return (((UInt64)(UInt32)x) << 32) | (UInt64)(UInt32)y | (UInt64)(UInt32)z; } }

    }
}
