using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllable
{
    void UpdateControl(int ID, Input_Info input_Info);
}