using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateSmashPressed : CharacterState
{
    [SerializeField]
    CharacterState idleState;
    [SerializeField]
    CharacterState smashRelease;

    [SerializeField]
    private float timeToReleaseSmash = 3.0f;

    public override void StartState(CharacterBase character, CharacterState oldState)
    {
        Debug.Log("Charging State");
        StartCoroutine(Charging(character));
	}

	public override void UpdateState(CharacterBase character)
	{

    }
    public override void EndState(CharacterBase character, CharacterState oldState)
    {
        //character.SetState(idleState);
    }

    public IEnumerator Charging(CharacterBase character)
    {
        yield return new WaitForSecondsRealtime(timeToReleaseSmash);
        Debug.Log("Smash Release"); 
        character.SetState(idleState);
        //character.SetState(smashRelease);
    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
