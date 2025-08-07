using UnityEngine;

public class TargetObjectInfo 
{
    public TargetObjectType targetObjectType;
    public int amount;

    public TargetObjectInfo() { }

    public TargetObjectInfo(TargetObjectType targetObjectType, int amount)
    {
        this.targetObjectType = targetObjectType;
        this.amount = amount;
    }
}

public enum TargetObjectType
{
    Box, Circle, Capsule, 
}