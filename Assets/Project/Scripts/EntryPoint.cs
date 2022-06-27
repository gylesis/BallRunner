using System.Collections.Generic;
using Project.Ball;
using Project.Level;
using Project.Utils;
using UnityEngine;

namespace Project
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private BallsSpawnService _ballsSpawnService;
        [SerializeField] private Ball.Ball _ballPrefab;
        [SerializeField] private BallsPlaceService _ballsPlaceService;
        [SerializeField] private BallsMover _ballsMover;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private LevelsContainer _levelsContainer;
        [SerializeField] private UIContainer _uiContainer;
        [SerializeField] private BallShooter _ballShooter;
        [SerializeField] private ScreenTapWatcher _screenTapWatcher;
        [SerializeField] private LevelFinishService _levelFinishService;
        [SerializeField] private ScreenFader _screenFader;

        private readonly List<IInitializable> _initializables = new List<IInitializable>();
        private readonly List<IUpdatable> _updatables = new List<IUpdatable>();

        private void Awake()
        {
            _levelFinishService.Init(_screenTapWatcher, _ballShooter);

            var levelsInfoService = new LevelsInfoService(_levelsContainer, _ballsPlaceService, _uiContainer);

            var levelReachedDispatcher = new LevelReachedDispatcher();
            var levelEndService = new LevelEndService(levelsInfoService, levelReachedDispatcher, _ballShooter, _levelFinishService, _screenFader);
            
            var levelsManager = new LevelsManager(_ballsPlaceService, levelEndService, levelsInfoService, _cameraController);

            levelReachedDispatcher.AddListener(levelsInfoService);
            levelReachedDispatcher.AddListener(levelsManager);

            IInputService inputService = GetInputsService();
            _screenTapWatcher.Init(inputService);

            _ballsMover.Init(inputService);
            _uiContainer.Init(_ballsMover);
            _ballsSpawnService.Init(_ballPrefab);

            var ballsHandler = new BallsHandler(_ballsPlaceService);

            var ballsPool = new BallsPool(_ballsSpawnService, ballsHandler, _ballsMover);
            ballsPool.CreateBalls(50);

            _ballShooter.Init(_ballsMover, _cameraController, _screenTapWatcher, ballsPool);

            _ballsPlaceService.Init(ballsPool, _ballsMover);
            _cameraController.Init(_ballsMover);

            _updatables.Add(inputService as IUpdatable);
            _initializables.Add(levelsManager);
        }

        private void Start()
        {
            foreach (IInitializable initializable in _initializables)
                initializable.Initialize();
        }

        private void Update()
        {
            foreach (IUpdatable updatable in _updatables)
                updatable.Update();
        }

        private IInputService GetInputsService()
        {
            IInputService inputService = new MobileInputService();

            if (Application.isMobilePlatform)
            {
                inputService = new MobileInputService();
            }
            else if (Application.isEditor)
            {
                inputService = new StandalondeInputService();
            }

            return inputService;
        }
    }

    public interface IInitializable
    {
        void Initialize();
    }
}