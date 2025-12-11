using Newtonsoft.Json;

public class SaveData
{
    public int CurrentLevelIdx { get; set; }

    public static SaveData FromJson(string json)
    {
        try
        {
            var data = JsonConvert.DeserializeObject<SaveData>(json);
            return new() { CurrentLevelIdx = data?.CurrentLevelIdx ?? 0 };
        }
        catch
        {
            return new SaveData();
        }
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}
