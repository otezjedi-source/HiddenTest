using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

public class Startup : IStartable
{
    [Inject] private readonly GameState gameState;
    [Inject] private readonly UI ui;

    async void IStartable.Start()
    {
        ui.ShowLoadingScreen();
        await InitializeSystems();
        ui.ShowMainMenuScreen();
    }

    private async UniTask InitializeSystems()
    {
        await gameState.Init();
    }
}
