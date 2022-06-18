using Project.Ball;

namespace Project.Level
{
    public class LevelsManager : IInitializable, ILevelReachedListener
    {
        private readonly BallsPlaceService _ballsPlaceService;
        private readonly LevelEndService _levelEndService;
        private readonly LevelsInfoService _levelsInfoService;
        private readonly CameraController _cameraController;

        public LevelsManager(BallsPlaceService ballsPlaceService, LevelEndService levelEndService, LevelsInfoService levelsInfoService,  CameraController cameraController)
        {
            _cameraController = cameraController;
            _levelsInfoService = levelsInfoService;
            _levelEndService = levelEndService;
            _ballsPlaceService = ballsPlaceService;
        }

        public void Initialize()
        {
            var levelReachedContext = new LevelReachedContext();
            LevelData levelData = new LevelData();
            levelData.Id = 0;
            levelReachedContext.LevelData = levelData;
            
            OnLevelReached(levelReachedContext);
            
            /*Level level = _levelsInfoService.LoadLevel(1);
            
            _levelEndService.OnLevelLoaded(level);

            _ballsPlaceService.SpawnBall(level.SpawnPoint.position);*/
        }

        public void OnLevelReached(LevelReachedContext levelReachedContext)
        {
            Level level = _levelsInfoService.LoadLevel(levelReachedContext.LevelData.Id + 1);
            
            _levelEndService.OnLevelLoaded(level);

            _cameraController.HasToFollow = true;
            _ballsPlaceService.SpawnInitBall(level.SpawnPoint.position);
            _cameraController.SetPos(level.SpawnPoint.position);
            
            //_cameraController.MoveTo();
        }
    }
    
}