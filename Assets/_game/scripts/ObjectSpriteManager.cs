using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectSpriteManager : Singleton<ObjectSpriteManager>
{
    public List<ObjectSprite> objectSprites = new List<ObjectSprite>(); 

    public Sprite GetSprite(TargetObjectType type)
    {
        return objectSprites.FirstOrDefault(x => x.targetObjectType == type).sprite;
    }
}

[Serializable]
public class ObjectSprite
{
    public TargetObjectType targetObjectType;
    public Sprite sprite;
}
