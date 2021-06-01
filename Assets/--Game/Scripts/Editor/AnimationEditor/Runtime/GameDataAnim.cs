using UnityEngine;

[CreateAssetMenu(menuName = "Toolbox/Game/GameData")]
public class GameDataAnim : ScriptableObject
{
    [SerializeField] private int _nbPlayers = 4;
    public float[] playersSpeed;
}
