using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Sirenix.OdinInspector;

public class BattleManager : MonoBehaviour
{
	//[SerializeField]
	public bool autoStart = true;

	[Title("Data")]
	[Expandable]
	public GameData gameData;
	public Color[] teamTextColors;

	[Title("Game Mode Managers")]
	public GameObject BombModeManager;
	public FlappyModeManager FlappyModeManager;

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


	[Title("Victory")]
	[SerializeField]
	private Menu.MenuWin menuWin;


	[Title("Boolean Condition")]
	public bool isGameStarted;

	private bool slowMowEnd;
	private float timer;

	Input_Info input;
	List<IControllable> standbyList = new List<IControllable>();

	public UnityEvent gameEndedEvent = new UnityEvent();


	//SINGLETON

	private static BattleManager _instance;
	public static BattleManager Instance { get { return _instance; } }

	private void Awake()
	{
		if (_instance != null)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
		}
	}

    public void ResetInstance()
    {
		_instance = null;
    }

    //END SINGLETON


    // Start is called before the first frame update
    void Start()
	{
		if(autoStart)
			StartBattleManager();
	}

    public void StartBattleManager()
	{
		standbyList = new List<IControllable>();
		input = new Input_Info();

		/*
		if (gameEndedEvent == null)
			gameEndedEvent = new UnityEvent();
		*/

		gameEndedEvent.AddListener(ManageEndBattle);

		SpawnPlayer();
		if (gameData.GameMode == GameModeStateEnum.Bomb_Mode)
		{
			GameObject go = Instantiate(BombModeManager, transform.parent);
			go.GetComponent<StickyBombManager>().BattleManager = this;
		}
		else if (gameData.GameMode == GameModeStateEnum.Flappy_Mode)
		{
			FlappyModeManager go = Instantiate(FlappyModeManager, transform.parent);
			go.BattleManager = this;
		}
	}

	// Update is called once per frame
	void Update()
	{
		for (int i = 0; i < standbyList.Count; i++)
		{
			standbyList[i].UpdateControl(0, input);
		}

		if(slowMowEnd)
        {
			if(timer < 2f)
			{
				timer += Time.unscaledDeltaTime;
				Debug.Log(timer);
            }
            else
            {
				timer = 0f;
				slowMowEnd = false;
				EndBattle();
            }
        }
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
			user.ControllerID = gameData.CharacterInfos[i].ControllerID;
			user.Model.SetColor(i, gameData.CharacterInfos[i].CharacterData.characterMaterials[gameData.CharacterInfos[i].CharacterColorID]);
			user.Model.SetTextColor(teamTextColors[(int) gameData.CharacterInfos[i].Team]);
			user.Movement.Direction = (int)spawningPoint[i].transform.localScale.x;

			user.Stats.GameData = gameData;
			user.Stats.gameEvent = gameEventUICharacter[i];
			user.Stats.InitStats();

            user.PowerGauge.gameEvent = gameEventUICharacter[i];

			//if (characterUi.Length != 0)
			//	characterUi[i].InitPlayerPanel(user);

			if (uiEvent != null)
				uiEvent.Raise(user);

			cameraController.targets.Add(new TargetsCamera(go.transform, 0));
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

			if(gameData.GameMode == GameModeStateEnum.Flappy_Mode)
            {
				foreach(SpawnerObstacle spO in FlappyModeManager.spawnerObstacles)
                {
					if(spO.pipes.Count != 0)
						foreach(GameObject go in spO.pipes)
						{
							if(go != null)
							{
								Destroy(go);
							}
						}

					spO.enabled = false;
				}
            }

			if(autoStart) // TMP CONDITION POUR TEST
				SlowMotionEnd();
			else
			{
				enabled = false;
				EndBattle();
			}

			//StartCoroutine(EndBattleCoroutine());
			//UnityEngine.SceneManagement.SceneManager.LoadScene("GP_Menu");
		}
    }

	public void SlowMotionEnd()
	{
		Time.timeScale = 0.2f;
		slowMowEnd = true;
	}


	public void EndBattle()
	{
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

		// Event end game
		gameEndedEvent.Invoke();
	}

	public void ManageEndBattle()
    {
		if (autoStart) // TMP CONDITION POUR TEST
			menuWin.InitializeWin(characterFullDead);
		else
			Debug.Log("END BATTLE GRAND SLAM");

	}


	// JSP si là c'est le mieux
	public void SetMenuControllable(IControllable controllable)
	{
		for (int i = 0; i < inputController.controllable.Length; i++)
		{
			if (inputController.controllable[i] != null)
			{
				standbyList.Add(inputController.controllable[i]);
			}
			inputController.controllable[i] = controllable;
		}
		// Enlevé les IA
	}

	public void SetBattleControllable()
	{
		standbyList.Clear();
		for (int i = 0; i < inputController.controllable.Length; i++)
		{
			inputController.controllable[i] = null;
		}


		for (int i = 0; i < characterAlive.Count; i++)
		{
			inputController.controllable[characterAlive[i].ControllerID] = characterAlive[i];
		}
		for (int i = 0; i < characterFullDead.Count; i++)
		{
			inputController.controllable[characterFullDead[i].ControllerID] = characterFullDead[i];
		}
	}
}