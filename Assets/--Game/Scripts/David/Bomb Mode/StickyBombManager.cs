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
    
    public float bombTimer = 10f;

    private BattleManager battleManager;
	public BattleManager BattleManager
    {
        set { battleManager = value; }
    }

    private CharacterBase oldBombedPlayer;
    private CharacterBase currentBombedPlayer;

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
        if(user.CharacterIcon.enabled)
        {
            oldBombedPlayer = user;
            currentBombedPlayer = target;
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
        UpdateBombIcons();
    }

    public void UpdateBombIcons()
    {
        if (oldBombedPlayer != null)
            oldBombedPlayer.CharacterIcon.SwitchIcon();

        currentBombedPlayer.CharacterIcon.SwitchIcon();
    }

}
