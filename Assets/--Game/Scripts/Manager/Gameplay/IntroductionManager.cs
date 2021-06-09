using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class IntroductionManager : MonoBehaviour, IControllable
{
	[Title("Logic")]
	[SerializeField]
	GameData gameData;

	[Title("UI")]
	[SerializeField]
	RectTransform[] cutInLayout;
	[SerializeField]
	RawImage[] cutInRenderer;


	[SerializeField]
	RenderTexture[] renderTextures;

	[Title("Feedbacks")]
	[SerializeField]
	float timeCutInTotal = 3;
	[SerializeField]
	float timeCutIn = 0.66f;
	[SerializeField]
	Animator animatorText2;
	[SerializeField]
	Animator animatorTransitionToBattle;


	BattleManager battleManager;
	public Color[] teamColors;

	bool active = false;
	List<CharacterBase> characters = new List<CharacterBase>();


	private void Start()
	{
		battleManager = BattleManager.Instance;
		if (battleManager.gameData.GameSetting.SkipIntro)
			this.gameObject.SetActive(false);
	}


	// Update is called once per frame
	public void UpdateControl(int id, Input_Info input)
	{
		if(active == true)
		{
			if (input.CheckAction(0, InputConst.Attack))
			{
				SkipIntro();
			}
		}
	}



	// Appelé par l'event Battle Manager
	public void StartIntroduction(CharacterBase character)
	{
		if(battleManager == null)
			battleManager = BattleManager.Instance;

		characters.Add(character);
		if(characters.Count == gameData.CharacterInfos.Count)
		{
			CreateCutInLayout(gameData.CharacterInfos.Count);
			battleManager.SetMenuControllable(this);
			active = true;
		}
	}

	public void CreateCutInLayout(int characterNumber)
	{
		float padding = 1f / characterNumber;
		for (int i = 0; i < characterNumber; i++)
		{
			cutInLayout[i].anchorMin = new Vector2((padding * i) - 0.25f, -0.2f);
			cutInLayout[i].anchorMax = new Vector2((padding * i) + padding, 1.2f);
			cutInRenderer[i].texture = renderTextures[i];
		}

		cutInLayout[characterNumber-1].anchorMax += new Vector2(0.25f, 0f);
		cutInRenderer[characterNumber - 1].rectTransform.anchoredPosition = Vector2.zero;
	}


	public void CutInAppear()
	{
		if (active == false)
			return;
		StartCoroutine(CutInAppearCoroutine(gameData.CharacterInfos.Count));
	}

	private IEnumerator CutInAppearCoroutine(int characterNumber)
	{
		animatorText2.gameObject.SetActive(true);
		float timeInterval = (timeCutInTotal - (timeCutIn * characterNumber)) / characterNumber;
		for (int i = 0; i < characterNumber; i++)
		{
			cutInLayout[i].gameObject.SetActive(true);
			int randIntro = Random.Range(0, gameData.CharacterInfos[i].CharacterData.introductions.Length);
			IntroductionCharacter intro = Instantiate(gameData.CharacterInfos[i].CharacterData.introductions[randIntro], characters[i].transform.position, Quaternion.identity, this.transform);
			intro.StartIntroduction(characters[i], renderTextures[i]);
			yield return new WaitForSeconds(timeCutIn);
			cutInRenderer[i].color = teamColors[(int)characters[i].TeamID];
			yield return new WaitForSeconds(timeInterval);
			intro.gameObject.SetActive(false);
		}
		yield return new WaitForSeconds(0.5f);
		active = false;
		StartCoroutine(SkipIntroductionCoroutine());
	}

	private IEnumerator SkipIntroductionCoroutine()
	{
		animatorTransitionToBattle.SetTrigger("Feedback");
		yield return new WaitForSeconds(1f);
		EndIntroduction();
	}

	public void SkipIntro()
	{
		active = false;
		StopAllCoroutines();
		StartCoroutine(SkipIntroductionCoroutine());
	}


	public void EndIntroduction()
	{
		battleManager.SetBattleControllable();
	}
}
