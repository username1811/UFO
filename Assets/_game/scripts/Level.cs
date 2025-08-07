using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Level : MonoBehaviour
{
    public LevelInfooo levelInfooo;
    public int heartAmount;
    public int initialHeartAmount;


    public void OnInitt(int heartAmount)
    {
        initialHeartAmount = heartAmount;
        this.heartAmount = initialHeartAmount;
        this.heartAmount = 999;
    }
    [Button]
    public void SaveToLevelInfoo()
    {
#if UNITY_EDITOR
        string levelPath = AssetDatabase.GetAssetPath(this.levelInfooo);
        if (levelPath.IsNullOrWhitespace())
        {
            levelPath = "Assets/_game/scriptable-objects/level-infos/";
            levelPath += this.gameObject.name;
            levelPath += ".asset";
        }

        LevelInfooo levelInfooo = AssetDatabase.LoadAssetAtPath<LevelInfooo>(levelPath);


        if (levelInfooo == null)
        {
            levelInfooo = ScriptableObject.CreateInstance<LevelInfooo>();
            this.levelInfooo = levelInfooo; 
            AssetDatabase.CreateAsset(levelInfooo, levelPath);
            AssetDatabase.SaveAssets();
        }

        levelInfooo.heartAmount = 3;
        EditorUtility.SetDirty(levelInfooo);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("save level data: " + levelInfooo.name);
#endif
    }

}
