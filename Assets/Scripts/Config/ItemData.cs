using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public class ItemData
{
    public string id;
    public string displayName;
    public bool isActive = true;
    public Sprite icon;
    public AssetReference itemRef;
}
