using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Ball
{
    public class BallsPool
    {
        private readonly Dictionary<Ball, bool> _availabilityInfo = new Dictionary<Ball, bool>();

        private readonly BallsSpawnService _ballsSpawnService;
        private readonly BallsHandler _ballsHandler;
        private BallsMover _ballsMover;

        public BallsPool(BallsSpawnService ballsSpawnService, BallsHandler ballsHandler, BallsMover ballsMover)
        {
            _ballsMover = ballsMover;
            _ballsHandler = ballsHandler;
            _ballsSpawnService = ballsSpawnService;
        }

        public void CreateBalls(int count)
        {
            for (int i = 1; i <= count; i++)
            {
                Ball ball = _ballsSpawnService.Spawn();
                _ballsHandler.Handle(ball);

                _availabilityInfo.Add(ball, true);
                OnEnterPool(ball);
            }
        }

        private void OnEnterPool(Ball ball)
        {
            ball.OnEnterPool();
            ball.Rigidbody.velocity = Vector3.zero;
            ball.Rigidbody.angularVelocity = Vector3.zero;
            
            ball.gameObject.SetActive(false);
            _availabilityInfo[ball] = true;
        }

        private void OnExitPool(Ball ball)
        {
            ball.Rigidbody.velocity = Vector3.zero;
            ball.Collider.enabled = true;
            ball.Rigidbody.useGravity = true;

            _availabilityInfo[ball] = false;
            ball.gameObject.SetActive(true);
            ball.OnExitPool();
        }

        public Ball Rent(Vector3 pos)
        {
            var availbaleBall = _availabilityInfo.FirstOrDefault(x => x.Value == true).Key;

            if (availbaleBall == null)
            {
                CreateBalls(_availabilityInfo.Count * 2);
                return Rent(pos);
            }

            availbaleBall.transform.position = pos;

           
            OnExitPool(availbaleBall);

            return availbaleBall;
        }

        public void ReturnToPool(Ball ball)
        {
            OnEnterPool(ball);

            if (_ballsMover.Balls.Contains(ball))
            {
                _ballsMover.Balls.Remove(ball);
            }
        }
    }
}