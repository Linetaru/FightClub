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
	float jumpHeight = 10;
	[SerializeField]
	float aerialSpeed = 10;
	[SerializeField]
	float gravity = 10;

	[Button]
	private void CalculateJumpTrajectory()
	{

	}




	private void OnDrawGizmos()
	{
		// NavmeshRun
		Gizmos.color = Color.black;
		for (int i = 0; i < navmeshNodesRun.Count; i++)
		{
			Gizmos.DrawLine(this.transform.position, navmeshNodesRun[i].transform.position);
		}

		Gizmos.color = Color.blue;
		for (int i = 0; i < navmeshNodesFall.Count; i++)
		{
			Gizmos.DrawLine(this.transform.position, navmeshNodesFall[i].transform.position);
		}

		Gizmos.color = Color.yellow;
		for (int i = 0; i < navmeshNodesJump.Count; i++)
		{
			Gizmos.DrawLine(this.transform.position, navmeshNodesJump[i].transform.position);
		}
	}
}

