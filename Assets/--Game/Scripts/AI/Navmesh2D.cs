using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Navmesh2D : MonoBehaviour
{
	private static Navmesh2D _instance;
	public static Navmesh2D Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    //public NavmeshNode[] nodes;

    public NavmeshNode[,] nodes;

    // Pour opti plus tard
    public List<NavmeshNode> nodesNavmesh = new List<NavmeshNode>();

    [Title("Calculate NavMesh")]
    [SerializeField]
    BoxCollider area;
    [SerializeField]
    NavmeshNode node;

    [Title("Parameter")]
    [SerializeField]
    LayerMask groundLayerMask;
    [SerializeField]
    float agentSize = 1;
    [SerializeField]
    float checkZ = 1;

    [Title("Parameter - Jump")]
    [SerializeField]
    float jumpHeight = 14;
    [SerializeField]
    float aerialSpeed = 8;
    [SerializeField]
    float gravity = 35;

    [Space]
    [SerializeField]
    float nbTrajectory = 1;
    [SerializeField]
    int jumpPointInterval = 4;
    [SerializeField]
    int maxNbOfPoint = 100;

    [SerializeField]
    LayerMask wallLayerMask;

    [Button]
    // Editor only
    public void CalculateNavMesh()
    {
        ClearOldNavMesh();
        float sizeX = (int)(area.bounds.size.x / agentSize);
        float sizeY = (int)(area.bounds.size.y / agentSize);
        nodes = new NavmeshNode[(int)sizeX, (int)sizeY];

        Vector3 pos = area.bounds.min;
        RaycastHit hit;
        RaycastHit hitInWall;
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                pos = area.bounds.min + new Vector3(area.bounds.size.x * (x / sizeX), area.bounds.size.y * (y / sizeY));
                Physics.Raycast(pos, Vector3.down, out hit, agentSize, groundLayerMask);
                Physics.Raycast(pos - new Vector3(0, 0, checkZ), Vector3.forward, out hitInWall, checkZ, groundLayerMask);
                if (hit.collider != null && hitInWall.collider == null)
                {
                    nodes[x,y] = Instantiate(node, hit.point, Quaternion.identity, this.transform);
                    nodesNavmesh.Add(nodes[x, y]);
                }
                else
                {
                    nodes[x, y] = null;
                }
            }
        }

        CalculateLinkNodes();
    }


    [Button]
    // Editor only
    public void ClearOldNavMesh()
    {
        if (nodes != null)
        {
            for (int x = 0; x < nodes.GetUpperBound(0); x++)
            {
                for (int y = 0; y < nodes.GetUpperBound(1); y++)
                {
                    if (nodes[x, y] != null)
                    {
                        DestroyImmediate(nodes[x, y].gameObject);
                        nodes[x, y] = null;
                    }
                }
            }
        }

        for (int i = 0; i < nodesNavmesh.Count; i++)
        {
            if(nodesNavmesh[i] != null)
                DestroyImmediate(nodesNavmesh[i].gameObject);
        }
        nodesNavmesh.Clear();
    }




    private void CalculateLinkNodes()
    {
        float sizeX = (int)(area.bounds.size.x / agentSize);
        float sizeY = (int)(area.bounds.size.y / agentSize);
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (nodes[x, y] != null)
                {
                    CalculateRunNodes(nodes[x, y], x, y);
                    CalculateFallNodes(nodes[x, y], x, y);
                   // CalculateJumpNodes(nodes[x, y]);
                }
            }
        }
        // On fait ça en deux temps parce que je ne veux pas qu'on saute sur un endroit accessible en courant
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (nodes[x, y] != null)
                {
                    CalculateJumpNodes(nodes[x, y]);
                }
            }
        }
    }

    private void CalculateRunNodes(NavmeshNode node, int nodeX, int nodeY)
    {
        if (nodes[nodeX - 1, nodeY] != null)
            node.navmeshNodesRun.Add(nodes[nodeX - 1, nodeY]);
        if (nodes[nodeX + 1, nodeY] != null)
            node.navmeshNodesRun.Add(nodes[nodeX + 1, nodeY]);
        // On check qu'il n'y ait pas de murs entre le point de départ et le point final
    }

    private void CalculateFallNodes(NavmeshNode node, int nodeX, int nodeY)
    {
        for (int y = nodeY - 1; y >= 0; y--)
        {
            if (nodes[nodeX, y] != null)
            {
                Vector2 direction = nodes[nodeX, y].transform.position - node.transform.position;
                if (CheckObstaclesFallNodes(direction, node))
                {
                    node.navmeshNodesFall.Add(nodes[nodeX, y]);
                }
                break;
            }
        }

        for (int y = nodeY - 1; y >= 0; y--)
        {
            if (nodes[nodeX - 1, y] != null)
            {
                Vector2 direction = nodes[nodeX-1, y].transform.position - node.transform.position;
                if (CheckObstaclesFallNodes(direction, node))
                {
                    node.navmeshNodesFall.Add(nodes[nodeX-1, y]);
                }
                break;
            }
        }

        for (int y = nodeY - 1; y >= 0; y--)
        {
            if (nodes[nodeX + 1, y] != null)
            {
                Vector2 direction = nodes[nodeX + 1, y].transform.position - node.transform.position;
                if (CheckObstaclesFallNodes(direction, node))
                {
                    node.navmeshNodesFall.Add(nodes[nodeX + 1, y]);
                }
                break;
            }
        }
        /*if (nodes[nodeX - 1, nodeY] == null)
        {
            for (int y = nodeY-1; y >= 0; y--)
            {
                if (nodes[nodeX - 1, y] != null)
                {
                    node.navmeshNodesFall.Add(nodes[nodeX - 1, y]);
                    break;
                }
            }
        }

        if (nodes[nodeX + 1, nodeY] == null)
        {
            for (int y = nodeY-1; y >= 0; y--)
            {
                if (nodes[nodeX + 1, y] != null)
                {
                    node.navmeshNodesFall.Add(nodes[nodeX + 1, y]);
                    break;
                }
            }
        }*/


    }

    private bool CheckObstaclesFallNodes(Vector2 direction, NavmeshNode startNode)
    {
        RaycastHit hit;
        Physics.Raycast(startNode.transform.position + new Vector3(0, agentSize, 0), new Vector3(direction.x, 0, 0), out hit, Mathf.Abs(direction.x), wallLayerMask);
        if (hit.collider == null)
        {
            Physics.Raycast(startNode.transform.position + new Vector3(direction.x, agentSize, 0), Vector3.down, out hit, Mathf.Abs(direction.y), wallLayerMask);
            if (hit.collider == null)
                return true;
        }
        return false;
    }



    private void CalculateJumpNodes(NavmeshNode node)
    {
        // On sélectionne uniquement les nodes de saut descandant
        for (int i = (int)-nbTrajectory; i <= nbTrajectory; i++)
        {
            node.CalculateJumpTrajectory(jumpHeight, aerialSpeed * (i / nbTrajectory), gravity, maxNbOfPoint, jumpPointInterval, checkZ, wallLayerMask);

            if (node.jumpTrajectory.Count == 0)
                continue;


            // On calcul si il y a des nodes à la bonne distance
            float startPosY = node.jumpTrajectory[0].y;
            bool nodeJumpFound = false;

            for (int k = 0; k < node.jumpTrajectory.Count; k++)
            {
                nodeJumpFound = false;
                for (int j = 0; j < nodesNavmesh.Count; j++)
                {
                    if (nodesNavmesh[j] == node)
                        continue;

                    if(startPosY >= nodesNavmesh[j].transform.position.y)
                    {
                        // Optimisable avec SqrMagnitude && on check qu'on ne peut pas y accéder a pied à ce node
                        if (Vector3.Distance(node.jumpTrajectory[k], nodesNavmesh[j].transform.position) < agentSize && !node.ContainNode(nodesNavmesh[j], node))
                        {
                            // On check qu'il n'y ait pas de murs entre le point de départ et le point final
                             Vector2 direction = nodesNavmesh[j].transform.position - node.transform.position;
                             RaycastHit hit;
                             Physics.Raycast(node.transform.position + new Vector3(0, agentSize, 0), new Vector3(direction.x, 0, 0), out hit, Mathf.Abs(direction.x), wallLayerMask);
                             if (hit.collider != null)
                                 continue;

                             if(direction.y < 0) // On a le layer ground
                                 Physics.Raycast(node.transform.position + new Vector3(direction.x, agentSize, 0), new Vector3(0, direction.y, 0), out hit, Mathf.Abs(direction.y), groundLayerMask);
                             else  // Si on va vers le haut osef
                                 Physics.Raycast(node.transform.position + new Vector3(direction.x, agentSize, 0), new Vector3(0, direction.y, 0), out hit, Mathf.Abs(direction.y), wallLayerMask);

                             if (hit.collider != null)
                                 continue;

                            node.navmeshNodesJump.Add(nodesNavmesh[j]);
                            nodeJumpFound = true;
                            break;
                        }
                    }
                }
                if (nodeJumpFound == true)
                    break;
            }
        }


    }


}
