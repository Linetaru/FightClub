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

    NavmeshNode[,] nodes;

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

    [Button]
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
                }
            }
        }
    }

    private void CalculateRunNodes(NavmeshNode node, int nodeX, int nodeY)
    {
        if (nodes[nodeX - 1, nodeY] != null)
            node.navmeshNodesRun.Add(nodes[nodeX - 1, nodeY]);
        else if (nodes[nodeX + 1, nodeY] != null)
            node.navmeshNodesRun.Add(nodes[nodeX + 1, nodeY]);
    }

    private void CalculateFallNodes(NavmeshNode node, int nodeX, int nodeY)
    {
        if (nodes[nodeX - 1, nodeY] == null)
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
        }

    }
    private void CalculateJumpNodes()
    {

    }

}
