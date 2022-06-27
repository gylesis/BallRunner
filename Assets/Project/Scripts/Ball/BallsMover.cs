using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Ball
{
    public class BallsMover : MonoBehaviour
    {
        [SerializeField] private float _speedZModifier = 12;
        [SerializeField] private float _sensitivityX = 1f;

        [SerializeField] private float _tightSpeed = 3f;
        [SerializeField] private float _offset = 0.05f;
        [SerializeField] private float _tightCooldown = 0.5f;

        private IInputService _inputService;
        private Vector3 _directionToMove;
        private Vector3 _lastMidPos;
        private Ball _ball;
        private float _timer;
        
        public List<Ball> Balls { get; } = new List<Ball>();
        public bool IsAllowedToPlay = false;

        public void Init(IInputService inputService)
        {
            _inputService = inputService;
        }

        public Vector3 GetSpawnPoint()
        {
            var balls = Balls.OrderBy(x => (_ball.transform.position - x.transform.position).sqrMagnitude)
                .ToList();

            var ball = balls[Random.Range(0, balls.Count - 1)];

            Vector3 position = ball.transform.position;
            Vector3 direction = (_ball.transform.position - position).normalized;

            var radius = ball.transform.localScale.x / 2;

            Debug.DrawRay(position, direction, Color.black);

            direction *= -radius;
            direction = Vector3.ProjectOnPlane(direction, Vector3.up);

           // direction = Rotate(position, direction, 45);

            Debug.DrawRay(position, direction, Color.red);
            Vector3 spawnPos = position + direction;

            return spawnPos;
        }

        private void Update()
        {
            _directionToMove = Vector3.zero;

            if (IsAllowedToPlay == false) return;

            _timer += Time.deltaTime;

            if (_timer >= _tightCooldown)
            {
                TightBalls();
                _timer = 0;
            }
            
            Vector2 inputVector = _inputService.InputVector;
            
            var directionToMove = new Vector3(inputVector.x, 0, 1);

            _directionToMove = directionToMove;

            Move(_directionToMove);
            //MoveTest(_directionToMove);
        }

        public void Move(Vector3 direction)
        {
            if (Balls.Count == 0) return;

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
                if (ball == _ball) continue;

                if(ball.IsOnBoost) continue;
                
                ball.Rigidbody.velocity = moveVector;
            }
        }

        public void TightBalls()
        {
            foreach (Ball ball in Balls)
            {
                if (ball == _ball) continue;

                if(ball.IsOnBoost) continue;
                
                Vector3 position = ball.transform.position;
                var radius = ball.transform.localScale.x / 2 - float.Epsilon;

                Vector3 directionToCenter = _ball.transform.position - position;

                var sphereCast = Physics.SphereCast(position, radius, directionToCenter, out var hit);

                if (sphereCast)
                {
                    //Debug.Log($"Hit distance {hit.distance}, magnitude {directionToCenter.magnitude}");

                    if (hit.distance >= _offset)
                    {
                        ball.Rigidbody.angularVelocity = Vector3.zero;
                        Vector3 direction = (hit.point - position).normalized;

                        ball.transform.position += direction * _tightSpeed * Time.deltaTime;
                    }
                }
            }
        }
        
        private void MoveTest(Vector3 direction)
        {
            if (Balls.Count == 0) return;

            _ball = Balls[0];

            var moveVector = direction;

            moveVector.x *= _sensitivityX;
            moveVector.z *= _speedZModifier;

            /*if (_ball.IsOnBoost == false)
                moveVector.y = -3f;
            else
                moveVector.y = _ball.Rigidbody.velocity.y;*/

            _ball.Rigidbody.velocity = moveVector;

            foreach (Ball ball in Balls)
            {
                if (ball == _ball) continue;

                Vector3 movePos;

                movePos = _ball.Rigidbody.velocity;

                /*if (_ball.IsOnBoost)
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
                }*/

                movePos.y = _ball.Rigidbody.velocity.y;

                ball.Rigidbody.velocity = movePos;
            }
        }

        private Vector3 Rotate(Vector3 origin, Vector3 direction, float angle)
        {
            float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
            float sin = Mathf.Sin(angle * Mathf.Deg2Rad);

            Vector3 newVector3 = direction;

            newVector3.x = direction.x * cos - direction.z * sin;
            newVector3.z = direction.x * sin + direction.z * cos;

            return newVector3 + origin;
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