using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BallBase : MonoBehaviour
{
	[SerializeField]
	CharacterBase ball;

	Input_Info input;

    private void Start()
    {
        input = new Input_Info();
    }

    private void Update()
    {
        ball.UpdateControl(0, input);
    }
}
