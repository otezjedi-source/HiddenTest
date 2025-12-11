using System;
using Cysharp.Threading.Tasks;
using UniRx;
using VContainer;
#if !UNITY_EDITOR
using UnityEngine;
#endif

public class GameState : IDisposable
{
    public enum State
    {
        MainMenu,
        Playing,
        Paused,
        Victory,
        Defeat
    }

    [Inject] private readonly GameConfig gameConfig;
    [Inject] private readonly ISaveManager saveManager;

    public readonly ReactiveProperty<State> CurrentState = new(State.MainMenu);
    public readonly ReactiveProperty<float> TimeLeft = new(0f);

    public LevelConfig CurrentLevel => gameConfig.levels[CurrentLevelIdx];
    public int CurrentLevelIdx => saveData.CurrentLevelIdx;
    public bool HasNextLevel => CurrentLevelIdx < gameConfig.levels.Count;

    private SaveData saveData;
    private int prevLevelIdx;

    public async UniTask Init()
    {
        saveData = await saveManager.LoadAsync();
    }

    public void StartNewGame()
    {
        prevLevelIdx = saveData.CurrentLevelIdx = 0;
        saveManager.SaveAsync(saveData).Forget();

        CurrentState.Value = State.Playing;
    }

    public void StartNextLevel()
    {
        prevLevelIdx = CurrentLevelIdx;

        CurrentState.Value = State.Playing;
    }

    public void RestartLevel()
    {
        saveData.CurrentLevelIdx = prevLevelIdx;
        saveManager.SaveAsync(saveData).Forget();

        CurrentState.Value = State.Playing;
    }

    public void Pause()
    {
        if (CurrentState.Value == State.Playing)
            CurrentState.Value = State.Paused;
    }

    public void Resume()
    {
        if (CurrentState.Value == State.Paused)
            CurrentState.Value = State.Playing;
    }

    public void OnLevelCompleted()
    {
        ++saveData.CurrentLevelIdx;
        saveManager.SaveAsync(saveData).Forget();

        CurrentState.Value = State.Victory;
    }

    public void OnLevelFailed()
    {
        CurrentState.Value = State.Defeat;
    }

    public void GoToMainMenu()
    {
        CurrentState.Value = State.MainMenu;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Dispose()
    {
        CurrentState?.Dispose();
        TimeLeft?.Dispose();
    }
}
