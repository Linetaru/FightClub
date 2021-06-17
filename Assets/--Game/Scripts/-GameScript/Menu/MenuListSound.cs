using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu 
{
	[RequireComponent(typeof(AkGameObj))]
	public class MenuListSound : MonoBehaviour
	{
		[SerializeField]
		MenuList menuList = null;

		[SerializeField]
		AK.Wwise.Event selectedSound = null;
		[SerializeField]
		AK.Wwise.Event validateSound = null;
		[SerializeField]
		AK.Wwise.Event backSound = null;

		private void Start()
		{
			menuList.OnSelected += PlaySoundSelected;
			menuList.OnValidate += PlaySoundValidate;
			menuList.OnEnd += PlaySoundQuit;
		}


		private void PlaySoundSelected(int i)
		{
			if (selectedSound.IsValid())
				AkSoundEngine.PostEvent(selectedSound.Id, this.gameObject);
		}

		private void PlaySoundValidate(int i)
		{
			if (validateSound.IsValid())
				AkSoundEngine.PostEvent(validateSound.Id, this.gameObject);
		}
		private void PlaySoundQuit()
		{
			if(backSound.IsValid())
				AkSoundEngine.PostEvent(backSound.Id, this.gameObject);
		}


		private void OnDestroy()
		{
			menuList.OnSelected -= PlaySoundSelected;
			menuList.OnValidate -= PlaySoundValidate;
			menuList.OnEnd -= PlaySoundQuit;
		}
	}
}
