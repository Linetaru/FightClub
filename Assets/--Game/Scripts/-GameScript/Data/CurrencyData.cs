using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[CreateAssetMenu(fileName = "CurrencyData_", menuName = "Data/CurrencyData", order = 1)]
public class CurrencyData : ScriptableObject, ISavable
{

    const string MoneyVariableName = "Money";

    [SerializeField]
    int money = 0;
    public int Money
    {
        get { return money; }
    }

    // Quand le joueur enchaine les combats, il gagne de l'argent, du coup on save l'argent qu'il a gagné
    // jusqu'a ce qu'il arrive dans un menu, pour le lui montrer
    int moneyToUpdate = 0;
    public int MoneyToUpdate
    {
        get { return moneyToUpdate; }
    }


    private readonly List<IListener<int>> eventListeners = new List<IListener<int>>();



    public void AddMoney(int value)
    {
        money += value;
        moneyToUpdate += value;
        Raise(money);
    }
    public void ResetMoneyToUpdate()
    {
        moneyToUpdate = 0;
    }



    public void Raise(int i_value)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised(i_value);
    }

    public void RegisterListener(IListener<int> listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add(listener);
        }
    }

    public void UnregisterListener(IListener<int> listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove(listener);
        }
    }





    public string GetSaveID()
    {
        return this.name;
    }


    public List<string> GetAllSavesID()
    {
        List<string> ids = new List<string>();
        ids.Add(MoneyVariableName);
        return ids;
    }

    public List<SaveGameVariable> Save()
    {
        List<SaveGameVariable> save = new List<SaveGameVariable>(1);
        save.Add(new SaveGameVariable(MoneyVariableName, money));
        return save;
    }

    public void Load(List<SaveGameVariable> gameVariables)
    {
        for (int i = 0; i < gameVariables.Count; i++)
        {
            if(MoneyVariableName.Equals(gameVariables[i].variableName))
            {
                money = gameVariables[i].variableValue;
                continue;
            }
        }
    }
}
