using Project.Ball;
using UniRx;
using UnityEngine;

namespace Project.Level.Gate
{
    public class GatesService : MonoBehaviour
    {
        [SerializeField] private GatePair[] _gates;
        
        private BallsPlaceService _ballsPlaceService;

        public void Init(BallsPlaceService ballsPlaceService)
        {
            _ballsPlaceService = ballsPlaceService;
        }

        private void Start()
        {
            foreach (GatePair gate in _gates)
            {
                gate.Init();
                gate.Triggered.TakeUntilDestroy(this).Subscribe((OnGateTriggered));
            }
        }

        private void OnGateTriggered(GateTriggerContext context)
        {
            context.GatePair.DisableGates();
            
            GateCommand gateCommand = context.TriggeredGateCommand;
            _ballsPlaceService.ManageBallsGateCommand(gateCommand);
        }
    }
}