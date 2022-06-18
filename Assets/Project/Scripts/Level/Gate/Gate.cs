using UniRx;
using UnityEngine;

namespace Project.Level.Gate
{
    public class Gate : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private GateView _gateView;
        [SerializeField] private GateCommand _gateCommand;
        public GateCommand GateCommand => _gateCommand;
        public Subject<Gate> PlayerTriggered { get; } = new Subject<Gate>();

        public void Init()
        {
            _gateView.Init(_gateCommand);
        }

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
            if (other.CompareTag("Ball"))
            {
                PlayerTriggered.OnNext(this);
            }
        }
    }
}