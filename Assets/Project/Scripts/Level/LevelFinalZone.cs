using Project.Utils;
using UnityEngine;

namespace Project.Level
{
    public class LevelFinalZone : MonoBehaviour
    {
        [SerializeField] private TriggerCollider _endTriggerCollider;
        [SerializeField] private Transform _ballsPullPoint;

        public Transform BallsPullPoint => _ballsPullPoint;
        public TriggerCollider EndTriggerCollider => _endTriggerCollider;
        
    }
}