using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "HiddenTest/Level Config")]
public class LevelConfig : ScriptableObject
{
    public AssetReference backgroundRef;
    public float timerSec = 120f;
    public int maxCurrentItems = 3;
    public ItemDisplayMode.Type itemDisplayMode = ItemDisplayMode.Type.Text;
    public List<ItemData> items;

    public bool IsTimerEnabled => timerSec > 0;

    [NonSerialized] private ReadOnlyCollection<ItemData> activeItems = null;
    public ReadOnlyCollection<ItemData> ActiveItems
    {
        get
        {
            if (activeItems == null)
                RebuildActiveItems();
            return activeItems;
        }
    }

    public void RebuildActiveItems()
    {
        var filtered = items.FindAll(i => i.isActive);
        activeItems = filtered.AsReadOnly();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        RebuildActiveItems();
    }
#endif
}
