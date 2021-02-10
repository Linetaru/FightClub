using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusLevel : MonoBehaviour
{
	public float halfXBounds = 20f;
	public float halfYBounds = 15f;
	public float halfZBounds = 15f;

	[ReadOnly] public Bounds focusBounds;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 pos = gameObject.transform.position;
		Bounds bounds = new Bounds();
		bounds.Encapsulate(new Vector3(pos.x - halfXBounds, pos.y - halfYBounds, pos.z - halfZBounds));
		bounds.Encapsulate(new Vector3(pos.x + halfXBounds, pos.y + halfYBounds, pos.z + halfZBounds));
		focusBounds = bounds;
	}
}