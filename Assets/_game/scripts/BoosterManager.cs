using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoosterManager : Singleton<BoosterManager>
{
    public bool isUsingBooster;
    public Booster currentBooster;
    [SerializeReference]
    public List<Booster> boosters = new();


    public void OnLoadLevel()
    {
        
    }

    public void UseBooster(BoosterType boosterType)
    {
        currentBooster = boosters.FirstOrDefault(x=>x.boosterType == boosterType);
        if (currentBooster != null)
        {
            currentBooster.Use();
        }
    }
}

public enum BoosterType {
    Hint, Time, 
}