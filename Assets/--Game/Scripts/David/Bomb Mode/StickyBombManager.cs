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

    [Title("Objects")]
    [SerializeField]
    private BombIcon bombIcon;

    [SerializeField]
    private GameObject explosionPrefab;

    [Title("Round Infos")]
    public float bombTimer = 10f;
    [SerializeField]
    private float timeBetweenRounds;

    private float originalBombTimer;

    private bool timeOut = true;

    [Title("Player Scale Infos")]
    // A partir de quel pourcentage de bombTimer la scale sera à son max
    // Si coefScaleMax = 0.5 => Le joueur aura sa scale max à 50% de bombTimer
    [SerializeField]
    [Range(0f, 1f)]
    private float coefScaleMax = 0.5f;

    [SerializeField]
    [Range(1f, 5f)]
    private float scaleMaxMultiplier = 2f;

    [Title("Special Rounds")]
    [SerializeField]
    private List<int> roundsWithFakeBomb = new List<int> { 2, 6, 10 };
    [SerializeField]
    private List<int> roundsWithInvisibleBomb = new List<int> { 5, 8, 14 };

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
        originalBombTimer = bombTimer;
        InitStickyBomb();
    }

    void Update()
    {
        TimerManager();
    }

    public void ManageHit(CharacterBase user, CharacterBase target)
    {
        // Si on veut le transfert de bombe dans tous les cas
        
        if (user == currentBombedPlayer && target != currentFakeBombedPlayer)
        {
            oldBombedPlayer = user;
            oldBombedPlayer.Status.RemoveStatus("osef");
            oldBombedPlayer.transform.localScale = playerOriginalScale;
            currentBombedPlayer = target;

            playerOriginalScale = currentBombedPlayer.transform.localScale;
            currentBombedPlayer.Status.AddStatus(new Status("osef", status));
            UpdateBombIcons();
        }
        else if (user == currentFakeBombedPlayer && target != currentBombedPlayer)
        {
            oldFakeBombedPlayer = user;

            currentFakeBombedPlayer.Status.RemoveStatus("osef");
            currentFakeBombedPlayer.transform.localScale = fakeBombedOriginalScale;

            currentFakeBombedPlayer = target;
            fakeBombedOriginalScale = currentFakeBombedPlayer.transform.localScale;
            currentFakeBombedPlayer.Status.AddStatus(new Status("osef", status));

            UpdateFakeBombIcons();
        }
        else if(user != currentBombedPlayer && user != currentFakeBombedPlayer)
        {
            if(target == currentBombedPlayer)
            {
                oldBombedPlayer = target;
                oldBombedPlayer.Status.RemoveStatus("osef");
                oldBombedPlayer.transform.localScale = playerOriginalScale;
                currentBombedPlayer = user;

                playerOriginalScale = currentBombedPlayer.transform.localScale;
                currentBombedPlayer.Status.AddStatus(new Status("osef", status));
                UpdateBombIcons();
            }
            else if(target == currentFakeBombedPlayer)
            {
                oldFakeBombedPlayer = target;

                currentFakeBombedPlayer.Status.RemoveStatus("osef");
                currentFakeBombedPlayer.transform.localScale = fakeBombedOriginalScale;

                currentFakeBombedPlayer = user;
                fakeBombedOriginalScale = currentFakeBombedPlayer.transform.localScale;
                currentFakeBombedPlayer.Status.AddStatus(new Status("osef", status));

                UpdateFakeBombIcons();
            }
        }
        


        // Si on veut le transfert de bombe seulement par celui qui la possède
        /*
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
        */

    }

    public void InitStickyBomb()
    {
        timeOut = false;
        currentRound++;

        if (battleManager.characterAlive.Count < 3)
            roundsWithFakeBomb.Clear();

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
            List<CharacterBase> tmpList = new List<CharacterBase>(battleManager.characterAlive);
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
            oldBombedPlayer.CharacterIcon.SwitchIcon();

        if (currentBombedPlayer != null)
            currentBombedPlayer.CharacterIcon.SwitchIcon();
    }

    public void UpdateFakeBombIcons()
    {
        if (oldFakeBombedPlayer != null)
        {
            oldFakeBombedPlayer.CharacterIcon.SwitchIcon();
        }

        if (currentFakeBombedPlayer != null)
        {
            currentFakeBombedPlayer.CharacterIcon.SwitchIcon();
        }
    }

    public void TimeOut()
    {

        ExplosionDeath();

        if (currentFakeBombedPlayer != null)
        {
            currentFakeBombedPlayer.CharacterIcon.SwitchIcon();
            currentFakeBombedPlayer.Status.RemoveStatus("osef");
            currentFakeBombedPlayer.transform.localScale = fakeBombedOriginalScale;
            currentFakeBombedPlayer = null;
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

    private void TimerManager()
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

    IEnumerator LerpScale(CharacterBase player)
    {
        Vector3 originalScale = playerOriginalScale;
        Vector3 targetScale = originalScale * scaleMaxMultiplier;

        while (bombTimer > 0)
        {
            Debug.LogError("UP SCALE !!!!");
            if(currentBombedPlayer != null)
            {
                // valeur arbitraire tmp pour le scale
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
