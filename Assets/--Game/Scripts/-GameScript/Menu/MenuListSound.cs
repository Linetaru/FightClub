using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu 
{
	[RequireComponent(typeof(AkGameObj))]
	public class MenuListSound : MonoBehaviour
	{
		[SerializeField]
		MenuList menuList;

		[SerializeField]
		AK.Wwise.Event selectedSound;
		[SerializeField]
		AK.Wwise.Event validateSound;

		private void Start()
		{
			menuList.OnSelected += PlaySoundSelected;
			menuList.OnValidate += PlaySoundValidate;
		}


		private void PlaySoundSelected(int i)
		{
			AkSoundEngine.PostEvent(selectedSound.Id, this.gameObject);
		}

		private void PlaySoundValidate(int i)
		{
			AkSoundEngine.PostEvent(validateSound.Id, this.gameObject);
		}



		private void OnDestroy()
		{
			menuList.OnSelected -= PlaySoundSelected;
			menuList.OnValidate -= PlaySoundValidate;
		}
	}
}
