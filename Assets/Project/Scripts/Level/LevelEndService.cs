using Project.Ball;
using Project.Utils;
using UniRx;

namespace Project.Level
{
    public class LevelEndService
    {
        private readonly LevelsInfoService _levelsInfoService;
        private readonly LevelReachedDispatcher _levelReachedDispatcher;
        private readonly BallShooter _ballShooter;
        private readonly LevelFinishService _levelFinishService;
        private ScreenFader _screenFader;

        public LevelEndService(LevelsInfoService levelsInfoService, LevelReachedDispatcher levelReachedDispatcher,
            BallShooter ballShooter, LevelFinishService levelFinishService, ScreenFader screenFader)
        {
            _screenFader = screenFader;
            _levelFinishService = levelFinishService;
            _ballShooter = ballShooter;
            _levelReachedDispatcher = levelReachedDispatcher;
            _levelsInfoService = levelsInfoService;
        }

        public void OnLevelLoaded(Level level)
        {
            level.EndReached.Take(1).Subscribe((OnLevelReached));
        }

        private async void OnLevelReached(Level level)
        {
            var shootAngle = await _ballShooter.Shoot(level);
            await _levelFinishService.Calculate(shootAngle);
            
            _screenFader.FadeIn((() =>
            {
                _levelsInfoService.UnloadLevel(level);
                
                var levelReachedContext = new LevelReachedContext();
                levelReachedContext.LevelData = _levelsInfoService.GetCurrentLevelData();

                _levelReachedDispatcher.LevelReach(levelReachedContext);

                _screenFader.FadeOut(null);
            }));
            
            
            
        }
    }
}