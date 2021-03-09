using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Menu
{
	public class MenuStageSelection : MonoBehaviour, IControllable
	{
		[SerializeField]
		MenuButtonListController listStage;

		[HorizontalGroup("Stages")]
		[ListDrawerSettings(Expanded = true)]
		[SerializeField]
		StageData[] stageDatabase;


		[HorizontalGroup("Stages")]
		[ListDrawerSettings(Expanded = true)]
		[SerializeField]
		Transform[] stagePositions;


		[Title("UI")]
		[SerializeField]
		TextMeshPro text3DStageName;
		[SerializeField]
		TextMeshProUGUI textDescription;
		[SerializeField]
		Image imageStageThumbnail;
		[SerializeField]
		TextMeshProUGUI textDate;


		[Title("Feedback")]
		[SerializeField]
		Transform cameraPivot;
		[SerializeField]
		Image noiseFeedback;

		[SerializeField]
		Animator feedback;





		private IEnumerator stageCoroutine;


		private void Start()
		{
			textDate.text = "Groove City - " + System.DateTime.Now;
			DrawItemList();
		}

		public void DrawItemList()
		{
			for (int i = 0; i < stageDatabase.Length; i++)
			{
				listStage.DrawItemList(i, null, stageDatabase[i].StageName);
			}
			//listStage.SelectIndex(0);
			SelectStage(0);
		}

		public void UpdateControl(int id, Input_Info input)
		{
			if (listStage.InputList(input) == true) // On s'est déplacé dans la liste
				SelectStage(listStage.IndexSelection);
			else if (input.CheckAction(id, InputConst.Interact) == true)
				ValidateStage(listStage.IndexSelection);
			else if (input.CheckAction(id, InputConst.Return) == true)
				QuitMenu();
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

		}

		public void QuitMenu()
		{

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
				cameraPivot.transform.localPosition *= 0.95f;
				yield return null;
			}

			noiseFeedback.gameObject.SetActive(false);
		}


	}
}
