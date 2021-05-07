using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMode_Feature_TP : MonoBehaviour
{
	public class BombTPData
    {
		public float timer;
		public CharacterBase player;

		public BombTPData(float timerMax, CharacterBase user)
        {
			timer = timerMax;
			player = user;
		}
    }

	public BombMode_Feature_TP otherTP;

	List<BombTPData> players = new List<BombTPData>();

	public float timerMax = 2f;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		transform.Rotate(0, 0, 50 * Time.deltaTime);

		if (players.Count > 0)
        {
			for(int i = 0; i < players.Count; i++)
            {
				if(players[i].timer > 0)
                {
					players[i].timer -= Time.deltaTime;
				}
				else
                {
					players.Remove(players[i]);
				}
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
		CharacterBase user;
		if (other.gameObject.GetComponent<CharacterBase>() != null )
		{
			user = other.gameObject.GetComponent<CharacterBase>();

			if (players.Count != 0)
			{
				foreach (BombTPData bombtp in players)
				{
					if (bombtp.player.ControllerID == user.ControllerID)
						return;
				}
			}

			otherTP.players.Add(new BombTPData(timerMax, user));
			players.Add(new BombTPData(timerMax, user));

			user.transform.position = otherTP.transform.position;
		}

	}
}