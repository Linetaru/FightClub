using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AkGameObj))]
public class SoundListPlayer : MonoBehaviour
{
	[SerializeField]
	List<AK.Wwise.Event> events = new List<AK.Wwise.Event>();

	public void PlaySound(int id)
	{
		AkSoundEngine.PostEvent(events[id].Id, this.gameObject);
	}
} 
