using UniRx;

namespace Project.Ball
{
    public class BallsHandler
    {
        private readonly BallsPlaceService _ballsPlaceService;

        public BallsHandler(BallsPlaceService ballsPlaceService)
        {
            _ballsPlaceService = ballsPlaceService;
        }
        
        public void Handle(Ball ball)
        {
            ball.Collision.Subscribe((OnCollision));        
        }

        private void OnCollision(BallCollisionContext context)
        {
            if (context.Collision.gameObject.TryGetComponent<Obstacle>(out var obstacle))
            {
                _ballsPlaceService.ReturnBall(context.Sender);
            }
        }
    }
}