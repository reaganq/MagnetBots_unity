using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ArmorAttribute {

    public ArmorAttributeName attributeName;
    public float attributeValue;
}

[Serializable]
public class ArmorAttributeMultiplier
{
    public int attributeIndex;
    public float attributeMultiplier = 1.0f;

}

public enum ArmorAttributeName
{
    damage,
    cooldown,
    reloadTime,
    ammoClipSize,
    limitedUse,
}