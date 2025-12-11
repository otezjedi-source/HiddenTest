using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text caption;

    public ItemData ItemData { get; private set; }

    private AsyncOperationHandle<Sprite> iconHandle;
    private readonly CompositeDisposable disposables = new();

    public void Init(ItemData itemData, Settings settings)
    {
        Clear();

        ItemData = itemData;

        if (itemData != null)
        {
            settings.ItemsDisplayMode
                .Subscribe(mode => SetDisplayMode(mode))
                .AddTo(disposables);

            icon.sprite = itemData.icon;
            caption.text = itemData.displayName;
            gameObject.SetActive(true);
        }
        else
            gameObject.SetActive(false);
    }

    private void SetDisplayMode(ItemDisplayMode.Type mode)
    {
        icon.gameObject.SetActive(mode.HasFlag(ItemDisplayMode.Type.Icon));
        caption.gameObject.SetActive(mode.HasFlag(ItemDisplayMode.Type.Text));
    }

    private void Clear()
    {
        ItemData = null;
        icon.sprite = null;
        caption.text = string.Empty;
        disposables?.Clear();

        if (iconHandle.IsValid())
            Addressables.Release(iconHandle);
    }

    private void OnDestroy()
    {
        Clear();
    }
}
