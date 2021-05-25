using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IListener<T>
{
    void OnEventRaised(T arg);
}


