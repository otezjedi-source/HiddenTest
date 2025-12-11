using System;
using System.Collections.Generic;
using UniRx;

public class ItemsManager : IDisposable
{
    public readonly Subject<(ItemData oldData, ItemData newData)> OnItemReplaced = new();

    public Queue<ItemData> ItemsQueue { get; private set; }
    public List<ItemData> CurrentItems { get; private set; }

    public void Initialize(LevelConfig levelConfig)
    {
        ItemsQueue = new(levelConfig.ActiveItems);
        CurrentItems = new();

        while (CurrentItems.Count < levelConfig.maxCurrentItems && ItemsQueue.TryDequeue(out var item))
            CurrentItems.Add(item);
    }

    public bool TryReplaceItem(ItemData foundItem)
    {
        var idx = CurrentItems.IndexOf(foundItem);
        if (idx < 0)
            return false;

        var oldData = CurrentItems[idx];
        CurrentItems[idx] = ItemsQueue.TryDequeue(out var newItem) ? newItem : null;

        OnItemReplaced.OnNext((oldData, CurrentItems[idx]));

        return !CurrentItems.Exists(i => i != null);
    }

    public void Dispose()
    {
        OnItemReplaced?.Dispose();
    }
}
