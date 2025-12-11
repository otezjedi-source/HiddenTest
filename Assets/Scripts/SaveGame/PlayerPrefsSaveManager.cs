using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerPrefsSaveManager : ISaveManager
{
    private const string SAVE_KEY = "SaveData";

    public async UniTask<SaveData> LoadAsync()
    {
        await UniTask.Yield();

        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            var json = PlayerPrefs.GetString(SAVE_KEY);
            return SaveData.FromJson(json);
        }

        return new SaveData();
    }

    public async UniTask SaveAsync(SaveData data)
    {
        await UniTask.Yield();

        var json = data.ToJson();
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    public async UniTask DeleteAsync()
    {
        await UniTask.Yield();

        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            PlayerPrefs.DeleteKey(SAVE_KEY);
            PlayerPrefs.Save();
        }
    }

    public async UniTask<bool> HasSave()
    {
        await UniTask.Yield();

        return PlayerPrefs.HasKey(SAVE_KEY);
    }
}
