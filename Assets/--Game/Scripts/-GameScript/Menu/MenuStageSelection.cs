using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace Menu
{
	public class MenuStageSelection : MonoBehaviour, IControllable
	{
		[SerializeField]
		MenuButtonListController listStage = null;

		[SerializeField]
		GameData gameData = null;

		[Title("Transforms")]
		[ListDrawerSettings(Expanded = true)]
		[SerializeField]
		Transform[] stagePositions = null;


		[Title("UI")]
		[SerializeField]
		TextMeshPro text3DStageName = null;
		[SerializeField]
		TextMeshProUGUI textDescription = null;
		[SerializeField]
		Image imageStageThumbnail = null;
		[SerializeField]
		TextMeshProUGUI textDate = null;


		[Title("Feedback")]
		[SerializeField]
		Transform cameraPivot = null;
		[SerializeField]
		Image noiseFeedback = null;

		[SerializeField]
		Animator feedback = null;

		[SerializeField]
		Animator fadeIn = null;
		[SerializeField]
		Animator animatorCamera = null;

		bool canControl = true;
		private IEnumerator stageCoroutine;
		[Scene]
		public string menuSelectionPersoScene;
		int characterID = 0;

		List<StageData> stageDatabase = null;

		private void Start()
		{
			textDate.text = "Groove City - " + System.DateTime.Now;

			if(gameData.GameSetting == null)
				gameData.SetGameSettings();

			stageDatabase = new List<StageData>();
			for (int i = 0; i < gameData.GameSetting.StagesAvailable.Database.Count; i++)
			{
				if(gameData.GameSetting.StagesAvailable.GetUnlocked(i))
				{
					stageDatabase.Add(gameData.GameSetting.StagesAvailable.Database[i]);
				}
			}

			// On a un problème on ne devrait pas être là si on a aucun stage de débloqué, donc je les débloque tous en mode cheat code
			if (stageDatabase.Count == 0)
			{
				for (int i = 0; i < gameData.GameSetting.StagesAvailable.Database.Count; i++)
				{
					stageDatabase.Add(gameData.GameSetting.StagesAvailable.Database[i]);
				}
			}

			DrawItemList();
		}

		public void DrawItemList()
		{
			for (int i = 0; i < stageDatabase.Count; i++)
			{
				listStage.DrawItemList(i, null, stageDatabase[i].StageName);
			}
			SelectStage(0);
			listStage.SelectIndex(0);
		}

		public void UpdateControl(int id, Input_Info input)
		{	
			if (!canControl)
				return;

			if (characterID == id)
			{
				if (listStage.InputList(input)) // On s'est déplacé dans la liste
					SelectStage(listStage.IndexSelection);
				else if (input.inputUiAction == InputConst.Interact)
					ValidateStage(listStage.IndexSelection);
				else if (input.inputUiAction == InputConst.Return)
					QuitMenu();
			}
			else if (Mathf.Abs(input.vertical) > 0.2f)
				characterID = id;
		}







		public void SelectStage(int id)
		{
			if (stageCoroutine != null)
				StopCoroutine(stageCoroutine);
			stageCoroutine = CameraCoroutine(id);
			StartCoroutine(stageCoroutine);


			feedback.SetTrigger("Feedback");
			feedback.transform.SetParent(stagePositions[id]);
			feedback.transform.localPosition = Vector3.zero;

			// UI
			text3DStageName.text = stageDatabase[id].StageName;
			text3DStageName.transform.SetParent(stagePositions[id]);

			imageStageThumbnail.sprite = stageDatabase[id].StageThumbnail;
			textDescription.text = stageDatabase[id].StageDescription;
		}

		public void ValidateStage(int id)
		{
			canControl = false;
			StartCoroutine(StageSelectedCoroutine(id));
		}

		public void QuitMenu()
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(menuSelectionPersoScene);
		}



		// ============================================================
		// Partie graphique

		private IEnumerator CameraCoroutine(int newIndex)
		{
			noiseFeedback.gameObject.SetActive(true);

			cameraPivot.SetParent(stagePositions[newIndex]);
			float t = 0;
			while(t < 1)
			{
				t += Time.deltaTime * 3;
				cameraPivot.transform.localPosition = Vector3.Lerp(cameraPivot.transform.localPosition, Vector3.zero, 0.25f) ;
				yield return null;
			}

			noiseFeedback.gameObject.SetActive(false);
		}


		private IEnumerator StageSelectedCoroutine(int id)
		{
			fadeIn.SetTrigger("Feedback");
			animatorCamera.SetTrigger("Feedback");

			float t = 0;
			while (t < 1)
			{
				t += Time.deltaTime;
				yield return null;
			}
			SceneManager.LoadScene(stageDatabase[id].SceneName);
		}


	}
}
