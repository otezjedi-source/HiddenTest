using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "HiddenTest/Game Config")]
public class GameConfig : ScriptableObject
{
    public float fadeDuration = 0.5f;
    public float shrinkDuration = 0.3f;
    public List<LevelConfig> levels;
}
