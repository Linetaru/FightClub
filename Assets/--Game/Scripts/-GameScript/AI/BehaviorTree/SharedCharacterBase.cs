using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviorDesigner.Runtime
{
	[System.Serializable]
	public class SharedCharacterBase : SharedVariable<CharacterBase>
	{
		public static implicit operator SharedCharacterBase(CharacterBase value) { return new SharedCharacterBase { mValue = value }; }
	}
}
