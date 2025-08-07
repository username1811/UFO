using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "level_wrapper", menuName = "Scriptable Objects/Level/Level Wrapper")]
public class LevelWrapperrr : ScriptableObject
{
    [ListDrawerSettings(NumberOfItemsPerPage = 10)]
    public LevelInfooo tutLevelInfo;
    public List<LevelInfooo> levels;

    [Button]
    public void SortByShapeCount()
    {
#if UNITY_EDITOR
        int n = levels.Count;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (levels[j].colorStarIDs.Count > levels[j + 1].colorStarIDs.Count)
                {
                    var temp = levels[j];
                    levels[j] = levels[j + 1];
                    levels[j + 1] = temp;
                }
            }
        }
        AssetDatabase.SaveAssets();
#endif
    }

    [Button]
    public void SortByStarCount()
    {
#if UNITY_EDITOR
        int n = levels.Count;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (levels[j].starDatas.Count > levels[j + 1].starDatas.Count)
                {
                    var temp = levels[j];
                    levels[j] = levels[j + 1];
                    levels[j + 1] = temp;
                }
            }
        }
        AssetDatabase.SaveAssets();
#endif
    }
     
    [Button]
    public void SortByName()
    {
#if UNITY_EDITOR
        var sortedList = levels.OrderBy(p => p.name).ToList();
        levels = sortedList;
        AssetDatabase.SaveAssets();
#endif
    }
    /*
        [Button]
        public void DuplicateLevels()
        {
    #if UNITY_EDITOR
            levels.AddRange(levels);
            AssetDatabase.SaveAssets();
    #endif
        }*/
}
