using UniRx;

public class Settings
{
    public ReactiveProperty<ItemDisplayMode.Type> ItemsDisplayMode = new(ItemDisplayMode.Type.Text);
}
