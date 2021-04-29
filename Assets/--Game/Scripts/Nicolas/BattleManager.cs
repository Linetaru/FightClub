using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class BattleManager : MonoBehaviour
{
	[Title("Data")]
	[Expandable]
	public GameData gameData;

	[Title("Game Mode Managers")]
	public GameObject BombModeManager;

	[Title("Events")]
	public PackageCreator.Event.GameEventCharacter uiEvent;

	[Title("Interractions")]
	public InputController inputController;

	public CameraZoomController cameraController;

	[Title("Composants")]
	public GameObject[] spawningPoint;

	public PackageCreator.Event.GameEventUICharacter[] gameEventUICharacter;

	[Title("Players List")]
	public List<CharacterBase> characterAlive;

	public List<CharacterBase> characterFullDead;

	//public List<GameObject> canvasPanelPlayer;
	//public List<TextMeshProUGUI> canvasPercentPlayer;

	[Title("Victory")]
	[SerializeField]
	private Menu.MenuWin menuWin;


	[Title("Boolean Condition")]
	public bool isGameStarted;

	// Start is called before the first frame update
	void Start()
	{
		if (gameData.GameMode == GameModeStateEnum.Bomb_Mode)
		{
			GameObject go = Instantiate(BombModeManager, transform.parent);
			go.GetComponent<StickyBombManager>().BattleManager = this;
		}
		SpawnPlayer();
	}

	// Update is called once per frame
	void Update()
	{
		//isFinish();
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

			//inputController.controllable[i] = user;
			inputController.controllable[gameData.CharacterInfos[i].ControllerID] = user;

			user.TeamID = gameData.CharacterInfos[i].Team;

			characterAlive.Add(user);

			user.PlayerID = i;
			user.Model.SetColor(i, gameData.CharacterInfos[i].CharacterData.characterMaterials[gameData.CharacterInfos[i].CharacterColorID]);

			user.Stats.GameData = gameData;
			user.Stats.gameEvent = gameEventUICharacter[i];
			user.Stats.InitStats();

            user.PowerGauge.gameEvent = gameEventUICharacter[i];

			//if (characterUi.Length != 0)
			//	characterUi[i].InitPlayerPanel(user);

			if (uiEvent != null)
				uiEvent.Raise(user);

			cameraController.targets.Add(go.transform);
		}
		isGameStarted = true;
	}


	public void ResetPlayer()
	{
		for (int i = 0; i < gameData.CharacterInfos.Count; i++)
		{
			characterAlive[i].transform.position = spawningPoint[i].transform.position;
			characterAlive[i].Stats.InitStats();

			characterAlive[i].Action.CancelAction();
			characterAlive[i].ResetToIdle();
			characterAlive[i].Movement.SpeedX = 0;
			characterAlive[i].Movement.SpeedY = 0;
		}
		isGameStarted = true;
	}

	// This function transfer player from isAlive to isFullDead and check if party over
	// Basically it manages definitive death
	public void ObliterateCharacter(CharacterBase cb)
    {
		Debug.Log("Character full dead");
		if(characterAlive.Contains(cb))
        {
			characterFullDead.Add(cb);
			characterAlive.Remove(cb);

			isFinish();
        }
    }

	public void isFinish()
    {
		if(characterAlive.Count == 1 && isGameStarted)
		{
			Debug.Log("The Game is Over, EVERYONE IS FULL DEAD EXCEPT THE ALMIGHTY BERNARD");

			StartCoroutine(EndBattleCoroutine());
			//UnityEngine.SceneManagement.SceneManager.LoadScene("GP_Menu");
        }
    }


	protected IEnumerator EndBattleCoroutine()
	{
		Time.timeScale = 0.2f;
		yield return new WaitForSecondsRealtime(2f);
		Time.timeScale = 1f;

		cameraController.gameObject.SetActive(false);
		for (int i = 0; i < inputController.controllable.Length; i++)
		{
			inputController.controllable[i] = menuWin;
		}



		for (int i = 0; i < characterAlive.Count; i++)
		{
			characterFullDead.Add(characterAlive[i]);
		}
		characterFullDead.Reverse();
		menuWin.InitializeWin(characterFullDead);
	}
}