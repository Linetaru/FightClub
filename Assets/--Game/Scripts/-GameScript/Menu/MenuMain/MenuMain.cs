using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace Menu
{
	public class MenuMain : MonoBehaviour, IControllable
	{
		[SerializeField]
		GameData gameData = null;
		[SerializeField]
		InputController inputController = null;

		[Title("UI")]
		[SerializeField]
		Canvas canvasStart = null;
		[SerializeField]
		Canvas canvasButton = null;
		[SerializeField]
		Canvas canvasOption = null;
		[SerializeField]
		MenuCurrency canvasMoney = null;

		[Title("Feedbacks")]
		[SerializeField]
		Animator animatorCamera = null;
		[SerializeField]
		GameObject fade = null;

		[Title("Menu")]
		[SerializeField]
		MenuList menuSlam = null;
		[SerializeField]
		MenuList menuModes = null;
		[SerializeField]
		MenuList menuExtra = null;
		[SerializeField]
		MenuList menuTraining = null;

		[Title("Events")]
		[SerializeField]
		UnityEvent unityEventStart = null;

		// Start is called before the first frame update
		void Start()
		{
			CheckMenuStartup();
			Cursor.visible = false;
		}


		private void CheckMenuStartup()
		{
			// Check Game Mode
			// Set Camera Instant
			// Set inputs
			for (int i = 0; i < inputController.controllable.Length; i++)
			{
				//inputController.controllable[i] = null;
			}
		}



		public void UpdateControl(int id, Input_Info input)
		{
			if (input.inputUiAction == InputConst.Pause)
			{
				input.inputUiAction = null;
				canvasStart.gameObject.SetActive(false);
				canvasButton.gameObject.SetActive(true);

				canvasMoney.animatorPanel.gameObject.SetActive(true);
				canvasMoney.animatorPanel.SetBool("Appear", true);

				unityEventStart.Invoke();
			}
		}


		public void SetCanvasStartAgain(bool delayed)
        {
			if(delayed)
				canvasStart.gameObject.SetActive(true);
			canvasButton.gameObject.SetActive(false);
			canvasMoney.animatorPanel.SetBool("Appear", false);
		}



		// Utilisé par des Unity Event
		public void QuitGame()
		{
			SaveManager.Instance.SaveFile();
			Application.Quit();
		}

		public void LockInput(float time)
		{

		}

		public void ReplaceByMenuMainAllControllable()
		{
			for (int i = 0; i < inputController.controllable.Length; i++)
			{
				inputController.controllable[i] = this;
			}
		}


		public void SetGameMode(int gameModeID)
		{
			gameData.GameMode = (GameModeStateEnum)gameModeID;
			gameData.SetGameSettings();
		}

		public void LoadScene(string sceneName)
		{
			fade.gameObject.SetActive(true);
			StartCoroutine(LoadSceneCoroutine(sceneName));
		}
		private IEnumerator LoadSceneCoroutine(string sceneName)
		{
			yield return new WaitForSeconds(0.5f);
			UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
		}

		public void SetControl(MenuList menu)
		{
			for (int i = 0; i < inputController.controllable.Length; i++)
			{
				inputController.controllable[i] = menu;
			}
		}


		public void MoveCamera(int id)
		{
			animatorCamera.SetInteger("State", id);
		}
	}
}
