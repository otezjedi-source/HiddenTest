using Cysharp.Threading.Tasks;

public interface ISaveManager
{
    UniTask<SaveData> LoadAsync();
    UniTask SaveAsync(SaveData data);
    UniTask DeleteAsync();
    UniTask<bool> HasSave();
}
