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

	// Start is called before the first frame update
	void Start()
	{
		foreach(CharacterBase cbase in battleManager.characterAlive)
        {
			cbase.Movement.JumpNumber = 9999;
			cbase.Movement.CurrentNumberOfJump = 9998;
        }

		battleManager.cameraController.ChangeFocusState();
		battleManager.cameraController.targets.Add(new TargetsCamera(battleManager.cameraController.focusLevel.transform, 10));
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}