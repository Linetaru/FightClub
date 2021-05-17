using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using Sirenix.OdinInspector;

public class StickyBombManager : MonoBehaviour
{
    // Singleton
    private static StickyBombManager _instance;
    public static StickyBombManager Instance { get { return _instance; } }

    public enum RoundMode { Normal = 0, FakeBomb = 1, Invisible = 2, Inv_Countdown = 3, BombReset = 4}
    List<string> roundModeList = new List<string> { "Classic !", "Fake Bomb !", "Invisible Bomb !", "No Countdown !", "Bomb Reset !" };
    List<float> timerList = new List<float>();

    [Title("Objects")]
    [SerializeField]
    private BombIcon bombIcon;

    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private GameObject stickyBombUI;

    private StickyBombUIManager uiManager;

    private int currentRound = 0;
    private RoundMode currentRoundMode;
    public RoundMode CurrentRoundMode
    {
        get { return currentRoundMode; }
    }

    [Title("Round Infos")]
    [SerializeField]
    private float timeBetweenRounds;

    [HideInInspector]
    public float bombTimer = 10f;

    [Title("Timers")]
    [SerializeField]
    [Range(1f, 120f)]
    private float normalModeTimer = 10f;
    [SerializeField]
    [Range(1f, 120f)]
    private float fakeModeTimer = 10f;
    [SerializeField]
    [Range(1f, 120f)]
    private float invisibleModeTimer = 10f;
    [SerializeField]
    [Range(1f, 120f)]
    private float noCountModeTimer = 10f;
    [SerializeField]
    [Range(1f, 120f)]
    private float resetModeTimer = 10f;

    private bool startRound;

    private float originalBombTimer;

    private bool timeOut = true;

    [Title("Player Scale Infos")]
    // A partir de quel pourcentage de bombTimer la scale sera à son max
    // Si coefScaleMax = 0.5 => Le joueur aura sa scale max à 50% de bombTimer
    [SerializeField]
    [Range(0f, 1f)]
    [PropertyTooltip("Pourcentage de bombTimer auquel le player aura sa taille maximum (0.5 = 50%)")]
    private float coefScaleMax = 0.5f;

    [SerializeField]
    [Range(1f, 5f)]
    private float scaleMaxMultiplier = 2f;

    [Title("Special Rounds")]
    [SerializeField]
    private bool randomRounds;
    [SerializeField]
    private int canRoundSpecialAfter = 3;


    [SerializeField]
    private List<int> specialRounds = new List<int>();


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

    [Title("Events")]
    //Float Event
    public PackageCreator.Event.GameEventUICharacter[] gameEventStocks;
    //Chararcter Event
    public PackageCreator.Event.GameEventCharacter gameEventCharacterFullDead;

    [Title("Status")]
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
            //DontDestroyOnLoad(this.gameObject);
        }
    }


    void Start()
    {
        InitTimerList();

        originalBombTimer = bombTimer;

        GameObject stickyBombUIGO = Instantiate(stickyBombUI);

        uiManager = stickyBombUIGO.GetComponent<StickyBombUIManager>();

        StartCoroutine(WaitBeforeNextRound());

        for (int i = 0; i < battleManager.characterAlive.Count; i++)
        {
            battleManager.characterAlive[i].Knockback.Parry.OnGuard += ManageHit;
        }
        //InitStickyBomb();
    }

    void Update()
    {
        if(startRound)
        {
            if(uiManager.isCountdownOver)
            {
                uiManager.isCountdownOver = false;
                startRound = false;
                InitStickyBomb();
            }
        }
        BombTimerManager();
    }

    /*public void Callback(CharacterBase target)
    {
        ManageHit( target);
    }*/

    public void ManageHit(CharacterBase user, CharacterBase target)
    {
        if(currentRoundMode != RoundMode.FakeBomb)
        {
            if (user == currentBombedPlayer)
            {
                SwitchBombPlayers(user, target);
            }
            else if (target == currentBombedPlayer && currentRoundMode != RoundMode.Invisible)
            {
                SwitchBombPlayers(target, user);
            }

            if(currentRoundMode == RoundMode.BombReset)
            {
                bombTimer = timerList[(int)currentRoundMode];
            }
        }
        else
        {
            if(user == currentFakeBombedPlayer && target != currentBombedPlayer)
            {
                SwitchFakeBombPlayers(user, target);
            }
            else if(user == currentBombedPlayer && target != currentFakeBombedPlayer)
            {
                SwitchBombPlayers(user, target);
            }
            else if(target == currentBombedPlayer && user != currentFakeBombedPlayer)
            {
                SwitchBombPlayers(target, user);
            }
            else if(target == currentFakeBombedPlayer && user != currentBombedPlayer)
            {
                SwitchFakeBombPlayers(target, user);
            }
        }
    }

    public void InitStickyBomb()
    {
        timeOut = false;

        for(int i = 0; i < battleManager.characterAlive.Count; i++)
        {
            battleManager.characterAlive[i].CharacterIcon.CreateIcon(bombIcon);
        }

        int firstPlayerBomb = Random.Range(0, battleManager.characterAlive.Count);
        currentBombedPlayer = battleManager.characterAlive[firstPlayerBomb];

        if(currentRoundMode != RoundMode.Invisible)
        {
            playerOriginalScale = currentBombedPlayer.transform.localScale;
            currentBombedPlayer.Status.AddStatus(new Status("osef", status));
            StartCoroutine(LerpScale(currentBombedPlayer));

            FakeBombManager();

            UpdateBombInfos();
        }
    }

    public void FakeBombManager()
    {
        if(currentRoundMode == RoundMode.FakeBomb)
        {
            List<CharacterBase> tmpList = new List<CharacterBase>(battleManager.characterAlive);
            tmpList.Remove(currentBombedPlayer);

            int playerFakeBomb = Random.Range(0, tmpList.Count);
            currentFakeBombedPlayer = tmpList[playerFakeBomb];
            currentFakeBombedPlayer.Status.AddStatus(new Status("osef", status));

            fakeBombedOriginalScale = currentFakeBombedPlayer.transform.localScale;

            StartCoroutine(LerpScale(currentFakeBombedPlayer));

            UpdateFakeBombInfos();
        }
    }

    public void UpdateBombInfos()
    {
        playerOriginalScale = currentBombedPlayer.transform.localScale;
        currentBombedPlayer.Status.AddStatus(new Status("osef", status));

        if (oldBombedPlayer != null)
            oldBombedPlayer.CharacterIcon.SwitchIcon();

        if (currentBombedPlayer != null)
            currentBombedPlayer.CharacterIcon.SwitchIcon();
    }

    public void UpdateFakeBombInfos()
    {
        if (currentRoundMode == RoundMode.FakeBomb)
        {
            fakeBombedOriginalScale = currentFakeBombedPlayer.transform.localScale;
            currentFakeBombedPlayer.Status.AddStatus(new Status("osef", status));

            if (oldFakeBombedPlayer != null)
                oldFakeBombedPlayer.CharacterIcon.SwitchIcon();

            if (currentFakeBombedPlayer != null)
                currentFakeBombedPlayer.CharacterIcon.SwitchIcon();
        }
    }

    public void SwitchFakeBombPlayers(CharacterBase oldFakeBombed, CharacterBase newFakeBombed)
    {
        oldFakeBombedPlayer = oldFakeBombed;
        currentFakeBombedPlayer = newFakeBombed;

        oldFakeBombedPlayer.Status.RemoveStatus("osef");
        oldFakeBombedPlayer.transform.localScale = playerOriginalScale;

        UpdateFakeBombInfos();
    }

    public void SwitchBombPlayers(CharacterBase oldBombed, CharacterBase newBombed)
    {
        oldBombedPlayer = oldBombed;
        currentBombedPlayer = newBombed;

        oldBombedPlayer.Status.RemoveStatus("osef");
        oldBombedPlayer.transform.localScale = playerOriginalScale;

        if (currentRoundMode != RoundMode.Invisible)
            UpdateBombInfos();
    }

    public void TimeOut()
    {
        ExplosionDeath();

        if (currentRoundMode == RoundMode.FakeBomb)
        {
            currentFakeBombedPlayer.CharacterIcon.SwitchIcon();
            currentFakeBombedPlayer.Status.RemoveStatus("osef");
            currentFakeBombedPlayer.transform.localScale = fakeBombedOriginalScale;
            currentFakeBombedPlayer = null;
            oldFakeBombedPlayer = null;
        }

        currentBombedPlayer.Status.RemoveStatus("osef");
        currentBombedPlayer.transform.localScale = playerOriginalScale;

        float stocks = currentBombedPlayer.Stats.LifeStocks;
        string tag = currentBombedPlayer.gameObject.tag;

        // Destroy all bombs
        for (int i = 0; i < battleManager.characterAlive.Count; i++)
        {
            battleManager.characterAlive[i].CharacterIcon.DestroyIcon();
        }

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

    private void UpdateRoundMode()
    {
        currentRound++;

        if (!randomRounds)
        {
            // Sécurité au cas ou pas assez de rounds prévus (pas sensé arriver)
            if (currentRound >= specialRounds.Count)
                currentRound = 1;

            currentRoundMode = (RoundMode) specialRounds[currentRound - 1];
            if (battleManager.characterAlive.Count < 3 && currentRoundMode == RoundMode.FakeBomb)
                RandomRound();
        }
        else
        {
            if (currentRound > canRoundSpecialAfter)
                RandomRound();
            else
                currentRoundMode = RoundMode.Normal;
        }

        bombTimer = timerList[(int)currentRoundMode];
        originalBombTimer = timerList[(int)currentRoundMode];
        uiManager.ChangeCurrentModeValue(roundModeList[(int)currentRoundMode]);
    }

    private void RandomRound()
    {
        int random = Random.Range(0, 3);

        if (random == 0)
        {
            if(battleManager.characterAlive.Count > 2)
                currentRoundMode = (RoundMode) Random.Range(1, 5);
            else
                currentRoundMode = (RoundMode) Random.Range(2, 5);
        }
        else
        {
            currentRoundMode = RoundMode.Normal;
        }
    }

    private void BombTimerManager()
    {
        if(!timeOut)
        {
            bombTimer -= Time.deltaTime;
            if (bombTimer <= 0f)
            {
                timeOut = true;
                bombTimer = originalBombTimer;
                TimeOut();
            }
        }
    }

    private void InitTimerList()
    {
        timerList.Add(normalModeTimer);
        timerList.Add(fakeModeTimer);
        timerList.Add(invisibleModeTimer);
        timerList.Add(noCountModeTimer);
        timerList.Add(resetModeTimer);
    }

    private void ExplosionDeath()
    {
        GameObject explosion = Instantiate(explosionPrefab, currentBombedPlayer.transform.position, Quaternion.identity);
        Destroy(explosion, 4f);
    }

    IEnumerator WaitBeforeNextRound()
    {
        uiManager.RoundIsOver();
        UpdateRoundMode();
        yield return new WaitForSecondsRealtime(timeBetweenRounds);
        uiManager.LaunchCountDownAnim();
        startRound = true;
        //InitStickyBomb();
    }

    IEnumerator LerpScale(CharacterBase player)
    {
        Vector3 originalScale = playerOriginalScale;
        Vector3 targetScale = originalScale * scaleMaxMultiplier;

        while (bombTimer > 0)
        {
            if(currentBombedPlayer != null)
            {
                currentBombedPlayer.transform.localScale = Vector3.Lerp(originalScale, targetScale, (originalBombTimer - bombTimer) / (originalBombTimer * coefScaleMax));

                if(currentFakeBombedPlayer != null)
                    currentFakeBombedPlayer.transform.localScale = Vector3.Lerp(originalScale, targetScale, (originalBombTimer - bombTimer) / (originalBombTimer * coefScaleMax));

                yield return null;
            }
            else
            {
                break;
            }
        }
    }

}
