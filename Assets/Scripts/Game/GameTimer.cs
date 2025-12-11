using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using VContainer;

public class GameTimer
{
    [Inject] private readonly GameState gameState;

    public async UniTask<bool> StartAsync(float duration, IReadOnlyReactiveProperty<bool> isActive)
    {
        float timeLeft = duration;
        gameState.TimeLeft.Value = timeLeft;

        while (timeLeft > 0 && isActive.Value)
        {
            await UniTask.Yield();

            if (gameState.CurrentState.Value == GameState.State.Paused)
                continue;

            timeLeft = Mathf.Max(0, timeLeft - Time.deltaTime);
            gameState.TimeLeft.Value = timeLeft;
        }

        return timeLeft <= 0;
    }
}
