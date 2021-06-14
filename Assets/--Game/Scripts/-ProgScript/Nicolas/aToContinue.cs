using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class aToContinue : MonoBehaviour
{
    public TextMeshProUGUI textToContinue;
    public Animator animator;

    const string DEFAULT_TEXT = "To Continue";

    public void AddOneChar()
    {
        if (textToContinue.text.Length == 0)
            textToContinue.text = DEFAULT_TEXT.Length.ToString();
        else if (textToContinue.text.Length <= DEFAULT_TEXT.Length)
            textToContinue.text = textToContinue.text.Insert(0, DEFAULT_TEXT[DEFAULT_TEXT.Length - textToContinue.text.Length].ToString());
        //else
        //    textToContinue.text = textToContinue.text.Insert(0, DEFAULT_TEXT[0].ToString());
    }

    public void RemoveOneChar()
    {
        if (textToContinue.text.Length > 0)
            textToContinue.text = textToContinue.text.Remove(0);
    }
}
