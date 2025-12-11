using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] private Button btnNext;
    [SerializeField] private Button btnRestart;
    [SerializeField] private Button btnQuit;

    [Inject] private readonly UI ui;
    [Inject] private readonly GameState gameState;
    [Inject] private readonly GameScene gameScene;

    private void Start()
    {
        btnNext.OnClickAsObservable()
            .Subscribe(_ => OnBtnNextClick().Forget());

        btnRestart.OnClickAsObservable()
            .Subscribe(_ => OnBtnRestartClick().Forget());

        btnQuit.OnClickAsObservable()
            .Subscribe(_ => OnBtnQuitClick());
    }

    private async UniTaskVoid OnBtnNextClick()
    {
        if (!gameState.HasNextLevel)
        {
            gameState.GoToMainMenu();
            ui.ShowEndScreen();
            await UniTask.Delay(2000);
            ui.ShowMainMenuScreen();
            return;
        }


        ui.ShowLoadingScreen();
        gameState.StartNextLevel();
        await gameScene.Init();
        ui.ShowGameScreen();
    }

    private async UniTaskVoid OnBtnRestartClick()
    {
        ui.ShowLoadingScreen();
        gameState.RestartLevel();
        await gameScene.Init();
        ui.ShowGameScreen();
    }
    
    private void OnBtnQuitClick()
    {
        gameState.GoToMainMenu();
        ui.ShowMainMenuScreen();
    }
}
