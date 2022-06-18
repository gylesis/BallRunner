using Project.Level.Gate;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Project.Ball
{
    public class BallsPlaceService : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;

        private BallsPool _ballsPool;
        private BallsMover _ballsMover;

        public void Init(BallsPool ballsPool, BallsMover ballsMover)
        {
            _ballsMover = ballsMover;
            _ballsPool = ballsPool;
        }

        public void ManageBallsGateCommand(GateCommand gateCommand)
        {
            GateOperation operation = gateCommand.Operation;

            var value = gateCommand.Value;

            var ballsCount = _ballsMover.Balls.Count;
            var newBallsCount = _ballsMover.Balls.Count;

            switch (operation)
            {
                case GateOperation.Multiplication:
                    newBallsCount *= value;
                    break;
                case GateOperation.Addition:
                    newBallsCount += value;
                    break;
                case GateOperation.Subtraction:
                    newBallsCount -= value;
                    break;
                case GateOperation.Divide:
                    newBallsCount /= value;
                    break;
            }

            var count = newBallsCount - ballsCount;

            if (operation == GateOperation.Divide || operation == GateOperation.Subtraction)
            {
                for (int i = 0; i < Mathf.Abs(count); i++)
                {
                    var index = Random.Range(0, _ballsMover.Balls.Count - 1);

                    Ball ball = _ballsMover.Balls[index];

                    ReturnBall(ball);
                }
            }

            for (int i = 1; i <= count; i++)
            {
                Ball ball = SpawnBall(_ballsMover.GetSpawnPoint());
            }
        }

        public Ball SpawnInitBall(Vector3 pos)
        {
            _ballsMover.IsAllowedToPlay = false;

            Ball spawnInitBall = SpawnBall(pos);
            
            return spawnInitBall;
        }
        
        public Ball SpawnBall(Vector3 pos)
        {
            Ball spawnBall = _ballsPool.Rent(pos);
            _ballsMover.Balls.Add(spawnBall);
            return spawnBall;
        }
        
        public void ReturnBall(Ball ball)
        {
            _ballsPool.ReturnToPool(ball);

            if (_ballsMover.Balls.Contains(ball))
            {
                _ballsMover.Balls.Remove(ball);
            }

            if (_ballsMover.Balls.Count == 0)
            {
                Debug.Log("Good Game");
                SceneManager.LoadScene(0);
                return;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                SpawnBall(_ballsMover.GetSpawnPoint());
            }
        }
    }
}