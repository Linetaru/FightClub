using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputMappingDataStatic
{
    public static List<InputMappingDataClassic> inputMappingDataClassics = new List<InputMappingDataClassic>();
}

[System.Serializable]
public class InputMappingDataClassic
{
    [SerializeField]
    public string profileName = "";

    [SerializeField]
    public EnumInput inputJump = EnumInput.A;
    [SerializeField]
    public EnumInput inputShortHop = EnumInput.B;
    [SerializeField]
    public EnumInput inputAttack = EnumInput.X;
    [SerializeField]
    public EnumInput inputSpecial = EnumInput.Y;

    [SerializeField]
    public EnumInput inputParry = EnumInput.R1;
    [SerializeField]
    public EnumInput inputDash = EnumInput.R2;

    public InputMappingDataClassic(string st)
    {
        profileName = st;
        inputJump = EnumInput.A; 
        inputShortHop = EnumInput.B;
        inputAttack = EnumInput.X;
        inputSpecial = EnumInput.Y;
        inputParry = EnumInput.R1;
        inputDash = EnumInput.R2;
    }
}
