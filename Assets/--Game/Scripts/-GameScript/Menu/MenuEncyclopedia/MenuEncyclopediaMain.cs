using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Menu
{
	public class MenuEncyclopediaMain : MenuList
	{
		[Title("")]
		[SerializeField]
		InputController inputController;
		[SerializeField]
		MenuList[] menuList;

		[Title("Animator")]
		[SerializeField]
		Animator animatorButtons;
		[SerializeField]
		Animator animatorRenderTexture;

		[Title("Animator")]
		[SerializeField]
		[Scene]
		string mainMenuScene;

		private IEnumerator moveButtonCoroutine = null;


		private void Awake()
		{
			for (int i = 0; i < menuList.Length; i++)
			{
				menuList[i].OnEnd += BackToMenu;
			}
		}

		private void OnDestroy()
		{
			for (int i = 0; i < menuList.Length; i++)
			{
				menuList[i].OnEnd -= BackToMenu;
			}
		}

		private void Start()
		{
			listEntry.SelectIndex(1);
			//SelectEntry(1);
		}

		public override void UpdateControl(int id, Input_Info input)
		{
			if (listEntry.InputListHorizontal(input) == true)
			{
				SelectEntry(listEntry.IndexSelection);
			}
			else if (input.inputUiAction == InputConst.Interact)
			{
				input.inputUiAction = null;
				ValidateEntry(listEntry.IndexSelection);
			}
			else if (input.inputUiAction == InputConst.Return)
			{
				QuitMenu();
			}
		}

		protected override void QuitMenu()
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuScene);
		}

		protected override void ValidateEntry(int id)
		{
			base.ValidateEntry(id);

			for (int i = 0; i < menuList.Length; i++)
			{
				menuList[i].gameObject.SetActive(false);
			}
			menuList[id].gameObject.SetActive(true);

			inputController.controllable[0] = menuList[id];
			menuList[id].InitializeMenu();

			animatorRenderTexture.SetBool("Appear", true);
			animatorRenderTexture.gameObject.SetActive(true);
		}

		protected override void SelectEntry(int id)
		{
			base.SelectEntry(id);

			if (moveButtonCoroutine != null)
				StopCoroutine(moveButtonCoroutine);
			moveButtonCoroutine = MoveButtons(0.5f * id, 0.5f);
			StartCoroutine(moveButtonCoroutine);
		}


		private IEnumerator MoveButtons(float finalValue, float time)
		{
			float t = 0f;
			float baseValue = animatorButtons.GetFloat("Blend");
			float value = baseValue;
			while (t<1)
			{
				t += (Time.deltaTime / time);
				value = Mathf.Lerp(value, finalValue, t);
				animatorButtons.SetFloat("Blend", value);
				yield return null;
			}
			moveButtonCoroutine = null;
		}




		private void BackToMenu()
		{
			inputController.controllable[0] = this;
			animatorRenderTexture.SetBool("Appear", false);
		}


		private void ShowMenu()
		{

		}




	}
}
