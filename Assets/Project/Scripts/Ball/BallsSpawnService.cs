using UnityEngine;

namespace Project.Ball
{
    public class BallsSpawnService : MonoBehaviour
    {
        [SerializeField] private Transform _parent;

        private Ball _ballPrefab;

        public void Init(Ball ballPrefab)
        {
            _ballPrefab = ballPrefab;
        }
        
        public Ball Spawn()
        {
            Ball ball = Instantiate(_ballPrefab, _parent);
            return ball;
        }
        
    }
    
}