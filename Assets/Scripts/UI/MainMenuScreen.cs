using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MainMenuScreen : MonoBehaviour
{
    [SerializeField] private Button btnNewGame;
    [SerializeField] private Button btnContinue;
    [SerializeField] private Button btnExit;

    [Inject] private readonly UI ui;
    [Inject] private readonly GameState gameState;
    [Inject] private readonly GameScene gameScene;

    private void Start()
    {
        btnNewGame
            .OnClickAsObservable()
            .Subscribe(_ => OnBtnNewGameClick().Forget());

        btnContinue
            .OnClickAsObservable()
            .Subscribe(_ => OnBtnContinueClick().Forget());

        btnExit
            .OnClickAsObservable()
            .Subscribe(_ => OnBtnExitClick());
    }

    private void OnEnable()
    {
        bool canContinue = gameState.CurrentLevelIdx > 0 && gameState.HasNextLevel;
        btnContinue.gameObject.SetActive(canContinue);
    }

    private async UniTask OnBtnNewGameClick()
    {
        ui.ShowLoadingScreen();
        gameState.StartNewGame();
        await gameScene.Init();
        ui.ShowGameScreen();
    }

    private async UniTask OnBtnContinueClick()
    {
        ui.ShowLoadingScreen();
        gameState.StartNextLevel();
        await gameScene.Init();
        ui.ShowGameScreen();
    }

    private void OnBtnExitClick()
    {
        gameState.QuitGame();
    }
}
