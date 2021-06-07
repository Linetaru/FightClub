using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Sirenix.OdinInspector;

public class BattleManager : MonoBehaviour
{
	[Title("Data")]
	[Expandable]
	public GameData gameData;
	public Color[] teamTextColors;

	[Title("Game Mode Managers")]
	public GameObject BombModeManager;
	public FlappyModeManager FlappyModeManager;

	[Title("Events")]
	public PackageCreator.Event.GameEventCharacter uiEvent;
	[HideInInspector]
	public UnityEvent gameEndedEvent = new UnityEvent();

	[Title("Interractions")]
	public InputController inputController;
	public AIController aIController;
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
	public Menu.MenuWin MenuWin
	{
		get { return menuWin; }
	}


	[Title("Boolean Condition")]
	public bool isGameStarted;

	private bool slowMowEnd;
	private float timer;

	[SerializeField]
	private Canvas canvasUI;
	[SerializeField]
	private Image fadeImage;

	Input_Info input;
	List<IControllable> standbyList = new List<IControllable>();


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
		Debug.Log("SLAM : " + gameData.slamMode);
		if(!gameData.slamMode)
		{
			fadeImage.enabled = true;
			StartBattleManager();
		}
	}

    public void StartBattleManager()
	{
		standbyList = new List<IControllable>();
		input = new Input_Info();



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


	bool shnell = false;
	// Update is called once per frame
	void Update()
	{
		if (standbyList.Count == 0 && shnell == false)
			aIController.StartBehaviors();
		shnell = true;
		for (int i = 0; i < standbyList.Count; i++)
		{
			standbyList[i].UpdateControl(0, input);
		}

		if(slowMowEnd)
        {
			if(timer < 2f)
			{
				timer += Time.unscaledDeltaTime;
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

			if (gameData.CharacterInfos[i].ControllerID >= 0)
			{
				inputController.controllable[gameData.CharacterInfos[i].ControllerID] = user;
				inputController.SetInputMapping(gameData.CharacterInfos[i].ControllerID, gameData.CharacterInfos[i].InputMapping);
			}
			else
			{
				int aiDifficulty = Mathf.Abs(gameData.CharacterInfos[i].ControllerID) - 1;
				AIBehavior aIBehavior = Instantiate(gameData.CharacterInfos[i].CharacterData.aiBehavior[aiDifficulty], user.transform);
				aIBehavior.SetCharacter(user, inputController);
				aIBehavior.StartBehavior();
				aIController.AIBehaviors.Add(aIBehavior);
			}

			user.TeamID = gameData.CharacterInfos[i].Team;

			characterAlive.Add(user);

			user.PlayerID = i;
			user.ControllerID = gameData.CharacterInfos[i].ControllerID;
			user.Model.SetColor(i, gameData.CharacterInfos[i].CharacterData.characterMaterials[gameData.CharacterInfos[i].CharacterColorID]);
			user.Model.SetTextColor(teamTextColors[(int) gameData.CharacterInfos[i].Team]);
			user.Movement.Direction = (int)spawningPoint[i].transform.localScale.x;

			user.Stats.gameEvent = gameEventUICharacter[i];
			user.Stats.InitStats();
			user.Stats.LifeStocks = gameData.NumberOfLifes;

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

			if(!gameData.slamMode)
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

		if(!gameData.slamMode)
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

		canvasUI.enabled = false;
	}

	public void ManageEndBattle()
    {
		if (!gameData.slamMode)
			menuWin.InitializeWin(characterFullDead);

	}


	// JSP si là c'est le mieux
	public void SetMenuControllable(IControllable controllable)
	{
		for (int i = 0; i < characterAlive.Count; i++)
		{
			if (characterAlive[i].ControllerID >= 0)
				standbyList.Add(characterAlive[i]);
		}
		for (int i = 0; i < characterFullDead.Count; i++)
		{
			if (characterFullDead[i].ControllerID >= 0)
				standbyList.Add(characterFullDead[i]);
		}

		for (int i = 0; i < inputController.controllable.Length; i++)
		{
			/*if (inputController.controllable[i] != null)
			{
				standbyList.Add(inputController.controllable[i]);
			}*/
			inputController.controllable[i] = controllable;
		}
		// Enlevé les IA
		aIController.StopBehaviors();
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
			if(characterAlive[i].ControllerID >= 0)
				inputController.controllable[characterAlive[i].ControllerID] = characterAlive[i];
		}
		for (int i = 0; i < characterFullDead.Count; i++)
		{
			if (characterFullDead[i].ControllerID >= 0)
				inputController.controllable[characterFullDead[i].ControllerID] = characterFullDead[i];
		}

		aIController.StartBehaviors();
	}
}