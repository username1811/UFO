using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    public int amount;
    public float spacing;
    public List<Transform> objs = new();




    private void Update()
    {
        if (objs.All(x => !x.gameObject.activeInHierarchy))
        {
            SpawnSomeObjects();
        }
    }

    public void SpawnSomeObjects()
    {
        objs.Clear();   
        for (int i = 0; i < amount; i++)
        {
            for (int j = 0; j < amount; j++)
            {
                Transform box = PoolManager.Ins.Spawn<Transform>(PoolType.Box);
                box.transform.position = new Vector3(i * spacing, 0, j * spacing) + new Vector3(5,0,5);
                box.transform.SetParent(this.transform);
                objs.Add(box);
            }
        }
    }
}
