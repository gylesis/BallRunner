using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Ball
{
    public class BallsMover : MonoBehaviour
    {
        public bool IsAllowedToPlay = false;
        
        [SerializeField] private float _speedZModifier = 12;
        [SerializeField] private float _sensitivityX = 1f;
        [SerializeField] private Rigidbody _rigidbody;

        private IInputService _inputService;
        private Vector3 _directionToMove;
        private Vector3 _lastMidPos;
        private Ball _ball;
        public List<Ball> Balls { get; } = new List<Ball>();

        public void Init(IInputService inputService)
        {
            _inputService = inputService;
        }

        public Vector3 GetSpawnPoint()
        {
            var balls = Balls.OrderByDescending(x => (_ball.transform.position - x.transform.position).sqrMagnitude).ToList();

            var index = Random.Range(0,balls.Count - 1);
            
            var ball = balls[index];

            Ball closestBall = FindClosestBall(ball);
            Vector3 direction = (closestBall.transform.position - ball.transform.position).normalized;

            var radius = ball.transform.localScale.x / 2;
            
            direction *= radius;

            direction = Vector3.ProjectOnPlane(direction, Vector3.up);
            
            Vector3 spawnPos = ball.transform.position + direction;

            return spawnPos;
        }
        
        private void Update()
        {
            _directionToMove = Vector3.zero;
            
            if(IsAllowedToPlay == false) return;
            
            Vector2 inputVector = _inputService.InputVector;

            var directionToMove = new Vector3(inputVector.x, 0, 1);
            
            _directionToMove = directionToMove;
            
            Move(_directionToMove);
        }

        private void Move(Vector3 direction)
        {
            if (Balls.Count <= 0) return;
            
            _ball = Balls[0];

            var moveVector = direction;

            moveVector.x *= _sensitivityX;
            moveVector.z *= _speedZModifier;

            if (_ball.IsOnBoost == false)
                moveVector.y = -3f;
            else
                moveVector.y = _ball.Rigidbody.velocity.y;

            _ball.Rigidbody.velocity = moveVector;
            
            foreach (Ball ball in Balls)
            {
                if(ball == _ball) continue;
                
                Vector3 movePos;
                
                movePos = _ball.Rigidbody.velocity;

                if (_ball.IsOnBoost)
                {
                    continue;
                }
                
                if (ball.IsInCrowd() == false)
                {
                    if (ball.IsOnBoost == false)
                    {
                         Ball closestBall = FindClosestBall(ball);
                         movePos = (closestBall.transform.position - ball.transform.position);
                    } 
                }
                else
                {
                    movePos = _ball.Rigidbody.velocity;
                }

                movePos.y = _ball.Rigidbody.velocity.y;

                ball.Rigidbody.velocity = movePos;
            }
            
        }

        public void MoveTest(Vector3 direction)
        {
            var moveVector = direction;

            moveVector.x *= _sensitivityX;
            moveVector.z *= _speedZModifier;
            moveVector.y = _rigidbody.velocity.y;

            _rigidbody.velocity = moveVector;
           
            if (Balls.Count == 1)
            {
                return;
            }
            
            foreach (Ball ball in Balls)
            {
                Vector3 movePos;
                
                movePos = _rigidbody.velocity;
                
                if (ball.IsInCrowd() == false)
                {
                    if (ball.IsOnBoost == false)
                    {
                        Ball closestBall = FindClosestBall(ball);
                        movePos = (closestBall.transform.position - ball.transform.position);
                    }
                }
                else
                {
                    movePos = _rigidbody.velocity;
                }

                movePos.y = ball.Rigidbody.velocity.y;

                ball.Rigidbody.velocity = movePos;
            }
        }

        public Vector3 GetMidPos()
        {
            var ballsCount = Balls.Count;
            
            if (ballsCount == 0)
            {
                return _lastMidPos;
            }
            
            Vector3 midPos = Vector3.one;

            foreach (Ball ball in Balls)
            {
                Vector3 ballPos = ball.transform.position;
                midPos.x += ballPos.x;
                midPos.y += ballPos.y;
                midPos.z += ballPos.z;
            }

            midPos.x /= ballsCount;
            midPos.y /= ballsCount;
            midPos.z /= ballsCount;

            _lastMidPos = midPos;
            
            return midPos;
        }

        public void StopMovement()
        {
            IsAllowedToPlay = false;
            
            foreach (Ball ball in Balls)
            {
                ball.Rigidbody.velocity = Vector3.zero;
            }
        }

        private Ball FindClosestBall(Ball ball)
        {
            var balls = Balls.OrderByDescending(x => (ball.transform.position - x.transform.position).sqrMagnitude)
                .ToList();

            var closestBall = balls[0];
            return closestBall;
        }
    }
}