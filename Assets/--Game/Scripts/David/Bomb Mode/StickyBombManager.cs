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

    [SerializeField]
    private List<int> roundsWithFakeBomb = new List<int> { 2, 6, 10 };

    private BattleManager battleManager;
	public BattleManager BattleManager
    {
        set { battleManager = value; }
    }

    private CharacterBase currentBombedPlayer;
    private CharacterBase oldBombedPlayer;
    private CharacterBase currentFakeBombedPlayer;
    private CharacterBase oldFakeBombedPlayer;

    private Vector3 playerOriginalScale;
    private Vector3 fakeBombedOriginalScale;

    //Float Event
    public PackageCreator.Event.GameEventUICharacter[] gameEventStocks;
    //Chararcter Event
    public PackageCreator.Event.GameEventCharacter gameEventCharacterFullDead;

    [SerializeField]
    private StatusData status;


    // Test Rounds
    private int currentRound = 0;

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
        if(user.CharacterIcon.Icon.IsEnabled && target != currentFakeBombedPlayer && target != currentBombedPlayer)
        {
            if(user == currentBombedPlayer)
            {
                oldBombedPlayer = user;
                oldBombedPlayer.Status.RemoveStatus("osef");
                oldBombedPlayer.transform.localScale = playerOriginalScale;
                currentBombedPlayer = target;

                playerOriginalScale = currentBombedPlayer.transform.localScale;
                currentBombedPlayer.Status.AddStatus(new Status("osef", status));
                UpdateBombIcons();
            }
            else if(user == currentFakeBombedPlayer)
            {
                oldFakeBombedPlayer = user;

                currentFakeBombedPlayer.Status.RemoveStatus("osef");
                currentFakeBombedPlayer.transform.localScale = fakeBombedOriginalScale;

                currentFakeBombedPlayer = target;
                fakeBombedOriginalScale = currentFakeBombedPlayer.transform.localScale;
                currentFakeBombedPlayer.Status.AddStatus(new Status("osef", status));

                UpdateFakeBombIcons();
            }

        }
    }

    public void InitStickyBomb()
    {
        currentRound++;

        if (battleManager.characterAlive.Count < 3)
            roundsWithFakeBomb = null;

        for(int i = 0; i < battleManager.characterAlive.Count; i++)
        {
            battleManager.characterAlive[i].CharacterIcon.CreateIcon(bombIcon);
        }

        int firstPlayerBomb = Random.Range(0, battleManager.characterAlive.Count);
        currentBombedPlayer = battleManager.characterAlive[firstPlayerBomb];
        playerOriginalScale = currentBombedPlayer.transform.localScale;
        currentBombedPlayer.Status.AddStatus(new Status("osef", status));

        StartCoroutine(LerpScale(currentBombedPlayer));

        FakeBombManager();

        UpdateBombIcons();
        UpdateFakeBombIcons();
    }

    public void FakeBombManager()
    {
        if(roundsWithFakeBomb.Contains(currentRound))
        {
            List<CharacterBase> tmpList = battleManager.characterAlive;
            tmpList.Remove(currentBombedPlayer);

            int playerFakeBomb = Random.Range(0, tmpList.Count);
            currentFakeBombedPlayer = tmpList[playerFakeBomb];
            currentFakeBombedPlayer.Status.AddStatus(new Status("osef", status));

            fakeBombedOriginalScale = currentFakeBombedPlayer.transform.localScale;

            StartCoroutine(LerpScale(currentFakeBombedPlayer));
        }
    }

    public void UpdateBombIcons()
    {
        if (oldBombedPlayer != null)
            oldBombedPlayer.CharacterIcon.SwitchIcon(false);

        if (currentBombedPlayer != null)
            currentBombedPlayer.CharacterIcon.SwitchIcon(false);
    }

    public void UpdateFakeBombIcons()
    {
        if (oldFakeBombedPlayer != null)
        {
            oldFakeBombedPlayer.CharacterIcon.SwitchIcon(true);
        }

        if (currentFakeBombedPlayer != null)
        {
            currentFakeBombedPlayer.CharacterIcon.SwitchIcon(true);
        }
    }

    public void TimeOutManager()
    {
        ExplosionDeath();

        if (currentFakeBombedPlayer != null)
        {
            currentFakeBombedPlayer.Status.RemoveStatus("osef");
            currentFakeBombedPlayer.transform.localScale = fakeBombedOriginalScale;
            currentFakeBombedPlayer = null;
        }

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
        Debug.LogError("Explosion de : " + currentBombedPlayer);
        GameObject explosion = Instantiate(explosionPrefab, currentBombedPlayer.transform.position, Quaternion.identity);
        Destroy(explosion, 4f);
    }

    IEnumerator WaitBeforeNextRound()
    {
        yield return new WaitForSecondsRealtime(timeBetweenRounds);
        InitStickyBomb();
    }



    IEnumerator LerpScale(CharacterBase player)
    {
        float time = 0f;

        Vector3 originalScale = playerOriginalScale;

        while (time < bombTimer)
        {
            if(currentBombedPlayer != null)
            {
                currentBombedPlayer.transform.localScale = Vector3.Lerp(originalScale, new Vector3(2, 2, 2), time / (bombTimer * 0.75f));

                if(currentFakeBombedPlayer != null)
                    currentFakeBombedPlayer.transform.localScale = Vector3.Lerp(originalScale, new Vector3(2, 2, 2), time / (bombTimer * 0.75f));


                time += Time.deltaTime;
                yield return null;
            }
            else
            {
                break;
            }
        }
    }

}
