using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour, IPointerClickHandler
{
    private readonly Subject<ItemData> onClickedSubject = new();
    public IObservable<ItemData> OnClicked => onClickedSubject;

    public ItemData ItemData { get; private set; }
    public bool IsClickable { get; set; } = false;

    private SpriteRenderer spriteRenderer;
    private GameConfig gameConfig;

    public void Init(ItemData itemData, GameConfig gameConfig) {
        ItemData = itemData;
        this.gameConfig = gameConfig;
    }

    private void Awake() {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        
        var collider = gameObject.AddComponent<BoxCollider2D>();
        collider.size = spriteRenderer.sprite.bounds.size;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (IsClickable)
            OnItemClicked().Forget();
    }

    private async UniTaskVoid OnItemClicked() {
        IsClickable = false;
        onClickedSubject.OnNext(ItemData);

        var tasks = new List<UniTask>
        {
            spriteRenderer.DOFade(0f, gameConfig.fadeDuration).ToUniTask(),
            transform.DOScale(0f, gameConfig.shrinkDuration).ToUniTask()
        };

        await UniTask.WhenAll(tasks);

        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        onClickedSubject?.Dispose();
    }
}
