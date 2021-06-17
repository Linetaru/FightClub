using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class SignatureMoveManager : MonoBehaviour
{

	BattleManager battleManager = null;
	CameraZoomController cameraManager = null;

	[SerializeField]
	Transform centerStage = null;

	[Title("Data")]
	[SerializeField]
	GameData gameData = null;

	[Title("UI")]
	[SerializeField]
	Animator feedback = null;
	[SerializeField]
	TextMeshProUGUI characterName = null;
	[SerializeField]
	Image characterFace = null;

	[Title("Feedback")]
	[SerializeField]
	ParticleSystem particleSystem = null;

	[Title("Sounds")]
	[SerializeField]
	AK.Wwise.Event announcerVoice = null;
	[SerializeField]
	AK.Wwise.Event eventMixOn = null;
	[SerializeField]
	AK.Wwise.Event eventMixOff = null;

	TargetsCamera target = null;


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
		AkSoundEngine.PostEvent(announcerVoice.Id, this.gameObject);
		if (battleManager == null)
		{
			battleManager = BattleManager.Instance;
			cameraManager = BattleManager.Instance.cameraController;
		}

		feedback.gameObject.SetActive(true);
		characterName.text = gameData.CharacterInfos[user.PlayerID].CharacterData.characterName;
		characterFace.sprite = gameData.CharacterInfos[user.PlayerID].CharacterData.characterFace;

		ParticleSystem particle = Instantiate(particleSystem, user.CenterPoint.position, Quaternion.identity);
		Destroy(particle.gameObject, 1f);

		for (int i = 0; i < battleManager.characterAlive.Count; i++)
		{
			battleManager.characterAlive[i].SetMotionSpeed(0, 1);
		}
		target = new TargetsCamera(user.transform, 2);
		cameraManager.targets.Add(target);

		StartCoroutine(SignatureMoveFeedback());
	}

	private IEnumerator SignatureMoveFeedback()
	{
		yield return new WaitForSeconds(1f);
		cameraManager.targets.Remove(target);
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
		AkSoundEngine.PostEvent(eventMixOn.Id, this.gameObject);
		move.StartSignatureMove(user, target);
		move.OnEnd += EndSignatureMove;


	}

	public void EndSignatureMove()
	{
		for (int i = 0; i < battleManager.characterAlive.Count; i++)
		{
			battleManager.characterAlive[i].SetMotionSpeed(0, 0.1f);
		}
		AkSoundEngine.PostEvent(eventMixOff.Id, this.gameObject);
		//Destroy(currentMove);
	}
}
