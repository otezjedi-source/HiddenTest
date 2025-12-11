using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class DefeatScreen : MonoBehaviour
{
    [SerializeField] private Button btnRestart;
    [SerializeField] private Button btnQuit;

    [Inject] private readonly UI ui;
    [Inject] private readonly GameState gameState;
    [Inject] private readonly GameScene gameScene;

    private void Start()
    {
        btnRestart
            .OnClickAsObservable()
            .Subscribe(_ => OnBtnRestartClick().Forget());

        btnQuit
            .OnClickAsObservable()
            .Subscribe(_ => OnBtnQuitClick());
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
    }
}
