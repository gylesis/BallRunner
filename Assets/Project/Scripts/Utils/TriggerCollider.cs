using UniRx;
using UnityEngine;

namespace Project.Utils
{
    public class TriggerCollider : MonoBehaviour
    {
        [SerializeField] private Collider _collider;    
        public Subject<TriggerColliderContext> TriggerEnter { get; } = new Subject<TriggerColliderContext>();

        public void Activate()
        {
            _collider.enabled = true;
        }
        
        public void Disable()
        {
            _collider.enabled = false;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            var triggerColliderContext = new TriggerColliderContext();
            triggerColliderContext.Collider = other;
            triggerColliderContext.Sender = this;
            
            TriggerEnter.OnNext(triggerColliderContext);
        }
    }
}