using Project.Ball;
using Project.Level.Gate;
using Project.Utils;
using UniRx;
using UnityEngine;

namespace Project.Level
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private GatesService _gatesService;
        [SerializeField] private LevelData _levelData;
        [SerializeField] private LevelFinalZone _levelFinalZone;

        public LevelFinalZone LevelFinalZone => _levelFinalZone;
        public LevelData LevelData => _levelData;
        public Subject<Level> EndReached { get; } = new Subject<Level>();
        
        public Transform SpawnPoint => _spawnPoint;

        public void Init(BallsPlaceService ballsPlaceService)
        {
            _gatesService.Init(ballsPlaceService);
            
            _levelFinalZone.EndTriggerCollider.TriggerEnter.TakeUntilDestroy(this).Subscribe((OnEndTriggerEnter));

            var deathColliders = GetComponentsInChildren<DeathCollider>();

            foreach (DeathCollider deathCollider in deathColliders)
            {
                deathCollider.Init(ballsPlaceService);
            }
        }

        private void OnEndTriggerEnter(TriggerColliderContext context)
        {
            if (context.Collider.CompareTag("Ball"))
            {
                context.Sender.Disable();
                EndReached.OnNext(this);
            }
        }
        
    }
}