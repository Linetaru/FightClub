using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationToCamera : MonoBehaviour
{
	[SerializeField]
	Vector3 localPos;

	[SerializeField]
	Camera mainCamera;

	// Start is called before the first frame update
	void Start()
	{
		//mainCamera = Camera.main;
	}

	// Update is called once per frame
	void Update()
	{
		this.transform.localPosition = localPos;
		this.transform.rotation = mainCamera.transform.rotation;
	}
}
