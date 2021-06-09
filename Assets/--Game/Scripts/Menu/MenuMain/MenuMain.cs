using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Menu
{
	public class MenuMain : MonoBehaviour, IControllable
	{
		[SerializeField]
		GameData gameData;
		[SerializeField]
		InputController inputController;



		/*[Title("Menu")]
		[SerializeField]
		MenuList menu;
		[SerializeField]
		MenuList[] menuList;*/

		// Start is called before the first frame update
		void Start()
		{
			CheckMenuStartup();
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

			}
		}
	}
}
