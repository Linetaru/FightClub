﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class OutsideScreenCursor : MonoBehaviour
{
	[Title("Logic")]
	[SerializeField]
	Transform character;

	[Title("UI")]
	[SerializeField]
	RectTransform cursorTransform;
	[SerializeField]
	RectTransform pivotArrow;

	Camera cam;
	Vector2 viewportPos;
	Vector2 viewportDirection;
	Vector2 center;

	// Start is called before the first frame update
	void Start()
	{
		cam = Camera.main.GetComponent<Camera>();
		center = Vector2.one * 0.5f;
	}

	// Update is called once per frame
	void Update()
	{
		if (character == null)
			return;
		viewportPos = cam.WorldToViewportPoint(character.transform.position);
		if (viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1)
		{
			cursorTransform.localScale = Vector3.one;
		}
		else
		{
			cursorTransform.localScale = Vector3.zero;
			return;
		}


		//viewportDirection = viewportPos - center;

		float viewportX = Mathf.Clamp(viewportPos.x, 0, 1);
		float viewportY = Mathf.Clamp(viewportPos.y, 0, 1);
		cursorTransform.anchorMin = new Vector2(viewportX, viewportY);
		cursorTransform.anchorMax = new Vector2(viewportX, viewportY);


		viewportDirection = Vector2.zero;
		if (viewportPos.x < 0)
			viewportDirection.x = -1;
		else if (viewportPos.x > 1)
			viewportDirection.x = 1;
		else
			viewportDirection.x = 0;

		if (viewportPos.y < 0)
			viewportDirection.y = -1;
		else if (viewportPos.y > 1)
			viewportDirection.y = 1;
		else
			viewportDirection.y = 0;


		cursorTransform.anchoredPosition = -cursorTransform.sizeDelta * viewportDirection;
		//cursorTransform.anchoredPosition *= new Vector2(Mathf.Sign(viewportDirection.x), Mathf.Sign(viewportDirection.y));

		pivotArrow.transform.localEulerAngles = new Vector3(0, 0, -Vector2.SignedAngle(viewportDirection, Vector2.down));

	}
}
