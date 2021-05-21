using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SmearsController : MonoBehaviour
{
	[SerializeField]
	Animator animator;
	[SerializeField]
	AnimationClip animationClip;
	[SerializeField]
	MeshRenderer meshRenderer;

	[OnValueChanged("Test")]
	[SerializeField]
	int frame = 0;

	[SerializeField]
	Vector2 textureTiling;
	[SerializeField]
	Vector3 rotation;

	[SerializeField]
	List<Vector2> textureTilling = new List<Vector2>();
	/*[SerializeField]
	List<Vector3> rotation = new List<Vector3>();*/

	[Button]
	public void Test()
	{
		animationClip.SampleAnimation(this.gameObject, frame/60f);
		//meshRenderer.material.mainTextureScale
	}

	[Button]
	public void Register()
	{
		textureTilling.Insert(frame, meshRenderer.material.mainTextureScale);
		animationClip.SampleAnimation(this.gameObject, frame / 60f);
		//meshRenderer.material.mainTextureScale
	}

}
