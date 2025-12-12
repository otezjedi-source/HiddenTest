using UnityEngine;
using DG.Tweening;
using VContainer;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject defeatScreen;
    [SerializeField] private GameObject endScreen;

    [Inject] private readonly GameState gameState;
    [Inject] private readonly GameScene gameScene;

    void Start()
    {
        HideAll();
    }

    public void ShowLoadingScreen()
    {
        HideAll();
        loadingScreen.SetActive(true);
    }

    public void ShowMainMenuScreen()
    {
        HideAll();
        mainMenuScreen.SetActive(true);
        gameState.GoToMainMenu();
    }

    public void ShowGameScreen()
    {
        HideAll();
        gameScene.gameObject.SetActive(true);
        gameScreen.SetActive(true);
    }

    public void ShowSettingsPanel()
    {
        settingsPanel.SetActive(true);
        gameState.Pause();
    }

    public void HideSettingsPanel()
    {
        settingsPanel.SetActive(false);
        gameState.Resume();
    }
    
    public void ShowGameResult(GamePlay.Result result)
    {
        if (result == GamePlay.Result.Victory)
            ShowVictoryScreen();
        else if (result == GamePlay.Result.Defeat)
            ShowDefeatScreen();
    }

    private void ShowVictoryScreen()
    {
        HideAll();
        victoryScreen.SetActive(true);
        victoryScreen.transform.localScale = Vector3.zero;
        victoryScreen.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
    }

    private void ShowDefeatScreen()
    {
        HideAll();
        defeatScreen.SetActive(true);
        defeatScreen.transform.localScale = Vector3.zero;
        defeatScreen.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
    }

    public void ShowEndScreen()
    {
        HideAll();
        endScreen.SetActive(true);
    }

    private void HideAll()
    {
        loadingScreen.SetActive(false);
        mainMenuScreen.SetActive(false);
        gameScreen.SetActive(false);
        settingsPanel.SetActive(false);
        victoryScreen.SetActive(false);
        defeatScreen.SetActive(false);
        endScreen.SetActive(false);
        gameScene.gameObject.SetActive(false);
    }
}
