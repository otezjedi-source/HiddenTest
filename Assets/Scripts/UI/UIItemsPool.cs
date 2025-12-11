using System.Collections.Generic;
using UnityEngine;

public class UIItemsPool
{
    private readonly UIItem prefab;
    private readonly Transform container;
    private readonly Stack<UIItem> pool = new();

    public UIItemsPool(UIItem prefab, Transform container)
    {
        this.prefab = prefab;
        this.container = container;
    }

    public UIItem Get()
    {
        UIItem item;

        if (pool.Count > 0)
        {
            item = pool.Pop();
            item.transform.SetAsLastSibling();
            item.gameObject.SetActive(true);
        }
        else
            item = Object.Instantiate(prefab, container);

        return item;
    }

    public void Return(UIItem item)
    {
        if (item == null)
            return;

        item.gameObject.SetActive(false);
        pool.Push(item);
    }

    public void ReturnAll(List<UIItem> items)
    {
        foreach (var item in items)
            Return(item);
        items.Clear();
    }

    public void Clear()
    {
        foreach (var item in pool)
        {
            if (item != null)
                Object.Destroy(item.gameObject);
        }
        pool.Clear();
    }
}
