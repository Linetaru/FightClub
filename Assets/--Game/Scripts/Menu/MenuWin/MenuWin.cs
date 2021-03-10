using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Menu
{
	public class MenuWin : MonoBehaviour, IControllable
	{
		// Database de victory

		// Positions des models 3D
		[SerializeField]
		Transform winnerPosition;
		[SerializeField]
		Transform[] loosersPosition;


		[Title("UI")]
		[SerializeField]
		MenuWinResultDrawer prefabResultDrawer;
		[SerializeField]
		Transform parentResult;

		bool stateResult = false;

		int winnerID = -1;
		List<MenuWinResultDrawer> listResultDrawers;
		List<int> listPlayerChoice;

		public void InitializeWin(List<CharacterBase> deadCharacters, List<CharacterBase> winners)
		{
			// Créer la liste playerChoice
			// Spawn du victory
			// Spawn des mesh sur les loosersPosition
		}


		public void UpdateControl(int id, Input_Info input)
		{
			if(stateResult == false)
				UpdateControlSkip(id, input);
			else
				UpdateControlResult(id, input);
		}

		private void UpdateControlSkip(int id, Input_Info input)
		{
			if (id == winnerID)
			{
				// Skip
			}
		}

		private void UpdateControlResult(int id, Input_Info input)
		{
			if (listPlayerChoice[id] == -1) // Aucun choix de fait  
			{
				if (input.CheckAction(id, InputConst.Return))
				{
					listPlayerChoice[id] = 0;
					CheckEndScreen();
				}
			}
			else // On a fait un choix donc on peut juste l'annuler
			{
				if (input.CheckAction(id, InputConst.Return))
				{
					listPlayerChoice[id] = -1;
				}
			}
		}




		// On check si chaque joueur a fais un choix, et donc qu'il n'y ait pas d'élément égal à -1 dans la listPlayerChoice
		private void CheckEndScreen()
		{
			bool onePlayerSurrend = false;
			for (int i = 0; i < listPlayerChoice.Count; i++)
			{
				if (listPlayerChoice[i] == -1)
					return;
				else if (listPlayerChoice[i] == 0) // Surrend
					onePlayerSurrend = true;
			}


			if (onePlayerSurrend == true)
			{
				// Retour au menu
			}
			else
			{
				// Rematch
			}
		}
	}
}
