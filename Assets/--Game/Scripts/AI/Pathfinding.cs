using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum PathMovement
{
	Run,
	Fall,
	Jump,
	DoubleJump,
	WallJump,
	Wait
}

[System.Serializable]
public class PathNode
{
	public NavmeshNode Node;
	public float FScore; // Distance entre le noeud 
	public float GScore; //

	public PathNode PreviousPath;
	public PathMovement PreviousMovement;

	public PathNode(NavmeshNode node, float fscore, float gscore)
	{
		Node = node;
		FScore = fscore;
		GScore = gscore;
	}

	public PathNode(NavmeshNode node, float score, float gscore, PathNode previousPath, PathMovement previousMovement)
	{
		Node = node;
		FScore = score;
		GScore = gscore;

		PreviousPath = previousPath;
		PreviousMovement = previousMovement;
	}
}

public class Pathfinding : MonoBehaviour
{

	[SerializeField]
	float runWeight = 2;
	[SerializeField]
	float fallWeight = 3;
	[SerializeField]
	float jumpWeight = 1;

	List<PathNode> nodeExplored = new List<PathNode>();
	List<PathNode> nodeToExplore = new List<PathNode>();

	[Title("Debug")]
	[SerializeField]
	Transform debugEnd;
	[SerializeField]
	public List<PathNode> finalPath = new List<PathNode>();

	public bool debug = true;

	NavmeshNode position;
	public NavmeshNode Position
	{
		get { return position; }
	}

	NavmeshNode destination;
	public NavmeshNode Destination
	{
		get { return destination; }
	}



	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		//CalculatePath(this.transform.position, debugEnd.transform.position);
	}

	public void CalculatePath(Vector3 pos, Vector3 dest)
	{
		position = FindNearest(pos);
		destination = FindNearest(dest);

		if (position == destination)
			return;

		nodeExplored.Clear();
		nodeToExplore.Clear();

		NavmeshNode currentNode = position;
		PathNode currentPath = new PathNode(currentNode, 0, 0);
		int timeOut = 0;
		int maxTimeOut = 200;
		while (currentNode != destination)
		{
			CalculateNodeHeuristic(currentNode, position.transform.position, destination.transform.position, currentPath);
			nodeExplored.Add(currentPath);
			nodeToExplore.Remove(currentPath);
			if (nodeToExplore.Count == 0)
			{
				// Oups aucun noeud a exploré on explose
				Debug.Log("Aucun chemin trouvé oups");
				return;
			}
			currentPath = FindSmallestHeuristic();
			currentNode = currentPath.Node;

			timeOut += 1;
			if(timeOut == maxTimeOut)
			{
				Debug.Log("Time out path");
				this.gameObject.SetActive(false);
				return;
			}
		}
		ReconstitutePath(currentPath, position);
	}

	private void ReconstitutePath(PathNode currentPath, NavmeshNode start)
	{
		finalPath.Clear();
		for (int i = 0; i < Navmesh2D.Instance.nodesNavmesh.Count; i++)
		{
			//Navmesh2D.Instance.nodesNavmesh[i].gameObject.SetActive(true);
			if(debug)
				Navmesh2D.Instance.nodesNavmesh[i].text.text = " ";
		}

		int timeOut = 0;
		int maxTimeOut = 200;
		while (currentPath.Node != start)
		{
			finalPath.Add(currentPath);
			if (debug)
				currentPath.Node.text.text = ".";
			currentPath = currentPath.PreviousPath;

			timeOut += 1;
			if (timeOut == maxTimeOut)
			{
				Debug.Log("Time out resolution Path");
				this.gameObject.SetActive(false);
				return;
			}
		}
		/*finalPath.Add(currentPath);
		currentPath.Node.text.text = ".";*/

		finalPath.Reverse();
	}

	private void CalculateNodeHeuristic(NavmeshNode node, Vector3 pos, Vector3 dest, PathNode parentNode)
	{
		for (int i = 0; i < node.navmeshNodesRun.Count; i++)
		{
			Heuristic(node.navmeshNodesRun[i], pos, dest, PathMovement.Run, parentNode);
		}

		for (int i = 0; i < node.navmeshNodesFall.Count; i++)
		{
			Heuristic(node.navmeshNodesFall[i], pos, dest, PathMovement.Fall, parentNode);
		}

		for (int i = 0; i < node.navmeshNodesJump.Count; i++)
		{
			Heuristic(node.navmeshNodesJump[i], pos, dest, PathMovement.Jump, parentNode);
		}
	}

	/*public void Heuristic(NavmeshNode node, Vector3 pos, Vector3 dest, PathMovement pathMovement, PathNode parentNode)
	{
		if (ContainsNode(nodeToExplore, node))
			return;
		if (ContainsNode(nodeExplored, node))
			return;
		float score = 0;
		score += Vector3.SqrMagnitude(node.transform.position - pos);
		score += Vector3.SqrMagnitude(node.transform.position - dest);
		nodeToExplore.Add(new PathNode(node, score, parentNode, pathMovement));

	}*/


	private void Heuristic(NavmeshNode node, Vector3 pos, Vector3 dest, PathMovement pathMovement, PathNode parentNode)
	{
		float gscore = parentNode.GScore;
		switch (pathMovement)
		{
			case PathMovement.Run:
				gscore += runWeight;
				break;
			case PathMovement.Fall:
				gscore += fallWeight;
				break;
			case PathMovement.Jump:
				gscore += jumpWeight;
				break;
		}
		gscore += Vector3.SqrMagnitude(node.transform.position - parentNode.Node.transform.position);
		float fscore = gscore + Vector3.SqrMagnitude(node.transform.position - dest);

		PathNode path = null;
		path = ContainsNode(nodeToExplore, node);
		if (path != null)
		{
			if(path.GScore > gscore)
			{
				path.GScore = gscore;
				path.FScore = fscore;
				path.PreviousPath = parentNode;
				path.PreviousMovement = pathMovement;
			}
			return;
		}
		path = ContainsNode(nodeExplored, node);
		if (path != null)
		{
			if (path.GScore > gscore)
			{
				path.GScore = gscore;
				path.FScore = fscore;
				path.PreviousPath = parentNode;
				path.PreviousMovement = pathMovement;
			}
			return;
		}

		nodeToExplore.Add(new PathNode(node, fscore, gscore, parentNode, pathMovement));
	}




	public NavmeshNode FindNearest(Vector3 pos)
	{
		float bestDistance = 999999;
		int bestIndex = -1;
		float distance = 0;
		for (int i = 0; i < Navmesh2D.Instance.nodesNavmesh.Count; i++)
		{
			distance = Vector3.SqrMagnitude(Navmesh2D.Instance.nodesNavmesh[i].transform.position - pos);
			if(distance < bestDistance)
			{
				bestDistance = distance;
				bestIndex = i;
			}
		}
		if (bestIndex == -1)
		{
			Debug.Log("Wesh c'est bizarre");
			return null;
		}
		return Navmesh2D.Instance.nodesNavmesh[bestIndex];
	}

	private PathNode FindSmallestHeuristic()
	{
		float bestScore = 999999;
		int bestIndex = -1;
		float score = 0;
		for (int i = 0; i < nodeToExplore.Count; i++)
		{
			score = nodeToExplore[i].FScore;
			if (score < bestScore)
			{
				bestScore = score;
				bestIndex = i;
			}
		}
		if (bestIndex == -1)
		{
			Debug.Log("Wesh !?");
			return null;
		}
		return nodeToExplore[bestIndex];
	}

	private PathNode ContainsNode(List<PathNode> pathNodes, NavmeshNode nodeToSearch)
	{
		for (int i = 0; i < pathNodes.Count; i++)
		{
			if (pathNodes[i].Node == nodeToSearch)
				return pathNodes[i];
		}
		return null;
	}
}
