using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    // [ExecuteInEditMode]
    public class GridManager : MonoBehaviour
    {
        Node[,,] grid;

        [SerializeField]
        float xzScale = 1f;

        [SerializeField]
        float yScale = 1f;

        [SerializeField]
        LineRenderer lineRenderer;

        Vector3 minPosition;
        Vector3 maxPosition;

        int maxX;
        int maxZ;
        int maxY;

        List<Vector3> nodeViz = new List<Vector3>();
        List<Vector3> nodeViz2 = new List<Vector3>();
        List<Vector3> nodeViz3 = new List<Vector3>();

        public Vector3 extends = new Vector3(1f, 1f, 1f);

        public Vector3 overlapExtends = new Vector3(0.4f, 0.4f, 0.4f);

        int pos_x;
        int pos_y;
        int pos_z;

        public GameObject unit;

        public GameObject cellVisualization;


        public LineRenderer GetLineRenderer()
        {
            return lineRenderer;
        }

        public void Init()
        {
            ReadLevel();

            // Node n = GetNode(unit.transform.position);
            // if (n != null)
            // {

            //     unit.transform.position = n.worldPosition;


            // }


        }

        public void ReadLevel()
        {
            GridPosition[] gp = GameObject.FindObjectsOfType<GridPosition>();

            float minX = float.MaxValue;
            float maxX = float.MinValue;
            float minZ = float.MaxValue;
            float maxZ = float.MinValue;
            float minY = float.MaxValue;
            float maxY = float.MinValue;

            for (int i = 0; i < gp.Length; i++)
            {

                Transform t = gp[i].transform;

                if (t.position.x < minX)
                {
                    minX = t.position.x;
                }

                if (t.position.x > maxX)
                {
                    maxX = t.position.x;
                }

                if (t.position.z < minZ)
                {
                    minZ = t.position.z;
                }

                if (t.position.z > maxZ)
                {
                    maxZ = t.position.z;
                }

                if (t.position.y < minY)
                {
                    minY = t.position.y;
                }

                if (t.position.y > maxY)
                {
                    maxY = t.position.y;
                }

            }

            pos_x = Mathf.FloorToInt((maxX - minX) / xzScale);
            pos_z = Mathf.FloorToInt((maxZ - minZ) / xzScale);
            pos_y = Mathf.FloorToInt((maxY - minY) / yScale);

            // We can't allow y to be 0, since it would mean the loop in create grid will not do anything
            if (pos_y == 0)
            {
                pos_y = 1;
            }

            minPosition = Vector3.zero;
            minPosition.x = minX;
            minPosition.z = minZ;
            minPosition.y = minY;


            maxPosition = Vector3.zero;
            maxPosition.x = maxX;
            maxPosition.z = maxZ;
            maxPosition.y = maxY;

            CreateGrid(pos_x, pos_z, pos_y);
        }

        void CreateGrid(int pos_x, int pos_z, int pos_y)
        {


            grid = new Node[pos_x, pos_z, pos_y];

            for (int y = 0; y < pos_y; y++)
            {
                for (int x = 0; x < pos_x; x++)
                {
                    for (int z = 0; z < pos_z; z++)
                    {
                        Node node = new Node();
                        node.x = x;
                        node.z = z;
                        node.y = y;

                        Vector3 worldPosition = minPosition;
                        worldPosition.x += (x * xzScale) + (xzScale / 2);
                        worldPosition.z += (z * xzScale) + (xzScale / 2);
                        worldPosition.y += (y * xzScale) + (yScale / 2);

                        node.worldPosition = worldPosition;

                        Vector3 extendsPosition = worldPosition;
                        extendsPosition.y -= Mathf.FloorToInt(extendsPosition.y / 2);

                        Collider[] overlapNode = Physics.OverlapBox(worldPosition, overlapExtends, Quaternion.identity);

                        // if (y == 1)
                        // {
                        //     nodeViz3.Add(worldPosition);
                        // }


                        if (overlapNode.Length > 0)
                        {

                            bool isWalkable = false;

                            for (int i = 0; i < overlapNode.Length; i++)
                            {
                                GridObject obj = overlapNode[i].GetComponentInChildren<GridObject>();

                                if (obj != null)
                                {


                                    if (Array.IndexOf(obj.walkableLevels, y) > -1 && node.obstacle == null)
                                    {

                                        isWalkable = true;

                                    }
                                    else
                                    {




                                        isWalkable = false;
                                        node.obstacle = obj;
                                    }

                                }
                            }



                            node.isWalkable = isWalkable;

                        }

                        if (node.isWalkable)
                        {

                            RaycastHit hit;

                            if (Physics.Raycast(node.worldPosition, Vector3.down, out hit, yScale / 2))
                            {
                                Debug.Log("Falling ");
                                node.worldPosition = hit.point;
                            }

                            GameObject tile = Instantiate(cellVisualization, node.worldPosition + Vector3.up * .01f, Quaternion.identity);
                            tile.SetActive(true);
                        }

                        grid[x, z, y] = node;

                        if (node.isWalkable)
                        {
                            nodeViz.Add(node.worldPosition);
                        }
                        else
                        {

                            nodeViz2.Add(node.worldPosition);

                        }

                    }
                }
            }
        }

        public Node GetNode(Vector3 worldPosition)
        {
            Vector3 position = worldPosition - minPosition;
            int x = Mathf.FloorToInt(position.x / xzScale);
            int y = Mathf.FloorToInt(position.y / yScale);
            int z = Mathf.FloorToInt(position.z / xzScale);
            return GetNode(x, y, z);
        }

        public Node GetNode(int x, int y, int z)
        {
            if (x < 0 || x > pos_x - 1 || y < 0 || y > pos_y - 1 || z < 0 || z > pos_z - 1)
            {
                return null;
            }

            return grid[x, z, y];
        }

        private void OnDrawGizmos()
        {


            // Gizmos.color = Color.red;
            // for (int i = 0; i < nodeViz.Count; i++)
            // {

            //     Gizmos.DrawWireCube(nodeViz[i], new Vector3(1f, 0f, 1f));

            // }

            // Gizmos.color = Color.green;

            // for (int i = 0; i < nodeViz2.Count; i++)
            // {
            //     Gizmos.DrawWireCube(nodeViz2[i], overlapExtends);
            // }

            // Gizmos.color = Color.blue;

            // for (int i = 0; i < nodeViz3.Count; i++)
            // {
            //     Gizmos.DrawWireCube(nodeViz2[i], overlapExtends);
            // }


        }

    }

}