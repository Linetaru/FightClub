using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleEntity : MonoBehaviour
{
    public class PlayerData
    {
        public float timer;
        public CharacterBase player;

        public PlayerData(float timerMax, CharacterBase user)
        {
            timer = timerMax;
            player = user;
        }
    }

    public float _speed;

	public float _deathTimer;

    public List<PlayerData> playerTouched = new List<PlayerData>();

	// Update is called once per frame
	void Update()
	{
		transform.position += Vector3.left * _speed * Time.deltaTime;
		_deathTimer -= Time.deltaTime;
		if (_deathTimer <= 0)
		{
			Destroy(gameObject);
		}

        if (playerTouched.Count > 0)
        {
            for (int i = 0; i < playerTouched.Count; i++)
            {
                if (playerTouched[i].timer > 0)
                {
                    playerTouched[i].timer -= Time.deltaTime;
                }
                else
                {
                    playerTouched.Remove(playerTouched[i]);
                }
            }
        }
    }

	private void OnTriggerEnter(Collider collision)
	{
        string tag = collision.gameObject.tag;

        CharacterBase playerCB = collision.transform.root.gameObject.GetComponent<CharacterBase>();

        if (playerCB != null)
        {
            if (!playerCB.Knockback.IsInvulnerable)
            {
                if (playerTouched.Count > 0)
                    foreach (PlayerData cB in playerTouched)
                    {
                        if (cB != null)
                            if (cB.player.ControllerID == playerCB.ControllerID && cB.timer > 0)
                                return;
                    }

                BlastZoneManager.Instance.ExplosionDeath(collision);
                float stocks = playerCB.Stats.LifeStocks;
                if (stocks - 1 > 0)
                {
                    // Respawn Manager
                    playerCB.SetState(playerCB.GetComponentInChildren<CharacterStateDeath>());
                    playerCB.Stats.RespawnStats();

                }
                else
                {
                    playerCB.Stats.Death = true;
                    playerCB.SetState(playerCB.GetComponentInChildren<CharacterStateDeath>());
                    BlastZoneManager.Instance.gameEventCharacterFullDead.Raise(playerCB);
                }

                //Float Event to update Stock UI
                if (tag == "Player1")
                    BlastZoneManager.Instance.gameEventStocks[0].Raise(playerCB);
                else if (tag == "Player2")
                    BlastZoneManager.Instance.gameEventStocks[1].Raise(playerCB);
                else if (tag == "Player3")
                    BlastZoneManager.Instance.gameEventStocks[2].Raise(playerCB);
                else if (tag == "Player4")
                    BlastZoneManager.Instance.gameEventStocks[3].Raise(playerCB);

                playerTouched.Add(new PlayerData(3, playerCB));
            }
        }
    }
}