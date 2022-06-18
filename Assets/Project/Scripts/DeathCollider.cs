using Project.Ball;
using UnityEngine;

namespace Project
{
    public class DeathCollider : MonoBehaviour
    {
        private BallsPlaceService _ballsPlaceService;

        public void Init(BallsPlaceService ballsPlaceService)
        {
            _ballsPlaceService = ballsPlaceService;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Ball.Ball>(out var ball))
            {
                _ballsPlaceService.ReturnBall(ball);
            }
        }
    }
}