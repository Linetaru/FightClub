using System.Collections;
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
		GameData gameData;
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

		public enum StateResult{
			Skip = 0,
			Result = 1,
			EndTransition = 2,
        }

		StateResult stateResult;

		//int stateResult = 0; // 0 = skip // 1 = result // 2 = transition de fin (faire un enum)

		int winnerID = -1;
		List<MenuWinResultDrawer> listResultDrawers;
		List<int> listPlayerChoice; // Choix de surrend ou rematch



		public void InitializeWin(List<CharacterBase> charactersPodium)
		{
			this.gameObject.SetActive(true);

			listResultDrawers = new List<MenuWinResultDrawer>(charactersPodium.Count);
			listPlayerChoice = new List<int>(charactersPodium.Count);


			// On instancie le winner
			listResultDrawers.Add(Instantiate(prefabResultDrawer, parentResult));
			listPlayerChoice.Add(0);

			int winnerID = charactersPodium[0].PlayerID;
			VictoryScreen victoryScreen = Instantiate(gameData.CharacterInfos[winnerID].CharacterData.victoryScreen, winnerPosition);
			victoryScreen.characterModel.SetColor(winnerID, gameData.CharacterInfos[winnerID].CharacterData.characterMaterials[gameData.CharacterInfos[winnerID].CharacterColorID]);
			victoryScreen.circularReference = this;


			// On instancie les loosers
			for (int i = 1; i < charactersPodium.Count; i++)
			{

				listResultDrawers.Add(Instantiate(prefabResultDrawer, parentResult));
				listPlayerChoice.Add(0);

				// Aled
				Character_Info characterInfo = gameData.CharacterInfos[charactersPodium[i].PlayerID];
				CharacterModel looser = Instantiate(characterInfo.CharacterData.looserModel, loosersPosition[i - 1]);
				looser.SetColor(charactersPodium[i].PlayerID, characterInfo.CharacterData.characterMaterials[characterInfo.CharacterColorID]);
			}
		}


		// Se lance après la petite ciné de victoire
		public void SetStateResult()
		{
			stateResult = StateResult.Result;
			for (int i = 0; i < listResultDrawers.Count; i++)
			{
				listResultDrawers[i].DrawResult(i+1, i+1);
			}
		}


		public void UpdateControl(int id, Input_Info input)
		{
			if (stateResult == StateResult.EndTransition)
				return;
			if (stateResult == StateResult.Skip)
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
			if (listPlayerChoice.Count <= id)
				return;

			if (listPlayerChoice[id] == 0) // Aucun choix de fait  
			{
				if (input.inputUiAction == InputConst.Return)
				{
					listPlayerChoice[id] = 1;
					listResultDrawers[id].SetFeedback("Surrend"); // le surrend a virer et ne pas hardcodé
					CheckEndScreen();
				}
				else if(input.inputUiAction == InputConst.Interact)
                {
					listPlayerChoice[id] = 2;
					listResultDrawers[id].SetFeedback("Rematch"); // le rematch a virer et ne pas hardcodé
					if (input.CheckAction(0, InputConst.Jump))
						input.inputActions[0].timeValue = 0;
					CheckEndScreen();
				}
			}
			else // On a fait un choix donc on peut juste l'annuler
			{
				if (input.CheckAction(0, InputConst.Jump))
				{
					listPlayerChoice[id] = 0;
					listResultDrawers[id].SetReverseFeedback();
				}
			}
		}


		// On check si chaque joueur a fais un choix, et donc qu'il n'y ait pas d'élément égal à -1 dans la listPlayerChoice
		private void CheckEndScreen()
		{
			int playerSurrend = 0;
			int playerRematch = 0;
			for (int i = 0; i < listPlayerChoice.Count; i++)
			{
				if (listPlayerChoice[i] == 0)
					return;
				else if (listPlayerChoice[i] == 1) // Surrend
					playerSurrend += 1;
				else if (listPlayerChoice[i] == 2)
					playerRematch += 1;
			}

			stateResult = StateResult.EndTransition;

			if (playerSurrend > 0)
			{
				StartCoroutine(EndCoroutine());
				// Retour au menu
			}
			else
			{
				StartCoroutine(RematchCoroutine());
				// Rematch
			}
		}

		private IEnumerator RematchCoroutine()
		{
			fadeInTransition.SetTrigger("Feedback");
			yield return new WaitForSeconds(1.2f);
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}


		private IEnumerator EndCoroutine()
		{
			fadeInTransition.SetTrigger("Feedback");
			yield return new WaitForSeconds(1.2f);
			SceneManager.LoadScene("CharacterSelection");
		}
	}
}
