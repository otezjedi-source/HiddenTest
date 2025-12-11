using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown itemsDisplayModeDropdown;
    [SerializeField] private Button btnQuit;
    [SerializeField] private Button btnClose;

    [Inject] private readonly Settings settings;
    [Inject] private readonly GameState gameState;
    [Inject] private readonly GamePlay gamePlay;
    [Inject] private readonly UI ui;

    private readonly CompositeDisposable disposables = new();

    private void Start()
    {
        itemsDisplayModeDropdown
            .onValueChanged
            .AsObservable()
            .Subscribe(value => OnItemsDisplayModeChange(value))
            .AddTo(disposables);

        btnQuit
            .OnClickAsObservable()
            .Subscribe(_ => OnQuitClick())
            .AddTo(disposables);

        btnClose
            .OnClickAsObservable()
            .Subscribe(_ => OnClose())
            .AddTo(disposables);
    }

    private void OnEnable()
    {
        var mode = settings.ItemsDisplayMode.Value;
        var idx = ItemDisplayMode.ToIndex(mode);
        itemsDisplayModeDropdown.SetValueWithoutNotify(idx);
        gameState.Pause();
    }

    private void OnItemsDisplayModeChange(int value)
    {
        var mode = ItemDisplayMode.FromIndex(value);
        settings.ItemsDisplayMode.Value = mode;
    }

    private void OnQuitClick()
    {
        gamePlay.QuitGame();
        gameState.GoToMainMenu();
        ui.ShowMainMenuScreen();
    }

    private void OnClose()
    {
        gameObject.SetActive(false);
        gameState.Resume();
    }
    
    private void OnDestroy()
    {
        disposables?.Dispose();
    }
}
