using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using VContainer;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour
{
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private UIItem itemPrefab;
    [SerializeField] private GameObject timer;
    [SerializeField] private TMP_Text timerValue;
    [SerializeField] private Button btnSettings;

    [Inject] private readonly UI ui;
    [Inject] private readonly GamePlay gamePlay;
    [Inject] private readonly Settings settings;
    
    private UIItemsPool itemsPool;
    private readonly List<UIItem> items = new();
    private readonly CompositeDisposable disposables = new();

    private void Awake()
    {
        itemsPool = new(itemPrefab, itemsContainer);

        gamePlay.IsActive
            .Where(active => active)
            .Subscribe(_ => OnGameStarted())
            .AddTo(disposables);

        gamePlay.OnItemReplaced
            .Subscribe(data => UpdateItem(data.oldItemData, data.newItemData))
            .AddTo(disposables);

        gamePlay.TimeLeft
            .Subscribe(time => UpdateTimer(time))
            .AddTo(disposables);

        gamePlay.GameResult
            .Where(result => result != GamePlay.Result.None)
            .Subscribe(result => ui.ShowGameResult(result))
            .AddTo(disposables);

        btnSettings.OnClickAsObservable()
            .Subscribe(_ => ui.ShowSettingsPanel())
            .AddTo(disposables);
    }

    private void OnGameStarted()
    {
        itemsPool.ReturnAll(items);

        for (int i = 0; i < gamePlay.CurrentLevel.maxCurrentItems; i++)
            items.Add(itemsPool.Get());
        
        UpdateItems();
    }

    private void UpdateItems()
    {
        for (int i = 0; i < items.Count; i++)
        {
            var itemData = gamePlay.CurrentItems[i];
            items[i].Init(itemData, settings);
        }
    }

    private void UpdateItem(ItemData oldItemData, ItemData newItemData)
    {
        var idx = items.FindIndex(i => i.ItemData == oldItemData);
        if (idx >= 0)
            items[idx].Init(newItemData, settings);
    }

    private void UpdateTimer(float time)
    {
        timer.SetActive(gamePlay.CurrentLevel.IsTimerEnabled);
        
        if (gamePlay.CurrentLevel.IsTimerEnabled )
        {
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            timerValue.text = $"{minutes:00}:{seconds:00}";

            timerValue.color = time < 10f ? Color.red : Color.white;
        }
    }

    private void OnDestroy()
    {
        disposables?.Dispose();
        
        foreach (var item in items)
        {
            if (item != null)
                Destroy(item.gameObject);
        }
        
        itemsPool?.Clear();
    }
}
