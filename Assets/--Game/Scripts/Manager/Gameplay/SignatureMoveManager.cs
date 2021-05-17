using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SignatureMoveManager : MonoBehaviour
{

	[SerializeField]
	BattleManager battleManager;

	[SerializeField]
	CameraManager cameraManager;

	[SerializeField]
	Transform centerStage;


	[SerializeField]
	ParticleSystem particleSystem;


	[Title("UI")]
	[SerializeField]
	Animator feedback;

	TargetsCamera target;


	private static SignatureMoveManager _instance;
	public static SignatureMoveManager Instance { get { return _instance; } }

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else
		{
			Destroy(this);
		}
	}



	public void StartSignatureMove(CharacterBase user)
	{
		feedback.gameObject.SetActive(true);

		ParticleSystem particle = Instantiate(particleSystem, user.CenterPoint.position, Quaternion.identity);
		Destroy(particle.gameObject, 1f);

		for (int i = 0; i < battleManager.characterAlive.Count; i++)
		{
			battleManager.characterAlive[i].SetMotionSpeed(0, 1);
		}
		target = new TargetsCamera(user.transform, 2);
		cameraManager.zoomController.targets.Add(target);

		StartCoroutine(SignatureMoveFeedback());
	}

	private IEnumerator SignatureMoveFeedback()
	{
		yield return new WaitForSeconds(1f);
		cameraManager.zoomController.targets.Remove(target);
		feedback.gameObject.SetActive(false);
	}




	public void CreateSignatureMove(SignatureMove signatureMove, CharacterBase user, CharacterBase target)
	{
		for (int i = 0; i < battleManager.characterAlive.Count; i++)
		{
			battleManager.characterAlive[i].SetMotionSpeed(0, 10);
		}
		SignatureMove move = Instantiate(signatureMove, centerStage.transform);
		move.transform.localScale = new Vector3(user.Movement.Direction, 1, 1);
		move.StartSignatureMove(user, target);
		move.OnEnd += EndSignatureMove;


	}

	public void EndSignatureMove()
	{
		for (int i = 0; i < battleManager.characterAlive.Count; i++)
		{
			battleManager.characterAlive[i].SetMotionSpeed(0, 0.1f);
		}
		//Destroy(currentMove);
	}
}
