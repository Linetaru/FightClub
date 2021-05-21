using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TestCamera : MonoBehaviour
{
	private List<Transform> targets = new List<Transform>();
	public List<Transform> Targets
	{
		get { return targets; }
		set { targets = value; }
	}

	[SerializeField]
	BoxCollider stageBounds;

	[Title("Camera Offset")]
	[SerializeField]
	Vector3 offsetPos = Vector3.zero;
	[SerializeField]
	Vector3 offsetRot = Vector3.zero;

	[Title("Zoom Z")]
	[SerializeField]
	float minZoom = 8;

	[Title("Distance To Start Zoom")]
	[SerializeField]
	Vector2 minDistance = Vector3.zero;



	[Title("Zoom Smooth Time")]
	[SerializeField]
	float zoomSmoothTime = 0.6f;
	[SerializeField]
	float dezoomSmoothTime = 0.2f;
	[SerializeField]
	float rotationSmoothTime = 1f;

	Camera cam;
	float previousSizeZoom;
	Bounds targetsBounds;
	Bounds cameraBounds;

	Vector3 velocity;

	public void SetStageBounds(BoxCollider newBounds)
	{
		stageBounds = newBounds;
	}


	public void AddCharacter(CharacterBase character)
	{
		targets.Add(character.transform);
	}

	private void Start()
	{
		cam = GetComponent<Camera>();
	}

	private void Update()
	{
		if (targets.Count == 0)
			return;

		// On calcule le point de focus
		targetsBounds = GetNewBoundsEncapsulate(targets);

		// On calcule la distance Z pour que tout le monde soit cadré
		// On a un ratio différent pour le X et le Y puisque l'écran est rectangle en horizontal, donc la camera doit dezoomer plus vite en Y
		float maxZoom = stageBounds.bounds.size.magnitude * 0.5f;

		float ratioY = (targetsBounds.size.y - minDistance.y) / (stageBounds.bounds.size.y - minDistance.y);
		float ratioX = (targetsBounds.size.x - minDistance.x) / (stageBounds.bounds.size.x - minDistance.x);
		float bestRatio = Mathf.Clamp(Mathf.Max(ratioX, ratioY), 0, 1);

		float sizeZoom = Mathf.Lerp(minZoom, maxZoom, bestRatio);

		/*float sizeZoom = targetsBounds.size.magnitude;
		sizeZoom = Mathf.Clamp(sizeZoom, minZoom, stageBounds.bounds.size.magnitude*0.5f);*/

		cameraBounds = CalculateCameraViewBounds(ref sizeZoom, targetsBounds.center);



		// Ensuite on décale la view pour que cette dernière soit bien a l'intérieur du Collider
		float offsetX = 0;
		if (cameraBounds.min.x < stageBounds.bounds.min.x)
			offsetX = stageBounds.bounds.min.x - cameraBounds.min.x;
		else if (cameraBounds.max.x > stageBounds.bounds.max.x)
			offsetX = stageBounds.bounds.max.x - cameraBounds.max.x;

		float offsetY = 0;
		if (cameraBounds.min.y < stageBounds.bounds.min.y)
			offsetY = stageBounds.bounds.min.y - cameraBounds.min.y;
		else if (cameraBounds.max.y > stageBounds.bounds.max.y)
			offsetY = stageBounds.bounds.max.y - cameraBounds.max.y;


		Vector3 finalPosition = new Vector3(targetsBounds.center.x + offsetX, targetsBounds.center.y + offsetY, targetsBounds.center.z - sizeZoom);

		// Différent smoothTime en fonction de si on zoom ou on dezoom
		float smoothTime = 0f;
		if (previousSizeZoom < sizeZoom)
			smoothTime = dezoomSmoothTime;
		else
			smoothTime = zoomSmoothTime;
		previousSizeZoom = sizeZoom;
		this.transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref velocity, smoothTime);
		this.transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(offsetRot), rotationSmoothTime);
	}





	//Get new bounds of the first target and encapsule all targets in the bounds
	private Bounds GetNewBoundsEncapsulate(List<Transform> focusTargets)
	{
		Bounds bounds = new Bounds(focusTargets[0].position + offsetPos, Vector3.zero);

		for (int i = 0; i < focusTargets.Count; i++)
		{
			bounds.Encapsulate(focusTargets[i].position + offsetPos);
		}
		return bounds;
	}


	// On passe distance en ref pour que le zoom en Z soit conforme avec la vue de la camera
	private Bounds CalculateCameraViewBounds(ref float distance, Vector3 center)
	{
		float frustumHeight = 2.0f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
		frustumHeight = Mathf.Clamp(frustumHeight, 0, stageBounds.size.y);

		distance = frustumHeight * 0.5f / Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);

		float frustumWidth = frustumHeight * cam.aspect;

		return new Bounds(center, new Vector3(frustumWidth, frustumHeight, 0));

	}







	private IEnumerator rotationImpulseCoroutine;
	public void CameraRotationImpulse(Vector2 impulse, float time)
	{
		if (rotationImpulseCoroutine != null)
			StopCoroutine(rotationImpulseCoroutine);
		rotationImpulseCoroutine = RotationImpulseCoroutine(impulse, time);
		StartCoroutine(rotationImpulseCoroutine);
	}

	private IEnumerator RotationImpulseCoroutine(Vector2 impulse, float time)
	{
		float t = 0f;
		Quaternion intialRot = this.transform.localRotation;
		Quaternion finalRot = Quaternion.Euler(impulse.x + offsetRot.x, impulse.y + offsetRot.y, 0);
		while (t < time)
		{
			t += Time.deltaTime;
			this.transform.localRotation = Quaternion.Lerp(intialRot, finalRot, t / time);
			yield return null;
		}
	}








}
