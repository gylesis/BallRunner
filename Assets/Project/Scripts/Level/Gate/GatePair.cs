using UniRx;
using UnityEngine;

namespace Project.Level.Gate
{
    public class GatePair : MonoBehaviour
    {
        [SerializeField] private Gate _gate1;
        [SerializeField] private Gate _gate2;

        public Subject<GateTriggerContext> Triggered { get; } = new Subject<GateTriggerContext>();

        public void Init()
        {
            _gate1.Init();
            _gate2.Init();
            
            _gate1.PlayerTriggered.TakeUntilDestroy(this).Subscribe((OnGateTriggered));
            _gate2.PlayerTriggered.TakeUntilDestroy(this).Subscribe((OnGateTriggered));
        }

        private void OnGateTriggered(Gate gate)
        {
            var gateTriggerContext = new GateTriggerContext();
            
            gateTriggerContext.GatePair = this;
            gateTriggerContext.TriggeredGateCommand = gate.GateCommand;
            
            Triggered.OnNext(gateTriggerContext);
        }

        public void DisableGates()
        {
            _gate1.Disable();
            _gate2.Disable();
        }
    }
    
    public struct GateTriggerContext
    {
        public GatePair GatePair;
        public GateCommand TriggeredGateCommand;
    }

}
