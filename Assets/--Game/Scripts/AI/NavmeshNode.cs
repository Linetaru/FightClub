using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class NavmeshNode : MonoBehaviour
{
	[SerializeField]
	public List<NavmeshNode>/*[]*/ navmeshNodesRun;

	[SerializeField]
	public List<NavmeshNode>/*[]*/ navmeshNodesJump;


	[SerializeField]
	public List<NavmeshNode>/*[]*/ navmeshNodesFall;



	[Title("Debug")]
	[SerializeField]
	public float debugJumpHeight = 14;
	[SerializeField]
	public float debugAerialSpeed = 8;
	[SerializeField]
	public float debugGravity = 35;

	[Space]
	[SerializeField]
	public float debugNbTrajectory = 1;
	[SerializeField]
	public int debugJumpPointInterval = 4;
	[SerializeField]
	public int debugMaxNbOfPoint = 100;

	[Space]
	[SerializeField]
	public LayerMask debugLayerMask;
	[SerializeField]
	public float debugCheckZ = 1;

	public bool debugDraw = false;

	// Pour le draw gizmo
	List<Vector3> jumpTrajectoryVisual = new List<Vector3>();
	// Pour les datas
	public List<Vector3> jumpTrajectory = new List<Vector3>();

	[Button]
	private void VisualizeJumpTrajectory()
	{
		jumpTrajectoryVisual.Clear();
		for (int i = (int)-debugNbTrajectory; i <= debugNbTrajectory; i++)
		{
			CalculateJumpTrajectory(debugJumpHeight, debugAerialSpeed * (i / debugNbTrajectory), debugGravity, debugMaxNbOfPoint, debugJumpPointInterval, debugCheckZ, debugLayerMask);
		}
	}

	[Button]
	private void ClearJumpTrajectory()
	{
		jumpTrajectory.Clear();
		jumpTrajectoryVisual.Clear();
	}

	public void CalculateJumpTrajectory(float jumpHeight, float aerialSpeed, float gravity, int maxNbOfPoint, int jumpPointInterval, float checkZ, LayerMask groundLayerMask)
	{
		jumpTrajectory.Clear();
		float deltaTime = 0.016f;
		float speedX = 0;
		float speedY = 0;
		int nbOfPoint = 0;
		RaycastHit hitInWall;

		Vector3 startPos = this.transform.position;

		speedX = aerialSpeed;
		speedY = jumpHeight;
		nbOfPoint = 0;
		startPos = this.transform.position;
		jumpTrajectoryVisual.Add(startPos);

		while (nbOfPoint < maxNbOfPoint)
		{
			startPos += new Vector3(speedX * deltaTime, speedY * deltaTime);
			nbOfPoint += 1;
			if (nbOfPoint % jumpPointInterval == 0)
			{
				Physics.Raycast(startPos - new Vector3(0, 0, checkZ), Vector3.forward, out hitInWall, checkZ, groundLayerMask);
				if (hitInWall.collider != null)
				{
					nbOfPoint = maxNbOfPoint;
				}
				else
				{
					if (speedY <= 0)
						jumpTrajectory.Add(startPos);
					jumpTrajectoryVisual.Add(startPos);
				}
			}
			speedY -= gravity * deltaTime;
			speedY = Mathf.Max(speedY, -20);
		}
	}





	// On part du principe qu'il n'y a pas de boucle
	public bool ContainNode(NavmeshNode nodeToSearch, NavmeshNode previousNode)
	{
		if (navmeshNodesRun.Contains(nodeToSearch))
		{
			return true;
		}
		for (int i = 0; i < navmeshNodesRun.Count; i++)
		{
			if (navmeshNodesRun[i] != previousNode)
				return navmeshNodesRun[i].ContainNode(nodeToSearch, this);
		}
		return false;
	}



	private void OnDrawGizmos()
	{
		// NavmeshRun
		Gizmos.color = Color.black;
		for (int i = 0; i < navmeshNodesRun.Count; i++)
		{
			Gizmos.DrawLine(this.transform.position, navmeshNodesRun[i].transform.position);
		}

		// NavmeshFall
		Gizmos.color = Color.blue;
		for (int i = 0; i < navmeshNodesFall.Count; i++)
		{
			Gizmos.DrawLine(this.transform.position, navmeshNodesFall[i].transform.position);
		}

		// NavmeshJump
		Gizmos.color = Color.yellow;
		for (int i = 0; i < navmeshNodesJump.Count; i++)
		{
			Gizmos.DrawLine(this.transform.position, navmeshNodesJump[i].transform.position);
		}

	}

	private void OnDrawGizmosSelected()
	{
		if (debugDraw == false)
			return;
		Gizmos.color = Color.green;
		for (int i = 1; i < jumpTrajectoryVisual.Count; i++)
		{
			Gizmos.DrawLine(jumpTrajectoryVisual[i - 1], jumpTrajectoryVisual[i]);
		}
	}
}

