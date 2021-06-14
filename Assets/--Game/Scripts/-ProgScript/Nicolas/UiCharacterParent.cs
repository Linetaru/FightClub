using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiCharacterParent : MonoBehaviour
{
	public GameData gameData;
	public CharacterHUD characterHudPrefab;
	public Transform parent;

	public TeamEnum teamEnums;
	public Color[] teamColors;

	List<CharacterHUD> huds = new List<CharacterHUD>();
	List<Transform> players = new List<Transform>();
	Camera cam;
	bool fade = false;



	private void Start()
	{
		cam = Camera.main;
	}



	public void CharacterInitUi(CharacterBase user)
	{
		CharacterHUD hud = Instantiate(characterHudPrefab, parent);
		if (Mathf.Sign(gameData.CharacterInfos[user.PlayerID].ControllerID) > -1)
		{
			if (gameData.CharacterInfos[user.PlayerID].InputMapping.profileName != "classic")
			{
				hud.DrawName(gameData.CharacterInfos[user.PlayerID].InputMapping.profileName + " J" + (user.PlayerID + 1));
			}
			else
				hud.DrawName(gameData.CharacterInfos[user.PlayerID].CharacterData.characterName + " J" + (user.PlayerID + 1));
		}
		else
        {
				hud.DrawName(gameData.CharacterInfos[user.PlayerID].CharacterData.characterName + " Bot J" + (user.PlayerID + 1));
		}
		hud.DrawFace(gameData.CharacterInfos[user.PlayerID].CharacterData.characterFace);
		hud.DrawLifeFace(gameData.CharacterInfos[user.PlayerID].CharacterData.characterLifeStocks);
		hud.InitPlayerPanel(user);
		hud.SetColor(teamColors[(int)user.TeamID]);

		players.Add(user.transform);
		huds.Add(hud);
	}



	private void Update()
	{
		for (int i = 0; i < players.Count; i++)
		{
			if (cam.WorldToViewportPoint(players[i].position).y < 0.05f)
			{
				if(fade == false)
				{
					for (int k = 0; k < huds.Count; k++)
					{
						huds[k].Fade(true);
					}
					fade = true;
				}
				return;
			}
		}

		if (fade == true)
		{
			for (int k = 0; k < huds.Count; k++)
			{
				huds[k].Fade(false);
			}
			fade = false;
		}

	}
}