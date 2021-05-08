using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathMovement
{
	Run,
	Fall,
	Jump
}

public class PathNode
{
	public NavmeshNode Node;
	public float Score;

	public PathNode PreviousPath;
	public PathMovement PreviousMovement;

	public PathNode(NavmeshNode node, float score)
	{
		Node = node;
		Score = score;
	}

	public PathNode(NavmeshNode node, float score, PathNode previousPath, PathMovement previousMovement)
	{
		Node = node;
		Score = score;

		PreviousPath = previousPath;
		PreviousMovement = previousMovement;
	}
}

public class Pathfinding : MonoBehaviour
{

	[SerializeField]
	Transform debugEnd;

	[SerializeField]
	float runWeight = 2;
	[SerializeField]
	float fallWeight = 3;
	[SerializeField]
	float jumpWeight = 1;

	List<PathNode> nodeExplored = new List<PathNode>();
	List<PathNode> nodeToExplore = new List<PathNode>();

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		CalculatePath(this.transform.position, debugEnd.transform.position);
	}

	public void CalculatePath(Vector3 pos, Vector3 dest)
	{
		NavmeshNode position = FindNearest(pos);
		NavmeshNode destination = FindNearest(dest);

		if (position == destination)
			return;

		nodeExplored.Clear();
		nodeToExplore.Clear();

		NavmeshNode currentNode = position;
		PathNode currentPath = new PathNode(currentNode, 0);
		int timeOut = 0;
		int maxTimeOut = 200;
		while (currentNode != destination)
		{
			CalculateNodeHeuristic(currentNode, pos, dest, currentPath);
			nodeExplored.Add(currentPath);
			nodeToExplore.Remove(currentPath);
			if (nodeToExplore.Count == 0)
			{
				// Oups aucun noeud a exploré on explose
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

	public void ReconstitutePath(PathNode currentPath, NavmeshNode start)
	{
		for (int i = 0; i < Navmesh2D.Instance.nodesNavmesh.Count; i++)
		{
			Navmesh2D.Instance.nodesNavmesh[i].gameObject.SetActive(true);
		}

		int timeOut = 0;
		int maxTimeOut = 200;
		while (currentPath.Node != start)
		{
			currentPath.Node.gameObject.SetActive(false);
			currentPath = currentPath.PreviousPath;


			timeOut += 1;
			if (timeOut == maxTimeOut)
			{
				Debug.Log("Time out resolution Path");
				this.gameObject.SetActive(false);
				return;
			}
		}
		currentPath.Node.gameObject.SetActive(false);
	}

	public void CalculateNodeHeuristic(NavmeshNode node, Vector3 pos, Vector3 dest, PathNode parentNode)
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

	public void Heuristic(NavmeshNode node, Vector3 pos, Vector3 dest, PathMovement pathMovement, PathNode parentNode)
	{
		if (ContainsNode(nodeToExplore, node))
			return;
		if (ContainsNode(nodeExplored, node))
			return;
		float score = 0;
		score += Vector3.SqrMagnitude(node.transform.position - pos);
		score += Vector3.SqrMagnitude(node.transform.position - dest);
		nodeToExplore.Add(new PathNode(node, score, parentNode, pathMovement));

		/*float score = 0;
		switch(path)
		{
			case PathMovement.Run:
				score += runWeight;
				break;
			case PathMovement.Fall:
				score += fallWeight;
				break;
			case PathMovement.Jump:
				score += jumpWeight;
				break;
		}
		score += Vector3.SqrMagnitude(node.transform.position - pos);
		score += Vector3.SqrMagnitude(node.transform.position - dest);

		if (dictPath.ContainsKey())*/
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

	public PathNode FindSmallestHeuristic()
	{
		float bestScore = 999999;
		int bestIndex = -1;
		float score = 0;
		for (int i = 0; i < nodeToExplore.Count; i++)
		{
			score = nodeToExplore[i].Score;
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

	public bool ContainsNode(List<PathNode> pathNodes, NavmeshNode nodeToSearch)
	{
		for (int i = 0; i < pathNodes.Count; i++)
		{
			if (pathNodes[i].Node == nodeToSearch)
				return true;
		}
		return false;
	}
}
