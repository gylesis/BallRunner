using System.Threading.Tasks;
using Project.Ball;
using Project.Utils;
using TMPro;
using UnityEngine;

namespace Project.Level
{
    public class LevelFinishService : MonoBehaviour
    {
        [SerializeField] private TMP_Text _finishSign;
        [SerializeField] private TMP_Text _tapToContinueSign;
        
        private ScreenTapWatcher _screenTapWatcher;
        private BallShooter _ballShooter;

        public void Init(ScreenTapWatcher screenTapWatcher, BallShooter ballShooter)
        {
            _ballShooter = ballShooter;
            _screenTapWatcher = screenTapWatcher;
        }

        public async Task Calculate(float value)
        {
            _finishSign.enabled = true;
            _finishSign.text = $"You knocked {value * Mathf.Rad2Deg / 180}!"; // magic number

            _tapToContinueSign.enabled = true;
            
            await _screenTapWatcher.CheckForTap((() =>
            {
                _ballShooter.Stop();
                _finishSign.enabled = false;
                _tapToContinueSign.enabled = false;
            }));

            Debug.Log("second tap");
        }
    }
}