using System.Collections;
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

	public List<GameObject> canvasPanelPlayer;
	public List<TextMeshProUGUI> canvasPercentPlayer;

	public CameraController cameraController;

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
			CharacterBase user = go.GetComponent<CharacterBase>();
			inputController.controllable[i] = user;
			characterAlive.Add(user);
			user.Stats.GameData = gameData;
			user.Stats.InitStats();
			user.Ui.InitPlayerPanel(user, canvasPanelPlayer[i], canvasPercentPlayer[i]);
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