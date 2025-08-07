using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Level Infooo")]
public class LevelInfooo : ScriptableObject
{
    public Sprite sprite;
    public Sprite iconOff;
    public Sprite iconOn;
    public List<ColorStarIDs> colorStarIDs = new List<ColorStarIDs>();
    public List<StarData> starDatas = new();
    public int heartAmount;


    public LevelInfooo()
    {
        colorStarIDs = new List<ColorStarIDs>();
    }

    public int GetMaxStarID()
    {
        int maxx = 0;
        foreach(var colorStars in colorStarIDs)
        {
            foreach(var starID in colorStars.starIDs)
            {
                if(starID > maxx)
                {
                    maxx = starID;
                }
            }
        }
        return maxx;
    }
}


[Serializable]
public class ColorStarIDs
{
    public int id;
    public Color color;
    public List<int> starIDs = new List<int>();

    public ColorStarIDs()
    {
        starIDs = new List<int>();
    }

    public ColorStarIDs(Color color, List<int> starIDs)
    {
        this.color = color;
        this.starIDs = starIDs;
    }
}

[Serializable]
public class StarData
{
    public int id;
    public Vector2 pos;

    public StarData(int id, Vector2 pos)
    {
        this.id = id;
        this.pos = pos;
    }   
}