﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
	[Expandable]
	public GameData gameData;

	public InputController inputController;

	public GameObject[] spawningPoint;

	public List<CharacterBase> characterAlive;

	public List<CharacterBase> characterFullDead;

	//public List<GameObject> canvasPanelPlayer;
	//public List<TextMeshProUGUI> canvasPercentPlayer;

	public CameraController cameraController;

	public CharacterUI[] characterUi;

	public PackageCreator.Event.GameEventFloat[] gameEventFloats;

	public bool isGameStarted;

	// Start is called before the first frame update
	void Start()
	{
		SpawnPlayer();
	}

	// Update is called once per frame
	void Update()
	{
		isFinish();
	}

	public void SpawnPlayer()
    {
		for(int i = 0; i < gameData.CharacterInfos.Count; i++)
        {
			GameObject go = Instantiate(gameData.CharacterInfos[i].CharacterData.playerPrefab, spawningPoint[i].transform.position, Quaternion.identity);
			go.name = gameData.CharacterInfos[i].CharacterData.playerPrefab.name;
			go.tag = "Player" + (i + 1);
			CharacterBase user = go.GetComponent<CharacterBase>();
			user.Model.tag = "Player" + (i + 1);
			inputController.controllable[i] = user;
			characterAlive.Add(user);
			user.Stats.GameData = gameData;
			user.Stats.gameEvent = gameEventFloats[i];
			user.Stats.InitStats();
			if(characterUi.Length != 0)
				characterUi[i].InitPlayerPanel(user);
			cameraController.playersTarget.Add(go);
		}
		isGameStarted = true;
	}

	public void isFinish()
    {
		if(characterAlive.Count == 1 && isGameStarted)
        {
			UnityEngine.SceneManagement.SceneManager.LoadScene("GP_Menu");
        }
    }
}