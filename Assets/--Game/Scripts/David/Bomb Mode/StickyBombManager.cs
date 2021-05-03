using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class StickyBombManager : MonoBehaviour
{
    // Singleton
    private static StickyBombManager _instance;
    public static StickyBombManager Instance { get { return _instance; } }

    [SerializeField]
    private BombIcon bombIcon;

    [SerializeField]
    private GameObject explosionPrefab;

    public float bombTimer = 10f;

    [SerializeField]
    private float timeBetweenRounds;

    private BattleManager battleManager;
	public BattleManager BattleManager
    {
        set { battleManager = value; }
    }

    private CharacterBase oldBombedPlayer;
    private CharacterBase currentBombedPlayer;

    private Vector3 playerOriginalScale;

    //Float Event
    public PackageCreator.Event.GameEventUICharacter[] gameEventStocks;
    //Chararcter Event
    public PackageCreator.Event.GameEventCharacter gameEventCharacterFullDead;

    [SerializeField]
    private StatusData status;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }


    void Start()
    {
        InitStickyBomb();
    }


    void Update()
    {
        
    }
    public void ManageHit(CharacterBase user, CharacterBase target)
    {
        if(user.CharacterIcon.Icon.IsEnabled)
        {
            oldBombedPlayer = user;
            oldBombedPlayer.Status.RemoveStatus("osef");
            oldBombedPlayer.transform.localScale = playerOriginalScale;
            currentBombedPlayer = target;
            playerOriginalScale = currentBombedPlayer.transform.localScale;
            currentBombedPlayer.Status.AddStatus(new Status("osef", status));
            UpdateBombIcons();
        }
    }

    public void InitStickyBomb()
    {
        for(int i = 0; i < battleManager.characterAlive.Count; i++)
        {
            battleManager.characterAlive[i].CharacterIcon.CreateIcon(bombIcon);
        }

        int firstPlayerBomb = Random.Range(0, battleManager.characterAlive.Count);
        currentBombedPlayer = battleManager.characterAlive[firstPlayerBomb];

        playerOriginalScale = currentBombedPlayer.transform.localScale;

        currentBombedPlayer.Status.AddStatus(new Status("osef", status));

        StartCoroutine(LerpScale());
        UpdateBombIcons();
    }

    public void UpdateBombIcons()
    {
        if (oldBombedPlayer != null)
            oldBombedPlayer.CharacterIcon.SwitchIcon();

        currentBombedPlayer.CharacterIcon.SwitchIcon();
    }

    public void TimeOutManager()
    {
        ExplosionDeath();
        currentBombedPlayer.Status.RemoveStatus("osef");
        currentBombedPlayer.transform.localScale = playerOriginalScale;

        // Destroy all bombs
        for (int i = 0; i < battleManager.characterAlive.Count; i++)
        {
            battleManager.characterAlive[i].CharacterIcon.DestroyIcon();
        }


        float stocks = currentBombedPlayer.Stats.LifeStocks;
        string tag = currentBombedPlayer.gameObject.tag;

        Debug.Log("TAG = " + tag);

        if (stocks - 1 > 0)
        {
            // Respawn Manager
            currentBombedPlayer.SetState(currentBombedPlayer.GetComponentInChildren<CharacterStateDeath>());
            currentBombedPlayer.Stats.RespawnStats();

        }
        else
        {
            currentBombedPlayer.Stats.Death = true;
            currentBombedPlayer.SetState(currentBombedPlayer.GetComponentInChildren<CharacterStateDeath>());
            gameEventCharacterFullDead.Raise(currentBombedPlayer);
        }

        //Float Event to update Stock UI
        if (tag == "Player1")
            gameEventStocks[0].Raise(currentBombedPlayer);
        else if (tag == "Player2")
            gameEventStocks[1].Raise(currentBombedPlayer);
        else if (tag == "Player3")
            gameEventStocks[2].Raise(currentBombedPlayer);
        else if (tag == "Player4")
            gameEventStocks[3].Raise(currentBombedPlayer);


        currentBombedPlayer = null;
        oldBombedPlayer = null;

        StartCoroutine(WaitBeforeNextRound());
    }

    private void ExplosionDeath()
    {
        GameObject explosion = Instantiate(explosionPrefab, currentBombedPlayer.transform.position, Quaternion.identity);
        Destroy(explosion, 4f);
    }

    IEnumerator WaitBeforeNextRound()
    {
        yield return new WaitForSecondsRealtime(timeBetweenRounds);
        InitStickyBomb();
    }



    IEnumerator LerpScale()
    {
        float time = 0f;

        while (time < bombTimer)
        {
            currentBombedPlayer.transform.localScale = Vector3.Lerp(playerOriginalScale, new Vector3(2, 2, 2), time / bombTimer);
            time += Time.deltaTime;
            yield return null;
        }
    }

}
