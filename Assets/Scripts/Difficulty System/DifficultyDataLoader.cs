using System.Collections.Generic;
using UnityEngine;

namespace DifficultySystem
{
    [CreateAssetMenu(fileName = "Difficulty Loader", menuName = "Game Data/Difficulty Loader", order = 1)]
    public class DifficultyDataLoader : ScriptableObject
    {
        [SerializeField] private List<DifficultyData> _data;

        public DifficultyData GetDificulty(Difficulties difficulties)
        {
            return _data.Find(x => x.DifficultyName.Equals(difficulties));
        }
    }
}
