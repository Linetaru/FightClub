﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

namespace Menu
{
	public class MenuWin : MonoBehaviour, IControllable
	{
		// Database de victory
		[SerializeField]
		VictoryScreen debugVictory;

		[Title("Positions")]
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

		[Title("Feedback")]
		[SerializeField]
		Animator fadeInTransition;

		int stateResult = 0; // 0 = skip // 1 = result // 2 = transition de fin (faire un enum)

		int winnerID = -1;
		List<MenuWinResultDrawer> listResultDrawers;
		List<int> listPlayerChoice;

		public void InitializeWin(List<CharacterBase> charactersPodium)
		{
			this.gameObject.SetActive(true);

			listResultDrawers = new List<MenuWinResultDrawer>(charactersPodium.Count);
			listPlayerChoice = new List<int>(charactersPodium.Count);
			for (int i = 0; i < charactersPodium.Count; i++)
			{
				listResultDrawers.Add(Instantiate(prefabResultDrawer, parentResult));
				listPlayerChoice.Add(i);
			}

			winnerID = 0;
			debugVictory.circularReference = this;
			// Créer la liste playerChoice
			// Spawn du victory
			// Spawn des mesh sur les loosersPosition
		}


		// Se lance après la petite ciné de victoire
		public void SetStateResult()
		{
			stateResult = 1;
			for (int i = 0; i < listResultDrawers.Count; i++)
			{
				listResultDrawers[i].DrawResult(i, 0);
			}
		}






		public void UpdateControl(int id, Input_Info input)
		{
			if (stateResult == 2)
				return;
			if (stateResult == 0)
				UpdateControlSkip(id, input);
			else
				UpdateControlResult(id, input);
		}

		private void UpdateControlSkip(int id, Input_Info input)
		{
			if (id == winnerID && input.CheckAction(id, InputConst.Jump))
			{
				SetStateResult();
				debugVictory.SkipCinematic();
			}
		}

		private void UpdateControlResult(int id, Input_Info input)
		{
			if (listPlayerChoice[id] == 0) // Aucun choix de fait  
			{
				if (input.inputUiAction == InputConst.Return)
				{
					listPlayerChoice[id] = 1;
					listResultDrawers[id].SetFeedback("Surrend"); // le surrend a virer et ne pas hardcodé
					CheckEndScreen();
				}
			}
			else // On a fait un choix donc on peut juste l'annuler
			{
				if (input.CheckAction(id, InputConst.Jump))
				{
					listPlayerChoice[id] = 0;
				}
			}
		}




		// On check si chaque joueur a fais un choix, et donc qu'il n'y ait pas d'élément égal à -1 dans la listPlayerChoice
		private void CheckEndScreen()
		{
			bool onePlayerSurrend = false;
			for (int i = 0; i < listPlayerChoice.Count; i++)
			{
				if (listPlayerChoice[i] == 0)
					return;
				else if (listPlayerChoice[i] == 1) // Surrend
					onePlayerSurrend = true;
			}


			if (onePlayerSurrend == true)
			{
				stateResult = 2;
				StartCoroutine(EndCoroutine());
				// Retour au menu
			}
			else
			{
				// Rematch
			}
		}




		private IEnumerator EndCoroutine()
		{
			fadeInTransition.SetTrigger("Feedback");
			yield return new WaitForSeconds(1.2f);
			SceneManager.LoadScene("CharacterSelection");
		}
	}
}