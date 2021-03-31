using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Menu
{
	public class VictoryScreen : MonoBehaviour
	{
		[SerializeField]
		float timeSkip;
		[SerializeField]
		PlayableDirector playableDirector;
		[SerializeField]
		public CharacterModel characterModel;

		[HideInInspector]
		public MenuWin circularReference; // Flemme de faire un event il est 1h du mat



		public void SkipCinematic()
		{
			playableDirector.time = timeSkip;
		}

		public void SetToResult()
		{
			circularReference.SetStateResult();
		}
	}
}
