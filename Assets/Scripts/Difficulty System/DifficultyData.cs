using UnityEngine;

namespace DifficultySystem
{
    [CreateAssetMenu(fileName = "Difficulty Data", menuName = "Game Data/Difficulty", order = 1)]
    public class DifficultyData : ScriptableObject
    {
        [field: SerializeField] public Difficulties DifficultyName { get; private set; }
        [field: SerializeField, Min(0.0f)] public float GateHeight { get; private set; }
        [field: SerializeField] public GameObject BottomPipe { get; private set; }
        [field: SerializeField] public GameObject TopPipe { get; private set; }
        [field: SerializeField, Min(0.0f)] public float PipeSpawnDelay { get; private set; }
        [field: SerializeField, Min(0.0f)] public float LevelObjectsSpeed { get; private set; }
    }
}