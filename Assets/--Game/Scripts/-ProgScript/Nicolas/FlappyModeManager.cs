using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyModeManager : MonoBehaviour
{
	private BattleManager battleManager;
	public BattleManager BattleManager
	{
		set { battleManager = value; }
	}

	public List<SpawnerObstacle> spawnerObstacles;

	public GameObject bigObstacle;

	enum State{
		ClassicSpawner1,
		ClassicSpawner2,
		DoubleSpawn,
    }

	private State state = State.ClassicSpawner1;

	int spawnerAvailable = 0;

	public float timeToStart;

	[Range(1, 9999)]
	public int JumpNumberMax = 9999;

	// Start is called before the first frame update
	void Start()
	{
		foreach(CharacterBase cbase in battleManager.characterAlive)
        {
			cbase.Movement.JumpNumber = JumpNumberMax;
			cbase.Movement.CurrentNumberOfJump = JumpNumberMax;
        }

		//battleManager.cameraController.ChangeFocusState();
		battleManager.cameraController.targets.Add(new TargetsCamera(battleManager.cameraController.focusLevel.transform, 0));
	}

	// Update is called once per frame
	void Update()
	{
		if(timeToStart > 0)
        {
			timeToStart -= Time.deltaTime;
			return;
		}

		for (int i = 0; i < spawnerObstacles.Count; i++)
		{
			spawnerObstacles[i]._timerSpawn -= Time.deltaTime;
			if (spawnerObstacles[i]._timerSpawn <= 0)
			{
				spawnerAvailable++;
				//spawnerObstacles[i].SpawnItem();
				spawnerObstacles[i]._timerSpawn = spawnerObstacles[i]._timeSpawnResetValue;
				spawnerObstacles[i].spawnPass++;
			}
		}

		if (spawnerAvailable >= spawnerObstacles.Count)
		{
			state = (State)Random.Range(0, 3);

            switch (state)
            {
                case State.ClassicSpawner1:
					spawnerObstacles[0].SpawnItem(bigObstacle);
					break;
                case State.ClassicSpawner2:
					spawnerObstacles[1].SpawnItem();
					break;
                case State.DoubleSpawn:
					spawnerObstacles[0].SpawnItem();
					spawnerObstacles[1].SpawnItem();
					break;
            }

			spawnerAvailable = 0;
		}
    }
}