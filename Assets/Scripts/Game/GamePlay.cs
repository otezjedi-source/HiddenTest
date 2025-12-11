using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using VContainer;
using System;
using System.Collections.Generic;

public class GamePlay
{
    public enum Result
    {
        None,
        Victory,
        Defeat
    }

    [Inject] private readonly GameState state;
    [Inject] private readonly GameTimer timer;
    [Inject] private readonly ItemsManager itemsManager;

    public readonly ReactiveProperty<bool> IsActive = new(false);
    public readonly ReactiveProperty<Result> GameResult = new(Result.None);
    public IReadOnlyReactiveProperty<float> TimeLeft => state.TimeLeft;
    public IObservable<(ItemData oldItemData, ItemData newItemData)> OnItemReplaced => itemsManager.OnItemReplaced;

    public LevelConfig CurrentLevel => state.CurrentLevel;
    public Queue<ItemData> ItemsQueue => itemsManager.ItemsQueue;
    public List<ItemData> CurrentItems => itemsManager.CurrentItems;

    public async UniTaskVoid StartGame()
    {
        GameResult.Value = Result.None;

        itemsManager.Initialize(CurrentLevel);

        if (itemsManager.ItemsQueue.Count == 0)
        {
            Debug.LogError("No active items!");
            return;
        }

        IsActive.Value = true;

        if (CurrentLevel.IsTimerEnabled)
        {
            bool timedOut = await timer.StartAsync(CurrentLevel.timerSec, IsActive);
            if (timedOut && IsActive.Value)
                EndGame(Result.Defeat);
        }
    }

    public void OnItemFound(ItemData itemData)
    {
        if (!IsActive.Value)
            return;

        bool allFound = itemsManager.TryReplaceItem(itemData);
        if (allFound)
            EndGame(Result.Victory);
    }

    private void EndGame(Result result)
    {
        IsActive.Value = false;
        GameResult.Value = result;

        if (result == Result.Victory)
            state.OnLevelCompleted();
        else if (result == Result.Defeat)
            state.OnLevelFailed();
    }

    public void QuitGame()
    {
        IsActive.Value = false;
    }
}
