﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class OutsideScreenCursorManager : MonoBehaviour
{
	[SerializeField]
	BoxCollider blastZone;
	[SerializeField]
	CameraZoomController cam;

	[Title("Canvas")]
	[SerializeField]
	Transform parent;
	[SerializeField]
	OutsideScreenCursor prefab;



	List<OutsideScreenCursor> outsideScreenCursors = new List<OutsideScreenCursor>();


	public void InitializeCharacter(CharacterBase c)
	{
		OutsideScreenCursor cursor = Instantiate(prefab, parent);
		cursor.Initialize(c.CenterPoint, blastZone.bounds, cam);

		outsideScreenCursors.Add(cursor); 
		cursor.gameObject.SetActive(true);
	}
}