using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using AK.Wwise;

public class AttackC_Sounds : AttackComponent
{

	/*[SerializeField]
	AK.Wwise.Event soundEvent;*/
	[SerializeField]
	AK.Wwise.Event hitSound;


	// Appelé au moment où l'attaque est initialisé
	public override void StartComponent(CharacterBase user)
    {
		//AkSoundEngine.PostEvent(soundEvent.Id, this.gameObject);
	}
	
	// Appelé tant que l'attaque existe 
	//(Peut-être remplacé par l'Update d'Unity de base si l'ordre d'éxécution n'est pas important)
	public override void UpdateComponent(CharacterBase user)
    {
    }
	
	// Appelé au moment où l'attaque touche une target
    public override void OnHit(CharacterBase user, CharacterBase target)
    {
		AkSoundEngine.PostEvent(hitSound.Id, this.gameObject);
	}
	
	// Appelé au moment de la destruction de l'attaque
    public override void EndComponent(CharacterBase user)
    {
		
    }
}
