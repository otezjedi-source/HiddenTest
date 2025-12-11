using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;
using VContainer;

public class LevelLoader
{
    [Inject] private readonly GameConfig gameConfig;
    [Inject] private readonly GamePlay gamePlay;

    private List<Item> loadedItems = new();
    private readonly List<AsyncOperationHandle> handles = new();

    public async UniTask LoadBackgroundAsync(SpriteRenderer spriteRenderer)
    {
        var levelConfig = gamePlay.CurrentLevel;
        if (levelConfig.backgroundRef == null || !levelConfig.backgroundRef.RuntimeKeyIsValid())
        {
            Debug.LogWarning("Background reference is not set or invalid!");
            return;
        }

        try
        {
            var handle = levelConfig.backgroundRef.LoadAssetAsync<Sprite>();
            handles.Add(handle);
            spriteRenderer.sprite = await handle.ToUniTask();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load background: {ex.Message}");
        }
    }

    public async UniTask<List<Item>> LoadItemsAsync(Transform parent)
    {
        var activeItems = gamePlay.CurrentLevel.ActiveItems;
        if (activeItems.Count == 0)
        {
            Debug.LogWarning("No active items to load!");
            return loadedItems;
        }

        var tasks = new List<UniTask<Item>>();
        foreach (var itemData in activeItems)
        {
            var item = LoadItemAsync(itemData, parent);
            tasks.Add(item);
        }

        var result = await UniTask.WhenAll(tasks);
        loadedItems = new(result);
        return loadedItems;
    }

    private async UniTask<Item> LoadItemAsync(ItemData itemData, Transform parent)
    {
        if (itemData.itemRef == null || !itemData.itemRef.RuntimeKeyIsValid())
        {
            Debug.LogWarning($"Item reference for '{itemData.id}' is not set or invalid!");
            return null;
        }

        try
        {
            var handle = itemData.itemRef.InstantiateAsync(parent, true);
            handles.Add(handle);

            var itemObject = await handle.ToUniTask();
            return InitItem(itemObject, itemData);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load item '{itemData.id}': {ex.Message}");
            return null;
        }
    }

    private Item InitItem(GameObject itemObject, ItemData itemData)
    {
        if (!itemObject.TryGetComponent<Item>(out var item))
            item = itemObject.AddComponent<Item>();
        item.Init(itemData, gameConfig);
        return item;
    }

    public void UnloadLevel()
    {
        foreach (var item in loadedItems)
        {
            if (item != null)
                Object.Destroy(item);
        }
        loadedItems.Clear();

        foreach (var handle in handles)
        {
            if (handle.IsValid())
                Addressables.Release(handle);
        }
        handles.Clear();
    }
}
