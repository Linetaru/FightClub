using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavable
{
	string GetSaveID();
    List<string> GetAllSavesID();


    List<SaveGameVariable> Save();
    void Load(List<SaveGameVariable> saves);


}

