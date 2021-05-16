using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class OutsideScreenCursor : MonoBehaviour
{
	[Title("UI")]
	[SerializeField]
	RectTransform cursorTransform;
	[SerializeField]
	RectTransform pivotArrow;
	[SerializeField]
	RectTransform blastZoneFill;

	CameraZoomController cam;
	Transform focus;
	Bounds blastZones;

	Vector2 viewportPos;
	Vector2 viewportDirection;
	//Vector2 center;



	public void Initialize(Transform character, Bounds blastZoneBounds, CameraZoomController camera)
	{
		focus = character;
		blastZones = blastZoneBounds;
		cam = camera;

		//center = Vector2.one * 0.5f;
	}


	// Update is called once per frame
	void Update()
	{
		if (focus == null)
			return;
		viewportPos = cam.Camera.WorldToViewportPoint(focus.transform.position);
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
		DrawBlastZoneFilled(viewportDirection);
	}


	private void DrawBlastZoneFilled(Vector2 viewportDir)
	{
		if (blastZones == null)
			return;

		// Calcule la position en world space du edge
		Vector3 edgePosition = cam.CameraView.center + new Vector3(cam.CameraView.extents.x * viewportDir.x, cam.CameraView.extents.y * viewportDir.y, 0);


		float pivotX = 0.5f;
		float pivotY = 0.5f;
		if (viewportDir.y == 1)
			pivotY = 0;
		else if (viewportDir.y == -1)
			pivotY = 1;

		if (viewportDir.x == 1)
			pivotX = 0;
		else if (viewportDir.x == -1)
			pivotX = 1;

		blastZoneFill.anchorMin = new Vector2(pivotX, pivotY);
		blastZoneFill.anchorMax = new Vector2(pivotX, pivotY);
		blastZoneFill.pivot = new Vector2(pivotX, pivotY);

		float ratioX = 1f;
		float ratioY = 1f;
		if (pivotX != 0.5f)
		{
			float distanceToBlastZoneX = Mathf.Abs(focus.transform.position.x - edgePosition.x);
			float distanceMaxX = Mathf.Abs(blastZones.center.x + (blastZones.extents.x * viewportDir.x) - edgePosition.x);
			ratioX = distanceToBlastZoneX / distanceMaxX;
		}

		if (pivotY != 0.5f)
		{
			float distanceToBlastZoneY = Mathf.Abs(focus.transform.position.y - edgePosition.y);
			float distanceMaxY = Mathf.Abs(blastZones.center.y + (blastZones.extents.y * viewportDir.y) - edgePosition.y);
			ratioY = distanceToBlastZoneY / distanceMaxY;
		}

		blastZoneFill.localScale = new Vector3(ratioX, ratioY, 1);
	}
}
