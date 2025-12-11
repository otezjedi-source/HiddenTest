using UnityEngine;
using UniRx;
using VContainer;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(SpriteRenderer))]
public class GameScene : MonoBehaviour {
    [Inject] private readonly GamePlay gamePlay;
    [Inject] private readonly GameState gameState;
    [Inject] private readonly LevelLoader levelLoader;

    private List<Item> items;

    private readonly CompositeDisposable disposables = new();

    void Awake()
    {
        gamePlay.IsActive
            .Skip(1)
            .Subscribe(_ => UpdateItemsClickability())
            .AddTo(disposables);

        Observable.EveryUpdate()
            .Where(_ => Keyboard.current?.escapeKey.wasPressedThisFrame ?? false)
            .Subscribe(_ => gameState.QuitGame())
            .AddTo(disposables);
    }

    public async UniTask Init()
    {
        levelLoader.UnloadLevel();
        
        var spriteRenderer = GetComponent<SpriteRenderer>();
        await levelLoader.LoadBackgroundAsync(spriteRenderer);

        items = await levelLoader.LoadItemsAsync(transform);
        if (items == null || items.Count == 0)
        {
            Debug.LogError("Failed to load level or no items loaded!");
            return;
        }

        foreach (var item in items)
        {
            item.OnClicked
                .Subscribe(itemData => OnItemClicked(itemData))
                .AddTo(disposables);
        }

        gamePlay.StartGame().Forget();
    }
    
    private void OnDestroy()
    {
        disposables?.Dispose();
        levelLoader?.UnloadLevel();
    }

    private void OnItemClicked(ItemData itemData)
    {
        gamePlay.OnItemFound(itemData);
        UpdateItemsClickability();
    }

    private void UpdateItemsClickability()
    {
        foreach (var item in items)
        {
            bool isClickable = gamePlay.IsActive.Value && gamePlay.CurrentItems.Contains(item.ItemData);
            item.IsClickable = isClickable;
        }
    }
}
