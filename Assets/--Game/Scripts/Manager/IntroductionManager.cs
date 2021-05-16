using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroductionManager : MonoBehaviour
{
	[SerializeField]
	GameData gameData;

	[SerializeField]
	RectTransform[] cutInLayout;
	[SerializeField]
	RawImage[] cutInRenderer;


	[SerializeField]
	RenderTexture[] renderTextures;

	/*[SerializeField]
	IntroductionCharacter[] introductionCinematic;*/


	[SerializeField]
	float timeCutInTotal = 3;
	[SerializeField]
	float timeCutIn = 0.66f;
	[SerializeField]
	Animator animatorText2;
	[SerializeField]
	Animator animatorTransitionToBattle;

	public Color[] teamColors;

	bool active = false;
	List<CharacterBase> characters = new List<CharacterBase>();

	// Start is called before the first frame update
	void Start()
	{
		CreateCutInLayout(gameData.CharacterInfos.Count);
	}

	// Update is called once per frame
	void Update()
	{
		if(active == true)
		{

		}
	}

	// Appelé par l'event Battle Manager
	public void StartIntroduction(CharacterBase character)
	{
		characters.Add(character);
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
		animatorTransitionToBattle.SetTrigger("Feedback");
		yield return new WaitForSeconds(1f);
		EndIntroduction();
	}


	public void EndIntroduction()
	{
		
	}
}
