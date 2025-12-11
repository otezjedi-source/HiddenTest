using UnityEngine;
using DG.Tweening;
using VContainer;
using VContainer.Unity;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private MainMenuScreen mainMenuScreen;
    [SerializeField] private GameScreen gameScreen;
    [SerializeField] private SettingsPanel settingsPanel;
    [SerializeField] private VictoryScreen victoryScreen;
    [SerializeField] private DefeatScreen defeatScreen;
    [SerializeField] private GameObject endScreen;

    void Start()
    {
        HideAll();
    }

    public void Register(IContainerBuilder builder)
    {
        builder.RegisterComponent(mainMenuScreen).AsSelf();
        builder.RegisterComponent(gameScreen).AsSelf();
        builder.RegisterComponent(settingsPanel).AsSelf();
        builder.RegisterComponent(victoryScreen).AsSelf();
        builder.RegisterComponent(defeatScreen).AsSelf();
    }

    public void ShowLoadingScreen()
    {
        HideAll();
        loadingScreen.SetActive(true);
    }

    public void ShowMainMenuScreen()
    {
        HideAll();
        mainMenuScreen.gameObject.SetActive(true);
    }

    public void ShowGameScreen()
    {
        HideAll();
        gameScreen.gameObject.SetActive(true);
    }

    public void ShowSettingsPanel()
    {
        settingsPanel.gameObject.SetActive(true);
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
        victoryScreen.gameObject.SetActive(true);
        victoryScreen.transform.localScale = Vector3.zero;
        victoryScreen.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
    }

    private void ShowDefeatScreen()
    {
        HideAll();
        defeatScreen.gameObject.SetActive(true);
        defeatScreen.transform.localScale = Vector3.zero;
        defeatScreen.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
    }

    public void ShowEndScreen()
    {
        endScreen.SetActive(true);
    }

    private void HideAll()
    {
        loadingScreen.SetActive(false);
        mainMenuScreen.gameObject.SetActive(false);
        gameScreen.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(false);
        victoryScreen.gameObject.SetActive(false);
        defeatScreen.gameObject.SetActive(false);
        endScreen.SetActive(false);
    }
}
