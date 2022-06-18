using UnityEngine;

namespace Project.Level
{
    [CreateAssetMenu(menuName = "StaticData/LevelsContainer ", fileName = "LevelsContainer", order = 0)]
    public class LevelsContainer : ScriptableObject
    {
        [SerializeField] private Level[] _levels;

        public Level[] Levels => _levels;
    }
}