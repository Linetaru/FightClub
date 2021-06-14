using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixGameDataBuild : MonoBehaviour
{
    public static FixGameDataBuild instance;

    [SerializeField]
    public GameData gameData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }
}
