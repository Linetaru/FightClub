using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStats
{
    Stats GetStat(MainStat mainStat);
    void ApplyStatModifs(MainStat mainStat);
}
