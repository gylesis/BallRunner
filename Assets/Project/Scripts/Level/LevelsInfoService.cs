using System.Linq;
using Project.Ball;
using UnityEngine;

namespace Project.Level
{
    public class LevelsInfoService : ILevelReachedListener
    {
        private readonly LevelsContainer _levelsContainer;

        private int _currentLevel = 1;
        private Level _level;
        private BallsPlaceService _ballsPlaceService;
        private UIContainer _uiContainer;

        public LevelsInfoService(LevelsContainer levelsContainer, BallsPlaceService ballsPlaceService,
            UIContainer uiContainer)
        {
            _uiContainer = uiContainer;
            _ballsPlaceService = ballsPlaceService;
            _levelsContainer = levelsContainer;
        }

        public LevelData GetCurrentLevelData()
        {
            return _level.LevelData;
        }

        public Level GetLevelPrefab(int levelId)
        {
            Level level = _levelsContainer.Levels.FirstOrDefault(x => x.LevelData.Id == levelId);

            if (level == null)
            {
                level = _levelsContainer.Levels.First();
            }

            return level;
        }

        public Level LoadLevel(int levelId)
        {
            Level levelPrefab = GetLevelPrefab(levelId);
            Level loadedLevel = Object.Instantiate(levelPrefab);

            _level = loadedLevel;

            OnLevelLoad(loadedLevel);

            return loadedLevel;
        }

        public void OnLevelReached(LevelReachedContext levelReachedContext)
        {
            _currentLevel += levelReachedContext.LevelData.Id + 1;
        }

        private void OnLevelLoad(Level level)
        {
            level.Init(_ballsPlaceService);

            _uiContainer.OnLevelLoaded(level.LevelData);
        }

        public Level LoadLevel(Level level)
        {
            Level levelInstance = Object.Instantiate(level);
            _level = levelInstance;
            return levelInstance;
        }

        public void UnloadLevel(Level level)
        {
            Object.Destroy(level.gameObject);
        }
    }
}